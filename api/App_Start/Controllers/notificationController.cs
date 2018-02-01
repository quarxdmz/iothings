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

namespace api.Controllers
{
    public class notificationController : ApiController
    {
        SV_db1Entities db = new SV_db1Entities();
        DateTime dt = DateTime.UtcNow;

        [HttpPost]
        [Route("user/notification")]
        public async Task<IHttpActionResult> postNotification()
        {

            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);

            string msg, user_id = null, patient_id = null, text = null, link = null, is_unread = null;

            try
            {
                #region "params"
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        switch (key)
                        {
                            case "user_id":
                                isRequired(key, val, 1);
                                user_id = val;
                                break;

                            case "patient_id":
                                patient_id = val;
                                break;
                            case "text":
                                text = val;
                                break;
                            case "link":
                                link = val;
                                break;
                            case "is_unread":
                                is_unread = val;
                                break;
                            default:
                                msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                                return Json(new { data = new string[] { }, message = msg, success = false });
                        }
                    }
                }

                isRequired("user_id", user_id, 1);
                if (hasError)
                {
                   return Json(new {data = new string[] { }, message = errmsg, success =false });
                }

                // check user info
                long user_id_new = 0;
                bool isValidUser = long.TryParse(user_id, out user_id_new);
                if (!isValidUser)
                { return Json(new { data = new string[] { }, message = "Invalid user_id value.", success = false });  }

                var _user = db.USERs.Find(user_id_new);
                if (_user == null) { return Json(new { data = new string[] { }, message = "Invalid user_id value.", success = false }); }

                // check patient info
                long patient_id_new = 0;
                bool isValidPatient = long.TryParse(patient_id, out patient_id_new);
                if (!isValidPatient) { return Json(new { data = new string[] { }, message = "Invalid patient_id value.", success = false }); }

                // check is_read value
                if (!string.IsNullOrEmpty(is_unread))
                {
                    if (is_unread.ToLower() != "false" || is_unread.ToLower() != "true")
                    { return Json(new { data = new string[] { }, message = "Invalid patient_id value.", success = false }); }
                }
                else
                { is_unread = "false"; }
                
                #endregion
                notification noti = new notification {
                    rel_USER_id = user_id_new,
                    SOUL_id = patient_id_new,
                    text = text,
                    link = link,
                    is_unread = is_unread.ToLower() == "true" ? true : false,
                    create_by__USER_id = user_id_new,
                    dt_create = dt
                 
                };

                db.notifications.Add(noti);
                db.SaveChanges();

                return Json(new { data = new string[] { }, message = "Notification successfully saved.", success = true });

            }
            catch (Exception ex) {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }


        [HttpPut]
        [Route("user/notification")]
        public async Task<IHttpActionResult> putNotification()
        {

            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);

            string msg, user_id = null, notification_id = null, text = null, link = null, is_unread = null;

            try
            {
                #region "params"
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        switch (key)
                        {
                            case "user_id":
                                isRequired(key, val, 1);
                                user_id = val;
                                break;
                            //case "patient_id":
                            //    patient_id = val;
                            //    break;

                            case "notification_id":
                                notification_id = val;
                                isRequired(key, val, 1);
                                break;

                            case "text":
                                text = val;
                                break;
                            case "link":
                                link = val;
                                break;
                            case "is_unread":
                                is_unread = val;
                                break;
                            default:
                                msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                                return Json(new { data = new string[] { }, message = msg, success = false });
                        }
                    }
                }

                isRequired("user_id", user_id, 1);
                isRequired("notification_id", notification_id, 1);
                if (hasError)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }

                // check user info
                long user_id_new = 0;
                bool isValidUser = long.TryParse(user_id, out user_id_new);
                if (!isValidUser)
                { return Json(new { data = new string[] { }, message = "Invalid user_id value.", success = false }); }

                var _user = db.USERs.Find(user_id_new);
                if (_user == null) { return Json(new { data = new string[] { }, message = "Invalid user_id value.", success = false }); }

                // check notif_id info
                long notif_id_new = 0;
                bool isValidPatient = long.TryParse(notification_id, out notif_id_new);
                if (!isValidPatient) { return Json(new { data = new string[] { }, message = "Invalid notification_id value.", success = false }); }

                var notif = db.notifications.Find(notif_id_new);
                if (notif == null) { return Json(new { data = new string[] { }, message = "Invalid notification_id value.", success = false }); }
             
                // check is_read value
                if (is_unread.ToLower() != "false" || is_unread.ToLower() != "true")
                { return Json(new { data = new string[] { }, message = "Invalid patient_id value.", success = false }); }
                #endregion


                if (!string.IsNullOrEmpty(text)) notif.text = text;
                if (!string.IsNullOrEmpty(link)) notif.link = link;
                if (!string.IsNullOrEmpty(is_unread)) notif.is_unread = is_unread.ToLower() == "true" ? true : false ;
                notif.update_by__USER_id = user_id_new;
                notif.dt_update = dt;
                //notification noti = new notification
                //{
                //    rel_USER_id = user_id_new,
                //    SOUL_id = patient_id_new,
                //    text = text,
                //    link = link,
                //    is_unread = is_unread.ToLower() == "true" ? true : false,
                //};

                db.Entry(notif).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return Json(new { data = new string[] { }, message = "Notification successfully saved.", success = true });

            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }

        [HttpGet]
        [Route("user/notification")]
        public IHttpActionResult getNotification(string user_id = null, string patient_id =null)
        {
            try
            {
                isRequired("user_id", user_id, 1);
                if (hasError)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }

                // check user_id info
                long user_id_new = 0;
                bool isValidUser = long.TryParse(user_id, out user_id_new);
                if (!isValidUser) { return Json(new { data = new string[] { }, message = "Invalid user_id value.", success = false }); }

                var _user = db.USERs.Find(user_id_new);
                if (_user == null) { return Json(new { data = new string[] { }, message = "Invalid user_id value.", success = false }); }

                // check user_id against notification table
                var notif = db.notifications.Where(a => a.rel_USER_id == user_id_new);
                if (notif.Count() == 0) { return Json(new { data = new string[] { }, message = "No record found.", success = false }); }

                if (patient_id != null)
                {
                    long patient_id_new = 0;
                    bool isValidPatient = long.TryParse(patient_id, out patient_id_new);
                    if (!isValidPatient) { return Json(new { data = new string[] { }, message = "Invalid patient_id value.", success = false }); }
                    var _patient = db.SOULs.Find(patient_id_new);
                    if (_patient == null) { return Json(new { data = new string[] { }, message = "Invalid patient_id value.", success = false }); }

                    notif = notif.Where(a => a.SOUL_id == patient_id_new);
                }
                
                // get notification
                List<rec_notification> rec_notif = new List<rec_notification>();
                if (notif.Count() > 0)
                {
                    foreach (var n in notif)
                    {
                        rec_notif.Add(new rec_notification
                        {
                            user_id = n.rel_USER_id,
                            appointment_id = n.APPOINTMENT_id,
                            patient_id = n.SOUL_id == null ? 0:  n.SOUL_id.Value,
                            link = n.link,
                            text = n.text,
                            is_unread = n.is_unread == null ? false : n.is_unread.Value,
                            date_created = n.dt_create.GetDateTimeFormats()[56]
                        });
                    }
                }
                else
                {
                    return Json(new { data = new string[] { }, message = "No notification found.", success = false });
                }

                var ret1 = JsonConvert.SerializeObject(rec_notif);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                return Json(new { data = json1, message = rec_notif.Count() + " notification(s) found.", success = true });
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = true });
            }

            
        }

        bool hasError = false;
        string errmsg = "", infomsg = "";
        public bool isRequired(string key, string val, int i) {
            if (i == 1)
            {
                if (string.IsNullOrEmpty(val))
                {
                    hasError = true;
                    errmsg += key + " is required. \r\n";
                    return false;
                }

                return true;
            }
            else
            {
                hasError = true;
                errmsg += " Missing parameter: " + key + ". \r\n";
                return false;
            }

            return true;
        }
    }


}
