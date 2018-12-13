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
using System.IO;

namespace Gone_Sin_Mal_API.Controllers
{
    public class PackageController : ApiController
    {
        private Gone_Sin_MalEntities db = new Gone_Sin_MalEntities();

        // GET: api/Package
        public IHttpActionResult GetPackage_Table()
        {
            return Ok(db.Package_Table.Where(p=> p.Package_id!=0).Select(s=> new { s.Package_id, s.Package_type, s.Myanpay_button_link, s.Package_coin_amount}) );
        }

        // GET: api/Package/image/5
        [Route("api/Package/image/{id}")]
        public HttpResponseMessage GetPackage_Image(long id)
        {
            try
            {
                Package_Table package_Table = db.Package_Table.Find(id);
                byte[] imgData = package_Table.Coin_img;
                MemoryStream ms = new MemoryStream(imgData);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(ms);
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                return response;
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }

        }

        // PUT: api/Package/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPackage_Table(long id, Package_Table package_Table)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != package_Table.Package_id)
            {
                return BadRequest();
            }

            db.Entry(package_Table).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Package_TableExists(id))
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

        // POST: api/Package
        [ResponseType(typeof(Package_Table))]
        public IHttpActionResult PostPackage_Table(Package_Table package_Table)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Package_Table.Add(package_Table);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = package_Table.Package_id }, package_Table);
        }

        // DELETE: api/Package/5
        [ResponseType(typeof(Package_Table))]
        public IHttpActionResult DeletePackage_Table(long id)
        {
            Package_Table package_Table = db.Package_Table.Find(id);
            if (package_Table == null)
            {
                return NotFound();
            }

            db.Package_Table.Remove(package_Table);
            db.SaveChanges();

            return Ok(package_Table);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Package_TableExists(long id)
        {
            return db.Package_Table.Count(e => e.Package_id == id) > 0;
        }
    }
}