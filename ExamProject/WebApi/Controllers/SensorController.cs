using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Helper;

namespace WebApi.Controllers
{
    [Authorize]
    public class SensorController : ApiController
    {
        private AzureDbConnection db = new AzureDbConnection();


        // GET: api/Sensor
        /// <summary>
        /// Get all sensors
        /// </summary>
        /// <returns>IQueryable<Sensor></returns>
        public IQueryable<Sensor> GetSensors()
        {
            return db.Sensors;
        }

        // GET: api/Sensor/5
        /// <summary>
        /// GetSensor by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Sensor))]
        public IHttpActionResult GetSensor(int id)
        {
            Sensor sensor = db.Sensors.Find(id);
            if (sensor == null)
            {
                return NotFound();
            }

            return Ok(sensor);
        }

        // PUT: api/Sensor/5
        /// <summary>
        /// Put sensor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sensor"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Post sensor
        /// </summary>
        /// <param name="sensor"></param>
        /// <returns>IHttpActionResult</returns>
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


        /// <summary>
        /// PostSensorByteData with byte[] sensorData
        /// </summary>
        /// <param name="sensorData"></param>
        /// <returns>IHttpActionResult</returns>
        [HttpPost]
        [Route("api/Sensor/postByte/")]
        public async Task<IHttpActionResult> PostSensorByteData(byte[] sensorData)
        {
            try
            {
                var parser = new SensorParser();
                return StatusCode(await parser.ParseInput(sensorData));
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }


        // DELETE: api/Sensor/5
        /// <summary>
        /// Deletesensor by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IHttpActionResult</returns>
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

        /// <summary>
        /// Check if sensor exists using sensor id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool sensorExists(int id)
        {
            return db.Sensors.Count(e => e.Id == id) > 0;
        }


        /// <summary>
        /// Get sensor with it's values using sensor id
        /// </summary>
        /// <param name="sensorId"></param>
        /// <returns>IQueryable<Value></returns>
        [HttpGet]
        [Route("api/Sensor/{sensorId}/Values/")]
        public IQueryable<Value> GetSensorsWithValues(int sensorId)
        {
            var sensor = db.Sensors.FirstOrDefault((i) => i.Id == sensorId);
            if (sensor == null) return null;
            var values = db.Values.Where((i) => i.FK_Sensor == sensorId);
            return values;
        }



        /// <summary>
        /// get all sensors as SensorDTO objects
        /// </summary>
        /// <returns>List<SensorDTO></returns>
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
        /// <summary>
        /// Filter sensor 
        /// </summary>
        /// <param name="sensorId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Get all information about sensor by id
        /// </summary>
        /// <param name="sensorId"></param>
        /// <returns></returns>
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
