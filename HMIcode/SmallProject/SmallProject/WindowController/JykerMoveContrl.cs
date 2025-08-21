using SmallProject.Aliyun;
using SmallProject.MCP;
using SmallProject.Serials.Slcan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallProject.WindowController
{
    internal class JykerMoveContrl
    {
        private MainWindow M;
        private JykerControlMCP jykerControlMCP;

        public JykerMoveContrl(MainWindow m)
        {
            M = m;
            m.bt_ApplyHomePosition.Click += Bt_ApplyHomePosition_Click; ;
            m.bt_MoveJoint.Click += Bt_MoveJoint_Click;
            m.bt_StopNow.Click += Bt_StopNow_Click;
            m.bt_ConnectAi.Click += Bt_ConnectAi_Click;
        }

        //连接语音助手
        private void Bt_ConnectAi_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //AiAssistant aiAssistant = new AiAssistant();
            //aiAssistant.Init();
            Task.Run(async () =>
            {
                jykerControlMCP = new JykerControlMCP();
                await jykerControlMCP.Init();
            });
            
        }

        //设置立刻停止
        private void Bt_StopNow_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.Core.Jyker.StopNow();
        }

        //设置当前位置为0位
        private void Bt_ApplyHomePosition_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.Core.Jyker.ApplyHomePosition();
        }


        //运动机械臂
        private void Bt_MoveJoint_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            double.TryParse(M.tb_Joint1.Text, out double a1);
            double.TryParse(M.tb_Joint2.Text, out double a2);
            double.TryParse(M.tb_Joint3.Text, out double a3);
            double.TryParse(M.tb_Joint4.Text, out double a4);
            double.TryParse(M.tb_Joint5.Text, out double a5);
            double.TryParse(M.tb_Joint6.Text, out double a6);

            App.Core.Jyker.Move(new double[6] { a1, a2, a3, a4, a5, a6 });
        }


    }
}
