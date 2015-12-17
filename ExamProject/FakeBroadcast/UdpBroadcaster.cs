using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FakeBroadcast
{
    class UdpBroadcaster
    {

        private int _port;
        private bool shouldRun = true;

        public UdpBroadcaster(int port)
        {
            _port = port;
        }

        public void Stop()
        {
            shouldRun = false;
        }

        public void Start()
        {
            var broadCastFromTeacherLounge = "RoomSensor Broadcasting\r\nLocation: Teachers room\r\nPlatform: Linux-3.12.28+-armv6l-with-debian-7.6\r\nMachine: armv6l\r\nPotentiometer(8bit): 129\r\nLight Sensor(8bit): 215\r\nTemperature(8bit): 212\r\nMovement last detected: 2015-11-09 14:07:49.396159\r\n";

            Task.Run(() =>
                    {
                        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Broadcast, _port);

                        using (UdpClient client = new UdpClient())
                        {
                            client.EnableBroadcast = true;
                            while(shouldRun)
                            {
                                Trace.TraceInformation("Broadcasting fake data:\n");
                                Trace.WriteLine(broadCastFromTeacherLounge);
                                byte[] data = Encoding.ASCII.GetBytes(broadCastFromTeacherLounge);
                                client.Send(data, data.Length, serverEndPoint);
                                Trace.TraceInformation("waiting 5 seconds before broadcasting again.\n\n");
                                Thread.Sleep(5 * 1000);
                            }
                        }
                    });
        }

    }
}
