using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SampleConsoleApp
{
    class ConsumeWebAPI
    {
     public void GetAllEventData() //Get All Events Records  
        {
            using (var client = new WebClient()) //WebClient  
            {
                client.Headers.Add("Content-Type:application/json"); //Content-Type  
                client.Headers.Add("Accept:application/json");
                var url = ConfigurationManager.AppSettings["BaseUrl"]+"Sample";
                var result = client.DownloadString("http://localhost:59394/api/sample"); //URI  
                Console.WriteLine(Environment.NewLine + result);
            }
        }
        
    }
}
