//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AIA.Life.Repository.AIAEntity
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblProductRider
    {
        public int ProductRiderId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<int> RiderId { get; set; }
        public Nullable<System.DateTime> EffectiveFrom { get; set; }
        public Nullable<System.DateTime> EffectiveTo { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> RelationID { get; set; }
    
        public virtual tblRider tblRider { get; set; }
        public virtual tblProduct tblProduct { get; set; }
    }
}
