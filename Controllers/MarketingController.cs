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
    public class MarketingController : Base.UserType
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
        [Route("marketer/signup")]
        public IHttpActionResult postMarketing([FromBody] post_market mark)
        {
            try {
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
            Is_Required("legal_entity_name", mark.legal_entity_name, 1);
            Is_Required("healthsplash_contact", mark.healthsplash_contact, 1);
            Is_Required("address1", mark.address1, 1);
            //IsRequired("address_2", mark.address2, 1);
            Is_Required("city", mark.city, 1);
            Is_Required("state", mark.state, 1);
            Is_Required("zip", mark.zip, 1);

            Is_Required("federal_tax_number", mark.federal_tax_number, 1);
            Is_Required("primary_phone", mark.primary_phone, 1);

            Is_Required("supplier_partner", mark.supplier_partner, 1);
            Is_Required("telemedicine_partner", mark.telemedicine_partner, 1);

                Is_Required("primary_contact_name_first", mark.primary_contact_name_first, 1);
                Is_Required("primary_contact_name_last", mark.primary_contact_name_last, 1);
                Is_Required("primary_contact_phone", mark.primary_contact_phone, 1);
                Is_Required("primary_contact_email", mark.primary_contact_email, 1);

                //Is_Required("operational_name_first", mark.operational_name_first, 1);
                //Is_Required("operational_name_last", mark.operational_name_last, 1);
                //Is_Required("operational_phone", mark.operational_phone, 1);
                //Is_Required("operational_email", mark.operational_email, 1);

                Is_Required("financial_name_first", mark.financial_name_first, 1);
                Is_Required("financial_name_last", mark.financial_name_last, 1);
                Is_Required("financial_phone", mark.financial_phone, 1);
                Is_Required("financial_email", mark.financial_email, 1);

                Is_Required("authorize_signatory_name_first", mark.authorize_signatory_name_first, 1);
                Is_Required("authorize_signatory_name_last", mark.authorize_signatory_name_last, 1);
                Is_Required("authorize_signatory_phone", mark.authorize_signatory_phone, 1);
                Is_Required("authorize_signatory_email", mark.authorize_signatory_email, 1);

                if (HAS_ERROR)
                {
                    return Json(new { data = new string[] { }, message = ERR_MSG, success = false });
                }

                long a = validateZip(mark);
                if (a == 1) { return Json(new { data = new string[] { }, message = "Invalid zip value.", success = false }); }
                else if (a == 2) { return Json(new { data = new string[] { }, message = "Invalid city value.", success = false }); }
                else if (a == 3) { return Json(new { data = new string[] { }, message = "Invalid state value.", success = false }); }


                var u_search = db.USERs.Where(b => b.username == mark.authorize_signatory_email.ToLower());
            if (u_search.Count()> 0)
            {
                return Json(new { data = new string[] { }, message = "Authorized email already exist.", success = false });
            }

            
          
            return _saveMarketer(mark);
            

            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
            
        }

        [HttpGet]
        [Route("marketer")]
        public IHttpActionResult GetMarketer([FromUri] get_market marketer)
        {
            var m = db.MARKETERs.Find(marketer.id);
            List<get_response_market> resp = new List<get_response_market>();
            if (m != null)
            {
                var ref_zip = db.ref_zip.Find(m.addr_zip_id);
                get_response_market ext = _getMarketer_ext(m.id);
                List<zip_search_address2> addr = new List<zip_search_address2>();
                if (ref_zip != null) {
                    addr.Add(new zip_search_address2
                    {
                        address1 = m.addr_address1,
                        address2 = m.addr_address2,
                        city = ref_zip == null ? "" : ref_zip.city_name,
                        state = ref_zip == null ? "" : ref_zip.city_state,
                        state_long = ref_zip == null ? "" : ref_zip.city_state_long,
                        zip = ref_zip == null ? "" : ref_zip.zip
                    });
                }


                resp.Add(new get_response_market {
                    legal_entity_name = m.legal_entity_name,
                    healthsplash_contact = m.healthsplash_contact,
            
                    address = addr == null?  new List<zip_search_address2>() { } : addr,

                    // public long zip_id { get; set; }
                    federal_tax_number = m.federal_tax_number,
                    primary_phone = m.primary_phone,
                    telemedicine_partner = m.telemedicine_partner,
                    supplier_partner = m.supplier_partner,

                    primary_contact_name_first = m.prim_contact_name_first,
                    primary_contact_name_last = m.prim_contact_name_last,
                    primary_contacty_phone = m.prim_contact_phone,
                    primary_contact_email = m.prim_contact_email,


                    operational_contact_name_first = m.oper_contact_name_first,
                    operational_contact_name_last = m.oper_contact_name_last,
                    operational_contact_phone = m.oper_contact_phone,
                    operational_contact_email = m.oper_contact_email,

                    financial_contact_name_first = m.fina_contact_name_first,
                    financial_contact_name_last = m.fina_contact_name_last,
                    financial_contact_phone = m.fina_contact_phone,
                    financial_contact_email = m.fina_contact_email,

                    authorize_contact_signatory_name_first = m.auth_contact_name_first,
                    authorize_contact_signatory_name_last = m.auth_contact_name_last,
                    authorize_contact_signatory_phone = m.auth_contact_phone,
                    authorize_contact_signatory_email = m.auth_contact_email,

                    fax_number = ext.fax_number == null ? "" : ext.fax_number,
                    process_external_transfer = ext.process_external_transfer == null ? "" : ext.process_external_transfer,
                    intake_entity = ext.intake_entity == null ? "" : ext.intake_entity,
                    pharmacy_relationship = ext.pharmacy_relationship == null ? "" : ext.pharmacy_relationship,
                    technology_platform = ext.technology_platform == null ? "" : ext.technology_platform,
                    pharmacy_customer_number = ext.pharmacy_customer_number == null ? "" : ext.pharmacy_customer_number,
                    pharmacy_fax = ext.pharmacy_fax == null ? "" : ext.pharmacy_fax,
                    preferred_geographic_market = ext.preferred_geographic_market == null ? "" : ext.preferred_geographic_market
                   

                });
             

            

                var ret1 = JsonConvert.SerializeObject(resp);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                string msg = "Successful login.";
                return Json(new { data = json1, message = msg, success = true });

            }


            return Json(new { data = new string[] { }, message = "", success = false });

        }

        private get_response_market _getMarketer_ext(long m_id) {
            var mx = db.MARKETER_ext.Where(a => a.rel_MARKETER_id == m_id);

            get_response_market resp = new get_response_market();
            foreach (var n in mx)
            {
                switch (n.attr_name)
                {
                    case "fax_number": resp.fax_number = n.value; break;

                    case "process_external_transfer":
                        resp.process_external_transfer = n.value; break;
                    case "intake_entity": resp.intake_entity = n.value; break;
                    case "pharmacy_relationship": resp.pharmacy_relationship = n.value; break;
                    case "technology_platform": resp.technology_platform = n.value; break;
                    case "pharmacy_customer_number": resp.pharmacy_customer_number = n.value; break;
                    case "pharmacy_fax": resp.pharmacy_fax = n.value; break;
                    case "preferred_geographic_market": resp.preferred_geographic_market = n.value; break;

                }

            }

            return resp;
        }

        private IHttpActionResult _saveMarketer(post_market m)
        {
           phoneValue(m);

            MARKETER mrk = new MARKETER
            {

                legal_entity_name = m.legal_entity_name,
                healthsplash_contact = m.healthsplash_contact,
                // addr_zip_id
                addr_address1 = m.address1,
                addr_address2 = string.IsNullOrEmpty(m.address2) == true? null: m.address2,
                addr_zip_id = m.zip_id,
                federal_tax_number = m.federal_tax_number,
                primary_phone = m.primary_phone,
                supplier_partner = m.supplier_partner,
                telemedicine_partner = m.telemedicine_partner,
                prim_contact_name_first = m.primary_contact_name_first,
                prim_contact_name_last = m.primary_contact_name_last,
                prim_contact_email = m.primary_contact_email,
                prim_contact_phone = m.primary_contact_phone,

                oper_contact_name_first = m.operational_name_first,
                oper_contact_name_last = m.operational_name_last,
                oper_contact_email = m.operational_email,
                oper_contact_phone = m.operational_phone ==null ? null:  m.operational_phone,

                fina_contact_name_first = m.financial_name_first,
                fina_contact_name_last = m.financial_name_last,
                fina_contact_email = m.financial_email,
                fina_contact_phone = m.financial_phone,

                auth_contact_name_first = m.authorize_signatory_name_first,
                auth_contact_name_last = m.authorize_signatory_name_last,
                auth_contact_email = m.authorize_signatory_email,
                auth_contact_phone = m.authorize_signatory_phone

            };

            db.MARKETERs.Add(mrk);
            db.SaveChanges();

            long user_id = saveMarketer_User(m);
            //bool saveMarketer_ext(string _attr_name, string _dname, string _value, long market_id = 0, long user_id = 0, int data_type = 0)            

            // mark.process_external_transfer
            if (!string.IsNullOrEmpty(m.process_external_transfer))
            {
                bool i = Entry.saveMarketer_ext("process_external_transfer", "Process External Transfer", m.process_external_transfer, mrk.id, 0, 0);
            }

            //mark.fax_number = "";
            if (!string.IsNullOrEmpty(m.fax_number))
            {
                bool i = Entry.saveMarketer_ext("fax_number", "fax_number", m.fax_number, mrk.id, 0, 0);
            }
            // mark.intake_entity 
            if (!string.IsNullOrEmpty(m.intake_entity))
            {
                bool i = Entry.saveMarketer_ext("intake_entity", "intake_entity", m.intake_entity, mrk.id, 0, 0);
            }
            // mark.technology_platform
            if (!string.IsNullOrEmpty(m.technology_platform))
            {
                bool i = Entry.saveMarketer_ext("technology_platform", "technology_platform", m.technology_platform, mrk.id, 0, 0);
            }
            //mark.pharmacy_relationship
            if (!string.IsNullOrEmpty(m.pharmacy_relationship))
            {
                bool i = Entry.saveMarketer_ext("pharmacy_relationship", "pharmacy_relationship", m.pharmacy_relationship, mrk.id, 0, 0);
            }
            //mark.unique_customer_number
            if (!string.IsNullOrEmpty(m.pharmacy_customer_number))
            {
                bool i = Entry.saveMarketer_ext("pharmacy_customer_number", "pharmacy_customer_number", m.pharmacy_customer_number, mrk.id, 0, 0);
            }
            //mark.pharmacy_fax
            if (!string.IsNullOrEmpty(m.pharmacy_fax))
            {
                bool i = Entry.saveMarketer_ext("pharmacy_fax", "pharmacy_fax", m.pharmacy_fax, mrk.id, 0, 0);
            }
            //mark.preferred_geographic_market
            if (!string.IsNullOrEmpty(m.preferred_geographic_market))
            {
                bool i = Entry.saveMarketer_ext("preferred_geographic_market", "preferred_geographic_market", m.preferred_geographic_market, mrk.id, 0, 0);
            }

            return Json(new { data = new string[] { }, message = "Successfully saved.", success = true });
        }

        private void phoneValue(post_market mark)
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
        private long saveMarketer_User(post_market m)
        {

            var u_find = db.USERs.Where(a => a.username == m.authorize_signatory_email.ToLower());
            USER u = new USER {
                name_first = m.authorize_signatory_name_first,
                name_last = m.authorize_signatory_name_last,
                username = m.authorize_signatory_email,
                dt_create = dt,
                create_by__USER_id = 0,
                rel_ref_USER_type_id = 10
            };

            db.USERs.Add(u);
            db.SaveChanges();

            return u.id;
        }

        private long xvalidateZip(string city, string state, string zip)
        {
            SV_db1Entities db = new SV_db1Entities();
            int nzip = 0;
            bool isvalid = int.TryParse(zip, out nzip);
            if (!isvalid)
            {
                return -1;
            }

            var ref_zip = db.ref_zip.Where(a => a.zip == zip);
            if (ref_zip.Count() > 0)
            {
                if (ref_zip.FirstOrDefault().city_name.ToLower() != city.ToLower())
                {
                    //return Json(new { data = new string[] { }, message = "Invalid city name.", success = false });
                    return 0;
                }

                if (ref_zip.FirstOrDefault().city_state.ToLower() != state.ToLower()
                    & ref_zip.FirstOrDefault().city_state_long.ToLower() != state.ToLower())
                {
                    //return Json(new { data = new string[] { }, message = "Invalid state name.", success = false });
                    return 0;
                }
            }

            //return Json(new { data = new string[] { }, message = "Match found.", success = true });
            return ref_zip.FirstOrDefault().id;
        }

        public static bool haserror = false;
        public static string errmsg = "", infomsg = "";
        /// <summary>
        /// To check if the request parameter is required or not.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool IsRequired(string key, string val, int i)
        {
            if (i == 1)
            {
                if (string.IsNullOrEmpty(val))
                {
                    haserror = true;
                    errmsg += key + " is required.\r\n";
                    return false;
                }
                return true;


            }
            else
            {
                if (string.IsNullOrEmpty(val))
                {
                    haserror = true;
                    errmsg += " Missing parameter: " + key + ".\r\n ";
                    return false;
                }
                return true;
            }

        }
    }

    public class get_market {
        public long id { get; set; }
    }
    public class post_market {
        public string legal_entity_name { get; set; }
        public string healthsplash_contact { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public long zip_id { get; set; }
        public string federal_tax_number { get; set; }//
        public string primary_phone { get; set; } // primary_phone
        public string supplier_partner { get; set; }
        public string telemedicine_partner { get; set; }


        public string fax_number { get; set; }
        public string process_external_transfer { get; set; }
        public string intake_entity { get; set; }
        public string technology_platform { get; set; } //Technology Platform for intake
        public string pharmacy_relationship { get; set;}
        public string pharmacy_customer_number { get; set; }//Unique Customer number with Pharmacy
        public string pharmacy_fax { get; set;}
        public string preferred_geographic_market { get; set;}

        public string primary_contact_name_first    { get; set; }
        public string primary_contact_name_last { get; set; }
        public string primary_contact_phone { get; set; }
        public string primary_contact_email { get; set; }


        public string operational_name_first  { get; set; }
        public string operational_name_last  { get; set; }
        public string operational_phone      { get; set; }
        public string operational_email       { get; set; }

        public string financial_name_first    { get; set; }
        public string financial_name_last     { get; set; }
        public string financial_phone        { get; set; }
        public string financial_email         { get; set; }

        public string authorize_signatory_name_first { get; set; }
        public string authorize_signatory_name_last { get; set; }
        public string authorize_signatory_phone { get; set; }
        public string authorize_signatory_email { get; set; }
    }

    public class get_response_market
    {
        public string legal_entity_name { get; set; }
        public string healthsplash_contact { get; set; }
        public List<zip_search_address2> address { get; set; }

        public string federal_tax_number { get; set; } //
       public string primary_phone { get; set; } // primary_phone
        public string telemedicine_partner { get; set; } // 
        public string supplier_partner { get; set; } // 

        public string fax_number { get; set; }
        public string process_external_transfer { get; set; }
        public string intake_entity { get; set; }
        public string technology_platform { get; set; } //Technology Platform for intake
        public string pharmacy_relationship { get; set; }
        public string pharmacy_customer_number { get; set; } // Unique Customer number with Pharmacy
        public string pharmacy_fax { get; set; }
        public string preferred_geographic_market { get; set; }

        public string primary_contact_name_first { get; set; }
        public string primary_contact_name_last { get; set; }
        public string primary_contacty_phone { get; set; }
        public string primary_contact_email { get; set; }


        public string operational_contact_name_first { get; set; }
        public string operational_contact_name_last { get; set; }
        public string operational_contact_phone { get; set; }
        public string operational_contact_email { get; set; }

        public string financial_contact_name_first { get; set; }
        public string financial_contact_name_last { get; set; }
        public string financial_contact_phone { get; set; }
        public string financial_contact_email { get; set; }

        public string authorize_contact_signatory_name_first { get; set; }
        public string authorize_contact_signatory_name_last { get; set; }
        public string authorize_contact_signatory_phone { get; set; }
        public string authorize_contact_signatory_email { get; set; }

       
    }
}
