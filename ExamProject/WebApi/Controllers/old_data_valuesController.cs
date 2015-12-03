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
    public class old_data_valuesController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/data_values
        public IQueryable<data_values> Getdata_values()
        {
            return db.data_values;
        }

        // GET: api/data_values/5
        [ResponseType(typeof(data_values))]
        public IHttpActionResult Getdata_values(int id)
        {
            data_values data_values = db.data_values.Find(id);
            if (data_values == null)
            {
                return NotFound();
            }

            return Ok(data_values);
        }

        // PUT: api/data_values/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putdata_values(int id, data_values data_values)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != data_values.id)
            {
                return BadRequest();
            }

            db.Entry(data_values).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!data_valuesExists(id))
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

        // POST: api/data_values
        [ResponseType(typeof(data_values))]
        public IHttpActionResult Postdata_values(data_values data_values)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.data_values.Add(data_values);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = data_values.id }, data_values);
        }

        // DELETE: api/data_values/5
        [ResponseType(typeof(data_values))]
        public IHttpActionResult Deletedata_values(int id)
        {
            data_values data_values = db.data_values.Find(id);
            if (data_values == null)
            {
                return NotFound();
            }

            db.data_values.Remove(data_values);
            db.SaveChanges();

            return Ok(data_values);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool data_valuesExists(int id)
        {
            return db.data_values.Count(e => e.id == id) > 0;
        }
    }
}