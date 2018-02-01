using api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using static api.Controllers.AppointmentController;
//C:\Users\glyni\OneDrive\Documents\Visual Studio 2015\Projects\api\api\Controllers\AppointmentController.cs

namespace api.Controllers
{
    public class SpecialtyController : ApiController
    {
        SV_db1Entities dbEntity = new SV_db1Entities();
        //public IHttpActionResult Post(long doctorId, long specialtyId)
        //{

        //    if (true)//progAuth.authorize()
        //    {
        //        ////var spec = db.ref_specialties.Where(a => a.name.ToLower() == specialty.ToLower() || a.description.ToLower() == specialty.ToLower()).FirstOrDefault();
        //        ////Doc_Specialties_Profile doc_spec = new Doc_Specialties_Profile();
        //        //var doc_spec = dbEntity.Doc_Specialties_Profile.Where(a => a.doctor_id == doctorId &&  a.specialty_id == specialtyId).FirstOrDefault();
        //        //if (doc_spec != null)
        //        //{
        //        //    doc_spec.doctor_id = doctorId;
        //        //    doc_spec.specialty_id = specialtyId;
        //        //    db.Doc_Specialties_Profile.Add(doc_spec);
        //        //    db.SaveChanges();
        //        //    //List<specPerDoctor> docSpec = new List<specPerDoctor>();
        //        //    //docSpec.Add(new specPerDoctor
        //        //    //{
        //        //    //    id = doc_spec.id,
        //        //    //    docId = doctorId,
        //        //    //    specialty_id = spec.id,
        //        //    //    specialty_name = spec.name,
        //        //    //    specialty_description = spec.description
        //        //    //});
        //        //    //var ret = JsonConvert.SerializeObject(docSpec);
        //        //    //var json = Newtonsoft.Json.Linq.JArray.Parse(ret);
        //        //    string msg = "Specialty for the doctor is added.";
        //        //    return Json(new { data = "", message = msg, success = true });
        //        //}
        //        string msg1 = "The Specialty is not found in the Specialty reference table.";
        //        return Json(new { data = new string[] { }, message = msg1, success = true });
        //    }
        //    else
        //    {
        //        string msg = "The authorization header is either not valid or isn't Basic.";
        //        return Json(new { data = new string[] { }, message = msg, success = false });
        //        //throw new Exception("The authorization header is either not valid or isn't Basic.");
        //    }
        //}


        [HttpGet]
        [Route("specialty/id")]
        public IHttpActionResult Getspecialty_id([FromUri]get_specialty param)
        {
            string msg = "";
            if (true)//progAuth.authorize()
            {

                List<doc_specialty_01112018> spec = specialty(param);

                var ret1 = JsonConvert.SerializeObject(spec);
                var json = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                // get appointment_type_id


                return Json(new { specialty = json, message = msg, success = true });



                //var ret = JsonConvert.SerializeObject(doctorSpec);
                //var json = Newtonsoft.Json.Linq.JArray.Parse(ret);
                msg = "Specialty for the doctor is added.";
                return Json(new { data = new string[] { }, message = msg, success = true });

            }
            else
            {
                msg = "The authorization header is either not valid or isn't Basic.";
                return Json(new { data = new string[] { }, message = msg, success = false });
                //throw new Exception("The authorization header is either not valid or isn't Basic.");
            }

        }

        [HttpGet]
        [Route("specialty")]
        public IHttpActionResult Getspecialty([FromUri]get_specialty param )
        {
            string msg = "";
            if (true)//progAuth.authorize()
            {

                List<doc_specialty_01112018> spec = specialty(param);

                var ret1 = JsonConvert.SerializeObject(spec);
                var json = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                // get appointment_type_id


                return Json(new { specialty = json, message = msg, success = true });



                //var ret = JsonConvert.SerializeObject(doctorSpec);
                //var json = Newtonsoft.Json.Linq.JArray.Parse(ret);
                msg = "Specialty for the doctor is added.";
                return Json(new { data = new string[] { }, message = msg, success = true });

            }
            else
            {
                msg = "The authorization header is either not valid or isn't Basic.";
                return Json(new { data = new string[] { }, message = msg, success = false });
                //throw new Exception("The authorization header is either not valid or isn't Basic.");
            }

        }

        [HttpGet]
        [Route("specialty/id1")]
        private IHttpActionResult Getspecialty_idvalue(string id)
        {
            string msg = "";
            if (true)//progAuth.authorize()
            {

                List<doc_specialty2> spec = _specialtyValue(id);

                var ret1 = JsonConvert.SerializeObject(spec);
                var json = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                // get appointment_type_id
                return Json(new { specialty = json, message = msg, success = true });

            }
            else
            {
                msg = "The authorization header is either not valid or isn't Basic.";
                return Json(new { data = new string[] { }, message = msg, success = false });
                //throw new Exception("The authorization header is either not valid or isn't Basic.");
            }

        }

        private List<doc_specialty2> _specialtyValue(string value)
        {
            long id = 0;
            bool btemp = long.TryParse(value, out id);

            List<doc_specialty2> spec = new List<doc_specialty2>();
            if (btemp)
            {
                var ref_spec = dbEntity.ref_specialty.Where(a => a.id == id);

                foreach (var i in ref_spec)
                {

                    //var cat = dbEntity.ref_specialty_category.Find(i.rel_ref_specialty_category_id);
                    spec.Add(new doc_specialty2
                    {
                        id = i.id,
                        code = i.code == null ? "" : i.code,
                        name = i.name == null ? "" : i.name,
                        provider_type = i.provider_type == null ? "" : i.provider_type,
                        specialization = i.specialization == null ? "" : i.specialization
                    });
                }
            }
            else // assume value passed is a description, not id
            {
                var ref_spec = dbEntity.ref_specialty.Where(a => a.name.ToLower().Contains(value.ToLower()));

                foreach (var i in ref_spec)
                {
                    //var cat = dbEntity.ref_specialty_category.Find(i.rel_ref_specialty_category_id);
                    spec.Add(new doc_specialty2
                    {
                        id = i.id,
                        code = i.code == null ? "" : i.code,
                        name = i.name == null ? "" : i.name,
                        provider_type = i.provider_type == null ? "" : i.provider_type,
                        specialization = i.specialization == null ? "" : i.specialization
                    });
                }
            }




            return spec;

        }

        private List<doc_specialty_01112018> specialty(get_specialty param)
        {
            // created: 01/11/2018

            List<doc_specialty_01112018> spec = new List<doc_specialty_01112018>();
            // 1/16/2018 var ref_specialty = from a in dbEntity.ref_specialty1
            //                         //join b in dbEntity.ref_condition on a.
            //                    select a;
            var ref_specialty = from a in dbEntity.con_ref_specialty_ref_condition
                                select new {
                                    level2_classification = a.ref_specialty1.level2_classification ,
                                    level3_specialization = a.ref_specialty1.level3_specialization,
                                    condition =  a.ref_condition
                                              
                                    };

            if (!string.IsNullOrEmpty(param.startwith))
            {
                ref_specialty = ref_specialty.Where(a => a.level3_specialization.StartsWith(param.startwith)
                            || a.level2_classification.StartsWith(param.startwith)
                );
            }    

                foreach (var i in ref_specialty)
                {
                    //List<string> condition = new List<string>();
                    ////if (i.conditions != null) {
                    ////    string[] con = i.conditions.Split('|');
                    //    foreach (var c in i.condition)
                    //    { condition.Add(c.description); }
                    ////}

                    //spec.Add(new doc_specialty_01112018
                    //{
                   
                    //    id = i.id,
                    //    provider_type = i.ref_specialty_provider.name,//  s.provider_type,

                    //    classification_code = i.level2_classification_code == null ? "" :  i.level2_classification_code,
                    //    classification = i.level2_classification,

                    //    specialization_code = i.level3_specialization_code == null ? "" : i.level3_specialization_code,
                    //    specialization = i.level3_specialization == null ? "" : i.level3_specialization, // s.specialization,
                    //    conditions = condition
                    //});
                }

                return spec;
           
        }

        [Obsolete]
        private List<doc_specialty2> specialty2 {
            // obsolete since: 01/11/2018
            get {
                List<doc_specialty2> spec = new List<doc_specialty2>();
                var ref_specialty = from a in dbEntity.ref_specialty select a;

                foreach (var i in ref_specialty)
                {

                    //var cat = dbEntity.ref_specialty_category.Find(i.rel_ref_specialty_category_id);
                    spec.Add(new doc_specialty2
                    {
                        id = i.id,
                        code = i.code == null ? "" : i.code,
                        name = i.name == null ? "" : i.name,
                        provider_type = i.provider_type == null ? "" : i.provider_type,
                        specialization = i.specialization == null ? "" : i.specialization
                    });
                }

                return spec;
            }
        }

        //bool haserror = false;
        //string errmsg = "";

        [HttpGet]
        [System.Web.Http.Route("specialty/doctor")]
        public IHttpActionResult Getdoctor(long specialty_id)
        {
            string root = HttpContext.Current.Server.MapPath("~/Temp");
            //long specialty_id = 0;

            var provider = new MultipartFormDataStreamProvider(root);
            try {
                //await Request.Content.ReadAsMultipartAsync(provider);
                //foreach (var key in provider.FormData.AllKeys)
                //{
                //    foreach (var val in provider.FormData.GetValues(key))
                //    {
                //        switch (key)
                //        {
                //            case "specialty_id":
                //                IsRequired(key, val, 1);
                //                long nVal = 0;
                //                bool nTemp = long.TryParse(val, out nVal);
                //                specialty_id = nVal;
                //                break;
                //        }
                //    }
                //}

                //IsRequired("specialty_id", specialty_id.ToString(), 2);
                //if (haserror)
                //{
                //    return Json(new { data = "", message = errmsg, success = false });
                //}

                var spec = dbEntity.ref_specialty.Find(specialty_id);

                if (spec != null)
                {
                    var con_doc = dbEntity.con_DOCTOR_ref_specialty.Where(a => a.rel_ref_specialty_id == spec.id);
                    List<get_docProfile> doc = new List<get_docProfile>();
                    if (con_doc.Count() > 0)
                    {
                        foreach (var i in con_doc)
                        {
                            var zip = dbEntity.ref_zip.Find(i.hs_DOCTOR.home_addr_zip_id);

                            doc.Add(new get_docProfile
                            {
                                id = i.id,
                                firstname = i.hs_DOCTOR.name_first == null ? "" : i.hs_DOCTOR.name_first,
                                lastname = i.hs_DOCTOR.name_last == null ? "" : i.hs_DOCTOR.name_last,
                                middlename = i.hs_DOCTOR.name_middle == null ? "" : i.hs_DOCTOR.name_middle,
                                gender = i.hs_DOCTOR.gender == null ? "" : i.hs_DOCTOR.gender.ToUpper(),
                                title = i.hs_DOCTOR.title == null ? "" : i.hs_DOCTOR.title,

                                email = i.hs_DOCTOR.email == null ? "" : i.hs_DOCTOR.email,
                                phone = i.hs_DOCTOR.phone == null ? "" : i.hs_DOCTOR.phone,
                                license = i.hs_DOCTOR.license_no == null ? "" : i.hs_DOCTOR.license_no,
                                npi = i.hs_DOCTOR.NPI == null ? "" : i.hs_DOCTOR.NPI,

                                //specialties
                                //image_url
                                //addr_id
                                address1 = i.hs_DOCTOR.home_addr_1 == null ? "" : i.hs_DOCTOR.home_addr_1,
                                //address2
                                //pecoscert
                                //faxto 
                                city = zip.city_name == null ? "" : zip.city_name,
                                state = zip.city_state == null ? "" : zip.city_state,
                                city_lat = zip.city_lat > 0 ? zip.city_lat : 0,
                                city_long = zip.city_lon > 0 ? zip.city_lon : 0,
                                zip = zip.zip == null ? "" : zip.zip,
                                bio = i.hs_DOCTOR.bio == null ? "" : i.hs_DOCTOR.bio
                            });
                        }

                        var ret1 = JsonConvert.SerializeObject(doc);
                        var json = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                        var msg = con_doc.Count() + (con_doc.Count() > 1 ? " Records found!" : " Record found!");

                        return Json(new { data = json, message = msg, success = true });
                    }

                    return Json(new { data = new string[] { }, message = "", success = false });
                }

                return Json(new { data = new string[] { }, message = "", success = false });
            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }


        }

        [HttpGet]
        [Route("doctors/specialty")]
        public IHttpActionResult getspecialist(string specialty_id = null, string gender =null, string date=null)
        {
            try {
                long specialty_id_new = 0;
                bool IsSpec = long.TryParse(specialty_id, out specialty_id_new);

                //if (specialty_id_new > 0 || !string.IsNullOrEmpty(gender))
                //{
                    //var doc_spec = dbEntity.con_DOCTOR_ref_specialty.Where(a => a.rel_ref_specialty_id == specialty_id_new);
                    var doc_spec = from a in dbEntity.con_DOCTOR_ref_specialty select a;

                    // filter Doctor by Specialty_id
                    if (specialty_id_new > 0) {
                        doc_spec = doc_spec.Where(b => b.rel_ref_specialty_id == specialty_id_new);
                    }
                    
                    // filter DOCTOR by gender
                    if (!string.IsNullOrEmpty(gender))
                    {
                        doc_spec = doc_spec.Where(a => a.hs_DOCTOR.gender == gender.ToLower());
                    }

                    List<doctor_specialty> doc = new List<doctor_specialty>();
                  

                    string vappt_name = "", vtime_slot = ""; long vappt_id = 0; double doc_fee = 0;
                 
                
                    foreach (var d in doc_spec) {
                    // 01/11/2018   List<doc_specialty2> spec = new List<Models.doc_specialty2>();
                    List<doc_specialty_01112018> spec = new List<doc_specialty_01112018>();
                        List<appt_type> doc_appt = new List<appt_type>();
   
                    var doc1 = d.hs_DOCTOR;
                        spec = custom.getSpecialty(doc1.id);

                        // get Doctor_ext
                        var doc_ext = dbEntity.DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doc1.id);
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
                                            var n1 = dbEntity.ref_APPOINTMENT_type.Find(vappt_id);
                                            vappt_name = n1.dname;

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
                            middle_name = doc1.name_middle == null? "" : doc1.name_middle,
                            doctor_fee = doc_fee,
                           
                            bio= doc1.bio == null? "" : doc1.bio,
                            email = doc1.email ==null? "" : doc1.email,
                            gender = doc1.gender == null? "" : doc1.gender,
                            title = doc1.title ==null ? "" : doc1.title,
                            phone = doc1.phone == null ? "" : doc1.phone,
                            license = doc1.license_no == null ? "" : doc1.license_no,
                            pecos_certificate = doc1.pecos_certification == null? "" : doc1.pecos_certification,
                            npi= doc1.NPI == null ? "" : doc1.NPI,
                            organization_name = doc1.organization_name == null? "": doc1.organization_name,
                            image_url = doc1.image_url == null ? "" : doc1.image_url,

                            appointment_type = doc_appt,
                            address = doc_address,
                            specialties = spec
                        });
                    }

                    var ret1 = JsonConvert.SerializeObject(doc);
                    var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                    return Json(new { data = json1, message = doc.Count() + (doc.Count() > 1 ? " Records found!" : " Record found!"), success = true });
                //}
                //else
                //{
                //    // specialty_id_new ==null
                //    return Json(new { data = new string[] { }, message = "Invalid parameter value provided.", success = false });
                //}

            }
            catch (Exception ex) {
                return Json(new {data=new string[] { },message=ex.Message, success=false });
            }
        }

        public IHttpActionResult Put(long doctorId, long specialtyId) {




            //Doc_Specialties_Profile doc_spec = new Doc_Specialties_Profile();
            //var doc_spec = dbEntity.Doc_Specialties_Profile.Where( a => a.doctor_id ==doctorId && a.specialty_id == specialtyId).FirstOrDefault();
            //if (doc_spec != null)
            //{
            //    doc_spec.doctor_id = doctorId;
            //    doc_spec.specialty_id = doc_spec.specialty_id;
            //    //db.Doc_Specialties_Profile.Add(doc_spec);
            //    dbEntity.Entry(doc_spec).State = System.Data.Entity.EntityState.Modified;
            //    dbEntity.SaveChanges();


            //    //List<specPerDoctor> docSpec = new List<specPerDoctor>();
            //    //docSpec.Add(new specPerDoctor
            //    //{
            //    //    id = doc_spec.id,
            //    //    docId = doctorId,
            //    //    specialty_id = spec.id,
            //    //    specialty_name = spec.name,
            //    //    specialty_description = spec.description
            //    //});

            //    //var ret = JsonConvert.SerializeObject(docSpec);
            //    //var json = Newtonsoft.Json.Linq.JArray.Parse(ret);
            //    string msg = "Specialty for the following doctor is updated: " + doc_spec.DOCTOR.namefirst + " " + doc_spec.DOCTOR.namelast;
            //    return Json(new { data = "", message = msg, success = true });
            //}

            string msg1 = "The Specialty is not found in the Specialty reference table.";
            return Json(new { data = new string[] { }, message = msg1, success = false });
        }

        [System.Web.Http.Route("Delete")]
        public IHttpActionResult Delete(long doctorId, long specialtyId) {
            //var ref_spec = db.Doc_Specialties_Profile

            //var doc_spec = dbEntity.Doc_Specialties_Profile.Where(a => a.doctor_id == doctorId && a.specialty_id == specialtyId).FirstOrDefault();
            //if (doc_spec != null)
            //{
            //    doc_spec.active = 0;
            //    dbEntity.Entry(doc_spec).State = System.Data.Entity.EntityState.Modified;
            //    dbEntity.SaveChanges();
            //}

            string msg1 = "The Specialty is not found in the Specialty reference table.";
            return Json(new { data = new string[] { }, message = msg1, success = false });

        }

        //public bool IsRequired(string key, string val, int i)
        //{
        //    if (i == 1)
        //    {
        //        if (string.IsNullOrEmpty(val))
        //        {
        //            haserror = true;
        //            errmsg += key + " is required. \r\n";
        //            return false;
        //        }
        //        return true;
        //    }
        //    else
        //    {
        //        if (string.IsNullOrEmpty(val))
        //        {
        //            haserror = true;
        //            errmsg += " Missing parameter: " + key + ".\r\n ";
        //            return false;
        //        }
        //        return true;
        //    }
        //}

        const double PIx = Math.PI;
        const double RADIO = 6378.16;

        public static double Radians(double x) {
            return x * PIx / 180;
        }



        [HttpGet]
        [Route("FindDoctor")]
        //public IHttpActionResult finddoctor(long specialty_id, string appointment_type, double lat, double longi)
        public IHttpActionResult Getfinddoctor( double lat, double longi, long specialty_id = 0, long appointment_type = 0)
        {

            //var doc1 = Newtonsoft.Json.Linq.JObject.Parse(@"{
            //            'Title': 'Alpha',
            //            'data': [{
            //                'Id': 'Fox 2',
            //                'Field': 'King6',
            //                'Value': 'Alpha',
            //                'Description': 'Tango'
            //            }]
            //        }");

            //var doc2 = Newtonsoft.Json.Linq.JObject.Parse(@"{
            //          'Title': 'Bravo',
            //          'data':[{
            //                'Id':'Kilo',
            //                'Field':'Echo',
            //                'Value':'Romeo',
            //                'Description':'Jester'
            //           }]
            //}");

            //var final = JsonConvert.SerializeObject(new
            //{
            //    Title = doc1["Title"],
            //    data = doc1["data"].Union(doc2["data"])
            //},
            // Newtonsoft.Json.Formatting.Indented);

            //double ref_lat = 39.780319;
            //double re_longi = -89.681066;
            double lat2 = 42.293;
            double longi2 = -83.715363;

            System.Device.Location.GeoCoordinate c1 = new System.Device.Location.GeoCoordinate(lat, longi);

            //var doc = from a in dbEntity.hs_DOCTOR select a;
            var doc = dbEntity.hs_DOCTOR.Where(a => a.ref_zip.city_lat == lat && a.ref_zip.city_lon == longi);
            var doc_spec = dbEntity.con_DOCTOR_ref_specialty.Where(a => a.rel_ref_specialty_id == specialty_id);


        
          
            List<proxi_doc> prox = new List<proxi_doc>();
            //foreach (var n in doc_spec)
            //{
            //    //double d = FindDistance(lat2, longi2, lat, longi);


            //    string c = n.DOCTOR.ref_zip.city_name;
            //    string s = n.DOCTOR.ref_zip.city_state;
               

            //    if (inKM < 30)
            //    {
            //        addr.Add(new spec_address {
            //            address = n.DOCTOR.addr_address1,
            //            city = n.DOCTOR.ref_zip.city_name,
            //            county = n.DOCTOR.ref_zip.city_county,
            //            state = n.DOCTOR.ref_zip.city_state,
            //            lat = n.DOCTOR.ref_zip.city_lat,
            //            longi = n.DOCTOR.ref_zip.city_lon
            //        });

            //        prox.Add(new proxi_doc {
            //            id = n.DOCTOR.id,
            //            firstname = n.DOCTOR.name_first,
            //            lastname = n.DOCTOR.name_last,
            //            address = addr
            //        });
            //    }
            //}

            foreach (var d in doc)
            {
                List<spec_address> addr = new List<spec_address>();

                double z_lat = d.ref_zip ==null?0:d.ref_zip.city_lat;
                double z_long = d.ref_zip == null ? 0 : d.ref_zip.city_lon;
                System.Device.Location.GeoCoordinate c2 = new System.Device.Location.GeoCoordinate(z_lat, z_long);

                double inKM = c1.GetDistanceTo(c2)/ 1000;
                if (inKM < 30)
                {
                    //addr.Add(new spec_address
                    //{
                    //    address = d.addr_address1,
                    //    city = d.ref_zip.city_name,
                    //    county = d.ref_zip.city_county,
                    //    state = d.ref_zip.city_state,
                    //    lat = d.ref_zip.city_lat,
                    //    longi = d.ref_zip.city_lon
                    //});

                    prox.Add(new proxi_doc
                    {
                        id = d.id,
                        firstname = d.name_first,
                        lastname = d.name_last,
                        address = addr
                    });
                }
            }
           

            return Json(new { data = prox });


            //double d = FindDistance(lat, longi, ref_lat, re_longi);
            //1866.8278770706186


            //  return Json(new { d });
        }


        public double FindDistance(Double lat1, Double longi1, Double lat2, Double longi2)
        {
            //var sCoord = new System.Device.Location.GeoCoordinate(lat1, longi1);
            //var ref_coord = new System.Device.Location.GeoCoordinate(583838,-0030412);
            //var s = sCoord.GetDistanceTo(ref_coord);

            double R = 6371; //km
            double dLat = Radians(lat2 - lat1);
            double dLon = Radians(longi2 - longi1);

            lat1 = Radians(lat1);
            lat2 = Radians(lat2);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c;

            return d;

        }
    }

    public class get_specialty {
        public string id { get; set; }
        public string startwith { get; set; }
    }
  
}

