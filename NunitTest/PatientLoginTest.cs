using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;
using api.Controllers;

namespace api.NunitTest
{
    [TestFixture]
    public class PatientLoginTest
    {
        // patient/otp
        [TestCase("dipa2an@yahoo.com", true)]
        public void patient_verificationCode(string email, bool result)
        {
            LoginController con = new LoginController();

            patient_login log = new patient_login {email = email };
            dynamic var1 = con.getPatientLoginVerificationCode(log);

            Assert.AreEqual(var1.Content.success, result);
        }

        [TestCase("dipa2an1@yahoo.com", false)]
        public void patient_verificationCode_email(string email, bool result)
        {
            LoginController con = new LoginController();

            patient_login log = new patient_login { email = email };
            dynamic var1 = con.getPatientLoginVerificationCode(log);

            Assert.AreEqual(var1.Content.success, result);
        }

        [TestCase("", false)]
        public void patient_verificationCode_empty(string email, bool result)
        {
            LoginController con = new LoginController();

            patient_login log = new patient_login { email = email };
            dynamic var1 = con.getPatientLoginVerificationCode(log);

            Assert.AreEqual(var1.Content.success, result);
        }



        // patient/profile/signup
        [TestCase("dipa2an@yahoo.com", "password", "neil", "ryan", "9/1/1800", "Male", true)]
        public void patientProf_signup(string email, string password, string name_first, string name_last, string dob, string gender, bool result)
        {
            //IsRequired("email", patient.email, 1);
            //IsRequired("password", patient.password, 1);
            //IsRequired("name_first", patient.name_first, 1);
            //IsRequired("name_last", patient.name_last, 1);
            //IsRequired("dob", patient.dob, 1);
            //IsRequired("gender", patient.gender, 1);

            patient_signup sig = new patient_signup {
                email = email, password = password, name_first = name_first, name_last = name_last, dob = dob, gender = gender
            };

            LoginController con = new LoginController();
            dynamic var1 = con.postPatientSignup(sig);

            Assert.AreEqual(var1.Content.success, result);
        }

        [TestCase("dipa2an@yahoo.com", "password", "", "ryan", "9/1/1800", "Male", false)]
        public void patientProf_required(string email, string password, string name_first, string name_last, string dob, string gender, bool result)
        {
            //IsRequired("email", patient.email, 1);
            //IsRequired("password", patient.password, 1);
            //IsRequired("name_first", patient.name_first, 1);
            //IsRequired("name_last", patient.name_last, 1);
            //IsRequired("dob", patient.dob, 1);
            //IsRequired("gender", patient.gender, 1);

            patient_signup sig = new patient_signup
            {
                email = email,
                password = password,
                name_first = name_first,
                name_last = name_last,
                dob = dob,
                gender = gender
            };

            LoginController con = new LoginController();
            dynamic var1 = con.postPatientSignup(sig);

            Assert.AreEqual(var1.Content.success, result);
        }

        [TestCase("dipa2an@yahoo.com", "password", "neil", "ryan", "9/1/1800", "Male", false)]
        public void patientProf_emailexist(string email, string password, string name_first, string name_last, string dob, string gender, bool result)
        {
           
            patient_signup sig = new patient_signup
            {
                email = email,
                password = password,
                name_first = name_first,
                name_last = name_last,
                dob = dob,
                gender = gender
            };

            LoginController con = new LoginController();
            dynamic var1 = con.postPatientSignup(sig);

            Assert.AreEqual(var1.Content.success, result);
        }



        // patient/login 
        [TestCase("dipa2an@yahoo.com", "password", true)]
        public void patientLogin(string email, string password, bool result)
        {
            // patient/login

            patient_login pat = new patient_login {
                email = email,  password = password
            };

            LoginController cont = new LoginController();
            dynamic var1 = cont.getPatientLoginPassword(pat);

            Assert.AreEqual(var1.Content.success, result);

        }

        [TestCase("dipa2an1@yahoo.com", "12345", false)]
        public void patientLogin_wrongEmail(string email, string password, bool result)
        {
            // patient/login
            patient_login pat = new patient_login
            {
                email = email,
                password = password
            };

            LoginController cont = new LoginController();
            dynamic var1 = cont.getPatientLoginPassword(pat);

            Assert.AreEqual(var1.Content.success, result);
        }

        [TestCase("dipa2an@yahoo.com", "12345", false)]
        public void patientLogin_wrongPassword(string email, string password, bool result)
        {
            // patient/login
            patient_login pat = new patient_login {
                email = email, password = password
            };

            LoginController cont = new LoginController();
            dynamic var1 = cont.getPatientLoginPassword(pat);

            Assert.AreEqual(var1.Content.success, result);
        }


        // patient/email
        [TestCase("dipa2an@yahoo.com", true)]
        public void patientLoginUsername(string email, bool success)
        {
            //patient/email
            patient_login pat = new patient_login {
                email = email
            };

            LoginController login = new LoginController();

            dynamic var1 = login.getPatientLoginUsername(pat);

            Assert.AreEqual(var1.Content.success, success);
        }

        [TestCase("dipa2an1@yahoo.com", false)]
        public void patientLoginUsername1(string email, bool success)
        {
            // patient/email
            patient_login pat = new patient_login
            {
                email = email
            };

            LoginController login = new LoginController();

            dynamic var1 = login.getPatientLoginUsername(pat);

            Assert.AreEqual(var1.Content.success, success);
        }

        [TestCase("", false)]
        public void patientLoginUsername2(string email, bool success)
        {
            // patient/email

            patient_login pat = new patient_login
            {
                email = email
            };

            LoginController login = new LoginController();

            dynamic var1 = login.getPatientLoginUsername(pat);

            Assert.AreEqual(var1.Content.success, success);
        }
    }
}