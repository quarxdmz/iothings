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
    
    public partial class ref_specialties
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ref_specialties()
        {
            this.Doc_Specialties_Profile = new HashSet<Doc_Specialties_Profile>();
        }
    
        public long id { get; set; }
        public string uid { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public string actor { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Doc_Specialties_Profile> Doc_Specialties_Profile { get; set; }
    }
}
