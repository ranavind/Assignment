//-------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Honeywell">
//      Copyright © 2017. Honeywell International Inc. All Rights Reserved.
//
//      This software is a copyrighted work and/or information protected as a trade secret. Legal rights of Honeywell Inc. in this software 
//      is distinct from ownership of any medium in which the software is embodied. 
//      Copyright or trade secret notices included must be reproduced in any copies authorized by Honeywell Inc. 
//      The information in this software is subject to change without notice and should not be considered as a commitment by Honeywell Inc.
// </copyright>
//-------------------------------------------------------------------------------------------------------------------------------------------
using System;
using Microsoft.Owin.Hosting;
using System.Configuration;
using System.Reflection;
using SelfHosted;
using static SelfHosted.Utilities;

namespace Honeywell.GLSS.Tools.SelfHosted
{
    /// <summary>
    /// The main program class
    /// </summary>
    public static class Program
    {
        #region private fields

        private static PortNumberServer _portNumberServer;
        private static HostStatusServer _hostStatusServer;

        #endregion private fields

        /// <summary>
        /// The Main starts the Owin host
        /// </summary>
        public static void Main(string[] args)
        {
            try
            {

#if DEBUG
                Console.WriteLine($"{Assembly.GetExecutingAssembly().GetName().Name} is starting.");
#endif
                var portNo = ConfigurationManager.AppSettings["portNo"] ?? "41469";
#if DEBUG
                if (args.Length > 0)
                {
                    portNo = args[0];
                }
#endif
                var url = $"http://localhost:{portNo}";
                StartPipeServers(portNo);

                using (WebApp.Start<Startup>(url))
                {
#if DEBUG
                    Console.WriteLine("Owin host started, any key to exit.  URL: " + url);
#endif
                    _hostStatusServer.Status = Utilities.HostStatus.Running;
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.ToString());
#endif
            }
            finally
            {
                StopPipeServers();
            }
        }

        #region private methods
        private static void StartPipeServers(string portNo)
        {
            _portNumberServer = new PortNumberServer(portNo);
            _portNumberServer.Start();
            _hostStatusServer = new HostStatusServer
            {
                Status = HostStatus.Starting
            };
            _hostStatusServer.Start();
        }
        private static void StopPipeServers()
        {
            _portNumberServer.Stop();
            _hostStatusServer.Stop();
        }
        #endregion private methods
    }
}