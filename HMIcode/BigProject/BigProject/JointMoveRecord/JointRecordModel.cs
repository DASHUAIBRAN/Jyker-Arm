using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigProject.JointMoveRecord
{
    public class JointRecordModel
    {
        //是否判断抓取
        public bool IsAjust { get; set; }
        public double J1 { get; set; }
        public double J2 { get; set; }
        public double J3 { get; set; }
        public double J4 { get; set; }
        public double J5 {  get; set; }
        public double J6 { get; set; }


        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }

        public double ClawAngle { get; set; }
        public DateTime AddTime { get; set; }   
    }
}
