using AIA.Life.Business.UserManagement;
using AIA.Life.Models.UserManagement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;

namespace AIA.Services.API.Controllers.UserManagement
{
    //[Route("AVO")]
    public class UserManagementApiController : ApiController
    {
        UserManagementBusiness objUserManagementBusiness = new UserManagementBusiness();
        public PermissionTree MenuPermissionTree(PermissionTree obj)
        {
            return objUserManagementBusiness.MenuPermissionTree(obj);
        }
        public PermissionTree PaymentPermissionTree(PermissionTree obj)
        {
            return objUserManagementBusiness.PaymentPermissionTree(obj);
        }
        public PermissionTree ProductPermissionTree(PermissionTree obj)
        {
            return objUserManagementBusiness.ProductPermissionTree(obj);
        }
        public CreateUserModel LoadCreateUser(CreateUserModel obj)
        {
            return objUserManagementBusiness.LoadCreateUser(obj);
        }
        public CreateUserModel SaveUserAIA(CreateUserModel objCreateUserModel)
        {
            return objUserManagementBusiness.SaveUserAIA(objCreateUserModel);
        }
        public CreateUserModel CreateRandomPassword(CreateUserModel objCreateUserModel)
        {
            return objUserManagementBusiness.CreateRandomPassword(objCreateUserModel);
        }
        public CreateUserModel FetchPaymentModes(CreateUserModel createusr)
        {
            return objUserManagementBusiness.FetchPaymentModes(createusr);
        }
        public ChangePassword LoadSecretQuestions(ChangePassword objChangePassword)
        {
            return objUserManagementBusiness.LoadSecretQuestions(objChangePassword);
        }
        public CreateUserModel GetAdvisorCodeData(CreateUserModel objCreateUserModel, string AdvisorCode)
        {
            return objUserManagementBusiness.GetAdvisorCodeData(objCreateUserModel, AdvisorCode);
        }
        public CreateUserModel SavePermission(CreateUserModel objCreateUserModel)
        {
            return objUserManagementBusiness.SavePermission(objCreateUserModel);
        }
        public ChangePassword GetResendOTPInformation(ChangePassword objChangePassword, string UserName)
        {
            return objUserManagementBusiness.GetResendOTPInformation(objChangePassword, UserName);
        }
        public CreateUserModel SaveOTPInformation(CreateUserModel objCreateUser)
        {
            return objUserManagementBusiness.SaveOTPInformation(objCreateUser);
        }
        public ChangePassword VerifyOTPInformation(ChangePassword objChangePassword)
        {
            return objUserManagementBusiness.VerifyOTPInformation(objChangePassword);
        }
        public ChangePassword Sendmailsmsresetpassword(ChangePassword Password)
        {
            return objUserManagementBusiness.Sendmailsmsresetpassword(Password);
        }
        public CreateUserModel FetchGridUserDetails(CreateUserModel Createusr)
        {
            return objUserManagementBusiness.FetchGridUserDetails(Createusr);
        }
        public ChangePassword UnlockUserOnSuccess(ChangePassword objChangePassword)
        {
            return objUserManagementBusiness.UnlockUserOnSuccess(objChangePassword);
        }
        public AIA.Life.Models.UserManagement.Configuration LoadConfigurationTask(AIA.Life.Models.UserManagement.Configuration objConfiguration)
        {
            return objUserManagementBusiness.LoadConfigurationTask(objConfiguration);
        }
        [HttpPost]
        public IMOUsers PushUserDetails(IMOUsers objIMOUsers)
        {
            try
            {
                objIMOUsers = objUserManagementBusiness.FilltblMasIMOUserDetails(objIMOUsers);
                return objIMOUsers;
            }
            catch(Exception ex)
            {
                objIMOUsers.StatusCode = "500";
                return objIMOUsers;
            }
        }
        [HttpPost]
        public Login UserLogin(Login objLogin)
        {
                objLogin = objUserManagementBusiness.UserLogin(objLogin);
                return objLogin;
        }
        [HttpPost]
        public UserToken GenerateTokenID(UserToken objLogin)
        {
            objLogin = objUserManagementBusiness.GenerateTokenID(objLogin);
            return objLogin;
        }
        public AIA.Life.Models.Policy.TransactLog ValidateUserAuth(AIA.Life.Models.Policy.TransactLog transactLog)
        {
            return objUserManagementBusiness.ValidateUserAuth(transactLog);
        }
        [HttpPost]
        public IMOUsers PushUWDetails(IMOUsers objIMOUsers)
        {
            try
            {
                objIMOUsers = objUserManagementBusiness.FilltblMasUWUserDetails(objIMOUsers);
                return objIMOUsers;
            }
            catch (Exception ex)
            {
                objIMOUsers.StatusCode = "500";
                return objIMOUsers;
            }
        }
        public CreateUserModel FetchUserDetails(CreateUserModel createusr)
        {
            return objUserManagementBusiness.FetchUserDetails(createusr);
        }
        public CreateUserModel UpdateDevicedetails(CreateUserModel objDevice)
        {
            return objUserManagementBusiness.UpdateDevicedetails(objDevice);
        }
        public CreateUserModel FetchDeviceHistory(CreateUserModel objDevice)
        {
            return objUserManagementBusiness.FetchDeviceHistory(objDevice);
        }
        public string SendNotificationFromFirebaseCloud(AppNotification objAppNotification,bool ISAppUpdate=false)
        {
           // string deviceToken = "cIb5sbjgNo0:APA91bEB18OdXVxW4obSmkgXTKsl84OWg-dSgMVXO3gExgF2iZTVDYiS4axHJaUZgsKKMTN4GyYRfiLQO7-Yd4-oZmWDHeI_IWHmA6HSya6-ajFezXLqRwv22ovmcrgfYDeXsAsbWZpL";
            string serverKey = ConfigurationManager.AppSettings["FCMServerKey"].ToString();
            string senderId = ConfigurationManager.AppSettings["FCMsenderId"].ToString();
            string FCMURL = ConfigurationManager.AppSettings["FCMAPIURL"].ToString();
            string result = string.Empty;
            WebRequest tRequest = WebRequest.Create(FCMURL);
            tRequest.Method = "post";
            tRequest.ContentType = "application/json";
            var objNotification = new
            {
                to = objAppNotification.AllDeviceTokenID,
                data = new
                {
                    status= ISAppUpdate==true?"AppUpdate":"AppNotification",
                    title = "Title",
                    body = objAppNotification.Message,
                }
            };
            string jsonNotificationFormat = Newtonsoft.Json.JsonConvert.SerializeObject(objNotification);

            Byte[] byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);
            tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
            tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
            tRequest.ContentLength = byteArray.Length;
            tRequest.ContentType = "application/json";
            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);

                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {
                            String responseFromFirebaseServer = tReader.ReadToEnd();

                            FCMResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<FCMResponse>(responseFromFirebaseServer);
                            if (response.success == 1)
                            {
                               objAppNotification.Result = "success";
                            }
                            else if (response.failure == 1)
                            {
                                objAppNotification.Result = "Failure";
                            }
                            else
                            {
                                objAppNotification.Result = "Failure";
                            }

                        }
                    }

                }
            }
            return result;
        }
        
        public AppNotification AppUpdateMaster(AppNotification objAppNotification)
        {
            return objUserManagementBusiness.AppUpdateMaster(objAppNotification);
        }
        public AppNotification AppNotificationMaster(AppNotification objAppNotification)
        {
            return objUserManagementBusiness.AppNotificationMaster(objAppNotification);
        }
        public AppNotification SendOnlyNotification(AppNotification objAppNotification)
        {
            if (objAppNotification.SelectRecepient != 2735)
            {           
                objAppNotification = objUserManagementBusiness.GetDeviceToken(objAppNotification);
            }
            objAppNotification.Result = SendNotificationFromFirebaseCloud(objAppNotification);
            return objAppNotification;
        }
        public UserSearch SearchIndividual(UserSearch objUserSearch)
        {
            objUserSearch = objUserManagementBusiness.SearchIndividual(objUserSearch);            
            return objUserSearch;
        }


    }
}