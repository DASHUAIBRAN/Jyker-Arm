using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigProject.Devices.Arm
{
    public class ArmClaw:BaseDevice
    {
        //开启夹爪
        public void Open()
        {
            App.Core.armSerial.CtrClaw(40,140,40);
        }

        //关闭夹爪
        public void Close()
        {
            App.Core.armSerial.CtrClaw(80, 110, 80);
        }
    }
}
