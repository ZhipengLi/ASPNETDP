using System.Web;
using System.Web.Mvc;

namespace ASPPatterns.Chap4.ActiveRecord.UI.MVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
