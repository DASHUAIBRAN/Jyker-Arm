using MotorControl.Logger;
using MotorControl.Serials.Slcan;
using RJCP.IO.Ports;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MotorControl.Serials
{
    internal enum InfoType
    {
        Current,
        Velocity,
        Position,
        CurrentLimint
    }

    internal class Serial
    {
        private Queue<string> DataToSend;
        public event Action<int, float, InfoType> GetResInfo;

        public void PushDataToQueue(string data)
        {
            if (!OpenResult)
            {
                JLog.Info("请先打开连接");
                return;
            }
            DataToSend.Enqueue(data);
            //JLog.Info($"加入队列:{data}");
        }

        private bool OpenResult;

        //串口工具
        private static SerialPortStream stream;
        public bool Open(string name)
        {
            try
            {
                if (stream != null && stream.IsOpen)
                {
                    stream.Dispose();
                }
                stream = new SerialPortStream(name, 115200, 8, RJCP.IO.Ports.Parity.None, RJCP.IO.Ports.StopBits.One);
                stream.ReadTimeout = 1000;

                stream.DataReceived += Stream_DataReceived;
                stream.Open();
                OpenResult = true;
                DataToSend = new Queue<string>();
                LoopSend();
                //设置波特率
                PushDataToQueue("S8\r");
                //开始
                PushDataToQueue("O\r");
            }
            catch (Exception)
            {
                OpenResult = false;

            }
            return OpenResult;
        }

        //循环发送数据
        private void LoopSend()
        {
            Task.Run(async () =>
            {
                while (OpenResult)
                {
                    try
                    {
                        if(DataToSend.Count==0)
                        {
                            Thread.Sleep(5);
                            continue;
                        }

                        string send = DataToSend.Dequeue();
                        if (!string.IsNullOrEmpty(send))
                        {
                            stream.Write(send);
                            //JLog.Info(send);
                        }
                    }
                    catch (Exception e)
                    {
                        JLog.Error(e);
                    }
                }
            });
        }

        //数据接收
        private void Stream_DataReceived(object sender, RJCP.IO.Ports.SerialDataReceivedEventArgs e)
        {
            var sp = sender as SerialPortStream;
            var recieveText = sp.ReadExisting();
            var frames = recieveText.Split('\r');

            foreach (var frame in frames)
            {
                if (string.IsNullOrEmpty(frame)) continue;
                var aFrame = SlcanParser.ParseSlcanFrame(frame);
                if (aFrame != null)
                {
                    switch (aFrame.Cmd)
                    {
                        case 0x21:
                            //电流信息
                            var current = BitConverter.ToSingle(aFrame.Data.Take(4).ToArray(),0);
                            GetResInfo?.Invoke(aFrame.Id, current, InfoType.Current);
                            JLog.Info($"电流 {current}");
                            break;
                        case 0x22:
                            // 速度信息
                            var velocity = BitConverter.ToSingle(aFrame.Data.Take(4).ToArray(), 0);
                            GetResInfo?.Invoke(aFrame.Id, velocity, InfoType.Velocity);
                            break;
                        case 0x23:
                            //角度信息
                            var angle = BitConverter.ToSingle(aFrame.Data.Take(4).ToArray(), 0);
                            GetResInfo?.Invoke(aFrame.Id, angle, InfoType.Position);
                            break;
                        case 0x26:
                            //堵转电流
                            var currentLimit = BitConverter.ToSingle(aFrame.Data.Take(4).ToArray(), 0);
                            GetResInfo?.Invoke(aFrame.Id, currentLimit, InfoType.CurrentLimint);
                            break;

                    }
                }
            }
        }

        public bool SerialDispose()
        {
            //关闭CAN 传输
            stream.Write("C/r");
            //关闭串口
            stream.Close();
            return true;
        }
    }
}
