using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi;
using WebApi.Helper;

namespace WebApi.Controllers
{
    //[Authorize]
    public class sensorsController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/sensors
        public IQueryable<sensor> Getsensors()
        {
            return db.sensors;
        }

        // GET: api/sensors/5
        [ResponseType(typeof(sensor))]
        public IHttpActionResult Getsensor(int id)
        {
            sensor sensor = db.sensors.Find(id);
            if (sensor == null)
            {
                return NotFound();
            }

            return Ok(sensor);
        }

        // PUT: api/sensors/5
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

        // POST: api/sensors
        [ResponseType(typeof(sensor))]
        public IHttpActionResult Postsensor(sensor sensor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.sensors.Add(sensor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sensor.id }, sensor);
        }


        [HttpPost]
        [Route("api/sensors/postByte/")]
        [ResponseType(typeof(sensor))]
        public IHttpActionResult PostSensorByteData(byte[] sensorData)
        {
            var result = new ParseSensorData(sensorData);
            sensor sensor = null;

            try
            {
                foreach (var item in result.Sensors)
                {
                    sensor = db.sensors.FirstOrDefault((i) => i.name == item.Key);
                    if (sensor == null)
                    {
                        sensor = new sensor();
                        sensor.name = item.Key;
                        sensor.location = result.Keywords["Location"];
                        sensor.platform = result.Keywords["Platform"];
                        db.sensors.Add(sensor);
                    }

                    sensor.data_values.Add(new data_values {created_on = DateTime.Now, value = item.Value});
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.Source);
                return BadRequest("Request failed misserably");
            }
            return Ok("Well done");
        }


        // DELETE: api/sensors/5
        [ResponseType(typeof(sensor))]
        public IHttpActionResult Deletesensor(int id)
        {
            sensor sensor = db.sensors.Find(id);
            if (sensor == null)
            {
                return NotFound();
            }

            db.sensors.Remove(sensor);
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
            return db.sensors.Count(e => e.id == id) > 0;
        }
    }
}