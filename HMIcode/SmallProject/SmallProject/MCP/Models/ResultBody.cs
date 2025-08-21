using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallProject.MCP.Models
{
    public class ResultBody
    {
        public string jsonrpc { get; set; } = "2.0";
        public int id { get; set; }
        public dynamic result { get; set; }

        public bool isError { get; set; } = false;
    }

    public class ToolRoot
    {
        public List<Tool> Tools { get; set; } = new List<Tool>();
    }

    public class Tool
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public InputSchema InputSchema { get; set; } = new InputSchema();
    }

    public class InputSchema
    {
        public Dictionary<string, Property> Properties { get; set; } = new Dictionary<string, Property>();
        public string[] Required { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
    }

    public class Property
    {
        public string Title { get; set; }
        public string Type { get; set; }
    }

    public class ContentRoot
    {
        public List<ResultText> content = new List<ResultText>();
    }
    public class ResultText
    {
        public string type { get; set; } = "text";

        public string text { get; set; }
    }

    public class ResultContent
    {
        public bool success { get; set; } = true;

        public object result { get; set; }
    }


}
