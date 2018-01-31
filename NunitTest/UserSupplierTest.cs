using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;
using api.Controllers;

namespace api.NunitTest
{
    public class UserSupplierTest
    {

        #region post_supplier mkt 
        post_supplier mkt = new post_supplier
        {
            authorize_signatory_name_first = "authorize_signatory_name_first",
            authorize_signatory_name_last = "authorize_signatory_name_last",
            authorize_signatory_phone = "authorize_signatory_phone",
            authorize_signatory_email = "authorize_signatory_email2",

            legal_entity_name = "legal_entity_name",
            healthsplash_contact = "healthsplash_contact",
            address1 = "address1",
            address2 = "",
            city = "Glyndon",
            state = "Minnesota",
            zip = "56547",
            //zip_id { get; set; }
            federal_tax_number = "federal_tax_number",
            primary_phone = "primary_phone",
            // primary_phone
            telemedicine_partner = "telemedicine_partner",
            organization_npi = "organization_npi",
            preferred_geographic_market = "preferred_geographic_market",
            insurance_providers = "insurance_providers",
            //List of your in-network/accepted insurance providers* 
            products_hcpcs = "products_hcpcs",
            patient_deductible_threshold = "patient_deductible_threshold",
            primary_contact_name_first = "primary_contact_name_first",
            primary_contact_name_last = "primary_contact_name_last",
            primary_contact_phone = "primary_contact_phone",
            primary_contact_email = "primary_contact_email",
            financial_name_first = "financial_name_first",
            financial_name_last = "financial_name_last",
            financial_phone = "financial_phone",
            financial_email = "financial_email",
           
            operational_name_first = "operational_name_first",
            operational_name_last = "operational_name_last",
            operational_phone = "operational_phone",
            operational_email = "operational_email",
            fax_number = "fax_number",
            process_external_transfer = "process_external_transfer",
            marketer_partner = "marketer_partner",
            intake_entity = "intake_entity",
            technology_platform = "technology_platform",
            //Technology Platform for intake
            pharmacy_relationship = "pharmacy_relationship",
            //Pharmacy Relationship (If applicable; include multiple if needed) 
            pharmacy_customer_number = "pharmacy_customer_number",
            // Unique Customer number with Pharmacy
            pharmacy_fax = "pharmacy_fax",
            ptan_number = "ptan_number"


        };
        #endregion

        #region  post_supplier mkt_required
        post_supplier mkt_required = new post_supplier
        {
            legal_entity_name = "legal_entity_name",
            healthsplash_contact = "healthsplash_contact",
            address1 = "address1",
            address2 = "",
            city = "",
            state = "",
            zip = "",
            //zip_id { get; set; }
            federal_tax_number = "federal_tax_number",
            primary_phone = "primary_phone",
            // primary_phone
            telemedicine_partner = "telemedicine_partner",
            organization_npi = "organization_npi",
            preferred_geographic_market = "preferred_geographic_market",
            insurance_providers = "insurance_providers",
            //List of your in-network/accepted insurance providers* 
            products_hcpcs = "products_hcpcs",
            patient_deductible_threshold = "patient_deductible_threshold",
            primary_contact_name_first = "primary_contact_name_first",
            primary_contact_name_last = "primary_contact_name_last",
            primary_contact_phone = "primary_contact_phone",
            primary_contact_email = "primary_contact_email",
            financial_name_first = "financial_name_first",
            financial_name_last = "financial_name_last",
            financial_phone = "financial_phone",
            financial_email = "financial_email",
            authorize_signatory_name_first = "authorize_signatory_name_first",
            authorize_signatory_name_last = "authorize_signatory_name_last",
            authorize_signatory_phone = "authorize_signatory_phone",
            authorize_signatory_email = "authorize_signatory_email1",
            operational_name_first = "operational_name_first",
            operational_name_last = "operational_name_last",
            operational_phone = "operational_phone",
            operational_email = "operational_email",
            fax_number = "fax_number",
            process_external_transfer = "process_external_transfer",
            marketer_partner = "marketer_partner",
            intake_entity = "intake_entity",
            technology_platform = "technology_platform",
            //Technology Platform for intake
            pharmacy_relationship = "pharmacy_relationship",
            //Pharmacy Relationship (If applicable; include multiple if needed) 
            pharmacy_customer_number = "pharmacy_customer_number",
            // Unique Customer number with Pharmacy
            pharmacy_fax = "pharmacy_fax",
            ptan_number = "ptan_number"


        };
        #endregion
        [Test]
        public void testpostSupplier()
        {
            bool success = true;

            SupplierController m = new SupplierController();

        
            //m.postMarketing(mkt);

            dynamic var1 = m.postSupplier(mkt);

            Assert.AreEqual(var1.Content.success, success);
        }

        [Test]
        public void testpostSupplier_email()
        {
            bool success = false;

            SupplierController m = new SupplierController();


            //m.postMarketing(mkt);

            dynamic var1 = m.postSupplier(mkt);

            Assert.AreEqual(var1.Content.success, success);
        }

        [Test]
        public void testpostSupplier_isrequired()
        {
            bool success = false;


            SupplierController m = new SupplierController();

          
            //m.postMarketing(mkt);

            dynamic var1 = m.postSupplier(mkt_required);

            Assert.AreEqual(var1.Content.success, success);
        }
    }
}