using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigProject.Devices.Arm.Kinematic.Models
{
    public class Joint6D_t
    {
        public Joint6D_t(double a1, double a2, double a3, double a4, double a5, double a6)
        {
            a = new double[] { a1, a2, a3, a4, a5, a6 };
        }


        public Joint6D_t() { }

        public double[] a = new double[6];

        public static Joint6D_t operator -(Joint6D_t _joints1, Joint6D_t _joints2)
        {
            Joint6D_t tmp = new Joint6D_t();
            for (int i = 0; i < 6; i++)
                tmp.a[i] = _joints1.a[i] - _joints2.a[i];
            return tmp;
        }

        public static Joint6D_t defult = new Joint6D_t(0, -90, 180, 0, 0, 0);
    }
}
