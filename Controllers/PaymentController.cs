using api.Models;
using authorization.hs;
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

    public class PaymentController : ApiController
    {
        SV_db1Entities db = new SV_db1Entities();
        HSAuth authorized = new HSAuth();
        DateTime dt = DateTime.UtcNow;


        [System.Web.Http.HttpGet]
        //[Route("payment")]
        [Route("user/payment-history")]
        public IHttpActionResult get(long user_id)
        {
            //var results = from p in db.Card_Payment_History
            //              where p.User_Card_Info.user_id == user_id
            //              orderby p.payment_date
            //             // select p;
            //select new
            //{
            //    payment_history = new payment_History
            //    {
            //        amount = p.amount.ToString(),
            //        payment_date = p.payment_date.ToString()
            //    }
            //};

            var result = db.log_payment.Where(a => a.User_Card_Info.user_id == user_id).OrderBy(b => b.payment_date);
            
            List<payment_History> pay_H = new List<payment_History>();
            foreach (var r in result) {
                pay_H.Add(new payment_History {
                    amount = r.amount.Value,
                    payment_date = r.payment_date == null ? "" : r.payment_date.Value.GetDateTimeFormats()[56],
                });
            }

            //var y = new payment_History();
            //foreach (var x in results)
            //{
            //    y = new payment_History
            //    {
            //        amount = x.amount.ToString(),
            //        payment_date = x.payment_date.ToString()
            //    };
            //}

               var ret1 = JsonConvert.SerializeObject(pay_H);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
            if (pay_H.Count() > 0)
            {
                return Json(new { data = json1, message = pay_H.Count().ToString() + " record(s) found.", success = true });
            }
            else
            {
                return Json(new { data = new string[] { }, message = "No records found.", success = false});
            }
               
            }
    }
   
}
