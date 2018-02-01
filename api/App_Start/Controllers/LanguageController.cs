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
    public class LanguageController : ApiController
    {
        SV_db1Entities db = new SV_db1Entities();

        // api: language/{id}
       //[HttpGet]
       //[Route("doctor/language/{id}")]
       // public IHttpActionResult getDoctor_language1( int id)
       // {

       //     var doc_ext = db.hs_DOCTOR_ext.Where(a => a.attr_name =="language_spoken" && (a.value.Contains("," + id + ",") || a.attr_name.Contains("" + id + ",") || a.attr_name.Contains("," + id + "")));

       //     foreach () { }

       //     return Json(new { data =new string[] { }, message=id, success=true });
       // }

        //// api: language
        //[Route("doctor/language")]
        //[HttpGet]
        //public IHttpActionResult getDoctor_language()
        //{

        //    List<res_language> lang = new List<res_language>();
        //    var ref_ = from a in db.ref_languages select a;

        //    foreach (var m in ref_)
        //    {
        //        lang.Add(new res_language { id = m.id, name = m.name });
        //    }


        //    var ret1 = JsonConvert.SerializeObject(lang);
        //    var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

        //    return Json(new { data = json1, message = "", success = true });
        //    //return Request.CreateResponse(HttpStatusCode.BadRequest, json1);
        //}

    }

    public class doc_language
    {
        public long id { get; set; }
        public string name { get; set; }
    }

    public class doc_state
    {
        public string state { get; set; }
        public string state_long { get; set; }
        public List<doc_state_cities> city { get; set; }
    }

    public class doc_state_cities
    {
        public string name { get; set; }
    }

    public class doc_hospital
    {
        public long id { get; set; }
        public string name { get; set; }
    }
}
