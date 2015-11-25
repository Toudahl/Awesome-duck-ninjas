using BroadcastReceiver;

namespace SensorReader
{
    internal class TeacherLoungeBroadcast : IBroadcastInterpreter
    {
        private ApiLink link;

        public TeacherLoungeBroadcast()
        {
            link = new ApiLink("PostData");
        }
        public async void HandleBroadcast(byte[] input)
        {
            await link.PostAsJsonAsync(input);
        }
    }
}