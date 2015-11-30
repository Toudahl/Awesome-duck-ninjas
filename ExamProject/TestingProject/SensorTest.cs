using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UITest.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi;
using WebApi.Areas.HelpPage;
using WebApi.Controllers;

namespace TestingProject
{
    [TestClass]
    public class SensorTest
    {
        private sensorsController sensorController;

        [TestInitialize]
        public void TestInitialize()
        {
          sensorController = new sensorsController();
        }

        [TestMethod]
        public void GetAllSensors()
        {
            var result = sensorController.Getsensors();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetSingleSensor()
        {
            var result = sensorController.Getsensor(1);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetSingleSensorName()
        {
            var result = sensorController.Getsensors();
            var sensor = result.FirstOrDefault((i) => i.name == "Potentiometer");
            Assert.IsNotNull(sensor);
        }


        [TestMethod]
        public void SensorExists()
        {
            var result = sensorController.sensorExists(1);
            Assert.IsNotNull(result);
        }


        /*
         * Sensor can not have any data_values due to foreign key
         */

        [TestMethod]
        public void DeleteSensor()
        {
            var result = sensorController.Deletesensor(4);
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void CreateSensor()
        {
            var sensor = new sensor{id = 5,location = "Teachers Room",name = "Test sensor",platform = "Linux Debian"};
            var result = sensorController.Postsensor(sensor);
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void CreateSensorWithByteArray()
        {

            String value = "RoomSensor Broadcasting\r\nLocation: Teachers room\r\nPlatform: Linux-3.12.28+-armv6l-with-debian-7.6\r\nMachine: armv6l\r\nPotentiometer(8bit): 129\r\nLight Sensor(8bit): 215\r\nTemperature(8bit): 212\r\nMovement last detected: 2015-11-09 14:07:49.396159\r\n";
            var byteArray = Encoding.UTF8.GetBytes(value);
            var result = sensorController.PostSensorByteData(byteArray);
            Assert.IsNotNull(result);
        }


        [TestMethod]
        //public void CreateSensorWithUrl()
        //{

        //    String value = "RoomSensor Broadcasting\r\nLocation: Teachers room\r\nPlatform: Linux-3.12.28+-armv6l-with-debian-7.6\r\nMachine: armv6l\r\nPotentiometer(8bit): 129\r\nLight Sensor(8bit): 215\r\nTemperature(8bit): 212\r\nMovement last detected: 2015-11-09 14:07:49.396159\r\n";
        //    var byteArray = Encoding.UTF8.GetBytes(value);
        //    var request = (HttpWebRequest)WebRequest.Create("https://localhost:43001/api/sensors/postBytes");

        //    request.Method = "POST";
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    request.ContentLength = byteArray.Length;

        //    using (var stream = request.GetRequestStream())
        //    {
        //        stream.Write(byteArray, 0, byteArray.Length);
        //    }

        //    var response = (HttpWebResponse)request.GetResponse();
        //    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        //}

        public void CreateSensorWithUrl()
        {

            String value = "RoomSensor Broadcasting\r\nLocation: Teachers room\r\nPlatform: Linux-3.12.28+-armv6l-with-debian-7.6\r\nMachine: armv6l\r\nPotentiometer(8bit): 129\r\nLight Sensor(8bit): 215\r\nTemperature(8bit): 212\r\nMovement last detected: 2015-11-09 14:07:49.396159\r\n";
            var byteArray = Encoding.UTF8.GetBytes(value);
          //  var uri = "http://localhost:2326/api/sensors/postByte";
            var uri = "https://awesomeduckninjas.azurewebsites.net/api/sensors/postByte";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.PostAsJsonAsync(uri, byteArray).Result;
            Assert.IsNotNull(response.EnsureSuccessStatusCode());
        }


        [TestMethod]
        public void UpdateSensor()
        {
            var sensor = new sensor { id = 5, location = "Teachers Room Update", name = "Test sensor", platform = "Linux Debian" };
            var result = sensorController.Putsensor(5,sensor);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SensorValidRoute()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:2326/api/sensors/1");
            // can add headers and content 
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            config.EnsureInitialized();

            var result = config.Routes.GetRouteData(request);
            Assert.AreEqual("api/{controller}/{id}", result.Route.RouteTemplate);
        }


        [TestMethod]
        public void SensorValidRouteValues()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:2326/api/sensors/1");
            // can add headers and content 
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            config.EnsureInitialized();

            var result = config.Routes.GetRouteData(request);
            var res = result.Values.Values.ToList();

            Assert.AreEqual("sensors",res[0]);
            Assert.AreEqual("1",res[1]);
        }
    }
}

