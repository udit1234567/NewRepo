using AIA.CrossCutting;
using AIA.Life.Data.API.ControllerLogic.Common;
using AIA.Life.Data.API.ControllerLogic.Policy;
//using AIA.Life.Integration.Services.LDMS;
//using AIA.Life.Integration.Services.LifeAsiaIntegration;
using AIA.Life.Models.Common;
using AIA.Life.Models.Opportunity;
using AIA.Life.Models.Payment;
using AIA.Life.Models.Policy;
using AIA.Life.Models.UWDecision;
using AIA.Life.Repository.AIAEntity;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Google.Apis.Services;
using Google.Apis.Vision.v1;
using Google.Apis.Vision.v1.Data;
using Grpc.Auth;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AIA.Life.Data.API.Controllers
{
    public class PolicyController : ApiController
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        public AIA.Life.Models.Policy.Policy OccupationQuestions(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic obj = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            objPolicy = obj.OccupationQuestions(objPolicy);
            return objPolicy;
        }
        public AIA.Life.Models.Policy.Policy LoadMastersRelationship(AIA.Life.Models.Policy.Policy objPolicy)
        {
            CommonBusiness ObjCommonBusiness = new CommonBusiness();
            return ObjCommonBusiness.LoadMastersRelationship(objPolicy);
        }

        public AIA.Life.Models.Policy.Policy LoadPolicyPreviousInsuranceGrid(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic obj = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            objPolicy = obj.LoadPolicyPreviousInsuranceGrid(objPolicy);
            return objPolicy;
        }

        public AIA.Life.Models.Policy.Policy ResidentialStatusQuestions(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic obj = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            objPolicy = obj.ResidentialStatusQuestions(objPolicy);
            return objPolicy;
        }
        // Added for Demo
        public AIA.Life.Models.Policy.UWInbox FetchUWProposals(AIA.Life.Models.Policy.UWInbox objUWInbox)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic obj = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            return obj.FetchUWProposals(objUWInbox);
        }
        // till here
        public AIA.Life.Models.Policy.UWInbox FetchUWProposalCount(AIA.Life.Models.Policy.UWInbox objUWInbox)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic obj = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            return obj.FetchUWHomeProposalCount(objUWInbox);
        }

        public AIA.Life.Models.Policy.Policy LoadMasters(AIA.Life.Models.Policy.Policy objPolicy)
        {

            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic obj = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            return obj.LoadMasters(objPolicy);
        }

        public AIA.Life.Models.Common.Address FillAddressMasterList(AIA.Life.Models.Common.Address objAddress)
        {
            AIA.Life.Data.API.ControllerLogic.Common.CommonBusiness obj = new AIA.Life.Data.API.ControllerLogic.Common.CommonBusiness();
            objAddress = obj.FillAddressMasterList();
            return objAddress;
        }

        public ProposalInbox FetchProposalIncompleteDetails(ProposalInbox objProposalData)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            objProposalData = objPolicyLogic.FetchProposalIncompleteDetails(objProposalData);
            return objProposalData;
        }

        public ProposalInbox FetchProposalSubmittedDetails(ProposalInbox objProposalData)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            objProposalData = objPolicyLogic.FetchProposalSubmittedDetails(objProposalData);
            return objProposalData;
        }

        public Policy LoadSuspectReAllocation(Policy objPolicyLoadData)
        {
            Policy objPolicy = new Policy();
            AIA.Life.Data.API.ControllerLogic.Prospect.ProspectLogic objProspectLogic = new ControllerLogic.Prospect.ProspectLogic();

            objPolicy.objSuspectReAllocation = objProspectLogic.GetSuspectReAllocation();
            return objPolicy;
        }

        public LifeQuote LoadQuotationPool(LifeQuote objPolicyLoadData)
        {
            AIA.Life.Data.API.ControllerLogic.Quote.QuoteLogic objQuoteLogic = new AIA.Life.Data.API.ControllerLogic.Quote.QuoteLogic();
            objPolicyLoadData.ObjQuotationPool = objQuoteLogic.GetQuotationPool(objPolicyLoadData);
            return objPolicyLoadData;
        }

        public AIA.Life.Models.Opportunity.LifeQuote LoadBenefits(AIA.Life.Models.Opportunity.LifeQuote objQuote)
        {
            AIA.Life.Data.API.ControllerLogic.Quote.QuoteLogic objQuoteLogic = new AIA.Life.Data.API.ControllerLogic.Quote.QuoteLogic();
            objQuote = objQuoteLogic.LoadBenefits(objQuote);
            ControllerLogic.PremiumCalculation.Premium premium = new ControllerLogic.PremiumCalculation.Premium();
            objQuote = premium.GetRiderSumAssured(objQuote);

            return objQuote;
        }

        public AIA.Life.Models.Policy.Policy LoadProposalBenefits(AIA.Life.Models.Policy.Policy objProposal)
        {
            AIA.Life.Data.API.ControllerLogic.Quote.QuoteLogic objQuoteLogic = new AIA.Life.Data.API.ControllerLogic.Quote.QuoteLogic();
            return objQuoteLogic.LoadProposalBenefits(objProposal);
        }
        public AIA.Life.Models.Policy.Policy LoadProposalBenefitsPolicy(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            return objPolicyLogic.LoadProposalBenefits(objPolicy);
        }
        public AIA.Life.Models.Opportunity.LifeQuote SaveQuote(AIA.Life.Models.Opportunity.LifeQuote objQuote)
        {
            try
            {
                // string objquote = Newtonsoft.Json.JsonConvert.SerializeObject(objQuote);
                //using(AIA.Life.Repository.AIAEntity.AVOAIALifeEntities entity=new AIA.Life.Repository.AIAEntity.AVOAIALifeEntities())
                // {
                //     objQuote.objProspect.OccupationCode = entity.tblMasLifeOccupations.Where(a => a.OccupationCode == objQuote.objProspect.Occupation).Select(a => a.CompanyCode).FirstOrDefault();
                //     objQuote.objProspect.SalutationCode = entity.tblMasCommonTypes.Where(a => a.Description == objQuote.objProspect.Salutation).Select(a => a.Code).FirstOrDefault();
                // }

                AIA.Life.Data.API.ControllerLogic.Quote.QuoteLogic objQuoteLogic = new AIA.Life.Data.API.ControllerLogic.Quote.QuoteLogic();
                // if (string.IsNullOrEmpty(objQuote.objProspect.ClientCode))
                // {
                //     Prospect objProspect = (Prospect)IL.ClientEnquiry(objQuote.objProspect);
                //     objProspect.Error = new Error();
                //     objQuote.objProspect.ClientCode = objProspect.ClientCode;
                //     if (string.IsNullOrEmpty(objQuote.objProspect.ClientCode))
                //     {
                //         objQuote.objProspect = (Prospect)IL.ClientCreation(objQuote.objProspect);
                //     }
                // }
                // objQuote.Error = objQuote.objProspect.Error;
                // if(string.IsNullOrEmpty(objQuote.Error.ErrorMessage))

                objQuote = objQuoteLogic.SaveQuote(objQuote);
                return objQuote;
            }
            catch (Exception e)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = objQuote.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(e);
                objQuote.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + objQuote.Error.ErrorCode + ". Sorry for the inconvenience caused";
                return objQuote;
            }
        }
        public AIA.Life.Models.Opportunity.LifeQuote SendEmailAndSMSNotificationOnQuoteCreation(AIA.Life.Models.Opportunity.LifeQuote objQuote)
        {
            AIA.Life.Data.API.ControllerLogic.Quote.QuoteLogic objQuoteLogic = new AIA.Life.Data.API.ControllerLogic.Quote.QuoteLogic();
            return objQuoteLogic.SendEmailAndSMSNotificationOnQuoteCreation(objQuote);
        }
        public AIA.Life.Models.Opportunity.SMSReminder SMSReminder(AIA.Life.Models.Opportunity.SMSReminder objSMSReminder)
        {
            AIA.Life.Data.API.ControllerLogic.Quote.QuoteLogic objQuoteLogic = new AIA.Life.Data.API.ControllerLogic.Quote.QuoteLogic();
            return objQuoteLogic.SMSReminder(objSMSReminder);
        }
        public AIA.Life.Models.Opportunity.Prospect SendEmailAndSMSNotificationOnSAveProspect(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            AIA.Life.Data.API.ControllerLogic.Quote.QuoteLogic objQuoteLogic = new AIA.Life.Data.API.ControllerLogic.Quote.QuoteLogic();
            return objQuoteLogic.SendEmailAndSMSNotificationOnSAveProspect(objProspect);
        }

        public AIA.Life.Models.Policy.Policy SendEmailAndSMSNotificationOnSaveProposal(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            return objPolicyLogic.SendEmailAndSMSNotificationOnSaveProposal(objPolicy);
        }

        public AIA.Life.Models.EmailSMSDetails.EmailDetails Email(AIA.Life.Models.EmailSMSDetails.EmailDetails objEmail)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            objEmail = objPolicyLogic.Email(objEmail);
            return objEmail;

        }
        public AIA.Life.Models.EmailSMSDetails.SMSDetails SMS(AIA.Life.Models.EmailSMSDetails.SMSDetails objSMS)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            objSMS = objPolicyLogic.SMS(objSMS);
            return objSMS;

        }

        public AIA.Life.Models.Policy.Policy LoadMastersForProposal(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Data.API.ControllerLogic.Common.CommonBusiness objCommonLogic = new AIA.Life.Data.API.ControllerLogic.Common.CommonBusiness();
            return objCommonLogic.LoadMastersForProposal(objPolicy);
        }

        public AIA.Life.Models.Policy.Policy AssuredMemberDetails(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            return objPolicyLogic.AssuredMemberDetails(objPolicy);
        }
        public AIA.Life.Models.Policy.Policy BenefitDetails(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            return objPolicy;
        }

        public AIA.Life.Models.Policy.Policy SaveProposal(AIA.Life.Models.Policy.Policy objPolicy)
        {
            try
            {
                // string objPolicyProposal = Newtonsoft.Json.JsonConvert.SerializeObject(objPolicy);
                AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
                return objPolicyLogic.SaveProposal(objPolicy);
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = objPolicy.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(ex);
                objPolicy.Error.ErrorMessage = objPolicy.Message = "Please inform the IT HelpDesk on this application issue. Error Code is " + objPolicy.Error.ErrorCode + ". Sorry for the inconvenience caused";
                //objPolicy.Message = ex.InnerException.Message;
                return objPolicy;
            }

        }
        public AIA.Life.Models.Opportunity.LifeQuote LoadMastersForQuote(AIA.Life.Models.Opportunity.LifeQuote objLifeQuote)
        {
            AIA.Life.Data.API.ControllerLogic.Quote.QuoteLogic objQuoteLogic = new AIA.Life.Data.API.ControllerLogic.Quote.QuoteLogic();
            return objQuoteLogic.LoadMastersForQuote(objLifeQuote);
        }
        public AIA.Life.Models.Policy.Policy SendMedicalLetterMail(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            return objPolicyLogic.SendMedicalLetterMail(objPolicy);
        }
        public AIA.Life.Models.Policy.Policy LoadMastersForProposalDetails(AIA.Life.Models.Policy.Policy objProposal)
        {
            AIA.Life.Data.API.ControllerLogic.Quote.QuoteLogic objQuoteLogic = new AIA.Life.Data.API.ControllerLogic.Quote.QuoteLogic();
            return objQuoteLogic.LoadMastersForProposalDetails(objProposal);
        }
        public AIA.Life.Models.Opportunity.LifeQuote FetchQuoteData(AIA.Life.Models.Opportunity.LifeQuote objLifeQuote)
        {
            AIA.Life.Data.API.ControllerLogic.Quote.QuoteLogic objQuoteLogic = new AIA.Life.Data.API.ControllerLogic.Quote.QuoteLogic();
            return objQuoteLogic.FetchQuoteInfo(objLifeQuote);
        }
        public AIA.Life.Models.Opportunity.LifeQuote CalculateQuotePremium(AIA.Life.Models.Opportunity.LifeQuote objLifeQuote)
        {
            try
            {
                ControllerLogic.PremiumCalculation.Premium premium = new ControllerLogic.PremiumCalculation.Premium();
                objLifeQuote = premium.ValidateLifeRiderDetails(objLifeQuote);
                if (string.IsNullOrEmpty(objLifeQuote.Error.ErrorMessage))
                {
                    if (objLifeQuote.IsForCounterOffer == false)
                    {

                        objLifeQuote = premium.ValidateProductDetails(objLifeQuote);
                        if (string.IsNullOrEmpty(objLifeQuote.Error.ErrorMessage))
                            objLifeQuote = premium.ValidateFHEC(objLifeQuote);
                        if (string.IsNullOrEmpty(objLifeQuote.Error.ErrorMessage))
                        {
                            objLifeQuote = premium.CalculateQuotePremium(objLifeQuote);
                            ControllerLogic.PremiumCalculation.Illustration illustration = new ControllerLogic.PremiumCalculation.Illustration();
                            objLifeQuote = illustration.GetIllustration(objLifeQuote);
                            objLifeQuote = premium.ValidatePremiumDetails(objLifeQuote);
                        }
                    }
                    else
                    {
                        objLifeQuote = premium.CalculateQuotePremium(objLifeQuote);
                        ControllerLogic.PremiumCalculation.Illustration illustration = new ControllerLogic.PremiumCalculation.Illustration();
                        objLifeQuote = illustration.GetIllustration(objLifeQuote);
                    }
                }

                return objLifeQuote;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = objLifeQuote.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(ex);
                objLifeQuote.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + objLifeQuote.Error.ErrorCode + ". Sorry for the inconvenience caused";
                return objLifeQuote;
            }
        }
        public AIA.Life.Models.Policy.Policy LoadProposalInfo(AIA.Life.Models.Policy.Policy objPolicy)
        {
            try
            {
                //string objPolicyProposal = Newtonsoft.Json.JsonConvert.SerializeObject(objPolicy);
                AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();

                objPolicy = objPolicyLogic.LoadProposalInfo(objPolicy);

                //#region Create Proposal Validation
                //if (ConfigurationManager.AppSettings["PublishEnvironment"].ToString() != "SIT" && ConfigurationManager.AppSettings["PublishEnvironment"].ToString() != "UAT")
                //{
                //    if (string.IsNullOrEmpty(objPolicy.LeadNo) || objPolicy.LeadNo == "A" || objPolicy.LeadNo == "B")
                //    {
                //        objPolicy.Error.ErrorMessage = "Proposal Cannot be created without Lead Number.";
                //    }
                //}
                //#endregion

                //if (string.IsNullOrEmpty(objPolicy.Error.ErrorMessage))
                //{
                //    #region IL Quick Proposal
                //    objPolicy.BizDate = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
                //    var names = objPolicy.objProspectDetails.FirstName != null ? objPolicy.objProspectDetails.FirstName.Split(' ') : null;
                //    if (names != null)
                //    {
                //        for (int j = 0; j < names.Length; j++)
                //        {
                //            if (j == 0)
                //                objPolicy.objProspectDetails.NameWithInitial = names[j].Substring(0, 1);
                //            else
                //                objPolicy.objProspectDetails.NameWithInitial += " " + names[j].Substring(0, 1);
                //        }
                //    }
                //    objPolicy.objProspectDetails.Nationality = "SL";
                //    if (string.IsNullOrEmpty(objPolicy.objProspectDetails.NewNICNO))
                //        objPolicy.objProspectDetails.Nationality = "SLC";
                //    if (string.IsNullOrEmpty(objPolicy.objProspectDetails.ClientCode))
                //    {
                //      //  objPolicy.objProspectDetails = (MemberDetails)IL.ClientEnquiry(objPolicy.objProspectDetails);
                //       // objPolicy.objProspectDetails.Error = new Error();
                //    }
                //    if (string.IsNullOrEmpty(objPolicy.objProspectDetails.ClientCode))
                //       // objPolicy.objProspectDetails = (MemberDetails)IL.ClientCreation(objPolicy.objProspectDetails);
                //    if (string.IsNullOrEmpty(objPolicy.objProspectDetails.Error.ErrorMessage))
                //    {

                //        if (string.IsNullOrEmpty(objPolicy.Error.ErrorMessage))
                //           // objPolicy = (Policy)IL.QuickProposal(objPolicy);
                //    }
                //    else
                //    {
                //        objPolicy.Error = objPolicy.objProspectDetails.Error;
                //    }
                //    #endregion
                //}
                //if (string.IsNullOrEmpty(objPolicy.Error.ErrorMessage) && !string.IsNullOrEmpty(objPolicy.ProposalNo))
                //{
                    objPolicy = SaveProposal(objPolicy);
                    Policy newPolicy = new Policy();
                    newPolicy.ProposalNo = objPolicy.ProposalNo;
                    newPolicy.QuoteNo = objPolicy.QuoteNo;
                    return objPolicyLogic.FetchProposalInfo(newPolicy);
                //}
                //else
                //{
                //    objPolicy.Error.ErrorMessage = "Sorry,the proposal number cannot be generated right now." + Environment.NewLine + "Please try again later or contact IT HelpDesk.";
                //}
                //return objPolicy;
            }
            catch (Exception e)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = objPolicy.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(e);
                objPolicy.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + objPolicy.Error.ErrorCode + ". Sorry for the inconvenience caused";
                return objPolicy;
            }

        }
        public AIA.Life.Models.Policy.Policy LoadMHDProposalInfo(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            return objPolicyLogic.LoadMHDProposalInfo(objPolicy);
        }
        public async Task<AIA.Life.Models.Policy.Policy> FetchProposalInfo(AIA.Life.Models.Policy.Policy ObjPolicy)
        {
            try
            {
                AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
                var FetchProposalData = Task.Run(() => objPolicyLogic.FetchProposalInfo(ObjPolicy));
                var LoadMasters = Task.Run(() => objPolicyLogic.LoadProposalMasters());
                await Task.WhenAll(FetchProposalData, LoadMasters);
                var ProposalFetch = await FetchProposalData;
                var MasterFetch = await LoadMasters;
                if (MasterFetch != null)
                {
                    ObjPolicy = objPolicyLogic.MapMasters(MasterFetch, ObjPolicy);
                }
                return ObjPolicy;
                //Task.WaitAll(new[] { FetchProposalData,LoadMasters });
                //if (MasterFetch.Result != null)
                //{
                //    ObjPolicy = MapMasters(LoadMasters.Result, ObjPolicy);
                //}
                // return objPolicyLogic.FetchProposalInfo(ObjPolicy);
            }
            catch (Exception e)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = ObjPolicy.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(e);
                ObjPolicy.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + ObjPolicy.Error.ErrorCode + ". Sorry for the inconvenience caused";
                return ObjPolicy;
            }

        }
        public AIA.Life.Models.Policy.Policy SaveUWRemarks(AIA.Life.Models.Policy.Policy ObjPolicy)
        {
            try
            {
                AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
                return objPolicyLogic.SaveUWRemarks(ObjPolicy);
            }
            catch (Exception e)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = ObjPolicy.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(e);
                ObjPolicy.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + ObjPolicy.Error.ErrorCode + ". Sorry for the inconvenience caused";
                return ObjPolicy;
            }
        }

        public AIA.Life.Models.Integration.Services.ProposalInfo UpdateUWInfo(AIA.Life.Models.Integration.Services.ProposalInfo objPropoalInfo)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            return objPolicyLogic.UpdateUWInfo(objPropoalInfo);
        }

        public UWMemberLevelDeviationStatus UpdateMemberLevelDeviation(UWMemberLevelDeviationStatus objStatus)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            return objPolicyLogic.UpdateMemberLevelDeviation(objStatus);

        }
        public MemberDetails FetchLoadigInfo(MemberDetails objMemberDetails)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            return objPolicyLogic.FetchLoadigInfo(objMemberDetails);

        }
        [HttpPost]
        public DeletePolicyDocuments DeletePolicyDocument(DeletePolicyDocuments ObjPolicyDocuments)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            return objPolicyLogic.DeletePolicyDocument(ObjPolicyDocuments);

        }
        [HttpPost]
        public DeletePolicyDocuments DeleteDocumentLinkPolicyDocument(DeletePolicyDocuments ObjPolicyDocuments)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            return objPolicyLogic.DeleteDocumentLinkPolicyDocument(ObjPolicyDocuments);

        }


        public AIA.Life.Models.Reports.UWDecisionReport UWDecisionReport(AIA.Life.Models.Reports.UWDecisionReport objUWDecisionReport)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            return objPolicyLogic.UWDecisionReport(objUWDecisionReport);
        }
        public MemberLevelDecisions DerivePolicyLevelDecision(MemberLevelDecisions objMemberDecision)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            return objPolicyLogic.DerivePolicyLevelDecision(objMemberDecision);
        }

        public AIA.Life.Models.Policy.Policy CalculateLoadingPremium(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            return objPolicyLogic.CalculateLoadingPremium(objPolicy);
        }
        public AIA.Life.Models.Policy.Policy SaveLoadingDetails(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            return objPolicyLogic.SaveLoadingDetails(objPolicy);
        }
        public AIA.Life.Models.Policy.Policy SubmitPolicyDocuments(AIA.Life.Models.Policy.Policy objpolicy)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            return objPolicyLogic.SubmitPolicyDocuments(objpolicy);
        }
        public AIA.Life.Models.Policy.Policy CounterOfferSubmit(AIA.Life.Models.Policy.Policy objpolicy)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            return objPolicyLogic.CounterOfferSubmit(objpolicy);
        }
        public AIA.Life.Models.Policy.Policy EmailNotificationOnUWDecision(AIA.Life.Models.Policy.Policy objpolicy)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            if (objPolicyLogic.EmailNotificationOnUWDecision(objpolicy, objpolicy.Decision))
            {
                objpolicy.Message = "Success";
                return objpolicy;
            }
            else
            {
                objpolicy.Message = "Error";
                return objpolicy;
            }

        }
        public AIA.Life.Models.Policy.Policy SMSNotificationOnUWDecision(AIA.Life.Models.Policy.Policy objpolicy)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            if (objPolicyLogic.SMSNotificationOnUWDecision(objpolicy, objpolicy.Decision))
            {
                objpolicy.Message = "Success";
                return objpolicy;
            }
            else
            {
                objpolicy.Message = "Error";
                return objpolicy;
            }

        }
        public AIA.Life.Models.Policy.Policy TestUWDeviation(AIA.Life.Models.Policy.Policy objPropoalInfo)
        {
            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
            return objPolicyLogic.TestUWDeviation(objPropoalInfo);
        }

        public AIA.Life.Models.Policy.Policy FetchProposalLifeAssuredDetails(AIA.Life.Models.Policy.Policy objPolicy)
        {
            try
            {
                AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();

                objPolicy = objPolicyLogic.FetchProposalLifeAssuredDetails(objPolicy);
                return objPolicy;
            }
            catch (Exception e)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = objPolicy.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(e);
                objPolicy.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + objPolicy.Error.ErrorCode + ". Sorry for the inconvenience caused";
                return objPolicy;
            }

        }

        public AIA.Life.Models.Policy.Policy FetchProposalNomineeDetailsWithMemberDetails(AIA.Life.Models.Policy.Policy objPolicy)
        {
            try
            {
                AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();

                objPolicy = objPolicyLogic.FetchProposalNomineeDetailsWithMemberDetails(objPolicy);
                return objPolicy;
            }
            catch (Exception e)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = objPolicy.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(e);
                objPolicy.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + objPolicy.Error.ErrorCode + ". Sorry for the inconvenience caused";
                return objPolicy;
            }

        }

        public AIA.Life.Models.Opportunity.LifeQuote LoadPreviousInsuranceGrid(AIA.Life.Models.Opportunity.LifeQuote objLifeQuote)
        {
            AIA.Life.Data.API.ControllerLogic.Quote.QuoteLogic objQuoteLogic = new AIA.Life.Data.API.ControllerLogic.Quote.QuoteLogic();
            return objQuoteLogic.LoadPreviousInsuranceGrid(objLifeQuote);
        }
        //public void InvokeILPolicyIssuance(PaymentModel payment)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(payment.ProposalNo))
        //        {
        //            payment = (PaymentModel)IL.RecieptEnquiry(payment);
        //            payment = (PaymentModel)IL.ProposalPreIssueValidation(payment);
        //            if (string.IsNullOrEmpty(payment.Error.ErrorMessage) && (payment.PreIssueValidations.Count <= 1))
        //            {
        //                if (string.IsNullOrEmpty(payment.Error.ErrorMessage))
        //                {
        //                    payment = (PaymentModel)IL.ProposalUWApproval(payment);
        //                    if (string.IsNullOrEmpty(payment.Error.ErrorMessage))
        //                    {
        //                        payment = (PaymentModel)IL.QualityControl(payment);
        //                        if (string.IsNullOrEmpty(payment.Error.ErrorMessage))
        //                            payment = (PaymentModel)IL.ProposalIssuance(payment);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        log4net.GlobalContext.Properties["ErrorCode"] = payment.Error.ErrorCode = Codes.GetErrorCode();
        //        Logger.Error(e);
        //        payment.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + payment.Error.ErrorCode + ". Sorry for the inconvenience caused";
        //    }

        //}
        //public async Task<Policy> InvokeILModifyProposal(AIA.Life.Models.Policy.Policy policy)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(policy.ProposalNo))
        //        {
        //            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
        //            string tempQuoteNo = policy.QuoteNo;
        //            policy = new Policy();
        //            policy.QuoteNo = tempQuoteNo;
        //            policy.ProposalFetch = true;
        //            policy = objPolicyLogic.FetchProposalInfo(policy);
        //            string mainClientCode = string.Empty;
        //            AVOAIALifeEntities entity = new AVOAIALifeEntities();
        //            bool isClientUpdated = policy.objMemberDetails.FindIndex(a => a.ClientCode == "" || a.ClientCode == null) == -1 ? true : false;

        //            if (!isClientUpdated)
        //            {
        //                #region Client creation

        //                #region If Proposer not same as main life
        //                if (policy.objProspectDetails.IsproposerlifeAssured == false)
        //                {
        //                    var names = string.IsNullOrEmpty(policy.objProspectDetails.FirstName) != true ? policy.objProspectDetails.FirstName.Split(' ') : null;
        //                    if (names != null)
        //                    {
        //                        for (int j = 0; j < names.Length; j++)
        //                        {
        //                            if (j == 0)
        //                                policy.objProspectDetails.NameWithInitial = names[j].Substring(0, 1);
        //                            else
        //                                policy.objProspectDetails.NameWithInitial += " " + names[j].Substring(0, 1);
        //                        }
        //                    }
        //                    policy.objProspectDetails.Language = policy.PreferredLanguage;

        //                    if (policy.objProspectDetails.Nationality == "USA")
        //                        policy.objProspectDetails.IsUSCitizen = true;

        //                    if (policy.objProspectDetails.OccupationID == 0)
        //                        policy.objProspectDetails.OccupationID = null;
        //                    if (string.IsNullOrEmpty(policy.objProspectDetails.NewNICNO))
        //                        policy.objProspectDetails.Nationality = "SLC";
        //                    if (string.IsNullOrEmpty(policy.objProspectDetails.ClientCode))
        //                    {
        //                        policy.objProspectDetails = (MemberDetails)IL.ClientEnquiry(policy.objProspectDetails);
        //                        policy.objProspectDetails.Error = new Error();
        //                    }
        //                    if (!string.IsNullOrEmpty(policy.objProspectDetails.ClientCode))
        //                    {
        //                        policy.objProspectDetails = (MemberDetails)IL.ModifyClient(policy.objProspectDetails);
        //                    }
        //                    if (string.IsNullOrEmpty(policy.objProspectDetails.ClientCode))
        //                    {
        //                        policy.objProspectDetails = (MemberDetails)IL.ClientCreation(policy.objProspectDetails);
        //                    }
        //                }
        //                #endregion

        //                for (int i = 0; i < policy.objMemberDetails.Count; i++)
        //                {
        //                    var names = policy.objMemberDetails[i].FirstName != null ? policy.objMemberDetails[i].FirstName.Split(' ') : null;
        //                    if (names != null)
        //                    {
        //                        for (int j = 0; j < names.Length; j++)
        //                        {
        //                            if (j == 0)
        //                                policy.objMemberDetails[i].NameWithInitial = names[j].Substring(0, 1);
        //                            else
        //                                policy.objMemberDetails[i].NameWithInitial += " " + names[j].Substring(0, 1);
        //                        }
        //                    }
        //                    policy.objMemberDetails[i].Language = policy.PreferredLanguage;

        //                    if (policy.objMemberDetails[i].Nationality == "USA")
        //                        policy.objMemberDetails[i].IsUSCitizen = true;
        //                    if (policy.objMemberDetails[i].RelationShipWithPropspect == "271" ||
        //                        policy.objMemberDetails[i].RelationShipWithPropspect == "270" ||
        //                        policy.objMemberDetails[i].RelationShipWithPropspect == "269")
        //                    {
        //                        policy.objMemberDetails[i].objLifeStyleQuetions.Height = 33;
        //                        policy.objMemberDetails[i].objLifeStyleQuetions.Weight = 3;
        //                        policy.objMemberDetails[i].MaritialStatus = "S";
        //                    }
        //                    if (policy.objMemberDetails[i].OccupationID == 0)
        //                        policy.objMemberDetails[i].OccupationID = null;
        //                    if (string.IsNullOrEmpty(policy.objMemberDetails[i].NewNICNO))
        //                        policy.objMemberDetails[i].Nationality = "SLC";
        //                    if (string.IsNullOrEmpty(policy.objMemberDetails[i].ClientCode))
        //                    {
        //                        policy.objMemberDetails[i] = (MemberDetails)IL.ClientEnquiry(policy.objMemberDetails[i]);
        //                        policy.objMemberDetails[i].Error = new Error();
        //                    }
        //                    if (!string.IsNullOrEmpty(policy.objMemberDetails[i].ClientCode))
        //                    {
        //                        policy.objMemberDetails[i] = (MemberDetails)IL.ModifyClient(policy.objMemberDetails[i]);
        //                    }
        //                    if (string.IsNullOrEmpty(policy.objMemberDetails[i].ClientCode))
        //                    {
        //                        policy.objMemberDetails[i] = (MemberDetails)IL.ClientCreation(policy.objMemberDetails[i]);
        //                    }
        //                    if (policy.objMemberDetails[i].RelationShipWithPropspect == "267")
        //                        mainClientCode = policy.objMemberDetails[i].ClientCode;
        //                    if (!string.IsNullOrEmpty(policy.objMemberDetails[i].ClientCode))
        //                    {
        //                        decimal membId = policy.objMemberDetails[i].MemberID;
        //                        var member = entity.tblPolicyMemberDetails.Where(a => a.MemberID == membId).FirstOrDefault();
        //                        member.ClientCode = policy.objMemberDetails[i].ClientCode;
        //                        entity.SaveChanges();
        //                    }
        //                }

        //                if (policy.objNomineeDetails.Count() > 0)
        //                {
        //                    for (int i = 0; i < policy.objNomineeDetails.Count(); i++)
        //                    {
        //                        MemberDetails nominee = new MemberDetails();
        //                        nominee.ClientCode = policy.objNomineeDetails[i].NomineeClientCode;
        //                        nominee.FirstName = policy.objNomineeDetails[i].NomineeName;
        //                        nominee.LastName = policy.objNomineeDetails[i].NomineeSurname;
        //                        string sal = policy.objNomineeDetails[i].NomineeSalutation;
        //                        nominee.SalutationCode = entity.tblMasCommonTypes.Where(a => a.Description == sal && a.MasterType == "Salutation").Select(a => a.Code).FirstOrDefault();
        //                        nominee.DateOfBirth = policy.objNomineeDetails[i].NomineeNicNoDOB;
        //                        nominee.NewNICNO = policy.objNomineeDetails[i].NomineeNICNo;
        //                        var names = policy.objNomineeDetails[i].NomineeName != null ? policy.objNomineeDetails[i].NomineeName.Split(' ') : null;
        //                        if (names != null)
        //                        {
        //                            for (int j = 0; j < names.Length; j++)
        //                            {
        //                                if (j == 0)
        //                                    nominee.NameWithInitial = names[j].Substring(0, 1);
        //                                else
        //                                    nominee.NameWithInitial += " " + names[j].Substring(0, 1);
        //                            }
        //                        }
        //                        else
        //                            nominee.NameWithInitial = policy.objNomineeDetails[i].NomineeName;
        //                        nominee.Gender = policy.objNomineeDetails[i].NomineeGender;
        //                        nominee.MaritialStatus = policy.objNomineeDetails[i].NomineeMaritalStatus;
        //                        nominee.MobileNo = policy.objNomineeDetails[i].NomineeTelephone;
        //                        nominee.objCommunicationAddress = policy.objProspectDetails.objCommunicationAddress;
        //                        nominee.Gender = policy.objNomineeDetails[i].NomineeGender;
        //                        if (string.IsNullOrEmpty(nominee.NewNICNO))
        //                            nominee.Nationality = "SLC";
        //                        else
        //                            nominee.Nationality = "SL";
        //                        nominee.OccupationID = null;
        //                        if (string.IsNullOrEmpty(nominee.ClientCode))
        //                            nominee = (MemberDetails)IL.ClientEnquiry(nominee);
        //                        if (string.IsNullOrEmpty(nominee.ClientCode))
        //                        {
        //                            nominee = (MemberDetails)IL.ClientCreation(nominee);
        //                        }
        //                        else
        //                        {
        //                            nominee = (MemberDetails)IL.ModifyClient(nominee);
        //                        }
        //                        policy.objNomineeDetails[i].NomineeClientCode = nominee.ClientCode;
        //                        if (!string.IsNullOrEmpty(policy.objNomineeDetails[i].NomineeClientCode))
        //                        {
        //                            decimal nomId = policy.objNomineeDetails[i].NomineeDetailsId;
        //                            var nomi = entity.tblPolicyNomineeDetails.Where(a => a.NomineeID == nomId).FirstOrDefault();
        //                            nomi.ClientCode = policy.objNomineeDetails[i].NomineeClientCode;
        //                            entity.SaveChanges();
        //                        }
        //                    }

        //                }
        //                #endregion

        //                #region Client Relation Creation
        //                List<ClientRelation> clientRelations = new List<ClientRelation>();
        //                if (policy.objProspectDetails.IsproposerlifeAssured == false)
        //                {
        //                    if (!string.IsNullOrEmpty(policy.objProspectDetails.ClientCode))
        //                    {
        //                        ClientRelation clientRelation = new ClientRelation();
        //                        clientRelation.ClientCode = policy.objProspectDetails.ClientCode;
        //                        clientRelation.Relation = policy.objProspectDetails.RelationShipWithPropspect;
        //                        clientRelations.Add(clientRelation);
        //                    }
        //                }
        //                for (int i = 0; i < policy.objMemberDetails.Count; i++)
        //                {
        //                    ClientRelation clientRelation = new ClientRelation();
        //                    if (policy.objMemberDetails[i].RelationShipWithPropspect == "268")
        //                    {
        //                        if (!string.IsNullOrEmpty(policy.objMemberDetails[i].ClientCode))
        //                        {
        //                            if (policy.objMemberDetails[i].Gender == "F")
        //                                clientRelation.Relation = "WIFE";
        //                            else
        //                                clientRelation.Relation = "HUSB";
        //                            clientRelation.ClientCode = policy.objMemberDetails[i].ClientCode;
        //                        }
        //                    }
        //                    if (policy.objMemberDetails[i].RelationShipWithPropspect == "271" ||
        //                        policy.objMemberDetails[i].RelationShipWithPropspect == "270" ||
        //                        policy.objMemberDetails[i].RelationShipWithPropspect == "269")
        //                    {
        //                        if (!string.IsNullOrEmpty(policy.objMemberDetails[i].ClientCode))
        //                        {
        //                            if (policy.objMemberDetails[i].Gender == "F")
        //                                clientRelation.Relation = "DAUG";
        //                            else
        //                                clientRelation.Relation = "SON";
        //                            clientRelation.ClientCode = policy.objMemberDetails[i].ClientCode;
        //                        }
        //                    }
        //                    if (!string.IsNullOrEmpty(clientRelation.ClientCode))
        //                        clientRelations.Add(clientRelation);
        //                }
        //                if (policy.objNomineeDetails.Count() > 0)
        //                {
        //                    for (int i = 0; i < policy.objNomineeDetails.Count(); i++)
        //                    {
        //                        ClientRelation clientRelation = new ClientRelation();
        //                        if (!string.IsNullOrEmpty(policy.objNomineeDetails[i].NomineeClientCode))
        //                        {
        //                            int relId = Convert.ToInt32(policy.objNomineeDetails[i].NomineeRelationship);
        //                            clientRelation.Relation = entity.tblMasCommonTypes.Where(a => a.CommonTypesID == relId).Select(a => a.Code).FirstOrDefault();
        //                            clientRelation.ClientCode = policy.objNomineeDetails[i].NomineeClientCode;
        //                        }
        //                        if (!string.IsNullOrEmpty(clientRelation.ClientCode))
        //                            clientRelations.Add(clientRelation);
        //                    }
        //                }
        //                if (clientRelations.Count() > 0)
        //                {
        //                    MemberDetails memberDetails = new MemberDetails();
        //                    memberDetails.ClientCode = mainClientCode;
        //                    memberDetails = (MemberDetails)IL.ClientRelationshipEnquiry(memberDetails);
        //                    List<ClientRelation> nonExistingRel = new List<ClientRelation>();
        //                    foreach (var item in clientRelations)
        //                    {
        //                        foreach (var e in memberDetails.ClientRelations)
        //                        {
        //                            if (item.ClientCode != e.ClientCode && item.Relation != e.Relation)
        //                            {
        //                                if (nonExistingRel
        //                            .Where(a => a.ClientCode == e.ClientCode && a.Relation == e.Relation)
        //                            .FirstOrDefault() == null)
        //                                    nonExistingRel.Add(e);
        //                            }
        //                        }
        //                    }
        //                    memberDetails.ClientRelations = new List<ClientRelation>();
        //                    memberDetails.ClientRelations = clientRelations;
        //                    memberDetails.ClientRelations.AddRange(nonExistingRel);
        //                    memberDetails.ClientRelations = memberDetails.ClientRelations.GroupBy(a => new { a.ClientCode, a.Relation }).Select(g => g.FirstOrDefault()).Distinct().ToList();
        //                    memberDetails = (MemberDetails)IL.ClientRelationshipCreation(memberDetails);
        //                }

        //                #endregion
        //            }

        //            #region Modify Proposal Header
        //            policy = (Policy)IL.ModifyProposalModifyLife(policy);
        //            #endregion

        //            #region Add Life and Riders

        //            List<Task<MemberDetails>> taskList = new List<Task<MemberDetails>>();
        //            for (int i = 0; i < policy.objMemberDetails.Count; i++)
        //            {
        //                if (!string.IsNullOrEmpty(policy.objMemberDetails[i].ClientCode))
        //                {
        //                    if (policy.objMemberDetails[i].RelationShipWithPropspect == "271" ||
        //                        policy.objMemberDetails[i].RelationShipWithPropspect == "270" ||
        //                        policy.objMemberDetails[i].RelationShipWithPropspect == "269")
        //                    {
        //                        policy.objMemberDetails[i].objLifeStyleQuetions.Height = 33;
        //                        policy.objMemberDetails[i].objLifeStyleQuetions.Weight = 3;
        //                        policy.objMemberDetails[i].MaritialStatus = "S";
        //                    }
        //                    policy.objMemberDetails[i].ProposalNo = policy.ProposalNo;
        //                    policy.objMemberDetails[i].PolicyTerm = Convert.ToInt32(policy.PolicyTerm);
        //                    policy.objMemberDetails[i].PremiumTerm = Convert.ToInt32(policy.PaymentTerm);
        //                    policy.objMemberDetails[i].PensionTerm = Convert.ToInt32(policy.SmartPensionReceivingPeriod);
        //                    policy.objMemberDetails[i].MonthlySavingBenifit = Convert.ToInt32(policy.SmartPensionMonthlyIncome);
        //                    policy.objMemberDetails[i].Deductible = policy.Deductible;
        //                    policy.objMemberDetails[i].MaturityBenefit = policy.MaturityBenefits;
        //                    policy.objMemberDetails[i].LifeNum = "0" + (i + 1).ToString();
        //                    //policy.objMemberDetails[i] = (MemberDetails)IL.ModifyProposalAddLife(policy.objMemberDetails[i]);
        //                    MemberDetails member = policy.objMemberDetails[i];
        //                    taskList.Add(Task.Run(() => (MemberDetails)IL.ModifyProposalAddLife(member)));
        //                    Thread.Sleep(1000);
        //                }
        //            }
                    
        //            #endregion

        //            #region Refresh Riders                    
        //            for (int i = 0; i < policy.objMemberDetails.Count; i++)
        //            {
        //                if (!string.IsNullOrEmpty(policy.objMemberDetails[i].ClientCode))
        //                {
        //                    MemberDetails member = policy.objMemberDetails[i];
        //                    taskList.Add(Task.Run(() => (MemberDetails)IL.RefreshRiders(member)));
        //                    Thread.Sleep(1000);
        //                    //policy.objMemberDetails[i] = (MemberDetails)IL.RefreshRiders(policy.objMemberDetails[i]);
        //                    //Thread.Sleep(3000);
        //                }
        //            }
        //            await Task.WhenAll(taskList);
        //            #endregion

        //            ILStatus iLStatus = new ILStatus();
        //            iLStatus.ProposalNo = policy.ProposalNo;
        //            iLStatus.ServiceName = "ModifyProposal";
        //            iLStatus.ServiceStatus = "SUCC";
        //            ServiceLog.UpdateILStatusLog(iLStatus);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        ILStatus iLStatus = new ILStatus();
        //        iLStatus.ProposalNo = policy.ProposalNo;
        //        iLStatus.ServiceName = "ModifyProposal";
        //        iLStatus.ServiceStatus = "FAIL";
        //        ServiceLog.UpdateILStatusLog(iLStatus);
        //        log4net.GlobalContext.Properties["ErrorCode"] = policy.Error.ErrorCode = Codes.GetErrorCode();
        //        Logger.Error(e);
        //        policy.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + policy.Error.ErrorCode + ". Sorry for the inconvenience caused";
        //    }
        //    return policy;
        //}
        //public async void InvokeILForDeleteLife(AIA.Life.Models.Policy.Policy policy)
        //{
        //    try
        //    {
        //        AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
        //        string tempQuoteNo = policy.QuoteNo;
        //        policy = new Policy();
        //        policy.QuoteNo = tempQuoteNo;
        //        policy.ProposalFetch = true;
        //        policy = objPolicyLogic.FetchProposalInfo(policy);
        //        AVOAIALifeEntities entity = new AVOAIALifeEntities();

        //        #region Delete Life and Riders
        //        List<Task<MemberDetails>> taskList = new List<Task<MemberDetails>>();
        //        for (int i = 0; i < policy.objMemberDetails.Count; i++)
        //        {
        //            policy.objMemberDetails[i].ProposalNo = policy.ProposalNo;
        //            policy.objMemberDetails[i].LifeNum = "0" + (i + 1).ToString();
        //            //policy.objMemberDetails[i] = (MemberDetails)IL.DeleteLife(policy.objMemberDetails[i]);
        //            MemberDetails member = policy.objMemberDetails[i];
        //            taskList.Add(Task.Run(() => (MemberDetails)IL.DeleteLife(member)));
        //            //Thread.Sleep(3000);
        //        }
        //        await Task.WhenAll(taskList);

        //        policy = (Policy)IL.ModifyProposalModifyLife(policy);
        //        #endregion

        //        ILStatus iLStatus = new ILStatus();
        //        iLStatus.ProposalNo = policy.ProposalNo;
        //        iLStatus.ServiceName = "DeleteLife";
        //        iLStatus.ServiceStatus = "SUCC";
        //        ServiceLog.UpdateILStatusLog(iLStatus);
        //    }
        //    catch (Exception e)
        //    {
        //        ILStatus iLStatus = new ILStatus();
        //        iLStatus.ProposalNo = policy.ProposalNo;
        //        iLStatus.ServiceName = "DeleteProposal";
        //        iLStatus.ServiceStatus = "FAIL";
        //        ServiceLog.UpdateILStatusLog(iLStatus);
        //        log4net.GlobalContext.Properties["ErrorCode"] = policy.Error.ErrorCode = Codes.GetErrorCode();
        //        Logger.Error(e);
        //        policy.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + policy.Error.ErrorCode + ". Sorry for the inconvenience caused";
        //    }
        //}
        //public async void InvokeILForAddLife(AIA.Life.Models.Policy.Policy policy)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(policy.ProposalNo))
        //        {
        //            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
        //            string tempQuoteNo = policy.QuoteNo;
        //            policy = new Policy();
        //            policy.QuoteNo = tempQuoteNo;
        //            policy.ProposalFetch = true;
        //            policy = objPolicyLogic.FetchProposalInfo(policy);
        //            #region Add Life and Riders
        //            List<Task<MemberDetails>> taskList = new List<Task<MemberDetails>>();
        //            for (int i = 0; i < policy.objMemberDetails.Count; i++)
        //            {
        //                if (!string.IsNullOrEmpty(policy.objMemberDetails[i].ClientCode))
        //                {
        //                    if (policy.objMemberDetails[i].Nationality == "USA")
        //                        policy.objMemberDetails[i].IsUSCitizen = true;
        //                    if (policy.objMemberDetails[i].RelationShipWithPropspect == "271" ||
        //                        policy.objMemberDetails[i].RelationShipWithPropspect == "270" ||
        //                        policy.objMemberDetails[i].RelationShipWithPropspect == "269")
        //                    {
        //                        policy.objMemberDetails[i].objLifeStyleQuetions.Height = 33;
        //                        policy.objMemberDetails[i].objLifeStyleQuetions.Weight = 3;
        //                        policy.objMemberDetails[i].MaritialStatus = "S";
        //                    }
        //                    if (policy.objMemberDetails[i].OccupationID == 0)
        //                        policy.objMemberDetails[i].OccupationID = null;
        //                    policy.objMemberDetails[i].ProposalNo = policy.ProposalNo;
        //                    policy.objMemberDetails[i].PolicyTerm = Convert.ToInt32(policy.PolicyTerm);
        //                    policy.objMemberDetails[i].PremiumTerm = Convert.ToInt32(policy.PaymentTerm);
        //                    policy.objMemberDetails[i].PensionTerm = Convert.ToInt32(policy.SmartPensionReceivingPeriod);
        //                    policy.objMemberDetails[i].MonthlySavingBenifit = Convert.ToInt32(policy.SmartPensionMonthlyIncome);
        //                    policy.objMemberDetails[i].Deductible = policy.Deductible;
        //                    policy.objMemberDetails[i].MaturityBenefit = policy.MaturityBenefits;
        //                    policy.objMemberDetails[i].LifeNum = "0" + (i + 1).ToString();
        //                    //policy.objMemberDetails[i] = (MemberDetails)IL.ModifyProposalAddLife(policy.objMemberDetails[i]);
        //                    MemberDetails member = policy.objMemberDetails[i];
        //                    taskList.Add(Task.Run(() => (MemberDetails)IL.ModifyProposalAddLife(member)));
        //                    Thread.Sleep(1000);
        //                }
        //            }
        //            await Task.WhenAll(taskList);
        //            #endregion

        //            #region Modify Life refresh rider

        //            for (int i = 0; i < policy.objMemberDetails.Count; i++)
        //            {
        //                if (!string.IsNullOrEmpty(policy.objMemberDetails[i].ClientCode))
        //                {
        //                    policy.objMemberDetails[i] = (MemberDetails)IL.RefreshRiders(policy.objMemberDetails[i]);
        //                    Thread.Sleep(3000);
        //                }
        //            }
        //            #endregion

        //            ILStatus iLStatus = new ILStatus();
        //            iLStatus.ProposalNo = policy.ProposalNo;
        //            iLStatus.ServiceName = "ModifyProposalAfterDelete";
        //            iLStatus.ServiceStatus = "SUCC";
        //            ServiceLog.UpdateILStatusLog(iLStatus);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        ILStatus iLStatus = new ILStatus();
        //        iLStatus.ProposalNo = policy.ProposalNo;
        //        iLStatus.ServiceName = "ModifyProposalAfterDelete";
        //        iLStatus.ServiceStatus = "FAIL";
        //        ServiceLog.UpdateILStatusLog(iLStatus);
        //        log4net.GlobalContext.Properties["ErrorCode"] = policy.Error.ErrorCode = Codes.GetErrorCode();
        //        Logger.Error(e);
        //    }
        //}
        //public void InvokeILUWApproval(AIA.Life.Models.Policy.Policy policy)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(policy.ProposalNo))
        //        {
        //            policy = (Policy)IL.ProposalPreIssueValidation(policy);
        //            if (string.IsNullOrEmpty(policy.Error.ErrorMessage))
        //                policy = (Policy)IL.ProposalFollowupEnquiry(policy);
        //            if (string.IsNullOrEmpty(policy.Error.ErrorMessage))
        //                policy = (Policy)IL.ProposalUWApproval(policy);
        //            if (string.IsNullOrEmpty(policy.Error.ErrorMessage))
        //                policy = (Policy)IL.QualityControl(policy);
        //            if (string.IsNullOrEmpty(policy.Error.ErrorMessage))
        //                policy = (Policy)IL.ProposalIssuance(policy);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        log4net.GlobalContext.Properties["ErrorCode"] = policy.Error.ErrorCode = Codes.GetErrorCode();
        //        Logger.Error(e);
        //        policy.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + policy.Error.ErrorCode + ". Sorry for the inconvenience caused";
        //    }

        //}
        //public void InvokeILWorkFlowAck(AIA.Life.Models.Policy.Policy policy)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(policy.ProposalNo))
        //        {
        //            policy = (Policy)IL.WorkflowAck(policy);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        log4net.GlobalContext.Properties["ErrorCode"] = policy.Error.ErrorCode = Codes.GetErrorCode();
        //        Logger.Error(e);
        //        policy.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + policy.Error.ErrorCode + ". Sorry for the inconvenience caused";
        //    }

        //}
        //public void InvokeILModifyProposalForExtras(AIA.Life.Models.Policy.Policy policy)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(policy.ProposalNo))
        //        {
        //            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
        //            string tempQuoteNo = policy.QuoteNo;
        //            policy = new Policy();
        //            policy.QuoteNo = tempQuoteNo;
        //            policy.ProposalFetch = true;
        //            policy = objPolicyLogic.FetchProposalInfo(policy);
        //            //policy = (Policy)IL.BizDate(policy);
        //            for (int i = 0; i < policy.objMemberDetails.Count; i++)
        //            {
        //                if (!string.IsNullOrEmpty(policy.objMemberDetails[i].ClientCode))
        //                {
        //                    policy.objMemberDetails[i].ProposalNo = policy.ProposalNo;
        //                    policy.objMemberDetails[i].PolicyTerm = Convert.ToInt32(policy.PolicyTerm);
        //                    policy.objMemberDetails[i].PremiumTerm = Convert.ToInt32(policy.PaymentTerm);
        //                    policy.objMemberDetails[i].PensionTerm = Convert.ToInt32(policy.SmartPensionReceivingPeriod);
        //                    policy.objMemberDetails[i].MonthlySavingBenifit = Convert.ToInt32(policy.SmartPensionMonthlyIncome);
        //                    policy.objMemberDetails[i].Deductible = policy.Deductible;
        //                    policy.objMemberDetails[i].MaturityBenefit = policy.MaturityBenefits;
        //                    policy.objMemberDetails[i].LifeNum = "0" + (i + 1).ToString();
        //                    policy.objMemberDetails[i] = (MemberDetails)IL.RefreshRiders(policy.objMemberDetails[i]);
        //                    Thread.Sleep(3000);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        log4net.GlobalContext.Properties["ErrorCode"] = policy.Error.ErrorCode = Codes.GetErrorCode();
        //        Logger.Error(e);
        //    }

        //}
        public ProposalStatus FetchProposalStatus(ProposalStatus proposalStatus)
        {
            try
            {
                AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
                proposalStatus = objPolicyLogic.FetchProposalStatus(proposalStatus);
                return proposalStatus;
            }
            catch (Exception e)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = proposalStatus.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(e);
                proposalStatus.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + proposalStatus.Error.ErrorCode + ". Sorry for the inconvenience caused";
                return proposalStatus;
            }

        }

        public AIA.Life.Models.Common.ProposalDetails LoadProposalDetails(AIA.Life.Models.Common.ProposalDetails objProposal)
        {
            try
            {
                AIA.Life.Data.API.ControllerLogic.Common.CommonBusiness obj = new AIA.Life.Data.API.ControllerLogic.Common.CommonBusiness();
                objProposal = obj.GetProposalDetails(objProposal);
                return objProposal;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = objProposal.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(ex);
                objProposal.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + objProposal.Error.ErrorCode + ". Sorry for the inconvenience caused";
                return objProposal;
            }
        }
        [HttpPost]
        public AIA.Life.Models.Common.AppVersion GetLatestVersion(AIA.Life.Models.Common.AppVersion objVersion)
        {
            try
            {
                AIA.Life.Data.API.ControllerLogic.Common.CommonBusiness obj = new AIA.Life.Data.API.ControllerLogic.Common.CommonBusiness();
                objVersion = obj.GetLatestVersion(objVersion);
                return objVersion;
            }
            catch (Exception ex)
            {
                objVersion.Error = new Error();
                log4net.GlobalContext.Properties["ErrorCode"] = objVersion.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(ex);
                objVersion.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + objVersion.Error.ErrorCode + ". Sorry for the inconvenience caused";
                return objVersion;
            }
        }

        public AIA.Life.Models.Common.AppVersion UpdateLatestVersion(AIA.Life.Models.Common.AppVersion objVersion)
        {
            try
            {
                AIA.Life.Data.API.ControllerLogic.Common.CommonBusiness obj = new AIA.Life.Data.API.ControllerLogic.Common.CommonBusiness();
                objVersion = obj.UpdateLatestVersion(objVersion);
                return objVersion;
            }
            catch (Exception ex)
            {
                objVersion.Error = new Error();
                log4net.GlobalContext.Properties["ErrorCode"] = objVersion.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(ex);
                objVersion.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + objVersion.Error.ErrorCode + ". Sorry for the inconvenience caused";
                return objVersion;
            }
        }
        public TransactLog FetchTraceID(TransactLog objTransactLog)
        {
            PolicyLogic objLogic = new PolicyLogic();
            CommonBusiness objcommonBusiness = new CommonBusiness();
            string TraceID = objcommonBusiness.GenerateTraceNumber("MAPP");
            objLogic.InsertIntoAPPAuthenticationDetails(TraceID, objTransactLog.UserID, objTransactLog.UserName);
            return objTransactLog;
        }
        public TransactLog Logout(TransactLog transactLog)
        {
            PolicyLogic objLogic = new PolicyLogic();
            string Result = objLogic.LogOutAPPAuthenticationDetails(transactLog.SerivceTraceID, transactLog.UserID, transactLog.UserName);
            transactLog.Message = Result;
            return transactLog;
        }


        #region OCR



        [HttpPost]
        public OCRResponse gooleVisionTextDecoderApi(OCRResponse objResp)
        {
            try
            {
                WriteError.WriteLog("Policy Controller", "Start", "dll");
                WriteError.WriteLog(objResp.UserName, "Before converting to byte", "");
                byte[] fileData = Convert.FromBase64String(objResp.Filedata);
                WriteError.WriteLog(objResp.UserName, fileData.ToString(), "After converting to byte");
                string googleVisionKey = System.Configuration.ConfigurationManager.AppSettings["GoogleVisionKeyFile"].ToString();
                if (fileData != null && objResp.DocType != null)
                {
                    WriteError.WriteLog(googleVisionKey, fileData.Length.ToString(), "");
                    OCRResult res = GetText(fileData, "", "en", "LABEL_DETECTION");
                    WriteError.WriteLog(res.WEB_DETECTION, res.Text_Detection, "");

                    objResp = NICValidation(res, objResp.DocType);
                }
                else
                {
                    objResp = new OCRResponse();
                    objResp.ErrorMessage = "Please upload a valid document respective to document type!";
                    objResp.ErrorCode = 1;
                    objResp.NIC_Number = null;
                }
                if (!string.IsNullOrEmpty(objResp.NIC_Number))
                {
                    objResp.NIC_Number = objResp.NIC_Number.Replace(" ", "").ToUpper();
                }
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
            }
            return objResp;

        }
        public OCRResponse NICValidation(OCRResult res, string DocType)
        {
            WriteError.WriteLog("NIC Validation", "Start", "");
            OCRResponse objResp = new OCRResponse();
            string NicNumber = null;
            bool isValid = false;
            try
            {
                if (res.ErrorMessage == null)
                {
                    string BestGuessLabel = res.WEB_DETECTION.ToUpper();
                    string txtValue = res.Text_Detection.ToUpper();
                    switch (DocType)
                    {
                        case "ID":
                            if (txtValue.Contains("SRILANKA NATIONAL IDENTITY CARD") ||
                            txtValue.Contains("NATIONAL IDENTITY") ||
                            BestGuessLabel.Contains("IDENTITY") ||
                            BestGuessLabel.Contains("IDENTITY DOCUMENT") ||
                            BestGuessLabel.Contains("SRI LANKA NATIONAL IDENTITY CARD") ||
                            BestGuessLabel.Contains("NATIONAL IDENTITY") ||
                            BestGuessLabel.Contains("SRI LANKAN IDENTITY CARD") ||
                            BestGuessLabel.Contains("NIC SRI LANKA"))
                            {
                                isValid = true;
                            }
                            break;
                        case "DL":
                            if (BestGuessLabel.Contains("SRI LANKA NEW DRIVING LICENCE") ||
                            BestGuessLabel.Contains("DRIVING LICENCE") ||
                            BestGuessLabel.Contains("DRIVING") ||
                            BestGuessLabel.Contains("DRIVER'S LICENCE") ||
                            txtValue.Contains("DRIVING LICENCE") ||
                            txtValue.Contains("SRI LANKA DRIVING LICENCE") ||
                            BestGuessLabel.Contains("SRI LANKA TRANSPORT BOARD"))
                            {
                                isValid = true;

                            }
                            break;
                        case "Passport":
                            if (BestGuessLabel.Contains("PASSPORT") ||
                            txtValue.Contains("PASSPORT") ||
                            txtValue.Contains("SRI LANKA"))
                            {
                                isValid = true;
                            }
                            break;
                    }
                    if (isValid)
                        NicNumber = DataTypeConvExtention.RegexNic(txtValue);
                }

                if (!string.IsNullOrEmpty(NicNumber))
                {
                    objResp = new OCRResponse();
                    objResp.NIC_Number = NicNumber;
                    objResp.ErrorMessage = "Success";
                    objResp.ErrorCode = 0;
                }
                else
                {
                    objResp = new OCRResponse();
                    objResp.ErrorMessage = res.ErrorMessage;
                    objResp.ErrorCode = 1;
                }

            }
            catch (Exception ex)
            {
                objResp = new OCRResponse();
                NicNumber = ex.Message + " Inner Exception: " + ex.InnerException;//Error in fetching NIC Number
                objResp.ErrorMessage = ex.Message + " Inner Exception: " + ex.InnerException;
            }
            return objResp;
        }
        //public OCRResponse NICValidation(OCRResult res, string DocType)
        //{
        //    WriteError.WriteLog("NIC Validation", "Start", "");
        //    OCRResponse objResp = new OCRResponse();
        //    string NicNumber = null;
        //    bool isValid = false;
        //    try
        //    {
        //        if (res.ErrorMessage == null)
        //        {
        //            string BestGuessLabel = res.WEB_DETECTION;
        //            string txtValue = res.Text_Detection.ToUpper();
        //            if (DocType == "ID")
        //            {


        //                if (res.Text_Detection.ToUpper().Contains("SRILANKA NATIONAL IDENTITY CARD") ||
        //                    res.Text_Detection.ToUpper().Contains("NATIONAL IDENTITY") ||
        //                    BestGuessLabel.ToUpper().Contains("IDENTITY") ||
        //                    BestGuessLabel.ToUpper().Contains("IDENTITY DOCUMENT") ||
        //                    BestGuessLabel.ToUpper().Contains("SRI LANKA NATIONAL IDENTITY CARD") ||
        //                    BestGuessLabel.ToUpper().Contains("NATIONAL IDENTITY") ||
        //                    BestGuessLabel.ToUpper().Contains("SRI LANKAN IDENTITY CARD") ||
        //                    BestGuessLabel.ToUpper().Contains("NIC SRI LANKA"))
        //                {
        //                    //if (txtValue.Contains("SRILANKA NATIONAL IDENTITY CARD") || 
        //                    //    txtValue.Contains("NATIONAL IDENTITY"))
        //                    //{
        //                    isValid = true;
        //                    var resoldNic = Regex.Matches(res.Text_Detection.ToString(), "(\\d{9,9})?\\-?\\d{9} +v");

        //                    if (resoldNic.Count == 1)
        //                    {
        //                        NicNumber = resoldNic[0].Value;
        //                    }
        //                    else
        //                    {
        //                        //For New Id Cards
        //                        string res1 = res.Text_Detection.ToString().Replace("\r\n", "");
        //                        var resNewNic = Regex.Matches(res1.ToString(), "(?<![\\w\\d])No.:(?![\\w\\d]) ([0-9]{12})");
        //                        if (resNewNic.Count == 1)
        //                        {
        //                            var result = Regex.Match(resNewNic[0].Value, @"\d+").Value;
        //                            NicNumber = result;

        //                        }
        //                        string pattern = @"No([0-9]{12})";
        //                        //RegexOptions options = RegexOptions.Multiline;
        //                        foreach (Match m in Regex.Matches(res1, pattern))
        //                        {
        //                            var result = Regex.Match(m.Value, @"\d+").Value;
        //                            NicNumber = result;
        //                            break;
        //                        }
        //                        string pattern1 = @"([0-9]{12})";
        //                        //RegexOptions options = RegexOptions.Multiline;
        //                        foreach (Match m in Regex.Matches(res1, pattern1))
        //                        {
        //                            var result = Regex.Match(m.Value, @"\d+").Value;
        //                            NicNumber = result;
        //                            break;
        //                        }

        //                    }

        //                    //}
        //                    //else if (txtValue != null)
        //                    //{
        //                    //    string[] stringSeparators = new string[] { "\r\n" };
        //                    //    string[] lines = res.Text_Detection.Split(stringSeparators, StringSplitOptions.None);
        //                    //    string Nic = "";
        //                    //    foreach (var x in lines)
        //                    //    {
        //                    //        Nic = x;
        //                    //        if (Nic.Contains(" "))
        //                    //        {
        //                    //            Nic = Nic.Replace(" ", "");
        //                    //        }
        //                    //        var resoldNic = Regex.Matches(Nic.ToString(), "([0-9]{9})[vV]");
        //                    //        var resoldNic1 = Regex.Matches(Nic.ToString(), "([0-9]{9})V");

        //                    //        if (resoldNic.Count == 1 || resoldNic1.Count == 1)
        //                    //        {
        //                    //            NicNumber = resoldNic.Count == 1 ? resoldNic[0].Value : resoldNic1.Count == 1 ? resoldNic1[0].Value : "";
        //                    //            break;
        //                    //        }
        //                    //    }

        //                    //    isValid = true;
        //                    //}
        //                    //else
        //                    //{
        //                    //    isValid = false;
        //                    //}

        //                }

        //            }
        //            else if (DocType == "DL")
        //            {

        //                if (BestGuessLabel.ToUpper().Contains("SRI LANKA NEW DRIVING LICENCE") ||
        //                    BestGuessLabel.ToUpper().Contains("DRIVING LICENCE") ||
        //                    BestGuessLabel.ToUpper().Contains("DRIVING") ||
        //                    BestGuessLabel.ToUpper().Contains("DRIVER'S LICENCE") ||
        //                    txtValue.ToUpper().Contains("DRIVING LICENCE") ||
        //                    txtValue.ToUpper().Contains("SRI LANKA DRIVING LICENCE") ||
        //                    BestGuessLabel.ToUpper().Contains("SRI LANKA TRANSPORT BOARD"))
        //                {
        //                    //if ((txtValue.ToUpper().Contains("DEMOCRATIC SOCIALIST REPUBLIC OF SRI LANKA") || 
        //                    //    txtValue.ToUpper().Contains("DRIVING LICENCE")) || 
        //                    //    txtValue.ToUpper().Contains("SRI LANKA DRIVING LICENCE") || 
        //                    //    txtValue.ToUpper().Contains("SRI LANKA NEW DRIVING LICENCE") || 
        //                    //    (txtValue.ToUpper().Contains("OTHER CLASSES OF MOTOR VEHICLES")) || (txtValue.ToUpper().Contains("COMMISSIONER OF MOTOR TRAFFIC")))
        //                    //{
        //                    isValid = true;
        //                    string[] stringSeparators = new string[] { "\n" };
        //                    string[] lines = txtValue.Split(stringSeparators, StringSplitOptions.None);
        //                    //string ValidNic;
        //                    //if (lines[4].Contains("."))
        //                    //{
        //                    //    ValidNic = lines[4].Split('.')[1].ToString();
        //                    //}
        //                    //else if (lines[2].Contains(":"))
        //                    //{
        //                    //    ValidNic = lines[2].Split(':')[1].ToString();
        //                    //}


        //                    var resoldNic = Regex.Matches(txtValue.ToString(), "(\\d{9,9})?\\-?\\d{9} +v");
        //                    var resoldNic1 = Regex.Matches(txtValue.ToString(), "(\\d{9,9})?\\-?\\d{9} +V");
        //                    if (resoldNic.Count == 1 || resoldNic1.Count == 1)
        //                    {
        //                        NicNumber = resoldNic.Count == 1 ? resoldNic[0].Value : resoldNic1.Count == 1 ? resoldNic1[0].Value : "";

        //                    }
        //                    else
        //                    {
        //                        //For New Id Cards
        //                        string res1 = txtValue.ToString().Replace("\r\n", "");
        //                        var resNewNic = Regex.Matches(res1.ToString(), "(?<![\\w\\d])No.:(?![\\w\\d]) ([0-9]{12})");
        //                        if (resNewNic.Count == 1)
        //                        {
        //                            var result = Regex.Match(resNewNic[0].Value, @"\d+").Value;
        //                            NicNumber = result;

        //                        }
        //                        string pattern = @"No([0-9]{12})";
        //                        //RegexOptions options = RegexOptions.Multiline;
        //                        foreach (Match m in Regex.Matches(res1, pattern))
        //                        {
        //                            var result = Regex.Match(m.Value, @"\d+").Value;
        //                            NicNumber = result;
        //                            break;
        //                        }

        //                    }

        //                    //}
        //                    //else
        //                    //{
        //                    //    isValid = false;
        //                    //}
        //                }
        //            }
        //            else if (DocType == "Passport")
        //            {
        //                if (BestGuessLabel.Contains("PASSPORT") ||
        //                    res.Text_Detection.ToString().Contains("PASSPORT") ||
        //                    res.Text_Detection.ToString().Contains("SRI LANKA"))
        //                {


        //                    //if (txtValue.Contains("PASSPORT") && txtValue.Contains("SRI LANKA"))
        //                    //{
        //                    isValid = true;
        //                    string[] stringSeparators = new string[] { "\r\n" };
        //                    string[] lines = txtValue.Split(stringSeparators, StringSplitOptions.None);
        //                    string ValidNic = lines[6].ToString();
        //                    if (ValidNic.Length == 9)
        //                    {
        //                        ValidNic = ValidNic + "V";
        //                    }
        //                    var resoldNic = Regex.Matches(txtValue.ToString(), "(\\d{9,9})?\\-?\\d{9} +v");
        //                    var resoldNic1 = Regex.Matches(txtValue.ToString(), "(\\d{9,9})?\\-?\\d{9} +V");
        //                    var resoldNic2 = Regex.Matches(ValidNic.ToString(), "([0-9]{9})V");
        //                    var resoldNic3 = Regex.Matches(txtValue.ToString(), "([0-9]{9})[vV]");
        //                    if (resoldNic.Count == 1 || resoldNic1.Count == 1 || resoldNic2.Count == 1 || resoldNic3.Count >= 1)
        //                    {
        //                        NicNumber = resoldNic.Count == 1 ? resoldNic[0].Value : resoldNic1.Count == 1 ? resoldNic1[0].Value : resoldNic2.Count == 1 ? resoldNic2[0].Value : resoldNic3.Count >= 1 ? resoldNic3[0].Value : "";

        //                    }
        //                    else
        //                    {
        //                        //For New Id Cards
        //                        string res1 = txtValue.ToString().Replace("\n", "");
        //                        var resNewNic = Regex.Matches(res1.ToString(), "(?<![\\w\\d])No.:(?![\\w\\d]) ([0-9]{12})");
        //                        if (resNewNic.Count == 1)
        //                        {
        //                            var result = Regex.Match(resNewNic[0].Value, @"\d+").Value;
        //                            NicNumber = result;

        //                        }
        //                        string pattern = @"No([0-9]{12})";
        //                        //RegexOptions options = RegexOptions.Multiline;
        //                        foreach (Match m in Regex.Matches(res1, pattern))
        //                        {
        //                            var result = Regex.Match(m.Value, @"\d+").Value;
        //                            NicNumber = result;
        //                            break;
        //                        }
        //                        // }
        //                    }
        //                }
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(NicNumber))
        //        {
        //            objResp = new OCRResponse();
        //            objResp.NIC_Number = NicNumber;
        //            objResp.ErrorMessage = "Success";
        //            objResp.ErrorCode = 0;
        //        }
        //        else
        //        {
        //            objResp = new OCRResponse();
        //            objResp.ErrorMessage = res.ErrorMessage;
        //            objResp.ErrorCode = 1;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        objResp = new OCRResponse();
        //        NicNumber = ex.Message + " Inner Exception: " + ex.InnerException;//Error in fetching NIC Number
        //        objResp.ErrorMessage = ex.Message + " Inner Exception: " + ex.InnerException;
        //    }
        //    return objResp;
        //}

        private static GoogleCredential CreateCredential()
        {
            string googleVisionKey = System.Configuration.ConfigurationManager.AppSettings["GoogleVisionKeyFile"].ToString();
            // this is the place to enter your own google API key (= json file). The app crashes without valid key. 
            using (var stream = new FileStream(googleVisionKey, FileMode.Open, FileAccess.Read))
            {
                string[] scopes = { VisionService.Scope.CloudPlatform };
                var credential = GoogleCredential.FromStream(stream);
                credential = credential.CreateScoped(scopes);
                return credential;
            }
        }

        private static VisionService CreateService(GoogleCredential credential)
        {
            var service = new VisionService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
                GZipEnabled = true,
                HttpClientFactory = new ProxySupportedHttpClientFactory()
            });

            return service;
        }

        public static string ApplicationName { get { return "Ocr"; } }

        public class OCRResult
        {
            public string Text_Detection { get; set; }
            public string DOCUMENT_TEXT_DETECTION { get; set; }
            public string FACE_DETECTION { get; set; }
            public string LOGO_DETECTION { get; set; }
            public string LABEL_DETECTION { get; set; }
            public string LANDMARK_DETECTION { get; set; }
            public string WEB_DETECTION { get; set; }
            public string IMAGE_PROPERTIES { get; set; }
            public string ErrorMessage { get; set; }
        }

        public string JsonResult { get; set; }
        public string TextResult { get; set; }
        public string Error { get; set; }
        public OCRResult GetText(byte[] fileBytes, string imgPath, string language, string type)
        {
            OCRResult Res = new OCRResult();
            TextResult = JsonResult = "";
            try
            {
                var credential = CreateCredential();
                var service = CreateService(credential);
                service.HttpClient.Timeout = new TimeSpan(1, 1, 1);
                //byte[] file = File.ReadAllBytes(imgPath);

                BatchAnnotateImagesRequest batchRequest = new BatchAnnotateImagesRequest();
                batchRequest.Requests = new List<AnnotateImageRequest>();
                batchRequest.Requests.Add(new AnnotateImageRequest()
                {
                    Features = new List<Feature>() { new Feature() { Type = "TEXT_DETECTION", MaxResults = 1 }, new Feature() { Type = "LABEL_DETECTION", MaxResults = 1 }, new Feature() { Type = "LOGO_DETECTION", MaxResults = 1 }, new Feature() { Type = "WEB_DETECTION", MaxResults = 1 }, },
                    ImageContext = new ImageContext() { LanguageHints = new List<string>() { language } },
                    Image = new Image() { Content = Convert.ToBase64String(fileBytes) }
                });

                var annotate = service.Images.Annotate(batchRequest);
                WriteError.WriteLog("Before Excecution of Batch annotate", "Start", "");
                BatchAnnotateImagesResponse batchAnnotateImagesResponse = annotate.Execute();
                WriteError.WriteLog("After Excecution of Batch annotate", "Start", "");

                if (batchAnnotateImagesResponse.Responses.Any())
                {
                    AnnotateImageResponse annotateImageResponse = batchAnnotateImagesResponse.Responses[0];
                    if (annotateImageResponse.Error != null)
                    {
                        if (annotateImageResponse.Error.Message != null)
                            Res.ErrorMessage = annotateImageResponse.Error.Message;
                    }
                    else
                    {
                        if (annotateImageResponse.TextAnnotations != null && annotateImageResponse.TextAnnotations.Any())
                        {
                            Res.Text_Detection = annotateImageResponse.TextAnnotations[0].Description.Replace("\n", "\r\n");
                            Res.LABEL_DETECTION = annotateImageResponse.LabelAnnotations[0].Description;
                            Res.WEB_DETECTION = annotateImageResponse.WebDetection.BestGuessLabels[0].Label;
                            Res.ErrorMessage = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError.WriteLog("After Excecution of Batch annotate", "Catch", ex.Message);
            }


            //return "";
            return Res;
        }

        #endregion

        public class ProxySupportedHttpClientFactory : Google.Apis.Http.HttpClientFactory
        {
            protected override HttpMessageHandler CreateHandler(CreateHttpClientArgs args)
            {
                var proxy = new WebProxy("http://10.7.192.136:10938", true, null, null);
                var webRequestHandler = new HttpClientHandler()
                {
                    UseProxy = true,
                    Proxy = proxy,
                    UseCookies = false
                };
                return webRequestHandler;
            }
        }
        public void CreateServiceLog(TpServiceLog obj)
        {
            ServiceLog.CreateServiceLog(obj);
        }

        public void UploadLdmsDocuments(Policy policy)
        {
            try
            {
                PolicyLogic policyLogic = new PolicyLogic();
                var docs = policyLogic.GetLdmsDocuments(policy);
                foreach (var doc in docs)
                {
                    try
                    {
                        //LdmsClient.UploadsFTP(doc);
                    }
                    catch (Exception ex)
                    {
                        log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                        Logger.Error(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
            }
        }
        public void UploadDocumentsLDMS(LdmsDocuments documents)
        {
            try
            {
                //LdmsClient.UploadsFTP(documents);
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
            }
        }
        public SARFALDetails FetchSarAndFalDetails(SARFALDetails sARFAL)
        {
            try
            {
                AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();

                sARFAL = objPolicyLogic.FetchSarAndFalDetails(sARFAL);
                return sARFAL;
            }
            catch (Exception e)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = sARFAL.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(e);
                sARFAL.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + sARFAL.Error.ErrorCode + ". Sorry for the inconvenience caused";
                return sARFAL;
            }

        }
        public void PostPolicyIssuanceTriggers(PaymentModel objPaymentModel)
        {
            try
            {
                AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
                objPolicyLogic.PostPolicyIssuanceTriggers(objPaymentModel);
                Policy policy = new Policy();
                policy.UserName = objPaymentModel.UserName;
                policy.ProposalNo = objPaymentModel.ProposalNo;
                policy.QuoteNo = objPaymentModel.QuoteNo;
                UploadLdmsDocuments(policy);
            }
            catch (Exception e)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(e);
            }

        }
        public void SendDocumentsEmail(Policy policy)
        {
            PolicyLogic policyLogic = new PolicyLogic();
            policyLogic.SendDocumentsEmail(policy);
        }
        public AuthorizeUser CheckAuthorisation(AuthorizeUser authorizeUser)
        {
            CommonBusiness commonBusiness = new CommonBusiness();
            return commonBusiness.CheckAuthorisation(authorizeUser);
        }
        public Policy SubmitCounterOfferQuote(Policy policy)
        {
            PolicyLogic policyLogic = new PolicyLogic();
            return policyLogic.SubmitCounterOfferQuote(policy);
        }
        public LifeAssuredAge CheckAgeChangeQuoteMembers(LifeAssuredAge lifeAssuredAge)
        {
            try
            {
                CommonBusiness common = new CommonBusiness();
                return common.CheckAgeChangeQuoteMembers(lifeAssuredAge);
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                return lifeAssuredAge;
            }
        }
        public Policy SaveBeforeClaGenerateQuote(Policy policy)
        {
            PolicyLogic policyLogic = new PolicyLogic();
            return policyLogic.SaveBeforeClaGenerateQuote(policy);
        }
        public void CallBizDate(Policy policy)
        {
            //IL.BizDate(policy);
        }
        //public Policy InvokeILClientCreation(AIA.Life.Models.Policy.Policy policy)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(policy.ProposalNo))
        //        {
        //            AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
        //            int updateIndex = policy.objMemberDetails.FindIndex(a => a.IsUpdate == true);
        //            policy = objPolicyLogic.SaveProposal(policy);
        //            string tempQuoteNo = policy.QuoteNo;
        //            policy = new Policy();
        //            policy.QuoteNo = tempQuoteNo;
        //            policy.ProposalFetch = true;
        //            policy = objPolicyLogic.FetchProposalInfo(policy);
        //            string mainClientCode = string.Empty;
        //            AVOAIALifeEntities entity = new AVOAIALifeEntities();

        //            #region Client creation

        //            #region If Proposer not same as main life
        //            if (policy.objProspectDetails.IsproposerlifeAssured == false)
        //            {
        //                var names = string.IsNullOrEmpty(policy.objProspectDetails.FirstName) != true ? policy.objProspectDetails.FirstName.Split(' ') : null;
        //                if (names != null)
        //                {
        //                    for (int j = 0; j < names.Length; j++)
        //                    {
        //                        if (j == 0)
        //                            policy.objProspectDetails.NameWithInitial = names[j].Substring(0, 1);
        //                        else
        //                            policy.objProspectDetails.NameWithInitial += " " + names[j].Substring(0, 1);
        //                    }
        //                }
        //                policy.objProspectDetails.Language = policy.PreferredLanguage;

        //                if (policy.objProspectDetails.Nationality == "USA")
        //                    policy.objProspectDetails.IsUSCitizen = true;

        //                if (policy.objProspectDetails.OccupationID == 0)
        //                    policy.objProspectDetails.OccupationID = null;
        //                if (string.IsNullOrEmpty(policy.objProspectDetails.NewNICNO))
        //                    policy.objProspectDetails.Nationality = "SLC";
        //                if (string.IsNullOrEmpty(policy.objProspectDetails.ClientCode))
        //                {
        //                    policy.objProspectDetails = (MemberDetails)IL.ClientEnquiry(policy.objProspectDetails);
        //                    policy.objProspectDetails.Error = new Error();
        //                }
        //                if (!string.IsNullOrEmpty(policy.objProspectDetails.ClientCode))
        //                {
        //                    policy.objProspectDetails = (MemberDetails)IL.ModifyClient(policy.objProspectDetails);
        //                }
        //                if (string.IsNullOrEmpty(policy.objProspectDetails.ClientCode))
        //                {
        //                    policy.objProspectDetails = (MemberDetails)IL.ClientCreation(policy.objProspectDetails);
        //                }
        //            }
        //            #endregion

        //            for (int i = 0; i < policy.objMemberDetails.Count; i++)
        //            {
        //                if (policy.objMemberDetails[i].RelationShipWithPropspect == "267")
        //                    mainClientCode = policy.objMemberDetails[i].ClientCode;
        //                if (i == updateIndex)
        //                {
        //                    var names = policy.objMemberDetails[i].FirstName != null ? policy.objMemberDetails[i].FirstName.Split(' ') : null;
        //                    if (names != null)
        //                    {
        //                        for (int j = 0; j < names.Length; j++)
        //                        {
        //                            if (j == 0)
        //                                policy.objMemberDetails[i].NameWithInitial = names[j].Substring(0, 1);
        //                            else
        //                                policy.objMemberDetails[i].NameWithInitial += " " + names[j].Substring(0, 1);
        //                        }
        //                    }
        //                    policy.objMemberDetails[i].Language = policy.PreferredLanguage;

        //                    if (string.IsNullOrEmpty(policy.objMemberDetails[i].Language))
        //                        policy.objMemberDetails[i].Language = "E";

        //                    if (policy.objMemberDetails[i].Nationality == "USA")
        //                        policy.objMemberDetails[i].IsUSCitizen = true;
        //                    if (policy.objMemberDetails[i].RelationShipWithPropspect == "271" ||
        //                        policy.objMemberDetails[i].RelationShipWithPropspect == "270" ||
        //                        policy.objMemberDetails[i].RelationShipWithPropspect == "269")
        //                    {
        //                        policy.objMemberDetails[i].objLifeStyleQuetions.Height = 33;
        //                        policy.objMemberDetails[i].objLifeStyleQuetions.Weight = 3;
        //                        policy.objMemberDetails[i].MaritialStatus = "S";
        //                    }
        //                    if (policy.objMemberDetails[i].OccupationID == 0)
        //                        policy.objMemberDetails[i].OccupationID = null;
        //                    if (string.IsNullOrEmpty(policy.objMemberDetails[i].NewNICNO))
        //                        policy.objMemberDetails[i].Nationality = "SLC";
        //                    if (policy.objMemberDetails[i].RelationShipWithPropspect == "268" || policy.objMemberDetails[i].RelationShipWithPropspect == "267")
        //                    {
        //                        if (string.IsNullOrEmpty(policy.objMemberDetails[i].ClientCode))
        //                        {
        //                            policy.objMemberDetails[i] = (MemberDetails)IL.ClientEnquiry(policy.objMemberDetails[i]);
        //                            policy.objMemberDetails[i].Error = new Error();
        //                        }
        //                    }
        //                    if (!string.IsNullOrEmpty(policy.objMemberDetails[i].ClientCode))
        //                    {
        //                        policy.objMemberDetails[i] = (MemberDetails)IL.ModifyClient(policy.objMemberDetails[i]);
        //                    }
        //                    if (string.IsNullOrEmpty(policy.objMemberDetails[i].ClientCode))
        //                    {
        //                        policy.objMemberDetails[i] = (MemberDetails)IL.ClientCreation(policy.objMemberDetails[i]);
        //                    }

        //                    if (!string.IsNullOrEmpty(policy.objMemberDetails[i].ClientCode))
        //                    {
        //                        decimal membId = policy.objMemberDetails[i].MemberID;
        //                        var member = entity.tblPolicyMemberDetails.Where(a => a.MemberID == membId).FirstOrDefault();
        //                        member.ClientCode = policy.objMemberDetails[i].ClientCode;
        //                        entity.SaveChanges();
        //                    }
        //                }
        //            }

        //            #endregion

        //            #region Client Relation Creation
        //            List<ClientRelation> clientRelations = new List<ClientRelation>();
        //            if (policy.objProspectDetails.IsproposerlifeAssured == false)
        //            {
        //                if (!string.IsNullOrEmpty(policy.objProspectDetails.ClientCode))
        //                {
        //                    ClientRelation clientRelation = new ClientRelation();
        //                    clientRelation.ClientCode = policy.objProspectDetails.ClientCode;
        //                    clientRelation.Relation = policy.objProspectDetails.RelationShipWithPropspect;
        //                    clientRelations.Add(clientRelation);
        //                }
        //            }
        //            for (int i = 0; i < policy.objMemberDetails.Count; i++)
        //            {
        //                ClientRelation clientRelation = new ClientRelation();
        //                if (policy.objMemberDetails[i].RelationShipWithPropspect == "268")
        //                {
        //                    if (!string.IsNullOrEmpty(policy.objMemberDetails[i].ClientCode))
        //                    {
        //                        if (policy.objMemberDetails[i].Gender == "F")
        //                            clientRelation.Relation = "WIFE";
        //                        else
        //                            clientRelation.Relation = "HUSB";
        //                        clientRelation.ClientCode = policy.objMemberDetails[i].ClientCode;
        //                    }
        //                }
        //                if (policy.objMemberDetails[i].RelationShipWithPropspect == "271" ||
        //                    policy.objMemberDetails[i].RelationShipWithPropspect == "270" ||
        //                    policy.objMemberDetails[i].RelationShipWithPropspect == "269")
        //                {
        //                    if (!string.IsNullOrEmpty(policy.objMemberDetails[i].ClientCode))
        //                    {
        //                        if (policy.objMemberDetails[i].Gender == "F")
        //                            clientRelation.Relation = "DAUG";
        //                        else
        //                            clientRelation.Relation = "SON";
        //                        clientRelation.ClientCode = policy.objMemberDetails[i].ClientCode;
        //                    }
        //                }
        //                if (!string.IsNullOrEmpty(clientRelation.ClientCode))
        //                    clientRelations.Add(clientRelation);
        //            }

        //            if (clientRelations.Count() > 0)
        //            {
        //                MemberDetails memberDetails = new MemberDetails();
        //                memberDetails.ClientCode = mainClientCode;
        //                memberDetails = (MemberDetails)IL.ClientRelationshipEnquiry(memberDetails);
        //                List<ClientRelation> nonExistingRel = new List<ClientRelation>();
        //                foreach (var item in clientRelations)
        //                {
        //                    foreach (var e in memberDetails.ClientRelations)
        //                    {
        //                        if (item.ClientCode != e.ClientCode && item.Relation != e.Relation)
        //                        {
        //                            if (nonExistingRel
        //                        .Where(a => a.ClientCode == e.ClientCode && a.Relation == e.Relation)
        //                        .FirstOrDefault() == null)
        //                                nonExistingRel.Add(e);
        //                        }
        //                    }
        //                }
        //                memberDetails.ClientRelations = new List<ClientRelation>();
        //                memberDetails.ClientRelations = clientRelations;
        //                memberDetails.ClientRelations.AddRange(nonExistingRel);
        //                memberDetails.ClientRelations = memberDetails.ClientRelations.GroupBy(a => new { a.ClientCode, a.Relation }).Select(g => g.FirstOrDefault()).Distinct().ToList();
        //                memberDetails = (MemberDetails)IL.ClientRelationshipCreation(memberDetails);
        //            }

        //            #endregion

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        log4net.GlobalContext.Properties["ErrorCode"] = policy.Error.ErrorCode = Codes.GetErrorCode();
        //        Logger.Error(e);
        //        policy.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + policy.Error.ErrorCode + ". Sorry for the inconvenience caused";
        //    }
        //    return policy;
        //}

        //public Policy InvokeILClientCreationForBeneficiary(Policy policy)
        //{
        //    try
        //    {
        //        AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Policy.PolicyLogic();
        //        policy = objPolicyLogic.SaveProposal(policy);
        //        string tempQuoteNo = policy.QuoteNo;
        //        policy = new Policy();
        //        policy.QuoteNo = tempQuoteNo;
        //        policy.ProposalFetch = true;
        //        policy = objPolicyLogic.FetchProposalInfo(policy);

        //        string mainClientCode = policy.objMemberDetails.Where(a => a.RelationShipWithPropspect == "267").Select(a => a.ClientCode).FirstOrDefault();

        //        AVOAIALifeEntities entity = new AVOAIALifeEntities();

        //        if (policy.objNomineeDetails.Count() > 0)
        //        {
        //            for (int i = 0; i < policy.objNomineeDetails.Count(); i++)
        //            {
        //                MemberDetails nominee = new MemberDetails();
        //                nominee.ClientCode = policy.objNomineeDetails[i].NomineeClientCode;
        //                nominee.FirstName = policy.objNomineeDetails[i].NomineeName;
        //                nominee.LastName = policy.objNomineeDetails[i].NomineeSurname;
        //                string sal = policy.objNomineeDetails[i].NomineeSalutation;
        //                nominee.SalutationCode = entity.tblMasCommonTypes.Where(a => a.Description == sal && a.MasterType == "Salutation").Select(a => a.Code).FirstOrDefault();
        //                nominee.DateOfBirth = policy.objNomineeDetails[i].NomineeNicNoDOB;
        //                nominee.NewNICNO = policy.objNomineeDetails[i].NomineeNICNo;
        //                var names = policy.objNomineeDetails[i].NomineeName != null ? policy.objNomineeDetails[i].NomineeName.Split(' ') : null;
        //                if (names != null)
        //                {
        //                    for (int j = 0; j < names.Length; j++)
        //                    {
        //                        if (j == 0)
        //                            nominee.NameWithInitial = names[j].Substring(0, 1);
        //                        else
        //                            nominee.NameWithInitial += " " + names[j].Substring(0, 1);
        //                    }
        //                }
        //                else
        //                    nominee.NameWithInitial = policy.objNomineeDetails[i].NomineeName;
        //                nominee.Gender = policy.objNomineeDetails[i].NomineeGender;
        //                nominee.MaritialStatus = policy.objNomineeDetails[i].NomineeMaritalStatus;
        //                nominee.MobileNo = policy.objNomineeDetails[i].NomineeTelephone;
        //                nominee.objCommunicationAddress = policy.objProspectDetails.objCommunicationAddress;
        //                nominee.Gender = policy.objNomineeDetails[i].NomineeGender;
        //                if (string.IsNullOrEmpty(nominee.NewNICNO))
        //                    nominee.Nationality = "SLC";
        //                else
        //                    nominee.Nationality = "SL";
        //                nominee.OccupationID = null;
        //                if (string.IsNullOrEmpty(nominee.ClientCode))
        //                    nominee = (MemberDetails)IL.ClientEnquiry(nominee);
        //                if (string.IsNullOrEmpty(nominee.ClientCode))
        //                {
        //                    nominee = (MemberDetails)IL.ClientCreation(nominee);
        //                }
        //                else
        //                {
        //                    nominee = (MemberDetails)IL.ModifyClient(nominee);
        //                }
        //                policy.objNomineeDetails[i].NomineeClientCode = nominee.ClientCode;
        //                if (!string.IsNullOrEmpty(policy.objNomineeDetails[i].NomineeClientCode))
        //                {
        //                    decimal nomId = policy.objNomineeDetails[i].NomineeDetailsId;
        //                    var nomi = entity.tblPolicyNomineeDetails.Where(a => a.NomineeID == nomId).FirstOrDefault();
        //                    nomi.ClientCode = policy.objNomineeDetails[i].NomineeClientCode;
        //                    entity.SaveChanges();
        //                }
        //            }

        //        }

        //        #region Client Relation Creation
        //        List<ClientRelation> clientRelations = new List<ClientRelation>();
        //        if (policy.objProspectDetails.IsproposerlifeAssured == false)
        //        {
        //            if (!string.IsNullOrEmpty(policy.objProspectDetails.ClientCode))
        //            {
        //                ClientRelation clientRelation = new ClientRelation();
        //                clientRelation.ClientCode = policy.objProspectDetails.ClientCode;
        //                clientRelation.Relation = policy.objProspectDetails.RelationShipWithPropspect;
        //                clientRelations.Add(clientRelation);
        //            }
        //        }
        //        for (int i = 0; i < policy.objMemberDetails.Count; i++)
        //        {
        //            ClientRelation clientRelation = new ClientRelation();
        //            if (policy.objMemberDetails[i].RelationShipWithPropspect == "268")
        //            {
        //                if (!string.IsNullOrEmpty(policy.objMemberDetails[i].ClientCode))
        //                {
        //                    if (policy.objMemberDetails[i].Gender == "F")
        //                        clientRelation.Relation = "WIFE";
        //                    else
        //                        clientRelation.Relation = "HUSB";
        //                    clientRelation.ClientCode = policy.objMemberDetails[i].ClientCode;
        //                }
        //            }
        //            if (policy.objMemberDetails[i].RelationShipWithPropspect == "271" ||
        //                policy.objMemberDetails[i].RelationShipWithPropspect == "270" ||
        //                policy.objMemberDetails[i].RelationShipWithPropspect == "269")
        //            {
        //                if (!string.IsNullOrEmpty(policy.objMemberDetails[i].ClientCode))
        //                {
        //                    if (policy.objMemberDetails[i].Gender == "F")
        //                        clientRelation.Relation = "DAUG";
        //                    else
        //                        clientRelation.Relation = "SON";
        //                    clientRelation.ClientCode = policy.objMemberDetails[i].ClientCode;
        //                }
        //            }
        //            if (!string.IsNullOrEmpty(clientRelation.ClientCode))
        //                clientRelations.Add(clientRelation);
        //        }

        //        if (clientRelations.Count() > 0)
        //        {
        //            MemberDetails memberDetails = new MemberDetails();
        //            memberDetails.ClientCode = mainClientCode;
        //            memberDetails = (MemberDetails)IL.ClientRelationshipEnquiry(memberDetails);
        //            List<ClientRelation> nonExistingRel = new List<ClientRelation>();
        //            foreach (var item in clientRelations)
        //            {
        //                foreach (var e in memberDetails.ClientRelations)
        //                {
        //                    if (item.ClientCode != e.ClientCode && item.Relation != e.Relation)
        //                    {
        //                        if (nonExistingRel
        //                    .Where(a => a.ClientCode == e.ClientCode && a.Relation == e.Relation)
        //                    .FirstOrDefault() == null)
        //                            nonExistingRel.Add(e);
        //                    }
        //                }
        //            }
        //            memberDetails.ClientRelations = new List<ClientRelation>();
        //            memberDetails.ClientRelations = clientRelations;
        //            memberDetails.ClientRelations.AddRange(nonExistingRel);
        //            memberDetails.ClientRelations = memberDetails.ClientRelations.GroupBy(a => new { a.ClientCode, a.Relation }).Select(g => g.FirstOrDefault()).Distinct().ToList();
        //            memberDetails = (MemberDetails)IL.ClientRelationshipCreation(memberDetails);
        //        }

        //        #endregion
        //    }
        //    catch (Exception e)
        //    {
        //        log4net.GlobalContext.Properties["ErrorCode"] = policy.Error.ErrorCode = Codes.GetErrorCode();
        //        Logger.Error(e);
        //    }
        //    return policy;
        //}
    }
    public class WriteError
    {
        public static void WriteLog(string info, string stackTrace, string InnerException)
        {

            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, INDIAN_ZONE);
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = path + "\\" + indianTime.ToString("dd-MM-yy") + ".txt";
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }

            using (StreamWriter w = File.AppendText(path))
            {
                w.WriteLine("\r\nDetails:: ");
                w.WriteLine("{0}", indianTime.ToString());
                w.WriteLine(info);
                w.WriteLine(stackTrace);
                w.WriteLine(InnerException);
                w.Flush();
                w.Close();
            }
        }
    }
}
