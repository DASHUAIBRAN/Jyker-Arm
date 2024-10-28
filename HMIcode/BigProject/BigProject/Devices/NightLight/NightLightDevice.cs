using BigProject.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BigProject.Devices.NightLight
{
    public class NightLightDevice:BaseDevice
    {
        public NightLightDevice(TcpClient _client, int _id, string _ip) {
            this.Client = _client;
            this.Id = _id;
            this.Ip = _ip;
            DeviceType = DeviceType.NightLight;
            Log.Info($"小灯 {this.Id} 已连接");
        }

        public void Init()
        {
            //Task.Run(() => {
            //    NetworkStream stream = Client.GetStream();
            //    byte[] buffer = new byte[1024];
            //    int bytesRead;
            //    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            //    {
            //        Accep(buffer.Take(bytesRead).ToArray());
            //    }
            //});


        }

        public void Loop()
        {
        }

        //获取传入数据
        public void Accep(byte[] buffer)
        {

        }

        //打开小灯
        public void Open()
        {
            SendMsg(new byte[] { 0x00, 0x02, 0 });
        }

        //关闭小灯
        public void Close()
        {
            SendMsg(new byte[] { 0x00, 0x01, 0 });
        }
    }
}
