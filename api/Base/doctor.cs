using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using api.Models;

namespace api.Base 
{
    public class doctor : Base.BaseController
    {

    
      

        public List<doc_search_profile2> getMobileDoctor(dynamic doc_items)
        {
            try
            {
                // balkon



                List<doc_search_profile2> dc = new List<doc_search_profile2>();

                List<long> doc_id = new List<long>();
                foreach (var doc in doc_items)
                {
                 
                    List<zip_search_address> home_addr = _getDoctor_homeaddress_doc1(doc);
                    List<zip_search_address> pract_addr = _getDoctor_practiceaddress_doc1(doc);

                    // bozal
                    var_getDoctor_ext d_ext = _getDoctor_ext_doc1(doc.ext);
                    //01/11/2018 List<doc_specialty2> spec = _getDoctor_specialty_doc1(doc.con_spec);
                    List<doc_specialty_01112018> spec = _getDoctor_specialty_doc1(doc.con_spec);
                    List<doc_language> lang = _getLanguage_doc1(doc.con_lang);
                    List<insurance> ins = _getInsurance_doc1(doc.con_ins);
                    getDoctor_rating dr = _get_averagerating_doc1(doc.id);

                    var prof = new Models.doc_search_profile2
                    {
                        id = doc.id,
                        npi = doc.NPI == null ? "" : doc.NPI,
                        first_name = doc.name_first == null ? "" : doc.name_first,
                        last_name = doc.name_last == null ? "" : doc.name_last,
                        middle_name = doc.name_middle == null ? "" : doc.name_middle,
                        email = doc.email == null ? "" : doc.email,
                        gender = doc.gender == null ? "" : doc.gender.Trim().ToUpper(),
                        title = doc.title == null ? "" : doc.title,
                        phone = doc.phone == null ? "" : doc.phone,
                        license = doc.license_no == null ? "" : doc.license_no,
                      
                        organization_name = doc.organization_name == null ? "" : doc.organization_name,
                        image_url = doc.image_url == null ? "" : doc.image_url,
                        DOB = doc.dob == null ? "" : doc.dob,
                        bio = doc.bio,
                        pecos_certificate = doc.pecos_certification,

                        //balkon favorite = dr.fave,
                        //balkon rating = dr.average_rating,
                        //balkon patient_review = dr.review,

                        doctor_fee = d_ext.doc_fee,
                        time_slot = d_ext.time_slot,

                        education = d_ext.education,
                        experience = d_ext.experience,
                        appointment_type = d_ext.appt_type,


                        network_insurance = ins, // d_ext.insurance == null ? new List<insurance>() { } : d_ext.insurance,
                        language_spoken = lang, // d_ext.language == null ? new List<doc_language>() { } : d_ext.language,
                        specialties = spec, // d_ext.spec == null ? new List<doc_specialty2>() { } : d_ext.spec,


                        home_address = home_addr == null ? new List<zip_search_address>() { } : home_addr,
                        practice_address = pract_addr == null ? new List<zip_search_address>() { } : pract_addr
                    };
                    dc.Add(prof);
                    //}
                }

                return dc;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        public List<zip_search_address> _getDoctor_homeaddress_doc1(dynamic doc)
        {
            // temporary for table DOCTOR1

            //string zipcode = value.Substring(0, 5).ToString();
            //var addr = dbEntity.ref_zip.Where(b => b.zip == zipcode);

            List<zip_search_address> addr = new List<zip_search_address>();



            if (doc.home_zip != null || !string.IsNullOrEmpty(doc.home_addr_1))
            {
                // http://localhost:64625/doctor/profile?npi=1477647402
                ////var ref_zip = dbEntity.ref_zip.Find(doc.home_addr_zip_id); // 606 in 1m
                //var ref_zip = ref_zip2.ref_zip1.Where(a => a.id == doc.home_addr_zip_id).FirstOrDefault();
                //bool sref = ref_zip == null;
                //addr.Add(new zip_search_address
                //{
                //    address1 = doc.home_addr_1 == null ? "" : doc.home_addr_1,
                //    address2 = doc.home_addr_2 == null ? "" : doc.home_addr_2,
                //    zip = sref ? "" : ref_zip.zip,
                //    city = sref ? "" : ref_zip.city_name,
                //    state = sref ? "" : ref_zip.city_state,
                //    state_long = sref ? "" : ref_zip.city_state_long,
                //    lat = sref ? 0 : ref_zip.city_lat,
                //    lng = sref ? 0 : ref_zip.city_lon,
                //    county = sref ? "" : ref_zip.city_county
                //});
                addr.Add(new zip_search_address
                {
                    address1 = doc.home_addr_1 == null ? "" : doc.home_addr_1,
                    address2 = doc.home_addr_2 == null ? "" : doc.home_addr_2,
                    zip = doc.home_zip == null ? "" : doc.home_zip,
                    city = doc.home_city == null ? "" : doc.home_city,
                    state = doc.home_state == null ? "" : doc.home_state,
                    state_long = doc.home_state_long == null ? "" : doc.home_state_long,
                    lat = doc.home_lat == null ? 0 : doc.home_lat,
                    lng = doc.home_long == null ? 0 : doc.home_long,
                    county = doc.home_county == null ? "" : doc.home_county
                });
            }


            return addr;
        }


        public List<zip_search_address> _getDoctor_practiceaddress_doc1(dynamic doc)
        {
            // temporary for table DOCTOR1

            //string zipcode = value.Substring(0, 5).ToString();
            //var addr = dbEntity.ref_zip.Where(b => b.zip == zipcode);

            List<zip_search_address> addr = new List<zip_search_address>();



            //if (doc.practice_addr_zip_id != null || !string.IsNullOrEmpty(doc.practice_addr_1))
            if (!string.IsNullOrEmpty(doc.pract_zip))
            {
                // var ref_zip = dbEntity.ref_zip.Find(doc.practice_addr_zip_id);
                // pract_zip = d.ref_zip.zip, pract_state = zip1.city_state, pract_state_long = zip1.city_state_long,   pract_lat = zip1.city_lat,
                //    pract_long = zip1.city_lon,      pract_city = zip1.city_name,        pract_county = zip1.city_county

                //pract_addr_1 = d.practice_addr_1, pract_addr_2 = d.practice_addr_2,
                addr.Add(new zip_search_address
                {
                    address1 = doc.pract_addr_1 == null ? "" : doc.pract_addr_1,
                    address2 = doc.pract_addr_2 == null ? "" : doc.pract_addr_2,
                    zip = doc.pract_zip,
                    city = doc.pract_city,
                    state = doc.pract_state,
                    state_long = doc.pract_state_long,
                    lat = doc.pract_lat,
                    lng = doc.pract_long,
                    county = doc.pract_county
                });

            }



            return addr;
        }


        public var_getDoctor_ext _getDoctor_ext_doc1(dynamic doc_ext)
        {
            try
            {
                //SV_db1Entities dbEntity = new SV_db1Entities();
                //var doc_ext = dbEntity.hs_DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doc.id);

                var_getDoctor_ext dx = new var_getDoctor_ext();

                List<appt_type> m_app_type = new List<appt_type>();
                List<string> m_time_slot = new List<string>() { };
                List<string> m_education = new List<string>() { };
                List<string> m_experience = new List<string>() { };                // referenced by:  /doctor/search
                foreach (var n in doc_ext)
                {
                    string e = n.ext_attr_name;
                    string v = n.ext_value;
                    switch (e)
                    {
                        case "language_spoken":
                            // removed from doctor_ext
                            break;
                        case "fee":
                            double d_fee;
                            bool bTemp = double.TryParse(v, out d_fee);

                            dx.doc_fee = d_fee;
                            break;
                        case "specialty_id":
                            // removed from doctor_ext
                            break;
                        case "home_zip":

                            //dx.home_address = _getDoctor_homeaddress(n.value);

                            break;
                        case "practice_zip":

                            //dx.practice_address = _getDoctor_practiceaddress(doctor_id, n.value);
                            break;


                        case "drappttype":
                            // dx.appt_type = _getDoctor_appointmenttype(n.value);
                            long vappt_id = 0;
                            bool isAppt = long.TryParse(v, out vappt_id);
                            if (isAppt)
                            {
                                appt_type appt_ = new appt_type();
                                appt_ = _getDoctor_appointmenttype(vappt_id);
                                //dx.appt_type.Add(new appt_type { id = appt_.id, type = appt_.type });
                                m_app_type.Add(new appt_type { id = appt_.id, type = appt_.type });
                            }
                            break;

                        case "time_slot":
                            m_time_slot.Add(v);
                            break;

                        case "education":
                            m_education.Add(v);
                            break;
                        case "experience":
                            m_experience.Add(v);
                            break;
                        case "dea":
                            dx.dea = v;
                            break;
                    }

                    //public List<string> time_slot { get; set; }
                    //public List<string> education { get; set; }
                    //public List<string> experience { get; set; }

                }

                // it wud yield an error, if class variable is assigned inside switch()
                dx.education = m_education;
                dx.time_slot = m_time_slot;
                dx.appt_type = m_app_type;
                dx.experience = m_experience;
                //if (m_education.Count() > 0) {  }
                //if (m_time_slot.Count() > 0) {  }
                //if (m_app_type.Count() > 0) { }

                return dx;
            }
            catch (Exception ex)
            {
                return null;
            }


        }



        /// <summary>
        /// Get appointment type per Doctor.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public appt_type _getDoctor_appointmenttype(long value)
        {
            SV_db1Entities dbEntity = new SV_db1Entities();

            appt_type appt_type = new appt_type();

            // long vappt_id = 0;
            // bool isAppt = long.TryParse(value, out vappt_id);

            var n1 = dbEntity.ref_APPOINTMENT_type.Find(value);
            string vappt_name = n1.dname;
            long vappt_id = n1.id;

            //appt_type.Add(new appt_type
            //{
            //    id = vappt_id,
            //    type = vappt_name
            //});
            appt_type.id = vappt_id;
            appt_type.type = vappt_name;


            return appt_type;

        }

        public List<doc_specialty_01112018> _getDoctor_specialty_doc1(dynamic con_spec)
        {
            try
            {
                SV_db1Entities dbEntity = new SV_db1Entities();
                //01/11/2018 List<doc_specialty2> spec = new List<doc_specialty2>();
                List<doc_specialty_01112018> spec = new List<doc_specialty_01112018>();
                //var con_spec = dbEntity.con_DOCTOR1_ref_specialty.Where(a => a.rel_DOCTOR_id == doctor_id);

                foreach (var i in con_spec)
                {


                    // 01/11/2018
                    List<string> condition = new List<string>();
                    foreach (var c in i.spec_condition)
                    {
                        //string[] con = i.spec_condition.Split('|');
                        //foreach (var c in con)
                        //{
                        if(!string.IsNullOrEmpty(c))
                            condition.Add(c);
                    //}
                    }
                

                    // 01/11/2018
                    spec.Add(new doc_specialty_01112018
                    {
                        // id = n.ref_specialty.id,
                        // description = n.ref_specialty.description,
                        // name = n.ref_specialty.name,
                        // actor = n.ref_specialty.actor == null ? "" : n.ref_specialty.actor

                        id = i.spec_id,
                        provider_type = i.spec_provider_type,//  s.provider_type,

                        classification_code = i.spec_classification_code,
                        classification = i.spec_classification,

                        specialization_code = i.spec_specialization_code == null? "" : i.spec_specialization_code,
                        specialization = i.spec_specialization == null?"" : i.spec_specialization, // s.specialization,
                        conditions = condition
                    });
                



            }

                return spec;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        // created: 01/11/2018
        public List<doc_specialty_01112018> getSpecialty(string code)
        {
            SV_db1Entities db = new SV_db1Entities();

            // 01/17/2018 var spec1 = db.ref_specialty1.Where(b => b.level2_classification_code == code || b.level3_specialization_code == code);
            var spec1 = from a in db.ref_specialty1
                        select new {
                            a.id,
                            specialty_provider = a.ref_specialty_provider.name,
                            a.level2_classification,
                            a.level2_classification_code,
                            a.level3_specialization,
                            a.level3_specialization_code,
                            condition = from b in a.ref_condition
                                        select b.dname,
                           
                        };
            List < doc_specialty_01112018 > spec = new List<doc_specialty_01112018>();

            foreach (var n in spec1) {

                //string[] con = n.conditions.Split('|');
                List<string> condition = new List<string>();
                foreach (var c in n.condition)
                { condition.Add(c); }

                spec.Add(new doc_specialty_01112018
                {
                    // id = n.ref_specialty.id,
                    // description = n.ref_specialty.description,
                    // name = n.ref_specialty.name,
                    // actor = n.ref_specialty.actor == null ? "" : n.ref_specialty.actor

                    id = n.id,
                    provider_type = n.specialty_provider,//  s.provider_type,

                    classification_code = n.level2_classification_code,
                    classification = n.level2_classification,

                    specialization_code = n.level3_specialization_code == null ? "" : n.level3_specialization_code,
                    specialization = n.level3_specialization == null ? "" : n.level3_specialization, // s.specialization,
                    conditions = condition
                });
            }
           

            return spec;
        }

        // created: 01/11/2018
        public List<doc_specialty_01112018> getSpecialty(long spec_id)
        {
            SV_db1Entities db = new SV_db1Entities();

            var n = db.ref_specialty1.Find(spec_id);
            List<doc_specialty_01112018> spec = new List<doc_specialty_01112018>();

            if (n != null) {
                //string[] con = n.conditions.Split('|');
                List<string> condition = new List<string>();
                foreach (var c in n.ref_condition)
                { condition.Add(c.dname); }

                spec.Add(new doc_specialty_01112018
                {
                    id = n.id,
                    provider_type = n.ref_specialty_provider.name,//  s.provider_type,

                    classification_code = n.level2_classification_code,
                    classification = n.level2_classification,

                    specialization_code = n.level2_classification_code == null ? "" : n.level2_classification_code,
                    specialization = n.level3_specialization == null ? "" : n.level3_specialization, // s.specialization,
                    conditions = condition
                });
            }    
      
            return spec;
        }

        public List<doc_language> _getLanguage_doc1(dynamic con_lang)
        {
            // temporary for table DOCTOR1

            try
            {
                SV_db1Entities dbEntity = new SV_db1Entities();
                List<doc_language> doc_lang = new List<doc_language>();
                //var con_lang = dbEntity.con_DOCTOR1_ref_language.Where(a => a.rel_DOCTOR_id == doctor_id);

                //string[] split_val = value.Split(',');
                foreach (var a in con_lang)
                {

                    // var ref_lang = dbEntity.ref_languages.Find(a.rel_ref_language_id);
                    //doc_lang.Add(new doc_language
                    //{
                    //    id = a.ref_languages.id,
                    //    name = a.ref_languages.name
                    //});

                    doc_lang.Add(new doc_language
                    {
                        id = a.lang_id,
                        name = a.lang_name
                    });

                }


                return doc_lang;

            }
            catch (Exception ex) { return null; }

        }

        public List<insurance> _getInsurance_doc1(dynamic con_ins)
        { // temporary for table DOCTOR1

            try
            {
                SV_db1Entities dbEntity = new SV_db1Entities();
                List<insurance> ins = new List<insurance>();
                // var con_ins = dbEntity.con_DOCTOR1_ref_insurance.Where(a => a.rel_DOCTOR_id == doctor_id);

                foreach (var n in con_ins)
                {
                    //ins.Add(new insurance
                    //{
                    //    id = n.rel_ref_insurance_provider_id.Value,
                    //    provider = n.ref_insurance_provider.PayerName.Split('|')[0]
                    //});
                    ins.Add(new insurance
                    {
                        id = n.ins_id,
                        provider = n.ins_provider.Split('|')[0]
                    });
                }

                return ins;
            }
            catch (Exception ex)
            {
                return null;
            }


        }

        public getDoctor_rating _get_averagerating_doc1(long doctor_id)
        {

            try
            {
                //balkon rating = dr.average_rating,
                //balkon favorite = dr.fave,
                //balkon patient_review = dr.review,

                SV_db1Entities dbEntity = new SV_db1Entities();

                // var appt = dbEntity.APPOINTMENTs.Where(a => a.doctor_id == doctor_id);
                var appt = from ap in dbEntity.APPOINTMENTs
                           where ap.doctor_id == doctor_id
                           select ap;
                // having Where clause slows down the response.
                // we shud consider the status of the Appointment, only those that were Closed not canceled

                getDoctor_rating ave = new getDoctor_rating();
                List<p_ratings_review> rating_review = new List<p_ratings_review>();
                foreach (var a in appt)
                {
                    // long id = a.doctor_id.Value;
                    // ave.average_rating= a.doctor_rating.Value;
                    // review = a.doctor_review;
                    double rating = a.doctor_rating == null ? 0 : a.doctor_rating.Value;
                    string review = a.doctor_review;

                    rating_review.Add(new p_ratings_review { rating = rating, remark = review });

                    // ave.review.Add{ rating_review};

                }

                ave.review = rating_review;
                return ave;
            }
            catch (Exception ex)
            {

                return null;
            }

        }
    }




}