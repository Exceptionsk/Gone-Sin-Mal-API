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
    public class PromotionController : ApiController
    {
        private Gone_Sin_MalEntities db = new Gone_Sin_MalEntities();

        // GET: api/Promotion
        public IQueryable<Promotion_Table> GetPromotion_Table()
        {
            return db.Promotion_Table;
        }

        // GET: api/Promotion/5
        [ResponseType(typeof(Promotion_Table))]
        public IHttpActionResult GetPromotion_Table(long id)
        {
            var promotion_Table = (from r in db.Restaurant_Table
                     join p in db.Promotion_Table
                     on r.Rest_id equals p.Rest_id
                     where p.User_id==id
                     select new {
                         p.Id,
                         p.User_promotion_amount,
                         r.Rest_id,
                         r.Rest_name,
                     });
            if (promotion_Table == null)
            {
                return NotFound();
            }

            return Ok(promotion_Table);
        }

        // PUT: api/Promotion/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPromotion_Table(long id, Promotion_Table promotion_Table)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != promotion_Table.Rest_id)
            {
                return BadRequest();
            }

            db.Entry(promotion_Table).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Promotion_TableExists(id))
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

        // POST: api/Promotion
        [ResponseType(typeof(Promotion_Table))]
        public IHttpActionResult PostPromotion_Table(Promotion_Table promotion_Table)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Promotion_Table.Add(promotion_Table);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (Promotion_TableExists(promotion_Table.Rest_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = promotion_Table.Rest_id }, promotion_Table);
        }

        // DELETE: api/Promotion/5
        [ResponseType(typeof(Promotion_Table))]
        public IHttpActionResult DeletePromotion_Table(long id)
        {
            Promotion_Table promotion_Table = db.Promotion_Table.Find(id);
            if (promotion_Table == null)
            {
                return NotFound();
            }

            db.Promotion_Table.Remove(promotion_Table);
            db.SaveChanges();

            return Ok(promotion_Table);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Promotion_TableExists(long id)
        {
            return db.Promotion_Table.Count(e => e.Rest_id == id) > 0;
        }
    }
}