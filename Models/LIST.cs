
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
    
public partial class LIST
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public LIST()
    {

        this.con_SOUL_LIST = new HashSet<con_SOUL_LIST>();

        this.LIST_ext = new HashSet<LIST_ext>();

    }


    public long id { get; set; }

    public Nullable<long> create_by__USER_id { get; set; }

    public System.DateTime dt_create { get; set; }

    public Nullable<long> update_by__USER_id { get; set; }

    public Nullable<System.DateTime> dt_update { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<con_SOUL_LIST> con_SOUL_LIST { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<LIST_ext> LIST_ext { get; set; }

}

}
