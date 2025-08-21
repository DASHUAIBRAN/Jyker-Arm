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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmallProject.Dialogs
{
    /// <summary>
    /// MotorConfigDialog.xaml 的交互逻辑
    /// </summary>
    public partial class MotorConfigDialog : Rubyer.RubyerWindow
    {
        public MotorConfigDialog()
        {
            InitializeComponent();
        }

        //读取信息
        private void bt_Read_Click(object sender, RoutedEventArgs e)
        {

        }

        private void bt_EnterOK_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)tb_ID.Value;
            var value = (float)tb_Current.Value;
            App.Core.Jyker.SetLimitCurrent(id, value);
        }
    }
}
