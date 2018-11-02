using System.Web;
using System.Web.Mvc;

namespace Gone_Sin_Mal_API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
