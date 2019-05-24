using AIA.Life.Repository.AIAEntity;
using AIA.Life.Models.Common;
using AIA.Life.Models.UWRules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using AIA.CrossCutting;
using log4net;

namespace AIA.Life.Data.API.ControllerLogic.UWRules
{
    public class RuleOutput
    {

        public string RuleId { get; set; }
        public string RuleName { get; set; }
        public string RuleDescription { get; set; }
        public string RuleCondition { get; set; }
        public string RuleAction { get; set; }
        public int MemberId { get; set; }
    }

    public class DoumentOutput
    {
        public Int32 DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }

    }
    public class UWRuleLogic
    {

        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);

        public string GetParameterValue(AIA.Life.Models.Policy.Policy objPolicy, MemberDetails objMemberDetails, string ParameterName)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                var ProductItem = Context.tblProducts.Where(a => a.ProductCode == objPolicy.PlanCode).FirstOrDefault();
                string ValueToReturn = string.Empty;
                switch (ParameterName)
                {
                    case "Age":
                    ValueToReturn = Convert.ToString(objMemberDetails.Age);
                    break;
                    case "BMI":
                    if (objMemberDetails.objLifeStyleQuetions != null)
                    {
                        #region Converting Height In other Formats to CMS
                        decimal _Height = decimal.Zero;
                        if (objMemberDetails.objLifeStyleQuetions.HeightUnit != null)
                        {
                            if (objMemberDetails.objLifeStyleQuetions.HeightUnit == "Feet")
                            {
                                _Height = (Convert.ToDecimal(objMemberDetails.objLifeStyleQuetions.Height) * (30.48m));
                            }
                            else if (objMemberDetails.objLifeStyleQuetions.HeightUnit == "Inches")
                            {
                                _Height = (Convert.ToDecimal(objMemberDetails.objLifeStyleQuetions.Height) * (2.54m));

                            }
                            else if (objMemberDetails.objLifeStyleQuetions.HeightUnit == "Cms")
                            {
                                _Height = Convert.ToDecimal(objMemberDetails.objLifeStyleQuetions.Height);
                            }

                        }
                        #endregion
                        decimal Height = _Height / 100;
                        decimal Weight = Convert.ToDecimal(objMemberDetails.objLifeStyleQuetions.Weight);
                        ValueToReturn = Convert.ToString(Weight / (Height * Height));
                    }
                    else
                    {
                        ValueToReturn = string.Empty;
                    }
                    break;
                    case "AnnualIncome":
                    ValueToReturn = objMemberDetails.MonthlyIncome;
                    break;
                    case "PrevClaimFlag":
                    ValueToReturn = string.Empty;
                    break;
                    case "Diabetes":
                    ValueToReturn = string.Empty;
                    break;
                    case "Asthma":
                    ValueToReturn = string.Empty;
                    break;
                    case "Thyroid":
                    ValueToReturn = string.Empty;
                    break;
                    case "Nationality":
                    ValueToReturn = string.Empty;
                    break;
                    case "PolicyOwnerLifeAssuredSame":
                    if (objMemberDetails.AssuredName == "MainLife")
                    {
                        ValueToReturn = "True";
                    }
                    else
                    {
                        ValueToReturn = "False";
                    }
                    break;
                    case "NoofSticksSmokingPerDay":
                    int SmokeCount = 0;
                    if (objMemberDetails.objLifeStyleQuetions != null)
                    {
                        if (objMemberDetails.objLifeStyleQuetions.IsSmoker)
                        {
                            if (objMemberDetails.objLifeStyleQuetions.objSmokeDetails != null && objMemberDetails.objLifeStyleQuetions.objSmokeDetails.Count() > 0)
                            {


                                foreach (var item in objMemberDetails.objLifeStyleQuetions.objSmokeDetails)
                                {
                                    SmokeCount = SmokeCount + Convert.ToInt32(item.SmokeQuantity);
                                }

                            }
                        }
                    }
                    ValueToReturn = SmokeCount.ToString();
                    break;
                    case "SumAssured":
                    ValueToReturn = objMemberDetails.BasicSumInsured;
                    break;
                    case "AlcoholQtyPerWeek":
                    ValueToReturn = string.Empty;
                    break;
                    case "AlcoholType":
                    ValueToReturn = string.Empty;
                    break;
                    case "AllocationType":
                    ValueToReturn = string.Empty;
                    break;

                    case "AnnualPremium":
                    ValueToReturn = Convert.ToString(objPolicy.AnnualPremium);
                    break;
                    case "ASBRider":
                    ValueToReturn = string.Empty;
                    break;
                    case "FamilyQ1":
                    ValueToReturn = string.Empty;
                    break;
                    case "FamilyQ2":
                    ValueToReturn = string.Empty;
                    break;
                    case "OccupationClass":
                    if (objMemberDetails.OccupationID > 0)
                    {
                        ValueToReturn = Context.tblMasLifeOccupations.Where(a => a.Code == objMemberDetails.OccupationID).FirstOrDefault().ClassType;
                        // ValueToReturn = Context.tblMasLifeOccupations.Where(a => a.ID == objMemberDetails.OccupationID).FirstOrDefault().ClassType;
                    }
                    else
                    {
                        ValueToReturn = string.Empty;
                    }
                    break;
                    case "OtherDocUploaded":
                    ValueToReturn = string.Empty;
                    break;
                    case "PregnancyPeriod":
                    ValueToReturn = string.Empty;
                    break;
                    case "PrevPolExpiryMonths":
                    ValueToReturn = string.Empty;
                    break;
                    case "PrevPolicyIndex":
                    ValueToReturn = string.Empty;
                    break;
                    case "ProductId":
                    ValueToReturn = Convert.ToString(ProductItem.ProductId);
                    break;
                    case "ResidencialStatus":
                    ValueToReturn = string.Empty;
                    break;
                    case "SUC":
                    ValueToReturn = string.Empty;
                    break;
                    case "TAP":
                    ValueToReturn = string.Empty;
                    break;
                    case "US_Citizen":
                    ValueToReturn = string.Empty;
                    break;
                    case "SIB1":
                    ValueToReturn = string.Empty;
                    break;
                    case "SASB1":
                    ValueToReturn = string.Empty;
                    break;
                    case "HIP1":
                    ValueToReturn = string.Empty;
                    break;
                    case "CHIP1":
                    ValueToReturn = string.Empty;
                    break;
                    case "SCIB1":
                    ValueToReturn = string.Empty;
                    break;
                    case "Backdating":
                    ValueToReturn = string.Empty;
                    break;
                    case "PrevPolCancellationDuration":
                    ValueToReturn = string.Empty;
                    break;
                    //case "ProductId":
                    //    ValueToReturn = string.Empty;
                    //    break;
                    case "PrevOE":
                    ValueToReturn = string.Empty;
                    break;
                    case "CurrOE":
                    ValueToReturn = string.Empty;
                    break;
                    //case "Gender":
                    //    if (objMemberDetails.Gender == "20")
                    //    {
                    //        ValueToReturn= "M";
                    //    }
                    //    else if (objMemberDetails.Gender == "21")
                    //    {
                    //        ValueToReturn="F";
                    //    }
                    //    else
                    //        ValueToReturn= objMemberDetails.Gender;
                    //    break;
                    default:
                    ValueToReturn = string.Empty;
                    break;
                }
                return ValueToReturn;
            }
            catch (Exception ex)
            {

                return string.Empty;
            }
        }

        /// <summary>
        /// To Check For UW Deviation
        /// </summary>
        /// <param name="objPolicy"></param>
        /// <returns></returns>
        public string ValidateDeviation(AIA.Life.Models.Policy.Policy objPolicy)
        {
            try
            {
                bool IsDeviationCase = false;
                var ExistingRuleCount = 0;
                int RuleID = Convert.ToInt32(ConfigurationManager.AppSettings["RuleID"].ToString());
                int i = 0;
                foreach (var item in objPolicy.objMemberDetails)
                {
                    objPolicy.objMemberDetails[i].MemberID = Convert.ToInt32(item.MemberID);
                    i++; 
                }
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    AIA.Life.Models.UWRules.Policy objUWPolicy = new AIA.Life.Models.UWRules.Policy();
                    MapProposalInfoToUWModel(objPolicy, ref objUWPolicy);

                    #region Object To Xml Convertion
                    StringWriter sw = new StringWriter();
                    XmlTextWriter tw = null;
                    string strOutPut = string.Empty;
                    using (var ms = new MemoryStream())
                    {
                        var xw = XmlWriter.Create(ms);// Remember to stop using XmlTextWriter  
                        var serializer = new XmlSerializer(objUWPolicy.GetType());
                        serializer.Serialize(xw, objUWPolicy);
                        xw.Flush();
                        ms.Seek(0, SeekOrigin.Begin);
                        var sr = new StreamReader(ms, System.Text.Encoding.UTF8);
                        strOutPut = sr.ReadToEnd();
                    }
                    #endregion
                    // string strOutPut= sw.ToString();
                    var idParam = new SqlParameter
                    {
                        ParameterName = "RuleSetId",
                        Value = RuleID.ToString()
                    };
                    var idParam1 = new SqlParameter
                    {
                        ParameterName = "xmlStr",
                        Value = strOutPut
                    };

                    #region  Log Input 
                    tbllogxml objlogxml = new tbllogxml();
                    objlogxml.Description = "UW Rule Input";
                    objlogxml.PolicyID = Convert.ToString(objPolicy.PolicyID);
                    objlogxml.UserID = string.Empty;
                    objlogxml.XMlData = strOutPut;
                    objlogxml.CreatedDate = DateTime.Now;
                    Context.tbllogxmls.Add(objlogxml);
                    #endregion

                    List<RuleOutput> Result = Context.Database.SqlQuery<RuleOutput>(
                       "exec usp_ValidateRules @RuleSetId , @xmlStr", idParam, idParam1).ToList();

                    string OutPut = string.Empty;
                    if (Result != null)
                    {
                        foreach (var Record in Result)
                        {
                            var RulesID = Convert.ToInt32(Record.RuleId);
                            var ExistingRule = Context.tblMemberDeviationInfoes.Where(a => a.MemberID == Record.MemberId && a.RuleId == RulesID).FirstOrDefault();

                            if (ExistingRule == null)
                            {
                                tblMemberDeviationInfo objTblMemberDeviationInfo = new tblMemberDeviationInfo();
                                objTblMemberDeviationInfo.RuleId = Convert.ToInt32(Record.RuleId);
                                objTblMemberDeviationInfo.Code = Record.RuleName;
                                objTblMemberDeviationInfo.Reason = Record.RuleDescription;
                                objTblMemberDeviationInfo.MemberID = Record.MemberId;
                                objTblMemberDeviationInfo.IsDeleted = false;
                                objTblMemberDeviationInfo.CreateDate = DateTime.Now;
                                Context.tblMemberDeviationInfoes.Add(objTblMemberDeviationInfo);
                                IsDeviationCase = true;
                            }
                            else
                            {
                                ExistingRuleCount++;
                            }
                        }
                        if ((Result.Count() - ExistingRuleCount) > 0)
                        {
                            OutPut = JsonConvert.SerializeObject(Result);
                        }

                    }

                    #region Add Output to Logs
                    tbllogxml objlogxmlOutput = new tbllogxml();
                    objlogxmlOutput.Description = "UW Rule Output";
                    objlogxmlOutput.PolicyID = Convert.ToString(objPolicy.PolicyID);
                    objlogxmlOutput.UserID = string.Empty;
                    objlogxmlOutput.XMlData = OutPut;
                    objlogxmlOutput.CreatedDate = DateTime.Now;
                    Context.tbllogxmls.Add(objlogxmlOutput);
                    #endregion


                    // Save Info
                    Context.SaveChanges();
                    // Till here

                    if (IsDeviationCase)
                    {
                        SaveRequiredMedicalDocuments(objPolicy);
                        var PolicyId = Context.tblPolicies.Where(a => a.ProposalNo == objPolicy.ProposalNo).Select(a => a.PolicyID).FirstOrDefault();
                        tblPolicyUWRemark objtblPolicyUWRemark = Context.tblPolicyUWRemarks.Where(a => a.PolicyID == PolicyId).FirstOrDefault();
                        var CreatedBy = Context.tblPolicies.Where(a => a.PolicyID == PolicyId).Select(a => a.Createdby).FirstOrDefault();
                        if (objtblPolicyUWRemark == null)
                        {
                            objtblPolicyUWRemark = new tblPolicyUWRemark();
                        }
                        objtblPolicyUWRemark.CreateBy = CreatedBy;
                        objtblPolicyUWRemark.CreatedDate = DateTime.Now;
                        objtblPolicyUWRemark.PolicyID = PolicyId;
                        objtblPolicyUWRemark.Decision = null;
                        if (objtblPolicyUWRemark.RemarkID == 0)
                        {
                            Context.tblPolicyUWRemarks.Add(objtblPolicyUWRemark);
                        }

                        tblPolicyUWRemarkHistory objtblPolicyUWRemarkHistory = Context.tblPolicyUWRemarkHistories.Where(a => a.PolicyID == PolicyId).FirstOrDefault();
                        if (objtblPolicyUWRemarkHistory == null)
                        {
                            objtblPolicyUWRemarkHistory = new tblPolicyUWRemarkHistory();
                        }
                        objtblPolicyUWRemarkHistory.CreatedBy = CreatedBy;
                        objtblPolicyUWRemarkHistory.CreatedDate = DateTime.Now;
                        objtblPolicyUWRemarkHistory.PolicyID = PolicyId;
                        objtblPolicyUWRemarkHistory.Decision = null;
                        if (objtblPolicyUWRemarkHistory.PolicyUWRemarkHistoryID == 0)
                        {
                            Context.tblPolicyUWRemarkHistories.Add(objtblPolicyUWRemarkHistory);
                        }
                        Context.SaveChanges();
                        return "Your proposal has been forwarded to the Underwriter for further processing.";
                    }
                    else
                    {
                        return string.Empty;
                    }

                }
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                return string.Empty;
            }

        }
        /// <summary>
        /// To get Property value Dynamically
        /// </summary>
        /// <param name="src"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static string GetPropValue(object src, string propName)
        {
            try
            {
                var output = src.GetType().GetProperty(propName).GetValue(src, null);
                if (output != null)
                {
                    return Convert.ToString(output);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        /// <summary>
        ///Mapping Proposal information to UW Model to validate  deviation
        /// </summary>
        /// <param name="objPolicy"></param>
        /// <param name="objUWPolicy"></param>
        public void MapProposalInfoToUWModel(AIA.Life.Models.Policy.Policy objPolicy, ref AIA.Life.Models.UWRules.Policy objUWPolicy)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    #region Policy Level
                    LifeAssuredAge assuredAge = new LifeAssuredAge();
                    assuredAge.QuoteNo = objPolicy.QuoteNo;
                    Common.CommonBusiness common = new Common.CommonBusiness();
                    assuredAge = common.CheckAgeChangeQuoteMembers(assuredAge);
                    int FhecCount = 0;
                    foreach (var member in objPolicy.objMemberDetails)
                    {
                        foreach (var benifit in member.objBenifitDetails)
                        {
                            if(benifit.BenifitOpted)
                            {
                                if (benifit.RiderID == 13 || benifit.RiderID == 15)
                                    FhecCount = FhecCount + 1;
                            }
                        }
                    }
                    objUWPolicy.Item = new List<Item>();
                    foreach (var Parameter in Context.tblMasRuleParameters.Where(a => a.ObjectName == "Policy").ToList())
                    {
                        Item objItem = new Item();
                        objItem.ParameterName = Parameter.ParameterName;
                        if (!string.IsNullOrEmpty(Parameter.PropertyName))
                        {
                            switch (Parameter.PropertyName)
                            {
                                case "Backdating":
                                var aa = objPolicy.objMemberDetails[0].objLstWealthPlannerQuestions;
                                var IsBackDating = objPolicy.objMemberDetails[0].objLstWealthPlannerQuestions.Where(a => a.QuestionText == "Is policy back dating required ?").Select(a => a.Answer).FirstOrDefault();
                                if (IsBackDating == "true")
                                    objItem.ParameterValue = "true";
                                else
                                    objItem.ParameterValue = string.Empty;
                                break;
                                case "MAgeChg":
                                if (assuredAge.MainLifeAge)
                                    objItem.ParameterValue = "true";
                                else
                                    objItem.ParameterValue = string.Empty;
                                break;
                                case "SAgeChg":
                                if (assuredAge.SpouseAge)
                                    objItem.ParameterValue = "true";
                                else
                                    objItem.ParameterValue = string.Empty;
                                break;
                                case "C1AgeChg":
                                if (assuredAge.Child1Age)
                                    objItem.ParameterValue = "true";
                                else
                                    objItem.ParameterValue = string.Empty;
                                break;
                                case "C2AgeChg":
                                if (assuredAge.Child2Age)
                                    objItem.ParameterValue = "true";
                                else
                                    objItem.ParameterValue = string.Empty;
                                break;
                                case "C3AgeChg":
                                if (assuredAge.Child3Age)
                                    objItem.ParameterValue = "true";
                                else
                                    objItem.ParameterValue = string.Empty;
                                break;
                                case "C4AgeChg":
                                if (assuredAge.Child4Age)
                                    objItem.ParameterValue = "true";
                                else
                                    objItem.ParameterValue = string.Empty;
                                break;
                                case "C5AgeChg":
                                if (assuredAge.Child5Age)
                                    objItem.ParameterValue = "true";
                                else
                                    objItem.ParameterValue = string.Empty;
                                break;
                                case "FHECCNTLESS":
                                    if (FhecCount!=0 && objPolicy.objMemberDetails.Count > FhecCount)
                                        objItem.ParameterValue = "true";
                                    else
                                        objItem.ParameterValue = "false";
                                    break;
                                default:
                                objItem.ParameterValue = GetPropValue(objPolicy, Parameter.PropertyName);
                                break;
                            }
                            objUWPolicy.Item.Add(objItem);
                        }
                    }
                    List<usp_GetDocumentsForPDF_Result> DocList = new List<usp_GetDocumentsForPDF_Result>();
                    if (!string.IsNullOrEmpty(objPolicy.QuoteNo))
                    {

                        DocList = Context.usp_GetDocumentsForPDF(objPolicy.QuoteNo).ToList();

                    }
                    #endregion

                    #region Member Level
                    objUWPolicy.Member = new List<Member>();
                    int MemberCount = 0;
                    foreach (var Member in objPolicy.objMemberDetails)
                    {
                        Member objMember = new Member();
                        objMember.Id = Convert.ToString(Member.MemberID);
                        objMember.Item = new List<Item>();
                        foreach (var Parameter in Context.tblMasRuleParameters.Where(a => a.ObjectName == "Member").ToList())
                        {
                            Item objItem = new Item();
                            objItem.ParameterName = Parameter.ParameterName;
                            if (!string.IsNullOrEmpty(Parameter.PropertyName))
                            {
                                objItem.ParameterValue = GetPropValue(Member, Parameter.PropertyName);
                                objMember.Item.Add(objItem);
                            }
                            else
                            {

                                #region Other Member Level Parameters
                                try
                                {
                                    switch (Parameter.ParameterName)
                                    {
                                        case "PregnancyPeriod":
                                        var PregnencyPeriod = Context.tblMemberQuestions.Where(a => a.QID == 349 && a.MemberID == Member.MemberID).FirstOrDefault();
                                        if (PregnencyPeriod != null)
                                        {
                                            objItem.ParameterValue = PregnencyPeriod.Answer;
                                            objMember.Item.Add(objItem);
                                        }
                                        break;
                                        case "OtherDocUploaded":
                                        var Count = Context.tblPolicyDocuments.Where(a => a.PolicyID == objPolicy.PolicyID && a.MemberType == Member.AssuredName && a.ItemType == "PolicyDocuments" && a.FileName != "Age Proof" && a.FilePath != null && a.Decision != "2370").Count();
                                        if (Count > 0)
                                        {
                                            objItem.ParameterValue = "true";
                                        }
                                        else
                                        {
                                            objItem.ParameterValue = "false";
                                        }
                                        objMember.Item.Add(objItem);
                                        break;
                                        case "PolicyOwnerLifeAssuredSame":
                                        if (objPolicy.objProspectDetails != null)
                                        {
                                            if (objPolicy.objProspectDetails.IsproposerlifeAssured == true)
                                            {
                                                objItem.ParameterValue = "true";
                                            }
                                            else
                                            {
                                                objItem.ParameterValue = "false";
                                            }
                                        }
                                        else
                                        {
                                            objItem.ParameterValue = "false";
                                        }
                                        objMember.Item.Add(objItem);
                                        break;
                                        case "ProofOfAge":
                                        var Count1 = Context.tblPolicyDocuments.Where(a => a.PolicyID == objPolicy.PolicyID && a.MemberType == Member.AssuredName && a.ItemType == "PolicyDocuments" && a.FileName == "Age Proof").Count();
                                        if (Count1 > 0)
                                        {
                                            objItem.ParameterValue = "false";
                                        }
                                        else
                                        {
                                            objItem.ParameterValue = "true";
                                        }
                                        objMember.Item.Add(objItem);
                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {

                                    log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                                    Logger.Error(ex);
                                }
                                #endregion


                            }
                        }


                        #region Riders
                        objMember.Rider = new Rider();
                        objMember.Rider.Item = new List<Item>();
                        objMember.Rider.Item = (from Rider in Member.objBenifitDetails
                                                select new Item
                                                {
                                                    ParameterName = Rider.RiderCode,
                                                    ParameterValue = "true"
                                                }).ToList();

                        #endregion

                        #region Health Questions
                        objMember.HealthQuestions = new HealthQuestions();
                        objMember.HealthQuestions.Item = new List<Item>();
                        foreach (var Parameter in Context.tblMasRuleParameters.Where(a => a.ObjectName == "HealthQuestions").ToList())
                        {
                            foreach (var MedicalQuestion in Member.objLstMedicalHistory.Where(a => a.QuestionID != 256)) // Pregnancy Questions is removed from list only No of months value will be taken for UW
                            {
                                Item objItem = new Item();
                                objItem.ParameterName = Convert.ToString(MedicalQuestion.QuestionID);
                                objItem.ParameterValue = MedicalQuestion.Answer;
                                objMember.HealthQuestions.Item.Add(objItem);
                            }

                        }

                        #endregion

                        #region Family Questions
                        objMember.FamilyQuestions = new FamilyQuestions();
                        objMember.FamilyQuestions.Item = new List<Item>();
                        foreach (var Parameter in Context.tblMasRuleParameters.Where(a => a.ObjectName == "FamilyQuestions").ToList())
                        {
                            foreach (var FamilyQuestion in Member.objLstFamily)
                            {
                                Item objItem = new Item();
                                objItem.ParameterName = Convert.ToString(FamilyQuestion.QuestionID);
                                objItem.ParameterValue = FamilyQuestion.Answer;
                                objMember.FamilyQuestions.Item.Add(objItem);
                            }

                        }
                        #endregion

                        #region LifeStyle
                        objMember.LifeStyle = new LifeStyle();
                        objMember.LifeStyle.Item = new List<Item>();

                        foreach (var Parameter in Context.tblMasRuleParameters.Where(a => a.ObjectName == "LifeStyle" && a.ParameterName != "LifeQuestions").ToList())
                        {
                            Item objItem = new Item();
                            objItem.ParameterName = Parameter.ParameterName;
                            if (Parameter.ParameterName == "BMI")
                            {
                                #region BMI
                                try
                                {
                                    if (Member.objLifeStyleQuetions != null)
                                    {
                                        decimal _Height = decimal.Zero;
                                        #region Converting Height In other Formats to CMS

                                        if (Member.objLifeStyleQuetions.HeightFeets != null)
                                        {
                                            if (Member.objLifeStyleQuetions.HeightFeets == 2411)//Ft
                                            {
                                                _Height = (Convert.ToDecimal(Member.objLifeStyleQuetions.Height) * (30.48m));
                                            }
                                            else if (Member.objLifeStyleQuetions.HeightFeets == 2412)//Inches
                                            {
                                                _Height = (Convert.ToDecimal(Member.objLifeStyleQuetions.Height) * (2.54m));

                                            }
                                            else if (Member.objLifeStyleQuetions.HeightFeets == 2413)// CMS
                                            {
                                                _Height = Convert.ToDecimal(Member.objLifeStyleQuetions.Height);
                                            }
                                            if (_Height > 0)
                                            {
                                                decimal Height = _Height / 100;
                                                decimal Weight = Convert.ToDecimal(Member.objLifeStyleQuetions.Weight);
                                                objItem.ParameterValue = Convert.ToString(Math.Round((Weight / (Height * Height))));
                                            }
                                            else
                                            {
                                                objItem.ParameterValue = string.Empty;
                                            }


                                        }
                                        else
                                        {
                                            objItem.ParameterValue = string.Empty;
                                        }
                                        #endregion

                                    }
                                    else
                                    {
                                        objItem.ParameterValue = string.Empty;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    objItem.ParameterValue = string.Empty;
                                    log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                                    Logger.Error(ex);
                                }

                                #endregion

                            }
                            else if (Parameter.ParameterName == "NoofSticksSmokingPerDay")
                            {

                                #region Smoke
                                try
                                {
                                    if (Member.objLifeStyleQuetions != null)
                                    {
                                        if (Member.objLifeStyleQuetions.objSmokeDetails != null && Member.objLifeStyleQuetions.objSmokeDetails.Count() > 0)
                                        {
                                            if (Member.objLifeStyleQuetions.objSmokeDetails[0].SmokeQuantity == "1-5")
                                            {
                                                objItem.ParameterValue = "5";
                                            }
                                            else if (Member.objLifeStyleQuetions.objSmokeDetails[0].SmokeQuantity == "6-10")
                                            {
                                                objItem.ParameterValue = "10";
                                            }
                                            else if (Member.objLifeStyleQuetions.objSmokeDetails[0].SmokeQuantity == "10-20")
                                            {
                                                objItem.ParameterValue = "20";
                                            }
                                            else if (Member.objLifeStyleQuetions.objSmokeDetails[0].SmokeQuantity == "20 Above")
                                            {
                                                objItem.ParameterValue = "21";
                                            }
                                            else
                                            {
                                                objItem.ParameterValue = string.Empty;
                                            }
                                        }
                                        else
                                        {
                                            objItem.ParameterValue = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        objItem.ParameterValue = string.Empty;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    objItem.ParameterValue = string.Empty;
                                    log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                                    Logger.Error(ex);
                                }

                                #endregion
                            }
                            objMember.LifeStyle.Item.Add(objItem);

                        }

                        #region Life Style Extra Question
                        if (Member.Questions != null)
                        {
                            foreach (var Questions in Member.Questions)
                            {
                                Item newItem = new Item();
                                newItem.ParameterName = Convert.ToString(Questions.QuestionID);
                                newItem.ParameterValue = Questions.Answer;
                                objMember.LifeStyle.Item.Add(newItem);
                            }
                        }
                        #endregion

                        #endregion

                        #region Alcohol
                        objMember.LifeStyle.Alcohol = new List<Alcohol>();
                        Alcohol objAlcohol = new Alcohol();
                        objAlcohol.Item = new List<Item>();

                        #region Alcohol
                        try
                        {
                            if (Member.objLifeStyleQuetions != null)
                            {
                                if (Member.objLifeStyleQuetions.objAlcoholDetails != null)
                                {
                                    for (int i = 0; i < Member.objLifeStyleQuetions.objAlcoholDetails.Count(); i++)
                                    {
                                        objAlcohol.Id = Convert.ToString((i + 1));
                                        foreach (var Parameter in Context.tblMasRuleParameters.Where(a => a.ObjectName == "Alcohol").ToList())
                                        {
                                            Item objItem = new Item();
                                            objItem.ParameterName = Parameter.ParameterName;
                                            if (!string.IsNullOrEmpty(Parameter.PropertyName))
                                            {
                                                objItem.ParameterValue = GetPropValue(Member.objLifeStyleQuetions.objAlcoholDetails[i], Parameter.PropertyName);
                                                objAlcohol.Item.Add(objItem);
                                            }
                                        }
                                        objMember.LifeStyle.Alcohol.Add(objAlcohol);
                                    }


                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                            Logger.Error(ex);
                        }
                        #endregion

                        #endregion

                        #region Additional Questions
                        objMember.AdditionalQuestions = new AdditionalQuestions();
                        objMember.AdditionalQuestions.Item = new List<Item>();
                        foreach (var Parameter in Context.tblMasRuleParameters.Where(a => a.ObjectName == "AdditionalQuestions").ToList())
                        {
                            foreach (var Question in Member.objAdditionalQuestions.Where(a => a.ControlType != "TextBox"))
                            {
                                Item objItem = new Item();
                                objItem.ParameterName = Convert.ToString(Question.QuestionID);
                                objItem.ParameterValue = Question.Answer;
                                objMember.AdditionalQuestions.Item.Add(objItem);
                            }

                        }
                        #endregion

                        #region Previous Policy
                        objMember.PreviousPolicy = new List<PreviousPolicy>();

                        #region Previous Policy
                        try
                        {
                            List<usp_GetPreviousPolicyDetailsForUW_Result> PrevList = new List<usp_GetPreviousPolicyDetailsForUW_Result>();
                            if (!string.IsNullOrEmpty(Member.NewNICNO))
                            {

                                PrevList = Context.usp_GetPreviousPolicyDetailsForUW(Member.NewNICNO).ToList();

                            }
                            if (PrevList != null)
                            {
                                foreach (var Previtem in PrevList)
                                {

                                    PreviousPolicy objPrevPolicy = new PreviousPolicy();
                                    objPrevPolicy.Item = new List<Item>();
                                    objPrevPolicy.Id = Previtem.POLICYNO;
                                    foreach (var Parameter in Context.tblMasRuleParameters.Where(a => a.ObjectName == "PreviousPolicy").ToList())
                                    {
                                        Item objItem = new Item();
                                        objItem.ParameterName = Parameter.ParameterName;
                                        if (!string.IsNullOrEmpty(Parameter.PropertyName))
                                        {
                                            objItem.ParameterValue = GetPropValue(Previtem, Parameter.PropertyName);
                                            objPrevPolicy.Item.Add(objItem);
                                        }
                                    }
                                    objPrevPolicy.PreviousRiders = new List<PreviousRiders>();
                                    //var PrevRiderList = Context.Sp_GetOccupationalLoadingDetails(Previtem.POLICYNO).ToList();
                                    //if (PrevRiderList != null)
                                    //{
                                    //    foreach (var _Rider in PrevRiderList)
                                    //    {
                                    //        PreviousRiders prevRider = new PreviousRiders();
                                    //        prevRider.Id = string.Empty;
                                    //        prevRider.Item = new List<Item>();
                                    //        foreach (var Parameter in Context.tblMasRuleParameters.Where(a => a.ObjectName == "PrevRiders").ToList())
                                    //        {
                                    //            Item objItem = new Item();
                                    //            objItem.ParameterName = Parameter.ParameterName;
                                    //            if (!string.IsNullOrEmpty(Parameter.PropertyName))
                                    //            {

                                    //                objItem.ParameterValue = GetPropValue(_Rider, Parameter.PropertyName);
                                    //                prevRider.Item.Add(objItem);
                                    //            }
                                    //            else if (Parameter.ParameterName == "CurrOE")
                                    //            {
                                    //                objItem.ParameterValue = "0"; // Default Value
                                    //                #region Set Current OE
                                    //                var QuoteInfo = Context.tblLifeQQs.Where(a => a.QuoteNo == objPolicy.QuoteNo).FirstOrDefault();
                                    //                if (QuoteInfo != null)
                                    //                {
                                    //                    var _RiderInfo = Context.tblProductPlanRiders.Where(a => a.PlanId == QuoteInfo.PlanId && (a.RefRiderCode == _Rider.CRTABLE || a.RefOldRiderCode == _Rider.CRTABLE)).FirstOrDefault();
                                    //                    if (_RiderInfo != null)
                                    //                    {
                                    //                        prevRider.Id = Convert.ToString(_RiderInfo.ProductPlanRiderId);// Set ID 
                                    //                        var MemberRiderInfo = Context.tblQuoteMemberBeniftDetials.Where(a => a.MemberID == Member.QuoteMemberID && a.BenifitID == _RiderInfo.ProductPlanRiderId).FirstOrDefault();

                                    //                        if (MemberRiderInfo != null)
                                    //                        {
                                    //                            if (MemberRiderInfo.LoadingPercentage > 0)
                                    //                            {
                                    //                                objItem.ParameterValue = Convert.ToString(MemberRiderInfo.LoadingPercentage);
                                    //                            }

                                    //                        }
                                    //                    }
                                    //                }
                                    //                #endregion
                                    //                prevRider.Item.Add(objItem);
                                    //            }
                                    //        }
                                    //        objPrevPolicy.PreviousRiders.Add(prevRider);

                                    //    }
                                    //}
                                    objMember.PreviousPolicy.Add(objPrevPolicy);
                                }

                            }
                        }
                        catch (Exception ex)
                        {

                            log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                            Logger.Error(ex);
                        }
                        #endregion

                        #endregion

                        #region Medical and Non-Medical Document
                        if (DocList != null)
                        {
                            if (DocList.Count > 0)
                            {
                                #region Medical Document
                                foreach (var Parameter in Context.tblMasRuleParameters.Where(a => a.ObjectName == "MedicalDoc").ToList())
                                {
                                    Item objItem = new Item();
                                    objItem.ParameterName = Parameter.ParameterName;
                                    var objMedDoc = DocList.Where(a => a.RelationId == (Member.RelationShipWithPropspectID == "267" ? 1 : (Member.RelationShipWithPropspectID == "268" ? 2 : 99))).FirstOrDefault();
                                    objItem.ParameterValue = "false";
                                    if (objMedDoc != null)
                                    {
                                        if (objMedDoc.MedicalDoc != null)
                                            objItem.ParameterValue = "true";
                                    }
                                    objMember.Item.Add(objItem);
                                }
                                #endregion

                                #region Financial Document
                                foreach (var Parameter in Context.tblMasRuleParameters.Where(a => a.ObjectName == "FinancialDoc").ToList())
                                {
                                    Item objItem = new Item();
                                    objItem.ParameterName = Parameter.ParameterName;
                                    var objFinacialDoc = DocList.Where(a => a.RelationId == (Member.RelationShipWithPropspectID == "267" ? 1 : (Member.RelationShipWithPropspectID == "268" ? 2 : 99))).FirstOrDefault();
                                    objItem.ParameterValue = "false";
                                    if (objFinacialDoc != null)
                                    {
                                        if (objFinacialDoc.FinancialDoc != null)
                                            objItem.ParameterValue = "true";
                                    }
                                    objMember.Item.Add(objItem);
                                }
                                #endregion
                            }

                            //}

                        }
                        #endregion

                        objUWPolicy.Member.Add(objMember);

                        MemberCount++;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);

            }
        }
        /// <summary>
        /// Save Required Medical Documents
        /// </summary>
        /// <param name="ObjMemberDetails"></param>
        /// <param name="QuoteNo"></param>
        /// <returns></returns>
        public bool SaveRequiredMedicalDocuments(AIA.Life.Models.Policy.Policy objPolicy)
        {

            try
            {
                foreach (var Member in objPolicy.objMemberDetails)
                {
                    #region Save Member Required Documents
                    AIA.Life.Models.UWRules.Documents.ProdRiders objProductRiders = new AIA.Life.Models.UWRules.Documents.ProdRiders();
                    objProductRiders.ProdRiderData = new List<AIA.Life.Models.UWRules.Documents.ProdRiderData>();
                    using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                    {
                        #region Member Data
                        AIA.Life.Models.UWRules.Documents.ProdRiderData objMemberData = new AIA.Life.Models.UWRules.Documents.ProdRiderData();
                        objMemberData.ProductId = Convert.ToString(objPolicy.ProductID);
                        objMemberData.Age = Convert.ToString(Member.Age);
                        objMemberData.UserId = Context.tblPolicies.Where(a => a.ProposalNo == objPolicy.ProposalNo).Select(a => a.Createdby).FirstOrDefault();
                        if (!string.IsNullOrEmpty(Member.Gender))
                        {
                            // int _GenderID = Convert.ToInt32(Member.Gender);
                            objMemberData.Gender = Context.tblMasCommonTypes.Where(a => a.MasterType == "Gender" && a.Code == Member.Gender).FirstOrDefault().Description;
                        }
                        objMemberData.SumInsured = Member.BasicSumInsured;
                        objProductRiders.ProdRiderData.Add(objMemberData);
                        #endregion

                        #region Rider Information
                        if (Member.objBenifitDetails == null)
                        {
                            Member.objBenifitDetails = new List<BenifitDetails>();
                        }
                        #region Added Condition For Counter Offer Case Data is not present in Screen
                        if (Member.objBenifitDetails.Count() == 0)
                        {
                            foreach (var Rider in Context.tblPolicyMemberBenefitDetails.Where(a => a.MemberID == Member.MemberID).ToList())
                            {
                                BenifitDetails objBenefitDetail = new BenifitDetails();
                                objBenefitDetail.RiderPremium = Rider.Premium;
                                var RiderInfo = Context.tblProductPlanRiders.Where(a => a.ProductPlanRiderId == Rider.BenifitID).FirstOrDefault();
                                if (RiderInfo != null)
                                {
                                    objBenefitDetail.RiderCode = RiderInfo.RefRiderCode;

                                }
                                objBenefitDetail.RiderSuminsured = Rider.SumInsured;
                                Member.objBenifitDetails.Add(objBenefitDetail);
                            }
                        }
                        #endregion

                        foreach (var Rider in Member.objBenifitDetails)
                        {
                            if (Rider.BenefitID > 0)
                            {
                                AIA.Life.Models.UWRules.Documents.ProdRiderData objRiderData = new AIA.Life.Models.UWRules.Documents.ProdRiderData();
                                objRiderData.SumInsured = Rider.RiderSuminsured;
                                objRiderData.RiderId = Convert.ToString(Rider.BenefitID);
                                objProductRiders.ProdRiderData.Add(objRiderData);
                            }
                        }
                        #endregion

                        #region Object To Xml Convertion
                        StringWriter sw = new StringWriter();
                        XmlTextWriter tw = null;
                        string strOutPut = string.Empty;
                        using (var ms = new MemoryStream())
                        {
                            var xw = XmlWriter.Create(ms);// Remember to stop using XmlTextWriter  
                            var serializer = new XmlSerializer(objProductRiders.GetType());
                            serializer.Serialize(xw, objProductRiders);
                            xw.Flush();
                            ms.Seek(0, SeekOrigin.Begin);
                            var sr = new StreamReader(ms, System.Text.Encoding.UTF8);
                            strOutPut = sr.ReadToEnd();
                        }
                        #endregion

                        // string strOutPut= sw.ToString();

                        var idParam = new SqlParameter
                        {
                            ParameterName = "xmlStr",
                            Value = strOutPut
                        };

                        List<DoumentOutput> Result = Context.Database.SqlQuery<DoumentOutput>(
                           "exec usp_FetchProdRiderDocs   @xmlstr", idParam).ToList();
                        if (Result != null)
                        {
                            foreach (var Doc in Result)
                            {
                                if (!CheckForExisitngDocument(Doc.DocumentName, objPolicy.PolicyID, Member.AssuredName))
                                {
                                    tblPolicyDocument objtblpolicyDocument = new tblPolicyDocument();
                                    objtblpolicyDocument.MemberType = Member.AssuredName;
                                    objtblpolicyDocument.PolicyID = objPolicy.PolicyID;
                                    objtblpolicyDocument.ItemType = "PolicyDocuments";
                                    objtblpolicyDocument.FileName = Doc.DocumentName;
                                    objtblpolicyDocument.DocumentType = Doc.DocumentType;
                                    Context.tblPolicyDocuments.Add(objtblpolicyDocument);
                                }

                            }
                            Context.SaveChanges();
                        }
                    }





                    #endregion
                }
                return true;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                return false;

            }
        }

        public bool CheckForExisitngDocument(string DocumentName, decimal PolicyID, string MemberType)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    if (Context.tblPolicyDocuments.Where(a => a.PolicyID == PolicyID && a.ItemType == "PolicyDocuments" && a.FileName == DocumentName && a.MemberType == MemberType).Count() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}