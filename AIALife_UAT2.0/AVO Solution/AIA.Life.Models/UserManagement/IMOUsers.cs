using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.UserManagement
{
  public  class IMOUsers
    {
        
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string AgentCode { get; set; }
        public string Channel { get; set; }
        public string NIC { get; set; }
        public string UserRole { get; set; }
        public string Branch { get; set; }
        public string MobNo { get; set; }
        public string Email { get; set; }
        public bool UserStatus { get; set; }
        public string TransactionNo { get; set; }   
        public string CreatedBy { get; set; }
        public string StatusCode { get; set; }
        public List<string> ErrorMessage { get; set; }
        public string Gender { get; set; }
        public string Designation { get; set; }
        public string ReportingManager { get; set; }
        public string LicenseNo { get; set; }
        public string ServiceTraceID { get; set; }        
        public string Message { get; set; }
        public int? AuthLimit { get; set; }
        public string LockoutEnable { get; set; }
    }
}
