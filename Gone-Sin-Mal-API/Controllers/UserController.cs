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
    public class UserController : ApiController
    {
        private Gone_Sin_MalEntities db = new Gone_Sin_MalEntities();

        // GET: api/User
        public IQueryable<User_Table> GetUser_Table()
        {
            return db.User_Table;
        }

        // GET: api/User/5
        [ResponseType(typeof(User_Table))]
        public IHttpActionResult GetUser_Table(long id)
        {
            User_Table user_Table = db.User_Table.Find(id);
            if (user_Table == null)
            {
                return NotFound();
            }

            return Ok(user_Table);
        }

        [Route("api/user/search")]
        public IHttpActionResult GetUserByName(string name)
        {
            var user = db.User_Table.Where(s => s.User_name.ToLower().Contains(name.ToLower())).Select(u => new { u.User_name, u.User_id, u.User_type});
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Route("api/user/promote")]
        public IHttpActionResult Promote(User_Table user_Table)
        {
            User_Table user = db.User_Table.Find(user_Table.User_id);
            if (User_TableExists(user_Table.User_id) == false)
            {
                return BadRequest();
            }
            else
            {
                user_Table = user;
                user_Table.User_type = "admin";
                db.Entry(user_Table).State = EntityState.Modified;
            }

            try
            {

                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return Ok(user_Table);
        }

        [Route("api/user/demote")]
        public IHttpActionResult Demote(User_Table user_Table)
        {
            User_Table user = db.User_Table.Find(user_Table.User_id);
            if (User_TableExists(user_Table.User_id) == false)
            {
                return BadRequest();
            }
            else
            {
                user_Table = user;
                user_Table.User_type = "customer";
                db.Entry(user_Table).State = EntityState.Modified;
            }

            try
            {

                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return Ok(user_Table);
        }

        // PUT: api/User/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser_Table(long id, User_Table user_Table)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user_Table.User_id)
            {
                return BadRequest();
            }

            db.Entry(user_Table).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!User_TableExists(id))
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

        // POST: api/User
        [ResponseType(typeof(User_Table))]
        public IHttpActionResult PostUser_Table(User_Table user_Table)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            User_Table user = db.User_Table.Find(user_Table.User_id);
            if (User_TableExists(user_Table.User_id)==false)
            {
                user_Table.User_type = "new";
                db.User_Table.Add(user_Table);
            }
            else
            {
                if (user.User_type == "new")
                {
                    user.User_type = user_Table.User_type;
                }               
                user_Table = user;
                db.Entry(user_Table).State = EntityState.Modified;
            }                    

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }
            
            return Ok(user_Table);
        }

        // DELETE: api/User/5
        [ResponseType(typeof(User_Table))]
        public IHttpActionResult DeleteUser_Table(long id)
        {
            User_Table user_Table = db.User_Table.Find(id);
            if (user_Table == null)
            {
                return NotFound();
            }

            db.User_Table.Remove(user_Table);
            db.SaveChanges();

            return Ok(user_Table);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool User_TableExists(long id)
        {
            return db.User_Table.Count(e => e.User_id == id) > 0;
        }
    }
}