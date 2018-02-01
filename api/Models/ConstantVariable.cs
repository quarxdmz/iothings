using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Models
{
    public static class ConstantVariable
    {
        static SV_db1Entities db = new SV_db1Entities();

        //static long doc_type = db.ref_USER_type.Where(a => a.dname.ToLower() == "doctor").FirstOrDefault().id;
        //public const long user_type = doc_type;
    }
}