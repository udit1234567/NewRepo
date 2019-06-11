using AIA.Life.Repository.AIAEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using AIA.Life.Models.EmailSMSDetails;

namespace AIA.Life.Integration.Services.EmailandSMS
{
    public class EmailIntegration
    {
        AVOAIALifeEntities Entities = new AVOAIALifeEntities();
        public bool EmailNotification(EmailDetails emailObj, bool IsQuotaionPool = false)
        {

            bool Res = true;
            if (!String.IsNullOrEmpty(emailObj.EmailID))
            {
                SmtpClient objClient = new SmtpClient();
                MailMessage message = new MailMessage();
                string fromEmailAddress = string.Empty;
                bool bImgReg = true;
                string FilePath = string.Empty;
                FilePath = Convert.ToString(ConfigurationManager.AppSettings["EmailTempleteImage"]);

                var credential = new NetworkCredential
                {
                    UserName = ConfigurationManager.AppSettings["LocalfromEmailAddress"],
                    Password = ConfigurationManager.AppSettings["LOcalEmailPassword"]
                };

                objClient.Credentials = credential;
                objClient.Host = ConfigurationManager.AppSettings["SmtpHost"];
                objClient.Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
                objClient.EnableSsl = true;

                message.From = new MailAddress(ConfigurationManager.AppSettings["LocalfromEmailAddress"]);

                message.To.Add(emailObj.EmailID);


                message.Subject = emailObj.Subject;
                string generatedMail = "";
                message.IsBodyHtml = true;

                #region Template Related Code
                switch (emailObj.MailTemplate)
                {
                    case "T001":
                        generatedMail = EmailIntegrationTemplete.T001.Replace("txtSurName", emailObj.Salutation);
                        generatedMail = generatedMail.Replace("txtName", emailObj.Name);
                        generatedMail = generatedMail.Replace("txtWPMobileNo", emailObj.WPMobileNo);
                        generatedMail = generatedMail.Replace("txtImage1", FilePath);
                        generatedMail = generatedMail.Replace("txtImage2", FilePath);
                        generatedMail = generatedMail.Replace("txtEnvronment", emailObj.Environment);
                        bImgReg = true;

                        if (emailObj.ByteArray != null)
                        {
                            string SuccessFile = "Retirement Calculator.pdf";
                            Attachment attach_Success = new Attachment(new MemoryStream(emailObj.ByteArray), SuccessFile);
                            message.Attachments.Add(attach_Success);
                        }
                        if (emailObj.ByteArray2 != null)
                        {
                            string SuccessFile = "Health Calculator.pdf";
                            Attachment attach_Success2 = new Attachment(new MemoryStream(emailObj.ByteArray2), SuccessFile);
                            message.Attachments.Add(attach_Success2);
                        }
                        if (emailObj.ByteArray3 != null)
                        {
                            string SuccessFile = "Education Calculator.pdf";
                            Attachment attach_Success3 = new Attachment(new MemoryStream(emailObj.ByteArray3), SuccessFile);
                            message.Attachments.Add(attach_Success3);
                        }
                        if (emailObj.ByteArray4 != null)
                        {
                            string SuccessFile = "Savings Calculator.pdf";
                            Attachment attach_Success4 = new Attachment(new MemoryStream(emailObj.ByteArray4), SuccessFile);
                            message.Attachments.Add(attach_Success4);
                        }
                        if (emailObj.ByteArray5 != null)
                        {
                            string SuccessFile = "Human Value Calculator.pdf";
                            Attachment attach_Success5 = new Attachment(new MemoryStream(emailObj.ByteArray5), SuccessFile);
                            message.Attachments.Add(attach_Success5);
                        }
                        if (emailObj.ByteArray6 != null)
                        {
                            string SuccessFile = "Comprehensive Financial Need Analysis.pdf";
                            Attachment attach_Success6 = new Attachment(new MemoryStream(emailObj.ByteArray6), SuccessFile);
                            message.Attachments.Add(attach_Success6);
                        }
                        if (emailObj.ByteArray8 != null)
                        {
                            string SuccessFile = "Financial Need Analysis.pdf";
                            Attachment attach_Success7 = new Attachment(new MemoryStream(emailObj.ByteArray8), SuccessFile);
                            message.Attachments.Add(attach_Success7);
                        }
                        break;
                    case "T002":
                        if (IsQuotaionPool)
                        {
                            generatedMail = EmailIntegrationTemplete.T002.Remove(0);
                            generatedMail = "PFA." + System.Environment.NewLine + "\n" + "<br/>";
                            string SuccessFileName = "" + emailObj.QuoteNumber + "_" + DateTime.Now.Ticks + ".pdf";
                            Attachment att_Success = new Attachment(new MemoryStream(emailObj.ByteArray), SuccessFileName);
                            message.Attachments.Add(att_Success);
                            if (!string.IsNullOrEmpty(emailObj.AgentEmailID))
                                message.To.Add(emailObj.AgentEmailID);
                        }
                        else
                        {
                            generatedMail = EmailIntegrationTemplete.T002.Replace("txtSurName", emailObj.Salutation);
                            generatedMail = generatedMail.Replace("txtName", emailObj.Name);
                            generatedMail = generatedMail.Replace("txtWPMobileNo", emailObj.WPMobileNo);
                            generatedMail = generatedMail.Replace("txtQuoteNo", emailObj.QuoteNumber);
                            generatedMail = generatedMail.Replace("txtProductName", emailObj.ProductName);
                            generatedMail = generatedMail.Replace("txtPremium", emailObj.Premium);
                            generatedMail = generatedMail.Replace("txtPolicyTerm", emailObj.PolicyTerm);
                            generatedMail = generatedMail.Replace("txtPPTerm", emailObj.PremiumPayingTerm);
                            generatedMail = generatedMail.Replace("txtImage1", FilePath);
                            generatedMail = generatedMail.Replace("txtImage2", FilePath);
                            generatedMail = generatedMail.Replace("txtEnvronment", emailObj.Environment);
                            bImgReg = true;

                            string SuccessFileName = "Life Insurance Quotation - " + emailObj.QuoteNumber +".pdf";
                            Attachment att_Success = new Attachment(new MemoryStream(emailObj.ByteArray), SuccessFileName);
                            message.Attachments.Add(att_Success);
                            if (!string.IsNullOrEmpty(emailObj.AgentEmailID))
                                message.To.Add(emailObj.AgentEmailID);
                        }

                        break;
                    case "T003":
                        generatedMail = EmailIntegrationTemplete.T003.Replace("txtSurName", emailObj.Salutation);
                        generatedMail = generatedMail.Replace("txtName", emailObj.Name);
                        generatedMail = generatedMail.Replace("txtQuoteNo", emailObj.QuoteNumber);
                        generatedMail = generatedMail.Replace("txtProposalNo", emailObj.ProposalNo);
                        generatedMail = generatedMail.Replace("txtProductName", emailObj.ProductName);
                        generatedMail = generatedMail.Replace("txtPremium", emailObj.Premium);
                        generatedMail = generatedMail.Replace("txtImage1", FilePath);
                        generatedMail = generatedMail.Replace("txtImage2", FilePath);
                        generatedMail = generatedMail.Replace("txtEnvronment", emailObj.Environment);
                        bImgReg = true;

                        string SuccessFileName1 = "Success_" + emailObj.ProposalNo + "_" + DateTime.Now.Ticks + ".pdf";
                        Attachment att_Success1 = new Attachment(new MemoryStream(emailObj.ByteArray), SuccessFileName1);
                        message.Attachments.Add(att_Success1);

                        break;
                    case "T004":
                        generatedMail = EmailIntegrationTemplete.T004.Replace("txtSurName", emailObj.Salutation);
                        generatedMail = generatedMail.Replace("txtName", emailObj.Name);
                        generatedMail = generatedMail.Replace("txtQuoteNo", emailObj.QuoteNumber);
                        generatedMail = generatedMail.Replace("txtProductName", emailObj.ProductName);
                        generatedMail = generatedMail.Replace("txtPremium", emailObj.Premium);
                        generatedMail = generatedMail.Replace("txtPolicyStartDate", emailObj.PolicyStartDate);
                        generatedMail = generatedMail.Replace("txtPolicyEndDate", emailObj.PolicyEndDate);
                        generatedMail = generatedMail.Replace("txtImage1", FilePath);
                        generatedMail = generatedMail.Replace("txtImage2", FilePath);
                        generatedMail = generatedMail.Replace("txtEnvronment", emailObj.Environment);
                        bImgReg = true;
                        break;
                    case "T005":
                        generatedMail = EmailIntegrationTemplete.T005.Replace("txtSurName", emailObj.Salutation);
                        generatedMail = generatedMail.Replace("txtName", emailObj.Name);
                        generatedMail = generatedMail.Replace("txtWPMobileNo", emailObj.WPMobileNo);
                        generatedMail = generatedMail.Replace("txtQuoteNo", emailObj.QuoteNumber);
                        generatedMail = generatedMail.Replace("txtProductName", emailObj.ProductName);
                        generatedMail = generatedMail.Replace("txtPremium", emailObj.Premium);
                        generatedMail = generatedMail.Replace("txtPolicyStartDate", emailObj.PolicyStartDate);
                        generatedMail = generatedMail.Replace("txtPolicyEndDate", emailObj.PolicyEndDate);
                        generatedMail = generatedMail.Replace("txtImage1", FilePath);
                        generatedMail = generatedMail.Replace("txtImage2", FilePath);
                        generatedMail = generatedMail.Replace("txtEnvronment", emailObj.Environment);
                        generatedMail = generatedMail.Replace("strQuotesTable", emailObj.TableQuotes);
                        generatedMail = generatedMail.Replace("strQuotesNonMedicalTable", emailObj.TableNonMedicalQuotes);
                        bImgReg = true;
                        if (!string.IsNullOrEmpty(emailObj.AgentEmailID))
                            message.To.Add(emailObj.AgentEmailID);
                        break;
                    case "T006":
                        generatedMail = EmailIntegrationTemplete.T006.Replace("txtSurName", emailObj.Salutation);
                        generatedMail = generatedMail.Replace("txtName", emailObj.Name);
                        generatedMail = generatedMail.Replace("txtWPMobileNo", emailObj.WPMobileNo);
                        generatedMail = generatedMail.Replace("txtImage1", FilePath);
                        generatedMail = generatedMail.Replace("txtImage2", FilePath);
                        generatedMail = generatedMail.Replace("txtEnvronment", emailObj.Environment);
                        bImgReg = true;
                        if (!string.IsNullOrEmpty(emailObj.AgentEmailID))
                            message.To.Add(emailObj.AgentEmailID);
                        break;

                    case "T007":
                        generatedMail = EmailIntegrationTemplete.T007.Replace("txtSurName", emailObj.Salutation);
                        generatedMail = generatedMail.Replace("txtName", emailObj.Name);
                        generatedMail = generatedMail.Replace("txtdeclinedecision", emailObj.UWDeclineDecision);
                        generatedMail = generatedMail.Replace("txtImage1", FilePath);
                        generatedMail = generatedMail.Replace("txtImage2", FilePath);
                        generatedMail = generatedMail.Replace("txtEnvronment", emailObj.Environment);
                        bImgReg = true;
                        if (!string.IsNullOrEmpty(emailObj.AgentEmailID))
                            message.To.Add(emailObj.AgentEmailID);
                        string FileName = "Decline Letter.pdf";
                        Attachment att = new Attachment(new MemoryStream(emailObj.ByteArray), FileName);
                        message.Attachments.Add(att);

                        // Decline
                        break;
                    case "T008":
                        generatedMail = EmailIntegrationTemplete.T008.Replace("txtSurName", emailObj.Salutation);
                        generatedMail = generatedMail.Replace("txtName", emailObj.Name);
                        generatedMail = generatedMail.Replace("txtQuoteNo", emailObj.QuoteNumber);
                        generatedMail = generatedMail.Replace("txtProductName", emailObj.ProductName);
                        generatedMail = generatedMail.Replace("txtDuration", emailObj.Duration);
                        generatedMail = generatedMail.Replace("txtImage1", FilePath);
                        generatedMail = generatedMail.Replace("txtImage2", FilePath);
                        generatedMail = generatedMail.Replace("txtEnvronment", emailObj.Environment);
                        generatedMail = generatedMail.Replace("txtdeclinedecision", emailObj.UWDeclineDecision);
                        if (!string.IsNullOrEmpty(emailObj.AgentEmailID))
                            message.To.Add(emailObj.AgentEmailID);
                        bImgReg = true;
                        break;
                    case "T009":
                        generatedMail = EmailIntegrationTemplete.T009.Replace("txtSurName", emailObj.Salutation);
                        generatedMail = generatedMail.Replace("txtName", emailObj.Name);
                        generatedMail = generatedMail.Replace("txtWPMobileNo", emailObj.WPMobileNo);
                        generatedMail = generatedMail.Replace("txtProposalNo", emailObj.ProposalNo);
                        generatedMail = generatedMail.Replace("txtPolocyNo", emailObj.PolicyNumber);
                        generatedMail = generatedMail.Replace("txtQuoteNo", emailObj.QuoteNumber);
                        generatedMail = generatedMail.Replace("txtProductName", emailObj.ProductName);
                        generatedMail = generatedMail.Replace("txtImage1", FilePath);
                        generatedMail = generatedMail.Replace("txtImage2", FilePath);
                        generatedMail = generatedMail.Replace("txtEnvronment", emailObj.Environment);
                        message.To.Add(emailObj.AgentEmailID);
                        bImgReg = true;
                        break;
                    case "T010":
                        generatedMail = EmailIntegrationTemplete.T010.Replace("txtSurName", emailObj.Salutation);
                        generatedMail = generatedMail.Replace("txtName", emailObj.Name);
                        generatedMail = generatedMail.Replace("txtImage1", FilePath);
                        generatedMail = generatedMail.Replace("txtImage2", FilePath);
                        generatedMail = generatedMail.Replace("txtEnvronment", emailObj.Environment);
                        if (!string.IsNullOrEmpty(emailObj.AgentEmailID))
                            message.To.Add(emailObj.AgentEmailID);
                        bImgReg = true;
                        switch (emailObj.ProductName)
                        {
                            case "Easy Pensions":
                                string filename = "EasyPensionsEnglishPolicy.pdf";
                                string path = ConfigurationManager.AppSettings["Path"].ToString();
                                string filepath = System.IO.Path.Combine(path, filename);
                                emailObj.ByteArray5 = System.IO.File.ReadAllBytes(filepath);
                                break;
                            case "Health Protector":
                                string filename1 = "HealthProtectorNewVersion.pdf";
                                string path1 = ConfigurationManager.AppSettings["Path"].ToString();
                                string filepath1 = System.IO.Path.Combine(path1, filename1);
                                emailObj.ByteArray5 = System.IO.File.ReadAllBytes(filepath1);
                                break;
                            case "Smart Builder Gold":
                                string filename2 = "SmartBuilderGoldEnglishPolicy.pdf";
                                string path2 = ConfigurationManager.AppSettings["Path"].ToString();
                                string filepath2 = System.IO.Path.Combine(path2, filename2);
                                emailObj.ByteArray5 = System.IO.File.ReadAllBytes(filepath2);
                                break;
                            case "Smart Builder":
                                string filename3 = "SmartBuilderEnglishBook.pdf";
                                string path3 = ConfigurationManager.AppSettings["Path"].ToString();
                                string filepath3 = System.IO.Path.Combine(path3, filename3);
                                emailObj.ByteArray5 = System.IO.File.ReadAllBytes(filepath3);
                                break;
                            case "Education Plan":
                                string filename5 = "EducationPlanEnglishPolicy.pdf";
                                string path5 = ConfigurationManager.AppSettings["Path"].ToString();
                                string filepath5 = System.IO.Path.Combine(path5, filename5);
                                emailObj.ByteArray5 = System.IO.File.ReadAllBytes(filepath5);
                                break;
                            default:
                                string filename4 = "SavingsPlusGoldPolicy.pdf";
                                string path4 = ConfigurationManager.AppSettings["Path"].ToString();
                                string filepath4 = System.IO.Path.Combine(path4, filename4);
                                emailObj.ByteArray5 = System.IO.File.ReadAllBytes(filepath4);
                                break;
                        }
                        if (emailObj.ByteArray2 != null)
                        {
                            string File2 = "Policy Schedule.pdf";
                            Attachment attach_File2 = new Attachment(new MemoryStream(emailObj.ByteArray2), File2);
                            message.Attachments.Add(attach_File2);
                        }
                        if (emailObj.ByteArray3 != null)
                        {
                            string File3 = "Proposal Form.pdf";
                            Attachment attach_File3 = new Attachment(new MemoryStream(emailObj.ByteArray3), File3);
                            message.Attachments.Add(attach_File3);

                        }
                        if (emailObj.ByteArray5 != null)
                        {
                            string File5 = "Policy Document.pdf";
                            Attachment attach_File5 = new Attachment(new MemoryStream(emailObj.ByteArray5), File5);
                            message.Attachments.Add(attach_File5);
                        }
                        //string File6 = emailObj.PolicyNumber + ".pdf";
                        //Attachment attach_File6 = new Attachment(new MemoryStream(emailObj.ByteArray6), File6);
                        //message.Attachments.Add(attach_File6);
                        break;
                    case "T011":
                        generatedMail = EmailIntegrationTemplete.T011.Replace("txtSurName", emailObj.Salutation);
                        generatedMail = generatedMail.Replace("txtName", emailObj.Name);
                        generatedMail = generatedMail.Replace("txtImage1", FilePath);
                        generatedMail = generatedMail.Replace("txtImage2", FilePath);
                        generatedMail = generatedMail.Replace("txtEnvronment", emailObj.Environment);
                        if (!string.IsNullOrEmpty(emailObj.AgentEmailID))
                            message.To.Add(emailObj.AgentEmailID);
                        string OnlyCLALetter = "Conditional Letter of Acceptance.pdf";
                        Attachment OnlyCLALetterAttachment = new Attachment(new MemoryStream(emailObj.ByteArray), OnlyCLALetter);
                        message.Attachments.Add(OnlyCLALetterAttachment);
                        break;
                    case "T012":
                        generatedMail = EmailIntegrationTemplete.T012.Replace("txtSurName", emailObj.Salutation);
                        generatedMail = generatedMail.Replace("txtName", emailObj.Name);
                        generatedMail = generatedMail.Replace("txtImage1", FilePath);
                        generatedMail = generatedMail.Replace("txtImage2", FilePath);
                        generatedMail = generatedMail.Replace("txtEnvronment", emailObj.Environment);
                        if (!string.IsNullOrEmpty(emailObj.AgentEmailID))
                            message.To.Add(emailObj.AgentEmailID);
                        string CLALetter = "Conditional Letter of Acceptance.pdf";
                        Attachment CLALetterAttachment = new Attachment(new MemoryStream(emailObj.ByteArray), CLALetter);
                        message.Attachments.Add(CLALetterAttachment);
                        if (emailObj.ByteArray2 != null)
                        {
                            string Illustration = "CLA Illustration.pdf";
                            Attachment IllustrationAttachment = new Attachment(new MemoryStream(emailObj.ByteArray2), Illustration);
                            message.Attachments.Add(IllustrationAttachment);
                        }
                        break;
                    case "T013":
                        generatedMail = EmailIntegrationTemplete.T013.Replace("txtSurName", emailObj.Salutation);
                        generatedMail = generatedMail.Replace("txtName", emailObj.Name);
                        generatedMail = generatedMail.Replace("txtEnvronment", emailObj.Environment);
                        if (emailObj.AgentEmailID != null)
                        {
                            message.To.Add(emailObj.AgentEmailID);
                        }
                        if (emailObj.ByteArray != null)
                        {
                            string QuoteAttachment = "Life Insurance Quotation - "+ emailObj.QuoteNumber+".pdf";
                            Attachment QuoteAttachmentFile = new Attachment(new MemoryStream(emailObj.ByteArray), QuoteAttachment);
                            message.Attachments.Add(QuoteAttachmentFile);
                        }
                        if (emailObj.ByteArray2 != null)
                        {
                            string CLALetterQuote = "Conditional Letter of Acceptance.pdf";
                            Attachment CLALetterQuoteAttachment = new Attachment(new MemoryStream(emailObj.ByteArray2), CLALetterQuote);
                            message.Attachments.Add(CLALetterQuoteAttachment);
                        }
                        break;
                    case "T014":
                        generatedMail = EmailIntegrationTemplete.T014.Replace("txtSurName", emailObj.Salutation);
                        generatedMail = generatedMail.Replace("txtName", emailObj.Name);
                        generatedMail = generatedMail.Replace("txtImage1", FilePath);
                        generatedMail = generatedMail.Replace("txtImage2", FilePath);
                        generatedMail = generatedMail.Replace("txtRequirement", emailObj.Req1);
                        generatedMail = generatedMail.Replace("txtEnvronment", emailObj.Environment);

                        break;
                    case "T015":
                        generatedMail = EmailIntegrationTemplete.T015.Replace("txtSurName", emailObj.Salutation);
                        generatedMail = generatedMail.Replace("txtName", emailObj.Name);
                        generatedMail = generatedMail.Replace("strQuotesTable", emailObj.TableQuotes);
                        generatedMail = generatedMail.Replace("strQuotesNonMedicalTable", emailObj.TableNonMedicalQuotes);
                        generatedMail = generatedMail.Replace("txtImage1", FilePath);
                        generatedMail = generatedMail.Replace("txtImage2", FilePath);
                        generatedMail = generatedMail.Replace("txtEnvronment", emailObj.Environment);
                        if (!string.IsNullOrEmpty(emailObj.AgentEmailID))
                            message.To.Add(emailObj.AgentEmailID);
                        bImgReg = true;
                        break;
                    case "T016":
                        generatedMail = EmailIntegrationTemplete.T016.Replace("txtSurName", emailObj.Salutation);
                        generatedMail = generatedMail.Replace("txtName", emailObj.Name);
                        generatedMail = generatedMail.Replace("txtImage1", FilePath);
                        generatedMail = generatedMail.Replace("txtImage2", FilePath);
                        generatedMail = generatedMail.Replace("txtEnvronment", emailObj.Environment);
                        if (!string.IsNullOrEmpty(emailObj.AgentEmailID))
                            message.To.Add(emailObj.AgentEmailID);
                        bImgReg = true;
                        if (emailObj.ByteArray != null)
                        {
                            string SuccessFile = "Medical Letter.pdf";
                            Attachment attach_Success = new Attachment(new MemoryStream(emailObj.ByteArray), SuccessFile);
                            message.Attachments.Add(attach_Success);
                        }
                        break;
                }
                #endregion

                message.Body = generatedMail;
                try
                {
                    objClient.Send(message);
                }

                catch (SmtpException ex)
                {
                    throw ex;

                }
            }
            return Res;
        }


    }
}