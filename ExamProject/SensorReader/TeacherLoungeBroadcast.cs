using System;
using System.Diagnostics;
using BroadcastReceiver;
using Microsoft.SqlServer.Server;

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
            try
            {
                var result = await link.PostAsJsonAsync(input);
                if (result.IsSuccessStatusCode)
                {
                    Trace.TraceInformation("Successfully posted the data to the api");
                }
                else
                    Trace.TraceWarning("Posting the data failed with the following status code: " + result.StatusCode);
            }
            catch (Exception ex)
            {
                
                Trace.TraceWarning("An exception of the type: " +ex.GetType() + "occured");
                Trace.TraceWarning(ex.Message);
            }
        }
    }
}