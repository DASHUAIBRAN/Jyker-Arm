using SmallProject.MCP.Models;
using SmallProject.MCP.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmallProject.MCP.Serializers
{
    internal class ToolsSerializer
    {


        public static List<Tool> SerializeTool<T>()
        {
            List<Tool> tools = new List<Tool>();
            Type type = typeof(T);
            // 获取所有公共实例方法
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(t=>t.DeclaringType==type);
            foreach (MethodInfo method in methods)
            {
                Tool tool = new Tool();
                // 获取方法的参数
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length > 0)
                {
                    foreach (ParameterInfo param in parameters)
                    {
                        
                        tool.InputSchema.Properties.Add(param.Name, new Property { Title = param.Name.ToUpper()
                            , Type = (param.ParameterType == typeof(int)? "integer":"text")
                        });
                    }
                    tool.InputSchema.Required = parameters.Select(t => t.Name).ToArray();
                }

                // 获取属性
                var desc = method.GetCustomAttribute<DescriptionAttribute>();


                tool.Name = method.Name;
                tool.Description = desc.Description;
                tool.InputSchema.Title = $"{method.Name}Arguments" ;
                tool.InputSchema.Type = "object";
                tools.Add(tool);
            }

            return tools;
        }
    }
}
