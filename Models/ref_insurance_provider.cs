
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
    
public partial class ref_insurance_provider
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ref_insurance_provider()
    {

        this.con_DOCTOR_ref_insurance = new HashSet<con_DOCTOR_ref_insurance>();

        this.con_DOCTOR1_ref_insurance = new HashSet<con_DOCTOR1_ref_insurance>();

        this.con_SOUL_ref_insurance = new HashSet<con_SOUL_ref_insurance>();

    }


    public long id { get; set; }

    public string PayerID { get; set; }

    public string PayerName { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<con_DOCTOR_ref_insurance> con_DOCTOR_ref_insurance { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<con_DOCTOR1_ref_insurance> con_DOCTOR1_ref_insurance { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<con_SOUL_ref_insurance> con_SOUL_ref_insurance { get; set; }

}

}
