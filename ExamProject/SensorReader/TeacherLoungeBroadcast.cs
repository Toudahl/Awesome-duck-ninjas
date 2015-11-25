using BroadcastReceiver;

namespace SensorReader
{
    internal class TeacherLoungeBroadcast : IBroadcastInterpreter
    {
        private readonly ApiLink link;

        public TeacherLoungeBroadcast()
        {
            link = new ApiLink("postsensorbytedata");
        }
        public async void HandleBroadcast(byte[] input)
        {
            await link.PostAsJsonAsync(input);
        }
    }
}