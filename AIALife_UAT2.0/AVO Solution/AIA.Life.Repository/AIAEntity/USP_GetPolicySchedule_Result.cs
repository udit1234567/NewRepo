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
    
    public partial class USP_GetPolicySchedule_Result
    {
        public string PolicyNumber { get; set; }
        public Nullable<System.DateTime> PolicyStartDate { get; set; }
        public string PolicyType { get; set; }
        public string InsuredName { get; set; }
        public string InsuredAddress { get; set; }
        public Nullable<short> ProposedPeriodofInsurance { get; set; }
        public Nullable<decimal> PremiumPayable { get; set; }
    }
}
