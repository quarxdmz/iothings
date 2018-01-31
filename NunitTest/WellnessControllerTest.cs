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
    public class WellnessControllerTest
    {
        HttpControllerContext controllerContext;
        WellnessController controller;
        [SetUp]
        public void SetUp()
        {
            controllerContext = new HttpControllerContext();

            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            controllerContext.Request = new HttpRequestMessage();

            controllerContext.Request.Headers.Add("Basic", "password");

            controller = new WellnessController();

            controller.ControllerContext = controllerContext;
        }

        [TestCase("645", true)]
        public void GetWellnessExam(string user_id, bool success)
        {
            dynamic res = controller.getwellnessexam(user_id);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);
        }

        [TestCase("645", true)]
        public void GetExamTaken(string user_id, bool success)
        {
            dynamic res = controller.getexamtaken(user_id);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);
        }
        //[Test, Ignore("Add wellness exam not yet done")]
        public void AddWellnessExam()
        {
            //
        }
        //[Test, Ignore("Add exam taken not yet done")]
        public void AddExamTaken()
        {
            //
        }
    }
}