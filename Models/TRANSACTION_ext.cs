
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
    
public partial class TRANSACTION_ext
{

    public long id { get; set; }

    public Nullable<long> rel_TRANSACTION_id { get; set; }

    public string attr_name { get; set; }

    public string dname { get; set; }

    public string value { get; set; }

    public Nullable<long> rel_ref_datatype_id { get; set; }

    public Nullable<long> create_by__USER_id { get; set; }

    public Nullable<System.DateTime> dt_create { get; set; }

    public Nullable<long> update_by__USER_id { get; set; }

    public Nullable<System.DateTime> dt_update { get; set; }



    public virtual TRANSACTION TRANSACTION { get; set; }

}

}
