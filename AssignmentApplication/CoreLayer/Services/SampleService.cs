using CoreLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services
{
    public class SampleService : ISampleService
    {
        public (HttpStatusCode status, object obj) Create(object viewModel)
        {
            //Here you can implement for saving the record to DB or text file etc.
            throw new NotImplementedException();
        }

        public (HttpStatusCode status, object obj) Delete(string id)
        {
            //Here you can implement for Delete the record to DB or text file etc.
            throw new NotImplementedException();
        }

        public (HttpStatusCode status, object obj) GetString()
        {
              return new ValueTuple<HttpStatusCode, object>(HttpStatusCode.OK, "Hello");
        }

        public (HttpStatusCode status, object obj) Read(string id)
        {
            //Here you can implement for reading the record based on id from the DB or text file etc.
            throw new NotImplementedException();
        }

        public (HttpStatusCode status, object obj) Update(object viewModel)
        {
            //Here you can implement for Update the record based into the DB or text file etc.
            throw new NotImplementedException();
        }
        //If need you you can extend the class or Interface with your methods
    }
}
