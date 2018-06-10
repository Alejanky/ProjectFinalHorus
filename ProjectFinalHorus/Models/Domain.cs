//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectFinalHorus.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Domain
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Domain()
        {
            this.Clients_Packages = new HashSet<Clients_Packages>();
            this.Packages = new HashSet<Package>();
            this.Staff_Domains = new HashSet<Staff_Domains>();
        }
    
        public int id { get; set; }
        public int id_user { get; set; }
        public string name { get; set; }
        public byte[] thumbnail { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string description { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Clients_Packages> Clients_Packages { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Package> Packages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Staff_Domains> Staff_Domains { get; set; }
    }
}
