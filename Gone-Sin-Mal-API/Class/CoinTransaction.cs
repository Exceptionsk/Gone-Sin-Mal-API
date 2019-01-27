using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gone_Sin_Mal_API.Class
{
    public class CoinTransaction
    {
        private long user_id;

        public long User_id
        {
            get { return user_id; }
            set { user_id = value; }
        }

        private long rest_id;

        public long Rest_id
        {
            get { return rest_id; }
            set { rest_id = value; }
        }

        private long amount;

        public long Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        private bool take;

        public bool Take
        {
            get { return take; }
            set { take = value; }
        }

        private bool special;

        public bool Special
        {
            get { return special; }
            set { special = value; }
        }

        private long promoid;

        public long PromoId
        {
            get { return promoid; }
            set { promoid = value; }
        }


    }
}