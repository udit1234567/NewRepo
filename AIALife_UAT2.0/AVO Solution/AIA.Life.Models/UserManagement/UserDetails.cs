using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.UserManagement
{
    public class UserDetails
    {
        public decimal NodeID { get; set; }
        //public decimal NodeID { get; set; }
        //public Guid? UserID { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        //public int? UserAccountID { get; set; }
        public Nullable<int> UserAccountID { get; set; }
        public int? LOBID { get; set; }
        public string UserName { get; set; }
        public int? NodeTypeId { get; set; }
        //public int? UserParentId { get; set; }
        public Nullable<int> UserParentId { get; set; }
        //public bool? Status { get; set; }
        public Nullable<bool> Status { get; set; }
       // public Guid? CreatedBy { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
       // public DateTime? CreatedDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public int? AgentID { get; set; }
        public int? BankBranchID { get; set; }
        public bool? locked { get; set; }
        public string LockedReason { get; set; }
        public DateTime? lockStartDate { get; set; }
        public DateTime? lockEndDate { get; set; }
        public bool? LockMechanism { get; set; }
        public int? BranchID { get; set; }
        public string Title { get; set; }
        //public string Title { get; set; }
        public string FirstName { get; set; }
        //public string FirstName { get; set; }
        public string LastName { get; set; }
       // public string LastName { get; set; }
        //public DateTime? DOB { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        //public int? Gender { get; set; }
        public Nullable<int> Gender { get; set; }
        //public string Email { get; set; }
        public string Email { get; set; }
        public string OfficeTel { get; set; }
        public string ResTel { get; set; }
        public string Mobile { get; set; }
        public string PAN { get; set; }
        //public string PAN { get; set; }
        //public string UserLevel { get; set; }
        public string userlevel { get; set; }
        public string MiddleName { get; set; }
        public string PaymentType { get; set; }
        public string AutorityCode { get; set; }
        public string CorporateID { get; set; }
       // public int AddressID { get; set; }
        public Nullable<decimal> AddressID { get; set; }
        public string EmployeeId { get; set; }
       // public string UserType { get; set; }
        public string UserType { get; set; }
        public bool isactivated { get; set; }
        public string LoginID { get; set; }
        public string UserCode { get; set; }
        public string IMDCode { get; set; }
        public Nullable<bool> Organisation { get; set; }
        public string CorpName { get; set; }
        public Nullable<System.DateTime> DOR { get; set; }
        public string ContactNo { get; set; }
        public string ContactNo2 { get; set; }
        public string IRDALicenseNo { get; set; }
        public string IMDName { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> PasswordQuestionID { get; set; }
        public string PasswordAnswer { get; set; }

    }
}


    
  
