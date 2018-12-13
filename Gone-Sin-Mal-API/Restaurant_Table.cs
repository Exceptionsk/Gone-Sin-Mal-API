//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gone_Sin_Mal_API
{
    using System;
    using System.Collections.Generic;
    
    public partial class Restaurant_Table
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Restaurant_Table()
        {
            this.Favorite_Table = new HashSet<Favorite_Table>();
            this.Promotion_Table = new HashSet<Promotion_Table>();
        }
    
        public long Rest_id { get; set; }
        public Nullable<long> User_id { get; set; }
        public string Rest_name { get; set; }
        public byte[] Rest_profile_picture { get; set; }
        public string Rest_password { get; set; }
        public byte[] Rest_gallery_1 { get; set; }
        public byte[] Rest_gallery_2 { get; set; }
        public byte[] Rest_gallery_3 { get; set; }
        public byte[] Rest_gallery_4 { get; set; }
        public string Rest_category { get; set; }
        public Nullable<long> Rest_coin { get; set; }
        public Nullable<long> Rest_special_coin { get; set; }
        public string Rest_email { get; set; }
        public string Rest_phno { get; set; }
        public string Rest_township { get; set; }
        public string Rest_location { get; set; }
        public string Rest_lat { get; set; }
        public string Rest_long { get; set; }
        public Nullable<System.DateTime> Rest_created_date { get; set; }
        public Nullable<long> Rest_coin_purchased { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Favorite_Table> Favorite_Table { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Promotion_Table> Promotion_Table { get; set; }
        public virtual User_Table User_Table { get; set; }
    }
}
