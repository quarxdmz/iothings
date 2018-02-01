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
    public class RatePatientController : ApiController
    {

        SV_db1Entities db = new SV_db1Entities();

        [HttpPost]
        [Route("patient/ra1te")]
        public async Task<IHttpActionResult> New1Rating()
        {
            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);

            long user_id = 0;
            string rate = "", patient_id = "", appointment_id="";
            DateTime dtNow = DateTime.Now;

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        // user_id, patient_id, rate
                        bool bTemp1 = false, bTemp3 =false;
                        switch (key)
                        {
                            case "user_id":
                                IsRequired(key, val, 1);
                                bTemp1 = long.TryParse(val, out user_id);
                                break;
                            case "patient_id":
                                IsRequired(key, val, 1);
                                //bTemp = long.TryParse(val, out patient_id);
                                patient_id = val;
                                break;

                            case "rate":
                                IsRequired(key, val, 1);
                                //bTemp3 = int.TryParse(val, out rate);
                                rate = val;
                                break;

                            //case "appointment_id":
                            //    appointment_id = val;

                            //    break;
                            default:
                                return Json(new { message = "Invalid parameter name: " + key, success = false });
                        }
                    }
                }

                IsRequired("user_id", user_id.ToString(), 2);
                IsRequired("patient_id", patient_id.ToString(), 3);
               // IsRequired("appointment_id", appointment_id, 3);
                IsRequired("rate", rate.ToString(), 3);
                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }

                long patient;
                bool bTemp = long.TryParse(patient_id, out patient);

                var ext_rating = db.SOUL_ext.Where(a => a.rel_SOUL_id == patient && a.attr_name == "rating").FirstOrDefault();
                if (ext_rating != null)
                {
                    db.SOUL_ext.Remove(ext_rating);
                    db.SaveChanges();
                }

                SOUL_ext pat_ext = new SOUL_ext()
                {
                    rel_SOUL_id = patient,
                    attr_name = "rating",
                    dname = "Rating",
                    value = rate.ToString(),
                    rel_ref_datatype_id = 3,
                    create_by__USER_id = user_id,
                    dt_create = dtNow
                };
                db.SOUL_ext.Add(pat_ext);
                db.SaveChanges();

                return Json(new { data = new string[] { }, message = "New rating saved successfully.", success = true });
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }


        [HttpPost]
        [Route("patient/rating")]
        public async Task<IHttpActionResult> NewRating()
        {
            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);

            long user_id = 0;
            string rate = "", patient_id = "", appointment_id = "";
            DateTime dtNow = DateTime.Now;

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        // user_id, patient_id, rate
                        bool bTemp1 = false, bTemp3 = false;
                        switch (key)
                        {
                            case "user_id":
                                IsRequired(key, val, 1);
                                bTemp1 = long.TryParse(val, out user_id);
                                break;
                            case "patient_id":
                                IsRequired(key, val, 1);
                                //bTemp = long.TryParse(val, out patient_id);
                                patient_id = val;
                                break;

                            case "rate":
                                IsRequired(key, val, 1);
                                //bTemp3 = int.TryParse(val, out rate);
                                rate = val;
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
                //IsRequired("patient_id", patient_id.ToString(), 3);
                IsRequired("appointment_id", appointment_id, 3);
                IsRequired("rate", rate.ToString(), 3);
                if (haserror)
                {
                    return Json(new { data = new string[] { }, message = errmsg, success = false });
                }

                long appt;
                bool bTemp = long.TryParse(appointment_id, out appt);

                var ext_rating = db.APPOINTMENT_ext.Where(a => a.rel_APPOINTMENT_id == appt && a.attr_name == "patient_rating").FirstOrDefault();
                if (ext_rating != null)
                {
                    db.APPOINTMENT_ext.Remove(ext_rating);
                    db.SaveChanges();
               }

                APPOINTMENT_ext pat_ext = new APPOINTMENT_ext()
                {
                    rel_APPOINTMENT_id = appt,
                    attr_name = "patient_rating",
                    dname = "Patient Rating",
                    value = rate.ToString(),
                    rel_ref_datatype_id = 3,
                    create_by__USER_id = user_id,
                    dt_create = dtNow
                };
                db.APPOINTMENT_ext.Add(pat_ext);
                db.SaveChanges();


                //[{"id":26,"patient_id":38,"appointment_id":16,"rating":"7"}]
                //var json = Newtonsoft.Json.Linq.JArray.Parse("[{\"appointment_id\" :" + appointment_id + "}]");
                return Json(new { data = new string[] { }, message = "New Patient rating saved successfully.", success = true });


            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }


        [HttpPut]
        [Route("patient/rating")]
        public async Task<IHttpActionResult> UpdateRating()
        {
            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);

            long user_id = 0, patient_id = 0;
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
                            case "user_id":
                                IsRequired(key, val, 1);
                                bTemp = long.TryParse(val, out user_id);
                                break;
                            case "patient_id":
                                IsRequired(key, val, 1);
                                bTemp = long.TryParse(val, out patient_id);
                                break;

                            case "rate":
                                IsRequired(key, val, 1);
                                bTemp = int.TryParse(val, out rate);
                                break;

                            default:
                                return Json(new { message = "Invalid parameter name: " + key, success = false });
                        }
                    }
                }

                var pat_ext = db.SOUL_ext.Where(a => a.rel_SOUL_id == patient_id && a.attr_name == "rating");

                if (pat_ext.Count() > 0)
                {
                    foreach (var i in pat_ext)
                    {
                        if (i.attr_name == "rating")
                        {
                            i.value = rate.ToString();
                            i.update_by__USER_id = user_id;
                            i.dt_update = dtNow;
                            db.Entry(i).State = System.Data.Entity.EntityState.Modified;
                        }


                    }

                    db.SaveChanges();

                }
                else
                {
                    return Json(new { data = new string[] { }, message = "No matching record found." });
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


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("patient/ra1ting")]
        public IHttpActionResult Get1Rate(long appointment_id, long patient_id=0)
        {
            //string root = HttpContext.Current.Server.MapPath("~/Temp");
            //var provider = new MultipartFormDataStreamProvider(root);

            long user_id = 0; //, doctor_id = 0;
            int rate = 0;
            // DateTime dtNow = DateTime.Now;

            try
            {
                var patient_ext = from a in db.APPOINTMENTs
                                      //where a.attr_name == "rating"
                                      join x in db.APPOINTMENT_ext on a.id equals x.rel_APPOINTMENT_id into g
                                  where a.id == appointment_id //&&  a.APPOINTMENT_ext.attr_name == "patient_rating"
                                  select new
                                  {
                                      rate = new pat_rate
                                      {
                                          //patient_id = a.re.Value,
                                          id = g.Where(v => v.attr_name == "patient_rating").Select(v => v.id).FirstOrDefault(),
                                          patient_id = a.rel_SOUL_id.Value,
                                          appointment_id = a.id,
                                          //rating = a.APPOINTMENT_ext.FirstOrDefault().value
                                          rating = g.Where(v => v.attr_name == "patient_rating").Select(v => v.value).FirstOrDefault()
                                      }
                                  };

                if (patient_id > 0)
                {
                    patient_ext = patient_ext.Where(a => a.rate.patient_id == patient_id);
                }

                if (patient_ext.Count() > 0)
                {
                    List<pat_rate> pat_rate = new List<pat_rate>();
                    //foreach (var i in patient_ext)
                    //{
                    //    pat_rate.Add(
                    //        new pat_rate
                    //        {
                    //            patient_id = i.rel_SOUL_id.Value,
                    //            firstname = i.SOUL.name_first,
                    //            lastname = i.SOUL.name_last,
                    //            rating = i.value
                    //        });
                    //}

                    var ret1 = JsonConvert.SerializeObject(patient_ext);
                    var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);


                    return Json(new { data = json1, message = "", success = true });
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
        [System.Web.Http.Route("patient/rating")]
        public IHttpActionResult GetRate(long patient_id =0)
        {
            //string root = HttpContext.Current.Server.MapPath("~/Temp");
            //var provider = new MultipartFormDataStreamProvider(root);

            long user_id = 0; //, doctor_id = 0;
            int rate = 0;
            // DateTime dtNow = DateTime.Now;

            try
            {

                List<long> appt_id = new List<long>();
                if (patient_id > 0)
                {
                    var appt = db.APPOINTMENTs.Where(a => a.rel_SOUL_id == patient_id);
                    foreach (var i in appt)
                    {
                        appt_id.Add(i.id);
                    }
                }
                else
                {
                    var appt = from a in db.APPOINTMENTs select a;
                    foreach (var i in appt)
                    {
                        appt_id.Add(i.id);
                    }
                }

                var patient_ext = from a in db.APPOINTMENT_ext
                                  where appt_id.Contains(a.rel_APPOINTMENT_id.Value) && a.attr_name == "patient_rating"
                                  select new {
                                      rate = new pat_rate {
                                          id = a.id,
                                          appointment_id = a.rel_APPOINTMENT_id.Value,
                                          patient_id = a.APPOINTMENT.rel_SOUL_id.Value,
                                          rating = a.value
                                      }
                                  };
             

                if (patient_ext.Count() > 0)
                {
                    var ret1 = JsonConvert.SerializeObject(patient_ext);
                    var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);


                    return Json(new { data = json1, message = "", success = true });
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
        [System.Web.Http.Route("patient/get1rate")]
        public IHttpActionResult GetRate1(long patient_id = 0)
        {
            //string root = HttpContext.Current.Server.MapPath("~/Temp");
            //var provider = new MultipartFormDataStreamProvider(root);

            long user_id = 0; //, doctor_id = 0;
            int rate = 0;
            // DateTime dtNow = DateTime.Now;

            try
            {
                var patient_ext = from a in db.SOUL_ext
                                  where a.attr_name == "rating"
                                  select new {
                                      rate = new pat_rate2 {
                                          patient_id = a.rel_SOUL_id.Value,
                                          rating = a.value
                                        
                                      }
                                       };

                if (patient_id > 0)
                {
                    patient_ext = patient_ext.Where(a => a.rate.patient_id  == patient_id);
                }

                if (patient_ext.Count() > 0)
                {
                    //List<pat_rate> pat_rate1 = new List<pat_rate>();
                    //foreach (var i in patient_ext)
                    //{
                    //    pat_rate1.Add(
                    //        new pat_rate
                    //        {
                    //            patient_id = i.rel_SOUL_id.Value,
                    //            firstname = i.SOUL.name_first,
                    //            lastname = i.SOUL.name_last,
                    //            rating = i.value
                    //        });
                    //}

                   


                    var ret1 = JsonConvert.SerializeObject(patient_ext);
                   

                    var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);


                    return Json(new { data = json1, message = "", success = true });
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

        [HttpDelete]
        [Route("patient/rating")]
        public async Task<IHttpActionResult> DeleteRating()
        {
            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);

            long user_id = 0, patient_id = 0;
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
                            case "patient_id":
                                //IsRequired(key, val, 1);
                                bTemp = long.TryParse(val, out patient_id);
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

                var pat_ext = db.SOUL_ext.Where(a => a.rel_SOUL_id == patient_id && a.attr_name == "rating");

                if (pat_ext.Count() > 0)
                {
                    foreach (var i in pat_ext)
                    {
                        //doc_ext = doc_ext.Where(a => a.attr_name == "rating");
                        //db.Entry(doc_ext).State = System.Data.Entity.EntityState.Modified;
                        db.SOUL_ext.Remove(i);
                    }

                    db.SaveChanges();
                }


                //db.DOCTOR_ext.Add(doc_ext);


                return Json(new { data = new string[] { }, message = "Patient Rating was removed.", success = true });
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
                if (!bTemp)
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
                    errmsg += "Missing parameter " + key + ".\r\n";
                    return false;
                }
                return true;
            }
            

        }
    }

   
}
