using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;

namespace Gone_Sin_Mal_API.Class
{
    public class Notification
    {
        public string to { get; set; }
        public string title { get; set; }
        public string body { get; set; }
    }

    public class RootObject
    {
        public Notification notification { get; set; }
    }
}