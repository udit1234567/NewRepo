using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIA.Life.Models.Policy;
using AIA.Life.Integration.Services;
using MoreLinq;
using AIA.Life.Models.UWDecision;
using AIA.Life.Models.Common;
using AIA.Life.Business.Common;
using AIA.Life.Models.Payment;

namespace AIA.Life.Business.Policy
{
    public class PolicyBusiness
    {

        public AIA.Life.Models.Policy.Policy LoadPolicyPreviousInsuranceGrid(AIA.Life.Models.Policy.Policy objpolicy)
        {
            #region Call API
            objpolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objpolicy, "LoadPolicyPreviousInsuranceGrid", "Policy");
            #endregion
            return objpolicy;
        }

        public AIA.Life.Models.Policy.Policy OccupationQuestions(AIA.Life.Models.Policy.Policy objpolicy)
        {
            #region Call API
            objpolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objpolicy, "OccupationQuestions", "Policy");
            #endregion
            return objpolicy;
        }

        public AIA.Life.Models.Policy.Policy LoadMastersRelationship(AIA.Life.Models.Policy.Policy objpolicy)
        {
            #region Call API
            objpolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objpolicy, "LoadMastersRelationship", "Policy");
            #endregion
            return objpolicy;
        }

        public AIA.Life.Models.Policy.Policy ResidentialStatusQuestions(AIA.Life.Models.Policy.Policy objpolicy)
        {
            #region Call API
            objpolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objpolicy, "ResidentialStatusQuestions", "Policy");
            #endregion
            return objpolicy;
        }

        // For Demo
        public AIA.Life.Models.Policy.UWInbox FetchUWProposals(AIA.Life.Models.Policy.UWInbox objUWInbox)
        {
            #region Call API
            objUWInbox = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.UWInbox>(objUWInbox, "FetchUWProposals", "Policy");
            #endregion
            return objUWInbox;
        }
        // Till here
        public AIA.Life.Models.Policy.UWInbox FetchUWProposalCount(AIA.Life.Models.Policy.UWInbox objUWInbox)
        {
            #region Call API
            objUWInbox = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.UWInbox>(objUWInbox, "FetchUWProposalCount", "Policy");
            #endregion
            return objUWInbox;
        }

        public ProposalInbox FetchProposalIncompleteDetails(ProposalInbox objProposalData)
        {
            #region Call API
            objProposalData = WebApiLogic.GetPostComplexTypeToAPI<ProposalInbox>(objProposalData, "FetchProposalIncompleteDetails", "Policy");
            #endregion
            return objProposalData;
        }

        public ProposalInbox FetchProposalSubmittedDetails(ProposalInbox objProposalData)
        {
            #region Call API
            objProposalData = WebApiLogic.GetPostComplexTypeToAPI<ProposalInbox>(objProposalData, "FetchProposalSubmittedDetails", "Policy");
            #endregion
            return objProposalData;
        }

        public AIA.Life.Models.Policy.Policy LoadMasters(AIA.Life.Models.Policy.Policy objpolicy)
        {
            #region Call API
            objpolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objpolicy, "LoadMasters", "Policy");
            #endregion
            return objpolicy;
        }

        public AIA.Life.Models.Common.Address FillAddressMasterList()
        {
            AIA.Life.Models.Common.Address objAddress = new AIA.Life.Models.Common.Address();

            #region Call API
            objAddress = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Common.Address>(objAddress, "FillAddressMasterList", "Policy");
            #endregion
            return objAddress;
        }

        public AIA.Life.Models.Policy.Policy LoadMastersForProposal(AIA.Life.Models.Policy.Policy objpolicy)
        {
            #region Call API
            objpolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objpolicy, "LoadMastersForProposal", "Policy");
            #endregion
            return objpolicy;
        }


        public AIA.Life.Models.Policy.Policy AssuredMemberDetails(AIA.Life.Models.Policy.Policy objpolicy)
        {
            #region Call API
            objpolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objpolicy, "AssuredMemberDetails", "Policy");
            #endregion
            return objpolicy;
        }

        public AIA.Life.Models.Policy.Policy BenefitDetails(AIA.Life.Models.Policy.Policy objpolicy)
        {
            #region Call API
            objpolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objpolicy, "BenefitDetails", "Policy");
            #endregion
            return objpolicy;
        }

        public AIA.Life.Models.Policy.Policy SaveProposal(AIA.Life.Models.Policy.Policy objpolicy)
        {
            #region Call API
            objpolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objpolicy, "SaveProposal", "Policy");
            #endregion

            //upload documents to LDMS asynchronously
            WebApiLogic.FireForgetAPI(objpolicy, "UploadLdmsDocuments", "Policy");

            return objpolicy;
        }
        public AIA.Life.Models.Policy.Policy LoadProposalInfo(AIA.Life.Models.Policy.Policy objpolicy)
        {
            #region Call API
            objpolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objpolicy, "LoadProposalInfo", "Policy");

            #endregion
            return objpolicy;
        }

        public AIA.Life.Models.Policy.Policy FetchProposalLifeAssuredDetails(AIA.Life.Models.Policy.Policy objpolicy)
        {
            #region Call API
            objpolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objpolicy, "FetchProposalLifeAssuredDetails", "Policy");

            #endregion
            return objpolicy;
        }

        public AIA.Life.Models.Policy.Policy FetchProposalNomineeDetailsWithMemberDetails(AIA.Life.Models.Policy.Policy objpolicy)
        {
            #region Call API
            objpolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objpolicy, "FetchProposalNomineeDetailsWithMemberDetails", "Policy");

            #endregion
            return objpolicy;
        }



        public AIA.Life.Models.Policy.Policy LoadMHDProposalInfo(AIA.Life.Models.Policy.Policy objpolicy)
        {
            #region Call API
            objpolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objpolicy, "LoadMHDProposalInfo", "Policy");
            #endregion
            return objpolicy;
        }
        public AIA.Life.Models.Opportunity.LifeQuote CalculateQuotePremium(AIA.Life.Models.Opportunity.LifeQuote objLifeQuote)
        {

            #region Added for Benefit  OverView
            objLifeQuote.LstBenefitOverView = new List<Models.Common.BenifitDetails>();
            objLifeQuote.LstPremiumOverview = new List<Models.Common.BenifitDetails>();
            List<Models.Common.BenifitDetails> BenefitOverView = new List<Models.Common.BenifitDetails>();
            Models.Common.BenifitDetails BasicCover = new Models.Common.BenifitDetails();
            BasicCover.BenefitID = 0;
            BasicCover.BenifitName = "Basic Cover";
            BenefitOverView.Add(BasicCover);
            foreach (var Member in objLifeQuote.objQuoteMemberDetails)
            {
                BenefitOverView.AddRange(Member.ObjBenefitDetails);
            }
            BenefitOverView = BenefitOverView.DistinctBy(a => a.BenefitID).ToList();

            // To Get Total
            Models.Common.BenifitDetails Total = new Models.Common.BenifitDetails();
            Total.BenefitID = -2;
            Total.BenifitName = "Total";
            BenefitOverView.Add(Total);
            // Till here

            objLifeQuote.LstBenefitOverView.AddRange(BenefitOverView);
            #endregion
            #region Call API
            objLifeQuote = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.LifeQuote>(objLifeQuote, "CalculateQuotePremium", "Policy");
            #endregion

            return objLifeQuote;
        }
        //public AIA.Life.Models.Policy.Policy CalculateProposalPremium(AIA.Life.Models.Policy.Policy objLifeQuote)
        //{
        //    AIA.Life.Integration.Services.Policy.PolicyIntegration objPolicyIntegration = new Integration.Services.Policy.PolicyIntegration();
        //    //  objLifeQuote = objPolicyIntegration.CalculateProposalPremium(objLifeQuote);
        //    return objLifeQuote;
        //}

        public AIA.Life.Models.Policy.Policy FetchProposalInfo(AIA.Life.Models.Policy.Policy objpolicy)
        {
            #region Call API
            objpolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objpolicy, "FetchProposalInfo", "Policy");

            #endregion
            return objpolicy;
        }
        public AIA.Life.Models.Policy.Policy SaveUWRemarks(AIA.Life.Models.Policy.Policy objpolicy)
        {
            #region Call API
            objpolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objpolicy, "SaveUWRemarks", "Policy");
            #endregion
            return objpolicy;
        }
        public AIA.Life.Models.Policy.Policy LoadProposalBenefits(AIA.Life.Models.Policy.Policy objPolicy)
        {
            #region Call API
            objPolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objPolicy, "LoadProposalBenefitsPolicy", "Policy");
            #endregion
            return objPolicy;

        }
        public AIA.Life.Models.Integration.Services.ProposalInfo UpdateUWInfo(AIA.Life.Models.Integration.Services.ProposalInfo objPropoalInfo)
        {
            #region Call API
            objPropoalInfo = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Integration.Services.ProposalInfo>(objPropoalInfo, "UpdateUWInfo", "Policy");
            #endregion
            return objPropoalInfo;
        }
        //public AIA.Life.Models.Policy.Policy CalculatePremium(AIA.Life.Models.Policy.Policy objpolicy)
        //{
        //    AIA.Life.Integration.Services.Policy.PolicyIntegration objPolicyIntegration = new Integration.Services.Policy.PolicyIntegration();
        //    //  objpolicy = objPolicyIntegration.CalculatePremium(objpolicy);
        //    return objpolicy;
        //}

        public UWMemberLevelDeviationStatus UpdateMemberLevelDeviation(UWMemberLevelDeviationStatus objStatus)
        {
            #region Call API
            objStatus = WebApiLogic.GetPostComplexTypeToAPI<UWMemberLevelDeviationStatus>(objStatus, "UpdateMemberLevelDeviation", "Policy");
            #endregion
            return objStatus;

        }

        public AIA.Life.Models.Policy.Policy TestUWDeviation(AIA.Life.Models.Policy.Policy objpolicy)
        {
            #region Call API
            objpolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objpolicy, "TestUWDeviation", "Policy");
            #endregion
            return objpolicy;
        }
        public AIA.Life.Models.Policy.Policy SubmitPolicyDocuments(AIA.Life.Models.Policy.Policy objpolicy)
        {
            #region Call API
            objpolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objpolicy, "SubmitPolicyDocuments", "Policy");
            #endregion
            return objpolicy;
        }
        public AIA.Life.Models.Policy.Policy CounterOfferSubmit(AIA.Life.Models.Policy.Policy objpolicy)
        {
            #region Call API
            objpolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objpolicy, "CounterOfferSubmit", "Policy");
            #endregion
            return objpolicy;
        }

        public MemberDetails FetchLoadigInfo(MemberDetails objMemberDetails)
        {
            #region Call API
            objMemberDetails = WebApiLogic.GetPostComplexTypeToAPI<MemberDetails>(objMemberDetails, "FetchLoadigInfo", "Policy");
            #endregion
            return objMemberDetails;

        }
        public DeletePolicyDocuments DeletePolicyDocument(DeletePolicyDocuments ObjPolicyDocuments)
        {

            #region Call API
            ObjPolicyDocuments = WebApiLogic.GetPostComplexTypeToAPI<DeletePolicyDocuments>(ObjPolicyDocuments, "DeletePolicyDocument", "Policy");
            #endregion
            return ObjPolicyDocuments;
        }

        public DeletePolicyDocuments DeleteDocumentLinkPolicyDocument(DeletePolicyDocuments ObjPolicyDocuments)
        {

            #region Call API
            ObjPolicyDocuments = WebApiLogic.GetPostComplexTypeToAPI<DeletePolicyDocuments>(ObjPolicyDocuments, "DeleteDocumentLinkPolicyDocument", "Policy");
            #endregion
            return ObjPolicyDocuments;
        }

        


        public AIA.Life.Models.Reports.UWDecisionReport UWDecisionReport(AIA.Life.Models.Reports.UWDecisionReport objUWDecisionReport)
        {
            #region Call API
            objUWDecisionReport = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Reports.UWDecisionReport>(objUWDecisionReport, "UWDecisionReport", "Policy");
            #endregion
            return objUWDecisionReport;
        }

        public MemberLevelDecisions DerivePolicyLevelDecision(MemberLevelDecisions objMemberDecision)
        {
            #region Call API
            objMemberDecision = WebApiLogic.GetPostComplexTypeToAPI<MemberLevelDecisions>(objMemberDecision, "DerivePolicyLevelDecision", "Policy");
            #endregion
            return objMemberDecision;
        }

        public AIA.Life.Models.Policy.Policy CalculateLoadingPremium(AIA.Life.Models.Policy.Policy objPolicy)
        {

            #region Call API
            objPolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objPolicy, "CalculateLoadingPremium", "Policy");
            #endregion
            return objPolicy;
        }
        public AIA.Life.Models.Policy.Policy SaveLoadingDetails(AIA.Life.Models.Policy.Policy objPolicy)
        {
            #region Call API
            objPolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objPolicy, "SaveLoadingDetails", "Policy");
            #endregion
            return objPolicy;
        }
        public AIA.Life.Models.Policy.Policy EmailNotificationOnUWDecision(AIA.Life.Models.Policy.Policy objPolicy)
        {
            #region Call API
            objPolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objPolicy, "EmailNotificationOnUWDecision", "Policy");
            #endregion
            return objPolicy;
        }
        public AIA.Life.Models.Policy.Policy SMSNotificationOnUWDecision(AIA.Life.Models.Policy.Policy objPolicy)
        {
            #region Call API
            objPolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objPolicy, "SMSNotificationOnUWDecision", "Policy");
            #endregion
            return objPolicy;
        }
        public void InvokeILPolicyIssuance(AIA.Life.Models.Policy.Policy objPolicy)
        {
            #region Call API
            WebApiLogic.FireForgetAPI(objPolicy, "InvokeILPolicyIssuance", "Policy");
            #endregion
        }
        public Models.Policy.Policy InvokeILModifyProposalAVO(AIA.Life.Models.Policy.Policy objPolicy)
        {
            #region Call API
            objPolicy=WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objPolicy, "InvokeILModifyProposal", "Policy");
            //WebApiLogic.FireForgetAPI(objPolicy, "InvokeILModifyProposal", "Policy");
            #endregion
            return objPolicy;
        }
        public Models.Policy.Policy InvokeILModifyProposal(AIA.Life.Models.Policy.Policy objPolicy)
        {
            #region Call API
            //objPolicy=WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objPolicy, "InvokeILModifyProposal", "Policy");
            WebApiLogic.FireForgetAPI(objPolicy, "InvokeILModifyProposal", "Policy");
            #endregion
            return objPolicy;
        }
        public void InvokeILUWApproval(AIA.Life.Models.Policy.Policy objPolicy)
        {
            #region Call API
            WebApiLogic.FireForgetAPI(objPolicy, "InvokeILUWApproval", "Policy");
            #endregion
        }
        public void InvokeILWorkFlowAck(AIA.Life.Models.Policy.Policy objPolicy)
        {
            #region Call API
            WebApiLogic.FireForgetAPI(objPolicy, "InvokeILWorkFlowAck", "Policy");
            #endregion
        }
        public void InvokeILModifyProposalForExtras(AIA.Life.Models.Policy.Policy objPolicy)
        {
            #region Call API
            WebApiLogic.FireForgetAPI(objPolicy, "InvokeILModifyProposalForExtras", "Policy");
            #endregion
        }

        public ProposalStatus FetchProposalStatus(ProposalStatus proposalStatus)
        {
            #region Call API
            proposalStatus = WebApiLogic.GetPostComplexTypeToAPI<ProposalStatus>(proposalStatus, "FetchProposalStatus", "Policy");
            #endregion
            return proposalStatus;
        }
        public AIA.Life.Models.Policy.Policy SendEmailAndSMSNotificationOnSaveProposal(AIA.Life.Models.Policy.Policy objPolicy)
        {
            #region Call API
            objPolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objPolicy, "SendEmailAndSMSNotificationOnSaveProposal", "Policy");
            #endregion
            return objPolicy;
        }
        public AIA.Life.Models.Policy.TransactLog FetchTraceID(TransactLog objTransactLog)
        {
            #region Call API
            objTransactLog = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.TransactLog>(objTransactLog, "FetchTraceID", "Policy");
            #endregion
            return objTransactLog;
        }

        public AIA.Life.Models.Policy.TransactLog LogOut(TransactLog objTransactLog)
        {
            #region Call API
            objTransactLog = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.TransactLog>(objTransactLog, "LogOut", "Policy");
            #endregion
            return objTransactLog;
        }

        public void UploadDocumentsLDMS(LdmsDocuments documents)
        {
            #region Call API
            WebApiLogic.FireForgetAPI(documents, "UploadDocumentsLDMS", "Policy");
            #endregion
        }
        public void PostPolicyIssuanceTriggers(PaymentModel objPaymentModel)
        {
            #region Call API
            WebApiLogic.FireForgetAPI(objPaymentModel, "PostPolicyIssuanceTriggers", "Policy");
            #endregion
        }
        public void SendDocumentsEmail(AIA.Life.Models.Policy.Policy objPolicy)
        {
            #region Call API
            WebApiLogic.FireForgetAPI(objPolicy, "SendDocumentsEmail", "Policy");
            #endregion
        }
        
        public AIA.Life.Models.Policy.Policy SendMedicalLetterMail(AIA.Life.Models.Policy.Policy objpolicy)
        {
            #region Call API
            objpolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objpolicy, "SendMedicalLetterMail", "Policy");
            #endregion
            return objpolicy;
        }
        public Models.Policy.Policy SubmitCounterOfferQuote(Models.Policy.Policy policy)
        {
            #region Call API
            policy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(policy, "SubmitCounterOfferQuote", "Policy");
            WebApiLogic.FireForgetAPI(policy, "InvokeILForAddLife", "Policy");
            #endregion
            return policy;
        }
        public Models.Policy.Policy SaveBeforeClaGenerateQuote(Models.Policy.Policy policy)
        {
            #region Call API
            policy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(policy, "SaveBeforeClaGenerateQuote", "Policy");
            WebApiLogic.FireForgetAPI(policy, "InvokeILForDeleteLife", "Policy");
            #endregion
            return policy;
        }
        public void CallBizDate(Models.Policy.Policy policy)
        {
            #region Call API
            policy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(policy, "CallBizDate", "Policy");
            #endregion
        }
        public Models.Policy.Policy InvokeILClientCreation(Models.Policy.Policy policy)
        {
            #region Call API
            policy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(policy, "InvokeILClientCreation", "Policy");
            #endregion
            return policy;
        }
        public Models.Policy.Policy InvokeILClientCreationForBeneficiary(Models.Policy.Policy policy)
        {
            #region Call API
            policy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(policy, "InvokeILClientCreationForBeneficiary", "Policy");
            #endregion
            return policy;
        }
    }
}
