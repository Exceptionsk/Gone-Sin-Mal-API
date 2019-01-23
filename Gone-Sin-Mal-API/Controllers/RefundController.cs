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
using Gone_Sin_Mal_API;

namespace Gone_Sin_Mal_API.Controllers
{
    public class RefundController : ApiController
    {
        private Gone_Sin_MalEntities db = new Gone_Sin_MalEntities();

        // GET: api/Refund
        public IQueryable<Refund_Table> GetRefund_Table()
        {
            return db.Refund_Table;
        }

        // GET: api/Refund/5
        [ResponseType(typeof(Refund_Table))]
        public IHttpActionResult GetRefund_Table(long id)
        {
            Refund_Table refund_Table = db.Refund_Table.Find(id);
            if (refund_Table == null)
            {
                return NotFound();
            }

            return Ok(refund_Table);
        }

        // PUT: api/Refund/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRefund_Table(long id, Refund_Table refund_Table)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != refund_Table.ID)
            {
                return BadRequest();
            }

            db.Entry(refund_Table).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Refund_TableExists(id))
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

        // POST: api/Refund
        [ResponseType(typeof(Refund_Table))]
        public IHttpActionResult PostRefund_Table(Refund_Table refund_Table)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Refund_Table.Add(refund_Table);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = refund_Table.ID }, refund_Table);
        }

        // DELETE: api/Refund/5
        [ResponseType(typeof(Refund_Table))]
        public IHttpActionResult DeleteRefund_Table(long id)
        {
            Refund_Table refund_Table = db.Refund_Table.Find(id);
            if (refund_Table == null)
            {
                return NotFound();
            }

            db.Refund_Table.Remove(refund_Table);
            db.SaveChanges();

            return Ok(refund_Table);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Refund_TableExists(long id)
        {
            return db.Refund_Table.Count(e => e.ID == id) > 0;
        }
    }
}