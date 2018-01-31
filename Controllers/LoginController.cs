using api.Models;
using authorization.hs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using api.Controllers;
using System.IO;
using Amazon.S3;
using Amazon.S3.Model;
using System.Data.Entity;

namespace api.Controllers
{
   
    public class LoginController : Base.BaseController
    {
        SV_db1Entities dbEntity = new SV_db1Entities();
        DateTime dt = DateTime.UtcNow;

        bool haserror = false;
        string errmsg = "", infomsg = "";


        [System.Web.Http.HttpPut]
        [Route("web/user/password/set")]
        public IHttpActionResult post_userpasswordset(post_userlogin param)
        {
            // created: 01/10/2018

            // email / account id
            // password

            if (!string.IsNullOrEmpty(param.email)) param.email = param.email.ToLower();



            var u = dbEntity.USERs.Where(a => a.username == param.email).FirstOrDefault();

            List<get_response_checkmail> chk = new List<get_response_checkmail>();
            if (u != null)
            {
                string salt = System.Guid.NewGuid().ToString();
                string encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(param.password + salt));

                u.tlas = salt;
                u.phash = encryp;
                dbEntity.Entry(u).State = EntityState.Modified;
                dbEntity.SaveChanges();

                //if (u.phash == encryp)
                //{
                chk.Add(new get_response_checkmail
                {
                    user_id = u.id,
                    email = u.username,
                    user_type = u.ref_USER_type == null ? "" : u.ref_USER_type.dname,
                    password_set = u.phash == null ? false : true
                });
                //}
            }

            if (chk.Count() == 0)
            {
                return Json(new { data = new string[] { }, message = "No record found.", success = false });
            }
            else
            {
                var ret1 = JsonConvert.SerializeObject(chk);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                return Json(new { data = json1, message = "Record found", success = true });
            }
        }

        [System.Web.Http.HttpPost]
        [Route("web/user/password/login")]
        public IHttpActionResult post_userpasswordlogin(post_userlogin param)
        {
            // created: 01/10/2018

            // email / account id
            // password

            if (!string.IsNullOrEmpty(param.email)) param.email = param.email.ToLower();
            var user = dbEntity.USERs.Where(a => a.username == param.email);

            List<get_response_checkmail> chk = new List<get_response_checkmail>();
            foreach (var u in user) {
                string salt = u.tlas;
                string encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(param.password + salt));
               
                if ( u.phash == encryp )
                {
                    chk.Add(new get_response_checkmail
                    {
                        user_id = u.id,
                        email = u.username,
                        user_type = u.ref_USER_type == null ? "" : u.ref_USER_type.dname,
                        password_set = u.phash == null ? false : true
                    });
                }
            }

            if (chk.Count() == 0)
            {
                return Json(new { data = new string[] { }, message = "No record found.", success = false });
            }
            else
            {
                var ret1 = JsonConvert.SerializeObject(chk);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                return Json(new { data = json1, message = "Record found.", success = true });
            }

        }



        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("web/user/signup")] // web/user/account
        public IHttpActionResult post_webuserAccount(post_webdoctorAccount param)
        {
            // create: 01/10/2018
            // param: first_name, last_name, DOB, gender, email,HSPPA authorization, Terms & condition

            try
            {
                Is_Required("first_name", param.first_name, 1);
                Is_Required("last_name", param.last_name, 1);
                Is_Required("dob", param.dob, 1);
                Is_Required("gender", param.gender, 1);
                Is_Required("email", param.email, 1);
                if (HAS_ERROR)
                {
                    return Json(new { data = new string[] { }, message = ERR_MSG, success = false });
                }

                param.email = param.email.ToLower();
                var check_email = dbEntity.USERs.Where(a => a.username == param.email);
                if (check_email.Count() > 0) { return Json(new { data = new string[] { }, message = "Email already exists.", success = false }); }


                var user_type = dbEntity.ref_USER_type.Where(a => a.dname == "Patient").FirstOrDefault();
                USER u = new USER
                {
                    name_first = param.first_name,
                    name_last = param.last_name,
                    username = param.email,
                    rel_ref_USER_type_id = user_type.id,
                    HIPAA_authorization = param.hipaa_authorization,
                    terms_condition = param.terms_conditions,
                    dt_create = dt,
                    create_by__USER_id = 0
                };
                dbEntity.USERs.Add(u);
                dbEntity.SaveChanges();

                //DOCTOR1 doc = new DOCTOR1
                //{
                //    name_first = param.first_name,
                //    name_last = param.last_name,
                //    dob = param.dob,
                //    gender = param.gender.ToCharArray()[0].ToString().ToUpper(),
                //    email = param.email,
                //    HIPAA_authorization = param.hipaa_authorization,
                //    terms_condition = param.terms_condition,
                //    dt_create = dt,
                //    create_by__USER_id = 0
                //};
                //dbEntity.DOCTOR1.Add(doc);
                //dbEntity.SaveChanges();
                SOUL s = new SOUL
                {
                    name_first = param.first_name,
                    name_last = param.last_name,
                    dob = param.dob,
                    gender = param.gender.ToCharArray()[0].ToString().ToUpper(),
                    email = param.email,

                    dt_create = dt,
                    create_by__USER_id = 0
                };

                return Json(new { data = new string[] { }, message = "New account saved successfully.", success = true });
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }

        }


        [HttpGet]
        [Route("web/user/checkemail")]
        public IHttpActionResult get_UserCheckmail([FromUri]get_usercheckmail param)
        {
            // create: 01/09/2018

            //public class get_usercheckmail
            //{
            //    public string email { get; set; }
            //}

            //public class get_response_checkmail
            //{
            //    public string user_id { get; set; }
            //    public string email { get; set; }
            //    public string user_type { get; set; }
            //    public bool is_password_set { get; set; }
            //}
            if (!string.IsNullOrEmpty(param.email)) param.email = param.email.ToLower();

            var user = dbEntity.USERs.Where(a => a.username == param.email);
            List<get_response_checkmail> chk = new List<get_response_checkmail>();
            foreach (var u in user)
            {
                chk.Add(new get_response_checkmail {
                    user_id = u.id,
                    email = u.username,
                    user_type = u.ref_USER_type ==null?"": u.ref_USER_type.dname,
                    password_set = u.phash == null? false : true
                });
            }

           
            // msg =  "Email verified."
            //msg = "Email verification is successful.";
           
            if (chk.Count() == 0)
            {
                return Json(new { data =new string[] { }, message ="No record found.", success =false });
            }
            else
            {
                var ret1 = JsonConvert.SerializeObject(chk);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                return Json(new { data = json1, message = "Record found.", success = true });
            }
        }

        // created: 01/10/2018
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
                        switch (key)
                        {
                            case "npi":
                                Is_Required(key, val, 1);
                                param.npi = val;
                                break;

                            case "image_url":
                                param.image_url = val;
                                break;

                            case "practice_name":
                                Is_Required(key, val, 1);
                                param.practice_name = val; break;
                            case "practice_phone":
                                Is_Required(key, val, 1);
                                param.practice_phone = val; break;
                            case "practice_address":
                                Is_Required(key, val, 1);
                                param.practice_address = val; break;
                            case "bio":
                                Is_Required(key, val, 1);
                                param.bio = val; break;

                            case "education":
                                Is_Required(key, val, 1);
                                param.education = val; break;
                            case "experience":
                                Is_Required(key, val, 1);
                                param.experience = val; break;
                            case "language_id":
                                Is_Required(key, val, 1);
                                param.language_id = val; break;
                            case "specialty_id":
                                Is_Required(key, val, 1);
                                param.specialty_id = val; break;

                            case "insurance_id":
                                Is_Required(key, val, 1);
                                param.insurance_id = val; break;

                            default:
                                return Json(new { message = "Object reference not set to an instance of an object. Invalid parameter name: " + key, success = false });
                        }
                    }
                }

                Is_Required("npi", param.npi, 1);
                Is_Required("practice_name", param.practice_name, 1);
                Is_Required("practice_phone", param.practice_phone, 1);
                Is_Required("practice_address", param.practice_address, 1);
                Is_Required("bio", param.bio, 1);
                Is_Required("education", param.education.ToString(), 1);
                Is_Required("experience", param.experience.ToString(), 1);
                Is_Required("language_id", param.language_id, 1);
                Is_Required("specialty_id", param.specialty_id, 1);
                Is_Required("insurance_id", param.insurance_id, 1);
                if (HAS_ERROR)
                {
                    return Json(new { data = new string[] { }, message = ERR_MSG, success = false });
                }


                //List<doc_search_profile2> dc = new List<doc_search_profile2>();
                return saveDoctor(param, provider);

                //var ret1 = JsonConvert.SerializeObject(resp);
                //var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                //return Json(new { data = new string[] { }, message = "Saved successfully.", success = true });
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }

        }

        private IHttpActionResult saveDoctor(post_webdoctorsignup doc, MultipartFileStreamProvider provider)
        {
            // 01/03/2018
            try
            {

                bool i = false;

                //var user = dbEntity.USERs.Where(a => a.username == doc.email);
                //if (user.Count() > 0)
                //{
                //    return Json(new {data =new string[] { }, message= "Email already exist.", success = false });
                //}
                //USER u = new USER {
                //    username = doc.email,
                //    name_first = doc.first_name,
                //    name_last = doc.last_name,
                //};


                var d1 = dbEntity.DOCTOR1.Where(a => a.NPI == doc.npi);

                if (d1.Count() == 0)
                {
                    return Json(new { data = new string[] { }, message = "Record not found.", success = false });
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

                if (!string.IsNullOrEmpty(doc.practice_address))
                {
                    d.practice_addr_1 = doc.practice_address;
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

                // save language
                if (!string.IsNullOrEmpty(doc.language_id))
                {
                    string[] s = doc.language_id.Split(',');
                    foreach (var e in s)
                    {
                        long e1 = 0;
                        if (long.TryParse(e, out e1))
                        {
                            con_DOCTOR1_ref_language re_lang = new con_DOCTOR1_ref_language();
                            re_lang.rel_DOCTOR_id = d.id;
                            re_lang.rel_ref_language_id = e1;
                            re_lang.create_by__USER_id = 0;
                            re_lang.dt_create = dt;
                            dbEntity.con_DOCTOR1_ref_language.Add(re_lang);

                        }
                    }
                    dbEntity.SaveChanges();
                }


                if (!string.IsNullOrEmpty(doc.exam_encounter))
                {
                    i = DoctorController.saveClaimDoctor_ext("exam_encounter", "Exam Encounter", doc.exam_encounter, d.id);
                }

                if (!string.IsNullOrEmpty(doc.clinician_role))
                {
                    i = DoctorController.saveClaimDoctor_ext("clinician_role", "Clinician Role", doc.exam_encounter, d.id);
                }



                // save insurance_id
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


                if (!string.IsNullOrEmpty(doc.education))
                {
                    string[] ed = doc.education.Split('|');
                    foreach (var e in ed)
                    {
                        i = saveClaimDoctor_ext("education", "Education", e, d.id);
                    }
                }

                if (!string.IsNullOrEmpty(doc.experience))
                {
                    string[] ex = doc.experience.Split('|');
                    foreach (var e in ex)
                    {
                        i = saveClaimDoctor_ext("experience", "Experience", e, d.id);
                    }
                }




                return Json(new { data = new string[] { }, message = "Save successfully.", success = true });
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }


        }




        [HttpGet]
        [Route("patient/email")]
        public IHttpActionResult getPatientLoginUsername([FromUri] patient_login user)
        {
            //mobile api
            // check valid email

            if (user == null)
            {
                return Json(new { data = new string[] { }, message = "Null parameter value.", success = false });
            }

            Is_Required("email", user.email, 1);
            if (HAS_ERROR)
            {
                return Json(new { data = new string[] { }, message = ERR_MSG, success = false });
            }
            else
            {
                user.email = user.email.ToLower();
            }

            if (Request !=null && Request.RequestUri.ToString().Contains("localhost"))
            {
                var u_type = dbEntity.ref_USER_type.Where(a => a.dname.ToLower() == "patient").FirstOrDefault();
            }

            var pat = dbEntity.USERs.Where(a => a.username == user.email && a.ref_USER_type.dname.ToLower() == "patient"); //user_type
            if (pat.Count() > 0)
            {
                List<patient_login_response> resp = new List<patient_login_response>();

               resp.Add(new patient_login_response{
                    is_email_available = true,
                    is_password_set = !string.IsNullOrEmpty(pat.FirstOrDefault().phash) ? true : false,
                    user_id = pat.FirstOrDefault().id
                });

                var ret1 = JsonConvert.SerializeObject(resp);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                return Json(new { data = json1, message="Record found.", success=true });
            }

            return Json(new { data = new string[] { }, message = "No record found.", success = false });
        }


        [HttpPost]
        [Route("patient/login")]
        public IHttpActionResult getPatientLoginPassword([FromBody] patient_login user)
        {
            //mobile api
            // Login

            if (user == null)
            {
                return Json(new {data = new string[] { }, message = "Null parameter value.", success = false });
            }

            IsRequired("email", user.email, 1);
            IsRequired("password", user.password, 1);

            if (haserror)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }

            user.email = user.email.ToLower();
            var pat = dbEntity.USERs.Where(a => a.username == user.email && a.ref_USER_type.dname.ToLower() == "patient"); //user_type
            if (pat.Count() > 0)
            {
                string salt = pat.FirstOrDefault().tlas;
                string encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(user.password + salt));
                var u_rec = pat.Where(a => a.phash == encryp);
                if (u_rec.Count() > 0)
                {
                    //public string first_name { get; set; }
                    //public string last_name { get; set; }
                    //public string profileImage { get; set; }
                    //public string gender { get; set; }
                    //public string dob { get; set; }
                    List<patient_login_response2> resp = new List<patient_login_response2>();
                    var soul = dbEntity.SOULs.Where(a => a.email == user.email);
                    if (soul.Count() > 0)
                    {
                        string gen = soul.FirstOrDefault().gender == null ? "" : soul.FirstOrDefault().gender;
                        string dob = soul.FirstOrDefault().dob == null ? "" : soul.FirstOrDefault().dob;
                        string img_url = soul.FirstOrDefault().image_url == null ? "" : soul.FirstOrDefault().image_url;
                        resp.Add(new patient_login_response2
                        {
                            first_name = u_rec.FirstOrDefault().name_first,
                            last_name = u_rec.FirstOrDefault().name_last,
                            gender = gen,
                            dob = dob,
                            profile_image = img_url
                        });

                    }

                    var ret1 = JsonConvert.SerializeObject(resp);
                    var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                    return Json(new { data = json1, message = "", success = true });
                }
                else // invalid password
                {
                    return Json(new { data =new string[] { }, message="Invalid password value.", success = false });
                }
            }
            else //invalid email
            {
                return Json(new { data = new string[] { }, message = "Invalid email value.", success = false });
            }
        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("patient/otp")]
        public IHttpActionResult getPatientLoginVerificationCode([FromUri] patient_login user)
        {
            //mobile api
            // send verification code 

            //var isauthorized = authorized.HSRequest(Request);
            //if (isauthorized.StatusCode == HttpStatusCode.Forbidden)
            //{
            //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized)
            //    {
            //        StatusCode = HttpStatusCode.Unauthorized,
            //    });
            //}
            if (user == null)
            {
                return Json(new { data = new string[] { }, message = "Null parameter value.", success = false });
            }

            Is_Required("email", user.email, 1);
            //IsRequired("user_id", user_id, 1);
            if (HAS_ERROR)
            {
                return Json(new { message = ERR_MSG, success = false });
            }
            else
            {
                string msg = "";
                try
                {
                    #region
                    //var _user = (from u in dbEntity.USERs
                    //             where (u.username.ToLower() == email.ToLower())
                    //             select new
                    //             {
                    //                 user = new new_USER
                    //                 {
                    //                     user_id = u.id,
                    //                     name = u.name_first + " " + u.name_last,
                    //                     username = u.username,
                    //                     status = (u.rel_ref_status_id == 7) ? true : false
                    //                 }
                    //             });
                    #endregion

                    //long user_id_new = 0;
                    //bool isValid_user = long.TryParse(user_id, out user_id_new);

                    user.email = user.email.ToLower();
                    var _user = dbEntity.USERs.Where(a=> a.username.ToLower() == user.email);
                               

                    // So we will send you the user_id and email_address and you guys just need to verify that email adress 
                    // by sending verfication link over there.

                    if (_user.Count() > 0)
                    {
                        // generate verification code
                        Random random = new Random();
                        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                        string veri_code = new string(Enumerable.Repeat(chars, 4)
                          .Select(s => s[random.Next(s.Length)]).ToArray());

                        #region
                        //USER_ext u_ext = new USER_ext();
                        //u_ext.rel_USER_id = user_id_new;
                        //u_ext.attr_name = "email_verification_code";
                        //u_ext.dname = "Email Verification Code";
                        //u_ext.rel_ref_datatype_id = 2;
                        //u_ext.value = veri_code;
                        //u_ext.create_by__USER_id = user_id_new; // user_id;
                        //u_ext.dt_create = dt;
                        //dbEntity.USER_ext.Add(u_ext);
                        //dbEntity.SaveChanges();
                        #endregion

                        _user.FirstOrDefault().verification_code = veri_code;
                        _user.FirstOrDefault().verified = false;
                        dbEntity.Entry(_user.FirstOrDefault()).State = System.Data.Entity.EntityState.Modified;
                        dbEntity.SaveChanges();

                        System.Net.Mail.MailAddress from = new System.Net.Mail.MailAddress("do-not-reply@healthsplash.com");
                        System.Net.Mail.MailAddress to = new System.Net.Mail.MailAddress(_user.FirstOrDefault().username);

                        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(from, to);
                        message.IsBodyHtml = true;
                        message.Subject = "HealthSplash Verification Code";
                        message.Body = "Verification Code: " + veri_code;

                        message.Priority = System.Net.Mail.MailPriority.Normal;

                        System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                        {
                            //UseDefaultCredentials = false,
                            Port = 587, //465,
                            Host = "box995.bluehost.com",
                            EnableSsl = true,
                            Credentials = new NetworkCredential("staff@nationalcenterforpain.com", "Staff1@#$%"),
                            Timeout = 10000
                        };
                        client.Send(message);

                        //List<patient_verification_code> code = new List<patient_verification_code>();
                        //code.Add(new patient_verification_code {
                        //    verification_code = veri_code
                        //});

                        //var ret1 = JsonConvert.SerializeObject(code);
                        //var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                        msg = "Verification code";
                        return Json(new { data = new string[] { }, message = msg, success = true });
                    }
                    else
                    {
                        // msg = "No result found.";
                        msg = "This email doesn't exist.";
                        return Json(new { data = new string[] { }, message = msg, success = false });
                    }
                }
                catch (ArgumentNullException ex)
                {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
                catch (ObjectDisposedException ex)
                {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
                catch (System.Net.Mail.SmtpException ex)
                {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
                catch (Exception ex)
                {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
            }
        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("patient/verify_code")]
        public IHttpActionResult getverifycode([FromUri] patient_verify_code user)
        {
            //mobile api
            // validate verification code

            if (user == null)
            {
                return Json(new {data =new string[] { }, message = "Null parameter value.", success = false });
            }

            Is_Required("email", user.email, 1);
            Is_Required("verification_code", user.verification_code, 1);
            if (HAS_ERROR)
            {
                return Json(new { message = ERR_MSG, success = false });
            }

            string msg = "";
            user.email = user.email.ToLower();

            var u_user = dbEntity.USERs.Where(a => a.username == user.email);

            if (u_user.Count() > 0)
            {
                long u_user_id = u_user.FirstOrDefault().id;
                //var u_ext = dbEntity.USER_ext.Where(a => a.attr_name == "email_verification_code" && a.value == verification_code);
                u_user = u_user.Where(a => a.verification_code == user.verification_code);
                if (u_user.Count() > 0)
                {
                    //return Json(new { data = new string[] { }, message = "", success = true });
                    u_user.FirstOrDefault().verified = true;
                    u_user.FirstOrDefault().dt_update = dt;
                    dbEntity.Entry(u_user.FirstOrDefault()).State = System.Data.Entity.EntityState.Modified;
                    dbEntity.SaveChanges();

                    //List<u_verify> n = new List<u_verify>();
                    //n.Add(new u_verify
                    //{
                    //    id = u_user_id
                    //});
                    
                    //var ret1 = JsonConvert.SerializeObject(n);
                    //var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                    // msg =  "Email verified."
                    msg = "Email verification is successful.";
                    return Json(new { data = new string[] { }, message = msg, success = true });
                }

                return Json(new { data = new string[] { }, message = "Invalid verification code.", success = false });
            }
            // msg = "Invalid email."
            msg = "This email doesn’t exist.";
            return Json(new { data = new string[] { }, message = msg, success = false });
        }


        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("patient/password/new")]
        public async Task<IHttpActionResult> putPatientPasswordNew([FromBody] patient_password user)
        {
            // set password

            //var isauthorized = authorized.HSRequest(Request);
            //if (isauthorized.StatusCode == HttpStatusCode.Forbidden)
            //{
            //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized)
            //    {
            //        StatusCode = HttpStatusCode.Unauthorized,
            //    });
            //}

            Is_Required("user_id", user.user_id > 0 ? user.user_id.ToString() : "", 2);
            Is_Required("password", user.password, 1);
            
            if (HAS_ERROR)
            {
                return Json(new { data= new string[] { }, message = ERR_MSG, success = false });
            }


            //long user_id_new = Convert.ToInt64(user_id);
            USER update_user = dbEntity.USERs.Find(user.user_id);
            if (update_user != null)
            {
                string salt = System.Guid.NewGuid().ToString();
                string encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(user.password + salt));
                update_user.phash = encryp;
                update_user.tlas = salt;
                update_user.dt_update = dt;
                update_user.update_by__USER_id = user.user_id;
                dbEntity.Entry(update_user).State = System.Data.Entity.EntityState.Modified;
                dbEntity.SaveChanges();

                var soul = dbEntity.SOULs.Where(a => a.email == update_user.username);
                string image_url = "", gender = "", dob = "";
                long soul_id = 0;
                foreach (var s in soul)
                {
                    image_url = s.image_url == null? "" : s.image_url;
                    gender = s.gender == null? "" : s.gender;
                    dob = s.dob == null ? "" : s.dob;
                    //var soul_ex = dbEntity.SOUL_ext.Where(a =>a.attr_name =="insurance_id");
                    soul_id = s.id;
                }

                List<patient_password_response> resp = new List<patient_password_response> ();
                resp.Add(new patient_password_response {
                    first_name = update_user.name_first,
                    last_name = update_user.name_last,
                    profile_image = image_url,
                    gender = gender,
                    dob = dob,
                    insurance = custom.getInsuranceBy_id(soul_id)
                });

                infomsg = "Password successfully saved.";

                //var ret1 = JsonConvert.SerializeObject(n);
                //var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                return Json(new { data = resp, message = infomsg, success = true });
            }
            
            return Json(new { data = new string[] { }, message = infomsg, success = false });
            //catch (Exception ex)
            //{
            //    return Json(new { data = new string[] { }, message = ex.Message, success = false });
            //}
        }

        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("patient/password/update")]
        public IHttpActionResult putPatientPasswordUpdate([FromBody] patient_password user)
        { // update password

            //var isauthorized = authorized.HSRequest(Request);
            //if (isauthorized.StatusCode == HttpStatusCode.Forbidden)
            //{
            //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized)
            //    {
            //        StatusCode = HttpStatusCode.Unauthorized,
            //    });
            //}

            Is_Required("user_id", user.user_id.ToString(), 2);
            Is_Required("password", user.password, 1);

            if (HAS_ERROR)
            {
                return Json(new {data=new string[] { }, message = ERR_MSG, success = false });
            }


            //long user_id_new = Convert.ToInt64(user_id);
            USER update_user = dbEntity.USERs.Find(user.user_id);
            if (update_user != null)
            {
                string salt = System.Guid.NewGuid().ToString();
                string encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(user.password + salt));
                update_user.phash = encryp;
                update_user.tlas = salt;
                update_user.dt_update = dt;
                update_user.update_by__USER_id = user.user_id;
                dbEntity.Entry(update_user).State = System.Data.Entity.EntityState.Modified;
                dbEntity.SaveChanges();

                //var soul = dbEntity.SOULs.Where(a => a.email == update_user.username);
                //string image_url = "", gender = "", dob = "";
                //long soul_id = 0;
                //foreach (var s in soul)
                //{
                //    image_url = s.image_url;
                //    gender = s.gender;
                //    dob = s.dob;
                //    //var soul_ex = dbEntity.SOUL_ext.Where(a =>a.attr_name =="insurance_id");

                //}

                //List<patient_password_response> resp = new List<patient_password_response>();
                //resp.Add(new patient_password_response
                //{
                //    first_name = update_user.name_first,
                //    last_name = update_user.name_last,
                //    gender = gender,
                //    dob = dob,
                //    insurance = getInsuranceBy_id(soul_id)
                //});

                infomsg = "Password successfully updated.";
                return Json(new { data = new string[] { } , message = infomsg, success = true });
            }

            //"patient/password/update" 
            return Json(new { data = new string[] { }, message = infomsg, success = true });
            //catch (Exception ex)
            //{
            //    return Json(new { data = new string[] { }, message = ex.Message, success = false });
            //}
        }


        [HttpPost]
        [Route("patient/profile/signup")]
        public IHttpActionResult postPatientSignup([FromBody] patient_signup patient)
        { // Signup

            Is_Required("email", patient.email, 1);
            Is_Required("password", patient.password, 1);
            Is_Required("name_first", patient.name_first,1);
            Is_Required("name_last", patient.name_last, 1);
            Is_Required("dob", patient.dob, 1);
            Is_Required("gender", patient.gender, 1);
            //IsRequired("insurance_id", patient.insurance_id.ToString(), 1);

            if (HAS_ERROR)
            {
                return Json(new {data = new string[] { }, message = ERR_MSG, success = false });
            }

            if (!string.IsNullOrEmpty(patient.email)) patient.email = patient.email.ToLower();
            var user = dbEntity.USERs.Where(a => a.username == patient.email);
            if (user.Count() > 0)
            {
                return Json(new { data = new string[] { }, message = "Email already existed.", success = false });
            }


            bool b = _savePatientSignup(patient);
            string msg = "";
            if (b) { msg = "Successfully saved."; }
            else { msg = "Error encountered in saving."; }

                return Json(new { data = new string[] { }, message = msg, success = b });
        }



        [HttpPut]
        [Route("patient/profile/update")]
        public IHttpActionResult putPatientUpdate([FromBody] patient_signup patient)
        { // Update Profile

            Is_Required("user_id", patient.user_id.ToString(), 1);
            //IsRequired("password", patient.password, 1);
            //IsRequired("name_first", patient.name_first, 1);
            //IsRequired("name_last", patient.name_last, 1);
            //IsRequired("dob", patient.dob, 1);
            //IsRequired("gender", patient.gender, 1);
            //IsRequired("insurance_id", patient.insurance_id.ToString(), 1);

            if (HAS_ERROR)
            {
                return Json(new { data = new string[] { }, message = ERR_MSG, success = false });
            }

            // if (!string.IsNullOrEmpty(patient.email)) patient.email = patient.email.ToLower();
            // var user = dbEntity.USERs.Where(a => a.username == patient.email);

            // date of birth
            if (!string.IsNullOrEmpty(patient.dob))
            {
                //12/12/1990
                string s = Validation.validateDate(patient.dob);
                if (s.Length > 0)
                {
                    //bool i = Validation.saveSOUL_ext("dob", "Date Of Birth", s, _soul.id, patient1.user_id);
                }
                else
                { return Json(new { data = new string[] { }, message = "Invalid dob value. Acceptable date format: 'MM/dd/yyyy'", success = false }); }
            }

   
            var user = dbEntity.USERs.Find(patient.user_id);
            string msg = "";

            if (user != null)
            {
                //email = user.username;

                //var soul = dbEntity.SOULs.Where(a => a.email == email);

                //foreach (var s in soul)
                //{
                    //patient.patient_id = s.id;
                    bool b = _savePatientSignup(patient);
                     if (b) { msg = "Successfully updated."; }
                    else { msg = "Error encountered in updating record."; }

                //}

                return Json(new { data = new string[] { }, message = msg, success = b });
                //return Json(new { data = new string[] { }, message = "Email already existed.", success = false });
            }


            return Json(new { data = new string[] { }, message = msg, success = false });
        }

        string new_filename;

        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("patient/profile/pic")]
        public async Task<IHttpActionResult> uploadprofilepic()
        { // update profile image

            //var isauthorized = authorized.HSRequest(Request);
            //if (isauthorized.StatusCode == HttpStatusCode.Forbidden)
            //{
            //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized)
            //    {
            //        StatusCode = HttpStatusCode.Unauthorized,
            //    });
            //}

            string user_id = null, patient_id =null, filename = "";
            long new_patient_id = 0;
            byte[] bytes = { };

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
                        if (key == "patient_id")
                        {
                            IsRequired(key, val, 1);
                            patient_id = val;
                        }
                        else
                        {
                            return Json(new { message = "Object reference not set to an instance of an object. Invalid parameter name: " + key, success = false });
                        }
                    }
                }


                bool bTemp = long.TryParse(patient_id, out new_patient_id);
                var pat_id = dbEntity.SOULs.Find(new_patient_id);
                if (pat_id != null)
                {
                    var user = dbEntity.USERs.Where(a => a.username == pat_id.email);
                    if (user.Count() > 0)
                    {
                        user_id = user.FirstOrDefault().id.ToString();
                    }
                }
                IsRequired("patient_id", patient_id, 2);
                bool is_upload = uploadpic(provider, user_id);
                IsRequired("image", new_filename, 2);

             


                long new_user_id = Convert.ToInt64(user_id);

                if (haserror)
                {
                    return Json(new { message = errmsg, success = false });
                }
                else if (!is_upload)
                {
                    return Json(new { message = "Error uploading image.", success = false });
                }
                else
                {
                    //bool hassave = false, hasupdate = false;

                    //race
                    var photo_USER = (from a in dbEntity.USER_ext
                                      where (a.rel_USER_id == new_user_id && a.attr_name == "image")
                                      select a.id);
                    if (photo_USER.Count() > 0)
                    {
                        USER_ext updateUSER_photo = dbEntity.USER_ext.First(a => a.rel_USER_id == new_user_id && a.attr_name == "image");
                        updateUSER_photo.value = new_filename;
                        updateUSER_photo.update_by__USER_id = Convert.ToInt64(user_id);
                        updateUSER_photo.dt_update = dt;
                        dbEntity.Entry(updateUSER_photo).State = System.Data.Entity.EntityState.Modified;
                        dbEntity.SaveChanges();
                        //hasupdate = true;
                    }
                    else
                    {
                        USER_ext addUSER_photo = new USER_ext
                        {
                            attr_name = "image",
                            dname = "Image",
                            value = new_filename,
                            rel_USER_id = new_user_id,
                            rel_ref_datatype_id = 2,
                            dt_create = dt,
                            create_by__USER_id = Convert.ToInt64(user_id)
                        };
                        dbEntity.USER_ext.Add(addUSER_photo);
                        dbEntity.SaveChanges();
                        //hassave = true;
                    }
                    //if (hasupdate)
                    //{
                    //    infomsg = "Image updated with this user_id: " + new_user_id;
                    //}
                    //if (hassave)
                    //{
                    //    infomsg = "Image saved with this user_id: " + new_user_id;
                    //}
                    string path = "https://s3-ap-southeast-1.amazonaws.com/hsrecs/images/" + new_filename;
                    List<img_url> img_url = new List<img_url>();
                    img_url.Add(new img_url { image_url = path });

                    infomsg = "Photo uploaded successfully";
                    return Json(new { data = img_url, message = infomsg, success = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }


        public bool uploadpic(MultipartFileStreamProvider f, string id)
        {
            try
            {
                string filename = "";
                string path = HttpContext.Current.Server.MapPath("~/Content/Temp/");
                Random rand = new Random((int)DateTime.Now.Ticks);
                int numIterations = 0;
                foreach (MultipartFileData file in f.FileData)
                {
                    numIterations = rand.Next(1, 99999);
                    string ext = file.Headers.ContentDisposition.FileName.Split('.')[1];
                    filename = numIterations + "-" + id + "." + ext.Replace("\"", "");
                    string new_path = path + filename.Replace("\"", "");
                    File.Move(file.LocalFileName, new_path);
                    try
                    {
                        AmazonS3Client S3Client = null;
                        Amazon.S3.AmazonS3Config S3Config = new Amazon.S3.AmazonS3Config()
                        {
                            ServiceURL = "http://s3-external-1.amazonaws.com"
                        };
                        string accessKey = System.Web.Configuration.WebConfigurationManager.AppSettings["AWSaccessKey"],
                               secretAccessKey = System.Web.Configuration.WebConfigurationManager.AppSettings["AWSsecretAccessKey"],
                               filePath = new_path,
                               newFileName = filename;
                        S3Client = new AmazonS3Client(accessKey, secretAccessKey, S3Config);
                        var s3PutObject = new PutObjectRequest()
                        {
                            FilePath = filePath,
                            BucketName = "hsrecs" + "/images",
                            CannedACL = S3CannedACL.PublicRead
                        };
                        if (!string.IsNullOrWhiteSpace(newFileName))
                        {
                            s3PutObject.Key = newFileName;
                        }
                        s3PutObject.Headers.Expires = new DateTime(2020, 1, 1);

                        PutObjectResponse s3PutResponse = S3Client.PutObject(s3PutObject);

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }


                }
                new_filename = filename;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        private bool _savePatientSignup(patient_signup p) {
            try {

                if (p.user_id > 0)
                {
                    var u_rec = dbEntity.USERs.Find(p.user_id);

                    if (u_rec != null)
                    {
                        var soul = dbEntity.SOULs.Where(a => a.email == u_rec.username).FirstOrDefault();

                        //if(!string.IsNullOrEmpty(p.email)) u_rec.username = p.email;
                        if(!string.IsNullOrEmpty(p.name_first)) u_rec.name_first = p.name_first;
                        if(!string.IsNullOrEmpty(p.name_last)) u_rec.name_last = p.name_last;
                        //u_rec.phash = encryp;
                        //u_rec.tlas = salt;
                        u_rec.create_by__USER_id = 0;
                        u_rec.rel_ref_USER_type_id = 1; // patient
                        u_rec.dt_create = dt;
                        dbEntity.Entry(u_rec).State = System.Data.Entity.EntityState.Modified;
                        dbEntity.SaveChanges();


                        //if (!string.IsNullOrEmpty(p.email1)) soul.email = p.email1;
                        if (!string.IsNullOrEmpty(p.name_first)) soul.name_first = p.name_first;
                        if (!string.IsNullOrEmpty(p.name_last)) soul.name_last = p.name_last;
                        if (!string.IsNullOrEmpty(p.dob)) soul.dob = p.dob;
                        if (!string.IsNullOrEmpty(p.gender)) soul.gender = p.gender.ToCharArray()[0].ToString().ToUpper();
                        soul.dt_create = dt;
                        dbEntity.Entry(soul).State = System.Data.Entity.EntityState.Modified;
                        dbEntity.SaveChanges();

                        //// save [insurance] to [con_SOUL_ref_insurance]
                        //con_SOUL_ref_insurance s_ins = new con_SOUL_ref_insurance
                        //{
                        //    rel_SOUL_id = soul.id,
                        //    rel_ref_insurance_provider_id = p.insurance_id,
                        //    dt_create = dt
                        //};
                        //dbEntity.con_SOUL_ref_insurance.Add(s_ins);
                        //dbEntity.SaveChanges();
                    }
                    
                }
                else
                {
                    // save to [USER]
                    string salt = System.Guid.NewGuid().ToString();
                    string encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(p.password + salt));

                    USER u_rec = new USER();
                    u_rec.username = p.email;
                    u_rec.name_first = p.name_first;
                    u_rec.name_last = p.name_last;
                    u_rec.phash = encryp;
                    u_rec.tlas = salt;

                    u_rec.create_by__USER_id = 0;
                    u_rec.rel_ref_USER_type_id = 1; // patient
                    u_rec.dt_create = dt;
                    dbEntity.USERs.Add(u_rec);
                    dbEntity.SaveChanges();

                    // save to [SOUL]
                    SOUL soul = new SOUL();
                    soul.email = p.email;
                    soul.name_first = p.name_first;
                    soul.name_last = p.name_last;
                    soul.dob = p.dob;
                    soul.gender = p.gender.ToCharArray()[0].ToString().ToUpper();
                    soul.dt_create = dt;
                    dbEntity.SOULs.Add(soul);
                    dbEntity.SaveChanges();

                    //// save [insurance] to [con_SOUL_ref_insurance]
                    //con_SOUL_ref_insurance s_ins = new con_SOUL_ref_insurance
                    //{
                    //    rel_SOUL_id = soul.id,
                    //    rel_ref_insurance_provider_id = p.insurance_id,
                    //    dt_create = dt
                    //};
                    //dbEntity.con_SOUL_ref_insurance.Add(s_ins);
                    //dbEntity.SaveChanges();
                }

               

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // non-mobile

        [HttpPost]
        [Route("login/social")]
        //public async Task<IHttpActionResult> Postsocial()
        public IHttpActionResult Postsocial([FromBody] param_loginsocial param1)
        {

            try
            {
                //string root = HttpContext.Current.Server.MapPath("~/Temp");
                //var provider = new MultipartFormDataStreamProvider(root);

                //string social_id = param1.social_id == null ? "" : param1.social_id,
                //    social_type = param1.social_type == null ? "" : param1.social_type,
                //    password = param1.password == null ? "" : param1.password,
                //    first_name = param1.first_name == null ? "" : param1.first_name,
                //    last_name = param1.last_name == null ? "" : param1.last_name,
                //    email = param1.email == null ? "" : param1.email;

                //long insurance_id = param1.insurance_id ==null?0: param1.insurance_id;
                //string user_type = "", device_type = "", device_token = "", platform;
                string msg = "";
                DateTime dtNow = DateTime.Now;

                #region "old param"
                //await Request.Content.ReadAsMultipartAsync(provider);
                //foreach (var key in provider.FormData.AllKeys)
                //{
                //    foreach (var val in provider.FormData.GetValues(key))
                //    {

                //        switch (key)
                //        {
                //            //string email = "", password = "", user_type = "";
                //            //string acck = "deftsoftapikey", device_type = "", device_token = "";
                //            case "social_id":
                //                IsRequired(key, val, 1);
                //                social_id = val;
                //                break;
                //            case "social_type":
                //                // social_type: facebook/ google
                //                IsRequired(key, val, 1);
                //                social_type = val;
                //                break;
                //            case "firstname":
                //                IsRequired(key, val, 1);
                //                firstname = val;
                //                break;
                //            case "lastname":
                //                IsRequired(key, val, 1);
                //                lastname = val;
                //                break;
                //            case "user_type":
                //                //IsRequired(key, val, 1);
                //                user_type = val;
                //                break;

                //            case "platform":
                //                platform = val;
                //                break;

                //            case "deviceToken":
                //                device_token = val;
                //                break;

                //            // optional params
                //            //firstname, lastname, email, password, insurance_id, user_type (doctor/patient/user),device_type, device_token

                //            case "password":
                //                password = val;
                //                break;
                //            case "email":
                //                email = val;
                //                break;
                //            case "insurance_id":
                //                insurance_id = val;
                //                break;

                //            //case "device_type":
                //            //    device_type = val;
                //            //    break;


                //            default:
                //                msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                //                return Json(new { data = new string[] { }, message = msg, success = false });
                //        }
                //    }
                //}
                #endregion

                IsRequired("social_id", param1.social_id, 2);
                IsRequired("social_type", param1.social_type, 2);
                //IsRequired("user_type", user_type, 2);
                IsRequired("first_name", param1.first_name, 2);
                IsRequired("last_name", param1.last_name, 2);

                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }

                // check if user status if Active
                // ref_user_type: 1	Patient, 2 Doctor, 3 User, 4 Admin
                var u_login = dbEntity.USERs.Where(a => a.social_id == param1.social_id &&  a.ref_USER_type.dname.ToLower() == "patient");
               
                // changed from "user"

                if (u_login.Count() == 0)
                {
                    // since the login does not exist yet
                    // this is where we INSERT it to [USER] table

                    // password,
                    
                    // OPTIONAL PARAMETERS:
                    //firstname, lastname, email, password, insurance_id, user_type (doctor/patient/user),device_type, device_token
                    return SaveToUSER(param1);
                 

                }
                else
                {
                    if (u_login.FirstOrDefault().rel_ref_status_id != 7)
                    {
                        msg = "User login is not Active.";
                        return Json(new { data = new string[] { }, message = msg, success = false });
                    }
                    else // status_id = active
                    {
                        // encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + user_rec.FirstOrDefault().tlas));
                        //var u_rec = user_rec.Where(a => a.phash == encryp).FirstOrDefault();

                        //if (u_rec != null)
                        //{

                        List<get_Login2> logs = new List<get_Login2>();
                        var ref_stat = dbEntity.ref_status_type.Where(a => a.dname == "USER").FirstOrDefault();
                        var ref_dname = ref_stat.ref_status.FirstOrDefault(b => b.id == u_login.FirstOrDefault().rel_ref_status_id).dname;
                        //var ref =    dbEntity.ref_status_type.Where(a => a.dname == "USER").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname.ToLower() == status.ToLower()).id;

                        string zipcode = "", insuranceid="", insurancename="";
                        //bool user_insurance = false;
                        var p_user = dbEntity.SOULs.Where(b => b.create_by__USER_id == u_login.FirstOrDefault().id);
                        List<user_secondary_patient> pat = new List<user_secondary_patient>();
                        if (p_user.Count() > 0)
                        {
                         
                            foreach (var n in p_user)
                            {
                                var _stat = dbEntity.ref_status.Find(n.rel_ref_status_id );
                                if (_stat.dname.ToLower() != "deleted") {
                                    var p_ext = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == n.id && a.attr_name == "insurance_id");
                                    var u_user = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == n.id && a.attr_name == "use_user_insurance");
                                    long ins_id = 0;
                                    string ins_id_string = "", ins_name = "";
                                    bool use_insurance = false;

                                    if (u_user.Count() > 0)
                                    {
                                        if (u_user.FirstOrDefault().value == "true")
                                            use_insurance = true;
                                        else
                                            use_insurance = false;
                                    }

                                    if (use_insurance == false && p_ext.Count() > 0)
                                    {

                                        // get insurance_id
                                        ins_id_string = p_ext.FirstOrDefault().value;
                                        if (p_ext.Count() > 0)
                                        {
                                            bool btemp = long.TryParse(ins_id_string, out ins_id);
                                            if (btemp)
                                            {
                                                var ins_ref = dbEntity.ref_insurance_provider.Find(ins_id);
                                                if (ins_ref != null)
                                                {
                                                    ins_name = (ins_ref.PayerName.Split('|')[0]).Trim();
                                                }
                                                else
                                                {
                                                    ins_name = "";
                                                }
                                            }
                                            else
                                            {
                                                ins_name = "";
                                            }

                                        }
                                        else
                                        {
                                            //msg = "No matching insurance record found.";
                                            //return Json(new { data = "", message = msg, success = false });
                                            //if (u_user.Count() > 0)
                                            //{
                                            //    use_insurance = true;
                                            //}
                                        }
                                    }

                                    if (n.is_prime.Value)
                                    {
                                        zipcode = n.ref_zip == null ? "" : n.ref_zip.zip;
                                        insuranceid = ins_id_string;
                                        insurancename = ins_name;
                                    }
                                    else //8-29-2017 removing primary patient in the patient list
                                    {
                                        pat.Add(new user_secondary_patient
                                        {
                                            id = n.id,
                                            first_name = n.name_first == null ? "" : n.name_first,
                                            last_name = n.name_last == null ? "" : n.name_last,
                                            is_prime = n.is_prime.Value,
                                            is_using_primary_patient_insurance = use_insurance,
                                            insurance_id = ins_id_string,  //p_ext.Count() == 0 ? "" : p_ext.FirstOrDefault().value,
                                            insurance_name = ins_name
                                        });
                                    }
                                  
                                }
                               
                            }
                        }
                        
                        var _image = dbEntity.USER_ext.Where(a => a.rel_USER_id == u_login.FirstOrDefault().id && a.attr_name == "image").FirstOrDefault();
                        var _social_type = dbEntity.USER_ext.Where(b => b.rel_USER_id == u_login.FirstOrDefault().id && b.attr_name == "social_type");

                        // get social_id reference in SOUL_ext
                        var s_user_ext = dbEntity.SOUL_ext.Where(b => b.attr_name == "social_id" && b.value == param1.social_id);
                        
                        long soul_id = 0; //string _socialtype = "";
                        if (s_user_ext.Count() > 0)
                        {
                            soul_id = s_user_ext.FirstOrDefault().rel_SOUL_id.Value;
                            
                        }

                        var s_user = dbEntity.SOULs.Find(soul_id);
                        
                        // get SOUL record
                        login_get_SOUL_info soul = get_SOUL_info(soul_id);
                        // 

                       

                        logs.Add(new get_Login2
                        {
                            user_id = u_login.FirstOrDefault().id,
                            first_name = u_login.FirstOrDefault().name_first,
                            last_name = u_login.FirstOrDefault().name_last,
                            username = u_login.FirstOrDefault().username,
                            password = param1.password,
                            //verification_code = "",
                            image_url = _image == null ? "" : "https://s3-ap-southeast-1.amazonaws.com/hsrecs/images/" + _image.value,
                            phone_no = s_user.phone == null ? "" : s_user.phone,
                            street = s_user.addr_address1 == null ? "" : s_user.addr_address1,
                            zip_code =  zipcode,
                            gender = s_user.gender == null ? "" : s_user.gender.ToUpper(),

                            //dob = isDoBvalid == false ? "" : dt_DOB.ToString("yyyy-MM-dd"),
                            dob = soul.dob == null ? "" : soul.dob,
                            parent_guardian = soul.parent_guardian == null ? "" : soul.parent_guardian,
                            city = soul.city == null ? "" : soul.city,
                            state = soul.state == null ? "" : soul.state,
                            height = soul.height == null ? 0 : Convert.ToDouble(soul.height),
                            weight = soul.weight == null ? 0 : Convert.ToDouble(soul.weight),
                            emergency_number = soul.emergency_number == null ? "" : soul.emergency_number,
                            note = soul.note == null ? "" : soul.note,
                            //status = ref_stat == null ? "" : ref_dname,
                            social_id = u_login.FirstOrDefault().social_id,
                            social_type = _social_type == null? "" : _social_type.FirstOrDefault().value, //social_type,
                            insurance_id = insuranceid,
                            insurance_name= insurancename,
                            
                            patient = pat
                        });

                        var ret1 = JsonConvert.SerializeObject(logs);
                        //[{"id":10,"firstname":"marding","lastname":"nizari","username":"mardingnizari","status":"Active"}]

                        //var ret1 = "[{\"id\":10,\"firstname\":\"marding\",\"lastname\":\"nizari\",\"username\":\"mardingnizari\",\"\":\"Active\"}]"
                        var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                        msg = "Successful login.";
                        return Json(new { data = json1, message = msg, success = true });
                        //}
                    }
                }

                //if (u_login.FirstOrDefault().rel_ref_status_id != 7)
                //{ }
                //else
                //{
                //    #region
                //    if (u_login.Count() > 0)
                //    { }
                //    else
                //    { }
                //    #endregion
                //    return Json(new { data = "", message = errmsg, success = false });
                //}
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }


        private login_get_SOUL_info get_SOUL_info(long soul_id)
        {
            //List<login_get_SOUL_info> soul_info = new List<login_get_SOUL_info>();
            login_get_SOUL_info info = new login_get_SOUL_info();

            //var s_user = dbEntity.SOULs.Where(a => a.email == u_login.FirstOrDefault().username).FirstOrDefault();
            
            //
            //var u_phone = dbEntity.SOUL_ext.Where(a => a.attr_name == "phone_no" && a.rel_SOUL_id == s_user.id).FirstOrDefault();
            var soul_ex = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == soul_id);
            foreach (var s in soul_ex)
            {
                switch (s.attr_name)
                {
                    case "city":
                        info.city = s.value;
                        break;
                    case "state":
                        info.state = s.value;
                        break;
                    case "dob":
                        info.dob = s.value;
                        break;
                    case "height":
                        info.height = s.value;
                        break;
                    case "weight":
                        info.weight = s.value;
                        break;
                    case "emergency_number":
                        info.emergency_number = s.value;
                        break;
                    case "parent_guardian":
                        info.parent_guardian = s.value;
                        break;
                    case "note":
                        info.note = s.value;
                        break;
                }
            }

            //var u_city = dbEntity.SOUL_ext.Where(a => a.attr_name == "city" && a.rel_SOUL_id == s_user.id).FirstOrDefault();
            //var u_state = dbEntity.SOUL_ext.Where(a => a.attr_name == "state" && a.rel_SOUL_id == s_user.id).FirstOrDefault();
            //var u_dob = dbEntity.SOUL_ext.Where(a => a.attr_name == "dob" && a.rel_SOUL_id == s_user.id).FirstOrDefault();
            //DateTime dt_DOB = DateTime.Now;
            //bool isDoBvalid = false;
            //if (u_dob != null)
            //{
            //    isDoBvalid = DateTime.TryParse(u_dob.value, out dt_DOB);
            //    //string n = dt_DOB.ToString("yyyy-MM-dd");
            //}
            //var u_height = dbEntity.SOUL_ext.Where(a => a.attr_name == "height" && a.rel_SOUL_id == s_user.id).FirstOrDefault();
            //var u_weight = dbEntity.SOUL_ext.Where(a => a.attr_name == "weight" && a.rel_SOUL_id == s_user.id).FirstOrDefault();
            //var u_emergency = dbEntity.SOUL_ext.Where(a => a.attr_name == "emergency_number" && a.rel_SOUL_id == s_user.id).FirstOrDefault();
            //var u_guardian = dbEntity.SOUL_ext.Where(a => a.attr_name == "parent_guardian" && a.rel_SOUL_id == s_user.id).FirstOrDefault();
            //var u_note = dbEntity.SOUL_ext.Where(a => a.attr_name == "note" && a.rel_SOUL_id == s_user.id).FirstOrDefault();
            ////


            return info;
        }


        // long user_id=0,
        //public IHttpActionResult SaveToUSER(string social_id, string social_type, string user_type, DateTime dtNow, string firstname, string lastname, string password = "", string email = "", long insurance_id = 0, string device_type = "", string device_token = "")
        public IHttpActionResult SaveToUSER(param_loginsocial login)
        {
            long added_user;

            string salt = "";
            string encryp = "";

            try
            {

                // oct 23 2017 'check if exist
                var u = dbEntity.USERs.Where(a => a.social_id == login.social_id);

                 var u_type = dbEntity.ref_USER_type.Where(a => a.dname.ToLower() == "patient").FirstOrDefault();
                //var u_type = dbEntity.ref_USER_type.Where(a => a.id == u.FirstOrDefault().rel_ref_USER_type_id);
                //if (u_type.FirstOrDefault().dname.ToLower() == "doctor")
                //{
                //    // 
                //}
                
                USER new_user = new USER();
                if (u.Count() == 0)
                {
                   
                    //required fields
                    new_user.social_id = login.social_id;
                    new_user.name_first = login.first_name;
                    new_user.name_last = login.last_name;
                    new_user.username = string.IsNullOrEmpty(login.email) == true ? "" : login.email.ToLower();

                    new_user.dt_create = dt;
                    //new_user.social_id = login.social_id;
                    new_user.create_by__USER_id = 0; //create != null ? 0 : create.id;
                    new_user.rel_ref_status_id = dbEntity.ref_status_type.Where(a => a.dname.ToLower() == "user").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname == "Active").id;
                    new_user.rel_ref_USER_type_id = u_type.id;
                    new_user.phash = string.IsNullOrEmpty(encryp) == true ? null : encryp;
                    new_user.tlas = string.IsNullOrEmpty(salt) == true ? null : salt;
                    // insurance_type
                    // user_type

                    dbEntity.USERs.Add(new_user);
                    dbEntity.SaveChanges();

                    added_user = new_user.id;
                }
                else // if social_id exist
                {
                    new_user = u.FirstOrDefault();
                    new_user.name_first = string.IsNullOrEmpty(login.first_name) ==true? "": login.first_name;
                    new_user.name_last = string.IsNullOrEmpty(login.last_name) == true ? "" : login.last_name;
                    new_user.username = string.IsNullOrEmpty(login.email) == true ? "" : login.email.ToLower();

                    new_user.dt_update = dt;
                    //new_user.social_id = login.social_id;
                    new_user.create_by__USER_id = 0; //create != null ? 0 : create.id;
                    new_user.rel_ref_status_id = dbEntity.ref_status_type.Where(a => a.dname.ToLower() == "soul").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname == "Active").id;
                    new_user.rel_ref_USER_type_id = u_type.id;
                    new_user.phash = string.IsNullOrEmpty(encryp) == true ? null : encryp;
                    new_user.tlas = string.IsNullOrEmpty(salt) == true ? null : salt;

                    dbEntity.Entry(new_user).State = System.Data.Entity.EntityState.Modified;
                    dbEntity.SaveChanges();
                }
           

                //string root = HttpContext.Current.Server.MapPath("~/Content/Temp/");
                //var provider = new MultipartFormDataStreamProvider(root);
                //bool is_upload = PatientController.uploadpic(provider, added_user.ToString());

                if (!string.IsNullOrEmpty(login.device_type))
                {
                    bool i = Validation.saveUSER_ext("device_type", "Device Type", login.device_type, new_user.id);
                }

                if (!string.IsNullOrEmpty(login.device_token))
                {
                    bool i = Validation.saveUSER_ext("device_token", "Device Token", login.device_token, new_user.id);
                }

                if (!string.IsNullOrEmpty(login.social_type))
                {
                    bool i = Validation.saveUSER_ext("social_type", "Social Type", login.social_type, new_user.id);
                }

               
                // save USER to SOUL table
                SOUL new_soul = new SOUL();

                var n_ext = dbEntity.SOUL_ext.Where( a => a.attr_name =="social_id" && a.value == new_user.social_id);
                if (n_ext.Count() == 0)
                {
                    new_soul.name_first = login.first_name;
                    new_soul.name_last = login.last_name;
                    new_soul.email = string.IsNullOrEmpty(login.email) == true ? null : login.email.ToLower();
                    new_soul.is_prime = true;

                    #region

                    //if (!string.IsNullOrEmpty(phone_number))
                    //{
                    //    new_soul.phone = phone_number;
                    //}

                    //if (!string.IsNullOrEmpty(address))
                    //{
                    //    new_soul.addr_address1 = address;
                    //}
                    #endregion


                    new_soul.dt_create = dt;
                    new_soul.rel_ref_status_id = dbEntity.ref_status_type.Where(a => a.dname == "SOUL").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname == "Active").id;
                    new_soul.create_by__USER_id = new_user.id;
                    dbEntity.SOULs.Add(new_soul);
                    dbEntity.SaveChanges();
                }
                else
                {
                    long id = n_ext.FirstOrDefault().rel_SOUL_id.HasValue == true ? n_ext.FirstOrDefault().rel_SOUL_id.Value : 0;
                    new_soul = dbEntity.SOULs.Where(b => b.id == id).FirstOrDefault();

                    // check if existing
                    new_soul.name_first = login.first_name;
                    new_soul.name_last = login.last_name;
                    new_soul.email = string.IsNullOrEmpty(login.email) == true ? null : login.email.ToLower();
                    new_soul.dt_create = dt;
                    new_soul.rel_ref_status_id = dbEntity.ref_status_type.Where(a => a.dname == "SOUL").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname == "Active").id;
                    new_soul.create_by__USER_id = new_user.id;
                    dbEntity.Entry(new_soul).State = System.Data.Entity.EntityState.Modified;
                    dbEntity.SaveChanges();
                }

               

                bool i2 = Validation.saveSOUL_ext("social_id", "Social ID", login.social_id, new_soul.id);

                //if (!string.IsNullOrEmpty(insurance_id))
                if (login.insurance_id > 0)
                {
                    bool i = Validation.saveSOUL_ext("insurance_id", "Insurance ID", login.insurance_id.ToString(), new_soul.id);
                }

       
                var p_user = dbEntity.SOULs.Where(b => b.create_by__USER_id == new_user.id);
                //List<user_secondary_patient> pat = new List<user_secondary_patient>();

                user_secondary_patient2 patie = new user_secondary_patient2();
                patie = getPatientUnderPrimaryUser(new_user.id); //get

                #region
                //string zipCode = "", insuranceid = "", insurancename = "";
                //if (p_user.Count() > 0)
                //{
                //    foreach (var i in p_user)
                //    {
                //        var p_ext = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == i.id && a.attr_name == "insurance_id");
                //        var u_user = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == i.id && a.attr_name == "use_user_insurance");

                //        bool user_insurance = false;
                //        string ins_id_string = "", ins_name = "";
                //        long ins_id = 0;
                //        if (u_user.Count() > 0)
                //        {
                //            if (u_user.FirstOrDefault().value == "value")
                //                user_insurance = true;
                //            else
                //                user_insurance = false;
                //        }

                //        if (user_insurance == false && p_ext.Count() > 0)
                //        {
                //            ins_id_string = p_ext.FirstOrDefault().value;
                //            if (p_ext.Count() > 0)
                //            {
                //                // get insurance_id
                //                bool bTemp = long.TryParse(ins_id_string, out ins_id);
                //                if (bTemp)
                //                {
                //                    var ins_ref = dbEntity.ref_insurance_provider.Find(ins_id);
                //                    if (ins_ref != null)
                //                    {
                //                        ins_name = (ins_ref.PayerName.Split('|')[0]).Trim();
                //                    }
                //                    else
                //                    {
                //                        ins_name = "";
                //                    }
                //                }
                //                else
                //                {
                //                    ins_name = "";
                //                }
                //            }
                //        }

                //        //bool bUser = false;
                //        //if (u_user.Count() > 0)
                //        //{
                //        //    bUser = true;
                //        //}
                //        //long ins_id = 0;
                //        //bool bIns_id = long.TryParse(p_ext.FirstOrDefault().value, out ins_id);
                //        //var ins_name = dbEntity.ref_insurance_provider.Find(ins_id);
                //        //string insname = (ins_name.PayerName.Split('|')[0]).Trim();

                //        if (i.is_prime.Value)
                //        {
                //            zipCode = i.ref_zip == null ? "" : i.ref_zip.zip;
                //            insuranceid = ins_id_string;
                //            insurancename = ins_name;
                //        }
                //        else //8-29-2017 removing the PRIMARY patient in the patient's LIST
                //        {
                //            pat.Add(new user_secondary_patient
                //            {
                //                id = i.id,
                //                first_name = i.name_first == null ? "" : i.name_first,
                //                last_name = i.name_last == null ? "" : i.name_last,
                //                is_prime = i.is_prime.Value,
                //                is_using_primary_patient_insurance = user_insurance,
                //                insurance_id = ins_id_string,
                //                insurance_name = ins_name

                //            });
                //        }

                //    }
                //}
                #endregion


                List<get_Login2> logs = new List<get_Login2>();
                //var ref_stat = dbEntity.ref_status_type.Where(a => a.dname == "USER").FirstOrDefault().ref_status; //.ref_status.FirstOrDefault(b => b.dname == "Active");
                //var ref_stat = dbEntity.ref_status.Find(new_user.rel_ref_status_id);
                //var _ext = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == new_user.id);
                var s_social = dbEntity.SOUL_ext.Where(a => a.attr_name == "social_id" && a.value == login.social_id).FirstOrDefault();
                var s_id = s_social.rel_SOUL_id;
                //var _soul = dbEntity.SOULs.Find(s_id);

                var ref_stat = dbEntity.ref_status_type.Where(a => a.dname == "USER").FirstOrDefault();
                var ref_dname = ref_stat.ref_status.FirstOrDefault(b => b.id == new_user.rel_ref_status_id).dname;

                //var u_user = dbEntity.SOULs.Where(a => a.email == u_login.FirstOrDefault().username).FirstOrDefault();
                //
                var u_phone = dbEntity.SOUL_ext.Where(a => a.attr_name == "phone" && a.rel_SOUL_id == s_id).FirstOrDefault();
                var u_city = dbEntity.SOUL_ext.Where(a => a.attr_name == "city" && a.rel_SOUL_id == s_id).FirstOrDefault();
                var u_state = dbEntity.SOUL_ext.Where(a => a.attr_name == "state" && a.rel_SOUL_id == s_id).FirstOrDefault();
                var u_dob = dbEntity.SOUL_ext.Where(a => a.attr_name == "dob" && a.rel_SOUL_id == s_id).FirstOrDefault();
                DateTime dt_DOB = DateTime.Now;
                bool isDoBvalid = false;
                if (u_dob != null)
                {
                    isDoBvalid = DateTime.TryParse(u_dob.value, out dt_DOB);
                    //string n = dt_DOB.ToString("yyyy-MM-dd");
                }

                var _image = dbEntity.USER_ext.Where(a => a.rel_USER_id == new_user.id && a.attr_name == "image").FirstOrDefault();

                var u_height = dbEntity.SOUL_ext.Where(a => a.attr_name == "height" && a.rel_SOUL_id == s_id).FirstOrDefault();
                var u_weight = dbEntity.SOUL_ext.Where(a => a.attr_name == "weight" && a.rel_SOUL_id == s_id).FirstOrDefault();
                var u_emergency = dbEntity.SOUL_ext.Where(a => a.attr_name == "emergency_number" && a.rel_SOUL_id == s_id).FirstOrDefault();
                var u_guardian = dbEntity.SOUL_ext.Where(a => a.attr_name == "parent_guardian" && a.rel_SOUL_id == s_id).FirstOrDefault();
                var u_note = dbEntity.SOUL_ext.Where(a => a.attr_name == "note" && a.rel_SOUL_id == s_id).FirstOrDefault();

                logs.Add(new get_Login2
                {
                    user_id = new_user.id,
                    first_name = new_user.name_first,
                    last_name = new_user.name_last,
                    username = new_user.username == null ? "" : new_user.username,
                    password = string.IsNullOrEmpty(login.password) == true ? "" : login.password,
                    image_url = _image == null ? "" : "https://s3-ap-southeast-1.amazonaws.com/hsrecs/images/" + _image.value,
                    phone_no = new_soul.phone == null ? "" : new_soul.phone,
                    street = new_soul.addr_address1 == null ? "" : new_soul.addr_address1,
                    city = u_city == null ? "" : u_city.value,
                    state = u_state == null ? "" : u_state.value,
                    dob = isDoBvalid == false ? "" : dt_DOB.ToString("yyyy-MM-dd"),
                    parent_guardian = u_guardian == null ? "" : u_guardian.value,
                    gender = new_soul.gender == null ? "" : new_soul.gender.ToUpper(),
                    height = 0,
                    weight = 0,
                    emergency_number = u_emergency == null ? "" : u_emergency.value,
                    note = u_note == null ? "" : u_note.value,
                    //8/23 status = ref_stat == null ? "" : ref_dname,
                    social_id = new_user.social_id,
                    social_type = login.social_type,

                    zip_code = patie.zipcode,
                    insurance_id = patie.insuranceid,
                    insurance_name = patie.insurancename,
                    patient = patie.patient
                });





                var ret1 = JsonConvert.SerializeObject(logs);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                return Json(new { data = json1, message = "Registration successful.", success = true });

            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }


        [System.Web.Http.HttpPost]
        [Route("login")]
        //public async Task<IHttpActionResult> postlogin()
        public IHttpActionResult postlogin([FromBody] param_login param1)
        {

        

            // Required Parameter:-  email, password ,user_type(doctor or patient) Optional Parameter:-device_type,device_token
            // Optional Parameter:-device_type,device_token
            // RETURN: all user details

            //string username = param1.username==null?"": param1.username, 
            //    password = param1.password==null?"": param1.password, 
            //    user_type = "";
            //string acck = "deftsoftapikey", device_type = "", 
            //    device_token = param1.device_token==null?"": param1.device_token;

            //System.Web.HttpContext httpCOn = System.Web.HttpContext.Current;
            //string v = httpCOn.Request.Headers["Authorization"];             

            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);

            DateTime dtNow = DateTime.Now;
            string msg = "",  platform="";

            try
            {

                #region 
                //await Request.Content.ReadAsMultipartAsync(provider);
                //foreach (var key in provider.FormData.AllKeys)
                //{
                //    foreach (var val in provider.FormData.GetValues(key))
                //    {
                //        switch (key)
                //        {
                //            //string email = "", password = "", user_type = "";
                //            //string acck = "deftsoftapikey", device_type = "", device_token = "";
                //            case "username": //email
                //                IsRequired(key, val, 1);
                //                username = val.ToLower();
                //                break;
                //            case "password":
                //                IsRequired(key, val, 1);
                //                password = val;
                //                break;
                //            //case "user_type":
                //            //    IsRequired(key, val, 1);
                //            //    user_type = val;
                //            //    break;
                //            //case "device_type":
                //            //    device_type = val;
                //            //    break;
                //            case "platform":
                //                platform = val;
                //                break;

                //            case "device_token":
                //                device_token = val;
                //                break;

                //            default:
                //                msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                //                return Json(new { data = new string[] { }, message = msg, success = false });
                //        }
                //    }
                //}
                #endregion


                IsRequired("username", param1.username, 2);
                IsRequired("password", param1.password, 2);
                //IsRequired("user_type", user_type, 2);
                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }

                //var u_type = dbEntity.ref_USER_type.Where(a => a.dname.ToLower() == user_type).FirstOrDefault();
                var user_rec = dbEntity.USERs.Where(a => a.username == param1.username.ToLower()); // && a.ref_USER_type.dname.ToLower() == "user");

              
                if (user_rec.Count() == 0)
                {
                    msg = "Invalid credentials."; 
                    return Json(new { data = new string[] { }, message = msg, success = false });
                }
                else
                {
                    if (user_rec.FirstOrDefault().rel_ref_status_id != 7)
                    {
                        msg = "User login is not Active.";
                        return Json(new { data = new string[] { }, message = msg, success = false });
                    }

                }


                string encryp;
                if (user_rec.Count() > 0)
                {
                    string salt = user_rec.FirstOrDefault().tlas;
                    encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(param1.password + salt));
                    var u_rec = user_rec.Where(a => a.phash == encryp).FirstOrDefault();


                    if (u_rec != null)
                    {
                        // redirect if [USER] is [DOCTOR]
                        if (user_rec.FirstOrDefault().ref_USER_type.dname.ToLower() == "doctor")
                        {
                            return doctorLogin(user_rec);
                        }


                        var u_ext = dbEntity.USER_ext.Where(a => a.rel_USER_id == u_rec.id && a.attr_name == "verified");
                        if (u_ext.Count() > 0)
                        {
                            if (u_ext.FirstOrDefault().value == "false")
                            {
                                return Json(new { data = new string[] { }, message = "Unable to login. Please verify your account.", success = false });
                            }
                        }

                        Random random = new Random();
                        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                        string veri_code = new string(Enumerable.Repeat(chars, 20)
                          .Select(s => s[random.Next(s.Length)]).ToArray());
                        //---
                        Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                        string login_token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(veri_code));
                        //---
                        // Save the token of login
                        u_rec.token_current = login_token;
                        u_rec.url_lastin =HttpContext.Current.Request.Url.ToString();
                        u_rec.dt_lastin = dt;
                        dbEntity.Entry(u_rec).State = System.Data.Entity.EntityState.Modified;
                        dbEntity.SaveChanges();

                        List<get_Login> logs = new List<get_Login>();
                        var ref_stat = dbEntity.ref_status_type.Where(a => a.dname == "USER").FirstOrDefault();
                        var ref_dname = ref_stat.ref_status.FirstOrDefault(b => b.id == u_rec.rel_ref_status_id).dname;
                        //var ref =    dbEntity.ref_status_type.Where(a => a.dname == "USER").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname.ToLower() == status.ToLower()).id;

                        // get the primary user's info
                        var prim_user = dbEntity.SOULs.Where(b => b.email == u_rec.username).FirstOrDefault();

                        // get the patient's info that is added by the primary user
                        user_secondary_patient2 patie = new user_secondary_patient2();
                        patie = getPatientUnderPrimaryUser(u_rec.id); //get

                        #region 
                        //var p_user = dbEntity.SOULs.Where(b => (b.create_by__USER_id == u_rec.id || b.email == u_rec.username) && b.rel_ref_status_id != 3);

                        //List<user_secondary_patient> pat = new List<user_secondary_patient>();
                        //string zipcode = "", insuranceid = "", insurancename = "";

                        //if (p_user.Count() > 0)
                        //{
                        //    foreach (var n in p_user)
                        //    {
                        //        // is_using_primary_patient_insurance
                        //        var u_user = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == n.id && a.attr_name == "use_user_insurance");

                        //        // get insurance name 
                        //        var p_ext = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == n.id && a.attr_name == "insurance_id");
                        //        bool user_insurance = false;
                        //        string ins_id_string = "", ins_name = "";
                        //        long ins_id = 0;
                        //        if (u_user.Count() > 0)
                        //        {
                        //            if (u_user.FirstOrDefault().value == "true")
                        //                user_insurance = true;
                        //            else
                        //                user_insurance = false;
                        //        }

                        //        if (p_ext.Count() > 0) //user_insurance == false &&
                        //        {
                        //            ins_id_string = p_ext.FirstOrDefault().value;
                        //            if (p_ext.Count() > 0)
                        //            {
                        //                // get insurance_id
                        //                bool btemp = long.TryParse(ins_id_string, out ins_id);
                        //                if (btemp)
                        //                {
                        //                    var ins_ref = dbEntity.ref_insurance_provider.Find(ins_id);
                        //                    if (ins_ref != null)
                        //                    {
                        //                        ins_name = (ins_ref.PayerName.Split('|')[0]).Trim();
                        //                    }
                        //                    else
                        //                    {
                        //                        ins_name = "";
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    ins_name = "";
                        //                }
                        //            }
                        //            else
                        //            {
                        //                //if (u_user.Count() > 0)
                        //                //{
                        //                //    user_insurance = true;
                        //                //}
                        //            }
                        //        }

                        //        if (n.is_prime.Value)
                        //        {
                        //            zipcode = n.ref_zip == null ? "" : n.ref_zip.zip;
                        //            insuranceid = ins_id_string;
                        //            insurancename = ins_name;
                        //        }
                        //        else
                        //        {
                        //            pat.Add(new user_secondary_patient
                        //            {
                        //                id = n.id,
                        //                first_name = n.name_first == null ? "" : n.name_first,
                        //                last_name = n.name_last == null ? "" : n.name_last,
                        //                is_prime = n.is_prime.Value,
                        //                //insurance_id =  p_ext.Count() == 0 ? "" : p_ext.ToList().First().value.ToString(),
                        //                is_using_primary_patient_insurance = user_insurance,
                        //                insurance_id = ins_id_string,
                        //                insurance_name = ins_name
                        //            });
                        //        }

                        //    }
                        //}
                        // end of patients info
                        #endregion



                        // GET the value if user is VERIFIED
                        var user_veri = dbEntity.USER_ext.Where(a => a.rel_USER_id == user_rec.FirstOrDefault().id && a.attr_name == "verified").FirstOrDefault();
                        var user_veri_code = dbEntity.USER_ext.Where(a => a.rel_USER_id == user_rec.FirstOrDefault().id && a.attr_name == "verification_code").FirstOrDefault();
                        var _image = dbEntity.USER_ext.Where(a => a.rel_USER_id == user_rec.FirstOrDefault().id && a.attr_name == "image").FirstOrDefault();
                        //var isVerified = user_ext.value;
                        bool isVerified = false;
                        if (user_veri.value == "true")
                        { isVerified = true; }

                        //var u_phone = dbEntity.SOUL_ext.Where(a => a.attr_name == "phone" && a.rel_SOUL_id == prim_user.id).FirstOrDefault();
                        var u_city = dbEntity.SOUL_ext.Where(a => a.attr_name == "city" && a.rel_SOUL_id == prim_user.id).FirstOrDefault();
                        var u_state = dbEntity.SOUL_ext.Where(a => a.attr_name == "state" && a.rel_SOUL_id == prim_user.id).FirstOrDefault();
                        var u_dob = dbEntity.SOUL_ext.Where(a => a.attr_name == "dob" && a.rel_SOUL_id == prim_user.id).FirstOrDefault();
                        DateTime dt_DOB = DateTime.Now;
                        bool isDoBvalid = false;
                        if (u_dob != null)
                        {
                            isDoBvalid = DateTime.TryParse(u_dob.value, out dt_DOB);
                            //string n = dt_D  OB.ToString("yyyy-MM-dd");
                        }

                        var u_height = dbEntity.SOUL_ext.Where(a => a.attr_name == "height" && a.rel_SOUL_id == prim_user.id).FirstOrDefault();
                        var u_weight = dbEntity.SOUL_ext.Where(a => a.attr_name == "weight" && a.rel_SOUL_id == prim_user.id).FirstOrDefault();
                        var u_emergency = dbEntity.SOUL_ext.Where(a => a.attr_name == "emergency_number" && a.rel_SOUL_id == prim_user.id).FirstOrDefault();
                        var u_guardian = dbEntity.SOUL_ext.Where(a => a.attr_name == "parent_guardian" && a.rel_SOUL_id == prim_user.id).FirstOrDefault();
                        var u_soc_type = dbEntity.SOUL_ext.Where(a => a.attr_name == "social_type" && a.rel_SOUL_id == prim_user.id).FirstOrDefault();
                        var u_note = dbEntity.SOUL_ext.Where(a => a.attr_name == "note" && a.rel_SOUL_id == prim_user.id).FirstOrDefault();

                        var u_type = u_rec.ref_USER_type.dname;


                        logs.Add(new get_Login
                        {
                            user_id = u_rec.id,
                            first_name = u_rec.name_first == null ? "" : u_rec.name_first,
                            last_name = u_rec.name_last == null ? "" : u_rec.name_last,
                            social_id = u_rec.social_id == null ? "" : u_rec.social_id,
                            social_type = u_soc_type ==null?"" : u_soc_type.value,
                            username = u_rec.username == null ? "" : u_rec.username,
                            password = u_rec.phash == null ? "" : param1.password,
                            image_url = _image == null ? "" : "https://s3-ap-southeast-1.amazonaws.com/hsrecs/images/" + _image.value,
                            verified = isVerified,
                            //8/23 verification_code = user_veri_code.value,
                            //9/5 token = login_token,
                            phone_no = prim_user.phone == null ? "" : prim_user.phone,
                            street = prim_user.addr_address1 == null ? "" : prim_user.addr_address1,
                            city = u_city == null ? "" : u_city.value,
                            state = u_state == null ? "" : u_state.value,
                            
                            dob = isDoBvalid == false ? "" : dt_DOB.ToString("yyyy-MM-dd"),
                            parent_guardian = u_guardian == null ? "" : u_guardian.value,
                            gender = prim_user.gender == null ? "" : prim_user.gender,
                            height = u_height == null ? 0 : Convert.ToDouble(u_height.value),
                            weight = u_weight == null ? 0 : Convert.ToDouble(u_weight.value),
                            emergency_number = u_emergency == null ? "" : u_emergency.value,
                            note = u_note == null ? "" : u_note.value,
                            //status = ref_stat == null ? "" : ref_dname,
                            user_type = u_type,
                            zip_code = patie.zipcode,
                            insurance_id = patie.insuranceid,
                            insurance_name = patie.insurancename,
                            patient = patie.patient
                        });

                        var ret1 = JsonConvert.SerializeObject(logs);
                        //[{"id":10,"firstname":"marding","lastname":"nizari","username":"mardingnizari","status":"Active"}]

                        //var ret1 = "[{\"id\":10,\"firstname\":\"marding\",\"lastname\":\"nizari\",\"username\":\"mardingnizari\",\"status\":\"Active\"}]"
                        var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                        msg = "Successful login.";
                        return Json(new { data = json1, message = msg, success = true });
                    }
                    else
                    {
                        //msg = "Invalid password.";
                        msg = "Invalid credentials.";
                        return Json(new { data = new string[] { }, message = msg, success = false });
                    }

                }
                else
                {
                    //var ret1 = JsonConvert.SerializeObject(logs);
                    //var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                    msg = "No result found.";
                    return Json(new { data = new string[] { }, message = msg, success = false });
                }

            }
            catch (Exception ex)
            {
                msg = "The authorization header is either not valid or isn't Basic.";
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }



            //if (acck == "deftsoftapikey")
            //{

            //    try {

            //    }
            //    catch(Exception ex) {
            //        return Json(new { data = "", message = ex.Message, success = false });
            //    }
            //}
            // msg = "The authorization header is either not valid or isn't Basic.";
            //return Json(new { data = "", message = msg, success = false });
        }

        private user_secondary_patient2 getPatientUnderPrimaryUser(long user_id, string username = "")
        {
            // get username
            var user = dbEntity.USERs.Find(user_id);
            if (user != null)
            {
                username = user.username;
            }

            var p_user = dbEntity.SOULs.Where(b => (b.create_by__USER_id == user_id || b.email == username) && b.rel_ref_status_id != 3);

            List<user_secondary_patient> pat = new List<user_secondary_patient>();
            string zipcode = "", insuranceid = "", insurancename = "";

            if (p_user.Count() > 0)
            {
                foreach (var n in p_user)
                {
                    // is_using_primary_patient_insurance
                    var u_user = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == n.id && a.attr_name == "use_user_insurance");

                    // get insurance name 
                    var p_ext = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == n.id && a.attr_name == "insurance_id");
                    bool user_insurance = false;
                    string ins_id_string = "", ins_name = "";
                    long ins_id = 0;
                    if (u_user.Count() > 0)
                    {
                        if (u_user.FirstOrDefault().value == "true")
                            user_insurance = true;
                        else
                            user_insurance = false;
                    }

                    if (p_ext.Count() > 0) //user_insurance == false &&
                    {
                        ins_id_string = p_ext.FirstOrDefault().value;
                        if (p_ext.Count() > 0)
                        {
                            // get insurance_id
                            bool btemp = long.TryParse(ins_id_string, out ins_id);
                            if (btemp)
                            {
                                var ins_ref = dbEntity.ref_insurance_provider.Find(ins_id);
                                if (ins_ref != null)
                                {
                                    ins_name = (ins_ref.PayerName.Split('|')[0]).Trim();
                                }
                                else
                                {
                                    ins_name = "";
                                }
                            }
                            else
                            {
                                ins_name = "";
                            }
                        }
                        else
                        {
                            //if (u_user.Count() > 0)
                            //{
                            //    user_insurance = true;
                            //}
                        }
                    }

                    if (n.is_prime.Value)
                    {
                        zipcode = n.ref_zip == null ? "" : n.ref_zip.zip;
                        insuranceid = ins_id_string;
                        insurancename = ins_name;
                    }
                    else
                    {
                        pat.Add(new user_secondary_patient
                        {
                            id = n.id,
                            first_name = n.name_first == null ? "" : n.name_first,
                            last_name = n.name_last == null ? "" : n.name_last,
                            is_prime = n.is_prime.Value,
                            //insurance_id =  p_ext.Count() == 0 ? "" : p_ext.ToList().First().value.ToString(),
                            is_using_primary_patient_insurance = user_insurance,
                            insurance_id = ins_id_string,
                            insurance_name = ins_name
                        });
                    }

                }
            }

            user_secondary_patient2 pati = new user_secondary_patient2();
            pati.patient = pat;
            pati.insuranceid = insuranceid;
                pati.insurancename = insurancename;
                pati.zipcode = zipcode;
           

            return pati;
        }

        private IHttpActionResult doctorLogin(IQueryable<USER> param1)
        {            //if (doc_user.Count() > 0)

            var doc_user = dbEntity.hs_DOCTOR.Where(a => a.email == param1.FirstOrDefault().username);
            foreach (var n in doc_user)
            {
                var n1 = n.NPI;
            }

            //{
            //   string encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(param1.password + doc_user.FirstOrDefault().tlas));
            //   var u_rec = doc_user.Where(a => a.phash == encryp).FirstOrDefault();
            // }
        
            List<doctor_login_response> doc = new List<doctor_login_response>();

            //if (param1 != null)
            // {
            doc.Add(new doctor_login_response {
                npi = doc_user.FirstOrDefault().NPI == null?"": doc_user.FirstOrDefault().NPI,
                    user_type = param1.FirstOrDefault().ref_USER_type.dname == null?"" : param1.FirstOrDefault().ref_USER_type.dname
            });
            //}

            var ret1 = JsonConvert.SerializeObject(doc);
            var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

            return Json(new { data = json1, message = "Doctor login.", success = true });
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IHttpActionResult> postlogout(){

            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);

            string msg = "", user_id ="", device_token ="";

            try {
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {

                        switch (key)
                        {
                            //string email = "", password = "", user_type = "";
                            //string acck = "deftsoftapikey", device_type = "", device_token = "";
                            case "user_id":
                                IsRequired(key, val, 1);
                                user_id = val;
                                break;
                            //case "device_token":
                            //    IsRequired(key, val, 1);
                            //    device_token = val;
                            //    break;
                          

                            default:
                                msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                                return Json(new { data = new string[] { }, message = msg, success = false });
                        }
                    }
                }

                IsRequired("user_id", user_id, 2);
                //IsRequired("device_token", device_token, 2);
                //IsRequired("user_type", user_type, 2);
                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }

                long user_id_new = 0;
                bool isUser = long.TryParse(user_id, out user_id_new);
                if (isUser) {
                    var u_rec = dbEntity.USERs.Find(user_id_new);
                    if (u_rec != null)
                    {
                        //u_rec = u_rec.Where(a => a.token_current == device_token);
                        //if (u_rec.Count() == 0)
                        //{
                            u_rec.token_current = null;
                            dbEntity.Entry(u_rec).State = System.Data.Entity.EntityState.Modified;
                            dbEntity.SaveChanges();

                            // successful logout
                            msg = "Logout successfully.";
                            return Json(new { data = new string[] { }, message = msg, success = true });
                        //}

                    }
                    else
                    {
                        //invalid username
                        msg = "Invalid user_id value.";
                        return Json(new { data = new string[] { }, message = msg, success = false });
                    }
                }
                msg = "Invalid username.";
                return Json(new { data = new string[] { }, message = msg, success = false });

            }
            catch (Exception ex) {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }


        DateTime date = DateTime.Today;
        //bool haserror = false;
        //string errmsg = "", infomsg = "";
        HSAuth authorized = new HSAuth();
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("forgotpassword")]
        public IHttpActionResult getforgotpassword(string email)
        {
            //var isauthorized = authorized.HSRequest(Request);
            //if (isauthorized.StatusCode == HttpStatusCode.Forbidden)
            //{
            //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized)
            //    {
            //        StatusCode = HttpStatusCode.Unauthorized,
            //    });
            //}

            IsRequired("email", email, 1);

            if (haserror)
            {
                return Json(new { message = errmsg, success = false });
            }
            else
            {
                string msg = "";
                try
                {
                    var _user = (from u in dbEntity.USERs
                                 where (u.username.ToLower() == email.ToLower())
                                 select new
                                 {
                                     user = new new_USER
                                     {
                                         user_id = u.id,
                                         name = u.name_first + " " + u.name_last,
                                         username = u.username,
                                         status = (u.rel_ref_status_id == 7) ? true : false
                                     }
                                 });

                    if (_user.Count() > 0)
                    {

                        #region  
                        //System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage
                        //{
                        //    //From = new System.Net.Mail.MailAddress("do-not-reply@healthsplash.com", "It says DO NOT REPLY")
                        //    From = new System.Net.Mail.MailAddress("do-not-reply@healthsplash.com", "It says DO NOT REPLY")
                        //};

                        //System.Net.Mail.MailAddress from = new System.Net.Mail.MailAddress("staff@nationalcenterforpain.com");
                        //System.Net.Mail.MailAddress to = new System.Net.Mail.MailAddress("neilsumanda@gmail.com");

                        //System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(from,to);

                        //message.IsBodyHtml = false;
                        //message.To.Add("neilsumanda@gmail.com");
                        //message.Subject = "my subject";
                        //message.Body = "bodys dfgsdgfdg";
                        //message.Priority = System.Net.Mail.MailPriority.Normal;
                        //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                        //{
                        //    //UseDefaultCredentials = false,
                        //    Port = 587, //465,
                        //    //Host = "mail.healthsplash.com", 
                        //    Host = "box995.bluehost.com",
                        //    EnableSsl = true,
                        //    //Credentials = new NetworkCredential("do-not-reply@healthsplash.com", "eJw6V^VIYD"),
                        //    Credentials = new NetworkCredential("staff@nationalcenterforpain.com", "Staff1@#$%"),
                        //    Timeout = 10000
                        //    //UseDefaultCredentials = false
                        //};
                        //client.Send(message);
                        #endregion

                        var u_ext_veri = dbEntity.USER_ext.Where(a => a.attr_name== "verification_code" && a.rel_USER_id == _user.FirstOrDefault().user.user_id).FirstOrDefault();
                        
                        System.Net.Mail.MailAddress from = new System.Net.Mail.MailAddress("do-not-reply@healthsplash.com");
                        System.Net.Mail.MailAddress to = new System.Net.Mail.MailAddress(_user.FirstOrDefault().user.username);

                        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(from, to);

                        message.IsBodyHtml = true;
                        message.Subject = "HealthSplash Verification";
                        //message.Body = "<br><br>Click " + "<a href=http://www.healthsplash.com/activate-email?email=" + to + "&code="  + u_ext_veri.value + ">" + "here" + "</a>, to verify.";
                        //message.Body = "<br><br>Click " + "<a href=http://dev.healthsplash.com/patient/verifyemail?email=" + to + "&code=" + u_ext_veri.value + ">" + "here" + "</a>, to verify.";

                        string str = "<!doctype html><html><head><meta charset =\"utf-8\"><title> Health Splash - Forgot Password </title></head>";
                        str += "<body><table style =\"width:600px; margin:0 auto;\" border =\"0\" cellpadding =\"0\" cellspacing =\"0\">";
                        str += "<tr><td style =\"text-align:center; padding:25px 0;\"><img src=\"http://healthsplash.com/Content/images/logo.png\" alt=\"Health Splash\" /></td></tr>";
                        str += "<tr><td style =\"border:1px solid #eee; background:#fff; padding:30px 60px\">";
                        str += "<table width=\"100%\" border =\"0\" cellspacing =\"0\" cellpadding =\"0\">";

                        //str += "<tr><td style =\"text-align:center;\"><img src =\"http://dev.healthsplash.com/Content/images/icon-lock.png\" alt =\"Newsletter\"/></td></tr>";
                        str += "<tr><td style =\"text-align:center;\">Greetings from HealthSplash! Here is the password reset you requested.</td></tr>";

                        //string str2 = "Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat";
                        string str2 = "If you have a HealthSplash account but did not request a password reset, please verify your account's security by logging in and requesting a new password link. If you do not have a HealthSplash account, then please ignore/delete this email.";
                        string str3 = "";

                        str += "<tr><td style =\"padding:25px 0; font-size:22px; color:#666; text-align:center; font-family: Arial, 'Helvetica Neue', Helvetica, sans-serif; border-bottom:1px solid #eee\" > Forgot your password</td></tr>";
                        str += "<tr><td style =\"padding:25px 0 0; font-size:16px; color:#666; text-align:center; line-height:30px; font-family: Arial, 'Helvetica Neue', Helvetica, sans-serif;\">" + str2 + "</td></tr>";
                        str += "<tr><td style=\"padding:25px 0; border-bottom:1px solid #eee\"><a href=\"http://dev.healthsplash.com/patient/verifyemail?email=" + to + "&code=" + u_ext_veri.value + "\" style =\"background: #ee6c7a; display:block; text-align:center; text-decoration:none;border-radius: 5px;color: #ffffff;font-size: 18px;font-weight: 500;height: 53px;line-height: 2.9;margin: 0;padding: 0 50px;font-family: Arial, 'Helvetica Neue', Helvetica, sans-serif;\">Reset Password</a></td></tr>";
                        str += "<tr><td style =\"padding:25px 0 0; font-size:15px; color:#999; font-style:italic; text-align:center; font-family: Arial, 'Helvetica Neue', Helvetica, sans-serif; line-height:24px;\">" + str3 + "</td></tr>";
                        str += "</table></td></tr>";
                        str += "</table>";
                        str += "</body>";
                        str += "</html>";
                        message.Body = str;

                        message.Priority = System.Net.Mail.MailPriority.Normal;

                        System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                        {
                            //UseDefaultCredentials = false,
                            Port = 587, //465,
                            Host = "box995.bluehost.com",
                            EnableSsl = true,
                            Credentials = new NetworkCredential("staff@nationalcenterforpain.com", "Staff1@#$%"),
                            Timeout = 10000
                        };

                        //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                        //{
                        //    //UseDefaultCredentials = false,
                        //    Port = 26, //465,
                        //    Host = "mail.healthsplash.net",
                        //    EnableSsl = true,
                        //    Credentials = new NetworkCredential("do-not-reply@healthsplash.net", "9_+Go=UWj_"),
                        //    Timeout = 10000
                        //};

                        client.Send(message);

                        var ret1 = JsonConvert.SerializeObject(_user);
                        var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                        // msg = "Mail sent."
                        msg = "A link has been sent to your registered email address. Please use the link to generate new password.";
                        return Json(new { data = json1, message= msg, success = true });
                    }
                    else
                    {
                        // msg = "No result found.";
                        msg = "This email doesn't exist.";
                        return Json(new { data = new string[] { }, message = msg, success = false });
                    }
                }
                catch (ArgumentNullException ex) {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
                catch (ObjectDisposedException ex)
                {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
                catch (System.Net.Mail.SmtpException ex)
                {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
                catch (Exception ex)
                {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("changepassword")]
        public IHttpActionResult postchangepassword(string email)
        {
            //var isauthorized = authorized.HSRequest(Request);
            //if (isauthorized.StatusCode == HttpStatusCode.Forbidden)
            //{
            //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized)
            //    {
            //        StatusCode = HttpStatusCode.Unauthorized,
            //    });
            //}

            IsRequired("email", email, 1);

            if (haserror)
            {
                return Json(new { message = errmsg, success = false });
            }
            else
            {
                string msg = "";
                try
                {
                    var _user = (from u in dbEntity.USERs
                                 where (u.username.ToLower() == email.ToLower())
                                 select new
                                 {
                                     user = new new_USER
                                     {
                                         user_id = u.id,
                                         name = u.name_first + " " + u.name_last,
                                         username = u.username,
                                         status = (u.rel_ref_status_id == 7) ? true : false
                                     }
                                 });

                    if (_user.Count() > 0)
                    {

                        //System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage
                        //{
                        //    //From = new System.Net.Mail.MailAddress("do-not-reply@healthsplash.com", "It says DO NOT REPLY")
                        //    From = new System.Net.Mail.MailAddress("do-not-reply@healthsplash.com", "It says DO NOT REPLY")
                        //};

                        //System.Net.Mail.MailAddress from = new System.Net.Mail.MailAddress("staff@nationalcenterforpain.com");
                        //System.Net.Mail.MailAddress to = new System.Net.Mail.MailAddress("neilsumanda@gmail.com");

                        //System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(from,to);

                        //message.IsBodyHtml = false;
                        //message.To.Add("neilsumanda@gmail.com");
                        //message.Subject = "my subject";
                        //message.Body = "bodys dfgsdgfdg";
                        //message.Priority = System.Net.Mail.MailPriority.Normal;
                        //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                        //{
                        //    //UseDefaultCredentials = false,
                        //    Port = 587, //465,
                        //    //Host = "mail.healthsplash.com", 
                        //    Host = "box995.bluehost.com",
                        //    EnableSsl = true,
                        //    //Credentials = new NetworkCredential("do-not-reply@healthsplash.com", "eJw6V^VIYD"),
                        //    Credentials = new NetworkCredential("staff@nationalcenterforpain.com", "Staff1@#$%"),
                        //    Timeout = 10000
                        //    //UseDefaultCredentials = false
                        //};
                        //client.Send(message);

                        var u_ext_veri = dbEntity.USER_ext.Where(a => a.attr_name == "verification_code" && a.rel_USER_id == _user.FirstOrDefault().user.user_id).FirstOrDefault();

                        System.Net.Mail.MailAddress from = new System.Net.Mail.MailAddress("do-not-reply@healthsplash.com");
                        System.Net.Mail.MailAddress to = new System.Net.Mail.MailAddress(_user.FirstOrDefault().user.username);

                        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(from, to);

                        message.IsBodyHtml = true;
                        message.Subject = "HealthSplash Verification";
                        //message.Body = "<br><br>Click " + "<a href=http://www.healthsplash.com/activate-email?email=" + to + "&code=" + u_ext_veri.value + ">" + "here" + "</a>, to verify.";
                        message.Priority = System.Net.Mail.MailPriority.Normal;

                        string str1 = "";

                        string str = "<!doctype html><html><head><meta charset=\"utf-8\"><title> Health Splash - Forgot Password </title></head>";
                        str += "<body><table style=\"width:600px; margin:0 auto;\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">";
                        str += "<tr><td style=\"text-align:center; padding:25px 0;\"><img src=\"http://healthsplash.com/Content/images/logo.png\" alt=\"Health Splash\"/></td></tr>";
                        str += "<tr><td style=\"border:1px solid #eee; background:#fff; padding:30px 60px\">";
                        str += "<table width=\"100%\" border =\"0\" cellspacing =\"0\" cellpadding =\"0\">";
                        str += "<tr><td style=\"text-align:center;\"><img src=\"http://dev.healthsplash.com/Content/images/icon-lock.png\" alt=\"Newsletter\"/></td></tr>";
                        str += "<tr><td style=\"padding:25px 0; font-size:22px; color:#666; text-align:center; font-family: Arial, 'Helvetica Neue', Helvetica, sans-serif; border-bottom:1px solid #eee\"> Forgot your password</td></tr>";
                        str += "<tr><td style=\"padding:25px 0 0; font-size:16px; color:#666; text-align:center; line-height:30px; font-family: Arial, 'Helvetica Neue', Helvetica, sans-serif;\">Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat </td></tr>";
                        str += "<tr><td style=\"padding:25px 0; border-bottom:1px solid #eee\"><a href=\"http://www.healthsplash.com/activate-email?email=" + to + "&code=" + u_ext_veri.value + "\" style =\"background: #ee6c7a; display:block; text-align:center; text-decoration:none;border-radius: 5px;color: #ffffff;font-size: 18px;font-weight: 500;height: 53px;line-height: 2.9;margin: 0;padding: 0 50px;font-family: Arial, 'Helvetica Neue', Helvetica, sans-serif;\">Reset Password </a></td></tr>";
                        str += "<tr><td style=\"padding:25px 0 0; font-size:15px; color:#999; font-style:italic; text-align:center; font-family: Arial, 'Helvetica Neue', Helvetica, sans-serif; line-height:24px;\">" + str1 + "</td></tr>";
                        str += "</table></td></tr>";
                        str += "</table>";
                        str += "</body>";
                        str += "</html>";
                        message.Body = str;

                        System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                        {
                            //UseDefaultCredentials = false,
                            Port = 587, //465,
                            Host = "box995.bluehost.com",
                            EnableSsl = true,
                            Credentials = new NetworkCredential("staff@nationalcenterforpain.com", "Staff1@#$%"),
                            Timeout = 10000
                        };

                        //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                        //{
                        //    //UseDefaultCredentials = false,
                        //    Port = 26, //465,
                        //    Host = "mail.healthsplash.net",
                        //    EnableSsl = true,
                        //    Credentials = new NetworkCredential("do-not-reply@healthsplash.net", "9_+Go=UWj_"),
                        //    Timeout = 10000
                        //};

                        client.Send(message);

                        var ret1 = JsonConvert.SerializeObject(_user);
                        var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                        // msg = "Mail sent."
                        msg = "A link has been sent to your registered email address. Please use the link to generate new password.";
                        return Json(new { data = json1, message = msg, success = true });
                    }
                    else
                    {
                        // msg = "No result found.";
                        msg = "This email doesn't exist.";
                        return Json(new { data = new string[] { }, message = msg, success = false });
                    }
                }
                catch (ArgumentNullException ex)
                {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
                catch (ObjectDisposedException ex)
                {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
                catch (System.Net.Mail.SmtpException ex)
                {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
                catch (Exception ex)
                {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("user/email")]
        public IHttpActionResult postuseremail(string email="", string user_id ="")
        {
            //var isauthorized = authorized.HSRequest(Request);
            //if (isauthorized.StatusCode == HttpStatusCode.Forbidden)
            //{
            //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized)
            //    {
            //        StatusCode = HttpStatusCode.Unauthorized,
            //    });
            //}

            IsRequired("email", email, 1);
            IsRequired("user_id", user_id, 1);
            if (haserror)
            {
                return Json(new { message = errmsg, success = false });
            }
            else
            {
                string msg = "";
                try
                {
                    //var _user = (from u in dbEntity.USERs
                    //             where (u.username.ToLower() == email.ToLower())
                    //             select new
                    //             {
                    //                 user = new new_USER
                    //                 {
                    //                     user_id = u.id,
                    //                     name = u.name_first + " " + u.name_last,
                    //                     username = u.username,
                    //                     status = (u.rel_ref_status_id == 7) ? true : false
                    //                 }
                    //             });
                    long user_id_new = 0;
                    bool isValid_user = long.TryParse(user_id, out user_id_new);

                    var _user = from u in dbEntity.USERs
                                where (u.username.ToLower() == email.ToLower() && (u.id == user_id_new))
                                select u;

                    // So we will send you the user_id and email_address and you guys just need to verify that email adress 
                    // by sending verfication link over there.

                    if (_user.Count() > 0)
                    {
                        // var u_ext_veri = dbEntity.USER_ext.Where(a => a.attr_name == "verification_code" && a.rel_USER_id == _user.FirstOrDefault().user.user_id).FirstOrDefault();
                        // generate verification code
                        Random random = new Random();
                        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                        string veri_code = new string(Enumerable.Repeat(chars, 4)
                          .Select(s => s[random.Next(s.Length)]).ToArray());

                        //USER_ext u_ext = new USER_ext();
                        //u_ext.rel_USER_id = user_id_new;
                        //u_ext.attr_name = "email_verification_code";
                        //u_ext.dname = "Email Verification Code";
                        //u_ext.rel_ref_datatype_id = 2;
                        //u_ext.value = veri_code;
                        //u_ext.create_by__USER_id = user_id_new; // user_id;
                        //u_ext.dt_create = dt;
                        //dbEntity.USER_ext.Add(u_ext);
                        _user.FirstOrDefault().verified = false;
                        _user.FirstOrDefault().verification_code = veri_code;
                        _user.FirstOrDefault().dt_update = dt;
                        dbEntity.Entry(_user.FirstOrDefault()).State = System.Data.Entity.EntityState.Modified;
                        dbEntity.SaveChanges();


                        System.Net.Mail.MailAddress from = new System.Net.Mail.MailAddress("do-not-reply@healthsplash.com");
                        System.Net.Mail.MailAddress to = new System.Net.Mail.MailAddress(_user.FirstOrDefault().username);

                        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(from, to);

                        message.IsBodyHtml = true;
                        message.Subject = "HealthSplash Change Email";
                        message.Body = "<br><br>Click " + "<a href=http://www.healthsplash.com/change-email?email=" + to + "&code=" + veri_code + ">" + "here" + "</a>, to verify.";
                        message.Priority = System.Net.Mail.MailPriority.Normal;
                        System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                        {
                            //UseDefaultCredentials = false,
                            Port = 587, //465,
                            Host = "box995.bluehost.com",
                            EnableSsl = true,
                            Credentials = new NetworkCredential("staff@nationalcenterforpain.com", "Staff1@#$%"),
                            Timeout = 10000
                        };
                        client.Send(message);

                        var ret1 = JsonConvert.SerializeObject(_user);
                        var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                        // msg = "Mail sent."
                        msg = "A link has been sent to your registered email address. Please use the link to generate new password.";
                        return Json(new { data = json1, message = msg, success = true });
                    }
                    else
                    {
                        // msg = "No result found.";
                        msg = "This email doesn't exist.";
                        return Json(new { data = new string[] { }, message = msg, success = false });
                    }
                }
                catch (ArgumentNullException ex)
                {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
                catch (ObjectDisposedException ex)
                {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
                catch (System.Net.Mail.SmtpException ex)
                {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
                catch (Exception ex)
                {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
            }
        }

        //[System.Web.Http.HttpPut]
        //[System.Web.Http.Route("user/email")]
        //public IHttpActionResult putuseremail(string verification_code = "", string email="")
        //{
 
        //    //IsRequired("email", email, 1);
        //    //IsRequired("user_id", user_id, 1);
        //    //if (haserror)
        //    //{
        //    //    return Json(new { message = errmsg, success = false });
        //    //}
            
        //        string msg = "";
        //        try
        //        {
           
        //            //long user_id_new = 0;
        //            //bool isValid_user = long.TryParse(user_id, out user_id_new);

        //            var _user = from u in dbEntity.USERs
        //                        where (u.username.ToLower() == email.ToLower())
        //                        select u;

        //            var user_ext 

        //            // So we will send you the user_id and email_address and you guys just need to verify that email adress 
        //            // by sending verficatiion link over there.

        //            if (_user.Count() > 0)
        //            {
        //                // var u_ext_veri = dbEntity.USER_ext.Where(a => a.attr_name == "verification_code" && a.rel_USER_id == _user.FirstOrDefault().user.user_id).FirstOrDefault();
        //                // generate verification code
        //                Random random = new Random();
        //                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        //                string veri_code = new string(Enumerable.Repeat(chars, 4)
        //                  .Select(s => s[random.Next(s.Length)]).ToArray());

        //                USER_ext u_ext = new USER_ext();
        //                u_ext.rel_USER_id = user_id_new;
        //                u_ext.attr_name = "email_verification_code";
        //                u_ext.dname = "Email Verification Code";
        //                u_ext.rel_ref_datatype_id = 2;
        //                u_ext.value = veri_code;
        //                u_ext.create_by__USER_id = user_id_new; // user_id;
        //                u_ext.dt_create = dt;
        //                dbEntity.USER_ext.Add(u_ext);
        //                dbEntity.SaveChanges();


        //                System.Net.Mail.MailAddress from = new System.Net.Mail.MailAddress("do-not-reply@healthsplash.com");
        //                System.Net.Mail.MailAddress to = new System.Net.Mail.MailAddress(_user.FirstOrDefault().username);

        //                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(from, to);

        //                message.IsBodyHtml = true;
        //                message.Subject = "HealthSplash Change Email";
        //                message.Body = "<br><br>Click " + "<a href=http://www.healthsplash.com/activate-email?email=" + to + "&code=" + veri_code + ">" + "here" + "</a>, to verify.";
        //                message.Priority = System.Net.Mail.MailPriority.Normal;
        //                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
        //                {
        //                    //UseDefaultCredentials = false,
        //                    Port = 587, //465,
        //                    Host = "box995.bluehost.com",
        //                    EnableSsl = true,
        //                    Credentials = new NetworkCredential("staff@nationalcenterforpain.com", "Staff1@#$%"),
        //                    Timeout = 10000
        //                };
        //                client.Send(message);

        //                var ret1 = JsonConvert.SerializeObject(_user);
        //                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
        //                // msg = "Mail sent."
        //                msg = "A link has been sent to your registered email address. Please use the link to generate new password.";
        //                return Json(new { data = json1, message = msg, success = true });
        //            }
        //            else
        //            {
        //                // msg = "No result found.";
        //                msg = "This email doesn't exist.";
        //                return Json(new { data = new string[] { }, message = msg, success = false });
        //            }
        //        }
        //        catch (ArgumentNullException ex)
        //        {
        //            return Json(new { data = new string[] { }, message = ex.Message, success = false });
        //        }
        //        catch (ObjectDisposedException ex)
        //        {
        //            return Json(new { data = new string[] { }, message = ex.Message, success = false });
        //        }
        //        catch (System.Net.Mail.SmtpException ex)
        //        {
        //            return Json(new { data = new string[] { }, message = ex.Message, success = false });
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { data = new string[] { }, message = ex.Message, success = false });
        //        }
            
        //}

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("user/verify_email")]
        public IHttpActionResult getverifyemail(string email, string verification_code)
        {
            string msg = "";
            var u_user = dbEntity.USERs.Where(a => a.username == email.ToLower());

            if (u_user.Count() > 0)
            {
                long u_user_id = u_user.FirstOrDefault().id;
                //var u_ext = dbEntity.USER_ext.Where(a => a.attr_name == "email_verification_code" && a.value == verification_code);
                var u_ext = dbEntity.USERs.Where(a=> a.verification_code == verification_code);
                if (u_ext.Count() > 0)
                {
                    //return Json(new { data = new string[] { }, message = "", success = true });
                    u_ext.FirstOrDefault().verified = true;
                    u_ext.FirstOrDefault().dt_update = dt;
                    dbEntity.Entry(u_ext.FirstOrDefault()).State = System.Data.Entity.EntityState.Modified;
                    dbEntity.SaveChanges();

                    List<u_verify> n = new List<u_verify>();
                    n.Add(new u_verify
                    {
                        id = u_user_id
                    });


                    var ret1 = JsonConvert.SerializeObject(n);
                    var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                    // msg =  "Email verified."
                    msg = "Change Email verification is successful.";
                    return Json(new { data = json1, message = msg, success = true });
                }

                return Json(new { data = new string[] { }, message = "Invalid verification code.", success = false });
            }
            // msg = "Invalid email."
            msg = "This email doesn’t exist.";
            return Json(new { data = new string[] { }, message = msg, success = false });
        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("forgotpassword/verify")]
        public IHttpActionResult getforgotpassword(string email, string code)
        {
            string msg = "";
            var u_user = dbEntity.USERs.Where(a => a.username == email.ToLower());

            if (u_user.Count() > 0)
            {
                long u_user_id = u_user.FirstOrDefault().id;
                var u_ext = dbEntity.USER_ext.Where(a => a.attr_name=="verification_code" && a.value == code);
                if (u_ext.Count() > 0)
                {
                    //return Json(new { data = new string[] { }, message = "", success = true });


                    List<u_verify> n = new List<u_verify>();
                    n.Add(new u_verify
                    {
                        id = u_user_id
                    });


                    var ret1 = JsonConvert.SerializeObject(n);
                    var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                    // msg =  "Email verified."
                    msg = "Email verification is successful.";
                    return Json(new { data = json1, message = msg, success = true });
                }

                return Json(new { data = new string[] { }, message="Invalid verification code." ,success=false });
            }
            // msg = "Invalid email."
            msg = "This email doesn’t exist.";
            return Json(new {data = new string[] { }, message= msg, success = false });
        }

        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("newpassword")]
        public async Task<IHttpActionResult> updateAppointment()
        {
            var isauthorized = authorized.HSRequest(Request);
            if (isauthorized.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                });
            }

            string user_id = null, password = null;
            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var key in provider.FormData.AllKeys)
                {

                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        if (key == "user_id")
                        {
                            IsRequired(key, val, 1);
                            user_id = val;
                        }
                        else if (key == "password")
                        {
                            IsRequired(key, val, 1);
                            password = val;
                        }
                        else
                        {
                            return Json(new { message = "Object reference not set to an instance of an object. Invalid parameter name: " + key, success = false });
                        }
                    }
                }

                IsRequired("user_id", user_id, 2);
                IsRequired("password", password, 2);
                if (haserror)
                {
                    return Json(new { message = errmsg, success = false });
                }
                else
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        long user_id_new = Convert.ToInt64(user_id);
                        USER update_user = dbEntity.USERs.First(a => a.id == user_id_new);
                        string salt = System.Guid.NewGuid().ToString();
                        string encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + salt));
                        update_user.phash = encryp;
                        update_user.tlas = salt;
                        update_user.dt_update = dt;
                        update_user.update_by__USER_id = user_id_new;
                        dbEntity.Entry(update_user).State = System.Data.Entity.EntityState.Modified;
                        dbEntity.SaveChanges();
                    }
                    infomsg = "Password successfully updated.";
                    return Json(new { data = new string[] { }, message = infomsg, success = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("docxtor/loxgin")]
        public async Task<IHttpActionResult> postDoctorLogin(param_doctor_login doc_log)
        {
            // request: doctorEmail, doctorPassword
            // response: message, success, data

            // temporary code to put email and password to USER table
            #region "temp password"
            var u = dbEntity.USERs.Where(a => a.username == doc_log.email);
            string password = doc_log.password; string salt1 = ""; string encryp1; string tlas = "";
            if (u.Count() > 0) { 
                tlas = u.FirstOrDefault().tlas;
            }
          
            #endregion

            try {
                var u_doc = dbEntity.USERs.Where(a => a.username == doc_log.email);
                if (u_doc.Count() > 0)
                {
                    string encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + tlas));

                    u_doc = u_doc.Where(a => a.phash == encryp);
                    if (u_doc.Count() > 0)
                    {
                        var u_docext = dbEntity.DOCTOR_ext.Where(b => b.rel_DOCTOR_id == u_doc.FirstOrDefault().id && b.attr_name == "is_reviewed");
                        //if (u_docext.FirstOrDefault().value == "true")
                        //if(true)
                        //{
                        Random random = new Random();
                        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                        string veri_code = new string(Enumerable.Repeat(chars, 20)
                          .Select(s => s[random.Next(s.Length)]).ToArray());
                        //---
                        Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                        string login_token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(veri_code));

                        u_doc.FirstOrDefault().dt_lastin = dt;
                        u_doc.FirstOrDefault().url_lastin = HttpContext.Current.Request.Url.ToString();
                        u_doc.FirstOrDefault().token_current = login_token;
                        dbEntity.Entry(u_doc.FirstOrDefault()).State = System.Data.Entity.EntityState.Modified;
                        dbEntity.SaveChanges();

                        return Json(new { data = new string[] { }, message = "Login successful.", success = true });
                        //}
                        //else // check if is_reviewed is 'true'
                        //{
                        //    //return Json(new { data = new string[] { }, message="Doctor is pending for review.", success=false });
                        //}

                    } // else password is incorrect
                    else
                    {
                        return Json(new { data = new string[] { }, message = "Password is incorrect.", success = false });
                    }
                    
                } // else email is non existent
                return Json(new { data = new string[] { }, message = "No record found.", success = false });
            }

            catch (Exception ex) {
                return Json(new { data = new string[] { }, message =ex.Message, success = false });
            }
         

        } 

        //public bool IsRequired(string key, string val, int i)
        //{
        //    if (i == 1)
        //    {
        //        if (string.IsNullOrEmpty(val))
        //        {
        //            haserror = true;
        //            errmsg += key + " is required.\r\n ";
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }

        //    }
        //    else
        //    {
        //        if (string.IsNullOrEmpty(val))
        //        {
        //            haserror = true;
        //            errmsg += " Missing parameter: " + key + ".\r\n ";
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }

        //}

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("user/verify")]
        public async Task<IHttpActionResult> GetVerifyEmail(string email="", string code ="")
        {
            string verification = "", email2 = "";
            string msg = "";

            try
            {
                //string root = HttpContext.Current.Server.MapPath("~/Temp");
                //var provider = new MultipartFormDataStreamProvider(root);

                //await Request.Content.ReadAsMultipartAsync(provider);
                //foreach (var key in provider.FormData.AllKeys)
                //{
                //    foreach (var val in provider.FormData.GetValues(key))
                //    {

                //        switch (key)
                //        {
                //            //string email = "", password = "", user_type = "";
                //            //string acck = "deftsoftapikey", device_type = "", device_token = "";
                //            case "email":
                //                IsRequired(key, val, 1);
                //                email = val.ToLower();
                //                break;
                //            case "verification_code":
                //                IsRequired(key, val, 1);
                //                email2 = val;
                //                break;


                //            default:
                //                msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                //                return Json(new { data = "", message = msg, success = false });
                //        }
                //    }
                //}

                //IsRequired("email", email2, 2);
                //IsRequired("verification_code", verification, 2);
                //if (haserror)
                //{
                //    return Json(new { data = "", message = errmsg, success = false });
                //}

                var u = dbEntity.USERs.Where(a => a.username == email);
                if (u.Count() > 0)
                {
                    USER_ext u_ext = dbEntity.USER_ext.Where(b => b.rel_USER_id == u.FirstOrDefault().id && b.attr_name == "verified").FirstOrDefault();
                    if (u_ext != null)
                    {
                        if (u_ext.value == "true")
                        {
                            return Json(new { data = new string[] { }, message = "Verification code may only be used once.", success = false });
                        }
                        else
                        {
                            USER_ext u_ext1 = dbEntity.USER_ext.Where(b => b.rel_USER_id == u.FirstOrDefault().id && b.attr_name == "verification_code" && b.value == code).FirstOrDefault();
                            if (u_ext1 != null)
                            {
                                u_ext.value = "true";
                                u_ext.dt_update = dt;
                                dbEntity.Entry(u_ext).State = System.Data.Entity.EntityState.Modified;
                                dbEntity.SaveChanges();

                                // msg = "Verification is successful."
                                msg = "Email verification is successful.";
                                return Json(new { data = new string[] { }, message = msg, success = true });
                            }
                            // "Verification is failed."
                            msg = "Invalid verification code.";
                            return Json(new { data = new string[] { }, message =msg , success = false });
                        }
             
                    }
                    // "Verification is failed."
                    msg = "Invalid verification code.";
                    return Json(new { data = new string[] { }, message =msg, success = false});
                }
                // "Verification is failed."
                msg = "Invalid verification code.";
                return Json(new { data = new string[] { }, message = msg, success = false });

              
            }
            catch (Exception ex) {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
         }


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("user/sendxmail")]
        public async Task<IHttpActionResult> GetSendEmail(string email = "", string verification_code = "")
        {
            string verification = "", email2 = "";
            string msg = "";

            try
            {

                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage
                {
                    From = new System.Net.Mail.MailAddress("do-not-reply@healthsplash.com", "DO NOT REPLY")
                };

                message.IsBodyHtml = false;
                //message.To.Add("neilsumanda@gmail.com");
                message.To.Add(email);
                message.Subject = "my subject";
                message.Body = "bodys dfgsdgfdg";
                message.Priority = System.Net.Mail.MailPriority.Normal;
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                {
                    Port = 465,
                    Host = "mail.healthsplash.com", //"box263.bluehost.com",
                    EnableSsl = true,
                    Credentials = new NetworkCredential("do-not-reply@healthsplash.com", "eJw6V^VIYD"),
                    Timeout = 100000,
                    UseDefaultCredentials = false
                    
                };
                client.Send(message);




                //string root = HttpContext.Current.Server.MapPath("~/Temp");
                //var provider = new MultipartFormDataStreamProvider(root);

                //await Request.Content.ReadAsMultipartAsync(provider);
                //foreach (var key in provider.FormData.AllKeys)
                //{
                //    foreach (var val in provider.FormData.GetValues(key))
                //    {

                //        switch (key)
                //        {
                //            //string email = "", password = "", user_type = "";
                //            //string acck = "deftsoftapikey", device_type = "", device_token = "";
                //            case "email":
                //                IsRequired(key, val, 1);
                //                email = val.ToLower();
                //                break;
                //            case "verification_code":
                //                IsRequired(key, val, 1);
                //                email2 = val;
                //                break;


                //            default:
                //                msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                //                return Json(new { data = "", message = msg, success = false });
                //        }
                //    }
                //}

                //IsRequired("email", email2, 2);
                //IsRequired("verification_code", verification, 2);
                //if (haserror)
                //{
                //    return Json(new { data = "", message = errmsg, success = false });
                //}

                var u = dbEntity.USERs.Where(a => a.username == email);
                if (u.Count() > 0)
                {
                    USER_ext u_ext = dbEntity.USER_ext.Where(b => b.rel_USER_id == u.FirstOrDefault().id && b.attr_name == "verified").FirstOrDefault();
                    if (u_ext != null)
                    {
                        USER_ext u_ext1 = dbEntity.USER_ext.Where(b => b.rel_USER_id == u.FirstOrDefault().id && b.attr_name == "verification_code" && b.value == verification_code).FirstOrDefault();
                        if (u_ext1 != null)
                        {
                            u_ext.value = "true";
                            u_ext.dt_update = dt;
                            dbEntity.Entry(u_ext).State = System.Data.Entity.EntityState.Modified;
                            dbEntity.SaveChanges();

                            return Json(new { data = new string[] { }, message = "Verification is successful.", success = true });
                        }

                        return Json(new { data = new string[] { }, message = "Verification is failed.", success = false });
                    }

                    return Json(new { data = new string[] { }, message = "Verification is failed.", success = false });
                }
                return Json(new { data = new string[] { }, message = "Verification is failed.", success = false });


            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }

       
        /// <summary>
        /// To check if the request parameter is required or not.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool IsRequired(string key, string val, int i)
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

    public class post_userlogin {
        public string email { get; set; }
        public long user_id { get; set; }
        public string password { get; set; }
    }

    public class get_usercheckmail {
        // created: 01/10/2018
        public string email { get; set; }
    }

    public class get_response_checkmail {
        public long user_id { get; set; }
        public string email { get; set; }
        public string user_type { get; set; }
        public bool password_set { get; set; }
    }


    public class patient_upload_pic {
        public long patient_id { get; set; }
        public string image { get; set;}
    }
    public class patient_signup
    {
        public long user_id { get; set; }
        public long patient_id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string name_first { get; set; }
        public string name_last { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public long insurance_id { get; set; }
    }

    public class patient_password
    {
        public long user_id { get; set; }
        public string password { get; set; }
    }
    public class patient_login
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    public class patient_login_response {
        public bool is_email_available { get; set; }
        public bool is_password_set { get; set; }
        public long user_id { get; set; }
    }

    public class patient_login_response2 {
      public string first_name { get; set; }
        public string last_name { get; set; }
        public string profile_image { get; set; }
        public string gender { get; set; }
        public string dob { get; set; }
    }


    public class patient_verify_code {
       public string email { get; set; }
        public string verification_code { get; set; }
    }

    public class patient_password_response
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string profile_image { get; set; }
        public string gender { get; set; }
        public string dob { get; set; }
        public List<_insurance> insurance { get; set; }
    }

    public class login_get_SOUL_info {
        public string city { get; set; }
        public string state { get; set; }
        public string dob { get; set; }
        public string height { get; set; }
        public string weight { get; set; }
        public string emergency_number { get; set; }
        public string parent_guardian { get; set; }
        public string note { get; set; }
    }

    public class new_USER
    {
        public long user_id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        //public string verification_code { get; set; }
        public bool status { get; set; }
    }

    public class u_verify
    {
        public long id { get; set; }
    }

    public class param_login
    {
        public string username { get; set; }
        public string password { get; set; }
        public string platform { get; set; }
        public string device_token { get; set; }
    }

    public class param_loginsocial
    {
        public string social_id { get; set; }
        public string social_type { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public long user_type { get; set; }
        public string platform { get; set; }
        public string device_token { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public long insurance_id { get; set;}

        public DateTime dtNow { get; set; }
        public string device_type { get; set; }
    }

    public class param_doctor_login {
        public string email { get; set; }
        public string password { get; set; }
    }

    public class doctor_login_response {
        public string npi { get; set; }
        public string user_type { get; set; }
    }

    public class patient_verification_code
    {
        public string verification_code { get; set; }
    }
}
