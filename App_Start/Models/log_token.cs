//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace api.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class log_token
    {
        public long id { get; set; }
        public string token { get; set; }
        public System.DateTime dt_create { get; set; }
        public long rel_ref_status_id { get; set; }
        public System.DateTime dt_lastactivity { get; set; }
        public string create_by__USER_id { get; set; }
    }
}
