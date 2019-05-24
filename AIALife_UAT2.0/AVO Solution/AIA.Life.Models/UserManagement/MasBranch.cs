using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.UserManagement
{
   public class MasBranch
    {
        public int Branch_ID_PK { get; set; }
        public int Branch_Region_ID_FK { get; set; }
        public int? Parent_Branch_ID_FK { get; set; }
        public int? Functional_Parent_Branch_ID_FK { get; set; }
        public int System_Branch_ID { get; set; }
        public string Region_Code { get; set; }
        public string Zone_Code { get; set; }
        public string Branch_Type_Name { get; set; }
        public string New_Branch_Code { get; set; }
        public string Branch_Code { get; set; }
        public string Branch_Name { get; set; }
        public string Address { get; set; }
        public string City_or_Village { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }
        public string Pickup_Location { get; set; }
        public string Pickup_Point { get; set; }
        public int? Bank_ACC_ID_FK { get; set; }
        public string Business_Area { get; set; }
        public string StdCode { get; set; }
        public string Phone { get; set; }
        public string Fax_Std_Code { get; set; }
        public string Fax { get; set; }
        public string Profit_Center { get; set; }
        public DateTime Created_Date { get; set; }
        public string Created_By { get; set; }
        public DateTime? Updated_Date { get; set; }
        public string Updated_By { get; set; }
        public DateTime? Deleted_Date { get; set; }
        public string Deleted_By { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public bool Branch_ARC { get; set; }
    }
}
