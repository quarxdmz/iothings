using api.Controllers;
using api.Models;
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
    public class SearchDoctorControllerTest
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

        //public void SearchDoctorsByKeys()
        //{
        //    dynamic res = controller.getwellnessexam(user_id);

        //    Console.WriteLine(res.Content.message);

        //    Assert.AreEqual(res.Content.success, success);
        //}

        [Test]
        public void test_getall_serach()
        {
            SearchDoctorController sdoc = new SearchDoctorController();
            param_getall par = new param_getall {
                doctor_name = "AAKALU"
            };
            dynamic res = sdoc.get_allsearch(par);
              
        }

        [Test]
        public void doctor_city_state()
        {
            SearchDoctorController sdoc = new SearchDoctorController();
          
            dynamic res = sdoc.getDoctor_states();

        }
    }
}