using BigProject.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BigProject.Devices
{
    public class BaseDevice
    {
        public DeviceType DeviceType { get; set; }
        public int Id { get; set; }
        public string Ip { get; set; }

        public TcpClient Client { get; set; }

        //向wifi 传递数据
        public void SendMsg(byte[] arr)
        {
            try
            {
                Client.Client.Send(arr);
            }
            catch (Exception e)
            {
                //传输失败
                Log.Error(e);
                //移除传输失败led，等待再次连接
                App.Core.Devices.Remove(this);
            }
        }
    }
}
