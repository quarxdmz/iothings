using api.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;

namespace api.NunitTest
{
    [TestFixture]
    public class ContactUsControllerTest
    {
        HttpControllerContext controllerContext;
        ContactUsController controller;
        [SetUp]
        public void SetUp()
        {
            controllerContext = new HttpControllerContext();

            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            controllerContext.Request = new HttpRequestMessage();

            controllerContext.Request.Headers.Add("Basic", "password");

            controller = new ContactUsController();

            controller.ControllerContext = controllerContext;
        }

        [Test, Ignore("this")]
        public void SendContactForm()
        {
            //have to change the code to use params instead of httpcontext
        }
    }
}