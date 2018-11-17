using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gone_Sin_Mal_API.Class
{
    public class Comfirmation
    {
        private long rest_id;

        public long Rest_id
        {
            get { return rest_id; }
            set { rest_id = value; }
        }

        private long tran_id;

        public long Tran_id
        {
            get { return tran_id; }
            set { tran_id = value; }
        }

    }
}