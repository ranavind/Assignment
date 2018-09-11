using CoreLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AssignmentApplication.Controllers
{
    public class SampleController : CommonController
    {

        private readonly ISampleService _sampleService;
        public SampleController(ISampleService sampleService) : base(sampleService)
        {
            _sampleService = sampleService;
        }
        // GET: api/Sample
        public IHttpActionResult Get()
        {
            var result = _sampleService.GetString();

            if ((HttpStatusCode)result.status == HttpStatusCode.OK)
            {
                
                return Ok(result.obj);
            }
            return InternalServerError();
        }

        // GET: api/Sample/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Sample
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Sample/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Sample/5
        public void Delete(int id)
        {
        }

    }
}
