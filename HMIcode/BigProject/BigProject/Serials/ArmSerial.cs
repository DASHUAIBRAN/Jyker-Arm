using BigProject.Config;
using BigProject.Devices.Arm;
using BigProject.Logger;
using Castle.DynamicProxy;
using Masuit.Tools;
using RJCP.IO.Ports;
using System;
using System.Collections.Generic;
using System.IO;
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
        //串口工具
        private SerialPortStream stream;
        public ArmSerial(string name1,out bool OpenResult)
        {
            try
            {
                if (stream!=null&&stream.IsOpen)
                {
                    stream.Dispose();
                }
                stream = new SerialPortStream(name1, 115200, 8, RJCP.IO.Ports.Parity.None, RJCP.IO.Ports.StopBits.One);
                stream.ReadTimeout = 1000;
                stream.Open();
                OpenResult = true;
            }
            catch (Exception)
            {
                OpenResult = false;
            }


        }

        public bool SerialDispose()
        {
            stream.Close();
            return true;
        }
        public bool SendMsgForResult(byte[] msg  ,out byte[] recMsg)
        {
            recMsg = new byte[msg.Length];
            try
            {
                stream.Write(msg, 0, msg.Length);
                byte[] buffer = new byte[1024];
                int bytesRead;
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                if(bytesRead==0)
                {
                    return false;
                }
                recMsg = buffer.Take(bytesRead).ToArray();
                //string z = "";
                //for (int i = 0; i < recMsg.Length; i++)
                //{
                //    z += recMsg[i].ToString("x2") + " ";
                //}
                //Log.Info($"数据接收z_{z}");
                return true;
            }
            catch (Exception e)
            {
                Log.Info(e.Message);
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
            var re =ReBuildData(bytes.ToArray());
            //发送命令
            SendMsgForResult(re,out byte[] resMsg);
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
            var re = ReBuildData(bytes.ToArray());
            //发送命令
            SendMsgForResult(re, out byte[] resMsg);

        }

        /// <summary>
        /// 触发所有电机按照命令转动
        /// </summary>
        public void CallMotion()
        {
            byte[] bytes = new byte[3] { 0x00, 0xFF, 0x66 };
            var re = ReBuildData(bytes.ToArray());
            //发送命令
            SendMsgForResult(re, out byte[] resMsg);
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

        //根据选择的验证方式重新整理发送内容
        public byte[] ReBuildData(byte[] input)
        {
            List<byte> bytes = new List<byte>();
            switch (App.Core.ArmConfig.checkFunction)
            {
                case Config.CheckFunction.MODBUS:
                    bytes.AddRange(input);
                    bytes.AddRange(CRC16(input));
                    return bytes.ToArray();
                case Config.CheckFunction._0X6B:
                    bytes.AddRange(input);
                    bytes.Add(0x6B);
                    return bytes.ToArray();
                default:
                    break;
            }
            return input;


        }

        #region CRC 校验运算
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

        List<byte[]> ParseMixedModbusRtu(byte[] data)
        {
            List<byte[]> messages = new List<byte[]>();
            int index = 0;

            while (index < data.Length)
            {
                if (data.Length - index < 4) break; // At least need address, function code, and CRC

                // Find the end of message by searching for a valid CRC
                bool foundValidMessage = false;
                for (int i = index + 4; i <= data.Length; i += 1) // Start from index+4 to ensure at least one byte for data
                {
                    byte[] potentialMessage = new byte[i - index];
                    Array.Copy(data, index, potentialMessage, 0, i - index);

                    ushort crcReceived = (ushort)((potentialMessage[potentialMessage.Length - 2] ) | potentialMessage[potentialMessage.Length - 1] << 8);
                    ushort crcCalculated = CalculateCRC16(potentialMessage, 0, potentialMessage.Length - 2);

                    if (crcReceived == crcCalculated&&crcReceived!=0)
                    {
                        messages.Add(potentialMessage);
                        index = i;
                        foundValidMessage = true;
                        break;
                    }
                }

                if (!foundValidMessage) break; // No valid message found, exit loop
            }

            return messages;
        }

        ushort CalculateCRC16(byte[] data, int offset, int length)
        {
            ushort crc = 0xFFFF;


            for (int pos = offset; pos < offset + length; pos++)
            {
                crc = (ushort)(crc ^ (data[pos]));
                for (int j = 0; j < 8; j++)
                {
                    crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                }
            }
            return crc;
        }
        #endregion

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
