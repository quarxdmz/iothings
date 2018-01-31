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
    public class InsuranceController : ApiController
    {

        [HttpGet]
        [Route("insurance")]
        //    resultsTextBox.Text += "Working . . . . . . .\r\n";
        //}
        public IHttpActionResult Getinsurance(long insurance_id = 0)
        {
            System.Web.HttpContext httpCOn = System.Web.HttpContext.Current;

            string a_pass = httpCOn.Request.Headers["Authorization"];
            bool b_pass = Validation.userAuth(a_pass);

            // Basic dXNlcjp1c2Vy
            // Basic dXNlcjp1c2Vy
            // "Basic ZGVmdHNvZnQ6ZGVmdHNvZnRhcGlrZXk="
            string msg = "The authorization header is not valid.";

            if (b_pass)
            {
                SV_db1Entities db = new SV_db1Entities();

                var ref_ins = from a in db.ref_insurance_provider select a;

                if (insurance_id > 0)
                {
                    ref_ins = ref_ins.Where(a => a.id == insurance_id);
                }

                List<insurance> ins = new List<insurance>();
                foreach (var i in ref_ins)
                {
                    ins.Add(
                        new insurance
                        {
                            id = i.id, //i.PayerID.Split(',')[0],
                            provider = i.PayerName.Split('|')[0].Trim()
                        });
                }

                var ret1 = JsonConvert.SerializeObject(ins);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                return Json(new { data = json1, message = "Record found.", success = true });
            }
         
            return Json(new { data = "", message = msg, success = false });
        }

        [HttpGet]
        [Route("insurancexxx")]
        public IHttpActionResult Get1()
        {
            SV_db1Entities db = new SV_db1Entities();
            string insurance_id = "";
            var ref_ins = from a in db.ref_insurance_provider
                          where a.PayerID.Contains(insurance_id)
                          select a;

            ref_ins = ref_ins.OrderBy(a => a.PayerName);

            List<insurance> ins = new List<insurance>();
            foreach (var i in ref_ins)
            {
                ins.Add(
                    new insurance
                    {
                        id =  i.id, //i.PayerID.Split(',')[0],
                        provider = i.PayerName.Split('|')[0].Trim()
                    });
            }


            var ret1 = JsonConvert.SerializeObject(ins);
            var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
            return Json(new { data = json1, message = "", success = true });

        }

     
    }

 
}
