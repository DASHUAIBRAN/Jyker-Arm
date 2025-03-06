using BigProject.Devices.Arm;
using BigProject.Logger;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace BigProject.Serials
{
    public class ArmSerial
    {
        private int LightBrightness = 0;
        //返回的字节个数
        public int recCount = 8;
        public List<byte> DataReceived = new List<byte>();
        private System.IO.Ports.SerialPort serialPort1 = new System.IO.Ports.SerialPort();
        public ArmSerial(string name1,out bool OpenResult)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    serialPort1.BaudRate = 115200;
                    serialPort1.PortName = name1;
                    serialPort1.Open();
                    serialPort1.ReadTimeout = 500; // 设置读取超时时间（毫秒）
                    serialPort1.WriteTimeout = 500; // 设置写入超时时间（毫秒）
                    //serialPort1.DataReceived += SerialPort1_DataReceived;

                }
                OpenResult = true;
            }
            catch (Exception)
            {
                OpenResult = false;
            }


        }

        public bool SerialDispose()
        {
            serialPort1.Close();
            return true;
        }

        //开启机械臂灯光
        public void LedOpen()
        {
            LightBrightness = 10;
            SendMsgForResult(new byte[] { 0xFF, 0xFF, 0, 0, 0, 10,0x6B }, out byte[] msg);
        }

        //关闭机械臂灯光
        public void LedClose()
        {
            LightBrightness = 0;
            SendMsgForResult(new byte[] { 0xFF, 0xFF, 0, 0, 0, 0, 0x6B }, out byte[] msg);
        }

        public void CtrClaw(int angleA, int angleB,int angleC)
        {
            SendMsgForResult(new byte[] { 0xFF, 0xFF, (byte)angleA, (byte)angleB, (byte)angleC, (byte)LightBrightness, 0x6B },out byte[] msg);
        }

        //接收byte数据
        private void SerialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            byte[] rec = new byte[1024];
            int lenth = sp.Read(rec, 0, 1024);
            if (lenth>100)
            {
                return;
            }
            DataReceived.AddRange(rec);
        }

        //发送数据
        public void SendMsgOnly(byte[] msg)
        {
            DataReceived.Clear();
            string x = "";
            for (int i = 0;i < msg.Length;i++)
            {
                x+= msg[i].ToString("x2")+" ";
            }
            Log.Info($"数据发送_{x}");
            if (serialPort1.IsOpen)
                serialPort1.Write(msg, 0, msg.Length);
        }

        public bool SendMsgForResult(byte[] msg  ,out byte[] recMsg, int readLenth=128)
        {
            recMsg = new byte[readLenth];
            try
            {
                string x = "";
                for (int i = 0; i < msg.Length; i++)
                {
                    x += msg[i].ToString("x2") + " ";
                }
                //Log.Info($"数据发送_{x}");
                if (!serialPort1.IsOpen)
                {
                    return false;

                }
                serialPort1.Write(msg, 0, msg.Length);
                Thread.Sleep(20);
                serialPort1.Read(recMsg, 0, recMsg.Length);
                Thread.Sleep(20);
                string y = "";
                for (int i = 0; i < recMsg.Length; i++)
                {
                    y += recMsg[i].ToString("x2") + " ";
                }
                //Log.Info($"数据返回_{y}");
                return true;
            }
            catch (System.TimeoutException)
            {
                return false;
            }
            
        }

        //多圈堵转回零
        public bool Zero(int addr = 1)
        {
            //01 9A 00 00 6B
            List<byte> bytes = new List<byte>();
            //从机地址
            bytes.Add((byte)addr);
            //其他默认字节
            bytes.AddRange(new byte[] { 0x9A, 0x02, 0x00, 0x6B });
            //发送命令
            SendMsgForResult(bytes.ToArray(), out byte[] resMsg);

            return true;
        }

        //单圈回零
        public bool ZeroOne(int addr = 6)
        {
            //01 93 88 01 6B
            List<byte> bytes = new List<byte>();
            //从机地址
            bytes.Add((byte)addr);
            //其他默认字节
            bytes.AddRange(new byte[] { 0x93, 0x88, 0x01, 0x6B });
            //发送命令
            SendMsgForResult(bytes.ToArray(), out byte[] resMsg);

            return true;
        }

        //修改回零参数
        public bool EditZeroParams(int addr = 1, ZeroType zeroType = ZeroType.Senless
            , int direction = 1, int speed = 30, int overTime = 10000
            , int SenlessZeroSpeed = 300, int SenlessZeroCurrent = 800
            , int SenlessZeroOverTime = 60, int AutoZero = 0)
        {
            List<byte> bytes = new List<byte>();
            //从机地址
            bytes.Add((byte)addr);
            //其他默认字节
            bytes.AddRange(new byte[] { 0x4C, 0xAE, 0x01 });
            //回零模式
            bytes.AddRange(new byte[] { (byte)zeroType });
            //回零方向
            bytes.AddRange(new byte[] { (byte)direction });
            //回零速度
            bytes.AddRange(ConvertToByteArr(speed, 2));
            //回零超时时间
            bytes.AddRange(ConvertToByteArr(overTime, 4));
            //无限位碰撞回零检测转速
            bytes.AddRange(ConvertToByteArr(SenlessZeroSpeed, 2));
            //无限位碰撞回零检测电流
            bytes.AddRange(ConvertToByteArr(SenlessZeroCurrent, 2));
            //无限位碰撞回零检测时间
            bytes.AddRange(ConvertToByteArr(SenlessZeroOverTime, 2));
            //上电自动触发回零
            bytes.Add((byte)AutoZero);
            //校验位
            bytes.AddRange(new byte[] { 0x6B });
            //发送命令
            SendMsgForResult(bytes.ToArray(),out byte[] resMsg);
            return true;
        }


        /// <summary>
        /// 闭环位置控制
        /// </summary>
        /// <param name="addr">从机地址</param>
        /// <param name="direction">方向</param>
        /// <param name="speed">速度</param>
        /// <param name="acceleration">加速度档位</param>
        /// <param name="pulse">脉冲数量</param>
        /// <param name="Relative_Absolute">相对运动或是绝对运动</param>
        /// <param name="isMultiMachine">是否多机同步</param>
        public void LocationControl(int addr = 1, int direction = 0
            , int speed = 1500, int acceleration = 0, int pulse = 32000
            , RelativeOrAbsolute Relative_Absolute = RelativeOrAbsolute.Relative
            , int isMultiMachine = 1)
        {
            List<byte> bytes = new List<byte>();
            //从机地址
            bytes.Add((byte)addr);
            //其他默认字节
            bytes.AddRange(new byte[] { 0xFD });
            //方向
            bytes.AddRange(new byte[] { (byte)direction });
            //速度
            bytes.AddRange(ConvertToByteArr(speed));
            //加速度
            bytes.AddRange(ConvertToByteArr(acceleration, 1));
            //脉冲
            bytes.AddRange(ConvertToByteArr(pulse, 4));
            //是否相对运动
            bytes.Add((byte)Relative_Absolute);
            //是否多机同步
            bytes.Add((byte)isMultiMachine);
            //校验位
            bytes.AddRange(new byte[] { 0x6B });
            //发送命令
            SendMsgForResult(bytes.ToArray(), out byte[] resMsg);

        }

        /// <summary>
        /// 触发所有电机按照命令转动
        /// </summary>
        public void CallMotion()
        {
            byte[] bytes = new byte[4] { 0x00, 0xFF, 0x66, 0x6B };
            //发送命令
            SendMsgForResult(bytes.ToArray(), out byte[] resMsg);
        }


        /// <summary>
        /// 数字转化为多数组
        /// </summary>
        /// <param name="input">输入数字</param>
        /// <param name="limitCount">限制字节数量</param>
        public List<byte> ConvertToByteArr(int input, int limitCount = 2)
        {
            List<byte> bytes = new List<byte>();
            if (limitCount == 1)
            {
                bytes.Add((byte)(input & 0xFF));
            }
            if (limitCount == 2)
            {
                bytes.Add((byte)(input >> 8 & 0xFF));
                bytes.Add((byte)(input & 0xFF));
            }
            if (limitCount == 4)
            {
                bytes.Add((byte)(input >> 16 >> 8 & 0xFF));
                bytes.Add((byte)(input >> 16 & 0xFF));
                bytes.Add((byte)(input >> 8 & 0xFF));
                bytes.Add((byte)(input & 0xFF));
            }

            return bytes;
        }

        //CRC16
        public byte[] CRC16(byte[] data)
        {
            int len = data.Length;
            if (len > 0)
            {
                ushort crc = 0xFFFF;

                for (int i = 0; i < len; i++)
                {
                    crc = (ushort)(crc ^ (data[i]));
                    for (int j = 0; j < 8; j++)
                    {
                        crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                    }
                }
                byte hi = (byte)((crc & 0xFF00) >> 8);  //高位置
                byte lo = (byte)(crc & 0x00FF);         //低位置

                return new byte[] { lo, hi };
            }
            return new byte[] { 0, 0 };
        }
    }

    //运动模式
    public enum RelativeOrAbsolute
    {
        Relative,
        Absolute
    }

    //回零模式
    public enum ZeroType
    {
        Neares,//单圈就近回零
        Dir,//方向回零
        Senless,//无限位回零
        EndStop//限位回零
    }
}
