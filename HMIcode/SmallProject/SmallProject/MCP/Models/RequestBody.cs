using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallProject.MCP.Models
{
    public class RequestBody
    {
        public int Id { get; set; }
        public string Jsonrpc { get; set; }
        public string Method { get; set; }
        public Params Params { get; set; }
    }

    public class Params
    {
        public string ProtocolVersion { get; set; }
        public CapabilitiesD Capabilities { get; set; }
        public ClientInfo ClientInfo { get; set; }

        public string Name{get;set;}
        public Dictionary<string, object> Arguments { get; set; } = new Dictionary<string, object>();
    }

    public class CapabilitiesD
    {
        public Sampling Sampling { get; set; }
        public Roots Roots { get; set; }
    }

    public class Sampling
    {
        // sampling 是一个空对象 {}
    }

    public class Roots
    {
        public bool ListChanged { get; set; }
    }

    public class ClientInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
    }
}
