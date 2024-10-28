using BigProject.Logger;
using BigProject.Serials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigProject.Devices.Arm
{
    public class ArmLed : BaseDevice
    {
        public ArmLed() {
            DeviceType = DeviceType.ArmLed;
            Log.Info($"机械臂灯{this.Id} 已连接");
        }

        //开启机械臂的灯
        public void Open()
        {
            App.Core.armSerial.LedOpen();
        }

        //关闭机械臂的灯
        public void Close()
        {
            App.Core.armSerial.LedClose();
        }
    }
}
