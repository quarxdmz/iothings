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
    public class SpecialtyControllerTest
    {
        HttpControllerContext controllerContext;
        SpecialtyController controller;
        [SetUp]
        public void SetUp()
        {
            controllerContext = new HttpControllerContext();

            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            controllerContext.Request = new HttpRequestMessage();

            controllerContext.Request.Headers.Add("Basic", "password");

            controller = new SpecialtyController();

            controller.ControllerContext = controllerContext;

        }

        [Test]
        public void GetAllSpecialty()
        {
            dynamic res = controller.getspecialist();

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, true);
        }

        [TestCase(1, true)]
        [TestCase(191912, false)]
        public void GetAllDoctorWithSpecificSpecialtyId(long specialty_id, bool success)
        {
            dynamic res = controller.Getdoctor(specialty_id);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);
        }

    }
}