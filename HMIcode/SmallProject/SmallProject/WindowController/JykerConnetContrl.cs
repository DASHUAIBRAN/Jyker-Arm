using Microsoft.Win32;
using SmallProject.Devices.Arm;
using SmallProject.Logger;
using SmallProject.Serials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace SmallProject.WindowController
{
    class JykerConnetContrl
    {
        private MainWindow M;

        public JykerConnetContrl(MainWindow m)
        {
            M = m;
            m.bt_Link.Click += bt_Link_Click;
            m.bt_LinkAuto.Click += bt_LinkAuto_Click;
            LoadComList();
        }

        //加载串口列表
        private List<string> LoadComList()
        {
            var list = new List<string>();
            const string keyPath = @"HARDWARE\DEVICEMAP\SERIALCOMM";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath))
            {
                if (key != null)
                {
                    M.Dispatcher.Invoke(() => {
                        M.cb_ComList.Items.Clear();
                        foreach (string valueName in key.GetValueNames())
                        {
                            if (!valueName.Contains("USB")) continue;
                            string portName = key.GetValue(valueName) as string;
                            var caption = $"{portName}_{valueName}";
                            Match match = Regex.Match(caption, @"COM\d+");
                            if (match.Success)
                            {
                                string comPortName = match.Value; // 提取匹配到的 COM 名称
                                M.cb_ComList.Items.Add(comPortName);
                                list.Add(comPortName);
                            }
                        }
                    });
                }
            }
            return list;
        }

        //自动连接
        private void bt_LinkAuto_Click(object sender, RoutedEventArgs e)
        {
            if(M.cb_ComList.Items.Count==0)
            {
                JLog.Info("没有搜索到连接口，请检查硬件连接");
            }

            if(M.cb_ComList.Items.Count>1)
            {
                JLog.Info("搜索到多个连接口，请选择一个");
            }

            M.cb_ComList.SelectedIndex = 0;
            //自动连接
            var name = M.cb_ComList.SelectedItem + "";
            var res = App.Core.Serial.Open(name);
            if (res)
            {
                M.cb_ComList.IsEnabled = false;
                M.bt_Link.Content = "断开连接";
                SetButtomState(true);
                M.gb_ClawControl.IsEnabled = true;
                M.gb_ArmControl.IsEnabled = true;
                M.bt_LinkAuto.IsEnabled = false;
                
            }
           
        }
        //手动连接
        private void bt_Link_Click(object sender, RoutedEventArgs e)
        {



            if (M.bt_Link.Content.ToString() == "断开连接")
            {
                M.bt_LinkAuto.IsEnabled = true;
                M.cb_ComList.IsEnabled = true;
                M.bt_Link.Content = "手动连接";
                SetButtomState(false);
                M.gb_ClawControl.IsEnabled = false;

            }
            else
            {
                var name = M.cb_ComList.SelectedItem + "";
                var res = App.Core.Serial.Open(name);
                if (res)
                {
                    M.cb_ComList.IsEnabled = false;
                    M.bt_Link.Content = "断开连接";
                    SetButtomState(true);
                    M.gb_ClawControl.IsEnabled = true;
                    M.gb_ArmControl.IsEnabled = true;
                    M.bt_LinkAuto.IsEnabled = false;
                }
            }
        }
        //设置按钮状态
        private void SetButtomState(bool State = false)
        {
            M.Dispatcher.Invoke(() =>
            {
                M.bt_ApplyHomePosition.IsEnabled = State;
                M.bt_StopNow.IsEnabled = State;
                //M.bt_FK.IsEnabled = State;
                //M.bt_IK.IsEnabled = State;
                M.bt_MoveJoint.IsEnabled = State;
                M.bt_GetCurrentAngle.IsEnabled = State;
                M.bt_AddRecord.IsEnabled = State;
                M.bt_MoveArmHand.IsEnabled = State;
                M.bt_MoveLoop.IsEnabled = State;
                M.bt_MoveLoopStop.IsEnabled = State;
                M.bt_DeleteRecord.IsEnabled = State;

            });
        }
    }

}
