using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.EF;

namespace WebApi.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/data_values
        public IQueryable<Value> Getdata_values()
        {
            return db.Values;
        }

        // GET: api/data_values/5
        [ResponseType(typeof(Value))]
        public IHttpActionResult Getdata_values(int id)
        {
            Value data_values = db.Values.Find(id);
            if (data_values == null)
            {
                return NotFound();
            }

            return Ok(data_values);
        }

        // PUT: api/data_values/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutValue(int id, Value values)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != values.Id)
            {
                return BadRequest();
            }

            db.Entry(values).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValueExists(id))
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
        [ResponseType(typeof(Value))]
        public IHttpActionResult PostValue(Value value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Values.Add(value);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = value.Id }, value);
        }

        // DELETE: api/data_values/5
        [ResponseType(typeof(Value))]
        public IHttpActionResult DeleteValue(int id)
        {
            Value values = db.Values.Find(id);
            if (values == null)
            {
                return NotFound();
            }

            db.Values.Remove(values);
            db.SaveChanges();

            return Ok(values);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ValueExists(int id)
        {
            return db.Values.Count(e => e.Id == id) > 0;
        }
    }
}
