using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BigProject.Devices
{
    public interface IDevice
    {
       
        void Init();
        void Loop();
    }

    public enum DeviceType
    {
        NightLight = 1,
        DeskLamp = 2,
        Fan = 3,
        WallPainting =4,
        ArmLed = 5,
        ArmClaw = 6,
        Arm = 7,
    }
}
