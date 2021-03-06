
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
    
public partial class NOTE
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public NOTE()
    {

        this.con_APPOINTMENT_NOTE = new HashSet<con_APPOINTMENT_NOTE>();

        this.con_DOCTOR_NOTE = new HashSet<con_DOCTOR_NOTE>();

        this.con_ORDER_NOTE = new HashSet<con_ORDER_NOTE>();

        this.con_TRANSACTION_NOTE = new HashSet<con_TRANSACTION_NOTE>();

    }


    public long id { get; set; }

    public string value { get; set; }

    public long rel_ref_NOTE_type_id { get; set; }

    public Nullable<long> create_by__USER_id { get; set; }

    public System.DateTime dt_create { get; set; }

    public Nullable<long> update_by__USER_id { get; set; }

    public Nullable<System.DateTime> dt_update { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<con_APPOINTMENT_NOTE> con_APPOINTMENT_NOTE { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<con_DOCTOR_NOTE> con_DOCTOR_NOTE { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<con_ORDER_NOTE> con_ORDER_NOTE { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<con_TRANSACTION_NOTE> con_TRANSACTION_NOTE { get; set; }

    public virtual ref_NOTE_type ref_NOTE_type { get; set; }

}

}
