using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroadcastReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() => new UdpBroadcastReceiver(new TeacherLoungeBroadcast()));
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
