using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Net.Http;
using System.Threading.Tasks;
using api.Models;
using Newtonsoft.Json;
using System.Text;
using System.Data.Entity;
using System.Net;
using System.IO;
using System.Security.Cryptography.Xml;
using System.Globalization;
using NUnit.Framework;
using System.Web.Http.Hosting;


namespace api.Controllers
{

    public class DoctorController : Base.doctor
    {
        SV_db1Entities dbEntity = new SV_db1Entities();
        //HSAuth authorized = new HSAuth();
        DateTime dt = DateTime.UtcNow;
        //System.Web.HttpContext httpCon = System.Web.HttpContext.Current;

            // date create: 1/3/2018
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("web/doctor/login")]
        public IHttpActionResult post_webdoctorlogin(post_webdoctorlogin param) {
            // if there is a match, redirect user to profile screen
            // request parameter: email/account id, password
            // response: profile details

            // if email/account id does not exists
            // request parameter: npi no, password, image, practice name, phone no, list of specialties, address, bio, list of degrees, list of languages, list of insurances, practising year
            // response: profile details

            try {
                if (!string.IsNullOrEmpty(param.email)) param.email = param.email.ToLower();

                var doc_profile = dbEntity.DOCTOR1.Where(a => a.email == param.email);

                List<doc_search_profile2> dc = new List<doc_search_profile2>();
                if (doc_profile.Count() > 0 && doc_profile.FirstOrDefault().is_claim.Value)
                {

                    dc = getMobileDoctor(doc_profile);

                }

                if (dc.Count() > 0)
                {
                    var res = Newtonsoft.Json.JsonConvert.SerializeObject(dc);
                    var json = Newtonsoft.Json.Linq.JArray.Parse(res);

                    return Json(new { data = json, message = dc.Count() + " Record(s) found.", success = true });
                }
                else
                {
                    return Json(new { data = new string[] { }, message = "No record found.", success = false });
                }

                //return Json(new { });
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }

     
        }

        // date create: 1/3/2018
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("web/doctor/signup")]
        public IHttpActionResult Get_webdoctorsignup(post_webdoctorsignup param)
        {

            //ref_specialty1();

            // if there is a match, redirect user to profile screen
            // request parameter: email/account id, password
            // response: profile details

            // if email/account id does not exists
            // request parameter: npi no, password, image, practice name, phone no, list of specialties, address, bio, list of degrees, list of languages, list of insurances, practising year
            // response: profile details
            try
            {
                //IsRequired("NPI", param.npi, 2);
                //if (haserror)
                //{
                //    return Json(new { data = new string[] { }, message = errmsg, success = false });
                //}


                var doc = dbEntity.DOCTOR1.Where(a => a.NPI == param.npi);
                List<doc_search_profile2> dc = new List<doc_search_profile2>();

                foreach (var d in doc)
                {
                    dc.Add(new doc_search_profile2
                    {
                      id = d.id,
                      npi = d.NPI,
                      first_name = d.name_first,
                      last_name = d.name_last,
                      middle_name = d.name_middle,
                      DOB = d.dob,

                     //public string organization_name { get { return this._organization_name; } set { this._organization_name = value; } }

                    //public string gender { get { return this._gender; } set { this._gender = value; } }

                    //public string title { get { return this._title; } set { this._title = value; } }

                    //public string email { get { return this._email; } set { this._email = value; } }

                    //public string phone { get { return this._phone; } set { this._phone = value; } }

                    //public string license { get { return this._license; } set { this._license = value; } }

                    //public double doctor_fee { get { return this._doctor_fee; } set { this._doctor_fee = value; } }

                    //public int favorite { get { return this._favorite; } set { this._favorite = value; } }


                    //public string image_url { get { return this._image_url; } set { this._image_url = value; } }

                    //public string bio { get { return this._bio; } set { this._bio = value; } }

                    //private List<p_ratings_review> _p_review = new List<p_ratings_review>() { };
                    //public List<p_ratings_review> user_review { get { return this._p_review; } set { this._p_review = value; } }

   
                    //private string _pecos_certificate = "";
                    //public string pecos_certificate { get { return this._pecos_certificate; } set { this._pecos_certificate = value; } }
     
                    //private List<string> _time_slot = new List<string>() { };
                    //public List<string> time_slot { get { return this._time_slot; } set { this._time_slot = value; } }

                    //private List<string> _education = new List<string>() { };
                    //public List<string> education { get { return this._education; } set { this._education = value; } }

                    //private List<string> _experience = new List<string>() { };
                    //public List<string> experience { get { return this._experience; } set { this._experience = value; } }



                    //private List<doc_specialty2> _specialties = new List<doc_specialty2>() { };
                    //public List<doc_specialty2> specialties { get { return this._specialties; } set { this._specialties = value; } }

                    //private List<insurance> _network_insurance = new List<insurance>() { };
                    //public List<insurance> network_insurance { get { return this._network_insurance; } set { this._network_insurance = value; } }

                    //private List<doc_language> _language_spoken = new List<doc_language>() { };
                    //public List<doc_language> language_spoken { get { return this._language_spoken; } set { this._language_spoken = value; } }

                    //private List<appt_type> _appointment_type = new List<appt_type>() { };
                    //public List<appt_type> appointment_type { get { return this._appointment_type; } set { this._appointment_type = value; } }

                    //private List<zip_search_address> _home_address = new List<zip_search_address>() { };
                    //public List<zip_search_address> home_address { get { return this._home_address; } set { this._home_address = value; } }

                    //private List<zip_search_address> _practice_address = new List<zip_search_address>() { };
                    //public List<zip_search_address> practice_address { get { return this._practice_address; } set { this._practice_address = value; } }
                  });
                }


                // if (!string.IsNullOrEmpty(param.email)) param.email = param.email.ToLower();

                //var doc_profile = dbEntity.DOCTOR1.Where(a => a.email == param.email);

                //if (doc_profile.Count() > 0 && doc_profile.FirstOrDefault().is_claim.Value)
                //{

                // dc = getMobileDoctor(param);

                //}


                //return saveDoctor(param);


                var ret1 = JsonConvert.SerializeObject(dc);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                return Json(new { data = json1, message = "Record found.", success = true });
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }

        }

        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("web/doctor/signup")]
        public async Task<IHttpActionResult> post_webdoctorsignup()
        {
                post_webdoctorsignup param = new post_webdoctorsignup();
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string root = HttpContext.Current.Server.MapPath("~/Content/Temp/");
                var provider = new MultipartFormDataStreamProvider(root);
                try
                {
                    await Request.Content.ReadAsMultipartAsync(provider);
                    foreach (var key in provider.FormData.AllKeys)
                    {

                        foreach (var val in provider.FormData.GetValues(key))
                        {
                            switch (key )
                            {
                               case "npi":
                                IsRequired(key, val, 1);
                                param.npi = val;
                                break;

                               case "image_url":
                                param.image_url = val;
                                break;

                            case "practice_name":
                                param.practice_name = val;  break;
                            case "practice_phone":
                                param.practice_phone = val; break;
                            case "specialty_id":
                                param.specialty_id = val; break;
                            case "practice_address":
                                param.practice_address1 = val; break;
                            case "bio":
                                param.bio = val; break;
                            default:
                                return Json(new { message = "Object reference not set to an instance of an object. Invalid parameter name: " + key, success = false });
                          }
                       }
                    }


                    //List<doc_search_profile2> dc = new List<doc_search_profile2>();
                    return saveDoctor(param, provider);

                //var ret1 = JsonConvert.SerializeObject(resp);
                //var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                //return Json(new { data = new string[] { }, message = "Saved successfully.", success = true });
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success=false });
            }
      
        }
        private IHttpActionResult saveDoctor(post_webdoctorsignup doc, MultipartFileStreamProvider provider)
        {
            // 01/03/2018
            try {

                bool i = false;

               

                var d1 = dbEntity.DOCTOR1.Where(a => a.NPI == doc.npi);

                if (d1.Count() == 0)
                {
                    return Json(new { data = new string[] { }, message= "Record not found.", success = false });
                }
                var d = d1.FirstOrDefault();

                // upload image profile
                //string root = HttpContext.Current.Server.MapPath("~/Content/Temp/");
                //var provider = new MultipartFormDataStreamProvider(root);
                bool is_upload = uploadpic(provider, d.id.ToString());

                // DOCTOR save_doc = new DOCTOR();
               
                if (!string.IsNullOrEmpty(doc.npi))
                {
                   // d.NPI = doc.npi;
                }
             
                if (!string.IsNullOrEmpty(doc.first_name))
                {
                    d.name_first = doc.first_name;
                }

             
                if (!string.IsNullOrEmpty(doc.last_name))
                {
                    d.name_last = doc.last_name;
                }


                if (!string.IsNullOrEmpty(doc.middle_name))
                {
                    d.name_middle = doc.middle_name;
                }

                if (!string.IsNullOrEmpty(doc.title))
                {
                    d.title = doc.title;
                }
                //doc.dea = dea;

                //d.gender = doc.gender == null ? null : doc.gender[0].ToString().ToUpper();



                if (!string.IsNullOrEmpty(doc.home_address1))
                {
                    d.home_addr_1 = doc.home_address1;
                }
                if (!string.IsNullOrEmpty(doc.home_address2))
                {
                    d.home_addr_2 = doc.home_address2;
                }

                // check if there is zip value
                if (!string.IsNullOrEmpty(doc.home_zip))
                {
                    string z = doc.home_zip.Substring(0, 5);
                    var zip = dbEntity.ref_zip.Where(a => a.zip == z);
                    if (zip.Count() > 0)
                    {
                        d.home_addr_zip_id = zip.FirstOrDefault().id;
                    }
                }

                if (!string.IsNullOrEmpty(doc.practice_address1))
                {
                    d.practice_addr_1 = doc.practice_address1;
                }

                if (!string.IsNullOrEmpty(doc.practice_address2))
                {
                    d.practice_addr_2 = doc.practice_address2;
                }

                // check if the practice zip
                if (!string.IsNullOrEmpty(doc.practice_zip))
                {
                    string z = doc.home_zip.Substring(0, 5);
                    var zip = dbEntity.ref_zip.Where(a => a.zip == z);
                    if (zip.Count() > 0)
                    {
                        d.practice_addr_zip_id = zip.FirstOrDefault().id;

                    }
                }

                if (!string.IsNullOrEmpty(doc.practice_phone))
                {
                    d.practice_phone = doc.practice_phone;
                    //i = saveClaimDoctor_ext("practice_phone", "Practice Phone", doc.practice_phone, save_doc.id);
                }


                //if (!string.IsNullOrEmpty(doc.practice_fax))
                //{
                //    d.practice_fax = doc.practice_fax;
                //    //i = saveClaimDoctor_ext("practice_fax", "Practice Fax", doc.practice_fax, save_doc.id);
                //}
                if (!string.IsNullOrEmpty(doc.practice_name))
                {
                    d.practice_name = doc.practice_name;
                }

                if (!string.IsNullOrEmpty(doc.dob))
                {
                    d.dob = doc.dob;
                }


                if (!string.IsNullOrEmpty(doc.bio))
                {
                    d.bio = doc.bio;
                }

                if (!string.IsNullOrEmpty(new_filename))
                {
                    d.image_url = new_filename;
                }

                d.dt_create = dt;
                d.create_by__USER_id = 0;
                //doc.rel_ref_zip =
                dbEntity.Entry(d).State = EntityState.Modified;
                dbEntity.SaveChanges();

              

                // save specialty
                if (!string.IsNullOrEmpty(doc.specialty_id))
                {
                    string[] s = doc.specialty_id.Split(',');
                    foreach (var e in s)
                    {
                        long e1 = 0;
                        if (long.TryParse(e, out e1))
                        {
                            con_DOCTOR1_ref_specialty re_spec = new con_DOCTOR1_ref_specialty();
                            re_spec.rel_DOCTOR_id = d.id;
                            re_spec.rel_ref_specialty_id = e1;
                            re_spec.create_by__USER_id = 0;
                            re_spec.dt_create = dt;
                            dbEntity.con_DOCTOR1_ref_specialty.Add(re_spec);
                           
                        }
                    }
                    dbEntity.SaveChanges();
                }


                if (!string.IsNullOrEmpty(doc.exam_encounter))
                {
                    i = saveClaimDoctor_ext("exam_encounter", "Exam Encounter", doc.exam_encounter, d.id);
                }

                if (!string.IsNullOrEmpty(doc.clinician_role))
                {
                    i = saveClaimDoctor_ext("clinician_role", "Clinician Role", doc.exam_encounter, d.id);
                }

               


                if (!string.IsNullOrEmpty(doc.insurance_id))
                {
                    string[] ins = doc.insurance_id.Split(',');
                    foreach (var n in ins)
                    {
                        long ins_id_new = 0;
                        bool b_ins = long.TryParse(doc.insurance_id, out ins_id_new);

                        if (b_ins)
                        {
                            // i = saveClaimDoctor_ext("insurance_id", "Insurance ID", doc.insurance_id, save_doc.id);
                            con_DOCTOR1_ref_insurance ref_in = new con_DOCTOR1_ref_insurance();
                            ref_in.rel_DOCTOR_id = d.id;
                            ref_in.rel_ref_insurance_provider_id = ins_id_new;
                            ref_in.dt_create = dt;
                            ref_in.create_by__USER_id = 0;
                            dbEntity.con_DOCTOR1_ref_insurance.Add(ref_in);
                            
                        }
                    }
                    dbEntity.SaveChanges();

                    // else
                    // { return Json(new { data = new string[] { }, message = "Invalid insurance_id value.", success = false }); }
                }

                // =============== below is not final

                //if (!string.IsNullOrEmpty(doc.password))
                //{
                //    string salt = System.Guid.NewGuid().ToString();
                //    string encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(doc.password + salt));
                //    i = saveClaimDoctor_ext("password", "Password", encryp, save_doc.id);
                //    i = saveClaimDoctor_ext("salt", "Password Salt", salt, save_doc.id);
                //}


            

                //if (!string.IsNullOrEmpty(doc.education))
                //{
                //    string[] ed = doc.education.Split(',');
                //    foreach (var e in ed)
                //    {
                //        i = saveClaimDoctor_ext("education", "Education", e, d.id);
                //    }
                //}

                //if (!string.IsNullOrEmpty(doc.experience))
                //{
                //    string[] ex = doc.experience.Split(',');
                //    foreach (var e in ex)
                //    {
                //        i = saveClaimDoctor_ext("experience", "Experience", e, d.id);
                //    }
                    
                //}


               

                return Json(new { data = new string[] { }, message = "Save successfully.", success = true });
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success =false });
            }

         
        }



        private void ref_specialty1()
        {
            var ref_spec = from a in dbEntity.ref_specialty1 select a;

            foreach (var s in ref_spec)
            {
                // ref_specialty1 spec = new ref_specialty1();


                //s.level1_provider_type = s.level1_provider_type.Replace(" [definition]", "").Trim();
                //s.level2_classification = s.level2_classification.Replace(" [definition]", "").Trim();

                string sp = s.level2_classification.Split('-')[1];
                if (sp.Length > 0)
                {
                    s.level2_classification = s.level2_classification.Substring(0, s.level2_classification.Length - 13);
                    //s.level2_classification_code = s.level2_classification.Substring(s.level2_classification.Length - 10, 10);
                } else
                {
                    s.level2_classification = s.level2_classification.Substring(0, s.level2_classification.Length - 2);
                }

                if (!string.IsNullOrEmpty(s.level3_specialization))
                {
                    //s.level3_specialization = s.level3_specialization.Replace(" [definition]", "").Trim(); 
                    s.level3_specialization = s.level3_specialization.Substring(0, s.level3_specialization.Length - 13);
                    //s.level3_specialization_code = s.level3_specialization.Substring(s.level3_specialization.Length - 10, 10);
                }

                dbEntity.Entry(s).State = EntityState.Modified;
             
            }
            dbEntity.SaveChanges();
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("doctor/favorite")]
        public async Task<IHttpActionResult> favoritedoctor()
        {
            //if (!authorize) return Json(new { data = new string[] { }, message = "not authorized", success = false });

            string user_id = "", doctor_id = "", action = "";
            string msg = "";

            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);

            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var key in provider.FormData.AllKeys)
            {
                foreach (var val in provider.FormData.GetValues(key))
                {
                    switch (key)
                    {
                        case "user_id":
                            IsRequired(key, val, 1);
                            user_id = val;
                            break;

                        case "doctor_id":
                            IsRequired(key, val, 1);
                            doctor_id = val; break;
                        case "action":
                            // value: add/remove
                            action = val.ToLower(); break;

                        default:
                            msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                            return Json(new { data = new string[] { }, message = msg, success = false });
                    }

                    //string insurance_type = "", device_type = "", device_token = "", 

                }
            }

            IsRequired("user_id", user_id, 2);
            IsRequired("doctor_id", doctor_id, 2);
            if (haserror)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }

            long user_id_new = 0;
            bool bTemp = long.TryParse(user_id, out user_id_new);
            if (bTemp)
            {
                var vUser_id = dbEntity.USERs.Find(user_id_new);
                if (vUser_id == null)
                {
                    return Json(new { data = new string[] { }, message = "Invalid user_id value.", success = false });
                }

                long doctor_id_new = 0;
                bool bTemp1 = long.TryParse(doctor_id, out doctor_id_new);
                if (!bTemp1)
                {
                    return Json(new { data = new string[] { }, message = "Invalid doctor_id value.", success = false });
                }

                var vDoctor = dbEntity.DOCTORs.Find(doctor_id_new);
                if (vDoctor == null)
                {
                    return Json(new { data = new string[] { }, message = "Invalid doctor_id value.", success = false });
                }

                var confav = dbEntity.con_USER_favorite_DOCTOR.Where(a => a.create_by__USER_id == user_id_new && a.rel_doctor_id == doctor_id_new);

                if (action == "add")
                {
                    if (confav.Count() > 0)
                    {
                        return Json(new { data = new string[] { }, message = "Doctor already in the list.", success = false });
                    }

                    con_USER_favorite_DOCTOR con_fav = new con_USER_favorite_DOCTOR();
                    con_fav.rel_doctor_id = doctor_id_new;
                    con_fav.create_by__USER_id = user_id_new;
                    con_fav.favor = true;
                    con_fav.dt_create = dt;
                    //con_fav.favor = favorite == "" ? true : Convert.ToBoolean(favorite);
                    dbEntity.con_USER_favorite_DOCTOR.Add(con_fav);
                    dbEntity.SaveChanges();
                    msg = "User favorite DOCTOR is successfully saved.";
                }

                if (action == "remove")
                {
                    //var confav = dbEntity.con_USER_favorite_DOCTOR.Where(a => a.create_by__USER_id == user_id_new && a.rel_doctor_id == doctor_id_new);
                    if (confav.Count() > 0)
                    {
                        dbEntity.con_USER_favorite_DOCTOR.Remove(confav.FirstOrDefault());
                        //dbEntity.Entry(n).State = EntityState.Deleted;
                        dbEntity.SaveChanges();
                        msg = "Doctor is removed from the list.";
                        return Json(new { data = new string[] { }, message = msg, success = true });
                    }
                    else
                    {
                        return Json(new { data = new string[] { }, message = "Doctor is not in the list.", success = false });
                    }

                }


                return Json(new { data = new string[] { }, message = msg, success = true });
            }
            else // invalid user_id value
            {
                return Json(new { data = new string[] { }, message = "Invalid user_id value.", success = false });
            }
        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("doctor/favorite")]
        //public async Task<IHttpActionResult> getfavoritedoctor(string user_id = "")
        public IHttpActionResult getfavoritedoctor(string user_id = "")
        {
            //if (!authorize) return Json(new { data = new string[] { }, message = "not authorized", success = false });

            string doctor_id = "", favorite = "";
            string msg = "";

            //if (!authorize) return Json(new { data = new string[] { }, message = "not authorized", success = false });

            #region
            //string root = HttpContext.Current.Server.MapPath("~/Temp");
            //var provider = new MultipartFormDataStreamProvider(root);

            //await Request.Content.ReadAsMultipartAsync(provider);
            //foreach (var key in provider.FormData.AllKeys)
            //{
            //    foreach (var val in provider.FormData.GetValues(key))
            //    {
            //        switch (key)
            //        {
            //            case "user_id":
            //                IsRequired(key, val, 1);
            //                user_id = val;
            //                break;

            //            //case "doctor_id":
            //            //    IsRequired(key, val, 1);
            //            //    doctor_id = val; break;
            //            //case "favorite":
            //            //    favorite = val; break;

            //            default:
            //                msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
            //                return Json(new { data = new string[] { }, message = msg, success = false });
            //        }

            //        //string insurance_type = "", device_type = "", device_token = "", 

            //    }
            //}
            #endregion


            IsRequired("user_id", user_id, 2);
            // IsRequired("doctor_id", doctor_id, 2);
            if (haserror)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }

            long user_id_new = 0;
            bool bTemp = long.TryParse(user_id, out user_id_new);
            if (bTemp)
            {
                var vUser_id = dbEntity.USERs.Find(user_id_new);
                if (vUser_id == null)
                {
                    return Json(new { data = new string[] { }, message = "Invalid user_id value.", success = false });
                }

                //long doctor_id_new = 0;
                //bool bTemp1 = long.TryParse(doctor_id, out doctor_id_new);
                //if (!bTemp1)
                //{
                //    return Json(new { data = new string[] { }, message = "Invalid doctor_id value.", success = false });
                //}

                //var vDoctor = dbEntity.DOCTORs.Find(doctor_id_new);
                //if (vDoctor == null)
                //{
                //    return Json(new { data = new string[] { }, message = "Invalid doctor_id value.", success = false });
                //}

                var confav = dbEntity.con_USER_favorite_DOCTOR.Where(a => a.create_by__USER_id == user_id_new);

                List<doc_search_profile> dc = new List<doc_search_profile>();
                if (confav.Count() > 0)
                {


                    foreach (var doc in confav)
                    {
                        int fave_doc = 0;

                        if (doc.favor == true)
                        {
                            fave_doc = 1;

                            var doc_info = doc.hs_DOCTOR;
                            var addr = dbEntity.ref_zip.Find(doc_info.home_addr_zip_id);

                            double doc_id = doc.rel_doctor_id;

                            #region "get doctor_rating"

                            // start: get average doctor rating
                            double ave_rating = 0;
                            var appt_docid = dbEntity.APPOINTMENT_ext.Where(a => a.attr_name == "doctor_id" && a.value == doc_id.ToString());
                            if (appt_docid.Count() > 0)
                            {
                                int cnt = 0; double doctor_rate = 0;
                                foreach (var n in appt_docid)
                                {

                                    var appt_docrate = dbEntity.APPOINTMENT_ext.Where(a => a.attr_name == "doctor_rating" && a.rel_APPOINTMENT_id == n.rel_APPOINTMENT_id);
                                    if (appt_docrate.Count() > 0)
                                    {
                                        foreach (var d in appt_docrate)
                                        {
                                            cnt++;
                                            double dRate = 0;
                                            bool temp = double.TryParse(d.value, out dRate);
                                            if (temp) doctor_rate += dRate;
                                        }


                                    }
                                }
                                if (cnt > 0)
                                {
                                    ave_rating = doctor_rate / cnt;
                                }

                            }
                            // end: get average doctor rating
                            #endregion

                            #region "get zip_address"
                            //string addr2 = doc_info.addr_address2 == null ? "" : doc_info.addr_address2;
                            zip_search_address address = new zip_search_address
                            {
                                //id = addr.id,
                                address1 = doc_info.home_addr_1 == null ? "" : doc_info.home_addr_1,
                                address2 = doc_info.home_addr_2 == null ? "" : doc_info.home_addr_2,
                                zip = addr.zip,
                                city = addr.city_name,
                                state = addr.city_state,
                                state_long = addr.city_state_long,
                                lat = addr.city_lat,
                                lng = addr.city_lon,
                                county = addr.city_county
                            };
                            #endregion

                            // get SPECIALTY
                            List<doc_specialty> spec = new List<doc_specialty>();
                            #region "get specialty"
                            var sp = (from a in dbEntity.con_DOCTOR_ref_specialty
                                      join b in dbEntity.ref_specialty on a.rel_ref_specialty_id equals b.id
                                      where a.rel_DOCTOR_id == doc_id
                                      select b).Distinct().ToList();
                            //var sp = dbEntity.con_DOCTOR_ref_specialty.Where(a => a.rel_DOCTOR_id == doc_id);

                            foreach (var i in sp)
                            {

                                spec.Add(new doc_specialty
                                {
                                    id = i.id,
                                    name = i.name, 
                                    provider_type = "",
                                    specialization = ""
                                    //description = i.description == null ? "" : i.description,
                                    //actor = i.actor == null ? "" : i.actor
                                });
                            }
                            #endregion

                            #region "get doctor_ext"
                            // get: doctor_ext
                            var doc_ext = doc_info.hs_DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doc_id);
                            List<appt_type> appt = new List<appt_type>();
                            double doc_fee = 0;
                            string vtime_slot = "";
                            foreach (var n in doc_ext)
                            {
                                switch (n.attr_name)
                                {
                                    case "fee":
                                        bool bTemp2 = double.TryParse(n.value, out doc_fee);
                                        break;
                                    case "drappttype":
                                        string vappt_name = "";
                                        long vappt_id = 0;
                                        bool isAppt = long.TryParse(n.value, out vappt_id);

                                        if (vappt_id > 0)
                                        {
                                            var n1 = dbEntity.ref_APPOINTMENT_type.Find(vappt_id);
                                            vappt_name = n1.dname;

                                            appt.Add(new appt_type
                                            {
                                                id = vappt_id,
                                                type = vappt_name
                                            });
                                        }
                                        break;

                                    case "time_slot":
                                        vtime_slot = n.value == null ? "" : n.value;
                                        break;
                                }


                            }
                            // end: doctor_ext
                            #endregion

                            #region "doc profile array"
                            string prof_addr = doc_info.home_addr_1 + (doc_info.home_addr_1 == null ? "" : " " + doc_info.home_addr_2);

                            var prof = new Models.doc_search_profile
                            {
                                id = doc.rel_doctor_id,
                                first_name = doc_info.name_first,
                                last_name = doc_info.name_last,
                                middle_name = doc_info.name_middle == null ? "" : doc_info.name_middle,
                                gender = doc_info.gender == null ? "" : doc_info.gender.Trim().ToUpper(),
                                title = doc_info.title == null ? "" : doc_info.title,

                                organization_name = doc_info.organization_name == null ? "" : doc_info.organization_name,
                                email = doc_info.email == null ? "" : doc_info.email,
                                phone = doc_info.phone == null ? "" : doc_info.phone,
                                license = doc_info.license_no == null ? "" : doc_info.license_no,
                                npi = doc_info.NPI == null ? "" : doc_info.NPI,
                                //orgname = doc_info.org_name == null ? "" : doc_info.org_name,
                                image_url = doc_info.image_url == null ? "" : doc_info.image_url,
                                rating = ave_rating,
                                favorite = fave_doc,
                                doctor_fee = doc_fee,
                                time_slot = vtime_slot,

                                address = address,
                                //address = address,
                                //address2 = doc.addr_address2,
                                pecos_certificate = doc_info.pecos_certification == null ? "" : doc_info.pecos_certification,
                                //faxto = doc.fax_to,
                                //state = addr != null ? addr.city_state : "",
                                //city = addr != null ? addr.city_name : "",
                                //city_lat = addr != null ? addr.city_lat : 0,
                                //city_long = addr != null ? addr.city_lon : 0,
                                //zipcode = addr != null ? addr.zip : "",
                                bio = doc_info.bio == null ? "" : doc_info.bio,
                                specialties = spec,
                                appointment_type = appt

                            };

                            dc.Add(prof);
                            #endregion
                        }



                    }
                }

                var ret1 = JsonConvert.SerializeObject(dc);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                msg = "";
                if (dc.Count() > 0)
                {
                    msg = dc.Count() + " favorite doctor(s) found.";
                    return Json(new { data = json1, message = msg, success = true });
                }
                else
                {
                    msg = "No favorite doctor(s) found.";
                    return Json(new { data = json1, message = msg, success = false });
                }

            }
            else // invalid user_id value
            {
                return Json(new { data = new string[] { }, message = "Invalid user_id value.", success = false });
            }

        }


        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("doctor/favorite")]
        public async Task<IHttpActionResult> delfavoritedoctor()
        {
            //if (!authorize) return Json(new { data = new string[] { }, message = "not authorized", success = false });

            string user_id = "", doctor_id = "", favorite = "";
            string msg = "";

            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);

            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var key in provider.FormData.AllKeys)
            {
                foreach (var val in provider.FormData.GetValues(key))
                {
                    switch (key)
                    {
                        case "user_id":
                            IsRequired(key, val, 1);
                            user_id = val;
                            break;

                        case "doctor_id":
                            IsRequired(key, val, 1);
                            doctor_id = val; break;
                        case "favorite":
                            favorite = val; break;

                        default:
                            msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                            return Json(new { data = new string[] { }, message = msg, success = false });
                    }

                    //string insurance_type = "", device_type = "", device_token = "", 

                }
            }

            IsRequired("user_id", user_id, 2);
            IsRequired("doctor_id", doctor_id, 2);
            if (haserror)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }

            long user_id_new = 0;
            bool bTemp = long.TryParse(user_id, out user_id_new);
            if (bTemp)
            {
                var vUser_id = dbEntity.USERs.Find(user_id_new);
                if (vUser_id == null)
                {
                    return Json(new { data = new string[] { }, message = "Invalid user_id value.", success = false });
                }

                long doctor_id_new = 0;
                bool bTemp1 = long.TryParse(doctor_id, out doctor_id_new);
                if (!bTemp1)
                {
                    return Json(new { data = new string[] { }, message = "Invalid doctor_id value.", success = false });
                }

                var vDoctor = dbEntity.DOCTORs.Find(doctor_id_new);
                if (vDoctor == null)
                {
                    return Json(new { data = new string[] { }, message = "Invalid doctor_id value.", success = false });
                }

                var confav = dbEntity.con_USER_favorite_DOCTOR.Where(a => a.create_by__USER_id == user_id_new && a.rel_doctor_id == doctor_id_new);
                if (confav.Count() > 0)
                {

                    dbEntity.con_USER_favorite_DOCTOR.Remove(confav.FirstOrDefault());
                    //dbEntity.Entry(n).State = EntityState.Deleted;
                    dbEntity.SaveChanges();

                    return Json(new { data = new string[] { }, message = "User favorite DOCTOR is removed from the list.", success = true });
                }

                // doctor is not found on the User's fav list
                return Json(new { data = new string[] { }, message = "Invalid doctor_id value.", success = false });
            }
            else // invalid user_id value
            {
                return Json(new { data = new string[] { }, message = "Invalid user_id value.", success = false });
            }
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("doctor/appointment/history")]
        public IHttpActionResult getappointmenthistory(string doctor_id = null)
        {
            //if (!authorize) return Json(new { data = new string[] { }, message = "not authorized", success = false });

            IsRequired("doctor_id", doctor_id, 2);
            if (haserror)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }

            try {
                if (Request.RequestUri.ToString().Contains("localhost"))
                {
                    var ref_appt_stat = dbEntity.ref_APPOINTMENT_status.Where(a => a.dname.ToLower() == "history").FirstOrDefault();
                    // 3 History

                    var ref_stat = dbEntity.ref_status.Where(a => a.ref_status_type.dname.ToLower() == "appointment").FirstOrDefault();
                    // 10 Active // 11 Inactive
                    // 12 Deleted
                }

                var appt_ext = dbEntity.APPOINTMENT_ext.Where(a => a.APPOINTMENT.rel_ref_APPOINTMENT_status_id == 3 && a.attr_name == "doctor_id" && a.value == doctor_id);
                //ex. appointment_id:521, sould: 282, doctor_id:1

                List<doctor_appointment_history> doc_history = new List<doctor_appointment_history>();
                foreach (var i in appt_ext)
                {
                    //var appt = dbEntity.APPOINTMENTs.Find(i.rel_APPOINTMENT_id);
                    List<d_patient> pat = new List<d_patient>();
                    var _s = dbEntity.SOULs.Find(i.APPOINTMENT.rel_SOUL_id);
                    var appt_type = dbEntity.ref_APPOINTMENT_type.Find(i.APPOINTMENT.rel_ref_APPOINTMENT_type_id);

                    List<a_type> ap_type = new List<a_type>();
                    ap_type.Add(new a_type { id = appt_type.id, name = appt_type.dname });

                    pat.Add(new d_patient {
                        id = _s.id,
                        firstname = _s.name_first,
                        lastname = _s.name_last,
                        appointment_type = ap_type
                    });

                    //get soul_, appointment_type, appointment_schedule
                    doc_history.Add(new doctor_appointment_history {
                        id = i.rel_APPOINTMENT_id.Value,
                        patient = pat
                    });

                }


                var ret1 = JsonConvert.SerializeObject(doc_history);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);


                return Json(new { data = json1, message = doc_history.Count() + " record(s) found.", success = true });
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("wr/doctor/availability")]
        public async Task<IHttpActionResult> wrpostavailability()
        {
            //if (!authorize) return Json(new { data = new string[] { }, message = "not authorized", success = false });

            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);

            string doctor_id = null, date = null;
            var time_slot = new List<string>();
            int n = 0;

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        // user_id, patient_id, rate
                        switch (key)
                        {
                            case "doctor_id":
                                IsRequired(key, val, 1);
                                //bTemp = long.TryParse(val, out doctor_id);
                                doctor_id = val;
                                break;

                            case "date":
                                IsRequired(key, val, 1);
                                //bTemp = int.TryParse(val, out rate);
                                date = val;
                                break;

                            case "time_slot":
                                IsRequired(key, val, 1);
                                //bTemp = int.TryParse(val, out rate);

                                //time_slot.Add(val);
                                n++;
                                break;

                            default:
                                return Json(new { message = "Invalid parameter name: " + key, success = false });
                        }
                    }
                }

                IsRequired("patient_id", doctor_id.ToString(), 2);
                IsRequired("date", date.ToString(), 2);
                IsRequired("time_slot", time_slot.ToString(), 2);
                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }

                long doc_id_new = 0;
                bool isDoc = long.TryParse(doctor_id, out doc_id_new);
                if (!isDoc) { return Json(new { data = new string[] { }, message = "Invalid doctor_id value.", success = false }); }


                foreach (var i in time_slot) {
                    DOCTOR_ext doc_ext = new DOCTOR_ext
                    {
                        rel_DOCTOR_id = doc_id_new,
                        attr_name = "time_slot",
                        dname = "Time Slot",
                        value = date.ToString() + "|" + i
                    };

                    dbEntity.DOCTOR_ext.Add(doc_ext);

                }
                dbEntity.SaveChanges();



                //var ret1 = JsonConvert.SerializeObject(doc_history);
                //var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                //return Json(new { data = json1, message = doc_history.Count() + " record(s) found.", success = true });

                return Json(new { data = new string[] { }, message = "Doctor availability was set successfully.", success = true });
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("doctor/availability")]
        public IHttpActionResult postavailability([FromBody] doc doctor_id1)
        {

            //if (!authorize) return Json(new { data = new string[] { }, message = "not authorized", success = false });

            string doctor_id = doctor_id1.doctor_id;
            string date = doctor_id1.date;
            string time_slot = doctor_id1.time_slot == null ? null : doctor_id1.time_slot[0];

            IsRequired("doctor_id", doctor_id1.doctor_id, 2);
            IsRequired("date", date, 2);
            IsRequired("time_slot", time_slot, 2);
            if (haserror)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }



            long doc_id_new = 0;
            bool isDoc = long.TryParse(doctor_id, out doc_id_new);
            if (!isDoc) { return Json(new { data = new string[] { }, message = "Invalid doctor_id value.", success = false }); }


            foreach (var i in doctor_id1.time_slot)
            {
                DOCTOR_ext doc_ext = new DOCTOR_ext
                {
                    rel_DOCTOR_id = doc_id_new,
                    attr_name = "time_slot",
                    dname = "Time Slot",
                    value = date.ToString() + "|" + i
                };

                dbEntity.DOCTOR_ext.Add(doc_ext);

            }
            dbEntity.SaveChanges();

            return Json(new { message = "Time availability was set successfully.", success = true });
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("doctor/practice")]
        public IHttpActionResult getPractice(string npi = null) {
            // this is old
            //if (!authorize) return Json(new { data = new string[] { }, message = "not authorized", success = false });

            IsRequired("npi", npi, 2);
            if (haserror)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }

            var doc = dbEntity.hs_DOCTOR.Where(a => a.NPI == npi);



            List<Models.doc_search_profile> doc_prof = new List<Models.doc_search_profile>();
            if (doc.Count() > 0)
            {
                foreach (var d in doc) {

                    // get: doctor_ext
                    var doc_ext = dbEntity.DOCTOR_ext.Where(a => a.rel_DOCTOR_id == d.id);
                    List<appt_type> appt = new List<appt_type>();
                    double doc_fee = 0;
                    string vtime_slot = "";
                    foreach (var n in doc_ext)
                    {
                        switch (n.attr_name)
                        {
                            case "fee":
                                bool bTemp = double.TryParse(n.value, out doc_fee);
                                break;
                            case "drappttype":
                                string vappt_name = "";
                                long vappt_id = 0;
                                bool isAppt = long.TryParse(n.value, out vappt_id);

                                if (vappt_id > 0)
                                {
                                    var n1 = dbEntity.ref_APPOINTMENT_type.Find(vappt_id);
                                    vappt_name = n1.dname;

                                    appt.Add(new appt_type
                                    {
                                        id = vappt_id,
                                        type = vappt_name
                                    });
                                }
                                break;

                            case "time_slot":
                                vtime_slot = n.value == null ? "" : n.value;
                                break;
                        }


                    }
                    // end: doctor_ext

                    //// start: get average doctor rating
                    //double ave_rating = 0;
                    //var appt_docid = dbEntity.APPOINTMENT_ext.Where(a => a.attr_name == "doctor_id" && a.value == d.id.ToString());

                    //// get doctor is in con_USER_favorite_DOCTOR
                    ////9/7 if (user_id > 0)
                    ////{
                    ////    appt_docid = appt_docid.Where(a => a.create_by__USER_id == user_id);
                    ////}
                    //if (appt_docid.Count() > 0)
                    //{

                    //    int cnt = 0; double doctor_rate = 0;
                    //    foreach (var n in appt_docid)
                    //    {

                    //        var appt_docrate = dbEntity.APPOINTMENT_ext.Where(a => a.attr_name == "doctor_rating" && a.rel_APPOINTMENT_id == n.rel_APPOINTMENT_id);
                    //        // get doctor is in con_USER_favorite_DOCTOR
                    //        if (faveList)
                    //        {
                    //            appt_docrate = appt_docrate.Where(a => a.create_by__USER_id == user_id);
                    //        }

                    //        if (appt_docrate.Count() > 0)
                    //        {
                    //            foreach (var d in appt_docrate)
                    //            {
                    //                cnt++;
                    //                double dRate = 0;
                    //                bool temp = double.TryParse(d.value, out dRate);
                    //                if (temp) doctor_rate += dRate;
                    //            }


                    //        }
                    //    }
                    //    if (cnt > 0)
                    //    {
                    //        ave_rating = doctor_rate / cnt;
                    //    }

                    //    // asdfasfsdfsd

                    //    // asdfasfsdfsd

                    //}
                    //// end: get average doctor rating
                    var addr = dbEntity.ref_zip.Find(d.home_addr_zip_id);
                    //string addr2 = d.addr_address2 == null ? "" : d.addr_address2;
                    zip_search_address address = new zip_search_address
                    {
                        //id = addr.id,
                        address1 = d.home_addr_1 == null ? "" : d.home_addr_1,
                        address2 = d.home_addr_2 == null ? "" : d.home_addr_2,
                        zip = addr.zip,
                        city = addr.city_name,
                        state = addr.city_state,
                        state_long = addr.city_state_long,
                        lat = addr.city_lat,
                        lng = addr.city_lon,
                        county = addr.city_county
                    };

                    // get SPECIALTY
                    List<doc_specialty> spec = new List<doc_specialty>();
                    var sp = (from a in dbEntity.con_DOCTOR_ref_specialty
                              join b in dbEntity.ref_specialty on a.rel_ref_specialty_id equals b.id
                              where a.rel_DOCTOR_id == d.id
                              select b).Distinct().ToList();



                    foreach (var i in sp)
                    {
                        spec.Add(new doc_specialty
                        {
                            id = i.id,
                            name = i.name
                            //description = i.description == null ? "" : i.description,
                            //actor = i.actor == null ? "" : i.actor
                        });
                    }

                    doc_prof.Add(new Models.doc_search_profile
                    {
                        id = d.id,
                        first_name = d.name_first,
                        last_name = d.name_last,
                        middle_name = d.name_middle == null ? "" : d.name_middle,
                        email = d.email == null ? "" : d.email,
                        gender = d.gender == null ? "" : d.gender.Trim().ToUpper(),
                        title = d.title == null ? "" : d.title,
                        phone = d.phone == null ? "" : d.phone,
                        license = d.license_no == null ? "" : d.license_no,
                        npi = d.NPI == null ? "" : d.NPI,
                        organization_name = d.organization_name == null ? "" : d.organization_name,
                        image_url = d.image_url == null ? "" : d.image_url,
                        //rating = ave_rating,
                        //doctor_fee = doc_fee,
                        //favorite = fave_doc,
                        time_slot = vtime_slot,

                        bio = d.bio == null ? "" : d.bio,
                        //PECOS (Provider Enrollment and Chain/Ownership System)
                        pecos_certificate = d.pecos_certification == null ? "" : d.pecos_certification,
                        specialties = spec,
                        appointment_type = appt,
                        address = address
                    });

                }

                if (doc_prof.Count() > 0)
                {
                    var res = Newtonsoft.Json.JsonConvert.SerializeObject(doc_prof);
                    var json = Newtonsoft.Json.Linq.JArray.Parse(res);

                    return Json(new { data = json, message = doc_prof.Count() + " Record(s) found.", success = true });
                }
                else
                {
                    return Json(new { data = new string[] { }, message = "No record found.", success = false });
                }
            }

            return Json(new { data = new string[] { }, message = "No record found.", success = false });

        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("wr/doctor/claim")] // doctor/claim
        public async Task<IHttpActionResult> wrpostClaimPractice()
        {
            //if (!authorize) return Json(new { data = new string[] { }, message = "not authorized", success = false });


            //string a_pass = httpCon.Request.Headers["Authorization"];
            //bool b_pass = Validation.userAuth(a_pass);

            DateTime birthdate = new DateTime();
            // double height = 0, weight = 0;
            doctor_claim doc = new doctor_claim();

            //DateTime dtNow = DateTime.Now;
            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);
            string msg = "";

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {

                        switch (key)
                        {
                            case "npi":
                                //npi = val;
                                IsRequired(key, val, 1);
                                doc.npi = val;

                                break;

                            case "first_name":
                                //first_name = val;
                                IsRequired(key, val, 1);
                                doc.first_name = val;
                                break;

                            case "middle_name":
                                // middle_name = val;
                                doc.middle_name = val;
                                break;
                            case "last_name":
                                // last_name = val;
                                IsRequired(key, val, 1);
                                doc.last_name = val;
                                break;

                            case "title":
                                //title = val;
                                doc.title = val;
                                break;

                            case "email":
                                //email = val;
                                IsRequired(key, val, 1);
                                doc.email = val;
                                break;

                            case "personal_practice_type":
                                //email = val;
                                //IsRequired(key, val, 1);
                                doc.personal_practice_type = val;
                                break;

                            // below values will go to SOUL_ext
                            case "dea":
                                //dea = val;
                                doc.dea = val;
                                break;


                            case "dob":
                                bool isBool = DateTime.TryParse(val, out birthdate);
                                IsRequired(key, val, 1);

                                if (isBool)
                                {
                                    doc.dob = birthdate.ToShortDateString();
                                }
                                break;
                            case "clinician_role": // 
                                                   // clinician_role = val;
                                IsRequired(key, val, 1);
                                doc.clinician_role = val;
                                break;
                            case "exam_encounter":
                                // exam_encounter = val;
                                IsRequired(key, val, 1);
                                doc.exam_encounter = val;
                                break;

                            case "gender": // gender = val;
                                IsRequired(key, val, 1);
                                doc.gender = val;
                                break;

                            //case "parent_guardian":
                            //    parent_guardian = val; break;

                            case "home_street1": // street1 = val; 
                                doc.home_street1 = val; break;

                            case "home_street2": // street2 = val; 
                                doc.home_street2 = val;
                                break;

                            case "home_city": //city = val;
                                IsRequired(key, val, 1);
                                doc.home_city = val;
                                break;

                            case "home_state": // state = val; 
                                IsRequired(key, val, 1);
                                doc.home_state = val;
                                break;

                            case "home_zip": // zip_code = val; 
                                doc.home_zip = val;
                                break;

                            case "practice_name": // practice_name = val; 
                                doc.practice_name = val;
                                break;

                            case "practice_phone": // practice_phone = val;
                                IsRequired(key, val, 1);
                                doc.practice_phone = val;
                                break;

                            case "practice_street1":
                                //bool b = double.TryParse(val, out height);
                                doc.practice_street1 = val;
                                break;
                            case "practice_street2":
                                //bool b1 = double.TryParse(val, out weight);
                                doc.practice_street2 = val;
                                break;

                            case "practice_city":
                                // practice_city = val;
                                IsRequired(key, val, 1);
                                doc.practice_city = val;
                                break;

                            case "practice_state":
                                // practice_state = val;
                                doc.practice_state = val;
                                break;

                            case "practice_zip":
                                // practice_zip = val;
                                IsRequired(key, val, 1);
                                doc.practice_zip = val;
                                break;

                            case "practice_fax":
                                // practice_fax = val;
                                IsRequired(key, val, 1);
                                doc.practice_fax = val;
                                break;

                            case "insurance_id":
                                // insurance_id = val;
                                long res = 0;
                                bool b = long.TryParse(val, out res);
                                if (b) doc.insurance_id = val;
                                break;

                            case "education":
                                doc.education = val;
                                break;
                            case "experience":
                                doc.experience = val;
                                break;

                            //case "note":
                            //    note = val; break;
                            default:
                                msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                                return Json(new { data = new string[] { }, message = msg, success = false });
                        }
                    }
                }

                IsRequired("npi", doc.npi, 2);
                IsRequired("first_name", doc.first_name, 2);
                IsRequired("last_name", doc.last_name, 2);
                IsRequired("dob", doc.dob, 2);
                IsRequired("clinician_role", doc.clinician_role, 2);
                IsRequired("exam_encounter", doc.exam_encounter, 2);
                IsRequired("gender", doc.gender, 2);
                IsRequired("email", doc.email, 2);

                IsRequired("home_city", doc.home_city, 2);
                IsRequired("home_state", doc.home_state, 2);

                IsRequired("practice_city", doc.practice_city, 2);
                IsRequired("practice_zip", doc.practice_zip, 2);
                IsRequired("practice_fax", doc.practice_fax, 2);
                IsRequired("practice_phone", doc.practice_phone, 2);
                IsRequired("insurance_id", doc.insurance_id, 2);

                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }

                return saveDoctorClaim(doc);
            }
            catch (Exception ex) {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }

        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("doctor/claim")] // doctor/claim
        public async Task<IHttpActionResult> postclaimPractice(doctor_claim doc)
        {

            IsRequired("npi", doc.npi, 2);
            IsRequired("first_name", doc.first_name, 2);
            IsRequired("last_name", doc.last_name, 2);
            IsRequired("dob", doc.dob, 2);
            IsRequired("clinician_role", doc.clinician_role, 2);
            IsRequired("exam_encounter", doc.exam_encounter, 2);
            IsRequired("gender", doc.gender, 2);
            IsRequired("email", doc.email, 2);
            IsRequired("password", doc.password, 2);

            IsRequired("home_city", doc.home_city, 2);
            IsRequired("home_state", doc.home_state, 2);

            IsRequired("practice_city", doc.practice_city, 2);
            IsRequired("practice_zip", doc.practice_zip, 2);
            IsRequired("practice_fax", doc.practice_fax, 2);
            IsRequired("practice_phone", doc.practice_phone, 2);
            IsRequired("insurance_id", doc.insurance_id, 2);

            if (haserror)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }

            return saveDoctorClaim(doc);
        }

      
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("doctor/profile")]
        public async Task<IHttpActionResult> postDoctorProfile(post_doctor_profile profile)
        {
            // doctor signup
            //if (!authorize) return Json(new { data = new string[] { }, message = "not authorized", success = false });

            return saveDoctorProfile(profile);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("wr/doctor/profile")] // removed: wr/
        //public async Task<IHttpActionResult> wrpostDoctorProfile()
        public IHttpActionResult wrpostDoctorProfile(post_doctor_profile profile)
        {

            // string root = HttpContext.Current.Server.MapPath("~/Temp");
            // var provider = new MultipartFormDataStreamProvider(root);
            string msg = "";
            // post_doctor_profile profile = new post_doctor_profile();

            //if (!authorize) return Json(new { data = new string[] { }, message = "not authorized", success = false });

            try
            {
                #region "old_param"
                //await Request.Content.ReadAsMultipartAsync(provider);
                //foreach (var key in provider.FormData.AllKeys)
                //{
                //    foreach (var val in provider.FormData.GetValues(key))
                //    {

                //        switch (key)
                //        {
                //            case "npi":
                //                profile.npi = val;
                //                break;
                //            case "first_name":
                //                profile.first_name = val; 
                //                break;
                //            case "last_name":
                //                profile.last_name = val;
                //                break;
                //            case "middle_name":
                //                profile.middle_name = val;
                //                break;
                //            case "title":
                //                profile.title = val;
                //                break;
                //            case "dob":
                //                profile.dob = val;
                //                break;
                //            case "gender":
                //                profile.gender = val;
                //                break;
                //            case "email":
                //                profile.email = val;
                //                break;
                //            case "personal_practice_type":
                //                profile.personal_practice_type = val;
                //                //// public string practice_type { get; set; }
                //                break;
                //            case "home_street1":
                //                profile.home_street1 = val;
                //                break;
                //            case "home_street2":
                //                profile.home_street2 = val;
                //                break;
                //            case "home_city":
                //                profile.home_city = val;
                //                break;
                //            case "home_state":
                //                profile.home_state = val; 
                //                break;
                //            case "home_zip":
                //                profile.home_zip = val;

                //                break;
                //            case "education":
                //                profile.education = val;
                //                break;
                //            case "language_spoken":
                //                profile.language_spoken = val;
                //                break;
                //            case "board_certification":
                //                profile.board_certification = val;
                //                break;
                //            case "specialty":
                //                profile.specialty = val;
                //                break;
                //            case "practice_npi":
                //                profile.practice_npi = val;
                //                break;
                //            case "practice_name":
                //                profile.practice_name = val;
                //                break;
                //            case "practice_type":
                //                profile.practice_type = val;
                //                break;
                //            case "dea":
                //                profile.dea = val;
                //                break;
                //            case "clinician_role":
                //                profile.clinician_role = val;
                //                break;
                //            case "scheduling_solution":
                //                profile.scheduling_solution = val;
                //                break;
                //            case "current_scheduling":
                //                profile.current_scheduling = val;
                //                break;
                //            case "practice_street":
                //                profile.practice_street = val;
                //                break;
                //            case "practice_zip":
                //                profile.practice_zip = val;

                //                break;
                //            case "practice_phone_primary":
                //                profile.practice_phone_primary = val; // primary phone number
                //                break;
                //            case "practice_fax":
                //                profile.practice_fax = val;
                //                break;
                //            case "practice_phone_cs":
                //                profile.practice_phone_cs = val; // customer service number
                //                break;
                //            case "practice_phone_office":
                //                profile.practice_phone_office = val; // office phone
                //                break;
                //            case "practice_clinicians":
                //                profile.practice_clinicians = val; // no of field clinicians
                //                break;
                //            case "practice_exams":
                //                profile.practice_exams = val; // no of exams you can handle per week
                //                break;

                //            case "geographic_market":
                //                profile.geographic_market = val; // geographic market
                //                break;
                //            case "practice_expansion":
                //                profile.practice_expansion = val; // future expansion plans, new market, 
                //                break;
                //            case "practice_insurance":
                //                profile.practice_insurance = val; // insurance list you are in Network or will acccept
                //                break;
                //            case "practice_tax_number":
                //                profile.practice_tax_number = val; // federal tax id number
                //                break;
                //            case "practice_emr":
                //                profile.practice_emr = val; // emr software that you are currently using
                //                break;
                //            case "network_insurance":
                //                profile.network_insurance = val; // in-network insurances
                //                break;

                //            // PRIMARY CONTACT
                //            case "primary_contact_name":
                //                profile.primary_contact_name = val; // primary contact/ operational contact/ financial contact
                //                break;
                //            case "primary_contact_email":
                //                profile.primary_contact_email = val; // primary contact/ operational contact/ financial contact
                //                break;
                //            case "primary_contact_phone":
                //                profile.primary_contact_phone = val; // primary contact/ operational contact/ financial contact
                //                break;

                //                // OPERATIONAL CONTACT
                //            case "operational_contact_name":
                //                profile.operational_contact_name = val; // primary contact/ operational contact/ financial contact
                //                break;
                //            case "operational_contact_email":
                //                profile.operational_contact_email = val; // primary contact/ operational contact/ financial contact
                //                break;
                //            case "operational_contact_phone":
                //                profile.operational_contact_phone = val; // primary contact/ operational contact/ financial contact
                //                break;

                //                // FINANCIAL CONTACT
                //            case "financial_contact_name":
                //                profile.financial_contact_name = val; // primary contact/ operational contact/ financial contact
                //                break;
                //            case "financial_contact_email":
                //                profile.financial_contact_email = val; // primary contact/ operational contact/ financial contact
                //                break;
                //            case "financial_contact_phone":
                //                profile.financial_contact_phone = val; // primary contact/ operational contact/ financial contact
                //                break;

                //            case "billing_bankname":

                //                profile.billing_bankname = val;
                //                break;
                //            case "billing_account":
                //                profile.billing_account = val;
                //                break;
                //            case "billing_routing":
                //                profile.billing_routing = val;
                //                break;

                //            case "password":
                //                profile.password = val;
                //                break;
                //            case "status":
                //                profile.status = val;
                //                break;
                //            case "is_reviewed":
                //                profile.is_reviewed = val;
                //                break;

                //            default:
                //                msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                //                return Json(new { data = new string[] { }, message = msg, success = false });
                //        }
                //    }
                //}
                #endregion

                //IsRequired("npi", profile.npi, 2);
                //IsRequired("firt_name", profile.first_name, 2);
                //IsRequired("last_name", profile.last_name, 2);
                //if (haserror)
                //{
                //    return Json(new { data = new string[] { }, message = errmsg, success = false });
                //}

                // System.Web.HttpContext httpCOn = System.Web.HttpContext.Current;
                //string v = httpCon.Request.Headers["Authorization"];

                return saveDoctorProfile(profile);
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }

        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("wr/doctor/profile")]
        public IHttpActionResult wrputDoctorProfile(post_doctor_profile prof)
        {

            //if (!authorize) return Json(new { data = new string[] { }, message = "not authorized", success = false });

            IsRequired("npi", prof.npi, 2);
            // IsRequired("first_name", prof.first_name, 2);
            // IsRequired("last_name", prof.last_name, 2);
            // IsRequired("email", prof.email, 2);
            // password shoud be required if there is an email
            if (haserror)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }

            var doc_rec = dbEntity.hs_DOCTOR.Where(a => a.NPI == prof.npi);

            if (doc_rec.Count() > 0)
            {
                // do update
                return _updateDoctor(doc_rec.FirstOrDefault(), prof);
            }

            return Json(new { data = new string[] { }, message = "No record found.", success = false });
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("mk/doctor/profile")]
        public async Task<IHttpActionResult> mkpostDoctorProfile()
        //public IHttpActionResult wrpostDoctorProfile(post_doctor_profile profile)
        {
            //if (!authorize) return Json(new { data = new string[] { }, message = "not authorized", success = false });

            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);
            string msg = "";
            post_doctor_profile profile = new post_doctor_profile();

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {

                        switch (key)
                        {
                            case "npi":
                                profile.npi = val;
                                break;
                            case "first_name":
                                profile.first_name = val;

                                break;
                            case "last_name":
                                profile.last_name = val;
                                break;
                            case "middle_name":
                                profile.middle_name = val;
                                break;
                            case "title":
                                profile.title = val;
                                break;
                            case "dob":
                                profile.dob = val;
                                break;
                            case "gender":
                                profile.gender = val;
                                break;
                            case "email":
                                profile.email = val;
                                break;
                            case "personal_practice_type":
                                profile.personal_practice_type = val;
                                //// public string practice_type { get; set; }
                                break;
                            case "home_street1":
                                profile.home_street1 = val;
                                break;
                            case "home_street2":
                                profile.home_street2 = val;
                                break;
                            case "home_city":
                                profile.home_city = val;
                                break;
                            case "home_state":
                                profile.home_state = val;
                                break;
                            case "home_zip":
                                profile.home_zip = val;

                                break;
                            case "education":
                                profile.education = val;
                                break;
                            case "language_spoken":
                                profile.language_spoken = val;
                                break;
                            case "board_certification":
                                profile.board_certification = val;
                                break;
                            case "specialty":
                            case "specialty_id":
                                //    profile.specialty = val;
                                break;
                            case "practice_npi":
                                profile.practice_npi = val;
                                break;
                            case "practice_name":
                                profile.practice_name = val;
                                break;
                            case "practice_type":
                                profile.practice_type = val;
                                break;
                            case "dea":
                                profile.dea = val;
                                break;
                            case "clinician_role":
                                profile.clinician_role = val;
                                break;
                            case "scheduling_solution":
                                profile.scheduling_solution = val;
                                break;
                            case "current_scheduling":
                                profile.current_scheduling = val;
                                break;
                            case "practice_street":
                                profile.practice_street = val;
                                break;
                            case "practice_zip":
                                profile.practice_zip = val;

                                break;
                            case "practice_phone_primary":
                                profile.practice_phone_primary = val; // primary phone number
                                break;
                            case "practice_fax":
                                profile.practice_fax = val;
                                break;
                            case "practice_phone_cs":
                                profile.practice_phone_cs = val; // customer service number
                                break;
                            case "practice_phone_office":
                                profile.practice_phone_office = val; // office phone
                                break;
                            case "practice_clinicians":
                                profile.practice_clinicians = val; // no of field clinicians
                                break;
                            case "practice_exams":
                                profile.practice_exams = val; // no of exams you can handle per week
                                break;

                            case "geographic_market":
                                profile.geographic_market = val; // geographic market
                                break;
                            case "practice_expansion":
                                profile.practice_expansion = val; // future expansion plans, new market, 
                                break;
                            case "practice_insurance":
                                profile.practice_insurance = val; // insurance list you are in Network or will acccept
                                break;
                            case "practice_tax_number":
                                profile.practice_tax_number = val; // federal tax id number
                                break;
                            case "practice_emr":
                                profile.practice_emr = val; // emr software that you are currently using
                                break;
                            case "network_insurance":
                                profile.network_insurance = val; // in-network insurances
                                break;

                            // PRIMARY CONTACT
                            case "primary_contact_name":
                                profile.primary_contact_name = val; // primary contact/ operational contact/ financial contact
                                break;
                            case "primary_contact_email":
                                profile.primary_contact_email = val; // primary contact/ operational contact/ financial contact
                                break;
                            case "primary_contact_phone":
                                profile.primary_contact_phone = val; // primary contact/ operational contact/ financial contact
                                break;

                            // OPERATIONAL CONTACT
                            case "operational_contact_name":
                                profile.operational_contact_name = val; // primary contact/ operational contact/ financial contact
                                break;
                            case "operational_contact_email":
                                profile.operational_contact_email = val; // primary contact/ operational contact/ financial contact
                                break;
                            case "operational_contact_phone":
                                profile.operational_contact_phone = val; // primary contact/ operational contact/ financial contact
                                break;

                            // FINANCIAL CONTACT
                            case "financial_contact_name":
                                profile.financial_contact_name = val; // primary contact/ operational contact/ financial contact
                                break;
                            case "financial_contact_email":
                                profile.financial_contact_email = val; // primary contact/ operational contact/ financial contact
                                break;
                            case "financial_contact_phone":
                                profile.financial_contact_phone = val; // primary contact/ operational contact/ financial contact
                                break;

                            case "billing_bankname":

                                profile.billing_bankname = val;
                                break;
                            case "billing_account":
                                profile.billing_account = val;
                                break;
                            case "billing_routing":
                                profile.billing_routing = val;
                                break;

                            case "password":
                                profile.password = val;
                                break;
                            case "status":
                                profile.status = val;
                                break;
                            case "is_reviewed":
                                profile.is_reviewed = val;
                                break;

                            default:
                                msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                                return Json(new { data = new string[] { }, message = msg, success = false });

                        }


                    }
                }

                return saveDoctorProfile(profile);
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("doctor")]
        public async Task<IHttpActionResult> saveDoctor(doctor_save doc)
        {
            try
            {
                IsRequired("username", doc.email, 2);
                IsRequired("first_name", doc.first_name, 2);
                IsRequired("last_name", doc.last_name, 2);
                IsRequired("password", doc.password, 2);
                IsRequired("device_token", doc.device_token, 2);
                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }

                // authorization is in the function
                return saveDoctor_user(doc);
            }
            catch (Exception ex) {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }

        }

        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("doctor/isreviewed")]
        public async Task<IHttpActionResult> putUpdateReviewed(update_doctor u)
        {
            //if (!authorize) return Json(new { data = new string[] { }, message = "not authorized", success = false });
            // accepts doctor id
            var doc = dbEntity.DOCTORs.Where(a => a.id == u.doctor_id);
            if (doc.Count() > 0)
            {
                var doc_ext = dbEntity.DOCTOR_ext.Where(b => b.rel_DOCTOR_id == u.doctor_id && b.attr_name == "is_reviewed");
                if (doc_ext.Count() > 0) {
                    if (u.is_reviewed == true)
                        doc_ext.FirstOrDefault().value = "true";
                    else
                        doc_ext.FirstOrDefault().value = "false";

                    dbEntity.Entry(doc_ext.FirstOrDefault()).State = System.Data.Entity.EntityState.Modified;


                    // add to [USER] table if [is_reviewed] = true
                    if (u.is_reviewed == true)
                    {
                        var u_doc = dbEntity.USERs.Where(b => b.username == doc.FirstOrDefault().email && b.ref_USER_type.dname.ToLower() == "doctor");
                        if (u_doc.Count() > 0)
                        {
                            return Json(new { data = new string[] { }, message = "Username already exist as USER.", success = false });
                        }

                        //string password = "password123";
                        //string salt1 = System.Guid.NewGuid().ToString();
                        //string encryp1 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + salt1));
                        var doc_password = dbEntity.DOCTOR_ext.Where(b => b.rel_DOCTOR_id == u.doctor_id && b.attr_name == "password");
                        var doc_salt = dbEntity.DOCTOR_ext.Where(b => b.rel_DOCTOR_id == u.doctor_id && b.attr_name == "password_salt");

                        USER new_user = new USER();
                        new_user.name_first = doc.FirstOrDefault().name_first;
                        new_user.name_last = doc.FirstOrDefault().name_last;
                        new_user.username = doc.FirstOrDefault().email; //email address?
                        new_user.phash = doc_password.FirstOrDefault().value;
                        new_user.tlas = doc_salt.FirstOrDefault().value;
                        new_user.rel_ref_status_id = 7;
                        new_user.rel_ref_USER_type_id = dbEntity.ref_USER_type.Where(a => a.dname.ToLower() == "doctor").FirstOrDefault().id;
                        new_user.create_by__USER_id = 0;
                        new_user.dt_create = dt;
                        dbEntity.USERs.Add(new_user);

                    }

                    dbEntity.SaveChanges();

                    return Json(new { data = "", message = "Updated.", success = true });
                }
                return Json(new { data = "", message = "", success = false });
            }
            return Json(new { data = "", message = "", success = false });
        }



        private IHttpActionResult saveDoctorClaim(doctor_claim doc)
        {
            //string npi = "", first_name = "", middle_name = "", last_name = "", email = "", title = "", dea = "", dob = "", clinician_role = "",
            //     gender = "", parent_guardian = "", street1 = "", street2 = "", city = "", state = "", zip_code = "",
            //     practice_name = "", practice_phone = "", practice_street1 = "", practice_street2 = "", practice_city = "", practice_state = "", practice_zip = "", practice_fax = "",
            //     insurance_id = "", exam_encounter = "";
            bool i = false;

            DOCTOR save_doc = new DOCTOR();
            save_doc.NPI = doc.npi;
            save_doc.name_first = doc.first_name;
            if (!string.IsNullOrEmpty(doc.middle_name))
            {
                save_doc.name_middle = doc.middle_name;
            }

            save_doc.name_last = doc.last_name;

            if (!string.IsNullOrEmpty(doc.title))
            {
                save_doc.title = doc.title;
            }
            //doc.dea = dea;

            save_doc.gender = doc.gender == null ? null : doc.gender[0].ToString().ToUpper();

            if (!string.IsNullOrEmpty(doc.home_street1))
            {
                save_doc.home_addr_1 = doc.home_street1;
            }
            if (!string.IsNullOrEmpty(doc.home_street2))
            {
                save_doc.home_addr_2 = doc.home_street2;
            }

            // check if there is zip value
            if (!string.IsNullOrEmpty(doc.home_zip))
            {
                i = validateZip("home_zip", "Home Zip", doc.home_zip, save_doc.id);

                //var _zip = dbEntity.ref_zip.Where(a => a.zip == doc.home_zip);
                //if (_zip.Count() > 0)
                //{ save_doc.addr_rel_ref_zip_id = _zip.FirstOrDefault().id; }
                //else
                //{ return Json(new { data = new string[] { }, message = "Invalid home_zip value.", success = false }); }
            }

            save_doc.dt_create = dt;
            save_doc.create_by__USER_id = 0;
            //doc.rel_ref_zip =
            dbEntity.DOCTORs.Add(save_doc);
            dbEntity.SaveChanges();


            if (!string.IsNullOrEmpty(doc.dob))
            {
                #region DOB
                //DOCTOR_ext d_ext = new DOCTOR_ext();
                //d_ext.rel_DOCTOR_id = save_doc.id;
                //d_ext.attr_name = "dob";
                //d_ext.dname = "DOB";
                //d_ext.value = doc.dob;
                //d_ext.dt_create = dt;
                //d_ext.create_by__USER_id = 0;
                //dbEntity.DOCTOR_ext.Add(d_ext);
                //dbEntity.SaveChanges();
                #endregion
                //bool i = saveDoctor_ext("dob", "DOB", doc.dob, save_doc.id);
                saveClaimDoctor_ext("dob", "Date of Birth", doc.dob, save_doc.id);
            }

            if (!string.IsNullOrEmpty(doc.password))
            {
                string salt = System.Guid.NewGuid().ToString();
                string encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(doc.password + salt));
                i = saveClaimDoctor_ext("password", "Password", encryp, save_doc.id);
                i = saveClaimDoctor_ext("salt", "Password Salt", salt, save_doc.id);
            }

            if (!string.IsNullOrEmpty(doc.exam_encounter))
            {
                #region
                //DOCTOR_ext d_ext = new DOCTOR_ext();
                //d_ext.rel_DOCTOR_id = save_doc.id;
                //d_ext.attr_name = "exam_encounter";
                //d_ext.dname = "Exam Encounter";
                //d_ext.value = doc.exam_encounter;
                //d_ext.dt_create = dt;
                //d_ext.create_by__USER_id = 0;
                //dbEntity.DOCTOR_ext.Add(d_ext);
                //dbEntity.SaveChanges();
                #endregion

                i = saveClaimDoctor_ext("exam_encounter", "Exam Encounter", doc.exam_encounter, save_doc.id);
            }

            if (!string.IsNullOrEmpty(doc.clinician_role))
            {
                #region
                //DOCTOR_ext d_ext = new DOCTOR_ext();
                //d_ext.rel_DOCTOR_id = save_doc.id;
                //d_ext.attr_name = "clinician_role";
                //d_ext.dname = "Clinician Role";
                //d_ext.value = doc.exam_encounter;
                //d_ext.dt_create = dt;
                //d_ext.create_by__USER_id = 0;
                //dbEntity.DOCTOR_ext.Add(d_ext);
                //dbEntity.SaveChanges();
                #endregion

                i = saveClaimDoctor_ext("clinician_role", "Clinician Role", doc.exam_encounter, save_doc.id);
            }

            if (!string.IsNullOrEmpty(doc.practice_name))
            {
                #region
                //DOCTOR_ext d_ext = new DOCTOR_ext();
                //d_ext.rel_DOCTOR_id = save_doc.id;
                //d_ext.attr_name = "practice_name";
                //d_ext.dname = "Practice Name";
                //d_ext.value = doc.practice_name;
                //d_ext.dt_create = dt;
                //d_ext.create_by__USER_id = 0;
                //dbEntity.DOCTOR_ext.Add(d_ext);
                //dbEntity.SaveChanges();
                #endregion

                i = saveClaimDoctor_ext("practice_name", "Practice Name", doc.practice_name, save_doc.id);
            }

            if (!string.IsNullOrEmpty(doc.practice_phone))
            {
                #region
                //DOCTOR_ext d_ext = new DOCTOR_ext();
                //d_ext.rel_DOCTOR_id = save_doc.id;
                //d_ext.attr_name = "practice_phone";
                //d_ext.dname = "Practice Phone";
                //d_ext.value = doc.practice_phone;
                //d_ext.dt_create = dt;
                //d_ext.create_by__USER_id = 0;
                //dbEntity.DOCTOR_ext.Add(d_ext);
                //dbEntity.SaveChanges();
                #endregion  
                i = saveClaimDoctor_ext("practice_phone", "Practice Phone", doc.practice_phone, save_doc.id);
            }

            if (!string.IsNullOrEmpty(doc.practice_street1))
            {
                #region
                //DOCTOR_ext d_ext = new DOCTOR_ext();
                //d_ext.rel_DOCTOR_id = save_doc.id;
                //d_ext.attr_name = "practice_street1";
                //d_ext.dname = "Practice Street1";
                //d_ext.value = doc.practice_street1;
                //d_ext.dt_create = dt;
                //d_ext.create_by__USER_id = 0;
                //dbEntity.DOCTOR_ext.Add(d_ext);
                //dbEntity.SaveChanges();
                #endregion

                i = saveClaimDoctor_ext("practice_street1", "Practice Street1", doc.practice_street1, save_doc.id);
            }

            if (!string.IsNullOrEmpty(doc.practice_street2))
            {
                #region
                //DOCTOR_ext d_ext = new DOCTOR_ext();
                //d_ext.rel_DOCTOR_id = save_doc.id;
                //d_ext.attr_name = "practice_street2";
                //d_ext.dname = "Practice Street2";
                //d_ext.value = doc.practice_street2;
                //d_ext.dt_create = dt;
                //d_ext.create_by__USER_id = 0;
                //dbEntity.DOCTOR_ext.Add(d_ext);
                //dbEntity.SaveChanges();
                #endregion

                i = saveClaimDoctor_ext("practice_street2", "Practice Street2", doc.practice_street2, save_doc.id);
            }

            if (!string.IsNullOrEmpty(doc.practice_city))
            {
                #region
                //DOCTOR_ext d_ext = new DOCTOR_ext();
                //d_ext.rel_DOCTOR_id = save_doc.id;
                //d_ext.attr_name = "practice_city";
                //d_ext.dname = "Practice City";
                //d_ext.value = doc.practice_city;
                //d_ext.dt_create = dt;
                //d_ext.create_by__USER_id = 0;
                //dbEntity.DOCTOR_ext.Add(d_ext);
                //dbEntity.SaveChanges();
                #endregion

                i = saveClaimDoctor_ext("practice_city", "Practice City", doc.practice_city, save_doc.id);
            }

            if (!string.IsNullOrEmpty(doc.practice_state))
            {
                #region
                //DOCTOR_ext d_ext = new DOCTOR_ext();
                //d_ext.rel_DOCTOR_id = save_doc.id;
                //d_ext.attr_name = "practice_state";
                //d_ext.dname = "Practice State";
                //d_ext.value = doc.practice_state;
                //d_ext.dt_create = dt;
                //d_ext.create_by__USER_id = 0;
                //dbEntity.DOCTOR_ext.Add(d_ext);
                //dbEntity.SaveChanges();
                #endregion

                i = saveClaimDoctor_ext("practice_state", "Practice State", doc.practice_state, save_doc.id);
            }

            if (!string.IsNullOrEmpty(doc.practice_zip))
            {
                i = validateZip("practice_zip", "Practice Zip", doc.practice_zip, save_doc.id);

                #region
                //var _zip1 = dbEntity.ref_zip.Where(a => a.zip == doc.practice_zip);
                //if (_zip1.Count() > 0)
                //{
                //    DOCTOR_ext d_ext = new DOCTOR_ext();
                //    d_ext.rel_DOCTOR_id = save_doc.id;
                //    d_ext.attr_name = "practice_zip";
                //    d_ext.dname = "Practice Zip";
                //    d_ext.value = doc.practice_zip;
                //    d_ext.dt_create = dt;
                //    d_ext.create_by__USER_id = 0;
                //    dbEntity.DOCTOR_ext.Add(d_ext);
                //    dbEntity.SaveChanges();
                //}
                //else
                //{ return Json(new { data = new string[] { }, message = "Invalid practice_zip value.", success = false }); }
                #endregion
            }


            if (!string.IsNullOrEmpty(doc.practice_fax))
            {
                #region
                //DOCTOR_ext d_ext = new DOCTOR_ext();
                //d_ext.rel_DOCTOR_id = save_doc.id;
                //d_ext.attr_name = "practice_fax";
                //d_ext.dname = "Practice Fax";
                //d_ext.value = doc.practice_fax;
                //d_ext.dt_create = dt;
                //d_ext.create_by__USER_id = 0;
                //dbEntity.DOCTOR_ext.Add(d_ext);
                //dbEntity.SaveChanges();
                #endregion

                i = saveClaimDoctor_ext("practice_fax", "Practice Fax", doc.practice_fax, save_doc.id);
            }


            if (!string.IsNullOrEmpty(doc.education))
            {
                i = saveClaimDoctor_ext("education", "Education", doc.education, save_doc.id);
            }
            if (!string.IsNullOrEmpty(doc.education))
            {
                i = saveClaimDoctor_ext("experience", "Experience", doc.experience, save_doc.id);
            }


            if (!string.IsNullOrEmpty(doc.insurance_id))
            {
                long ins_id_new = 0;
                bool b_ins = long.TryParse(doc.insurance_id, out ins_id_new);

                if (b_ins)
                {
                    #region
                    //DOCTOR_ext d = new DOCTOR_ext();
                    //d.rel_DOCTOR_id = save_doc.id;
                    //d.attr_name = "insurance_id";
                    //d.dname = "Insurance ID";
                    //d.value = doc.insurance_id;
                    //d.dt_create = dt;
                    //d.create_by__USER_id = 0;
                    //dbEntity.DOCTOR_ext.Add(d);
                    //dbEntity.SaveChanges();
                    #endregion

                    i = saveClaimDoctor_ext("insurance_id", "Insurance ID", doc.insurance_id, save_doc.id);
                }
                else
                { return Json(new { data = new string[] { }, message = "Invalid insurance_id value.", success = false }); }

                #region
                //DOCTOR_ext d_ext = new DOCTOR_ext();
                //d_ext.rel_DOCTOR_id = save_doc.id;
                //d_ext.attr_name = "is_reviewed";
                //d_ext.dname = "Is Reviewed?";
                //d_ext.value = "false";
                //d_ext.dt_create = dt;
                //d_ext.create_by__USER_id = 0;
                //dbEntity.DOCTOR_ext.Add(d_ext);
                //dbEntity.SaveChanges();
                #endregion

                i = saveClaimDoctor_ext("is_reviewed", "Is Reviewed?", "false", save_doc.id);

            }

            return Json(new { data = new string[] { }, message = "Doctor's claim save successfully.", success = true });
        }


        private IHttpActionResult saveDoctor_user(doctor_save doc)
        {
            //string v = httpCon.Request.Headers["Authorization"];
            string msg = "The authorization header is not valid.";

            //if (Validation.userAuth(v))
            //{

            string salt = System.Guid.NewGuid().ToString();
            string encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(doc.password + salt));

            USER new_user = new USER();
            new_user.name_first = doc.first_name; // first_name
            new_user.name_last = doc.last_name; // last_name
            new_user.username = doc.email;
            new_user.phash = encryp;
            new_user.tlas = salt;
            new_user.dt_create = dt;
            new_user.create_by__USER_id = 0;
            new_user.rel_ref_status_id = 7;
            new_user.rel_ref_USER_type_id = dbEntity.ref_USER_type.Where(a => a.dname.ToLower() == "doctor").FirstOrDefault().id;
            dbEntity.USERs.Add(new_user);
            dbEntity.SaveChanges();

            return Json(new { data = new string[] { }, message = "Doctor successfully saved.", success = true });
            //}
            //return Json(new { data = new string[] { }, message = msg, success = true });
        }

        private IHttpActionResult saveDoctorProfile(post_doctor_profile prof) {
            // save the doctor profile, if the npi is not yet existingsaveDoctorProfile
            // update the doctor profile, if the npi is already existing

            //string v = httpCon.Request.Headers["Authorization"];
            string msg = "The authorization header is not valid.";

            //if (Validation.userAuth(v))
            //{
            IsRequired("npi", prof.npi, 2);
            IsRequired("first_name", prof.first_name, 2);
            IsRequired("last_name", prof.last_name, 2);
            IsRequired("email", prof.email, 2);
            // password shoud be required if there is an email
            if (haserror)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }

            var doc_rec = dbEntity.hs_DOCTOR.Where(a => a.NPI == prof.npi);

            //if (doc_rec.Count() > 0)
            //{
            //    // do update
            //   // return _updateDoctor(doc_rec.FirstOrDefault(), prof);
            //}
            //else
            if (doc_rec.Count() == 0)
            {
                // do save 
                return _saveDoctor(prof);
            }
            //}
            return Json(new { data = new string[] { }, message = msg, success = false });
        }

        private IHttpActionResult _updateDoctor(hs_DOCTOR doc_rec, post_doctor_profile prof)
        {

            //string v = httpCon.Request.Headers["Authorization"];
            string msg = "The authorization header is not valid.";

            //if (Validation.userAuth(v))
            //{


            prof.email = prof.email == null ? null : prof.email.ToLower();

            var d = dbEntity.hs_DOCTOR.Where(a => a.email == prof.email);
            if (d.Count() > 0)
            {
                if (d.FirstOrDefault().NPI != prof.npi)
                {
                    return Json(new { data = new string[] { }, message = "Email already exist.", success = false });
                }
            }
            else
            {
                //long doc_type = dbEntity.ref_USER_type.Where(a => a.dname.ToLower() == "patient").FirstOrDefault().id;
                var u = dbEntity.USERs.Where(a => a.username == prof.email);
                if (u.Count() > 0)
                {
                    return Json(new { data = new string[] { }, message = "Email already exist.", success = false });
                }
            }

            bool i = false;
            // temp code entry to [USER]
            //if (string.IsNullOrEmpty(doc_rec.email)) {
            //    doc_rec.email = prof.email;
            //    dbEntity.Entry(doc_rec).State = EntityState.Modified;
            //    dbEntity.SaveChanges();
            //}

            //var doc_user = dbEntity.USERs.Where(a => a.username == doc_rec.email);
            // if (doc_user.Count() > 0) // || string.IsNullOrEmpty(doc_rec.email)
            if (!string.IsNullOrEmpty(prof.email))
            {
                doc_rec.email = prof.email;
                dbEntity.Entry(doc_rec).State = EntityState.Modified;
                dbEntity.SaveChanges();

                if (!string.IsNullOrEmpty(prof.password))
                {

                    string salt = System.Guid.NewGuid().ToString();
                    string encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(prof.password + salt));
                    i = Validation.saveDoctor_ext("password", "Password", encryp, doc_rec.id);
                    i = Validation.saveDoctor_ext("password_salt", "Password Salt", salt, doc_rec.id);
                }

                var u_pass = dbEntity.hs_DOCTOR_ext.Where(a => a.attr_name == "password" && a.rel_DOCTOR_id == doc_rec.id);

                //i = saveDoctor_ext("password_salt", "Password Salt", salt, save_doc.id);
                var u_salt = dbEntity.hs_DOCTOR_ext.Where(a => a.attr_name == "password_salt" && a.rel_DOCTOR_id == doc_rec.id);


                // added code to save hs_DOCTOR on USER
                i = saveDoctorUser(prof, u_pass.FirstOrDefault().value, u_salt.FirstOrDefault().value);
            }
            // temporary code: end




            if (!string.IsNullOrEmpty(prof.is_reviewed))
            {
                var d_ext = dbEntity.hs_DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doc_rec.id && a.attr_name == "is_reviewed");
                if (d_ext.Count() > 0)
                {
                    //var u_doc = dbEntity.USERs.Where(b => b.username == doc_rec.email && b.ref_USER_type.dname.ToLower() == "doctor");
                    //if (u_doc.Count() > 0)
                    //{
                    //    return Json(new { data = new string[] { }, message = "Username already exist as USER.", success = false });
                    //}

                    //if (prof.is_reviewed == "true")
                    //{
                    //    //string password = "password123";
                    //    //string salt1 = System.Guid.NewGuid().ToString();
                    //    //string encryp1 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + salt1));

                    //    //USER new_user = new USER();
                    //    //new_user.name_first = doc_rec.name_first;
                    //    //new_user.name_last = doc_rec.name_last;
                    //    //new_user.username = doc_rec.email; //email address?
                    //    //new_user.phash = encryp1;
                    //    //new_user.tlas = salt1;
                    //    //new_user.rel_ref_status_id = 7;
                    //    //new_user.rel_ref_USER_type_id = dbEntity.ref_USER_type.Where(a => a.dname.ToLower() == "doctor").FirstOrDefault().id;
                    //    //new_user.create_by__USER_id = 0;
                    //    //new_user.dt_create = dt;
                    //    //dbEntity.USERs.Add(new_user);
                    //    //dbEntity.SaveChanges();
                    //}
                }

            }


            //// do update
            //foreach (var n in doc_rec)
            //{
            if (!string.IsNullOrEmpty(prof.first_name))
                doc_rec.name_first = prof.first_name;

            if (!string.IsNullOrEmpty(prof.last_name))
                doc_rec.name_last = prof.last_name;

            if (!string.IsNullOrEmpty(prof.middle_name))
                doc_rec.name_middle = prof.middle_name;

            if (!string.IsNullOrEmpty(prof.title))
                doc_rec.title = prof.title;

            //if (!string.IsNullOrEmpty(prof.dob))
            //    doc_rec. = prof.dob;

            if (!string.IsNullOrEmpty(prof.gender))
                doc_rec.gender = prof.gender[0].ToString().ToUpper();

            if (!string.IsNullOrEmpty(prof.email))
                doc_rec.email = prof.email.ToLower();

            if (!string.IsNullOrEmpty(prof.home_street1))
                doc_rec.home_addr_1 = prof.home_street1;

            if (!string.IsNullOrEmpty(prof.home_street2))
                doc_rec.home_addr_2 = prof.home_street2;

            if (!string.IsNullOrEmpty(prof.home_zip))
            {
                string z = prof.home_zip.Substring(0, 5);
                var zip = dbEntity.ref_zip.Where(a => a.zip == z);

                if (zip.Count() > 0)
                {
                    doc_rec.home_addr_zip_id = zip.FirstOrDefault().id;
                }

            }

            // check this condition, to see if there is an update
            if (doc_rec != null) { }

            if (!string.IsNullOrEmpty(prof.first_name)
                || !string.IsNullOrEmpty(prof.last_name)
                || !string.IsNullOrEmpty(prof.middle_name)
                || !string.IsNullOrEmpty(prof.title)
                || !string.IsNullOrEmpty(prof.gender)
                || !string.IsNullOrEmpty(prof.email)
                || !string.IsNullOrEmpty(prof.home_street1)
                || !string.IsNullOrEmpty(prof.home_street2)
                )
            {

                doc_rec.dt_update = dt;
                dbEntity.Entry(doc_rec).State = System.Data.Entity.EntityState.Modified;
                dbEntity.SaveChanges();
            }

            // home_city
            if (!string.IsNullOrEmpty(prof.home_city))
            {
                i = Validation.saveDoctor_ext("home_city", "Home City", prof.home_city, doc_rec.id);
            }

            // home_state
            if (!string.IsNullOrEmpty(prof.home_state))
            {
                i = Validation.saveDoctor_ext("home_state", "Home State", prof.home_state, doc_rec.id);
            }

            // home_zip
            if (!string.IsNullOrEmpty(prof.home_zip))
            {
                if (!validateZip("home_zip", "Home Zip", prof.home_zip, doc_rec.id))
                {
                    return Json(new { data = new string[] { }, message = "Invalid home_zip value.", success = false });
                }
                //  bool i = saveDoctor_ext("home_zip", "Home Zip", prof.home_zip, doc_rec.id);
            }

            // education
            if (!string.IsNullOrEmpty(prof.education))
            {
                i = Validation.saveDoctor_ext("education", "Education", prof.education, doc_rec.id);
            }

            // language_spoken
            if (!string.IsNullOrEmpty(prof.language_spoken))
            {
                #region
                //var lang_spo = dbEntity.hs_DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doc_rec.id && a.attr_name == "language_spoken");

                //string s1 = "";
                //if (lang_spo.Count() > 0)
                //{
                //    //string[] s = prof.language_spoken.Split(',');
                //    string[] s = lang_spo.FirstOrDefault().value.Split(',');

                //    foreach (var n in s)
                //    {
                //        if (!string.IsNullOrEmpty(n.Trim()))
                //        {
                //            var ab = dbEntity.ref_languages.Where(a => a.name.ToLower() == n.ToLower());
                //            if (ab.Count() == 0)
                //            {
                //                ref_languages ref_lang = new ref_languages();
                //                ref_lang.name = n;
                //                dbEntity.ref_languages.Add(ref_lang);
                //                dbEntity.SaveChanges();

                //                s1 += ref_lang.id.ToString();

                //            }
                //            else
                //            {
                //                s1 += ab.FirstOrDefault().id;
                //            }

                //            s1 += ",";
                //        }
                //    }
                //    s1 = s1.Substring(0, (s1.Length - 1));
                //}
                #endregion


                i = Validation.saveDoctor_ext("language_spoken", "Language Spoken", prof.language_spoken, doc_rec.id);
            }

            // board_certification
            if (!string.IsNullOrEmpty(prof.board_certification))
            {
                i = Validation.saveDoctor_ext("board_certification", "Board Certification", prof.board_certification, doc_rec.id);
            }

            // personal_practice_type
            if (!string.IsNullOrEmpty(prof.personal_practice_type))
            {
                i = Validation.saveDoctor_ext("personal_practice_type", "Personal Practice Type", prof.personal_practice_type, doc_rec.id);
            }

            // specialty
            if (!string.IsNullOrEmpty(prof.specialty))
            {
                i = Validation.saveDoctor_ext("specialty_id", "Specialty ID", prof.specialty, doc_rec.id);
            }

            // hospital_affiliation
            if (!string.IsNullOrEmpty(prof.hospital_affiliation))
            {
                i = Validation.saveDoctor_ext("hospital_affiliation", "Hospital Affiliation", prof.hospital_affiliation, doc_rec.id);
            }
            // practice_npi
            if (!string.IsNullOrEmpty(prof.practice_npi))
            {
                i = Validation.saveDoctor_ext("practice_npi", "Practice NPI", prof.practice_npi, doc_rec.id);
            }

            // practice_name
            if (!string.IsNullOrEmpty(prof.practice_name))
            {
                i = Validation.saveDoctor_ext("practice_name", "Practice Name", prof.practice_name, doc_rec.id);
            }

            // practice_type
            if (!string.IsNullOrEmpty(prof.practice_type))
            {
                i = Validation.saveDoctor_ext("practice_type", "Practice Type", prof.practice_type, doc_rec.id);
            }

            // dob
            if (!string.IsNullOrEmpty(prof.dob))
            {
                string s = Validation.validateDate(prof.dob);
                if (s.Length > 0)
                {
                    i = Validation.saveDoctor_ext("dob", "DOB", prof.dob, doc_rec.id);
                }
                else
                { return Json(new { data = new string[] { }, message = "Invalid parameter value: dob.", success = false }); }
            }

            // dea 
            if (!string.IsNullOrEmpty(prof.dea))
            {
                i = Validation.saveDoctor_ext("dea", "DEA", prof.dea, doc_rec.id);
            }

            // clinician_role
            if (!string.IsNullOrEmpty(prof.clinician_role))
            {
                i = Validation.saveDoctor_ext("clinician_role", "Clinician Role", prof.clinician_role, doc_rec.id);
            }

            // scheduling_solution
            if (!string.IsNullOrEmpty(prof.scheduling_solution))
            {
                i = Validation.saveDoctor_ext("scheduling_solution", "Scheduling Solution", prof.scheduling_solution, doc_rec.id);
            }

            // current_scheduling
            if (!string.IsNullOrEmpty(prof.current_scheduling))
            {
                i = Validation.saveDoctor_ext("current_scheduling", "Current Scheduling", prof.current_scheduling, doc_rec.id);
            }

            // practice_street
            if (!string.IsNullOrEmpty(prof.practice_street))
            {
                i = Validation.saveDoctor_ext("practice_street", "Practice Street", prof.practice_street, doc_rec.id);
            }

            // practice_zip
            if (!string.IsNullOrEmpty(prof.practice_zip))
            {
                if (!validateZip("practice_zip", "Practice Zip", prof.practice_zip, doc_rec.id))
                {
                    return Json(new { data = new string[] { }, message = "Invalid practice_zip value.", success = false });
                }
                //bool i = saveDoctor_ext("practice_zip", "practice_zip", prof.practice_zip, doc_rec.id);
            }

            // practice_phone_primary // primary phone number
            if (!string.IsNullOrEmpty(prof.practice_phone_primary))
            {
                i = Validation.saveDoctor_ext("practice_phone_primary", "Practice Phone Primary", prof.practice_phone_primary, doc_rec.id);
            }

            // practice_fax 
            if (!string.IsNullOrEmpty(prof.practice_fax))
            {
                i = Validation.saveDoctor_ext("practice_fax", "Practice Fax", prof.practice_fax, doc_rec.id);
            }

            // practice_phone_cs // customer service number
            if (!string.IsNullOrEmpty(prof.practice_phone_cs))
            {
                i = Validation.saveDoctor_ext("practice_phone_cs", "Practice Phone CS", prof.practice_phone_cs, doc_rec.id);
            }

            // practice_phone_office // office phone
            if (!string.IsNullOrEmpty(prof.practice_phone_office))
            {
                i = Validation.saveDoctor_ext("practice_phone_office", "Practice Phone Office", prof.practice_phone_office, doc_rec.id);
            }

            // practice_clinicians // no of field clinicians
            if (!string.IsNullOrEmpty(prof.practice_clinicians))
            {
                i = Validation.saveDoctor_ext("practice_clinicians", "Practice Clinicians", prof.practice_clinicians, doc_rec.id);
            }

            // practice_exams  // no of exams you can handle per week
            if (!string.IsNullOrEmpty(prof.practice_exams))
            {
                i = Validation.saveDoctor_ext("practice_exams", "Practice Exams", prof.practice_exams, doc_rec.id);
            }

            // geographic_market  // geographic market
            if (!string.IsNullOrEmpty(prof.geographic_market))
            {
                i = Validation.saveDoctor_ext("geographic_market", "Geographic Market", prof.geographic_market, doc_rec.id);
            }

            // practice_expansion  // future expansion plans, new market, 
            if (!string.IsNullOrEmpty(prof.practice_expansion))
            {
                i = Validation.saveDoctor_ext("practice_expansion", "Practice Expansion", prof.practice_expansion, doc_rec.id);
            }

            // practice_insurance // insurance list you are in Network or will acccept
            if (!string.IsNullOrEmpty(prof.practice_insurance))
            {
                i = Validation.saveDoctor_ext("practice_insurance", "Practice Insurance", prof.practice_insurance, doc_rec.id);
            }

            // practice_tax_number // federal tax id number
            if (!string.IsNullOrEmpty(prof.practice_tax_number))
            {
                i = Validation.saveDoctor_ext("practice_tax_number", "Practice Tax Number", prof.practice_tax_number, doc_rec.id);
            }

            // practice_emr  // emr software that you are currently using
            if (!string.IsNullOrEmpty(prof.practice_emr))
            {
                i = Validation.saveDoctor_ext("practice_emr", "Practice Emr", prof.practice_emr, doc_rec.id);
            }

            // network_insurance // in-network insurances
            if (!string.IsNullOrEmpty(prof.network_insurance))
            {
                i = Validation.saveDoctor_ext("network_insurance", "Network Insurance", prof.network_insurance, doc_rec.id);
            }

            // primary_contact_name // primary contact/ operational contact/ financial contact
            if (!string.IsNullOrEmpty(prof.primary_contact_name))
            {
                i = Validation.saveDoctor_ext("primary_contact_name", "Primary Contact Name", prof.primary_contact_name, doc_rec.id);
            }

            // primary_contact_phone // primary contact/ operational contact/ financial contact
            if (!string.IsNullOrEmpty(prof.primary_contact_phone))
            {
                i = Validation.saveDoctor_ext("primary_contact_phone", "Primary Contact Phone", prof.primary_contact_phone, doc_rec.id);
            }
            // primary_contact_email // primary contact/ operational contact/ financial contact
            if (!string.IsNullOrEmpty(prof.primary_contact_email))
            {
                i = Validation.saveDoctor_ext("primary_contact_email", "Primary Contact Email", prof.primary_contact_email.ToLower(), doc_rec.id);
            }
            // operational_contact_name // primary contact/ operational contact/ financial contact
            if (!string.IsNullOrEmpty(prof.operational_contact_name))
            {
                i = Validation.saveDoctor_ext("operational_contact_name", "Operational Contact Name", prof.operational_contact_name, doc_rec.id);
            }
            // operational_contact_phone // primary contact/ operational contact/ financial contact
            if (!string.IsNullOrEmpty(prof.operational_contact_phone))
            {
                i = Validation.saveDoctor_ext("operational_contact_phone", "Operational Contact Phone", prof.operational_contact_phone, doc_rec.id);
            }
            // operational_contact_email // primary contact/ operational contact/ financial contact
            if (!string.IsNullOrEmpty(prof.operational_contact_email))
            {
                i = Validation.saveDoctor_ext("operational_contact_email", "Operational Contact Email", prof.operational_contact_email.ToLower(), doc_rec.id);
            }
            // financial_contact_name // primary contact/ operational contact/ financial contact
            if (!string.IsNullOrEmpty(prof.financial_contact_name))
            {
                i = Validation.saveDoctor_ext("financial_contact_name", "Financial Contact Name", prof.financial_contact_name, doc_rec.id);
            }
            // financial_contact_phone // primary contact/ operational contact/ financial contact
            if (!string.IsNullOrEmpty(prof.financial_contact_phone))
            {
                i = Validation.saveDoctor_ext("financial_contact_phone", "Financial Contact Phone", prof.financial_contact_phone, doc_rec.id);
            }
            // financial_contact_email // primary contact/ operational contact/ financial contact
            if (!string.IsNullOrEmpty(prof.financial_contact_email))
            {
                i = Validation.saveDoctor_ext("financial_contact_email", "Financial Contact Email", prof.financial_contact_email.ToLower(), doc_rec.id);
            }


            // billing_bankname { get; set; }
            if (!string.IsNullOrEmpty(prof.billing_bankname))
            {
                i = Validation.saveDoctor_ext("billing_bankname", "Billing Bankname", prof.billing_bankname, doc_rec.id);
            }

            // billing_account 
            if (!string.IsNullOrEmpty(prof.billing_account))
            {
                i = Validation.saveDoctor_ext("billing_account", "Billing Account", prof.billing_account, doc_rec.id);
            }

            // billing_routing 
            if (!string.IsNullOrEmpty(prof.billing_routing))
            {
                i = Validation.saveDoctor_ext("billing_routing", "Billing Routing", prof.billing_routing, doc_rec.id);

            }

            // education 
            if (!string.IsNullOrEmpty(prof.education))
            {
                i = Validation.saveDoctor_ext("education", "Education", prof.education, doc_rec.id);

            }


            // experience 
            if (!string.IsNullOrEmpty(prof.experience))
            {
                i = Validation.saveDoctor_ext("experience", "Experience", prof.experience, doc_rec.id);

            }

            //} // n in doc_rec
            // check if reviewed done
            return Json(new { data = new string[] { }, message = "Doctor updated.", success = true });
            //}  
            //return Json(new { data = new string[] { }, message = msg, success = false });
        }

        private bool validateZip(string attr_name, string dname, string zip1, long doc_id)
        {
            string z2 = zip1.Substring(0, 5);
            // if zip is more than 5 in length, get only the first 5

            var _zip = dbEntity.ref_zip.Where(a => a.zip == z2);
            if (_zip.Count() > 0)
            {
                return Validation.saveDoctor_ext(attr_name, dname, zip1, doc_id);

            }
            return false;
        }

        private ref_zip validateZip2(string zip)
        {
            string z2 = zip.Substring(0, 5);
            // if zip is more than 5 in length, get only the first 5

            var _zip = dbEntity.ref_zip.Where(a => a.zip == z2);
            if (_zip.Count() > 0)
            {
                return _zip.FirstOrDefault();

            }
            return null;
        }

        private IHttpActionResult _saveDoctor(post_doctor_profile prof)
        {
            prof.email = prof.email == null ? "" : prof.email.ToLower();

            // check email
            var d = dbEntity.hs_DOCTOR.Where(a => a.email == prof.email);
            if (d.Count() > 0)
            {
                var u = dbEntity.USERs.Where(a => a.username == prof.email);
                if (u.Count() > 0)
                {
                    return Json(new { data = new string[] { }, message = "Email already exist.", success = false });
                }
                return Json(new { data = new string[] { }, message = "Email already exist.", success = false });
            }


            // check email: end 

            // check email if exist in [hs_DOCTOR]
            var doc_check = dbEntity.hs_DOCTOR.Where(a => a.email == prof.email);
            if (doc_check.Count() > 0)
            {
                return Json(new { data = new string[] { }, message = "Email already exist.", success = false });
            }

            // check email if exist in [USER]
            var doc_check2 = dbEntity.USERs.Where(a => a.username == prof.email);
            if (doc_check2.Count() > 0)
            {
                return Json(new { data = new string[] { }, message = "Email already exist.", success = false });
            }


            // this adds the doctor profile if the npi is not yet in the database
            hs_DOCTOR save_doc = new hs_DOCTOR();
            save_doc.NPI = prof.npi;
            save_doc.name_first = prof.first_name;
            save_doc.name_last = prof.last_name;
            if (!string.IsNullOrEmpty(prof.middle_name))
            {
                save_doc.name_middle = prof.middle_name;
            }

            if (!string.IsNullOrEmpty(prof.title))
            {
                save_doc.title = prof.title;
            }

            if (!string.IsNullOrEmpty(prof.gender)) {
                save_doc.gender = prof.gender[0].ToString().ToUpper();
            }

            if (!string.IsNullOrEmpty(prof.email)) {
                save_doc.email = prof.email;
            }

            if (!string.IsNullOrEmpty(prof.dob))
            {
                if (!string.IsNullOrEmpty(Validation.validateDate(prof.dob)))
                {
                    save_doc.dob = prof.dob;
                }
                else
                { 
                    return Json(new {data = new string[] { }, message = "Invalid DOB value.", success = false });
                }
                
            }

            if (!string.IsNullOrEmpty(prof.home_zip))
            {
                ref_zip addr_zip = validateZip2(prof.home_zip);
                if (addr_zip !=null)
                {
                    if (prof.home_city != null && addr_zip.city_name.ToLower() != prof.home_city.ToLower())
                    {
                        return Json(new { data= new string[] { }, message="Invalid home_city value.", success=false });
                    }
                    if (prof.home_state != null && addr_zip.city_state.ToLower() != prof.home_state.ToLower())
                    {
                        return Json(new { data = new string[] { }, message = "Invalid home_state value.", success = false });
                    }

                    //if (addr_zip.city_name.ToLower() != prof.home_city.ToLower() && addr_zip.city_state.ToLower() != prof.home_state.ToLower())
                    //{
                        save_doc.home_addr_zip_id = addr_zip.id;
                    //}
                    
                }

                //string z = prof.home_zip.Substring(0, 5);
                //var zip = dbEntity.ref_zip.Where(a => a.zip == z);

                //if (zip.Count() > 0)
                //{
                //    save_doc.addr_rel_ref_zip_id = zip.FirstOrDefault().id;
                //}

            }

        
            //if (!string.IsNullOrEmpty(prof.password)) { save_doc.password = prof.password; }

            if (!string.IsNullOrEmpty(prof.home_street1))
            {
                save_doc.home_addr_1 = prof.home_street1;
            }

            if (!string.IsNullOrEmpty(prof.home_street2)) {
                save_doc.home_addr_1 = prof.home_street2;
            }

            // practice_street
            if (!string.IsNullOrEmpty(prof.practice_street))
            {
                save_doc.home_addr_1 = prof.practice_street;
                //bool i = Validation.saveDoctor_ext("practice_street", "Practice Street", prof.practice_street, save_doc.id);
            }

         
            // practice_zip
            if (!string.IsNullOrEmpty(prof.practice_zip))
            {
                //if (!validateZip("practice_zip", "Practice Zip", prof.practice_zip, save_doc.id))
                //{
                //    return Json(new { data = new string[] { }, message = "Invalid practice_zip value.", success = false });
                //}

                ref_zip addr_zip = validateZip2(prof.practice_zip);
                if (addr_zip != null)
                {
                    if (prof.practice_city != null && addr_zip.city_name.ToLower() != prof.practice_city.ToLower())
                    {
                        return Json(new { data = new string[] { }, message = "Invalid practice_city value.", success = false });
                    }
                    if (prof.practice_state != null && addr_zip.city_state.ToLower() != prof.practice_state.ToLower())
                    {
                        return Json(new { data = new string[] { }, message = "Invalid practice_state value.", success = false });
                    }

                    //if (addr_zip.city_name.ToLower() != prof.home_city.ToLower() && addr_zip.city_state.ToLower() != prof.home_state.ToLower())
                    //{
                    save_doc.practice_addr_zip_id = addr_zip.id;
                    //}

                }
            }

            save_doc.create_by__USER_id = 0;
            save_doc.dt_create = dt;


            dbEntity.hs_DOCTOR.Add(save_doc);
            dbEntity.SaveChanges();

            // saving hs_DOCTOR_ext
            if (!string.IsNullOrEmpty(prof.password))
            {
                string salt = System.Guid.NewGuid().ToString();
                string encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(prof.password + salt));
                bool i = Validation.saveDoctor_ext("password", "Password", encryp, save_doc.id);
                i = Validation.saveDoctor_ext("password_salt", "Password Salt", salt, save_doc.id);

                // added code to save DOCTOR on USER
                i = saveDoctorUser(prof, encryp, salt);
            }

            // prof.spoken_language
            // expect: language_ids, separated by commas
            if (!string.IsNullOrEmpty(prof.language_spoken))
            {
                string[] lang = prof.language_spoken.Split(',');
                foreach (var n in lang)
                {
                    long language_id = Convert.ToInt64(n);
                    var ref_lang = dbEntity.ref_languages.Find(language_id);
                    if (ref_lang !=  null)
                    {
                        con_DOCTOR_ref_language con_doctor = new con_DOCTOR_ref_language();
                        con_doctor.rel_DOCTOR_id = save_doc.id;
                        con_doctor.rel_ref_language_id = ref_lang.id;
                        con_doctor.create_by__USER_id = 0;
                        con_doctor.dt_create = dt;
                        dbEntity.con_DOCTOR_ref_language.Add(con_doctor);
                        dbEntity.SaveChanges();
                    }
                }
                //bool i = Validation.saveDoctor_ext("language_spoken", "Language Spoken", prof.language_spoken, save_doc.id);
            }

            // prof.specialty
            // expect: specialty name, separated by comma
            if (!string.IsNullOrEmpty(prof.specialty))
            { // this will accept string of Specialty ID
                string[] spec = prof.specialty.Split(',');
                foreach (var n in spec)
                {
                    long spec_id = Convert.ToInt64(n);
                    var ref_spec = dbEntity.ref_specialty.Find(spec_id);

                    if (ref_spec != null)
                    {
                        con_DOCTOR_ref_specialty ds = new con_DOCTOR_ref_specialty();
                        ds.rel_DOCTOR_id = save_doc.id;
                        ds.rel_ref_specialty_id = spec_id;
                        ds.create_by__USER_id = 0;
                        ds.dt_create = dt;
                        dbEntity.con_DOCTOR_ref_specialty.Add(ds);
                        dbEntity.SaveChanges();

                    }
              
                }

                //bool i = Validation.saveDoctor_ext("specialty_id", "Specialty ID", prof.specialty, save_doc.id);
            }


            #region
            //// if zip is more than 5 in length, get only the first 5
            //if (!string.IsNullOrEmpty(prof.home_zip))
            //{
            //    // && prof.home_zip.Length > 
            //    if (!validateZip("home_zip", "Home Zip", prof.home_zip, save_doc.id))
            //    {
            //        return Json(new { data = new string[] { }, message = "Invalid home_zip value.", success = false });
            //    }
            //}

            ////doc_prof.dob = prof.dob;
            //if (!string.IsNullOrEmpty(prof.dob))
            //{
            //    //12/12/1990
            //    string s = Validation.validateDate(prof.dob);
            //    if (s.Length > 0)
            //    { bool i = Validation.saveDoctor_ext("dob", "DOB", prof.dob, save_doc.id);
            //    }
            //    else
            //    { return Json(new { data = new string[] { }, message = "Invalid parameter value: dob.", success = false }); }
            //}

            //// prof.home_city
            //if (!string.IsNullOrEmpty(prof.home_city))
            //{
            //    bool i = Validation.saveDoctor_ext("home_city", "Home City", prof.home_city, save_doc.id);
            //}


            //// prof.home_state
            //if (!string.IsNullOrEmpty(prof.home_state))
            //{
            //    bool i = Validation.saveDoctor_ext("home_state", "Home State", prof.home_city, save_doc.id);
            //}
            #endregion


            // prof.education
            if (!string.IsNullOrEmpty(prof.education))
            {
                bool i = Validation.saveDoctor_ext("education", "Education", prof.education, save_doc.id);
            }

          
            // prof.certification
            if (!string.IsNullOrEmpty(prof.board_certification))
            {
                bool i = Validation.saveDoctor_ext("board_certification", "Board Certification", prof.board_certification, save_doc.id);
            }

           
            // hospital_affiliation
            if (!string.IsNullOrEmpty(prof.hospital_affiliation))
            {
                bool i = Validation.saveDoctor_ext("hospital_affiliation", "Hospital Affiliation", prof.hospital_affiliation, save_doc.id);
            }

            // personal_practice_type
            if (!string.IsNullOrEmpty(prof.personal_practice_type))
            {
                bool i = Validation.saveDoctor_ext("personal_practice_type", "Personal Practice Type", prof.personal_practice_type, save_doc.id);
            }

            // practice_npi
            if (!string.IsNullOrEmpty(prof.practice_npi))
            {
                bool i = Validation.saveDoctor_ext("practice_npi", "Practice NPI", prof.npi, save_doc.id);
            }

            // practice_name
            if (!string.IsNullOrEmpty(prof.practice_name))
            {
                bool i = Validation.saveDoctor_ext("practice_name", "Practice Name", prof.practice_name, save_doc.id);
            }

            // practice_type
            if (!string.IsNullOrEmpty(prof.practice_type))
            {
                bool i = Validation.saveDoctor_ext("practice_type", "Practice Type", prof.practice_type, save_doc.id);
            }

            // dea
            if (!string.IsNullOrEmpty(prof.dea))
            {
                bool i = Validation.saveDoctor_ext("dea", "DEA", prof.dea, save_doc.id);
            }

            // clinician_role
            if (!string.IsNullOrEmpty(prof.clinician_role))
            {
                bool i = Validation.saveDoctor_ext("clinician_role", "Clinician Role", prof.clinician_role, save_doc.id);
            }

            // scheduling_solution
            if (!string.IsNullOrEmpty(prof.scheduling_solution))
            {
                bool i = Validation.saveDoctor_ext("scheduling_solution", "Scheduling Solution", prof.scheduling_solution, save_doc.id);
            }

            // current_scheduling
            if (!string.IsNullOrEmpty(prof.current_scheduling))
            {
                bool i = Validation.saveDoctor_ext("current_scheduling", "Current Scheduling", prof.current_scheduling, save_doc.id);
            }

       

            // practice_phone_primary
            if (!string.IsNullOrEmpty(prof.practice_phone_primary))
            {
                bool i = Validation.saveDoctor_ext("practice_phone_primary", "Practice Phone Primary", prof.practice_phone_primary, save_doc.id);
            }

            // practice_phone_cs
            if (!string.IsNullOrEmpty(prof.practice_phone_cs))
            {
                bool i = Validation.saveDoctor_ext("practice_phone_cs", "Practice Customer Service Number", prof.practice_phone_cs, save_doc.id);
            }

            // practice_phone_office
            if (!string.IsNullOrEmpty(prof.practice_phone_office))
            {
                bool i = Validation.saveDoctor_ext("practice_phone_office", "Practice Phone Office", prof.practice_phone_office, save_doc.id);
            }

            // practice_clinicians
            if (!string.IsNullOrEmpty(prof.practice_clinicians))
            {
                bool i = Validation.saveDoctor_ext("practice_clinicians", "Practice Clinicians", prof.practice_clinicians, save_doc.id);
            }

            // practice_exams
            if (!string.IsNullOrEmpty(prof.practice_exams))
            {
                bool i = Validation.saveDoctor_ext("practice_exams", "Practice Exams", prof.practice_exams, save_doc.id);
            }

            // geographic_market
            if (!string.IsNullOrEmpty(prof.geographic_market))
            {
                bool i = Validation.saveDoctor_ext("geographic_market", "Geographic Market", prof.geographic_market, save_doc.id);
            }

            // practice_insurance
            if (!string.IsNullOrEmpty(prof.practice_insurance))
            {
                bool i = Validation.saveDoctor_ext("practice_insurance", "Practice Insurance", prof.practice_insurance, save_doc.id);
            }

            // practice_tax_number
            if (!string.IsNullOrEmpty(prof.practice_tax_number))
            {
                bool i = Validation.saveDoctor_ext("practice_tax_number", "Practice Tax Number", prof.practice_tax_number, save_doc.id);
            }

            // practice_emr
            if (!string.IsNullOrEmpty(prof.practice_emr))
            {
                bool i = Validation.saveDoctor_ext("practice_emr", "Practice EMR", prof.practice_emr, save_doc.id);
            }

            // network_insurance
            if (!string.IsNullOrEmpty(prof.network_insurance))
            {
                bool i = Validation.saveDoctor_ext("network_insurance", "Network Insurance", prof.network_insurance, save_doc.id);
            }

            // practice_contact
            if (!string.IsNullOrEmpty(prof.primary_contact_name))
            {
                bool i = Validation.saveDoctor_ext("primary_contact_name", "Practice Contact Name", prof.primary_contact_name, save_doc.id);
            }

            if (!string.IsNullOrEmpty(prof.primary_contact_email))
            {
                bool i = Validation.saveDoctor_ext("primary_contact_email", "primary Contact Email", prof.primary_contact_email.ToLower(), save_doc.id);
            }

            if (!string.IsNullOrEmpty(prof.primary_contact_phone))
            {
                bool i = Validation.saveDoctor_ext("primary_contact_phone", "primary Contact Phone", prof.primary_contact_phone, save_doc.id);
            }

            // OPERATIONAL CONTACT
            if (!string.IsNullOrEmpty(prof.operational_contact_name))
            {
                bool i = Validation.saveDoctor_ext("operational_contact_name", "operational_contact_name", prof.operational_contact_name, save_doc.id);
            }
            if (!string.IsNullOrEmpty(prof.operational_contact_email))
            {
                bool i = Validation.saveDoctor_ext("operational_contact_email", "Operational Contact Email", prof.operational_contact_email.ToLower(), save_doc.id);
            }

            if (!string.IsNullOrEmpty(prof.operational_contact_phone))
            {
                bool i = Validation.saveDoctor_ext("operational_contact_phone", "Operational Contact Phone", prof.operational_contact_phone, save_doc.id);
            }

            // education
            if (!string.IsNullOrEmpty(prof.education))
            {
                bool i = Validation.saveDoctor_ext("education", "Education", prof.education, save_doc.id);
            }

            // experience
            if (!string.IsNullOrEmpty(prof.experience))
            {
                bool i = Validation.saveDoctor_ext("experience", "Eperience", prof.experience, save_doc.id);
            }

            // FINANCIAL CONTACT
            if (!string.IsNullOrEmpty(prof.financial_contact_name))
            {
                bool i = Validation.saveDoctor_ext("financial_contact_name", "Financial Contact Name", prof.financial_contact_name, save_doc.id);
            }
            if (!string.IsNullOrEmpty(prof.financial_contact_email))
            {
                bool i = Validation.saveDoctor_ext("financial_contact_email", "Financial Contact Email", prof.financial_contact_email.ToLower(), save_doc.id);
            }

            if (!string.IsNullOrEmpty(prof.financial_contact_phone))
            {
                bool i = Validation.saveDoctor_ext("financial_contact_phone", "Financial Contact Phone", prof.financial_contact_phone, save_doc.id);
            }

            // billing_bankname
            if (!string.IsNullOrEmpty(prof.billing_bankname))
            {
                bool i = Validation.saveDoctor_ext("billing_bankname", "Billing Bankname", prof.billing_bankname, save_doc.id);
            }

            // billing_account
            if (!string.IsNullOrEmpty(prof.billing_account))
            {
                bool i = Validation.saveDoctor_ext("billing_account", "Billing Account", prof.billing_account, save_doc.id);
            }

            // billing_routing
            if (!string.IsNullOrEmpty(prof.billing_routing))
            {
                bool i = Validation.saveDoctor_ext("billing_routing", "Billing Routing", prof.billing_routing, save_doc.id);
            }

            // check if Doctor (claim your practice) is already reviewed.        
            bool i2 = Validation.saveDoctor_ext("is_reviewed", "Is Reviewed", "false", save_doc.id);

            return Json(new { data = new string[] { }, message = "Thank you, our HealthSplash team will contact you for more details.", success = true });
        }

        private bool saveDoctorUser(post_doctor_profile save_doc, string encryp, string salt)
        {
            // added code to save DOCTOR on USER
            var u = dbEntity.USERs.Where(a => a.username == save_doc.email);
            if (u.Count() > 0)
            {
                if (!string.IsNullOrEmpty(save_doc.first_name))
                    u.FirstOrDefault().name_first = save_doc.first_name;

                if (!string.IsNullOrEmpty(save_doc.last_name))
                    u.FirstOrDefault().name_last = save_doc.last_name;

                u.FirstOrDefault().username = save_doc.email; //email address?
                u.FirstOrDefault().phash = encryp;
                u.FirstOrDefault().tlas = salt;
                u.FirstOrDefault().rel_ref_status_id = 7;
                u.FirstOrDefault().rel_ref_USER_type_id = dbEntity.ref_USER_type.Where(a => a.dname.ToLower() == "doctor").FirstOrDefault().id;
                u.FirstOrDefault().create_by__USER_id = 0;
                u.FirstOrDefault().dt_create = dt;
                //dbEntity.USERs.Add(new_user);
                dbEntity.Entry(u.FirstOrDefault()).State = EntityState.Modified;
                dbEntity.SaveChanges();
            }
            else
            {
                USER new_user = new USER();

                new_user.name_first = save_doc.first_name;
                new_user.name_last = save_doc.last_name;
                new_user.username = save_doc.email.ToLower(); //email address?
                new_user.phash = encryp;
                new_user.tlas = salt;
                new_user.rel_ref_status_id = 7;
                new_user.rel_ref_USER_type_id = dbEntity.ref_USER_type.Where(a => a.dname.ToLower() == "doctor").FirstOrDefault().id;
                new_user.create_by__USER_id = 0;
                new_user.dt_create = dt;
                dbEntity.USERs.Add(new_user);
                dbEntity.SaveChanges();
            }


            return true;
        }


        private bool saveClaimDoctor_ext(string _attr_name, string _dname, string _value, long doc_id = 0)
        {
            // 01/04/2018: used by Doctor signup/login (HS-67)
            try
            {

                var d_ext = dbEntity.DOCTOR_ext.Where(a => a.attr_name == _attr_name && a.rel_DOCTOR_id == doc_id).FirstOrDefault();
                if (d_ext == null) // add attr if does not exist yet
                {
                    d_ext = new DOCTOR_ext();
                    d_ext.rel_DOCTOR_id = doc_id;
                    d_ext.attr_name = _attr_name;
                    d_ext.dname = _dname;
                    d_ext.value = _value;
                    d_ext.dt_create = dt;
                    d_ext.create_by__USER_id = 0;
                    dbEntity.DOCTOR_ext.Add(d_ext);
                    dbEntity.SaveChanges();
                }
                else // update the record if attr already exist
                {
                    //DOCTOR_ext d_ext = new DOCTOR_ext();
                    //var d_ext = dbEntity.DOCTOR_ext.Where(a => a.attr_name == _attr_name && a.rel_DOCTOR_id == doc_id).FirstOrDefault();

                    d_ext.attr_name = _attr_name;
                    d_ext.dname = _dname;
                    d_ext.value = _value;
                    d_ext.dt_update = dt;
                    d_ext.update_by__USER_id = 0;
                    dbEntity.Entry(d_ext).State = System.Data.Entity.EntityState.Modified;
                    dbEntity.SaveChanges();

                }
            }
            catch (Exception ex) { }


            return true;
        }

        private List<get_doctor_profile> _getDoctor_ext(IEnumerable<dynamic> doc, npi_registry reg)
        {

            List<get_doctor_profile> prof = new List<get_doctor_profile>();
            foreach (var n in doc)
            {
                long doctor_id = Convert.ToInt64(n.id);
                var doc_ext = dbEntity.hs_DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doctor_id);

                //List<doc_specialty> spec = new List<Models.doc_specialty>();
                List<doc_specialty2> spec1 = new List<doc_specialty2>();
                foreach (var dx in doc_ext)
                {
                    switch (dx.attr_name)
                    {
                        case "dob": reg.dob = dx.value; break;

                        case "gender": reg.gender = dx.value; break;
                        // case "email": _email = dx.value; break;

                        case "personal_practice_type": reg.personal_practice_type = dx.value; break;

                        // case "home_street1": _home_street1 = dx.value; break;
                        // case "home_street2": _home_street2 = dx.value; break;
                        // case "home_city": _home_city = dx.value; break;
                        // case "home_state": _home_state = dx.value; break;
                        case "home_zip":
                            reg.home_zip = dx.value;
                            string z = reg.home_zip.Substring(0, 5);
                            var h_zip = dbEntity.ref_zip.Where(a => a.zip == z);
                            reg.home_city = h_zip.FirstOrDefault().city_name; //_home_city = n.ref_zip == null ? "" :  n.ref_zip.city_name;
                            reg.home_state = h_zip.FirstOrDefault().city_state;  //_home_state = n.ref_zip == null ? "" : n.ref_zip.city_state;

                            break;

                        case "education": reg.education = dx.value; break;
                        case "experience": reg.experience = dx.value; break;

                        case "language_spoken": reg.language_spoken = dx.value; break;

                        case "board_certification": reg.board_certification = dx.value; break;

                        case "specialty":
                        case "specialty_id":
                            reg.specialty = dx.value;
                            //spec = null;
                            string[] get_spec = dx.value.Split(',');

                            foreach (var i in get_spec)
                            {

                                long spec_id = 0;
                                bool s1 = long.TryParse(i, out spec_id);

                                if (s1)
                                {
                                    var ref_spec = dbEntity.ref_specialty.Find(spec_id);

                                    //spec.Add(new doc_specialty
                                    //{
                                    //    id = ref_spec.id,
                                    //    code = ref_spec.code == null? "" : ref_spec.code,
                                    //    description = ref_spec.description ==null? "": ref_spec.description,
                                    //    name = ref_spec.name==null?"": ref_spec.name,
                                    //    actor = ref_spec.actor == null ? "" : ref_spec.actor
                                    //});

                                    if (ref_spec != null)
                                    {
                                        spec1.Add(new doc_specialty2
                                        {
                                            id = ref_spec.id,
                                            name = ref_spec.name,
                                            code = ref_spec.code,
                                            specialization = ref_spec.specialization,
                                            provider_type = ref_spec.provider_type
                                        });
                                    }
                                }

                            }

                            break;

                        case "practice_npi": reg.practice_npi = dx.value; break;

                        case "practice_name": reg.practice_name = dx.value; break;

                        case "practice_type": reg.practice_type = dx.value; break;

                        case "dea": reg.dea = dx.value; break;

                        case "clinician_role": reg.clinician_role = dx.value; break;

                        case "scheduling_solution": reg.scheduling_solution = dx.value; break;

                        case "current_scheduling": reg.current_scheduling = dx.value; break;

                        case "practice_street": reg.practice_street = dx.value; break;

                        case "practice_zip": // service zip codes
                            reg.practice_zip = dx.value;

                            break;
                        case "practice_phone_primary":// primary phone number
                            reg.practice_phone_primary = dx.value; break;

                        case "practice_fax": reg.practice_fax = dx.value; break;

                        case "practice_phone_cs": // customer service number
                            reg.practice_phone_cs = dx.value; break;

                        case "practice_phone_office":// office phone
                            reg.practice_phone_office = dx.value; break;

                        case "practice_clinicians": // no of field clinicians
                            reg.practice_clinicians = dx.value; break;

                        case "practice_exams": // no of exams you can handle per week
                            reg.practice_exams = dx.value; break;

                        case "geographic_market": // geographic market
                            reg.geographic_market = dx.value; break;

                        case "practice_expansion": // future expansion plans, new market, 
                            reg.practice_expansion = dx.value; break;

                        case "practice_insurance":// insurance list you are in Network or will acccept
                            reg.practice_insurance = dx.value; break;

                        case "practice_tax_number":// federal tax id number
                            reg.practice_tax_number = dx.value; break;

                        case "primary_contact_name":
                            reg.primary_contact_name = dx.value;
                            break;

                        case "primary_contact_phone":
                            reg.primary_contact_phone = dx.value;
                            break;

                        case "primary_contact_email":
                            reg.primary_contact_email = dx.value;
                            break;

                        case "operational_contact_name":
                            reg.operational_contact_name = dx.value;
                            break;

                        case "operational_contact_phone": reg.operational_contact_phone = dx.value; break;

                        case "operational_contact_email": reg.operational_contact_email = dx.value; break;

                        case "financial_contact_name": reg.financial_contact_name = dx.value; break;

                        case "financial_contact_phone": reg.financial_contact_phone = dx.value; break;

                        case "financial_contact_email": reg.financial_contact_email = dx.value; break;

                        case "practice_emr":  // emr software that you are currently using
                            reg.practice_emr = dx.value; break;

                        case "network_insurance":// in-network insurances
                            reg.network_insurance = dx.value; break;

                        // public string practice_contact { get; set; } // primary contact/ operational contact/ financial contact
                        case "billing_bankname": reg.billing_bankname = dx.value; break;
                        case "billing_account": reg.billing_account = dx.value; break;
                        case "billing_routing": reg.billing_routing = dx.value; break;

                    }

                }

                List<zip_search_address> home = new List<zip_search_address>();
                home = _getnpi_Homeaddress(n.home_addr_1, n.home_addr_2, reg.home_zip);
                List<zip_search_address> prac_add = new List<zip_search_address>();
                prac_add = _getnpi_Homeaddress(reg.practice_street, "", reg.practice_zip);


                prof.Add(new get_doctor_profile
                {
                    id = n.id,
                    npi = n.NPI,
                    first_name = n.name_first,
                    last_name = n.name_last,
                    middle_name = n.name_middle == null ? "" : n.name_middle,
                    title = n.title == null ? "" : n.title,
                    gender = n.gender == null ? "" : n.gender,
                    email = n.email == null ? "" : n.email,
                    //home_street1 = n.home_addr_1 == null ? "" : n.home_addr_1,
                    //home_street2 = n.home_addr_2 == null ? "" : n.home_addr_2,
                    //home_zip = reg.home_zip,
                    dob = reg.dob,
                    //home_city = reg.home_city,
                    //home_state = reg.home_state,
                    education = reg.education == null ? "" : reg.education,
                    personal_practice_type = reg.personal_practice_type == null ? "" : reg.personal_practice_type,
                    board_certification = reg.board_certification == null ? "" : reg.board_certification,

                    specialty = spec1,
                    practice_npi = reg.practice_npi == null ? "" : reg.practice_npi,
                    practice_name = reg.practice_name == null ? "" : reg.practice_name,
                    practice_type = reg.practice_type == null ? "" : reg.practice_type,
                    dea = reg.dea == null ? "" : reg.dea,
                    clinician_role = reg.clinician_role == null ? "" : reg.clinician_role,
                    scheduling_solution = reg.scheduling_solution == null ? "" : reg.scheduling_solution,
                    current_scheduling = reg.current_scheduling == null ? "" : reg.current_scheduling,
                    //practice_street = reg.practice_street == null ? "" : reg.practice_street,
                    //practice_zip = reg.practice_zip == null ? "" : reg.practice_zip,
                    practice_phone_primary = reg.practice_phone_primary == null ? "" : reg.practice_phone_primary,
                    practice_fax = reg.practice_fax == null ? "" : reg.practice_fax,
                    practice_phone_cs = reg.practice_phone_cs == null ? "" : reg.practice_phone_cs,
                    practice_phone_office = reg.practice_phone_office == null ? "" : reg.practice_phone_office,
                    practice_clinicians = reg.practice_clinicians == null ? "" : reg.practice_clinicians,

                    practice_exams = reg.practice_exams == null ? "" : reg.practice_exams,
                    geographic_market = reg.geographic_market == null ? "" : reg.geographic_market,
                    practice_expansion = reg.practice_expansion == null ? "" : reg.practice_expansion,
                    practice_insurance = reg.practice_insurance == null ? "" : reg.practice_insurance,
                    practice_tax_number = reg.practice_tax_number == null ? "" : reg.practice_tax_number,
                    primary_contact_name = reg.primary_contact_name == null ? "" : reg.primary_contact_name,
                    primary_contact_phone = reg.primary_contact_phone == null ? "" : reg.primary_contact_phone,
                    primary_contact_email = reg.primary_contact_email == null ? "" : reg.primary_contact_email,
                    operational_contact_name = reg.operational_contact_name == null ? "" : reg.operational_contact_name,
                    operational_contact_phone = reg.operational_contact_phone == null ? "" : reg.operational_contact_phone,
                    operational_contact_email = reg.operational_contact_email == null ? "" : reg.operational_contact_email,
                    financial_contact_name = reg.financial_contact_name == null ? "" : reg.financial_contact_name,
                    financial_contact_phone = reg.financial_contact_phone == null ? "" : reg.financial_contact_phone,
                    financial_contact_email = reg.financial_contact_email == null ? "" : reg.financial_contact_email,
                    practice_emr = reg.practice_emr == null ? "" : reg.practice_emr,
                    billing_bankname = reg.billing_bankname == null ? "" : reg.billing_bankname,
                    billing_account = reg.billing_account == null ? "" : reg.billing_account,
                    billing_routing = reg.billing_routing == null ? "" : reg.billing_routing,

                    network_insurance = new List<insurance>() { }, //reg.network_insurance == null ? "" : reg.network_insurance,
                    language_spoken = new List<doc_language>() { }, //reg.language_spoken == null ? new List<doc_language>() { } : reg.language_spoken,
                    home_address = home == null ? new List<zip_search_address>() { } : home,
                    practice_address = prac_add == null ? new List<zip_search_address>() { } : prac_add
                });

            }



            return prof;
        }

        private IHttpActionResult convert_get_post_doctor(get_doctor_profile prof)
        {
            post_doctor_profile prof1 = new post_doctor_profile();
            string post_spec = "";
            foreach (var s in prof.specialty)
            {
                post_spec += s.id + ",";
            }
            post_spec = post_spec.Substring(0, post_spec.Length - 1);

            //List<zip_search_address> home = new List<zip_search_address>();
            //home = _getnpi_Homeaddress(prof.ho, prof.home_street2, prof.home_zip);
            //List<zip_search_address> prac_add = new List<zip_search_address>();
            //prac_add = _getnpi_Homeaddress(addr_practice["address_1"].ToString(), addr_practice["address_2"].ToString(), addr_practice["postal_code"].ToString());

            prof1 = new post_doctor_profile
            {
                npi = prof.npi, // i[0]["number"],
                first_name = prof.first_name, // per_profile["first_name"],
                last_name = prof.last_name, // per_profile["last_name"],
                middle_name = prof.middle_name, // per_profile["middle_name"] == null ? "" : per_profile["middle_name"],
                title = "", //n.title == null ? "" : n.title,
                gender = prof.gender, // per_profile["gender"] == null ? "" : per_profile["gender"],
                email = "", // n.email == null ? "" : n.email,
                //home_street1 = prof.home_street1, // addr_home["address_1"] == null ? "" : addr_home["address_1"],
                //home_street2 = prof.home_street2, // addr_home["address_2"] == null ? "" : addr_home["address_2"],
                //home_zip = prof.home_zip, // addr_home["postal_code"],
                dob = prof.dob, // _dob,
                //home_city = prof.home_city, // addr_home["city"],
                //home_state = prof.home_state, // _home_state,
                education = prof.education, // _education,
                personal_practice_type = prof.personal_practice_type, // _personal_practice_type,
                board_certification = prof.board_certification, // _board_certification,
                //specialty = post_spec,
                practice_npi = prof.practice_npi, //_practice_npi,
                practice_name = prof.practice_name,
                practice_type = prof.practice_type,
                dea = prof.dea,
                clinician_role = prof.clinician_role,
                scheduling_solution = prof.scheduling_solution,
                current_scheduling = prof.current_scheduling,
                //practice_street = prof.practice_street,
                //practice_zip = prof.practice_zip,
                practice_phone_primary = prof.practice_phone_primary,
                practice_fax = prof.practice_fax,
                practice_phone_cs = prof.practice_phone_cs,
                practice_phone_office = prof.practice_phone_office,
                practice_clinicians = prof.practice_clinicians,

                practice_exams = prof.practice_exams,
                geographic_market = prof.geographic_market,
                practice_expansion = prof.practice_expansion,
                practice_insurance = prof.practice_insurance,
                practice_tax_number = prof.practice_tax_number,
                primary_contact_name = prof.primary_contact_name,
                primary_contact_phone = prof.primary_contact_phone,
                primary_contact_email = prof.primary_contact_email,
                operational_contact_name = prof.operational_contact_name,
                operational_contact_phone = prof.operational_contact_phone,
                operational_contact_email = prof.operational_contact_email,
                financial_contact_name = prof.financial_contact_name,
                financial_contact_phone = prof.financial_contact_phone,
                financial_contact_email = prof.financial_contact_email,
                practice_emr = prof.practice_emr,
                billing_bankname = prof.billing_bankname,
                billing_account = prof.billing_account,
                billing_routing = prof.billing_routing,

                
                //network_insurance = prof.network_insurance,
                //language_spoken = new List<doc_language>() { }// prof.language_spoken, // _language_spoken,

            };

            return saveDoctorProfile(prof1);

        }

        //private bool validateDate(string attr_name, string dname, string dat, long doc_id)
        //{
        //    //12/12/1990
        //    string[] formats = {"M/d/yyyy", "MM/dd/yyyy","M/dd/yyyy",
        //        "M-d-yyyy", "MM-dd-yyyy","M-dd-yyyy" };

        //    DateTime dateVal;

        //    bool i = DateTime.TryParseExact(dat, formats,
        //        new CultureInfo("en-US"),
        //        System.Globalization.DateTimeStyles.None, out dateVal);

        //    if (i) {
        //        return saveDoctor_ext(attr_name, dname, dateVal.ToString().Split(' ')[0], doc_id);
        //    }
        //    return false;

        //}

        private string getTrim2()
        {
            var db = from a in dbEntity.ref_specialty
                     select a;

            string lvl = "";
            foreach (var n in db)
            {
                string[] name = n.name.Split('-');
                //n.code = n.code.Trim();
                //n.name = name[0].Trim();
                if (n.code.Contains("Lev"))
                {
                    lvl = n.name;
                }
                //n.level = lvl;
                n.group = lvl;
                //n.provider_type = n.provider_type.Trim();
                //n.specialization = n.specialization.Trim();
                dbEntity.Entry(n).State = System.Data.Entity.EntityState.Modified;

            }

            dbEntity.SaveChanges();
            return "";
        }

        //private const string ServiceBaseURL  = "http://localhost:64625/";
        //private HttpResponseMessage _response;
        //[TestCase(null)]
        //public void getAllProductsTest(string npi) {
        //    var productController = new DoctorController()
        //    {
        //        Request = new HttpRequestMessage
        //        {
        //            Method = HttpMethod.Get,
        //            RequestUri = new Uri (ServiceBaseURL + "doctor/claim ")
        //        }
        //    };

        //    productController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
        //    _response = productController.getDoctorclaim1(null);
        //    var responseResult = JsonConvert.SerializeObject(_response);

        //    Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
        //    Assert.AreEqual(responseResult.Any(), true);
        //}


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("doctor/claim1")] // doctor
        //public IHttpActionResult getDoctorclaim(string npi = null)
        private HttpResponseMessage getDoctorclaim1(string npi = null)
        {
            //  Add_SingleNumbers_ReturnsTheNumber();
            //  TestUnit.testdemo.add_invalid_string();
            //if (!authorize) return Json(new { data = new string[] { }, message = "not authorized", success = false });

            //string v = httpCon.Request.Headers["Authorization"];
            string msg = "The authorization header is not valid.";

            //if (Validation.userAuth(v))
            //{

            //string s = getTrim2();
            IsRequired("npi", npi, 2);
            if (haserror)
            {
                //return Json(new { data = new string[] { }, message = errmsg, success = false });
                return Request.CreateResponse(HttpStatusCode.BadRequest, "error1");
            }

            var doc = dbEntity.DOCTORs.Where(a => a.NPI == npi);

            try
            {

                List<get_doctor_profile> prof = new List<get_doctor_profile>();
                List<doc_specialty2> spec = new List<Models.doc_specialty2>();
                // _email="", _home_street1 = "", _home_street2 = "", _home_zip = "",

                npi_registry reg_doc = new npi_registry();

                if (doc.Count() == 0)
                {
                    // fetch from NPI registry

                    var uri = "https://npiregistry.cms.hhs.gov/api?number=" + npi;
                    System.Net.WebRequest req = System.Net.WebRequest.Create(uri);
                    System.Net.WebResponse resp = req.GetResponse();
                    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                    var x = sr.ReadToEnd().Trim();

                    dynamic ret2 = JsonConvert.DeserializeObject(x);
                    int cnt = Convert.ToInt32(ret2["result_count"]);
                    if (cnt == 0)
                    {
                        //return Json(new { data = new string[] { }, message = "No record found.", success = false });
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "no record found1.");
                    }

                    var i = ret2["results"];
                    var res = i[0]["taxonomies"]; //JsonConvert.DeserializeObject(ret2["results"].ToString());
                    var addr_home = i[0]["addresses"][0];
                    var addr_practice = i[0]["addresses"][1];
                    var per_profile = i[0]["basic"];
                    var npi1 = i[0]["number"];
                    var addr1 = addr_home["address_1"];



                    string spec_code = res[0]["code"].ToString();
                    var spec1 = dbEntity.ref_specialty.Where(b => b.code == spec_code);

                    //spec.Add(
                    //    new doc_specialty {
                    //        id = spec1.FirstOrDefault().id,
                    //        description  = spec1.FirstOrDefault().description,
                    //        code = spec1.FirstOrDefault().code,
                    //        actor = spec1.FirstOrDefault().actor == null?"": spec1.FirstOrDefault().actor,
                    //        name = ""
                    //    });
                    spec.Add(new doc_specialty2
                    {
                        id = spec1.FirstOrDefault().id,
                        name = spec1.FirstOrDefault().name,
                        code = spec1.FirstOrDefault().code,
                        specialization = spec1.FirstOrDefault().specialization,
                        provider_type = spec1.FirstOrDefault().provider_type
                    });


                    List<zip_search_address> home = new List<zip_search_address>();
                    home = _getnpi_Homeaddress(addr_home["address_1"].ToString(), addr_home["address_2"].ToString(), addr_home["postal_code"].ToString());
                    List<zip_search_address> prac_add = new List<zip_search_address>();
                    prac_add = _getnpi_Homeaddress(addr_practice["address_1"].ToString(), addr_practice["address_2"].ToString(), addr_practice["postal_code"].ToString());

                    #region "get_doctor_profile"
                    prof.Add(new get_doctor_profile
                    {
                        id = 0,
                        npi = i[0]["number"],
                        first_name = per_profile["first_name"],
                        last_name = per_profile["last_name"],
                        middle_name = per_profile["middle_name"] == null ? "" : per_profile["middle_name"],
                        title = "", //n.title == null ? "" : n.title,
                        gender = per_profile["gender"] == null ? "" : per_profile["gender"],
                        email = "", // n.email == null ? "" : n.email,
                        //home_street1 = addr_home["address_1"] == null ? "" : addr_home["address_1"],
                        //home_street2 = addr_home["address_2"] == null ? "" : addr_home["address_2"],
                        //home_zip = addr_home["postal_code"] == null ? "" : addr_home["postal_code"],
                        dob = reg_doc.dob,
                        //home_city = addr_home["city"] == null ? "" : addr_home["city"],
                        //home_state = addr_home["state"] == null ? "" : addr_home["state"],
                        education = reg_doc.education,
                        personal_practice_type = reg_doc.personal_practice_type,
                        board_certification = reg_doc.board_certification,
                        specialty = spec,
                        hospital_affiliation = reg_doc.hospital_affiliation,
                        practice_npi = reg_doc.practice_npi,
                        practice_name = reg_doc.practice_name,
                        practice_type = reg_doc.practice_type,
                        dea = reg_doc.dea,
                        clinician_role = reg_doc.clinician_role,
                        scheduling_solution = reg_doc.scheduling_solution,
                        current_scheduling = reg_doc.current_scheduling,
                        //practice_street = addr_practice["address_1"] + " " + addr_practice["address_2"],
                        //practice_zip = addr_practice["postal_code"],
                        practice_phone_primary = addr_practice["telephone_number"],
                        practice_fax = reg_doc.practice_fax,
                        practice_phone_cs = reg_doc.practice_phone_cs,
                        practice_phone_office = reg_doc.practice_phone_office,
                        practice_clinicians = reg_doc.practice_clinicians,

                        practice_exams = reg_doc.practice_exams,
                        geographic_market = reg_doc.geographic_market,
                        practice_expansion = reg_doc.practice_expansion,
                        practice_insurance = reg_doc.practice_insurance,
                        practice_tax_number = reg_doc.practice_tax_number,
                        primary_contact_name = reg_doc.primary_contact_name,
                        primary_contact_phone = reg_doc.primary_contact_phone,
                        primary_contact_email = reg_doc.primary_contact_email,
                        operational_contact_name = reg_doc.operational_contact_name,
                        operational_contact_phone = reg_doc.operational_contact_phone,
                        operational_contact_email = reg_doc.operational_contact_email,
                        financial_contact_name = reg_doc.financial_contact_name,
                        financial_contact_phone = reg_doc.financial_contact_phone,
                        financial_contact_email = reg_doc.financial_contact_email,
                        practice_emr = reg_doc.practice_emr,
                        //network_insurance = reg_doc.network_insurance,
                        billing_bankname = reg_doc.billing_bankname,
                        billing_account = reg_doc.billing_account,
                        billing_routing = reg_doc.billing_routing,

                        language_spoken = new List<doc_language>() { }, //reg_doc.language_spoken,

                        home_address = home == null ? new List<zip_search_address>() { } : home,
                        practice_address = prac_add == null ? new List<zip_search_address>() { } : prac_add
                    });
                    #endregion

                    // save Doctor profile here
                    // IHttpActionResult save = convert_get_post_doctor(prof[0]);
                    if (prof.Count() > 0)
                        // return Json(new { data = prof, message = "", success = true });
                        return Request.CreateResponse(HttpStatusCode.OK, "no record found2.");
                    else
                        //return Json(new { data = new string[] { }, message = "No record found.", success = false });
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "no record found3.");
                }




                foreach (var n in doc)
                {
                    // string home_zip = "";// n.ref_zip == null ? "" : n.ref_zip.zip;
                    //_home_city = n.ref_zip == null ? "" :  n.ref_zip.city_name;
                    //_home_state = n.ref_zip == null ? "" : n.ref_zip.city_state;

                    var doc_ext = dbEntity.DOCTOR_ext.Where(a => a.rel_DOCTOR_id == n.id);

                    //List<doc_specialty> spec = new List<Models.doc_specialty>();
                    List<doc_specialty2> spec1 = new List<doc_specialty2>();
                    foreach (var dx in doc_ext)
                    {
                        switch (dx.attr_name)
                        {
                            case "dob": reg_doc.dob = dx.value; break;

                            case "gender": reg_doc.gender = dx.value; break;
                            // case "email": _email = dx.value; break;

                            case "personal_practice_type": reg_doc.personal_practice_type = dx.value; break;

                            // case "home_street1": _home_street1 = dx.value; break;
                            // case "home_street2": _home_street2 = dx.value; break;
                            // case "home_city": _home_city = dx.value; break;
                            // case "home_state": _home_state = dx.value; break;
                            case "home_zip":
                                reg_doc.home_zip = dx.value;
                                string z = reg_doc.home_zip.Substring(0, 5);
                                var h_zip = dbEntity.ref_zip.Where(a => a.zip == z);
                                reg_doc.home_city = h_zip.FirstOrDefault().city_name; //_home_city = n.ref_zip == null ? "" :  n.ref_zip.city_name;
                                reg_doc.home_state = h_zip.FirstOrDefault().city_state;  //_home_state = n.ref_zip == null ? "" : n.ref_zip.city_state;

                                break;

                            case "education": reg_doc.education = dx.value; break;

                            case "language_spoken": reg_doc.language_spoken = dx.value; break;

                            case "board_certification": reg_doc.board_certification = dx.value; break;

                            case "specialty":
                            case "specialty_id":
                                reg_doc.specialty = dx.value;
                                //spec = null;
                                string[] get_spec = dx.value.Split(',');

                                foreach (var i in get_spec)
                                {

                                    long spec_id = 0;
                                    bool s1 = long.TryParse(i, out spec_id);

                                    if (s1)
                                    {
                                        var ref_spec = dbEntity.ref_specialty.Find(spec_id);

                                        //spec.Add(new doc_specialty
                                        //{
                                        //    id = ref_spec.id,
                                        //    code = ref_spec.code == null? "" : ref_spec.code,
                                        //    description = ref_spec.description ==null? "": ref_spec.description,
                                        //    name = ref_spec.name==null?"": ref_spec.name,
                                        //    actor = ref_spec.actor == null ? "" : ref_spec.actor
                                        //});
                                        spec1.Add(new doc_specialty2
                                        {
                                            id = ref_spec.id,
                                            name = ref_spec.name,
                                            code = ref_spec.code,
                                            specialization = ref_spec.specialization,
                                            provider_type = ref_spec.provider_type
                                        });

                                    }

                                }

                                break;

                            case "practice_npi": reg_doc.practice_npi = dx.value; break;

                            case "practice_name": reg_doc.practice_name = dx.value; break;

                            case "practice_type": reg_doc.practice_type = dx.value; break;

                            case "dea": reg_doc.dea = dx.value; break;

                            case "clinician_role": reg_doc.clinician_role = dx.value; break;

                            case "scheduling_solution": reg_doc.scheduling_solution = dx.value; break;

                            case "current_scheduling": reg_doc.current_scheduling = dx.value; break;

                            case "hospital_affiliation": reg_doc.hospital_affiliation = dx.value; break;
                            case "practice_street": reg_doc.practice_street = dx.value; break;

                            case "practice_zip": // service zip codes
                                reg_doc.practice_zip = dx.value;

                                break;
                            case "practice_phone_primary":// primary phone number
                                reg_doc.practice_phone_primary = dx.value; break;

                            case "practice_fax": reg_doc.practice_fax = dx.value; break;

                            case "practice_phone_cs": // customer service number
                                reg_doc.practice_phone_cs = dx.value; break;

                            case "practice_phone_office":// office phone
                                reg_doc.practice_phone_office = dx.value; break;

                            case "practice_clinicians": // no of field clinicians
                                reg_doc.practice_clinicians = dx.value; break;

                            case "practice_exams": // no of exams you can handle per week
                                reg_doc.practice_exams = dx.value; break;

                            case "geographic_market": // geographic market
                                reg_doc.geographic_market = dx.value; break;

                            case "practice_expansion": // future expansion plans, new market, 
                                reg_doc.practice_expansion = dx.value; break;

                            case "practice_insurance":// insurance list you are in Network or will acccept
                                reg_doc.practice_insurance = dx.value; break;

                            case "practice_tax_number":// federal tax id number
                                reg_doc.practice_tax_number = dx.value; break;

                            case "primary_contact_name":
                                reg_doc.primary_contact_name = dx.value;
                                break;

                            case "primary_contact_phone":
                                reg_doc.primary_contact_phone = dx.value;
                                break;

                            case "primary_contact_email":
                                reg_doc.primary_contact_email = dx.value;
                                break;

                            case "operational_contact_name":
                                reg_doc.operational_contact_name = dx.value;
                                break;

                            case "operational_contact_phone": reg_doc.operational_contact_phone = dx.value; break;

                            case "operational_contact_email": reg_doc.operational_contact_email = dx.value; break;

                            case "financial_contact_name": reg_doc.financial_contact_name = dx.value; break;

                            case "financial_contact_phone": reg_doc.financial_contact_phone = dx.value; break;

                            case "financial_contact_email": reg_doc.financial_contact_email = dx.value; break;

                            case "practice_emr":  // emr software that you are currently using
                                reg_doc.practice_emr = dx.value; break;

                            case "network_insurance":// in-network insurances
                                reg_doc.network_insurance = dx.value; break;

                            // public string practice_contact { get; set; } // primary contact/ operational contact/ financial contact
                            case "billing_bankname": reg_doc.billing_bankname = dx.value; break;
                            case "billing_account": reg_doc.billing_account = dx.value; break;
                            case "billing_routing": reg_doc.billing_routing = dx.value; break;

                        }

                    }
                    List<zip_search_address> home = new List<zip_search_address>();
                    home = _getnpi_Homeaddress(n.practice_addr_1, n.practice_addr_2, reg_doc.home_zip);
                    List<zip_search_address> prac_add = new List<zip_search_address>();
                    prac_add = _getnpi_Homeaddress(reg_doc.practice_street, "", reg_doc.practice_zip);


                    prof.Add(new get_doctor_profile
                    {
                        id = n.id,
                        npi = n.NPI,
                        first_name = n.name_first,
                        last_name = n.name_last,
                        middle_name = n.name_middle == null ? "" : n.name_middle,
                        title = n.title == null ? "" : n.title,
                        gender = n.gender == null ? "" : n.gender,
                        email = n.email == null ? "" : n.email,
                        //home_street1 = n.addr_address1 == null ? "" : n.addr_address1,
                        //home_street2 = n.addr_address2 == null ? "" : n.addr_address2,
                        //home_zip = reg_doc.home_zip,
                        dob = reg_doc.dob,
                        //home_city = reg_doc.home_city,
                        //home_state = reg_doc.home_state,
                        education = reg_doc.education,
                        //language_spoken = reg_doc.language_spoken,
                        personal_practice_type = reg_doc.personal_practice_type,
                        board_certification = reg_doc.board_certification,
                        specialty = spec1,
                        practice_npi = reg_doc.practice_npi,
                        practice_name = reg_doc.practice_name,
                        practice_type = reg_doc.practice_type,
                        dea = reg_doc.dea,
                        clinician_role = reg_doc.clinician_role,
                        scheduling_solution = reg_doc.scheduling_solution,
                        current_scheduling = reg_doc.current_scheduling,

                        hospital_affiliation = reg_doc.hospital_affiliation,
                        //practice_street = reg_doc.practice_street,
                        //practice_zip = reg_doc.practice_zip,
                        practice_phone_primary = reg_doc.practice_phone_primary,
                        practice_fax = reg_doc.practice_fax,
                        practice_phone_cs = reg_doc.practice_phone_cs,
                        practice_phone_office = reg_doc.practice_phone_office,
                        practice_clinicians = reg_doc.practice_clinicians,

                        practice_exams = reg_doc.practice_exams,
                        geographic_market = reg_doc.geographic_market,
                        practice_expansion = reg_doc.practice_expansion,
                        practice_insurance = reg_doc.practice_insurance,
                        practice_tax_number = reg_doc.practice_tax_number,
                        primary_contact_name = reg_doc.primary_contact_name,
                        primary_contact_phone = reg_doc.primary_contact_phone,
                        primary_contact_email = reg_doc.primary_contact_email,
                        operational_contact_name = reg_doc.operational_contact_name,
                        operational_contact_phone = reg_doc.operational_contact_phone,
                        operational_contact_email = reg_doc.operational_contact_email,
                        financial_contact_name = reg_doc.financial_contact_name,
                        financial_contact_phone = reg_doc.financial_contact_phone,
                        financial_contact_email = reg_doc.financial_contact_email,
                        practice_emr = reg_doc.practice_emr,
                        //network_insurance = reg_doc.network_insurance,
                        billing_bankname = reg_doc.billing_bankname,
                        billing_account = reg_doc.billing_account,
                        billing_routing = reg_doc.billing_routing,

                        language_spoken = new List<doc_language>() { },
                        network_insurance = new List<insurance>() { },
                        home_address = home == null ? new List<zip_search_address>() { } : home,
                        practice_address = prac_add == null ?  new List<zip_search_address>() { } : prac_add
                    });
                }

                if (prof.Count() > 0)
                {
                    var ret1 = JsonConvert.SerializeObject(prof);
                    var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                    //return Json(new { data = json1, message = "", success = true });
                    return Request.CreateResponse(HttpStatusCode.BadRequest, json1);
                }
                else
                {
                    msg = "Invalid NPI.";
                    //return Json(new { data = new string[] { }, message = msg, success = false });
                    return Request.CreateResponse(HttpStatusCode.BadRequest, msg);
                }
            }
            catch (Exception ex)
            {
                //return Json(new { data = new string[] { }, message = ex.Message, success = false });
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            //}
            ////return Json(new { data = new string[] { }, message = msg, success = false });
            //return Request.CreateResponse(HttpStatusCode.BadRequest, "no record found4.");
        }

        private hs_DOCTOR mdoc_address;
        public hs_DOCTOR doc_address
        {
            get { return mdoc_address; }
            set { mdoc_address = value; }
        }

        
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("doctor/claim")] // doctor
        //public IHttpActionResult getDoctorclaim(string npi = null)x
        public IHttpActionResult getDoctorclaim(string npi = null)
        {

            //if (!authorize) return Json(new { data = new string[] { }, message = "not authorized", success = false });

            //string v = httpCon.Request.Headers["Authorization"];
            string msg = "The authorization header is not valid.";

            //if (Validation.userAuth(v))
            //{

            //string s = getTrim2();
            IsRequired("npi", npi, 2);
            if (haserror)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }

            var doctor = dbEntity.hs_DOCTOR.Where(a => a.NPI == npi);

            try
            {

                List<get_doctor_profile> prof = new List<get_doctor_profile>();
                List<doc_specialty2> spec = new List<Models.doc_specialty2>();
                // _email="", _home_street1 = "", _home_street2 = "", _home_zip = "",

                npi_registry reg_doc = new npi_registry();

                if (doctor.Count() == 0)
                {
                    // fetch from NPI registry

                    var uri = "https://npiregistry.cms.hhs.gov/api?number=" + npi;
                    System.Net.WebRequest req = System.Net.WebRequest.Create(uri);
                    System.Net.WebResponse resp = req.GetResponse();
                    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                    var x = sr.ReadToEnd().Trim();

                    dynamic ret2 = JsonConvert.DeserializeObject(x);
                    int cnt = Convert.ToInt32(ret2["result_count"]);
                    if (cnt == 0)
                    {
                        return Json(new { data = new string[] { }, message = "No record found.", success = false });
                    }

                    var i = ret2["results"];
                    var res = i[0]["taxonomies"]; //JsonConvert.DeserializeObject(ret2["results"].ToString());
                    var addr_home = i[0]["addresses"][0];
                    var addr_practice = i[0]["addresses"][1];
                    var per_profile = i[0]["basic"];
                    var npi1 = i[0]["number"];
                    var addr1 = addr_home["address_1"];



                    string spec_code = res[0]["code"].ToString();
                    //var spec1 = dbEntity.ref_specialty.Where(b => b.code == spec_code);

                    if (!string.IsNullOrEmpty(spec_code))
                    {
                        spec.Add(_getSpecialtyByCode(spec_code));
                    }

                    List<zip_search_address> home = new List<zip_search_address>();
                    home =_getnpi_Homeaddress(addr_home["address_1"].ToString(), addr_home["address_2"].ToString(), addr_home["postal_code"].ToString());
                    List<zip_search_address> prac_add = new List<zip_search_address>();
                    prac_add = _getnpi_Homeaddress(addr_practice["address_1"].ToString(), addr_practice["address_2"].ToString(), addr_practice["postal_code"].ToString());


                    #region "get_doctor_profile"
                    prof.Add(new get_doctor_profile
                    {
                        id = 0,
                        npi = i[0]["number"],
                        first_name = per_profile["first_name"],
                        last_name = per_profile["last_name"],
                        middle_name = per_profile["middle_name"] == null ? "" : per_profile["middle_name"],
                        title = "", //n.title == null ? "" : n.title,
                        gender = per_profile["gender"] == null ? "" : per_profile["gender"],
                        email = "", // n.email == null ? "" : n.email,
                        //home_street1 = addr_home["address_1"] == null ? "" : addr_home["address_1"],
                        //home_street2 = addr_home["address_2"] == null ? "" : addr_home["address_2"],
                        //home_zip = addr_home["postal_code"] == null ? "" : addr_home["postal_code"],
                        dob = reg_doc.dob,
                        // = addr_home["city"] == null ? "" : addr_home["city"],
                        //home_state = addr_home["state"] == null ? "" : addr_home["state"],
                        education = reg_doc.education == null ?"" : reg_doc.education,
                        personal_practice_type = reg_doc.personal_practice_type == null? "" : reg_doc.personal_practice_type,
                        board_certification = reg_doc.board_certification == null? "" : reg_doc.board_certification,
                        
                        practice_npi = reg_doc.practice_npi ==null? "": reg_doc.practice_npi,
                        practice_name = reg_doc.practice_name ==null? "" : reg_doc.practice_name,
                        practice_type = reg_doc.practice_type ==null? "" : reg_doc.practice_type,
                        dea = reg_doc.dea == null ? "" : reg_doc.dea,
                        clinician_role = reg_doc.clinician_role == null ? "" : reg_doc.clinician_role,
                        scheduling_solution = reg_doc.scheduling_solution == null ? "" : reg_doc.scheduling_solution,
                        current_scheduling = reg_doc.current_scheduling == null ? "" : reg_doc.current_scheduling,
                        //practice_street = addr_practice["address_1"] + " " + addr_practice["address_2"],
                        //practice_zip = addr_practice["postal_code"],
                        practice_phone_primary = addr_practice["telephone_number"],
                        practice_fax = reg_doc.practice_fax == null ? "" : reg_doc.practice_fax,
                        practice_phone_cs = reg_doc.practice_phone_cs == null ? "" : reg_doc.practice_phone_cs,
                        practice_phone_office = reg_doc.practice_phone_office == null ? "" : reg_doc.practice_phone_office,
                        practice_clinicians = reg_doc.practice_clinicians == null ? "" : reg_doc.practice_clinicians,
                        
                        practice_exams = reg_doc.practice_exams == null ? "" : reg_doc.practice_exams,
                        geographic_market = reg_doc.geographic_market == null ? "" : reg_doc.geographic_market,
                        practice_expansion = reg_doc.practice_expansion == null ? "" : reg_doc.practice_expansion,
                        practice_insurance = reg_doc.practice_insurance == null ? "" : reg_doc.practice_insurance,
                        practice_tax_number = reg_doc.practice_tax_number == null ? "" : reg_doc.practice_tax_number,
                        primary_contact_name = reg_doc.primary_contact_name == null ? "" : reg_doc.primary_contact_name,
                        primary_contact_phone = reg_doc.primary_contact_phone == null ? "" : reg_doc.primary_contact_phone,
                        primary_contact_email = reg_doc.primary_contact_email == null ? "" : reg_doc.primary_contact_email,
                        operational_contact_name = reg_doc.operational_contact_name == null ? "" : reg_doc.operational_contact_name,
                        operational_contact_phone = reg_doc.operational_contact_phone == null ? "" : reg_doc.operational_contact_phone,
                        operational_contact_email = reg_doc.operational_contact_email == null ? "" : reg_doc.operational_contact_email,
                        financial_contact_name = reg_doc.financial_contact_name == null ? "" : reg_doc.financial_contact_name,
                        financial_contact_phone = reg_doc.financial_contact_phone == null ? "" : reg_doc.financial_contact_phone,
                        financial_contact_email = reg_doc.financial_contact_email == null ? "" : reg_doc.financial_contact_email,
                        practice_emr = reg_doc.practice_emr == null ? "" : reg_doc.practice_emr,
                        billing_bankname = reg_doc.billing_bankname == null ? "" : reg_doc.billing_bankname,
                        billing_account = reg_doc.billing_account == null ? "" : reg_doc.billing_account,
                        billing_routing = reg_doc.billing_routing == null ? "" : reg_doc.billing_routing,

                        hospital_affiliation = reg_doc.hospital_affiliation == null? "" : reg_doc.hospital_affiliation,
                        network_insurance = new List<insurance>(), //reg_doc.network_insurance,
                        //language_spoken = reg_doc.language_spoken == null ? "" : reg_doc.language_spoken,
                        language_spoken = new List<doc_language>(), // : reg_doc.language,
                        specialty = new List<doc_specialty2>(), // : reg_doc.specialty,
                        //appointment_type = reg_doc.appt_type == null ? new List<appt_type>() { } : reg_doc.appt_type,
                        home_address = home == null ? new List<zip_search_address>() { } : home,
                        practice_address = prac_add == null ? new List<zip_search_address>() { } : prac_add,
                        
                    });
                    #endregion

                    // save Doctor profile here
                    // IHttpActionResult save = convert_get_post_doctor(prof[0]);
                    if (prof.Count() > 0)
                        return Json(new { data = prof, message = "", success = true });
                    else
                        return Json(new { data = new string[] { }, message = "No record found.", success = false });
                }



                // npi is found on db
                foreach (var d in doctor)
                {
                    // string home_zip = "";// n.ref_zip == null ? "" : n.ref_zip.zip;
                    //_home_city = n.ref_zip == null ? "" :  n.ref_zip.city_name;
                    //_home_state = n.ref_zip == null ? "" : n.ref_zip.city_state;

                   

                    doc_address = d;
                    // practice_address is the one that is saved in the Doctor table

                    var_getDoctorClaim_ext dx = _getDoctor_ext(d);
                    //getDoctor_rating dr = SearchDoctorController._get_averagerating(d.id, d.create_by__USER_id);

                    prof.Add(new get_doctor_profile
                    {
                        id = d.id,
                        npi = d.NPI,
                        first_name = d.name_first,
                        last_name = d.name_last,
                        middle_name = d.name_middle == null ? "" : d.name_middle,
                        title = d.title == null ? "" : d.title,
                        gender = d.gender == null ? "" : d.gender,
                        email = d.email == null ? "" : d.email,
                        //home_zip = reg_doc.home_zip,
                        dob = reg_doc.dob == null ? "" : reg_doc.dob,
                        //home_city = reg_doc.home_city,
                        //home_state = reg_doc.home_state,
                        education = reg_doc.education == null ? "" : reg_doc.dob,
                        personal_practice_type = reg_doc.personal_practice_type == null ? "" : reg_doc.dob,
                        board_certification = reg_doc.board_certification == null ? "" : reg_doc.dob,
                        practice_npi = reg_doc.practice_npi == null ? "" : reg_doc.dob,
                        practice_name = reg_doc.practice_name == null ? "" : reg_doc.dob,
                        practice_type = reg_doc.practice_type == null ? "" : reg_doc.dob,
                        dea = reg_doc.dea == null ? "" : reg_doc.dob,
                        clinician_role = reg_doc.clinician_role == null ? "" : reg_doc.dob,
                        scheduling_solution = reg_doc.scheduling_solution == null ? "" : reg_doc.dob,
                        current_scheduling = reg_doc.current_scheduling == null ? "" : reg_doc.dob,
                        //practice_street = reg_doc.practice_street,
                        //practice_zip = reg_doc.practice_zip,
                        practice_phone_primary = reg_doc.practice_phone_primary == null ? "" : reg_doc.dob,
                        practice_fax = reg_doc.practice_fax == null ? "" : reg_doc.dob,
                        practice_phone_cs = reg_doc.practice_phone_cs == null ? "" : reg_doc.dob,
                        practice_phone_office = reg_doc.practice_phone_office == null ? "" : reg_doc.dob,
                        practice_clinicians = reg_doc.practice_clinicians == null ? "" : reg_doc.dob,

                        practice_exams = reg_doc.practice_exams == null ? "" : reg_doc.dob,
                        geographic_market = reg_doc.geographic_market == null ? "" : reg_doc.dob,
                        practice_expansion = reg_doc.practice_expansion == null ? "" : reg_doc.dob,
                        practice_insurance = reg_doc.practice_insurance == null ? "" : reg_doc.dob,
                        practice_tax_number = reg_doc.practice_tax_number == null ? "" : reg_doc.practice_tax_number,
                        primary_contact_name = reg_doc.primary_contact_name == null ? "" : reg_doc.primary_contact_name,
                        primary_contact_phone = reg_doc.primary_contact_phone == null ? "" : reg_doc.primary_contact_phone,
                        primary_contact_email = reg_doc.primary_contact_email == null ? "" : reg_doc.primary_contact_email,
                        operational_contact_name = reg_doc.operational_contact_name == null ? "" : reg_doc.operational_contact_name,
                        operational_contact_phone = reg_doc.operational_contact_phone == null ? "" : reg_doc.operational_contact_phone,
                        operational_contact_email = reg_doc.operational_contact_email == null ? "" : reg_doc.operational_contact_email,
                        financial_contact_name = reg_doc.financial_contact_name == null ? "" : reg_doc.financial_contact_name,
                        financial_contact_phone = reg_doc.financial_contact_phone == null ? "" : reg_doc.financial_contact_phone,
                        financial_contact_email = reg_doc.financial_contact_email == null ? "" : reg_doc.financial_contact_email,
                        practice_emr = reg_doc.practice_emr == null ? "" : reg_doc.practice_emr,
                        billing_bankname = reg_doc.billing_bankname == null ? "" : reg_doc.billing_bankname,
                        billing_account = reg_doc.billing_account == null ? "" : reg_doc.billing_account,
                        billing_routing = reg_doc.billing_routing == null ? "" : reg_doc.billing_routing,

                        specialty = dx.spec == null ? new List<doc_specialty2>() { } : dx.spec,

                        home_address = dx.home_address == null ? new List<zip_search_address>() { } : dx.home_address,
                        practice_address = dx.practice_address == null ? new List<zip_search_address>() { } : dx.practice_address,
                        language_spoken = dx.language == null ? new List<doc_language>() { } : dx.language,
                        network_insurance = dx.insurance == null ? new List<insurance>() { } : dx.insurance

                    });
                }

                if (prof.Count() > 0)
                {
                    var ret1 = JsonConvert.SerializeObject(prof);
                    var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);



                    //var obj = JsonConvert.DeserializeObject<response>(json1.ToString());
                    //var dta = JsonConvert.DeserializeObject<get_doctor_profile>(obj.data.ToString());

                    return Json(new { data = json1, message = "", success = true });
                }
                else
                {
                    msg = "Invalid NPI.";
                    return Json(new { data = new string[] { }, message = msg, success = false });
                }
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
            //}
            //return Json(new { data =new string[] { }, message = msg, success =false });
        }

        private List<zip_search_address> _getnpi_Homeaddress(string addr_home1 = null, string addr_home2 = null, string addr_home_zip = null)
        {
            addr_home1 = addr_home1 == null ? "" : addr_home1;
            addr_home2 = addr_home2 == null ? "" : addr_home2;
            addr_home_zip = addr_home_zip == null ? "" : addr_home_zip;
            List<zip_search_address> home = new List<zip_search_address>();

            string zip = addr_home_zip.Count() > 0 ? addr_home_zip.Substring(0, 5) : addr_home_zip;
              
            var home_addr = dbEntity.ref_zip.Where(a => a.zip == zip);

            if (home_addr.Count() > 0)
            {
                home.Add(new zip_search_address
                {
                    address1 = addr_home1 == null ? "" : addr_home1,
                    address2 = addr_home2 == null ? "" : addr_home2,

                    zip = home_addr.FirstOrDefault().zip,
                    city = home_addr.FirstOrDefault().city_name,
                    state = home_addr.FirstOrDefault().city_state,
                    state_long = home_addr.FirstOrDefault().city_state_long,
                    lat = home_addr.FirstOrDefault().city_lat,
                    lng = home_addr.FirstOrDefault().city_lon,
                    county = home_addr.FirstOrDefault().city_county
                });
            }
         

          return home;
        }

        private doc_specialty2 _getSpecialtyByCode(string code)
        {
            doc_specialty2 spec = new doc_specialty2();
            //string[] sp = value.Split(',');
            //foreach (var i in sp)
            //{

                //long spec_id = 0;
                //bool s = long.TryParse(i, out spec_id);
                //if (s)
                //{
                    var get_spec = dbEntity.ref_specialty.Where(a => a.code == code);
            //if (get_spec != null) {
            foreach (var g in get_spec) {
                spec = new doc_specialty2
                {
                    id = g.id ,
                    name = g.name == null ? "" : g.name,
                    code = g.code == null ? "" : g.code,
                    provider_type = g.provider_type == null ? "" : g.provider_type,
                    specialization = g.specialization == null ? "" : g.specialization

                };

            }
            //}
            //spec.Add(new doc_specialty2
            //{
            //    id = spec1.FirstOrDefault().id,
            //    name = spec1.FirstOrDefault().name,
            //    code = spec1.FirstOrDefault().code,
            //    specialization = spec1.FirstOrDefault().specialization,
            //    provider_type = spec1.FirstOrDefault().provider_type
            //});
            //}

            //}

            return spec;
        }

        private var_getDoctorClaim_ext _getDoctor_ext(hs_DOCTOR doc)
        {
            SV_db1Entities dbEntity = new SV_db1Entities();
            var doc_ext = dbEntity.hs_DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doc.id);

            var_getDoctorClaim_ext dx = new var_getDoctorClaim_ext();
            
            dx.spec =  SearchDoctorController._getDoctor_specialty(doc.id);
            dx.language = SearchDoctorController._getLanguage(doc.id);
            dx.insurance = SearchDoctorController._getInsurance(doc.id);
            dx.home_address = custom._getDoctor_homeaddress(doc);
            dx.practice_address = custom._getDoctor_practiceaddress(doc);


            //List<doc_specialty> spec = new List<Models.doc_specialty>();
            List<doc_specialty2> spec1 = new List<doc_specialty2>();
            foreach (var d in doc_ext)
            {
                switch (d.attr_name)
                {
                    case "dob": dx.dob = d.value; break;

                    case "gender": dx.gender = d.value; break;
                    // case "email": _email = dx.value; break;

                    case "personal_practice_type": dx.personal_practice_type = d.value; break;

                    // case "home_street1": _home_street1 = dx.value; break;
                    // case "home_street2": _home_street2 = dx.value; break;
                    // case "home_city": _home_city = dx.value; break;
                    // case "home_state": _home_state = dx.value; break;
                    //case "home_zip":
                    //    reg_doc.home_zip = d.value;
                    //    string z = reg_doc.home_zip.Substring(0, 5);
                    //    var h_zip = dbEntity.ref_zip.Where(a => a.zip == z);
                    //    reg_doc.home_city = h_zip.FirstOrDefault().city_name; //_home_city = n.ref_zip == null ? "" :  n.ref_zip.city_name;
                    //    reg_doc.home_state = h_zip.FirstOrDefault().city_state;  //_home_state = n.ref_zip == null ? "" : n.ref_zip.city_state;
                    //    break;

                    case "education": dx.education = d.value; break;
                    case "experience": dx.experience = d.value; break;

                    //case "language_spoken": reg_doc.language_spoken = d.value; break;

                    case "board_certification": dx.board_certification = d.value; break;

                    case "specialty":
                    case "specialty_id":
                    #region

                    //reg_doc.specialty = d.value;
                    ////spec = null;
                    //string[] get_spec = d.value.Split(',');

                    //foreach (var i in get_spec)
                    //{

                    //    long spec_id = 0;
                    //    bool s1 = long.TryParse(i, out spec_id);

                    //    if (s1)
                    //    {
                    //        var ref_spec = dbEntity.ref_specialty.Find(spec_id);

                    //        //spec.Add(new doc_specialty
                    //        //{
                    //        //    id = ref_spec.id,
                    //        //    code = ref_spec.code == null? "" : ref_spec.code,
                    //        //    description = ref_spec.description ==null? "": ref_spec.description,
                    //        //    name = ref_spec.name==null?"": ref_spec.name,
                    //        //    actor = ref_spec.actor == null ? "" : ref_spec.actor
                    //        //});
                    //        spec1.Add(new doc_specialty2
                    //        {
                    //            id = ref_spec.id,
                    //            name = ref_spec.name,
                    //            code = ref_spec.code,
                    //            specialization = ref_spec.specialization,
                    //            provider_type = ref_spec.provider_type
                    //        });
                    //    }
                    //}

                    //break;
                    #endregion


                    case "hospital_affiliation": dx.hospital_affiliation = d.value; break;
                    case "practice_npi": dx.practice_npi = d.value; break;

                    case "practice_name": dx.practice_name = d.value; break;

                    case "practice_type": dx.practice_type = d.value; break;

                    case "dea": dx.dea = d.value; break;

                    case "clinician_role": dx.clinician_role = d.value; break;

                    case "scheduling_solution": dx.scheduling_solution = d.value; break;

                    case "current_scheduling": dx.current_scheduling = d.value; break;

                    case "practice_street": dx.practice_street = d.value; break;

                    //case "practice_zip": // service zip codes
                    //    reg_doc.practice_zip = d.value;

                    //    break;
                    case "practice_phone_primary":// primary phone number
                        dx.practice_phone_primary = d.value; break;

                    case "practice_fax": dx.practice_fax = d.value; break;

                    case "practice_phone_cs": // customer service number
                        dx.practice_phone_cs = d.value; break;

                    case "practice_phone_office":// office phone
                        dx.practice_phone_office = d.value; break;

                    case "practice_clinicians": // no of field clinicians
                        dx.practice_clinicians = d.value; break;

                    case "practice_exams": // no of exams you can handle per week
                        dx.practice_exams = d.value; break;

                    case "geographic_market": // geographic market
                        dx.geographic_market = d.value; break;

                    case "practice_expansion": // future expansion plans, new market, 
                        dx.practice_expansion = d.value; break;

                    case "practice_insurance":// insurance list you are in Network or will acccept
                        dx.practice_insurance = d.value; break;

                    case "practice_tax_number":// federal tax id number
                        dx.practice_tax_number = d.value; break;

                    case "primary_contact_name":
                        dx.primary_contact_name = d.value;
                        break;

                    case "primary_contact_phone":
                        dx.primary_contact_phone = d.value;
                        break;

                    case "primary_contact_email":
                        dx.primary_contact_email = d.value;
                        break;

                    case "operational_contact_name":
                        dx.operational_contact_name = d.value;
                        break;

                    case "operational_contact_phone": dx.operational_contact_phone = d.value; break;

                    case "operational_contact_email": dx.operational_contact_email = d.value; break;

                    case "financial_contact_name": dx.financial_contact_name = d.value; break;

                    case "financial_contact_phone": dx.financial_contact_phone = d.value; break;

                    case "financial_contact_email": dx.financial_contact_email = d.value; break;

                    case "practice_emr":  // emr software that you are currently using
                        dx.practice_emr = d.value; break;

                    case "network_insurance":// in-network insurances
                        dx.network_insurance = d.value; break;

                    // public string practice_contact { get; set; } // primary contact/ operational contact/ financial contact
                    case "billing_bankname": dx.billing_bankname = d.value; break;
                    case "billing_account": dx.billing_account = d.value; break;
                    case "billing_routing": dx.billing_routing = d.value; break;

                }

            }
            return dx;
        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("doctor/profile")] // doctor
        public IHttpActionResult getDoctorProfile(string npi = null)
        {
            //if (!authorize) return Json(new { data = new string[] { }, message = "not authorized", success = false });

            //string v = httpCon.Request.Headers["Authorization"];
            string msg = "The authorization header is not valid.";


            //if (Validation.userAuth(v))
            //{
            //string s = getTrim2();
            IsRequired("npi", npi, 2);
            if (haserror)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }

            var doc = dbEntity.hs_DOCTOR.Where(a => a.NPI == npi);

            try
            {

                List<get_doctor_profile> prof = new List<get_doctor_profile>();
                List<doc_specialty2> spec = new List<Models.doc_specialty2>();
                // _email="", _home_street1 = "", _home_street2 = "", _home_zip = "",

                npi_registry reg = new npi_registry();

                if (doc.Count() == 0)
                {
                    // fetch from NPI registry

                    var uri = "https://npiregistry.cms.hhs.gov/api?number=" + npi;
                    System.Net.WebRequest req = System.Net.WebRequest.Create(uri);
                    System.Net.WebResponse resp = req.GetResponse();
                    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                    var x = sr.ReadToEnd().Trim();

                    dynamic ret2 = JsonConvert.DeserializeObject(x);
                    int cnt = Convert.ToInt32(ret2["result_count"]);
                    if (cnt == 0)
                    {
                        return Json(new { data = new string[] { }, message = "No record found.", success = false });
                    }

                    var i = ret2["results"];
                    var res = i[0]["taxonomies"]; //JsonConvert.DeserializeObject(ret2["results"].ToString());
                    var addr_home = i[0]["addresses"][0];
                    var addr_practice = i[0]["addresses"][1];
                    var per_profile = i[0]["basic"];
                    var npi1 = i[0]["number"];
                    var addr1 = addr_home["address_1"];



                    string spec_code = res[0]["code"].ToString();
                    var spec1 = dbEntity.ref_specialty.Where(b => b.code == spec_code);

                    //spec.Add(
                    //    new doc_specialty {
                    //        id = spec1.FirstOrDefault().id,
                    //        description  = spec1.FirstOrDefault().description,
                    //        code = spec1.FirstOrDefault().code,
                    //        actor = spec1.FirstOrDefault().actor == null?"": spec1.FirstOrDefault().actor,
                    //        name = ""
                    //    });
                    spec.Add(new doc_specialty2
                    {
                        id = spec1.FirstOrDefault().id,
                        name = spec1.FirstOrDefault().name,
                        code = spec1.FirstOrDefault().code,
                        specialization = spec1.FirstOrDefault().specialization,
                        provider_type = spec1.FirstOrDefault().provider_type
                    });

                    List<zip_search_address> home = new List<zip_search_address>();
                    home = _getnpi_Homeaddress(addr_home["address_1"], addr_home["address_2"], addr_home["postal_code"]);
                    List<zip_search_address> prac_add = new List<zip_search_address>();
                    prac_add = _getnpi_Homeaddress(addr_practice["address_1"], "", addr_practice["postal_code"]);

                    #region "get_doctor_profile"
                    prof.Add(new get_doctor_profile
                    {
                        id = 0,
                        npi = i[0]["number"],
                        first_name = per_profile["first_name"],
                        last_name = per_profile["last_name"],
                        middle_name = per_profile["middle_name"] == null ? "" : per_profile["middle_name"],
                        title = "", //n.title == null ? "" : n.title,
                        gender = per_profile["gender"] == null ? "" : per_profile["gender"],
                        email = "", // n.email == null ? "" : n.email,
                        //home_street1 = addr_home["address_1"] == null ? "" : addr_home["address_1"],
                        //home_street2 = addr_home["address_2"] == null ? "" : addr_home["address_2"],
                        //home_zip = addr_home["postal_code"] == null ? "" : addr_home["postal_code"],
                        dob = reg.dob,
                        //home_city = addr_home["city"] == null ? "" : addr_home["city"],
                        //home_state = addr_home["state"] == null ? "" : addr_home["state"],
                        education = reg.education,
                        
                        personal_practice_type = reg.personal_practice_type,
                        board_certification = reg.board_certification,
                        specialty = spec,
                        practice_npi = reg.practice_npi,
                        practice_name = reg.practice_name,
                        practice_type = reg.practice_type,
                        dea = reg.dea,
                        clinician_role = reg.clinician_role,
                        scheduling_solution = reg.scheduling_solution,
                        current_scheduling = reg.current_scheduling,
                        //practice_street = addr_practice["address_1"] + " " + addr_practice["address_2"],
                        //practice_zip = addr_practice["postal_code"],
                        practice_phone_primary = addr_practice["telephone_number"],
                        practice_fax = reg.practice_fax,
                        practice_phone_cs = reg.practice_phone_cs,
                        practice_phone_office = reg.practice_phone_office,
                        practice_clinicians = reg.practice_clinicians,

                        practice_exams = reg.practice_exams,
                        geographic_market = reg.geographic_market,
                        practice_expansion = reg.practice_expansion,
                        practice_insurance = reg.practice_insurance,
                        practice_tax_number = reg.practice_tax_number,
                        primary_contact_name = reg.primary_contact_name,
                        primary_contact_phone = reg.primary_contact_phone,
                        primary_contact_email = reg.primary_contact_email,
                        operational_contact_name = reg.operational_contact_name,
                        operational_contact_phone = reg.operational_contact_phone,
                        operational_contact_email = reg.operational_contact_email,
                        financial_contact_name = reg.financial_contact_name,
                        financial_contact_phone = reg.financial_contact_phone,
                        financial_contact_email = reg.financial_contact_email,
                        practice_emr = reg.practice_emr,
                       
                        billing_bankname = reg.billing_bankname,
                        billing_account = reg.billing_account,
                        billing_routing = reg.billing_routing,

                        home_address = home == null ? new List<zip_search_address>() { } : home,
                        practice_address = prac_add == null ? new List<zip_search_address>() { } : prac_add,
                        language_spoken = new List<doc_language>(), //reg.language_spoken,
                        network_insurance = new List<insurance>() //reg.network_insurance,
                    });
                    #endregion

                    // save Doctor profile here
                    // IHttpActionResult save = convert_get_post_doctor(prof[0]);
                    if (prof.Count() > 0)
                        return Json(new { data = prof, message = "", success = true });
                    else
                        return Json(new { data = new string[] { }, message = "No record found.", success = false });
                }
                else
                {
                    prof = _getDoctor_ext(doc,  reg);
                }

                //foreach (var n in doc)
                //{
                //    // string home_zip = "";// n.ref_zip == null ? "" : n.ref_zip.zip;
                //    //_home_city = n.ref_zip == null ? "" :  n.ref_zip.city_name;
                //    //_home_state = n.ref_zip == null ? "" : n.ref_zip.city_state;
                //}

                if (prof.Count() > 0)
                {
                    var ret1 = JsonConvert.SerializeObject(prof);
                    var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                    return Json(new { data = json1, message = "", success = true });
                }
                else
                {
                    msg = "Invalid NPI.";
                    return Json(new { data = new string[] { }, message = msg, success = false });
                }
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
            //}

            //return Json(new { data = new string[] { }, message = msg, success = false });
        }


     

        

        bool haserror = false;
        string errmsg = "", infomsg = "";
        private bool IsRequired(string key, string val, int i)
        {
            if (i == 1)
            {
                if (string.IsNullOrEmpty(val))
                {
                    haserror = true;
                    errmsg += key + " is required. \r\n";
                    return false;
                }
                return true;


            }
            else
            {
                if (string.IsNullOrEmpty(val))
                {
                    haserror = true;
                    errmsg += " Missing parameter: " + key + ". \r\n";
                    return false;
                }
                return true;
            }

        }

        // GET: Get
        //public ActionResult Index()
        //{
        //    return View();
        //}
        //private bool authorize() {
        //    HttpContext httpContext = HttpContext.Current;
        //    string authHeader = httpContext.Request.Headers["Authorization"];
        //    if (authHeader != null && authHeader.StartsWith("Basic"))
        //    {
        //        string encodedUserNamePassword = authHeader.Substring("Basic ".Length).Trim();
        //        Encoding encoding = Encoding.GetEncoding("iso-8859-1");
        //        string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUserNamePassword));

        //        int sep = usernamePassword.IndexOf(':');
        //        string username = usernamePassword.Substring(0, sep);
        //        string password = usernamePassword.Substring(sep + 1);

        //        if (username == "deftsoft" && password == "deftsoftapikey")
        //        {
        //            return true;
        //        }
        //        return false;
        //    }

        //    return false;
        //}

        //SV_db1Entities dbEntity = new SV_db1Entities();




        private IHttpActionResult Get([FromUri]doc_query query)
        {

            if (true)//progAuth.authorize()
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
            else {
                throw new Exception("The authorization header is either empty or isn't Basic.");
            }
            // doctor_name, npi,license_no, address, state, city, zipcode


            //if (string.IsNullOrEmpty(query.acck)) return Json(new { data = "", message = "Unauthorized Access. No api key was provided.", success = false });
            //if (query.acck == "deftsoftapikey")
            //{


            //}
            //else
            //{
            //    return Json(new { data = "", message = "Unauthorized Access. Invalid api key was provided.", success = false });
            //}
        }

        private IHttpActionResult Put([FromBody] put_param p)
        {
            //long docId, string acck = "deftsoftapikey", string firstname = "", string lastname = "", string middlename = "", string zipcode = "", string address1 = "", string address2 = "",
            //                           string gender = "", string title = "", string email = "", string phone = "", string license = "",
            //                           string npi = "", string npitype = "", string orgname = "",
            //                           bool pecoscert = false, string fax = "", string city = "", string state = "",
            //                           string bio = ""
            try
            {
                if (p.acck == "deftsoftapikey")
                {
                    doc_profile doc = new doc_profile();
                    doc.id = p.docId; doc.firstname = p.firstname; doc.lastname = p.lastname; doc.middlename = p.middlename;
                    doc.gender = p.gender; doc.title = p.title; doc.phone = p.phone; doc.email = p.email; doc.license = p.license;
                    doc.npi = p.npi; doc.npitype = p.npitype; doc.organization_name = p.orgname; //doc.specialties = specialties;
                    doc.address = p.address1; //doc.address2 = address2; 
                    doc.pecos_certificate = p.pecos_certificate; doc.faxto = p.fax; doc.city = p.city;
                    doc.state = p.state; doc.zipcode = p.zipcode; doc.bio = p.bio; //doc.acck = acck;

                    var docProf = dbEntity.DOCTORs.Find(doc.id);
                    double city_lat = 0; double city_lon = 0;
                    if (docProf != null)
                    {
                        if (!string.IsNullOrEmpty(doc.firstname)) docProf.name_first = doc.firstname;
                        if (!string.IsNullOrEmpty(doc.lastname)) docProf.name_last = doc.lastname;
                        if (!string.IsNullOrEmpty(doc.middlename)) docProf.name_middle = doc.middlename;
                        if (!string.IsNullOrEmpty(doc.gender))
                        {
                            char[] g = p.gender.ToCharArray();
                            docProf.gender = g[0].ToString();
                        }

                        if (!string.IsNullOrEmpty(doc.title)) doc.title = p.title;
                        if (!string.IsNullOrEmpty(doc.phone)) doc.phone = p.phone;
                        if (!string.IsNullOrEmpty(doc.email)) doc.email = p.email;
                        if (!string.IsNullOrEmpty(doc.license)) doc.license = p.license;
                        if (!string.IsNullOrEmpty(doc.npi)) doc.npi = p.npi;
                        if (!string.IsNullOrEmpty(doc.npitype)) doc.npitype = p.npitype;
                        if (!string.IsNullOrEmpty(doc.organization_name)) doc.organization_name = p.orgname;
                        //doc.specialties = specialties;
                        //if (!string.IsNullOrEmpty(doc.license)) doc.address1 = address1;
                        //if (!string.IsNullOrEmpty(doc.license)) doc.address2 = address2;
                        if (!string.IsNullOrEmpty(doc.pecos_certificate)) doc.pecos_certificate = p.pecos_certificate;
                        if (!string.IsNullOrEmpty(doc.faxto)) doc.faxto = p.fax;
                        if (!string.IsNullOrEmpty(doc.city)) doc.city = p.city;
                        //if (!string.IsNullOrEmpty(doc.state)) doc.state = state;
                        //if (!string.IsNullOrEmpty(doc.zipcode)) doc.zipcode = zipcode;
                        if (!string.IsNullOrEmpty(doc.bio)) doc.bio = p.bio;
                        // doc.acck = acck;

                        //docProf.rel_ADDRESS_id =// zip code
                        // docProf.gender = doc.gender; docProf.email = email; docProf.phone = phone; docProf.license_no = license; docProf.NPI = npi;
                        //docProf.NPI_type = npitype; docProf.org_name = orgname; docProf.pecos_cert = pecoscert; docProf.fax_to = fax;

                        //var docAddr = db.DOCTORs.Find(docProf.rel_ADDRESS_id);
                        //var addr = dbEntity.ADDRESSes.Find(docProf.rel_ADDRESS_id);

                        //if (addr != null)
                        //{
                        //    if (!string.IsNullOrEmpty(doc.address1)) addr.address1 = doc.address1;
                        //    if (!string.IsNullOrEmpty(doc.address2)) addr.address2 = doc.address2;
                        //    var zip = dbEntity.ref_zip.Where(a => a.zip == doc.zipcode).FirstOrDefault();
                        //    if (zip != null)
                        //    {
                        //        addr.rel_ref_zip_id = zip.id;
                        //        city_lat = zip.city_lat;
                        //        city_lon = zip.city_lon;
                        //    }
                        //    dbEntity.Entry(addr).State = System.Data.Entity.EntityState.Modified;
                        //}
                    }

                    dbEntity.Entry(docProf).State = System.Data.Entity.EntityState.Modified;
                    dbEntity.SaveChanges();

                    List<doc_profile> dc = new List<doc_profile>();
                    dc.Add(new doc_profile
                    {
                        id = docProf.id,
                        firstname = p.firstname,
                        lastname = p.lastname,
                        middlename = p.middlename,

                        gender = p.gender,
                        title = p.title,
                        phone = p.phone,
                        email = p.email,
                        license = p.license,
                        npi = p.npi,
                        npitype = p.npitype,
                        organization_name = p.orgname,
                        pecos_certificate = p.pecos_certificate,
                        faxto = p.fax,
                        city = p.city,
                        city_lat = city_lat,
                        city_long = city_lon,
                        bio = p.bio,
                        // doc.acck = acck;
                        address = doc.address,
                        //address2 = doc.address2,
                        zipcode = p.zipcode

                    });

                    var ret = JsonConvert.SerializeObject(dc);
                    var json = Newtonsoft.Json.Linq.JArray.Parse(ret);
                    string msg = "Record is updated.";
                    return Json(new { data = json, message = msg, success = true });
                }
                else //  (acck == "deftsoftapikey")
                {
                    return Json(new { data = "", message = "Unauthorized Access. Invalid api key was provided.", success = false });
                }


            }
            catch (Exception ex) {
                return Json(new { data = "", message = ex.Message, success = false });
            }

        }

        // public void Post()
        // {
        //     doc_profile doc;


        // //, string lastname, string address1, string acck
        // //&lastname=godwin&address1=3453 Barrington&acck =deftsoftapikey
        //}

        private IHttpActionResult Post([FromBody] post_param p)
        {
            //string firstname, string lastname, string middlename, string acck, string zipcode, string address1 = "", string address2 = "",
            //                           string gender = "", string title = "", string email = "", string phone = "", string license = "",
            //                           string npi = "", string npitype = "", string orgname = "", string specialties = "",
            //                           bool pecoscert = false, string fax = "", string city = "", string state = "",
            //                           string bio = ""

            try
            {
                doc_profile doc = new doc_profile();
                doc.firstname = p.firstname; doc.lastname = p.lastname; doc.middlename = p.middlename;
                doc.gender = p.gender; doc.title = p.title; doc.phone = p.phone; doc.email = p.email;
                doc.license = p.license; doc.npi = p.npi; doc.npitype = p.npitype; doc.organization_name = p.organization_name;
                doc.specialties = p.specialties; doc.address = p.address1; //doc.address2 = address2; 
                doc.pecos_certificate = p.pecos_certificate; doc.faxto = p.fax; doc.city = p.city; doc.state = p.state;
                doc.zipcode = p.zipcode; doc.bio = p.bio; doc.acck = p.acck;

                if (string.IsNullOrEmpty(doc.acck)) return Json(new { data = "", message = "Unauthorized Access. No api key was provided.", success = false });
                if (doc.acck == "deftsoftapikey")
                {
                    //ADDRESS addr = new ADDRESS();
                    // if (!string.IsNullOrEmpty(zipcode))
                    //{
                    //    var zip = dbEntity.ref_zip.FirstOrDefault(a => a.zip.Contains(doc.zipcode));

                    //     if (!string.IsNullOrEmpty(doc.address1) && doc.address1.ToLower() != "null") addr.address1 = doc.address1;
                    //     if (!string.IsNullOrEmpty(doc.address2) && doc.address2.ToLower() != "null") addr.address2 = doc.address2;
                    //     addr.rel_ref_zip_id = zip.id;
                    //     addr.last_update = DateTime.Now;

                    //     dbEntity.ADDRESSes.Add(addr);
                    //     dbEntity.SaveChanges();
                    // }

                    DOCTOR d = new DOCTOR();

                    //d.doc_name = doc.firstname + "|" + doc.lastname + "|" + doc.title;
                    d.name_first = doc.firstname;
                    d.name_last = doc.lastname;
                    d.name_middle = doc.middlename;
                    if (!string.IsNullOrEmpty(doc.gender) && doc.gender.ToLower() != "null") d.gender = doc.gender.ToUpper();
                    if (!string.IsNullOrEmpty(doc.title) && doc.title.ToLower() != "null") d.title = doc.title;
                    if (!string.IsNullOrEmpty(doc.npi) && doc.npi.ToLower() != "null") d.NPI = doc.npi;
                    if (!string.IsNullOrEmpty(doc.license) && doc.license.ToLower() != "null") d.license_no = doc.license;
                    if (!string.IsNullOrEmpty(doc.phone) && doc.phone.ToLower() != "null") d.phone = doc.phone;

                    if (!string.IsNullOrEmpty(doc.faxto) && doc.faxto.ToLower() != "null") d.fax_to = doc.faxto;
                    //if (!string.IsNullOrEmpty(doc.specialties) && doc.specialties.ToLower() != "null") d.specialty = doc.specialties;

                    if (!string.IsNullOrEmpty(p.zipcode))
                    {
                        if (!string.IsNullOrEmpty(doc.address) && doc.address.ToLower() != "null") d.practice_addr_1 = doc.address;
                        //if (!string.IsNullOrEmpty(doc.address2) && doc.address2.ToLower() != "null") d.addr_address2 = doc.address2;
                        //d.address_state = zip.city_state;
                        //if (!string.IsNullOrEmpty(doc.city) && doc.city.ToLower() != "null") d.address_city = doc.city;

                        var zip = dbEntity.ref_zip.Where(a => a.zip.Contains(doc.zipcode)).FirstOrDefault();
                        if (!string.IsNullOrEmpty(doc.zipcode) && doc.zipcode.ToLower() != "null") d.practice_addr_zip_id = zip.id;
                        //d.rel_ADDRESS_id = addr.id;
                        //d.last_update_by = 0;

                    }

                    dbEntity.DOCTORs.Add(d);
                    dbEntity.SaveChanges();

                    // add the address details to doc_profile
                    doc.id = d.id;


                    // //new_doc = new doc_profile();
                    //List<doc_profile> dc = new List<doc_profile>();
                    //dc.Add(new doc_profile { guid= d.guid,
                    //    firstname = doc.firstname,
                    //    lastname = doc.lastname,

                    //    city = addr.ref_zip.city_name,
                    //    state = addr.ref_zip.city_state,
                    //    city_lat = addr.ref_zip.city_lat,
                    //    city_long = addr.ref_zip.city_lon,
                    //    address1 = addr.address1
                    //});
                    ////var json = searchResponse(doc);

                    // var ret = JsonConvert.SerializeObject(dc); 
                    //var json = Newtonsoft.Json.Linq.JArray.Parse(ret);
                    string msg = "New record is saved.";
                    return Json(new { data = "", message = msg, success = true });
                    // return Json(new { data = ret, message = msg, count = cnt, success = true });
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


        ////[System.Web.Http.Route("GetDoctor/{id}/{name}/{specialty}")]
        //public IHttpActionResult GetDoctor([FromUri]doc_query query, string id, string name, string specialty)
        //{
        //    // doctor_name, npi,license_no, address, state, city, zipcode

        //    if (string.IsNullOrEmpty(query.acck)) return Json(new { data = "", message = "Unauthorized Access. No api key was provided.", success = false });
        //    if (query.acck == "deftsoftapikey")
        //    {
        //        //if (!string.IsNullOrEmpty(query.patient_name)) query.patient_name = query.patient_name.ToLower(); else query.patient_name = string.Empty;
        //        //doctor_name = this.doctor_name;
        //        if (!String.IsNullOrEmpty(query.firstname)) query.firstname = query.firstname.ToLower().Trim(); else query.firstname = string.Empty;
        //        if (!String.IsNullOrEmpty(query.lastname)) query.lastname = query.lastname.ToLower().Trim(); else query.lastname = string.Empty;
        //        if (!String.IsNullOrEmpty(query.name)) query.name = query.name.ToLower().Trim(); else query.name = string.Empty;
        //        //npi = this.npi;
        //        if (!String.IsNullOrEmpty(query.npi)) query.npi = query.npi.ToLower().Trim(); else query.npi = string.Empty;
        //        //license_no = this.license_no;
        //        if (!String.IsNullOrEmpty(query.license)) query.license = query.license.ToLower().Trim(); else query.license = string.Empty;
        //        //address = this.address;
        //        if (!String.IsNullOrEmpty(query.address)) query.address = query.address.ToLower().Trim(); else query.address = string.Empty;
        //        //state = this.state;
        //        if (!String.IsNullOrEmpty(query.state)) query.state = query.state.ToLower().Trim(); else query.state = string.Empty;

        //        //city = this.city;
        //        int zip;
        //        bool isZip = int.TryParse(query.zipcode, out zip);
        //        if (!String.IsNullOrEmpty(query.city) && !isZip) query.city = query.city.ToLower().Trim(); else query.city = string.Empty;
        //        //zipcode = this.zipcode;
        //        zip = 0;
        //        isZip = int.TryParse(query.zipcode, out zip);
        //        if (!String.IsNullOrEmpty(query.zipcode) && isZip) query.zipcode = query.zipcode.ToLower().Trim(); else query.zipcode = string.Empty;


        //        var json = searchResponse(query);

        //        int cnt = doc_count;
        //        string msg = cnt.ToString() + (cnt > 1 ? " results" : " result") + " found.";

        //        var ret = Newtonsoft.Json.Linq.JArray.Parse(json);

        //        return Json(new { data = ret, message = msg, count = cnt, success = true });

        //    }
        //    else
        //    {
        //        return Json(new { data = "", message = "Unauthorized Access. Invalid api key was provided.", success = false });
        //    }

        //}

        private int doc_count;
        //private readonly DateTime? dt;

        private string searchResponse(doc_query param1)
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

            var detail = from a in dbEntity.hs_DOCTOR select a;

            //   var blah = dbEntity.DOCTORs.Where(a => a.id.value.);

            if (!string.IsNullOrEmpty(param1.name))
            {
                string[] n = param1.name.Split(' ');

                //switch (n.Length)
                //{
                //    case 1:
                //        param1.firstname = param1.name.Split(' ')[0];
                //        detail = detail.Where(b => b.doc_name.ToLower().Contains(param1.name));
                //        break;
                //    case 2:
                //        param1.firstname = param1.name.Split(' ')[0];
                //        param1.lastname = param1.name.Split(' ')[1];
                //        break;
                //}


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
                detail = detail.Where(b => b.name_last.ToLower().Contains(param1.lastname));
            }

            if (!string.IsNullOrEmpty(param1.npi))
                detail = detail.Where(b => b.NPI.Contains(param1.npi));
            if (!string.IsNullOrEmpty(param1.license))
                detail = detail.Where(b => b.license_no.Contains(param1.license));

            if (!string.IsNullOrEmpty(param1.address))
            {
                //List<long> addr = new List<long>();
                //var address = dbEntity.ADDRESSes.Where(a => a.address1.ToLower().Contains(param1.address) || a.address2.ToLower().Contains(param1.address));
                //// create a class type for this
                //foreach (var a in address)
                //{
                //    addr.Add(a.id);
                //}

                //detail = detail.Where(a => addr.Contains(a.rel_ADDRESS_id.Value));
                detail = detail.Where(a => a.home_addr_1.ToLower().Contains(param1.address) || a.home_addr_2.ToLower().Contains(param1.address));
            }

            if (!string.IsNullOrEmpty(param1.state))
            {
                //var state = dbEntity.ref_zip.Where(a => a.city_state.ToLower().Contains(param1.state));
                //List<long> _state = new List<long>();
                //// create a class type for this
                //foreach (var a in state)
                //{
                //    _state.Add(a.id);
                //}

                //List<long> addr_state = new List<long>();
                //var address = dbEntity.ADDRESSes.Where(a => _state.Contains(a.rel_ref_zip_id));
                //foreach (var a in address)
                //{
                //    addr_state.Add(a.id);
                //}

                List<long> _state = new List<long>();
                var state = dbEntity.ref_zip.Where(a => a.city_state.ToLower().Contains(param1.state));
                // create a class type for this
                foreach (var a in state)
                {
                    _state.Add(a.id);
                }

                detail = detail.Where(a => _state.Contains(a.home_addr_zip_id.Value));

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
                //detail = detail.Where(a => _city.Contains(a.rel_ADDRESS_id.Value));

            }

            if (!string.IsNullOrEmpty(param1.zipcode))
            {
                List<long> addr_zip = new List<long>();
                var ref_zip = dbEntity.ref_zip.Where(a => a.zip == param1.zipcode);
                foreach (var i in ref_zip)
                {
                    addr_zip.Add(i.id);
                }

                detail = detail.Where(b => addr_zip.Contains(b.home_addr_zip_id.Value));

                //detail = detail.Where(a => a.ADDRESS.ref_zip.zip.Contains(param1.zipcode));
            }


            // name,title,npi,licenseno,address, state, city, zipcode
            List<doc_list> dc = new List<doc_list>();
            doc_profile prof = new doc_profile();

            // insurance
            if (!string.IsNullOrEmpty(param1.insurance))
            {
                param1.insurance = param1.insurance.ToLower();

                string[] ins = param1.insurance.Split(',');

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


            detail = detail.OrderBy(a => a.id);
            if (!Double.IsNaN(param1.take) && param1.take > 0 && !double.IsNaN(param1.skip))
                detail = detail.Take(param1.take).Skip(param1.skip);


            foreach (var li in detail)
            {

                var addr = dbEntity.ref_zip.Find(li.home_addr_zip_id);
                string prof_addr = li.home_addr_1 + (li.home_addr_2 == null ? "" : " " + li.home_addr_2);

                prof = new Models.doc_profile
                {

                    id = li.id,
                    firstname = li.name_first,
                    lastname = li.name_last,
                    gender = li.gender == null ? "" : li.gender.Trim(),
                    title = li.title,
                    phone = li.phone,
                    npi = li.NPI,
                    organization_name = li.organization_name,

                    image_url = li.image_url,
                    //add_id =  ,
                    //address1 = li.ADDRESS.address1,
                    //address2 = li.ADDRESS.address2,
                    address = prof_addr,
                    pecos_certificate = li.pecos_certification == null ? "" : li.pecos_certification,
                    faxto = li.fax_to,
                    state = addr != null ? addr.city_state : "",
                    city = addr != null ? addr.city_name : "",
                    city_lat = addr != null ? addr.city_lat : 0,
                    city_long = addr != null ? addr.city_lon : 0,
                    zipcode = addr != null ? addr.zip : "",

                };


                //// get SPECIALTY
                //List<doc_specialty> spec = new List<doc_specialty>();
                //var sp = dbEntity.Doc_Specialties_Profile.Where(a => a.doctor_id == li.id);
                //if (sp != null)
                //{
                //    foreach (var i in sp)
                //    {
                //        spec.Add(new doc_specialty
                //        {
                //            id = i.specialty_id.Value,
                //            name = i.ref_specialties.name,
                //            actor = i.ref_specialties.actor
                //        });
                //    }

                //}

                //// get doctor's education
                //List<doc_education> educ = new List<doc_education>();
                //var ed = dbEntity.Doc_Education.Where(a => a.doctor_id == li.id);
                //if (ed != null) {
                //    foreach (var i in ed) {
                //        educ.Add(new doc_education { degree = i.degree, school = i.school, graduation_year = i.graduation_year != null ? i.graduation_year.Trim() :"" });
                //    }
                //}

                //// get Insurance Affiliations for Doctor
                //List<doc_insurance> insu = new List<doc_insurance>();
                //var ins = dbEntity.Doc_Insurance_Affiliation.Where(a => a.doctor_id == li.id);
                //if (ins != null)
                //{
                //    foreach (var i in ins) {
                //        insu.Add(new doc_insurance {
                //            provider_name = i.ref_insurancecompanies.PayerName,
                //            provider_id = i.ref_insurancecompanies.PayerID
                //        });
                //    }
                //}

                // dc.Add(new doc_list { profile = prof, specialties = spec, educations = educ });
                dc.Add(new doc_list { profile = prof });
                doc_count++;

            }



            var var1 = JsonConvert.SerializeObject(dc);

            return var1;
        }
    }



    public class npi_registry {
        public string dob { get; set; }
        public string gender { get; set; }
        public string personal_practice_type { get; set; }
        public string home_zip { get; set; }
        public string home_city { get; set; }
        public string home_state { get; set; }
        public string education { get; set; }
        public string experience { get; set; }

        public string language_spoken { get; set; }
        public string board_certification { get; set; }
        public string specialty { get; set; }
        public string practice_npi { get; set; }
        public string practice_name { get; set; }
        public string practice_type { get; set; }
        public string dea { get; set; }
        public string clinician_role { get; set; }
        public string scheduling_solution { get; set; }
        public string current_scheduling { get; set; }

        public string hospital_affiliation { get; set; }
        public string practice_street { get; set; }
        public string practice_zip { get; set; }
        public string practice_phone_primary { get; set; }
        public string practice_fax { get; set; }
        public string practice_phone_cs { get; set; }
        public string practice_phone_office { get; set; }
        public string practice_clinicians { get; set; }
        public string practice_exams { get; set; }
        public string geographic_market { get; set; }
        public string practice_expansion { get; set; }
        public string practice_insurance { get; set; }
        public string practice_tax_number { get; set; }
        public string primary_contact_name { get; set; }
        public string primary_contact_phone { get; set; }
        public string primary_contact_email { get; set; }
        public string operational_contact_name { get; set; }
        public string operational_contact_phone { get; set; }
        public string operational_contact_email { get; set; }
        public string financial_contact_name { get; set; }
        public string financial_contact_phone { get; set; }
        public string financial_contact_email { get; set; }
        public string practice_emr { get; set; }
        public string network_insurance { get; set; }
        public string billing_bankname { get; set; }
        public string billing_account { get; set; }
        public string billing_routing { get; set; }
    }


    public class get_doctor_profile {
        public long id { get; set;}
        public string npi { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; } 
        public string middle_name { get; set; }
        public string title { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public string personal_practice_type { get; set; }
        //public string home_street1 { get; set; }
        //public string home_street2 { get; set; }
        //public string home_city { get; set; }
        //public string home_state { get; set; }
        //public string home_zip { get; set; }

        public string education { get; set; }
        public string board_certification { get; set; }
        
        public string hospital_affiliation { get; set; }
        public string practice_npi { get; set; }
        public string practice_name { get; set; }
        public string practice_type { get; set; }
        public string dea { get; set; }
        public string clinician_role { get; set; }
        public string scheduling_solution { get; set; }
        public string current_scheduling { get; set; }
        //public string practice_street { get; set; }
        //public string practice_zip { get; set; }// service zip codes
        public string practice_phone_primary { get; set; } // primary phone number
        public string practice_fax { get; set; }
        public string practice_phone_cs { get; set; } // customer service number
        public string practice_phone_office { get; set; } // office phone
        public string practice_clinicians { get; set; } // no of field clinicians
        public string practice_exams { get; set; }// no of exams you can handle per week
        public string geographic_market { get; set; } // geographic market
        public string practice_expansion { get; set; } // future expansion plans, new market, 
        public string practice_insurance { get; set; } // insurance list you are in Network or will acccept
        public string practice_tax_number { get; set; } // federal tax id number

        public string primary_contact_name { get; set; }
        public string primary_contact_phone { get; set; }
        public string primary_contact_email { get; set; }

        public string operational_contact_name { get; set; }
        public string operational_contact_phone { get; set; }
        public string operational_contact_email { get; set; }

        public string financial_contact_name { get; set; }
        public string financial_contact_phone { get; set; }
        public string financial_contact_email { get; set; }

        public string practice_emr { get; set; } // emr software that you are currently using

        public List<insurance> network_insurance { get; set; } // in-network insurances
        public List<doc_specialty2> specialty { get; set; }
        public List<doc_language> language_spoken { get; set; }
        // public string practice_contact { get; set; } // primary contact/ operational contact/ financial contact
        public List<zip_search_address> home_address { get; set; }
        public List<zip_search_address> practice_address { get; set; }
        public string billing_bankname { get; set; }
        public string billing_account { get; set; }
        public string billing_routing { get; set; }
    }

    public class post_doctor_profile
    {
        public string npi { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string middle_name { get; set; }
        public string password { get; set; }
        public string is_reviewed { get; set; }
        public string status { get; set; }
        public string title { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public string personal_practice_type { get; set; }
        public string home_street1 { get; set; }
        public string home_street2 { get; set; }
        public string home_city { get; set; }
        public string home_state { get; set; }
        public string home_zip { get; set; }

        public string education { get; set; }
        public string experience { get; set; }
        public string language_spoken { get; set; }
        public string board_certification { get; set; }
        public string specialty { get; set; }
        

        public string hospital_affiliation { get; set; }
        public string practice_npi { get; set; }
        public string practice_name { get; set; }
        public string practice_type { get; set; }
        public string dea { get; set; }
        public string clinician_role { get; set; }
        public string scheduling_solution { get; set; }
        public string current_scheduling { get; set; }
        public string practice_street { get; set; }
        public string practice_city { get; set; }
        public string practice_state { get; set; }
        public string practice_zip { get; set; }// service zip codes
        public string practice_phone_primary { get; set; } // primary phone number
        public string practice_fax { get; set; }
        public string practice_phone_cs { get; set; } // customer service number
        public string practice_phone_office { get; set; } // office phone
        public string practice_clinicians { get; set; } // no of field clinicians
        public string practice_exams { get; set; }// no of exams you can handle per week
        public string geographic_market { get; set; } // geographic market
        public string practice_expansion { get; set; } // future expansion plans, new market, 
        public string practice_insurance { get; set; } // insurance list you are in Network or will acccept
        public string practice_tax_number { get; set; } // federal tax id number

        public string primary_contact_name { get; set; }
        public string primary_contact_phone { get; set; }
        public string primary_contact_email { get; set; }

        public string operational_contact_name { get; set; }
        public string operational_contact_phone { get; set; }
        public string operational_contact_email { get; set; }

        public string financial_contact_name { get; set; }
        public string financial_contact_phone { get; set; }
        public string financial_contact_email { get; set; }

        public string practice_emr { get; set; } // emr software that you are currently using
        public string network_insurance { get; set; } // in-network insurances
                                                      // public string practice_contact { get; set; } // primary contact/ operational contact/ financial contact

        public string billing_bankname { get; set; }
        public string billing_account { get; set; }
        public string billing_routing { get; set; }
    }
    public class doc
    {
        public string doctor_id { get; set; }
        public string date { get; set; }
        public List<string> time_slot { get; set; }

    }

    public class doctor_claim {
        public string npi { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public string title { get; set;}
        public string email { get; set; }
        public string password { get; set; }
        public string dea { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string clinician_role { get; set; }
        public string exam_encounter { get; set; }
        public string education { get; set; }
        public string experience { get; set; }

        public string home_street1 { get; set; }
        public string home_street2 { get; set; }
        public string home_city { get; set; }
        public string home_state { get; set; }
        public string home_zip { get; set; }
       
        public string personal_practice_type { get; set; }
        public string practice_name { get; set; }
        public string practice_street1 { get; set; }
        public string practice_street2 { get; set; }
        public string practice_city { get; set; }
        public string practice_state { get; set; }
        public string practice_zip { get; set; }
        public string practice_fax { get; set; }
        public string practice_phone { get; set; }
       public string insurance_id { get; set; }
    }
  

    // doctor/appointment/history
    public class doctor_appointment_history{
        public long id { get; set; }
        //get soul_, appointment_type, appointment_schedule

        public List<d_patient> patient { get; set; }
    
   }

    public class d_patient
    {
        public long id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public List<a_type> appointment_type { get; set; }
    }

    public class update_doctor
    {
        public long doctor_id { get; set; }
        public bool is_reviewed { get; set; }
    }

    public class a_type {
        public long id { get; set; }
        public string name { get; set; }
    }

    public class post_param
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string middlename { get; set; }
        public string acck { get; set; }
        public string zipcode { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string gender { get; set; }
        public string title { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string license { get; set; }
        public string npi { get; set; }
        public string npitype { get; set; }
        public string organization_name { get; set; }
        public string specialties { get; set; }
        public string pecos_certificate { get; set; }
        public string fax { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string bio { get; set; }
    }

    public class put_param {
        public long docId { get; set; }
        public string acck { get; set; }
        public string firstname { get; set; }

        public string lastname { get; set; }

        public string middlename { get; set; }

        public string zipcode { get; set; }

        public string address1 { get; set; }
        public string address2 { get; set; }
        public string gender { get; set; }
        public string title { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string license { get; set; }
        public string npi { get; set; }
        public string npitype { get; set; }
        public string orgname { get; set; }
        public string pecos_certificate { get; set; }
        public string fax { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string bio { get; set; }
    }
}