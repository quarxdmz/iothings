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
    
    public partial class PRODUCT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PRODUCT()
        {
            this.PRODUCT_ext = new HashSet<PRODUCT_ext>();
        }
    
        public long id { get; set; }
        public Nullable<long> create_by__USER_id { get; set; }
        public System.DateTime dt_create { get; set; }
        public Nullable<long> update_by__USER_id { get; set; }
        public Nullable<System.DateTime> dt_update { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRODUCT_ext> PRODUCT_ext { get; set; }
    }
}
