using api.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;

namespace api.Models
{



    /// <summary>
    /// User-defined classes.
    /// </summary>
    public class custom {

       
        static SV_db1Entities dbEntity = new SV_db1Entities();

        // created: 01/11/2018
        public static List<doc_specialty_01112018> getSpecialty_all()
        {
           

            // 01/17/2018 var specialty = from a in dbEntity.ref_specialty1 select a;
            var specialty = from a in dbEntity.ref_specialty1
                            select new
                            {
                                a.id,
                                specialty_provider = a.ref_specialty_provider.name,
                                a.level2_classification,
                                a.level2_classification_code,
                                a.level3_specialization,
                                a.level3_specialization_code,
                                condition = from b in a.ref_condition
                                            select b.dname,

                            };
            List<doc_specialty_01112018> spec = new List<doc_specialty_01112018>();

            specialty.OrderByDescending(a => a.id);

            foreach (var s in specialty)
            {
         
                // string[] con = s.conditions.Split('|');
                List<string> condition = new List<string>();
                foreach (var c in condition)
                { condition.Add(c); }

                spec.Add(new doc_specialty_01112018
                {
                    id = s.id,
                    // name = s.name,
                    provider_type = s.specialty_provider,//  s.provider_type,

                    classification_code = s.level2_classification_code,
                    classification = s.level2_classification,

                    specialization_code = s.level3_specialization_code == null ? "" : s.level3_specialization_code,
                    specialization = s.level3_specialization == null ? "" : s.level3_specialization, // s.specialization,
                    conditions = condition
                });
            }

            return spec;
        }

        public static List<doc_specialty_01112018> getSpecialty(long doctor_id)
        {
            // created : 01/11/2018
            List<doc_specialty_01112018> spec = new List<doc_specialty_01112018>();

            if (doctor_id > 0)
            {
                   var con_ref_spec = dbEntity.con_DOCTOR1_ref_specialty.Where(a => a.rel_DOCTOR_id == doctor_id);
                   

                    foreach (var n in con_ref_spec)
                    {

                        // string[] con = n.ref_specialty1.conditions.Split('|');
                        List<string> condition = new List<string>();
                        foreach (var c in n.ref_specialty1.ref_condition)
                        { condition.Add(c.dname); }

                    spec.Add(new doc_specialty_01112018
                        {
                            // id = n.ref_specialty.id,
                            // description = n.ref_specialty.description,
                            // name = n.ref_specialty.name,
                            // actor = n.ref_specialty.actor == null ? "" : n.ref_specialty.actor

                            id = n.id,
                            provider_type = n.ref_specialty1.ref_specialty_provider.name,//  s.provider_type,

                            classification_code = n.ref_specialty1.level2_classification_code,
                            classification = n.ref_specialty1.level2_classification,

                        specialization_code = n.ref_specialty1.level3_specialization_code == null ? "" : n.ref_specialty1.level3_specialization_code,
                        specialization = n.ref_specialty1.level3_specialization == null ? "" : n.ref_specialty1.level3_specialization, // s.specialization,
                            conditions = condition
                        });
                    }
             

                //if (doc_ext.Count() > 0)
                //{
                //    string[] spec_ids = doc_ext.FirstOrDefault().value.Split(',');
                //    foreach (var n in spec_ids)
                //    {
                //        long spec_id = 0;
                //        bool i = long.TryParse(n, out spec_id);
                //        var ref_spec = dbEntity.ref_specialty.Find(spec_id);
                //        if (ref_spec != null)
                //        {
                //            spec.Add(new doc_specialty2
                //            {
                //                id = ref_spec.id,
                //                name = ref_spec.name == null ? "" : ref_spec.name,
                //                code = ref_spec.code == null ? "" : ref_spec.code,
                //                specialization = ref_spec.specialization == null ? "" : ref_spec.specialization,
                //                provider_type = ref_spec.provider_type == null ? "" : ref_spec.provider_type
                //            });
                //        }
                //    }
                //}



            }

            return spec;
        }


        [System.Obsolete]
        //public static List<doc_specialty2> getSpecialty(long doctor_id)
        public static List<doc_specialty2> getSpecialty2(long doctor_id)
        {
            // deprecated : 01/11/2018
            List<doc_specialty2> spec = new List<doc_specialty2>();

            if (doctor_id > 0)
            {
                //var con_ref_spec = db1.con_DOCTOR_ref_specialty.Where(a => a.rel_DOCTOR_id == doctor_id);
                var doc_ext = dbEntity.DOCTOR_ext.Where(a => a.attr_name == "specialty_id" && a.rel_DOCTOR_id == doctor_id);
                if (doc_ext.Count() > 0)
                {
                    string[] spec_ids = doc_ext.FirstOrDefault().value.Split(',');
                    foreach (var n in spec_ids)
                    {
                        long spec_id = 0;
                        bool i = long.TryParse(n, out spec_id);
                        var ref_spec = dbEntity.ref_specialty.Find(spec_id);
                        if (ref_spec != null)
                        {
                            spec.Add(new doc_specialty2
                            {
                                id = ref_spec.id,
                                name = ref_spec.name == null ? "" : ref_spec.name,
                                code = ref_spec.code == null ? "" : ref_spec.code,
                                specialization = ref_spec.specialization == null ? "" : ref_spec.specialization,
                                provider_type = ref_spec.provider_type == null ? "" : ref_spec.provider_type
                            });
                        }
                    }

                }

                //if (doc_ext.Count() > 0)
                //{
                //    foreach (var n in con_ref_spec)
                //    {
                //        spec.Add(new doc_specialty
                //        {
                //            id = n.ref_specialty.id,
                //            description = n.ref_specialty.description,
                //            name = n.ref_specialty.name,
                //            actor = n.ref_specialty.actor == null ? "" : n.ref_specialty.actor
                //        });
                //    }
                //}

            }

            return spec;
        }

        [System.Obsolete]
        public static List<doc_specialty> getSpecialty_all_2()
        {
            // obsolete since: 01/11/2018
            var specialty = from a in dbEntity.ref_specialty select a;
            List<doc_specialty> spec = new List<doc_specialty>();

            specialty.OrderByDescending(a => a.id);
            foreach (var s in specialty)
            {
                //1/15/2018 var con = dbEntity.con_ref_specialty_ref_condition.Where(a => a.ref_specialty1.id == s.id);
                List<doc_condition2> condition = new List<doc_condition2>();

                //1/15/2018 if (con.Count() > 0) con.OrderByDescending(a => a.id);
                //foreach (var c in con)
                //{
                //    condition.Add(new doc_condition2 {
                //        id = c.ref_condition.id,
                //        name = c.ref_condition.name,
                //        description = c.ref_condition.description
                //    });
                //}


                spec.Add(new doc_specialty {
                    id = s.id,
                    code = s.code,
                    name = s.name,
                    provider_type = s.provider_type,
                    specialization = s.specialization,
                    conditions = condition
                });
            }

            return spec;

       }

        public static soul_name getSoul_name(long soul_id) {

            soul_name s = new soul_name();
            var _soul = dbEntity.SOULs.Find(soul_id);
            if (_soul != null)
            {
                s.name_first = _soul.name_first == null ? "" : _soul.name_first;
                s.name_last = _soul.name_last == null ? "" : _soul.name_last;
            }

            return s;
        }

        /// <summary>
        /// Get doctor's practice address.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<zip_search_address> _getDoctor_practiceaddress(hs_DOCTOR doc)
        {
            //string zipcode = value.Substring(0, 5).ToString();
            //var addr = dbEntity.ref_zip.Where(b => b.zip == zipcode);

            List<zip_search_address> addr = new List<zip_search_address>();



            if (doc.practice_addr_zip_id != null || !string.IsNullOrEmpty(doc.practice_addr_1))
            {
                var ref_zip = dbEntity.ref_zip.Find(doc.practice_addr_zip_id);

                bool sref = ref_zip == null;
                addr.Add(new zip_search_address
                {
                    address1 = doc.practice_addr_1 == null ? "" : doc.practice_addr_1,
                    address2 = doc.practice_addr_2 == null ? "" : doc.practice_addr_2,
                    zip = sref ? "" : ref_zip.zip,
                    city = sref ? "" : doc.ref_zip.city_name,
                    state = sref ? "" : doc.ref_zip.city_state,
                    state_long = sref ? "": doc.ref_zip.city_state_long,
                    lat = sref ? 0 : ref_zip.city_lat,
                    lng = sref ? 0 : ref_zip.city_lon,
                    county = sref ? "" : doc.ref_zip.city_county
                });

            }



            return addr;
        }

        public static List<zip_search_address> _getDoctor_practiceaddress_doc1(DOCTOR1 doc)
        {
            // temporary for table DOCTOR1

            //string zipcode = value.Substring(0, 5).ToString();
            //var addr = dbEntity.ref_zip.Where(b => b.zip == zipcode);

            List<zip_search_address> addr = new List<zip_search_address>();



            if (doc.practice_addr_zip_id != null || !string.IsNullOrEmpty(doc.practice_addr_1))
            {
                var ref_zip = dbEntity.ref_zip.Find(doc.practice_addr_zip_id);

                bool sref = ref_zip == null;
                addr.Add(new zip_search_address
                {
                    address1 = doc.practice_addr_1 == null ? "" : doc.practice_addr_1,
                    address2 = doc.practice_addr_2 == null ? "" : doc.practice_addr_2,
                    zip = sref ? "" : ref_zip.zip,
                    city = sref ? "" : doc.ref_zip.city_name,
                    state = sref ? "" : doc.ref_zip.city_state,
                    state_long = sref ? "" : doc.ref_zip.city_state_long,
                    lat = sref ? 0 : ref_zip.city_lat,
                    lng = sref ? 0 : ref_zip.city_lon,
                    county = sref ? "" : doc.ref_zip.city_county
                });

            }



            return addr;
        }

        /// <summary>
        /// Get doctor's home address.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<zip_search_address> _getDoctor_homeaddress(hs_DOCTOR doc)
        {
            //string zipcode = value.Substring(0, 5).ToString();
            //var addr = dbEntity.ref_zip.Where(b => b.zip == zipcode);

            List<zip_search_address> addr = new List<zip_search_address>();


            if (doc.home_addr_zip_id != null || !string.IsNullOrEmpty(doc.home_addr_1))
            {
                http://localhost:64625/doctor/profile?npi=1477647402
                var ref_zip = dbEntity.ref_zip.Find(doc.home_addr_zip_id);
                bool sref = ref_zip == null;
                addr.Add(new zip_search_address
                {
                    address1 = doc.home_addr_1 == null ? "" : doc.home_addr_1,
                    address2 = doc.home_addr_2 == null ? "" : doc.home_addr_2,
                    zip = sref ? "" : ref_zip.zip,
                    city = sref ? "" : doc.ref_zip.city_name,
                    state = sref ? "" : doc.ref_zip.city_state,
                    state_long = sref ? "" : doc.ref_zip.city_state_long,
                    lat = sref ? 0 : ref_zip.city_lat,
                    lng = sref ? 0 : ref_zip.city_lon,
                    county = sref ? "" : doc.ref_zip.city_county
                });
            }

            //foreach (var i in addr)
            //{
            //    //string addr2 = doc_address.addr_address2 == null ? "" : doc_address.addr_address2;
            //    // saved in the doctor table is the practice address

            //    // address.street = doc_address.addr_address1 == null ? "" : doc_address.addr_address1 + " " + addr2;
            //    //ref_zip_id = System.Convert.ToInt32(doc.addr_rel_ref_zip_id),

            //    address.Add(new zip_search_address {

            //        //address1 = doc_info.addr_address1 == null ? "" : doc_info.addr_address1,
            //        //address2 = doc_info.addr_address2 == null ? "" : doc_info.addr_address2,
            //        address1 = "",
            //        address2 = "",
            //        zip = i.zip == null ? "" : i.zip,
            //        city = i.city_name == null ? "" : i.city_name,
            //        state = i.city_state == null ? "" : i.city_state,
            //        lat = i.city_lat,
            //        lng = i.city_lon,
            //        county = i.city_county == null ? "" : i.city_county
            //});
            //}

            return addr;
        }

        public static List<zip_search_address> _getDoctor_homeaddress_doc1(DOCTOR1 doc)
        {
            // temporary for table DOCTOR1

            //string zipcode = value.Substring(0, 5).ToString();
            //var addr = dbEntity.ref_zip.Where(b => b.zip == zipcode);

            List<zip_search_address> addr = new List<zip_search_address>();


            if (doc.home_addr_zip_id != null || !string.IsNullOrEmpty(doc.home_addr_1))
            {
               // http://localhost:64625/doctor/profile?npi=1477647402
                var ref_zip = dbEntity.ref_zip.Find(doc.home_addr_zip_id);
                bool sref = ref_zip == null;
                addr.Add(new zip_search_address
                {
                    address1 = doc.home_addr_1 == null ? "" : doc.home_addr_1,
                    address2 = doc.home_addr_2 == null ? "" : doc.home_addr_2,
                    zip = sref ? "" : ref_zip.zip,
                    city = sref ? "" : ref_zip.city_name,
                    state = sref ? "" : ref_zip.city_state,
                    state_long = sref ? "" : ref_zip.city_state_long,
                    lat = sref ? 0 : ref_zip.city_lat,
                    lng = sref ? 0 : ref_zip.city_lon,
                    county = sref ? "" : ref_zip.city_county
                });
            }

            //foreach (var i in addr)
            //{
            //    //string addr2 = doc_address.addr_address2 == null ? "" : doc_address.addr_address2;
            //    // saved in the doctor table is the practice address

            //    // address.street = doc_address.addr_address1 == null ? "" : doc_address.addr_address1 + " " + addr2;
            //    //ref_zip_id = System.Convert.ToInt32(doc.addr_rel_ref_zip_id),

            //    address.Add(new zip_search_address {

            //        //address1 = doc_info.addr_address1 == null ? "" : doc_info.addr_address1,
            //        //address2 = doc_info.addr_address2 == null ? "" : doc_info.addr_address2,
            //        address1 = "",
            //        address2 = "",
            //        zip = i.zip == null ? "" : i.zip,
            //        city = i.city_name == null ? "" : i.city_name,
            //        state = i.city_state == null ? "" : i.city_state,
            //        lat = i.city_lat,
            //        lng = i.city_lon,
            //        county = i.city_county == null ? "" : i.city_county
            //});
            //}

            return addr;
        }

        public static List<_insurance> getInsuranceBy_id(long soul_id)
        {
            List<_insurance> ref_ins = new List<_insurance>();
            //var soul_ex = dbEntity.SOUL_ext.Where(a =>a.rel_SOUL_id == soul_id);
            var con_ins = dbEntity.con_SOUL_ref_insurance.Where(a => a.rel_SOUL_id == soul_id);

            foreach (var s in con_ins)
            {
                long ins_id = Convert.ToInt64(s.rel_ref_insurance_provider_id);
                var ins = dbEntity.ref_insurance_provider.Find(ins_id);
                if (ins != null)
                {
                    ref_ins.Add(new _insurance {
                        id = ins.id,
                        provider = ins.PayerName.Split('|')[0]
                    });
                }

               
            }

         
            return ref_ins;
          

        }
    }

    public class soul_name {
       public string name_first { get; set; }
        public string name_last { get; set; }
    }
    public class Entry {
        static DateTime dt = DateTime.UtcNow;
        static SV_db1Entities dbEntity = new SV_db1Entities();
        public static bool saveAppt_ext(string _attr_name, string _dname, string _value, long appt_id = 0, long user_id = 0, int data_type = 0)
        {
            try
            {



                var d_ext = dbEntity.APPOINTMENT_ext.Where(a => a.attr_name == _attr_name && a.rel_APPOINTMENT_id == appt_id).FirstOrDefault();
                if (d_ext == null) // add attr if does not exist yet
                {
                    //dbEntity.SOUL_ext.Add(ext_city);
                    d_ext = new APPOINTMENT_ext();
                    d_ext.rel_APPOINTMENT_id = appt_id;
                    d_ext.attr_name = _attr_name;
                    d_ext.dname = _dname;
                    d_ext.value = _value;
                    d_ext.dt_create = dt;
                    d_ext.rel_ref_datatype_id = data_type;
                    d_ext.create_by__USER_id = user_id;
                    dbEntity.APPOINTMENT_ext.Add(d_ext);
                    dbEntity.SaveChanges();
                }
                else // update the record if attr_name already exist
                {
                    //DOCTOR_ext d_ext = new DOCTOR_ext();
                    //var d_ext = dbEntity.DOCTOR_ext.Where(a => a.attr_name == _attr_name && a.rel_DOCTOR_id == doc_id).FirstOrDefault();

                    //d_ext.attr_name = _attr_name;
                    //d_ext.dname = _dname;
                    d_ext.value = _value;
                    d_ext.dt_update = dt;
                    d_ext.update_by__USER_id = user_id;
                    dbEntity.Entry(d_ext).State = System.Data.Entity.EntityState.Modified;
                    dbEntity.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return false;  //Json(new { data = new string[] { }, message = ex.Message, success = false });

            }


            return true;
        }


        public static bool saveMarketer_ext(string _attr_name, string _dname, string _value, long market_id = 0, long user_id = 0, int data_type = 0)
        {
            try
            {
                var d_ext = dbEntity.MARKETER_ext.Where(a => a.attr_name == _attr_name && a.rel_MARKETER_id == market_id).FirstOrDefault();
                if (d_ext == null) // add attr if does not exist yet
                {
                    //dbEntity.SOUL_ext.Add(ext_city);
                    d_ext = new MARKETER_ext();
                    d_ext.rel_MARKETER_id = market_id;
                    d_ext.attr_name = _attr_name;
                    d_ext.dname = _dname;
                    d_ext.value = _value;
                    d_ext.dt_create = dt;
                    d_ext.rel_ref_datatype_id = data_type;
                    d_ext.create_by__USER_id = user_id;
                    dbEntity.MARKETER_ext.Add(d_ext);
                    dbEntity.SaveChanges();
                }
                else // update the record if attr_name already exist
                {
                    //DOCTOR_ext d_ext = new DOCTOR_ext();
                    //var d_ext = dbEntity.DOCTOR_ext.Where(a => a.attr_name == _attr_name && a.rel_DOCTOR_id == doc_id).FirstOrDefault();

                    //d_ext.attr_name = _attr_name;
                    //d_ext.dname = _dname;
                    d_ext.value = _value;
                    d_ext.dt_update = dt;
                    d_ext.update_by__USER_id = user_id;
                    dbEntity.Entry(d_ext).State = System.Data.Entity.EntityState.Modified;
                    dbEntity.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return false;  //Json(new { data = new string[] { }, message = ex.Message, success = false });

            }


            return true;
        }

        public static bool savePhysician_ext(string _attr_name, string _dname, string _value, long market_id = 0, long user_id = 0, int data_type = 0)
        {
            try
            {
                var d_ext = dbEntity.PHYSICIAN_ext.Where(a => a.attr_name == _attr_name && a.rel_PHYSICIAN_id == market_id).FirstOrDefault();
                if (d_ext == null) // add attr if does not exist yet
                {
                    //dbEntity.SOUL_ext.Add(ext_city);
                    d_ext = new PHYSICIAN_ext();
                    d_ext.rel_PHYSICIAN_id = market_id;
                    d_ext.attr_name = _attr_name;
                    d_ext.dname = _dname;
                    d_ext.value = _value;
                    d_ext.dt_create = dt;
                    d_ext.rel_ref_datatype_id = data_type;
                    d_ext.create_by__USER_id = user_id;
                    dbEntity.PHYSICIAN_ext.Add(d_ext);
                    dbEntity.SaveChanges();
                }
                else // update the record if attr_name already exist
                {
                    //DOCTOR_ext d_ext = new DOCTOR_ext();
                    //var d_ext = dbEntity.DOCTOR_ext.Where(a => a.attr_name == _attr_name && a.rel_DOCTOR_id == doc_id).FirstOrDefault();

                    //d_ext.attr_name = _attr_name;
                    //d_ext.dname = _dname;
                    d_ext.value = _value;
                    d_ext.dt_update = dt;
                    d_ext.update_by__USER_id = user_id;
                    dbEntity.Entry(d_ext).State = System.Data.Entity.EntityState.Modified;
                    dbEntity.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return false;  //Json(new { data = new string[] { }, message = ex.Message, success = false });

            }


            return true;
        }

        public static bool saveSupplier_ext(string _attr_name, string _dname, string _value, long market_id = 0, long user_id = 0, int data_type = 0)
        {
            try
            {
                var d_ext = dbEntity.SUPPLIER_ext.Where(a => a.attr_name == _attr_name && a.rel_SUPPLIER_id == market_id).FirstOrDefault();
                if (d_ext == null) // add attr if does not exist yet
                {
                    //dbEntity.SOUL_ext.Add(ext_city);
                    d_ext = new SUPPLIER_ext();
                    d_ext.rel_SUPPLIER_id = market_id;
                    d_ext.attr_name = _attr_name;
                    d_ext.dname = _dname;
                    d_ext.value = _value;
                    d_ext.dt_create = dt;
                    d_ext.rel_ref_datatype_id = data_type;
                    d_ext.create_by__USER_id = user_id;
                    dbEntity.SUPPLIER_ext.Add(d_ext);
                    dbEntity.SaveChanges();
                }
                else // update the record if attr_name already exist
                {
                    //DOCTOR_ext d_ext = new DOCTOR_ext();
                    //var d_ext = dbEntity.DOCTOR_ext.Where(a => a.attr_name == _attr_name && a.rel_DOCTOR_id == doc_id).FirstOrDefault();

                    //d_ext.attr_name = _attr_name;
                    //d_ext.dname = _dname;
                    d_ext.value = _value;
                    d_ext.dt_update = dt;
                    d_ext.update_by__USER_id = user_id;
                    dbEntity.Entry(d_ext).State = System.Data.Entity.EntityState.Modified;
                    dbEntity.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return false;  //Json(new { data = new string[] { }, message = ex.Message, success = false });

            }


            return true;
        }

    }
    public class Validation {

        static DateTime dt = DateTime.UtcNow;


        public static long checkApptType(long appointment_type_id)
        {
            SV_db1Entities db = new SV_db1Entities();

            //long appt_type_id = 0;
            //bool btemp = long.TryParse(appointment_type_id.ToString(), out appt_type_id);
            var appt_type = db.ref_APPOINTMENT_type.Find(appointment_type_id);
            if (appt_type == null)
            {
                return 0;
                //return Json(new { data = new string[] { }, message = "Invalid appoitnment_type_id value.", success = false });
            }
            return appt_type.id;
        }

        public static long checkStatus(string appointment_type_id)
        {
            SV_db1Entities db = new SV_db1Entities();

            long appt_type_id = 0;
            bool btemp = long.TryParse(appointment_type_id, out appt_type_id);
            var appt_type = db.ref_APPOINTMENT_type.Find(appt_type_id);
            if (appt_type == null)
            {
                return 0;
                //return Json(new { data = new string[] { }, message = "Invalid appoitnment_type_id value.", success = false });
            }
            return appt_type_id;
        }

   

        public static bool saveUSER_ext(string _attr_name, string _dname, string _value, long user_id = 0)
        {
            try
            {
                SV_db1Entities dbEntity = new SV_db1Entities();

                var d_ext = dbEntity.USER_ext.Where(a => a.attr_name == _attr_name && a.rel_USER_id == user_id).FirstOrDefault();
                if (d_ext == null) // add attr if does not exist yet
                {
                    //dbEntity.SOUL_ext.Add(ext_city);
                    d_ext = new USER_ext();
                    d_ext.rel_USER_id = user_id;
                    d_ext.attr_name = _attr_name;
                    d_ext.dname = _dname;
                    d_ext.value = _value;
                    d_ext.dt_create = dt;
                    d_ext.create_by__USER_id = 0;
                    dbEntity.USER_ext.Add(d_ext);
                    dbEntity.SaveChanges();
                }
                else // update the record if attr already exist
                {
                    //d_ext.attr_name = _attr_name;
                    //d_ext.dname = _dname;
                    d_ext.value = _value;
                    d_ext.dt_update = dt;
                    d_ext.update_by__USER_id = 0;
                    dbEntity.Entry(d_ext).State = System.Data.Entity.EntityState.Modified;
                    dbEntity.SaveChanges();

                }
            }
            catch (Exception ex) { }


            return true;
        }

        public static bool saveSOUL_ext(string _attr_name, string _dname, string _value, long soul_id = 0, long user_id = 0)
        {
            try
            {
                SV_db1Entities dbEntity = new SV_db1Entities();


                var d_ext = dbEntity.SOUL_ext.Where(a => a.attr_name == _attr_name && a.rel_SOUL_id == soul_id).FirstOrDefault();
                if (d_ext == null) // add attr if does not exist yet
                {
                    //dbEntity.SOUL_ext.Add(ext_city);
                    d_ext = new SOUL_ext();
                    d_ext.rel_SOUL_id = soul_id;
                    d_ext.attr_name = _attr_name;
                    d_ext.dname = _dname;
                    d_ext.value = _value;
                    d_ext.dt_create = dt;
                    d_ext.create_by__USER_id = user_id;
                    dbEntity.SOUL_ext.Add(d_ext);
                    dbEntity.SaveChanges();
                }
                else // update the record if attr_name already exist
                {
                    //DOCTOR_ext d_ext = new DOCTOR_ext();
                    //var d_ext = dbEntity.DOCTOR_ext.Where(a => a.attr_name == _attr_name && a.rel_DOCTOR_id == doc_id).FirstOrDefault();

                    //d_ext.attr_name = _attr_name;
                    //d_ext.dname = _dname;
                    d_ext.value = _value;
                    d_ext.dt_update = dt;
                    d_ext.update_by__USER_id = user_id;
                    dbEntity.Entry(d_ext).State = System.Data.Entity.EntityState.Modified;
                    dbEntity.SaveChanges();
                }
            }
            catch (Exception ex) { }


            return true;
        }


        public static bool userAuth(string auth)
        {
            if (auth != null) {

                string u_auth = "deftsoft";
                string p_auth = "deftsoftapikey";

                string auth64 = "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(u_auth + ":" + p_auth));
                if (auth64 == auth)
                {
                    return true;
                }
                else
                { return false; }
            }
            return true;
        }

        public static string validateDate(string dat)
        {
            //12/12/1990
            string[] formats = {"M/d/yyyy", "MM/dd/yyyy","M/dd/yyyy",
                "M-d-yyyy", "MM-dd-yyyy","M-dd-yyyy" };

            DateTime dateVal;

            bool i = DateTime.TryParseExact(dat, formats,
                new CultureInfo("en-US"),
                System.Globalization.DateTimeStyles.None, out dateVal);

            if (i)
            {
                return dateVal.ToString().Split(' ')[0]; //saveDoctor_ext(attr_name, dname, dateVal.ToString().Split(' ')[0], doc_id);
            }
            return "";

        }

        public static bool validateDate1(string attr_name, string dname, string dat, long doc_id)
        {
            //12/12/1990
            string[] formats = {"M/d/yyyy", "MM/dd/yyyy","M/dd/yyyy",
                "M-d-yyyy", "MM-dd-yyyy","M-dd-yyyy" };

            DateTime dateVal;

            bool i = DateTime.TryParseExact(dat, formats,
                new CultureInfo("en-US"),
                System.Globalization.DateTimeStyles.None, out dateVal);

            if (i)
            {
                return saveDoctor_ext(attr_name, dname, dateVal.ToString().Split(' ')[0], doc_id);
            }
            return false;

        }

        public static bool saveDoctor_ext(string _attr_name, string _dname, string _value, long doc_id = 0)
        {
            try
            {
                SV_db1Entities dbEntity = new SV_db1Entities();

                var d_ext = dbEntity.hs_DOCTOR_ext.Where(a => a.attr_name == _attr_name && a.rel_DOCTOR_id == doc_id).FirstOrDefault();
                if (d_ext == null) // add attr_name if does not exist yet
                {
                    d_ext = new hs_DOCTOR_ext();
                    d_ext.rel_DOCTOR_id = doc_id;
                    d_ext.attr_name = _attr_name;
                    d_ext.dname = _dname;
                    d_ext.value = _value;
                    d_ext.dt_create = dt;
                    d_ext.create_by__USER_id = 0;
                    dbEntity.hs_DOCTOR_ext.Add(d_ext);
                    dbEntity.SaveChanges();
                }
                else // update the record if attr_name already exist
                {
                    //DOCTOR_ext d_ext = new DOCTOR_ext();
                    //var d_ext = dbEntity.DOCTOR_ext.Where(a => a.attr_name == _attr_name && a.rel_DOCTOR_id == doc_id).FirstOrDefault();

                    d_ext.attr_name = _attr_name;
                    d_ext.dname = _dname;
                    d_ext.value = _value;
                    d_ext.dt_update = dt;
                    d_ext.update_by__USER_id = 0;
                    dbEntity.Entry(d_ext).State = System.Data.Entity.EntityState.Modified;
                    dbEntity.SaveChanges();

                }
            }
            catch (Exception ex) { }


            return true;
        }
    }
    public class progAuth {

        //bool haserror = false;
        //string errmsg = "", infomsg = "";
        // public bool IsRequired(string key, string val, int i)
        // {
        //     if (i == 1)
        //     {
        //         if (string.IsNullOrEmpty(val))
        //         {
        //             haserror = true;
        //             errmsg += key + " is required. \r\n";
        //             return false;
        //         }
        //         return true;
        //     }
        //     else
        //     {
        //         if (string.IsNullOrEmpty(val))
        //         {
        //             haserror = true;
        //             errmsg += " Missing parameter: " + key + ". \r\n";
        //             return false;
        //         }
        //         return true;
        //     }
        // }


        // GET /doctor HTTP/1.1
        // Host: api.healthsplash.com
        // Authorization: Basic ZGVmdHNvZnQ6ZGVmdHNvZnRhcGlrZXk=
        // Cache - Control: no-cache
        // Postman-Token: 19868b9a-9722-98bc-d00b-5c307af6feda

        public static bool authorize()
        {
            //source: http://stackoverflow.com/questions/25855698/how-can-i-retrieve-basic-authentication-credentials-from-the-header
            HttpContext httpContext = HttpContext.Current;
            string authHeader = httpContext.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodedUserNamePassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUserNamePassword));

                int sep = usernamePassword.IndexOf(':');
                string username = usernamePassword.Substring(0, sep);
                string password = usernamePassword.Substring(sep + 1);

                if (username == "deftsoft" && password == "deftsoftapikey")
                {
                    return true;
                }
                return false;
            }

            return false;
        }


    }
    public class appt_id
    {
        public long appointment_id { get; set; }
    }

    public class user_log {
        public long id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string username { get; set; }
    }
    public class doc_query
    {
        // doctor_name, npi,license_no, address, state, city, zipcode


        public string name { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string npi { get; set; }
        public string specialty { get; set; }
        public string license { get; set; }
        public string address { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }
        public string acck { get; set; }
        public int take { get; set; }
        public int skip { get; set; }
        public string insurance { get; set; }

        public double city_lat { get; set; }
        public double city_long { get; set; }
    }

    public class doc_search_mobile
    {
        public double lat { get; set; }
        public double longi { get; set; }

        public int take { get; set; }
        public int skip { get; set; }
    }

    public class doc_search_mobile_loguser
    {
        //public string name { get; set; }
        //public string firstname { get; set; }
        //public string lastname { get; set; }
        //public string npi { get; set; }
        //public string license { get; set; }
        //public string address { get; set; }
        //public string state { get; set; }
        //public string city { get; set; }
        //public string zipcode { get; set; }
        //public string acck { get; set; }
        public long specialty_id { get; set; }
        public long insurance_id { get; set; }
        public string condition { get; set; }

        public string search { get; set; }

        public double lat { get; set; }
        public double longi { get; set; }

        public int take { get; set; }
        public int skip { get; set; }

        //public int appt_type { get; set; }
        public long user_id { get; set; }
    }

    public class doc_search_query
    {
        // doctor_name, npi,license_no, address, state, city, zipcode

        //public string name { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string npi { get; set; }
        public double specialty { get; set; }
        public string license { get; set; }
        public string address { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }
        public string acck { get; set; }
        public string insurance { get; set; }

        public int take { get; set; }
        public int skip { get; set; }

        public double lat { get; set; }
        public double longi { get; set; }

        public int appt_type { get; set; }
        public long user_id { get; set; }
    }

    public class rec_notification
    {
        public long user_id { get; set; }
        public long patient_id { get; set; }
        public long appointment_id { get; set; }
        public string text { get; set; }
        public string link { get; set; }
        public bool is_unread { get; set; }
        public string date_created { get; set; }
    }

    public class patient_query
    {
        //patient_name, phone_number, birth_date, address, zipcode, emergency_contact, doctor_name

        public string firstname { get; set; }
        public string lastname { get; set; }
        public string phone { get; set; }
        public DateTime birthdate { get; set; }
        public string address { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }
        public string emergencycontact { get; set; }
        public string doctor { get; set; }
        public string acck { get; set; }

    }
    public class doc_profile
    {
        public long id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string middlename { get; set; }
        public string gender { get; set; }
        public string title { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string license { get; set; }
        public string npi { get; set; }
        public string npitype { get; set; }
        public string organization_name { get; set; }
        public string specialties { get; set; }
        public string image_url { get; set; }
        public long addr_id { get; set; }
        public string address { get; set; }
        //public string address2 { get; set; }
        public string pecos_certificate { get; set; }
        public string faxto { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public double city_lat { get; set; }
        public double city_long { get; set; }
        public string zipcode { get; set; }
        public string bio { get; set; }
        public string acck { get; set; }
    }

    public class doctor_specialty
    {
        public long id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string middle_name { get; set; }
        public double doctor_fee { get; set; }
        //public int favorite { get; set; }
        //public double rating { get; set; }
        public string bio { get; set; }
        //public string date { get; set; }
        //public string time_slot { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string title { get; set; }
        public string phone { get; set; }
        public string license { get; set; }
        public string pecos_certificate { get; set; }
        public string npi { get; set; }
        public string organization_name { get; set; }

        // doctor_name

        // image_name
        // specialty
        // appointment_type
        public string image_url { get; set; }
        //public string lat { get; set; }
        //public string longi { get; set; }
        //public string timeslot { get; set; }
        public List<appt_type> appointment_type { get; set; }
        public zip_search_address address { get; set; }
        // 01/11/2018 public List<doc_specialty2> specialties { get; set; }
        public List<doc_specialty_01112018> specialties { get; set; }

    }
    public class doc_search_profile
    {
        public long id { get; set; }
        public string npi { get; set; }
        public string organization_name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string middle_name { get; set; }
        public string gender { get; set; }
        public string title { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string license { get; set; }
        public double rating { get; set; }
        public double doctor_fee { get; set; }
        public int favorite { get; set; }
        public string time_slot { get; set; }
        public string image_url { get; set; }
        public string bio { get; set; }
        public List<doc_specialty> specialties { get; set; }
        public List<appt_type> appointment_type { get; set; }
        public zip_search_address address { get; set; }
        //public string npitype { get; set; }
        //public long addr_id { get; set; }
        //public string address1 { get; set; }
        //public string address2 { get; set; }
        public string pecos_certificate { get; set; }
        //public string faxto { get; set; }
        //public string city { get; set; }
        //public string state { get; set; }
        //public double city_lat { get; set; }
        //public double city_long { get; set; }
        //public string zipcode { get; set; }
        //public string acck { get; set; }
    }

    public class p_ratings_review {
        public double rating { get; set; }
        public string remark { get; set; }
    }
    public class doc_search_profile2
    {
        private long _id = 0;
        public long id { get { return this._id; } set { this._id = value; } }

        private string _npi = "";
        public string npi { get { return this._npi; } set { this._npi = value; } }

        private string _first_name = "";
        public string first_name { get { return this._first_name; } set { this._first_name = value; } }

        private string _last_name = "";
        public string last_name { get { return this._last_name; } set { this._last_name = value; } }

        private string _middle_name = "";
        public string middle_name { get { return this._middle_name; } set { this._middle_name = value; } }

        private string _DOB = "";
        public string DOB { get { return this._DOB; } set { this._DOB = value; } }

        private string _organization_name ="";
        public string organization_name { get { return this._organization_name; } set { this._organization_name = value; } }

        private string _gender = "";
        public string gender { get { return this._gender; } set { this._gender = value; } }

        private string _title = "";
        public string title { get { return this._title; } set { this._title = value; } }

        private string _email = "";
        public string email { get { return this._email; } set { this._email = value; } }

        private string _phone = "";
        public string phone { get { return this._phone; } set { this._phone = value; } }

        private string _license = "";
        public string license { get { return this._license; } set { this._license = value; } }

        private double _doctor_fee = 0;
        public double doctor_fee { get { return this._doctor_fee; } set { this._doctor_fee = value; } }

        private int _favorite = 0;
        public int favorite { get { return this._favorite; } set { this._favorite = value; } }

        
        private string _image_url = "";
        public string image_url { get { return this._image_url; } set { this._image_url = value; } }

        private string _bio = "";
        public string bio { get { return this._bio; } set { this._bio = value; } }

        //private double _rating = 0;
        //public double rating { get { return this._rating; } set { this._rating = value; } }
        private List<p_ratings_review> _p_review = new List<p_ratings_review>() { };
        public List<p_ratings_review> user_review { get { return this._p_review; } set { this._p_review = value; } }

        //public string npitype { get; set; }
        //public long addr_id { get; set; }
        //public string address1 { get; set; }
        //public string address2 { get; set; }

        private string _pecos_certificate = "";
        public string pecos_certificate { get { return this._pecos_certificate; } set { this._pecos_certificate = value; } }
        //public string faxto { get; set; }
        //public string city { get; set; }
        //public string state { get; set; }
        //public double city_lat { get; set; }
        //public double city_long { get; set; }
        //public string zipcode { get; set; }
        //public string acck { get; set; }

        private List<string> _time_slot = new List<string>() { };
        public List<string> time_slot { get { return this._time_slot; } set { this._time_slot = value; } }

        private List<string> _education = new List<string>() { };
        public List<string> education { get { return this._education; } set { this._education = value; } }

        private List<string> _experience = new List<string>() { };
        public List<string> experience { get { return this._experience; } set { this._experience = value; } }

       

        //01/11/2018 public List<doc_specialty2> specialties { get; set; }
        public List<doc_specialty_01112018> specialties { get; set; }

        private List<insurance> _network_insurance = new List<insurance>() { };
        public List<insurance> network_insurance { get { return this._network_insurance; } set { this._network_insurance = value; } }

        private List<doc_language> _language_spoken = new List<doc_language>() { };
        public List<doc_language> language_spoken { get { return this._language_spoken; } set { this._language_spoken = value; } }

        private List<appt_type> _appointment_type = new List<appt_type>() { };
        public List<appt_type> appointment_type { get { return this._appointment_type; } set { this._appointment_type = value; } }

        private List<zip_search_address> _home_address = new List<zip_search_address>() { };
        public List<zip_search_address> home_address { get { return this._home_address; } set { this._home_address = value; } }

        private List<zip_search_address> _practice_address = new List<zip_search_address>() { };
        public List<zip_search_address> practice_address { get { return this._practice_address; } set { this._practice_address = value; } }
    }


    public class rating_review
    {

        private double _rating = 0;
        public double rating { get { return this._rating; } set { this._rating = value; } }

        private List<p_ratings_review> _comment = new List<p_ratings_review>() { };
        public List<p_ratings_review> comment { get { return this._comment; } set { this._comment = value; } }
    }


    public class doctor_address {
        public zip_search_address home_address { get; set; }
        public zip_search_address practice_address { get; set; }
    }

    public class appt_type {
        public long id { get; set; }
        public string type { get; set; }
    }

    public class get_docProfile
    {
        public long id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string middlename { get; set; }
        public string gender { get; set; }
        public string title { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string license { get; set; }
        public string npi { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public double city_lat { get; set; }
        public double city_long { get; set; }
        public string zip { get; set; }
        public string bio { get; set; }

        //public bool pecoscert { get; set; }
        //public string faxto { get; set; }
        //public string npitype { get; set; }
        //public string orgname { get; set; }
        //public string specialties { get; set; }
        //public string image_url { get; set; }
        //public long addr_id { get; set; }
        //public string acck { get; set; }
    }



    public class doc_condition
    {
        //1/16/2018 public long id { get; set; }
        //public string code { get; set; }
        public string name { get; set; }
        //1/16/2018 public string description { get; set; }
        //public string actor { get; set; }
        //1/16/2018 public List<doc_specialty2> specialty { get; set; }
    }

    public class doc_specialty
    {
        //public long id { get; set; }
        //public string code { get; set; }
        //public string name { get; set; }
        //public string description { get; set; }
        //public string actor { get; set; }

        public long id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string provider_type { get; set; }
        public string specialization { get; set; }
        public List<doc_condition2> conditions { get; set; }
    }

    public class doc_specialty_01112018
    {
        //public long id { get; set; }
        //public string code { get; set; }
        //public string name { get; set; }
        //public string description { get; set; }
        //public string actor { get; set; }

        public long id { get; set; }
        // public string code { get; set; }
        // public string name { get; set; }
        public string provider_type { get; set; }
        public string classification_code { get; set; }
        public string classification { get; set; }
        public string specialization_code { get; set; }
        public string specialization { get; set; }
        public List<string> conditions { get; set; }
    }

    public class doc_condition2
    {
        public long id { get; set; }
        //public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        //public string actor { get; set; }
    }

    public class doc_specialty2
    {
        public long id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string provider_type { get; set; }
        public string specialization { get; set; }
        public List<string> conditions { get; set; }
    }

    public class doc_education
    {

        public string school { get; set; }
        public string graduation_year { get; set; }
        public string degree { get; set; }
    }

    public class doc_cities
    {
        //public long id { get; set; }
        public string name { get; set; }
    }

    public class doc_insurance
    {
        //public string plan_uid { get; set; }
        //public string plan_name { get; set; }
        //public string plan_category { get; set; }
        public string provider_id { get; set; }
        public string provider_name { get; set; }
    }
    public class doc_list
    {

        public doc_profile profile { get; set; }
        public List<doc_specialty> specialties { get; set; }
        public List<doc_education> educations { get; set; }
        public List<doc_insurance> insurances { get; set; }

    }

    public class patient_list
    {
        public patient_list()
        {
            profile = this.profile;
        }
        public pat_profile profile { get; set; }
    }

    public class p_patient
    {
        public long id { get; set; }
    }

    public class get_User
    {
        public long user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string social_id { get; set; }
        public string social_type { get; set; }
        public bool verified { get; set; }
        public string image_url { get; set; }
        //8/23 public string verification_code { get; set; }
        //8/23 public string status { get; set; }
        public string phone_no { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
        public string dob { get; set; }
        public string parent_guardian { get; set; }
        public string gender { get; set; }
        public double height { get; set; }
        public double weight { get; set; }
        public string emergency_number { get; set; }
        public string note { get; set; }
        public string insurance_id { get; set; }
        public string insurance_name { get; set; }

        //public string phash { get; set; }
        //public string tlas { get; set; }
        //public System.DateTime dt_create { get; set; }
        //public Nullable<System.DateTime> dt_update { get; set; }
        //public long create_by__USER_id { get; set; }
        //public Nullable<long> update_by__USER_id { get; set; }
        //public Nullable<System.DateTime> dt_lastin { get; set; }
        //public string url_lastin { get; set; }
        //public long rel_ref_status_id { get; set; }
        //public long rel_ref_usertype_id { get; set; }
        //public string token_current { get; set; }
        public List<user_secondary_patient> patient { get; set; }
    }

    public class get_Login
    {
        public long user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string social_id { get; set; }
        public string social_type { get; set; }
        public bool verified { get; set; }
        //8/23 public string verification_code { get; set; }
        //public string token { get; set; }
        public string image_url { get; set; }
        //8/23 public string status { get; set; }
        public string phone_no { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
        public string dob { get; set; }
        public string parent_guardian { get; set; }
        public string gender { get; set; }
        public double height { get; set; }
        public double weight { get; set; }
        public string emergency_number { get; set; }
        public string note { get; set; }
        public string insurance_id { get; set; }
        public string insurance_name { get; set; }
        public string user_type { get; set; }
        public List<user_secondary_patient> patient { get; set; }
    }

    public class get_Login2
    {
        // this is used in login/social
        public long user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string social_id { get; set; }
        public string social_type { get; set; }
        //public bool verified { get; set; }
        //public string verification_code { get; set; }
        public string image_url { get; set; }
        //8/23 public string status { get; set; }
        public string phone_no { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
        public string dob { get; set; }
        public string parent_guardian { get; set; }
        public string gender { get; set; }
        public double height { get; set; }
        public double weight { get; set; }
        public string emergency_number { get; set; }
        public string note { get; set; }
        public string insurance_id { get; set; }
        public string insurance_name { get; set; }

        public List<user_secondary_patient> patient { get; set; }
    }

    //public class user_patient
    //{
    //    public List<user_primary_patient> primary_patient { get; set; }
    //    public List<user_secondary_patient> secondary_patient { get; set; }
    //}

    public class get_param
    {
        public string name { get; set; }
        public string startwith { get; set; }
        public int take { get; set; }
        public int skip { get; set; }
    }

    public class search_city
    {
        public long id { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
    }

    public class user_secondary_patient
    {
        //public long sPatientId { get; set; }
        //public string sPatientFirstName { get; set; }
        //public string sPatientLastName { get; set; }
        //public bool is_prime { get; set; }

        //public bool isUsingPrimaryPatientInsurance { get; set; }
        //public string secondaryPatientInsuranceId { get; set; }
        //public string secondaryPatientInsuranceName { get; set; }

        public long id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public bool is_prime { get; set; }

        public bool is_using_primary_patient_insurance { get; set; }
        public string insurance_id { get; set; }
        public string insurance_name { get; set; }

        //public bool use_user_insurance { get; set; }
        //public string phone { get; set; }
        //public string address { get; set; }
        //public string city { get; set; }
        //public string state { get; set; }
        //public DateTime dob { get; set; }
        //public string parent_guardian {get;set;}
        //public double height { get; set; }
        //public double weight { get; set; }
        //public string emergency_number { get; set; }
        //public string note { get; set; }
    }

    public class user_secondary_patient2
    {
        public List<user_secondary_patient> patient { get; set; }
        public string zipcode { get; set; }
        public string insuranceid { get; set; }
        public string insurancename { get; set; }
    }
    public class user_primary_patient
    {
        public long pPatientId { get; set; }
        public string pPatientFirstName { get; set; }
        public string pPatientLastName { get; set; }
        //public bool is_prime { get; set; }

        //public bool use_user_insurance { get; set; }
        public bool isUsingPrimaryPatientInsurance { get; set; }
        public string secondaryPatientInsuranceId { get; set; }
        public string secondaryPatientInsuranceName { get; set; }

        //public string phone { get; set; }
        //public string address { get; set; }
        //public string city { get; set; }
        //public string state { get; set; }
        //public DateTime dob { get; set; }
        //public string parent_guardian {get;set;}
        //public double height { get; set; }
        //public double weight { get; set; }
        //public string emergency_number { get; set; }
        //public string note { get; set; }
    }

    public class profile
    {
  
        public string firstname { get; set; }
        public string lastname { get; set; }
    }
    public class pat_profile
    {
        //    public pat_profile() {
        //        id = this.id;
        //        firstname = this.firstname;
        //        lastname = this.lastname;
        //        homephone = this.homephone;
        //        workphone = this.workphone;
        //        guardian = this.guardian;

        //    // address
        //        rel_address_id = this.rel_address_id;
        //        address1 = this.address1;
        //        address2 = this.address2;
        //        // zip_id = this.zip_id;
        //        country_id = this.country_id;
        //        zipcode = this.zipcode;
        //        city = this.city;
        //        state = this.state;
        //        city_lat = this.city_lat;
        //        city_long = this.city_long;
        //        city_county = this.city_county;


        //        rel_address_id = this.rel_address_id;
        //        email = this.email;
        //        meta = this.meta;
        //        or_list_res_code = this.or_list_res_code;
        //        last_contact_by = this.last_contact_by;
        //        last_update = this.last_update;
        //        emergencyfirstname = this.emergencyfirstname; // Emergency Contact Name
        //        emergencylastname = this.emergencylastname; // Emergency Contact Name
        //        emergencyphone = this.emergencyphone; // Emergency Contact Phone
        //        emergencyrelationship = this.emergencyrelationship; // Emergency Contact Relationship

        //        // SOUL_info_3
        //        birthdate = this.birthdate;
        //        height = this.height;
        //        weight = this.weight;
        //        gender = this.gender;
        //        acck = this.acck;
        //}
        public long id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string homephone { get; set; }
        public string workphone { get; set; }
        public string guardian { get; set; }

        // address
        public long rel_address_id { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public long zip_id { get; set; }
        public long country_id { get; set; }
        public string zipcode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public double city_lat { get; set; }
        public double city_long { get; set; }
        public long city_county { get; set; }

        public string email { get; set; }
        public string meta { get; set; }
        public string or_list_res_code { get; set; }
        public long last_contact_by { get; set; }
        public string last_update { get; set; }
        public string emergencyfirstname { get; set; } // Emergency Contact Name
        public string emergencylastname { get; set; } // Emergency Contact Name
        public string emergencyphone { get; set; } // Emergency Contact Phone
        public string emergencyrelationship { get; set; } // Emergency Contact Relationship

        // SOUL_info_2
        public string birthdate { get; set; }
        public Double height { get; set; }
        public Double weight { get; set; }
        public string gender { get; set; }

        // SOUL_info_3

        public string acck { get; set; }
    }

    public class pat_profile2
    {

        public long id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string homephone { get; set; }
        public string workphone { get; set; }
        public string guardian { get; set; }

        // address
        public string address1 { get; set; }
        public string address2 { get; set; }
        public long zip_id { get; set; }
        public long country_id { get; set; }
        public string zipcode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public double city_lat { get; set; }
        public double city_long { get; set; }
        public long city_county { get; set; }

        public string email { get; set; }
        public string meta { get; set; }
        public string or_list_res_code { get; set; }
        public long last_contact_by { get; set; }
        public string last_update { get; set; }
        public string emergencyfirstname { get; set; } // Emergency Contact Name
        public string emergencylastname { get; set; } // Emergency Contact Name
        public string emergencyphone { get; set; } // Emergency Contact Phone
        public string emergencyrelationship { get; set; } // Emergency Contact Relationship

        // SOUL_info_2
        public string birthdate { get; set; }
        public Double height { get; set; }
        public Double weight { get; set; }
        public string gender { get; set; }

        // SOUL_info_3

    }

    public class zip_address
    {
        //public zip_address()
        //{
        //    id = this.id;
        //    address1 = this.address1;
        //    address2 = this.address2;
        //    ref_zip_id = this.ref_zip_id;
        //    zip = this.zip;
        //    city_name = this.city_name;
        //    city_state = this.city_state;
        //    city_lat = this.city_lat;
        //    city_lon = this.city_lon;
        //    city_country = this.city_country;

        //    minLat = this.minLat;
        //    minLon = this.minLon;
        //    maxLat = this.maxLat;
        //    maxLon = this.maxLon;
        //}

        public long id { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public int ref_zip_id { get; set; }
        public string zip { get; set; }
        public string city_name { get; set; }
        public string city_state { get; set; }
        public double city_lat { get; set; }
        public double city_lon { get; set; }
        public string city_country { get; set; }
        public double minLat { get; set; }
        public double minLon { get; set; }
        public double maxLat { get; set; }
        public double maxLon { get; set; }
    }

    public class zip_search_address
    {
        //public long id { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        //public int ref_zip_id { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string state_long { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public string county { get; set; }
    }

    public class zip_search_address2
    {
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string state_long { get; set; }
        //public double lat { get; set; }
        //public double lng { get; set; }
        //public string county { get; set; }
    }

    public class insurance_class
    {
        //public insurance_class()
        //{
        //    insurance_id = this.insurance_id;
        //    plan_name = this.plan_name;
        //    provider = this.provider;
        //    category = this.category;
        //    doc_id = this.doc_id;
        //    doc_first_name = this.doc_first_name;
        //    doc_last_name = this.doc_last_name;
        //    PayerID = this.PayerID;
        //    PayerName = this.PayerName;
        //}

        public long id { get; set; }
        public string plan_name { get; set; }
        public string provider { get; set; }
        public string category { get; set; }
        public int doc_id { get; set; }
        public string doc_first_name { get; set; }
        public string doc_last_name { get; set; }
        public string PayerID { get; set; }
        public string PayerName { get; set; }
    }

    public class item_class
    {

        //public item_class()
        //{
        //    id = this.id;
        //    name = this.name;
        //    code = this.code;
        //    description = this.description;
        //}
        public long id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }

    //public class appt_id
    //{
    //    public long appointment_id { get; set; }
    //}

    public class schedule
    {
        public long doctor_id { get; set; }
        //public List<string> date { get; set; }
        public List<slot1> timeslot { get; set; }
    }



    public class timeslot
    {
        public string time { get; set; }
    }




    public class all_Appointment
    {
        public List<new_APPOINTMENT> current = new List<new_APPOINTMENT>();
        public List<new_APPOINTMENT> history = new List<new_APPOINTMENT>();
    }

    public class new_APPOINTMENT
    {
     
        public long appointment_id { get; set; }
        public long patient_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string date { get; set; }
        public string time_slot { get; set; }
        public string status { get; set; }

        //public string appointment { get; set; }
        public string date_created { get; set; }
        public long user_id { get; set; }
        //public string doctor_name { get; set; }
        public List<appt_type> appointment_type { get; set; }
        public long doctor_id { get; set; }
        public string doctor_name { get; set; }
        public double rating { get; set; }
        public List<doctor_profile> doctor { get; set; }
        //public List<proposed_doctor> proposed_doctor { get; set; }
        //public string specialty { get; set; }
        //public string doctor_image { get; set; }
    }

    public class dashboard_Appointment
    {

        public long appointment_id { get; set; }
        public long patient_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string date { get; set; }
        public string time_slot { get; set; }
        public string status { get; set; }

        //public string appointment { get; set; }
        public string date_created { get; set; }
        public long user_id { get; set; }
        //public string doctor_name { get; set; }
        //public List<appt_type> appointment_type { get; set; }
        //public long doctor_id { get; set; }
        //public string doctor_name { get; set; }
        //public double rating { get; set; }
        //public List<doctor_profile> doctor { get; set; }
        //public List<proposed_doctor> proposed_doctor { get; set; }
        //public string specialty { get; set; }
        //public string doctor_image { get; set; }

    }

    public class doctor_dashboard
    {
        public List<dashboard_Appointment> pending { get; set; }
        public List<dashboard_Appointment> current { get; set; }
        public List<dashboard_Appointment> marketplace { get; set; }
    }

    //public class doctor_specialty
    //{
    //    public long id { get; set; }
    //    public string first_name { get; set; }
    //    public string last_name { get; set; }
    //    public string middle_name { get; set; }
    //    public double doctor_fee { get; set; }
    //    //public int favorite { get; set; }
    //    //public double rating { get; set; }
    //    public string bio { get; set; }
    //    //public string date { get; set; }
    //    //public string time_slot { get; set; }
    //    public string email { get; set; }
    //    public string gender { get; set; }
    //    public string title { get; set; }
    //    public string phone { get; set; }
    //    public string license { get; set; }
    //    public bool pecoscert { get; set; }
    //    public string npi { get; set; }
    //    public string orgname { get; set; }

    //    // doctor_name

    //    // image_name
    //    // specialty
    //    // appointment_type
    //    public string image_url { get; set; }
    //    //public string lat { get; set; }
    //    //public string longi { get; set; }
    //    //public string timeslot { get; set; }
    //    public List<appt_type> appointment_type { get; set; }
    //    public zip_search_address address { get; set; }
    //    public List<doc_specialty2> specialties { get; set; }

    //}

    public class doctor_profile
    {
        public long id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string middle_name { get; set; }
        public double doctor_fee { get; set; }
        public int favorite { get; set; }
        public double rating { get; set; }
        public string bio { get; set; }
        public string date { get; set; }
        public string time_slot { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string title { get; set; }
        public string phone { get; set; }
        public string license { get; set; }
        public string pecos_certificate { get; set; }
        public string npi { get; set; }
        public string organization_name { get; set; }

        // doctor_name

        // image_name
        // specialty
        // appointment_type
        public string image_url { get; set; }
        //public string lat { get; set; }
        //public string longi { get; set; }
        //public string timeslot { get; set; }
        public List<appt_type> appointment_type { get; set; }
        //public zip_search_address address { get; set; }
        public List<zip_search_address> home_address { get; set; }
        public List<zip_search_address> practice_address { get; set; }
        //01/11/2018 public List<doc_specialty2> specialties { get; set; }
        public List<doc_specialty_01112018> specialties { get; set; }

    }

    public class proposed_doctor
    {
        public long id { get; set; }
        public long doctor_id { get; set; }

        public string first_name { get; set; }
        public string last_name { get; set; }
        public string middle_name { get; set; }
        public double doctor_fee { get; set; }
        public int favorite { get; set; }
        public double rating { get; set; }
        public string bio { get; set; }

        public string date { get; set; }
        public string time_slot { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string title { get; set; }
        public string phone { get; set; }
        public string license { get; set; }
        public bool pecoscert { get; set; }
        public string npi { get; set; }
        public string organization_name { get; set; }

        public string image_url { get; set; }
        public List<appt_type> appointment_type { get; set; }
        public zip_search_address address { get; set; }
        public List<doc_specialty> specialties { get; set; }
    }

    //public class appt_id
    //{
    //    public long appointment_id { get; set; }
    //}

    //public class schedule
    //{
    //    public long doctor_id { get; set; }
    //    //public List<string> date { get; set; }
    //    public List<slot1> timeslot { get; set; }
    //}

    public class slot
    {
        public string date { get; set; }
        public List<timeslot> time { get; set; }
    }
    public class slot1
    {
        public string date { get; set; }
        public string time { get; set; }
    }


    //public class timeslot
    //{
    //    public string time { get; set; }
    //}



    //public class all_Appointment
    //{
    //    public List<new_APPOINTMENT> current = new List<new_APPOINTMENT>();
    //    public List<new_APPOINTMENT> history = new List<new_APPOINTMENT>();
    //}

    //public class new_APPOINTMENT
    //{
    //    public long appointment_id { get; set; }
    //    public long patient_id { get; set; }
    //    public string first_name { get; set; }
    //    public string last_name { get; set; }
    //    public string date { get; set; }
    //    public string time_slot { get; set; }
    //    public string status { get; set; }

    //    //public string appointment { get; set; }
    //    public string date_created { get; set; }
    //    public long user_id { get; set; }
    //    //public string doctor_name { get; set; }
    //    public List<appt_type> appointment_type { get; set; }
    //    public long doctor_id { get; set; }
    //    public string doctor_name { get; set; }
    //    public double rating { get; set; }
    //    public List<doctor_profile> doctor { get; set; }
    //    //public List<proposed_doctor> proposed_doctor { get; set; }
    //    //public string specialty { get; set; }
    //    //public string doctor_image { get; set; }
    //}

    //public class dashboard_Appointment
    //{

    //    public long appointment_id { get; set; }
    //    public long patient_id { get; set; }
    //    public string first_name { get; set; }
    //    public string last_name { get; set; }
    //    public string date { get; set; }
    //    public string time_slot { get; set; }
    //    public string status { get; set; }

    //    //public string appointment { get; set; }
    //    public string date_created { get; set; }
    //    public long user_id { get; set; }
    //    //public string doctor_name { get; set; }
    //    //public List<appt_type> appointment_type { get; set; }
    //    //public long doctor_id { get; set; }
    //    //public string doctor_name { get; set; }
    //    //public double rating { get; set; }
    //    //public List<doctor_profile> doctor { get; set; }
    //    //public List<proposed_doctor> proposed_doctor { get; set; }
    //    //public string specialty { get; set; }
    //    //public string doctor_image { get; set; }

    //}

    //public class doctor_dashboard
    //{
    //    public List<dashboard_Appointment> pending { get; set; }
    //    public List<dashboard_Appointment> current { get; set; }
    //    public List<dashboard_Appointment> marketplace { get; set; }
    //}

    //public class doctor_specialty
    //{
    //    public long id { get; set; }
    //    public string first_name { get; set; }
    //    public string last_name { get; set; }
    //    public string middle_name { get; set; }
    //    public double doctor_fee { get; set; }
    //    //public int favorite { get; set; }
    //    //public double rating { get; set; }
    //    public string bio { get; set; }
    //    //public string date { get; set; }
    //    //public string time_slot { get; set; }
    //    public string email { get; set; }
    //    public string gender { get; set; }
    //    public string title { get; set; }
    //    public string phone { get; set; }
    //    public string license { get; set; }
    //    public bool pecoscert { get; set; }
    //    public string npi { get; set; }
    //    public string orgname { get; set; }

    //    // doctor_name

    //    // image_name
    //    // specialty
    //    // appointment_type
    //    public string image_url { get; set; }
    //    //public string lat { get; set; }
    //    //public string longi { get; set; }
    //    //public string timeslot { get; set; }
    //    public List<appt_type> appointment_type { get; set; }
    //    public zip_search_address address { get; set; }
    //    public List<doc_specialty2> specialties { get; set; }

    //}

    //public class doctor_profile
    //{
    //    public long id { get; set; }
    //    public string first_name { get; set; }
    //    public string last_name { get; set; }
    //    public string middle_name { get; set; }
    //    public double doctor_fee { get; set; }
    //    public int favorite { get; set; }
    //    public double rating { get; set; }
    //    public string bio { get; set; }
    //    public string date { get; set; }
    //    public string time_slot { get; set; }
    //    public string email { get; set; }
    //    public string gender { get; set; }
    //    public string title { get; set; }
    //    public string phone { get; set; }
    //    public string license { get; set; }
    //    public bool pecoscert { get; set; }
    //    public string npi { get; set; }
    //    public string orgname { get; set; }

    //    // doctor_name

    //    // image_name
    //    // specialty
    //    // appointment_type
    //    public string image_url { get; set; }
    //    //public string lat { get; set; }
    //    //public string longi { get; set; }
    //    //public string timeslot { get; set; }
    //    public List<appt_type> appointment_type { get; set; }
    //    public zip_search_address address { get; set; }
    //    public List<doc_specialty2> specialties { get; set; }

    //}

    //public class proposed_doctor
    //{
    //    public long id { get; set; }
    //    public long doctor_id { get; set; }

    //    public string first_name { get; set; }
    //    public string last_name { get; set; }
    //    public string middle_name { get; set; }
    //    public double doctor_fee { get; set; }
    //    public int favorite { get; set; }
    //    public double rating { get; set; }
    //    public string bio { get; set; }

    //    public string date { get; set; }
    //    public string time_slot { get; set; }
    //    public string email { get; set; }
    //    public string gender { get; set; }
    //    public string title { get; set; }
    //    public string phone { get; set; }
    //    public string license { get; set; }
    //    public bool pecoscert { get; set; }
    //    public string npi { get; set; }
    //    public string orgname { get; set; }

    //    public string image_url { get; set; }
    //    public List<appt_type> appointment_type { get; set; }
    //    public zip_search_address address { get; set; }
    //    public List<doc_specialty> specialties { get; set; }
    //}

    public class doctor_save
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string device_token { get; set; }
    }

    public class patient
    {
        public long patient_id { get; set; }
        //public string firstname { get; set; }
        //public string lastname { get; set; }
        public List<_insurance> insurance { get; set; }
    }

    public class _insurance
    {
        public long id { get; set; }
        public string provider { get; set; }
    }

    public class img_url
    {
        public string image_url { get; set; }
    }

    public class pat_info
    {
        public List<patient_APPOINTMENT> appointment { get; set; }
        public List<doctor_profile> doctor { get; set; }
    }

    public class g_patient
    {
        //public long patientId { get; set; }
        //public string firstName { get; set; }
        //public string lastName { get; set; }
        //public string userName { get; set; }
        //public string password { get; set; }
        //public string socialId { get; set; }
        //public string socialType { get; set; }
        //public string verified { get; set; }

        //public string phoneNo { get; set; }
        //public string street { get; set; }
        //public string city { get; set; }
        //public string state { get; set; }
        //public string zipCode { get; set; }
        //public string dob { get; set; }
        //public string parentGuardian { get; set; }
        //public string gender { get; set; }
        //public string height { get; set; }
        //public string weight { get; set; }
        //public string emergencyNumber { get; set; }
        //public string about { get; set; }
        //public string insuranceId { get; set; }
        //public string insuranceName { get; set; }
        //public string imageURL { get; set; }

        ////public List<_address> address { get; set; } 
        ////public string address { get; set; }
        ////public string city { get; set; }

        public long id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public bool is_prime { get; set; }
        public long insurance_id { get; set; }
        public string insurance_name { get; set; }
        //public bool user_insurance { get; set; }
        public bool is_using_primary_patient_insurance { get; set; }
        public string image { get; set; }
        public string status { get; set; }

        ////public string gender { get; set; }
        ////public string salutation { get; set; }
        ////public string create_by { get; set; }
        ////public string update_by { get; set; }
        ////public string create_date { get; set; }
        ////public string update_date { get; set; }
        //public string status { get; set; }
    }

    public class u_patient
    {
        public long patient_id { get; set; }
        public long user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public long insurance_id { get; set; }
        public string is_using_primary_patient_insurance { get; set; }
        public string is_prime { get; set; }
        public string status { get; set; }
        public string dob { get; set; }
        public string device_type { get; set; }
        public string device_token { get; set; }
    }
    public class _address
    {

        //public string zip { get; set; }
        public string address { get; set; }
        //public double lat { get; set; }
        //public double longi { get; set; }
        public string city { get; set; }
        //public string state { get; set; }

    }

    public class patient_APPOINTMENT
    {
        public long appointment_id { get; set; }
        public long patient_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string date { get; set; }
        public string time_slot { get; set; }
        public string status { get; set; }
        public string date_created { get; set; }
        public long user_id { get; set; }
        public List<appt_type> appointment_type { get; set; }
        public long doctor_id { get; set; }
        public string doctor_name { get; set; }
        public double rating { get; set; }
        //public List<doctor_profile> doctor { get; set; }

    }

    public class card_Info
    {
        public string card_number { get; set; }
        public string cvv { get; set; }
        public string expiry_date { get; set; }
        public string payment_date { get; set; }
        public payment_History payment_history { get; set; }
    }
    public class payment_History
    {
        public double amount { get; set; }
        public string payment_date { get; set; }
    }

    public class doc_rate2
    {
        public doc_rate rate { get; set; }
    }
    public class doc_rate
    {
        //public long id { get; set; }
        public long appointment_id { get; set; }
        //public string firstname { get; set; }
        //public string lastname { get; set; }
        public string doctor_id { get; set; }
        public List<doctor_specialty> doctor { get; set; }
        public string rating { get; set; }
        public string review { get; set; }
    }

    //public class id 
    public class pat_rate
    {
        public long id { get; set; }
        public long appointment_id { get; set; }
        //public string firstname { get; set; }
        //public string lastname { get; set; }
        public long patient_id { get; set; }
        public string rating { get; set; }
    }


    public class pat_rate2
    {
        public long patient_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }

        public string rating { get; set; }
    }

  

    public class var_getDoctor_ext
    {
        public double doc_fee { get; set; }
       //01/11/2018 public List<doc_specialty2> spec { get; set; }
       public List<doc_specialty_01112018> spec { get; set; }
        public List<appt_type> appt_type { get; set; }
        public List<doc_language> language { get; set; }
        public List<insurance> insurance { get; set; }
        public List<string> time_slot { get; set; }
        public List<string> education { get; set; }
        public List<string> experience { get; set; }
        public string dea { get; set; }
    }

    public class var_getDoctor_address
    {
        public List<zip_search_address> home_address { get; set; }
        public List<zip_search_address> practice_address { get; set; }
    }

    public class var_getDoctorClaim_ext
    {
        public double doc_fee { get; set; }
        //01/11/2018 public List<doc_specialty2> spec { get; set; }
        public List<doc_specialty_01112018> spec { get; set; }
        public List<appt_type> appt_type { get; set; }
        public List<zip_search_address> home_address { get; set; }
        public List<zip_search_address> practice_address { get; set; }
        public List<doc_language> language { get; set; }
        public List<insurance> insurance { get; set; }
        public string time_slot { get; set; }
        //public string education { get; set; }
        //public string experience { get; set; }
        public string board_certification { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string personal_practice_type { get; set; }
        public string education { get; set; }
        public string experience { get; set; }
        public string hospital_affiliation { get; set; }
        public string practice_npi { get; set; }
        public string practice_name { get; set; }
        public string practice_type { get; set; }
        public string dea { get; set; }
        public string clinician_role { get; set; }
        public string scheduling_solution { get; set; }
        public string current_scheduling { get; set; }
        public string practice_street { get; set; }
        public string practice_phone_primary { get; set; }
        public string practice_fax { get; set; }
        public string practice_phone_cs { get; set; }
        public string practice_phone_office { get; set; }
        public string practice_clinicians { get; set; }
        public string practice_exams { get; set; }
        public string geographic_market { get; set; }
        public string practice_expansion { get; set; }
        public string practice_insurance { get; set; }
        public string practice_tax_number { get; set; }
        public string primary_contact_name { get; set; }
        public string primary_contact_phone { get; set; }
        public string primary_contact_email { get; set; }
        public string operational_contact_name { get; set; }
        public string operational_contact_phone { get; set; }
        public string operational_contact_email { get; set; }
        public string financial_contact_name { get; set; }
        public string financial_contact_phone { get; set; }
        public string financial_contact_email { get; set; }
        public string practice_emr { get; set; }
        public string network_insurance { get; set; }


        public string billing_bankname { get; set; }
        public string billing_account { get; set; }
        public string billing_routing { get; set; }
    }

    public class param_getall
    {
        public string doctor_name { get; set; }
        public string practice_name { get; set; }
        public string city { get; set; }
        //public string state { get; set; }
        public string condition_id { get; set; }
        public string condition { get; set; } // 1/16/2018
        public string specialty_id { get; set; }
        public string language_id { get; set; }
        public string hospital_affiliation { get; set; }
        public string insurance_id { get; set; }

        public int take { get; set; }
        public int skip { get; set; }
    }

    public class param_directory
    {
        //public string letter { get; set; }
        public string startwith { get; set; }
        public string name { get; set; }
        public bool is_date { get; set; }
        public int take { get; set; }
        public int skip { get; set; }
    }

    public class param_practicename
    {
        public string practice_name { get; set; }
        public bool recent { get; set; }
    }

    public class ref_spec_appt_type
    {
        public List<user_secondary_patient> patient = new List<user_secondary_patient>();
        public List<ref_appt_type> appointment_type { get; set; }
        // public List<doc_specialty> specialty { get; set; }
        public List<doc_specialty_01112018> specialty { get; set; }
    }
    public class ref_spec
    {
        public long id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string actor { get; set; }
        public string category { get; set; }
    }

    public class ref_appt_type
    {
        public long id { get; set; }
        public string appointment_type { get; set; }
    }

    class proxi_doc
    {
        public long id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public List<spec_address> address { get; set; }
    }

    public class spec_address
    {
        public string address { get; set; }
        public string city { get; set; }
        public string county { get; set; }
        public string state { get; set; }
        public double lat { get; set; }
        public double longi { get; set; }
    }

    public class specPerDoctor
    {
        public long id { get; set; }
        public long docId { get; set; }
        //public string firstname { get; set; }
        //public string lastname { get; set; }
        //public string middlename { get; set; }
        public long specialty_id { get; set; }
        public string specialty_name { get; set; }
        public string specialty_description { get; set; }
    }

    public class doctor_user
    {
        public long id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int status { get; set; }
    }

    public class patient_user
    {
        public long id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int status { get; set; }
    }

    public class user_info
    {
        public long user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        //public string user_type { get; set; }
        public string password { get; set; }
        public string social_id { get; set; }
        public string social_type { get; set; }
        //public string verification_code { get; set; }
        public bool verified { get; set; }

        public string phone_no { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
        public string dob { get; set; }
        public string parent_guardian { get; set; }
        public string gender { get; set; }
        public string height { get; set; }
        public string weight { get; set; }
        public string emergency_number { get; set; }
        public string about { get; set; }
        public string note { get; set; }
        public string insurance_id { get; set; }
        public string insurance_name { get; set; }
        public string image_url { get; set; }

        public List<user_secondary_patient> patient { get; set; }

        //public string insurance_type { get; set; }
        //public string device_type { get; set; }
        //public string phone_number { get; set; }
        //public string address { get; set; }
        //public string city { get; set; }
        //public DateTime dob { get; set; }
        //public string parent_guardian { get; set; }
        //public double height { get; set; }
        //public double weight { get; set; }
        //public string emergency_number { get; set; }
        //public string note { get; set; }
        //public string status { get; set; }
    }

    public class patient_info
    {
        public long patient_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        //public string user_type { get; set; }
        public string image { get; set; }
        public string email { get; set; }
        public bool is_prime { get; set; }

        //public string insurance_type { get; set; }
        //public string device_type { get; set; }
        //public string phone_number { get; set; }
        //public string address { get; set; }
        //public string city { get; set; }
        //public DateTime dob { get; set; }
        //public string parent_guardian { get; set; }
        //public double height { get; set; }
        //public double weight { get; set; }
        //public string emergency_number { get; set; }
        //public string note { get; set; }
        public string status { get; set; }
    }

    public class param_user
    {
        public string username { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string password { get; set; }
        public string device_type { get; set; }
        public string device_token { get; set; }
        public string platform { get; set; }
        public string phone_number { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string dob { get; set; }
        public string parent_guardian { get; set; }
        public double height { get; set; }
        public double weight { get; set; }
        public string emergency_number { get; set; }
        public string gender { get; set; }
        public string note { get; set; }
    }

    public class u_user
    {
        public long user_id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string status { get; set; }
        public string phone_no { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string parent_guardian { get; set; }
        public double height { get; set; }
        public double weight { get; set; }
        public string emergency_number { get; set; }
        public string note { get; set; }
        public string zip_code { get; set; }
        public long insurance_id { get; set; }
        public string about { get; set; }
    }

    public class wellness_exam1
    {
        public long exam_id { get; set; }
        public string exam_title { get; set; }
    }

    public class wellness_exam2
    {
        public long exam_id { get; set; }
        public string exam_title { get; set; }
    }

    public class exam_taken
    {
        public long patient_id { get; set; }
        public List<wellness_exam2> wellness { get; set; }
    }

    public class topsearch
    {
        public string display_value { get; set; }
        public string url { get; set; }
        public int count { get; set; }
    }

    public class get_appointment_ext {
        public long doctor_id { get; set; }
        public string time_slot { get; set; }
        public double doctor_rating { get; set; }
        public List<doctor_profile> proposed_doctor { get; set; }
    }

    public class get_appointment_doctor
    {
        public List<doctor_profile> doctor { get; set; }
        public List<appt_type> doc_appt { get; set; }
        public string time_slot { get; set; }
        public double doc_fee { get; set; }
        public string doctor_name { get;  set; }
        public int is_favorite { get; set; } 
    }
}