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
using Gone_Sin_Mal_API.Class;

namespace Gone_Sin_Mal_API.Controllers
{
    public class RefundController : ApiController
    {
        private Gone_Sin_MalEntities db = new Gone_Sin_MalEntities();

        // GET: api/Refund
        public IHttpActionResult GetRefund_Table()
        {
            return Ok((from n in db.Refund_Table
                       join r in db.Restaurant_Table
                       on n.User_id equals r.User_id
                       select new
                       {
                           n.Amount,
                           n.ID,
                           r.Rest_name,
                           n.Myan_pay
                       }).OrderByDescending(s => s.ID));
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
            Restaurant_Table rest = db.Restaurant_Table.Where(r=> r.User_id==refund_Table.User_id).FirstOrDefault();
            
            if (rest.Rest_coin >= refund_Table.Amount)
            {
 
                PushNotification pushnoti = new PushNotification();
                var adminlist = db.User_Table.Where(u => u.User_type == "admin");
                System_Table system = db.System_Table.FirstOrDefault();
                foreach (User_Table user in adminlist)
                {
                    pushnoti.pushNoti(user.User_noti_token, "New Refund Request", rest.Rest_name + " requested a refund");
                }     
                db.Refund_Table.Add(refund_Table);
                rest.Rest_coin -= refund_Table.Amount;
                system.Sold_coins += refund_Table.Amount;
                db.Entry(rest).State = EntityState.Modified;
                db.Entry(system).State = EntityState.Modified;
                db.SaveChanges();
                var return_rest = db.Restaurant_Table.Where(r => r.User_id == refund_Table.User_id).Select(r => new {
                    r.Rest_id,
                    r.Rest_name,
                    r.Rest_category,
                    r.Rest_coin,
                    r.Rest_email,
                    r.Rest_location,
                    r.Rest_phno,
                    r.Rest_state,
                    r.Rest_special_coin,
                    r.Rest_lat,
                    r.Rest_long,
                }).FirstOrDefault();
                return Ok(return_rest);
            }
            else
            {
                return Ok("Not Enough");
            }
            

          
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

            return GetRefund_Table();
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