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
    
    public partial class tblDiscountLoadingChart
    {
        public int DiscountLoadingChartId { get; set; }
        public string ChartCode { get; set; }
        public Nullable<int> Frequency { get; set; }
        public Nullable<int> AgeFrom { get; set; }
        public Nullable<int> AgeTo { get; set; }
        public Nullable<decimal> SumAssureFrom { get; set; }
        public Nullable<decimal> SumAssuredTo { get; set; }
        public Nullable<int> LivesFrom { get; set; }
        public Nullable<int> LivesTo { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<bool> WithDeductible { get; set; }
        public Nullable<bool> WithFamilyFloater { get; set; }
    }
}
