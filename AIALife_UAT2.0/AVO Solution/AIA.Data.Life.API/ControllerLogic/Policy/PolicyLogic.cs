using AIA.Life.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MoreLinq;
using AIA.Life.Repository.AIAEntity;
using AIA.Life.Models.Policy;
using AIA.Data.Life.API.ControllerLogic.Common;
using System.Data;
using System.Web.Mvc;
using AIA.Life.Models.UWDecision;
using AIA.Data.Life.API.ControllerLogic.UWRules;
using System.IO;
using System.Configuration;
using AIA.Life.Models.Reports;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Serialization;
using log4net;
using AIA.CrossCutting;
using AIA.Life.Integration.Services.EmailandSMS;
using AIA.Life.Models.EmailSMSDetails;
using System.Data.Entity;
using AIA.Life.Integration.Services.SamsIntegration;
using System.Threading.Tasks;
using AIA.Life.Integration.Services.LifeAsiaIntegration;
using System.Threading;
using System.Text;
using System.Globalization;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace AIA.Data.Life.API.ControllerLogic.Policy
{

    public class PolicyLogic
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        public string descision = string.Empty;

        public AIA.Life.Models.Policy.Policy FetchProposalNomineeDetailsWithMemberDetails(AIA.Life.Models.Policy.Policy objPolicy)
        {
            string userId = Common.CommonBusiness.GetUserId(objPolicy.UserName);
            AVOAIALifeEntities Context = new AVOAIALifeEntities();

            objPolicy.ObjNomineeLifeAssuredDetails = (from objpolicyNominee in objPolicy.objMemberDetails.Where(a => a.RelationShipWithPropspectID == objPolicy.NomineeRelationshipDetailsID && a.IsLifeAssuredSeleted != true).ToList()
                                                      select new NomineeLifeAssuredDetails
                                                      {
                                                          RelationShipWithPropspectID = objpolicyNominee.RelationShipWithPropspectID,
                                                          RelationShipwithProposer = objpolicyNominee.RelationShipWithPropspect,
                                                          Salutation = objpolicyNominee.Salutation,
                                                          FirstName = objpolicyNominee.FirstName,
                                                          LastName = objpolicyNominee.LastName,
                                                          MobileNo = objpolicyNominee.MobileNo,
                                                          DateOfBirth = objpolicyNominee.DateOfBirth.ToString(),
                                                          Gender = Context.tblMasCommonTypes.Where(a => a.MasterType == "Gender" && a.Code == objpolicyNominee.Gender).Select(a => a.Description).FirstOrDefault(),
                                                          GenderText = objpolicyNominee.GenderText,
                                                          MaritialStatus = objpolicyNominee.MaritialStatus,
                                                          NewNICNO = objpolicyNominee.NewNICNO
                                                      }).ToList();
            for (int i = 0; i < objPolicy.ObjNomineeLifeAssuredDetails.Count; i++)
            {
                objPolicy.ObjNomineeLifeAssuredDetails[i].Index = i;
                objPolicy.ObjNomineeLifeAssuredDetails[i].DateOfBirth = objPolicy.ObjNomineeLifeAssuredDetails[i].DateOfBirth.ToDate().ToString("dd/MM/yyyy");
            }
            return objPolicy;
        }
        public ProposalInbox FetchProposalIncompleteDetails(ProposalInbox objProposalInbox)
        {
            string userId = Common.CommonBusiness.GetUserId(objProposalInbox.UserName);
            AVOAIALifeEntities Context = new AVOAIALifeEntities();

            objProposalInbox.objProposalDetails = (from objtblpolicy in Context.tblPolicies.Where(a => a.PolicyStageStatusID == 476 || a.PolicyStageStatusID == 1153 && a.Createdby == userId)
                                                   join objtbllifeQQ in Context.tblLifeQQs
                                                   on objtblpolicy.QuoteNo equals objtbllifeQQ.QuoteNo
                                                   join Common in Context.tblProducts on objtblpolicy.ProductID equals Common.ProductId
                                                   join Contact in Context.tblContacts
                                                   on objtbllifeQQ.ContactID equals Contact.ContactID
                                                   join relationship in Context.tblPolicyRelationships
                                                   on objtblpolicy.PolicyID equals relationship.PolicyID
                                                   join policyClients in Context.tblPolicyClients
                                                   on relationship.PolicyClientID equals policyClients.PolicyClientID
                                                   join commontype in Context.tblMasCommonTypes
                                                   on objtblpolicy.PolicyStageStatusID equals commontype.CommonTypesID
                                                   where objtblpolicy.Createdby == userId
                                                   select new InboxDetails
                                                   {
                                                       PolicyID = objtblpolicy.PolicyID,
                                                       QuoteNo = objtblpolicy.QuoteNo,
                                                       FirstName = policyClients.FirstName,
                                                       ProposalNo = objtblpolicy.ProposalNo,
                                                       NIC = policyClients.NEWNICNo,
                                                       Salutation = policyClients.Title,
                                                       Surname = policyClients.LastName,
                                                       PreferredLanguage = objtblpolicy.PreferredLanguage,
                                                       ProductCode = Common.ProductCode,
                                                       LeadNo = Contact.LeadNo,
                                                       Banca = Contact.IntroducerCode,
                                                       ProposalStatus = commontype.Description,
                                                       FullName =(policyClients.FullName!= "CORP" ? Context.tblMasCommonTypes.Where(a => a.Code == policyClients.Title).Select(b => b.ShortDesc).FirstOrDefault()+ " " + policyClients.FirstName + " " + policyClients.LastName:policyClients.CorporateName)
                                                   }).DistinctBy(a => a.PolicyID).OrderByDescending(a => a.PolicyID).ToList();
            return objProposalInbox;
        }

        public ProposalInbox FetchProposalSubmittedDetails(ProposalInbox ObjProposalInbox)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                var UserInfo = Context.AspNetUsers.Where(a => a.UserName == ObjProposalInbox.UserName).FirstOrDefault();
                if (UserInfo != null)
                {
                    ObjProposalInbox.LstSubmittedProposals = (from objtblpolicy in Context.tblPolicies.Where(a => a.PolicyStageStatusID == 1153 || a.PolicyStageStatusID == 476 || a.PolicyStageStatusID == 477 || a.PolicyStageStatusID == 193
                                                              || a.PolicyStageStatusID == 191 || a.PolicyStageStatusID == 192 || a.PolicyStageStatusID == 194 || a.PolicyStageStatusID == 195 || a.PolicyStageStatusID == 196 || a.PolicyStageStatusID == 197 || a.PolicyStageStatusID == 198
                                                              || a.PolicyStageStatusID == 1068 || a.PolicyStageStatusID == 1447 || a.PolicyStageStatusID == 1448 || a.PolicyStageStatusID == 2374 || a.PolicyStageStatusID == 2375 || a.PolicyStageStatusID == 2376 || a.PolicyStageStatusID == 2490 || a.PolicyStageStatusID == 2491 && a.Createdby == UserInfo.Id)
                                                              join lifeqq in Context.tblLifeQQs
                                                              on objtblpolicy.QuoteNo equals lifeqq.QuoteNo
                                                              //join Common in Context.tblProducts on objtblpolicy.ProductID equals Common.ProductId
                                                              join Contact in Context.tblContacts
                                                              on lifeqq.ContactID equals Contact.ContactID
                                                              join commontype in Context.tblMasCommonTypes
                                                              on objtblpolicy.PolicyStageStatusID equals commontype.CommonTypesID
                                                              join relationship in Context.tblPolicyRelationships
                                                              on objtblpolicy.PolicyID equals relationship.PolicyID
                                                              join policyClients in Context.tblPolicyClients
                                                              on relationship.PolicyClientID equals policyClients.PolicyClientID
                                                              where objtblpolicy.Createdby == UserInfo.Id

                                                              select new SubmittedProposals
                                                              {
                                                                  PropId = objtblpolicy.PolicyID.ToString(),
                                                                  ProposalNo = objtblpolicy.ProposalNo,
                                                                  QuoteNo = objtblpolicy.QuoteNo,
                                                                  Name = policyClients.FirstName,
                                                                  Surname = policyClients.LastName,
                                                                  NicNo = policyClients.NEWNICNo,
                                                                  Salutation = policyClients.Title,
                                                                  //SubmittedPropMobile = objtblpolicyclients.MobileNo,
                                                                  //SubmittedPropHome = objtblpolicyclients.HomeNo,
                                                                  //SubmittedPropWork = objtblpolicyclients.WorkNo,
                                                                  // SubmittedPropEmail = objtblpolicyclients.EmailID,
                                                                  LeadNo = Contact.LeadNo,
                                                                  Status = commontype.Description,
                                                                  FullName = (policyClients.FullName != "CORP" ? Context.tblMasCommonTypes.Where(a => a.Code == policyClients.Title).Select(b => b.ShortDesc).FirstOrDefault() + " " + policyClients.FirstName + " " + policyClients.LastName : policyClients.CorporateName)
                                                                  //SubmittedPropInforce = "",
                                                              }).DistinctBy(a => a.PropId).OrderByDescending(a => a.ProposalNo).ToList();
                }
                if (ObjProposalInbox.LstSubmittedProposals == null)
                {
                    ObjProposalInbox.LstSubmittedProposals = new List<SubmittedProposals>();
                }
            }

            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
            }
            return ObjProposalInbox;

        }

        public AIA.Life.Models.Policy.UWInbox FetchUWHomeProposalCount(AIA.Life.Models.Policy.UWInbox objUWInbox)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    var UserInfo = Context.AspNetUsers.Where(a => a.UserName == objUWInbox.UserName).FirstOrDefault();
                    if (UserInfo != null)
                    {


                        objUWInbox.UWPoolCount = (from Allocation in Context.tblPolicyUWAllocations.Where(a => a.AllocatedTo == UserInfo.Id)
                                                  join _Policy in Context.tblPolicies.Where(a => a.PolicyStageStatusID == CrossCuttingConstants.PolicyStageStatusReferToUW)
                                                  on Allocation.PolicyID equals _Policy.PolicyID
                                                  select new { Policyid = _Policy.PolicyID }).Count();
                        objUWInbox.AllocationCount = Context.tblPolicies.Where(a => a.PolicyStageStatusID == CrossCuttingConstants.PolicyStageStatusReferToUW && a.IsAllocated == false).Count();

                        List<decimal?> PolicyIDs = (Context.tblPolicyUWRemarkHistories.Where(a => a.CreatedBy == UserInfo.Id && a.CommonID != null
                                                    && (a.Decision == CrossCuttingConstants.UWDecisionAccepted || a.Decision == CrossCuttingConstants.UWDecisionDecline
                                                    || a.Decision == CrossCuttingConstants.UWDecisionPostPone || a.Decision == CrossCuttingConstants.UWDecisionWithDrawn || a.Decision == CrossCuttingConstants.UWDecisionReferToUW
                                                     || a.Decision == CrossCuttingConstants.UWDecisionOutStandingReq || a.Decision == CrossCuttingConstants.UWDecisionNotTaken
                                                     || a.Decision == CrossCuttingConstants.UWDecisionCounterOffer)).Select(a => a.PolicyID)).ToList();


                        var ListOfSubmittedProposals = (from PolicyInfo in Context.tblPolicies.Where(a => PolicyIDs.Contains(a.PolicyID))
                                                        select new InboxProposals
                                                        {
                                                            ProposalNo = PolicyInfo.ProposalNo,
                                                            QuoteNo = PolicyInfo.QuoteNo,
                                                            PolicyId = PolicyInfo.PolicyID,
                                                            Decision = PolicyInfo.PolicyStageStatusID.ToString(),

                                                        }).OrderByDescending(a => a.PolicyId).ToList();


                        if (ListOfSubmittedProposals != null)
                        {

                            objUWInbox.AcceptedCount = ListOfSubmittedProposals.Where(a => a.Decision == CrossCuttingConstants.PolicyStageStatusIssued.ToString()).Count();
                            objUWInbox.RejectCount = ListOfSubmittedProposals.Where(a => a.Decision == CrossCuttingConstants.PolicyStageStatusDecline.ToString()).Count();
                            objUWInbox.PostponeCount = ListOfSubmittedProposals.Where(a => a.Decision == CrossCuttingConstants.PolicyStageStatusPostPone.ToString()).Count();
                            objUWInbox.WithDrawnCount = ListOfSubmittedProposals.Where(a => a.Decision == CrossCuttingConstants.PolicyStageStatusWithDrawn.ToString()).Count();
                            objUWInbox.NotTakenCount = ListOfSubmittedProposals.Where(a => a.Decision == CrossCuttingConstants.PolicyStageStatusnotTaken.ToString()).Count();
                            objUWInbox.ReferToSRUWCount = ListOfSubmittedProposals.Where(a => a.Decision == CrossCuttingConstants.PolicyStageStatusReferToUW.ToString()).Count();
                            objUWInbox.CounterOffer = ListOfSubmittedProposals.Where(a => a.Decision == CrossCuttingConstants.PolicyStageStatusCounterOffer.ToString()).Count();
                            objUWInbox.OutStandingCount = ListOfSubmittedProposals.Where(a => a.Decision == CrossCuttingConstants.PolicyStageStatusOutStandingReq.ToString()).Count();
                            objUWInbox.SubmittedProposals = ListOfSubmittedProposals.Count();

                        }
                        // till here

                    }
                    objUWInbox.Message = "Success";
                    return objUWInbox;
                }
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                objUWInbox.Message = "Error";
                return objUWInbox;

            }
        }
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
        public AIA.Life.Models.Policy.UWInbox FetchUWProposals(AIA.Life.Models.Policy.UWInbox objUWInbox)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            try
            {
                var USerInfo = Context.AspNetUsers.Where(a => a.UserName == objUWInbox.UserName).FirstOrDefault();
                if (USerInfo != null)
                {
                    if (objUWInbox.Message == "Pending")
                    {
                        #region Pending Proposals
                        objUWInbox.LstProposals = (from objpolicy in Context.tblPolicies.Where(a => a.Createdby == USerInfo.Id)
                                                   join objproduct in Context.tblProducts
                                                   on objpolicy.ProductID equals objproduct.ProductId
                                                   join objtblpolicyrelationship in Context.tblPolicyRelationships on objpolicy.PolicyID equals objtblpolicyrelationship.PolicyID
                                                   join objtblpolicyclients in Context.tblPolicyClients on objtblpolicyrelationship.PolicyClientID equals objtblpolicyclients.PolicyClientID
                                                   join objProposalPayments in Context.tblProposalPremiums on objpolicy.PolicyID equals objProposalPayments.PolicyID
                                                   where //objpolicy.PolicyStageStatusID == CrossCuttingConstants.PolicyStageStatusDecline ||
                                                   objpolicy.PolicyStageStatusID == CrossCuttingConstants.PolicyStageStatusPending
                                                   || objpolicy.PolicyStageStatusID == CrossCuttingConstants.PolicyStageStatusCounterOffer
                                                    || objpolicy.PolicyStageStatusID == CrossCuttingConstants.PolicyStageStatusOutStandingReq
                                                   select new InboxProposals
                                                   {
                                                       ProposalNo = objpolicy.ProposalNo,
                                                       QuoteNo = objpolicy.QuoteNo,
                                                       //InsuredName = objtblpolicyclients.FirstName,
                                                       InsuredName = (objtblpolicyclients.FullName != "CORP" ? Context.tblMasCommonTypes.Where(a => a.Code == objtblpolicyclients.Title).Select(b => b.ShortDesc).FirstOrDefault()+" " + objtblpolicyclients.FirstName+" "+ objtblpolicyclients.LastName : objtblpolicyclients.CorporateName),
                                                       PlanName = objproduct.ProductName,
                                                       PolicyId = objpolicy.PolicyID,
                                                       PolicyTerm = objpolicy.PolicyTerm,
                                                       IssueDate = objpolicy.CreatedDate,
                                                       IssuedDate = objpolicy.CreatedDate.ToString(),
                                                       Premium = (objProposalPayments.AnnualPremium + objProposalPayments.AdditionalPremium),
                                                       Premiumlkr = (objProposalPayments.AnnualPremium + objProposalPayments.AdditionalPremium).ToString(),
                                                       AdditionalPremium = objProposalPayments.AdditionalPremium,
                                                   }).OrderByDescending(a => a.PolicyId).ToList();
                        for (int i = 0; i < objUWInbox.LstProposals.Count(); i++)
                        {
                            objUWInbox.LstProposals[i].Premiumlkr = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64((objUWInbox.LstProposals[i].Premiumlkr.Split('.')[0])));
                        }
                        #endregion
                    }
                    else if (objUWInbox.Message == "Processed")
                    {
                        #region Processed Proposals
                        List<decimal?> PolicyIDs = (Context.tblPolicyUWRemarkHistories.Where(a => a.CreatedBy == USerInfo.Id && a.CommonID != null
                                                  && (a.Decision == CrossCuttingConstants.UWDecisionAccepted || a.Decision == CrossCuttingConstants.UWDecisionDecline
                                                  || a.Decision == CrossCuttingConstants.UWDecisionPostPone || a.Decision == CrossCuttingConstants.UWDecisionWithDrawn || a.Decision == CrossCuttingConstants.UWDecisionReferToUW
                                                   || a.Decision == CrossCuttingConstants.UWDecisionOutStandingReq || a.Decision == CrossCuttingConstants.UWDecisionNotTaken
                                                   || a.Decision == CrossCuttingConstants.UWDecisionCounterOffer)).Select(a => a.PolicyID)).ToList();


                        objUWInbox.LstProposals = (from objpolicy in Context.tblPolicies.Where(a => PolicyIDs.Contains(a.PolicyID))
                                                   join objproduct in Context.tblProducts
                                                   on objpolicy.ProductID equals objproduct.ProductId
                                                   join objtblpolicyrelationship in Context.tblPolicyRelationships on objpolicy.PolicyID equals objtblpolicyrelationship.PolicyID
                                                   join objtblpolicyclients in Context.tblPolicyClients on objtblpolicyrelationship.PolicyClientID equals objtblpolicyclients.PolicyClientID
                                                   join objProposalPayments in Context.tblProposalPremiums on objpolicy.PolicyID equals objProposalPayments.PolicyID
                                                   select new InboxProposals
                                                   {
                                                       ProposalNo = objpolicy.ProposalNo,
                                                       QuoteNo = objpolicy.QuoteNo,
                                                       NIC = objtblpolicyclients.NEWNICNo,
                                                       InsuredName = objtblpolicyclients.LastName,
                                                       PlanName = objproduct.ProductName,
                                                       PolicyId = objpolicy.PolicyID,
                                                       PolicyTerm = objpolicy.PolicyTerm,
                                                       IssueDate = objpolicy.CreatedDate,
                                                       IssuedDate = objpolicy.CreatedDate.ToString(),
                                                       Premium = (objProposalPayments.AnnualPremium + objProposalPayments.AdditionalPremium),
                                                       Decision = objpolicy.PolicyStageStatusID.ToString(),

                                                   }).OrderByDescending(a => a.PolicyId).ToList().Select(c => new InboxProposals
                                                   {
                                                       ProposalNo = c.ProposalNo,
                                                       QuoteNo = c.QuoteNo,
                                                       NIC = c.NIC,
                                                       InsuredName = c.InsuredName,
                                                       PlanName = c.PlanName,
                                                       PolicyId = c.PolicyId,
                                                       PolicyTerm = c.PolicyTerm,
                                                       IssueDate = c.IssueDate,
                                                       Premium = c.Premium,
                                                       Decision = GetDecisionDescription(c.Decision),
                                                   }).ToList();
                        #endregion
                    }

                    else
                    {
                        #region UW Inbox 
                        objUWInbox.LstProposals = (from objpolicy in Context.tblPolicies
                                                   join objproduct in Context.tblProducts
                                                  on objpolicy.ProductID equals objproduct.ProductId

                                                   join AspnetUsers in Context.AspNetUsers on objpolicy.Createdby equals AspnetUsers.Id
                                                   join objUserdetails in Context.tblUserDetails on AspnetUsers.UserName equals objUserdetails.LoginID
                                                   join objUserChannelMap in Context.tblUserChannelMaps on objUserdetails.NodeID equals objUserChannelMap.NodeId
                                                   join objChannel in Context.tblmasChannels on objUserChannelMap.ChannelID equals objChannel.ChannelID


                                                   join objtblpolicyrelationship in Context.tblPolicyRelationships on objpolicy.PolicyID equals objtblpolicyrelationship.PolicyID
                                                   join objtblpolicyclients in Context.tblPolicyClients on objtblpolicyrelationship.PolicyClientID equals objtblpolicyclients.PolicyClientID
                                                   join objProposalPayments in Context.tblProposalPremiums on objpolicy.PolicyID equals objProposalPayments.PolicyID

                                                   join objtblpolicyMemberDetails in Context.tblPolicyMemberDetails.Where(a => a.Assuredname == "MainLife") on objpolicy.PolicyID equals objtblpolicyMemberDetails.PolicyID

                                                   join objpolicyAllocation in Context.tblPolicyUWAllocations on objpolicy.PolicyID equals objpolicyAllocation.PolicyID
                                                   where objpolicy.PolicyStageStatusID == CrossCuttingConstants.PolicyStageStatusReferToUW && objpolicyAllocation.AllocatedTo == USerInfo.Id // UW
                                                   let ISAFC = objtblpolicyMemberDetails.AFC == true ? objtblpolicyMemberDetails.AFC : false
                                                   select new InboxProposals
                                                   {
                                                       ProposalNo = objpolicy.ProposalNo,
                                                       ProductPriority = objproduct.Priority,
                                                       ChannelPriority = objChannel.Priority,
                                                       QuoteNo = objpolicy.QuoteNo,
                                                       InsuredName = objtblpolicyclients.LastName,
                                                       NIC = objtblpolicyclients.NEWNICNo,
                                                       PlanName = objproduct.ProductName,
                                                       PolicyId = objpolicy.PolicyID,
                                                       PolicyTerm = objpolicy.PolicyTerm,
                                                       IssueDate = objpolicy.CreatedDate,
                                                       IssuedDate = objpolicy.CreatedDate.ToString(),
                                                       Premium = (objProposalPayments.AnnualPremium + objProposalPayments.AdditionalPremium),
                                                       Channel = objChannel.ChannelName,
                                                       AllocatedDate = objpolicyAllocation.CreatedDate,
                                                       SARVal = objtblpolicyMemberDetails.SAR,
                                                       ISAFC = ISAFC,
                                                       NoofDays = DbFunctions.DiffDays(objpolicyAllocation.CreatedDate, DateTime.Now)
                                                   }).OrderBy(a => a.AllocatedDate).// Allocation Date
                                                   ThenBy(a => a.ChannelPriority). // Channel
                                                   ThenBy(a => a.ProductPriority). // Product
                                                   ThenByDescending(a => a.ISAFC).  // AFC
                                                   ThenBy(a => a.Premium).  // Premium
                                                   ThenBy(a => a.SARVal).  // SAR
                                                   ToList();
                        #endregion
                    }

                    foreach (var obj in objUWInbox.LstProposals)
                    {
                        obj.IssuedDate = obj.IssuedDate.ToDate().ToString("dd/MM/yyyy");
                    }

                }


            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
            }
            return objUWInbox;

        }
        // till here
        public AIA.Life.Models.Policy.Policy LoadMasters(AIA.Life.Models.Policy.Policy ObjPolicy)
        {
            Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();

            ObjPolicy.ListPlan = objCommonBusiness.ListProducts();
            ObjPolicy.lstRelations = objCommonBusiness.GetMasCommonTypeMasterListItem("HealthRelationship");
            ObjPolicy.lstRelations = objCommonBusiness.GetMasCommonTypeMasterListItem("Relationshipwiththepolicyowner");
            //ObjPolicy.MaritalStatuslist = objCommonBusiness.GetMasCommonTypeMasterListItem("MaritalStatus");
            ObjPolicy.MaritalStatuslist = objCommonBusiness.GetMaritalStatus();
            ObjPolicy.Nationalities = objCommonBusiness.GetMasCommonTypeMasterListItem("Nationality");
            //ObjPolicy.lstGender = objCommonBusiness.GetMasCommonTypeMasterListItem("Gender");
            ObjPolicy.lstGender = objCommonBusiness.GetGender();
            ObjPolicy.lstOccupation = objCommonBusiness.GetOccupation();
            ObjPolicy.lstSalutation = objCommonBusiness.GetSalutation();
            ObjPolicy.lstDocumentName = objCommonBusiness.GetMasCommonTypeMasterListItem("HealthDocumentNames");
            ObjPolicy.lstLanguage = objCommonBusiness.ListLanguage();
            ObjPolicy.lstHeight = objCommonBusiness.ListHeight();
            ObjPolicy.lstWeight = objCommonBusiness.ListWeight();
            ObjPolicy.lstCity = objCommonBusiness.ListCity();
            //Added LstMaturityBenefits
            ObjPolicy.LstMaturityBenefits = objCommonBusiness.GetMaturityBenefits();
            ObjPolicy.LstResidentialStatus = objCommonBusiness.GetResidentialStatus();
            // ObjPolicy.LstFillMemberCountryofOccupation = objCommonBusiness.GetCountryofOccupation();

            return ObjPolicy;
        }

        public AIA.Life.Models.Policy.Policy SaveProposal(AIA.Life.Models.Policy.Policy ObjPolicy)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                tblPolicy objpolicy = new tblPolicy();

                if (!string.IsNullOrEmpty(ObjPolicy.QuoteNo))
                {
                    objpolicy = Context.tblPolicies.Where(a => a.QuoteNo == ObjPolicy.QuoteNo).FirstOrDefault();
                    if (objpolicy == null)
                    {
                        objpolicy = new tblPolicy();
                    }
                }
                if (ObjPolicy.RefNo == null)
                {
                    ObjPolicy.RefNo = ObjPolicy.UserName + " - AVO";
                }
                objpolicy.RefNo = ObjPolicy.RefNo;
                tblLifeQQ objlifeQQ = new tblLifeQQ();
                objlifeQQ = Context.tblLifeQQs.Where(a => a.QuoteNo == ObjPolicy.QuoteNo).FirstOrDefault();

                objpolicy = FillPolicyInfo(ObjPolicy, objpolicy);
                if (ObjPolicy.PlanName != null)
                {
                    //objpolicy.PlanID = Context.tblProducts.Where(a => a.ProductName == ObjPolicy.PlanName).FirstOrDefault().ProductId;
                    objpolicy.PlanID = objlifeQQ.PlanId;
                }
                if (ObjPolicy.PlanName != null)
                {
                    //objpolicy.ProductID = Context.tblMasProductPlans.Where(a => a.PlanDescriprion == ObjPolicy.PlanName).FirstOrDefault().PlanId;
                    objpolicy.ProductID = objlifeQQ.ProductNameID;
                }
                if (ObjPolicy.PlanName != null)
                {
                    ObjPolicy.PlanCode = objlifeQQ.PlanCode;
                }


                tblPolicyRelationship objtblpolicyrelationship = objpolicy.tblPolicyRelationships.FirstOrDefault();
                if (objtblpolicyrelationship == null)
                {
                    objtblpolicyrelationship = new tblPolicyRelationship();
                }

                tblPolicyClient objtblPolicyClient = objtblpolicyrelationship.tblPolicyClient;
                if (objtblPolicyClient == null)
                {
                    objtblPolicyClient = new tblPolicyClient();
                }

                objtblPolicyClient = FilltblpolicyClient(ObjPolicy.objProspectDetails, objtblPolicyClient);

                #region FillCustomer Info
                tblCustomer objCustomer = objtblPolicyClient.tblCustomer;
                if (objCustomer == null)
                {
                    objCustomer = new tblCustomer();
                }
                objCustomer = FillCustomerInfo(ObjPolicy.objProspectDetails, objCustomer);
                objCustomer.tblAddress = objtblPolicyClient.tblAddress1;
                objtblPolicyClient.tblCustomer = objCustomer;
                #endregion

                objtblpolicyrelationship.tblPolicyClient = objtblPolicyClient;
                objtblpolicyrelationship.CreatedDate = DateTime.Now;
                objtblpolicyrelationship.RelationshipID = 267;//prospect

                #region  Member Details
                if (ObjPolicy.objMemberDetails != null)
                {
                    #region Identify Main Life
                    string MainLifeRelationID = string.Empty;
                    if (ObjPolicy.objProspectDetails.IsSelfCovered)
                    {
                        MainLifeRelationID = "267";
                    }
                    else
                    {
                        if (ObjPolicy.objProspectDetails.IsSpouseCoverd)
                        {
                            MainLifeRelationID = "268";
                        }
                    }
                    #endregion

                    foreach (MemberDetails objMember in ObjPolicy.objMemberDetails)
                    {

                        tblPolicyMemberDetail objMemberDetail = new tblPolicyMemberDetail(); //null
                        tblPolicyMemberDetail ExisitingobjMemberDetail = new tblPolicyMemberDetail(); //= null;//
                        if (objMember.MemberID > 0)
                        {

                            ExisitingobjMemberDetail = objpolicy.tblPolicyMemberDetails.Where(a => a.MemberID == objMember.MemberID).FirstOrDefault();
                            objMemberDetail = objpolicy.tblPolicyMemberDetails.Where(a => a.MemberID == objMember.MemberID).FirstOrDefault();
                            FillMemberDetails(Context, objMemberDetail, objMember, ObjPolicy.QuoteNo);
                        }
                        else
                        {
                            objMemberDetail = new tblPolicyMemberDetail();
                            FillMemberDetails(Context, objMemberDetail, objMember, ObjPolicy.QuoteNo);
                        }


                        if (objMember.RelationShipWithPropspect == "267" || objMember.RelationShipWithPropspect == "268")
                        {
                            #region LifeStyleDetails
                            if (objMember.objLifeStyleQuetions != null)
                            {
                                tblMemberLifeStyleDetail objMemberLifeStyleDetails = objMemberDetail.tblMemberLifeStyleDetails.FirstOrDefault();
                                if (objMemberLifeStyleDetails == null)
                                {
                                    objMemberLifeStyleDetails = new tblMemberLifeStyleDetail();
                                }
                                if (objMember.objLifeStyleQuetions.MemberLifeStyleID > 0 && objMemberDetail.MemberID > 0)
                                {
                                    //UpdateLifeStyleDetails(objMember.objLifeStyleQuetions.MemberLifeStyleID);// Added To Mark deleted for existing entry

                                    //var ExistingobjMemberLifeStyleDetails = objMemberDetail.tblMemberLifeStyleDetails.Where(a => a.MemberLifeStyleID == objMember.objLifeStyleQuetions.MemberLifeStyleID).FirstOrDefault();
                                    objMemberLifeStyleDetails.Height = Convert.ToString(objMember.objLifeStyleQuetions.Height);
                                    objMemberLifeStyleDetails.HeightFeets = Convert.ToString(objMember.objLifeStyleQuetions.HeightFeets);
                                    objMemberLifeStyleDetails.Weight = Convert.ToString(objMember.objLifeStyleQuetions.Weight);
                                    objMemberLifeStyleDetails.UnitofHeight = objMember.objLifeStyleQuetions.HeightUnit;
                                    objMemberLifeStyleDetails.UnitofWeight = objMember.objLifeStyleQuetions.WeightUnit;
                                    objMemberLifeStyleDetails.IsWeightSteady = objMember.objLifeStyleQuetions.SteadyWeight;
                                    objMemberLifeStyleDetails.IsAlcoholic = objMember.objLifeStyleQuetions.IsAlcholic;
                                    objMemberLifeStyleDetails.IsSmoker = objMember.objLifeStyleQuetions.IsSmoker;
                                    ////objMemberLifeStyleDetails.IsNarcoticDrug = objMember.objLifeStyleQuetions.IsNarcoticDrugs;

                                    //objMemberLifeStyleDetails.MemberID = ExistingobjMemberLifeStyleDetails.MemberID;
                                    //objMemberLifeStyleDetails.MemberLifeStyleID = ExistingobjMemberLifeStyleDetails.MemberLifeStyleID;


                                    foreach (var SmokeDetails in objMember.objLifeStyleQuetions.objSmokeDetails)
                                    {
                                        tblMemberAdditionalLifeStyleDetail objMemberAdditionalLifeStyleDetail = new tblMemberAdditionalLifeStyleDetail();
                                        if (SmokeDetails.AdditionalLifeStyleID > 0 && objMemberLifeStyleDetails.MemberLifeStyleID > 0)
                                        {
                                            var ExistingAdditionalLifeStyleDetail = Context.tblMemberAdditionalLifeStyleDetails.Where(a => a.AdditionalLifeStyleID == SmokeDetails.AdditionalLifeStyleID).FirstOrDefault();
                                            objMemberAdditionalLifeStyleDetail.ItemType = "Smoke";
                                            objMemberAdditionalLifeStyleDetail.MemberLifeStyleID = ExistingAdditionalLifeStyleDetail.MemberLifeStyleID;
                                            objMemberAdditionalLifeStyleDetail.Type = SmokeDetails.SmokeType;
                                            objMemberAdditionalLifeStyleDetail.Number = SmokeDetails.SmokeQuantity;
                                            objMemberAdditionalLifeStyleDetail.Per = SmokeDetails.SmokePerDay;
                                            objMemberAdditionalLifeStyleDetail.Term = SmokeDetails.SmokeDuration;
                                            objMemberAdditionalLifeStyleDetail.IsDeleted = SmokeDetails.Isdeleted;
                                            objMemberAdditionalLifeStyleDetail.AdditionalLifeStyleID = ExistingAdditionalLifeStyleDetail.AdditionalLifeStyleID;
                                            Context.Entry(ExistingAdditionalLifeStyleDetail).CurrentValues.SetValues(objMemberAdditionalLifeStyleDetail);
                                        }
                                        else
                                        {
                                            objMemberAdditionalLifeStyleDetail.ItemType = "Smoke";
                                            objMemberAdditionalLifeStyleDetail.Type = SmokeDetails.SmokeType;
                                            objMemberAdditionalLifeStyleDetail.Number = SmokeDetails.SmokeQuantity;
                                            objMemberAdditionalLifeStyleDetail.Per = SmokeDetails.SmokePerDay;
                                            objMemberAdditionalLifeStyleDetail.Term = SmokeDetails.SmokeDuration;
                                            objMemberAdditionalLifeStyleDetail.IsDeleted = SmokeDetails.Isdeleted;
                                            objMemberAdditionalLifeStyleDetail.MemberLifeStyleID = objMemberLifeStyleDetails.MemberLifeStyleID;
                                            Context.tblMemberAdditionalLifeStyleDetails.Add(objMemberAdditionalLifeStyleDetail);
                                            //  objMemberLifeStyleDetails.tblMemberAdditionalLifeStyleDetails.Add(objMemberAdditionalLifeStyleDetail);
                                        }

                                    }

                                    foreach (var AlcoholDetails in objMember.objLifeStyleQuetions.objAlcoholDetails)
                                    {
                                        tblMemberAdditionalLifeStyleDetail objMemberAdditionalLifeStyleDetail = new tblMemberAdditionalLifeStyleDetail();
                                        if (AlcoholDetails.AdditionalLifeStyleID > 0 && objMemberLifeStyleDetails.MemberLifeStyleID > 0)
                                        {
                                            var ExistingAdditionalLifeStyleDetail = Context.tblMemberAdditionalLifeStyleDetails.Where(a => a.AdditionalLifeStyleID == AlcoholDetails.AdditionalLifeStyleID).FirstOrDefault();
                                            objMemberAdditionalLifeStyleDetail.MemberLifeStyleID = ExistingAdditionalLifeStyleDetail.MemberLifeStyleID;
                                            objMemberAdditionalLifeStyleDetail.ItemType = "Alcohol";
                                            objMemberAdditionalLifeStyleDetail.Type = AlcoholDetails.AlcholType;
                                            objMemberAdditionalLifeStyleDetail.Number = AlcoholDetails.AlcholQuantity;
                                            objMemberAdditionalLifeStyleDetail.Per = AlcoholDetails.AlcholPerDay;
                                            objMemberAdditionalLifeStyleDetail.Term = AlcoholDetails.AlcholDuration;
                                            objMemberAdditionalLifeStyleDetail.IsDeleted = AlcoholDetails.Isdeleted;
                                            objMemberAdditionalLifeStyleDetail.AdditionalLifeStyleID = ExistingAdditionalLifeStyleDetail.AdditionalLifeStyleID;
                                            Context.Entry(ExistingAdditionalLifeStyleDetail).CurrentValues.SetValues(objMemberAdditionalLifeStyleDetail);
                                        }
                                        else
                                        {
                                            objMemberAdditionalLifeStyleDetail.ItemType = "Alcohol";
                                            objMemberAdditionalLifeStyleDetail.Type = AlcoholDetails.AlcholType;
                                            objMemberAdditionalLifeStyleDetail.Number = AlcoholDetails.AlcholQuantity;
                                            objMemberAdditionalLifeStyleDetail.Per = AlcoholDetails.AlcholPerDay;
                                            objMemberAdditionalLifeStyleDetail.Term = AlcoholDetails.AlcholDuration;
                                            objMemberAdditionalLifeStyleDetail.IsDeleted = AlcoholDetails.Isdeleted;
                                            objMemberAdditionalLifeStyleDetail.MemberLifeStyleID = objMemberLifeStyleDetails.MemberLifeStyleID;
                                            Context.tblMemberAdditionalLifeStyleDetails.Add(objMemberAdditionalLifeStyleDetail);
                                            // objMemberLifeStyleDetails.tblMemberAdditionalLifeStyleDetails.Add(objMemberAdditionalLifeStyleDetail);
                                        }
                                    }
                                }
                                else
                                {
                                    objMemberLifeStyleDetails.Height = Convert.ToString(objMember.objLifeStyleQuetions.Height);
                                    objMemberLifeStyleDetails.HeightFeets = Convert.ToString(objMember.objLifeStyleQuetions.HeightFeets);
                                    objMemberLifeStyleDetails.Weight = Convert.ToString(objMember.objLifeStyleQuetions.Weight);
                                    objMemberLifeStyleDetails.UnitofHeight = objMember.objLifeStyleQuetions.HeightUnit;
                                    objMemberLifeStyleDetails.UnitofWeight = objMember.objLifeStyleQuetions.WeightUnit;
                                    objMemberLifeStyleDetails.IsWeightSteady = objMember.objLifeStyleQuetions.SteadyWeight;
                                    objMemberLifeStyleDetails.IsAlcoholic = objMember.objLifeStyleQuetions.IsAlcholic;
                                    objMemberLifeStyleDetails.IsSmoker = objMember.objLifeStyleQuetions.IsSmoker;
                                    //objMemberLifeStyleDetails.IsNarcoticDrug = objMember.objLifeStyleQuetions.IsNarcoticDrugs;

                                    foreach (var SmokeDetails in objMember.objLifeStyleQuetions.objSmokeDetails.Where(a => a.Isdeleted != true).ToList())
                                    {
                                        tblMemberAdditionalLifeStyleDetail objMemberAdditionalLifeStyleDetail = new tblMemberAdditionalLifeStyleDetail();

                                        objMemberAdditionalLifeStyleDetail.ItemType = "Smoke";
                                        objMemberAdditionalLifeStyleDetail.Type = SmokeDetails.SmokeType;
                                        objMemberAdditionalLifeStyleDetail.Number = SmokeDetails.SmokeQuantity;
                                        objMemberAdditionalLifeStyleDetail.Per = SmokeDetails.SmokePerDay;
                                        objMemberAdditionalLifeStyleDetail.Term = SmokeDetails.SmokeDuration;
                                        objMemberAdditionalLifeStyleDetail.IsDeleted = false;
                                        objMemberLifeStyleDetails.tblMemberAdditionalLifeStyleDetails.Add(objMemberAdditionalLifeStyleDetail);

                                    }
                                    foreach (var AlcoholDetails in objMember.objLifeStyleQuetions.objAlcoholDetails.Where(a => a.Isdeleted != true).ToList())
                                    {
                                        tblMemberAdditionalLifeStyleDetail objMemberAdditionalLifeStyleDetail = new tblMemberAdditionalLifeStyleDetail();
                                        objMemberAdditionalLifeStyleDetail.ItemType = "Alcohol";
                                        objMemberAdditionalLifeStyleDetail.Type = AlcoholDetails.AlcholType;
                                        objMemberAdditionalLifeStyleDetail.Number = AlcoholDetails.AlcholQuantity;
                                        objMemberAdditionalLifeStyleDetail.Per = AlcoholDetails.AlcholPerDay;
                                        objMemberAdditionalLifeStyleDetail.Term = AlcoholDetails.AlcholDuration;
                                        objMemberAdditionalLifeStyleDetail.IsDeleted = false;
                                        objMemberLifeStyleDetails.tblMemberAdditionalLifeStyleDetails.Add(objMemberAdditionalLifeStyleDetail);
                                    }

                                    objMemberDetail.tblMemberLifeStyleDetails.Add(objMemberLifeStyleDetails);
                                }
                            }
                            #endregion

                            #region BMI Calucation 
                            if (objMember.objLifeStyleQuetions != null)
                            {
                                decimal _Height = decimal.Zero;
                                if (objMember.objLifeStyleQuetions.HeightFeets != null)
                                {
                                    if (objMember.objLifeStyleQuetions.HeightFeets == 2411)//Ft
                                    {
                                        _Height = (Convert.ToDecimal(objMember.objLifeStyleQuetions.Height) * (30.48m));
                                    }
                                    else if (objMember.objLifeStyleQuetions.HeightFeets == 2412)//Inches
                                    {
                                        _Height = (Convert.ToDecimal(objMember.objLifeStyleQuetions.Height) * (2.54m));

                                    }
                                    else if (objMember.objLifeStyleQuetions.HeightFeets == 2413)// CMS
                                    {
                                        _Height = Convert.ToDecimal(objMember.objLifeStyleQuetions.Height);
                                    }
                                    if (_Height > 0)
                                    {
                                        decimal Height = _Height / 100;
                                        decimal Weight = Convert.ToDecimal(objMember.objLifeStyleQuetions.Weight);
                                        objMemberDetail.BMIValue = Convert.ToString(Math.Round((Weight / (Height * Height))));
                                    }
                                    else
                                    {
                                        objMemberDetail.BMIValue = null;
                                    }
                                }

                            }
                            #endregion

                            #region Life Style Q&A
                            if (objMember.Questions != null)
                            {
                                foreach (var LifeQuestions in objMember.Questions)
                                {
                                    tblMemberQuestion objMemberQuestion = new tblMemberQuestion();
                                    if (LifeQuestions.MemberQuestionID > 0 || objMemberDetail.MemberID > 0)
                                    {
                                        tblMemberQuestion ExistingMemberQuestion = objMemberDetail.tblMemberQuestions.Where(a => a.QID == LifeQuestions.QuestionID).FirstOrDefault();
                                        objMemberQuestion.ItemType = "LifeStyle";
                                        objMemberQuestion.QID = Convert.ToInt32(LifeQuestions.QuestionID);
                                        objMemberQuestion.Answer = LifeQuestions.Answer;
                                        if (ExistingMemberQuestion != null)
                                        {
                                            objMemberQuestion.MemberID = ExistingMemberQuestion.MemberID;
                                            objMemberQuestion.MemberQuestionID = ExistingMemberQuestion.MemberQuestionID;
                                            Context.Entry(ExistingMemberQuestion).CurrentValues.SetValues(objMemberQuestion);
                                        }
                                        else
                                        {
                                            objMemberQuestion.MemberID = objMember.MemberID;
                                            objMemberDetail.tblMemberQuestions.Add(objMemberQuestion);
                                        }
                                    }
                                    else
                                    {
                                        objMemberQuestion.ItemType = "LifeStyle";
                                        objMemberQuestion.QID = Convert.ToInt32(LifeQuestions.QuestionID);
                                        objMemberQuestion.Answer = LifeQuestions.Answer;
                                        objMemberDetail.tblMemberQuestions.Add(objMemberQuestion);
                                    }
                                }
                            }
                            #endregion

                            #region Life Style EasyPension Q&A
                            if (objMember.LstEasyPensionQuestions != null)
                            {
                                foreach (var LifeEasyPensionQuestions in objMember.LstEasyPensionQuestions)
                                {
                                    tblMemberQuestion objMemberQuestion = new tblMemberQuestion();
                                    if (LifeEasyPensionQuestions.MemberQuestionID > 0 || objMemberDetail.MemberID > 0)
                                    {
                                        tblMemberQuestion ExistingMemberQuestion = objMemberDetail.tblMemberQuestions.Where(a => a.QID == LifeEasyPensionQuestions.QuestionID).FirstOrDefault();
                                        objMemberQuestion.ItemType = "LifeStyle";
                                        objMemberQuestion.QID = Convert.ToInt32(LifeEasyPensionQuestions.QuestionID);
                                        objMemberQuestion.Answer = LifeEasyPensionQuestions.Answer;
                                        if (ExistingMemberQuestion != null)
                                        {
                                            objMemberQuestion.MemberID = ExistingMemberQuestion.MemberID;
                                            objMemberQuestion.MemberQuestionID = ExistingMemberQuestion.MemberQuestionID;
                                            Context.Entry(ExistingMemberQuestion).CurrentValues.SetValues(objMemberQuestion);
                                        }
                                        else
                                        {
                                            objMemberQuestion.MemberID = objMember.MemberID;
                                            objMemberDetail.tblMemberQuestions.Add(objMemberQuestion);
                                        }
                                    }
                                    else
                                    {
                                        objMemberQuestion.ItemType = "LifeStyle";
                                        objMemberQuestion.QID = Convert.ToInt32(LifeEasyPensionQuestions.QuestionID);
                                        objMemberQuestion.Answer = LifeEasyPensionQuestions.Answer;
                                        objMemberDetail.tblMemberQuestions.Add(objMemberQuestion);
                                    }
                                }
                            }
                            #endregion

                            #region Family BackGround History  
                            if (objMember.objLstFamilyBackground != null)
                            {
                                foreach (var item in objMember.objLstFamilyBackground)
                                {
                                    tblPolicyMemberFamilyHistory objfamilyHistory = new tblPolicyMemberFamilyHistory();
                                    if (item.FamilyBackgroundId > 0 && objMemberDetail.MemberID > 0)
                                    {
                                        var ExistingobjfamilyHistory = objMemberDetail.tblPolicyMemberFamilyHistories.Where(a => a.MemberFamilyHistoryID == item.FamilyBackgroundId).FirstOrDefault();
                                        objfamilyHistory.IsDeleted = item.Isdeleted;
                                        objfamilyHistory.PresentAge = item.PresentAge;
                                        objfamilyHistory.RelationshipWithMember = item.FamilyPersonType;
                                        objfamilyHistory.StateofHealth = item.StateOfHealth;
                                        objfamilyHistory.AgeAtDeath = item.AgeAtDeath;
                                        objfamilyHistory.CauseofDeath = item.Cause;
                                        objfamilyHistory.AnyPerson = item.AnyMemberOfFamily;
                                        objfamilyHistory.Below_60_Age_Death = item.DeathBelow;
                                        objfamilyHistory.Details = item.Details;
                                        //  UpdateFamilyHistory(item, objMember.MemberID);
                                        objfamilyHistory.MemberID = ExistingobjfamilyHistory.MemberID;
                                        objfamilyHistory.MemberFamilyHistoryID = ExistingobjfamilyHistory.MemberFamilyHistoryID;
                                        Context.Entry(ExistingobjfamilyHistory).CurrentValues.SetValues(objfamilyHistory);
                                    }
                                    else
                                    {
                                        FillFamilyHistory(objfamilyHistory, item);
                                        objfamilyHistory.AnyPerson = item.AnyMemberOfFamily;
                                        objfamilyHistory.Below_60_Age_Death = item.DeathBelow;
                                        objfamilyHistory.RelationshipWithMember = item.FamilyPersonType;
                                        objfamilyHistory.Details = item.Details;
                                        objfamilyHistory.IsDeleted = item.Isdeleted;
                                        objMemberDetail.tblPolicyMemberFamilyHistories.Add(objfamilyHistory);
                                    }
                                }
                            }
                            #endregion

                        }

                        #region Set Gender Based on relationship
                        if (objMember.RelationShipWithPropspect == "269")
                        {
                            //objMemberDetails[i].Gender = Context.tblQuoteMemberDetials.Where(a => a.Relationship == "269").Select(a => a.Gender).FirstOrDefault();
                            objMember.Gender = "M";
                        }
                        else if (objMember.RelationShipWithPropspect == "270")
                        {
                            //objMemberDetails[i].Gender = Context.tblQuoteMemberDetials.Where(a => a.Relationship == "270").Select(a => a.Gender).FirstOrDefault();
                            objMember.Gender = "F";
                        }
                        #endregion

                        #region Member Medical History
                        if (objMember.objLstMedicalHistory != null)
                        {
                            foreach (var MedicalHistoryQuestion in objMember.objLstMedicalHistory)
                            {
                                tblMemberQuestion objMemberQuestion = new tblMemberQuestion();
                                if (MedicalHistoryQuestion.MemberQuestionID > 0 || objMemberDetail.MemberID > 0)
                                {
                                    tblMemberQuestion ExistingMemberQuestion = objMemberDetail.tblMemberQuestions.Where(a => a.QID == MedicalHistoryQuestion.QuestionID).FirstOrDefault();
                                    objMemberQuestion.ItemType = "MedicalHistory";
                                    objMemberQuestion.QID = Convert.ToInt32(MedicalHistoryQuestion.QuestionID);
                                    objMemberQuestion.Answer = MedicalHistoryQuestion.Answer;
                                    if (ExistingMemberQuestion != null)
                                    {
                                        objMemberQuestion.MemberID = ExistingMemberQuestion.MemberID;
                                        objMemberQuestion.MemberQuestionID = ExistingMemberQuestion.MemberQuestionID;
                                        Context.Entry(ExistingMemberQuestion).CurrentValues.SetValues(objMemberQuestion);
                                    }
                                    else
                                    {
                                        objMemberQuestion.MemberID = objMember.MemberID;
                                        objMemberDetail.tblMemberQuestions.Add(objMemberQuestion);
                                    }
                                }
                                else
                                {
                                    objMemberQuestion.ItemType = "MedicalHistory";
                                    objMemberQuestion.QID = Convert.ToInt32(MedicalHistoryQuestion.QuestionID);
                                    objMemberQuestion.Answer = MedicalHistoryQuestion.Answer;
                                    objMemberDetail.tblMemberQuestions.Add(objMemberQuestion);
                                }



                            }
                        }
                        #endregion

                        #region Medical Questionnaries Arthritis Doctors Grid View Questionnaries Details & Additional Questions Grid View
                        if (objMember.LstMedicalQuestionnariesDetails != null)
                        {
                            if (objMember.LstMedicalQuestionnariesDetails.Count > 0)
                            {
                                //if (objMember.MemberID > 0)
                                //{
                                //    UpdateQuestionDetailsInfo(Convert.ToInt32(objMember.MemberID));
                                //}
                                foreach (var item in objMember.LstMedicalQuestionnariesDetails)
                                {
                                    string MedicalQuestionID = Context.tblMasLifeQuestionnaires.Where(a => a.QId == 294).Select(a => a.QId).FirstOrDefault().ToString();

                                    //tblQuestionDetail objquestiondetails = objMemberDetail.tblQuestionDetails.Where(a => a.MemberID == objMember.MemberID).FirstOrDefault();
                                    tblQuestionDetail objquestiondetails = new tblQuestionDetail();
                                    if (objMember.MemberID > 0 && item.QuestionsId > 0)
                                    {
                                        var ExistingQuestionDetails = Context.tblQuestionDetails.Where(a => a.QuestionsId == item.QuestionsId).FirstOrDefault();
                                        objquestiondetails.QID = Convert.ToInt32(MedicalQuestionID);
                                        objquestiondetails.varcharFiled9 = item.varcharFiled9;
                                        objquestiondetails.varcharFiled10 = item.varcharFiled10;
                                        objquestiondetails.varcharFiled11 = item.varcharFiled11;
                                        objquestiondetails.DateFiled3 = item.DateFiled3;
                                        //objquestiondetails.MemberID = objMember.MemberID;
                                        objquestiondetails.IsDeleted = item.IsDeleted;
                                        if (ExistingQuestionDetails != null)
                                        {
                                            objquestiondetails.QuestionsId = ExistingQuestionDetails.QuestionsId;
                                            objquestiondetails.MemberID = ExistingQuestionDetails.MemberID;
                                            objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                            //Context.tblQuestionDetails.Add(objquestiondetails);
                                            Context.Entry(ExistingQuestionDetails).CurrentValues.SetValues(objquestiondetails);
                                        }
                                    }
                                    else
                                    {
                                        objquestiondetails = new tblQuestionDetail();
                                        objquestiondetails.QID = Convert.ToInt32(MedicalQuestionID);
                                        objquestiondetails.varcharFiled9 = item.varcharFiled9;
                                        objquestiondetails.varcharFiled10 = item.varcharFiled10;
                                        objquestiondetails.varcharFiled11 = item.varcharFiled11;
                                        objquestiondetails.DateFiled3 = item.DateFiled3;
                                        objquestiondetails.MemberID = objMember.MemberID;
                                        objquestiondetails.IsDeleted = false;
                                        objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                        Context.tblQuestionDetails.Add(objquestiondetails);
                                        //Context.tblQuestionDetails.Add(objquestiondetails);
                                    }

                                    //if (objquestiondetails.QuestionsId == 0)
                                    //{
                                    //    objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                    //    Context.tblQuestionDetails.Add(objquestiondetails);
                                    //}
                                }
                            }
                        }
                        #endregion

                        #region LstMedicalDoctorsQuestionnariesDetails
                        if (objMember.LstMedicalDoctorsQuestionnariesDetails != null)
                        {
                            if (objMember.LstMedicalDoctorsQuestionnariesDetails.Count > 0)
                            {
                                //if (objMember.MemberID > 0)
                                //{
                                //    UpdateQuestionDetailsInfo(Convert.ToInt32(objMember.MemberID));
                                //}
                                foreach (var item in objMember.LstMedicalDoctorsQuestionnariesDetails)
                                {
                                    string MedicalQuestionID = Context.tblMasLifeQuestionnaires.Where(a => a.QId == 299).Select(a => a.QId).FirstOrDefault().ToString();
                                    tblQuestionDetail objquestiondetails = new tblQuestionDetail();
                                    //tblQuestionDetail objquestiondetails = objMemberDetail.tblQuestionDetails.Where(a => a.QuestionsId == item.QuestionsId).FirstOrDefault();
                                    if (objMember.MemberID > 0 && item.QuestionsId > 0)
                                    {
                                        var ExistingQuestionDetails = Context.tblQuestionDetails.Where(a => a.QuestionsId == item.QuestionsId).FirstOrDefault();
                                        objquestiondetails.QID = Convert.ToInt32(MedicalQuestionID);
                                        // objquestiondetails.QID = ExistingQuestionDetails.QID;
                                        objquestiondetails.varcharFiled1 = item.varcharFiled1;
                                        objquestiondetails.varcharFiled2 = item.varcharFiled2;
                                        objquestiondetails.DateFiled1 = item.DateFiled1;
                                        objquestiondetails.IsDeleted = item.IsDeleted;
                                        if (ExistingQuestionDetails != null)
                                        {
                                            objquestiondetails.QuestionsId = ExistingQuestionDetails.QuestionsId;
                                            objquestiondetails.MemberID = ExistingQuestionDetails.MemberID;
                                            objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                            //Context.tblQuestionDetails.Add(objquestiondetails);
                                            Context.Entry(ExistingQuestionDetails).CurrentValues.SetValues(objquestiondetails);
                                        }
                                    }
                                    else
                                    {
                                        objquestiondetails = new tblQuestionDetail();
                                        objquestiondetails.QID = Convert.ToInt32(MedicalQuestionID);
                                        objquestiondetails.MemberID = objMember.MemberID;
                                        objquestiondetails.varcharFiled1 = item.varcharFiled1;
                                        objquestiondetails.varcharFiled2 = item.varcharFiled2;
                                        objquestiondetails.DateFiled1 = item.DateFiled1;
                                        objquestiondetails.IsDeleted = false;
                                        objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                        Context.tblQuestionDetails.Add(objquestiondetails);
                                        // Context.tblQuestionDetails.Add(objquestiondetails);
                                    }
                                    //if (objquestiondetails.QuestionsId == 0)
                                    //{
                                    //    objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                    //    Context.tblQuestionDetails.Add(objquestiondetails);
                                    //}
                                }
                            }
                        }
                        #endregion

                        #region Arthritis Test Grid
                        if (objMember.LstMedicalTestQuestionnariesDetails != null)
                        {
                            if (objMember.LstMedicalTestQuestionnariesDetails.Count > 0)
                            {
                                //if (objMember.MemberID > 0)
                                //{
                                //    UpdateQuestionDetailsInfo(Convert.ToInt32(objMember.MemberID));
                                //}
                                foreach (var item in objMember.LstMedicalTestQuestionnariesDetails)
                                {
                                    string MedicalQuestionID = Context.tblMasLifeQuestionnaires.Where(a => a.QId == 296).Select(a => a.QId).FirstOrDefault().ToString();
                                    tblQuestionDetail objquestiondetails = new tblQuestionDetail();
                                    //tblQuestionDetail objquestiondetails = objMemberDetail.tblQuestionDetails.Where(a => a.MemberID == objMember.MemberID).FirstOrDefault();
                                    if (objMember.MemberID > 0 && item.QuestionsId > 0)
                                    {
                                        var ExistingQuestionDetails = Context.tblQuestionDetails.Where(a => a.QuestionsId == item.QuestionsId && a.MemberID == objMember.MemberID).FirstOrDefault();
                                        objquestiondetails.QID = Convert.ToInt32(MedicalQuestionID);
                                        //objquestiondetails.QID = ExistingQuestionDetails.QID;
                                        objquestiondetails.varcharFiled3 = item.varcharFiled3;
                                        objquestiondetails.varcharFiled4 = item.varcharFiled4;
                                        objquestiondetails.varcharFiled5 = item.varcharFiled5;
                                        objquestiondetails.DateFiled2 = item.DateFiled2;
                                        objquestiondetails.IsDeleted = item.IsDeleted;

                                        if (ExistingQuestionDetails != null)
                                        {
                                            Context.tblQuestionDetails.Remove(ExistingQuestionDetails);
                                            //objquestiondetails.QuestionsId = ExistingQuestionDetails.QuestionsId;
                                            //objquestiondetails.MemberID = ExistingQuestionDetails.MemberID;
                                            //objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                            //Context.Entry(ExistingQuestionDetails).CurrentValues.SetValues(objquestiondetails);
                                        }
                                        objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                        Context.tblQuestionDetails.Add(objquestiondetails);

                                    }
                                    else
                                    {
                                        objquestiondetails = new tblQuestionDetail();
                                        objquestiondetails.QID = Convert.ToInt32(MedicalQuestionID);
                                        objquestiondetails.MemberID = objMember.MemberID;
                                        objquestiondetails.varcharFiled3 = item.varcharFiled3;
                                        objquestiondetails.varcharFiled4 = item.varcharFiled4;
                                        objquestiondetails.varcharFiled5 = item.varcharFiled5;
                                        objquestiondetails.DateFiled2 = item.DateFiled2;
                                        objquestiondetails.IsDeleted = false;
                                        objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                        Context.tblQuestionDetails.Add(objquestiondetails);
                                        //Context.tblQuestionDetails.Add(objquestiondetails);
                                    }
                                    //if (objquestiondetails.QuestionsId == 0)
                                    //{
                                    //    objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                    //    Context.tblQuestionDetails.Add(objquestiondetails);
                                    //}
                                }
                            }
                        }
                        #endregion

                        #region Arthritis Current Test Grid
                        if (objMember.LstMedicalCurrentQuestionnariesDetails != null)
                        {
                            if (objMember.LstMedicalCurrentQuestionnariesDetails.Count > 0)
                            {
                                //if (objMember.MemberID > 0)
                                //{
                                //    UpdateQuestionDetailsInfo(Convert.ToInt32(objMember.MemberID));
                                //}
                                foreach (var item in objMember.LstMedicalCurrentQuestionnariesDetails)
                                {
                                    string MedicalQuestionID = Context.tblMasLifeQuestionnaires.Where(a => a.QId == 292).Select(a => a.QId).FirstOrDefault().ToString();
                                    tblQuestionDetail objquestiondetails = new tblQuestionDetail();
                                    //tblQuestionDetail objquestiondetails = objMemberDetail.tblQuestionDetails.Where(a=>a.MemberID == objMember.MemberID).FirstOrDefault();
                                    if (objMember.MemberID > 0 && item.QuestionsId > 0)
                                    {
                                        var ExistingQuestionDetails = Context.tblQuestionDetails.Where(a => a.QuestionsId == item.QuestionsId).FirstOrDefault();
                                        objquestiondetails.QID = Convert.ToInt32(MedicalQuestionID);
                                        //objquestiondetails.QID = ExistingQuestionDetails.QID;
                                        objquestiondetails.varcharFiled6 = item.varcharFiled6;
                                        objquestiondetails.varcharFiled7 = item.varcharFiled7;
                                        objquestiondetails.varcharFiled8 = item.varcharFiled8;
                                        objquestiondetails.IsDeleted = item.IsDeleted;
                                        if (ExistingQuestionDetails != null)
                                        {
                                            objquestiondetails.QuestionsId = ExistingQuestionDetails.QuestionsId;
                                            objquestiondetails.MemberID = ExistingQuestionDetails.MemberID;
                                            objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                            //Context.tblQuestionDetails.Add(objquestiondetails);
                                            Context.Entry(ExistingQuestionDetails).CurrentValues.SetValues(objquestiondetails);

                                        }
                                    }
                                    else
                                    {
                                        objquestiondetails = new tblQuestionDetail();
                                        objquestiondetails.QID = Convert.ToInt32(MedicalQuestionID);
                                        objquestiondetails.MemberID = objMember.MemberID;
                                        objquestiondetails.varcharFiled6 = item.varcharFiled6;
                                        objquestiondetails.varcharFiled7 = item.varcharFiled7;
                                        objquestiondetails.varcharFiled8 = item.varcharFiled8;
                                        objquestiondetails.IsDeleted = false;
                                        objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                        Context.tblQuestionDetails.Add(objquestiondetails);
                                        //Context.tblQuestionDetails.Add(objquestiondetails);
                                    }
                                    //if (objquestiondetails.QuestionsId == 0)
                                    //{
                                    //    objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                    //    Context.tblQuestionDetails.Add(objquestiondetails);
                                    //}
                                }
                            }
                        }
                        #endregion

                        #region PAQ Grid View 1
                        if (objMember.LstConcurrentlyProposedInsurancePAQ1Details != null)
                        {
                            if (objMember.LstConcurrentlyProposedInsurancePAQ1Details.Count > 0)
                            {
                                //if (objMember.MemberID > 0)
                                //{
                                //    UpdateQuestionDetailsInfo(Convert.ToInt32(objMember.MemberID));
                                //}
                                foreach (var item in objMember.LstConcurrentlyProposedInsurancePAQ1Details)
                                {
                                    string MedicalQuestionID = Context.tblMasLifeQuestionnaires.Where(a => a.QId == 1178).Select(a => a.QId).FirstOrDefault().ToString();
                                    tblQuestionDetail objquestiondetails = new tblQuestionDetail();
                                    //tblQuestionDetail objquestiondetails = objMemberDetail.tblQuestionDetails.Where(a=>a.MemberID == objMember.MemberID).FirstOrDefault();
                                    if (objMember.MemberID > 0 && item.QuestionsId > 0)
                                    {
                                        var ExistingQuestionDetails = Context.tblQuestionDetails.Where(a => a.QuestionsId == item.QuestionsId).FirstOrDefault();
                                        objquestiondetails.QID = Convert.ToInt32(MedicalQuestionID);
                                        //objquestiondetails.QID = ExistingQuestionDetails.QID;
                                        objquestiondetails.PAQvarcharFiled1 = item.PAQvarcharFiled1;
                                        objquestiondetails.PAQvarcharFiled2 = item.PAQvarcharFiled2;
                                        objquestiondetails.PAQvarcharFiled3 = item.PAQvarcharFiled3;
                                        objquestiondetails.PAQvarcharFiled4 = item.PAQvarcharFiled4;
                                        objquestiondetails.IsDeleted = item.IsDeleted;
                                        if (ExistingQuestionDetails != null)
                                        {
                                            objquestiondetails.QuestionsId = ExistingQuestionDetails.QuestionsId;
                                            objquestiondetails.MemberID = ExistingQuestionDetails.MemberID;
                                            objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                            //Context.tblQuestionDetails.Add(objquestiondetails);
                                            Context.Entry(ExistingQuestionDetails).CurrentValues.SetValues(objquestiondetails);

                                        }
                                    }
                                    else
                                    {
                                        objquestiondetails = new tblQuestionDetail();
                                        objquestiondetails.QID = Convert.ToInt32(MedicalQuestionID);
                                        objquestiondetails.MemberID = objMember.MemberID;
                                        objquestiondetails.PAQvarcharFiled1 = item.PAQvarcharFiled1;
                                        objquestiondetails.PAQvarcharFiled2 = item.PAQvarcharFiled2;
                                        objquestiondetails.PAQvarcharFiled3 = item.PAQvarcharFiled3;
                                        objquestiondetails.PAQvarcharFiled4 = item.PAQvarcharFiled4;
                                        objquestiondetails.IsDeleted = item.IsDeleted;
                                        objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                        Context.tblQuestionDetails.Add(objquestiondetails);
                                        //Context.tblQuestionDetails.Add(objquestiondetails);
                                    }
                                    //if (objquestiondetails.QuestionsId == 0)
                                    //{
                                    //    objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                    //    Context.tblQuestionDetails.Add(objquestiondetails);
                                    //}
                                }
                            }
                        }
                        #endregion

                        #region PAQ Grid View 2
                        if (objMember.LstExistingPolicieswithAIAlnsurancePAQ2Details != null)
                        {
                            if (objMember.LstExistingPolicieswithAIAlnsurancePAQ2Details.Count > 0)
                            {
                                //if (objMember.MemberID > 0)
                                //{
                                //    UpdateQuestionDetailsInfo(Convert.ToInt32(objMember.MemberID));
                                //}
                                foreach (var item in objMember.LstExistingPolicieswithAIAlnsurancePAQ2Details)
                                {
                                    string MedicalQuestionID = Context.tblMasLifeQuestionnaires.Where(a => a.QId == 1180).Select(a => a.QId).FirstOrDefault().ToString();
                                    tblQuestionDetail objquestiondetails = new tblQuestionDetail();
                                    //tblQuestionDetail objquestiondetails = objMemberDetail.tblQuestionDetails.Where(a=>a.MemberID == objMember.MemberID).FirstOrDefault();
                                    if (objMember.MemberID > 0 && item.QuestionsId > 0)
                                    {
                                        var ExistingQuestionDetails = Context.tblQuestionDetails.Where(a => a.QuestionsId == item.QuestionsId).FirstOrDefault();
                                        objquestiondetails.QID = Convert.ToInt32(MedicalQuestionID);
                                        //objquestiondetails.QID = ExistingQuestionDetails.QID;
                                        objquestiondetails.PAQvarcharFiled5 = item.PAQvarcharFiled5;
                                        objquestiondetails.PAQvarcharFiled6 = item.PAQvarcharFiled6;
                                        objquestiondetails.PAQvarcharFiled7 = item.PAQvarcharFiled7;
                                        objquestiondetails.PAQvarcharFiled8 = item.PAQvarcharFiled8;
                                        objquestiondetails.IsDeleted = item.IsDeleted;
                                        if (ExistingQuestionDetails != null)
                                        {
                                            objquestiondetails.QuestionsId = ExistingQuestionDetails.QuestionsId;
                                            objquestiondetails.MemberID = ExistingQuestionDetails.MemberID;
                                            objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                            //Context.tblQuestionDetails.Add(objquestiondetails);
                                            Context.Entry(ExistingQuestionDetails).CurrentValues.SetValues(objquestiondetails);
                                        }
                                    }
                                    else
                                    {
                                        objquestiondetails = new tblQuestionDetail();
                                        objquestiondetails.QID = Convert.ToInt32(MedicalQuestionID);
                                        objquestiondetails.MemberID = objMember.MemberID;
                                        objquestiondetails.PAQvarcharFiled5 = item.PAQvarcharFiled5;
                                        objquestiondetails.PAQvarcharFiled6 = item.PAQvarcharFiled6;
                                        objquestiondetails.PAQvarcharFiled7 = item.PAQvarcharFiled7;
                                        objquestiondetails.PAQvarcharFiled8 = item.PAQvarcharFiled8;
                                        objquestiondetails.IsDeleted = item.IsDeleted;
                                        objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                        Context.tblQuestionDetails.Add(objquestiondetails);
                                        //Context.tblQuestionDetails.Add(objquestiondetails);
                                    }
                                    //if (objquestiondetails.QuestionsId == 0)
                                    //{
                                    //    objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                    //    Context.tblQuestionDetails.Add(objquestiondetails);
                                    //}
                                }
                            }
                        }
                        #endregion

                        #region PAQ Grid View 3
                        if (objMember.ObjTotalAnnualIncomePAQ3Details != null)
                        {
                            //if (objMember.LstTotalAnnualIncomePAQ3Details.Count > 0)
                            //{
                            //if (objMember.MemberID > 0)
                            //{
                            //    UpdateQuestionDetailsInfo(Convert.ToInt32(objMember.MemberID));
                            //}
                            //foreach (var item in objMember.LstTotalAnnualIncomePAQ3Details)
                            //{
                            string MedicalQuestionID = Context.tblMasLifeQuestionnaires.Where(a => a.QId == 1181).Select(a => a.QId).FirstOrDefault().ToString();
                            tblQuestionDetail objquestiondetails = new tblQuestionDetail();
                            //tblQuestionDetail objquestiondetails = objMemberDetail.tblQuestionDetails.Where(a=>a.MemberID == objMember.MemberID).FirstOrDefault();
                            if (objMember.MemberID > 0 && objMember.ObjTotalAnnualIncomePAQ3Details.QuestionsId > 0)
                            {
                                var ExistingQuestionDetails = Context.tblQuestionDetails.Where(a => a.QuestionsId == objMember.ObjTotalAnnualIncomePAQ3Details.QuestionsId).FirstOrDefault();
                                objquestiondetails.QID = Convert.ToInt32(MedicalQuestionID);
                                //objquestiondetails.QID = ExistingQuestionDetails.QID;
                                objquestiondetails.PAQYearFiled1 = objMember.ObjTotalAnnualIncomePAQ3Details.PAQYearFiled1;
                                objquestiondetails.PAQYearFiled2 = objMember.ObjTotalAnnualIncomePAQ3Details.PAQYearFiled2;
                                objquestiondetails.PAQYearFiled3 = objMember.ObjTotalAnnualIncomePAQ3Details.PAQYearFiled3;
                                objquestiondetails.IsDeleted = false;
                                if (ExistingQuestionDetails != null)
                                {
                                    objquestiondetails.QuestionsId = ExistingQuestionDetails.QuestionsId;
                                    objquestiondetails.MemberID = ExistingQuestionDetails.MemberID;
                                    objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                    //Context.tblQuestionDetails.Add(objquestiondetails);
                                    Context.Entry(ExistingQuestionDetails).CurrentValues.SetValues(objquestiondetails);
                                }
                            }
                            else
                            {
                                objquestiondetails = new tblQuestionDetail();
                                objquestiondetails.QID = Convert.ToInt32(MedicalQuestionID);
                                objquestiondetails.MemberID = objMember.MemberID;
                                objquestiondetails.PAQYearFiled1 = objMember.ObjTotalAnnualIncomePAQ3Details.PAQYearFiled1;
                                objquestiondetails.PAQYearFiled2 = objMember.ObjTotalAnnualIncomePAQ3Details.PAQYearFiled2;
                                objquestiondetails.PAQYearFiled3 = objMember.ObjTotalAnnualIncomePAQ3Details.PAQYearFiled3;
                                objquestiondetails.IsDeleted = false;
                                objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                Context.tblQuestionDetails.Add(objquestiondetails);
                                //Context.tblQuestionDetails.Add(objquestiondetails);
                            }
                            //if (objquestiondetails.QuestionsId == 0)
                            //{
                            //    objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                            //    Context.tblQuestionDetails.Add(objquestiondetails);
                            //}
                            //}
                            //}
                        }
                        #endregion

                        #region PAQ Grid View 4
                        if (objMember.ObjAssetsandLiabilitiesPAQ4Details != null)
                        {

                            string MedicalQuestionID = Context.tblMasLifeQuestionnaires.Where(a => a.QId == 1184).Select(a => a.QId).FirstOrDefault().ToString();
                            tblQuestionDetail objquestiondetails = new tblQuestionDetail();
                            //tblQuestionDetail objquestiondetails = objMemberDetail.tblQuestionDetails.Where(a=>a.MemberID == objMember.MemberID).FirstOrDefault();
                            if (objMember.MemberID > 0 && objMember.ObjAssetsandLiabilitiesPAQ4Details.QuestionsId > 0)
                            {
                                var ExistingQuestionDetails = Context.tblQuestionDetails.Where(a => a.QuestionsId == objMember.ObjAssetsandLiabilitiesPAQ4Details.QuestionsId).FirstOrDefault();
                                objquestiondetails.QID = Convert.ToInt32(MedicalQuestionID);
                                //objquestiondetails.QID = ExistingQuestionDetails.QID;


                                objquestiondetails.PAQAssetsProperty = objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQAssetsProperty;
                                objquestiondetails.PAQAssetsInvestment = objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQAssetsInvestment;
                                objquestiondetails.PAQAssetsEquities = objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQAssetsEquities;
                                objquestiondetails.PAQAssetsOther = objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQAssetsOther;

                                objquestiondetails.PAQLiabilitiesLoans = objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQLiabilitiesLoans;
                                objquestiondetails.PAQLiabilitiesMortgages = objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQLiabilitiesMortgages;
                                objquestiondetails.PAQLiabilitiesOthers = objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQLiabilitiesOthers;

                                objquestiondetails.PAQAssetsTotal = objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQAssetsTotal;
                                objquestiondetails.PAQLiabilitiesTotal = objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQLiabilitiesTotal;
                                objquestiondetails.IsDeleted = false;
                                if (ExistingQuestionDetails != null)
                                {
                                    objquestiondetails.QuestionsId = ExistingQuestionDetails.QuestionsId;
                                    objquestiondetails.MemberID = ExistingQuestionDetails.MemberID;
                                    objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                    //Context.tblQuestionDetails.Add(objquestiondetails);
                                    Context.Entry(ExistingQuestionDetails).CurrentValues.SetValues(objquestiondetails);
                                }
                            }
                            else
                            {
                                objquestiondetails = new tblQuestionDetail();
                                objquestiondetails.QID = Convert.ToInt32(MedicalQuestionID);
                                objquestiondetails.MemberID = objMember.MemberID;

                                objquestiondetails.PAQAssetsProperty = objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQAssetsProperty;
                                objquestiondetails.PAQAssetsInvestment = objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQAssetsInvestment;
                                objquestiondetails.PAQAssetsEquities = objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQAssetsEquities;
                                objquestiondetails.PAQAssetsOther = objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQAssetsOther;

                                objquestiondetails.PAQLiabilitiesLoans = objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQLiabilitiesLoans;
                                objquestiondetails.PAQLiabilitiesMortgages = objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQLiabilitiesMortgages;
                                objquestiondetails.PAQLiabilitiesOthers = objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQLiabilitiesOthers;

                                objquestiondetails.PAQAssetsTotal = objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQAssetsTotal;
                                objquestiondetails.PAQLiabilitiesTotal = objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQLiabilitiesTotal;
                                objquestiondetails.IsDeleted = false;
                                objquestiondetails.IsDeleted = false;
                                objquestiondetails.tblPolicyMemberDetail = objMemberDetail;
                                Context.tblQuestionDetails.Add(objquestiondetails);
                                //Context.tblQuestionDetails.Add(objquestiondetails);
                            }


                        }
                        #endregion

                        #region Family BackGround History Q&A
                        if (objMember.objLstFamily != null)
                        {
                            foreach (var FamilyBackGroundQuestion in objMember.objLstFamily)
                            {
                                tblMemberQuestion objMemberQuestion = new tblMemberQuestion();
                                if (FamilyBackGroundQuestion.MemberQuestionID > 0 || objMemberDetail.MemberID > 0)
                                {
                                    tblMemberQuestion ExistingMemberQuestion = objMemberDetail.tblMemberQuestions.Where(a => a.QID == FamilyBackGroundQuestion.QuestionID).FirstOrDefault();
                                    objMemberQuestion.ItemType = "FamilyBackGround";
                                    objMemberQuestion.QID = Convert.ToInt32(FamilyBackGroundQuestion.QuestionID);
                                    objMemberQuestion.Answer = FamilyBackGroundQuestion.Answer;
                                    if (ExistingMemberQuestion != null)
                                    {
                                        objMemberQuestion.MemberID = ExistingMemberQuestion.MemberID;
                                        objMemberQuestion.MemberQuestionID = ExistingMemberQuestion.MemberQuestionID;
                                        Context.Entry(ExistingMemberQuestion).CurrentValues.SetValues(objMemberQuestion);
                                    }
                                    else
                                    {
                                        objMemberQuestion.MemberID = objMember.MemberID;
                                        objMemberDetail.tblMemberQuestions.Add(objMemberQuestion);
                                    }
                                }
                                else
                                {
                                    objMemberQuestion.ItemType = "FamilyBackGround";
                                    objMemberQuestion.QID = Convert.ToInt32(FamilyBackGroundQuestion.QuestionID);
                                    objMemberQuestion.Answer = FamilyBackGroundQuestion.Answer;
                                    objMemberDetail.tblMemberQuestions.Add(objMemberQuestion);
                                }
                            }
                        }
                        #endregion

                        #region Additional Q&A
                        if (objMember.objAdditionalQuestions != null)
                        {
                            //if (objMember.RelationShipWithPropspect == "267")
                            //{
                            //    if (!string.IsNullOrEmpty(ObjPolicy.MainLifeAdditionalQuestion))
                            //    {
                            //        var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<QuestionsList>>(ObjPolicy.MainLifeAdditionalQuestion);
                            //        // Newtonsoft.Json.JsonSerializerSettings setting = new Newtonsoft.Json.JsonSerializerSettings();
                            //        objMember.objAdditionalQuestions =Newtonsoft.Json.JsonConvert.DeserializeObject<List<QuestionsList>>(ObjPolicy.MainLifeAdditionalQuestion).ToList();
                            //    }
                            //}
                            //if (objMember.RelationShipWithPropspect == "268")
                            //{
                            //    if (!string.IsNullOrEmpty(ObjPolicy.SpouseAdditionalQuestion))
                            //    {
                            //        //Newtonsoft.Json.JsonSerializerSettings setting = new Newtonsoft.Json.JsonSerializerSettings();
                            //        objMember.objAdditionalQuestions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<QuestionsList>>(ObjPolicy.SpouseAdditionalQuestion).ToList();

                            //    }

                            //}
                            // For PAQ Questionaires
                            foreach (var AdditionalQuestion in objMember.objAdditionalQuestions)
                            {
                                tblMemberQuestion objMemberQuestion = new tblMemberQuestion();
                                if (AdditionalQuestion.MemberQuestionID > 0 || objMemberDetail.MemberID > 0)
                                {
                                    tblMemberQuestion ExistingMemberQuestion = objMemberDetail.tblMemberQuestions.Where(a => a.QID == AdditionalQuestion.QuestionID).FirstOrDefault();
                                    objMemberQuestion.ItemType = "AdditionalQuestions";
                                    objMemberQuestion.QID = Convert.ToInt32(AdditionalQuestion.QuestionID);
                                    objMemberQuestion.Answer = AdditionalQuestion.Answer;
                                    if (ExistingMemberQuestion != null)
                                    {
                                        objMemberQuestion.MemberID = ExistingMemberQuestion.MemberID;
                                        objMemberQuestion.MemberQuestionID = ExistingMemberQuestion.MemberQuestionID;
                                        Context.Entry(ExistingMemberQuestion).CurrentValues.SetValues(objMemberQuestion);
                                    }
                                    else
                                    {
                                        objMemberQuestion.MemberID = objMember.MemberID;
                                        objMemberDetail.tblMemberQuestions.Add(objMemberQuestion);
                                    }
                                }
                                else
                                {
                                    objMemberQuestion.ItemType = "AdditionalQuestions";
                                    objMemberQuestion.QID = Convert.ToInt32(AdditionalQuestion.QuestionID);
                                    objMemberQuestion.Answer = AdditionalQuestion.Answer;
                                    objMemberDetail.tblMemberQuestions.Add(objMemberQuestion);
                                }
                            }
                        }
                        #endregion

                        //objMemberDetails[i].OccupationID = Context.tblQuoteMemberDetials.Where(a => a.LifeQQID == tbllifeQQ.LifeQQID && a.IsDeleted != true).Select(a => a.OccupationID).FirstOrDefault();


                        if (objMember.RelationShipWithPropspect == "267")
                        {
                            #region  WealthPlannerQuestions
                            // objMember.objLstWealthPlannerQuestions = GetMasQuestions(Context, "WealthPlannerQuestions", null, null, null, null, Member.MemberID);
                            if (objMember.objLstWealthPlannerQuestions != null)
                            {
                                foreach (var LstWealthPlannerQuestions in objMember.objLstWealthPlannerQuestions)
                                {

                                    tblMemberQuestion objMemberQuestion = new tblMemberQuestion();
                                    if (LstWealthPlannerQuestions.MemberQuestionID > 0 || objMemberDetail.MemberID > 0)
                                    {
                                        if (ObjPolicy.objMemberDetails[0].RelationShipWithPropspect == "267")
                                        {
                                            tblMemberQuestion ExistingMemberQuestion = objMemberDetail.tblMemberQuestions.Where(a => a.QID == LstWealthPlannerQuestions.QuestionID).FirstOrDefault();
                                            objMemberQuestion.ItemType = "WealthPlannerQuestions";
                                            objMemberQuestion.QID = Convert.ToInt32(LstWealthPlannerQuestions.QuestionID);
                                            objMemberQuestion.Answer = LstWealthPlannerQuestions.Answer;
                                            if (ExistingMemberQuestion != null)
                                            {
                                                objMemberQuestion.MemberID = ExistingMemberQuestion.MemberID;
                                                objMemberQuestion.MemberQuestionID = ExistingMemberQuestion.MemberQuestionID;
                                                Context.Entry(ExistingMemberQuestion).CurrentValues.SetValues(objMemberQuestion);
                                            }
                                            else
                                            {
                                                objMemberQuestion.MemberID = objMember.MemberID;
                                                objMemberDetail.tblMemberQuestions.Add(objMemberQuestion);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (ObjPolicy.objMemberDetails[0].RelationShipWithPropspect == "267")
                                        {
                                            objMemberQuestion.ItemType = "WealthPlannerQuestions";
                                            objMemberQuestion.QID = Convert.ToInt32(LstWealthPlannerQuestions.QuestionID);
                                            objMemberQuestion.Answer = LstWealthPlannerQuestions.Answer;
                                            objMemberDetail.tblMemberQuestions.Add(objMemberQuestion);
                                        }
                                    }

                                }
                            }
                            #endregion
                        }

                        #region Previous & Other Insurance Questions
                        if (objMember.objLstOtherInsuranceDetails != null)
                        {
                            foreach (var OtherInsuranceQuestion in objMember.objLstOtherInsuranceDetails)
                            {
                                tblMemberQuestion objMemberQuestion = new tblMemberQuestion();
                                if (OtherInsuranceQuestion.MemberQuestionID > 0 || objMemberDetail.MemberID > 0)
                                {
                                    tblMemberQuestion ExistingMemberQuestion = objMemberDetail.tblMemberQuestions.Where(a => a.QID == OtherInsuranceQuestion.QuestionID).FirstOrDefault();
                                    objMemberQuestion.ItemType = "PreviousAndCurrentLifeInsurance";
                                    objMemberQuestion.QID = Convert.ToInt32(OtherInsuranceQuestion.QuestionID);
                                    objMemberQuestion.Answer = OtherInsuranceQuestion.Answer;
                                    if (ExistingMemberQuestion != null)
                                    {
                                        objMemberQuestion.MemberID = ExistingMemberQuestion.MemberID;
                                        objMemberQuestion.MemberQuestionID = ExistingMemberQuestion.MemberQuestionID;
                                        Context.Entry(ExistingMemberQuestion).CurrentValues.SetValues(objMemberQuestion);
                                    }
                                    else
                                    {
                                        objMemberQuestion.MemberID = objMember.MemberID;
                                        objMemberDetail.tblMemberQuestions.Add(objMemberQuestion);
                                    }
                                }
                                else
                                {
                                    objMemberQuestion.ItemType = "PreviousAndCurrentLifeInsurance";
                                    objMemberQuestion.QID = Convert.ToInt32(OtherInsuranceQuestion.QuestionID);
                                    objMemberQuestion.Answer = OtherInsuranceQuestion.Answer;
                                    objMemberDetail.tblMemberQuestions.Add(objMemberQuestion);
                                }
                            }
                        }
                        #endregion

                        #region OtherInsurance Info
                        int Index = ObjPolicy.objMemberDetails.FindIndex(a => a.AssuredName == ObjPolicy.AssuredName);
                        ObjPolicy.AssuredIndex = Index;
                        if (objMember.objLifeAssuredOtherInsurance != null)
                        {
                            if (objMember.objLifeAssuredOtherInsurance.Count > 0)
                            {
                                if (objMember.MemberID > 0)
                                {
                                    UpdatePreviousInsuranceInfo(Convert.ToInt32(objMember.MemberID));
                                }
                                foreach (var item in objMember.objLifeAssuredOtherInsurance.Where(a => a.IsDeleted != true))
                                {
                                    tblPolicyMemberInsuranceInfo objOtherInsuInfo = new tblPolicyMemberInsuranceInfo();
                                    if (objMemberDetail.MemberID > 0 && item.OtherInsuranceId > 0)
                                    {
                                        // objOtherInsuInfo= objMemberDetail.tblPolicyMemberInsuranceInfoes.Where(a => a.MemberInsuranceID == item.OtherInsuranceId).FirstOrDefault();
                                        var ExistingobjOtherInsuInfo = objMemberDetail.tblPolicyMemberInsuranceInfoes.Where(a => a.MemberInsuranceID == item.OtherInsuranceId).FirstOrDefault();
                                        objOtherInsuInfo.AccidentalBenifit = item.AccidentalBenefitAmount;
                                        objOtherInsuInfo.CompanyName = item.CompanyName;
                                        objOtherInsuInfo.CriticalIllnessBenifit = item.CriticalIllnessBenefit;
                                        //objOtherInsuInfo.HospitalizationReimbursement = item.TotalPermanentDisability; 
                                        objOtherInsuInfo.HospitalizationPerDay = item.HospitalizationPerDay;
                                        objOtherInsuInfo.HospitalizationReimbursement = item.HospitalizationReimbursement;
                                        objOtherInsuInfo.Policy_ProposalNo = item.PolicyNo;
                                        objOtherInsuInfo.TotalSIAtDeath = item.TotalSAAtDeath;
                                        objOtherInsuInfo.CurrentStatus = item.CurrentStatus;
                                        objOtherInsuInfo.IsDeleted = item.IsDeleted;
                                        if (ExistingobjOtherInsuInfo != null)
                                        {
                                            objOtherInsuInfo.MemberID = ExistingobjOtherInsuInfo.MemberID;
                                            objOtherInsuInfo.MemberInsuranceID = ExistingobjOtherInsuInfo.MemberInsuranceID;
                                            Context.Entry(ExistingobjOtherInsuInfo).CurrentValues.SetValues(objOtherInsuInfo);
                                        }

                                    }
                                    else
                                    {
                                        objOtherInsuInfo.AccidentalBenifit = item.AccidentalBenefitAmount;
                                        objOtherInsuInfo.CompanyName = item.CompanyName;
                                        objOtherInsuInfo.CriticalIllnessBenifit = item.CriticalIllnessBenefit;
                                        objOtherInsuInfo.HospitalizationPerDay = item.HospitalizationPerDay;
                                        objOtherInsuInfo.HospitalizationReimbursement = item.HospitalizationReimbursement;
                                        objOtherInsuInfo.Policy_ProposalNo = item.PolicyNo;
                                        objOtherInsuInfo.TotalSIAtDeath = item.TotalSAAtDeath;
                                        objOtherInsuInfo.IsDeleted = item.IsDeleted;
                                        objOtherInsuInfo.CurrentStatus = item.CurrentStatus;
                                        objMemberDetail.tblPolicyMemberInsuranceInfoes.Add(objOtherInsuInfo);
                                    }
                                }
                            }
                        }

                        #endregion

                        #region Policy Member Claim Info
                        objMemberDetail.IsClaimExits = objMember.AreyouClaimedAnyPolicies;
                        if (objMember.MemberID > 0)
                        {
                            UpdatePreviousClaimInfo(objMember.MemberID);
                        }
                        if (objMember.objClaimInfo != null)
                        {
                            if (objMember.objClaimInfo.Count > 0)
                            {

                                foreach (var item in objMember.objClaimInfo.Where(a => a.IsDeleted != true))
                                {
                                    tblPolicyMemberClaimInfo tblMemberClaimInfo = new tblPolicyMemberClaimInfo();
                                    if (objMemberDetail.MemberID > 0 && item.OtherClaimId != null)
                                    {
                                        var ExistingobjClaimInfo = objMemberDetail.tblPolicyMemberClaimInfoes.Where(a => a.MemberID == objMember.MemberID).FirstOrDefault();
                                        tblMemberClaimInfo.CompanyName = item.CompanyName;
                                        tblMemberClaimInfo.NatureOfClaim = item.NatureOfClaim;
                                        tblMemberClaimInfo.ProposalNo = item.PolicyNo;
                                        tblMemberClaimInfo.DateOfClaim = item.DateOfClaim;
                                        tblMemberClaimInfo.IsDeleted = item.IsDeleted;
                                        if (ExistingobjClaimInfo != null)
                                        {
                                            tblMemberClaimInfo.MemberID = ExistingobjClaimInfo.MemberID;
                                            tblMemberClaimInfo.MemberClaimID = ExistingobjClaimInfo.MemberClaimID;
                                            Context.Entry(ExistingobjClaimInfo).CurrentValues.SetValues(tblMemberClaimInfo);
                                        }

                                    }
                                    else
                                    {
                                        tblMemberClaimInfo.CompanyName = item.CompanyName;
                                        tblMemberClaimInfo.NatureOfClaim = item.NatureOfClaim;
                                        tblMemberClaimInfo.ProposalNo = item.PolicyNo;
                                        tblMemberClaimInfo.DateOfClaim = item.DateOfClaim;
                                        tblMemberClaimInfo.IsDeleted = item.IsDeleted;
                                        objMemberDetail.tblPolicyMemberClaimInfoes.Add(tblMemberClaimInfo);
                                    }
                                }
                            }
                        }
                        #endregion

                        if (objMember.MemberID > 0)
                        {
                            Context.Entry(ExisitingobjMemberDetail).CurrentValues.SetValues(objMemberDetail);
                        }
                        else
                        {
                            #region Adding Benefit Details
                            //var QuoteDetails = Context.tblLifeQQs.Where(a => a.QuoteNo == ObjPolicy.QuoteNo).FirstOrDefault();
                            //var QuoteMember = Context.tblQuoteMemberDetials.Where(a => a.MemberID == objlifeQQ.QuoteNo).FirstOrDefault();
                            // var QuoteMember = Context.tblQuoteMemberDetials.Where(a => a.LifeQQID == QuoteDetails.LifeQQID && a.AssuredName==objMember.AssuredName).FirstOrDefault();
                            var QuoteMember = Context.tblQuoteMemberDetials.Where(a => a.MemberID == objMember.QuoteMemberID).FirstOrDefault();
                            if (QuoteMember != null)
                            {
                                foreach (var QuoteBenefit in QuoteMember.tblQuoteMemberBeniftDetials.ToList())
                                {
                                    tblPolicyMemberBenefitDetail objProposalBenifit = new tblPolicyMemberBenefitDetail();
                                    objProposalBenifit.SumInsured = QuoteBenefit.SumInsured;
                                    //objProposalBenifit.Premium = QuoteBenefit.Premium;
                                    objProposalBenifit.Premium = QuoteBenefit.ActualPremium.ToString();
                                    objProposalBenifit.BenifitID = QuoteBenefit.BenifitID;
                                    objProposalBenifit.AssuredName = QuoteMember.AssuredName;
                                    objProposalBenifit.RelationShipWithProposer = QuoteMember.Relationship;
                                    objProposalBenifit.LoadingAmount = Convert.ToString(QuoteBenefit.LoadingAmount);
                                    objProposalBenifit.LoadingPerc = QuoteBenefit.LoadingPercentage;
                                    objProposalBenifit.LoadinPerMille = QuoteBenefit.LoadinPerMille;
                                    // objProposalBenifit.TotalPremium = QuoteBenefit.ActualPremium.ToString();
                                    objProposalBenifit.TotalPremium = QuoteBenefit.Premium;
                                    objProposalBenifit.IsDeleted = false;
                                    objMemberDetail.tblPolicyMemberBenefitDetails.Add(objProposalBenifit);

                                    if (QuoteBenefit.LoadingPercentage > 0)
                                    {
                                        tblMemberBenefitOtherDetail objRiderDetails = new tblMemberBenefitOtherDetail();
                                        objRiderDetails.tblPolicyMemberBenefitDetail = objProposalBenifit;
                                        objRiderDetails.tblPolicyMemberBenefitDetail1 = objProposalBenifit;
                                        objRiderDetails.LoadingType = Context.tblMasCommonTypes.Where(a => a.MasterType == "LoadingType" && a.Description == "Percentage" && a.isDeleted == 0).Select(a => a.CommonTypesID).FirstOrDefault().ToString();
                                        objRiderDetails.LoadingBasis = Context.tblMasCommonTypes.Where(a => a.MasterType == "LoadingBasis" && a.Code == "OC" && a.isDeleted == 0).Select(a => a.CommonTypesID).FirstOrDefault().ToString();
                                        objRiderDetails.LoadingAmount = Convert.ToString(QuoteBenefit.LoadingPercentage);
                                        objRiderDetails.ExtraPremium = Convert.ToString(QuoteBenefit.LoadingAmount);
                                        objRiderDetails.CreatedDate = DateTime.Now;
                                        Context.tblMemberBenefitOtherDetails.Add(objRiderDetails);
                                    }
                                    if (QuoteBenefit.LoadinPerMille > 0)
                                    {
                                        tblMemberBenefitOtherDetail objRiderDetails = new tblMemberBenefitOtherDetail();
                                        objRiderDetails.tblPolicyMemberBenefitDetail = objProposalBenifit;
                                        objRiderDetails.tblPolicyMemberBenefitDetail1 = objProposalBenifit;
                                        objRiderDetails.LoadingType = Context.tblMasCommonTypes.Where(a => a.MasterType == "LoadingType" && a.Description == "Per Milli" && a.isDeleted == 0).Select(a => a.CommonTypesID).FirstOrDefault().ToString();
                                        objRiderDetails.LoadingBasis = Context.tblMasCommonTypes.Where(a => a.MasterType == "LoadingBasis" && a.Code == "OC" && a.isDeleted == 0).Select(a => a.CommonTypesID).FirstOrDefault().ToString();
                                        objRiderDetails.LoadingAmount = Convert.ToString(QuoteBenefit.LoadinPerMille);
                                        objRiderDetails.ExtraPremium = Convert.ToString(QuoteBenefit.LoadingAmount);
                                        objRiderDetails.CreatedDate = DateTime.Now;
                                        Context.tblMemberBenefitOtherDetails.Add(objRiderDetails);
                                    }
                                }
                                objpolicy.tblPolicyMemberDetails.Add(objMemberDetail);
                            }

                            #endregion
                        }
                    }
                }

                #endregion

                #region Nominee Details
                if (ObjPolicy.objNomineeDetails != null)
                {
                    foreach (var Nominee in ObjPolicy.objNomineeDetails)
                    {
                        if (ObjPolicy.PolicyID > 0)
                        {
                            UpdateNomineeDetails(ObjPolicy.PolicyID);
                        }
                        tblPolicyNomineeDetail objNomineeDetails = new tblPolicyNomineeDetail();
                        if (Nominee.NomineeDetailsId > 0)
                        {
                            //decimal NOID = Nominee.NomineeDetailsId;
                            var objNomineeDetailsexisting = objpolicy.tblPolicyNomineeDetails.Where(a => a.NomineeID == Nominee.NomineeDetailsId).FirstOrDefault();
                            objNomineeDetails.NomineeID = objNomineeDetailsexisting.NomineeID;
                            objNomineeDetails.PolicyID = objNomineeDetailsexisting.PolicyID;
                            objNomineeDetails.Salutation = Nominee.NomineeSalutation;
                            objNomineeDetails.NomineeSurName = Nominee.NomineeSurname != null ? Nominee.NomineeSurname.Trim() : Nominee.NomineeSurname;
                            objNomineeDetails.DOB = Nominee.NomineeNicNoDOB;
                            objNomineeDetails.NomineeIntialName = Nominee.NomineeInitial;
                            objNomineeDetails.NomineeName = Nominee.NomineeName != null ? Nominee.NomineeName.Trim() : Nominee.NomineeName;
                            objNomineeDetails.NICNo = Nominee.NomineeNICNo;
                            objNomineeDetails.NomineeGender = Nominee.NomineeGender;
                            objNomineeDetails.NomineeMartialStatus = Nominee.NomineeMaritalStatus;
                            // objNomineeDetails.NomineeAddress = Nominee.NomineeAddress;
                            objNomineeDetails.NomineeMobileNo = Nominee.NomineeTelephone;
                            objNomineeDetails.IsDeleted = Nominee.IsDeleted;
                            objNomineeDetails.ClientCode = Nominee.NomineeClientCode;
                            try
                            {
                                objNomineeDetails.NomineeRelation = Convert.ToInt32(Nominee.NomineeRelationship);
                            }
                            catch (Exception)
                            {

                                objNomineeDetails.NomineeRelation = null;
                            }
                            //objNomineeDetails.NomineeRelation = Convert.ToInt32(Nominee.NomineeRelationship);
                            objNomineeDetails.NomineeShare = Nominee.NomineePercentage;
                            Context.Entry(objNomineeDetailsexisting).CurrentValues.SetValues(objNomineeDetails);
                        }
                        else
                        {
                            objNomineeDetails.Salutation = Nominee.NomineeSalutation;
                            objNomineeDetails.NomineeSurName = Nominee.NomineeSurname;
                            objNomineeDetails.NomineeIntialName = Nominee.NomineeInitial;
                            objNomineeDetails.NomineeName = Nominee.NomineeName != null ? Nominee.NomineeName.Trim() : Nominee.NomineeName;
                            objNomineeDetails.NICNo = Nominee.NomineeNICNo;
                            objNomineeDetails.NomineeGender = Nominee.NomineeGender;
                            objNomineeDetails.NomineeMartialStatus = Nominee.NomineeMaritalStatus;
                            objNomineeDetails.ClientCode = Nominee.NomineeClientCode;
                            objNomineeDetails.NomineeMobileNo = Nominee.NomineeTelephone;
                            objNomineeDetails.DOB = Nominee.NomineeNicNoDOB;
                            objNomineeDetails.IsDeleted = Nominee.IsDeleted;
                            //objNomineeDetails.NomineeRelation = Convert.ToInt32(Nominee.NomineeRelationship);
                            try
                            {
                                objNomineeDetails.NomineeRelation = Convert.ToInt32(Nominee.NomineeRelationship);
                            }
                            catch (Exception)
                            {
                                objNomineeDetails.NomineeRelation = null;
                            }

                            objNomineeDetails.NomineeShare = Nominee.NomineePercentage;
                            objpolicy.tblPolicyNomineeDetails.Add(objNomineeDetails);
                        }
                        //Nominee.Index = Nominee.Index++;
                    }
                }
                #endregion

                #region Document Upload
                try
                {
                    if (ObjPolicy.HdnDocumentDetails != null)
                    {
                        decimal DocumentpolicyID;
                        try
                        {
                            DocumentpolicyID = objpolicy.PolicyID;
                        }
                        catch (Exception)
                        {
                            DocumentpolicyID = 0;
                        }


                        Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
                        List<DocumentUploadFile> objLstDoc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DocumentUploadFile>>(ObjPolicy.HdnDocumentDetails, settings);

                        if (objLstDoc != null && objLstDoc.Count() > 0)
                        {


                            foreach (var Document in objLstDoc)
                            {
                                //if (DocumentpolicyID > 0)
                                //{
                                //    DeleteExisitingPolicyDocuments(Document, DocumentpolicyID);
                                //}
                                var objPOlicyDocumnets = Context.tblPolicyDocuments.Where(a => a.PolicyID == DocumentpolicyID && a.ItemType == "PolicyDocuments" && a.DocumentUploadID == Document.DOCID).FirstOrDefault();
                                if (objPOlicyDocumnets != null)
                                {
                                    objPOlicyDocumnets.DocumentUploadID = Document.DOCID;
                                    objPOlicyDocumnets.PolicyID = DocumentpolicyID;
                                    if (!string.IsNullOrEmpty(Document.FileName))
                                    {
                                        var DocMaster = Context.tblMasDocuments.Where(a => a.DocumentName == Document.FileName).FirstOrDefault();
                                        if (DocMaster != null)
                                        {
                                            if (DocMaster.DocumentType == "Medical")
                                            {
                                                objPOlicyDocumnets.DocumentType = DocMaster.DocumentType;
                                            }
                                            if (DocMaster.DocumentType == "Non Medical")
                                            {
                                                objPOlicyDocumnets.DocumentType = DocMaster.DocumentType;
                                            }
                                            if (DocMaster.DocumentType == "Financial")
                                            {
                                                objPOlicyDocumnets.DocumentType = DocMaster.DocumentType;
                                            }
                                        }
                                    }
                                    objPOlicyDocumnets.FileName = Document.FileName;
                                    objPOlicyDocumnets.CreatedDate = objPOlicyDocumnets.CreatedDate;
                                    if (!String.IsNullOrEmpty(Document.FileName))
                                    {
                                        objPOlicyDocumnets.FilePath = objPOlicyDocumnets.FilePath;

                                    }
                                    objPOlicyDocumnets.ItemType = "PolicyDocuments";
                                    objPOlicyDocumnets.MemberType = Document.MemberType;
                                    objpolicy.tblPolicyDocuments.Add(objPOlicyDocumnets);
                                    Context.SaveChanges();
                                }
                                else
                                {
                                    tblPolicyDocument objtblPolicyDocument = new tblPolicyDocument();
                                    objtblPolicyDocument.PolicyID = DocumentpolicyID;
                                    if (!string.IsNullOrEmpty(Document.FileName))
                                    {
                                        var DocMaster = Context.tblMasDocuments.Where(a => a.DocumentName == Document.FileName).FirstOrDefault();
                                        if (DocMaster != null)
                                        {
                                            if (DocMaster.DocumentType == "Medical")
                                            {
                                                objtblPolicyDocument.DocumentType = DocMaster.DocumentType;
                                            }
                                            if (DocMaster.DocumentType == "Non Medical")
                                            {
                                                objtblPolicyDocument.DocumentType = DocMaster.DocumentType;
                                            }
                                            if (DocMaster.DocumentType == "Financial")
                                            {
                                                objtblPolicyDocument.DocumentType = DocMaster.DocumentType;
                                            }
                                        }
                                    }
                                    objtblPolicyDocument.FileName = Document.FileName;
                                    objtblPolicyDocument.CreatedDate = DateTime.Now;
                                    objtblPolicyDocument.ItemType = "PolicyDocuments";
                                    objtblPolicyDocument.MemberType = Document.MemberType;
                                    objtblPolicyDocument.FilePath = Document.FilePath;
                                    objpolicy.tblPolicyDocuments.Add(objtblPolicyDocument);

                                }
                                //tblPolicyDocument objPolicyDocument = new tblPolicyDocument();
                                //objPolicyDocument.FileName = Document.FileName;
                                //objPolicyDocument.FilePath = Document.FilePath;
                                //// objPolicyDocument.File = ObjPolicy.ProposerSignatureFile;
                                //#region Set Document Type
                                //if (!string.IsNullOrEmpty(Document.FileName))
                                //{
                                //    var DocMaster = Context.tblMasDocuments.Where(a => a.DocumentName == Document.FileName).FirstOrDefault();
                                //    if (DocMaster != null)
                                //    {
                                //        if (DocMaster.DocumentType == "Medical")
                                //        {
                                //            objPolicyDocument.DocumentType = DocMaster.DocumentType;
                                //        }
                                //        if (DocMaster.DocumentType == "Non Medical")
                                //        {
                                //            objPolicyDocument.DocumentType = DocMaster.DocumentType;
                                //        }
                                //        if (DocMaster.DocumentType == "Financial")
                                //        {
                                //            objPolicyDocument.DocumentType = DocMaster.DocumentType;
                                //        }
                                //    }
                                //}
                                //#endregion
                                //objPolicyDocument.ItemType = Document.ItemType;
                                //objPolicyDocument.CreatedDate = DateTime.Now;
                                //objPolicyDocument.MemberType = Document.MemberType;

                                // FTP File Upload
                                //byte[] bytes = System.IO.File.ReadAllBytes(Document.FilePath);
                                //System.IO.File.WriteAllBytes(Document.FilePath, bytes);

                            }

                        }
                        if (ObjPolicy.ProposerSignatureFile != null)
                        {
                            tblPolicyDocument objPolicyDocument = objpolicy.tblPolicyDocuments.Where(a => a.FileName == "Proposer Signature").FirstOrDefault();
                            if (objPolicyDocument == null)
                                objPolicyDocument = new tblPolicyDocument();
                            objPolicyDocument.FileName = "Proposer Signature";
                            objPolicyDocument.ItemType = "Proposer Signature";
                            if (!string.IsNullOrEmpty(ObjPolicy.Signature))
                            {
                                objPolicyDocument.FilePath = ObjPolicy.Signature;
                            }
                            else if (!string.IsNullOrEmpty(ObjPolicy.ProposerSignPath))
                            {
                                objPolicyDocument.FilePath = ObjPolicy.ProposerSignPath;
                            }
                            objPolicyDocument.File = ObjPolicy.ProposerSignatureFile;
                            objPolicyDocument.CreatedDate = DateTime.Now;
                            if (objPolicyDocument.DocumentUploadID == decimal.Zero)
                                objpolicy.tblPolicyDocuments.Add(objPolicyDocument);
                        }
                        if (ObjPolicy.WPSignatureFile != null)
                        {
                            tblPolicyDocument objPolicyDocument = objpolicy.tblPolicyDocuments.Where(a => a.FileName == "WPProposerSignature").FirstOrDefault();
                            if (objPolicyDocument == null)
                                objPolicyDocument = new tblPolicyDocument();
                            objPolicyDocument.FileName = "WPProposerSignature";
                            objPolicyDocument.ItemType = "WPProposerSignature";
                            if (!string.IsNullOrEmpty(ObjPolicy.WPProposerSignature))
                            {
                                objPolicyDocument.FilePath = ObjPolicy.WPProposerSignature;
                            }
                            else if (!string.IsNullOrEmpty(ObjPolicy.WPProposerSignPath))
                            {
                                objPolicyDocument.FilePath = ObjPolicy.WPProposerSignPath;
                            }
                            objPolicyDocument.File = ObjPolicy.WPSignatureFile;
                            objPolicyDocument.CreatedDate = DateTime.Now;
                            if (objPolicyDocument.DocumentUploadID == decimal.Zero)
                                objpolicy.tblPolicyDocuments.Add(objPolicyDocument);
                        }
                        if (ObjPolicy.SpouseSignatureFile != null)
                        {
                            tblPolicyDocument objPolicyDocument = objpolicy.tblPolicyDocuments.Where(a => a.FileName == "SpouseSignature").FirstOrDefault();
                            if (objPolicyDocument == null)
                                objPolicyDocument = new tblPolicyDocument();
                            objPolicyDocument.FileName = "SpouseSignature";
                            objPolicyDocument.ItemType = "SpouseSignature";
                            if (!string.IsNullOrEmpty(ObjPolicy.SpouseSignature))
                            {
                                objPolicyDocument.FilePath = ObjPolicy.SpouseSignature;
                            }
                            else if (!string.IsNullOrEmpty(ObjPolicy.SpouseSignPath))
                            {
                                objPolicyDocument.FilePath = ObjPolicy.SpouseSignPath;
                            }
                            objPolicyDocument.File = ObjPolicy.SpouseSignatureFile;
                            objPolicyDocument.CreatedDate = DateTime.Now;
                            if (objPolicyDocument.DocumentUploadID == decimal.Zero)
                                objpolicy.tblPolicyDocuments.Add(objPolicyDocument);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                    Logger.Error(ex);
                }
                #endregion

                #region Premium Details
                tblProposalPremium objtblProposalPremium = null;
                objtblProposalPremium = objpolicy.tblProposalPremiums.FirstOrDefault();
                if (objtblProposalPremium == null)
                {
                    objtblProposalPremium = new tblProposalPremium();
                }
                objtblProposalPremium.AnnualPremium = ObjPolicy.AnnualPremium;
                objtblProposalPremium.BasicPremium = ObjPolicy.Premium;
                objtblProposalPremium.HalfYearlyPremium = ObjPolicy.HalfYearlyPremium;
                objtblProposalPremium.QuaterlyPremium = ObjPolicy.QuaterlyPremium;
                objtblProposalPremium.MonthlyPremium = ObjPolicy.MonthlyPremium;
                objtblProposalPremium.VAT = ObjPolicy.VAT;
                objtblProposalPremium.Cess = ObjPolicy.Cess;
                objtblProposalPremium.PolicyFee = ObjPolicy.PolicyFee;
                objtblProposalPremium.AdditionalPremium = ObjPolicy.AdditionalPremium;
                if (objtblProposalPremium.PremiumID == decimal.Zero)
                {
                    objtblProposalPremium.tblPolicy = objpolicy;
                    Context.tblProposalPremiums.Add(objtblProposalPremium);
                }
                #endregion

                #region Policy Extension
                tblPolicyExtension objtblpolicyextension = new tblPolicyExtension();
                var ExistingPolicyExtension = objpolicy.tblPolicyExtensions.FirstOrDefault();
                if (ExistingPolicyExtension != null)
                {
                    objtblpolicyextension.DoctorName = ObjPolicy.DoctorName;
                    objtblpolicyextension.LabName = ObjPolicy.LabName;
                    objtblpolicyextension.PaymentMadeByForDoctor = ObjPolicy.PaymentMadeByForDoctor;
                    objtblpolicyextension.PaymentMadeByForLab = ObjPolicy.PaymentMadeByForLab;
                    objtblpolicyextension.ReportsTobeSendTo = ObjPolicy.ReportsTobeSendTo;
                    objtblpolicyextension.ProposerDate = ObjPolicy.ProposerDate;
                    objtblpolicyextension.ProposerPlace = ObjPolicy.ProposerPlace;
                    objtblpolicyextension.ProposerCountry = ObjPolicy.ProposerCountry;
                    objtblpolicyextension.ProposerDocumentType = ObjPolicy.ProposerDocumentType;
                    objtblpolicyextension.ProposerWealthPlanner = ObjPolicy.WPDeclaration;
                    //objtblpolicyextension.ProposerWealthPlannerPolicy = ObjPolicy.ProposerWealthPlannerPolicyDateing;
                    // objtblpolicyextension.ProposerWealthPlannerPolicyBackDate = ObjPolicy.ProposerWealthPlannerPolicyDate == null ? DateTime.Now : Convert.ToDateTime(ObjPolicy.ProposerWealthPlannerPolicyDate);
                    objtblpolicyextension.ProposerWealthPlannerComments = ObjPolicy.ProposerWealthPlannerComments;
                    objtblpolicyextension.SpouseDate = ObjPolicy.SpouseDate;
                    objtblpolicyextension.SpousePlace = ObjPolicy.SpousePlace;
                    objtblpolicyextension.SpouseCountry = ObjPolicy.SpouseCountry;
                    objtblpolicyextension.SpouseDocumentType = ObjPolicy.SpouseDocumentType;
                    objtblpolicyextension.SpouseWealthPlanner = ObjPolicy.SpouseWealthPlanner;
                    objtblpolicyextension.SpouseWealthPlannerPolicy = ObjPolicy.SpouseWealthPlannerPolicyDateing;
                    objtblpolicyextension.SpouseWealthPlannerPolicyBackDate = ObjPolicy.SpouseWealthPlannerPolicyDate;
                    objtblpolicyextension.SpouseWealthPlannerComments = ObjPolicy.SpouseWealthPlannerComments;

                    //objtblpolicyextension.ProspectSignPath = ObjPolicy.ProspectSignPath;
                    //objtblpolicyextension.SpouseSignPath = ObjPolicy.SpouserSignPath;
                    objtblpolicyextension.Declaration = ObjPolicy.Declaration;
                    objtblpolicyextension.ProposalNeed = ObjPolicy.ProposalNeed;
                    objtblpolicyextension.PolicyID = ExistingPolicyExtension.PolicyID;
                    objtblpolicyextension.PolicyExtensionID = ExistingPolicyExtension.PolicyExtensionID;
                    Context.Entry(ExistingPolicyExtension).CurrentValues.SetValues(objtblpolicyextension);
                }
                else
                {
                    objtblpolicyextension.DoctorName = ObjPolicy.DoctorName;
                    objtblpolicyextension.LabName = ObjPolicy.LabName;
                    objtblpolicyextension.PaymentMadeByForDoctor = ObjPolicy.PaymentMadeByForDoctor;
                    objtblpolicyextension.PaymentMadeByForLab = ObjPolicy.PaymentMadeByForLab;
                    objtblpolicyextension.ReportsTobeSendTo = ObjPolicy.ReportsTobeSendTo;
                    objtblpolicyextension.ProposerDate = ObjPolicy.ProposerDate;
                    objtblpolicyextension.ProposerPlace = ObjPolicy.ProposerPlace;
                    objtblpolicyextension.ProposerCountry = ObjPolicy.ProposerCountry;
                    objtblpolicyextension.ProposerDocumentType = ObjPolicy.ProposerDocumentType;
                    objtblpolicyextension.ProposerWealthPlanner = ObjPolicy.WPDeclaration;
                    //objtblpolicyextension.ProposerWealthPlannerPolicy = ObjPolicy.ProposerWealthPlannerPolicyDateing;
                    //objtblpolicyextension.ProposerWealthPlannerPolicyBackDate = ObjPolicy.ProposerWealthPlannerPolicyDate == null ? DateTime.Now : Convert.ToDateTime(ObjPolicy.ProposerWealthPlannerPolicyDate);
                    objtblpolicyextension.ProposerWealthPlannerComments = ObjPolicy.ProposerWealthPlannerComments;
                    // objtblpolicyextension.ProspectSignPath = ObjPolicy.ProspectSignPath;
                    // objtblpolicyextension.SpouseSignPath = ObjPolicy.SpouserSignPath;
                    objtblpolicyextension.SpouseDate = ObjPolicy.SpouseDate;
                    objtblpolicyextension.SpousePlace = ObjPolicy.SpousePlace;
                    objtblpolicyextension.SpouseCountry = ObjPolicy.SpouseCountry;
                    objtblpolicyextension.SpouseDocumentType = ObjPolicy.SpouseDocumentType;
                    objtblpolicyextension.SpouseWealthPlanner = ObjPolicy.SpouseWealthPlanner;
                    objtblpolicyextension.SpouseWealthPlannerPolicy = ObjPolicy.SpouseWealthPlannerPolicyDateing;
                    objtblpolicyextension.SpouseWealthPlannerPolicyBackDate = ObjPolicy.SpouseWealthPlannerPolicyDate;
                    objtblpolicyextension.SpouseWealthPlannerComments = ObjPolicy.SpouseWealthPlannerComments;
                    objtblpolicyextension.Declaration = ObjPolicy.Declaration;
                    objtblpolicyextension.ProposalNeed = ObjPolicy.ProposalNeed;
                    objpolicy.tblPolicyExtensions.Add(objtblpolicyextension);
                }

                #endregion


                if (objpolicy.PolicyID == 0)
                {
                    if (objlifeQQ != null)
                    {
                        objlifeQQ.StatusID = 2;
                    }
                    objpolicy.tblPolicyRelationships.Add(objtblpolicyrelationship);
                    // For Demo purpose Added payment status

                    Context.tblPolicies.Add(objpolicy);
                }
                if (ObjPolicy.ProcceedToPayment)
                {
                    objpolicy.PolicyStageStatusID = 1153;  // Payment pending
                }
                else
                {
                    objpolicy.PolicyStageStatusID = 476;// Proposal Stage
                }
                string userId = Common.CommonBusiness.GetUserId(ObjPolicy.UserName);
                objpolicy.Createdby = userId;
                objpolicy.IsAfc = ObjPolicy.IsAfc;
                Context.SaveChanges();

                ObjPolicy.ProposalNo = objpolicy.ProposalNo;
                ObjPolicy.Message = "Success";

                ObjPolicy.PolicyID = objpolicy.PolicyID;
                
                return ObjPolicy;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                ObjPolicy.Message = ex.InnerException.Message;
                return ObjPolicy;
            }
        }
        public AIA.Life.Models.Policy.Policy SendMedicalLetterMail(AIA.Life.Models.Policy.Policy objPolicy)
        {
            try
            {

                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                var PolicyId = Context.tblPolicies.Where(a => a.QuoteNo == objPolicy.QuoteNo).Select(a => a.PolicyID).FirstOrDefault();
                var PolicyDet = Context.tblPolicies.Where(a => a.QuoteNo == objPolicy.QuoteNo).FirstOrDefault();
                var Details = Context.tblPolicyMemberDetails.Where(a => a.PolicyID == PolicyId).FirstOrDefault();
                EmailIntegration ObjEmailIntegration = new EmailIntegration();
                EmailDetails ObjEmailDetails = new EmailDetails();
                AIA.Data.Life.API.Controllers.ReportsController objReportController = new AIA.Data.Life.API.Controllers.ReportsController();
                Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
                ObjEmailDetails.EmailID = Details.Email;
                var Sal = Details.Salutation;
                var ESalutation = Context.tblMasCommonTypes.Where(a => a.Code == Sal && a.MasterType == "Salutation").Select(a => a.Description).FirstOrDefault();
                var ESalu = Context.tblMasCommonTypes.Where(a => a.Description == Sal && a.MasterType == "Salutation").Select(a => a.Description).FirstOrDefault();
                if (!String.IsNullOrEmpty(ESalutation))
                {
                    ObjEmailDetails.Salutation = ESalutation;
                }
                else if (!String.IsNullOrEmpty(ESalu))
                {
                    ObjEmailDetails.Salutation = ESalu;
                }
                else
                {
                    ObjEmailDetails.Salutation = Sal;
                }
                ObjEmailDetails.Subject = "Medical Letter for " + ObjEmailDetails.Salutation + " " + objCommonBusiness.ConverttoTitlecase(Details.LastName) + " - " + PolicyDet.ProposalNo;
                ObjEmailDetails.MailTemplate = "T016";
                ObjEmailDetails.QuoteNumber = objPolicy.QuoteNo;
                ObjEmailDetails.ProposalNo = PolicyDet.ProposalNo;
                ObjEmailDetails.AgentEmailID = Context.tblMasIMOUsers.Where(a => a.UserID == objPolicy.UserName).Select(a => a.Email).FirstOrDefault();
                ObjEmailDetails.WPMobileNo = Context.tblMasIMOUsers.Where(a => a.UserID == objPolicy.UserName).Select(a => a.MobileNo).FirstOrDefault();
                //ObjEmailDetails.Name = objPolicy.objProspectDetails.FirstName;
                //ObjEmailDetails.Salutation = objCommonBusiness.ConverttoTitlecase(Details.Salutation);//objPolicy.objProspectDetails.Salutation;
                ObjEmailDetails.Name = objCommonBusiness.ConverttoTitlecase(Details.LastName);
                ObjEmailDetails.EmailID = Details.Email;
                ObjEmailDetails.ProductName = objPolicy.ProductCode; //Context.tblProducts.Where(a => a.ProductId == plan).Select(b => b.ProductName).FirstOrDefault();
                ObjEmailDetails.Environment = Convert.ToString(ConfigurationManager.AppSettings["Environment"]);
                ObjEmailDetails.ByteArray = objPolicy.ByteArray;
                ObjEmailIntegration.EmailNotification(ObjEmailDetails);
                objPolicy.Message = "Success";
                return objPolicy;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                objPolicy.Message = "Error";
                return objPolicy;
            }
        }

        public AIA.Life.Models.Policy.Policy SendEmailAndSMSNotificationOnSaveProposal(AIA.Life.Models.Policy.Policy objPolicy)
        {
            try
            {

                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                //int plan = Convert.ToInt32(objPolicy.PlanCode);
                objPolicy.PlanCode = Context.tblProducts.Where(a => a.ProductName == objPolicy.PlanName).FirstOrDefault().ProductId.ToString();
                int plan = Context.tblProducts.Where(a => a.ProductName == objPolicy.PlanName).FirstOrDefault().ProductId;

                EmailIntegration ObjEmailIntegration = new EmailIntegration();
                EmailDetails ObjEmailDetails = new EmailDetails();
                AIA.Data.Life.API.Controllers.ReportsController objReportController = new AIA.Data.Life.API.Controllers.ReportsController();
                Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
                ObjEmailDetails.EmailID = objPolicy.objProspectDetails.Email;
                ObjEmailDetails.Subject = "Completed Life Insurance Aplication of " + objPolicy.objProspectDetails.Salutation + " " + objCommonBusiness.ConverttoTitlecase(objPolicy.objProspectDetails.LastName) + "-" + objPolicy.ProposalNo;
                ObjEmailDetails.MailTemplate = "T003";
                ObjEmailDetails.QuoteNumber = objPolicy.QuoteNo;
                ObjEmailDetails.ProposalNo = objPolicy.ProposalNo;
                ObjEmailDetails.WPMobileNo = Context.tblMasIMOUsers.Where(a => a.UserName == objPolicy.UserName).Select(a => a.MobileNo).FirstOrDefault();
                //ObjEmailDetails.Name = objPolicy.objProspectDetails.FirstName;
                ObjEmailDetails.Salutation = objPolicy.objProspectDetails.Salutation;
                ObjEmailDetails.Name = objCommonBusiness.ConverttoTitlecase(objPolicy.objProspectDetails.LastName);
                ObjEmailDetails.Premium = Convert.ToString(objPolicy.AnnualPremium);
                ObjEmailDetails.ProductName = Context.tblProducts.Where(a => a.ProductId == plan).Select(b => b.ProductName).FirstOrDefault();
                ObjEmailDetails.PolicyTerm = Convert.ToString(objPolicy.PolicyTerm);
                ObjEmailDetails.PremiumPayingTerm = objPolicy.PaymentTerm;
                ObjEmailDetails.Environment = Convert.ToString(ConfigurationManager.AppSettings["Environment"]);
                ObjEmailDetails.ByteArray = objPolicy.ByteArray;

                ObjEmailIntegration.EmailNotification(ObjEmailDetails);

                if (objPolicy.ProcceedToPayment)
                {
                    ProposalEmailingSendSMS(objPolicy);
                }
                //else
                //{
                //    ProposalSendSMS(objPolicy);
                //}

                objPolicy.Message = "Success";
                return objPolicy;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                objPolicy.Message = "Error";
                return objPolicy;
            }
        }

        //public void ProposalSendSMS(AIA.Life.Models.Policy.Policy objPolicy)
        //{
        //    try
        //    {
        //        AVOAIALifeEntities Context = new AVOAIALifeEntities();
        //        SMSIntegration objSMSIntegration = new SMSIntegration();
        //        SMSDetails objSMSDetails = new SMSDetails();
        //        Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
        //        objSMSDetails.Salutation = objCommonBusiness.ConverttoTitlecase(Context.tblMasCommonTypes.Where(a => a.Code == objPolicy.objProspectDetails.Salutation).Select(a => a.ShortDesc).FirstOrDefault());//objCommonBusiness.ConverttoTitlecase(objPolicy.objProspectDetails.Salutation);
        //        objSMSDetails.Name = objCommonBusiness.ConverttoTitlecase(objPolicy.objProspectDetails.LastName);
        //        objSMSDetails.ProposalNumber = objPolicy.ProposalNo;
        //        objSMSDetails.SMSTemplate = "S003";
        //        objSMSDetails.MobileNumber = objPolicy.objProspectDetails.MobileNo;
        //        objSMSDetails.WPMobileNumber = Context.tblMasIMOUsers.Where(a => a.UserName == objPolicy.UserName).Select(a => a.MobileNo).FirstOrDefault();
        //        objSMSDetails.Category = "Life Insuarnce Aplication of " + objPolicy.objProspectDetails.Salutation + "." + objCommonBusiness.ConverttoTitlecase(objPolicy.objProspectDetails.LastName); ;
        //        objSMSDetails.SMSEnvironment = Convert.ToString(ConfigurationManager.AppSettings["SMSEnvironment"]);
        //        objSMSIntegration.SMSNotification(objSMSDetails);

        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}

        public void ProposalEmailingSendSMS(AIA.Life.Models.Policy.Policy objPolicy)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                SMSIntegration objSMSIntegration = new SMSIntegration();
                SMSDetails objSMSDetails = new SMSDetails();
                Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
                //objSMSDetails.Salutation = objCommonBusiness.ConverttoTitlecase(Context.tblMasCommonTypes.Where(a => a.Code == objPolicy.objProspectDetails.Salutation).Select(a => a.ShortDesc).FirstOrDefault());//objCommonBusiness.ConverttoTitlecase(objPolicy.objProspectDetails.Salutation);
                //objSMSDetails.Name = objCommonBusiness.ConverttoTitlecase(objPolicy.objProspectDetails.LastName);
                objSMSDetails.ProposalNumber = objPolicy.ProposalNo;
                objSMSDetails.SMSTemplate = "S005";
                //objSMSDetails.MobileNumber = objPolicy.objProspectDetails.MobileNo;
                objSMSDetails.WPMobileNumber = Context.tblMasIMOUsers.Where(a => a.UserID == objPolicy.UserName).Select(a => a.MobileNo).FirstOrDefault();
                objSMSDetails.Category = "Life Insuarnce Aplication of " + objPolicy.objProspectDetails.Salutation + "." + objCommonBusiness.ConverttoTitlecase(objPolicy.objProspectDetails.LastName);
                objSMSDetails.SMSEnvironment = Convert.ToString(ConfigurationManager.AppSettings["SMSEnvironment"]);
                objSMSIntegration.SMSNotification(objSMSDetails);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void FillMemberDetails(AVOAIALifeEntities Context, tblPolicyMemberDetail objMemberDetail, MemberDetails objMember, String QuoteNO)
        {
            try
            {
                //objMemberDetail = Context.tblPolicyMemberDetails.Where(a => a.MemberID == objMember.MemberID).FirstOrDefault();
                if (objMemberDetail == null)
                {
                    objMemberDetail = new tblPolicyMemberDetail();
                }
                objMemberDetail.OCR = objMember.IsOCRSeleted;
                objMemberDetail.OCRImageRecognition = objMember.IsOCRImageRecognition;
                objMemberDetail.MemberPremium = objMember.Memberpremium;
                objMemberDetail.ClientCode = objMember.ClientCode;
                objMemberDetail.SAM = objMember.SAM;
                objMemberDetail.Age = objMember.Age;
                objMemberDetail.CompanyName = objMember.CompanyName;
                objMemberDetail.DOB = objMember.DateOfBirth;
                objMemberDetail.Email = objMember.Email;
                objMemberDetail.FirstName = objMember.FirstName != null ? objMember.FirstName.Trim() : objMember.FirstName;
                objMemberDetail.MiddleName = objMember.MiddleName;
                objMemberDetail.LastName = objMember.LastName != null ? objMember.LastName.Trim() : objMember.LastName;


                objMemberDetail.Gender = objMember.Gender;
                //if (objMember.Gender != null)
                //{
                //    //objMemberDetail.Gender = Convert.ToInt32(objMember.Gender);
                // objMemberDetail.Gender = GetMaritialStatus(objMember.Gender);
                //}
                //#region Set Gender Based on relationship
                //if (objMember.RelationShipWithPropspect == "269")
                //{
                //    //objMemberDetails[i].Gender = Context.tblQuoteMemberDetials.Where(a => a.Relationship == "269").Select(a => a.Gender).FirstOrDefault();
                //    objMemberDetail.Gender = "M";
                //}
                //else if (objMember.RelationShipWithPropspect == "270")
                //{
                //    //objMemberDetails[i].Gender = Context.tblQuoteMemberDetials.Where(a => a.Relationship == "270").Select(a => a.Gender).FirstOrDefault();
                //    objMemberDetail.Gender = "F";
                //}
                //#endregion
                objMemberDetail.Landline = objMember.HomeNumber;
                objMemberDetail.MaritialStatus = objMember.MaritialStatus;
                //objMemberDetail.MaritialStatus = Convert.ToInt32(objMember.MaritialStatus);
                objMemberDetail.Mobile = objMember.MobileNo;
                objMemberDetail.AlternateMobileNo = objMember.OtherMobileNo;
                objMemberDetail.Home = objMember.HomeNumber;
                objMemberDetail.Work = objMember.WorkNumber;
                objMemberDetail.MonthlyIncome = objMember.MonthlyIncome;
                objMemberDetail.NameWithInitial = objMember.NameWithInitial;
                objMemberDetail.Nationality = objMember.Nationality;
                objMemberDetail.NatureOfDuties = objMember.NameOfDuties;
                objMemberDetail.NEWNICNO = objMember.NewNICNO;
                // objMemberDetail.OccupationID = GetSaveOccupationID(objMember.OccupationID);
                objMemberDetail.OccupationID = objMember.OccupationID;
                objMemberDetail.OLDNICNO = objMember.OLDNICNo;
                objMemberDetail.PreferredName = objMember.PrefferedName;
                objMemberDetail.RelationShipWithProposer = Convert.ToInt32(objMember.RelationShipWithPropspect);
                string title = Context.tblMasCommonTypes.Where(a => a.Description == objMember.Salutation && a.MasterType == "Salutation").Select(b => b.Code).FirstOrDefault();
                if (string.IsNullOrEmpty(title))
                    objMemberDetail.Salutation = objMember.Salutation;
                else
                    objMemberDetail.Salutation = title;
                objMemberDetail.Assuredname = objMember.AssuredName;
                objMemberDetail.QuoteMemberid = objMember.QuoteMemberID;

                objMemberDetail.NoofJsPolicies = objMember.NoofJsPolicies;
                objMemberDetail.NoofOtherPolicies = objMember.NoofOtherPolicies;
                objMemberDetail.IsOtherPolicy = objMember.AreyouCoveredUnderOtherPolicies;
                objMemberDetail.TotalAccidental = objMember.TotalAccidental;
                objMemberDetail.TotalCritical = objMember.TotalCritical;
                objMemberDetail.TotalDeath = objMember.TotalDeath;
                objMemberDetail.TotalHospitalization = objMember.TotalHospitalization;
                objMemberDetail.TotalHospitalizationReimbursement = objMember.TotalHospitalizationReimbursement;
                objMemberDetail.IsSameasProposerAddress = objMember.IsSameasProposerAddress;
                objMemberDetail.BasicSuminsured = objMember.BasicSumInsured;
                objMemberDetail.BasicPremium = objMember.Basicpremium;

                objMemberDetail.CitizenShip = objMember.CitizenShip;
                objMemberDetail.Citizenship1 = objMember.Citizenship1;
                objMemberDetail.Citizenship2 = objMember.Citizenship2;
                objMemberDetail.ResidentialNationality = objMember.Residential;
                objMemberDetail.ResidentialNationalityStatus = objMember.ResidentialStatus;

                objMemberDetail.OccupationHazardousWork = ((objMember.OccupationHazardousWork == null && objMember.SpecifiyOccupationHazardousWork != null) ? true : objMember.OccupationHazardousWork);
                objMemberDetail.SpecifyHazardousWork = objMember.SpecifiyOccupationHazardousWork;
                objMemberDetail.PassportNumber = objMember.PassportNumber;
                objMemberDetail.DrivingLicense = objMember.DrivingLicense;
                objMemberDetail.USTaxpayerId = objMember.USTaxpayerId;
                objMemberDetail.CountryOccupation = objMember.CountryofOccupation;
                //objMemberDetail.LifeAssured = objMember.LifeAssured;


                #region FillAddressDetails
                tblAddress objtbladdress = objMemberDetail.tblAddress1;
                if (objtbladdress == null)
                {
                    objtbladdress = new tblAddress();
                }
                objtbladdress = FillAddressDetails(objMember.objCommunicationAddress, objtbladdress);
                objMemberDetail.tblAddress1 = objtbladdress;
                if (objMember.IsRegAsCommunication)
                {
                    objMemberDetail.IsPermanentAddrSameasCommAddr = true;
                    objMemberDetail.tblAddress = objtbladdress;
                }
                else
                {
                    tblAddress objtblPermanentaddress = objMemberDetail.tblAddress;
                    if (objtblPermanentaddress == null)
                    {
                        objtblPermanentaddress = new tblAddress();
                    }
                    objtblPermanentaddress = FillAddressDetails(objMember.objPermenantAddress, objtblPermanentaddress);
                    objMemberDetail.tblAddress = objtblPermanentaddress;
                }

                #endregion
                GetSARAndFALDetails(ref objMember, QuoteNO);

                objMemberDetail.FAL = objMember.FAL;
                objMemberDetail.SAR = objMember.SAR;
                objMemberDetail.AFC = objMember.AFC;
                objMemberDetail.AnnualPremium = objMember._AnnualPremium;

            }
            catch (Exception ex)
            {


            }
        }
        public void GetSARAndFALDetails(ref MemberDetails objMember, string QuoteNO)
        {
            decimal SAR = decimal.Zero;
            decimal FAL = decimal.Zero;
            bool IsAFC = false;
            SAR = objMember.SAR;
            FAL = objMember.FAL;
            IsAFC = objMember.AFC;
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    if (!string.IsNullOrEmpty(objMember.NewNICNO))
                    {

                        // Current proposal Annual premium
                        int QuoteMemberID = objMember.QuoteMemberID;
                        var _QuoteMemberDetail = Context.tblQuoteMemberDetials.Where(a => a.MemberID == QuoteMemberID).FirstOrDefault();
                        if (_QuoteMemberDetail != null)
                        {
                            if (!string.IsNullOrEmpty(_QuoteMemberDetail.MemberPremium))
                            {
                                objMember._CurrentproposalAnnualPremium = Convert.ToDecimal(_QuoteMemberDetail.MemberPremium);
                            }
                        }
                        // Till here

                        var SARDetails = Context.SP_GetSARDetails(objMember.NewNICNO).FirstOrDefault();
                        if (SARDetails != null)
                        {
                            // Previous Proposal Annual premium
                            decimal AnnualPrem = Convert.ToDecimal(SARDetails.ANNPREM);
                            objMember._AnnualPremium = AnnualPrem + objMember._CurrentproposalAnnualPremium;
                            if (objMember._AnnualPremium > 250000)
                            {
                                IsAFC = true;
                            }
                            // Till here
                        }
                        //var FALDetails = Context.SP_GetFALDetails(objMember.NewNICNO).FirstOrDefault();
                        //if (FALDetails != null)
                        //{
                        //    FAL = FAL + FALDetails.Value;
                        //}
                        //var CurrentProposalFAL = Context.SP_GetFALDetailsForQuote(QuoteNO).ToList();
                        //if (CurrentProposalFAL != null)
                        //{
                        //    decimal QuoteMemberId = objMember.QuoteMemberID;
                        //    var MemberInfo = CurrentProposalFAL.Where(a => a.MemberID == QuoteMemberId).FirstOrDefault();
                        //    if (MemberInfo != null)
                        //    {
                        //        FAL = FAL + Convert.ToDecimal(MemberInfo.FAL);
                        //    }
                        //}

                    }

                    var CurrentProposalSAR = Context.SP_GetSARAndFALDetailsForQuote(QuoteNO).ToList();
                    if (CurrentProposalSAR != null)
                    {
                        decimal QuoteMemberId = objMember.QuoteMemberID;
                        var MemberInfo = CurrentProposalSAR.Where(a => a.MemberID == QuoteMemberId).FirstOrDefault();
                        if (MemberInfo != null)
                        {
                            SAR = Convert.ToDecimal(MemberInfo.SAR);
                            FAL = Convert.ToDecimal(MemberInfo.FAL);
                        }
                    }
                }

                objMember.SAR = Convert.ToDecimal(SAR);
                objMember.FAL = Convert.ToDecimal(FAL);
                objMember.AFC = IsAFC;
            }
            catch (Exception)
            {
                objMember.SAR = Convert.ToDecimal(SAR);
                objMember.FAL = Convert.ToDecimal(FAL);
                objMember.AFC = IsAFC;
            }
        }

        public tblPolicyMemberFamilyHistory FillFamilyHistory(tblPolicyMemberFamilyHistory objtblfamilyHistory, LifeAssuredFamilyBackground objfamilyHistory)
        {
            objtblfamilyHistory.PresentAge = objfamilyHistory.PresentAge;
            objtblfamilyHistory.RelationshipWithMember = objfamilyHistory.FamilyPersonType;
            objtblfamilyHistory.StateofHealth = objfamilyHistory.StateOfHealth;
            objtblfamilyHistory.AgeAtDeath = objfamilyHistory.AgeAtDeath;
            objtblfamilyHistory.CauseofDeath = objfamilyHistory.Cause;
            objtblfamilyHistory.Details = objfamilyHistory.Details;
            return objtblfamilyHistory;
        }

        public void UpdateFamilyHistory(LifeAssuredFamilyBackground objfamilyHistory, int MemberID)
        {
            AVOAIALifeEntities Entity = new AVOAIALifeEntities();
            tblPolicyMemberFamilyHistory objtblfamilyHistory = Entity.tblPolicyMemberFamilyHistories.Where(a => a.MemberFamilyHistoryID == objfamilyHistory.FamilyBackgroundId).FirstOrDefault();
            objtblfamilyHistory.PresentAge = objfamilyHistory.PresentAge;
            objtblfamilyHistory.RelationshipWithMember = objfamilyHistory.FamilyPersonType;
            objtblfamilyHistory.StateofHealth = objfamilyHistory.StateOfHealth;
            objtblfamilyHistory.AgeAtDeath = objfamilyHistory.AgeAtDeath;
            objtblfamilyHistory.CauseofDeath = objfamilyHistory.Cause;
            objtblfamilyHistory.Details = objfamilyHistory.Details;
            Entity.SaveChanges();

        }

        public void UpdateLifeStyleDetails(int LifeStyleMemberID)
        {

            try
            {
                using (AVOAIALifeEntities Entity = new AVOAIALifeEntities())
                {
                    var MemberLifeStyleDetails = Entity.tblMemberLifeStyleDetails.Where(a => a.MemberLifeStyleID == LifeStyleMemberID).FirstOrDefault();
                    if (MemberLifeStyleDetails != null)
                    {
                        List<tblMemberAdditionalLifeStyleDetail> lstAdditionalInfo = MemberLifeStyleDetails.tblMemberAdditionalLifeStyleDetails.ToList();
                        for (int i = 0; i < lstAdditionalInfo.Count(); i++)
                        {
                            lstAdditionalInfo[1].IsDeleted = true;
                            Entity.SaveChanges();
                        }

                    }
                }
            }
            catch (Exception ex)
            {


            }
        }


        public void UpdatePreviousInsuranceInfo(int LifeStyleMemberID)
        {

            try
            {
                using (AVOAIALifeEntities Entity = new AVOAIALifeEntities())
                {
                    foreach (var item in Entity.tblPolicyMemberInsuranceInfoes.Where(a => a.MemberID == LifeStyleMemberID).ToList())
                    {
                        item.IsDeleted = true;
                        Entity.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        public void UpdateQuestionDetailsInfo(int QuestionsMemberID)
        {

            try
            {
                using (AVOAIALifeEntities Entity = new AVOAIALifeEntities())
                {
                    foreach (var item in Entity.tblQuestionDetails.Where(a => a.MemberID == QuestionsMemberID).ToList())
                    {
                        item.IsDeleted = true;
                        Entity.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }


        public void UpdatePreviousClaimInfo(decimal LifeStyleMemberID)
        {
            try
            {
                using (AVOAIALifeEntities Entity = new AVOAIALifeEntities())
                {
                    foreach (var item in Entity.tblPolicyMemberClaimInfoes.Where(a => a.MemberID == LifeStyleMemberID).ToList())
                    {
                        item.IsDeleted = true;
                        Entity.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void UpdateNomineeDetails(decimal PolicyID)
        {

            try
            {
                using (AVOAIALifeEntities Entity = new AVOAIALifeEntities())
                {
                    foreach (var item in Entity.tblPolicyNomineeDetails.Where(a => a.PolicyID == PolicyID).ToList())
                    {
                        item.NomineeAddress = "";
                        item.IsDeleted = true;
                        Entity.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        public tblCustomer FillCustomerInfo(MemberDetails objMemberDetails, tblCustomer objtblcustomer)
        {
            objtblcustomer.CompanyName = objMemberDetails.CompanyName;
            objtblcustomer.DateOfBirth = objMemberDetails.DateOfBirth;
            objtblcustomer.EmailID = objMemberDetails.Email;
            objtblcustomer.FullName = objMemberDetails.FullName;
            objtblcustomer.Gender = objMemberDetails.Gender;
            objtblcustomer.HomeNo = objMemberDetails.HomeNumber;
            objtblcustomer.MaritalStatusID = GetCustomerMaritialStatus(objMemberDetails.MaritialStatus);
            //objtblcustomer.MaritalStatusID = Convert.ToInt32(objMemberDetails.MaritialStatus);
            objtblcustomer.MobileNo = objMemberDetails.MobileNo;
            objtblcustomer.MonthlyIncome = objMemberDetails.MonthlyIncome;
            objtblcustomer.NameWithInitials = objMemberDetails.NameWithInitial;
            //objtblcustomer.NationalityID = Convert.ToInt32(objMemberDetails.Nationality);
            objtblcustomer.NationalityID = GetSaveNationality(objMemberDetails.Nationality);
            objtblcustomer.NatureofDuties = objMemberDetails.NameOfDuties;
            objtblcustomer.NEWNICNO = objMemberDetails.NewNICNO;
            objtblcustomer.OLDNICNO = objMemberDetails.OLDNICNo;
            objtblcustomer.PreferredName = objMemberDetails.PrefferedName;
            objtblcustomer.Title = objMemberDetails.Salutation;
            objtblcustomer.WorkNo = objMemberDetails.WorkNumber;
            objtblcustomer.CreatedDate = DateTime.Now;
            objtblcustomer.Citizenship1 = objMemberDetails.Citizenship1;
            objtblcustomer.Citizenship2 = objMemberDetails.Citizenship2;
            objtblcustomer.ResidentialNationalityStatus = objMemberDetails.ResidentialStatus;
            objtblcustomer.ResidentialNationality = objMemberDetails.Residential;
            objtblcustomer.OccupationHazardousWork = objMemberDetails.OccupationHazardousWork;
            objtblcustomer.PassportNumber = objMemberDetails.PassportNumber;
            objtblcustomer.DrivingLicense = objMemberDetails.DrivingLicense;
            objtblcustomer.USTaxpayerId = objMemberDetails.USTaxpayerId;
            objtblcustomer.CountryOccupation = objMemberDetails.CountryofOccupation;
            return objtblcustomer;
        }

        public tblPolicy FillPolicyInfo(AIA.Life.Models.Policy.Policy ObjPolicy, tblPolicy objpolicy)
        {
            try
            {
                string userId = Common.CommonBusiness.GetUserId(ObjPolicy.UserName);
                objpolicy.Createdby = userId;
                objpolicy.PolicyTerm = ObjPolicy.PolicyTerm;
                objpolicy.PremiumTerm = ObjPolicy.PaymentTerm;
                objpolicy.PaymentFrequency = ObjPolicy.PaymentFrequency;
                objpolicy.PaymentMethod = ObjPolicy.PaymentMethod;
                objpolicy.PaymentPaidBy = ObjPolicy.PaymentPaidBy;
                objpolicy.Others = ObjPolicy.others;
                //PaymentReceiptPrefferdBy
                objpolicy.ProposalNo = ObjPolicy.ProposalNo;
                objpolicy.ModeOfCommunication = ObjPolicy.ModeOfCommunication;
                objpolicy.PreferredReceipt = ObjPolicy.PaymentReceiptPrefferdBy;
                objpolicy.PreferredLanguage = ObjPolicy.PreferredLanguage;

                objpolicy.MaturityBenefits = ObjPolicy.MaturityBenefits;
                objpolicy.Years = ObjPolicy.Years;
                objpolicy.SmartPensionReceivingPeriod = ObjPolicy.SmartPensionReceivingPeriod;
                objpolicy.SmartPensionMonthlyIncome = ObjPolicy.SmartPensionMonthlyIncome;
                objpolicy.AnnualPremium = ObjPolicy.TotalAnnualPremiumContribution;
                objpolicy.DepositPremium = ObjPolicy.ProposalDepositPremium;
                objpolicy.BankAccountNumber = ObjPolicy.BankAccountNo;
                objpolicy.BankBranchName = ObjPolicy.BranchName;
                objpolicy.CreditCardNo = ObjPolicy.CreditCardNo;
                objpolicy.BankName = ObjPolicy.BankName;

                //if (!string.IsNullOrEmpty(ObjPolicy.QuoteNo) && ObjPolicy.ProposalNo == null)
                //{
                //    objpolicy.ProposalNo = ObjPolicy.QuoteNo.Replace("Q", "P");
                //}
                //else
                //{
                objpolicy.ProposalNo = ObjPolicy.ProposalNo;
                //}
                objpolicy.QuoteNo = ObjPolicy.QuoteNo;
                objpolicy.PolicyVersion = 1;
                objpolicy.Createdby = (new Guid()).ToString();
                if(objpolicy.CreatedDate==DateTime.MinValue)
                    objpolicy.CreatedDate = DateTime.Now;
                objpolicy.ProposalSubmitDate = DateTime.Now;
                objpolicy.InceptionTime = DateTime.Now.TimeOfDay;
                //Temp Purpose
                DateTime today = DateTime.Now;
                int[] exceptionDays = new int[3] { 29, 30, 31 };
                if (exceptionDays.Contains(today.Day))
                    today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 28);
                ObjPolicy.RiskCommencementDate = today;
                objpolicy.PolicyIssueDate = ObjPolicy.RiskCommencementDate;
                objpolicy.PolicyStartDate = ObjPolicy.RiskCommencementDate;
                objpolicy.PolicyEndDate = ObjPolicy.RiskCommencementDate.AddYears(Convert.ToInt32(ObjPolicy.PolicyTerm));

                objpolicy.Deductible = ObjPolicy.Deductible;
                objpolicy.LeadNo = ObjPolicy.LeadNo;
                objpolicy.IntroducerCode = ObjPolicy.IntroducerCode;

                return objpolicy;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public int GetMaritialStatus(string Code)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            return Context.tblMasCommonTypes.Where(a => a.Code == Code).Select(a => a.CommonTypesID).FirstOrDefault();
        }

        public decimal GetCustomerMaritialStatus(string Code)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            return Context.tblMasCommonTypes.Where(a => a.Code == Code).Select(a => a.CommonTypesID).FirstOrDefault();
        }
        public int GetSaveNationality(string Code)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            return Context.tblMasCommonTypes.Where(a => a.Code == Code).Select(a => a.CommonTypesID).FirstOrDefault();
        }
        public int GetSaveOccupationID(int? ID)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            return Context.tblMasLifeOccupations.Where(a => a.ID == ID).Select(a => a.Code).ToString().FirstOrDefault();
        }
        public int GetFetchOccupationID(int? code)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            return Context.tblMasLifeOccupations.Where(a => a.Code == code).Select(a => a.ID).ToString().FirstOrDefault();
        }

        public tblPolicyClient FilltblpolicyClient(MemberDetails objMemberDetails, tblPolicyClient objPolicyClient)
        {
            try
            {

                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                objPolicyClient.ContactPerson = objMemberDetails.ContactPerson;
                objPolicyClient.Designation = objMemberDetails.Designation;
                objPolicyClient.BusinessRegistrationNo = objMemberDetails.BusinessRegistrationNo;

                objPolicyClient.FullName = objMemberDetails.RelationShipWithPropspect;
                objPolicyClient.CompanyName = objMemberDetails.CompanyName;
                objPolicyClient.CorporateName = objMemberDetails.CorporateName;
                objPolicyClient.ProposerEamilID = objMemberDetails.ProposerEmailID;
                objPolicyClient.ProposerTelepohoneNo = objMemberDetails.ProposerMobileNo;
                //objPolicyClient.DateOfBirth = objMemberDetails.DateOfBirth;
                objPolicyClient.DateOfBirth = objMemberDetails.DateOfBirth == null ? (string.IsNullOrEmpty(objMemberDetails.NewNICNO) == false ? Convert.ToDateTime(CommonBusiness.FetchDateMonth(objMemberDetails.NewNICNO)) : DateTime.Now) : objMemberDetails.DateOfBirth;
                objPolicyClient.Age = objMemberDetails.Age;
                objPolicyClient.EmailID = objMemberDetails.Email;
                objPolicyClient.FirstName = objMemberDetails.FirstName;
                objPolicyClient.MiddleName = objMemberDetails.MiddleName;
                objPolicyClient.LastName = objMemberDetails.LastName;
                string names = string.Empty;
                if (objMemberDetails.RelationShipWithPropspect != "CORP")
                {
                    var name = objMemberDetails.FirstName.Split(' ');
                    if (name != null)
                    {
                        foreach (var item in name)
                        {
                            if (item != "")
                            {
                                if (name.Count() > 1)
                                {
                                    names = names + " " + item[0];
                                }
                                else
                                {
                                    names = names + item[0];
                                }
                            }
                        }
                    }
                    objPolicyClient.NameWithInitials = names.TrimStart() + " " + objMemberDetails.LastName;
                }


                //objPolicyClient.NameWithInitials = objMemberDetails.NameWithInitial;
                objPolicyClient.Gender = objMemberDetails.Gender;
                string title = Context.tblMasCommonTypes.Where(a => a.Description == objMemberDetails.Salutation && a.MasterType == "Salutation").Select(b => b.Code).FirstOrDefault();
                if (string.IsNullOrEmpty(title))
                    objPolicyClient.Title = objMemberDetails.Salutation;
                else
                    objPolicyClient.Title = title;

                objPolicyClient.WorkNo = objMemberDetails.WorkNumber;
                objPolicyClient.HomeNo = objMemberDetails.HomeNumber;
                objPolicyClient.MaritalStatus = GetMaritialStatus(objMemberDetails.MaritialStatus);
                //objPolicyClient.MaritalStatus = objMemberDetails.MaritialStatus;

                //objPolicyClient.MaritalStatus = objMemberDetails.MaritalStatuslist
                //objPolicyClient.MaritalStatus = Convert.ToInt32(objMemberDetails.MaritialStatus);
                objPolicyClient.MobileNo = objMemberDetails.MobileNo;
                objPolicyClient.AlteranteMobileNO = objMemberDetails.OtherMobileNo;
                objPolicyClient.MonthlyIncome = objMemberDetails.MonthlyIncome;

                // objPolicyClient.Nationality = Convert.ToInt32(objMemberDetails.Nationality);
                objPolicyClient.Nationality = GetSaveNationality(objMemberDetails.Nationality);
                objPolicyClient.NatureOfDuties = objMemberDetails.NameOfDuties;
                objPolicyClient.NEWNICNo = objMemberDetails.NewNICNO;
                objPolicyClient.OLDNICNo = objMemberDetails.OLDNICNo;
                objPolicyClient.IsProposerAssured = objMemberDetails.IsproposerlifeAssured;
                objPolicyClient.OccupationID = objMemberDetails.OccupationID;
                //objPolicyClient.PreferredName = objMemberDetails.PrefferedName           
                objPolicyClient.CreatedDate = DateTime.Now;

                objPolicyClient.CountryOccupation = objMemberDetails.CountryofOccupation;
                objPolicyClient.CitizenShip = objMemberDetails.CitizenShip;
                objPolicyClient.Citizenship1 = objMemberDetails.Citizenship1;
                objPolicyClient.Citizenship2 = objMemberDetails.Citizenship2;
                objPolicyClient.ResidentialNationality = objMemberDetails.Residential;
                objPolicyClient.ResidentialNationalityStatus = objMemberDetails.ResidentialStatus;
                objPolicyClient.USTaxpayerId = objMemberDetails.USTaxpayerId;
                objPolicyClient.OccupationHazardousWork = objMemberDetails.OccupationHazardousWork;
                objPolicyClient.SpecifyHazardousWork = objMemberDetails.SpecifiyOccupationHazardousWork;
                objPolicyClient.PassportNumber = objMemberDetails.PassportNumber;
                objPolicyClient.DrivingLicense = objMemberDetails.DrivingLicense;


                //objPolicyClient.CountryofOccupation = objMemberDetails.CountryofOccupation; 

                #region FillAddressDetails
                tblAddress objtbladdress = objPolicyClient.tblAddress;
                if (objtbladdress == null)
                {
                    objtbladdress = new tblAddress();
                }
                objtbladdress = FillAddressDetails(objMemberDetails.objCommunicationAddress, objtbladdress);
                objPolicyClient.tblAddress = objtbladdress;
                if (objMemberDetails.IsRegAsCommunication)
                {
                    objPolicyClient.IsPermanentAddrSameasCommAddr = true;
                    objPolicyClient.tblAddress1 = objtbladdress;
                }
                else
                {
                    tblAddress objtblPermanentaddress = objPolicyClient.tblAddress;
                    if (objtblPermanentaddress == null)
                    {
                        objtblPermanentaddress = new tblAddress();
                    }
                    if (objMemberDetails.RelationShipWithPropspect != "CORP")
                    {
                        objtblPermanentaddress = FillAddressDetails(objMemberDetails.objPermenantAddress, objtblPermanentaddress);
                    }

                    objPolicyClient.tblAddress1 = objtblPermanentaddress;
                }
                #endregion

                return objPolicyClient;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public tblAddress FillAddressDetails(Address objAddress, tblAddress objtblAddress)
        {
            try
            {
                if (objAddress.Pincode != null)
                {
                    string[] PinCity = objAddress.Pincode.Split('|');
                    string pin = PinCity[0];
                    string City = PinCity[1];
                    objtblAddress.Address1 = objAddress.Address1;
                    objtblAddress.Address2 = objAddress.Address2;
                    objtblAddress.Address3 = objAddress.Address3;
                    objtblAddress.City = City;
                    objtblAddress.District = objAddress.District;
                    objtblAddress.State = objAddress.State;
                    objtblAddress.Pincode = pin;
                }
                return objtblAddress;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public Address FetchAddressDetails(tblAddress objtblAddress)
        {

            Address objAddress = new Address();
            objAddress.Address1 = objtblAddress.Address1;
            objAddress.Address2 = objtblAddress.Address2;
            objAddress.Address3 = objtblAddress.Address3;
            objAddress.Pincode = objtblAddress.Pincode + "|" + objtblAddress.City;
            objAddress.PincodeNew = objtblAddress.Pincode;
            objAddress.City = objtblAddress.City;
            objAddress.District = objtblAddress.District;
            objAddress.State = objtblAddress.State;
            objAddress.Province = objtblAddress.State;
            objAddress.Country = objtblAddress.Country;
            return objAddress;
        }

        public List<MasterListItem> GetDropDownValuesForQuestions(string MasterType)
        {
            Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
            List<MasterListItem> objList = new List<MasterListItem>();
            if (!string.IsNullOrEmpty(MasterType))
            {
                objList = objCommonBusiness.GetMasCommonTypeMasterListItem(MasterType);
            }

            if (objList == null)
            {
                objList = new List<MasterListItem>();
                objList.Add(new MasterListItem { ID = 1, Value = "1", Text = "1" });
                objList.Add(new MasterListItem { ID = 2, Value = "2", Text = "2" });
            }
            return objList;
        }
        public List<QuestionsList> GetMasQuestions(AVOAIALifeEntities Context, string qType, int? parentID = null, int? ProductID = null, int? OccupationID = 0, string Gender = null, decimal memberId = 0, string ResidentialStatus = null, bool OccupationHazardousWork = false, string FAL = null, string QForm = null) //,
        {
            //string NationalityQuestion = Convert.ToString(ResidentialStatus);
            var list = (from Question in Context.tblMasLifeQuestionnaires
                        where Question.QType == qType && Question.ParentID == parentID && Question.IsDeleted != true && Question.Value == "true" && Question.ProductID == ProductID && (Question.OccupationID == null || Question.OccupationID == OccupationID) && (Question.Gender == null || Question.Gender == Gender) && (Question.ResidentialStatus == null || Question.ResidentialStatus == ResidentialStatus) && (Question.OccupationHazardouswork == null || Question.OccupationHazardouswork == OccupationHazardousWork) && (Question.PAQQuestionsID == null || Question.PAQQuestionsID == FAL) //&& (Question.QForm == null || Question.QForm == QForm)
                        select Question
                        ).ToList();
            List<QuestionsList> lstQuestionlist = new List<QuestionsList>();

            foreach (var item in list)
            {
                QuestionsList objQues = new QuestionsList();
                objQues.QuestionID = item.QId;
                objQues.QuestionText = item.QText;
                objQues.QuestionType = item.QType;
                objQues.ControlType = item.ControlType;
                objQues.Gender = item.Gender;
                objQues.Value = item.Value;
                objQues.Answer = Context.tblMemberQuestions.Where(a => a.MemberID == memberId && a.QID == item.QId).Select(a => a.Answer).FirstOrDefault();
                objQues.SubControlType = item.SubControlType;
                objQues.LstQuestionsTypes = GetMasQuestions(Context, objQues.QuestionType, item.QId, item.ProductID, item.OccupationID, item.Gender, memberId, item.ResidentialStatus, Convert.ToBoolean(item.OccupationHazardouswork), item.PAQQuestionsID);//,item.QForm
                objQues.LstDropDownvalues = GetDropDownValuesForQuestions(item.Master);
                //objQues.QuestionIndex = (item.SequenceNo);
                lstQuestionlist.Add(objQues);
            }
            return lstQuestionlist;
        }

        public MemberDetails MemberDetailsForSelfAndSpouse(MemberDetails objMember, string Plancode, string QuoteNo = null)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();

            #region Life Style Questions 
            if (Plancode == "1")
            {
                objMember.LstEasyPensionQuestions = GetMasQuestions(Context, "LifeStyle", null, Convert.ToInt32(Plancode), null, null); //
                objMember.Questions = GetMasQuestions(Context, "LifeStyle", null, null, null, null); //Convert.ToInt32(Plancode)
            }
            else
            {
                objMember.Questions = GetMasQuestions(Context, "LifeStyle", null, null, null, null);
            }

            #endregion

            #region Medical History Questions
            if (Plancode == "1")
            {
                objMember.objLstMedicalHistory = GetMasQuestions(Context, "MedicalHistory", null, Convert.ToInt32(Plancode), null, objMember.Gender);
            }
            //else if (Plancode == "77" || Plancode == "78")
            //{
            //    var NMLCount = 0; //SAR <= NML 
            //    var result = Context.usp_GetDocumentsForPDF(QuoteNo).ToList();
            //    if (result.Count == 0)
            //    {
            //    }
            //    else
            //    {
            //        foreach (var item in result)
            //        {
            //            if (!String.IsNullOrEmpty(item.MedicalDoc))
            //            {
            //                NMLCount++;//SAR > NML
            //            }
            //        }
            //    }
            //    var PlanCode = Convert.ToInt64(Plancode);
            //    var ProductID = Context.tblMasProductPlans.Where(a => a.PlanId == PlanCode).Select(a => a.ProductId).FirstOrDefault();
            //    var MinSAM = Context.tblMasSAMs.Where(a => a.PlanId == PlanCode).Where(a=>a.MinAge <= objMember.Age).Where(a=>a.MixAge >= objMember.Age).Select(a => a.MinSAM).Min();
            //    var SAM = Context.tblLifeQQs.Where(a => a.QuoteNo == QuoteNo).Select(a => a.SAM).FirstOrDefault();
            //    if (Convert.ToInt32(PlanCode) == 77)
            //    {
            //        if (Convert.ToInt32(SAM) == MinSAM && NMLCount == 0)
            //        {
            //            objMember.objLstMedicalHistory = GetMasQuestions(Context, "MedicalHistory", null, Convert.ToInt32(ProductID), null, null, objMember.MemberID, null, false, null, "Short");
            //        }
            //        else if (Convert.ToInt32(SAM) > MinSAM || NMLCount != 0)
            //        {
            //            objMember.objLstMedicalHistory = GetMasQuestions(Context, "MedicalHistory", null, Convert.ToInt32(ProductID), null, null, objMember.MemberID, null, false, null, "Long");
            //        }
            //    }
            //    else
            //    {
            //        if (Convert.ToInt32(SAM) == MinSAM && NMLCount == 0)
            //        {
            //            objMember.objLstMedicalHistory = GetMasQuestions(Context, "MedicalHistory", null, Convert.ToInt32(ProductID), null, null, objMember.MemberID, null, false, null, "Short");
            //        }
            //        else if (Convert.ToInt32(SAM) > MinSAM || NMLCount != 0)
            //        {
            //            objMember.objLstMedicalHistory = GetMasQuestions(Context, "MedicalHistory", null, Convert.ToInt32(ProductID), null, null, objMember.MemberID, null, false, null, "Long");
            //        }
            //    }

            //}
            else
            {
                objMember.objLstMedicalHistory = GetMasQuestions(Context, "MedicalHistory", null, null, null, objMember.Gender);
            }


            #endregion

            #region  State Health Questionnaires


            objMember.objLststateofhelath = GetMasQuestions(Context, "MedicalHistory", null, null, null, objMember.Gender);


            #endregion

            #region Family Back Ground Questions
            objMember.objLstFamily = GetMasQuestions(Context, "FamilyBackGround", null, null, null, null);
            #endregion

            #region  Additional Questionnaires

            //objMember.objAdditionalQuestions = GetMasQuestions(Context, "AdditionalQuestions", null);

            objMember.objAdditionalQuestions = GetMasQuestions(Context, "AdditionalQuestions", null, null, null, null, objMember.MemberID, null, Convert.ToBoolean(null), null);

            #endregion

            #region  Previous And Current Life Insurance Questions
            objMember.objLstOtherInsuranceDetails = GetMasQuestions(Context, "PreviousAndCurrentLifeInsurance", null, null, null, null);
            #endregion

            objMember.objLifeAssuredOtherInsurance = new List<LifeAssuredOtherInsurance>();

            return objMember;
        }

        public AIA.Life.Models.Policy.Policy OccupationQuestions(AIA.Life.Models.Policy.Policy objProposal)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            if (objProposal.objMemberDetails != null)
            {
                for (int i = 0; i < objProposal.objMemberDetails.Count(); i++)
                {
                    objProposal.objMemberDetails[i] = MemberDetailsForSelfAndSpouse(objProposal.objMemberDetails[i], null);
                    //objProposal.objMemberDetails[i].objAdditionalQuestions = GetMasQuestions(Context, "AdditionalQuestions", null, null, objProposal.objMemberDetails[i].OccupationID, null, objProposal.objMemberDetails[i].MemberID, null);
                }
            }
            return objProposal;
        }

        public AIA.Life.Models.Policy.Policy ResidentialStatusQuestions(AIA.Life.Models.Policy.Policy objProposal)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            if (objProposal.objMemberDetails != null)
            {
                for (int i = 0; i < objProposal.objMemberDetails.Count(); i++)
                {
                    string FALValue = "";
                    string NSRLValue = "";
                    if (objProposal.objMemberDetails[i].SAR >= 5000000)
                    {
                        FALValue = "5000000.0";
                    }
                    if (objProposal.objMemberDetails[i].Nationality == "SL" && objProposal.objMemberDetails[i].ResidentialStatus == "SL")
                    {
                        NSRLValue = "SL";
                    }
                    else if (objProposal.objMemberDetails[i].Nationality == "SL" && objProposal.objMemberDetails[i].CountryofOccupation != "SL")
                    {
                        NSRLValue = "SL";
                    }
                    else
                    {
                        NSRLValue = null;
                    }
                    objProposal.lstPAQAssets = (from CommonType in Context.tblMasCommonTypes.Where(a => a.MasterType == "PAQAssets")
                                                select new MasterListItem
                                                {
                                                    ID = CommonType.CommonTypesID,
                                                    Text = CommonType.Description
                                                }).ToList();
                    objProposal.lstPAQLiabilities = (from CommonType in Context.tblMasCommonTypes.Where(a => a.MasterType == "PAQLiabilities")
                                                     select new MasterListItem
                                                     {
                                                         ID = CommonType.CommonTypesID,
                                                         Text = CommonType.Description
                                                     }).ToList();

                    objProposal.objMemberDetails[i].objAdditionalQuestions = GetMasQuestions(Context, "AdditionalQuestions", null, null, objProposal.objMemberDetails[i].OccupationID, null, objProposal.objMemberDetails[i].MemberID, NSRLValue, Convert.ToBoolean(objProposal.objMemberDetails[i].OccupationHazardousWork), FALValue);
                }
            }
            return objProposal;
        }

        public MemberDetails MemberDetailsForChild(MemberDetails objMember)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();

            //MedicalHistory
            //objMember.objLstMedicalHistory = new List<MedicalHistoryQuestions>();
            if (objMember.RelationShipWithPropspect == "270")
            {
                #region Medical History Questions

                objMember.objLstMedicalHistory = GetMasQuestions(Context, "MedicalHistory", null, null, null, objMember.Gender = "C").Where(a => a.Gender == "C").ToList();

                #endregion

            }
            else
            {
                #region Medical History Questions

                objMember.objLstMedicalHistory = GetMasQuestions(Context, "MedicalHistory", null, null, null, objMember.Gender = "C").Where(a => a.Gender == "C").ToList();

                #endregion
            }


            //AdditionalQuestions


            // Prev insurance
            objMember.objLifeAssuredOtherInsurance = new List<LifeAssuredOtherInsurance>();

            return objMember;
        }

        public AIA.Life.Models.Policy.Policy AssuredMemberDetails(AIA.Life.Models.Policy.Policy ObjPolicy)
        {

            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            ObjPolicy.ListAssured = new List<string>();
            List<AssuredRelation> objLstAssuredRelation = new List<AssuredRelation>();
            int ChildCount = 0;
            for (int i = 0; i < ObjPolicy.objMemberDetails.Count(); i++)
            {
                if (ObjPolicy.objMemberDetails[i].RelationShipWithPropspect == "267")
                {
                    #region LifeAssured Details
                    if (ObjPolicy.objProspectDetails.IsSelfIsMainLife)
                    {
                        ObjPolicy.objMemberDetails[i].AssuredName = "MainLife";
                        ObjPolicy.ListAssured.Add("MainLife");
                        AssuredRelation objAssuredRelation = new AssuredRelation();
                        objAssuredRelation.Member_Relationship = ObjPolicy.objMemberDetails[i].RelationShipWithPropspect;
                        objAssuredRelation.Assured_Name = "MainLife";
                        objLstAssuredRelation.Add(objAssuredRelation);
                    }
                    else
                    {
                        ObjPolicy.objMemberDetails[i].AssuredName = "Spouse";
                        ObjPolicy.ListAssured.Add("Spouse");
                        AssuredRelation objAssuredRelation = new AssuredRelation();
                        objAssuredRelation.Member_Relationship = ObjPolicy.objMemberDetails[i].RelationShipWithPropspect;
                        objAssuredRelation.Assured_Name = "Spouse";
                        objLstAssuredRelation.Add(objAssuredRelation);
                    }

                    #endregion
                    ObjPolicy.objMemberDetails[i] = MemberDetailsForSelfAndSpouse(ObjPolicy.objMemberDetails[i], null);
                }
                else if (ObjPolicy.objMemberDetails[i].RelationShipWithPropspect == "268")
                {
                    #region LifeAssured Details
                    if (!ObjPolicy.objProspectDetails.IsSelfIsMainLife)
                    {
                        ObjPolicy.objMemberDetails[i].AssuredName = "MainLife";
                        ObjPolicy.ListAssured.Add("MainLife");
                        AssuredRelation objAssuredRelation = new AssuredRelation();
                        objAssuredRelation.Member_Relationship = ObjPolicy.objMemberDetails[i].RelationShipWithPropspect;
                        objAssuredRelation.Assured_Name = "MainLife";
                        objLstAssuredRelation.Add(objAssuredRelation);
                    }
                    else
                    {
                        ObjPolicy.objMemberDetails[i].AssuredName = "Spouse";
                        ObjPolicy.ListAssured.Add("Spouse");
                        AssuredRelation objAssuredRelation = new AssuredRelation();
                        objAssuredRelation.Member_Relationship = ObjPolicy.objMemberDetails[i].RelationShipWithPropspect;
                        objAssuredRelation.Assured_Name = "Spouse";
                        objLstAssuredRelation.Add(objAssuredRelation);
                    }
                    #endregion
                    ObjPolicy.objMemberDetails[i] = MemberDetailsForSelfAndSpouse(ObjPolicy.objMemberDetails[i], null);
                }
                else
                {
                    ChildCount++;
                    ObjPolicy.objMemberDetails[i] = MemberDetailsForChild(ObjPolicy.objMemberDetails[i]);
                    ObjPolicy.objMemberDetails[i].AssuredName = "Child" + ChildCount;
                    ObjPolicy.ListAssured.Add("Child" + ChildCount);
                    AssuredRelation objAssuredRelation = new AssuredRelation();
                    objAssuredRelation.Member_Relationship = ObjPolicy.objMemberDetails[i].RelationShipWithPropspect;
                    objAssuredRelation.Assured_Name = "Child" + ChildCount;
                    objLstAssuredRelation.Add(objAssuredRelation);
                }
            }
            CommonBusiness objCommon = new CommonBusiness();
            ObjPolicy.lstCauseOfDeath = objCommon.GetMasCommonTypeMasterListItem("CauseOfDeathCase");
            ObjPolicy.lstSateofHealth = objCommon.GetMasCommonTypeMasterListItem("StateOfHealth");
            ObjPolicy.lstFamilyBackGroundRelationship = objCommon.GetMasCommonTypeMasterListItem("FamilyBackGroundRelationship");

            List<AIA.Life.Models.Common.BenifitDetails> lstBebefit = new List<AIA.Life.Models.Common.BenifitDetails>();
            lstBebefit = (from Benefit in Context.tblMasBenefits.Where(a => a.IsDeleted != true)
                          select new AIA.Life.Models.Common.BenifitDetails()
                          {
                              BenifitName = Benefit.BenefitName,
                              BenefitID = Benefit.BenefitID,
                              BenifitOpted = false

                          }).ToList().Select(x => new AIA.Life.Models.Common.BenifitDetails
                          {

                              BenifitName = x.BenifitName,
                              BenefitID = x.BenefitID,
                              BenifitOpted = x.BenifitOpted,
                              objBenefitMemberRelationship = objLstAssuredRelation
                          }).ToList();
            ObjPolicy.LstBenifitDetails = lstBebefit;


            return ObjPolicy;
        }

        public AIA.Life.Models.Policy.Policy LoadProposalInfo(AIA.Life.Models.Policy.Policy ObjPolicy)
        {
            try
            {

                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
                string FAL = "0";
                string SAR = "0";
                //string AFC = "0";
                // Added to Avoid Duplicate
                var PolicyDetails = Context.tblPolicies.Where(a => a.QuoteNo == ObjPolicy.QuoteNo).FirstOrDefault();
                if (PolicyDetails != null)
                {
                    ObjPolicy.PolicyID = PolicyDetails.PolicyID;
                    return ObjPolicy;
                }
                // Till here
                var tbllifeQQ = Context.tblLifeQQs.Where(a => a.QuoteNo == ObjPolicy.QuoteNo).FirstOrDefault();
                if (tbllifeQQ != null)
                {
                    ObjPolicy.ProposerDate = DateTime.Now;
                    ObjPolicy.SpouseDate = DateTime.Now;

                    ObjPolicy.ContactID = tbllifeQQ.ContactID;
                    if (!string.IsNullOrEmpty(tbllifeQQ.MaturityBenifits))
                    {

                        ObjPolicy.MaturityBenefits = tbllifeQQ.MaturityBenifits;
                        if (ObjPolicy.MaturityBenefits == "2380")
                        {
                            ObjPolicy.Years = Convert.ToString(tbllifeQQ.DrawDownPeriod);
                        }

                    }
                    ObjPolicy.IsAfc = tbllifeQQ.IsAfc;
                    ObjPolicy.PolicyTerm = Convert.ToString(tbllifeQQ.PolicyTermID);
                    ObjPolicy.PaymentTerm = Convert.ToString(tbllifeQQ.PremiumTerm);
                    ObjPolicy.PlanName = Context.tblProducts.Where(a => a.ProductId == tbllifeQQ.ProductNameID).FirstOrDefault().ProductName;
                    ObjPolicy.PlanCode = Convert.ToString(tbllifeQQ.PlanCode);
                    ObjPolicy.ProductID = Convert.ToInt32(tbllifeQQ.ProductNameID);
                    ObjPolicy.PaymentFrequency = tbllifeQQ.PreferredTerm;
                    ObjPolicy.SmartPensionMonthlyIncome = Convert.ToString(tbllifeQQ.MonthlySurvivorIncome);
                    ObjPolicy.SmartPensionReceivingPeriod = Convert.ToString((tbllifeQQ.PensionPeriod == null || tbllifeQQ.PensionPeriod == 0) ? tbllifeQQ.DrawDownPeriod : tbllifeQQ.PensionPeriod);
                    ObjPolicy.Deductible = tbllifeQQ.Deductable ?? false;
                    ObjPolicy.PaymentMethod = "2085";
                    ObjPolicy.PaymentPaidBy = "267";

                    //ObjPolicy.ProposalNeed = tbllifeQQ.QType;
                    var tblcontact = Context.tblContacts.Where(a => a.ContactID == tbllifeQQ.ContactID).FirstOrDefault();
                    ObjPolicy.objProspectDetails = new MemberDetails();
                    ObjPolicy.objProspectDetails.ClientCode = tblcontact.ClientCode;
                    ObjPolicy.objProspectDetails.SAM = Convert.ToString(tbllifeQQ.SAM);
                    ObjPolicy.objProspectDetails.FirstName = tblcontact.FirstName;
                    ObjPolicy.objProspectDetails.LastName = tblcontact.LastName;
                    string role = Context.usp_GetCurrentUserRole(ObjPolicy.UserName).FirstOrDefault();
                    if (role == "FPC-Banca")
                        ObjPolicy.LeadNo = "B" + tblcontact.LeadNo;
                    else
                        ObjPolicy.LeadNo = "A" + tblcontact.LeadNo;
                    ObjPolicy.IntroducerCode = tblcontact.IntroducerCode;
                    //ObjPolicy.objProspectDetails.NameWithInitial = tblcontact.FirstName[0] + " " + tblcontact.LastName;
                    string names = string.Empty;
                    var name = tblcontact.FirstName.Split(' ');
                    foreach (var item in name)
                    {
                        if (item != "")
                        {
                            if (name.Count() > 1)
                            {
                                names = names + " " + item[0];
                            }
                            else
                            {
                                names = names + item[0];
                            }
                        }
                    }
                    ObjPolicy.objProspectDetails.NameWithInitial = names.TrimStart() + " " + tblcontact.LastName;
                    ObjPolicy.objProspectDetails.Email = tblcontact.EmailID;
                    ObjPolicy.objProspectDetails.MobileNo = tblcontact.MobileNo;
                    ObjPolicy.objProspectDetails.HomeNumber = tblcontact.PhoneNo;
                    ObjPolicy.objProspectDetails.WorkNumber = tblcontact.Work;
                    ObjPolicy.objProspectDetails.Age = Convert.ToInt32(tblcontact.Age);
                    ObjPolicy.objProspectDetails.DateOfBirth = Convert.ToDateTime(tblcontact.DateOfBirth);
                    ObjPolicy.objProspectDetails.OccupationID = Convert.ToInt32(tblcontact.OccupationID);
                    ObjPolicy.objProspectDetails.NewNICNO = tblcontact.NICNO;
                    ObjPolicy.objProspectDetails.MaritialStatus = Context.tblMasCommonTypes.Where(a => a.CommonTypesID == tblcontact.MaritalStatusID && a.MasterType == "MaritalStatus").Select(a => a.Code).FirstOrDefault();
                    //ObjPolicy.objProspectDetails.MaritialStatus = Convert.ToString(tblcontact.MaritalStatusID);//Context.tblMasCommonTypes.Where(a => a.CommonTypesID == tblcontact.MaritalStatusID && a.MasterType == "MaritalStatus").Select(a => a.CommonTypesID).FirstOrDefault();
                    //ObjPolicy.objProspectDetails.MaritialStatus=GetCustomerMaritialStatus(tblcontact.MaritalStatusID).ToString();
                    ObjPolicy.objProspectDetails.Gender = tblcontact.Gender;
                    ObjPolicy.objProspectDetails.MonthlyIncome = tblcontact.MonthlyIncome;
                    ObjPolicy.objProspectDetails.Salutation = Context.tblMasCommonTypes.Where(a => a.Code == tblcontact.Title).Select(b => b.Description).FirstOrDefault();
                    ObjPolicy.objProspectDetails.SalutationCode = tblcontact.Title;
                    ObjPolicy.objProspectDetails.CompanyName = tblcontact.Employer;
                    ObjPolicy.objProspectDetails.PassportNumber = tblcontact.PassportNo;
                    ObjPolicy.objProspectDetails.ResidentialStatus = "SL";
                    ObjPolicy.objProspectDetails.Nationality = "SL";
                    ObjPolicy.objProspectDetails.CountryofOccupation = "SL";

                    ObjPolicy.objProspectDetails.IsSelfIsMainLife = true;
                    //ObjPolicy.objProspectDetails.lstAvgMonthlyIncome = objCommonBusiness.GetMasCommonTypeMasterListItem("MonthlyIncomeRange");
                    if (tblcontact.tblAddress != null)
                    {
                        if (ObjPolicy.objProspectDetails.objCommunicationAddress == null)
                        {
                            ObjPolicy.objProspectDetails.objCommunicationAddress = new AIA.Life.Models.Common.Address();
                        }
                        if (ObjPolicy.objProspectDetails.objPermenantAddress == null)
                        {
                            ObjPolicy.objProspectDetails.objPermenantAddress = new AIA.Life.Models.Common.Address();
                        }
                        ObjPolicy.objProspectDetails.objCommunicationAddress.Address1 = tblcontact.tblAddress.Address1;
                        ObjPolicy.objProspectDetails.objCommunicationAddress.Address2 = tblcontact.tblAddress.Address2;
                        ObjPolicy.objProspectDetails.objCommunicationAddress.Address3 = tblcontact.tblAddress.Address3;
                        ObjPolicy.objProspectDetails.objCommunicationAddress.Pincode = tblcontact.tblAddress.Pincode + "|" + tblcontact.tblAddress.City;
                        ObjPolicy.objProspectDetails.objCommunicationAddress.PincodeNew = tblcontact.tblAddress.Pincode;
                        ObjPolicy.objProspectDetails.objCommunicationAddress.City = tblcontact.tblAddress.City;
                        ObjPolicy.objProspectDetails.objCommunicationAddress.Province = tblcontact.tblAddress.State;
                        ObjPolicy.objProspectDetails.objCommunicationAddress.State = tblcontact.tblAddress.State;
                        ObjPolicy.objProspectDetails.objCommunicationAddress.District = tblcontact.tblAddress.District;
                        ObjPolicy.objProspectDetails.objCommunicationAddress.Country = tblcontact.tblAddress.Country;
                        //Added LstMaturityBenefits
                        ObjPolicy.LstMaturityBenefits = objCommonBusiness.GetMaturityBenefits();
                        ObjPolicy.LstResidentialStatus = objCommonBusiness.GetResidentialStatus();
                        ObjPolicy.LstFillMemberCountryofOccupation = objCommonBusiness.GetCountryofOccupation();


                    }
                    int MemberIndex = 0;
                    #region Get List of Distinct Riders For the Given Quote

                    //   lstBenefit

                    List<BenifitDetails> lstBenefit = (from QuoteMember in Context.tblQuoteMemberDetials.Where(a => a.LifeQQID == tbllifeQQ.LifeQQID && a.IsDeleted != true)
                                                       join MemberBenefit in Context.tblQuoteMemberBeniftDetials
                                                       on QuoteMember.MemberID equals MemberBenefit.MemberID
                                                       join MasBenefit in Context.tblProductPlanRiders
                                                       on MemberBenefit.BenifitID equals MasBenefit.ProductPlanRiderId
                                                       select new AIA.Life.Models.Common.BenifitDetails
                                                       {
                                                           BenifitName = MasBenefit.DisplayName,
                                                           BenefitID = MasBenefit.ProductPlanRiderId,
                                                           ActualRiderPremium = MemberBenefit.ActualPremium.ToString(),
                                                           LoadingAmount = MemberBenefit.LoadingAmount.ToString()

                                                       }).DistinctBy(a => a.BenefitID).ToList();

                    //  List<BenifitDetails> BenefitOverView = new List<BenifitDetails>();

                    #endregion

                    #region  Member Details
                    ObjPolicy.objMemberDetails = new List<MemberDetails>();
                    ObjPolicy.ListAssured = new List<string>();

                    List<MemberDetails> objMemberDetails = (from QuoteMember in Context.tblQuoteMemberDetials.Where(a => a.LifeQQID == tbllifeQQ.LifeQQID && a.IsDeleted != true)
                                                            select new MemberDetails
                                                            {
                                                                RelationShipWithPropspect = QuoteMember.Relationship,
                                                                RelationShipWithPropspectID = QuoteMember.Relationship,
                                                                DateOfBirth = QuoteMember.DateOfBirth ?? DateTime.Now,
                                                                FirstName = QuoteMember.Name,
                                                                Gender = QuoteMember.Gender,
                                                                GenderText = QuoteMember.Gender,
                                                                Age = QuoteMember.Age,
                                                                QuoteMemberID = QuoteMember.MemberID,
                                                                INTBasicSumInsured = QuoteMember.BasicSuminsured,
                                                                Basicpremium = QuoteMember.BasicPremium,
                                                                Memberpremium = QuoteMember.MemberPremium,
                                                                AssuredName = QuoteMember.AssuredName,
                                                                OccupationID = QuoteMember.OccupationID,
                                                                //BasicSumInsured = QuoteMember.BasicSuminsured.ToString(),
                                                                // OccupationID = GetFetchOccupationID(QuoteMember.OccupationID)
                                                                //,
                                                                //AssuredName =QuoteMember.AssuredName //WednusDay

                                                            }).ToList();

                    int ChildCount = 0;
                    // GetSARAndFALDetails(ref ObjPolicy.objProspectDetails, ObjPolicy.QuoteNo);
                    for (int i = 0; i < objMemberDetails.Count(); i++)
                    {

                        //objMemberDetails[i].OccupationID = Context.tblQuoteMemberDetials.Where(a => a.LifeQQID == tbllifeQQ.LifeQQID && a.IsDeleted != true).Select(a => a.OccupationID).FirstOrDefault();
                        objMemberDetails[i].BasicSumInsured = Convert.ToString(objMemberDetails[i].INTBasicSumInsured);
                        if (objMemberDetails[i].RelationShipWithPropspect == "267")
                        {
                            #region LifeAssured Details
                            var CurrentProposalSARAndFAL = Context.SP_GetSARAndFALDetailsForQuote(ObjPolicy.QuoteNo).ToList();
                            if (CurrentProposalSARAndFAL != null)
                            {
                                decimal QuoteMemberId = objMemberDetails[i].QuoteMemberID;
                                var MemberInfo = CurrentProposalSARAndFAL.Where(a => a.MemberID == QuoteMemberId).FirstOrDefault();
                                if (MemberInfo != null)
                                {
                                    objMemberDetails[i].SAR = Convert.ToDecimal(MemberInfo.SAR);
                                    objMemberDetails[i].FAL = Convert.ToDecimal(MemberInfo.FAL);
                                }
                            }

                            objMemberDetails[i].ClientCode = tblcontact.ClientCode;
                            objMemberDetails[i].FirstName = tblcontact.FirstName;
                            objMemberDetails[i].LastName = tblcontact.LastName;
                            objMemberDetails[i].Email = tblcontact.EmailID;
                            objMemberDetails[i].MobileNo = tblcontact.MobileNo;
                            // objMemberDetails[i].OtherMobileNo = tblcontact.Other;
                            objMemberDetails[i].HomeNumber = tblcontact.PhoneNo;
                            objMemberDetails[i].WorkNumber = tblcontact.Work;
                            objMemberDetails[i].Age = Convert.ToInt32(tblcontact.Age);
                            if (tblcontact.DateOfBirth != null)
                            {
                                objMemberDetails[i].DateOfBirth = tblcontact.DateOfBirth.Value.Date;
                            }

                            objMemberDetails[i].OccupationID = Convert.ToInt32(tblcontact.OccupationID);
                            objMemberDetails[i].NewNICNO = tblcontact.NICNO;
                            //objMemberDetails[i].MaritialStatus = Convert.ToString(tblcontact.MaritalStatusID);
                            objMemberDetails[i].MaritialStatus = GetLoadMaritialStatus(tblcontact.MaritalStatusID);
                            objMemberDetails[i].Gender = tblcontact.Gender;
                            objMemberDetails[i].MonthlyIncome = tblcontact.MonthlyIncome;
                            objMemberDetails[i].Salutation = tblcontact.Title;
                            objMemberDetails[i].CompanyName = tblcontact.Employer;
                            objMemberDetails[i].IsSelfIsMainLife = true;
                            if (objMemberDetails[i].objCommunicationAddress == null)
                            {
                                objMemberDetails[i].objCommunicationAddress = new AIA.Life.Models.Common.Address();
                            }
                            if (objMemberDetails[i].objPermenantAddress == null)
                            {
                                objMemberDetails[i].objPermenantAddress = new AIA.Life.Models.Common.Address();
                            }
                            objMemberDetails[i].objCommunicationAddress.Address1 = tblcontact.tblAddress.Address1;
                            objMemberDetails[i].objCommunicationAddress.Address2 = tblcontact.tblAddress.Address2;
                            objMemberDetails[i].objCommunicationAddress.Address3 = tblcontact.tblAddress.Address3;
                            objMemberDetails[i].objCommunicationAddress.State = tblcontact.tblAddress.State;
                            objMemberDetails[i].objCommunicationAddress.StateID = tblcontact.tblAddress.StateID;
                            objMemberDetails[i].objCommunicationAddress.Pincode = tblcontact.tblAddress.Pincode + "|" + tblcontact.tblAddress.City;
                            objMemberDetails[i].objCommunicationAddress.City = tblcontact.tblAddress.City;

                            objMemberDetails[i].objCommunicationAddress.Province = tblcontact.tblAddress.State;
                            objMemberDetails[i].objCommunicationAddress.District = tblcontact.tblAddress.District;
                            objMemberDetails[i].objCommunicationAddress.Country = tblcontact.tblAddress.Country;
                            if (objMemberDetails[i].objPermenantAddress == null)
                            {
                                objMemberDetails[i].objPermenantAddress = new AIA.Life.Models.Common.Address();
                            }
                            if (ObjPolicy.objProspectDetails.IsSelfIsMainLife)
                            {
                                objMemberDetails[i].AssuredName = "MainLife";
                                ObjPolicy.ListAssured.Add("MainLife");

                            }
                            else
                            {
                                objMemberDetails[i].AssuredName = "Spouse";
                                ObjPolicy.ListAssured.Add("Spouse");

                            }

                            var PlanID = Context.tblMasProductPlans.Where(a => a.PlanCode == ObjPolicy.PlanCode).Select(a => a.PlanId).FirstOrDefault();
                            #endregion
                            if (ObjPolicy.ProductID == 1)
                            {
                                objMemberDetails[i] = MemberDetailsForSelfAndSpouse(objMemberDetails[i], Convert.ToString(ObjPolicy.ProductID));
                            }
                            //else if (PlanID == 77 || PlanID == 78)
                            //{
                            //    objMemberDetails[i] = MemberDetailsForSelfAndSpouse(objMemberDetails[i], Convert.ToString(PlanID), ObjPolicy.QuoteNo);
                            //}

                            else
                            {
                                objMemberDetails[i] = MemberDetailsForSelfAndSpouse(objMemberDetails[i], null);
                            }

                            #region  WealthPlannerQuestions
                            objMemberDetails[0].objLstWealthPlannerQuestions = GetMasQuestions(Context, "WealthPlannerQuestions", null, null, null, null);
                            #endregion

                            objMemberDetails[i].Index = i;

                            //ObjPolicy.objMemberDetails.Add(item);

                            for (int j = 0; j < lstBenefit.Count(); j++)
                            {
                                AssuredRelation objAssuredRelation = new AssuredRelation();
                                objAssuredRelation.Assured_Name = objMemberDetails[i].AssuredName;
                                objAssuredRelation.Member_Relationship = objMemberDetails[i].RelationShipWithPropspect;
                                lstBenefit[j].objBenefitMemberRelationship.Add(objAssuredRelation);

                            }
                            FillRiderDetails(objMemberDetails[i].QuoteMemberID, objMemberDetails[i].AssuredName, objMemberDetails[i], ref lstBenefit);


                        }
                        else if (objMemberDetails[i].RelationShipWithPropspect == "268")
                        {
                            #region LifeAssured Details

                            //if (!ObjPolicy.objProspectDetails.IsSelfIsMainLife)
                            //{
                            //    objMemberDetails[i].AssuredName = "MainLife";
                            //    ObjPolicy.ListAssured.Add("MainLife");
                            //}
                            //else
                            //{
                            //    objMemberDetails[i].AssuredName = "Spouse";

                            //    ObjPolicy.ListAssured.Add("Spouse");
                            //    ObjPolicy.IsSpouseCovered = true;
                            //}

                            #endregion
                            var CurrentProposalSARAndFAL = Context.SP_GetSARAndFALDetailsForQuote(ObjPolicy.QuoteNo).ToList();
                            if (CurrentProposalSARAndFAL != null)
                            {
                                decimal QuoteMemberId = objMemberDetails[i].QuoteMemberID;
                                var MemberInfo = CurrentProposalSARAndFAL.Where(a => a.MemberID == QuoteMemberId).FirstOrDefault();
                                if (MemberInfo != null)
                                {
                                    objMemberDetails[i].SAR = Convert.ToDecimal(MemberInfo.SAR);
                                    objMemberDetails[i].FAL = Convert.ToDecimal(MemberInfo.FAL);
                                }
                            }

                            #region LifeAssured Details

                            ObjPolicy.ListAssured.Add(objMemberDetails[i].AssuredName);

                            //var SpouseInfo = Context.tblLifeQQs.Where(a => a.ContactID == tblcontact.ContactID).FirstOrDefault();
                            //var SpouseInf = Context.tblContacts.Where(a => a.ContactID == SpouseInfo.ContactID).FirstOrDefault();
                            //if (SpouseInf != null)
                            //{
                            //      SpouseInf.SpouseName = objMemberDetails[i].FirstName;
                            //}
                            ObjPolicy.IsSpouseCovered = true;

                            #endregion

                            if (ObjPolicy.ProductID == 1)
                            {
                                objMemberDetails[i] = MemberDetailsForSelfAndSpouse(objMemberDetails[i], Convert.ToString(ObjPolicy.ProductID));
                            }
                            else
                            {
                                objMemberDetails[i] = MemberDetailsForSelfAndSpouse(objMemberDetails[i], null);
                            }
                            //objMemberDetails[i] = MemberDetailsForSelfAndSpouse(objMemberDetails[i],ObjPolicy.PlanCode);

                            //ObjPolicy.objProspectDetails.Index = i;
                            objMemberDetails[i].Index = i;

                            objMemberDetails[i].objCommunicationAddress = new AIA.Life.Models.Common.Address();
                            objMemberDetails[i].objPermenantAddress = new AIA.Life.Models.Common.Address();

                            //ObjPolicy.objMemberDetails.Add(ObjPolicy.objProspectDetails);
                            for (int j = 0; j < lstBenefit.Count(); j++)
                            {
                                AssuredRelation objAssuredRelation = new AssuredRelation();
                                objAssuredRelation.Assured_Name = objMemberDetails[i].AssuredName;
                                objAssuredRelation.Member_Relationship = objMemberDetails[i].RelationShipWithPropspect;
                                lstBenefit[j].objBenefitMemberRelationship.Add(objAssuredRelation);
                            }
                            FillRiderDetails(objMemberDetails[i].QuoteMemberID, objMemberDetails[i].AssuredName, objMemberDetails[i], ref lstBenefit);

                        }
                        else
                        {

                            objMemberDetails[i].Index = i;
                            ChildCount++;

                            objMemberDetails[i] = MemberDetailsForChild(objMemberDetails[i]);
                            objMemberDetails[i].AssuredName = "Child" + ChildCount;
                            ObjPolicy.ListAssured.Add("Child" + ChildCount);


                            #region Set Gender Based on relationship
                            if (objMemberDetails[i].RelationShipWithPropspect == "269")
                            {
                                //objMemberDetails[i].GenderText = Context.tblQuoteMemberDetials.Where(a => a.MemberID == objMemberDetails[i].QuoteMemberID).Select(a => a.Gender).FirstOrDefault();
                                objMemberDetails[i].Gender = objMemberDetails[i].GenderText;
                            }
                            else if (objMemberDetails[i].RelationShipWithPropspect == "270")
                            {
                                //objMemberDetails[i].Gender = Context.tblQuoteMemberDetials.Where(a => a.AssuredName == objMemberDetails[i].AssuredName).Select(a => a.Gender).FirstOrDefault();
                                objMemberDetails[i].Gender = "F";
                            }
                            #endregion

                            objMemberDetails[i].objCommunicationAddress = new AIA.Life.Models.Common.Address();
                            objMemberDetails[i].objPermenantAddress = new AIA.Life.Models.Common.Address();
                            // ObjPolicy.objMemberDetails.Add(objMemberDetails[i]);
                            for (int j = 0; j < lstBenefit.Count(); j++)
                            {
                                AssuredRelation objAssuredRelation = new AssuredRelation();
                                objAssuredRelation.Assured_Name = objMemberDetails[i].AssuredName;
                                objAssuredRelation.Member_Relationship = objMemberDetails[i].RelationShipWithPropspect;
                                lstBenefit[j].objBenefitMemberRelationship.Add(objAssuredRelation);
                            }

                            FillRiderDetails(objMemberDetails[i].QuoteMemberID, objMemberDetails[i].AssuredName, objMemberDetails[i], ref lstBenefit);
                        }

                        //objMember.AnyAdverseRemarks = objMemberDetails.tblMemberQuestions.Where(a => a.Answer == "true").Select(a => a.Answer).FirstOrDefault() == "true" ? true : false;
                        objMemberDetails[i].Index = MemberIndex;
                        #region Member Riders For Proposal Fetch For UW Refferal 
                        //ObjPolicy.ProposalFetch = true;
                        //if (ObjPolicy.ProposalFetch)
                        //{
                        string memberID = objMemberDetails[i].RelationShipWithPropspect;
                        decimal quotememberid = objMemberDetails[i].QuoteMemberID;
                        // foreach (var Rider in Context.tblQuoteMemberDetials.Where(a => a.LifeQQID == tbllifeQQ.LifeQQID && a.IsDeleted != true))
                        // {
                        BenifitDetails objBenefitDetail = new BenifitDetails();
                        var lstmemberbenifits = (from QuoteMember in Context.tblQuoteMemberDetials.Where(a => a.LifeQQID == tbllifeQQ.LifeQQID && a.IsDeleted != true)
                                                 join MemberBenefit in Context.tblQuoteMemberBeniftDetials
                                                 on QuoteMember.MemberID equals MemberBenefit.MemberID
                                                 join MasBenefit in Context.tblProductPlanRiders
                                                 on MemberBenefit.BenifitID equals MasBenefit.ProductPlanRiderId
                                                 where QuoteMember.Relationship == memberID && QuoteMember.MemberID == quotememberid
                                                 orderby MasBenefit.DisplayOrder ascending
                                                 select new BenifitDetails
                                                 {
                                                     BenifitName = MasBenefit.DisplayName,
                                                     RiderPremium = MemberBenefit.Premium,
                                                     RiderSuminsured = MemberBenefit.SumInsured,
                                                     BenefitID = MasBenefit.ProductPlanRiderId,
                                                     ActualRiderPremium = MemberBenefit.ActualPremium.ToString(),
                                                     LoadingAmount = MemberBenefit.LoadingAmount.ToString()
                                                     //ActualRiderPremium = MasBenefit.ActualRiderPremium,
                                                 }).DistinctBy(a => a.BenefitID).ToList();
                        objMemberDetails[i].objBenifitDetails.AddRange(lstmemberbenifits);
                        // }
                        //}
                        #endregion
                        ObjPolicy.objMemberDetails.Add(objMemberDetails[i]);
                        MemberIndex++;

                    }
                    if (ObjPolicy.objFillMemberDetials == null)
                    {
                        ObjPolicy.objFillMemberDetials = new MemberDetails();
                        ObjPolicy.objFillMemberDetials.objCommunicationAddress = new Address();
                        ObjPolicy.objFillMemberDetials.objPermenantAddress = new Address();
                    }
                    ObjPolicy.objMemberDetails = objMemberDetails;
                    ObjPolicy.lstCauseOfDeath = objCommonBusiness.GetMasCommonTypeMasterListItem("CauseOfDeathCase");
                    ObjPolicy.lstSateofHealth = objCommonBusiness.GetMasCommonTypeMasterListItem("StateOfHealth");
                    ObjPolicy.lstFamilyBackGroundRelationship = objCommonBusiness.GetMasCommonTypeMasterListItem("FamilyBackGroundRelationship");
                    // ObjPolicy.lstFamilyBackGroundRelationship = objCommonBusiness.GetMasCommonTypeMasterListItem("");

                    #endregion

                    #region Fill Benefit And Premium Details

                    ObjPolicy.LstBenifitDetails = lstBenefit;
                    ObjPolicy.LstPremiumDetails = lstBenefit;
                    for (int i = 0; i < objMemberDetails.Count(); i++)
                    {
                        decimal total = 0;
                        for (int j = 0; j < ObjPolicy.LstPremiumDetails.Count(); j++)
                        {
                            foreach (var item in ObjPolicy.LstPremiumDetails[j].objBenefitMemberRelationship.Where(a => a.Assured_Name == objMemberDetails[i].AssuredName))
                            {
                                decimal total_ = Convert.ToDecimal(item.Rider_Premium);
                                total = total_ + total;
                            }
                        }
                        ObjPolicy.objMemberDetails[i].Memberpremium = Convert.ToString(total);
                    }
                    ObjPolicy.AnnualPremium = Convert.ToDecimal(tbllifeQQ.AnnualPremium);
                    ObjPolicy.HalfYearlyPremium = Convert.ToDecimal(tbllifeQQ.HalfyearlyPremium);
                    ObjPolicy.QuaterlyPremium = Convert.ToDecimal(tbllifeQQ.QuarterlyPremium);
                    ObjPolicy.MonthlyPremium = Convert.ToDecimal(tbllifeQQ.Monthly);
                    ObjPolicy.VAT = Convert.ToDecimal(tbllifeQQ.Vat);
                    ObjPolicy.Cess = Convert.ToDecimal(tbllifeQQ.Cess);
                    ObjPolicy.PolicyFee = Convert.ToDecimal(tbllifeQQ.PolicyFee);

                    #endregion
                }

                ObjPolicy = objCommonBusiness.LoadMastersForProposal(ObjPolicy);

                // Added for Drop Down Values
                ObjPolicy.DropDownMemberDetails = GetDropDownValueForMemberDetails(ObjPolicy);
                // Till here


                #region Fetch NeedAnalysis Signature 
                //var ObjNeedAnalysis = Context.tblLifeNeedAnalysis.Where(a => a.ContactID == ObjPolicy.ContactID).FirstOrDefault();
                //if (ObjNeedAnalysis != null)
                //{
                //    ObjPolicy.ProposerSignPath = ObjNeedAnalysis.UploadSignPath;
                //}
                #endregion

                //string objPolicyProposal = Newtonsoft.Json.JsonConvert.SerializeObject(ObjPolicy);
                //Getting agent Code
                ObjPolicy.AgentCode = Context.tblMasIMOUsers.Where(a => a.UserID == ObjPolicy.UserName).Select(a => a.AgentCode).FirstOrDefault();

                return ObjPolicy;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MasterListItem> GetDropDownValueForMemberDetails(AIA.Life.Models.Policy.Policy ObjPolicy)
        {
            ObjPolicy.DropDownMemberDetails = new List<MasterListItem>();
            if (ObjPolicy.ListAssured != null)
            {
                foreach (var Member in ObjPolicy.ListAssured)
                {
                    MasterListItem obj = new MasterListItem();
                    obj.Text = Member;
                    obj.Value = Member;
                    ObjPolicy.DropDownMemberDetails.Add(obj);
                }
            }

            return ObjPolicy.DropDownMemberDetails;
        }

        public AIA.Life.Models.Policy.Policy LoadMHDProposalInfo(AIA.Life.Models.Policy.Policy ObjPolicy)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
            ObjPolicy.ListAssured = new List<string>();
            int Index = ObjPolicy.objMemberDetails.FindIndex(a => a.AssuredName == ObjPolicy.AssuredName);
            if (Index >= 0)
            {
                if (ObjPolicy.objMemberDetails[Index].objLstMedicalHistory.Where(a => a.QuestionID == 31).FirstOrDefault().Diseases.Count() > 0)
                {
                    ObjPolicy.objMemberDetails[Index].DiseasesSelected = ObjPolicy.objMemberDetails[Index].objLstMedicalHistory.Where(a => a.QuestionID == 31).FirstOrDefault().Diseases.ToList();
                    ObjPolicy.objMemberDetails[Index] = MemberDetailsForMedicalHistoryDiseases(ObjPolicy.objMemberDetails[Index]);
                    ObjPolicy.objMemberDetails[Index].Index = Index;
                    ObjPolicy.AssuredIndex = Index;
                }
            }
            return ObjPolicy;
        }

        public MemberDetails MemberDetailsForMedicalHistoryDiseases(MemberDetails objMember)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            objMember.objLstDiseaseHistory = new List<MedicalHistoryQuestions>();
            string QuestionSubType = string.Empty;

            if (objMember.DiseasesSelected != null)
            {
                for (int i = 0; i < objMember.DiseasesSelected.Count; i++)
                {
                    QuestionSubType = objMember.DiseasesSelected[i];
                    GetDiseasesWithSubType(objMember, QuestionSubType);
                }
            }

            return objMember;
        }
        public MemberDetails GetDiseasesWithSubType(MemberDetails objMember, string QuestionSubType)
        {
            List<MedicalHistoryQuestions> objListQuestions = new List<MedicalHistoryQuestions>();
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            objListQuestions = (from Question in Context.tblMasLifeQuestionnaires
                                where Question.QType == "DiseaseQuestions" && Question.Subtype == QuestionSubType && Question.IsDeleted != true
                                select new MedicalHistoryQuestions
                                {
                                    QuestionID = Question.QId,
                                    QuestionText = Question.QText,
                                    SubType = Question.Subtype,
                                    CotrolType = Question.ControlType,
                                    SubQuestion = Question.SubQuestion,
                                    SubControlType = Question.SubControlType,
                                    Master = Question.Master,
                                    Value = Question.Value,
                                    SequenceNo = Question.SequenceNo
                                }).ToList().Select(x => new MedicalHistoryQuestions
                                {
                                    QuestionID = x.QuestionID,
                                    QuestionText = x.QuestionText,
                                    SubType = x.SubType,
                                    CotrolType = x.CotrolType,
                                    SubQuestion = x.SubQuestion,
                                    SubControlType = x.SubControlType,
                                    LstDropDownvalues = GetDropDownValuesForQuestions(x.Master),
                                    Value = x.Value,
                                    SequenceNo = x.SequenceNo
                                }).ToList();

            objMember.objLstDiseaseHistory.AddRange(objListQuestions);
            return objMember;
        }
        //End

        public AIA.Life.Models.Policy.Policy FetchProposalInfo(AIA.Life.Models.Policy.Policy ObjPolicy)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                tblPolicy objpolicy = Context.tblPolicies.Where(a => a.PolicyID == ObjPolicy.PolicyID).FirstOrDefault();
                if (objpolicy == null && !string.IsNullOrEmpty(ObjPolicy.QuoteNo))
                {
                    objpolicy = new tblPolicy();
                    objpolicy = Context.tblPolicies.Where(a => a.QuoteNo == ObjPolicy.QuoteNo).FirstOrDefault();
                }
                Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
                if (objpolicy != null)
                {
                    ObjPolicy.QuoteNo = objpolicy.QuoteNo;
                    var tbllifeQQ = Context.tblLifeQQs.Where(a => a.QuoteNo == objpolicy.QuoteNo).FirstOrDefault();
                    ObjPolicy.ContactID = tbllifeQQ.ContactID;

                    #region  Policy Info
                    Guid createdBy = new Guid(objpolicy.Createdby);
                    ObjPolicy.AgentCode = Context.tblUserDetails.Where(a => a.UserID == createdBy).Select(a => a.IMDCode).FirstOrDefault();
                    ObjPolicy.PolicyID = objpolicy.PolicyID;
                    ObjPolicy.PolicyTerm = objpolicy.PolicyTerm;
                    ObjPolicy.PaymentTerm = objpolicy.PremiumTerm;
                    ObjPolicy.PaymentFrequency = objpolicy.PaymentFrequency;
                    ObjPolicy.PaymentMethod = objpolicy.PaymentMethod;
                    ObjPolicy.PaymentPaidBy = objpolicy.PaymentPaidBy;
                    ObjPolicy.others = objpolicy.Others;
                    ObjPolicy.ProposalNo = objpolicy.ProposalNo;
                    ObjPolicy.ModeOfCommunication = objpolicy.ModeOfCommunication;
                    ObjPolicy.PaymentReceiptPrefferdBy = objpolicy.PreferredReceipt;
                    ObjPolicy.PreferredLanguage = objpolicy.PreferredLanguage;
                    ObjPolicy.QuoteNo = objpolicy.QuoteNo;
                    ObjPolicy.PolicyStageStatusID = objpolicy.PolicyStageStatusID;
                    ObjPolicy.PolicyStageID = objpolicy.PolicyStageID;

                    ObjPolicy.TotalAnnualPremiumContribution = objpolicy.AnnualPremium;
                    ObjPolicy.ProposalDepositPremium = objpolicy.DepositPremium;
                    ObjPolicy.BankAccountNo = objpolicy.BankAccountNumber;
                    ObjPolicy.BranchName = objpolicy.BankBranchName;
                    ObjPolicy.CreditCardNo = objpolicy.CreditCardNo;
                    ObjPolicy.BankName = objpolicy.BankName;

                    ObjPolicy.MaturityBenefits = objpolicy.MaturityBenefits;
                    ObjPolicy.Years = objpolicy.Years;
                    ObjPolicy.SmartPensionReceivingPeriod = objpolicy.SmartPensionReceivingPeriod;
                    ObjPolicy.SmartPensionMonthlyIncome = objpolicy.SmartPensionMonthlyIncome;
                    ObjPolicy.Deductible = objpolicy.Deductible;
                    ObjPolicy.LeadNo = objpolicy.LeadNo;
                    ObjPolicy.IntroducerCode = objpolicy.IntroducerCode;
                    ObjPolicy.IsAfc = objpolicy.IsAfc;
                    ObjPolicy.RiskCommencementDate = objpolicy.PolicyIssueDate;
                    int[] exceptionDays = new int[3] { 29, 30, 31 };
                    if (exceptionDays.Contains(ObjPolicy.RiskCommencementDate.Day))
                        ObjPolicy.RiskCommencementDate = new DateTime(ObjPolicy.RiskCommencementDate.Year, ObjPolicy.RiskCommencementDate.Month, 28);
                    DateTime submitDate = objpolicy.ProposalSubmitDate ?? DateTime.Now;
                    ObjPolicy.CommencementDate = ObjPolicy.RiskCommencementDate.Year.ToString() + ObjPolicy.RiskCommencementDate.Month.ToString().PadLeft(2, '0') + ObjPolicy.RiskCommencementDate.Day.ToString().PadLeft(2, '0');
                    ObjPolicy.ProposalDate = objpolicy.CreatedDate.Year.ToString() + objpolicy.CreatedDate.Month.ToString().PadLeft(2, '0') + objpolicy.CreatedDate.Day.ToString().PadLeft(2, '0');
                    ObjPolicy.ProposalSubmittedDate = submitDate.Year.ToString() + submitDate.Month.ToString().PadLeft(2, '0') + submitDate.Day.ToString().PadLeft(2, '0');

                    if (objpolicy.PlanID != null)
                    {
                        var PlanInfo = Context.tblProducts.Where(a => a.ProductId == objpolicy.ProductID).FirstOrDefault();
                        if (PlanInfo != null)
                        {
                            ObjPolicy.PlanName = PlanInfo.ProductName;
                            ObjPolicy.PlanCode = PlanInfo.ProductCode;
                            ObjPolicy.ProductID = PlanInfo.ProductId;
                        }
                    }

                    #endregion

                    #region Fill Prospect Info
                    ObjPolicy.objProspectDetails = new MemberDetails();
                    var objtblpolicyRelationship = objpolicy.tblPolicyRelationships.FirstOrDefault();
                    tblPolicyClient objPolicyClient = objtblpolicyRelationship.tblPolicyClient;
                    ObjPolicy.objProspectDetails = FetchProposerInfo(ObjPolicy.objProspectDetails, objPolicyClient);
                    //ObjPolicy.objProspectDetails.lstAvgMonthlyIncome = objCommonBusiness.GetMasCommonTypeMasterListItem("MonthlyIncomeRange");
                    #region FillAddressDetails


                    tblAddress objtbladdress = objPolicyClient.tblAddress;
                    Address objAddress = new Address();
                    ObjPolicy.objProspectDetails.objCommunicationAddress = FetchAddressDetails(objtbladdress);

                    if (ObjPolicy.objProspectDetails.IsRegAsCommunication)
                    {
                        ObjPolicy.objProspectDetails.objPermenantAddress = ObjPolicy.objProspectDetails.objCommunicationAddress;
                    }
                    else
                    {
                        tblAddress objtblPermanentaddress = objPolicyClient.tblAddress1;
                        ObjPolicy.objProspectDetails.objPermenantAddress = FetchAddressDetails(objtblPermanentaddress);
                    }
                    #endregion

                    #endregion

                    #region  Member Details
                    ObjPolicy.ListAssured = new List<string>();
                    int MainLifeRelationship = IdentifyMainLife(objpolicy.tblPolicyMemberDetails.Where(a => a.IsDeleted != true).ToList());
                    int MemberIndex = 0;
                    int ChildCount = 0;
                    string UWDeviationMessage = string.Empty;
                    List<AIA.Life.Models.Common.BenifitDetails> lstBenefit = new List<AIA.Life.Models.Common.BenifitDetails>();
                    foreach (tblPolicyMemberDetail Member in objpolicy.tblPolicyMemberDetails.Where(a => a.IsDeleted != true).ToList())
                    {
                        MemberDetails objMember = new MemberDetails();

                        var PlanId = 0;
                        if (tbllifeQQ != null)
                        {
                            PlanId = tbllifeQQ.PlanId;
                        }

                        objMember = FetchMemberDetails(Member, objMember, ObjPolicy.ProposalFetch, PlanId);
                        int Memberid = Member.RelationShipWithProposer;
                        #region Get List of Distinct Riders For the Given Quote                        
                        List<BenifitDetails> BenefitOverView = (from QuoteMember in Context.tblPolicyMemberDetails.Where(a => a.PolicyID == objpolicy.PolicyID && a.IsDeleted != true)
                                                                join MemberBenefit in Context.tblPolicyMemberBenefitDetails
                                                                on QuoteMember.MemberID equals MemberBenefit.MemberID
                                                                join MasBenefit in Context.tblProductPlanRiders
                                                                on MemberBenefit.BenifitID equals MasBenefit.ProductPlanRiderId
                                                                where QuoteMember.RelationShipWithProposer == Member.RelationShipWithProposer
                                                                select new AIA.Life.Models.Common.BenifitDetails
                                                                {
                                                                    BenifitName = MasBenefit.DisplayName,
                                                                    BenefitID = MasBenefit.ProductPlanRiderId
                                                                }).OrderBy(a => a.BenifitName).DistinctBy(a => a.BenefitID).ToList();
                        lstBenefit.AddRange(BenefitOverView);
                        #endregion
                        if (!string.IsNullOrEmpty(objMember.Citizenship1))
                            objMember.Citizenship1 = objMember.Citizenship1.ToUpper();

                        if (!string.IsNullOrEmpty(objMember.Citizenship2))
                            objMember.Citizenship2 = objMember.Citizenship2.ToUpper();

                        if (objMember.Nationality == "US" || objMember.Citizenship1 == "US" || objMember.Citizenship2 == "US") // To set USCitizen Flag
                        {
                            objMember.IsUSCitizen = true;
                        }
                        #region MemberLevel UW Section
                        if (ObjPolicy.UserType == "UW")
                        {
                            objMember = FetchUWDeviation(objMember, ObjPolicy.QuoteNo, ObjPolicy.UserName);
                            //ObjMemberDetails.ObjUwDecision.Decision;
                            ObjPolicy.LstMedicalCodes = objCommonBusiness.GetMasLookUpTypeReason(objMember.ObjUwDecision.Decision);
                            ObjPolicy.LstReason = objCommonBusiness.GetMasLookUpTypeMedicalCodes();

                            #region UW Member Summary
                            objMember.ObjUwDecision.objMemberSummary = new MemberSummary();
                            objMember.ObjUwDecision.objMemberSummary.Name = objMember.FirstName;
                            objMember.ObjUwDecision.objMemberSummary.Age = objMember.Age;
                            objMember.ObjUwDecision.objMemberSummary.FAL = Convert.ToString(objMember.FAL);
                            objMember.ObjUwDecision.objMemberSummary.SAR = Convert.ToString(objMember.SAR);
                            objMember.ObjUwDecision.objMemberSummary.BMIValue = Convert.ToString(objMember.BMIValue);
                            objMember.ObjUwDecision.objMemberSummary.Occupation = Convert.ToInt32(objMember.OccupationID);
                            var AnnualPremium = Context.tblProposalPremiums.Where(a => a.PolicyID == ObjPolicy.PolicyID).Select(a => a.AnnualPremium).FirstOrDefault();
                            if (ObjPolicy.PaymentFrequency == "01"|| ObjPolicy.PaymentFrequency == "1")
                            {
                                objMember.ObjUwDecision.objMemberSummary.TotalAnnualPremium = Convert.ToString(Convert.ToInt64(AnnualPremium));
                            }
                            else if (ObjPolicy.PaymentFrequency == "04"|| ObjPolicy.PaymentFrequency == "4")
                            {
                                objMember.ObjUwDecision.objMemberSummary.TotalAnnualPremium = Convert.ToString(Convert.ToInt64(AnnualPremium)*4);
                            }
                            else if (ObjPolicy.PaymentFrequency == "02"|| ObjPolicy.PaymentFrequency == "2")
                            {
                                objMember.ObjUwDecision.objMemberSummary.TotalAnnualPremium = Convert.ToString(Convert.ToInt64(AnnualPremium)*2);
                            }
                            else if (ObjPolicy.PaymentFrequency == "12")
                            {
                                objMember.ObjUwDecision.objMemberSummary.TotalAnnualPremium = Convert.ToString(Convert.ToInt64(AnnualPremium)*12);
                            }
                           
                            objMember.ObjUwDecision.objMemberSummary.AFC = objMember.AFC;

                            tblMemberLevelDecision memberLevelDecision = Context.tblMemberLevelDecisions.Where(a => a.MemberID == objMember.MemberID).FirstOrDefault();
                            if (memberLevelDecision != null)
                            {
                                objMember.ObjUwDecision.Decision = memberLevelDecision.Decision;
                                objMember.ObjUwDecision.DecisionDate = memberLevelDecision.DecisionDate;
                                objMember.ObjUwDecision.CommencementDate = memberLevelDecision.Commencement_Date;
                                objMember.ObjUwDecision.Remarks = memberLevelDecision.Remarks;
                                objMember.ObjUwDecision.UWReason = memberLevelDecision.UWReason;
                                objMember.ObjUwDecision.UWMonth = memberLevelDecision.UWDuration;
                                objMember.ObjUwDecision.UWMedicalCode = memberLevelDecision.UWMedicalCode;
                                objMember.ObjUwDecision.MedicalFeePaidBy = memberLevelDecision.MedicalFeePaidBy;
                            }
                            

                            var CurrentProposalSARAndFAL = Context.SP_GetSARAndFALDetailsForQuote(ObjPolicy.QuoteNo).ToList();
                            if (CurrentProposalSARAndFAL != null)
                            {
                                decimal QuoteMemberId = objMember.QuoteMemberID;
                                var MemberInfo = CurrentProposalSARAndFAL.Where(a => a.MemberID == QuoteMemberId).FirstOrDefault();
                                if (MemberInfo != null)
                                {
                                    objMember.ObjUwDecision.objMemberSummary.FAL = Convert.ToString(MemberInfo.FAL);
                                    objMember.ObjUwDecision.objMemberSummary.SAR = Convert.ToString(MemberInfo.SAR);
                                }
                            }
                            #endregion

                            #region UW Member History
                            objMember.ObjUwDecision.objMemberUWHistory = GetMemberUWhistoryDetials(objMember);
                            #endregion
                        }
                        #endregion

                        #region Member Riders
                        ObjPolicy.ListAssured.Add(objMember.AssuredName);
                        for (int j = 0; j < lstBenefit.Count(); j++)
                        {
                            AssuredRelation objAssuredRelation = new AssuredRelation();
                            objAssuredRelation.Assured_Name = objMember.AssuredName;
                            objAssuredRelation.Member_Relationship = objMember.RelationShipWithPropspect;
                            lstBenefit[j].objBenefitMemberRelationship.Add(objAssuredRelation);
                        }
                        FillRiderDetailsForProposal(objMember.MemberID, objMember.AssuredName, objMember, ref lstBenefit);
                        #endregion

                        #region Fill Benefit And Premium Details

                        ObjPolicy.LstBenifitDetails = lstBenefit;
                        ObjPolicy.LstPremiumDetails = lstBenefit;

                        #endregion
                        if (Member.RelationShipWithProposer == 268)
                        {
                            objMember.IsSpouseCoverd = true;
                            ObjPolicy.IsSpouseCovered = true;
                        }
                        else if (Member.RelationShipWithProposer == 267)
                        {
                            objMember.IsSelfCovered = true;
                        }
                        else
                        {
                            ChildCount++;
                        }
                        if (Member.RelationShipWithProposer == 267 || Member.RelationShipWithProposer == 268)
                        {
                            #region LifeStyleDetails
                            if (Member.tblMemberLifeStyleDetails.Count() > 0)
                            {
                                var tblmemberLifeStyleDetails = Member.tblMemberLifeStyleDetails.FirstOrDefault();
                                objMember.objLifeStyleQuetions = new LifeStyleQA();
                                objMember.objLifeStyleQuetions.Height = Convert.ToInt32(tblmemberLifeStyleDetails.Height);
                                objMember.objLifeStyleQuetions.Weight = Convert.ToInt32(tblmemberLifeStyleDetails.Weight);
                                objMember.objLifeStyleQuetions.HeightFeets = Convert.ToInt32(tblmemberLifeStyleDetails.HeightFeets);
                                objMember.objLifeStyleQuetions.WeightUnit = tblmemberLifeStyleDetails.UnitofWeight;
                                objMember.objLifeStyleQuetions.SteadyWeight = Convert.ToBoolean(tblmemberLifeStyleDetails.IsWeightSteady);
                                objMember.objLifeStyleQuetions.IsAlcholic = Convert.ToBoolean(tblmemberLifeStyleDetails.IsAlcoholic);
                                objMember.objLifeStyleQuetions.IsSmoker = Convert.ToBoolean(tblmemberLifeStyleDetails.IsSmoker);
                                objMember.objLifeStyleQuetions.MemberLifeStyleID = tblmemberLifeStyleDetails.MemberLifeStyleID;

                                objMember.objLifeStyleQuetions.objSmokeDetails = new List<SmokeDetails>();
                                objMember.objLifeStyleQuetions.objSmokeDetails = (from smokeDetails in tblmemberLifeStyleDetails.tblMemberAdditionalLifeStyleDetails.Where(a => a.IsDeleted != true && a.ItemType == "Smoke")
                                                                                  select new SmokeDetails
                                                                                  {
                                                                                      AdditionalLifeStyleID = smokeDetails.AdditionalLifeStyleID,
                                                                                      SmokeType = smokeDetails.Type,
                                                                                      SmokeQuantity = smokeDetails.Number,
                                                                                      SmokePerDay = smokeDetails.Per,
                                                                                      SmokeDuration = smokeDetails.Term,
                                                                                      Isdeleted = false
                                                                                  }).ToList();

                                objMember.objLifeStyleQuetions.objAlcoholDetails = new List<AlcoholDetails>();
                                objMember.objLifeStyleQuetions.objAlcoholDetails = (from _AlcoholDetails in tblmemberLifeStyleDetails.tblMemberAdditionalLifeStyleDetails.Where(a => a.IsDeleted != true && a.ItemType == "Alcohol")
                                                                                    select new AlcoholDetails
                                                                                    {
                                                                                        AdditionalLifeStyleID = _AlcoholDetails.AdditionalLifeStyleID,
                                                                                        AlcholType = _AlcoholDetails.Type,
                                                                                        AlcholQuantity = _AlcoholDetails.Number,
                                                                                        AlcholPerDay = _AlcoholDetails.Per,
                                                                                        AlcholDuration = _AlcoholDetails.Term,
                                                                                        Isdeleted = false
                                                                                    }).ToList();
                            }
                            #endregion

                            #region Family BackGround History  

                            objMember.objLstFamily = new List<QuestionsList>();
                            objMember.objLstFamily = GetMasQuestions(Context, "FamilyBackGround", null, null, null, null, Member.MemberID);

                            if (Member.tblPolicyMemberFamilyHistories.Count() > 0)
                            {
                                objMember.objLstFamilyBackground = new List<LifeAssuredFamilyBackground>();
                                foreach (var item in Member.tblPolicyMemberFamilyHistories.Where(a => a.IsDeleted != true).ToList())
                                {
                                    LifeAssuredFamilyBackground objfamilyHistory = new LifeAssuredFamilyBackground();
                                    objfamilyHistory.PresentAge = item.PresentAge;
                                    objfamilyHistory.FamilyPersonType = item.RelationshipWithMember;
                                    objfamilyHistory.StateOfHealth = item.StateofHealth;
                                    objfamilyHistory.AgeAtDeath = item.AgeAtDeath;
                                    objfamilyHistory.Cause = item.CauseofDeath;
                                    objfamilyHistory.Details = item.Details;
                                    objfamilyHistory.FamilyBackgroundId = Convert.ToInt32(item.MemberFamilyHistoryID);
                                    objMember.objLstFamilyBackground.Add(objfamilyHistory);
                                }
                            }
                            #endregion
                            #region Member Medical History & Additional Questions Grid View
                            if (ObjPolicy.ProductID == 1)
                            {
                                objMember.objLstMedicalHistory = GetMasQuestions(Context, "MedicalHistory", null, ObjPolicy.ProductID, null, objMember.Gender, Member.MemberID);
                            }
                            //else if (objpolicy.PlanID == 77 || objpolicy.PlanID == 78)
                            //{
                            //    var NMLCount = 0; //SAR <= NML 
                            //    var result = Context.usp_GetDocumentsForPDF(ObjPolicy.QuoteNo).ToList();
                            //    if (result.Count == 0)
                            //    {
                            //    }
                            //    else
                            //    {
                            //        foreach (var item in result)
                            //        {
                            //            if (!String.IsNullOrEmpty(item.MedicalDoc))
                            //            {
                            //                NMLCount++;//SAR > NML
                            //            }
                            //        }
                            //    }
                            //    var MinSAM = Context.tblMasSAMs.Where(a => a.PlanId == objpolicy.PlanID).Where(a =>a.MinAge <= Member.Age).Where(a=>a.MixAge >= Member.Age).Select(a => a.MinSAM).Min();
                            //    var SAM = Context.tblLifeQQs.Where(a => a.QuoteNo == objpolicy.QuoteNo).Select(a => a.SAM).FirstOrDefault();
                            //    if (objpolicy.PlanID == 77)
                            //    {
                            //        if (Convert.ToInt32(SAM) == MinSAM && NMLCount == 0 )
                            //        {
                            //            objMember.objLstMedicalHistory = GetMasQuestions(Context, "MedicalHistory", null, Convert.ToInt32(objpolicy.ProductID), null, null, Member.MemberID, null, false, null, "Short");
                            //        }
                            //        else if(Convert.ToInt32(SAM) > MinSAM || NMLCount != 0)
                            //        {
                            //            objMember.objLstMedicalHistory = GetMasQuestions(Context, "MedicalHistory", null, Convert.ToInt32(objpolicy.ProductID), null, null, Member.MemberID, null, false, null, "Long");
                            //        }
                            //    }
                            //    else
                            //    {
                            //        if (Convert.ToInt32(SAM) == MinSAM && NMLCount == 0)
                            //        {
                            //            objMember.objLstMedicalHistory = GetMasQuestions(Context, "MedicalHistory", null, Convert.ToInt32(objpolicy.ProductID), null, null, Member.MemberID, null, false, null, "Short");
                            //        }
                            //        else if (Convert.ToInt32(SAM) > MinSAM || NMLCount != 0)
                            //        {
                            //            objMember.objLstMedicalHistory = GetMasQuestions(Context, "MedicalHistory", null, Convert.ToInt32(objpolicy.ProductID), null, null,  Member.MemberID, null, false, null, "Long");
                            //        }
                            //    }


                            //}
                            else
                            {
                                objMember.objLstMedicalHistory = GetMasQuestions(Context, "MedicalHistory", null, null, null, objMember.Gender, Member.MemberID);
                            }

                            objMember.LstMedicalQuestionnariesDetails = (from QuestionsDetails in Member.tblQuestionDetails.Where(a => a.IsDeleted != true && a.QID == 294 && a.MemberID == objMember.MemberID).ToList()
                                                                         select new MedicalQuestionnariesDetails
                                                                         {
                                                                             QuestionsId = QuestionsDetails.QuestionsId,  //Medical Questions GridView
                                                                             //varcharFiled1 = QuestionsDetails.varcharFiled1,
                                                                             //varcharFiled2 = QuestionsDetails.varcharFiled2,
                                                                             //DateFiled1 = QuestionsDetails.DateFiled1,
                                                                             //varcharFiled3 = QuestionsDetails.varcharFiled3,// TreatMent Grid View
                                                                             //varcharFiled4 = QuestionsDetails.varcharFiled4,
                                                                             //varcharFiled5 = QuestionsDetails.varcharFiled5,
                                                                             //DateFiled2 = QuestionsDetails.DateFiled2,
                                                                             //varcharFiled6 = QuestionsDetails.varcharFiled6,  // Current Grid
                                                                             //varcharFiled7 = QuestionsDetails.varcharFiled7,
                                                                             //varcharFiled8 = QuestionsDetails.varcharFiled8,
                                                                             varcharFiled9 = QuestionsDetails.varcharFiled9,  // Past Grid
                                                                             varcharFiled10 = QuestionsDetails.varcharFiled10,
                                                                             varcharFiled11 = QuestionsDetails.varcharFiled11,
                                                                             DateFiled3 = QuestionsDetails.DateFiled3,

                                                                             //PAQvarcharFiled1 = QuestionsDetails.PAQvarcharFiled1, // PAQ Grid View 1
                                                                             //PAQvarcharFiled2 = QuestionsDetails.PAQvarcharFiled2,
                                                                             //PAQvarcharFiled3 = QuestionsDetails.PAQvarcharFiled3,
                                                                             //PAQvarcharFiled4 = QuestionsDetails.PAQvarcharFiled4,
                                                                             //PAQvarcharFiled5 = QuestionsDetails.PAQvarcharFiled5, // PAQ Grid View 2
                                                                             //PAQvarcharFiled6 = QuestionsDetails.PAQvarcharFiled6,
                                                                             //PAQvarcharFiled7 = QuestionsDetails.PAQvarcharFiled7,
                                                                             //PAQvarcharFiled8 = QuestionsDetails.PAQvarcharFiled8,
                                                                             //PAQvarcharFiled9 = QuestionsDetails.PAQvarcharFiled9,  // PAQ Grid View 4
                                                                             //PAQvarcharFiled10 = QuestionsDetails.PAQvarcharFiled10,
                                                                             //PAQvarcharFiled11 = QuestionsDetails.PAQvarcharFiled11,
                                                                             //PAQvarcharFiled12 = QuestionsDetails.PAQvarcharFiled12,
                                                                             //PAQAssetsTotal = QuestionsDetails.PAQAssetsTotal,
                                                                             //PAQLiabilitiesTotal = QuestionsDetails.PAQLiabilitiesTotal,
                                                                             //PAQYearFiled1 = QuestionsDetails.PAQYearFiled1, // PAQ Grid View 3
                                                                             //PAQYearFiled2 = QuestionsDetails.PAQYearFiled2,
                                                                             //PAQYearFiled3 = QuestionsDetails.PAQYearFiled3

                                                                         }).ToList();

                            objMember.LstMedicalDoctorsQuestionnariesDetails = (from QuestionsDetails in Member.tblQuestionDetails.Where(a => a.QID == 299 && a.IsDeleted != true && a.MemberID == objMember.MemberID).ToList()
                                                                                select new DoctorsMedicalQuestionnariesDetails
                                                                                {
                                                                                    QuestionsId = QuestionsDetails.QuestionsId,  //Medical Questions GridView
                                                                                    varcharFiled1 = QuestionsDetails.varcharFiled1,
                                                                                    varcharFiled2 = QuestionsDetails.varcharFiled2,
                                                                                    DateFiled1 = QuestionsDetails.DateFiled1,
                                                                                }).ToList();
                            objMember.LstMedicalTestQuestionnariesDetails = (from QuestionsDetails in Member.tblQuestionDetails.Where(a => a.QID == 296 && a.IsDeleted != true && a.MemberID == objMember.MemberID).ToList()
                                                                             select new TestMedicalQuestionnariesDetails
                                                                             {
                                                                                 QuestionsId = QuestionsDetails.QuestionsId,  //Medical Questions GridView
                                                                                 varcharFiled3 = QuestionsDetails.varcharFiled3,
                                                                                 varcharFiled4 = QuestionsDetails.varcharFiled4,
                                                                                 varcharFiled5 = QuestionsDetails.varcharFiled5,
                                                                                 DateFiled2 = QuestionsDetails.DateFiled2,
                                                                             }).ToList();
                            objMember.LstMedicalCurrentQuestionnariesDetails = (from QuestionsDetails in Member.tblQuestionDetails.Where(a => a.QID == 292 && a.IsDeleted != true && a.MemberID == objMember.MemberID).ToList()
                                                                                select new CurrentMedicalQuestionnariesDetails
                                                                                {
                                                                                    QuestionsId = QuestionsDetails.QuestionsId,  //Medical Questions GridView
                                                                                    varcharFiled6 = QuestionsDetails.varcharFiled6,
                                                                                    varcharFiled7 = QuestionsDetails.varcharFiled7,
                                                                                    varcharFiled8 = QuestionsDetails.varcharFiled8,
                                                                                }).ToList();
                            objMember.LstConcurrentlyProposedInsurancePAQ1Details = (from QuestionsDetails in Member.tblQuestionDetails.Where(a => a.QID == 1178 && a.IsDeleted != true && a.MemberID == objMember.MemberID).ToList()
                                                                                     select new ConcurrentlyProposedInsurancePAQ1Details
                                                                                     {
                                                                                         QuestionsId = QuestionsDetails.QuestionsId,  //PAQ Questions GridView 1
                                                                                         PAQvarcharFiled1 = QuestionsDetails.PAQvarcharFiled1,
                                                                                         PAQvarcharFiled2 = QuestionsDetails.PAQvarcharFiled2,
                                                                                         PAQvarcharFiled3 = QuestionsDetails.PAQvarcharFiled3,
                                                                                         PAQvarcharFiled4 = QuestionsDetails.PAQvarcharFiled4,
                                                                                     }).ToList();
                            objMember.LstExistingPolicieswithAIAlnsurancePAQ2Details = (from QuestionsDetails in Member.tblQuestionDetails.Where(a => a.QID == 1180 && a.IsDeleted != true && a.MemberID == objMember.MemberID).ToList()
                                                                                        select new ExistingPolicieswithAIAlnsurancePAQ2Details
                                                                                        {
                                                                                            QuestionsId = QuestionsDetails.QuestionsId,  //PAQ Questions GridView 2
                                                                                            PAQvarcharFiled5 = QuestionsDetails.PAQvarcharFiled5,
                                                                                            PAQvarcharFiled6 = QuestionsDetails.PAQvarcharFiled6,
                                                                                            PAQvarcharFiled7 = QuestionsDetails.PAQvarcharFiled7,
                                                                                            PAQvarcharFiled8 = QuestionsDetails.PAQvarcharFiled8,
                                                                                        }).ToList();
                            objMember.ObjTotalAnnualIncomePAQ3Details.PAQYearFiled1 = Member.tblQuestionDetails.Where(a => a.QID == 1181 && a.IsDeleted != true && a.MemberID == objMember.MemberID && a.PAQYearFiled1 != null).Select(a => a.PAQYearFiled1).LastOrDefault();
                            objMember.ObjTotalAnnualIncomePAQ3Details.PAQYearFiled2 = Member.tblQuestionDetails.Where(a => a.QID == 1181 && a.IsDeleted != true && a.MemberID == objMember.MemberID && a.PAQYearFiled2 != null).Select(a => a.PAQYearFiled2).LastOrDefault();
                            objMember.ObjTotalAnnualIncomePAQ3Details.PAQYearFiled3 = Member.tblQuestionDetails.Where(a => a.QID == 1181 && a.IsDeleted != true && a.MemberID == objMember.MemberID && a.PAQYearFiled3 != null).Select(a => a.PAQYearFiled3).LastOrDefault();
                            objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQAssetsProperty = Member.tblQuestionDetails.Where(a => a.QID == 1184 && a.IsDeleted != true && a.MemberID == objMember.MemberID && a.PAQAssetsProperty != null).Select(a => a.PAQAssetsProperty).LastOrDefault();
                            objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQAssetsEquities = Member.tblQuestionDetails.Where(a => a.QID == 1184 && a.IsDeleted != true && a.MemberID == objMember.MemberID && a.PAQAssetsEquities != null).Select(a => a.PAQAssetsEquities).LastOrDefault();
                            objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQAssetsInvestment = Member.tblQuestionDetails.Where(a => a.QID == 1184 && a.IsDeleted != true && a.MemberID == objMember.MemberID && a.PAQAssetsInvestment != null).Select(a => a.PAQAssetsInvestment).LastOrDefault();
                            objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQAssetsOther = Member.tblQuestionDetails.Where(a => a.QID == 1184 && a.IsDeleted != true && a.MemberID == objMember.MemberID && a.PAQAssetsOther != null).Select(a => a.PAQAssetsOther).FirstOrDefault();
                            objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQAssetsTotal = Member.tblQuestionDetails.Where(a => a.QID == 1184 && a.IsDeleted != true && a.MemberID == objMember.MemberID && a.PAQAssetsTotal != null).Select(a => a.PAQAssetsTotal).FirstOrDefault();
                            objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQLiabilitiesLoans = Member.tblQuestionDetails.Where(a => a.QID == 1184 && a.IsDeleted != true && a.MemberID == objMember.MemberID && a.PAQLiabilitiesLoans != null).Select(a => a.PAQLiabilitiesLoans).LastOrDefault();
                            objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQLiabilitiesMortgages = Member.tblQuestionDetails.Where(a => a.QID == 1184 && a.IsDeleted != true && a.MemberID == objMember.MemberID && a.PAQLiabilitiesMortgages != null).Select(a => a.PAQLiabilitiesMortgages).LastOrDefault();
                            objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQLiabilitiesOthers = Member.tblQuestionDetails.Where(a => a.QID == 1184 && a.IsDeleted != true && a.MemberID == objMember.MemberID && a.PAQLiabilitiesOthers != null).Select(a => a.PAQLiabilitiesOthers).LastOrDefault();
                            objMember.ObjAssetsandLiabilitiesPAQ4Details.PAQLiabilitiesTotal = Member.tblQuestionDetails.Where(a => a.QID == 1184 && a.IsDeleted != true && a.MemberID == objMember.MemberID && a.PAQLiabilitiesTotal != null).Select(a => a.PAQLiabilitiesTotal).LastOrDefault();


                            #endregion

                            #region Member Life Style Questions        

                            objMember.Questions = new List<QuestionsList>();
                            if (ObjPolicy.ProductID == 1)
                            {
                                objMember.LstEasyPensionQuestions = GetMasQuestions(Context, "LifeStyle", null, ObjPolicy.ProductID, null, null, Member.MemberID);
                                objMember.Questions = GetMasQuestions(Context, "LifeStyle", null, null, null, null, Member.MemberID);
                            }
                            else
                            {
                                objMember.Questions = GetMasQuestions(Context, "LifeStyle", null, null, null, null, Member.MemberID);
                            }

                            #endregion

                            #region Additional Q&A
                            objMember.objAdditionalQuestions = new List<QuestionsList>();
                            string ResidentialNationalityStatus = "";
                            if (Member.Nationality == "SL" && Member.ResidentialNationalityStatus == "SL")
                            {
                                ResidentialNationalityStatus = "ABC";
                            }
                            if (Member.Nationality == "SL" && Member.CountryOccupation == "SL")
                            {
                                ResidentialNationalityStatus = "ABC";
                            }
                            if (Member.Nationality == "SL" && Member.ResidentialNationalityStatus != "SL")
                            {
                                ResidentialNationalityStatus = "SL";
                            }
                            if (Member.Nationality == "SL" && Member.CountryOccupation != "SL")
                            {
                                ResidentialNationalityStatus = "SL";
                            }
                            string FAL = "";
                            if (Member.FAL >= 5000000)
                            {
                                FAL = "5000000.0";
                            }
                            objMember.objAdditionalQuestions = GetMasQuestions(Context, "AdditionalQuestions", null, null, Member.OccupationID, null, Member.MemberID, ResidentialNationalityStatus, Convert.ToBoolean(Member.OccupationHazardousWork), FAL);

                            //if (Member.Nationality == "SL" && Member.ResidentialNationalityStatus == "SL" || Member.ResidentialNationalityStatus == null || Member.CountryOccupation == "2652")
                            //{
                            //    string ResidentialNationalityStatus = "ABC";

                            //    objMember.objAdditionalQuestions = GetMasQuestions(Context, "AdditionalQuestions", null, null, Member.OccupationID, null, Member.MemberID, ResidentialNationalityStatus, Convert.ToBoolean(Member.OccupationHazardousWork), SAR);
                            //}
                            //if (Member.Nationality == "SL" && Member.ResidentialNationalityStatus != "SL" && Member.ResidentialNationalityStatus != null || Member.CountryOccupation != "2652")
                            //{
                            //    string SAR = "";
                            //    if (Member.SAR >= 5000000)
                            //    {
                            //        SAR = "5000000.0";
                            //    }
                            //    string ResidentialNationalityStatus = "SL";
                            //    objMember.objAdditionalQuestions = GetMasQuestions(Context, "AdditionalQuestions", null, null, Member.OccupationID, null, Member.MemberID, ResidentialNationalityStatus, Convert.ToBoolean(Member.OccupationHazardousWork), SAR);
                            //}
                            #endregion
                        }
                        else
                        {
                            #region Member Chid Medical History    
                            objMember.objLstMedicalHistory = new List<QuestionsList>();
                            objMember.objLstMedicalHistory = GetMasQuestions(Context, "MedicalHistory", null, null, null, objMember.Gender = "C", objMember.MemberID).Where(a => a.Gender == "C" && a.Gender != null && a.Gender != "F").ToList();
                            #endregion

                            #region Set Gender Based on relationship
                            if (objMember.RelationShipWithPropspect == "269")
                            {
                                objMember.Gender = objMember.GenderText;
                            }
                            else if (objMember.RelationShipWithPropspect == "270")
                            {
                                objMember.Gender = objMember.GenderText;
                            }
                            #endregion
                        }

                        #region PreviousInsuranceGrid
                        LoadPolicyPreviousInsuranceGrid(ObjPolicy);
                        #endregion


                        if (Member.RelationShipWithProposer == 267)
                        {
                            #region  WealthPlannerQuestions
                            objMember.objLstWealthPlannerQuestions = GetMasQuestions(Context, "WealthPlannerQuestions", null, null, null, null, Member.MemberID);
                            #endregion
                        }


                        #region OtherInsurance Info
                        objMember.objLstOtherInsuranceDetails = new List<QuestionsList>();
                        objMember.objLstOtherInsuranceDetails = GetMasQuestions(Context, "PreviousAndCurrentLifeInsurance", null, null, null, null, Member.MemberID);
                        objMember.objLifeAssuredOtherInsurance = new List<LifeAssuredOtherInsurance>();
                        objMember.objLifeAssuredOtherInsurance = (from objOtherInsuInfo in Member.tblPolicyMemberInsuranceInfoes.Where(a => a.IsDeleted != true)
                                                                  select new LifeAssuredOtherInsurance
                                                                  {
                                                                      AccidentalBenefitAmount = objOtherInsuInfo.AccidentalBenifit,
                                                                      CompanyName = objOtherInsuInfo.CompanyName,
                                                                      HospitalizationPerDay = objOtherInsuInfo.HospitalizationPerDay,
                                                                      HospitalizationReimbursement = objOtherInsuInfo.HospitalizationReimbursement,
                                                                      PolicyNo = objOtherInsuInfo.Policy_ProposalNo,
                                                                      CriticalIllnessBenefit = objOtherInsuInfo.CriticalIllnessBenifit,
                                                                      CurrentStatus = objOtherInsuInfo.CurrentStatus,
                                                                      TotalSAAtDeath = objOtherInsuInfo.TotalSIAtDeath,
                                                                      RelationShipwithProposer = Member.RelationShipWithProposer,
                                                                      OtherInsuranceId = objOtherInsuInfo.MemberInsuranceID
                                                                  }).ToList();
                        #endregion

                        #region Policy Member Claim Info
                        objMember.AreyouClaimedAnyPolicies = Member.IsClaimExits != null ? Convert.ToBoolean(Member.IsClaimExits) : false;
                        objMember.objClaimInfo = new List<ClaimInformation>();
                        objMember.objClaimInfo = (from objOtherClaimInfo in Member.tblPolicyMemberClaimInfoes.Where(a => a.IsDeleted != true).ToList()
                                                  select new ClaimInformation
                                                  {
                                                      CompanyName = objOtherClaimInfo.CompanyName,
                                                      PolicyNo = objOtherClaimInfo.ProposalNo,
                                                      NatureOfClaim = objOtherClaimInfo.NatureOfClaim,
                                                      DateOfClaim = objOtherClaimInfo.DateOfClaim,
                                                      OtherClaimId = objOtherClaimInfo.MemberClaimID

                                                  }).ToList();
                        #endregion

                        //added to send the data to IL
                        objMember.AnyAdverseRemarks = Member.tblMemberQuestions.Where(a => a.Answer == "true").Select(a => a.Answer).FirstOrDefault() == "true" ? true : false;
                        objMember.Index = MemberIndex;
                        #region Member Riders For Proposal Fetch For UW Refferal 
                        ObjPolicy.ProposalFetch = true;
                        if (ObjPolicy.ProposalFetch)
                        {
                            var proposalRiders = (from po in Context.tblPolicyMemberBenefitDetails.Where(a => a.MemberID == Member.MemberID && a.IsDeleted != true)
                                                  join pa in Context.tblProductPlanRiders
 on po.BenifitID equals pa.ProductPlanRiderId
                                                  orderby pa.DisplayOrder ascending
                                                  select po).ToList();
                            foreach (var Rider in proposalRiders)
                            {
                                BenifitDetails objBenefitDetail = new BenifitDetails();
                                objBenefitDetail.RiderPremium = Rider.TotalPremium;
                                var RiderInfo = Context.tblProductPlanRiders.Where(a => a.ProductPlanRiderId == Rider.BenifitID).FirstOrDefault();
                                if (RiderInfo != null)
                                {
                                    objBenefitDetail.RiderCode = RiderInfo.RefRiderCode;
                                    objBenefitDetail.BenifitName = RiderInfo.DisplayName;
                                }
                                objBenefitDetail.RiderSuminsured = Rider.SumInsured;
                                objBenefitDetail.LoadingAmount = Convert.ToString(Rider.LoadingAmount);
                                objBenefitDetail.LoadingPercentage = Convert.ToString(Rider.LoadingPerc);
                                objBenefitDetail.LoadinPerMille = Convert.ToString(Rider.LoadinPerMille);
                                objBenefitDetail.ActualRiderPremium = Rider.Premium;
                                foreach (var loading in Context.tblMemberBenefitOtherDetails.Where(b => b.MemberBenifitID == Rider.MemberBenifitID).ToList())
                                {
                                    BenefitLoading benefitLoading = new BenefitLoading();
                                    int loadingBasis = Convert.ToInt32(loading.LoadingBasis);
                                    benefitLoading.LoadingBasis = Context.tblMasCommonTypes.Where(a => a.CommonTypesID == loadingBasis).Select(a => a.Code).FirstOrDefault();
                                    benefitLoading.LoadingType = loading.LoadingType;
                                    benefitLoading.LoadingPercentage = loading.LoadingAmount;
                                    objBenefitDetail.BenefitLoadings.Add(benefitLoading);
                                }

                                objMember.objBenifitDetails.Add(objBenefitDetail);
                            }
                        }
                        #endregion
                        ObjPolicy.objMemberDetails.Add(objMember);
                        MemberIndex++;
                    }
                    #endregion

                    #region Nominee Details
                    ObjPolicy.objNomineeDetails = new List<NomineeDetails>();
                    int NomineeIndex = 0;
                    foreach (var Nominee in objpolicy.tblPolicyNomineeDetails.Where(a => a.IsDeleted != true)) //NomineeID == Convert.ToDecimal(ObjPolicy.objNomineeDetails.NomineeDetailsId) &&
                    {
                        NomineeDetails objNominee = new NomineeDetails();
                        objNominee.NomineeRelationship = Convert.ToString(Nominee.NomineeRelation);
                        objNominee.NomineeSalutation = Nominee.Salutation;
                        objNominee.NomineeName = Nominee.NomineeName;
                        objNominee.NomineeSurname = Nominee.NomineeSurName;
                        objNominee.NomineeMaritalStatus = Nominee.NomineeMartialStatus;
                        objNominee.NomineeNICNo = Nominee.NICNo;
                        objNominee.NomineeNicNoDOB = Nominee.DOB;
                        objNominee.NomineeGender = Nominee.NomineeGender;
                        //var list = Context.tblPolicyMemberDetails.Where(a => a.PolicyID = Nominee.PolicyID).ToList();
                        //foreach (tblPolicyMemberDetail Member in objpolicy.tblPolicyMemberDetails.Where(a => a.IsDeleted != true).ToList())
                        //{
                        //    objNominee.NomineeAddress = Member.tblAddress.Address1 + "," + Member.tblAddress.Address2 + "," + Member.tblAddress.Address3 + "," + Member.tblAddress.City + "," + Member.tblAddress.State + "," + Member.tblAddress.Pincode;
                        //}
                        objNominee.NomineeTelephone = Nominee.NomineeMobileNo;
                        objNominee.NomineePercentage = Nominee.NomineeShare;
                        objNominee.NomineeDetailsId = Nominee.NomineeID;
                        objNominee.NomineeClientCode = Nominee.ClientCode;
                        objNominee.Index = NomineeIndex;
                        NomineeIndex++;
                        ObjPolicy.objNomineeDetails.Add(objNominee);
                    }
                    #endregion

                    #region Fill Document Info
                    ObjPolicy.objDocumentUpload = new List<DocumentUpload>();

                    List<tblPolicyDocument> objListPolicyDocuments = objpolicy.tblPolicyDocuments.Where(a => a.ItemType == "PolicyDocuments").ToList();
                    bool IsPendingReqCase = false;
                    // Pending Requirements
                    if (objpolicy.PolicyStageStatusID == CrossCuttingConstants.PolicyStageStatusPending || objpolicy.PolicyStageStatusID == CrossCuttingConstants.PolicyStageStatusCounterOffer || objpolicy.PolicyStageStatusID == CrossCuttingConstants.PolicyStageStatusOutStandingReq)
                    {
                        IsPendingReqCase = true;
                    }
                    ObjPolicy.objDocumentUpload = FetchPolicyDocuments(objListPolicyDocuments, ObjPolicy.ListAssured, IsPendingReqCase);

                    //var ProspectImage = objpolicy.tblPolicyDocuments.Where(a => a.ItemType == "ProposerImage").FirstOrDefault();
                    //if (ProspectImage != null)
                    //{
                    //    ObjPolicy.ProspectImagePath = ProspectImage.FilePath;
                    //}
                    //var SpouseImage = objpolicy.tblPolicyDocuments.Where(a => a.ItemType == "SpouseImage").FirstOrDefault();
                    //if (SpouseImage != null)
                    //{
                    //    ObjPolicy.SpouseImagePath = SpouseImage.FilePath;
                    //}


                    ////var proposerSignature = objpolicy.tblPolicyDocuments.Where(a => a.ItemType == "PropserSignature").FirstOrDefault();
                    ////if (proposerSignature != null)
                    ////{
                    ////    ObjPolicy.SpouseSignPath = proposerSignature.FilePath;
                    ////}
                    //if (ObjPolicy.PolicyStageStatusID != CrossCuttingConstants.PolicyStageStatusCounterOffer)
                    //{
                    var SpouseSignature = objpolicy.tblPolicyDocuments.Where(a => a.ItemType == "SpouseSignature").FirstOrDefault();
                    if (SpouseSignature != null)
                    {
                        ObjPolicy.SpouseSignPath = SpouseSignature.FilePath;
                        ObjPolicy.SpouseSignatureFile = SpouseSignature.File;
                    }
                    var WPProposerSignature = objpolicy.tblPolicyDocuments.Where(a => a.ItemType == "WPProposerSignature").FirstOrDefault();
                    if (WPProposerSignature != null)
                    {
                        ObjPolicy.WPProposerSignPath = WPProposerSignature.FilePath;
                        ObjPolicy.WPSignatureFile = WPProposerSignature.File;
                    }
                    var proposersignature = objpolicy.tblPolicyDocuments.Where(a => a.ItemType == "Proposer Signature").FirstOrDefault();
                    //var ObjNeedAnalysis = Context.tblLifeNeedAnalysis.Where(a => a.ContactID == ObjPolicy.ContactID).FirstOrDefault();
                    if (proposersignature != null)
                    {
                        ObjPolicy.ProposerSignPath = proposersignature.FilePath;
                        ObjPolicy.ProposerSignatureFile = proposersignature.File;
                        // ObjPolicy.ProspectSignPath = ObjNeedAnalysis.ProspectSign;
                    }
                    //}
                    #endregion

                    #region  Fetch Policy Extension               
                    var objPolicyExtensions = objpolicy.tblPolicyExtensions.FirstOrDefault();
                    if (objPolicyExtensions != null)
                    {
                        ObjPolicy.DoctorName = objPolicyExtensions.DoctorName;
                        ObjPolicy.LabName = objPolicyExtensions.LabName;
                        ObjPolicy.PaymentMadeByForDoctor = objPolicyExtensions.PaymentMadeByForDoctor;
                        ObjPolicy.PaymentMadeByForLab = objPolicyExtensions.PaymentMadeByForLab;
                        ObjPolicy.ReportsTobeSendTo = objPolicyExtensions.ReportsTobeSendTo;
                        ObjPolicy.ProposerDate = Convert.ToDateTime(objPolicyExtensions.ProposerDate);
                        ObjPolicy.ProposerPlace = objPolicyExtensions.ProposerPlace;
                        ObjPolicy.ProposerCountry = objPolicyExtensions.ProposerCountry;
                        ObjPolicy.ProposerDocumentType = objPolicyExtensions.ProposerDocumentType;
                        ObjPolicy.WPDeclaration = Convert.ToBoolean(objPolicyExtensions.ProposerWealthPlanner);
                        //ObjPolicy.ProposerWealthPlannerPolicyDateing = Convert.ToBoolean(objPolicyExtensions.ProposerWealthPlannerPolicy);
                        //ObjPolicy.ProposerWealthPlannerPolicyDate = objPolicyExtensions.ProposerWealthPlannerPolicyBackDate;
                        //ObjPolicy.ProsperSignPath = objPolicyExtensions.ProspectSignPath;
                        // ObjPolicy.SpouserSignPath = objPolicyExtensions.SpouseSignPath;
                        ObjPolicy.ProposerWealthPlannerComments = objPolicyExtensions.ProposerWealthPlannerComments;
                        ObjPolicy.SpouseDate = Convert.ToDateTime(objPolicyExtensions.SpouseDate);
                        ObjPolicy.SpousePlace = objPolicyExtensions.SpousePlace;
                        ObjPolicy.SpouseCountry = objPolicyExtensions.SpouseCountry;
                        ObjPolicy.SpouseDocumentType = objPolicyExtensions.SpouseDocumentType;
                        ObjPolicy.SpouseWealthPlanner = Convert.ToBoolean(objPolicyExtensions.SpouseWealthPlanner);
                        ObjPolicy.SpouseWealthPlannerPolicyDateing = Convert.ToBoolean(objPolicyExtensions.SpouseWealthPlannerPolicy);
                        ObjPolicy.SpouseWealthPlannerPolicyDate = Convert.ToDateTime(objPolicyExtensions.SpouseWealthPlannerPolicyBackDate);
                        ObjPolicy.SpouseWealthPlannerComments = objPolicyExtensions.SpouseWealthPlannerComments;
                        ObjPolicy.Declaration = objPolicyExtensions.Declaration != null ? Convert.ToBoolean(objPolicyExtensions.Declaration) : false;
                        ObjPolicy.ProposalNeed = objPolicyExtensions.ProposalNeed;

                    }
                    #endregion

                    #region Premium Info

                    var PremiumInfo = objpolicy.tblProposalPremiums.FirstOrDefault();
                    //if (objpolicy.PolicyStageStatusID == 2376)
                    //{
                    //    PremiumInfo = objpolicy.tblProposalPremiums.Where(a => a.isDeleted == true).FirstOrDefault();
                    //}

                    if (PremiumInfo != null)
                    {

                        ObjPolicy.MonthlyPremium = PremiumInfo.MonthlyPremium;
                        ObjPolicy.QuaterlyPremium = PremiumInfo.QuaterlyPremium;
                        ObjPolicy.HalfYearlyPremium = PremiumInfo.HalfYearlyPremium;
                        ObjPolicy.AnnualPremium = Convert.ToInt64(PremiumInfo.AnnualPremium);
                        ObjPolicy.VAT = PremiumInfo.VAT;
                        ObjPolicy.Cess = PremiumInfo.Cess;
                        ObjPolicy.PolicyFee = PremiumInfo.PolicyFee;
                        ObjPolicy.AdditionalPremium = Convert.ToDecimal(PremiumInfo.AdditionalPremium);

                        ObjPolicy.ProposalPayablePremium = Convert.ToString(ObjPolicy.AnnualPremium + ObjPolicy.AdditionalPremium);
                        if (ObjPolicy.PaymentFrequency == "1")
                        {
                            ObjPolicy.Premium = ObjPolicy.MonthlyPremium;
                        }
                        else if (ObjPolicy.PaymentFrequency == "4")
                        {
                            ObjPolicy.Premium = ObjPolicy.QuaterlyPremium;
                        }
                        else if (ObjPolicy.PaymentFrequency == "2")
                        {
                            ObjPolicy.Premium = ObjPolicy.HalfYearlyPremium;
                        }
                        else if (ObjPolicy.PaymentFrequency == "12")
                        {
                            ObjPolicy.Premium = ObjPolicy.AnnualPremium;
                        }

                    }
                    #endregion

                    #region Added for Demo UnderWriter

                    var PolicyRemarks = objpolicy.tblPolicyUWRemarks.FirstOrDefault();
                    if (PolicyRemarks != null)
                    {
                        ObjPolicy.Decision = PolicyRemarks.Decision;
                        ObjPolicy.UWComments = PolicyRemarks.UWRemarks;
                    }
                    #endregion

                    #region Payment Info
                    var PolicyPaymentMapInfo = objpolicy.tblPolicyPaymentMaps.FirstOrDefault();
                    if (PolicyPaymentMapInfo != null)
                    {
                        if (PolicyPaymentMapInfo.tblPayment != null)
                        {
                            ObjPolicy.objPaymentInfo = new PaymentInfo();
                            ObjPolicy.objPaymentInfo.AmountPaid = PolicyPaymentMapInfo.tblPayment.PaidAmount;
                            ObjPolicy.objPaymentInfo.TransactionNo = PolicyPaymentMapInfo.tblPayment.TxnNo;
                            ObjPolicy.objPaymentInfo.objInstrumentDetails = (from ObjInstrumentDetails in PolicyPaymentMapInfo.tblPayment.tblPaymentInstrumentDetails
                                                                             select new InstrumentDetails
                                                                             {

                                                                                 InstrumentAmount = ObjInstrumentDetails.InstrumentAmount,
                                                                                 Name = ObjInstrumentDetails.Drawer,
                                                                                 PaymentMode = ObjInstrumentDetails.PaymentMode,
                                                                                 InstrumentNo = ObjInstrumentDetails.InstrumentNo,
                                                                                 BankBranch = ObjInstrumentDetails.BankBranch,
                                                                                 BankName = ObjInstrumentDetails.BankName,
                                                                                 InstrumentDate = ObjInstrumentDetails.CreatedDate
                                                                             }).ToList();
                        }
                    }
                    #endregion

                    #region Load Masters
                    //   ObjPolicy= objCommonBusiness.LoadMastersForProposal(ObjPolicy);
                    if (ObjPolicy.UserType == "UW")
                    {
                        ObjPolicy.LstDecision = objCommonBusiness.GetUWDecision();
                        ObjPolicy.LstDocument = objCommonBusiness.GetDocumentType();
                        ObjPolicy.LstAdditionalMedicalDocument = objCommonBusiness.GetAdditionalMedicalDocument();
                        ObjPolicy.LstAdditionalNonMedicalDocument = objCommonBusiness.GetAdditionalNonMedicalDocument();
                        ObjPolicy.LstUWName = objCommonBusiness.GetUnderWriterList(ObjPolicy.UserName, false);
                    }
                    // Added for Drop Down Values For Document Section
                    ObjPolicy.DropDownMemberDetails = GetDropDownValueForMemberDetails(ObjPolicy);
                    // Till here

                    ObjPolicy.LstAgeProof = objCommonBusiness.GetAgeProof();


                    //ObjPolicy.lstPAQAssets = (from CommonType in Context.tblMasCommonTypes.Where(a => a.MasterType == "PAQAssets")
                    //                          select new MasterListItem
                    //                          {
                    //                              ID = CommonType.CommonTypesID,
                    //                              Text = CommonType.Description
                    //                          }).ToList();
                    //ObjPolicy.lstPAQLiabilities = (from CommonType in Context.tblMasCommonTypes.Where(a => a.MasterType == "PAQLiabilities")
                    //                               select new MasterListItem
                    //                               {
                    //                                   ID = CommonType.CommonTypesID,
                    //                                   Text = CommonType.Description
                    //                               }).ToList();
                    ObjPolicy.lstPAQAssets = objCommonBusiness.GetPAQAssets();
                    ObjPolicy.lstPAQLiabilities = objCommonBusiness.GetPAQLiabilities();


                    #endregion

                    if (ObjPolicy.objFillMemberDetials == null)
                        ObjPolicy.objFillMemberDetials = new MemberDetails();
                    if (ObjPolicy.objFillMemberDetials.objCommunicationAddress == null)
                        ObjPolicy.objFillMemberDetials.objCommunicationAddress = new Address();
                    if (ObjPolicy.objFillMemberDetials.objPermenantAddress == null)
                        ObjPolicy.objFillMemberDetials.objPermenantAddress = new Address();
                    ObjPolicy.ReferralReason = UWDeviationMessage;
                }
                return ObjPolicy;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AIA.Life.Models.Policy.ProposalMasters LoadProposalMasters()
        {
            CommonBusiness objCommmon = new CommonBusiness();
            return objCommmon.LoadProposalMasters();
        }

        public AIA.Life.Models.Policy.Policy MapMasters(AIA.Life.Models.Policy.ProposalMasters objProposalMasters, AIA.Life.Models.Policy.Policy ObjPolicy)
        {
            ObjPolicy.lstRelations = objProposalMasters.lstRelations;
            ObjPolicy.lstBeneficiaryRelations = objProposalMasters.lstBeneficiaryRelations;
            ObjPolicy.Nationalities = objProposalMasters.Nationalities;
            ObjPolicy.lstProposalNeed = objProposalMasters.lstProposalNeed;
            ObjPolicy.ProposalMaritalStatuslist = objProposalMasters.ProposalMaritalStatuslist;
            ObjPolicy.lstRelationshipwithpolicyowner = objProposalMasters.lstRelationshipwithpolicyowner;
            ObjPolicy.MaritalStatuslist = objProposalMasters.MaritalStatuslist;
            ObjPolicy.lstGender = objProposalMasters.lstGender;
            ObjPolicy.lstSalutation = objProposalMasters.lstSalutation;
            ObjPolicy.lstBeneficiarySalutation = objProposalMasters.lstBeneficiarySalutation;
            ObjPolicy.lstOccupation = objProposalMasters.lstOccupation;
            ObjPolicy.lstMemberOccupation = objProposalMasters.lstMemberOccupation;
            ObjPolicy.lstDocumentName = objProposalMasters.lstDocumentName;
            ObjPolicy.lstLanguage = objProposalMasters.lstLanguage;
            ObjPolicy.ListPlan = objProposalMasters.ListPlan;
            ObjPolicy.LstPolicyTerm = objProposalMasters.LstPolicyTerm;
            ObjPolicy.LstEasyPensionPolicyTerm = objProposalMasters.LstEasyPensionPolicyTerm;
            ObjPolicy.LstSmartPensionPolicyTerm = objProposalMasters.LstSmartPensionPolicyTerm;
            ObjPolicy.LstSmartBuilderPolicyTerm = objProposalMasters.LstSmartBuilderPolicyTerm;
            ObjPolicy.LstSmartBuilderPaymentTerm = objProposalMasters.LstSmartBuilderPaymentTerm;
            ObjPolicy.LstSmartBuilderGoldPolicyTerm = objProposalMasters.LstSmartBuilderGoldPolicyTerm;
            ObjPolicy.LstSmartBuilderGoldPaymentTerm = objProposalMasters.LstSmartBuilderGoldPaymentTerm;
            ObjPolicy.LstPriorityValuePolicyTerm = objProposalMasters.LstPriorityValuePolicyTerm;
            ObjPolicy.LstPriorityValuePaymentTerm = objProposalMasters.LstPriorityValuePaymentTerm;
            ObjPolicy.LstBenefitYears = objProposalMasters.LstBenefitYears;
            ObjPolicy.LstPaymentfrequency = objProposalMasters.LstPaymentfrequency;
            ObjPolicy.LstPaymentMethod = objProposalMasters.LstPaymentMethod;
            ObjPolicy.LstPaymentRelations = objProposalMasters.LstPaymentRelations;
            ObjPolicy.lstCauseOfDeath = objProposalMasters.lstCauseOfDeath;
            ObjPolicy.lstSateofHealth = objProposalMasters.lstSateofHealth;
            ObjPolicy.lstFamilyBackGroundRelationship = objProposalMasters.lstFamilyBackGroundRelationship;
            ObjPolicy.LstModeofCommunication = objProposalMasters.LstModeofCommunication;
            ObjPolicy.LstPreferredReceipt = objProposalMasters.LstPreferredReceipt;
            ObjPolicy.LstMaturityBenefits = objProposalMasters.LstMaturityBenefits;
            ObjPolicy.LstResidentialStatus = objProposalMasters.LstResidentialStatus;
            ObjPolicy.LstFillMemberCountryofOccupation = objProposalMasters.LstFillMemberCountryofOccupation;
            ObjPolicy.LstLifeType = objProposalMasters.LstLifeType;
            ObjPolicy.LstUWStatus = objProposalMasters.LstUWStatus;
            ObjPolicy.lstSmokeAndAlcholQuantity = objProposalMasters.lstSmokeAndAlcholQuantity;
            ObjPolicy.LstHeightFeets = objProposalMasters.LstHeightFeets;
            ObjPolicy.LstSelectedMedicalDocuments = objProposalMasters.LstSelectedMedicalDocuments;
            ObjPolicy.lstCauseOfDeath = objProposalMasters.lstCauseOfDeath;
            ObjPolicy.lstSateofHealth = objProposalMasters.lstSateofHealth;
            ObjPolicy.lstFamilyBackGroundRelationship = objProposalMasters.lstFamilyBackGroundRelationship;
            ObjPolicy.lstDependentRelationship = objProposalMasters.lstDependentRelationship;
            ObjPolicy.LstHeightFeets = objProposalMasters.LstHeightFeets;
            ObjPolicy.lstSmokeAndAlcholPer = objProposalMasters.lstSmokeAndAlcholPer;
            ObjPolicy.lstSmokeTypes = objProposalMasters.lstSmokeTypes;
            ObjPolicy.lstAlcoholTypes = objProposalMasters.lstAlcoholTypes;
            ObjPolicy.lstCurrentStatus = objProposalMasters.lstCurrentStatus;
            ObjPolicy.lstAerobicExercise = objProposalMasters.lstAerobicExercise;
            ObjPolicy.lstFruitOrVegetablePortions = objProposalMasters.lstFruitOrVegetablePortions;
            ObjPolicy.lstFluidOrWater = objProposalMasters.lstFluidOrWater;
            ObjPolicy.lstDoctorNames = objProposalMasters.lstDoctorNames;
            ObjPolicy.lstLabNames = objProposalMasters.lstLabNames;
            ObjPolicy.LstHealthCheckupCategory = objProposalMasters.LstHealthCheckupCategory;
            ObjPolicy.LstMaturityBenefits = objProposalMasters.LstMaturityBenefits;
            ObjPolicy.LstResidentialStatus = objProposalMasters.LstResidentialStatus;
            return ObjPolicy;
        }

        public List<AIA.Life.Models.Common.BenifitDetails> LoadMasProposalBenifits(string PlanID, string AssuredMember, string relationship)
        {
            List<AIA.Life.Models.Common.BenifitDetails> lstBebefit = new List<AIA.Life.Models.Common.BenifitDetails>();
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            lstBebefit = (from Benefit in Context.tblMasBenefits.Where(a => a.IsDeleted != true)
                          select new AIA.Life.Models.Common.BenifitDetails()
                          {

                              BenifitName = Benefit.BenefitName,
                              AssuredMember = AssuredMember,
                              BenefitID = Benefit.BenefitID,
                              BenifitOpted = false,
                              RelationshipWithProspect = relationship

                          }).ToList();

            return lstBebefit;
        }

        public List<AIA.Life.Models.Common.BenifitDetails> LoadMasProposalBenifits(string PlanID, string AssuredMember)
        {
            List<AIA.Life.Models.Common.BenifitDetails> lstBebefit = new List<AIA.Life.Models.Common.BenifitDetails>();
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            lstBebefit = (from Benefit in Context.tblMasBenefits.Where(a => a.IsDeleted != true)
                          select new AIA.Life.Models.Common.BenifitDetails()
                          {

                              BenifitName = Benefit.BenefitName,
                              AssuredMember = AssuredMember,
                              BenefitID = Benefit.BenefitID,
                              BenifitOpted = false

                          }).ToList();


            return lstBebefit;
        }
        public AIA.Life.Models.EmailSMSDetails.EmailDetails Email(AIA.Life.Models.EmailSMSDetails.EmailDetails objEmail)
        {
            EmailIntegration objEmailIntegration = new EmailIntegration();
            bool obj = objEmailIntegration.EmailNotification(objEmail);
            if (obj == true)
            {
                objEmail.Response = "Email Sent Successfully";
            }
            else
            {
                objEmail.Response = "Some Error Occurred";
            }
            return objEmail;
        }
        public AIA.Life.Models.EmailSMSDetails.SMSDetails SMS(AIA.Life.Models.EmailSMSDetails.SMSDetails objSMS)
        {
            SMSIntegration objSMSIntegration = new SMSIntegration();
            bool obj = objSMSIntegration.SMSNotification(objSMS);
            if (obj == true)
            {
                objSMS.Response = "SMS Sent Successfully";
            }
            else
            {
                objSMS.Response = "Some Error Occurred";
            }
            return objSMS;
        }
        public AIA.Life.Models.Policy.Policy LoadProposalBenefits(AIA.Life.Models.Policy.Policy objProposal)
        {
            objProposal.LstBenifitDetails = new List<AIA.Life.Models.Common.BenifitDetails>();
            objProposal.ListAssured = new List<string>();

            if (objProposal.objMemberDetails != null)
            {
                if (objProposal.objMemberDetails.Where(a => a.RelationShipWithPropspect == "267").Count() > 0)
                {
                    objProposal.LstBenifitDetails.AddRange(LoadMasProposalBenifits(string.Empty, "LifeAssured1", "267"));
                    objProposal.ListAssured.Add("LifeAssured1");
                    if (objProposal.objMemberDetails.Where(a => a.RelationShipWithPropspect == "268").Count() > 0)
                    {
                        objProposal.LstBenifitDetails.AddRange(LoadMasProposalBenifits(string.Empty, "LifeAssured2", "268"));
                        objProposal.ListAssured.Add("LifeAssured2");
                    }
                    int count = 1;
                    foreach (var item in objProposal.objMemberDetails.Where(a => a.RelationShipWithPropspect != "267" && a.RelationShipWithPropspect != "268"))
                    {
                        string Child = "Child" + count;

                        objProposal.LstBenifitDetails.AddRange(LoadMasProposalBenifits(string.Empty, Child, "271"));
                        objProposal.ListAssured.Add(Child);
                        count++;
                    }
                }
                else if (objProposal.objMemberDetails.Where(a => a.RelationShipWithPropspect == "268").Count() > 0)
                {
                    objProposal.LstBenifitDetails.AddRange(LoadMasProposalBenifits(string.Empty, "LifeAssured1", "268"));
                    objProposal.ListAssured.Add("LifeAssured1");
                    int count = 1;
                    foreach (var item in objProposal.objMemberDetails.Where(a => a.RelationShipWithPropspect != "267" && a.RelationShipWithPropspect != "268"))
                    {
                        string Child = "Child" + count;

                        objProposal.LstBenifitDetails.AddRange(LoadMasProposalBenifits(string.Empty, Child, "271"));
                        objProposal.ListAssured.Add(Child);
                        count++;
                    }
                }
            }
            return objProposal;
        }

        public int IdentifyMainLife(List<tblPolicyMemberDetail> objMemberDetails)
        {

            if (objMemberDetails.Where(a => a.RelationShipWithProposer == 267).Count() > 0)
            {
                return 267;
            }
            else if (objMemberDetails.Where(a => a.RelationShipWithProposer == 268).Count() > 0)
            {
                return 268;
            }
            else
            {
                return 0;
            }
        }


        //public void DeleteExisitingPolicyDocuments(DocumentUploadFile Document, decimal PolicyId)
        //{
        //    try
        //    {
        //        AVOAIALifeEntities Context = new AVOAIALifeEntities();
        //        var objPOlicyDocumnets = Context.tblPolicyDocuments.Where(a => a.PolicyID == PolicyId && a.ItemType == "PolicyDocuments" && a.DocumentUploadID == Document.DOCID).FirstOrDefault();
        //        if (objPOlicyDocumnets != null)
        //        {
        //            tblPolicyDocument objtblPolicyDocument = new tblPolicyDocument();
        //            objtblPolicyDocument.DocumentUploadID = Document.DOCID;
        //            objtblPolicyDocument.PolicyID = PolicyId;
        //            objtblPolicyDocument.DocumentType = objPOlicyDocumnets.DocumentType;
        //            objtblPolicyDocument.FileName = Document.FileName;
        //            objtblPolicyDocument.CreatedDate = objPOlicyDocumnets.CreatedDate;
        //            if (!String.IsNullOrEmpty(Document.FileName))
        //            {
        //                objtblPolicyDocument.FilePath = objPOlicyDocumnets.FilePath;

        //            }
        //            objtblPolicyDocument.ItemType = "PolicyDocuments";
        //            objtblPolicyDocument.MemberType = Document.MemberType;
        //        }
        //        else
        //        {

        //        }
        //        tblPolicyDocument.Add()
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        /// <summary>
        /// Save UW Remarks 
        /// </summary>
        /// <param name="objPolicy"></param>
        /// <returns></returns>
        public AIA.Life.Models.Policy.Policy SaveUWRemarks(AIA.Life.Models.Policy.Policy objPolicy)
        {

            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {

                    var UserInfo = Context.AspNetUsers.Where(a => a.UserName == objPolicy.UserName).FirstOrDefault();
                    if (UserInfo != null)
                    {
                        tblPolicy objtblPolicy = new tblPolicy();
                        tblPolicyUWRemark objtblPolicyUWRemark = new tblPolicyUWRemark();
                        Guid CommonGuid = Guid.NewGuid();
                        objtblPolicy = Context.tblPolicies.Where(a => a.ProposalNo == objPolicy.ProposalNo).FirstOrDefault();
                        if (objtblPolicy != null)
                        {
                            objPolicy.CreatedBy = Context.tblUserDetails.Where(a => a.UserID.ToString() == objtblPolicy.Createdby).Select(a => a.LoginID).FirstOrDefault();
                            objPolicy.QuoteNo = objtblPolicy.QuoteNo;
                            objtblPolicy.ModifiedDate = DateTime.Now;
                            objtblPolicy.PolicyIssueDate = objPolicy.RiskCommencementDate;
                            objtblPolicy.PolicyStartDate = objPolicy.RiskCommencementDate;
                            objtblPolicy.PolicyEndDate = objPolicy.RiskCommencementDate.AddYears(Convert.ToInt32(objtblPolicy.PolicyTerm));
                            AddMemberLevelDecision(objPolicy, CommonGuid);

                            if (!objPolicy.IsIntermSave)
                            { AddMemberDeviationInfoHistory(objPolicy, CommonGuid); }

                            #region Policy Level UW Remarks
                            objtblPolicyUWRemark = objtblPolicy.tblPolicyUWRemarks.FirstOrDefault();
                            if (objtblPolicyUWRemark == null)
                            {
                                objtblPolicyUWRemark = new tblPolicyUWRemark();
                            }
                            objtblPolicyUWRemark.UWRemarks = objPolicy.UWComments;
                            objtblPolicyUWRemark.Decision = objPolicy.Decision;
                            objtblPolicyUWRemark.CreateBy = UserInfo.Id;
                            objtblPolicyUWRemark.CreatedDate = DateTime.Now;
                            objtblPolicyUWRemark.IsExclusion = objPolicy.IsExlusions;
                            objtblPolicyUWRemark.IsLoading = objPolicy.IsLoading;
                            objtblPolicyUWRemark.Loading = objPolicy.LoadingDetails;
                            objtblPolicyUWRemark.NoofDays = objPolicy.NoofDays;
                            int PolicyStageStatusID = CrossCuttingConstants.PolicyStageStatusReferToUW; // Pending With UW
                            if (!string.IsNullOrEmpty(objPolicy.Decision))
                            {
                                if (objPolicy.Decision == CrossCuttingConstants.UWDecisionAccepted)// Accept 
                                {
                                    PolicyStageStatusID = CrossCuttingConstants.PolicyStageStatusIssued; //issued
                                    objtblPolicy.PolicyNo = objPolicy.ProposalNo;
                                }
                                else if (objPolicy.Decision == CrossCuttingConstants.UWDecisionAcceptwithloading)//  Accept with loading
                                {
                                    PolicyStageStatusID = CrossCuttingConstants.PolicyStageStatusIssued; //issued
                                    objtblPolicy.PolicyNo = objPolicy.ProposalNo;                                          // PolicyStageStatusID = 195; 
                                }
                                else if (objPolicy.Decision == CrossCuttingConstants.UWDecisionDecline)//  Decline 
                                {
                                    PolicyStageStatusID = CrossCuttingConstants.PolicyStageStatusDecline; //Decline

                                }
                                else if (objPolicy.Decision == CrossCuttingConstants.UWDecisionPostPone)// Postpone
                                {
                                    PolicyStageStatusID = CrossCuttingConstants.PolicyStageStatusPostPone; //Postpone

                                }
                                else if (objPolicy.Decision == CrossCuttingConstants.UWDecisionWithDrawn)//   with Drawn
                                {
                                    PolicyStageStatusID = CrossCuttingConstants.PolicyStageStatusWithDrawn; //with Drawn

                                }
                                else if (objPolicy.Decision == CrossCuttingConstants.UWDecisionReferToUW)//  Refer to UW
                                {
                                    PolicyStageStatusID = CrossCuttingConstants.PolicyStageStatusReferToUW; //Refer TO UW

                                    #region Allocation  In Case of Refer To UW
                                    var POlicyAllcation = Context.tblPolicyUWAllocations.Where(a => a.PolicyID == objtblPolicy.PolicyID).FirstOrDefault();
                                    if (POlicyAllcation != null && !objPolicy.IsIntermSave)
                                    {
                                        POlicyAllcation.AllocatedFrom = POlicyAllcation.AllocatedTo;
                                        POlicyAllcation.AllocatedTo = objPolicy.UnderwriterName;
                                    }
                                    #endregion
                                }
                                else if (objPolicy.Decision == CrossCuttingConstants.UWDecisionOutStandingReq)// Outstanding Requirement
                                {
                                    PolicyStageStatusID = CrossCuttingConstants.PolicyStageStatusOutStandingReq; //Outstanding Requirement
                                }
                                else if (objPolicy.Decision == CrossCuttingConstants.UWDecisionCounterOffer)// Counter Offer
                                {
                                    PolicyStageStatusID = CrossCuttingConstants.PolicyStageStatusCounterOffer; //Counter Offer
                                    objPolicy.IsCLALetterReq = false;
                                    objPolicy.IsIllustartionReq = false;
                                    #region Set PlanCode
                                    var PlanInfo = Context.tblMasProductPlans.Where(a => a.PlanId == objtblPolicy.PlanID).FirstOrDefault();
                                    if (PlanInfo != null)
                                    {
                                        objPolicy.PlanCode = PlanInfo.PlanCode;
                                    }
                                    var generateQuote = objtblPolicy.tblPolicyCLAQuotes.ToList();
                                    if (generateQuote.Count >= 1)
                                    {
                                        //objPolicy.IsIllustartionReq = true;
                                        objPolicy.IsClaGenerateQuote = true;
                                    }
                                    #endregion

                                    foreach (var _Member in Context.tblPolicyMemberDetails.Where(a => a.PolicyID == objtblPolicy.PolicyID).ToList())
                                    {
                                        foreach (var RiderInfo in _Member.tblPolicyMemberBenefitDetails.ToList())
                                        {
                                            var MasRiderInfo = Context.tblProductPlanRiders.Where(a => a.ProductPlanRiderId == RiderInfo.BenifitID).FirstOrDefault();

                                            #region Check For Letters That Need to be Generated in Counter Offer Case
                                            if (MasRiderInfo != null)
                                            {

                                                var MemberOtherBeniftDetails = Context.tblMemberBenefitOtherDetails.Where(a => a.MemberBenifitID == RiderInfo.MemberBenifitID).FirstOrDefault();
                                                if (MemberOtherBeniftDetails != null)// Check If Loading Exisits For that Rider
                                                {
                                                    #region Basic Life Cover
                                                    if (MasRiderInfo.DisplayName == "Basic Life Cover")
                                                    {
                                                        if ((MemberOtherBeniftDetails.LoadingBasis == "2205" ||
                                                            MemberOtherBeniftDetails.LoadingBasis == "2206" ||
                                                            MemberOtherBeniftDetails.LoadingBasis == "2207") &&
                                                            (MemberOtherBeniftDetails.LoadingAmount != "" ||
                                                            MemberOtherBeniftDetails.LoadingAmount != null)) //(Health / Occ / Res)
                                                        {
                                                            objPolicy.IsCLALetterReq = true;
                                                            objPolicy.IsIllustartionReq = true;
                                                            // CLA Letter + Illustration
                                                        }
                                                    }
                                                    #endregion
                                                    #region Other Than Basic life cover
                                                    else if (MasRiderInfo.DisplayName != "Basic Life Cover")
                                                    {
                                                        if ((MemberOtherBeniftDetails.LoadingBasis == "2205" ||
                                                            MemberOtherBeniftDetails.LoadingBasis == "2206") &&
                                                            (MemberOtherBeniftDetails.LoadingAmount != "" ||
                                                            MemberOtherBeniftDetails.LoadingAmount != null)) //(Occ / Res)
                                                        {
                                                            // No Letter
                                                        }
                                                        else if (MemberOtherBeniftDetails.LoadingBasis == "2207" &&
                                                            (MemberOtherBeniftDetails.LoadingAmount != "" ||
                                                            MemberOtherBeniftDetails.LoadingAmount != null)) //(Health)
                                                        {
                                                            objPolicy.IsCLALetterReq = true;
                                                        }

                                                    }
                                                    #endregion
                                                    #region Exclusion
                                                    if ((MemberOtherBeniftDetails.LoadingBasis == "" || MemberOtherBeniftDetails.LoadingBasis == null) &&
                                                                                                       (MemberOtherBeniftDetails.Exclusion != "" ||
                                                                                                       MemberOtherBeniftDetails.Exclusion != null)) //(Exclusion Case)
                                                    {
                                                        objPolicy.IsCLALetterReq = true;
                                                    }
                                                    #endregion


                                                }
                                                #endregion

                                            }
                                        }
                                    }
                                }
                                else if (objPolicy.Decision == CrossCuttingConstants.UWDecisionNotTaken)//  Not Taken Up
                                {
                                    PolicyStageStatusID = CrossCuttingConstants.PolicyStageStatusnotTaken; //Not Taken Up

                                }

                                //PolicyStageStatusID = Convert.ToInt32(objPolicy.Decision);
                            }

                            if (!objPolicy.IsIntermSave && !objPolicy.CounterOfferCase)
                            { objtblPolicy.PolicyStageStatusID = PolicyStageStatusID; }

                            if (objtblPolicyUWRemark.RemarkID == decimal.Zero)
                            {
                                objtblPolicyUWRemark.PolicyID = objtblPolicy.PolicyID;
                                Context.tblPolicyUWRemarks.Add(objtblPolicyUWRemark);
                            }
                            #endregion

                            #region Policy Level UW Remarks History
                            if (!objPolicy.IsIntermSave)
                            {
                                tblPolicyUWRemarkHistory objpolicyRemarkHistory = new tblPolicyUWRemarkHistory();
                                objpolicyRemarkHistory.PolicyID = objtblPolicy.PolicyID;
                                objpolicyRemarkHistory.Remarks = objPolicy.UWComments;
                                objpolicyRemarkHistory.Decision = objPolicy.Decision;
                                objpolicyRemarkHistory.CreatedBy = UserInfo.Id;
                                objpolicyRemarkHistory.CreatedDate = DateTime.Now;
                                objpolicyRemarkHistory.IsLoading = objPolicy.IsLoading;
                                objpolicyRemarkHistory.Loading = objPolicy.LoadingDetails;
                                objpolicyRemarkHistory.CommonID = CommonGuid;
                                Context.tblPolicyUWRemarkHistories.Add(objpolicyRemarkHistory);
                            }
                            #endregion

                            #region Proposal Illustration
                            PremiumCalculation.Illustration illustration = new PremiumCalculation.Illustration();
                            objPolicy = illustration.GetProposalIllustration(objPolicy);
                            SavePolicyIllustration(objPolicy);
                            #endregion
                            Context.SaveChanges();
                            if (!objPolicy.IsIntermSave)
                            {
                                objPolicy = UpdateProposalStatusToIL(Context, objPolicy, PolicyStageStatusID);
                            }
                        }
                        if (string.IsNullOrEmpty(objPolicy.Message))
                        {
                            objPolicy.Message = "Success";
                        }
                    }


                    return objPolicy;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public AIA.Life.Models.Policy.Policy UpdateProposalStatusToIL(AVOAIALifeEntities Context, AIA.Life.Models.Policy.Policy policy, int PolicyStageStatusID)
        {
            try
            {
                if (PolicyStageStatusID == CrossCuttingConstants.PolicyStageStatusPostPone
                || PolicyStageStatusID == CrossCuttingConstants.PolicyStageStatusDecline
                || PolicyStageStatusID == CrossCuttingConstants.PolicyStageStatusnotTaken
                || PolicyStageStatusID == CrossCuttingConstants.PolicyStageStatusWithDrawn)
                {
                    AIA.Life.Models.Policy.Policy objPolicy = new AIA.Life.Models.Policy.Policy();
                    objPolicy.ProposalNo = policy.ProposalNo;
                    objPolicy.QuoteNo = policy.QuoteNo;
                    objPolicy.PolicyID = policy.PolicyID;
                    objPolicy.UserType = "UW";
                    objPolicy = FetchProposalInfo(objPolicy);
                    objPolicy = (AIA.Life.Models.Policy.Policy)IL.UpdatePolicyNotes(objPolicy);
                    Thread.Sleep(2000);
                    objPolicy = (AIA.Life.Models.Policy.Policy)IL.WithdrawProposal(objPolicy);
                }
                if (PolicyStageStatusID == CrossCuttingConstants.PolicyStageStatusIssued)
                {
                    AIA.Life.Models.Policy.Policy objPolicy = new AIA.Life.Models.Policy.Policy();
                    objPolicy.ProposalNo = policy.ProposalNo;
                    objPolicy.QuoteNo = policy.QuoteNo;
                    objPolicy.PolicyID = policy.PolicyID;
                    objPolicy.UserType = "UW";
                    objPolicy = FetchProposalInfo(objPolicy);
                    objPolicy = (AIA.Life.Models.Policy.Policy)IL.UpdatePolicyNotes(objPolicy);
                    for (int i = 0; i < objPolicy.objMemberDetails.Count; i++)//refreshing the riders if cess date is not matching
                    {
                        if (!string.IsNullOrEmpty(objPolicy.objMemberDetails[i].ClientCode))
                        {
                            objPolicy.objMemberDetails[i].ProposalNo = objPolicy.ProposalNo;
                            objPolicy.objMemberDetails[i].PolicyTerm = Convert.ToInt32(objPolicy.PolicyTerm);
                            objPolicy.objMemberDetails[i].PremiumTerm = Convert.ToInt32(objPolicy.PaymentTerm);
                            objPolicy.objMemberDetails[i].PensionTerm = Convert.ToInt32(objPolicy.SmartPensionReceivingPeriod);
                            objPolicy.objMemberDetails[i].MonthlySavingBenifit = Convert.ToInt32(objPolicy.SmartPensionMonthlyIncome);
                            objPolicy.objMemberDetails[i].Deductible = objPolicy.Deductible;
                            objPolicy.objMemberDetails[i].MaturityBenefit = objPolicy.MaturityBenefits;
                            objPolicy.objMemberDetails[i].LifeNum = "0" + (i + 1).ToString();
                            objPolicy.objMemberDetails[i] = (MemberDetails)IL.RefreshRiders(objPolicy.objMemberDetails[i]);
                            Thread.Sleep(3000);
                        }
                    }
                    if (string.IsNullOrEmpty(objPolicy.Error.ErrorMessage))
                    {
                        AIA.Life.Models.Payment.PaymentModel paymentModel = new AIA.Life.Models.Payment.PaymentModel();
                        paymentModel.ProposalNo = objPolicy.ProposalNo;
                        paymentModel = (AIA.Life.Models.Payment.PaymentModel)IL.ProposalPreIssueValidation(paymentModel); //calling pre issue to complete check of follow ups
                        tblPolicy tblPolicyUpdate = Context.tblPolicies.Where(a => a.ProposalNo == objPolicy.ProposalNo).FirstOrDefault();
                        
                        objPolicy = (AIA.Life.Models.Policy.Policy)IL.ProposalFollowupEnquiry(objPolicy);
                        if (objPolicy.FollowUps.Count() > 0)
                            objPolicy = (AIA.Life.Models.Policy.Policy)IL.ProposalFollowupModify(objPolicy);
                        Thread.Sleep(2000);
                        paymentModel = (AIA.Life.Models.Payment.PaymentModel)IL.ProposalPreIssueValidation(paymentModel);// checking pre issue validations after waiving off the follow ups
                        foreach (var item in paymentModel.PreIssueValidations)
                        {
                            if (item.ToUpper().Contains("REVISIT FACULT"))
                            {
                                policy.Message = "Success";
                                policy.Error.ErrorMessage = "Please issue the policy manually in IL as it is required to revisit with Reassurance Faculty";
                                return policy;
                            }
                        }
                        if (string.IsNullOrEmpty(objPolicy.Error.ErrorMessage) && paymentModel.PreIssueValidations.Count <= 1)
                        {
                            Thread.Sleep(30000);
                            objPolicy.BizDate = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
                            objPolicy = (AIA.Life.Models.Policy.Policy)IL.ProposalUWApproval(objPolicy);
                            if (string.IsNullOrEmpty(objPolicy.Error.ErrorMessage))
                            {
                                objPolicy = (AIA.Life.Models.Policy.Policy)IL.QualityControl(objPolicy);
                                if (string.IsNullOrEmpty(objPolicy.Error.ErrorMessage))
                                {
                                    objPolicy = (AIA.Life.Models.Policy.Policy)IL.ProposalIssuance(objPolicy);
                                    policy.Message = "Success";
                                }
                                else
                                {
                                    policy.Message = "Error";
                                    policy.Error.ErrorMessage = "Some error occurred, Failed in Quality Check from IL";
                                    tblPolicyUpdate.PolicyStageStatusID = CrossCuttingConstants.PolicyStageStatusReferToUW;
                                    Context.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            policy.Message = "Error";
                            policy.Error.ErrorMessage = "Some error occurred, Pending Pre issue validations from IL";
                            tblPolicyUpdate.PolicyStageStatusID = CrossCuttingConstants.PolicyStageStatusReferToUW;
                            Context.SaveChanges();
                        }
                    }
                    else
                    {
                        tblPolicy tblPolicyUpdate = Context.tblPolicies.Where(a => a.ProposalNo == objPolicy.ProposalNo).FirstOrDefault();
                        tblPolicyUpdate.PolicyStageStatusID = CrossCuttingConstants.PolicyStageStatusReferToUW;
                        Context.SaveChanges();
                        policy.Message = "Error";
                        policy.Error.ErrorMessage = "Some error occurred, Policy notes are not updated.";
                    }
                }
                if (PolicyStageStatusID == CrossCuttingConstants.PolicyStageStatusCounterOffer)
                {
                    AIA.Life.Models.Policy.Policy objPolicy = new AIA.Life.Models.Policy.Policy();
                    objPolicy.ProposalNo = policy.ProposalNo;
                    objPolicy.QuoteNo = policy.QuoteNo;
                    objPolicy.PolicyID = policy.PolicyID;
                    objPolicy.UserType = "UW";
                    objPolicy = FetchProposalInfo(objPolicy);
                    objPolicy = (AIA.Life.Models.Policy.Policy)IL.UpdatePolicyNotes(objPolicy);
                    for (int i = 0; i < objPolicy.objMemberDetails.Count; i++)//refreshing the riders if cess date is not matching
                    {
                        if (!string.IsNullOrEmpty(objPolicy.objMemberDetails[i].ClientCode))
                        {
                            objPolicy.objMemberDetails[i].ProposalNo = objPolicy.ProposalNo;
                            objPolicy.objMemberDetails[i].PolicyTerm = Convert.ToInt32(objPolicy.PolicyTerm);
                            objPolicy.objMemberDetails[i].PremiumTerm = Convert.ToInt32(objPolicy.PaymentTerm);
                            objPolicy.objMemberDetails[i].PensionTerm = Convert.ToInt32(objPolicy.SmartPensionReceivingPeriod);
                            objPolicy.objMemberDetails[i].MonthlySavingBenifit = Convert.ToInt32(objPolicy.SmartPensionMonthlyIncome);
                            objPolicy.objMemberDetails[i].Deductible = objPolicy.Deductible;
                            objPolicy.objMemberDetails[i].MaturityBenefit = objPolicy.MaturityBenefits;
                            objPolicy.objMemberDetails[i].LifeNum = "0" + (i + 1).ToString();
                            objPolicy.objMemberDetails[i] = (MemberDetails)IL.RefreshRiders(objPolicy.objMemberDetails[i]);
                            Thread.Sleep(3000);
                        }
                    }
                }

            }
            catch (Exception ilEx)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ilEx);
                policy.Message = "Failure";
            }
            return policy;
        }
        public AIA.Life.Models.Policy.Policy SaveBeforeClaGenerateQuote(AIA.Life.Models.Policy.Policy objPolicy)
        {

            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {

                    var UserInfo = Context.AspNetUsers.Where(a => a.UserName == objPolicy.UserName).FirstOrDefault();
                    if (UserInfo != null)
                    {
                        tblPolicy objtblPolicy = new tblPolicy();
                        tblPolicyUWRemark objtblPolicyUWRemark = new tblPolicyUWRemark();
                        Guid CommonGuid = Guid.NewGuid();
                        objtblPolicy = Context.tblPolicies.Where(a => a.ProposalNo == objPolicy.ProposalNo).FirstOrDefault();
                        if (objtblPolicy != null)
                        {
                            objPolicy.CreatedBy = Context.tblUserDetails.Where(a => a.UserID.ToString() == objtblPolicy.Createdby).Select(a => a.LoginID).FirstOrDefault();
                            objPolicy.QuoteNo = objtblPolicy.QuoteNo;
                            objtblPolicy.ModifiedDate = DateTime.Now;
                            objtblPolicy.PolicyIssueDate = objPolicy.RiskCommencementDate;
                            objtblPolicy.PolicyStartDate = objPolicy.RiskCommencementDate;
                            objtblPolicy.PolicyEndDate = objPolicy.RiskCommencementDate.AddYears(Convert.ToInt32(objtblPolicy.PolicyTerm));
                            AddMemberLevelDecision(objPolicy, CommonGuid);

                            if (!objPolicy.IsIntermSave)
                            { AddMemberDeviationInfoHistory(objPolicy, CommonGuid); }

                            #region Policy Level UW Remarks
                            objtblPolicyUWRemark = objtblPolicy.tblPolicyUWRemarks.FirstOrDefault();
                            if (objtblPolicyUWRemark == null)
                            {
                                objtblPolicyUWRemark = new tblPolicyUWRemark();
                            }
                            objtblPolicyUWRemark.UWRemarks = objPolicy.UWComments;
                            objtblPolicyUWRemark.Decision = objPolicy.Decision;
                            objtblPolicyUWRemark.CreateBy = UserInfo.Id;
                            objtblPolicyUWRemark.CreatedDate = DateTime.Now;
                            objtblPolicyUWRemark.IsExclusion = objPolicy.IsExlusions;
                            objtblPolicyUWRemark.IsLoading = objPolicy.IsLoading;
                            objtblPolicyUWRemark.Loading = objPolicy.LoadingDetails;
                            objtblPolicyUWRemark.NoofDays = objPolicy.NoofDays;
                            if (objtblPolicyUWRemark.RemarkID == decimal.Zero)
                            {
                                objtblPolicyUWRemark.PolicyID = objtblPolicy.PolicyID;
                                Context.tblPolicyUWRemarks.Add(objtblPolicyUWRemark);
                            }
                            #endregion
                            objtblPolicy.PolicyStageStatusID = CrossCuttingConstants.PolicyStageStatusCounterOffer;
                            Context.SaveChanges();
                        }
                        if (string.IsNullOrEmpty(objPolicy.Message))
                        {
                            objPolicy.Message = "Success";
                        }
                    }


                    return objPolicy;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Email integration on UW Decision
        /// </summary>
        /// <param name="objPolicy"></param>
        /// <param name="Status"></param>
        public bool EmailNotificationOnUWDecision(AIA.Life.Models.Policy.Policy objPolicy, string Status)
        {
            try
            {
                if (!objPolicy.IsIntermSave)
                {
                    AVOAIALifeEntities Context = new AVOAIALifeEntities();
                    Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();

                    #region Email
                    EmailIntegration ObjEmailIntegration = new EmailIntegration();
                    EmailDetails ObjEmailDetails = new EmailDetails();
                    ObjEmailDetails.EmailID = objPolicy.objProspectDetails.Email;
                    //ObjEmailDetails.Name = objPolicy.objProspectDetails.LastName;
                    ObjEmailDetails.Name = objCommonBusiness.ConverttoTitlecase(objPolicy.objProspectDetails.LastName);

                    var createdBy = Context.tblPolicies.Where(a => a.ProposalNo == objPolicy.ProposalNo).Select(a => a.Createdby).FirstOrDefault();

                    var AgentEmail = Context.tblUserDetails.Where(a => a.UserID.ToString() == createdBy).Select(a => a.Email).FirstOrDefault();
                    ObjEmailDetails.AgentEmailID = AgentEmail;
                    var Sal = objPolicy.objProspectDetails.Salutation;
                    var ESalutation = Context.tblMasCommonTypes.Where(a => a.Code == Sal && a.MasterType == "Salutation").Select(a => a.Description).FirstOrDefault();
                    var ESalu = Context.tblMasCommonTypes.Where(a => a.Description == Sal && a.MasterType == "Salutation").Select(a => a.Description).FirstOrDefault();
                    if (!String.IsNullOrEmpty(ESalutation))
                    {
                        ObjEmailDetails.Salutation = ESalutation;
                    }
                    else if (!String.IsNullOrEmpty(ESalu))
                    {
                        ObjEmailDetails.Salutation = ESalu;
                    }
                    else
                    {
                        ObjEmailDetails.Salutation = Sal;
                    }
                    #endregion

                    switch (Status)
                    {
                        case CrossCutting.CrossCuttingConstants.UWDecisionAccepted:
                        case CrossCutting.CrossCuttingConstants.UWDecisionAcceptwithloading:
                        ObjEmailDetails.Subject = "Issuance of  Life insuarnce Policy :" + objPolicy.ProposalNo + " - " + objPolicy.objProspectDetails.Salutation + ". " + objPolicy.objProspectDetails.LastName;
                        ObjEmailDetails.MailTemplate = "T010";
                        break;
                        case CrossCutting.CrossCuttingConstants.UWDecisionDecline:
                        #region Email
                        var UWDeclineDecision = string.Empty;
                        foreach (var item in objPolicy.objMemberDetails)
                        {
                            var UWDeclineDecisionCode = Context.tblMemberLevelDecisions.Where(a => a.MemberID == item.MemberID).Select(a => a.UWReason).FirstOrDefault();
                            if (!string.IsNullOrEmpty(UWDeclineDecisionCode))
                            {
                                UWDeclineDecision = Context.tblMasCommonTypes.Where(a => a.Code == UWDeclineDecisionCode && a.isDeleted == 0).Select(a => a.Description).FirstOrDefault();
                            }
                        }
                        ObjEmailDetails.Subject = "Declinature of Life insurance proposal: " + objPolicy.ProposalNo + " - " + ObjEmailDetails.Salutation + " " + ObjEmailDetails.Name;
                        ObjEmailDetails.UWDeclineDecision = UWDeclineDecision;
                        ObjEmailDetails.MailTemplate = "T007";
                        #endregion

                        #region SMS To Wealth Planner
                        //SendSMSToWP_UW(objPolicy, "S015");
                        #endregion

                        #region SMS TO Customer
                        //SendSMSToCustomer_UW(objPolicy, "S016");
                        #endregion

                        break;
                        case CrossCutting.CrossCuttingConstants.UWDecisionNotTaken:
                        #region Email
                        ObjEmailDetails.Subject = "Cancellation of Life insurance proposal: " + objPolicy.ProposalNo + " - " + ObjEmailDetails.Salutation + " " + ObjEmailDetails.Name;
                        ObjEmailDetails.MailTemplate = "T009";
                        ObjEmailDetails.WPMobileNo = Context.tblUserDetails.Where(a => a.UserID.ToString() == createdBy).Select(a => a.ContactNo).FirstOrDefault();
                        //ObjEmailDetails.WPMobileNo = Context.tblMasIMOUsers.Where(a => a.UserName == objPolicy.UserName).Select(a => a.MobileNo).FirstOrDefault(); 
                        #endregion

                        #region SMS To Wealth Planner
                        //SendSMSToWP_UW(objPolicy, "S021");
                        #endregion

                        break;
                        case CrossCutting.CrossCuttingConstants.UWDecisionPostPone:
                            #region Email
                            int duration = 0;
                            var UWDeclineDecisions = string.Empty;
                            ObjEmailDetails.Subject = "Deferment of Life insurance proposal: " + objPolicy.ProposalNo + " - " + ObjEmailDetails.Salutation + " " + ObjEmailDetails.Name;
                            //foreach (var item in objPolicy.objMemberDetails)
                            //{
                            //    duration = Convert.ToInt32(item.ObjUwDecision.UWMonth);
                            //}
                            for (int z = objPolicy.objMemberDetails.Count - 1; z >= 0; z--)
                            {
                                if (objPolicy.objMemberDetails[z].ObjUwDecision.Decision == "1449")
                                {
                                    duration = Convert.ToInt32(objPolicy.objMemberDetails[z].ObjUwDecision.UWMonth);
                                    var MemberID = objPolicy.objMemberDetails[z].MemberID;
                                    var UWDeclineDecisionCode = Context.tblMemberLevelDecisions.Where(a => a.MemberID == MemberID).Select(a => a.UWReason).FirstOrDefault();
                                    if (!string.IsNullOrEmpty(UWDeclineDecisionCode))
                                    {
                                        UWDeclineDecisions = Context.tblMasCommonTypes.Where(a => a.Code == UWDeclineDecisionCode && a.isDeleted == 0).Select(a => a.Description).FirstOrDefault();
                                    }

                                }
                            }
                            //foreach (var item in objPolicy.objMemberDetails)
                            //{
                            //    var UWDeclineDecisionCode = Context.tblMemberLevelDecisions.Where(a => a.MemberID == item.MemberID).Select(a => a.UWReason).FirstOrDefault();
                            //    if (!string.IsNullOrEmpty(UWDeclineDecisionCode))
                            //    {
                            //        UWDeclineDecision = Context.tblMasCommonTypes.Where(a => a.Code == UWDeclineDecisionCode && a.isDeleted == 0).Select(a => a.Description).FirstOrDefault();
                            //    }
                            //}
                            ObjEmailDetails.UWDeclineDecision = UWDeclineDecisions;
                            ObjEmailDetails.Duration = Convert.ToString(duration);
                            ObjEmailDetails.MailTemplate = "T008";
                            #endregion

                        #region SMS To Wealth Planner
                        //SendSMSToWP_UW(objPolicy, "S017");
                        #endregion

                        #region SMS TO Customer
                        //SendSMSToCustomer_UWPostPone(objPolicy, "S018");
                        #endregion

                        break;
                        case CrossCutting.CrossCuttingConstants.UWDecisionWithDrawn:

                        #region Email
                        ObjEmailDetails.Subject = "Cancellation of Life insurance proposal: " + objPolicy.ProposalNo + " - " + ObjEmailDetails.Salutation + " " + ObjEmailDetails.Name;
                        ObjEmailDetails.MailTemplate = "T006";
                        ObjEmailDetails.WPMobileNo = Context.tblMasIMOUsers.Where(a => a.UserName == objPolicy.UserName).Select(a => a.MobileNo).FirstOrDefault();
                        #endregion

                        #region SMS To Wealth Planner
                        //SendSMSToWP_UW(objPolicy, "S019");
                        #endregion

                        #region SMS TO Customer
                        //SendSMSToCustomer_UW(objPolicy, "S020");
                        #endregion

                        break;
                        case CrossCutting.CrossCuttingConstants.UWDecisionOutStandingReq:

                        #region Email
                        ObjEmailDetails.Subject = objPolicy.ProposalNo + " - " + ObjEmailDetails.Salutation + " " + ObjEmailDetails.Name + " - Additional requirements to consider the proposal";

                        #region Medical Document List
                        //List<tblPolicyDocument> MedicaldocumentList = new List<tblPolicyDocument>();
                        var PolicyID = Context.tblPolicies.Where(a => a.ProposalNo == objPolicy.ProposalNo).Select(a => a.PolicyID).FirstOrDefault();
                        var MedicaldocumentList = Context.tblPolicyDocuments.Where(a => a.ItemType == "PolicyDocuments" && a.PolicyID == PolicyID && a.Decision == "2368" && a.DocumentType == "Medical").ToList();
                        StringBuilder sb = new StringBuilder();

                        sb.Append(" <table border = '1' cellpadding = '0' cellspacing = '0' width='600'> ");
                        sb.Append("<tr>");
                        sb.Append("<td align='center' width='50'><b>No</b></td>");
                        // sb.Append("<td align='center'><b>Document Type</b></td>");
                        sb.Append("<td align='center' width='400'><b>Document Name</b></td>");
                        sb.Append("<td align='center' width='150'><b>Member Type</b></td>");
                        sb.Append("</tr>");

                        int i = 0;
                        foreach (var item in MedicaldocumentList)
                        {
                            i = i + 1;
                            sb.Append("<tr>");
                            sb.Append("<td align='center'>" + i + "</td>");
                            //sb.Append("<td align='center'>" + Convert.ToString(item.DocumentType + "</td>"));
                            sb.Append("<td align='center'>" + Convert.ToString(item.FileName + "</td>"));
                            sb.Append("<td align='center'>" + Convert.ToString(item.MemberType + "</td>"));

                            sb.Append("</tr>");
                        }
                        sb.Append("</table>");
                        sb.Append("");
                        ObjEmailDetails.TableQuotes = sb.ToString();
                        #endregion

                        #region Non-Medical List
                        //List<tblPolicyDocument> NonMedicaldocumentList = new List<tblPolicyDocument>();
                        var NonMedicaldocumentList = Context.tblPolicyDocuments.Where(a => a.ItemType == "PolicyDocuments" && a.PolicyID == PolicyID && a.Decision == "2368" && a.DocumentType != "Medical").ToList();
                        StringBuilder sbNonmedical = new StringBuilder();

                        sbNonmedical.Append(" <table border = '1' cellpadding = '0' cellspacing = '0' width='600'> ");
                        sbNonmedical.Append("<tr>");
                        sbNonmedical.Append("<td align='center' width='50'><b>No</b></td>");
                        sbNonmedical.Append("<td align='center' width='400'><b>Document Name</b></td>");
                        sbNonmedical.Append("<td align='center' width='150'><b>Member Type</b></td>");
                        sbNonmedical.Append("</tr>");

                        int j = 0;
                        foreach (var jitem in NonMedicaldocumentList)
                        {
                            j = j + 1;
                            sbNonmedical.Append("<tr>");
                            sbNonmedical.Append("<td align='center'>" + j + "</td>");
                            sbNonmedical.Append("<td align='center'>" + Convert.ToString(jitem.FileName + "</td>"));
                            sbNonmedical.Append("<td align='center'>" + Convert.ToString(jitem.MemberType + "</td>"));

                            sbNonmedical.Append("</tr>");
                        }
                        sbNonmedical.Append("</table>");
                        sbNonmedical.Append("");
                        ObjEmailDetails.TableNonMedicalQuotes = sbNonmedical.ToString();
                        #endregion

                        ObjEmailDetails.MailTemplate = "T015";

                        #endregion


                        #region SMS To Wealth Planner
                        //SendSMSToWP_UW(objPolicy, "S006");
                        #endregion

                        #region SMS TO Customer
                        //SendSMSToCustomer_UW(objPolicy, "S007");
                        #endregion
                        #region SMS To Wealth Planner
                        // SendSMSToWP_UW(objPolicy, "S009");
                        #endregion

                        #region SMS TO Customer
                        //SendSMSToCustomer_UW(objPolicy, "S010");
                        #endregion

                        break;
                        case CrossCutting.CrossCuttingConstants.UWDecisionCounterOffer:
                        #region Email
                        if (objPolicy.TemplateCode == "T011")
                        {
                            ObjEmailDetails.Subject = "Additional Clause for Life Insurance Proposal: " + objPolicy.ProposalNo + " - " + ObjEmailDetails.Salutation + " " + ObjEmailDetails.Name;
                            }
                        else if (objPolicy.TemplateCode == "T012")
                        {
                            ObjEmailDetails.Subject = "Additional Clause with Premium for Life Insurance Proposal: " + objPolicy.ProposalNo + " - " + ObjEmailDetails.Salutation + " " + ObjEmailDetails.Name;
                                ObjEmailDetails.ByteArray2 = objPolicy.ProspectSignPath;
                        }
                        ObjEmailDetails.MailTemplate = objPolicy.TemplateCode;
                        #endregion

                        #region SMS To Wealth Planner
                        //SendSMSToWP_UW(objPolicy, "S006");
                        #endregion

                        #region SMS TO Customer
                        //SendSMSToCustomer_UW(objPolicy, "S007");
                        #endregion
                        break;
                    }

                    ObjEmailDetails.ByteArray = objPolicy.ByteArray;
                   
                    //ObjEmailDetails.Salutation = objPolicy.objProspectDetails.Salutation;
                    //ObjEmailDetails.ByteArray2 = objPolicy.ByteArray2;
                    //ObjEmailDetails.ByteArray3 = objPolicy.ByteArray3;
                    //ObjEmailDetails.ByteArray4 = objPolicy.ByteArray4;

                    //ObjEmailDetails.surName = objPolicy.objProspectDetails.LastName;
                    ObjEmailDetails.QuoteNumber = objPolicy.QuoteNo;
                    ObjEmailDetails.ProposalNo = objPolicy.ProposalNo;
                    ObjEmailDetails.ProductName = objPolicy.PlanName;
                    ObjEmailDetails.PolicyNumber = objPolicy.ProposalNo;
                    //  ObjEmailDetails.Premium = Convert.ToString(objPolicy.AnnualPremium);
                    ObjEmailIntegration.EmailNotification(ObjEmailDetails);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                return false;
            }
        }
        public bool SMSNotificationOnUWDecision(AIA.Life.Models.Policy.Policy objPolicy, string Status)
        {
            try
            {
                if (!objPolicy.IsIntermSave)
                {
                    AVOAIALifeEntities Context = new AVOAIALifeEntities();
                    #region Email
                    SMSIntegration objSMSIntegration = new SMSIntegration();
                    SMSDetails objSMSDetails = new SMSDetails();
                    Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
                    #endregion
                    if (objPolicy.Decision == "1177")
                    {
                        List<UWDocument> OutStandDocRecNonMedical = new List<UWDocument>();
                        List<UWDocument> OutStandDocRecMedical = new List<UWDocument>();
                        for (int i = objPolicy.objMemberDetails.Count - 1; i >= 0; i--)
                        {
                            OutStandDocRecNonMedical = objPolicy.objMemberDetails[i].ObjUwDecision.lstUWNonMedicalDocument;
                            OutStandDocRecMedical = objPolicy.objMemberDetails[i].ObjUwDecision.lstUWMedicalDocument;
                        }
                        if (OutStandDocRecNonMedical.Count > 0)
                        {
                            foreach (var item in OutStandDocRecNonMedical)
                            {
                                if (item.Status == "2370" && objPolicy.Decision == "1177")
                                {
                                    #region SMS TO Customer
                                    SendSMSToWP_UW(objPolicy, "S010");
                                    #endregion
                                    #region SMS TO Customer
                                    SendSMSToCustomer_UW(objPolicy, "S009");
                                    #endregion
                                    break;
                                }
                            }
                        }
                        else if (OutStandDocRecMedical.Count > 0)
                        {
                            foreach (var item in OutStandDocRecMedical)
                            {
                                if (item.Status == "2370" && objPolicy.Decision == "1177")
                                {
                                    #region SMS TO Customer
                                    SendSMSToWP_UW(objPolicy, "S010");
                                    #endregion
                                    #region SMS TO Customer
                                    SendSMSToCustomer_UW(objPolicy, "S009");
                                    #endregion
                                    break;
                                }
                            }
                        }
                    }
                    switch (Status)
                    {
                        case CrossCutting.CrossCuttingConstants.UWDecisionOutStandingReq:
                        #region SMS To Wealth Planner
                        SendSMSToWP_UW(objPolicy, "S006");
                        #endregion

                        #region SMS TO Customer
                        SendSMSToCustomer_UW(objPolicy, "S007");
                        #endregion


                        break;

                        case CrossCutting.CrossCuttingConstants.UWDecisionDecline:
                        #region SMS To Wealth Planner
                        SendSMSToWP_UW(objPolicy, "S015");
                        #endregion

                        #region SMS TO Customer
                        SendSMSToCustomer_UW(objPolicy, "S016");
                        #endregion
                        break;
                        case CrossCutting.CrossCuttingConstants.UWDecisionPostPone:
                        #region SMS To Wealth Planner
                        SendSMSToWP_UW(objPolicy, "S017");
                        #endregion

                        #region SMS TO Customer
                        SendSMSToCustomer_UWPostPone(objPolicy, "S018");
                        #endregion

                        break;
                        case CrossCutting.CrossCuttingConstants.UWDecisionWithDrawn:
                        #region SMS To Wealth Planner
                        SendSMSToWP_UW(objPolicy, "S019");
                        #endregion
                        #region SMS TO Customer
                        SendSMSToCustomer_UW(objPolicy, "S020");
                        #endregion

                        break;
                        case CrossCutting.CrossCuttingConstants.UWDecisionNotTaken:
                        #region SMS To Wealth Planner
                        SendSMSToWP_UW(objPolicy, "S021");
                        #endregion
                        break;
                        case CrossCutting.CrossCuttingConstants.UWDecisionAccepted:
                        #region SMS To Wealth Planner
                        SendSMSToWP_UW(objPolicy, "S022");
                        #endregion
                        #region SMS TO Customer
                        SendSMSToCustomer_UW(objPolicy, "S023");
                        #endregion

                        break;


                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                return false;
            }
        }
        /// <summary>
        /// Add Member Level Decision
        /// </summary>
        /// <param name="objPolicy"></param>
        /// <returns></returns>
        public AIA.Life.Models.Policy.Policy AddMemberLevelDecision(AIA.Life.Models.Policy.Policy objPolicy, Guid CommonGuid)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {

                    var UserInfo = Context.AspNetUsers.Where(a => a.UserName == objPolicy.UserName).FirstOrDefault();
                    if (UserInfo != null)
                    {
                        foreach (var Member in objPolicy.objMemberDetails)
                        {
                            var MemberDetails = Context.tblPolicyMemberDetails.Where(a => a.MemberID == Member.MemberID).FirstOrDefault();
                            if (MemberDetails != null)
                            {
                                #region Member Level Decision
                                tblMemberLevelDecision objMemberLevelDecision = MemberDetails.tblMemberLevelDecisions.FirstOrDefault();
                                if (objMemberLevelDecision != null)
                                {
                                    objMemberLevelDecision.MemberID = MemberDetails.MemberID;
                                }
                                else
                                {
                                    objMemberLevelDecision = new tblMemberLevelDecision();
                                    objMemberLevelDecision.MemberID = MemberDetails.MemberID;
                                }
                                objMemberLevelDecision.Decision = Member.ObjUwDecision.Decision;
                                objMemberLevelDecision.DecisionDate = Member.ObjUwDecision.DecisionDate;
                                objMemberLevelDecision.Commencement_Date = Member.ObjUwDecision.CommencementDate;
                                objMemberLevelDecision.Remarks = Member.ObjUwDecision.Remarks;
                                objMemberLevelDecision.CreatedBy = UserInfo.Id;
                                objMemberLevelDecision.UWReason = Member.ObjUwDecision.UWReason;
                                objMemberLevelDecision.UWDuration = Member.ObjUwDecision.UWMonth;
                                objMemberLevelDecision.UWMedicalCode = Member.ObjUwDecision.UWMedicalCode;
                                objMemberLevelDecision.MedicalFeePaidBy = Member.ObjUwDecision.MedicalFeePaidBy;

                                if (objMemberLevelDecision.MemberDecisionID > 0)
                                {
                                }
                                else
                                {
                                    Context.tblMemberLevelDecisions.Add(objMemberLevelDecision);
                                }
                                #endregion

                                #region Member Decision History
                                if (!objPolicy.IsIntermSave)
                                {
                                    tblMemberLevelDecisionHistory objMemberDecisionHistory = new tblMemberLevelDecisionHistory();
                                    objMemberDecisionHistory.MemberID = MemberDetails.MemberID;
                                    objMemberDecisionHistory.Decision = Member.ObjUwDecision.Decision;
                                    objMemberDecisionHistory.DecisionDate = Member.ObjUwDecision.DecisionDate;
                                    objMemberDecisionHistory.Commencement_Date = Member.ObjUwDecision.CommencementDate;
                                    objMemberDecisionHistory.Remarks = Member.ObjUwDecision.Remarks;
                                    objMemberDecisionHistory.UWReason = Member.ObjUwDecision.UWReason;
                                    objMemberDecisionHistory.UWMonth = Member.ObjUwDecision.UWMonth;
                                    objMemberDecisionHistory.UWMedicalCode = Member.ObjUwDecision.UWMedicalCode;
                                    objMemberDecisionHistory.CreatedDate = DateTime.Now;
                                    objMemberDecisionHistory.CreatedBy = UserInfo.Id;
                                    objMemberDecisionHistory.CommonID = CommonGuid;
                                    Context.tblMemberLevelDecisionHistories.Add(objMemberDecisionHistory);
                                }
                                #endregion

                                Context.SaveChanges();


                                #region To Add New Documents
                                AddNewRequiredDocuments(Member, UserInfo.Id, MemberDetails.PolicyID);
                                #endregion

                                #region Update Document Status
                                UpdateDocumentStatus(Member, UserInfo.Id, CommonGuid, objPolicy.IsIntermSave, MemberDetails.PolicyID);
                                #endregion
                            }
                        }

                    }
                    return objPolicy;

                }

            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                objPolicy.Message = ex.InnerException.Message;
                return objPolicy;
            }
        }
        /// <summary>
        /// Member Deviation History
        /// </summary>
        /// <param name="objPolicy"></param>
        /// <returns></returns>
        public AIA.Life.Models.Policy.Policy AddMemberDeviationInfoHistory(AIA.Life.Models.Policy.Policy objPolicy, Guid CommonID)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    var UserInfo = Context.AspNetUsers.Where(a => a.UserName == objPolicy.UserName).FirstOrDefault();
                    if (UserInfo != null)
                    {
                        foreach (var Member in objPolicy.objMemberDetails)
                        {

                            foreach (var Deviation in Member.ObjUwDecision.lstMemberDeviationrules)
                            {
                                #region Added For Deviation History
                                tblMemberDeviationHistory objtblmemberdeviationHistory = new tblMemberDeviationHistory();
                                objtblmemberdeviationHistory.MemberDeviationID = Deviation.MemberDeviationid;
                                objtblmemberdeviationHistory.Decision = Deviation.Decision;
                                objtblmemberdeviationHistory.Createddate = DateTime.Now;
                                objtblmemberdeviationHistory.CreatedBy = UserInfo.Id;
                                objtblmemberdeviationHistory.CommonID = CommonID;
                                Context.tblMemberDeviationHistories.Add(objtblmemberdeviationHistory);
                                #endregion
                            }
                            Context.SaveChanges();
                        }
                    }
                    return objPolicy;
                }
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                objPolicy.Message = "Error";
                return objPolicy;
            }
        }

        public void FillRiderDetailsForProposal(decimal MemberID, string AssuredName, MemberDetails objMemberDetails, ref List<BenifitDetails> objLstBenefitOverView)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            foreach (var Rider in Context.tblPolicyMemberBenefitDetails.Where(a => a.MemberID == MemberID).ToList())
            {
                int BenefitIndex = objLstBenefitOverView.FindIndex(a => a.BenefitID == Rider.BenifitID);
                if (BenefitIndex >= 0)
                {
                    if (objLstBenefitOverView[BenefitIndex].objBenefitMemberRelationship == null)
                    {
                        objLstBenefitOverView[BenefitIndex].objBenefitMemberRelationship = new List<AssuredRelation>();
                    }
                    int BasicCoverMemberIndex = objLstBenefitOverView[BenefitIndex].objBenefitMemberRelationship.FindIndex(a => a.Assured_Name == AssuredName);
                    // objLstBenefitOverView[BenefitIndex].objBenefitMemberRelationship[BasicCoverMemberIndex].PolicyMemberBenefitID = Rider.MemberBenifitID;
                    objLstBenefitOverView[BenefitIndex].objBenefitMemberRelationship[BasicCoverMemberIndex].RiderSI = Rider.SumInsured;
                    objLstBenefitOverView[BenefitIndex].objBenefitMemberRelationship[BasicCoverMemberIndex].Rider_Premium = Rider.Premium;
                    //objLstBenefitOverView[BenefitIndex].objBenefitMemberRelationship[BasicCoverMemberIndex].ActualRiderPremium = Rider.ActualPremium.ToString();
                    //objLstBenefitOverView[BenefitIndex].objBenefitMemberRelationship[BasicCoverMemberIndex].LoadingAmount = Rider.LoadingAmount;
                }
            }
        }
        public void FillRiderDetails(int QuoteMemberID, string AssuredName, MemberDetails objMemberDetails, ref List<BenifitDetails> objLstBenefitOverView)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            foreach (var Rider in Context.tblQuoteMemberBeniftDetials.Where(a => a.MemberID == QuoteMemberID).ToList())
            {
                int BenefitIndex = objLstBenefitOverView.FindIndex(a => a.BenefitID == Rider.BenifitID);
                if (objLstBenefitOverView[BenefitIndex].objBenefitMemberRelationship == null)
                {
                    objLstBenefitOverView[BenefitIndex].objBenefitMemberRelationship = new List<AssuredRelation>();
                }
                int BasicCoverMemberIndex = objLstBenefitOverView[BenefitIndex].objBenefitMemberRelationship.FindIndex(a => a.Assured_Name == AssuredName);
                objLstBenefitOverView[BenefitIndex].objBenefitMemberRelationship[BasicCoverMemberIndex].RiderSI = Rider.SumInsured;
                objLstBenefitOverView[BenefitIndex].objBenefitMemberRelationship[BasicCoverMemberIndex].Rider_Premium = Rider.Premium;
                objLstBenefitOverView[BenefitIndex].objBenefitMemberRelationship[BasicCoverMemberIndex].ActualRiderPremium = Rider.ActualPremium.ToString();
                objLstBenefitOverView[BenefitIndex].objBenefitMemberRelationship[BasicCoverMemberIndex].LoadingAmount = Rider.LoadingAmount.ToString();
            }


            //#region Set Basic Cover Info
            //int basicCoverIndex = 0;
            //if (objLstBenefitOverView[basicCoverIndex].objBenefitMemberRelationship == null)
            //{
            //    objLstBenefitOverView[basicCoverIndex].objBenefitMemberRelationship = new List<AssuredRelation>();
            //}
            //int MemberIndex = objLstBenefitOverView[basicCoverIndex].objBenefitMemberRelationship.FindIndex(a => a.Assured_Name == AssuredName);
            //objLstBenefitOverView[basicCoverIndex].objBenefitMemberRelationship[MemberIndex].RiderSI = objMemberDetails.BasicSumInsured;
            //objLstBenefitOverView[basicCoverIndex].objBenefitMemberRelationship[MemberIndex].Rider_Premium = objMemberDetails.Basicpremium;
            //#endregion

        }

        public string GetFetchMaritialStatus(int CommonTypesID)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            return Context.tblMasCommonTypes.Where(a => a.CommonTypesID == CommonTypesID).Select(a => a.Code).FirstOrDefault();
        }
        public string GetLoadMaritialStatus(decimal? CommonTypesID)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            return Context.tblMasCommonTypes.Where(a => a.CommonTypesID == CommonTypesID).Select(a => a.Code).FirstOrDefault();
        }
        public string GetFetchNationality(int CommonTypesID)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            return Context.tblMasCommonTypes.Where(a => a.CommonTypesID == CommonTypesID).Select(a => a.Code).FirstOrDefault();
        }
        public MemberDetails FetchProposerInfo(MemberDetails objMemberDetails, tblPolicyClient objPolicyClient)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            objMemberDetails.RelationShipWithPropspect = objPolicyClient.FullName;
            objMemberDetails.ContactPerson = objPolicyClient.ContactPerson;
            objMemberDetails.Designation = objPolicyClient.Designation;
            objMemberDetails.BusinessRegistrationNo = objPolicyClient.BusinessRegistrationNo;
            objMemberDetails.LifeAssured = objPolicyClient.FullName;
            objMemberDetails.CompanyName = objPolicyClient.CompanyName;
            objMemberDetails.CorporateName = objPolicyClient.CorporateName;
            objMemberDetails.ProposerEmailID = objPolicyClient.ProposerEamilID;
            objMemberDetails.ProposerMobileNo = objPolicyClient.ProposerTelepohoneNo;
            objMemberDetails.DateOfBirth = Convert.ToDateTime(objPolicyClient.DateOfBirth);
            objMemberDetails.Age = objPolicyClient.Age;
            objMemberDetails.Email = objPolicyClient.EmailID;
            objMemberDetails.FirstName = objPolicyClient.FirstName;
            objMemberDetails.MiddleName = objPolicyClient.MiddleName;
            objMemberDetails.LastName = objPolicyClient.LastName;
            objMemberDetails.NameWithInitial = objPolicyClient.NameWithInitials;
            objMemberDetails.Gender = objPolicyClient.Gender;
            objMemberDetails.Salutation = Context.tblMasCommonTypes.Where(a => a.Code == objPolicyClient.Title).Select(a => a.Description).FirstOrDefault();
            objMemberDetails.SalutationCode = objPolicyClient.Title;
            objMemberDetails.WorkNumber = objPolicyClient.WorkNo;
            objMemberDetails.HomeNumber = objPolicyClient.HomeNo;
            //objMemberDetails.MaritialStatus = Convert.ToString(objPolicyClient.MaritalStatus);
            objMemberDetails.MaritialStatus = GetFetchMaritialStatus(Convert.ToInt32(objPolicyClient.MaritalStatus));
            objMemberDetails.MobileNo = objPolicyClient.MobileNo;
            objMemberDetails.OtherMobileNo = objPolicyClient.AlteranteMobileNO;
            objMemberDetails.OccupationID = Convert.ToInt32(objPolicyClient.OccupationID);
            objMemberDetails.MonthlyIncome = objPolicyClient.MonthlyIncome;
            objMemberDetails.NameWithInitial = objPolicyClient.NameWithInitials;
            //objMemberDetails.Nationality = Convert.ToString(objPolicyClient.Nationality);
            objMemberDetails.Nationality = GetFetchNationality(Convert.ToInt16(objPolicyClient.Nationality));
            objMemberDetails.NameOfDuties = objPolicyClient.NatureOfDuties;
            objMemberDetails.NewNICNO = objPolicyClient.NEWNICNo;
            objMemberDetails.OLDNICNo = objPolicyClient.OLDNICNo;
            objMemberDetails.PrefferedName = objPolicyClient.PreferredName;
            objMemberDetails.IsproposerlifeAssured = objPolicyClient.IsProposerAssured;
            objMemberDetails.IsRegAsCommunication = Convert.ToBoolean(objPolicyClient.IsPermanentAddrSameasCommAddr);
            objMemberDetails.CountryofOccupation = objPolicyClient.CountryOccupation;
            objMemberDetails.CitizenShip = Convert.ToBoolean(objPolicyClient.CitizenShip);
            objMemberDetails.Citizenship1 = objPolicyClient.Citizenship1;
            objMemberDetails.Citizenship2 = objPolicyClient.Citizenship2;
            objMemberDetails.ResidentialStatus = objPolicyClient.ResidentialNationalityStatus;
            objMemberDetails.Residential = objPolicyClient.ResidentialNationality;
            objMemberDetails.OccupationHazardousWork = Convert.ToBoolean(objPolicyClient.OccupationHazardousWork);
            objMemberDetails.SpecifiyOccupationHazardousWork = objPolicyClient.SpecifyHazardousWork;
            objMemberDetails.PassportNumber = objPolicyClient.PassportNumber;
            objMemberDetails.DrivingLicense = objPolicyClient.DrivingLicense;
            objMemberDetails.USTaxpayerId = objPolicyClient.USTaxpayerId;




            return objMemberDetails;

        }

        public string GetFetchMemberDetailsGender(int CommonTypesID)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            return Context.tblMasCommonTypes.Where(a => a.CommonTypesID == CommonTypesID).Select(a => a.Code).FirstOrDefault();
        }
        public MemberDetails FetchMemberDetails(tblPolicyMemberDetail objMemberDetail, MemberDetails objMember, bool ProposalFetch = false, int PlanId = 0)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            decimal SAR = decimal.Zero;
            bool IsAFC = false;

            objMember.IsOCRSeleted = Convert.ToBoolean(objMemberDetail.OCR);
            objMember.IsOCRImageRecognition = Convert.ToBoolean(objMemberDetail.OCRImageRecognition);
            objMember.Memberpremium = objMemberDetail.MemberPremium;
            objMember.ClientCode = objMemberDetail.ClientCode;
            objMember.SAM = objMemberDetail.SAM;
            objMember.Age = objMemberDetail.Age;
            objMember.CompanyName = objMemberDetail.CompanyName;
            objMember.DateOfBirth = Convert.ToDateTime(objMemberDetail.DOB);
            objMember.Email = objMemberDetail.Email;
            objMember.FirstName = objMemberDetail.FirstName;
            objMember.MiddleName = objMemberDetail.MiddleName;
            objMember.LastName = objMemberDetail.LastName;
            objMember.NameWithInitial = objMemberDetail.NameWithInitial;
            objMember.HomeNumber = objMemberDetail.Landline;
            objMember.MaritialStatus = objMemberDetail.MaritialStatus;
            objMember.MobileNo = objMemberDetail.Mobile;
            objMember.OtherMobileNo = objMemberDetail.AlternateMobileNo;
            objMember.HomeNumber = objMemberDetail.Home;
            objMember.WorkNumber = objMemberDetail.Work;
            objMember.MonthlyIncome = objMemberDetail.MonthlyIncome;
            objMember.NameWithInitial = objMemberDetail.NameWithInitial;
            objMember.Nationality = objMemberDetail.Nationality;
            objMember.NameOfDuties = objMemberDetail.NatureOfDuties;
            objMember.NewNICNO = objMemberDetail.NEWNICNO;
            objMember.OccupationID = Convert.ToInt32(objMemberDetail.OccupationID);
            objMember.OLDNICNo = objMemberDetail.OLDNICNO;
            objMember.PrefferedName = objMemberDetail.PreferredName;
            objMember.RelationShipWithPropspect = Convert.ToString(objMemberDetail.RelationShipWithProposer);
            objMember.Salutation = Context.tblMasCommonTypes.Where(a => a.Code == objMemberDetail.Salutation).Select(a => a.Description).FirstOrDefault();
            objMember.SalutationCode = objMemberDetail.Salutation;
            objMember.QuoteMemberID = Convert.ToInt32(objMemberDetail.QuoteMemberid);
            objMember.AssuredName = objMemberDetail.Assuredname;
            objMember.MemberID = objMemberDetail.MemberID;
            //objMember.Gender = objMemberDetail.Gender != null ? Convert.ToString(objMemberDetail.Gender) : string.Empty;

            objMember.Gender = objMemberDetail.Gender;
            objMember.GenderText = objMember.Gender;
            //objMember.Gender = GetFetchMemberDetailsGender(Convert.ToInt32(objMemberDetail.Gender));

            objMember.NoofJsPolicies = objMemberDetail.NoofJsPolicies != null ? Convert.ToInt32(objMemberDetail.NoofJsPolicies) : 0;
            objMember.NoofOtherPolicies = objMemberDetail.NoofOtherPolicies != null ? Convert.ToInt32(objMemberDetail.NoofOtherPolicies) : 0;
            objMember.AreyouCoveredUnderOtherPolicies = objMemberDetail.IsOtherPolicy != null ? Convert.ToBoolean(objMemberDetail.IsOtherPolicy) : false;
            objMember.TotalAccidental = objMemberDetail.TotalAccidental;
            objMember.TotalCritical = objMemberDetail.TotalCritical;
            objMember.TotalDeath = objMemberDetail.TotalDeath;
            objMember.TotalHospitalization = objMemberDetail.TotalHospitalization;
            objMember.Basicpremium = objMemberDetail.BasicPremium;
            objMember.BasicSumInsured = objMemberDetail.BasicSuminsured;
            objMember.TotalHospitalizationReimbursement = objMemberDetail.TotalHospitalizationReimbursement;

            objMember.IsSameasProposerAddress = objMemberDetail.IsSameasProposerAddress != null ? Convert.ToBoolean(objMemberDetail.IsSameasProposerAddress) : false;

            objMember.CitizenShip = objMemberDetail.CitizenShip != null ? Convert.ToBoolean(objMemberDetail.CitizenShip) : false;
            objMember.Citizenship1 = objMemberDetail.Citizenship1;
            objMember.Citizenship2 = objMemberDetail.Citizenship2;
            objMember.ResidentialStatus = objMemberDetail.ResidentialNationalityStatus;
            objMember.Residential = objMemberDetail.ResidentialNationality;
            objMember.OccupationHazardousWork = Convert.ToBoolean(objMemberDetail.OccupationHazardousWork);
            objMember.SpecifiyOccupationHazardousWork = objMemberDetail.SpecifyHazardousWork;
            objMember.PassportNumber = objMemberDetail.PassportNumber;
            objMember.DrivingLicense = objMemberDetail.DrivingLicense;
            objMember.USTaxpayerId = objMemberDetail.USTaxpayerId;
            objMember.CountryofOccupation = objMemberDetail.CountryOccupation;
            objMember.BMIValue = objMemberDetail.BMIValue;
            objMember.SAR = Convert.ToDecimal(objMemberDetail.SAR);
            objMember.FAL = Convert.ToDecimal(objMemberDetail.FAL);
            objMember.AFC = objMemberDetail.AFC != null ? Convert.ToBoolean(objMemberDetail.AFC) : false;
            objMember.IsOCRSeleted = objMemberDetail.OCR ?? false;


            #region FillAddressDetails
            tblAddress objtbladdress = objMemberDetail.tblAddress;
            if (objtbladdress != null)
            {
                objMember.objCommunicationAddress = FetchAddressDetails(objtbladdress);
                //  objMember.IsSameasProposerAddress
                if (objMemberDetail.IsPermanentAddrSameasCommAddr == true)
                {

                    objMember.IsRegAsCommunication = true;
                    objMember.objPermenantAddress = objMember.objCommunicationAddress;
                }
                else
                {
                    objMember.IsRegAsCommunication = false;
                    tblAddress objtblPermanentaddress = objMemberDetail.tblAddress1;
                    if (objtblPermanentaddress != null)
                    {
                        objMember.objPermenantAddress = FetchAddressDetails(objtblPermanentaddress);
                    }


                }
            }


            #endregion

            if (ProposalFetch)
            {
                try
                {
                    // Current proposal Annual premium
                    var _QuoteMemberDetail = Context.tblQuoteMemberDetials.Where(a => a.MemberID == objMemberDetail.QuoteMemberid).FirstOrDefault();
                    if (_QuoteMemberDetail != null)
                    {
                        if (!string.IsNullOrEmpty(_QuoteMemberDetail.MemberPremium))
                        {
                            objMember._CurrentproposalAnnualPremium = Convert.ToDecimal(_QuoteMemberDetail.MemberPremium);
                        }
                    }
                    // Till here
                    // Previous Proposal Annual premium
                    var SARDetails = Context.SP_GetSARDetails(objMember.NewNICNO).FirstOrDefault();
                    if (SARDetails != null)
                    {
                        decimal AnnualPrem = Convert.ToDecimal(SARDetails.ANNPREM);
                        objMember._AnnualPremium = AnnualPrem + objMember._CurrentproposalAnnualPremium;
                    }
                    // Till here

                    //Set  Occupation Class
                    if (objMember.OccupationID > 0)
                    {
                        int OccupationCode = Convert.ToInt32(objMember.OccupationID);
                        var Masoccupation = Context.tblMasLifeOccupations.Where(a => a.Code == OccupationCode).FirstOrDefault();
                        if (Masoccupation != null)
                        {
                            objMember.OccuaptionClass = Masoccupation.ClassType;
                        }
                    }
                    // Till here
                    if (!string.IsNullOrEmpty(objMember.NewNICNO))
                    {
                        objMember.PreviousPolicyFlag = Context.Database.SqlQuery<string>(
                       "select case when COUNT(*)=0  then 'False' else 'True'end  from  sync.NXMLIAP  where Securityno='" + objMember.NewNICNO + "'").FirstOrDefault();
                        //var PrevRider = Context.usp_GetPreviousPolicyRiderSI(objMember.NewNICNO).FirstOrDefault();
                        var PrevRider = Context.usp_GetPreviousPolicyRiderSIDetails(objMember.NewNICNO, objMember.QuoteMemberID, PlanId).FirstOrDefault();
                        if (PrevRider != null)
                        {
                            objMember.ADDBSA = PrevRider.ADDBSA;
                            objMember.ASBSA = PrevRider.ASBSA;
                            objMember.CIBHPSA = PrevRider.CIBHPSA;
                            objMember.CIBSA = PrevRider.CIBSA;
                            objMember.IPBSA = PrevRider.IPBSA;
                            objMember.HIPSA = PrevRider.HIPSA;
                            objMember.HECHPSA = PrevRider.HECHPSA;
                            objMember.HECSA = PrevRider.HECSA;
                            objMember.HIPHPSA = PrevRider.HIPHPSA;
                        }

                        var PrevClaims = Context.SP_GetPreviousClaimDetails(objMember.NewNICNO).FirstOrDefault();
                        if (PrevClaims != null)
                        {
                            if (PrevClaims.Value > 0)
                            {
                                objMember.ClaimCount = PrevClaims.Value;
                            }
                            else
                            {
                                objMember.ClaimCount = 0;
                            }
                        }
                        var _PrevLoading = Context.Sp_GetOccupationalLoadingDetails(objMember.NewNICNO).FirstOrDefault();

                        if (_PrevLoading != null)
                        {
                            objMember.PrevOE = _PrevLoading.PrevOE;
                            objMember.PrevHE = _PrevLoading.PrevHE;
                            objMember.PrevRE = _PrevLoading.PrevRE;
                        }
                    }



                }
                catch (Exception)
                {


                }
            }
            return objMember;
        }
        public AIA.Life.Models.Integration.Services.ProposalInfo UpdateUWInfo(AIA.Life.Models.Integration.Services.ProposalInfo objProposalInfo)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            var tblPolicyInfo = Context.tblPolicies.Where(a => a.ProposalNo == objProposalInfo.ProposalNo).FirstOrDefault();
            if (tblPolicyInfo != null)
            {
                tblUWRemark obj = new tblUWRemark();
                obj.DocumentID = objProposalInfo.DocumentID;
                obj.Remarks = objProposalInfo.UWDecision;
                obj.Reason = objProposalInfo.UWRemarks;
                obj.QuoteNo = tblPolicyInfo.QuoteNo;
                obj.ProposalNo = objProposalInfo.ProposalNo;
                obj.CreatedDate = DateTime.Now;
                obj.ModifiedDate = DateTime.Now;
                obj.PolicyId = tblPolicyInfo.PolicyID;
                Context.tblUWRemarks.Add(obj);
                tblPolicyInfo.PolicyNo = objProposalInfo.PolicyNo;
                Context.SaveChanges();
            }
            objProposalInfo.Status = "success";
            return objProposalInfo;

        }
        /// <summary>
        /// Submit Pending Requirements
        /// </summary>
        /// <param name="objpolicy"></param>
        /// <returns></returns>
        public AIA.Life.Models.Policy.Policy SubmitPolicyDocuments(AIA.Life.Models.Policy.Policy ObjPolicy)
        {
            try
            {
                #region Document Upload
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    if (ObjPolicy.HdnDocumentDetails != null)
                    {
                        Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
                        List<DocumentUploadFile> objLstDoc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DocumentUploadFile>>(ObjPolicy.HdnDocumentDetails, settings);
                        var objtblpolicy = Context.tblPolicies.Where(a => a.ProposalNo == ObjPolicy.ProposalNo).FirstOrDefault();
                        if (objtblpolicy != null)
                        {
                            ObjPolicy.PolicyStageStatusID = objtblpolicy.PolicyStageStatusID;
                            if (objLstDoc != null && objLstDoc.Count() > 0)
                            {
                                foreach (var Document in objLstDoc)
                                {
                                    tblPolicyDocument objPolicyDocument = objtblpolicy.tblPolicyDocuments.Where(a => a.FileName == Document.FileName && a.MemberType == Document.MemberType).FirstOrDefault();
                                    if (objPolicyDocument == null)
                                        objPolicyDocument = new tblPolicyDocument();
                                    objPolicyDocument.FileName = Document.FileName;
                                    #region Set Document Type
                                    if (!string.IsNullOrEmpty(Document.FileName))
                                    {
                                        var DocMaster = Context.tblMasDocuments.Where(a => a.DocumentName == Document.FileName).FirstOrDefault();
                                        if (DocMaster != null)
                                        {
                                            if (DocMaster.DocumentType == "Medical")
                                            {
                                                objPolicyDocument.DocumentType = DocMaster.DocumentType;
                                            }
                                        }
                                    }
                                    #endregion
                                    objPolicyDocument.FilePath = Document.FilePath;
                                    objPolicyDocument.ItemType = Document.ItemType;
                                    objPolicyDocument.MemberType = Document.MemberType;
                                    objPolicyDocument.PolicyID = objtblpolicy.PolicyID;
                                    objPolicyDocument.CreatedDate = DateTime.Now;
                                    if (objPolicyDocument.DocumentUploadID == decimal.Zero)
                                        Context.tblPolicyDocuments.Add(objPolicyDocument);
                                }
                            }

                            if (!ObjPolicy.ProcceedToPayment)
                            {
                                objtblpolicy.PolicyStageStatusID = 193;// UW
                                objtblpolicy.IsAllocated = false; // Pending for Allocation
                            }


                        }
                        Context.SaveChanges();
                    }
                }

                #endregion
                ObjPolicy.Message = "Success";
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                ObjPolicy.Message = "Error";
            }
            return ObjPolicy;
        }


        /// <summary>
        /// Counter Offer subimt Case
        /// </summary>
        /// <param name="objMemberDetails"></param>
        /// <returns></returns>
        public AIA.Life.Models.Policy.Policy CounterOfferSubmit(AIA.Life.Models.Policy.Policy ObjPolicy)
        {
            try
            {
                #region Document Upload
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    #region Add Newly Uploaded Documents
                    if (ObjPolicy.HdnDocumentDetails != null)
                    {
                        Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
                        List<DocumentUploadFile> objLstDoc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DocumentUploadFile>>(ObjPolicy.HdnDocumentDetails, settings);
                        var objtblpolicy = Context.tblPolicies.Where(a => a.PolicyID == ObjPolicy.PolicyID).FirstOrDefault();
                        if (objtblpolicy != null)
                        {

                            if (objLstDoc != null && objLstDoc.Count() > 0)
                            {
                                foreach (var Document in objLstDoc)
                                {
                                    tblPolicyDocument objPolicyDocument = new tblPolicyDocument();
                                    objPolicyDocument.FileName = Document.FileName;
                                    objPolicyDocument.FilePath = Document.FilePath;
                                    objPolicyDocument.ItemType = Document.ItemType;
                                    objPolicyDocument.CreatedDate = DateTime.Now;
                                    objPolicyDocument.MemberType = Document.MemberType;
                                    objPolicyDocument.PolicyID = ObjPolicy.PolicyID;
                                    Context.tblPolicyDocuments.Add(objPolicyDocument);
                                }
                            }


                        }
                    }
                    Context.SaveChanges();
                    #endregion

                    UWRuleLogic objRuleLogic = new UWRuleLogic();
                    objRuleLogic.SaveRequiredMedicalDocuments(ObjPolicy);
                }

                #endregion
                ObjPolicy.Message = "Success";
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                ObjPolicy.Message = "Error";
            }
            return ObjPolicy;

        }
        //7-5-2018 Edited following function dude to decimal error on MemberBenifitID & Loading.MemberBenifitDetailsID so that it can handle null
        public MemberDetails FetchLoadigInfo(MemberDetails objMemberDetails)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    objMemberDetails.objBenifitDetails = new List<BenifitDetails>();
                    objMemberDetails.objBenifitDetails = (from Member in Context.tblPolicyMemberDetails.Where(a => a.MemberID == objMemberDetails.MemberID && a.IsDeleted != true)
                                                          join MemberBenefit in Context.tblPolicyMemberBenefitDetails.Where(b => b.IsDeleted != true)
                                                          on Member.MemberID equals MemberBenefit.MemberID
                                                          join MasBenefit in Context.tblProductPlanRiders
                                                          on MemberBenefit.BenifitID equals MasBenefit.ProductPlanRiderId
                                                          join OtherDetails in Context.tblMemberBenefitOtherDetails
                                                          on MemberBenefit.MemberBenifitID equals OtherDetails.MemberBenifitID into LoadingInfo
                                                          from Loading in LoadingInfo.DefaultIfEmpty()
                                                          orderby MasBenefit.DisplayOrder ascending
                                                          select new BenifitDetails
                                                          {
                                                              BenifitName = MasBenefit.DisplayName,
                                                              BenefitID = (int)MasBenefit.ProductPlanRiderId,
                                                              MemberBenifitID = MemberBenefit.MemberBenifitID != null ? MemberBenefit.MemberBenifitID : 0,
                                                              RiderSuminsured = MemberBenefit.SumInsured,
                                                              RiderPremium = MemberBenefit.Premium,
                                                              LoadingType = Loading.LoadingType,
                                                              LoadingBasis = Loading.LoadingBasis,
                                                              ExtraPremium = Loading.ExtraPremium,
                                                              Exclusion = Loading.Exclusion,
                                                              LoadingAmount = Loading.LoadingAmount,
                                                              MemberBenefitDetailID = Loading.MemberBenifitDetailsID != null ? Loading.MemberBenifitDetailsID : 0
                                                          }).ToList();
                    #region Load Required Masters
                    objMemberDetails.LstLoadingType = (from CommonType in Context.tblMasCommonTypes.Where(a => a.MasterType == "LoadingType")
                                                       select new MasterListItem
                                                       {
                                                           ID = CommonType.CommonTypesID,
                                                           Text = CommonType.Description
                                                       }).ToList();
                    objMemberDetails.LstLoadingBasis = (from CommonType in Context.tblMasCommonTypes.Where(a => a.MasterType == "LoadingBasis")
                                                        select new MasterListItem
                                                        {
                                                            ID = CommonType.CommonTypesID,
                                                            Text = CommonType.Description
                                                        }).ToList();
                    #endregion
                }
            }
            catch (Exception ex)
            {

                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
            }
            return objMemberDetails;
        }
        public UWMemberLevelDeviationStatus UpdateMemberLevelDeviation(UWMemberLevelDeviationStatus objStatus)
        {
            try
            {
                using (AVOAIALifeEntities Entity = new AVOAIALifeEntities())
                {
                    var UserInfo = Entity.AspNetUsers.Where(a => a.UserName == objStatus.UserName).FirstOrDefault();
                    if (UserInfo != null)
                    {
                        var MemberDeviationInfo = Entity.tblMemberDeviationInfoes.Where(a => a.MemberDeviationID == objStatus.MemberDeviationID).FirstOrDefault();
                        if (MemberDeviationInfo != null)
                        {
                            MemberDeviationInfo.Decision = objStatus.Status;
                            MemberDeviationInfo.CreatedBy = UserInfo.Id;
                            MemberDeviationInfo.CreateDate = DateTime.Now;
                            Entity.SaveChanges();
                        }
                    }


                    return objStatus;
                }
            }

            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                return objStatus;
            }

        }
        public void UpdateMemberDiseaseQuestions(decimal Memberid)
        {
            try
            {
                using (AVOAIALifeEntities Entity = new AVOAIALifeEntities())
                {
                    foreach (var MemeberQuestion in Entity.tblMemberQuestions.Where(a => a.MemberID == Memberid && a.ItemType == "DiseaseQuestion"))
                    {
                        MemeberQuestion.IsDeleted = true;
                        Entity.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
            }
        }

        public List<UWMedicalDocumentsList> UWMedicalDocumentsList()
        {
            List<UWMedicalDocumentsList> ObjUWMedicalDocumentsList = new List<UWMedicalDocumentsList>();
            ObjUWMedicalDocumentsList.Add(new UWMedicalDocumentsList { MedicalDocumentsName = "Lipid Test", MedicalDocumentsDate = DateTime.Now });
            ObjUWMedicalDocumentsList.Add(new UWMedicalDocumentsList { MedicalDocumentsName = "BP Test", MedicalDocumentsDate = DateTime.Now });
            return ObjUWMedicalDocumentsList;
        }

        public List<UWNonMedicalDocumentsList> UWNonMedicalDocumentsList()
        {
            List<UWNonMedicalDocumentsList> ObjUWNonMedicalDocumentsList = new List<UWNonMedicalDocumentsList>();
            ObjUWNonMedicalDocumentsList.Add(new UWNonMedicalDocumentsList { MedicalDocumentsName = "Age Proof", NonMedicalDocumentsDate = DateTime.Now });
            ObjUWNonMedicalDocumentsList.Add(new UWNonMedicalDocumentsList { MedicalDocumentsName = "Address Proof", NonMedicalDocumentsDate = DateTime.Now });
            return ObjUWNonMedicalDocumentsList;
        }
        public MasterListItem GetMasterTypeItem(string CommontypeID, AVOAIALifeEntities Context)
        {
            MasterListItem obj = new MasterListItem();
            int ID = Convert.ToInt32(CommontypeID);
            var CommonType = Context.tblMasCommonTypes.Where(a => a.CommonTypesID == ID).FirstOrDefault();
            if (CommonType != null)
            {
                obj.ID = CommonType.CommonTypesID;
                obj.Value = CommonType.Description;
            }
            return obj;
        }
        public AIA.Life.Models.Common.MemberDetails FetchUWDeviation(AIA.Life.Models.Common.MemberDetails ObjMemberDetails, string QuoteNo, string userName)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            try
            {
                if (String.IsNullOrEmpty(userName))
                {
                    return ObjMemberDetails;
                }

                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetUWPermissions";
                cmd.Parameters.Add("@QuoteNo", SqlDbType.VarChar);
                cmd.Parameters.Add("@UserName", SqlDbType.VarChar);
                cmd.Parameters.Add("@MemberType", SqlDbType.VarChar);

                cmd.Parameters[0].Value = QuoteNo;
                cmd.Parameters[1].Value = userName;// Test Purpose
                cmd.Parameters[2].Value = ObjMemberDetails.AssuredName;
                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    MemberDeviationrules MemberDev = new MemberDeviationrules();
                    MemberDev.DeviatIonCode = ds.Tables[0].Rows[i].ItemArray[3].ToString();
                    MemberDev.Deviation = ds.Tables[0].Rows[i].ItemArray[4].ToString();
                    string _Permissionstring = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[7]);
                    var PermissionArray = _Permissionstring.Split('/');
                    if (PermissionArray != null)
                    {
                        MemberDev.LstDecision = new List<MasterListItem>();
                        foreach (var _permission in PermissionArray)
                        {
                            if (!string.IsNullOrEmpty(_permission))
                            {
                                MemberDev.LstDecision.Add(GetMasterTypeItem(_permission, Context));
                            }
                        }
                    }
                    MemberDev.Decision = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[5]);
                    MemberDev.RuleId = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[2]);
                    MemberDev.MemberDeviationid = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]);// 

                    ObjMemberDetails.ObjUwDecision.lstMemberDeviationrules.Add(MemberDev);
                }
                //DataSet ds1 = new DataSet();
                //System.Data.SqlClient.SqlDataAdapter da1 = new System.Data.SqlClient.SqlDataAdapter(cmd);
                //da.Fill(ds1);
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    UWDocument objUWDocument = new UWDocument();
                    objUWDocument.DocumentId = Convert.ToInt32(ds.Tables[1].Rows[i].ItemArray[0]);
                    string DocType = Convert.ToString(Convert.ToString(ds.Tables[1].Rows[i].ItemArray[1]));
                    string _Permissionstring = Convert.ToString(ds.Tables[1].Rows[i].ItemArray[7]);
                    var PermissionArray = _Permissionstring.Split('/');
                    if (PermissionArray != null)
                    {
                        objUWDocument.LstStatus = new List<MasterListItem>();
                        foreach (var _permission in PermissionArray)
                        {
                            if (!string.IsNullOrEmpty(_permission))
                            {
                                objUWDocument.LstStatus.Add(GetMasterTypeItem(_permission, Context));
                            }
                        }


                    }
                    objUWDocument.Status = Convert.ToString(ds.Tables[1].Rows[i].ItemArray[5]);
                    objUWDocument.Remarks = Convert.ToString(ds.Tables[1].Rows[i].ItemArray[6]);
                    objUWDocument.Link = Convert.ToString(ds.Tables[1].Rows[i].ItemArray[3]);
                    if (!(ds.Tables[1].Rows[i].ItemArray[9] is DBNull))
                    {
                        var IsNewDocument = Convert.ToInt32(ds.Tables[1].Rows[i].ItemArray[9]);
                        objUWDocument.IsNewDocumentAddedbyUW = Convert.ToBoolean(IsNewDocument);
                    }
                    objUWDocument.LstFileLinks = new List<FileLinks>();
                    if (!(ds.Tables[1].Rows[i].ItemArray[8] is DBNull))
                    {
                        objUWDocument.DateTime = Convert.ToDateTime(ds.Tables[1].Rows[i].ItemArray[8]);

                    }

                    if (!string.IsNullOrEmpty(objUWDocument.Link))
                    {
                        List<string> Links = objUWDocument.Link.Split(',').ToList();
                        if (Links.Count() > 0)
                        {
                            foreach (var Link in Links)
                            {
                                FileLinks objFileLink = new FileLinks();
                                objFileLink.FileName = Path.GetFileName(Link);
                                objFileLink.Link = Link;
                                objUWDocument.LstFileLinks.Add(objFileLink);
                            }
                        }

                    }
                    if (DocType == "Medical")
                    {
                        objUWDocument.DocType = DocType;
                        objUWDocument.Document = ds.Tables[1].Rows[i].ItemArray[2].ToString();
                        ObjMemberDetails.ObjUwDecision.lstUWMedicalDocument.Add(objUWDocument);
                    }

                    else
                    {
                        objUWDocument.DocType = DocType;
                        objUWDocument.Document = ds.Tables[1].Rows[i].ItemArray[2].ToString();
                        ObjMemberDetails.ObjUwDecision.lstUWNonMedicalDocument.Add(objUWDocument);
                    }

                }

                var MemberLevelDecision = Context.tblMemberLevelDecisions.Where(a => a.MemberID == ObjMemberDetails.MemberID).FirstOrDefault();
                if (MemberLevelDecision != null)
                {
                    ObjMemberDetails.ObjUwDecision.DecisionDate = MemberLevelDecision.DecisionDate;
                    ObjMemberDetails.ObjUwDecision.CommencementDate = MemberLevelDecision.Commencement_Date;
                    ObjMemberDetails.ObjUwDecision.Decision = MemberLevelDecision.Decision;
                    ObjMemberDetails.ObjUwDecision.Remarks = MemberLevelDecision.Remarks;
                    //Changed today 30-4-2018
                    descision = ObjMemberDetails.ObjUwDecision.Decision;
                    ObjMemberDetails.ObjUwDecision.UWReason = MemberLevelDecision.UWReason;
                    ObjMemberDetails.ObjUwDecision.UWMonth = MemberLevelDecision.UWDuration;
                    ObjMemberDetails.ObjUwDecision.UWMedicalCode = MemberLevelDecision.UWMedicalCode;
                    ObjMemberDetails.ObjUwDecision.MedicalFeePaidBy = MemberLevelDecision.MedicalFeePaidBy;

                }

            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                
            }
            return ObjMemberDetails;
        }

        public AIA.Life.Models.Common.MemberDetails AddNewRequiredDocuments(AIA.Life.Models.Common.MemberDetails objMemberDetails, string UserID, decimal PolicyID)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    Guid Createdby = new Guid(UserID);

                    #region Update  Medical Documents
                    foreach (var MedicalDOc in objMemberDetails.ObjUwDecision.lstUWMedicalDocument.Where(a => a.IsAdded == true || a.DocumentId == 0).ToList())
                    {
                        tblPolicyDocument objpolicyDocument = new tblPolicyDocument();
                        objpolicyDocument.DocumentType = "Medical";
                        objpolicyDocument.PolicyID = PolicyID;
                        objpolicyDocument.ItemType = "PolicyDocuments";
                        objpolicyDocument.MemberType = objMemberDetails.AssuredName;
                        objpolicyDocument.FileName = MedicalDOc.Document;
                        objpolicyDocument.CreatedBy = Createdby;
                        objpolicyDocument.CreatedDate = DateTime.Now;
                        objpolicyDocument.Remarks = MedicalDOc.Remarks;
                        objpolicyDocument.Decision = MedicalDOc.Status;
                        objpolicyDocument.isNewDocumentAdded = true;
                        Context.tblPolicyDocuments.Add(objpolicyDocument);

                    }
                    #endregion

                    #region Update Non Medical Documents
                    foreach (var NOnMedicalDOc in objMemberDetails.ObjUwDecision.lstUWNonMedicalDocument.Where(a => a.IsAdded == true || a.DocumentId == 0).ToList())
                    {
                        tblPolicyDocument objpolicyDocument = new tblPolicyDocument();
                        if (!string.IsNullOrEmpty(NOnMedicalDOc.DocType))
                            objpolicyDocument.DocumentType = NOnMedicalDOc.DocType;
                        else
                            objpolicyDocument.DocumentType = "";

                        objpolicyDocument.PolicyID = PolicyID;
                        objpolicyDocument.ItemType = "PolicyDocuments";
                        objpolicyDocument.MemberType = objMemberDetails.AssuredName;
                        objpolicyDocument.FileName = NOnMedicalDOc.Document;
                        objpolicyDocument.CreatedBy = Createdby;
                        objpolicyDocument.CreatedDate = DateTime.Now;
                        objpolicyDocument.Remarks = NOnMedicalDOc.Remarks;
                        objpolicyDocument.Decision = NOnMedicalDOc.Status;
                        objpolicyDocument.isNewDocumentAdded = true;
                        Context.tblPolicyDocuments.Add(objpolicyDocument);

                    }
                    #endregion

                    Context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);

            }
            return objMemberDetails;
        }
        public AIA.Life.Models.Common.MemberDetails UpdateDocumentStatus(AIA.Life.Models.Common.MemberDetails objMemberDetails, string UserID, Guid CommonGuid, bool IsIntermSave, decimal PolicyID)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    Guid Createdby = new Guid(UserID);

                    #region Add New Documents

                    #endregion

                    #region Update  Medical Documents
                    foreach (var MedicalDOc in objMemberDetails.ObjUwDecision.lstUWMedicalDocument.Where(a => a.IsAdded != true))
                    {
                        foreach (var MedicalDocument in Context.tblPolicyDocuments.Where(a => a.FileName == MedicalDOc.Document && a.ItemType == "PolicyDocuments" && a.MemberType == objMemberDetails.AssuredName && a.PolicyID == PolicyID).ToList())
                        {
                            MedicalDocument.Decision = MedicalDOc.Status;
                            MedicalDocument.CreatedBy = Createdby;
                            MedicalDocument.Remarks = MedicalDOc.Remarks;

                            #region Medical Doc History
                            if (!IsIntermSave)
                            {
                                tblPolicyDocumentHistory objpolicyDocumentHistory = new tblPolicyDocumentHistory();
                                objpolicyDocumentHistory.DocumentType = MedicalDocument.DocumentType;
                                objpolicyDocumentHistory.PolicyID = MedicalDocument.PolicyID;
                                objpolicyDocumentHistory.ItemType = MedicalDocument.ItemType;
                                objpolicyDocumentHistory.MemberType = MedicalDocument.MemberType;
                                objpolicyDocumentHistory.FileName = MedicalDocument.FileName;
                                objpolicyDocumentHistory.CreatedBy = Createdby;
                                objpolicyDocumentHistory.CreatedDate = DateTime.Now;
                                objpolicyDocumentHistory.Remarks = MedicalDOc.Remarks;
                                objpolicyDocumentHistory.Decision = MedicalDOc.Status;
                                objpolicyDocumentHistory.CommonID = CommonGuid;
                                Context.tblPolicyDocumentHistories.Add(objpolicyDocumentHistory);
                            }
                            #endregion

                            Context.SaveChanges();
                        }
                    }
                    #endregion


                    #region Update Non Medical Documents
                    foreach (var NonMedicalDOc in objMemberDetails.ObjUwDecision.lstUWNonMedicalDocument.Where(a => a.IsAdded != true))
                    {
                        foreach (var NonMedicalDocument in Context.tblPolicyDocuments.Where(a => a.FileName == NonMedicalDOc.Document && a.ItemType == "PolicyDocuments" && a.MemberType == objMemberDetails.AssuredName && a.PolicyID == PolicyID).ToList())
                        {
                            NonMedicalDocument.Decision = NonMedicalDOc.Status;

                            NonMedicalDocument.CreatedBy = Createdby;
                            NonMedicalDocument.Remarks = NonMedicalDOc.Remarks;


                            #region Non Medical Doc History
                            if (!IsIntermSave)
                            {
                                tblPolicyDocumentHistory objpolicyDocumentHistory = new tblPolicyDocumentHistory();
                                objpolicyDocumentHistory.DocumentType = NonMedicalDocument.DocumentType;
                                objpolicyDocumentHistory.PolicyID = NonMedicalDocument.PolicyID;
                                objpolicyDocumentHistory.ItemType = NonMedicalDocument.ItemType;
                                objpolicyDocumentHistory.MemberType = NonMedicalDocument.MemberType;
                                objpolicyDocumentHistory.FileName = NonMedicalDocument.FileName;
                                objpolicyDocumentHistory.CreatedBy = Createdby;
                                objpolicyDocumentHistory.CreatedDate = DateTime.Now;
                                objpolicyDocumentHistory.Remarks = NonMedicalDOc.Remarks;
                                objpolicyDocumentHistory.Decision = NonMedicalDOc.Status;
                                objpolicyDocumentHistory.CommonID = CommonGuid;
                                Context.tblPolicyDocumentHistories.Add(objpolicyDocumentHistory);
                            }
                            #endregion

                            Context.SaveChanges();
                        }
                    }
                    #endregion



                }
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);

            }
            return objMemberDetails;
        }

        public List<MemberUWHistory> GetMemberUWhistoryDetials(AIA.Life.Models.Common.MemberDetails ObjMemberDetails)
        {
            List<MemberUWHistory> LstMemberHistory = new List<MemberUWHistory>();
            try
            {

                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    foreach (var MemberHistory in Context.tblMemberLevelDecisionHistories.Where(a => a.MemberID == ObjMemberDetails.MemberID).ToList())
                    {
                        if (MemberHistory.CreatedBy != null)
                        {
                            var UserInfo = Context.AspNetUsers.Where(a => a.Id == MemberHistory.CreatedBy).FirstOrDefault();
                            if (UserInfo != null)
                            {
                                MemberUWHistory objMemberHistory = new MemberUWHistory();
                                objMemberHistory.Day = MemberHistory.CreatedDate.Value.Day;
                                objMemberHistory.Year = MemberHistory.CreatedDate.Value.Year;
                                objMemberHistory.Month = GetMonth(MemberHistory.CreatedDate.Value.Month);
                                objMemberHistory.UWName = UserInfo.UserName;
                                string DecisionDescription = string.Empty;
                                if (!string.IsNullOrEmpty(MemberHistory.Decision))
                                {
                                    int ID = Convert.ToInt32(MemberHistory.Decision);
                                    DecisionDescription = Context.tblMasCommonTypes.Where(a => a.CommonTypesID == ID).FirstOrDefault().Description;
                                    objMemberHistory.Decision = DecisionDescription;
                                }
                                else
                                {
                                    objMemberHistory.Decision = DecisionDescription;
                                }
                                if (MemberHistory.tblPolicyMemberDetail != null)
                                {
                                    var AllocatedDetails = Context.tblPolicyUWAllocations.Where(a => a.PolicyID == MemberHistory.tblPolicyMemberDetail.PolicyID).FirstOrDefault();
                                    if (AllocatedDetails != null)
                                    {
                                        objMemberHistory.ProposalReceivedDate = AllocatedDetails.AllocatedDate;
                                    }
                                    else
                                    {
                                        objMemberHistory.ProposalReceivedDate = DateTime.Now;// Test
                                    }
                                }
                                else
                                {
                                    objMemberHistory.ProposalReceivedDate = DateTime.Now;// Test
                                }

                                objMemberHistory.objListDocuments = new List<UWDocument>();

                                objMemberHistory.objListDocuments = (from Doc in Context.tblPolicyDocumentHistories.Where(a => a.CommonID == MemberHistory.CommonID && a.ItemType == "PolicyDocuments" && a.MemberType == ObjMemberDetails.AssuredName)
                                                                     select new UWDocument
                                                                     {
                                                                         Document = Doc.FileName,
                                                                         Remarks = Doc.Remarks,
                                                                         Status = Doc.Decision,
                                                                         DateTime = Doc.CreatedDate

                                                                     }).ToList();

                                LstMemberHistory.Add(objMemberHistory);
                            }
                        }
                    }
                }

                return LstMemberHistory;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                return LstMemberHistory;

            }
        }
        public string GetMonth(int? Month)
        {
            string _monthName = string.Empty;
            switch (Month)
            {
                case 1:
                _monthName = "Jan";
                break;


                case 2:
                _monthName = "Feb";
                break;

                case 3:
                _monthName = "March";
                break;

                case 4:
                _monthName = "Apr";
                break;

                case 5:
                _monthName = "May";
                break;

                case 6:
                _monthName = "June";
                break;

                case 7:
                _monthName = "July";
                break;

                case 8:
                _monthName = "Aug";
                break;

                case 9:
                _monthName = "Sep";
                break;
                case 10:
                _monthName = "Oct";
                break;
                case 11:
                _monthName = "Nov";
                break;
                case 12:
                _monthName = "Dec";
                break;

            }
            return _monthName;
        }

        public List<AIA.Life.Models.Common.DocumentUpload> FetchPolicyDocuments(List<tblPolicyDocument> objListPolicyDocuments, List<string> LstAssured, bool IsPendingRequirement = false)
        {
            List<AIA.Life.Models.Common.DocumentUpload> LstDocuments = new List<DocumentUpload>();
            List<string> FileTypes = objListPolicyDocuments.Select(b => b.FileName).ToList();
            foreach (var Member in LstAssured.Distinct())
            {
                foreach (var FileType in objListPolicyDocuments)
                {
                    int DocIndex = 0;
                    AIA.Life.Models.Common.DocumentUpload objDocument = new DocumentUpload();
                    bool DocExisits = false;
                    objDocument.objlstDocuments = new List<DocumentUpload>();
                    if (IsPendingRequirement)
                    {
                        #region Pending Req Case
                        foreach (var Doc in objListPolicyDocuments.Where(a => a.MemberType == Member && a.FileName == FileType.FileName && (a.Decision == "2368" || a.Decision == null)).ToList())//only outstanding documnets has to be shown in WP/Banca
                        {
                            DocExisits = true;
                            objDocument.Document_Name = Doc.FileName;
                            objDocument.MemberType = Member;
                            // list of Documents
                            DocumentUpload objSubDoc = new DocumentUpload();
                            objSubDoc.ExistingFileName = Doc.FilePath;
                            objSubDoc.HiddenFilePath = Doc.FilePath;
                            objSubDoc.FileName = Path.GetFileName(Doc.FilePath);
                            objSubDoc.DOCID = Convert.ToInt32(Doc.DocumentUploadID);
                            objDocument.objlstDocuments.Add(objSubDoc);
                            // till here
                        }
                        #endregion
                    }
                    else
                    {
                        #region Other Cases
                        foreach (var Doc in objListPolicyDocuments.Where(a => a.MemberType == Member && a.FileName == FileType.FileName && a.DocumentUploadID == FileType.DocumentUploadID).ToList())
                        {
                            DocExisits = true;
                            objDocument.Document_Name = Doc.FileName;
                            objDocument.MemberType = Member;
                            // list of Documents
                            DocumentUpload objSubDoc = new DocumentUpload();
                            objSubDoc.ExistingFileName = Doc.FilePath;
                            objSubDoc.HiddenFilePath = Doc.FilePath;
                            objSubDoc.FileName = Path.GetFileName(Doc.FilePath);
                            objSubDoc.DOCID = Convert.ToInt32(Doc.DocumentUploadID);
                            objDocument.objlstDocuments.Add(objSubDoc);
                            // till here
                        }
                        #endregion
                    }

                    objDocument.DocIndex = DocIndex;
                    objDocument.DOCID = Convert.ToInt32(FileType.DocumentUploadID);

                    if (DocExisits)
                    {
                        DocIndex = DocIndex + 1;
                        LstDocuments.Add(objDocument);
                    }

                }
            }
            return LstDocuments;
        }
        public List<AIA.Life.Models.Common.DocumentUpload> FetchPolicyDocuments1(List<tblPolicyDocument> objListPolicyDocuments, List<string> LstAssured, bool IsPendingRequirement = false)
        {
            List<AIA.Life.Models.Common.DocumentUpload> LstDocuments = new List<DocumentUpload>();
            List<string> FileTypes = objListPolicyDocuments.DistinctBy(a => a.FileName).Select(b => b.FileName).ToList();
            foreach (var Member in LstAssured.Distinct())
            {
                foreach (var FileType in FileTypes)
                {
                    int DocIndex = 0;
                    AIA.Life.Models.Common.DocumentUpload objDocument = new DocumentUpload();
                    bool DocExisits = false;
                    objDocument.objlstDocuments = new List<DocumentUpload>();
                    if (IsPendingRequirement)
                    {
                        #region Pending Req Case
                        foreach (var Doc in objListPolicyDocuments.Where(a => a.MemberType == Member && a.FileName == FileType && (a.Decision == "2368" || a.Decision == null)).ToList())//only outstanding documnets has to be shown in WP/Banca
                        {
                            DocExisits = true;
                            objDocument.Document_Name = Doc.FileName;
                            objDocument.MemberType = Member;
                            // list of Documents
                            DocumentUpload objSubDoc = new DocumentUpload();
                            objSubDoc.ExistingFileName = Doc.FilePath;
                            objSubDoc.HiddenFilePath = Doc.FilePath;
                            objSubDoc.FileName = Path.GetFileName(Doc.FilePath);
                            objSubDoc.DOCID = Convert.ToInt32(Doc.DocumentUploadID);
                            objDocument.objlstDocuments.Add(objSubDoc);
                            // till here
                        }
                        #endregion
                    }
                    else
                    {
                        #region Other Cases
                        foreach (var Doc in objListPolicyDocuments.Where(a => a.MemberType == Member && a.FileName == FileType).ToList())
                        {
                            DocExisits = true;
                            objDocument.Document_Name = Doc.FileName;
                            objDocument.MemberType = Member;
                            // list of Documents
                            DocumentUpload objSubDoc = new DocumentUpload();
                            objSubDoc.ExistingFileName = Doc.FilePath;
                            objSubDoc.HiddenFilePath = Doc.FilePath;
                            objSubDoc.FileName = Path.GetFileName(Doc.FilePath);
                            objSubDoc.DOCID = Convert.ToInt32(Doc.DocumentUploadID);
                            objDocument.objlstDocuments.Add(objSubDoc);
                            // till here
                        }
                        #endregion
                    }

                    objDocument.DocIndex = DocIndex;

                    if (DocExisits)
                    {
                        DocIndex = DocIndex + 1;
                        LstDocuments.Add(objDocument);
                    }

                }
            }
            return LstDocuments;
        }

        public DeletePolicyDocuments DeletePolicyDocument(DeletePolicyDocuments ObjPolicyDocuments)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    var DocList = Context.tblPolicyDocuments.Where(a => a.MemberType == ObjPolicyDocuments.MemberType && a.FileName == ObjPolicyDocuments.DocumentName && a.DocumentUploadID == ObjPolicyDocuments.DocID).FirstOrDefault();
                    if (DocList != null)
                    {
                        Context.tblPolicyDocuments.Remove(DocList);
                        Context.SaveChanges();
                        ObjPolicyDocuments.Message = "Success";
                        return ObjPolicyDocuments;
                    }
                }
                ObjPolicyDocuments.Message = "Success";
                return ObjPolicyDocuments;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                ObjPolicyDocuments.Message = "Error";
                return ObjPolicyDocuments;
            }


        }

        public DeletePolicyDocuments DeleteDocumentLinkPolicyDocument(DeletePolicyDocuments ObjPolicyDocuments)
        {
            try
            {

                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    var DocList = Context.tblPolicyDocuments.Where(a => a.MemberType == ObjPolicyDocuments.MemberType && a.FileName == ObjPolicyDocuments.DocumentName && a.PolicyID == ObjPolicyDocuments.DocumentPolicyID).ToList();
                    //var DocList1 = Context.tblPolicyDocuments.Where(a => a.MemberType == ObjPolicyDocuments.MemberType && a.FileName == ObjPolicyDocuments.DocumentName).ToList();
                    //var query = Context.tblPolicyDocuments.AsEnumerable().Where(r => r.Field<string>("col1") == "ali");
                    tblPolicyDocument objPolicyDocument = new tblPolicyDocument();
                    if (DocList != null)
                    {
                        tblPolicyDocument ExistingPolicyDocuments = Context.tblPolicyDocuments.Where(a => a.MemberType == ObjPolicyDocuments.MemberType && a.FileName == ObjPolicyDocuments.DocumentName && a.PolicyID == ObjPolicyDocuments.DocumentPolicyID).ToList().FirstOrDefault();
                        if (ExistingPolicyDocuments != null)
                        {
                            objPolicyDocument.FileName = ExistingPolicyDocuments.FileName;
                            objPolicyDocument.File = ExistingPolicyDocuments.File;
                            objPolicyDocument.PolicyID = ExistingPolicyDocuments.PolicyID;
                            objPolicyDocument.DocumentType = ExistingPolicyDocuments.DocumentType;
                            objPolicyDocument.ItemType = ExistingPolicyDocuments.ItemType;
                            objPolicyDocument.MemberType = ExistingPolicyDocuments.MemberType;
                            objPolicyDocument.CreatedDate = ExistingPolicyDocuments.CreatedDate;
                            objPolicyDocument.DocumentUploadID = ExistingPolicyDocuments.DocumentUploadID;
                            objPolicyDocument.FilePath = null;
                            Context.Entry(ExistingPolicyDocuments).CurrentValues.SetValues(objPolicyDocument);
                            Context.SaveChanges();
                        }

                        ObjPolicyDocuments.Message = "Success";
                        return ObjPolicyDocuments;
                    }
                }
                ObjPolicyDocuments.Message = "Success";
                return ObjPolicyDocuments;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                ObjPolicyDocuments.Message = "Error";
                return ObjPolicyDocuments;
            }


        }

        public MemberLevelDecisions DerivePolicyLevelDecision(MemberLevelDecisions objMemberDecision)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    string MainLifeDecision = string.Empty;
                    var Mainlife = objMemberDecision.objDecisions.Where(a => a.AssuredName == "MainLife").FirstOrDefault();
                    if (Mainlife != null)
                    {
                        MainLifeDecision = (Mainlife.Decision != null ? Mainlife.Decision : "");
                    }

                    var idParam1 = new SqlParameter
                    {
                        ParameterName = "MainLifeStatus",
                        Value = MainLifeDecision,
                    };

                    string SPOUSEDecision = string.Empty;
                    var Spouse = objMemberDecision.objDecisions.Where(a => a.AssuredName == "Spouse").FirstOrDefault();
                    if (Spouse != null)
                    { SPOUSEDecision = (Spouse.Decision != null ? Spouse.Decision : ""); }

                    var idParam2 = new SqlParameter
                    {
                        ParameterName = "SpouseStatus",
                        Value = SPOUSEDecision,
                    };


                    string Child1Decision = string.Empty;
                    var Child1 = objMemberDecision.objDecisions.Where(a => a.AssuredName == "Child1").FirstOrDefault();
                    if (Child1 != null)
                    { Child1Decision = (Child1.Decision != null ? Child1.Decision : ""); }

                    var idParam3 = new SqlParameter
                    {
                        ParameterName = "Child1Status",
                        Value = Child1Decision,
                    };


                    string Child2Decision = string.Empty;
                    var Child2 = objMemberDecision.objDecisions.Where(a => a.AssuredName == "Child2").FirstOrDefault();
                    if (Child2 != null)
                    { Child2Decision = (Child2.Decision != null ? Child2.Decision : ""); }

                    var idParam4 = new SqlParameter
                    {
                        ParameterName = "Child2Status",
                        Value = Child2Decision,
                    };


                    string Child3Decision = string.Empty;
                    var Child3 = objMemberDecision.objDecisions.Where(a => a.AssuredName == "Child3").FirstOrDefault();
                    if (Child3 != null)
                    { Child3Decision = (Child3.Decision != null ? Child3.Decision : ""); }

                    var idParam5 = new SqlParameter
                    {
                        ParameterName = "Child3Status",
                        Value = Child3Decision,
                    };


                    string Child4Decision = string.Empty;
                    var Child4 = objMemberDecision.objDecisions.Where(a => a.AssuredName == "Child4").FirstOrDefault();
                    if (Child4 != null)
                    { Child4Decision = (Child4.Decision != null ? Child4.Decision : ""); }

                    var idParam6 = new SqlParameter
                    {
                        ParameterName = "Child4Status",
                        Value = Child4Decision,
                    };


                    string Child5Decision = string.Empty;
                    var Child5 = objMemberDecision.objDecisions.Where(a => a.AssuredName == "Child5").FirstOrDefault();
                    if (Child5 != null)
                    { Child5Decision = (Child5.Decision != null ? Child5.Decision : ""); }

                    var idParam7 = new SqlParameter
                    {
                        ParameterName = "Child5Status",
                        Value = Child5Decision,
                    };
                    var Result = Context.Database.SqlQuery<Int32>(
                        "exec usp_GetProposalStatus   @MainLifeStatus ,@SpouseStatus ,@Child1Status,@Child2Status, @Child3Status ,@Child4Status, @Child5Status", idParam1, idParam2, idParam3, idParam4, idParam5, idParam6, idParam7).FirstOrDefault();
                    //  if (!string.IsNullOrEmpty(Result))
                    if (Result > 0)
                    {
                        //  int id = Convert.ToInt32(Result);
                        var Commontype = Context.tblMasCommonTypes.Where(a => a.CommonTypesID == Result).FirstOrDefault();
                        if (Commontype != null)
                        {
                            objMemberDecision.Result = Convert.ToString(Commontype.CommonTypesID);
                            objMemberDecision.ResultText = Commontype.Description;

                        }
                    }
                    objMemberDecision.Message = "Success";
                    return objMemberDecision;
                }
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                objMemberDecision.Message = "Error";
                return objMemberDecision;
            }
        }
        public AIA.Life.Models.Reports.UWDecisionReport UWDecisionReport(AIA.Life.Models.Reports.UWDecisionReport objUWDecisionReport)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {

                    var PolicyInfo = Context.tblPolicies.Where(a => a.ProposalNo == objUWDecisionReport.ProposalNo).FirstOrDefault();
                    if (PolicyInfo != null)
                    {
                        UWDecisionReport objUWReport = new UWDecisionReport();
                        objUWReport.ListAssured = new List<string>();
                        if (PolicyInfo.PlanID != null)
                        {
                            var PlanInfo = Context.tblProducts.Where(a => a.ProductId == PolicyInfo.PlanID).FirstOrDefault();
                            if (PlanInfo != null)
                            {
                                objUWReport.Product = PlanInfo.ProductName;
                            }
                        }

                        objUWReport.Commencement = DateTime.Now; // Test Purpose
                        objUWReport.PolicyNo = PolicyInfo.PolicyNo;

                        #region Policy Level Decision
                        var PolicylevelRemarks = Context.tblPolicyUWRemarks.Where(a => a.PolicyID == PolicyInfo.PolicyID).FirstOrDefault();
                        if (PolicylevelRemarks != null)
                        {
                            objUWReport.Decision = PolicylevelRemarks.Decision;
                            objUWReport.DateTime = PolicylevelRemarks.CreatedDate;
                        }

                        #endregion


                        objUWReport.objMemberDetails = new List<AssuredMembers>();
                        objUWReport.objLstRiderInfo = new List<RiderInfo>();

                        #region Distinct Rider Info
                        List<RiderInfo> BenefitOverView = (from Member in Context.tblPolicyMemberDetails.Where(a => a.PolicyID == PolicyInfo.PolicyID && a.IsDeleted != true)
                                                           join MemberBenefit in Context.tblPolicyMemberBenefitDetails
                                                           on Member.MemberID equals MemberBenefit.MemberID
                                                           join MasBenefit in Context.tblRiders
                                                           on MemberBenefit.BenifitID equals MasBenefit.RiderId
                                                           select new AIA.Life.Models.Reports.RiderInfo
                                                           {
                                                               RiderName = MasBenefit.RiderName,
                                                               RiderID = MasBenefit.RiderId
                                                           }).DistinctBy(a => a.RiderID).ToList();
                        #endregion

                        foreach (var Member in PolicyInfo.tblPolicyMemberDetails.Where(a => a.IsDeleted != true).ToList())
                        {
                            AssuredMembers objAssuredMember = new AssuredMembers();
                            objAssuredMember.Age = Member.Age;
                            objAssuredMember.AssuredName = Member.Assuredname;
                            objUWReport.ListAssured.Add(objAssuredMember.AssuredName);
                            if (Member.OccupationID != null)
                            {
                                try
                                {
                                    objAssuredMember.Occupation = Context.tblMasLifeOccupations.Where(a => a.ID == Member.OccupationID).FirstOrDefault().OccupationCode;
                                }
                                catch (Exception)
                                {
                                    objAssuredMember.Occupation = string.Empty;
                                }
                            }


                            objAssuredMember.Name = Member.FirstName;
                            objAssuredMember.MedicalSAR = "34"; // Test
                            objAssuredMember.FinancialSAR = "22"; // test
                            objAssuredMember.ListMedicalRequirements = Context.tblPolicyDocuments.Where(a => a.MemberType == Member.Assuredname && a.PolicyID == Member.PolicyID && a.ItemType == "PolicyDocuments" && a.DocumentType == "Medical").Select(a => a.FileName).ToList();
                            objAssuredMember.ListFianacialRequirements = Context.tblPolicyDocuments.Where(a => a.MemberType == Member.Assuredname && a.PolicyID == Member.PolicyID && a.ItemType == "PolicyDocuments" && a.DocumentType != "Medical").Select(a => a.FileName).ToList();
                            objAssuredMember.objUWDeviations = new List<UWDeviationInfo>();

                            #region Fetch Rider Information
                            for (int j = 0; j < BenefitOverView.Count(); j++)
                            {
                                RiderRelation objAssuredRelation = new RiderRelation();
                                objAssuredRelation.Assured_Name = Member.Assuredname;
                                objAssuredRelation.Member_Relationship = Convert.ToString(Member.RelationShipWithProposer);
                                BenefitOverView[j].objBenefitMemberRelationship.Add(objAssuredRelation);
                            }

                            foreach (var Rider in Context.tblPolicyMemberBenefitDetails.Where(a => a.MemberID == Member.MemberID).ToList())
                            {

                                int BenefitIndex = BenefitOverView.FindIndex(a => a.RiderID == Rider.BenifitID);
                                if (BenefitOverView[BenefitIndex].objBenefitMemberRelationship == null)
                                {
                                    BenefitOverView[BenefitIndex].objBenefitMemberRelationship = new List<RiderRelation>();
                                }
                                int BasicCoverMemberIndex = BenefitOverView[BenefitIndex].objBenefitMemberRelationship.FindIndex(a => a.Assured_Name == Member.Assuredname);
                                BenefitOverView[BenefitIndex].objBenefitMemberRelationship[BasicCoverMemberIndex].RiderCurrentSI = Rider.SumInsured;
                                BenefitOverView[BenefitIndex].objBenefitMemberRelationship[BasicCoverMemberIndex].Rider_Premium = Rider.Premium;
                            }
                            #endregion

                            #region Fetch Deviation Info
                            foreach (var Deviation in Member.tblMemberDeviationInfoes.ToList())
                            {
                                UWDeviationInfo objDeviation = new UWDeviationInfo();
                                objDeviation.DeviationMessage = Deviation.Reason;
                                objDeviation.Decision = Deviation.Reason;
                                objDeviation.Date = Deviation.CreateDate;
                                objDeviation.Remarks = Deviation.Reason;
                                if (Deviation.CreatedBy != null)
                                {
                                    objDeviation.User = Context.AspNetUsers.Where(a => a.Id == Deviation.CreatedBy).FirstOrDefault().UserName;
                                }
                                objAssuredMember.objUWDeviations.Add(objDeviation);
                            }
                            #endregion

                            objUWReport.objMemberDetails.Add(objAssuredMember);
                        }
                        objUWReport.objLstRiderInfo = BenefitOverView;  // Set Rider Info

                    }


                }
                objUWDecisionReport.Message = "Success";
            }
            catch (Exception ex)
            {
                objUWDecisionReport.Message = "Error";
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);

            }
            return objUWDecisionReport;
        }

        /// <summary>
        /// Save Member Level Loading Information
        /// </summary>
        /// <param name="objPolicy"></param>
        /// <returns></returns>
        public AIA.Life.Models.Policy.Policy SaveLoadingDetails(AIA.Life.Models.Policy.Policy objPolicy)
        {
            try
            {
                int memberRelation = 0;
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    if (objPolicy.objMemberDetails != null)
                    {

                        var UserInfo = Context.AspNetUsers.Where(a => a.UserName == objPolicy.UserName).FirstOrDefault();
                        if (UserInfo != null)
                        {
                            if (objPolicy.objMemberDetails[0].objBenifitDetails.Count() > 0)
                            {
                                #region Save Rider Level Loading Info
                                decimal MemberID = objPolicy.objMemberDetails[0].MemberID;
                                var MemberDetail = Context.tblPolicyMemberDetails.Where(a => a.MemberID == MemberID).FirstOrDefault();
                                memberRelation = MemberDetail.RelationShipWithProposer;
                                string ExtraPrem = string.Empty;
                                if (MemberDetail != null)
                                {
                                    ExtraPrem = objPolicy.objMemberDetails[0].ExtraPremium;
                                    var PolicyInfo = Context.tblPolicies.Where(a => a.PolicyID == MemberDetail.PolicyID).FirstOrDefault();
                                    objPolicy.objMemberDetails[0].objBenifitDetails = objPolicy.objMemberDetails[0].objBenifitDetails.Where(a => a.IsDeleted != true).ToList();
                                    //List<int> wopRiders = Context.tblProductPlanRiders.Where(a => a.RiderId == 10).Select(a => a.ProductPlanRiderId).ToList();
                                    for (int i = 0; i < objPolicy.objMemberDetails[0].objBenifitDetails.Count(); i++)
                                    {
                                        if (objPolicy.objMemberDetails[0].objBenifitDetails[i].IsDeleted != true)
                                        {
                                            tblMemberBenefitOtherDetail objRiderDetails = new tblMemberBenefitOtherDetail();
                                            objRiderDetails.MemberBenifitID = objPolicy.objMemberDetails[0].objBenifitDetails[i].MemberBenifitID;
                                            objRiderDetails.LoadingType = objPolicy.objMemberDetails[0].objBenifitDetails[i].LoadingType;
                                            objRiderDetails.LoadingBasis = objPolicy.objMemberDetails[0].objBenifitDetails[i].LoadingBasis;
                                            objRiderDetails.LoadingAmount = objPolicy.objMemberDetails[0].objBenifitDetails[i].LoadingAmount;
                                            objRiderDetails.Exclusion = objPolicy.objMemberDetails[0].objBenifitDetails[i].Exclusion;
                                            objRiderDetails.ExtraPremium = objPolicy.objMemberDetails[0].objBenifitDetails[i].ExtraPremium;
                                            if (objPolicy.objMemberDetails[0].objBenifitDetails[i].MemberBenefitDetailID > 0)
                                            {
                                                decimal ID = objPolicy.objMemberDetails[0].objBenifitDetails[i].MemberBenefitDetailID;
                                                tblMemberBenefitOtherDetail objExisitingRiderDetails = Context.tblMemberBenefitOtherDetails.Where(a => a.MemberBenifitDetailsID == ID).FirstOrDefault();
                                                objRiderDetails.MemberBenifitDetailsID = objExisitingRiderDetails.MemberBenifitDetailsID;
                                                objRiderDetails.CreatedBy = objExisitingRiderDetails.CreatedBy;
                                                objRiderDetails.CreatedDate = objExisitingRiderDetails.CreatedDate;
                                                Context.Entry(objExisitingRiderDetails).CurrentValues.SetValues(objRiderDetails);
                                            }
                                            else
                                            {
                                                objRiderDetails.CreatedBy = UserInfo.Id;
                                                objRiderDetails.CreatedDate = DateTime.Now;
                                                Context.tblMemberBenefitOtherDetails.Add(objRiderDetails);
                                            }


                                        }

                                    }
                                    //MemberDetail.AdditionalPremium = objPolicy.objMemberDetails[0].ExtraPremium;
                                    //Context.SaveChanges();
                                    //UpdateAdditionalPremiumInProposalLevel(MemberDetail.PolicyID);

                                    objPolicy.ProposalNo = MemberDetail.tblPolicy.ProposalNo;
                                    objPolicy.QuoteNo = MemberDetail.tblPolicy.QuoteNo;
                                    objPolicy.PolicyID = MemberDetail.tblPolicy.PolicyID;
                                }

                                MemberDetail.AdditionalPremium = ExtraPrem;
                                Context.SaveChanges();
                                UpdateMemberLevelInfo(MemberDetail.MemberID);
                                UpdateAdditionalPremiumInProposalLevel(MemberDetail.PolicyID);
                                #endregion


                            }
                        }

                    }
                }
                objPolicy = CalculateSaveWopLoadingDetails(objPolicy);
                objPolicy.Message = "Success";
                return objPolicy;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                objPolicy.Message = "Error";
                return objPolicy;
            }
        }
        public AIA.Life.Models.Policy.Policy CalculateSaveWopLoadingDetails(AIA.Life.Models.Policy.Policy objPolicy)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    List<MemberDetails> lstMembers = new List<MemberDetails>();
                    decimal policyId = 0M;
                    if (objPolicy.objMemberDetails != null)
                    {
                        #region Load Required Masters
                        objPolicy.objMemberDetails[0].LstLoadingType = (from CommonType in Context.tblMasCommonTypes.Where(a => a.MasterType == "LoadingType")
                                                                        select new MasterListItem
                                                                        {
                                                                            ID = CommonType.CommonTypesID,
                                                                            Text = CommonType.Description
                                                                        }).ToList();
                        objPolicy.objMemberDetails[0].LstLoadingBasis = (from CommonType in Context.tblMasCommonTypes.Where(a => a.MasterType == "LoadingBasis")
                                                                         select new MasterListItem
                                                                         {
                                                                             ID = CommonType.CommonTypesID,
                                                                             Text = CommonType.Description
                                                                         }).ToList();
                        #endregion

                        #region Map Rider Information to Xml Object to pass to Store Procedure
                        decimal MemberID = objPolicy.objMemberDetails[0].MemberID;
                        var MemberDetail = Context.tblPolicyMemberDetails.Where(a => a.MemberID == MemberID).FirstOrDefault();
                        
                        if (MemberDetail != null)
                        {
                            var PolicyInfo = Context.tblPolicies.Where(a => a.PolicyID == MemberDetail.PolicyID).FirstOrDefault();
                            policyId = PolicyInfo.PolicyID;
                            var tblMembers = PolicyInfo.tblPolicyMemberDetails.Where(a => a.IsDeleted != true).ToList();
                            // For temp Purpose to Fetch product and plan as these are not saved at proposal level
                            var LifeQQ = Context.tblLifeQQs.Where(a => a.QuoteNo == PolicyInfo.QuoteNo).FirstOrDefault();
                            AIA.Life.Models.UWDecision.ProposalDetails objProposalDetails = new AIA.Life.Models.UWDecision.ProposalDetails();
                            objProposalDetails.Product = new Product();
                            objProposalDetails.Product.ProductId = Convert.ToString(LifeQQ.ProductNameID);
                            objProposalDetails.Product.PlanId = Convert.ToString(LifeQQ.PlanId);
                            objProposalDetails.Product.ProposalPremium = Convert.ToString(PolicyInfo.tblProposalPremiums.FirstOrDefault().AnnualPremium);
                            objProposalDetails.Product.ProposalNo = PolicyInfo.ProposalNo;
                            objProposalDetails.Product.PolicyTerm = Convert.ToString(LifeQQ.PolicyTermID);
                            objProposalDetails.Product.PaymentFrequency = Convert.ToInt32(LifeQQ.PreferredTerm).ToString();
                            objProposalDetails.Product.ApplyOccupationLoading = "1";
                            objProposalDetails.Member = new List<Member>();

                            foreach (var item in tblMembers)
                            {
                                MemberDetails memberDetails = new MemberDetails();
                                memberDetails = FetchMemberDetails(item, memberDetails);
                                memberDetails.objBenifitDetails.AddRange((from Member in Context.tblPolicyMemberDetails.Where(a => a.MemberID == item.MemberID && a.IsDeleted != true)
                                                                          join MemberBenefit in Context.tblPolicyMemberBenefitDetails.Where(b => b.IsDeleted != true)
                                                                          on Member.MemberID equals MemberBenefit.MemberID
                                                                          join MasBenefit in Context.tblProductPlanRiders
                                                                          on MemberBenefit.BenifitID equals MasBenefit.ProductPlanRiderId
                                                                          join OtherDetails in Context.tblMemberBenefitOtherDetails
                                                                          on MemberBenefit.MemberBenifitID equals OtherDetails.MemberBenifitID into LoadingInfo
                                                                          from Loading in LoadingInfo.DefaultIfEmpty()
                                                                          orderby MasBenefit.DisplayOrder ascending
                                                                          select new BenifitDetails
                                                                          {
                                                                              RiderID = MasBenefit.RiderId ?? 0,
                                                                              BenifitName = MasBenefit.DisplayName,
                                                                              RiderCode = MasBenefit.RefRiderCode,
                                                                              BenefitID = (int)MasBenefit.ProductPlanRiderId,
                                                                              MemberBenifitID = MemberBenefit.MemberBenifitID != null ? MemberBenefit.MemberBenifitID : 0,
                                                                              RiderSuminsured = MemberBenefit.SumInsured,
                                                                              ActualRiderPremium = MemberBenefit.Premium,
                                                                              RiderPremium = MemberBenefit.TotalPremium,
                                                                              LoadingType = Loading.LoadingType,
                                                                              LoadingBasis = Loading.LoadingBasis,
                                                                              ExtraPremium = Loading.ExtraPremium,
                                                                              LoadingAmount = Loading.LoadingAmount,
                                                                              LoadingPercentage = (Loading.LoadingType=="2204" ? Loading.LoadingAmount : "0").ToString(),
                                                                              LoadinPerMille = (Loading.LoadingType == "2203" ? Loading.LoadingAmount : "0").ToString(),
                                                                              MemberBenefitDetailID = Loading.MemberBenifitDetailsID != null ? Loading.MemberBenifitDetailsID : 0
                                                                          }).ToList());
                                lstMembers.Add(memberDetails);
                            }
                            
                            for (int i = 0; i < lstMembers.Count; i++)
                            {
                                Member objMember = new Member();
                                objMember.Age = lstMembers[i].Age.ToString();
                                objMember.Id = (i + 1).ToString();
                                switch (lstMembers[i].RelationShipWithPropspect)
                                {
                                    case "267":
                                        objMember.Relation = "1";
                                        break;
                                    case "268":
                                        objMember.Relation = "2";
                                        break;
                                    default:
                                        objMember.Relation = "3";
                                        break;
                                }
                                objMember.Rider = new List<Rider>();
                                int riderLoadingIndex = 0;
                                for (int j = 0; j < lstMembers[i].objBenifitDetails.Count(); j++)
                                {
                                    lstMembers[i].objBenifitDetails[j].RiderLoadingIndex = riderLoadingIndex;
                                    int benefitID = lstMembers[i].objBenifitDetails[j].BenefitID;
                                    lstMembers[i].objBenifitDetails[j].RiderID = Context.tblProductPlanRiders.Where(a => a.ProductPlanRiderId == benefitID).Select(a => a.RiderId).FirstOrDefault() ?? 0;
                                    if (lstMembers[i].objBenifitDetails[j].IsDeleted != true)
                                    {
                                        Rider objRider = new Rider();
                                        objRider.RowId = riderLoadingIndex.ToString();
                                        objRider.BenefitId = Convert.ToString(lstMembers[i].objBenifitDetails[j].BenefitID);
                                        objRider.ExtraPremium = string.IsNullOrEmpty(lstMembers[i].objBenifitDetails[j].ExtraPremium) == true ? "0" : lstMembers[i].objBenifitDetails[j].ExtraPremium;
                                        objRider.BasicPremium= lstMembers[i].objBenifitDetails[j].ActualRiderPremium;
                                        if (lstMembers[i].objBenifitDetails[j].RiderID == 10)
                                        {
                                            if(lstMembers[i].objBenifitDetails[j].LoadingType=="2204")
                                                objRider.LoadingPer = lstMembers[i].objBenifitDetails[j].LoadingPercentage;
                                            else if (lstMembers[i].objBenifitDetails[j].LoadingType == "2203")
                                                objRider.LoadingPerMille = lstMembers[i].objBenifitDetails[j].LoadinPerMille;
                                        }
                                        objMember.Rider.Add(objRider);
                                    }
                                    riderLoadingIndex++;
                                }
                                objProposalDetails.Member.Add(objMember);
                            }


                            #endregion

                            #region Object To Xml Convertion
                            StringWriter sw = new StringWriter();
                            XmlTextWriter tw = null;
                            string strOutPut = string.Empty;
                            using (var ms = new MemoryStream())
                            {
                                var xw = XmlWriter.Create(ms);// Remember to stop using XmlTextWriter  
                                var serializer = new XmlSerializer(objProposalDetails.GetType());
                                serializer.Serialize(xw, objProposalDetails);
                                xw.Flush();
                                ms.Seek(0, SeekOrigin.Begin);
                                var sr = new StreamReader(ms, System.Text.Encoding.UTF8);
                                strOutPut = sr.ReadToEnd();
                            }
                            #endregion

                            #region  Log Input 
                            tbllogxml objlogxml = new tbllogxml();
                            objlogxml.Description = "ApplyLoadingOnWOP Save";
                            objlogxml.PolicyID = Convert.ToString(MemberDetail.PolicyID);
                            objlogxml.UserID = string.Empty;
                            objlogxml.XMlData = strOutPut;
                            objlogxml.CreatedDate = DateTime.Now;
                            Context.tbllogxmls.Add(objlogxml);
                            #endregion
                            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                            con.Open();
                            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                            cmd.Connection = con;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "usp_ApplyLoadingOnWOP";
                            cmd.Parameters.Add("@s", SqlDbType.VarChar);
                            cmd.Parameters[0].Value = strOutPut;
                            DataSet ds = new DataSet();
                            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                            da.Fill(ds);
                            List<LoadingPremiumOutput> Result = new List<LoadingPremiumOutput>();
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                LoadingPremiumOutput objLoadingPremium = new LoadingPremiumOutput();
                                objLoadingPremium.RowId = Convert.ToInt32(ds.Tables[0].Rows[i]["RowId"].ToString());
                                objLoadingPremium.RiderID = ds.Tables[0].Rows[i]["BenefitId"].ToString();
                                objLoadingPremium.SumAssured = ds.Tables[0].Rows[i]["SumAssured"].ToString();
                                objLoadingPremium.RiderPremium = ds.Tables[0].Rows[i]["RiderPremium"].ToString();
                                if (ds.Tables[0].Rows[i]["ExtraPremium"] is DBNull)
                                {
                                    objLoadingPremium.ExtraPremium = decimal.Zero;
                                }
                                else { objLoadingPremium.ExtraPremium = Convert.ToDecimal(ds.Tables[0].Rows[i]["ExtraPremium"]); }
                                if (ds.Tables[0].Rows[i]["TotalPremium"] is DBNull)
                                {
                                    objLoadingPremium.TotalPremium = decimal.Zero;
                                }
                                else { objLoadingPremium.TotalPremium = Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalPremium"]); }
                                Result.Add(objLoadingPremium);
                            }

                            #region Map Loading Premium to Riders
                            MemberDetails mainLife = lstMembers.Where(a => a.RelationShipWithPropspect == "267").FirstOrDefault();
                            for (int i = 0; i < Result.Count(); i++)
                            {
                                int _index = mainLife.objBenifitDetails.FindIndex(a => a.RiderLoadingIndex == Result[i].RowId);
                                if (_index >= 0)
                                {
                                    mainLife.objBenifitDetails[_index].RiderSuminsured = Result[i].SumAssured;
                                    mainLife.objBenifitDetails[_index].ExtraPremium = Convert.ToString(Result[i].ExtraPremium);
                                    mainLife.objBenifitDetails[_index].RiderPremium = Result[i].RiderPremium;
                                    mainLife.objBenifitDetails[_index].TotalPremium = Convert.ToString(Result[i].TotalPremium);
                                    decimal memberBenifitDetailsID = mainLife.objBenifitDetails[_index].MemberBenefitDetailID;
                                    decimal memberBenefitId = mainLife.objBenifitDetails[_index].MemberBenifitID;
                                    tblPolicyMemberBenefitDetail memberBenefitDetail = Context.tblPolicyMemberBenefitDetails.Where(a => a.MemberBenifitID == memberBenefitId).FirstOrDefault();
                                    memberBenefitDetail.SumInsured = mainLife.objBenifitDetails[_index].RiderSuminsured;
                                    memberBenefitDetail.Premium = mainLife.objBenifitDetails[_index].RiderPremium;
                                    tblMemberBenefitOtherDetail benefitOtherDetail = Context.tblMemberBenefitOtherDetails.Where(a => a.MemberBenifitDetailsID == memberBenifitDetailsID).FirstOrDefault();
                                    benefitOtherDetail.ExtraPremium = mainLife.objBenifitDetails[_index].ExtraPremium;
                                    Context.SaveChanges();
                                }
                                
                            }
                            UpdateWopMemberLevelInfo(mainLife.MemberID);
                            UpdateAdditionalPremiumInProposalLevelWhenWop(policyId);
                            #endregion

                        }
                    }
                }
                return objPolicy;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                objPolicy.Message = "Error";
                return objPolicy;
            }
        }
        public void UpdateWopMemberLevelInfo(decimal MemberID)
        {
            #region Update Premium Info
            try
            {
                using (AVOAIALifeEntities SubContext = new AVOAIALifeEntities())
                {
                    List<int> wopBenefit = SubContext.tblProductPlanRiders.Where(a => a.RiderId == 10).Select(a => a.ProductPlanRiderId).ToList();
                    foreach (tblPolicyMemberBenefitDetail objRider in SubContext.tblPolicyMemberBenefitDetails.Where(a => a.MemberID == MemberID && wopBenefit.Contains(a.BenifitID)).ToList())
                    {

                        var MemberDetail = SubContext.tblPolicyMemberDetails.Where(a => a.MemberID == MemberID).FirstOrDefault();
                        if (MemberDetail != null)
                        {

                            if (objRider != null)
                            {
                                decimal _premium = decimal.Zero;
                                decimal _LoadingPercentage = decimal.Zero;
                                decimal _LoadingPerMilli = decimal.Zero;
                                decimal _LoadingAmount = decimal.Zero;

                                foreach (var RiderOtherDetails in SubContext.tblMemberBenefitOtherDetails.Where(a => a.MemberBenifitID == objRider.MemberBenifitID).ToList())
                                {
                                    if (RiderOtherDetails.LoadingType == "2203")//Per Milli
                                    {
                                        _LoadingPerMilli = _LoadingPerMilli + RiderOtherDetails.LoadingAmount != null ? Convert.ToDecimal(RiderOtherDetails.LoadingAmount) : decimal.Zero;

                                    }
                                    else if (RiderOtherDetails.LoadingType == "2204")// Percentage
                                    {
                                        _LoadingPercentage = _LoadingPercentage + RiderOtherDetails.LoadingAmount != null ? Convert.ToDecimal(RiderOtherDetails.LoadingAmount) : decimal.Zero;
                                    }

                                    _LoadingAmount = _LoadingAmount + Convert.ToDecimal(RiderOtherDetails.ExtraPremium);
                                }
                                if (!string.IsNullOrEmpty(objRider.Premium))
                                {
                                    _premium = Convert.ToDecimal(objRider.Premium);
                                }
                                _premium = _premium + _LoadingAmount;

                                #region Set Premium Info
                                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                                {
                                    var RiderDetail = Context.tblPolicyMemberBenefitDetails.Where(a => a.MemberBenifitID == objRider.MemberBenifitID).FirstOrDefault();
                                    if (RiderDetail != null)
                                    {
                                        RiderDetail.LoadingPerc = Convert.ToInt32(_LoadingPercentage);
                                        RiderDetail.LoadinPerMille = Convert.ToInt32(_LoadingPerMilli);
                                        RiderDetail.LoadingAmount = Convert.ToString(_LoadingAmount);
                                        RiderDetail.TotalPremium = Convert.ToString(_premium);
                                        Context.SaveChanges();
                                    }
                                }
                                #endregion
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {


            }
            #endregion
        }
        public void UpdateAdditionalPremiumInProposalLevelWhenWop(decimal Policyid)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    decimal LoadingPremium = decimal.Zero;
                    List<string> premiums=(from members in Context.tblPolicyMemberDetails.Where(a => a.PolicyID == Policyid && a.IsDeleted != true)
                                            join benefits in Context.tblPolicyMemberBenefitDetails.Where(a => a.IsDeleted != true)
                                            on members.MemberID equals benefits.MemberID
                                            select benefits.TotalPremium).ToList();
                    foreach (string pre in premiums)
                    {
                        if (!string.IsNullOrEmpty(pre))
                        {
                            LoadingPremium = LoadingPremium + Convert.ToDecimal(pre);
                        }
                    }
                    if (LoadingPremium > 0)
                    {
                        var PremiumInfo = Context.tblProposalPremiums.Where(a => a.PolicyID == Policyid).FirstOrDefault();
                        if (PremiumInfo != null)
                        {
                            PremiumInfo.AdditionalPremium = LoadingPremium - PremiumInfo.AnnualPremium ?? 0 ;
                            Context.SaveChanges();
                        }
                    }


                }
            }
            catch (Exception ex)
            {

                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
            }
        }
        public void UpdateMemberLevelInfo(decimal MemberID)
        {
            #region Update Premium Info
            try
            {
                using (AVOAIALifeEntities SubContext = new AVOAIALifeEntities())
                {
                    foreach (tblPolicyMemberBenefitDetail objRider in SubContext.tblPolicyMemberBenefitDetails.Where(a => a.MemberID == MemberID).ToList())
                    {

                        var MemberDetail = SubContext.tblPolicyMemberDetails.Where(a => a.MemberID == MemberID).FirstOrDefault();
                        if (MemberDetail != null)
                        {

                            if (objRider != null)
                            {
                                decimal _premium = decimal.Zero;
                                decimal _LoadingPercentage = decimal.Zero;
                                decimal _LoadingPerMilli = decimal.Zero;
                                decimal _LoadingAmount = decimal.Zero;

                                foreach (var RiderOtherDetails in SubContext.tblMemberBenefitOtherDetails.Where(a => a.MemberBenifitID == objRider.MemberBenifitID).ToList())
                                {
                                    if (RiderOtherDetails.LoadingType == "2203")//Per Milli
                                    {
                                        _LoadingPerMilli = _LoadingPerMilli + RiderOtherDetails.LoadingAmount != null ? Convert.ToDecimal(RiderOtherDetails.LoadingAmount) : decimal.Zero;

                                    }
                                    else if (RiderOtherDetails.LoadingType == "2204")// Percentage
                                    {
                                        _LoadingPercentage = _LoadingPercentage + RiderOtherDetails.LoadingAmount != null ? Convert.ToDecimal(RiderOtherDetails.LoadingAmount) : decimal.Zero;
                                    }

                                    _LoadingAmount = _LoadingAmount + Convert.ToDecimal(RiderOtherDetails.ExtraPremium);
                                }
                                if (!string.IsNullOrEmpty(objRider.Premium))
                                {
                                    _premium = Convert.ToDecimal(objRider.Premium);
                                }
                                _premium = _premium + _LoadingAmount;
                                
                                #region Set Premium Info
                                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                                {
                                    var RiderDetail = Context.tblPolicyMemberBenefitDetails.Where(a => a.MemberBenifitID == objRider.MemberBenifitID).FirstOrDefault();
                                    if (RiderDetail != null)
                                    {
                                        RiderDetail.LoadingPerc = Convert.ToInt32(_LoadingPercentage);
                                        RiderDetail.LoadinPerMille = Convert.ToInt32(_LoadingPerMilli);
                                        RiderDetail.LoadingAmount = Convert.ToString(_LoadingAmount);
                                        RiderDetail.TotalPremium = Convert.ToString(_premium);
                                        Context.SaveChanges();
                                    }
                                }
                                #endregion
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {


            }
            #endregion
        }
        /// <summary>
        /// Update Proposal Level Premium
        /// </summary>
        /// <param name="Policyid"></param>
        public void UpdateAdditionalPremiumInProposalLevel(decimal Policyid)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    decimal LoadingPremium = decimal.Zero;
                    foreach (var Member in Context.tblPolicyMemberDetails.Where(a => a.PolicyID == Policyid && a.IsDeleted != true).ToList())
                    {
                        if (!string.IsNullOrEmpty(Member.AdditionalPremium))
                        {
                            LoadingPremium = LoadingPremium + Convert.ToDecimal(Member.AdditionalPremium);
                        }
                    }
                    if (LoadingPremium > 0)
                    {
                        var PremiumInfo = Context.tblProposalPremiums.Where(a => a.PolicyID == Policyid).FirstOrDefault();
                        if (PremiumInfo != null)
                        {
                            PremiumInfo.AdditionalPremium = Convert.ToDecimal(LoadingPremium);
                            Context.SaveChanges();
                        }
                    }


                }
            }
            catch (Exception ex)
            {

                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
            }
        }
        /// <summary>
        /// Test UW Deviation
        /// </summary>
        /// <param name="objPolicy"></param>
        /// <returns></returns>
        public AIA.Life.Models.Policy.Policy TestUWDeviation(AIA.Life.Models.Policy.Policy objPolicy)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                tblPolicy objtblpolicy = new tblPolicy();
                objtblpolicy = Context.tblPolicies.Where(a => a.QuoteNo == objPolicy.QuoteNo).FirstOrDefault();

                #region Added For Demo
                UWRuleLogic objLogic = new UWRuleLogic();
                PolicyLogic objPolicyLogic = new PolicyLogic();
                objPolicy = objPolicyLogic.FetchProposalInfo(objPolicy);
                string Message = objLogic.ValidateDeviation(objPolicy);
                if (!string.IsNullOrEmpty(Message.Trim()))
                {
                    if (objtblpolicy != null)
                    {
                        objtblpolicy.PolicyRemarks = Message;
                        objtblpolicy.PolicyStageStatusID = 193;// UW
                        objtblpolicy.IsAllocated = false; // Pending for Allocation
                    }
                    objPolicy.Message = "Payment is Successfull.Proposal is referred to UW  for below reason. " + Message;
                    Context.SaveChanges();
                    return objPolicy;
                }
                #endregion
                objPolicy.Message = "Success";
                if (objtblpolicy != null)
                {
                    objtblpolicy.PolicyNo = objPolicy.ProposalNo = objPolicy.ProposalNo;
                    objtblpolicy.PolicyStageStatusID = 192;// Issued
                }
                Context.SaveChanges();
                return objPolicy;

            }
            catch (Exception ex)
            {
                objPolicy.Message = "Error";
                return objPolicy;
            }
        }

        public AIA.Life.Models.Policy.Policy FetchProposalLifeAssuredDetails(AIA.Life.Models.Policy.Policy ObjPolicy)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                var tbllifeQQ = Context.tblLifeQQs.Where(a => a.QuoteNo == ObjPolicy.QuoteNo).FirstOrDefault();

                ObjPolicy.ContactID = tbllifeQQ.ContactID;
                ObjPolicy.PolicyTerm = Convert.ToString(tbllifeQQ.PolicyTermID);
                ObjPolicy.PaymentTerm = Convert.ToString(tbllifeQQ.PremiumTerm);
                var tblcontact = Context.tblContacts.Where(a => a.ContactID == tbllifeQQ.ContactID).FirstOrDefault();
                ObjPolicy.objProspectDetails = new MemberDetails();
                ObjPolicy.objProspectDetails.ClientCode = tblcontact.ClientCode;
                ObjPolicy.objProspectDetails.FirstName = tblcontact.FirstName;
                ObjPolicy.objProspectDetails.LastName = tblcontact.LastName;
                ObjPolicy.objProspectDetails.Email = tblcontact.EmailID;
                ObjPolicy.objProspectDetails.MobileNo = tblcontact.MobileNo;
                ObjPolicy.objProspectDetails.HomeNumber = tblcontact.PhoneNo;
                ObjPolicy.objProspectDetails.WorkNumber = tblcontact.Work;
                ObjPolicy.objProspectDetails.Age = Convert.ToInt32(tblcontact.Age);
                ObjPolicy.objProspectDetails.DateOfBirth = Convert.ToDateTime(tblcontact.DateOfBirth);
                ObjPolicy.objProspectDetails.OccupationID = Convert.ToInt32(tblcontact.OccupationID);
                ObjPolicy.objProspectDetails.NewNICNO = tblcontact.NICNO;
                ObjPolicy.objProspectDetails.MaritialStatus = Context.tblMasCommonTypes.Where(a => a.CommonTypesID == tblcontact.MaritalStatusID && a.MasterType == "MaritalStatus").Select(a => a.Code).FirstOrDefault();
                //ObjPolicy.objProspectDetails.MaritialStatus = Convert.ToString(tblcontact.MaritalStatusID);//Context.tblMasCommonTypes.Where(a => a.CommonTypesID == tblcontact.MaritalStatusID && a.MasterType == "MaritalStatus").Select(a => a.CommonTypesID).FirstOrDefault();
                //ObjPolicy.objProspectDetails.MaritialStatus=GetCustomerMaritialStatus(tblcontact.MaritalStatusID).ToString();
                ObjPolicy.objProspectDetails.Gender = tblcontact.Gender;
                ObjPolicy.objProspectDetails.MonthlyIncome = tblcontact.MonthlyIncome;
                ObjPolicy.objProspectDetails.Salutation = tblcontact.Title;
                //ObjPolicy.objProspectDetails.CompanyName = tblcontact.Employer;
                if (ObjPolicy.PolicyID > 0)
                {
                    var tblpolicyrelationships = Context.tblPolicyRelationships.Where(a => a.PolicyID == ObjPolicy.PolicyID).FirstOrDefault();
                    var tblpolicyClients = Context.tblPolicyClients.Where(a => a.PolicyClientID == tblpolicyrelationships.PolicyClientID).FirstOrDefault();
                    //ObjPolicy.objProspectDetails.Nationality = Convert.ToString(tblpolicyClients.Nationality);
                    ObjPolicy.objProspectDetails.Nationality = GetFetchNationality(Convert.ToInt16(tblpolicyClients.Nationality));
                    ObjPolicy.objProspectDetails.USTaxpayerId = tblpolicyClients.USTaxpayerId;
                    ObjPolicy.objProspectDetails.SpecifyNationality = tblpolicyClients.USTaxpayerId;
                    ObjPolicy.objProspectDetails.OccupationHazardousWork = tblpolicyClients.OccupationHazardousWork;
                    ObjPolicy.objProspectDetails.SpecifiyOccupationHazardousWork = tblpolicyClients.SpecifyHazardousWork;
                    ObjPolicy.objProspectDetails.PassportNumber = tblpolicyClients.PassportNumber;
                    ObjPolicy.objProspectDetails.DrivingLicense = tblpolicyClients.DrivingLicense;
                    ObjPolicy.objProspectDetails.CompanyName = tblpolicyClients.CompanyName;
                    ObjPolicy.objProspectDetails.ResidentialStatus = tblpolicyClients.ResidentialNationalityStatus;
                    ObjPolicy.objProspectDetails.Residential = tblpolicyClients.ResidentialNationality;
                    ObjPolicy.objProspectDetails.CitizenShip = Convert.ToBoolean(tblpolicyClients.CitizenShip);
                    ObjPolicy.objProspectDetails.Citizenship1 = tblpolicyClients.Citizenship1;
                    ObjPolicy.objProspectDetails.Citizenship2 = tblpolicyClients.Citizenship2;
                    ObjPolicy.objProspectDetails.MobileNo = tblpolicyClients.MobileNo;
                    ObjPolicy.objProspectDetails.OtherMobileNo = tblpolicyClients.AlteranteMobileNO;
                    ObjPolicy.objProspectDetails.HomeNumber = tblpolicyClients.HomeNo;
                    ObjPolicy.objProspectDetails.WorkNumber = tblpolicyClients.WorkNo;
                    ObjPolicy.objProspectDetails.Email = tblpolicyClients.EmailID;
                }
                ObjPolicy.objProspectDetails.objCommunicationAddress.Address1 = tblcontact.tblAddress.Address1;
                ObjPolicy.objProspectDetails.objCommunicationAddress.Address2 = tblcontact.tblAddress.Address2;
                ObjPolicy.objProspectDetails.objCommunicationAddress.Address3 = tblcontact.tblAddress.Address3;
                ObjPolicy.objProspectDetails.objCommunicationAddress.Pincode = tblcontact.tblAddress.Pincode + "|" + tblcontact.tblAddress.City;
                ObjPolicy.objProspectDetails.objCommunicationAddress.PincodeNew = tblcontact.tblAddress.Pincode;
                ObjPolicy.objProspectDetails.objCommunicationAddress.City = tblcontact.tblAddress.City;
                ObjPolicy.objProspectDetails.objCommunicationAddress.Province = tblcontact.tblAddress.State;
                ObjPolicy.objProspectDetails.objCommunicationAddress.State = tblcontact.tblAddress.State;
                ObjPolicy.objProspectDetails.objCommunicationAddress.District = tblcontact.tblAddress.District;
                ObjPolicy.objProspectDetails.objCommunicationAddress.Country = tblcontact.tblAddress.Country;

                return ObjPolicy;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// Calculate Loading Premium
        /// </summary>
        /// <param name="objPolicy"></param>
        /// <returns></returns>
        public AIA.Life.Models.Policy.Policy CalculateLoadingPremium(AIA.Life.Models.Policy.Policy objPolicy)
        {
            try
            {
                int memberRelation = 0;
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    if (objPolicy.objMemberDetails != null)
                    {
                        #region Load Required Masters
                        objPolicy.objMemberDetails[0].LstLoadingType = (from CommonType in Context.tblMasCommonTypes.Where(a => a.MasterType == "LoadingType")
                                                                        select new MasterListItem
                                                                        {
                                                                            ID = CommonType.CommonTypesID,
                                                                            Text = CommonType.Description
                                                                        }).ToList();
                        objPolicy.objMemberDetails[0].LstLoadingBasis = (from CommonType in Context.tblMasCommonTypes.Where(a => a.MasterType == "LoadingBasis")
                                                                         select new MasterListItem
                                                                         {
                                                                             ID = CommonType.CommonTypesID,
                                                                             Text = CommonType.Description
                                                                         }).ToList();
                        #endregion

                        #region Map Rider Information to Xml Object to pass to Store Procedure
                        decimal MemberID = objPolicy.objMemberDetails[0].MemberID;
                        var MemberDetail = Context.tblPolicyMemberDetails.Where(a => a.MemberID == MemberID).FirstOrDefault();
                        List<int> wopRiders = Context.tblProductPlanRiders.Where(a => a.RiderId == 10).Select(a => a.ProductPlanRiderId).ToList();
                        memberRelation = MemberDetail.RelationShipWithProposer;
                        if (MemberDetail != null)
                        {
                            var PolicyInfo = Context.tblPolicies.Where(a => a.PolicyID == MemberDetail.PolicyID).FirstOrDefault();
                            // For temp Purpose to Fetch product and plan as these are not saved at proposal level
                            var LifeQQ = Context.tblLifeQQs.Where(a => a.QuoteNo == PolicyInfo.QuoteNo).FirstOrDefault();
                            AIA.Life.Models.UWDecision.ProposalDetails objProposalDetails = new AIA.Life.Models.UWDecision.ProposalDetails();
                            objProposalDetails.Product = new Product();
                            objProposalDetails.Rider = new List<Rider>();
                            objProposalDetails.Product.ProductId = Convert.ToString(LifeQQ.ProductNameID);
                            objProposalDetails.Product.PlanId = Convert.ToString(LifeQQ.PlanId);
                            objProposalDetails.Product.ProposalPremium = Convert.ToString(PolicyInfo.tblProposalPremiums.FirstOrDefault().AnnualPremium);
                            objProposalDetails.Product.PaymentFrequency = Convert.ToString(PolicyInfo.PaymentFrequency);
                            objPolicy.objMemberDetails[0].objBenifitDetails = objPolicy.objMemberDetails[0].objBenifitDetails.Where(a => a.IsDeleted != true).ToList();
                            int RiderLoadingIndex = 0;
                            for (int i = 0; i < objPolicy.objMemberDetails[0].objBenifitDetails.Count(); i++)
                            {
                                if (objPolicy.objMemberDetails[0].objBenifitDetails[i].IsDeleted != true && !wopRiders.Contains(objPolicy.objMemberDetails[0].objBenifitDetails[i].BenefitID))
                                {
                                    Rider objRider = new Rider();
                                    objPolicy.objMemberDetails[0].objBenifitDetails[i].RiderLoadingIndex = RiderLoadingIndex;
                                    objRider.RiderId = Convert.ToString(objPolicy.objMemberDetails[0].objBenifitDetails[i].BenefitID);
                                    objRider.SumAssured = objPolicy.objMemberDetails[0].objBenifitDetails[i].RiderSuminsured;
                                    objRider.RiderPremium = objPolicy.objMemberDetails[0].objBenifitDetails[i].RiderPremium;
                                    objRider.LoadingAmount = objPolicy.objMemberDetails[0].objBenifitDetails[i].LoadingAmount;
                                    objRider.LoadingType = objPolicy.objMemberDetails[0].objBenifitDetails[i].LoadingType;
                                    objRider.LoadingBasis = objPolicy.objMemberDetails[0].objBenifitDetails[i].LoadingBasis;
                                    objRider.RowId = RiderLoadingIndex.ToString();
                                    objProposalDetails.Rider.Add(objRider);
                                    RiderLoadingIndex++;
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
                                var serializer = new XmlSerializer(objProposalDetails.GetType());
                                serializer.Serialize(xw, objProposalDetails);
                                xw.Flush();
                                ms.Seek(0, SeekOrigin.Begin);
                                var sr = new StreamReader(ms, System.Text.Encoding.UTF8);
                                strOutPut = sr.ReadToEnd();
                            }
                            #endregion


                            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                            con.Open();
                            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                            cmd.Connection = con;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "usp_ApplyLoading";
                            cmd.Parameters.Add("@s", SqlDbType.VarChar);
                            cmd.Parameters[0].Value = strOutPut;
                            DataSet ds = new DataSet();
                            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                            da.Fill(ds);
                            List<LoadingPremiumOutput> Result = new List<LoadingPremiumOutput>();
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                LoadingPremiumOutput objLoadingPremium = new LoadingPremiumOutput();
                                objLoadingPremium.RowId = Convert.ToInt32(ds.Tables[0].Rows[i]["RowId"].ToString());
                                objLoadingPremium.RiderID = ds.Tables[0].Rows[i]["RiderId"].ToString();
                                objLoadingPremium.SumAssured = ds.Tables[0].Rows[i]["SumAssured"].ToString();
                                objLoadingPremium.RiderPremium = ds.Tables[0].Rows[i]["RiderPremium"].ToString();
                                objLoadingPremium.LoadingAmount = ds.Tables[0].Rows[i]["LoadingAmount"].ToString();
                                objLoadingPremium.LoadingBasis = ds.Tables[0].Rows[i]["LoadingBasis"].ToString();
                                objLoadingPremium.LoadingType = ds.Tables[0].Rows[i]["LoadingType"].ToString();
                                if (ds.Tables[0].Rows[i]["ExtraPremium"] is DBNull)
                                {
                                    objLoadingPremium.ExtraPremium = decimal.Zero;
                                }
                                else { objLoadingPremium.ExtraPremium = Convert.ToDecimal(ds.Tables[0].Rows[i]["ExtraPremium"]); }
                                if (ds.Tables[0].Rows[i]["TotalPremium"] is DBNull)
                                {
                                    objLoadingPremium.TotalPremium = decimal.Zero;
                                }
                                else { objLoadingPremium.TotalPremium = Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalPremium"]); }
                                Result.Add(objLoadingPremium);
                            }

                            #region Map Loading Premium to Riders

                            for (int i = 0; i < Result.Count(); i++)
                            {
                                int _index = objPolicy.objMemberDetails[0].objBenifitDetails.FindIndex(a => a.RiderLoadingIndex == Result[i].RowId);
                                if (_index >= 0)
                                {
                                    objPolicy.objMemberDetails[0].objBenifitDetails[_index].ExtraPremium = Convert.ToString(Result[i].ExtraPremium);
                                    objPolicy.objMemberDetails[0].objBenifitDetails[_index].RiderPremium = Result[i].RiderPremium;
                                    objPolicy.objMemberDetails[0].objBenifitDetails[_index].TotalPremium = Convert.ToString(Result[i].TotalPremium);
                                }

                            }


                            #endregion


                            #region Member level Extra Premium
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                objPolicy.objMemberDetails[0].ExtraPremium = ds.Tables[1].Rows[0].ItemArray[2].ToString();
                            }
                            #endregion
                        }
                    }
                }
                if(memberRelation==267)
                    CalculateWopLoadingPremium(objPolicy);
                objPolicy.Message = "Success";
                return objPolicy;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                objPolicy.Message = "Error";
                return objPolicy;
            }
        }

        public AIA.Life.Models.Policy.Policy CalculateWopLoadingPremium(AIA.Life.Models.Policy.Policy objPolicy)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    List<MemberDetails> lstMembers = new List<MemberDetails>();

                    if (objPolicy.objMemberDetails != null)
                    {
                        #region Load Required Masters
                        objPolicy.objMemberDetails[0].LstLoadingType = (from CommonType in Context.tblMasCommonTypes.Where(a => a.MasterType == "LoadingType")
                                                                        select new MasterListItem
                                                                        {
                                                                            ID = CommonType.CommonTypesID,
                                                                            Text = CommonType.Description
                                                                        }).ToList();
                        objPolicy.objMemberDetails[0].LstLoadingBasis = (from CommonType in Context.tblMasCommonTypes.Where(a => a.MasterType == "LoadingBasis")
                                                                         select new MasterListItem
                                                                         {
                                                                             ID = CommonType.CommonTypesID,
                                                                             Text = CommonType.Description
                                                                         }).ToList();
                        #endregion

                        #region Map Rider Information to Xml Object to pass to Store Procedure
                        decimal MemberID = objPolicy.objMemberDetails[0].MemberID;
                        var MemberDetail = Context.tblPolicyMemberDetails.Where(a => a.MemberID == MemberID).FirstOrDefault();
                        lstMembers.Add(FetchMemberDetails(MemberDetail, new MemberDetails()));
                        lstMembers[0].objBenifitDetails = objPolicy.objMemberDetails[0].objBenifitDetails;
                        if (MemberDetail != null)
                        {
                            var PolicyInfo = Context.tblPolicies.Where(a => a.PolicyID == MemberDetail.PolicyID).FirstOrDefault();
                            var tblMembers = PolicyInfo.tblPolicyMemberDetails.Where(a => a.IsDeleted != true).ToList();
                            // For temp Purpose to Fetch product and plan as these are not saved at proposal level
                            var LifeQQ = Context.tblLifeQQs.Where(a => a.QuoteNo == PolicyInfo.QuoteNo).FirstOrDefault();
                            AIA.Life.Models.UWDecision.ProposalDetails objProposalDetails = new AIA.Life.Models.UWDecision.ProposalDetails();
                            objProposalDetails.Product = new Product();
                            objProposalDetails.Product.ProductId = Convert.ToString(LifeQQ.ProductNameID);
                            objProposalDetails.Product.PlanId = Convert.ToString(LifeQQ.PlanId);
                            objProposalDetails.Product.ProposalPremium = Convert.ToString(PolicyInfo.tblProposalPremiums.FirstOrDefault().AnnualPremium);
                            objProposalDetails.Product.ProposalNo = PolicyInfo.ProposalNo;
                            objProposalDetails.Product.PolicyTerm = Convert.ToString(LifeQQ.PolicyTermID);
                            objProposalDetails.Product.PaymentFrequency = Convert.ToInt32(LifeQQ.PreferredTerm).ToString();
                            objProposalDetails.Product.ApplyOccupationLoading = "1";
                            objProposalDetails.Member = new List<Member>();

                            foreach (var item in tblMembers)
                            {
                                if (item.RelationShipWithProposer != Convert.ToInt32(lstMembers[0].RelationShipWithPropspect))
                                {
                                    MemberDetails memberDetails = new MemberDetails();
                                    memberDetails = FetchMemberDetails(item, memberDetails);
                                    //var proposalRiders = (from po in Context.tblPolicyMemberBenefitDetails.Where(a => a.MemberID == item.MemberID && a.IsDeleted != true)
                                    //                      join pa in Context.tblProductPlanRiders
                                    //                      on po.BenifitID equals pa.ProductPlanRiderId
                                    //                      orderby pa.DisplayOrder ascending
                                    //                      select po).ToList();
                                    //foreach (var Rider in proposalRiders)
                                    //{
                                    //    BenifitDetails objBenefitDetail = new BenifitDetails();
                                    //    objBenefitDetail.RiderPremium = Rider.TotalPremium;
                                    //    var RiderInfo = Context.tblProductPlanRiders.Where(a => a.ProductPlanRiderId == Rider.BenifitID).FirstOrDefault();
                                    //    if (RiderInfo != null)
                                    //    {
                                    //        objBenefitDetail.RiderCode = RiderInfo.RefRiderCode;
                                    //        objBenefitDetail.BenifitName = RiderInfo.DisplayName;
                                    //    }

                                    //    objBenefitDetail.BenefitID = Rider.BenifitID;
                                    //    objBenefitDetail.RiderSuminsured = Rider.SumInsured;
                                    //    objBenefitDetail.LoadingAmount = Convert.ToString(Rider.LoadingAmount);
                                    //    objBenefitDetail.LoadingPercentage = Convert.ToString(Rider.LoadingPerc);
                                    //    objBenefitDetail.LoadinPerMille = Convert.ToString(Rider.LoadinPerMille);
                                    //    objBenefitDetail.ActualRiderPremium = Rider.Premium;
                                    //    foreach (var loading in Context.tblMemberBenefitOtherDetails.Where(b => b.MemberBenifitID == Rider.MemberBenifitID).ToList())
                                    //    {
                                    //        BenefitLoading benefitLoading = new BenefitLoading();
                                    //        int loadingBasis = Convert.ToInt32(loading.LoadingBasis);
                                    //        benefitLoading.LoadingBasis = Context.tblMasCommonTypes.Where(a => a.CommonTypesID == loadingBasis).Select(a => a.Code).FirstOrDefault();
                                    //        benefitLoading.LoadingType = loading.LoadingType;
                                    //        benefitLoading.LoadingPercentage = loading.LoadingAmount;
                                    //        objBenefitDetail.BenefitLoadings.Add(benefitLoading);
                                    //    }

                                    //    memberDetails.objBenifitDetails.Add(objBenefitDetail);
                                    //}
                                    memberDetails.objBenifitDetails.AddRange((from Member in Context.tblPolicyMemberDetails.Where(a => a.MemberID == item.MemberID && a.IsDeleted != true)
                                                                              join MemberBenefit in Context.tblPolicyMemberBenefitDetails.Where(b => b.IsDeleted != true)
                                                                              on Member.MemberID equals MemberBenefit.MemberID
                                                                              join MasBenefit in Context.tblProductPlanRiders
                                                                              on MemberBenefit.BenifitID equals MasBenefit.ProductPlanRiderId
                                                                              join OtherDetails in Context.tblMemberBenefitOtherDetails
                                                                              on MemberBenefit.MemberBenifitID equals OtherDetails.MemberBenifitID into LoadingInfo
                                                                              from Loading in LoadingInfo.DefaultIfEmpty()
                                                                              orderby MasBenefit.DisplayOrder ascending
                                                                              select new BenifitDetails
                                                                              {
                                                                                  RiderID = MasBenefit.RiderId ?? 0,
                                                                                  BenifitName = MasBenefit.DisplayName,
                                                                                  RiderCode = MasBenefit.RefRiderCode,
                                                                                  BenefitID = (int)MasBenefit.ProductPlanRiderId,
                                                                                  MemberBenifitID = MemberBenefit.MemberBenifitID != null ? MemberBenefit.MemberBenifitID : 0,
                                                                                  RiderSuminsured = MemberBenefit.SumInsured,
                                                                                  ActualRiderPremium = MemberBenefit.Premium,
                                                                                  RiderPremium = MemberBenefit.TotalPremium,
                                                                                  LoadingType = Loading.LoadingType,
                                                                                  LoadingBasis = Loading.LoadingBasis,
                                                                                  ExtraPremium = Loading.ExtraPremium,
                                                                                  LoadingAmount = Loading.LoadingAmount,
                                                                                  LoadingPercentage = (Loading.LoadingType == "2204" ? Loading.LoadingAmount : "0").ToString(),
                                                                                  LoadinPerMille = (Loading.LoadingType == "2203" ? Loading.LoadingAmount : "0").ToString(),
                                                                                  MemberBenefitDetailID = Loading.MemberBenifitDetailsID != null ? Loading.MemberBenifitDetailsID : 0
                                                                              }).ToList());
                                    lstMembers.Add(memberDetails);
                                }
                            }
                            
                            for (int i = 0; i < lstMembers.Count; i++)
                            {
                                Member objMember = new Member();
                                objMember.Age = lstMembers[i].Age.ToString();
                                objMember.Id = (i + 1).ToString();
                                switch (lstMembers[i].RelationShipWithPropspect)
                                {
                                    case "267":
                                    objMember.Relation = "1";
                                    break;
                                    case "268":
                                    objMember.Relation = "2";
                                    break;
                                    default:
                                    objMember.Relation = "3";
                                    break;
                                }
                                objMember.Rider = new List<Rider>();
                                int riderLoadingIndex = 0;
                                for (int j = 0; j < lstMembers[i].objBenifitDetails.Count(); j++)
                                {
                                    objPolicy.objMemberDetails[0].objBenifitDetails[j].RiderLoadingIndex = riderLoadingIndex;
                                    int benefitID = lstMembers[i].objBenifitDetails[j].BenefitID;
                                    lstMembers[i].objBenifitDetails[j].RiderID = Context.tblProductPlanRiders.Where(a => a.ProductPlanRiderId == benefitID).Select(a => a.RiderId).FirstOrDefault() ?? 0;
                                    if (lstMembers[i].objBenifitDetails[j].IsDeleted != true)
                                    {
                                        Rider objRider = new Rider();
                                        objRider.RowId = riderLoadingIndex.ToString();
                                        objRider.BenefitId = Convert.ToString(lstMembers[i].objBenifitDetails[j].BenefitID);
                                        objRider.ExtraPremium =string.IsNullOrEmpty(lstMembers[i].objBenifitDetails[j].ExtraPremium) == true ? "0" : lstMembers[i].objBenifitDetails[j].ExtraPremium;
                                        if (MemberDetail.RelationShipWithProposer == Convert.ToInt32(lstMembers[i].RelationShipWithPropspect))
                                        {
                                            objRider.BasicPremium = lstMembers[i].objBenifitDetails[j].RiderPremium;
                                            
                                            if (lstMembers[i].objBenifitDetails[j].RiderID == 10)
                                            {
                                                if (lstMembers[i].objBenifitDetails[j].LoadingType == "2204")
                                                    objRider.LoadingPer = lstMembers[i].objBenifitDetails[j].LoadingAmount;
                                                else if (lstMembers[i].objBenifitDetails[j].LoadingType == "2203")
                                                    objRider.LoadingPerMille = lstMembers[i].objBenifitDetails[j].LoadingAmount;
                                            }
                                        }
                                        else
                                        {
                                            objRider.BasicPremium = lstMembers[i].objBenifitDetails[j].ActualRiderPremium;
                                        }
                                        objMember.Rider.Add(objRider);
                                    }
                                    riderLoadingIndex++;
                                }
                                objProposalDetails.Member.Add(objMember);
                            }


                            #endregion

                            #region Object To Xml Convertion
                            StringWriter sw = new StringWriter();
                            XmlTextWriter tw = null;
                            string strOutPut = string.Empty;
                            using (var ms = new MemoryStream())
                            {
                                var xw = XmlWriter.Create(ms);// Remember to stop using XmlTextWriter  
                                var serializer = new XmlSerializer(objProposalDetails.GetType());
                                serializer.Serialize(xw, objProposalDetails);
                                xw.Flush();
                                ms.Seek(0, SeekOrigin.Begin);
                                var sr = new StreamReader(ms, System.Text.Encoding.UTF8);
                                strOutPut = sr.ReadToEnd();
                            }
                            #endregion

                            #region  Log Input 
                            tbllogxml objlogxml = new tbllogxml();
                            objlogxml.Description = "ApplyLoadingOnWOP Cal";
                            objlogxml.PolicyID = Convert.ToString(MemberDetail.PolicyID);
                            objlogxml.UserID = string.Empty;
                            objlogxml.XMlData = strOutPut;
                            objlogxml.CreatedDate = DateTime.Now;
                            Context.tbllogxmls.Add(objlogxml);
                            #endregion
                            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                            con.Open();
                            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                            cmd.Connection = con;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "usp_ApplyLoadingOnWOP";
                            cmd.Parameters.Add("@s", SqlDbType.VarChar);
                            cmd.Parameters[0].Value = strOutPut;
                            DataSet ds = new DataSet();
                            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                            da.Fill(ds);
                            List<LoadingPremiumOutput> Result = new List<LoadingPremiumOutput>();
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                LoadingPremiumOutput objLoadingPremium = new LoadingPremiumOutput();
                                objLoadingPremium.RowId = Convert.ToInt32(ds.Tables[0].Rows[i]["RowId"].ToString());
                                objLoadingPremium.RiderID = ds.Tables[0].Rows[i]["BenefitId"].ToString();
                                objLoadingPremium.SumAssured = ds.Tables[0].Rows[i]["SumAssured"].ToString();
                                objLoadingPremium.RiderPremium = ds.Tables[0].Rows[i]["RiderPremium"].ToString();
                                if (ds.Tables[0].Rows[i]["ExtraPremium"] is DBNull)
                                {
                                    objLoadingPremium.ExtraPremium = decimal.Zero;
                                }
                                else { objLoadingPremium.ExtraPremium = Convert.ToDecimal(ds.Tables[0].Rows[i]["ExtraPremium"]); }
                                if (ds.Tables[0].Rows[i]["TotalPremium"] is DBNull)
                                {
                                    objLoadingPremium.TotalPremium = decimal.Zero;
                                }
                                else { objLoadingPremium.TotalPremium = Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalPremium"]); }
                                Result.Add(objLoadingPremium);
                            }

                            #region Map Loading Premium to Riders

                            for (int i = 0; i < Result.Count(); i++)
                            {
                                int _index = objPolicy.objMemberDetails[0].objBenifitDetails.FindIndex(a => a.RiderLoadingIndex == Result[i].RowId);
                                if (_index >= 0)
                                {
                                    objPolicy.objMemberDetails[0].objBenifitDetails[_index].RiderSuminsured = Result[i].SumAssured;
                                    objPolicy.objMemberDetails[0].objBenifitDetails[_index].ExtraPremium = Convert.ToString(Result[i].ExtraPremium);
                                    objPolicy.objMemberDetails[0].objBenifitDetails[_index].RiderPremium = Result[i].RiderPremium;
                                    objPolicy.objMemberDetails[0].objBenifitDetails[_index].TotalPremium = Convert.ToString(Result[i].TotalPremium);
                                }
                            }


                            #endregion

                        }
                    }
                }
                objPolicy.Message = "Success";
                return objPolicy;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                objPolicy.Message = "Error";
                return objPolicy;
            }
        }

        public AIA.Life.Models.Policy.Policy LoadPolicyPreviousInsuranceGrid(AIA.Life.Models.Policy.Policy ObjPolicy)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            var LstDetails = Context.usp_GetPreviousPolicyDetails(ObjPolicy.objProspectDetails.NewNICNO).ToList();
            List<AIALifeAssuredOtherInsurance> lstPreviousInsuranceList = new List<AIALifeAssuredOtherInsurance>();
            List<AIALifeAssuredOtherInsurance> lstPreviousInsuranceList1 = new List<AIALifeAssuredOtherInsurance>();
            List<AIALifeAssuredOtherInsurance> lstPreviousInsuranceList2 = new List<AIALifeAssuredOtherInsurance>();
            List<AIALifeAssuredOtherInsurance> lstPreviousInsuranceList3 = new List<AIALifeAssuredOtherInsurance>();
            lstPreviousInsuranceList = LstDetails.Select(a => new AIALifeAssuredOtherInsurance()
            {
                //AnnualPremium = Convert.ToInt32(a.POlPREM),
                PolicyNo = a.POLICYNO,
                CompanyName = "AIA",
                TotalSAAtDeath = Convert.ToString(a.SumInsured),
                AccidentalBenefitAmount = Convert.ToString(a.ADB),
                IllNessBenifit = Convert.ToString(a.CI),
                PermanentDisability = Convert.ToString(a.WOB),
                HospitalizationPerDay = Convert.ToString(a.HDB),
                CurrentStatus = a.Longdesc

            }).ToList();
            var LstDetails1 = Context.usp_GetOngoingProposalDetails(ObjPolicy.objProspectDetails.NewNICNO).ToList();
            lstPreviousInsuranceList1 = LstDetails1.Select(a => new AIALifeAssuredOtherInsurance()
            {
                PolicyNo = a.POLICYNO,
                CompanyName = "AIA",
                TotalSAAtDeath = Convert.ToString(a.SumInsured),
                AccidentalBenefitAmount = Convert.ToString(a.ADB),
                IllNessBenifit = Convert.ToString(a.CI),
                PermanentDisability = Convert.ToString(a.WOB),
                HospitalizationPerDay = Convert.ToString(a.HDB),
                CurrentStatus = a.LongDesc
            }).ToList();
            lstPreviousInsuranceList2 = lstPreviousInsuranceList.Concat(lstPreviousInsuranceList1).ToList();
            lstPreviousInsuranceList3 = lstPreviousInsuranceList2.Select(a => new AIALifeAssuredOtherInsurance()
            {
                //AnnualPremium = Convert.ToInt32(a.POlPREM),
                PolicyNo = "",
                CompanyName = "Total",
                TotalSAAtDeath = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64(lstPreviousInsuranceList2.Sum(item => Convert.ToInt64(item.TotalSAAtDeath.Split('.')[0])))),
                AccidentalBenefitAmount = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64(lstPreviousInsuranceList2.Sum(item => Convert.ToInt64(item.AccidentalBenefitAmount.Split('.')[0])))),
                IllNessBenifit = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64(lstPreviousInsuranceList2.Sum(item => Convert.ToInt64(item.IllNessBenifit.Split('.')[0])))),
                PermanentDisability = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64(lstPreviousInsuranceList2.Sum(item => Convert.ToInt64(item.PermanentDisability.Split('.')[0])))),
                HospitalizationPerDay = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64(lstPreviousInsuranceList2.Sum(item => Convert.ToInt64(item.HospitalizationPerDay.Split('.')[0])))),
                CurrentStatus = ""


            }).Take(1).ToList();


            for (int i = 0; i < lstPreviousInsuranceList2.Count(); i++)
            {
                lstPreviousInsuranceList2[i].TotalSAAtDeath = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64(lstPreviousInsuranceList2[i].TotalSAAtDeath));
                lstPreviousInsuranceList2[i].AccidentalBenefitAmount = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64(lstPreviousInsuranceList2[i].AccidentalBenefitAmount));
                lstPreviousInsuranceList2[i].IllNessBenifit = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64(lstPreviousInsuranceList2[i].IllNessBenifit));
                lstPreviousInsuranceList2[i].HospitalizationPerDay = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64(lstPreviousInsuranceList2[i].HospitalizationPerDay));
                lstPreviousInsuranceList2[i].PermanentDisability = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64(lstPreviousInsuranceList2[i].PermanentDisability));


            }

            ObjPolicy.ObjAIALifeAssuredOtherInsurance.AddRange(lstPreviousInsuranceList2);
            ObjPolicy.ObjAIALifeAssuredOtherInsurance.AddRange(lstPreviousInsuranceList3);
            return ObjPolicy;
        }
        public ProposalStatus FetchProposalStatus(ProposalStatus proposalStatus)
        {
            using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
            {
                var list = proposalStatus.Proposals.Select(a => a.ProposalNo).ToList();
                proposalStatus.Proposals = Context.tblPolicies.Where(a => list.Contains(a.ProposalNo)).Select(a => new ProposalList
                {
                    ProposalNo = a.ProposalNo,
                    Status = Context.tblMasCommonTypes.Where(c => c.CommonTypesID == a.PolicyStageStatusID).Select(c => c.Description).FirstOrDefault()
                }).ToList();

            }

            return proposalStatus;
        }
        public void InsertIntoAPPAuthenticationDetails(string TraceID, string UserID, string Username)
        {
            using (AVOAIALifeEntities entity = new AVOAIALifeEntities())
            {
                var UserResponseID = entity.AspNetUsers.Where(a => a.UserName == UserID).Select(a => a.Id).FirstOrDefault();
                tblServiceAuthenticationDetail objtblServiceAuthenticationDetail = new tblServiceAuthenticationDetail();
                objtblServiceAuthenticationDetail.TraceID = TraceID;
                objtblServiceAuthenticationDetail.SessionID = UserResponseID;
                objtblServiceAuthenticationDetail.UserName = UserID;
                objtblServiceAuthenticationDetail.LoginTime = DateTime.UtcNow;
                entity.tblServiceAuthenticationDetails.Add(objtblServiceAuthenticationDetail);
                entity.SaveChanges();
            }
        }
        public string ValidateAPPServiceAuthenticationDetails(string TraceID, string UserID, string Username)
        {
            string result = string.Empty;
            using (AVOAIALifeEntities entity = new AVOAIALifeEntities())
            {
                var UserResponseID = entity.AspNetUsers.Where(a => a.UserName == Username).Select(a => a.Id).FirstOrDefault();
                var AuthenticationDetails = entity.tblServiceAuthenticationDetails.Where(x => x.TraceID == TraceID && (x.UserName == Username || x.SessionID == UserResponseID)).FirstOrDefault();
                if (AuthenticationDetails != null)
                {
                    DateTime AuthDate = Convert.ToDateTime(AuthenticationDetails.LoginTime);
                    if (AuthDate.AddMinutes(15) < DateTime.UtcNow)
                    {
                        return result = "Session Time out.";
                    }
                    else if (!string.IsNullOrEmpty(UserID) && AuthenticationDetails.SessionID != UserResponseID)
                    {
                        return result = "Mismatch in Trace ID and User ID.Kindly check with Correct User ID & Trace ID.";
                    }
                    else
                    {
                        AuthenticationDetails.LoginTime = DateTime.UtcNow;
                        entity.SaveChanges();
                        result = "";
                    }
                }
                else
                {
                    return result = "Invalid Session.";
                }
            }
            return result;
        }
        public string LogOutAPPAuthenticationDetails(string TraceID, string UserID, string Username)
        {

            string result = string.Empty;
            using (AVOAIALifeEntities entity = new AVOAIALifeEntities())
            {
                var AuthenticationDetails = entity.tblServiceAuthenticationDetails.Where(x => x.TraceID == TraceID && x.UserName == Username && x.SessionID == UserID).FirstOrDefault();
                if (AuthenticationDetails != null)
                {
                    AuthenticationDetails.LoginTime = DateTime.UtcNow.AddMinutes(-10);
                    entity.SaveChanges();
                    result = "Sucess";
                }
            }
            return result;

        }
        public string ToUpperFirstLetter(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            // convert to char array of the string
            char[] letters = source.ToCharArray();
            // upper case the first char
            letters[0] = char.ToUpper(letters[0]);
            // return the array made of the new char array
            return new string(letters);
        }
        public void SavePolicyIllustration(AIA.Life.Models.Policy.Policy objPolicy)
        {
            using (AVOAIALifeEntities entity = new AVOAIALifeEntities())
            {
                foreach (var item in entity.tblPolicyIllustrations.Where(a => a.ProposalNo == objPolicy.ProposalNo).ToList())
                {
                    entity.Entry(item).State = EntityState.Deleted;
                }
                foreach (var item in objPolicy.LstIllustation)
                {
                    tblPolicyIllustration PolicyIllustration = new tblPolicyIllustration();
                    PolicyIllustration.PolicyYear = item.PolicyYear;
                    PolicyIllustration.BasicPremium = item.BasicPremium;
                    PolicyIllustration.MainBenefitsPremium = item.MainBenefitsPremium;
                    PolicyIllustration.AdditionalBenefitsPremiums = item.AdditionalBenefitsPremiums;
                    PolicyIllustration.TotalPremium = item.TotalPremium;
                    PolicyIllustration.InvestmentACBalance = item.FundBalanceDiv4;
                    PolicyIllustration.SurrenderValue4 = item.SurrenderValueDiv4;
                    PolicyIllustration.MonthlyDrawDown4 = item.DrawDownDiv4;
                    PolicyIllustration.InvestmentACBalance8 = item.FundBalanceDiv8;
                    PolicyIllustration.SurrenderValue8 = item.SurrenderValueDiv8;
                    PolicyIllustration.MonthlyDrawDown8 = item.DrawDownDiv8;
                    PolicyIllustration.InvestmentACBalance12 = item.FundBalanceDiv12;
                    PolicyIllustration.SurrenderValue12 = item.SurrenderValueDiv12;
                    PolicyIllustration.MonthlyDrawDown12 = item.DrawDownDiv12;
                    PolicyIllustration.PensionBoosterDiv4 = item.PensionBoosterDiv4;
                    PolicyIllustration.PensionBoosterDiv8 = item.PensionBoosterDiv8;
                    PolicyIllustration.PensionBoosterDiv12 = item.PensionBoosterDiv12;
                    PolicyIllustration.InvestmentACBalance5 = item.FundBalanceDiv5;
                    PolicyIllustration.InvestmentACBalance6 = item.FundBalanceDiv6;
                    PolicyIllustration.InvestmentACBalance7 = item.FundBalanceDiv7;
                    PolicyIllustration.InvestmentACBalance9 = item.FundBalanceDiv9;
                    PolicyIllustration.InvestmentACBalance10 = item.FundBalanceDiv10;
                    PolicyIllustration.InvestmentACBalance11 = item.FundBalanceDiv11;
                    PolicyIllustration.MonthlyDrawDown5 = item.DrawDownDiv5;
                    PolicyIllustration.MonthlyDrawDown6 = item.DrawDownDiv6;
                    PolicyIllustration.MonthlyDrawDown7 = item.DrawDownDiv7;
                    PolicyIllustration.MonthlyDrawDown9 = item.DrawDownDiv9;
                    PolicyIllustration.MonthlyDrawDown10 = item.DrawDownDiv10;
                    PolicyIllustration.MonthlyDrawDown11 = item.DrawDownDiv11;
                    PolicyIllustration.UnAllocatedPremium = item.UnAllocatedPremium;

                    PolicyIllustration.ProposalNo = objPolicy.ProposalNo;
                    entity.tblPolicyIllustrations.Add(PolicyIllustration);
                }

                entity.SaveChanges();
            }
        }

        public void SendSMSToWP_UW(AIA.Life.Models.Policy.Policy objPolicy, string TemplateName = "")
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                var UWDeclineDecision = string.Empty;
                int duration = 0;
                //foreach (var item in objPolicy.objMemberDetails)
                //{
                //    var UWDeclineDecisionCode = Context.tblMemberLevelDecisions.Where(a => a.MemberID == item.MemberID).Select(a => a.UWReason).FirstOrDefault();
                //    if (!string.IsNullOrEmpty(UWDeclineDecisionCode))
                //    {
                //        UWDeclineDecision = Context.tblMasCommonTypes.Where(a => a.Code == UWDeclineDecisionCode && a.isDeleted == 0).Select(a => a.Description).FirstOrDefault();
                //    }
                //}
                for (int z = objPolicy.objMemberDetails.Count - 1; z >= 0; z--)
                {
                    if (objPolicy.objMemberDetails[z].ObjUwDecision.Decision == "1449"|| objPolicy.objMemberDetails[z].ObjUwDecision.Decision == "187")
                    {
                        duration = Convert.ToInt32(objPolicy.objMemberDetails[z].ObjUwDecision.UWMonth);
                        var MemberID = objPolicy.objMemberDetails[z].MemberID;
                        var UWDeclineDecisionCode = Context.tblMemberLevelDecisions.Where(a => a.MemberID == MemberID).Select(a => a.UWReason).FirstOrDefault();
                        if (!string.IsNullOrEmpty(UWDeclineDecisionCode))
                        {
                            UWDeclineDecision = Context.tblMasCommonTypes.Where(a => a.Code == UWDeclineDecisionCode && a.isDeleted == 0).Select(a => a.Description).FirstOrDefault();
                        }

                    }
                }

                SMSIntegration objSMSIntegration = new SMSIntegration();
                SMSDetails objSMSDetails = new SMSDetails();
                Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
                //var Salutation = Context.tblMasCommonTypes.Where(a => a.Code == objPolicy.objProspectDetails.Salutation).Select(a => a.ShortDesc).FirstOrDefault();
                //objSMSDetails.Salutation = objCommonBusiness.ConverttoTitlecase(Salutation);//objCommonBusiness.ConverttoTitlecase(objPolicy.objProspectDetails.Salutation);
                var Sal = objPolicy.objProspectDetails.Salutation;
                var Salutation = Context.tblMasCommonTypes.Where(a => a.Code == Sal && a.MasterType == "Salutation").Select(a => a.ShortDesc).FirstOrDefault();
                var Salu = Context.tblMasCommonTypes.Where(a => a.Description == Sal && a.MasterType == "Salutation").Select(a => a.ShortDesc).FirstOrDefault();
                if (!String.IsNullOrEmpty(Salutation))
                {
                    objSMSDetails.Salutation = Salutation;
                }
                else if (!String.IsNullOrEmpty(Salu))
                {
                    objSMSDetails.Salutation = Salu;
                }
                else
                {
                    objSMSDetails.Salutation = Sal;
                }
                objSMSDetails.Name = objCommonBusiness.ConverttoTitlecase(objPolicy.objProspectDetails.LastName);
                objSMSDetails.PolicyNo = objPolicy.ProposalNo;
                objSMSDetails.SMSTemplate = TemplateName;
                objSMSDetails.HealthconditionOccupation = UWDeclineDecision;
                //objSMSDetails.MobileNumber = objPolicy.objProspectDetails.MobileNo;
                var createdBy = Context.tblPolicies.Where(a => a.ProposalNo == objPolicy.ProposalNo).Select(a => a.Createdby).FirstOrDefault();
                // wealth planner Mobile...............................
                objSMSDetails.MobileNumber = Context.tblUserDetails.Where(a => a.UserID.ToString() == createdBy).Select(a => a.ContactNo).FirstOrDefault();
                //Context.tblMasIMOUsers.Where(a => a.UserName == objPolicy.UserName).Select(a => a.MobileNo).FirstOrDefault();
                objSMSDetails.SMSEnvironment = Convert.ToString(ConfigurationManager.AppSettings["SMSEnvironment"]);
                objSMSIntegration.SMSNotification(objSMSDetails);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void SendSMSToCustomer_UW(AIA.Life.Models.Policy.Policy objPolicy, string TemplateName = "")
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                SMSIntegration objSMSIntegration = new SMSIntegration();
                SMSDetails objSMSDetails = new SMSDetails();
                Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
                var UWDeclineDecision = string.Empty;
                int duration = 0;
                //foreach (var item in objPolicy.objMemberDetails)
                //{
                //    var UWDeclineDecisionCode = Context.tblMemberLevelDecisions.Where(a => a.MemberID == item.MemberID).Select(a => a.UWReason).FirstOrDefault();
                //    if (!string.IsNullOrEmpty(UWDeclineDecisionCode))
                //    {
                //        UWDeclineDecision = Context.tblMasCommonTypes.Where(a => a.Code == UWDeclineDecisionCode && a.isDeleted == 0).Select(a => a.Description).FirstOrDefault();
                //    }
                //}
                for (int z = objPolicy.objMemberDetails.Count - 1; z >= 0; z--)
                {
                    if (objPolicy.objMemberDetails[z].ObjUwDecision.Decision == "187")
                    {
                        duration = Convert.ToInt32(objPolicy.objMemberDetails[z].ObjUwDecision.UWMonth);
                        var MemberID = objPolicy.objMemberDetails[z].MemberID;
                        var UWDeclineDecisionCode = Context.tblMemberLevelDecisions.Where(a => a.MemberID == MemberID).Select(a => a.UWReason).FirstOrDefault();
                        if (!string.IsNullOrEmpty(UWDeclineDecisionCode))
                        {
                            UWDeclineDecision = Context.tblMasCommonTypes.Where(a => a.Code == UWDeclineDecisionCode && a.isDeleted == 0).Select(a => a.Description).FirstOrDefault();
                        }

                    }
                }

                //objSMSDetails.Salutation = objCommonBusiness.ConverttoTitlecase(objPolicy.objProspectDetails.Salutation);
                var Sal = objPolicy.objProspectDetails.Salutation;
                var Salutation = Context.tblMasCommonTypes.Where(a => a.Code == Sal && a.MasterType == "Salutation").Select(a => a.ShortDesc).FirstOrDefault();
                var Salu = Context.tblMasCommonTypes.Where(a => a.Description == Sal && a.MasterType == "Salutation").Select(a => a.ShortDesc).FirstOrDefault();
                if (!String.IsNullOrEmpty(Salutation))
                {
                    objSMSDetails.Salutation = Salutation;
                }
                else if (!String.IsNullOrEmpty(Salu))
                {
                    objSMSDetails.Salutation = Salu;
                }
                else
                {
                    objSMSDetails.Salutation = Sal;
                }
                objSMSDetails.Name = objCommonBusiness.ConverttoTitlecase(objPolicy.objProspectDetails.LastName);
                objSMSDetails.PolicyNo = objPolicy.ProposalNo;
                objSMSDetails.SMSTemplate = TemplateName;
                objSMSDetails.HealthconditionOccupation = UWDeclineDecision;
                objSMSDetails.MobileNumber = objPolicy.objProspectDetails.MobileNo; // Customer Mobile No
                objSMSDetails.SMSEnvironment = Convert.ToString(ConfigurationManager.AppSettings["SMSEnvironment"]);
                objSMSIntegration.SMSNotification(objSMSDetails);

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void SendSMSToCustomer_UWPostPone(AIA.Life.Models.Policy.Policy objPolicy, string TemplateName = "")
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                var UWDeclineDecision = string.Empty;
                //foreach (var item in objPolicy.objMemberDetails)
                //{
                //    var UWDeclineDecisionCode = Context.tblMemberLevelDecisions.Where(a => a.MemberID == item.MemberID).Select(a => a.UWReason).FirstOrDefault();
                //    if (!string.IsNullOrEmpty(UWDeclineDecisionCode))
                //    {
                //        UWDeclineDecision = Context.tblMasCommonTypes.Where(a => a.Code == UWDeclineDecisionCode && a.isDeleted == 0).Select(a => a.Description).FirstOrDefault();
                //    }
                //}
                int duration = 0;
                //foreach (var item in objPolicy.objMemberDetails)
                //{
                //    duration = Convert.ToInt32(item.ObjUwDecision.UWMonth);
                //}
                for (int z = objPolicy.objMemberDetails.Count - 1; z >= 0; z--)
                {
                    if (objPolicy.objMemberDetails[z].ObjUwDecision.Decision == "1449")
                    {
                        duration = Convert.ToInt32(objPolicy.objMemberDetails[z].ObjUwDecision.UWMonth);
                        var MemberID = objPolicy.objMemberDetails[z].MemberID;
                        var UWDeclineDecisionCode = Context.tblMemberLevelDecisions.Where(a => a.MemberID == MemberID).Select(a => a.UWReason).FirstOrDefault();
                        if (!string.IsNullOrEmpty(UWDeclineDecisionCode))
                        {
                            UWDeclineDecision = Context.tblMasCommonTypes.Where(a => a.Code == UWDeclineDecisionCode && a.isDeleted == 0).Select(a => a.Description).FirstOrDefault();
                        }

                    }
                }

                SMSIntegration objSMSIntegration = new SMSIntegration();
                SMSDetails objSMSDetails = new SMSDetails();
                Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
                //var Salutation = Context.tblMasCommonTypes.Where(a => a.Code == objPolicy.objProspectDetails.Salutation).Select(a => a.ShortDesc).FirstOrDefault();
                var Sal = objPolicy.objProspectDetails.Salutation;
                var Salutation = Context.tblMasCommonTypes.Where(a => a.Code == Sal && a.MasterType == "Salutation").Select(a => a.ShortDesc).FirstOrDefault();
                var Salu = Context.tblMasCommonTypes.Where(a => a.Description == Sal && a.MasterType == "Salutation").Select(a => a.ShortDesc).FirstOrDefault();
                if (!String.IsNullOrEmpty(Salutation))
                {
                    objSMSDetails.Salutation = Salutation;
                }
                else if (!String.IsNullOrEmpty(Salu))
                {
                    objSMSDetails.Salutation = Salu;
                }
                else
                {
                    objSMSDetails.Salutation = Sal;
                }
                //objSMSDetails.Salutation = objCommonBusiness.ConverttoTitlecase(Salutation);//objCommonBusiness.ConverttoTitlecase(objPolicy.objProspectDetails.Salutation);
                objSMSDetails.Name = objCommonBusiness.ConverttoTitlecase(objPolicy.objProspectDetails.LastName);
                objSMSDetails.PolicyNo = objPolicy.ProposalNo;
                objSMSDetails.SMSTemplate = TemplateName;
                objSMSDetails.HealthconditionOccupation = UWDeclineDecision;
                objSMSDetails.Months = Convert.ToString(duration);
                objSMSDetails.MobileNumber = objPolicy.objProspectDetails.MobileNo; // Customer Mobile No
                objSMSDetails.SMSEnvironment = Convert.ToString(ConfigurationManager.AppSettings["SMSEnvironment"]);
                objSMSIntegration.SMSNotification(objSMSDetails);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<LdmsDocuments> GetLdmsDocuments(AIA.Life.Models.Policy.Policy policy)
        {
            using (AVOAIALifeEntities entity = new AVOAIALifeEntities())
            {
                tblPolicy tblPolicy = entity.tblPolicies.Where(a => a.ProposalNo == policy.ProposalNo).FirstOrDefault();
                List<LdmsDocuments> lstldmsDocuments = new List<LdmsDocuments>();
                if (tblPolicy != null)
                {
                    var policyDocs = entity.tblPolicyDocuments.Where(a => a.PolicyID == tblPolicy.PolicyID).ToList();

                    foreach (var doc in policyDocs)
                    {
                        string filName = "";
                        if (!string.IsNullOrEmpty(doc.FilePath))
                        {
                            var arr = doc.FilePath.Split('\\');
                            var len = arr.Length;
                            filName = arr[len - 1];

                        }
                        LdmsDocuments ldmsDocuments = new LdmsDocuments();
                        ldmsDocuments.AgentCode = policy.UserName;
                        ldmsDocuments.ProposalNo = policy.ProposalNo;
                        ldmsDocuments.DocCode = entity.tblMasDocuments.Where(a => a.DocumentName == doc.FileName).Select(a => a.DocumentCode).FirstOrDefault();
                        if (!String.IsNullOrEmpty(ldmsDocuments.DocCode))
                        {
                            ldmsDocuments.SourcePath = (filName != "" ? doc.FilePath.Replace(filName, "LDMS\\" + ldmsDocuments.DocCode + ".pdf") : doc.FilePath);
                            lstldmsDocuments.Add(ldmsDocuments);
                        }
                        
                    }
                }
                return lstldmsDocuments;
            }
        }
        public SARFALDetails FetchSarAndFalDetails(SARFALDetails sARFAL)
        {
            using (AVOAIALifeEntities context = new AVOAIALifeEntities())
            {
                sARFAL.FAL = context.SP_GetFALDetails(sARFAL.Nic).FirstOrDefault();
                sARFAL.SAR = context.SP_GetSARDetails(sARFAL.Nic).FirstOrDefault().SAR;
                return sARFAL;
            }
        }
        public void PostPolicyIssuanceTriggers(AIA.Life.Models.Payment.PaymentModel objPaymentModel)
        {
            using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
            {
                objPaymentModel.lstPaymentItems = (from objpolicy in Context.tblPolicies.Where(a => a.QuoteNo == objPaymentModel.QuoteNo)
                                                   join objtblpolicyrelationship in Context.tblPolicyRelationships on objpolicy.PolicyID equals objtblpolicyrelationship.PolicyID
                                                   join objtblpolicyclients in Context.tblPolicyClients on objtblpolicyrelationship.PolicyClientID equals objtblpolicyclients.PolicyClientID
                                                   join objProposalPayments in Context.tblProposalPremiums on objpolicy.PolicyID equals objProposalPayments.PolicyID
                                                   join objProduct in Context.tblProducts on objpolicy.ProductID equals objProduct.ProductId
                                                   select new AIA.Life.Models.Payment.PaymentItems
                                                   {
                                                       ProposalNo = objpolicy.ProposalNo,
                                                       InsuredName = objtblpolicyclients.FirstName,
                                                       InsuredLastName = objtblpolicyclients.LastName,
                                                       PlanName = objProduct.ProductName,
                                                       PolicyId = objpolicy.PolicyID,
                                                       PolicyTerm = objpolicy.PolicyTerm,
                                                       IssueDate = objpolicy.CreatedDate,
                                                       Premium = objpolicy.PolicyStageStatusID == 2376 ? objProposalPayments.AdditionalPremium : objProposalPayments.AnnualPremium,
                                                       CustomerMobile = objtblpolicyclients.MobileNo,
                                                       PreferredLanguage = objpolicy.PreferredLanguage,
                                                       Salutation = objtblpolicyclients.Title,
                                                       PolicyStartDate = objpolicy.PolicyStartDate,
                                                       ProductID = objProduct.ProductId,
                                                       PolicyEndDate = objpolicy.PolicyEndDate,
                                                       Email = objtblpolicyclients.EmailID,
                                                       PrefferedMode = objpolicy.PaymentFrequency,
                                                       PlanId = objpolicy.PlanID,
                                                       Mobile = objtblpolicyclients.MobileNo
                                                   }).OrderByDescending(a => a.PolicyId).ToList();
                var WPEmailId = Context.tblMasIMOUsers.Where(a => a.UserName == objPaymentModel.UserName).Select(a => a.Email).FirstOrDefault();
                for (int i = 0; i < objPaymentModel.lstPaymentItems.Count; i++)
                {
                    var allEmails = objPaymentModel.lstPaymentItems[i].Email + "," + WPEmailId;
                    if (allEmails.StartsWith(","))
                    {
                        allEmails = allEmails.Substring(1);
                    }
                    if (allEmails.EndsWith(","))
                    {
                        allEmails = allEmails.Remove(allEmails.Count() - 1);
                    }
                    if (allEmails != null && allEmails != ",")
                    {
                        string wpMobile = Context.tblMasIMOUsers.Where(a => a.UserID == objPaymentModel.UserName).Select(a => a.MobileNo).FirstOrDefault();
                        SMSIntegration objSMSIntegration = new SMSIntegration();
                        SMSDetails objSMSDetails = new SMSDetails();
                        Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
                        string Sal = objPaymentModel.lstPaymentItems[i].Salutation;
                        var Salutation = Context.tblMasCommonTypes.Where(a => a.Code == Sal && a.MasterType == "Salutation").Select(a => a.ShortDesc).FirstOrDefault();
                        var Salu = Context.tblMasCommonTypes.Where(a => a.Description == Sal && a.MasterType == "Salutation").Select(a => a.ShortDesc).FirstOrDefault();
                        if (!String.IsNullOrEmpty(Salutation))
                        {
                            objSMSDetails.Salutation = Salutation;
                        }
                        else if (!String.IsNullOrEmpty(Salu))
                        {
                            objSMSDetails.Salutation = Salu;
                        }
                        else
                        {
                            objSMSDetails.Salutation = Sal;
                        }
                        //string Salutation = Context.tblMasCommonTypes.Where(a => a.Code == Salu && a.MasterType == "Salutation").Select(a => a.ShortDesc).FirstOrDefault();
                        //objSMSDetails.Salutation = Salutation;
                        objSMSDetails.Name = objCommonBusiness.ConverttoTitlecase(objPaymentModel.lstPaymentItems[i].InsuredLastName);
                        objSMSDetails.PolicyNo = objPaymentModel.lstPaymentItems[i].ProposalNo;
                        objSMSDetails.SMSEnvironment = Convert.ToString(ConfigurationManager.AppSettings["SMSEnvironment"]);
                        //to customer on proposal acceptance
                        objSMSDetails.SMSTemplate = "S023";
                        objSMSDetails.MobileNumber = objPaymentModel.lstPaymentItems[i].CustomerMobile;
                        objSMSIntegration.SMSNotification(objSMSDetails);
                        //to WP on proposal acceptance
                        objSMSDetails.SMSTemplate = "S022";
                        objSMSDetails.MobileNumber = wpMobile;
                        objSMSIntegration.SMSNotification(objSMSDetails);
                        EmailIntegration ObjEmailIntegration = new EmailIntegration();
                        EmailDetails ObjEmailDetails = new EmailDetails();
                        ObjEmailDetails.EmailID = allEmails;
                        ObjEmailDetails.MailTemplate = "T010";
                        ObjEmailDetails.MobileNumber = objPaymentModel.lstPaymentItems[i].CustomerMobile;
                        ObjEmailDetails.Name = objCommonBusiness.ConverttoTitlecase(objPaymentModel.lstPaymentItems[i].InsuredLastName);
                        //ObjEmailDetails.Salutation = Salutation;
                        string ESal = objPaymentModel.lstPaymentItems[i].Salutation;
                        var ESalutation = Context.tblMasCommonTypes.Where(a => a.Code == Sal && a.MasterType == "Salutation").Select(a => a.Description).FirstOrDefault();
                        var ESalu = Context.tblMasCommonTypes.Where(a => a.Description == Sal && a.MasterType == "Salutation").Select(a => a.Description).FirstOrDefault();
                        if (!String.IsNullOrEmpty(ESalutation))
                        {
                            ObjEmailDetails.Salutation = ESalutation;
                        }
                        else if (!String.IsNullOrEmpty(ESalu))
                        {
                            ObjEmailDetails.Salutation = ESalu;
                        }
                        else
                        {
                            ObjEmailDetails.Salutation = ESal;
                        }
                        ObjEmailDetails.Subject = "Issuance of Life Insurance Policy: " + objPaymentModel.lstPaymentItems[i].ProposalNo + " - " + ObjEmailDetails.Salutation + " " + objCommonBusiness.ConverttoTitlecase(objPaymentModel.lstPaymentItems[i].InsuredLastName);
                        ObjEmailDetails.PolicyStartDate = DateTime.Now.ToString();
                        ObjEmailDetails.WPMobileNo = wpMobile;
                        ObjEmailDetails.PolicyEndDate = DateTime.Now.ToString();
                        ObjEmailDetails.PolicyNumber = objPaymentModel.lstPaymentItems[i].ProposalNo;
                        ObjEmailDetails.ProductName = objPaymentModel.lstPaymentItems[i].PlanName;
                        ObjEmailDetails.Premium = objPaymentModel.lstPaymentItems[i].Premium.ToString();
                        ObjEmailDetails.ByteArray2 = objPaymentModel.ByteArray2;
                        ObjEmailDetails.ByteArray3 = objPaymentModel.ByteArray3;
                        ObjEmailDetails.PolicyNumber = objPaymentModel.lstPaymentItems[i].ProposalNo;

                        ObjEmailIntegration.EmailNotification(ObjEmailDetails);

                        //to customer on policy pdf mail
                        objSMSDetails.SMSTemplate = "S024";
                        //var Saluta = objPaymentModel.lstPaymentItems[i].Salutation;
                        //var Title = Context.tblMasCommonTypes.Where(a => a.Code == Saluta).Select(a => a.ShortDesc).FirstOrDefault();
                        //objSMSDetails.surName = objCommonBusiness.ConverttoTitlecase(Title);
                        
                        if (!String.IsNullOrEmpty(Salutation))
                        {
                            objSMSDetails.Salutation = Salutation;
                        }
                        else if (!String.IsNullOrEmpty(Salu))
                        {
                            objSMSDetails.Salutation = Salu;
                        }
                        else
                        {
                            objSMSDetails.Salutation = Sal;
                        }
                        objSMSDetails.Name = objCommonBusiness.ConverttoTitlecase(objPaymentModel.lstPaymentItems[i].InsuredLastName);
                        objSMSDetails.PolicyNo = objPaymentModel.lstPaymentItems[i].ProposalNo;
                        objSMSDetails.MobileNumber = objPaymentModel.lstPaymentItems[i].CustomerMobile;
                        objSMSIntegration.SMSNotification(objSMSDetails);
                        //to WP on policy pdf mail
                        objSMSDetails.SMSTemplate = "S025";
                        objSMSDetails.MobileNumber = wpMobile;
                        objSMSDetails.PolicyNo = objPaymentModel.lstPaymentItems[i].ProposalNo;
                        objSMSIntegration.SMSNotification(objSMSDetails);
                    }
                }
            }
        }
        public AIA.Life.Models.Policy.Policy SubmitCounterOfferQuote(AIA.Life.Models.Policy.Policy ObjPolicy)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    tblPolicy policy = Context.tblPolicies.Where(a => a.QuoteNo == ObjPolicy.QuoteNo).FirstOrDefault();
                    if (policy != null)
                    {
                        ObjPolicy.ProposalNo = policy.ProposalNo;
                        ObjPolicy.PolicyID = policy.PolicyID;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ObjPolicy;

        }
        public void SendDocumentsEmail(AIA.Life.Models.Policy.Policy ObjPolicy)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                EmailIntegration ObjEmailIntegration = new EmailIntegration();
                EmailDetails ObjEmailDetails = new EmailDetails();
                Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
                var objtblPolicy = Context.tblPolicies.Where(a => a.ProposalNo == ObjPolicy.ProposalNo).FirstOrDefault();
                var objPolicyMember = Context.tblPolicyMemberDetails.Where(a => a.PolicyID == objtblPolicy.PolicyID).FirstOrDefault();
                ObjEmailDetails.EmailID = objPolicyMember.Email;
                var WPEmail = Context.tblMasIMOUsers.Where(a => a.UserName == ObjPolicy.UserName).Select(a => a.Email).FirstOrDefault();
                var WPMobile = Context.tblMasIMOUsers.Where(a => a.UserName == ObjPolicy.UserName).Select(a => a.MobileNo).FirstOrDefault();
                ObjEmailDetails.AgentEmailID = WPEmail;
                ObjEmailDetails.WPMobileNo = WPMobile;
                ObjEmailDetails.MailTemplate = "T005";
                ObjEmailDetails.MobileNumber = objPolicyMember.Mobile;
                ObjEmailDetails.Name = objCommonBusiness.ConverttoTitlecase(objPolicyMember.LastName);
                //ObjEmailDetails.Salutation = Salutation;
                string ESal = objPolicyMember.Salutation;
                var ESalutation = Context.tblMasCommonTypes.Where(a => a.Code == ESal && a.MasterType == "Salutation").Select(a => a.Description).FirstOrDefault();
                var ESalu = Context.tblMasCommonTypes.Where(a => a.Description == ESal && a.MasterType == "Salutation").Select(a => a.Description).FirstOrDefault();
                if (!String.IsNullOrEmpty(ESalutation))
                {
                    ObjEmailDetails.Salutation = ESalutation;
                }
                else if (!String.IsNullOrEmpty(ESalu))
                {
                    ObjEmailDetails.Salutation = ESalu;
                }
                else
                {
                    ObjEmailDetails.Salutation = ESal;
                }
                ObjEmailDetails.Subject = ObjPolicy.ProposalNo + " - " + ObjEmailDetails.Salutation + " " + ObjEmailDetails.Name + " - Receipt of additional requirements";
                List<string> RecievedDoc = new List<string>();
                List<string> PendingDoc = new List<string>();
                for (int a = 0; a < ObjPolicy.objMemberDetails.Count; a++)
                {
                    for (int b = 0; b < ObjPolicy.objMemberDetails[a].ObjUwDecision.lstUWMedicalDocument.Count; b++)
                    {
                        if (ObjPolicy.objMemberDetails[a].ObjUwDecision.lstUWMedicalDocument[b].Status == "2370")
                        {
                            Documents ObjDocuments = new Documents();
                            var MDoc = ObjPolicy.objMemberDetails[a].ObjUwDecision.lstUWMedicalDocument[b].Document;
                            if (a == 0)
                            {
                                ObjDocuments.DocumentName = MDoc;
                                ObjDocuments.Member = "MainLife";
                            }
                            if (a == 1)
                            {
                                ObjDocuments.DocumentName = MDoc;
                                ObjDocuments.Member = "Spouse";
                            }
                            if (a > 1)
                            {
                                ObjDocuments.DocumentName = MDoc;
                                ObjDocuments.Member = "Child"+(a-1);
                            }
                            ObjEmailDetails.objRecievedDoc.Add(ObjDocuments);
                        }
                        if (ObjPolicy.objMemberDetails[a].ObjUwDecision.lstUWMedicalDocument[b].Status == "2368")
                        {
                            Documents ObjDocuments = new Documents();
                            var MDoc = ObjPolicy.objMemberDetails[a].ObjUwDecision.lstUWMedicalDocument[b].Document;
                            if (a == 0)
                            {
                                ObjDocuments.DocumentName = MDoc;
                                ObjDocuments.Member = "MainLife";
                            }
                            if (a == 1)
                            {
                                ObjDocuments.DocumentName = MDoc;
                                ObjDocuments.Member = "Spouse";
                            }
                            if (a > 1)
                            {
                                ObjDocuments.DocumentName = MDoc;
                                ObjDocuments.Member = "Child" + (a - 1);
                            }
                            ObjEmailDetails.objPendingDoc.Add(ObjDocuments);

                        }
                    }
                    for (int c = 0; c < ObjPolicy.objMemberDetails[a].ObjUwDecision.lstUWNonMedicalDocument.Count; c++)
                    {
                        if (ObjPolicy.objMemberDetails[a].ObjUwDecision.lstUWNonMedicalDocument[c].Status == "2370" && ObjPolicy.objMemberDetails[a].ObjUwDecision.lstUWNonMedicalDocument[c].Document != "Age Proof")
                        {
                            Documents ObjDocuments = new Documents();
                            var NMDoc = ObjPolicy.objMemberDetails[a].ObjUwDecision.lstUWNonMedicalDocument[c].Document;
                            if (a == 0)
                            {
                                ObjDocuments.DocumentName = NMDoc;
                                ObjDocuments.Member = "MainLife";
                            }
                            if (a == 1)
                            {
                                ObjDocuments.DocumentName = NMDoc;
                                ObjDocuments.Member = "Spouse";
                            }
                            if (a > 1)
                            {
                                ObjDocuments.DocumentName = NMDoc;
                                ObjDocuments.Member = "Child" + (a - 1);
                            }
                            ObjEmailDetails.objRecievedDoc.Add(ObjDocuments);

                        }
                        if (ObjPolicy.objMemberDetails[a].ObjUwDecision.lstUWNonMedicalDocument[c].Status == "2368")
                        {
                            Documents ObjDocuments = new Documents();
                            var NMDoc = ObjPolicy.objMemberDetails[a].ObjUwDecision.lstUWNonMedicalDocument[c].Document;
                            if (a == 0)
                            {
                                ObjDocuments.DocumentName = NMDoc;
                                ObjDocuments.Member = "MainLife";
                            }
                            if (a == 1)
                            {
                                ObjDocuments.DocumentName = NMDoc;
                                ObjDocuments.Member = "Spouse";
                            }
                            if (a > 1)
                            {
                                ObjDocuments.DocumentName = NMDoc;
                                ObjDocuments.Member = "Child" + (a - 1);
                            }
                            ObjEmailDetails.objPendingDoc.Add(ObjDocuments);

                        }
                    }
                }
                
                StringBuilder sb = new StringBuilder();

                sb.Append(" <table border = '1' cellpadding = '0' cellspacing = '0' width='600'> ");
                sb.Append("<tr>");
                sb.Append("<td align='center' width='50'><b>No</b></td>");
                // sb.Append("<td align='center'><b>Document Type</b></td>");
                sb.Append("<td align='center' width='400'><b>Document Name</b></td>");
                sb.Append("<td align='center' width='150'><b>Member</b></td>");
                sb.Append("</tr>");

                int i = 0;
                foreach (var item in ObjEmailDetails.objRecievedDoc)
                {
                    i = i + 1;
                    sb.Append("<tr>");
                    sb.Append("<td align='center' width='50'>" + i + "</td>");
                    //sb.Append("<td align='center'>" + Convert.ToString(item.DocumentType + "</td>"));
                    sb.Append("<td align='center' width='400'>" + Convert.ToString(item.DocumentName + "</td>"));
                    sb.Append("<td align='center' width='150'>" + Convert.ToString(item.Member + "</td>"));

                    sb.Append("</tr>");
                }
                sb.Append("</table>");
                sb.Append("");
                ObjEmailDetails.TableQuotes = sb.ToString();

                StringBuilder sc = new StringBuilder();

                sc.Append(" <table border = '1' cellpadding = '0' cellspacing = '0' width='600'> ");
                sc.Append("<tr>");
                sc.Append("<td align='center' width='50'><b>No</b></td>");
                // sb.Append("<td align='center'><b>Document Type</b></td>");
                sc.Append("<td align='center' width='400'><b>Document Name</b></td>");
                sc.Append("<td align='center' width='150'><b>Member</b></td>");
                sc.Append("</tr>");

                int z = 0;
                foreach (var item in ObjEmailDetails.objPendingDoc)
                {
                    z = z + 1;
                    sc.Append("<tr>");
                    sc.Append("<td align='center' width='50'>" + z + "</td>");
                    //sb.Append("<td align='center'>" + Convert.ToString(item.DocumentType + "</td>"));
                    sc.Append("<td align='center' width='400'>" + Convert.ToString(item.DocumentName + "</td>"));
                    sc.Append("<td align='center' width='150'>" + Convert.ToString(item.Member + "</td>"));

                    sc.Append("</tr>");
                }
                sc.Append("</table>");
                sc.Append("");
                ObjEmailDetails.TableNonMedicalQuotes = sc.ToString();
                ObjEmailIntegration.EmailNotification(ObjEmailDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

        }
    }
}