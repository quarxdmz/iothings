using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;


namespace api.Controllers
{
    [TestFixture]
    public class TestController
    {

        //**https://www.zocdoc.com/directory

        [TestCase("vi", false, true)]
        [TestCase("", true, true)]
        public void testdirectory_all(string city, bool recent, bool success)
        {
            SearchDoctorController sd = new SearchDoctorController();
            param_directory par = new param_directory{
                is_date = recent, letter = city
            };

            dynamic res = sd.getDirectory(par);

            Assert.AreEqual(res.Content.success, success);
        }
        // ======================================

        [TestCase("Alexandria", true)]
        [TestCase("Savannah", true)]
        public void testSearchDoctor(string city, bool success)
        {
            SearchDoctorController sd = new SearchDoctorController();
            Models.doc_search_query par = new Models.doc_search_query();
            par.city = city; //par.specialty = 3;

            System.Web.HttpContext.Current = new System.Web.HttpContext(new System.Web.HttpRequest(null, "http://localhost", null), new System.Web.HttpResponse(null));
            System.Web.Http.Controllers.HttpControllerContext controllerContext = new System.Web.Http.Controllers.HttpControllerContext();
            controllerContext.Request = new HttpRequestMessage();

            dynamic res = sd.getDoctor_search(par);

            Console.WriteLine(res.Content.message);
            Assert.AreEqual(res.Content.success, success);
        }

        [TestCase("AL", true)]
        public void testSearchDoctor_state(string state, bool success)
        {
            SearchDoctorController sd = new SearchDoctorController();
            Models.doc_search_query par = new Models.doc_search_query();
            par.lat = 38.755769; par.longi = -77.085389;
            par.state = state; //par.specialty = 3;


            dynamic res = sd.getDoctor_search(par);

            Console.WriteLine(res.Content.message);
            Assert.AreEqual(res.Content.success, success);
        }

        [TestCase("10950", true)]
        [TestCase("48109", true)]
        [TestCase("10951", false)]
        [TestCase("", false)]
        public void testSearchDoctor_zip(string zip, bool success)
        {
            SearchDoctorController sd = new SearchDoctorController();
            Models.doc_search_query par = new Models.doc_search_query();
            //par.lat = 38.755769; par.longi = -77.085389;
            par.zipcode = zip; //par.specialty = 3;


            dynamic res = sd.getDoctor_search(par);

          //  Console.WriteLine(res.data);
            Assert.AreEqual(res.Content.success, success);
        }


        [TestCase(38.755769, -77.085389, true)]
        //[ExpectedException("Exception")]
        public void testSearchDoctor_latlong(double lat, double longi, bool success)
        {
            SearchDoctorController sd = new SearchDoctorController();
            Models.doc_search_query par = new Models.doc_search_query();
            par.lat = lat; par.longi = longi; //par.specialty = 3;


            dynamic res = sd.getDoctor_search(par);

            Console.WriteLine(res.Content.message);
            Assert.AreEqual(res.Content.success, success);

       }

     [TestCase(true)]  
       public void testSearchDoctor_( bool success)
        {
            SearchDoctorController sd = new SearchDoctorController();
            Models.doc_search_query par = new Models.doc_search_query();
            par.lat = 38.755769; par.longi = -77.085389; //par.specialty = 3;
            par.city = "Ann Arbor";

            dynamic res = sd.getDoctor_search(par);

            Console.WriteLine(res.Content.message);
            Assert.AreEqual(res.Content.success, success);

        }

        [Test]
        public void testSearchDoctor_noparam()
        {
            SearchDoctorController sd = new SearchDoctorController();
            Models.doc_search_query par = new Models.doc_search_query();
            //par.lat = 38.755769; par.longi = -77.085389; //par.specialty = 3;
            //par.city = "Ann Arbor";

            dynamic res = sd.getDoctor_search(par);

            Console.WriteLine(res.Content.message);
            Assert.AreEqual(res.Content.success, false);

        }

        //[Test]
        //public void Add_EmptyString_Returns_0()
        //{
        //    StringCalculator calc = new StringCalculator();
        //    int expectedResult = 0;
        //    int result = calc.Add("");
        //    Assert.AreEqual(expectedResult, result);
        //}

        ////public void Add_MultipleNumbers_SumOfAllNumbers
        //[TestCase("1", 1)]
        //[TestCase("2", 2)]
        //[TestCase("3", 3)]
        //[TestCase("6", 6)]
        //[TestCase("9", 9)]
        //public void Add_SingleNumbers_ReturnsTheNumber(string input, string expectedResult)
        //{
        //    StringCalculator calc = new StringCalculator();
        //    //var expectedResult = 3;
        //    int result = calc.Add(input);
        //    Assert.AreEqual(expectedResult, result);
        //}


    }

    public class response
    {
        public string data { get; set; }
        public string message { get; set; }
        public string success { get; set; }
    }

    public class StringCalculator
    {
        public int Add(string input)
        {
            if (string.IsNullOrEmpty(input)) return 0;

            var numbers = input.Split(',');
            var total = 0;
            foreach (var number in numbers) {
                total += int.Parse(number);
            }
            return total;
        }
    }

}
