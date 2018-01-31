using api.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.TestUnit
{
    [TestFixture]
    public class TestSearchDoctor
    {
        [Test]
        public void test_specialty()
        {

            SearchDoctorController sd = new SearchDoctorController();

            param_getall all = new param_getall();
            //all.city = "Alexandria, VA";

            dynamic res = sd.get_allsearch(all);

            Assert.AreEqual(res.Content.success, true);
        }
    }
}