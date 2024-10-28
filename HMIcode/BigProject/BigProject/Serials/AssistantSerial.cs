using BigProject.Devices;
using BigProject.Devices.Arm;
using BigProject.Devices.DeskLamp;
using BigProject.Devices.Fan;
using BigProject.Devices.NightLight;
using BigProject.Devices.WallPainting;
using BigProject.Logger;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigProject.Serials
{
    public class AssistantSerial
    {
        private System.IO.Ports.SerialPort serialPort1 = new System.IO.Ports.SerialPort();
        public AssistantSerial(string name1 = "COM17")
        {

            if (!serialPort1.IsOpen)
            {
                serialPort1.BaudRate = 9600;
                serialPort1.PortName = name1;
                serialPort1.Open();
                serialPort1.DataReceived += SerialPort1_DataReceived;
                Log.Info($"语音助手已连接");
            }
            
        }

        //接收byte数据
        private void SerialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            byte[] rec = new byte[sp.ReadBufferSize];
            int lenth = sp.Read(rec, 0, sp.ReadBufferSize);
            AssistantContrl(rec[0]);
            Log.Info($"Assistant send {rec[0]}");
        }

        public void AssistantContrl(int contrlWord)
        {
            switch (contrlWord)
            {
                case 1:
                    var dlList = App.Core.Devices.Where(t => t.DeviceType == Devices.DeviceType.DeskLamp);
                    foreach (var dl in dlList)
                    {
                        var one = dl as DeskLampDevice;
                        one.Open();
                        Log.Info($"台灯{one.Id} 已打开");
                    }

                    break;
                case 2:
                    var _dlList = App.Core.Devices.Where(t => t.DeviceType == Devices.DeviceType.DeskLamp);
                    foreach (var dl in _dlList)
                    {
                        var one = dl as DeskLampDevice;
                        one.Close();
                        Log.Info($"台灯{one.Id} 已关闭");
                    }
                    break;
                case 3:
                    var fans1 = App.Core.Devices.Where(t => t.DeviceType == Devices.DeviceType.Fan);
                    foreach (var fan in fans1)
                    {
                        var one = fan as FanDevice;
                        one.Open();
                        Log.Info($"风扇{one.Id} 已开启");
                    }
                    break;
                case 4:
                    var fans2 = App.Core.Devices.Where(t => t.DeviceType == Devices.DeviceType.Fan);
                    foreach (var fan in fans2)
                    {
                        var one = fan as FanDevice;
                        one.Close();
                        Log.Info($"风扇{one.Id} 已关闭");
                    }
                    break;
                case 5:
                    var fans3 = App.Core.Devices.Where(t => t.DeviceType == Devices.DeviceType.Fan);
                    foreach (var fan in fans3)
                    {
                        var one = fan as FanDevice;
                        one.LevelDown();
                        Log.Info($"风扇{one.Id} 已调小");
                    }
                    break;
                case 6:
                    var fans4 = App.Core.Devices.Where(t => t.DeviceType == Devices.DeviceType.Fan);
                    foreach (var fan in fans4)
                    {
                        var one = fan as FanDevice;
                        one.LevelUp();
                        Log.Info($"风扇{one.Id} 已调大");
                    }
                    break;
                case 7:
                    var wps1 = App.Core.Devices.Where(t => t.DeviceType == Devices.DeviceType.WallPainting);
                    foreach (var wp in wps1)
                    {
                        var one = wp as WallPaintingDevice;
                        one.Open();
                        Log.Info($"壁画{one.Id} 已开启");
                    }
                    break;
                case 8:
                    var wps2 = App.Core.Devices.Where(t => t.DeviceType == Devices.DeviceType.WallPainting);
                    foreach (var wp in wps2)
                    {
                        var one = wp as WallPaintingDevice;
                        one.Close();
                        Log.Info($"壁画{one.Id} 已关闭");
                    }
                    break;
                case 15:
                    var lights1 = App.Core.Devices.Where(t => t.DeviceType == Devices.DeviceType.NightLight);
                    foreach (var li in lights1)
                    {
                        var one = li as NightLightDevice;
                        one.Open();
                        Log.Info($"小灯{one.Id} 已开启");
                    }
                    break;
                case 16:
                    var lights2 = App.Core.Devices.Where(t => t.DeviceType == Devices.DeviceType.NightLight);
                    foreach (var li in lights2)
                    {
                        var one = li as NightLightDevice;
                        one.Close();
                        Log.Info($"小灯{one.Id} 已关闭");
                    }
                    break;
                case 13:
                    //夜间模式
                    foreach (var dev in App.Core.Devices)
                    {
                        if(dev.DeviceType == DeviceType.NightLight)
                        {
                            var one = dev as NightLightDevice;
                            one.Open();
                            Log.Info($"小灯{one.Id} 已打开");
                        }
                        if(dev.DeviceType == DeviceType.DeskLamp)
                        {
                            var one = dev as DeskLampDevice;
                            one.Open();
                            Log.Info($"台灯{one.Id} 已打开");
                        }
                        if(dev.DeviceType == DeviceType.ArmLed)
                        {
                            var one = dev as ArmLed;
                            one.Open();
                            Log.Info($"机械臂灯 {one.Id} 已打开");
                        }
                    }
                    break;
                case 17:
                case 18:
                    //单机模式，除了电脑，全部关闭
                    foreach (var dev in App.Core.Devices)
                    {
                        if (dev.DeviceType == DeviceType.NightLight)
                        {
                            var one = dev as NightLightDevice;
                            one.Close();
                            Log.Info($"小灯{one.Id} 已关闭");
                        }
                        if (dev.DeviceType == DeviceType.DeskLamp)
                        {
                            var one = dev as DeskLampDevice;
                            one.Close();
                            Log.Info($"台灯{one.Id} 已关闭");
                        }
                        if (dev.DeviceType == DeviceType.ArmLed)
                        {
                            var one = dev as ArmLed;
                            one.Close();
                            Log.Info($"机械臂灯 {one.Id} 已关闭");
                        }
                        if(dev.DeviceType == DeviceType.WallPainting)
                        {
                            var one = dev as WallPaintingDevice;
                            one.Close();
                            Log.Info($"壁画 {one.Id} 已关闭");
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
