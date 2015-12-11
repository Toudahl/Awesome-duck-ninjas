using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebApi.Controllers
{
    [Authorize]
    public class BroadcasterController : ApiController
    {
         private AzureDbConnection db = new AzureDbConnection();
        // GET: api/Broadcaster

        /// <summary>
        /// Get all broadcasters
        /// </summary>
         /// <returns>IEnumerable<Broadcaster></returns>
        public IEnumerable<Broadcaster> Get()
        {
            return db.Broadcasters;
        }

       
        // GET: api/Broadcaster/5
        /// <summary>
        /// Get broadcaste by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Broadcaster))]
        public IHttpActionResult Get(int id)
        {
            var broadcaster = db.Broadcasters.FirstOrDefault((i) => i.Id == id);
            if (broadcaster == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return Ok(broadcaster);
        }


        /// <summary>
        /// Get broadcaster with location, sensor type
        /// </summary>
        /// <param name="broadcasterId"></param>
        /// <returns>IEnumerable<SensorDTO></returns>
        [HttpGet]
        [Route("api/Broadcaster/{broadcasterId}/Sensors")]
        public IEnumerable<SensorDTO> GetBroadcasterSensors(int broadcasterId)
        {
            var broadcaster = db.Broadcasters.FirstOrDefault((i) => i.Id == broadcasterId);
            if (broadcaster == null) return null;

            List<SensorDTO> sensorsDTO = new List<SensorDTO>();
            var sensors = db.Sensors.Where((i) => i.Fk_Broadcaster == broadcasterId);
            foreach (Sensor sensor in sensors)
            {
                var sensorDTO = new SensorDTO();

                sensorDTO.Id = sensor.Id;
                sensorDTO.LocationName = db.Locations.FirstOrDefault((i) => i.Id == sensor.Fk_Location).Name;
                sensorDTO.SensorType =   db.SensorTypes.FirstOrDefault((i) => i.Id == sensor.Fk_SensorType).Type;
                sensorDTO.Broadcaster = broadcaster.Name;
                sensorsDTO.Add(sensorDTO);
            }
            return sensorsDTO;;
        }
    }
}
