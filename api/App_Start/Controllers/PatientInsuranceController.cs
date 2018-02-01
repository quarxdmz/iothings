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
    public class PatientInsuranceController : ApiController
    {
        SV_db1Entities db = new SV_db1Entities();

        [HttpGet]
        [Route("patient/insurance")]
        public IHttpActionResult get(long patient_id)
        {
            // user_id, patient_id
            long user_id = 0;

            if(patient_id > 0)
            {
                var results = from p in db.con_SOUL_ref_insurance
                              where p.rel_SOUL_id == patient_id
                              group p by p.rel_SOUL_id into g
                              select new
                              {
                                  patientId = g.Key,
                                  ins = g.ToList()
                              };

                if (user_id > 0)
                    results = results.Where(a => a.patientId == patient_id);


                long nTemp = 0;
                List<patient> pat = new List<patient>();

                if (results.Count() > 0)
                {
                    foreach (var li in results)
                    {
                        long patientId = li.patientId.Value;
                        string firstname = "";
                        string lastname = "";
                        List<con_SOUL_ref_insurance> lst = new List<con_SOUL_ref_insurance>();
                        List<_insurance> ins = new List<_insurance>();

                        foreach (var li2 in li.ins)
                        {


                            ins.Add(new _insurance
                            {
                                id = li2.ref_insurance_provider.PayerID.Split(',')[0].Trim(),
                                provider = li2.ref_insurance_provider.PayerName.Split('|')[0].Trim()
                            });
                            firstname = li2.SOUL.name_first;
                            lastname = li2.SOUL.name_last;
                        }


                        pat.Add(new patient
                        {
                            patient_id = patientId,
                            //firstname = firstname,
                            //lastname = lastname,
                            insurance = ins
                        });
                    }

                    var ret1 = JsonConvert.SerializeObject(pat);
                    var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                    return Json(new { data = json1, message = "Record found.", success = true });
                }

                return Json(new { data = "", message = "No matching record found.", success = false });
            }


            return Json(new { data = "", message = "Must supply required parameter.", success = false });
        }

        //Delete patient insurance 
        public IHttpActionResult Post(long patient_id, long insurance_id, long user_id)
        {
            //var pi = db.con_SOUL_ref_insurance.Where(a => a.rel_SOUL_id == patient_id && a.rel_ref_insurance_provider_id == insurance_id);
            con_SOUL_ref_insurance ref_ins = new con_SOUL_ref_insurance();

            // example
            // patient_id: 5,6,7,9,12,13
            // insurance_id:1,2,3,4,5,6,7,8,9,10

            DateTime dt = DateTime.Now;
            ref_ins.rel_SOUL_id = patient_id;
            ref_ins.rel_ref_insurance_provider_id = insurance_id;
            ref_ins.create_by__USER_id = user_id;
            ref_ins.dt_create = dt;
            db.con_SOUL_ref_insurance.Add(ref_ins);
            db.SaveChanges();
            // shud we add status in the con_SOUL_ref_insurance


            return Json(new { data ="", msg="Added patient insurance successfully.", success = true});
        }

        [HttpDelete]
        [Route("patient/insurance")]
        public IHttpActionResult Delete(long patient_id, long insurance_id, long user_id)
        {
            try {
                var pi = db.con_SOUL_ref_insurance.Where(a => a.rel_SOUL_id == patient_id && a.rel_ref_insurance_provider_id == insurance_id).FirstOrDefault();

                // shud we add status in the con_SOUL_ref_insurance
                db.con_SOUL_ref_insurance.Remove(pi);
                db.SaveChanges();

                string msg = "Successfully removed patient insurance.";
                return Json(new { data = "", message = msg, success = true });
            }
            catch (Exception ex) {
                return Json(new { data = "", message = ex.Message, success = false});
            }
            
        }
    }

    //public class patient {
    //    public long patient_id { get; set; }
    //    //public string firstname { get; set; }
    //    //public string lastname { get; set; }
    //    public List<_insurance> insurance { get; set; }
    // }

    //public class _insurance
    //{
    //    public string id { get; set; }
    //    public string provider { get; set; }
    //}
}
