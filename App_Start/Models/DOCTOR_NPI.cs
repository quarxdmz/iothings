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
    
    public partial class DOCTOR_NPI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DOCTOR_NPI()
        {
            this.DOCTOR_ext_NPI = new HashSet<DOCTOR_ext_NPI>();
        }
    
        public long id { get; set; }
        public string NPI { get; set; }
        public string organ_name { get; set; }
        public string name_first { get; set; }
        public string name_last { get; set; }
        public string name_middle { get; set; }
        public string gender { get; set; }
        public string title { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string medicare_upin { get; set; }
        public string license_no { get; set; }
        public Nullable<bool> pecos_cert { get; set; }
        public string fax_to { get; set; }
        public string addr_address1 { get; set; }
        public string addr_address2 { get; set; }
        public Nullable<long> addr_rel_ref_zip_id { get; set; }
        public string image_url { get; set; }
        public string bio { get; set; }
        public long create_by__USER_id { get; set; }
        public Nullable<System.DateTime> dt_create { get; set; }
        public Nullable<long> update_by__USER_id { get; set; }
        public Nullable<System.DateTime> dt_update { get; set; }
        public Nullable<long> rel_ref_status_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCTOR_ext_NPI> DOCTOR_ext_NPI { get; set; }
        public virtual ref_zip ref_zip { get; set; }
    }
}
