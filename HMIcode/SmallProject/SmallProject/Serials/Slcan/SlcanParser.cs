using SmallProject.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallProject.Serials.Slcan
{
    public static class SlcanParser
    {
        public static CanFrame ParseSlcanFrame(string frame)
        {
            try
            {
                char cmd = frame[0];
                var canFrame = new CanFrame();

                switch (cmd)
                {
                    case 't': // 标准帧
                        canFrame.IsExtended = false;
                        canFrame.IsRtr = false;
                        break;
                    case 'T': // 扩展帧
                        canFrame.IsExtended = true;
                        canFrame.IsRtr = false;
                        break;
                    case 'r': // 远程标准帧
                        canFrame.IsExtended = false;
                        canFrame.IsRtr = true;
                        break;
                    case 'R': // 远程扩展帧
                        canFrame.IsExtended = true;
                        canFrame.IsRtr = true;
                        break;
                    default:
                        throw new ArgumentException("Unsupported frame type.");
                }


                // 解析 ID 和 DLC
                var IdCmd = frame.Substring(1, 3);
                canFrame.Id = Convert.ToInt32(IdCmd, 16) >> 7;
                canFrame.Cmd = Convert.ToInt32(IdCmd, 16) & 0x7F;
                canFrame.Dlc = byte.Parse(frame.Substring(4, 1));

                if (canFrame.Dlc>0)
                {
                    canFrame.Data = new byte[canFrame.Dlc];
                    for (int i = 0; i < canFrame.Dlc; i++)
                    {
                        var oneByte = byte.Parse(frame.Substring(5 + i * 2, 2), System.Globalization.NumberStyles.HexNumber);
                        canFrame.Data[i] = oneByte;
                    }
                }
                else
                {
                    canFrame.Data = Array.Empty<byte>();
                }

                return canFrame;
            }
            catch (Exception e)
            {
                JLog.Error(e);
                return null;
            }

            
        }

        public static CanFrame ParseSlcanFrame(int Id, int Cmd, float value, bool resACK = true)
        {
            return ParseSlcanFrame(Id, Cmd, value,0, resACK);
        }

        public static CanFrame ParseSlcanFrame(int Id, int Cmd, float value1, float value2, bool resACK = true)
        {
            CanFrame canFrame = new CanFrame();
            canFrame.Id = Id;
            canFrame.Cmd = Cmd;
            canFrame.Dlc = 8;
            canFrame.Data = new byte[8];
            byte[] bytes1 = BitConverter.GetBytes(value1);
            for (int i = 0; i < bytes1.Length; i++)
            {
                canFrame.Data[i] = bytes1[i];
            }
            byte[] bytes2 = BitConverter.GetBytes(value2);
            for (int i = 0; i < bytes2.Length; i++)
            {
                canFrame.Data[i + 4] = bytes2[i];
            }
            return canFrame;
        }

        public static string ParseSlcanFrameStr(int Id, int Cmd)
        {
            return ParseSlcanFrame(Id, Cmd, 0, 0).ToStr();
        }
        public static string ParseSlcanFrameStr(int Id, int Cmd, float value,bool resACK = true)
        {
            return ParseSlcanFrame(Id, Cmd, value, resACK).ToStr();
        }
        public static string ParseSlcanFrameStr(int Id, int Cmd, float value1, float value2)
        {
            return ParseSlcanFrame(Id, Cmd, value1, value2).ToStr();
        }
    }
}
