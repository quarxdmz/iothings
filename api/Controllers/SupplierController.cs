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
        public class SupplierController : Base.UserType
        {
            #region
            // marketing_ext
            // fax_number
            // process for external transfer
            // intake entity
            // technology platform for intake
            // pharmacy relationship
            // unique customer number with pharmacy
            // pharmacy fax number
            // preferred geographic markets
            #endregion

        SV_db1Entities db = new SV_db1Entities();
        DateTime dt = DateTime.UtcNow;

        [HttpPost]
        [Route("supplier/signup")]
        public IHttpActionResult postSupplier([FromBody] post_supplier supp)
        {
                try
                {
                    #region
                    //legal_entity_name: sdfdfddddd
                    // healthsplash_contact:dsdfsdf
                    // address1:dsfsfdsdfsdfsf
                    // address2:sdfsfdd
                    // city:Cayey
                    // state:Puerto Rico
                    //zip: 00736
                    //federal_tax_number: adfasd
                    // primary_phone:asdfs
                    // partner_supplier:asdf
                    // partner_telemedicine:asdfsad
                    // primary_contact_name_first:afsdf
                    // primary_contact_name_last:asdfd
                    // primary_contacty_phone:asdfsd
                    // primary_contact_email:asfsd
                    // operational_name_first:asfdf
                    // operational_name_first:asdfsd
                    // operational_name_last:sfsdd
                    // operational_phone:sdd
                    // operational_email:ddss
                    // financial_name_first:lllkkd
                    // financial_name_last:dsddfd
                    // financial_phone:seeed
                    // financial_email:dsdgge
                    // authorize_signatory_name_first:sddggd
                    // authorize_signatory_name_last:dsdfdsfd
                    // authorize_signatory_phone:sdfdfd
                    // authorize_signatory_email:sdfdsfd
                    // fax_number:sdfs
                    // process_external_transfer: asdfsdf
                    // intake_entity:safsdf
                    // technology_platform:sdfsdfdfd
                    // pharmacy_relationship:sdddf
                    // unique_customer_number:sdfsdfds
                    // pharmacy_fax:ddsdbggg
                    // preferred_geographic_market:sdfsfdsfd
                    #endregion
                    Is_Required("legal_entity_name", supp.legal_entity_name, 1);
                    Is_Required("healthsplash_contact", supp.healthsplash_contact, 1);
                    Is_Required("address1", supp.address1, 1);
                    Is_Required("city", supp.city, 1);
                    Is_Required("state", supp.state, 1);
                    Is_Required("zip", supp.zip, 1);

                    Is_Required("federal_tax_number", supp.federal_tax_number, 1);
                    Is_Required("primary_phone", supp.primary_phone, 1);

                //Is_Required("supplier_partner", mark.partner_supplier, 1);
                Is_Required("telemedicine_partner", supp.telemedicine_partner, 1);
                Is_Required("organization_npi", supp.organization_npi, 1);
                Is_Required("preferred_geographic_market", supp.preferred_geographic_market, 1);
               Is_Required("insurance_providers", supp.insurance_providers, 1);
                Is_Required("products_hcpcs", supp.products_hcpcs, 1);
                Is_Required("patient_deductible_threshold", supp.patient_deductible_threshold, 1);

                Is_Required("primary_contact_name_first", supp.primary_contact_name_first, 1);
                    Is_Required("primary_contact_name_last", supp.primary_contact_name_last, 1);
                    Is_Required("primary_contacty_phone", supp.primary_contact_phone, 1);
                    Is_Required("primary_contact_email", supp.primary_contact_email, 1);

                    //Is_Required("operational_name_first", mark.operational_name_first, 1);
                    //Is_Required("operational_name_last", mark.operational_name_last, 1);
                    //Is_Required("operational_phone", mark.operational_phone, 1);
                    //Is_Required("operational_email", mark.operational_email, 1);

                    Is_Required("financial_name_first", supp.financial_name_first, 1);
                    Is_Required("financial_name_last", supp.financial_name_last, 1);
                    Is_Required("financial_phone", supp.financial_phone, 1);
                    Is_Required("financial_email", supp.financial_email, 1);

                    Is_Required("authorize_signatory_name_first", supp.authorize_signatory_name_first, 1);
                    Is_Required("authorize_signatory_name_last", supp.authorize_signatory_name_last, 1);
                    Is_Required("authorize_signatory_phone", supp.authorize_signatory_phone, 1);
                    Is_Required("authorize_signatory_email", supp.authorize_signatory_email, 1);

                    if (HAS_ERROR)
                    {
                        return Json(new {data= new string[] { }, message = ERR_MSG, success = false });
                    }

                    long a = validateZip(supp);
                if (a == 1) { return Json(new { data = new string[] { }, message = "Invalid zip value.", success = false }); }
                else if (a == 2) { return Json(new { data = new string[] { }, message = "Invalid city value.", success = false }); }
                else if (a == 3) { return Json(new { data = new string[] { }, message = "Invalid state value.", success = false }); }

                var u_search = db.USERs.Where(b => b.username == supp.authorize_signatory_email.ToLower());
                    if (u_search.Count() > 0)
                    {
                        return Json(new { data = new string[] { }, message = "Authorized email already exist.", success = false });
                    }

                    return _saveSupplier(supp);
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
        [Route("supplier")]
       public IHttpActionResult GetSupplier([FromUri] get_supplier supplier)
            {
                var m = db.SUPPLIERs.Find(supplier.id);
                List<get_response_supplier> resp = new List<get_response_supplier>();
                if (m != null)
                {
                    var ref_zip = db.ref_zip.Find(m.addr_zip_id);
                    get_response_supplier ext = _getSupplier_ext(m.id);
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


                    resp.Add(new get_response_supplier
                    {
                        legal_entity_name = m.legal_entity_name,
                        healthsplash_contact = m.healthsplash_contact,
                        address = addr == null ? new List<zip_search_address2>() { } : addr,

                        federal_tax_number = m.federal_tax_number,
                        primary_phone = m.primary_phone,
                        organization_npi = m.organization_npi,
                        patient_deductible_threshold = m.patient_deductible_threshold,
                        products_hcpcs = m.products_hcpcs,
                        
                        telemedicine_partner = m.telemedicine_partner,
                        insurance_providers = m.insurance_provider,
                        fax_number = ext.fax_number == null ? "" : ext.fax_number,
                        process_external_transfer = ext.process_external_transfer == null ? "" : ext.process_external_transfer,
                        intake_entity = ext.intake_entity == null ? "" : ext.intake_entity,
                        pharmacy_relationship = ext.pharmacy_relationship == null ? "" : ext.pharmacy_relationship,
                        technology_platform = ext.technology_platform == null ? "" : ext.technology_platform,
                        pharmacy_customer_number = ext.pharmacy_customer_number == null ? "" : ext.pharmacy_customer_number,
                        pharmacy_fax = ext.pharmacy_fax == null ? "" : ext.pharmacy_fax,
                        preferred_geographic_market = ext.preferred_geographic_market == null ? "" : ext.preferred_geographic_market,
                        marketer_partner = ext.marketer_partner == null?"": ext.marketer_partner,
                        ptan_number = ext.ptan_number == null ? "" : ext.ptan_number,
                        
                                                
                        primary_contact_name_first = m.prim_contact_name_first,
                        primary_contact_name_last = m.prim_contact_name_last,
                        primary_contacty_phone = m.prim_contact_phone,
                        primary_contact_email = m.prim_contact_email,


                        operational_name_first = m.oper_contact_name_first,
                        operational_name_last = m.oper_contact_name_last,
                        operational_phone = m.oper_contact_phone,
                        operational_email = m.oper_contact_email,

                        financial_name_first = m.fina_contact_name_first,
                        financial_name_last = m.fina_contact_name_last,
                        financial_phone = m.fina_contact_phone,
                        financial_email = m.fina_contact_email,

                        authorize_signatory_name_first = m.auth_contact_name_first,
                        authorize_signatory_name_last = m.auth_contact_name_last,
                        authorize_signatory_phone = m.auth_contact_phone,
                        authorize_signatory_email = m.auth_contact_email,

                      
                    });




                    var ret1 = JsonConvert.SerializeObject(resp);
                    var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                    string msg = resp.Count() + " Record found.";
                return Json(new { data = json1, message = msg, success = true });

                }


                return Json(new { data = new string[] { }, message = "", success = false });

            }

      private get_response_supplier _getSupplier_ext(long m_id)
            {
                var mx = db.MARKETER_ext.Where(a => a.rel_MARKETER_id == m_id);

                get_response_supplier resp = new get_response_supplier();
                foreach (var n in mx)
                {
                    switch (n.attr_name)
                    {
                        case "fax_number": resp.fax_number = n.value; break;
                        case "process_external_transfer": resp.process_external_transfer = n.value; break;
                    case "marketer_partner": resp.marketer_partner = n.value; break;

                    case "intake_entity": resp.intake_entity = n.value; break;
                    case "technology_platform": resp.technology_platform = n.value; break;
                    case "pharmacy_customer_number": resp.pharmacy_customer_number = n.value; break;
                    case "pharmacy_relationship": resp.pharmacy_relationship = n.value; break;

                    case "pharmacy_fax": resp.pharmacy_fax = n.value; break;
                       case "ptan_nuber": resp.ptan_number = n.value; break;

                }

                }

                return resp;
            }

      private IHttpActionResult _saveSupplier(post_supplier m)
      {
            phoneValue(m);
            if (m.organization_npi.Length > 10) m.organization_npi = m.organization_npi.Substring(0, 10);

            SUPPLIER mrk = new SUPPLIER
                {
                    legal_entity_name = m.legal_entity_name,
                    healthsplash_contact = m.healthsplash_contact,
                    // addr_zip_id
                    addr_address1 = m.address1,
                    addr_address2 = string.IsNullOrEmpty(m.address2) ==true ? null:  m.address2,
                    addr_zip_id = m.zip_id,
                    federal_tax_number = m.federal_tax_number,
                    primary_phone = m.primary_phone,
                    telemedicine_partner = m.telemedicine_partner,
                    organization_npi = m.organization_npi,
                    preferred_geographic_market = m.preferred_geographic_market,
                    insurance_provider = m.insurance_providers,
                    products_hcpcs = m.products_hcpcs,
                    patient_deductible_threshold = m.patient_deductible_threshold,
                    

                    prim_contact_name_first = m.primary_contact_name_first,
                    prim_contact_name_last = m.primary_contact_name_last,
                    prim_contact_email = m.primary_contact_email,
                prim_contact_phone = m.primary_contact_phone,
                
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

                db.SUPPLIERs.Add(mrk);
                db.SaveChanges();

                long user_id = saveSupplier_User(m);
                //bool saveMarketer_ext(string _attr_name, string _dname, string _value, long market_id = 0, long user_id = 0, int data_type = 0)            

             
            //mark.fax_number = "";
            if (!string.IsNullOrEmpty(m.fax_number))
            {
                bool i = Entry.saveSupplier_ext("fax_number", "fax_number", m.fax_number, mrk.id, 0, 0);
            }

            // mark.process_external_transfer
            if (!string.IsNullOrEmpty(m.process_external_transfer))
            {
                bool i = Entry.saveSupplier_ext("process_external_transfer", "Process External Transfer", m.process_external_transfer, mrk.id, 0, 0);
            }


            // mark.marketer_partner
            if (!string.IsNullOrEmpty(m.marketer_partner))
            {
                bool i = Entry.saveSupplier_ext("marketer_partner", "marketer_partner", m.marketer_partner, mrk.id, 0, 0);
            }

         
                // mark.intake_entity 
                if (!string.IsNullOrEmpty(m.intake_entity))
                {
                    bool i = Entry.saveSupplier_ext("intake_entity", "intake_entity", m.intake_entity, mrk.id, 0, 0);
                }

           

            // mark.technology_platform
            if (!string.IsNullOrEmpty(m.technology_platform))
                {
                    bool i = Entry.saveSupplier_ext("technology_platform", "technology_platform", m.technology_platform, mrk.id, 0, 0);
                }

            //mark.pharmacy_relationship
            if (!string.IsNullOrEmpty(m.pharmacy_relationship))
            {
                bool i = Entry.saveSupplier_ext("pharmacy_relationship", "pharmacy_relationship", m.pharmacy_relationship, mrk.id, 0, 0);
            }

            //mark.pharmacy_customer_number
            if (!string.IsNullOrEmpty(m.pharmacy_customer_number))
                {
                    bool i = Entry.saveSupplier_ext("pharmacy_customer_number", "pharmacy_customer_number", m.pharmacy_customer_number, mrk.id, 0, 0);
                }


           

            //mark.pharmacy_fax
            if (!string.IsNullOrEmpty(m.pharmacy_fax))
            {
                bool i = Entry.saveSupplier_ext("pharmacy_fax", "pharmacy_fax", m.pharmacy_fax, mrk.id, 0, 0);
            }

            //mark.ptan_number
            if (!string.IsNullOrEmpty(m.ptan_number))
                {
                    bool i = Entry.saveSupplier_ext("ptan_number", "ptan_number", m.ptan_number, mrk.id, 0, 0);
                }

               

                return Json(new { data = new string[] { }, message = "Successfully saved.", success = true });
            }

        private void phoneValue(post_supplier mark)
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
        private long saveSupplier_User(post_supplier m)
            {

                var u_find = db.USERs.Where(a => a.username == m.authorize_signatory_email.ToLower());
                USER u = new USER
                {
                    name_first = m.authorize_signatory_name_first,
                    name_last = m.authorize_signatory_name_last,
                    username = m.authorize_signatory_email,
                    dt_create = dt,
                    create_by__USER_id = 0,
                    rel_ref_USER_type_id = 11
                };

                db.USERs.Add(u);
                db.SaveChanges();

                return u.id;
            }

        //private long xvalidateZip(string city, string state, string zip)
        //    {
        //        SV_db1Entities db = new SV_db1Entities();

        //    int nzip = 0;
        //    bool isvalid = int.TryParse(zip, out nzip);
        //    if (!isvalid)
        //    {
        //        return -1;
        //    }

        //    var ref_zip = db.ref_zip.Where(a => a.zip == zip);
        //        if (ref_zip.Count() > 0)
        //        {
        //            if (ref_zip.FirstOrDefault().city_name.ToLower() != city.ToLower())
        //            {
        //                //return Json(new { data = new string[] { }, message = "Invalid city name.", success = false });
        //                return 0;
        //            }

        //            if (ref_zip.FirstOrDefault().city_state.ToLower() != state.ToLower()
        //                & ref_zip.FirstOrDefault().city_state_long.ToLower() != state.ToLower())
        //            {
        //                //return Json(new { data = new string[] { }, message = "Invalid state name.", success = false });
        //                return 0;
        //            }
        //        }

        //        //return Json(new { data = new string[] { }, message = "Match found.", success = true });
        //        return ref_zip.FirstOrDefault().id;
        //    }

          
        }

        public class get_supplier
        {
            public long id { get; set; }
        }
        public class post_supplier
        {
            public string legal_entity_name { get; set; }
            public string healthsplash_contact { get; set; }
            public string address1 { get; set; }
            public string address2 { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zip { get; set; }
            public long zip_id { get; set; }

            public string federal_tax_number { get; set; }
            public string primary_phone { get; set; } // primary_phone
        public string telemedicine_partner { get; set; }
        public string organization_npi { get; set; }
        public string preferred_geographic_market { get; set; }
        public string insurance_providers { get; set; } //List of your in-network/accepted insurance providers* 
        public string products_hcpcs { get; set; }
        public string patient_deductible_threshold { get; set; }

        public string fax_number { get; set; }
        public string process_external_transfer { get; set; }
         public string marketer_partner { get; set; }
        public string intake_entity { get; set; }
        public string technology_platform { get; set; } //Technology Platform for intake
        public string pharmacy_relationship { get; set; } //Pharmacy Relationship (If applicable; include multiple if needed) 
        public string pharmacy_customer_number { get; set; } // Unique Customer number with Pharmacy
        public string pharmacy_fax { get; set; }
        
        public string ptan_number { get; set; }

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

        public class get_response_supplier
        {
            public string legal_entity_name { get; set; }
            public string healthsplash_contact { get; set; }
            public List<zip_search_address2> address { get; set; }

        public string federal_tax_number { get; set; }
            public string primary_phone { get; set; } // primary_phone

        public string fax_number { get; set; }
          
            public string telemedicine_partner { get; set; }
        public string organization_npi { get; set; }
        public string preferred_geographic_market { get; set; }
        public string insurance_providers { get; set; }
        public string products_hcpcs { get; set; }
        public string patient_deductible_threshold { get; set; }

        public string process_external_transfer { get; set; }
        public string marketer_partner { get; set; }
        public string intake_entity { get; set; }
            public string technology_platform { get; set; }
            public string pharmacy_relationship { get; set; }
            public string pharmacy_customer_number { get; set; }
            public string pharmacy_fax { get; set; }
           public string ptan_number { get; set; }

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

