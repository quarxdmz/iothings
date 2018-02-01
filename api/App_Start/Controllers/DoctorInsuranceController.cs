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
    public class DoctorInsuranceController : ApiController
    {

        public IHttpActionResult Post(long doctorId, int insuranceId)
        {

            ////if (progAuth.authorize())
            ////{
            //    //mainEntities db = new mainEntities();

            //    //var spec = db.ref_specialties.Where(a => a.name.ToLower() == specialty.ToLower() || a.description.ToLower() == specialty.ToLower()).FirstOrDefault();

            //    //Doc_Specialties_Profile doc_spec = new Doc_Specialties_Profile();
            //    var doc_ins = db.Doc_Insurance_Affiliation.Where(a => a.doctor_id == doctorId && a.insurance_id == insuranceId).FirstOrDefault();
            //    if (doc_ins != null)
            //    {

            //        doc_ins.doctor_id = doctorId;
            //        doc_ins.insurance_id = insuranceId;
            //        doc_ins.active = 1;
            //        db.Doc_Insurance_Affiliation.Add(doc_ins);
            //        db.SaveChanges();


            //        //List<specPerDoctor> docSpec = new List<specPerDoctor>();
            //        //docSpec.Add(new specPerDoctor
            //        //{
            //        //    id = doc_spec.id,
            //        //    docId = doctorId,
            //        //    specialty_id = spec.id,
            //        //    specialty_name = spec.name,
            //        //    specialty_description = spec.description
            //        //});

            //        //var ret = JsonConvert.SerializeObject(docSpec);
            //        //var json = Newtonsoft.Json.Linq.JArray.Parse(ret);
            //        string msg = "Specialty for the doctor is added.";
            //        return Json(new { data = "", message = msg, success = true });
            //    }

            //    string msg1 = "The Specialty is not found in the Specialty reference table.";
            //    return Json(new { data = "", message = msg1, success = true });
            ////}
            ////else
            ////{
            ////    string msg = "The authorization header is either not valid or isn't Basic.";
                return Json(new { data = "", message = "", success = true });

            ////    //throw new Exception("The authorization header is either not valid or isn't Basic.");
            ////}

        }

        public IHttpActionResult Get(long doctorId = 0, string doctorName = "", string specialtyName = "")
        {
            if (true)//progAuth.authorize()
            {
               
                ////from a in dbEntity.DOCTORs select a;
                //var specialty = db.Doc_Specialties_Profile.Where(a => a.doctor_id == doctorId);

                //List<specPerDoctor> doctorSpec = new List<specPerDoctor>();
                //foreach (var i in specialty)
                //{
                //    doctorSpec.Add(new specPerDoctor
                //    {
                //        id = i.id,
                //        docId = i.doctor_id.Value,
                //        specialty_id = i.specialty_id.Value,
                //        specialty_name = i.ref_specialties.name,
                //        specialty_description = i.ref_specialties.name
                //    });
                //}

                //var ret = JsonConvert.SerializeObject(doctorSpec);
                //var json = Newtonsoft.Json.Linq.JArray.Parse(ret);
                string msg = "Specialty for the doctor is added";
                return Json(new { data = "", message = msg, success = true });

            }
            else
            {
                string msg = "The authorization header is either not valid or isn't Basic.";
                return Json(new { data = "", message = msg, success = true });
                //throw new Exception("The authorization header is either not valid or isn't Basic.");
            }

        }

        //updates records
        public IHttpActionResult Put(long doctorId, int insuranceId)
        {
            // i think this is not necessary

            //mainEntities db = new mainEntities();

            ////Doc_Specialties_Profile doc_spec = new Doc_Specialties_Profile();
            //var doc_spec = db.Doc_Insurance_Affiliation.Where(a => a.doctor_id == doctorId && a.insurance_id == insuranceId).FirstOrDefault();
            //if (doc_spec != null)
            //{
            //    doc_spec.doctor_id = doctorId;
            //    doc_spec.insurance_id = insuranceId;
            //    //db.Doc_Specialties_Profile.Add(doc_spec);
            //    db.Entry(doc_spec).State = System.Data.Entity.EntityState.Modified;
            //    db.SaveChanges();


            //   //var ret = JsonConvert.SerializeObject(docSpec);
            //    //var json = Newtonsoft.Json.Linq.JArray.Parse(ret);
            //    string msg = "Specialty for the following doctor is updated: " + doc_spec.DOCTOR.namefirst + " " + doc_spec.DOCTOR.namelast;
            //    return Json(new { data = "", message = msg, success = true });
            //}

            string msg1 = "Update Insurance affiliations successfully.";
            return Json(new { data = "", message = msg1, success = true });
        }

        public IHttpActionResult Delete(long doctorId, int insuranceId)
        {

            //mainEntities db = new mainEntities();

            //var ref_spec = db.Doc_Specialties_Profile

            //var doc_ins = db.Doc_Insurance_Affiliation.Where(a => a.doctor_id == doctorId && a.insurance_id == insuranceId).FirstOrDefault();
            //if (doc_ins != null)
            //{
            //    doc_ins.active = 0;
            //    db.Entry(doc_ins).State = System.Data.Entity.EntityState.Modified;
            //    db.SaveChanges();
            //}

            string msg1 = "The Specialty is not found in the Specialty reference table.";
            return Json(new { data = "", message = msg1, success = true });

        }
    }
}

