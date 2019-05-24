using AIA.Life.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AIA.Life.Models.Opportunity
{
    public class LifeQuote
    {
        public LifeQuote()
        {
            objProspect = new Prospect();
            ObjQuotationPreviousInsurance = new QuotationPreviousInsurance();
            lstGender = new List<Common.MasterListItem>();
            lstOccupation = new List<Common.MasterListItem>();
            objProductDetials = new ProductDetials();
            objQuoteMemberDetails = new List<QuoteMemberDetails>();
            LstBenefitOverView = new List<BenifitDetails>();
            LstPremiumOverview = new List<BenifitDetails>();
            LstIllustation = new List<Illustation>();
            objQuoteMemberDetails = new List<QuoteMemberDetails>();
            ListPlan = new List<MasterListItem>();
            LstPolicyTerm = new List<MasterListItem>();
            LstPremiumTerm = new List<MasterListItem>();
            lstLanguage = new List<MasterListItem>();
            lstPrefMode = new List<MasterListItem>();
            LstBenefitOverView = new List<BenifitDetails>();
            LstPremiumOverview = new List<BenifitDetails>();
            lstSumInsured = new List<MasterListItem>();
            objPreviousInsuranceList = new List<PreviousInsuranceList>();
            objChildDetials = new List<ChildDetails>();
            objSpouseDetials = new SpouseDetails();
            lstSAM = new List<MasterListItem>();
            Error = new Error();
            LstDrawDownDetails = new List<Common.DrawDownDetails>();
        }

        public string RefNo { get; set; }  
        public string ServiceTraceID { get; set; }
        public QuotationPreviousInsurance ObjQuotationPreviousInsurance { get; set; }
        public bool IsForServices { get; set; }
        public int QuoteIndex { get; set; }
        public List<Illustation> LstIllustation { get; set; }
        public List<DrawDownDetails> LstDrawDownDetails { get; set; }
        public Prospect objProspect { get; set; }
        public ProductDetials objProductDetials { get; set; }
        public int? Age { get; set; }
        public string QuotationType { get; set; }
        public  string  Cess { get; set; }
        public string PolicyFee { get; set; }
        public string VAT { get; set; }
        public string AnnualPremium { get; set; }
        public string HalfYearlyPremium { get; set; }
        public string QuaterlyPremium { get; set; }
        public string MonthlyPremium { get; set; }
        public string BasicSumInsured { get; set; }
        public string BasicPremium { get; set; }
        public int Contactid { get; set; }
        public bool IsSelfPay { get; set; }
        public bool IsSelfCovered { get; set; }
        public bool IsSpouseCovered { get; set; }
        public bool IsChildCovered { get; set; }
        public bool IsNeedsIdentified { get; set; }
        public bool IsModifyQuote { get; set; }
        public string NoofChilds { get; set; }
        public SpouseDetails objSpouseDetials { get; set; }
        public List<ChildDetails> objChildDetials { get; set; }        
        public string STRHtml { get; set; }
        public string STRPremiumHtml { get; set; }
        public string STRBenefitHtml { get; set; }
        //public List<BenifitDetails> ObjBenefitDetails { get; set; }
        public string UserName { get; set; }
        public string QuoteNo { get; set; }
        public string PrevQuoteNo { get; set; }
        public string Message { get; set; }
        public int LifeQQID { get; set; }
        public List<string> ListAssured { get; set; }
        public string PanelIndex { get; set; }
        public int _memberIndex { get; set; }
        public int QuoteVersion { get; set; }
        public int TotalSumAssured { get; set; }
     

        public bool IsForCounterOffer { get; set; }

        public Address objAddress { get; set; }
        public List<QuoteMemberDetails> objQuoteMemberDetails { get; set; }
        public List<MasterListItem> ListPlan { get; set; }
        
        public string ProposalNo { get; set; } // CounterOfferCase
        public List<MasterListItem> LstPolicyTerm { get; set; }
        public List<MasterListItem> LstPremiumTerm { get; set; }
        public List<MasterListItem> lstLanguage { get; set; }
        public List<MasterListItem> lstPrefMode { get; set; }
        public List<BenifitDetails> LstBenefitOverView { get; set; }
        public List<BenifitDetails> LstPremiumOverview { get; set; }
        public List<MasterListItem> lstSumInsured { get; set; }
        public List<MasterListItem> lstSAM { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstGender { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstOccupation { get; set; }
        [XmlIgnore]
        public List<QuotionPool> ObjQuotationPool { get; set; }
        public List<PreviousInsuranceList> objPreviousInsuranceList { get; set; }

        public byte[] ByteArray { get; set; }
        public byte[] ByteArray1 { get; set; }
        public string QuotePDFPath { get; set; }
        public Error Error { get; set; }
        public string ProposerSignPath { get; set; }        
        public string WPProposerSignPath { get; set; }
        public string Signature { get; set; }
        public byte[] ProspectSign { get; set; }
        public string WPProposerSignature { get; set; }
        public string WPProposerSigPath { get; set; }
        public byte[] WPSignature { get; set; }
        public string SignType { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? RiskCommencementDate { get; set; }

        public int ChildDeleteIndex { get; set; }
    }
    public class QuoteMemberDetails
    {
        public QuoteMemberDetails()
        {
            ObjBenefitDetails = new List<BenifitDetails>();
        }     
       
        //public int hdnBasicSumInsured { get; set; }
        public string Assured { get; set; }
        public string Relationship { get; set; }
        public string MemberIndex { get; set; }
        public int AgeNextBirthDay { get; set; }
        public int CurrentAge { get; set; }
        public string ChildRelationship { get; set; }
        public string TabIndex { get; set; }
        public bool IsBenefitRequested { get; set; }
        public List<BenifitDetails > ObjBenefitDetails { get; set; }
        public string HospitalizationAvailability { get; set; }
        public DateTime? DateOfBirth { get; set; }
      
    }


    public class QuotionPool
    {
        public int? QuotationId { get; set; }
        public string QuotationNo { get; set; }
        public string Name { get; set; }
        public string NicNo { get; set; }
        public string Mobile { get; set; }
        public string Home { get; set; }
        public string Work { get; set; }
        public string Email { get; set; }
        public int Daysleft { get; set; }
        public string QuotaionType { get; set; }
        public string QuotationCreationDate { get; set; }
        public string Salutation { get; set; }
        public string LeadNo { get; set; }
        public string BancaFPC { get; set; }
        public string SurName { get; set; }
        public string Plancode { get; set; }
        public string ProductCode { get; set; }
        public string PreferredLanguauge { get; set; }
        public string FullName { get; set; }
    }
    public class QuoteList
    {
        public QuoteList()
        {
            objProspect = new Prospect();
            objListQuote = new List<LifeQuote>();           
            ObjLifeQuote = new LifeQuote();
            ObjQuotationPreviousInsurance = new QuotationPreviousInsurance();
        }
        public Prospect objProspect { get; set; }
        public QuotationPreviousInsurance ObjQuotationPreviousInsurance { get; set; }
        public List<LifeQuote> objListQuote { get; set; }
        //Added for loading Reasons
        public List<string> objListReason { get; set; }
        public LifeQuote ObjLifeQuote { get; set; }           
        public string Username { get; set; }
        public string Message { get; set; }
        public string QuoteNo { get; set; }
        public string PrevQuoteNo { get; set; }

        public int SelectedQuote { get; set; }
        public bool IsForCounterOffer { get; set; }
        public string QuoteProposerSignPath { get; set; }
        public string QuoteProposerSignature { get; set; }
        public string QuoteWPProposerSignPath { get; set; }

        public string Signature { get; set; }
        public byte[] ProspectSign { get; set; }
        



    }
    public class PreviousInsuranceList
    {
        public string NameOfTheComp { get; set; }
        public string PolicyNumber { get; set; }
        public string SumAssured { get; set; }      
        public string AnnualPremium { get; set; }
        public string Deathbenifit { get; set; }
        public string IllNessBenifit { get; set; }
        public string PermanentDisability { get; set; }
        public string HospitalizationPerDay { get; set; }
        public string status { get; set; }      
    }

    public class QuotationPreviousInsurance
    {
        public int? OnGoingProposalWithAIA { get; set; }
        public int? NoOfOnGoingProposalWithAIA { get; set; }
        public int? PreviousPolicyWithAIA { get; set; }
        public int? NoOfPreviousPolicyWithAIA { get; set; }
    }
    public class SMSReminder
    {
        public string PolicyID { get; set; }
        public int NoOfDays { get; set; } 
    }
}
