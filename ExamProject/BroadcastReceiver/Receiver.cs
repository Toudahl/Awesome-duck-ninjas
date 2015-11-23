namespace BroadcastReceiver
{
    class Receiver : UdpBroadcastReceiver
    {
        public Receiver(IBroadcastInterpreter interpreter) : base(interpreter)
        {
        }
    }
}