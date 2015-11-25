using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SensorReader
{
    class ApiLink
    {
        private readonly string _apiPath;
        private const string ServerUrl = "https://awesomeduckninjas.azurewebsites.net/api/";

        public ApiLink(string apiPath)
        {
            _apiPath = apiPath;
        }

        #region PostAsJsonAsync(byte array)

        /// <summary>
        /// Using this method will create a new row in the database, in the table that matches the type of <see cref="T"/>
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns>The result of the attempted database query</returns>
        public async Task<HttpResponseMessage> PostAsJsonAsync(byte[] byteArray)
        {
            try
            {
                using (var client = GetClient())
                {
                    return await client.PostAsJsonAsync(ServerUrl + _apiPath, byteArray);
                }
            }
            catch (Exception e)
            {
                return FailedClient(e);
            }
        }
        #endregion

        #region GetClient()
        /// <summary>
        /// Instansiates the HttpClient that is used by all the methods in this class
        /// </summary>
        /// <returns>HttpClient</returns>
        private HttpClient GetClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = new TimeSpan(0, 0, 30);
            return client;
        }
        #endregion

        #region FailedClient
        /// <summary>
        /// Any method that returns a HttpResponseMessage will return this upon an exception
        /// </summary>
        /// <returns>HttpStatusCode.NoContent</returns>
        private HttpResponseMessage FailedClient(Exception e)
        {
            var response = new HttpResponseMessage();
            response.ReasonPhrase = e.Message;
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
        #endregion
    }
}
