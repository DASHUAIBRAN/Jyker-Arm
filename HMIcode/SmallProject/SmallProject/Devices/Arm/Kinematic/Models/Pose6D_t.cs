using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallProject.Devices.Arm.Kinematic.Models
{
    public class Pose6D_t
    {

        public Pose6D_t()
        {

        }

        public Pose6D_t(double x, double y, double z, double a, double b, double c)
        {
            X = x;
            Y = y;
            Z = z;
            A = a;
            B = b;
            C = c;
        }

        public Pose6D_t(double[] vals)
        {
            X = vals[0];
            Y = vals[1];
            Z = vals[2];
            A = vals[3];
            B = vals[4];
            C = vals[5];
        }
        //x 坐标
        public double X { get; set; }
        //y 坐标
        public double Y { get; set; }
        //z 坐标
        public double Z { get; set; }
        //角度 a
        public double A { get; set; }
        //角度 b
        public double B { get; set; }
        //角度 c
        public double C { get; set; }
        //变换矩阵
        public double[] R { get; set; }
        //是否存在变换矩阵
        public bool hasR { get; set; } = false;
    }
}
