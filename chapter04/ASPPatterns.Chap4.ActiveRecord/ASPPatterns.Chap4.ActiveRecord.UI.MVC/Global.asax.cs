using ASPPatterns.Chap4.ActiveRecord.Model;
using Castle.ActiveRecord.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ASPPatterns.Chap4.ActiveRecord.UI.MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            IConfigurationSource source = ConfigurationManager.GetSection("activeRecord") as IConfigurationSource;
            Castle.ActiveRecord.ActiveRecordStarter.Initialize(source, typeof(Post), typeof(Comment));
        }
    }
}
