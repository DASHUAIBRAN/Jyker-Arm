using BigProject.Devices.Arm.Kinematic.Models;
using BigProject.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BigProject
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Rubyer.RubyerWindow
    {
        WindowState ws;
        WindowState wsl;
        NotifyIcon notifyIcon;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;

            icon();

            //ContextMenuStrip
            contextMenu();

            //保证窗体显示在上方。
            wsl = WindowState;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Rubyer.ThemeManager.SwitchThemeMode(Rubyer.Enums.ThemeMode.Dark);
            this.StateChanged += MainWindow_StateChanged;
            //添加日志输出
            Log.MessageEvent += Log_MessageEvent;

            //按钮事件
            //回零
            bt_Home.Click += Bt_Home_Click;
            //立即停止
            bt_StopNow.Click += Bt_StopNow_Click;
            //正解计算
            bt_FK.Click += Bt_FK_Click;
            //逆解计算
            bt_IK.Click += Bt_IK_Click;
            //让机械臂运动
            bt_MoveJoint.Click += Bt_MoveJoint_Click;
            //获取当前位置
            bt_GetCurrentAngle.Click += Bt_GetCurrentAngle_Click;
            //赋值当前角度位置
            //MainWindow_UpdateJointAngle();
            bt_OpenClaw.Click += Bt_OpenClaw_Click;
            bt_CloseClaw.Click += Bt_CloseClaw_Click;

            bt_MoveToDesk.Click += Bt_MoveToDesk_Click;
            bt_MoveToMan.Click += Bt_MoveToMan_Click;
        }

        //运动到我这
        private void Bt_MoveToMan_Click(object sender, RoutedEventArgs e)
        {
            var a1 = -45;
            var a2 = 45;
            var a3 = 60;
            var a4 = 0;
            var a5 = 0;
            var a6 = 0;

            App.Core.armContrl.MoveJ(a1, a2, a3, a4, a5, a6);
            var pose6 = App.Core.armContrl.currentPose6D;
            var joint = App.Core.armContrl.currentJoints;

            tb_X.Text = Math.Round(pose6.X, 2) + "";
            tb_Y.Text = Math.Round(pose6.Y, 2) + "";
            tb_Z.Text = Math.Round(pose6.Z, 2) + "";
            tb_A.Text = Math.Round(pose6.A, 2) + "";
            tb_B.Text = Math.Round(pose6.B, 2) + "";
            tb_C.Text = Math.Round(pose6.C, 2) + "";

            tb_Joint1.Text = Math.Round(joint.a[0], 2) + "";
            tb_Joint2.Text = Math.Round(joint.a[1], 2) + "";
            tb_Joint3.Text = Math.Round(joint.a[2], 2) + "";
            tb_Joint4.Text = Math.Round(joint.a[3], 2) + "";
            tb_Joint5.Text = Math.Round(joint.a[4], 2) + "";
            tb_Joint6.Text = Math.Round(joint.a[5], 2) + "";

            App.Core.armContrl.MoveJoints();
        }

        //运动到桌面为止
        private void Bt_MoveToDesk_Click(object sender, RoutedEventArgs e)
        {
            var a1 = -10;
            var a2 = 18;
            var a3 = 160;
            var a4 = 0;
            var a5 = 0;
            var a6 = 0;

            App.Core.armContrl.MoveJ(a1, a2, a3, a4, a5, a6);
            var pose6 = App.Core.armContrl.currentPose6D;
            var joint = App.Core.armContrl.currentJoints;

            tb_X.Text = Math.Round(pose6.X, 2) + "";
            tb_Y.Text = Math.Round(pose6.Y, 2) + "";
            tb_Z.Text = Math.Round(pose6.Z, 2) + "";
            tb_A.Text = Math.Round(pose6.A, 2) + "";
            tb_B.Text = Math.Round(pose6.B, 2) + "";
            tb_C.Text = Math.Round(pose6.C, 2) + "";

            tb_Joint1.Text = Math.Round(joint.a[0], 2) + "";
            tb_Joint2.Text = Math.Round(joint.a[1], 2) + "";
            tb_Joint3.Text = Math.Round(joint.a[2], 2) + "";
            tb_Joint4.Text = Math.Round(joint.a[3], 2) + "";
            tb_Joint5.Text = Math.Round(joint.a[4], 2) + "";
            tb_Joint6.Text = Math.Round(joint.a[5], 2) + "";

            App.Core.armContrl.MoveJoints();
        }

        //关闭夹爪
        private void Bt_CloseClaw_Click(object sender, RoutedEventArgs e)
        {
            App.Core.armClaw.Close();
        }

        //打开夹爪
        private void Bt_OpenClaw_Click(object sender, RoutedEventArgs e)
        {
            App.Core.armClaw.Open();
        }

        private void Bt_GetCurrentAngle_Click(object sender, RoutedEventArgs e)
        {
            MainWindow_UpdateJointAngle();
        }

        //获取当前角度位置
        private void MainWindow_UpdateJointAngle()
        {
            Task.Run(() =>
            {
                this.Dispatcher.Invoke(() => {
                    var motorJ = App.Core.armContrl.motorJ;
                    for (int i = 0; i < motorJ.Length; i++)
                    {
                        App.Core.armContrl.GetCurrentAngle(i + 1, motorJ[i]);
                    }

                    var a1 = motorJ[0].CurrentAngle;
                    var a2 = motorJ[1].CurrentAngle;
                    var a3 = motorJ[2].CurrentAngle;
                    var a4 = motorJ[3].CurrentAngle;
                    var a5 = motorJ[4].CurrentAngle;
                    var a6 = motorJ[5].CurrentAngle;

                    App.Core.armContrl.MoveJ(a1, a2, a3, a4, a5, a6);
                    var pose6 = App.Core.armContrl.currentPose6D;
                    var joint = App.Core.armContrl.currentJoints;
                    App.Core.armContrl.lastJoints = App.Core.armContrl.currentJoints;

                    tb_X.Text = Math.Round(pose6.X, 2) + "";
                    tb_Y.Text = Math.Round(pose6.Y, 2) + "";
                    tb_Z.Text = Math.Round(pose6.Z, 2) + "";
                    tb_A.Text = Math.Round(pose6.A, 2) + "";
                    tb_B.Text = Math.Round(pose6.B, 2) + "";
                    tb_C.Text = Math.Round(pose6.C, 2) + "";

                    tb_Joint1.Text = Math.Round(joint.a[0], 2) + "";
                    tb_Joint2.Text = Math.Round(joint.a[1], 2) + "";
                    tb_Joint3.Text = Math.Round(joint.a[2], 2) + "";
                    tb_Joint4.Text = Math.Round(joint.a[3], 2) + "";
                    tb_Joint5.Text = Math.Round(joint.a[4], 2) + "";
                    tb_Joint6.Text = Math.Round(joint.a[5], 2) + "";
                });
            });

            
        }

        //让机械臂运动
        private void Bt_MoveJoint_Click(object sender, RoutedEventArgs e)
        {
            App.Core.armContrl.MoveJoints();
        }

        //逆解计算
        private void Bt_IK_Click(object sender, RoutedEventArgs e)
        {
            double.TryParse(tb_X.Text, out double x);
            double.TryParse(tb_Y.Text, out double y);
            double.TryParse(tb_Z.Text, out double z);
            double.TryParse(tb_A.Text, out double a);
            double.TryParse(tb_B.Text, out double b);
            double.TryParse(tb_C.Text, out double c);

            var res =App.Core.armContrl.MoveL(x, y, z, a, b, c);
            if(!res)
            {
                Log.Info("逆解无解");
            }
            var joint = App.Core.armContrl.currentJoints;

            tb_Joint1.Text = Math.Round(joint.a[0],2) + "";
            tb_Joint2.Text = Math.Round(joint.a[1],2) + "";
            tb_Joint3.Text = Math.Round(joint.a[2],2) + "";
            tb_Joint4.Text = Math.Round(joint.a[3],2) + "";
            tb_Joint5.Text = Math.Round(joint.a[4],2) + "";
            tb_Joint6.Text = Math.Round(joint.a[5], 2) + "";
        }
        //正解计算
        private void Bt_FK_Click(object sender, RoutedEventArgs e)
        {
            double.TryParse(tb_Joint1.Text, out double a1);
            double.TryParse(tb_Joint2.Text, out double a2);
            double.TryParse(tb_Joint3.Text, out double a3);
            double.TryParse(tb_Joint4.Text, out double a4);
            double.TryParse(tb_Joint5.Text, out double a5);
            double.TryParse(tb_Joint6.Text, out double a6);

             
            bool isSole = App.Core.armContrl.MoveJ(a1, a2, a3, a4, a5, a6);
            if(!isSole)
            {
                Log.Info("角度限制无法解除");
            }
            var pose6 = App.Core.armContrl.currentPose6D;

            tb_X.Text = Math.Round(pose6.X, 2) + "";
            tb_Y.Text = Math.Round(pose6.Y, 2) + "";
            tb_Z.Text = Math.Round(pose6.Z, 2) + "";
            tb_A.Text = Math.Round(pose6.A, 2) + "";
            tb_B.Text = Math.Round(pose6.B, 2) + "";
            tb_C.Text = Math.Round(pose6.C, 2) + "";
        }

        //立即停止
        private void Bt_StopNow_Click(object sender, RoutedEventArgs e)
        {
            App.Core.armContrl.ArmStopNow();
        }

        //回零操作
        private void Bt_Home_Click(object sender, RoutedEventArgs e)
        {
            App.Core.armContrl.Homing();

            //回零之后更新正逆解参数
            App.Core.armContrl.MoveJ(0, -90, 180,0, 0, 0);
            var pose6 = App.Core.armContrl.currentPose6D;
            var joint = App.Core.armContrl.currentJoints;

            tb_X.Text = Math.Round(pose6.X, 2) + "";
            tb_Y.Text = Math.Round(pose6.Y, 2) + "";
            tb_Z.Text = Math.Round(pose6.Z, 2) + "";
            tb_A.Text = Math.Round(pose6.A, 2) + "";
            tb_B.Text = Math.Round(pose6.B, 2) + "";
            tb_C.Text = Math.Round(pose6.C, 2) + "";

            tb_Joint1.Text = Math.Round(joint.a[0], 2) + "";
            tb_Joint2.Text = Math.Round(joint.a[1], 2) + "";
            tb_Joint3.Text = Math.Round(joint.a[2], 2) + "";
            tb_Joint4.Text = Math.Round(joint.a[3], 2) + "";
            tb_Joint5.Text = Math.Round(joint.a[4], 2) + "";
            tb_Joint6.Text = Math.Round(joint.a[5], 2) + "";
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            ws = this.WindowState;
            if (ws == WindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void icon()
        {
            string path = System.IO.Path.GetFullPath(@"Static\icon.ico");
            if (File.Exists(path))
            {
                this.notifyIcon = new NotifyIcon();
                this.notifyIcon.BalloonTipText = "Jyker"; //设置程序启动时显示的文本
                this.notifyIcon.Text = "Jyker";//最小化到托盘时，鼠标点击时显示的文本
                System.Drawing.Icon icon = new System.Drawing.Icon(path);//程序图标
                this.notifyIcon.Icon = icon;
                this.notifyIcon.Visible = true;
                notifyIcon.MouseDoubleClick += OnNotifyIconDoubleClick;
                
            }

        }

        private void OnNotifyIconDoubleClick(object sender, EventArgs e)
        {
            this.Show();
            WindowState = wsl;
        }


        #region 托盘右键菜单
        private void contextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();

            //关联 NotifyIcon 和 ContextMenuStrip
            notifyIcon.ContextMenuStrip = cms;

            System.Windows.Forms.ToolStripMenuItem exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exitMenuItem.Text = "退出";
            exitMenuItem.Click += new EventHandler(exitMenuItem_Click);

            System.Windows.Forms.ToolStripMenuItem hideMenumItem = new System.Windows.Forms.ToolStripMenuItem();
            hideMenumItem.Text = "隐藏";
            hideMenumItem.Click += new EventHandler(hideMenumItem_Click);

            System.Windows.Forms.ToolStripMenuItem showMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            showMenuItem.Text = "显示";
            showMenuItem.Click += new EventHandler(showMenuItem_Click);

            cms.Items.Add(exitMenuItem);
            cms.Items.Add(hideMenumItem);
            cms.Items.Add(showMenuItem);
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;

            System.Windows.Application.Current.Shutdown();
        }

        private void hideMenumItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void showMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.Activate();
        }
        #endregion

        #region 日志
        int richTextLine = 0;
        private void Log_MessageEvent(string text)
        {
            var str = $"{DateTime.Now.ToString("HH:mm:ss")}:{text}\n";
            this.Dispatcher.Invoke(() =>
            {
                tbLog.AppendText(str);
                tbLog.ScrollToEnd();
                if (richTextLine >= 3000)
                {
                    tbLog.Text = string.Empty;
                    richTextLine = 0;
                }
                richTextLine++;
            });
        }
        #endregion
    }
}
