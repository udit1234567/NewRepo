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
    
    public partial class tblPolicyNomineeDetail
    {
        public decimal NomineeID { get; set; }
        public Nullable<decimal> PolicyID { get; set; }
        public string Salutation { get; set; }
        public string NomineeName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<int> Age { get; set; }
        public Nullable<int> NomineeRelation { get; set; }
        public string OtherRelation { get; set; }
        public string AppointeeName { get; set; }
        public string NICNo { get; set; }
        public string NomineeShare { get; set; }
        public Nullable<System.DateTime> AppointeeDOB { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string NomineeSurName { get; set; }
        public string NomineeIntialName { get; set; }
        public string NomineeGender { get; set; }
        public string NomineeMartialStatus { get; set; }
        public string NomineeAddress { get; set; }
        public string NomineeMobileNo { get; set; }
        public string ClientCode { get; set; }
    
        public virtual tblPolicy tblPolicy { get; set; }
    }
}
