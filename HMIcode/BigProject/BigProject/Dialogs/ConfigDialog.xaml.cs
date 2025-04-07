using BigProject.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BigProject.Dialogs
{
    /// <summary>
    /// ConfigDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigDialog : Rubyer.RubyerWindow
    {
        public ConfigDialog()
        {
            InitializeComponent();
            //加载配置项
            this.DataContext = App.Core.ArmConfig;
            this.Loaded += ConfigDialog_Loaded;
        }

        //加载
        private void ConfigDialog_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void EnterOK_Click(object sender, RoutedEventArgs e)
        {
            var conf = App.Core.ArmConfig;
            ConfigResposity.WriteConfigs(conf);
            Rubyer.MessageBoxR.Success("保存成功。");
        }
    }
}
