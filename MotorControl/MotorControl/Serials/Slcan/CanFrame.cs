using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorControl.Serials.Slcan
{
    public class CanFrame
    {
        public int Id { get; set; } = 0;
        public int Cmd { get; set; }
        public byte[] Data { get; set; } = new byte[0];
        public bool IsExtended { get; set; }
        public bool IsRtr { get; set; }
        public byte Dlc { get; set; } = 0;

        public override string ToString()
        {
            return $"ID: 0x{Id:X8}, DLC: {Dlc}, Data: {BitConverter.ToString(Data)}";
        }

        public string ToStr()
        {
            var IdCmd = (Id << 7 | Cmd).ToString("X3");
            var DlcStr = Dlc.ToString("X1");
            var DataStr = string.Concat(Data.Select(t => t.ToString("X2")));
            return $"t{IdCmd}{DlcStr}{DataStr}\r";
        }
    }
}
