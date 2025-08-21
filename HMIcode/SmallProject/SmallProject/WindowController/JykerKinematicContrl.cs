using Microsoft.VisualBasic.Logging;
using SmallProject.Devices.Arm.Kinematic.Models;
using SmallProject.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SmallProject.WindowController
{
    //正解逆解坐标实时解算
    class JykerKinematicContrl
    {
        private MainWindow M;

        public JykerKinematicContrl(MainWindow m)
        {
            M = m;

            //初始化值
            Init();

            //绑定事件
            m.tb_X.TextChanged += Tb_IK;
            m.tb_Y.TextChanged += Tb_IK;
            m.tb_Z.TextChanged += Tb_IK;
            m.tb_A.TextChanged += Tb_IK;
            m.tb_B.TextChanged += Tb_IK;
            m.tb_C.TextChanged += Tb_IK;

            m.tb_Joint1.TextChanged += Tb_FK;
            m.tb_Joint2.TextChanged += Tb_FK;
            m.tb_Joint3.TextChanged += Tb_FK;
            m.tb_Joint4.TextChanged += Tb_FK;
            m.tb_Joint5.TextChanged += Tb_FK;
            m.tb_Joint6.TextChanged += Tb_FK;
        }

        private void Init()
        {
            var joint = App.Core.Jyker.prepareJoints;
            M.tb_Joint1.Text = Math.Round(joint.a[0], 2) + "";
            M.tb_Joint2.Text = Math.Round(joint.a[1], 2) + "";
            M.tb_Joint3.Text = Math.Round(joint.a[2], 2) + "";
            M.tb_Joint4.Text = Math.Round(joint.a[3], 2) + "";
            M.tb_Joint5.Text = Math.Round(joint.a[4], 2) + "";
            M.tb_Joint6.Text = Math.Round(joint.a[5], 2) + "";

            var pose6 = App.Core.Jyker.preparePose6D;
            App.Core.Jyker.dof6Solver.SolveFK(App.Core.Jyker.prepareJoints, App.Core.Jyker.preparePose6D);         
            M.tb_X.Text = Math.Round(pose6.X, 2) + "";
            M.tb_Y.Text = Math.Round(pose6.Y, 2) + "";
            M.tb_Z.Text = Math.Round(pose6.Z, 2) + "";
            M.tb_A.Text = Math.Round(pose6.A, 2) + "";
            M.tb_B.Text = Math.Round(pose6.B, 2) + "";
            M.tb_C.Text = Math.Round(pose6.C, 2) + "";
        }

        //正解
        private void Tb_FK(object sender, TextChangedEventArgs e)
        {
            double.TryParse(M.tb_Joint1.Text, out double a1);
            double.TryParse(M.tb_Joint2.Text, out double a2);
            double.TryParse(M.tb_Joint3.Text, out double a3);
            double.TryParse(M.tb_Joint4.Text, out double a4);
            double.TryParse(M.tb_Joint5.Text, out double a5);
            double.TryParse(M.tb_Joint6.Text, out double a6);

            App.Core.Jyker.prepareJoints = new Joint6D_t(a1, a2, a3, a4, a5, a6);
            App.Core.Jyker.dof6Solver.SolveFK(App.Core.Jyker.prepareJoints, App.Core.Jyker.preparePose6D);



            //更新正解位姿
            var pose6 = App.Core.Jyker.preparePose6D;
            M.tb_X.TextChanged -= Tb_IK;
            M.tb_Y.TextChanged -= Tb_IK;
            M.tb_Z.TextChanged -= Tb_IK;
            M.tb_A.TextChanged -= Tb_IK;
            M.tb_B.TextChanged -= Tb_IK;
            M.tb_C.TextChanged -= Tb_IK;
            M.tb_X.Text = Math.Round(pose6.X, 2) + "";
            M.tb_Y.Text = Math.Round(pose6.Y, 2) + "";
            M.tb_Z.Text = Math.Round(pose6.Z, 2) + "";
            M.tb_A.Text = Math.Round(pose6.A, 2) + "";
            M.tb_B.Text = Math.Round(pose6.B, 2) + "";
            M.tb_C.Text = Math.Round(pose6.C, 2) + "";
            M.tb_X.TextChanged += Tb_IK;
            M.tb_Y.TextChanged += Tb_IK;
            M.tb_Z.TextChanged += Tb_IK;
            M.tb_A.TextChanged += Tb_IK;
            M.tb_B.TextChanged += Tb_IK;
            M.tb_C.TextChanged += Tb_IK;
        }

        //逆解
        private void Tb_IK(object sender, TextChangedEventArgs e)
        {
            double.TryParse(M.tb_X.Text, out double x);
            double.TryParse(M.tb_Y.Text, out double y);
            double.TryParse(M.tb_Z.Text, out double z);
            double.TryParse(M.tb_A.Text, out double a);
            double.TryParse(M.tb_B.Text, out double b);
            double.TryParse(M.tb_C.Text, out double c);

            var res = App.Core.Jyker.SolveIK(new double[] { x, y, z, a, b, c });
            if (!res)
            {
                JLog.Info("逆解无解");
            }
            var joint = App.Core.Jyker.prepareJoints;


            M.tb_Joint1.TextChanged -= Tb_FK;
            M.tb_Joint2.TextChanged -= Tb_FK;
            M.tb_Joint3.TextChanged -= Tb_FK;
            M.tb_Joint4.TextChanged -= Tb_FK;
            M.tb_Joint5.TextChanged -= Tb_FK;
            M.tb_Joint6.TextChanged -= Tb_FK;
            M.tb_Joint1.Text = Math.Round(joint.a[0], 2) + "";
            M.tb_Joint2.Text = Math.Round(joint.a[1], 2) + "";
            M.tb_Joint3.Text = Math.Round(joint.a[2], 2) + "";
            M.tb_Joint4.Text = Math.Round(joint.a[3], 2) + "";
            M.tb_Joint5.Text = Math.Round(joint.a[4], 2) + "";
            M.tb_Joint6.Text = Math.Round(joint.a[5], 2) + "";
            M.tb_Joint1.TextChanged += Tb_FK;
            M.tb_Joint2.TextChanged += Tb_FK;
            M.tb_Joint3.TextChanged += Tb_FK;
            M.tb_Joint4.TextChanged += Tb_FK;
            M.tb_Joint5.TextChanged += Tb_FK;
            M.tb_Joint6.TextChanged += Tb_FK;
        }
    }
}
