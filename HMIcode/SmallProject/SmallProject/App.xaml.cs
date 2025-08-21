using SmallProject.Configs;
using SmallProject.Devices.Arm;
using SmallProject.Serials;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SmallProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static JConfiguration JConfig { get; set; }
        public static Core Core { get; set; }

        public App()
        {
            JConfig = ConfigResposity.ReadConfigs();
            Core = new Core();
        }
    }
}
