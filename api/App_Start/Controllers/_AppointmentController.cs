using api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;

namespace api.Controllers
{
    public class _AppointmentController : ApiController
    {
        SV_db1Entities db = new SV_db1Entities();
        DateTime dt = DateTime.UtcNow;
        DateTime date = DateTime.Today;
        bool haserror = false;
        string errmsg = "", infomsg = "";
        List<IQueryable> Arraydata = new List<IQueryable>();
        string user_id = null;

        public static SV_db1Entities db1 = new SV_db1Entities();


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("appointment")]
        public IHttpActionResult newAppointment(param_appointment par)
        {
            try
            {
                #region
                //        if (key == "user_id")
                //            IsRequired(key, val, 1);
                //        else if (key == "patient_id")
                //            IsRequired(key, val, 1);
                //        else if (key == "specialty_id")
                //            IsRequired(key, val, 1);
                //            specialty_id = val;
                //        else if (key == "appointment_type_id")
                //            IsRequired(key, val, 1);
                //        // 2017-7-19.. Appointment_status_id not required
                //        //else if (key == "appointment_status_id")
                //        //    IsRequired(key, val, 1);
                //        else if (key == "lat")
                //            IsRequired(key, val, 1);
                //        else if (key == "longi")
                //            IsRequired(key, val, 1);
                //        else if (key == "appointment_date")
                //            // IsRequired(key, val, 1);
                //        else if (key == "time_slot")
                //            // IsRequired(key, val, 1);
                //        else if (key == "doctor_id")
                //            // IsRequired(key, val, 1);
                          
                #endregion


                IsRequired("user_id", par.user_id.ToString(), 2);
                IsRequired("patient_id", par.patient_id.ToString(), 2);
                IsRequired("specialty_id", par.specialty_id.ToString(), 2);
                IsRequired("appointment_type_id", par.appointment_type_id.ToString(), 2);
                //IsRequired("appointment_status_id", appointment_status_id, 2);
                IsRequired("lat", par.lat.ToString(), 2);
                IsRequired("longi", par.longi.ToString(), 2);

                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }
                else
                {

                    //ref_APPOINTMENT_type addref_APPOINTMENT_type = new ref_APPOINTMENT_type
                    //{
                    //    dt_create = dt,
                    //    create_by__USER_id = Convert.ToInt64( user_id),
                    //    dname = type_appointment,
                    //};
                    //db.ref_APPOINTMENT_type.Add(addref_APPOINTMENT_type);
                    //db.SaveChanges();

                    //10  Active  4
                    //11  Inactive    4
                    //12  Deleted 4

                    APPOINTMENT addAPPOINTMENT = new APPOINTMENT();
                    APPOINTMENT_ext app_ex = new APPOINTMENT_ext();

                    //long appt_type_id = 0;
                    //bool btemp = long.TryParse(appointment_type_id, out appt_type_id);
                    //var appt_type = db.ref_APPOINTMENT_type.Find(appt_type_id);
                    //if (appt_type == null) {
                    //    return Json(new { data = new string[] { }, message = "Invalid appoitnment_type_id value.", success = false });
                    //}

                    long appt_type_id = api.Models.Validation.checkApptType(par.appointment_type_id);
                    if (appt_type_id == 0)
                    {
                        return Json(new { data = new string[] { }, message = "Invalid appoitnment_type_id value.", success = false });
                    }


                    //long appt_status_id = api.Models.Validation.checkStatus(appointment_status_id);
                    //if (appt_status_id == 0)
                    //{
                    //    return Json(new { data = new string[] { }, message = "Invalid appoitnment_status_id value.", success = false });
                    //}

                    //long appt_status_id = 0;
                    //bool btemp1 = long.TryParse(appointment_status_id, out appt_status_id);
                    //var appt_stat = db.ref_APPOINTMENT_status.Find(appt_status_id);
                    //if (appt_stat == null) {
                    //    return Json(new { data = new string[] { }, message = "Invalid appoitnment_status_id value.", success = false });
                    //}

                    addAPPOINTMENT.rel_ref_APPOINTMENT_status_id = 1; // Convert.ToInt64(appt_status_id);
                    // 2017-7-19.. Appointment_status_id not required

                    addAPPOINTMENT.rel_ref_APPOINTMENT_type_id = Convert.ToInt64(appt_type_id);
                    addAPPOINTMENT.rel_SOUL_id = Convert.ToInt64(par.patient_id);
                    addAPPOINTMENT.rel_ref_status_id = 10;
                    addAPPOINTMENT.create_by__USER_id = Convert.ToInt64(user_id);
                    addAPPOINTMENT.dt_create = dt;
                    if (par.appointment_date != null)
                    {
                        addAPPOINTMENT.date = Convert.ToDateTime(par.appointment_date);
                    }


                    db.APPOINTMENTs.Add(addAPPOINTMENT);
                    db.SaveChanges();

                    bool i = false;
                    if (par.doctor_id > 0)
                    {
                         i=Entry.saveAppt_ext("doctor_id", "DOCTOR", par.doctor_id.ToString(), addAPPOINTMENT.id);
                    }

                    if (!string.IsNullOrEmpty(par.time_slot))
                    {
                         i = Entry.saveAppt_ext("time_slot", "Time Slot", par.time_slot, addAPPOINTMENT.id);
                    }

                    i = Entry.saveAppt_ext("lat", "Latitude", par.lat.ToString(), addAPPOINTMENT.id);
             
                    i = Entry.saveAppt_ext("longi", "Longitude", par.longi.ToString(), addAPPOINTMENT.id);
          
                    i = Entry.saveAppt_ext("specialty_id", "Specialty", par.specialty_id.ToString(), addAPPOINTMENT.id);
        
                    List<appt_id> d = new List<appt_id>();
                    d.Add(new appt_id { appointment_id = addAPPOINTMENT.id });

                    var ret1 = JsonConvert.SerializeObject(d);
                    var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                    // insert to [notification] table
                    notification notif = new notification {
                        rel_USER_id = Convert.ToInt64(user_id),
                        SOUL_id = Convert.ToInt64(par.patient_id),
                        text = "Appoinment detail has been sent to the doctor.",
                        link ="",
                        APPOINTMENT_id = addAPPOINTMENT.id ,
                        is_unread= true,
                        dt_create = dt,
                        create_by__USER_id = Convert.ToInt64(user_id)
                    };
                    db.notifications.Add(notif);
                    db.SaveChanges();


                    //string msg = "The appointment has been scheduled successfully.";
                    string msg = "Appoinment detail has been sent to the doctor.";
                    return Json(new { data = json1, message = msg, success = true });
                }


            }



        
            catch (System.Exception ex)
            {
                return Json(new { message = ex.Message, success = false });

            }

        }

        //private bool saveAppointment_ext(string _attr_name, string _dname, string _value, long appointment_id = 0)
        //{
        //    try
        //    {

        //        var d_ext = db.APPOINTMENT_ext.Where(a => a.attr_name == _attr_name && a.rel_APPOINTMENT_id == appointment_id).FirstOrDefault();
        //        if (d_ext == null) // add attr_name if does not exist yet
        //        {
        //            d_ext = new APPOINTMENT_ext();
        //            d_ext.rel_APPOINTMENT_id = appointment_id;
        //            d_ext.attr_name = _attr_name;
        //            d_ext.dname = _dname;
        //            d_ext.value = _value;
        //            d_ext.dt_create = dt;
        //            d_ext.create_by__USER_id = user_id == null ? 0 : Convert.ToInt64(user_id);
        //            db.APPOINTMENT_ext.Add(d_ext);
        //            db.SaveChanges();
        //        }
        //        else // update the record if attr_name already exist
        //        {
        //            //DOCTOR_ext d_ext = new DOCTOR_ext();
        //            //var d_ext = dbEntity.DOCTOR_ext.Where(a => a.attr_name == _attr_name && a.rel_DOCTOR_id == doc_id).FirstOrDefault();

        //            //d_ext.attr_name = _attr_name;
        //            //d_ext.dname = _dname;
        //            d_ext.value = _value;
        //            d_ext.dt_update = dt;
        //            d_ext.update_by__USER_id = user_id == null ? 0 : Convert.ToInt64(user_id);
        //            db.Entry(d_ext).State = System.Data.Entity.EntityState.Modified;
        //            db.SaveChanges();

        //        }
        //    }
        //    catch (Exception ex) { }
        //    return true;
        //}

         
        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("appointment/cancel")]
        public async Task<IHttpActionResult> cancelAppointment()
        {
            string user_id = null, appointment_id = null;
            //string specialty_id = null, appointment_type_id = null, lat = null, longi = null, appointment_date = null;

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
                        else if (key == "appointment_id")
                        {
                            IsRequired(key, val, 1);
                            appointment_id = val;
                        }
                        else
                        {
                            return Json(new { data = new string[] { }, message = "Object reference not set to an instance of an object. Invalid parameter name: " + key, success = false });
                        }
                    }
                }

                IsRequired("user_id", user_id, 2);
                IsRequired("appointment_id", appointment_id, 2);

                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }
                else
                {
                    //4 APPOINTMENT
                    //10  Active  4
                    //11  Inactive    4
                    //12  Deleted 4

                    long userid = 0;
                    bool bTemp1 = long.TryParse(user_id, out userid);
                    if (bTemp1)
                    {
                        var t_User = db.USERs.Find(userid);
                        if (t_User == null)
                        {
                            return Json(new { data = new string[] { }, message = "Invalid user_id.", success = false });
                        }
                    }
                    else
                    {
                        return Json(new { data = new string[] { }, message = "Invalid user_id value.", success = false });
                    }

                    string errmsg = "";
                    long appt_id = 0;
                    bool bTemp = long.TryParse(appointment_id, out appt_id);
                    if (bTemp)
                    {
                        var appt = db.APPOINTMENTs.Find(appt_id);

                        if (appt != null)
                        {
                            appt.rel_ref_status_id = 11;
                            appt.update_by__USER_id = userid;
                            appt.dt_update = dt;
                            db.Entry(appt).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();


                            // update to [notification] table
                            var notif = db.notifications.Where(a => a.APPOINTMENT_id == appt_id);
                            if (notif.Count() > 0)
                            {
                                foreach (var n in notif)
                                {
                                    //n.text = "Appointment has been cancelled.";
                                    n.rel_USER_id = Convert.ToInt64(user_id);
                                    n.is_unread = true;
                                    n.update_by__USER_id = Convert.ToInt64(user_id);
                                    n.dt_update = dt;
                                    db.Entry(n).State = System.Data.Entity.EntityState.Modified;
                                }
                                db.SaveChanges();
                            }

                            return Json(new { data = new string[] { }, message = "Appointment has been cancelled.", success = true });
                        }
                        else
                        {
                            errmsg = "Invalid appointment_id value.";
                            return Json(new { data = new string[] { }, message = errmsg, success = false });
                        }
                    }
                    else
                    {
                        errmsg = "Invalid appointment_id value.";
                        return Json(new { data = new string[] { }, message = errmsg, success = false });
                    }


                }
                //***********************************
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false });
            }
        }

        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("appointment")]
        public IHttpActionResult updateAppointment(param_appointment par)
        {
              try
            {

                #region
                //        if (key == "user_id")
                //            IsRequired(key, val, 1);
                //        else if (key == "appointment_id")
                //            IsRequired(key, val, 1); 
                //        else if (key == "specialty_id")
                //        else if (key == "appointment_type_id")
                //        else if (key == "lat")
                //        else if (key == "longi")
                //        else if (key == "appointment_date")
                //        else if (key == "doctor_id") // added by monton.posadas 
                //        else
                #endregion


                bool i = false;
                IsRequired("user_id", par.user_id.ToString(), 2);
                IsRequired("appointment_id", par.appointment_id.ToString(), 2);

                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }
                else
                {


                    long appointment_id_new = Convert.ToInt64(par.appointment_id);
                    var appt = (from a in db.APPOINTMENTs
                                where a.id == appointment_id_new
                                select a);
                    if (appt.Count() > 0)
                    {
                          
                       

                        bool hasupdate = false;
                        long appt_type = appt.First().rel_ref_APPOINTMENT_type_id.Value;

                        //if (!string.IsNullOrEmpty(appointment_type_id))
                        if(par.appointment_type_id > 0)
                        {
                            long appt_type_id = 0;
                            bool btemp = long.TryParse(par.appointment_type_id.ToString(), out appt_type_id);
                            var appt_type1 = db.ref_APPOINTMENT_type.Find(appt_type_id);

                            if (appt_type1 == null)
                            {
                                return Json(new { data = new string[] { }, message = "Invalid appoitnment_type_id value.", success = false });
                            }

                          
                            APPOINTMENT update_appt = db.APPOINTMENTs.First(a => a.id == appointment_id_new);
                            update_appt.rel_ref_APPOINTMENT_type_id = Convert.ToInt64(appt_type_id);
                            update_appt.dt_update = dt;
                            update_appt.update_by__USER_id = Convert.ToInt64(par.user_id);
                            db.Entry(update_appt).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            hasupdate = true;

                        }

                        //if (!string.IsNullOrEmpty(specialty_id))
                        if(par.specialty_id > 0)
                        {
                            #region
                            //var apt_ext = (from s in db.APPOINTMENT_ext
                            //               where s.rel_APPOINTMENT_id == appointment_id_new && s.dname == "Specialty"
                            //               select s);

                            //long apt_id = apt_ext.First().id;
                            //APPOINTMENT_ext update_appt_type = db.APPOINTMENT_ext.First(a => a.id == apt_id);
                            //update_appt_type.value = specialty_id;
                            //update_appt_type.update_by__USER_id = Convert.ToInt64(user_id);
                            //update_appt_type.dt_update = dt;
                            //db.Entry(update_appt_type).State = System.Data.Entity.EntityState.Modified;
                            //db.SaveChanges();
                            //hasupdate = true;
                            #endregion

                            i = Entry.saveAppt_ext("specialty_id", "Specialty ID", par.specialty_id.ToString(), appointment_id_new, par.user_id, 3);
                        }

                        //if (!string.IsNullOrEmpty(longi))
                        if(par.longi !=0)
                        {
                            #region
                            //var apt_ext = (from s in db.APPOINTMENT_ext
                            //               where s.rel_APPOINTMENT_id == appointment_id_new && s.dname == "Longitude"
                            //               select s);
                            //long apt_id = apt_ext.First().id;
                            //APPOINTMENT_ext update_appt_type = db.APPOINTMENT_ext.First(a => a.id == apt_id);
                            //update_appt_type.value = longi;
                            //update_appt_type.update_by__USER_id = Convert.ToInt64(user_id);
                            //update_appt_type.dt_update = dt;
                            //db.Entry(update_appt_type).State = System.Data.Entity.EntityState.Modified;
                            //db.SaveChanges();
                            //hasupdate = true;
                            #endregion

                            i = Entry.saveAppt_ext("longi", "Longitude", par.longi.ToString(), appointment_id_new);
                        }
                        //if (lat == null || lat == "")
                        if(par.lat != 0)
                        {
                            #region
                            //var apt_ext = (from s in db.APPOINTMENT_ext
                            //               where s.rel_APPOINTMENT_id == appointment_id_new && s.dname == "Latitude"
                            //               select s);
                            //long apt_id = apt_ext.First().id;
                            //APPOINTMENT_ext update_appt_type = db.APPOINTMENT_ext.First(a => a.id == apt_id);
                            //update_appt_type.value = lat;
                            //update_appt_type.update_by__USER_id = Convert.ToInt64(user_id);
                            //update_appt_type.dt_update = dt;
                            //db.Entry(update_appt_type).State = System.Data.Entity.EntityState.Modified;
                            //db.SaveChanges();
                            //hasupdate = true;
                            #endregion

                            i = Entry.saveAppt_ext("longi", "Longitude", par.lat.ToString(), appointment_id_new,par.user_id);
                        }
                        //if (appointment_date == null || appointment_date == "")
                        if(!string.IsNullOrEmpty(par.appointment_date))
                        {
                            #region
                            //APPOINTMENT update_appt = db.APPOINTMENTs.First(a => a.id == appointment_id_new);
                            //update_appt.date = Convert.ToDateTime(par.appointment_date);
                            //update_appt.dt_update = dt;
                            //update_appt.update_by__USER_id = Convert.ToInt64(user_id);
                            //db.Entry(update_appt).State = System.Data.Entity.EntityState.Modified;
                            //db.SaveChanges();
                            //hasupdate = true;
                            #endregion


                            i = Entry.saveAppt_ext("appointment_date", "Appointment Date", par.appointment_date.ToString(), appointment_id_new, par.user_id);
                        }


                        //if (doctor_id == null || doctor_id == "")
                        if(par.doctor_id > 0)
                        {
                            #region
                            //    var apt_ext = (from s in db.APPOINTMENT_ext
                            //               where s.rel_APPOINTMENT_id == appointment_id_new && s.dname == "DOCTOR"
                            //               select s);
                            //long apt_id = apt_ext.First().id;
                            //APPOINTMENT_ext update_appt_type = db.APPOINTMENT_ext.First(a => a.id == apt_id);
                            //update_appt_type.value = doctor_id;
                            //update_appt_type.update_by__USER_id = Convert.ToInt64(user_id);
                            //update_appt_type.dt_update = dt;
                            //db.Entry(update_appt_type).State = System.Data.Entity.EntityState.Modified;
                            //db.SaveChanges();
                            //hasupdate = true;

                            #endregion

                            i = Entry.saveAppt_ext("doctor_id", "Doctor ID", par.doctor_id.ToString(), appointment_id_new, par.user_id);
                        }


                        // update to [notification] table
                        var notif = db.notifications.Where(a => a.APPOINTMENT_id == appointment_id_new);
                        if (notif.Count() > 0)
                        {
                            foreach (var n in notif)
                            {
                                n.text = "Appointment has been updated.";
                                n.rel_USER_id = Convert.ToInt64(user_id);
                                n.is_unread = true; 
                                n.update_by__USER_id = Convert.ToInt64(user_id);
                                n.dt_update = dt;
                                db.Entry(n).State = System.Data.Entity.EntityState.Modified;
                            }
                            db.SaveChanges();
                        }

                        infomsg = "Record updated with this appointment_id: " + appointment_id_new;
                        return Json(new { data = new string[] { }, message = infomsg, success = true });
                    }
                    else
                    {
                        infomsg = "No record found with this appointment_id: " + appointment_id_new;
                        return Json(new { data = new string[] { }, message = infomsg, success = false });
                    }



                }
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false });
            }
        }  

        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("appointment")]
        public async Task<IHttpActionResult> deleteAppointment()
        {
            string user_id = null, appointment_id = null;

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
                        else if (key == "appointment_id")
                        {
                            IsRequired(key, val, 1);
                            appointment_id = val;
                        }
                        else
                        {
                            return Json(new { message = "Object reference not set to an instance of an object. Invalid parameter name: " + key, success = false });
                        }
                    }
                }

                IsRequired("user_id", user_id, 2);
                IsRequired("appointment_id", appointment_id, 2);

                if (haserror)
                {
                    return Json(new { message = errmsg, success = false });
                }
                else
                {
                    long appointment_id_new = Convert.ToInt64(appointment_id);
                    var appt = (from a in db.APPOINTMENTs
                                where a.id == appointment_id_new
                                select a);
                    if (appt.Count() > 0)
                    {
                        APPOINTMENT update_appt = db.APPOINTMENTs.First(a => a.id == appointment_id_new);
                        update_appt.rel_ref_status_id = 12;
                        update_appt.dt_update = dt;
                        update_appt.update_by__USER_id = Convert.ToInt64(user_id);
                        db.Entry(update_appt).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        infomsg = "Record deleted: " + appointment_id_new;
                        return Json(new { message = infomsg, success = true });
                    }
                    else
                    {
                        infomsg = "No record found with this appointment_id: " + appointment_id_new;
                        return Json(new { message = infomsg, success = false });
                    }



                }
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false });
            }
        }



        [System.Web.Http.Route("appointment/all")]
        [HttpGet]
        public IHttpActionResult Getall(string patient_id = null, string user_id = null, string appointment_type = null)
        {

            //IsRequired("user_id", user_id, 2);
            if (haserror)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }
            else
            {
                try
                {
                    long user_id_new = 0;
                    bool bTem = long.TryParse(user_id, out user_id_new);

                    var querydata1 = (from a in db.APPOINTMENTs
                                      where a.date >= date && a.rel_ref_status_id != 12 // 12 = DELETED
                                                                                        //where (a.date >= date && a.create_by__USER_id == user_id_new)
                                                                                        //where (a.create_by__USER_id == user_id_new)
                                                                                        // orderby a.rel_SOUL_id, a.id
                                      select a);
                    //select new
                    //{
                    //    present = new new_APPOINTMENT
                    //    {
                    //        appointment_id = a.id,
                    //        //appointment = a.ref_APPOINTMENT_type.dname,
                    //        user_id = a.create_by__USER_id.Value,
                    //        patient_id = a.rel_SOUL_id.Value,
                    //        firstname = a.SOUL.name_first == null ? "" : a.SOUL.name_first,
                    //        lastname = a.SOUL.name_last == null ? "" : a.SOUL.name_last,
                    //        date = a.date.ToString(),
                    //        status = (a.rel_ref_status_id.Value == 7) ? "Active" : "Inactive",
                    //        //doctor = "",
                    //        //specialty = "",
                    //       // doctor_image = ""
                    //    }
                    //});

                    if (user_id_new > 0)
                    {
                        // present
                        querydata1 = querydata1.Where(a => a.create_by__USER_id == user_id_new);

                        //querydata1 = (from a in db.APPOINTMENTs
                        //                  where a.create_by__USER_id == user_id_new && a.rel_ref_status_id != 9 // 9 = DELETED
                        //                  //where (a.date >= date && a.create_by__USER_id == user_id_new)
                        //                  //where (a.create_by__USER_id == user_id_new)
                        //                  orderby a.rel_SOUL_id, a.id
                        //                  select new
                        //                  {
                        //                      present = new new_APPOINTMENT
                        //                      {
                        //                          appointment_id = a.id,
                        //                          //appointment = a.ref_APPOINTMENT_type.dname,
                        //                          user_id = a.create_by__USER_id.Value,
                        //                          patient_id = a.rel_SOUL_id.Value,
                        //                          firstname = a.SOUL.name_first == null ? "" : a.SOUL.name_first,
                        //                          lastname = a.SOUL.name_last == null ? "" : a.SOUL.name_last,
                        //                          date = a.date.ToString(),
                        //                          status = (a.rel_ref_status_id.Value == 7) ? "Active" : "Inactive",
                        //                          //doctor_name = "",
                        //                          //specialty = "",
                        //                          //doctor_image = ""
                        //                      }
                        //                  });
                    }



                    if (patient_id != null)
                    {
                        // present
                        long patient_id_new = Convert.ToInt64(patient_id);
                        querydata1 = querydata1.Where(a => a.rel_SOUL_id == patient_id_new);
                    }



                    long ctr = 0;
                    List<new_APPOINTMENT> new_APPOINTMENT = new List<new_APPOINTMENT>();
                    if (querydata1.Count() > 0)
                    {
                        foreach (var i in querydata1)
                        {
                            //long doc_id = 0;
                            string doc_name = "", doc_specialty = null, image_url = null;

                            // retrieve info from Appointment_ext
                            var appt_ext = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == i.id);
                            List<doctor_profile> doc = new List<doctor_profile>();

                            double appt_ext_rate = 0; string appt_ext_timeslot = "";
                            long doctor_id = 0;
                            List<appt_type> appt = new List<appt_type>();
                            List<appt_type> doc_appt = new List<appt_type>();
                            if (i.rel_ref_APPOINTMENT_type_id != null)
                            {
                                appt.Add(new appt_type
                                {
                                    id = i.rel_ref_APPOINTMENT_type_id.Value,
                                    type = i.ref_APPOINTMENT_type.dname
                                });
                            }

                            if (appt_ext.Count() > 0)
                            {
                                foreach (var k in appt_ext)
                                {
                                    switch (k.attr_name)
                                    {
                                        case "doctor_rating":
                                            bool bTemp = double.TryParse(k.value, out appt_ext_rate);

                                            break;

                                        case "time_slot":
                                            appt_ext_timeslot = k.value;
                                            break;

                                        case "doctor_id":
                                            bool bTemp2 = long.TryParse(k.value, out doctor_id);
                                            break;
                                    }
                                }

                                //List<doctor_profile> doc = new List<doctor_profile>();

                                if (doctor_id > 0)
                                {
                                    var doctor_name = db.DOCTORs.Find(doctor_id);
                                    //var con_ref_spec = db.con_DOCTOR_ref_specialty.Where(a => a.rel_DOCTOR_id == doctor_id);

                                    image_url = doctor_name.image_url;

                                    List<doc_specialty2> spec = new List<Models.doc_specialty2>();
                                    spec = getSpecialty(doctor_id);

                                    //string addr2 = doctor_name.addr_address2 == null ? "" : " " + doctor_name.addr_address2;
                                    zip_search_address doc_address = new zip_search_address {
                                        address1 = doctor_name.addr_address1 == null ? "" : doctor_name.addr_address1,
                                        address2 = doctor_name.addr_address2 == null ? "" : doctor_name.addr_address2,
                                        zip = doctor_name.ref_zip.zip,
                                        city = doctor_name.ref_zip.city_name,
                                        state = doctor_name.ref_zip.city_state,
                                        lat = doctor_name.ref_zip.city_lat,
                                        lng = doctor_name.ref_zip.city_lon,
                                        county = doctor_name.ref_zip.city_county
                                    };

                                    string appt_name = "", vtime_slot = "";
                                    long vappt_id = 0; double doc_fee = 0, time_slot = 0;

                                    var doc_ext = db.DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doctor_id);
                                    if (doc_ext.Count() > 0)
                                    {
                                        foreach (var n in doc_ext) {
                                            switch (n.attr_name)
                                            {
                                                case "drappttype":
                                                    bool isAppt = long.TryParse(n.value, out vappt_id);

                                                    if (vappt_id > 0)
                                                    {
                                                        var n1 = db.ref_APPOINTMENT_type.Find(vappt_id);
                                                        //vappt_name   = 

                                                        doc_appt.Add(new appt_type {
                                                            id = vappt_id,
                                                            type = n1.dname
                                                        });
                                                    }
                                                    break;

                                                case "fee":
                                                    bool bTemp = double.TryParse(n.value, out doc_fee);
                                                    break;
                                                case "time_slot":
                                                    vtime_slot = n.value == null ? "" : n.value;
                                                    break;
                                            }
                                        }

                                        // get if Doctor is 
                                        int fave_doc = 0;
                                        if (user_id_new > 0)
                                        {
                                            var con_fav = db.con_USER_favorite_DOCTOR.Where(a => a.create_by__USER_id == user_id_new && a.rel_doctor_id == doctor_name.id);

                                            if (con_fav.Count() > 0)
                                            {
                                                if (con_fav.FirstOrDefault().favor == true)
                                                    fave_doc = 1;
                                                else
                                                    fave_doc = 0;
                                            }
                                        }



                                        doc.Add(new doctor_profile {
                                            id = doctor_name.id,
                                            doctor_fee = doc_fee,
                                            bio = doctor_name.bio == null ? "" : doctor_name.bio,
                                            time_slot = vtime_slot,
                                            //name = doc_name,
                                            //rating
                                            favorite = fave_doc,
                                            first_name = doctor_name.name_first == null ? "" : doctor_name.name_first,
                                            last_name = doctor_name.name_last == null ? "" : doctor_name.name_last,
                                            middle_name = doctor_name.name_middle == null ? "" : doctor_name.name_middle,
                                            email = doctor_name.email,
                                            gender = doctor_name.gender,
                                            title = doctor_name.title,
                                            phone = doctor_name.phone,
                                            license = doctor_name.license_no,
                                            npi = doctor_name.NPI,
                                            orgname = doctor_name.organ_name,
                                            pecoscert = doctor_name.pecos_cert == null ? false : true,
                                            image_url = doctor_name.image_url == null ? "" : doctor_name.image_url,
                                            //timeslot = appt_ext_timeslot,
                                            address = doc_address,
                                            specialties = spec
                                            // 
                                        });
                                    }
                                }

                            }

                            var _soul = db.SOULs.Find(i.rel_SOUL_id);
                            
                            new_APPOINTMENT.Add(new _AppointmentController.new_APPOINTMENT {
                                appointment_id = i.id,
                                patient_id = i.rel_SOUL_id.Value,
                                first_name = _soul.name_first == null ? "" : _soul.name_first,
                                last_name = _soul.name_last == null ? "" : _soul.name_last,
                                time_slot = appt_ext_timeslot,
                                rating = appt_ext_rate,
                                status = (i.rel_ref_status_id.Value == 10) ? "Active" : "Inactive",
                                date = i.date.ToString(),
                                user_id = i.create_by__USER_id.Value,
                                doctor = doc,
                                doctor_id = doctor_id,
                                doctor_name = doc_name,
                                appointment_type = appt
                            });
                        }


                        // ----------------------
                        ctr = querydata1.Count();
                        Arraydata.Add(querydata1);
                    }

                    if (Arraydata.Count() == 0)
                    {
                        string msg = "No result found.";
                        return Json(new { data = new string[] { }, message = msg, success = false });
                    }
                    else
                    {
                        var ret1 = JsonConvert.SerializeObject(Arraydata);
                        var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                        string msg1 = ctr + (ctr > 1 ? " Records found!" : " Record found!");
                        return Json(new { data = json1, message = msg1, success = true });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
            }
        }

        public static List<doc_specialty> getxSpecialty(long doctor_id)
        {
            List<doc_specialty> spec = new List<doc_specialty>();

            if (doctor_id > 0)
            {
                var con_ref_spec = db1.con_DOCTOR_ref_specialty.Where(a => a.rel_DOCTOR_id == doctor_id);
                if (con_ref_spec.Count() > 0)
                {
                    foreach (var n in con_ref_spec)
                    {
                        spec.Add(new doc_specialty
                        {
                            id = n.ref_specialty.id,
                           // description = n.ref_specialty.description,
                            name = n.ref_specialty.name,
                           // actor = n.ref_specialty.actor == null ? "" : n.ref_specialty.actor
                        });
                    }
                }

            }

            return spec;
        }

        //public List<doc_specialty2> specialty {
        //    get {

        //        List<doc_specialty2> spec = new List<doc_specialty2>();

        //        if (doctor_id > 0)
        //        {
        //            //var con_ref_spec = db1.con_DOCTOR_ref_specialty.Where(a => a.rel_DOCTOR_id == doctor_id);
        //            var doc_ext = db1.DOCTOR_ext.Where(a => a.attr_name == "specialty_id" && a.rel_DOCTOR_id == doctor_id);
        //            if (doc_ext.Count() > 0)
        //            {
        //                string[] spec_ids = doc_ext.FirstOrDefault().value.Split(',');
        //                foreach (var n in spec_ids)
        //                {
        //                    long spec_id = 0;
        //                    bool i = long.TryParse(n, out spec_id);
        //                    var ref_spec = db1.ref_specialty_def.Find(spec_id);
        //                    if (ref_spec != null)
        //                    {
        //                        spec.Add(new doc_specialty2
        //                        {
        //                            id = ref_spec.id,
        //                            name = ref_spec.name == null ? "" : ref_spec.name,
        //                            code = ref_spec.code == null ? "" : ref_spec.code,
        //                            specialization = ref_spec.specialization == null ? "" : ref_spec.specialization,
        //                            provider_type = ref_spec.provider_type == null ? "" : ref_spec.provider_type
        //                        });
        //                    }
        //                }

        //            }

              
        //        }
        //        return spec;
        //    }
        //    //set {
        //    //    this._specialty = value;
        //    //}
        //}

        public static List<doc_specialty2> getSpecialty(long doctor_id)
        {
            List<doc_specialty2> spec = new List<doc_specialty2>();

            if (doctor_id > 0)
            {
                //var con_ref_spec = db1.con_DOCTOR_ref_specialty.Where(a => a.rel_DOCTOR_id == doctor_id);
                var doc_ext = db1.DOCTOR_ext.Where(a => a.attr_name== "specialty_id" && a.rel_DOCTOR_id== doctor_id);
                if (doc_ext.Count() > 0)
                {
                    string[] spec_ids = doc_ext.FirstOrDefault().value.Split(',');
                    foreach (var n in spec_ids)
                    {
                        long spec_id = 0;
                        bool i = long.TryParse(n, out spec_id);
                        var ref_spec = db1.ref_specialty.Find(spec_id);
                        if (ref_spec != null)
                        {
                            spec.Add(new doc_specialty2 {
                                id =ref_spec.id,
                                name = ref_spec.name == null?"": ref_spec.name,
                                code = ref_spec.code == null?"": ref_spec.code,
                                specialization= ref_spec.specialization == null?"": ref_spec.specialization,
                                provider_type= ref_spec.provider_type == null?"": ref_spec.provider_type
                            });
                        }
                    }

                }

                //if (doc_ext.Count() > 0)
                //{
                //    foreach (var n in con_ref_spec)
                //    {
                //        spec.Add(new doc_specialty
                //        {
                //            id = n.ref_specialty.id,
                //            description = n.ref_specialty.description,
                //            name = n.ref_specialty.name,
                //            actor = n.ref_specialty.actor == null ? "" : n.ref_specialty.actor
                //        });
                //    }
                //}

            }

            return spec;
        }

        [System.Web.Http.Route("appointment/pending")]
        [HttpGet]
        public IHttpActionResult Getpending(string patient_id = null, string user_id = null, string appointment_type = null)
        {

            //  IsRequired("appointment_type", appointment_type, 2);
            if (haserror)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }
            else
            {
                try
                {
                    // 1 Pending 
                    // get Active only
                    //var querydata1 = (from a in db.APPOINTMENTs where a.rel_ref_APPOINTMENT_status_id == 1 && a.rel_ref_status_id != 12 select a);
                    // 10 
                    if (Request.RequestUri.ToString().Contains("localhost"))
                    {
                        // to get the of APPOINTMENT status id.
                        // 
                        var ref_stat = db.ref_status.Where(a => a.dname.ToLower() == "active" && a.ref_status_type.dname.ToLower() == "appointment").FirstOrDefault();
                    }

                    var querydata1 = (from a in db.APPOINTMENTs where a.rel_ref_APPOINTMENT_status_id == 1 && a.rel_ref_status_id == 10 select a);
                    if (patient_id != null)
                    {
                        long patient_id_new = Convert.ToInt64(patient_id);
                        querydata1 = querydata1.Where(a => a.rel_SOUL_id == patient_id_new);
                    }

                    long user_id_new = 0;
                    if (user_id != null)
                    {
                        user_id_new = Convert.ToInt64(user_id);
                        querydata1 = querydata1.Where(a => a.create_by__USER_id == user_id_new);
                    }
                    List<new_APPOINTMENT> new_APPOINTMENT = new List<new_APPOINTMENT>();
                    if (querydata1.Count() > 0)
                    {

                        foreach (var i in querydata1)
                        {
                            string doc_name = "", doc_specialty = null, image_url = null;
                            List<appt_type> appt = new List<appt_type>();
                            List<appt_type> doc_appt = new List<appt_type>();

                            if (i.rel_ref_APPOINTMENT_type_id != null)
                            {
                                appt.Add(new appt_type
                                {
                                    id = i.rel_ref_APPOINTMENT_type_id.Value,
                                    type = i.ref_APPOINTMENT_type.dname
                                });
                            }

                            // retrieve info from Appointment_ext
                            var appt_ext = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == i.id);
                            double appt_ext_rate = 0; string appt_ext_timeslot = ""; long doctor_id = 0;

                            bool hasProposed = false;
                            List<doctor_profile> prop_doc = new List<doctor_profile>();

                            // && a.attr_name == "doctor_rating"
                            if (appt_ext.Count() > 0)
                            {
                                foreach (var k in appt_ext)
                                {
                                    switch (k.attr_name)
                                    {
                                        case "doctor_rating":
                                            bool bTemp = double.TryParse(k.value, out appt_ext_rate);

                                            break;
                                        case "time_slot":
                                            appt_ext_timeslot = k.value;
                                            break;

                                        case "doctor_id":
                                            bool bTemp2 = long.TryParse(k.value, out doctor_id);
                                            break;

                                        case "proposed_doctor":
                                            hasProposed = true;
                                            // id|doctor_id|date|time_slot
                                            string[] doc1 = k.value.Split('|');
                                            long prop_appt = Convert.ToInt64(doc1[0]);
                                            long prop_doctor = Convert.ToInt64(doc1[0]);
                                            string prop_date = doc1[1];
                                            string prop_time = doc1[2];

                                            prop_doc.Add(new doctor_profile
                                            {
                                                //id = k.id,
                                                id = prop_doctor,
                                                date = prop_date,
                                                time_slot = prop_time
                                            });
                                            break;
                                    }
                                }
                            }

                            //var doctor_id = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == i.id && a.attr_name == "doctor_id").FirstOrDefault();
                            List<doctor_profile> doc = new List<doctor_profile>();

                            if (doctor_id > 0)
                            {
                                #region "has Doctors"
                                //doc_id = Convert.ToInt64(doctor_id.value);
                                var doctor_name = db.DOCTORs.Where(a => a.id == doctor_id).FirstOrDefault();
                                //var con_ref_spec = db.con_DOCTOR_ref_specialty.Where(a => a.rel_DOCTOR_id == doctor_id);

                                string namefirst = (doctor_name == null) ? "" : doctor_name.name_first;
                                string namelast = (doctor_name == null) ? "" : doctor_name.name_last;
                                doc_name = namefirst + " " + namelast;
                                image_url = doctor_name.image_url;
                                //long spec_id = Convert.ToInt64(ref_specialty.rel_ref_specialty_id);
                                //var specialty = db.ref_specialty.Where(a => a.id == spec_id);
                                //doc_specialty = specialty.name;

                                List<doc_specialty2> spec = new List<Models.doc_specialty2>();
                                spec = getSpecialty(doctor_id);
                                //if (con_ref_spec.Count() > 0)
                                //{
                                //    foreach (var n in con_ref_spec) {
                                //        spec.Add(new Models.doc_specialty {
                                //            id = n.id,
                                //            description = n.ref_specialty.description,
                                //            name = n.ref_specialty.name,
                                //            actor = n.ref_specialty.actor == null?"": n.ref_specialty.actor
                                //        });
                                //    }
                                //}



                                //string addr2 = doctor_name.addr_address2 == null ? "" : " " + doctor_name.addr_address2;
                                zip_search_address doc_address = new zip_search_address
                                {
                                    address1 = doctor_name.addr_address1 == null ? "" : doctor_name.addr_address1,
                                    address2 = doctor_name.addr_address2 == null ? "" : doctor_name.addr_address2,
                                    zip = doctor_name.ref_zip.zip,
                                    county = doctor_name.ref_zip.city_county,
                                    city = doctor_name.ref_zip.city_name,
                                    state = doctor_name.ref_zip.city_state,
                                    lat = doctor_name.ref_zip.city_lat,
                                    lng = doctor_name.ref_zip.city_lon
                                };


                                string vappt_name = "", vtime_slot = ""; long vappt_id = 0; double doc_fee = 0;

                                var doc_ext = db.DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doctor_name.id);

                                if (doc_ext.Count() > 0)
                                {

                                    foreach (var n in doc_ext)
                                    {
                                        switch (n.attr_name)
                                        {
                                            case "drappttype":
                                                // a.attr_name == "drappttype" &&
                                                bool isAppt = long.TryParse(n.value, out vappt_id);
                                                if (vappt_id > 0)
                                                {
                                                    var n1 = db.ref_APPOINTMENT_type.Find(vappt_id);
                                                    vappt_name = n1.dname;
                                                    doc_appt.Add(new appt_type
                                                    {
                                                        id = vappt_id,
                                                        type = vappt_name
                                                    });
                                                }
                                                break;

                                            case "fee":
                                                bool bTemp = double.TryParse(n.value, out doc_fee);
                                                break;
                                            case "time_slot":
                                                vtime_slot = n.value == null ? "" : n.value;
                                                break;
                                        }
                                    }
                                }


                                // get if Doctor is 
                                int fave_doc = 0;
                                if (user_id_new > 0)
                                {
                                    var con_fav = db.con_USER_favorite_DOCTOR.Where(a => a.create_by__USER_id == user_id_new && a.rel_doctor_id == doctor_name.id);

                                    if (con_fav.Count() > 0)
                                    {
                                        if (con_fav.FirstOrDefault().favor == true)
                                            fave_doc = 1;
                                        else
                                            fave_doc = 0;
                                    }
                                }



                                doc.Add(new doctor_profile
                                {
                                    //public double fee { get; set; }
                                    id = doctor_name.id,
                                    doctor_fee = doc_fee,
                                    //name = doc_name,
                                    rating = appt_ext_rate,
                                    favorite = fave_doc,
                                    first_name = doctor_name.name_first == null ? "" : doctor_name.name_first,
                                    last_name = doctor_name.name_last == null ? "" : doctor_name.name_last,
                                    middle_name = doctor_name.name_middle == null ? "" : doctor_name.name_middle,
                                    email = doctor_name.email,
                                    gender = doctor_name.gender,
                                    title = doctor_name.title,
                                    phone = doctor_name.phone,
                                    license = doctor_name.license_no == null ? "" : doctor_name.license_no,
                                    npi = doctor_name.NPI,
                                    orgname = doctor_name.organ_name == null ? "" : doctor_name.organ_name,
                                    pecoscert = doctor_name.pecos_cert == null ? false : true,
                                    date = i.date == null ? "" : i.date.Value.ToUniversalTime().GetDateTimeFormats()[3],
                                    time_slot = vtime_slot,
                                    image_url = (image_url == null) ? "" : image_url,

                                    //public string timeslot { get; set; }
                                    //timeslot =appt_ext_timeslot ,
                                    bio = doctor_name.bio == null ? "" : doctor_name.bio,
                                    specialties = spec,
                                    appointment_type = doc_appt,
                                    address = doc_address
                                });
                                #endregion

                            }
                            else if(hasProposed) // no doctor_id, but has proposed_doctor
                            {
                                #region "hasProposed Doctors"
                            
                                foreach (var d in prop_doc)
                                {
                                    doc_appt = new List<appt_type>();
                                    var doctor_name = db.DOCTORs.Find(d.id);
                                    //var con_ref_spec = db.con_DOCTOR_ref_specialty.Where(a => a.rel_DOCTOR_id == doctor_id);

                                    string namefirst = (doctor_name == null) ? "" : doctor_name.name_first;
                                    string namelast = (doctor_name == null) ? "" : doctor_name.name_last;
                                    doc_name = namefirst + " " + namelast;
                                    image_url = (doctor_name == null) ? "" : doctor_name.image_url;

                                    List<doc_specialty2> spec = new List<Models.doc_specialty2>();
                                    spec = getSpecialty(d.id);

                                    //string addr2 = doctor_name.addr_address2 == null ? "" : " " + doctor_name.addr_address2;
                                    zip_search_address doc_address = new zip_search_address
                                    {
                                        address1= doctor_name.addr_address1 == null ? "" : doctor_name.addr_address1,
                                        address2 = doctor_name.addr_address2 == null ? "" : doctor_name.addr_address2,
                                        zip = doctor_name.ref_zip.zip,
                                        county = doctor_name.ref_zip.city_county,
                                        city = doctor_name.ref_zip.city_name,
                                        state = doctor_name.ref_zip.city_state,
                                        lat = doctor_name.ref_zip.city_lat,
                                        lng = doctor_name.ref_zip.city_lon
                                    };


                                    string vappt_name = "", vtime_slot = ""; long vappt_id = 0; double doc_fee = 0;

                                    var doc_ext = db.DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doctor_name.id);

                                    if (doc_ext.Count() > 0)
                                    {

                                        foreach (var n in doc_ext)
                                        {
                                            switch (n.attr_name)
                                            {
                                                case "drappttype":
                                                    // a.attr_name == "drappttype" &&
                                                    bool isAppt = long.TryParse(n.value, out vappt_id);
                                                    if (vappt_id > 0)
                                                    {
                                                        var n1 = db.ref_APPOINTMENT_type.Find(vappt_id);
                                                        vappt_name = n1.dname;
                                                        doc_appt.Add(new appt_type
                                                        {
                                                            id = vappt_id,
                                                            type = vappt_name
                                                        });
                                                    }
                                                    break;

                                                case "fee":
                                                    bool bTemp = double.TryParse(n.value, out doc_fee);
                                                    break;
                                                case "time_slot":
                                                    vtime_slot = n.value == null ? "" : n.value;
                                                    break;
                                            }



                                        }
                                    }

                                    // get if Doctor is 
                                    int fave_doc = 0;
                                    if (user_id_new > 0)
                                    {
                                        var con_fav = db.con_USER_favorite_DOCTOR.Where(a => a.create_by__USER_id == user_id_new && a.rel_doctor_id == doctor_name.id);

                                        if (con_fav.Count() > 0)
                                        {
                                            if (con_fav.FirstOrDefault().favor == true)
                                                fave_doc = 1;
                                            else
                                                fave_doc = 0;
                                        }
                                    }

                                    // update list proposed_doctors
                                    d.doctor_fee = doc_fee;
                                    d.rating = appt_ext_rate;
                                    d.favorite = fave_doc;
                                    d.first_name = doctor_name.name_first == null ? "" : doctor_name.name_first;
                                    d.last_name = doctor_name.name_last == null ? "" : doctor_name.name_last;
                                    d.middle_name = doctor_name.name_middle == null ? "" : doctor_name.name_middle;
                                    d.email = doctor_name.email;
                                    d.gender = doctor_name.gender;
                                    d.title = doctor_name.title;
                                    d.phone = doctor_name.phone;
                                    d.license = doctor_name.license_no == null ? "" : doctor_name.license_no;
                                    d.npi = doctor_name.NPI;
                                    d.orgname = doctor_name.organ_name == null ? "" : doctor_name.organ_name;
                                    d.pecoscert = doctor_name.pecos_cert == null ? false : true;
                                    d.time_slot = vtime_slot;
                                    d.image_url = (image_url == null) ? "" : image_url;

                                    d.bio = doctor_name.bio == null ? "" : doctor_name.bio;
                                    d.specialties = spec;
                                    d.appointment_type = doc_appt;
                                    d.address = doc_address;
                                }
                                #endregion
                            }


                            var _soul = db.SOULs.Find(i.rel_SOUL_id);
                            string name_first = "", name_last = "";
                            if (_soul != null)
                            {
                                name_first = _soul.name_first == null ? "" : _soul.name_first;
                                name_last = _soul.name_last == null ? "" : _soul.name_last;
                            }

                            new_APPOINTMENT.Add(new new_APPOINTMENT
                            {
                                appointment_id = i.id,
                                patient_id = i.rel_SOUL_id.Value,
                                first_name = name_first,
                                last_name = name_last,

                                status = (i.rel_ref_status_id.Value == 10) ? "Active" : "Inactive",
                                time_slot = appt_ext_timeslot,
                                date = i.date == null ? "" : i.date.Value.ToUniversalTime().GetDateTimeFormats()[3],
                                //appointment = (i.ref_APPOINTMENT_type == null)?"": i.ref_APPOINTMENT_type.dname,
                                user_id = i.create_by__USER_id.Value,
                                doctor_id = doctor_id,
                                doctor_name = doctor_id > 0 ? doc_name :"",
                                rating = appt_ext_rate,
                                doctor = doc.Count() > 0 ? doc : prop_doc,
                                //proposed_doctor = prop_doc,
                                date_created = i.dt_create.GetDateTimeFormats()[56],
                                appointment_type = appt
                                //specialty = (doc_specialty == null) ? "" : doc_specialty,
                                //doctor_image = (image_url == null) ? "" : image_url
                            });

                        }


                    }
                    if (new_APPOINTMENT.Count() == 0)
                    {
                        string msg = "No result found.";
                        return Json(new { data = new string[] { }, message = msg, success = false });
                    }
                    else
                    {
                        var ret1 = JsonConvert.SerializeObject(new_APPOINTMENT);
                        var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                        return Json(new { data = json1, message = new_APPOINTMENT.Count() + (new_APPOINTMENT.Count() > 1 ? " Records found!" : " Record found!"), success = true });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
            }
        }

        [System.Web.Http.Route("appointment/history")]
        [HttpGet]
        public IHttpActionResult Gethistory(string patient_id = null, string user_id = null, string appointment_type = null)
        {

            //  IsRequired("appointment_type", appointment_type, 2);
            if (haserror)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }
            else
            {
                try
                {
                    if (Request.RequestUri.ToString().Contains("localhost"))
                    {
                        var ref_appt_stat = db.ref_APPOINTMENT_status.Where(a => a.dname.ToLower() == "history");
                        // 3 History

                        var ref_stat = db.ref_status.Where(a => a.ref_status_type.dname.ToLower() == "appointment").FirstOrDefault();
                        // 10 Active // 11 Inactive
                        // 12 Deleted
                    }

                    // 3 history 
                    // get history Active|Cancel
                 
                    var querydata1 = (from a in db.APPOINTMENTs where 
                                      (a.rel_ref_APPOINTMENT_status_id == 3
                                      || a.date < dt.Date)
                                      && a.rel_ref_status_id != 12
                                      select a);

                    if (patient_id != null)
                    {
                        long patient_id_new = Convert.ToInt64(patient_id);
                        querydata1 = querydata1.Where(a => a.rel_SOUL_id == patient_id_new);
                    }

                    long user_id_new = 0;
                    if (user_id != null)
                    {
                        user_id_new = Convert.ToInt64(user_id);
                        querydata1 = querydata1.Where(a => a.create_by__USER_id == user_id_new);
                    }
                    List<new_APPOINTMENT> new_APPOINTMENT = new List<new_APPOINTMENT>();
                    //if (querydata1.Count() > 0)
                    //{

                        foreach (var i in querydata1)
                        {
                            string doc_name = "", doc_specialty = null, image_url = null;
                            List<appt_type> appt = new List<appt_type>();
                            List<appt_type> doc_appt = new List<appt_type>();
                            if (i.rel_ref_APPOINTMENT_type_id != null)
                            {
                                appt.Add(new appt_type
                                {
                                    id = i.rel_ref_APPOINTMENT_type_id.Value,
                                    type = i.ref_APPOINTMENT_type.dname
                                });
                            }

                            // retrieve info from Appointment_ext
                            var appt_ext = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == i.id);
                            double appt_ext_rate = 0; string appt_ext_timeslot = ""; long doctor_id = 0;

                            if (appt_ext.Count() > 0)
                            {
                                foreach (var k in appt_ext) {
                                    switch (k.attr_name)
                                    {
                                        case "doctor_rating":
                                            bool bTemp = double.TryParse(k.value, out appt_ext_rate);

                                            break;
                                        case "time_slot":
                                            appt_ext_timeslot = k.value;
                                            break;
                                        case "doctor_id":
                                            bool bTemp2 = long.TryParse(k.value, out doctor_id);
                                            break;
                                    }
                                }
                            }

                            //var doctor_id = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == i.id && a.attr_name == "doctor_id").FirstOrDefault();

                            List<doctor_profile> doc = new List<doctor_profile>();

                            if (doctor_id > 0)
                            {
                                //doc_id = Convert.ToInt64(doctor_id.value);
                                var doctor_name = db.DOCTORs.Where(a => a.id == doctor_id).FirstOrDefault();
                                //var con_ref_spec = db.con_DOCTOR_ref_specialty.Where(a => a.rel_DOCTOR_id == doctor_id);

                                string namefirst = (doctor_name == null) ? "" : doctor_name.name_first;
                                string namelast = (doctor_name == null) ? "" : doctor_name.name_last;
                                doc_name = namefirst + " " + namelast;
                                //doc_name = doctor_name.name_first + " " + doctor_name.name_last;
                                //image_url = doctor_name.image_url;
                                //long spec_id = Convert.ToInt64(ref_specialty.rel_ref_specialty_id);

                                //doc_specialty = specialty.name;

                                List<doc_specialty2> spec = new List<Models.doc_specialty2>();
                                spec = getSpecialty(doctor_id);
                                //if (con_ref_spec.Count() > 0) {
                                //    foreach (var n in con_ref_spec) {

                                //        spec.Add(new Models.doc_specialty {
                                //             id = n.id,
                                //             description = n.ref_specialty.description,
                                //             name  = n.ref_specialty.name,
                                //             actor = n.ref_specialty.actor == null? "" : n.ref_specialty.actor
                                //        });
                                //    }
                                //}

                                //string addr2 = doctor_name.addr_address2 == null ? "" : " " + doctor_name.addr_address2;
                                zip_search_address doc_address = new zip_search_address {
                                    address1 = doctor_name.addr_address1 == null ? "" : doctor_name.addr_address1,
                                    address2 = doctor_name.addr_address2 == null ? "" : " " + doctor_name.addr_address2,
                                    zip = doctor_name.ref_zip.zip,
                                    county = doctor_name.ref_zip.city_county,
                                    city = doctor_name.ref_zip.city_name,
                                    state = doctor_name.ref_zip.city_state,
                                    lat = doctor_name.ref_zip.city_lat,
                                    lng = doctor_name.ref_zip.city_lon
                                };


                                string vappt_name = "", vtime_slot = "", vdate ="";
                                long vappt_id = 0; double doc_fee = 0;

                                var doc_ext = db.DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doctor_name.id);
                                if (doc_ext.Count() > 0)
                                {
                                    foreach (var n in doc_ext) {
                                        switch (n.attr_name)
                                        {
                                            case "drappttype":
                                                bool isAppt = long.TryParse(n.value, out vappt_id);

                                                if (vappt_id > 0)
                                                {
                                                    var n1 = db.ref_APPOINTMENT_type.Find(vappt_id);
                                                    vappt_name = n1.dname;
                                                    doc_appt.Add(new appt_type {
                                                        id = vappt_id,
                                                        type = vappt_name
                                                    });
                                                }
                                                break;

                                            case "fee":
                                                bool bTemp = double.TryParse(n.value, out doc_fee);

                                                break;
                                            case "time_slot":
                                                string[] s = n.value.Split('|');

                                                vdate = s[0];
                                                vtime_slot = s[1];
                                                
                                                break;
                                        }
                                    }
                                }


                                int fave_doc = 0;
                                if (user_id_new > 0) {
                                    var con_fav = db.con_USER_favorite_DOCTOR.Where(a => a.create_by__USER_id == user_id_new && a.rel_doctor_id == doctor_name.id);

                                    if (con_fav.Count() > 0)
                                    {
                                        if (con_fav.FirstOrDefault().favor == true)
                                            fave_doc = 1;
                                        else
                                            fave_doc = 0;
                                    }
                                }
                                
                                doc.Add(new doctor_profile {
                                    id = doctor_name.id,
                                    //name = doc_name,
                                    first_name = doctor_name.name_first == null ? "" : doctor_name.name_first,
                                    last_name = doctor_name.name_last == null ? "" : doctor_name.name_last,
                                    middle_name = doctor_name.name_middle == null ? "" : doctor_name.name_middle,
                                    email = doctor_name.email,
                                    // rating
                                    favorite = fave_doc,
                                    gender = doctor_name.gender == null ?"": doctor_name.gender,
                                    title = doctor_name.title == null? "": doctor_name.title,
                                    phone = doctor_name.phone,
                                    license = doctor_name.license_no ==null?"":doctor_name.license_no,
                                    pecoscert = doctor_name.pecos_cert == null ? false : true,
                                    npi = doctor_name.NPI,
                                    orgname = doctor_name.organ_name ==null ?"": doctor_name.organ_name,
                                    doctor_fee = doc_fee,
                                    rating = appt_ext_rate,
                                    date = vdate,
                                    time_slot = vtime_slot,
                                    bio = doctor_name.bio == null ? "" : doctor_name.bio,
                                    image_url = image_url == null ? "" : image_url,

                                    address = doc_address,
                                    specialties = spec,
                                    appointment_type = doc_appt
                                });
                            }

                            var _soul = db.SOULs.Find(i.rel_SOUL_id);

                            new_APPOINTMENT.Add(new new_APPOINTMENT
                            {
                                appointment_id = i.id,
                                patient_id = i.rel_SOUL_id.Value,
                                first_name = _soul.name_first == null ? "" : _soul.name_first,
                                last_name = _soul.name_last == null ? "" : _soul.name_last,
                                time_slot = appt_ext_timeslot,
                                status = (i.rel_ref_status_id.Value == 10) ? "Active" : "Cancelled",
                                date = i.date == null ? "" : i.date.Value.GetDateTimeFormats()[3],

                                user_id = i.create_by__USER_id.Value,
                                doctor_id = doctor_id,
                                doctor_name = doc_name,
                                rating = appt_ext_rate,
                                doctor = doc,
                                date_created = i.dt_create.GetDateTimeFormats()[56],
                                appointment_type = appt
                                //doctor_name =  doc_name,
                                //specialty = (doc_specialty == null) ? "" : doc_specialty,
                                //doctor_image = (image_url == null) ? "" : image_url
                            });

                        }
                    //}

                    if (new_APPOINTMENT.Count() == 0)
                    {
                        string msg = "No result found.";
                        return Json(new { data = new string[] { }, message = msg, success = false });
                    }
                    else
                    {
                        var ret1 = JsonConvert.SerializeObject(new_APPOINTMENT);
                        var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                        return Json(new { data = json1, message = new_APPOINTMENT.Count() + (new_APPOINTMENT.Count() > 1 ? " Records found!" : " Record found!"), success = true });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
            }
        }

        [System.Web.Http.Route("appointment/current")]
        [HttpGet]
        public IHttpActionResult Getcurrent(string patient_id = null, string user_id = null, string appointment_type = null)
        {

            //  IsRequired("appointment_type", appointment_type, 2);
            if (haserror)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }
            else
            {
                try
                {
                    // 2 Current
                    // get Active only
                    // 10 Active
                    if (Request.RequestUri.ToString().Contains("localhost"))
                    {
                        var ref_stat = db.ref_status.Where(a => a.dname.ToLower() == "active" && a.ref_status_type.dname.ToLower() == "appointment").FirstOrDefault();
                    }
                        
                    var querydata1 = (from a in db.APPOINTMENTs where a.rel_ref_APPOINTMENT_status_id == 2 && a.rel_ref_status_id == 10 select a);
                    if (patient_id != null)
                    {
                        long patient_id_new = Convert.ToInt64(patient_id);
                        querydata1 = querydata1.Where(a => a.rel_SOUL_id == patient_id_new);
                    }

                    long user_id_new = 0;
                    if (user_id != null)
                    {
                        user_id_new = Convert.ToInt64(user_id);
                        querydata1 = querydata1.Where(a => a.create_by__USER_id == user_id_new);
                    }
                    List<new_APPOINTMENT> new_APPOINTMENT = new List<new_APPOINTMENT>();
                    if (querydata1.Count() > 0)
                    {

                        foreach (var i in querydata1)
                        {
                            string doc_name = "", doc_specialty = null, image_url = null;
                            List<appt_type> appt = new List<appt_type>();
                            List<appt_type> doc_appt = new List<appt_type>();
                            if (i.rel_ref_APPOINTMENT_type_id != null)
                            {
                                appt.Add(new appt_type
                                {
                                    id = i.rel_ref_APPOINTMENT_type_id.Value,
                                    type = i.ref_APPOINTMENT_type.dname
                                });
                            }

                            // retrieve info from Appointment_ext
                            var appt_ext = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == i.id);
                            double appt_ext_rate = 0; string appt_ext_timeslot = ""; long doctor_id = 0;

                            if (appt_ext.Count() > 0)
                            {
                                foreach (var k in appt_ext) {
                                    switch (k.attr_name) {
                                        case "doctor_rating":
                                            bool bTemp = double.TryParse(k.value, out appt_ext_rate);
                                            break;
                                        case "time_slot":
                                            appt_ext_timeslot = k.value;
                                            break;
                                        case "doctor_id":
                                            bool bTemp2 = long.TryParse(k.value, out doctor_id);
                                            break;
                                    }
                                }
                            }

                            //var doctor_id = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == i.id && a.attr_name == "doctor_id").FirstOrDefault();
                            List<doctor_profile> doc = new List<doctor_profile>();

                            if (doctor_id > 0)
                            {
                                //doc_id = Convert.ToInt64(doctor_id.value);
                                var doctor_name = db.DOCTORs.Where(a => a.id == doctor_id).FirstOrDefault();
                                //var ref_specialty = db.con_DOCTOR_ref_specialty.Where(a => a.rel_DOCTOR_id == doctor_id).FirstOrDefault();
                                //var con_ref_spec = db.con_DOCTOR_ref_specialty.Where(a => a.rel_DOCTOR_id == doctor_id);

                                string namefirst = (doctor_name == null) ? "" : doctor_name.name_first;
                                string namelast = (doctor_name == null) ? "" : doctor_name.name_last;
                                doc_name = namefirst + " " + namelast;
                                //doc_name = doctor_name.name_first + " " + doctor_name.name_last;
                                // image_url = doctor_name.image_url;
                                //long spec_id = Convert.ToInt64(ref_specialty.rel_ref_specialty_id);

                                //var specialty = db.ref_specialty.Where(a => a.id == spec_id);
                                // doc_specialty = specialty.name;
                                List<doc_specialty2> spec = new List<Models.doc_specialty2>();
                                spec = getSpecialty(doctor_id);
                                //if (con_ref_spec.Count() > 0)
                                //{
                                //    foreach (var n in con_ref_spec)
                                //    {
                                //        spec.Add(new Models.doc_specialty {
                                //            id = n.id,
                                //            description = n.ref_specialty.description,
                                //            name = n.ref_specialty.name,
                                //            actor = n.ref_specialty.actor == null? "" : n.ref_specialty.actor
                                //        });
                                //    }
                                //}

                                //string addr2 = doctor_name.addr_address2 == null ? "" : doctor_name.addr_address2;
                                zip_search_address doc_address = new zip_search_address {
                                    address1 = doctor_name.addr_address1 == null ? "" : doctor_name.addr_address1,
                                    address2 = doctor_name.addr_address2 == null ? "" : doctor_name.addr_address2,
                                    zip = doctor_name.ref_zip.zip,
                                    city = doctor_name.ref_zip.city_name,
                                    state = doctor_name.ref_zip.city_state,
                                    county = doctor_name.ref_zip.city_county,
                                    lat = doctor_name.ref_zip.city_lat,
                                    lng = doctor_name.ref_zip.city_lon
                                };

                                string vappt_name = "", vtime_slot = ""; long vappt_id = 0; double doc_fee = 0;

                                var doc_ext = db.DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doctor_name.id);
                                if (doc_ext.Count() > 0)
                                {
                                    foreach (var n in doc_ext)
                                    {
                                        switch (n.attr_name)
                                        {
                                            case "drappttype":
                                                bool isAppt = long.TryParse(n.value, out vappt_id);

                                                if (vappt_id > 0)
                                                {
                                                    var n1 = db.ref_APPOINTMENT_type.Find(vappt_id);
                                                    vappt_name = n1.dname;

                                                    doc_appt.Add(new appt_type {
                                                        id = vappt_id,
                                                        type = vappt_name
                                                    });

                                                }
                                                break;

                                            case "fee":
                                                bool bTemp = double.TryParse(n.value, out doc_fee);
                                                break;
                                            case "time_slot":
                                                vtime_slot = n.value == null ? "" : n.value;
                                                break;
                                        }
                                    }
                                }

                                // get doctor_id if user_id
                                int fave_doc = 0;
                                if (user_id_new > 0) {
                                    var con_fav = db.con_USER_favorite_DOCTOR.Where(a => a.create_by__USER_id == user_id_new && a.rel_doctor_id == doctor_name.id);

                                    if (con_fav.Count() > 0)
                                    {
                                        if (con_fav.FirstOrDefault().favor == true)
                                            fave_doc = 1;
                                        else
                                            fave_doc = 0;
                                    }
                                }
                                

                                doc.Add(new doctor_profile {
                                    id = doctor_name.id,
                                    doctor_fee = doc_fee,
                                    bio = doctor_name.bio == null ? "" : doctor_name.bio,
                                    time_slot = vtime_slot,
                                    //name = doc_name,
                                    favorite = fave_doc,
                                    first_name = doctor_name.name_first == null ? "" : doctor_name.name_first,
                                    last_name = doctor_name.name_last == null ? "" : doctor_name.name_last,
                                    middle_name = doctor_name.name_middle == null ? "" : doctor_name.name_middle,
                                    image_url = doctor_name.image_url == null ? "" : doctor_name.image_url,
                                    email = doctor_name.email == null ? "" : doctor_name.email,
                                    gender = doctor_name.gender == null ? "" : doctor_name.gender,
                                    title = doctor_name.title == null ? "" : doctor_name.title,
                                    phone = doctor_name.phone == null ? "" : doctor_name.phone,
                                    license = doctor_name.license_no == null ? "" : doctor_name.license_no,
                                    pecoscert = doctor_name.pecos_cert == null ? false : true,
                                    npi = doctor_name.NPI == null ? "" : doctor_name.NPI,
                                    orgname = doctor_name.organ_name == null ? "" : doctor_name.organ_name,
                                    rating = appt_ext_rate,

                                    specialties = spec,
                                    appointment_type = doc_appt,
                                    address = doc_address,
                                });
                            }

                            var _soul = db.SOULs.Find(i.rel_SOUL_id);

                            new_APPOINTMENT.Add(new new_APPOINTMENT
                            {
                                appointment_id = i.id,
                                patient_id = i.rel_SOUL_id.Value,
                                first_name = _soul == null ? "" : _soul.name_first,
                                last_name = _soul == null ? "" : _soul.name_last,

                                status = (i.rel_ref_status_id.Value == 10) ? "Active" : "Inactive",
                                date = i.date == null ? "" : i.date.Value.GetDateTimeFormats()[3],
                                //appointment = i.ref_APPOINTMENT_type.dname,
                                rating = appt_ext_rate,
                                time_slot = appt_ext_timeslot,
                                user_id = i.create_by__USER_id.Value,
                                doctor_name = doc_name,
                                doctor_id = doctor_id,
                                doctor = doc,
                                date_created = i.dt_create.GetDateTimeFormats()[56],
                                appointment_type = appt
                                //specialty = (doc_specialty == null) ? "" : doc_specialty,
                                //doctor_image = (image_url == null) ? "" : image_url
                            });

                        }


                    }
                    if (new_APPOINTMENT.Count() == 0)
                    {
                        string msg = "No result found.";
                        return Json(new { data = new string[] { }, message = msg, success = false });
                    }
                    else
                    {
                        var ret1 = JsonConvert.SerializeObject(new_APPOINTMENT);
                        var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                        return Json(new { data = json1, message = new_APPOINTMENT.Count() + (new_APPOINTMENT.Count() > 1 ? " Records found!" : " Record found!"), success = true });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
            }
        }


        [System.Web.Http.Route("appointment")]
        [HttpGet]
        public IHttpActionResult Getappointment(string appointment_id = null)
        {
            IsRequired("appointment_id", appointment_id, 2);

            if (haserror)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }
            else
            {
                try
                {
                    List<new_APPOINTMENT> new_APPOINTMENT = new List<new_APPOINTMENT>();

                    long appointment_id_new = Convert.ToInt64(appointment_id);
                    //var querydata1 = (from a in db.APPOINTMENTs
                    //                  where (a.id == appointment_id_new)
                    //                  select new
                    //                  {
                    //                      appointment = new new_APPOINTMENT
                    //                      {
                    //                          appointment_id = a.id,
                    //                          appointment = a.ref_APPOINTMENT_type.dname,
                    //                          user_id = a.create_by__USER_id.Value,
                    //                          patient_id = a.rel_SOUL_id.Value,
                    //                          date = a.date.ToString(),
                    //                          status = (a.rel_ref_status_id.Value == 7) ? "Active" : "Inactive"
                    //                      }
                    //                  });

                    var querydata1 = (from a in db.APPOINTMENTs
                                      orderby a.rel_SOUL_id, a.id
                                      where (a.id == appointment_id_new && a.rel_ref_status_id != 12)
                                      select a);

                    if (querydata1.Count() == 0)
                    {
                        string msg = "No result found.";
                        return Json(new { data = new string[] { }, message = msg, success = false });
                    }
                    else
                    {


                        foreach (var i in querydata1)
                        {
                            //long doc_id = 0;
                            string doc_name = "", doc_specialty = null, image_url = null;

                            // retrieve info from Appointment_ext
                            var appt_ext = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == i.id);
                            string appt_ext_timeslot = "";
                            double appt_ext_rate = 0;
                            long doctor_id = 0;

                            //var doctor_id = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == i.id && a.attr_name == "doctor_id").FirstOrDefault();
                            if (appt_ext.Count() > 0)
                            {
                                foreach (var k in appt_ext) {
                                    switch (k.attr_name) {
                                        case "doctor_id":
                                            bool bTemp = long.TryParse(k.value, out doctor_id);
                                            break;
                                        case "time_slot":
                                            appt_ext_timeslot = k.value;
                                            break;
                                        case "doctor_rating":
                                            bool bTemp1 = double.TryParse(k.value, out appt_ext_rate);
                                            break;

                                    }
                                }
                            }

                            List<doctor_profile> doc = new List<doctor_profile>();
                            List<appt_type> appt = new List<appt_type>();
                            List<appt_type> doc_appt = new List<appt_type>();
                            if (i.rel_ref_APPOINTMENT_type_id != null)
                            {
                                appt.Add(new appt_type
                                {
                                    id = i.rel_ref_APPOINTMENT_type_id.Value,
                                    type = i.ref_APPOINTMENT_type.dname
                                });
                            }

                            if (doctor_id > 0)
                            {
                                //doc_id = Convert.ToInt64(doctor_id.value);
                                var doctor_name = db.DOCTORs.Find(doctor_id);
                                //var con_ref_spec = db.con_DOCTOR_ref_specialty.Where(a => a.rel_DOCTOR_id == doctor_id);

                                string namefirst = (doctor_name.name_first == null) ? "" : doctor_name.name_first;
                                string namelast = (doctor_name.name_last == null) ? "" : doctor_name.name_last;
                                doc_name = namefirst + " " + namelast;
                                //doc_name = doctor_name.name_first + " " + doctor_name.name_last;
                                image_url = doctor_name.image_url;
                                //long spec_id = Convert.ToInt64(ref_specialty.rel_ref_specialty_id);
                                //var specialty = db.ref_specialty.Where(a => a.id == spec_id).FirstOrDefault();
                                //doc_specialty = specialty.name;

                                List<doc_specialty2> spec = new List<Models.doc_specialty2>();
                                spec = getSpecialty(doctor_id);
                                //if (con_ref_spec.Count() > 0) {
                                //    foreach (var n in con_ref_spec) {
                                //        spec.Add(new Models.doc_specialty {
                                //            id = n.id,
                                //            description = n.ref_specialty.description,
                                //            name = n.ref_specialty.name,
                                //            actor = n.ref_specialty.actor == null?"":n.ref_specialty.actor
                                //        });
                                //    }
                                //}

                                //string addr2 = doctor_name.addr_address2 == null ? "" : " " + doctor_name.addr_address2;
                                zip_search_address doc_address = new zip_search_address {
                                    address1 = doctor_name.addr_address1 == null ? "" : doctor_name.addr_address1,
                                    address2 = doctor_name.addr_address2 == null ? "" : doctor_name.addr_address2,
                                    zip = doctor_name.ref_zip.zip,
                                    city = doctor_name.ref_zip.city_name,
                                    county = doctor_name.ref_zip.city_county,
                                    state = doctor_name.ref_zip.city_state,
                                    lat = doctor_name.ref_zip.city_lat,
                                    lng = doctor_name.ref_zip.city_lon
                                };

                                string vappt_name = "", vtime_slot = ""; long vappt_id = 0; double doc_fee = 0;

                                var doc_ext = db.DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doctor_name.id);
                                if (doc_ext.Count() > 0)
                                {
                                    foreach (var n in doc_ext) {
                                        switch (n.attr_name) {
                                            case "drappttype":
                                                bool isAppt = long.TryParse(n.value, out vappt_id);
                                                if (vappt_id > 0)
                                                {
                                                    var n1 = db.ref_APPOINTMENT_type.Find(vappt_id);
                                                    vappt_name = n1.dname;

                                                    doc_appt.Add(new appt_type {
                                                        id = vappt_id,
                                                        type = vappt_name
                                                    });
                                                }
                                                break;

                                            case "fee":
                                                bool bTemp = double.TryParse(n.value, out doc_fee);
                                                break;
                                            case "time_slot":
                                                vtime_slot = n.value == null ? "" : n.value;
                                                break;
                                        }
                                    }
                                }

                                // get if Doctor is 
                                int fave_doc = 0;
                                //if (user_id_new > 0)
                                //{
                                    var con_fav = db.con_USER_favorite_DOCTOR.Where(a => a.create_by__USER_id == doctor_name.create_by__USER_id && a.rel_doctor_id == doctor_name.id);

                                    if (con_fav.Count() > 0)
                                    {
                                        if (con_fav.FirstOrDefault().favor == true)
                                            fave_doc = 1;
                                        else
                                            fave_doc = 0;
                                    }
                                //}


                                doc.Add(new doctor_profile {
                                    id = doctor_name.id,
                                    doctor_fee = doc_fee,
                                    bio = doctor_name.bio == null ? "" : doctor_name.bio,
                                    //name = doc_name,
                                    favorite = fave_doc,
                                    first_name = doctor_name.name_first == null ? "" : doctor_name.name_first,
                                    last_name = doctor_name.name_last == null ? "" : doctor_name.name_last,
                                    middle_name = doctor_name.name_middle == null ? "" : doctor_name.name_middle,
                                    time_slot = vtime_slot,
                                    image_url = doctor_name.image_url == null ? "" : doctor_name.image_url,
                                    appointment_type = doc_appt,
                                    pecoscert = doctor_name.pecos_cert == null ? false : true,
                                    //timeslot ="",
                                    address = doc_address,
                                    specialties = spec

                                });
                            }

                            var _soul = db.SOULs.Find(i.rel_SOUL_id);


                            new_APPOINTMENT.Add(new new_APPOINTMENT
                            {
                                appointment_id = i.id,
                                patient_id = i.rel_SOUL_id.Value,
                                status = (i.rel_ref_status_id.Value == 10) ? "Active" : "Inactive",
                                first_name = _soul.name_first,
                                last_name = _soul.name_last,
                                date = i.date == null ? "" : i.date.Value.GetDateTimeFormats()[3],
                                //appointment = i.ref_APPOINTMENT_type.dname,
                                user_id = i.create_by__USER_id.Value,
                                time_slot = appt_ext_timeslot,
                                //specialty = (doc_specialty==null)? "": doc_specialty,
                                //doctor_image = (image_url == null ) ? "": image_url
                                doctor = doc,
                                doctor_id = doctor_id,
                                doctor_name = doc_name,
                                rating = appt_ext_rate,
                                date_created = i.dt_create.GetDateTimeFormats()[56],
                                appointment_type = appt
                            });

                        }

                        var ret1 = JsonConvert.SerializeObject(new_APPOINTMENT);
                        var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                        return Json(new { data = json1, message = new_APPOINTMENT.Count() + (new_APPOINTMENT.Count() > 1 ? " Records found!" : " Record found!"), success = true });
                    }

                }
                catch (Exception ex)
                {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
            }
        }

        [HttpGet]
        [System.Web.Http.Route("appointment/patient")]
        public IHttpActionResult Getpatientappointment(string patient_id = null)
        {

            IsRequired("patient_id", patient_id, 2);

            if (haserror)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }
            else
            {
                try
                {
                    long patient_id_new = 0;
                    bool bp = long.TryParse(patient_id, out patient_id_new);

                    // current appointment
                    var querydata1 = (from a in db.APPOINTMENTs
                                      where ((a.date >= date || a.date == null) && a.rel_SOUL_id == patient_id_new && a.rel_ref_status_id != 12)
                                      select a);
                    //{
                    //    present = new new_APPOINTMENT
                    //    {
                    //        appointment_id = a.id,
                    //        //appointment = a.ref_APPOINTMENT_type == null?"" :a.ref_APPOINTMENT_type.dname,
                    //        user_id = a.create_by__USER_id.Value,
                    //        patient_id = a.rel_SOUL_id.Value,
                    //        date = a.date.ToString(),
                    //        status = (a.rel_ref_status_id.Value == 7) ? "Active" : "Inactive",
                    //        //doctor_name = "",
                    //        //doctor_image = "",
                    //        //specialty =""
                    //    }
                    //});


                    // history appointment
                    var querydata2 = (from a in db.APPOINTMENTs
                                      where (a.date < date && a.rel_SOUL_id == patient_id_new && a.rel_ref_status_id != 12)
                                      select new
                                      {
                                          past = new new_APPOINTMENT
                                          {
                                              appointment_id = a.id,
                                              //appointment = a.ref_APPOINTMENT_type == null ? "" : a.ref_APPOINTMENT_type.dname,
                                              user_id = a.create_by__USER_id.Value,
                                              patient_id = a.rel_SOUL_id.Value,
                                              date = a.date.ToString(),
                                              status = (a.rel_ref_status_id.Value == 7) ? "Active" : "Inactive",
                                              //doctor_name = "",
                                              //doctor_image = "",
                                              //specialty = ""
                                          }
                                      });


                    // get Current details
                    List<new_APPOINTMENT> Appt_current = new List<new_APPOINTMENT>();

                    if (querydata1.Count() > 0)
                    {
                        foreach (var i in querydata1)
                        {
                            string doc_name = "", doc_specialty = null, image_url = null, appt_ext_timeslot = "";
                            double appt_ext_rate = 0;
                            long doctor_id = 0;


                            var appt_ext = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == i.id);

                            if (appt_ext.Count() > 0)
                            {
                                foreach (var k in appt_ext) {
                                    switch (k.attr_name)
                                    {
                                        case "doctor_rating":
                                            bool bTemp = double.TryParse(k.value, out appt_ext_rate);
                                            break;

                                        case "time_slot":
                                            appt_ext_timeslot = k.value;
                                            break;

                                        case "doctor_id":
                                            bool bTemp2 = long.TryParse(k.value, out doctor_id);
                                            break;
                                    }
                                }
                            }

                            List<appt_type> appt = new List<appt_type>();
                            List<appt_type> doc_appt = new List<appt_type>();
                            if (i.rel_ref_APPOINTMENT_type_id != null)
                            {
                                appt.Add(new appt_type
                                {
                                    id = i.rel_ref_APPOINTMENT_type_id.Value,
                                    type = i.ref_APPOINTMENT_type.dname
                                });
                            }

                            List<doctor_profile> doc = new List<doctor_profile>();
                            if (doctor_id > 0)
                            {
                                var doctor_name = db.DOCTORs.Find(doctor_id);
                                string namefirst = (doctor_name == null) ? "" : doctor_name.name_first;
                                string namelast = (doctor_name == null) ? "" : doctor_name.name_last;
                                doc_name = namefirst + " " + namelast;

                                List<doc_specialty2> spec = new List<doc_specialty2>();
                                spec = getSpecialty(doctor_id);


                                //string addr2 = doctor_name.addr_address2 == null ? "" : " " + doctor_name.addr_address2;
                                zip_search_address doc_address = new zip_search_address
                                {
                                    address1 = doctor_name.addr_address1 == null ? "" : doctor_name.addr_address1,
                                    address2 = doctor_name.addr_address2 == null ? "" : doctor_name.addr_address2,
                                    zip = doctor_name.ref_zip.zip,
                                    county = doctor_name.ref_zip.city_county,
                                    city = doctor_name.ref_zip.city_name,
                                    state = doctor_name.ref_zip.city_state,
                                    lat = doctor_name.ref_zip.city_lat,
                                    lng = doctor_name.ref_zip.city_lon
                                };

                                string vappt_name = "", vtime_slot = ""; long vappt_id = 0; double doc_fee = 0;

                                var doc_ext = db.DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doctor_name.id);
                                if (doc_ext.Count() > 0)
                                {

                                    foreach (var n in doc_ext)
                                    {
                                        switch (n.attr_name)
                                        {
                                            case "drappttype":
                                                // a.attr_name == "drappttype" &&
                                                bool isAppt = long.TryParse(n.value, out vappt_id);

                                                if (vappt_id > 0)
                                                {
                                                    var n1 = db.ref_APPOINTMENT_type.Find(vappt_id);
                                                    vappt_name = n1.dname;

                                                    doc_appt.Add(new appt_type
                                                    {
                                                        id = vappt_id,
                                                        type = vappt_name
                                                    });
                                                }

                                                break;


                                            case "fee":
                                                bool bTemp = double.TryParse(n.value, out doc_fee);
                                                break;

                                            case "time_slot":
                                                vtime_slot = n.value == null ? "" : n.value;
                                                break;
                                        }



                                    }
                                }

                                // get if Doctor is 
                                        int fave_doc = 0;
                                //if (user_id_new > 0)
                                //{
                                //long nUser_id = 0;
                                //bool n = long.TryParse(doctor_name.create_by__USER_id, out nUser_id);

                                            var con_fav = db.con_USER_favorite_DOCTOR.Where(a => a.create_by__USER_id == doctor_name.create_by__USER_id && a.rel_doctor_id == doctor_name.id);

                                            if (con_fav.Count() > 0)
                                            {
                                                if (con_fav.FirstOrDefault().favor == true)
                                                    fave_doc = 1;
                                                else
                                                    fave_doc = 0;
                                            }
                                       // }


                                doc.Add(new doctor_profile
                                {
                                    id = doctor_name.id,
                                    doctor_fee = doc_fee,
                                    bio = doctor_name.bio == null ? "" : doctor_name.bio,
                                    time_slot = vtime_slot,
                                    //name = doc_name,
                                    favorite =fave_doc,
                                    first_name = doctor_name.name_first == null ? "" : doctor_name.name_first,
                                    last_name = doctor_name.name_last == null ? "" : doctor_name.name_last,
                                    middle_name = doctor_name.name_middle == null ? "" : doctor_name.name_middle,
                                    email = doctor_name.email,
                                    gender = doctor_name.gender,
                                    title = doctor_name.title,
                                    phone = doctor_name.phone,
                                    license = doctor_name.license_no,
                                    pecoscert = doctor_name.pecos_cert == null ? false : true,
                                    npi = doctor_name.NPI,
                                    orgname = doctor_name.organ_name,
                                    image_url = doctor_name.image_url == null ? "" : doctor_name.image_url,
                                    //timeslot = appt_ext_timeslot,
                                    address = doc_address,
                                    specialties = spec

                                });
                            }

                            var _soul = db.SOULs.Find(i.rel_SOUL_id);

                            Appt_current.Add(new new_APPOINTMENT
                            {
                                appointment_id = i.id,
                                patient_id = i.rel_SOUL_id.Value,
                                first_name = _soul.name_first == null ? "" : _soul.name_first,
                                last_name = _soul.name_last == null ? "" : _soul.name_last,

                                status = (i.rel_ref_status_id.Value == 10) ? "Active" : "Inactive",
                                time_slot = appt_ext_timeslot,
                                date = i.date == null ? "" : i.date.Value.GetDateTimeFormats()[3],
                                doctor_id = doctor_id,
                                doctor_name = doc_name,
                                rating = appt_ext_rate,
                                //appointment = (i.ref_APPOINTMENT_type == null)?"": i.ref_APPOINTMENT_type.dname,
                                user_id = i.create_by__USER_id.Value,
                                doctor = doc,
                                date_created = i.dt_create.GetDateTimeFormats()[56],
                                appointment_type = appt
                                //specialty = (doc_specialty == null) ? "" : doc_specialty,
                                //doctor_image = (image_url == null) ? "" : image_url
                            });
                        }

                    } // end of Appt_current


                    // get History details
                    List<new_APPOINTMENT> Appt_history = new List<new_APPOINTMENT>();

                    if (querydata2.Count() > 0)
                    {
                        foreach (var i in querydata1)
                        {
                            string doc_name = "", doc_specialty = null, appt_ext_timeslot = "";
                            double appt_ext_rate = 0;
                            long doctor_id = 0;


                            var appt_ext = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == i.id);

                            if (appt_ext.Count() > 0)
                            {
                                foreach (var k in appt_ext)
                                {
                                    switch (k.attr_name)
                                    {
                                        case "doctor_rating":
                                            bool bTemp = double.TryParse(k.value, out appt_ext_rate);
                                            break;

                                        case "time_slot":
                                            appt_ext_timeslot = k.value;
                                            break;

                                        case "doctor_id":
                                            bool bTemp2 = long.TryParse(k.value, out doctor_id);
                                            break;
                                    }
                                }
                            }

                            List<appt_type> appt = new List<appt_type>();
                            List<appt_type> doc_appt = new List<appt_type>();

                            if (i.rel_ref_APPOINTMENT_type_id != null)
                            {
                                appt.Add(new appt_type
                                {
                                    id = i.rel_ref_APPOINTMENT_type_id.Value,
                                    type = i.ref_APPOINTMENT_type.dname
                                });
                            }

                            List<doctor_profile> doc = new List<doctor_profile>();
                            if (doctor_id > 0)
                            {
                                var doctor_name = db.DOCTORs.Find(doctor_id);
                                string namefirst = (doctor_name == null) ? "" : doctor_name.name_first;
                                string namelast = (doctor_name == null) ? "" : doctor_name.name_last;
                                doc_name = namefirst + " " + namelast;

                                List<doc_specialty2> spec = new List<doc_specialty2>();
                                spec = getSpecialty(doctor_id);


                                //string addr2 = doctor_name.addr_address2 == null ? "" : " " + doctor_name.addr_address2;
                                zip_search_address doc_address = new zip_search_address
                                {
                                    address1 = doctor_name.addr_address1 == null ? "" : doctor_name.addr_address1,
                                    address2 = doctor_name.addr_address2 == null ? "" : doctor_name.addr_address2,
                                    zip = doctor_name.ref_zip.zip,
                                    county = doctor_name.ref_zip.city_county,
                                    city = doctor_name.ref_zip.city_name,
                                    state = doctor_name.ref_zip.city_state,
                                    lat = doctor_name.ref_zip.city_lat,
                                    lng = doctor_name.ref_zip.city_lon
                                };

                                string vappt_name = "", vtime_slot = ""; long vappt_id = 0; double doc_fee = 0;

                                var doc_ext = db.DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doctor_name.id);
                                if (doc_ext.Count() > 0)
                                {

                                    foreach (var n in doc_ext)
                                    {
                                        switch (n.attr_name)
                                        {
                                            case "drappttype":
                                                // a.attr_name == "drappttype" &&
                                                bool isAppt = long.TryParse(n.value, out vappt_id);

                                                if (vappt_id > 0)
                                                {
                                                    var n1 = db.ref_APPOINTMENT_type.Find(vappt_id);
                                                    vappt_name = n1.dname;

                                                    doc_appt.Add(new appt_type
                                                    {
                                                        id = vappt_id,
                                                        type = vappt_name
                                                    });
                                                }

                                                break;


                                            case "fee":
                                                bool bTemp = double.TryParse(n.value, out doc_fee);
                                                break;

                                            case "time_slot":
                                                vtime_slot = n.value == null ? "" : n.value;
                                                break;
                                        }



                                    }
                                }

                                var con_fav = db.con_USER_favorite_DOCTOR.Where(a => a.create_by__USER_id == Convert.ToInt64(doctor_name.create_by__USER_id) && a.rel_doctor_id == doctor_name.id);
                                int fave_doc = 0;
                                if (con_fav.Count() > 0)
                                {
                                    if (con_fav.FirstOrDefault().favor == true)
                                        fave_doc = 1;
                                    else
                                        fave_doc = 0;
                                }


                                doc.Add(new doctor_profile
                                {
                                    id = doctor_name.id,
                                    doctor_fee = doc_fee,
                                    bio = doctor_name.bio == null ? "" : doctor_name.bio,
                                    //name = doc_name,
                                    favorite = fave_doc,
                                    first_name = doctor_name.name_first == null ? "" : doctor_name.name_first,
                                    last_name = doctor_name.name_last == null ? "" : doctor_name.name_last,
                                    middle_name = doctor_name.name_middle == null ? "" : doctor_name.name_middle,
                                    email = doctor_name.email,
                                    gender = doctor_name.gender,
                                    title = doctor_name.title,
                                    phone = doctor_name.phone,
                                    license = doctor_name.license_no,
                                    pecoscert = doctor_name.pecos_cert == null ? false : true,
                                    npi = doctor_name.NPI,
                                    orgname = doctor_name.organ_name,

                                    time_slot = vtime_slot,
                                    image_url = doctor_name.image_url == null ? "" : doctor_name.image_url,
                                    //timeslot = appt_ext_timeslot,
                                    address = doc_address,
                                    specialties = spec

                                });
                            }

                            var _soul = db.SOULs.Find(i.rel_SOUL_id);

                            Appt_history.Add(new new_APPOINTMENT
                            {
                                appointment_id = i.id,
                                patient_id = i.rel_SOUL_id.Value,
                                first_name = _soul.name_first == null ? "" : _soul.name_first,
                                last_name = _soul.name_last == null ? "" : _soul.name_last,
                                status = (i.rel_ref_status_id.Value == 10) ? "Active" : "Inactive",
                                date = i.date == null ? "" : i.date.Value.GetDateTimeFormats()[3],
                                //appointment = (i.ref_APPOINTMENT_type == null)?"": i.ref_APPOINTMENT_type.dname,
                                time_slot = appt_ext_timeslot,
                                user_id = i.create_by__USER_id.Value,
                                doctor_id = doctor_id,
                                doctor_name = doc_name,
                                rating = appt_ext_rate,
                                doctor = doc,
                                date_created = i.dt_create.GetDateTimeFormats()[56],
                                appointment_type = appt
                                //specialty = (doc_specialty == null) ? "" : doc_specialty,
                                //doctor_image = (image_url == null) ? "" : image_url
                            });
                        }

                    } // end of Appt_history

                    List<all_Appointment> all_appt = new List<all_Appointment>();
                    all_appt.Add(new all_Appointment
                    {
                        current = Appt_current,
                        history = Appt_history
                    });


                    //if (querydata1.Count() > 0)
                    //{
                    //    Arraydata.Add(querydata1);
                    //}

                    //if (querydata2.Count() > 0)
                    //{
                    //    Arraydata.Add(querydata2);
                    //}

                    if (all_appt.Count() == 0)
                    {
                        string msg = "No result found.";
                        return Json(new { data = new string[] { }, message = msg, success = false });
                    }
                    else
                    {
                        string msg = Appt_current.Count() + " Pending record(s), " + Appt_history.Count() + " History record(s)";
                        var ret1 = JsonConvert.SerializeObject(all_appt);
                        var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                        return Json(new { data = json1, message = msg, success = true });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
            }
        }


        // get specialty, appointment_type, user's patient
        [HttpGet]
        [Route("specialty_patient_appointment_type")]
        public IHttpActionResult getappointmentspecialty(string user_id = "") {
            IsRequired("user_id", user_id, 1);
            if (haserror)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }
            else
            {

                long nUser_id = 0;
                bool bTemp = long.TryParse(user_id, out nUser_id);

                // start of patients info
                List<user_secondary_patient> pat = new List<user_secondary_patient>();

                if (nUser_id > 0)
                {
                    // get prime user
                    var prime = db.USERs.Find(nUser_id);

                    if (prime != null)
                    {

                        // get the patient's info that is added by the primary user
                        //|| b.email == prime.username
                        // 3 Deleted
                        if (Request.RequestUri.ToString().Contains("localhost"))
                        {
                            var ref_stat = db.ref_status.Where(a => a.dname.ToLower() == "deleted" && a.ref_status_type.dname.ToLower() == "soul").FirstOrDefault();
                        }
                        
                        
                        var p_user = db.SOULs.Where(b => (b.create_by__USER_id == nUser_id ) && b.rel_ref_status_id != 3).OrderBy(c => c.name_first).ThenBy(d => d.name_last);

                        foreach (var n in p_user)
                        {
                            // is_using_primary_patient_insurance
                            var u_user = db.SOUL_ext.Where(a => a.rel_SOUL_id == n.id && a.attr_name == "use_user_insurance");

                            // get insurance name 
                            var p_ext = db.SOUL_ext.Where(a => a.rel_SOUL_id == n.id && a.attr_name == "insurance_id");
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
                                        var ins_ref = db.ref_insurance_provider.Find(ins_id);
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

                            //List<user_secondary_patient> second_pat = new List<user_secondary_patient>();

                            pat.Add(new user_secondary_patient
                               {
                                   id = n.id,
                                   first_name = n.name_first == null ? "" : n.name_first,
                                   last_name = n.name_last == null ? "" : n.name_last,
                                   is_prime = n.is_prime.Value,
                                   //insurance_id =  p_ext.Count() == 0 ? "" : p_ext.ToList().First().value.ToString(),
                                   is_using_primary_patient_insurance = user_insurance,
                                   insurance_id= ins_id_string,
                                   insurance_name= ins_name
                               });
                        }

                    } // check p_user.Count()

                }
                // end of patients info

                string msg = "";

                var ref_specialty = from a in db.ref_specialty
                                    where a.name != null
                                    orderby a.name select a;
                List<doc_specialty2> spec = new List<doc_specialty2>();
                foreach (var i in ref_specialty)
                {
                    //if (i.name != null)
                    //{
                    var cat = db.ref_specialty_category.Find(i.rel_ref_specialty_category_id);
                    spec.Add(new doc_specialty2
                    {
                        id = i.id,
                        code = i.code == null ? "" : i.code,
                        name = i.name == null ? "" : i.name,
                        provider_type = i.provider_type == null ? "" : i.provider_type,
                        specialization = i.specialization == null ? "" : i.specialization 
                     });
                    //}

                }


                //var ret = JsonConvert.SerializeObject(spec);
                //var json = Newtonsoft.Json.Linq.JArray.Parse(ret);

                var ref_appointment = from a in db.ref_APPOINTMENT_type orderby a.dname select a;
                List<ref_appt_type> appt_type = new List<ref_appt_type>();
                foreach (var n in ref_appointment)
                {
                    if (!string.IsNullOrEmpty(n.dname))
                    {
                        appt_type.Add(new ref_appt_type
                        {
                            id = n.id,
                            appointment_type = n.dname
                        });
                    }
                }

                //var ret1 = JsonConvert.SerializeObject(appt_type );
                //var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                List<ref_spec_appt_type> spec_appt = new List<ref_spec_appt_type>();
                spec_appt.Add(new ref_spec_appt_type
                {
                    specialty = spec,
                    appointment_type = appt_type,
                    patient = pat
                });

                var ret1 = JsonConvert.SerializeObject(spec_appt);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                return Json(new { data = json1, message = pat.Count() + " Patient record(s) found.", success = true });
            }


        }

        // TODO
        [HttpGet]
        [Route("appointment/user")]
        public IHttpActionResult GetPatientAppointmentPerUser(string user_id = "")
        {

            IsRequired("user_id", user_id, 2);

            if (haserror)
            {
                return Json(new { message = errmsg, success = false });
            }
            else
            {
                try
                {
                    long nUserId = 0;
                    bool bTemp = long.TryParse(user_id, out nUserId);
                    //For the 3rd API we need to get the list of 
                    //    available specialities, 
                    //    available appointment types and 
                    //    all the patients of the current user in 1 service.

                    if (bTemp)
                    {
                        long curretnt_user = 0;
                        var c_user = db.USERs.Find(nUserId);

                        if (c_user != null)
                        {
                            //var c_appt = db.APPOINTMENTs.Where(a => a.SOUL.create_by__USER_id == c_user.id || a.SOUL.email == c_user.username)
                            //    .OrderBy(b => b.SOUL.name_first).ThenBy(b => b.SOUL.name_last).ThenBy(b => b.id);

                            var c_appt = from a in db.APPOINTMENTs
                                         join b in db.SOULs on a.rel_SOUL_id equals b.id
                                         where b.create_by__USER_id == c_user.id || b.email == c_user.username
                                         orderby b.name_first ascending, b.name_last ascending
                                         select a ;

                            if (c_appt.Count() > 0)
                            {
                                string doc_name = "";
                                List<new_APPOINTMENT> user_Appt = new List<new_APPOINTMENT>();
                                foreach (var i in c_appt)
                                {
                                    #region"appt_ext_timeslot"
                                    string appt_ext_timeslot = "";
                                    double appt_ext_rate = 0;
                                    long doctor_id = 0;

                                    var appt_ext = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == i.id);

                                    if (appt_ext.Count() > 0)
                                    {
                                        foreach (var k in appt_ext)
                                        {
                                            switch (k.attr_name)
                                            {
                                                case "doctor_id":
                                                    bool bTemp2 = long.TryParse(k.value, out doctor_id);
                                                    break;
                                                case "time_slot":
                                                    appt_ext_timeslot = k.value;
                                                    break;
                                                case "doctor_rating":
                                                    bool bTemp1 = double.TryParse(k.value, out appt_ext_rate);
                                                    break;

                                            }
                                        }
                                    }
                                    #endregion

                                    #region"doc"
                                    List<appt_type> appt = new List<appt_type>();
                                    List<appt_type> doc_appt = new List<appt_type>();
                                    List<doctor_profile> doc = new List<doctor_profile>();
                                    if (i.rel_ref_APPOINTMENT_type_id != null)
                                    {
                                        appt.Add(new appt_type
                                        {
                                            id = i.rel_ref_APPOINTMENT_type_id.Value,
                                            type = i.ref_APPOINTMENT_type.dname
                                        });
                                    }

                                    if (doctor_id > 0)
                                    {
                                        //doc_id = Convert.ToInt64(doctor_id.value);
                                        var doctor_name = db.DOCTORs.Find(doctor_id);
                                        doc_name = doctor_name.name_first + " " + doctor_name.name_last;

                                        List<doc_specialty2> spec = new List<Models.doc_specialty2>();
                                        spec = getSpecialty(doctor_id);

                                        //string addr2 = doctor_name.addr_address2 == null ? "" : " " + doctor_name.addr_address2;
                                        zip_search_address doc_address = new zip_search_address
                                        {
                                            address1 = doctor_name.addr_address1 == null ? "" : doctor_name.addr_address1,
                                            address2 = doctor_name.addr_address2 == null ? "" : doctor_name.addr_address2,
                                            zip = doctor_name.ref_zip.zip,
                                            city = doctor_name.ref_zip.city_name,
                                            county = doctor_name.ref_zip.city_county,
                                            state = doctor_name.ref_zip.city_state,
                                            lat = doctor_name.ref_zip.city_lat,
                                            lng = doctor_name.ref_zip.city_lon
                                        };

                                        #region "appt"
                                        string vappt_name = "", vtime_slot = "";
                                        long vappt_id = 0; double doc_fee = 0;

                                        var doc_ext = db.DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doctor_name.id);
                                        if (doc_ext.Count() > 0)
                                        {
                                            foreach (var n in doc_ext)
                                            {
                                                switch (n.attr_name)
                                                {
                                                    case "drappttype":
                                                        bool isAppt = long.TryParse(n.value, out vappt_id);
                                                        if (vappt_id > 0)
                                                        {
                                                            var n1 = db.ref_APPOINTMENT_type.Find(vappt_id);
                                                            vappt_name = n1.dname;

                                                            doc_appt.Add(new appt_type
                                                            {
                                                                id = vappt_id,
                                                                type = vappt_name
                                                            });
                                                        }
                                                        break;

                                                    case "fee":
                                                        bool bTemp2 = double.TryParse(n.value, out doc_fee);
                                                        break;
                                                    case "time_slot":
                                                        vtime_slot = n.value == null ? "" : n.value;
                                                        break;
                                                }
                                            }
                                        }

                                         // get if Doctor is 
                                        int fave_doc = 0;
                                        if (nUserId > 0)
                                        {
                                            var con_fav = db.con_USER_favorite_DOCTOR.Where(a => a.create_by__USER_id == nUserId && a.rel_doctor_id == doctor_name.id);

                                            if (con_fav.Count() > 0)
                                            {
                                                if (con_fav.FirstOrDefault().favor == true)
                                                    fave_doc = 1;
                                                else
                                                    fave_doc = 0;
                                            }
                                        }

                                        doc.Add(new doctor_profile
                                        {
                                            id = doctor_name.id,
                                            doctor_fee = doc_fee,
                                            bio = doctor_name.bio == null ? "" : doctor_name.bio,
                                            //name = doc_name,
                                            favorite = fave_doc,
                                            first_name = doctor_name.name_first == null ? "" : doctor_name.name_first,
                                            last_name = doctor_name.name_last == null ? "" : doctor_name.name_last,
                                            middle_name = doctor_name.name_middle == null ? "" : doctor_name.name_middle,
                                            email = doctor_name.email,
                                            gender = doctor_name.gender,
                                            title = doctor_name.title,
                                            phone = doctor_name.phone,
                                            license = doctor_name.license_no,
                                            pecoscert = doctor_name.pecos_cert == null ? false : true,
                                            npi = doctor_name.NPI,
                                            orgname = doctor_name.organ_name,

                                            time_slot = vtime_slot,
                                            image_url = doctor_name.image_url == null ? "" : doctor_name.image_url,
                                            //timeslot ="",
                                            address = doc_address,
                                            specialties = spec

                                        });
                                        #endregion

                                    }
                                    #endregion

                                    #region "user_Appt"
                                    var _soul = db.SOULs.Find(i.rel_SOUL_id);

                                    user_Appt.Add(new new_APPOINTMENT
                                    {
                                        appointment_id = i.id,
                                        patient_id = i.rel_SOUL_id.Value,
                                        first_name = _soul.name_first == null ? "" : _soul.name_first,
                                        last_name = _soul.name_last == null ? "" : _soul.name_last,

                                        time_slot = appt_ext_timeslot,
                                        status = (i.rel_ref_status_id.Value == 10) ? "Active" : "Inactive",
                                        date = i.date == null ? "" : i.date.Value.GetDateTimeFormats()[3],
                                        user_id = i.create_by__USER_id.Value,
                                        doctor = doc,
                                        doctor_id = doctor_id,
                                        doctor_name = doc_name,
                                        rating = appt_ext_rate,
                                        date_created = i.dt_create.GetDateTimeFormats()[56],
                                        appointment_type = appt
                                    });
                                    #endregion

                                }


                                var ret1 = JsonConvert.SerializeObject(user_Appt);
                                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                                return Json(new { data = json1, message = user_Appt.Count() + " Record(s) Found!", success = true });
                            }

                            return Json(new { data = new string[] { }, message = "No appointment available.", success = false });
                        }
                        else // user_id does not exist
                        {
                            return Json(new { data = new string[] { }, message = "User does not exist.", success = false });
                        }
                    } // invalid user_id
                    return Json(new { data = new string[] { }, message = "Invalid user_id.", success = false });
                }
                catch (Exception ex) {
                    return Json(new { data = new string[] { }, message = ex.Message, success = false });
                }
            }
        }


        public List<doctor_profile> getdoctor(long doctor_id) {

            List<doctor_profile> doc = new List<doctor_profile>();
            var doctor_name = db.DOCTORs.Find(doctor_id);
            string image_url = null; string doc_name = null;


            string namefirst = (doctor_name == null) ? "" : doctor_name.name_first;
            string namelast = (doctor_name == null) ? "" : doctor_name.name_last;
            doc_name = namefirst + " " + namelast;
            image_url = (doctor_name == null) ? "" : doctor_name.image_url;


            // get DOCTOR
            if (doctor_name != null)
            {
                string vappt_name = "", vtime_slot = ""; long vappt_id = 0; double doc_fee = 0;
                List<appt_type> doc_appt = new List<appt_type>();

                var doc_ext = db.DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doctor_name.id);

                    foreach (var n in doc_ext)
                    {
                        switch (n.attr_name)
                        {
                            case "drappttype":
                                // a.attr_name == "drappttype" &&
                                bool isAppt = long.TryParse(n.value, out vappt_id);
                                if (vappt_id > 0)
                                {
                                    var n1 = db.ref_APPOINTMENT_type.Find(vappt_id);
                                    vappt_name = n1.dname;
                                    doc_appt.Add(new appt_type
                                    {
                                        id = vappt_id,
                                        type = vappt_name
                                    });
                                }
                                break;

                            case "fee":
                                bool bTemp = double.TryParse(n.value, out doc_fee);
                                break;
                            case "time_slot":
                                vtime_slot = n.value == null ? "" : n.value;
                                break;
                        }
                    }

                List<doc_specialty2> spec = new List<Models.doc_specialty2>();
                spec = getSpecialty(doctor_id);


                doc.Add(new doctor_profile
                {

                    id = doctor_name.id,
                    first_name = doctor_name.name_first == null ? "" : doctor_name.name_first,
                    last_name = doctor_name.name_last == null ? "" : doctor_name.name_last,
                    middle_name = doctor_name.name_middle == null ? "" : doctor_name.name_middle,
                    email = doctor_name.email,
                    gender = doctor_name.gender,
                    title = doctor_name.title,
                    phone = doctor_name.phone,
                    license = doctor_name.license_no == null ? "" : doctor_name.license_no,
                    npi = doctor_name.NPI,
                    orgname = doctor_name.organ_name == null ? "" : doctor_name.organ_name,
                    pecoscert = doctor_name.pecos_cert == null ? false : true,
                  
                    //date = i.date == null ? "" : i.date.Value.ToUniversalTime().GetDateTimeFormats()[3],
                    time_slot = vtime_slot,

                    image_url = (image_url == null) ? "" : image_url,

                    //public string timeslot { get; set; }
                    //timeslot =appt_ext_timeslot ,
                    bio = doctor_name.bio == null ? "" : doctor_name.bio,
                    specialties = spec,
                    //appointment_type = doc_appt,
                    //address = doc_address
                });
            }
            return doc;
        }


        [HttpGet]
        [Route("doctor/dashboard")]
        public IHttpActionResult getdoctordashboard(string doctor_id = null)
        {
            try
            {

                // GET PENDING 
                #region "get Pending"
                // 1 Pending 
                // get Active only
                var querydata1 = (from a in db.APPOINTMENTs where a.rel_ref_APPOINTMENT_status_id == 1 && a.rel_ref_status_id == 10
                                  orderby a.id
                                  select a);

                //var query_ext = (from a in db.APPOINTMENT_ext where a.attr_name == "doctor_id" select a);
                List<long> pending_appt_id = new List<long>();
                List<dashboard_Appointment> new_APPOINTMENT = new List<dashboard_Appointment>();
                if (!string.IsNullOrEmpty(doctor_id))
                {
                    foreach (var qd in querydata1) {

                        var qPending_ext = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == qd.id 
                                                                      && a.attr_name =="doctor_id" 
                                                                      && a.value == doctor_id);
                        if (qPending_ext.Count() > 0)
                        {
                            foreach (var qpend_ext in qPending_ext)
                            {
                              

                                //List<doctor_profile> doc = new List<doctor_profile>();
                                long _doc = Convert.ToInt64(qpend_ext.value);
                                //List<doctor_profile> doc = getdoctor(_doc);

                                #region "appt_ext.Count() > 0"
                                double appt_ext_rate = 0;
                                string appt_ext_timeslot = "";
                                var qPending_ext2 = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == qd.id);

                                if (qPending_ext2.Count() > 0)
                                {
                                    foreach (var k in qPending_ext2)
                                    {
                                        switch (k.attr_name)
                                        {
                                            case "doctor_rating":
                                                bool bTemp = double.TryParse(k.value, out appt_ext_rate);

                                                break;
                                            case "time_slot":
                                                appt_ext_timeslot = k.value;
                                                break;

                                            //case "doctor_id":
                                            //    bool bTemp2 = long.TryParse(k.value, out doctor_id_var);
                                            //    break;

                                            case "proposed_doctor":
                                                //hasProposed = true;
                                                //// id|doctor_id|date|time_slot
                                                //string[] doc1 = k.value.Split('|');
                                                //long prop_appt = Convert.ToInt64(doc1[0]);
                                                //long prop_doctor = Convert.ToInt64(doc1[0]);
                                                //string prop_date = doc1[1];
                                                //string prop_time = doc1[2];

                                                //prop_doc.Add(new doctor_profile
                                                //{
                                                //    //id = k.id,
                                                //    id = prop_doctor,
                                                //    date = prop_date,
                                                //    time_slot = prop_time
                                                //});
                                                break;
                                        }
                                    }
                                }
                                #endregion

                                #region " GET APPOINTMENT_TYPE"
                                List<appt_type> appt = new List<appt_type>();
                                if (qd.rel_ref_APPOINTMENT_type_id != null)
                                {
                                    appt.Add(new appt_type
                                    {
                                        id = qd.rel_ref_APPOINTMENT_type_id.Value,
                                        type = qd.ref_APPOINTMENT_type.dname
                                    });
                                }
                                #endregion


                                var _soul = db.SOULs.Find(qd.rel_SOUL_id);
                                string name_first = "", name_last = "";
                                if (_soul != null)
                                {
                                    name_first = _soul.name_first == null ? "" : _soul.name_first;
                                    name_last = _soul.name_last == null ? "" : _soul.name_last;
                                }

                                new_APPOINTMENT.Add(new dashboard_Appointment
                                {
                                    appointment_id = qd.id,
                                    patient_id = qd.rel_SOUL_id.Value,
                                    first_name = name_first,
                                    last_name = name_last,

                                    status = (qd.rel_ref_status_id.Value == 10) ? "Active" : "Inactive",
                                    time_slot = appt_ext_timeslot,
                                    date = qd.date == null ? "" : qd.date.Value.ToUniversalTime().GetDateTimeFormats()[3],
                                    //appointment = (i.ref_APPOINTMENT_type == null)?"": i.ref_APPOINTMENT_type.dname,
                                    // ** user_id = i.create_by__USER_id.Value,
                                    //doctor_id = doctor_id,
                                    //doctor_name = doctor_id > 0 ? doc_name : "",
                                    //doctor_name = "",
                                    // ** rating = appt_ext_rate,
                                    //doctor =   doc ,
                                    //proposed_doctor = prop_doc,

                                    date_created = qd.dt_create.GetDateTimeFormats()[56]
                                    //appointment_type = appt,
                                    //specialty = (doc_specialty == null) ? "" : doc_specialty,
                                    //doctor_image = (image_url == null) ? "" : image_url
                                });
                            }

                            
                        }
                    }
                }

                #region "COMMENTED OUT"
                // xxxxxxxxxxxxxxxxxx
                //if (querydata1.Count() > 0)
                //{

                //    foreach (var i in querydata1)
                //    {
                //        string doc_name = "", doc_specialty = null, image_url = null;
                //        List<appt_type> appt = new List<appt_type>();
                //        List<appt_type> doc_appt = new List<appt_type>();

                //        if (i.rel_ref_APPOINTMENT_type_id != null)
                //        {
                //            appt.Add(new appt_type
                //            {
                //                id = i.rel_ref_APPOINTMENT_type_id.Value,
                //                type = i.ref_APPOINTMENT_type.dname
                //            });
                //        }

                //        // retrieve info from Appointment_ext
                //        var appt_ext = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == i.id);
                //        double appt_ext_rate = 0; string appt_ext_timeslot = "";


                //        bool hasProposed = false;
                //        List<doctor_profile> prop_doc = new List<doctor_profile>();

                //        // && a.attr_name == "doctor_rating"
                //        #region "appt_ext.Count() > 0"
                //        if (appt_ext.Count() > 0)
                //        {
                //            foreach (var k in appt_ext)
                //            {
                //                switch (k.attr_name)
                //                {
                //                    case "doctor_rating":
                //                        bool bTemp = double.TryParse(k.value, out appt_ext_rate);

                //                        break;
                //                    case "time_slot":
                //                        appt_ext_timeslot = k.value;
                //                        break;

                //                    //case "doctor_id":
                //                    //    bool bTemp2 = long.TryParse(k.value, out doctor_id_var);
                //                    //    break;

                //                    case "proposed_doctor":
                //                        hasProposed = true;
                //                        // id|doctor_id|date|time_slot
                //                        string[] doc1 = k.value.Split('|');
                //                        long prop_appt = Convert.ToInt64(doc1[0]);
                //                        long prop_doctor = Convert.ToInt64(doc1[0]);
                //                        string prop_date = doc1[1];
                //                        string prop_time = doc1[2];

                //                        prop_doc.Add(new doctor_profile
                //                        {
                //                            //id = k.id,
                //                            id = prop_doctor,
                //                            date = prop_date,
                //                            time_slot = prop_time
                //                        });
                //                        break;
                //                }
                //            }
                //        }
                //        #endregion


                //        //var doctor_id = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == i.id && a.attr_name == "doctor_id").FirstOrDefault();
                //        List<doctor_profile> doc = new List<doctor_profile>();

                //        //if (doctor_id_var > 0)

                //        if (hasProposed) // no doctor_id, but has proposed_doctor
                //        {
                //            #region "hasProposed Doctors"
                //            foreach (var d in prop_doc)
                //            {
                //                var doctor_name = db.DOCTORs.Find(d.id);
                //                //var con_ref_spec = db.con_DOCTOR_ref_specialty.Where(a => a.rel_DOCTOR_id == doctor_id);

                //                string namefirst = (doctor_name == null) ? "" : doctor_name.name_first;
                //                string namelast = (doctor_name == null) ? "" : doctor_name.name_last;
                //                doc_name = namefirst + " " + namelast;
                //                image_url = doctor_name.image_url;

                //                List<doc_specialty> spec = new List<Models.doc_specialty>();
                //                spec = getSpecialty(d.id);

                //                string addr2 = doctor_name.addr_address2 == null ? "" : " " + doctor_name.addr_address2;
                //                zip_search_address doc_address = new zip_search_address
                //                {
                //                    address = doctor_name.addr_address1 == null ? "" : doctor_name.addr_address1 + addr2,
                //                    //address2 = doctor_name.addr_address2 == null ? "" : doctor_name.addr_address2,
                //                    zip = doctor_name.ref_zip.zip,
                //                    county = doctor_name.ref_zip.city_county,
                //                    city = doctor_name.ref_zip.city_name,
                //                    state = doctor_name.ref_zip.city_state,
                //                    lat = doctor_name.ref_zip.city_lat,
                //                    lng = doctor_name.ref_zip.city_lon
                //                };


                //                string vappt_name = "", vtime_slot = ""; long vappt_id = 0; double doc_fee = 0;

                //                var doc_ext = db.DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doctor_name.id);

                //                if (doc_ext.Count() > 0)
                //                {

                //                    foreach (var n in doc_ext)
                //                    {
                //                        switch (n.attr_name)
                //                        {
                //                            case "drappttype":
                //                                // a.attr_name == "drappttype" &&
                //                                bool isAppt = long.TryParse(n.value, out vappt_id);
                //                                if (vappt_id > 0)
                //                                {
                //                                    var n1 = db.ref_APPOINTMENT_type.Find(vappt_id);
                //                                    vappt_name = n1.dname;
                //                                    doc_appt.Add(new appt_type
                //                                    {
                //                                        id = vappt_id,
                //                                        type = vappt_name
                //                                    });
                //                                }
                //                                break;

                //                            case "fee":
                //                                bool bTemp = double.TryParse(n.value, out doc_fee);
                //                                break;
                //                            case "time_slot":
                //                                vtime_slot = n.value == null ? "" : n.value;
                //                                break;
                //                        }



                //                    }
                //                }

                //                // get if Doctor is 
                //                //int fave_doc = 0;
                //                //if (user_id_new > 0)
                //                //{
                //                //    var con_fav = db.con_USER_favorite_DOCTOR.Where(a => a.create_by__USER_id == user_id_new && a.rel_doctor_id == doctor_name.id);

                //                //    if (con_fav.Count() > 0)
                //                //    {
                //                //        if (con_fav.FirstOrDefault().favor == true)
                //                //            fave_doc = 1;
                //                //        else
                //                //            fave_doc = 0;
                //                //    }
                //                //}

                //                // update list proposed_doctors
                //                d.doctor_fee = doc_fee;
                //                d.rating = appt_ext_rate;
                //                //d.favorite = fave_doc;
                //                d.firstname = doctor_name.name_first == null ? "" : doctor_name.name_first;
                //                d.lastname = doctor_name.name_last == null ? "" : doctor_name.name_last;
                //                d.middlename = doctor_name.name_middle == null ? "" : doctor_name.name_middle;
                //                d.email = doctor_name.email;
                //                d.gender = doctor_name.gender;
                //                d.title = doctor_name.title;
                //                d.phone = doctor_name.phone;
                //                d.license = doctor_name.license_no == null ? "" : doctor_name.license_no;
                //                d.npi = doctor_name.NPI;
                //                d.orgname = doctor_name.org_name == null ? "" : doctor_name.org_name;
                //                d.pecoscert = doctor_name.pecos_cert == null ? false : true;
                //                d.time_slot = vtime_slot;
                //                d.image_url = (image_url == null) ? "" : image_url;

                //                d.bio = doctor_name.bio == null ? "" : doctor_name.bio;
                //                d.specialties = spec;
                //                d.appointment_type = doc_appt;
                //                d.address = doc_address;
                //            }
                //            #endregion
                //        }

                //        #region "rel_SOUL_ID"
                //        var _soul = db.SOULs.Find(i.rel_SOUL_id);
                //        string name_first = "", name_last = "";
                //        if (_soul != null)
                //        {
                //            name_first = _soul.name_first == null ? "" : _soul.name_first;
                //            name_last = _soul.name_last == null ? "" : _soul.name_last;
                //        }
                //        #endregion


                //        #region "new_APPOINTMENT"
                //        new_APPOINTMENT.Add(new new_APPOINTMENT
                //        {
                //            appointment_id = i.id,
                //            patient_id = i.rel_SOUL_id.Value,
                //            firstname = name_first,
                //            lastname = name_last,

                //            status = (i.rel_ref_status_id.Value == 10) ? "Active" : "Inactive",
                //            time_slot = appt_ext_timeslot,
                //            date = i.date == null ? "" : i.date.Value.ToUniversalTime().GetDateTimeFormats()[3],
                //            //appointment = (i.ref_APPOINTMENT_type == null)?"": i.ref_APPOINTMENT_type.dname,
                //            user_id = i.create_by__USER_id.Value,
                //            //doctor_id = doctor_id,
                //            //doctor_name = doctor_id > 0 ? doc_name : "",
                //            doctor_name = "",
                //            rating = appt_ext_rate,
                //            doctor = doc.Count() > 0 ? doc : prop_doc,
                //            //proposed_doctor = prop_doc,
                //            date_created = i.dt_create.GetDateTimeFormats()[56],
                //            appointment_type = appt
                //            //specialty = (doc_specialty == null) ? "" : doc_specialty,
                //            //doctor_image = (image_url == null) ? "" : image_url
                //        });
                //        #endregion


                //    },


                //}
                #endregion

                #endregion

                // GET PENDING unassigned (marketplace)
                #region "GET PENDING unassigned"
                // no doctor_id is assigned //&& a.value != doctor_id
                var queryUnassiged = db.APPOINTMENT_ext.Where(a => a.attr_name == "doctor_id").OrderBy(b => b.rel_APPOINTMENT_id);
                // get all APPOINTMENT with assigned DOCTOR
                List<long> appt_id = new List<long>();
                foreach (var u in queryUnassiged)
                {
                    appt_id.Add(u.rel_APPOINTMENT_id.Value);
                }

                var qPending = db.APPOINTMENTs.Where(a => !appt_id.Contains(a.id) && a.rel_ref_status_id == 10);
                List<dashboard_Appointment> appt_Unassigned = new List<dashboard_Appointment>();
                foreach (var p in qPending)
                {
                    long id = p.id;

                    var _soul = db.SOULs.Find(p.rel_SOUL_id);
                    string name_first = "", name_last = "";
                    if (_soul != null)
                    {
                        name_first = _soul.name_first == null ? "" : _soul.name_first;
                        name_last = _soul.name_last == null ? "" : _soul.name_last;
                    }

                    #region "appt_ext.Count() > 0"
                    double appt_ext_rate = 0;
                    string appt_ext_timeslot = "";
                    var qPending_ext2 = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == p.id);

                    if (qPending_ext2.Count() > 0)
                    {
                        foreach (var k in qPending_ext2)
                        {
                            switch (k.attr_name)
                            {
                                case "doctor_rating":
                                    bool bTemp = double.TryParse(k.value, out appt_ext_rate);

                                    break;
                                case "time_slot":
                                    appt_ext_timeslot = k.value;
                                    break;

                                //case "doctor_id":
                                //    bool bTemp2 = long.TryParse(k.value, out doctor_id_var);
                                //    break;

                                case "proposed_doctor":
                                    //hasProposed = true;
                                    //// id|doctor_id|date|time_slot
                                    //string[] doc1 = k.value.Split('|');
                                    //long prop_appt = Convert.ToInt64(doc1[0]);
                                    //long prop_doctor = Convert.ToInt64(doc1[0]);
                                    //string prop_date = doc1[1];
                                    //string prop_time = doc1[2];

                                    //prop_doc.Add(new doctor_profile
                                    //{
                                    //    //id = k.id,
                                    //    id = prop_doctor,
                                    //    date = prop_date,
                                    //    time_slot = prop_time
                                    //});
                                    break;
                            }
                        }
                    }
                    #endregion

                    List<doctor_profile> doc = new List<doctor_profile>();
                    appt_Unassigned.Add(new dashboard_Appointment
                    {
                        appointment_id = p.id,
                        patient_id = p.rel_SOUL_id.Value,
                        first_name = name_first,
                        last_name = name_last,

                        status = (p.rel_ref_status_id.Value == 10) ? "Active" : "Inactive",
                        time_slot = appt_ext_timeslot,
                        date = p.date == null ? "" : p.date.Value.ToUniversalTime().GetDateTimeFormats()[3],
                        //appointment = (i.ref_APPOINTMENT_type == null)?"": i.ref_APPOINTMENT_type.dname,
                        user_id = p.create_by__USER_id.Value,
                       
                        //doctor_id = doctor_id,
                        //doctor_name = doctor_id > 0 ? doc_name : "",
                       // doctor_name = "",
                        //**rating = appt_ext_rate,
                        //**doctor = doc.Count() > 0 ? doc : prop_doc,
                        //doctor = doc,
                        //proposed_doctor = prop_doc,
                        date_created = p.dt_create.GetDateTimeFormats()[56],
                        //**appointment_type = appt_ty
                        //specialty = (doc_specialty == null) ? "" : doc_specialty,
                        //doctor_image = (image_url == null) ? "" : image_url
                    });
                }
                #endregion


                long doctor_id_var = 0;
                // GET CURRENT APPOINTMENT 
                #region "get current"
                bool isDoctor = long.TryParse(doctor_id, out doctor_id_var);
                List<dashboard_Appointment> appt_cur = new List<dashboard_Appointment>();
                if (isDoctor)
                {

                    var appt_ext = db.APPOINTMENT_ext.Where(a =>  (a.attr_name == "doctor_id" && a.value == doctor_id_var.ToString())
                                || (a.attr_name == "proposed_doctor" && a.value.Contains(doctor_id_var.ToString() + "|")));

                    List<long> curr_Appt = new List<long>();
                    foreach (var id in appt_ext) {
                        curr_Appt.Add(id.rel_APPOINTMENT_id.Value);
                    }

                    var qCurrent_appt = from a in db.APPOINTMENTs
                                        where a.rel_ref_APPOINTMENT_status_id == 2 && a.rel_ref_status_id == 10
                                              && curr_Appt.Contains(a.id)
                                        orderby a.id
                                        select a;
                    //***********
                    foreach (var qcur in qCurrent_appt)
                    {

                      
                        //foreach (var d in appt_ext)
                        //{
                            //var appt = db.APPOINTMENTs.Find(d.rel_APPOINTMENT_id);
                            var _soul = db.SOULs.Find(qcur.rel_SOUL_id);
                            List<appt_type> appt_ty = new List<appt_type>();

                            if (qcur.rel_ref_APPOINTMENT_type_id != null)
                            {
                                appt_ty.Add(new appt_type
                                {
                                    id = qcur.rel_ref_APPOINTMENT_type_id.Value,
                                    type = qcur.ref_APPOINTMENT_type.dname
                                });
                            }

                            string name_first = "", name_last = "";
                            if (_soul != null)
                            {
                                name_first = _soul.name_first == null ? "" : _soul.name_first;
                                name_last = _soul.name_last == null ? "" : _soul.name_last;
                            }

                           // List<doctor_profile> doc = getdoctor(doctor_id_var);

                        #region "appt_ext.Count() > 0"
                        double appt_ext_rate = 0;
                        string appt_ext_timeslot = "";
                        var qPending_ext2 = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == qcur.id);

                        if (qPending_ext2.Count() > 0)
                        {
                            foreach (var k in qPending_ext2)
                            {
                                switch (k.attr_name)
                                {
                                    case "doctor_rating":
                                        bool bTemp = double.TryParse(k.value, out appt_ext_rate);

                                        break;
                                    case "time_slot":
                                        appt_ext_timeslot = k.value;
                                        break;

                                    //case "doctor_id":
                                    //    bool bTemp2 = long.TryParse(k.value, out doctor_id_var);
                                    //    break;

                                    case "proposed_doctor":
                                        //hasProposed = true;
                                        //// id|doctor_id|date|time_slot
                                        //string[] doc1 = k.value.Split('|');
                                        //long prop_appt = Convert.ToInt64(doc1[0]);
                                        //long prop_doctor = Convert.ToInt64(doc1[0]);
                                        //string prop_date = doc1[1];
                                        //string prop_time = doc1[2];

                                        //prop_doc.Add(new doctor_profile
                                        //{
                                        //    //id = k.id,
                                        //    id = prop_doctor,
                                        //    date = prop_date,
                                        //    time_slot = prop_time
                                        //});
                                        break;
                                }
                            }
                        }
                        #endregion

                        appt_cur.Add(new dashboard_Appointment
                            {
                                appointment_id = qcur.id,
                                patient_id = qcur.rel_SOUL_id.Value,
                                first_name = name_first,
                                last_name = name_last,

                                status = (qcur.rel_ref_status_id.Value == 10) ? "Active" : "Inactive",
                                //time_slot = appt_ext_timeslot,
                                date = qcur.date == null ? "" : qcur.date.Value.ToUniversalTime().GetDateTimeFormats()[3],
                                //appointment = (i.ref_APPOINTMENT_type == null)?"": i.ref_APPOINTMENT_type.dname,
                                user_id = qcur.create_by__USER_id.Value,
                                time_slot = appt_ext_timeslot,
                                //doctor_id = doctor_id,
                                //doctor_name = doctor_id > 0 ? doc_name : "",
                                //doctor_name = "",
                                //**rating = appt_ext_rate,
                                //doctor = doc.Count() > 0 ? doc : prop_doc,
                                //doctor = doc,
                                //proposed_doctor = prop_doc,
                                 date_created = qcur.dt_create.GetDateTimeFormats()[56]
                                //appointment_type = appt_ty
                                //specialty = (doc_specialty == null) ? "" : doc_specialty,
                                //doctor_image = (image_url == null) ? "" : image_url
                            });
                        //}
                    }
                 

                   
                   // List<appt_type> appt_ty = new List<appt_type>();
                    //List<appt_type> doc_appt = new List<appt_type>();

                  
                  

                }
                #endregion


                List<doctor_dashboard> doc_dash = new List<doctor_dashboard>();
                doc_dash.Add(new doctor_dashboard {
                    pending = new_APPOINTMENT,
                    current = appt_cur,
                    marketplace = appt_Unassigned
                });

                string msg="";
                if (doc_dash.Count() == 0)
                {
                    msg = "No result found.";
                    return Json(new { data = new string[] { }, message = msg, success = false });
                }
                else
                {
                    var ret1 = JsonConvert.SerializeObject(doc_dash);
                    var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                    msg = new_APPOINTMENT.Count() + (new_APPOINTMENT.Count() > 1 ? " Pending Records found!" : " Pending Record found!");
                    msg += " " + appt_cur.Count() + (appt_cur.Count() > 1 ? " Current Records found!" : " Current Record found!");
                    msg += " " + appt_Unassigned.Count() + (appt_Unassigned.Count() > 1 ? " Marketplace Records found!" : " Marketplace Record found!");

                    return Json(new { data = json1, message = msg, success = true });
                }
            }
            catch (Exception ex) {
                return Json(new { data = new string[] { }, message = ex.Message, success=false });
            }
        }


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




        public IHttpActionResult Post1(long user_id = 0, long select_specialty = 0, long type_appointment = 0, double lat = 0, double longi = 0, long patient_id = 0)
        {

            string msg = "particular message Appointment was set.";
            return Json(new { data = "", message = msg, success = false });
        }

        //update
        public IHttpActionResult Put1(long select_specialty = 0, long type_appointment = 0, string current_location = "", long appointment = 0)
        {

            string msg = "particular message Appointment was set.";
            return Json(new { data = "", message = msg, success = false });
        }

        //delete
        public IHttpActionResult Delete1(long select_specialty = 0, long type_appointment = 0)
        {

            string msg = "particular message Appointment was set.";
            return Json(new { data = "", message = msg, success = false });
        }
    }

    
}
