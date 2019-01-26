using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gone_Sin_Mal_API.Class
{
    public class UserInfo
    {
        private long coin;

        public long Coin
        {
            get { return coin; }
            set { coin = value; }
        }

        private long capacity;

        public long Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }

        private long exceed;

        public long Exceed
        {
            get { return exceed; }
            set { exceed = value; }
        }

        private double expirein;

        public double ExpireIn
        {
            get { return expirein; }
            set { expirein = value; }
        }

        private string state;

        public string State
        {
            get { return state; }
            set { state = value; }
        }

        private long visited;

        public long Visited
        {
            get { return visited; }
            set { visited = value; }
        }

    }
}