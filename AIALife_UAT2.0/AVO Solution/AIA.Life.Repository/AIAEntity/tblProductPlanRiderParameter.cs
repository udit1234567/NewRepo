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
    
    public partial class tblProductPlanRiderParameter
    {
        public int PlanRiderParameterId { get; set; }
        public Nullable<int> ProductPlanRiderId { get; set; }
        public Nullable<int> ParameterId { get; set; }
        public Nullable<decimal> NumericValueFrom { get; set; }
        public Nullable<decimal> NumericValueTo { get; set; }
        public string StringValueFrom { get; set; }
        public string ListValue { get; set; }
        public Nullable<int> RelationId { get; set; }
        public string ApplyOn { get; set; }
        public string MinRateType { get; set; }
        public string MaxRateType { get; set; }
        public string MinVal { get; set; }
        public string MaxVal { get; set; }
        public string StringValueTo { get; set; }
        public string ApplyOnTo { get; set; }
    
        public virtual tblMasParameter tblMasParameter { get; set; }
        public virtual tblProductPlanRider tblProductPlanRider { get; set; }
    }
}