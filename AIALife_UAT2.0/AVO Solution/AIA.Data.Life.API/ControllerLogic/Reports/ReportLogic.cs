using AIA.CrossCutting;
using AIA.Life.Models.Reports;
using AIA.Life.Repository.AIAEntity;
using log4net;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIA.Data.Life.API.ControllerLogic.Reports
{
    public class ReportLogic
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);

        public string GetDecisionDescription(string Decision)
        {
            string Description = string.Empty;
            if (!string.IsNullOrEmpty(Decision))
            {
                try
                {
                    using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                    {
                        int ID = Convert.ToInt32(Decision);
                        Description = Context.tblMasCommonTypes.Where(a => a.CommonTypesID == ID).FirstOrDefault().Description;
                    }
                }
                catch (Exception ex)
                {
                    log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                    Logger.Error(ex);

                }
            }
            return Description;
        }
        public string GetUserName(string UserID)
        {
            string UserName = string.Empty;
            if (!string.IsNullOrEmpty(UserID))
            {
                try
                {
                    using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                    {
                        UserName = Context.AspNetUsers.Where(a => a.Id == UserID).FirstOrDefault().UserName;
                    }
                }
                catch (Exception ex)
                {
                    log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                    Logger.Error(ex);

                }
            }
            return UserName;
        }
        public UWDecisionReport FetchDataForUWDecisionReport(UWDecisionReport objUWReport)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    var objPolicyinfo = Context.tblPolicies.Where(a => a.ProposalNo == objUWReport.ProposalNo).FirstOrDefault();
                    if (objPolicyinfo != null)
                    {
                        objUWReport.Product = Context.tblProducts.Where(a => a.ProductId == objPolicyinfo.ProductID).FirstOrDefault().ProductName;
                        objUWReport.Commencement = objPolicyinfo.PolicyStartDate;
                        objUWReport.PolicyNo = objPolicyinfo.PolicyNo;
                        objUWReport.objMemberDetails = new List<AssuredMembers>();
                        objUWReport.objLstRiderInfo = new List<RiderInfo>();
                        objUWReport.objLstRiderInfo = (from QuoteMember in Context.tblPolicyMemberDetails.Where(a => a.PolicyID == objPolicyinfo.PolicyID && a.IsDeleted != true)
                                                       join MemberBenefit in Context.tblPolicyMemberBenefitDetails
                                                       on QuoteMember.MemberID equals MemberBenefit.MemberID
                                                       join MasRiders in Context.tblProductPlanRiders
                                                       on MemberBenefit.BenifitID equals MasRiders.ProductPlanRiderId
                                                       select new RiderInfo
                                                       {
                                                           RiderName = MasRiders.DisplayName,
                                                           RiderID = MasRiders.ProductPlanRiderId
                                                       }).DistinctBy(a => a.RiderID).ToList();

                        foreach (var Member in objPolicyinfo.tblPolicyMemberDetails.ToList())
                        {
                            AssuredMembers objAssuredMember = new AssuredMembers();
                            objAssuredMember.Age = Member.Age;
                            objAssuredMember.AssuredName = Member.Assuredname;
                            if (Member.OccupationID != null)
                            {
                                var Occupation = Context.tblMasLifeOccupations.Where(a => a.ID == Member.OccupationID).FirstOrDefault();
                                if (Occupation != null)
                                {
                                    objAssuredMember.Occupation = Occupation.OccupationCode;
                                }
                            }
                            objAssuredMember.Name = Member.NameWithInitial;
                            // As this info is currently not getting saved
                            objAssuredMember.MedicalSAR = string.Empty;
                            objAssuredMember.FinancialSAR = string.Empty;
                            // till here
                            objAssuredMember.ListMedicalRequirements = Context.tblPolicyDocuments.Where(a => a.PolicyID == Member.PolicyID && a.MemberType == Member.Assuredname && a.ItemType == "PolicyDocuments" && a.DocumentType == "Medical").Select(a => a.FileName).Distinct().ToList();
                            objAssuredMember.ListFianacialRequirements = Context.tblPolicyDocuments.Where(a => a.PolicyID == Member.PolicyID && a.MemberType == Member.Assuredname && a.ItemType == "PolicyDocuments" && a.DocumentType != "Medical").Select(a => a.FileName).Distinct().ToList();

                            objAssuredMember.objUWDeviations = new List<UWDeviationInfo>();
                            objAssuredMember.objUWDeviations = (from Deviation in Context.tblMemberDeviationInfoes.Where(a => a.MemberID == Member.MemberID)
                                                                select new UWDeviationInfo
                                                                {
                                                                    DeviationMessage = Deviation.Reason,
                                                                    Decision = Deviation.Decision,
                                                                    Date = Deviation.CreateDate,
                                                                    Remarks = Deviation.Remarks,
                                                                    User = Deviation.CreatedBy
                                                                }).ToList()
                                                                .Select(b => new UWDeviationInfo
                                                                 {
                                                                     DeviationMessage = b.DeviationMessage,
                                                                     Decision =GetDecisionDescription( b.Decision),
                                                                     Date = b.Date,
                                                                     Remarks = b.Remarks,
                                                                     User = GetUserName( b.User)

                                                                 }).ToList();



                            objUWReport.objMemberDetails.Add(objAssuredMember);

                            #region Fill Rider Information
                            for (int j = 0; j < objUWReport.objLstRiderInfo.Count(); j++)
                            {
                                RiderRelation objAssuredRelation = new RiderRelation();
                                objAssuredRelation.Assured_Name = Member.Assuredname;
                                objAssuredRelation.Member_Relationship = Convert.ToString(Member.RelationShipWithProposer);
                                int RiderID = objUWReport.objLstRiderInfo[j].RiderID;
                                var RiderInfo = Context.tblPolicyMemberBenefitDetails.Where(a => a.MemberID == Member.MemberID && a.BenifitID == RiderID).FirstOrDefault();
                                if (RiderInfo != null)
                                {
                                    objAssuredRelation.RiderCurrentSI = RiderInfo.SumInsured;
                                    objAssuredRelation.RiderTotalSI = RiderInfo.SumInsured;
                                    objAssuredRelation.Rider_Premium = RiderInfo.Premium;
                                }
                                if (objUWReport.objLstRiderInfo[j].objBenefitMemberRelationship == null)
                                {
                                    objUWReport.objLstRiderInfo[j].objBenefitMemberRelationship = new List<RiderRelation>();
                                }
                                objUWReport.objLstRiderInfo[j].objBenefitMemberRelationship.Add(objAssuredRelation);
                            }
                            #endregion

                        }
                    }

                    #region Policy level  Decision
                    var PolicyRemarksInfo = Context.tblPolicyUWRemarks.Where(a => a.PolicyID == objPolicyinfo.PolicyID).FirstOrDefault();
                    if (PolicyRemarksInfo != null)
                    {
                        int DecisionId = Convert.ToInt32(PolicyRemarksInfo.Decision);
                        string Description = Context.tblMasCommonTypes.Where(a => a.CommonTypesID == DecisionId).FirstOrDefault().Description;
                        objUWReport.Decision = Description;
                        objUWReport.DateTime = PolicyRemarksInfo.CreatedDate;
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {


            }
            return objUWReport;
        }

    }
}