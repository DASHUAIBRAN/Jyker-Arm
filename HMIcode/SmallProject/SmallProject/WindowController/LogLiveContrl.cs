using SmallProject.BiliBili;
using SmallProject.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallProject.WindowController
{
    internal class LogLiveContrl
    {
        private MainWindow M;

        public LogLiveContrl(MainWindow m)
        {
            M = m;
            JLog.MessageEventLive += Log_MessageEvent;

            BStationLive bStationLive = new BStationLive();
            bStationLive.Init();
        }
        int richTextLine = 0;
        //打印日志到桌面
        private void Log_MessageEvent(string text)
        {
            try
            {
                var str = $"{text}\n";
                M.Dispatcher.Invoke(() =>
                {
                    M.tbLogLive.AppendText(str);
                    M.tbLogLive.ScrollToEnd();
                    if (richTextLine >= 3000)
                    {
                        M.tbLogLive.Text = string.Empty;
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
