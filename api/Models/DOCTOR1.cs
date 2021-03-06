
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
    
public partial class DOCTOR1
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public DOCTOR1()
    {

        this.con_DOCTOR1_ref_insurance = new HashSet<con_DOCTOR1_ref_insurance>();

        this.con_DOCTOR1_ref_language = new HashSet<con_DOCTOR1_ref_language>();

        this.con_DOCTOR1_ref_specialty = new HashSet<con_DOCTOR1_ref_specialty>();

        this.DOCTOR_ext1 = new HashSet<DOCTOR_ext1>();

    }


    public long id { get; set; }

    public string NPI { get; set; }

    public string name_first { get; set; }

    public string name_last { get; set; }

    public string name_middle { get; set; }

    public string gender { get; set; }

    public string title { get; set; }

    public string practice_name { get; set; }

    public string organization_name { get; set; }

    public string email { get; set; }

    public string phone { get; set; }

    public string medicare_upin { get; set; }

    public string license_no { get; set; }

    public string pecos_certification { get; set; }

    public string fax_to { get; set; }

    public string home_phone { get; set; }

    public string home_fax { get; set; }

    public string home_addr_1 { get; set; }

    public string home_addr_2 { get; set; }

    public Nullable<long> home_addr_zip_id { get; set; }

    public string practice_phone { get; set; }

    public string practice_fax { get; set; }

    public string practice_addr_1 { get; set; }

    public string practice_addr_2 { get; set; }

    public Nullable<long> practice_addr_zip_id { get; set; }

    public string image_url { get; set; }

    public string bio { get; set; }

    public string dob { get; set; }

    public Nullable<bool> is_claim { get; set; }

    public long create_by__USER_id { get; set; }

    public Nullable<System.DateTime> dt_create { get; set; }

    public Nullable<long> update_by__USER_id { get; set; }

    public Nullable<System.DateTime> dt_update { get; set; }

    public Nullable<long> rel_ref_status_id { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<con_DOCTOR1_ref_insurance> con_DOCTOR1_ref_insurance { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<con_DOCTOR1_ref_language> con_DOCTOR1_ref_language { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<con_DOCTOR1_ref_specialty> con_DOCTOR1_ref_specialty { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<DOCTOR_ext1> DOCTOR_ext1 { get; set; }

    public virtual ref_zip ref_zip { get; set; }

}

}
