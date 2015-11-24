using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi;
using WebApi.Controllers;

namespace TestingProject
{
     [TestClass]
    public class DataValuesTest
     {
         private data_valuesController dataController;
         [TestInitialize]
         public void TestInitialize()
         {
            dataController = new data_valuesController();
         }

         [TestMethod]
         public void GetData()
         {
             var res = dataController.Getdata_values();
             Assert.IsNotNull(res);
             Assert.AreEqual(3, res.Count());
             
             var sensor_id = res.First((i) => i.sensor_id == 1).id;
             Assert.AreEqual(1,sensor_id);
         }


         [TestMethod]
         public void GetDataForOneSensor()
         {
             var res = dataController.Getdata_values(1);
             var contentResult = res as OkNegotiatedContentResult<data_values>;

             Assert.IsNotNull(contentResult);
             Assert.IsNotNull(contentResult.Content);

             var dataValues = contentResult.Content.sensor.data_values;
             var dataValue = dataValues.First((i) => i.id == 1);

             Assert.AreEqual("Potentiometer",dataValue.sensor.name);
         }

         [TestMethod]
         public void CreateDataValues()
         {
             data_values data = new data_values {sensor_id = 1, value = "999"};
             var res = dataController.Postdata_values(data);
             Assert.IsNotNull(res);
         }

         [TestMethod]
         public void UpdateData()
         {
             data_values data = new data_values { sensor_id = 1, value = "889",id = 6};
             var res = dataController.Putdata_values(6, data);
             Assert.IsNotNull(res);
         }

         [TestMethod]
         public void UpdateDataCheckForValue()
         {
             data_values data = new data_values { sensor_id = 1, value = "789", id = 6 };
             var res = dataController.Putdata_values(6, data);
             Assert.IsNotNull(res);

             var contentResult = res as StatusCodeResult;

            // Debug.WriteLine(contentResult.StatusCode + " Code");
             Assert.AreEqual(HttpStatusCode.NoContent,contentResult.StatusCode);
         }

     }
}
