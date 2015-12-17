using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeBroadcast
{
    class Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener
            {
                Filter = new EventTypeFilter(SourceLevels.All)
            });
            var broadcaster = new UdpBroadcaster(7000);
            broadcaster.Start();
            StopBroadcaster(broadcaster);
        }

        private static void StopBroadcaster(UdpBroadcaster broadcaster)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.S)
            {
                broadcaster.Stop();
                return;
            }
            StopBroadcaster(broadcaster);
        }
    }
}
