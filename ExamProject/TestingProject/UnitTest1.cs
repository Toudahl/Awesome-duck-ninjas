using System;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi;
using WebApi.Controllers;

namespace TestingProject
{
    [TestClass]
    public class UnitTest1
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
    }
}
