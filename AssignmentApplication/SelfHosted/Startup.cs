//-------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="Honeywell">
//      Copyright © 2017. Honeywell International Inc. All Rights Reserved.
//
//      This software is a copyrighted work and/or information protected as a trade secret. Legal rights of Honeywell Inc. in this software 
//      is distinct from ownership of any medium in which the software is embodied. 
//      Copyright or trade secret notices included must be reproduced in any copies authorized by Honeywell Inc. 
//      The information in this software is subject to change without notice and should not be considered as a commitment by Honeywell Inc.
// </copyright>
//-------------------------------------------------------------------------------------------------------------------------------------------

using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Swashbuckle.Application;
using System.Web.Hosting;
using Honeywell.GLSS.Tools.Services.Config;
using Honeywell.GLSS.Tools.Logging;

[assembly: OwinStartup(typeof(Honeywell.GLSS.Tools.SelfHosted.Startup))]

namespace Honeywell.GLSS.Tools.SelfHosted
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            HttpConfiguration config = new HttpConfiguration();

            config
                .EnableSwagger(c =>
                {
                    //Name of the API
                    c.SingleApiVersion("v1", "Fusion Configuration API");
                    /**
                     * Make sure the project Honeywell.GLSS.Tools.WebAPI have XML document option enabled
                     * and the file Honeywell.GLSS.Tools.WebAPI.xml is available in BaseDirectory. Otherwise
                     * Swagger Ui will not show any documentation.
                     */
                    c.IncludeXmlComments(string.Format(@"{0}\Honeywell.GLSS.Tools.WebAPI.xml",
                        System.AppDomain.CurrentDomain.BaseDirectory));
                    c.PrettyPrint();
                })
                .EnableSwaggerUi();


            WebApiConfig.Register(config);
            DryIocConfig.Configure(config);
            LoggingConfig.Configure();
            app.UseWebApi(config);
        }
    }
}

