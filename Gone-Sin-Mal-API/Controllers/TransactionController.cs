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
    public class TransactionController : ApiController
    {
        private Gone_Sin_MalEntities db = new Gone_Sin_MalEntities();

        // GET: api/Transaction
        public IQueryable<Transaction_Table> GetTransaction_Table()
        {
            return db.Transaction_Table;
        }

        // GET: api/Transaction/5
        [ResponseType(typeof(Transaction_Table))]
        public IHttpActionResult GetTransaction_Table(long id)
        {
            Transaction_Table transaction_Table = db.Transaction_Table.Find(id);
            if (transaction_Table == null)
            {
                return NotFound();
            }

            return Ok(transaction_Table);
        }

        // PUT: api/Transaction/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTransaction_Table(long id, Transaction_Table transaction)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //if (id != transaction_Table.ID)
            //{
            //    return BadRequest();
            //}

            db.Entry(transaction).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Transaction_TableExists(id))
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

        // POST: api/Transaction
        [ResponseType(typeof(Transaction_Table))]
        public IHttpActionResult PostTransaction_Table(Transaction_Table transaction)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            db.Transaction_Table.Add(transaction);
            db.SaveChanges();

            return Ok(transaction);
        }

        // DELETE: api/Transaction/5
        [ResponseType(typeof(Transaction_Table))]
        public IHttpActionResult DeleteTransaction_Table(long id)
        {
            Transaction_Table transaction_Table = db.Transaction_Table.Find(id);
            if (transaction_Table == null)
            {
                return NotFound();
            }

            db.Transaction_Table.Remove(transaction_Table);
            db.SaveChanges();

            return Ok(transaction_Table);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Transaction_TableExists(long id)
        {
            return db.Transaction_Table.Count(e => e.ID == id) > 0;
        }
    }
}