using AIA.CrossCutting;
using AIA.Life.Models.Common;
using AIA.Life.Models.Opportunity;
using AIA.Life.Repository.AIAEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace AIA.Data.Life.API.ControllerLogic.PremiumCalculation
{
    #region XML classes
    [XmlRoot(ElementName = "ProposalDetails")]
    public class ProposalDetails
    {
        [XmlElement(ElementName = "Product")]
        public Product Product { get; set; }
        [XmlElement(ElementName = "Member")]
        public List<Member> Member { get; set; }
    }
    [XmlRoot(ElementName = "Product")]
    public class Product
    {
        [XmlAttribute(AttributeName = "ProductId")]
        public string ProductId { get; set; }
        [XmlAttribute(AttributeName = "PlanId")]
        public string PlanId { get; set; }
        [XmlAttribute(AttributeName = "PaymentFrequency")]
        public string PaymentFrequency { get; set; }
        [XmlAttribute(AttributeName = "DrawDownPeriod")]
        public string DrawDownPeriod { get; set; }
        [XmlAttribute(AttributeName = "AdditionalMortalityPer")]
        public string AdditionalMortalityPer { get; set; }
        [XmlAttribute(AttributeName = "AdditionalMortality_per_mille")]
        public string AdditionalMortalitypermille { get; set; }
        [XmlAttribute(AttributeName = "BasicSumAssured")]
        public string BasicSumAssured { get; set; }
        [XmlAttribute(AttributeName = "PolicyTerm")]
        public string PolicyTerm { get; set; }
        [XmlAttribute(AttributeName = "PremiumPayingTerm")]
        public string PremiumPayingTerm { get; set; }
        [XmlAttribute(AttributeName = "WOPAvailability")]
        public string WOPAvailability { get; set; }
        [XmlAttribute(AttributeName = "Premium")]
        public string Premium { get; set; }
        [XmlAttribute(AttributeName = "SumAssuredLevel")]
        public string SumAssuredLevel { get; set; }
        [XmlAttribute(AttributeName = "NoOfChildren")]
        public string NoOfChildren { get; set; }

        [XmlAttribute(AttributeName = "HIRDeductible")]
        public string HIRDeductible { get; set; }
        [XmlAttribute(AttributeName = "HIRFamilyFloater")]
        public string HIRFamilyFloater { get; set; }
        [XmlAttribute(AttributeName = "ApplyOccupationLoading")]
        public string ApplyOccupationLoading { get; set; }
        [XmlAttribute(AttributeName = "TotalLifeBenefit")]
        public string TotalLifeBenefit { get; set; }

    }

    [XmlRoot(ElementName = "Rider")]
    public class Rider
    {
        [XmlAttribute(AttributeName = "RiderId")]
        public string RiderId { get; set; }
        [XmlAttribute(AttributeName = "SumAssured")]
        public string SumAssured { get; set; }
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
        [XmlAttribute(AttributeName = "ChildCount")]
        public string ChildCount { get; set; }
        [XmlAttribute(AttributeName = "OccupationId")]
        public string OccupationId { get; set; }
        [XmlAttribute(AttributeName = "Gender")]
        public string Gender { get; set; }
        [XmlElement(ElementName = "Rider")]
        public List<Rider> Rider { get; set; }
    }
    #endregion

    public class MapObject
    {
        public string MapQuotePremiumObject(AVOAIALifeEntities entity, LifeQuote objLifeQuote, bool forRiders = false)
        {
            ProposalDetails proposalDetails = new ProposalDetails();
            proposalDetails.Product = new Product();
            proposalDetails.Product.BasicSumAssured = objLifeQuote.objProductDetials.BasicSumInsured.ToString();
            if (objLifeQuote.objProductDetials.Plan != "1" && objLifeQuote.objProductDetials.Plan != "3")
                proposalDetails.Product.BasicSumAssured = "";
            if (objLifeQuote.objProductDetials.Plan == "1")
            {
                proposalDetails.Product.TotalLifeBenefit = proposalDetails.Product.BasicSumAssured;
                proposalDetails.Product.BasicSumAssured = "";

            }
            proposalDetails.Product.ProductId = objLifeQuote.objProductDetials.Plan;
            proposalDetails.Product.PlanId = objLifeQuote.objProductDetials.Variant;
            proposalDetails.Product.AdditionalMortalityPer = "0";
            proposalDetails.Product.AdditionalMortalitypermille = "0";
            proposalDetails.Product.DrawDownPeriod = string.IsNullOrEmpty(objLifeQuote.objProductDetials.DrawDownPeriod) == true ? objLifeQuote.objProductDetials.PensionPeriod : objLifeQuote.objProductDetials.DrawDownPeriod;
            proposalDetails.Product.NoOfChildren = Convert.ToInt32(objLifeQuote.NoofChilds) >= 1 ? "1" : "0";
            proposalDetails.Product.PaymentFrequency = objLifeQuote.objProductDetials.PreferredMode;
            proposalDetails.Product.PremiumPayingTerm = objLifeQuote.objProductDetials.PremiumTerm;
            proposalDetails.Product.PolicyTerm = objLifeQuote.objProductDetials.PolicyTerm;
            if (objLifeQuote.objProductDetials.PlanCode == "HPA")
            {
                proposalDetails.Product.Premium = "";
            }
            else if ((objLifeQuote.objProductDetials.PlanCode != "HPA"))
            {
                proposalDetails.Product.Premium = objLifeQuote.objProductDetials.AnnualPremium;
            }
            proposalDetails.Product.SumAssuredLevel = objLifeQuote.objProductDetials.SAM == 0 ? "" : objLifeQuote.objProductDetials.SAM.ToString();
            proposalDetails.Product.HIRDeductible = objLifeQuote.objProductDetials.Deductable == true ? "1" : "0";
            proposalDetails.Product.HIRFamilyFloater = objLifeQuote.objProductDetials.IsFamilyFloater == true ? "1" : "0";
            proposalDetails.Product.ApplyOccupationLoading = "1";

            proposalDetails.Product.WOPAvailability = objLifeQuote.objQuoteMemberDetails[0].ObjBenefitDetails.Where(a => a.RiderID == 10).Select(b => b.BenifitOpted).FirstOrDefault() == true ? "1" : "0";

            proposalDetails.Member = new List<Member>();
            int pCount = 0;
            for (int i = 0; i < objLifeQuote.objQuoteMemberDetails.Count; i++)
            {

                Member member = new Member();
                member.Age = (objLifeQuote.objQuoteMemberDetails[i].AgeNextBirthDay - 1).ToString();
                member.Id = objLifeQuote.objQuoteMemberDetails[i].TabIndex = (i + 1).ToString();

                if (objLifeQuote.objQuoteMemberDetails[i].Relationship == "267")
                {
                    member.Relation = "1";
                    string EngOccupation = "";
                    if (!string.IsNullOrEmpty(objLifeQuote.objProspect.Occupation))
                    {
                        string[] SplitOccupation = objLifeQuote.objProspect.Occupation.Split('|');
                        EngOccupation = SplitOccupation[0];
                    }
                    else
                    {
                        EngOccupation = objLifeQuote.objProspect.Occupation;
                    }

                    member.OccupationId = entity.tblMasLifeOccupations.Where(a => a.OccupationCode == EngOccupation).Select(a => a.ID).FirstOrDefault().ToString();
                    member.Gender = objLifeQuote.objProspect.Gender;
                }
                else if (objLifeQuote.objQuoteMemberDetails[i].Relationship == "268")
                {
                    member.Relation = "2";
                    member.OccupationId = entity.tblMasLifeOccupations.Where(a => a.CompanyCode == objLifeQuote.objSpouseDetials.Occupation).Select(a => a.ID).FirstOrDefault().ToString();
                    member.Gender = objLifeQuote.objSpouseDetials.Gender;
                }
                else
                {

                    member.Gender = objLifeQuote.objChildDetials[pCount].Gender;
                    member.Relation = "3";
                    pCount = pCount + 1;
                    member.ChildCount = Convert.ToString(pCount);
                }
                member.Rider = new List<Rider>();
                var selectedRiders = forRiders == true ? objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails : objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails.Where(a => a.BenifitOpted == true).ToList();
                foreach (var item in selectedRiders)
                {
                    Rider rider = new Rider();
                    rider.RiderId = item.BenefitID.ToString();
                    if (item.RiderID == 10 || item.CalType == "Cal")
                    {
                        rider.SumAssured = "";
                    }
                    else
                    {
                        rider.SumAssured = item.RiderSuminsured == null ? "" : item.RiderSuminsured.ToString();
                    }
                    if (item.RiderID == 5 && item.CalType == "Cal")
                    {
                        if (objLifeQuote.objProductDetials.PlanCode != "PPG" && objLifeQuote.objProductDetials.PlanCode != "SBA" &&
                            objLifeQuote.objProductDetials.PlanCode != "SBB" && objLifeQuote.objProductDetials.PlanCode != "SBC"
                                && objLifeQuote.objProductDetials.PlanCode != "SBD")
                        {
                            if (Convert.ToUInt32(objLifeQuote.objProductDetials.SAM) * Convert.ToUInt32(objLifeQuote.objProductDetials.AnnualPremium) > 30000000)
                            {
                                rider.SumAssured = "30000000";
                            }
                        }


                    }


                    rider.SumAssured = rider.SumAssured.Replace(" ", "");
                    member.Rider.Add(rider);
                }
                proposalDetails.Member.Add(member);


            }

            return proposalDetails.ToXml();
        }

        public string MapQuotePremiumObjectFOrUW(AVOAIALifeEntities entity, AIA.Life.Models.Policy.Policy objPolicy, bool forRiders = false)
        {
            var QuoteObject = entity.tblLifeQQs.Where(x => x.QuoteNo == objPolicy.QuoteNo).FirstOrDefault();
            var QuotememberDetails = entity.tblQuoteMemberDetials.Where(x => x.LifeQQID == QuoteObject.LifeQQID).ToList();

            ProposalDetails proposalDetails = new ProposalDetails();
            proposalDetails.Product = new Product();
            proposalDetails.Product.BasicSumAssured = Convert.ToString(objPolicy.objProspectDetails.BasicSumInsured);
            if (QuoteObject.PlanId != 1 && QuoteObject.PlanId != 3)
                proposalDetails.Product.BasicSumAssured = "";
            proposalDetails.Product.ProductId = Convert.ToString(objPolicy.ProductID);
            proposalDetails.Product.PlanId = Convert.ToString(QuoteObject.PlanId);
            proposalDetails.Product.AdditionalMortalityPer = "0";
            proposalDetails.Product.AdditionalMortalitypermille = "0";
            if (QuoteObject.DrawDownPeriod != null)
                proposalDetails.Product.DrawDownPeriod = QuoteObject.DrawDownPeriod.ToString();
            else
                proposalDetails.Product.DrawDownPeriod = QuoteObject.PensionPeriod.ToString();

            proposalDetails.Product.NoOfChildren = Convert.ToInt32(QuoteObject.NoOfChild) >= 1 ? "1" : "0";
            proposalDetails.Product.PaymentFrequency = Convert.ToString(objPolicy.PaymentFrequency);
            proposalDetails.Product.PremiumPayingTerm = Convert.ToString(QuoteObject.PremiumTerm);
            proposalDetails.Product.PolicyTerm = Convert.ToString(QuoteObject.PolicyTermID);

            if (QuoteObject.PlanCode == "HPA")
            {
                proposalDetails.Product.Premium = "";
            }
            else if ((QuoteObject.PlanCode != "HPA"))
            {
                proposalDetails.Product.Premium = QuoteObject.AnnualPremium;
            }


            proposalDetails.Product.SumAssuredLevel = QuoteObject.SAM == 0 ? "" : QuoteObject.SAM.ToString();
            proposalDetails.Product.HIRDeductible = QuoteObject.Deductable == true ? "1" : "0";
            proposalDetails.Product.HIRFamilyFloater = QuoteObject.IsFamilyFloater == true ? "1" : "0";
            proposalDetails.Product.ApplyOccupationLoading = "1";

            proposalDetails.Product.WOPAvailability = objPolicy.LstBenifitDetails.Where(a => a.RiderID == 10).Select(b => b.BenifitOpted).FirstOrDefault() == true ? "1" : "0";

            proposalDetails.Member = new List<Member>();
            int pCount = 0;
            for (int i = 0; i < objPolicy.objMemberDetails.Count; i++)
            {

                Member member = new Member();
                member.Age = (objPolicy.objMemberDetails[i].Age - 1).ToString();
                // member.Id = objPolicy.objMemberDetails[i].TabIndex = (i + 1).ToString();

                if (objPolicy.objMemberDetails[i].RelationShipWithPropspect == "267")
                {
                    member.Relation = "1";
                    member.OccupationId = entity.tblMasLifeOccupations.Where(a => a.OccupationCode == objPolicy.objProspectDetails.OccupationID.ToString()).Select(a => a.ID).FirstOrDefault().ToString();
                    member.Gender = objPolicy.objProspectDetails.Gender;
                }
                else if (objPolicy.objMemberDetails[i].RelationShipWithPropspect == "268")
                {
                    member.Relation = "2";
                    member.OccupationId = entity.tblMasLifeOccupations.Where(a => a.CompanyCode == objPolicy.objProspectDetails.OccupationID.ToString()).Select(a => a.ID).FirstOrDefault().ToString();
                    member.Gender = objPolicy.objProspectDetails.Gender;
                }
                else
                {

                    member.Gender = objPolicy.objMemberDetails[pCount].Gender;
                    member.Relation = "3";
                    pCount = pCount + 1;
                }
                member.Rider = new List<Rider>();
                var selectedRiders = forRiders == true ? objPolicy.objMemberDetails[i].objBenifitDetails : objPolicy.objMemberDetails[i].objBenifitDetails.Where(a => a.BenifitOpted == true).ToList();
                foreach (var item in selectedRiders)
                {
                    Rider rider = new Rider();
                    rider.RiderId = item.BenefitID.ToString();
                    if (item.RiderID == 10 || item.CalType == "Cal")
                        rider.SumAssured = "";
                    else
                        rider.SumAssured = item.RiderSuminsured == null ? "" : item.RiderSuminsured.ToString();
                    member.Rider.Add(rider);
                }
                proposalDetails.Member.Add(member);


            }

            return proposalDetails.ToXml();
        }

    }
    public class Premium : MapObject
    {
        public LifeQuote CalculateQuotePremium(LifeQuote objLifeQuote, bool AnnualMode = false)
        {
            using (AVOAIALifeEntities entity = new AVOAIALifeEntities())
            {

                string xmlStr = MapQuotePremiumObject(entity, objLifeQuote);
                #region  Log Input 
                tbllogxml objlogxml = new tbllogxml();
                objlogxml.Description = "premium xml";
                objlogxml.PolicyID = Convert.ToString(objLifeQuote.objProspect.ContactID);
                objlogxml.UserID = objLifeQuote.UserName;
                objlogxml.XMlData = xmlStr;
                objlogxml.CreatedDate = DateTime.Now;
                entity.tbllogxmls.Add(objlogxml);
                entity.SaveChanges();
                #endregion
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetPremiumForAllProducts";
                cmd.Parameters.Add("@s", SqlDbType.VarChar);
                cmd.Parameters.Add("@withDecimal", SqlDbType.Bit);
                cmd.Parameters[0].Value = xmlStr;
                cmd.Parameters[1].Value = 0;
                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);

                List<BenifitDetails> lstPremium = new List<BenifitDetails>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    BenifitDetails benifit = new BenifitDetails();
                    benifit.BenefitID = Convert.ToInt32(ds.Tables[0].Rows[i]["ProductRiderId"]);
                    benifit.BenifitName = Convert.ToString(ds.Tables[0].Rows[i]["RiderName"]);
                    benifit.RiderSuminsured = Convert.ToString(ds.Tables[0].Rows[i]["SumAssured"]);
                    benifit.RiderPremium = Convert.ToString(ds.Tables[0].Rows[i]["RiderPremium"] == DBNull.Value ? "0" : ds.Tables[0].Rows[i].ItemArray[3]);
                    benifit.AssuredMember = Convert.ToString(ds.Tables[0].Rows[i]["RelationID"]);
                    benifit.TotalPremium = Convert.ToString(ds.Tables[0].Rows[i]["PayablePremium"]);
                    benifit.MemberID = Convert.ToString(ds.Tables[0].Rows[i]["MemberId"]);
                    benifit.LoadingAmount = Convert.ToString(ds.Tables[0].Rows[i]["LoadingAmount"]);
                    benifit.DiscountAmount = Convert.ToString(ds.Tables[0].Rows[i]["DiscountAmount"]);
                    benifit.AnnualRiderPremium = Convert.ToString(ds.Tables[0].Rows[i]["AnnualRiderPremium"]);
                    benifit.LoadingPercentage = Convert.ToString(ds.Tables[0].Rows[i]["LoadingPer"]);
                    benifit.LoadinPerMille = Convert.ToString(ds.Tables[0].Rows[i]["LoadingPerMille"]);
                    lstPremium.Add(benifit);
                }
                if (AnnualMode == false)
                {
                    objLifeQuote.AnnualPremium = lstPremium.Select(a => a.TotalPremium).FirstOrDefault();
                }


                for (int i = 0; i < objLifeQuote.objQuoteMemberDetails.Count; i++)
                {
                    var Id = objLifeQuote.objQuoteMemberDetails[i].TabIndex;
                    for (int j = 0; j < objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails.Count; j++)
                    {
                        var riderId = objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenefitID;

                        var prem = lstPremium.Where(a => a.BenefitID == riderId && a.MemberID == Id).FirstOrDefault();
                        if (prem != null)
                        {
                            if (AnnualMode == true)
                            {
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].AnnualModeLoadingAmount = prem.LoadingAmount;
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].AnnualModeDiscountAmount = prem.DiscountAmount;
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].AnnualModeAnnualpremium = prem.AnnualRiderPremium;
                            }
                            else
                            {
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].AssuredMember = prem.AssuredMember;
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].ActualRiderPremium = (Convert.ToInt64(prem.RiderPremium) - Convert.ToInt64(prem.LoadingAmount)).ToString();
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured = prem.RiderSuminsured;
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].LoadingAmount = prem.LoadingAmount;
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderPremium = prem.RiderPremium;
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].DiscountAmount = prem.DiscountAmount;
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].AnnualRiderPremium = prem.AnnualRiderPremium;
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].LoadingPercentage = prem.LoadingPercentage;
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].LoadinPerMille = prem.LoadinPerMille;
                            }

                        }
                        else
                        {
                            if (AnnualMode == true)
                            {
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].AnnualModeLoadingAmount = "0";
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].AnnualModeDiscountAmount = "0";
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].AnnualModeAnnualpremium = "0";
                            }
                            else
                            {
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderPremium = "0";
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].LoadingAmount = "0";
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].ActualRiderPremium = "0";
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured = "0";
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].DiscountAmount = "0";
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].AnnualRiderPremium = "0";
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].LoadingPercentage = "0";
                                objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].LoadinPerMille = "0";
                            }
                        }
                        // objLifeQuote.AnnualModePremium.Add(prem);

                    }

                }

            }

            return objLifeQuote;
        }
        public LifeQuote GetRiderSumAssured(LifeQuote objLifeQuote)
        {
            using (AVOAIALifeEntities entity = new AVOAIALifeEntities())
            {

                string xmlStr = MapQuotePremiumObject(entity, objLifeQuote, true);
                #region  Log Input 
                tbllogxml objlogxml = new tbllogxml();
                objlogxml.Description = "premium xml";
                objlogxml.PolicyID = Convert.ToString(objLifeQuote.objProspect.ContactID);
                objlogxml.UserID = objLifeQuote.UserName;
                objlogxml.XMlData = xmlStr;
                objlogxml.CreatedDate = DateTime.Now;
                entity.tbllogxmls.Add(objlogxml);
                entity.SaveChanges();
                #endregion
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetPremiumForAllProducts";
                cmd.Parameters.Add("@s", SqlDbType.VarChar);
                cmd.Parameters.Add("@withDecimal", SqlDbType.Bit);
                cmd.Parameters[0].Value = xmlStr;
                cmd.Parameters[1].Value = 0;
                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);

                List<BenifitDetails> lstPremium = new List<BenifitDetails>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    BenifitDetails benifit = new BenifitDetails();
                    benifit.BenefitID = Convert.ToInt32(ds.Tables[0].Rows[i]["ProductRiderId"]);
                    benifit.BenifitName = Convert.ToString(ds.Tables[0].Rows[i]["RiderName"]);
                    benifit.RiderSuminsured = Convert.ToString(ds.Tables[0].Rows[i]["SumAssured"]);
                    benifit.RiderPremium = Convert.ToString(ds.Tables[0].Rows[i]["RiderPremium"] == DBNull.Value ? "0" : ds.Tables[0].Rows[i].ItemArray[3]);
                    benifit.AssuredMember = Convert.ToString(ds.Tables[0].Rows[i]["RelationID"]);
                    benifit.MemberID = Convert.ToString(ds.Tables[0].Rows[i]["MemberId"]);
                    benifit.LoadingAmount = Convert.ToString(ds.Tables[0].Rows[i]["LoadingAmount"]);
                    benifit.DiscountAmount = Convert.ToString(ds.Tables[0].Rows[i]["DiscountAmount"]);
                    benifit.AnnualRiderPremium = Convert.ToString(ds.Tables[0].Rows[i]["AnnualRiderPremium"]);
                    lstPremium.Add(benifit);
                }


                for (int i = 0; i < objLifeQuote.objQuoteMemberDetails.Count; i++)
                {
                    var Id = objLifeQuote.objQuoteMemberDetails[i].TabIndex;
                    for (int j = 0; j < objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails.Count; j++)
                    {
                        var riderId = objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenefitID;
                        var prem = lstPremium.Where(a => a.BenefitID == riderId && a.MemberID == Id).FirstOrDefault();
                        if (prem != null)
                        {
                            objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].AssuredMember = prem.AssuredMember;
                            objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].ActualRiderPremium = (Convert.ToInt64(prem.RiderPremium) - Convert.ToInt64(prem.LoadingAmount)).ToString();
                            objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured = prem.RiderSuminsured;
                            objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].LoadingAmount = prem.LoadingAmount;
                            objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderPremium = prem.RiderPremium;
                            objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].DiscountAmount = prem.DiscountAmount;
                            objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].AnnualRiderPremium = prem.AnnualRiderPremium;


                        }
                    }

                }

            }

            return objLifeQuote;
        }

        public LifeQuote ValidateProductDetails(LifeQuote objLifeQuote)
        {
            using (AVOAIALifeEntities entity = new AVOAIALifeEntities())
            {
                long BasicSumAssured = objLifeQuote.objProductDetials.BasicSumInsured;
                int SAM = objLifeQuote.objProductDetials.SAM;
                long TotalSumAssured = BasicSumAssured * SAM;
                decimal AnnualBasicPremium = Convert.ToDecimal(0.1 * BasicSumAssured);
                long ABP = 5 * BasicSumAssured;
                string occupation = "";
                if (!string.IsNullOrEmpty(objLifeQuote.objProspect.Occupation))
                {
                    occupation = objLifeQuote.objProspect.Occupation.Split('|')[0];
                }             
                var occId = entity.tblMasLifeOccupations.Where(a => a.OccupationCode == occupation).Select(a => a.ID).FirstOrDefault();

                var spouseOccId = entity.tblMasLifeOccupations.Where(a => a.CompanyCode == objLifeQuote.objSpouseDetials.Occupation).Select(a => a.ID).FirstOrDefault();
                var spouseOcc = entity.tblMasLifeOccupations.Where(a => a.CompanyCode == objLifeQuote.objSpouseDetials.Occupation).Select(a => a.OccupationCode).FirstOrDefault();
                List<int> listRiders = new List<int>();
                List<int> listMainlifeRiders = new List<int>();
                List<int> listSpouseRiders = new List<int>();
                decimal FamilyGlobalHospMainLIfe = 0;
                decimal FamilyGlobalHospSpouse = 0;
                decimal FamilyGlobalHospChild = 0;
                decimal MainLifeCriticalIllnessSA = 0;
                decimal MainlifesugerySA = 0;
                decimal MainlifeHospitalizationSA = 0;
                decimal MainLifeHECSA = 0;
                decimal SpouseCriticalIllnessSA = 0;
                decimal SpousesugerySA = 0;
                decimal SpouseHospitalizationSA = 0;
                decimal SpouseHECSA = 0;
                decimal ChildHealthcareSA = 0;
                decimal ChildHospitalizationSA = 0;
                decimal ChildHECSA = 0;
                decimal TwoBasicSumHPA = BasicSumAssured * 2;
                decimal APCP = 0;
                decimal BasicBenefit = 0;
                decimal AccBenefit = 0;
                decimal CriticalIllness = 0;
                decimal WomenBenefit = 0;
                decimal MainLifeFHCE = 0;
                List<int> SmartPensionMaxHEC = new List<int>();
                SmartPensionMaxHEC.Add(100000);
                SmartPensionMaxHEC.Add(200000);
                SmartPensionMaxHEC.Add(300000);
                SmartPensionMaxHEC.Add(400000);
                SmartPensionMaxHEC.Add(500000);
                SmartPensionMaxHEC.Add(750000);
                SmartPensionMaxHEC.Add(1000000);
                long MaxHEC = 0;

                //decimal APCP = 
                for (int i = 0; i < objLifeQuote.objQuoteMemberDetails.Count; i++)
                {
                    //  objLifeQuote.objQuoteMemberDetails[i].Assured == "Spouse"
                    listRiders.AddRange(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails.Where(a => a.BenifitOpted == true).Select(a => a.BenefitID).ToList());
                    listMainlifeRiders.AddRange(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails.Where(a => a.BenifitOpted == true && a.AssuredMember == "1").Select(a => a.BenefitID).ToList());
                    listSpouseRiders.AddRange(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails.Where(a => a.BenifitOpted == true && a.AssuredMember == "2").Select(a => a.BenefitID).ToList());

                    for (int j = 0; j < objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails.Count; j++)
                    {


                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitOpted)
                        {

                            string RiderCode = objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode;

                            if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) < Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].MinSumInsured))
                            {
                                objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " sum assured should not be less than " + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].MinSumInsured;
                            }
                            if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].MaxSumInsured) != 0 && Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].MaxSumInsured))
                            {
                                objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " sum assured should not be greater than " + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].MaxSumInsured;
                            }
                            else
                            {
                                if (objLifeQuote.objProductDetials.PlanCode == "PPH")
                                {
                                    //RiderSumAssured Should Be less than (BasicSumAssured*SAM):Accident Benefit,Critical Illness Benefit,Adult Surgery Benefit,Spouse Life Benefit 
                                    if (RiderCode == "TMAC" || RiderCode == "TMCD" || RiderCode == "TMMB" || RiderCode == "TSFD")

                                    {
                                        if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(TotalSumAssured))
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " sum assured should not be greater than " + TotalSumAssured;
                                        }
                                    }

                                    //Hospitalization Benefit
                                    if (RiderCode == "TMHC")

                                    {
                                        if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(AnnualBasicPremium))
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " sum assured should not be greater than " + Convert.ToDecimal(AnnualBasicPremium);
                                        }
                                    }


                                    //Health Expense Cover
                                    if (RiderCode == "TMRA" || RiderCode == "TFRA")

                                    {
                                        if ((Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 100000)
                                            && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 200000)
                                            && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 300000)
                                            && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 400000)
                                            && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 500000)
                                            && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 750000)
                                            && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 1000000))
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured is incorrect";
                                        }
                                        if ((Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) == 100000)
                                            || (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) == 200000)
                                            || (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) == 300000)
                                            || (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) == 400000)
                                            || (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) == 500000)
                                            || (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) == 750000)
                                            || (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) == 1000000))
                                        {
                                            for (int k = 0; k < SmartPensionMaxHEC.Count; k++)
                                            {
                                                if (SmartPensionMaxHEC[k] > Convert.ToDecimal(ABP))
                                                {
                                                    MaxHEC = SmartPensionMaxHEC[k - 1];
                                                    if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(MaxHEC))
                                                    {

                                                        objLifeQuote.Error.ErrorMessage += "<br />" + "For" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + "Sum Assured is incorrect";
                                                        break;

                                                    }

                                                }
                                                else
                                                {
                                                    MaxHEC = ABP;
                                                    if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(MaxHEC))
                                                    {

                                                        objLifeQuote.Error.ErrorMessage += "<br />" + "For" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + "Sum Assured is incorrect";
                                                        break;
                                                    }
                                                }
                                            }


                                        }


                                    }

                                    //If Main life has not taken the common cover then spouse and child can not take  

                                    if (!listRiders.Contains(26))
                                    {
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSCA")
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + "Spouse  Critical Illness Benefit is Not allowed because Critical Illness Benefit  is not taken in MainLife";
                                        }
                                    }
                                    if (!listRiders.Contains(28))
                                    {
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSMA")
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + "Spouse Surgery Benefit is Not allowed because Adult Surgery Benefit  is not taken in MainLife";
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCMB")
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + "Child Health Care  is Not allowed because  Adult Surgery Benefit  is not taken in MainLife";
                                        }
                                    }
                                    if (!listRiders.Contains(31))
                                    {
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSHC")
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + "Spouse Hospitalization Benefit is Not allowed because Hospitalization Benefit is not taken in MainLife";
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCHC")
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + "Child  Hospitalization Benefit is Not allowed because Hospitalization Benefit is not taken in MainLife";
                                        }
                                    }

                                    if (!listRiders.Contains(35))
                                    {
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSRA")
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + "Spouse Hospital Expense Cover is Not allowed because Health Expense Cover is not taken in MainLife";
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCRA")
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + "Child Hospital Expense Cover is Not allowed because Health Expense Cover is not taken in MainLife";
                                        }

                                    }

                                    //...................
                                    if (objLifeQuote.objQuoteMemberDetails[i].Assured == "MainLife")
                                    {

                                        if (RiderCode == "TFRA")
                                        {
                                            FamilyGlobalHospMainLIfe = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }


                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TMCD")

                                        {
                                            MainLifeCriticalIllnessSA = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TMMB")

                                        {
                                            MainlifesugerySA = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TMHC")

                                        {
                                            MainlifeHospitalizationSA = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TMRA")

                                        {
                                            MainLifeHECSA = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                    }
                                    //.......................................
                                    if (objLifeQuote.objQuoteMemberDetails[i].Assured == "Spouse")
                                    {
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSCA")

                                        {
                                            SpouseCriticalIllnessSA = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSMA")

                                        {
                                            SpousesugerySA = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }

                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSHC")

                                        {
                                            SpouseHospitalizationSA = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSRA")

                                        {
                                            SpouseHECSA = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                    }
                                    //...............................
                                    if (objLifeQuote.objQuoteMemberDetails[i].Assured.StartsWith("Child"))
                                    {
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCMB")

                                        {
                                            ChildHealthcareSA = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCHC")

                                        {
                                            ChildHospitalizationSA = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCRA")

                                        {
                                            ChildHECSA = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                    }


                                    if (objLifeQuote.objQuoteMemberDetails[i].Assured == "Spouse")
                                    {
                                        ////Spouse  Critical Illness Benefit
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSCA")
                                        {
                                            if (MainLifeCriticalIllnessSA != 0)
                                            {
                                                if (MainLifeCriticalIllnessSA < SpouseCriticalIllnessSA)
                                                {
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + " Sum Assured for Spouse Critical Illness Benefit Can Not be greater than Mainlife Critical Illness Benefit";
                                                }
                                            }
                                        }
                                    }
                                    //Spouse Surgery Benefit
                                    if (listRiders.Contains(28))
                                    {
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSMA")
                                        {
                                            if (MainlifesugerySA != 0)
                                            {
                                                if (MainlifesugerySA < SpousesugerySA)
                                                {
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + " Sum Assured for Spouse Surgery Benefit Can Not be greater than Mainlife Adult Surgery Benefit";
                                                }
                                            }
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCMB")
                                        {
                                            if (MainlifesugerySA != 0)
                                            {
                                                if (MainlifesugerySA < ChildHealthcareSA)
                                                {
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + " Sum Assured for Child Health Care Benifit Can Not be greater than Mainlife Adult Surgery Benefit";
                                                }
                                            }
                                        }
                                    }
                                    //Spouse Hospitalization Benefit
                                    if (listRiders.Contains(31))
                                    {
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSHC")
                                        {
                                            if (MainlifeHospitalizationSA != 0)
                                            {
                                                if (MainlifeHospitalizationSA < SpouseHospitalizationSA)
                                                {
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + " Sum Assured for Spouse Hospitalization Benefit Can Not be greater than Mainlife Hospitalization Benefit";
                                                }
                                            }
                                        }
                                        //Spouse Surgery Benefit

                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSMA")
                                        {
                                            if (MainlifesugerySA != 0)
                                            {
                                                if (MainlifesugerySA < SpousesugerySA)
                                                {
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + " Sum Assured for Spouse Surgery Benefit Can Not be greater than Mainlife Adult Surgery Benefit";
                                                }
                                            }
                                        }
                                        //Spouse Health Indemnity Benefit
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSRA")
                                        {

                                            if (MainLifeHECSA != SpouseHECSA)
                                            {
                                                //objLifeQuote.Error.ErrorMessage += "<br />" + " Sum Assured for Spouse Hospital Expense Cover Can Not be greater than Mainlife Hospital Expense Cover";
                                                objLifeQuote.Error.ErrorMessage += "<br />" + " Sum Assured for Spouse Hospital Expense Cover Should be same as Mainlife Hospital Expense Cover";
                                            }


                                        }

                                        //Family Spouse Health Indemnity Benefit

                                        if (RiderCode == "TFRA")
                                        {
                                            if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != FamilyGlobalHospMainLIfe)
                                            {
                                                objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be same as MainLife";
                                            }
                                        }


                                    }

                                    if (objLifeQuote.objQuoteMemberDetails[i].Assured.StartsWith("Child"))
                                    {
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCMB")
                                        {
                                            if (MainlifesugerySA != 0)
                                            {
                                                if (MainlifesugerySA < ChildHealthcareSA)
                                                {
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + " Sum Assured for Child Health Care Benifit Can Not be greater than Mainlife Adult Surgery Benefit";
                                                }
                                            }
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCHC")
                                        {
                                            if (MainlifeHospitalizationSA != 0)
                                            {

                                                if (MainlifeHospitalizationSA < ChildHospitalizationSA)
                                                {
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + " Sum Assured for Child  Hospitalization Benefit Can Not be greater than Mainlife Hospitalization Benefit";
                                                }
                                            }
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCRA")
                                        {
                                            if (MainLifeHECSA != 0)
                                            {
                                                if (MainLifeHECSA != ChildHECSA)
                                                {
                                                    //objLifeQuote.Error.ErrorMessage += "<br />" + " Sum Assured for Child Hospital Expense Cover Can Not be greater than Mainlife Hospital Expense Cover";
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + " Sum Assured for Child Hospital Expense Cover Should be same as Mainlife Hospital Expense Cover";
                                                }
                                            }
                                        }
                                        if (RiderCode == "TFRA")
                                        {
                                            if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != FamilyGlobalHospMainLIfe)
                                            {
                                                objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be same as MainLife";
                                            }
                                        }

                                    }



                                }

                                if (objLifeQuote.objProductDetials.PlanCode == "EPB")
                                {
                                    //RiderSumAssured Should Be less than (BasicSumAssured*SAM):Accident Benefit,Critical Illness Benefit,Adult Surgery Benefit,Spouse Life Benefit 
                                    if (RiderCode == "TMAC" || RiderCode == "TMCD" || RiderCode == "TMMB" || RiderCode == "TSFD")

                                    {
                                        if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(TotalSumAssured))
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " sum assured should not be greater than " + TotalSumAssured;
                                        }
                                    }
                                    //Health Indemnity Benefit
                                    if (RiderCode == "TMRA")

                                    {
                                        if ((Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 100000)
                                            && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 200000)
                                            && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 300000)
                                            && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 400000)
                                            && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 500000)
                                            && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 750000)
                                            && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 1000000))
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured is incorrect";
                                        }
                                        if ((Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) == 100000)
                                            || (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) == 200000)
                                            || (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) == 300000)
                                            || (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) == 400000)
                                            || (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) == 500000)
                                            || (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) == 750000)
                                            || (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) == 1000000))
                                        {
                                            for (int k = 0; k < SmartPensionMaxHEC.Count; k++)
                                            {
                                                if (SmartPensionMaxHEC[k] > Convert.ToDecimal(ABP))
                                                {
                                                    MaxHEC = SmartPensionMaxHEC[k - 1];
                                                    if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(MaxHEC))
                                                    {

                                                        objLifeQuote.Error.ErrorMessage += "<br />" + "For" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + "Sum Assured is incorrect";
                                                        break;

                                                    }

                                                }
                                                else
                                                {
                                                    MaxHEC = ABP;
                                                    if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(MaxHEC))
                                                    {

                                                        objLifeQuote.Error.ErrorMessage += "<br />" + "For" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + "Sum Assured is incorrect";
                                                        break;
                                                    }
                                                }
                                            }


                                        }


                                    }


                                    //Hospitalization Benefit
                                    if (RiderCode == "TMHC")

                                    {
                                        if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(AnnualBasicPremium))
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " sum assured should not be greater than " + Convert.ToDecimal(AnnualBasicPremium);
                                        }
                                    }
                                    //Income Protection Benefit
                                    if (RiderCode == "TMIA")

                                    {
                                        if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(BasicSumAssured))
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " sum assured should not be greater than " + Convert.ToDecimal(BasicSumAssured);
                                        }
                                    }
                                    //If Main life has not taken the common cover then spouse and child can not take  

                                    if (!listRiders.Contains(71))
                                    {
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSCA")
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + "Spouse  Critical Illness Benefit is Not allowed because Critical Illness Benefit  is not taken in MainLife";
                                        }
                                    }
                                    if (!listRiders.Contains(74))
                                    {
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSMA")
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + "Spouse Surgery Benefit is Not allowed because Adult Surgery Benefit  is not taken in MainLife";
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCMB")
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + "Child Health Care  is Not allowed because  Adult Surgery Benefit  is not taken in MainLife";
                                        }
                                    }
                                    if (!listRiders.Contains(77))
                                    {
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSHC")
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + "Spouse Hospitalization Benefit is Not allowed because Hospitalization Benefit is not taken in MainLife";
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCHC")
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + "Child  Hospitalization Benefit is Not allowed because Hospitalization Benefit is not taken in MainLife";
                                        }
                                    }

                                    if (!listRiders.Contains(81))
                                    {
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSRA")
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + "Spouse Hospital Expense Cover is Not allowed because Health Expense Cover is not taken in MainLife";
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCRA")
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + "Child Hospital Expense Cover is Not allowed because Health Expense Cover is not taken in MainLife";
                                        }

                                    }

                                    //...................
                                    if (objLifeQuote.objQuoteMemberDetails[i].Assured == "MainLife")
                                    {
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TMCD")

                                        {
                                            MainLifeCriticalIllnessSA = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TMMB")

                                        {
                                            MainlifesugerySA = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TMHC")

                                        {
                                            MainlifeHospitalizationSA = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TMRA")

                                        {
                                            MainLifeHECSA = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                    }
                                    //.......................................
                                    if (objLifeQuote.objQuoteMemberDetails[i].Assured == "Spouse")
                                    {
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSCA")

                                        {
                                            SpouseCriticalIllnessSA = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSMA")

                                        {
                                            SpousesugerySA = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSHC")

                                        {
                                            SpouseHospitalizationSA = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSRA")

                                        {
                                            SpouseHECSA = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                    }
                                    //...............................
                                    if (objLifeQuote.objQuoteMemberDetails[i].Assured.StartsWith("Child"))
                                    {
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCMB")

                                        {
                                            ChildHealthcareSA = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCHC")

                                        {
                                            ChildHospitalizationSA = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCRA")

                                        {
                                            ChildHECSA = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                    }
                                    ////Spouse  Critical Illness Benefit
                                    if (listRiders.Contains(71))
                                    {
                                        if (MainLifeCriticalIllnessSA != 0)
                                        {
                                            if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSCA")
                                            {
                                                if (MainLifeCriticalIllnessSA < SpouseCriticalIllnessSA)
                                                {
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + " Sum Assured for Spouse Critical Illness Benefit Can Not be greater than Mainlife Critical Illness Benefit";
                                                }
                                            }
                                        }
                                    }
                                    //Spouse Surgery Benefit
                                    if (listRiders.Contains(74))
                                    {
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSMA")
                                        {
                                            if (MainlifesugerySA != 0)
                                            {
                                                if (MainlifesugerySA < SpousesugerySA)
                                                {
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + " Sum Assured for Spouse Surgery Benefit cannot be greater than Mainlife Adult Surgery Benefit";
                                                }
                                            }

                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCMB")
                                        {

                                            if (MainlifesugerySA != 0)
                                            {
                                                if (MainlifesugerySA < ChildHealthcareSA)
                                                {
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + " Sum Assured for Child Health Care Benifit cannot be greater than Mainlife Adult Surgery Benefit";
                                                }
                                            }
                                        }
                                    }
                                    //Spouse Hospitalization Benefit
                                    if (listRiders.Contains(77))
                                    {
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSHC")
                                        {
                                            if (MainlifeHospitalizationSA != 0)
                                            {
                                                if (MainlifeHospitalizationSA < SpouseHospitalizationSA)
                                                {
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + " Sum Assured for Spouse Hospitalization Benefit Can Not be greater than Mainlife Hospitalization Benefit";
                                                }
                                            }
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCHC")
                                        {
                                            if (MainlifeHospitalizationSA != 0)
                                            {
                                                if (MainlifeHospitalizationSA < ChildHospitalizationSA)
                                                {
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + " Sum Assured for Child  Hospitalization Benefit Can Not be greater than Mainlife Hospitalization Benefit";
                                                }
                                            }
                                        }
                                    }
                                    //Spouse Health Indemnity Benefit
                                    if (listRiders.Contains(81))
                                    {
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSRA")
                                        {
                                            if (MainLifeHECSA != 0)
                                            {
                                                if (MainLifeHECSA != SpouseHECSA)
                                                {
                                                    //objLifeQuote.Error.ErrorMessage += "<br />" + " Sum Assured for Spouse Hospital Expense Cover Can Not be greater than Mainlife Hospital Expense Cover";
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + " Sum Assured for Spouse Hospital Expense Cover Should be same as Mainlife Hospital Expense Cover";
                                                }
                                            }
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCRA")
                                        {
                                            if (MainLifeHECSA != 0)
                                            {
                                                if (MainLifeHECSA != ChildHECSA)
                                                {
                                                    //objLifeQuote.Error.ErrorMessage += "<br />" + " Sum Assured for Child Hospital Expense Cover Can Not be greater than Mainlife Hospital Expense Cover";
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + " Sum Assured for Child Hospital Expense Cover Should be same as Mainlife Hospital Expense Cover";
                                                }
                                            }

                                        }
                                    }

                                }

                            }
                            //Product Wise Validation
                            if (objLifeQuote.objProductDetials.PlanCode == "HPA")
                            {

                                if (listRiders.Contains(20))
                                {
                                    if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TMRB" || objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCRB" || objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSRB")
                                    {
                                        objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " is not allowed for" + objLifeQuote.objQuoteMemberDetails[i].Assured + " as you have taken Family Global Hospitalisation Care";
                                    }

                                }

                            }
                            if (objLifeQuote.objProductDetials.PlanCode == "PPH")
                            {
                                if (listRiders.Contains(87))
                                {

                                    if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TMRA" || objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSRA" || objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCRA")
                                    {
                                        objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " is not allowed for" + objLifeQuote.objQuoteMemberDetails[i].Assured + " as you have taken Family Hospital Expense Cover";


                                    }
                                }

                                if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails.Where(a => a.RiderCode.Contains("TCMB")).Select(a => a.BenifitOpted).FirstOrDefault())
                                {
                                    if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCRA" || objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCHC")
                                    {
                                        objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " is not allowed for " + objLifeQuote.objQuoteMemberDetails[i].Assured + "As You have taken Child Health Care Benefit";
                                    }

                                }
                                if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails.Where(a => a.RiderCode.Contains("TMRA")).Select(a => a.BenifitOpted).FirstOrDefault())
                                {
                                    if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TMMB")
                                    {
                                        objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " and Hospital Expense Cover are not allowed together";
                                    }
                                }



                            }
                            if (objLifeQuote.objProductDetials.PlanCode == "EPB")
                            {
                                if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails.Where(a => a.RiderCode.Contains("TCMB")).Select(a => a.BenifitOpted).FirstOrDefault())
                                {
                                    if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCRA" || objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCHC")
                                    {
                                        objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " is not allowed for" + objLifeQuote.objQuoteMemberDetails[i].Assured + "As You have taken Child Health Care Benefit";
                                    }

                                }
                                if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails.Where(a => a.RiderCode.Contains("TMRA")).Select(a => a.BenifitOpted).FirstOrDefault())
                                {
                                    if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TMMB")
                                    {
                                        objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " and Hospital Expense Cover are not allowed together";
                                    }
                                }
                            }
                            if (objLifeQuote.objProductDetials.PlanCode == "PPG")
                            {
                                if (BasicSumAssured == 360000)
                                {
                                    if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TMCI")
                                    {
                                        if (Convert.ToInt32(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 100000)
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + "Please Enter Valid Sum Assured For" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName;
                                        }


                                    }
                                    if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TMHF")
                                    {
                                        if (Convert.ToInt32(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 1000)
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + "Please Enter Valid Sum Assured For" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName;
                                        }

                                    }
                                }
                                if (BasicSumAssured == 480000)
                                {
                                    if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TMCI")
                                    {
                                        if (Convert.ToInt32(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 100000 && Convert.ToInt32(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 200000)
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + "Please Enter Valid Sum Assured For" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName;
                                        }

                                    }
                                    if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TMHF")
                                    {
                                        if (Convert.ToInt32(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 1000 && Convert.ToInt32(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 2000)
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + "Please Enter Valid Sum Assured For" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName;
                                        }

                                    }
                                }
                                if (BasicSumAssured == 600000)
                                {
                                    if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TMCI")
                                    {
                                        if (Convert.ToInt32(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 100000 && Convert.ToInt32(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 200000 && Convert.ToInt32(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 300000)
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + "Please Enter Valid Sum Assured For" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName;
                                        }

                                    }
                                    if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TMHF")
                                    {
                                        if (Convert.ToInt32(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 1000 && Convert.ToInt32(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 2000 && Convert.ToInt32(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 3000)
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + "Please Enter Valid Sum Assured For" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName;
                                        }

                                    }
                                }
                                if (BasicSumAssured == 720000)
                                {
                                    if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TMCI")
                                    {
                                        if (Convert.ToInt32(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 100000 && Convert.ToInt32(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 200000 && Convert.ToInt32(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 300000 && Convert.ToInt32(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 400000)
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + "Please Enter Valid Sum Assured For" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName;
                                        }

                                    }
                                    if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TMHF")
                                    {
                                        if (Convert.ToInt32(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 1000 && Convert.ToInt32(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 2000 && Convert.ToInt32(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 3000 && Convert.ToInt32(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 4000)
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + "Please Enter Valid Sum Assured For" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName;
                                        }

                                    }
                                }
                            }
                            if (objLifeQuote.objProductDetials.PlanCode == "HPA")
                            {
                                if (objLifeQuote.objQuoteMemberDetails[i].Assured == "MainLife")
                                {
                                    if (objLifeQuote.objProspect.Gender == "M")
                                    {

                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName == "Basic Life Cover")
                                        {
                                            BasicBenefit = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName == "Accident Benefit")
                                        {
                                            AccBenefit = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName == "Critical Illness Plus")
                                        {
                                            CriticalIllness = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                        APCP = BasicBenefit + AccBenefit + CriticalIllness;

                                    }
                                    if (objLifeQuote.objProspect.Gender == "F")
                                    {

                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName == "Basic Life Cover")
                                        {
                                            BasicBenefit = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName == "Accident Benefit")
                                        {
                                            AccBenefit = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName == "Critical Illness Plus")
                                        {
                                            CriticalIllness = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName == "Women Health Benefit")
                                        {
                                            WomenBenefit = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        }

                                    }
                                }
                            }

                            var MaXSA = (from occ in entity.tblProductPlanRiderOccupationCharts
                                         join rider in listRiders
                                         on occ.ProductPlanRiderId equals rider
                                         where occ.OccupationId == occId && occ.Type == "MAXSA"
                                         select occ.Value).ToList();

                            foreach (var item in MaXSA)
                            {
                                if (Convert.ToInt32(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > item.Value)
                                {
                                    objLifeQuote.Error.ErrorMessage += "<br />" + "Selected Riders are not allowed for the occupation " + objLifeQuote.objProspect.Occupation;
                                }
                            }

                        }

                    }
                    if (objLifeQuote.objProductDetials.PlanCode == "HPA")
                    {
                        if (objLifeQuote.objQuoteMemberDetails[i].Assured == "MainLife")
                        {
                            if (objLifeQuote.objProspect.Gender == "M")
                            {
                                if (BasicBenefit == 0 || AccBenefit == 0 || CriticalIllness == 0)
                                {
                                    objLifeQuote.Error.ErrorMessage += "<br />" + "All Compulsory Premium Package must be selected";
                                }


                            }
                            if (objLifeQuote.objProspect.Gender == "F")
                            {
                                if (BasicBenefit == 0 || AccBenefit == 0 || (CriticalIllness == 0 && WomenBenefit == 0))
                                {
                                    objLifeQuote.Error.ErrorMessage += "<br />" + "All Compulsory Premium Package must be selected";
                                }

                            }
                        }
                    }


                }

                var type = (from occ in entity.tblProductPlanRiderOccupationCharts
                            join rider in listMainlifeRiders
                            on occ.ProductPlanRiderId equals rider
                            where occ.OccupationId == occId
                            select occ.Type).ToList();
                List<int?> lstriderid = (from occ in entity.tblProductPlanRiderOccupationCharts
                                       join rider in listMainlifeRiders
                                       on occ.ProductPlanRiderId equals rider
                                       where occ.OccupationId == occId && occ.Type== "DECLINE"
                                         select occ.ProductPlanRiderId).ToList();
                List<string> ridename = (from R in entity.tblProductPlanRiders
                                         join rider in lstriderid
                                         on R.ProductPlanRiderId equals rider                                       
                                         select R.DisplayName).ToList();
                string MainlifeRider = "";
                if (ridename.Count != 0)
                {
                    for (int i = 0; i < ridename.Count; i++)
                    {
                        MainlifeRider +=  ridename[i]+"," ;
                    }

                }

                var spouseType = (from occ in entity.tblProductPlanRiderOccupationCharts
                                  join rider in listSpouseRiders
                                  on occ.ProductPlanRiderId equals rider
                                  where occ.OccupationId == spouseOccId
                                  select occ.Type).ToList();
                List<int?> lstrideridspouse = (from occ in entity.tblProductPlanRiderOccupationCharts
                                         join rider in listSpouseRiders
                                         on occ.ProductPlanRiderId equals rider
                                         where occ.OccupationId == spouseOccId && occ.Type == "DECLINE"
                                               select occ.ProductPlanRiderId).ToList();
                List<string> ridenamespouse = (from R in entity.tblProductPlanRiders
                                         join rider in lstrideridspouse
                                         on R.ProductPlanRiderId equals rider
                                         select R.DisplayName).ToList();
                
                string SpouseRider = "";
                if (ridenamespouse.Count != 0)
                {
                    for (int i = 0; i < ridenamespouse.Count; i++)
                    {

                        SpouseRider += ridenamespouse[i]+",";
                    }
                }

                if (type.Contains("DECLINE"))

                    objLifeQuote.Error.ErrorMessage += "<br />" + "" + MainlifeRider + " not allowed for mainlife for the occupation " + objLifeQuote.objProspect.Occupation;

                if (spouseType.Contains("DECLINE"))
                    objLifeQuote.Error.ErrorMessage += "<br />" + "" + SpouseRider + " not allowed for spouse for the occupation " + spouseOcc;

                return objLifeQuote;
            }
        }
        public LifeQuote ValidatePremiumDetails(LifeQuote objLifeQuote)
        {

            using (AVOAIALifeEntities entity = new AVOAIALifeEntities())
            {
                long BasicSumAssured = objLifeQuote.objProductDetials.BasicSumInsured;
                int SAM = objLifeQuote.objProductDetials.SAM;
                long TotalSumAssured = BasicSumAssured * SAM;
                decimal AnnualBasicPremium = Convert.ToDecimal(0.1 * BasicSumAssured);
                long ABP = 5 * BasicSumAssured;
                var occId = entity.tblMasLifeOccupations.Where(a => a.OccupationCode == objLifeQuote.objProspect.Occupation).Select(a => a.ID).FirstOrDefault();
                List<int> listRiders = new List<int>();
                decimal TwoBasicSumHPA = BasicSumAssured * 2;
                decimal APCP = 0;
                decimal BasicBenefit = 0;
                decimal AccBenefit = 0;
                decimal CriticalIllness = 0;
                decimal WomenBenefit = 0;
                decimal APCP10Percent = 0;
                decimal MainCriticalIllness = 0;
                decimal SpouseLifeBenifit = 0;
                decimal MainSurgeryBenifit = 0;
                decimal SpouseDailyHospCash = 0;
                decimal GlobalHospCare = 0;
                decimal FamilyGlobalHosp = 0;
                List<int> HealthPlan = new List<int>();
                HealthPlan.Add(250000);
                HealthPlan.Add(500000);
                HealthPlan.Add(750000);
                HealthPlan.Add(1000000);
                HealthPlan.Add(2000000);
                HealthPlan.Add(3000000);
                HealthPlan.Add(4000000);
                HealthPlan.Add(5000000);
                HealthPlan.Add(7500000);
                HealthPlan.Add(10000000);
                HealthPlan.Add(15000000);
                HealthPlan.Add(20000000);
                int FHECCOUNTPPH = 0;
                for (int i = 0; i < objLifeQuote.objQuoteMemberDetails.Count; i++)
                {
                    //  objLifeQuote.objQuoteMemberDetails[i].Assured == "Spouse"
                    listRiders.AddRange(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails.Where(a => a.BenifitOpted == true).Select(a => a.BenefitID).ToList());

                    for (int j = 0; j < objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails.Count; j++)
                    {
                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderID == 13)
                            FHECCOUNTPPH++;

                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitOpted)
                        {
                            string RiderCode = objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode;

                            if (objLifeQuote.objProductDetials.PlanCode == "HPA")
                            {
                                if (objLifeQuote.objQuoteMemberDetails[i].Assured == "MainLife")
                                {
                                    if (objLifeQuote.objProspect.Gender == "M")
                                    {

                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName == "Basic Life Cover")
                                        {
                                            BasicBenefit = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderPremium);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName == "Accident Benefit")
                                        {
                                            AccBenefit = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderPremium);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName == "Critical Illness Plus")
                                        {
                                            CriticalIllness = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderPremium);
                                        }
                                        APCP = BasicBenefit + AccBenefit + CriticalIllness;
                                        objLifeQuote.objProductDetials.APCP = Convert.ToInt32(APCP);
                                    }
                                    if (objLifeQuote.objProspect.Gender == "F")
                                    {

                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName == "Basic Life Cover")
                                        {
                                            BasicBenefit = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderPremium);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName == "Accident Benefit")
                                        {
                                            AccBenefit = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderPremium);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName == "Critical Illness Plus")
                                        {
                                            CriticalIllness = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderPremium);
                                        }
                                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName == "Women Health Benefit")
                                        {
                                            WomenBenefit = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderPremium);
                                        }
                                        if (CriticalIllness == 0)
                                        {
                                            APCP = BasicBenefit + AccBenefit + WomenBenefit;
                                            objLifeQuote.objProductDetials.APCP = Convert.ToInt32(APCP);
                                        }
                                        else
                                        {
                                            APCP = BasicBenefit + AccBenefit + CriticalIllness;
                                            objLifeQuote.objProductDetials.APCP = Convert.ToInt32(APCP);

                                        }

                                    }
                                }
                            }
                        }
                    }

                    if (objLifeQuote.objProductDetials.PlanCode == "HPA")
                    {
                        if (objLifeQuote.objQuoteMemberDetails[i].Assured == "MainLife")
                        {
                            if (objLifeQuote.objProductDetials.PreferredMode == "01")
                            {
                                if (objLifeQuote.objProspect.Gender == "M")
                                {
                                    if (APCP < 60000)
                                    {
                                        objLifeQuote.Error.ErrorMessage += "<br />" + "Compulsory Premium Package cannot be less than 60000";
                                    }

                                }
                                if (objLifeQuote.objProspect.Gender == "F")
                                {

                                    if (APCP < 60000)
                                    {
                                        objLifeQuote.Error.ErrorMessage += "<br />" + "Compulsory Premium Package cannot be less than 60000";
                                    }
                                }
                            }
                            else if (objLifeQuote.objProductDetials.PreferredMode == "02")
                            {
                                if (objLifeQuote.objProspect.Gender == "M")
                                {
                                    if (APCP < 30000)
                                    {
                                        objLifeQuote.Error.ErrorMessage += "<br />" + "Compulsory Premium Package cannot be less than 30000";
                                    }
                                    else
                                    {
                                        APCP *= 2;
                                    }

                                }
                                if (objLifeQuote.objProspect.Gender == "F")
                                {

                                    if (APCP < 30000)
                                    {
                                        objLifeQuote.Error.ErrorMessage += "<br />" + "Compulsory Premium Package cannot be less than 30000";
                                    }
                                    else
                                    {
                                        APCP *= 2;
                                    }
                                }
                            }
                            else if (objLifeQuote.objProductDetials.PreferredMode == "04")
                            {
                                if (objLifeQuote.objProspect.Gender == "M")
                                {
                                    if (APCP < 15000)
                                    {
                                        objLifeQuote.Error.ErrorMessage += "<br />" + "Compulsory Premium Package cannot be less than 15000";
                                    }
                                    else
                                    {
                                        APCP *= 4;
                                    }

                                }
                                if (objLifeQuote.objProspect.Gender == "F")
                                {

                                    if (APCP < 15000)
                                    {
                                        objLifeQuote.Error.ErrorMessage += "<br />" + "Compulsory Premium Package cannot be less than 15000";
                                    }
                                    else
                                    {
                                        APCP *= 4;
                                    }

                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < objLifeQuote.objQuoteMemberDetails.Count; i++)
                {
                    //  objLifeQuote.objQuoteMemberDetails[i].Assured == "Spouse"
                    listRiders.AddRange(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails.Where(a => a.BenifitOpted == true).Select(a => a.BenefitID).ToList());

                    for (int j = 0; j < objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails.Count; j++)
                    {


                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitOpted)
                        {
                            string RiderCode = objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode;

                            if (objLifeQuote.objProductDetials.PlanCode == "HPA")
                            {
                                if (objLifeQuote.objQuoteMemberDetails[i].Assured == "MainLife")
                                {
                                    if (RiderCode == "TMAM")
                                    {
                                        if ((Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != Convert.ToDecimal(BasicSumAssured)) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != TwoBasicSumHPA) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 30000000))
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be one time or two times of Basic Life Cover subjected to maximum of 30000000";
                                        }
                                    }
                                    if (RiderCode == "TMCH")
                                    {
                                        MainCriticalIllness = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(BasicSumAssured))
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be less than " + Convert.ToDecimal(BasicSumAssured);
                                        }
                                    }
                                    if (RiderCode == "TFCA")
                                    {
                                        if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(BasicSumAssured))
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be less than " + Convert.ToDecimal(BasicSumAssured);
                                        }
                                    }
                                    if (RiderCode == "TMMD")
                                    {
                                        MainSurgeryBenifit = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(BasicSumAssured))
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be less than " + Convert.ToDecimal(BasicSumAssured);
                                        }
                                    }
                                    if (RiderCode == "TMHE")
                                    {
                                        APCP10Percent = (APCP * 10) / 100;
                                        SpouseDailyHospCash = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);

                                        if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(APCP10Percent))
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be less than " + Math.Min(Math.Floor(APCP10Percent / 5000) * 5000, 20000);
                                        }
                                        else if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) % 5000 != 0)
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be less than " + Math.Min(Math.Floor(APCP10Percent / 5000) * 5000, 20000) + " and in multiples of 5000";
                                        }
                                    }
                                    if (RiderCode == "TMRB")
                                    {
                                        GlobalHospCare = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        if ((Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 250000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 500000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 750000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 1000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 2000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 3000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 4000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 5000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 7500000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 10000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 15000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 20000000))
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured is incorrect";
                                        }
                                        else
                                        {
                                            if (APCP <= 100000)
                                            {
                                                if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 250000 && Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 500000 && Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 750000 && Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 1000000)
                                                {
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be 250000, 500000, 750000 or 1000000";
                                                }
                                            }
                                            if (APCP > 100000 && APCP <= 300000)
                                            {
                                                var APCPRange = 20 * APCP;
                                                var MaxRange = 0;
                                                for (int z = 0; z < HealthPlan.Count; z++)
                                                {
                                                    if (HealthPlan[z] > APCPRange)
                                                    {
                                                        MaxRange = HealthPlan[z - 1];
                                                        break;
                                                    }
                                                }

                                                if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(MaxRange))
                                                {
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured is incorrect";
                                                }
                                            }
                                            if (APCP > 300000)
                                            {
                                                var APCPRange = 25 * APCP;
                                                var MaxRange = 0;
                                                for (int z = 0; z < HealthPlan.Count; z++)
                                                {
                                                    if (HealthPlan[z] > APCPRange)
                                                    {
                                                        MaxRange = HealthPlan[z - 1];
                                                        break;

                                                    }
                                                }
                                                if (MaxRange == 0)
                                                {
                                                    MaxRange = 20000000;
                                                }
                                                if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(MaxRange))
                                                {
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured is incorrect";
                                                }
                                            }
                                        }

                                    }
                                    if (RiderCode == "TFRB")
                                    {
                                        FamilyGlobalHosp = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        if ((Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 250000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 500000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 750000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 1000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 2000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 3000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 4000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 5000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 7500000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 10000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 15000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 20000000))
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured is incorrect";
                                        }
                                        else
                                        {
                                            if (APCP <= 100000)
                                            {
                                                if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 250000 && Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 500000 && Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 750000 && Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 1000000)
                                                {
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be 250000, 500000, 750000 or 1000000";
                                                }
                                            }
                                            if (APCP > 100000 && APCP <= 300000)
                                            {
                                                var APCPRange = 20 * APCP;
                                                var MaxRange = 0;
                                                for (int z = 0; z < HealthPlan.Count; z++)
                                                {
                                                    if (HealthPlan[z] > APCPRange)
                                                    {
                                                        MaxRange = HealthPlan[z - 1];
                                                        break;
                                                    }
                                                }

                                                if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(MaxRange))
                                                {
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured is incorrect";
                                                }
                                            }
                                            if (APCP > 300000)
                                            {
                                                var APCPRange = 25 * APCP;
                                                var MaxRange = 0;
                                                for (int z = 0; z < HealthPlan.Count; z++)
                                                {
                                                    if (HealthPlan[z] > APCPRange)
                                                    {
                                                        MaxRange = HealthPlan[z - 1];
                                                        break;

                                                    }
                                                }
                                                if (MaxRange == 0)
                                                {
                                                    MaxRange = 20000000;
                                                }
                                                if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(MaxRange))
                                                {
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured is incorrect";
                                                }
                                            }
                                        }

                                    }

                                }
                                if (objLifeQuote.objQuoteMemberDetails[i].Assured == "Spouse")
                                {
                                    if (RiderCode == "TFRB")
                                    {
                                        if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != FamilyGlobalHosp)
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be same as MainLife";
                                        }
                                    }
                                    if (RiderCode == "TFCA")
                                    {
                                        if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(BasicSumAssured))
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be less than " + Convert.ToDecimal(BasicSumAssured);
                                        }
                                    }
                                    if (RiderCode == "TSFE")
                                    {
                                        SpouseLifeBenifit = Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured);
                                        if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(BasicSumAssured))
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be less than " + Convert.ToDecimal(BasicSumAssured);
                                        }
                                    }
                                    if (RiderCode == "TSAA")
                                    {
                                        if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(BasicSumAssured))
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be less than " + Convert.ToDecimal(BasicSumAssured);
                                        }
                                    }
                                    if (RiderCode == "TSCF")
                                    {
                                        if (MainCriticalIllness == 0 || SpouseLifeBenifit == 0)
                                        {
                                            if (MainCriticalIllness == 0)
                                            {
                                                objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " can only be selected if Main Life is selected ";
                                            }
                                            else
                                            {
                                                if (SpouseLifeBenifit == 0)
                                                {
                                                    if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > MainCriticalIllness)
                                                    {
                                                        objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be less than Main Life";
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (SpouseLifeBenifit > 500000)
                                            {
                                                if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Math.Min(SpouseLifeBenifit, MainCriticalIllness))
                                                {
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be less than " + Math.Min(SpouseLifeBenifit, MainCriticalIllness);
                                                }
                                            }
                                            else
                                            {
                                                if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > MainCriticalIllness)
                                                {
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be less than " + Math.Min(SpouseLifeBenifit, MainCriticalIllness);
                                                }
                                            }

                                        }
                                    }
                                    if (RiderCode == "TSMD")
                                    {
                                        if (MainSurgeryBenifit == 0 || SpouseLifeBenifit == 0)
                                        {
                                            if (MainSurgeryBenifit == 0)
                                            {
                                                objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " can only be selected if Main Life is selected ";
                                            }
                                            else
                                            {
                                                if (SpouseLifeBenifit == 0)
                                                {
                                                    if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > MainSurgeryBenifit)
                                                    {
                                                        objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be less than " + MainSurgeryBenifit;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Math.Min(SpouseLifeBenifit, MainSurgeryBenifit))
                                            {
                                                objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be less than " + Math.Min(SpouseLifeBenifit, MainSurgeryBenifit);
                                            }
                                        }
                                    }
                                    if (RiderCode == "TSHE")
                                    {
                                        if (SpouseDailyHospCash == 0)
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " not eligible since Main Life has not been selected";
                                        }
                                        else
                                        {
                                            if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > SpouseDailyHospCash)
                                            {
                                                objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be less than " + SpouseDailyHospCash;
                                            }
                                            if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) % 5000 != 0)
                                            {
                                                objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be less than " + Convert.ToDecimal(SpouseDailyHospCash) + " and in multiples of 5000";
                                            }
                                        }
                                    }
                                    if (RiderCode == "TSRB")
                                    {
                                        if (GlobalHospCare == 0)
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " can only be selected if Main Life is selected";
                                        }
                                        else
                                        {
                                            if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > GlobalHospCare)
                                            {
                                                objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be less than " + GlobalHospCare;
                                            }
                                            else
                                            {
                                                if ((Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 250000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 500000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 750000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 1000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 2000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 3000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 4000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 5000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 7500000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 10000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 15000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 20000000))
                                                {
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured is incorrect";
                                                }
                                                else
                                                {
                                                    if (APCP <= 100000)
                                                    {
                                                        if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 250000 && Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 500000 && Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 750000 && Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 1000000)
                                                        {
                                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be 250000, 500000, 750000 or 1000000";
                                                        }
                                                    }
                                                    if (APCP > 100000 && APCP <= 300000)
                                                    {
                                                        var APCPRange = 20 * APCP;
                                                        var MaxRange = 0;
                                                        for (int z = 0; z < HealthPlan.Count; z++)
                                                        {
                                                            if (HealthPlan[z] > APCPRange)
                                                            {
                                                                MaxRange = HealthPlan[z - 1];
                                                                break;
                                                            }
                                                        }

                                                        if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(MaxRange))
                                                        {
                                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured is incorrect";
                                                        }
                                                    }
                                                    if (APCP > 300000)
                                                    {
                                                        var APCPRange = 25 * APCP;
                                                        var MaxRange = 0;
                                                        for (int z = 0; z < HealthPlan.Count; z++)
                                                        {
                                                            if (HealthPlan[z] > APCPRange)
                                                            {
                                                                MaxRange = HealthPlan[z - 1];
                                                                break;

                                                            }
                                                        }
                                                        if (MaxRange == 0)
                                                        {
                                                            MaxRange = 20000000;
                                                        }
                                                        if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(MaxRange))
                                                        {
                                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured is incorrect";
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                                if (objLifeQuote.objQuoteMemberDetails[i].Assured.StartsWith("Child"))
                                {
                                    if (RiderCode == "TFRB")
                                    {
                                        if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != FamilyGlobalHosp)
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be same as MainLife";
                                        }
                                    }
                                    if (RiderCode == "TCMD")
                                    {
                                        if (MainSurgeryBenifit == 0)
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " not eligible since Main Life has not been selected";
                                        }
                                        else
                                        {
                                            if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > MainSurgeryBenifit)
                                            {
                                                objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be less than " + MainSurgeryBenifit;
                                            }
                                        }
                                    }
                                    if (RiderCode == "TCHD")
                                    {
                                        if (SpouseDailyHospCash == 0)
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " not eligible since Main Life has not been selected";
                                        }
                                        else
                                        {
                                            if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > SpouseDailyHospCash)
                                            {
                                                objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be less than " + SpouseDailyHospCash;
                                            }
                                            if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) % 5000 != 0)
                                            {
                                                objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be less than " + Convert.ToDecimal(SpouseDailyHospCash) + " and in multiples of 5000";
                                            }
                                        }
                                    }
                                    if (RiderCode == "TCRB")
                                    {
                                        if (GlobalHospCare == 0)
                                        {
                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " can only be selected if Main Life is selected";
                                        }
                                        else
                                        {
                                            if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > GlobalHospCare)
                                            {
                                                objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be less than " + GlobalHospCare;
                                            }
                                            else
                                            {
                                                if ((Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 250000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 500000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 750000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 1000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 2000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 3000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 4000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 5000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 7500000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 10000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 15000000) && (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 20000000))
                                                {
                                                    objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured is incorrect";
                                                }
                                                else
                                                {
                                                    if (APCP <= 100000)
                                                    {
                                                        if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 250000 && Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 500000 && Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 750000 && Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) != 1000000)
                                                        {
                                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured should be 250000, 500000, 750000 or 1000000";
                                                        }
                                                    }
                                                    if (APCP > 100000 && APCP <= 300000)
                                                    {
                                                        var APCPRange = 20 * APCP;
                                                        var MaxRange = 0;
                                                        for (int z = 0; z < HealthPlan.Count; z++)
                                                        {
                                                            if (HealthPlan[z] > APCPRange)
                                                            {
                                                                MaxRange = HealthPlan[z - 1];
                                                                break;
                                                            }
                                                        }

                                                        if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(MaxRange))
                                                        {
                                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured is incorrect";
                                                        }
                                                    }
                                                    if (APCP > 300000)
                                                    {
                                                        var APCPRange = 25 * APCP;
                                                        var MaxRange = 0;
                                                        for (int z = 0; z < HealthPlan.Count; z++)
                                                        {
                                                            if (HealthPlan[z] > APCPRange)
                                                            {
                                                                MaxRange = HealthPlan[z - 1];
                                                                break;

                                                            }
                                                        }
                                                        if (MaxRange == 0)
                                                        {
                                                            MaxRange = 20000000;
                                                        }
                                                        if (Convert.ToDecimal(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderSuminsured) > Convert.ToDecimal(MaxRange))
                                                        {
                                                            objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " Sum Assured is incorrect";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //Product Wise Validation
                        if (objLifeQuote.objProductDetials.PlanCode == "HPA")
                        {
                            if (listRiders.Contains(20))
                            {
                                if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TMRB" || objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCRB" || objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TSRB")
                                {
                                    objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " is not allowed for" + objLifeQuote.objQuoteMemberDetails[i].Assured + " as you have taken Family Global Hospitalisation Care";
                                }

                            }
                            if (listRiders.Contains(21))
                            {
                                if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitOpted)
                                {
                                    if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCRB")
                                    {
                                        objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " is not allowed for " + objLifeQuote.objQuoteMemberDetails[i].Assured + "As You have taken Child Health Care Benefit";
                                    }
                                    if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderCode == "TCHD")
                                    {
                                        objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitName + " is not allowed for " + objLifeQuote.objQuoteMemberDetails[i].Assured + "As You have taken Child Health Care Benefit";
                                    }
                                }
                            }

                        }


                    }

                }

                return objLifeQuote;
            }
        }
        public LifeQuote ValidateFHEC(LifeQuote objLifeQuote)
        {
            List<int> listRiders = new List<int>();
            int FHECPPHCount = 0;
            int FHECHPACount = 0;
            int FHECPPHSelected = 0;
            int FHECHPASelected = 0;
            for (int i = 0; i < objLifeQuote.objQuoteMemberDetails.Count; i++)
            {
                listRiders.AddRange(objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails.Where(a => a.BenifitOpted == true).Select(a => a.BenefitID).ToList());

                for (int j = 0; j < objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails.Count; j++)
                {
                    if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderID == 13)
                        FHECPPHCount++;
                    if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderID == 15)
                        FHECHPACount++;

                    if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].BenifitOpted)
                    {
                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderID == 13)
                            FHECPPHSelected++;
                        if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails[j].RiderID == 15)
                            FHECHPASelected++;
                    }
                }
            }
            if (objLifeQuote.objProductDetials.PlanCode == "PPH")
            {
                if (objLifeQuote.objQuoteMemberDetails.Count == FHECPPHCount && FHECPPHSelected < objLifeQuote.objQuoteMemberDetails.Count)
                {
                    objLifeQuote.Error.ErrorMessage += "<br />" + "Family Hospital Expense Cover is mandatory for all member";
                }
                if (objLifeQuote.objQuoteMemberDetails.Count != FHECPPHCount && FHECPPHSelected > 0)
                {
                    objLifeQuote.Error.ErrorMessage += "<br />" + "FHEC rider cannot be selected as the spouse is not within the acceptable range - 19 to 55";
                }
            }
            if (objLifeQuote.objProductDetials.PlanCode == "HPA")
            {
                if (objLifeQuote.objQuoteMemberDetails.Count == FHECHPACount && FHECHPASelected < objLifeQuote.objQuoteMemberDetails.Count)
                {
                    objLifeQuote.Error.ErrorMessage += "<br />" + "Family Hospital Expense Cover is mandatory for all member";
                }
                if (objLifeQuote.objQuoteMemberDetails.Count != FHECHPACount && FHECHPASelected > 0)
                {
                    objLifeQuote.Error.ErrorMessage += "<br />" + "Family Hospital Expense Cover is not allowed as there is no Dependency";
                }
            }

            return objLifeQuote;


        }
        public LifeQuote ValidateLifeRiderDetails(LifeQuote objLifeQuote)
        {
            for (int i = 0; i < objLifeQuote.objQuoteMemberDetails.Count; i++)
            {
                if (objLifeQuote.objQuoteMemberDetails[i].ObjBenefitDetails.Where(a => a.BenifitOpted == true).ToList().Count == 0)
                {
                    objLifeQuote.Error.ErrorMessage += "<br />" + objLifeQuote.objQuoteMemberDetails[i].Assured + " cannot be added. Minimum one rider is mandatory to add a Life.";
                }
            }
            return objLifeQuote;
        }
    }
}