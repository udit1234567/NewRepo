using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.UserManagement
{
   public class MasEmployee
    {
        public int Employee_ID_PK { get; set; }
        public int? Reporting_Employee_ID_PK { get; set; }
        public int? System_SM_ID { get; set; }
        public string SM_Vertical { get; set; }
        public string SM_Channel { get; set; }
        public int? SM_Channel_ID_FK { get; set; }
        public DateTime Date_of_Birth { get; set; }
        public DateTime Date_of_Joining { get; set; }
        public DateTime? Date_of_Resigning { get; set; }
        public DateTime? System_Date_of_Resigining { get; set; }
        public string Employee_Designation { get; set; }
        public string Employee_Department { get; set; }
        public int Employee_Branch_ID_FK { get; set; }
        public int? Employee_Branch_Located_AT { get; set; }
        public bool Located_At { get; set; }
        public DateTime? LocatedAt_JoinDate { get; set; }
        public string Employee_HR_Function { get; set; }
        public string Employee_Function { get; set; }
        public string Employee_Name { get; set; }
        public string Employee_Code { get; set; }
        public string Fax { get; set; }
        public DateTime Created_Date { get; set; }
        public string Created_By { get; set; }
        public DateTime? Updated_Date { get; set; }
        public string Updated_By { get; set; }
        public DateTime? Deleted_Date { get; set; }
        public string Deleted_By { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public bool Employee_ARC { get; set; }
        public string Title { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Gendor { get; set; }
        public bool? Resgn_In_Progress { get; set; }
        public DateTime? Last_Working_Date { get; set; }
        public string Status { get; set; }
        public int? Direct_Rep_Auth { get; set; }
        public int? Functional_Rep_Auth { get; set; }
        public DateTime? Date_Of_Offer { get; set; }
        public string Job_Code { get; set; }
        public string CostCenter_Code { get; set; }
        public string OM_Code { get; set; }
        public string Grade { get; set; }
        public string Contact_No { get; set; }
        public string EMail { get; set; }
        public string Position_Code { get; set; }
        public bool? Spoke_Type { get; set; }
        public string Spoke_Branch { get; set; }
        public string RecruitmentType { get; set; }
        public DateTime? Employee_Transfer_Date { get; set; }
        public DateTime? Confirmation_Date { get; set; }
        public bool? Confirmation_Status { get; set; }
        public int? Employee_Function_ID_FK { get; set; }
        public int? Employee_HR_Role_ID_FK { get; set; }
    }
}
