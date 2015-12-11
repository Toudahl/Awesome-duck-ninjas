using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebApi.Controllers
{
    [Authorize]
    public class SensorTypeController : ApiController
    {
        private AzureDbConnection db = new AzureDbConnection();

        // GET: api/SensorType
        /// <summary>
        /// Get all sensorsTypes
        /// </summary>
        /// <returns>IEnumerable<SensorType></returns>
        public IEnumerable<SensorType> Get()
        {
           return db.SensorTypes;
        }

        // GET: api/SensorType/5
        /// <summary>
        /// Get sensorType by sensorTypeId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(SensorType))]
        public IHttpActionResult Get(int id)
        {
            var sensorType = db.SensorTypes.FirstOrDefault((i) => i.Id == id);

            if (sensorType == null)
            {
                return NotFound();
            }
            return Ok(sensorType);
        }

        // POST: api/SensorType
        /// <summary>
        /// Post sensorType 
        /// </summary>
        /// <param name="sensorType"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(SensorType))]
        public IHttpActionResult Post(SensorType sensorType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SensorTypes.Add(sensorType);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sensorType.Id }, sensorType);
        }

        // PUT: api/SensorType/5
        /// <summary>
        /// Put sensorType by id 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sensorType"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult Put(int id, SensorType sensorType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sensorType.Id)
            {
                return BadRequest();
            }

            db.Entry(sensorType).State = EntityState.Modified;
            db.SaveChanges();
         
            return StatusCode(HttpStatusCode.OK);
        }

        // DELETE: api/SensorType/5

        /// <summary>
        /// Delete sensorType by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(SensorType))]
        public IHttpActionResult Delete(int id)
        {
            var sensorType = db.SensorTypes.First((i) => i.Id == id);
            if(sensorType == null) return BadRequest("SensorType with Id "+id+" not found");

            try
            {
                db.SensorTypes.Remove(sensorType);
                db.SaveChanges();
                return StatusCode(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                Trace.WriteLine(e.Source);
                return StatusCode(HttpStatusCode.NotModified);
            }
        }
    }
}
