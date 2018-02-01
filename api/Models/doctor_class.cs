using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Models
{
    public class doctor_class
    {
    }

    public class post_webdoctorAccount
    {
        // created: 01/10/2018
       public string first_name { get; set; }
       public string last_name { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public bool hipaa_authorization { get; set; }
        public bool terms_conditions { get; set; }

    }
    public class post_webdoctorlogin
    {
        // date create: 1/3/2018
        public string email { get; set; }
        public string password { get; set; }
    }

    public class get_webdoctorsingup {
        public string npi { get; set; }
    }

    public class post_webdoctorsignup
    {
        // date create: 1/3/2018
        //  NPI no
        //  Password
        //  Image
        //  Practice name
        //  Phone no
        //  List of specialities
        //  Address
        //  Bio
        //  List of Degrees
        //  List of languages
        //  List of insurances
        //  Practising year
        public string npi { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public string title { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string dea { get; set; }
        public string dob { get; set; }
        public string image_url { get; set; }
        public string bio { get; set; }
        public string gender { get; set; }
        public string clinician_role { get; set; }
        public string exam_encounter { get; set; }

        public string education { get; set; }
        public string experience { get; set; }
        public string language_id { get; set; }
        public string specialty_id { get; set; }
        public string insurance_id { get; set; }

        public string home_address1 { get; set; }
        public string home_address2 { get; set; }
        public string home_city { get; set; }
        public string home_state { get; set; }
        public string home_zip { get; set; }

        public string practice_address { get; set; }
        public string practice_address2 { get; set; }
        public string practice_city { get; set; }
        public string practice_state { get; set; }
        public string practice_zip { get; set; }

        public string personal_practice_type { get; set; }
        public string practice_name { get; set; }
        
        public string practice_fax { get; set; }
        public string practice_phone { get; set; }
        // public List<long> specialty_id { get; set; }
       
    }


    public class getDoctor_rating
    {
        public double average_rating { get; set; }
        public int fave { get; set; }
        public List<p_ratings_review> review { get; set; }
        //public double rating { get; set; }
        //public string remark { get; set; }
    }

    public class doc_language
    {
        public long id { get; set; }
        public string name { get; set; }
    }

    public class insurance
    {
        //public long id { get; set; }
        public long id { get; set; }
        public string provider { get; set; }
    }
}