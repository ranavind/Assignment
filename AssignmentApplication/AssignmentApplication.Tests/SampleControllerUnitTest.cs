using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AssignmentApplication.Controllers;
using System.Web.Http;
using System.Web.Http.Results;
using System.Net;
using Moq;
using CoreLayer.Interfaces;

namespace AssignmentApplication.Tests
{
    [TestClass]
    public class SampleControllerUnitTest
    {
        [TestMethod]
        public void TestGetHelloString()
        {
            var mockRepo = new Mock<ISampleService>();
            mockRepo.Setup(x => x.GetString()).Returns((HttpStatusCode.OK,"Hello"));
            var controller = new SampleController(mockRepo.Object);
          
            IHttpActionResult actionResult = controller.Get();
            var contentResult = actionResult as OkNegotiatedContentResult<object>;
            Assert.IsNotNull(contentResult);
           // Assert.AreEqual(HttpStatusCode.OK, contentResult.);
            Assert.IsNotNull(contentResult.Content);
        }
    }
}
