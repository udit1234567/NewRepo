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
    
    public partial class tblQuoteDrawDownDetail
    {
        public int QuoteDrawDownDetailsId { get; set; }
        public Nullable<int> LifeQQID { get; set; }
        public Nullable<int> PaymentFequency { get; set; }
        public Nullable<decimal> DrawDownDiv4 { get; set; }
        public Nullable<decimal> DrawDownDiv8 { get; set; }
        public Nullable<decimal> DrawDownDiv12 { get; set; }
    
        public virtual tblLifeQQ tblLifeQQ { get; set; }
    }
}
