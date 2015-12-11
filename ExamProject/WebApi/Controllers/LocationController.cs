using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebApi.Controllers
{
    [Authorize]
    public class LocationController : ApiController
    {
        Model1 db = new Model1();
        // GET: api/Location
        
        /// <summary>
        /// Get all locations
        /// </summary>
        /// <returns>IEnumerable<Location></returns>
        public IEnumerable<Location> Get()
        {
            return db.Locations;
        }

        // GET: api/Location/5
        /// <summary>
        /// Get location by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Location))]
        public IHttpActionResult Get(int id)
        {
            var location = db.Locations.First((i) => i.Id == id);
            if (location == null) return BadRequest();
            return Ok(location);
        }

        // POST: api/Location
        /// <summary>
        /// Post location
        /// </summary>
        /// <param name="location"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Location))]
        public IHttpActionResult Post(Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Locations.Add(location);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = location.Id }, location);
   
        }

        // PUT: api/Location/5
        /// <summary>
        /// Put location 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="location"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Location))]
        public IHttpActionResult Put(int id, Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != location.Id)
            {
                return BadRequest();
            }

            db.Entry(location).State = EntityState.Modified;
            db.SaveChanges();

            return StatusCode(HttpStatusCode.OK);
        }

        // DELETE: api/Location/5
        /// <summary>
        /// Delete location
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult Delete(int id)
        {
            var location = db.Locations.First((i) => i.Id == id);
            if (location == null) return BadRequest("Location with Id " + id + " not found");

            try
            {
                db.Locations.Remove(location);
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
