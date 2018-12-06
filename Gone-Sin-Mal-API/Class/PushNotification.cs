using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
namespace Gone_Sin_Mal_API.Class
{
    public class PushNotification
    {
        public async void pushNoti(string to, string title, string body)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Notification noti = new Notification();
            noti.to = to;
            noti.title = title;
            noti.body = body;
            HttpResponseMessage response = await client.PostAsJsonAsync("https://exp.host/--/api/v2/push/send", noti);
        }
    }
}