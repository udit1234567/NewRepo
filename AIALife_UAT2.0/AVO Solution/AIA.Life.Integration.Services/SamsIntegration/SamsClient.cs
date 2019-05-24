using AIA.Life.Models.Opportunity;
using AIA.Life.Repository.AIAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Integration.Services.SamsIntegration
{
    public class SamsClient : SamsBase
    {
        public void CreateLead(AVOAIALifeEntities Context, Suspect suspect)
        {
            try
            {
                LeadCreation leadCreation = new LeadCreation();
                leadCreation.AgentCode = Context.tblMasIMOUsers.Where(a => a.UserID == suspect.CreatedBy).Select(a => a.AgentCode).FirstOrDefault();
                leadCreation.AvoLeadNumber = suspect.ContactID.ToString();
                //int Type = Context.tblMasCommonTypes.Where(a => a.Description == suspect.Type).Select(a => a.CommonTypesID).FirstOrDefault();
                int type = Context.tblMasCommonTypes.Where(a => a.Description == suspect.Type && (a.MasterType == "Type" || a.MasterType == "BancaType")).Select(a => a.CommonTypesID).FirstOrDefault();
                if (type == 0)
                {
                    type = Context.tblMasCommonTypes.Where(a => a.Code == suspect.Type && a.MasterType == "Type").Select(a => a.CommonTypesID).FirstOrDefault();
                    if (type == 0)
                        type = Convert.ToInt32(suspect.Type);
                }
                leadCreation.AvoLeadType = Context.tblMasCommonTypes.Where(a => a.CommonTypesID == type && (a.MasterType == "Type" || a.MasterType == "BancaType")).Select(a => a.Code).FirstOrDefault();
                string role = Context.usp_GetCurrentUserRole(leadCreation.AgentCode).FirstOrDefault();
                if (role == "FPC-Banca")
                    leadCreation.Channel = "BA";
                else
                    leadCreation.Channel = "AG";
                if (!string.IsNullOrEmpty(suspect.SamsLeadNumber))
                    leadCreation.Flag = "UPDATE";
                else
                    leadCreation.Flag = "NEW";
                //int titleId = Convert.ToInt32(suspect.Title);
                leadCreation.LeadTitle = Context.tblMasCommonTypes.Where(a => a.Description == suspect.Title && a.MasterType == "Salutation").Select(a => a.Code).FirstOrDefault();
                if (string.IsNullOrEmpty(leadCreation.LeadTitle))
                    leadCreation.LeadTitle = suspect.Title;
                leadCreation.LeadFirstName = suspect.Name;
                leadCreation.LeadLastName = suspect.LastName;
                leadCreation.LeadContactNumber = suspect.Mobile;
                leadCreation.LeadEmail = suspect.Email;
                leadCreation.UserId = suspect.CreatedBy;
                leadCreation.LeadPlace = suspect.Place;
                leadCreation.LeadContactOffice = suspect.Work;
                leadCreation.LeadContactResident = suspect.Home;
                leadCreation.NIC = suspect.NIC;
                leadCreation.LeadIntroducer = string.IsNullOrEmpty(suspect.IntroducerCode) == false ? suspect.IntroducerCode.TrimEnd() : "";
                //AuthenticateSams();
                #region  Log Input 
                tbllogxml objlogxml = new tbllogxml();
                objlogxml.Description = "Sams Lead creation request";
                objlogxml.PolicyID = leadCreation.AvoLeadNumber;
                objlogxml.UserID = suspect.CreatedBy;
                objlogxml.XMlData = Newtonsoft.Json.JsonConvert.SerializeObject(leadCreation);
                objlogxml.CreatedDate = DateTime.Now;
                Context.tbllogxmls.Add(objlogxml);
                #endregion
                LeadResponse leadResponse = new LeadResponse();
                InvokeApi<LeadCreation, LeadResponse>(leadCreation, "sams/", ref leadResponse);
                #region  Log output 
                tbllogxml outlogxml = new tbllogxml();
                outlogxml.Description = "Sams Lead creation response";
                outlogxml.PolicyID = leadCreation.AvoLeadNumber;
                outlogxml.UserID = suspect.CreatedBy;
                outlogxml.XMlData = Newtonsoft.Json.JsonConvert.SerializeObject(leadResponse);
                outlogxml.CreatedDate = DateTime.Now;
                Context.tbllogxmls.Add(outlogxml);
                #endregion
                if (leadResponse.body != null)
                {
                    if (!string.IsNullOrEmpty(leadResponse.body.SamsLeadNumber) && leadResponse.body.SamsLeadNumber != "0")
                        suspect.SamsLeadNumber = leadResponse.body.SamsLeadNumber;
                }
            }
            catch (Exception ex)
            {
                suspect.SamsLeadNumber = "";
            }
        }
        public void UpdateLead(AVOAIALifeEntities Context, Prospect prospect)
        {
            try
            {
                LeadCreation leadCreation = new LeadCreation();
                if (!string.IsNullOrEmpty(prospect.AssignedTo))
                    leadCreation.AgentCode = prospect.AssignedTo;
                else
                    leadCreation.AgentCode = Context.tblMasIMOUsers.Where(a => a.UserID == prospect.UserName).Select(a => a.AgentCode).FirstOrDefault();
                leadCreation.AvoLeadNumber = prospect.ContactID.ToString();
                int Type = Context.tblMasCommonTypes.Where(a => a.Description == prospect.Type).Select(a => a.CommonTypesID).FirstOrDefault();
                if (Type == 0)
                {
                    Type = Convert.ToInt32(prospect.Type);
                }
                leadCreation.AvoLeadType = Context.tblMasCommonTypes.Where(a => a.CommonTypesID == Type && (a.MasterType == "Type" || a.MasterType == "BancaType")).Select(a => a.Code).FirstOrDefault();
                string role = Context.usp_GetCurrentUserRole(leadCreation.AgentCode).FirstOrDefault();
                if (role == "FPC-Banca")
                    leadCreation.Channel = "BA";
                else
                    leadCreation.Channel = "AG";
                if (string.IsNullOrEmpty(prospect.SamsLeadNumber))
                    leadCreation.Flag = "NEW";
                else
                    leadCreation.Flag = "UPDATE";

                //int titleId = Convert.ToInt32(prospect.Title);
                leadCreation.LeadTitle = Context.tblMasCommonTypes.Where(a => a.Description == prospect.Salutation && a.MasterType == "Salutation").Select(a => a.Code).FirstOrDefault();
                if (string.IsNullOrEmpty(leadCreation.LeadTitle))
                    leadCreation.LeadTitle = prospect.Salutation;
                leadCreation.LeadFirstName = prospect.Name;
                leadCreation.LeadLastName = prospect.LastName;
                leadCreation.LeadContactNumber = prospect.Mobile;
                leadCreation.NIC = prospect.NIC;
                leadCreation.LeadEmail = prospect.Email;
                leadCreation.UserId = Context.tblMasIMOUsers.Where(a => a.UserID == prospect.UserName).Select(a => a.AgentCode).FirstOrDefault(); ;
                leadCreation.LeadPlace = prospect.Place;
                leadCreation.LeadContactOffice = prospect.Work;
                leadCreation.LeadContactResident = prospect.Home;
                leadCreation.Address1 = prospect.objAddress.Address1;
                leadCreation.Address2 = prospect.objAddress.Address2;
                leadCreation.Address3 = prospect.objAddress.Address3;
                leadCreation.MaritalStatus = prospect.MaritalStatus;
                leadCreation.Gender = prospect.Gender;
                leadCreation.DOB = Convert.ToDateTime(prospect.DateofBirth).ToString("yyyy-MM-dd");
                leadCreation.SamsLeadNumber = Convert.ToInt32(prospect.SamsLeadNumber);
                leadCreation.LeadIntroducer = string.IsNullOrEmpty(prospect.IntroducerCode) == false ? prospect.IntroducerCode.TrimEnd() : "";
                //leadCreation.AssignedTo = prospect.AssignedTo;
                //AuthenticateSams();
                #region  Log Input 
                tbllogxml objlogxml = new tbllogxml();
                objlogxml.Description = "Sams Lead Update request";
                objlogxml.PolicyID = leadCreation.AvoLeadNumber;
                objlogxml.UserID = prospect.CreatedBy;
                objlogxml.XMlData = Newtonsoft.Json.JsonConvert.SerializeObject(leadCreation);
                objlogxml.CreatedDate = DateTime.Now;
                Context.tbllogxmls.Add(objlogxml);
                Context.SaveChanges();
                #endregion
                LeadResponse leadResponse = new LeadResponse();
                InvokeApi<LeadCreation, LeadResponse>(leadCreation, "sams/", ref leadResponse);
                #region  Log output 
                tbllogxml outlogxml = new tbllogxml();
                outlogxml.Description = "Sams Lead Update response";
                outlogxml.PolicyID = leadCreation.AvoLeadNumber;
                outlogxml.UserID = prospect.CreatedBy;
                outlogxml.XMlData = Newtonsoft.Json.JsonConvert.SerializeObject(leadResponse);
                outlogxml.CreatedDate = DateTime.Now;
                Context.tbllogxmls.Add(outlogxml);
                Context.SaveChanges();
                #endregion
                if (leadResponse.body != null)
                {
                    if (!string.IsNullOrEmpty(leadResponse.body.SamsLeadNumber) && leadResponse.body.SamsLeadNumber != "0")
                        prospect.SamsLeadNumber = leadResponse.body.SamsLeadNumber;
                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(prospect.SamsLeadNumber))
                {
                    prospect.SamsLeadNumber = "";
                }
            }
        }
        public void UpdateLeadStatus(AVOAIALifeEntities Context, int leadNo, int status)
        {
            try
            {
                LeadStatus leadStatus = new LeadStatus();
                leadStatus.samsLeadNumber = leadNo;
                leadStatus.status = status;
                leadStatus.statusDateTime = DateTime.Now.ToString("yyyy-MM-dd h:mm tt");
                #region  Log Input 
                tbllogxml objlogxml = new tbllogxml();
                objlogxml.Description = "Lead status request";
                objlogxml.PolicyID = leadNo.ToString();
                objlogxml.XMlData = Newtonsoft.Json.JsonConvert.SerializeObject(leadStatus);
                objlogxml.CreatedDate = DateTime.Now;
                Context.tbllogxmls.Add(objlogxml);
                #endregion
                LeadResponse leadResponse = new LeadResponse();
                InvokeApi<LeadStatus, LeadResponse>(leadStatus, "sams/update/status", ref leadResponse);
                #region  Log output 
                tbllogxml outlogxml = new tbllogxml();
                outlogxml.Description = "Lead Update Response";
                outlogxml.PolicyID = leadNo.ToString();
                outlogxml.XMlData = Newtonsoft.Json.JsonConvert.SerializeObject(leadResponse);
                outlogxml.CreatedDate = DateTime.Now;
                Context.tbllogxmls.Add(outlogxml);
                #endregion
            }
            catch(Exception ex)
            {

            }
        }
        public string SendMessage(string mobileNo, string message)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            MessageRequest messageRequest = new MessageRequest();
            messageRequest.destination = mobileNo;
            messageRequest.message = message;
            #region  Log Input
            tblLogSM objLogSMS = new tblLogSM();
            objLogSMS.Description = "SMS Request";
            objLogSMS.CreatedDate = DateTime.Now;
            objLogSMS.Message = Newtonsoft.Json.JsonConvert.SerializeObject(messageRequest);
            objLogSMS.Number = mobileNo;
            Context.tblLogSMS.Add(objLogSMS);
            Context.SaveChanges();
            #endregion
            MessageResponse messageResponse = new MessageResponse();
            InvokeApi<MessageRequest, MessageResponse>(messageRequest, "sms-management/save-message", ref messageResponse);
            if (messageResponse.errorCode == "200")
            {
                #region  Log output 
                tblLogSM outobjLogSMS = new tblLogSM();
                outobjLogSMS.Description = "SMS Response";
                outobjLogSMS.CreatedDate = DateTime.Now;
                outobjLogSMS.Message = Newtonsoft.Json.JsonConvert.SerializeObject(messageResponse);
                outobjLogSMS.Number = mobileNo;
                Context.tblLogSMS.Add(outobjLogSMS);
                Context.SaveChanges();
                #endregion
                return "success";
            }
            else
            {
                return messageResponse.errorDescription;
            }
        }
        public string AUthenticateImo(User user)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            #region  Log Input 
            tbllogxml objlogxml = new tbllogxml();
            objlogxml.Description = "Imo Login Request";
            objlogxml.PolicyID = user.userName;
            objlogxml.XMlData = Newtonsoft.Json.JsonConvert.SerializeObject(user);
            objlogxml.CreatedDate = DateTime.Now;
            Context.tbllogxmls.Add(objlogxml);
            Context.SaveChanges();
            #endregion
            Error error = AuthenticateSams(user);
            #region  Log output 
            tbllogxml outlogxml = new tbllogxml();
            outlogxml.Description = "Imo Login Response";
            outlogxml.PolicyID = user.userName;
            outlogxml.XMlData = Newtonsoft.Json.JsonConvert.SerializeObject(error);
            outlogxml.CreatedDate = DateTime.Now;
            Context.tbllogxmls.Add(outlogxml);
            Context.SaveChanges();
            #endregion
            if (error != null)
            {
                switch (error.errorCode)
                {
                    case "200":
                    return "Success";
                    case "401":
                    return "Invalid username or password. Please enter correct credentials";
                    case "402":
                    return "Invalid username or password. Your account will be locked next time";
                    case "403":
                    return "Your iMO account is locked. Please reset the password from iMO and try again";
                    case "405":
                    return "Invalid user account. Please contact the IT HelpDesk";
                    default:
                    return "Invalid username or password. Please enter correct credentials";
                }
            }
            else
                return "Invalid username or password. Please enter correct credentials";
        }
    }
}

