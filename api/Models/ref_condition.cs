
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
    
public partial class ref_condition
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ref_condition()
    {

        this.con_ref_specialty_ref_condition = new HashSet<con_ref_specialty_ref_condition>();

    }


    public long id { get; set; }

    public Nullable<long> rel_ref_specialty_id { get; set; }

    public string dname { get; set; }

    public Nullable<System.DateTime> dt_create { get; set; }

    public long create_by__USER_id { get; set; }

    public Nullable<System.DateTime> dt_update { get; set; }

    public Nullable<long> update_by__USER_id { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<con_ref_specialty_ref_condition> con_ref_specialty_ref_condition { get; set; }

    public virtual ref_specialty1 ref_specialty1 { get; set; }

}

}
