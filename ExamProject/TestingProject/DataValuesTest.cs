using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using WebApi;
using WebApi.Controllers;

namespace TestingProject
{
    [TestClass]
    public class DataValuesTest
    {
        private ValuesController dataController;
        private ApiLink link;
        [TestInitialize]
        public void TestInitialize()
        {
            dataController = new ValuesController();
            link = new ApiLink("data_values");
        }

        [TestMethod]
        public void GetData()
        {
            var res = dataController.GetValues();
            Assert.IsNotNull(res);
            //Assert.AreEqual(120, res.Count());

            var dataId = res.First((i) => i.FK_Sensor == 2).Id;
            Assert.AreEqual(2, dataId);
        }


        [TestMethod]
        public void GetDataByUrl()
        {
            var result = link.GetAsync().Result;

            Assert.IsTrue(result.IsSuccessStatusCode);
        }


        [TestMethod]
        public void GetDataForOneSensor()
        {
            var res = dataController.GetValue(2);
            var contentResult = res as OkNegotiatedContentResult<Value>;
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);

            var sensorId = contentResult.Content.FK_Sensor;
            Assert.AreEqual(2, sensorId);
        }

        [TestMethod]
        public void GetDataForOneSensorUrl()
        {
            var result = link.GetAsync(2)
                             .Result;

            Assert.IsTrue(result.IsSuccessStatusCode);
            var stringResult = result.Content.ReadAsStringAsync().Result;
            Assert.IsNotNull(stringResult);

            var dataValue = JsonConvert.DeserializeObject<Value>(stringResult);

            Assert.AreEqual(2, dataValue.FK_Sensor);
        }

        [TestMethod]
        public void CreateDataValues()
        {
            Value data = new Value{ FK_Sensor = 2, ValueInput = "999",CreatedOn = DateTime.Now};
            var res = dataController.PostValue(data);
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void CreateDataValuesByUrl()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = client.PostAsJsonAsync("http://awesomeduckninjas.azurewebsites.net/api/data_values", new Value{ FK_Sensor = 2, ValueInput = "888",CreatedOn = DateTime.Now}).Result;
                Assert.IsTrue(result.IsSuccessStatusCode);
            }
        }

        [TestMethod]
        public void UpdateData()
        {
            Value data = new Value{FK_Sensor= 2, ValueInput = "889"};
            var res = dataController.PutValue(6, data);
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void UpdateDataCheckForValue()
        {
            Value data = new Value{ FK_Sensor= 2, ValueInput = "789"};
            var res = dataController.PutValue(62, data);
            Assert.IsNotNull(res);

            var contentResult = res as StatusCodeResult;

            // Debug.WriteLine(contentResult.StatusCode + " Code");
            Assert.AreEqual(HttpStatusCode.NoContent, contentResult.StatusCode);
        }

        [TestMethod]
        public void DataValuesValidRoute()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://awesomeduckninjas.azurewebsites.net/api/data_values");
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            config.EnsureInitialized();

            var result = config.Routes.GetRouteData(request);
            Assert.AreEqual("api/{controller}/{id}", result.Route.RouteTemplate);
        }

        [TestMethod]
        public void DataValuesValidRouteValues()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://awesomeduckninjas.azurewebsites.net/api/data_values/1");
            // can add headers and content 
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            config.EnsureInitialized();

            var result = config.Routes.GetRouteData(request);
            var res = result.Values.Values.ToList();

            Assert.AreEqual("data_values", res[0]);
            Assert.AreEqual("1", res[1]);
        }

    }
}
