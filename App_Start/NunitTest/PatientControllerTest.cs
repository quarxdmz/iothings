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
    public class PatientControllerTest
    {
        HttpControllerContext controllerContext;
        PatientController controller;
        [SetUp]
        public void SetUp()
        {
            controllerContext = new HttpControllerContext();

            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            controllerContext.Request = new HttpRequestMessage();

            controllerContext.Request.Headers.Add("Basic", "password");

            controller = new PatientController();

            controller.ControllerContext = controllerContext;
        }

        [TestCase(620, true)]
        public void GetPatientInformation(long id, bool success)
        {
            dynamic res = controller.Get(id);

            Console.WriteLine(res.Content.success);

            Assert.AreEqual(res.Content.success, success);
        }

        //[Test, Ignore("Have to change code!")]
        public void AddNewPatientToUser()
        {

        }

        //[Test, Ignore("Have to change code!")]
        public void UpdatePatient()
        {

        }



    }
}