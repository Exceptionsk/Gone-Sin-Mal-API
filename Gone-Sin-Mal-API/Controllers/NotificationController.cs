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
    public class NotificationController : ApiController
    {
        private Gone_Sin_MalEntities db = new Gone_Sin_MalEntities();

        // GET: api/Notification
        //public IQueryable<Notification_Table> GetNotification_Table()
        //{
        //    return db.Notification_Table;
        //}
        [HttpGet]
        [Route("api/notification/{id}")]
        public IHttpActionResult Noti(long id, string type="customer", long transactionID=0)
        {
            if (type == "restaurant")
            {
                return Ok((from n in db.Notification_Table
                           join t in db.Transaction_Table
                           on n.ID equals t.ID
                           join p in db.Package_Table
                           on t.Package_id equals p.Package_id
                           select new
                           {
                               n.ID,
                               n.Noti_id,
                               n.Noti_type,
                               n.Noti_status,
                               n.Notification,
                               p.Package_coin_amount,
                               t.Tran_type
                           }).OrderByDescending(s => s.Noti_id));
            }
            if (transactionID==0)
            {
                return Ok(db.Notification_Table.Where(n => n.User_id == id && n.Noti_type == type).Select(s => new { s.ID, s.Noti_id, s.Noti_status, s.Notification }));

            }
            else
            {
               
                return Ok(db.Notification_Table.Where(n => n.User_id == id && n.Noti_type == type && n.ID == transactionID).Select(s => new { s.ID, s.Noti_id, s.Noti_status, s.Notification }));
            }
                      
        }

        // GET: api/Notification/5
        [ResponseType(typeof(Notification_Table))]
        public IHttpActionResult GetNotification_Table(long id)
        {
            Notification_Table notification_Table = db.Notification_Table.Find(id);
            if (notification_Table == null)
            {
                return NotFound();
            }

            return Ok(notification_Table);
        }

        // PUT: api/Notification/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNotification_Table(long id, Notification_Table notification_Table)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notification_Table.Noti_id)
            {
                return BadRequest();
            }

            db.Entry(notification_Table).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Notification_TableExists(id))
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

        // POST: api/Notification
        [ResponseType(typeof(Notification_Table))]
        public IHttpActionResult PostNotification_Table(Notification_Table notification_Table)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Notification_Table.Add(notification_Table);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = notification_Table.Noti_id }, notification_Table);
        }

        // DELETE: api/Notification/5
        [ResponseType(typeof(Notification_Table))]
        public IHttpActionResult DeleteNotification_Table(long id)
        {
            Notification_Table notification_Table = db.Notification_Table.Find(id);
            if (notification_Table == null)
            {
                return NotFound();
            }

            db.Notification_Table.Remove(notification_Table);
            db.SaveChanges();

            return Ok(notification_Table);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Notification_TableExists(long id)
        {
            return db.Notification_Table.Count(e => e.Noti_id == id) > 0;
        }
    }
}