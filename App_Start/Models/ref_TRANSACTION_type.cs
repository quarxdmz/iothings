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
    
    public partial class ref_TRANSACTION_type
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ref_TRANSACTION_type()
        {
            this.TRANSACTIONs = new HashSet<TRANSACTION>();
        }
    
        public long id { get; set; }
        public string dname { get; set; }
        public Nullable<long> create_by__USER_id { get; set; }
        public Nullable<System.DateTime> dt_create { get; set; }
        public Nullable<long> update_by__USER_id { get; set; }
        public Nullable<System.DateTime> dt_update { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRANSACTION> TRANSACTIONs { get; set; }
    }
}
