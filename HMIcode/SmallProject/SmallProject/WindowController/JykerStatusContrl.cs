using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SmallProject.WindowController
{
    public class JykerStatusContrl
    {
        private MainWindow M;

        public JykerStatusContrl(MainWindow m)
        {
            M = m;
            m.bt_ArmLoopStart.Click += Bt_ArmLoopStart_Click;
            m.bt_ArmLoopEnd.Click += Bt_ArmLoopEnd_Click;
        }

       

        //监听机械臂各个轴的状态
        private void Bt_ArmLoopStart_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.Core.Loop += LoopStatus;
            App.Core.Loop += App.Core.Jyker.LoopStatus;
            M.bt_ArmLoopStart.IsEnabled = false;
            M.bt_ArmLoopEnd.IsEnabled = true;
        }
        //结束监听机械臂各个轴的状态
        private void Bt_ArmLoopEnd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.Core.Loop -= LoopStatus;
            App.Core.Loop -= App.Core.Jyker.LoopStatus;
            M.bt_ArmLoopStart.IsEnabled = true;
            M.bt_ArmLoopEnd.IsEnabled = false;
        }

        //循环获取电机信息
        public void LoopStatus()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    M.Dispatcher.Invoke(() =>
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            var pb = M.FindName($"pb_pJ{i + 1}") as ProgressBar;
                            var tb = M.FindName($"tb_pJ{i + 1}") as TextBlock;
                            if(pb!=null&&tb!=null)
                            {
                                pb.Value = App.Core.Jyker.motorJ[i].Current*1000;
                                tb.Text = App.Core.Jyker.motorJ[i].Current*1000 + "";
                            }
                        }
                        
                    });
                    Thread.Sleep(50);
                }
            });
        }
    }
}
