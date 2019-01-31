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
    public class AdminController : ApiController
    {
        private Gone_Sin_MalEntities db = new Gone_Sin_MalEntities();

        //GET: api/Admin
        public IHttpActionResult GetSystem_Table()
        {
            return Ok(db.System_Table.Select(s=>new { s.Expired_coins, s.Sold_coins, s.Sold_special_coins }).FirstOrDefault());
        }

        //// GET: api/Admin/5
        //[ResponseType(typeof(System_Table))]
        //public IHttpActionResult GetSystem_Table(long id)
        //{
        //    System_Table system_Table = db.System_Table.Find(id);
        //    if (system_Table == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(system_Table);
        //}
        //[HttpGet]
        //[Route("api/Admin/{cointype}")]
        //public IHttpActionResult Coinstatus(string cointype)
        //{
        //    if (cointype == "expired")
        //    {
        //        var coinstatus = db.System_Table.Sum(c => c.Expired_coins).Value;
        //        return Ok(coinstatus);
        //    }
        //    else if (cointype == "normal")
        //    {
        //        var coinstatus = db.System_Table.Sum(c => c.Sold_coins).Value;
        //        return Ok(coinstatus);
        //    }
        //    else if (cointype == "special")
        //    {
        //        var coinstatus = db.System_Table.Sum(c => c.Sold_special_coins).Value;
        //        return Ok(coinstatus);
        //    }
        //    else if (cointype == "total")
        //    {
        //        var coinstatus = db.System_Table.Sum(c => c.Expired_coins+c.Sold_coins+c.Sold_special_coins).Value;
        //        return Ok(coinstatus);
        //    }
        //        return Ok();

        //}
        // PUT: api/Admin/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSystem_Table(long id, System_Table system_Table)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != system_Table.Record_id)
            {
                return BadRequest();
            }

            db.Entry(system_Table).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!System_TableExists(id))
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

        // POST: api/Admin
        [ResponseType(typeof(System_Table))]
        public IHttpActionResult PostSystem_Table(System_Table system_Table)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.System_Table.Add(system_Table);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = system_Table.Record_id }, system_Table);
        }

        [HttpGet]
        [Route("api/Admin/authenticate")]
        public IHttpActionResult CheckAdmin(String key)
        {
            System_Table master = db.System_Table.FirstOrDefault();
            if (master.Masterkey == key)
            {
                return Ok("Yes");
            }
            else
            {
                return Ok("No");
            }
           
        }


        // DELETE: api/Admin/5
        [ResponseType(typeof(System_Table))]
        public IHttpActionResult DeleteSystem_Table(long id)
        {
            System_Table system_Table = db.System_Table.Find(id);
            if (system_Table == null)
            {
                return NotFound();
            }

            db.System_Table.Remove(system_Table);
            db.SaveChanges();

            return Ok(system_Table);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool System_TableExists(long id)
        {
            return db.System_Table.Count(e => e.Record_id == id) > 0;
        }
    }
}