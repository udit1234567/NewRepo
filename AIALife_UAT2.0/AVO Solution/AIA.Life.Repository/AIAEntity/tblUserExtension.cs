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
    
    public partial class tblUserExtension
    {
        public decimal UserExtensionID { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public Nullable<bool> PaymentType { get; set; }
        public Nullable<bool> FloatType { get; set; }
        public Nullable<decimal> FloatNo { get; set; }
        public Nullable<decimal> FloatLimit { get; set; }
        public Nullable<int> LimitDays { get; set; }
        public Nullable<decimal> AvailableFloat { get; set; }
        public Nullable<decimal> AdvDepNo { get; set; }
        public Nullable<decimal> AvailableAdvDeposit { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsMDRT { get; set; }
    }
}
