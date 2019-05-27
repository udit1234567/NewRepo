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
    
    public partial class tblProspectReference
    {
        public decimal ReferenceID { get; set; }
        public Nullable<decimal> ProspectID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string EmployeeCode { get; set; }
        public Nullable<decimal> AddressID { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string EmployeeReference2Name { get; set; }
        public string EmployeeReference2Code { get; set; }
        public string EmployeeReference2Company { get; set; }
        public string EmployeeReference2Designation { get; set; }
        public string EmployeeReference2Relationship { get; set; }
        public string NonEmployeenameRef1_name { get; set; }
        public string NonEmployeenameRef1_Occupation { get; set; }
        public string NonEmployeenameRef2_name { get; set; }
        public string NonEmployeenameRef2_Occupation { get; set; }
        public Nullable<decimal> NonEmployeenameRef1_MobileNo { get; set; }
        public Nullable<decimal> NonEmployeenameRef2_MobileNo { get; set; }
        public Nullable<bool> IsChkFirstEmployeeRef { get; set; }
        public Nullable<bool> IsChkSecondEmployeeRef { get; set; }
        public string NonrefFirstSpecifyProfession { get; set; }
        public string NonrefSecondSpecifyProfession { get; set; }
        public Nullable<decimal> AddressIDRef2 { get; set; }
        public string EmployeeReference1Relationship { get; set; }
        public string EmployeeReference1Company { get; set; }
    
        public virtual tblAddress tblAddress { get; set; }
        public virtual tblAddress tblAddress1 { get; set; }
        public virtual tblProspect tblProspect { get; set; }
    }
}