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

namespace api.Controllers
{
   
    public class LoginController : ApiController
    {
        SV_db1Entities dbEntity = new SV_db1Entities();
        DateTime dt = DateTime.UtcNow;

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

                        //var s_user = dbEntity.SOULs.Where(a => a.email == u_login.FirstOrDefault().username).FirstOrDefault();
                        var s_user = dbEntity.SOULs.Find(soul_id);

                        //
                        //var u_phone = dbEntity.SOUL_ext.Where(a => a.attr_name == "phone_no" && a.rel_SOUL_id == s_user.id).FirstOrDefault();
                        var u_city = dbEntity.SOUL_ext.Where(a => a.attr_name == "city" && a.rel_SOUL_id == s_user.id).FirstOrDefault();
                        var u_state = dbEntity.SOUL_ext.Where(a => a.attr_name == "state" && a.rel_SOUL_id == s_user.id).FirstOrDefault();
                        var u_dob = dbEntity.SOUL_ext.Where(a => a.attr_name == "dob" && a.rel_SOUL_id == s_user.id).FirstOrDefault();
                        DateTime dt_DOB = DateTime.Now;
                        bool isDoBvalid = false;
                        if (u_dob != null)
                        {
                            isDoBvalid = DateTime.TryParse(u_dob.value, out dt_DOB);
                            //string n = dt_DOB.ToString("yyyy-MM-dd");
                        }

                        var u_height = dbEntity.SOUL_ext.Where(a => a.attr_name == "height" && a.rel_SOUL_id == s_user.id).FirstOrDefault();
                        var u_weight = dbEntity.SOUL_ext.Where(a => a.attr_name == "weight" && a.rel_SOUL_id == s_user.id).FirstOrDefault();
                        var u_emergency = dbEntity.SOUL_ext.Where(a => a.attr_name == "emergency_number" && a.rel_SOUL_id == s_user.id).FirstOrDefault();
                        var u_guardian = dbEntity.SOUL_ext.Where(a => a.attr_name == "parent_guardian" && a.rel_SOUL_id == s_user.id).FirstOrDefault();
                        var u_note = dbEntity.SOUL_ext.Where(a => a.attr_name == "note" && a.rel_SOUL_id == s_user.id).FirstOrDefault();
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
                            city = u_city == null ? "" : u_city.value,
                            state = u_state == null ? "" : u_state.value,
                            zip_code =  zipcode,
                            dob = isDoBvalid == false ? "" : dt_DOB.ToString("yyyy-MM-dd"),
                            parent_guardian = u_guardian == null ? "" : u_guardian.value,
                            gender = s_user.gender == null ? "" : s_user.gender.ToUpper(),
                            height = u_height == null ? 0 : Convert.ToDouble(u_height.value),
                            weight = u_weight == null ? 0 : Convert.ToDouble(u_weight.value),
                            emergency_number = u_emergency == null ? "" : u_emergency.value,
                            note = u_note == null ? "" : u_note.value,
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

                    #region
                    //USER_ext ext_ins_type = new USER_ext();
                    //ext_ins_type.rel_USER_id = new_user.id;
                    //ext_ins_type.attr_name = "device_type";
                    //ext_ins_type.dname = "Device Type";
                    //ext_ins_type.value = login.device_type;
                    //ext_ins_type.rel_ref_datatype_id = 2;
                    //ext_ins_type.dt_create = dt;
                    //ext_ins_type.create_by__USER_id = 0;
                    //dbEntity.USER_ext.Add(ext_ins_type);
                    //dbEntity.SaveChanges();
                    #endregion
                }

                if (!string.IsNullOrEmpty(login.device_token))
                {
                    bool i = Validation.saveUSER_ext("device_token", "Device Token", login.device_token, new_user.id);

                    #region
                    //USER_ext ext_ins_type = new USER_ext();
                    //ext_ins_type.rel_USER_id = new_user.id;
                    //ext_ins_type.attr_name = "device_token";
                    //ext_ins_type.dname = "Device Token";
                    //ext_ins_type.value = login.device_token;
                    //ext_ins_type.rel_ref_datatype_id = 2;
                    //ext_ins_type.dt_create = dt;
                    //ext_ins_type.create_by__USER_id = 0;
                    //dbEntity.USER_ext.Add(ext_ins_type);
                    //dbEntity.SaveChanges();
                    #endregion

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

                    #region
                    //SOUL_ext ext_ins_type = new SOUL_ext();
                    //ext_ins_type.rel_SOUL_id = new_soul.id;
                    //ext_ins_type.attr_name = "insurance_id";
                    //ext_ins_type.dname = "Insurance ID";
                    //ext_ins_type.value = login.insurance_id.ToString();
                    //ext_ins_type.rel_ref_datatype_id = 2;
                    //ext_ins_type.dt_create = dt;
                    //ext_ins_type.create_by__USER_id = 0;
                    //dbEntity.SOUL_ext.Add(ext_ins_type);
                    //dbEntity.SaveChanges();
                    #endregion
                }

                #region
                //// city
                //if (!string.IsNullOrEmpty(city))
                //{
                //    SOUL_ext ext_city = new SOUL_ext();
                //    ext_city.rel_SOUL_id = new_soul.id;
                //    ext_city.attr_name = "city";
                //    ext_city.dname = "City";
                //    ext_city.value = city;
                //    ext_city.rel_ref_datatype_id = 2;
                //    ext_city.dt_create = dtNow;
                //    ext_city.create_by__USER_id = user_id;
                //    dbEntity.SOUL_ext.Add(ext_city);
                //    dbEntity.SaveChanges();
                //}

                //// date of birth
                //if (birthdate > new DateTime())
                //{
                //    SOUL_ext ext_dob = new SOUL_ext();
                //    ext_dob.rel_SOUL_id = new_soul.id;
                //    ext_dob.attr_name = "dob";
                //    ext_dob.dname = "DOB";
                //    ext_dob.value = birthdate.ToShortDateString();
                //    ext_dob.rel_ref_datatype_id = 5;
                //    ext_dob.dt_create = dtNow;
                //    ext_dob.create_by__USER_id = user_id;
                //    dbEntity.SOUL_ext.Add(ext_dob);
                //    dbEntity.SaveChanges();
                //}


                //// parent/guardian
                //if (!string.IsNullOrEmpty(parent_guardian))
                //{
                //    SOUL_ext ext_parent = new SOUL_ext();
                //    ext_parent.rel_SOUL_id = new_soul.id;
                //    ext_parent.attr_name = "parent_guardian";
                //    ext_parent.dname = "Parent or Guardian";
                //    ext_parent.value = parent_guardian;
                //    ext_parent.rel_ref_datatype_id = 2;
                //    ext_parent.dt_create = dtNow;
                //    ext_parent.create_by__USER_id = user_id;
                //    dbEntity.SOUL_ext.Add(ext_parent);
                //    dbEntity.SaveChanges();
                //}

                //if (height > 0)
                //{
                //    SOUL_ext ext_height = new SOUL_ext();
                //    ext_height.rel_SOUL_id = new_soul.id;
                //    ext_height.attr_name = "height";
                //    ext_height.dname = "Height";
                //    ext_height.value = height.ToString();
                //    ext_height.rel_ref_datatype_id = 1;
                //    ext_height.dt_create = dtNow;
                //    ext_height.create_by__USER_id = user_id;
                //    dbEntity.SOUL_ext.Add(ext_height);
                //    dbEntity.SaveChanges();
                //}

                //if (weight > 0)
                //{
                //    SOUL_ext ext_weight = new SOUL_ext();
                //    ext_weight.rel_SOUL_id = new_soul.id;
                //    ext_weight.attr_name = "weight";
                //    ext_weight.dname = "Weight";
                //    ext_weight.value = weight.ToString();
                //    ext_weight.rel_ref_datatype_id = 1;
                //    ext_weight.dt_create = dtNow;
                //    ext_weight.create_by__USER_id = user_id;
                //    dbEntity.SOUL_ext.Add(ext_weight);
                //    dbEntity.SaveChanges();
                //}

                //if (!string.IsNullOrEmpty(emergency_number))
                //{
                //    SOUL_ext ext_emerg = new SOUL_ext();
                //    ext_emerg.rel_SOUL_id = new_soul.id;
                //    ext_emerg.attr_name = "emergency_number";
                //    ext_emerg.dname = "Emergency Number";
                //    ext_emerg.value = emergency_number.ToString();
                //    ext_emerg.rel_ref_datatype_id = 2;
                //    ext_emerg.dt_create = dtNow;
                //    ext_emerg.create_by__USER_id = user_id;
                //    dbEntity.SOUL_ext.Add(ext_emerg);
                //    dbEntity.SaveChanges();

                //}

                //if (!string.IsNullOrEmpty(note))
                //{
                //    SOUL_ext ext_note = new SOUL_ext();
                //    ext_note.rel_SOUL_id = new_soul.id;
                //    ext_note.attr_name = "note";
                //    ext_note.dname = "Note";
                //    ext_note.value = note.ToString();
                //    ext_note.rel_ref_datatype_id = 2;
                //    ext_note.dt_create = dtNow;
                //    ext_note.create_by__USER_id = user_id;
                //    dbEntity.SOUL_ext.Add(ext_note);
                //    dbEntity.SaveChanges();
                //}


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



                var p_user = dbEntity.SOULs.Where(b => b.create_by__USER_id == new_user.id);
                List<user_secondary_patient> pat = new List<user_secondary_patient>();
                string zipCode = "", insuranceid = "", insurancename = "";
                if (p_user.Count() > 0)
                {
                    foreach (var i in p_user)
                    {
                        var p_ext = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == i.id && a.attr_name == "insurance_id");
                        var u_user = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == i.id && a.attr_name == "use_user_insurance");

                        bool user_insurance = false;
                        string ins_id_string = "", ins_name = "";
                        long ins_id = 0;
                        if (u_user.Count() > 0)
                        {
                            if (u_user.FirstOrDefault().value == "value")
                                user_insurance = true;
                            else
                                user_insurance = false;
                        }

                        if (user_insurance == false && p_ext.Count() > 0)
                        {
                            ins_id_string = p_ext.FirstOrDefault().value;
                            if (p_ext.Count() > 0)
                            {
                                // get insurance_id
                                bool bTemp = long.TryParse(ins_id_string, out ins_id);
                                if (bTemp)
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
                        }

                        //bool bUser = false;
                        //if (u_user.Count() > 0)
                        //{
                        //    bUser = true;
                        //}
                        //long ins_id = 0;
                        //bool bIns_id = long.TryParse(p_ext.FirstOrDefault().value, out ins_id);
                        //var ins_name = dbEntity.ref_insurance_provider.Find(ins_id);
                        //string insname = (ins_name.PayerName.Split('|')[0]).Trim();

                        if (i.is_prime.Value)
                        {
                            zipCode = i.ref_zip == null ? "" : i.ref_zip.zip;
                            insuranceid = ins_id_string;
                            insurancename = ins_name;
                        }
                        else //8-29-2017 removing the PRIMARY patient in the patient's LIST
                        {
                            pat.Add(new user_secondary_patient
                            {
                                id = i.id,
                                first_name = i.name_first == null ? "" : i.name_first,
                                last_name = i.name_last == null ? "" : i.name_last,
                                is_prime = i.is_prime.Value,
                                is_using_primary_patient_insurance = user_insurance,
                                insurance_id = ins_id_string,
                                insurance_name = ins_name

                            });
                        }
                      
                    }
                }


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
                    zip_code =zipCode,
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
                    insurance_id = insuranceid,
                    insurance_name = insurancename,
                    patient = pat
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
                        var p_user = dbEntity.SOULs.Where(b => (b.create_by__USER_id == u_rec.id || b.email == u_rec.username) && b.rel_ref_status_id != 3);

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
                        // end of patients info


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
                            zip_code = zipcode,
                            dob = isDoBvalid == false ? "" : dt_DOB.ToString("yyyy-MM-dd"),
                            parent_guardian = u_guardian == null ? "" : u_guardian.value,
                            gender = prim_user.gender == null ? "" : prim_user.gender,
                            height = u_height == null ? 0 : Convert.ToDouble(u_height.value),
                            weight = u_weight == null ? 0 : Convert.ToDouble(u_weight.value),
                            emergency_number = u_emergency == null ? "" : u_emergency.value,
                            note = u_note == null ? "" : u_note.value,
                            //status = ref_stat == null ? "" : ref_dname,
                            user_type = u_type,
                            insurance_id = insuranceid,
                            insurance_name = insurancename,
                            patient = pat
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
        bool haserror = false;
        string errmsg = "", infomsg = "";
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

                        USER_ext u_ext = new USER_ext();
                        u_ext.rel_USER_id = user_id_new;
                        u_ext.attr_name = "email_verification_code";
                        u_ext.dname = "Email Verification Code";
                        u_ext.rel_ref_datatype_id = 2;
                        u_ext.value = veri_code;
                        u_ext.create_by__USER_id = user_id_new; // user_id;
                        u_ext.dt_create = dt;
                        dbEntity.USER_ext.Add(u_ext);
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
                var u_ext = dbEntity.USER_ext.Where(a => a.attr_name == "email_verification_code" && a.value == verification_code);
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

        public bool IsRequired(string key, string val, int i)
        {
            if (i == 1)
            {
                if (string.IsNullOrEmpty(val))
                {
                    haserror = true;
                    errmsg += key + " is required.\r\n ";
                    return false;
                }
                else
                {
                    return true;
                }

            }
            else
            {
                if (string.IsNullOrEmpty(val))
                {
                    haserror = true;
                    errmsg += " Missing parameter: " + key + ".\r\n ";
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }

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
        public class new_USER
        {
            public long user_id { get; set; }
            public string name { get; set; }
            public string username { get; set; }
            //public string verification_code { get; set; }
            public bool status { get; set; }
        }

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
    }
