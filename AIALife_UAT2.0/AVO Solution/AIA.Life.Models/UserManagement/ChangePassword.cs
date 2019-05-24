using AIA.Life.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace AIA.Life.Models.UserManagement
{
   public class ChangePassword
    {
        public string userName { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
        public string confirmNewPassword { get; set; }
        public string emailID { get; set; }
        public string mobileno { get; set; }
        public bool IsSecurityChecked { get; set; }
        public string NICNumber { get; set; }
        //[XmlIgnore]
        //public IEnumerable<SelectListItem> hintQuetn { get; set; }
        public string hintQuetn { get; set; }
        public List<MasterListItem> secretQuestions { get; set; }
        public string hintAns { get; set; }
        public string PasswordQuestion1 { get; set; }
        public string PasswordQuestion2 { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public bool OTPOrAnswer { get; set; }
        public bool NICAnswer { get; set; }
        public string OTP { get; set; }
        public bool IsCaptchaError { get; set; }
        public string Result { get; set; }
        public bool IsStatus { get; set; }
    }
}
