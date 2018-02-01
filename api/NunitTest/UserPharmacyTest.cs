using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;
using api.Controllers;

namespace api.NunitTest
{
    [TestFixture]
    public class UserPharmacyTest
    {
        #region post_pharmacy mkt
        public post_pharmacy mkt = new post_pharmacy
        {
            authorize_signatory_name_first = "asdfsdf",
            authorize_signatory_name_last = "asdfsdf",
            authorize_signatory_phone = "asdfsdf",
            authorize_signatory_email = "asdfsdf1",

            legal_entity_name = "farmacia",
            healthsplash_contact = "fadsdfsdf",
            address1 = "pkjyrbe",
            address2 = "",
            city = " Washington",
            state = "dc",
            zip = "20412",
            //zip_id { get; set; }
            specialty_services = "sdfadd",
            //Specialty Services/Company Type
            federal_tax_number = "sdsfsd",
            primary_phone = "asfsd",
            customer_service_number = "asfdfsd",
            technology_platform_retrieval = "asdfdf",
            // Technology Platform for Retrieval of Records (i.e. API, Fax, SharePoint)*
            ncpdp_number = "asdfsdf",
            practice_npi_number = "sfdsadfs",
            primary_contact_name_first = "asfs",
            primary_contact_name_last = "sdfsd",
            primary_contact_phone = "asdfsfd",
            primary_contact_email = "asdfsdfsd",
            financial_name_first = "asdfsdf",
            financial_name_last = "asdfsdf",
            financial_phone = "asdfsdf",
            financial_email = "asdfsdf",
            
            operational_name_first = "sdfsf",
            operational_name_last = "sd",
            operational_phone = "cc",
            operational_email = "ccc",
            fax_number = "ccc",
            marketer_partner = "cc",
            //Marketer (If applicable, include all)
            pharmacy_fax_number = "cc",
            geographic_market = "ccc",
            insurance_provider = "cca",
            //List of your in-network/accepted insurance providers
            product_list = "sdfsdf"
        };
        #endregion

        #region post_pharmacy mkt_required
        public post_pharmacy mkt_required = new post_pharmacy
        {
            legal_entity_name = "farmacia",
            healthsplash_contact = "fadsdfsdf",
            address1 = "pkjyrbe",
            address2 = "",
            city = " Washington",
            state = "dc",
            zip = "20412",
            //zip_id { get; set; }
            specialty_services = "sdfadd",
            //Specialty Services/Company Type
            federal_tax_number = "sdsfsd",
            primary_phone = "asfsd",
            customer_service_number = "asfdfsd",
            technology_platform_retrieval = "asdfdf",
            // Technology Platform for Retrieval of Records (i.e. API, Fax, SharePoint)*
            ncpdp_number = "asdfsdf",
            practice_npi_number = "sfdsadfs",
            primary_contact_name_first = "asfs",
            primary_contact_name_last = "sdfsd",
            primary_contact_phone = "asdfsfd",
            primary_contact_email = "asdfsdfsd",
            financial_name_first = "asdfsdf",
            financial_name_last = "asdfsdf",
            financial_phone = "asdfsdf",
            financial_email = "asdfsdf",
            authorize_signatory_name_first = "asdfsdf",
            authorize_signatory_name_last = "asdfsdf",
            authorize_signatory_phone = "asdfsdf",
            authorize_signatory_email = "asdfsdf",
            operational_name_first = "sdfsf",
            operational_name_last = "sd",
            operational_phone = "cc",
            operational_email = "ccc",
            fax_number = "ccc",
            marketer_partner = "cc",
            //Marketer (If applicable, include all)
            pharmacy_fax_number = "cc",
            geographic_market = "ccc",
            insurance_provider = "cca",
            //List of your in-network/accepted insurance providers
            product_list = "sdfsdf"
        };
        #endregion

        [Test]
        public void testpostPharmacy()
        {
            bool success = true;

            PharmacyController m = new PharmacyController();
          
            //m.postMarketing(mkt);

            dynamic var1 = m.postPharmacy(mkt);

            Assert.AreEqual(var1.Content.success, success);
        }

        [Test]
        public void testpostPharmacy_email()
        {
            bool success = false;

            PharmacyController m = new PharmacyController();


            //m.postMarketing(mkt);

            dynamic var1 = m.postPharmacy(mkt);

            Assert.AreEqual(var1.Content.success, success);
        }

        [Test]
        public void testpostPharmacy_isrequired()
        {
            bool success = false;

            PharmacyController m = new PharmacyController();

          

            dynamic var1 = m.postPharmacy(mkt_required);

            Assert.AreEqual(var1.Content.success, success);
        }
    }
}