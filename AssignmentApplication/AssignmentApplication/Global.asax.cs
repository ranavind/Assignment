
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace AssignmentApplication
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            HttpConfiguration config = new HttpConfiguration();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            DryIocConfig.Configure(config);
        }
    }
}
