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
        //[ResponseType(typeof(Sensor))]
        //public IHttpActionResult Getsensor(int id)
        //{
        //    Sensor sensor = db.Sensors.Find(id);
        //    if (sensor == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(sensor);
        //}

        // PUT: api/Sensor/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putsensor(int id, Sensor sensor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sensor.Id)
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
        public IHttpActionResult PostSensorByteData(byte[] sensorData)
        {
            SensorParser parser = new SensorParser();

            try
            {
                parser.ParseInput(sensorData);
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            var result = new ParseSensorData(sensorData);
            //Sensor sensor = null;

        
            return StatusCode(HttpStatusCode.Accepted);
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

        [HttpGet]
        [Route("api/Sensor/{sensorId}/Values/")]
        public IQueryable<Value> GetSensorsWithValues(int sensorId)
        {
            var sensor = db.Sensors.FirstOrDefault((i) => i.Id == sensorId);
            if (sensor == null) return null;
            var values = db.Values.Where((i) => i.FK_Sensor == sensorId);
            return values;
        }



        [HttpGet]
        [Route("api/Sensor/All")]
        public List<SensorDTO> GetAllSensorsInfo()
        {
            var sensors = db.Sensors;
            if (sensors == null) return null;

            List<SensorDTO> sensorsDTO = new List<SensorDTO>();

            foreach (Sensor sensor in sensors)
            {
                var location = db.Locations.FirstOrDefault((i) => i.Id == sensor.Fk_Location);
                var sensorType = db.SensorTypes.FirstOrDefault((i) => i.Id == sensor.Fk_SensorType);
                var values = db.Values.Where((i) => i.FK_Sensor == sensor.Id);
                var broadcaster = db.Broadcasters.First((i) => i.Id == sensor.Fk_Broadcaster).Name;

                var sensorDTO = new SensorDTO();
                sensorDTO.Id = sensor.Id;
                sensorDTO.LocationName = location.Name;
                sensorDTO.SensorType = sensorType.Type;
                sensorDTO.Values = values;
                sensorDTO.Broadcaster = broadcaster;
                sensorsDTO.Add(sensorDTO);
            }

            return sensorsDTO;
        }


        //usage http://awesomeduckninjas.azurewebsites.net/api/Sensor/FilterSensorData/?sensorId=2&fromDate=2015-10-10&toDate=2015-12-01
        [HttpGet]
        [Route("api/Sensor/FilterSensorData")]
        public SensorDTO FilterSensorByDates(int sensorId, DateTime ? fromDate = null, DateTime ? toDate = null)
        {
            var sensor = db.Sensors.First((i) => i.Id == sensorId);
            if (sensor == null) return null;

            var broadcaster = db.Broadcasters.First((i) => i.Id == sensor.Fk_Broadcaster).Name;
            var location = db.Locations.First((i) => i.Id == sensor.Fk_Location).Name;
            var sensorType = db.SensorTypes.First((i) => i.Id == sensor.Fk_SensorType).Type;

            var sensorDTO = new SensorDTO();
            sensorDTO.Id = sensor.Id;
            sensorDTO.Broadcaster = broadcaster;
            sensorDTO.LocationName = location;
            sensorDTO.SensorType = sensorType;

            var tempValues = db.Values.Where((i) => i.FK_Sensor == sensor.Id);
            List<Value> values = new List<Value>();
            if (toDate == null) toDate = DateTime.MaxValue;
            if (fromDate == null) fromDate = DateTime.MinValue;

            foreach (Value val in tempValues)
            {
                DateTime parsedTime = new DateTime();
                DateTime.TryParse(val.ValueInput, out parsedTime);
                if (parsedTime >= fromDate && parsedTime <= toDate)
                {
                    values.Add(val);
                }
            }
            sensorDTO.Values = values.AsQueryable();
            return sensorDTO;
        }


        [HttpGet]
        [Route("api/Sensor/{sensorId}/All")]
        public SensorDTO GetAllSensorInfo(int sensorId)
        {
            var sensor = db.Sensors.FirstOrDefault((i) => i.Id == sensorId);
            if (sensor == null) return null;

            var location = db.Locations.FirstOrDefault((i) => i.Id == sensor.Fk_Location);
            var sensorType = db.SensorTypes.FirstOrDefault((i) => i.Id == sensor.Fk_SensorType);
            var values = db.Values.Where((i) => i.FK_Sensor == sensor.Id);
            var broadcaster = db.Broadcasters.First((i) => i.Id == sensor.Fk_Broadcaster).Name;

            var sensorDTO = new SensorDTO();
            sensorDTO.Id = sensor.Id;
            sensorDTO.LocationName = location.Name;
            sensorDTO.SensorType = sensorType.Type;
            sensorDTO.Values = values;
            sensorDTO.Broadcaster = broadcaster;

            return sensorDTO;
        }

    }
}
