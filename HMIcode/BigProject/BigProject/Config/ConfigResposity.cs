using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigProject.Config
{
    public class ConfigResposity
    {
        private static string DataDic = "./DATA";
        private static string ReadConfigFile = $"{DataDic}/Config.data";

        /// <summary>
        /// 读取产型数据
        /// </summary>
        /// <returns></returns>
        public static ConfigEntity ReadConfigs()
        {
            lock (DataDic)
            {
                if (!Directory.Exists(DataDic))
                {
                    Directory.CreateDirectory(DataDic);
                }
                if (!File.Exists(ReadConfigFile))
                {
                    using (File.Create(ReadConfigFile)) { }
                }
                var json = File.ReadAllText(ReadConfigFile, Encoding.UTF8);
                var conf = JsonConvert.DeserializeObject<ConfigEntity>(json);
                if (conf == null)
                {
                    return new ConfigEntity();
                }
                return conf;
            }

        }

        /// <summary>
        /// 写入产型数据
        /// </summary>
        /// <param name="productTypes"></param>
        public static void WriteConfigs(ConfigEntity conf)
        {
            lock (DataDic)
            {
                if (!Directory.Exists(DataDic))
                {
                    Directory.CreateDirectory(DataDic);
                }
                if (!File.Exists(ReadConfigFile))
                {
                    File.Create(ReadConfigFile);
                }
                using (FileStream fileStream = File.Create(ReadConfigFile)) //打开文件流
                {
                    var str = JsonConvert.SerializeObject(conf, Formatting.Indented); //序列化工程文件
                    byte[] by = ASCIIEncoding.UTF8.GetBytes(str); //把序列化的工程文件转成字节流
                    fileStream.Write(by, 0, by.Length); //字节流写入到文件
                }
            }

        }
    }
}
