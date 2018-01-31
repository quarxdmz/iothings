using api.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using api.Models;

namespace api.NunitTest
{
    [TestFixture]
    public class AppointmentControllerTest
    {
        HttpControllerContext controllerContext;
        AppointmentController controller;
        
        [SetUp]
        public void SetUp()
        {
            controllerContext = new HttpControllerContext();

            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            controllerContext.Request = new HttpRequestMessage();

            controllerContext.Request.Headers.Add("Basic", "password");

            controller = new AppointmentController();

            controller.ControllerContext = controllerContext;
        }

        [Test, Ignore("this")]
        public void NewAppointment()
        {
            //dynamic res = controller.newAppointment(){
            //    "patient_id" : "305"
            //};

            //Console.WriteLine(res.Content.message);

            //Assert.AreEqual(res.Content.success, success);
        }

        [Test, Ignore("this")]
        public void CancelAppointment()
        {
            //have to change the code to use params instead of httpcontext
        }

        [Test, Ignore("this")]
        public void DeleteAppointment()
        {
            //have to change the code to use class instead of httpcontext
        }

        [TestCase("658", false)]
        [TestCase("1", false)]
        public void GetAppointmentByAppointmentID(string appointment_id, bool success)
        {
            dynamic res = controller.Getappointment(appointment_id);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);
        }

        [TestCase("219", true)]
        [TestCase("", false)]
        public void GetSpecialtyPatientAppointmentType(string user_id, bool success)
        {
            dynamic res = controller.Getappointmentspecialty(user_id);
            
            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);
        }

        [TestCase("626", "", "", true)]
        [TestCase("", "1", "", false)]
        public void Getcurrent(string patient_id, string user_id, string appointment_type, bool success)
        {
            dynamic res = controller.Getcurrent(patient_id, user_id, appointment_type);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);

        }

        [TestCase("" , true)]
        public void GetDoctorDashboard(string doctor_id, bool success)
        {
            dynamic res = controller.getdoctordashboard();

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);
        }

        [TestCase("", "", "", false)]
        public void GetPatientAppointmentHistory(string patient_id, string user_id, string appointment_type, bool success)
        {
            dynamic res = controller.Gethistory(patient_id, user_id, appointment_type);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);
        }
        
        [TestCase("", false)]
        [TestCase("626", true)]
        public void GetPatientAppointments(string patient_id, bool success)
        {
            dynamic res = controller.Getpatientappointment(patient_id);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);
        }

        [TestCase("1", "", "", false)]
        [TestCase("626", "", "", true)]
        public void GetPendingAppointments(string patient_id, string user_id, string appointment_type, bool success)
        {
            dynamic res = controller.Getpending(patient_id, user_id, appointment_type);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);
        }










    }
}