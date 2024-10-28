using BigProject.Devices;
using BigProject.Devices.Arm;
using BigProject.Devices.Arm.Config;
using BigProject.Devices.DeskLamp;
using BigProject.Devices.Fan;
using BigProject.Devices.NightLight;
using BigProject.Devices.WallPainting;
using BigProject.Serials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BigProject
{
    public class Core
    {
        public string ipBegin = "192.168.1.";
        //设备列表
        public List<BaseDevice> Devices = new List<BaseDevice>();
        //接收的数据
        Byte[] buffer = new Byte[4096];
        //语音助手
        public AssistantSerial assistantSerial;
        //机械臂助手
        public ArmSerial armSerial;
        //配置信息
        public ArmConfig ArmConfig { get; set; }
        //机械臂控制
        public ArmContrl armContrl { get; set; }
        //机械臂夹爪
        public ArmClaw armClaw { get; set; }
        //机械臂灯光
        public ArmLed armLed { get; set; }
        public Core()
        {
            InitDevice();
            InitAssistant();
            InitArm();
        }


        //初始化设备
        public void InitDevice()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Parallel.For(2, 255, (i) =>
                    {
                        string ip = $"{ipBegin}{i}";
                        if (Devices.Exists(t => t.Ip == ip))
                            return;
                        //如果不存在该ip ,尝试连接
                        Ping pingSender = new Ping();
                        PingReply reply = pingSender.Send(ip, 500);
                        if (reply.Status != IPStatus.Success)
                        {
                            return;
                        }

                        Task.Run(() => {
                            TcpClient client = new TcpClient();
                            try
                            {
                                client.Connect(ip, 10086);
                                NetworkStream stream = client.GetStream();
                                byte[] buffer = new byte[1024];
                                int bytesRead;
                                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    if (bytesRead == 5)
                                    {
                                        int id = (int)buffer[1];
                                        switch (buffer[0])
                                        {
                                            case 1:
                                                var nl = new NightLightDevice(client, id, ip);
                                                nl.Init();
                                                Devices.Add(nl);
                                                break;
                                            case 2:
                                                var dl = new DeskLampDevice(client, id, ip);
                                                dl.Init();
                                                Devices.Add(dl);
                                                break;
                                            case 3:
                                                var fan = new FanDevice(client, id, ip);
                                                fan.Init();
                                                Devices.Add(fan);
                                                break;
                                            case 4:
                                                var wp = new WallPaintingDevice(client, id, ip);
                                                wp.Init();
                                                Devices.Add(wp);
                                                break;
                                            default:
                                                break;
                                        }

                                    }
                                }
                            }
                            catch (Exception)
                            {
                                return;
                            }
                            
                        });

                    });
                    Thread.Sleep(5000);
                }
            });

        }

        //初始化语音助手
        public void InitAssistant()
        {
            assistantSerial = new AssistantSerial();
        }

        //初始化机械臂
        private void InitArm()
        {
            armSerial = new ArmSerial();
            ArmConfig = new ArmConfig() { D_BASE = 0, L_BASE = 158.5, L_ARM = 148, D_ELBOW = 71, L_FOREARM = 94.345443, L_WRIST = 182 };
            armContrl = new ArmContrl(ArmConfig, armSerial);
            armClaw = new ArmClaw();
            Devices.Add(armClaw);
            armLed = new ArmLed();
            Devices.Add(armLed);
        }
    }
}
