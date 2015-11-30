using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BroadcastReceiver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingProject
{
    [TestClass]
    public class TestingUDPReceiver
    {
        private const int BroadCastPort = 7000;
        private UdpTestBroadcaster testBroadcaster;


        [TestInitialize]
        public void Initializer()
        {
            testBroadcaster = new UdpTestBroadcaster(BroadCastPort);
            testBroadcaster.Start();
        }

        [TestMethod]
        public void VerifyThatBroadcastIsReceived()
        {
            var handler = new FakeHandler();

            UdpBroadcastReceiver receiver = new FakeReceiver(handler);

            handler.AddReceiver(receiver);
            receiver.ListenForBroadcast(BroadCastPort);
            Assert.IsTrue(true); // The test will never get here, unless a broadcast is received.
        }

        [TestCleanup]
        public void Cleaner()
        {
            testBroadcaster = null;
        }
    }

    class UdpTestBroadcaster
    {
        private int _port;

        public UdpTestBroadcaster(int port)
        {
            _port = port;
        }


        public void Start()
        {
            Task.Run(() =>
            {
                IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Broadcast, _port);

                using (UdpClient client = new UdpClient())
                {
                    int count = 0;
                    client.EnableBroadcast = true;
                    while (count < 10)
                    {
                        byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToString(new CultureInfo("da-dk")));
                        client.Send(data, data.Length, serverEndPoint);
                        count++;
                    }
                }
            });
        }

    }


    public class FakeHandler : IBroadcastInterpreter
    {
        private UdpBroadcastReceiver _receiver;


        public void HandleBroadcast(byte[] input)
        {
            if(input.Length > 0)
            {
                _receiver.StopListening();
            }
        }

        public void AddReceiver(UdpBroadcastReceiver receiver)
        {
            _receiver = receiver;
        }
    }

    class FakeReceiver : UdpBroadcastReceiver
    {
        public FakeReceiver(IBroadcastInterpreter interpreter) : base(interpreter)
        {
        }
    }
}
