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
    public class RateDoctorController : ApiController
    {

        SV_db1Entities db = new SV_db1Entities();

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("doctor/rating")]
        public async Task<IHttpActionResult> NewRating()
        {
            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);

            long user_id = 0;
            string rate = "", doctor_id = "", appointment_id="", review = "";
            DateTime dtNow = DateTime.Now;

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        // user_id, patient_id, rate
                        bool bTemp1 = false;
                        switch (key)
                        {
                            case "user_id":
                                IsRequired(key, val, 1);
                                bTemp1 = long.TryParse(val, out user_id);
                                break;
                            case "doctor_id":
                                //IsRequired(key, val, 1);
                                //bTemp = long.TryParse(val, out doctor_id);
                                doctor_id = val;    
                                break;
                               
                            case "rate":
                                IsRequired(key, val, 1);
                                //bTemp = int.TryParse(val, out rate);
                                rate = val;
                                break;

                            case "review":
                                IsRequired(key, val, 1);
                                //bTemp = int.TryParse(val, out rate);
                                review = val;
                                break;

                            case "appointment_id":
                                IsRequired(key, val, 1);
                                appointment_id = val;
                                break;
                            default:
                                return Json(new { message = "Invalid parameter name: " + key, success = false });
                        }
                    }
                }

                IsRequired("user_id", user_id.ToString(), 2);
                //IsRequired("patient_id", doctor_id.ToString(), 3);
                IsRequired("rate", rate.ToString(), 3);
                IsRequired("appointment_id", appointment_id, 3);
                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }

                long appt_id;
                bool bTemp = long.TryParse(appointment_id, out appt_id);

                var ext_rating = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == appt_id && a.attr_name == "doctor_rating").FirstOrDefault();
                if (ext_rating != null)
                {
                    db.APPOINTMENT_ext.Remove(ext_rating);
                    db.SaveChanges();
                }

                APPOINTMENT_ext doc_ext = new APPOINTMENT_ext() {
                    rel_APPOINTMENT_id = appt_id,
                    attr_name = "doctor_rating",
                    dname = "Doctor Rating",
                    value = rate.ToString(),
                    rel_ref_datatype_id = 3,
                    create_by__USER_id =user_id,
                    dt_create = dtNow
                };
                db.APPOINTMENT_ext.Add(doc_ext);

                APPOINTMENT_ext doc_rev = new APPOINTMENT_ext()
                {
                    rel_APPOINTMENT_id = appt_id,
                    attr_name = "doctor_review",
                    dname = "Doctor Review",
                    value = review.ToString(),
                    rel_ref_datatype_id = 3,
                    create_by__USER_id = user_id,
                    dt_create = dtNow
                };
                db.APPOINTMENT_ext.Add(doc_rev);

                db.SaveChanges();

                return Json(new { data = new string[] { }, message = "New Doctor rating saved successfully.", success = true });
            }
            catch (Exception ex)
            {
                return Json(new { data="", message= ex.Message, success = false});
            }
        }




        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("doctor/updaterating")]
        public IHttpActionResult updateAppointmentSched(long user_id = 0, 
            string doctor_id = "", string appointment_id = "", string review = "",int rate = 0)
        {
            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);

            //long user_id = 0, doctor_id = 0;
            //string review = "", appointment_id = "";
            //int rate = 0;
            DateTime dtNow = DateTime.Now;

            try
            {
                
                //await Request.Content.ReadAsMultipartAsync(provider);
                //foreach (var key in provider.FormData.AllKeys)
                //{
                //    foreach (var val in provider.FormData.GetValues(key))
                //    {
                //        // user_id, patient_id, rate
                //        bool bTemp = false;
                //        switch (key)
                //        {
                //            case "user_id":
                //                IsRequired(key, val, 1);
                //                bTemp = long.TryParse(val, out user_id);
                //                break;
                //            case "doctor_id":
                //                //IsRequired(key, val, 1);
                //                bTemp = long.TryParse(val, out doctor_id);
                //                break;

                //            case "appointment_id":
                //                IsRequired(key, val, 1);
                //                appointment_id = val;
                //                break;

                //            case "review":
                //                IsRequired(key, val, 1);
                //                //bTemp = int.TryParse(val, out rate);
                //                review = val;
                //                break;

                //            case "rate":
                //                IsRequired(key, val, 1);
                //                bTemp = int.TryParse(val, out rate);
                //                break;

                //            default:
                //                return Json(new { message = "Invalid parameter name: " + key, success = false });
                //        }
                //    }
                //}

                // IsRequired("doctor_id", doctor_id.ToString(), 3);
                IsRequired("appointment_id", appointment_id, 3);
                IsRequired("rate", rate.ToString(), 3);
                IsRequired("review", review.ToString(), 2);
                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }


                //var doc_ext = db.APPOINTMENT_ext.Where(a => a.rel_DOCTOR_id == doctor_id && a.attr_name == "rating");
                long appt_id_new = 0;
                bool bAppt = long.TryParse(appointment_id, out appt_id_new);
                var appt_ext = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == appt_id_new);

                if (appt_ext.Count() > 0)
                {
                    bool hasRate = false, hasReview = false;
                    foreach (var i in appt_ext)
                    {
                        if (i.attr_name == "doctor_rating")
                        {
                            i.value = rate.ToString();
                            i.update_by__USER_id = user_id;
                            i.dt_update = dtNow;
                            db.Entry(i).State = System.Data.Entity.EntityState.Modified;
                            hasRate = true;
                        }
                        if (i.attr_name == "doctor_review")
                        {
                            i.value = review.ToString();
                            i.update_by__USER_id = user_id;
                            i.dt_update = dtNow;
                            db.Entry(i).State = System.Data.Entity.EntityState.Modified;
                            hasReview = true;
                        }

                    }

                    //long appt_id = appt_ext.FirstOrDefault().id;

                    if (!string.IsNullOrEmpty(rate.ToString()) && !hasRate)
                    {
                        APPOINTMENT_ext doc_ext = new APPOINTMENT_ext()
                        {
                            rel_APPOINTMENT_id = appt_id_new,
                            attr_name = "doctor_rating",
                            dname = "Doctor Rating",
                            value = rate.ToString(),
                            rel_ref_datatype_id = 3,
                            create_by__USER_id = user_id,
                            dt_create = dtNow
                        };
                        db.APPOINTMENT_ext.Add(doc_ext);
                    }

                    if (!string.IsNullOrEmpty(review) && !hasReview)
                    {
                        APPOINTMENT_ext doc_rev = new APPOINTMENT_ext()
                        {
                            rel_APPOINTMENT_id = appt_id_new,
                            attr_name = "doctor_review",
                            dname = "Doctor Review",
                            value = review.ToString(),
                            rel_ref_datatype_id = 3,
                            create_by__USER_id = user_id,
                            dt_create = dtNow
                        };
                        db.APPOINTMENT_ext.Add(doc_rev);
                    }


                    db.SaveChanges();

                }
                else
                {
                    return Json(new { data = "", message = "No matching record found.", success = false });
                }


                //DOCTOR_ext doc_ext = new DOCTOR_ext()
                //{
                //    rel_DOCTOR_id = doctor_id,
                //    attr_name = "rating",
                //    dname = "Rating",
                //    value = rate.ToString(),
                //    rel_ref_datatype_id = 3
                //};
                //db.DOCTOR_ext.Add(doc_ext);


                return Json(new { data = new string[] { }, message = "Doctor rating saved successfully.", success = true });
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }
        //[System.Web.Http.HttpPut]
        //[System.Web.Http.Route("doctor/rating")]
        //public async Task<IHttpActionResult> UpdateRating()
        //{
        //    string root = HttpContext.Current.Server.MapPath("~/Temp");
        //    var provider = new MultipartFormDataStreamProvider(root);

        //    long user_id = 0, doctor_id = 0;
        //    string review = "", appointment_id ="";
        //    int rate = 0;
        //    DateTime dtNow = DateTime.Now;

        //    try
        //    {
        //        await Request.Content.ReadAsMultipartAsync(provider);
        //        foreach (var key in provider.FormData.AllKeys)
        //        {
        //            foreach (var val in provider.FormData.GetValues(key))
        //            {
        //                // user_id, patient_id, rate
        //                bool bTemp = false;
        //                switch (key)
        //                {
        //                    case "user_id":
        //                        IsRequired(key, val, 1);
        //                        bTemp = long.TryParse(val, out user_id);
        //                        break;
        //                    case "doctor_id":
        //                        //IsRequired(key, val, 1);
        //                        bTemp = long.TryParse(val, out doctor_id);
        //                        break;

        //                    case "appointment_id":
        //                        IsRequired(key, val, 1);
        //                        appointment_id = val;
        //                        break;

        //                    case "review":
        //                        IsRequired(key, val, 1);
        //                        //bTemp = int.TryParse(val, out rate);
        //                        review = val;
        //                        break;

        //                    case "rate":
        //                        IsRequired(key, val, 1);
        //                        bTemp = int.TryParse(val, out rate);
        //                        break;

        //                    default:
        //                        return Json(new { message = "Invalid parameter name: " + key, success = false });
        //                }
        //            }
        //        }

        //       // IsRequired("doctor_id", doctor_id.ToString(), 3);
        //        IsRequired("appointment_id", appointment_id, 3);
        //        IsRequired("rate", rate.ToString(), 3);
        //        IsRequired("review", review.ToString(), 2);
        //        if (haserror)
        //        {
        //            return Json(new { data = new string[] { }, message = errmsg, success = false });
        //        }


        //        //var doc_ext = db.APPOINTMENT_ext.Where(a => a.rel_DOCTOR_id == doctor_id && a.attr_name == "rating");
        //        long appt_id_new = 0;
        //        bool bAppt = long.TryParse(appointment_id, out appt_id_new);
        //        var appt_ext = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == appt_id_new);

        //        if (appt_ext.Count() > 0)
        //        {
        //            bool hasRate = false, hasReview = false;
        //            foreach (var i in appt_ext)
        //            {
        //                if (i.attr_name == "doctor_rating")
        //                {
        //                    i.value = rate.ToString();
        //                    i.update_by__USER_id = user_id;
        //                    i.dt_update = dtNow;
        //                    db.Entry(i).State = System.Data.Entity.EntityState.Modified;
        //                    hasRate = true;
        //                }
        //                if (i.attr_name == "doctor_review")
        //                {
        //                    i.value = review.ToString();
        //                    i.update_by__USER_id = user_id;
        //                    i.dt_update = dtNow;
        //                    db.Entry(i).State = System.Data.Entity.EntityState.Modified;
        //                    hasReview = true;
        //                }

        //            }

        //            //long appt_id = appt_ext.FirstOrDefault().id;

        //            if (!string.IsNullOrEmpty(rate.ToString()) && !hasRate) {
        //                APPOINTMENT_ext doc_ext = new APPOINTMENT_ext()
        //                {
        //                    rel_APPOINTMENT_id = appt_id_new,
        //                    attr_name = "doctor_rating",
        //                    dname = "Doctor Rating",
        //                    value = rate.ToString(),
        //                    rel_ref_datatype_id = 3,
        //                    create_by__USER_id = user_id,
        //                    dt_create = dtNow
        //                };
        //                db.APPOINTMENT_ext.Add(doc_ext);
        //            }

        //            if (!string.IsNullOrEmpty(review) && !hasReview)
        //            {
        //                 APPOINTMENT_ext doc_rev = new APPOINTMENT_ext()
        //                 {
        //                     rel_APPOINTMENT_id = appt_id_new,
        //                     attr_name = "doctor_review",
        //                     dname = "Doctor Review",
        //                     value = review.ToString(),
        //                     rel_ref_datatype_id = 3,
        //                     create_by__USER_id = user_id,
        //                     dt_create = dtNow
        //                 };
        //                db.APPOINTMENT_ext.Add(doc_rev);
        //            }


        //            db.SaveChanges();

        //        }
        //        else
        //        {
        //            return Json(new { data="", message="No matching record found.", success=false});
        //        }


        //        //DOCTOR_ext doc_ext = new DOCTOR_ext()
        //        //{
        //        //    rel_DOCTOR_id = doctor_id,
        //        //    attr_name = "rating",
        //        //    dname = "Rating",
        //        //    value = rate.ToString(),
        //        //    rel_ref_datatype_id = 3
        //        //};
        //        //db.DOCTOR_ext.Add(doc_ext);


        //        return Json(new { data = new string[] { }, message = "Doctor rating saved successfully.", success = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { data = new string[] { }, message = ex.Message, success = false });
        //    }
        //}


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("doctor/rating0810")]
        public IHttpActionResult GetRate0810(long doctor_id = 0, long user_id = 0)
        {
            //string root = HttpContext.Current.Server.MapPath("~/Temp");
            //var provider = new MultipartFormDataStreamProvider(root);

            //long user_id = 0; //, doctor_id = 0;
            int rate = 0;
            // DateTime dtNow = DateTime.Now;

            //TODO: 2017-07-13 get by user_id
            try
            {
                List<long> apps = new List<long>();

                // get Doctor's ratings with user_id
                if (user_id > 0)
                {
                    var appt_id1 = db.APPOINTMENT_ext.Where(a => a.create_by__USER_id == user_id && a.attr_name == "doctor_rating");
                    if (appt_id1.Count() > 0)
                    {
                        foreach (var i in appt_id1)
                        {
                            apps.Add(i.rel_APPOINTMENT_id.Value);
                        }
                    }
                }

                // get Doctor's ratings with doctor_id
                if (doctor_id > 0)
                {
                    var appt_id1 = db.APPOINTMENT_ext.Where(a => a.attr_name == "doctor_id" && a.value == doctor_id.ToString());
                    foreach (var i in appt_id1)
                    {
                        apps.Add(i.rel_APPOINTMENT_id.Value);
                    }

                }
                else if (user_id == 0)
                {
                    var appt_id2 = db.APPOINTMENT_ext.Where(a => a.attr_name == "doctor_id");
                    foreach (var i in appt_id2)
                    {
                        apps.Add(i.rel_APPOINTMENT_id.Value);
                    }
                }


                var doc_ext = from a in db.APPOINTMENT_ext
                              where (apps.Contains(a.rel_APPOINTMENT_id.Value) && (a.attr_name == "doctor_rating")
                              || (apps.Contains(a.rel_APPOINTMENT_id.Value) && a.attr_name == "doctor_id")
                              || (apps.Contains(a.rel_APPOINTMENT_id.Value) && a.attr_name == "doctor_review")
                              )
                              orderby a.rel_APPOINTMENT_id ascending
                              select a;
                //select new {
                //    rate = new doc_rate{
                //      id = a.id,
                //      appointment_id =a.rel_APPOINTMENT_id.Value ,
                //      rating = a.value
                //   }
                //};

                //if (doctor_id > 0)
                //{
                //    doc_ext = doc_ext.Where(a => a.rate.doctor_id == doctor_id);
                //}
                doc_rate d = new doc_rate();
                List<doc_rate2> d1 = new List<doc_rate2>();
                string doc_val = "", rating_val = "", review_val = "";
                long app_id = 0, app_id2 = 0;
                int val = 0;

                if (doc_ext.Count() > 0)
                {
                    foreach (var i in doc_ext)
                    {
                        if (app_id2 == 0) app_id2 = i.rel_APPOINTMENT_id.Value;

                        //adfsadfasdf
                        if (val > 1 && app_id2 != i.rel_APPOINTMENT_id) //
                        {

                            if (!string.IsNullOrEmpty(rating_val) || !string.IsNullOrEmpty(review_val))
                            {


                                d = (new doc_rate
                                {
                                    //id = app_id,
                                    appointment_id = app_id2,
                                    rating = rating_val,
                                    doctor_id = doc_val,
                                    review = review_val
                                });
                                d1.Add(new doc_rate2 { rate = d });

                                val = 0;
                                rating_val = "";
                                review_val = "";
                                app_id2 = 0;
                            }
                        }
                        //adfsadfasdf

                        if (i.attr_name == "doctor_id")
                        {
                            doc_val = i.value; val++;
                        }

                        if (i.attr_name == "doctor_rating")
                        {
                            rating_val = i.value; val++;
                        }

                        if (i.attr_name == "doctor_review")
                        {
                            review_val = i.value; val++;
                        }


                        app_id2 = i.rel_APPOINTMENT_id.Value;

                    }

                    if (!string.IsNullOrEmpty(rating_val) || !string.IsNullOrEmpty(review_val))
                    {
                        d = (new doc_rate
                        {
                            //id = app_id,
                            appointment_id = app_id2,
                            rating = rating_val,
                            doctor_id = doc_val,
                            review = review_val
                        });
                        d1.Add(new doc_rate2 { rate = d });
                    }

                }


                //var ret = JsonConvert.SerializeObject(d1);
                //var json = Newtonsoft.Json.Linq.JArray.Parse( ret );
                //return Json(new { data = json, message = "", success = true });

                if (doc_ext.Count() > 0)
                {
                    var ret1 = JsonConvert.SerializeObject(d1);
                    var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);


                    return Json(new { data = json1, message = d1.Count() + " record(s) found.", success = true });
                }
                else
                {
                    return Json(new { data = new string[] { }, message = "No matching record found.", success = false });
                }


            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("doctor/rating")]
        public IHttpActionResult GetRate(long doctor_id = 0, long user_id = 0)
        {
            //string root = HttpContext.Current.Server.MapPath("~/Temp");
            //var provider = new MultipartFormDataStreamProvider(root);

            //long user_id = 0; //, doctor_id = 0;
            int rate = 0;
            // DateTime dtNow = DateTime.Now;

            //TODO: 2017-07-13 get by user_id
            try
            {
                List<long> apps = new List<long>();
                //var appt_id1 = from a in db.APPOINTMENTs select a;
                // get Doctor's ratings with user_id
                if (user_id > 0 && doctor_id ==0)
                {
                    //var appt_id1 = db.APPOINTMENT_ext.Where(a => a.create_by__USER_id == user_id && a.attr_name == "doctor_rating");
                    var appt_id1 = db.APPOINTMENT_ext.Where(a => a.create_by__USER_id == user_id && a.attr_name == "doctor_id");
                    //&& a.attr_name =="doctor_id" ); //&& a.attr_name == "doctor_rating");

                    //if (appt_id1.Count() > 0)
                    //{
                        foreach (var i in appt_id1)
                        {
                            apps.Add(i.rel_APPOINTMENT_id.Value);
                        }
                    //}
                }

                // get Doctor's ratings with doctor_id
                if (doctor_id > 0)
                {
                    //var appt_id1 = db.APPOINTMENT_ext.Where(a => a.attr_name == "doctor_id" && a.value == doctor_id.ToString());
                    var appt_id1 = db.APPOINTMENT_ext.Where(a => a.create_by__USER_id == user_id
                                             && a.attr_name == "doctor_id" && a.value == doctor_id.ToString()
                                                     );//

                    foreach (var i in appt_id1)
                    {
                        apps.Add(i.rel_APPOINTMENT_id.Value);
                    }

                }
                else if (user_id == 0)
                {
                    //var appt_id2 = db.APPOINTMENT_ext.Where(a => a.attr_name == "doctor_id");
                    //foreach (var i in appt_id2)
                    //{
                    //    apps.Add(i.rel_APPOINTMENT_id.Value);
                    //}
                }

              


                //2017/08/08 var appt_ext = from a in db.APPOINTMENT_ext
                //              where (apps.Contains(a.rel_APPOINTMENT_id.Value) && (a.attr_name == "doctor_rating")
                //              || (apps.Contains(a.rel_APPOINTMENT_id.Value) && a.attr_name == "doctor_id")
                //              || (apps.Contains(a.rel_APPOINTMENT_id.Value) && a.attr_name == "doctor_review")
                //              )
                //              orderby a.rel_APPOINTMENT_id ascending
                //              select a;

                var appt_main = from a in db.APPOINTMENTs where apps.Contains(a.id) select a;

                List<doc_rate> d = new List<doc_rate>();
                List<doc_rate2> d1 = new List<doc_rate2>();
                foreach (var n1 in appt_main)
                {
                    List<doctor_specialty> doc = new List<doctor_specialty>();
                    var appt_ext = from a in db.APPOINTMENT_ext
                                   where ( a.rel_APPOINTMENT_id==n1.id
                                            && (apps.Contains(a.rel_APPOINTMENT_id.Value) && (a.attr_name == "doctor_rating")
                                                      || (apps.Contains(a.rel_APPOINTMENT_id.Value) && a.attr_name == "doctor_id")
                                                      || (apps.Contains(a.rel_APPOINTMENT_id.Value) && a.attr_name == "doctor_review")
                                                      ))
                                   orderby a.rel_APPOINTMENT_id ascending
                                   select a;

                    int val = 0; string doc_val = "", rating_val = "", review_val = "";
                    long app_id = 0, app_id2 = 0;
                    foreach (var n1_ext in appt_ext) {
                      
                         
                        switch (n1_ext.attr_name)
                        { 
                            case "doctor_id":
                                doc_val = n1_ext.value; val++;
                                break;

                            case "doctor_rating":
                                rating_val = n1_ext.value; val++;
                                break;

                            case "doctor_review":
                                review_val = n1_ext.value; val++;
                                break;

                        }
                   }

                    if (!string.IsNullOrEmpty(rating_val) || !string.IsNullOrEmpty(review_val)) {
                        //doctor profile
                        if (!string.IsNullOrEmpty(doc_val))
                        {
                            long doc_id = Convert.ToInt64(doc_val);
                            var doc1 = db.hs_DOCTOR.Find(doc_id);
                            // get Doctor_ext
                            var doc_ext = db.DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doc1.id);
                            string vappt_name = "", vtime_slot = ""; long vappt_id = 0; double doc_fee = 0;
                            List<doc_specialty2> spec = new List<Models.doc_specialty2>();
                            List<appt_type> doc_appt = new List<appt_type>();

                            spec = custom.getSpecialty(doc1.id);
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
                                                var a_type = db.ref_APPOINTMENT_type.Find(vappt_id);
                                                vappt_name = a_type.dname;

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

                            //string addr2 = doc1.addr_address2 == null ? "" : doc1.addr_address2;
                            zip_search_address doc_address = new zip_search_address
                            {
                                address1 = doc1.home_addr_1 == null ? "" : doc1.home_addr_1,
                                address2 = doc1.home_addr_2 == null ? "" : doc1.home_addr_2,
                                zip = doc1.ref_zip.zip,
                                city = doc1.ref_zip.city_name,
                                state = doc1.ref_zip.city_state,
                                state_long = doc1.ref_zip.city_state_long,
                                county = doc1.ref_zip.city_county,
                                lat = doc1.ref_zip.city_lat,
                                lng = doc1.ref_zip.city_lon
                            };

                            doc.Add(new doctor_specialty
                            {
                                id = doc1.id,
                                first_name = doc1.name_first == null ? "" : doc1.name_first,
                                last_name = doc1.name_last == null ? "" : doc1.name_last,
                                middle_name = doc1.name_middle == null ? "" : doc1.name_middle,

                                doctor_fee = doc_fee,
                                bio = doc1.bio == null ? "" : doc1.bio,
                                email = doc1.email == null ? "" : doc1.email,
                                gender = doc1.gender == null ? "" : doc1.gender,
                                title = doc1.title == null ? "" : doc1.title,
                                phone = doc1.phone == null ? "" : doc1.phone,
                                license = doc1.license_no == null ? "" : doc1.license_no,
                                pecos_certificate = doc1.pecos_certification == null ? "" : doc1.pecos_certification,
                                npi = doc1.NPI == null ? "" : doc1.NPI,
                                organization_name = doc1.organization_name == null ? "" : doc1.organization_name,
                                image_url = doc1.image_url == null ? "" : doc1.image_url,

                                appointment_type = doc_appt,
                                address = doc_address,
                                specialties = spec
                            });


                        }

                        //eof: doctor profile
                        d.Add(new doc_rate
                        {
                            //id = app_id,
                            appointment_id = n1.id,
                            rating = rating_val,
                            doctor_id = doc_val,
                            doctor = doc,
                            review = review_val
                        });
                    }
                    rating_val = ""; review_val = "";
                  }
                // xxxxxxx

                if (appt_main.Count() > 0)
                {
                    var ret1 = JsonConvert.SerializeObject(d);
                    var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);


                    return Json(new { data = json1, message = d.Count() + " review(s) found.", success = true });
                }
                else
                {
                    return Json(new { data = new string[] { }, message = "No matching record found.", success = false });
                }


            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }


        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("doctor/rating")]
        public async Task<IHttpActionResult> DeleteRating()
        {
            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);

            long user_id = 0, doctor_id = 0;
            int rate = 0;
            DateTime dtNow = DateTime.Now;

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        // user_id, patient_id, rate
                        bool bTemp = false;
                        switch (key)
                        {
                            //case "user_id":
                            //    IsRequired(key, val, 1);
                            //    bTemp = long.TryParse(val, out user_id);
                            //    break;
                            case "doctor_id":
                                //IsRequired(key, val, 1);
                                bTemp = long.TryParse(val, out doctor_id);
                                break;

                            //case "rate":
                            //    //IsRequired(key, val, 1);
                            //    bTemp = int.TryParse(val, out rate);
                            //    break;

                            default:
                                return Json(new { message = "Invalid parameter name: " + key, success = false });
                        }
                    }
                }

                var doc_ext = db.DOCTOR_ext.Where(a => a.rel_DOCTOR_id==doctor_id &&  a.attr_name== "rating");

                if (doc_ext.Count()  > 0)
                {
                    foreach (var i in doc_ext) {
                        //doc_ext = doc_ext.Where(a => a.attr_name == "rating");
                        //db.Entry(doc_ext).State = System.Data.Entity.EntityState.Modified;
                        db.DOCTOR_ext.Remove(i);
                    }
                    
                    db.SaveChanges();
                }


                //db.DOCTOR_ext.Add(doc_ext);


                return Json(new { data = new string[] { }, message = "Doctor Rating was removed.", success = true });
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
            else if (i == 3)
            {
                int nVal = 0;
                bool bTemp = int.TryParse(val, out nVal);
                if (nVal == 0)
                {
                    haserror = true;
                    errmsg += "Missing parameter value for: " + key + ".\r\n";
                    return false;
                }
                return true;
            }
            else //if(i == 2)
            {
                if (string.IsNullOrEmpty(val))
                {
                    haserror = true;
                    errmsg += " Missing parameter " + key + ".\r\n";
                    return false;
                }
                return true;
            }

        }
    }

   
}
