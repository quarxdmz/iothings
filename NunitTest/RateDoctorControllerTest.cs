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
    public class RateDoctorControllerTest
    {
        HttpControllerContext controllerContext;
        RateDoctorController controller;
        [SetUp]
        public void SetUp()
        {
            controllerContext = new HttpControllerContext();

            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            controllerContext.Request = new HttpRequestMessage();

            controllerContext.Request.Headers.Add("Basic", "password");

            controller = new RateDoctorController();

            controller.ControllerContext = controllerContext;
        }

        [TestCase(0, 0, false)]
        public void GetDoctorRate(long doctor_id, long user_id, bool success)
        {
            dynamic res = controller.GetRate(doctor_id, user_id);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);
        }

        //[Test, Ignore("Have to change code!")]
        public void DeleteDoctorRating()
        {

        }

        //[Test, Ignore("Have to change code!")]
        public void AddDoctorRating()
        {

        }

    }
}