using AIA.Life.Data.API.ControllerLogic.Common;
using AIA.Life.Data.API.ControllerLogic.Prospect;
using AIA.Life.Repository.AIAEntity;
using AIA.Life.Models.Common;
using AIA.Life.Models.Opportunity;
using AIA.Life.Models.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MoreLinq;
using System.Data;
using System.Data.Entity;
using AIA.Life.Integration.Services.EmailandSMS;
using AIA.Life.Models.EmailSMSDetails;
using AIA.CrossCutting;
using log4net;
//using AIA.Life.Integration.Services.SamsIntegration;
using System.Configuration;
using System.Globalization;

namespace AIA.Life.Data.API.ControllerLogic.Quote
{
    public class QuoteLogic
    {


        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        public List<QuotionPool> GetQuotationPool(LifeQuote objLifeQuote)
        {
            string userId = Common.CommonBusiness.GetUserId(objLifeQuote.UserName);
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            List<QuotionPool> objLstQuotationPool = (from Opportunity in Context.tblLifeQQs.Where(a => a.StatusID == 1 && a.IsActive == true && a.Createdby == userId)
                                                     join Contact in Context.tblContacts.Where(a => a.CreatedBy == userId)
                                                     on Opportunity.ContactID equals Contact.ContactID
                                                     join Common in Context.tblProducts on Opportunity.ProductNameID equals Common.ProductId
                                                     orderby Opportunity.CreateDate descending
                                                     select new QuotionPool
                                                     {
                                                         QuotationId = Opportunity.LifeQQID,
                                                         QuotaionType = Contact.ContactType,
                                                         QuotationNo = Opportunity.QuoteNo,
                                                         Plancode = Opportunity.PlanCode,
                                                         PreferredLanguauge = Opportunity.PreferredLanguage,
                                                         ProductCode = Common.ProductCode,
                                                         Name = Contact.FirstName,
                                                         Mobile = Contact.MobileNo,
                                                         Salutation = Context.tblMasCommonTypes.Where(a => a.Code == Contact.Title).Select(b => b.ShortDesc).FirstOrDefault(),
                                                         LeadNo = Contact.LeadNo,
                                                         QuotationCreationDate = Opportunity.CreateDate.ToString(),
                                                         SurName = Contact.LastName,
                                                         BancaFPC = Contact.IntroducerCode,
                                                         Email = Contact.EmailID,
                                                         NicNo = Contact.NICNO,
                                                         Daysleft = 3,
                                                         FullName = Context.tblMasCommonTypes.Where(a => a.Code == Contact.Title).Select(b => b.ShortDesc).FirstOrDefault() + " " + Contact.FirstName + " " + Contact.LastName

                                                     }).ToList();
            foreach (var obj in objLstQuotationPool)
            {
                obj.QuotationCreationDate = obj.QuotationCreationDate.ToDate().ToString("dd/MM/yyyy");

            }


            return objLstQuotationPool;
        }


        public AIA.Life.Models.Opportunity.LifeQuote LoadMastersForQuote(AIA.Life.Models.Opportunity.LifeQuote ObjLifeQuote)
        {
            Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
            ProductMasters obj = new ProductMasters();
            obj.ProductName = ObjLifeQuote.objProductDetials.Plan;
            obj = objCommonBusiness.LoadProductMasters(obj);
            ObjLifeQuote.lstGender = objCommonBusiness.GetGender();
            ObjLifeQuote.lstOccupation = objCommonBusiness.GetOccupation();
            ObjLifeQuote.lstLanguage = objCommonBusiness.GetMasCommonTypeMasterListItem("Language");
            ObjLifeQuote.lstPrefMode = objCommonBusiness.GetPreferredModes(string.Empty);
            ObjLifeQuote.lstSumInsured = objCommonBusiness.GetSumInsured();

            return ObjLifeQuote;
        }

        public AIA.Life.Models.Policy.Policy LoadMastersForProposalDetails(AIA.Life.Models.Policy.Policy ObjLifeQuote)
        {
            Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
            ObjLifeQuote.ListPlan = objCommonBusiness.ListProducts();
            // ObjLifeQuote.ListPlan = objCommonBusiness.GetMasCommonTypeMasterListItem("PlanMaster");
            ObjLifeQuote.LstPolicyTerm = objCommonBusiness.GetPensionPeriod();
            ObjLifeQuote.lstGender = objCommonBusiness.GetGender();
            ObjLifeQuote.lstOccupation = objCommonBusiness.GetOccupation();
            ObjLifeQuote.lstLanguage = objCommonBusiness.GetMasCommonTypeMasterListItem("Language");
            return ObjLifeQuote;
        }

        public List<AIA.Life.Models.Common.BenifitDetails> LoadMasBenifits(int Plan, QuoteMemberDetails objMember,bool AgeChange=false,LifeQuote ObjLifeQuote=null)
        {
            string AssuredMember = objMember.Assured;

            List<int> rel = new List<int>();

            if (objMember.Relationship == "267")
            {
                rel.Add(6);
                rel.Add(1);
            }
            else if (objMember.Relationship == "268")
            {
                rel.Add(2);
            }
            else
            {
                rel.Add(3);
            }
            List<AIA.Life.Models.Common.BenifitDetails> lstRiders = new List<AIA.Life.Models.Common.BenifitDetails>();
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            int nextAge = 0;
            nextAge = objMember.AgeNextBirthDay;
            CommonBusiness CommonObj = new CommonBusiness();
            nextAge=CommonObj.GetCurrentAge(objMember.DateOfBirth,ObjLifeQuote.RiskCommencementDate);
         
            
            lstRiders = Context.tblProductPlanRiders.Where(Rider => Rider.PlanId == Plan && rel.Contains(Rider.RelationID ?? 0) && (nextAge >= Rider.MinAge && nextAge <= Rider.MaxAge)).OrderBy(a => a.DisplayOrder).Select(Rider => new AIA.Life.Models.Common.BenifitDetails()
            {
                BenifitName = Rider.DisplayName,
                BenefitID = Rider.ProductPlanRiderId,
                RiderCode = Rider.RefRiderCode,
                RiderID = Rider.RiderId ?? 0,
                MinAge = Rider.MinAge,
                MaxAge = Rider.MaxAge,
                MinSumInsured = Rider.MinSumAssured == null ? "0" : Rider.MinSumAssured.ToString(),
                MaxSumInsured = Rider.MaxSumAssured == null ? "0" : Rider.MaxSumAssured.ToString(),
                CalType = Rider.CalType == null ? "UI" : Rider.CalType,
                BenifitOpted = Rider.Mandatory ?? false,
                Mandatory = Rider.Mandatory ?? false
            }).ToList();

            return lstRiders;
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
        public AIA.Life.Models.Opportunity.LifeQuote LoadBenefits(AIA.Life.Models.Opportunity.LifeQuote objQuote)
        {
            int planId = Convert.ToInt32(objQuote.objProductDetials.Variant);
            int age = 0;
            for (int MemberIndex = 0; MemberIndex < objQuote.objQuoteMemberDetails.Count; MemberIndex++)

            {
                List<int> rel = new List<int>();

                objQuote._memberIndex = MemberIndex;
                objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails = new List<AIA.Life.Models.Common.BenifitDetails>();
                if (objQuote.objQuoteMemberDetails[MemberIndex].Relationship == "267")
                {
                    rel.Add(6);
                    rel.Add(1);
                    objQuote.objQuoteMemberDetails[MemberIndex].AgeNextBirthDay = age = Convert.ToInt32(objQuote.objProspect.AgeNextBdy);
                }
                else if (objQuote.objQuoteMemberDetails[MemberIndex].Relationship == "268")
                {
                    rel.Add(2);
                    objQuote.objQuoteMemberDetails[MemberIndex].AgeNextBirthDay = age = Convert.ToInt32(objQuote.objSpouseDetials.AgeNextBirthday);
                }
                else
                {
                    if (objQuote.objQuoteMemberDetails[MemberIndex].Assured != null)
                    {
                        // Added for Child Age Issue
                        try
                        {
                            rel.Add(3);
                            string ChildIndex = objQuote.objQuoteMemberDetails[MemberIndex].Assured.Substring(objQuote.objQuoteMemberDetails[MemberIndex].Assured.Length - 1, 1);
                            int index = Convert.ToInt32(ChildIndex);
                            if (index > 0)
                            {
                                int _index = (index - 1);
                                objQuote.objQuoteMemberDetails[MemberIndex].AgeNextBirthDay = age = objQuote.objChildDetials[_index].AgeNextBirthday;
                            }
                        }
                        catch (Exception ex)
                        {
                        }

                    }

                }
                string preferredLanguage = objQuote.objProductDetials.PreferredLangauage;
                AVOAIALifeEntities entities = new AVOAIALifeEntities();

                var premium = entities.tblProductPlanRiders.Where(Rider => Rider.PlanId == planId && Rider.IsActive == true && rel.Contains(Rider.RelationID ?? 0) && (age >= Rider.MinAge && age <= Rider.MaxAge)).OrderBy(a => a.DisplayOrder).Select(Rider => new AIA.Life.Models.Common.BenifitDetails()
                {
                    BenifitName = Rider.DisplayName,
                    //BenifitName = (preferredLanguage== "1137" && !string.IsNullOrEmpty(Rider.DisplayName) ? Rider.DisplayName: preferredLanguage == "1138" && !string.IsNullOrEmpty(Rider.DisplaySinhala) ? Rider.DisplaySinhala : preferredLanguage == "1139" && !string.IsNullOrEmpty(Rider.DisplayTamil) ? Rider.DisplayTamil : Rider.DisplayName),
                    BenefitID = Rider.ProductPlanRiderId,
                    RiderCode = Rider.RefRiderCode,
                    RiderID = Rider.RiderId ?? 0,
                    MinAge = Rider.MinAge,
                    MaxAge = Rider.MaxAge,
                    MinSumInsured = Rider.MinSumAssured == null ? "0" : Rider.MinSumAssured.ToString(),
                    MaxSumInsured = Rider.MaxSumAssured == null ? "0" : Rider.MaxSumAssured.ToString(),
                    CalType = Rider.CalType == null ? "UI" : Rider.CalType,
                    BenifitOpted = Rider.Mandatory ?? false,
                    Mandatory = Rider.Mandatory ?? false

                }).ToList();

                objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails.AddRange(premium);
                List<int> toBeRemoved = new List<int>();
                List<BenifitDetails> removeList = new List<BenifitDetails>();
                for (int i = 0; i < objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails.Count; i++)
                {
                    if (objQuote.objProductDetials.IsFamilyFloater == true)
                    {

                        if (Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 28 ||
                            Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 35
                            || Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 29
                            || Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 36
                            || Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 37
                            || Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 17
                            || Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 18
                            || Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 19)
                        {
                            toBeRemoved.Add(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID);
                        }

                    }
                    else
                    {
                        if (Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 87
                            || Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 20
                            || Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 93
                            || Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 94
                            || Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 95
                            || Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 96)

                        {
                            toBeRemoved.Add(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID);
                        }
                    }
                    if (objQuote.objProspect.Gender == "M")
                    {
                        if (Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 22)

                        {
                            toBeRemoved.Add(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID);
                        }
                    }
                    if (objQuote.objSpouseDetials.Gender == "M")
                    {
                        if (Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 92)

                        {
                            toBeRemoved.Add(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID);
                        }
                    }
                    if (toBeRemoved.Count > 0)
                    {

                        removeList = objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails.Where(a => toBeRemoved.Contains(a.BenefitID)).ToList();


                    }
                }
                foreach (var item in removeList)
                {
                    objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails.Remove(item);
                }


                //objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails.AddRange(LoadMasBenifits(objQuote.objProductDetials.Plan, objQuote.objQuoteMemberDetails[MemberIndex]));
            }

            return objQuote;
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



        public AIA.Life.Models.Opportunity.LifeQuote SaveQuote(AIA.Life.Models.Opportunity.LifeQuote objQuote)
        {
            try
            {
                #region recalculating premium for sellwell
                if (!string.IsNullOrEmpty(objQuote.ServiceTraceID))
                {
                    PremiumCalculation.Premium premium = new PremiumCalculation.Premium();
                    objQuote = premium.ValidateProductDetails(objQuote);
                    if (string.IsNullOrEmpty(objQuote.Error.ErrorMessage))
                    {
                        objQuote = premium.CalculateQuotePremium(objQuote);
                        objQuote = premium.ValidatePremiumDetails(objQuote);
                    }
                }
                #endregion

                if (string.IsNullOrEmpty(objQuote.Error.ErrorMessage))
                {
                    AVOAIALifeEntities Context = new AVOAIALifeEntities();
                    tblLifeQQ objlifeQQ = new tblLifeQQ();

                    string userId = Common.CommonBusiness.GetUserId(objQuote.UserName);
                    objlifeQQ.Createdby = userId;
                    if (objQuote.RefNo == null)
                    {

                        objQuote.RefNo = objQuote.UserName + " - AVO";
                    }
                    objlifeQQ.RefNo = objQuote.RefNo;
                    objlifeQQ.PolicyTermID = Convert.ToInt32(objQuote.objProductDetials.PolicyTerm);
                    objlifeQQ.PremiumTerm = Convert.ToInt32(objQuote.objProductDetials.PremiumTerm);
                    objlifeQQ.QType = objQuote.QuotationType;
                    tblContact objcontact = null;
                    if (!string.IsNullOrEmpty(objQuote.objProspect.SamsLeadNumber))
                        objcontact = Context.tblContacts.Where(a => a.LeadNo == objQuote.objProspect.SamsLeadNumber).FirstOrDefault();
                    else
                        objcontact = Context.tblContacts.Where(a => a.ContactID == objQuote.objProspect.ContactID).FirstOrDefault();
                    if (objcontact == null)
                    {
                        objcontact = new tblContact();
                    }
                    if (objcontact.tblAddress == null)
                        objcontact.tblAddress = new tblAddress();
                    //tblOpportunity objOppurtunity = Context.tblOpportunities.Where(a => a.ContactID == objQuote.objProspect.ContactID).FirstOrDefault();
                    //if(objOppurtunity==null)
                    //{
                    //    objOppurtunity = new tblOpportunity();
                    //    objOppurtunity.tblContact = objcontact;
                    //    objOppurtunity.StageID = 4; // need analysis
                    //    objOppurtunity.tblLifeNeedAnalysi = objcontact.tblLifeNeedAnalysis.FirstOrDefault();
                    //    objOppurtunity.Createdby = userId;
                    //    Context.tblOpportunities.Add(objOppurtunity);
                    //}
                    //else
                    //{
                    //    objOppurtunity.StageID = 4; // need analysis
                    //    objOppurtunity.tblLifeNeedAnalysi = objcontact.tblLifeNeedAnalysis.FirstOrDefault();
                    //}
                    objcontact.CreatedBy = userId;
                    objlifeQQ.Createdby = userId;
                    int type = Context.tblMasCommonTypes.Where(a => a.Description == objQuote.objProspect.Type && (a.MasterType == "Type" || a.MasterType == "BancaType")).Select(a => a.CommonTypesID).FirstOrDefault();
                    if (type == 0)
                    {
                        type = Context.tblMasCommonTypes.Where(a => a.Code == objQuote.objProspect.Type && a.MasterType == "Type").Select(a => a.CommonTypesID).FirstOrDefault();
                        if (type == 0)
                            type = Convert.ToInt32(objQuote.objProspect.Type);
                    }

                    objcontact.ContactType = Context.tblMasCommonTypes.Where(a => a.CommonTypesID == type).Select(a => a.Description).FirstOrDefault();
                    objcontact.FirstName = objQuote.objProspect.Name;
                    objcontact.LastName = objQuote.objProspect.LastName;
                    objcontact.Work = objQuote.objProspect.Work;
                    objcontact.MobileNo = objQuote.objProspect.Mobile;
                    objcontact.PhoneNo = objQuote.objProspect.Home;
                    objcontact.LeadNo = objQuote.objProspect.SamsLeadNumber;
                    objcontact.EmailID = objQuote.objProspect.Email;
                    objcontact.NICNO = objQuote.objProspect.NIC;
                    objcontact.Title = Context.tblMasCommonTypes.Where(a => a.Description == objQuote.objProspect.Salutation && a.MasterType == "Salutation").Select(a => a.Code).FirstOrDefault();
                    if (string.IsNullOrEmpty(objcontact.Title))
                        objcontact.Title = objQuote.objProspect.Salutation;
                    objcontact.Place = objQuote.objProspect.Place;

                    //objcontact.OccupationID = Convert.ToInt32(Context.tblMasLifeOccupations.Where(a => a.OccupationCode == objQuote.objProspect.Occupation).Select(a => a.CompanyCode).FirstOrDefault());
                    if (!string.IsNullOrEmpty(objQuote.objProspect.Occupation))
                    {
                        string[] SplitOccupation = objQuote.objProspect.Occupation.Split('|');
                        string EngOccupation = SplitOccupation[0];
                        objcontact.OccupationID = Convert.ToInt32(Context.tblMasLifeOccupations.Where(a => a.OccupationCode == EngOccupation).Select(a => a.CompanyCode).FirstOrDefault());
                    }
                    else
                    {
                        objcontact.OccupationID = 0;
                    }
                    objcontact.PassportNo = objQuote.objProspect.PassPort;
                    objcontact.DateOfBirth = objQuote.objProspect.DateofBirth;
                    objcontact.Age = objQuote.objProspect.AgeNextBdy;
                    objcontact.CurrentAge = objQuote.objProspect.CurrentAge;
                    objcontact.MaritalStatusID = Context.tblMasCommonTypes.Where(a => a.Code == objQuote.objProspect.MaritalStatus && a.MasterType == "MaritalStatus").Select(b => b.CommonTypesID).FirstOrDefault();

                    objcontact.Gender = objQuote.objProspect.Gender;
                    objcontact.MonthlyIncome = objQuote.objProspect.AvgMonthlyIncome;

                    if (!string.IsNullOrEmpty(objQuote.objProspect.objAddress.Pincode) && objQuote.objProspect.objAddress.Pincode.Contains('|'))
                    {
                        string[] PinCity = objQuote.objProspect.objAddress.Pincode.Split('|');
                        if (PinCity != null && PinCity.Length == 2)
                        {
                            string pin = PinCity[0];
                            string City = PinCity[1];
                            objcontact.tblAddress.City = City;
                            objcontact.tblAddress.Pincode = pin;
                        }
                    }
                    objcontact.tblAddress.Address1 = objQuote.objProspect.objAddress.Address1;
                    objcontact.tblAddress.Address2 = objQuote.objProspect.objAddress.Address2;
                    objcontact.tblAddress.Address3 = objQuote.objProspect.objAddress.Address3;
                    objcontact.tblAddress.District = objQuote.objProspect.objAddress.District;
                    objcontact.tblAddress.State = objQuote.objProspect.objAddress.State;
                    objcontact.tblAddress.Country = objQuote.objProspect.objAddress.Province;
                    objcontact.ClientCode = objQuote.objProspect.ClientCode;

                    if (objcontact.ContactID == 0)
                    {
                        objcontact.CreationDate = DateTime.Now;
                        Context.tblContacts.Add(objcontact);
                    }
                    else
                    {
                        Context.SaveChanges();
                    }
                    objlifeQQ.tblContact = objcontact;
                    int Plan = Convert.ToInt32(objQuote.objProductDetials.Plan);
                    var objProductMaster = Context.tblProducts.Where(a => a.ProductId == Plan).FirstOrDefault();
                    objlifeQQ.RiskCommencementDate = objQuote.RiskCommencementDate;
                    //var objProductMaster = Context.tblProducts.Where(a => a.ProductName == objQuote.objProductDetials.Plan).FirstOrDefault();
                    objlifeQQ.ProductNameID = objProductMaster.ProductId;
                    objlifeQQ.PreferredTerm = objQuote.objProductDetials.PreferredMode;
                    objlifeQQ.PlanCode = objQuote.objProductDetials.PlanCode;
                    objlifeQQ.PlanId = Convert.ToInt32(objQuote.objProductDetials.Variant);
                    objlifeQQ.PreferredLanguage = objQuote.objProductDetials.PreferredLangauage;
                    objlifeQQ.PensionPeriod = Convert.ToInt32(objQuote.objProductDetials.PensionPeriod);
                    objlifeQQ.SelfPay = objQuote.IsSelfPay;
                    objlifeQQ.IsFamilyFloater = objQuote.objProductDetials.IsFamilyFloater;
                    objlifeQQ.Deductable = objQuote.objProductDetials.Deductable;
                    objlifeQQ.DrawDownPeriod = Convert.ToInt32(objQuote.objProductDetials.DrawDownPeriod);
                    objlifeQQ.MaturityBenifits = objQuote.objProductDetials.MaturityBenefits;
                    objlifeQQ.RetirementAge = Convert.ToInt32(objQuote.objProductDetials.RetirementAge);
                    objlifeQQ.MonthlySurvivorIncome = objQuote.objProductDetials.MonthlySurvivorIncome;
                    objlifeQQ.SAM = objQuote.objProductDetials.SAM;
                    objlifeQQ.AnnualizePremium = Convert.ToInt32(objQuote.objProductDetials.AnnualPremium);
                    objlifeQQ.ModalPremium = objQuote.objProductDetials.ModalPremium;
                    if (!string.IsNullOrEmpty(objQuote.objProductDetials.IsAfc))
                    {
                        objlifeQQ.IsAfc = objQuote.objProductDetials.IsAfc;
                    }
                    else
                    {
                        objlifeQQ.IsAfc = "False";
                    }
                    objlifeQQ.NoOfOnGoingProposalWithAIA = objQuote.ObjQuotationPreviousInsurance.NoOfOnGoingProposalWithAIA;
                    objlifeQQ.NoOfPreviousPolicyWithAIA = objQuote.ObjQuotationPreviousInsurance.NoOfPreviousPolicyWithAIA;
                    objlifeQQ.OnGoingProposalWithAIA = Convert.ToString(objQuote.ObjQuotationPreviousInsurance.OnGoingProposalWithAIA);
                    objlifeQQ.PreviousPolicyWithAIA = Convert.ToString(objQuote.ObjQuotationPreviousInsurance.PreviousPolicyWithAIA);

                    if (!string.IsNullOrEmpty(objQuote.NoofChilds))
                    {
                        objlifeQQ.NoOfChild = Convert.ToInt32(objQuote.NoofChilds);
                    }

                    if (objlifeQQ.tblQuoteMemberDetials.Count() > 0)
                    {
                        DeleteExisitigMemberDetails(objlifeQQ.LifeQQID);
                    }
                    int ChildCount = 0;
                    if (objlifeQQ.PreferredTerm != "01")
                    {
                        ControllerLogic.PremiumCalculation.Premium premium = new ControllerLogic.PremiumCalculation.Premium();
                        string PrevPreferredMode = objQuote.objProductDetials.PreferredMode;
                        objQuote.objProductDetials.PreferredMode = "1";
                        objQuote = premium.CalculateQuotePremium(objQuote, true);
                        objQuote.objProductDetials.PreferredMode = PrevPreferredMode;
                    }

                    foreach (var Member in objQuote.objQuoteMemberDetails)
                    {

                        tblQuoteMemberDetial objQuoteMember = new tblQuoteMemberDetial();
                        if (Member.Relationship == "267")
                        {
                            objQuoteMember.CreatedBy = objcontact.CreatedBy;
                            objQuoteMember.Name = objcontact.FirstName;
                            objQuoteMember.Gender = objcontact.Gender;
                            objQuoteMember.OccupationID = objcontact.OccupationID;
                            objQuoteMember.Age = objcontact.Age;
                            objQuoteMember.CurrentAge = objcontact.CurrentAge;
                            objQuoteMember.Relationship = Member.Relationship;
                            objQuoteMember.DateOfBirth = objcontact.DateOfBirth;
                            objQuoteMember.NICNO = objQuote.objProspect.NIC;

                        }
                        else if (Member.Relationship == "268")
                        {
                            if (objQuote.objSpouseDetials != null)
                            {
                                objQuoteMember.Name = objQuote.objSpouseDetials.SpouseName;
                                objQuoteMember.Gender = objQuote.objSpouseDetials.Gender;
                                string[] SplitOccupation = objQuote.objSpouseDetials.Occupation.Split('|');
                                string EngOccupation = SplitOccupation[0];
                                objQuoteMember.OccupationID = Convert.ToInt32(Context.tblMasLifeOccupations.Where(a => a.OccupationCode == EngOccupation).Select(a => a.CompanyCode).FirstOrDefault());
                                //objQuoteMember.OccupationID = /*Convert.ToInt32*/(objQuote.objSpouseDetials.OccupationID);
                               // objQuoteMember.Occupation = objQuote.objSpouseDetials.Occupation;
                                objQuoteMember.Age = objQuote.objSpouseDetials.AgeNextBirthday;
                                objQuoteMember.CurrentAge = objQuote.objSpouseDetials.CurrrentAge;
                                objQuoteMember.Relationship = Member.Relationship;
                                objQuoteMember.DateOfBirth = objQuote.objSpouseDetials.DOB;
                                objQuoteMember.NICNO = objQuote.objSpouseDetials.SpouseNIC;
                            }
                        }
                        else
                        {
                            if (objQuote.objChildDetials != null)
                            {
                                if (objQuote.objChildDetials.Count() > ChildCount)
                                {
                                    var child = objQuote.objChildDetials[ChildCount];
                                    objQuoteMember.Age = child.AgeNextBirthday;
                                    objQuoteMember.CurrentAge = child.CurrentAge;
                                    objQuoteMember.Name = child.Name;
                                    objQuoteMember.DateOfBirth = child.DateofBirth;
                                    objQuoteMember.Gender = child.Gender;
                                    if (objQuoteMember.Gender == "M")
                                    {
                                        objQuoteMember.Relationship = "269";
                                    }
                                    if (objQuoteMember.Gender == "F")
                                    {
                                        objQuoteMember.Relationship = "270";
                                    }

                                    ChildCount++;
                                }

                            }

                        }
                        objQuoteMember.IsDeleted = false;
                        objQuoteMember.BasicSuminsured = Convert.ToInt32(objQuote.objProductDetials.BasicSumInsured);
                        objQuoteMember.BasicPremium = Member.ObjBenefitDetails.Where(a => a.AssuredMember == "6").Select(a => a.RiderPremium).FirstOrDefault();
                        objQuoteMember.AssuredName = Member.Assured;
                        //objQuoteMember.MemberPremium = GetRiderPremium(Member.Relationship, Member.Assured, -2, objQuote.LstBenefitOverView);
                        decimal MemberPremium = decimal.Zero;
                        foreach (var item in Member.ObjBenefitDetails.Where(a => a.BenifitOpted == true).ToList())
                        {

                            tblQuoteMemberBeniftDetial objQuoteBenifit = new tblQuoteMemberBeniftDetial();
                            objQuoteBenifit.SumInsured = item.RiderSuminsured;
                            objQuoteBenifit.Premium = item.RiderPremium;
                            objQuoteBenifit.BenifitID = item.BenefitID;
                            MemberPremium = MemberPremium + Convert.ToDecimal(item.AnnualRiderPremium);
                            objQuoteBenifit.AnnualRiderPremium = Convert.ToDecimal(item.AnnualRiderPremium);
                            objQuoteBenifit.LoadingAmount = Convert.ToDecimal(item.LoadingAmount);
                            objQuoteBenifit.DiscountAmount = Convert.ToDecimal(item.DiscountAmount);
                            objQuoteBenifit.ActualPremium = Convert.ToDecimal(item.ActualRiderPremium);
                            objQuoteBenifit.LoadingPercentage = Convert.ToInt32(Convert.ToDecimal(item.LoadingPercentage));
                            objQuoteBenifit.LoadinPerMille = Convert.ToInt32(Convert.ToDecimal(item.LoadinPerMille));
                            if (objlifeQQ.PreferredTerm == "01")
                            {
                                objQuoteBenifit.AnnualModePremium = Convert.ToDecimal(item.AnnualRiderPremium);
                                objQuoteBenifit.AnnualDiscountAmount = Convert.ToDecimal(item.DiscountAmount);
                                objQuoteBenifit.AnnualLoadingAmount = Convert.ToInt32(Convert.ToDecimal(item.LoadingAmount));
                            }
                            else
                            {
                                objQuoteBenifit.AnnualModePremium = Convert.ToDecimal(item.AnnualModeAnnualpremium);
                                objQuoteBenifit.AnnualDiscountAmount = Convert.ToDecimal(item.AnnualModeDiscountAmount);
                                objQuoteBenifit.AnnualLoadingAmount = Convert.ToInt32(Convert.ToDecimal(item.AnnualModeLoadingAmount));
                            }

                            objQuoteMember.tblQuoteMemberBeniftDetials.Add(objQuoteBenifit);
                        }


                        objQuoteMember.MemberPremium = Convert.ToString(MemberPremium);
                        objlifeQQ.tblQuoteMemberDetials.Add(objQuoteMember);


                    }
                    //foreach (var item in objQuote.objProductDetials.LstTopUpDetails)
                    //{
                    //    if (item.IsDeleted == false)
                    //    {
                    //        tblTopupDetail ObjtblTopupDetail = new tblTopupDetail();
                    //        ObjtblTopupDetail.TopupPolicyYear = item.Topup_PolicyYear;
                    //        ObjtblTopupDetail.Amount = item.Topup_Amount;
                    //        objlifeQQ.tblTopupDetails.Add(ObjtblTopupDetail);
                    //    }



                    //}

                    objlifeQQ.IsActive = true;
                    objlifeQQ.HalfyearlyPremium = objQuote.HalfYearlyPremium;
                    objlifeQQ.QuarterlyPremium = objQuote.QuaterlyPremium;
                    objlifeQQ.AnnualPremium = objQuote.AnnualPremium;
                    objlifeQQ.Monthly = objQuote.MonthlyPremium;
                    objlifeQQ.Vat = objQuote.VAT;
                    objlifeQQ.Cess = objQuote.Cess;
                    objlifeQQ.PolicyFee = objQuote.PolicyFee;
                    //if (objQuote.IsModifyQuote)
                    //{
                    //    objlifeQQ.VersionNo = objQuote.QuoteVersion;
                    //    objlifeQQ.QuoteNo = objQuote.QuoteNo;
                    //    objlifeQQ.StatusID = 1;// Open
                    //    objlifeQQ.CreateDate = DateTime.Now;
                    //    Context.tblLifeQQs.Add(objlifeQQ);
                    //    Context.SaveChanges();
                    //}
                    if (string.IsNullOrEmpty(objQuote.QuoteNo))
                    {
                        objlifeQQ.VersionNo = 1;
                        QuoteNo objQuoteNo = new QuoteNo();

                        objlifeQQ.QuoteNo = objQuoteNo.GenerateQuoteNo("", "01");

                        objlifeQQ.StatusID = 1;// Open
                        objlifeQQ.CreateDate = DateTime.Now;
                        Context.tblLifeQQs.Add(objlifeQQ);
                        Context.SaveChanges();
                    }
                    else
                    {
                        string Result = objQuote.QuoteNo.Substring(0, objQuote.QuoteNo.Length - 2);
                        var VersionNo = (from Quote in Context.tblLifeQQs
                                         where Quote.QuoteNo.Contains(Result)
                                         orderby Quote.VersionNo descending
                                         select Quote.VersionNo
                                         ).FirstOrDefault();

                        int NextVersion = Convert.ToInt32(VersionNo) + 1;
                        string value = NextVersion.ToString("D2");
                        objQuote.QuoteNo = objQuote.QuoteNo.Remove(objQuote.QuoteNo.Length - 2, 2) + value;
                        objQuote.QuoteVersion = NextVersion;
                        objlifeQQ.VersionNo = NextVersion;
                        objlifeQQ.QuoteNo = objQuote.QuoteNo;
                        objlifeQQ.StatusID = 1;// Open
                        objlifeQQ.CreateDate = DateTime.Now;
                        Context.tblLifeQQs.Add(objlifeQQ);
                        Context.SaveChanges();
                    }
                    if (!string.IsNullOrEmpty(objQuote.Signature))
                    {
                        objlifeQQ.ProspectSignature = objQuote.ProspectSign;
                        objlifeQQ.ProposerSignPath = objQuote.ProposerSignPath.Replace("ContactID", objlifeQQ.QuoteNo);
                        objlifeQQ.SignType = "ProposerSign";
                        Context.SaveChanges();

                    }
                    if (!string.IsNullOrEmpty(objQuote.WPProposerSignature))
                    {
                        objlifeQQ.WPSignature = objQuote.WPSignature;
                        objlifeQQ.WPPSignPath = objQuote.WPProposerSignPath.Replace("ContactID", objlifeQQ.QuoteNo); ;
                        objlifeQQ.SignType = "WealthPlannerSign";
                        Context.SaveChanges();
                    }

                    //else
                    //{

                    //    else
                    //    {

                    //        string Result = objQuote.QuoteNo.Substring(0, objQuote.QuoteNo.Length - 2);
                    //        var VersionNo = (from Quote in Context.tblLifeQQs
                    //                         where Quote.QuoteNo.Contains(Result)
                    //                         orderby Quote.VersionNo descending
                    //                         select Quote.VersionNo
                    //                         ).FirstOrDefault();

                    //        int NextVersion = Convert.ToInt32(VersionNo) + 1;
                    //        string value = NextVersion.ToString("D2");
                    //        objQuote.QuoteNo = objQuote.QuoteNo.Remove(objQuote.QuoteNo.Length - 2, 2) + value;
                    //        objQuote.QuoteVersion = NextVersion;
                    //        objlifeQQ.VersionNo = NextVersion;
                    //        objlifeQQ.QuoteNo = objQuote.QuoteNo;
                    //        objlifeQQ.StatusID = 1;// Open
                    //        objlifeQQ.CreateDate = DateTime.Now;
                    //        Context.tblLifeQQs.Add(objlifeQQ);
                    //        Context.SaveChanges();

                    //    }
                    //}

                    #region Counter Offer Case
                    if (objQuote.IsForCounterOffer)
                    {
                        var PolicyInfo = Context.tblPolicies.Where(a => a.QuoteNo == objQuote.PrevQuoteNo).FirstOrDefault();
                        if (PolicyInfo != null)
                        {
                            objQuote.ProposalNo = PolicyInfo.ProposalNo; // Set proposal No For CLA letter Generation
                        }
                        else
                        {
                            // For Second time Save
                            var _PolicyInfo = Context.tblPolicies.Where(a => a.QuoteNo == objQuote.QuoteNo).FirstOrDefault();
                            if (_PolicyInfo != null)
                            {
                                objQuote.ProposalNo = _PolicyInfo.ProposalNo; // Set proposal No For CLA letter Generation
                                objQuote.PrevQuoteNo = _PolicyInfo.QuoteNo;
                            }
                        }
                        
                        CounterOfferChanges(objQuote);
                    }
                    #endregion

                    objQuote.QuoteNo = objlifeQQ.QuoteNo;
                    objQuote.Message = "Success";
                    objQuote.LifeQQID = objlifeQQ.LifeQQID;
                    PremiumCalculation.Illustration illustration = new PremiumCalculation.Illustration();
                    objQuote = illustration.GetIllustration(objQuote);
                    SaveIllustration(objQuote);
                    illustration.GetDrawDownDetails(objQuote);

                    //SamsClient samsClient = new SamsClient();
                    //if (!string.IsNullOrEmpty(objcontact.LeadNo))
                    //{
                    //    samsClient.UpdateLeadStatus(Context, Convert.ToInt32(objcontact.LeadNo), 5);
                    //}
                    //else if (objQuote.QuotationType == "Lead")
                    //{
                    //    samsClient.UpdateLead(Context, objQuote.objProspect);
                    //    objcontact.LeadNo = objQuote.objProspect.SamsLeadNumber;
                    //    samsClient.UpdateLeadStatus(Context, Convert.ToInt32(objcontact.LeadNo), 5);
                    //    Context.SaveChanges();
                    //}
                }
                return objQuote;

            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                objQuote.Message = "Error";
                throw ex;
            }
        }
        public void SendSMS(LifeQuote objQuote)
        {

            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                SMSIntegration objSMSIntegration = new SMSIntegration();
                SMSDetails objSMSDetails = new SMSDetails();
                Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
                var Salu = Context.tblMasCommonTypes.Where(a => a.Description == objQuote.objProspect.Salutation && a.MasterType == "Salutation").Select(a => a.ShortDesc).FirstOrDefault();
                var Salutation = Context.tblMasCommonTypes.Where(a => a.Code == objQuote.objProspect.Salutation && a.MasterType == "Salutation").Select(a => a.ShortDesc).FirstOrDefault();
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
                    objSMSDetails.Salutation = objQuote.objProspect.Salutation; 
                }
                objSMSDetails.Name = objCommonBusiness.ConverttoTitlecase(objQuote.objProspect.LastName);
                objSMSDetails.MobileNumber = objQuote.objProspect.Mobile;
                //objSMSDetails.Name = objQuote.objProspect.Name;
                objSMSDetails.EmailID = objQuote.objProspect.Email;
                objSMSDetails.SMSTemplate = "S002";
                objSMSDetails.WPMobileNumber = Context.tblMasIMOUsers.Where(a => a.UserID == objQuote.UserName).Select(a => a.MobileNo).FirstOrDefault();
                objSMSDetails.Category = "Life Insuarnce Quoatation of" + objQuote.objProspect.Salutation + objQuote.objProspect.LastName + "of the customer - Quoatation number ";
                objSMSDetails.SMSEnvironment = Convert.ToString(ConfigurationManager.AppSettings["SMSEnvironment"]);
                objSMSIntegration.SMSNotification(objSMSDetails);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public LifeQuote SendEmailAndSMSNotificationOnQuoteCreation(LifeQuote objQuote)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                int plan = Convert.ToInt32(objQuote.objProductDetials.Plan);
                EmailIntegration ObjEmailIntegration = new EmailIntegration();
                EmailDetails ObjEmailDetails = new EmailDetails();
                Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
                AIA.Life.Data.API.Controllers.ReportsController objReportController = new AIA.Life.Data.API.Controllers.ReportsController();
                string[] str = objQuote.objProspect.Email.Split(',');
                for (int i = 0; i < str.Length; i++)
                {
                    ObjEmailDetails.EmailID = str[i];

                    //ObjEmailDetails.EmailID = objQuote.objProspect.Email;
                    string userId = Context.tblLifeQQs.Where(a => a.QuoteNo == objQuote.QuoteNo).Select(a => a.Createdby).FirstOrDefault();
                    Guid userGuid = new Guid(userId);
                    objQuote.UserName = Context.tblUserDetails.Where(a => a.UserID == userGuid).Select(a => a.LoginID).FirstOrDefault();
                    if (String.IsNullOrEmpty(objQuote.ProposalNo))
                    {
                        objQuote.ProposalNo = Context.tblPolicies.Where(a => a.QuoteNo == objQuote.QuoteNo).Select(a=>a.ProposalNo).FirstOrDefault();
                    }
                    var WPEmail = Context.tblUserDetails.Where(a => a.UserID == userGuid).Select(a => a.Email).FirstOrDefault();
                    ObjEmailDetails.EmailID = ObjEmailDetails.EmailID;
                    ObjEmailDetails.AgentEmailID = WPEmail;
                    ObjEmailDetails.MailTemplate = "T002";
                    ObjEmailDetails.QuoteNumber = objQuote.QuoteNo;
                    ObjEmailDetails.MobileNumber = objQuote.objProspect.Mobile;
                    ObjEmailDetails.WPMobileNo = Context.tblMasIMOUsers.Where(a => a.UserName == objQuote.UserName).Select(a => a.MobileNo).FirstOrDefault();
                    //ObjEmailDetails.Name = objQuote.objProspect.Name;
                    var Salutation = Context.tblMasCommonTypes.Where(a => a.Code == objQuote.objProspect.Salutation && a.MasterType == "Salutation").Select(a => a.Description).FirstOrDefault();
                    var Salu = Context.tblMasCommonTypes.Where(a => a.Description == objQuote.objProspect.Salutation && a.MasterType == "Salutation").Select(a => a.Description).FirstOrDefault();
                    if (!String.IsNullOrEmpty(Salutation))
                    {
                        ObjEmailDetails.Salutation = Salutation;
                    }
                    else if(!String.IsNullOrEmpty(Salu))
                    {
                        ObjEmailDetails.Salutation = Salu;
                    }
                    else
                    {
                        ObjEmailDetails.Salutation = objQuote.objProspect.Salutation;
                    }
                    ObjEmailDetails.Subject = "Life Insurance Quotation for " + ObjEmailDetails.Salutation + " " + objCommonBusiness.ConverttoTitlecase(objQuote.objProspect.LastName) + " - " + objQuote.QuoteNo;

                    ObjEmailDetails.Name = objCommonBusiness.ConverttoTitlecase(objQuote.objProspect.LastName);
                    //if (objQuote.objProductDetials.PlanCode == "HPA" || objQuote.objProductDetials.PlanCode == "PHP")
                    //{
                        ObjEmailDetails.Premium = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt32(objQuote.AnnualPremium));
                    //}
                    //else
                    //{
                    //    ObjEmailDetails.Premium = objQuote.objProductDetials.AnnualPremium;
                    //}
                    ObjEmailDetails.ProductName = Context.tblProducts.Where(a => a.ProductId == plan).Select(b => b.ProductName).FirstOrDefault();
                    ObjEmailDetails.PolicyTerm = objQuote.objProductDetials.PolicyTerm;
                    ObjEmailDetails.PremiumPayingTerm = objQuote.objProductDetials.PremiumTerm;
                    ObjEmailDetails.Environment = Convert.ToString(ConfigurationManager.AppSettings["Environment"]);
                    ObjEmailDetails.ByteArray = objQuote.ByteArray;
                    if (objQuote.IsForCounterOffer)
                    {
                        ObjEmailDetails.Subject = "Additional Clause for Life Insurance Proposal: " + objQuote.ProposalNo + " " + ObjEmailDetails.Salutation + " " + objCommonBusiness.ConverttoTitlecase(objQuote.objProspect.LastName);
                        ObjEmailDetails.MailTemplate = "T013";
                        ObjEmailDetails.ByteArray2 = objQuote.ByteArray1;// CLA Letter
                        ObjEmailDetails.QuoteNumber = objQuote.QuoteNo;
                    }
                    ObjEmailIntegration.EmailNotification(ObjEmailDetails);
                    SendSMS(objQuote);

                }
                objQuote.Message = "Success";
                return objQuote;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                objQuote.Message = "Error";
                return objQuote;
            }
        }
        public AIA.Life.Models.Opportunity.SMSReminder SMSReminder(AIA.Life.Models.Opportunity.SMSReminder objSMSReminder)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                SMSIntegration objSMSIntegration = new SMSIntegration();
                SMSDetails objSMSDetails = new SMSDetails();
                Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
                var PolicyId = Convert.ToDecimal(objSMSReminder.PolicyID);
                var objPolicy = Context.tblPolicies.Where(a => a.PolicyID == PolicyId).FirstOrDefault();
                var objPolicyMember = Context.tblPolicyMemberDetails.Where(a => a.PolicyID == PolicyId).FirstOrDefault();

                if (objSMSReminder.NoOfDays == 15)
                {
                    //var Salu = Convert.ToInt32(objPolicyMember.Salutation);
                    //var Salutation = Context.tblMasCommonTypes.Where(a => a.CommonTypesID == Salu).Select(a => a.ShortDesc).FirstOrDefault();
                    //objSMSDetails.Salutation = Salutation;
                    var Sal = objPolicyMember.Salutation;
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
                    objSMSDetails.Name = objCommonBusiness.ConverttoTitlecase(objPolicyMember.LastName);
                    objSMSDetails.SMSTemplate = "S011";
                    objSMSDetails.MobileNumber = objPolicyMember.Mobile;
                    objSMSDetails.Category = "Pending Reminder of " + Salutation + "." + objPolicyMember.LastName;
                    objSMSDetails.SMSEnvironment = Convert.ToString(ConfigurationManager.AppSettings["SMSEnvironment"]);
                    objSMSIntegration.SMSNotification(objSMSDetails);
                }
                if (objSMSReminder.NoOfDays == 30)
                {
                    var Date = DateTime.Now.AddDays(15);
                    //var Salu = Convert.ToInt32(objPolicyMember.Salutation);
                    //var Salutation = Context.tblMasCommonTypes.Where(a => a.CommonTypesID == Salu).Select(a => a.ShortDesc).FirstOrDefault();
                    var Sal = objPolicyMember.Salutation;
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
                    //objSMSDetails.Salutation = Salutation;
                    objSMSDetails.Name = objCommonBusiness.ConverttoTitlecase(objPolicyMember.LastName);
                    objSMSDetails.PolicyNo = objPolicy.ProposalNo;
                    objSMSDetails.SMSTemplate = "S014";
                    objSMSDetails.MobileNumber = objPolicyMember.Mobile;
                    objSMSDetails.Category = "Proposal withdrawn reminder of " + objSMSDetails.Salutation + "." + objPolicyMember.LastName;
                    objSMSDetails.SMSEnvironment = Convert.ToString(ConfigurationManager.AppSettings["SMSEnvironment"]);
                    var Dates= Date.ToString("dd/MM/yyyy");
                    string[] LstDate = Dates.Split('-');
                    var Month = Date.ToString("MMM");
                    LstDate[1] = Month;
                    objSMSDetails.PolicyEndDate = String.Join("-", LstDate);
                    //objSMSDetails.PolicyEndDate = Date.ToString("dd/MM/yyyy"); 
                    objSMSIntegration.SMSNotification(objSMSDetails);

                    objSMSDetails.SMSTemplate = "S013";
                    objSMSDetails.PolicyNo = objPolicy.ProposalNo;
                    //objSMSDetails.MobileNumber = objPolicyMember.Mobile;
                    var createdBy = Context.tblPolicies.Where(a => a.ProposalNo == objPolicy.ProposalNo).Select(a => a.Createdby).FirstOrDefault();
                    objSMSDetails.MobileNumber = Context.tblUserDetails.Where(a => a.UserID.ToString() == createdBy).Select(a => a.ContactNo).FirstOrDefault();
                    objSMSDetails.Category = "Proposal withdrawn reminder of " + objSMSDetails.Salutation + "." + objPolicyMember.LastName;
                    objSMSDetails.SMSEnvironment = Convert.ToString(ConfigurationManager.AppSettings["SMSEnvironment"]);
                    //objSMSDetails.PolicyEndDate = Date.ToString("dd/MM/yyyy");
                    objSMSIntegration.SMSNotification(objSMSDetails);

                    EmailIntegration ObjEmailIntegration = new EmailIntegration();
                    EmailDetails ObjEmailDetails = new EmailDetails();
                    ObjEmailDetails.EmailID = objPolicyMember.Email;
                    ObjEmailDetails.AgentEmailID = Context.tblUserDetails.Where(a => a.UserID.ToString() == createdBy).Select(a => a.Email).FirstOrDefault();
                    var ESalutation = Context.tblMasCommonTypes.Where(a => a.Code == objPolicyMember.Salutation && a.MasterType == "Salutation").Select(a => a.Description).FirstOrDefault();
                    var ESalu = Context.tblMasCommonTypes.Where(a => a.Description == objPolicyMember.Salutation && a.MasterType == "Salutation").Select(a => a.Description).FirstOrDefault();
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
                        ObjEmailDetails.Salutation = objPolicyMember.Salutation;
                    }
                    ObjEmailDetails.Subject = "Cancellation of Life Insurance Proposal: "+ objPolicy.ProposalNo+" - " + ObjEmailDetails.Salutation + " " + objCommonBusiness.ConverttoTitlecase(objPolicyMember.LastName);
                    ObjEmailDetails.MailTemplate = "T006";
                    ObjEmailDetails.WPMobileNo = Context.tblUserDetails.Where(a => a.UserID.ToString() == createdBy).Select(a => a.ContactNo).FirstOrDefault();
                    ObjEmailDetails.Name = objCommonBusiness.ConverttoTitlecase(objPolicyMember.LastName);
                    ObjEmailIntegration.EmailNotification(ObjEmailDetails);

                }
                return objSMSReminder;
            }
            catch (Exception ex)
            {
                return objSMSReminder;
            }
        }

        public AIA.Life.Models.Opportunity.Prospect SendEmailAndSMSNotificationOnSAveProspect(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                EmailIntegration ObjEmailIntegration = new EmailIntegration();
                EmailDetails ObjEmailDetails = new EmailDetails();
                Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
                ObjEmailDetails.EmailID = objProspect.Email;
                var CreatedBy = Convert.ToString(objProspect.CreatedBy);
                var WPEmail = Context.tblUserDetails.Where(a =>a.LoginID == CreatedBy).Select(a => a.Email).FirstOrDefault();
                //var Salutation = Context.tblMasCommonTypes.Where(b => b.Code == objProspect.Salutation).Select(b => b.ShortDesc).FirstOrDefault();
                var ESalutation = Context.tblMasCommonTypes.Where(a => a.Code == objProspect.Salutation && a.MasterType == "Salutation").Select(a => a.Description).FirstOrDefault();
                var ESalu = Context.tblMasCommonTypes.Where(a => a.Description == objProspect.Salutation && a.MasterType == "Salutation").Select(a => a.Description).FirstOrDefault();
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
                    ObjEmailDetails.Salutation = objProspect.Salutation;
                }
                ObjEmailDetails.Subject = "Financial Need Analysis for " + ObjEmailDetails.Salutation + " " + objCommonBusiness.ConverttoTitlecase(objProspect.LastName);
                ObjEmailDetails.MailTemplate = "T001";
                ObjEmailDetails.MobileNumber = objProspect.Mobile;
                //ObjEmailDetails.Name = objProspect.Name;
                //ObjEmailDetails.Salutation = objCommonBusiness.ConverttoTitlecase(Salutation);
                ObjEmailDetails.Name = objCommonBusiness.ConverttoTitlecase(objProspect.LastName);
                ObjEmailDetails.PolicyStartDate = DateTime.Now.ToString();
                ObjEmailDetails.WPMobileNo = Context.tblMasIMOUsers.Where(a => a.UserName == objProspect.CreatedBy).Select(a => a.MobileNo).FirstOrDefault();
                ObjEmailDetails.PolicyEndDate = DateTime.Now.ToString();
                ObjEmailDetails.Environment = Convert.ToString(ConfigurationManager.AppSettings["Environment"]);
                ObjEmailDetails.ByteArray = objProspect.ByteArray;
                ObjEmailDetails.ByteArray2 = objProspect.ByteArray1;
                ObjEmailDetails.ByteArray3 = objProspect.ByteArray2;
                ObjEmailDetails.ByteArray4 = objProspect.ByteArray3;
                ObjEmailDetails.ByteArray5 = objProspect.ByteArray4;
                ObjEmailDetails.ByteArray6 = objProspect.ByteArray5;
                ObjEmailDetails.ByteArray8 = objProspect.ByteArrayGraph;
                if (String.IsNullOrEmpty(WPEmail))
                {
                    ObjEmailDetails.EmailID = ObjEmailDetails.EmailID;
                }
                else
                {
                    ObjEmailDetails.EmailID = ObjEmailDetails.EmailID + "," + WPEmail;

                }
                ObjEmailIntegration.EmailNotification(ObjEmailDetails);

                SMSIntegration objSMSIntegration = new SMSIntegration();
                SMSDetails objSMSDetails = new SMSDetails();
                var Salutation = Context.tblMasCommonTypes.Where(a => a.Code == objProspect.Salutation && a.MasterType == "Salutation").Select(a => a.ShortDesc).FirstOrDefault();
                var Salu = Context.tblMasCommonTypes.Where(a => a.Description == objProspect.Salutation && a.MasterType == "Salutation").Select(a => a.ShortDesc).FirstOrDefault();
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
                    objSMSDetails.Salutation = objProspect.Salutation;
                }
                //objSMSDetails.Salutation = objCommonBusiness.ConverttoTitlecase(Salutation);
                objSMSDetails.Name = objCommonBusiness.ConverttoTitlecase(objProspect.LastName);
                objSMSDetails.SMSTemplate = "S001";
                objSMSDetails.MobileNumber = objProspect.Mobile;
                objSMSDetails.WPMobileNumber = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.MobileNo).FirstOrDefault();
                objSMSDetails.Category = "Financial Need Analysis of " + objProspect.Salutation + "." + objProspect.LastName;
                objSMSDetails.SMSEnvironment = Convert.ToString(ConfigurationManager.AppSettings["SMSEnvironment"]);
                objSMSIntegration.SMSNotification(objSMSDetails);
                return objProspect;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                objProspect.Message = "Error";
                return objProspect;
            }
        }
        /// <summary>
        /// Counter offer Changes
        /// </summary>
        /// <param name="lifeQuote"></param>
        public void CounterOfferChanges(LifeQuote lifeQuote)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    var PolicyInfo = Context.tblPolicies.Where(a => a.QuoteNo == lifeQuote.PrevQuoteNo).FirstOrDefault();
                    if (PolicyInfo != null)
                    {
                        tblPolicyCLAQuote policyCLAQuote = new tblPolicyCLAQuote();
                        policyCLAQuote.tblLifeQQ = Context.tblLifeQQs.Where(a => a.QuoteNo == lifeQuote.PrevQuoteNo).FirstOrDefault(); ;
                        policyCLAQuote.Active = false;
                        policyCLAQuote.tblPolicy = PolicyInfo;
                        Context.tblPolicyCLAQuotes.Add(policyCLAQuote);//adding previous versions to cla quotes table for tracking original quote

                        PolicyInfo.QuoteNo = lifeQuote.QuoteNo;// Changing Quote No
                        PolicyInfo.PolicyStageStatusID = CrossCutting.CrossCuttingConstants.PolicyStageStatusReferToUW;// Counter Offer to be directed to UW

                        var QuoteInfo = Context.tblLifeQQs.Where(a => a.QuoteNo == lifeQuote.QuoteNo).FirstOrDefault();
                        if (QuoteInfo != null)
                        {
                            QuoteInfo.StatusID = 2; // Counter offer (should not appear in Quote Pool)

                            #region Update  Product Details
                            PolicyInfo.ProductID = QuoteInfo.ProductNameID;
                            PolicyInfo.PlanID = QuoteInfo.PlanId;
                            PolicyInfo.PaymentFrequency = QuoteInfo.PreferredTerm;
                            PolicyInfo.PolicyTerm = Convert.ToString(QuoteInfo.PolicyTermID);
                            PolicyInfo.PremiumTerm = Convert.ToString(QuoteInfo.PremiumTerm);
                            #endregion

                        }

                        tblPolicyClient policyClient = PolicyInfo.tblPolicyRelationships.FirstOrDefault().tblPolicyClient;
                        policyClient.Age = QuoteInfo.tblContact.Age;

                        DeleteExisitngMembers(PolicyInfo.PolicyID);

                        #region Update Member Details

                        foreach (tblPolicyMemberDetail ExisitingobjMemberDetail in PolicyInfo.tblPolicyMemberDetails.ToList())
                        {
                            #region Fill Member Details
                            tblPolicyMemberDetail objMemberDetail = ExisitingobjMemberDetail;
                            
                            var PrevQuoteMember = Context.tblQuoteMemberDetials.Where(a => a.MemberID == ExisitingobjMemberDetail.QuoteMemberid).FirstOrDefault();
                            if (PrevQuoteMember != null)
                            {
                                var QuoteMember = Context.tblQuoteMemberDetials.Where(a => a.LifeQQID == QuoteInfo.LifeQQID && a.Gender == PrevQuoteMember.Gender && a.Relationship == PrevQuoteMember.Relationship && a.Name == PrevQuoteMember.Name).FirstOrDefault();
                                if (QuoteMember != null)
                                {
                                    objMemberDetail.QuoteMemberid = QuoteMember.MemberID;
                                    objMemberDetail.BasicSuminsured = Convert.ToString(QuoteMember.BasicSuminsured);
                                    objMemberDetail.BasicPremium = QuoteMember.BasicPremium;
                                    objMemberDetail.MemberPremium = QuoteMember.MemberPremium;
                                    objMemberDetail.Assuredname = QuoteMember.AssuredName;
                                    objMemberDetail.Age = QuoteMember.Age;
                                    objMemberDetail.IsDeleted = false;

                                    #region SAR & FAL
                                    if (!string.IsNullOrEmpty(objMemberDetail.NEWNICNO))
                                    {
                                        if (!string.IsNullOrEmpty(QuoteMember.MemberPremium))
                                        {
                                            var SARDetails = Context.SP_GetSARDetails(objMemberDetail.NEWNICNO).FirstOrDefault();
                                            if (SARDetails != null)
                                            {
                                                // Previous Proposal Annual premium
                                                decimal AnnualPrem = Convert.ToDecimal(SARDetails.ANNPREM);
                                                AnnualPrem = AnnualPrem + Convert.ToDecimal(QuoteMember.MemberPremium);
                                                if (AnnualPrem > 250000)
                                                {
                                                    objMemberDetail.AFC = true;
                                                }
                                            }
                                        }
                                    }
                                    var CurrentProposalSAR = Context.SP_GetSARAndFALDetailsForQuote(lifeQuote.QuoteNo).ToList();
                                    if (CurrentProposalSAR != null)
                                    {
                                        int QuoteMemberId = QuoteMember.MemberID;
                                        var MemberInfo = CurrentProposalSAR.Where(a => a.MemberID == QuoteMemberId).FirstOrDefault();
                                        if (MemberInfo != null)
                                        {
                                            objMemberDetail.SAR = Convert.ToDecimal(MemberInfo.SAR);
                                            objMemberDetail.FAL = Convert.ToDecimal(MemberInfo.FAL);
                                        }
                                    }
                                    #endregion

                                    #region Adding Benefit Details
                                    DeleteExisitngMemberRiders(objMemberDetail.MemberID);
                                    foreach (var QuoteBenefit in QuoteMember.tblQuoteMemberBeniftDetials.ToList())
                                    {
                                        tblPolicyMemberBenefitDetail objProposalBenifitExisitng = Context.tblPolicyMemberBenefitDetails.Where(a => a.BenifitID == QuoteBenefit.BenifitID && a.MemberID == ExisitingobjMemberDetail.MemberID).FirstOrDefault();
                                        if (objProposalBenifitExisitng == null)
                                            objProposalBenifitExisitng = new tblPolicyMemberBenefitDetail();
                                        objProposalBenifitExisitng.SumInsured = QuoteBenefit.SumInsured;
                                        objProposalBenifitExisitng.Premium = Convert.ToString(QuoteBenefit.ActualPremium);
                                        objProposalBenifitExisitng.BenifitID = QuoteBenefit.BenifitID;
                                        objProposalBenifitExisitng.AssuredName = QuoteMember.AssuredName;
                                        objProposalBenifitExisitng.RelationShipWithProposer = QuoteMember.Relationship;
                                        objProposalBenifitExisitng.MemberBenifitID = objProposalBenifitExisitng.MemberBenifitID;
                                        objProposalBenifitExisitng.MemberID = objMemberDetail.MemberID;
                                        objProposalBenifitExisitng.IsDeleted = false;
                                        objProposalBenifitExisitng.LoadingAmount = Convert.ToString(QuoteBenefit.LoadingAmount);
                                        objProposalBenifitExisitng.LoadingPerc = QuoteBenefit.LoadingPercentage;
                                        objProposalBenifitExisitng.LoadinPerMille = QuoteBenefit.LoadinPerMille;
                                        objProposalBenifitExisitng.TotalPremium = QuoteBenefit.Premium;
                                        
                                        if(objProposalBenifitExisitng.MemberBenifitID == decimal.Zero)
                                        {
                                            Context.tblPolicyMemberBenefitDetails.Add(objProposalBenifitExisitng);
                                        }
                                        else
                                        {
                                            Context.Entry(objProposalBenifitExisitng).CurrentValues.SetValues(objProposalBenifitExisitng);
                                            #region Delete Loading Info
                                            DeleteRiderOtherDetails(objProposalBenifitExisitng.MemberBenifitID);
                                            #endregion
                                        }

                                        #region Adding other Benefit Details
                                        if (QuoteBenefit.LoadingPercentage > 0)
                                        {
                                            tblMemberBenefitOtherDetail objRiderDetails = new tblMemberBenefitOtherDetail();
                                            objRiderDetails.tblPolicyMemberBenefitDetail = objProposalBenifitExisitng;
                                            objRiderDetails.tblPolicyMemberBenefitDetail1 = objProposalBenifitExisitng;
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
                                            objRiderDetails.tblPolicyMemberBenefitDetail = objProposalBenifitExisitng;
                                            objRiderDetails.tblPolicyMemberBenefitDetail1 = objProposalBenifitExisitng;
                                            objRiderDetails.LoadingType = Context.tblMasCommonTypes.Where(a => a.MasterType == "LoadingType" && a.Description == "Per Milli" && a.isDeleted == 0).Select(a => a.CommonTypesID).FirstOrDefault().ToString();
                                            objRiderDetails.LoadingBasis = Context.tblMasCommonTypes.Where(a => a.MasterType == "LoadingBasis" && a.Code == "OC" && a.isDeleted == 0).Select(a => a.CommonTypesID).FirstOrDefault().ToString();
                                            objRiderDetails.LoadingAmount = Convert.ToString(QuoteBenefit.LoadinPerMille);
                                            objRiderDetails.ExtraPremium = Convert.ToString(QuoteBenefit.LoadingAmount);
                                            objRiderDetails.CreatedDate = DateTime.Now;
                                            Context.tblMemberBenefitOtherDetails.Add(objRiderDetails);
                                        }

                                        #endregion
                                    }
                                    #endregion
                                }
                                else { objMemberDetail.IsDeleted = true; }
                            }
                            else
                            {
                                objMemberDetail.IsDeleted = true;
                            }

                            #endregion
                        }
                        #endregion

                        var ProposalPremium = PolicyInfo.tblProposalPremiums.FirstOrDefault();
                        if (ProposalPremium != null)
                        {
                            decimal? ExtraPremium = Convert.ToDecimal(QuoteInfo.AnnualPremium) - (ProposalPremium.AnnualPremium);
                            ProposalPremium.AdditionalPremium = ExtraPremium;
                        }
                        Context.SaveChanges();

                    }
                }
            }
            catch (Exception ex)
            {

                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);

            }
        }

        public void DeleteRiderOtherDetails(decimal MemberBenefitID)
        {
            try
            {
                using (AVOAIALifeEntities SubContext = new AVOAIALifeEntities())
                {
                    var List = SubContext.tblMemberBenefitOtherDetails.Where(a => a.MemberBenifitID == MemberBenefitID).ToList();
                    if (List != null)
                    {
                        SubContext.tblMemberBenefitOtherDetails.RemoveRange(List);
                        SubContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Delete Exisitng Member Details
        /// </summary>
        /// <param name="MemberID"></param>
        public void DeleteExisitngMemberRiders(decimal MemberID)
        {
            using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
            {
                foreach (var MemberRider in Context.tblPolicyMemberBenefitDetails.Where(a => a.MemberID == MemberID).ToList())
                {
                    MemberRider.IsDeleted = true;
                    Context.SaveChanges();
                }
            }
        }
        /// <summary>
        /// Delete Exisitng Member Details
        /// </summary>
        /// <param name="MemberID"></param>
        public void DeleteExisitngMembers(decimal PolicyID)
        {
            using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
            {
                foreach (var member in Context.tblPolicyMemberDetails.Where(a => a.PolicyID == PolicyID).ToList())
                {
                    member.IsDeleted = true;
                    Context.SaveChanges();
                }
            }
        }
        public void SaveIllustration(LifeQuote lifeQuote)
        {
            using (AVOAIALifeEntities entity = new AVOAIALifeEntities())
            {
                foreach (var item in entity.tblQuoteIllustrations.Where(a => a.QuoteNo == lifeQuote.QuoteNo).ToList())
                {
                    entity.Entry(item).State = EntityState.Deleted;
                }
                foreach (var item in lifeQuote.LstIllustation)
                {
                    tblQuoteIllustration quoteIllustration = new tblQuoteIllustration();
                    quoteIllustration.PolicyYear = item.PolicyYear;
                    quoteIllustration.BasicPremium = item.BasicPremium;
                    quoteIllustration.MainBenefitsPremium = item.MainBenefitsPremium;
                    quoteIllustration.AdditionalBenefitsPremiums = item.AdditionalBenefitsPremiums;
                    quoteIllustration.TotalPremium = item.TotalPremium;
                    quoteIllustration.InvestmentACBalance = item.FundBalanceDiv4;
                    quoteIllustration.SurrenderValue4 = item.SurrenderValueDiv4;
                    quoteIllustration.MonthlyDrawDown4 = item.DrawDownDiv4;
                    quoteIllustration.InvestmentACBalance8 = item.FundBalanceDiv8;
                    quoteIllustration.SurrenderValue8 = item.SurrenderValueDiv8;
                    quoteIllustration.MonthlyDrawDown8 = item.DrawDownDiv8;
                    quoteIllustration.InvestmentACBalance12 = item.FundBalanceDiv12;
                    quoteIllustration.SurrenderValue12 = item.SurrenderValueDiv12;
                    quoteIllustration.MonthlyDrawDown12 = item.DrawDownDiv12;
                    quoteIllustration.PensionBoosterDiv4 = item.PensionBoosterDiv4;
                    quoteIllustration.PensionBoosterDiv8 = item.PensionBoosterDiv8;
                    quoteIllustration.PensionBoosterDiv12 = item.PensionBoosterDiv12;
                    quoteIllustration.InvestmentACBalance5 = item.FundBalanceDiv5;
                    quoteIllustration.InvestmentACBalance6 = item.FundBalanceDiv6;
                    quoteIllustration.InvestmentACBalance7 = item.FundBalanceDiv7;
                    quoteIllustration.InvestmentACBalance9 = item.FundBalanceDiv9;
                    quoteIllustration.InvestmentACBalance10 = item.FundBalanceDiv10;
                    quoteIllustration.InvestmentACBalance11 = item.FundBalanceDiv11;
                    quoteIllustration.MonthlyDrawDown5 = item.DrawDownDiv5;
                    quoteIllustration.MonthlyDrawDown6 = item.DrawDownDiv6;
                    quoteIllustration.MonthlyDrawDown7 = item.DrawDownDiv7;
                    quoteIllustration.MonthlyDrawDown9 = item.DrawDownDiv9;
                    quoteIllustration.MonthlyDrawDown10 = item.DrawDownDiv10;
                    quoteIllustration.MonthlyDrawDown11 = item.DrawDownDiv11;
                    quoteIllustration.UnAllocatedPremium = item.UnAllocatedPremium;

                    quoteIllustration.QuoteNo = lifeQuote.QuoteNo;
                    entity.tblQuoteIllustrations.Add(quoteIllustration);
                }

                entity.SaveChanges();
            }
        }
        public void DeleteExisitigMemberDetails(int LifeQQID)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                foreach (var Member in Context.tblQuoteMemberDetials.Where(a => a.LifeQQID == LifeQQID).ToList())
                {
                    Member.IsDeleted = true;
                    Context.SaveChanges();
                }


            }
            catch (Exception ex)
            {

                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
            }
        }
        public string GetRiderPremium(string Relationship, string Assured_Name, int RiderID, List<BenifitDetails> LstPremiumOverView)
        {
            try
            {
                var RiderDetails = LstPremiumOverView.Where(a => a.BenefitID == RiderID).FirstOrDefault();
                if (RiderDetails != null)
                {
                    var RiderInfo = RiderDetails.objBenefitMemberRelationship.Where(a => a.Member_Relationship == Relationship && a.Assured_Name == Assured_Name).FirstOrDefault();

                    if (RiderInfo != null)
                    {
                        return RiderInfo.Rider_Premium;
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {

                return string.Empty;

            }

        }
        public AIA.Life.Models.Opportunity.LifeQuote FetchQuoteInfo(AIA.Life.Models.Opportunity.LifeQuote objQuote)
        {
            try
            {

                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                tblLifeQQ objlifeQQ = Context.tblLifeQQs.Where(a => a.QuoteNo == objQuote.QuoteNo).FirstOrDefault();

                if (objlifeQQ != null)
                {
                    objQuote.objProductDetials = new ProductDetials();
                    objQuote.objSpouseDetials = new SpouseDetails();
                    objQuote.objChildDetials = new List<ChildDetails>();
                    objQuote.objQuoteMemberDetails = new List<QuoteMemberDetails>();
                    objQuote.objProductDetials.PolicyTerm = Convert.ToString(objlifeQQ.PolicyTermID);
                    objQuote.objProductDetials.PremiumTerm = Convert.ToString(objlifeQQ.PremiumTerm);
                    objQuote.Contactid = objlifeQQ.ContactID;
                    var objProductMaster = Context.tblProducts.Where(a => a.ProductId == objlifeQQ.ProductNameID).FirstOrDefault();
                    objQuote.objProductDetials.Plan = objlifeQQ.ProductNameID.ToString();
                    objQuote.objProductDetials.PlanCode = objProductMaster.ProductCode;
                    objQuote.objProductDetials.Variant = objlifeQQ.PlanId.ToString();
                    objQuote.objProductDetials.PreferredMode = objlifeQQ.PreferredTerm;
                    objQuote.IsSelfPay = objlifeQQ.SelfPay ?? false;
                    objQuote.objProductDetials.IsFamilyFloater = objlifeQQ.IsFamilyFloater ?? false;
                    objQuote.objProductDetials.Deductable = objlifeQQ.Deductable ?? false;
                    objQuote.objProductDetials.PreferredLangauage = objlifeQQ.PreferredLanguage;
                    objQuote.objProductDetials.PensionPeriod = Convert.ToString(objlifeQQ.PensionPeriod);
                    objQuote.objProductDetials.DrawDownPeriod = Convert.ToString(objlifeQQ.DrawDownPeriod);
                    objQuote.objProductDetials.MaturityBenefits = objlifeQQ.MaturityBenifits;
                    objQuote.objProductDetials.RetirementAge = Convert.ToString(objlifeQQ.RetirementAge);
                    objQuote.objProductDetials.MonthlySurvivorIncome = objlifeQQ.MonthlySurvivorIncome ?? 0;
                    objQuote.objProductDetials.SAM = objlifeQQ.SAM ?? 0;
                    objQuote.objProductDetials.AnnualPremium = Convert.ToString(objlifeQQ.AnnualizePremium);
                    objQuote.objProductDetials.ModalPremium = objlifeQQ.ModalPremium;
                    if (!string.IsNullOrEmpty(objlifeQQ.OnGoingProposalWithAIA))
                    {
                        objQuote.ObjQuotationPreviousInsurance.OnGoingProposalWithAIA = Convert.ToInt32(objlifeQQ.OnGoingProposalWithAIA);
                        objQuote.ObjQuotationPreviousInsurance.NoOfOnGoingProposalWithAIA = objlifeQQ.NoOfOnGoingProposalWithAIA;

                    }
                    if (!string.IsNullOrEmpty(objlifeQQ.PreviousPolicyWithAIA))
                    {
                        objQuote.ObjQuotationPreviousInsurance.PreviousPolicyWithAIA = Convert.ToInt32(objlifeQQ.PreviousPolicyWithAIA);
                        objQuote.ObjQuotationPreviousInsurance.NoOfPreviousPolicyWithAIA = objlifeQQ.NoOfPreviousPolicyWithAIA;
                    }
                    objQuote.Contactid = objlifeQQ.ContactID;

                    objQuote.ListAssured = new List<string>();
                    if (Context.tblQuoteMemberDetials.Where(a => a.LifeQQID == objlifeQQ.LifeQQID && a.Relationship == "267" && a.IsDeleted != true).Count() > 0)
                    {
                        objQuote.IsSelfCovered = true;
                    }
                    var ListMemberDetails = Context.tblQuoteMemberDetials.Where(a => a.LifeQQID == objlifeQQ.LifeQQID && a.IsDeleted != true).ToList();
                    int Count = 0;
                    //var ListTopUp = Context.tblTopupDetails.Where(a => a.LifeQQID == objlifeQQ.LifeQQID).ToList();
                    //foreach (var item in ListTopUp)
                    //{
                    //    TopUpDetails ObjTopUpDetails = new TopUpDetails();
                    //    ObjTopUpDetails.Topup_PolicyYear = item.TopupPolicyYear;
                    //    ObjTopUpDetails.Topup_Amount = item.Amount;
                    //    objQuote.objProductDetials.LstTopUpDetails.Add(ObjTopUpDetails);
                    //}
                    LifeAssuredAge objLifeAssuedAge = new LifeAssuredAge();
                    objLifeAssuedAge.QuoteNo = objlifeQQ.QuoteNo;
                    objLifeAssuedAge.Rcd = objQuote.RiskCommencementDate;
                    CommonBusiness objcommon = new CommonBusiness();
                    bool AgeChange = false;
                    objLifeAssuedAge = objcommon.CheckAgeChangeQuoteMembers(objLifeAssuedAge);
                    if(objLifeAssuedAge.MainLifeAge==true ||objLifeAssuedAge.SpouseAge==true||objLifeAssuedAge.Child1Age==true||
                        objLifeAssuedAge.Child2Age==true||objLifeAssuedAge.Child3Age==true||objLifeAssuedAge.Child4Age==true||objLifeAssuedAge.Child5Age==true)
                    {
                        AgeChange = true;
                    }
                    #region Fetch Member Details
                    foreach (var item in ListMemberDetails)
                    {
                        QuoteMemberDetails objMemberDetails = new QuoteMemberDetails();
                        if (item.Relationship == "267" || item.Relationship == "Prospect")
                        {
                            objMemberDetails.Assured = "MainLife";
                            objQuote.ListAssured.Add("MainLife");
                            objQuote.IsSelfCovered = true;
                        }
                        else if (item.Relationship == "268" || item.Relationship == "Spouse")
                        {
                            objQuote.IsSpouseCovered = true;
                            if (!objQuote.IsSelfCovered)
                            {
                                objQuote.ListAssured.Add("MainLife");
                                objMemberDetails.Assured = "MainLife";
                            }
                            else
                            {
                                objQuote.ListAssured.Add("Spouse");
                                objMemberDetails.Assured = "Spouse";
                            }
                            objQuote.objSpouseDetials.Gender = item.Gender;
                            int? OccupationId = item.OccupationID;
                            objQuote.objSpouseDetials.Occupation = Context.tblMasLifeOccupations.Where(a => a.CompanyCode == OccupationId.ToString()).Select(a => a.OccupationCode + "|" + a.SinhalaDesc + "|" + a.TamilDesc).FirstOrDefault();
                            objQuote.objSpouseDetials.AgeNextBirthday = Convert.ToInt32(item.Age);
                            CommonBusiness objCommon = new CommonBusiness();
                            int age = objCommon.GetCurrentAge(item.DateOfBirth,objQuote.RiskCommencementDate);
                            objQuote.objSpouseDetials.AgeNextBirthday = age;
                           objMemberDetails.DateOfBirth = item.DateOfBirth;
                            objQuote.objSpouseDetials.SpouseName = item.Name;
                            objQuote.objSpouseDetials.SpouseNIC = item.NICNO;
                            objQuote.objSpouseDetials.DOB = item.DateOfBirth;

                        }
                        else
                        {
                            objQuote.IsChildCovered = true;
                            Count++;

                            string AssuredName = "Child" + (Count);
                            objMemberDetails.Assured = AssuredName;

                            objQuote.ListAssured.Add(AssuredName);
                            ChildDetails objchild = new ChildDetails();
                            objchild.AgeNextBirthday = Convert.ToInt32(item.Age);
                            CommonBusiness objCommon = new CommonBusiness();
                            int age = objCommon.GetCurrentAge(item.DateOfBirth, objQuote.RiskCommencementDate);
                            objchild.AgeNextBirthday = age;
                            objMemberDetails.DateOfBirth = item.DateOfBirth;
                            objchild.Gender = item.Gender;
                            objchild.Name = item.Name;
                            objchild.DateofBirth = item.DateOfBirth;
                            objchild.Relationship = item.Relationship;
                            objQuote.objChildDetials.Add(objchild);
                            objQuote.NoofChilds = Convert.ToString(Count);
                        }
                        CommonBusiness objCommonBusines = new CommonBusiness();
                        int Age = objCommonBusines.GetCurrentAge(item.DateOfBirth, objQuote.RiskCommencementDate);
                        objMemberDetails.AgeNextBirthDay = Convert.ToInt32(Age);
                        objMemberDetails.DateOfBirth = item.DateOfBirth;
                        objMemberDetails.CurrentAge = Convert.ToInt32(item.CurrentAge);
                        objMemberDetails.Relationship = item.Relationship;
                        objQuote.objProductDetials.BasicSumInsured = Convert.ToInt32(item.BasicSuminsured);
                        objMemberDetails.ObjBenefitDetails.AddRange(LoadMasBenifits(Convert.ToInt32(objQuote.objProductDetials.Variant), objMemberDetails, AgeChange,objQuote));

                        foreach (var benifit in item.tblQuoteMemberBeniftDetials)
                        {

                            int index = objMemberDetails.ObjBenefitDetails.FindIndex(a => a.BenefitID == benifit.BenifitID);
                            if (index >= 0)
                            {
                                objMemberDetails.ObjBenefitDetails[index].RiderSuminsured = benifit.SumInsured;
                                objMemberDetails.ObjBenefitDetails[index].RiderPremium = (AgeChange==true?"0": benifit.Premium);
                                objMemberDetails.ObjBenefitDetails[index].MemberBenifitID = benifit.MemberBenifitID;
                                objMemberDetails.ObjBenefitDetails[index].ActualRiderPremium = (AgeChange == true ? "0" : benifit.ActualPremium.ToString());
                                objMemberDetails.ObjBenefitDetails[index].AnnualRiderPremium = (AgeChange == true ? "0" : benifit.AnnualRiderPremium.ToString());
                                objMemberDetails.ObjBenefitDetails[index].DiscountAmount = benifit.DiscountAmount.ToString();
                                objMemberDetails.ObjBenefitDetails[index].LoadingAmount = benifit.LoadingAmount.ToString();
                                objMemberDetails.ObjBenefitDetails[index].BenifitOpted = true;
                            }
                        }
                        objQuote.objQuoteMemberDetails.Add(objMemberDetails);

                    }
                    List<int> toBeRemoved = new List<int>();
                    List<BenifitDetails> removeList = new List<BenifitDetails>();
                    for (int MemberIndex = 0; MemberIndex < objQuote.objQuoteMemberDetails.Count; MemberIndex++)

                    {
                        for (int i = 0; i < objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails.Count; i++)
                        {
                            if (objQuote.objProductDetials.IsFamilyFloater == true)
                            {

                                if (Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 28 ||
                                    Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 35
                                    || Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 29
                                    || Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 36
                                    || Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 37
                                    || Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 17
                                    || Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 18
                                    || Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 19)
                                {
                                    toBeRemoved.Add(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID);
                                }

                            }
                            else
                            {
                                if (Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 87
                                 || Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 20
                                 || Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 93
                                 || Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 94
                                 || Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 95
                                 || Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 96)

                                {
                                    toBeRemoved.Add(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID);
                                }
                            }
                            if (objQuote.objProspect.Gender == "M")
                            {
                                if (Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 22)

                                {
                                    toBeRemoved.Add(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID);
                                }
                            }
                            if (objQuote.objSpouseDetials.Gender == "M")
                            {
                                if (Convert.ToInt32(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID) == 92)

                                {
                                    toBeRemoved.Add(objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails[i].BenefitID);
                                }
                            }
                            if (toBeRemoved.Count > 0)
                            {

                                removeList = objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails.Where(a => toBeRemoved.Contains(a.BenefitID)).ToList();


                            }
                        }
                        foreach (var item in removeList)
                        {
                            objQuote.objQuoteMemberDetails[MemberIndex].ObjBenefitDetails.Remove(item);
                        }
                    }
                    #endregion

                    objQuote.QuoteIndex = 0;

                    objQuote.QuotationType = objlifeQQ.QType;
                    objQuote.AnnualPremium = (AgeChange == true ? "0" : objlifeQQ.AnnualPremium);
                    objQuote.QuaterlyPremium = objlifeQQ.QuarterlyPremium;
                    objQuote.HalfYearlyPremium = objlifeQQ.HalfyearlyPremium;
                    objQuote.MonthlyPremium = objlifeQQ.Monthly;
                    objQuote.PolicyFee = objlifeQQ.PolicyFee;
                    objQuote.VAT = objlifeQQ.Vat;
                    objQuote.Cess = objlifeQQ.Cess;
                    if (objlifeQQ.VersionNo > 0)
                    {
                        objQuote.QuoteVersion = objlifeQQ.VersionNo + 1;
                    }
                    objQuote.LstBenefitOverView = new List<BenifitDetails>();

                    if (objQuote.IsModifyQuote)
                    {
                        objQuote.PrevQuoteNo = objlifeQQ.QuoteNo;
                        #region Generate string For Next Version
                        string Result = objlifeQQ.QuoteNo.Substring(0, objlifeQQ.QuoteNo.Length - 2);
                        var VersionNo = (from Quote in Context.tblLifeQQs
                                         where Quote.QuoteNo.Contains(Result)
                                         orderby Quote.VersionNo descending
                                         select Quote.VersionNo
                                         ).FirstOrDefault();

                        int NextVersion = Convert.ToInt32(VersionNo) + 1;
                        string value = NextVersion.ToString("D2");
                        string _QuoteNo = objlifeQQ.QuoteNo;
                        objQuote.QuoteNo = _QuoteNo.Remove(_QuoteNo.Length - 2, 2) + value;
                        objQuote.QuoteVersion = NextVersion;
                        #endregion

                    }
                }

                #region Loading Master For Quote Page
                Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
                ProductMasters obj = new ProductMasters();
                obj.ProductID = Convert.ToInt32(objlifeQQ.ProductNameID);
                if (objQuote.IsForServices != true)
                {
                    obj = objCommonBusiness.LoadProductMasters(obj);
                    objQuote.ListPlan = objCommonBusiness.ListProducts();
                    objQuote.lstGender = objCommonBusiness.GetGender();
                    objQuote.lstOccupation = objCommonBusiness.GetOccupation();
                    objQuote.lstLanguage = objCommonBusiness.GetMasCommonTypeMasterListItem("Language");
                    objQuote.lstPrefMode = objCommonBusiness.GetPreferredModes(string.Empty);
                }

                #endregion

                #region Loading Prospect And Need Analysis Info
                ProspectLogic objProspectLogic = new ProspectLogic();
                AIA.Life.Models.Opportunity.Prospect objProspect = new AIA.Life.Models.Opportunity.Prospect();
                objProspect.IsForServices = objQuote.IsForServices;
                objProspect.ContactID = objQuote.Contactid;
                objProspect.CreatedBy = objQuote.UserName;
                objQuote.objProspect = objProspectLogic.LoadContactInformation(objProspect,objQuote);
                #endregion

                //#region Fetch Sinatures 

                //var ProposerSignature = Context.tblLifeQQs.Where(a => a.QuoteNo == objQuote.QuoteNo).FirstOrDefault();
                //if (ProposerSignature != null)
                //{
                //    objQuote.ProposerSignPath = ProposerSignature.ProposerSignPath;
                //    objQuote.ProspectSign = ProposerSignature.ProspectSignature;
                //}

                //var WPSignature = Context.tblLifeQQs.Where(a => a.QuoteNo == objQuote.QuoteNo).FirstOrDefault();
                //if (WPSignature != null)
                //{
                //    objQuote.WPProposerSignPath = WPSignature.WPPSignPath;
                //    objQuote.WPSignature = WPSignature.WPSignature;
                //}
                //#endregion


                return objQuote;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                return objQuote;
            }

        }

        public AIA.Life.Models.Opportunity.LifeQuote LoadPreviousInsuranceGrid(AIA.Life.Models.Opportunity.LifeQuote ObjLifeQuote)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            var LstDetails = Context.usp_GetPreviousPolicyDetails(ObjLifeQuote.objProspect.NIC).ToList();
            List<PreviousInsuranceList> lstPreviousInsuranceList = new List<PreviousInsuranceList>();
            List<PreviousInsuranceList> lstPreviousInsuranceList1 = new List<PreviousInsuranceList>();
            List<PreviousInsuranceList> lstPreviousInsuranceList2 = new List<PreviousInsuranceList>();
            List<PreviousInsuranceList> lstPreviousInsuranceList3 = new List<PreviousInsuranceList>();
            lstPreviousInsuranceList = LstDetails.Select(a => new PreviousInsuranceList()
            {
                AnnualPremium = Convert.ToString(a.POlPREM),
                PolicyNumber = a.POLICYNO,
                NameOfTheComp = "AIA",
                SumAssured = Convert.ToString(a.SumInsured),
                Deathbenifit = Convert.ToString(a.ADB),
                IllNessBenifit = Convert.ToString(a.CI),
                PermanentDisability = Convert.ToString(a.WOB),
                HospitalizationPerDay = Convert.ToString(a.HDB),
                status = a.Longdesc
            }).ToList();

            var LstDetails1 = Context.usp_GetOngoingProposalDetails(ObjLifeQuote.objProspect.NIC).ToList();
            lstPreviousInsuranceList1 = LstDetails1.Select(a => new PreviousInsuranceList()
            {
                AnnualPremium = Convert.ToString(a.POlPREM),
                PolicyNumber = a.POLICYNO,
                NameOfTheComp = "AIA",
                SumAssured = Convert.ToString(a.SumInsured),
                Deathbenifit = Convert.ToString(a.ADB),
                IllNessBenifit = Convert.ToString(a.CI),
                PermanentDisability = Convert.ToString(a.WOB),
                HospitalizationPerDay = Convert.ToString(a.HDB),
                status = a.LongDesc
            }).ToList();

            lstPreviousInsuranceList.AddRange(lstPreviousInsuranceList1);
            lstPreviousInsuranceList2 = lstPreviousInsuranceList;
            lstPreviousInsuranceList3 = lstPreviousInsuranceList2.Select(a => new PreviousInsuranceList()
            {

                PolicyNumber = "",
                NameOfTheComp = "Total",
                AnnualPremium = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64(lstPreviousInsuranceList2.Sum(item => Convert.ToInt64(item.AnnualPremium)))),
                SumAssured = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64(lstPreviousInsuranceList2.Sum(item => Convert.ToInt64(item.SumAssured)))),
                Deathbenifit = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64(lstPreviousInsuranceList2.Sum(item => Convert.ToInt64(item.Deathbenifit)))),
                IllNessBenifit = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64(lstPreviousInsuranceList2.Sum(item => Convert.ToInt64(item.IllNessBenifit)))),
                PermanentDisability = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64(lstPreviousInsuranceList2.Sum(item => Convert.ToInt64(item.PermanentDisability)))),
                HospitalizationPerDay = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64(lstPreviousInsuranceList2.Sum(item => Convert.ToInt64(item.HospitalizationPerDay)))),
                status = ""
            }).Take(1).ToList();

            for (int i = 0; i < lstPreviousInsuranceList2.Count(); i++)
            {
                lstPreviousInsuranceList2[i].AnnualPremium = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64(lstPreviousInsuranceList2[i].AnnualPremium));
                lstPreviousInsuranceList2[i].SumAssured = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64(lstPreviousInsuranceList2[i].SumAssured));
                lstPreviousInsuranceList2[i].Deathbenifit = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64(lstPreviousInsuranceList2[i].Deathbenifit));
                lstPreviousInsuranceList2[i].IllNessBenifit = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64(lstPreviousInsuranceList2[i].IllNessBenifit));
                lstPreviousInsuranceList2[i].PermanentDisability = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64(lstPreviousInsuranceList2[i].PermanentDisability));
                lstPreviousInsuranceList2[i].HospitalizationPerDay = string.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", Convert.ToInt64(lstPreviousInsuranceList2[i].HospitalizationPerDay));

            }
            ObjLifeQuote.objPreviousInsuranceList.AddRange(lstPreviousInsuranceList2);
            ObjLifeQuote.objPreviousInsuranceList.AddRange(lstPreviousInsuranceList3);

            return ObjLifeQuote;
        }
   

    }
}
