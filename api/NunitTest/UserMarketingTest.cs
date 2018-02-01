using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;
using api.Controllers;

namespace api.NunitTest
{
    [TestFixture]
    public class UserMarketingTest
    {
        #region post_market mkt
        public post_market mkt = new post_market
        {
            authorize_signatory_name_first = "auto1",
            authorize_signatory_name_last = "auto2",
            authorize_signatory_phone = "auto3",
            authorize_signatory_email = "auto9@ahs.com",

            legal_entity_name = "legal now",
            healthsplash_contact = "needs hscontact",
            address1 = "lasd",
            address2 = "",
            city = "Aguadilla",
            state = "Puerto Rico",
            zip = "00604",
            federal_tax_number = "1000000",
            primary_phone = "888888888",
            supplier_partner = "superintendent",
            telemedicine_partner = "telefon",
            primary_contact_name_first = "primero",
            primary_contact_name_last = "contact",
            primary_contact_phone = "11111",
            primary_contact_email = "11111",
            financial_name_first = "fino1",
            financial_name_last = "fino2",
            financial_phone = "fino3",
            financial_email = "fino4",

            operational_name_first = "not required1",
            operational_name_last = "not required2",
            operational_phone = "notrequirrtyergdfgdfghfg",
            operational_email = "not require4",
            fax_number = "fax not requred",
            process_external_transfer = "extrno transfer not required",
            intake_entity = "intake not required",
            technology_platform = "tech platform not requreid",
            //Technology Platform for intake
            pharmacy_relationship = "farma rela not required",
            pharmacy_customer_number = "farma customer not req",
            //Unique Customer number with Pharmacy
            pharmacy_fax = "farm fax not req",
            preferred_geographic_market = "prefered mercado not req"

        };
        #endregion

        #region post_market mkt_req
        public post_market mkt_req = new post_market
        {
            authorize_signatory_name_first = "auto1",
            authorize_signatory_name_last = "auto2",
            authorize_signatory_phone = "auto3",
            authorize_signatory_email = "auto7@ahs.com",

            legal_entity_name = "",
            healthsplash_contact = "",
            address1 = "",
            address2 = "",
            city = "",
            state = "",
            zip = "",
            federal_tax_number = "1000000",
            primary_phone = "888888888",
            supplier_partner = "superintendent",
            telemedicine_partner = "telefon",
            primary_contact_name_first = "primero",
            primary_contact_name_last = "contact",
            primary_contact_phone = "11111",
            primary_contact_email = "11111",
            financial_name_first = "fino1",
            financial_name_last = "fino2",
            financial_phone = "fino3",
            financial_email = "fino4",

            operational_name_first = "not required1",
            operational_name_last = "not required2",
            operational_phone = "notrequirrtyergdfgdfghfg",
            operational_email = "not require4",
            fax_number = "fax not requred",
            process_external_transfer = "extrno transfer not required",
            intake_entity = "intake not required",
            technology_platform = "tech platform not requreid",
            //Technology Platform for intake
            pharmacy_relationship = "farma rela not required",
            pharmacy_customer_number = "farma customer not req",
            //Unique Customer number with Pharmacy
            pharmacy_fax = "farm fax not req",
            preferred_geographic_market = "prefered mercado not req"

        };

        #endregion


        [Test]
        public void testpostMarketing()
        {
            bool success = true;

            MarketerController m = new MarketerController();

        
            //m.postMarketing(mkt);

            dynamic var1 = m.postMarketing(mkt);

            Assert.AreEqual(var1.Content.success, success);
        }

        [Test]
        public void testpostMarketing_email()
        {
            bool success = false;

            MarketerController m = new MarketerController();


            //m.postMarketing(mkt);

            dynamic var1 = m.postMarketing(mkt);

            Assert.AreEqual(var1.Content.success, success);
        }

        [Test]
        public void testpostMarketing_isrequired()
        {
            bool success = false;

            MarketerController m = new MarketerController();

           
            //m.postMarketing(mkt);

            dynamic var1 = m.postMarketing(mkt_req);

            Assert.AreEqual(var1.Content.success, success);
        }
    }
}