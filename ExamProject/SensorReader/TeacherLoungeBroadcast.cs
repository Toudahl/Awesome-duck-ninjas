using System;
using System.Diagnostics;
using System.Text;
using BroadcastReceiver;
using Microsoft.SqlServer.Server;

namespace SensorReader
{
    internal class TeacherLoungeBroadcast : IBroadcastInterpreter
    {
        private readonly ApiLink link;

        public TeacherLoungeBroadcast()
        {
            try
            {
                link = new ApiLink("postsensorbytedata");
            }
            catch (ArgumentNullException argumentNullException)
            {
                Trace.TraceWarning(argumentNullException.Message);
            }
        }
        public async void HandleBroadcast(byte[] input)
        {
            Trace.TraceInformation("Got broadcast:");
            Trace.TraceInformation(Encoding.ASCII.GetString(input));
            try
            {
                var result = await link.PostAsJsonAsync(input);
                if (result.IsSuccessStatusCode)
                {
                    Trace.TraceInformation("Successfully posted the data to the api");
                }
                else
                    Trace.TraceWarning($"Posting the data failed with the following status code: {(int)result.StatusCode} ({result.StatusCode})");
            }
            catch (Exception ex)
            {
                
                Trace.TraceWarning("An exception of the type: " +ex.GetType() + "occured");
                Trace.TraceWarning(ex.Message);
            }
        }
    }
}