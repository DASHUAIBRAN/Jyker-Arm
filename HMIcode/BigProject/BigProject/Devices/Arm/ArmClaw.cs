using BigProject.Logger;
using BigProject.Serials;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BigProject.Devices.Arm
{
    public class ArmClaw:BaseDevice
    {
        //角度转换成脉冲 3200 为 360°
        public const double DEG_TO_PULSE = 8.888888889;
        public const double CLAW_FINGER_LENTH = 45;
        public const double CLAW_FINGER_WIDTH = 15;
        public const double CLAW_FINGER_ANGLE = 70.53;
        private ArmSerial _armClawSerial;
        //更新信息回调
        public static event Action<double, double, double> UpdateClawMsgEvent;

        public ArmClaw(ArmSerial armClawSerial) {
            _armClawSerial = armClawSerial;
        }


        //夹爪回零
        public void Home()
        {
            _armClawSerial.EditZeroParams(addr: 7, direction: 1);
            _armClawSerial.Zero(7);
        }

        //夹爪停止
        public void Stop()
        {
            byte[] send = new byte[3] { 0x07, 0x0E, 0x52 };
            send = _armClawSerial.ReBuildData(send);
            _armClawSerial.SendMsgForResult(send, out byte[] resMsg);
        }

        //设置夹爪角度
        public void SetAngle(double Angle,double Speed = 5000,int Reduction = 9)
        {
            _armClawSerial.LocationControl(7, 0, (int)Speed, (int)0, (int)(Angle* DEG_TO_PULSE* Reduction), RelativeOrAbsolute.Absolute,isMultiMachine:0);    
        }

        public void ReadAngle()
        {
            try
            {
                //读取状态信息 07 43 7A 6B
                byte[] send = new byte[3] { 0x07, 0x43, 0x7A };
                send = _armClawSerial.ReBuildData(send);
                var result = _armClawSerial.SendMsgForResult(send, out byte[] resMsg);
                if (!result)
                {
                    return;
                }
                if (resMsg[1] == 0x00 && resMsg[2] == 0xEE)
                {
                    return;
                }
                if (resMsg[0] != 0x07)
                {
                    return;
                }

                //实时角度
                var temp = (resMsg[19] * 256 * 256 * 256 + resMsg[20] * 256 * 256 + resMsg[21] * 256 + resMsg[22]) * 360 / 65536.0 / 9 * 2;
                var Angle = Math.Round(temp, 2);
                if (Angle <= -0.8 || Angle > 180)
                {
                    return;
                }
                //根据三角函数计算末端长度
                var Length = Math.Cos((180 - (Angle / 2) - CLAW_FINGER_ANGLE) / 180 * Math.PI) * CLAW_FINGER_LENTH + CLAW_FINGER_WIDTH;
                Length = Math.Round(Length * 2, 2);

                //根据实时相电流计算力矩大小
                var Power = (resMsg[6] * 256 + resMsg[7]) * 1.0;
                Power = Math.Round(Power, 2);
                if (Power == 0)
                {
                    return;
                }
                //回调函数，让数值显示出来
                UpdateClawMsgEvent?.Invoke(Angle, Length, Power);
                //Log.Info($"Angle:{Angle}--Length:{Length}--Power:{Power}");
            }
            catch (Exception ex)
            {

            }
            
        }
    } 
}
