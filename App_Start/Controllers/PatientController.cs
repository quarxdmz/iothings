using Amazon.S3;
using Amazon.S3.Model;
using api.Models;
using authorization.hs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using api.Models;

namespace api.Controllers
{
    public class PatientController : ApiController
    {
        SV_db1Entities dbEntity = new SV_db1Entities();
        DateTime dt = DateTime.UtcNow;
        //bool haserror = false;
        //string errmsg = "", infomsg = "";
        HSAuth authorized = new HSAuth();
        string new_filename;


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("Patient")]
        public async Task<IHttpActionResult> postPatient()
        {
            string root = HttpContext.Current.Server.MapPath("~/Temp");
            string device_token = "", device_type = "", firstname = "", lastname = "",  user_type = "";
            string insurance_id = "", use_insurance = "";
            bool is_prime = false;
            //int nprimary_type = 0, nuse_insurance = 0;
            //string   address = "", email = "", password = "", zip = "", phone = "", gender = "", salutation = "", acck = "deftsoftapikey";

            //string firstname, string lastname, string email, string password, string user_type,
            string user_id = "";
            string msg = "";
            DateTime dtNow = DateTime.Now;
            //bool b_prime = false;

            var provider = new MultipartFormDataStreamProvider(root);
            try
            {

                //bool use_patient_insurance, long user_id,

                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        switch (key)
                        {
                            case "user_id":
                                custom.IsRequired(key, val, 1);
                                //long nVal = 0;
                                //bool nTemp = long.TryParse(val, out nVal);
                                user_id = val;
                                break;
                            //case "email":
                            //    IsRequired(key, val, 1);
                            //    email = val; break;
                            case "firstname":
                                custom.IsRequired(key, val, 1);
                                firstname = val; break;
                            case "lastname":
                                custom.IsRequired(key, val, 1);
                                lastname = val; break;
                            case "insurance_id":
                                // IsRequired(key, val, 1);
                                insurance_id = val; break;
                            case "is_using_primary_patient_insurance":
                                // IsRequired(key, val, 1);
                                use_insurance = val.ToLower();
                                //bool isvalid = int.TryParse(val, out nuse_insurance);
                                break;
                            case "is_prime": //expecting: true/false string
                                             //IsRequired(key, val, 1);
                                             //is_prime = val;
                                             //bool isVal2 = int.TryParse(val, out nprimary_type);
                                switch (val.ToLower())
                                {
                                    case "true":
                                        is_prime = true;
                                        break;

                                    case "false":
                                        is_prime = false;
                                        break;
                                    default:
                                        msg = "Cannot parse value under parameter name: " + key;
                                        return Json(new { data = new string[] { }, message = msg, success = false });
                                }

                                break;
                            //case "address": address = val; break;
                            //case "zip": zip = val; break;
                            //case "phone": phone = val;break;
                            //case "gender": gender = val; break;
                            //case "salutation": salutation = val; break;

                            case "device_type": device_type = val; break;
                            case "device_token": device_token = val; break;

                            default:
                                msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                                return Json(new { data = new string[] { }, message = msg, success = false });
                        }

                        //string insurance_type = "", device_type = "", device_token = "", 

                    }
                }

                custom.IsRequired("user_id", user_id, 2);
                custom.IsRequired("firstname", firstname, 2);
                custom.IsRequired("lastname", lastname, 2);
                if (!string.IsNullOrEmpty(use_insurance) || !string.IsNullOrEmpty(insurance_id))
                {
                    if (!string.IsNullOrEmpty(use_insurance))
                    {
                        custom.IsRequired("use_user_insurance", use_insurance, 1);
                    }

                    if (!string.IsNullOrEmpty(insurance_id))
                    {
                        custom.IsRequired("insurance_id", insurance_id, 1);
                    }
                }
                else
                {
                    custom.errmsg += "either insurance_id or is_using_primary_patient_insurance required parameter \r\n";
                    return Json(new { data = new string[] { }, message = custom.errmsg, success = false });
                }
                custom.IsRequired("is_prime", is_prime.ToString(), 2);
                if (custom.haserror)
                {
                    return Json(new { data = new string[] { }, message = custom.errmsg, success = false });
                }

                //var check_dup = dbEntity.SOULs.Where(a => a.email == email);
                //if (check_dup.Count() > 0)
                //{
                //    msg = "Email already exist.";
                //    return Json(new { data = "", message = msg, success = false });
                //}

                // (start)Get user_id's username
                var u_user = dbEntity.USERs.Find(Convert.ToInt64(user_id));

                var u_patient = dbEntity.SOULs.Where(a => a.create_by__USER_id == u_user.id || a.email == u_user.username);

                // is_prime == true, set the rest to false
                if (is_prime)
                {
                    if (u_patient.Count() > 0)
                    {
                        foreach (var i in u_patient)
                        {
                            //var u_pat_ext = dbEntity.SOUL_ext.Where(a => a.attr_name == "is_prime" && a.rel_SOUL_id == i.id);
                            //if (u_pat_ext.FirstOrDefault().value == "true")
                            //{

                            //}
                            if (i.is_prime == true)
                            {
                                i.is_prime = false;
                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                        dbEntity.SaveChanges();
                    }
                }
                // (end)Get user_id's username

                var u_type = dbEntity.ref_USER_type.Where(a => a.dname.ToLower() == user_type.ToLower()).FirstOrDefault();

                SOUL new_soul = new SOUL();
                //required fields
                new_soul.name_first = firstname;
                new_soul.name_last = lastname;
                new_soul.is_prime = is_prime;
                new_soul.dt_create = dt;
                new_soul.create_by__USER_id = Convert.ToInt64(user_id); //create != null ? 0 : create.id;
                new_soul.rel_ref_status_id = dbEntity.ref_status_type.Where(a => a.dname == "SOUL").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname == "Active").id;
                //            // insurance_type
                //            // user_type

                dbEntity.SOULs.Add(new_soul);
                dbEntity.SaveChanges();



                // data type
                //1   double
                //2   string
                //3   long
                //4   bool
                //5   datetime

                if (!string.IsNullOrEmpty(use_insurance))
                {
                    // is_using_primary_patient_insurance    
                    SOUL_ext ext_pat_ins = new SOUL_ext();
                    ext_pat_ins.rel_SOUL_id = new_soul.id;
                    ext_pat_ins.attr_name = "use_user_insurance";
                    ext_pat_ins.dname = "Is Using Primary Patient Insurance";
                    ext_pat_ins.rel_ref_datatype_id = 4;

                    //int a;
                    //bool b = int.TryParse(primary_type, out a);
                    ext_pat_ins.value = use_insurance; //old value: nuse_insurance.ToString();
                    ext_pat_ins.create_by__USER_id = Convert.ToInt64(user_id);
                    ext_pat_ins.dt_create = dt;
                    dbEntity.SOUL_ext.Add(ext_pat_ins);
                    dbEntity.SaveChanges();

                }

                if (!string.IsNullOrEmpty(insurance_id))
                {
                    SOUL_ext s_ext = new SOUL_ext();
                    s_ext.rel_SOUL_id = new_soul.id;
                    s_ext.attr_name = "insurance_id";
                    s_ext.dname = "Insurance ID";
                    s_ext.rel_ref_datatype_id = 2;

                    //int a;
                    //bool b = int.TryParse(insurance_type, out a);
                    s_ext.value = insurance_id.ToString();
                    s_ext.create_by__USER_id = Convert.ToInt64(user_id);
                    s_ext.dt_create = dt;
                    dbEntity.SOUL_ext.Add(s_ext);
                    dbEntity.SaveChanges();
                }

                //case "device_type": device_type = val; break;
                //case "device_token": device_token = val; break;
                if (!string.IsNullOrEmpty(device_type))
                {
                    SOUL_ext s_ext = new SOUL_ext();
                    s_ext.rel_SOUL_id = new_soul.id;
                    s_ext.attr_name = "device_type";
                    s_ext.dname = "Device Type";
                    s_ext.rel_ref_datatype_id = 2;

                    s_ext.value = device_type;
                    s_ext.create_by__USER_id = Convert.ToInt64(user_id);
                    s_ext.dt_create = dt;
                    dbEntity.SOUL_ext.Add(s_ext);
                    dbEntity.SaveChanges();
                }

                if (!string.IsNullOrEmpty(device_token))
                {
                    SOUL_ext s_ext = new SOUL_ext();
                    s_ext.rel_SOUL_id = new_soul.id;
                    s_ext.attr_name = "device_token";
                    s_ext.dname = "Device Token";
                    s_ext.rel_ref_datatype_id = 2;

                    s_ext.value = device_token;
                    s_ext.create_by__USER_id = Convert.ToInt64(user_id);
                    s_ext.dt_create = dt;
                    dbEntity.SOUL_ext.Add(s_ext);
                    dbEntity.SaveChanges();
                }

                //if (!string.IsNullOrEmpty(is_prime))
                //{
                //    SOUL_ext ext_prim = new SOUL_ext();
                //    ext_prim.rel_SOUL_id = new_soul.id;
                //    ext_prim.attr_name = "is_prime";
                //    ext_prim.dname = "Is Primary";
                //    ext_prim.rel_ref_datatype_id = 4;

                //    ext_prim.value = nprimary_type.ToString();
                //    ext_prim.create_by__USER_id = 0;
                //    ext_prim.dt_create = dtNow;
                //    dbEntity.SOUL_ext.Add(ext_prim);
                //    dbEntity.SaveChanges();
                //}



                List<p_patient> n = new List<p_patient>();
                //var ref_stat = dbEntity.ref_status_type.Where(a => a.dname == "USER").FirstOrDefault().ref_status.FirstOrDefault(b => b.dname == "Active");
                n.Add(new p_patient
                {
                    id = new_soul.id
                });

                var ret1 = JsonConvert.SerializeObject(n);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                msg = "New Patient is added successfully.";
                return Json(new { data = json1, message = msg, success = true });

            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }

        //[System.Web.Http.Route("Post")]
        //public IHttpActionResult Post( string firstname, string lastname, string insurance_type, bool use_patient_insurance, bool primary_type, long user_id,
        //    string address = "", string zip = "", string phone = "", string email = "", string gender = "", string salutation = "",
        //    string acck ="deftsoftapikey")
        ////  addr_address1
        ////  addr_address2
        ////  addr_rel_ref_zip_id
        ////  email
        ////  gender
        ////  salutation
        ////  dt_create
        ////  dt_update
        ////  create_by__USER_id
        ////  update_by__USER_id
        ////  taken_by__USER_id
        ////  dt_taken
        ////  rel_ref_status_id
        //// Address
        //// zip code)
        //{
        //    // New Patient
        //    // Required parameter: user_id, firstname, lastname, insurance_type, use_patient_insurance(0/1), primary_type(0/1),
        //    // Return required message

        //    string msg = "";
        //    if (acck == "deftsoftapikey")
        //    {
        //        try {
        //            // test data
        //            //var ret1 = "[{\"id\":10,\"firstname\":\"marding\",\"lastname\":\"nizari\",\"email\":\"nizari@ferret9.com\",\"gender\":\"m\",\"address\":\"803 Kings Road\",\"city\":\"Kansas City\",\"state\":\"AZ\",\"zip\":\"00501\",\"status\":\"Active\"}]";
        //            //var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
        //            //return Json(new { data = json1, message = msg, success = true });
        //            // end test data

        //            long zip_id = 0;
        //            if (!string.IsNullOrEmpty(zip))
        //            {
        //                var z = db.ref_zip.Where(a => a.zip == zip).FirstOrDefault();
        //                if (z != null) zip_id = z.id;
        //            }

        //            DateTime dt = DateTime.Now;

        //            SOUL s = new SOUL();
        //            s.name_first = firstname;
        //            s.name_last = lastname;
        //            // insurance_type, 
        //            // use_patient_insurance,
        //            // primary_type

        //            s.phone = phone;
        //            s.addr_address1 = address;
        //            //  addr_address2
        //            //s.addr_address2 = address2;

        //            s.addr_rel_ref_zip_id = zip_id;
        //            s.email = email.ToLower();
        //            s.gender = gender.Substring(0,1).ToLower();
        //            s.salutation = salutation;
        //            s.dt_create = dt;
        //            s.create_by__USER_id = user_id;
        //            //  update_by__USER_id
        //            //  taken_by__USER_id
        //            //  dt_taken
        //            //  rel_ref_status_id
        //            var ref_status = db.ref_status_type.Where(a => a.dname == "SOUL").FirstOrDefault().ref_status.Where(b => b.dname == "Active").FirstOrDefault();
        //            if (ref_status != null)
        //            { s.rel_ref_status_id = ref_status.id; }


        //            db.SOULs.Add(s);
        //            db.SaveChanges();


        //            //var ret1 = JsonConvert.SerializeObject(n);
        //            //var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

        //            msg = "The new record is saved.";
        //            string data = "";
        //            return Json(new { data = "", message = msg, success = true });
        //        }
        //        catch (Exception ex) {

        //            return Json(new { data = "", message = ex.Message, success = true });
        //        }
        //    }

        //    msg = "The authorization header is either not valid or isn't Basic.";
        //    return Json(new { data = "", message = msg, success = false });
        //}
        

        //public bool IsRequired(string key, string val, int i)
        //{
        //    if (i == 1)
        //    {
        //        if (string.IsNullOrEmpty(val))
        //        {
        //            haserror = true;
        //            errmsg += key + " is required. \r\n";
        //            return false;
        //        }
        //        return true;


        //    }
        //    else
        //    {
        //        if (string.IsNullOrEmpty(val))
        //        {
        //            haserror = true;
        //            errmsg += " Missing parameter: " + key + ".\r\n ";
        //            return false;
        //        }
        //        return true;
        //    }

        //}


        // get patient
        [HttpGet]
        [System.Web.Http.Route("Patient")]
        public async Task<IHttpActionResult> Get(long patient_id = 0)
        {
            // Required Parameter :-user_id, first_name, lastname, insuance_type, use_patient_insurance(0 / 1), primary_type(0 / 1),patient_id
            // Return required message

            long user_id = 0;
            //string firstname = "", string lastname = "", string insurance_type = "",
            // //int use_patient_insurance = 0, int primary_type = 0, 
            string acck = "deftsoftapikey"; //, string address = "", string zip = "", string phone = "",
                                            //string email = "", string gender = "", string salutation = ""

            //var root = HttpContext.Current.Server.MapPath("~/Temp");
            //var provider = new MultipartFormDataStreamProvider(root);
            //string msg = "";

            //try {
            //    await Request.Content.ReadAsMultipartAsync(provider);
            //    foreach (var key in provider.FormData.AllKeys) {
            //        foreach (var val in provider.FormData.GetValues(key)) {

            //            switch (key)
            //            {

            //                case "user_id": break;
            //                default:
            //                    msg = "Object reference not set to an instance of an object.";
            //                    return Json(new { data = "", message = msg, success = false });
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex) { }

            string msg = "";
            //var _user = dbEntity.USERs.Find(patient_id);
            //if (_user == null)
            //{
            //   msg = "Object reference not set to an instance of an object.";
            //   return Json(new { data = "", message = msg, success = false });
            //}

            //***************

            if (acck == "deftsoftapikey")
            {
                DateTime dt = DateTime.Now;

                ////SOUL s = new SOUL();
                var _soul = from a in dbEntity.SOULs select a;

                if (patient_id > 0)
                {
                    _soul = _soul.Where(a => a.id == patient_id);
                }

                List<g_patient> patient = new List<g_patient>();
                foreach (var i in _soul)
                {
                    //List<_address> addr = new List<_address>();

                    //if (i.addr_rel_ref_zip_id != null) {
                    //var _zip = dbEntity.ref_zip.Find(i.addr_rel_ref_zip_id.Value);
                    var _ext = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == i.id && a.attr_name == "city").FirstOrDefault();
                    var _note = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == i.id && a.attr_name == "note").FirstOrDefault();
                    //addr.Add(new _address
                    //{
                    //       address = i.addr_address1 + " " + i.addr_address2,
                    //       city = _ext == null ? "": _ext.value
                    //state = _zip.city_state,
                    //lat = _zip.city_lat,
                    //longi = _zip.city_lon,
                    //zip = _zip.zip
                    // });
                    //}
                    //get image profile
                    var _image = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == i.id && a.attr_name == "image").FirstOrDefault();
                    //end get image profile
                    //var _create_fname = dbEntity.USERs.Find(i.create_by__USER_id);
                    //var _create_lname = dbEntity.USERs.Find(i.create_by__USER_id);
                    //var _update_fname = dbEntity.USERs.Find(i.update_by__USER_id);
                    //var _update_lname = dbEntity.USERs.Find(i.update_by__USER_id);
                    var _stat = dbEntity.ref_status.Find(i.rel_ref_status_id);

                    if (_stat.dname.ToLower() != "deleted")
                    {
                        var p_ext = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == i.id && a.attr_name == "insurance_id");
                        var u_user = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == i.id && a.attr_name == "use_user_insurance");

                        long ins_id = 0;
                        string ins_id_string = "", ins_name = "";
                        bool use_insurance = false;


                        if (p_ext.Count() > 0)
                        {
                            ins_id_string = p_ext.FirstOrDefault().value;
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
                                    //msg = "No matching insurance record found.";
                                    //return Json(new { data = "", message = msg, success = false });
                                }
                            }
                            else
                            {
                                //msg = "No matching insurance record found.";
                                //return Json(new { data = "", message = msg, success = false });
                            }

                        }
                        else
                        {

                            if (u_user.Count() > 0)
                            {
                                use_insurance = true;
                            }
                            else
                            {
                                //msg = "No matching insurance record found.";
                                //return Json(new { data = "", message = msg, success = false });
                            }
                        }

                        patient.Add(new g_patient
                        {
                            id = i.id,
                            firstname = i.name_first.Trim(),
                            lastname = i.name_last.Trim(),
                            email = i.email == null ? "" : i.email,
                            is_prime = i.is_prime == null ? false : i.is_prime.Value,
                            insurance_id = p_ext.Count() == 0 ? 0 : Convert.ToInt32(p_ext.FirstOrDefault().value),
                            insurance_name = ins_name,
                            is_using_primary_patient_insurance = use_insurance,

                            //phone = string.IsNullOrEmpty(i.phone) ? "" : i.phone.Trim(),
                            //address = i.addr_address1,
                            //city = _ext == null ? "" : _ext.value,
                            //address = i.addr_address1 + " " + i.addr_address2,
                            //zip = db.ref_zip.Find( i.addr_rel_ref_zip_id).zip,
                            //city,
                            //state,
                            //email = string.IsNullOrEmpty(i.email) ? "" : i.email,
                            //gender = string.IsNullOrEmpty(i.gender) ? "" : i.gender.ToUpper(),
                            //salutation = string.IsNullOrEmpty(i.salutation) ? "" : i.salutation.Trim(),
                            //create_by = (_create_fname == null ? "" : (_create_fname.name_first) + " " + (_create_lname == null ? "" : _create_lname.name_last)).Trim(),
                            //update_by = (_create_fname == null ? "" : (_create_fname.name_first) + " " + (_create_lname == null ? "" : _create_lname.name_last)).Trim(),
                            //create_date = i.dt_create == null ? "" : i.dt_create.ToString(),
                            //update_date = i.dt_update == null ? "" : i.dt_update.ToString(),
                            status = _stat == null ? "" : _stat.dname.Trim(),
                            image = _image == null ? "" : "https://s3-ap-southeast-1.amazonaws.com/hsrecs/images/" + _image.value// add image profile
                        });
                    }

                }


                var ret1 = JsonConvert.SerializeObject(patient);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                //msg = "The patient is updated.";
                return Json(new { data = json1, message = patient.Count() + (patient.Count() > 1 ? " Records found!" : " Record found!"), success = true });
            }

            msg = "The authorization header is either not valid or isn't Basic.";
            return Json(new { data = new string[] { }, message = msg, success = false });
        }


        [HttpPut]
        [System.Web.Http.Route("wr/patient")]
        public IHttpActionResult wrPutpatient(u_patient patient1)
        {
            // Required Parameter :-user_id, first_name, lastname, insuance_type, use_patient_insurance(0 / 1), primary_type(0 / 1),patient_id
            // Return required message
            long patient = patient1.patient_id;
            string patient_id = patient1.patient_id == 0 ? "" : patient1.patient_id.ToString(),
                user_id = patient1.user_id == 0 ? "" : patient1.user_id.ToString();
            string insurance_id = patient1.insurance_id == 0 ? "" : patient1.insurance_id.ToString();
            //int primary_type = 0, use_patient_insurance = 0;


            // string acck = "deftsoftapikey", address = "", zip = "", phone = "";
            //string email = "", gender = "", salutation = "", status = "", device_type = "", device_token = "";

            //nt keyCnt = 0;

            string msg = "", use_insurance = "";
            DateTime dtNow = DateTime.Now;

            #region
            //string root = HttpContext.Current.Server.MapPath("~/Temp");
            //var provider = new MultipartFormDataStreamProvider(root);
            //await Request.Content.ReadAsMultipartAsync(provider);
            //foreach (var key in provider.FormData.AllKeys)
            //{
            //    keyCnt++;
            //    foreach (var val in provider.FormData.GetValues(key))
            //    {
            //        switch (key)
            //        {
            //            case "user_id":
            //                IsRequired(key, val, 1);
            //                //bool isUser = long.TryParse(val, out user_id);
            //                user_id = val;

            //                break;
            //            case "patient_id":
            //                IsRequired(key, val, 1);
            //                patient_id = val;
            //                bool bpatient = long.TryParse(val, out patient);

            //                break;
            //            case "firstname":
            //                //IsRequired(key, val, 1);
            //                firstname = val; break;

            //            case "lastname":
            //                //IsRequired(key, val, 1);
            //                lastname = val; break;
            //            case "insurance_id":
            //                //IsRequired(key, val, 1);
            //                insurance_id = val; break;

            //            //int use_patient_insurance = 0, int primary_type = 0, 
            //            case "is_using_primary_patient_insurance":
            //                //IsRequired(key, val, 1);
            //                //bool use_insurance = int.TryParse(val, out use_patient_insurance);
            //                use_insurance = val;
            //                break;

            //            //case "primary_type":
            //            //    //IsRequired(key, val, 1);
            //            //    //bool bprimary = int.TryParse(val, out primary_type);
            //            //   // break;

            //            case "is_prime":
            //                //bool bprimary = bool.TryParse(val, out is_prime);
            //                //bool isVal2 = int.TryParse(val, out nprimary_type);
            //                switch (val.ToLower())
            //                {
            //                    case "true":
            //                        is_prime = true;
            //                        break;

            //                    case "false":
            //                        is_prime = false;
            //                        break;
            //                    default:
            //                        msg = "Cannot parse value under parameter name: " + key;
            //                        return Json(new { data = new string[] { }, message = msg, success = false });
            //                }

            //                break;

            //            //case "address": address = val; break;
            //            //case "zip": zip = val;  break;
            //            //case "phone": phone = val; break;
            //            //case "email": email = val; break;
            //            //case "gender": gender = val; break;
            //            //case "salutation":salutation = val; break;
            //            case "status": status = val; break;

            //            case "device_type":
            //                device_type = val; break;

            //            case "device_token":
            //                device_token = val; break;

            //            default:
            //                msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
            //                return Json(new { data = new string[] { }, message = msg, success = false });
            //        }
            //    }
            //}

            #endregion

            //firstname, lastname, insurance_type, primary_type 

            custom.IsRequired("user_id", user_id, 2);
            custom.IsRequired("patient_id", patient_id, 2);
            //IsRequired("firstname", firstname, 2);
            //IsRequired("lastname", lastname, 2);
            //IsRequired("insurance_type", insurance_type, 2);
            //IsRequired("use_patient_insurance", use_patient_insurance.ToString(), 2);
            //IsRequired("primary_type", primary_type.ToString(), 2);

            //if (!string.IsNullOrEmpty(use_insurance) || !string.IsNullOrEmpty(insurance_id))
            //{
            if (!string.IsNullOrEmpty(use_insurance))
            {
                custom.IsRequired("use_user_insurance", use_insurance, 1);
            }
            if (!string.IsNullOrEmpty(insurance_id))
            {
                custom.IsRequired("insurance_id", insurance_id, 1);
            }
            //}
            //else
            //{
            //    errmsg += "either insurance_id or is_using_primary_patient_insurance required parameter \r\n";
            //    return Json(new { data = new string[] { }, message = errmsg, success = false });
            //}

            if (custom.haserror)
            {
                return Json(new { data = new string[] { }, message = custom.errmsg, success = false });
            }

            long nUser_id = 0;
            bool bUser = long.TryParse(user_id, out nUser_id);
            if (!bUser)
            {
                return Json(new { data = new string[] { }, message = "Invalid parameter value: user_id", success = false });
            }

            var _soul = dbEntity.SOULs.Find(patient);

            #region
            ////xxxxxxxxxxxxx
            //// if only parameters are passed, this would be for deletion
            //if (keyCnt == 2)
            //{
            //    if (_soul != null)
            //    {
            //        var ref_status = dbEntity.ref_status_type.Where(a => a.dname == "SOUL").FirstOrDefault().ref_status.Where(b => b.dname.ToLower() == "deleted").FirstOrDefault();
            //        _soul.rel_ref_status_id = ref_status.id;
            //        _soul.update_by__USER_id = Convert.ToInt64(user_id);
            //        _soul.dt_update = dt;
            //        dbEntity.Entry(_soul).State = System.Data.Entity.EntityState.Modified;
            //        dbEntity.SaveChanges();
            //    }
            //    msg = "Patient record successfully deleted.";
            //    return Json(new { message = msg, success = true });
            //}
            //// xxxxxxxxxxxxxxxxxxx
            #endregion


            //20170802 var _soul = dbEntity.SOULs.Find(patient);
            if (_soul != null)
            {
                patient1.user_id = nUser_id;
                return _updatePatient(patient1);

            }

            msg = "No result found.";
            return Json(new { data = new string[] { }, message = msg, success = false });


            #region
            //// ***************************************
            //msg = "";
            //if (acck == "deftsoftapikey")
            //{
            //    try {
            //        DateTime dt = DateTime.Now;

            //        //SOUL s = new SOUL();
            //        //var _soul = dbEntity.SOULs.Find(patient_id);

            //        if (!string.IsNullOrEmpty(firstname))
            //        {
            //            //_soul = _soul.Where(a => a.name_first.ToLower() == firstname.ToLower());
            //            _soul.name_first = firstname;
            //        }

            //        if (!string.IsNullOrEmpty(lastname))
            //        {
            //            //_soul = _soul.Where(a => a.name_last.ToLower() == lastname.ToLower());
            //            _soul.name_last = lastname;
            //        }

            //        // insurance_type, 
            //        // use_patient_insurance,
            //        // primary_type

            //        //  phone  
            //        if (!string.IsNullOrEmpty(phone))
            //        {
            //            //_soul = _soul.Where(a => a.phone == phone);
            //            _soul.phone = phone;
            //        }

            //        //  addr_address1
            //        if (!string.IsNullOrEmpty(address))
            //        {
            //            //_soul = _soul.Where(a => a.addr_address1.ToLower() == address.ToLower() ||
            //            //                    a.addr_address2.ToLower() == address.ToLower());
            //            _soul.addr_address1 = address;
            //        }

            //        //  addr_address2
            //        //s.addr_address2 = address2;
            //        //  addr_rel_ref_zip_id
            //        if (!string.IsNullOrEmpty(zip))
            //        {
            //            var _zip = dbEntity.ref_zip.Where(a => a.zip == zip).FirstOrDefault();
            //            if (_zip != null)
            //            {
            //                //_soul = _soul.Where(a => a.addr_rel_ref_zip_id == _zip.id);
            //                _soul.addr_rel_ref_zip_id = _zip.id;
            //            }
            //        }

            //        //  email
            //        if (!string.IsNullOrEmpty(email))
            //        {
            //            //_soul = _soul.Where(a => a.email == email.ToLower());
            //            _soul.email = email.ToLower();
            //        }

            //        //  gender
            //        if (!string.IsNullOrEmpty(gender))
            //        {
            //            string _gender = gender.Substring(0, 1).ToLower();
            //            //_soul = _soul.Where(a => a.gender == _gender);
            //            _soul.gender = _gender;
            //        }

            //        //  salutation
            //        if (!string.IsNullOrEmpty(salutation))
            //        {
            //            //_soul = _soul.Where(a => a.salutation.ToLower() == salutation.ToLower());
            //            _soul.salutation = salutation;
            //        }

            //        // status
            //        if (!string.IsNullOrEmpty(status))
            //        {
            //            var ref_status = dbEntity.ref_status_type.Where(a => a.dname == "SOUL").FirstOrDefault().ref_status.Where(b => b.dname.ToLower() == status.ToLower()).FirstOrDefault();
            //            if(ref_status != null)
            //                _soul.rel_ref_status_id = ref_status.id;

            //        }

            //        dbEntity.Entry(_soul).State = System.Data.Entity.EntityState.Modified;
            //        dbEntity.SaveChanges();

            //        msg = "The patient record is updated.";
            //        return Json(new { data = "", message = msg, success = true });
            //    }
            //    catch (Exception ex)
            //    {
            //        return Json(new { data = "", message = ex.Message, success = false});
            //    }


            //}

            #endregion

            msg = "The authorization header is either not valid or isn't Basic.";
            return Json(new { data = new string[] { }, message = msg, success = false });
        }

        // update patient
        [HttpPut]
        [System.Web.Http.Route("patient")]
        public async Task<IHttpActionResult> putPatient()
        {
            // Required Parameter :-user_id, first_name, lastname, insuance_type, use_patient_insurance(0 / 1), primary_type(0 / 1),patient_id
            // Return required message
            long patient = 0;
            string patient_id = "", user_id = "";
            string first_name = "", last_name = "", insurance_id = "";
            // int primary_type = 0, use_patient_insurance = 0;

            bool is_prime = false;
            //string acck = "deftsoftapikey", address = "", zip = "", phone = "", email = "", gender = "", salutation = "",;
            string status = "", device_type = "", device_token = "";

            int keyCnt = 0;

            string msg = "", use_insurance = "";
            DateTime dtNow = DateTime.Now;
            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);

            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var key in provider.FormData.AllKeys)
            {
                keyCnt++;
                foreach (var val in provider.FormData.GetValues(key))
                {
                    switch (key)
                    {
                        case "user_id":
                            custom.IsRequired(key, val, 1);
                            //bool isUser = long.TryParse(val, out user_id);
                            user_id = val;

                            break;
                        case "patient_id":
                            custom.IsRequired(key, val, 1);
                            patient_id = val;
                            bool bpatient = long.TryParse(val, out patient);

                            break;
                        case "first_name":
                            //IsRequired(key, val, 1);
                            first_name = val; break;

                        case "last_name":
                            //IsRequired(key, val, 1);
                            last_name = val; break;
                        case "insurance_id":
                            //IsRequired(key, val, 1);
                            insurance_id = val; break;

                        //int use_patient_insurance = 0, int primary_type = 0, 
                        case "is_using_primary_patient_insurance":
                            //IsRequired(key, val, 1);
                            //bool use_insurance = int.TryParse(val, out use_patient_insurance);
                            use_insurance = val;
                            break;

                        //case "primary_type":
                        //    //IsRequired(key, val, 1);
                        //    //bool bprimary = int.TryParse(val, out primary_type);
                        //   // break;

                        case "is_prime":
                            //bool bprimary = bool.TryParse(val, out is_prime);
                            //bool isVal2 = int.TryParse(val, out nprimary_type);
                            switch (val.ToLower())
                            {
                                case "true":
                                    is_prime = true;
                                    break;

                                case "false":
                                    is_prime = false;
                                    break;
                                default:
                                    msg = "Cannot parse value under parameter name: " + key;
                                    return Json(new { data = new string[] { }, message = msg, success = false });
                            }

                            break;

                        //case "address": address = val; break;
                        //case "zip": zip = val;  break;
                        //case "phone": phone = val; break;
                        //case "email": email = val; break;
                        //case "gender": gender = val; break;
                        //case "salutation":salutation = val; break;
                        case "status": status = val; break;

                        case "device_type":
                            device_type = val; break;

                        case "device_token":
                            device_token = val; break;

                        default:
                            msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                            return Json(new { data = new string[] { }, message = msg, success = false });
                    }
                }
            }

            //firstname, lastname, insurance_type, primary_type 

            custom.IsRequired("user_id", user_id, 2);
            custom.IsRequired("patient_id", patient_id, 2);
            //IsRequired("firstname", firstname, 2);
            //IsRequired("lastname", lastname, 2);
            //IsRequired("insurance_type", insurance_type, 2);
            //IsRequired("use_patient_insurance", use_patient_insurance.ToString(), 2);
            //IsRequired("primary_type", primary_type.ToString(), 2);

            //if (!string.IsNullOrEmpty(use_insurance) || !string.IsNullOrEmpty(insurance_id))
            //{
            if (!string.IsNullOrEmpty(use_insurance))
            {
                custom.IsRequired("use_user_insurance", use_insurance, 1);
            }
            if (!string.IsNullOrEmpty(insurance_id))
            {
                custom.IsRequired("insurance_id", insurance_id, 1);
            }
            //}
            //else
            //{
            //    errmsg += "either insurance_id or is_using_primary_patient_insurance required parameter \r\n";
            //    return Json(new { data = new string[] { }, message = errmsg, success = false });
            //}

            if (custom.haserror)
            {
                return Json(new { data = new string[] { }, message = custom.errmsg, success = false });
            }

            long nUser_id = 0;
            bool bUser = long.TryParse(user_id, out nUser_id);
            if (!bUser)
            {
                return Json(new { data = new string[] { }, message = "Invalid parameter value: user_id", success = false });
            }

            var _soul = dbEntity.SOULs.Find(patient);

            if (_soul != null)
            {
                u_patient patient1 = new u_patient()
                {
                    user_id = Convert.ToInt64(user_id),
                    patient_id = Convert.ToInt64(patient_id),
                    first_name = first_name,
                    last_name = last_name,
                    insurance_id = insurance_id == "" ? 0 : Convert.ToInt64(insurance_id),
                    is_using_primary_patient_insurance = use_insurance,
                    is_prime = is_prime.ToString(),
                    status = status,
                    device_type = device_type,
                    device_token = device_token
                };

                return _updatePatient(patient1);
            }
            else
            {
                return Json(new { data = new string[] { }, message = "Invalid parameter value: patient_id", success = false });
            }


            #region
            ////xxxxxxxxxxxxx
            //// if only parameters are passed, this would be for deletion
            //if (keyCnt == 2)
            //{
            //    if (_soul != null)
            //    {
            //        var ref_status = dbEntity.ref_status_type.Where(a => a.dname == "SOUL").FirstOrDefault().ref_status.Where(b => b.dname.ToLower() == "deleted").FirstOrDefault();
            //        _soul.rel_ref_status_id = ref_status.id;
            //        _soul.update_by__USER_id = Convert.ToInt64(user_id);
            //        _soul.dt_update = dt;
            //        dbEntity.Entry(_soul).State = System.Data.Entity.EntityState.Modified;
            //        dbEntity.SaveChanges();
            //    }
            //    msg = "Patient record successfully deleted.";
            //    return Json(new { message = msg, success = true });
            //}
            //// xxxxxxxxxxxxxxxxxxx
            #endregion

            //if (is_prime)
            //{
            //    // (start)Get user_id's username
            //    var u_user = dbEntity.USERs.Find(Convert.ToInt64(user_id));

            //    var u_patient = dbEntity.SOULs.Where(a => a.create_by__USER_id == u_user.id || a.email == u_user.username);
            //    if (is_prime)
            //    {
            //        if (u_patient.Count() > 0)
            //        {
            //            foreach (var i in u_patient)
            //            {
            //                if (i.is_prime == true)
            //                {
            //                    i.is_prime = false;
            //                    dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
            //                }
            //            }
            //            dbEntity.SaveChanges();
            //        }
            //    }
            //    // (end)Get user_id's username
            //}

            //20170802 var _soul = dbEntity.SOULs.Find(patient);
            if (_soul != null)
            {

                //if (!string.IsNullOrEmpty(firstname))
                //    _soul.name_first = firstname;

                //if (!string.IsNullOrEmpty(lastname))
                //    _soul.name_last = lastname;

                //_soul.is_prime = is_prime;

                // insurance_type, 
                // use_patient_insurance,
                // primary_type

                //if (!string.IsNullOrEmpty(phone))
                //    _soul.phone = phone;

                //if (!string.IsNullOrEmpty(address))
                //    _soul.addr_address1 = address;

                //var _zip = dbEntity.ref_zip.Where(a => a.zip == zip).FirstOrDefault();
                //if (_zip != null)
                //{
                //   _soul.addr_rel_ref_zip_id = _zip.id;
                //}

                //if (!string.IsNullOrEmpty(email))
                //    _soul.email = email.ToLower();

                //if (!string.IsNullOrEmpty(gender))
                //{
                //    string _gender = gender.Substring(0, 1).ToLower();
                //    _soul.gender = _gender;
                //}

                //if (!string.IsNullOrEmpty(salutation))
                //    _soul.salutation = salutation;
                var soul_ext = dbEntity.SOUL_ext.Where(a => a.rel_SOUL_id == _soul.id);
                bool hasInsurance = false, hasUseUser = false, hasDevType = false, hasDevToken = false;

                ////bool hasPrimary
                //if (soul_ext.Count() > 0)
                //{
                //    foreach(var i in soul_ext)
                //    {
                //        switch (i.attr_name) {
                //            case "insurance_id":

                //                if (!string.IsNullOrEmpty(insurance_id))
                //                {
                //                    //var ext_ins_type = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == i.rel_SOUL_id && b.attr_name == "insurance_id").FirstOrDefault();
                //                    i.value = insurance_id;

                //                    i.dt_update = dtNow;
                //                    i.update_by__USER_id = nUser_id;
                //                    dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                //                    hasInsurance = true;
                //                }


                //                break;
                //            case "use_user_insurance":
                //                // Is Using Primary Patient Insurance

                //                if (!string.IsNullOrEmpty(use_insurance))
                //                {
                //                    //var ext_use_pat = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == i.rel_SOUL_id && b.attr_name == "use_user_insurance").FirstOrDefault();
                //                    i.value = use_insurance;

                //                    i.dt_update = dtNow;
                //                    i.update_by__USER_id = nUser_id;
                //                    dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                //                    hasUseUser = true;
                //                }

                //                break;

                //            case "device_type":
                //                //device_type = val; 
                //                if (!string.IsNullOrEmpty(device_type))
                //                {
                //                    //var ext_use_pat = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == i.rel_SOUL_id && b.attr_name == "device_type").FirstOrDefault();
                //                    i.value = device_type;

                //                    i.dt_update = dtNow;
                //                    i.update_by__USER_id = nUser_id;
                //                    dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                //                    hasDevType = true;
                //                }
                //                break;

                //            case "device_token":
                //                //device_token = val; 
                //                if (!string.IsNullOrEmpty(device_token))
                //                {
                //                    //var ext_use_pat = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == i.rel_SOUL_id && b.attr_name == "use_user_insurance").FirstOrDefault();
                //                    i.value = device_token;

                //                    i.dt_update = dtNow;
                //                    i.update_by__USER_id = nUser_id;
                //                    dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                //                    hasDevToken = true;
                //                }
                //                break;

                //                //case "primary_type":
                //                //    var ext_prim_type = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == i.rel_SOUL_id && b.attr_name == "use_patient_insurance").FirstOrDefault();
                //                //    ext_prim_type.value = primary_type.ToString();
                //                //    ext_prim_type.dt_update = dtNow;
                //                //    ext_prim_type.update_by__USER_id = user_id;
                //                //    dbEntity.Entry(ext_prim_type).State = System.Data.Entity.EntityState.Modified;
                //                //    break;

                //        }

                //    }
                //    dbEntity.SaveChanges();
                //}

                //1   double
                //2   string
                //3   long
                //4   bool
                //5   datetime

                #region
                //// insurance_id
                //if (!hasInsurance)
                //{
                //       if (!string.IsNullOrEmpty(insurance_id))
                //        {
                //            SOUL_ext ext_city = new SOUL_ext();
                //            ext_city.rel_SOUL_id = _soul.id;
                //            ext_city.attr_name = "insurance_id";
                //            ext_city.dname = "Insurance ID";
                //            ext_city.value = insurance_id;
                //            ext_city.rel_ref_datatype_id = 3;
                //            ext_city.create_by__USER_id = nUser_id;
                //            ext_city.dt_create = dt;
                //            dbEntity.SOUL_ext.Add(ext_city);
                //            dbEntity.SaveChanges();
                //        }
                //}

                //// Is_Using_Primary_Patient_Insurance
                //if (!hasUseUser)
                //{
                //    if (!string.IsNullOrEmpty(use_insurance))
                //    {
                //        SOUL_ext ext_use = new SOUL_ext();
                //        ext_use.rel_SOUL_id = _soul.id;
                //        ext_use.attr_name = "use_user_insurance";
                //        ext_use.dname = "Is Using Primary Patient Insurance";
                //        ext_use.value = use_insurance;
                //        ext_use.rel_ref_datatype_id = 4;
                //        ext_use.create_by__USER_id = nUser_id;
                //        ext_use.dt_create = dt;
                //        dbEntity.SOUL_ext.Add(ext_use);
                //        dbEntity.SaveChanges();
                //    }
                //}

                //// Device_token
                //if (!hasDevToken)
                //{
                //    if (!string.IsNullOrEmpty(device_token))
                //    {
                //        SOUL_ext ext_use = new SOUL_ext();
                //        ext_use.rel_SOUL_id = _soul.id;
                //        ext_use.attr_name = "device_token";
                //        ext_use.dname = "Device Token";
                //        ext_use.value = device_token;
                //        ext_use.rel_ref_datatype_id = 4;
                //        ext_use.create_by__USER_id = nUser_id;
                //        ext_use.dt_create = dt;
                //        dbEntity.SOUL_ext.Add(ext_use);
                //        dbEntity.SaveChanges();
                //    }
                //}
                //// Device_type
                //if (!hasDevType)
                //{
                //    if (!string.IsNullOrEmpty(device_type))
                //    {
                //        SOUL_ext ext_use = new SOUL_ext();
                //        ext_use.rel_SOUL_id = _soul.id;
                //        ext_use.attr_name = "device_type";
                //        ext_use.dname = "Device Type";
                //        ext_use.value = device_type;
                //        ext_use.rel_ref_datatype_id = 4;
                //        ext_use.create_by__USER_id = nUser_id;
                //        ext_use.dt_create = dt;
                //        dbEntity.SOUL_ext.Add(ext_use);
                //        dbEntity.SaveChanges();
                //    }
                //}


                //if (!string.IsNullOrEmpty(status))
                //{
                //    var ref_status = dbEntity.ref_status_type.Where(a => a.dname == "SOUL").FirstOrDefault().ref_status.Where(b => b.dname.ToLower() == status.ToLower()).FirstOrDefault();
                //    if (ref_status != null)
                //        _soul.rel_ref_status_id = ref_status.id;
                //}
                //dbEntity.Entry(_soul).State = System.Data.Entity.EntityState.Modified;
                //dbEntity.SaveChanges();
                #endregion


                msg = "The patient record is updated.";
                return Json(new { data = new string[] { }, message = msg, success = true });
            }

            msg = "No result found.";
            return Json(new { data = new string[] { }, message = msg, success = false });


            #region
            //// ***************************************
            //msg = "";
            //if (acck == "deftsoftapikey")
            //{
            //    try {
            //        DateTime dt = DateTime.Now;

            //        //SOUL s = new SOUL();
            //        //var _soul = dbEntity.SOULs.Find(patient_id);

            //        if (!string.IsNullOrEmpty(firstname))
            //        {
            //            //_soul = _soul.Where(a => a.name_first.ToLower() == firstname.ToLower());
            //            _soul.name_first = firstname;
            //        }

            //        if (!string.IsNullOrEmpty(lastname))
            //        {
            //            //_soul = _soul.Where(a => a.name_last.ToLower() == lastname.ToLower());
            //            _soul.name_last = lastname;
            //        }

            //        // insurance_type, 
            //        // use_patient_insurance,
            //        // primary_type

            //        //  phone  
            //        if (!string.IsNullOrEmpty(phone))
            //        {
            //            //_soul = _soul.Where(a => a.phone == phone);
            //            _soul.phone = phone;
            //        }

            //        //  addr_address1
            //        if (!string.IsNullOrEmpty(address))
            //        {
            //            //_soul = _soul.Where(a => a.addr_address1.ToLower() == address.ToLower() ||
            //            //                    a.addr_address2.ToLower() == address.ToLower());
            //            _soul.addr_address1 = address;
            //        }

            //        //  addr_address2
            //        //s.addr_address2 = address2;
            //        //  addr_rel_ref_zip_id
            //        if (!string.IsNullOrEmpty(zip))
            //        {
            //            var _zip = dbEntity.ref_zip.Where(a => a.zip == zip).FirstOrDefault();
            //            if (_zip != null)
            //            {
            //                //_soul = _soul.Where(a => a.addr_rel_ref_zip_id == _zip.id);
            //                _soul.addr_rel_ref_zip_id = _zip.id;
            //            }
            //        }

            //        //  email
            //        if (!string.IsNullOrEmpty(email))
            //        {
            //            //_soul = _soul.Where(a => a.email == email.ToLower());
            //            _soul.email = email.ToLower();
            //        }

            //        //  gender
            //        if (!string.IsNullOrEmpty(gender))
            //        {
            //            string _gender = gender.Substring(0, 1).ToLower();
            //            //_soul = _soul.Where(a => a.gender == _gender);
            //            _soul.gender = _gender;
            //        }

            //        //  salutation
            //        if (!string.IsNullOrEmpty(salutation))
            //        {
            //            //_soul = _soul.Where(a => a.salutation.ToLower() == salutation.ToLower());
            //            _soul.salutation = salutation;
            //        }

            //        // status
            //        if (!string.IsNullOrEmpty(status))
            //        {
            //            var ref_status = dbEntity.ref_status_type.Where(a => a.dname == "SOUL").FirstOrDefault().ref_status.Where(b => b.dname.ToLower() == status.ToLower()).FirstOrDefault();
            //            if(ref_status != null)
            //                _soul.rel_ref_status_id = ref_status.id;

            //        }

            //        dbEntity.Entry(_soul).State = System.Data.Entity.EntityState.Modified;
            //        dbEntity.SaveChanges();

            //        msg = "The patient record is updated.";
            //        return Json(new { data = "", message = msg, success = true });
            //    }
            //    catch (Exception ex)
            //    {
            //        return Json(new { data = "", message = ex.Message, success = false});
            //    }


            //}
            #endregion


            msg = "The authorization header is either not valid or isn't Basic.";
            return Json(new { data = new string[] { }, message = msg, success = false });
        }

        private IHttpActionResult _updatePatient(u_patient patient1)
        {
            var _soul = dbEntity.SOULs.Find(patient1.patient_id);

            bool is_prime = patient1.is_prime == "true" ? true : false;
            if (is_prime)
            {
                // (start)Get user_id's username
                var u_user = dbEntity.USERs.Find(Convert.ToInt64(patient1.user_id));

                var u_patient = dbEntity.SOULs.Where(a => a.create_by__USER_id == u_user.id || a.email == u_user.username);
                if (is_prime)
                {
                    if (u_patient.Count() > 0)
                    {
                        foreach (var i in u_patient)
                        {
                            if (i.is_prime == true)
                            {
                                i.is_prime = false;
                                dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                        dbEntity.SaveChanges();
                    }
                }
                // (end)Get user_id's username
            }


            if (!string.IsNullOrEmpty(patient1.first_name))
                _soul.name_first = patient1.first_name;

            if (!string.IsNullOrEmpty(patient1.last_name))
                _soul.name_last = patient1.last_name;

            _soul.is_prime = is_prime;

            if (!string.IsNullOrEmpty(patient1.status))
            {
                var ref_status = dbEntity.ref_status_type.Where(a => a.dname == "SOUL").FirstOrDefault().ref_status.Where(b => b.dname.ToLower() == patient1.status.ToLower()).FirstOrDefault();
                if (ref_status != null)
                    _soul.rel_ref_status_id = ref_status.id;

            }

            dbEntity.Entry(_soul).State = System.Data.Entity.EntityState.Modified;
            dbEntity.SaveChanges();

            #region
            // insurance_type, 
            // use_patient_insurance,
            // primary_type

            //if (!string.IsNullOrEmpty(phone))
            //    _soul.phone = phone;

            //if (!string.IsNullOrEmpty(address))
            //    _soul.addr_address1 = address;

            //var _zip = dbEntity.ref_zip.Where(a => a.zip == zip).FirstOrDefault();
            //if (_zip != null)
            //{
            //   _soul.addr_rel_ref_zip_id = _zip.id;
            //}

            //if (!string.IsNullOrEmpty(email))
            //    _soul.email = email.ToLower();

            //if (!string.IsNullOrEmpty(gender))
            //{
            //    string _gender = gender.Substring(0, 1).ToLower();
            //    _soul.gender = _gender;
            //}

            //if (!string.IsNullOrEmpty(salutation))
            //    _soul.salutation = salutation;
            #endregion


            #region
            ////bool hasPrimary
            //if (soul_ext.Count() > 0)
            //{
            //    foreach (var i in soul_ext)
            //    {
            //        switch (i.attr_name)
            //        {
            //            case "insurance_id":

            //                if (!string.IsNullOrEmpty(insurance_id))
            //                {
            //                    //var ext_ins_type = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == i.rel_SOUL_id && b.attr_name == "insurance_id").FirstOrDefault();
            //                    i.value = insurance_id;

            //                    i.dt_update = dtNow;
            //                    i.update_by__USER_id = nUser_id;
            //                    dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
            //                    hasInsurance = true;
            //                }


            //                break;
            //            case "use_user_insurance":
            //                // Is Using Primary Patient Insurance

            //                if (!string.IsNullOrEmpty(use_insurance))
            //                {
            //                    //var ext_use_pat = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == i.rel_SOUL_id && b.attr_name == "use_user_insurance").FirstOrDefault();
            //                    i.value = use_insurance;

            //                    i.dt_update = dtNow;
            //                    i.update_by__USER_id = nUser_id;
            //                    dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
            //                    hasUseUser = true;
            //                }

            //                break;

            //            case "device_type":
            //                //device_type = val; 
            //                if (!string.IsNullOrEmpty(device_type))
            //                {
            //                    //var ext_use_pat = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == i.rel_SOUL_id && b.attr_name == "device_type").FirstOrDefault();
            //                    i.value = device_type;

            //                    i.dt_update = dtNow;
            //                    i.update_by__USER_id = nUser_id;
            //                    dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
            //                    hasDevType = true;
            //                }
            //                break;

            //            case "device_token":
            //                //device_token = val; 
            //                if (!string.IsNullOrEmpty(device_token))
            //                {
            //                    //var ext_use_pat = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == i.rel_SOUL_id && b.attr_name == "use_user_insurance").FirstOrDefault();
            //                    i.value = device_token;

            //                    i.dt_update = dtNow;
            //                    i.update_by__USER_id = nUser_id;
            //                    dbEntity.Entry(i).State = System.Data.Entity.EntityState.Modified;
            //                    hasDevToken = true;
            //                }
            //                break;

            //                //case "primary_type":
            //                //    var ext_prim_type = dbEntity.SOUL_ext.Where(b => b.rel_SOUL_id == i.rel_SOUL_id && b.attr_name == "use_patient_insurance").FirstOrDefault();
            //                //    ext_prim_type.value = primary_type.ToString();
            //                //    ext_prim_type.dt_update = dtNow;
            //                //    ext_prim_type.update_by__USER_id = user_id;
            //                //    dbEntity.Entry(ext_prim_type).State = System.Data.Entity.EntityState.Modified;
            //                //    break;

            //        }

            //    }
            //    dbEntity.SaveChanges();
            //}
            #endregion

            //1   double
            //2   string
            //3   long
            //4   bool
            //5   datetime

            // insurance_id
            if (patient1.insurance_id > 0)
            {
                bool i = Validation.saveSOUL_ext("insurance_id", "Insurance ID", patient1.insurance_id.ToString(), _soul.id, patient1.user_id);

                #region
                //if (!string.IsNullOrEmpty(insurance_id))
                //{
                //    SOUL_ext ext_city = new SOUL_ext();
                //    ext_city.rel_SOUL_id = _soul.id;
                //    ext_city.attr_name = "insurance_id";
                //    ext_city.dname = "Insurance ID";
                //    ext_city.value = insurance_id;
                //    ext_city.rel_ref_datatype_id = 3;
                //    ext_city.create_by__USER_id = nUser_id;
                //    ext_city.dt_create = dt;
                //    dbEntity.SOUL_ext.Add(ext_city);
                //    dbEntity.SaveChanges();
                //}
                #endregion

            }

            // Is_Using_Primary_Patient_Insurance
            if (!string.IsNullOrEmpty(patient1.is_using_primary_patient_insurance))
            {
                bool i = Validation.saveSOUL_ext("use_user_insurance", "Is Using Primary Patient Insurance", patient1.is_using_primary_patient_insurance, _soul.id, patient1.user_id);

                #region
                //if (!string.IsNullOrEmpty(use_insurance))
                //{
                //    SOUL_ext ext_use = new SOUL_ext();
                //    ext_use.rel_SOUL_id = _soul.id;
                //    ext_use.attr_name = "use_user_insurance";
                //    ext_use.dname = "Is Using Primary Patient Insurance";
                //    ext_use.value = use_insurance;
                //    ext_use.rel_ref_datatype_id = 4;
                //    ext_use.create_by__USER_id = nUser_id;
                //    ext_use.dt_create = dt;
                //    dbEntity.SOUL_ext.Add(ext_use);
                //    dbEntity.SaveChanges();
                //}
                #endregion
            }

            // Device_token
            if (!string.IsNullOrEmpty(patient1.device_token))
            {
                bool i = Validation.saveSOUL_ext("device_token", "Device Token", patient1.device_token, _soul.id, patient1.user_id);

                #region
                //if (!string.IsNullOrEmpty(device_token))
                //{
                //    SOUL_ext ext_use = new SOUL_ext();
                //    ext_use.rel_SOUL_id = _soul.id;
                //    ext_use.attr_name = "device_token";
                //    ext_use.dname = "Device Token";
                //    ext_use.value = device_token;
                //    ext_use.rel_ref_datatype_id = 4;
                //    ext_use.create_by__USER_id = nUser_id;
                //    ext_use.dt_create = dt;
                //    dbEntity.SOUL_ext.Add(ext_use);
                //    dbEntity.SaveChanges();
                //}
                #endregion
            }

            // Device_type
            if (!string.IsNullOrEmpty(patient1.device_type))
            {
                bool i = Validation.saveSOUL_ext("device_type", "Device Type", patient1.device_type, _soul.id, patient1.user_id);

                #region
                //if (!string.IsNullOrEmpty(device_type))
                //{
                //    SOUL_ext ext_use = new SOUL_ext();
                //    ext_use.rel_SOUL_id = _soul.id;
                //    ext_use.attr_name = "device_type";
                //    ext_use.dname = "Device Type";
                //    ext_use.value = device_type;
                //    ext_use.rel_ref_datatype_id = 4;
                //    ext_use.create_by__USER_id = nUser_id;
                //    ext_use.dt_create = dt;
                //    dbEntity.SOUL_ext.Add(ext_use);
                //    dbEntity.SaveChanges();
                //}

                #endregion

            }

            // date of birth
            if (!string.IsNullOrEmpty(patient1.dob))
            {
                //12/12/1990
                string s = Validation.validateDate(patient1.dob);
                if (s.Length > 0)
                {
                    bool i = Validation.saveSOUL_ext("dob", "Date Of Birth", s, _soul.id, patient1.user_id);
                }
                else
                { return Json(new { data = new string[] { }, message = "Invalid dob value.", success = false }); }
            }


            string msg = "The patient record is updated.";
            return Json(new { data = new string[] { }, message = msg, success = true });

        }


        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("patient/profile_pic")]
        public async Task<IHttpActionResult> uploadprofilepic1()
        {
            //var isauthorized = authorized.HSRequest(Request);
            //if (isauthorized.StatusCode == HttpStatusCode.Forbidden)
            //{
            //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized)
            //    {
            //        StatusCode = HttpStatusCode.Unauthorized,
            //    });
            //}

            string user_id = null, patient_id = null, filename = "";
            byte[] bytes = { };

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }


            string root = HttpContext.Current.Server.MapPath("~/Content/Temp");
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
                            custom.IsRequired(key, val, 1);
                            user_id = val;
                        }
                        else if (key == "patient_id")
                        {
                            custom.IsRequired(key, val, 1);
                            patient_id = val;
                        }
                        else
                        {
                            return Json(new { message = "Object reference not set to an instance of an object. Invalid parameter name: " + key, success = false });
                        }
                    }
                }

                custom.IsRequired("user_id", user_id, 2);
                custom.IsRequired("patient_id", patient_id, 2);
                bool is_upload = uploadpic(provider, patient_id);
                custom.IsRequired("image", new_filename, 2);

                long soul_id = Convert.ToInt64(patient_id);

                if (custom.haserror)
                {
                    return Json(new { message = custom.errmsg, success = false });
                }
                else if (!is_upload)
                {
                    return Json(new { message = "Error uploading image.", success = false });
                }
                else
                {
                    bool hassave = false, hasupdate = false;

                    //race
                    var photo_SOUL = (from a in dbEntity.SOUL_ext
                                      where (a.rel_SOUL_id == soul_id && a.attr_name == "image")
                                      select a);
                    if (photo_SOUL.Count() > 0)
                    {
                        SOUL_ext updateSOUL_photo = dbEntity.SOUL_ext.First(a => a.rel_SOUL_id == soul_id && a.attr_name == "image");
                        updateSOUL_photo.value = new_filename;
                        updateSOUL_photo.update_by__USER_id = Convert.ToInt64(user_id);
                        updateSOUL_photo.dt_update = dt;
                        dbEntity.Entry(updateSOUL_photo).State = System.Data.Entity.EntityState.Modified;
                        dbEntity.SaveChanges();
                        hasupdate = true;
                    }
                    else
                    {
                        SOUL_ext addSOUL_photo = new SOUL_ext
                        {
                            attr_name = "image",
                            dname = "Image",
                            value = new_filename,
                            rel_SOUL_id = soul_id,
                            rel_ref_datatype_id = 2,
                            dt_create = dt,
                            create_by__USER_id = Convert.ToInt64(user_id)
                        };
                        dbEntity.SOUL_ext.Add(addSOUL_photo);
                        dbEntity.SaveChanges();
                        hassave = true;
                    }

                    string path = "https://s3-ap-southeast-1.amazonaws.com/hsrecs/images/" + new_filename;
                    List<img_url> img_url = new List<img_url>();
                    img_url.Add(new img_url { image_url = path });

                    if (hasupdate)
                    {

                        custom.infomsg = "Image updated with this patient_id: " + soul_id;
                    }
                    if (hassave)
                    {
                        custom.infomsg = "Image saved with this patient_id: " + soul_id;
                    }
                    return Json(new { data = img_url, message = custom.infomsg, success = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }

        [HttpDelete] //method changed to PUT, since in DELETE all FromBody parameters are ignored
        [Route("Patient")]
        public IHttpActionResult deletexxPatient(string user_id = null, string patient_id = null)
        {

            //string root = HttpContext.Current.Server.MapPath("~/Content/Temp");
            //var provider = new MultipartFormDataStreamProvider(root);
            try
            {
                string msg;                //long user_id = 0, patient_id = 0;

                //await Request.Content.ReadAsMultipartAsync(provider);
                //foreach (var key in provider.FormData.AllKeys)
                //{
                //    foreach (var val in provider.FormData.GetValues(key))
                //    {
                //        if (key == "user_id")
                //        {
                //            IsRequired(key, val, 1);
                //            bool isval = long.TryParse(val, out user_id);
                //            //user_id = val;
                //        }
                //        else if (key == "patient_id")
                //        {
                //            IsRequired(key, val, 1);
                //            bool isval2 = long.TryParse(val, out patient_id);
                //            //patient_id = val;
                //        }
                //        else
                //        {
                //            msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                //            return Json(new { message = msg, success = false });
                //        }
                //    }
                //}

                custom.IsRequired("user_id", user_id, 2);
                custom.IsRequired("patient_id", patient_id, 2);
                if (custom.haserror)
                {
                    return Json(new { data = new string[] { }, message = custom.errmsg, success = false });
                }

                long patient_id_new = 0;
                bool bPat = long.TryParse(patient_id, out patient_id_new);
                if (!bPat)
                {
                    return Json(new { data = new string[] { }, message = "Invalid patient_id value.", success = false });
                }
                long user_id_new = 0;
                bool bUse = long.TryParse(user_id, out user_id_new);
                if (!bUse)
                {
                    return Json(new { data = new string[] { }, message = "Invalid user_id value.", success = false });
                }

                var _soul = dbEntity.SOULs.Find(patient_id_new);
                if (_soul != null)
                {
                    var ref_status = dbEntity.ref_status_type.Where(a => a.dname == "SOUL").FirstOrDefault().ref_status.Where(b => b.dname.ToLower() == "deleted").FirstOrDefault();
                    _soul.rel_ref_status_id = ref_status.id;
                    _soul.update_by__USER_id = user_id_new;
                    _soul.dt_update = dt;

                    dbEntity.Entry(_soul).State = System.Data.Entity.EntityState.Modified;
                    dbEntity.SaveChanges();


                }

                msg = "Patient record successfully deleted.";
                return Json(new { data = new string[] { }, message = msg, success = true });
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }

        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("user/profile_pic")]
        public async Task<IHttpActionResult> uploadprofilepic()
        {
            //var isauthorized = authorized.HSRequest(Request);
            //if (isauthorized.StatusCode == HttpStatusCode.Forbidden)
            //{
            //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized)
            //    {
            //        StatusCode = HttpStatusCode.Unauthorized,
            //    });
            //}

            string user_id = null, filename = "";
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
                        if (key == "user_id")
                        {
                            custom.IsRequired(key, val, 1);
                            user_id = val;
                        }
                        else
                        {
                            return Json(new { message = "Object reference not set to an instance of an object. Invalid parameter name: " + key, success = false });
                        }
                    }
                }
                custom.IsRequired("user_id", user_id, 2);
                bool is_upload = uploadpic(provider, user_id);
                custom.IsRequired("image", new_filename, 2);


                long new_user_id = Convert.ToInt64(user_id);

                if (custom.haserror)
                {
                    return Json(new { message = custom.errmsg, success = false });
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

                    custom.infomsg = "Photo uploaded successfully";
                    return Json(new { data = img_url, message = custom.infomsg, success = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }

        [HttpPost]
        [Route("image/editprofile1")]
        public async Task<string> EditProfile1()
        //public string EditProfile()
        {
            try
            {

                string fullImagePath = string.Empty;
                var request = HttpContext.Current.Request;

                HttpPostedFile profileImage = request.Files["profile_image"];

                if (profileImage != null)
                {
                    var mapPath = HttpContext.Current.Server.MapPath("~/Images");
                    // CreateDirectory(mapPath);
                    fullImagePath = Path.Combine(mapPath, profileImage.FileName);

                    profileImage.SaveAs(fullImagePath);
                    return "Success";
                }
                else
                {
                    return "Failed";
                }
            }
            catch (Exception ex)
            {
                return "Failed";
            }
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("patient/information")]
        public IHttpActionResult getpatientinfo(string patient_id = null)
        {
            custom.IsRequired("patient_id", patient_id, 2);
            if (custom.haserror)
            {
                return Json(new { data = new string[] { }, message = custom.errmsg, success = false });
            }

            long patient_id_new = 0;
            bool isPat = long.TryParse(patient_id, out patient_id_new);
            if (isPat)
            {
                // previous appointments
                // previous doctor seen

                List<patient_APPOINTMENT> new_APPOINTMENT = new List<patient_APPOINTMENT>();
                List<doctor_profile> doc_prof = new List<doctor_profile>();

                //&& a.rel_ref_APPOINTMENT_status_id == 3
                var appt = dbEntity.APPOINTMENTs.Where(a => a.rel_SOUL_id == patient_id_new);
                List<long> appt_doc = new List<long>();
                foreach (var i in appt)
                {

                    var appt_ext = dbEntity.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == i.id);

                    string vtime_slot = "";
                    long vDoc_id = 0;
                    // GET appointment_ext
                    foreach (var j in appt_ext)
                    {
                        switch (j.attr_name)
                        {
                            case "time_slot":
                                vtime_slot = j.value;
                                break;

                            case "doctor_id":
                                bool isDoc = long.TryParse(j.value, out vDoc_id);
                                break;
                        }
                    }


                    var doc = dbEntity.hs_DOCTOR.Find(vDoc_id);

                    //xxxxxxxxxxxxxxxxx
                    //GET PATIENT's Doctor
                    #region "GET patients doctor"
                    if (!appt_doc.Contains(doc.id))
                    {
                        appt_doc.Add(Convert.ToInt64(doc.id));
                        List<appt_type> doc_appt = new List<appt_type>();
                        List<doc_specialty2> spec = new List<Models.doc_specialty2>();
                        spec = custom.getSpecialty(doc.id);

                        //string addr2 = doc.addr_address2 == null ? "" : " " + doc.addr_address2;
                        zip_search_address doc_address = new zip_search_address
                        {
                            address1 = doc.home_addr_1 == null ? "" : " " + doc.home_addr_1,
                            address2 = doc.home_addr_2 == null ? "" : " " + doc.home_addr_2,
                            zip = doc.ref_zip.zip,
                            city = doc.ref_zip.city_name,
                            state = doc.ref_zip.city_state,
                            state_long = doc.ref_zip.city_state_long,
                            lat = doc.ref_zip.city_lat,
                            lng = doc.ref_zip.city_lon,
                            county = doc.ref_zip.city_county
                        };
                        var doc_ext = dbEntity.DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doc.id
                                      && a.attr_name == "drappttype");

                        long vappt_id = 0;
                        foreach (var a in doc_ext)
                        {
                            bool isApp_type = long.TryParse(a.value, out vappt_id);

                            var n1 = dbEntity.ref_APPOINTMENT_type.Find(vappt_id);
                            //vappt_name   = 

                            doc_appt.Add(new appt_type
                            {
                                id = vappt_id,
                                type = n1.dname
                            });
                        }


                        doc_prof.Add(new doctor_profile
                        {
                            id = doc.id,
                            // doctor_fee = doc_fee,

                            // rating = appt_ext_rate,
                            // favorite = fave_doc,
                            first_name = doc.name_first == null ? "" : doc.name_first,
                            last_name = doc.name_last == null ? "" : doc.name_last,
                            middle_name = doc.name_middle == null ? "" : doc.name_middle,

                            email = doc.email,
                            gender = doc.gender == null ? "" : doc.gender,
                            title = doc.title == null ? "" : doc.title,
                            phone = doc.phone,
                            license = doc.license_no == null ? "" : doc.license_no,
                            npi = doc.NPI,
                            organization_name = doc.organization_name == null ? "" : doc.organization_name,
                            pecos_certificate = doc.pecos_certification == null ? "" : doc.pecos_certification,
                            date = i.date == null ? "" : i.date.Value.ToUniversalTime().GetDateTimeFormats()[3],
                            time_slot = vtime_slot,
                            image_url = (doc.image_url == null) ? "" : doc.image_url,

                            bio = doc.bio == null ? "" : doc.bio,
                            specialties = spec,
                            appointment_type = doc_appt,
                            //practice_address
                            //home_address
                            //address = doc_address
                        });

                    }
                    #endregion

                    //xxxxxxxxxxxxxxxxx

                    new_APPOINTMENT.Add(
                        new patient_APPOINTMENT
                        {
                            appointment_id = i.id,
                            date = i.date == null ? "" : i.date.Value.ToUniversalTime().GetDateTimeFormats()[3],
                            time_slot = vtime_slot,
                            doctor_id = vDoc_id,
                            doctor_name = doc == null ? "" : doc.name_first + " " + doc.name_last,
                            firstname = "",
                            lastname = "",
                            patient_id = 0//i.rel_SOUL_id.Value,

                        });
                }


                List<pat_info> patient_info = new List<pat_info>();
                patient_info.Add(new pat_info
                {
                    appointment = new_APPOINTMENT,
                    doctor = doc_prof
                });

                var ret1 = JsonConvert.SerializeObject(patient_info);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                return Json(new { data = json1, message = "", success = true });
            }

            return Json(new { data = new string[] { }, message = "", success = false });
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
                        AmazonS3Config S3Config = new AmazonS3Config()
                        {
                            ServiceURL = "http://s3-external-1.amazonaws.com"
                        };
                        string accessKey = WebConfigurationManager.AppSettings["AWSaccessKey"],
                               secretAccessKey = WebConfigurationManager.AppSettings["AWSsecretAccessKey"],
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



        // Remove patient insurance
        // Required Parameter :- user_id,first_name,lastname,insuance_type, use_patient_insurance(0/1), primary_type(0/1),patient_id
        // Return required message


    }

    
}