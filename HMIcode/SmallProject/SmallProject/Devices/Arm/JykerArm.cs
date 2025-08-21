using SmallProject.Configs;
using SmallProject.Devices.Arm.CtrlStep;
using SmallProject.Devices.Arm.Kinematic.Models;
using SmallProject.Devices.Arm.Kinematic;
using SmallProject.Logger;
using SmallProject.Serials.Slcan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using Microsoft.VisualBasic.Logging;

namespace SmallProject.Devices.Arm
{
    public class JykerArm
    {
        //角度转换成脉冲 3200 为 360°
        public const double DEG_TO_PULSE = 8.888888889;

        public Dof6kinematic dof6Solver;
        public CtrlStepMotor[] motorJ;
        public Joint6D_t currentJoints;
        public Joint6D_t prepareJoints;
        public Pose6D_t preparePose6D;

        public JykerArm() {
            dof6Solver = new Dof6kinematic(App.JConfig);
            motorJ = new CtrlStepMotor[6] {
                new CtrlStepMotor(){ AngleLimitMin =-90.1,AngleLimitMax = 90.1,Reduction = App.JConfig.ReductionJ1}
                ,new CtrlStepMotor(){ AngleLimitMin = -90.1,AngleLimitMax = 90.1 , Direction =-1, OffsetAngle = -90,Reduction = App.JConfig.ReductionJ2 }
                ,new CtrlStepMotor(){ AngleLimitMin = -0.1,AngleLimitMax= 180.1,OffsetAngle = 180 , Direction = 1,Reduction = App.JConfig.ReductionJ3}
                ,new CtrlStepMotor(){ AngleLimitMin = -0.1,AngleLimitMax = 180.1 , Direction =-1,Reduction = App.JConfig.ReductionJ4}
                ,new CtrlStepMotor(){ AngleLimitMin = -90.1 , AngleLimitMax = 90.1,Reduction = App.JConfig.ReductionJ5}
                ,new CtrlStepMotor() { AngleLimitMin = -180.1,AngleLimitMax = 180.1,Reduction = App.JConfig.ReductionJ6}
            };
            preparePose6D = new Pose6D_t();
            prepareJoints = Joint6D_t.defult;
            currentJoints = Joint6D_t.defult;
        }

        public void LoopStatus()
        {
            Task.Run(() =>
            {
                for (int i = 0; i < 6; i++)
                {
                    GetStatus(i + 1);
                }
            });
        }

        public void GetStatus(int Id)
        {
            var motor = motorJ[Id - 1];
            motor.Current = 0;
            var frame = SlcanParser.ParseSlcanFrameStr(Id, 0x21);
            App.Core?.Serial?.PushDataToQueue(frame);
        }

        //更新电流
        public void RecieveCurrent(int Id,float value)
        {
            var motor = motorJ[Id - 1];
            motor.Current = Math.Abs(value);
        }
        //更新速度
        public void RecieveVelocity(int Id, float velocity)
        {
            var motor = motorJ[Id - 1];
            motor.Velocity = Math.Abs(velocity);
        }
        //更新位置
        public void RecievePos(int Id, float value,bool isFinish)
        {
            var motor = motorJ[Id - 1];
            motor.Angle = value/ motor.Reduction;
            motor.IsFinish = isFinish;
            JLog.Info(""+motor.Angle);
        }

        //更新机械臂位置
        public void Move(double[] angles)
        {
            if (angles.Length!=6)
            {
                JLog.Info("机械臂角度参数缺失");
                return;
            }    
            for (int i = 0; i < angles.Length; i++)
            {
                if (angles[i] > motorJ[i].AngleLimitMax 
                    || angles[i] < motorJ[i].AngleLimitMin)
                {
                    JLog.Info("机械臂设置角度超限");
                    return;
                }
            }
            var speed = App.JConfig.Speed;
            if (speed <= 0)
            {
                JLog.Info("速度设置为0 ，请从新设置");
                return;
            }

            //设置新坐标值
            prepareJoints = new Joint6D_t(angles);
            dof6Solver.SolveFK(prepareJoints, preparePose6D);

            var angleList = (prepareJoints - currentJoints).a;
            var maxAngle = angleList.Max(t => Math.Abs(t));
            var time = maxAngle / speed;
            JLog.Info($"time:{time}");
            for (int i = 0; i < angles.Length; i++)
            {
                if (i == 5) time = 1;
                //if (i == 3||i==4||i==5 ) continue;
                motorJ[i].Angle = prepareJoints.a[i] - Joint6D_t.defult.a[i];
                var moveAngle =  Convert.ToSingle(motorJ[i].Angle * motorJ[i].Direction * motorJ[i].Reduction / 360);

                var frame = SlcanParser.ParseSlcanFrameStr(i + 1, 0x06
                    , moveAngle, (float)time);
                JLog.Info($"moveAngle[{i}]:{moveAngle} time: {time}");
                App.Core.Serial.PushDataToQueue(frame);

            }
            //更新位置
            currentJoints = prepareJoints;
            


        }
        //设置当前位置为零位
        public void ApplyHomePosition()
        {
            var canFrame = SlcanParser.ParseSlcanFrameStr(0, 0x15);
            App.Core.Serial?.PushDataToQueue(canFrame);
        }
        //设置立刻停止
        public void StopNow()
        {
            var canFrame = SlcanParser.ParseSlcanFrameStr(0, 0x04);
            App.Core.Serial?.PushDataToQueue(canFrame);
        }
        //生成逆解解算结果
        public bool SolveIK(double[] vals)
        {
            var preparePose6D = new Pose6D_t(vals);
            var res =dof6Solver.AfterSolveIK(preparePose6D, prepareJoints, currentJoints, motorJ, out Joint6D_t _outputJoints);
            if(res)
            {
                prepareJoints = _outputJoints;
            }
            return res;
        }
        //设置轴的最大限制电流
        public void SetLimitCurrent(int Id,float current)
        {
            if(current>3000)
            {
                JLog.Info("电流最大设置为3000mA.");
                return;
            }
            if(current<=0)
            {
                JLog.Info("电流设置值有误.");
                return;
            }
            var canFrame = SlcanParser.ParseSlcanFrameStr(Id, 0x12, current / 1000);
            App.Core.Serial?.PushDataToQueue(canFrame);
        }
    }
}
