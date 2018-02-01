using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using api.Models;
using api.Controllers;

namespace api.Base
{
    public class UserType : Base.BaseController
    {
        // validate zip in MarketControler
        public long validateZip(post_market mar)
        {
            SV_db1Entities db = new SV_db1Entities();
            int nzip = 0;
            bool isvalid = int.TryParse(mar.zip, out nzip);
            if (!isvalid)
            {
                return 1;
            }

            //if (a == 1) { return Json(new { data = new string[] { }, message = "Invalid zip value.", success = false }); }
            //else if (a == 2) { return Json(new { data = new string[] { }, message = "Invalid city value.", success = false }); }
            //else if (a == 3) { return Json(new { data = new string[] { }, message = "Invalid state value.", success = false }); }

            var ref_zip = db.ref_zip.Where(a => a.zip == mar.zip);
            if (ref_zip.Count() > 0)
            {
                if (ref_zip.FirstOrDefault().city_name.ToLower() != mar.city.ToLower())
                {
                    //return Json(new { data = new string[] { }, message = "Invalid zip value.", success = false });
                    return 2;
                }

                if (ref_zip.FirstOrDefault().city_state.ToLower() != mar.state.ToLower()
                    & ref_zip.FirstOrDefault().city_state_long.ToLower() != mar.state.ToLower())
                {
                    //return Json(new { data = new string[] { }, message = "Invalid state name.", success = false });
                    return 3;
                }
            }
            mar.zip_id = ref_zip.FirstOrDefault().id;
            //return Json(new { data = new string[] { }, message = "Match found.", success = true });
            return 0;
        }

        // validate zip in PhysicianController
        public long validateZip(post_physician phy)
        {
            SV_db1Entities db = new SV_db1Entities();
            int nzip = 0;
            bool isvalid = int.TryParse(phy.zip, out nzip);
            if (!isvalid)
            {
                return 1;
            }

            var ref_zip = db.ref_zip.Where(a => a.zip == phy.zip);
            if (ref_zip.Count() > 0)
            {
                if (ref_zip.FirstOrDefault().city_name.ToLower() != phy.city.ToLower())
                {
                    //return Json(new { data = new string[] { }, message = "Invalid city name.", success = false });
                    return 2;
                }

                if (ref_zip.FirstOrDefault().city_state.ToLower() != phy.state.ToLower()
                    & ref_zip.FirstOrDefault().city_state_long.ToLower() != phy.state.ToLower())
                {
                    //return Json(new { data = new string[] { }, message = "Invalid state name.", success = false });
                    return 3;
                }
            }
            phy.zip_id = ref_zip.FirstOrDefault().id;
            //return Json(new { data = new string[] { }, message = "Match found.", success = true });
            return 0;
        }

        // validate zip in PharmacyController
        public long validateZip(post_pharmacy pha)
        {
            SV_db1Entities db = new SV_db1Entities();
            int nzip = 0;
            bool isvalid = int.TryParse(pha.zip, out nzip);
            if (!isvalid)
            {
                return 1;
            }

            var ref_zip = db.ref_zip.Where(a => a.zip == pha.zip);
            if (ref_zip.Count() > 0)
            {
                if (ref_zip.FirstOrDefault().city_name.ToLower() != pha.city.ToLower())
                {
                    //return Json(new { data = new string[] { }, message = "Invalid city name.", success = false });
                    return 2;
                }

                if (ref_zip.FirstOrDefault().city_state.ToLower() != pha.state.ToLower()
                    & ref_zip.FirstOrDefault().city_state_long.ToLower() != pha.state.ToLower())
                {
                    //return Json(new { data = new string[] { }, message = "Invalid state name.", success = false });
                    return 3;
                }
            }
            pha.zip_id = ref_zip.FirstOrDefault().id;
            //return Json(new { data = new string[] { }, message = "Match found.", success = true });
            return 0;
        }

        public long validateZip(post_supplier sup)
        {
            SV_db1Entities db = new SV_db1Entities();
            int nzip = 0;
            bool isvalid = int.TryParse(sup.zip, out nzip);
            if (!isvalid)
            {
                return 1;
            }

            var ref_zip = db.ref_zip.Where(a => a.zip == sup.zip);
            if (ref_zip.Count() > 0)
            {
                if (ref_zip.FirstOrDefault().city_name.ToLower() != sup.city.ToLower())
                {
                    //return Json(new { data = new string[] { }, message = "Invalid city name.", success = false });
                    return 2;
                }

                if (ref_zip.FirstOrDefault().city_state.ToLower() != sup.state.ToLower()
                    & ref_zip.FirstOrDefault().city_state_long.ToLower() != sup.state.ToLower())
                {
                    //return Json(new { data = new string[] { }, message = "Invalid state name.", success = false });
                    return 3;
                }
            }
            sup.zip_id = ref_zip.FirstOrDefault().id;
            //return Json(new { data = new string[] { }, message = "Match found.", success = true });
            return 0;
        }

    }
}
