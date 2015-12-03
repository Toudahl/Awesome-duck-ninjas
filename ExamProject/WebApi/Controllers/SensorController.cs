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
using WebApi.EF;
using WebApi.Helper;

namespace WebApi.Controllers
{
    public class SensorController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/Sensor
        public IQueryable<Sensor> GetSensor()
        {
            return db.Sensors;
        }

        // GET: api/Sensor/5
        [ResponseType(typeof(sensor))]
        public IHttpActionResult Getsensor(int id)
        {
            Sensor sensor = db.Sensors.Find(id);
            if (sensor == null)
            {
                return NotFound();
            }

            return Ok(sensor);
        }

        // PUT: api/Sensor/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putsensor(int id, sensor sensor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sensor.id)
            {
                return BadRequest();
            }

            db.Entry(sensor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!sensorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Sensor
        [ResponseType(typeof(Sensor))]
        public IHttpActionResult Postsensor(Sensor sensor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sensors.Add(sensor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sensor.Id }, sensor);
        }


        [HttpPost]
        [Route("api/Sensor/postByte/")]
        [ResponseType(typeof(Sensor))]
        public IHttpActionResult PostSensorByteData(byte[] sensorData)
        {
            SensorParser parser = new SensorParser();

            parser.ParseInput(sensorData);
            var result = new ParseSensorData(sensorData);
            Sensor sensor = null;

        
            return Ok("Well done");
        }


        // DELETE: api/Sensor/5
        [ResponseType(typeof(Sensor))]
        public IHttpActionResult Deletesensor(int id)
        {
            Sensor sensor = db.Sensors.Find(id);
            if (sensor == null)
            {
                return NotFound();
            }

            db.Sensors.Remove(sensor);
            db.SaveChanges();

            return Ok(sensor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool sensorExists(int id)
        {
            return db.Sensors.Count(e => e.Id == id) > 0;
        }
    }
}
