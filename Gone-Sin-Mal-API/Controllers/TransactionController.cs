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
using OpenPop.Pop3;
using Gone_Sin_Mal_API.Class;
using OpenPop.Mime;

namespace Gone_Sin_Mal_API.Controllers
{
    public class TransactionController : ApiController
    {
        StringMethods str_method = new StringMethods();
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
        [Route("api/transaction/request")]
        public IHttpActionResult RequestTransaction(Transaction_Table transaction)
        {
            db.Transaction_Table.Add(transaction);
            var noti = new Notification_Table();
            PushNotification pushnoti = new PushNotification();
            User_Table user = new User_Table();
            user = db.User_Table.Find(transaction.User_id);
            noti.Noti_text = "Comfirmation Required!";
            noti.Notification = "Please send the transaction ID from Myan Pay email to receive Coin bought.";
            noti.Noti_type = "restaurant";
            noti.Noti_status = false;
            noti.User_id = transaction.User_id;
            noti.ID = transaction.ID;
            db.Notification_Table.Add(noti);
            db.SaveChanges();
            pushnoti.pushNoti(user.User_noti_token, "Transaction ID required!", noti.Notification);
            return Ok("success");
        }

        [Route("api/transaction/comfirm")]
        public IHttpActionResult CheckMail_GiveCoin(Comfirmation comfirm)
        {
            string amount, tran_id;
            Pop3Client pop3Client = new Pop3Client();
            pop3Client.Connect("pop.gmail.com", 995, true);
            pop3Client.Authenticate("gonesinmal@gmail.com", "gonesinmal123!@#");
            int count = pop3Client.GetMessageCount(); //total count of email in MessageBox  
            //var Emails = new List<POPEmail>();
           
            for (int i = count; i >= 1; i--)
            {
                Message message = pop3Client.GetMessage(i);
                POPEmail email = new POPEmail()
                {
                    MessageNumber = i,
                    Subject = message.Headers.Subject,
                    DateSent = message.Headers.DateSent,
                    From = message.Headers.From.DisplayName + ":" + message.Headers.From.Address,

                };
                if (email.DateSent < DateTime.Now.AddDays(-7))
                {
                    break;
                }
                if (message.Headers.From.Address == "minthukhant.mtk03@gmail.com" && email.Subject== "MyanPay Notification [ Receive money for donation transaction is completed.]")
                {
                    MessagePart body = message.FindFirstPlainTextVersion();
                    if (body != null)
                    {
                        try
                        {
                           amount = str_method.GetBetween(body.GetBodyAsText(), "sent", "kyats");
                           amount = str_method.GetBetween(amount, "<b>", ".00</b>");
                           tran_id = str_method.GetBetween(body.GetBodyAsText(), "TransactionID\r\n-", "is");
                           tran_id = str_method.GetBetween(tran_id, "<b>", "</b>");
                            email.Body = "Email Read " + i;
                            try
                            {
                                email.Amount = int.Parse(amount);
                            }
                            catch (Exception)
                            {
                                email.Amount = 0;
                            }
                            try
                            {
                                email.Tran_id = int.Parse(tran_id);
                            }
                            catch (Exception)
                            {
                                email.Tran_id = 0;
                            }
                           
                        }
                         catch (Exception)
                        {
                            //email.Body = body.GetBodyAsText();
                            email.Amount = 0;
                            email.Tran_id = 0;
                        }
                    }
                    //else
                    //{
                    //    body = message.FindFirstPlainTextVersion();
                    //    if (body != null)
                    //    {
                    //        email.Body = body.GetBodyAsText();
                    //    }
                    //}
                    //Emails.Add(email);

                    if (email.Amount != 0 && email.Tran_id != 0)
                    {
                        if (email.Tran_id == comfirm.Tran_id)
                        {
                            var tran_record = db.Transaction_Table.Where(t => t.ID == comfirm.ID).FirstOrDefault();
                            var restaurant = db.Restaurant_Table.Where(r => r.User_id == comfirm.Rest_id).FirstOrDefault();
                        
                            if (tran_record != null)
                            {
                                var coin =email.Amount / 100;
                                var noti = new Notification_Table();
                                var adminnoti = new Notification_Table();
                                PushNotification pushnoti = new PushNotification();
                                System_Table system = db.System_Table.FirstOrDefault();
                                if (tran_record.Tran_type == "normal")
                                {
                                    restaurant.Rest_coin = restaurant.Rest_coin + coin;
                                    noti.Noti_text = "Comfirmation completed!";
                                    noti.Notification = coin + " coins have been delivered to your balance.";
                                    adminnoti.User_id = long.Parse(restaurant.User_id.ToString());
                                    adminnoti.Noti_type = "admin";
                                    adminnoti.Noti_text = "Normal Coin Sold!";                           
                                    adminnoti.Notification = restaurant.Rest_name + " just bought " + coin + " special coins.";
                                    system.Sold_coins += coin;
                                }
                                else if (tran_record.Tran_type == "special")
                                {
                                    system.Sold_special_coins += coin;
                                    restaurant.Rest_special_coin = restaurant.Rest_special_coin + coin;
                                    var nearbyuser = db.User_Table.Where(u => u.User_state.Equals(restaurant.Rest_state) && u.User_type=="customer").OrderBy(o=> Guid.NewGuid()).Take(comfirm.Count);
                                    if (nearbyuser.Count() <= 0)
                                    {
                                        nearbyuser = db.User_Table.Where(u=>u.User_type=="customer");
                                        foreach (User_Table var in nearbyuser)
                                        {
                                            Notification_Table custNoti = new Notification_Table();
                                            Promotion_Table promo = new Promotion_Table();
                                            promo.Rest_id = restaurant.Rest_id;
                                            promo.User_id = var.User_id;
                                            promo.User_promotion_amount = coin / comfirm.Count;
                                            promo.ExpireIn= DateTime.Now.AddMonths(3);
                                            custNoti.Noti_status = false;
                                            custNoti.User_id = var.User_id;
                                            custNoti.Noti_type = "customer";
                                            custNoti.Noti_text = "Congratulation!";
                                            custNoti.Notification = "You got " + promo.User_promotion_amount + " FREE coins to  Gone Sin at " + restaurant.Rest_name;
                                            pushnoti.pushNoti(var.User_noti_token, "Gone Sin Oppotunity!", custNoti.Notification);
                                            db.Notification_Table.Add(custNoti);
                                            db.Promotion_Table.Add(promo);
                                        }
                                    }
                                    else if (nearbyuser.Count() < comfirm.Count)
                                    {
                                        foreach (User_Table var in nearbyuser)
                                        {
                                            Notification_Table custNoti = new Notification_Table();
                                            Promotion_Table promo = new Promotion_Table();
                                            promo.Rest_id = restaurant.Rest_id;
                                            promo.User_id = var.User_id;
                                            promo.User_promotion_amount = coin / comfirm.Count;
                                            promo.ExpireIn = DateTime.Now.AddMonths(3);
                                            custNoti.Noti_status = false;
                                            custNoti.User_id = var.User_id;
                                            custNoti.Noti_type = "customer";
                                            custNoti.Noti_text = "Congratulation!";
                                            custNoti.Notification = "You got " + promo.User_promotion_amount + " FREE coins to  Gone Sin at " + restaurant.Rest_name;
                                            pushnoti.pushNoti(var.User_noti_token, "Gone Sin Oppotunity!", custNoti.Notification);
                                            db.Notification_Table.Add(custNoti);
                                            db.Promotion_Table.Add(promo);
                                        }
                                        nearbyuser = db.User_Table.Where(u => u.User_type == "customer").OrderBy(o => Guid.NewGuid()).Take(comfirm.Count- nearbyuser.Count());
                                        foreach (User_Table var in nearbyuser)
                                        {
                                            Notification_Table custNoti = new Notification_Table();
                                            Promotion_Table promo = new Promotion_Table();
                                            promo.Rest_id = restaurant.Rest_id;
                                            promo.User_id = var.User_id;
                                            promo.User_promotion_amount = coin / comfirm.Count;
                                            promo.ExpireIn = DateTime.Now.AddMonths(3);
                                            custNoti.Noti_status = false;
                                            custNoti.User_id = var.User_id;
                                            custNoti.Noti_type = "customer";
                                            custNoti.Noti_text = "Congratulation!";
                                            custNoti.Notification = "You got " + promo.User_promotion_amount + " FREE coins to  Gone Sin at " + restaurant.Rest_name;
                                            pushnoti.pushNoti(var.User_noti_token, "Gone Sin Oppotunity!", custNoti.Notification);
                                            db.Notification_Table.Add(custNoti);
                                            db.Promotion_Table.Add(promo);
                                        }
                                    }
                                    else if(nearbyuser.Count()>= comfirm.Count)
                                    {
                                        foreach (User_Table var in nearbyuser)
                                        {
                                            Notification_Table custNoti = new Notification_Table();
                                            Promotion_Table promo = new Promotion_Table();
                                            promo.Rest_id = restaurant.Rest_id;
                                            promo.User_id = var.User_id;
                                            promo.User_promotion_amount = coin / comfirm.Count;
                                            promo.ExpireIn = DateTime.Now.AddMonths(3);
                                            custNoti.Noti_status = false;
                                            custNoti.User_id = var.User_id;
                                            custNoti.Noti_type = "customer";
                                            custNoti.Noti_text = "Congratulation!";
                                            custNoti.Notification = "You got " + promo.User_promotion_amount + " FREE coins to  Gone Sin at " + restaurant.Rest_name;
                                            pushnoti.pushNoti(var.User_noti_token, "Gone Sin Oppotunity!", custNoti.Notification);
                                            db.Notification_Table.Add(custNoti);
                                            db.Promotion_Table.Add(promo);
                                        }
                                    }
                                    
                                    adminnoti.Noti_type = "admin";
                                    adminnoti.User_id = long.Parse(restaurant.User_id.ToString());
                                    adminnoti.Noti_text = "Special Coin Sold!";
                                    adminnoti.Notification = restaurant.Rest_name + " just bought " + coin + " special coins.";
                                    noti.Noti_text = "Comfirmation completed!";
                                    noti.Notification = "Special coins have been delivered to nearby customers.";
                                    
                                }
                                restaurant.Rest_coin_purchased += coin;
                                tran_record.Pending = true;
                                noti.Noti_status = false;
                                noti.User_id = comfirm.Rest_id;
                                noti.ID = comfirm.ID;
                                noti.Noti_type = "restaurant";
                                db.Entry(tran_record).State = EntityState.Modified;
                                db.Entry(restaurant).State = EntityState.Modified;
                                db.Entry(system).State = EntityState.Modified;
                                Notification_Table del_noti = db.Notification_Table.Where(n => n.ID == comfirm.ID).FirstOrDefault();
                                db.Notification_Table.Remove(del_noti);
                                db.Notification_Table.Add(noti);
                                db.Notification_Table.Add(adminnoti);
                                db.SaveChanges();
                                User_Table user = db.User_Table.Find(comfirm.Rest_id);
                                pushnoti.pushNoti(user.User_noti_token, "Comfirmation Completed", noti.Notification);                            
                                return Ok("Yes");
                            }
                            else
                            {
                                return Ok("Failed");
                            }
                        }
                        //add restaurant coin
                        

                    }               
                }

            }
            return Ok("Failed");
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