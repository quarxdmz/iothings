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
    public class DoctorEducationController : ApiController
    {
        SV_db1Entities dbEntity = new SV_db1Entities();
        public IHttpActionResult Post(long doctorId, string school="", string degree ="", string graduation_year="")
        {

            if (true)//progAuth.authorize()
            {
              
                   //Doc_Education doc_edu = new Doc_Education();
              
                   // doc_edu.doctor_id = doctorId;
                   // doc_edu.school = school;
                   // doc_edu.degree = degree;
                   // doc_edu.graduation_year = graduation_year;
                   // doc_edu.active = 1;
                   // db.Doc_Education.Add(doc_edu);
                   // db.SaveChanges();


                    //List<specPerDoctor> docSpec = new List<specPerDoctor>();
                    //docSpec.Add(new specPerDoctor
                    //{
                    //    id = doc_spec.id,
                    //    docId = doctorId,
                    //    specialty_id = spec.id,
                    //    specialty_name = spec.name,
                    //    specialty_description = spec.description
                    //});

                    //var ret = JsonConvert.SerializeObject(docSpec);
                    //var json = Newtonsoft.Json.Linq.JArray.Parse(ret);
                    string msg = "Educational background for the doctor is added.";
                    return Json(new { data = "", message = msg, success = true });
                

          }
            else
            {
                string msg = "The authorization header is either not valid or isn't Basic.";
                return Json(new { data = "", message = msg, success = false });

                //throw new Exception("The authorization header is either not valid or isn't Basic.");
            }

        }

        public IHttpActionResult Get(long doctorId, string school = "", string degree = "", string graduation_year = "")
        {
            if (true)//progAuth.authorize()
            {
                 //from a in dbEntity.DOCTORs select a;
                //var educ = db.Doc_Education.Where(a => a.doctor_id == doctorId);

                //if (!string.IsNullOrEmpty(school)) {
                //    educ = educ.Where(a => a.school.ToLower().Contains(school.ToLower()));
                //}

                //if (!string.IsNullOrEmpty(degree))
                //{
                //    educ = educ.Where(a => a.degree.ToLower().Contains(degree.ToLower()));
                //}

                //if (!string.IsNullOrEmpty(graduation_year))
                //{
                //    educ = educ.Where(a => a.graduation_year.Contains(graduation_year));
                //}

                //List<educPerDoctor> doctorSpec = new List<educPerDoctor>();
                //foreach (var i in educ)
                //{
                //    doctorSpec.Add(new educPerDoctor
                //    {
                //        id = i.id,
                //         docId = i.doctor_id.Value,
                //         shool = i.school,
                //         degree = i.degree,
                //         graduated_year = i.graduation_year
                //    });
                //}

                //var ret = JsonConvert.SerializeObject(doctorSpec);
                //var json = Newtonsoft.Json.Linq.JArray.Parse(ret);
                string msg = "Retrieving education background for doctor";
                return Json(new { data = "", message = msg, success = true });

            }
            else
            {
                string msg = "The authorization header is either not valid or isn't Basic.";
                return Json(new { data = "", message = msg, success = false });
                //throw new Exception("The authorization header is either not valid or isn't Basic.");
            }

        }

        public IHttpActionResult Put(long doctorId, string school = "", string degree = "", string graduation_year = "")
        {

          
            //var educ = db.Doc_Education.Where( a => a.doctor_id ==doctorId);

            //if (!string.IsNullOrEmpty(school))
            //{
            //    educ = educ.Where(a => a.school.ToLower().Contains(school.ToLower()));
            //}

            //if (!string.IsNullOrEmpty(degree))
            //{
            //    educ = educ.Where(a => a.degree.ToLower().Contains(degree.ToLower()));
            //}

            //if (!string.IsNullOrEmpty(graduation_year))
            //{
            //    educ = educ.Where(a => a.graduation_year.Contains(graduation_year));
            //}

            //if (educ == null)
            //{
            //    string msg = "Cannot find matching record.";
            //    return Json(new { data = "", message = msg, success = false });
            //}

            //if (educ.Count() > 1)
            //{
            //    string msg = "Multiple instances have been found.";
            //    return Json(new { data = "", message = msg, success = false });
            //}


            //if (educ != null)
            //{

              
            //    educ.FirstOrDefault().doctor_id = doctorId;
            //    educ.FirstOrDefault().school = school;
            //    educ.FirstOrDefault().degree = degree;
            //    educ.FirstOrDefault().graduation_year = graduation_year;
            //    //db.Doc_Specialties_Profile.Add(doc_spec);
            //    db.Entry(educ).State = System.Data.Entity.EntityState.Modified;
            //    db.SaveChanges();

                //var ret = JsonConvert.SerializeObject(docSpec);
                //var json = Newtonsoft.Json.Linq.JArray.Parse(ret);
                string msg = "Education background have been updated.";
                return Json(new { data = "", message = msg, success = true });
            //}

           // string msg1 = "Updating Specialty related to the doctor can be done through the DELETE and POST method.";
           // return Json(new { data = "", message = msg1, success = true });
        }

        public IHttpActionResult Delete(long doctorId, string school = "", string degree = "", string graduation_year = "")
        {

            //mainEntities db = new mainEntities();

            //var educ = db.Doc_Education.Where(a => a.doctor_id == doctorId);

            //if (!string.IsNullOrEmpty(school))
            //{
            //    educ = educ.Where(a => a.school.ToLower().Contains(school.ToLower()));
            //}

            //if (!string.IsNullOrEmpty(degree))
            //{
            //    educ = educ.Where(a => a.degree.ToLower().Contains(degree.ToLower()));
            //}

            //if (!string.IsNullOrEmpty(graduation_year))
            //{
            //    educ = educ.Where(a => a.graduation_year.Contains(graduation_year));
            //}

            //if (educ == null)
            //{
            //    string msg = "Cannot find matching record.";
            //    return Json(new { data = "", message = msg, success = false });
            //}

            //if (educ.Count() > 1)
            //{
            //    string msg = "Multiple instances have been found.";
            //    return Json(new { data = "", message = msg, success = false });
            //}


            //if (educ != null)
            //{


            //    //educ.FirstOrDefault().doctor_id = doctorId;
            //    //educ.FirstOrDefault().school = school;
            //    //educ.FirstOrDefault().degree = degree;
            //    //educ.FirstOrDefault().graduation_year = graduation_year;
            //    educ.FirstOrDefault().active = 0;
            //    //db.Doc_Specialties_Profile.Add(doc_spec);
            //    db.Entry(educ).State = System.Data.Entity.EntityState.Modified;
            //    db.SaveChanges();

            //    //var ret = JsonConvert.SerializeObject(docSpec);
            //    //var json = Newtonsoft.Json.Linq.JArray.Parse(ret);
            //    string msg = "Education background have been updated.";
            //    return Json(new { data = "", message = msg, success = true });
            //}

            string msg1 = "The Specialty is not found in the Specialty reference table.";
            return Json(new { data = "", message = "", success = true });
        }
        }
    }

    public class educPerDoctor
    {
        public long id { get; set; }
        public long docId { get; set; }
        //public string firstname { get; set; }
        //public string lastname { get; set; }
        //public string middlename { get; set; }
        public string degree { get; set; }
        public string shool { get; set; }
        public string graduated_year { get; set; }
    }

