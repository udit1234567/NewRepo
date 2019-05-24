using AIA.Life.Business;
using AIA.Life.Business.Policy;
using AIA.Life.Models.Common;
using AIA.Life.Models.Policy;
using AIA.Life.Models.UWDecision;
using AIA.Life.Business.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AIA.Life.Models.Payment;

namespace AIA.Services.API.Controllers.Life
{
    public class LifePolicyApiController : ApiController
    {
        

        public AIA.Life.Models.Policy.Policy LoadPolicyPreviousInsuranceGrid(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            objPolicy = obj.LoadPolicyPreviousInsuranceGrid(objPolicy);
            return objPolicy;
        }

        public AIA.Life.Models.Policy.Policy OccupationQuestions(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            objPolicy = obj.OccupationQuestions(objPolicy);
            return objPolicy;
        }
        public AIA.Life.Models.Policy.Policy LoadMastersRelationship(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            objPolicy = obj.LoadMastersRelationship(objPolicy);
            return objPolicy;
        }
        public AIA.Life.Models.Policy.Policy ResidentialStatusQuestions(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            objPolicy = obj.ResidentialStatusQuestions(objPolicy);
            return objPolicy;
        }
        // For Demo
        public AIA.Life.Models.Policy.UWInbox FetchUWProposals(AIA.Life.Models.Policy.UWInbox objUWInbox)
        {
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            return obj.FetchUWProposals(objUWInbox);

        }

        // Till here
        public AIA.Life.Models.Policy.UWInbox FetchUWProposalCount(AIA.Life.Models.Policy.UWInbox objUWInbox)
        {
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            return obj.FetchUWProposalCount(objUWInbox);
        }
        public AIA.Life.Models.Policy.Policy LoadMasters(AIA.Life.Models.Policy.Policy objpolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            objpolicy = obj.LoadMasters(objpolicy);
            return objpolicy;
        }

        public AIA.Life.Models.Common.Address FillAddressMasterList(AIA.Life.Models.Common.Address objAddress)
        {
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            objAddress = obj.FillAddressMasterList();
            return objAddress;
        }
        public AIA.Life.Models.Policy.Policy LoadMastersForProposal(AIA.Life.Models.Policy.Policy objpolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            objpolicy = obj.LoadMastersForProposal(objpolicy);
            return objpolicy;
        }

        public AIA.Life.Models.Policy.Policy AssuredMemberDetails(AIA.Life.Models.Policy.Policy objpolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            objpolicy = obj.AssuredMemberDetails(objpolicy);
            return objpolicy;
        }

        public AIA.Life.Models.Policy.Policy BenefitDetails(AIA.Life.Models.Policy.Policy objpolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            objpolicy = obj.BenefitDetails(objpolicy);
            return objpolicy;
        }
        public AIA.Life.Models.Policy.Policy SaveProposal(AIA.Life.Models.Policy.Policy objpolicy)
        {
            //string objPolicyProposal = Newtonsoft.Json.JsonConvert.SerializeObject(objpolicy);
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            objpolicy = obj.SaveProposal(objpolicy);
            return objpolicy;
        }
        public AIA.Life.Models.Policy.Policy SendEmailAndSMSNotificationOnSaveProposal(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            return objPolicyBusiness.SendEmailAndSMSNotificationOnSaveProposal(objPolicy);

        }
        public AIA.Life.Models.Policy.Policy LoadProposalInfo(AIA.Life.Models.Policy.Policy objpolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            objpolicy = obj.LoadProposalInfo(objpolicy);
            return objpolicy;
        }

        public AIA.Life.Models.Common.ProposalDetails LoadPreviousPolicyDetails(AIA.Life.Models.Common.ProposalDetails objProposal)
        {
            AIA.Life.Business.Common.CommonBusiness obj = new AIA.Life.Business.Common.CommonBusiness();
            objProposal = obj.GetPolicyDetails(objProposal);
            return objProposal;
        }


        public AIA.Life.Models.Policy.Policy FetchProposalLifeAssuredDetails(AIA.Life.Models.Policy.Policy objpolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            objpolicy = obj.FetchProposalLifeAssuredDetails(objpolicy);
            return objpolicy;
        }

        public AIA.Life.Models.Policy.Policy FetchProposalNomineeDetailsWithMemberDetails(AIA.Life.Models.Policy.Policy objpolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            objpolicy = obj.FetchProposalNomineeDetailsWithMemberDetails(objpolicy);
            return objpolicy;
        }


        //MHD
        public AIA.Life.Models.Policy.Policy LoadMHDProposalInfo(AIA.Life.Models.Policy.Policy objpolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            objpolicy = obj.LoadMHDProposalInfo(objpolicy);
            return objpolicy;
        }
        public AIA.Life.Models.Opportunity.LifeQuote CalculateQuotePremium(AIA.Life.Models.Opportunity.LifeQuote objLifeQuote)
        {
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            objLifeQuote = obj.CalculateQuotePremium(objLifeQuote);
            return objLifeQuote;
        }
        public AIA.Life.Models.Policy.Policy CalculateProposalPremium(AIA.Life.Models.Policy.Policy objLifeProposal)
        {
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            objLifeProposal = obj.CalculateProposalPremium(objLifeProposal);
            return objLifeProposal;
        }
        public AIA.Life.Models.Policy.Policy FetchProposalInfo(AIA.Life.Models.Policy.Policy objpolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            return obj.FetchProposalInfo(objpolicy);

        }
        public AIA.Life.Models.Policy.Policy SaveUWRemarks(AIA.Life.Models.Policy.Policy objpolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            return obj.SaveUWRemarks(objpolicy);

        }
        public AIA.Life.Models.Policy.Policy LoadProposalBenefits(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            return objPolicyBusiness.LoadProposalBenefits(objPolicy);
        }

       
        public ProposalInbox FetchProposalIncompleteDetails(ProposalInbox objProposalData)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            return objPolicyBusiness.FetchProposalIncompleteDetails(objProposalData);
        }

        public ProposalInbox FetchProposalSubmittedDetails(ProposalInbox objProposalData)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            return objPolicyBusiness.FetchProposalSubmittedDetails(objProposalData);
        }

        public UWMemberLevelDeviationStatus UpdateMemberLevelDeviation(UWMemberLevelDeviationStatus objStatus)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            return objPolicyBusiness.UpdateMemberLevelDeviation(objStatus);
        }
        

        public AIA.Life.Models.Policy.Policy TestUWDeviation(AIA.Life.Models.Policy.Policy objpolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            return objPolicyBusiness.TestUWDeviation(objpolicy);
        }

        public MemberDetails FetchLoadigInfo(MemberDetails objMemberDetails)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            return objPolicyBusiness.FetchLoadigInfo(objMemberDetails);
        }
        public AIA.Life.Models.Policy.Policy CounterOfferSubmit(AIA.Life.Models.Policy.Policy objpolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            return objPolicyBusiness.CounterOfferSubmit(objpolicy);        
        }
        [HttpPost]
        public AIA.Life.Models.Policy.Policy SubmitPolicyDocuments(AIA.Life.Models.Policy.Policy objpolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            return objPolicyBusiness.SubmitPolicyDocuments(objpolicy);
        }

        [HttpPost]
        public DeletePolicyDocuments DeletePolicyDocument(DeletePolicyDocuments ObjPolicyDocuments)
        {

            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            return objPolicyBusiness.DeletePolicyDocument(ObjPolicyDocuments);
        }
        public AIA.Life.Models.Reports.UWDecisionReport UWDecisionReport(AIA.Life.Models.Reports.UWDecisionReport objUWDecisionReport)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            return objPolicyBusiness.UWDecisionReport(objUWDecisionReport);
        }
        public MemberLevelDecisions DerivePolicyLevelDecision(MemberLevelDecisions  objMemberDecision)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            return objPolicyBusiness.DerivePolicyLevelDecision(objMemberDecision);
        }

        public Policy CalculateLoadingPremium(Policy objPolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            return objPolicyBusiness.CalculateLoadingPremium(objPolicy);
        }
        public AIA.Life.Models.Policy.Policy SaveLoadingDetails(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            return objPolicyBusiness.SaveLoadingDetails(objPolicy);
        }
        public AIA.Life.Models.Policy.Policy EmailNotificationOnUWDecision(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            return objPolicyBusiness.EmailNotificationOnUWDecision(objPolicy);
        }
        
        ///Services
        ///
        [HttpPost]
        public AIA.Life.Models.Integration.Services.ProposalInfo UpdateProposalInfo(AIA.Life.Models.Integration.Services.ProposalInfo objProposalInfo)
        {
            if (string.IsNullOrEmpty(objProposalInfo.ProposalNo))
            {
                objProposalInfo.Message = "Please Provide Proposal Number.";
                objProposalInfo.Status = "Error";
                return objProposalInfo;
            }
            else
            {
                AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
                return objPolicyBusiness.UpdateUWInfo(objProposalInfo);
            }
      

        }
        public void InvokeILPolicyIssuance(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            objPolicyBusiness.InvokeILPolicyIssuance(objPolicy);
        }
        public void InvokeILModifyProposal(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            objPolicyBusiness.InvokeILModifyProposal(objPolicy);
        }
        public void InvokeILUWApproval(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            objPolicyBusiness.InvokeILUWApproval(objPolicy);
        }
        public void InvokeILWorkFlowAck(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            objPolicyBusiness.InvokeILWorkFlowAck(objPolicy);
        }
        public void InvokeILModifyProposalForExtras(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            objPolicyBusiness.InvokeILModifyProposalForExtras(objPolicy);
        }
        
        public ProposalStatus FetchProposalStatus(ProposalStatus proposalStatus)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            return objPolicyBusiness.FetchProposalStatus(proposalStatus);
        }
        [HttpPost]
        public AppVersion GetLatestVersion(AppVersion objVersion)
        {
            AIA.Life.Business.Common.CommonBusiness obj = new AIA.Life.Business.Common.CommonBusiness();
            objVersion = obj.GetLatestVersion(objVersion);
            return objVersion;
        }

        public AppVersion UpdateLatestVersion(AppVersion objVersion)
        {
            AIA.Life.Business.Common.CommonBusiness obj = new AIA.Life.Business.Common.CommonBusiness();
            objVersion = obj.UpdateLatestVersion(objVersion);
            return objVersion;
        }

        [HttpPost]
        public OCRResponse gooleVisionTextDecoderApi(OCRResponse objResp)
        {
            AIA.Life.Business.Common.CommonBusiness obj = new AIA.Life.Business.Common.CommonBusiness();
            objResp = obj.gooleVisionTextDecoderApi(objResp);
            return objResp;
        }

        public TransactLog FetchTraceID(TransactLog objTransactLog)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
           return objPolicyBusiness.FetchTraceID(objTransactLog);
        }
        public void CreateServiceLog(TpServiceLog transactLog)
        {
            CommonBusiness commonBusiness = new CommonBusiness();
            commonBusiness.CreateServiceLog(transactLog);
        }
        public void UploadDocumentsLDMS(LdmsDocuments documents)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            objPolicyBusiness.UploadDocumentsLDMS(documents);
        }
        public SARFALDetails FetchSarAndFalDetails(SARFALDetails sARFALDetails)
        {
            AIA.Life.Business.Common.CommonBusiness obj = new AIA.Life.Business.Common.CommonBusiness();
            sARFALDetails = obj.FetchSarAndFalDetails(sARFALDetails);
            return sARFALDetails;
        }
        public void PostPolicyIssuanceTriggers(PaymentModel objPaymentModel)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            objPolicyBusiness.PostPolicyIssuanceTriggers(objPaymentModel);
        }
        public void InvokeILDeleteLife(AIA.Life.Models.Policy.Policy objpolicy)
        {
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            objPolicyBusiness.InvokeILDeleteLife(objpolicy);
        }
    }
}