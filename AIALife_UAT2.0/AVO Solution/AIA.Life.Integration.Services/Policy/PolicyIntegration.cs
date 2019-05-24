using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using AIA.Life.Models.Integration;
using System.Net;
using Newtonsoft.Json;
using AIA.CrossCutting;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

using AIA.Life.Models.Integration.Services;
using AIA.Life.Models.Common;
using AIA.Life.Repository.AIAEntity;
using System.Data;

namespace AIA.Life.Integration.Services.Policy
{
    public class PolicyIntegration
    {
        
        //public AIA.Life.Models.Opportunity.LifeQuote CalculateQuotePremium(AIA.Life.Models.Opportunity.LifeQuote objLifeQuote)
        //{

        //    return objLifeQuote;
        //}
        //public AIA.Life.Models.Policy.Policy CalculateProposalPremium(AIA.Life.Models.Policy.Policy objLifePolicy)
        //{
        //    if (objLifePolicy != null)
        //    {
        //        int term = 0;
        //        var policyterm = term;
        //        if (objLifePolicy.PaymentTerm != null)
        //        {
        //            policyterm = Convert.ToInt32(objLifePolicy.PaymentTerm);
        //        }
        //        int? Age = objLifePolicy.objMemberDetails[0].Age;
        //        int? policyrate = (from obj in Contex.tblMasLifePricingRates
        //                           where (obj.Age == (Age + 1) && obj.PolicyPremiumTerm == policyterm)
        //                           select obj.Rate).FirstOrDefault();
        //        int basicSI = Convert.ToInt32(objLifePolicy.BasicSumInsured);
        //        var BasicPremium = ((policyrate) * (basicSI)) / 1000;
        //        int? BasicSpousePremium = 0;
        //        if (objLifePolicy.objProspectDetails.IsSpouseCoverd == true)
        //        {
        //            int? spouserate = (from obj in Contex.tblMasLifePricingRates
        //                               where (obj.Age == (objLifePolicy.objFillMemberDetials.Age + 1) && obj.PolicyPremiumTerm == policyterm)
        //                               select obj.Rate).FirstOrDefault();
        //            BasicSpousePremium = ((spouserate) * (basicSI)) / 1000;
        //            decimal occupationSpouseID = Convert.ToDecimal(objLifePolicy.objProspectDetails.OccupationID);
        //            var OccupationSpousetype = (from obj in Contex.tblMasLifeOccupations
        //                                        where obj.ID == occupationSpouseID
        //                                        select obj.ClassType).FirstOrDefault();
        //            if (OccupationSpousetype == "CLASS-I")
        //            {

        //            }
        //            else if (OccupationSpousetype == "CLASS-II")
        //            {


        //            }
        //            else if (OccupationSpousetype == "CLASS-III")
        //            {

        //            }
        //            else if (OccupationSpousetype == "CLASS-IV")
        //            {

        //            }
        //        }
        //        objLifePolicy.BasicPremium = Convert.ToString(BasicPremium);
        //        decimal occupationID = Convert.ToDecimal(objLifePolicy.objProspectDetails.OccupationID);
        //        var Occupationtype = (from obj in Contex.tblMasLifeOccupations
        //                              where obj.ID == occupationID
        //                              select obj.ClassType).FirstOrDefault();
        //        var AnnualPremium = 0;
        //        if (objLifePolicy.LstBenifitDetails != null)
        //        {
        //            foreach (var item in objLifePolicy.LstBenifitDetails)
        //            {
        //                if (item.BenifitOpted == true)
        //                {
        //                    int? BasePremium = 0;
        //                    if (Occupationtype == "CLASS-I")
        //                    {
        //                        int? SI = Convert.ToInt32(item.Suminsured);
        //                        BasePremium = ((policyrate) * (SI)) / 1000;
        //                    }
        //                    else if (Occupationtype == "CLASS-II")
        //                    {
        //                        if (item.BenifitName == "Life Additional Cover")
        //                        {
        //                            int? SI = Convert.ToInt32(item.Suminsured);
        //                            BasePremium = ((policyrate) * (SI)) / 1000;
        //                        }
        //                        else if (item.BenifitName == "Personal Accident Cover")
        //                        {
        //                            int? SI = Convert.ToInt32(item.Suminsured);
        //                            BasePremium = ((policyrate) * (SI)) / 1000;
        //                            BasePremium = Convert.ToInt32((1.5 * BasePremium));
        //                        }
        //                        else if (item.BenifitName == "Crtical Illness Cover")
        //                        {
        //                            int? SI = Convert.ToInt32(item.Suminsured);
        //                            BasePremium = ((policyrate) * (SI)) / 1000;
        //                            BasePremium = Convert.ToInt32((1.25 * BasePremium));
        //                        }
        //                        else if (item.BenifitName == "Hospitalization Daily Benifit")
        //                        {
        //                            int? SI = Convert.ToInt32(item.Suminsured);
        //                            BasePremium = ((policyrate) * (SI)) / 1000;
        //                            BasePremium = Convert.ToInt32((2 * BasePremium));
        //                        }
        //                        else if (item.BenifitName == "Hospitalization Reimbursement")
        //                        {
        //                            int? SI = Convert.ToInt32(item.Suminsured);
        //                            BasePremium = ((policyrate) * (SI)) / 1000;
        //                            BasePremium = Convert.ToInt32((1.5 * BasePremium));
        //                        }
        //                        else if (item.BenifitName == "Family Income Benifit")
        //                        {
        //                            int? SI = Convert.ToInt32(item.Suminsured);
        //                            BasePremium = ((policyrate) * (SI)) / 1000;
        //                            BasePremium = Convert.ToInt32((1.8 * BasePremium));
        //                        }
        //                        else if (item.BenifitName == "Funeral Benifit")
        //                        {
        //                            int? SI = Convert.ToInt32(item.Suminsured);
        //                            BasePremium = ((policyrate) * (SI)) / 1000;
        //                            BasePremium = Convert.ToInt32((1.9 * BasePremium));
        //                        }
        //                    }
        //                    else if (Occupationtype == "CLASS-III")
        //                    {
        //                        if (item.BenifitName == "Life Additional Cover")
        //                        {
        //                            int? SI = Convert.ToInt32(item.Suminsured);
        //                            BasePremium = ((policyrate) * (SI)) / 1000;
        //                            BasePremium = (BasePremium + 3);
        //                        }
        //                        else if (item.BenifitName == "Personal Accident Cover" || item.BenifitName == "Crtical Illness Cover" || item.BenifitName == "Hospitalization Daily Benifit" || item.BenifitName == "Hospitalization Reimbursement" || item.BenifitName == "Funeral Benifit" || item.BenifitName == "Family Income Benifit")
        //                        {
        //                            int? SI = Convert.ToInt32(item.Suminsured);
        //                            BasePremium = ((policyrate) * (SI)) / 1000;
        //                            BasePremium = Convert.ToInt32((2 * BasePremium));
        //                        }
        //                    }
        //                    else if (Occupationtype == "CLASS-IV")
        //                    {
        //                        if (item.BenifitName == "Life Additional Cover")
        //                        {
        //                            int? SI = Convert.ToInt32(item.Suminsured);
        //                            BasePremium = ((policyrate) * (SI)) / 1000;
        //                            BasePremium = (BasePremium + 4);
        //                        }
        //                        else if (item.BenifitName == "Personal Accident Cover" || item.BenifitName == "Crtical Illness Cover" || item.BenifitName == "Hospitalization Daily Benifit" || item.BenifitName == "Hospitalization Reimbursement" || item.BenifitName == "Funeral Benifit" || item.BenifitName == "Family Income Benifit")
        //                        {
        //                            BasePremium = 0;
        //                        }
        //                    }
        //                    item.Premium = Convert.ToString(BasePremium);
        //                    AnnualPremium = Convert.ToInt32(AnnualPremium + BasePremium);
        //                }
        //            }
        //            objLifePolicy.Cess=Convert.ToInt32((BasicPremium * 0.2) / 100);
        //            objLifePolicy.PolicyFee = 450;
        //            BasicPremium = Convert.ToInt32((BasicPremium) + ((BasicPremium * 0.2)/100));
        //            AnnualPremium = Convert.ToInt32((AnnualPremium + BasicPremium+450));
        //            objLifePolicy.AnnualPremium = Convert.ToString(AnnualPremium);
        //            objLifePolicy.HalfYearlyPremium = Convert.ToInt32(((AnnualPremium) * 0.5601));
        //            objLifePolicy.QuaterlyPremium = Convert.ToInt32((AnnualPremium * 0.2603));
        //            objLifePolicy.MonthlyPremium = Convert.ToInt32((AnnualPremium * 0.0883));

        //        }                
        //        long TotalSI = Convert.ToInt64(objLifePolicy.BasicSumInsured);
        //        if(Age >= 0 && Age <= 17 && TotalSI >= 1500001 && TotalSI <= 2500000)
        //        {
        //            var lstMedicalReports = (from obj in Contex.tblMasMedicalReports
        //                                               where obj.MedicalTest == "Cat 1"
        //                                               select obj.MedicalCertifications).ToList();
        //            objLifePolicy.LstMedicalReports = lstMedicalReports;
        //        }
        //       else if (Age >= 0 && Age <= 17 && TotalSI >= 2500000 && TotalSI <= 99999999999)
        //        {
        //            var lstMedicalReports = (from obj in Contex.tblMasMedicalReports
        //                                     where obj.MedicalTest == "Cat 1"
        //                                     select obj.MedicalCertifications).ToList();
        //            objLifePolicy.LstMedicalReports = lstMedicalReports;
        //        }
        //       else if (Age >= 18 && Age <= 24 && TotalSI >= 1500001 && TotalSI <= 2500000)
        //        {
        //            var lstMedicalReports = (from obj in Contex.tblMasMedicalReports
        //                                     where obj.MedicalTest == "Cat 2"
        //                                     select obj.MedicalCertifications).ToList();
        //            objLifePolicy.LstMedicalReports = lstMedicalReports;
        //        }
        //        else if (Age >= 18 && Age <= 24 && TotalSI >= 2500000 && TotalSI <= 99999999999)
        //        {
        //            var lstMedicalReports = (from obj in Contex.tblMasMedicalReports
        //                                     where obj.MedicalTest == "Cat 2"
        //                                     select obj.MedicalCertifications).ToList();
        //            objLifePolicy.LstMedicalReports = lstMedicalReports;
        //        }
        //        else if (Age >= 25 && Age <= 45 && TotalSI >= 1500001 && TotalSI <= 2500000)
        //        {
        //            var lstMedicalReports = (from obj in Contex.tblMasMedicalReports
        //                                     where obj.MedicalTest == "Cat 3"
        //                                     select obj.MedicalCertifications).ToList();
        //            objLifePolicy.LstMedicalReports = lstMedicalReports;
        //        }
        //        else if (Age >= 25 && Age <= 45 && TotalSI >= 2500000 && TotalSI <= 99999999999)
        //        {
        //            var lstMedicalReports = (from obj in Contex.tblMasMedicalReports
        //                                     where obj.MedicalTest == "Cat 4"
        //                                     select obj.MedicalCertifications).ToList();
        //            objLifePolicy.LstMedicalReports = lstMedicalReports;
        //        }
        //        else if (Age >= 46 && Age <= 55 && TotalSI >= 0 && TotalSI <= 1000000)
        //        {
        //            var lstMedicalReports = (from obj in Contex.tblMasMedicalReports
        //                                     where obj.MedicalTest == "Cat 3"
        //                                     select obj.MedicalCertifications).ToList();
        //            objLifePolicy.LstMedicalReports = lstMedicalReports;
        //        }
        //        else if (Age >= 46 && Age <= 55 && TotalSI >= 1000001 && TotalSI <= 1500000)
        //        {
        //            var lstMedicalReports = (from obj in Contex.tblMasMedicalReports
        //                                     where obj.MedicalTest == "Cat 4"
        //                                     select obj.MedicalCertifications).ToList();
        //            objLifePolicy.LstMedicalReports = lstMedicalReports;
        //        }
        //        else if (Age >= 46 && Age <= 55 && TotalSI >= 1500001 && TotalSI <= 2500000)
        //        {
        //            var lstMedicalReports = (from obj in Contex.tblMasMedicalReports
        //                                     where obj.MedicalTest == "Cat 5"
        //                                     select obj.MedicalCertifications).ToList();
        //            objLifePolicy.LstMedicalReports = lstMedicalReports;
        //        }
        //        else if (Age >= 46 && Age <= 55 && TotalSI >= 2500000 && TotalSI <= 99999999999)
        //        {
        //            var lstMedicalReports = (from obj in Contex.tblMasMedicalReports
        //                                     where obj.MedicalTest == "Cat 6"
        //                                     select obj.MedicalCertifications).ToList();
        //            objLifePolicy.LstMedicalReports = lstMedicalReports;
        //        }
        //        else if (Age >= 56 && Age <= 65 && TotalSI >= 0 && TotalSI <= 1000000)
        //        {
        //            var lstMedicalReports = (from obj in Contex.tblMasMedicalReports
        //                                     where obj.MedicalTest == "Cat 5"
        //                                     select obj.MedicalCertifications).ToList();
        //            objLifePolicy.LstMedicalReports = lstMedicalReports;
        //        }
        //        else if (Age >= 56 && Age <= 65 && TotalSI >= 1000001 && TotalSI <= 1500000)
        //        {
        //            var lstMedicalReports = (from obj in Contex.tblMasMedicalReports
        //                                     where obj.MedicalTest == "Cat 5"
        //                                     select obj.MedicalCertifications).ToList();
        //            objLifePolicy.LstMedicalReports = lstMedicalReports;
        //        }
        //        else if (Age >= 56 && Age <= 65 && TotalSI >= 1500001 && TotalSI <= 2500000)
        //        {
        //            var lstMedicalReports = (from obj in Contex.tblMasMedicalReports
        //                                     where obj.MedicalTest == "Cat 6"
        //                                     select obj.MedicalCertifications).ToList();
        //            objLifePolicy.LstMedicalReports = lstMedicalReports;
        //        }
        //        else if (Age >= 56 && Age <= 65 && TotalSI >= 2500000 && TotalSI <= 99999999999)
        //        {
        //            var lstMedicalReports = (from obj in Contex.tblMasMedicalReports
        //                                     where obj.MedicalTest == "Cat 6"
        //                                     select obj.MedicalCertifications).ToList();
        //            objLifePolicy.LstMedicalReports = lstMedicalReports;
        //        }
        //        var idParam = new SqlParameter
        //        {
        //            ParameterName = "age",
        //            Value = (objLifePolicy.objMemberDetails[0].Age + 1)
        //        };
        //        var idParam1 = new SqlParameter
        //        {
        //            ParameterName = "policyTerm",
        //            Value = Convert.ToInt32(objLifePolicy.PolicyTerm)
        //        };
        //        var idParam2 = new SqlParameter
        //        {
        //            ParameterName = "payTerm",
        //            Value = Convert.ToInt32(objLifePolicy.PaymentTerm)
        //        };
        //        var idParam3 = new SqlParameter
        //        {
        //            ParameterName = "paymentMode",
        //            Value = "cheque"
        //        };
        //        var idParam4 = new SqlParameter
        //        {
        //            ParameterName = "sA",
        //            Value = Convert.ToInt64(objLifePolicy.BasicSumInsured)
        //        };
        //        var idParam5 = new SqlParameter
        //        {
        //            ParameterName = "premium",
        //            Value = Convert.ToInt32(objLifePolicy.AnnualPremium)
        //        };
        //        var Result = Contex.Database.SqlQuery<AIA.Life.Models.Common.Illustation>(
        //          "exec usp_GetIllustration @age , @policyTerm,@payTerm,@paymentMode,@sA,@premium", idParam, idParam1, idParam2, idParam3, idParam4, idParam5).ToList();
        //        objLifePolicy.LstIllustation = Result;
        //    }
        //    return objLifePolicy;
        //}

        #region Premium Calculation
        
        //public RootObject MappingQuoteObjectToPremiumObject(ref AIA.Life.Models.Opportunity.LifeQuote objLifeQuote)
        //{

        //    objLifeQuote.ListAssured = new List<string>();
        //    RootObject objObject = new RootObject();
        //    int cntid = objLifeQuote.Contactid;
        //    AVOAIALifeEntities Context = new AVOAIALifeEntities();
        //    var ContactInfo = Context.tblContacts.Where(a => a.ContactID == cntid).FirstOrDefault();
        //    var AddressInfo = Context.tblAddresses.Where(a => a.AddressID == ContactInfo.AddressID).FirstOrDefault();
        //    objObject.planCode = objLifeQuote.objProductDetials.PlanCode;
        //    objObject.policyTerm = objLifeQuote.objProductDetials.PolicyTerm;
        //    objObject.frequency = objLifeQuote.objProductDetials.PreferredMode;//
        //    objObject.proposalDate = Convert.ToString(DateTime.Now.Date);
        //    objObject.postalCode = AddressInfo.Pincode;
        //    objObject.modeOfPayment = string.Empty;
        //    objObject.premiumpaymentTerm = objObject.policyTerm;


        //    if (objObject.planCode == "NJU4")
        //    {
        //        objObject.premiumpaymentTerm = objLifeQuote.objProductDetials.PremiumTerm;
        //    }

        //    #region  Main Life

        //    var MainLife = objLifeQuote.objQuoteMemberDetails.Where(a => a.Assured == "MainLife").FirstOrDefault();
        //    if (MainLife != null)
        //    {
        //        for (int i = 0; i < objLifeQuote.LstBenefitOverView.Count(); i++)
        //        {
        //            Models.Common.AssuredRelation objAssuredRelation = new Models.Common.AssuredRelation();
        //            objAssuredRelation.Assured_Name = MainLife.Assured;
        //            objAssuredRelation.Member_Relationship = MainLife.Relationship;
        //            objLifeQuote.LstBenefitOverView[i].objBenefitMemberRelationship.Add(objAssuredRelation);
        //        }


        //        objObject.mainInsured = new MainInsured();
        //        objObject.mainInsured.basicCover = new BasicCover();
        //        objObject.mainInsured.riderLevel = new List<RiderLevel>();

        //        int TotalMainLifeSI = 0;
        //        if (MainLife.Relationship == "267")
        //        {
        //            // Data will map from prospect Details
        //            objObject.mainInsured.age = Convert.ToString(objLifeQuote.objProspect.Age);
        //            objObject.mainInsured.basicCover.basicSumAssured = MainLife.BasicSumInsured;

        //            //  objObject.mainInsured.occupationCode = GetOccupationCode(Convert.ToInt32(ContactInfo.OccupationID));
        //            objObject.mainInsured.occupationCode = Convert.ToString(ContactInfo.OccupationID);
        //            objObject.mainInsured.monthlyIncome = GetMonthlyIncome(ContactInfo.MonthlyIncome);


        //        }
        //        else if (MainLife.Relationship == "268")
        //        {
        //            // Data will be map from Spouse details
        //            objObject.mainInsured.age = Convert.ToString(objLifeQuote.objSpouseDetials.Age);
        //            objObject.mainInsured.basicCover.basicSumAssured = MainLife.BasicSumInsured;
        //            objObject.mainInsured.occupationCode = objLifeQuote.objSpouseDetials.Occupation;
        //            objObject.mainInsured.monthlyIncome = string.Empty;
        //        }
        //        TotalMainLifeSI = Convert.ToInt32(MainLife.BasicSumInsured);


        //        int BasicCoverIndex = objLifeQuote.LstBenefitOverView.FindIndex(a => a.BenefitID == 0);
        //        int BasicCoverMemberIndex = objLifeQuote.LstBenefitOverView[BasicCoverIndex].objBenefitMemberRelationship.FindIndex(a => a.Assured_Name == MainLife.Assured);
        //        objLifeQuote.LstBenefitOverView[BasicCoverIndex].objBenefitMemberRelationship[BasicCoverMemberIndex].RiderSI = Convert.ToString(MainLife.BasicSumInsured);

        //        if (MainLife.ObjBenefitDetails != null)
        //        {
        //            foreach (var Rider in MainLife.ObjBenefitDetails.Where(a => a.BenifitOpted == true).ToList())
        //            {

        //                RiderLevel objrider1 = new RiderLevel();

        //                objrider1.riderCode = Rider.RiderCode;
        //                objrider1.minSumAssured = Rider.MinSumInsured;
        //                objrider1.maxSumAssured = Rider.MaxSumInsured;

        //                // To Skip Exception, Need To Change once changes are done in Service
        //                String s = Rider.RiderSuminsured;
        //                Double temp;
        //                Boolean isOk = Double.TryParse(s, out temp);
        //                Int32 value = isOk ? (Int32)temp : 0;
        //                objrider1.sumAssured = value;
        //                //objrider1.sumAssured = Convert.ToInt32(Rider.RiderSuminsured);
        //                //TotalMainLifeSI = TotalMainLifeSI + Convert.ToInt32(Rider.RiderSuminsured);
        //                TotalMainLifeSI = TotalMainLifeSI + value;
        //                int BenefitIndex = objLifeQuote.LstBenefitOverView.FindIndex(a => a.BenefitID == Rider.BenefitID);

        //                if (objLifeQuote.LstBenefitOverView[BenefitIndex].objBenefitMemberRelationship == null)
        //                {
        //                    objLifeQuote.LstBenefitOverView[BenefitIndex].objBenefitMemberRelationship = new List<Models.Common.AssuredRelation>();
        //                }
        //                int MemberAssuredIndex = objLifeQuote.LstBenefitOverView[BenefitIndex].objBenefitMemberRelationship.FindIndex(a => a.Assured_Name == MainLife.Assured);
        //                objLifeQuote.LstBenefitOverView[BenefitIndex].objBenefitMemberRelationship[MemberAssuredIndex].RiderSI = Rider.RiderSuminsured;

        //                objObject.mainInsured.riderLevel.Add(objrider1);
        //            }

        //        }
        //        objLifeQuote.ListAssured.Add(MainLife.Assured);

        //        // Set Total SI
        //        int TotalIndex = objLifeQuote.LstBenefitOverView.FindIndex(a => a.BenefitID == -2);
        //        int TotalMemberIndex = objLifeQuote.LstBenefitOverView[TotalIndex].objBenefitMemberRelationship.FindIndex(a => a.Assured_Name == "MainLife");
        //        objLifeQuote.LstBenefitOverView[TotalIndex].objBenefitMemberRelationship[TotalMemberIndex].RiderSI = Convert.ToString(TotalMainLifeSI);
        //        // till here
        //    }
        //    #endregion

        //    #region Spouse
        //    var Spouse = objLifeQuote.objQuoteMemberDetails.Where(a => a.Assured == "Spouse").FirstOrDefault();
        //    if (Spouse != null)
        //    {
        //        for (int i = 0; i < objLifeQuote.LstBenefitOverView.Count(); i++)
        //        {
        //            Models.Common.AssuredRelation objAssuredRelation = new Models.Common.AssuredRelation();
        //            objAssuredRelation.Assured_Name = Spouse.Assured;
        //            objAssuredRelation.Member_Relationship = Spouse.Relationship;
        //            objLifeQuote.LstBenefitOverView[i].objBenefitMemberRelationship.Add(objAssuredRelation);
        //        }
        //        objObject.spouse = new Spouse();
        //        objObject.spouse.basicCover = new BasicCover2();
        //        objObject.spouse.riderLevel = new List<RiderLevel2>();

        //        objObject.spouse.age = Convert.ToString(objLifeQuote.objSpouseDetials.Age);
        //        objObject.spouse.basicCover.basicSumAssured = Convert.ToString(Spouse.BasicSumInsured);
        //        int TotalSpouseLifeSI = Convert.ToInt32(Spouse.BasicSumInsured);
        //        objObject.spouse.occupationCode = objLifeQuote.objSpouseDetials.Occupation;
        //        objObject.spouse.monthlyIncome = string.Empty;

        //        int BasicCoverIndex = objLifeQuote.LstBenefitOverView.FindIndex(a => a.BenefitID == 0);
        //        int BasicCoverMemberIndex = objLifeQuote.LstBenefitOverView[BasicCoverIndex].objBenefitMemberRelationship.FindIndex(a => a.Assured_Name == Spouse.Assured);
        //        objLifeQuote.LstBenefitOverView[BasicCoverIndex].objBenefitMemberRelationship[BasicCoverMemberIndex].RiderSI = Convert.ToString(Spouse.BasicSumInsured);


        //        if (Spouse.ObjBenefitDetails != null)
        //        {
        //            foreach (var Rider in Spouse.ObjBenefitDetails.Where(a => a.BenifitOpted == true).ToList())
        //            {

        //                RiderLevel2 objrider1 = new RiderLevel2();
        //                objrider1.riderCode = Rider.RiderCode;
        //                objrider1.minSumAssured = Rider.MinSumInsured;
        //                objrider1.sumAssured = Convert.ToString(Rider.RiderSuminsured);
        //                objrider1.maxSumAssured = Rider.MaxSumInsured;
        //                TotalSpouseLifeSI = TotalSpouseLifeSI + Convert.ToInt32(Rider.RiderSuminsured);

        //                int BenefitIndex = objLifeQuote.LstBenefitOverView.FindIndex(a => a.BenefitID == Rider.BenefitID);

        //                int MemberAssuredIndex = objLifeQuote.LstBenefitOverView[BenefitIndex].objBenefitMemberRelationship.FindIndex(a => a.Assured_Name == Spouse.Assured);
        //                objLifeQuote.LstBenefitOverView[BenefitIndex].objBenefitMemberRelationship[MemberAssuredIndex].RiderSI = objrider1.sumAssured;
        //                objObject.spouse.riderLevel.Add(objrider1);
        //            }

        //        }
        //        objLifeQuote.ListAssured.Add(Spouse.Assured);

        //        // Set Total SI
        //        int TotalIndex = objLifeQuote.LstBenefitOverView.FindIndex(a => a.BenefitID == -2);
        //        int TotalMemberIndex = objLifeQuote.LstBenefitOverView[TotalIndex].objBenefitMemberRelationship.FindIndex(a => a.Assured_Name == Spouse.Assured);
        //        objLifeQuote.LstBenefitOverView[TotalIndex].objBenefitMemberRelationship[TotalMemberIndex].RiderSI = Convert.ToString(TotalSpouseLifeSI);
        //        // till here
        //    }

        //    #endregion

        //    #region Children
        //    objObject.children = new List<Child>();
        //    foreach (var ChildMembers in objLifeQuote.objQuoteMemberDetails.Where(a => a.Relationship == "269" || a.Relationship == "270").ToList())
        //    {
        //        for (int i = 0; i < objLifeQuote.LstBenefitOverView.Count(); i++)
        //        {
        //            Models.Common.AssuredRelation objAssuredRelation = new Models.Common.AssuredRelation();
        //            objAssuredRelation.Assured_Name = ChildMembers.Assured;
        //            objAssuredRelation.Member_Relationship = ChildMembers.Relationship;
        //            objLifeQuote.LstBenefitOverView[i].objBenefitMemberRelationship.Add(objAssuredRelation);
        //        }
        //        Child Child = new Child();
        //        Child.basicCover = new BasicCover3();
        //        Child.riderLevel = new List<RiderLevel3>();
        //        int TotalChildSI = 0;
        //        Child.age = Convert.ToString(ChildMembers.Age);

        //        Child.basicCover.basicSumAssured = ChildMembers.BasicSumInsured > 0 ? Convert.ToString(ChildMembers.BasicSumInsured) : "";
        //        Child.occupationCode = string.Empty;
        //        Child.monthlyIncome = string.Empty;

        //        int BasicCoverIndex = objLifeQuote.LstBenefitOverView.FindIndex(a => a.BenefitID == 0);
        //        int BasicCoverMemberIndex = objLifeQuote.LstBenefitOverView[BasicCoverIndex].objBenefitMemberRelationship.FindIndex(a => a.Assured_Name == ChildMembers.Assured);
        //        objLifeQuote.LstBenefitOverView[BasicCoverIndex].objBenefitMemberRelationship[BasicCoverMemberIndex].RiderSI = Convert.ToString(ChildMembers.BasicSumInsured);


        //        foreach (var Rider in ChildMembers.ObjBenefitDetails.Where(a => a.BenifitOpted == true).ToList())
        //        {
        //            RiderLevel3 objrider1 = new RiderLevel3();
        //            objrider1.riderCode = Rider.RiderCode;
        //            objrider1.minSumAssured = Rider.MinSumInsured;
        //            objrider1.sumAssured = Convert.ToString(Rider.RiderSuminsured);
        //            objrider1.maxSumAssured = Rider.MaxSumInsured;
        //            Child.riderLevel.Add(objrider1);
        //            TotalChildSI = TotalChildSI + Convert.ToInt32(Rider.RiderSuminsured);


        //            int BenefitIndex = objLifeQuote.LstBenefitOverView.FindIndex(a => a.BenefitID == Rider.BenefitID);


        //            int MemberAssuredIndex = objLifeQuote.LstBenefitOverView[BenefitIndex].objBenefitMemberRelationship.FindIndex(a => a.Assured_Name == ChildMembers.Assured);
        //            objLifeQuote.LstBenefitOverView[BenefitIndex].objBenefitMemberRelationship[MemberAssuredIndex].RiderSI = objrider1.sumAssured;


        //        }
        //        objObject.children.Add(Child);
        //        objLifeQuote.ListAssured.Add(ChildMembers.Assured);

        //        // Set Total SI
        //        int TotalIndex = objLifeQuote.LstBenefitOverView.FindIndex(a => a.BenefitID == -2);
        //        int TotalMemberIndex = objLifeQuote.LstBenefitOverView[TotalIndex].objBenefitMemberRelationship.FindIndex(a => a.Assured_Name == ChildMembers.Assured);
        //        objLifeQuote.LstBenefitOverView[TotalIndex].objBenefitMemberRelationship[TotalMemberIndex].RiderSI = Convert.ToString(TotalChildSI);
        //        // till here
        //    }

        //    #endregion

        //    return objObject;
        //}
        //public void MappingServiceResponse(ref AIA.Life.Models.Opportunity.LifeQuote objLifeQuote, AIA.Life.Models.Integration.PremiumResponse.RootObject objResponseObject)
        //{
        //    try
        //    {
        //        if (objResponseObject.status == "success")
        //        {

        //            if (objResponseObject.data != null)
        //            {
        //                if (objResponseObject.data.mainInsured != null)
        //                {
        //                    // To Set Basic Cover Premium
        //                    int BasicCoverIndex = objLifeQuote.LstBenefitOverView.FindIndex(a => a.BenefitID == 0);
        //                    int BasicCoverMemberIndex = objLifeQuote.LstBenefitOverView[BasicCoverIndex].objBenefitMemberRelationship.FindIndex(a => a.Assured_Name == "MainLife");
        //                    if (objResponseObject.data.mainInsured.basicCover != null)
        //                    {
        //                        objLifeQuote.LstBenefitOverView[BasicCoverIndex].objBenefitMemberRelationship[BasicCoverMemberIndex].Rider_Premium = objResponseObject.data.mainInsured.basicCover.purePremium;
        //                    }
        //                    // Till Here

        //                    // To Set Total Premium
        //                    int TotalIndex = objLifeQuote.LstBenefitOverView.FindIndex(a => a.BenefitID == -2);
        //                    int TotalMemberIndex = objLifeQuote.LstBenefitOverView[TotalIndex].objBenefitMemberRelationship.FindIndex(a => a.Assured_Name == "MainLife");

        //                    if (objLifeQuote.objProductDetials.PreferredMode == "1")
        //                    {
        //                        objLifeQuote.LstBenefitOverView[TotalIndex].objBenefitMemberRelationship[TotalMemberIndex].Rider_Premium = objResponseObject.data.mainInsured.annualPremium;
        //                    }
        //                    else if (objLifeQuote.objProductDetials.PreferredMode == "12")
        //                    {
        //                        objLifeQuote.LstBenefitOverView[TotalIndex].objBenefitMemberRelationship[TotalMemberIndex].Rider_Premium = objResponseObject.data.mainInsured.monthlyPremium;
        //                    }
        //                    else if (objLifeQuote.objProductDetials.PreferredMode == "4")
        //                    {
        //                        objLifeQuote.LstBenefitOverView[TotalIndex].objBenefitMemberRelationship[TotalMemberIndex].Rider_Premium = objResponseObject.data.mainInsured.quaterlyPremium;
        //                    }
        //                    else if (objLifeQuote.objProductDetials.PreferredMode == "2")
        //                    {
        //                        objLifeQuote.LstBenefitOverView[TotalIndex].objBenefitMemberRelationship[TotalMemberIndex].Rider_Premium = objResponseObject.data.mainInsured.halfYearlyPremium;
        //                    }


        //                    // Till Here

        //                    foreach (var Rider in objResponseObject.data.mainInsured.riderLevel)
        //                    {
        //                        if (Rider != null)
        //                        {
        //                            int BenefitIndex = objLifeQuote.LstBenefitOverView.FindIndex(a => a.RiderCode == Rider.riderCode);
        //                            if (BenefitIndex >= 0)
        //                            {
        //                                int MemberAssuredIndex = objLifeQuote.LstBenefitOverView[BenefitIndex].objBenefitMemberRelationship.FindIndex(a => a.Assured_Name == "MainLife");
        //                                objLifeQuote.LstBenefitOverView[BenefitIndex].objBenefitMemberRelationship[MemberAssuredIndex].Rider_Premium = Rider.purePremium;
        //                            }
        //                        }

        //                    }
        //                }


        //                if (objResponseObject.data.spouse != null)
        //                {
        //                    int BasicCoverIndex = objLifeQuote.LstBenefitOverView.FindIndex(a => a.BenefitID == 0);
        //                    int BasicCoverMemberIndex = objLifeQuote.LstBenefitOverView[BasicCoverIndex].objBenefitMemberRelationship.FindIndex(a => a.Assured_Name == "Spouse");
        //                    if (BasicCoverMemberIndex >= 0)
        //                    {

        //                        if (objResponseObject.data.spouse.basicCover != null)
        //                        {
        //                            objLifeQuote.LstBenefitOverView[BasicCoverIndex].objBenefitMemberRelationship[BasicCoverMemberIndex].Rider_Premium = objResponseObject.data.spouse.basicCover.purePremium;
        //                        }


        //                        // To Set Total Premium
        //                        int TotalIndex = objLifeQuote.LstBenefitOverView.FindIndex(a => a.BenefitID == -2);
        //                        int TotalMemberIndex = objLifeQuote.LstBenefitOverView[TotalIndex].objBenefitMemberRelationship.FindIndex(a => a.Assured_Name == "Spouse");
        //                        if (objLifeQuote.objProductDetials.PreferredMode == "1")
        //                        {
        //                            objLifeQuote.LstBenefitOverView[TotalIndex].objBenefitMemberRelationship[TotalMemberIndex].Rider_Premium = objResponseObject.data.spouse.annualPremium;
        //                        }
        //                        else if (objLifeQuote.objProductDetials.PreferredMode == "12")
        //                        {
        //                            objLifeQuote.LstBenefitOverView[TotalIndex].objBenefitMemberRelationship[TotalMemberIndex].Rider_Premium = objResponseObject.data.spouse.monthlyPremium;
        //                        }
        //                        else if (objLifeQuote.objProductDetials.PreferredMode == "4")
        //                        {
        //                            objLifeQuote.LstBenefitOverView[TotalIndex].objBenefitMemberRelationship[TotalMemberIndex].Rider_Premium = objResponseObject.data.spouse.quaterlyPremium;
        //                        }
        //                        else if (objLifeQuote.objProductDetials.PreferredMode == "2")
        //                        {
        //                            objLifeQuote.LstBenefitOverView[TotalIndex].objBenefitMemberRelationship[TotalMemberIndex].Rider_Premium = objResponseObject.data.spouse.halfYearlyPremium;
        //                        }


        //                        // Till Here
        //                        foreach (var Rider in objResponseObject.data.spouse.riderLevel)
        //                        {
        //                            if (Rider != null)
        //                            {
        //                                int BenefitIndex = objLifeQuote.LstBenefitOverView.FindIndex(a => a.RiderCode == Rider.riderCode);
        //                                if (BenefitIndex >= 0)
        //                                {
        //                                    int MemberAssuredIndex = objLifeQuote.LstBenefitOverView[BenefitIndex].objBenefitMemberRelationship.FindIndex(a => a.Assured_Name == "Spouse");
        //                                    objLifeQuote.LstBenefitOverView[BenefitIndex].objBenefitMemberRelationship[MemberAssuredIndex].Rider_Premium = Rider.purePremium;
        //                                }
        //                            }

        //                        }
        //                    }



        //                }



        //                if (objResponseObject.data.children != null)
        //                {
        //                    int ChildCount = 0;
        //                    foreach (AIA.Life.Models.Integration.PremiumResponse.Child Child in objResponseObject.data.children)
        //                    {
        //                        ChildCount++;
        //                        string _AssuredName = "Child" + ChildCount;
        //                        int BasicCoverIndex = objLifeQuote.LstBenefitOverView.FindIndex(a => a.BenefitID == 0);
        //                        int BasicCoverMemberIndex = objLifeQuote.LstBenefitOverView[BasicCoverIndex].objBenefitMemberRelationship.FindIndex(a => a.Assured_Name == _AssuredName);

        //                        if (BasicCoverMemberIndex >= 0)
        //                        {
        //                            if (Child.basicCover != null)
        //                            {
        //                                objLifeQuote.LstBenefitOverView[BasicCoverIndex].objBenefitMemberRelationship[BasicCoverMemberIndex].Rider_Premium = Child.basicCover.purePremium;
        //                            }


        //                            // To Set Total Premium
        //                            int TotalIndex = objLifeQuote.LstBenefitOverView.FindIndex(a => a.BenefitID == -2);
        //                            int TotalMemberIndex = objLifeQuote.LstBenefitOverView[TotalIndex].objBenefitMemberRelationship.FindIndex(a => a.Assured_Name == _AssuredName);

        //                            if (objLifeQuote.objProductDetials.PreferredMode == "1")
        //                            {
        //                                objLifeQuote.LstBenefitOverView[TotalIndex].objBenefitMemberRelationship[TotalMemberIndex].Rider_Premium = Child.annualPremium;
        //                            }
        //                            else if (objLifeQuote.objProductDetials.PreferredMode == "12")
        //                            {
        //                                objLifeQuote.LstBenefitOverView[TotalIndex].objBenefitMemberRelationship[TotalMemberIndex].Rider_Premium = Child.monthlyPremium;
        //                            }
        //                            else if (objLifeQuote.objProductDetials.PreferredMode == "4")
        //                            {
        //                                objLifeQuote.LstBenefitOverView[TotalIndex].objBenefitMemberRelationship[TotalMemberIndex].Rider_Premium = Child.quaterlyPremium;
        //                            }
        //                            else if (objLifeQuote.objProductDetials.PreferredMode == "2")
        //                            {
        //                                objLifeQuote.LstBenefitOverView[TotalIndex].objBenefitMemberRelationship[TotalMemberIndex].Rider_Premium = Child.halfYearlyPremium;
        //                            }


        //                            // Till Here
        //                            foreach (var Rider in Child.riderLevel)
        //                            {
        //                                if (Rider != null)
        //                                {
        //                                    int BenefitIndex = objLifeQuote.LstBenefitOverView.FindIndex(a => a.RiderCode == Rider.riderCode);
        //                                    if (BenefitIndex >= 0)
        //                                    {
        //                                        int MemberAssuredIndex = objLifeQuote.LstBenefitOverView[BenefitIndex].objBenefitMemberRelationship.FindIndex(a => a.Assured_Name == _AssuredName);
        //                                        objLifeQuote.LstBenefitOverView[BenefitIndex].objBenefitMemberRelationship[MemberAssuredIndex].Rider_Premium = Rider.purePremium;
        //                                    }
        //                                }

        //                            }
        //                        }


        //                    }


        //                }

        //                objLifeQuote.AnnualPremium = objResponseObject.data.annualPremium;
        //                objLifeQuote.HalfYearlyPremium = objResponseObject.data.halfYearlyPremium;
        //                objLifeQuote.QuaterlyPremium = objResponseObject.data.quaterlyPremium;
        //                objLifeQuote.MonthlyPremium = objResponseObject.data.monthlyPremium;
        //                objLifeQuote.VAT = objResponseObject.data.vat;
        //                objLifeQuote.Cess = objResponseObject.data.cess;
        //                objLifeQuote.PolicyFee = objResponseObject.data.policyAdminFee;
        //            }

        //            // objLifeQuote.LstPremiumOverview.AddRange(objLifeQuote.LstBenefitOverView);
        //            objLifeQuote.Message = "Success";
        //        }
        //        else
        //        {
        //            objLifeQuote.Message = "Error";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objLifeQuote.Message = "Error";

        //    }

        //}

        #endregion

        public List<string> FetchMedicalTests(AIA.Life.Models.Common.MemberDetails objMemberDetails, string ProductName)
        {
            AIA.Life.Models.Integration.Services.MedicalTestRequest objMedicaltestRequest = new AIA.Life.Models.Integration.Services.MedicalTestRequest();
            objMedicaltestRequest.age = Convert.ToString(objMemberDetails.Age);
            objMedicaltestRequest.sumInsured = objMemberDetails.BasicSumInsured;
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            objMedicaltestRequest.productCode = Context.tblProducts.Where(a => a.ProductName == ProductName).FirstOrDefault().ProductCode;

            string result = GetPostParametersToAPI("LifeNB", "medicalTest", objMedicaltestRequest);
            Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
            
            return new List<string>();
        }
       

        public string GetOccupationCode(int ID)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                var val = Context.tblMasLifeOccupations.Where(a => a.ID == ID).FirstOrDefault().Code;
                return Convert.ToString(val);
                // return "1";
            }
            catch (Exception)
            {

                return string.Empty;
            }
        }
        public string GetMonthlyIncome(string Value)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                int ID = Convert.ToInt32(Value);
                return Context.tblMasCommonTypes.Where(a => a.MasterType == "MonthlyIncomeRange" && a.CommonTypesID == ID).FirstOrDefault().Description;
            }
            catch (Exception)
            {

                return string.Empty;
            }
        }



        public string GetCodeForMasterItem(string ID)
        {

            try
            {
                if (!string.IsNullOrEmpty(ID))
                {
                    int _id = Convert.ToInt32(ID);
                    AVOAIALifeEntities Context = new AVOAIALifeEntities();
                    return Context.tblMasCommonTypes.Where(a => a.CommonTypesID == _id).FirstOrDefault().Code;
                }
                else
                    return string.Empty;
                
            }
            catch (Exception)
            {

                return string.Empty;
            }
        }

        public static string GetPostParametersToAPI(string controllerName, string MethodName, object obj, int? ContactID = null)
        {
            try
            {
                string _url = "http://52.163.51.32:8090/";
                _url = _url + "ngrestapi/" + controllerName + "/" + MethodName;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
                request.Method = "POST";
                request.ContentType = "application/json";
                string requestData = JsonConvert.SerializeObject(obj);
                if (MethodName == "workiteminitiate")
                {
                    var settings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };
                    requestData = JsonConvert.SerializeObject(obj, settings);
                }

                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] data = encoder.GetBytes(requestData);
                //data = Encoding.UTF8.GetBytes(objObject.ConvertToXML());
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                //response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                SaveServiceResuestAndResponse(requestData, result, MethodName, ContactID);
                return result;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public static void SaveServiceResuestAndResponse(string Request, string Response, string MethodName, int? ContactID = null)
        {
            try
            {
                using (AVOAIALifeEntities Entity = new AVOAIALifeEntities())
                {
                    tblServiceLog tblServiceLogs = new tblServiceLog();
                    tblServiceLogs.ContactID = ContactID;
                    tblServiceLogs.Request = Request;
                    tblServiceLogs.Response = Response;
                    tblServiceLogs.MethodName = MethodName;
                    Entity.tblServiceLogs.Add(tblServiceLogs);
                    Entity.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
        }

    
        public AIA.Life.Models.Policy.Policy PushProposalInfoToNewGen(AIA.Life.Models.Policy.Policy objPolicy)
        {

            AIA.Life.Models.Integration.Services.ProposalRequest objObject = MapProposalInfoToNewGenRequestObject(objPolicy);
            string result = GetPostParametersToAPI("work", "workiteminitiate", objObject, objPolicy.ContactID);
            AIA.Life.Models.Integration.Services.ProposalResponse objProposalResponse = new AIA.Life.Models.Integration.Services.ProposalResponse();
            Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
            objProposalResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<AIA.Life.Models.Integration.Services.ProposalResponse>(result, settings);
            if (objProposalResponse.status == "success")
            {
                objPolicy.Message = "Success";
                objPolicy.ProposalNo = objProposalResponse.data;
                try
                {
                    using (AVOAIALifeEntities Entity = new AVOAIALifeEntities())
                    {
                        var tblpolicy = Entity.tblPolicies.Where(a => a.QuoteNo == objPolicy.QuoteNo).FirstOrDefault();
                        if (tblpolicy != null)
                        {
                            tblpolicy.ProposalNo = objPolicy.ProposalNo;
                            tblpolicy.PolicyStageStatusID = 193;
                            Entity.SaveChanges();
                        }
                    }
                }
                catch (Exception)
                {


                }
            }
            else
            {
                objPolicy.Message = "Error";
            }
            return objPolicy;
        }
        public AIA.Life.Models.Integration.Services.ProposalRequest MapProposalInfoToNewGenRequestObject(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Models.Integration.Services.ProposalRequest objRequest = new AIA.Life.Models.Integration.Services.ProposalRequest();

            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            var QuoteInfo = Context.tblLifeQQs.Where(a => a.QuoteNo == objPolicy.QuoteNo).FirstOrDefault();
            Agent objAgentInfo = new Agent();
            if (!string.IsNullOrEmpty(objPolicy.UserName))
            {
                objAgentInfo= FetchAgentBranch(objPolicy.UserName);
            }
            if (QuoteInfo != null)
            {
                #region BasicInfo


                objRequest.Quotation_Number = objPolicy.QuoteNo;
                objRequest.Quotation_Date = Convert.ToString(DateTime.Now.Date);
                objRequest.Plan_Name = objPolicy.PlanName;
                objRequest.Proposal_Number = string.Empty;
                if (objPolicy.PlanName != null)
                {
                    objRequest.Plan_Code = Context.tblProducts.Where(a => a.ProductName == objPolicy.PlanName).FirstOrDefault().ProductCode;
                }
                objRequest.Policy_Term = objPolicy.PolicyTerm;
                //"Loan_Amount": "",
                //"Interest_Rate": "",
                objRequest.ProposalDate = Convert.ToString(DateTime.Now.Date);


                #endregion


                #region QProposalReg
                objRequest.q_proposal_reg = new Models.Integration.Services.QProposalReg();
                if (objAgentInfo != null)
                {
                    objRequest.q_proposal_reg.AGENT_CODE = objAgentInfo.AgentCode;
                    objRequest.q_proposal_reg.AGENT_NAME = string.Empty;
                    objRequest.q_proposal_reg.BRANCH = objAgentInfo.BranchName;
                }
                else
                {
                    objRequest.q_proposal_reg.AGENT_CODE = "AGE000000000158"; // test
                    objRequest.q_proposal_reg.AGENT_NAME = string.Empty;
                    objRequest.q_proposal_reg.BRANCH = "SIF";  // test

                }
               
                objRequest.q_proposal_reg.BUSINESS_TYPE = string.Empty;
                objRequest.q_proposal_reg.CLIENT_CODE = string.Empty;
                objRequest.q_proposal_reg.COMPANY_CODE = "003";
                objRequest.q_proposal_reg.DOB = Convert.ToString(objPolicy.objProspectDetails.DateOfBirth);
                objRequest.q_proposal_reg.FIRST_NAME = objPolicy.objProspectDetails.FirstName;
                objRequest.q_proposal_reg.LAST_NAME = objPolicy.objProspectDetails.LastName;
                objRequest.q_proposal_reg.MIDDLE_NAME = objPolicy.objProspectDetails.MiddleName;
                objRequest.q_proposal_reg.MODE_OFPAYMENT = objPolicy.PaymentMethod;
                objRequest.q_proposal_reg.NAME_WITH_INITIAL = objPolicy.objProspectDetails.NameWithInitial;
                objRequest.q_proposal_reg.PREMIUM = Convert.ToString(objPolicy.Premium);
                objRequest.q_proposal_reg.PREMIUM_PAYINGTERM = objPolicy.PaymentTerm;
                #endregion

                #region QProposerdetail

                objRequest.q_proposerdetail = new Models.Integration.Services.QProposerdetail();
                objRequest.q_proposerdetail.EXISTING_CLIENT = string.Empty;
                objRequest.q_proposerdetail.NIC_NUMBER = objPolicy.objProspectDetails.NewNICNO;
                objRequest.q_proposerdetail.CLIENT_CODE = string.Empty;
                objRequest.q_proposerdetail.SALUATION = GetCodeForMasterItem (objPolicy.objProspectDetails.Salutation);
                objRequest.q_proposerdetail.FIRST_NAME = objPolicy.objProspectDetails.FirstName;
                objRequest.q_proposerdetail.MIDDLE_NAME = objPolicy.objProspectDetails.MiddleName;
                objRequest.q_proposerdetail.SUR_NAME = objPolicy.objProspectDetails.LastName;
                objRequest.q_proposerdetail.NAME_WITHINITIALS = objPolicy.objProspectDetails.NameWithInitial;
                objRequest.q_proposerdetail.GENDER = GetCodeForMasterItem(objPolicy.objProspectDetails.Gender);
                objRequest.q_proposerdetail.AGE = Convert.ToString(objPolicy.objProspectDetails.Age);
                objRequest.q_proposerdetail.UNIT = string.Empty;
                objRequest.q_proposerdetail.WUNITS = string.Empty;
                objRequest.q_proposerdetail.MARTIAL_STATUS = GetCodeForMasterItem(objPolicy.objProspectDetails.MaritialStatus);
                objRequest.q_proposerdetail.OLD_NICNO = objPolicy.objProspectDetails.OLDNICNo;
                objRequest.q_proposerdetail.NEW_NICNO = objPolicy.objProspectDetails.NewNICNO;
                objRequest.q_proposerdetail.OCCUPATION = Convert.ToString(objPolicy.objProspectDetails.OccupationID);
                objRequest.q_proposerdetail.COMPANY_NAME = objPolicy.objProspectDetails.CompanyName;
                objRequest.q_proposerdetail.NATURE_OFDUTIES = objPolicy.objProspectDetails.NameOfDuties;
                objRequest.q_proposerdetail.MONTHLY_INCOME = objPolicy.objProspectDetails.MonthlyIncome;
                objRequest.q_proposerdetail.NATIONALITY = objPolicy.objProspectDetails.Nationality;
                objRequest.q_proposerdetail.m_NO = string.Empty;
                objRequest.q_proposerdetail.m_TWO = string.Empty;
                objRequest.q_proposerdetail.w_UNIT = string.Empty;
                objRequest.q_proposerdetail.BIRTHDATE = Convert.ToString(objPolicy.objProspectDetails.DateOfBirth);
                objRequest.q_proposerdetail.CADDRESS1 = objPolicy.objProspectDetails.objCommunicationAddress.Address1;
                objRequest.q_proposerdetail.CADDRESS2 = objPolicy.objProspectDetails.objCommunicationAddress.Address2;
                objRequest.q_proposerdetail.CCITY = objPolicy.objProspectDetails.objCommunicationAddress.City;
                objRequest.q_proposerdetail.CDISTRICT = objPolicy.objProspectDetails.objCommunicationAddress.District;
                objRequest.q_proposerdetail.CDISTRICT = objPolicy.objProspectDetails.objCommunicationAddress.Province;
                objRequest.q_proposerdetail.CPOSTCODE = objPolicy.objProspectDetails.objCommunicationAddress.Pincode;
                objRequest.q_proposerdetail.EMAIL = objPolicy.objProspectDetails.Email;
                objRequest.q_proposerdetail.ADDRESS1 = objPolicy.objProspectDetails.objPermenantAddress.Address1;
                objRequest.q_proposerdetail.ADDRESS2 = objPolicy.objProspectDetails.objPermenantAddress.Address2;
                objRequest.q_proposerdetail.CITY = objPolicy.objProspectDetails.objPermenantAddress.City;
                objRequest.q_proposerdetail.DISTRICT = objPolicy.objProspectDetails.objPermenantAddress.District;
                objRequest.q_proposerdetail.PROVINCE = objPolicy.objProspectDetails.objPermenantAddress.Province;
                objRequest.q_proposerdetail.POSTCODE = objPolicy.objProspectDetails.objPermenantAddress.Pincode;
                objRequest.q_proposerdetail.WORK = objPolicy.objProspectDetails.WorkNumber;
                objRequest.q_proposerdetail.SAMEADDRESS = objPolicy.objProspectDetails.IsRegAsCommunication == true ? "True" : "False";

                #endregion



                #region MemberDetails

                objRequest.Q_NG_LNB_MEMBER_DETAIL = new Models.Integration.Services.QNGLNBMEMBERDETAIL();
                objRequest.Q_NG_LNB_MEMBER_DETAIL.MEMBERGRID = new List<MEMBERGRID>();
                foreach (var Member in objPolicy.objMemberDetails)
                {

                    MEMBERGRID objMember = new MEMBERGRID();
                    objMember.Existing_Client = string.Empty;
                    objMember.MemberType = Member.AssuredName;
                    objMember.ClientCode = string.Empty;// Need to fill
                    objMember.Relationship_with_Proposer = GetCodeForMasterItem( Member.RelationShipWithPropspect);
                    objMember.Saluation = GetCodeForMasterItem(Member.Salutation);
                    objMember.First_Name = Member.FirstName;
                    objMember.Middle_Name = Member.MiddleName;
                    objMember.Sur_Name = Member.LastName;
                    objMember.CONTRIBUTION = string.Empty;
                    objMember.Name_with_initial = Member.NameWithInitial;
                    objMember.Date_o_Birth = Convert.ToString(Member.DateOfBirth);
                    objMember.Age = Convert.ToString(Member.Age);
                    objMember.Height = string.Empty;
                    objMember.Unit = string.Empty;
                    objMember.Weight = string.Empty;
                    objMember.W_Unit = string.Empty;
                    objMember.Marital_Status = GetCodeForMasterItem(Member.MaritialStatus);
                    objMember.Old_NIC_Number = Member.OLDNICNo;
                    objMember.New_NIC_Number = Member.NewNICNO;
                    objMember.Occupation = Convert.ToString(Member.OccupationID);
                    objMember.Company_Name = Member.CompanyName;
                    objMember.Nature_of_duties = Member.NameOfDuties;
                    if (Member.MonthlyIncome != null)
                    {
                        objMember.Monthly_Income = GetMonthlyIncome(Member.MonthlyIncome);
                    }

                    objMember.BMI = string.Empty;
                    objMember.M_NO = Member.MobileNo;
                    objMember.HOME = Member.HomeNumber;
                    objMember.WORk = Member.WorkNumber;
                    objMember.EMAIL = Member.Email;

                    objMember.ADDRESS1 = Member.objCommunicationAddress.Address1;
                    objMember.ADDRTESS2 = Member.objCommunicationAddress.Address2;

                    objMember.PROVINCE = Member.objCommunicationAddress.State;
                    objMember.DISTRICT = Member.objCommunicationAddress.District;
                    objMember.CITY = Member.objCommunicationAddress.City;
                    objMember.POSTCODE = Member.objCommunicationAddress.Pincode;
                    objMember.SAME_ADDRESS = Convert.ToString(Member.IsRegAsCommunication);
                    objMember.P_ADDRESS = Member.objPermenantAddress.Address1;
                    objMember.P_ADDRESS2 = Member.objPermenantAddress.Address2;
                    objMember.P_DISTRICT = Member.objPermenantAddress.District;
                    objMember.P_POSTCODE = Member.objPermenantAddress.Pincode;
                    objMember.P_PROVINCE = Member.objPermenantAddress.State;
                    objMember.W_DATE = Convert.ToString(Member.WeddingAnniversaryDate);
                    objMember.M_TWO = string.Empty;

                    objRequest.Q_NG_LNB_MEMBER_DETAIL.MEMBERGRID.Add(objMember);
                }


                #endregion

                #region Rider Details
                objRequest.QRiderDetails = new QRiderDetails();
                objRequest.QRiderDetails.EntityGrid = new List<EntityGrid2>();
                foreach (var Member in objPolicy.objMemberDetails)
                {
                    EntityGrid2 objEntityGrid = new EntityGrid2();
                    objEntityGrid.EntityName = Member.AssuredName;
                    objEntityGrid.RiderGrid = new List<RiderGrid>();
                    foreach (var Rider in Member.objBenifitDetails)
                    {                     
                            RiderGrid objRider = new RiderGrid();
                            objRider.RiderCode = Rider.RiderCode;
                            objRider.SumAssured = Rider.RiderSuminsured;
                            objRider.LoadingGrid = new List<LoadingGrid>();
                            objEntityGrid.RiderGrid.Add(objRider);
                    }

                    objRequest.QRiderDetails.EntityGrid.Add(objEntityGrid);
                }
                #endregion

                #region QRECIEPT
                objRequest.Q_RECIEPT = new QRECIEPT();
                if (objPolicy.objPaymentInfo != null)
                {

                    if (objPolicy.objPaymentInfo.objInstrumentDetails != null && objPolicy.objPaymentInfo.objInstrumentDetails.Count() > 0)
                    {

                        objRequest.Q_RECIEPT.CLIENT_CODE = string.Empty;
                        objRequest.Q_RECIEPT.FIRST_NAME = objPolicy.objPaymentInfo.objInstrumentDetails[0].Name;
                        objRequest.Q_RECIEPT.MIDDLE_NAME = string.Empty;
                        objRequest.Q_RECIEPT.PAYMENT_MODE = objPolicy.objPaymentInfo.objInstrumentDetails[0].PaymentMode;
                        objRequest.Q_RECIEPT.PLAN = string.Empty;
                        objRequest.Q_RECIEPT.PROPOSAL_DATE = Convert.ToString(DateTime.Now.Date);
                        objRequest.Q_RECIEPT.PROPOSAL_NUMBER = string.Empty;
                        objRequest.Q_RECIEPT.SUR_NAME = string.Empty;
                    }
                }

                #endregion

                #region QExclusion

                objRequest.q_exclusion = new QExclusion();
                objRequest.q_exclusion.APPLY = string.Empty;
                objRequest.q_exclusion.CODE = string.Empty;
                objRequest.q_exclusion.DESCRIPTION = string.Empty;
                objRequest.q_exclusion.DETAILS = string.Empty;
                objRequest.q_exclusion.EXCLUSIONGRID = new List<Models.Integration.Services.EXCLUSIONGRID>();
                #endregion

                #region QTERMSANDCOND
                objRequest.Q_TERMSANDCOND = new QTERMSANDCOND();
                objRequest.Q_TERMSANDCOND.TERMSANDCONDGRID = new List<TERMSANDCONDGRID>();
                #endregion

                objRequest.document = new List<Document>();
                objRequest.q_child_assured = new QChildAssured();

                #region Premium


                objRequest.q_premium = new QPremium();
                objRequest.q_premium.CESS = Convert.ToString(objPolicy.Cess);
                objRequest.q_premium.CLIENT_PREMIUM = Convert.ToString(objPolicy.Premium);
                objRequest.q_premium.CLIENT_PREMIUM_FOREIGN_CURRENCY = string.Empty;

                objRequest.q_premium.CONTRIBUTION = string.Empty;
                objRequest.q_premium.IF_OTHER = objPolicy.others;
                objRequest.q_premium.POLICY_FEE = Convert.ToString(objPolicy.PolicyFee);
                objRequest.q_premium.PREMIUM = Convert.ToString(objPolicy.Premium);

                objRequest.q_premium.PREMIUM_FOREIGN_CURRENCY = string.Empty;
                objRequest.q_premium.PREMIUM_PAID_BY = objPolicy.PaymentPaidBy;
                objRequest.q_premium.PREMIUM_PAYMENT_TERM = objPolicy.PaymentTerm;
                objRequest.q_premium.PREMIUM_PAY_METHOD = objPolicy.PaymentMethod;
                objRequest.q_premium.PREMIUM_RECEIPT_BY = objPolicy.PaymentReceiptPrefferdBy;
                objRequest.q_premium.VAT = Convert.ToString(objPolicy.VAT);
                #endregion

                #region Questionnaire
                objRequest.Q_Question = new QQuestion();
                objRequest.Q_Question.EntityGrid = new List<EntityGrid>();
                foreach (var Member in objPolicy.objMemberDetails)
                {
                    EntityGrid objEntityGrid = new EntityGrid();
                    objEntityGrid.EntityName = Member.FirstName;
                    objEntityGrid.EntityType = Member.AssuredName;
                    objEntityGrid.Questions = new List<Question>();
                    #region LifeStyle Questions
                    if (Member.objLifeStyleQuetions != null)
                    {
                        if (Member.Questions != null)
                        {
                            foreach (var LifeStyleQuestion in Member.Questions)
                            {
                                Question objQuestion = new Question();
                                objQuestion.QId = Convert.ToString(LifeStyleQuestion.QuestionID);
                                objQuestion.QType = "LifeStyle";
                                objQuestion.Value = LifeStyleQuestion.Answer;
                                objQuestion.SubAnswer = LifeStyleQuestion.SubAnswer;
                                objEntityGrid.Questions.Add(objQuestion);
                            }
                        }
                    }
                    #endregion

                    #region Medical Questions
                    if (Member.objLstMedicalHistory != null)
                    {
                        foreach (var MedicalHistoryQuestion in Member.objLstMedicalHistory)
                        {
                            Question objQuestion = new Question();
                            objQuestion.QId = Convert.ToString(MedicalHistoryQuestion.QuestionID);
                            objQuestion.QType = "MedicalHistory";
                            objQuestion.Value = MedicalHistoryQuestion.Answer;
                            objQuestion.SubAnswer = MedicalHistoryQuestion.SubAnswer;
                            objEntityGrid.Questions.Add(objQuestion);
                        }

                    }
                    #endregion

                    #region Additional Questions
                    if (Member.objAdditionalQuestions != null)
                    {
                        foreach (var AdditionalQuestion in Member.objAdditionalQuestions)
                        {
                            Question objQuestion = new Question();
                            objQuestion.QId = Convert.ToString(AdditionalQuestion.QuestionID);
                            objQuestion.QType = "AdditionalQuestions";
                            objQuestion.Value = AdditionalQuestion.Answer;
                            objQuestion.SubAnswer = AdditionalQuestion.SubAnswer;
                            objEntityGrid.Questions.Add(objQuestion);
                        }

                    }
                    #endregion

                    #region Family BackGround
                    if (Member.objLstFamily != null)
                    {
                        foreach (var FamilyQuestion in Member.objLstFamily)
                        {
                            Question objQuestion = new Question();
                            objQuestion.QId = Convert.ToString(FamilyQuestion.QuestionID);
                            objQuestion.QType = "FamilyBackGround";
                            objQuestion.Value = FamilyQuestion.Answer;
                            objQuestion.SubAnswer = FamilyQuestion.SubAnswer;
                            objEntityGrid.Questions.Add(objQuestion);
                        }

                    }
                    //objEntityGrid.FamilyBackground = new List<FamilyBackground>();
                    //if (Member.objLstFamilyBackground != null)
                    //{
                    //    foreach (var FamilyBackGround in Member.objLstFamilyBackground)
                    //    {
                    //        FamilyBackground objFamilyBackGround = new FamilyBackground();
                    //        objFamilyBackGround.Relationship = GetCodeForMasterItem(FamilyBackGround.FamilyPersonType);
                    //        objFamilyBackGround.PresentAge = Convert.ToString(FamilyBackGround.PresentAge);
                    //        objFamilyBackGround.StateofHealth = Convert.ToString(FamilyBackGround.StateOfHealth);
                    //        if (FamilyBackGround.PresentAge > 0)
                    //        {
                    //            objFamilyBackGround.Alive = "YES";
                    //        }
                    //        else
                    //        {
                    //            objFamilyBackGround.Alive = "NO";
                    //        }

                    //        objFamilyBackGround.AgeatDeath = Convert.ToString(FamilyBackGround.AgeAtDeath);
                    //        objFamilyBackGround.CauseofDeath = FamilyBackGround.Cause;
                    //        objFamilyBackGround.Detailsofpoorhealth = FamilyBackGround.Details;
                    //        objEntityGrid.FamilyBackground.Add(objFamilyBackGround);

                    //    }
                    //}
                    #endregion

                    #region Previous Insurance Info
                    if (Member.objLstOtherInsuranceDetails != null)
                    {
                        foreach (var InsuranceQuestion in Member.objLstOtherInsuranceDetails)
                        {
                            Question objQuestion = new Question();
                            objQuestion.QId = Convert.ToString(InsuranceQuestion.QuestionID);
                            objQuestion.QType = "PreviousAndCurrentLifeInsurance";
                            objQuestion.Value = InsuranceQuestion.Answer;
                            objQuestion.SubAnswer = InsuranceQuestion.SubAnswer;
                            objEntityGrid.Questions.Add(objQuestion);
                        }

                    }

                    //objEntityGrid.PrevandCurrLifeIns = new PrevandCurrLifeIns();
                    //if (Member.objLifeAssuredOtherInsurance != null && Member.objLifeAssuredOtherInsurance.Count() > 0)
                    //{

                    //    objEntityGrid.PrevandCurrLifeIns.NoofPoliciesJS = Convert.ToString(Member.NoofJsPolicies);
                    //    if (Member.NoofOtherPolicies > 0)
                    //    {
                    //        objEntityGrid.PrevandCurrLifeIns.otherinspolicy = "YES";
                    //        objEntityGrid.PrevandCurrLifeIns.noofotherpolicies = Convert.ToString(Member.NoofOtherPolicies);
                    //    }
                    //    objEntityGrid.PrevandCurrLifeIns.Otherdetail = new List<Models.Integration.Services.Otherdetail>();

                    //    foreach (var OtherInsuranceInfo in Member.objLifeAssuredOtherInsurance)
                    //    {
                    //        Otherdetail objOtherDetails = new Otherdetail();
                    //        objOtherDetails.CompanyName = OtherInsuranceInfo.CompanyName;
                    //        objOtherDetails.policyPropNo = OtherInsuranceInfo.PolicyNo;
                    //        objOtherDetails.totalSAforDeath = OtherInsuranceInfo.TotalSAAtDeath;
                    //        objOtherDetails.accBenefitAmt = OtherInsuranceInfo.AccidentalBenefitAmount;
                    //        objOtherDetails.criticalIllnessBenefit = OtherInsuranceInfo.CriticalIllnessBenefit;
                    //        objOtherDetails.hospDailyBenefit = OtherInsuranceInfo.HospitalizationPerDay;
                    //        objOtherDetails.hospReimbBenefitAmt = OtherInsuranceInfo.HospitalizationReimbursement;
                    //        objOtherDetails.currStatus = OtherInsuranceInfo.CurrentStatus;
                    //        objOtherDetails.comments = string.Empty;
                    //        objEntityGrid.PrevandCurrLifeIns.Otherdetail.Add(objOtherDetails);
                    //    }

                    //}

                    #endregion


                    objRequest.Q_Question.EntityGrid.Add(objEntityGrid);
                }

                #endregion

            }

            return objRequest;
        }


        public Agent FetchAgentBranch(string UserName)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
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
            catch (Exception ex)
            {

                return null;
            }
        }
        //public string TestAPI()
        //{
        //    AIA.Life.Models.Opportunity.LifeQuote objLifePolicy = new AIA.Life.Models.Opportunity.LifeQuote();
        //    RootObject objObject = MappingQuoteObjectToPremiumObject(ref objLifePolicy);
        //    string str = GetPostParametersToAPI("LifeNB", "premium", objObject);
        //    return str;
        //}


    }
}