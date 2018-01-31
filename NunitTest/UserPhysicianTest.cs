using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;
using api.Controllers;

namespace api.NunitTest
{
    [TestFixture]
    public class UserPhysicianTest
    {
        #region post_physician mkt
        post_physician mkt = new post_physician
        {
            legal_entity_name = "entity required",
            healthsplash_contact = "hs requ",
            address1 = " asdfsdf",
            address2 = "asdfsd",
            city = "adfdsf",
            state = "sdfsdf",
            zip = "dsafasdf",
            //zip_id { get; set; }
            practice_type = "pediatrinc",
            // Practice Type(Family practice, pediatrician, urgent care, house call group, telemedicine, etc.)*
            federal_tax_number = "sadfasfdsf",
            primary_phone = "asdfsdf",
            // primary_phone
            customer_service_number = "asfdsdf",
            number_clinicians = 0,
            number_exams_per_week = 0,
            practice_npi = "asfsdfdf",
            geographic_market = "asdfsdfd",
            insurance_providers = "asdfsdf",
            //List of your in-network/accepted insurance providers*
            EMR_software_used = "asdfsdf",
            //EMR software you are currently using*
            primary_contact_name_first = "asdfsdf",
            primary_contact_name_last = "asdfsdf",
            primary_contact_phone = "asdfsd",
            primary_contact_email = "asdfsdf",
            financial_name_first = "asdf",
            financial_name_last = "sadfsad",
            financial_phone = "asdf",
            financial_email = "asd",
            authorize_signatory_name_first = "asdf",
            authorize_signatory_name_last = "asdfs",
            authorize_signatory_phone = "ssadfsa",
            authorize_signatory_email = "asdfsdf",
            operational_name_first = "asdf",
            operational_name_last = "sadfas",
            operational_phone = "sdf",
            operational_email = "asdf",
            future_expansion = "sdf",
            current_scheduling_solution = "sdfa"
            //Current Scheduling Solution (House calls and Home Health only)


        };
        #endregion

        #region  post_physician mkt_required
        post_physician mkt_required = new post_physician
        {
            legal_entity_name = "",
            healthsplash_contact = "hs requ",
            address1 = " asdfsdf",
            address2 = "asdfsd",
            city = "adfdsf",
            state = "sdfsdf",
            zip = "dsafasdf",
            //zip_id { get; set; }
            practice_type = "pediatrinc",
            // Practice Type(Family practice, pediatrician, urgent care, house call group, telemedicine, etc.)*
            federal_tax_number = "sadfasfdsf",
            primary_phone = "asdfsdf",
            // primary_phone
            customer_service_number = "asfdsdf",
            number_clinicians = 0,
            number_exams_per_week = 0,
            practice_npi = "asfsdfdf",
            geographic_market = "asdfsdfd",
            insurance_providers = "asdfsdf",
            //List of your in-network/accepted insurance providers*
            EMR_software_used = "asdfsdf",
            //EMR software you are currently using*
            primary_contact_name_first = "asdfsdf",
            primary_contact_name_last = "asdfsdf",
            primary_contact_phone = "asdfsd",
            primary_contact_email = "asdfsdf",
            financial_name_first = "asdf",
            financial_name_last = "sadfsad",
            financial_phone = "asdf",
            financial_email = "asd",
            authorize_signatory_name_first = "asdf",
            authorize_signatory_name_last = "asdfs",
            authorize_signatory_phone = "ssadfsa",
            authorize_signatory_email = "asdfsdf",
            operational_name_first = "asdf",
            operational_name_last = "sadfas",
            operational_phone = "sdf",
            operational_email = "asdf",
            future_expansion = "sdf",
            current_scheduling_solution = "sdfa"
            //Current Scheduling Solution (House calls and Home Health only)
        };
        #endregion

        [Test]
        public void testpostPhysician()
        {
            bool success = true;

            PhysicianController m = new PhysicianController();

           
            //m.postMarketing(mkt);

            dynamic var1 = m.postPhysician(mkt);

            Assert.AreEqual(var1.Content.success, success);
        }


        [Test]
        public void testpostPhysician_email()
        {
            bool success = false;

            PhysicianController m = new PhysicianController();


            //m.postMarketing(mkt);

            dynamic var1 = m.postPhysician(mkt);

            Assert.AreEqual(var1.Content.success, success);
        }

        [Test]
        public void testpostPhysician_isrequired()
        {
            bool success = false;

            PhysicianController m = new PhysicianController();

           
            //m.postMarketing(mkt);

            dynamic var1 = m.postPhysician(mkt_required);

            Assert.AreEqual(var1.Content.success, success);
        }
    }
}