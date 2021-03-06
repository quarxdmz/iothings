
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
    
public partial class USER
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public USER()
    {

        this.notifications = new HashSet<notification>();

        this.User_Card_Info = new HashSet<User_Card_Info>();

        this.USER_ext = new HashSet<USER_ext>();

    }


    public long id { get; set; }

    public string name_first { get; set; }

    public string name_last { get; set; }

    public string username { get; set; }

    public string phash { get; set; }

    public string tlas { get; set; }

    public string social_id { get; set; }

    public string social_type { get; set; }

    public string image_url { get; set; }

    public string verification_code { get; set; }

    public Nullable<bool> verified { get; set; }

    public string platform { get; set; }

    public string device_token { get; set; }

    public Nullable<bool> HIPAA_authorization { get; set; }

    public Nullable<bool> terms_condition { get; set; }

    public System.DateTime dt_create { get; set; }

    public Nullable<System.DateTime> dt_update { get; set; }

    public long create_by__USER_id { get; set; }

    public Nullable<long> update_by__USER_id { get; set; }

    public Nullable<System.DateTime> dt_lastin { get; set; }

    public string url_lastin { get; set; }

    public long rel_ref_status_id { get; set; }

    public long rel_ref_USER_type_id { get; set; }

    public string token_current { get; set; }

    public Nullable<long> rel_USER_type_access_id { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<notification> notifications { get; set; }

    public virtual ref_USER_type ref_USER_type { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<User_Card_Info> User_Card_Info { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<USER_ext> USER_ext { get; set; }

}

}
