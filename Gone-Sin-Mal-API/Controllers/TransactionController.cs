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
        [HttpGet]
        [Route("api/transaction/checkmail")]
        public IHttpActionResult CheckMail_GiveCoin()
        {
            string amount, tran_id;
            Pop3Client pop3Client = new Pop3Client();
            pop3Client.Connect("pop.gmail.com", 995, true);
            pop3Client.Authenticate("minthukhant.mtk03@gmail.com", "minthukhantfgh");
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
                if (message.Headers.From.Address == "service@myanpay.com.mm" && email.Subject== "MyanPay Notification [ Receive money for donation transaction is completed.]")
                {
                    MessagePart body = message.FindFirstHtmlVersion();
                    if (body != null)
                    {
                        try
                        {
                           amount = str_method.GetBetween(body.GetBodyAsText(), "sent", "kyats");
                           amount = str_method.GetBetween(amount, "<b>", ".00</b>");
                           tran_id = str_method.GetBetween(body.GetBodyAsText(), "Transaction ID -", "is");
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

                        //add restaurant coin
                        return Ok(email);

                    }
                    else
                    {
                        return Ok("failed");
                    }

                }

            }
            return Ok("success");
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