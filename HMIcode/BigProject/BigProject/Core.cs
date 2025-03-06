using BigProject.Devices;
using BigProject.Devices.Arm;
using BigProject.Devices.Arm.Config;
using BigProject.JointMoveRecord;
using BigProject.Logger;
using BigProject.Serials;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        //机械臂助手
        public ArmSerial ArmSerial;
        //配置信息
        public ArmConfig ArmConfig { get; set; }
        //机械臂控制
        public ArmContrl ArmContrl { get; set; }
        //夹爪控制
        public ArmClaw ArmClaw {  get; set; }
        //轴移动记录
        public ObservableCollection<JointRecordModel> JointRecords = new ObservableCollection<JointRecordModel>();
        public Core()
        {

        }


        //初始化机械臂
        public bool InitArm(string comName)
        {
            ArmSerial = new ArmSerial(comName,out bool Resutl);
            if(!Resutl)
            {
                Log.Info($"串口{comName}连接失败");
                return false;
            }
            ArmConfig = new ArmConfig() { D_BASE = 0, L_BASE = 161.5, L_ARM = 170, D_ELBOW = 70, L_FOREARM = 117, L_WRIST = 97 };
            ArmContrl = new ArmContrl(ArmConfig, ArmSerial);
            ArmClaw = new ArmClaw(ArmSerial);
            return true;
        }

        public bool ArmDispose()
        {
            return ArmSerial.SerialDispose();
        }
    }
}
