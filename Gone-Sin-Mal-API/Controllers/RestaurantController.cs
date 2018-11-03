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
    public class RestaurantController : ApiController
    {
        private Gone_Sin_MalEntities db = new Gone_Sin_MalEntities();

        // GET: api/Restaurant
        public IQueryable<Restaurant_Table> GetRestaurant_Table()
        {
            return db.Restaurant_Table;
        }

        // GET: api/Restaurant/5
        [ResponseType(typeof(Restaurant_Table))]
        public IHttpActionResult GetRestaurant_Table(long id)
        {
            Restaurant_Table restaurant_Table = db.Restaurant_Table.Find(id);
            if (restaurant_Table == null)
            {
                return NotFound();
            }

            return Ok(restaurant_Table);
        }

        // PUT: api/Restaurant/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRestaurant_Table(long id, Restaurant_Table restaurant_Table)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != restaurant_Table.Rest_id)
            {
                return BadRequest();
            }

            db.Entry(restaurant_Table).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Restaurant_TableExists(id))
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

        // POST: api/Restaurant
        [ResponseType(typeof(Restaurant_Table))]
        public IHttpActionResult PostRestaurant_Table(Restaurant_Table restaurant_Table)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            db.Restaurant_Table.Add(restaurant_Table);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = restaurant_Table.Rest_id }, restaurant_Table);
        }

        // DELETE: api/Restaurant/5
        [ResponseType(typeof(Restaurant_Table))]
        public IHttpActionResult DeleteRestaurant_Table(long id)
        {
            Restaurant_Table restaurant_Table = db.Restaurant_Table.Find(id);
            if (restaurant_Table == null)
            {
                return NotFound();
            }

            db.Restaurant_Table.Remove(restaurant_Table);
            db.SaveChanges();

            return Ok(restaurant_Table);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Restaurant_TableExists(long id)
        {
            return db.Restaurant_Table.Count(e => e.Rest_id == id) > 0;
        }
    }
}