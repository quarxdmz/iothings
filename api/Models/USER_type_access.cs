
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
    
public partial class USER_type_access
{

    public long id { get; set; }

    public string dname { get; set; }

    public long rel_ref_USER_type_id { get; set; }

    public string allowed { get; set; }

    public System.DateTime dt_create { get; set; }

    public Nullable<System.DateTime> dt_update { get; set; }

    public long create_by__USER_id { get; set; }

    public Nullable<long> update_by__USER_id { get; set; }

    public string url_home { get; set; }

    public string menu { get; set; }

}

}
