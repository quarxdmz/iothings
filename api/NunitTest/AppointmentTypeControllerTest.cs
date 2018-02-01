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
    public class AppointmentTypeControllerTest
    {
        HttpControllerContext controllerContext;
        AppointmentTypeController controller;
        [SetUp]
        public void SetUp()
        {
            controllerContext = new HttpControllerContext();

            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            controllerContext.Request = new HttpRequestMessage();

            controllerContext.Request.Headers.Add("Basic", "password");

            controller = new AppointmentTypeController();

            controller.ControllerContext = controllerContext;
        }

        [TestCase(1,true)]
        [TestCase(0, false)]
        public void GetAppointmentTypeDetails(long appointment_type_id, bool success)
        {

            dynamic res = controller.getappointmenttype(appointment_type_id);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);

        }
    }
}