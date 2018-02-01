using api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using authorization.hs;
using System.Data.Entity;


namespace api.Controllers
{
    public class UserController : ApiController
    {
        SV_db1Entities dbEntity = new SV_db1Entities();
        HSAuth authorized = new HSAuth();
        DateTime dt = DateTime.UtcNow;

        //[AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("user")] //user signup
        //public async Task<IHttpActionResult> Post()
        public  IHttpActionResult  Postuser([FromBody] param_user param1)
        {
            //var isauthorized = authorized.HSRequest(Request);
            //if (isauthorized.StatusCode == HttpStatusCode.Forbidden)
            //{
            //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized)
            //    {
            //        StatusCode = HttpStatusCode.Unauthorized,
            //    });
            //}

            string birthdate = param1.dob==null?"": param1.dob;

            double height= param1.height, weight= param1.weight;

            long user_id = 0,  added_user=0; 
            DateTime dtNow = DateTime.Now;
            string msg = "";

            // string root = HttpContext.Current.Server.MapPath("~/Temp");
            // var provider = new MultipartFormDataStreamProvider(root);
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
                //            case "username": ///email
                //                IsRequired(key, val, 1);
                //                //email = val;
                //                userName = val.ToLower();
                //                break;
                //            case "firstname":
                //                IsRequired(key, val, 1);
                //                firstname = val;
                //                break;
                //            case "lastname":
                //                IsRequired(key, val, 1);
                //                lastname = val;
                //                break;
                //            // user_type, insurance_type, device_type, device_token, user_id
                //            case "password":
                //                IsRequired(key, val, 1);
                //                password = val;
                //                break;
                //            //case "user_type":
                //            //    IsRequired(key, val, 1);
                //            //    user_type = val; break;
                //            //case "insurance_id":
                //            //    IsRequired(key, val, 1);
                //            //    insurance_id = val; break;

                //            case "device_type": device_type = val; break;
                //            case "device_token":
                //                IsRequired(key, val, 1);
                //                device_token = val; break;
                //            case "platform":
                //                platform = val;
                //                break;

                //            case "user_id": long nVal = 0;
                //                bool nTemp = long.TryParse(val, out nVal);
                //                user_id = nVal;
                //                break;

                //            // below values will go to SOUL_ext
                //            case "phone_number":
                //                phone_number = val;
                //                break;
                //            case "address":
                //                address = val;
                //                break;
                //            case "city": city = val; break;
                //            case "dob":
                //                bool isBool = DateTime.TryParse(val, out birthdate);
                //                break;
                //            case "parent_guardian":
                //                parent_guardian = val; break;
                //            case "height":
                //                bool b = double.TryParse(val, out height); break;
                //            case "weight":
                //                bool b1 = double.TryParse(val, out weight); break;
                //            case "emergency_number":
                //                emergency_number = val; break;
                //            case "note":
                //                note = val; break;
                //            case "gender":
                //                gender = val; break;

                //            default:
                //                msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                //                return Json(new { data = new string[] { }, message = msg, success = false });
                //        }
                //   }
                //}
                #endregion
                IsRequired("username", param1.username, 2);
                //8/23 IsRequired("insurance_id", insurance_id, 2);
                IsRequired("first_name", param1.first_name, 2);
                IsRequired("last_name", param1.last_name, 2);
                IsRequired("password", param1.password, 2);
                //8/23 IsRequired("user_type", user_type, 2);
                IsRequired("device_token", param1.device_token, 2);

                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }

                var check_dup = dbEntity.USERs.Where(a =>a.username == param1.username);
                if (check_dup.Count() > 0)
                {
                    msg = "Email already exist.";
                    return Json(new { data = new string[] { }, message = msg, success = false });
                }

                string salt = System.Guid.NewGuid().ToString();
                string encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(param1.password + salt));
                var u_type = dbEntity.ref_USER_type.Where(a => a.dname.ToLower() == "patient").FirstOrDefault();
               
                USER new_user = new USER();
                //required fields
                            new_user.name_first = param1.first_name;
                            new_user.name_last = param1.last_name;
                            new_user.username = param1.username.ToLower();
                            new_user.dt_create = dt;
                            new_user.create_by__USER_id = user_id; //create != null ? 0 : create.id;
                            new_user.rel_ref_status_id = dbEntity.ref_status_type.Where(a => a.dname == "USER").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname == "Active").id;
                            new_user.rel_ref_USER_type_id = u_type.id;
                            new_user.phash = encryp;
                            new_user.tlas = salt;
                //            // insurance_type
                //            // user_type

                           dbEntity.USERs.Add(new_user);
                           dbEntity.SaveChanges();

                added_user = new_user.id;

                // generate verification code
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                string veri_code = new string(Enumerable.Repeat(chars, 4)
                  .Select(s => s[random.Next(s.Length)]).ToArray());

        

                bool i =  Validation.saveUSER_ext("verification_code", "Verification Code", veri_code, new_user.id);
                #region
                //USER_ext u_ext = new USER_ext();
                //u_ext.rel_USER_id = new_user.id;
                //u_ext.attr_name = "verification_code";
                //u_ext.dname = "Verification Code";
                //u_ext.rel_ref_datatype_id = 2;
                //u_ext.value = veri_code;
                //u_ext.create_by__USER_id = added_user; // user_id;
                //u_ext.dt_create = dt;
                //dbEntity.USER_ext.Add(u_ext);
                //dbEntity.SaveChanges();
                #endregion

                i = Validation.saveUSER_ext("verified", "Verified", "false", new_user.id);
                #region
                //u_ext = new USER_ext();
                //u_ext.rel_USER_id = new_user.id;
                //u_ext.attr_name = "verified";
                //u_ext.dname = "Verified";
                //u_ext.rel_ref_datatype_id = 4;
                //u_ext.value = "false";
                //u_ext.create_by__USER_id = added_user; //user_id;
                //u_ext.dt_create = dt;
                //dbEntity.USER_ext.Add(u_ext);
                //dbEntity.SaveChanges();
                #endregion

                // platform
                if (!string.IsNullOrEmpty(param1.platform))
                {
                   i = Validation.saveUSER_ext("platform", "Platform", param1.platform, new_user.id);

                    #region
                    //u_ext = new USER_ext();
                    //u_ext.rel_USER_id = new_user.id;
                    //u_ext.attr_name = "platform";
                    //u_ext.dname = "Platform";
                    //u_ext.rel_ref_datatype_id = 4;
                    //u_ext.value = platform;
                    //u_ext.create_by__USER_id = added_user; //user_id;
                    //u_ext.dt_create = dt;
                    //dbEntity.USER_ext.Add(u_ext);
                    //dbEntity.SaveChanges();
                    #endregion
                }

                // device_token
                if (!string.IsNullOrEmpty(param1.device_token))
                {
                    i = Validation.saveUSER_ext("device_token", "Device Token", param1.device_token, new_user.id);

                    #region
                    //u_ext = new USER_ext();
                    //u_ext.rel_USER_id = new_user.id;
                    //u_ext.attr_name = "device_token";
                    //u_ext.dname = "Device Token";
                    //u_ext.rel_ref_datatype_id = 4;
                    //u_ext.value = device_token;
                    //u_ext.create_by__USER_id = added_user; //user_id;
                    //u_ext.dt_create = dt;
                    //dbEntity.USER_ext.Add(u_ext);
                    //dbEntity.SaveChanges();
                    #endregion

                }

                //if (user_type.ToLower() == "patient") 
                //{
                SOUL new_soul = new SOUL();
                    new_soul.name_first = param1.first_name;
                    new_soul.name_last = param1.last_name;
                    new_soul.email = param1.username.ToLower();
                    new_soul.is_prime = true;
                    if (!string.IsNullOrEmpty(param1.phone_number))
                    {
                        new_soul.phone = param1.phone_number;
                    }

                    if (!string.IsNullOrEmpty(param1.street))
                    {
                        new_soul.addr_address1 = param1.street;
                    }

                if (!string.IsNullOrEmpty(param1.gender))
                {
                    new_soul.gender = param1.gender[0].ToString().ToUpper();
                }


                    new_soul.dt_create = dt;
                    new_soul.rel_ref_status_id = dbEntity.ref_status_type.Where(a => a.dname == "SOUL").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname == "Active").id;
                    new_soul.create_by__USER_id = added_user; // user_id;
                dbEntity.SOULs.Add(new_soul);
                    dbEntity.SaveChanges();

                #region
                //if (!string.IsNullOrEmpty(param1.insurance_id)) {
                //     i = saveSOUL_ext("insurance_id", "Insurance ID", param1.insurance_id, new_soul.id);


                //    //SOUL_ext ext_ins_type = new SOUL_ext();
                //    //ext_ins_type.rel_SOUL_id = new_soul.id;
                //    //ext_ins_type.attr_name = "insurance_id";
                //    //ext_ins_type.dname = "Insurance ID";
                //    //ext_ins_type.value =  insurance_id;
                //    //ext_ins_type.rel_ref_datatype_id = 2;
                //    //ext_ins_type.dt_create = dt;
                //    //ext_ins_type.create_by__USER_id = added_user; //user_id;
                //    //dbEntity.SOUL_ext.Add(ext_ins_type);
                //    //dbEntity.SaveChanges();
                //}
                  #endregion

                // city
                if (!string.IsNullOrEmpty(param1.city))
                    {
                     i = Validation.saveSOUL_ext("city", "City", param1.city, new_soul.id, new_user.id);

                    #region

                    //SOUL_ext ext_city = new SOUL_ext();
                    //ext_city.rel_SOUL_id = new_soul.id;
                    //ext_city.attr_name = "city";
                    //ext_city.dname = "City";
                    //ext_city.value = city;
                    //ext_city.rel_ref_datatype_id = 2;
                    //ext_city.dt_create = dt;
                    //ext_city.create_by__USER_id = added_user; // user_id;
                    //dbEntity.SOUL_ext.Add(ext_city);
                    //dbEntity.SaveChanges();
                    #endregion

                }

                #region
                //// date of birth
                //if (birthdate > new DateTime())
                //{
                //    SOUL_ext ext_dob = new SOUL_ext();
                //    ext_dob.rel_SOUL_id = new_soul.id;
                //    ext_dob.attr_name = "dob";
                //    ext_dob.dname = "DOB";
                //    ext_dob.value = birthdate.ToShortDateString();
                //    ext_dob.rel_ref_datatype_id = 5;
                //    ext_dob.dt_create = dt;
                //    ext_dob.create_by__USER_id = added_user; //user_id;
                //     dbEntity.SOUL_ext.Add(ext_dob);
                //    dbEntity.SaveChanges();
                //}
                #endregion

                if (!string.IsNullOrEmpty(birthdate))
                {
                     i = Validation.saveSOUL_ext("dob", "Date Of Birth", birthdate, new_soul.id, new_user.id);
                    #region

                    //SOUL_ext ext_dob = new SOUL_ext();
                    //ext_dob.rel_SOUL_id = new_soul.id;
                    //ext_dob.attr_name = "dob";
                    //ext_dob.dname = "DOB";
                    //ext_dob.value = birthdate;
                    //ext_dob.rel_ref_datatype_id = 5;
                    //ext_dob.dt_create = dt;
                    //ext_dob.create_by__USER_id = added_user; //user_id;
                    //dbEntity.SOUL_ext.Add(ext_dob);
                    //dbEntity.SaveChanges();
                    #endregion
                }


                // parent/guardian
                if (!string.IsNullOrEmpty(param1.parent_guardian))
                    {
                     i = Validation.saveSOUL_ext("parent_guardian", "Parent or Guardian", param1.parent_guardian, new_soul.id, new_user.id);

                    #region
                    //SOUL_ext ext_parent = new SOUL_ext();
                    //ext_parent.rel_SOUL_id = new_soul.id;
                    //ext_parent.attr_name = "parent_guardian";
                    //ext_parent.dname = "Parent or Guardian";
                    //ext_parent.value = parent_guardian;
                    //ext_parent.rel_ref_datatype_id = 2;
                    //ext_parent.dt_create = dt;
                    //ext_parent.create_by__USER_id = added_user; //user_id;
                    //dbEntity.SOUL_ext.Add(ext_parent);
                    //dbEntity.SaveChanges();
                    #endregion

                }

                if (param1.height > 0)
                {
                     i = Validation.saveSOUL_ext("height", "Height", param1.height.ToString(), new_soul.id, new_user.id);

                    #region
                    //SOUL_ext ext_height = new SOUL_ext();
                    //ext_height.rel_SOUL_id = new_soul.id;
                    //ext_height.attr_name = "height";
                    //ext_height.dname = "Height";
                    //ext_height.value = height.ToString();
                    //ext_height.rel_ref_datatype_id = 1;
                    //ext_height.dt_create = dt;
                    //ext_height.create_by__USER_id = added_user; // user_id;
                    //dbEntity.SOUL_ext.Add(ext_height);
                    //dbEntity.SaveChanges();
                    #endregion
                }

                if (param1.weight > 0)
                    {
                     i = Validation.saveSOUL_ext("weight", "Weight", param1.weight.ToString(), new_soul.id, new_user.id);

                    #region
                    //SOUL_ext ext_weight = new SOUL_ext();
                    //ext_weight.rel_SOUL_id = new_soul.id;
                    //ext_weight.attr_name = "weight";
                    //ext_weight.dname = "Weight";
                    //ext_weight.value = weight.ToString();
                    //ext_weight.rel_ref_datatype_id = 1;
                    //ext_weight.dt_create = dt;
                    //ext_weight.create_by__USER_id = added_user; //user_id;
                    //dbEntity.SOUL_ext.Add(ext_weight);
                    //dbEntity.SaveChanges();
                    #endregion

                }

                    if (!string.IsNullOrEmpty(param1.emergency_number))
                {
                     i = Validation.saveSOUL_ext("emergency_number", "Emergency Number", param1.emergency_number, new_soul.id, new_user.id);

                    #region
                    //SOUL_ext ext_emerg = new SOUL_ext();
                    //ext_emerg.rel_SOUL_id = new_soul.id;
                    //ext_emerg.attr_name = "emergency_number";
                    //ext_emerg.dname = "Emergency Number";
                    //ext_emerg.value = emergency_number.ToString();
                    //ext_emerg.rel_ref_datatype_id = 2;
                    //ext_emerg.dt_create = dt;
                    //ext_emerg.create_by__USER_id = added_user; //user_id;
                    //dbEntity.SOUL_ext.Add(ext_emerg);
                    //dbEntity.SaveChanges();
                    #endregion

                }

                    if (!string.IsNullOrEmpty(param1.note))
                    {
                     i = Validation.saveSOUL_ext("note", "Note", param1.note, new_soul.id, new_user.id);

                    #region

                    //SOUL_ext ext_note = new SOUL_ext();
                    //ext_note.rel_SOUL_id = new_soul.id;
                    //ext_note.attr_name = "note";
                    //ext_note.dname = "Note";
                    //ext_note.value = note.ToString();
                    //ext_note.rel_ref_datatype_id = 2;
                    //ext_note.dt_create = dt;
                    //ext_note.create_by__USER_id = added_user; // user_id;
                    //dbEntity.SOUL_ext.Add(ext_note);
                    //dbEntity.SaveChanges();
                    #endregion

                }

                #region "insurance_id"
                //if (!string.IsNullOrEmpty(insurance_id))
                //{
                //    i = saveSOUL_ext("insurance_id", "Insurance ID", insurance_id, new_soul.id);

                //    #region
                //    //SOUL_ext ext_note = new SOUL_ext();
                //    //ext_note.rel_SOUL_id = new_soul.id;
                //    //ext_note.attr_name = "insurance_id";
                //    //ext_note.dname = "Insurance ID";
                //    //ext_note.value = insurance_id.ToString();
                //    //ext_note.rel_ref_datatype_id = 2;
                //    //ext_note.dt_create = dt;
                //    //ext_note.create_by__USER_id = added_user; //user_id;
                //    //dbEntity.SOUL_ext.Add(ext_note);
                //    //dbEntity.SaveChanges();
                //    #endregion
                //}
                #endregion


                #region
                //var ref_ins = dbEntity.ref_insurance_provider.Where(a => a.PayerName == "");
                //if (ref_ins != null)
                //{
                //    con_SOUL_ref_insurance soul_ref = new con_SOUL_ref_insurance();
                //    soul_ref.rel_SOUL_id = new_soul.id;
                //    soul_ref.rel_ref_insurance_provider_id = 0;
                //    dbEntity.con_SOUL_ref_insurance.Add(soul_ref);
                //    dbEntity.SaveChanges();
                //}
                //}
                #endregion

                string insurance_id = "";
                // add patient info
                List<user_secondary_patient> prime_pat = new List<user_secondary_patient>();
             
                long nInsurance = 0;
                bool bIns = long.TryParse(insurance_id, out nInsurance);
                var ins_name = dbEntity.ref_insurance_provider.Find(nInsurance);

                string zipcode = "", insuranceid = "", insurancename = "";
                if (new_soul.is_prime.Value)
                {
                    zipcode = new_soul.ref_zip == null ? "" : new_soul.ref_zip.zip;
                    insuranceid = insurance_id;
                    insurancename = ins_name == null ? "" : ins_name.PayerName.Split('|')[0];
                }
                else // 8-29-2017 removing primary patient in the patient List
                {
                    prime_pat.Add(new user_secondary_patient
                    {
                        id = new_soul.id,
                        first_name = new_soul.name_first == null ? "" : new_soul.name_first,
                        last_name = new_soul.name_last == null ? "" : new_soul.name_last,
                        is_prime = new_soul.is_prime.Value,
                        //insurance_id =  p_ext.Count() == 0 ? "" : p_ext.ToList().First().value.ToString(),
                        is_using_primary_patient_insurance = true,
                        insurance_id = insurance_id,
                        insurance_name = ins_name == null ? "" : ins_name.PayerName.Split('|')[0]
                    });

                }

                List<get_User> n = new List<get_User>();
                //var ref_stat = dbEntity.ref_status_type.Where(a => a.dname == "USER").FirstOrDefault().ref_status; //.ref_status.FirstOrDefault(b => b.dname == "Active");
                var ref_stat = dbEntity.ref_status.Find(new_user.rel_ref_status_id);
                var _ext = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == new_user.id);
                var u_state = dbEntity.SOUL_ext.Where(a => a.attr_name == "state" && a.rel_SOUL_id == new_user.id).FirstOrDefault();

                n.Add(new get_User
                {
                    user_id = new_user.id,
                    first_name = new_user.name_first == null ? "" : new_user.name_first,
                    last_name = new_user.name_last == null ? "" : new_user.name_last,
                    username = new_user.username == null ? "" : new_user.username,
                    gender = string.IsNullOrEmpty(param1.gender) ? "" : param1.gender[0].ToString(),
                    //status = ref_stat == null ? "" : ref_stat.dname,
                    //verification_code = veri_code,
                    verified = false,
                    password = param1.password,
                    social_id = "",
                    social_type = "",
                    dob = string.IsNullOrEmpty(birthdate) ? "" : birthdate,
                    phone_no = param1.phone_number == null ? "" : param1.phone_number,
                    street = param1.street == null ? "" : param1.street,
                    zip_code = "",
                    city = param1.city == null ? "" : param1.city,
                    state = u_state == null ? "" : u_state.value,
                    parent_guardian = param1.parent_guardian == null ? "" : param1.parent_guardian,
                    height = height,
                    weight = weight,
                    emergency_number = param1.emergency_number == null?"": param1.emergency_number,
                    note = param1.note==null?"": param1.note,
                    insurance_id = "",
                    insurance_name = "",
                    image_url = "",
                    patient = prime_pat
                });


                // send verification through email
                #region " send verification"

                // ~/user/verify

                System.Net.Mail.MailAddress to = new System.Net.Mail.MailAddress(new_user.username);
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage
                    {
                        From = new System.Net.Mail.MailAddress("do-not-reply@healthsplash.com", "DO NOT REPLY")
                    };

                    message.IsBodyHtml = true;
                    //message.To.Add("neilsumanda@gmail.com");
                    message.To.Add(to);
                    message.Subject = "Verification Code";
                //message.Body = "VERIFICATION CODE: " + "<a href=http://www.healthsplash.com/verify-user?email=" + to + "&code=" + veri_code + ">" + "here" + "</a>, to verify.";
                //message.Body = "VERIFICATION CODE: " + "<a href=http://dev.healthsplash.com/Patient/confirmemail?email=" + to + "&code=" + veri_code + ">" + "here" + "</a>, to verify.";
                string str = "<!doctype html><html><head><meta charset=\"utf-8\"><title> Health Splash</title></head>";
                str += "<body><table style = \"width:600px; margin:0 auto;\" border = \"0\" cellpadding = \"0\" cellspacing = \"0\">";
                str += "<tr><td style = \"text-align:center; padding:25px 0;\"><img src=\"http://healthsplash.com/Content/images/logo.png\" alt=\"Health Splash\"/></td></tr>";
                str += "<tr><td style =\"border:1px solid #eee; background:#fff; padding:30px 60px\">";
                str += "<table width =\"100%\" border = \"0\" cellspacing=\"0\" cellpadding=\"0\">";

                str += "<tr><td style=\"text-align:center;\"><img src=\"http://dev.healthsplash.com/Content/images/icon-newsletter.png\" alt=\"Newsletter\"/></td></tr>";
                str += "<tr><td style=\"padding:25px 0; font-size:22px; color:#666; text-align:center; font-family: Arial, 'Helvetica Neue', Helvetica, sans-serif; border-bottom:1px solid #eee\"> Verify your email address </td></tr>";
                str += "<tr><td style=\"padding:25px 0 0; font-size:16px; color:#666; text-align:center; line-height:30px; font-family: Arial, 'Helvetica Neue', Helvetica, sans-serif;\" > In order to start your Health Splash account, you need to confirm your email address.</td></tr>";
                str += "<tr><td style= \"padding:25px 0; border-bottom:1px solid #eee\" ><a href=\"http://dev.healthsplash.com/Patient/confirmemail?email=" + to + "&code=" + veri_code + "\" style=\"background: #ee6c7a; display:block; text-align:center; text-decoration:none;border-radius: 5px;color: #ffffff;font-size: 18px;font-weight: 500;height: 53px;line-height: 2.9;margin: 0;padding: 0 50px;font-family: Arial, \'Helvetica Neue\', Helvetica, sans-serif;\" > Verify Email Address</a></td></tr>";

                string str2 = "If you did not sign up for this account you can ignore this email and the account will be deleted.";
                str += "<tr><td style=\"padding:25px 0 0; font-size:15px; color:#999; font-style:italic; text-align:center; font-family: Arial, \'Helvetica Neue\', Helvetica, sans-serif; line-height:24px;\">" + str2 + "</td></tr>";
                str += "</table>";
                str += "</td>";
                str += "</tr>";
                str += "</table>";
                str += "</body>";
                str += "</html>";

                message.Body = str;

                message.Priority = System.Net.Mail.MailPriority.Normal;
                    System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                    {
                        //Port = 587,
                        //Host = "box995.bluehost.com", //"box263.bluehost.com",
                        //EnableSsl = true,
                        //Credentials = new NetworkCredential("do-not-reply@healthsplash.com", "eJw6V^VIYD"),
                        //Timeout = 100000,
                        //UseDefaultCredentials = false
                        Port = 587, //465,
                        Host = "box995.bluehost.com",
                        EnableSsl = true,
                        Credentials = new NetworkCredential("staff@nationalcenterforpain.com", "Staff1@#$%"),
                        Timeout = 10000
                    };
                    client.Send(message);
                #endregion


                var ret1 = JsonConvert.SerializeObject(n);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                
                return Json(new { data= json1, message="Registration successful.", success = true });
               
            }
            catch(Exception ex)
            {

               
                return Json(new {data= new string[] { }, message = ex.Message, success = false });
            }
                
        }

      

      

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("user/changepassword")]
        public IHttpActionResult getchangepassword(string email = "", string password ="")
        {
            IsRequired("email", email, 2);
            IsRequired("password", password, 2);
            if (haserror)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }

            try {
                var _user = dbEntity.USERs.Where(a => a.username == email.ToLower());
                foreach (USER u in _user)
                {
                    string salt = System.Guid.NewGuid().ToString();
                    string encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + salt));
                    //var u_type = dbEntity.ref_USER_type.Where(a => a.dname.ToLower() == "user").FirstOrDefault();

                    //USER new_user = new USER();
                    //required fields
                    //new_user.name_first = firstname;
                    //new_user.name_last = lastname;
                    // new_user.username = userName.ToLower();
                    u.dt_update = dt;
                    //new_user.create_by__USER_id = user_id; //create != null ? 0 : create.id;
                    //new_user.rel_ref_status_id = dbEntity.ref_status_type.Where(a => a.dname == "USER").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname == "Active").id;
                    //new_user.rel_ref_USER_type_id = u_type.id;
                    u.phash = encryp;
                    u.tlas = salt;
                    //            // insurance_type
                    //            // user_type

                    dbEntity.Entry(u).State = EntityState.Modified;
                   
                }
                dbEntity.SaveChanges();

                return Json(new { data = new string[] { }, message="Successfully changed password.", success=true });
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("user/changepassword/verify")]
        public IHttpActionResult getchangepasswordverify(string email = "", string old_password = "", string new_password ="")
        {
            IsRequired("email", email, 2);
            IsRequired("old_password", old_password, 2);
            IsRequired("new_password", new_password, 2);
            if (haserror)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }

            try
            {
                var _user = dbEntity.USERs.Where(a => a.username == email.ToLower());
                foreach (var u in _user)
                {
                    string salt = u.tlas;
                    string encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(old_password + salt));

                    var pass_verify = _user.Where(a => a.phash == encryp);
                    if (pass_verify.Count() > 0)
                    {
                        string new_salt = System.Guid.NewGuid().ToString();
                        string new_encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(new_password + new_salt));

                       //var u_type = dbEntity.ref_USER_type.Where(a => a.dname.ToLower() == "user").FirstOrDefault();

                        //USER new_user = new USER();
                        //required fields
                        //new_user.name_first = firstname;
                        //new_user.name_last = lastname;
                        // new_user.username = userName.ToLower();
                        u.dt_update = dt;
                        //new_user.create_by__USER_id = user_id; //create != null ? 0 : create.id;
                        //new_user.rel_ref_status_id = dbEntity.ref_status_type.Where(a => a.dname == "USER").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname == "Active").id;
                        //new_user.rel_ref_USER_type_id = u_type.id;
                        u.phash = new_encryp;
                        u.tlas = new_salt;
                        //            // insurance_type
                        //            // user_type

                        dbEntity.Entry(u).State = System.Data.Entity.EntityState.Modified;
                       
                    }
                    else
                    {
                        return Json(new { data = new string[] { }, message = "Unable to verify old_password value.", success = false });
                    }

              
                }
                dbEntity.SaveChanges();
                return Json(new { data = new string[] { }, message = "Successfully changed password.", success = true });
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }

        [HttpPost]
        [System.Web.Http.Route("wr/user/changeemail")]
        public async Task<IHttpActionResult> wrchangeemail()
        {
            string username = "", new_username = "";

            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);
            string msg = "";

           

            try {
                string veri_code = "";
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                       
                        switch (key)
                        {
                            case "username":
                                IsRequired(key, val, 1);
                                long nVal = 0;
                                bool nTemp = long.TryParse(val, out nVal);
                                username = val;
                                break;
                            case "new_username":
                                IsRequired(key, val, 1);

                                new_username = val;
                                break;

                            default:
                                msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                                return Json(new { data = new string[] { }, message = msg, success = false });
                        }
                    }
                }

                    IsRequired("username", username, 2);
                    IsRequired("new_username", new_username, 2);
                    if (haserror)
                    {
                        return Json(new { data = new string[] { }, message = errmsg, success = false });
                    }


                    var u = dbEntity.USERs.Where(a => a.username == username);
                    if (u.Count() > 0)
                    {
                        long user_id_new = u.FirstOrDefault().id;
                        // check if new_username exist anywhere else
                        var new_u = dbEntity.USERs.Where(a => a.username == new_username.ToLower());
                        if (new_u.Count() > 0)
                        {
                            // print json error: username already exist
                            return Json(new { data = new string[] { }, message = "New username already exist.", success = false });
                        }
                        else
                        {
                            var soul = dbEntity.SOULs.Where(a => a.email == username.ToLower()).FirstOrDefault();
                            soul.email = new_username.ToLower();
                        dbEntity.Entry(soul).State = System.Data.Entity.EntityState.Modified;

                        u.FirstOrDefault().username = new_username.ToLower();
                            dbEntity.Entry(u.FirstOrDefault()).State = System.Data.Entity.EntityState.Modified;

                        // send verification mail
                        Random random = new Random();
                            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                            veri_code = new string(Enumerable.Repeat(chars, 4)
                              .Select(s => s[random.Next(s.Length)]).ToArray());

                            var u_verification_code = dbEntity.USER_ext.Where(a => a.rel_USER_id == user_id_new && a.attr_name == "verification_code").FirstOrDefault();
                            var u_verified = dbEntity.USER_ext.Where(a => a.rel_USER_id == user_id_new && a.attr_name == "verified").FirstOrDefault();

                            bool hasVerification = false, hasVerified = false;
                            if (u_verification_code != null)
                            {
                                hasVerification = true;
                                u_verification_code.value = veri_code;
                                dbEntity.Entry(u_verification_code).State = System.Data.Entity.EntityState.Modified;
                            }

                            if (u_verified != null)
                            {
                                u_verified.value = "false";
                                hasVerified = true;
                                dbEntity.Entry(u_verified).State = System.Data.Entity.EntityState.Modified;
                            }
                        dbEntity.SaveChanges();

                        if (!hasVerification)
                        {
                            bool i = Validation.saveUSER_ext("verification_code", "Verification Code", veri_code, u.FirstOrDefault().id);

                            #region
                            //USER_ext u_ext = new USER_ext();
                            ////u_ext.rel_USER_id =
                            //u_ext.rel_USER_id = u.FirstOrDefault().id;
                            //u_ext.attr_name = "verification_code";
                            //u_ext.dname = "Verification Code";
                            //u_ext.rel_ref_datatype_id = 2;
                            //u_ext.value = veri_code;
                            //u_ext.create_by__USER_id = user_id_new;
                            //u_ext.dt_create = dt;
                            //dbEntity.USER_ext.Add(u_ext);

                            #endregion
                        }

                        if (!hasVerified)
                        {
                            bool i = Validation.saveUSER_ext("verified", "Verified", "false", u.FirstOrDefault().id);

                            #region
                            //USER_ext u_ext = new USER_ext();
                            //// u_ext.rel_USER_id = 
                            //u_ext.rel_USER_id = u.FirstOrDefault().id;
                            //u_ext.attr_name = "verified";
                            //    u_ext.dname = "Verified";
                            //    u_ext.rel_ref_datatype_id = 2;
                            //    u_ext.value = "false";
                            //    u_ext.create_by__USER_id = user_id_new;
                            //    u_ext.dt_create = dt;
                            //    dbEntity.USER_ext.Add(u_ext);
                            #endregion
                        }
                    }
                    
                }

                // send verification code through email
                if (!string.IsNullOrEmpty(veri_code))
                {
                    System.Net.Mail.MailAddress from = new System.Net.Mail.MailAddress("do-not-reply@healthsplash.com");
                    System.Net.Mail.MailAddress to = new System.Net.Mail.MailAddress(new_username);

                    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(from, to);

                    message.IsBodyHtml = true;
                    message.Subject = "HealthSplash Username Verification";
                    //message.Body = "<br><br>Click " + "<a href=http://www.healthsplash.com/activate-email?email=" + to + "&code="  + u_ext_veri.value + ">" + "here" + "</a>, to verify.";
                    //message.Body = "<br><br>Click " + "<a href=http://dev.healthsplash.com/patient/verifyemail?email=" + to + "&code=" + veri_code + ">" + "here" + "</a>, to verify.";
                    string str1 = "";

                    string str = "<!doctype html><html><head><meta charset=\"utf-8\"><title> Health Splash</title></head>";
                    str += "<body><table style = \"width:600px; margin:0 auto;\" border = \"0\" cellpadding = \"0\" cellspacing = \"0\">";
                    str += "<tr><td style = \"text-align:center; padding:25px 0;\"><img src =\"http://healthsplash.com/Content/images/logo.png\" alt=\"Health Splash\"/></td></tr>";
                    str += "<tr><td style =\"border:1px solid #eee; background:#fff; padding:30px 60px\">";
                    str += "<table width =\"100%\" border = \"0\" cellspacing=\"0\" cellpadding=\"0\">";
                    str += "<tr><td style=\"text-align:center;\"><img src=\"http://dev.healthsplash.com/Content/images/icon-newsletter.png\" alt=\"Newsletter\"/></td></tr>";
                    str += "<tr><td style=\"padding:25px 0; font-size:22px; color:#666; text-align:center; font-family: Arial, 'Helvetica Neue', Helvetica, sans-serif; border-bottom:1px solid #eee\"> Verify your email address </td></tr>";
                    str += "<tr><td style=\"padding:25px 0 0; font-size:16px; color:#666; text-align:center; line-height:30px; font-family: Arial, 'Helvetica Neue', Helvetica, sans-serif;\"> In order to start your Health Splash account, you need to confirm your email address.</td></tr>";
                    str += "<tr><td style=\"padding:25px 0; border-bottom:1px solid #eee\"><a href=\"http://dev.healthsplash.com/patient/verifyemail?email=" + to + "&code=" + veri_code + "\" style=\"background: #ee6c7a; display:block; text-align:center; text-decoration:none;border-radius: 5px;color: #ffffff;font-size: 18px;font-weight: 500;height: 53px;line-height: 2.9;margin: 0;padding: 0 50px;font-family: Arial, \'Helvetica Neue\', Helvetica, sans-serif;\"> Verify Email Address</a></td></tr>";

                    str1 = "";
                    str += "<tr><td style=\"padding:25px 0 0; font-size:15px; color:#999; font-style:italic; text-align:center; font-family: Arial, \'Helvetica Neue\', Helvetica, sans-serif; line-height:24px;\">" + str1 +  "</td></tr>";
                    str += "</table>";
                    str += "</td>";
                    str += "</tr>";
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
                    return Json(new { data = new string[] { }, message = "New username successfully changed.", success = true });
                }

                return Json(new { data = new string[] { }, message = "Email sending failed.", success = false });

            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
           

        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("user/demographic")] //user signup
        public async Task<IHttpActionResult> demographic()
        {
           
            string root = HttpContext.Current.Server.MapPath("~/Temp");
            string  race = "", ethnicity = "", language = "";
            long user_id = 0;
            string msg = "";
            bool check_success = false, syn_with_calendar = false;
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
                            case "race":
                                IsRequired(key, val, 1);
                                race = val;
                                break;
                            case "ethnicity":
                                IsRequired(key, val, 1);
                                ethnicity = val;
                                break;
                            case "language":
                                IsRequired(key, val, 1);
                                language = val;
                                break;
                            // user_type, insurance_type, device_type, device_token, user_id
                            case "syn_with_calendar":
                                IsRequired(key, val, 1);
                                bool bval = false;
                                bool ntemp = bool.TryParse(val, out bval);
                                syn_with_calendar = bval;
                                break;
                            case "user_id":
                                IsRequired(key, val, 1);
                                long nVal = 0;
                                bool nTemp = long.TryParse(val, out nVal);
                                user_id = nVal;
                                break;
                            default:
                                msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                                return Json(new { data = new string[] { }, message = msg, success = false });
                        }
                    }
                }

                IsRequired("user_id", Convert.ToString(user_id), 2);
                IsRequired("race", race, 2);
                IsRequired("ethnicity", ethnicity, 2);
                IsRequired("language", language, 2);
                IsRequired("syn_with_calendar", Convert.ToString(syn_with_calendar), 2);
                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }

                var user_tbl = dbEntity.USERs.Where(a => a.id == user_id);
                if (user_tbl.Count() > 0)
                {
                    var user_ext = dbEntity.USER_ext.Where(a => a.rel_USER_id == user_id);
                    bool is_user_race_exist = false, is_user_ethnicity = false, is_user_language = false, is_user_syn_with_calendar = false;
                    if (user_ext.Count() > 0)
                    {
                        foreach (var i in user_ext)
                        {
                            if (i.attr_name == "user_race")
                            {
                                i.value = race.ToString();
                                i.update_by__USER_id = user_id;
                                i.dt_update = dt;
                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                                is_user_race_exist = true;
                            }
                            if (i.attr_name == "user_ethnicity")
                            {
                                i.value = ethnicity.ToString();
                                i.update_by__USER_id = user_id;
                                i.dt_update = dt;
                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                                is_user_ethnicity = true;
                            }
                            if (i.attr_name == "user_language")
                            {
                                i.value = language.ToString();
                                i.update_by__USER_id = user_id;
                                i.dt_update = dt;
                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                                is_user_language = true;
                            }
                            if (i.attr_name == "user_syn_with_calendar")
                            {
                                i.value = syn_with_calendar.ToString();
                                i.update_by__USER_id = user_id;
                                i.dt_update = dt;
                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                                is_user_syn_with_calendar = true;
                            }

                        }
                    }
                    if (!is_user_race_exist)
                    {

                        USER_ext user_ext_race = new USER_ext()
                        {
                            rel_USER_id = user_id,
                            attr_name = "user_race",
                            dname = "User Race",
                            rel_ref_datatype_id = 2,
                            value = race.ToString(),
                            create_by__USER_id = user_id,
                            dt_create = dt
                        };
                        dbEntity.USER_ext.Add(user_ext_race);
                    }
                    if (!is_user_ethnicity)
                    {
                        USER_ext user_ext_ethnicity = new USER_ext()
                        {
                            rel_USER_id = user_id,
                            attr_name = "user_ethnicity",
                            dname = "User Ethnicity",
                            rel_ref_datatype_id = 2,
                            value = ethnicity.ToString(),
                            create_by__USER_id = user_id,
                            dt_create = dt
                        };
                        dbEntity.USER_ext.Add(user_ext_ethnicity);
                    }
                    if (!is_user_language)
                    {
                        USER_ext user_ext_language = new USER_ext()
                        {
                            rel_USER_id = user_id,
                            attr_name = "user_language",
                            dname = "User Language",
                            rel_ref_datatype_id = 2,
                            value = language.ToString(),
                            create_by__USER_id = user_id,
                            dt_create = dt
                        };
                        dbEntity.USER_ext.Add(user_ext_language);
                    }
                    if (!is_user_syn_with_calendar)
                    {
                        USER_ext user_ext_syn_with_calendar = new USER_ext()
                        {
                            rel_USER_id = user_id,
                            attr_name = "user_syn_with_calendar",
                            dname = "User Syn With Calendar",
                            rel_ref_datatype_id = 3,
                            value = syn_with_calendar.ToString(),
                            create_by__USER_id = user_id,
                            dt_create = dt
                        };
                        dbEntity.USER_ext.Add(user_ext_syn_with_calendar);
                    }
                    dbEntity.SaveChanges();
                    msg = "Record saved/updated.";
                    check_success = true;

                }
                else
                {
                    msg = "No matching record found.";
                    check_success = false;
                }
                
                //var ret1 = JsonConvert.SerializeObject("testc");
                //var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                return Json(new { data = new string[] { }, message = msg, success = check_success });

            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }

        }




        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("us1er/so1cial")]
        public async Task<IHttpActionResult> Postsocial()
        {
            //var isauthorized = authorized.HSRequest(Request);
            //if (isauthorized.StatusCode == HttpStatusCode.Forbidden)
            //{
            //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized)
            //    {
            //        StatusCode = HttpStatusCode.Unauthorized,
            //    });
            //}

            string root = HttpContext.Current.Server.MapPath("~/Temp");
            string firstname = "", lastname = "", social_id = "", password = "", user_type = "", acck = "deftsoftapikey";
            string email="", insurance_id = "", device_type = "", device_token = "";
            //string firstname, string lastname, string email, string password, string user_type,
            string phone_number = "", address = "", city = "", parent_guardian = "", emergency_number = "", note = "";
            DateTime birthdate = new DateTime();
            double height = 0, weight = 0;

            long user_id = 0, added_user = 0;
            DateTime dtNow = DateTime.Now;
            string msg = "";

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
                            case "social_id":
                                IsRequired(key, val, 1);
                                social_id = val;
                                break;
                            case "email":
                                //IsRequired(key, val, 1);
                                email = val;
                                break;
                            case "firstname":
                                IsRequired(key, val, 1);
                                firstname = val;
                                break;
                            case "lastname":
                                IsRequired(key, val, 1);
                                lastname = val;
                                break;
                            // user_type, insurance_type, device_type, device_token, user_id
                            case "password":
                                //IsRequired(key, val, 1);
                                password = val;
                                break;
                            case "user_type":
                                IsRequired(key, val, 1);
                                user_type = val; break;
                            case "insurance_id":
                                //IsRequired(key, val, 1);
                                insurance_id = val; break;
                            case "device_type": device_type = val; break;
                            case "device_token": device_token = val; break;
                            case "user_id":
                                long nVal = 0;
                                bool nTemp = long.TryParse(val, out nVal);
                                user_id = nVal;
                                break;

                            // below values will go to SOUL_ext
                            case "phone_number":
                                phone_number = val;
                                break;
                            case "address":
                                address = val;
                                break;
                            case "city": city = val; break;
                            case "dob":
                                bool isBool = DateTime.TryParse(val, out birthdate);
                                break;
                            case "parent_guardian":
                                parent_guardian = val; break;
                            case "height":
                                bool b = double.TryParse(val, out height); break;
                            case "weight":
                                bool b1 = double.TryParse(val, out weight); break;
                            case "emergency_number":
                                emergency_number = val; break;
                            case "note":
                                note = val; break;

                            default:
                                msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                                return Json(new { data = new string[] { }, message = msg, success = false });
                        }
                    }
                }

                IsRequired("social_id", social_id, 2);
                IsRequired("firstname", firstname, 2);
                IsRequired("lastname", lastname, 2);
                //IsRequired("password", password, 2);
                IsRequired("user_type", user_type, 2);

                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }

                var check_dup = dbEntity.USERs.Where(a => a.social_id == social_id && a.ref_USER_type.dname.ToLower() == user_type.ToLower());
                 if (check_dup.Count() > 0)
                {
                    msg = "User login already exist.";
                    return Json(new { data = new string[] { }, message = msg, success = false });
                }

                string salt = "";
                string encryp = "";

                if (password.Length > 0)
                {
                    salt = System.Guid.NewGuid().ToString();
                    encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + salt));
                }

                
                var u_type = dbEntity.ref_USER_type.Where(a => a.dname.ToLower() == user_type.ToLower()).FirstOrDefault();


                USER new_user = new USER();
                //required fields
                new_user.social_id = social_id;
                new_user.name_first = firstname;
                new_user.name_last = lastname;
                new_user.username = email.ToLower();
                new_user.dt_create = dt;
                new_user.create_by__USER_id = user_id; //create != null ? 0 : create.id;
                new_user.rel_ref_status_id = dbEntity.ref_status_type.Where(a => a.dname == "USER").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname == "Active").id;
                new_user.rel_ref_USER_type_id = u_type.id;
                new_user.phash = string.IsNullOrEmpty(encryp) ==true ? null : encryp;
                new_user.tlas = string.IsNullOrEmpty(salt) == true ? null : salt;
                //            // insurance_type
                //            // user_type

                dbEntity.USERs.Add(new_user);
                dbEntity.SaveChanges();

                added_user = new_user.id;

                //if (user_type.ToLower() == "patient") 
                //{
                SOUL new_soul = new SOUL();
                new_soul.name_first = firstname;
                new_soul.name_last = lastname;
                new_soul.email = email.ToLower();
                if (!string.IsNullOrEmpty(phone_number))
                {
                    new_soul.phone = phone_number;
                }

                if (!string.IsNullOrEmpty(address))
                {
                    new_soul.addr_address1 = address;
                }


                new_soul.dt_create = dt;
                new_soul.rel_ref_status_id = dbEntity.ref_status_type.Where(a => a.dname == "SOUL").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname == "Active").id;
                new_soul.create_by__USER_id = user_id;
                dbEntity.SOULs.Add(new_soul);
                dbEntity.SaveChanges();

                SOUL_ext ext_ins_type = new SOUL_ext();
                ext_ins_type.rel_SOUL_id = new_soul.id;
                ext_ins_type.attr_name = "insurance_type";
                ext_ins_type.dname = "Insurance Type";
                ext_ins_type.value = city;
                ext_ins_type.rel_ref_datatype_id = 2;
                ext_ins_type.dt_create = dt;
                ext_ins_type.create_by__USER_id = user_id;
                dbEntity.SOUL_ext.Add(ext_ins_type);
                dbEntity.SaveChanges();

                // city
                if (!string.IsNullOrEmpty(city))
                {
                    SOUL_ext ext_city = new SOUL_ext();
                    ext_city.rel_SOUL_id = new_soul.id;
                    ext_city.attr_name = "city";
                    ext_city.dname = "City";
                    ext_city.value = city;
                    ext_city.rel_ref_datatype_id = 2;
                    ext_city.dt_create = dt;
                    ext_city.create_by__USER_id = user_id;
                    dbEntity.SOUL_ext.Add(ext_city);
                    dbEntity.SaveChanges();
                }

                // date of birth
                if (birthdate > new DateTime())
                {
                    SOUL_ext ext_dob = new SOUL_ext();
                    ext_dob.rel_SOUL_id = new_soul.id;
                    ext_dob.attr_name = "dob";
                    ext_dob.dname = "DOB";
                    ext_dob.value = birthdate.ToShortDateString();
                    ext_dob.rel_ref_datatype_id = 5;
                    ext_dob.dt_create = dt;
                    ext_dob.create_by__USER_id = user_id;
                    dbEntity.SOUL_ext.Add(ext_dob);
                    dbEntity.SaveChanges();
                }


                // parent/guardian
                if (!string.IsNullOrEmpty(parent_guardian))
                {
                    SOUL_ext ext_parent = new SOUL_ext();
                    ext_parent.rel_SOUL_id = new_soul.id;
                    ext_parent.attr_name = "parent_guardian";
                    ext_parent.dname = "Parent or Guardian";
                    ext_parent.value = parent_guardian;
                    ext_parent.rel_ref_datatype_id = 2;
                    ext_parent.dt_create = dt;
                    ext_parent.create_by__USER_id = user_id;
                    dbEntity.SOUL_ext.Add(ext_parent);
                    dbEntity.SaveChanges();
                }

                if (height > 0)
                {
                    SOUL_ext ext_height = new SOUL_ext();
                    ext_height.rel_SOUL_id = new_soul.id;
                    ext_height.attr_name = "height";
                    ext_height.dname = "Height";
                    ext_height.value = height.ToString();
                    ext_height.rel_ref_datatype_id = 1;
                    ext_height.dt_create = dt;
                    ext_height.create_by__USER_id = user_id;
                    dbEntity.SOUL_ext.Add(ext_height);
                    dbEntity.SaveChanges();
                }

                if (weight > 0)
                {
                    SOUL_ext ext_weight = new SOUL_ext();
                    ext_weight.rel_SOUL_id = new_soul.id;
                    ext_weight.attr_name = "weight";
                    ext_weight.dname = "Weight";
                    ext_weight.value = weight.ToString();
                    ext_weight.rel_ref_datatype_id = 1;
                    ext_weight.dt_create = dt;
                    ext_weight.create_by__USER_id = user_id;
                    dbEntity.SOUL_ext.Add(ext_weight);
                    dbEntity.SaveChanges();
                }

                if (!string.IsNullOrEmpty(emergency_number))
                {
                    SOUL_ext ext_emerg = new SOUL_ext();
                    ext_emerg.rel_SOUL_id = new_soul.id;
                    ext_emerg.attr_name = "emergency_number";
                    ext_emerg.dname = "Emergency Number";
                    ext_emerg.value = emergency_number.ToString();
                    ext_emerg.rel_ref_datatype_id = 2;
                    ext_emerg.dt_create = dt;
                    ext_emerg.create_by__USER_id = user_id;
                    dbEntity.SOUL_ext.Add(ext_emerg);
                    dbEntity.SaveChanges();

                }

                if (!string.IsNullOrEmpty(note))
                {
                    SOUL_ext ext_note = new SOUL_ext();
                    ext_note.rel_SOUL_id = new_soul.id;
                    ext_note.attr_name = "note";
                    ext_note.dname = "Note";
                    ext_note.value = note.ToString();
                    ext_note.rel_ref_datatype_id = 2;
                    ext_note.dt_create = dt;
                    ext_note.create_by__USER_id = user_id;
                    dbEntity.SOUL_ext.Add(ext_note);
                    dbEntity.SaveChanges();
                }

                if (!string.IsNullOrEmpty(insurance_id))
                {
                    SOUL_ext ext_note = new SOUL_ext();
                    ext_note.rel_SOUL_id = new_soul.id;
                    ext_note.attr_name = "insurance_id";
                    ext_note.dname = "Insurance ID";
                    ext_note.value = insurance_id.ToString();
                    ext_note.rel_ref_datatype_id = 2;
                    ext_note.dt_create = dt;
                    ext_note.create_by__USER_id = user_id;
                    dbEntity.SOUL_ext.Add(ext_note);
                    dbEntity.SaveChanges();
                }



                //var ref_ins = dbEntity.ref_insurance_provider.Where(a => a.PayerName == "");
                //if (ref_ins != null)
                //{
                //    con_SOUL_ref_insurance soul_ref = new con_SOUL_ref_insurance();
                //    soul_ref.rel_SOUL_id = new_soul.id;
                //    soul_ref.rel_ref_insurance_provider_id = 0;
                //    dbEntity.con_SOUL_ref_insurance.Add(soul_ref);
                //    dbEntity.SaveChanges();
                //}
                //}

                List<get_User> n = new List<get_User>();
                //var ref_stat = dbEntity.ref_status_type.Where(a => a.dname == "USER").FirstOrDefault().ref_status; //.ref_status.FirstOrDefault(b => b.dname == "Active");
                var ref_stat = dbEntity.ref_status.Find(new_user.rel_ref_status_id);
                var _ext = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == new_user.id);

                n.Add(new get_User
                {
                    user_id = new_user.id,
                    first_name = new_user.name_first,
                    last_name = new_user.name_last,
                    username = new_user.username,
                    //status = ref_stat == null ? "" : ref_stat.dname,
                    social_id = string.IsNullOrEmpty(social_id) ? "" : social_id,
                    //verification_code = ""
                    
                    //dob = birthdate > new DateTime() ? birthdate.ToShortDateString() : "" ,
                    //phone = phone_number,
                    //address = address,
                    //city = city,
                    //parent = parent_guardian,
                    //height = height,
                    //weight = weight,
                    //emergency = emergency_number,
                    //note = note
                });




                var ret1 = JsonConvert.SerializeObject(n);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                return Json(new { data = json1, message = "Registration successful.", success = true });

            }
            catch (Exception ex)
            {


                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }

        }
        //public IHttpActionResult Post(string firstname, string lastname, string email, string password, string user_type,
        //    long user_id=0, string acck= "deftsoftapikey",
        //    string insurance_type="", 
        //                               string device_type = "", string device_token="")
        //{
        //    string msg = "";
        //    if (acck == "deftsoftapikey")
        //    {
        //        //// test data
        //        //var ret1 = "[{\"id\":10,\"firstname\":\"" + firstname + "\",\"lastname\":\"" + lastname+ "\",\"username\":\"" + email +"\",\"status\":\"Active\"}]";
        //        //var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
        //        //return Json(new { data = json1, message = msg, success = true });
        //        //// end test data

        //        /*  Required fields: firstname, lastname, email, password, insurance_type, user_type(doctor or patient)
        //         *  Optional fields: device_type, device_token
        //         *  user_type: 1 - patient, 2 - doctor
        //         */

        //        try {
        //            var _user = dbEntity.USERs.Find(user_id);
        //            if (_user != null)
        //            {

        //            }

        //            USER new_user = new USER();
        //            DateTime dtNow = DateTime.Now;

        //            string salt = System.Guid.NewGuid().ToString();
        //            string encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + salt));
        //            var u_type = dbEntity.ref_USER_type.Where(a => a.dname.ToLower() == user_type.ToLower()).FirstOrDefault();

        //            //required fields
        //            new_user.name_first = firstname;
        //            new_user.name_last = lastname;
        //            new_user.username = email.ToLower();
        //            new_user.dt_create = dtNow;
        //            new_user.create_by__USER_id = user_id; //create != null ? 0 : create.id;
        //            new_user.rel_ref_status_id = dbEntity.ref_status_type.Where(a => a.dname == "USER").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname == "Active").id;
        //            new_user.rel_ref_USER_type_id = u_type.id;
        //            new_user.phash = encryp;
        //            new_user.tlas = salt;
        //            // insurance_type
        //            // user_type

        //            dbEntity.USERs.Add(new_user);
        //            dbEntity.SaveChanges();


        //*** gi-transfer nko ang code
        //            if (user_type.ToLower() == "patient")
        //            {
        //                SOUL new_soul = new SOUL();
        //                new_soul.name_first = firstname;
        //                new_soul.name_last = lastname;
        //                new_soul.email = email.ToLower();
        //                new_soul.dt_create = dtNow;
        //                new_soul.rel_ref_status_id = dbEntity.ref_status_type.Where(a => a.dname == "SOUL").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname == "Active").id;
        //                new_soul.create_by__USER_id = user_id;
        //                dbEntity.SOULs.Add(new_soul);
        //                dbEntity.SaveChanges();

        //                var ref_ins = dbEntity.ref_insurance_provider.Where(a => a.PayerName=="");
        //                if (ref_ins != null)
        //                {
        //                    con_SOUL_ref_insurance soul_ref = new con_SOUL_ref_insurance();
        //                    soul_ref.rel_SOUL_id = new_soul.id;
        //                    soul_ref.rel_ref_insurance_provider_id = 0;
        //                    dbEntity.con_SOUL_ref_insurance.Add(soul_ref);
        //                    dbEntity.SaveChanges();
        //                }
        //            }


        //            List<get_User> n = new List<get_User>();
        //            n.Add(new get_User
        //            {
        //                id = new_user.id,
        //                firstname = new_user.name_first,
        //                lastname = new_user.name_last,
        //                username = new_user.username
        //            });


        //            var ret1 = JsonConvert.SerializeObject(n);
        //            var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

        //            msg = "The new user is added";
        //            //return Json(new { data= new_user.id, msg="", success= true});

        //            // var json1 = Newtonsoft.Json.Linq.JArray.Parse("{ \"id\" : " + new_user.id + "}");
        //            //msg = "[{"id":10,"firstname":"marding","lastname":"nizari","username":"mardingnizari","status":null}]";
        //            return Json(new { data = json1, message = msg, success = true });
        //        }
        //        catch(Exception ex)
        //        {
        //            return Json(new { data = "", message = ex.Message, success = false });
        //        }


        //    }

        //    msg = "The authorization header is either not valid or isn't Basic.";
        //    return Json(new { data = "", message = msg, success = false });

        // }


        // HERE using email as username

        [System.Web.Http.HttpGet]
        [Route("user")]
        public async Task<IHttpActionResult> Get(long user_id = 0)
        {
            try {
                string msg ="";
                if (true) //api.Models.progAuth.authorize()
                {
                    var get_user = from a in dbEntity.USERs select a;

                    if (user_id > 0)
                    {
                        get_user = get_user.Where(a => a.id == user_id);
                    }

                    List<user_info> get_info = new List<user_info>();

                    if (get_user.Count() > 0)
                    {
                        var _image = dbEntity.USER_ext.Where(a => a.rel_USER_id == user_id && a.attr_name == "image").FirstOrDefault();
                        var _socialid = get_user.FirstOrDefault().social_id == null ? "" : get_user.FirstOrDefault().social_id;
                        var _socialtype = dbEntity.USER_ext.Where(a => a.rel_USER_id == user_id && a.attr_name == "social_type").FirstOrDefault();

                        foreach (var i in get_user)
                        {
                            var _stat = dbEntity.ref_status.Find(i.rel_ref_status_id);

                            if(_stat.dname.ToLower() != "deleted")
                            {
                                var u_verify = dbEntity.USER_ext.Where(a => a.attr_name == "verified" && a.rel_USER_id == user_id).FirstOrDefault();
                                bool verified = false;
                                bool bVerify = u_verify == null ? false : bool.TryParse(u_verify.value, out verified);
                                //var u_veri_code = dbEntity.USER_ext.Where(a => a.attr_name == "verification_code" && a.rel_USER_id == user_id);
                                //string veri_code = u_veri_code.Count() == 0 ?"": u_veri_code.FirstOrDefault().value;

                                // GET Patient under USER_ID
                                var pat1 = dbEntity.SOULs.Where(a => a.create_by__USER_id == user_id);
                                List<user_secondary_patient> pat_list = new List<user_secondary_patient>();
                                string ins_id_string = "", ins_name = "";
                                bool user_insurance = false;
                                string p_zipcode = "", p_insuranceid = "", p_insurancename = "";
                                string phoneNo = "", street = "", city = "", state = "", dob = "", parentGuardian = "", gender = "",
                                    height = "", weight = "", emergencyNumber = "", about = "", _note="";

                                foreach (var n in pat1)
                                { // pat1 is SOUL
                                    var pat1_ext = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == n.id);
                                    foreach (var ext in pat1_ext)
                                    {
                                        switch (ext.attr_name)
                                        {
                                            case "use_user_insurance":
                                                if (ext.value == "true")
                                                    user_insurance = true;
                                                
                                                break;
                                            case "insurance_id":
                                                ins_id_string = ext.value;
                                                
                                                if (!string.IsNullOrEmpty(ins_id_string))
                                                {
                                                    long ins_id = 0;
                                                    bool bId = long.TryParse(ins_id_string, out ins_id);
                                                    if (bId)
                                                    {
                                                        var ref_ins = dbEntity.ref_insurance_provider.Find(ins_id);
                                                        ins_name = ref_ins == null ? "" : ref_ins.PayerName.Split('|')[0].Trim();
                                                    }
                                                    else
                                                    {
                                                        return Json(new { data = new string[] { }, message = "No matching insurance found!", success = false });
                                                    }
                                                }
                                                
                                                break;

                                            case "dob":
                                                dob = ext.value;
                                                break;
                                            case "weight":
                                                weight = ext.value;
                                                break;
                                            case "height":
                                                height = ext.value;
                                                break;
                                            //case "bio":
                                            //    about = ext.value;
                                            //    break;

                                            case "parent_guardian":
                                                parentGuardian = ext.value;
                                                break;
                                            case "emergency_number":
                                                emergencyNumber = ext.value;
                                                break;
                                            case "about":
                                                about = ext.value;
                                                break;
                                            case "note":
                                                _note = ext.value;
                                                break;
                                        }
                                    }

                                    if (n.is_prime.Value)
                                    {
                                        p_zipcode = n.ref_zip == null ? "" : n.ref_zip.zip;
                                        p_insuranceid = ins_id_string;
                                        p_insurancename = ins_name;

                                        phoneNo = n.phone ==null?"" : n.phone;
                                        street = n.addr_address1 + (n.addr_address2 == null ? "" : " " + n.addr_address2);
                                        city = n.ref_zip ==null?"": n.ref_zip.city_name;
                                        state = n.ref_zip == null ? "" : n.ref_zip.city_state;
                                        gender = n.gender==null ? "" : n.gender;
                                       
                                    }
                                    else
                                    {
                                   

                                        pat_list.Add(new user_secondary_patient {
                                            id = n.id,
                                            first_name = n.name_first,
                                            last_name = n.name_last,
                                            insurance_id = ins_id_string,
                                            insurance_name = ins_name   ,
                                            is_using_primary_patient_insurance = user_insurance
                                        });
                                    }
                                }

                                get_info.Add(new user_info
                                {
                                    user_id = i.id,
                                    //user_type = i.ref_USER_type.dname == null ? "" : i.ref_USER_type.dname,
                                    first_name = i.name_first == null ? "" : i.name_first,
                                    last_name = i.name_last == null ? "" : i.name_last,
                                    //status = _stat == null ? "" : _stat.dname,
                                    username = i.username == null ? "" : i.username,
                                    password ="",
                                    note = _note,
                                    verified = verified,
                                    social_id =  _socialid,
                                    social_type = _socialtype == null ? "": _socialtype.value,
                                    phone_no = phoneNo,
                                    street = street,
                                    city = city,
                                    state = state,
                                    zip_code =p_zipcode,
                                    dob= dob,
                                    parent_guardian = parentGuardian,
                                    gender= gender,
                                    height=height,
                                    weight= weight,
                                    emergency_number = emergencyNumber,
                                    about = about,
                                    insurance_id = p_insuranceid,
                                    insurance_name = p_insurancename,
                                    image_url = _image == null ? "" : "https://s3-ap-southeast-1.amazonaws.com/hsrecs/images/" + _image.value,
                                    patient = pat_list

                                    //verification_code = veri_code

                                    // add image profile
                                    //email = email // available in SOUL
                                    //insurance_type = _stat == null? "" : i.in
                                    //device_type = i.dev // available in SOUL
                                    //phone_number = i.ph // available in SOUL
                                    //public string address // available in SOUL
                                    //public string city // available in SOUL
                                    //public DateTime dob // available in SOUL
                                    //public string parent_guardian // available in SOUL
                                    //public double height // available in SOUL
                                    //public double weight // available in SOUL
                                    //public string emergency_number // available in SOUL
                                    //public string note// available in SOUL
                                });
                            }
                           
                        }

                    }
                    var ret1 = JsonConvert.SerializeObject(get_info);
                    var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                    //msg = "The patient is updated.";
                    return Json(new { data = json1, message = get_info.Count() + (get_info.Count() > 1 ? " Records found!" : " Record found!") , success = true });

                }

                msg = "The authorization header is either not valid or isn't Basic.";
                return Json(new { data = new string[] { }, message = msg, success= false });
            }
            catch (Exception ex) {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
           
         
        }

        [System.Web.Http.Route("user/patients")]
        public async Task<IHttpActionResult> Getpatient(long user_id = 0)
        {
            try
            {
               
               
                if (user_id > 0)
                {
                    var get_patient = from a in dbEntity.SOULs select a;

                    var _user = dbEntity.USERs.Find(user_id);
                    if (_user == null)
                    { return Json(new { data = new string[] { }, message = "Invalid user_id value.", success = false });  }

                    get_patient = get_patient.Where(b => b.create_by__USER_id == user_id);

                    List<user_secondary_patient> get_info = new List<user_secondary_patient>();

                    if (get_patient.Count() > 0)
                    {


                        foreach (var i in get_patient)
                        {
                            bool use_insurance = false;
                            long ins_id = 0;
                            var _image = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == i.id && a.attr_name == "image").FirstOrDefault();
                            var _stat = dbEntity.ref_status.Find(i.rel_ref_status_id);


                            if (_stat.dname.ToLower() != "deleted")
                            {
                                string ins_id_string = "", ins_name = "";
                                var p_ext = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == i.id && a.attr_name == "insurance_id");
                                var u_user = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == i.id && a.attr_name == "use_user_insurance");

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
                                    ins_id_string = p_ext.First().value;
                                    if (p_ext.Count() > 0)
                                    {
                                        bool bTemp = long.TryParse(ins_id_string, out ins_id);
                                        if (bTemp)
                                        {
                                            var ins_ref = dbEntity.ref_insurance_provider.Find(ins_id);
                                            if (ins_ref != null)
                                            {
                                                ins_name = (ins_ref.PayerName.Split('|')[0].Trim());
                                            }
                                            else
                                            {
                                                ins_name = "";
                                            }
                                        }
                                    }
                                }

                                get_info.Add(new user_secondary_patient
                                {
                                    id = i.id,
                                    // user_type = i.ref_USER_type.dname,
                                    first_name = i.name_first,
                                    last_name = i.name_last,
                                    is_prime = i.is_prime.Value,
                                    is_using_primary_patient_insurance= use_insurance,
                                    insurance_id = ins_id_string,  //p_ext.Count() == 0 ? "" : p_ext.FirstOrDefault().value,
                                    insurance_name = ins_name

                                    //email = i.email == null ? "" : i.email,  // available in SOUL
                                    //status = _stat == null ? "" : _stat.dname,
                                    //image = _image == null ? "" : "https://s3-ap-southeast-1.amazonaws.com/hsrecs/images/" + _image.value// add image profile

                                    //insurance_type = _stat == null? "" : i.in

                                    //device_type = i.dev // available in SOUL
                                    //phone_number = i.ph // available in SOUL
                                    //public string address // available in SOUL
                                    //public string city // available in SOUL
                                    //public DateTime dob // available in SOUL
                                    //public string parent_guardian // available in SOUL
                                    //public double height // available in SOUL
                                    //public double weight // available in SOUL
                                    //public string emergency_number // available in SOUL
                                    //public string note// available in SOUL
                                });
                            }

                        }

                    }
                    var ret1 = JsonConvert.SerializeObject(get_info);
                    var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                    string msg = get_info.Count() + (get_info.Count() > 1 ? " Records found!" : " Record found!");
                    return Json(new { data = json1, message = "", success = true });
                }


                return Json(new { data = new string[] { }, message = "Invalid user_id value.", success = false });

            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }


        }


        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("wr/user")]
        public IHttpActionResult wrupdateUser([FromBody] u_user user1)
        {

            long user_id=0;
            string first_name = "", last_name = "",  email = "", password = "", state = "", status = "", gender="";

            // save to USER table
            string phone_no = "", street = "", city = "", parent_guardian = "", emergency_number = "", note = "",
                 zip_code="", about ="";
            string birthdate = "";
            double height = 0, weight = 0;
            long insurance_id = 0;

            //DateTime dtNow = DateTime.Now;
            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);
            string msg = "";

            try
            {
               // await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        // optional parameter: name, email, password, phone, address, city, 
                        // optional parameter: state, dob, parent_guardian, gender, height, weight, emergency_number, note
                        switch (key)
                        {
                            case "user_id":
                                IsRequired(key, val, 1);
                                long nVal = 0;
                                bool nTemp = long.TryParse(val, out nVal);
                                user_id = nVal;
                                break;
                            //case "email": email = val; break;
                            case "password": password = val; break;

                            case "first_name": first_name = val; break;
                            case "last_name": last_name = val; break;
                            // user_type, insurance_type, device_type, device_token, user_id
                            //case "user_type": user_type = val; break;
                            //case "insurance_type": insurance_type = val; break;
                            //case "device_type": device_type = val; break;
                            //case "device_token": device_token = val; break;
                            
                            case "status":
                                status = val;
                                break;

                            // below values will go to SOUL_ext
                            case "phone_no":
                                phone_no = val;
                                break;
                            case "street":
                                street = val;
                                break;
                            case "city": city = val; break;
                            case "state": state = val; break;

                            case "dob":
                                //bool isBool = DateTime.TryParse(val, out birthdate);
                                birthdate = val;
                                break;

                            //case "zipcode":
                            //    zipcode = val;
                            //    break;

                            case "gender":
                                gender = val; break;
                            case "parent_guardian":
                                parent_guardian = val; break;

                            case "insurance_id":
                                //insurance_id = val;
                                bool bIns = long.TryParse(val, out insurance_id);
                                if (!bIns)
                                {
                                    return Json(new { data = new string[] { }, message = "Invalid insurance_id value.", success = false });
                                }
                                break;

                            case "zip_code":
                                zip_code = val; break;
                            case "note":
                                note = val; break;
                            case "about":
                                about = val; break;

                            case "height":
                                bool b = double.TryParse(val, out height); break;
                            case "weight":
                                bool b1 = double.TryParse(val, out weight); break;
                            case "emergency_number":
                                emergency_number = val; break;
                            //case "note":
                            //    note = val; break;
                            default:
                                msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                                return Json(new { data = new string[] { }, message = msg, success = false });
                        }
                    }
                }

                // variable declaration to unify the functionality below this code
                u_user user2 = new u_user
                {
                    user_id = user_id,
                    first_name = first_name,
                    last_name = last_name,
                    email = email,
                    password = password,
                    state = state,
                    status = status,
                    gender = gender,
                    dob = birthdate,
                    phone_no = phone_no,
                    street = street,
                    city = city,
                  
                    parent_guardian = parent_guardian,
                    emergency_number = emergency_number,
                    note = note,
                    height = height,
                    weight = weight,
                    zip_code = zip_code,
                    insurance_id = insurance_id,
                    about =about
                };

               return _updatingUser(user1);

                //    IsRequired("user_id", user_id.ToString(), 2);
                //    if (haserror)
                //    {
                //        return Json(new { data = new string[] { }, message = errmsg, success = false });
                //    }



                //    var _user = dbEntity.USERs.Find(user_id);
                //    if (_user == null)
                //    {
                //        msg = "Invalid user_id value.";
                //        return Json(new { data = new string[] { }, message = msg, success = false });
                //    }

                //    // we need check if EMAIL already existed
                //    if (!string.IsNullOrEmpty(email))
                //    {

                //        var user = dbEntity.USERs.Where(a => a.username == email.ToLower());
                //        if (user.Count() > 0) {
                //            //user = user.Where(a => a.id == user_id);
                //            //if (user.Count() > 0)
                //            //{  }
                //            //else
                //            //{
                //                msg = "Email already existed.";
                //                return Json(new { data = new string[] { }, message = msg, success = false });
                //            //}
                //        }
                //    }


                //    if (_user != null)
                //    {
                //        //USER _user = new USER();

                //        //required fields
                //        if (!string.IsNullOrEmpty(first_name))
                //        {
                //            _user.name_first = first_name;
                //        }

                //        if (!string.IsNullOrEmpty(last_name))
                //        {
                //            _user.name_last = last_name;
                //        }

                //        var old_email = _user.username; 
                //        if (!string.IsNullOrEmpty(email))
                //        { _user.username = email.ToLower(); }

                //        if (!string.IsNullOrEmpty(status))
                //        { 
                //            _user.rel_ref_status_id = dbEntity.ref_status_type.Where(a => a.dname == "USER").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname.ToLower() == status.ToLower()).id;
                //            //_user.rel_ref_USER_type_id = u_type.id;
                //        }



                //        if (!string.IsNullOrEmpty(password))
                //        {
                //            string salt = System.Guid.NewGuid().ToString();
                //            string encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + salt));
                //            //var u_type = dbEntity.ref_USER_type.Where(a => a.dname.ToLower() == user_type.ToLower()).FirstOrDefault();
                //            _user.phash = encryp;
                //            _user.tlas = salt;
                //        }

                //        if (!string.IsNullOrEmpty(first_name) || !string.IsNullOrEmpty(last_name) || !string.IsNullOrEmpty(email) 
                //            || !string.IsNullOrEmpty(status) || !string.IsNullOrEmpty(password)) {
                //            // insurance_type
                //            // user_type
                //            _user.dt_update = dt;
                //            _user.update_by__USER_id = user_id;
                //            dbEntity.Entry(_user).State = System.Data.Entity.EntityState.Modified;
                //            dbEntity.SaveChanges();
                //        }


                //        string user_type = dbEntity.ref_USER_type.Find(_user.rel_ref_USER_type_id).dname.ToLower();
                //        if (true) //user_type == "patient"
                //        {
                //            // ****
                //            // SAVE TO table SOUL
                //            var _soul = from a in dbEntity.SOULs select a;
                //            if (!string.IsNullOrEmpty(old_email))
                //            {
                //                _soul = _soul.Where(a => a.email==old_email); // _soul.Where(a => a.email == old_email).FirstOrDefault(); 
                //            }
                //            else //if no email address we need the SOCIAL_ID as the reference
                //            {
                //                var user_ext = dbEntity.USERs.Find(user_id);
                //                var s_ext = dbEntity.SOUL_ext.Where(a => a.attr_name =="social_id" && a.value== user_ext.social_id).FirstOrDefault();
                //                _soul = _soul.Where(a => a.id ==s_ext.rel_SOUL_id);
                //            }

                //            //if (!string.IsNullOrEmpty(zipcode))
                //            //{
                //            //    var ref_zip = dbEntity.ref_zip.Where(a => a.zip == zipcode).FirstOrDefault();
                //            //    _soul.FirstOrDefault().addr_rel_ref_zip_id = ref_zip.id;
                //            //}

                //            if (!string.IsNullOrEmpty(first_name))
                //                _soul.FirstOrDefault().name_first = first_name;
                //            if (!string.IsNullOrEmpty(last_name))
                //                _soul.FirstOrDefault().name_last = last_name;
                //            if (!string.IsNullOrEmpty(email))
                //                _soul.FirstOrDefault().email = email.ToLower();
                //            if (!string.IsNullOrEmpty(phone_no))
                //                _soul.FirstOrDefault().phone = phone_no;

                //            if (!string.IsNullOrEmpty(gender))
                //            {
                //                _soul.FirstOrDefault().gender = gender.Substring(0, 1);
                //            }

                //            if (!string.IsNullOrEmpty(zip_code)) {
                //                var z = dbEntity.ref_zip.Where( a => a.zip == zip_code).FirstOrDefault();

                //                if (z != null)
                //                {
                //                    _soul.FirstOrDefault().addr_rel_ref_zip_id = z.id;
                //                }
                //                else
                //                {
                //                    return Json(new { data = new string[] { }, message = "zip_code not found.", success = false });
                //                }

                //            }

                //            if (!string.IsNullOrEmpty(street))
                //                _soul.FirstOrDefault().addr_address1 = street;

                //            if (!string.IsNullOrEmpty(zip_code)||  
                //                !string.IsNullOrEmpty(gender) || 
                //                !string.IsNullOrEmpty(first_name) ||
                //                !string.IsNullOrEmpty(last_name) ||
                //                !string.IsNullOrEmpty(email) || 
                //                !string.IsNullOrEmpty(phone_no) ||
                //                !string.IsNullOrEmpty(street)) {
                //                _soul.FirstOrDefault().dt_update = dt;
                //                //_soul.rel_ref_status_id = dbEntity.ref_status_type.Where(a => a.dname == "USER").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname == "Active").id;
                //                _soul.FirstOrDefault().update_by__USER_id = user_id;
                //                //dbEntity.SOULs.Add(_soul);
                //                dbEntity.Entry(_soul.FirstOrDefault()).State = System.Data.Entity.EntityState.Modified;
                //                dbEntity.SaveChanges();
                //            }


                //            //  ***
                //            //  update TO table SOUL_ext
                //            long _soul_id = _soul.FirstOrDefault().id;
                //            var soul_ext = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == _soul_id);
                //            bool hasInsurance = false, hasPhone = false, hasAddr = false, hasCity = false, hasState= false, hasDOB = false, hasParent = false, 
                //                hasHeight = false, hasWeight = false, hasEmergency = false, hasNote = false, hasAbout=false;
                //            if (soul_ext.Count() > 0)
                //            {
                //                foreach (var i in soul_ext)
                //                {
                //                    switch (i.attr_name)
                //                    {

                //                        case "city":
                //                            #region "city"
                //                            if (!string.IsNullOrEmpty(city))
                //                            {
                //                                hasCity = true;

                //                               //var ext_city = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "city").FirstOrDefault();
                //                                i.value = city;

                //                                i.dt_update = dt;
                //                                i.update_by__USER_id = user_id;
                //                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                //                                //dbEntity.SaveChanges();
                //                            }
                //                            break;
                //                        #endregion

                //                        case "state":
                //                            #region "state"
                //                            if (!string.IsNullOrEmpty(state))
                //                            {
                //                                hasState = true;

                //                                //SOUL_ext ext_city = new SOUL_ext();
                //                                //var ext_state = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "state").FirstOrDefault();
                //                                i.value = city;

                //                                i.dt_update = dt;
                //                                i.update_by__USER_id = user_id;
                //                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                //                                //dbEntity.SaveChanges();
                //                            }
                //                            break;
                //                        #endregion

                //                        case "dob":

                //                            #region "dob"
                //                            if (birthdate > new DateTime())
                //                            {
                //                                hasDOB = true;

                //                                //var ext_dob = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "dob").FirstOrDefault();
                //                                i.value = birthdate.ToShortDateString();
                //                                i.dt_update = dt;
                //                                i.update_by__USER_id = user_id;
                //                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                //                                //dbEntity.SaveChanges();
                //                            }
                //                            break;

                //                        #endregion

                //                        case "parent_guardian":

                //                            #region "parent_guardian"
                //                            if (!string.IsNullOrEmpty(parent_guardian))
                //                            {
                //                                hasParent = true;

                //                                //SOUL_ext ext_parent = new SOUL_ext();
                //                                //var ext_parent = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "parent_guardian").FirstOrDefault();
                //                                i.value = parent_guardian;

                //                                i.dt_update = dt;
                //                                i.update_by__USER_id = user_id;
                //                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                //                                //dbEntity.SaveChanges();
                //                            }
                //                            break;
                //                        #endregion


                //                        case "insurance_id":

                //                            #region "insurance_id"
                //                            if (!string.IsNullOrEmpty(insurance_id))
                //                            {
                //                                hasInsurance = true;

                //                                //SOUL_ext ext_parent = new SOUL_ext();
                //                                //var ext_parent = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "parent_guardian").FirstOrDefault();
                //                                i.value = insurance_id;

                //                                i.dt_update = dt;
                //                                i.update_by__USER_id = user_id;
                //                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                //                                //dbEntity.SaveChanges();
                //                            }
                //                            break;
                //                        #endregion

                //                        case "height":

                //                            #region "height"
                //                            if (height > 0)
                //                            {
                //                                hasHeight = true;

                //                                //SOUL_ext ext_height = new SOUL_ext();
                //                                //var ext_height = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "height").FirstOrDefault();
                //                                i.value = height.ToString();

                //                                i.dt_update = dt;
                //                                i.update_by__USER_id = user_id;
                //                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                //                                //dbEntity.SaveChanges();
                //                            }
                //                            break;
                //                        #endregion


                //                        case "weight":

                //                            #region "weight"
                //                            if (weight > 0)
                //                            {
                //                                hasWeight = true;

                //                                // var ext_weight = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "weight").FirstOrDefault();
                //                                i.value = weight.ToString();

                //                                i.dt_update = dt;
                //                                i.update_by__USER_id = user_id;
                //                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                //                                //dbEntity.SaveChanges();
                //                            }
                //                            break;
                //                        #endregion


                //                        case "emergency_number":

                //                            #region "emergency_number"
                //                            if (!string.IsNullOrEmpty(emergency_number))
                //                            {
                //                                hasEmergency = true;

                //                                //var ext_emerg = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "emergency_number").FirstOrDefault();
                //                                i.value = emergency_number.ToString();

                //                                i.dt_update = dt;
                //                                i.update_by__USER_id = user_id;
                //                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                //                                //dbEntity.SaveChanges();

                //                            }
                //                            break;
                //                        #endregion


                //                        case "note":
                //                            #region "note"
                //                            if (!string.IsNullOrEmpty(note))
                //                            {
                //                                hasNote = true;

                //                                //var ext_note = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "note").FirstOrDefault();
                //                                i.value = note.ToString();

                //                                i.dt_update = dt;
                //                                i.update_by__USER_id = user_id;
                //                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                //                                //dbEntity.SaveChanges();
                //                            }
                //                            break;
                //                        #endregion

                //                        case "about":
                //                            #region "about"
                //                            if (!string.IsNullOrEmpty(about))
                //                            {
                //                                hasAbout = true;

                //                                //var ext_note = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "note").FirstOrDefault();
                //                                i.value = about;

                //                                i.dt_update = dt;
                //                                i.update_by__USER_id = user_id;
                //                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                //                                //dbEntity.SaveChanges();
                //                            }
                //                            break;
                //                            #endregion

                //                    }


                //                }

                //                dbEntity.SaveChanges();

                //            }

                //            // city
                //            #region "city"
                //            if (!hasCity)
                //            {
                //                if (!string.IsNullOrEmpty(city))
                //                {
                //                    SOUL_ext ext_city = new SOUL_ext();
                //                    ext_city.rel_SOUL_id = _soul_id;
                //                    ext_city.attr_name = "city";
                //                    ext_city.dname = "City";
                //                    ext_city.value = city;
                //                    ext_city.rel_ref_datatype_id = 2;
                //                    ext_city.create_by__USER_id = user_id;
                //                    ext_city.dt_create = dt;
                //                    dbEntity.SOUL_ext.Add(ext_city);
                //                    dbEntity.SaveChanges();
                //                }
                //            }
                //            #endregion

                //            // state
                //            #region "state"
                //            if (!hasState)
                //            {
                //                if (!string.IsNullOrEmpty(state))
                //                {
                //                    SOUL_ext ext_state = new SOUL_ext();
                //                    ext_state.rel_SOUL_id = _soul_id;
                //                    ext_state.attr_name = "state";
                //                    ext_state.dname = "state";
                //                    ext_state.value = state;
                //                    ext_state.rel_ref_datatype_id = 2;
                //                    ext_state.create_by__USER_id = user_id;
                //                    ext_state.dt_create = dt;
                //                    dbEntity.SOUL_ext.Add(ext_state);
                //                    dbEntity.SaveChanges();
                //                }
                //            }
                //            #endregion

                //            // date of birth
                //            #region "date of birth"
                //            if (!hasDOB)
                //            {
                //                if (birthdate > new DateTime())
                //                {
                //                    SOUL_ext ext_dob = new SOUL_ext();
                //                    ext_dob.rel_SOUL_id = _soul_id;
                //                    ext_dob.attr_name = "dob";
                //                    ext_dob.dname = "DOB";
                //                    ext_dob.value = birthdate.ToShortDateString();
                //                    ext_dob.rel_ref_datatype_id = 2;
                //                    ext_dob.create_by__USER_id = user_id;
                //                    ext_dob.dt_create = dt;
                //                    dbEntity.SOUL_ext.Add(ext_dob);
                //                    dbEntity.SaveChanges();
                //                }
                //            }
                //            #endregion

                //            // parent/guardian
                //            if (!hasParent) {
                //                if (!string.IsNullOrEmpty(parent_guardian))
                //                {
                //                    SOUL_ext ext_parent = new SOUL_ext();
                //                    ext_parent.rel_SOUL_id = _soul_id;
                //                    ext_parent.attr_name = "parent_guardian";
                //                    ext_parent.dname = "Parent or Guardian";
                //                    ext_parent.value = parent_guardian;
                //                    ext_parent.rel_ref_datatype_id = 2;
                //                    ext_parent.create_by__USER_id = user_id;
                //                    ext_parent.dt_create = dt;
                //                    dbEntity.SOUL_ext.Add(ext_parent);
                //                    dbEntity.SaveChanges();
                //                }
                //            }

                //            // insurance_id
                //            if (!hasInsurance)
                //            {
                //                if (!string.IsNullOrEmpty(insurance_id))
                //                {
                //                    SOUL_ext ext_parent = new SOUL_ext();
                //                    ext_parent.rel_SOUL_id = _soul_id;
                //                    ext_parent.attr_name = "insurance_id";
                //                    ext_parent.dname = "Insurance ID";
                //                    ext_parent.value = insurance_id;
                //                    ext_parent.rel_ref_datatype_id = 2;
                //                    ext_parent.create_by__USER_id = user_id;
                //                    ext_parent.dt_create = dt;
                //                    dbEntity.SOUL_ext.Add(ext_parent);
                //                    dbEntity.SaveChanges();
                //                }
                //            }

                //            // height
                //            if (!hasHeight) {
                //                if (height > 0)
                //                {
                //                    SOUL_ext ext_height = new SOUL_ext();
                //                    ext_height.rel_SOUL_id = _soul_id;
                //                    ext_height.attr_name = "height";
                //                    ext_height.dname = "Height";
                //                    ext_height.value = height.ToString();
                //                    ext_height.rel_ref_datatype_id = 1;
                //                    ext_height.create_by__USER_id = user_id;
                //                    ext_height.dt_create = dt;
                //                    dbEntity.SOUL_ext.Add(ext_height);
                //                    dbEntity.SaveChanges();
                //                }
                //            }


                //            // weight
                //            if (!hasWeight) {
                //                if (weight > 0)
                //                {
                //                    SOUL_ext ext_weight = new SOUL_ext();
                //                    ext_weight.rel_SOUL_id = _soul_id;
                //                    ext_weight.attr_name = "weight";
                //                    ext_weight.dname = "Weight";
                //                    ext_weight.value = weight.ToString();
                //                    ext_weight.rel_ref_datatype_id = 1;
                //                    ext_weight.create_by__USER_id = user_id;
                //                    ext_weight.dt_create = dt;
                //                    dbEntity.SOUL_ext.Add(ext_weight);
                //                    dbEntity.SaveChanges();
                //                }
                //            }


                //            // emergency
                //            if (!hasEmergency) {
                //                if (!string.IsNullOrEmpty(emergency_number))
                //                {
                //                    SOUL_ext ext_emerg = new SOUL_ext();
                //                    ext_emerg.rel_SOUL_id = _soul_id;
                //                    ext_emerg.attr_name = "emergency_number";
                //                    ext_emerg.dname = "Emergency Number";
                //                    ext_emerg.value = emergency_number.ToString();
                //                    ext_emerg.rel_ref_datatype_id = 2;
                //                    ext_emerg.create_by__USER_id = user_id;
                //                    ext_emerg.dt_create = dt;
                //                    dbEntity.SOUL_ext.Add(ext_emerg);
                //                    dbEntity.SaveChanges();

                //                }
                //            }


                //            // Note
                //            if (!hasNote) {
                //                if (!string.IsNullOrEmpty(note))
                //                {
                //                    SOUL_ext ext_note = new SOUL_ext();
                //                    ext_note.rel_SOUL_id = _soul_id;
                //                    ext_note.attr_name = "note";
                //                    ext_note.dname = "Note";
                //                    ext_note.value = note.ToString();
                //                    ext_note.rel_ref_datatype_id = 2;
                //                    ext_note.create_by__USER_id = user_id;
                //                    ext_note.dt_create = dt;
                //                    dbEntity.SOUL_ext.Add(ext_note);
                //                    dbEntity.SaveChanges();
                //                }
                //            }

                //            // About
                //            if (!hasAbout)
                //            {
                //                if (!string.IsNullOrEmpty(about))
                //                {
                //                    SOUL_ext ext_note = new SOUL_ext();
                //                    ext_note.rel_SOUL_id = _soul_id;
                //                    ext_note.attr_name = "about";
                //                    ext_note.dname = "About";
                //                    ext_note.value = about.ToString();
                //                    ext_note.rel_ref_datatype_id = 2;
                //                    ext_note.create_by__USER_id = user_id;
                //                    ext_note.dt_create = dt;
                //                    dbEntity.SOUL_ext.Add(ext_note);
                //                    dbEntity.SaveChanges();
                //                }
                //            }

                //        }


                //        return Json(new { data = new string[] { }, message = "Update USER record successfully.", success = true });
                //    }

                //    // if user is not FOUND
                //    msg = "No result found.";
                //    return Json(new { data = new string[] { }, message = msg, success = false });
            }
            catch (Exception ex)
            {

                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }

        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("user")]
        public IHttpActionResult putupdateUser([FromBody]u_user user1)
        //public  IHttpActionResult updateUser(u_user user1)
        {
            // Content-Type:application/x-www-form-urlencoded
            return _updatingUser(user1);

            //long user_id = user1.user_id == 0 ? 0 : user1.user_id;
            //string firstname = user1.first_name == null ? "" : user1.first_name,
            //    lastname = user1.last_name == null ? "" : user1.last_name,
            //    email = user1.email == null ? "" : user1.email,
            //    password = user1.password == null ? "" : user1.password,
            //    state = user1.state == null ? "" : user1.state,
            //    status = user1.status == null ? "" : user1.status,
            //    gender = user1.gender == null ? "" : user1.gender,
            //    insurance_id = user1.insurance_id ==null?"": user1.insurance_id,
            //    about = user1.about ==null?"": user1.about;

            //// save to SOUL table
            //string    phone_number =user1.phone_no == null?"": user1.phone_no,
            //    street = user1.street ==null?"": user1.street,
            //    city = user1.city ==null?"": user1.city,
            //    parent_guardian = user1.parent_guardian==null?"": user1.parent_guardian,
            //    emergency_number = user1.emergency_number == null?"": user1.emergency_number,
            //    note = user1.note ==null?"": user1.note;

            //DateTime birthdate = new DateTime();
            //double height = user1.height, weight = user1.weight;

            //////DateTime dtNow = DateTime.Now;
            ////string root = HttpContext.Current.Server.MapPath("~/Temp");
            ////var provider = new MultipartFormDataStreamProvider(root);
            //string msg = "";




            //try
            //{

            //    IsRequired("user_id", (user_id == 0 ? "" : user_id.ToString()), 2);
            //    if (haserror)
            //    {
            //        return Json(new { data = new string[] { }, message = errmsg, success = false });
            //    }



            //    var _user = dbEntity.USERs.Find(user_id);
            //    if (_user == null)
            //    {
            //        msg = "Invalid user_id value.";
            //        return Json(new { data = new string[] { }, message = msg, success = false });
            //    }

            //    // we need check if EMAIL already existed
            //    if (!string.IsNullOrEmpty(email))
            //    {

            //        var user = dbEntity.USERs.Where(a => a.username == email.ToLower());
            //        if (user.Count() > 0)
            //        {
            //            //user = user.Where(a => a.id == user_id);
            //            //if (user.Count() > 0)
            //            //{  }
            //            //else
            //            //{
            //            msg = "Email already existed.";
            //            return Json(new { data = new string[] { }, message = msg, success = false });
            //            //}
            //        }
            //    }


            //    if (_user != null)
            //    {
            //        //USER _user = new USER();

            //        //required fields
            //        if (!string.IsNullOrEmpty(firstname))
            //        {
            //            _user.name_first = firstname;
            //        }

            //        if (!string.IsNullOrEmpty(lastname))
            //        {
            //            _user.name_last = lastname;
            //        }

            //        var old_email = _user.username;
            //        if (!string.IsNullOrEmpty(email))
            //        { _user.username = email.ToLower(); }

            //        if (!string.IsNullOrEmpty(status))
            //        {
            //            _user.rel_ref_status_id = dbEntity.ref_status_type.Where(a => a.dname == "USER").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname.ToLower() == status.ToLower()).id;
            //            //_user.rel_ref_USER_type_id = u_type.id;
            //        }

            //        if (!string.IsNullOrEmpty(password))
            //        {
            //            string salt = System.Guid.NewGuid().ToString();
            //            string encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + salt));
            //            //var u_type = dbEntity.ref_USER_type.Where(a => a.dname.ToLower() == user_type.ToLower()).FirstOrDefault();
            //            _user.phash = encryp;
            //            _user.tlas = salt;
            //        }

            //        if (!string.IsNullOrEmpty(firstname) || !string.IsNullOrEmpty(lastname) || !string.IsNullOrEmpty(email)
            //            || !string.IsNullOrEmpty(status) || !string.IsNullOrEmpty(password))
            //        {
            //            // insurance_type
            //            // user_type
            //            _user.dt_update = dt;
            //            _user.update_by__USER_id = user_id;
            //            dbEntity.Entry(_user).State = System.Data.Entity.EntityState.Modified;
            //            dbEntity.SaveChanges();
            //        }


            //        string user_type = dbEntity.ref_USER_type.Find(_user.rel_ref_USER_type_id).dname.ToLower();
            //        if (true) //user_type == "patient"
            //        {
            //            // ****
            //            // SAVE TO table SOUL
            //            var _soul = from a in dbEntity.SOULs select a;
            //            if (!string.IsNullOrEmpty(old_email))
            //            {
            //                _soul = _soul.Where(a => a.email == old_email); // _soul.Where(a => a.email == old_email).FirstOrDefault(); 
            //            }
            //            else //if no email address we need the SOCIAL_ID as the reference
            //            {
            //                var user_ext = dbEntity.USERs.Find(user_id);
            //                var s_ext = dbEntity.SOUL_ext.Where(a => a.attr_name == "social_id" && a.value == user_ext.social_id).FirstOrDefault();
            //                _soul = _soul.Where(a => a.id == s_ext.rel_SOUL_id);
            //            }

            //            if (!string.IsNullOrEmpty(firstname))
            //                _soul.FirstOrDefault().name_first = firstname;
            //            if (!string.IsNullOrEmpty(lastname))
            //                _soul.FirstOrDefault().name_last = lastname;
            //            if (!string.IsNullOrEmpty(email))
            //                _soul.FirstOrDefault().email = email.ToLower();
            //            if (!string.IsNullOrEmpty(phone_number))
            //                _soul.FirstOrDefault().phone = phone_number;


            //            if (!string.IsNullOrEmpty(street))
            //                _soul.FirstOrDefault().addr_address1 = street;

            //            if (!string.IsNullOrEmpty(firstname) || !string.IsNullOrEmpty(lastname) ||
            //                !string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(phone_number) ||
            //                !string.IsNullOrEmpty(street))
            //            {
            //                _soul.FirstOrDefault().dt_update = dt;
            //                //_soul.rel_ref_status_id = dbEntity.ref_status_type.Where(a => a.dname == "USER").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname == "Active").id;
            //                _soul.FirstOrDefault().update_by__USER_id = user_id;
            //                //dbEntity.SOULs.Add(_soul);
            //                dbEntity.Entry(_soul.FirstOrDefault()).State = System.Data.Entity.EntityState.Modified;
            //                dbEntity.SaveChanges();
            //            }


            //            //  ***
            //            //  update TO table SOUL_ext
            //            long _soul_id = _soul.FirstOrDefault().id;
            //            var soul_ext = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == _soul_id);
            //            bool hasPhone = false, hasAddr = false, hasCity = false, hasState = false, hasDOB = false, hasParent = false,
            //                hasHeight = false, hasWeight = false, hasEmergency = false, hasNote = false;
            //            if (soul_ext.Count() > 0)
            //            {
            //                foreach (var i in soul_ext)
            //                {
            //                    switch (i.attr_name)
            //                    {

            //                        case "city":
            //                            #region "city"
            //                            if (!string.IsNullOrEmpty(city))
            //                            {
            //                                hasCity = true;

            //                                //var ext_city = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "city").FirstOrDefault();
            //                                i.value = city;

            //                                i.dt_update = dt;
            //                                i.update_by__USER_id = user_id;
            //                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
            //                                //dbEntity.SaveChanges();
            //                            }
            //                            break;
            //                        #endregion

            //                        case "state":
            //                            #region "state"
            //                            if (!string.IsNullOrEmpty(state))
            //                            {
            //                                hasState = true;

            //                                //SOUL_ext ext_city = new SOUL_ext();
            //                                //var ext_state = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "state").FirstOrDefault();
            //                                i.value = city;

            //                                i.dt_update = dt;
            //                                i.update_by__USER_id = user_id;
            //                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
            //                                //dbEntity.SaveChanges();
            //                            }
            //                            break;
            //                        #endregion

            //                        case "dob":

            //                            #region "dob"
            //                            if (birthdate > new DateTime())
            //                            {
            //                                hasDOB = true;

            //                                //var ext_dob = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "dob").FirstOrDefault();
            //                                i.value = birthdate.ToShortDateString();
            //                                i.dt_update = dt;
            //                                i.update_by__USER_id = user_id;
            //                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
            //                                //dbEntity.SaveChanges();
            //                            }
            //                            break;

            //                        #endregion

            //                        case "parent_guardian":

            //                            #region "parent_guardian"
            //                            if (!string.IsNullOrEmpty(parent_guardian))
            //                            {
            //                                hasParent = true;

            //                                //SOUL_ext ext_parent = new SOUL_ext();
            //                                //var ext_parent = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "parent_guardian").FirstOrDefault();
            //                                i.value = parent_guardian;

            //                                i.dt_update = dt;
            //                                i.update_by__USER_id = user_id;
            //                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
            //                                //dbEntity.SaveChanges();
            //                            }
            //                            break;
            //                        #endregion


            //                        case "height":

            //                            #region "height"
            //                            if (height > 0)
            //                            {
            //                                hasHeight = true;

            //                                //SOUL_ext ext_height = new SOUL_ext();
            //                                //var ext_height = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "height").FirstOrDefault();
            //                                i.value = height.ToString();

            //                                i.dt_update = dt;
            //                                i.update_by__USER_id = user_id;
            //                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
            //                                //dbEntity.SaveChanges();
            //                            }
            //                            break;
            //                        #endregion


            //                        case "weight":

            //                            #region "weight"
            //                            if (weight > 0)
            //                            {
            //                                hasWeight = true;

            //                                // var ext_weight = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "weight").FirstOrDefault();
            //                                i.value = weight.ToString();

            //                                i.dt_update = dt;
            //                                i.update_by__USER_id = user_id;
            //                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
            //                                //dbEntity.SaveChanges();
            //                            }
            //                            break;
            //                        #endregion


            //                        case "emergency_number":

            //                            #region "emergency_number"
            //                            if (!string.IsNullOrEmpty(emergency_number))
            //                            {
            //                                hasEmergency = true;

            //                                //var ext_emerg = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "emergency_number").FirstOrDefault();
            //                                i.value = emergency_number.ToString();

            //                                i.dt_update = dt;
            //                                i.update_by__USER_id = user_id;
            //                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
            //                                //dbEntity.SaveChanges();

            //                            }
            //                            break;
            //                        #endregion


            //                        case "note":

            //                            #region "note"
            //                            if (!string.IsNullOrEmpty(note))
            //                            {
            //                                hasNote = true;

            //                                //var ext_note = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "note").FirstOrDefault();
            //                                i.value = note.ToString();

            //                                i.dt_update = dt;
            //                                i.update_by__USER_id = user_id;
            //                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
            //                                //dbEntity.SaveChanges();
            //                            }
            //                            break;
            //                            #endregion

            //                    }


            //                }

            //                dbEntity.SaveChanges();

            //            }

            //            // city
            //            if (!hasCity)
            //            {
            //                if (!string.IsNullOrEmpty(city))
            //                {
            //                    SOUL_ext ext_city = new SOUL_ext();
            //                    ext_city.rel_SOUL_id = _soul_id;
            //                    ext_city.attr_name = "city";
            //                    ext_city.dname = "City";
            //                    ext_city.value = city;
            //                    ext_city.rel_ref_datatype_id = 2;
            //                    ext_city.create_by__USER_id = user_id;
            //                    ext_city.dt_create = dt;
            //                    dbEntity.SOUL_ext.Add(ext_city);
            //                    dbEntity.SaveChanges();
            //                }
            //            }

            //            // state
            //            if (!hasState)
            //            {
            //                if (!string.IsNullOrEmpty(state))
            //                {
            //                    SOUL_ext ext_state = new SOUL_ext();
            //                    ext_state.rel_SOUL_id = _soul_id;
            //                    ext_state.attr_name = "state";
            //                    ext_state.dname = "state";
            //                    ext_state.value = state;
            //                    ext_state.rel_ref_datatype_id = 2;
            //                    ext_state.create_by__USER_id = user_id;
            //                    ext_state.dt_create = dt;
            //                    dbEntity.SOUL_ext.Add(ext_state);
            //                    dbEntity.SaveChanges();
            //                }
            //            }

            //            // date of birth
            //            if (!hasDOB)
            //            {
            //                if (birthdate > new DateTime())
            //                {
            //                    SOUL_ext ext_dob = new SOUL_ext();
            //                    ext_dob.rel_SOUL_id = _soul_id;
            //                    ext_dob.attr_name = "dob";
            //                    ext_dob.dname = "DOB";
            //                    ext_dob.value = birthdate.ToShortDateString();
            //                    ext_dob.rel_ref_datatype_id = 2;
            //                    ext_dob.create_by__USER_id = user_id;
            //                    ext_dob.dt_create = dt;
            //                    dbEntity.SOUL_ext.Add(ext_dob);
            //                    dbEntity.SaveChanges();
            //                }
            //            }

            //            // parent/guardian
            //            if (!hasParent)
            //            {
            //                if (!string.IsNullOrEmpty(parent_guardian))
            //                {
            //                    SOUL_ext ext_parent = new SOUL_ext();
            //                    ext_parent.rel_SOUL_id = _soul_id;
            //                    ext_parent.attr_name = "parent_guardian";
            //                    ext_parent.dname = "Parent or Guardian";
            //                    ext_parent.value = parent_guardian;
            //                    ext_parent.rel_ref_datatype_id = 2;
            //                    ext_parent.create_by__USER_id = user_id;
            //                    ext_parent.dt_create = dt;
            //                    dbEntity.SOUL_ext.Add(ext_parent);
            //                    dbEntity.SaveChanges();
            //                }
            //            }

            //            // height
            //            if (!hasHeight)
            //            {
            //                if (height > 0)
            //                {
            //                    SOUL_ext ext_height = new SOUL_ext();
            //                    ext_height.rel_SOUL_id = _soul_id;
            //                    ext_height.attr_name = "height";
            //                    ext_height.dname = "Height";
            //                    ext_height.value = height.ToString();
            //                    ext_height.rel_ref_datatype_id = 1;
            //                    ext_height.create_by__USER_id = user_id;
            //                    ext_height.dt_create = dt;
            //                    dbEntity.SOUL_ext.Add(ext_height);
            //                    dbEntity.SaveChanges();
            //                }
            //            }


            //            // weight
            //            if (!hasWeight)
            //            {
            //                if (weight > 0)
            //                {
            //                    SOUL_ext ext_weight = new SOUL_ext();
            //                    ext_weight.rel_SOUL_id = _soul_id;
            //                    ext_weight.attr_name = "weight";
            //                    ext_weight.dname = "Weight";
            //                    ext_weight.value = weight.ToString();
            //                    ext_weight.rel_ref_datatype_id = 1;
            //                    ext_weight.create_by__USER_id = user_id;
            //                    ext_weight.dt_create = dt;
            //                    dbEntity.SOUL_ext.Add(ext_weight);
            //                    dbEntity.SaveChanges();
            //                }
            //            }


            //            // emergency
            //            if (!hasEmergency)
            //            {
            //                if (!string.IsNullOrEmpty(emergency_number))
            //                {
            //                    SOUL_ext ext_emerg = new SOUL_ext();
            //                    ext_emerg.rel_SOUL_id = _soul_id;
            //                    ext_emerg.attr_name = "emergency_number";
            //                    ext_emerg.dname = "Emergency Number";
            //                    ext_emerg.value = emergency_number.ToString();
            //                    ext_emerg.rel_ref_datatype_id = 2;
            //                    ext_emerg.create_by__USER_id = user_id;
            //                    ext_emerg.dt_create = dt;
            //                    dbEntity.SOUL_ext.Add(ext_emerg);
            //                    dbEntity.SaveChanges();

            //                }
            //            }


            //            // Note
            //            if (!hasNote)
            //            {
            //                if (!string.IsNullOrEmpty(note))
            //                {
            //                    SOUL_ext ext_note = new SOUL_ext();
            //                    ext_note.rel_SOUL_id = _soul_id;
            //                    ext_note.attr_name = "note";
            //                    ext_note.dname = "Note";
            //                    ext_note.value = note.ToString();
            //                    ext_note.rel_ref_datatype_id = 2;
            //                    ext_note.create_by__USER_id = user_id;
            //                    ext_note.dt_create = dt;
            //                    dbEntity.SOUL_ext.Add(ext_note);
            //                    dbEntity.SaveChanges();
            //                }
            //            }



            //        }


            //        return Json(new { data = new string[] { }, message = "Update USER record successfully.", success = true });
            //    }

            //    // if user is not FOUND
            //    msg = "No result found.";
            //    return Json(new { data = new string[] { }, message = msg, success = false });
            //}
            //catch (Exception ex)
            //{

            //    return Json(new { data = new string[] { }, message = ex.Message, success = false });
            //}
        }

        //updates records

        private IHttpActionResult  _updatingUser(u_user user1)
        {
            // ~/user  --acceppts class object
            // ~/wr/user -- does not accept
            long user_id = user1.user_id == 0 ? 0 : user1.user_id;
            //string first_name = user1.first_name == null ? "" : user1.first_name,
            //    last_name = user1.last_name == null ? "" : user1.last_name,
            //    email = user1.email == null ? "" : user1.email,
            //    password = user1.password == null ? "" : user1.password,
            //    state = user1.state == null ? "" : user1.state,
            //    status = user1.status == null ? "" : user1.status,
            //    gender = user1.gender == null ? "" : user1.gender,
            //    zip_code = user1.zip_code == null?"": user1.zip_code,
            //    about = user1.about == null ? "" : user1.about;

            long insurance_id = user1.insurance_id == 0 ? 0 : user1.insurance_id;

            // save to SOUL table
            //string phone_no = user1.phone_no == null ? "" : user1.phone_no,
            //    street = user1.street == null ? "" : user1.street,
            //    city = user1.city == null ? "" : user1.city,
            //    parent_guardian = user1.parent_guardian == null ? "" : user1.parent_guardian,
            //    emergency_number = user1.emergency_number == null ? "" : user1.emergency_number,
            //    note = user1.note == null ? "" : user1.note;

            //string birthdate = new DateTime();
            // bool isBool = DateTime.TryParse(user1.dob, out birthdate);
            //double height = user1.height, weight = user1.weight;

            string msg = "";

            try
            {


                IsRequired("user_id", user_id.ToString(), 2);
                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }

                var _user = dbEntity.USERs.Find(user_id);
                if (_user == null)
                {
                    msg = "Invalid user_id value.";
                    return Json(new { data = new string[] { }, message = msg, success = false });
                }

                // we need check if EMAIL already existed
                if (!string.IsNullOrEmpty(user1.email))
                {

                    var user = dbEntity.USERs.Where(a => a.username == user1.email.ToLower());
                    if (user.Count() > 0)
                    {
                        //user = user.Where(a => a.id == user_id);
                        //if (user.Count() > 0)
                        //{  }
                        //else
                        //{
                        msg = "Email already existed.";
                        return Json(new { data = new string[] { }, message = msg, success = false });
                        //}
                    }
                }


                if (_user != null)
                {
                    //USER _user = new USER();

                    //required fields
                    if (!string.IsNullOrEmpty(user1.first_name))
                    {
                        _user.name_first = user1.first_name;
                    }

                    if (!string.IsNullOrEmpty(user1.last_name))
                    {
                        _user.name_last = user1.last_name;
                    }

                    var old_email = _user.username;
                    if (!string.IsNullOrEmpty(user1.email))
                    { _user.username = user1.email.ToLower(); }

                    if (!string.IsNullOrEmpty(user1.status))
                    {
                        _user.rel_ref_status_id = dbEntity.ref_status_type.Where(a => a.dname == "USER").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname.ToLower() == user1.status.ToLower()).id;
                        //_user.rel_ref_USER_type_id = u_type.id;
                    }



                    if (!string.IsNullOrEmpty(user1.password))
                    {
                        string salt = System.Guid.NewGuid().ToString();
                        string encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(user1.password + salt));
                        //var u_type = dbEntity.ref_USER_type.Where(a => a.dname.ToLower() == user_type.ToLower()).FirstOrDefault();
                        _user.phash = encryp;
                        _user.tlas = salt;
                    }

                    if (!string.IsNullOrEmpty(user1.first_name) || !string.IsNullOrEmpty(user1.last_name) || !string.IsNullOrEmpty(user1.email)
                        || !string.IsNullOrEmpty(user1.status) || !string.IsNullOrEmpty(user1.password))
                    {
                        // insurance_type
                        // user_type
                        _user.dt_update = dt;
                        _user.update_by__USER_id = user_id;
                        dbEntity.Entry(_user).State = System.Data.Entity.EntityState.Modified;
                        dbEntity.SaveChanges();
                    }


                    string user_type = dbEntity.ref_USER_type.Find(_user.rel_ref_USER_type_id).dname.ToLower();
                    if (true) //user_type == "patient"
                    {
                        // ****
                        // SAVE TO table SOUL
                        var _soul = from a in dbEntity.SOULs select a;
                        if (!string.IsNullOrEmpty(old_email))
                        {
                            _soul = _soul.Where(a => a.email == old_email); // _soul.Where(a => a.email == old_email).FirstOrDefault(); 
                        }
                        else //if no email address we need the SOCIAL_ID as the reference
                        {
                            var user_ext = dbEntity.USERs.Find(user_id);
                            var s_ext = dbEntity.SOUL_ext.Where(a => a.attr_name == "social_id" && a.value == user_ext.social_id).FirstOrDefault();
                            _soul = _soul.Where(a => a.id == s_ext.rel_SOUL_id);
                        }

                        //if (!string.IsNullOrEmpty(zipcode))
                        //{
                        //    var ref_zip = dbEntity.ref_zip.Where(a => a.zip == zipcode).FirstOrDefault();
                        //    _soul.FirstOrDefault().addr_rel_ref_zip_id = ref_zip.id;
                        //}

                        if (!string.IsNullOrEmpty(user1.first_name))
                            _soul.FirstOrDefault().name_first = user1.first_name;
                        if (!string.IsNullOrEmpty(user1.last_name))
                            _soul.FirstOrDefault().name_last = user1.last_name;
                        if (!string.IsNullOrEmpty(user1.email))
                            _soul.FirstOrDefault().email = user1.email.ToLower();
                        if (!string.IsNullOrEmpty(user1.phone_no))
                            _soul.FirstOrDefault().phone = user1.phone_no;

                        if (!string.IsNullOrEmpty(user1.gender))
                        {
                            _soul.FirstOrDefault().gender = user1.gender.Substring(0, 1).ToUpper();
                        }

                        if (!string.IsNullOrEmpty(user1.zip_code))
                        {
                            var z = dbEntity.ref_zip.Where(a => a.zip == user1.zip_code).FirstOrDefault();

                            if (z != null)
                            {
                                _soul.FirstOrDefault().addr_rel_ref_zip_id = z.id;
                            }
                            else
                            {
                                return Json(new { data = new string[] { }, message = "zip_code not found.", success = false });
                            }

                        }

                        if (!string.IsNullOrEmpty(user1.street))
                            _soul.FirstOrDefault().addr_address1 = user1.street;

                        if (!string.IsNullOrEmpty(user1.zip_code) ||
                            !string.IsNullOrEmpty(user1.gender) ||
                            !string.IsNullOrEmpty(user1.first_name) ||
                            !string.IsNullOrEmpty(user1.last_name) ||
                            !string.IsNullOrEmpty(user1.email) ||
                            !string.IsNullOrEmpty(user1.phone_no) ||
                            !string.IsNullOrEmpty(user1.street))
                        {
                            _soul.FirstOrDefault().dt_update = dt;
                            //_soul.rel_ref_status_id = dbEntity.ref_status_type.Where(a => a.dname == "USER").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname == "Active").id;
                            _soul.FirstOrDefault().update_by__USER_id = user_id;
                            //dbEntity.SOULs.Add(_soul);
                            dbEntity.Entry(_soul.FirstOrDefault()).State = System.Data.Entity.EntityState.Modified;
                            dbEntity.SaveChanges();
                        }


                        //  ***
                        //  update TO table SOUL_ext
                        long _soul_id = _soul.FirstOrDefault().id;
                        var soul_ext = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == _soul_id);

                        #region
                        //bool hasInsurance = false, hasPhone = false, hasAddr = false, hasCity = false, hasState = false, hasDOB = false, hasParent = false,
                        //    hasHeight = false, hasWeight = false, hasEmergency = false, hasNote = false, hasAbout = false;
                        //if (soul_ext.Count() > 0)
                        //{
                        //    foreach (var i in soul_ext)
                        //    {
                        //        switch (i.attr_name)
                        //        {
                        //            case "city":
                        //                #region "city"
                        //                if (!string.IsNullOrEmpty(city))
                        //                {
                        //                    hasCity = true;
                        //                    //var ext_city = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "city").FirstOrDefault();
                        //                    i.value = city;

                        //                    i.dt_update = dt;
                        //                    i.update_by__USER_id = user_id;
                        //                    dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                        //                    //dbEntity.SaveChanges();
                        //                }
                        //                break;
                        //            #endregion

                        //            case "state":
                        //                #region "state"
                        //                if (!string.IsNullOrEmpty(state))
                        //                {
                        //                    hasState = true;

                        //                    //SOUL_ext ext_city = new SOUL_ext();
                        //                    //var ext_state = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "state").FirstOrDefault();
                        //                    i.value = city;

                        //                    i.dt_update = dt;
                        //                    i.update_by__USER_id = user_id;
                        //                    dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                        //                    //dbEntity.SaveChanges();
                        //                }
                        //                break;
                        //            #endregion

                        //            case "dob":

                        //                #region "dob"
                        //                if (birthdate > new DateTime())
                        //                {
                        //                    hasDOB = true;

                        //                    //var ext_dob = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "dob").FirstOrDefault();
                        //                    i.value = birthdate.ToShortDateString();
                        //                    i.dt_update = dt;
                        //                    i.update_by__USER_id = user_id;
                        //                    dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                        //                    //dbEntity.SaveChanges();
                        //                }
                        //                break;

                        //            #endregion

                        //            case "parent_guardian":

                        //                #region "parent_guardian"
                        //                if (!string.IsNullOrEmpty(parent_guardian))
                        //                {
                        //                    hasParent = true;

                        //                    //SOUL_ext ext_parent = new SOUL_ext();
                        //                    //var ext_parent = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "parent_guardian").FirstOrDefault();
                        //                    i.value = parent_guardian;

                        //                    i.dt_update = dt;
                        //                    i.update_by__USER_id = user_id;
                        //                    dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                        //                    //dbEntity.SaveChanges();
                        //                }
                        //                break;
                        //            #endregion


                        //            case "insurance_id":

                        //                #region "insurance_id"
                        //                // if (!string.IsNullOrEmpty(insurance_id))
                        //                if(insurance_id > 0)
                        //                {
                        //                    hasInsurance = true;

                        //                    //SOUL_ext ext_parent = new SOUL_ext();
                        //                    //var ext_parent = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "parent_guardian").FirstOrDefault();
                        //                    i.value = insurance_id.ToString();

                        //                    i.dt_update = dt;
                        //                    i.update_by__USER_id = user_id;
                        //                    dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                        //                    //dbEntity.SaveChanges();
                        //                }
                        //                break;
                        //            #endregion

                        //            case "height":

                        //                #region "height"
                        //                if (height > 0)
                        //                {
                        //                    hasHeight = true;

                        //                    //SOUL_ext ext_height = new SOUL_ext();
                        //                    //var ext_height = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "height").FirstOrDefault();
                        //                    i.value = height.ToString();

                        //                    i.dt_update = dt;
                        //                    i.update_by__USER_id = user_id;
                        //                    dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                        //                    //dbEntity.SaveChanges();
                        //                }
                        //                break;
                        //            #endregion


                        //            case "weight":

                        //                #region "weight"
                        //                if (weight > 0)
                        //                {
                        //                    hasWeight = true;

                        //                    // var ext_weight = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "weight").FirstOrDefault();
                        //                    i.value = weight.ToString();

                        //                    i.dt_update = dt;
                        //                    i.update_by__USER_id = user_id;
                        //                    dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                        //                    //dbEntity.SaveChanges();
                        //                }
                        //                break;
                        //            #endregion


                        //            case "emergency_number":

                        //                #region "emergency_number"
                        //                if (!string.IsNullOrEmpty(emergency_number))
                        //                {
                        //                    hasEmergency = true;

                        //                    //var ext_emerg = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "emergency_number").FirstOrDefault();
                        //                    i.value = emergency_number.ToString();

                        //                    i.dt_update = dt;
                        //                    i.update_by__USER_id = user_id;
                        //                    dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                        //                    //dbEntity.SaveChanges();

                        //                }
                        //                break;
                        //            #endregion


                        //            case "note":
                        //                #region "note"
                        //                if (!string.IsNullOrEmpty(note))
                        //                {
                        //                    hasNote = true;

                        //                    //var ext_note = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "note").FirstOrDefault();
                        //                    i.value = note.ToString();

                        //                    i.dt_update = dt;
                        //                    i.update_by__USER_id = user_id;
                        //                    dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                        //                    //dbEntity.SaveChanges();
                        //                }
                        //                break;
                        //            #endregion

                        //            case "about":
                        //                #region "about"
                        //                if (!string.IsNullOrEmpty(about))
                        //                {
                        //                    hasAbout = true;

                        //                    //var ext_note = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == _soul.id && b.attr_name == "note").FirstOrDefault();
                        //                    i.value = about;

                        //                    i.dt_update = dt;
                        //                    i.update_by__USER_id = user_id;
                        //                    dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                        //                    //dbEntity.SaveChanges();
                        //                }
                        //                break;
                        //                #endregion
                        //        }
                        //    }
                        //    dbEntity.SaveChanges();
                        //}
                        #endregion


                        // city
                        if (!string.IsNullOrEmpty(user1.city))
                        {
                            bool i = Validation.saveSOUL_ext("city", "City", user1.city, _soul_id, user_id);
                        }

                        #region "city_"
                        //if (!hasCity)
                        //{

                        #region
                        //SOUL_ext ext_city = new SOUL_ext();
                        //    ext_city.rel_SOUL_id = _soul_id;
                        //    ext_city.attr_name = "city";
                        //    ext_city.dname = "City";
                        //    ext_city.value = city;
                        //    ext_city.rel_ref_datatype_id = 2;
                        //    ext_city.create_by__USER_id = user_id;
                        //    ext_city.dt_create = dt;
                        //    dbEntity.SOUL_ext.Add(ext_city);
                        //    dbEntity.SaveChanges();
                        #endregion
                        //}
                        #endregion

                        // state
                        if (!string.IsNullOrEmpty(user1.state))
                        {
                            bool i = Validation.saveSOUL_ext("state", "State", user1.state, _soul_id, user_id);
                        }

                        #region "state_"
                        //if (!hasState)
                        //{
                        //    SOUL_ext ext_state = new SOUL_ext();
                        //    ext_state.rel_SOUL_id = _soul_id;
                        //    ext_state.attr_name = "state";
                        //    ext_state.dname = "state";
                        //    ext_state.value = state;
                        //    ext_state.rel_ref_datatype_id = 2;
                        //    ext_state.create_by__USER_id = user_id;
                        //    ext_state.dt_create = dt;
                        //    dbEntity.SOUL_ext.Add(ext_state);
                        //    dbEntity.SaveChanges();
                        //}
                        #endregion

                        // date of birth
                        if (!string.IsNullOrEmpty(user1.dob))
                        {
                            string dat = Validation.validateDate(user1.dob);
                            if (dat.Length > 0) {
                                bool i = Validation.saveSOUL_ext("dob", "Date Of Birth", dat, _soul_id, user_id);
                            }
                            else
                            { return Json(new { data = new string[] { }, message = dat, success = false }); }
                        }

                        #region "date of birth_"
                        //if (!hasDOB)
                        //{
                        //    //SOUL_ext ext_dob = new SOUL_ext();
                        //    //ext_dob.rel_SOUL_id = _soul_id;
                        //    //ext_dob.attr_name = "dob";
                        //    //ext_dob.dname = "DOB";
                        //    //ext_dob.value = birthdate.ToShortDateString();
                        //    //ext_dob.rel_ref_datatype_id = 2;
                        //    //ext_dob.create_by__USER_id = user_id;
                        //    //ext_dob.dt_create = dt;
                        //    //dbEntity.SOUL_ext.Add(ext_dob);
                        //    //dbEntity.SaveChanges();
                        //}
                        #endregion

                        // parent/guardian
                        if (!string.IsNullOrEmpty(user1.parent_guardian))
                        {
                            bool i = Validation.saveSOUL_ext("parent_guardian", "Parent or Guardian", user1.parent_guardian, _soul_id, user_id);
                        }

                        #region
                        //if (!hasParent)
                        //{


                        //    SOUL_ext ext_parent = new SOUL_ext();
                        //    ext_parent.rel_SOUL_id = _soul_id;
                        //    ext_parent.attr_name = "parent_guardian";
                        //    ext_parent.dname = "Parent or Guardian";
                        //    ext_parent.value = parent_guardian;
                        //    ext_parent.rel_ref_datatype_id = 2;
                        //    ext_parent.create_by__USER_id = user_id;
                        //    ext_parent.dt_create = dt;
                        //    dbEntity.SOUL_ext.Add(ext_parent);
                        //    dbEntity.SaveChanges();
                        //}
                        #endregion

                        // insurance_id
                        if (insurance_id > 0)
                        {
                            bool i = Validation.saveSOUL_ext("insurance_id", "Insurance ID", insurance_id.ToString(), _soul_id, user_id);
                        }

                        #region
                        //if (!hasInsurance)
                        //{
                        //    //if (!string.IsNullOrEmpty(insurance_id))



                        //    SOUL_ext ext_parent = new SOUL_ext();
                        //    ext_parent.rel_SOUL_id = _soul_id;
                        //    ext_parent.attr_name = "insurance_id";
                        //    ext_parent.dname = "Insurance ID";
                        //    ext_parent.value = insurance_id.ToString();
                        //    ext_parent.rel_ref_datatype_id = 2;
                        //    ext_parent.create_by__USER_id = user_id;
                        //    ext_parent.dt_create = dt;
                        //    dbEntity.SOUL_ext.Add(ext_parent);
                        //    dbEntity.SaveChanges();
                        //}
                        #endregion


                        // height
                        if (user1.height > 0)
                        {
                            bool i = Validation.saveSOUL_ext("height", "Height", user1.height.ToString(), _soul_id, user_id);
                        }

                        #region
                        //if (!hasHeight)
                        //{
                        //    SOUL_ext ext_height = new SOUL_ext();
                        //    ext_height.rel_SOUL_id = _soul_id;
                        //    ext_height.attr_name = "height";
                        //    ext_height.dname = "Height";
                        //    ext_height.value = height.ToString();
                        //    ext_height.rel_ref_datatype_id = 1;
                        //    ext_height.create_by__USER_id = user_id;
                        //    ext_height.dt_create = dt;
                        //    dbEntity.SOUL_ext.Add(ext_height);
                        //    dbEntity.SaveChanges();
                        //}
                        #endregion

                        // weight
                        if (user1.weight > 0)
                        {
                            bool i = Validation.saveSOUL_ext("weight", "Weight", user1.weight.ToString(), _soul_id, user_id);
                        }

                        #region
                        //if (!hasWeight)
                        //{

                        //    SOUL_ext ext_weight = new SOUL_ext();
                        //    ext_weight.rel_SOUL_id = _soul_id;
                        //    ext_weight.attr_name = "weight";
                        //    ext_weight.dname = "Weight";
                        //    ext_weight.value = weight.ToString();
                        //    ext_weight.rel_ref_datatype_id = 1;
                        //    ext_weight.create_by__USER_id = user_id;
                        //    ext_weight.dt_create = dt;
                        //    dbEntity.SOUL_ext.Add(ext_weight);
                        //    dbEntity.SaveChanges();
                        //}
                        #endregion


                        // emergency
                        if (!string.IsNullOrEmpty(user1.emergency_number))
                        {
                            bool i = Validation.saveSOUL_ext("emergency_number", "Emergency Number", user1.emergency_number, _soul_id, user_id);

                        }

                        #region
                        //if (!hasEmergency)
                        //{
                        //    SOUL_ext ext_emerg = new SOUL_ext();
                        //    ext_emerg.rel_SOUL_id = _soul_id;
                        //    ext_emerg.attr_name = "emergency_number";
                        //    ext_emerg.dname = "Emergency Number";
                        //    ext_emerg.value = emergency_number.ToString();
                        //    ext_emerg.rel_ref_datatype_id = 2;
                        //    ext_emerg.create_by__USER_id = user_id;
                        //    ext_emerg.dt_create = dt;
                        //    dbEntity.SOUL_ext.Add(ext_emerg);
                        //    dbEntity.SaveChanges();
                        //}
                        #endregion

                        // Note
                        if (!string.IsNullOrEmpty(user1.note))
                        {
                            bool i = Validation.saveSOUL_ext("note", "Note", user1.note, _soul_id, user_id);
                        }

                        #region
                        //if (!hasNote)
                        //{
                        //    SOUL_ext ext_note = new SOUL_ext();
                        //    ext_note.rel_SOUL_id = _soul_id;
                        //    ext_note.attr_name = "note";
                        //    ext_note.dname = "Note";
                        //    ext_note.value = note.ToString();
                        //    ext_note.rel_ref_datatype_id = 2;
                        //    ext_note.create_by__USER_id = user_id;
                        //    ext_note.dt_create = dt;
                        //    dbEntity.SOUL_ext.Add(ext_note);
                        //    dbEntity.SaveChanges();
                        //}
                        #endregion

                        // About
                        if (!string.IsNullOrEmpty(user1.about))
                        {
                            bool i = Validation.saveSOUL_ext("about", "About", user1.about, _soul_id, user_id);
                        }

                        #region
                        //if (!hasAbout)
                        //{


                        //    SOUL_ext ext_note = new SOUL_ext();
                        //    ext_note.rel_SOUL_id = _soul_id;
                        //    ext_note.attr_name = "about";
                        //    ext_note.dname = "About";
                        //    ext_note.value = about.ToString();
                        //    ext_note.rel_ref_datatype_id = 2;
                        //    ext_note.create_by__USER_id = user_id;
                        //    ext_note.dt_create = dt;
                        //    dbEntity.SOUL_ext.Add(ext_note);
                        //    dbEntity.SaveChanges();
                        //}
                        #endregion

                    }


                    return Json(new { data = new string[] { }, message = "Update USER record successfully.", success = true });
                }

                // if user is not FOUND
                msg = "No result found.";
                return Json(new { data = new string[] { }, message = msg, success = false });
            }
            catch (Exception ex)
            {

                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
            //return Json(new { data = new string[] { }, message = "ex.Message", success = false });
        }

        public async Task<IHttpActionResult> Put1(long user_id,
            string firstname="", string lastname="", string email="",string password="", string status=""
            )
        {
            //string password=""; string status=""; long doctorId=0; long patientId = 0;

            string msg = "";
           
            try {
                var u_cre = dbEntity.USERs.Find(user_id);
                if (u_cre != null)
                {
                    USER _user = new USER();
                    DateTime dtNow = DateTime.Now;

                    

                    //required fields
                    if (!string.IsNullOrEmpty(firstname))
                    {
                        _user.name_first = firstname;
                    }

                    if (!string.IsNullOrEmpty(lastname))
                    {
                        _user.name_last = lastname;
                    }

                    if (!string.IsNullOrEmpty(email))
                    { _user.username = email.ToLower(); }

                    if (!string.IsNullOrEmpty(status)) {
                        _user.rel_ref_status_id = dbEntity.ref_status_type.Where(a => a.dname == "USER").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname.ToLower() == status.ToLower()).id;
                                                     //_user.rel_ref_USER_type_id = u_type.id;
                    }

                    if (!string.IsNullOrEmpty(password)) {
                        //// password is updated, we need to get 
                        //IsRequired("old_password", old_password, 1);
                        //if (haserror)
                        //{
                        //    return Json(new { data = new string[] { }, message = errmsg, success = false });
                        //}

                        string salt = System.Guid.NewGuid().ToString();
                        string encryp = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + salt));
                        //var u_type = dbEntity.ref_USER_type.Where(a => a.dname.ToLower() == user_type.ToLower()).FirstOrDefault();
                        _user.phash = encryp;
                        _user.tlas = salt;
                    }
                    
                    // insurance_type
                    // user_type
                    dbEntity.Entry(_user).State = System.Data.Entity.EntityState.Modified;
                    dbEntity.SaveChanges();
                }

                // if user is not FOUND
                msg = "No result found.";
                return Json(new { message = msg, success = false });
            }
            catch (Exception ex)
            {

                return Json(new { message = ex.Message, success = false });
            }
      
        }

        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("user")]
        public async Task<IHttpActionResult> Delete()
        {
            string msg = "";
            long user_id=0, toupdate_id = 0;
            DateTime dtNow = DateTime.Now;

            string root = HttpContext.Current.Server.MapPath("~/Temp");
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
                            case "user_id":
                                IsRequired(key, val, 1);
                                long n_user = 0;
                                bool b_user = long.TryParse(val, out n_user);
                                toupdate_id = n_user; break;
                            //case "patient_id":
                            //    long patient_id = 0;
                            //    bool b_patient = long.TryParse(val, out patient_id);
                            //    toupdate_id = patient_id; break;
                            default:
                                msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                                return Json(new { data = new string[] { }, message = "", success = false });
                        }
                    }
                }

                IsRequired("user_id", toupdate_id.ToString(), 2);
                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }

                var _update = dbEntity.USERs.Find(toupdate_id);
                if (_update != null)
                {
                    _update.rel_ref_status_id = dbEntity.ref_status_type.Where(a => a.dname == "USER").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname == "Deleted").id;
                    _update.update_by__USER_id = user_id;
                    _update.dt_update = dt;

                    dbEntity.Entry(_update).State = System.Data.Entity.EntityState.Modified;
                    dbEntity.SaveChanges();

                    return Json(new { data = new string[] { }, message = "USER successfully deleted.", success = true });

                }
                else
                {
                    msg = "No result found.";
                    return Json(new { data = new string[] { }, message = msg, success = false });
                }

            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }

        }

      


        bool haserror = false;
        string errmsg = "", infomsg = "";
        public bool IsRequired(string key, string val, int i)
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

    }

  
}
