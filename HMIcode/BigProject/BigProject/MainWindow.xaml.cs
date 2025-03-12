using BigProject.Devices.Arm.Kinematic.Models;
using BigProject.Logger;
using BigProject.Serials;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Xml.Linq;
using System.Management;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Text.RegularExpressions;
using System.Drawing;
using BigProject.Devices.Arm;
using Rubyer;
using BigProject.JointMoveRecord;
using System.Collections.ObjectModel;

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

            //添加夹爪信息回调
            ArmClaw.UpdateClawMsgEvent+= UpdateClawMsg;

            //赋值当前角度位置
            //MainWindow_UpdateJointAngle();

            //加载串口列表
            LoadComList();
            //设置按钮不可按
            SetButtomState(false);
            //加载记录列表
            LoadJointRecords();
            
        }

        #region 机械臂控制
        //加载记录列表
        private void LoadJointRecords()
        {
            App.Core.JointRecords = JointRecordRes.Read();
            dg_JointRecord.ItemsSource = App.Core.JointRecords;
        } 


        //设置手动移动机械臂
        private void bt_MoveArmHand_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(() => {
                var motorJ = App.Core.ArmContrl.motorJ;
                for (int i = 0; i < motorJ.Length; i++)
                {
                    if (!App.Core.ArmContrl.SetIfEnable(i+1,false))
                    {
                        Log.Info($"电机{i + 1}设置失能状态失败");
                    }
                }
            });
            
        }
        //记录机械臂当前位置
        private void bt_AddRecord_Click(object sender, RoutedEventArgs e)
        {
            MainWindow_UpdateJointAngle();
            this.Dispatcher.Invoke(() => {
                var pose6 = App.Core.ArmContrl.currentPose6D;
                var joint = App.Core.ArmContrl.currentJoints;
                App.Core.JointRecords.Add(new JointMoveRecord.JointRecordModel
                {
                    AddTime = DateTime.Now,
                    J1 = Math.Round(joint.a[0], 2),
                    J2 = Math.Round(joint.a[1], 2),
                    J3 = Math.Round(joint.a[2], 2),
                    J4 = Math.Round(joint.a[3], 2),
                    J5 = Math.Round(joint.a[4], 2),
                    J6 = Math.Round(joint.a[5], 2),

                    X = Math.Round(pose6.X, 2),
                    Y = Math.Round(pose6.Y, 2),
                    Z = Math.Round(pose6.Z, 2),
                    A = Math.Round(pose6.A, 2),
                    B = Math.Round(pose6.B, 2),
                    C = Math.Round(pose6.C, 2),
                });
                dg_JointRecord.ItemsSource = App.Core.JointRecords;
                JointRecordRes.Write(App.Core.JointRecords);
            });
            
        }
        //循环运动机械臂
        bool JointLoopIsRun = false;
        private void bt_MoveLoop_Click(object sender, RoutedEventArgs e)
        {
            if (JointLoopIsRun) return;
            var motorJ = App.Core.ArmContrl.motorJ;
            for (int i = 0; i < motorJ.Length; i++)
            {
                if (!App.Core.ArmContrl.SetIfEnable(i + 1, true))
                {
                    Log.Info($"电机{i + 1}设置使能状态失败");
                }
            }
            if (!App.Core.ArmContrl.SetIfEnable(7, true))
            {
                Log.Info($"夹爪设置使能状态失败");
            }
            JointLoopIsRun = true;
            Task.Run(() => {
                while (JointLoopIsRun)
                {
                    Thread.Sleep(50);
                    foreach (var rec in App.Core.JointRecords)
                    {
                        if (!JointLoopIsRun) return;
                        this.Dispatcher.Invoke(() =>
                        {
                            dg_JointRecord.SelectedItem = rec;
                        });
                        App.Core.ArmContrl.currentJoints.a[0] = rec.J1;
                        App.Core.ArmContrl.currentJoints.a[1] = rec.J2;
                        App.Core.ArmContrl.currentJoints.a[2] = rec.J3;
                        App.Core.ArmContrl.currentJoints.a[3] = rec.J4;
                        App.Core.ArmContrl.currentJoints.a[4] = rec.J5;
                        App.Core.ArmContrl.currentJoints.a[5] = rec.J6;
                        App.Core.ArmContrl.MoveJoints();
                        Thread.Sleep(100);
                        while (!App.Core.ArmContrl.IsMoveOver())
                        {
                            if (!JointLoopIsRun) return;
                            Thread.Sleep(2000);
                        }
                    }
                }
                App.Core.ArmContrl.ArmStopNow();
            });
        }
        //停止循环
        private void bt_MoveLoopStop_Click(object sender, RoutedEventArgs e)
        {
            JointLoopIsRun = false;
        }
        //删除记录
        private void bt_DeleteRecord_Click(object sender, RoutedEventArgs e)
        {
            if(dg_JointRecord.SelectedItem==null)
            {
                MessageBoxR.Show("请先选择一条记录.");
                return;
            }
            var rec = dg_JointRecord.SelectedItem as JointRecordModel;
            App.Core.JointRecords.Remove(rec);
            dg_JointRecord.ItemsSource = App.Core.JointRecords;
            JointRecordRes.Write(App.Core.JointRecords);
        }
        //设置按钮状态
        private void SetButtomState(bool State = false)
        {
            this.Dispatcher.Invoke(() =>
            {
                bt_Home.IsEnabled = State;
                bt_StopNow.IsEnabled = State;
                bt_FK.IsEnabled = State;
                bt_IK.IsEnabled = State;
                bt_MoveJoint.IsEnabled = State;
                bt_GetCurrentAngle.IsEnabled = State;
                bt_AddRecord.IsEnabled = State;
                bt_MoveArmHand.IsEnabled = State;
                bt_MoveLoop.IsEnabled = State;
                bt_MoveLoopStop.IsEnabled = State;
                bt_DeleteRecord.IsEnabled = State;
                
            });
        }
        //加载串口列表
        private List<string> LoadComList()
        {
            // 查询所有串口设备
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_PnPEntity WHERE Caption LIKE '%(COM%)' AND Caption LIKE '%USB%'");
            var list = new List<string>();
            this.Dispatcher.Invoke(() => {
                cb_ComList.Items.Clear();
                foreach (var port in searcher.Get())
                {
                    string caption = port["Caption"]?.ToString() ?? "无描述";
                    Match match = Regex.Match(caption, @"COM\d+");
                    if (match.Success)
                    {
                        string comPortName = match.Value; // 提取匹配到的 COM 名称
                        cb_ComList.Items.Add(comPortName);
                        list.Add(comPortName);
                    }
                    
                }
            });
            
            return list;
        }

        //自动连接
        private void bt_LinkAuto_Click(object sender, RoutedEventArgs e)
        {
            var list = LoadComList();
            //连接

            Task.Run(() => {
                //加载串口列表
                try
                {
                    foreach (var item in list)
                    {
                        Log.Info($"尝试连接{item}...");
                        var result = App.Core.InitArm(item);
                        if (result)
                        {
                            //查看是否存在机械臂
                            var armConnected = false;
                            var res =App.Core.ArmSerial.SendMsgForResult(new byte[3] { 0x01, 0x33, 0x6b },out byte[] resMsg);
                            Thread.Sleep(500);
                            if (res&&resMsg[0] > 0)
                            {
                                Log.Info($"{item}连接成功，找到jyker机械臂");
                                this.Dispatcher.Invoke(() => {
                                    cb_ComList.SelectedItem = item;
                                    bt_LinkAuto.IsEnabled = false;
                                    cb_ComList.IsEnabled = false;
                                    bt_Link.Content = "断开连接";
                                    SetButtomState(true);
                                    armConnected = true;
                                    //读取位置信息并赋值
                                    MainWindow_UpdateJointAngle();
                                });
                            }
                            //夹爪连接
                            var clawConnected = false;
                            var resClaw = App.Core.ArmSerial.SendMsgForResult(new byte[3] { 0x07, 0x33, 0x6b }, out byte[] resMsgClaw);
                            if (resClaw && resMsgClaw[0] > 0)
                            {
                                clawConnected = true;
                                Log.Info($"{item}连接成功，找到jyker夹爪");
                                this.Dispatcher.Invoke(() => {
                                    gb_ClawControl.IsEnabled = true;
                                });
                            }
                            if (result&&!armConnected&&!clawConnected)
                            {
                                App.Core.ArmDispose();
                            }
                        }



                    } 
                }
                catch (Exception ex)
                {
                    Log.Info(ex.ToString());
                }
            });


        }
        //手动连接
        private void bt_Link_Click(object sender, RoutedEventArgs e)
        {
            if (bt_Link.Content.ToString() == "断开连接")
            {
                bt_LinkAuto.IsEnabled = true;
                cb_ComList.IsEnabled = true;
                bt_Link.Content = "手动连接";
                App.Core.ArmDispose();
                SetButtomState(false);
                gb_ClawControl.IsEnabled = false;
            }
            else
            {
                var name = cb_ComList.SelectedItem + "";
                var result = App.Core.InitArm(name);
                if (result)
                {
                    bt_LinkAuto.IsEnabled = false;
                    cb_ComList.IsEnabled = false;
                    bt_Link.Content = "断开连接";
                    SetButtomState(true);
                    //读取位置信息并赋值
                    MainWindow_UpdateJointAngle();
                    gb_ClawControl.IsEnabled = true;
                }
            }
        }
        //获取当前电机目标位置
        private void bt_GetCurrentAngle_Click(object sender, RoutedEventArgs e)
        {
            MainWindow_UpdateJointAngle();
        }

        //获取当前角度位置
        private void MainWindow_UpdateJointAngle()
        {
            this.Dispatcher.Invoke(() => {
                var motorJ = App.Core.ArmContrl.motorJ;
                for (int i = 0; i < motorJ.Length; i++)
                {
                    if(!App.Core.ArmContrl.GetCurrentAngle(i + 1, motorJ[i],out double angle))
                    {
                        Log.Info($"电机{i + 1}角度获取失败");
                    }
                }

                var a1 = motorJ[0].CurrentAngle;
                var a2 = motorJ[1].CurrentAngle;
                var a3 = motorJ[2].CurrentAngle;
                var a4 = motorJ[3].CurrentAngle;
                var a5 = motorJ[4].CurrentAngle;
                var a6 = motorJ[5].CurrentAngle;

                App.Core.ArmContrl.MoveJ(a1, a2, a3, a4, a5, a6);
                var pose6 = App.Core.ArmContrl.currentPose6D;
                var joint = App.Core.ArmContrl.currentJoints;

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


        }

        //让机械臂运动
        private void bt_MoveJoint_Click(object sender, RoutedEventArgs e)
        {
            App.Core.ArmContrl.MoveJoints();
        }

        //逆解计算
        private void bt_IK_Click(object sender, RoutedEventArgs e)
        {
            double.TryParse(tb_X.Text, out double x);
            double.TryParse(tb_Y.Text, out double y);
            double.TryParse(tb_Z.Text, out double z);
            double.TryParse(tb_A.Text, out double a);
            double.TryParse(tb_B.Text, out double b);
            double.TryParse(tb_C.Text, out double c);

            var res =App.Core.ArmContrl.MoveL(x, y, z, a, b, c);
            if(!res)
            {
                Log.Info("逆解无解");
            }
            var joint = App.Core.ArmContrl.currentJoints;

            tb_Joint1.Text = Math.Round(joint.a[0],2) + "";
            tb_Joint2.Text = Math.Round(joint.a[1],2) + "";
            tb_Joint3.Text = Math.Round(joint.a[2],2) + "";
            tb_Joint4.Text = Math.Round(joint.a[3],2) + "";
            tb_Joint5.Text = Math.Round(joint.a[4],2) + "";
            tb_Joint6.Text = Math.Round(joint.a[5], 2) + "";
        }
        //正解计算
        private void bt_FK_Click(object sender, RoutedEventArgs e)
        {
            double.TryParse(tb_Joint1.Text, out double a1);
            double.TryParse(tb_Joint2.Text, out double a2);
            double.TryParse(tb_Joint3.Text, out double a3);
            double.TryParse(tb_Joint4.Text, out double a4);
            double.TryParse(tb_Joint5.Text, out double a5);
            double.TryParse(tb_Joint6.Text, out double a6);

             
            bool isSole = App.Core.ArmContrl.MoveJ(a1, a2, a3, a4, a5, a6);
            if(!isSole)
            {
                Log.Info("角度限制无法解除");
            }
            var pose6 = App.Core.ArmContrl.currentPose6D;

            tb_X.Text = Math.Round(pose6.X, 2) + "";
            tb_Y.Text = Math.Round(pose6.Y, 2) + "";
            tb_Z.Text = Math.Round(pose6.Z, 2) + "";
            tb_A.Text = Math.Round(pose6.A, 2) + "";
            tb_B.Text = Math.Round(pose6.B, 2) + "";
            tb_C.Text = Math.Round(pose6.C, 2) + "";
        }

        //立即停止
        private void bt_StopNow_Click(object sender, RoutedEventArgs e)
        {
            App.Core.ArmContrl.ArmStopNow();
        }

        //回零操作
        private async void bt_Home_Click(object sender, RoutedEventArgs e)
        {
            var result = await MessageBoxR.Show("注意！非金属减速机版本请手动回零，否则容易损坏减速器！确定要回零吗?",
                "确认操作",
                MessageBoxButton.OKCancel,
                MessageBoxType.Question);
            if(result == MessageBoxResult.OK)
            {
                App.Core.ArmContrl.Homing();

                //回零之后更新正逆解参数
                App.Core.ArmContrl.MoveJ(0, -90, 180, 0, 90, 0);
                var pose6 = App.Core.ArmContrl.currentPose6D;
                var joint = App.Core.ArmContrl.currentJoints;

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
            
        }
        #endregion

        #region 夹爪控制
        bool LoopReadClawAngle = false;

        private void bt_ClawHome_Click(object sender, RoutedEventArgs e)
        {
            App.Core.ArmClaw.Home();
        }
        //关闭堵转保护
        private void bt_ClawStop_Click(object sender, RoutedEventArgs e)
        {
            App.Core.ArmClaw.Stop();
        }
        //开启循环读取信息
        private void bt_ClawLoopStart_Click(object sender, RoutedEventArgs e)
        {
            LoopReadClawAngle = true;
            bt_ClawLoopStart.IsEnabled = false;
            bt_ClawStop.IsEnabled = true;
            Task.Run(() => {
                
                while (LoopReadClawAngle)
                {
                    App.Core.ArmClaw.ReadAngle();
                    Thread.Sleep(50);
                }
               
            });
        }
        //关闭循环读取角度信息
        private void bt_ClawLoopEnd_Click(object sender, RoutedEventArgs e)
        {
            LoopReadClawAngle = false;
            bt_ClawLoopStart.IsEnabled = true;
            bt_ClawStop.IsEnabled = false;
        }
        //设置夹爪角度
        private void pg_ClawAngle_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!gb_ClawControl.IsEnabled)
            {
                return;
            }
            var value = e.NewValue;
            Task.Run(() =>
            {
                App.Core.ArmClaw.SetAngle(value);
            });
        }
        //更新夹爪的信息
        private void UpdateClawMsg(double angle,double length,double power)
        {
            this.Dispatcher.Invoke(() => {
                //pg_ClawAngle.Value = angle;
                pg_ClawLength.Value = length;
                pg_ClawPower.Value = power;
                tb_ClawAngle.Text = $"{angle}°";
                tb_ClawLength.Text = $"{length}mm";
                tb_ClawPower.Text = $"{power}";
            });
        }

        private void pg_ClawAngle_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }
        #endregion


        #region 托盘右键菜单

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
