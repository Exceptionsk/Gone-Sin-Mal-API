using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gone_Sin_Mal_API.Class
{
    public class POPEmail
    {
        public POPEmail()
        {
            this.Attachments = new List<Attachment>();
        }
        public int MessageNumber { get; set; }

        public string From { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
        public DateTime DateSent { get; set; }

        public List<Attachment> Attachments { get; set; }

        private int amount;

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        private int tran_id;

        public int Tran_id
        {
            get { return tran_id; }
            set { tran_id = value; }
        }

    }
    [Serializable]
    public class Attachment
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}