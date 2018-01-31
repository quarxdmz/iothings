using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using api.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace api.Controllers
{
    public class AppointmentTypeController : ApiController
    {

        // post
        [HttpPost]
        [Route("appointment/type")]
        public async Task<IHttpActionResult> postappointmenttype()
        {

            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);
            try {
               

                string display_name = "";
                string user_id = "";
                string msg = "";
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        switch (key) {
                            case "display_name":
                                IsRequired(key, val, 1);
                                display_name = val;
                                break;

                            case "user_id":
                                user_id = val;
                                break;

                            default:
                                msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                                return Json(new { data = new string[] { }, message = msg, success = false });
                        }
                    }
                }

                IsRequired("display_name", display_name, 1);
                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }

                long nUser_id = 0;
                bool bTemp = long.TryParse(user_id, out nUser_id);

                ref_APPOINTMENT_type app_type = new ref_APPOINTMENT_type();
                app_type.dname = display_name;
                app_type.create_by__USER_id = nUser_id;
                dbEntity.ref_APPOINTMENT_type.Add(app_type);
                dbEntity.SaveChanges();

                msg = "Appointment type added successfully.";
                return Json(new { data = new string[] { }, message = msg, success = true });
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false});
            }
        }
        // put
        // delete

        SV_db1Entities dbEntity = new SV_db1Entities();
        // get
        [HttpGet]
        [Route("appointment/type")]
        public IHttpActionResult getappointmenttype(long appointment_type_id = 0)
        {
            string msg = "";
            var app_type = from a in dbEntity.ref_APPOINTMENT_type select a;
            if (appointment_type_id >0) {
                app_type = app_type.Where(b => b.id == appointment_type_id);
            }

            List<app_type> appointment = new List<Controllers.app_type>();
            foreach (var n in app_type)
            {
                appointment.Add(new Controllers.app_type {
                    id = n.id,
                    display_name = n.dname,

                });
            }

            var ret1 = JsonConvert.SerializeObject(appointment);
            var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

            msg = appointment.Count() + " Record(s) found.";
            return Json(new { data = json1, message = msg, success = true });
        }

        [HttpPut]
        [Route("appointment/type")]
        public async Task<IHttpActionResult> putappointmenttype()
        {
            string msg = "";

            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);
            string appointment_type_id = "";
          
            try {
                string display_name = "";
                string user_id = "";
              
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        switch (key)
                        {
                            case "appointment_type_id":
                                IsRequired(key, val, 1);
                                appointment_type_id = val;
                                break;
                            case "display_name":
                                IsRequired(key, val, 1);
                                display_name = val;
                                break;

                            case "user_id":
                                user_id = val;
                                break;

                            default:
                                msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                                return Json(new { data = new string[] { }, message = msg, success = false });
                        }
                    }
                }

                IsRequired("display_name", display_name, 1);
                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }

                long nApp_type = 0;
                bool bApp_type = long.TryParse(appointment_type_id, out nApp_type);

                var app_type = dbEntity.ref_APPOINTMENT_type.Find(nApp_type);

                if (app_type != null)
                {
                    app_type.dname = display_name;
                    dbEntity.Entry(app_type).State = System.Data.Entity.EntityState.Modified;
                    dbEntity.SaveChanges();
                }

                //var ret1 = JsonConvert.SerializeObject(appointment);
                //var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                //msg = appointment.Count() + " Record(s) found.";

                return Json(new { data = new string[] { }, message = msg, success = true });
            }
            catch (Exception ex)
            {
                
                return Json(new { data = new string[] { }, message = ex.Message, success = false});
            }
          
        }

        //[HttpDelete]
        //[Route("user")]
        //public async Task<IHttpActionResult> Delete() 
        //{
        //    string root = HttpContext.Current.Server.MapPath("~/Temp");
        //    var provider = new MultipartFormDataStreamProvider(root);
        //    string msg = "", appointment_type_id ="";
        //    try {
        //        await Request.Content.ReadAsMultipartAsync(provider);
        //        foreach (var key in provider.FormData.AllKeys) {
        //            foreach (var val in provider.FormData.GetValues(key)) {

        //                switch (key)
        //                {
        //                    case "appointment_type_id":
        //                        IsRequired(key, val, 1);
        //                        appointment_type_id = val;
        //                        break;
        //                    default:
        //                        msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
        //                        return Json(new { data = new string[] { }, message = "", success = false });

        //                }
        //            }

        //            IsRequired("appointment_type_id", appointment_type_id, 2);
        //            if (haserror)
        //            {
        //                return Json(new { data = new string[] { }, message = errmsg, success = false });
        //            }

        //            long nApp_type = 0;
        //            bool bApp_type = long.TryParse(appointment_type_id, out nApp_type);

        //            var app_type = dbEntity.ref_APPOINTMENT_type.Find(nApp_type);
        //            if (app_type != null)
        //            {
        //                dbEntity.ref_APPOINTMENT_type.Remove(app_type);
        //                dbEntity.SaveChanges();
        //            }

        //        }
        //    }
        //    catch (Exception ex) {
        //        return Json(new { data = new string[] { }, message = ex.Message, success = false });
        //    }
        //}


        bool haserror = false;
        string errmsg = "", infomsg = "";
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


    }

    public class app_type
    {
        public long id { get; set; }
        public string display_name { get; set; }
    }
}
