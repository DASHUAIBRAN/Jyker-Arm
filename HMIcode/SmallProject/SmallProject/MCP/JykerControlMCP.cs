using Microsoft.Extensions.Hosting;
using SmallProject.MCP.Xiaozhi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmallProject.MCP.Tools;
using Newtonsoft.Json;
using SmallProject.MCP.Models;

namespace SmallProject.MCP
{
    internal class JykerControlMCP
    {
        public async Task Init()
        {
            McpPip pi = new McpPip();
            pi.Init();
        }

    }
}
