using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using AIA.Life.Repository.AIAEntity;
using System.Xml;
using Newtonsoft.Json;
using AIA.Life.Models.EmailSMSDetails;
using System.Net.Mail;
using AIA.CrossCutting;

namespace AIA.Life.Integration.Services.EmailandSMS
{
    public class SMSIntegration
    {
        public bool SMSNotification(SMSDetails objsmsDetails)
        {
            bool Res = false;
            try
            {
                string message = string.Empty;
                switch (objsmsDetails.SMSTemplate)
                {
                    case "S001":
                        message = SMSIntegrationTemplete.S001.Replace("txtSalutation", objsmsDetails.Salutation);
                        message = message.Replace("txtSurname", objsmsDetails.Name);
                        message = message.Replace("txtWPMobileNo", objsmsDetails.WPMobileNumber);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);
                        break;
                    case "S002":
                        message = SMSIntegrationTemplete.S002.Replace("txtSalutation", objsmsDetails.Salutation);
                        message = message.Replace("txtSurname", objsmsDetails.Name);
                        message = message.Replace("txtWPMobileNo", objsmsDetails.WPMobileNumber);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);
                        break;
                    case "S003":
                        message = SMSIntegrationTemplete.S003.Replace("txtSalutation", objsmsDetails.Salutation);
                        message = message.Replace("txtSurname", objsmsDetails.Name);
                        message = message.Replace("txtProposalNo", objsmsDetails.ProposalNumber);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);

                        break;
                    case "S004":
                        message = SMSIntegrationTemplete.S004.Replace("txtSalutation", objsmsDetails.Salutation);
                        message = message.Replace("txtSurname", objsmsDetails.Name);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);

                        break;
                    case "S005":
                        message = SMSIntegrationTemplete.S005.Replace("txtProposalNo", objsmsDetails.PolicyNo);

                        break;
                    case "S006":
                        message = SMSIntegrationTemplete.S006.Replace("txtProposalNo", objsmsDetails.PolicyNo);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);

                        break;
                    case "S007":
                        message = SMSIntegrationTemplete.S007.Replace("txtSalutation", objsmsDetails.Salutation);
                        message = message.Replace("txtSurname", objsmsDetails.Name);
                        message = message.Replace("txtProposalNo", objsmsDetails.PolicyNo);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);
                        break;
                    case "S008":
                        message = SMSIntegrationTemplete.S008.Replace("txtSalutation", objsmsDetails.Salutation);
                        message = message.Replace("txtSurname", objsmsDetails.Name);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);
                        break;
                    case "S009":
                        message = SMSIntegrationTemplete.S009.Replace("txtSalutation", objsmsDetails.Salutation);
                        message = message.Replace("txtSurname", objsmsDetails.Name);
                        //message = message.Replace("txtReq1", objsmsDetails.Req1);
                        //message = message.Replace("txtReq2", objsmsDetails.Req2);
                        //message = message.Replace("txtReq3", objsmsDetails.Req3);
                        //message = message.Replace("txtReq4", objsmsDetails.Req4);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);
                        break;
                    case "S010":
                        message = SMSIntegrationTemplete.S010.Replace("txtProposalNo", objsmsDetails.PolicyNo);
                        //message = message.Replace("txtReq1", objsmsDetails.Req1);
                        //message = message.Replace("txtReq2", objsmsDetails.Req2);
                        //message = message.Replace("txtReq3", objsmsDetails.Req3);
                        //message = message.Replace("txtReq4", objsmsDetails.Req4);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);

                        break;
                    case "S011":
                        message = SMSIntegrationTemplete.S011.Replace("txtSalutation", objsmsDetails.Salutation);
                        message = message.Replace("txtSurname", objsmsDetails.Name);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);
                        break;
                    case "S012":
                        message = SMSIntegrationTemplete.S012.Replace("txtProposalNo", objsmsDetails.PolicyNo);
                        message = message.Replace("txtPremium", objsmsDetails.Premium);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);
                        break;
                    case "S013":
                        message = SMSIntegrationTemplete.S013.Replace("txtProposalNo", objsmsDetails.PolicyNo);
                        message = message.Replace("txtProposalEndDate ", objsmsDetails.PolicyEndDate);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);
                        break;
                    case "S014":
                        message = SMSIntegrationTemplete.S014.Replace("txtSalutation", objsmsDetails.Salutation);
                        message = message.Replace("txtSurname", objsmsDetails.Name);
                        message = message.Replace("txtProposalNo", objsmsDetails.PolicyNo);
                        message = message.Replace("txtProposalEndDate ", objsmsDetails.PolicyEndDate);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);
                        break;
                    case "S015":
                        message = SMSIntegrationTemplete.S015.Replace("txtProposalNo", objsmsDetails.PolicyNo);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);
                        message = message.Replace("txtHealthconditionOccupation", objsmsDetails.HealthconditionOccupation);
                        break;
                    case "S016":
                        message = SMSIntegrationTemplete.S016.Replace("txtSalutation", objsmsDetails.Salutation);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);
                        message = message.Replace("txtSurname", objsmsDetails.Name);
                        message = message.Replace("txtHealthconditionOccupation", objsmsDetails.HealthconditionOccupation);
                        break;
                    case "S017":
                        message = SMSIntegrationTemplete.S017.Replace("txtProposalNo", objsmsDetails.PolicyNo);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);
                        message = message.Replace("txtHealthconditionOccupation", objsmsDetails.HealthconditionOccupation);
                        break;
                    case "S018":
                        message = SMSIntegrationTemplete.S018.Replace("txtSalutation", objsmsDetails.Salutation);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);
                        message = message.Replace("txtSurname", objsmsDetails.Name);
                        message=message.Replace("txtHealthconditionOccupation", objsmsDetails.HealthconditionOccupation);
                        message = message.Replace("txtMonths", objsmsDetails.Months);
                        break;
                    case "S019":
                        message = SMSIntegrationTemplete.S019.Replace("txtProposalNo", objsmsDetails.PolicyNo);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);
                        break;
                    case "S020":
                        message = SMSIntegrationTemplete.S020.Replace("txtSalutation", objsmsDetails.Salutation);
                        message = message.Replace("txtSurname", objsmsDetails.Name);
                        message = message.Replace("txtProposalNo", objsmsDetails.PolicyNo);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);
                        break;
                    case "S021":
                        message = SMSIntegrationTemplete.S021.Replace("txtProposalNo", objsmsDetails.PolicyNo);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);
                        break;
                    case "S022":
                        message = SMSIntegrationTemplete.S022.Replace("txtProposalNo", objsmsDetails.PolicyNo);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);
                        break;
                    case "S023":
                        message = SMSIntegrationTemplete.S023.Replace("txtSalutation", objsmsDetails.Salutation);
                        message = message.Replace("txtSurname", objsmsDetails.Name);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);
                        break;
                    case "S024":
                        message = SMSIntegrationTemplete.S024.Replace("txtSalutation", objsmsDetails.Salutation);
                        message = message.Replace("txtSurname", objsmsDetails.Name);
                        message = message.Replace("txtPolicyNo", objsmsDetails.PolicyNo);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);
                        break;
                    case "S025":
                        message = SMSIntegrationTemplete.S025.Replace("txtPolicyNo", objsmsDetails.PolicyNo);
                        message = message.Replace("txtSMSEnvironment", objsmsDetails.SMSEnvironment);
                        break;


                }
               // SamsIntegration.SamsClient samsClient = new SamsIntegration.SamsClient();
               // string send = samsClient.SendMessage(objsmsDetails.MobileNumber, message);
                //if (send == "success")
                //{
                  //  Res = true;
                //}
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Res;

        }

    }
}