using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BroadcastReceiver;

namespace SensorReader
{
    class Program
    {
        static void Main(string[] args)
        {

            var receiver = new UdpBroadcastReceiver(new TeacherLoungeBroadcast());
            Task.Run(() => receiver.ListenForBroadcast(7000));
            Console.WriteLine("Press any key to stop listening for UDP broadcasts");
            Console.ReadKey();
        }
    }

    internal class TeacherLoungeBroadcast : IBroadcastInterpreter
    {
        public void HandleBroadcast(byte[] input)
        {

        }
    }
}
