using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi;

namespace WebApi.Controllers
{
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

        public IHttpActionResult PostSensorByte(byte[] sensor)
        {
            throw new NotImplementedException();
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