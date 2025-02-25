using BigProject.Devices.Arm.Kinematic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigProject.Devices.Arm.CtrlStep
{
    /// <summary>
    /// 已包含减速器的步进电机
    /// </summary>
    public class CtrlStepMotor
    {
        /// <summary>
        /// 回零之后偏差角度
        /// </summary>
        public double OffsetAngle { get; set; } = 0;
        /// <summary>
        /// 最大扭转角
        /// </summary>
        public double AngleLimitMax { get; set; } = 180;
        /// <summary>
        /// 最小扭转角
        /// </summary>
        public double AngleLimitMin { get; set; } = -0.01;

        /// <summary>
        /// 加速度
        /// </summary>
        public double Acceleration { get; set; } = 0;
        /// <summary>
        /// 速度
        /// </summary>
        public double Speed { get; set; } = 50;
        /// <summary>
        /// 当前角度
        /// </summary>
        public double CurrentAngle { get; set; } = 0;
        /// <summary>
        /// 减速比
        /// </summary>
        public double Reduction { get; set; } = 30;
        /// <summary>
        /// 电机方向
        /// </summary>
        public int Direction { get; set; } = 0;
        /// <summary>
        /// 3200个脉冲转一圈
        /// </summary>
        public int OneDegPause { get; set; } = 3200;

    }
}
