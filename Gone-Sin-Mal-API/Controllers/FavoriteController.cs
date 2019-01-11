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
    public class FavoriteController : ApiController
    {
        private Gone_Sin_MalEntities db = new Gone_Sin_MalEntities();

        // GET: api/Favorite
        //public IQueryable<Favorite_Table> GetFavorite_Table()
        //{
        //    return db.Favorite_Table;
        //}

        // GET: api/Favorite/5
        public bool GetFavorite_Table(long User_id, long Rest_id)
        {
            Favorite_Table favorite_Table = db.Favorite_Table.Where(f=> f.User_id== User_id && f.Rest_id==Rest_id).FirstOrDefault();
            if (favorite_Table == null)
            {
                return false;
            }

            return true;
        }
        [Route("api/Favorite/all")]
        public IHttpActionResult GetFavoriteByUser(long User_id)
        {
            var favorite_Table = (from f in db.Favorite_Table
                                  join r in db.Restaurant_Table
                                  on f.Rest_id equals r.Rest_id
                                  where f.User_id == User_id
                                  select new {
                                      f.Rest_id,
                                      r.Rest_name,
                                      r.Rest_category
                                  });
            if (favorite_Table == null)
            {
                return Ok("null");
            }

            return Ok(favorite_Table);
        }

        //// PUT: api/Favorite/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutFavorite_Table(long id, Favorite_Table favorite_Table)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != favorite_Table.Fav_id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(favorite_Table).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!Favorite_TableExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/Favorite
        public bool PostFavorite_Table(Favorite_Table favorite_Table)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            bool loved = GetFavorite_Table(favorite_Table.User_id, favorite_Table.Rest_id);
            if (loved)
            {
                 DeleteFavorite_Table(favorite_Table);
                return false;
            }
            else
            {
                db.Favorite_Table.Add(favorite_Table);
            }
            
            db.SaveChanges();

            return true;
        }

        // DELETE: api/Favorite/5
        public bool DeleteFavorite_Table(Favorite_Table fav)
        {
            Favorite_Table favorite_Table = db.Favorite_Table.Where(f=> f.User_id==fav.User_id && f.Rest_id==fav.Rest_id).FirstOrDefault();
            if (favorite_Table == null)
            {
                return false;
            }

            db.Favorite_Table.Remove(favorite_Table);
            db.SaveChanges();

            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Favorite_TableExists(long id)
        {
            return db.Favorite_Table.Count(e => e.Fav_id == id) > 0;
        }
    }
}