using AIA.Life.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Business.UserManagement
{
    
   public class UserManagementBusiness
    {
        public PermissionTree MenuPermissionTree(PermissionTree obj)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<PermissionTree>(obj, "MenuPermissionTree", "UserManagementData");
            #endregion
            return obj;
        }
        public PermissionTree PaymentPermissionTree(PermissionTree obj)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<PermissionTree>(obj, "PaymentPermissionTree", "UserManagementData");
            #endregion
            return obj;
        }
        public PermissionTree ProductPermissionTree(PermissionTree obj)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<PermissionTree>(obj, "ProductPermissionTree", "UserManagementData");
            #endregion
            return obj;
        }
        public CreateUserModel LoadCreateUser(CreateUserModel obj)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<CreateUserModel>(obj, "LoadCreateUser", "UserManagementData");
            #endregion
            return obj;
        }
        public CreateUserModel SaveUserAIA(CreateUserModel objCreateUserModel)
        {
            #region Call API
            objCreateUserModel = WebApiLogic.GetPostComplexTypeToAPI<CreateUserModel>(objCreateUserModel, "SaveUserAIA", "UserManagementData");
            #endregion
            return objCreateUserModel;
        }
        public CreateUserModel CreateRandomPassword(CreateUserModel objCreateUserModel)
        {
            #region Call API
            objCreateUserModel = WebApiLogic.GetPostComplexTypeToAPI<CreateUserModel>(objCreateUserModel, "CreateRandomPassword", "UserManagementData");
            #endregion
            return objCreateUserModel;
        }
        public CreateUserModel FetchPaymentModes(CreateUserModel createusr)
        {
            #region Call API
            createusr = WebApiLogic.GetPostComplexTypeToAPI<CreateUserModel>(createusr, "FetchPaymentModes", "UserManagementData");
            #endregion
            return createusr;
        }
        public ChangePassword LoadSecretQuestions(ChangePassword objChangePassword)
        {
            #region Call API
            objChangePassword = WebApiLogic.GetPostComplexTypeToAPI<ChangePassword>(objChangePassword, "LoadSecretQuestions", "UserManagementData");
            #endregion
            return objChangePassword;
        }
        public CreateUserModel GetAdvisorCodeData(CreateUserModel objCreateUserModel,string AdvisorCode)
        {
            #region Call API
            objCreateUserModel = WebApiLogic.GetPostParametersToAPI<CreateUserModel>(objCreateUserModel, "UserManagementData", "GetAdvisorCodeData", "AdvisorCode", AdvisorCode);         
            #endregion
            return objCreateUserModel;
        }
        public CreateUserModel SavePermission(CreateUserModel objCreateUserModel)
        {
            #region Call API
            objCreateUserModel = WebApiLogic.GetPostComplexTypeToAPI<CreateUserModel>(objCreateUserModel, "SavePermission", "UserManagementData");            
            #endregion
            return objCreateUserModel;
        }
        public ChangePassword GetResendOTPInformation(ChangePassword objChangePassword,string UserName)
        {
            #region Call API            
            objChangePassword = WebApiLogic.GetPostParametersToAPI<ChangePassword>(objChangePassword, "UserManagementData", "GetResendOTPInformation", "UserName", UserName);
           // UserName = WebApiLogic.GetPostComplexTypeToAPI<ChangePassword>(UserName, "ResendOTPInformation", "UserManagementData");
            #endregion
            return objChangePassword;
        }
        public CreateUserModel SaveOTPInformation(CreateUserModel objCreateUser)
        {
            #region Call API
            objCreateUser = WebApiLogic.GetPostComplexTypeToAPI<CreateUserModel>(objCreateUser, "SaveOTPInformation", "UserManagementData");
            #endregion
            return objCreateUser;
        }
        public ChangePassword VerifyOTPInformation(ChangePassword objChangePassword)
        {
            #region Call API
            objChangePassword = WebApiLogic.GetPostComplexTypeToAPI<ChangePassword>(objChangePassword, "VerifyOTPInformation", "UserManagementData");
            #endregion
            return objChangePassword;
        }
        public ChangePassword Sendmailsmsresetpassword(ChangePassword Password)
        {
            #region Call API
            Password = WebApiLogic.GetPostComplexTypeToAPI<ChangePassword>(Password, "Sendmailsmsresetpassword", "UserManagementData");
            #endregion
            return Password;
        }
        public CreateUserModel FetchGridUserDetails(CreateUserModel Createusr)
        {
            #region Call API
            Createusr = WebApiLogic.GetPostComplexTypeToAPI<CreateUserModel>(Createusr, "FetchGridUserDetails", "UserManagementData");
            #endregion
            return Createusr;
        }
        public ChangePassword UnlockUserOnSuccess(ChangePassword objChangePassword)
        {
            #region Call API
            objChangePassword = WebApiLogic.GetPostComplexTypeToAPI<ChangePassword>(objChangePassword, "UnlockUserOnSuccess", "UserManagementData");
            #endregion
            return objChangePassword;
        }

        public AIA.Life.Models.UserManagement.Configuration LoadConfigurationTask(AIA.Life.Models.UserManagement.Configuration objConfiguration)
        {
            #region Call API
            objConfiguration = WebApiLogic.GetPostComplexTypeToAPI(objConfiguration, "LoadConfigurationTask", "UserManagementData");
            #endregion
            return objConfiguration;
        }
        public IMOUsers FilltblMasIMOUserDetails(IMOUsers objIMOUsers)
        {
            UserManagementBusiness objUserManagementBusiness = new UserManagementBusiness();
            objIMOUsers=objUserManagementBusiness.ValidatetblMasIMOUserDetails(objIMOUsers);
            if (objIMOUsers.ErrorMessage.Count==0)
            {
                #region Call API
                objIMOUsers = WebApiLogic.GetPostComplexTypeToAPI<IMOUsers>(objIMOUsers, "FilltblMasIMOUserDetails", "UserManagementData");
                #endregion
                objIMOUsers.StatusCode = "200";
            }
            else
            {
                objIMOUsers.StatusCode = "600";
            }
            return objIMOUsers;
        } 
        
        public IMOUsers ValidatetblMasIMOUserDetails(IMOUsers objIMOUsers)
        {
            objIMOUsers.ErrorMessage = new List<string>();
            if (string.IsNullOrEmpty(objIMOUsers.UserId))
            {
                objIMOUsers.ErrorMessage.Add("UserId is Mandatory");
            }
            if(string.IsNullOrEmpty(objIMOUsers.UserName))
            {
                objIMOUsers.ErrorMessage.Add("UserName is Mandatory");
            }
            if (string.IsNullOrEmpty(objIMOUsers.AgentCode))
            {
                objIMOUsers.ErrorMessage.Add("AgentCode is Mandatory");
            }
            if (string.IsNullOrEmpty(objIMOUsers.Channel))
            {
                objIMOUsers.ErrorMessage.Add("Channel is Mandatory");
            }
            if (string.IsNullOrEmpty(objIMOUsers.NIC))
            {
                objIMOUsers.ErrorMessage.Add("NIC is Mandatory");
            }
            if (string.IsNullOrEmpty(objIMOUsers.UserRole))
            {
                objIMOUsers.ErrorMessage.Add("UserRole is Mandatory");
            }
            if (string.IsNullOrEmpty(objIMOUsers.Branch))
            {
                objIMOUsers.ErrorMessage.Add("Branch is Mandatory");
            }
            //if (string.IsNullOrEmpty(objIMOUsers.MobNo))
            //{
            //    objIMOUsers.ErrorMessage.Add("Mobile No is Mandatory");
            //}
            if (string.IsNullOrEmpty(objIMOUsers.Email))
            {
                objIMOUsers.ErrorMessage.Add("Email is Mandatory");
            }
        
            if (string.IsNullOrEmpty(objIMOUsers.Gender))
            {
                objIMOUsers.ErrorMessage.Add("Gender Is Mandatory");
            }
            return objIMOUsers;
        }
        public Login UserLogin(Login objLogin)
        {
           
            if (objLogin.UserId != "")
            {
                #region Call API
                objLogin = WebApiLogic.GetPostComplexTypeToAPI<Login>(objLogin, "UserLogin", "UserManagementData");
                #endregion
            
            }
           
            return objLogin;
        }
        public AIA.Life.Models.Policy.TransactLog ValidateUserAuth(Models.Policy.TransactLog transactLog)
        {
            #region Call API
            transactLog = WebApiLogic.GetPostComplexTypeToAPI<Models.Policy.TransactLog>(transactLog, "ValidateUserAuth", "UserManagementData");
            #endregion
            return transactLog;
        }
        public UserToken GenerateTokenID(UserToken objLogin)
        {           
                #region Call API
                objLogin = WebApiLogic.GetPostComplexTypeToAPI<UserToken>(objLogin, "GenerateTokenID", "UserManagementData");
                #endregion           

            return objLogin;
        }
        public IMOUsers FilltblMasUWUserDetails(IMOUsers objIMOUsers)
        {           
                #region Call API
                objIMOUsers = WebApiLogic.GetPostComplexTypeToAPI<IMOUsers>(objIMOUsers, "FilltblMasIMOUserDetails", "UserManagementData");
                #endregion
               
            
            return objIMOUsers;
        }
        public CreateUserModel FetchUserDetails(CreateUserModel obj)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<CreateUserModel>(obj, "FetchUserDetails", "UserManagementData");
            #endregion            

            return obj;
        }
        public CreateUserModel FetchUserData(CreateUserModel obj)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<CreateUserModel>(obj, "FetchUserData", "UserManagementData");
            #endregion            

            return obj;
        }
        public CreateUserModel FetchDeviceName(CreateUserModel obj)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<CreateUserModel>(obj, "FetchDeviceName", "UserManagementData");
            #endregion            

            return obj;
        }
        
        public CreateUserModel UpdateDevicedetails(CreateUserModel obj)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<CreateUserModel>(obj, "UpdateDevicedetails", "UserManagementData");
            #endregion            

            return obj;
        }
        public CreateUserModel FetchDeviceHistory(CreateUserModel obj)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<CreateUserModel>(obj, "FetchDeviceHistory", "UserManagementData");
            #endregion            

            return obj;
        }
        public AppNotification SaveAppUpdate(AppNotification ObjAppNotification)
        {
            #region Call API
            ObjAppNotification = WebApiLogic.GetPostComplexTypeToAPI<AppNotification>(ObjAppNotification, "SaveAppUpdate", "UserManagementData");
            #endregion            

            return ObjAppNotification;
        }
        public AppNotification AppUpdateMaster(AppNotification ObjAppNotification)
        {
            #region Call API
            ObjAppNotification = WebApiLogic.GetPostComplexTypeToAPI<AppNotification>(ObjAppNotification, "AppUpdateMaster", "UserManagementData");
            #endregion            

            return ObjAppNotification;
        }
        public AppNotification AppNotificationMaster(AppNotification ObjAppNotification)
        {
            #region Call API
            ObjAppNotification = WebApiLogic.GetPostComplexTypeToAPI<AppNotification>(ObjAppNotification, "AppNotificationMaster", "UserManagementData");
            #endregion            

            return ObjAppNotification;
        }
        public AppNotification GetDeviceToken(AppNotification ObjAppNotification)
        {
            #region Call API
            ObjAppNotification = WebApiLogic.GetPostComplexTypeToAPI<AppNotification>(ObjAppNotification, "DeviceToken", "UserManagementData");
            #endregion            

            return ObjAppNotification;
        }
        public UserSearch SearchIndividual(UserSearch ObjUserSearch)
        {
            #region Call API
            ObjUserSearch = WebApiLogic.GetPostComplexTypeToAPI<UserSearch>(ObjUserSearch, "SearchIndividual", "UserManagementData");
            #endregion            
            return ObjUserSearch;
        }
        public string SendNotificationFromFirebaseCloud(AppNotification objAppNotification, bool ISAppUpdate = false)

        {
            // string deviceToken = "cIb5sbjgNo0:APA91bEB18OdXVxW4obSmkgXTKsl84OWg-dSgMVXO3gExgF2iZTVDYiS4axHJaUZgsKKMTN4GyYRfiLQO7-Yd4-oZmWDHeI_IWHmA6HSya6-ajFezXLqRwv22ovmcrgfYDeXsAsbWZpL";
            string serverKey = ConfigurationManager.AppSettings["FCMServerKey"].ToString();
            string senderId = ConfigurationManager.AppSettings["FCMsenderId"].ToString();
            string FCMURL = ConfigurationManager.AppSettings["FCMAPIURL"].ToString();

           
            string result = string.Empty;
            string Status = string.Empty;
            string SubStatus = string.Empty;
            WebRequest tRequest = WebRequest.Create(FCMURL);
            tRequest.Method = "post";
            tRequest.ContentType = "application/json";


            //if (ISAppUpdate)
            if(objAppNotification.SelectSubRecepient == "App Update")
            {
                Status = objAppNotification.SelectSubRecepient;
                SubStatus = "Update available";
            }
            else if (objAppNotification.SelectSubRecepient == "Data Wipeup")
            {
                Status = objAppNotification.SelectSubRecepient;
                SubStatus = "Clear Data";
            }
            else
            {
                Status = "AppNotification";
                SubStatus = "General Notification";
            }

            var objNotification = new
            {
                to = objAppNotification.AllDeviceTokenID,
                priority = "high",
                content_available = true,

                notification = new
                {
                    status = Status,
                    title = Status,
                    subtitle= SubStatus,
                    body =objAppNotification.Message,
                    
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
                            responseFromFirebaseServer = responseFromFirebaseServer.Replace(@"\", "");

                            FCMResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<FCMResponse>(responseFromFirebaseServer);
                            if (response.success == 1)
                            {
                                objAppNotification.Result = "Notification sent Successfully.";
                            }
                            else if (response.failure == 1)
                            {
                                objAppNotification.Result = "Notification sending Failed";
                            }
                            else
                            {
                                objAppNotification.Result = "Notification sending Failed";
                            }

                        }
                    }

                }
            }
            return objAppNotification.Result;
        }
        public ContentManagement FetchResourceCatagory(ContentManagement objContentManagement)
        {
            #region Call API
            objContentManagement = WebApiLogic.GetPostComplexTypeToAPI<ContentManagement>(objContentManagement, "FetchResourceCatagory", "UserManagementData");
            #endregion            

            return objContentManagement;
        }
        public ContentManagement FetchResourceCatagoryDetails(ContentManagement objContentManagement)
        {
            #region Call API
            objContentManagement = WebApiLogic.GetPostComplexTypeToAPI<ContentManagement>(objContentManagement, "FetchResourceCatagoryDetails", "UserManagementData");
            #endregion            

            return objContentManagement;
        }
        public ContentManagement AddResourceCatagoryDetails(ContentManagement objContentManagement)
        {
            #region Call API
            objContentManagement = WebApiLogic.GetPostComplexTypeToAPI<ContentManagement>(objContentManagement, "AddResourceCatagoryDetails", "UserManagementData");
            #endregion            

            return objContentManagement;
        }
        public ContentManagement EditResourceContent(ContentManagement objContentManagement)
        {
            #region Call API
            objContentManagement = WebApiLogic.GetPostComplexTypeToAPI<ContentManagement>(objContentManagement, "EditResourceContent", "UserManagementData");
            #endregion            

            return objContentManagement;
        }
        
        public ContentManagement DeleteResourceDetails(ContentManagement objContentManagement)
        {
            #region Call API
            objContentManagement = WebApiLogic.GetPostComplexTypeToAPI<ContentManagement>(objContentManagement, "RemoveResourceDetails", "UserManagementData");
            #endregion            

            return objContentManagement;
        }
        public ContentManagement DeleteResource(ContentManagement objContentManagement)
        {
            #region Call API
            objContentManagement = WebApiLogic.GetPostComplexTypeToAPI<ContentManagement>(objContentManagement, "DelResource", "UserManagementData");
            #endregion            

            return objContentManagement;
        }
        public ResouceManagent ContentList(ResouceManagent objContentManagement)
        {
            #region Call API
            objContentManagement = WebApiLogic.GetPostComplexTypeToAPI<ResouceManagent>(objContentManagement, "ContentList", "UserManagementData");
            #endregion            

            return objContentManagement;
        }


    }
}
