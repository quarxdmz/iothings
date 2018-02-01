using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using api.Models;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Data.Entity.SqlServer;
using System.Text;
using System.Threading;
using api.Classes;
//using System.Device.Location;

namespace api.Controllers
{
    public class SearchDoctorController : Base.doctor
    {
        DateTime dt = DateTime.UtcNow;
        SV_db1Entities dbEntity = new SV_db1Entities();

        [HttpGet]
        [Route("doctors/search")]
        public IHttpActionResult getDoctor_search([FromUri]doc_search_query _params)
        {
            try {



           NameValueCollection mapQUery = UriExtensions.ParseQueryString(HttpContext.Current.Request.Url);
           foreach (var k in mapQUery.Keys)
           {
              var res = typeof(doc_search_query).GetProperty(k.ToString());
                    if(res == null)
                         return Json(new { data = new string[] { }, message = "Object reference not set to an instance of an object. Invalid parameter name: " + k.ToString(), success = false });
           }

           // xxxxxxxs

            //IsRequired("lat", _params.lat.ToString(), 2);
            if (_params.lat > 0)
            {
            //    IsRequired("longi", _params.longi.ToString(), 2);
            }
            if (_params.longi > 0)
            {
            //    IsRequired("lat", _params.lat.ToString(), 2);
            }

        

            long user_id = _params.user_id;

            // parsing the SQL string
            string sql = parsingSQL(_params);
                // return getDirectory(sql);

                var items = dbEntity.hs_DOCTOR.SqlQuery(sql).ToList();
                // what happen if no .ToList()

               // var items = dbEntity.hs_DOCTOR.SqlQuery<hs_DOCTOR>(sql);

                //items = items.Skip(0).Take(25);
            

            if (items.Count() == 0)
            {
                return Json(new { data = new string[] { }, message ="No record found.", success= false });
            }

            List<doc_search_profile2> dc = new List<doc_search_profile2>();

            foreach (var doc in items)
            {
              
                double doc_id = doc.id;

                // get doctor is in con_USER_favorite_DOCTOR
                bool faveList = false; 
                if (user_id > 0)
                {
                    var fave_doctor = dbEntity.con_USER_favorite_DOCTOR.Where(a => a.rel_doctor_id == doc_id && a.create_by__USER_id == user_id);
                    if (fave_doctor.Count() > 0)
                    {
                        faveList = true;
                    }
                }


                // start: get average doctor rating
                double ave_rating = 0;
                var appt_docid = dbEntity.APPOINTMENT_ext.Where(a => a.attr_name == "doctor_id" && a.value == doc_id.ToString());

                // get doctor is in con_USER_favorite_DOCTOR
                if (user_id > 0)
                {
                    appt_docid = appt_docid.Where(a => a.create_by__USER_id == user_id);
                }

                if (appt_docid.Count() > 0)
                {
                   
                    int cnt = 0; double doctor_rate=0;
                    foreach (var n in appt_docid)
                    {
                     
                        var appt_docrate = dbEntity.APPOINTMENT_ext.Where(a => a.attr_name == "doctor_rating" && a.rel_APPOINTMENT_id == n.rel_APPOINTMENT_id);
                        // get doctor is in con_USER_favorite_DOCTOR
                        if (faveList)
                        {
                            appt_docrate = appt_docrate.Where(a => a.create_by__USER_id == user_id);
                        }

                        if (appt_docrate.Count() > 0) {
                            foreach (var d in appt_docrate)
                            {
                                cnt++;
                                double dRate = 0;
                                bool temp = double.TryParse(d.value, out dRate);
                                if(temp) doctor_rate += dRate;
                            }

                          
                        }
                    }
                    if (cnt > 0)
                    {
                        ave_rating = doctor_rate / cnt;
                    }

                    // asdfasfsdfsd
                    
                    // asdfasfsdfsd

                }
                // end: get average doctor rating

                // get SPECIALTY
                
                //var sp = (from a in dbEntity.con_DOCTOR_ref_specialty
                //          join b in dbEntity.ref_specialty on a.rel_ref_specialty_id equals b.id
                //          where a.rel_DOCTOR_id == doc_id
                //          select b).Distinct().ToList();
               

                // get: doctor_ext
                    var_getDoctor_ext d_ext = _getDoctor_ext(doc);

                    #region
                    //List<doc_specialty2> spec = new List<doc_specialty2>();
                    //var doc_ext = doc.hs_DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doc_id);
                    //List<appt_type> appt = new List<appt_type>();
                    //zip_search_address address = new zip_search_address()
                    //{
                    //    street = "", zip = "", city = "", state = "",
                    //    lat = 0, lng = 0, county = ""
                    //};

                    //double doc_fee = 0;
                    //string vtime_slot = "";

                    //foreach (var n in doc_ext)
                    //{
                    //    switch (n.attr_name)
                    //    {
                    //        case "fee":
                    //            bool bTemp = double.TryParse(n.value, out doc_fee);
                    //            break;
                    //        case "specialty_id":
                    //            //var sp_ex = dbEntity.DOCTOR_ext.Where(a => a.attr_name == "specialty_id" && a.rel_DOCTOR_id == doc_id).FirstOrDefault();
                    //            //string[] sp = sp_ex.value.Split(',');

                    //            string[] sp = n.value.Split(',');
                    //            foreach (var i in sp)
                    //            {

                    //                long spec_id = 0;
                    //                bool s = long.TryParse(i, out spec_id);
                    //                if (s)
                    //                {
                    //                    var get_spec = dbEntity.ref_specialty_def.Find(spec_id);
                    //                    //if (get_spec != null) {

                    //                    spec.Add(new doc_specialty2
                    //                    {
                    //                        id = spec_id,
                    //                        name = get_spec == null ? "" : get_spec.name,
                    //                        code = get_spec == null ? "" : get_spec.code,
                    //                        provider_type = get_spec == null ? "" : get_spec.provider_type,
                    //                        specialization = get_spec == null ? "" : get_spec.specialization
                    //                    });
                    //                    //}

                    //                }

                    //            }
                    //            break;
                    //        case "home_zip":
                    //            //var doc_ext = dbEntity.hs_DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doc.id && a.attr_name == "home_zip");
                    //            //foreach (var i in doc_ext)
                    //            //{
                    //            string zipcode = n.value.Substring(0, 5).ToString();
                    //            var addr = dbEntity.ref_zip.Where(b => b.zip == zipcode);

                    //            foreach (var i in addr)
                    //            {
                    //                string addr2 = doc.addr_address2 == null ? "" : doc.addr_address2;
                    //                //address = new zip_search_address
                    //                //{
                    //                //id = addr.id,
                    //                address.street = doc.addr_address1 == null ? "" : doc.addr_address1 + " " + addr2;
                    //                //ref_zip_id = System.Convert.ToInt32(doc.addr_rel_ref_zip_id),
                    //                address.zip = i.zip == null ? "" : i.zip;
                    //                address.city = i.city_name == null ? "" : i.city_name;
                    //                address.state = i.city_state == null ? "" : i.city_state;
                    //                address.lat = i.city_lat;
                    //                address.lng = i.city_lon;
                    //                address.county = i.city_county == null ? "" : i.city_county;
                    //                //};
                    //            }
                    //            //}
                    //            break;
                    //        case "drappttype":
                    //            string vappt_name = "";
                    //            long vappt_id = 0;
                    //            bool isAppt = long.TryParse(n.value, out vappt_id);

                    //            if (vappt_id > 0)
                    //            {
                    //                var n1 = dbEntity.ref_APPOINTMENT_type.Find(vappt_id);
                    //                vappt_name = n1.dname;

                    //                appt.Add(new appt_type
                    //                {
                    //                    id = vappt_id,
                    //                    type = vappt_name
                    //                });
                    //            }
                    //            break;

                    //        case "time_slot":
                    //            vtime_slot = n.value == null ? "" : n.value;
                    //            break;
                    //    }
                    //}
                    // end: doctor_ext
                    #endregion


                int fave_doc = 0;
                if (user_id > 0 && faveList)
                {
                    var con_fav = dbEntity.con_USER_favorite_DOCTOR.Where(a => a.rel_doctor_id == doc.id);
                    if (con_fav.Count() > 0)
                    {
                        if (con_fav.FirstOrDefault().favor == true)
                            fave_doc = 1;
                        else
                            fave_doc = 0;
                    }

                }

                    List<zip_search_address> home_addr = custom._getDoctor_homeaddress(doc);
                    List<zip_search_address> pract_addr = custom._getDoctor_practiceaddress(doc);

                    var prof = new Models.doc_search_profile2
                {
                    id = doc.id,
                    first_name = doc.name_first,
                    last_name = doc.name_last,
                    middle_name = doc.name_middle == null ? "" : doc.name_middle,
                    email = doc.email == null ? "" : doc.email,
                    gender = doc.gender == null ? "" : doc.gender.Trim().ToUpper(),
                    title = doc.title == null ? "" : doc.title,
                    phone = doc.phone == null ? "" : doc.phone,
                    license = doc.license_no == null ? "" : doc.license_no,
                    npi = doc.NPI == null ? "" : doc.NPI,
                    organization_name = doc.organization_name == null ? "" : doc.organization_name,
                    image_url = doc.image_url == null ? "" : doc.image_url,
                    // balkon rating = ave_rating,
                    doctor_fee = d_ext.doc_fee,
                    favorite = fave_doc,
                    time_slot = d_ext.time_slot,
                    bio = doc.bio == null ? "" : doc.bio,
                    specialties = d_ext.spec == null ? new List<doc_specialty_01112018>() { } : d_ext.spec,
                    appointment_type = d_ext.appt_type == null ? new List<appt_type>() { } : d_ext.appt_type,
                    
                    home_address = home_addr == null ? new List<zip_search_address>() { } : home_addr,
                    practice_address = pract_addr == null ? new List<zip_search_address>() { } : pract_addr
                    };

                dc.Add(prof);
            }
            
            //var Query = dbEntity.ref_zip.SqlQuery(sql);

            //var doctors = dbEntity.ref_zip.SqlQuery(sql);
            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(_params);
            if (dc.Count() > 0)
            {
                var res = Newtonsoft.Json.JsonConvert.SerializeObject(dc);
                var json = Newtonsoft.Json.Linq.JArray.Parse(res);

                return Json(new { data = json, message = dc.Count() + " Record(s) found.", success = true });
            }
            else
            {
                return Json(new { data = new string[] { }, message = "No record found.", success = false });
            }

            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = "No record found.", success = false });
            }
        }

        private string mobile_parsingSQL(doc_search_mobile _params, bool bCountAll)
        {

            string lat = _params.lat.ToString();
            string lon = _params.longi.ToString();

             //var doc_name = from d in dbEntity.DOCTOR1
             //              join zip1 in dbEntity.ref_zip on d.home_addr_zip_id equals zip1.id
             //              //join zip2 in dbEntity.ref_zip on d.home_addr_zip_id equals zip1.id
             //              select new
             //              {
             //                  d.id,
             //                  d.NPI,
             //                  name_first = d.name_first == null ? "" : d.name_first,
             //                  name_last = d.name_last == null ? "" : d.name_last,
             //                  name_middle = d.name_middle == null ? "" : d.name_middle,
             //                  organization_name = d.organization_name == null ? "" : d.organization_name,
             //                  email = d.email == null ? "" : d.email,
             //                  gender = d.gender == null ? "" : d.gender,
             //                  title = d.title == null ? "" : d.title,
             //                  phone = d.phone == null ? "" : d.phone,
             //                  license_no = d.license_no == null ? "" : d.license_no,

             //                  d.home_addr_1,
             //                  d.home_addr_2,
             //                  home_zip = zip1.zip,
             //                  home_state = zip1.city_state,
             //                  home_state_long = zip1.city_state_long,
             //                  home_lat = zip1.city_lat,
             //                  home_long = zip1.city_lon,
             //                  home_city = zip1.city_name,
             //                  home_county = zip1.city_county,

             //                  pract_addr_1 = d.practice_addr_1,
             //                  pract_addr_2 = d.practice_addr_2,
             //                  pract_zip = d.ref_zip.zip,
             //                  pract_state = d.ref_zip.city_state,
             //                  pract_state_long = d.ref_zip.city_state_long,
             //                  pract_lat = d.ref_zip.city_lat,
             //                  pract_long = d.ref_zip.city_lon,
             //                  pract_city = d.ref_zip.city_name,
             //                  pract_county = d.ref_zip.city_county,

             //                  con_spec = from spec in d.con_DOCTOR1_ref_specialty
             //                             select new
             //                             {
             //                                 spec_id = spec.ref_specialty.id,
             //                                 spec_name = spec.ref_specialty.name,
             //                                 spec_code = spec.ref_specialty.code,
             //                                 spec_provider_type = spec.ref_specialty.provider_type,
             //                                 spec_specialization = spec.ref_specialty.specialization
             //                             },

             //                  con_lang = from lang in d.con_DOCTOR1_ref_language
             //                             select new { lang_id = lang.ref_languages.id, lang_name = lang.ref_languages.name },

             //                  con_ins = from ins in d.con_DOCTOR1_ref_insurance
             //                            select new { ins_id = ins.ref_insurance_provider.id, ins_provider = ins.ref_insurance_provider.PayerName }


             //              };



            // =====================
            string sql = @"SELECT DISTINCT(docs.id), docs.* FROM 
                        (
	                        SELECT 
	                        dc.*,
                            zp.zip, zp.city_name, zp.city_state, zp.city_lat, zp.city_lon, zp.city_county,
                            ROW_NUMBER() over(Order by dc.name_first, dc.name_last) as 'rownumber'";
            // --sp.rel_ref_specialty_id as spid,
            // --appt.value as appt_id,

            if (_params.lat != 0 && _params.longi != 0)
            {
                sql += @"  ,
	                        src.*,(DEGREES(acos(
                                    cos(RADIANS(src.latpoint))
                                    * cos(RADIANS(zp.city_lat))
                                    * cos(RADIANS(src.longpoint - zp.city_lon))
                                    + sin(RADIANS(src.latpoint))
                                    * sin(RADIANS(zp.city_lat))
                                )) * src.unit_distance) AS distance";
            }

            sql += @"      
	                        FROM DOCTOR1 AS dc 
	                        JOIN ref_zip AS zp ON dc.practice_addr_zip_id = zp.id ";
            //--JOIN con_DOCTOR_ref_specialty sp ON sp.rel_DOCTOR_id = dc.id
            //--JOIN hs_DOCTOR_ext appt ON appt.rel_DOCTOR_id = dc.id AND appt.attr_name = 'drappttype' AND appt.value = " + _params.appt_type + "";


            //// if CITY param is present
            //if (!string.IsNullOrEmpty(_params.city) || !string.IsNullOrEmpty(_params.state) || !string.IsNullOrEmpty(_params.zipcode))
            //{
            //    //if (lokup > 0) sql += "@ AND ";
            //    //else lokup = 1;

            //    sql += @" 
            //          JOIN DOCTOR_ext1 prac ON prac.rel_DOCTOR_id = dc.id AND prac.attr_name = 'practice_zip'";
            //}

            //if (_params.specialty > 0)
            //{
            //    string sp_id = _params.specialty.ToString();
            //    sql += @"
            //             JOIN DOCTOR_ext1 spec ON spec.rel_DOCTOR_id = dc.id AND spec.attr_name='specialty_id' AND (spec.[value] like '%," + sp_id + ",%' or spec.[value] like '" + sp_id + ",%'  or spec.[value] like '%," + sp_id + "' or spec.[value] = '" + sp_id + "')";
            //}

            //if (_params.appt_type > 0)
            //{
            //    string ap_type = _params.appt_type.ToString();
            //    sql += @"
            //            JOIN DOCTOR_ext1 appt ON appt.rel_DOCTOR_id = dc.id AND appt.attr_name='drappttype' AND (appt.[value] like '%," + ap_type + ",%' or appt.[value] like '" + ap_type + ",%'  or appt.[value] like '%," + ap_type + "' or appt.[value] = '" + ap_type + "')";
            //}

            int lokup = 0;
            if (_params.lat != 0 && _params.longi != 0)
            {
                sql += @"
                     JOIN (
		                        SELECT " + lat + @" AS latpoint,
				                       " + lon + @" AS longpoint,
				                        1000.0 AS radius,
				                        69.0 AS unit_distance,
				                        'miles' AS unit
	                        ) AS src ON 1=1";
            }

            sql += @"
               
                        ) AS docs";

            //&& !string.IsNullOrEmpty(_params.state)
            if ((_params.lat != 0 && _params.longi != 0)
                      //|| !string.IsNullOrEmpty(_params.state)
                      // || !string.IsNullOrEmpty(_params.city)
                      // || !string.IsNullOrEmpty(_params.zipcode)
                      )
            {
                sql += @" WHERE ";
            }


            if (_params.lat != 0 && _params.longi != 0)
            {
                lokup = 1;
                sql += @" docs.distance <= docs.radius ";
            }

            //// if STATE param is present
            //if (!string.IsNullOrEmpty(_params.state))
            //{
            //    if (lokup > 0) sql += " AND ";
            //    else lokup = 1;

            //    sql += @"  docs.city_state='" + _params.state + "'";
            //}

            //// if CITY param is present
            //if (!string.IsNullOrEmpty(_params.city))
            //{
            //    if (lokup > 0) sql += " AND ";
            //    else lokup = 1;

            //    sql += @" docs.city_name  like '%" + _params.city + "%'";


            //// if ZIP param is present
            //if (!string.IsNullOrEmpty(_params.zipcode))
            //{
            //    if (lokup > 0) sql +=  AND ";
            //    else lokup = 1;

            //    sql += @" docs.zip='" + _params.zipcode + "'";
            //}

            // exclude this, to GET total_count
            if (!bCountAll)
            {
                sql += @" 
                        AND rownumber BETWEEN " + _params.skip + " AND " + _params.take + @"";
                //-- AND docs.appt_id = " + _params.appt_type + @" 
                // -- AND docs.spid = " + _params.specialty + "";
            }

            return sql;
        }

        private string mobile_parsingSQL1(doc_search_mobile _params, bool bCountAll)
        {

            string lat = _params.lat.ToString();
            string lon = _params.longi.ToString();

            string sql = @"SELECT DISTINCT(docs.id), docs.* FROM 
                        (
	                        SELECT 
	                        dc.*,
                            zp.zip, zp.city_name, zp.city_state, zp.city_lat, zp.city_lon, zp.city_county,
                            ROW_NUMBER() over(Order by dc.name_first, dc.name_last) as 'rownumber'";
                            // --sp.rel_ref_specialty_id as spid,
                            // --appt.value as appt_id,

            if (_params.lat != 0 && _params.longi != 0)
            {
                sql += @"  ,
	                        src.*,(DEGREES(acos(
                                    cos(RADIANS(src.latpoint))
                                    * cos(RADIANS(zp.city_lat))
                                    * cos(RADIANS(src.longpoint - zp.city_lon))
                                    + sin(RADIANS(src.latpoint))
                                    * sin(RADIANS(zp.city_lat))
                                )) * src.unit_distance) AS distance";
            }

            sql += @"      
	                        FROM DOCTOR1 AS dc 
	                        JOIN ref_zip AS zp ON dc.practice_addr_zip_id = zp.id ";
                            //--JOIN con_DOCTOR_ref_specialty sp ON sp.rel_DOCTOR_id = dc.id
                            //--JOIN hs_DOCTOR_ext appt ON appt.rel_DOCTOR_id = dc.id AND appt.attr_name = 'drappttype' AND appt.value = " + _params.appt_type + "";


            //// if CITY param is present
            //if (!string.IsNullOrEmpty(_params.city) || !string.IsNullOrEmpty(_params.state) || !string.IsNullOrEmpty(_params.zipcode))
            //{
            //    //if (lokup > 0) sql += "@ AND ";
            //    //else lokup = 1;

            //    sql += @" 
            //          JOIN DOCTOR_ext1 prac ON prac.rel_DOCTOR_id = dc.id AND prac.attr_name = 'practice_zip'";
            //}

            //if (_params.specialty > 0)
            //{
            //    string sp_id = _params.specialty.ToString();
            //    sql += @"
            //             JOIN DOCTOR_ext1 spec ON spec.rel_DOCTOR_id = dc.id AND spec.attr_name='specialty_id' AND (spec.[value] like '%," + sp_id + ",%' or spec.[value] like '" + sp_id + ",%'  or spec.[value] like '%," + sp_id + "' or spec.[value] = '" + sp_id + "')";
            //}

            //if (_params.appt_type > 0)
            //{
            //    string ap_type = _params.appt_type.ToString();
            //    sql += @"
            //            JOIN DOCTOR_ext1 appt ON appt.rel_DOCTOR_id = dc.id AND appt.attr_name='drappttype' AND (appt.[value] like '%," + ap_type + ",%' or appt.[value] like '" + ap_type + ",%'  or appt.[value] like '%," + ap_type + "' or appt.[value] = '" + ap_type + "')";
            //}

            int lokup = 0;
            if (_params.lat != 0 && _params.longi != 0)
            {
                sql += @"
                     JOIN (
		                        SELECT " + lat + @" AS latpoint,
				                       " + lon + @" AS longpoint,
				                        1000.0 AS radius,
				                        69.0 AS unit_distance,
				                        'miles' AS unit
	                        ) AS src ON 1=1";
            }

            sql += @"
               
                        ) AS docs";

            //&& !string.IsNullOrEmpty(_params.state)
            if ((_params.lat != 0 && _params.longi != 0)
                    //|| !string.IsNullOrEmpty(_params.state)
                    // || !string.IsNullOrEmpty(_params.city)
                     // || !string.IsNullOrEmpty(_params.zipcode)
                      )
            {
                sql += @" WHERE ";
            }


            if (_params.lat != 0 && _params.longi != 0)
            {
                lokup = 1;
                sql += @" docs.distance <= docs.radius ";
            }

            //// if STATE param is present
            //if (!string.IsNullOrEmpty(_params.state))
            //{
            //    if (lokup > 0) sql += " AND ";
            //    else lokup = 1;

            //    sql += @"  docs.city_state='" + _params.state + "'";
            //}

            //// if CITY param is present
            //if (!string.IsNullOrEmpty(_params.city))
            //{
            //    if (lokup > 0) sql += " AND ";
            //    else lokup = 1;

            //    sql += @" docs.city_name  like '%" + _params.city + "%'";
            

            //// if ZIP param is present
            //if (!string.IsNullOrEmpty(_params.zipcode))
            //{
            //    if (lokup > 0) sql +=  AND ";
            //    else lokup = 1;

            //    sql += @" docs.zip='" + _params.zipcode + "'";
            //}

            // exclude this, to GET total_count
            if (!bCountAll)
            {
                sql += @" 
                        AND rownumber BETWEEN " + _params.skip + " AND " + _params.take + @"";
                //-- AND docs.appt_id = " + _params.appt_type + @" 
                // -- AND docs.spid = " + _params.specialty + "";
            }
            
            return sql;
        }

        private string mobile_parsingSQL_loguser(doc_search_mobile_loguser _params, bool bCountAll = false)
        {

            string lat = _params.lat.ToString();
            string lon = _params.longi.ToString();

            // accepts Condition_ string separated by comma
            // output: specialty_id
            List<long> spec_id = new List<long>();
            List<long> doc_id = new List<long>();
            string[] str_con_id = _params.condition.Split(',');
            foreach (var s in str_con_id)
            {
                string sTrim = s.Trim().ToLower();
                //select* from con_doctor1_ref_specialty spec
                //left join con_doctor1_ref_insurance ins ON ins.rel_doctor_id = spec.rel_doctor_id
                //left join con_ref_specialty_ref_condition sc ON spec.rel_ref_specialty_id = sc.rel_ref_specialty_id
                //left join ref_condition c ON c.id = sc.rel_ref_condition_id
                //where c.name = 'acne'
                var get_condition = from d in dbEntity.DOCTOR1
                                    join spec in dbEntity.con_DOCTOR1_ref_specialty on d.id equals spec.rel_DOCTOR_id
                                    // 1/15/2018 join sc in dbEntity.con_ref_specialty_ref_condition on spec.rel_ref_specialty_id equals sc.rel_ref_specialty_id
                                    join ins in dbEntity.con_DOCTOR1_ref_insurance on spec.rel_DOCTOR_id equals ins.rel_DOCTOR_id

                                    // 1/15/2018 join con1 in dbEntity.ref_condition on sc.rel_ref_condition_id equals con1.id
                                    // 1/15/2018  con1.name.Contains(sTrim) ||
                                    where ( d.name_first.Contains(sTrim) || d.name_last.Contains(sTrim) || d.name_middle.Contains(sTrim))
                                        && ins.rel_ref_insurance_provider_id == _params.insurance_id
                                    select spec;

                foreach (var d in get_condition)
                {
                    if (!doc_id.Contains(d.rel_DOCTOR_id.Value)) { doc_id.Add(d.rel_DOCTOR_id.Value); }
                }
            }
                //foreach (var s in str_con_id)
                //{
             
                //    var con_str = dbEntity.con_ref_spehttps://www.youtube.com/watch?v=4L7gsJfgJawcialty_ref_condition.Where(a => a.ref_condition.description.ToLower().Contains(s));

                //    //var con_str = from a in dbEntity.con_ref_specialty_ref_condition
                //    //              join b in dbEntity.con_DOCTOR1_ref_specialty on a.rel_ref_specialty_id equals b.rel_ref_specialty_id
                //    //              where a.ref_condition.description.Contains(sTrim)
                //                  //select b;
                //    //========
                //    var ins_str = dbEntity.con_DOCTOR1_ref_insurance.Where(a => a.rel_ref_insurance_provider_id == _params.insurance_id);

                //    foreach (var c in con_str)
                //    {
                //        //spec_id.Add(c.rel_ref_specialty_id.Value);
                //        var doc_str = dbEntity.con_DOCTOR1_ref_specialty.Where(a => a.rel_ref_specialty_id == c.rel_ref_specialty_id);

                //        foreach (var d in doc_str)
                //        {
                //            if (!doc_id.Contains(d.rel_DOCTOR_id.Value)) { doc_id.Add(d.rel_DOCTOR_id.Value); }

                //        }
                //    }

                //}


                string sql = @"SELECT DISTINCT(docs.id), docs.* FROM 
            
                        (
	                        SELECT 
	                        dc.*,
                            --zp.zip, zp.city_name, zp.city_state, zp.city_lat, zp.city_lon, zp.city_county,
                            --ins.rel_DOCTOR_id, ins.rel_ref_insurance_provider_id,                      
                               
                            ROW_NUMBER() over(Order by dc.name_first, dc.name_last) as 'rownumber'";
            // --sp.rel_ref_specialty_id as spid,
            // --appt.value as appt_id,

            if (spec_id.Count() > 0)
            {
                sql += @",
                       sp.rel_ref_specialty_id as 'specialty_id' ";
            }

            if (_params.lat != 0 && _params.longi != 0)
            {
                sql += @"  ,
	                        src.*,(DEGREES(acos(
                                    cos(RADIANS(src.latpoint))
                                    * cos(RADIANS(zp.city_lat))
                                    * cos(RADIANS(src.longpoint - zp.city_lon))
                                    + sin(RADIANS(src.latpoint))
                                    * sin(RADIANS(zp.city_lat))
                                )) * src.unit_distance) AS distance";
            }

            sql += @"      
	                        FROM DOCTOR1 AS dc 
	                        JOIN ref_zip AS zp ON dc.practice_addr_zip_id = zp.id  ";
            // 12/14/2017 JOIN con_DOCTOR1_ref_insurance AS ins ON ins.rel_DOCTOR_id = dc.id ";

            //--JOIN con_DOCTOR_ref_specialty sp ON sp.rel_DOCTOR_id = dc.id
            //--JOIN hs_DOCTOR_ext appt ON appt.rel_DOCTOR_id = dc.id AND appt.attr_name = 'drappttype' AND appt.value = " + _params.appt_type + "";

            //if (str_con_id.Count() > 0)
            // {
            //     //string sp_id = _params.specialty.ToString();
            //     sql += @"
            //               JOIN con_DOCTOR1_ref_specialty AS sp ON sp.rel_DOCTOR_id = dc.id";
            //     //JOIN DOCTOR_ext1 spec ON spec.rel_DOCTOR_id = dc.id AND spec.attr_name='specialty_id' AND (spec.[value] like '%," + sp_id + ",%' or spec.[value] like '" + sp_id + ",%'  or spec.[value] like '%," + sp_id + "' or spec.[value] = '" + sp_id + "')";

            // }


            int lokup = 0;
            if (_params.lat != 0 && _params.longi != 0)
            {
                sql += @"
                     JOIN (
		                        SELECT " + lat + @" AS latpoint,
				                       " + lon + @" AS longpoint,
				                        1000.0 AS radius,
				                        69.0 AS unit_distance,
				                        'miles' AS unit
	                        ) AS src ON 1=1";
            }

            sql += @"
               
                        ) AS docs";

            //&& !string.IsNullOrEmpty(_params.state)
            if ((_params.lat != 0 && _params.longi != 0)
                    //|| !string.IsNullOrEmpty(_params.state)
                    // || !string.IsNullOrEmpty(_params.city)
                     // || !string.IsNullOrEmpty(_params.zipcode)
                      )
            {
                sql += @" WHERE ";
            }


            if (_params.lat != 0 && _params.longi != 0)
            {
                lokup = 1;
                sql += @" docs.distance <= docs.radius ";
            }

            if (doc_id.Count() > 0)
            {
                sql += @"
                         AND docs.id in  (" + string.Join(",", doc_id) + @")";
                // AND docs.specialty_id IN (" +  string.Join(",", spec_id) +  @")" ;
            }
            else
            { sql += @"
                         AND docs.id in  (0)"; }

            //sql += @" 
            //           AND docs.rel_ref_insurance_provider_id = " + _params.insurance_id + @"";
            
     

                // exclude this, to GET total_count
                if (!bCountAll)
            {
                sql += @" 
                        AND rownumber BETWEEN " + _params.skip + " AND " + _params.take + @"";
                //-- AND docs.appt_id = " + _params.appt_type + @" 
                // -- AND docs.spid = " + _params.specialty + "";
            }

            // -- AND docs.appt_id = " + _params.appt_type + @" 
            // -- AND docs.spid = " + _params.specialty + "";




            return sql;
        }

        private string parsingSQL(doc_search_query _params) {

            string lat = _params.lat.ToString();
            string lon = _params.longi.ToString();

            string  sql = @"SELECT DISTINCT(docs.id), docs.* FROM 
                        (
	                        SELECT 
	                        dc.*,
                            zp.zip, zp.city_name, zp.city_state, zp.city_lat, zp.city_lon, zp.city_county";
            // --sp.rel_ref_specialty_id as spid,
            // --appt.value as appt_id,

            if (_params.lat != 0 && _params.longi != 0)
            {
                sql += @"  ,
	                        src.*,(DEGREES(acos(
                                    cos(RADIANS(src.latpoint))
                                    * cos(RADIANS(zp.city_lat))
                                    * cos(RADIANS(src.longpoint - zp.city_lon))
                                    + sin(RADIANS(src.latpoint))
                                    * sin(RADIANS(zp.city_lat))
                                )) * src.unit_distance) AS distance";
            }

            sql += @"      
	                        FROM hs_DOCTOR AS dc 
	                        JOIN ref_zip AS zp ON dc.addr_rel_ref_zip_id = zp.id
                            --JOIN con_DOCTOR_ref_specialty sp ON sp.rel_DOCTOR_id = dc.id
                            --JOIN hs_DOCTOR_ext appt ON appt.rel_DOCTOR_id = dc.id AND appt.attr_name = 'drappttype' AND appt.value = " + _params.appt_type + "";


            // if CITY param is present
            if (!string.IsNullOrEmpty(_params.city) || !string.IsNullOrEmpty(_params.state) || !string.IsNullOrEmpty(_params.zipcode))
            {
                //if (lokup > 0) sql += "@ AND ";
                //else lokup = 1;

                sql += @" 
                      JOIN hs_DOCTOR_ext prac ON prac.rel_DOCTOR_id = dc.id AND prac.attr_name = 'practice_zip'";
            }

            if (_params.specialty > 0)
            {
                string sp_id = _params.specialty.ToString();
                sql += @"
                         JOIN hs_DOCTOR_ext spec ON spec.rel_DOCTOR_id = dc.id AND spec.attr_name='specialty_id' AND (spec.[value] like '%," + sp_id + ",%' or spec.[value] like '" + sp_id + ",%'  or spec.[value] like '%," + sp_id + "' or spec.[value] = '" + sp_id + "')";

            }

            if (_params.appt_type > 0)
            {
                string ap_type = _params.appt_type.ToString();
                sql += @"
                        JOIN hs_DOCTOR_ext appt ON appt.rel_DOCTOR_id = dc.id AND appt.attr_name='drappttype' AND (appt.[value] like '%," + ap_type + ",%' or appt.[value] like '" + ap_type + ",%'  or appt.[value] like '%," + ap_type + "' or appt.[value] = '" + ap_type + "')";
            }

            int lokup = 0;
            if (_params.lat != 0 && _params.longi != 0)
            {
                sql += @"
                     JOIN (
		                        SELECT " + lat + @" AS latpoint,
				                       " + lon + @" AS longpoint,
				                        1000.0 AS radius,
				                        69.0 AS unit_distance,
				                        'miles' AS unit
	                        ) AS src ON 1=1";
            }

            sql += @"
               
                        ) AS docs";

            //&& !string.IsNullOrEmpty(_params.state)
            if ((_params.lat != 0 && _params.longi != 0)
                    || !string.IsNullOrEmpty(_params.state)
                     || !string.IsNullOrEmpty(_params.city)
                      || !string.IsNullOrEmpty(_params.zipcode))
            {
                sql += @" WHERE ";
            }


            if (_params.lat != 0 && _params.longi != 0)
            {
                lokup = 1;
                sql += @" docs.distance <= docs.radius ";
            }

            // if STATE param is present
            if (!string.IsNullOrEmpty(_params.state))
            {
                if (lokup > 0) sql += " AND ";
                else lokup = 1;

                sql += @"  docs.city_state='" + _params.state + "'";
            }

            // if CITY param is present
            if (!string.IsNullOrEmpty(_params.city))
            {
                if (lokup > 0) sql += " AND ";
                else lokup = 1;

                sql += @" docs.city_name  like '%" + _params.city + "%'";
            }

            // if ZIP param is present
            if (!string.IsNullOrEmpty(_params.zipcode))
            {
                if (lokup > 0) sql += " AND ";
                else lokup = 1;

                sql += @" docs.zip='" + _params.zipcode + "'";
            }

            sql += @" 
                      -- AND docs.appt_id = " + _params.appt_type + @" 
                      --AND docs.spid = " + _params.specialty + "";



            return sql;
        }

      

        const double PIx = Math.PI;
        //const double RADIO = 6378.16;

        public static double Radians(double x)
        {
            return x * PIx / 180;
        }

        public double FindDistance(Double lat1, Double longi1, Double latpoint, Double longpoint)
        {
            //var sCoord = new System.Device.Location.GeoCoordinate(lat1, longi1);
            //var ref_coord = new System.Device.Location.GeoCoordinate(583838,-0030412);
            //var s = sCoord.GetDistanceTo(ref_coord);

            // double R = 6371; //km
            double R = 3958.756; // miles
            double dLat = Radians(latpoint - lat1);
            double dLon = Radians(longpoint - longi1);

            lat1 = Radians(lat1);
            latpoint = Radians(latpoint);

            double a =
             Math.Sin((latpoint - lat1) * (PIx / 180) / 2)
             * Math.Sin(dLat / 2)
             + Math.Sin(dLon / 2)
             * Math.Sin(dLon / 2)
             * Math.Cos(lat1)
             * Math.Cos(latpoint);

            //double a = 
            //    Math.Sin(dLat / 2) 
            //    * Math.Sin(dLat / 2) 
            //    + Math.Sin(dLon / 2) 
            //    * Math.Sin(dLon / 2)
            //    * Math.Cos(lat1) 
            //    * Math.Cos(latpoint);



            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c;

            return d;

        }


        [HttpGet]
        [Route("mobile/doctors/search/loguser")]
        public IHttpActionResult getDoctor_search_mobile_loguser([FromUri]doc_search_mobile_loguser _params)
        {
            try
            {
                //if (!authorize)
                //{
                //    return Json(new { data = new string[] { }, message = "Invalid authentication. Cannot continue.", success = false });
                //}

                // checking parameter it is in the class object
                NameValueCollection mapQUery = UriExtensions.ParseQueryString(HttpContext.Current.Request.Url);
                foreach (var k in mapQUery.Keys)
                {
                    var res = typeof(doc_search_mobile_loguser).GetProperty(k.ToString());
                    if (res == null)
                        return Json(new { data = new string[] { }, message = "Object reference not set to an instance of an object. Invalid parameter name: " + k.ToString(), success = false });
                }


                if (_params.take == 0)
                {
                    _params.take = 25;
                }
                //IsRequired("lat", _params.lat.ToString(), 2);
                if (_params.lat > 0)
                {
                    //    IsRequired("longi", _params.longi.ToString(), 2);
                }
                if (_params.longi > 0)
                {
                    //    IsRequired("lat", _params.lat.ToString(), 2);
                }

                //if (custom.haserror)
                //{
                //    return Json(new { data = new string[] { }, message = errmsg, success = false });
                //}

                //long user_id = _params.user_id;
                //// GET total_count
              
                //string sql1 = mobile_parsingSQL_loguser(_params, true);
             
                //var item_ = dbEntity.DOCTOR1.SqlQuery(sql1);
                //long item_count = item_.Count();

                //// parsing the SQL string
                //string sql = mobile_parsingSQL_loguser(_params);
                //// return getDirectory(sql);

                //var items = dbEntity.DOCTOR1.SqlQuery(sql);
                //// what happen if no .ToList()
                // var items = dbEntity.hs_DOCTOR.SqlQuery<hs_DOCTOR>(sql);

                //items = items.Skip(0).Take(25);
                List<long> condi = new List<long>();
                if (!string.IsNullOrEmpty(_params.search))
                {
                    //string str_search = _params.search.ToLower();
                    _params.search = _params.search.ToLower();
                }

                //var m_condi = dbEntity.ref_condition.Where(a => a.name.ToLower().Contains(str_search) || a.description.ToLower().Contains(str_search));
                //foreach (var c in m_condi)
                //{

                //    foreach () { }
                //    condi.Add(c.con_ref_specialty_ref_condition.);
                //}
                // filter conditions

                #region 
                var doc_items = from s in dbEntity.con_DOCTOR1_ref_specialty
                                join zip1 in dbEntity.ref_zip on s.DOCTOR1.home_addr_zip_id equals zip1.id

                                let distance = SqlFunctions.Asin(
                                               SqlFunctions.SquareRoot(
                                               SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (zip1.city_lat - _params.lat)) / 2)
                                             * SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (zip1.city_lat - _params.lat)) / 2)
                                             + SqlFunctions.Cos((SqlFunctions.Pi() / 180) * _params.lat)
                                             * SqlFunctions.Cos((SqlFunctions.Pi() / 180) * (zip1.city_lat))
                                             * SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (zip1.city_lon - _params.longi)) / 2)
                                             * SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (zip1.city_lon - _params.longi)) / 2)))


                                where
                                     distance <= 100 &
                                           (s.DOCTOR1.name_first.Contains(_params.search)
                                           || s.DOCTOR1.name_last.Contains(_params.search)
                                           || s.DOCTOR1.name_middle.Contains(_params.search)
                                           //01/17/2018 || s.ref_specialty1.conditions.Contains(_params.search)
                                           )
                                select new
                                {
                                    s.DOCTOR1.id,
                                    s.DOCTOR1.NPI,
                                    name_first = s.DOCTOR1.name_first == null ? "" : s.DOCTOR1.name_first,
                                    name_last = s.DOCTOR1.name_last == null ? "" : s.DOCTOR1.name_last,
                                    name_middle = s.DOCTOR1.name_middle == null ? "" : s.DOCTOR1.name_middle,
                                    organization_name = s.DOCTOR1.organization_name == null ? "" : s.DOCTOR1.organization_name,
                                    email = s.DOCTOR1.email == null ? "" : s.DOCTOR1.email,
                                    gender = s.DOCTOR1.gender == null ? "" : s.DOCTOR1.gender,
                                    title = s.DOCTOR1.title == null ? "" : s.DOCTOR1.title,
                                    phone = s.DOCTOR1.phone == null ? "" : s.DOCTOR1.phone,
                                    license_no = s.DOCTOR1.license_no == null ? "" : s.DOCTOR1.license_no,
                                    dob = s.DOCTOR1.dob == null ? "" : s.DOCTOR1.dob,
                                    image_url = s.DOCTOR1.image_url == null ? "" : s.DOCTOR1.image_url,
                                    pecos_certification = s.DOCTOR1.pecos_certification == null ? "" : s.DOCTOR1.pecos_certification,
                                    bio = s.DOCTOR1.bio == null ? "" : s.DOCTOR1.bio,
                                    distance,
                                    s.DOCTOR1.home_addr_1,
                                    s.DOCTOR1.home_addr_2,
                                    home_zip = zip1.zip,
                                    home_state = zip1.city_state,
                                    home_state_long = zip1.city_state_long,
                                    home_lat = zip1.city_lat,
                                    home_long = zip1.city_lon,
                                    home_city = zip1.city_name,
                                    home_county = zip1.city_county,

                                    pract_addr_1 = s.DOCTOR1.practice_addr_1,
                                    pract_addr_2 = s.DOCTOR1.practice_addr_2,
                                    pract_zip = s.DOCTOR1.ref_zip.zip,
                                    pract_state = s.DOCTOR1.ref_zip.city_state,
                                    pract_state_long = s.DOCTOR1.ref_zip.city_state_long,
                                    pract_lat = s.DOCTOR1.ref_zip.city_lat,
                                    pract_long = s.DOCTOR1.ref_zip.city_lon,
                                    pract_city = s.DOCTOR1.ref_zip.city_name,
                                    pract_county = s.DOCTOR1.ref_zip.city_county,

                                    con_spec = from sp in s.DOCTOR1.con_DOCTOR1_ref_specialty
                                               select new
                                               {
                                                   spec_id = sp.ref_specialty1.id,
                                                   spec_provider_type = sp.ref_specialty1.ref_specialty_provider.name,
                                                   spec_classification_code = sp.ref_specialty1.level2_classification_code,
                                                   spec_classification = sp.ref_specialty1.level2_classification,
                                                   spec_specialization_code = sp.ref_specialty1.level3_specialization_code,
                                                   spec_specialization = sp.ref_specialty1.level3_specialization,
                                                   spec_condition = from a2 in sp.ref_specialty1.ref_condition
                                                                    select a2.dname

                                               },

                                    con_ins = from ins in s.DOCTOR1.con_DOCTOR1_ref_insurance
                                              select new { ins_id = ins.ref_insurance_provider.id, ins_provider = ins.ref_insurance_provider.PayerName }
                                                ,
                                    ext = from d_ext in s.DOCTOR1.DOCTOR_ext1
                                          select new
                                          {
                                              ext_id = d_ext.id,
                                              ext_attr_name = d_ext.attr_name == null ? "" : d_ext.attr_name,
                                              ext_value = d_ext.value == null ? "" : d_ext.value
                                          },
                                    con_lang = from lang in s.DOCTOR1.con_DOCTOR1_ref_language
                                               select new { lang_id = lang.ref_languages.id, lang_name = lang.ref_languages.dname }

                                };
                #endregion

                var doc_items25 = from s in dbEntity.con_DOCTOR1_ref_specialty
                                      //join sc in dbEntity.con_ref_specialty_ref_condition on s.rel_ref_specialty_id equals sc.rel_ref_specialty_id
                                      //join sc in dbEntity.ref_condition on s.ref_specialty1.id equals sc.rel_ref_specialty_id
                                      join sc in dbEntity.ref_condition on  s.rel_ref_specialty_id equals sc.rel_ref_specialty_id
                                      
                                  join zip1 in dbEntity.ref_zip on s.DOCTOR1.home_addr_zip_id equals zip1.id

                                  let distance = SqlFunctions.Asin(
                                                 SqlFunctions.SquareRoot(
                                                 SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (zip1.city_lat - _params.lat)) / 2)
                                               * SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (zip1.city_lat - _params.lat)) / 2)
                                               + SqlFunctions.Cos((SqlFunctions.Pi() / 180) * _params.lat)
                                               * SqlFunctions.Cos((SqlFunctions.Pi() / 180) * (zip1.city_lat))
                                               * SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (zip1.city_lon - _params.longi)) / 2)
                                               * SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (zip1.city_lon - _params.longi)) / 2)))

                                  where
                                      distance <= 100
                                        &
                                             (s.DOCTOR1.name_first.Contains(_params.search)
                                              || s.DOCTOR1.name_last.Contains(_params.search)
                                              || s.DOCTOR1.name_middle.Contains(_params.search)

                                             || sc.dname.Contains(_params.search)
                                             )
                                  select new
                                {
                                    s.DOCTOR1.id,
                                    s.DOCTOR1.NPI,
                                    name_first = s.DOCTOR1.name_first == null ? "" : s.DOCTOR1.name_first,
                                    name_last = s.DOCTOR1.name_last == null ? "" : s.DOCTOR1.name_last,
                                    name_middle = s.DOCTOR1.name_middle == null ? "" : s.DOCTOR1.name_middle,
                                    organization_name = s.DOCTOR1.organization_name == null ? "" : s.DOCTOR1.organization_name,
                                    email = s.DOCTOR1.email == null ? "" : s.DOCTOR1.email,
                                    gender = s.DOCTOR1.gender == null ? "" : s.DOCTOR1.gender,
                                    title = s.DOCTOR1.title == null ? "" : s.DOCTOR1.title,
                                    phone = s.DOCTOR1.phone == null ? "" : s.DOCTOR1.phone,
                                    license_no = s.DOCTOR1.license_no == null ? "" : s.DOCTOR1.license_no,
                                    dob = s.DOCTOR1.dob == null ? "" : s.DOCTOR1.dob,
                                    image_url = s.DOCTOR1.image_url == null ? "" : s.DOCTOR1.image_url,
                                    pecos_certification = s.DOCTOR1.pecos_certification == null ? "" : s.DOCTOR1.pecos_certification,
                                    bio = s.DOCTOR1.bio == null ? "" : s.DOCTOR1.bio,
                                    distance,
                                    s.DOCTOR1.home_addr_1,
                                    s.DOCTOR1.home_addr_2,
                                    home_zip = zip1.zip,
                                    home_state = zip1.city_state,
                                    home_state_long = zip1.city_state_long,
                                    home_lat = zip1.city_lat,
                                    home_long = zip1.city_lon,
                                    home_city = zip1.city_name,
                                    home_county = zip1.city_county,

                                    pract_addr_1 = s.DOCTOR1.practice_addr_1,
                                    pract_addr_2 = s.DOCTOR1.practice_addr_2,
                                    pract_zip = s.DOCTOR1.ref_zip.zip,
                                    pract_state = s.DOCTOR1.ref_zip.city_state,
                                    pract_state_long = s.DOCTOR1.ref_zip.city_state_long,
                                    pract_lat = s.DOCTOR1.ref_zip.city_lat,
                                    pract_long = s.DOCTOR1.ref_zip.city_lon,
                                    pract_city = s.DOCTOR1.ref_zip.city_name,
                                    pract_county = s.DOCTOR1.ref_zip.city_county,

                                    con_spec = from sp in s.DOCTOR1.con_DOCTOR1_ref_specialty
                                               //join sc1 in dbEntity.con_ref_specialty_ref_condition on sp.rel_ref_specialty_id equals sc1.rel_ref_specialty_id
                                               select new
                                               {
                                                   spec_id = sp.ref_specialty1.id,
                                                   spec_provider_type = sp.ref_specialty1.ref_specialty_provider.name,
                                                   spec_classification_code = sp.ref_specialty1.level2_classification_code,
                                                   spec_classification = sp.ref_specialty1.level2_classification,
                                                   spec_specialization_code = sp.ref_specialty1.level3_specialization_code,
                                                   spec_specialization = sp.ref_specialty1.level3_specialization,
                                                   //spec_condition = from a1 in dbEntity.con_ref_specialty_ref_condition
                                                   //                 where sp.rel_ref_specialty_id == a1.rel_ref_specialty_id
                                                   //                 select new { a1.ref_condition.description }
                                                 
                                                                    // okay na ang condition diri        
                                                   spec_condition = from a2 in sp.ref_specialty1.ref_condition 
                                                             select  a2.dname
                                               },

                                    //condi1 = from a1  in dbEntity.ref_condition where s.ref_specialty1.id == a1.rel_ref_specialty_id
                                    //         select new { a1.dname },

                                    con_ins = from ins in s.DOCTOR1.con_DOCTOR1_ref_insurance
                                              select new { ins_id = ins.ref_insurance_provider.id, ins_provider = ins.ref_insurance_provider.PayerName }
                                                ,
                                    ext = from d_ext in s.DOCTOR1.DOCTOR_ext1
                                          select new
                                          {
                                              ext_id = d_ext.id,
                                              ext_attr_name = d_ext.attr_name == null ? "" : d_ext.attr_name,
                                              ext_value = d_ext.value == null ? "" : d_ext.value
                                          },
                                    con_lang = from lang in s.DOCTOR1.con_DOCTOR1_ref_language
                                               select new { lang_id = lang.ref_languages.id, lang_name = lang.ref_languages.dname }

                                };


                //doc_items25 = doc_items25.Where(a => a.distance <= 100
                //                                && (
                //                                   a.name_first.Contains(_params.search)
                //                                   || a.name_last.Contains(_params.search)
                //                                   || a.name_middle.Contains(_params.search)
                //                                   || a.cond1.Contains(_params.search)
                //                                )
                //                                );
                //doc_items25 = doc_items25.Where(a => a.condi1..Contains(_params.search));

                // doc_items15 = doc_items15.Where(b => b.condition.AsQueryable == _params.search));
                //doc_items = doc_items.Where(a => a.con_spec.Where( b => b.spec_condition.Contains(_params.search)));
                long cnt = doc_items25.Count();
                var doc_item15 = doc_items25.OrderBy(a => a.distance).Skip(_params.skip).Take(_params.take);

                // ===============================
                if (cnt == 0)
                {
                    return Json(new { data = new string[] { }, message = "No record found.", success = false });
                }

                List<doc_search_profile2> dc = new List<doc_search_profile2>();
                dc = getMobileDoctor(doc_item15);
             

                if (cnt > 0)
                {
                    var res = Newtonsoft.Json.JsonConvert.SerializeObject(dc);
                    var json = Newtonsoft.Json.Linq.JArray.Parse(res);

                    return Json(new { data = json, message = cnt + " Record(s) found.", success = true });
                }
                else
                {
                    return Json(new { data = new string[] { }, message = "No record found.", success = false });
                }

            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }
        }




        [HttpGet]
        [Route("mobile/doctors/search")]
        //[BasicAuthentication]
        public IHttpActionResult getDoctor_search_mobile([FromUri]doc_search_mobile _params)
        {
            try
            {
                //if (!authorize)
                //{
                //    return Json(new { data = new string[] { }, message = "Invalid authentication. Cannot continue.", success = false });
                //}

                // string username = Thread.CurrentPrincipal.Identity.Name;

                // validating parameters, if they are valid parameter's name
                NameValueCollection mapQUery = UriExtensions.ParseQueryString(HttpContext.Current.Request.Url);
                foreach (var k in mapQUery.Keys)
                {
                    var res = typeof(doc_search_mobile).GetProperty(k.ToString());
                    if (res == null)
                        return Json(new { data = new string[] { }, message = "Object reference not set to an instance of an object. Invalid parameter name: " + k.ToString(), success = false });
                }



                //IsRequired("lat", _params.lat.ToString(), 2);
                if (_params.lat > 0)
                {
                    //    IsRequired("longi", _params.longi.ToString(), 2);
                }
                if (_params.longi > 0)
                {
                    //    IsRequired("lat", _params.lat.ToString(), 2);
                }

                //if (custom.haserror)
                //{
                //    return Json(new { data = new string[] { }, message = errmsg, success = false });
                //}

                // 12/13/2017 long user_id = _params.user_id;

                if (_params.take == 0) _params.take = 25;

                var doc_items = from d in dbEntity.DOCTOR1
                                join zip1 in dbEntity.ref_zip on d.home_addr_zip_id equals zip1.id
                                //join zip2 in dbEntity.ref_zip on d.home_addr_zip_id equals zip1.id

                                let distance = SqlFunctions.Asin(
                                               SqlFunctions.SquareRoot(
                                               SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (zip1.city_lat - _params.lat)) / 2)
                                             * SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (zip1.city_lat - _params.lat)) / 2)
                                             + SqlFunctions.Cos((SqlFunctions.Pi() / 180) * _params.lat)
                                             * SqlFunctions.Cos((SqlFunctions.Pi() / 180) * (zip1.city_lat))
                                             * SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (zip1.city_lon - _params.longi)) / 2)
                                             * SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (zip1.city_lon - _params.longi)) / 2)))
                                //where geo.GetDistanceTo(new System.Device.Location.GeoCoordinate { Latitude = _params.lat, Longitude = _params.longi}) <= 1000
                                where distance <= 100
                                //where SqlFunctions.Asin(SqlFunctions.SquareRoot(SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (zip1.city_lat - _params.lat)) / 2) * SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (zip1.city_lat - _params.lat)) / 2) +
                                //    SqlFunctions.Cos((SqlFunctions.Pi() / 180) * _params.lat) * SqlFunctions.Cos((SqlFunctions.Pi() / 180) * (zip1.city_lat)) *
                                //    SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (zip1.city_lon - _params.longi)) / 2) * SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (zip1.city_lon  - _params.longi)) / 2)))  <= 1000
                                select new
                                {
                                    d.id,
                                    d.NPI,
                                    name_first = d.name_first == null ? "" : d.name_first,
                                    name_last = d.name_last == null ? "" : d.name_last,
                                    name_middle = d.name_middle == null ? "" : d.name_middle,
                                    organization_name = d.organization_name == null ? "" : d.organization_name,
                                    email = d.email == null ? "" : d.email,
                                    gender = d.gender == null ? "" : d.gender,
                                    title = d.title == null ? "" : d.title,
                                    phone = d.phone == null ? "" : d.phone,
                                    license_no = d.license_no == null ? "" : d.license_no,
                                    dob = d.dob == null ? "" : d.dob,
                                    image_url = d.image_url == null ? "" : d.image_url,
                                    pecos_certification = d.pecos_certification == null ? "" : d.pecos_certification,
                                    bio = d.bio == null ? "" : d.bio,


                                    distance,

                                    //distance2 = (new System.Device.Location.GeoCoordinate { Latitude = zip1.city_lat, Longitude = zip1.city_lon }).GetDistanceTo(new System.Device.Location.GeoCoordinate { Latitude = _params.lat, Longitude = _params.longi }),

                                    d.home_addr_1,
                                    d.home_addr_2,
                                    home_zip = zip1.zip,
                                    home_state = zip1.city_state,
                                    home_state_long = zip1.city_state_long,
                                    home_lat = zip1.city_lat,
                                    home_long = zip1.city_lon,
                                    home_city = zip1.city_name,
                                    home_county = zip1.city_county,

                                    pract_addr_1 = d.practice_addr_1,
                                    pract_addr_2 = d.practice_addr_2,
                                    pract_zip = d.ref_zip.zip,
                                    pract_state = d.ref_zip.city_state,
                                    pract_state_long = d.ref_zip.city_state_long,
                                    pract_lat = d.ref_zip.city_lat,
                                    pract_long = d.ref_zip.city_lon,
                                    pract_city = d.ref_zip.city_name,
                                    pract_county = d.ref_zip.city_county,

                                    ext = from d_ext in d.DOCTOR_ext1
                                                 select new
                                                 {
                                                     ext_id = d_ext.id,
                                                     ext_attr_name = d_ext.attr_name == null ? "" : d_ext.attr_name,
                                                     ext_value = d_ext.value == null ? "" : d_ext.value
                                                 },

                                    con_spec = from sp in d.con_DOCTOR1_ref_specialty
                                               select new
                                               {
                                                   spec_id = sp.ref_specialty1.id,
                                                   spec_provider_type = sp.ref_specialty1.ref_specialty_provider.name,
                                                   spec_classification_code = sp.ref_specialty1.level2_classification_code,
                                                   spec_classification = sp.ref_specialty1.level2_classification,
                                                   spec_specialization_code = sp.ref_specialty1.level3_specialization_code,
                                                   spec_specialization = sp.ref_specialty1.level3_specialization,
                                                   spec_condition = from a2 in sp.ref_specialty1.ref_condition
                                                                    select a2.dname
                                                                   
                                               },

                                    con_lang = from lang in d.con_DOCTOR1_ref_language
                                               select new { lang_id = lang.ref_languages.id, lang_name = lang.ref_languages.dname },

                                    con_ins = from ins in d.con_DOCTOR1_ref_insurance
                                              select new { ins_id = ins.ref_insurance_provider.id, ins_provider = ins.ref_insurance_provider.PayerName }


                                };

                long item_count = doc_items.Count();

                List<doc_search_profile2> dc = new List<doc_search_profile2>(); 

                //doc_items = doc_items.OrderBy(a => a.name_first).ThenBy(b => b.name_last).Skip(_params.skip).Take(_params.take);
                doc_items = doc_items.OrderBy(a => a.distance).Skip(_params.skip).Take(_params.take);

                // mobile/doctors/search
                dc = getMobileDoctor(doc_items);
                // balkon2

                if (item_count > 0)
                {
                    var res = Newtonsoft.Json.JsonConvert.SerializeObject(dc);
                    var json = Newtonsoft.Json.Linq.JArray.Parse(res);

                    return Json(new { data = json, message = item_count + " Record(s) found.", success = true });
                }
                else
                {
                    return Json(new { data = new string[] { }, message = "No record found.", success = false });
                }

            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = "No record found.", success = false });
            }
        }


        //**https://www.zocdoc.com/directory // doctor/directory
        //**https://www.zocdoc.com/practicedirectory // doctor/practicedirectory
        //https://www.zocdoc.com/morespecialties  // getspecialty
        //https://www.zocdoc.com/procedures   (except ours will be called "conditions")
        //https://www.zocdoc.com/languages
        //https://www.zocdoc.com/locations
        //https://www.zocdoc.com/hospital-directory
        //https://www.zocdoc.com/insurances

        private hs_DOCTOR mdoc_address;
        public hs_DOCTOR doc_address
        {
            get { return mdoc_address; }
            set { mdoc_address = value; }
        }

        [HttpGet]
        [Route("doctor/search")]
        public IHttpActionResult get_allsearch([FromUri] param_getall search)
        {
            try {

                //if (!authorize)
                //{
                //    return Json(new { data = new string[] { }, message = "Invalid authentication. Cannot continue.", success = false });
                //}

                if (search == null)
                {
                    return Json(new { data = new string[] { }, message = "No parameter is provided.", success = false });
                }

                if (search.take == 0) search.take = 25;


                // SEARCH DOCTOR with all the filters found in spreadsheet
                // name
                #region
                // this LINQ will not work, dili parejo ~/mobile/doctors/search/loguser, kay naa man WHERE
                var doc_namexx = from d in dbEntity.con_DOCTOR1_ref_specialty //from d in dbEntity.DOCTOR1
                                                                             //join ref_con in dbEntity.ref_condition on d.rel_ref_specialty_id equals ref_con.rel_specialty_id
                                join sc in dbEntity.ref_condition on d.rel_ref_specialty_id equals sc.rel_ref_specialty_id
                                join zip1 in dbEntity.ref_zip on d.DOCTOR1.home_addr_zip_id equals zip1.id
                                //join zip2 in dbEntity.ref_zip on d.home_addr_zip_id equals zip1.id
                                select new
                                {
                                    d.id,
                                    d.DOCTOR1.NPI,
                                    name_first = d.DOCTOR1.name_first == null ? "" : d.DOCTOR1.name_first,
                                    name_last = d.DOCTOR1.name_last == null ? "" : d.DOCTOR1.name_last,
                                    name_middle = d.DOCTOR1.name_middle == null ? "" : d.DOCTOR1.name_middle,
                                    organization_name = d.DOCTOR1.organization_name == null ? "" : d.DOCTOR1.organization_name,
                                    email = d.DOCTOR1.email == null ? "" : d.DOCTOR1.email,
                                    gender = d.DOCTOR1.gender == null ? "" : d.DOCTOR1.gender,
                                    title = d.DOCTOR1.title == null ? "" : d.DOCTOR1.title,
                                    phone = d.DOCTOR1.phone == null ? "" : d.DOCTOR1.phone,
                                    license_no = d.DOCTOR1.license_no == null ? "" : d.DOCTOR1.license_no,

                                    image_url = d.DOCTOR1.image_url == null ? "" : d.DOCTOR1.image_url,
                                    dob = d.DOCTOR1.dob == null ? "" : d.DOCTOR1.dob,
                                    bio = d.DOCTOR1.bio == null ? "" : d.DOCTOR1.bio,
                                    pecos_certification = d.DOCTOR1.pecos_certification == null ? "" : d.DOCTOR1.pecos_certification,

                                    d.DOCTOR1.home_addr_1,
                                    d.DOCTOR1.home_addr_2,
                                    home_zip = zip1.zip,
                                    home_state = zip1.city_state,
                                    home_state_long = zip1.city_state_long,
                                    home_lat = zip1.city_lat,
                                    home_long = zip1.city_lon,
                                    home_city = zip1.city_name,
                                    home_county = zip1.city_county,

                                    pract_addr_1 = d.DOCTOR1.practice_addr_1,
                                    pract_addr_2 = d.DOCTOR1.practice_addr_2,
                                    pract_zip = d.DOCTOR1.ref_zip.zip,
                                    pract_state = d.DOCTOR1.ref_zip.city_state,
                                    pract_state_long = d.DOCTOR1.ref_zip.city_state_long,
                                    pract_lat = d.DOCTOR1.ref_zip.city_lat,
                                    pract_long = d.DOCTOR1.ref_zip.city_lon,
                                    pract_city = d.DOCTOR1.ref_zip.city_name,
                                    pract_county = d.DOCTOR1.ref_zip.city_county,

                                    ext = from x in d.DOCTOR1.DOCTOR_ext1
                                          select new
                                          {
                                              ext_id = x.id,
                                              ext_attr_name = x.attr_name,
                                              ext_value = x.value
                                          },

                                    con_spec = from sp in d.DOCTOR1.con_DOCTOR1_ref_specialty
                                               select new
                                               {
                                                   spec_id = sp.ref_specialty1.id,
                                                   spec_provider_type = sp.ref_specialty1.ref_specialty_provider.name,
                                                   spec_classification_code = sp.ref_specialty1.level2_classification_code,
                                                   spec_classification = sp.ref_specialty1.level2_classification,
                                                   spec_specialization_code = sp.ref_specialty1.level3_specialization_code,
                                                   spec_specialization = sp.ref_specialty1.level3_specialization,
                                                   spec_condition = from a2 in sp.ref_specialty1.ref_condition
                                                                    select a2.dname
                                               },
                                    condi =  from c in d.DOCTOR1.con_DOCTOR1_ref_specialty
                                                     select c.rel_ref_specialty_id,

                                    con_lang = from lang in d.DOCTOR1.con_DOCTOR1_ref_language
                                               select new { lang_id = lang.ref_languages.id, lang_name = lang.ref_languages.dname },

                                    con_ins = from ins in d.DOCTOR1.con_DOCTOR1_ref_insurance
                                              select new { ins_id = ins.ref_insurance_provider.id, ins_provider = ins.ref_insurance_provider.PayerName }

                                              //,appointment = from appt  in dbEntity.APPOINTMENTs
                                              // select new { appt.doctor_id, appt.doctor_review } 
                                };
                #endregion

                var doc_name = (from d in dbEntity.DOCTOR1
                               //from s in dbEntity.con_DOCTOR1_ref_specialty
                               // join sc in dbEntity.ref_condition on d.con_DOCTOR1_ref_special equals sc.rel_ref_specialty_id
                               //from sc in dbEntity.ref_condition
                               //from sc in dbEntity.con_DOCTOR1_ref_specialty
                                join zip1 in dbEntity.ref_zip on d.home_addr_zip_id equals  zip1.id
                                //join zip2 in dbEntity.ref_zip on d.home_addr_zip_id equals zip1.id
                               // where(d.name_first.ToLower().Contains(search.doctor_name))
                               select  new {
                                   d.id, d.NPI,
                                   name_first = d.name_first == null ? "" : d.name_first, name_last = d.name_last == null ?"" : d.name_last, name_middle = d.name_middle == null? "": d.name_middle, organization_name = d.organization_name == null? "" : d.organization_name,
                                   email = d.email == null ? "" : d.email, gender = d.gender ==null? "" : d.gender, title = d.title == null?"" : d.title, phone = d.phone == null?"" : d.phone, license_no = d.license_no ==null?"": d.license_no,

                                   image_url = d.image_url == null ? "": d.image_url, 
                                   dob =  d.dob == null ? "" : d.dob,
                                   bio =  d.bio == null ? "" : d.bio,
                                   pecos_certification =  d.pecos_certification == null ? "" : d.pecos_certification,

                                   d.home_addr_1,  d.home_addr_2,
                                   home_zip = zip1.zip, home_state = zip1.city_state, home_state_long = zip1.city_state_long, home_lat = zip1.city_lat,
                                   home_long =  zip1.city_lon, home_city = zip1.city_name, home_county = zip1.city_county,

                                   pract_addr_1 = d.practice_addr_1, pract_addr_2 = d.practice_addr_2,
                                   pract_zip = d.ref_zip.zip, pract_state = d.ref_zip.city_state, pract_state_long = d.ref_zip.city_state_long,   pract_lat = d.ref_zip.city_lat,
                                   pract_long = d.ref_zip.city_lon, pract_city = d.ref_zip.city_name,  pract_county = d.ref_zip.city_county,

                                   ext = from x in d.DOCTOR_ext1
                                             select new {
                                                 ext_id = x.id, ext_attr_name =x.attr_name, ext_value = x.value
                                             },

                                   con_spec = from sp in d.con_DOCTOR1_ref_specialty
                                              select new
                                              {
                                                  spec_id = sp.ref_specialty1.id,
                                                  spec_provider_type = sp.ref_specialty1.ref_specialty_provider.name,
                                                  spec_classification_code = sp.ref_specialty1.level2_classification_code,
                                                  spec_classification = sp.ref_specialty1.level2_classification,
                                                  spec_specialization_code = sp.ref_specialty1.level3_specialization_code,
                                                  spec_specialization = sp.ref_specialty1.level3_specialization,
                                                  spec_condition = from a2 in sp.ref_specialty1.ref_condition
                                                                   select a2.dname  
                                              },
                                   condit = from c in d.con_DOCTOR1_ref_specialty
                                            select c.rel_ref_specialty_id,

                                   con_lang = from lang in d.con_DOCTOR1_ref_language
                                              select new { lang_id = lang.ref_languages.id, lang_name = lang.ref_languages.dname },

                                   con_ins = from ins in d.con_DOCTOR1_ref_insurance
                                             select new { ins_id = ins.ref_insurance_provider.id, ins_provider = ins.ref_insurance_provider.PayerName}

                                    //,appointment = from appt  in dbEntity.APPOINTMENTs
                                    // select new { appt.doctor_id, appt.doctor_review } 
                               });

                string display_value = "";


                // doctor name **********
                if (!string.IsNullOrEmpty(search.doctor_name))
                {

                    #region NAME
                    search.doctor_name = search.doctor_name.ToLower();
                    doc_name = doc_name.Where(a => a.name_first.ToLower().Contains(search.doctor_name)
                                                            || a.name_last.ToLower().Contains(search.doctor_name)
                                                            || a.name_middle.ToLower().Contains(search.doctor_name));

                    display_value = search.doctor_name;
                    #endregion
                }

                #region
                //// practice_name ********** applies only to Hospital/Organization
                //#region PRACTICE_NAME
                //if (!string.IsNullOrEmpty(ge.practice_name))
                //{
                //    ge.practice_name = ge.practice_name.ToLower();
                //    var doc_practice = dbEntity.hs_DOCTOR_ext.Where(a => a.attr_name.ToLower() == ge.practice_name);
                //}
                //#endregion
                #endregion


                // condition ***********
                //01/16/2018 List<long> con_doctor = new List<long>();
                //List<long> con_doctor = new List<long>();

                if (!string.IsNullOrEmpty(search.condition))
                {
                    //addToLog("condition_id", ge.condition_id);
                    #region CONDITION

                    // 01/16/2018 con_doctor = _getCondition_param(search.condition);

                    var sp = dbEntity.ref_condition.Where(a => a.dname.ToLower()== search.condition.ToLower()).FirstOrDefault();
                    // 01/16/2018 doc_name = doc_name.Where(a => con_doctor.Contains(a.id));
                    doc_namexx = doc_namexx.Where(a => a.condi.Contains(sp.rel_ref_specialty_id));

                  
                    #endregion
                }


                // specialty ***********
                List<long> spec_doc = new List<long>();
                if (!string.IsNullOrEmpty(search.specialty_id))
                {
                    //addToLog("specialty_id", ge.specialty_id);
                    #region SPECIALTY
                    spec_doc = _getSpecialty_param(search.specialty_id);


                    doc_name = doc_name.Where(a => spec_doc.Contains(a.id));
                    #endregion
                }


                // city *********** or State
                // TODO: expects a string, i,e. 'Alexandria, VA'
                List<long> city_doc = new List<long>();
                if (!string.IsNullOrEmpty(search.city))
                {
                    //addToLog("city_state", ge.city);
                    #region CITY    
                    city_doc = _getCity_param(search.city);
                    // 12/15/2017 doc_name = doc_name.Where(a => city_doc.Contains(a.home_addr_zip_id.Value));
                    #endregion
                }


                // state ********** STATE and CITY are one
                #region STATE
                //List<long> state_doc = new List<long>();
                //if (!string.IsNullOrEmpty(ge.state))
                //{
                //    ge.state = ge.state.ToLower();
                //    var doc_zip = dbEntity.ref_zip.Where(a => a.city_state.ToLower().Contains(ge.state));
                //    foreach (var i in doc_zip)
                //    {
                //        state_doc.Add(i.id);
                //    }
                //    doc_name = doc_name.Where(a => state_doc.Contains(a.addr_rel_ref_zip_id.Value));
                //}
                #endregion

                // language *************
                List<long> lang_doc = new List<long>();
                if (!string.IsNullOrEmpty(search.language_id))
                {
                    //addToLog("language_id", ge.language_id);
                    #region LANGUAGE

                    lang_doc = _getLanguage_param(search.language_id);

                    doc_name = doc_name.Where(a => lang_doc.Contains(a.id));
                    #endregion
                }


                // insurance *********
                List<long> ins_doc = new List<long>();
                if (!string.IsNullOrEmpty(search.insurance_id))
                {
                    //addToLog("insurance_id", ge.insurance_id);
                    #region INSURANCE
                    ins_doc = _getInsurance_param(search.insurance_id);


                    doc_name = doc_name.Where(a => ins_doc.Contains(a.id));
                    #endregion
                }


                // hospital_affiliation ********** not yet available
                #region HOSPITAL_AFFILIATION
                //List<long> hosp_doc = new List<long>();
                //if (!string.IsNullOrEmpty(ge.hospital_affiliation))
                //{
                //    ge.hospital_affiliation = ge.hospital_affiliation.ToLower();

                //    var con_hosp = dbEntity.con_DOCTOR_ref_Hospital.Where(a => a.hs_Hospital.org_name.ToLower().Contains(ge.hospital_affiliation));

                //    foreach (var i in con_hosp)
                //    {
                //        hosp_doc.Add(i.id);
                //    }

                //    doc_name = doc_name.Where(a => hosp_doc.Contains(a.id));
                //}
                #endregion


                //neil
                addToLog(search);

                var doc_name1 = doc_name.OrderBy(a => a.name_first).ThenBy(b => b.name_last).Skip(search.skip).Take(search.take);

                List<doc_search_profile2> doc_profile = new List<doc_search_profile2>();
                doc_profile = getMobileDoctor(doc_namexx);

             
                var ret1 = JsonConvert.SerializeObject(doc_profile);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);
                long doc_count = doc_name.Count();

                return Json(new { data = json1, message = doc_count + " Record(s) found.", success = true });

            }
            catch (Exception ex)
            {
                return Json(new { data = new string[] { }, message = ex.Message, success= false });
            }
           
        }

    
        // api: specialty/{id} :Nov 3,2017
        [System.Web.Http.HttpGet]
        [Route("doctor/specialty")]
        public IHttpActionResult getDoctor_specialty([FromUri] get_param param)
        {
            string msg = "";
            if (true)//progAuth.authorize()
            {
                //List<doc_specialty2> spec2 = _specialty(id);
                string name = "";
                int ntake = 25, nskip = 0;
                if (param != null )
                {
                    if(param.name != null) name = param.name.ToLower();
                    if (param.take > 0) ntake = param.take;

                    nskip = param.skip;
                }
              
                List<doc_condition2> con = new List<doc_condition2>();
                //01/11/2018 List<doc_specialty> spec1 = new List<doc_specialty>();
                List<doc_specialty_01112018> spec1 = new List<doc_specialty_01112018>();

                List<doc_search_profile2> doc = new List<doc_search_profile2>();

                //long specialty_id = 0;
                //bool bTemp = long.TryParse(id, out specialty_id);

                // 01/11/2018 var con_spec = dbEntity.ref_specialty1.Where(a => a.name.ToLower().Contains(name)
                //                || a.specialization.ToLower().Contains(name));

                var con_spec = from a in dbEntity.ref_specialty1
                               //join c in dbEntity.ref_condition on a.id equals c.rel_ref_specialty_id
                               select new {
                                   a.id,
                                   a.level2_classification, a.level2_classification_code,
                                   a.level3_specialization, a.level3_specialization_code,
                                   condition = from c1 in a.ref_condition
                                               select  c1,
                                   specialty_provider =  a.ref_specialty_provider.name
                               };

                if (param != null) 
                {
                    if(!string.IsNullOrEmpty(param.startwith))
                        con_spec = con_spec.Where(a => a.level2_classification.StartsWith(param.startwith) || a.level3_specialization.StartsWith(param.startwith)); // 


                    if (!string.IsNullOrEmpty(param.name))
                    {
                        con_spec = con_spec.Where(a => a.level2_classification.ToLower() == param.name || a.level3_specialization.ToLower() == param.name);
                    }
                }

                int cnt = con_spec.Count();

                con_spec = con_spec.OrderBy(a => a.level2_classification).ThenBy(b => b.level3_specialization).Skip(nskip).Take(ntake);
                foreach (var s in con_spec)
                {
                    // con = _getCondition(s.id);
                    //spec1.Add(new doc_specialty
                    //{
                    //    id = s.id,
                    //    code = s.code,
                    //    name = s.name,
                    //    provider_type = s.provider_type,
                    //    specialization = s.specialization,
                    //    conditions = con
                    //});

                    // 01/11/2018
                    // string[] con1 = s.conditions.Split('|');
                    List<string> condition = new List<string>();
                    foreach (var c in s.condition)
                    { condition.Add(c.dname); }

                    // 01/11/2018
                    spec1.Add(new doc_specialty_01112018
                    {
                        // id = n.ref_specialty.id,
                        // description = n.ref_specialty.description,
                        // name = n.ref_specialty.name,
                        // actor = n.ref_specialty.actor == null ? "" : n.ref_specialty.actor

                        id = s.id,
                        provider_type = s.specialty_provider,//  s.provider_type,

                        classification_code = s.level2_classification_code ==null? "" : s.level2_classification_code,
                        classification = s.level2_classification,

                        specialization_code = s.level3_specialization_code == null ? "" : s.level3_specialization_code,
                        specialization = s.level3_specialization == null ? "" : s.level3_specialization, // s.specialization,
                        conditions = condition
                    });
                }



                var ret1 = JsonConvert.SerializeObject(spec1);
                var json = Newtonsoft.Json.Linq.JArray.Parse(ret1);

                if (cnt > 0)
                    // get appointment_type_id
                    return Json(new { data = json, message = cnt + " Record(s) found.", success = true });
                else
                { return Json(new { data = json, message = cnt + " Record(s) found.", success = false }); }

            }
            else
            {
                msg = "The authorization header is either not valid or isn't Basic.";
                return Json(new { data = new string[] { }, message = msg, success = false });
                //throw new Exception("The authorization header is either not valid or isn't Basic.");
            }

        }

        private void addToLog(param_getall param)
        {
            //http://14.141.82.35/designer/brett/hs/procedures.html
            string[] d_value = HttpContext.Current.Request.QueryString.ToString().Split('&');
            string disp_value = "";
            string attr_name = "doctor_search";
            foreach (var d in d_value)
            {
                string[] kv = d.Split('=');
                long id = 0; bool b = false;

                 switch (kv[0])
                {
                    case "condition_id":
                        b = long.TryParse(kv[1], out id);
                        var c = dbEntity.ref_condition.Find(id);
                        if (c != null)  disp_value += c.dname + "|";
                        break;
                    case "specialty_id":
                        b = long.TryParse(kv[1], out id);
                        var s = dbEntity.ref_condition.Find(id);
                        if(s != null) disp_value += s.dname + "|";
                        break;
                    case "language_id":
                        b = long.TryParse(kv[1], out id);
                        var l = dbEntity.ref_condition.Find(id);
                        if (l != null) disp_value += l.dname + "|";
                        break;
                    case "insurance_id":
                        b = long.TryParse(kv[1], out id);
                        var i = dbEntity.ref_condition.Find(id);
                        if (i != null) disp_value += i.dname + "|";
                        break;

                    case "city":
                    case "doctor_name":
                    //case "ref":
                    //    disp_value += kv[1] + "|";

                        break;
                   
                }
            }
            if (disp_value.Count() > 0) {
                disp_value = disp_value.Substring(0, disp_value.Length - 1);

                top_search top = new top_search
                {
                    attr_name = attr_name,
                    value = HttpContext.Current.Request.QueryString.ToString(),
                    // url = HttpContext.Current.Request.Url.ToString(),
                    dt_create = dt,
                    create_by__USER_id = 0,
                    display_value = disp_value
                };


                dbEntity.top_search.Add(top);
                dbEntity.SaveChanges();
            }

            
        }

        // 01/16/2018 private List<long> _getCondition_param(string condition_id)
        //{
        //    List<long> con1 = new List<long>();
        //    List<long> con_doctor = new List<long>();
        //    //ge.condition = ge.condition.ToLower();
        //    string[] con_id = condition_id.Split(',');
        //    foreach (var c in con_id)
        //    {
        //        con1.Add(Convert.ToInt64(c));
        //    }

        //    //var con_condition = dbEntity.con_DOCTOR_ref_condition.Where(a => a.ref_condition.name.ToLower().Contains(ge.condition));
        //    //var con_condition = dbEntity.con_DOCTOR_ref_condition.Where(a => con1.Contains(a.rel_ref_condition_id.Value));
        //    var con_condition = dbEntity.con_ref_specialty_ref_condition.Where(a => con1.Contains(a.rel_ref_condition_id.Value));

        //    foreach (var c in con_condition)
        //    {
        //        //1/15/2018 var con_doctor_spec = dbEntity.con_DOCTOR_ref_specialty.Where(a => a.rel_ref_specialty_id == c.rel_ref_specialty_id);
        //        //if (con_doctor_spec.Count() > 0)
        //        //{
        //        //    con_doctor.Add(con_doctor_spec.FirstOrDefault().rel_DOCTOR_id.Value);
        //        //}

        //    }

        //    return con_doctor;
        //}


        private List<long> _getCondition_param(string condition)
        {
            // method created: 01/16/2018

            List<string> con1 = new List<string>();
            List<long> con_doctor = new List<long>();
            //ge.condition = ge.condition.ToLower();
            string[] con_id = condition.Split(',');
            foreach (var c in con_id)
            {
                //con1.Add(Convert.ToInt64(c));
                con1.Add(c);
            }

            var rc = dbEntity.ref_condition.Where(a=> con1.Contains(a.dname) );
            List<long> sp = new List<long>();
            foreach (var s in rc)
            {
                if(s.rel_ref_specialty_id !=null)
                sp.Add(s.rel_ref_specialty_id.Value);
            }

            //var con_condition = dbEntity.con_DOCTOR_ref_condition.Where(a => a.ref_condition.name.ToLower().Contains(ge.condition));
            //var con_condition = dbEntity.con_DOCTOR_ref_condition.Where(a => con1.Contains(a.rel_ref_condition_id.Value));
            //01/16/2018 var con_condition = dbEntity.con_ref_specialty_ref_condition.Where(a => con1.Contains(a.rel_ref_condition_id.Value));

            //01/16/2018 foreach (var c in con_condition)
            //{
            //    //1/15/2018 var con_doctor_spec = dbEntity.con_DOCTOR_ref_specialty.Where(a => a.rel_ref_specialty_id == c.rel_ref_specialty_id);
            //    //if (con_doctor_spec.Count() > 0)
            //    //{
            //    //    con_doctor.Add(con_doctor_spec.FirstOrDefault().rel_DOCTOR_id.Value);
            //    //}
            //}

            //01/16/2018 return con_doctor;
            return sp;
        }


        private List<long> _getSpecialty_param(string specialty_id)
        {
            // ge.specialty = ge.specialty.tolower();
            string[] spec_id = specialty_id.Split(',');
            List<long> spec1 = new List<long>();
            foreach (var c in spec_id)
            {
                spec1.Add(Convert.ToInt64(c));
            }

            //var doc_specialty = dbentity.con_doctor_ref_specialty.where(a => a.ref_specialty_def.code.tolower().contains(ge.specialty));
            var doc_specialty = dbEntity.con_DOCTOR_ref_specialty.Where(a => spec1.Contains(a.ref_specialty.id));
            List<long> spec_doc = new List<long>();
            foreach (var i in doc_specialty)
            {
                spec_doc.Add(i.rel_DOCTOR_id.Value);
            }

            return spec_doc;
        }
        private List<doc_specialty2> _getSpecialty(long condition_id = 0)
        {
            List<doc_specialty2> spec = new List<doc_specialty2>();
            //var ref_ = from a in dbEntity.ref_condition select a;
            var ref_ = from a in dbEntity.con_ref_specialty_ref_condition select a;

            if (condition_id > 0)
            {
                ref_ = ref_.Where(a => a.rel_ref_condition_id == condition_id);
            }

            ref_ = ref_.OrderByDescending(a => a.id);

            foreach (var m in ref_)
            {
                //spec.Add(new doc_specialty2
                //{
                //    id = m.ref_specialty.id,
                //    name = m.ref_specialty.name,
                //    code = m.ref_specialty.code,
                //    provider_type = m.ref_specialty.provider_type,
                //    specialization = m.ref_specialty.specialization
                //});

            }

            return spec;
        }
        private List<doc_specialty2> _specialty(string value)
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
        private List<long> _getCity_param(string city_state)
        {
            //ge.city = ge.city.ToLower();
            string[] cit1 = city_state.Split(',');
            string city = cit1[0].Trim().ToLower();
            string state = "";
         

            var doc_zip = dbEntity.ref_zip.Where(a => a.city_name.ToLower().Contains(city));
            if (cit1.Count() > 1)
            {
                state = cit1[1].Trim().ToLower();
                doc_zip = doc_zip.Where(a=> a.city_state.ToLower().Contains(state) || a.city_state_long.ToLower().Contains( state))  ;
            }
            
            List<long> city_doc = new List<long>();

            foreach (var i in doc_zip)
            {
                city_doc.Add(i.id);
            }

            return city_doc;
        }
        private List<long> _getLanguage_param(string language_id) {
            //ge.language = ge.language.tolower();
            string[] lang_id = language_id.Split(',');
            List<long> lang = new List<long>();
            foreach (var n in lang_id)
            {
                lang.Add(Convert.ToInt64(n));
            }

            var con_lang = dbEntity.con_DOCTOR_ref_language.Where(a => lang.Contains(a.rel_ref_language_id.Value));
            List<long> lang_doc = new List<long>();
            foreach (var i in con_lang)
            {
                lang_doc.Add(i.rel_DOCTOR_id.Value);
            }

            return lang_doc;
        }
        private List<long> _getInsurance_param(string insurance_id)
        {
            //ge.insurance = ge.insurance.ToLower();
            string[] ins_id = insurance_id.Split(',');
            List<long> ins = new List<long>();
            foreach (var n in ins_id)
            {
                ins.Add(Convert.ToInt64(n));
            }

            var con_ins = dbEntity.con_DOCTOR_ref_insurance.Where(a => ins.Contains(a.rel_ref_insurance_provider_id.Value));
            List<long> ins_doc = new List<long>();
            foreach (var i in con_ins)
            {
                ins_doc.Add(i.rel_DOCTOR_id.Value);
            }

            return ins_doc;
        }
     

        

     


        //private List<doc_search_profile2> getDoctors(IQueryable<dynamic> con_doctor) { }

        // api: specialty :Nov 3,2017
        [Route("doctoxr/specialty")]
        [HttpGet]
        private IHttpActionResult getxDoctor_specialty()
        {
            List<doc_specialty> spec = new List<doc_specialty>();
           
            var ref_ = from a in dbEntity.ref_specialty select a;

            foreach (var m in ref_)
            {
                List<doc_condition2> con = new List<doc_condition2>();
                con = _getCondition(m.id);
               

                spec.Add(new doc_specialty
                { id = m.id, name = m.name, code=m.code, provider_type = m.provider_type, specialization = m.specialization,
                   conditions = con
                });
            }


            var ret1 = JsonConvert.SerializeObject(spec);
            var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

            return Json(new { data = json1, message = "", success = true });
            //return Request.CreateResponse(HttpStatusCode.BadRequest, json1);
        }

        // api: condition :Nov 3,2017
        [Route("doctoxr/condition")]
        [HttpGet]
        private IHttpActionResult getxDoctor_condition()
        {
            //List<doc_specialty2>
            List<doc_condition> spec = new List<doc_condition>();
            List<doc_specialty2> spec1 = new List<doc_specialty2>();
            var ref_ = from a in dbEntity.ref_condition select a;

            foreach (var m in ref_)
            {
                spec1 = _getSpecialty(m.id);
                //1/16/2018 spec.Add(new doc_condition { id = m.id, name = m.dname, description = m.dname,
                //    specialty = spec1 
                //});
            }


            var ret1 = JsonConvert.SerializeObject(spec);
            var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

            return Json(new { data = json1, message = spec.Count() +  " record(s) found.", success = true });
            //return Request.CreateResponse(HttpStatusCode.BadRequest, json1);
        }

        [HttpGet]
        [Route("topsearch")]
        public IHttpActionResult getTopsearch()
        {

            var tops = (from a in dbEntity.top_search
                       group a.value by new {
                           display_value = a.display_value,
                           //value = a.value,
                           //url = a.url
                       } into g 
                       let count = g.Count()
                       orderby count descending 
                       select new {
                           //value = g.Key.value,
                           display_value = g.Key.display_value,
                           //url = g.Key.url,
                           count =  count
                       }).Take(10);

            List<topsearch> top = new List<topsearch>();

            top.OrderByDescending(a => a.count);
            foreach (var t in tops)
            {
                top.Add(new topsearch {
                    display_value = t.display_value == null ? "": t.display_value,
                    //url = t.url,
                    count = t.count
                });    
            }

            var ret = JsonConvert.SerializeObject(top);
            var json = Newtonsoft.Json.Linq.JArray.Parse(ret);
            return Json(new {data = json, message ="", success= true } );
        }

        private List<doc_condition2> _getCondition(long specialty_id = 0)
        {
            List<doc_condition2> spec = new List<doc_condition2>();
            //var ref_ = from a in dbEntity.ref_condition select a;
            var ref_ = from a in dbEntity.con_ref_specialty_ref_condition select a;

            if (specialty_id > 0)
            {
                // 1/15/2018 ref_ = ref_.Where(a => a.rel_ref_specialty_id == specialty_id);
            }

            ref_ = ref_.OrderByDescending(a => a.id);
            foreach (var m in ref_)
            {
                spec.Add(new doc_condition2 { id = m.ref_condition.id, name = m.ref_condition.dname,
                    description = m.ref_condition.dname });
            }

            return spec;
        }

  

    
        // api: language/{id}
        [HttpGet]
       // [Route("doctor/language/{id}")]
        private IHttpActionResult getxxDoctor_language(string id)
        {
            // removed param: 

            //var doc_ext = dbEntity.hs_DOCTOR_ext.Where(a => a.attr_name == "language_spoken" && (a.value.Contains("," + id + ",") || a.attr_name.Contains("" + id + ",") || a.attr_name.Contains("," + id + "")));
            var doc_ext = dbEntity.hs_DOCTOR_ext.Where(a => a.attr_name == "language_spoken"); //&& (a.value.Contains(id + ",") || a.value.Contains("," + id + ",") || a.value.Contains("," + id )));

            List<doc_search_profile2> dc2 = new List<doc_search_profile2>();
            foreach (var d in doc_ext)
            {
                string[] lng = d.value.Split(',');
                foreach (var i in lng)
                {
                    if (i == id) {
                        var lng2 = dbEntity.hs_DOCTOR.Find(d.rel_DOCTOR_id);
                        dc2.Add(_factorResponse(lng2));
                    }
                }
                // var d1 = dbEntity.hs_DOCTOR.Find(d.rel_DOCTOR_id);
                // dc2.Add(_factorResponse(d1));
            }

            if (dc2.Count() > 0)
            {
                var res1 = Newtonsoft.Json.JsonConvert.SerializeObject(dc2);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(res1);
                return Json(new { data = json1, message = dc2.Count() + " Record(s) found.", success = true });
            }
            else
            {

            }
            return Json(new { data = new string[] { }, message = "No record found.", success = false });
        }

        [HttpGet]
        [Route("doctor/language")]
        public IHttpActionResult getDoctor_language([FromUri]get_param param)
        {
            //par.name = par.name.ToLower();
            // var con_lang = dbEntity.ref_languages.Where(a => a.name.ToLower().Contains(name));
            var con_lang = from a in dbEntity.ref_languages select a ;
            string name = "";
            int ntake = 25, nskip = 0;
            if (param != null)
            {
                if (param.name != null) name = param.name.ToLower();
                if (param.take == 0) { ntake = 25; }
                else ntake = param.take;

                nskip = param.skip;
            }


            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.name))
                {
                    //name = param.name.ToLower();
                    con_lang = con_lang.Where(a => a.dname.Contains(name));
                }


                if (!string.IsNullOrEmpty(param.startwith))
                {
                    con_lang = con_lang.Where(a => a.dname.StartsWith(param.startwith));
                }
            }

            int cnt = con_lang.Count();
            con_lang = con_lang.OrderBy(a => a.dname).Skip(nskip).Take(ntake);

            List<doc_language> lang = new List<doc_language>();

            foreach (var n in con_lang)
            {
                lang.Add(new doc_language {
                    id = n.id,
                    name = n.dname
                });
            }

            var res1 = Newtonsoft.Json.JsonConvert.SerializeObject(lang);
            var json1 = Newtonsoft.Json.Linq.JArray.Parse(res1);

            if (cnt > 0)
            {
                return Json(new { data = json1, message = cnt + " Record(s) found.", success = true });
            }
            else
            {
                return Json(new { data = json1, message = cnt + " Record(s) found.", success = false });
            }
            
        }

        [HttpGet]
        [Route("doctor/states")]
        public IHttpActionResult getDoctor_states() //[FromUri]get_param param
        {
            //par.name = par.name.ToLower();
            string name = "";
            //if (param != null && param.name != null)
            //    name = param.name.ToLower();

            //var con_lang = dbEntity.ref_languages.Where(a => a.name.ToLower().Contains(name));
            var ref_state = (from a in dbEntity.ref_zip
                             join meta in dbEntity.hs_DOCTOR on a.id equals meta.practice_addr_zip_id
                             select new { a.city_state, a.city_state_long, a.city_name }).Distinct().OrderBy(b => b.city_state).ThenBy(c => c.city_name);


            List < doc_state > lang = new List<doc_state>();
            List<doc_state_cities> stat_cit = new List<doc_state_cities>();
            string stat = "", stat_long="";
           // ref_state.Distinct();
            foreach (var n in ref_state)
            {
                string state = n.city_state;
                string city = n.city_name;
               
             
                if (stat != n.city_state && stat_cit.Count() > 0)
                {
                   lang.Add(new doc_state { state = stat, state_long = stat_long , city= stat_cit });
                   stat_cit = new List<doc_state_cities>();
                }
                stat_cit.Add(new doc_state_cities
                {
                    name = n.city_name
                });

                stat = n.city_state;
                stat_long = n.city_state_long;
                //lang.Add(new doc_state { state = stat, city = stat_cit });
            }

            var res1 = Newtonsoft.Json.JsonConvert.SerializeObject(lang);
            var json1 = Newtonsoft.Json.Linq.JArray.Parse(res1);
            return Json(new { data = json1, message = lang.Count() + " Record(s) found.", success = true });

        }


        // api: cities :Nov 3, 2017
        [HttpGet]
        [Route("doctor/cities")]
        public IHttpActionResult getDoctor_cities([FromUri]get_param param)
        {
            string name = "";

            //if (param == null)
            //{
            //    return Json(new { data = new string[] { }, message = "Null parameter value.", success = false });
            //}

           

            List<doc_cities> doc = new List<doc_cities>();
            var _ref = from a in dbEntity.ref_zip
                       //where a.city_name.ToLower().Contains(name)
                       //orderby a.city_name + ", " + a.city_state
                       select a;

            if (param != null)
            {
                if (param.name != null) name = param.name.ToLower();

                _ref = _ref.Where(a => a.city_name.Contains(name)).OrderBy(d => d.city_name).ThenBy(c => c.city_state);
            }
            

            string b = "";
            foreach (var a in _ref)
            {

                if (b != a.city_name + ", " + a.city_state)
                {
                    doc.Add(new doc_cities
                    {
                        //id = a.id,
                        name = a.city_name + ", " + a.city_state
                    });
                }

                b = a.city_name + ", " + a.city_state;
            }

            var ret1 = JsonConvert.SerializeObject(doc);
            var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

            return Json(new { data = json1, message = doc.Count() + " Record(s) found.", success = true });
        }



        //[HttpGet]
        //[Route("search/cities")]
        //public IHttpActionResult getSearch_cities([FromUri]get_param par)
        //{
        //    //par.name = par.name.ToLower();
        //    var con_zip= dbEntity.ref_zip.Where(a => a.city_name.ToLower().Contains(par.name));
        //    List<search_city> city = new List<search_city>();

        //    foreach (var n in con_zip)
        //    {
        //        city.Add(new search_city
        //        {
        //            id = n.id,
        //            city = n.city_name,
        //            state = n.city_state,
        //            zip = n.zip
        //        });
        //    }

        //    var res1 = Newtonsoft.Json.JsonConvert.SerializeObject(city);
        //    var json1 = Newtonsoft.Json.Linq.JArray.Parse(res1);
        //    return Json(new { data = json1, message = city.Count() + " Record(s) found.", success = true });

        //}

        [HttpGet]
        [Route("doctor/condition")]
        public IHttpActionResult getDoctor_condition([FromUri]get_param param)
        {
            //par.name = par.name.ToLower();
            string name = "";

            int ntake = 25, nskip = 0;
            if (param != null)
            {
                if (param.name != null) name = param.name.ToLower();
                if (param.take == 0) { ntake = 25; }
                else ntake = param.take;

                nskip = param.skip;
            }


            //var con_zip = dbEntity.ref_zip.Where(a => a.city_name.ToLower().Contains(par.name));
            //1/16/2018 var con_condition = dbEntity.ref_condition.Where(a => a.dname.ToLower().Contains(name));
            var con_condition = (from a in dbEntity.ref_condition
                                 select a)
                               ;

            if (param != null) 
            {
                if(!string.IsNullOrEmpty(param.startwith))
                     con_condition = con_condition.Where(a => a.dname.StartsWith(param.startwith));
                

                if (!string.IsNullOrEmpty(param.name)) {
                    con_condition = con_condition.Where(a => a.dname.ToLower() == param.name);
                }
            }

            int cnt = con_condition.Count();
            con_condition = con_condition.OrderBy(a => a.dname).Skip(nskip).Take(ntake);
            List < doc_condition > cond = new List<doc_condition>();
         

            foreach (var n in con_condition)
            {
                //1/16/2018 List<doc_specialty2> spec1 = _getSpecialty(n.id);
                
                cond.Add(new doc_condition
                {
                   //id = n.id,
                   name = n.dname,
                   //description = n.dname,
                   //specialty = spec1
                });
            }

            var res1 = Newtonsoft.Json.JsonConvert.SerializeObject(cond);
            var json1 = Newtonsoft.Json.Linq.JArray.Parse(res1);

            if (cond.Count() > 0)
            {
                return Json(new { data = json1, message = cnt + " Record(s) found.", success = true });
            }
            else
            {
                return Json(new { data = json1, message = cnt + " Record(s) found.", success = false });
            }

        }

        // api: language
        [Route("doctoxr/language")]
        [HttpGet]
        private IHttpActionResult getxDoctor_language()
        {

            List<doc_language> lang = new List<doc_language>();
            var ref_ = from a in dbEntity.ref_languages select a;

            foreach (var m in ref_)
            {
                lang.Add(new doc_language { id = m.id, name = m.dname });
            }


            var ret1 = JsonConvert.SerializeObject(lang);
            var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

            return Json(new { data = json1, message = "", success = true });
            //return Request.CreateResponse(HttpStatusCode.BadRequest, json1);
        }

        // api: language/{id}
        [HttpGet]
        [Route("doctor/hospital-directory/{id}")]
        private IHttpActionResult getDoctor_hospital(string id)
        {

            //var doc_ext = dbEntity.hs_DOCTOR_ext.Where(a => a.attr_name == "language_spoken" && (a.value.Contains("," + id + ",") || a.attr_name.Contains("" + id + ",") || a.attr_name.Contains("," + id + "")));
            var doc_ext = dbEntity.hs_DOCTOR_ext.Where(a => a.attr_name == "hospital_affiliation"); //&& (a.value.Contains(id + ",") || a.value.Contains("," + id + ",") || a.value.Contains("," + id )));

            List<doc_search_profile2> dc2 = new List<doc_search_profile2>();
            foreach (var d in doc_ext)
            {
                string[] lng = d.value.Split(',');
                foreach (var i in lng)
                {
                    if (i == id)
                    {
                        var lng2 = dbEntity.hs_DOCTOR.Find(d.rel_DOCTOR_id);
                        dc2.Add(_factorResponse(lng2));
                    }
                }
                // var d1 = dbEntity.hs_DOCTOR.Find(d.rel_DOCTOR_id);
                // dc2.Add(_factorResponse(d1));
            }

            if (dc2.Count() > 0)
            {
                var res1 = Newtonsoft.Json.JsonConvert.SerializeObject(dc2);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(res1);
                return Json(new { data = json1, message = dc2.Count() + " Record(s) found.", success = true });
            }
            else
            {

            }
            return Json(new { data = new string[] { }, message = "No record found.", success = false });
         }

        // api: language
        [Route("doctor/hospital-directory")]
        [HttpGet]
        private IHttpActionResult getDoctor_hospital()
        {

            List<doc_hospital> org = new List<doc_hospital>();
            var ref_ = from a in dbEntity.hs_ORGANIZATION select a;

            foreach (var m in ref_)
            {
                org.Add(new doc_hospital { id = m.id, name = m.org_name });
            }


            var ret1 = JsonConvert.SerializeObject(org);
            var json1 = Newtonsoft.Json.Linq.JArray.Parse(ret1);

            return Json(new { data = json1, message = "", success = true });
            //return Request.CreateResponse(HttpStatusCode.BadRequest, json1);
        }

        public static List<insurance> _getInsurance(long doctor_id)
        {
            SV_db1Entities dbEntity = new SV_db1Entities();
            List<insurance> ins = new List<insurance>();
            var con_ins = dbEntity.con_DOCTOR_ref_insurance.Where(a => a.rel_DOCTOR_id == doctor_id);
            foreach (var n in con_ins) {
                ins.Add(new insurance {
                    id = n.rel_ref_insurance_provider_id.Value,
                    provider = n.ref_insurance_provider.PayerName.Split('|')[0]
                });
            }

            return ins;

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
            catch (Exception ex) {
                return null; }
            

        }


        private List<doc_search_profile2> _getInsurance_loop(IEnumerable<dynamic> doc, string id)
        {
            List<doc_search_profile2> dc2 = new List<doc_search_profile2>();

            List<insurance> ins = new List<insurance>();
            var ref_ins = dbEntity.ref_insurance_provider.Where(a => a.PayerName.ToLower().Contains(id));
            foreach (var n in ref_ins)
            {
                ins.Add(new insurance {
                    id = n.id,
                    provider = n.PayerName.Split('|')[0]
                });
            }

            

            foreach (var d in doc)
            {
                long doc_id = d.id == null ? 0 : d.id;
                var doc_ext = dbEntity.hs_DOCTOR_ext.Where(a => a.attr_name == "practice_insurance" && a.rel_DOCTOR_id == doc_id);

                foreach (var d2 in doc_ext)
                {
                    //ins_id.Add(n.id);
                    string[] ext = d2.value.Split(',');
                    foreach (var i in ext)
                    {
                        foreach (var n in ins)
                        {
                            if (i == n.id.ToString())
                            {
                                //var d1 = dbEntity.hs_DOCTOR.Find(d.rel_DOCTOR_id);
                                dc2.Add(_factorResponse(d));
                            }

                        }
                    }
                }
                
            }

            return dc2;
        }

        // api: doctor/insurance/{id}
        [HttpGet]
        [Route("doctor/insurance/{value}")]
        private IHttpActionResult getDoctor_insurance(string value)
        {
            value = value.ToLower();
            var ins = dbEntity.ref_insurance_provider.Where(a => a.PayerName.ToLower().Contains(value));

            var con_ins = dbEntity.con_DOCTOR_ref_insurance.Where(a => a.ref_insurance_provider.PayerName.ToLower().Contains(value));
            List<long> doc_ins = new List<long>();

            List<doc_search_profile2> dc2 = new List<doc_search_profile2>();
            foreach (var n in con_ins)
            {
                var d1 = dbEntity.hs_DOCTOR.Find(n.rel_DOCTOR_id);
                dc2.Add(_factorResponse(d1));
            }

            if (dc2.Count() > 0)
            {
                var res1 = Newtonsoft.Json.JsonConvert.SerializeObject(dc2);
                var json1 = Newtonsoft.Json.Linq.JArray.Parse(res1);
                return Json(new { data = json1, message = dc2.Count() + " Record(s) found.", success = true });
            }

            return Json(new { data = new string[] { }, message = "No Record found.", success = false });

            #region 


            //var doc_ext = dbEntity.hs_DOCTOR_ext.Where(a => a.attr_name == "practice_insurance"); // && (a.value.Contains(id + ",") || a.value.Contains("," + id + ",") || a.value.Substring("," + id)));
            //List<long> ins_id = new List<long>();
            //List<doc_search_profile2> dc2 = new List<doc_search_profile2>();

            //foreach (var n in doc_ext)
            //{
            //    //ins_id.Add(n.id);
            //    string[] ins = n.value.Split(',');
            //    foreach (var i in ins)
            //    {
            //        if (i == value)
            //        {
            //            var d1 = dbEntity.hs_DOCTOR.Find(n.rel_DOCTOR_id);
            //            dc2.Add(_factorResponse(d1));
            //        }
            //    }
            //}

            // if (dc2.Count() > 0)
            //{
            //    var res1 = Newtonsoft.Json.JsonConvert.SerializeObject(dc2);
            //    var json1 = Newtonsoft.Json.Linq.JArray.Parse(res1);
            //    return Json(new { data = json1, message = dc2.Count() + " Record(s) found.", success = true });
            //}

            //return Json(new { data = new string[] { }, message = "No Record found.", success = false });
            #endregion

        }

        // api: insurance
        [HttpGet]
        [Route("doctor/insurance")]
        public IHttpActionResult getDoctor_insurance([FromUri]get_param param)
        {
            //par.name = par.name.ToLower();
            string name = "";
            int take = 0, skip = 0;
            if (param != null)
            {
                if (param.name != null) name = param.name.ToLower();
                if (param.take == 0) { take = 25; }
                else take = param.take;
                skip = param.skip;
            }

            List<doc_language> lang = new List<doc_language>();
            var ref_ins = from a in dbEntity.ref_insurance_provider select a;
            //01/18/2018 var ref_ins = dbEntity.ref_insurance_provider.Where(a => a.PayerName.ToLower().Contains(name));

            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.startwith))
                    ref_ins = ref_ins.Where(a => a.PayerName.ToLower().StartsWith(param.startwith));


                if ( !string.IsNullOrEmpty(name))
                {
                    ref_ins = ref_ins.Where(a => a.PayerName.ToLower().Contains(name));
                }
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
            return Json(new { data = json1, message = ins.Count() + " Record(s) found.", success = true });
        }



        [HttpGet]
        [Route("doctor/practicedirectory")]
        public IHttpActionResult getpracticedirectory([FromUri]param_practicename  direct) //
        {
            //**https://www.zocdoc.com/practicedirectory
            // request parameters: 
            // beginning letter of the name
            // recently added doctor profile

            var doc = from a in dbEntity.hs_DOCTOR select a;
            //var doc = dbEntity.hs_DOCTOR.SqlQuery(sql).ToList
            List<hs_DOCTOR> doc_records = new List<hs_DOCTOR>();

            if (direct.recent)
            {
                doc = doc.OrderByDescending(a => a.dt_create);


            }
            else if (!string.IsNullOrEmpty(direct.practice_name))
            {
                var prac_spec = dbEntity.ref_specialty.Where(a => a.name.ToLower().Contains(direct.practice_name.ToLower())
                       || a.specialization.ToLower().Contains(direct.practice_name.ToLower()
               ));

                // Get all specialties with the parameter practice_name
                List<doc_search_profile2> dc1 = new List<doc_search_profile2>();
                List<long> sp = new List<long>();
                foreach (var n in prac_spec)
                {
                    //sp.Add(n.id);

                    var doc_sp = dbEntity.hs_DOCTOR_ext.Where(a => a.attr_name == "specialty_id" && (a.value.Contains("," + n.id + ",") || a.attr_name.Contains("" + n.id + ",") || a.attr_name.Contains("," + n.id + "")));
                    foreach (var rec in doc_sp)
                    {
                        
                        var d = dbEntity.hs_DOCTOR.Find(rec.rel_DOCTOR_id);
                        dc1.Add(_factorResponse(d));

                     

                    }
                }

                var res = Newtonsoft.Json.JsonConvert.SerializeObject(dc1);
                var json = Newtonsoft.Json.Linq.JArray.Parse(res);

                return Json(new { data = json, message = dc1.Count() + " Record(s) found.", success = true });

                // many to many relationship
                //JOIN hs_DOCTOR_ext spec ON spec.rel_DOCTOR_id = dc.id AND spec.attr_name='specialty_id' AND (spec.[value] like '%," + sp_id + ",%' or spec.[value] like '" + sp_id + ",%'  or spec.[value] like '%," + sp_id + "' or spec.[value] = '" + sp_id + "')";


            }

            // IF no practice_name is provided
            List<doc_search_profile2> dc2 = new List<doc_search_profile2>();
            foreach (var d in doc)
            {
                //var d = dbEntity.hs_DOCTOR.Find(rec.rel_DOCTOR_id);
                dc2.Add(_factorResponse(d));
            }

            var res1 = Newtonsoft.Json.JsonConvert.SerializeObject(dc2);
            var json1 = Newtonsoft.Json.Linq.JArray.Parse(res1);
            return Json(new { data = json1, message = dc2.Count() + " Record(s) found.", success = true });

            #region
            //List<doc_search_profile2> dc = new List<doc_search_profile2>();
            //if (doc.Count() == 0)
            //{
            //    return Json(new { data = new string[] { }, message = "No record found.", success = false });
            //}

            //foreach (var d in doc)
            //{
            //    doc_address = d;
            //    // practice_address is the one that is saved in the Doctor table

            //    var_getDoctor_ext d1 = getDoctor_ext(d.id);
            //    getDoctor_rating dr = get_averagerating(d.id, d.create_by__USER_id);


            //    dc.Add(new doc_search_profile2
            //    {
            //        id = d.id,
            //        first_name = d.name_first,
            //        last_name = d.name_last,
            //        middle_name = d.name_middle == null ? "" : d.name_middle,
            //        email = d.email == null ? "" : d.email,
            //        gender = d.gender == null ? "" : d.gender.Trim().ToUpper(),
            //        title = d.title == null ? "" : d.title,
            //        phone = d.phone == null ? "" : d.phone,
            //        license = d.license_no == null ? "" : d.license_no,
            //        npi = d.NPI == null ? "" : d.NPI,
            //        organ_name = d.organ_name == null ? "" : d.organ_name,
            //        image_url = d.image_url == null ? "" : d.image_url,
            //        rating = dr.average_rating,
            //        doctor_fee = d1.doc_fee,
            //        favorite = dr.fave,
            //        time_slot = d1.time_slot == null ? "" : d1.time_slot,
            //        bio = d.bio == null ? "" : d.bio,
            //        specialties = d1.spec == null ? new List<doc_specialty2>() { } : d1.spec,
            //        appointment_type = d1.appt_type == null ? new List<appt_type>() { } : d1.appt_type,
            //        home_address = d1.home_address == null ? new List<zip_search_address>() { } : d1.home_address,
            //        practice_address = d1.practice_address == null ? new List<zip_search_address>() { } : d1.practice_address
            //    });

            //}

            //var res = Newtonsoft.Json.JsonConvert.SerializeObject(dc);
            //var json = Newtonsoft.Json.Linq.JArray.Parse(res);

            //return Json(new { data = json, message = dc.Count() + " Record(s) found.", success = true });
            #endregion

        }

        private doc_search_profile2 _factorResponse(hs_DOCTOR d)
        {
            doc_address = d; // accepts hs_DOCTOR variable type
                             // practice_address is the one that is saved in the Doctor table
            List<zip_search_address> home_addr = custom._getDoctor_homeaddress(d);
            List<zip_search_address> pract_addr = custom._getDoctor_practiceaddress(d);

            var_getDoctor_ext d1 = _getDoctor_ext(d);
            // getDoctor_rating dr = _get_averagerating(d.id, d.create_by__USER_id);

            doc_search_profile2 dc1 = new doc_search_profile2();

            dc1 = (new doc_search_profile2
            {
                id = d.id,
                first_name = d.name_first,
                last_name = d.name_last,
                middle_name = d.name_middle == null ? "" : d.name_middle,
                email = d.email == null ? "" : d.email,
                gender = d.gender == null ? "" : d.gender.Trim().ToUpper(),
                title = d.title == null ? "" : d.title,
                phone = d.phone == null ? "" : d.phone,
                license = d.license_no == null ? "" : d.license_no,
                npi = d.NPI == null ? "" : d.NPI,
                organization_name = d.organization_name == null ? "" : d.organization_name,
                image_url = d.image_url == null ? "" : d.image_url,
              // balik  rating = dr.average_rating,
                doctor_fee = d1.doc_fee,
                
                // balik favorite = dr.fave,
                time_slot = d1.time_slot,
                bio = d.bio == null ? "" : d.bio,
                language_spoken = d1.language == null? new List<doc_language>() { } : d1.language,
                specialties = d1.spec == null ? new List<doc_specialty_01112018>() { } : d1.spec,
                appointment_type = d1.appt_type == null ? new List<appt_type>() { } : d1.appt_type,
                home_address = home_addr == null ? new List<zip_search_address>() { } : home_addr,
                practice_address = pract_addr == null ? new List<zip_search_address>() { } : pract_addr
            });

            return dc1;
        }

   

        public string insertLanguage() { 
            //https://www.zocdoc.com/languages

            string[] language = { "Afrikaans","Akan","Albanian","American Sign Language","Amharic","Arabic","Armenian","Assamese",
            "Assyrian","Azerbaijani","Bahasa","Balochi","Basque","Batak","Belarusian","Bengali","Bhojpuri","Bosnian","Bulgarian","Burmese",
            "Catalan","Cebuano","Chinese (Cantonese)","Chinese (Fujian)","Chinese (Mandarin)","Chinese (Shanghainese)","Chinese (Wenzhounese)",
            "Creole","Creole (Haitian)","Croatian","Czech","Danish","Dutch","English","Estonian","Farsi","Fijian","Filipino","Finnish","French",
            "Georgian","German","Greek","Guarani","Gujarati","Hakka","Hausa","Hebrew","Hindi","Hmong","Hungarian","Ibo","Icelandic","Igbo",
            "Ilokano","Indonesian","Irish","Italian","Japanese","Kannada","Kashmiri","Kazakh","Khmer","Kinyarwanda","Korean","Kurdish","Kyrgyz",
            "Lao","Latvian","Lithuanian","Macedonian","Maithili","Malagasy","Malay","Malayalam","Marathi","Mongolian","More","Navajo",
            "Nepali","Norwegian","Oriya","Oromo","Pashto","Polish","Portuguese","Punjabi","Quechua","Romani","Romanian","Russian","Serbian",
            "Serbo-Croatian","Shona","Sindhi","Sinhalese","Slovak","Slovenian","Somali","Spanish","Swahili","Swedish","Tagalog","Taiwanese",
            "Tajik","Tamil","Telugu","Thai","Tibetan","Tigrinya","Tshiluba","Tuareg","Turkish","Turkmen","Ukrainian","Urdu","Uzbek","Vietnamese",
            "Welsh","Xhosa","Yiddish","Yoruba","Zulu" };


            SV_db1Entities db = new SV_db1Entities();
            
            foreach (var n in language)
            {
                var db1 = db.ref_languages.Where(a => a.dname.ToLower() == n.ToLower());
                if (db1.Count() == 0)
                {
                    ref_languages ref_lang = new ref_languages();
                    ref_lang.dname = n;
                    db.ref_languages.Add(ref_lang);
                    db.SaveChanges();
                }
            }

            return "";
        }


        [HttpGet]
        [Route("doctor/directory")]
        public IHttpActionResult getDirectory([FromUri]param_directory param) //
        {

            //string s = getDoctor_language();
            //if (param == null)
            //{
            //    return Json(new { data = new string[] { }, message = "Null parameter value.", success = false });
            //}

            //direct.skip = 0;
            //direct.take = 10;
            //**https://www.zocdoc.com/directory
            // request parameters: 
            // beginning letter of the name
            // recently added doctor profile

            // var doc = from a in dbEntity.DOCTOR1 select a;
            var doc = from d in dbEntity.DOCTOR1
                               //join ref_con in dbEntity.ref_condition on d.rel_ref_specialty_id equals ref_con.rel_specialty_id
                           join zip1 in dbEntity.ref_zip on d.home_addr_zip_id equals zip1.id
                           //join zip2 in dbEntity.ref_zip on d.home_addr_zip_id equals zip1.id
                           select new
                           {
                               d.id,
                               d.NPI,
                               name_first = d.name_first == null ? "" : d.name_first,
                               name_last = d.name_last == null ? "" : d.name_last,
                               name_middle = d.name_middle == null ? "" : d.name_middle,
                               organization_name = d.organization_name == null ? "" : d.organization_name,
                               email = d.email == null ? "" : d.email,
                               gender = d.gender == null ? "" : d.gender,
                               title = d.title == null ? "" : d.title,
                               phone = d.phone == null ? "" : d.phone,
                               license_no = d.license_no == null ? "" : d.license_no,

                               image_url = d.image_url == null ? "" : d.image_url,
                               dob = d.dob == null ? "" : d.dob,
                               bio = d.bio == null ? "" : d.bio,
                               pecos_certification = d.pecos_certification == null ? "" : d.pecos_certification,

                               d.home_addr_1,
                               d.home_addr_2,
                               home_zip = zip1.zip,
                               home_state = zip1.city_state,
                               home_state_long = zip1.city_state_long,
                               home_lat = zip1.city_lat,
                               home_long = zip1.city_lon,
                               home_city = zip1.city_name,
                               home_county = zip1.city_county,

                               pract_addr_1 = d.practice_addr_1,
                               pract_addr_2 = d.practice_addr_2,
                               pract_zip = d.ref_zip.zip,
                               pract_state = d.ref_zip.city_state,
                               pract_state_long = d.ref_zip.city_state_long,
                               pract_lat = d.ref_zip.city_lat,
                               pract_long = d.ref_zip.city_lon,
                               pract_city = d.ref_zip.city_name,
                               pract_county = d.ref_zip.city_county,

                               ext = from x in d.DOCTOR_ext1
                                     select new
                                     {
                                         ext_id = x.id,
                                         ext_attr_name = x.attr_name,
                                         ext_value = x.value
                                     },

                               con_spec = from sp in d.con_DOCTOR1_ref_specialty
                                          select new
                                          {
                                              spec_id = sp.ref_specialty1.id,
                                              spec_provider_type = sp.ref_specialty1.ref_specialty_provider.name,
                                              spec_classification_code = sp.ref_specialty1.level2_classification_code,
                                              spec_classification = sp.ref_specialty1.level2_classification,
                                              spec_specialization_code = sp.ref_specialty1.level3_specialization_code,
                                              spec_specialization = sp.ref_specialty1.level3_specialization,
                                              // spec_condition = sp.ref_specialty1.conditions
                                              spec_condition = from a2 in sp.ref_specialty1.ref_condition
                                                               select a2.dname
                                          },

                               con_lang = from lang in d.con_DOCTOR1_ref_language
                                          select new { lang_id = lang.ref_languages.id, lang_name = lang.ref_languages.dname },

                               con_ins = from ins in d.con_DOCTOR1_ref_insurance
                                         select new { ins_id = ins.ref_insurance_provider.id, ins_provider = ins.ref_insurance_provider.PayerName }

                                         //,appointment = from appt  in dbEntity.APPOINTMENTs
                                         // select new { appt.doctor_id, appt.doctor_review } 
                           };

            if (param != null )
            { 
                if(param.take == 0) param.take = 25;
                if (!string.IsNullOrEmpty(param.startwith))
                {
                    doc = doc.Where(a => a.name_first.StartsWith(param.startwith)
                                        // 01/17/2018 || a.name_last.ToLower().Contains(search.doctor_name)
                                        // 01/17/2018 || a.name_middle.ToLower().Contains(search.doctor_name)
                                        );
                }
            }
            //var doc = dbEntity.hs_DOCTOR.SqlQuery(sql).ToList();
            //if (direct.is_date)
            //{
            //    doc = doc.OrderByDescending(a => a.dt_create);
            //}
            //else if (!string.IsNullOrEmpty(direct.letter))
            //{
            //    doc = doc.Where(a=> a.name_first.ToLower().Contains( direct.letter.ToLower() ) ||
            //        a.name_last.ToLower().Contains(direct.letter.ToLower())
            //    );
            //}



            List<doc_search_profile2> dc = new List<doc_search_profile2>();
            if (doc.Count() == 0)
            {
                return Json(new { data = new string[] { }, message = "No record found.", success = false });
            }

            int cnt = doc.Count();

            //doc = doc.Take(param.take).Skip(param.skip);
            doc = doc.OrderBy(a => a.name_first).ThenBy(b => b.name_last).Skip(param.skip).Take(param.take);
            dc = getMobileDoctor(doc);
            

            var res = Newtonsoft.Json.JsonConvert.SerializeObject(dc);
            var json = Newtonsoft.Json.Linq.JArray.Parse(res);

            return Json(new { data = json, message = cnt + " Record(s) found.", success=true});
        }

        [HttpGet]
        [Route("doctor/typeahead")]
        public IHttpActionResult getTypeAhead([FromUri]param_directory param )
        {

            var doctor = from d in dbEntity.DOCTOR1 select d;
            List<doct1> doc = new List<doct1>();

            int ntake = 25, nskip = 0;
            if (param != null)
            {
                //if (param.name != null) name = param.name.ToLower();
                if (param.take > 0) ntake = param.take;

                nskip = param.skip;
            }

            dynamic doctor1 ;
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.startwith))
                    doctor1 = doctor.Where(a => a.name_first.StartsWith(param.startwith)); // 


                if (!string.IsNullOrEmpty(param.name))
                {
                    doctor1 = doctor.Where(a => a.name_first.ToLower() == param.name);
                }
            }

             doctor1 = doctor.OrderBy(a => a.name_first).ThenBy(b=> b.name_last).Skip(nskip).Take(ntake);
            //con_spec = con_spec.OrderBy(a => a.level2_classification).ThenBy(b => b.level3_specialization).Skip(nskip).Take(ntake);
            foreach (var d in doctor1)
            {
                doc.Add(new doct1 {
                    id = d.id,
                    npi = d.NPI,
                    first_name = d.name_first == null? "" : d.name_first,
                    last_name = d.name_last == null ? "" : d.name_last,
                    middle_name = d.name_middle == null ? "" : d.name_middle,
                    dob = d.dob
                });
            }

            var ref_spec = dbEntity.con_DOCTOR1_ref_specialty.Where(a => a.ref_specialty1.level3_specialization.StartsWith(param.startwith));

            List<spec1> spec = new List<spec1>();
            foreach (var  s in ref_spec)
            {
                spec.Add(new spec1 {
                    id = s.id,
                    specialization = s.ref_specialty1.level3_specialization == null ? s.ref_specialty1.level3_specialization : s.ref_specialty1.level2_classification
                });
            }

            return Json(new { doctor = doc, specialization = spec });
        }

       

        /// <summary>
        /// Get Doctor_ext details
        /// </summary>
        /// <param name="doctor_id">The doctor_id</param>
        /// <returns></returns>
        public var_getDoctor_ext _getDoctor_ext(hs_DOCTOR doc) {
            SV_db1Entities dbEntity = new SV_db1Entities();
            var doc_ext = dbEntity.hs_DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doc.id);

            var_getDoctor_ext dx = new var_getDoctor_ext();

            dx.spec = _getDoctor_specialty(doc.id);
            dx.language = _getLanguage(doc.id);
            dx.insurance = _getInsurance(doc.id);
            //dx.home_address = custom._getDoctor_homeaddress(doc);
            //dx.practice_address = custom._getDoctor_practiceaddress(doc);

         
            foreach (var n in doc_ext)
            {
                
                switch (n.attr_name)
                {
                    case "language_spoken":
                        // removed from doctor_ext
                        break;
                    case "fee":
                        double d_fee;
                        bool bTemp = double.TryParse(n.value, out d_fee);

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
                        //dx.appt_type = _getDoctor_appointmenttype(n.value);

                        long vappt_id = 0;
                        bool isAppt = long.TryParse(n.value, out vappt_id);
                        if (isAppt)
                        {

                            dx.appt_type.Add(_getDoctor_appointmenttype(vappt_id));
                        }
                        break;

                    case "time_slot":
                        // dx.time_slot = n.value == null ? "" : n.value;
                        dx.time_slot.Add(n.value);
                        break;

                    case "education":
                        // dx.education = n.value;
                        dx.education.Add(n.value);
                        break;
                    case "experience":
                        // dx.experience = n.value;
                        dx.experience.Add(n.value);
                        break;

                        #region
                        //case "dob": dx.dob = n.value; break;

                        //case "gender": dx.gender = n.value; break;
                        // case "email": _email = dx.value; break;

                        //case "personal_practice_type": dx.personal_practice_type = n.value; break;

                        // case "home_street1": _home_street1 = dx.value; break;
                        // case "home_street2": _home_street2 = dx.value; break;
                        // case "home_city": _home_city = dx.value; break;
                        // case "home_state": _home_state = dx.value; break;
                        //case "home_zip":
                        //    dx.home_zip = dx.value;
                        //    string z = dx.home_zip.Substring(0, 5);
                        //    var h_zip = dbEntity.ref_zip.Where(a => a.zip == z);
                        //    dx.home_city = h_zip.FirstOrDefault().city_name; //_home_city = n.ref_zip == null ? "" :  n.ref_zip.city_name;
                        //    dx.home_state = h_zip.FirstOrDefault().city_state;  //_home_state = n.ref_zip == null ? "" : n.ref_zip.city_state;
                        //    break;

                        //case "education": dx.education = n.value; break;
                        //case "experience": dx.experience = n.value; break;

                        //case "language_spoken": dx.language_spoken = n.value; break;

                        //case "board_certification": dx.board_certification = n.value; break;

                        //case "specialty":
                        //case "specialty_id":
                        //    dx.specialty = n.value;
                        //    //spec = null;
                        //    string[] get_spec = n.value.Split(',');

                        //    foreach (var i in get_spec)
                        //    {

                        //        long spec_id = 0;
                        //        bool s1 = long.TryParse(i, out spec_id);

                        //        if (s1)
                        //        {
                        //            var ref_spec = dbEntity.ref_specialty.Find(spec_id);

                        //            //spec.Add(new doc_specialty
                        //            //{
                        //            //    id = ref_spec.id,
                        //            //    code = ref_spec.code == null? "" : ref_spec.code,
                        //            //    description = ref_spec.description ==null? "": ref_spec.description,
                        //            //    name = ref_spec.name==null?"": ref_spec.name,
                        //            //    actor = ref_spec.actor == null ? "" : ref_spec.actor
                        //            //});
                        //            spec1.Add(new doc_specialty2
                        //            {
                        //                id = ref_spec.id,
                        //                name = ref_spec.name,
                        //                code = ref_spec.code,
                        //                specialization = ref_spec.specialization,
                        //                provider_type = ref_spec.provider_type
                        //            });

                        //        }

                        //    }

                        //    break;

                        //case "hospital_affiliation": dx.hospital_affiliation = n.value; break;
                        //case "practice_npi": dx.practice_npi = n.value; break;

                        //case "practice_name": dx.practice_name = n.value; break;

                        //case "practice_type": dx.practice_type = n.value; break;

                        //case "dea": dx.dea = n.value; break;

                        //case "clinician_role": dx.clinician_role = n.value; break;

                        //case "scheduling_solution": dx.scheduling_solution = n.value; break;

                        //case "current_scheduling": dx.current_scheduling = n.value; break;

                        //case "practice_street": dx.practice_street = n.value; break;

                        ////case "practice_zip": // service zip codes
                        ////    // removed from ext

                        ////    break;
                        //case "practice_phone_primary":// primary phone number
                        //    dx.practice_phone_primary = n.value; break;

                        //case "practice_fax": dx.practice_fax = n.value; break;

                        //case "practice_phone_cs": // customer service number
                        //    dx.practice_phone_cs = n.value; break;

                        //case "practice_phone_office":// office phone
                        //    dx.practice_phone_office = n.value; break;

                        //case "practice_clinicians": // no of field clinicians
                        //    dx.practice_clinicians = n.value; break;

                        //case "practice_exams": // no of exams you can handle per week
                        //    dx.practice_exams = n.value; break;

                        //case "geographic_market": // geographic market
                        //    dx.geographic_market = n.value; break;

                        //case "practice_expansion": // future expansion plans, new market, 
                        //    dx.practice_expansion = n.value; break;

                        //case "practice_insurance":// insurance list you are in Network or will acccept
                        //    dx.practice_insurance = n.value; break;

                        //case "practice_tax_number":// federal tax id number
                        //    dx.practice_tax_number = n.value; break;

                        //case "primary_contact_name":
                        //    dx.primary_contact_name = n.value;
                        //    break;

                        //case "primary_contact_phone":
                        //    dx.primary_contact_phone = n.value;
                        //    break;

                        //case "primary_contact_email":
                        //    dx.primary_contact_email = n.value;
                        //    break;

                        //case "operational_contact_name":
                        //    dx.operational_contact_name = n.value;
                        //    break;

                        //case "operational_contact_phone": dx.operational_contact_phone = n.value; break;

                        //case "operational_contact_email": dx.operational_contact_email = n.value; break;

                        //case "financial_contact_name": dx.financial_contact_name = n.value; break;

                        //case "financial_contact_phone": dx.financial_contact_phone = n.value; break;

                        //case "financial_contact_email": dx.financial_contact_email = n.value; break;

                        //case "practice_emr":  // emr software that you are currently using
                        //    dx.practice_emr = n.value; break;

                        //case "network_insurance":// in-network insurances
                        //    dx.network_insurance = n.value; break;

                        //// public string practice_contact { get; set; } // primary contact/ operational contact/ financial contact
                        //case "billing_bankname": dx.billing_bankname = n.value; break;
                        //case "billing_account": dx.billing_account = n.value; break;
                        //case "billing_routing": dx.billing_routing = n.value; break;
                        #endregion

                }


            }
            return dx;

        }


       
        public  var_getDoctor_ext _getDoctor_ext(DOCTOR1 doc)
        {
            SV_db1Entities dbEntity = new SV_db1Entities();
            var doc_ext = dbEntity.hs_DOCTOR_ext.Where(a => a.rel_DOCTOR_id == doc.id);

            var_getDoctor_ext dx = new var_getDoctor_ext();

            dx.spec = _getDoctor_specialty(doc.id);
            dx.language = _getLanguage(doc.id);
            dx.insurance = _getInsurance(doc.id);
            //dx.home_address = custom._getDoctor_homeaddress_doc1(doc);
            //dx.practice_address = custom._getDoctor_practiceaddress_doc1(doc);


            foreach (var n in doc_ext)
            {

                switch (n.attr_name)
                {
                    case "language_spoken":
                        // removed from doctor_ext
                        break;
                    case "fee":
                        double d_fee;
                        bool bTemp = double.TryParse(n.value, out d_fee);

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
                        bool isAppt = long.TryParse(n.value, out vappt_id);
                        if (isAppt)
                        {

                            dx.appt_type.Add(_getDoctor_appointmenttype(vappt_id));
                        }
                        break;

                    case "time_slot":
                        dx.time_slot.Add(n.value); //== null ? "" : n.value;
                        break;

                    case "education":
                        dx.education.Add(n.value);
                        break;
                    case "experience":
                        dx.experience.Add(n.value);
                        break;

                        #region 
                        //case "dob": dx.dob = n.value; break;

                        //case "gender": dx.gender = n.value; break;
                        // case "email": _email = dx.value; break;

                        //case "personal_practice_type": dx.personal_practice_type = n.value; break;

                        // case "home_street1": _home_street1 = dx.value; break;
                        // case "home_street2": _home_street2 = dx.value; break;
                        // case "home_city": _home_city = dx.value; break;
                        // case "home_state": _home_state = dx.value; break;
                        //case "home_zip":
                        //    dx.home_zip = dx.value;
                        //    string z = dx.home_zip.Substring(0, 5);
                        //    var h_zip = dbEntity.ref_zip.Where(a => a.zip == z);
                        //    dx.home_city = h_zip.FirstOrDefault().city_name; //_home_city = n.ref_zip == null ? "" :  n.ref_zip.city_name;
                        //    dx.home_state = h_zip.FirstOrDefault().city_state;  //_home_state = n.ref_zip == null ? "" : n.ref_zip.city_state;
                        //    break;

                        //case "education": dx.education = n.value; break;
                        //case "experience": dx.experience = n.value; break;

                        //case "language_spoken": dx.language_spoken = n.value; break;

                        //case "board_certification": dx.board_certification = n.value; break;

                        //case "specialty":
                        //case "specialty_id":
                        //    dx.specialty = n.value;
                        //    //spec = null;
                        //    string[] get_spec = n.value.Split(',');

                        //    foreach (var i in get_spec)
                        //    {

                        //        long spec_id = 0;
                        //        bool s1 = long.TryParse(i, out spec_id);

                        //        if (s1)
                        //        {
                        //            var ref_spec = dbEntity.ref_specialty.Find(spec_id);

                        //            //spec.Add(new doc_specialty
                        //            //{
                        //            //    id = ref_spec.id,
                        //            //    code = ref_spec.code == null? "" : ref_spec.code,
                        //            //    description = ref_spec.description ==null? "": ref_spec.description,
                        //            //    name = ref_spec.name==null?"": ref_spec.name,
                        //            //    actor = ref_spec.actor == null ? "" : ref_spec.actor
                        //            //});
                        //            spec1.Add(new doc_specialty2
                        //            {
                        //                id = ref_spec.id,
                        //                name = ref_spec.name,
                        //                code = ref_spec.code,
                        //                specialization = ref_spec.specialization,
                        //                provider_type = ref_spec.provider_type
                        //            });

                        //        }

                        //    }

                        //    break;

                        //case "hospital_affiliation": dx.hospital_affiliation = n.value; break;
                        //case "practice_npi": dx.practice_npi = n.value; break;

                        //case "practice_name": dx.practice_name = n.value; break;

                        //case "practice_type": dx.practice_type = n.value; break;

                        //case "dea": dx.dea = n.value; break;

                        //case "clinician_role": dx.clinician_role = n.value; break;

                        //case "scheduling_solution": dx.scheduling_solution = n.value; break;

                        //case "current_scheduling": dx.current_scheduling = n.value; break;

                        //case "practice_street": dx.practice_street = n.value; break;

                        ////case "practice_zip": // service zip codes
                        ////    // removed from ext

                        ////    break;
                        //case "practice_phone_primary":// primary phone number
                        //    dx.practice_phone_primary = n.value; break;

                        //case "practice_fax": dx.practice_fax = n.value; break;

                        //case "practice_phone_cs": // customer service number
                        //    dx.practice_phone_cs = n.value; break;

                        //case "practice_phone_office":// office phone
                        //    dx.practice_phone_office = n.value; break;

                        //case "practice_clinicians": // no of field clinicians
                        //    dx.practice_clinicians = n.value; break;

                        //case "practice_exams": // no of exams you can handle per week
                        //    dx.practice_exams = n.value; break;

                        //case "geographic_market": // geographic market
                        //    dx.geographic_market = n.value; break;

                        //case "practice_expansion": // future expansion plans, new market, 
                        //    dx.practice_expansion = n.value; break;

                        //case "practice_insurance":// insurance list you are in Network or will acccept
                        //    dx.practice_insurance = n.value; break;

                        //case "practice_tax_number":// federal tax id number
                        //    dx.practice_tax_number = n.value; break;

                        //case "primary_contact_name":
                        //    dx.primary_contact_name = n.value;
                        //    break;

                        //case "primary_contact_phone":
                        //    dx.primary_contact_phone = n.value;
                        //    break;

                        //case "primary_contact_email":
                        //    dx.primary_contact_email = n.value;
                        //    break;

                        //case "operational_contact_name":
                        //    dx.operational_contact_name = n.value;
                        //    break;

                        //case "operational_contact_phone": dx.operational_contact_phone = n.value; break;

                        //case "operational_contact_email": dx.operational_contact_email = n.value; break;

                        //case "financial_contact_name": dx.financial_contact_name = n.value; break;

                        //case "financial_contact_phone": dx.financial_contact_phone = n.value; break;

                        //case "financial_contact_email": dx.financial_contact_email = n.value; break;

                        //case "practice_emr":  // emr software that you are currently using
                        //    dx.practice_emr = n.value; break;

                        //case "network_insurance":// in-network insurances
                        //    dx.network_insurance = n.value; break;

                        //// public string practice_contact { get; set; } // primary contact/ operational contact/ financial contact
                        //case "billing_bankname": dx.billing_bankname = n.value; break;
                        //case "billing_account": dx.billing_account = n.value; break;
                        //case "billing_routing": dx.billing_routing = n.value; break;
                        #endregion

                }


            }
            return dx;

        }

  

        public var_getDoctor_ext _getDoctor_ext_doc1(ICollection<DOCTOR_ext1> doc_ext, long doc_id )
        {
            SV_db1Entities dbEntity = new SV_db1Entities();
            // var doc_ext1 = dbEntity.DOCTOR_ext1.Where(a => a.rel_DOCTOR_id == doc_ext.id);

            var_getDoctor_ext dx = new var_getDoctor_ext();

            // 12/15/2017 dx.spec = _getDoctor_specialty_doc1(doc_id); // _getDoctor_specialty(doc.id);
            // 12/15/2017 dx.language = _getLanguage_doc1(doc_id); // _getLanguage(doc.id);
            // 12/15/2017 dx.insurance = _getInsurance_doc1(doc_id); // _getInsurance(doc.id);
            // 12/15/2017 dx.home_address = custom._getDoctor_homeaddress_doc1(doc); // custom._getDoctor_homeaddress(doc);
            // 12/15/2017 dx.practice_address = custom._getDoctor_practiceaddress_doc1(doc); // custom._getDoctor_practiceaddress(doc);

            //var ext = doc_ext.Where(a => a.attr_name == "fee");
            //double d_fee;
            //bool bTemp = double.TryParse(ext.FirstOrDefault().value, out d_fee);
            //if (bTemp) dx.doc_fee = d_fee;

            //var ext1 = doc_ext.Where(a => a.attr_name == "appt_type");
            //// dx.appt_type = _getDoctor_appointmenttype(n.value);
            //dx.appt_type = _getDoctor_appointmenttype(ext1.FirstOrDefault().value);

            //var ext2 = doc_ext.Where(a => a.attr_name == "time_slot");
            ////dx.time_slot = n.value == null ? "" : n.value;
            //dx.time_slot = ext2.FirstOrDefault().value;

            //var ext3 = doc_ext.Where(a => a.attr_name == "education");
            //// dx.education = n.value;
            //dx.education = ext3.FirstOrDefault().value;

            //var ext4 = doc_ext.Where(a => a.attr_name == "experience");
            //// dx.experience = n.value;
            //dx.experience = ext4.FirstOrDefault().value;

            foreach (var n in doc_ext)
            {

                switch (n.attr_name)
                {
                    case "language_spoken":
                        // removed from doctor_ext
                        break;
                    case "fee":
                        double d_fee;
                        bool bTemp = double.TryParse(n.value, out d_fee);

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
                        //long vappt_id = 0;
                        //bool isAppt = long.TryParse(n.value, out vappt_id);
                        //if (isAppt)
                        //{
                        //    appt_type a = new appt_type();
                        //    a = _getDoctor_appointmenttype(vappt_id);
                        //    var n1 = dbEntity.ref_APPOINTMENT_type.Find(vappt_id);
                        //    if (n1 != null)
                        //    {
                        //        dx.appt_type.Add(new appt_type { id = n1.id, type = n1.dname });
                        //    }
                        //    // dx.appt_type.Add(a);
                        //    //dx.appt_type.Add(_getDoctor_appointmenttype(vappt_id));
                            
                        //}

                        break;

                    case "time_slot":
                        // dx.time_slot = n.value == null ? "" : n.value;
                        dx.time_slot.Add(n.value);
                        break;

                    case "education":
                        // dx.education = n.value;
                        dx.education.Add(n.value);
                        break;
                    case "experience":
                        // dx.experience = n.value;
                        dx.experience.Add(n.value);
                        break;

                        #region
                        //case "dob": dx.dob = n.value; break;

                        //case "gender": dx.gender = n.value; break;
                        // case "email": _email = dx.value; break;

                        //case "personal_practice_type": dx.personal_practice_type = n.value; break;

                        // case "home_street1": _home_street1 = dx.value; break;
                        // case "home_street2": _home_street2 = dx.value; break;
                        // case "home_city": _home_city = dx.value; break;
                        // case "home_state": _home_state = dx.value; break;
                        //case "home_zip":
                        //    dx.home_zip = dx.value;
                        //    string z = dx.home_zip.Substring(0, 5);
                        //    var h_zip = dbEntity.ref_zip.Where(a => a.zip == z);
                        //    dx.home_city = h_zip.FirstOrDefault().city_name; //_home_city = n.ref_zip == null ? "" :  n.ref_zip.city_name;
                        //    dx.home_state = h_zip.FirstOrDefault().city_state;  //_home_state = n.ref_zip == null ? "" : n.ref_zip.city_state;
                        //    break;

                        //case "education": dx.education = n.value; break;
                        //case "experience": dx.experience = n.value; break;

                        //case "language_spoken": dx.language_spoken = n.value; break;

                        //case "board_certification": dx.board_certification = n.value; break;

                        //case "specialty":
                        //case "specialty_id":
                        //    dx.specialty = n.value;
                        //    //spec = null;
                        //    string[] get_spec = n.value.Split(',');

                        //    foreach (var i in get_spec)
                        //    {

                        //        long spec_id = 0;
                        //        bool s1 = long.TryParse(i, out spec_id);

                        //        if (s1)
                        //        {
                        //            var ref_spec = dbEntity.ref_specialty.Find(spec_id);

                        //            //spec.Add(new doc_specialty
                        //            //{
                        //            //    id = ref_spec.id,
                        //            //    code = ref_spec.code == null? "" : ref_spec.code,
                        //            //    description = ref_spec.description ==null? "": ref_spec.description,
                        //            //    name = ref_spec.name==null?"": ref_spec.name,
                        //            //    actor = ref_spec.actor == null ? "" : ref_spec.actor
                        //            //});
                        //            spec1.Add(new doc_specialty2
                        //            {
                        //                id = ref_spec.id,
                        //                name = ref_spec.name,
                        //                code = ref_spec.code,
                        //                specialization = ref_spec.specialization,
                        //                provider_type = ref_spec.provider_type
                        //            });

                        //        }

                        //    }

                        //    break;

                        //case "hospital_affiliation": dx.hospital_affiliation = n.value; break;
                        //case "practice_npi": dx.practice_npi = n.value; break;

                        //case "practice_name": dx.practice_name = n.value; break;

                        //case "practice_type": dx.practice_type = n.value; break;

                        //case "dea": dx.dea = n.value; break;

                        //case "clinician_role": dx.clinician_role = n.value; break;

                        //case "scheduling_solution": dx.scheduling_solution = n.value; break;

                        //case "current_scheduling": dx.current_scheduling = n.value; break;

                        //case "practice_street": dx.practice_street = n.value; break;

                        ////case "practice_zip": // service zip codes
                        ////    // removed from ext

                        ////    break;
                        //case "practice_phone_primary":// primary phone number
                        //    dx.practice_phone_primary = n.value; break;

                        //case "practice_fax": dx.practice_fax = n.value; break;

                        //case "practice_phone_cs": // customer service number
                        //    dx.practice_phone_cs = n.value; break;

                        //case "practice_phone_office":// office phone
                        //    dx.practice_phone_office = n.value; break;

                        //case "practice_clinicians": // no of field clinicians
                        //    dx.practice_clinicians = n.value; break;

                        //case "practice_exams": // no of exams you can handle per week
                        //    dx.practice_exams = n.value; break;

                        //case "geographic_market": // geographic market
                        //    dx.geographic_market = n.value; break;

                        //case "practice_expansion": // future expansion plans, new market, 
                        //    dx.practice_expansion = n.value; break;

                        //case "practice_insurance":// insurance list you are in Network or will acccept
                        //    dx.practice_insurance = n.value; break;

                        //case "practice_tax_number":// federal tax id number
                        //    dx.practice_tax_number = n.value; break;

                        //case "primary_contact_name":
                        //    dx.primary_contact_name = n.value;
                        //    break;

                        //case "primary_contact_phone":
                        //    dx.primary_contact_phone = n.value;
                        //    break;

                        //case "primary_contact_email":
                        //    dx.primary_contact_email = n.value;
                        //    break;

                        //case "operational_contact_name":
                        //    dx.operational_contact_name = n.value;
                        //    break;

                        //case "operational_contact_phone": dx.operational_contact_phone = n.value; break;

                        //case "operational_contact_email": dx.operational_contact_email = n.value; break;

                        //case "financial_contact_name": dx.financial_contact_name = n.value; break;

                        //case "financial_contact_phone": dx.financial_contact_phone = n.value; break;

                        //case "financial_contact_email": dx.financial_contact_email = n.value; break;

                        //case "practice_emr":  // emr software that you are currently using
                        //    dx.practice_emr = n.value; break;

                        //case "network_insurance":// in-network insurances
                        //    dx.network_insurance = n.value; break;

                        //// public string practice_contact { get; set; } // primary contact/ operational contact/ financial contact
                        //case "billing_bankname": dx.billing_bankname = n.value; break;
                        //case "billing_account": dx.billing_account = n.value; break;
                        //case "billing_routing": dx.billing_routing = n.value; break;
                        #endregion

                }


            }


            return dx;

        }

      

        public static List<doc_language> _getLanguage(long doctor_id)
        {
            SV_db1Entities dbEntity = new SV_db1Entities();
            List<doc_language> doc_lang = new List<doc_language>();
            var con_lang = dbEntity.con_DOCTOR_ref_language.Where(a => a.rel_DOCTOR_id == doctor_id);

            //string[] split_val = value.Split(',');
            foreach (var a in con_lang)
            {
            
                    var ref_lang = dbEntity.ref_languages.Find(a.rel_ref_language_id);
                    doc_lang.Add(new doc_language
                    {
                        id = ref_lang.id,
                        name = ref_lang.dname
                    });
              
                    //var ref_lang = dbEntity.ref_languages.Where(b => b.name.ToLower() == a.ToLower());
                    //if (ref_lang.Count() > 0)
                    //{
                    //    doc_lang.Add(new doc_language
                    //    {
                    //        name = ref_lang.FirstOrDefault().name,
                    //        id = ref_lang.FirstOrDefault().id
                    //    });
                    //}
                //}

            }


            return doc_lang;

        }

        

        // TODO: obsolete function
        private List<doc_language> _getLanguage2(string value)
        {
            List<doc_language> doc_lang = new List<doc_language>();

            string[] split_val = value.Split(',');
            foreach (var a in split_val)
            {
                int id = 0;
                bool isId = int.TryParse(a, out id);

                if (isId)
                {
                    var ref_lang = dbEntity.ref_languages.Find(id);
                    doc_lang.Add(new doc_language
                    {
                        name = ref_lang.dname,
                        id = ref_lang.id
                    });
                }
                else
                {
                    var ref_lang = dbEntity.ref_languages.Where(b => b.dname.ToLower() == a.ToLower());
                    if (ref_lang.Count() > 0)
                    {
                        doc_lang.Add(new doc_language
                        {
                            name = ref_lang.FirstOrDefault().dname,
                            id = ref_lang.FirstOrDefault().id
                        });
                    }
               }
             
            }

    
            return doc_lang;

        }
        public static  getDoctor_rating _get_averagerating(long doctor_id)
        {
            long user_id = 0;
            SV_db1Entities dbEntity = new SV_db1Entities();

            // obtain: doctor_id, doctor_review
            var ap_ext = dbEntity.APPOINTMENT_ext.Where(a => a.attr_name == "doctor_id"  );

            // =====================
            bool faveList = false;
            if (user_id > 0)
            {
                var fave_doctor = dbEntity.con_USER_favorite_DOCTOR.Where(a => a.rel_doctor_id == doctor_id && a.create_by__USER_id == user_id);
                if (fave_doctor.Count() > 0)
                { faveList = true;  }
            }


            // start: get average doctor rating
            double ave_rating = 0;
            var appt_docid = dbEntity.APPOINTMENT_ext.Where(a => a.attr_name =="doctor_id" && a.value == doctor_id.ToString());

            // get doctor is in con_USER_favorite_DOCTOR
            if (user_id > 0) {
                appt_docid = appt_docid.Where(a => a.create_by__USER_id == user_id);
            }

            List<p_ratings_review> rev = new List<p_ratings_review>();
            if (appt_docid.Count() > 0)
            {
                int cnt = 0; double doctor_rate = 0;
                foreach (var n in appt_docid)
                {
                    var appt_docrate = dbEntity.APPOINTMENT_ext.Where(a => (a.attr_name =="doctor_rating" || a.attr_name == "doctor_review") && a.rel_APPOINTMENT_id ==n.rel_APPOINTMENT_id );

                    // get doctor is i con_USER_favorite_DOCTOR
                    if (faveList)
                    {
                        appt_docrate = appt_docrate.Where(a => a.create_by__USER_id == user_id);
                    }
                  
                    string rev1 = ""; double dRate = 0;
                    if (appt_docrate.Count() > 0) {
                        int cnt1 = 0; int appt_cnt = appt_docrate.Count();
                        foreach (var d in appt_docrate) {
                            cnt1++;
                            switch (d.attr_name)
                            {
                                case "doctor_rating":
                                    cnt++;
                                    dRate = 0;
                                    bool temp = double.TryParse(d.value, out dRate);
                                    if (temp) doctor_rate += dRate;
                                    break;
                                case "doctor_review":
                                    rev1 = d.value;
                                    break;
                            }
                            if(cnt1 == appt_cnt)
                            rev.Add(new p_ratings_review { remark = rev1, rating = dRate });
                        }
                    }
                }

                if (cnt > 0)
                { ave_rating = doctor_rate / cnt; }
            }

            int fave_doc = 0;
            if (user_id > 0 && faveList)
            {
                var con_fav = dbEntity.con_USER_favorite_DOCTOR.Where(a => a.rel_doctor_id == doctor_id);
                if (con_fav.Count() > 0) {
                    if (con_fav.FirstOrDefault().favor == true)
                        fave_doc = 1;
                    else
                        fave_doc = 0;
                }
            }

            getDoctor_rating ave = new getDoctor_rating {
                average_rating = ave_rating,
                fave = fave_doc,
                review = rev
            };
            return ave;
        }

   

        public static List<doc_specialty_01112018> _getDoctor_specialty(long doctor_id)
        {
            SV_db1Entities dbEntity = new SV_db1Entities();
            //01/11/2018 List<doc_specialty2> spec = new List<doc_specialty2>();
            List<doc_specialty_01112018> spec = new List<doc_specialty_01112018>();
            var con_spec = dbEntity.con_DOCTOR_ref_specialty.Where(a => a.rel_DOCTOR_id == doctor_id);

            foreach (var i in con_spec)
            {

                //long spec_id = 0;
                //bool s = long.TryParse(i, out spec_id);
                //if (s)
                //{
                    var s = dbEntity.ref_specialty1.Find(i.rel_ref_specialty_id);
                    //if (get_spec != null) {

                if(s != null) {
          
                    // string[] con = s.conditions.Split('|');
                    List<string> condition = new List<string>();
                    foreach (var c in s.ref_condition)
                    { condition.Add(c.dname); }

                    spec.Add(new doc_specialty_01112018
                    {
                        // id = n.ref_specialty.id,
                        // description = n.ref_specialty.description,
                        // name = n.ref_specialty.name,
                        // actor = n.ref_specialty.actor == null ? "" : n.ref_specialty.actor

                        id = s.id,
                        provider_type = s.ref_specialty_provider.name,//  s.provider_type,

                        classification_code = s.level2_classification_code,
                        classification = s.level2_classification,

                        specialization_code = s.level2_classification_code == null ? "" : s.level2_classification_code,
                        specialization = s.level3_specialization == null ? "" : s.level3_specialization, // s.specialization,
                        conditions = condition
                    });
                }
                //}

                //}

            }

            return spec;
        }



        public List<doc_specialty2> _getDoctor_specialty_doc1(dynamic con_spec)
        {
            try {
                SV_db1Entities dbEntity = new SV_db1Entities();
                List<doc_specialty2> spec = new List<doc_specialty2>();
                //var con_spec = dbEntity.con_DOCTOR1_ref_specialty.Where(a => a.rel_DOCTOR_id == doctor_id);

                foreach (var i in con_spec)
                {

           
                    List<string> con = new List<string>();

                    if (i.spec_condition != null) {
                        string[] cond = i.spec_condition.Split('|');

                        foreach (var c in cond)
                        {
                            con.Add(c.Trim());
                        }
                    }
                  

                    spec.Add(new doc_specialty2
                    {
                        id = i.spec_id, // get_spec.id,
                        name = i.spec_name, // get_spec == null ? "" : get_spec.name,
                        code = i.spec_code, // get_spec == null ? "" : get_spec.code,
                        provider_type = i.spec_provider_type, // get_spec == null ? "" : get_spec.provider_type,
                        specialization = i.spec_specialization,  // get_spec == null ? "" : get_spec.specialization
                        conditions = con
                    });


           
                }

                return spec;
            }
            catch (Exception ex) {
                return null;
            }
           
        }

       

        private List<doc_specialty2> _getDoctor_specialty2(string value)
        { 
            List<doc_specialty2> spec = new List<doc_specialty2>();
            string[] sp = value.Split(',');
            foreach (var i in sp)
            {

                long spec_id = 0;
                bool s = long.TryParse(i, out spec_id);
                if (s)
                {
                    var get_spec = dbEntity.ref_specialty.Find(spec_id);
                    //if (get_spec != null) {

                    spec.Add(new doc_specialty2
                    {
                        id = spec_id,
                        name = get_spec == null ? "" : get_spec.name,
                        code = get_spec == null ? "" : get_spec.code,
                        provider_type = get_spec == null ? "" : get_spec.provider_type,
                        specialization = get_spec == null ? "" : get_spec.specialization
                    });
                    //}

                }

            }

            return spec;
        }

      
    }

    public static class ref_zip2
    {
        // private static IQueryable<ref_zip> _zip_code = new IQueryable<ref_zip>();
        public static IQueryable<ref_zip> ref_zip1
        {
            get {
                SV_db1Entities db = new SV_db1Entities();
                IQueryable<ref_zip> d =  from a in db.ref_zip select a;
                return d;
            }
           
        }
    }

    public class doct1
    {
        public long id { get; set; }
        public string npi { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public string dob { get; set; }
    }

    public class spec1
    {
        public long id { get; set; }
        public string specialization { get; set; }
        public string classification { get; set; }
        //public string middle_name { get; set; }
        //public string last_name { get; set; }
        //public string dob { get; set; }
    }
}