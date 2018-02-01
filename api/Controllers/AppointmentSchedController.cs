using api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace api.Controllers
{
    public class AppointmentSchedController : ApiController
    {
        SV_db1Entities db = new SV_db1Entities();
        DateTime dt = DateTime.UtcNow;


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("appointment/schedule")]
        public IHttpActionResult newAppointmentSched([FromBody] param_appointment par)
        {

            //long user_id = 0;
            //string appointment_id = null, patient_id = null, specialty_id=null, lat = null, longi = null, status = "",
            //    appointment_date = null, time_slot=null, appointment_type_id=null, doctor_id = null;

            string msg = "";

            //string root = HttpContext.Current.Server.MapPath("~/Temp");
            //var provider = new MultipartFormDataStreamProvider(root);
            try
            {
                #region "param"
                /* REQUIRED: 
                    user_id,
                 */

                /* OPTIONAL:
                    appointment_id, appointment_date, time_slot
                    patient_id
                */

                #region
                //await Request.Content.ReadAsMultipartAsync(provider);
                //foreach (var key in provider.FormData.AllKeys)
                //{

                //    foreach (var val in provider.FormData.GetValues(key))
                //    {
                //        if (key == "user_id")
                //        {
                //            IsRequired(key, val, 1);
                //            bool btemp = long.TryParse(val, out user_id);
                //        }
                //        else if (key == "appointment_id")
                //        {
                //            //IsRequired(key, val, 1);
                //            //bool isnum = long.TryParse(val, out appointment_id);
                //            appointment_id = val;
                //        }
                //        else if (key == "appointment_type_id")
                //        {
                //            //IsRequired(key, val, 1);
                //            appointment_type_id = val;
                //        }
                //        else if (key == "patient_id")
                //        {
                //            IsRequired(key, val, 1);
                //            //bool isnum = long.TryParse(val, out appointment_id);
                //            patient_id = val;
                //        }
                //        else if (key == "lat")
                //        {
                //            lat = val;
                //        }
                //        else if (key == "longi")
                //        {
                //            longi = val;
                //        }
                //        else if (key == "appointment_date")
                //        {
                //            appointment_date = val;
                //        }
                //        else if (key == "doctor_id")
                //        {
                //            //bool nTemp = long.TryParse(val, out doctor_id);
                //            doctor_id = val;
                //        }
                //        else if (key == "specialty_id")
                //        {
                //            //IsRequired(key, val, 1);
                //            specialty_id = val;
                //        }
                //        else if (key == "time_slot")
                //        {
                //            //bool nTemp1 = long.TryParse(val, out time_slot);
                //            time_slot = val;
                //        }
                //        else if (key == "status")
                //        {
                //            status = val;
                //            //appointment_date = val;
                //        }

                //        else
                //        {
                //            return Json(new { data = new string[] { }, message = "Object reference not set to an instance of an object. Invalid parameter name: " + key, success = false });
                //        }
                //    }
                //}
                #endregion



                IsRequired("user_id", par.user_id.ToString(), 2);
                //IsRequired("appointment_type_id", appointment_type_id, 2);
                IsRequired("patient_id", par.patient_id.ToString(), 2);

                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }
                #endregion

                bool hasupdate = false;

                #region "validation"
                // checking patient_id is
                long patient_id_new = 0;
                //if (!string.IsNullOrEmpty(patient_id))
                if(par.patient_id > 0)
                {
                    bool bTemp = long.TryParse(par.patient_id.ToString(), out patient_id_new);
                    if (!bTemp)
                    {
                        return Json(new { data = new string[] { }, message = "Invalid patient_id value.", success = false });
                    }

                    var _soul = db.SOULs.Find(patient_id_new);
                    if (_soul == null)
                    {
                        return Json(new { data = new string[] { }, message = "Invalid patient_id value.", success = false });
                    }
                }
                   


                long appt_type_id = api.Models.Validation.checkApptType(par.appointment_type_id);
                //if (!string.IsNullOrEmpty(appointment_type_id))
                if(par.appointment_type_id > 0)
                {
                    if (appt_type_id == 0)
                    {
                        return Json(new { data = new string[] { }, message = "Invalid appoitnment_type_id value.", success = false });
                    }
                }
                
                long appt_status_id = string.IsNullOrEmpty(par.status) ? 0 : Validation.checkStatus(par.status);
                if (!string.IsNullOrEmpty(par.status))
                {
                    if (appt_status_id == 0)
                    {
                        return Json(new { data = new string[] { }, message = "Invalid appoitnment_status_id value.", success = false });
                    }
                }


                long appointment_id_new = 0;
                bool bTemp1 = false;
                //if (!string.IsNullOrEmpty(appointment_id))
                if(par.appointment_id > 0)
                { bTemp1 = long.TryParse(par.appointment_id.ToString(), out appointment_id_new); }

                // for development purposes
                if (Request.RequestUri.ToString().Contains("localhost"))
                {
                    var ref_appt_stat = db.ref_APPOINTMENT_status.Where(a =>a.dname.ToLower() == "pending");
                    // 2 Current

                    var ref_stat = db.ref_status.Where(a => a.dname.ToLower() == "active" && a.ref_status_type.dname.ToLower() == "appointment").FirstOrDefault();
                    // 10 Pending
                }
                
                var querydata1 = (from a in db.APPOINTMENTs where a.rel_ref_APPOINTMENT_status_id == 2 && a.rel_ref_status_id == 10 && a.id == appointment_id_new select a);
                if (querydata1.Count() > 0)
                {
                    return Json(new { data = new string[] { }, message = "This slot is already occupied by some other patient", success = false });
                }

                //if (doctor_id != null && time_slot != null && appointment_date != null)
                if (par.doctor_id > 0 && par.time_slot != null && par.appointment_date != null)
                {
                    var querydata = (from a in db.APPOINTMENT_ext where (a.attr_name == "doctor_id" && a.value == par.doctor_id.ToString()) select a.rel_APPOINTMENT_id);
                   
                    if (querydata.Count() > 0)
                    {
                        foreach (var i in querydata)
                        {
                                var new_appt_date = Convert.ToDateTime(par.appointment_date);
                                var querydata_time_slot = (from a in db.APPOINTMENT_ext where (a.attr_name == "time_slot" && a.value == par.time_slot.ToString() && a.rel_APPOINTMENT_id == i.Value) select a.value).FirstOrDefault();
                                var querydata_appointment_date = (from a in db.APPOINTMENTs where (a.date == new_appt_date && a.id == i.Value) select a.date).FirstOrDefault();
                                if (querydata_time_slot != null && querydata_time_slot == par.time_slot && querydata_appointment_date != null && querydata_appointment_date == Convert.ToDateTime(par.appointment_date))
                                {
                                    return Json(new { data = new string[] { }, message = "This slot is already occupied by some other patient", success = false });
                                }
                        }
                    }  
                }

                #endregion


                List<appt_id> d = new List<appt_id>();
                if (appointment_id_new > 0)
                {
                    #region "updateAppointment"

                    var appt = db.APPOINTMENTs.Find(appointment_id_new);
                    appt.rel_ref_APPOINTMENT_status_id = 2;// Convert.ToInt64(appt_status_id);
                    if (Convert.ToInt64(appt_type_id) > 0)
                    {
                        appt.rel_ref_APPOINTMENT_type_id = Convert.ToInt64(appt_type_id);
                    }
                    //if (!string.IsNullOrEmpty(patient_id)) 
                    if(par.patient_id > 0)
                    {
                        appt.rel_SOUL_id = par.patient_id;
                    }
                       
                    appt.rel_ref_status_id = 10;
                    //appt.create_by__USER_id = Convert.ToInt64(user_id);
                    appt.update_by__USER_id = Convert.ToInt64(par.user_id);
                    appt.dt_update = dt;
                    if (par.appointment_date != null)
                    {
                        appt.date = Convert.ToDateTime(par.appointment_date);
                    }

                    db.Entry(appt).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    #region 
                    //if (!string.IsNullOrEmpty(appointment_date))
                    //{
                    //    DateTime appt_date = DateTime.Now;
                    //    string format = "MM/dd/yyyy";
                    //    bool isD = DateTime.TryParseExact(appointment_date, format,
                    //        CultureInfo.InvariantCulture,
                    //        System.Globalization.DateTimeStyles.None, out appt_date);

                    //    //if (appt_date.CompareTo(dt) => 0)
                    //    //{
                    //    appt.date = Convert.ToDateTime(appointment_date);
                    //    db.Entry(appt).State = System.Data.Entity.EntityState.Modified;
                    //    db.SaveChanges();
                    //    //}
                    //    //else
                    //    //{
                    //    //    return Json(new { data = new string[] { }, message = "Invalid Date value in appointment_date.", success = false });
                    //    //}
                    //}
                    #endregion

                    var appt_ext = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == appointment_id_new);
                    //bool doc_updated = false, lat_update = false, longi_update = false, specialty_update=false;
                    //bool time_updated = false;

                    // doctor_id
                    // "doctor_id", string _dname, string _value, long appt_id = 0, long user_id = 0
                    bool i2 = Entry.saveAppt_ext("doctor_id", "Doctor ID", par.doctor_id.ToString(), appointment_id_new, par.user_id);

                    // time_slot
                    i2 = Entry.saveAppt_ext("time_slot", "Time Slot", par.time_slot.ToString(), appointment_id_new, par.user_id);

                    // latitude
                    i2 = Entry.saveAppt_ext("lat", "Latitude", par.lat.ToString(), appointment_id_new, par.user_id);
                    // longitude
                    i2 = Entry.saveAppt_ext("longi", "Longitude", par.longi.ToString(), appointment_id_new, par.user_id);
                    // specialty_id
                    i2 = Entry.saveAppt_ext("specialty_id", "Specialty ID", par.specialty_id.ToString(), appointment_id_new, par.user_id);

                    // comment the code below
                    #region
                    //foreach (var i in appt_ext)
                    //{
                    //    switch (i.attr_name)
                    //    {
                    //        case "doctor_id":
                    //            #region
                    //            //if (!string.IsNullOrEmpty(doctor_id))
                    //            //{
                    //            //    doc_updated = true;

                    //            //    i.value = doctor_id.ToString();
                    //            //    i.update_by__USER_id = user_id;
                    //            //    i.rel_ref_datatype_id = 3;
                    //            //    i.dt_update = dt;
                    //            //    db.Entry(i).State = System.Data.Entity.EntityState.Modified;
                    //            //}
                    //            #endregion


                    //            break;
                    //        case "time_slot":
                    //            #region
                    //            //if (!string.IsNullOrEmpty(time_slot))
                    //            //{
                    //            //    time_updated = true;
                    //            //    i.value = time_slot.ToString();
                    //            //    i.update_by__USER_id = user_id;
                    //            //    i.rel_ref_datatype_id = 3;
                    //            //    i.dt_update = dt;
                    //            //    db.Entry(i).State = System.Data.Entity.EntityState.Modified;
                    //            //}
                    //            #endregion

                    //            break;

                    //        case "lat":
                    //            #region
                    //            //if (!string.IsNullOrEmpty(lat))
                    //            //{
                    //            //    lat_update = true;
                    //            //    i.value = lat.ToString();
                    //            //    i.update_by__USER_id = user_id;
                    //            //    i.rel_ref_datatype_id = 3;
                    //            //    i.dt_update = dt;
                    //            //    db.Entry(i).State = System.Data.Entity.EntityState.Modified;
                    //            //}

                    //            #endregion


                    //            break;
                    //        case "longi":
                    //            #region
                    //            //if (!string.IsNullOrEmpty(longi))
                    //            //{
                    //            //    longi_update = true;

                    //            //    i.value = longi.ToString();
                    //            //    i.update_by__USER_id = user_id;
                    //            //    i.rel_ref_datatype_id = 3;
                    //            //    i.dt_update = dt;
                    //            //    db.Entry(i).State = System.Data.Entity.EntityState.Modified;
                    //            //}
                    //            #endregion



                    //            break;
                    //        case "specialty_id":
                    //            #region
                    //            //if (!string.IsNullOrEmpty(specialty_id))
                    //            //{
                    //            //    specialty_update = true;

                    //            //    i.value = specialty_id.ToString();
                    //            //    i.update_by__USER_id = user_id;
                    //            //    i.rel_ref_datatype_id = 3;
                    //            //    i.dt_update = dt;
                    //            //    db.Entry(i).State = System.Data.Entity.EntityState.Modified;
                    //            //}
                    //            #endregion



                    //            break;
                    //    }
                    //}
                    #endregion

                    #region
                    //if (!string.IsNullOrEmpty(doctor_id))
                    //{
                    //    if (!doc_updated)
                    //    {
                    //        #region "doctor_id"
                    //        APPOINTMENT_ext doc_appointment = new APPOINTMENT_ext
                    //        {
                    //            rel_APPOINTMENT_id = appt.id,
                    //            attr_name = "doctor_id",
                    //            dname = "DOCTOR",
                    //            value = doctor_id.ToString(),
                    //            rel_ref_datatype_id = 3
                    //        };
                    //        db.APPOINTMENT_ext.Add(doc_appointment);
                    //        #endregion
                    //    }
                    //}

                    //if (!string.IsNullOrEmpty(time_slot))
                    //{
                    //    if (!time_updated)
                    //    {
                    //        #region"time_slot"
                    //        APPOINTMENT_ext doc_appointment = new APPOINTMENT_ext
                    //        {
                    //            rel_APPOINTMENT_id = appt.id,
                    //            attr_name = "time_slot",
                    //            dname = "Time Slot",
                    //            value = time_slot,
                    //            rel_ref_datatype_id = 1
                    //        };
                    //        db.APPOINTMENT_ext.Add(doc_appointment);
                    //        #endregion
                    //    }
                    //}

                    //if (!string.IsNullOrEmpty(lat))
                    //{
                    //    if (!lat_update) {
                    //        #region "lat"
                    //        APPOINTMENT_ext addAppointment_lat = new APPOINTMENT_ext
                    //        {
                    //            rel_APPOINTMENT_id = appt.id,
                    //            attr_name = "lat",
                    //            value = lat,
                    //            dname = "Latitude",
                    //            rel_ref_datatype_id = 1,// double
                    //            create_by__USER_id = Convert.ToInt64(user_id),
                    //            dt_create = dt,
                    //        };
                    //        db.APPOINTMENT_ext.Add(addAppointment_lat);
                    //        #endregion

                    //    }
                    //}

                    //if (!string.IsNullOrEmpty(longi))
                    //{
                    //    if (!longi_update)
                    //    {
                    //        #region "longi"
                    //        APPOINTMENT_ext addAppointment_longi = new APPOINTMENT_ext
                    //        {
                    //            rel_APPOINTMENT_id = appt.id,
                    //            attr_name = "longi",
                    //            value = longi,
                    //            dname = "Longitude",
                    //            rel_ref_datatype_id = 1,// double
                    //            create_by__USER_id = Convert.ToInt64(user_id),
                    //            dt_create = dt,
                    //        };
                    //        db.APPOINTMENT_ext.Add(addAppointment_longi);
                    //        #endregion   }
                    //    }
                    //}

                    //if (!string.IsNullOrEmpty(specialty_id))
                    //{
                    //    if (!specialty_update)
                    //    {
                    //        #region "specialty_id"
                    //            APPOINTMENT_ext addAppointment_select_specialty = new APPOINTMENT_ext
                    //            {
                    //                rel_APPOINTMENT_id = appt.id,
                    //                attr_name = "specialty_id",
                    //                value = specialty_id,
                    //                dname = "Specialty",
                    //                rel_ref_datatype_id = 3, // string
                    //                create_by__USER_id = Convert.ToInt64(user_id),
                    //                dt_create = dt,
                    //            };
                    //            db.APPOINTMENT_ext.Add(addAppointment_select_specialty);
                    //            #endregion
                    //    }
                    //}

                    //db.SaveChanges();
                    #endregion


                    // update to [notification] table
                    var notif = db.notifications.Where(a => a.APPOINTMENT_id == appt.id);
                    if (notif.Count() > 0)
                    {
                        foreach (var n in notif)
                        {
                            //n.text = "Appointment has been cancelled.";
                            n.rel_USER_id = Convert.ToInt64(par.user_id);
                            n.is_unread = true;
                            n.update_by__USER_id = Convert.ToInt64(par.user_id);
                            n.dt_update = dt;
                            db.Entry(n).State = System.Data.Entity.EntityState.Modified;
                        }
                        db.SaveChanges();
                    }

                    msg = "The appointment has been scheduled successfully.";
                    // end region: updateAppointment
                    #endregion
                }

               

                if (appointment_id_new == 0)
                {
                    #region "newAppointment"

                    APPOINTMENT appt = new APPOINTMENT();
                    //APPOINTMENT_ext appt_ext = new APPOINTMENT_ext();

                    // status_id
                    // 1 Pending
                    // 2 Status
                    // 3 History

                    appt.rel_ref_APPOINTMENT_status_id = 2;// Convert.ToInt64(appt_status_id);
                    appt.rel_ref_APPOINTMENT_type_id = Convert.ToInt64(appt_type_id);
                    //if (patient_id_new > 0)
                    //{
                        appt.rel_SOUL_id = Convert.ToInt64(par.patient_id);
                    //}
                    
                    appt.rel_ref_status_id = 10;
                    appt.create_by__USER_id = Convert.ToInt64(par.user_id);
                    appt.dt_create = dt;
                    if (par.appointment_date != null)
                    {
                        appt.date = Convert.ToDateTime(par.appointment_date);
                    }
                    db.APPOINTMENTs.Add(appt);
                    db.SaveChanges();


                  
                  
                   
                    //if (!string.IsNullOrEmpty(doctor_id))
                    if(par.doctor_id > 0)
                    {
                        // doctor_id
                        bool i3 = Entry.saveAppt_ext("doctor_id", "Doctor ID", par.doctor_id.ToString(), appt.id, par.user_id, 3);
                        #region 
                        //    APPOINTMENT_ext new_doc = new APPOINTMENT_ext()
                        //    {
                        //        rel_APPOINTMENT_id = appt.id,
                        //        attr_name = "doctor_id",
                        //        dname = "Doctor ID",
                        //        value = doctor_id.ToString(),
                        //        rel_ref_datatype_id = 3,
                        //        create_by__USER_id = user_id,
                        //        dt_create = dt
                        //    };
                        //    db.APPOINTMENT_ext.Add(new_doc);
                        #endregion
                    }


                  
                    if (!string.IsNullOrEmpty(par.time_slot))
                    {
                        // time_slot
                        bool i3 = Entry.saveAppt_ext("time_slot", "Time Slot", par.time_slot.ToString(), appt.id, par.user_id, 3);

                        #region
                        //    APPOINTMENT_ext new_doc = new APPOINTMENT_ext()
                        //    {
                        //        rel_APPOINTMENT_id = appt.id,
                        //        attr_name = "time_slot",
                        //        dname = "Time Slot",
                        //        value = time_slot.ToString(),
                        //        rel_ref_datatype_id = 3,
                        //        create_by__USER_id = user_id,
                        //        dt_create = dt
                        //    };
                        //    db.APPOINTMENT_ext.Add(new_doc);
                        #endregion
                    }



                    //if (!string.IsNullOrEmpty(lat))
                    if(par.lat != 0)
                    {
                        // lat
                        bool i3 = Entry.saveAppt_ext("lat", "Latitude", par.lat.ToString(), appt.id, par.user_id, 3);

                        #region
                        //APPOINTMENT_ext new_doc = new APPOINTMENT_ext()
                        //{
                        //    rel_APPOINTMENT_id = appt.id,
                        //    attr_name = "lat",
                        //    dname = "Latitude",
                        //    value = lat,
                        //    rel_ref_datatype_id = 3,
                        //    create_by__USER_id = user_id,
                        //    dt_create = dt
                        //};
                        //db.APPOINTMENT_ext.Add(new_doc);
                        #endregion
                    }

                    //if (!string.IsNullOrEmpty(longi))
                    if(par.longi != 0)
                    {
                        // longi
                        bool i3 = Entry.saveAppt_ext("longi", "Longitude", par.longi.ToString(), appt.id, par.user_id, 3);

                        #region
                        //APPOINTMENT_ext new_doc = new APPOINTMENT_ext()
                        //{
                        //    rel_APPOINTMENT_id = appt.id,
                        //    attr_name = "longi",
                        //    dname = "Longitude",
                        //    value = longi,
                        //    rel_ref_datatype_id = 3,
                        //    create_by__USER_id = user_id,
                        //    dt_create = dt
                        //};
                        //db.APPOINTMENT_ext.Add(new_doc);
                        #endregion

                    }

                    //if (!string.IsNullOrEmpty(specialty_id))
                    if(par.specialty_id > 0)
                    {
                        // specialty_id
                        bool i3 = Entry.saveAppt_ext("specialty_id", "Specialty ID", par.specialty_id.ToString(), appt.id, par.user_id, 3);

                        #region
                        //APPOINTMENT_ext new_doc = new APPOINTMENT_ext()
                        //{
                        //    rel_APPOINTMENT_id = appt.id,
                        //    attr_name = "specialty_id",
                        //    dname = "Specialty ID",
                        //    value = specialty_id,
                        //    rel_ref_datatype_id = 3,
                        //    create_by__USER_id = user_id,
                        //    dt_create = dt
                        //};
                        //db.APPOINTMENT_ext.Add(new_doc);
                        #endregion
                    }

                    //db.SaveChanges();

                    d.Add(new appt_id { appointment_id = appt.id });
                    hasupdate = true;

                    // insert to [notification] table
                    notification notif = new notification
                    {
                        //text = "Appoinment detail has been sent to the doctor.",
                        text = "The appointment has been scheduled successfully.",
                        link = "",
                        SOUL_id = Convert.ToInt64(par.patient_id),
                        rel_USER_id = Convert.ToInt64(par.user_id),
                        APPOINTMENT_id = appt.id,
                        is_unread = true,
                        dt_create = dt,
                        create_by__USER_id = Convert.ToInt64(par.user_id)
                    };
                    db.notifications.Add(notif);
                    db.SaveChanges();

                    //msg = "Appoinment detail has been sent to the doctor.";
                    msg = "The appointment has been scheduled successfully.";
                    #endregion


                }

               

                var ret1 = JsonConvert.SerializeObject(d);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                //string msg = "The new record is saved successfully.";
                 
                return Json(new { data = json1, message = msg, success = true });


            }
            catch (Exception ex) {
                return Json(new { data = new string[] { }, message = ex.Message, success= false });
            }

        }

        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("appointment/schedule")]
        public async Task<IHttpActionResult> updateAppointmentSched()
        {
            string   appointment_date = null;
            string lat = null,  lng = null, status="";
            long doctor_id_new = 0, appointment_type_id = 0, user_id = 0;
            string appointment_id = "", time_slot = "", doctor_id ="";
            DateTime dtNow = DateTime.Now;
            

            //Schedule Appointment (change appointment status and add doctor)

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
                            bool btemp = long.TryParse(val, out user_id);
                              
                        }
                        else if (key == "appointment_id")
                        {
                            IsRequired(key, val, 1);
                            //bool isnum = long.TryParse(val, out appointment_id);
                            appointment_id = val;
                        }
                      
                        //else if (key == "specialty_id")
                        //{
                        //    specialty_id = val;
                        //}
                        //else if (key == "appointment_type_id")
                        //{
                        //    bool bTemp = long.TryParse(val, out appointment_type_id);
                        //    //appointment_type_id = val;
                        //}
                        else if (key == "lat")
                        {
                            lat = val;
                        }
                        else if (key == "lng")
                        {
                            lng = val;
                        }
                        else if (key == "appointment_date")
                        {
                           
                            appointment_date = val;
                        }
                        else if (key == "doctor_id")
                        {
                            IsRequired(key, val, 1);
                            doctor_id = val;
                            bool nTemp = long.TryParse(val, out doctor_id_new);
                            //appointment_date = val;
                        }
                        else if (key == "time_slot")
                        {
                            //bool nTemp1 = long.TryParse(val, out time_slot);
                            time_slot = val;
                        }
                        else if (key == "status")
                        {
                            status = val;
                            //appointment_date = val;
                        }
                        else
                        {
                            return Json(new {data = new string[] { }, message = "Object reference not set to an instance of an object. Invalid parameter name: " + key, success = false });
                        }
                    }
                }

                IsRequired("user_id", user_id.ToString(), 2);
                IsRequired("appointment_id", appointment_id, 2);
             
                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }
                else
                {
                    long appointment_id_new = 0;
                    bool bTemp = long.TryParse(appointment_id, out appointment_id_new);
                    if (!bTemp) // appointment_id not valid
                    {
                        return Json(new { data = new string[] { }, message = "Invalid appointment_id value.", success = false });
                    }

                   var appt = db.APPOINTMENTs.Find(appointment_id_new);
                    if (appt == null)
                    {
                        return Json(new { data = new string[] { }, message = "No record found for appointment_id.", success = false });
                    }
                    else
                    {
                        bool hasupdate = false;
                        long appt_type = appt.rel_ref_APPOINTMENT_type_id.Value;

                        // schedule appointmnet kay mao ni mag butang ug doctor ug time slot sa appointment
                        //time_slot => appoint_ext
                        //doctor_id = appointment_ext

                        var doc = db.DOCTORs.Find(doctor_id_new);
                        // update DOCTOR in dbo.Appointment_ext
                        if (doc == null)
                        {
                            return Json(new { data = new string[] { }, message = "Invalid value for doctor_id.", success = false });
                        }
                        else // doctor_id existed in DOCTOR table
                        {

                            var appoint_ext = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == appointment_id_new);

                            if (!string.IsNullOrEmpty(appointment_date))
                            {
                                DateTime appt_date = DateTime.Now;
                                string format= "MM/dd/yyyy";
                                bool isD = DateTime.TryParseExact(appointment_date, format,
                                    CultureInfo.InvariantCulture,
                                    System.Globalization.DateTimeStyles.None, out appt_date);

                                //if (appt_date.CompareTo(dt) => 0)
                                //{
                              
                                appt.date = Convert.ToDateTime(appointment_date);
                               
                                //}
                                //else
                                //{
                                //    return Json(new { data = new string[] { }, message = "Invalid Date value in appointment_date.", success = false });
                                //}
                            }

                            if (!string.IsNullOrEmpty(lat) || !string.IsNullOrEmpty(lng) || !string.IsNullOrEmpty(appointment_date)
                                || !string.IsNullOrEmpty(doctor_id) || !string.IsNullOrEmpty(time_slot) || !string.IsNullOrEmpty(status)) {
                                appt.rel_ref_APPOINTMENT_status_id = 2; // current
                                db.Entry(appt).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }
                            

                            bool doc_updated = false;
                            bool time_updated = false;
                            foreach (var i in appoint_ext)
                            {
                                switch (i.attr_name) {
                                    case "doctor_id":
                                        if (!string.IsNullOrEmpty(doctor_id))
                                        {
                                            doc_updated = true;

                                            i.value = doctor_id.ToString();
                                            i.update_by__USER_id = user_id;
                                            i.rel_ref_datatype_id = 3;
                                            i.dt_update = dt;
                                            db.Entry(i).State = System.Data.Entity.EntityState.Modified;
                                        }
                                        
                                        break;
                                    case "time_slot":
                                        if (!string.IsNullOrEmpty(time_slot)) {
                                            time_updated = true;
                                            i.value = time_slot.ToString();
                                            i.update_by__USER_id = user_id;
                                            i.rel_ref_datatype_id = 3;
                                            i.dt_update = dt;
                                            db.Entry(i).State = System.Data.Entity.EntityState.Modified;
                                        }
                                        
                                        break;
                                }

                            }

                            if (!doc_updated)
                            {
                                APPOINTMENT_ext new_doc = new APPOINTMENT_ext() {
                                    rel_APPOINTMENT_id = appointment_id_new,
                                    attr_name = "doctor_id",
                                    dname ="Doctor ID",
                                    value = doctor_id.ToString(),
                                    rel_ref_datatype_id =3,
                                    create_by__USER_id = user_id,
                                    dt_create = dt
                                };
                                db.APPOINTMENT_ext.Add(new_doc);
                             }

                            if (!time_updated)
                            {
                                APPOINTMENT_ext new_doc = new APPOINTMENT_ext()
                                {
                                    rel_APPOINTMENT_id = appointment_id_new,
                                    attr_name = "time_slot",
                                    dname = "Time Slot",
                                    value = time_slot.ToString(),
                                    rel_ref_datatype_id = 3,
                                    create_by__USER_id = user_id,
                                    dt_create = dt
                                };
                                db.APPOINTMENT_ext.Add(new_doc);
                            }

                            db.SaveChanges();


                            hasupdate = true;


                            // update to [notification] table
                            var notif = db.notifications.Where(a => a.APPOINTMENT_id == appointment_id_new);
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
                        }


                        infomsg = "Appointment is updated successfully.";// with this appointment_id: " + appointment_id_new;
                    }
                    //else
                    //{
                    //    infomsg = "No matching record.";// found with this appointment_id: " + appointment_id_new;
                    //}

                    //List<appt_id> d = new List<appt_id>();
                    //d.Add(new appt_id { appointment_id = appointment_id_new });
                    //var ret1 = JsonConvert.SerializeObject(d);
                    //var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                    return Json(new { data= new string[] { }, message = infomsg, success = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }

        //[HttpGet]
        //[Route("doctor/time1slot")]
        //public async Task<IHttpActionResult> timeslot1(string doctor_id = null)
        //{
        //    long user_id = 0;
        //    //string appointment_id = null, patient_id = null, specialty_id = null, lat = null, longi = null, status = "",
        //    //    appointment_date = null, time_slot = null, appointment_type_id = null, doctor_id = null;


        //    //string root = HttpContext.Current.Server.MapPath("~/Temp");
        //    //var provider = new MultipartFormDataStreamProvider(root);

        //    try {
        //        #region "param"
        //        //await Request.Content.ReadAsMultipartAsync(provider);
        //        //foreach (var key in provider.FormData.AllKeys)
        //        //{

        //        //    foreach (var val in provider.FormData.GetValues(key))
        //        //    {
        //        //        if (key == "doctor_id")
        //        //        {
        //        //            IsRequired(key, val, 1);
        //        //            // bool btemp = long.TryParse(val, out user_id);
        //        //            doctor_id = val;
        //        //        }


        //        //        else
        //        //        {
        //        //            return Json(new { data = new string[] { }, message = "Object reference not set to an instance of an object. Invalid parameter name: " + key, success = false });
        //        //        }
        //        //    }
        //        //}

        //        IsRequired("doctor_id", user_id.ToString(), 2);
        //        if (haserror)
        //        {
        //            return Json(new { data = new string[] { }, message = errmsg, success = false });
        //        }
        //        #endregion

        //        long doc_id_new = 0;
        //        bool bDoc = long.TryParse(doctor_id, out doc_id_new);
        //        if (!bDoc)
        //        {
        //            return Json(new { data = new string[] { }, message = "Invalid doctor_id. Value must be numeric.", success = false });
        //        }

        //        else
        //        {
        //            var doc = db.DOCTORs.Find(doc_id_new);
        //            if (doc == null)
        //            {
        //                return Json(new { data = new string[] { }, message = "Invalid doctor_id.", success = false });
        //            }

        //            var doc_ext = db.DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doc.id && a.attr_name == "time_slot")
        //                .ToList();
        //           var doc_ext1 = doc_ext.OrderBy(a => DateTime.Parse(a.value));

        //            if (doc_ext1 == null)
        //            {
        //                return Json(new { data = new string[] { }, message = "Invalid doctor_id.", success = false });
        //            }
        //            else // doc_ext is not empty, so retrieved the time_slot schedule
        //            {
        //                List<schedule> tm = new List<schedule>();
        //                //List<schedule> t_slot = new List<schedule>();
        //                List<slot1> slot = new List<slot1>();
        //                foreach (var n in doc_ext1)
        //                {

        //                    //tm.Add(new timeslot{ doctor_id = n.rel_DOCTOR_id.Value });
        //                    //slot.Add(n.value);
        //                    //slot.Add(new slot { date = n.date.Value.GetDateTimeFormats()[56], time = n.time });
        //                }

        //                tm.Add(new schedule {
        //                    doctor_id = doc_id_new,
        //                    timeslot = slot
        //                });

        //                var ret1 = JsonConvert.SerializeObject(tm);
        //                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
        //                return Json(new { data = json1, message = errmsg, success = true });
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { data = new string[] { }, message = errmsg, success = false });
        //    }

        //}

        [HttpGet]
        [Route("doctor/timeslot1")]
        public async Task<IHttpActionResult> timeslot1(string doctor_id = null, string date = null)
        {
            long user_id = 0;
            //string appointment_id = null, patient_id = null, specialty_id = null, lat = null, longi = null, status = "",
            //    appointment_date = null, time_slot = null, appointment_type_id = null, doctor_id = null;


            //string root = HttpContext.Current.Server.MapPath("~/Temp");
            //var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                #region "param"
                //await Request.Content.ReadAsMultipartAsync(provider);
                //foreach (var key in provider.FormData.AllKeys)
                //{

                //    foreach (var val in provider.FormData.GetValues(key))
                //    {
                //        if (key == "doctor_id")
                //        {
                //            IsRequired(key, val, 1);
                //            // bool btemp = long.TryParse(val, out user_id);
                //            doctor_id = val;
                //        }


                //        else
                //        {
                //            return Json(new { data = new string[] { }, message = "Object reference not set to an instance of an object. Invalid parameter name: " + key, success = false });
                //        }
                //    }
                //}

                IsRequired("doctor_id", user_id.ToString(), 2);
                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }
                #endregion

                long doc_id_new = 0;
                bool bDoc = long.TryParse(doctor_id, out doc_id_new);
                if (!bDoc)
                {
                    return Json(new { data = new string[] { }, message = "Invalid doctor_id. Value must be numeric.", success = false });
                }

                else
                {
                    var doc = db.DOCTORs.Find(doc_id_new);
                    if (doc == null)
                    {
                        return Json(new { data = new string[] { }, message = "Invalid doctor_id.", success = false });
                    }

                    var doc_time = db.DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doc_id_new && a.value.Contains(date + "|")); //.OrderBy(b => b.date); //.ThenBy(c =>  DateTime.Parse(c.time));
                                                                                                                            //doc_time = doc_time.OrderBy(c => DateTime.Parse(c.time));


                    if (doc_time == null)
                    {
                        return Json(new { data = new string[] { }, message = "Invalid doctor_id.", success = false });
                    }
                    else // doc_ext is not empty, so retrieved the time_slot schedule
                    {
                        List<slot> slot = new List<slot>();
                        List<string> times = new List<string>();

                        if (date != null)
                        {
                            //DateTime dInput = DateTime.Parse(date);
                            //doc_time = doc_time.Where(a => a.date.Value.Equals(dInput)).OrderBy(b => b.date);
                        }

                        //DateTime dInput = DateTime.Parse(date);
                        ////var appt = db.APPOINTMENTs.Where(a => a.date.Value == dInput);
                        //var appt = from a in db.APPOINTMENTs where a.date.Value == dInput select a;
                        //foreach (var li in appt)
                        //{
                        //    var appt_ext = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == li.id 
                        //    && (a.attr_name =="doctor_id" && a.value == doctor_id.ToString()));

                        //    foreach (var li2 in appt_ext) {
                        //    }
                        //}

                        List<timeslot> tm = new List<timeslot>();
                        if (doc_time.Count() > 0)
                        {
                            string dt1 = date;// string dt2 = "";
                           
                            DateTime dInput = DateTime.Parse(date);
                            //var appt = db.APPOINTMENTs.Where(a => a.date.Value == dInput);
                            var appt = from a in db.APPOINTMENTs where a.date.Value == dInput select a;

                            foreach (var n in doc_time)
                            {

                                long appt_id = 0;
                                foreach (var n1 in appt)
                                {
                                    appt_id = n1.id;

                                    string[] d = n.value.Split('|');
                                    string v = d[1];// d[0] + "|" + d[1];
                                    var appt_ext = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == appt_id &&
                                                            ((a.attr_name == "doctor_id" && a.value == doctor_id.ToString())
                                                            ||
                                                            (a.attr_name == "time_slot" && a.value == v)));

                                    if (appt_ext.Count() < 2)
                                    {
                                        tm.Add(new timeslot
                                        {
                                            time = d[1]
                                        });
                                    }
                                }
                            }

                            slot.Add(new slot
                            {
                                date = dt1,
                                time = tm
                            });
                        }

                        var ret1 = JsonConvert.SerializeObject(slot);
                        var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                        errmsg = "";
                        if (slot.Count() == 0)
                        {
                            return Json(new { data = new string[] { }, message = "No record found!", success = false });
                        }
                        else
                        {
                            return Json(new { data = json1, message = tm.Count() + " time slot(s) found.", success = true });
                        }
                        
                    }
                }

            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }

        }

        [HttpGet]
        [Route("doctor/timeslot")]
        public async Task<IHttpActionResult> timeslot(string doctor_id = null, string date = null)
        {
            long user_id = 0;
            //string appointment_id = null, patient_id = null, specialty_id = null, lat = null, longi = null, status = "",
            //    appointment_date = null, time_slot = null, appointment_type_id = null, doctor_id = null;


            //string root = HttpContext.Current.Server.MapPath("~/Temp");
            //var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                #region "param"
                //await Request.Content.ReadAsMultipartAsync(provider);
                //foreach (var key in provider.FormData.AllKeys)
                //{

                //    foreach (var val in provider.FormData.GetValues(key))
                //    {
                //        if (key == "doctor_id")
                //        {
                //            IsRequired(key, val, 1);
                //            // bool btemp = long.TryParse(val, out user_id);
                //            doctor_id = val;
                //        }


                //        else
                //        {
                //            return Json(new { data = new string[] { }, message = "Object reference not set to an instance of an object. Invalid parameter name: " + key, success = false });
                //        }
                //    }
                //}

                IsRequired("doctor_id", user_id.ToString(), 2);
                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }
                #endregion

                long doc_id_new = 0;
                bool bDoc = long.TryParse(doctor_id, out doc_id_new);
                if (!bDoc)
                {
                    return Json(new { data = new string[] { }, message = "Invalid doctor_id. Value must be numeric.", success = false });
                }

                else
                {
                    var doc = db.DOCTORs.Find(doc_id_new);
                    if (doc == null)
                    {
                        return Json(new { data = new string[] { }, message = "Invalid doctor_id.", success = false });
                    }

                    var doc_time = db.con_DOCTOR_timeslot.Where(a => a.rel_DOCTOR_id == doc_id_new).OrderBy(b => b.date); //.ThenBy(c =>  DateTime.Parse(c.time));
                                                                                                                          //doc_time = doc_time.OrderBy(c => DateTime.Parse(c.time));


                    if (doc_time == null)
                    {
                        return Json(new { data = new string[] { }, message = "Invalid doctor_id.", success = false });
                    }
                    else // doc_ext is not empty, so retrieved the time_slot schedule
                    {
                        //List<timeslot> tm = new List<timeslot>();
                        //List<string> slot = new List<string>();
                        //foreach (var n in doc_ext1)
                        //{
                        //     slot.Add(n.value);
                        //}

                        //tm.Add(new Controllers.timeslot
                        //{
                        //    doctor_id = doc_id_new,
                        //    slot = slot
                        //});

                        //var ret1 = JsonConvert.SerializeObject(tm);
                        //var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                        //return Json(new { data = json1, message = errmsg, success = true });

                        List<schedule> tm = new List<schedule>();
                        List<slot1> slot = new List<slot1>();
                        List<string> times = new List<string>();

                        if (date != null)
                        {
                            DateTime dInput = DateTime.Parse(date);
                            doc_time = doc_time.Where(a => a.date.Value.Equals(dInput)).OrderBy(b => b.date);
                        }

                        if (doc_time.Count() > 0)
                        {
                            string dt1 = ""; string dt2 = "";
                            foreach (var n in doc_time)
                            {
                                //string dt3 = DateTime.Parse(n.date.ToString()).ToShortDateString() +" "+ n.time;
                                //if (dt1 == "")
                                //{
                                //    dt1 = n.date.Value.GetDateTimeFormats()[0];
                                //}
                                //string dt2 = n.date.Value.GetDateTimeFormats()[0];

                                //times.Add(n.time);
                                dt1 = DateTime.Parse(n.date.ToString()).ToShortDateString();
                                slot.Add(new slot1
                                {
                                    date = dt1,
                                    time = n.time
                                });

                                //dt2 = DateTime.Parse(n.date.ToString()).ToShortDateString();

                            }
                        }

                        tm.Add(new schedule
                        {
                            doctor_id = doc_id_new,
                            timeslot = slot
                        });

                        var ret1 = JsonConvert.SerializeObject(tm);
                        var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                        errmsg = "";
                        return Json(new { data = json1, message = errmsg, success = true });
                    }
                }

            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = errmsg, success = false });
            }
        }


        [HttpGet]
        [Route("doctor/time2slot")]
        public async Task<IHttpActionResult> timeslot2(string doctor_id = null, string day = null)
        {
            long user_id = 0;
            //string appointment_id = null, patient_id = null, specialty_id = null, lat = null, longi = null, status = "",
            //    appointment_date = null, time_slot = null, appointment_type_id = null, doctor_id = null;


            //string root = HttpContext.Current.Server.MapPath("~/Temp");
            //var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                #region "param"
                //await Request.Content.ReadAsMultipartAsync(provider);
                //foreach (var key in provider.FormData.AllKeys)
                //{

                //    foreach (var val in provider.FormData.GetValues(key))
                //    {
                //        if (key == "doctor_id")
                //        {
                //            IsRequired(key, val, 1);
                //            // bool btemp = long.TryParse(val, out user_id);
                //            doctor_id = val;
                //        }


                //        else
                //        {
                //            return Json(new { data = new string[] { }, message = "Object reference not set to an instance of an object. Invalid parameter name: " + key, success = false });
                //        }
                //    }
                //}

                //IsRequired("doctor_id", user_id.ToString(), 2);
                //if (haserror)
                //{
                //    return Json(new { data = new string[] { }, message = errmsg, success = false });
                //}
                #endregion

                long doc_id_new = 0;
                bool bDoc = long.TryParse(doctor_id, out doc_id_new);
                if (!bDoc)
                {
                    return Json(new { data = new string[] { }, message = "Invalid doctor_id. Value must be numeric.", success = false });
                }

                var con_doc = db.con_DOCTOR_timeslot.Where(a => a.rel_DOCTOR_id == doc_id_new).OrderByDescending(b => b.id);
                if (con_doc.Count() > 0) {
                    foreach (var n1 in con_doc)
                    {
                       
                        //day | date 

                        string dt = DateTime.UtcNow.DayOfWeek.ToString();


                        //switch (n1.date) {
                        //    case "monday":
                        //    case "tuesday":
                        //    case "wednesday":
                        //    case "thursday":
                        //    case "friday":
                                
                        //        break;
                        //    //case "": break;
                        //}
                    }
                }
                return Json(new { data = new string[] { }, message = "", success = true });
            }
            catch (Exception ex) {
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
                    errmsg += " Missing parameter " + key + ". \r\n";
                    return false;
                }
                return true;
            }

        }
    }



    public class param_appointment
    {
        public long user_id { get; set; }
        public long appointment_id { get; set; }
        public long appointment_type_id { get; set; }
        public long patient_id { get; set; }
        public double lat { get; set; }
        public double longi { get; set; }
        public string appointment_date { get; set; }
        public long doctor_id { get; set; }
        public long specialty_id { get; set; }
        public string time_slot { get; set; }
        public string status { get; set; }

    }


}
