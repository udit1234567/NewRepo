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
    
    public partial class tblProductAssumptionMap
    {
        public int ProductAssumptionMapId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<int> ParameterId { get; set; }
        public Nullable<decimal> Frequency { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<int> PolicyTerm { get; set; }
        public Nullable<int> PremiumPayTerm { get; set; }
    }
}
