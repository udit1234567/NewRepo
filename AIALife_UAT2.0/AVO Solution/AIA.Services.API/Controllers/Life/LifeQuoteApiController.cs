using AIA.Life.Models.Opportunity;
using AIA.Life.Models.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;



namespace AIA.Services.API.Controllers.Life
{

    public class LifeQuoteApiController : ApiController
    {

        public AIA.Life.Models.Policy.Policy LoadSubmittedProposalsData(Policy objPolicyData)
        {
            AIA.Life.Models.Policy.Policy objPolicy = new AIA.Life.Models.Policy.Policy();

            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            objPolicy = objQuoteBusiness.LoadSubmittedProposals(objPolicyData);
            return objPolicy;
        }

        public AIA.Life.Models.Policy.Policy LoadPendingRequirementsData(Policy objPolicyData)
        {
            AIA.Life.Models.Policy.Policy objPolicy = new AIA.Life.Models.Policy.Policy();

            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            objPolicy = objQuoteBusiness.LoadPendingRequirements(objPolicyData);
            return objPolicy;
        }

        public LifeQuote LoadQuotationPoolData(LifeQuote objPolicyData)
        {

            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            objPolicyData = objQuoteBusiness.LoadQuotationPool(objPolicyData);
            return objPolicyData;
        }

        public AIA.Life.Models.Policy.Policy LoadProposalIncompleteData(Policy objPolicyData)
        {
            AIA.Life.Models.Policy.Policy objPolicy = new AIA.Life.Models.Policy.Policy();

            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            objPolicy = objQuoteBusiness.LoadProposalIncomplete(objPolicyData);
            return objPolicy;
        }

        public AIA.Life.Models.Policy.Policy LoadQuotationReAllocateData(Policy objPolicyData)
        {
            AIA.Life.Models.Policy.Policy objPolicy = new AIA.Life.Models.Policy.Policy();

            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            objPolicy = objQuoteBusiness.LoadQuotationReAllocate(objPolicyData);
            return objPolicy;
        }

        public AIA.Life.Models.Policy.Policy LoadProposalReAllocateData(Policy objPolicyData)
        {
            AIA.Life.Models.Policy.Policy objPolicy = new AIA.Life.Models.Policy.Policy();

            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            objPolicy = objQuoteBusiness.LoadProposalReAllocate(objPolicyData);
            return objPolicy;
        }

        public AIA.Life.Models.Opportunity.LifeQuote LoadBenefits(AIA.Life.Models.Opportunity.LifeQuote objQuote)
        {
            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            return objQuoteBusiness.LoadBenefits(objQuote);
        }        
        public AIA.Life.Models.Policy.Policy LoadProposalBenefits(AIA.Life.Models.Policy.Policy objProposal)
        {
            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            return objQuoteBusiness.LoadProposalBenefits(objProposal);
        }
        public AIA.Life.Models.Opportunity.LifeQuote SaveQuote(AIA.Life.Models.Opportunity.LifeQuote ObjQuote)
        {
            

            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            return objQuoteBusiness.SaveQuote(ObjQuote);

        }
        public AIA.Life.Models.Opportunity.LifeQuote SendEmailAndSMSNotificationOnQuoteCreation(AIA.Life.Models.Opportunity.LifeQuote ObjQuote)
        {
            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            return objQuoteBusiness.SendEmailAndSMSNotificationOnQuoteCreation(ObjQuote);

        }
        
        public AIA.Life.Models.EmailSMSDetails.EmailDetails Email(AIA.Life.Models.EmailSMSDetails.EmailDetails ObjEmailDetails) 
        {
           // ObjEmailDetails = new AIA.Life.Models.EmailSMSDetails.EmailDetails();
            string ObjEmail = Newtonsoft.Json.JsonConvert.SerializeObject(ObjEmailDetails);

            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            ObjEmailDetails = objQuoteBusiness.Email(ObjEmailDetails);
            return ObjEmailDetails;

        }
        public AIA.Life.Models.EmailSMSDetails.SMSDetails SMS(AIA.Life.Models.EmailSMSDetails.SMSDetails ObjSMSDetails)
        {
            //ObjSMSDetails = new AIA.Life.Models.EmailSMSDetails.SMSDetails();
            string ObjEmail = Newtonsoft.Json.JsonConvert.SerializeObject(ObjSMSDetails);

            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            ObjSMSDetails = objQuoteBusiness.SMS(ObjSMSDetails);
            return ObjSMSDetails;

        }
        
        public AIA.Life.Models.Opportunity.LifeQuote FetchQuoteData(AIA.Life.Models.Opportunity.LifeQuote objLifeQuote)
        {
            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            return objQuoteBusiness.FetchQuoteData(objLifeQuote);
        }
            
        public AIA.Life.Models.Opportunity.LifeQuote LoadMastersForQuote(AIA.Life.Models.Opportunity.LifeQuote objLifeQuote)
        {

            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            return objQuoteBusiness.LoadMastersForQuote(objLifeQuote);

        }        
       public AIA.Life.Models.Policy.Policy LoadMastersForProposalDetails(AIA.Life.Models.Policy.Policy objProposal)
        {

            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            return objQuoteBusiness.LoadMastersForProposalDetails(objProposal);

        }


        ///Services 
        ///
       
        public AIA.Life.Models.Integration.Services.QuoteInfo GetQuoteInformation(string QuoteNo)
        {
            AIA.Life.Models.Integration.Services.QuoteInfo objLifeQuote = new AIA.Life.Models.Integration.Services.QuoteInfo();
            objLifeQuote.QuoteNo = QuoteNo;
            objLifeQuote.IsForServices = true;
            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            return objQuoteBusiness.FetchServicesQuoteData(objLifeQuote);
            
        }
     
        public AIA.Life.Models.Opportunity.LifeQuote LoadPreviousInsuranceGrid(AIA.Life.Models.Opportunity.LifeQuote objLifeQuote)
        {

            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            return objQuoteBusiness.LoadPreviousInsuranceGrid(objLifeQuote);

        }

    }
}