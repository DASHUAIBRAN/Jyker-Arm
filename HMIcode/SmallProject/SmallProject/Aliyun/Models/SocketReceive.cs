using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallProject.Aliyun.Models
{
    internal class SocketReceive
    {
        public ReceiveHeader header { get; set; } = new ReceiveHeader();

        public ReceivePayload payload { get; set; } = new ReceivePayload();
    }

    public class ReceiveHeader
    {
        public string @event { get; set; } = string.Empty;

        public string task_id { get; set; } = string.Empty;
    }

    public class ReceivePayload
    {
        public Output output { get; set; } = new Output();
        public Usage usage { get; set; } = new Usage();
    }

    public class Output
    {
        public string @event{ get; set; } = string.Empty;
        public string dialog_id { get; set; } = string.Empty;
        public string state { get; set; } = string.Empty;
    }

    public class Usage
    {
        public int invoke { get; set; } = 0;

        public int model_x { get; set; } = 0;
    }
}
