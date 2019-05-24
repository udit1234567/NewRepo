using AIA.Life.Models.Integration.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Integration.Services
{



    public class QProposalReg
    {
        public string AGENT_CODE { get; set; }
        public string AGENT_NAME { get; set; }
        public string BRANCH { get; set; }
        public string BUSINESS_TYPE { get; set; }
        public string CLIENT_CODE { get; set; }
        public string COMPANY_CODE { get; set; }
        public string DOB { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string MIDDLE_NAME { get; set; }
        public string MODE_OFPAYMENT { get; set; }
        public string NAME_WITH_INITIAL { get; set; }
        public string NATIONAL_ID { get; set; }
        public string PREMIUM { get; set; }
        public string PREMIUM_PAYINGTERM { get; set; }
    }

    public class Document
    {
        public string docName { get; set; }
        public string docIndex { get; set; }
        public string volume { get; set; }
        public string size { get; set; }
        public string docExtension { get; set; }
        public string noOfPage { get; set; }
    }

    public class QProposerdetail
    {
        public string ADDRESS1 { get; set; }
        public string ADDRESS2 { get; set; }
        public string AGE { get; set; }
        public string BIRTHDATE { get; set; }
        public string CADDRESS1 { get; set; }
        public string CADDRESS2 { get; set; }
        public string CCITY { get; set; }
        public string CDISTRICT { get; set; }
        public string CITY { get; set; }
        public string CLIENT_CODE { get; set; }
        public string COMPANY_NAME { get; set; }
        public string CPOSTCODE { get; set; }
        public string CPROVINCE { get; set; }
        public string DISTRICT { get; set; }
        public string EMAIL { get; set; }
        public string EPF { get; set; }
        public string EXISTING_CLIENT { get; set; }
        public string FIRST_NAME { get; set; }
        public string GENDER { get; set; }
        public string HEIGHT { get; set; }
        public string HOME { get; set; }
        public string MARTIAL_STATUS { get; set; }
        public string MIDDLE_NAME { get; set; }
        public string MONTHLY_INCOME { get; set; }
        public string NAME_WITHINITIALS { get; set; }
        public string NATIONALITY { get; set; }
        public string NATURE_OFDUTIES { get; set; }
        public string NEW_NICNO { get; set; }
        public string NIC_NUMBER { get; set; }
        public string OCCUPATION { get; set; }
        public string OLD_NICNO { get; set; }
        public string POSTCODE { get; set; }
        public string PROVINCE { get; set; }
        public string SALUATION { get; set; }
        public string SAMEADDRESS { get; set; }
        public string STAR { get; set; }
        public string SUR_NAME { get; set; }
        public string UNIT { get; set; }
        public string WEIGHT { get; set; }
        public string WORK { get; set; }
        public string WUNIT { get; set; }
        public string WUNITS { get; set; }
        public string m_NO { get; set; }
        public string m_TWO { get; set; }
        public string w_UNIT { get; set; }
    }

    public class MEMBERGRID
    {
        public string Existing_Client { get; set; }

        public string MemberType { get; set; }
        public string ClientCode { get; set; }
        public string Relationship_with_Proposer { get; set; }
        public string Saluation { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Sur_Name { get; set; }
        public string CONTRIBUTION { get; set; }
        public string Name_with_initial { get; set; }
        public string Preferred_Name { get; set; }
        public string Gender { get; set; }
        public string Date_o_Birth { get; set; }
        public string Age { get; set; }
        public string Height { get; set; }
        public string Unit { get; set; }
        public string Weight { get; set; }
        public string W_Unit { get; set; }
        public string Marital_Status { get; set; }
        public string Old_NIC_Number { get; set; }
        public string New_NIC_Number { get; set; }
        public string Occupation { get; set; }
        public string Company_Name { get; set; }
        public string Nature_of_duties { get; set; }
        public string Monthly_Income { get; set; }
        public string Nationality { get; set; }
        public string BMI { get; set; }
        public string M_NO { get; set; }
        public string HOME { get; set; }
        public string WORk { get; set; }
        public string EMAIL { get; set; }
        public string ADDRESS1 { get; set; }
        public string ADDRTESS2 { get; set; }
        public string PROVINCE { get; set; }
        public string DISTRICT { get; set; }
        public string CITY { get; set; }
        public string POSTCODE { get; set; }
        public string SAME_ADDRESS { get; set; }
        public string P_ADDRESS { get; set; }
        public string P_ADDRESS2 { get; set; }
        public string P_DISTRICT { get; set; }
        public string P_CITY { get; set; }
        public string P_POSTCODE { get; set; }
        public string P_PROVINCE { get; set; }
        public string W_DATE { get; set; }
        public string M_TWO { get; set; }
    }

    public class QNGLNBMEMBERDETAIL
    {
        public List<MEMBERGRID> MEMBERGRID { get; set; }
    }

    public class QRECIEPT
    {
        public string CLIENT_CODE { get; set; }
        public string FIRST_NAME { get; set; }
        public string MIDDLE_NAME { get; set; }
        public string PAYMENT_MODE { get; set; }
        public string PLAN { get; set; }
        public string PROPOSAL_DATE { get; set; }
        public string PROPOSAL_NUMBER { get; set; }
        public string SUR_NAME { get; set; }
    }

    public class TERMSANDCONDGRID
    {
        public string CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string DETAILS { get; set; }
        public string APPLY { get; set; }
    }

    public class QTERMSANDCOND
    {
        public List<TERMSANDCONDGRID> TERMSANDCONDGRID { get; set; }
    }

    public class QChildAssured
    {
        public string ADDRESS1 { get; set; }
        public string ADDRESS2 { get; set; }
        public string ADDRESS_SAME { get; set; }
        public string AGE_NBIRTHDAY { get; set; }
        public string CITY { get; set; }
        public string DISTRICT { get; set; }
        public string DOB { get; set; }
        public string FATHER_NAME { get; set; }
        public string FIRST_NAME { get; set; }
        public string GENDER { get; set; }
        public string MOTHER_NAME { get; set; }
        public string NAME_WITHINITIALS { get; set; }
        public string POSTCODE { get; set; }
        public string PROVINCE { get; set; }
        public string RELATIONSHIP_WITHPROPOSER { get; set; }
        public string l_NAME { get; set; }
        public string m_NAME { get; set; }
    }

    public class EXCLUSIONGRID
    {
        public string APPLY { get; set; }
        public string CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string DETAILS { get; set; }
    }

    public class QExclusion
    {
        public string APPLY { get; set; }
        public string CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string DETAILS { get; set; }
        public List<EXCLUSIONGRID> EXCLUSIONGRID { get; set; }
    }

    public class QPremium
    {
        public string CESS { get; set; }
        public string CLIENT_PREMIUM { get; set; }
        public string CLIENT_PREMIUM_FOREIGN_CURRENCY { get; set; }
        public string CONTRIBUTION { get; set; }
        public string IF_OTHER { get; set; }
        public string POLICY_FEE { get; set; }
        public string PREMIUM { get; set; }
        public string PREMIUM_FOREIGN_CURRENCY { get; set; }
        public string PREMIUM_PAID_BY { get; set; }
        public string PREMIUM_PAYMENT_TERM { get; set; }
        public string PREMIUM_PAY_METHOD { get; set; }
        public string PREMIUM_RECEIPT_BY { get; set; }
        public string VAT { get; set; }
    }

    public class Question
    {
        public string QId { get; set; }
        public string QType { get; set; }
        public string Value { get; set; }
        public string SubAnswer { get; set; }
    }

    public class FamilyBackground
    {
        public string Relationship { get; set; }
        public string PresentAge { get; set; }
        public string StateofHealth { get; set; }
        public string Alive { get; set; }
        public string AgeatDeath { get; set; }
        public string CauseofDeath { get; set; }
        public string Detailsofpoorhealth { get; set; }
    }

    public class Otherdetail
    {
        public string CompanyName { get; set; }
        public string policyPropNo { get; set; }
        public string totalSAforDeath { get; set; }
        public string accBenefitAmt { get; set; }
        public string criticalIllnessBenefit { get; set; }
        public string hospDailyBenefit { get; set; }
        public string hospReimbBenefitAmt { get; set; }
        public string currStatus { get; set; }
        public string comments { get; set; }
    }

    public class PrevandCurrLifeIns
    {
        public string NoofPoliciesJS { get; set; }
        public string otherinspolicy { get; set; }
        public string noofotherpolicies { get; set; }
        public List<Otherdetail> Otherdetail { get; set; }
    }

    public class EntityGrid
    {
        public string EntityName { get; set; }
        public string EntityType { get; set; }
        public List<Question> Questions { get; set; }
        public List<FamilyBackground> FamilyBackground { get; set; }
        public PrevandCurrLifeIns PrevandCurrLifeIns { get; set; }
    }

    public class QQuestion
    {
        public List<EntityGrid> EntityGrid { get; set; }
    }

    public class LoadingGrid
    {
        public string LoadingName { get; set; }
        public string Rate { get; set; }
        public string ExtraPremiumAnnualBase { get; set; }
        public string ExtraPremiumAnnualForeign { get; set; }
        public string ProRate { get; set; }
        public string ExtraPremiumModeBase { get; set; }
        public string ExtraPremiumModeForeign { get; set; }
        public string Type { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string EMClass { get; set; }
        public string EMClassPer { get; set; }
        public string EMRate { get; set; }
        public string Premium { get; set; }
    }

    public class RiderGrid
    {
        public string RiderCode { get; set; }
        public string Description { get; set; }
        public string SumAssured { get; set; }
        public string ForeignSA { get; set; }
        public string Rate { get; set; }
        public string AnnualPremiumBase { get; set; }
        public string AnnualPremiumForeign { get; set; }
        public string ProRate { get; set; }
        public string ModePremiumBase { get; set; }
        public string ModePremiumForeign { get; set; }
        public string ExtraTotalAnnualBase { get; set; }
        public string ExtraTotalAnnualForeign { get; set; }
        public string ExtraTotalModeBase { get; set; }
        public string ExtraTotalModeForeign { get; set; }
        public List<LoadingGrid> LoadingGrid { get; set; }
    }

    public class EntityGrid2
    {
        public string EntityName { get; set; }
        public string TotalPremiumAnnualBase { get; set; }
        public string TotalPremiumAnnualForeign { get; set; }
        public string TotalPremiumModeBase { get; set; }
        public string TotalPremiumModeForeign { get; set; }
        public List<RiderGrid> RiderGrid { get; set; }
    }

    public class QRiderDetails
    {
        public string Currency { get; set; }
        public List<EntityGrid2> EntityGrid { get; set; }
    }

    public class ProposalRequest
    {
        public string Quotation_Number { get; set; }
        public string Quotation_Date { get; set; }
        public string Proposal_Number { get; set; }
        public string Plan_Name { get; set; }
        public string Plan_Code { get; set; }
        public string Policy_Term { get; set; }
        public string Loan_Amount { get; set; }
        public string Interest_Rate { get; set; }
        public string ProposalDate { get; set; }
        public QProposalReg q_proposal_reg { get; set; }
        public List<Document> document { get; set; }//pending
        public QProposerdetail q_proposerdetail { get; set; }
        public QNGLNBMEMBERDETAIL Q_NG_LNB_MEMBER_DETAIL { get; set; }
        public QRECIEPT Q_RECIEPT { get; set; }
        public QTERMSANDCOND Q_TERMSANDCOND { get; set; }
        public QChildAssured q_child_assured { get; set; } // Jeevetha Thilana
        public QExclusion q_exclusion { get; set; }
        public QPremium q_premium { get; set; }
        public QQuestion Q_Question { get; set; }
        public QRiderDetails QRiderDetails { get; set; }
    }


    public class ProposalResponse
    {
        public string data { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }

}
