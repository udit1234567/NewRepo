using AIA.Life.Models.Opportunity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Business.Quote
{
  public   class QuoteBusiness
    {

     

     
        public AIA.Life.Models.Policy.Policy LoadSubmittedProposals(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Models.Policy.Policy objPolicySubmittedProposals = new Models.Policy.Policy();
            #region Call API
            objPolicySubmittedProposals = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objPolicySubmittedProposals, "LoadSubmittedProposals", "Policy");
            #endregion
            return objPolicySubmittedProposals;
        }

        public AIA.Life.Models.Policy.Policy LoadPendingRequirements(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Models.Policy.Policy objPolicyPendingRequirements = new Models.Policy.Policy();
            #region Call API
            objPolicyPendingRequirements = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objPolicyPendingRequirements, "LoadPendingRequirements", "Policy");
            #endregion
            return objPolicyPendingRequirements;
        }



        public LifeQuote LoadQuotationPool(LifeQuote objPolicy)
        {

            #region Call API
            objPolicy = WebApiLogic.GetPostComplexTypeToAPI<LifeQuote>(objPolicy, "LoadQuotationPool", "Policy");
            #endregion
            return objPolicy;
        }

        public AIA.Life.Models.Policy.Policy LoadProposalIncomplete(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Models.Policy.Policy objProposalIncomplete = new Models.Policy.Policy();
            #region Call API
            objProposalIncomplete = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objProposalIncomplete, "LoadProposalIncomplete", "Policy");
            #endregion
            return objProposalIncomplete;
        }



        public AIA.Life.Models.Policy.Policy LoadQuotationReAllocate(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Models.Policy.Policy objQuotationReAllocate = new Models.Policy.Policy();
            #region Call API
            objQuotationReAllocate = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objQuotationReAllocate, "LoadQuotationReAllocate", "Policy");
            #endregion
            return objQuotationReAllocate;
        }

        public AIA.Life.Models.Policy.Policy LoadProposalReAllocate(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Models.Policy.Policy objProposalReAllocate = new Models.Policy.Policy();
            #region Call API
            objProposalReAllocate = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objProposalReAllocate, "LoadProposalReAllocate", "Policy");
            #endregion
            return objProposalReAllocate;
        }
        public AIA.Life.Models.Opportunity.LifeQuote LoadBenefits(AIA.Life.Models.Opportunity.LifeQuote objQuote)
        {
            #region Call API
            objQuote = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.LifeQuote>(objQuote, "LoadBenefits", "Policy");
            #endregion
            return objQuote;
            
        }        
       public AIA.Life.Models.Policy.Policy LoadProposalBenefits(AIA.Life.Models.Policy.Policy objProposal)
        {
            #region Call API
            objProposal = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objProposal, "LoadProposalBenefits", "Policy");
            #endregion
            return objProposal;

        }
        public AIA.Life.Models.Opportunity.LifeQuote SaveQuote(AIA.Life.Models.Opportunity.LifeQuote objQuote)
        {
            #region Call API
            objQuote = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.LifeQuote>(objQuote, "SaveQuote", "Policy");
            #endregion
            return objQuote;
        }
        public AIA.Life.Models.Opportunity.LifeQuote SendEmailAndSMSNotificationOnQuoteCreation(AIA.Life.Models.Opportunity.LifeQuote objQuote)
        {
            #region Call API
            objQuote = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.LifeQuote>(objQuote, "SendEmailAndSMSNotificationOnQuoteCreation", "Policy");
            #endregion
            return objQuote;
        }
        public AIA.Life.Models.Opportunity.SMSReminder SMSReminder(AIA.Life.Models.Opportunity.SMSReminder objSMSReminder)
        {
            #region Call API
            var Message = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.SMSReminder>(objSMSReminder, "SMSReminder", "Policy");
            #endregion
            return Message;
        }
        public AIA.Life.Models.Opportunity.Prospect SendEmailAndSMSNotificationOnSAveProspect(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            #region Call API
            objProspect = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.Prospect>(objProspect, "SendEmailAndSMSNotificationOnSAveProspect", "Policy");
            //WebApiLogic.FireForgetAPI(objProspect, "SendEmailAndSMSNotificationOnSAveProspect", "Policy");
            #endregion
            return objProspect;
        }
        public AIA.Life.Models.EmailSMSDetails.EmailDetails Email(AIA.Life.Models.EmailSMSDetails.EmailDetails objEmail)
        {
            #region Call API
            objEmail = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.EmailSMSDetails.EmailDetails>(objEmail, "Email", "Policy");
            #endregion
            return objEmail;
        }
        public AIA.Life.Models.EmailSMSDetails.SMSDetails SMS(AIA.Life.Models.EmailSMSDetails.SMSDetails objSMS) 
        {
            #region Call API
            objSMS = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.EmailSMSDetails.SMSDetails>(objSMS, "SMS", "Policy");
            #endregion
            return objSMS;
        }
        public AIA.Life.Models.Opportunity.LifeQuote SaveCreateQuote(AIA.Life.Models.Opportunity.LifeQuote objQuote)
        {
            #region Call API
            objQuote = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.LifeQuote>(objQuote, "SaveCreateQuote", "Policy");
            #endregion
            return objQuote;
        }
        

        public AIA.Life.Models.Opportunity.LifeQuote FetchQuoteData(AIA.Life.Models.Opportunity.LifeQuote objLifeQuote)
        {
            #region Call API
            objLifeQuote = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.LifeQuote>(objLifeQuote, "FetchQuoteData", "Policy");
            #endregion
          
            return objLifeQuote;

        }
        public AIA.Life.Models.Integration.Services.QuoteInfo FetchServicesQuoteData(AIA.Life.Models.Integration.Services.QuoteInfo objQuote)
        {
          //  AIA.Life.Models.Integration.Services.QuoteInfo objQuote = new AIA.Life.Models.Integration.Services.QuoteInfo();
            #region Call API
            AIA.Life.Models.Opportunity.LifeQuote objLifeQuote = new AIA.Life.Models.Opportunity.LifeQuote();
            objLifeQuote.IsForServices = true;
            objLifeQuote.QuoteNo = objQuote.QuoteNo;
            objLifeQuote = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.LifeQuote>(objLifeQuote, "FetchQuoteData", "Policy");
            #endregion
            if (objLifeQuote != null)
            {

                objQuote.objProductDetials = objLifeQuote.objProductDetials;
                objQuote.QuoteNo = objLifeQuote.QuoteNo;
                objQuote.AnnualPremium = objLifeQuote.AnnualPremium;
                objQuote.HalfYearlyPremium = objLifeQuote.HalfYearlyPremium;
                objQuote.QuaterlyPremium = objLifeQuote.QuaterlyPremium;
                objQuote.MonthlyPremium = objLifeQuote.MonthlyPremium;
                objQuote.Cess = objLifeQuote.Cess;
                objQuote.PolicyFee = objLifeQuote.PolicyFee;
                objQuote.VAT = objLifeQuote.VAT;
                objQuote.IsSelfCovered = objLifeQuote.IsSelfCovered;
                objQuote.IsSpouseCovered = objLifeQuote.IsSpouseCovered;
                objQuote.IsChildCovered = objLifeQuote.IsChildCovered;
                objQuote.NoofChilds = objLifeQuote.NoofChilds;
                objQuote.objQuoteMemberDetails = objLifeQuote.objQuoteMemberDetails;
                objQuote.Status = "Success";
            }
            else
            {
                objQuote.Status = "Error";
            }

            return objQuote;

        }
        public AIA.Life.Models.Opportunity.LifeQuote LoadMastersForQuote(AIA.Life.Models.Opportunity.LifeQuote objLifeQuote)
        {
            #region Call API
            objLifeQuote = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.LifeQuote>(objLifeQuote, "LoadMastersForQuote", "Policy");
            #endregion
            return objLifeQuote;

        }
   
        

        public AIA.Life.Models.Policy.Policy LoadMastersForProposalDetails(AIA.Life.Models.Policy.Policy objLifeQuote)
        {
            #region Call API
            objLifeQuote = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objLifeQuote, "LoadMastersForProposalDetails", "Policy");
            #endregion
            return objLifeQuote;

        }
        public AIA.Life.Models.Opportunity.LifeQuote LoadPreviousInsuranceGrid(AIA.Life.Models.Opportunity.LifeQuote objLifeQuote)
        {
            #region Call API
            objLifeQuote = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.LifeQuote>(objLifeQuote, "LoadPreviousInsuranceGrid", "Policy");
            #endregion
            return objLifeQuote;

        }
    }
}
