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
    
    public partial class Doc_Education
    {
        public long id { get; set; }
        public Nullable<long> doc_id { get; set; }
        public string school { get; set; }
        public string graduation_year { get; set; }
        public string degree { get; set; }
    
        public virtual DOCTOR DOCTOR { get; set; }
    }
}
