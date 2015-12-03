//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Web.Http;
//using System.Web.Http.Results;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Newtonsoft.Json;
//using WebApi;
//using WebApi.Controllers;

//namespace TestingProject
//{
//    [TestClass]
//    public class DataValuesTest
//    {
//        private old_data_valuesController dataController;
//        private ApiLink link;
//        [TestInitialize]
//        public void TestInitialize()
//        {
//            dataController = new old_data_valuesController();
//            link = new ApiLink("data_values");
//        }

//        [TestMethod]
//        public void GetData()
//        {
//            var res = dataController.Getdata_values();
//            Assert.IsNotNull(res);
//            //Assert.AreEqual(120, res.Count());

//            var data_id = res.First((i) => i.sensor_id == 292).id;
//            Assert.AreEqual(62, data_id);
//        }


//        [TestMethod]
//        public void GetDataByUrl()
//        {
//            var result = link.GetAsync().Result;

//            Assert.IsTrue(result.IsSuccessStatusCode);
//        }


//        [TestMethod]
//        public void GetDataForOneSensor()
//        {
//            var res = dataController.Getdata_values(62);
//            var contentResult = res as OkNegotiatedContentResult<data_values>;

//            Assert.IsNotNull(contentResult);
//            Assert.IsNotNull(contentResult.Content);

//            var dataValues = contentResult.Content.sensor.data_values;
//            var dataValue = dataValues.First((i) => i.id == 62);

//            Assert.AreEqual("Potentiometer(8bit)", dataValue.sensor.name);
//        }

//        [TestMethod]
//        public void GetDataForOneSensorUrl()
//        {
//            var result = link.GetAsync(62)
//                             .Result;

//            Assert.IsTrue(result.IsSuccessStatusCode);
//            var stringResult = result.Content.ReadAsStringAsync().Result;
//            Assert.IsNotNull(stringResult);

//            var dataValue = JsonConvert.DeserializeObject<data_values>(stringResult);

//                Assert.AreEqual("Potentiometer(8bit)", dataValue.sensor.name);
//        }

//        [TestMethod]
//        public void CreateDataValues()
//        {
//            data_values data = new data_values { sensor_id = 292, value = "999" };
//            var res = dataController.Postdata_values(data);
//            Assert.IsNotNull(res);
//        }

//        [TestMethod]
//        public void CreateDataValuesByUrl()
//        {
//            using(var client = new HttpClient())
//            {
//                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//                var result = client.PostAsJsonAsync("http://awesomeduckninjas.azurewebsites.net/api/data_values", new data_values { sensor_id = 292, value = "999" }).Result;
//                Assert.IsTrue(result.IsSuccessStatusCode);
//            }
//        }

//        [TestMethod]
//        public void UpdateData()
//        {
//            data_values data = new data_values { sensor_id = 1, value = "889", id = 6 };
//            var res = dataController.Putdata_values(6, data);
//            Assert.IsNotNull(res);
//        }

//        [TestMethod]
//        public void UpdateDataCheckForValue()
//        {
//            data_values data = new data_values { sensor_id = 292, value = "789", id = 62 };
//            var res = dataController.Putdata_values(62, data);
//            Assert.IsNotNull(res);

//            var contentResult = res as StatusCodeResult;

//            // Debug.WriteLine(contentResult.StatusCode + " Code");
//            Assert.AreEqual(HttpStatusCode.NoContent, contentResult.StatusCode);
//        }

//        [TestMethod]
//        public void DataValuesValidRoute()
//        {
//            var request = new HttpRequestMessage(HttpMethod.Get, "http://awesomeduckninjas.azurewebsites.net/api/data_values");
//            var config = new HttpConfiguration();
//            WebApiConfig.Register(config);
//            config.EnsureInitialized();

//            var result = config.Routes.GetRouteData(request);
//            Assert.AreEqual("api/{controller}/{id}", result.Route.RouteTemplate);
//        }

//        [TestMethod]
//        public void DataValuesValidRouteValues()
//        {
//            var request = new HttpRequestMessage(HttpMethod.Get, "http://awesomeduckninjas.azurewebsites.net/api/data_values/1");
//            // can add headers and content 
//            var config = new HttpConfiguration();
//            WebApiConfig.Register(config);
//            config.EnsureInitialized();

//            var result = config.Routes.GetRouteData(request);
//            var res = result.Values.Values.ToList();

//            Assert.AreEqual("data_values", res[0]);
//            Assert.AreEqual("1", res[1]);
//        }

//    }
//}
