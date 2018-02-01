using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using api.Models;
using Newtonsoft.Json;

namespace api.Controllers
{
    public class PharmacyController : Base.UserType
    {

    

        SV_db1Entities db = new SV_db1Entities();
        DateTime dt = DateTime.UtcNow;

        [HttpPost]
        [Route("pharmacy/signup")]
        public IHttpActionResult postPharmacy([FromBody] post_pharmacy pharma)
        {
            try
            {

                Is_Required("legal_entity_name", pharma.legal_entity_name, 1);
                Is_Required("healthsplash_contact", pharma.healthsplash_contact, 1);
               
                Is_Required("address1", pharma.address1, 1);
                Is_Required("city", pharma.city, 1);
                Is_Required("state", pharma.state, 1);
                Is_Required("zip", pharma.zip, 1);

                Is_Required("federal_tax_number", pharma.federal_tax_number, 1);
                Is_Required("primary_phone", pharma.primary_phone, 1);

                Is_Required("specialty_services", pharma.specialty_services, 1);
                Is_Required("customer_service_number", pharma.customer_service_number, 1);
                Is_Required("technology_platform_retrieval", pharma.technology_platform_retrieval, 1);
                
                Is_Required("ncpdp_number", pharma.ncpdp_number, 1);
                Is_Required("practice_npi_number", pharma.practice_npi_number, 1);
                Is_Required("primary_contact_name_first", pharma.primary_contact_name_first, 1);
                Is_Required("primary_contact_name_last", pharma.primary_contact_name_last, 1);
                Is_Required("primary_contacty_phone", pharma.primary_contact_phone, 1);
                Is_Required("primary_contact_email", pharma.primary_contact_email, 1);

                //Is_Required("operational_name_first", mark.operational_name_first, 1);
                //Is_Required("operational_name_last", mark.operational_name_last, 1);
                //Is_Required("operational_phone", mark.operational_phone, 1);
                //Is_Required("operational_email", mark.operational_email, 1);

                Is_Required("financial_name_first", pharma.financial_name_first, 1);
                Is_Required("financial_name_last", pharma.financial_name_last, 1);
                Is_Required("financial_phone", pharma.financial_phone, 1);
                Is_Required("financial_email", pharma.financial_email, 1);

                Is_Required("authorize_signatory_name_first", pharma.authorize_signatory_name_first, 1);
                Is_Required("authorize_signatory_name_last", pharma.authorize_signatory_name_last, 1);
                Is_Required("authorize_signatory_phone", pharma.authorize_signatory_phone, 1);
                Is_Required("authorize_signatory_email", pharma.authorize_signatory_email, 1);

                if (HAS_ERROR)
                {
                    return Json(new { data = new string[] { }, message = ERR_MSG, success = false });
                }

                long a = validateZip(pharma);
                if (a == 1){ return Json(new { data = new string[] { }, message = "Invalid zip value.", success = false }); }
                else if (a == 2) { return Json(new { data = new string[] { }, message = "Invalid city value.", success = false }); }
                else if (a == 3) { return Json(new { data = new string[] { }, message = "Invalid state value.", success = false }); }
                    

                var u_search = db.USERs.Where(b => b.username == pharma.authorize_signatory_email.ToLower());
                if (u_search.Count() > 0)
                {
                    return Json(new { data = new string[] { }, message = "Authorized email already exist.", success = false });
                }

                return _savePharmacy(pharma);

                //mark.fax_number = "";
                // mark.process_external_transfer
                // mark.intake_entity 
                // mark.technology_platform
                //mark.pharmacy_relationship
                //mark.unique_customer_number
                //mark.pharmacy_fax
                //mark.preferred_geographic_market

            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }

        }

        [HttpGet]
        [Route("pharmacy")]
        public IHttpActionResult GetPharmacy([FromUri] get_pharmacy pharmacy)
        {
            var m = db.PHARMACies.Find(pharmacy.id);
            List<get_response_pharmacy> resp = new List<get_response_pharmacy>();
            if (m != null)
            {
                var ref_zip = db.ref_zip.Find(m.addr_zip_id);
                get_response_pharmacy ext = _getPharmacy_ext(m.id);
                List<zip_search_address2> addr = new List<zip_search_address2>();
                if (ref_zip != null)
                {
                    addr.Add(new zip_search_address2
                    {
                        address1 = m.addr_address1,
                        address2 = m.addr_address2 == null ? "" : m.addr_address2,
                        city = ref_zip == null ? "" : ref_zip.city_name,
                        state = ref_zip == null ? "" : ref_zip.city_state,
                        state_long = ref_zip == null ? "" : ref_zip.city_state_long,
                        zip = ref_zip == null ? "" : ref_zip.zip
                    });
                }


                resp.Add(new get_response_pharmacy
                {
                    legal_entity_name = m.legal_entity_name,
                    healthsplash_contact = m.healthsplash_contact,

                    address = addr == null ? new List<zip_search_address2>() { } : addr,

                    specialty_services = m.specialty_service,
                    federal_tax_number = m.federal_tax_number,
                    primary_phone = m.primary_phone,
                    customer_service_number = m.customer_service_number,
                    technology_platform_retrieval = m.technology_platform_retrieval,
                    ncpdp_number = m.NCPDP_number,
                    practice_npi_number = m.practice_npi_number,

                    fax_number = ext.fax_number == null ? "" : ext.fax_number,
                    marketer_partner = ext.marketer_partner == null ? "" : ext.marketer_partner,
                    pharmacy_fax_number = ext.pharmacy_fax_number == null ? "" : ext.pharmacy_fax_number,
                    insurance_provider = ext.insurance_provider == null ? "" : ext.insurance_provider,
                    product_list = ext.product_list == null ? "" : ext.product_list,
                    geographic_market = ext.geographic_market == null ? "": ext.geographic_market,

                    primary_contact_name_first = m.prim_contact_name_first,
                    primary_contact_name_last = m.prim_contact_name_last,
                    primary_contacty_phone = m.prim_contact_phone,
                    primary_contact_email = m.prim_contact_email,


                    operational_name_first = m.oper_contact_name_first,
                    operational_name_last = m.oper_contact_name_last,
                    operational_phone = m.oper_contact_email,
                    operational_email = m.oper_contact_email,

                    financial_name_first = m.fina_contact_name_first,
                    financial_name_last = m.fina_contact_name_last,
                    financial_phone = m.fina_contact_phone,
                    financial_email = m.fina_contact_email,

                    authorize_signatory_name_first = m.auth_contact_name_first,
                    authorize_signatory_name_last = m.auth_contact_name_last,
                    authorize_signatory_phone = m.auth_contact_phone,
                    authorize_signatory_email = m.auth_contact_email
        
                });




                var ret1 = JsonConvert.SerializeObject(resp);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                string msg = resp.Count() + " Record found.";
                return Json(new { data = json1, message = msg, success = true });

            }


            return Json(new { data = new string[] { }, message = "", success = false });

        }

        private get_response_pharmacy _getPharmacy_ext(long m_id)
        {
            var mx = db.PHARMACY_ext.Where(a => a.rel_PHARMACY_id == m_id);

            get_response_pharmacy resp = new get_response_pharmacy();
            foreach (var n in mx)
            {
                switch (n.attr_name)
                {
                    case "fax_number": resp.fax_number = n.value; break;
                    case "marketer_partner": resp.marketer_partner = n.value; break;
                    case "pharmacy_fax_number": resp.pharmacy_fax_number = n.value; break;
                    case "geographic_market": resp.geographic_market = n.value; break;
                    case "insurance_provider": resp.insurance_provider = n.value; break;
                    case "product_list": resp.product_list = n.value; break;
                   

                }

            }

            return resp;
        }

        private IHttpActionResult _savePharmacy(post_pharmacy m)
        {
            phoneValue(m);
            PHARMACY mrk = new PHARMACY
            {

                legal_entity_name = m.legal_entity_name,
                healthsplash_contact = m.healthsplash_contact,
                // addr_zip_id
                addr_address1 = m.address1,
                addr_address2 = string.IsNullOrEmpty(m.address2) == true ? null : m.address2,
                addr_zip_id = m.zip_id,

                specialty_service = m.specialty_services,

                federal_tax_number = m.federal_tax_number,
                primary_phone = m.primary_phone,
                customer_service_number = m.customer_service_number,
                technology_platform_retrieval = m.technology_platform_retrieval,
                NCPDP_number = m.ncpdp_number,
                practice_npi_number = m.practice_npi_number,

                prim_contact_name_first = m.primary_contact_name_first,
                prim_contact_name_last = m.primary_contact_name_last,
                prim_contact_email = m.primary_contact_email,
                prim_contact_phone = m.primary_phone,

                oper_contact_name_first = m.operational_name_first,
                oper_contact_name_last = m.operational_name_last,
                oper_contact_email = m.operational_email,
                oper_contact_phone = m.operational_phone,

                fina_contact_name_first = m.financial_name_first,
                fina_contact_name_last = m.financial_name_last,
                fina_contact_email = m.financial_email,
                fina_contact_phone = m.financial_phone,

                auth_contact_name_first = m.authorize_signatory_name_first,
                auth_contact_name_last = m.authorize_signatory_name_last,
                auth_contact_email = m.authorize_signatory_email,
                auth_contact_phone = m.authorize_signatory_phone,

            };
           

            db.PHARMACies.Add(mrk);
            db.SaveChanges();

            long user_id = savePhysician_User(m);
            //bool saveMarketer_ext(string _attr_name, string _dname, string _value, long market_id = 0, long user_id = 0, int data_type = 0)            

            //mark.fax_number = "";
            if (!string.IsNullOrEmpty(m.fax_number))
            {
                bool i = Entry.saveMarketer_ext("fax_number", "fax_number", m.fax_number, mrk.id, 0, 0);
            }

            //mark.marketer_partner = "";
            if (!string.IsNullOrEmpty(m.marketer_partner))
            {
                bool i = Entry.saveMarketer_ext("marketer_partner", "marketer_partner", m.marketer_partner, mrk.id, 0, 0);
            }

            //mark.pharmacy_fax_number = "";
            if (!string.IsNullOrEmpty(m.marketer_partner))
            {
                bool i = Entry.saveMarketer_ext("pharmacy_fax_number", "pharmacy_fax_number", m.pharmacy_fax_number, mrk.id, 0, 0);
            }


            //mark.geographic_market = "";
            if (!string.IsNullOrEmpty(m.geographic_market))
            {
                bool i = Entry.saveMarketer_ext("geographic_market", "geographic_market", m.geographic_market, mrk.id, 0, 0);
            }

            // mark.insurance_provider
            if (!string.IsNullOrEmpty(m.insurance_provider))
            {
                bool i = Entry.saveMarketer_ext("insurance_provider", "insurance_provider", m.insurance_provider, mrk.id, 0, 0);
            }

            // mark.product_list 
            if (!string.IsNullOrEmpty(m.product_list))
            {
                bool i = Entry.saveMarketer_ext("product_list", "product_list", m.product_list, mrk.id, 0, 0);
            }

         

            return Json(new { data = new string[] { }, message = "Successfully saved.", success = true });
        }

        private void phoneValue(post_pharmacy mark)
        {

            if (!string.IsNullOrEmpty(mark.primary_phone) & mark.primary_phone.Length > 10)
            {
                mark.primary_phone = mark.primary_phone.Substring(0, 10);
            }

            if (!string.IsNullOrEmpty(mark.financial_phone) & mark.financial_phone.Length > 10)
            {
                mark.financial_phone = mark.financial_phone.Substring(0, 10);
            }
            if (!string.IsNullOrEmpty(mark.operational_phone) & mark.operational_phone.Length > 10)
            {
                mark.operational_phone = mark.operational_phone.Substring(0, 10);
            }
            if (!string.IsNullOrEmpty(mark.primary_contact_phone) & mark.primary_contact_phone.Length > 10)
            {
                mark.primary_contact_phone = mark.primary_contact_phone.Substring(0, 10);
            }
            if (!string.IsNullOrEmpty(mark.authorize_signatory_phone) & mark.authorize_signatory_phone.Length > 10)
            {
                mark.authorize_signatory_phone = mark.authorize_signatory_phone.Substring(0, 10);
                //return Json(new { data = new string[] { }, message = "Phone numbe", success = false });
            }

            //return Json(new { data = new string[] { }, message = "Phone numbe", success = false });
        }
        private long savePhysician_User(post_pharmacy m)
        {

            var u_find = db.USERs.Where(a => a.username == m.authorize_signatory_email.ToLower());
            USER u = new USER
            {
                name_first = m.authorize_signatory_name_first,
                name_last = m.authorize_signatory_name_last,
                username = m.authorize_signatory_email,
                dt_create = dt,
                create_by__USER_id = 0,
                rel_ref_USER_type_id = 13
            };

            db.USERs.Add(u);
            db.SaveChanges();

            return u.id;
        }

        //private long xvalidateZip(string city, string state, string zip)
        //{
        //    SV_db1Entities db = new SV_db1Entities();
        //    int nzip = 0;
        //    bool isvalid = int.TryParse(zip, out nzip);
        //    if (!isvalid)
        //    {
        //        return -1;
        //    }

        //    var ref_zip = db.ref_zip.Where(a => a.zip == zip);
        //    if (ref_zip.Count() > 0)
        //    {
        //        if (ref_zip.FirstOrDefault().city_name.ToLower() != city.ToLower())
        //        {
        //            //return Json(new { data = new string[] { }, message = "Invalid city name.", success = false });
        //            return 0;
        //        }

        //        if (ref_zip.FirstOrDefault().city_state.ToLower() != state.ToLower()
        //            & ref_zip.FirstOrDefault().city_state_long.ToLower() != state.ToLower())
        //        {
        //            //return Json(new { data = new string[] { }, message = "Invalid state name.", success = false });
        //            return 0;
        //        }
        //    }

        //    //return Json(new { data = new string[] { }, message = "Match found.", success = true });
        //    return ref_zip.FirstOrDefault().id;
        //}


    
   }


    public class get_pharmacy
    {
        public long id { get; set; }
    }
    public class post_pharmacy
    {
        public string legal_entity_name { get; set; }   //
        public string healthsplash_contact { get; set; }    //
        public string address1 { get; set; }    //
        public string address2 { get; set; }    //
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public long zip_id { get; set; }
        
        public string specialty_services { get; set; }  //Specialty Services/Company Type
        public string federal_tax_number { get; set; }  //
        public string primary_phone { get; set; }   //
        public string customer_service_number { get; set; }     //
        public string technology_platform_retrieval { get; set; }  // Technology Platform for Retrieval of Records (i.e. API, Fax, SharePoint)*
        public string ncpdp_number { get; set; }    //
        public string practice_npi_number { get; set; }    //

        public string fax_number { get; set; }
        public string marketer_partner { get; set; } //Marketer (If applicable, include all)
        public string pharmacy_fax_number { get; set; }
        public string geographic_market { get; set; }
        public string insurance_provider { get; set; } //List of your in-network/accepted insurance providers
        public string product_list { get; set; }


        public string primary_contact_name_first { get; set; }
        public string primary_contact_name_last { get; set; }
        public string primary_contact_phone { get; set; }
        public string primary_contact_email { get; set; }

        public string operational_name_first { get; set; }
        public string operational_name_last { get; set; }
        public string operational_phone { get; set; }
        public string operational_email { get; set; }

        public string financial_name_first { get; set; }
        public string financial_name_last { get; set; }
        public string financial_phone { get; set; }
        public string financial_email { get; set; }

        public string authorize_signatory_name_first { get; set; }
        public string authorize_signatory_name_last { get; set; }
        public string authorize_signatory_phone { get; set; }
        public string authorize_signatory_email { get; set; }

    }

    public class get_response_pharmacy
    {
        public string legal_entity_name { get; set; }
        public string healthsplash_contact { get; set; }
        public List<zip_search_address2> address { get; set; }

        public string specialty_services { get; set; }  //
        public string federal_tax_number { get; set; }  //
        public string primary_phone { get; set; }   //
        public string customer_service_number { get; set; }     //
        public string technology_platform_retrieval { get; set; }  //
        public string ncpdp_number { get; set; }    //
        public string practice_npi_number { get; set; }    //

        public string fax_number { get; set; }
        public string marketer_partner { get; set; }
        public string pharmacy_fax_number { get; set; }
        public string geographic_market { get; set; }
        public string insurance_provider { get; set; }
        public string product_list { get; set; }
        

        public string primary_contact_name_first { get; set; }
        public string primary_contact_name_last { get; set; }
        public string primary_contacty_phone { get; set; }
        public string primary_contact_email { get; set; }


        public string operational_name_first { get; set; }
        public string operational_name_last { get; set; }
        public string operational_phone { get; set; }
        public string operational_email { get; set; }

        public string financial_name_first { get; set; }
        public string financial_name_last { get; set; }
        public string financial_phone { get; set; }
        public string financial_email { get; set; }

        public string authorize_signatory_name_first { get; set; }
        public string authorize_signatory_name_last { get; set; }
        public string authorize_signatory_phone { get; set; }
        public string authorize_signatory_email { get; set; }

    }
}
