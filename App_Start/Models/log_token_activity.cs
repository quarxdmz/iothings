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
    
    public partial class log_token_activity
    {
        public string token { get; set; }
        public System.DateTime dt_create { get; set; }
        public string action_type { get; set; }
        public string trigger { get; set; }
        public string var_from { get; set; }
        public string var_to { get; set; }
        public long create_by__USER_id { get; set; }
    }
}
