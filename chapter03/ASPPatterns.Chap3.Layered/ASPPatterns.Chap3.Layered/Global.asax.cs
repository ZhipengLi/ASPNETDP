using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace ASPPatterns.Chap3.Layered
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            ASPPatterns.Chap3.Layered.IoC.BootStrapper.ConfigureStructureMap();
        }
    }
}