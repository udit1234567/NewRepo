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
    
    public partial class tblPolicyImdPayment
    {
        public decimal PaymentID { get; set; }
        public Nullable<decimal> PolicyID { get; set; }
        public Nullable<decimal> TotalPremium { get; set; }
        public Nullable<decimal> AgentPayment { get; set; }
        public Nullable<decimal> CustomerPayment { get; set; }
        public string Replenishmenttype { get; set; }
        public string ReplenishmentStatus { get; set; }
        public string Remarks { get; set; }
        public string CDTxnID { get; set; }
        public Nullable<System.DateTime> PolicyCreationDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<decimal> ReplenishAmount { get; set; }
    }
}
