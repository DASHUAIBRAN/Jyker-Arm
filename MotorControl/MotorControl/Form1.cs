using Masuit.Tools;
using Microsoft.Win32;
using MotorControl.Logger;
using MotorControl.Serials;
using MotorControl.Serials.Slcan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MotorControl
{
    public partial class Form1 : Form
    {

        Serial Serial;
        //坐标列表
        List<float> positions = new List<float>();
        //记录获取位置信息的时间
        DateTime PosTime = DateTime.Now;
        //记录获取速递信息的时间
        DateTime VelocityTime = DateTime.Now;
        //当前位置
        float PosNow = 0;
        //当前速度
        float VelocityNow = 0;
        //停止运动标志
        bool IsStop = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Serial = new Serial();
            Serial.GetResInfo += Serial_GetResInfo;
        }

        //加载串口
        private void cb_Com_DropDown(object sender, EventArgs e)
        {
            var list = new List<string>();
            const string keyPath = @"HARDWARE\DEVICEMAP\SERIALCOMM";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath))
            {
                if (key != null)
                {
                    cb_ComList.Items.Clear();
                    foreach (string valueName in key.GetValueNames())
                    {
                        if (!valueName.Contains("USB")) continue;
                        string portName = key.GetValue(valueName) as string;
                        var caption = $"{portName}_{valueName}";
                        Match match = Regex.Match(caption, @"COM\d+");
                        if (match.Success)
                        {
                            string comPortName = match.Value; // 提取匹配到的 COM 名称
                            cb_ComList.Items.Add(comPortName);
                            list.Add(comPortName);
                        }
                    }
                }
            }
        }

        //打开连接
        private void bt_Link_Click(object sender, EventArgs e)
        {
            if (bt_Link.Text == "断开连接")
            {
                cb_ComList.Enabled = true;
                bt_Link.Text = "连接";
                SetButtomState(false);

            }
            else
            {
                var name = cb_ComList.SelectedItem + "";
                var res = Serial.Open(name);
                if (res)
                {
                    cb_ComList.Enabled = false;
                    bt_Link.Text = "断开连接";
                    SetButtomState(true);
                }
            }
        }
        private void SetButtomState(bool state)
        {

        }
        //读取信息
        private void bt_ReadInfo_Click(object sender, EventArgs e)
        {
            var Id = (int)numID.Value;
            var frame = SlcanParser.ParseSlcanFrameStr(Id, 0x21);
            Serial?.PushDataToQueue(frame);
            Thread.Sleep(5);
            frame = SlcanParser.ParseSlcanFrameStr(Id, 0x22);
            Serial?.PushDataToQueue(frame);
            Thread.Sleep(5);
            frame = SlcanParser.ParseSlcanFrameStr(Id, 0x23);
            Serial?.PushDataToQueue(frame);
            Thread.Sleep(5);
            frame = SlcanParser.ParseSlcanFrameStr(Id, 0x26);
            Serial?.PushDataToQueue(frame);
        }

        //读取数据回调
        private void Serial_GetResInfo(int Id, float value, InfoType type)
        {
            this.Invoke(new Action(() => {
                switch (type)
                {
                    case InfoType.Current:
                        lb_Current.Text = value + "";
                        break;
                    case InfoType.Velocity:
                        lb_Velocity.Text = value + "";
                        VelocityTime = DateTime.Now;
                        VelocityNow = value;
                        break;
                    case InfoType.Position:
                        lb_Pos.Text = value + "";
                        PosTime = DateTime.Now;
                        PosNow = value;
                        JLog.Info("这时候获取的");
                        break;
                    case InfoType.CurrentLimint:
                        lb_LimitCurrent.Text = value + "";
                        break;
                    default:
                        break;
                }
                lb_ID.Text = Id + "";
            }));
        }

        //使能(失能)电机
        private void bt_Enable_Click(object sender, EventArgs e)
        {
            var Id = (int)numID.Value;
            var frame = SlcanParser.ParseSlcanFrameStr(Id, 0x01);
            Serial?.PushDataToQueue(frame);
            if (bt_Enable.Text.Equals("使能电机"))
            {
                bt_Enable.Text = "失能电机";
            }
            else
            {
                bt_Enable.Text = "使能电机";
            }
        }

        //校准电机
        private void bt_Call_Click(object sender, EventArgs e)
        {
            var Id = (int)numID.Value;
            var frame = SlcanParser.ParseSlcanFrameStr(Id, 0x02);
            Serial?.PushDataToQueue(frame);
        }

        //转一圈
        private void bt_PosOnce_Click(object sender, EventArgs e)
        {
            var Id = (int)numID.Value;
            var frame = SlcanParser.ParseSlcanFrameStr(Id, 0x05, 1);
            Serial?.PushDataToQueue(frame);
        }

        //以一圈每秒的速度旋转
        private void bt_SpeedOnce_Click(object sender, EventArgs e)
        {
            var Id = (int)numID.Value;
            var frame = SlcanParser.ParseSlcanFrameStr(Id, 0x04, 1);
            Serial?.PushDataToQueue(frame);
        }

        //以1A的电流旋转
        private void bt_InOnce_Click(object sender, EventArgs e)
        {
            var Id = (int)numID.Value;
            var frame = SlcanParser.ParseSlcanFrameStr(Id, 0x03, 1);
            Serial?.PushDataToQueue(frame);
        }

        //马上停止
        private void bt_StopNow_Click(object sender, EventArgs e)
        {
            var Id = (int)numID.Value;
            var frame = SlcanParser.ParseSlcanFrameStr(Id, 0x04, 0);
            Serial?.PushDataToQueue(frame);
            IsStop = true;
        }
        //转到xx圈，绝度位移
        private void bt_SetPosCom_Click(object sender, EventArgs e)
        {
            var Id = (int)numID.Value;
            var value = (float)num_Position.Value;
            var frame = SlcanParser.ParseSlcanFrameStr(Id, 0x05, value);
            Serial?.PushDataToQueue(frame);
        }
        //设置以xx速度运行
        private void bt_SetVelocityCom_Click(object sender, EventArgs e)
        {
            var Id = (int)numID.Value;
            var value = (float)num_Velocity.Value;
            var frame = SlcanParser.ParseSlcanFrameStr(Id, 0x04, value);
            Serial?.PushDataToQueue(frame);
        }
        //以xx的电流运行电机
        private void bt_SetCurrentCom_Click(object sender, EventArgs e)
        {
            var Id = (int)numID.Value;
            var value = (float)num_Current.Value;
            var frame = SlcanParser.ParseSlcanFrameStr(Id, 0x03, value);
            Serial?.PushDataToQueue(frame);
        }
        //设置限制电流
        private void bt_SetLimitCurrent_Click(object sender, EventArgs e)
        {
            var Id = (int)numID.Value;
            var value = ((float)(numLimitCurrent.Value)) * 0.001f;
            var frame = SlcanParser.ParseSlcanFrameStr(Id, 0x12, value, resACK: true);
            Serial?.PushDataToQueue(frame);
        }

        //记录当前位置
        private void bt_RecordPos_Click(object sender, EventArgs e)
        {
            Task.Run(() => {
                var Id = (int)numID.Value;
                var frame = SlcanParser.ParseSlcanFrameStr(Id, 0x23);
                Serial?.PushDataToQueue(frame);
                int count = 0;
                while (true && count < 10)
                {
                    JLog.Info(PosTime.ToString());
                    count++;
                    if (PosTime.AddSeconds(1) > DateTime.Now)
                    {
                        this.Invoke(new Action(() => {
                            //一秒之内获取的，还比较新鲜，就用这个
                            list_Postions.Items.Add(PosNow);
                        }));
                        return;
                    }
                    Thread.Sleep(10);
                }

                this.Invoke(new Action(() => {
                    MessageBox.Show("获取当前位置失败");
                }));
                

            });
            
        }

        //循环运动
        private void bt_LoopPos_Click(object sender, EventArgs e)
        {
            IsStop = false;
            Task.Run(() =>
            {
                while (!IsStop)
                {

                    var items = list_Postions.Items;
                    if (items.Count == 0)
                    {
                        IsStop = true;
                        JLog.Info("没有循环数据");
                    }

                    
                    for (int i = 0; i < items.Count; i++)
                    {
                        // 发送移动命令
                        var val = Convert.ToString(items[i]);
                        if (!float.TryParse(val, out float res))
                        {
                            continue;
                        }
                        var Id = (int)numID.Value;
                        var frame = SlcanParser.ParseSlcanFrameStr(Id, 0x05, res);
                        Serial?.PushDataToQueue(frame);
                        this.Invoke(new Action(() =>
                        {
                            //设置select;
                            list_Postions.SelectedItem = items[i];
                        }));
                        Thread.Sleep(100);
                        // 等待电机到位
                        while (Math.Abs(PosNow - res) > 0.01&& !IsStop)
                        {
                            Id = (int)numID.Value;
                            frame = SlcanParser.ParseSlcanFrameStr(Id, 0x23);
                            Serial?.PushDataToQueue(frame);
                            Thread.Sleep(10);
                        }
                    }
                    
                }
            });
        }


        //打开说明文档
        private void bt_OpenDocunment_Click(object sender, EventArgs e)
        {
            string pdfPath = @"电机说明文档.pdf";

            if (!System.IO.File.Exists(pdfPath))
            {
                MessageBox.Show("文件不存在！");
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = pdfPath,
                    UseShellExecute = true  // 必须为 true 才能使用系统默认程序
                });
            }
            catch (Exception ex)
            {
                JLog.Error(ex);
            }
        }
    }
}
