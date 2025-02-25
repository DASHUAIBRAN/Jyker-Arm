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
        }

        //开启机械臂的灯
        public void Open()
        {
            App.Core.ArmSerial.LedOpen();
        }

        //关闭机械臂的灯
        public void Close()
        {
            App.Core.ArmSerial.LedClose();
        }
    }
}
