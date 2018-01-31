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
    public class PatientInsuranceControllerTest
    {
        HttpControllerContext controllerContext;
        PatientInsuranceController controller;
        [SetUp]
        public void SetUp()
        {
            controllerContext = new HttpControllerContext();

            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            controllerContext.Request = new HttpRequestMessage();

            controllerContext.Request.Headers.Add("Basic", "password");

            controller = new PatientInsuranceController();

            controller.ControllerContext = controllerContext;
        }

        [TestCase(465, false)]
        public void GetPatientInsurance(long patient_id, bool success)
        {
            dynamic res = controller.get(patient_id);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);

        }

        //[TestCase(465, 1, 465, true), Ignore("have to debug this")]
        public void AddNewInsuranceToPatient(long patient_id, long insurance_id, long user_id, bool success)
        {
            dynamic res = controller.Post(patient_id, insurance_id, user_id);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);
        }

        [TestCase(465, 1, 465, false)]
        public void DeletePatientInsurance(long patient_id, long insurance_id, long user_id, bool success)
        {
            dynamic res = controller.Delete(patient_id, insurance_id, user_id);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);
        }

    }
}