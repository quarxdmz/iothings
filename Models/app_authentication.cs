
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
    
public partial class app_authentication
{

    public long id { get; set; }

    public string name { get; set; }

    public string password { get; set; }

    public Nullable<System.DateTime> dt_create { get; set; }

    public Nullable<long> create_by__USER_id { get; set; }

    public Nullable<System.DateTime> dt_update { get; set; }

    public Nullable<long> update_by__USER_id { get; set; }

}

}
