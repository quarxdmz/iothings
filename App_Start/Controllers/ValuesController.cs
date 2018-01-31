using api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

using System.Net.Http.Headers;
using System.Text;


namespace api.Controllers
{

    //[Authorize]
    public class ValuesController : ApiController
    {
        SV_db1Entities dbEntity = new SV_db1Entities();
        // GET api/values
        public IEnumerable<string> Get()
       {
            return new string[] { "value1", "value2" };
        }


        //// POST api/values
        //public void Post([FromBody]string value)
        //{

        //}

        //public void Post()
        //{

        //}

        // GET api/values/5
        public string Get([FromUri]int id)
        {
            return "value";
        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("GetCity")]
        public IHttpActionResult GetCity([FromUri] doc_query query)
        {
            var detail = from a in dbEntity.ref_zip select a;
            List<zip_address> city_address = new List<zip_address>();
            int nTemp = 0;
            bool bTemp = int.TryParse(query.city, out nTemp);
            if(bTemp)
            {
                detail = detail.Where(a => a.zip == query.city);


                detail = detail.OrderBy(a => a.id);

                foreach (var i in detail)
                {
                    city_address.Add(new Models.zip_address
                    {
                        id = i.id,
                        zip = i.zip,
                        city_name = i.city_name,
                        city_state = i.city_state,
                        city_lat = i.city_lat,
                        city_lon = i.city_lon,
                        city_country = i.city_county
                    });
                }
            }
            else //if (!string.IsNullOrEmpty(query.city))
            {
                query.city = query.city.ToLower();
                detail = detail.Where(a => a.city_name.ToLower() == query.city);

                var detail1 = from o in dbEntity.ref_zip
                              where o.city_name.ToLower() == query.city
                              group o by   o.city_name into g
                           
                              orderby g.Key
                              select new  {
                                city = g.Key,
                                minLat = g.Min(z => z.city_lat ),
                                minLon = g.Min(z => z.city_lon),
                                maxLat = g.Max(z => z.city_lat),
                                maxLon = g.Max(z => z.city_lon)
                              };

               
                foreach (var i in detail1)
                {
                    city_address.Add(new zip_address
                    {
                        minLat = i.minLat,
                        minLon = i.minLon,
                        maxLat = i.maxLat,
                        maxLon = i.maxLon
                        
                    });
                }
            }


          

            //string msg = cnt.ToString() + (cnt > 1 ? " results" : " result") + " found.";

            var ret = JsonConvert.SerializeObject(city_address);
            var json = Newtonsoft.Json.Linq.JArray.Parse(ret);

            return Json(new { data = json, message = "msg", count = 0, success = true });
        }

      

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("GetProfile")]
        // 
        public IHttpActionResult GetProfile([FromUri]doc_query query)
        {
            // doctor_name, npi,license_no, address, state, city, zipcode

            if (string.IsNullOrEmpty(query.acck)) return Json(new { data = "", message = "Unauthorized Access. No api key was provided.", success = false });
            if (query.acck == "deftsoftapikey")
            {
                //if (!string.IsNullOrEmpty(query.patient_name)) query.patient_name = query.patient_name.ToLower(); else query.patient_name = string.Empty;
                //doctor_name = this.doctor_name;
                if (!String.IsNullOrEmpty(query.firstname)) query.firstname = query.firstname.ToLower().Trim(); else query.firstname = string.Empty;
                if (!String.IsNullOrEmpty(query.lastname)) query.lastname = query.lastname.ToLower().Trim(); else query.lastname = string.Empty;
                if (!String.IsNullOrEmpty(query.name)) query.name = query.name.ToLower().Trim(); else query.name = string.Empty;
                //npi = this.npi;
                if (!String.IsNullOrEmpty(query.npi)) query.npi = query.npi.ToLower().Trim(); else query.npi = string.Empty;
                //license_no = this.license_no;
                if (!String.IsNullOrEmpty(query.license)) query.license = query.license.ToLower().Trim(); else query.license = string.Empty;
                //address = this.address;
                if (!String.IsNullOrEmpty(query.address)) query.address = query.address.ToLower().Trim(); else query.address = string.Empty;
                //state = this.state;
                if (!String.IsNullOrEmpty(query.state)) query.state = query.state.ToLower().Trim(); else query.state = string.Empty;

                //city = this.city;
                int zip;
                bool isZip = int.TryParse(query.zipcode, out zip);
                if (!String.IsNullOrEmpty(query.city) && !isZip) query.city = query.city.ToLower().Trim(); else query.city = string.Empty;
                //zipcode = this.zipcode;
                zip = 0;
                isZip = int.TryParse(query.zipcode, out zip);
                if (!String.IsNullOrEmpty(query.zipcode) && isZip) query.zipcode = query.zipcode.ToLower().Trim(); else query.zipcode = string.Empty;


                var json = searchProfile(query);

                int cnt = doc_count;
                string msg = cnt.ToString() + (cnt > 1 ? " results" : " result") + " found.";

                var ret = Newtonsoft.Json.Linq.JArray.Parse(json);

                return Json(new { data = ret, message = msg, count = cnt, success = true });

            }
            else
            {
                return Json(new { data = "", message = "Unauthorized Access. Invalid api key was provided.", success = false });
            }

        }


        //[System.Web.Http.HttpPost]
        //[System.Web.Http.Route("doctor1")]
        //// public IHttpActionResult PostDoctor(string name, string gender, string title, string npi, string license) // [FromBody]string q
        //public IHttpActionResult PostDoctor([FromUri] doc_profile doc)
        //{

        //    try {

        //        if (string.IsNullOrEmpty(doc.acck)) return Json(new { data = "", message = "Unauthorized Access. No api key was provided.", success = false });
        //        if (doc.acck == "deftsoftapikey")
        //        {
        //            var zip = dbEntity.ref_zip.FirstOrDefault(a => a.zip.Contains(doc.zipcode));

        //            ADDRESS addr = new ADDRESS();
        //            if(!string.IsNullOrEmpty(doc.address1) && doc.address1.ToLower() != "null") addr.address1 = doc.address1;
        //            if(!string.IsNullOrEmpty(doc.address2) && doc.address2.ToLower() != "null") addr.address2 = doc.address2;
        //            addr.rel_ref_zip_id = zip.id;
        //            addr.last_update = DateTime.Now;

        //            dbEntity.ADDRESSes.Add(addr);
        //            dbEntity.SaveChanges();

        //            DOCTOR d = new DOCTOR();

        //            d.doc_name = doc.firstname + "|" + doc.lastname + "|" + doc.title;
        //            if (!string.IsNullOrEmpty(doc.gender) && doc.gender.ToLower() != "null") d.gender = doc.gender;
        //            if (!string.IsNullOrEmpty(doc.title) && doc.title.ToLower() != "null") d.title = doc.title;
        //            if (!string.IsNullOrEmpty(doc.npi) && doc.npi.ToLower() != "null") d.NPI = doc.npi;
        //            if (!string.IsNullOrEmpty(doc.license) && doc.license.ToLower() != "null") d.license_no = doc.license;
        //            if (!string.IsNullOrEmpty(doc.phone) && doc.phone.ToLower() != "null") d.phone = doc.phone;

        //            if (!string.IsNullOrEmpty(doc.faxto) && doc.faxto.ToLower() != "null") d.fax_to = doc.faxto;
        //            //if (!string.IsNullOrEmpty(doc.specialties) && doc.specialties.ToLower() != "null") d.specialty = doc.specialties;

        //            if (!string.IsNullOrEmpty(doc.address1) && doc.address1.ToLower() != "null") d.address1 = doc.address1;
        //            if (!string.IsNullOrEmpty(doc.address2) && doc.address2.ToLower() != "null") d.address2 = doc.address2;
        //            d.address_state = zip.city_state;
        //            if (!string.IsNullOrEmpty(doc.city) && doc.city.ToLower() != "null") d.address_city = doc.city;
        //            if (!string.IsNullOrEmpty(doc.zipcode) && doc.zipcode.ToLower() != "null") d.address_zip = doc.zipcode;
        //            d.rel_ADDRESS_id = addr.id;
        //            //d.last_update_by = 0;
        //            d.last_update = DateTime.Now;


        //            dbEntity.DOCTORs.Add(d);
        //            dbEntity.SaveChanges();

        //            return Json(new { data = "", message = "New record is saved.", success = true });
        //        }
        //        else
        //        {
        //            return Json(new { data = "", message = "Unauthorized Access. Invalid api key was provided.", success = false });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        // return BadRequest("Error occured upon saving. Record is not saved. ");

        //        return Json(new { data = "", message = "Error occurred upon saving. Record is not saved.", success = false });
        //    }
        //}

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("doctor1")]
        // public IHttpActionResult PostDoctor(string name, string gender, string title, string npi, string license) // [FromBody]string q
        public IHttpActionResult PostDoctor([FromUri] doc_profile doc)
        {

            try
            {

                if (string.IsNullOrEmpty(doc.acck)) return Json(new { data = "", message = "Unauthorized Access. No api key was provided.", success = false });
                if (doc.acck == "deftsoftapikey")
                {
                    var zip = dbEntity.ref_zip.FirstOrDefault(a => a.zip.Contains(doc.zipcode));

                    //ADDRESS addr = new ADDRESS();
                    //if (!string.IsNullOrEmpty(doc.address1) && doc.address1.ToLower() != "null") addr.address1 = doc.address1;
                    //if (!string.IsNullOrEmpty(doc.address2) && doc.address2.ToLower() != "null") addr.address2 = doc.address2;
                    //addr.rel_ref_zip_id = zip.id;
                    //addr.last_update = DateTime.Now;

                    //dbEntity.ADDRESSes.Add(addr);
                    //dbEntity.SaveChanges();

                    DOCTOR d = new DOCTOR();

                    d.name_first = doc.firstname;
                    d.name_last = doc.lastname;

                    // middlename
                    if (!string.IsNullOrEmpty(doc.middlename) && doc.middlename.ToLower() != "null") d.name_middle = doc.middlename;
                    // gender
                    if (!string.IsNullOrEmpty(doc.gender) && doc.gender.ToLower() != "null") d.gender = doc.gender;
                    // title
                    if (!string.IsNullOrEmpty(doc.title) && doc.title.ToLower() != "null") d.title = doc.title;
                    // npi
                    if (!string.IsNullOrEmpty(doc.npi) && doc.npi.ToLower() != "null") d.NPI = doc.npi;
                    // license
                    if (!string.IsNullOrEmpty(doc.license) && doc.license.ToLower() != "null") d.license_no = doc.license;
                    // phone
                    if (!string.IsNullOrEmpty(doc.phone) && doc.phone.ToLower() != "null") d.phone = doc.phone;

                    if (!string.IsNullOrEmpty(doc.faxto) && doc.faxto.ToLower() != "null") d.fax_to = doc.faxto;
                    // ADDRESS
                    if (!string.IsNullOrEmpty(doc.address) && doc.address.ToLower() != "null") d.addr_address1 = doc.address;
                   // if (!string.IsNullOrEmpty(doc.address2) && doc.address2.ToLower() != "null") d.addr_address2 = doc.address2;
                    //if (!string.IsNullOrEmpty(doc.city) && doc.city.ToLower() != "null") d.addr_address_city = doc.city;
                    if (zip != null)
                    {
                        d.addr_rel_ref_zip_id = zip.id;
                        //d.addr_address_state = zip.city_state;
                    }
                    
                    DateTime dtNow = DateTime.Now; 

                    //d.last_update_by = 0;
                    d.dt_create  =dtNow;


                    dbEntity.DOCTORs.Add(d);
                    dbEntity.SaveChanges();

                    return Json(new { data = "", message = "New record is saved.", success = true });
                }
                else
                {
                    return Json(new { data = "", message = "Unauthorized Access. Invalid api key was provided.", success = false });
                }

            }
            catch (Exception e)
            {
                // return BadRequest("Error occured upon saving. Record is not saved. ");

                return Json(new { data = "", message = "Error occurred upon saving. Record is not saved.", success = false });
            }

        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("GetInsurance")]
        public IHttpActionResult GetInsurance() {
            //var insurance = dbEntity.Doc_Insurance_Affiliation;
            
            var insurance = from a in dbEntity.ref_insurance_provider
                            orderby a.PayerName ascending
                            select a;

            List<insurance_class> ins = new List<insurance_class>();
            foreach (var i in insurance)
            {
                ins.Add(new insurance_class
                {
                    id = i.id,
                    PayerName = i.PayerName,
                    PayerID = i.PayerID
                });
            }

            var var1 = JsonConvert.SerializeObject(ins);
            //var ret = Newtonsoft.Json.Linq.JArray.Parse(var1);
            //return Json(var1, JsonRequestBehavior.AllowGet);

            var ret = Newtonsoft.Json.Linq.JArray.Parse(var1);

            return Json(new { data = ret, message = "", count = "", success = true });
        }

        //[System.Web.Http.HttpGet]
        //[System.Web.Http.Route("GetItemRequested")]
        //public IHttpActionResult GetItemRequested()
        //{
        //    //var insurance = dbEntity.Doc_Insurance_Affiliation;

        //    var items = from a in dbEntity.ITEMs
        //                where a.description.Contains("active|")
        //                select a;


        //    List<item_class> ins = new List<item_class>();
        //    foreach (var i in items)
        //    {
        //        ins.Add(new item_class
        //        {
        //            id = i.id,
        //            name = i.name,
        //            code = i.code,
        //            description = i.description
        //        });
        //    }




        //    var var1 = JsonConvert.SerializeObject(ins);
        //    //var ret = Newtonsoft.Json.Linq.JArray.Parse(var1);
        //    //return Json(var1, JsonRequestBehavior.AllowGet);
        //    var ret = Newtonsoft.Json.Linq.JArray.Parse(var1);
        //    return Json(new { data = ret, message = "", count = "", success = true });
        //}

        //[System.Web.Http.HttpPost]
        //[System.Web.Http.Route("patient")]
        //public IHttpActionResult PostPatient([FromUri] pat_profile pat)
        //{  //params :
        //   // zip_code

        //    try
        //    {

        //        if (string.IsNullOrEmpty(pat.acck)) return Json(new { data = "", message = "Unauthorized Access. No api key was provided.", success = false });
        //        if (pat.acck == "deftsoftapikey")
        //        {
        //            var ref_zip = dbEntity.ref_zip.FirstOrDefault(a => a.zip == pat.zipcode);

        //            // saving address 1, address 2, and ref_zip.id
        //            ADDRESS add = new ADDRESS();
        //            if (!string.IsNullOrEmpty(pat.address1) && pat.address1.ToLower() != "null") add.address1 = pat.address1;
        //            if (!string.IsNullOrEmpty(pat.address2) && pat.address2.ToLower() != "null") add.address2 = pat.address2;
        //            add.rel_ref_zip_id = ref_zip.id; // get the id from 'ref_zip'
        //            //add.rel_ref_country_id
        //            add.last_update = DateTime.Now;

        //            dbEntity.ADDRESSes.Add(add);
        //            dbEntity.SaveChanges();



        //            SOUL p = new SOUL();

        //            if (!string.IsNullOrEmpty(pat.firstname) && pat.firstname.ToLower() != "null") p.name_first = pat.firstname;
        //            if (!string.IsNullOrEmpty(pat.lastname) && pat.lastname.ToLower() != "null") p.name_last = pat.lastname;
        //            if (!string.IsNullOrEmpty(pat.homephone) && pat.homephone.ToLower() != "null") p.phone_number = pat.homephone;
        //            p.rel_ADDRESS_id = add.id;
        //            if (!string.IsNullOrEmpty(pat.email) && pat.email.ToLower() != "null") p.email = pat.email;
        //            if (!string.IsNullOrEmpty(pat.meta) && pat.meta.ToLower() != "null") p.meta = pat.meta; // notes

        //            p.OR_LIST_res_code = ""; // needs value
        //            p.last_contact = DateTime.Now; 
        //            p.last_contact_by = 0; // needs value
        //            p.last_update = DateTime.Now;
        //            p.last_update_by = 0; // needs value
        //            p.timestamp = DateTime.Now;
        //            if (!string.IsNullOrEmpty(pat.emergencyfirstname) && pat.emergencyfirstname.ToLower() != "null") p.alt_name = pat.emergencyfirstname + "|" + pat.emergencylastname; // Emergency Contact Name
        //            if (!string.IsNullOrEmpty(pat.emergencyphone) && pat.emergencyphone.ToLower() != "null") p.alt_phone = pat.emergencyphone; // Emergency Contact Phone
        //            if (!string.IsNullOrEmpty(pat.emergencyrelationship) && pat.emergencyrelationship.ToLower() != "null") p.alt_relationship = pat.emergencyrelationship; // Emergency Contact Relationship
        //            dbEntity.SOULs.Add(p);
        //            dbEntity.SaveChanges();

        //            // SOUL_info_2
        //            SOUL_info_2 p2 = new SOUL_info_2();
        //            p2.rel_SOUL_id = p.id;
        //            //birth_date = this.birth_date;
        //            if (!string.IsNullOrEmpty(pat.birthdate) && pat.birthdate.ToLower() != "null") p2.date_birth = Convert.ToDateTime(pat.birthdate);
        //            //height = this.height;
        //            p2.weight = pat.weight;
        //            //weight = this.weight;
        //            p2.height = pat.height;
        //            // gender = this.gender;
        //            p2.gender = pat.gender;

        //            dbEntity.SOUL_info_2.Add(p2);

        //            dbEntity.SaveChanges();

        //            return Json(new { data = "", message = "New record is saved.", success = true });
        //        }
        //        else
        //        {
        //            return Json(new { data = "", message = "Unauthorized Access. Invalid api key was provided.", success = false });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        //JsonResult jRes = new JsonResult();
        //        //jRes.Data = BadRequest("Error occurred upon saving. Record is not saved. ");
        //        //jRes.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //        //jRes.ContentType = "application/json; charset=utf-8";

        //        return Json(new { data = "", message = "Error occurred upon saving. Record is not saved.", success = false });
        //    }

        //}

        public string callbackQueryParameter
        {
            get { return callbackQueryParameter ?? "callback"; }
            set { callbackQueryParameter = value; }
        }
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("doctor1")]
        //[System.Web.Http.Route("GetDoctor/{id}/{name}/{specialty}")]
        public IHttpActionResult GetDoctor([FromUri]doc_query query) //, string id, string name, string specialty
        {
            // doctor_name, npi,license_no, address, state, city, zipcode

            if (string.IsNullOrEmpty(query.acck)) return Json(new { data = "", message = "Unauthorized Access. No api key was provided.", success = false });
            if (query.acck == "deftsoftapikey")
            {
                //if (!string.IsNullOrEmpty(query.patient_name)) query.patient_name = query.patient_name.ToLower(); else query.patient_name = string.Empty;
                //doctor_name = this.doctor_name;
                if (!String.IsNullOrEmpty(query.firstname)) query.firstname = query.firstname.ToLower().Trim(); else query.firstname = string.Empty;
                if (!String.IsNullOrEmpty(query.lastname)) query.lastname = query.lastname.ToLower().Trim(); else query.lastname = string.Empty;
                if (!String.IsNullOrEmpty(query.name)) query.name = query.name.ToLower().Trim(); else query.name = string.Empty;
                //npi = this.npi;
                if (!String.IsNullOrEmpty(query.npi)) query.npi = query.npi.ToLower().Trim(); else query.npi = string.Empty;
                //license_no = this.license_no;
                if (!String.IsNullOrEmpty(query.license)) query.license = query.license.ToLower().Trim(); else query.license = string.Empty;
                //address = this.address;
                if (!String.IsNullOrEmpty(query.address)) query.address = query.address.ToLower().Trim(); else query.address = string.Empty;
                //state = this.state;
                if (!String.IsNullOrEmpty(query.state)) query.state = query.state.ToLower().Trim(); else query.state = string.Empty;

                //city = this.city;
                int zip;
                bool isZip = int.TryParse(query.zipcode, out zip);
                if (!String.IsNullOrEmpty(query.city) && !isZip) query.city = query.city.ToLower().Trim(); else query.city = string.Empty;
                //zipcode = this.zipcode;
                zip = 0;
                isZip = int.TryParse(query.zipcode, out zip);
                if (!String.IsNullOrEmpty(query.zipcode) && isZip) query.zipcode = query.zipcode.ToLower().Trim(); else query.zipcode = string.Empty;


                var json = searchResponse(query);

                int cnt = doc_count;
                string msg = cnt.ToString() + (cnt > 1 ? " results" : " result") + " found.";

                var ret = Newtonsoft.Json.Linq.JArray.Parse(json);

                return Json(new { data = ret, message = msg, count = cnt, success = true });

            }
            else
            {
                return Json(new { data = "", message = "Unauthorized Access. Invalid api key was provided.", success = false });
            }

        }

        public string searchResponse(doc_query param1)
        {
            // search by npi, address, name
            // doc_specialty_profile <doctor name  specialty>

            //var result1 = dbEntity.DOCTORs.Where(a => a.doctor_name.ToLower().Contains(param1.name) 
            //            || a.doctor_NPI.Contains(param1.npi)
            //            || a.doctor_license_no.Contains(param1.license)
            //            || a.doctor_address1.ToLower().Contains(param1.address) || a.doctor_address2.ToLower().Contains(param1.address)
            //            || a.doctor_address_city.ToLower().Contains(param1.city) || a.doctor_address_city.ToLower().Contains(param1.city) || a.doctor_address_city.ToLower().Contains(param1.city)
            //            || a.doctor_address_state.ToLower().Contains(param1.state) || a.doctor_address_state.ToLower().Contains(param1.state) || a.doctor_address_state.ToLower().Contains(param1.state)
            //            || a.doctor_address_zip.ToLower().Contains(param1.zipcode) || a.doctor_address_zip.ToLower().Contains(param1.zipcode) || a.doctor_address_zip.ToLower().Contains(param1.zipcode)
            //            );


            //var detail = (from a in dbEntity.DOCTORs
            //              select new { a.id, a.doctor_name,
            //                  a.doctor_NPI,
            //                  a.doctor_specialty,
            //                  a.title,
            //                  a.gender,
            //                  a.doctor_phone,
            //                  a.doctor_NPI_type, a.doctor_org_name, a.ADDRESS.address1, a.ADDRESS.address2, a.doctor_pecos_cert,
            //                  a.doctor_fax_to,
            //                  a.doctor_license_no,
            //                  a.doctor_address_state,
            //                  a.ADDRESS.ref_zip.zip,
            //                  a.ADDRESS.ref_zip.city_lat,
            //                  a.ADDRESS.ref_zip.city_lon,
            //                  a.ADDRESS.ref_zip.city_state,
            //                  a.ADDRESS.ref_zip.city_name,
            //                  a.image_url
            //              }).ToList().Select( 
            //    w => new {
            //        id = w.id, ///
            //        name = w.doctor_name,
            //        firstname = w.doctor_name.Split('|')[0],
            //        lastname = w.doctor_name.Split('|')[1],
            //        gender = w.gender,
            //        title = w.doctor_name.Split('|')[2],
            //        specialty = w.doctor_specialty == null ? "" : w.doctor_specialty,
            //        faxt_to = w.doctor_fax_to,
            //        doctor_NPI = w.doctor_NPI,
            //        doctor_phone = w.doctor_phone,
            //        doctor_NPI_type =w.doctor_NPI_type,
            //        orgname = w.doctor_org_name,
            //        doctor_license_no = w.doctor_license_no,
            //        doctor_org_name = w.doctor_org_name,
            //        doctor_pecos_cert = w.doctor_pecos_cert == null ? false : Convert.ToBoolean(w.doctor_pecos_cert.Value),
            //        image_url = w.image_url,

            //        address1 = w.address1,
            //        address2 = w.address2,
            //        city_state = w.city_state,
            //        city_name = w.city_name,
            //        city_lat = w.city_lat,
            //        city_long = w.city_lon,
            //        zip = w.zip
            //    }) ;

            var detail = from a in dbEntity.DOCTORs select a;

            if (!string.IsNullOrEmpty(param1.name))
            {
                string[] n = param1.name.Split(' ');

                switch (n.Length)
                {
                    case 1:
                        param1.firstname = param1.name.Split(' ')[0];
                        detail = detail.Where(b => b.name_first.ToLower().Contains(param1.name)
                                                || b.name_last.ToLower().Contains(param1.name)
                                                || b.name_middle.ToLower().Contains(param1.name));
                        break;
                    case 2:
                        param1.firstname = param1.name.Split(' ')[0];
                        param1.lastname = param1.name.Split(' ')[1];
                        break;
                }


                //detail = detail.Where(b => b.doctor_name.ToLower().Contains(param1.firstname)); //firstname | lastname | title


            }


            if (!string.IsNullOrEmpty(param1.firstname))
            {
                //detail = detail.Where(b => b.doctor_name.ToLower().Contains(param1.firstname)); //firstname | lastname | title

                //detail = detail.Where(b => b.doctor_name.ToLower().StartsWith(param1.firstname + "|"));
                detail = detail.Where(b => b.name_first.ToLower().Contains(param1.firstname));
            }


            if (!string.IsNullOrEmpty(param1.lastname))
            {
                //detail = detail.Where(b => b.doctor_name.ToLower().Split('|')[1].Contains(param1.lastname)); //firstname | lastname | title
                detail = detail.Where(b => b.name_last.ToLower().Contains( param1.lastname));
            }

            if (!string.IsNullOrEmpty(param1.npi))
                detail = detail.Where(b => b.NPI.Contains(param1.npi));
            if (!string.IsNullOrEmpty(param1.license))
                detail = detail.Where(b => b.license_no.Contains(param1.license));

            if (!string.IsNullOrEmpty(param1.address))
            {
                //List<long> addr = new List<long>();
                ////var address = dbEntity.ADDRESSes.Where(a => a.address1.ToLower().Contains(param1.address) || a.address2.ToLower().Contains(param1.address));
                //SOUL address = dbEntity.SOUL.Where(a => a.address1.ToLower().Contains(param1.address) || a.address2.ToLower().Contains(param1.address));
                //// create a class type for this
                //foreach (var a in address)
                //{
                //    addr.Add(a.id);
                //}
                //detail = detail.Where(a => addr.Contains(a.rel_ADDRESS_id.Value));
                ////detail = detail.Where(a => a.ADDRESS.address1.ToLower().Contains(param1.address) || a.ADDRESS.address2.ToLower().Contains(param1.address));
                detail = detail.Where(a => a.addr_address1.ToLower().Contains(param1.address)
                        || a.addr_address2.ToLower().Contains(param1.address));
            }

            if (!string.IsNullOrEmpty(param1.state))
            {
           

                List<long> _state = new List<long>();
                var state = dbEntity.ref_zip.Where(a => a.city_state.ToLower().Contains(param1.state));
                // create a class type for this
                foreach (var a in state)
                {
                    _state.Add(a.id);
                }
                if (_state.Contains(46))
                {

                }

                //detail = detail.Where(a => _state.Contains(a.rel_ADDRESS_id.Value));

                //detail = detail.Where(a => a.ADDRESS.ref_zip.city_state.ToLower().Contains(param1.state));
            }

            if (!string.IsNullOrEmpty(param1.city))
            {
                //4-15 detail = detail.Where(a => a.ADDRESS.ref_zip.city_name.ToLower().Contains(param1.city));

                List<long> _city = new List<long>();
                var city = dbEntity.ref_zip.Where(a => a.city_name.ToLower().Contains(param1.city));

                foreach (var a in city)
                {
                    _city.Add(a.id);
                }
                detail = detail.Where(a => _city.Contains(a.addr_rel_ref_zip_id.Value));

            }

            if (!string.IsNullOrEmpty(param1.zipcode))
            {
                List<long> addr_zip = new List<long>();
                var ref_zip = dbEntity.ref_zip.Where(a => a.zip == param1.zipcode);
                foreach (var i in ref_zip)
                {
                    addr_zip.Add(i.id);
                }

                detail = detail.Where(b => addr_zip.Contains(b.addr_rel_ref_zip_id.Value));

                //detail = detail.Where(a => a.ADDRESS.ref_zip.zip.Contains(param1.zipcode));
            }


            // name,title,npi,licenseno,address, state, city, zipcode
            List<doc_list> dc = new List<doc_list>();
            doc_profile prof = new doc_profile();

            // insurance
            if (!string.IsNullOrEmpty(param1.insurance))
            {
                //param1.insurance = param1.insurance.ToLower();

                //string[] ins = param1.insurance.Split(',');

                //var insurance = (from c in dbEntity.Doc_Insurance_Affiliation
                //                 where ins.Contains(c.ref_insurancecompanies.PayerID)
                //                 select c.doctor_id).ToList();

                //if (detail.Count() > 0)
                //{
                //    if (insurance.Count() > 0)
                //    {
                //        detail = detail.Where(c => insurance.Contains(c.id));
                //    }
                //}
            }

            // specialty
            if (!string.IsNullOrEmpty(param1.name))
            {
                //var specialty = (from c in dbEntity.Doc_Specialties_Profile
                //                 where c.ref_specialties.name.ToLower().Contains(param1.name)
                //                 select c.doctor_id).ToList();

                //if (detail.Count() > 0)
                //{
                //    if (specialty.Count() > 0)
                //    {
                //        detail = detail.Where(c => specialty.Contains(c.id));
                //    }
                //}
                //else // if param1.name returns 0 count in searching through name, will search through specialty
                //{
                //    detail = from a in dbEntity.DOCTORs
                //             where specialty.Contains(a.id)
                //             select a;
                //}
            }


            detail = detail.OrderBy(a => a.name_last);
            if (!Double.IsNaN(param1.take) && param1.take > 0 && !double.IsNaN(param1.skip))
                detail = detail.Take(param1.take).Skip(param1.skip);


            foreach (var li in detail)
            {
                //var addr = dbEntity.ADDRESSes.Find(li.rel_ADDRESS_id.Value);
                var addr = dbEntity.ref_zip.Find(li.addr_rel_ref_zip_id);

                prof = new Models.doc_profile
                {

                  id = li.id,
                    firstname = li.name_first,
                    lastname = li.name_last,
                    middlename = li.name_last,
                    gender = li.gender.Trim(),
                    title = li.title,
                    phone = li.phone,
                    npi = li.NPI,
                    // npitype = li.NPI_type == null ? "" : li.NPI_type.Trim(),
                    orgname = li.organ_name,
                    //specialties = li.specialty,
                    image_url = li.image_url,
                    //add_id =  ,
                   
                    address = li.addr_address1,
                    //address2 = li.addr_address2,
                    pecoscert = li.pecos_cert == null ? false : Convert.ToBoolean(li.pecos_cert),
                    faxto = li.fax_to,
                    state = addr != null ? "" : addr.city_state,
                    city = addr != null ? "" : addr.city_name,
                    city_lat = addr != null ? 0 : addr.city_lat,
                    city_long = addr != null ? 0 : addr.city_lon,
                    zipcode = addr != null ? "" : addr.zip

                };

                //List<doc_specialty> spec = new List<doc_specialty>();
                //var sp = dbEntity.Doc_Specialties_Profile.Where(a => a.doctor_id == li.id);
                //foreach (var i in sp)
                //{
                //    spec.Add(new doc_specialty
                //    {
                //        id = i.specialty_id.Value,
                //        name = i.ref_specialties.name,
                //        actor = i.ref_specialties.actor
                //    });
                //}
                dc.Add(new doc_list { profile = prof });

                doc_count++;

            }

            var var1 = JsonConvert.SerializeObject(dc);

            return var1;
        }

        [System.Web.Http.HttpGet]
        //[System.Web.Http.Route("patient")]
        public IHttpActionResult GetPatient([FromUri]patient_query query)
        {
            // patient_name, phone_number, birth_date, addres, zipcode emergency_contact, doctor_name
            try { } catch (Exception e) { }

            if (string.IsNullOrEmpty(query.acck)) return Json(new { data = "", message = "No api key was provided.", success = false });
            if (query.acck == "deftsoftapikey")
            {


                
               
                //if (!string.IsNullOrEmpty(query.birth_date)) query.patient_name = query.patient_name.ToLower(); else query.patient_name = string.Empty;
               
                //zipcode = this.zipcode;
                if (!string.IsNullOrEmpty(query.zipcode)) query.zipcode = query.zipcode.ToLower(); else query.zipcode = string.Empty;
                //emergency_contact = this.emergency_contact;
                if (!string.IsNullOrEmpty(query.emergencycontact)) query.emergencycontact = query.emergencycontact.ToLower(); else query.emergencycontact = string.Empty;
                //doctor_name = this.doctor_name;
                if (!string.IsNullOrEmpty(query.doctor)) query.doctor = query.doctor.ToLower(); else query.doctor = string.Empty;

                //var result = dbEntity.SOULs.Where(a => a.name_first.ToLower().Contains(query.name) || a.name_last.ToLower().Contains(query.name)
                //            || a.phone_number.Contains(query.phone)
                //          );
                var detail = from a in dbEntity.SOULs select a;


                if (!string.IsNullOrEmpty(query.firstname)) {
                    query.firstname = query.firstname.ToLower();
                    detail = detail.Where(a => a.name_first.ToLower() == query.firstname);

                }

                if (!string.IsNullOrEmpty(query.lastname))
                {
                    query.firstname = query.lastname.ToLower();
                    detail = detail.Where(a => a.name_last.ToLower() == query.lastname);

                }


                if (!string.IsNullOrEmpty(query.phone))
                {
                    detail = detail.Where(a => a.phone == query.phone);
                }
             

                if (!string.IsNullOrEmpty(query.address))
                {
                    //query.address = query.address.ToLower();
                    List<Guid> addr = new List<Guid>();

                   // var address = dbEntity.SOULs.Where(a => a.s_address1.ToLower().Contains(query.address) || a.s_address2.ToLower().Contains(query.address));
                    // create a class type for this
                    //foreach (var a in address)
                    //{
                    //    addr.Add(a.guid);
                    //}

                    detail = detail.Where(a => a.addr_address1.ToLower().Contains(query.address) || a.addr_address2.ToLower().Contains(query.address));

                    //detail = detail.Where(a=> a.ADDRESS.address1.ToLower().Contains(query.address) || a.ADDRESS.address2.ToLower().Contains(query.address));
                }

                if (!string.IsNullOrEmpty(query.state))
                {
                    List<long> addr_state = new List<long>();
                    List<long> _state = new List<long>();
                    var state = dbEntity.ref_zip.Where(a => a.city_state.ToLower().Contains(query.state));
                    // create a class type for this
                    foreach (var a in state)
                    {
                        _state.Add(a.id);
                    }

                    //var address = dbEntity.SOULs.Where(a => _state.Contains(a.s_zip));
                    //foreach (var a in address)
                    //{
                    //    addr_state.Add(a.id);
                    //}
                    //detail = detail.Where(a => addr_state.Contains(a.rel_ADDRESS_id));

                    detail = detail.Where(a => _state.Contains(a.addr_rel_ref_zip_id.Value));

                }

                if (!string.IsNullOrEmpty(query.city))
                {
                    List<long> addr_city = new List<long>();
                    List<long> _city = new List<long>();
                    var city = dbEntity.ref_zip.Where(a => a.city_name.ToLower().Contains(query.city));
                    // create a class type for this
                    foreach (var a in city)
                    {
                        addr_city.Add(a.id);
                    }

                    //var address = dbEntity.ADDRESSes.Where(a => addr_city.Contains(a.rel_ref_zip_id));
                    //foreach (var a in address)
                    //{
                    //    _city.Add(a.id);
                    //}
                    //detail = detail.Where(a => _city.Contains(a.rel_ADDRESS_id));

                    detail = detail.Where(a => _city.Contains(a.addr_rel_ref_zip_id.Value));
                }


                //List<long> addr_zip = new List<long>();
                if (!string.IsNullOrEmpty(query.zipcode))
                {
                    var zip = dbEntity.ref_zip.Where(a => a.zip.Contains(query.zipcode));
                    // create a class type for this
                    List<long> addr_zip = new List<long>();
                    foreach (var a in zip)
                    {
                        addr_zip.Add(a.id);
                    }
                    detail = detail.Where(a => addr_zip.Contains(a.addr_rel_ref_zip_id.Value));

                    //detail = detail.Where(a => a.ADDRESS.ref_zip.zip.Contains(query.zipcode));

                }

                //List<pat_profile> pat = new List<pat_profile>();
                int cnt = 0;
                List<patient_list> patient = new List<patient_list>();
                foreach (var li in detail)
                {
                   
                    //var s_info2 = dbEntity.SOUL_info_2.Where(a => a.rel_SOUL_id == li.guid);
                    var addr = dbEntity.ref_zip.Find(li.addr_rel_ref_zip_id);
                    string bdate=string.Empty;
                    //foreach (var v in s_info2)
                    //{
                    //    bdate = v.date_birth.Value.ToShortDateString();
                    //}

                    patient.Add(new patient_list { profile = new pat_profile {
                        id = li.id,
                        firstname = li.name_first,
                        lastname = li.name_last,
                        birthdate = bdate,
                        homephone = li.phone,
                        //rel_address_id = li.rel_ADDRESS_id,
                        //zip_id = zip.id,
                        zipcode = addr.zip,
                        state = addr.city_state,
                        city = addr.city_name,
                        city_lat = addr.city_lat,
                        city_long = addr.city_lon,
                        email = li.email,
                        //or_list_res_code = li.OR_LIST_res_code,
                        //last_contact_by = Convert.ToInt32(li.last_contact_by),
                        //last_update =  li.last_update.Value.ToShortDateString(),
                        //emergencyfirstname = li.alt_name.Split('|')[0], // Emergency Contact Name
                        //emergencylastname = li.alt_name.Split('|')[1],
                        //emergencyphone = li.alt_phone, // Emergency Contact Phone
                        //emergencyrelationship = li.alt_relationship // Emergency Contact Relationship
                    }
                    });
                    cnt++;
               }

                var ret = JsonConvert.SerializeObject(patient);
                var json = Newtonsoft.Json.Linq.JArray.Parse(ret);
                string msg = cnt.ToString() + (cnt > 1 ? " results" : " result") + " found.";

                return Json(new { data = json, message = msg, count = cnt, success = true });
            }
            else
            {
                return Json(new { data = "", message = "Invalid api key was provided.", success = false });
            }
        }

        private int doc_count;
        public string searchProfile(doc_query param1)
        {
            // search by npi, address, name
            // doc_specialty_profile <doctor name  specialty>

            //var result1 = dbEntity.DOCTORs.Where(a => a.doctor_name.ToLower().Contains(param1.name) 
            //            || a.doctor_NPI.Contains(param1.npi)
            //            || a.doctor_license_no.Contains(param1.license)
            //            || a.doctor_address1.ToLower().Contains(param1.address) || a.doctor_address2.ToLower().Contains(param1.address)
            //            || a.doctor_address_city.ToLower().Contains(param1.city) || a.doctor_address_city.ToLower().Contains(param1.city) || a.doctor_address_city.ToLower().Contains(param1.city)
            //            || a.doctor_address_state.ToLower().Contains(param1.state) || a.doctor_address_state.ToLower().Contains(param1.state) || a.doctor_address_state.ToLower().Contains(param1.state)
            //            || a.doctor_address_zip.ToLower().Contains(param1.zipcode) || a.doctor_address_zip.ToLower().Contains(param1.zipcode) || a.doctor_address_zip.ToLower().Contains(param1.zipcode)
            //            );


            //var detail = (from a in dbEntity.DOCTORs
            //              select new { a.id, a.doctor_name,
            //                  a.doctor_NPI,
            //                  a.doctor_specialty,
            //                  a.title,
            //                  a.gender,
            //                  a.doctor_phone,
            //                  a.doctor_NPI_type, a.doctor_org_name, a.ADDRESS.address1, a.ADDRESS.address2, a.doctor_pecos_cert,
            //                  a.doctor_fax_to,
            //                  a.doctor_license_no,
            //                  a.doctor_address_state,
            //                  a.ADDRESS.ref_zip.zip,
            //                  a.ADDRESS.ref_zip.city_lat,
            //                  a.ADDRESS.ref_zip.city_lon,
            //                  a.ADDRESS.ref_zip.city_state,
            //                  a.ADDRESS.ref_zip.city_name,
            //                  a.image_url
            //              }).ToList().Select( 
            //    w => new {
            //        id = w.id, ///
            //        name = w.doctor_name,
            //        firstname = w.doctor_name.Split('|')[0],
            //        lastname = w.doctor_name.Split('|')[1],
            //        gender = w.gender,
            //        title = w.doctor_name.Split('|')[2],
            //        specialty = w.doctor_specialty == null ? "" : w.doctor_specialty,
            //        faxt_to = w.doctor_fax_to,
            //        doctor_NPI = w.doctor_NPI,
            //        doctor_phone = w.doctor_phone,
            //        doctor_NPI_type =w.doctor_NPI_type,
            //        orgname = w.doctor_org_name,
            //        doctor_license_no = w.doctor_license_no,
            //        doctor_org_name = w.doctor_org_name,
            //        doctor_pecos_cert = w.doctor_pecos_cert == null ? false : Convert.ToBoolean(w.doctor_pecos_cert.Value),
            //        image_url = w.image_url,

            //        address1 = w.address1,
            //        address2 = w.address2,
            //        city_state = w.city_state,
            //        city_name = w.city_name,
            //        city_lat = w.city_lat,
            //        city_long = w.city_lon,
            //        zip = w.zip
            //    }) ;

            var detail = from a in dbEntity.DOCTORs select a;
            if (!string.IsNullOrEmpty(param1.name))
            {
                string[] n = param1.name.Split(' ');

                switch (n.Length)
                {
                    case 1:
                        param1.firstname = param1.name.Split(' ')[0];
                        detail = detail.Where(b => b.name_first.ToLower().Contains(param1.name));
                        break;
                    case 2:
                        param1.firstname = param1.name.Split(' ')[0];
                        param1.lastname = param1.name.Split(' ')[1];
                        break;
                }
                //detail = detail.Where(b => b.doctor_name.ToLower().Contains(param1.firstname)); //firstname | lastname | title
            }


            if (!string.IsNullOrEmpty(param1.firstname))
            {
                //detail = detail.Where(b => b.doctor_name.ToLower().Contains(param1.firstname)); //firstname | lastname | title

                //detail = detail.Where(b => b.name_first.ToLower().StartsWith(param1.firstname + "|"));
                detail = detail.Where(b => b.name_first.ToLower().Contains(param1.firstname));
            }


            if (!string.IsNullOrEmpty(param1.lastname))
            {
                //detail = detail.Where(b => b.name_last.ToLower().Contains("|" + param1.lastname + "|"));
                detail = detail.Where(b => b.name_last.ToLower().Contains(param1.lastname));
            }


            if (!string.IsNullOrEmpty(param1.npi))
                detail = detail.Where(b => b.NPI.Contains(param1.npi));
            if (!string.IsNullOrEmpty(param1.license))
                detail = detail.Where(b => b.license_no.Contains(param1.license));

            List<long> addr = new List<long>();
            if (!string.IsNullOrEmpty(param1.address))
            {
                //var address = dbEntity.ADDRESSes.Where(a => a.address1.ToLower().Contains(param1.address) || a.address2.ToLower().Contains(param1.address));
                // create a class type for this
                //foreach (var a in address)
                //{
                //    addr.Add(a.id);
                //}
                //detail = detail.Where(a => addr.Contains(a.rel_ADDRESS_id.Value));


                detail = detail.Where(a => a.addr_address1.ToLower().Contains(param1.address) || a.addr_address2.ToLower().Contains(param1.address));
            }

            List<long> _state = new List<long> ();
            if (!string.IsNullOrEmpty(param1.state))
            {
                //var state = dbEntity.ref_zip.Where(a => a.city_state.ToLower().Contains(param1.state));
                //// create a class type for this
                //foreach (var a in state)
                //{
                //    _state.Add(a.id);
                //}

                var state = dbEntity.ref_zip.Where(a => a.city_state.ToLower().Contains(param1.state));
                // create a class type for this
                foreach (var a in state)
                {
                    _state.Add(a.id);
                }


                //var address = dbEntity.ADDRESSes.Where(a => _state.Contains(a.rel_ref_zip_id));
                //List<long> addr_state = new List<long>();
                //foreach (var a in address)
                //{
                //    addr_state.Add(a.id);
                //}
                detail = detail.Where(a => _state.Contains(a.addr_rel_ref_zip_id.Value));

                //detail = detail.Where(a => a.ADDRESS.ref_zip.city_state.ToLower().Contains(param1.state));
            }

            if (!string.IsNullOrEmpty(param1.city))
            {
                //4-15 detail = detail.Where(a => a.ADDRESS.ref_zip.city_name.ToLower().Contains(param1.city));

                List<long> _city = new List<long>();
                var city = dbEntity.ref_zip.Where(a => a.city_name.ToLower().Contains(param1.city));

                foreach (var a in city)
                {
                    _city.Add(a.id);
                }
                detail = detail.Where(a => _city.Contains(a.addr_rel_ref_zip_id.Value));
            }

            if (!string.IsNullOrEmpty(param1.zipcode))
            {
                List<long> addr_zip = new List<long>();
                var ref_zip = dbEntity.ref_zip.Where(a => a.zip == param1.zipcode);
                foreach (var i in ref_zip)
                {
                    addr_zip.Add(i.id);
                }
                detail = detail.Where(b => addr_zip.Contains(b.addr_rel_ref_zip_id.Value));

                //detail = detail.Where(a => a.ADDRESS.ref_zip.zip.Contains(param1.zipcode));
            }

            detail = detail.OrderBy(a => a.name_last);
            if (!Double.IsNaN(param1.take) && param1.take > 0 && !double.IsNaN(param1.skip))
                detail = detail.Take(param1.take).Skip(param1.skip);

            // name,title,npi,licenseno,address, state, city, zipcode
            List<doc_list> dc = new List<doc_list>();
            doc_profile prof = new doc_profile();
            
                foreach (var li in detail)
                {

                     var addr1 = dbEntity.ref_zip.Find(li.addr_rel_ref_zip_id.Value);

                      prof = new Models.doc_profile
                    {

                        id = li.id,
                        firstname = li.name_first,
                        lastname = li.name_last,
                        middlename = li.name_middle,
                        gender = li.gender.Trim(),
                        title = li.title,
                        phone = li.phone,
                        npi = li.NPI,
                        //npitype = li.NPI_type == null ? "" : li.NPI_type.Trim(),
                        orgname = li.organ_name,
                        image_url = li.image_url,
                        //add_id =  ,
                        address = li.addr_address1,
                        //address2 = li.addr_address2,
                        pecoscert = Convert.ToBoolean(li.pecos_cert),
                        faxto = li.fax_to,
                        state = addr1 != null ? addr1.city_state : "",
                        city = addr1 != null ? addr1.city_name : "",
                        city_lat = addr1 != null? addr1.city_lat : 0,
                        city_long = addr1 != null ? addr1.city_lon : 0,
                        zipcode = addr1 != null ? addr1.zip : "",
                        bio = li.bio
                    };

                    //List<doc_specialty> spec = new List<doc_specialty>();
                    //var sp = dbEntity.Doc_Specialties_Profile.Where(a => a.doctor_id == li.id);
                    //foreach (var i in sp)
                    //{
                    //    spec.Add(new doc_specialty
                    //    {
                    //        id = i.specialty_id.Value,
                    //        name = i.ref_specialties.name,
                    //        actor = i.ref_specialties.actor
                    //    });
                    //}

                    //// balik
                    //List<doc_education> educ = new List<doc_education>();
                    //var ed = dbEntity.Doc_Education.Where(a => a.DOCTOR.id == li.id);
                    //foreach (var i in ed)
                    //{
                    //    educ.Add(new doc_education
                    //    {
                    //        school = i.school,
                    //        graduation_year = i.graduation_year,
                    //        degree = i.degree
                    //    });
                    //}

                    //List<doc_insurance> insu = new List<doc_insurance>();
                    //var ins = dbEntity.Doc_Insurance.Where(a => a.doc_id == li.id);
                    //foreach (var i in ins)
                    //{

                    //    insu.Add(new doc_insurance
                    //    {
                    //        //plan_uid = i.ref_insurance_plan.uid,
                    //        //plan_name = i.ref_insurance_plan.name,
                    //        //plan_category = i.ref_insurance_plan.category,
                    //        provider_id = i.ref_insurance_plan.ref_insurancecompanies.uid,
                    //        provider_name = (i.ref_insurance_plan.ref_insurancecompanies.PayerName).Split('|')[0]
                    //    });
                    //}

                    dc.Add(new doc_list { profile = prof });

                    doc_count++;
                }
            
               
            var var1 = JsonConvert.SerializeObject(dc);

            return var1;
        }


        // reference: http://stackoverflow.com/questions/11775594/how-to-secure-an-asp-net-web-api/11782361#11782361
        // http://stackoverflow.com/questions/11775594/how-to-secure-an-asp-net-web-api/11782361#11782361
        // http://www.piotrwalat.net/hmac-authentication-in-asp-net-web-api/
        private static string computehash(string hashedpassword, string message = "deftsoft")
        {
            var key = System.Text.Encoding.UTF8.GetBytes(hashedpassword.ToUpper());
            string hashstring;

            using (var hmac = new System.Security.Cryptography.HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(message));
                hashstring = Convert.ToBase64String(hash);
            }

            return hashstring;
        }
        //        So, how to prevent replay attack?
        //        Add constraint for the timestamp, something like:
        //servertime - X minutes|seconds  <= timestamp <= servertime + X minutes|seconds

        //steps:
        // create hashstring, then send to client
        // when client request, verify the api key
        
        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {


        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }


    
}
