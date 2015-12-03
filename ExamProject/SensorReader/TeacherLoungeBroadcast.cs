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
                link = new ApiLink("sensor/postbyte");
            }
            catch (ArgumentNullException argumentNullException)
            {
                Trace.TraceWarning(argumentNullException.Message);
            }
        }

        /// <summary>
        /// Will attempt to send the data to the webapi.
        /// Will notify of status.
        /// </summary>
        /// <param name="input">The byte array to send.</param>
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
                    Trace.TraceWarning("Posting the data failed");
            }
            catch (Exception ex)
            {
                
                Trace.TraceWarning("An exception of the type: " +ex.GetType() + "occured");
                Trace.TraceWarning(ex.Message);
            }
        }
    }
}