using api.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.TestUnit
{
    [TestFixture]
    public class TestDoctorController
    {
        [Test]
        public void testsave() {
            DoctorController doc = new DoctorController();

            post_doctor_profile prof = new post_doctor_profile {
                npi = "123568991",
                first_name = "unit",
                last_name = "test",
                email = "unit@teset.com"
            };
           dynamic v =  doc.postDoctorProfile(prof);
        }
    }
}