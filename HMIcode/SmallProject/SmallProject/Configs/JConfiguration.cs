using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallProject.Configs
{
    public class JConfiguration
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
        public float L_BASE { get; set; } = 0;
        public float D_BASE { get; set; } = 161.5f;
        public float L_ARM { get; set; } = 170;
        public float D_ELBOW { get; set; } = 70;
        public float L_FOREARM { get; set; } = 117;
        public float L_WRIST { get; set; } = 97;

        //机械臂速度限制 单位 raw/s
        public float Speed { get; set; } = 0.2f;
        //最大电流值 单位（A）
        public float Current { get; set; } = 1f;

        //小智连接的端点
        public string EndpointUrl { get; set; } = "wss://api.xiaozhi.me/mcp/?token=eyJhbGciOiJFUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOjE5NDgyOCwiYWdlbnRJZCI6MTE1MTY5LCJlbmRwb2ludElkIjoiYWdlbnRfMTE1MTY5IiwicHVycG9zZSI6Im1jcC1lbmRwb2ludCIsImlhdCI6MTc1MjgwMTE1NX0.iNZDJOTaAz1pzpvTxnsX42mixld2zbmyAiOvof4q0lv_3tMGXoVa8YK7lCO4qxWc3n1H4yd-dAOg95cW2kjkSQ";

        public string FindModelPath { get; set; } = "DETECT_IMAGE";
    }
}
