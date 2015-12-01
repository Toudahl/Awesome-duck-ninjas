using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SensorReader
{
    class ApiLink
    {
        private readonly string _apiPath;
        private const string ServerUrl = "https://awesomeduckninjas.azurewebsites.net/api/";
        //private const string ServerUrl = "http://localhost:2326/api/";
        private TokenResponce _token;

        public ApiLink(string apiPath)
        {
            _apiPath = apiPath;
            GetToken("toudahl@gmail.com","Password1234.");
        }

        /// <summary>
        /// Gets the token to auth with the api.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        private async void GetToken(string userName, string password)
        {
            using (var client = new HttpClient())
            {
                var tokenUrl = ServerUrl.Replace("api/", "") + "token";
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                client.Timeout = new TimeSpan(0, 0, 30);

                var result = client.PostAsync(tokenUrl, new StringContent("username="+userName+"&password="+password+"&grant_type=password")).Result;
                if (result.IsSuccessStatusCode)
                {
                    _token = result.Content.ReadAsAsync<TokenResponce>().Result;
                    return;
                }
                // TODO: This exception cannot be caught. We need a fix for this.
                throw new ArgumentNullException("Failed to get auth token");
            }
        }

        #region TokenResponce class
        class TokenResponce
        {

            public string access_token
            {
                get;
                set;
            }

            public string token_type
            {
                get;
                set;
            }

            public int expires_in
            {
                get;
                set;
            }

            public string userName
            {
                get;
                set;
            }

        }
        #endregion


        #region PostAsJsonAsync(byte array)

        /// <summary>
        /// This will store send the byte array to the webapi
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
            //while(_token == null)
            //{
            //    Thread.Sleep(500);
            //}
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.access_token);

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
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            response.ReasonPhrase = e.Message;
            return response;
        }
        #endregion
    }
}
