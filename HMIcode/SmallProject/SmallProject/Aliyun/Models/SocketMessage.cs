using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SmallProject.Aliyun.Models
{
    internal class SocketMessage
    {
        public Header header { get; set; } = new Header();

        public Payload payload { get; set; } = new Payload();
    }

    public class Header
    {
        public string action { get; set; } = "run-task";

        public string task_id { get; set; } = Guid.NewGuid().ToString("N");

        public string streaming { get; set; } = "duplex";
    }

    public class Payload
    { 
        public string task_group { get; set; } = "aigc";

        public string task { get; set; } = "multimodal-generation";

        public string function { get; set; } = "generation";

        public string model { get; set; } = "multimodal-dialog";

        public Input input { get; set; } = new Input();

        public Parameters parameters { get; set; } = new Parameters();
    }

    public class Input
    {
        public string directive { get; set; } = "Start";

        public string workspace_id { get; set; }

        public string app_id { get; set; }

    }

    public class Parameters
    {
        public Upstream upstream { get; set; } = new Upstream();

        public Client_info client_info = new Client_info();
    }

    public class Upstream
    {
        public string type { get; set; } = "AudioOnly";

        public string mode { get; set; } = "duplex";
    }

    public class Client_info
    {
        public string user_id { get; set; } = Guid.NewGuid().ToString("N");
        public Device device { get; set; } = new Device();
    }

    public class Device
    {
        public string uuid { get; set; }
    }
}
