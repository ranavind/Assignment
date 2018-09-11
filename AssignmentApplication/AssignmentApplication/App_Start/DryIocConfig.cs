using System;
using System.IO;
using System.Web.Http;

using DryIoc;
using DryIoc.WebApi;
using System.Reflection;
using CoreLayer.Interfaces;
using CoreLayer.Services;

namespace AssignmentApplication
{
    /// <summary>
    /// DryIoc configuration class
    /// </summary>
    public static class DryIocConfig
    {
     
        /// <summary>
        /// Configure the IOC container
        /// </summary>
        /// <param name="config">Represents a configuration of HttpServer instances</param>
        public static void Configure(HttpConfiguration config)
        {
            try
            {
                var container = new Container();
                container.Register<ISampleService, SampleService>();
                //RegisterServices(container);
              
                container.WithWebApi(config, null, throwIfUnresolved: type => type.IsController());
            }
            catch (Exception ex)
            {

            }
        }

        #region Register with container
      
        private static void RegisterServices(IContainer container)
        {
            container.Register<ISampleService, SampleService>();
          
        }

        #endregion
    }
}
