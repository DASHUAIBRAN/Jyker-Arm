using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigProject.Config
{
    public class ConfigEntity
    {
        /// <summary>
        /// J1 减速比
        /// </summary>
        public int ReductionJ1 { get; set; } = 30;
        /// <summary>
        /// J2 减速比
        /// </summary>
        public int ReductionJ2 { get; set; } = 50;
        /// <summary>
        /// J3 减速比
        /// </summary>
        public int ReductionJ3 { get; set; } = 30;
        /// <summary>
        /// J4 减速比
        /// </summary>
        public int ReductionJ4 { get; set; } = 30;
        /// <summary>
        /// J5 减速比
        /// </summary>
        public int ReductionJ5 { get; set; } = 30;
        /// <summary>
        /// J6 减速比
        /// </summary>
        public int ReductionJ6 { get; set; } = 1;

        // D_BASE = 0, L_BASE = 161.5, L_ARM = 170, D_ELBOW = 70, L_FOREARM = 117, L_WRIST = 97
        public double L_BASE { get; set; } = 161.5;
        public double D_BASE { get; set; } = 0;
        public double L_ARM { get; set; } = 170;
        public double D_ELBOW { get; set; } = 70;
        public double L_FOREARM { get; set; } = 117;
        public double L_WRIST { get; set; } = 97;

        public CheckFunction checkFunction { get; set; } = CheckFunction._0X6B;
    }

    //检验方式
    public enum CheckFunction
    {
        _0X6B,
        MODBUS
    }
}
