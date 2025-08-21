using SmallProject.Aliyun;
using SmallProject.Configs;
using SmallProject.Devices.Arm;
using SmallProject.Serials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmallProject
{
    public class Core
    {

        public string ViewName = "";

        public Core() {
            Serial = new Serial();
            Jyker = new JykerArm();
            

            //定时任务
            Task.Run(() =>
            {
                while (true)
                {
                    Loop?.Invoke();
                    Thread.Sleep(20);
                }
            });
            
        }

        public Serial Serial { get; set; }

        public JykerArm Jyker { get; set; }

        public event Action Loop;

    }
}
