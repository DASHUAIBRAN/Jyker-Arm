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
    }
}
