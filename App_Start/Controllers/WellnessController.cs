using api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    public class WellnessController : ApiController
    {
     
        Models.SV_db1Entities db = new Models.SV_db1Entities();

        //get wellness exam - 
        //parameter: @user_id
        //response: exam_id, exam_title,
        [System.Web.Http.Route("user/wellness-exam")]
        [HttpGet]
        public IHttpActionResult getwellnessexam(string user_id = null)
        {

            try
            {
                var _userWellness = from a in db.ref_wellness_exam select a;

                List<wellness_exam1> wellness = new List<wellness_exam1>();
                foreach (var n in _userWellness)
                {
                    wellness.Add(new wellness_exam1 {
                        exam_id= n.id,
                        exam_title = n.dname
                    });
                }

                if (wellness.Count() > 0)
                {
                    var ret1 = JsonConvert.SerializeObject(wellness);
                    var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                    //msg = "The patient is updated.";
                    return Json(new { data = json1, message = wellness.Count() + (wellness.Count() > 1 ? " Records found!" : " Record found!"), success = true });
                }

                return Json(new { data = new string[] { }, message = "No record found.", success = false });
            }
            catch (Exception ex) {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }

        [System.Web.Http.Route("user/exam-taken")]
        [HttpGet]
        public IHttpActionResult getexamtaken(string user_id = null)
        {

            long user_id_new = 0;
            try
            {
                bool bTemp = long.TryParse(user_id, out user_id_new);
                //var _userWellness = from a in db.ref_wellness_exam select a;

                if (bTemp)
                {
                    var _userWellness = db.wellness_exam.Where(a => a.create_by__USER == user_id_new).OrderBy(b => b.rel_SOUL_id);

                    //var _userWellness2 = from a in db.wellness_exam
                    //                     where a.create_by__USER == user_id_new
                    //                     select new{
                    //                            exam_taken = new exam_taken{
                    //                                   patient_id =a.rel_SOUL_id,
                    //                                   wellness = new wellness_exam2 {
                    //                                       exam_id =a.rel_exam_id,
                    //                                       exam_title = a.ref_wellness_exam.dname
                    //                                   }
                    //                                 }
                    //                        };


                    List < exam_taken > taken = new List<exam_taken>();
                    List<wellness_exam2> exam = new List<wellness_exam2>();
                    long pat_id = 0, pat_id2 = 0;
                    long _exam_id = 0;
                    string _exam_title = "";
                    foreach (var n in _userWellness)
                    {
                        if(pat_id2 ==0) pat_id2 = n.rel_SOUL_id;

                        if (pat_id2 != n.rel_SOUL_id)
                        {
                            //if (pat_id2 != pat_id)
                            //{
                            taken.Add(new exam_taken
                            {
                                patient_id = n.rel_SOUL_id,
                                wellness = exam
                            });

                            pat_id2 = 0;
                            exam = new List<wellness_exam2>();
                            //}
                        }
                        exam.Add(new wellness_exam2
                        {
                            exam_id = n.rel_exam_id,
                            exam_title = n.ref_wellness_exam.dname
                        });

                     
                        pat_id = n.rel_SOUL_id;
                        _exam_title = "";
                        _exam_id = 0;

                      
                        //pat_id = n.rel_SOUL_id;
                    }

                    taken.Add(new exam_taken
                    {
                        patient_id = pat_id,
                        wellness = exam
                    });


                    //if (pat_id2 == pat_id)
                    //{

                    //    taken.Add(new exam_taken
                    //    {
                    //        patient_id = pat_id,
                    //        wellness = exam
                    //    });

                    //    //pat_id2 = n.rel_SOUL_id;
                    //}
                    //else
                    //{
                    //    //exam.Add(new wellness_exam2
                    //    //{
                    //    //    exam_id = _exam_id,
                    //    //    exam_title = _exam_title
                    //    //});

                    //    taken.Add(new exam_taken {
                    //        patient_id = pat_id,
                    //        wellness = exam 
                    //    });
                    //}



                    if (taken.Count() > 0)
                    {
                        var ret1 = JsonConvert.SerializeObject(taken);
                        var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                        //msg = "The patient is updated.";
                        return Json(new { data = json1, message = taken.Count() + (taken.Count() > 1 ? " Records found!" : " Record found!"), success = true });
                    }
                }
               

                return Json(new { data = new string[] { }, message = "No record found.", success = false });
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }

    }

   
}
