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
    public class DoctorControllerTest
    {
        HttpControllerContext controllerContext;
        DoctorController controller;
        [SetUp]
        public void SetUp()
        {
            controllerContext = new HttpControllerContext();

            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            controllerContext.Request = new HttpRequestMessage();

            controllerContext.Request.Headers.Add("Basic", "password");

            controller = new DoctorController();

            controller.ControllerContext = controllerContext;
        }

        [TestCase("1114022555", true)]
        [TestCase("111402255", false)]
        public void getDoctorclaim(string npi, bool success)
        {

            dynamic res = controller.getDoctorclaim(npi);

            Console.WriteLine(res.Content.success);

            Assert.AreEqual(res.Content.success, success);

        }

        [Test, Ignore("Will have to update code!")]
        public void UserAddFavoriteDoctor()
        {

        }

        [TestCase("1", false)]
        [TestCase("358", false)]
        public void GetUserFavoriteDoctor(string user_id, bool success)
        {
            dynamic res = controller.getfavoritedoctor(user_id);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);
        }

        [TestCase("8913225471", true)]
        [TestCase("89132254711", false)]
        public void GetDoctorInformation(string npi, bool success)
        {
            dynamic res = controller.getPractice(npi);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);
        }
    }
}