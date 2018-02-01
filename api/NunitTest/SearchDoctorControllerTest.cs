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
        //lat=41.851482&longi=-87.669444&search=arthritis&insurance_id=1&take=25&skip=0
        [TestCase(41.851482, 87.669444, "arthritis", 1, false)]
        [TestCase(41.851482, 87.669444, "Hospice", 1, false)]
        [TestCase(41.851482, 87.669444, 123, 1, false)]
        public void modile_doctor_search_loguser(double lat, double longi, string search, long insurance_id, bool success)
        {
            SearchDoctorController sd = new SearchDoctorController();

            var data = new doc_search_mobile_loguser()
            {
                lat = lat,
                longi = longi,
                search = search,
                insurance_id = insurance_id

              };

            dynamic res = sd.getDoctor_search_mobile_loguser(data);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);

        }
        //return doctor near the users current location
        [TestCase(41.851482, 87.669444, true)]
        [TestCase(1, 1, false)]
        [TestCase(1, "", false)]
        public void get_doctors(double lat, double longi, bool success)
        {
            SearchDoctorController sd = new SearchDoctorController();

            var data = new doc_search_mobile()
            {
                lat = lat,
                longi = longi
            };

            dynamic res = sd.getDoctor_search_mobile(data);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);
        }
    }
}