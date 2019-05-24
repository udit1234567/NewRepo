using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AIA.Life.Models.UWDecision
{


    [XmlRoot(ElementName = "Product")]
    public class Product
    {
        [XmlAttribute(AttributeName = "ProductId")]
        public string ProductId { get; set; }
        [XmlAttribute(AttributeName = "PlanId")]
        public string PlanId { get; set; }
        [XmlAttribute(AttributeName = "ProposalPremium")]
        public string ProposalPremium { get; set; }
        [XmlAttribute(AttributeName = "MemberPremium")]
        public string MemberPremium { get; set; }
        [XmlAttribute(AttributeName = "PolicyTerm")]
        public string PolicyTerm { get; set; }
        [XmlAttribute(AttributeName = "PaymentFrequency")]
        public string PaymentFrequency { get; set; }
        [XmlAttribute(AttributeName = "ProposalNo")]
        public string ProposalNo { get; set; }
        [XmlAttribute(AttributeName = "ApplyOccupationLoading")]
        public string ApplyOccupationLoading { get; set; }
    }

    [XmlRoot(ElementName = "Rider")]
    public class Rider
    {
        [XmlAttribute(AttributeName = "RiderId")]
        public string RiderId { get; set; }
        [XmlAttribute(AttributeName = "SumAssured")]
        public string SumAssured { get; set; }
        [XmlAttribute(AttributeName = "RiderPremium")]
        public string RiderPremium { get; set; }
        [XmlAttribute(AttributeName = "LoadingAmount")]
        public string LoadingAmount { get; set; }
        [XmlAttribute(AttributeName = "LoadingType")]
        public string LoadingType { get; set; }
        [XmlAttribute(AttributeName = "LoadingBasis")]
        public string LoadingBasis { get; set; }
        [XmlAttribute(AttributeName = "BenefitId")]
        public string BenefitId { get; set; }
        [XmlAttribute(AttributeName = "ExtraPremium")]
        public string ExtraPremium { get; set; }
        [XmlAttribute(AttributeName = "LoadingPer")]
        public string LoadingPer { get; set; }
        [XmlAttribute(AttributeName = "LoadingPerMille")]
        public string LoadingPerMille { get; set; }
        [XmlAttribute(AttributeName = "BasicPremium")]
        public string BasicPremium { get; set; }
        [XmlAttribute(AttributeName = "RowId")]
        public string RowId { get; set; }
    }

    [XmlRoot(ElementName = "ProposalDetails")]
    public class ProposalDetails
    {
        [XmlElement(ElementName = "Product")]
        public Product Product { get; set; }
        [XmlElement(ElementName = "Member")]
        public List<Member> Member { get; set; }
        [XmlElement(ElementName = "Rider")]
        public List<Rider> Rider { get; set; }
    }
    [XmlRoot(ElementName = "Member")]
    public class Member
    {
        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "Relation")]
        public string Relation { get; set; }
        [XmlAttribute(AttributeName = "Age")]
        public string Age { get; set; }
        [XmlAttribute(AttributeName = "MemberPremium")]
        public string MemberPremium { get; set; }
        [XmlElement(ElementName = "Rider")]
        public List<Rider> Rider { get; set; }
    }

    public class LoadingPremiumOutput
    {
        public string RiderID { get; set; }
        public string SumAssured { get; set; }
        public string RiderPremium { get; set; }
        public string LoadingAmount { get; set; }
        public string LoadingType { get; set; }
        public string LoadingBasis { get; set; }
        public decimal ExtraPremium { get; set; }
        public decimal TotalPremium { get; set; }
        public int RowId { get; set; }
    }

}
