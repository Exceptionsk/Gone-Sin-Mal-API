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
using System.Threading.Tasks;
using Gone_Sin_Mal_API.Class;


namespace Gone_Sin_Mal_API.Controllers
{
    public class RestaurantController : ApiController
    {

       
        private Gone_Sin_MalEntities db = new Gone_Sin_MalEntities();

        // GET: api/Restaurant
        //public IQueryable<Restaurant_Table> GetRestaurant_Table()
        //{
        //    return db.Restaurant_Table;
        //}



        //GET: api/Restaurant/5
        [ResponseType(typeof(Restaurant_Table))]
        public IHttpActionResult GetRestaurantByUserID(string id, bool profile)
        {
            if (profile)
            {
               var restaurant_Table = (from r in db.Restaurant_Table
                                   where r.User_id.ToString().Equals(id)
                                   select new {
                                       r.Rest_id,
                                       r.Rest_name,
                                       r.Rest_category,
                                       r.Rest_coin,
                                       r.Rest_email,
                                       r.Rest_location,
                                       r.Rest_phno,
                                       r.Rest_state,     
                                       r.Rest_special_coin,
                                   }).FirstOrDefault();

                if (restaurant_Table == null)
                {
                    return NotFound();
                }

                return Ok(restaurant_Table);
            }
            else
            {
                var restaurant_Table = (from r in db.Restaurant_Table
                                        where r.Rest_id.ToString().Equals(id)
                                        select new
                                        {
                                            r.Rest_id,
                                            r.Rest_name,
                                            r.Rest_category,
                                            r.Rest_coin,
                                            r.Rest_email,
                                            r.Rest_location,
                                            r.Rest_phno,
                                            r.Rest_state,                                         
                                        }).FirstOrDefault();
                if (restaurant_Table == null)
                {
                    return NotFound();
                }

                return Ok(restaurant_Table);
            }

           
        }
        [Route("api/restaurant/search")]
        public IHttpActionResult GetUserByName(string name)
        {
            var restaurant = (from r in db.Restaurant_Table
                              where r.Rest_name.ToLower().Contains(name.ToLower())
                              select new
                              {
                                  r.Rest_id,
                                  r.Rest_category,
                                  r.Rest_name
                              });
            if (restaurant == null)
            {
                return NotFound();
            }

            return Ok(restaurant);
        }

        [HttpGet]
        [Route("api/restaurant/{type}")]
        public IHttpActionResult GetRestaurant(string type)
        {
            IQueryable restaurants = null;
            if (type == "new")
            {
                restaurants = db.Restaurant_Table.OrderByDescending(r => r.Rest_created_date).Select(re => new { re.Rest_name, re.Rest_id } );
            }else if (type == "recommended")
            {
                restaurants = db.Restaurant_Table.OrderByDescending(r => r.Rest_coin_purchased).Select(re => new { re.Rest_name, re.Rest_id });
            }
            else
            {
                restaurants = db.Restaurant_Table.Where(s => s.Rest_name.ToLower().Contains(type.ToLower())).Select(re => new { re.Rest_name, re.Rest_id });
            }
            
            if (restaurants == null)
            {
                return NotFound();
            }

            return Ok(restaurants);
        }
        [HttpPut]
        [Route("api/updatephone")]
        public IHttpActionResult putPhone(long id, String phone)
        {
            Restaurant_Table r = new Restaurant_Table();
            r = db.Restaurant_Table.Find(id);
            r.Rest_phno = phone;
            db.Entry(r).State = EntityState.Modified;
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPut]
        [Route("api/updateemail")]
        public IHttpActionResult putEmail(long id, String email)
        {
            Restaurant_Table r = new Restaurant_Table();
            r = db.Restaurant_Table.Find(id);
            r.Rest_email = email;
            db.Entry(r).State = EntityState.Modified;
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPut]
        [Route("api/updatecategory")]
        public IHttpActionResult putCategory(long id, String category)
        {
            Restaurant_Table r = new Restaurant_Table();
            r = db.Restaurant_Table.Find(id);
            r.Rest_category = category;
            db.Entry(r).State = EntityState.Modified;
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        //// PUT: api/Restaurant/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutRestaurant_Table(long id, Restaurant_Table restaurant_Table)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != restaurant_Table.Rest_id)
        //    {
        //        return BadRequest();
        //    }
        //    Restaurant_Table r = new Restaurant_Table();
        //    r = db.Restaurant_Table.Find(id);
        //    if (restaurant_Table.Rest_phno == "")
        //    {          
        //        r.Rest_email = restaurant_Table.Rest_email;
        //    }else if (restaurant_Table.Rest_email == "")
        //    {
        //        r.Rest_phno = restaurant_Table.Rest_phno;
        //    }

        //    db.Entry(r).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!Restaurant_TableExists(id))
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

            return Ok(restaurant_Table);
        }
        [Route("api/restaurant/qr")]
        public IHttpActionResult QRScan(CoinTransaction transaction)
        {
            Restaurant_Table rest = db.Restaurant_Table.Where(r => r.User_id==transaction.Rest_id).FirstOrDefault();
            User_Table user = db.User_Table.Where(u => u.User_id == transaction.User_id).FirstOrDefault();
            Notification_Table noti = new Notification_Table();
            PushNotification pushnoti = new PushNotification();

            noti.User_id = transaction.User_id;
            noti.Noti_status = false;

            if (transaction.Take)
            {
                if (user.User_available_coin < transaction.Amount)
                {
                    return Ok("not enough");
                }
                else
                {
                    rest.Rest_coin = rest.Rest_coin + transaction.Amount;
                    user.User_available_coin = user.User_available_coin - transaction.Amount;
                    noti.Notification = "You have used" + transaction.Amount + "for" + rest.Rest_name;
                    noti.Noti_type = "customer";
                    pushnoti.pushNoti(user.User_noti_token, "Coin Spend", noti.Notification);
                } 
            }
            else
            {
                if(rest.Rest_coin< transaction.Amount)
                {
                    return Ok("not enough");
                }
                else
                {
                    rest.Rest_coin = rest.Rest_coin - transaction.Amount;
                    user.User_available_coin = user.User_available_coin + transaction.Amount;
                    noti.Notification = "You have gained" + transaction.Amount + "from" + rest.Rest_name + ". Please keep that in mind that these points are only valid before the expire date.";
                    noti.Noti_type = "customer";
                    pushnoti.pushNoti(user.User_noti_token, "Coin Gained", noti.Notification);
                }
            }
            db.Notification_Table.Add(noti);
            db.Entry(rest).State = EntityState.Modified;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            return Ok("OK");
        }

        [HttpGet]
        [Route("api/restaurant/pic")]
        public HttpResponseMessage GetImage(long id, long gallery=0)
        {
            try
            {
                Gone_Sin_MalEntities db = new Gone_Sin_MalEntities();
                var data = from i in db.Restaurant_Table
                           where i.Rest_id == id
                           select i;                           
                Restaurant_Table team = (Restaurant_Table)data.SingleOrDefault();
                byte[] imgData = null;

                if (gallery == 0)
                {
                    imgData = team.Rest_profile_picture;
                }
                else if (gallery == 1)
                {
                    imgData = team.Rest_gallery_1;
                }
                else if (gallery == 2)
                {
                    imgData = team.Rest_gallery_2;
                }
                else if (gallery == 3)
                {
                    imgData = team.Rest_gallery_3;
                }
                else
                {
                    imgData = team.Rest_gallery_4;
                }
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
        [HttpPost]
        [Route("api/restaurant/pic")]
        public Task<IEnumerable<string>> Img(long id, long gallery=0)
        {
            if (Request.Content.IsMimeMultipartContent())
            {
                string fullPath = System.Web.HttpContext.Current.Server.MapPath("~/Temp");
                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(fullPath);
                var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith(t =>
                {
                    if (t.IsFaulted || t.IsCanceled)
                        throw new HttpResponseException(HttpStatusCode.InternalServerError);
                    var fileInfo = streamProvider.FileData.Select(i =>
                    {
                        var info = new FileInfo(i.LocalFileName);
                        Gone_Sin_MalEntities db = new Gone_Sin_MalEntities();
                        byte[] img = File.ReadAllBytes(info.FullName);
                        var team = db.Restaurant_Table.FirstOrDefault(e => e.Rest_id == id);
                        if (gallery == 0)
                        {
                            team.Rest_profile_picture = img;
                        }else if (gallery == 1)
                        {
                            team.Rest_gallery_1 = img;
                        }else if (gallery == 2)
                        {
                            team.Rest_gallery_2 = img;
                        }else if (gallery ==3)
                        {
                            team.Rest_gallery_3 = img;
                        }
                        else
                        {
                            team.Rest_gallery_4 = img;
                        }
                        db.Entry(team).State = EntityState.Modified;
                        db.SaveChanges();
                        return "File uploaded successfully!";
                    });
                    return fileInfo;
                });
                return task;
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid Request!"));
            }
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