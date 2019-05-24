using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIA.Life.Models.Common;
using AIA.Life.Repository.AIAEntity;
using AIA.Life.Models.Policy;
using System.Data.SqlClient;
using AIA.Life.Models.NeedAnalysis;
using AIA.CrossCutting;
using log4net;
using System.Data.Entity.Core.Objects;
using System.Data;

namespace AIA.Data.Life.API.ControllerLogic.Common
{
    public class CommonBusiness
    {
        AVOAIALifeEntities Context = new AVOAIALifeEntities();
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);

        public static string GetUserId(string UserName)
        {
            using (AVOAIALifeEntities context = new AVOAIALifeEntities())
                return context.AspNetUsers.Where(a => a.UserName == UserName).FirstOrDefault().Id;
        }

        public tblMasCommonType GetMasCommonTypeList(string MasterType, string ShortDesc)
        {
            return Context.tblMasCommonTypes.Where(a => a.MasterType == MasterType && a.ShortDesc == ShortDesc && a.isDeleted == 0).FirstOrDefault();

        }

        public IEnumerable<tblMasCommonType> GetMasCommonTypeList(string MasterType, int? CommonTypeId = null, List<int?> lstCommonTypeId = null)
        {

            IEnumerable<tblMasCommonType> records;
            if (lstCommonTypeId == null)
                records = Context.tblMasCommonTypes.Where(a => a.isDeleted != 1 &&
                                                ((MasterType != null && a.MasterType == MasterType) ||
                                                (CommonTypeId != null && a.CommonTypesID == CommonTypeId)
                                                )).AsEnumerable();
            else
                records = Context.tblMasCommonTypes.Where(a => a.isDeleted == 0 &&
                                                    ((MasterType != null && a.MasterType == MasterType) ||
                                                    (CommonTypeId != null && a.CommonTypesID == CommonTypeId) ||
                                                    (lstCommonTypeId.Contains(a.CommonTypesID))
                                                    )).AsEnumerable();

            return records;
        }



        //public List<MasterListItem> GetBranchNameList(string UserName)
        //{
        //    Guid UserId = GetUserId(UserName);
        //    decimal NodeId = GetUserNodeId(UserId);
        //    List<decimal> LstBranchids = GetFGBranchCodeList(NodeId);
        //    List<tblMasBranch> lst = GetBranchList(LstBranchids).ToList();
        //    return lst.Select(o => new MasterListItem
        //    {
        //        ID = Convert.ToInt32(o.BranchId),
        //        Value = o.ShortDesc
        //    }).ToList();

        //}
        public decimal GetUserNodeId(Guid UserId)
        {
            return Context.tblUserDetails.Where(a => a.UserID == UserId).FirstOrDefault().NodeID;
        }
        //public List<decimal> GetFGBranchCodeList(decimal NodeId)
        //{
        //    return Context.tblUserBranchMappings.Where(a => a.UserDetailsId == NodeId).Select(a => new
        //    {
        //        Value = a.FGBranchCode
        //    }).ToList().Select(o => Convert.ToDecimal(o.Value)
        //                      ).ToList();



        //}
        public IEnumerable<tblMasBranch> GetBranchList(List<decimal> LstBranchids = null)
        {
            IEnumerable<tblMasBranch> records;
            if (LstBranchids == null)
                records = Context.tblMasBranches.AsEnumerable();
            else
                records = Context.tblMasBranches.Where(a => LstBranchids.Contains(a.BranchId)).AsEnumerable();
            return records;
        }
        public IEnumerable<tblMasCommonType> GetRelationship(string MasterType)
        {
            return Context.tblMasCommonTypes.Where(a => a.MasterType == MasterType && a.isDeleted == 0);

        }
        //JanaShankthi
        //public IEnumerable<tblMasCommonType> GetMotorVehicleType(string MasterType)
        //{
        //    return Context.tblMasCommonTypes.Where(a => a.MasterType == MasterType && a.isDeleted == 0);
        //}

        public List<MasterListItem> GetMasCommonTypeMasterListItem(string MasterType, int? CommonTypeId = null, List<int?> lstCommonTypeId = null)
        {

            return GetMasCommonTypeList(MasterType, CommonTypeId, lstCommonTypeId).Select(a => new MasterListItem { ID = a.CommonTypesID, Value = a.Description }).ToList();
        }

        public List<MasterListItem> GetMasLookUpTypeMasterListItem(string LookUpType, int? ID = null, List<int?> lstCommonTypeId = null)
        {

            return GetMasLookupTypeList(LookUpType, ID, lstCommonTypeId).Select(a => new MasterListItem { ID = a.ID, Value = a.DESCRIPTION }).ToList();
        }

        public IEnumerable<tblmasDoctor> GetMasLookupTypeList(string LookUpType, int? ID = null, List<int?> lstCommonTypeId = null)
        {
            bool value = Convert.ToBoolean(0);
            bool value1 = Convert.ToBoolean(1);

            IEnumerable<tblmasDoctor> records;

            if (lstCommonTypeId == null)
                records = Context.tblmasDoctors.Where(a => a.IsDeleted != value1 && a.LookUpType == LookUpType
                                                ).AsEnumerable();
            else
                records = Context.tblmasDoctors.Where(a => a.IsDeleted == value && a.LookUpType == LookUpType
                                                    || (lstCommonTypeId.Contains(a.ID))
                                                    ).AsEnumerable();

            return records;
        }

        public List<MasterListItem> GetMasLookUpTypeLabsListItem()
        {

            return Context.tblMasMedicalLabs.Select(a => new MasterListItem { Value = a.ID_PK.ToString(), Text = a.MedicalLabName }).ToList();
        }

        public List<MasterListItem> GetMasLookUpTypeReason(string Decision)
        {
            List<MasterListItem> Reasons = Context.tblUWReasons.Where(a => a.Reason == "Decline").Select(b => new MasterListItem
            {
                Value = b.ItemCode,
                Text = b.LongDescription
            }).OrderBy(b => b.Text).ToList();
            return Reasons;
        }

        public List<MasterListItem> GetMasLookUpTypeMedicalCodes()
        {
            List<MasterListItem> Reasons = Context.tblMasCommonTypes.Where(a => a.isDeleted == 0 && a.MasterType == "UWReasons").Select(a => new MasterListItem
            {
                Value = a.Code,
                Text = a.Description
            }).OrderBy(a => a.Text).ToList();
            return Reasons;
        }

        public List<MasterListItem> GetPreferredModes(string PlanCode)
        {
            List<MasterListItem> lstPreferredModes = (from occupation in Context.tblMasCommonTypes
                                                      where occupation.isDeleted == 0 && occupation.MasterType == "PaymentFrequency"
                                                      select new MasterListItem
                                                      {
                                                          Text = occupation.Description,
                                                          Value = occupation.Code
                                                      }).ToList();
            return lstPreferredModes;
        }

        public List<MasterListItem> ListUWMedicalDocumentsStatus()
        {
            List<MasterListItem> lstUWMedicalDocumentsModes = (from documentsstatus in Context.tblMasCommonTypes
                                                               where documentsstatus.isDeleted == 0 && documentsstatus.MasterType == "DocumentStatus"
                                                               select new MasterListItem
                                                               {
                                                                   Text = documentsstatus.Description,
                                                                   Value = documentsstatus.Code
                                                               }).ToList();
            return lstUWMedicalDocumentsModes;
        }
        public List<MasterListItem> GetPensionPeriod()
        {

            return (from terms in Context.tblMasCommonTypes
                    where terms.MasterType == "PensionPeriod" && terms.isDeleted == 0
                    select new MasterListItem
                    {
                        Text = terms.Description,
                        Value = terms.Description
                    }).ToList();
        }
        public List<MasterListItem> GetEasyPensionPolicyTerm()
        {

            return (from terms in Context.tblMasCommonTypes
                    where terms.MasterType == "EasyPensionPolicyTerm" && terms.isDeleted == 0
                    select new MasterListItem
                    {
                        Text = terms.Description,
                        Value = terms.Description
                    }).ToList();
        }
        public List<MasterListItem> GetSmartPensionPolicyTerm()
        {

            return (from terms in Context.tblMasCommonTypes
                    where terms.MasterType == "SmartPensionPolicyTerm" && terms.isDeleted == 0
                    select new MasterListItem
                    {
                        Text = terms.Description,
                        Value = terms.Description
                    }).ToList();
        }
        public List<MasterListItem> GetSmartBuilderPolicyTerm()
        {

            return (from terms in Context.tblMasCommonTypes
                    where terms.MasterType == "SmartBuilderPolicyTerm" && terms.isDeleted == 0
                    select new MasterListItem
                    {
                        Text = terms.Description,
                        Value = terms.Description
                    }).ToList();
        }
        public List<MasterListItem> GetSmartBuilderPaymentTerm()
        {

            return (from terms in Context.tblMasCommonTypes
                    where terms.MasterType == "SmartBuilderPaymentTerm" && terms.isDeleted == 0
                    select new MasterListItem
                    {
                        Text = terms.Description,
                        Value = terms.Description
                    }).ToList();
        }
        public List<MasterListItem> GetSmartBuilderGoldPolicyTerm()
        {

            return (from terms in Context.tblMasCommonTypes
                    where terms.MasterType == "SmartBuilderGoldPolicyTerm" && terms.isDeleted == 0
                    select new MasterListItem
                    {
                        Text = terms.Description,
                        Value = terms.Description
                    }).ToList();
        }
        public List<MasterListItem> GetSmartBuilderGoldPaymentTerm()
        {

            return (from terms in Context.tblMasCommonTypes
                    where terms.MasterType == "SmartBuilderGoldPaymentTerm" && terms.isDeleted == 0
                    select new MasterListItem
                    {
                        Text = terms.Description,
                        Value = terms.Description
                    }).ToList();
        }
        public List<MasterListItem> GetPriorityValuePolicyTerm()
        {

            return (from terms in Context.tblMasCommonTypes
                    where terms.MasterType == "PriorityValuePolicyTerm" && terms.isDeleted == 0
                    select new MasterListItem
                    {
                        Text = terms.Description,
                        Value = terms.Description
                    }).ToList();
        }
        public List<MasterListItem> GetPriorityValuePaymentTerm()
        {

            return (from terms in Context.tblMasCommonTypes
                    where terms.MasterType == "PriorityValuePaymentTerm" && terms.isDeleted == 0
                    select new MasterListItem
                    {
                        Text = terms.Description,
                        Value = terms.Description
                    }).ToList();
        }
        public List<MasterListItem> GetBenefitYears()
        {

            return (from terms in Context.tblMasCommonTypes
                    where terms.MasterType == "BenefitYears" && terms.isDeleted == 0
                    orderby terms.Description
                    select new MasterListItem
                    {
                        Text = terms.Description,
                        Value = terms.Description
                    }).ToList();
        }

        public List<MasterListItem> GetAgeProof()
        {

            return (from s in Context.tblMasCommonTypes
                    where s.MasterType == "AgeProofList" && s.isDeleted == 0 // Master Type AgeProofList
                    select new MasterListItem
                    {
                        Text = s.Description,
                        Value = s.Code,
                        ID = s.CommonTypesID
                    }).ToList();
        }

        public List<MasterListItem> GetResidentialStatus()
        {

            return (from s in Context.tblMasCommonTypes
                    where s.MasterType == "Country" && s.isDeleted == 0 // Master Type ResidentialStatus
                    select new MasterListItem
                    {
                        Text = s.Description,
                        Value = s.Code,
                        ID = s.CommonTypesID
                    }).ToList();
        }
        public List<MasterListItem> GetPAQAssets()
        {

            return (from s in Context.tblMasCommonTypes
                    where s.MasterType == "PAQAssets" && s.isDeleted == 0 // Master Type PAQAssets
                    select new MasterListItem
                    {
                        ID = s.CommonTypesID,
                        Text = s.Description
                        //Value = s.CommonTypesID.ToString(),
                    }).ToList();
        }
        public List<MasterListItem> GetPAQLiabilities()
        {

            return (from s in Context.tblMasCommonTypes
                    where s.MasterType == "PAQLiabilities" && s.isDeleted == 0 // Master Type PAQLiabilities
                    select new MasterListItem
                    {
                        ID = s.CommonTypesID,
                        Text = s.Description
                    }).ToList();
        }

        public List<MasterListItem> GetMaturityBenefits()
        {

            return (from s in Context.tblMasCommonTypes
                    where s.MasterType == "MaturityBenefits" && s.isDeleted == 0
                    select new MasterListItem
                    {
                        Text = s.Description,
                        Value = s.CommonTypesID.ToString()
                    }).ToList();
        }
        public List<MasterListItem> GetHeightFeets()
        {

            return (from s in Context.tblMasCommonTypes
                    where s.MasterType == "HeightFeets" && s.isDeleted == 0
                    select new MasterListItem
                    {
                        Text = s.Description,
                        Value = s.CommonTypesID.ToString(),
                        ID = s.CommonTypesID

                    }).ToList();
        }

        public List<MasterListItem> GetCountryofOccupation()
        {

            return (from s in Context.tblMasCommonTypes
                    where s.MasterType == "Country" && s.isDeleted == 0 // Master Type CountryofOccupation
                    select new MasterListItem
                    {
                        Text = s.Description,
                        Value = s.Code.ToString(),
                        ID = s.CommonTypesID
                    }).ToList();
        }
        public List<MasterListItem> GetPaymentMethod()
        {

            return (from s in Context.tblMasCommonTypes
                    where s.MasterType == "PaymentMethod" && s.isDeleted == 0
                    orderby s.Description
                    select new MasterListItem
                    {
                        Text = s.Description,
                        Value = s.CommonTypesID.ToString()
                    }).ToList();
        }
        public List<MasterListItem> GetPaymentRelations()
        {

            return (from s in Context.tblMasCommonTypes
                    where s.MasterType == "HealthRelationship" && s.isDeleted == 0
                    orderby s.Description
                    select new MasterListItem
                    {
                        Text = s.Description,
                        Value = s.CommonTypesID.ToString()
                    }).ToList();
        }

        public List<MasterListItem> GetPaymentReceiptPrefferdBy()
        {

            return (from s in Context.tblMasCommonTypes
                    where s.MasterType == "PreferredReceipt" && s.isDeleted == 0
                    orderby s.Description
                    select new MasterListItem
                    {
                        Text = s.Description,
                        Value = s.CommonTypesID.ToString()
                    }).ToList();
        }

        public List<MasterListItem> GetRetirementAge()
        {

            return (from terms in Context.tblMasCommonTypes
                    where terms.MasterType == "RetirementAge" && terms.isDeleted == 0
                    select new MasterListItem
                    {
                        Text = terms.Description,
                        Value = terms.Description
                    }).ToList();
        }

        public List<MasterListItem> GetDrawDownPeriod()
        {

            return (from terms in Context.tblMasCommonTypes
                    where terms.MasterType == "DrawDownPeriod" && terms.isDeleted == 0
                    select new MasterListItem
                    {
                        Text = terms.Description,
                        Value = terms.Description
                    }).ToList();
        }


        public List<MasterListItem> ListLanguage()
        {
            return (from terms in Context.tblMasCommonTypes
                    where terms.MasterType == "PrefferedLanguage" && terms.isDeleted == 0
                    select new MasterListItem
                    {
                        Text = terms.Description,
                        Value = terms.Code
                    }).ToList();
        }
        public AIA.Life.Models.Common.Address FillAddressMasterList()
        {
            AIA.Life.Models.Common.Address objAddress = new AIA.Life.Models.Common.Address();
            var pincode = GetPincode();
            objAddress.LstPincode = pincode;
            return objAddress;
        }
        public List<MasterListItem> GetPincode()
        {

            List<MasterListItem> records = (from city in Context.tblMasCityDistrictProvinces
                                            where city.IsDeleted == false
                                            select new MasterListItem
                                            {
                                                Value = city.PostalCode + "|" + city.CityName,
                                                selected = city.MasterID
                                            }).ToList();

            return records;
        }

        public List<MasterListItem> ListHeight()
        {
            List<MasterListItem> lstHeight = new List<MasterListItem>();
            lstHeight.Add(new MasterListItem { Text = "5.0", Value = "5.0" });
            lstHeight.Add(new MasterListItem { Text = "5.5", Value = "5.5" });
            lstHeight.Add(new MasterListItem { Text = "5.8", Value = "5.8" });
            return lstHeight;
        }


        public List<MasterListItem> ListWeight()
        {
            List<MasterListItem> lstWeight = new List<MasterListItem>();
            lstWeight.Add(new MasterListItem { Text = "55", Value = "55" });
            lstWeight.Add(new MasterListItem { Text = "65", Value = "65" });
            lstWeight.Add(new MasterListItem { Text = "75", Value = "75" });
            return lstWeight;
        }

        public List<MasterListItem> ListCity()
        {
            List<MasterListItem> lstCity = new List<MasterListItem>();
            lstCity.Add(new MasterListItem { Text = "Bangalore", Value = "Bangalore" });
            lstCity.Add(new MasterListItem { Text = "Hyderabad", Value = "Hyderabad" });
            lstCity.Add(new MasterListItem { Text = "Chennai", Value = "Chennai" });
            return lstCity;
        }


        public AIA.Life.Models.Policy.Policy LoadMastersForProposal(AIA.Life.Models.Policy.Policy objpolicy)
        {

            objpolicy.lstRelations = GetMasCommonTypeMasterListItem("HealthRelationship");
            objpolicy.lstBeneficiaryRelations = GetMasCommonTypeMasterListItem("Relationshipwiththepolicyowner");
            objpolicy.lstPAQAssets = GetPAQAssets();
            objpolicy.lstPAQLiabilities = GetPAQLiabilities();
            objpolicy.Nationalities = GetNationality();
            objpolicy.lstProposalNeed = GetMasCommonTypeMasterListItem("NeedCategory");
            objpolicy.ProposalMaritalStatuslist = GetProposalMaritalStatus();
            objpolicy.lstRelationshipwithpolicyowner = GetRelationshipWithPolicyOwner();
            objpolicy.MaritalStatuslist = GetMaritalStatus();
            objpolicy.lstGender = GetGender();
            objpolicy.lstSalutation = GetSalutation();
            objpolicy.lstBeneficiarySalutation = GetBeneficiarySalutation();
            objpolicy.lstOccupation = GetOccupation();

            objpolicy.lstMemberOccupation = GetOccupation();
            objpolicy.lstSalutation = GetSalutation();
            objpolicy.lstDocumentName = GetMasDocumentNames();
            objpolicy.lstLanguage = ListLanguage();
            objpolicy.ListPlan = ListProducts();
            objpolicy.LstPolicyTerm = GetPensionPeriod();
            objpolicy.LstEasyPensionPolicyTerm = GetEasyPensionPolicyTerm();
            objpolicy.LstSmartPensionPolicyTerm = GetSmartPensionPolicyTerm();
            objpolicy.LstSmartBuilderPolicyTerm = GetSmartBuilderPolicyTerm();
            objpolicy.LstSmartBuilderPaymentTerm = GetSmartBuilderPaymentTerm();

            objpolicy.LstSmartBuilderGoldPolicyTerm = GetSmartBuilderGoldPolicyTerm();
            objpolicy.LstSmartBuilderGoldPaymentTerm = GetSmartBuilderGoldPaymentTerm();
            objpolicy.LstPriorityValuePolicyTerm = GetPriorityValuePolicyTerm();
            objpolicy.LstPriorityValuePaymentTerm = GetPriorityValuePaymentTerm();
            objpolicy.LstBenefitYears = GetBenefitYears();
            objpolicy.LstPaymentfrequency = GetPreferredModes("Paymentfrequency");
            objpolicy.LstPaymentMethod = GetPaymentMethod();
            objpolicy.LstPaymentRelations = GetPaymentRelations();
            objpolicy.lstCauseOfDeath = GetMasCommonTypeMasterListItem("CauseOfDeathCase");
            objpolicy.lstSateofHealth = GetMasCommonTypeMasterListItem("StateOfHealth");
            objpolicy.lstFamilyBackGroundRelationship = GetMasCommonTypeMasterListItem("FamilyBackGroundRelationship");
            objpolicy.LstModeofCommunication = GetMasCommonTypeMasterListItem("ModeOfCommunication");
            objpolicy.LstPreferredReceipt = GetPaymentReceiptPrefferdBy();

            objpolicy.LstMaturityBenefits = GetMaturityBenefits();
            objpolicy.LstResidentialStatus = GetResidentialStatus();
            //objpolicy.lstPAQAssets = GetPAQAssets();
            //objpolicy.lstPAQLiabilities = GetPAQLiabilities();
            objpolicy.LstAgeProof = GetAgeProof();
            objpolicy.LstFillMemberCountryofOccupation = GetCountryofOccupation();
            objpolicy.LstLifeType = ListUWLifeType();
            objpolicy.LstUWStatus = ListUWMedicalDocumentsStatus();
            objpolicy.lstSmokeAndAlcholQuantity = ListSmokeAndAlcholQuantity();
            objpolicy.LstHeightFeets = GetHeightFeets();
            objpolicy.LstSelectedMedicalDocuments = ListMedicalDocuments();


            objpolicy.lstCauseOfDeath = GetMasCommonTypeMasterListItem("CauseOfDeathCase");
            objpolicy.lstSateofHealth = GetMasCommonTypeMasterListItem("StateOfHealth");
            objpolicy.lstFamilyBackGroundRelationship = GetMasCommonTypeMasterListItem("FamilyBackGroundRelationship");
            List<int?> lstCommontypeID = new List<int?>() { 269, 270 };// IDS For Dependent Relationship
            objpolicy.lstDependentRelationship = GetMasCommonTypeMasterListItem(null, null, lstCommontypeID);
            objpolicy.LstHeightFeets = GetHeightFeets();
            objpolicy.lstSmokeAndAlcholPer = GetMasCommonTypeMasterListItem("Smoke&AlcoholPer");
            objpolicy.lstSmokeTypes = GetMasCommonTypeMasterListItem("SmokeTypes");
            objpolicy.lstAlcoholTypes = GetMasCommonTypeMasterListItem("AlcoholTypes");
            objpolicy.lstCurrentStatus = GetMasCommonTypeMasterListItem("PrevInsurenceCurrentStatus");
            objpolicy.lstAerobicExercise = GetMasCommonTypeMasterListItem("AerobicExercise");
            objpolicy.lstFruitOrVegetablePortions = GetMasCommonTypeMasterListItem("FruitOrVegetablePortions");
            objpolicy.lstFluidOrWater = GetMasCommonTypeMasterListItem("FluidOrWater");
            objpolicy.lstDoctorNames = GetMasLookUpTypeMasterListItem("MDOCT");
            //  objpolicy.lstLabNames = GetMasLookUpTypeLabsListItem();
            objpolicy.LstHealthCheckupCategory = GetMasCommonTypeMasterListItem("HealthCheckupCategory");
            objpolicy.LstMaturityBenefits = GetMaturityBenefits();
            objpolicy.LstResidentialStatus = GetResidentialStatus();
            objpolicy.LstAgeProof = GetAgeProof();

            return objpolicy;
        }


        public AIA.Life.Models.Policy.ProposalMasters LoadProposalMasters()
        {
            AIA.Life.Models.Policy.ProposalMasters objProposalMasters = new AIA.Life.Models.Policy.ProposalMasters();
            objProposalMasters.lstRelations = GetMasCommonTypeMasterListItem("HealthRelationship");
            objProposalMasters.lstBeneficiaryRelations = GetMasCommonTypeMasterListItem("Relationshipwiththepolicyowner");
            objProposalMasters.lstPAQAssets = GetMasCommonTypeMasterListItem("PAQAssets");
            objProposalMasters.lstPAQLiabilities = GetMasCommonTypeMasterListItem("PAQLiabilities");
            objProposalMasters.Nationalities = GetNationality();
            objProposalMasters.lstProposalNeed = GetMasCommonTypeMasterListItem("NeedCategory");
            objProposalMasters.ProposalMaritalStatuslist = GetProposalMaritalStatus();
            objProposalMasters.lstRelationshipwithpolicyowner = GetRelationshipWithPolicyOwner();
            objProposalMasters.MaritalStatuslist = GetMaritalStatus();
            objProposalMasters.lstGender = GetGender();
            objProposalMasters.lstSalutation = GetSalutation();
            objProposalMasters.lstBeneficiarySalutation = GetBeneficiarySalutation();
            objProposalMasters.lstOccupation = GetOccupation();

            objProposalMasters.lstMemberOccupation = GetOccupation();
            objProposalMasters.lstSalutation = GetSalutation();
            objProposalMasters.lstDocumentName = GetMasDocumentNames();
            objProposalMasters.lstLanguage = ListLanguage();
            objProposalMasters.ListPlan = ListProducts();
            objProposalMasters.LstPolicyTerm = GetPensionPeriod();
            objProposalMasters.LstEasyPensionPolicyTerm = GetEasyPensionPolicyTerm();
            objProposalMasters.LstSmartPensionPolicyTerm = GetSmartPensionPolicyTerm();
            objProposalMasters.LstSmartBuilderPolicyTerm = GetSmartBuilderPolicyTerm();
            objProposalMasters.LstSmartBuilderPaymentTerm = GetSmartBuilderPaymentTerm();

            objProposalMasters.LstSmartBuilderGoldPolicyTerm = GetSmartBuilderGoldPolicyTerm();
            objProposalMasters.LstSmartBuilderGoldPaymentTerm = GetSmartBuilderGoldPaymentTerm();
            objProposalMasters.LstPriorityValuePolicyTerm = GetPriorityValuePolicyTerm();
            objProposalMasters.LstPriorityValuePaymentTerm = GetPriorityValuePaymentTerm();
            objProposalMasters.LstBenefitYears = GetBenefitYears();
            objProposalMasters.LstPaymentfrequency = GetPreferredModes("Paymentfrequency");
            objProposalMasters.LstPaymentMethod = GetPaymentMethod();
            objProposalMasters.LstPaymentRelations = GetPaymentRelations();
            objProposalMasters.lstCauseOfDeath = GetMasCommonTypeMasterListItem("CauseOfDeathCase");
            objProposalMasters.lstSateofHealth = GetMasCommonTypeMasterListItem("StateOfHealth");
            objProposalMasters.lstFamilyBackGroundRelationship = GetMasCommonTypeMasterListItem("FamilyBackGroundRelationship");
            objProposalMasters.LstModeofCommunication = GetMasCommonTypeMasterListItem("ModeOfCommunication");
            objProposalMasters.LstPreferredReceipt = GetPaymentReceiptPrefferdBy();

            objProposalMasters.LstMaturityBenefits = GetMaturityBenefits();
            objProposalMasters.LstResidentialStatus = GetResidentialStatus();
            objProposalMasters.LstAgeProof = GetAgeProof();
            objProposalMasters.LstFillMemberCountryofOccupation = GetCountryofOccupation();
            objProposalMasters.LstLifeType = ListUWLifeType();
            objProposalMasters.LstUWStatus = ListUWMedicalDocumentsStatus();
            objProposalMasters.lstSmokeAndAlcholQuantity = ListSmokeAndAlcholQuantity();
            objProposalMasters.LstHeightFeets = GetHeightFeets();
            objProposalMasters.LstSelectedMedicalDocuments = ListMedicalDocuments();


            objProposalMasters.lstCauseOfDeath = GetMasCommonTypeMasterListItem("CauseOfDeathCase");
            objProposalMasters.lstSateofHealth = GetMasCommonTypeMasterListItem("StateOfHealth");
            objProposalMasters.lstFamilyBackGroundRelationship = GetMasCommonTypeMasterListItem("FamilyBackGroundRelationship");
            List<int?> lstCommontypeID = new List<int?>() { 269, 270 };// IDS For Dependent Relationship
            objProposalMasters.lstDependentRelationship = GetMasCommonTypeMasterListItem(null, null, lstCommontypeID);
            objProposalMasters.LstHeightFeets = GetHeightFeets();
            objProposalMasters.lstSmokeAndAlcholPer = GetMasCommonTypeMasterListItem("Smoke&AlcoholPer");
            objProposalMasters.lstSmokeTypes = GetMasCommonTypeMasterListItem("SmokeTypes");
            objProposalMasters.lstAlcoholTypes = GetMasCommonTypeMasterListItem("AlcoholTypes");
            objProposalMasters.lstCurrentStatus = GetMasCommonTypeMasterListItem("PrevInsurenceCurrentStatus");
            objProposalMasters.lstAerobicExercise = GetMasCommonTypeMasterListItem("AerobicExercise");
            //objProposalMasters.lstFruitOrVegetablePortions = GetMasCommonTypeMasterListItem("FruitOrVegetablePortions");
            //objProposalMasters.lstFluidOrWater = GetMasCommonTypeMasterListItem("FluidOrWater");
            //objProposalMasters.lstDoctorNames = GetMasLookUpTypeMasterListItem("MDOCT");
            objProposalMasters.lstLabNames = GetMasLookUpTypeLabsListItem();
            objProposalMasters.LstHealthCheckupCategory = GetMasCommonTypeMasterListItem("HealthCheckupCategory");
            objProposalMasters.LstMaturityBenefits = GetMaturityBenefits();
            objProposalMasters.LstResidentialStatus = GetResidentialStatus();
            objProposalMasters.LstAgeProof = GetAgeProof();

            return objProposalMasters;
        }


        public AIA.Life.Models.Opportunity.Suspect LoadType(AIA.Life.Models.Opportunity.Suspect objSuspect)
        {
            Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
            List<GetIntroducerCode_Result> listIntroducer = new List<GetIntroducerCode_Result>();
            string CreatedBy = objSuspect.CreatedBy;

            string role = Context.GetRoleByUserName(CreatedBy).FirstOrDefault();
            if (!string.IsNullOrEmpty(role))
            {
                if (role == "Banca Agent ")
                {
                    objSuspect.LstType = GetBancaType();
                    objSuspect.Role = role;
                    listIntroducer = Context.GetIntroducerCode(CreatedBy).ToList();

                    List<MasterListItem> ObjListItems = new List<MasterListItem>();
                    for (int i = 0; i < listIntroducer.Count(); i++)
                    {
                        MasterListItem obj = new MasterListItem();
                        obj.Value = listIntroducer[i].code;
                        obj.Text = listIntroducer[i].Introducercode;
                        ObjListItems.Add(obj);
                    }


                    objSuspect.LstIntroducerCode = ObjListItems;
                }
                if (role != "Banca Agent ")
                {
                    objSuspect.Role = role;
                    objSuspect.LstType = GetType();
                }
            }
            return objSuspect;

        }
        public AIA.Life.Models.Opportunity.Suspect LoadMastersSalutation(AIA.Life.Models.Opportunity.Suspect objSuspect)
        {
            Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
            objSuspect.LstSalutation = objCommonBusiness.GetSalutation();
            return objSuspect;

        }
        public AIA.Life.Models.Opportunity.Suspect LoadMastersOccupation(AIA.Life.Models.Opportunity.Suspect objSuspect)
        {
            Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
            objSuspect.LstOccupation = objCommonBusiness.GetOccupation(objSuspect.Prefix.ToUpper());
            return objSuspect;

        }

        public AIA.Life.Models.Policy.Policy LoadMastersRelationship(AIA.Life.Models.Policy.Policy objPolicy)
        {
            Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
            objPolicy.lstRelations = objCommonBusiness.GetRelationShips();
            return objPolicy;

        }

        public AIA.Life.Models.Opportunity.Prospect LoadMastersAnnualAmount(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
            objProspect.objNeedAnalysis.dllannualamount = objCommonBusiness.GetAnnualAmountFNA();
            return objProspect;

        }
        public List<MasterListItem> ListProducts()
        {
            List<MasterListItem> lstProducts = new List<MasterListItem>();
            lstProducts = (from Product in Context.tblProducts.Where(a => a.IsActive == true)
                           select new MasterListItem
                           {
                               Text = Product.ProductName,
                               Value = Product.ProductId.ToString()
                           }
                      ).ToList();


            return lstProducts;


        }


        public AddressMaster GetAddresMasters(string Term)
        {
            AddressMaster obj = new AddressMaster();
            obj.Term = Term;
            obj.LstAddressMaster = new List<MasterListItem>();
            if (!string.IsNullOrEmpty(Term))
            {
                obj.LstAddressMaster = (from objAddressMaster in Context.tblMasCityDistrictProvinces
                                        where objAddressMaster.IsDeleted != true &&
                                        (objAddressMaster.CityName.ToUpper() + " " + objAddressMaster.DistrictName.ToUpper() + " " + objAddressMaster.ProvinceName.ToUpper()).Contains(Term.ToUpper())
                                        select new MasterListItem
                                        {
                                            Text = objAddressMaster.CityName + " | " + objAddressMaster.DistrictName + " | " + objAddressMaster.ProvinceName + " | " + objAddressMaster.PostalCode,
                                            Value = objAddressMaster.CityName + " | " + objAddressMaster.DistrictName + " | " + objAddressMaster.ProvinceName + " | " + objAddressMaster.PostalCode
                                        }).ToList();
            }

            return obj;
        }
        public List<Address> GetProvinceMaster()
        {
            List<Address> listProvince = new List<Address>();
            var listtblMasProvince = Context.tblMasCityDistrictProvinces.Where(a => a.IsDeleted == false && !string.IsNullOrEmpty(a.ProvinceName)).Select(a => new { provincecode = a.ProvinceCode, provincename = a.ProvinceName }).Distinct();
            if (listtblMasProvince != null)
            {
                foreach (var item in listtblMasProvince)
                {
                    Address obj = new Address();
                    obj.ProvinceCode = item.provincecode;
                    obj.Province = item.provincename;
                    listProvince.Add(obj);
                }
                return listProvince;
            }
            else
            {
                return listProvince;
            }
        }
        public List<Address> GetDistrictMaster(string ProvinceCode)
        {
            List<Address> listDistrict = new List<Address>();
            var listtblMasDistrict = Context.tblMasCityDistrictProvinces.Where(a => a.IsDeleted == false && a.ProvinceCode == ProvinceCode && !string.IsNullOrEmpty(a.DistrictName)).Select(a => new { districtcode = a.DistrictCode, districtname = a.DistrictName }).Distinct().ToList();
            if (listtblMasDistrict != null)
            {
                foreach (var item in listtblMasDistrict)
                {
                    Address obj = new Address();
                    obj.DistrictCode = item.districtcode;
                    obj.District = item.districtname;
                    listDistrict.Add(obj);
                }
                return listDistrict;
            }
            else
            {
                return listDistrict;
            }
        }
        public List<Address> GetCityMaster(string DistrictCode)
        {
            List<Address> listCity = new List<Address>();
            var listtblMasCity = Context.tblMasCityDistrictProvinces.Where(a => a.IsDeleted == false && a.DistrictCode == DistrictCode && !string.IsNullOrEmpty(a.CityName)).Select(a => new { citycode = a.CityCode, cityname = a.CityName }).Distinct().ToList();
            if (listtblMasCity != null)
            {
                foreach (var item in listtblMasCity)
                {
                    Address obj = new Address();
                    obj.CityCode = item.citycode;
                    obj.City = item.cityname;
                    listCity.Add(obj);
                }
                return listCity;
            }
            else
            {
                return listCity;
            }
        }
        public string GetPostalCodeMaster(string CityCode)
        {
            try
            {

                string PostalCode = null;
                if (CityCode != null && CityCode != "")
                {
                    PostalCode = Context.tblMasCityDistrictProvinces.Where(a => a.IsDeleted == false && a.CityCode == CityCode && !string.IsNullOrEmpty(a.CityCode) && a.PostalCode != "0").Select(a => a.PostalCode).SingleOrDefault();
                }
                return PostalCode;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GetProvince(string CityCode)
        {
            try
            {
                string[] postalcode = CityCode.Split('|');
                string PostalCode = postalcode[0];
                string province = "";
                if (CityCode != null && CityCode != "")
                {
                    province = Context.tblMasCityDistrictProvinces.Where(a => a.IsDeleted == false && a.PostalCode == PostalCode && !string.IsNullOrEmpty(a.PostalCode) && a.PostalCode != "0").Select(a => a.ProvinceName).SingleOrDefault();
                }
                return province;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetDistrict(string CityCode)
        {
            try
            {
                string[] postalcode = CityCode.Split('|');
                string PostalCode = postalcode[0];
                string District = "";
                if (CityCode != null && CityCode != "")
                {
                    District = Context.tblMasCityDistrictProvinces.Where(a => a.IsDeleted == false && a.PostalCode == PostalCode && !string.IsNullOrEmpty(a.PostalCode) && a.PostalCode != "0").Select(a => a.DistrictName).SingleOrDefault();
                }
                return District;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProductMasters LoadProductMasters(ProductMasters obj)
        {

            tblProduct Product = new tblProduct();
            if (obj.ProductID > 0)
            {
                Product = Context.tblProducts.Where(a => a.ProductId == obj.ProductID).FirstOrDefault();
            }
            else
            {
                Product = Context.tblProducts.Where(a => a.ProductName == obj.ProductName).FirstOrDefault();
            }

            if (Product != null)
            {
                obj.ProductID = Product.ProductId;
                obj.PlanCode = Product.ProductCode;

                #region Policy Term
                var ProductParameter = Context.tblProductParameters.Where(a => a.ParameterId == 6 && a.ProductId == obj.ProductID).FirstOrDefault();
                if (ProductParameter != null)
                {
                    obj.LstPolicyTerm = new List<MasterListItem>();
                    int Minvalue = Convert.ToInt32(ProductParameter.NumericValueFrom);

                    while (Minvalue <= ProductParameter.NumericValueTo)
                    {
                        MasterListItem objMasterItem = new MasterListItem();
                        objMasterItem.Text = Convert.ToString(Minvalue);
                        objMasterItem.Value = Convert.ToString(Minvalue);
                        obj.LstPolicyTerm.Add(objMasterItem);
                        Minvalue++;
                    }

                }
                #endregion


                #region Policy Premium Term
                var PolicyPremiumTerm = Context.tblProductParameters.Where(a => a.ParameterId == 8 && a.ProductId == obj.ProductID).FirstOrDefault();
                if (PolicyPremiumTerm != null)
                {

                    obj.LstPremiumTerm = new List<MasterListItem>();
                    if (PolicyPremiumTerm.StringValueFrom == "Same as policy term")
                    {
                        obj.LstPremiumTerm.AddRange(obj.LstPolicyTerm);
                    }
                    else
                    {
                        int Minvalue = Convert.ToInt32(PolicyPremiumTerm.StringValueFrom);
                        int MaxValue = Convert.ToInt32(PolicyPremiumTerm.StringValueTo);
                        while (Minvalue <= MaxValue)
                        {
                            MasterListItem objMasterItem = new MasterListItem();
                            objMasterItem.Text = Convert.ToString(Minvalue);
                            objMasterItem.Value = Convert.ToString(Minvalue);
                            obj.LstPremiumTerm.Add(objMasterItem);
                            Minvalue++;
                        }
                    }


                }
                #endregion
            }




            return obj;
        }

        public Agent FetchAgentBranch(string UserName)
        {
            Agent objCommonBusiness = new Agent();
            var AgentCode = "";
            if (UserName != null && UserName != "")
            {
                AgentCode = (from obj in Context.tblUserDetails
                             where obj.LoginID == UserName
                             select obj.IMDCode).FirstOrDefault();
                if (AgentCode != null)
                {
                    var BranchData = (from obj in Context.tblProspects
                                      join obj1 in Context.tblProspectOfficials
                                      on obj.ProspectID equals obj1.ProspectID
                                      where obj.AgentCode == AgentCode
                                      select obj1).FirstOrDefault();
                    objCommonBusiness.AgentCode = AgentCode;
                    objCommonBusiness.BranchName = BranchData.BranchName;
                    objCommonBusiness.BranchCode = BranchData.BranchCode;
                }

            }
            return objCommonBusiness;
        }


        public List<MasterListItem> ListSmokeAndAlcholQuantity()
        {
            List<MasterListItem> lstlifetype = (from lifetype in Context.tblMasCommonTypes
                                                where lifetype.isDeleted == 0 && lifetype.MasterType == "SmokeLifeStyle"

                                                select new MasterListItem
                                                {
                                                    Text = lifetype.Description,
                                                    Value = lifetype.Code
                                                }).ToList();
            return lstlifetype;
            //Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
            //List<MasterListItem> objList = new List<MasterListItem>();
            //// For test purpose 
            //objList = new List<MasterListItem>();
            //objList.Add(new MasterListItem { ID = 1, Value = "1-20", Text = "1-20" });
            //objList.Add(new MasterListItem { ID = 2, Value = "20 Above", Text = "20 Above" });
            //// Till here
            //return objList;
        }

        public List<MasterListItem> ListUWLifeType()
        {
            List<MasterListItem> lstlifetype = (from lifetype in Context.tblMasCommonTypes
                                                where lifetype.isDeleted == 0 && lifetype.MasterType == "UWLife"
                                                select new MasterListItem
                                                {
                                                    Text = lifetype.Description,
                                                    Value = lifetype.Code
                                                }).ToList();
            return lstlifetype;
        }

        public List<MasterListItem> ListMedicalDocuments()
        {
            List<MasterListItem> lstmedicaldocuments = (from medicaldocuments in Context.tblMasCommonTypes
                                                        where medicaldocuments.isDeleted == 0 && medicaldocuments.MasterType == "DocumentsTypes"
                                                        select new MasterListItem
                                                        {
                                                            Text = medicaldocuments.Description,
                                                            Value = medicaldocuments.Code
                                                        }).ToList();
            return lstmedicaldocuments;
        }



        public List<MasterListItem> GetSumInsured()
        {

            List<MasterListItem> lstSumInsured = (from s in Context.tblMasCommonTypes
                                                  where s.MasterType == "EasyPensionSA"
                                                  select new MasterListItem
                                                  {
                                                      Text = s.Description,
                                                      Value = s.Description,
                                                  }).ToList();
            return lstSumInsured;
        }
        public List<MasterListItem> GetUWDecision()
        {

            List<MasterListItem> lstUWDecision = (from s in Context.tblMasCommonTypes
                                                  where s.MasterType == "Decision" && s.isDeleted != 1
                                                  select new MasterListItem
                                                  {
                                                      Value = s.Description,
                                                      ID = s.CommonTypesID,
                                                  }).ToList();
            return lstUWDecision;
        }
        public List<MasterListItem> GetDocumentType()
        {

            List<MasterListItem> lstUWDocumentType = (from s in Context.tblMasCommonTypes
                                                      where s.MasterType == "DocumentType"
                                                      select new MasterListItem
                                                      {
                                                          Text = s.Description,
                                                          Value = s.Description,
                                                      }).ToList();
            return lstUWDocumentType;
        }
        public List<MasterListItem> GetAdditionalMedicalDocument()
        {

            List<MasterListItem> lstAdditionalMedicalDocument = (from s in Context.tblMasDocuments
                                                                 where s.DocumentType == "Medical"
                                                                 select new MasterListItem
                                                                 {
                                                                     Text = s.DocumentName,
                                                                     Value = s.DocumentName,
                                                                 }).ToList();
            return lstAdditionalMedicalDocument;
        }
        public List<MasterListItem> GetAdditionalNonMedicalDocument()
        {

            List<MasterListItem> lstAdditionalNonMedicalDocument = (from s in Context.tblMasDocuments
                                                                    where s.DocumentType != "Medical"
                                                                    select new MasterListItem
                                                                    {
                                                                        Text = s.DocumentName,
                                                                        Value = s.DocumentName,
                                                                    }).ToList();
            return lstAdditionalNonMedicalDocument;
        }
        public List<MasterListItem> GetType()
        {

            List<MasterListItem> lstType = (from s in Context.tblMasCommonTypes
                                            where s.MasterType == "Type"
                                            select new MasterListItem
                                            {
                                                ID = s.CommonTypesID,
                                                Value = s.Description,
                                            }).OrderBy(a => a.Value).ToList();
            return lstType;
        }
        public List<MasterListItem> GetBancaType()
        {

            List<MasterListItem> lstType = (from s in Context.tblMasCommonTypes
                                            where s.MasterType == "BancaType"
                                            select new MasterListItem
                                            {
                                                ID = s.CommonTypesID,
                                                Value = s.Description,
                                            }).OrderBy(a => a.Value).ToList();
            return lstType;
        }
        public List<MasterListItem> GetSalutation()
        {

            List<MasterListItem> lstType = (from s in Context.tblMasCommonTypes
                                            where s.MasterType == "Salutation" && s.isDeleted == 0
                                            select new MasterListItem
                                            {
                                                Text = s.Description,
                                                Value = s.Code

                                            }).OrderBy(a => a.Text).ToList();
            return lstType;
        }
        public List<MasterListItem> GetNationality()
        {

            List<MasterListItem> lstType = (from s in Context.tblMasCommonTypes
                                            where s.MasterType == "Country" && s.isDeleted == 0
                                            select new MasterListItem
                                            {
                                                Text = s.Description,
                                                Value = s.Code,
                                                ID = s.CommonTypesID

                                            }).ToList();
            return lstType;
        }


        public List<MasterListItem> GetBeneficiarySalutation()
        {

            List<MasterListItem> lstType = (from s in Context.tblMasCommonTypes
                                            where s.MasterType == "BeneficiarySalutation" && s.isDeleted == 0
                                            select new MasterListItem
                                            {
                                                Text = s.Description,
                                                Value = s.Code

                                            }).OrderBy(a => a.Text).ToList();
            return lstType;
        }


        public List<MasterListItem> GetGender()
        {

            List<MasterListItem> lstType = (from s in Context.tblMasCommonTypes
                                            where s.MasterType == "Gender" && s.isDeleted == 0
                                            select new MasterListItem
                                            {
                                                Text = s.Description,
                                                Value = s.Code

                                            }).OrderBy(a => a.Text).ToList();
            return lstType;
        }
        public List<Dependants> GetDependant(AIA.Life.Models.Opportunity.LifeQuote objQuoteList)
        {
            var objDependants = Context.tblDependants.Where(a => a.ContactID == objQuoteList.Contactid).ToList();
            foreach (var item in objDependants)
            {
                Dependants obj = new Dependants();
                obj.Name = item.DependantName;
                obj.AgeNextBirthday = Convert.ToInt32(item.DependantAge);
                obj.DOB = item.DependantDOB;
                obj.Relationship = item.DependantRelation;
                objQuoteList.objProspect.objNeedAnalysis.objDependants.Add(obj);
            }
            return objQuoteList.objProspect.objNeedAnalysis.objDependants;
        }
        public List<MasterListItem> GetMaritalStatus()
        {

            List<MasterListItem> lstType = (from s in Context.tblMasCommonTypes
                                            where s.MasterType == "MaritalStatus" && s.isDeleted == 0
                                            select new MasterListItem
                                            {
                                                Text = s.Description,
                                                Value = s.Code.ToString()

                                            }).OrderBy(a => a.Text).ToList();
            return lstType;
        }
        public List<MasterListItem> GetRelationshipWithPolicyOwner()
        {

            List<MasterListItem> lstType = (from s in Context.tblMasCommonTypes
                                            where s.MasterType == "Relationshipwiththepolicyowner" && s.isDeleted == 0
                                            select new MasterListItem
                                            {
                                                Text = s.Description,
                                                Value = s.Code.ToString(),
                                            }).OrderBy(a => a.Text).ToList();
            return lstType;
        }

        public List<MasterListItem> GetOccupation(string Prefix = null)
        {
            List<MasterListItem> lstOccupation = new List<MasterListItem>();
            if (Prefix == null)
                lstOccupation = (from occupation in Context.tblMasLifeOccupations
                                 select new MasterListItem
                                 {
                                     Text = string.Concat(occupation.OccupationCode, "|", occupation.SinhalaDesc, "|", occupation.TamilDesc),
                                     Value = occupation.CompanyCode
                                 }).OrderBy(a => a.Text).ToList();
            else
                lstOccupation = (from occupation in Context.tblMasLifeOccupations
                                 where occupation.OccupationCode.StartsWith(Prefix) || occupation.TanglishDesc.StartsWith(Prefix) || occupation.SinglishDesc.StartsWith(Prefix)
                                 select new MasterListItem
                                 {
                                     Text = string.Concat(occupation.OccupationCode, "|", occupation.SinhalaDesc, "|", occupation.TamilDesc),
                                     Value = occupation.CompanyCode
                                 }).OrderBy(a => a.Text).ToList();
            return lstOccupation;
        }

        //public List<MasterListItem> GetMemberOccupation()
        //{
        //    List<MasterListItem> lst = new List<MasterListItem>();

        //    return (from occupation in Context.tblMasLifeOccupations
        //            select new MasterListItem
        //            {
        //                Text = occupation.OccupationCode,
        //                Value = occupation.Code.ToString()
        //            }).OrderBy(a => a.Text).ToList();
        //}

        public List<MasterListItem> GetAnnualAmountFNA()
        {

            List<MasterListItem> lstType = (from s in Context.tblMasCommonTypes
                                            where s.MasterType == "AnnualAmountFNA" && s.isDeleted == 0
                                            select new MasterListItem
                                            {
                                                Text = s.Description,
                                                ID = s.CommonTypesID

                                            }).OrderBy(a => a.ID).ToList();
            return lstType;

        }
        public List<MasterListItem> GetCoverageFNA()
        {

            List<MasterListItem> lstType = (from s in Context.tblMasCommonTypes
                                            where s.MasterType == "CoverageFNA" && s.isDeleted == 0
                                            select new MasterListItem
                                            {
                                                Text = s.Description,
                                                Value = s.Code

                                            }).OrderByDescending(a => a.Text).ToList();
            return lstType;

        }
        public List<MasterListItem> GetAdequacyFNA()
        {

            List<MasterListItem> lstType = (from s in Context.tblMasCommonTypes
                                            where s.MasterType == "AdequacyFNA" && s.isDeleted == 0
                                            select new MasterListItem
                                            {
                                                Text = s.Description,
                                                Value = s.Code

                                            }).OrderByDescending(a => a.Text).ToList();
            return lstType;

        }
        public List<MasterListItem> GetHealthAdversitiesFNA()
        {

            List<MasterListItem> lstType = (from s in Context.tblMasCommonTypes
                                            where s.MasterType == "HealthAdversitiesFNA" && s.isDeleted == 0
                                            select new MasterListItem
                                            {
                                                Text = s.Description,
                                                Value = s.Code

                                            }).OrderBy(a => a.Text).ToList();
            return lstType;

        }
        public List<MasterListItem> GetChildRelationship()
        {

            List<MasterListItem> lstType = (from s in Context.tblMasCommonTypes
                                            where s.MasterType == "RelationshipFNA" && (s.CommonTypesID == 2435 || s.CommonTypesID == 2436)
                                            select new MasterListItem
                                            {
                                                Text = s.Description,
                                                Value = s.Code

                                            }).OrderBy(a => a.Text).ToList();
            return lstType;

        }
        public List<MasterListItem> GetChildName(AIA.Life.Models.Opportunity.Prospect objProspect)
        {

            List<MasterListItem> lstType = (from s in Context.tblDependants
                                            where s.ContactID == objProspect.ContactID
                                            select new MasterListItem
                                            {
                                                Text = s.DependantName,
                                                Value = s.DependantID.ToString()

                                            }).OrderBy(a => a.Text).ToList();
            return lstType;

        }
        public List<MasterListItem> GetRelationShips()
        {

            List<MasterListItem> lstType = (from s in Context.tblMasCommonTypes
                                            where s.MasterType == "HealthRelationship" && s.isDeleted == 0
                                            select new MasterListItem
                                            {
                                                Text = s.Description,
                                                Value = s.Code

                                            }).OrderBy(a => a.Text).ToList();
            return lstType;
        }

        public List<MasterListItem> GetMasDocumentNames()
        {

            //List<MasterListItem> lstType = (from s in Context.tblMasCommonTypes
            //                                where s.MasterType == "HealthDocumentNames" && s.isDeleted == 0
            //                                select new MasterListItem
            //                                {
            //                                    Text = s.Description,
            //                                    Value = s.Code.ToString()

            //                                }).OrderBy(a => a.Text).ToList();
            List<MasterListItem> lstType = (from s in Context.tblMasDocuments
                                            select new MasterListItem
                                            {
                                                Text = s.DocumentName,
                                                Value = s.DocumentName

                                            }).OrderBy(a => a.Text).ToList();
            return lstType;
        }

        public List<MasterListItem> GetProposalMaritalStatus()
        {

            List<MasterListItem> lstType = (from s in Context.tblMasCommonTypes
                                            where s.MasterType == "MaritalStatus" && s.isDeleted == 0
                                            select new MasterListItem
                                            {
                                                Text = s.Description,
                                                Value = s.CommonTypesID.ToString()

                                            }).OrderBy(a => a.Text).ToList();
            return lstType;
        }
        public List<MasterListItem> GetInstrumentType()
        {

            List<MasterListItem> lstType = (from s in Context.tblMasCommonTypes
                                            where s.MasterType == "InstrumentType" && s.isDeleted == 0
                                            select new MasterListItem
                                            {
                                                Text = s.Description,
                                                ID = s.CommonTypesID

                                            }).OrderBy(a => a.Text).ToList();
            return lstType;
        }

        public List<MasterListItem> GetUnderWriterList(string UserName, bool IsCurrentUser = true)
        {
            string RoleID = Context.AspNetRoles.Where(a => a.Name == "UW User").FirstOrDefault().Id;
            var idParam = new SqlParameter
            {
                ParameterName = "@RoleId",
                Value = RoleID
            };
            List<string> UserIDs = Context.Database.SqlQuery<string>(
               "exec GetUsersByRoleId @RoleId", idParam).ToList();

            if (UserIDs != null)
            {
                List<MasterListItem> LstUwDetails = (from aspnetusers in Context.AspNetUsers.Where(a => UserIDs.Contains(a.Id))
                                                     join userdetails in Context.tblUserDetails
                                                     on aspnetusers.UserName equals userdetails.LoginID
                                                     select new MasterListItem
                                                     {
                                                         Text = aspnetusers.UserName,
                                                         Value = aspnetusers.Id
                                                     }).ToList();
                if (!IsCurrentUser)
                {
                    int Index = LstUwDetails.FindIndex(a => a.Text == UserName);
                    if (Index >= 0)
                    {
                        LstUwDetails.RemoveAt(Index);
                    }
                }
                return LstUwDetails;
            }
            else
            {
                return new List<MasterListItem>();
            }
        }

        public ProposalDetails GetProposalDetails(ProposalDetails obj)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "@NICNo",
                Value = obj.NICNo
            };
            try
            {
                List<PreviousPolicyDetails> previousProposalList = Context.Database.SqlQuery<PreviousPolicyDetails>(
                   "exec usp_GetPreviousPolicyDetails @NICNo", idParam).ToList();
                idParam = new SqlParameter
                {
                    ParameterName = "@NICNo",
                    Value = obj.NICNo
                };
                List<OnGoingProposalDetails> ongoingProposalList = Context.Database.SqlQuery<OnGoingProposalDetails>(
                   "exec usp_GetOngoingProposalDetails @NICNo", idParam).ToList();
                idParam = new SqlParameter
                {
                    ParameterName = "@NICNo",
                    Value = obj.NICNo
                };
                List<SARDetails> SARDetails = Context.Database.SqlQuery<SARDetails>(
                   "exec SP_GetSARDetails @NICNo", idParam).ToList();
                List<SARFALDetails> SARFALDetails = new List<AIA.Life.Models.Common.SARFALDetails>();
                if (obj.QuoteNo != null)
                {
                    idParam = new SqlParameter
                    {
                        ParameterName = "@QuoteNo",
                        Value = obj.QuoteNo
                    };
                    SARFALDetails = Context.Database.SqlQuery<SARFALDetails>(
                       "exec SP_GetSARAndFALDetailsForQuote @QuoteNo", idParam).ToList();
                }

                obj = new ProposalDetails();
                //if (SARDetails[0].ANNPREM != null)
                //{
                //    if (SARDetails[0].ANNPREM > 250000)
                //        obj.AFC = true;
                //    else
                //        obj.AFC = false;
                //}
                if (previousProposalList.Count() > 0)
                {
                    obj.PreviousProposalsList = previousProposalList;
                }
                if (ongoingProposalList.Count() > 0)
                {
                    obj.OnGoingProposalsList = ongoingProposalList;
                }
                if (SARDetails != null)
                {
                    obj.SARDetails = SARDetails[0];
                }
                else
                {
                    obj.SARDetails = new AIA.Life.Models.Common.SARDetails();
                }
                if (SARFALDetails.Count() > 0)
                {
                    obj.SARFALDetails = SARFALDetails;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }

        public AppVersion GetLatestVersion(AppVersion obj)
        {
            try
            {
                string Version = obj.VersionNo;
                tblAppVersion objTblAppVersion = new tblAppVersion();
                if (string.IsNullOrEmpty(obj.AppType))
                {
                    objTblAppVersion = Context.tblAppVersions.ToList().OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                }
                else
                {
                    objTblAppVersion = Context.tblAppVersions.Where(x => x.APKType == obj.AppType).ToList().OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                }
                obj.AppName = objTblAppVersion.AppName;
                obj.CloudPath = objTblAppVersion.CloudPath;
                obj.VersionNo = objTblAppVersion.VersioNo;
                obj.isMandatory = objTblAppVersion.isMandatory == 1 ? true : false;
                obj.Message = objTblAppVersion.CustomMessage;
                obj.Error = new Error();
                if (obj.VersionNo != Version)
                {

                    obj.Error.ErrorCode = "0";
                    obj.Error.ErrorMessage = string.Empty;
                }
                else
                {

                    obj.Error.ErrorCode = "-1";
                    obj.Error.ErrorMessage = "Not available";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }

        public AppVersion UpdateLatestVersion(AppVersion obj)
        {
            try
            {

                tblAppVersion objTblAppVersion = Context.tblAppVersions.Where(x => x.VersioNo == obj.VersionNo && x.APKType == obj.AppType).FirstOrDefault();
                if (objTblAppVersion == null)
                {
                    objTblAppVersion = new tblAppVersion();
                    objTblAppVersion.AppName = obj.AppName;
                    objTblAppVersion.CloudPath = obj.CloudPath;
                    objTblAppVersion.VersioNo = obj.VersionNo;
                    objTblAppVersion.CreatedDate = DateTime.Now;
                    objTblAppVersion.APKType = obj.AppType;
                    objTblAppVersion.isMandatory = obj.isMandatory == true ? 1 : 0;
                    objTblAppVersion.CustomMessage = obj.Message;
                    Context.tblAppVersions.Add(objTblAppVersion);
                    Context.SaveChanges();

                }
                else
                {
                    obj.Error = new Error();
                    obj.Error.ErrorMessage = "Version already Exists..!";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }

        public String GenerateTraceNumber(string ProductCode)
        {
            Int32 NextSeqNo = GetNextQuoteNumber("TraceNo");
            string DateLogic = DateTime.Today.Day.ToString().PadLeft(2, '0') + DateTime.Today.Month.ToString().PadLeft(2, '0') + DateTime.Today.Year.ToString().Substring(2, 2);
            String TraceNumber = "T" + ProductCode + DateLogic + NextSeqNo.ToString().PadLeft(6, '0');
            return TraceNumber;

        }
        public int GetNextQuoteNumber(string numberingtype)
        {
            ObjectParameter nextNo = new ObjectParameter("NextNo", SqlDbType.Int);
            Context.usp_GetNextQuoteNumber(numberingtype, nextNo);
            return Convert.ToInt32(nextNo.Value);
        }
        public string ConverttoTitlecase(string TitleCase)

        {

            string output = "";

            if (TitleCase.ToUpper().Contains('+'))

            {

                output = String.Join("+", TitleCase.Split('+').Select(i => i.Substring(0, 1).ToUpper() + i.Substring(1).ToLower()).ToArray());

            }

            else

                output = String.Join("", TitleCase.Split(' ').Select(i => i.Substring(0, 1).ToUpper() + i.Substring(1).ToLower()).ToArray());

            return output;

        }
        public static string FetchDateMonth(string NIC)
        {
            string NoofDays = null;
            string Year = null;
            string day = null;
            string month = null;
            if (NIC.Length == 12)
            {
                NoofDays = NIC.Substring(4, 3);
                Year = NIC.Substring(0, 4);
            }
            else if (NIC.Length == 10)
            {
                NoofDays = NIC.Substring(2, 3);
                Year = NIC.Substring(0, 2);

            }
            string Date = null;
            if (NoofDays != null && NoofDays != "")
            {
                int NICYear = Convert.ToInt32(Year);
                int TotalDays = Convert.ToInt32(NoofDays);
                if (TotalDays <= 366 && TotalDays >= 0)
                {
                    if (NICYear % 4 != 0)
                    {
                        if (TotalDays > 59)
                        {
                            TotalDays = TotalDays - 1;
                        }
                    }
                    double Days = Convert.ToDouble(TotalDays - 1);
                    DateTime Dates = DateTime.Parse("01-jan-" + Year).AddDays(Days);
                    if (Dates.Day < 10)
                    {
                        day = "0" + Dates.Day.ToString();
                    }
                    else
                    {
                        day = Dates.Day.ToString();
                    }
                    if (Dates.Month < 10)
                    {
                        month = "0" + Dates.Month.ToString();
                    }
                    else
                    {
                        month = Dates.Month.ToString();
                    }
                    Date = day + "/" + month + "/" + Dates.Year;
                }
                else if (TotalDays > 500 && TotalDays <= 866)
                {
                    if (NICYear % 4 != 0)
                    {
                        if (TotalDays > 559)
                        {
                            TotalDays = TotalDays - 1;
                        }
                    }
                    double Days = Convert.ToDouble((TotalDays - 1) - 500);
                    DateTime Dates = DateTime.Parse("01-jan-" + Year).AddDays(Days);
                    if (Dates.Day < 10)
                    {
                        day = "0" + Dates.Day.ToString();
                    }
                    else
                    {
                        day = Dates.Day.ToString();
                    }
                    if (Dates.Month < 10)
                    {
                        month = "0" + Dates.Month.ToString();
                    }
                    else
                    {
                        month = Dates.Month.ToString();
                    }
                    Date = day + "/" + month + "/" + Dates.Year;
                }
                else
                {
                    Date = DateTime.Now.ToddMMyyyyString();
                }
            }
            return Date;
        }
        public AuthorizeUser CheckAuthorisation(AuthorizeUser authorizeUser)
        {
            using (AVOAIALifeEntities entities = new AVOAIALifeEntities())
            {
                Guid? userGuid = entities.tblUserDetails.Where(a => a.LoginID == authorizeUser.UserName).Select(a => a.UserID).FirstOrDefault();
                string role = entities.usp_GetCurrentUserRole(authorizeUser.UserName).FirstOrDefault();
                if (role != "UW User" && role != "SUPADMIN")
                {
                    string userId = string.Empty;
                    if (userGuid != null)
                    {
                        userId = userGuid.ToString();
                    }
                    if (!string.IsNullOrEmpty(authorizeUser.QuoteNo))
                    {

                        var record = entities.tblLifeQQs.Where(a => a.QuoteNo == authorizeUser.QuoteNo && a.Createdby == userId).FirstOrDefault();
                        if (record == null)
                        {
                            authorizeUser.Error.ErrorMessage = "Sorry!! You are not authorized to access this Page.";
                        }
                        var claRecord = entities.tblPolicyCLAQuotes.Where(a => a.PolicyId == (entities.tblPolicies.Where(x => x.QuoteNo == authorizeUser.QuoteNo).Select(x => x.PolicyID).FirstOrDefault())).FirstOrDefault();
                        if (claRecord != null)
                            authorizeUser.Error.ErrorMessage = "";
                    }
                    else if (!string.IsNullOrEmpty(authorizeUser.ProposalNo) || authorizeUser.PolicyId != 0)
                    {
                        var record = entities.tblPolicies.Where(a => (a.ProposalNo == authorizeUser.ProposalNo || a.PolicyID == authorizeUser.PolicyId) && a.Createdby == userId).FirstOrDefault();
                        if (record == null)
                        {
                            authorizeUser.Error.ErrorMessage = "Sorry!! You are not authorized to access this Page.";
                        }
                    }
                    else if (authorizeUser.ContactId != 0)
                    {
                        var record = entities.tblContacts.Where(a => a.ContactID == authorizeUser.ContactId && a.CreatedBy == userId).FirstOrDefault();
                        if (record == null)
                        {
                            authorizeUser.Error.ErrorMessage = "Sorry!! You are not authorized to access this Page.";
                        }
                    }
                }
                return authorizeUser;
            }
        }
        public LifeAssuredAge CheckAgeChangeQuoteMembers(LifeAssuredAge lifeAssuredAge)
        {
            using (AVOAIALifeEntities entities = new AVOAIALifeEntities())
            {
                tblLifeQQ lifeQQ = entities.tblLifeQQs.Where(a => a.QuoteNo == lifeAssuredAge.QuoteNo).FirstOrDefault();
                var members = lifeQQ.tblQuoteMemberDetials.ToList();
                foreach (var item in members)
                {
                    int currentAge = CalculateCurrentAgeNextBday(item.DateOfBirth ?? DateTime.Now, lifeAssuredAge.Rcd);
                    if (currentAge != item.Age)
                    {
                        switch (item.AssuredName)
                        {
                            case "MainLife":
                                lifeAssuredAge.MainLifeAge = true;
                                break;
                            case "Spouse":
                            case "Spouse Life":
                                lifeAssuredAge.SpouseAge = true;
                                break;
                            case "Child1":
                                lifeAssuredAge.Child1Age = true;
                                break;
                            case "Child2":
                                lifeAssuredAge.Child2Age = true;
                                break;
                            case "Child3":
                                lifeAssuredAge.Child3Age = true;
                                break;
                            case "Child4":
                                lifeAssuredAge.Child4Age = true;
                                break;
                            case "Child5":
                                lifeAssuredAge.Child5Age = true;
                                break;
                        }
                    }
                }
                return lifeAssuredAge;
            }
        }
        public static int CalculateCurrentAgeNextBday(DateTime dob, DateTime? rcd = null)
        {
            DateTime today = DateTime.Now;
            if (rcd != null)
                today = Convert.ToDateTime(rcd);
            int[] exceptionDays = new int[3] { 29, 30, 31 };
            if (exceptionDays.Contains(today.Day))
                today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 28);
            int todayYear = today.Year;
            int dayOfYear = today.DayOfYear;
            int dobYear = dob.Year;
            int dobDayOfYear = dob.DayOfYear;
            int calYears = todayYear - dobYear;
            int calDays = dayOfYear - dobDayOfYear;
            if ((calDays < 0) || (calDays == 0 && today < dob))
                calYears--;
            return calYears + 1;
        }
        public int GetCurrentAge(DateTime? dob, DateTime? RiskCommencementdate = null)
        {
            string Now = "";
            DateTime dt = RiskCommencementdate ?? DateTime.Now;

            DateTime DateOfBirth = DateTime.Now;
            if (dob != null)
            {
                DateOfBirth = Convert.ToDateTime(dob);
            }
            else
            {
                return 0;
            }
            string Dob = DateOfBirth.ToString("dd/MM/yyyy");

            //int Days = (DateTime.Now - DateOfBirth).Days;
            //return Days;            
            Now = dt.ToString("dd/MM/yyyy");
            var arr1 = Now.Split('-');
            var arr = Dob.Split('-');
            var birthYear = arr[2];
            var birthMonth = arr[1];
            var birthdate = arr[0];
            var Year = arr1[2];
            var Month = arr1[1];
            var date = arr1[0];
            var calYear = Convert.ToInt32(Year) - Convert.ToInt32(birthYear);
            var CalMonth = Convert.ToInt32(Month) - Convert.ToInt32(birthMonth);// monthDiff(BirthDate,dt); //              
            var calcAge = 0;
            if (CalMonth < 0 || (CalMonth == 0 && Convert.ToInt32(date) < Convert.ToInt32(birthdate)))
            {
                calYear--;
            }
            return calYear + 1;
        }
    }
}
