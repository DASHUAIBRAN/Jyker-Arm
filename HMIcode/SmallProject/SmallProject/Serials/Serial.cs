using RJCP.IO.Ports;
using SharpCompress.Compressors.Xz;
using SmallProject.Devices.Arm;
using SmallProject.Logger;
using SmallProject.Serials.Slcan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmallProject.Serials
{
    public class Serial
    {

        private Queue<string> DataToSend;

        public void PushDataToQueue(string data)
        {
            if(!OpenResult)
            {
                JLog.Info("请先打开连接");
                return;
            }
            DataToSend.Enqueue(data);
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
                while(OpenResult)
                {
                    try
                    {
                        for (int i = 0; i < DataToSend.Count; i++)
                        {
                            if (DataToSend.TryDequeue(out string? send))
                            {
                                stream.Write(send);
                                JLog.Info(send);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        JLog.Error(e);
                    }
                    await Task.Delay(50);
                }
            });
        }

        //数据接收
        private void Stream_DataReceived(object? sender, SerialDataReceivedEventArgs e)
        {
            var sp = sender as SerialPortStream;
            var recieveText = sp.ReadExisting();
            var frames =recieveText.Split('\r');
            
            foreach(var frame in frames)
            {
                if (string.IsNullOrEmpty(frame)) continue;
                var aFrame = SlcanParser.ParseSlcanFrame(frame);
                if(aFrame!=null)
                {
                    switch(aFrame.Cmd) {
                        case 0x21:
                            //电流信息
                            var current = BitConverter.ToSingle(aFrame.Data.Take(4).ToArray());
                            App.Core.Jyker.RecieveCurrent(aFrame.Id, current);
                            JLog.Info($"电流 {current}");
                            break;
                        case 0x22:
                            // 速度信息
                            var velocity = BitConverter.ToSingle(aFrame.Data.Take(4).ToArray());
                            App.Core.Jyker.RecieveVelocity(aFrame.Id, velocity);
                            break;
                        case 0x23:
                            //角度信息
                            var angle = BitConverter.ToSingle(aFrame.Data.Take(4).ToArray());
                            var isFinish = BitConverter.ToBoolean(aFrame.Data.Skip(4).Take(1).ToArray());
                            App.Core.Jyker.RecievePos(aFrame.Id, angle, isFinish);
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
