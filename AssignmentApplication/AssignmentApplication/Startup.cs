using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(AssignmentApplication.Startup))]

namespace AssignmentApplication
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            HttpConfiguration config = new HttpConfiguration();         


            WebApiConfig.Register(config);
            DryIocConfig.Configure(config);

            app.UseWebApi(config);
        }
    }
}