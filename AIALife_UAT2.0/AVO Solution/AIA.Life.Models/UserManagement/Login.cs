using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.UserManagement
{
   public class Login
    {
        public Login()
        {
            lstPermission = new List<Permission>();
        }
        public String UserId { get; set; }
        public string Password { get; set; }
        public string DeviceID { get; set; }
        public List<Permission> lstPermission { get; set; }
        public List<string> listIntroducer { get; set; }
        public String Message { get; set; }        
        public string UserName { get; set; }   
        public string ServiceTraceID { get; set; }
        public string APKVersion { get; set; }
        public string DeviceModelName { get; set; }
        public string DeviceOS { get; set; }
        public string DeviceToken { get; set; }
        public string Role { get; set; }
    }
    public class DeviceInfo
    {

    }
    public class Permission
    {
        public Nullable<int> PermissionID { get; set; }
        
      
    }
    public class UserToken
    {
        public String UserId { get; set; }
        public String Message { get; set; }
        public string ServiceTraceID { get; set; }
        public string UserName { get; set; }
        public string ValidUser { get; set; }
        public string Password { get; set; }
    }
}
