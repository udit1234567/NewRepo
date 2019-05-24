using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AIA.Life.Models.UserManagement;
using AIA.Presentation.AVOLife.ExceptionHandling;
using AIA.Life.Repository.AIAEntity;
using AIA.Life.Models.Common;
using Newtonsoft.Json;
using System.IO;
using Grid.Mvc.Ajax.GridExtensions;
using System.Globalization;
using AIA.Life.Models.Payment;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using CaptchaMvc.HtmlHelpers;
using AIA.Presentation.AVOLife.Models;
using Microsoft.AspNet.Identity;
using AIA.CrossCutting;
using System.Security.Cryptography;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Text;
using reCAPTCHA.MVC;
using System.Net;
using AIA.Life.Business.UserManagement;
using System.DirectoryServices;
using log4net;

namespace AIA.Presentation.AVOLife.Controllers
{
    [Authorize]
    [ErrorLogging]
    [SessionTimeout]
    public class UserManagementController : Controller
    {
        AVOAIALifeEntities Entities = new AVOAIALifeEntities();
        CreateUserModel objUserModel = new CreateUserModel();
        Life.Models.UserManagement.Configuration objConfiguration = new Life.Models.UserManagement.Configuration();
        JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
        private ApplicationUserManager _userManager;
        private ApplicationUser _userManagers;
        private ApplicationRoleManager _roleManager;
        string userName;
        string[] Role;
        string userID;
        string RoleID;
        string RoleName;
        UserManagementBusiness objUserManagementBusiness = new UserManagementBusiness();
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        public UserManagementController()
        {
            userName = System.Web.HttpContext.Current.User.Identity.Name;
            userID = System.Web.HttpContext.Current.User.Identity.GetUserId();
            AspNetUser selectedUser = Entities.AspNetUsers.FirstOrDefault(u => u.UserName == userName);
            if (selectedUser != null)
            {
                List<AspNetRole> selectedUsersRoles = selectedUser.AspNetRoles.ToList();
                foreach (var item in selectedUsersRoles)
                {
                    RoleName = item.Name;
                    RoleID = item.Id;
                }
            }
        }

        //
        // GET: /UserManagement/

        public ActionResult Users(CreateUserModel Createusr)
        {
            try
            {
                TempData["Load"] = "FirstTime";
                // Createusr.lstBranchCode = .GetBranchCode();
                return View("~/Views/UserManagement/Users.cshtml", Createusr);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }


        public ActionResult IntermediaryPrivilages()
        {
            try
            {
                CreateUserModel obj = new CreateUserModel();
                return PartialView("~/Views/UserManagement/IntermediaryPrivilages.cshtml", obj);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }

        public ActionResult PaymentModes()
        {
            try
            {
                return PartialView("~/Views/UserManagement/PaymentModes.cshtml");
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }
        public ActionResult MenuTree(string userId, string AppId = "IMD")
        {
            try
            {
                var userDetails = Entities.AspNetUsers.FirstOrDefault(a => a.UserName == userId);
                PermissionTree obj = new PermissionTree();
                obj.objTree = new List<TreeView>();
                obj.UserID = new Guid(userDetails.Id);
                ViewBag.UserId = obj.UserID;
                obj = objUserManagementBusiness.MenuPermissionTree(obj);
                //obj.objTree = objUMLogic.MenuPermissionTree(AppId, userId, "Menu");

                var userdetails = Entities.tblUserDetails.Where(a => a.UserID == obj.UserID).FirstOrDefault();
                if (userdetails != null)
                {
                    while (userdetails.userlevel != "L0")
                    {
                        userdetails = Entities.tblUserDetails.Where(a => a.NodeID == userdetails.UserParentId).FirstOrDefault();
                    }

                    //var imdID = Entities.tblImdDetails.Where(b => b.IMDCode == userdetails.IMDCode).Select(b => b.IMDTypeID).FirstOrDefault();
                    //if (imdID == 85 || imdID == 88)
                    //{
                    //    TreeView objTemp = new TreeView();
                    //    obj.objTree = obj.objTree.Distinct().OrderBy(a => a.ItemId).ToList();
                    //}
                }

                return PartialView("~/Views/UserManagement/MenuTree.cshtml", obj);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }

        public ActionResult PaymentTree(string userId)
        {
            try
            {
                var userDetails = Entities.AspNetUsers.FirstOrDefault(a => a.UserName == userId);
                PermissionTree obj = new PermissionTree();
                obj.objTree = new List<TreeView>();
                ViewBag.UserId = userId;
                obj.UserID = new Guid(userDetails.Id);
                obj = objUserManagementBusiness.PaymentPermissionTree(obj);
                // obj.objTree = objUMLogic.PaymentPermissionTree("IMD", userId, "Payment");
                return PartialView("~/Views/UserManagement/PaymentModes.cshtml", obj);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }


        [OutputCache(Duration = 1, VaryByParam = "none")]
        public ActionResult ProductTree(Guid userId)
        {
            try
            {
                PermissionTree obj = new PermissionTree();
                obj.objTree = new List<TreeView>();
                ViewBag.UserId = userId;
                obj.UserID = userId;

                obj = objUserManagementBusiness.ProductPermissionTree(obj);
                return PartialView("~/Views/UserManagement/ProductTree.cshtml", obj);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }


        //public ActionResult ProductSpecificPrivilages(int productID, Guid userID)
        //{
        //    try
        //    {
        //        MasterController objMasterController = new MasterController();
        //        UserProductPermissions objProductPermissions = new UserProductPermissions();
        //        objUMLogic.fillProductSpecificPrivilages(objProductPermissions, userID, productID);               
        //        objProductPermissions.ListMasMakeModel = objUMLogic.GetMakeModel(productID, userID);            
        //        return PartialView("~/Views/UserManagement/ProductSpecificPrivilages.cshtml", objProductPermissions);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLogging objErrorLog = new ErrorLogging();
        //        var ErrorCode = objErrorLog.WriteException(ex, "UserManagement", "ProductSpecificPrivilages");
        //        ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
        //        return View("Error");
        //    }
        //}

        public ActionResult Roles()
        {
            try
            {
                return View("~/Views/UserManagement/Roles.cshtml");
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }

        //public ActionResult ModifyUser(int? id, CreateUserModel userobj)
        //{
        //    try
        //    {
        //        ViewData["UserName"] = id;                
        //        userobj.ParentUsers = new List<MasterListItem>();
        //        userobj.secretQuestions = new List<MasterListItem>();
        //        userobj.LstGender = objUMLogic.FetchGenders();
        //        userobj.FgbranchDetails = (from obj in Entities.tblMasBranches
        //                                   select new UserBranchMapping
        //                                   {
        //                                       FGBranchCode = obj.Code,
        //                                       FGBancaBranchCode = "",
        //                                       FGBancaBranchDescription = "",
        //                                       Receipting = "",
        //                                       AreaCode = ""
        //                                   });
        //        return View("~/Views/UserManagement/ModifyUser.cshtml", userobj);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLogging objErrorLog = new ErrorLogging();
        //        var ErrorCode = objErrorLog.WriteException(ex, "UserManagement", "ModifyUser");
        //        ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
        //        return View("Error");
        //    }
        //}
        public ActionResult CreateUser(CreateUserModel obj)
        {
            try
            {
                obj = objUserManagementBusiness.LoadCreateUser(obj);
                obj.PageName = "CreateUser";
                return View("~/Views/UserManagement/CreateUser.cshtml", obj);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }


        }

        [HttpPost]
        public void DocumentUpload(HttpPostedFileBase imgFile, string directryPath)
        {
            try
            {
                if (imgFile != null)
                {
                    if (imgFile.ContentLength > 0)
                    {
                        //string directryPath = Server.MapPath("Upload");
                        if (!Directory.Exists(directryPath))
                        {
                            Directory.CreateDirectory(directryPath);
                        }
                        var fileName = Path.GetFileName(imgFile.FileName);

                        var filename = Path.Combine(directryPath, Path.GetFileName(imgFile.FileName));
                        imgFile.SaveAs(filename);
                    }
                }
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
            }
        }
        public ActionResult UserCreation(CreateUserModel objCreateUserModel)
        {
            try
            {
                IMOUsers objIMOUsers = new IMOUsers();
                objIMOUsers.UserName = objCreateUserModel.UserName;
                objIMOUsers.UserId = objCreateUserModel.UserName;
                //objIMOUsers.u = objCreateUserModel.UserName;
                objIMOUsers.MobNo = objCreateUserModel.MobileNo;
                objIMOUsers.UserStatus = objCreateUserModel.UserStatus == "1" ? true : false;
                objIMOUsers.UserRole = objCreateUserModel.UserRole;
                objIMOUsers.AuthLimit = objCreateUserModel.AuthLimit;
                objIMOUsers.AgentCode = objCreateUserModel.UserCode;
                
                objIMOUsers.ReportingManager = objCreateUserModel.ReportingCode;
                if (objCreateUserModel.PageName != "CreateUser")
                {
                    if (objCreateUserModel.DeactivateUser == true)
                    {
                        objIMOUsers.LockoutEnable = "2030-01-01";
                        objIMOUsers.UserStatus = false;
                    }
                    if (objCreateUserModel.ReactivateUser == true)
                    {
                        objIMOUsers.LockoutEnable = null;
                        objIMOUsers.UserStatus = true;
                    }
                    else if (objCreateUserModel.DeactivateUser == false && objCreateUserModel.ReactivateUser == false)
                    {
                        objIMOUsers.LockoutEnable = objCreateUserModel.DeactivateStatus;
                    }

                }
                string userStatus = CheckADUserStatus(objIMOUsers.UserId);
                if (userStatus == "Success")
                {
                    
                    objCreateUserModel = objUserManagementBusiness.FetchUserData(objCreateUserModel);
                    if (objCreateUserModel.PageName == "CreateUser")
                    {
                        if (!objCreateUserModel.UserExist)
                        {
                            objIMOUsers = objUserManagementBusiness.FilltblMasUWUserDetails(objIMOUsers);
                        }
                        else
                        {
                            userStatus = "Duplicate";
                        }
                    }
                    else
                    {
                        objIMOUsers = objUserManagementBusiness.FilltblMasUWUserDetails(objIMOUsers);
                    }                    
                   

                }
                else
                {
                    userStatus = "Failure";
                }
                    objIMOUsers.StatusCode = userStatus;
                return Json(new { objIMOUsers.StatusCode, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json("Error");
            }
        }
        public static string CheckADUserStatus(string strUserName)
        {
            try
            {
                string ldapDomain = System.Web.Configuration.WebConfigurationManager.AppSettings["LDAPDomain"];
                string ldapBase = System.Web.Configuration.WebConfigurationManager.AppSettings["LDAPBase"];
                using (DirectoryEntry deAuthenticate = new DirectoryEntry("LDAP://" + ldapDomain + "/" + ldapBase, "iposngaduser", "J$syrhsk@3322"))
                {
                    try
                    {
                        // TODO: add your specific tasks here
                        DirectorySearcher ds = new DirectorySearcher(deAuthenticate);
                        ds.SearchScope = SearchScope.Subtree;
                        ds.Filter = "(&(objectClass=User)(sAMAccountName="+ strUserName + "))";
                        ds.PropertiesToLoad.Add("useraccountcontrol");
                        SearchResult result = ds.FindOne();
                        if (result != null)
                        {
                            foreach (System.Collections.DictionaryEntry item in result.Properties)
                            {
                                if (item.Key.ToString() == "useraccountcontrol")
                                {
                                    ResultPropertyValueCollection collection = (ResultPropertyValueCollection)item.Value;
                                    foreach (var inner in collection)
                                    {
                                        switch (Convert.ToInt32(inner))
                                        {
                                            case 514:
                                            return "User is Disabled/Terminated";
                                            case 512:
                                            return "Success";
                                            default:
                                            return "Success";
                                        }
                                    }
                                }
                            }
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                        Logger.Error(ex);
                        return "Error";
                    }
                    return "Error";
                }
                
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                return "Error";
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        [AllowAnonymous]
        public ActionResult ValidateUser(ChangePassword objChangePassword)
        {
            var userDetails = Entities.AspNetUsers.FirstOrDefault(a => a.UserName == objChangePassword.userName);
            if (userDetails != null)
            {
                var appId = (from obj in Entities.AspNetUsers
                             where obj.Id == userDetails.Id
                             select obj.UserName).FirstOrDefault();
                return Json("Valid", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        [AllowAnonymous]
        public bool GenerateRandomOTP(string UserName)
        {
            string OTP = null;
            MasterController objmaster = new MasterController();
            ChangePassword objChangePassword = new Life.Models.UserManagement.ChangePassword();
            objChangePassword = objUserManagementBusiness.GetResendOTPInformation(objChangePassword, UserName);
            //OTP = WebApiLogic.GetPostComplexTypeToAPI<string>(UserName, "ResendOTPInformation", "UserManagementAPI");            
            //OTP = objUMLogic.ResendOTPInformation(UserName);
            if (objChangePassword.Result == null)
            {
                OTP = objmaster.GenerateRandomOTP();
                string mobileNumber = null;
                string UserId = null;
                string email = null;
                var userDetails = Entities.tblUserDetails.FirstOrDefault(a => a.LoginID == UserName);
                if (userDetails != null)
                {
                    mobileNumber = userDetails.ContactNo;
                    UserId = Convert.ToString(userDetails.UserID);
                    email = userDetails.Email;
                }
                CreateUserModel objCreateUser = new CreateUserModel();
                objCreateUser.UserName = UserName;
                objCreateUser.UserID = new Guid(UserId);
                objCreateUser.OTP = OTP;
                objCreateUser = objUserManagementBusiness.SaveOTPInformation(objCreateUser);
                // bool status= objUMLogic.SaveOTPInformation(new Guid(UserId), OTP, UserName);               
                return objCreateUser.Status;
            }
            else
            {
                return true;
            }
        }
        [AllowAnonymous]
        public ActionResult ResetForgotPasswordOnSuccess(ChangePassword objChangePassword)
        {
            try
            {
                string result = string.Empty;
                if (objChangePassword.NICAnswer == true)
                {
                    var Result = ValidateNICNumber(objChangePassword.NICNumber, objChangePassword.userName);
                    return Json(objChangePassword.Result, JsonRequestBehavior.AllowGet);
                }
                if (objChangePassword.OTPOrAnswer == true)
                {
                    objChangePassword = objUserManagementBusiness.VerifyOTPInformation(objChangePassword);
                    //result = objUMLogic.VerifyOTPInformation(objChangePassword.OTP, objChangePassword.userName);
                    return Json(objChangePassword.Result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string question = string.Empty;
                    string answer = string.Empty;
                    string appId = string.Empty;

                    var appDetails = (from obj in Entities.tblUserDetails
                                      where obj.LoginID == objChangePassword.userName
                                      select obj).FirstOrDefault();
                    //if (appDetails != null)
                    //{
                    //    appId = appDetails.ApplicationId.ToString().ToUpper();
                    //}

                    //if (ConfigurationManager.AppSettings["EMPAppID"] == appId)
                    //{
                    //    return Json("Insufficient privileges to Reset password", JsonRequestBehavior.AllowGet);
                    //}

                    tblUserDetail objUserDetails = Entities.tblUserDetails.FirstOrDefault(a => a.LoginID == objChangePassword.userName);
                    if (objUserDetails != null)
                    {
                        if (objChangePassword.hintQuetn == Convert.ToString(objUserDetails.PasswordQuestionID))
                        {
                            question = Convert.ToString(objUserDetails.PasswordQuestionID);
                            answer = objUserDetails.PasswordAnswer;
                        }
                        else
                        {
                            result = "Password Question Did not Match";
                        }

                        if (question != string.Empty)
                        {
                            if (objChangePassword.hintAns == answer)
                            {
                                result = "true";
                            }
                            else
                            {
                                result = "Password Answer did not match";
                            }
                        }
                    }
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }
        [AllowAnonymous]
        public ActionResult ResetForgotUserPassword(ChangePassword objChangePassword)
        {
            try
            {
                if (this.IsCaptchaValid("Invalid Captcha"))
                {
                    return View("~/Views/UserManagement/ResetForgotPassword.cshtml", objChangePassword);
                }
                else
                {
                    objChangePassword.IsCaptchaError = true;
                    objChangePassword = objUserManagementBusiness.LoadSecretQuestions(objChangePassword);
                    return RedirectToAction("ForgotPassword", objChangePassword);
                }
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }
        [AllowAnonymous]
        public async Task<ActionResult> UnlockUserOnSuccess(ChangePassword objChangePassword)
        {
            try
            {
                if (this.IsCaptchaValid("Invalid Captcha"))
                {
                    // objChangePassword = WebApiLogic.GetPostComplexTypeToAPI<ChangePassword>(objChangePassword, "UnlockUserOnSuccess", "UserManagementApi");
                    var UserID = (from obj in Entities.AspNetUsers
                                  where obj.UserName == objChangePassword.userName
                                  select obj.Id).FirstOrDefault();
                    var result = await UserManager.SetLockoutEnabledAsync(Convert.ToString(UserID), true);
                    if (result.Succeeded)
                    {
                        await UserManager.ResetAccessFailedCountAsync(Convert.ToString(UserID));
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    objChangePassword.IsCaptchaError = true;
                    objChangePassword = objUserManagementBusiness.LoadSecretQuestions(objChangePassword);
                    return RedirectToAction("ForgotPassword", objChangePassword);
                }
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }
        [AllowAnonymous]
        public async Task<ActionResult> ResetForgotPasswordDetails(ChangePassword Password)
        {
            try
            {
                string userName = Password.userName; string NewPassword = Password.newPassword;
                var record = (from objtbluserDetails in Entities.tblUserDetails
                              where objtbluserDetails.LoginID == userName
                              select new
                              {
                                  Email = objtbluserDetails.Email,
                                  Mobile = objtbluserDetails.ContactNo
                              });
                string emailID = record.FirstOrDefault().Email;
                string MobileNo = record.FirstOrDefault().Mobile;
                var user = await UserManager.FindByNameAsync(userName);
                if (user == null)
                {
                    return View("~/Views/Account/ResetPasswordConfirmation.cshtml", JsonRequestBehavior.AllowGet);
                }
                if (user.PasswordHash != null)
                {
                    UserManager.RemovePassword(user.Id);
                }
                var PasswordResult = UserManager.AddPassword(user.Id, Password.newPassword);
                if (PasswordResult.Succeeded)
                {
                    tblUserDetail objUserDetail = Entities.tblUserDetails.FirstOrDefault(a => a.LoginID == userName);
                    if (objUserDetail != null)
                    {
                        if (Password.IsSecurityChecked == true)
                        {
                            objUserDetail.PasswordQuestionID = Convert.ToInt32(Password.hintQuetn);
                            objUserDetail.PasswordAnswer = Password.hintAns;
                            objUserDetail.IsSecurityChecked = true;
                        }
                        objUserDetail.LastPasswordChangedDate = DateTime.Now;
                        Entities.SaveChanges();
                    }
                    Password.mobileno = MobileNo;
                    Password.emailID = emailID;
                    Password = objUserManagementBusiness.Sendmailsmsresetpassword(Password);
                    return Json("True", JsonRequestBehavior.AllowGet);
                }

                return Json("Unable to Reset Password", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return Json("Error", "application/json");
            }
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            return RedirectToAction("ResetPasswordConfirmation", "Account");
        }
        [AllowAnonymous]
        public string ResendOTP(string UserName)
        {

            ChangePassword objChangePassword = new Life.Models.UserManagement.ChangePassword();
            objChangePassword = objUserManagementBusiness.GetResendOTPInformation(objChangePassword, UserName);
            return objChangePassword.Result;
        }
        [AllowAnonymous]
        public ActionResult ValidateNICNumber(string NICNumber, string UserName)
        {
            try
            {
                string Status = string.Empty;
                if (NICNumber != null && NICNumber != "")
                {
                    UserName = UserName.Remove(UserName.Length - 3, 3);
                    var Result = (from obj in Entities.tblProspects
                                  where obj.NIC == NICNumber && obj.AgentCode == UserName
                                  select obj.NIC).FirstOrDefault();
                    if (Result != null)
                    {
                        if (Result.Count() > 0)
                        {
                            Status = "true";
                        }
                    }
                    else
                    {
                        Status = "false";
                    }
                }
                else
                {
                    Status = "false";
                }
                return Json(Status, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        //public ActionResult SaveUserDetails(string objUserBranchMapping, CreateUserModel objCreateUserModel, HttpPostedFileBase UploadImage)
        //{
        //    try
        //    {
        //        Guid loginUserId = Guid.Empty;
        //        string userName = objCreateUserModel.UserIdName;
        //        List<UserBranchMapping> objBranchDetails = new List<UserBranchMapping>();
        //        objBranchDetails = JsonConvert.DeserializeObject<List<UserBranchMapping>>(objUserBranchMapping);
        //        var currentUser = UserManager.FindByName(objCreateUserModel.UserCode);
        //        if (!objCreateUserModel.Parent)
        //        {
        //            objCreateUserModel.UserRole = objCreateUserModel.RoleName;
        //        }
        //        var roleresult = UserManager.AddToRole(currentUser.Id, objCreateUserModel.UserRole);
        //        objCreateUserModel = objUMLogic.SaveUserFG(objCreateUserModel, userName, objBranchDetails, UploadImage);                            
        //        objCreateUserModel.ParentUsers = objUMLogic.FetchParentId();
        //        objCreateUserModel.FgbranchDetails = objBranchDetails;               
        //        return Json(objCreateUserModel, "application/json");
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLogging objErrorLog = new ErrorLogging();
        //        var ErrorCode = objErrorLog.WriteException(ex, "UserManagement", "SaveUserDetails");
        //        ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
        //        return Json("Error", "application/json");
        //    }
        //}

        public ActionResult SavePaymentModes(CreateUserModel createusr)
        {
            createusr = objUserManagementBusiness.FetchPaymentModes(createusr);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchIMD()
        {
            try
            {
                return PartialView("~/Views/UserManagement/SearchIMDUser.cshtml");
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }

        //public ActionResult SearchUserDetails(CreateUserModel objsearch)
        //{
        //    try
        //    {
        //        IMDUsers Createusr = new IMDUsers();
        //        Createusr.ListUserData = objUMLogic.GetUserDetails(objsearch.UserName, objsearch.catagory, objsearch.userType, objsearch.agentCode);
        //        var GridUserData = Createusr.ListUserData.AsQueryable();
        //        AjaxGrid<IMDUsers> usergrid = null;
        //        usergrid = (AjaxGrid<IMDUsers>)new AjaxGridFactory().CreateAjaxGrid(GridUserData, 1, false);

        //        return PartialView("~/Views/UserManagement/GridUserDetailsPartial.cshtml", usergrid);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLogging objErrorLog = new ErrorLogging();
        //        var ErrorCode = objErrorLog.WriteException(ex, "UserManagement", "SearchUserDetails");
        //        ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
        //        return View("Error");
        //    }
        //}       
        public ActionResult AuthenticateUsers()
        {
            try
            {
                return View("~/Views/UserManagement/AuthenticateImdLogin.cshtml");
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }
        [AllowAnonymous]
        public ActionResult GetMobileNumber(string UserName)
        {
            try
            {
                if (UserName == null)
                    UserName = LoginUser.GetUserName();
                var mobileNo = (from obj in Entities.tblUserDetails
                                where obj.LoginID == UserName
                                select obj.ContactNo).FirstOrDefault();
                if (mobileNo != null)
                {
                    var FistOne = mobileNo.Substring(0, 1);
                    var LastTwo = mobileNo.Substring(mobileNo.Length - 2, 2);
                    mobileNo = FistOne + "xxxxxx" + LastTwo;

                }
                return Json(mobileNo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        [AllowAnonymous]
        public ActionResult GetUnlockAccount(ChangePassword objChangePassword = null)
        {
            try
            {
                if (objChangePassword == null)
                {
                    objChangePassword = new ChangePassword();
                }
                objChangePassword = objUserManagementBusiness.LoadSecretQuestions(objChangePassword);
                return View("~/Views/UserManagement/UnlockAccount.cshtml", objChangePassword);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }
        [AllowAnonymous]
        public ActionResult GetForgotPassword()
        {
            try
            {
                return View("~/Views/UserManagement/ForgotPassword.cshtml");
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }
        public ActionResult ResetForgotPassword()
        {
            try
            {
                return View("~/Views/UserManagement/ResetPassword.cshtml");
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }
        [AllowAnonymous]
        public ActionResult ForgotPassword(ChangePassword objChangePassword = null)
        {
            try
            {
                if (objChangePassword == null)
                {
                    objChangePassword = new ChangePassword();
                }
                objChangePassword = objUserManagementBusiness.LoadSecretQuestions(objChangePassword);
                return View("~/Views/UserManagement/ForgotPassword.cshtml", objChangePassword);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }
        public ActionResult DeleteUser(int id)
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }
        public ActionResult SearchClient(ImdcodeCreationModel objsearch)
        {
            try
            {
                return PartialView("~/Views/UserManagement/SearchClient.cshtml");
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }
        public ActionResult PaymentModeView()
        {
            try
            {
                TempData["Load"] = "FirstTime";
                return View("~/Views/UserManagement/PaymentView.cshtml");
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }
        public ActionResult PartialPayment()
        {
            try
            {
                return PartialView("~/Views/UserManagement/PartialPaymentOptions.cshtml");
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }

        public ActionResult UsersDetails(CreateUserModel Createusr)
        {
            try
            {
                IMDUsers objimdUsers = new IMDUsers();
                Createusr = objUserManagementBusiness.FetchGridUserDetails(Createusr);
                //objimdUsers.ListUserData = objUMLogic.FetchGridUserDetails(Createusr);
                var GridUserData = Createusr.ListUserData.AsQueryable();
                ViewBag.Details = Createusr;
                AjaxGrid<IMDUsers> usergrid = null;
                usergrid = (AjaxGrid<IMDUsers>)new AjaxGridFactory().CreateAjaxGrid(GridUserData, 1, false);

                return PartialView("~/Views/UserManagement/GridUserDetailsPartial.cshtml", usergrid);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }
        public ActionResult ChildIdView()
        {
            try
            {
                return PartialView("~/Views/UserManagement/ChildEnquiry.cshtml");
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }
        public ActionResult DeletePhoto(string imgurl)
        {
            try
            {
                var photoName = "";
                photoName = imgurl;
                string fullPath = Server.MapPath("~/Uploads/userId/" + photoName);

                System.IO.File.Delete(fullPath);

                return View();
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }

        public ActionResult EditUserProfile(CreateUserModel user)
        {
            try
            {
                //user.EditUserID =Convert.ToInt32(id);
                user = objUserManagementBusiness.LoadCreateUser(user);
                //if (id != 0 && id != null)
                //{
                //    ViewBag.isEditUser = true;
                //}
                //else
                //{
                //    ViewBag.isEditUser = false;
                //}
                //user.userAccount = new UserAccount();
                //user.userDetails = new UserDetails();
                //user.UserID = LoginUser.GetUserId();
                //user.UserIdName = LoginUser.GetUserName();
                //user.userExtension = new UserExtension();
                //if (!string.IsNullOrEmpty(user.UserIdName))
                //{
                //    if (LoginUser.GetUserName().ToLower().Trim() == user.UserIdName.ToLower().Trim())
                //    {
                //        ViewBag.isActiveUser = true;
                //    }
                //    else
                //    {
                //        ViewBag.isActiveUser = false;
                //    }
                //}
                //else
                //{
                //    ViewBag.isActiveUser = false;
                //}
                //AVOAIALifeEntities entity = new AVOAIALifeEntities();

                //var all = entity.AspNetRoles.Select(x => x);
                //foreach (var item in all)
                //{
                //    SelectListItem obj = new SelectListItem();
                //    obj.Value = item.Name;
                //    obj.Text = item.Name;
                //    user.ddlRoles.Add(obj);
                //}

                return View("~/Views/UserManagement/ModifyUser.cshtml", user);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult LoadImdsNxtPage(int? Page, bool status, string imdCode, string firstName, string lastName, string branchCode, decimal fgChannel)
        {
            try
            {
                ImdcodeCreationModel objIMdCode = new ImdcodeCreationModel();
                objIMdCode.Imdcode = (string.IsNullOrEmpty(imdCode) || imdCode == "null") ? "" : imdCode;
                objIMdCode.FirstName = (string.IsNullOrEmpty(firstName) || firstName == "null") ? "" : firstName;
                objIMdCode.Lastname = (string.IsNullOrEmpty(lastName) || lastName == "null") ? "" : lastName;
                objIMdCode.BranchCode = (string.IsNullOrEmpty(branchCode) || branchCode == "null") ? "" : branchCode;
                objIMdCode.FGChannel = fgChannel;
                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                List<IMDUsers> lstIMD = new List<IMDUsers>();
                //lstIMD = objUMLogic.FetchGridImdDetails(objIMdCode).ToList();
                var grid = aj.CreateAjaxGrid(lstIMD.AsQueryable(), Page.HasValue ? Page.Value : 1, Page.HasValue, 2);

                ViewBag.Data = status;
                ViewBag.Details = objIMdCode;
                return Json(new
                {
                    // Html = RenderPartialViewToString("~/Views/UserManagement/GridImdDetailsPartial.cshtml", grid),
                    HasItems = grid.HasItems
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";

                return View("Error");
            }
        }
        [HttpGet]
        public ActionResult LoadNxtUserPage(int? Page, string imdCode, string userCode, string loginId, string branchCode)
        {
            try
            {
                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                List<IMDUsers> lstIMD = new List<IMDUsers>();
                CreateUserModel createUser = new CreateUserModel();
                createUser.IMDCode = imdCode;
                createUser.UserCode = userCode;
                createUser.UserIdName = loginId;
                createUser.branhCode = branchCode;
                createUser = objUserManagementBusiness.FetchGridUserDetails(createUser);
                //lstIMD = objUMLogic.FetchGridUserDetails(createUser).ToList();

                var grid = aj.CreateAjaxGrid(createUser.ListUserData.AsQueryable(), Page.HasValue ? Page.Value : 1, Page.HasValue, 2);
                ViewBag.Details = createUser;

                return Json(new
                {
                    Html = RenderPartialViewToString("~/Views/UserManagement/GridUserDetailsPartial.cshtml", grid),
                    HasItems = grid.HasItems
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return Json("Error", "application/json");
            }
        }
        protected string RenderPartialViewToString(string viewName, object model)
        {
            try
            {
                if (string.IsNullOrEmpty(viewName))
                    viewName = ControllerContext.RouteData.GetRequiredString("action");

                ViewData.Model = model;

                using (var sw = new StringWriter())
                {
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                    var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                    viewResult.View.Render(viewContext, sw);

                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return "Error";
            }
        }

        [HttpGet]
        public ActionResult LoadNxtPageSearchIMD(int? Page, string Details)
        {
            try
            {
                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                List<ImdcodeCreationModel> lstIMD = new List<ImdcodeCreationModel>();
                var result = Details.Split(',');
                //lstIMD = objUMLogic.GetIMDUserInfo(result[0], result[1], result[2], result[3], result[4]).ToList();

                var grid = aj.CreateAjaxGrid(lstIMD.AsQueryable(), Page.HasValue ? Page.Value : 1, Page.HasValue, 2);

                return Json(new
                {
                    //Html = RenderPartialViewToString("~/Views/UserManagement/SearchIMDUserGrid.cshtml", grid),
                    HasItems = grid.HasItems
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return Json("Error", "application/json");
            }
        }

        [HttpGet]
        public ActionResult LoadNxtPageClient(int? Page, string Details)
        {
            try
            {
                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                List<ImdcodeCreationModel> lstIMD = new List<ImdcodeCreationModel>();
                var result = Details.Split(',');
                //lstIMD = objUMLogic.GetSearchClientInfo(result[0], result[1], result[2], result[3], result[4]).ToList();

                var grid = aj.CreateAjaxGrid(lstIMD.AsQueryable(), Page.HasValue ? Page.Value : 1, Page.HasValue, 2);


                return Json(new
                {
                    //Html = RenderPartialViewToString("~/Views/UserManagement/SearchClientCodeGrid.cshtml", grid),
                    HasItems = grid.HasItems
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return Json("Error", "application/json");
            }
        }

        [HttpGet]
        public ActionResult LoadNxtClientIDs(int? Page, string nodeID)
        {
            try
            {
                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                List<ChildIDs> lstChildIDs = new List<ChildIDs>();


                var grid = aj.CreateAjaxGrid(lstChildIDs.AsQueryable(), Page.HasValue ? Page.Value : 1, Page.HasValue, 2);

                return Json(new
                {
                    //  Html = RenderPartialViewToString("~/Views/UserManagement/ChildEnquiry.cshtml", grid),
                    HasItems = grid.HasItems
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return Json("Error", "application/json");
            }
        }
        public ActionResult SavePermissions(List<int?> permissionIDs, List<int?> IndetPerm, string RoleNames, string permissionType, string IMEINum, bool SelfInsp = false, bool RaiseInsp = false, bool Recommendation = false)
        {
            try
            {
                CreateUserModel objCreateUserModel = new CreateUserModel();
                objCreateUserModel.PermissionIDs = permissionIDs;
                objCreateUserModel.IndetPerm = IndetPerm;
                if (RoleNames == null || RoleNames == "")
                {
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }
                tblUserDetail objUserDetails = new tblUserDetail();
                var RoleName = (from obj in Entities.AspNetRoles
                                where obj.Name == RoleNames
                                select obj.Name).FirstOrDefault();
                objCreateUserModel.RoleName = RoleName;
                if (RoleName == null)
                {
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }
                objCreateUserModel.permissionType = permissionType;
                objCreateUserModel.IMEINumber = IMEINum;
                objCreateUserModel = objUserManagementBusiness.SavePermission(objCreateUserModel);
                return Json(objCreateUserModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return Json("Error", "application/json");
            }
        }
        [OutputCache(Duration = 1, VaryByParam = "none")]
        public ActionResult ProductPrivilages(string userId)
        {
            try
            {
                CreateUserModel obj = new CreateUserModel();
                obj.userDetails = new UserDetails();
                obj.userDetails.UserID = new Guid(userId);
                return PartialView("~/Views/UserManagement/ProductPrivilages.cshtml", obj);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }

        //public ActionResult SaveProductPrivilages(UserProductPermissions productPermision, int ProductId, Guid UserId, string productCodes)
        //{
        //    productCodes = productPermision.vhProductCodes;
        //    try
        //    {
        //        int saveStatus = 0;               
        //       saveStatus = objUMLogic.SaveProductPrivilages(productPermision, ProductId, UserId, productCodes);
        //        return Json(Convert.ToString(saveStatus), "application/json");
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLogging objErrorLog = new ErrorLogging();
        //        var ErrorCode = objErrorLog.WriteException(ex, "UserManagement", "SaveProductPrivilages");
        //        ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
        //        return Json("Error", "application/json");
        //    }
        //}    
        public ActionResult ChangePassword()
        {
            try
            {

                ChangePassword objChangePassword = new ChangePassword();
                objChangePassword = objUserManagementBusiness.LoadSecretQuestions(objChangePassword);
                ViewBag.IsFirstTimeLogin = TempData["IsFirstTimeLogin"];
                if (ViewBag.IsFirstTimeLogin == "True")
                {
                    objChangePassword.IsSecurityChecked = true;
                }
                else
                {
                    objChangePassword.IsSecurityChecked = false;
                }
                return View("~/Views/UserManagement/ChangePassword.cshtml", objChangePassword);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }
        //[AllowAnonymous]
        // [ValidateAntiForgeryToken]
        //[CaptchaValidator(
        //PrivateKey = "6Lc1MEEUAAAAAKofi3TE2GR43bT2Yo8-0xCIEo6o",
        //ErrorMessage = "Invalid input captcha.",
        //RequiredMessage = "The captcha field is required.")]
        [HttpPost]
        [CaptchaValidator]
        public async Task<ActionResult> ResetPassword(ChangePassword Password, bool captchaValid)
        {
            //const string secretKey = "6Lc1MEEUAAAAAKofi3TE2GR43bT2Yo8-0xCIEo6o";
            //string userResponse = Request.Form["g-Recaptcha-Response"];
            //var webClient = new System.Net.WebClient();
            // string verification = webClient.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, userResponse));
            //var verificationJson = Newtonsoft.Json.Linq.JObject.Parse(verification);
            //if (verificationJson.Value<bool>())
            //{
            //    Session["I_AM_NOT_A_ROBOT"] = "true";
            //    return RedirectToAction("Index", "Demo");
            //}
            //if (!ModelState.IsValid)
            //{
            //    return View(Password);
            //}
            //            var encodedResponse = Request.Form["g-Recaptcha-Response"];
            //var isCaptchaValid = CaptchaResponse.Validate(encodedResponse);

            //if (!isCaptchaValid)
            //{
            //    // E.g. Return to view or set an error message to visible
            //} 
            //if (!captchaValid)
            //{
            //    return Json("Invalida Captch", JsonRequestBehavior.AllowGet);
            //}
            var user = await UserManager.FindByNameAsync(userName);
            if (user == null)
            {
                return View("~/Views/Account/ResetPasswordConfirmation.cshtml", JsonRequestBehavior.AllowGet);
            }
            if (!UserManager.CheckPassword(user, Password.oldPassword))
            {
                return Json("old password is incorrect", JsonRequestBehavior.AllowGet);
            }
            if (user.PasswordHash != null)
            {
                UserManager.RemovePassword(user.Id);
            }

            var result = UserManager.AddPassword(user.Id, Password.newPassword);
            //var result = await UserManager.ResetPasswordAsync(user.Id, userName, Password.newPassword);
            if (result.Succeeded)
            {
                tblUserDetail objUserDetail = Entities.tblUserDetails.FirstOrDefault(a => a.LoginID == userName);
                if (objUserDetail != null)
                {
                    if (Password.IsSecurityChecked == true)
                    {
                        objUserDetail.PasswordQuestionID = Convert.ToInt32(Password.hintQuetn);
                        objUserDetail.PasswordAnswer = Password.hintAns;
                        objUserDetail.IsSecurityChecked = true;
                    }
                    objUserDetail.LastPasswordChangedDate = DateTime.Now;
                    Entities.SaveChanges();
                }
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            AddErrors(result);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        public ActionResult FetchAdvisorCodeData(string AdvisorCode)
        {
            try
            {
                CreateUserModel objCreateUserModel = new CreateUserModel();
                objCreateUserModel = objUserManagementBusiness.GetAdvisorCodeData(objCreateUserModel, AdvisorCode);
                //objCreateUserModel = objUMLogic.FetchAdvisorCodeData(AdvisorCode);
                return Json(objCreateUserModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        [AllowAnonymous]
        public ActionResult GetResetForgotPassword(ChangePassword pwd)
        {
            try
            {
                string question = string.Empty;
                string answer = string.Empty;
                string result = string.Empty;
                string appId = string.Empty;

                var appDetails = (from obj in Entities.Users
                                  where obj.UserName == pwd.userName
                                  select obj).FirstOrDefault();
                if (appDetails != null)
                {
                    appId = appDetails.ApplicationId.ToString().ToUpper();
                }

                if (ConfigurationManager.AppSettings["EMPAppID"] == appId)
                {
                    return Json("Insufficient privileges to Reset password", JsonRequestBehavior.AllowGet);
                }

                tblUserDetail objUserDetails = Entities.tblUserDetails.FirstOrDefault(a => a.LoginID == pwd.userName);
                if (objUserDetails != null)
                {
                    if (pwd.hintQuetn == Convert.ToString(objUserDetails.PasswordQuestionID))
                    {
                        question = Convert.ToString(objUserDetails.PasswordQuestionID);
                        answer = objUserDetails.PasswordAnswer;
                    }
                    else
                    {
                        result = "false" + "|" + "Password Question Did not Match";
                    }

                    if (question != string.Empty)
                    {
                        if (pwd.hintAns == answer)
                        {
                            var reset = ResetUserPassword(pwd.userName, pwd.hintAns);
                            var res1 = (((System.Web.Mvc.JsonResult)(reset)).Data).ToString();
                            var res = res1.Split('|');
                            if (res[0] == "true")
                            {
                                result = "true";
                            }
                        }
                        else
                        {
                            result = "false" + "|" + "Password Answer did not match";
                        }
                    }
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return Json("Error", "application/json");
            }
        }

        public ActionResult ResetUserPassword(string userName, string answer)
        {
            try
            {
                var emailID = (from objtbluserDetails in Entities.tblUserDetails
                               where objtbluserDetails.LoginID == userName
                               select objtbluserDetails.Email).FirstOrDefault();
                string appName = string.Empty;

                appName = (from obj in Entities.Users
                           where obj.UserName == userName
                           select obj.Application.ApplicationName).FirstOrDefault();


                string result1 = "";
                string result2 = "";
                string password = "";
                bool emailStatus = false;
                try
                {
                    var membershipUser = System.Web.Security.Membership.Providers["UserMembershipProvider"];
                    membershipUser.ApplicationName = appName;
                    membershipUser.UnlockUser(userName);
                    System.Web.Security.MembershipUser currentUser = membershipUser.GetUser(userName, false);
                    if (currentUser != null)
                    {
                        userName = currentUser.ToString();
                    }
                    password = currentUser.ResetPassword();
                    if (password != "" && password != null)
                    {
                        result1 = "true";
                    }

                }
                catch (Exception)
                {
                    result1 = "false";
                }

                if (result1 == "true")
                {
                    //--To send password to email account--    
                    //EmailIntegration objEmail = new EmailIntegration();
                    //emailStatus = objEmail.sendUserActivationEmail(emailID, password, userName);
                }
                result2 = Convert.ToString(emailStatus);
                result1 = result1 + "|" + result2;
                return Json(result1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return Json("Error", "application/json");
            }
        }
        public static string Encrypt(string input, string key)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public ActionResult Configuration()
        {
            objConfiguration = objUserManagementBusiness.LoadConfigurationTask(objConfiguration);
            if (objConfiguration.ObjConfigurationGridData == null)
            {
                objConfiguration.ObjConfigurationGridData = new List<Life.Models.UserManagement.Configuration.ConfigurationGridData>();
            }
            return View(objConfiguration);
        }
        public ActionResult ConfigurationTasks()
        {


            return View();
        }
        public ActionResult UserAccess(CreateUserModel obj)
        {
            try
            {
                return View("~/Views/UserManagement/UserAccess.cshtml", obj);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }
        }
        public ActionResult GetAllUserDetails(CreateUserModel obj)
        {
            try
            {
                obj = objUserManagementBusiness.FetchUserDetails(obj);
                TempData["Load"] = "FirstTime";
                //var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                //var grid = (AjaxGrid<CreateUserModel>)new AjaxGridFactory().CreateAjaxGrid((obj.LstCreateUserModel).AsQueryable(), 1, false);
                ViewBag.count = obj.LstCreateUserModel.Count();
                return PartialView("~/Views/UserManagement/_PartialUserSearch.cshtml", obj);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }

        }
        public ActionResult PartialUserDetails(string UserName,string UserCode,string DeviceId,int NodeId)
        {
      
            try
            {
                CreateUserModel obj = new CreateUserModel();
                obj.UserName = UserName;
                obj.UserCode = UserCode;
                obj.DeviceID = DeviceId;
                obj.NodeID = NodeId;
                obj = objUserManagementBusiness.FetchUserDetails(obj);
                obj.UserCatagory = obj.UserRole;
                return PartialView("~/Views/UserManagement/_PartialValidatedUser.cshtml", obj);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }

        }
        public ActionResult ValidateUserDetails(CreateUserModel obj)
        {
            try
            {
                obj.PageName = "UserSearch";                
                obj = objUserManagementBusiness.FetchUserDetails(obj);
                obj.UserCatagory = obj.UserRole;
                 if(!string.IsNullOrEmpty(obj.UserName) && !string.IsNullOrEmpty(obj.IMDCode))
                {
                    obj.ErrorMessage = "Success";
                    return PartialView("~/Views/UserManagement/_PartialValidatedUser.cshtml", obj);
                }
                else
                {
                    obj.ErrorMessage = "NotExist";
                    return PartialView("~/Views/UserManagement/_PartialValidatedUser.cshtml", obj);
                }       
            
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }

        }
        public ActionResult ValidateDeviceName(CreateUserModel obj)
        {
            try
            {
                obj.PageName = "UserSearch";
                if (string.IsNullOrEmpty(obj.DeviceID))
                {
                    obj.DeviceID = obj.objDeviceDetails.DeviceID;
                }
                obj = objUserManagementBusiness.FetchDeviceName(obj);
                obj.UserCatagory = obj.UserRole;

                return Json(new { obj, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }

        }
        public ActionResult DiviceHistory(CreateUserModel obj)
        {
            try
            {
                obj = objUserManagementBusiness.FetchDeviceHistory(obj);
                return PartialView("~/Views/UserManagement/_PartialDeviceHistory.cshtml", obj);
            }
            catch (Exception ex)
            {
                string ErrorCode = "";
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ErrorCode + ")";
                return View("Error");
            }

        }
        public ActionResult UpdateDevicedetails(CreateUserModel obj)
        {
            try
            {
                obj = objUserManagementBusiness.UpdateDevicedetails(obj);
                return Json(new { obj, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json("Error");
            }
        }
        public ActionResult AppUpdate()
        {
            AppNotification obj = new AppNotification();
            obj = objUserManagementBusiness.AppUpdateMaster(obj);
            return View(obj);
        }
        public ActionResult AppNotofication()
        {
            AppNotification obj = new AppNotification();
            obj = objUserManagementBusiness.AppNotificationMaster(obj);
            return View(obj);
        }
        public ActionResult SendNotificationFromFirebaseCloud(AppNotification objAppNotification)
        {
            try
            {
                if (objAppNotification.SelectRecepient == 2735)
                {
                    UserSearch objUserSearch = new UserSearch();
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    if (!string.IsNullOrEmpty(objAppNotification.HdnLSTRecepient))
                    {
                        objAppNotification.lstUserSearch = JsonConvert.DeserializeObject<List<UserSearch>>(objAppNotification.HdnLSTRecepient, settings);
                        var LstDeviceToken = objAppNotification.lstUserSearch.Select(x => x.DeviceID).ToList();
                        objAppNotification.AllDeviceTokenID = string.Join(",", LstDeviceToken.ToArray());
                    }
                    UserManagementBusiness objUserManagementBusiness = new UserManagementBusiness();
                    if (objAppNotification.SelectRecepient != 2735)
                    {
                        objAppNotification = objUserManagementBusiness.GetDeviceToken(objAppNotification);
                    }
                    objAppNotification.Result = objUserManagementBusiness.SendNotificationFromFirebaseCloud(objAppNotification);
                }
                //objAppNotification.Result = objUserManagementBusiness.SendNotificationFromFirebaseCloud(objAppNotification);
               return Json(new { objAppNotification.Result, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json("Error");
            }
        }

        public ActionResult SendAppUpdate(AppNotification objAppUpdate)
        {
            try
            {
                objAppUpdate = objUserManagementBusiness.SaveAppUpdate(objAppUpdate);
                objAppUpdate.Result = objUserManagementBusiness.SendNotificationFromFirebaseCloud(objAppUpdate, true);
                return Json(new { objAppUpdate.Result, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json("Error");
            }
        }
        public ActionResult GetIndividual(string term)
        {
            try
            {
                UserSearch objUserSearch = new UserSearch();
                IEnumerable<UserSearch> records = null;
                objUserSearch.ParmData = term;
                objUserSearch = objUserManagementBusiness.SearchIndividual(objUserSearch);

                records = objUserSearch.LstUserSerch.Where(l => l.Value.Contains(term.ToUpper())).ToList();
                return Json(records, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult AddRecepient(AppNotification objAppNotification)
        {
            try
            {
                UserSearch objUserSearch = new UserSearch();
                JsonSerializerSettings settings = new JsonSerializerSettings();
                if (!string.IsNullOrEmpty(objAppNotification.HdnLSTRecepient))
                {
                    // objAppNotification.HdnLSTRecepient = objAppNotification.HdnLSTRecepient.Replace('quot', '"');
                    objAppNotification.lstUserSearch = JsonConvert.DeserializeObject<List<UserSearch>>(objAppNotification.HdnLSTRecepient, settings);
                    if (objAppNotification.lstUserSearch.Where(x => x.NodeId == objAppNotification.HdnNodeID).Count() > 0)
                    {
                        return Json("Error", JsonRequestBehavior.AllowGet);
                    }
                }
                objUserSearch = objUserManagementBusiness.SearchIndividual(objUserSearch);
                var data = objUserSearch.LstUserSerch.Where(x => x.NodeId == objAppNotification.HdnNodeID).FirstOrDefault();
                UserSearch objUser = new UserSearch();
                if (objAppNotification.lstUserSearch == null)
                    objAppNotification.lstUserSearch = new List<UserSearch>();
                objAppNotification.lstUserSearch.Add(data);
                // objAppNotification.HdnLSTRecepient = JsonConvert.SerializeObject(objAppNotification.lstUserSearch);
                return PartialView("~/Views/UserManagement/_PartialRecepient.cshtml", objAppNotification);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ValidateRecepient(decimal? NodeID, string HdnLSt)
        {
            try
            {
                UserSearch objUserSearch = new UserSearch();
                JsonSerializerSettings settings = new JsonSerializerSettings();
                if (!string.IsNullOrEmpty(HdnLSt))
                {
                    // objAppNotification.HdnLSTRecepient = objAppNotification.HdnLSTRecepient.Replace('quot', '"');
                    objUserSearch.LstUserSerch = JsonConvert.DeserializeObject<List<UserSearch>>(HdnLSt, settings);
                    if (objUserSearch.LstUserSerch.Where(x => x.NodeId == NodeID).Count() > 0)
                    {
                        return Json("User Is Already Added", JsonRequestBehavior.AllowGet);
                    }
                }
                objUserSearch = objUserManagementBusiness.SearchIndividual(objUserSearch);
                var data = objUserSearch.LstUserSerch.Where(x => x.NodeId == NodeID).FirstOrDefault();
                if (data != null && data.DeviceID == null)
                {
                    return Json("Device Id is Not Registered ", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteRecepient(AppNotification objAppNotification)
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                if (!string.IsNullOrEmpty(objAppNotification.HdnLSTRecepient))
                {
                    objAppNotification.lstUserSearch = JsonConvert.DeserializeObject<List<UserSearch>>(objAppNotification.HdnLSTRecepient, settings);
                    if (objAppNotification.lstUserSearch.Count() > 0)
                    {
                        var data = objAppNotification.lstUserSearch.Where(x => x.NodeId == objAppNotification.HdnNodeID).FirstOrDefault();
                        objAppNotification.lstUserSearch.Remove(data);
                        return PartialView("~/Views/UserManagement/_PartialRecepient.cshtml", objAppNotification);
                    }
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ContentManagement()
        {
            ContentManagement objContentManagement = new ContentManagement();
            objContentManagement = objUserManagementBusiness.FetchResourceCatagory(objContentManagement);
            return View(objContentManagement);

        }
        public ActionResult CreateResource( int ResourceID=0)
        {
            ContentManagement objContentManagement = new ContentManagement();
            objContentManagement.ResourceID = ResourceID;
            objContentManagement = objUserManagementBusiness.FetchResourceCatagoryDetails(objContentManagement);
            return View(objContentManagement);
        }
        public ActionResult AddResourceContent(ContentManagement objContentManagement)
        {
            ModelState.Clear();
            objContentManagement = objUserManagementBusiness.AddResourceCatagoryDetails(objContentManagement);            
            return PartialView("_PartialResourceDetails", objContentManagement);
        }
        public ActionResult EditResourceContent(ContentManagement objContentManagement)
        {
            ModelState.Clear();
            objContentManagement = objUserManagementBusiness.EditResourceContent(objContentManagement);
            return PartialView("_PartialCreateResourceDetails", objContentManagement);
        }
        public ActionResult DeleteResourceDetails(ContentManagement objContentManagement)
        {
            //ContentManagement objContentManagement = new ContentManagement();
            //objContentManagement.ResourceChildID = ResouceChildID;
            objContentManagement = objUserManagementBusiness.DeleteResourceDetails(objContentManagement);
            return PartialView("_PartialResourceDetails", objContentManagement);
        }
        public ActionResult DeleteResource(int ResourceID)
        {
            ContentManagement objContentManagement = new ContentManagement();
            objContentManagement.ResourceID = ResourceID;
            objContentManagement = objUserManagementBusiness.DeleteResource(objContentManagement);
            return View("ContentManagement", objContentManagement);
        }
        public static bool IsAllowedFileType(Stream allBytes)
        {
            //BinaryReader reader = new BinaryReader(new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            BinaryReader reader = new BinaryReader(allBytes);
            reader.BaseStream.Position = 0x0; // The offset you are reading the data from  
            byte[] data = reader.ReadBytes(0x10); // Read 16 bytes into an array  
            string data_as_hex = BitConverter.ToString(data);
            //reader.Close();

            // substring to select first 11 characters from hexadecimal array  
            string signature = data_as_hex.Substring(0, 11);

            bool isAllowed = false;
            var AllowedFileTypes = new string[]//https://asecuritysite.com/forensics/magic
            {
                "25-50-44-46",//pdf
                "D0-CF-11-E0",//doc xls ppt vsd
                "D0-CF-11-E0-A1-B1-1A-E1",//doc xls ppt vsd
                "50-4B-03-04",//docx xlsx pptx
				"89-50-4E-47",//png
                "FF D8 FF E0","FF D8 FF E1","FF D8 FF E8",//jpeg jpg 
            };
            isAllowed = AllowedFileTypes.Contains(signature);

            return isAllowed;
        }
        public string DocumentUploadContent(HttpPostedFileBase file, string FileName = null)
        {
            string fileName = string.Empty;
            try
            {
                if (file != null)
                {
                    string directryPath = Server.MapPath("~/ContentUpload/");
                    if (!Directory.Exists(directryPath))
                    {
                        Directory.CreateDirectory(directryPath);
                    }
                    fileName = Path.GetFileName(file.FileName);
                    if (FileName != null) fileName = FileName;
                    var filename = Path.Combine(directryPath, fileName);
                    file.SaveAs(filename);
                }

            }
            catch (Exception ex)
            {
               
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator";
            }
            return fileName;
        }
        public ActionResult FileUpload()
        {
            try
            {
                string Listfiles = string.Empty;
                string FileName = string.Empty;
                string StrWriteException = string.Empty;
                string[] FileExtentions = { ".pdf", ".png", ".jpeg" };
                for(int i=0;i< Request.Files.Count;i++)
                {
                    if (Request.Files[i] != null)
                    {
                        FileName = Request.Files[i].FileName;
                        FileName = Path.GetFileName(FileName);
                        var FileStreamBytes = Request.Files[i].InputStream;
                 //       var IsAllowed = IsAllowedFileType(FileStreamBytes);
                        HttpPostedFileBase file = Request.Files[i];
                        var ext = Path.GetExtension(FileName);


                        //if (file.ContentLength <= 1000000 && IsAllowed)
                        //{
                        //    FileName = DocumentUploadContent(Request.Files[i], FileName);
                        //}
                        //else
                        //{
                            //if (!IsAllowed)
                            //{
                            //    StrWriteException = "Please upload valid file format (png, jpeg, pdf)";
                            //}
                            //else
                            //{
                            //    StrWriteException = "File size should not exceed 1Mb";
                            //}

                      //  }


                        string directryPath = Server.MapPath("~/ContentUpload/");
                    }
                    if (string.IsNullOrEmpty(Listfiles))
                    {
                        Listfiles = FileName;
                    }
                    else {
                        Listfiles += "||" + FileName;
                    }
                }
                if (string.IsNullOrEmpty(StrWriteException))
                {
                    return Json("Sucess||" + Listfiles, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failure||" + StrWriteException, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {                
            }
            return Json("Error while upload", JsonRequestBehavior.AllowGet);

        }
        public ActionResult LoadKnowledge()
        {
            ResouceManagent objResouceManagent = new ResouceManagent();
            objResouceManagent = objUserManagementBusiness.ContentList(objResouceManagent);
            return PartialView("LoadKnowledge", objResouceManagent);
        }
        public ActionResult LoadKnowledgePartial(int ResoucecatagoryPK)
        {
            ModelState.Clear();
            ResouceManagent objResouceManagent = new ResouceManagent();
            objResouceManagent = objUserManagementBusiness.ContentList(objResouceManagent);
            ResourceCatagory SelectedResourceCatagory = objResouceManagent.lstResourceCatagory.Where(x => x.ResoucecatagoryPK == ResoucecatagoryPK).FirstOrDefault();
            return PartialView("_PartialKnowledgeCenterDetails", SelectedResourceCatagory);
        }
        //_PartialKnowledgeCenterDetails
    }
}