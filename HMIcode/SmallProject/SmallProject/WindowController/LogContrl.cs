using SmallProject.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SmallProject.WindowController
{
    internal class LogContrl
    {
        private MainWindow M;

        public LogContrl(MainWindow m)
        {
            M = m;
            JLog.MessageEvent += Log_MessageEvent;
        }
        int richTextLine = 0;
        //打印日志到桌面
        private void Log_MessageEvent(string text)
        {
            try
            {
                var str = $"{DateTime.Now.ToString("HH:mm:ss")}:{text}\n";
                M.Dispatcher.Invoke(() =>
                {
                    M.tbLog.AppendText(str);
                    M.tbLog.ScrollToEnd();
                    if (richTextLine >= 3000)
                    {
                        M.tbLog.Text = string.Empty;
                        richTextLine = 0;
                    }
                    richTextLine++;
                });
            }
            catch (Exception)
            {

            }
        }
    }
}
