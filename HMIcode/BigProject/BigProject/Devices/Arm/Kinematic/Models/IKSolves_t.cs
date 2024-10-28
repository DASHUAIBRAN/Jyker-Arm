using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigProject.Devices.Arm.Kinematic.Models
{
    public class IKSolves_t
    {
        public Joint6D_t[] config = new Joint6D_t[8] {
            new Joint6D_t(), new Joint6D_t()
            , new Joint6D_t(), new Joint6D_t()
            ,new Joint6D_t(), new Joint6D_t()
            ,new Joint6D_t(), new Joint6D_t() };
        public int[][] solFlag = new int[8][] { new int[3], new int[3], new int[3], new int[3], new int[3], new int[3], new int[3], new int[3] };
    }
}
