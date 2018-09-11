using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Interfaces
{
    // Here you can extend your own methods
   public interface ISampleService :ICurdService
    {
        (HttpStatusCode status, object obj) GetString();
    }
}
