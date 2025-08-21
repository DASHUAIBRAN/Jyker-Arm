using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallProject.Devices.Arm.CtrlStep
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
        /// 当前电流（比例得出力矩）
        /// </summary>
        public float Current { get; set; }
        /// <summary>
        /// 速度
        /// </summary>
        public double Velocity { get; set; } = 50;
        /// <summary>
        /// 当前角度
        /// </summary>
        public double Angle { get; set; } = 0;
        /// <summary>
        /// 减速比
        /// </summary>
        public double Reduction { get; set; } = 30;
        /// <summary>
        /// 电机方向
        /// </summary>
        public int Direction { get; set; } = 1;
        /// <summary>
        /// 是否执行完命令
        /// </summary>
        public bool IsFinish { get; set; } = true;

    }
}
