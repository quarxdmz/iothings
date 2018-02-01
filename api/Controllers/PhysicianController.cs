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
    public class PhysicianController : Base.UserType
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
        [Route("physician/signup")]
        public IHttpActionResult postPhysician([FromBody] post_physician p)
        {
            try
            {
                   
                    Is_Required("legal_entity_name", p.legal_entity_name, 1);
                    Is_Required("healthsplash_contact", p.healthsplash_contact, 1);
                Is_Required("practice_type", p.practice_type, 1);
                Is_Required("address1", p.address1, 1);
                    Is_Required("city", p.city, 1);
                    Is_Required("state", p.state, 1);
                    Is_Required("zip", p.zip, 1);

                    Is_Required("federal_tax_number", p.federal_tax_number, 1);
                    Is_Required("primary_phone", p.primary_phone, 1);

                 Is_Required("customer_service_number", p.customer_service_number, 1);
                Is_Required("number_clinicians", p.number_clinicians.ToString(), 1);
                Is_Required("number_exams_per_week", p.number_exams_per_week.ToString(), 1);
                Is_Required("practice_npi", p.practice_npi, 1);
                Is_Required("geographic_market", p.geographic_market, 1);
                Is_Required("insurance_providers", p.insurance_providers, 1);
                Is_Required("EMR_software_used", p.EMR_software_used, 1);

                Is_Required("primary_contact_name_first", p.primary_contact_name_first, 1);
                    Is_Required("primary_contact_name_last", p.primary_contact_name_last, 1);
                    Is_Required("primary_contacty_phone", p.primary_contact_phone, 1);
                    Is_Required("primary_contact_email", p.primary_contact_email, 1);

                    //Is_Required("operational_name_first", p.operational_name_first, 1);
                    //Is_Required("operational_name_last", p.operational_name_last, 1);
                    //Is_Required("operational_phone", p.operational_phone, 1);
                    //Is_Required("operational_email", p.operational_email, 1);

                    Is_Required("financial_name_first", p.financial_name_first, 1);
                    Is_Required("financial_name_last", p.financial_name_last, 1);
                    Is_Required("financial_phone", p.financial_phone, 1);
                    Is_Required("financial_email", p.financial_email, 1);

                    Is_Required("authorize_signatory_name_first", p.authorize_signatory_name_first, 1);
                    Is_Required("authorize_signatory_name_last", p.authorize_signatory_name_last, 1);
                    Is_Required("authorize_signatory_phone", p.authorize_signatory_phone, 1);
                    Is_Required("authorize_signatory_email", p.authorize_signatory_email, 1);

                    if (HAS_ERROR)
                    {
                        return Json(new { data = new string[] { }, message = ERR_MSG, success = false });
                    }

                
                long a = validateZip(p);
                if (a == 1) { return Json(new { data = new string[] { }, message = "Invalid zip value.", success = false }); }
                else if (a == 2) { return Json(new { data = new string[] { }, message = "Invalid city value.", success = false }); }
                else if (a == 3) { return Json(new { data = new string[] { }, message = "Invalid state value.", success = false }); }

                var u_search = db.USERs.Where(b => b.username == p.authorize_signatory_email.ToLower());
                if (u_search.Count() > 0)
                {
                    return Json(new { data = new string[] { }, message = "Authorized email already exist.", success = false });
                }

                 return _savePhysician(p);
                    
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
        [Route("physician")]
        public IHttpActionResult GetPhysician([FromUri] get_physician physician)
        {
            try {
                var m = db.PHYSICIANs.Find(physician.id);
                List<get_response_physician> resp = new List<get_response_physician>();
                if (m != null)
                {
                    var ref_zip = db.ref_zip.Find(m.addr_zip_id);
                    get_response_physician ext = _getPhysician_ext(m.id);
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


                    resp.Add(new get_response_physician
                    {
                        legal_entity_name = m.legal_entity_name,
                        healthsplash_contact = m.healthsplash_contact,

                        address = addr == null ? new List<zip_search_address2>() { } : addr,

                        federal_tax_number = m.federal_tax_number,
                        primary_phone = m.primary_phone,
                        geographic_market = m.geographic_market,
                        insurance_providers = m.insurance_providers,
                        practice_npi = m.practice_npi,
                        practice_type = m.practice_type,
                        customer_service_number = m.customer_service_number,
                        number_clinicians = m.number_clinicians.Value,
                        number_exams_per_week = m.number_exams_per_week.Value,
                        EMR_software_used = m.EMR_software_used,

                        fax_number = ext.fax_number == null ? "" : ext.fax_number,
                        current_scheduling_solution = ext.current_scheduling_solution == null ? "" : ext.current_scheduling_solution,
                        future_expansion = ext.future_expansion == null ? "" : ext.future_expansion,


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
                        authorize_signatory_email = m.auth_contact_email
                    });




                    var ret1 = JsonConvert.SerializeObject(resp);
                    var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                    string msg = resp.Count() + " Record found.";
                    return Json(new { data = json1, message = msg, success = true });

                }
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }

            return Json(new { data = new string[] { }, message = "", success = false });

        }

        private get_response_physician _getPhysician_ext(long m_id)
            {
                var mx = db.PHYSICIAN_ext.Where(a => a.rel_PHYSICIAN_id == m_id);

            get_response_physician resp = new get_response_physician();
                foreach (var n in mx)
                {
                    switch (n.attr_name)
                    {
                        case "fax_number": resp.fax_number = n.value; break;
                        case "future_expansion": resp.future_expansion = n.value; break;
                         case "current_scheduling_solution": resp.current_scheduling_solution = n.value; break;


                    }

                }

                return resp;
            }

        private IHttpActionResult _savePhysician(post_physician m)
        {
            phoneValue(m);
            if (m.practice_npi.Length > 10) m.practice_npi = m.practice_npi.Substring(0,10);

            PHYSICIAN mrk = new PHYSICIAN
            {

                    legal_entity_name = m.legal_entity_name,
                    healthsplash_contact = m.healthsplash_contact,
                    // addr_zip_id
                    addr_address1 = m.address1,
                    addr_address2 = string.IsNullOrEmpty(m.address2) == true ? null : m.address2,
                    addr_zip_id = m.zip_id,
                    federal_tax_number = m.federal_tax_number,
                    primary_phone = m.primary_phone,
                    customer_service_number = m.customer_service_number,
                    number_clinicians = m.number_clinicians,
                    number_exams_per_week = m.number_exams_per_week,
                    practice_npi = m.practice_npi,
                    geographic_market = m.geographic_market,
                    insurance_providers = m.insurance_providers,
                    EMR_software_used = m.EMR_software_used,
                    practice_type = m.practice_type,
                    
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
                    auth_contact_phone = m.authorize_signatory_phone
                    
                };

                db.PHYSICIANs.Add(mrk);
                db.SaveChanges();

                long user_id = savePhysician_User(m);
                //bool saveMarketer_ext(string _attr_name, string _dname, string _value, long market_id = 0, long user_id = 0, int data_type = 0)            

          
                //mark.fax_number = "";
                if (!string.IsNullOrEmpty(m.fax_number))
                {
                    bool i = Entry.savePhysician_ext("fax_number", "fax_number", m.fax_number, mrk.id, 0, 0);
                }

            //mark.future_expansion = "";
            if (!string.IsNullOrEmpty(m.future_expansion))
            {
                bool i = Entry.savePhysician_ext("future_expansion", "future_expansion", m.future_expansion, mrk.id, 0, 0);
            }

            //mark.current_scheduling_solution = "";
            if (!string.IsNullOrEmpty(m.current_scheduling_solution))
            {
                bool i = Entry.savePhysician_ext("current_scheduling_solution", "current_scheduling_solution", m.current_scheduling_solution, mrk.id, 0, 0);
            }

         
            return Json(new { data = new string[] { }, message = "Successfully saved.", success = true });
            }

        private void phoneValue(post_physician mark)
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
        private long savePhysician_User(post_physician m)
            {

                var u_find = db.USERs.Where(a => a.username == m.authorize_signatory_email.ToLower());
                USER u = new USER
                {
                    name_first = m.authorize_signatory_name_first,
                    name_last = m.authorize_signatory_name_last,
                    username = m.authorize_signatory_email,
                    dt_create = dt,
                    create_by__USER_id = 0,
                    rel_ref_USER_type_id = 12
                };

                db.USERs.Add(u);
                db.SaveChanges();

                return u.id;
            }

        //private long xvalidateZip(string city, string state, string zip)
        //{
        //        SV_db1Entities db = new SV_db1Entities();
        //    int nzip = 0;
        //    bool isvalid = int.TryParse(zip, out nzip);
        //    if (!isvalid)
        //    {
        //        return -1;
        //    }
        //        var ref_zip = db.ref_zip.Where(a => a.zip == zip);
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

        public class get_physician
        {
            public long id { get; set; }
        }
        public class post_physician
        {
            public string legal_entity_name { get; set; }
            public string healthsplash_contact { get; set; }
            public string address1 { get; set; }
            public string address2 { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zip { get; set; }
            public long zip_id { get; set; }

        public string practice_type { get; set; } // Practice Type(Family practice, pediatrician, urgent care, house call group, telemedicine, etc.)*
        public string federal_tax_number { get; set; }
            public string primary_phone { get; set; } // primary_phone
        public string customer_service_number { get; set; }
        public int number_clinicians { get; set; }
        public int number_exams_per_week { get; set; }
        public string practice_npi { get; set; }
        public string geographic_market { get; set; }
        public string insurance_providers { get; set; } //List of your in-network/accepted insurance providers*
        public string EMR_software_used { get; set; }//EMR software you are currently using*



        public string fax_number { get; set; }
        public string future_expansion { get; set; }
        public string current_scheduling_solution { get; set; } //Current Scheduling Solution (House calls and Home Health only)

        
        
     

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

        public class get_response_physician
        {
            public string legal_entity_name { get; set; }
            public string healthsplash_contact { get; set; }
            public List<zip_search_address2> address { get; set; }

        public string practice_type { get; set; } // Practice Type(Family practice, pediatrician, urgent care, house call group, telemedicine, etc.)*
        public string federal_tax_number { get; set; }
        public string primary_phone { get; set; } // primary_phone
        public string customer_service_number { get; set; }
        public int number_clinicians { get; set; }
        public int number_exams_per_week { get; set; }
        public string practice_npi { get; set; }
        public string geographic_market { get; set; }
        public string insurance_providers { get; set; } //List of your in-network/accepted insurance providers*
        public string EMR_software_used { get; set; }//EMR software you are currently using*
    


        public string fax_number { get; set; }
        public string future_expansion { get; set; }//Future Expansion Plans/New Markets
        public string current_scheduling_solution { get; set; } //Current Scheduling Solution (House calls and Home Health only)


        

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
