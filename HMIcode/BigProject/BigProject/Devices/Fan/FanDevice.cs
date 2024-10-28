using BigProject.Logger;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BigProject.Devices.Fan
{
    public class FanDevice : BaseDevice
    {
        private int PWM = 10;
        private int PWMLimit = 100;

        public FanDevice(TcpClient _client, int _id, string _ip)
        {
            this.Client = _client;
            this.Id = _id;
            this.Ip = _ip;
            DeviceType = DeviceType.NightLight;
            Log.Info($"风扇 {this.Id} 已连接");
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

        //打开风扇
        public void Open()
        {
            SendMsg(new byte[] { 0x00, 0x01, (byte)PWM });
        }

        //关闭风扇
        public void Close()
        {
            SendMsg(new byte[] { 0x00, 0x01, 0 });
        }

        //调大一点
        public void LevelUp()
        {
            PWM += 10;
            if(PWM> PWMLimit) PWM = PWMLimit;
            SendMsg(new byte[] { 0x00, 0x01, (byte)PWM });
        }

        //调小一点
        public void LevelDown()
        {
            PWM -= 10;
            if (PWM < 10) PWM = 10;
            SendMsg(new byte[] { 0x00, 0x01, (byte)PWM });
        }
    }
}
