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
    
    public partial class con_DOCTOR_NOTE
    {
        public long id { get; set; }
        public Nullable<long> rel_DOCTOR_id { get; set; }
        public Nullable<long> rel_NOTE_id { get; set; }
        public System.DateTime dt_create { get; set; }
        public Nullable<System.DateTime> dt_update { get; set; }
        public long create_by__USER_id { get; set; }
        public Nullable<long> update_by__USER_id { get; set; }
    
        public virtual NOTE NOTE { get; set; }
    }
}
