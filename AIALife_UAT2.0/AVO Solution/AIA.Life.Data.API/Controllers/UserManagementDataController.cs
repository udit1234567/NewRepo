using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AIA.Life.Data.API.ControllerLogic.Common;
using AIA.Life.Data.API.ControllerLogic.Policy;
using AIA.Life.Data.API.ControllerLogic.UserManagement;
using AIA.Life.Models.UserManagement;
//using AIA.Life.Integration.Services.SamsIntegration;
using AIA.Life.Models.Common;

namespace AIA.Life.Data.API.Controllers
{
    public class UserManagementDataController : ApiController
    {
        UserManagementLogic objUserManagementLogic = new UserManagementLogic();
        public PermissionTree MenuPermissionTree(PermissionTree obj)
        {
            obj.objTree = objUserManagementLogic.MenuPermissionTree("IMD", obj.UserID, "Menu");
            return obj;
        }
        public PermissionTree PaymentPermissionTree(PermissionTree obj)
        {
            obj.objTree = objUserManagementLogic.PaymentPermissionTree("IMD", obj.UserID, "Payment");
            return obj;
        }
        public PermissionTree ProductPermissionTree(PermissionTree obj)
        {
            obj.objTree = objUserManagementLogic.ProductPermissionTree("IMD", obj.UserID, "Product");
            return obj;
        }
        public CreateUserModel LoadCreateUser(CreateUserModel obj)
        {
            if (obj.EditUserID != 0)
            {

                obj = objUserManagementLogic.FetchUserDetails(obj, obj.EditUserID);
            }
            obj.LstUserRole= objUserManagementLogic.GetUserRole();
            obj.LstAuthLimit = objUserManagementLogic.FetchAuthorityLimit();
            obj.lstUserstatus = objUserManagementLogic.GetUserStatus();
            obj.ListImdLevel = objUserManagementLogic.GetIMDLevel();
            // obj.Listchannel = objUserManagementLogic.GetChannel("");
            obj.lstBranchCode = objUserManagementLogic.GetBranchCode();
            obj.lstReceiptingBankCode = objUserManagementLogic.GetReceiptingBankCode();
            // obj.lstAreaCode = objUserManagementLogic.GetAreaCode();
            obj.ListOffice = objUserManagementLogic.GetOffice();
            //obj.ListSalutation = objUserManagementLogic.GetSalutationsList();
            obj.LstGender = objUserManagementLogic.FetchGenders();
            obj.ParentUsers = objUserManagementLogic.FetchParentId();
            obj.secretQuestions = objUserManagementLogic.FetchSecretQuestion();
            
            return obj;
        }
        public CreateUserModel SaveUserAIA(CreateUserModel objCreateUserModel)
        {
            objCreateUserModel = objUserManagementLogic.SaveUserAIA(objCreateUserModel);
            return objCreateUserModel;
        }
        public CreateUserModel CreateRandomPassword(CreateUserModel objCreateUserModel)
        {
            objCreateUserModel.Password = CreateRandomPassword(8);
            return objCreateUserModel;
        }
        public static string CreateRandomPassword(int PasswordLength)
        {
            string chars = UserManagementLogic.Generate(PasswordLength, 8);
            return chars;
        }
        public CreateUserModel FetchPaymentModes(CreateUserModel objCreateUserModel)
        {
            // objCreateUserModel = objUserManagementLogic.FetchPaymentModes(objCreateUserModel);
            return objCreateUserModel;
        }
        public ChangePassword LoadSecretQuestions(ChangePassword objChangePassword)
        {
            objChangePassword.secretQuestions = objUserManagementLogic.FetchSecretQuestion();
            return objChangePassword;
        }
        public CreateUserModel GetAdvisorCodeData(CreateUserModel objCreateUserModel, string AdvisorCode)
        {
            objCreateUserModel = objUserManagementLogic.FetchAdvisorCodeData(AdvisorCode);
            return objCreateUserModel;
        }
        public CreateUserModel SavePermission(CreateUserModel objCreateUserModel)
        {
            string IMEINum = objCreateUserModel.IMEINumber;
            bool SelfInsp = false;
            bool RaiseInsp = false;
            bool Recommendation = false;
            objCreateUserModel.IsMenuPermissionSaved = objUserManagementLogic.SavePermission(objCreateUserModel.PermissionIDs, objCreateUserModel.IndetPerm, objCreateUserModel.RoleName, objCreateUserModel.permissionType, objCreateUserModel.IMEINumber, SelfInsp, RaiseInsp, Recommendation);
            return objCreateUserModel;
        }
        public ChangePassword GetResendOTPInformation(ChangePassword objChangePassword, string UserName)
        {
            objChangePassword = new ChangePassword();
            objChangePassword.Result = objUserManagementLogic.ResendOTPInformation(UserName);
            return objChangePassword;
        }
        public CreateUserModel SaveOTPInformation(CreateUserModel objCreateUser)
        {
            objCreateUser.Status = objUserManagementLogic.SaveOTPInformation(objCreateUser.UserID, objCreateUser.OTP, objCreateUser.UserName);
            return objCreateUser;
        }
        public ChangePassword VerifyOTPInformation(ChangePassword objChangePassword)
        {
            objChangePassword.Result = objUserManagementLogic.VerifyOTPInformation(objChangePassword.OTP, objChangePassword.userName);
            return objChangePassword;
        }
        public ChangePassword Sendmailsmsresetpassword(ChangePassword Password)
        {
            Password.IsStatus = objUserManagementLogic.Sendmailsmsresetpassword(Password.mobileno, Password.emailID);
            return Password;
        }
        public CreateUserModel FetchGridUserDetails(CreateUserModel Createusr)
        {
            Createusr.ListUserData = objUserManagementLogic.FetchGridUserDetails(Createusr);
            return Createusr;
        }
        public ChangePassword UnlockUserOnSuccess(ChangePassword objChangePassword)
        {
            // objChangePassword.Result = objUserManagementLogic.UnlockUserOnSuccess(objChangePassword);
            return objChangePassword;
        }
        public Configuration LoadConfigurationTask(Configuration objConfiguration)
        {
            objConfiguration.ObjConfigurationGridData = objUserManagementLogic.GetConfiguration();

            return objConfiguration;
        }

        public IMOUsers FilltblMasIMOUserDetails(IMOUsers objIMOUsers)
        {
            UserManagementLogic obj = new UserManagementLogic();
            objIMOUsers = obj.FilltblMasIMOUserDetails(objIMOUsers);
            return objIMOUsers;
        }

        //public Login UserLogin(Login objLogin)
        //{
        //    //SamsClient samsClient = new SamsClient();
        //   // AIA.Life.Integration.Services.SamsIntegration.User user = new User();
        //   // user.userName = objLogin.UserId;
        //   // user.loginPassword = objLogin.Password;
        //    //string status = samsClient.AUthenticateImo(user);
        //    //if (status == "Success")
        //   // {
        //     //   UserManagementLogic obj = new UserManagementLogic();
        //       // objLogin = obj.UserLogin(objLogin);
        //   // }
        //    //else
        //     //   objLogin.Message = status;
        //    //return objLogin;
        //}
        public AIA.Life.Models.Policy.TransactLog ValidateUserAuth(AIA.Life.Models.Policy.TransactLog transactLog)
        {
            PolicyLogic objLogic = new PolicyLogic();
            transactLog.Message = objLogic.ValidateAPPServiceAuthenticationDetails(transactLog.SerivceTraceID, transactLog.UserID, transactLog.UserName);
            return transactLog;
        }
        public UserToken GenerateTokenID(UserToken objLogin)
        {
            UserManagementLogic obj = new UserManagementLogic();
            objLogin = obj.ValidateUserLogin(objLogin);           
            return objLogin;
        }
        public CreateUserModel FetchUserDetails(CreateUserModel objuser)
        {
            UserManagementLogic obj = new UserManagementLogic();
            if (objuser.PageName == "UserSearch")
            {
                objuser = obj.FetchUserData(objuser,true);
            }
           else if (objuser.PageName == "Login")
            {
                objuser = obj.FetchUserData(objuser, true);
            }
            else if (objuser.NodeID == decimal.Zero)
            {
                objuser = obj.FetchUserData(objuser);
            }
            else
            {
                objuser = obj.FetchUserDataByNodeID(objuser);
            }
            return objuser;
        }
        public CreateUserModel FetchUserData(CreateUserModel objuser)
        {
            UserManagementLogic obj = new UserManagementLogic();
            
                objuser = obj.FetchUserExistance(objuser);
           
            return objuser;
        }
        
        public CreateUserModel FetchDeviceName(CreateUserModel objuser)
        {
            UserManagementLogic obj = new UserManagementLogic();
            if (objuser.PageName == "UserSearch")
            {
                objuser = obj.FetchDeviceName(objuser, true);
            }
           
            return objuser;
        }
        
        public CreateUserModel UpdateDevicedetails(CreateUserModel objuser)
        {
            UserManagementLogic obj = new UserManagementLogic();
            objuser = obj.UpdateDevicedetails(objuser);
            return objuser;
        }
        public CreateUserModel FetchDeviceHistory(CreateUserModel objuser)
        {
            UserManagementLogic obj = new UserManagementLogic();
            objuser = obj.FetchDeviceHistory(objuser);
            return objuser;
        }
        public AppNotification SaveAppUpdate(AppNotification objAppNotification)
        {
            UserManagementLogic obj = new UserManagementLogic();
            objAppNotification = obj.SaveAppUpdate(objAppNotification);
            return objAppNotification;
        }
        public AppNotification AppUpdateMaster(AppNotification objAppNotification)
        {
            UserManagementLogic obj = new UserManagementLogic();
            objAppNotification.LstRecepient = obj.GetAppUpdateDropdown();
        
            objAppNotification.LstSubRecepient.Add(new MasterListItem { Text = "Data Wipeup", Value = "data wipeup" });
            objAppNotification.LstSubRecepient.Add(new MasterListItem { Text = "App Update", Value = "App Update" });
            return objAppNotification;
        }
        public AppNotification AppNotificationMaster(AppNotification objAppNotification)
        {
            UserManagementLogic obj = new UserManagementLogic();
            objAppNotification.LstRecepient = obj.GetAppNotificationDropdown();

            objAppNotification.LstSubRecepient.Add(new MasterListItem { Text = "Data Wipeup", Value = "data wipeup" });
            objAppNotification.LstSubRecepient.Add(new MasterListItem { Text = "App Notification", Value = "App Notification" });
            return objAppNotification;

            return objAppNotification;
        }
        public AppNotification DeviceToken(AppNotification objAppNotification)
        {
            UserManagementLogic obj = new UserManagementLogic();
            objAppNotification.AllDeviceTokenID = obj.GetDeviceToken(objAppNotification.SelectRecepient);
            return objAppNotification;
        }
        public UserSearch SearchIndividual(UserSearch objUserSearch)
        {
            UserManagementLogic obj = new UserManagementLogic();
            objUserSearch.LstUserSerch = obj.SearchIndividual();
            return objUserSearch;
        }
        public ContentManagement FetchResourceCatagory(ContentManagement objContentManagement)
        {
            UserManagementLogic obj = new UserManagementLogic();
            objContentManagement = obj.FetchResourceCatagory(objContentManagement);
            return objContentManagement;
        }
        public ContentManagement FetchResourceCatagoryDetails(ContentManagement objContentManagement)
        {
            UserManagementLogic obj = new UserManagementLogic();
            objContentManagement = obj.FetchResourceCatagoryMaster(objContentManagement);
            if (objContentManagement.ResourceID != 0)
            {
                objContentManagement = obj.FetchResource(objContentManagement);
            }
            return objContentManagement;
        }
        public ContentManagement AddResourceCatagoryDetails(ContentManagement objContentManagement)
        {
            UserManagementLogic obj = new UserManagementLogic();
            objContentManagement = obj.AddResourceCatagoryDetails(objContentManagement);
            return objContentManagement;
        }
        public ContentManagement RemoveResourceDetails(ContentManagement objContentManagement)
        {
            UserManagementLogic obj = new UserManagementLogic();
            objContentManagement = obj.DeleteResourceDetails(objContentManagement);
            return objContentManagement;
        }
        public ContentManagement DelResource(ContentManagement objContentManagement)
        {
            UserManagementLogic obj = new UserManagementLogic();
            objContentManagement = obj.DeleteResource(objContentManagement);
            return objContentManagement;
        }
        public ContentManagement EditResourceContent(ContentManagement objContentManagement)
        {
            UserManagementLogic obj = new UserManagementLogic();
            objContentManagement = obj.EditResourceContent(objContentManagement);
            objContentManagement = obj.FetchResource(objContentManagement);
            return objContentManagement;
        }
        public ResouceManagent ContentList(ResouceManagent objContentManagement)
        {
            UserManagementLogic obj = new UserManagementLogic();
            objContentManagement = obj.ContentList(objContentManagement);            
            return objContentManagement;
        }      


    }
}