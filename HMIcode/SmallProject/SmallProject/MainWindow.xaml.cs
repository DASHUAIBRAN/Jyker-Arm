using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Drawing;
using Rubyer;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using SmallProject.WindowController;
using SmallProject.Dialogs;

namespace SmallProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Rubyer.RubyerWindow
    {

        JykerConnetContrl JykerConnetContrl;
        JykerMoveContrl JykerMoveContrl;
        LogContrl LogContrl;
        JykerStatusContrl JykerStatusContrl;
        JykerKinematicContrl JykerKinematicContrl;
        JykerViewContrl JykerViewContrl;
        LogLiveContrl LogLiveContrl;
        public MainWindow()
        {
            InitializeComponent();
            //窗体加载
            this.Loaded += MainWindow_Loaded;
            //mt_Config 事件监听
            mt_Config.Click += Mt_Config_Click;
            mt_MotorConfig.Click += Mt_MotorConfig_Click;

        }
        //弹出电机配置页面
        private void Mt_MotorConfig_Click(object sender, RoutedEventArgs e)
        {
            MotorConfigDialog motorConfigDialog = new MotorConfigDialog();
            motorConfigDialog.ShowDialog();
        }

        //弹出配置页面
        private void Mt_Config_Click(object sender, RoutedEventArgs e)
        {
            ConfigDialog configDialog = new ConfigDialog();
            configDialog.ShowDialog();
        }

        

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            icon();

            //ContextMenuStrip
            contextMenu();

            //保证窗体显示在上方。
            wsl = WindowState;

            Rubyer.ThemeManager.SwitchThemeMode(Rubyer.Enums.ThemeMode.Dark);
            this.StateChanged += MainWindow_StateChanged;

            //初始化控制器
            JykerConnetContrl = new JykerConnetContrl(this);
            LogContrl = new LogContrl(this);
            JykerMoveContrl = new JykerMoveContrl(this);
            JykerStatusContrl = new JykerStatusContrl(this);
            JykerKinematicContrl = new JykerKinematicContrl(this);
            JykerViewContrl = new JykerViewContrl(this);
            LogLiveContrl = new LogLiveContrl(this);
        }


        #region 托盘右键菜单
        WindowState ws;
        WindowState wsl;
        NotifyIcon notifyIcon;
        private void MainWindow_StateChanged(object? sender, EventArgs e)
        {
            ws = this.WindowState;
            if (ws == WindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void icon()
        {
            string path = System.IO.Path.GetFullPath(@"Static\icon.ico");
            if (File.Exists(path))
            {
                this.notifyIcon = new NotifyIcon();
                this.notifyIcon.BalloonTipText = "Jyker"; //设置程序启动时显示的文本
                this.notifyIcon.Text = "Jyker";//最小化到托盘时，鼠标点击时显示的文本
                System.Drawing.Icon icon = new System.Drawing.Icon(path);//程序图标
                this.notifyIcon.Icon = icon;
                this.notifyIcon.Visible = true;
                notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick; 

            }

        }

        private void NotifyIcon_MouseDoubleClick(object? sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.Show();
            WindowState = wsl;
        }
        private void contextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();

            //关联 NotifyIcon 和 ContextMenuStrip
            notifyIcon.ContextMenuStrip = cms;

            System.Windows.Forms.ToolStripMenuItem exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exitMenuItem.Text = "退出";
            exitMenuItem.Click += new EventHandler(exitMenuItem_Click);

            System.Windows.Forms.ToolStripMenuItem hideMenumItem = new System.Windows.Forms.ToolStripMenuItem();
            hideMenumItem.Text = "隐藏";
            hideMenumItem.Click += new EventHandler(hideMenumItem_Click);

            System.Windows.Forms.ToolStripMenuItem showMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            showMenuItem.Text = "显示";
            showMenuItem.Click += new EventHandler(showMenuItem_Click);

            cms.Items.Add(exitMenuItem);
            cms.Items.Add(hideMenumItem);
            cms.Items.Add(showMenuItem);
        }

        private void exitMenuItem_Click(object? sender, EventArgs e)
        {
            notifyIcon.Visible = false;

            System.Windows.Application.Current.Shutdown();
        }

        private void hideMenumItem_Click(object? sender, EventArgs e)
        {
            this.Hide();
        }

        private void showMenuItem_Click(object? sender, EventArgs e)
        {
            this.Show();
            this.Activate();
        }
        #endregion
    }
} 
