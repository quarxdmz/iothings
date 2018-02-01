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
    public class AppointmentSchedControllerTest
    {
        HttpControllerContext controllerContext;
        AppointmentSchedController controller;
        [SetUp]
        public void SetUp()
        {
            controllerContext = new HttpControllerContext();

            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            controllerContext.Request = new HttpRequestMessage();

            controllerContext.Request.Headers.Add("Basic", "password");

            controller = new AppointmentSchedController();

            controller.ControllerContext = controllerContext;
        }

        [Test, Ignore("this")]
        public void ScheduleAppointment()
        {
            //have to change the code to use params instead of httpcontext
        }

        [TestCase("122", "", false)]
        [TestCase("0", "", false)]
        public void GetDoctorTimeslots(string doctor_id, string date, bool success)
        {
            dynamic res = controller.timeslot(doctor_id, date);

            Console.WriteLine(res.Content.success);
            
            Assert.AreEqual(res.Content.success, success);
        }
    }
}