//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CFMMCD.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class INVUOMP0
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public INVUOMP0()
        {
            this.INVRIMP0 = new HashSet<INVRIMP0>();
            this.INVRIMP01 = new HashSet<INVRIMP0>();
            this.INVRIMP02 = new HashSet<INVRIMP0>();
        }
    
        public string UOMDES { get; set; }
        public string UOMDEL { get; set; }
        public Nullable<System.DateTime> UOMDAT { get; set; }
        public string UOMUSR { get; set; }
        public string STATUS { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INVRIMP0> INVRIMP0 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INVRIMP0> INVRIMP01 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INVRIMP0> INVRIMP02 { get; set; }
    }
}
