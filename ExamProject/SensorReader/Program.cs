using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            if (args.Length != 1)
            {
                Trace.WriteLine("Write the port you want to listen on.");
                return;
            }

            SetupTracing();

            int port = ParseArgsToInt(args[0]);

            if (!(port >= 0 && port <= 65535))
            {
                Trace.TraceWarning("Ports must be in the range 0-65535");
                return;
            }

            StartReciever(port);
            Console.WriteLine("Press any key to stop listening for UDP broadcasts");
            Console.ReadKey();
        }

        private static void StartReciever(int port)
        {
            var receiver = new UdpBroadcastReceiver(new TeacherLoungeBroadcast());
            Task.Run(() => receiver.ListenForBroadcast(port));
        }

        private static int ParseArgsToInt(string args)
        {
            try
            {
                return Convert.ToInt32(args);
            }
            catch(FormatException)
            {
                Trace.TraceInformation("input port number as integer. ie, 7000");
            }
            catch(Exception)
            {
                Trace.TraceWarning("Unexpected exception");
            }
            return -1;
        }

        private static void SetupTracing()
        {
            Trace.AutoFlush = true;
            Trace.Listeners.Add(new ConsoleTraceListener
                                {
                                    Filter = new EventTypeFilter(SourceLevels.All)
                                });
        }
    }
}
