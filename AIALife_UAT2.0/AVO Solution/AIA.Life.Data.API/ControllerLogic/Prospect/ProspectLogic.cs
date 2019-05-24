using AIA.Life.Repository.AIAEntity;
using AIA.Life.Models.Policy;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AIA.Life.Models;
using AIA.Life.Models.Opportunity;
using AIA.Life.Models.NeedAnalysis;
using System.Configuration;
using AIA.Life.Models.Common;
//using AIA.Life.Integration.Services.SamsIntegration;
using System.Text;
using AIA.Life.Integration.Services.EmailandSMS;
using AIA.Life.Models.EmailSMSDetails;
using AIA.CrossCutting;

namespace AIA.Life.Data.API.ControllerLogic.Prospect
{
    public class ProspectLogic
    {
        public List<SuspectPool> GetSuspectPool(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            string userId = Common.CommonBusiness.GetUserId(objProspect.CreatedBy);

            AVOAIALifeEntities Context = new AVOAIALifeEntities();

            List<SuspectPool> list = (from Opportunity in Context.tblOpportunities.Where(a => a.StageID == 1 && a.IsDeleted != true && a.Createdby == userId)
                                      join Contact in Context.tblContacts.Where(b => b.CreatedBy == userId)
                                      on Opportunity.ContactID equals Contact.ContactID
                                      orderby Contact.CreationDate descending
                                      select new SuspectPool
                                      {
                                          ContactId = Contact.ContactID,
                                          SuspectId = Contact.ContactID,
                                          SuspectType = Contact.ContactType,
                                          SuspectName = Contact.FirstName,
                                          NIC = Contact.NICNO,
                                          LeadNo = Contact.LeadNo,
                                          Salutation = Context.tblMasCommonTypes.Where(a => a.Code == Contact.Title).Select(b => b.Description).FirstOrDefault(),
                                          Place = Contact.Place,
                                          LeadDate = Contact.CreationDate.ToString(),
                                          SuspectLastName = Contact.LastName,
                                          SuspectMobile = Contact.MobileNo,
                                          SuspectWork = Contact.Work,
                                          SuspectEmail = Contact.EmailID,
                                          FullName= Context.tblMasCommonTypes.Where(a => a.Code == Contact.Title).Select(b => b.ShortDesc).FirstOrDefault()+" "+ Contact.FirstName+" "+ Contact.LastName,


                                      }).ToList();
            foreach (var obj in list)
            {
                obj.LeadDate = obj.LeadDate.ToDate().ToString("dd/MM/yyyy");
            }

            return list;
        }

        //Step 1 to check if NIC avialable
        public bool GetNICValidate(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            bool found = (from Opportunity in Context.tblContacts.Where(a => a.NICNO == objProspect.NIC) select Opportunity).Any();

            if (found == true)
            {
                objProspect.NICAVAIL = true;
                GetDetailsOnExist(objProspect.NICAVAIL, objProspect);

            }

            else
            {
                objProspect.NICAVAIL = false;
            }
            return found;
        }

        //Step 1 - a to check if NIC avialable Quote
        public bool GetNICValidateQuote(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            bool found = (from Opportunity in Context.tblContacts.Where(a => a.NICNO == objProspect.NIC) select Opportunity).Any();

            if (found == true)
            {
                objProspect.NICAVAIL = true;
                GetDetailsOnExistQuote(objProspect.NICAVAIL, objProspect);

            }
            else
            {
                objProspect.NICAVAIL = false;
            }
            return found;
        }
        //15-5-2018 -step 2 for Yes if nic avialable
        public void GetDetailsFromPolicyCLientIL(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            using (AVOAIALifeEntities cnt = new AVOAIALifeEntities())
            {
                var a = cnt.tblMasILClients.Where(b => b.SECUITYNO == objProspect.NIC).FirstOrDefault();

                if (a != null)
                {
                    objProspect.NIC = a.SECUITYNO != null ? a.SECUITYNO.Trim() : "";
                    objProspect.ClientCode = a.CLNTNUM != null ? a.CLNTNUM.Trim() : "";
                    objProspect.Email = a.RINTERNET != null ? a.RINTERNET.Trim() : "";
                    objProspect.Salutation = cnt.tblMasCommonTypes.Where(b => b.Code == (a.SALUTL != null ? a.SALUTL.Trim() : "") && b.MasterType == "Salutation").Select(c => c.Description).FirstOrDefault();
                    objProspect.Mobile = a.RMBLPHONE != null ? a.RMBLPHONE.Trim() : "";
                    objProspect.LastName = a.SURNAMES != null ? a.SURNAMES.Trim() : "";
                    objProspect.MaritalStatus = a.MARRYD != null ? a.MARRYD.Trim() : "";
                    objProspect.Name = string.IsNullOrEmpty(a.LGIVNAME) == true ? (a.GIVNAME != null ? a.GIVNAME.Trim() : "") : a.LGIVNAME.Trim();
                    objProspect.Home = a.CLTPHONE01 != null ? a.CLTPHONE01.Trim() : "";
                    objProspect.Work = a.CLTPHONE02 != null ? a.CLTPHONE02.Trim() : "";
                    objProspect.Occupation = cnt.tblMasLifeOccupations.Where(b => b.CompanyCode == (a.OCCPCODE != null ? a.OCCPCODE.Trim() : "")).Select(c => c.OccupationCode + "|" + c.SinhalaDesc + "|" + c.TamilDesc).FirstOrDefault(); ;
                    objProspect.objAddress.Address1 = a.CLTADDR02 != null ? a.CLTADDR02.Trim() : "";
                    objProspect.objAddress.Address2 = a.CLTADDR03 != null ? a.CLTADDR03.Trim() : "";
                    objProspect.objAddress.Address3 = a.CLTADDR04 != null ? a.CLTADDR04.Trim() : "";
                    var addres = cnt.tblMasCityDistrictProvinces.Where(p => p.PostalCode == (a.CLTPCODE != null ? a.CLTPCODE.Trim() : "")).FirstOrDefault();
                    if (addres != null)
                    {
                        objProspect.objAddress.Pincode = addres.PostalCode + "|" + addres.CityName;
                        objProspect.objAddress.District = addres.DistrictName;
                        objProspect.objAddress.Province = addres.ProvinceName;
                    }
                    objProspect.Message = "Success";
                    objProspect.NICAVAIL = true;
                }
                else
                {
                    objProspect.NICAVAIL = false;
                    objProspect.Message = "No Records are there for Given Nic";
                }
            }
        }
        public void GetDetailsOnExist(bool found, AIA.Life.Models.Opportunity.Prospect objProspect)
        {

            if (found == true)
            {
                using (AVOAIALifeEntities cnt = new AVOAIALifeEntities())
                {
                    var a = (from Contact in cnt.tblContacts.Where(b => b.NICNO == objProspect.NIC)
                             orderby Contact.CreationDate descending
                             select new
                             {
                                 Contact.NICNO,
                                 Contact.FirstName,
                                 Contact.EmailID,
                                 Contact.Title,
                                 Contact.MobileNo,
                                 Contact.LastName,
                                 Contact.Place,
                                 Contact.Work,
                                 Contact.PhoneNo,
                                 Contact.OccupationID,
                                 Contact.tblAddress.Address1,
                                 Contact.tblAddress.Address2,
                                 Contact.tblAddress.Address3,
                                 Contact.tblAddress.Pincode,
                                 Contact.tblAddress.City,
                                 Contact.tblAddress.State,
                                 Contact.tblAddress.District,
                                 Contact.MonthlyIncome,
                                 Contact.PassportNo,
                                 Contact.MaritalStatusID
                             }).FirstOrDefault();
                    if (a != null)
                    {
                        objProspect.NIC = a.NICNO;
                        objProspect.Email = a.EmailID;
                        objProspect.Salutation = cnt.tblMasCommonTypes.Where(b => b.Code == a.Title).Select(c => c.Description).FirstOrDefault();
                        objProspect.Mobile = a.MobileNo;
                        objProspect.LastName = a.LastName;
                        objProspect.Place = a.Place;
                        objProspect.Name = a.FirstName;
                        objProspect.Work = a.Work;
                        objProspect.Home = a.PhoneNo;
                        string companycode = Convert.ToString(a.OccupationID);
                        //objProspect.Occupation = cnt.tblMasLifeOccupations.Where(b => b.CompanyCode == companycode).Select(c => c.OccupationCode).FirstOrDefault();
                        objProspect.Occupation = cnt.tblMasLifeOccupations.Where(b => b.CompanyCode == companycode).Select(b => b.OccupationCode + "|" + b.SinhalaDesc + "|" + b.TamilDesc).FirstOrDefault();
                        objProspect.objAddress.Address1 = a.Address1;
                        objProspect.objAddress.Address2 = a.Address2;
                        objProspect.objAddress.Address3 = a.Address3;
                        if (!string.IsNullOrEmpty(a.Pincode))
                        {
                            objProspect.objAddress.Pincode = a.Pincode + '|' + a.City;
                        }
                        objProspect.objAddress.Province = a.State;
                        objProspect.objAddress.District = a.District;
                        int maritalstatus = Convert.ToInt32(a.MaritalStatusID);
                        objProspect.MaritalStatus = cnt.tblMasCommonTypes.Where(b => b.CommonTypesID == maritalstatus).Select(c => c.Code).FirstOrDefault();
                        objProspect.AvgMonthlyIncome = a.MonthlyIncome;

                    }
                }

            }

        }
        public void GetDetailsOnExistQuote(bool found, AIA.Life.Models.Opportunity.Prospect objProspect)
        {

            if (found == true)
            {
                using (AVOAIALifeEntities cnt = new AVOAIALifeEntities())
                {
                    var a = (from Contact in cnt.tblContacts.Where(b => b.NICNO == objProspect.NIC)
                             orderby Contact.CreationDate descending
                             select new
                             {
                                 Contact.NICNO,
                                 Contact.FirstName,
                                 Contact.EmailID,
                                 Contact.Title,
                                 Contact.MobileNo,
                                 Contact.LastName,
                                 Contact.Place,
                                 Contact.Work,
                                 Contact.PhoneNo,
                                 Contact.OccupationID,
                                 Contact.tblAddress.Address1,
                                 Contact.tblAddress.Address2,
                                 Contact.tblAddress.Address3,
                                 Contact.tblAddress.Pincode,
                                 Contact.tblAddress.City,
                                 Contact.tblAddress.State,
                                 Contact.tblAddress.District,
                                 Contact.MonthlyIncome,
                                 Contact.PassportNo,
                                 Contact.MaritalStatusID
                             }).FirstOrDefault();
                    if (a != null)
                    {
                        objProspect.NIC = a.NICNO;
                        objProspect.Email = a.EmailID;
                        objProspect.Salutation = cnt.tblMasCommonTypes.Where(b => b.Code == a.Title).Select(c => c.Description).FirstOrDefault();
                        objProspect.Mobile = a.MobileNo;
                        objProspect.LastName = a.LastName;
                        objProspect.Place = a.Place;
                        objProspect.Name = a.FirstName;
                        objProspect.Work = a.Work;
                        objProspect.Home = a.PhoneNo;
                        string companycode = Convert.ToString(a.OccupationID);
                        if (companycode != "0")
                            objProspect.Occupation = cnt.tblMasLifeOccupations.Where(b => b.CompanyCode == companycode).Select(c => c.OccupationCode + "|" + c.SinhalaDesc + "|" + c.TamilDesc).FirstOrDefault();
                        objProspect.objAddress.Address1 = a.Address1;
                        objProspect.objAddress.Address2 = a.Address2;
                        objProspect.objAddress.Address3 = a.Address3;
                        if (!string.IsNullOrEmpty(a.Pincode))
                        {
                            objProspect.objAddress.Pincode = a.Pincode + '|' + a.City;
                        }
                        objProspect.objAddress.Province = a.State;
                        objProspect.objAddress.District = a.District;
                        int maritalstatus = Convert.ToInt32(a.MaritalStatusID);
                        objProspect.MaritalStatus = cnt.tblMasCommonTypes.Where(b => b.CommonTypesID == maritalstatus).Select(c => c.Code).FirstOrDefault();
                        objProspect.AvgMonthlyIncome = a.MonthlyIncome;

                    }
                }

            }

        }
        public List<SuspectPool> GetAllocateSuspect(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            string userId = Common.CommonBusiness.GetUserId(objProspect.CreatedBy);

            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            decimal nodeid = Context.tblUserDetails.Where(a => a.LoginID == objProspect.CreatedBy).Select(b => b.NodeID).FirstOrDefault();
            int nodeId = Convert.ToInt32(nodeid);
            var Count = 1;
            List<SuspectPool> list = (from Opportunity in Context.tblOpportunities.Where(a => a.StageID == 1 && a.IsDeleted != true && a.Createdby == userId)
                                      join Contact in Context.tblContacts.Where(b => b.CreatedBy == userId)
                                      on Opportunity.ContactID equals Contact.ContactID
                                      orderby Contact.CreationDate descending
                                      select new SuspectPool
                                      {
                                          ContactId = Contact.ContactID,
                                          SuspectId = Contact.ContactID,
                                          SuspectType = Contact.ContactType,
                                          SuspectName = Contact.FirstName,
                                          NIC = Contact.NICNO,
                                          SuspectMobile = Contact.MobileNo,
                                          SuspectEmail = Contact.EmailID,
                                          Passport = Contact.PassportNo,
                                          FullName = Context.tblMasCommonTypes.Where(a => a.Code == Contact.Title).Select(b => b.ShortDesc).FirstOrDefault() + " " + Contact.FirstName + " " + Contact.LastName,
                                          lstAssignedTo = Context.tblUserDetails.Where(a => a.UserParentId == nodeId && a.Status == true).Select(a => new MasterListItem
                                          {
                                              Text = a.LoginID,
                                              Value = a.LoginID
                                          }).ToList(),
                                          RowId = Count + 1

                                      }).ToList();
            return list;
        }

        public List<SuspectReAllocation> GetSuspectReAllocation()
        {
            List<SuspectReAllocation> objLstSuspectReAllocation = new List<SuspectReAllocation>();
            objLstSuspectReAllocation.Add(new SuspectReAllocation { SuspectReAllocationId = 1, SuspectReAllocationType = "Type1", SuspectReAllocationName = "SuspectRe1", SuspectReAllocationMobile = 80508965, SuspectReAllocationHome = "Home1", SuspectReAllocationWork = "Work1", SuspectReAllocationEmail = "Email1", SuspectReAllocationDecision = "Decision", SuspectReAllocationReportee = "Reportee" });
            objLstSuspectReAllocation.Add(new SuspectReAllocation { SuspectReAllocationId = 2, SuspectReAllocationType = "Type2", SuspectReAllocationName = "SuspectRe2", SuspectReAllocationMobile = 80529965, SuspectReAllocationHome = "Home2", SuspectReAllocationWork = "Work2", SuspectReAllocationEmail = "Email1", SuspectReAllocationDecision = "Decision", SuspectReAllocationReportee = "Reportee" });
            objLstSuspectReAllocation.Add(new SuspectReAllocation { SuspectReAllocationId = 3, SuspectReAllocationType = "Type3", SuspectReAllocationName = "SuspectRe3", SuspectReAllocationMobile = 80829965, SuspectReAllocationHome = "Home2", SuspectReAllocationWork = "Work2", SuspectReAllocationEmail = "Email1", SuspectReAllocationDecision = "Decision", SuspectReAllocationReportee = "Reportee" });
            objLstSuspectReAllocation.Add(new SuspectReAllocation { SuspectReAllocationId = 4, SuspectReAllocationType = "Type4", SuspectReAllocationName = "SuspectRe4", SuspectReAllocationMobile = 80829965, SuspectReAllocationHome = "Home2", SuspectReAllocationWork = "Work2", SuspectReAllocationEmail = "Email1", SuspectReAllocationDecision = "Decision", SuspectReAllocationReportee = "Reportee" });
            objLstSuspectReAllocation.Add(new SuspectReAllocation { SuspectReAllocationId = 5, SuspectReAllocationType = "Type5", SuspectReAllocationName = "SuspectRe5", SuspectReAllocationMobile = 80508965, SuspectReAllocationHome = "Home2", SuspectReAllocationWork = "Work2", SuspectReAllocationEmail = "Email1", SuspectReAllocationDecision = "Decision", SuspectReAllocationReportee = "Reportee" });
            return objLstSuspectReAllocation;
        }

        public List<ProspectPool> GetProspectPool(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            string userId = Common.CommonBusiness.GetUserId(objProspect.CreatedBy);
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            List<ProspectPool> objLstProspectPool = (from Opportunity in Context.tblOpportunities.Where(a => a.StageID == objProspect.ProspectStage && a.IsDeleted != true && a.Createdby == userId)
                                                     join Contact in Context.tblContacts.Where(b => b.CreatedBy == userId)
                                                     on Opportunity.ContactID equals Contact.ContactID
                                                     orderby Contact.ContactID descending
                                                     select new ProspectPool
                                                     {
                                                         ProspectId = Contact.ContactID,
                                                         ProspectType = Contact.ContactType,
                                                         ProspectName = Contact.FirstName,
                                                         ProspectLastName = Contact.LastName,
                                                         Salutation = Context.tblMasCommonTypes.Where(a => a.Code == Contact.Title).Select(b => b.Description).FirstOrDefault(),
                                                         ProspectMobile = Contact.MobileNo,
                                                         ProspectHome = Contact.PhoneNo,
                                                         ProspectWork = Contact.Work,
                                                         ProspectEmail = Contact.EmailID,
                                                         ProspectNicNo = Contact.NICNO,
                                                         LeadNo = Contact.LeadNo,
                                                         Place = Contact.Place,
                                                         LeadDate = Contact.CreationDate.ToString(),
                                                         // Dob = Contact.DateOfBirth.ToString().ToDate().ToString("dd/MM/yyyy"),
                                                         Dob = Contact.DateOfBirth.ToString(),
                                                         ProspectDaysleft = 3,
                                                         FullName= Context.tblMasCommonTypes.Where(a => a.Code == Contact.Title).Select(b => b.ShortDesc).FirstOrDefault()+" "+ Contact.FirstName+" "+ Contact.LastName

                                                     }).ToList();

            foreach (var obj in objLstProspectPool)
            {
                obj.Dob = obj.Dob.ToDate().ToString("dd/MM/yyyy");
                obj.LeadDate = obj.LeadDate.ToDate().ToString("dd/MM/yyyy");
            }

            return objLstProspectPool;

        }

        public AIA.Life.Models.Opportunity.Suspect SaveSuspect(AIA.Life.Models.Opportunity.Suspect objSuspect)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                tblContact objContact = null;
                if (!string.IsNullOrEmpty(objSuspect.SamsLeadNumber))
                    objContact = Context.tblContacts.Where(a => a.LeadNo == objSuspect.SamsLeadNumber).FirstOrDefault();
                if (objContact == null)
                    objContact = new tblContact();

                string userId = Common.CommonBusiness.GetUserId(objSuspect.CreatedBy);

                objContact.CreatedBy = userId;
                int type = Context.tblMasCommonTypes.Where(a => a.Description == objSuspect.Type && (a.MasterType == "Type" || a.MasterType == "BancaType")).Select(a => a.CommonTypesID).FirstOrDefault();
                if (type == 0)
                {
                    type = Context.tblMasCommonTypes.Where(a => a.Code == objSuspect.Type && a.MasterType == "Type").Select(a => a.CommonTypesID).FirstOrDefault();
                    if (type == 0)
                        type = Convert.ToInt32(objSuspect.Type);
                }

                objContact.ContactType = Context.tblMasCommonTypes.Where(a => a.CommonTypesID == type).Select(a => a.Description).FirstOrDefault();
                objContact.FirstName = objSuspect.Name;
                objContact.LastName = objSuspect.LastName;
                objContact.Work = objSuspect.Work;
                objContact.MobileNo = objSuspect.Mobile;
                objContact.PhoneNo = objSuspect.Home;
                objContact.EmailID = objSuspect.Email;
                objContact.NICNO = objSuspect.NIC;
                if (!string.IsNullOrEmpty(objSuspect.SamsLeadNumber))
                {
                    objContact.ContactType = Context.tblMasCommonTypes.Where(a => a.Code == objSuspect.Type && (a.MasterType == "Type")).Select(a => a.Description).FirstOrDefault();
                    objContact.LeadNo = objSuspect.SamsLeadNumber;
                    objContact.Title = objSuspect.Title;
                }
                else
                    objContact.Title = Context.tblMasCommonTypes.Where(a => a.Description == objSuspect.Title && a.MasterType == "Salutation").Select(a => a.Code).FirstOrDefault();
                if (string.IsNullOrEmpty(objContact.Title))
                    objContact.Title = objSuspect.Title;
                objContact.Place = objSuspect.Place;
                objContact.CreationDate = DateTime.Now;
                objContact.ClientCode = objSuspect.ClientCode;
                objContact.PassportNo = objSuspect.Passport;
                if (!string.IsNullOrEmpty(objSuspect.SamsLeadNumber))
                    objContact.LeadNo = objSuspect.SamsLeadNumber;
                if (!string.IsNullOrEmpty(objSuspect.IntroducerCode))
                {
                    objContact.IntroducerCode = objSuspect.IntroducerCode;
                }
                if (objContact.ContactID == 0)
                {
                    Context.tblContacts.Add(objContact);
                    tblOpportunity objOppurtunity = new tblOpportunity();
                    tblOpportunityHistory objOpportunityHistory = new tblOpportunityHistory();
                    objOppurtunity.tblContact = objContact;
                    objOppurtunity.StageID = 1; // Suspect
                    objOppurtunity.Createdby = userId;
                    objOpportunityHistory.StageID = 1;
                    objOpportunityHistory.OpportunityID = objOppurtunity.OppurtunityID;
                    objOpportunityHistory.CreatedDate = DateTime.Now;
                    Context.tblOpportunities.Add(objOppurtunity);
                    Context.tblOpportunityHistories.Add(objOpportunityHistory);
                }

                Context.SaveChanges();
                objSuspect.ContactID = objContact.ContactID;
                objSuspect.LeadCreationDate = Convert.ToDateTime(objContact.CreationDate);
                objSuspect.Message = "Success";
                //SamsClient samsClient = new SamsClient();
                //if (string.IsNullOrEmpty(objSuspect.SamsLeadNumber))
                //{
                //    samsClient.CreateLead(Context, objSuspect);
                //    var lead = Context.tblContacts.Where(a => a.ContactID == objSuspect.ContactID).FirstOrDefault();
                //    lead.LeadNo = objSuspect.SamsLeadNumber;
                //    Context.SaveChanges();
                //}
                //if (string.IsNullOrEmpty(objSuspect.SamsLeadNumber))
                //{
                //    objSuspect.Error.ErrorMessage = objSuspect.Message = "Sorry, system is unable to connect with the SAMS. You will get the lead number before creating the proposal. Please proceed.....";
                //}
                return objSuspect;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public AIA.Life.Models.Opportunity.Prospect SaveProspect(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                tblContact objContact = Context.tblContacts.Where(a => a.ContactID == objProspect.ContactID).FirstOrDefault();
                tblOpportunity objOppurtunity = Context.tblOpportunities.Where(a => a.ContactID == objProspect.ContactID).FirstOrDefault();
                bool IsProspect = true;
                bool IsSuspectComplete = false;
                bool IsProductSelected = false;
                tblAddress objAddress = new tblAddress();
                int type = Context.tblMasCommonTypes.Where(a => a.Description == objProspect.Type && (a.MasterType == "Type" || a.MasterType == "BancaType")).Select(a => a.CommonTypesID).FirstOrDefault();
                if (type == 0)
                {
                    type = Context.tblMasCommonTypes.Where(a => a.Code == objProspect.Type && a.MasterType == "Type").Select(a => a.CommonTypesID).FirstOrDefault();
                    if (type == 0)
                        type = Convert.ToInt32(objProspect.Type);
                }

                objContact.ContactType = Context.tblMasCommonTypes.Where(a => a.CommonTypesID == type).Select(a => a.Description).FirstOrDefault();
                objProspect.Salutation = Context.tblMasCommonTypes.Where(a => a.Description == objProspect.Salutation && a.MasterType == "Salutation").Select(a => a.Code).FirstOrDefault();
                objContact.Title = objProspect.Salutation;
                objContact.FirstName = objProspect.Name;
                objContact.LastName = objProspect.LastName;


                if (string.IsNullOrEmpty(objProspect.LastName))
                {
                    IsProspect = false;
                }
                if (string.IsNullOrEmpty(objProspect.Salutation))
                {
                    IsProspect = false;
                }
                //if (string.IsNullOrEmpty(objProspect.Work) && string.IsNullOrEmpty(objProspect.Mobile) && string.IsNullOrEmpty(objProspect.Home) && string.IsNullOrEmpty(objProspect.Email))
                if (string.IsNullOrEmpty(objProspect.Mobile))
                {
                    IsProspect = false;
                }
                objContact.Work = objProspect.Work;
                objContact.MobileNo = objProspect.Mobile;
                objContact.PhoneNo = objProspect.Home;
                objContact.EmailID = objProspect.Email;
                objContact.NICNO = objProspect.NIC;
                objContact.PassportNo = objProspect.PassPort;
                if (string.IsNullOrEmpty(objProspect.MaritalStatus))
                {
                    IsProspect = false;
                }
                objContact.MaritalStatusID = Context.tblMasCommonTypes.Where(a => a.Code == objProspect.MaritalStatus && a.MasterType == "MaritalStatus").Select(b => b.CommonTypesID).FirstOrDefault();

                objContact.SpouseName = objProspect.objNeedAnalysis.objSpouseDetails.FullName;
                objContact.SpouseDOB = objProspect.objNeedAnalysis.objSpouseDetails.DateOfBirth;
                objContact.SpouseAge = objProspect.objNeedAnalysis.objSpouseDetails.AgeNextBirthday;
                objContact.CurrentAge = objProspect.objNeedAnalysis.objSpouseDetails.CurrentAge;
                objContact.DependenceCount = objProspect.objNeedAnalysis.DependantCount;

                List<tblDependant> objDependants = Context.tblDependants.Where(x => x.ContactID == objProspect.ContactID).ToList();
                foreach (var item in objDependants)
                {
                    objContact.tblDependants.Remove(item);

                }
                foreach (var item in objProspect.objNeedAnalysis.objDependants)
                {
                    tblDependant objDependant = new tblDependant();
                    objDependant.DependantName = item.Name;
                    objDependant.DependantDOB = item.DOB;
                    objDependant.DependantAge = item.AgeNextBirthday;
                    objDependant.DependantRelation = item.Relationship;
                    objDependant.ContactID = objProspect.ContactID;
                    objContact.tblDependants.Add(objDependant);

                }


                // objContact.Gender = Convert.ToString(GetGenderStatus(objProspect.Gender));
                objContact.Gender = objProspect.Gender;
                if (string.IsNullOrEmpty(objProspect.AvgMonthlyIncome))
                {
                    IsProspect = false;
                }
                objContact.MonthlyIncome = objProspect.AvgMonthlyIncome;
                if (string.IsNullOrEmpty(objProspect.Occupation))
                {
                    IsProspect = false;
                }
                //objContact.OccupationID = Convert.ToInt32(Context.tblMasLifeOccupations.Where(a => a.OccupationCode == objProspect.Occupation).Select(a => a.CompanyCode).FirstOrDefault());
                if (!string.IsNullOrEmpty(objProspect.Occupation))
                {
                    string[] SplitOccupation = objProspect.Occupation.Split('|');
                    string EngOccupation = SplitOccupation[0];
                    objContact.OccupationID = Convert.ToInt32(Context.tblMasLifeOccupations.Where(a => a.OccupationCode == EngOccupation).Select(a => a.CompanyCode).FirstOrDefault());
                }
                else
                {
                    objContact.OccupationID = 0;
                }
                objContact.Place = objProspect.Place;
                if (objProspect.DateofBirth != null)
                {
                    if (objProspect.AgeNextBdy > 0)
                    {

                    }
                    else
                    {
                        IsProspect = false;
                    }
                }
                else
                {
                    IsProspect = false;
                }

                objContact.Age = objProspect.AgeNextBdy;
                objContact.CurrentAge = objProspect.CurrentAge;
                //objContact.ContactType = "Own";
                objContact.DateOfBirth = objProspect.DateofBirth;

                if (objProspect.objAddress != null)
                {
                    if (string.IsNullOrEmpty(objProspect.objAddress.Address1) || string.IsNullOrEmpty(objProspect.objAddress.Pincode))
                    {
                        IsProspect = false;
                    }
                    string pin = "";
                    string City = "";
                    if (!string.IsNullOrEmpty(objProspect.objAddress.Pincode))
                    {
                        string[] PinCity = objProspect.objAddress.Pincode.Split('|');
                        pin = PinCity[0];
                        City = PinCity[1];
                    }

                    objAddress.Address1 = objProspect.objAddress.Address1;
                    objAddress.Address2 = objProspect.objAddress.Address2;
                    objAddress.Address3 = objProspect.objAddress.Address3;
                    objAddress.City = City;
                    objAddress.District = objProspect.objAddress.District;
                    objAddress.State = objProspect.objAddress.State;
                    objAddress.Pincode = pin;
                    objContact.tblAddress = objAddress;
                }
                tblLifeNeedAnalysi objNeedAnalysis = new tblLifeNeedAnalysi();
                objNeedAnalysis.chkProtection1 = objProspect.objNeedAnalysis.chkProtection1;
                objNeedAnalysis.chkProtection2 = objProspect.objNeedAnalysis.chkProtection2;
                objNeedAnalysis.chkProtection3 = objProspect.objNeedAnalysis.chkProtection3;
                objNeedAnalysis.chkProtection4 = objProspect.objNeedAnalysis.chkProtection4;
                objNeedAnalysis.chkProtection5 = objProspect.objNeedAnalysis.chkProtection5;
                objNeedAnalysis.chkRetire1 = objProspect.objNeedAnalysis.chkRetirement1;
                objNeedAnalysis.chkRetire2 = objProspect.objNeedAnalysis.chkRetirement2;
                objNeedAnalysis.chkRetire3 = objProspect.objNeedAnalysis.chkRetirement3;
                objNeedAnalysis.chkRetire4 = objProspect.objNeedAnalysis.chkRetirement4;
                objNeedAnalysis.chkRetire5 = objProspect.objNeedAnalysis.chkRetirement5;
                objNeedAnalysis.chkSaving1 = objProspect.objNeedAnalysis.chkSaving1;
                objNeedAnalysis.chkSaving2 = objProspect.objNeedAnalysis.chkSaving2;
                objNeedAnalysis.chkSaving3 = objProspect.objNeedAnalysis.chkSaving3;
                objNeedAnalysis.chkSaving4 = objProspect.objNeedAnalysis.chkSaving4;
                objNeedAnalysis.chkSaving5 = objProspect.objNeedAnalysis.chkSaving5;
                objNeedAnalysis.chkEdu1 = objProspect.objNeedAnalysis.chkEducation1;
                objNeedAnalysis.chkEdu2 = objProspect.objNeedAnalysis.chkEducation2;
                objNeedAnalysis.chkEdu3 = objProspect.objNeedAnalysis.chkEducation3;
                objNeedAnalysis.chkEdu4 = objProspect.objNeedAnalysis.chkEducation4;
                objNeedAnalysis.chkEdu5 = objProspect.objNeedAnalysis.chkEducation5;
                objNeedAnalysis.chkHealth1 = objProspect.objNeedAnalysis.chkHealth1;
                objNeedAnalysis.chkHealth2 = objProspect.objNeedAnalysis.chkHealth2;
                objNeedAnalysis.chkHealth3 = objProspect.objNeedAnalysis.chkHealth3;
                objNeedAnalysis.chkHealth4 = objProspect.objNeedAnalysis.chkHealth4;
                objNeedAnalysis.chkHealth5 = objProspect.objNeedAnalysis.chkHealth5;
                objNeedAnalysis.chkconfirm = objProspect.objNeedAnalysis.chkconfirm;
                objNeedAnalysis.chkprodconfirm = objProspect.objNeedAnalysis.chkprodconfirm;

                objNeedAnalysis.FromYear = objProspect.objNeedAnalysis.FNAFromYear;
                objNeedAnalysis.ToYear = objProspect.objNeedAnalysis.FNAToYear;
                objNeedAnalysis.PlanNoOfYears = objProspect.objNeedAnalysis.FNAPlanNoYear;
                objNeedAnalysis.InflationRate = objProspect.objNeedAnalysis.FNAInflationRate;
                objNeedAnalysis.RateOfInterest = objProspect.objNeedAnalysis.FNAIntrestRate;
                objNeedAnalysis.Land_Assets = objProspect.objNeedAnalysis.Assets0;
                objNeedAnalysis.FixedDeposit = objProspect.objNeedAnalysis.Assets1;
                objNeedAnalysis.Shares = objProspect.objNeedAnalysis.Assets2;
                objNeedAnalysis.Vehicle = objProspect.objNeedAnalysis.Assets3;
                objNeedAnalysis.Jewellery = objProspect.objNeedAnalysis.Assets4;
                objNeedAnalysis.OtherAssets = objProspect.objNeedAnalysis.Assets5;
                objNeedAnalysis.TotalAssets = objProspect.objNeedAnalysis.objAssetsAndLiabilities.AssetsTotal;
                objNeedAnalysis.TotalLiability = objProspect.objNeedAnalysis.objAssetsAndLiabilities.LiabilityTotal;
                objNeedAnalysis.InsuredTotalLiability = objProspect.objNeedAnalysis.objAssetsAndLiabilities.InsuredLiabilityTotal;
                objNeedAnalysis.Loan = objProspect.objNeedAnalysis.Liabilityone0;
                objNeedAnalysis.InsuredLoan = objProspect.objNeedAnalysis.Liabilitytwo0;
                objNeedAnalysis.CreditCard = objProspect.objNeedAnalysis.Liabilityone1;
                objNeedAnalysis.InsuredCreditCard = objProspect.objNeedAnalysis.Liabilitytwo1;
                objNeedAnalysis.Lease = objProspect.objNeedAnalysis.Liabilityone2;
                objNeedAnalysis.InsuredLease = objProspect.objNeedAnalysis.Liabilitytwo2;
                objNeedAnalysis.OtherLiability = objProspect.objNeedAnalysis.Liabilityone3;
                objNeedAnalysis.InsuredOtherLiability = objProspect.objNeedAnalysis.Liabilitytwo3;

                objNeedAnalysis.NetAssets = objProspect.objNeedAnalysis.objAssetsAndLiabilities.NetAssests;
                objNeedAnalysis.LumpSumReq = objProspect.objNeedAnalysis.objAssetsAndLiabilities.LumpsumRequirement;
                objNeedAnalysis.Salary = objProspect.objNeedAnalysis.Income0;

                objNeedAnalysis.Intrest = objProspect.objNeedAnalysis.Income1;
                objNeedAnalysis.Rent = objProspect.objNeedAnalysis.Income2;
                objNeedAnalysis.OtherIncome = objProspect.objNeedAnalysis.Income3;
                objNeedAnalysis.TotalIncome = objProspect.objNeedAnalysis.objFamilyIncome.TotalIncome;
                objNeedAnalysis.AnnualExp = objProspect.objNeedAnalysis.Expense0;
                objNeedAnalysis.AnnualVacation = objProspect.objNeedAnalysis.Expense1;
                objNeedAnalysis.Installment = objProspect.objNeedAnalysis.Expense2;
                objNeedAnalysis.VehExp = objProspect.objNeedAnalysis.Expense3;
                objNeedAnalysis.LoanExp = objProspect.objNeedAnalysis.Expense4;
                objNeedAnalysis.OtherExp = objProspect.objNeedAnalysis.Expense5;
                objNeedAnalysis.TotalExp = objProspect.objNeedAnalysis.objFamilyIncome.TotalExpense;
                objNeedAnalysis.LumpSumReqExp = objProspect.objNeedAnalysis.objFamilyIncome.IncomeLumpsumRequirement;
                objNeedAnalysis.SurplusExp = objProspect.objNeedAnalysis.objAssetsAndLiabilities.SurPlusAssets;
                objNeedAnalysis.CriticalIllnessReq = objProspect.objNeedAnalysis.CriticalIllnessRequirement;
                objNeedAnalysis.CriticalIllnessFund = objProspect.objNeedAnalysis.CriticalIllnessAvailable;
                objNeedAnalysis.CriticalIllnessGap = objProspect.objNeedAnalysis.CriticalIllnessGap;
                objNeedAnalysis.HospitalizationReq = objProspect.objNeedAnalysis.HospitalRequirement;
                objNeedAnalysis.HospitalizationFund = objProspect.objNeedAnalysis.HospitalAvailable;
                objNeedAnalysis.HospitalizationGap = objProspect.objNeedAnalysis.HospitalGap;
                objNeedAnalysis.TotalReq = objProspect.objNeedAnalysis.TotalRequirement;
                objNeedAnalysis.TotalFund = objProspect.objNeedAnalysis.TotalAvailable;
                objNeedAnalysis.TotalGap = objProspect.objNeedAnalysis.TotalGap;
                objNeedAnalysis.AddExpReq = objProspect.objNeedAnalysis.AdditionalRequirement;
                objNeedAnalysis.AddExpFund = objProspect.objNeedAnalysis.AdditionalAvailable;
                objNeedAnalysis.AddExpGap = objProspect.objNeedAnalysis.AdditionalGap;
                objNeedAnalysis.WealthReq = objProspect.objNeedAnalysis.WealthRequirement;
                objNeedAnalysis.IncomeReq = objProspect.objNeedAnalysis.LivingExpense;
                objNeedAnalysis.DreamReq = objProspect.objNeedAnalysis.FinancialExpense;
                objNeedAnalysis.MaturityDreamReq = objProspect.objNeedAnalysis.FinancialExpense2017;
                objNeedAnalysis.TotalReq1 = objProspect.objNeedAnalysis.TotalExpense;
                objNeedAnalysis.MaturityTotalReq1 = objProspect.objNeedAnalysis.TotalExpense2017;
                objNeedAnalysis.EmergencyPolicy1 = objProspect.objNeedAnalysis.EmergencyFund1;
                objNeedAnalysis.EmergencyPolicy2 = objProspect.objNeedAnalysis.EmergencyFund2;
                objNeedAnalysis.EmergencyPolicy3 = objProspect.objNeedAnalysis.EmergencyFund3;
                objNeedAnalysis.EmergencyTotal2 = objProspect.objNeedAnalysis.TotalEmergencyFund;
                objNeedAnalysis.MaturityPolicy1 = objProspect.objNeedAnalysis.MaturityFund1;
                objNeedAnalysis.MaturityPolicy2 = objProspect.objNeedAnalysis.MaturityFund2;
                objNeedAnalysis.MaturityPolicy3 = objProspect.objNeedAnalysis.MaturityFund3;
                objNeedAnalysis.MaturityTotal2 = objProspect.objNeedAnalysis.TotalMaturityFund;
                objNeedAnalysis.ProspectSign = objProspect.objNeedAnalysis.ProspectSign;


                if ((objProspect.objNeedAnalysis.EmergencyFundGap != null && objProspect.objNeedAnalysis.EmergencyFundGap != 0) && (objProspect.objNeedAnalysis.MaturityFundGap != null && objProspect.objNeedAnalysis.MaturityFundGap != 0) && (objProspect.objNeedAnalysis.Income0 != null && objProspect.objNeedAnalysis.Income0 != 0))
                {
                    IsSuspectComplete = true;
                }

                objNeedAnalysis.Gap1 = objProspect.objNeedAnalysis.EmergencyFundGap;
                objNeedAnalysis.Gap2 = objProspect.objNeedAnalysis.MaturityFundGap;
                if (objProspect.objNeedAnalysis.SelectedProducts != null && objProspect.objNeedAnalysis.chkprodconfirm)
                {
                    IsProductSelected = true;
                }
                objNeedAnalysis.ProductsSelected = objProspect.objNeedAnalysis.SelectedProducts;
                objNeedAnalysis.UploadSignPath = objProspect.objNeedAnalysis.UploadSignPath;
                objNeedAnalysis.NotePadPath = objProspect.objNeedAnalysis.NotePadPath;

                objNeedAnalysis.NoOfOtherPolicies = objProspect.objNeedAnalysis.DependantCount;
                objOppurtunity.NeedAnalysisID = objNeedAnalysis.NeedAnalysisID;
                objContact.tblLifeNeedAnalysis.Add(objNeedAnalysis);


                foreach (var item in objProspect.objNeedAnalysis.objFinancialNeeds)
                {
                    tblNeedFinancialNeed obj = new tblNeedFinancialNeed();
                    obj.CurrentReq = item.CurrReq;
                    obj.EstimatedAmount = item.EstAmount;
                    obj.FundBalance = item.FundBalance;
                    obj.Gap = item.Gap;
                    obj.Relationship = item.Relationship;
                    obj.Name = item.Name;
                    obj.NeedAnalysisID = objProspect.objNeedAnalysis.NeedAnalysisID;

                    objNeedAnalysis.tblNeedFinancialNeeds.Add(obj);
                }
                objNeedAnalysis.FinancialCurrReqTotal = objProspect.objNeedAnalysis.objFinancialNeed.RequirementTotal;
                objNeedAnalysis.FinancialEstAmount = objProspect.objNeedAnalysis.objFinancialNeed.EstimateTotal;
                objNeedAnalysis.FinancialFund = objProspect.objNeedAnalysis.objFinancialNeed.FundBalanceTotal;
                objNeedAnalysis.FinancialGap = objProspect.objNeedAnalysis.objFinancialNeed.GapTotal;
                List<tblPrevPolicy> objPrevPolicy = new List<tblPrevPolicy>();
                foreach (PrevPolicy item in objProspect.objNeedAnalysis.objPrevPolicy)
                {
                    tblPrevPolicy obj = new tblPrevPolicy();
                    obj.MaturityFund = item.MaturityFund;
                    obj.PolicyNumber = item.PolicyNo;
                    obj.NeedAnalysisID = objProspect.objNeedAnalysis.NeedAnalysisID;

                    objNeedAnalysis.tblPrevPolicies.Add(obj);
                }
                tblNeedRetirementCalculator objRetire = new tblNeedRetirementCalculator();
                objRetire.FromYear = objProspect.objNeedAnalysis.CalculatorFromYear;
                objRetire.ToYear = objProspect.objNeedAnalysis.CalculatorToYear;
                objRetire.InflationRate = objProspect.objNeedAnalysis.CalculatorInflationRate;
                objRetire.PlanNoYears = objProspect.objNeedAnalysis.CalculatorPlanNoYears;
                objRetire.IntrestRate = objProspect.objNeedAnalysis.CalculatorIntrestRate;
                if (objProspect.objNeedAnalysis.objCalculator != null)
                {
                    objRetire.TotalMonthlyExp = objProspect.objNeedAnalysis.objCalculator.TotalMonthlyExpense;
                    objRetire.EstMonthlyExp = objProspect.objNeedAnalysis.objCalculator.EstimatedTotalMonthlyExpense;
                    objRetire.CurrentFoodExp = objProspect.objNeedAnalysis.objCalculator.FoodExpense;
                    objRetire.EstFoodExp = objProspect.objNeedAnalysis.objCalculator.EstimatedFoodExpense;
                    objRetire.CurrentWaterExp = objProspect.objNeedAnalysis.objCalculator.WaterExpense;
                    objRetire.EstWaterExp = objProspect.objNeedAnalysis.objCalculator.EstimatedWaterExpense;
                    objRetire.CurrentRentExp = objProspect.objNeedAnalysis.objCalculator.RentExpense;
                    objRetire.EstRentExp = objProspect.objNeedAnalysis.objCalculator.EstimatedRentExpense;
                    objRetire.CurrentLeaseExp = objProspect.objNeedAnalysis.objCalculator.LeaseExpense;
                    objRetire.EstLeaseExp = objProspect.objNeedAnalysis.objCalculator.EstimatedLeaseExpense;
                    objRetire.CurrentTransportExp = objProspect.objNeedAnalysis.objCalculator.TransportExpense;
                    objRetire.EstTransportExp = objProspect.objNeedAnalysis.objCalculator.EstimatedTransportExpense;
                    objRetire.CurrentMedExp = objProspect.objNeedAnalysis.objCalculator.MedicineExpense;
                    objRetire.EstMedExp = objProspect.objNeedAnalysis.objCalculator.EstimatedMedicineExpense;
                    objRetire.CurrentEduExp = objProspect.objNeedAnalysis.objCalculator.EducationExpense;
                    objRetire.EstEduExp = objProspect.objNeedAnalysis.objCalculator.EstimatedEducationExpense;
                    objRetire.CurrentClothesExp = objProspect.objNeedAnalysis.objCalculator.ClothesExpense;
                    objRetire.EstClothesExp = objProspect.objNeedAnalysis.objCalculator.EstimatedClothesExpense;
                    objRetire.CurrentEntertainmentExp = objProspect.objNeedAnalysis.objCalculator.EntertainmentExpense;
                    objRetire.EstEntertainmentExp = objProspect.objNeedAnalysis.objCalculator.EstimatedEntertainmentExpense;
                    objRetire.CurrentCharity = objProspect.objNeedAnalysis.objCalculator.CharityExpense;
                    objRetire.EstCharity = objProspect.objNeedAnalysis.objCalculator.EstimatedCharityExpense;
                    objRetire.CurrentOtherExp = objProspect.objNeedAnalysis.objCalculator.OtherExpense;
                    objRetire.EstOtherExp = objProspect.objNeedAnalysis.objCalculator.EstimatedOtherExpense;
                    objRetire.CurrentMonthlySalary = objProspect.objNeedAnalysis.objCalculator.Salary;
                    objRetire.CurrentEPFBalance = objProspect.objNeedAnalysis.objCalculator.CurrentEPFBalance;
                    objRetire.EstEPFBalance = objProspect.objNeedAnalysis.objCalculator.EstimatedEPFBalance;
                    objRetire.CurrentMonthly20Sal = objProspect.objNeedAnalysis.objCalculator.MonthlyAllocation20;
                    objRetire.CurrentETFBalance = objProspect.objNeedAnalysis.objCalculator.CurrentETFBalance;
                    objRetire.EstETFBalance = objProspect.objNeedAnalysis.objCalculator.EstimatedETFBalance;
                    objRetire.CurrentMonthly3Sal = objProspect.objNeedAnalysis.objCalculator.MonthlyAllocation3;
                    objRetire.CurrentGratuityFund = objProspect.objNeedAnalysis.objCalculator.CurrentGratuityFund;
                    objRetire.EstGratuityFund = objProspect.objNeedAnalysis.objCalculator.EstimatedGratuityFund;
                    objRetire.TotalEstMonthlyExpFund = objProspect.objNeedAnalysis.objCalculator.TotalRetirementFund;
                    objRetire.ChildEduFund = objProspect.objNeedAnalysis.Financial0;
                    objRetire.ChildWeddingFund = objProspect.objNeedAnalysis.Financial1;
                    objRetire.VehicleFund = objProspect.objNeedAnalysis.Financial2;
                    objRetire.LoanFund = objProspect.objNeedAnalysis.Financial3;
                    objRetire.OtherFund = objProspect.objNeedAnalysis.Financial4;
                    objRetire.FundBalance = objProspect.objNeedAnalysis.objCalculator.FundBalanceTotal;
                    objRetire.PerAnnIncomeIntrest = objProspect.objNeedAnalysis.objCalculator.PerAnnumIncome;
                    objRetire.EstAnnualLivExp = objProspect.objNeedAnalysis.objCalculator.EstimatedAnnuallivingExpenses;
                    objRetire.TotalAnnualExp = objProspect.objNeedAnalysis.objCalculator.AnnualIncomeSurplus;
                    objRetire.ExistingOthIncome = objProspect.objNeedAnalysis.objCalculator.Exsitingotherincome;
                }

                if (objProspect.objNeedAnalysis.objCalculator.MonthlyPensionGap != null && objProspect.objNeedAnalysis.objCalculator.MonthlyPensionGap != 0)
                {
                    IsSuspectComplete = true;
                }
                objRetire.PensionGap = objProspect.objNeedAnalysis.objCalculator.MonthlyPensionGap;
                objNeedAnalysis.tblNeedRetirementCalculators.Add(objRetire);

                tblNeedHealthCalculator objHealth = new tblNeedHealthCalculator();
                objHealth.CriticalillnessReq = objProspect.objNeedAnalysis.CriticalRequiremenent;
                objHealth.CriticalIllenssFund = objProspect.objNeedAnalysis.CriticalFund;
                objHealth.CriticalIllnessGap = objProspect.objNeedAnalysis.CriticalGap;
                objHealth.HospReq = objProspect.objNeedAnalysis.HospitalizationRequiremenent;
                objHealth.HospFund = objProspect.objNeedAnalysis.HospitalizationFund;
                objHealth.HospGap = objProspect.objNeedAnalysis.HospitalizationGap;
                objHealth.AddLossReq = objProspect.objNeedAnalysis.additionalexpenseRequiremenent;
                objHealth.AddLossFund = objProspect.objNeedAnalysis.additionalexpenseFund;
                objHealth.AddLossGap = objProspect.objNeedAnalysis.additionalexpenseGap;
                objHealth.HospitalBills = objProspect.objNeedAnalysis.HospitalBills;
                objHealth.HospRetireExp = objProspect.objNeedAnalysis.HospitalRtrExp;
                if (objProspect.objNeedAnalysis.objadversities != null)
                {
                    objHealth.HealthAdversities = String.Join(",", objProspect.objNeedAnalysis.objadversities);
                }
                objHealth.AnnualAmountHealthExp = objProspect.objNeedAnalysis.objannualamount;
                objHealth.CoverageHealthExp = objProspect.objNeedAnalysis.objcoverage;
                objHealth.AdequacyHealthExp = objProspect.objNeedAnalysis.objadequacy;
                if ((objProspect.objNeedAnalysis.CriticalGap != null && objProspect.objNeedAnalysis.CriticalGap != 0) && (objProspect.objNeedAnalysis.HospitalizationGap != null && objProspect.objNeedAnalysis.HospitalizationGap != 0) && (objProspect.objNeedAnalysis.additionalexpenseGap != null && objProspect.objNeedAnalysis.additionalexpenseGap != 0))
                {
                    IsSuspectComplete = true;
                }

                objNeedAnalysis.tblNeedHealthCalculators.Add(objHealth);

                tblNeedEducationCalculator objEdu = new tblNeedEducationCalculator();
                objEdu.Inflationrate = objProspect.objNeedAnalysis.EduInflationRate;
                objEdu.AnnualEduExp = objProspect.objNeedAnalysis.AnnualEduExpense;
                objEdu.EduMaturityValue = objProspect.objNeedAnalysis.EduMaturity;
                objEdu.LumpSum = objProspect.objNeedAnalysis.EduLumpSum;
                objEdu.MonthlyEduExp = objProspect.objNeedAnalysis.MonthlyEduExpense;
                if (objProspect.objNeedAnalysis.EduGapTotal != null)
                {
                    IsSuspectComplete = true;
                }
                objEdu.EduGapTotal = objProspect.objNeedAnalysis.EduGapTotal;
                objEdu.NeedAnalysisID = objProspect.objNeedAnalysis.NeedAnalysisID;
                objNeedAnalysis.tblNeedEducationCalculators.Add(objEdu);

                List<tblNeedEduGCEAL> objGCEAL = new List<tblNeedEduGCEAL>();
                foreach (var item in objProspect.objNeedAnalysis.objGCEAL)
                {
                    tblNeedEduGCEAL obj = new tblNeedEduGCEAL();
                    obj.CurrentReq = item.CurrRequirement;
                    obj.Term = item.Term;
                    obj.MaturityAge = item.MaturityAge;
                    obj.EstAmount = item.EstAmount;
                    obj.AvailableFund = item.AvailableFund;
                    obj.Gap = item.Gap;
                    obj.Relationship = item.Relationship;
                    obj.Age = item.Age;
                    obj.EduCalcID = objEdu.Id;
                    objEdu.tblNeedEduGCEALs.Add(obj);
                }

                List<tblNeedEduForeign> objForeign = new List<tblNeedEduForeign>();
                foreach (var item in objProspect.objNeedAnalysis.objHigherForeign)
                {
                    tblNeedEduForeign obj = new tblNeedEduForeign();
                    obj.CurrentReq = item.CurrRequirement;
                    obj.Term = item.Term;
                    obj.MaturityAge = item.MaturityAge;
                    obj.EstAmount = item.EstAmount;
                    obj.AvailableFund = item.AvailableFund;
                    obj.Gap = item.Gap;
                    obj.Relationship = item.Relationship;
                    obj.Age = item.Age;
                    obj.EduCalcID = objEdu.Id;

                    objEdu.tblNeedEduForeigns.Add(obj);
                }
                List<tblNeedEduHigher> objHigher = new List<tblNeedEduHigher>();
                foreach (var item in objProspect.objNeedAnalysis.objHigherEdu)
                {
                    tblNeedEduHigher obj = new tblNeedEduHigher();
                    obj.CurrentReq = item.CurrRequirement;
                    obj.Term = item.Term;
                    obj.MaturityAge = item.MaturityAge;
                    obj.EstAmount = item.EstAmount;
                    obj.AvailableFund = item.AvailableFund;
                    obj.Gap = item.Gap;
                    obj.Relationship = item.Relationship;
                    obj.Age = item.Age;
                    obj.EduCalcID = objEdu.Id;
                    objEdu.tblNeedEduHighers.Add(obj);
                }
                List<tblNeedEduLocal> objLocal = new List<tblNeedEduLocal>();
                foreach (var item in objProspect.objNeedAnalysis.objLocal)
                {
                    tblNeedEduLocal obj = new tblNeedEduLocal();
                    obj.CurrentReq = item.CurrRequirement;
                    obj.Term = item.Term;
                    obj.MaturityAge = item.MaturityAge;
                    obj.EstAmount = item.EstAmount;
                    obj.AvailableFund = item.AvailableFund;
                    obj.Gap = item.Gap;
                    obj.Relationship = item.Relationship;
                    obj.Age = item.Age;
                    obj.EduCalcID = objEdu.Id;
                    objEdu.tblNeedEduLocals.Add(obj);
                }

                tblNeedSavingCalculator objSave = new tblNeedSavingCalculator();
                objSave.Inflationrate = objProspect.objNeedAnalysis.SavInflationRate;
                objSave.AnnualSavingExp = objProspect.objNeedAnalysis.AnnualSaveExpense;
                if ((objProspect.objNeedAnalysis.SavingReqTotal != null && objProspect.objNeedAnalysis.SavingReqTotal != 0) && (objProspect.objNeedAnalysis.SavingEstTotal != null && objProspect.objNeedAnalysis.SavingEstTotal != 0) && (objProspect.objNeedAnalysis.SavingCurrentTotal != null && objProspect.objNeedAnalysis.SavingCurrentTotal != 0) && (objProspect.objNeedAnalysis.SavingGapTotal != null && objProspect.objNeedAnalysis.SavingGapTotal != 0))
                {
                    IsSuspectComplete = true;
                }
                objSave.CurrReqTotal = objProspect.objNeedAnalysis.SavingReqTotal;
                objSave.EstAmountTotal = objProspect.objNeedAnalysis.SavingEstTotal;
                objSave.AvailableFund = objProspect.objNeedAnalysis.SavingCurrentTotal;
                objSave.GapTotal = objProspect.objNeedAnalysis.SavingGapTotal;
                objSave.MonthlySaveExp = objProspect.objNeedAnalysis.MonthlySaveExpense;
                objNeedAnalysis.tblNeedSavingCalculators.Add(objSave);

                List<tblNeedSaveCar> objCar = new List<tblNeedSaveCar>();
                foreach (var item in objProspect.objNeedAnalysis.objCar)
                {
                    tblNeedSaveCar obj = new tblNeedSaveCar();
                    obj.CurrentReq = item.CurrRequirement;
                    obj.Term = item.Term;
                    obj.EstAmount = item.EstAmount;
                    obj.AvailableFund = item.AvailableFund;
                    obj.Gap = item.Gap;
                    obj.Relationship = item.Relationship;
                    obj.Age = item.Age;
                    obj.MaturityAge = item.MaturityAge;
                    obj.SaveCalcID = objSave.Id;
                    objSave.tblNeedSaveCars.Add(obj);
                }

                List<tblNeedSaveHouse> objHouse = new List<tblNeedSaveHouse>();
                foreach (var item in objProspect.objNeedAnalysis.objHouse)
                {
                    tblNeedSaveHouse obj = new tblNeedSaveHouse();
                    obj.CurrentReq = item.CurrRequirement;
                    obj.Term = item.Term;
                    obj.EstAmount = item.EstAmount;
                    obj.AvailableFund = item.AvailableFund;
                    obj.Gap = item.Gap;
                    obj.Relationship = item.Relationship;
                    obj.Age = item.Age;
                    obj.MaturityAge = item.MaturityAge;
                    obj.SaveCalcID = objSave.Id;
                    objSave.tblNeedSaveHouses.Add(obj);
                }
                List<tblNeedSaveOther> objOther = new List<tblNeedSaveOther>();
                foreach (var item in objProspect.objNeedAnalysis.objOthers)
                {
                    tblNeedSaveOther obj = new tblNeedSaveOther();
                    obj.CurrentReq = item.CurrRequirement;
                    obj.Term = item.Term;
                    obj.EstAmount = item.EstAmount;
                    obj.AvailableFund = item.AvailableFund;
                    obj.Gap = item.Gap;
                    obj.Relationship = item.Relationship;
                    obj.Age = item.Age;
                    obj.MaturityAge = item.MaturityAge;
                    obj.SaveCalcID = objSave.Id;
                    objSave.tblNeedSaveOthers.Add(obj);
                }
                List<tblNeedSaveTour> objTour = new List<tblNeedSaveTour>();
                foreach (var item in objProspect.objNeedAnalysis.objForeignTour)
                {
                    tblNeedSaveTour obj = new tblNeedSaveTour();
                    obj.CurrentReq = item.CurrRequirement;
                    obj.Term = item.Term;
                    obj.EstAmount = item.EstAmount;
                    obj.AvailableFund = item.AvailableFund;
                    obj.Gap = item.Gap;
                    obj.Relationship = item.Relationship;
                    obj.Age = item.Age;
                    obj.MaturityAge = item.MaturityAge;
                    obj.SaveCalcID = objSave.Id;
                    objSave.tblNeedSaveTours.Add(obj);
                }
                List<tblNeedSaveWedding> objWedding = new List<tblNeedSaveWedding>();
                foreach (var item in objProspect.objNeedAnalysis.objWedding)
                {
                    tblNeedSaveWedding obj = new tblNeedSaveWedding();
                    obj.CurrentReq = item.CurrRequirement;
                    obj.Term = item.Term;
                    obj.EstAmount = item.EstAmount;
                    obj.AvailableFund = item.AvailableFund;
                    obj.Gap = item.Gap;
                    obj.Relationship = item.Relationship;
                    obj.Age = item.Age;
                    obj.MaturityAge = item.MaturityAge;
                    obj.SaveCalcID = objSave.Id;
                    objSave.tblNeedSaveWeddings.Add(obj);
                }

                tblNeedHumanValueCalculator objHumanValue = new tblNeedHumanValueCalculator();
                objHumanValue.MonthlyEarning = objProspect.objNeedAnalysis.MonthlyEarning;
                objHumanValue.NoOfYears = objProspect.objNeedAnalysis.YearsofEarning;
                objHumanValue.IntrestRate = objProspect.objNeedAnalysis.ProIntrestRate;
                objHumanValue.EstIncome = objProspect.objNeedAnalysis.EstimatedIncome;
                objHumanValue.FutureAvailableFund = objProspect.objNeedAnalysis.FutureFund;
                if (objProspect.objNeedAnalysis.EmergencyFund != null && objProspect.objNeedAnalysis.EmergencyFund != 0)
                {
                    IsSuspectComplete = true;
                }
                objHumanValue.EmergencyFundReq = objProspect.objNeedAnalysis.EmergencyFund;
                objHumanValue.AvailableFund = objProspect.objNeedAnalysis.AvailableFund;
                objNeedAnalysis.tblNeedHumanValueCalculators.Add(objHumanValue);
                //SamsClient samsClient = new SamsClient();
                objOppurtunity.tblContact = objContact;
                objProspect.UserName = objProspect.CreatedBy;
                tblOpportunityHistory objOpportunityHistory = new tblOpportunityHistory();
                if (IsProspect)
                {
                    //samsClient.UpdateLead(Context, objProspect);
                    objContact.LeadNo = objProspect.SamsLeadNumber;
                    objOppurtunity.StageID = 2; // Prospect
                    objOpportunityHistory.StageID = 2;
                    objOpportunityHistory.OpportunityID = objOppurtunity.OppurtunityID;
                    objOpportunityHistory.CreatedDate = DateTime.Now;
                    //samsClient.UpdateLeadStatus(Context, Convert.ToInt32(objContact.LeadNo), 3);
                }
                else
                {
                    //samsClient.UpdateLead(Context, objProspect);
                    //objContact.LeadNo = objProspect.SamsLeadNumber;
                    objOppurtunity.StageID = 1; // Suspect
                    objOpportunityHistory.StageID = 1;
                    objOpportunityHistory.OpportunityID = objOppurtunity.OppurtunityID;
                    objOpportunityHistory.CreatedDate = DateTime.Now;
                }
                if (IsProductSelected == true && IsProspect == true)
                {
                    //samsClient.UpdateLead(Context, objProspect);
                    //objContact.LeadNo = objProspect.SamsLeadNumber;
                    //samsClient.UpdateLeadStatus(Context, Convert.ToInt32(objContact.LeadNo), 4);
                    objOppurtunity.StageID = 4;//Need Analysis Completed
                    objOpportunityHistory.StageID = 4;
                    objOpportunityHistory.OpportunityID = objOppurtunity.OppurtunityID;
                    objOpportunityHistory.CreatedDate = DateTime.Now;
                }
                //if (IsSuspectComplete == true && IsProductSelected == true && IsProspect == true)
                //{
                //    samsClient.UpdateLeadStatus(Context, Convert.ToInt32(objContact.LeadNo), 4);
                //    objOppurtunity.StageID = 4;//Need Analysis Completed
                //}
                //else
                // {
                //     objOppurtunity.StageID = 2; // Prospect
                //     samsClient.UpdateLeadStatus(Context, Convert.ToInt32(objContact.LeadNo), 3);
                // }
                Context.tblOpportunityHistories.Add(objOpportunityHistory);
                Context.SaveChanges();
                objProspect.Message = "Success";

                objProspect.ProspectStage = objOppurtunity.StageID;

                //if (string.IsNullOrEmpty(objProspect.SamsLeadNumber))
                //{
                //    objProspect.Error.ErrorMessage = objProspect.Message = "Sorry, system is unable to connect with the SAMS. You will get the lead number before creating the proposal. Please proceed.....";
                //}
                return objProspect;
            }
            catch (Exception ex)
            {
                throw ex;            }

        }

        private bool IsConfirmedProspect(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            if (!string.IsNullOrEmpty(objProspect.NIC) && !string.IsNullOrEmpty(objProspect.MaritalStatus) && !string.IsNullOrEmpty(objProspect.Gender) && !string.IsNullOrEmpty(objProspect.AvgMonthlyIncome))
            {
                return true;
            }
            else
                return false;
        }

        public bool ValidateDependentsInformation(AIA.Life.Models.Opportunity.Prospect objProspect)
        {

            #region Validate  Dependents Info

            if (objProspect.objNeedAnalysis.objSpouseDetails != null)
            {
                if (objProspect.MaritalStatus == "14")  // if Married
                {
                    if (string.IsNullOrEmpty(objProspect.objNeedAnalysis.objSpouseDetails.FullName) || string.IsNullOrEmpty(objProspect.objNeedAnalysis.objSpouseDetails.OccuaptionID) || string.IsNullOrEmpty(objProspect.objNeedAnalysis.objSpouseDetails.Employer))
                    {
                        return false;
                    }
                    if (objProspect.objNeedAnalysis.objSpouseDetails.DateOfBirth == null)
                    {
                        if (objProspect.objNeedAnalysis.objSpouseDetails.AgeNextBirthday <= 0)
                        {
                            return false;
                        }
                    }
                    if (objProspect.objNeedAnalysis.objSpouseDetails.AgeNextBirthday <= 0)
                    {
                        if (objProspect.objNeedAnalysis.objSpouseDetails.DateOfBirth == null)
                        {
                            return false;
                        }
                    }
                }
            }

            if (objProspect.objNeedAnalysis.objDependents != null)
            {
                foreach (var item in objProspect.objNeedAnalysis.objDependents)
                {

                    if (string.IsNullOrEmpty(item.DependentName)) //|| string.IsNullOrEmpty(item.Relationship))
                    {
                        return false;
                    }
                    if (item.DateOfBirth == null)
                    {
                        if (item.Age <= 0)
                        {
                            return false;
                        }
                    }
                    if (item.Age <= 0)
                    {
                        if (item.DateOfBirth == null)
                        {
                            return false;
                        }
                    }
                }
            }
            #endregion
            return true;
        }

        public bool ValidateExpences(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            #region Estimates
            if (objProspect.objNeedAnalysis.objEstimateDetails != null)
            {

                if ((objProspect.objNeedAnalysis.objEstimateDetails.Food >= 0) && (objProspect.objNeedAnalysis.objEstimateDetails.HouseElectricityWaterRent >= 0)
                 && (objProspect.objNeedAnalysis.objEstimateDetails.Clothes >= 0) && (objProspect.objNeedAnalysis.objEstimateDetails.Transport >= 0)
                 && (objProspect.objNeedAnalysis.objEstimateDetails.HealthCare >= 0) && (objProspect.objNeedAnalysis.objEstimateDetails.FamilyEducation >= 0)
                 && (objProspect.objNeedAnalysis.objEstimateDetails.SpecialEvents >= 0) && (objProspect.objNeedAnalysis.objEstimateDetails.MaidAndOtherHelpers >= 0)
                 && (objProspect.objNeedAnalysis.objEstimateDetails.OtherMontly >= 0) && (objProspect.objNeedAnalysis.objEstimateDetails.MonthlyInstallments >= 0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            #endregion
            return false;
        }

        public bool ValidateFamilyIncome(AIA.Life.Models.Opportunity.Prospect objProspect)
        {

            #region Prospect And Spouse
            if ((objProspect.objNeedAnalysis.objFamilyIncome.ProspectMonthlyIncome >= 0) && (objProspect.objNeedAnalysis.objFamilyIncome.SavingsAndInvestments >= 0))
            {
            }
            else
            {
                return false;
            }
            if (objProspect.MaritalStatus == "14")  // if Married
            {
                if (objProspect.objNeedAnalysis.objFamilyIncome.SpouseMonthlyIncome >= 0)
                {

                }
                else
                {
                    return false;
                }
            }
            #endregion

            return true;
        }

        public bool ValidateNeeds(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            if (objProspect.objNeedAnalysis.objNeeds.Where(a => a.IsNeedOpted == true).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ValidateAssestAndLiability(AIA.Life.Models.Opportunity.Prospect objProspect)
        {


            if ((objProspect.objNeedAnalysis.objAssetsAndLiabilities.LandOrHouse >= 0) && (objProspect.objNeedAnalysis.objAssetsAndLiabilities.MotorVehicle >= 0)
                  && (objProspect.objNeedAnalysis.objAssetsAndLiabilities.BankDeposits >= 0) && (objProspect.objNeedAnalysis.objAssetsAndLiabilities.Investments >= 0))
            {
                #region Liability
                if ((objProspect.objNeedAnalysis.objAssetsAndLiabilities.Loans >= 0) && (objProspect.objNeedAnalysis.objAssetsAndLiabilities.Mortgauges >= 0)
                       && (objProspect.objNeedAnalysis.objAssetsAndLiabilities.leases >= 0) && (objProspect.objNeedAnalysis.objAssetsAndLiabilities.others >= 0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
                #endregion

            }
            else
            {
                return false;
            }




        }

        public bool ValidateAdditionalDetails(AIA.Life.Models.Opportunity.Prospect objProspect)
        {

            if (string.IsNullOrEmpty(objProspect.objNeedAnalysis.UploadSignPath))
            {
                if (objProspect.objNeedAnalysis.ProspectSign != null)
                {
                }
                else
                {
                    return false;
                }
            }

            if (string.IsNullOrEmpty(objProspect.objNeedAnalysis.SelectedProducts))
            {
                return false;
            }

            return true;
        }

        public AIA.Life.Models.Opportunity.Prospect SaveNeedAnalysis(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                tblContact objContact = Context.tblContacts.Where(a => a.ContactID == objProspect.ContactID).FirstOrDefault();
                tblOpportunity objOppurtunity = Context.tblOpportunities.Where(a => a.ContactID == objProspect.ContactID).FirstOrDefault();
                tblAddress objAddress = objContact.tblAddress;
                bool IsProspect = true;
                bool IsSuspectComplete = false;
                bool IsProductSelected = false;
                objContact.FirstName = objProspect.Name;
                objContact.LastName = objProspect.LastName;
                objContact.Work = objProspect.Work;
                objContact.MobileNo = objProspect.Mobile;
                objContact.PhoneNo = objProspect.Home;
                objContact.EmailID = objProspect.Email;
                objContact.Age = objProspect.AgeNextBdy;
                objContact.CurrentAge = objProspect.CurrentAge;
                objContact.ContactType = "Own";
                objContact.DateOfBirth = objProspect.DateofBirth;
                //objContact.OccupationID = Convert.ToInt32(Context.tblMasLifeOccupations.Where(a => a.OccupationCode == objProspect.Occupation).Select(a => a.CompanyCode).FirstOrDefault());
                if (!string.IsNullOrEmpty(objProspect.Occupation))
                {
                    string[] SplitOccupation = objProspect.Occupation.Split('|');
                    string EngOccupation = SplitOccupation[0];
                    objContact.OccupationID = Convert.ToInt32(Context.tblMasLifeOccupations.Where(a => a.OccupationCode == EngOccupation).Select(a => a.CompanyCode).FirstOrDefault());
                }
                else
                {
                    objContact.OccupationID = 0;
                }
                objContact.Employer = objProspect.EmployerName;
                objContact.NICNO = objProspect.NIC;
                objContact.MaritalStatusID = Context.tblMasCommonTypes.Where(a => a.Code == objProspect.MaritalStatus && a.MasterType == "MaritalStatus").Select(b => b.CommonTypesID).FirstOrDefault();
                objContact.MonthlyIncome = objProspect.AvgMonthlyIncome;
                objContact.ClientCode = objProspect.ClientCode;
                objProspect.IsConfirmedProspect = IsConfirmedProspect(objProspect);
                if (objProspect.objAddress != null)
                {
                    objAddress.Address1 = objProspect.objAddress.Address1;
                    objAddress.Address2 = objProspect.objAddress.Address2;
                    objAddress.Address3 = objProspect.objAddress.Address3;
                    objAddress.City = objProspect.objAddress.City;
                    objAddress.District = objProspect.objAddress.District;
                    objAddress.State = objProspect.objAddress.State;
                    objAddress.Pincode = objProspect.objAddress.Pincode;

                    objContact.tblAddress = objAddress;
                }

                if (objProspect.objNeedAnalysis != null)
                {
                    bool IsNeedAnalysisInfoAdded = true;
                    //bool IsIntermSaveRequired = false;
                    #region Adding Dependents Info
                    if (objProspect.objNeedAnalysis.objSpouseDetails != null)
                    {
                        if (!string.IsNullOrEmpty(objProspect.objNeedAnalysis.objSpouseDetails.FullName))
                        {
                            tblContact objSpouseContact = new tblContact();
                            tblContact ExisitingSpouseContact = new tblContact();
                            if (objProspect.objNeedAnalysis.objSpouseDetails.ContactID > 0)
                            {
                                ExisitingSpouseContact = Context.tblContacts.Where(a => a.ContactID == objProspect.objNeedAnalysis.objSpouseDetails.ContactID).FirstOrDefault();
                                objSpouseContact = Context.tblContacts.Where(a => a.ContactID == objProspect.objNeedAnalysis.objSpouseDetails.ContactID).FirstOrDefault();
                            }
                            objSpouseContact.FirstName = objProspect.objNeedAnalysis.objSpouseDetails.FullName;
                            objSpouseContact.DateOfBirth = objProspect.objNeedAnalysis.objSpouseDetails.DateOfBirth;
                            objSpouseContact.Age = objProspect.objNeedAnalysis.objSpouseDetails.AgeNextBirthday;
                            objSpouseContact.CurrentAge = objProspect.objNeedAnalysis.objSpouseDetails.CurrentAge;
                            objSpouseContact.MaritalStatusID = Convert.ToDecimal(objProspect.objNeedAnalysis.objSpouseDetails.MaritialStatus);
                            objSpouseContact.OccupationID = Convert.ToInt32(objProspect.objNeedAnalysis.objSpouseDetails.OccuaptionID);
                            objSpouseContact.Employer = objProspect.objNeedAnalysis.objSpouseDetails.Employer;
                            objSpouseContact.ParentContactID = objProspect.ContactID;
                            objSpouseContact.Isparent = false;
                            objSpouseContact.Relationship = "268";// Spouse
                            if (objProspect.objNeedAnalysis.objSpouseDetails.ContactID > 0)
                            {

                                Context.Entry(ExisitingSpouseContact).CurrentValues.SetValues(objSpouseContact);
                            }
                            else
                            {
                                Context.tblContacts.Add(objSpouseContact);
                            }
                        }
                    }

                    if (objProspect.objNeedAnalysis.objDependents != null)
                    {
                        foreach (var item in objProspect.objNeedAnalysis.objDependents.Where(a => a.IsDeleted != true).ToList())
                        {
                            tblContact objdepenndentContact = new tblContact();
                            tblContact ExisitingDepenndentContact = new tblContact();
                            if (item.ContactID > 0)
                            {
                                ExisitingDepenndentContact = Context.tblContacts.Where(a => a.ContactID == item.ContactID).FirstOrDefault();
                                objdepenndentContact = Context.tblContacts.Where(a => a.ContactID == item.ContactID).FirstOrDefault();
                            }
                            objdepenndentContact.ParentContactID = objProspect.ContactID;
                            objdepenndentContact.Isparent = false;
                            objdepenndentContact.FirstName = item.DependentName;
                            objdepenndentContact.Relationship = item.Relationship;
                            objdepenndentContact.Age = item.Age;
                            objdepenndentContact.DateOfBirth = item.DateOfBirth;
                            if (!string.IsNullOrEmpty(objdepenndentContact.FirstName))
                            {
                                if (item.ContactID > 0)
                                {
                                    Context.Entry(ExisitingDepenndentContact).CurrentValues.SetValues(objdepenndentContact);
                                }
                                else
                                {
                                    Context.tblContacts.Add(objdepenndentContact);
                                }
                            }
                        }
                    }
                    if (IsNeedAnalysisInfoAdded)
                    {
                        IsNeedAnalysisInfoAdded = ValidateDependentsInformation(objProspect);
                    }
                    #endregion


                    tblLifeNeedAnalysi objtblLifeNeedAnalysis = new tblLifeNeedAnalysi();
                    tblLifeNeedAnalysi ExisitingtblLifeNeedAnalysis = new tblLifeNeedAnalysi();
                    tblPreviousInsurenceInfo objtblPreviousInsurenceInfo = new tblPreviousInsurenceInfo();
                    if (objProspect.objNeedAnalysis.NeedAnalysisID > 0)
                    {
                        objtblLifeNeedAnalysis = Context.tblLifeNeedAnalysis.Where(a => a.NeedAnalysisID == objProspect.objNeedAnalysis.NeedAnalysisID).FirstOrDefault();
                        ExisitingtblLifeNeedAnalysis = Context.tblLifeNeedAnalysis.Where(a => a.NeedAnalysisID == objProspect.objNeedAnalysis.NeedAnalysisID).FirstOrDefault();
                    }

                    #region Estimates
                    if (objProspect.objNeedAnalysis.objEstimateDetails != null)
                    {
                        if (objProspect.objNeedAnalysis.objEstimateDetails.Food != null && objProspect.objNeedAnalysis.objEstimateDetails.Food != 0)
                        {
                            objtblLifeNeedAnalysis.Food = objProspect.objNeedAnalysis.objEstimateDetails.Food;
                            objtblLifeNeedAnalysis.House_Elec_Water_Phone = objProspect.objNeedAnalysis.objEstimateDetails.HouseElectricityWaterRent;
                            objtblLifeNeedAnalysis.Clothes = objProspect.objNeedAnalysis.objEstimateDetails.Clothes;
                            objtblLifeNeedAnalysis.Transport = objProspect.objNeedAnalysis.objEstimateDetails.Transport;
                            objtblLifeNeedAnalysis.Family_HealthCare = objProspect.objNeedAnalysis.objEstimateDetails.HealthCare;
                            objtblLifeNeedAnalysis.Edu_of_Child = objProspect.objNeedAnalysis.objEstimateDetails.FamilyEducation;
                            objtblLifeNeedAnalysis.SpecialEvents = objProspect.objNeedAnalysis.objEstimateDetails.SpecialEvents;
                            objtblLifeNeedAnalysis.MaidAndOthers = objProspect.objNeedAnalysis.objEstimateDetails.MaidAndOtherHelpers;
                            objtblLifeNeedAnalysis.Other_Monthly = objProspect.objNeedAnalysis.objEstimateDetails.OtherMontly;
                            objtblLifeNeedAnalysis.Total_Monthly = objProspect.objNeedAnalysis.objEstimateDetails.TotalMonthlyExp;
                            objtblLifeNeedAnalysis.Monthly_Installments = objProspect.objNeedAnalysis.objEstimateDetails.MonthlyInstallments;
                        }
                    }
                    if (IsNeedAnalysisInfoAdded)
                    {
                        IsNeedAnalysisInfoAdded = ValidateExpences(objProspect);
                    }
                    #endregion

                    #region Assests And Liabilities
                    if (objProspect.objNeedAnalysis.objAssetsAndLiabilities != null)
                    {
                        objtblLifeNeedAnalysis.Land_Assets = objProspect.objNeedAnalysis.objAssetsAndLiabilities.LandOrHouse;
                        objtblLifeNeedAnalysis.Motor_Assets = objProspect.objNeedAnalysis.objAssetsAndLiabilities.MotorVehicle;
                        objtblLifeNeedAnalysis.MotorVehicleType = objProspect.objNeedAnalysis.objAssetsAndLiabilities.MotorVehicleType;
                        objtblLifeNeedAnalysis.Bank_Assets = objProspect.objNeedAnalysis.objAssetsAndLiabilities.BankDeposits;
                        objtblLifeNeedAnalysis.Assets_investments = Convert.ToString(objProspect.objNeedAnalysis.objAssetsAndLiabilities.Investments);
                        objtblLifeNeedAnalysis.Total_Assets = objProspect.objNeedAnalysis.objAssetsAndLiabilities.AssetsTotal;
                        objtblLifeNeedAnalysis.Loan_LB = objProspect.objNeedAnalysis.objAssetsAndLiabilities.Loans;
                        objtblLifeNeedAnalysis.Mortgages_LB = objProspect.objNeedAnalysis.objAssetsAndLiabilities.Mortgauges;
                        objtblLifeNeedAnalysis.Leases_LB = objProspect.objNeedAnalysis.objAssetsAndLiabilities.leases;
                        objtblLifeNeedAnalysis.Others_LB = objProspect.objNeedAnalysis.objAssetsAndLiabilities.others;
                        objtblLifeNeedAnalysis.Total_LB = objProspect.objNeedAnalysis.objAssetsAndLiabilities.Liab_Total;
                    }
                    if (IsNeedAnalysisInfoAdded)
                    {
                        IsNeedAnalysisInfoAdded = ValidateAssestAndLiability(objProspect);
                    }
                    #endregion

                    #region FamilyIncome
                    if (objProspect.objNeedAnalysis.objFamilyIncome != null)
                    {
                        if (objProspect.objNeedAnalysis.objFamilyIncome.ProspectMonthlyIncome != null && objProspect.objNeedAnalysis.objFamilyIncome.ProspectMonthlyIncome != 0)
                        {
                            objtblLifeNeedAnalysis.Prospect_Monthly = objProspect.objNeedAnalysis.objFamilyIncome.ProspectMonthlyIncome;
                            objtblLifeNeedAnalysis.Spouse_Monthly = objProspect.objNeedAnalysis.objFamilyIncome.SpouseMonthlyIncome;
                            objtblLifeNeedAnalysis.HouseHold_Monthly = objProspect.objNeedAnalysis.objFamilyIncome.HouseHoldTotal;
                            objtblLifeNeedAnalysis.Capital_Monthly_Exp_1 = objProspect.objNeedAnalysis.objFamilyIncome.CapitalReq;
                            objtblLifeNeedAnalysis.Life_Insurance = objProspect.objNeedAnalysis.objFamilyIncome.PersonalLifeInsurance;
                            objtblLifeNeedAnalysis.OtherSavings = objProspect.objNeedAnalysis.objFamilyIncome.SavingsAndInvestments;
                            objtblLifeNeedAnalysis.Capital_Monthly_Exp_2 = objProspect.objNeedAnalysis.objFamilyIncome.TotalProtection;
                            objtblLifeNeedAnalysis.Gap_Identified = objProspect.objNeedAnalysis.objFamilyIncome.GapIdentified;
                            objtblLifeNeedAnalysis.RateOfInterest = objProspect.objNeedAnalysis.objFamilyIncome.RateOfInterest;
                            objtblLifeNeedAnalysis.IsCoveredUnderOtherPolicy = objProspect.objNeedAnalysis.objFamilyIncome.IsOtherInsurance;
                            objtblLifeNeedAnalysis.NoOfJanashaktiPolicies = objProspect.objNeedAnalysis.objFamilyIncome.NoOfJanashaktiPolicy;
                            objtblLifeNeedAnalysis.NoOfOtherPolicies = objProspect.objNeedAnalysis.objFamilyIncome.NoOfOtherPolicies;

                        }
                        if (objProspect.objNeedAnalysis.objFamilyIncome.objLstPrevInsuranceDetails != null && objProspect.objNeedAnalysis.objFamilyIncome.objLstPrevInsuranceDetails != null)
                        {
                            if (objProspect.objNeedAnalysis.NeedAnalysisID > 0)
                            {
                                DeleteExisitingInsurerInfo(Convert.ToInt32(objProspect.objNeedAnalysis.NeedAnalysisID));
                            }

                            for (int i = 0; i < objProspect.objNeedAnalysis.objFamilyIncome.objLstPrevInsuranceDetails.Count; i++)
                            {
                                objtblPreviousInsurenceInfo.CompanyName = objProspect.objNeedAnalysis.objFamilyIncome.objLstPrevInsuranceDetails[i].Company;
                                objtblPreviousInsurenceInfo.Policy_ProposalNo = objProspect.objNeedAnalysis.objFamilyIncome.objLstPrevInsuranceDetails[i].PolicyOrProposalNo;
                                objtblPreviousInsurenceInfo.TotalSIAtDeath = Convert.ToString(objProspect.objNeedAnalysis.objFamilyIncome.objLstPrevInsuranceDetails[i].Death);
                                objtblPreviousInsurenceInfo.AccidentalBenifit = Convert.ToString(objProspect.objNeedAnalysis.objFamilyIncome.objLstPrevInsuranceDetails[i].Accidental);
                                objtblPreviousInsurenceInfo.CriticalIllnessBenifit = Convert.ToString(objProspect.objNeedAnalysis.objFamilyIncome.objLstPrevInsuranceDetails[i].Critical);
                                objtblPreviousInsurenceInfo.HospitalizationPerDay = Convert.ToString(objProspect.objNeedAnalysis.objFamilyIncome.objLstPrevInsuranceDetails[i].Hospitalization);
                                objtblPreviousInsurenceInfo.CurrentStatus = objProspect.objNeedAnalysis.objFamilyIncome.objLstPrevInsuranceDetails[i].CurrentStatus;
                                objtblPreviousInsurenceInfo.IsDeleted = objProspect.objNeedAnalysis.objFamilyIncome.objLstPrevInsuranceDetails[i].IsDeleted;
                                objtblPreviousInsurenceInfo.NeedAnalysisID = objtblLifeNeedAnalysis.NeedAnalysisID;
                                objtblLifeNeedAnalysis.tblPreviousInsurenceInfoes.Add(objtblPreviousInsurenceInfo);
                            }
                        }
                        if (IsNeedAnalysisInfoAdded)
                        {
                            IsNeedAnalysisInfoAdded = ValidateFamilyIncome(objProspect);
                        }
                    }
                    #endregion

                    #region Needs

                    if (objProspect.objNeedAnalysis.objNeeds != null)
                    {
                        if (objProspect.objNeedAnalysis.NeedAnalysisID > 0)
                        {
                            UpdateNeedStatus(Convert.ToInt32(objProspect.objNeedAnalysis.NeedAnalysisID));
                        }

                        foreach (var item in objProspect.objNeedAnalysis.objNeeds.Where(a => a.IsNeedOpted).ToList())
                        {
                            tblNeed objNeed = new tblNeed();
                            tblNeed ExisitingNeed = new tblNeed();
                            if (item.PKNeedID > 0)
                            {
                                ExisitingNeed = Context.tblNeeds.Where(a => a.NeedID == item.PKNeedID).FirstOrDefault();
                                objNeed = Context.tblNeeds.Where(a => a.NeedID == item.PKNeedID).FirstOrDefault();
                            }
                            objNeed.NeedName = item.NeedName;
                            objNeed.IdentifiedNeed = item.NeedID;
                            objNeed.NeedAnalysisID = objtblLifeNeedAnalysis.NeedAnalysisID;
                            objNeed.Priority = item.Priority;
                            objNeed.Value = item.Value;
                            objNeed.PlanSuggested = item.PlanSuggested;
                            objNeed.StatusID = 1;// Open Status
                            if (item.PKNeedID > 0)
                            {
                                Context.Entry(ExisitingNeed).CurrentValues.SetValues(objNeed);
                            }
                            else
                            {
                                objNeed.tblLifeNeedAnalysi = objtblLifeNeedAnalysis;
                                objtblLifeNeedAnalysis.tblNeeds.Add(objNeed);
                            }

                        }
                        if (objProspect.objNeedAnalysis.Total != null)
                        {
                            objtblLifeNeedAnalysis.TotalNeedValue = Convert.ToDecimal(objProspect.objNeedAnalysis.Total);
                        }
                        if (IsNeedAnalysisInfoAdded)
                        {
                            IsNeedAnalysisInfoAdded = ValidateNeeds(objProspect);
                        }
                    }
                    #endregion

                    #region Additional Information

                    objtblLifeNeedAnalysis.Date_NextMeeting = objProspect.objNeedAnalysis.DateOfNextMeeting;
                    if (objProspect.objNeedAnalysis.TimeOfNextMeeting != null)
                    {
                        objtblLifeNeedAnalysis.Time_NextMeeting = TimeSpan.Parse(objProspect.objNeedAnalysis.TimeOfNextMeeting);
                    }
                    objtblLifeNeedAnalysis.PurposeOfMeeting = objProspect.objNeedAnalysis.PurposeOfMeeting;
                    objtblLifeNeedAnalysis.ProspectSign = objProspect.objNeedAnalysis.ProspectSign;
                    objtblLifeNeedAnalysis.CreatedDate = objProspect.objNeedAnalysis.CreatedDate;
                    objtblLifeNeedAnalysis.UploadSignPath = objProspect.objNeedAnalysis.UploadSignPath;
                    objtblLifeNeedAnalysis.ProductsSelected = objProspect.objNeedAnalysis.SelectedProducts;
                    try
                    {
                        objtblLifeNeedAnalysis.NeedAnalysispath = objProspect.objNeedAnalysis.NeedAnalysisFileAttachment;
                    }
                    catch (Exception)
                    {


                    }
                    if (IsNeedAnalysisInfoAdded)
                    {
                        IsNeedAnalysisInfoAdded = ValidateAdditionalDetails(objProspect);
                    }
                    #endregion

                    #region Adding Need Analysis Info To table
                    objtblLifeNeedAnalysis.tblContact = objContact;
                    if (objProspect.objNeedAnalysis.NeedAnalysisID > 0)
                    {

                        Context.Entry(ExisitingtblLifeNeedAnalysis).CurrentValues.SetValues(objtblLifeNeedAnalysis);
                    }
                    else
                    {
                        objOppurtunity.tblLifeNeedAnalysi = objtblLifeNeedAnalysis;
                        Context.tblLifeNeedAnalysis.Add(objtblLifeNeedAnalysis);
                    }

                    #endregion

                    if (IsNeedAnalysisInfoAdded)
                    {
                        objProspect.IsNeedAnalysisCompleted = true;
                    }
                    else
                    {
                        objProspect.IsNeedAnalysisCompleted = true;
                    }
                    objtblLifeNeedAnalysis.chkProtection1 = objProspect.objNeedAnalysis.chkProtection1;
                    objtblLifeNeedAnalysis.chkProtection2 = objProspect.objNeedAnalysis.chkProtection2;
                    objtblLifeNeedAnalysis.chkProtection3 = objProspect.objNeedAnalysis.chkProtection3;
                    objtblLifeNeedAnalysis.chkProtection4 = objProspect.objNeedAnalysis.chkProtection4;
                    objtblLifeNeedAnalysis.chkProtection5 = objProspect.objNeedAnalysis.chkProtection5;
                    objtblLifeNeedAnalysis.chkRetire1 = objProspect.objNeedAnalysis.chkRetirement1;
                    objtblLifeNeedAnalysis.chkRetire2 = objProspect.objNeedAnalysis.chkRetirement2;
                    objtblLifeNeedAnalysis.chkRetire3 = objProspect.objNeedAnalysis.chkRetirement3;
                    objtblLifeNeedAnalysis.chkRetire4 = objProspect.objNeedAnalysis.chkRetirement4;
                    objtblLifeNeedAnalysis.chkRetire5 = objProspect.objNeedAnalysis.chkRetirement5;
                    objtblLifeNeedAnalysis.chkSaving1 = objProspect.objNeedAnalysis.chkSaving1;
                    objtblLifeNeedAnalysis.chkSaving2 = objProspect.objNeedAnalysis.chkSaving2;
                    objtblLifeNeedAnalysis.chkSaving3 = objProspect.objNeedAnalysis.chkSaving3;
                    objtblLifeNeedAnalysis.chkSaving4 = objProspect.objNeedAnalysis.chkSaving4;
                    objtblLifeNeedAnalysis.chkSaving5 = objProspect.objNeedAnalysis.chkSaving5;
                    objtblLifeNeedAnalysis.chkEdu1 = objProspect.objNeedAnalysis.chkEducation1;
                    objtblLifeNeedAnalysis.chkEdu2 = objProspect.objNeedAnalysis.chkEducation2;
                    objtblLifeNeedAnalysis.chkEdu3 = objProspect.objNeedAnalysis.chkEducation3;
                    objtblLifeNeedAnalysis.chkEdu4 = objProspect.objNeedAnalysis.chkEducation4;
                    objtblLifeNeedAnalysis.chkEdu5 = objProspect.objNeedAnalysis.chkEducation5;
                    objtblLifeNeedAnalysis.chkHealth1 = objProspect.objNeedAnalysis.chkHealth1;
                    objtblLifeNeedAnalysis.chkHealth2 = objProspect.objNeedAnalysis.chkHealth2;
                    objtblLifeNeedAnalysis.chkHealth3 = objProspect.objNeedAnalysis.chkHealth3;
                    objtblLifeNeedAnalysis.chkHealth4 = objProspect.objNeedAnalysis.chkHealth4;
                    objtblLifeNeedAnalysis.chkHealth5 = objProspect.objNeedAnalysis.chkHealth5;

                    objtblLifeNeedAnalysis.FromYear = objProspect.objNeedAnalysis.FNAFromYear;
                    objtblLifeNeedAnalysis.ToYear = objProspect.objNeedAnalysis.FNAToYear;
                    objtblLifeNeedAnalysis.InflationRate = objProspect.objNeedAnalysis.FNAInflationRate;
                    objtblLifeNeedAnalysis.RateOfInterest = objProspect.objNeedAnalysis.FNAIntrestRate;
                    objtblLifeNeedAnalysis.Land_Assets = objProspect.objNeedAnalysis.Assets0;
                    objtblLifeNeedAnalysis.FixedDeposit = objProspect.objNeedAnalysis.Assets1;
                    objtblLifeNeedAnalysis.Shares = objProspect.objNeedAnalysis.Assets2;
                    objtblLifeNeedAnalysis.Vehicle = objProspect.objNeedAnalysis.Assets3;
                    objtblLifeNeedAnalysis.Jewellery = objProspect.objNeedAnalysis.Assets4;
                    objtblLifeNeedAnalysis.OtherAssets = objProspect.objNeedAnalysis.Assets5;
                    objtblLifeNeedAnalysis.TotalAssets = objProspect.objNeedAnalysis.objAssetsAndLiabilities.AssetsTotal;
                    objtblLifeNeedAnalysis.TotalLiability = objProspect.objNeedAnalysis.objAssetsAndLiabilities.LiabilityTotal;
                    objtblLifeNeedAnalysis.InsuredTotalLiability = objProspect.objNeedAnalysis.objAssetsAndLiabilities.InsuredLiabilityTotal;
                    objtblLifeNeedAnalysis.Loan = objProspect.objNeedAnalysis.Liabilityone0;
                    objtblLifeNeedAnalysis.InsuredLoan = objProspect.objNeedAnalysis.Liabilitytwo0;
                    objtblLifeNeedAnalysis.CreditCard = objProspect.objNeedAnalysis.Liabilityone1;
                    objtblLifeNeedAnalysis.InsuredCreditCard = objProspect.objNeedAnalysis.Liabilitytwo1;
                    objtblLifeNeedAnalysis.Lease = objProspect.objNeedAnalysis.Liabilityone2;
                    objtblLifeNeedAnalysis.InsuredLease = objProspect.objNeedAnalysis.Liabilitytwo2;
                    objtblLifeNeedAnalysis.OtherLiability = objProspect.objNeedAnalysis.Liabilityone3;
                    objtblLifeNeedAnalysis.InsuredOtherLiability = objProspect.objNeedAnalysis.Liabilitytwo3;

                    objtblLifeNeedAnalysis.NetAssets = objProspect.objNeedAnalysis.objAssetsAndLiabilities.NetAssests;
                    objtblLifeNeedAnalysis.LumpSumReq = objProspect.objNeedAnalysis.objAssetsAndLiabilities.LumpsumRequirement;
                    objtblLifeNeedAnalysis.Salary = objProspect.objNeedAnalysis.Income0;
                    if (objProspect.objNeedAnalysis.Income0 == null)
                    {
                        IsSuspectComplete = false;
                    }
                    else
                    {
                        IsSuspectComplete = true;
                    }
                    objtblLifeNeedAnalysis.Intrest = objProspect.objNeedAnalysis.Income1;
                    objtblLifeNeedAnalysis.Rent = objProspect.objNeedAnalysis.Income2;
                    objtblLifeNeedAnalysis.OtherIncome = objProspect.objNeedAnalysis.Income3;
                    objtblLifeNeedAnalysis.TotalIncome = objProspect.objNeedAnalysis.objFamilyIncome.TotalIncome;
                    objtblLifeNeedAnalysis.AnnualExp = objProspect.objNeedAnalysis.Expense0;
                    objtblLifeNeedAnalysis.AnnualVacation = objProspect.objNeedAnalysis.Expense1;
                    objtblLifeNeedAnalysis.Installment = objProspect.objNeedAnalysis.Expense2;
                    objtblLifeNeedAnalysis.VehExp = objProspect.objNeedAnalysis.Expense3;
                    objtblLifeNeedAnalysis.LoanExp = objProspect.objNeedAnalysis.Expense4;
                    objtblLifeNeedAnalysis.OtherExp = objProspect.objNeedAnalysis.Expense5;
                    objtblLifeNeedAnalysis.LumpSumReqExp = objProspect.objNeedAnalysis.objFamilyIncome.IncomeLumpsumRequirement;
                    objtblLifeNeedAnalysis.SurplusExp = objProspect.objNeedAnalysis.objAssetsAndLiabilities.SurPlusAssets;
                    objtblLifeNeedAnalysis.CriticalIllnessReq = objProspect.objNeedAnalysis.CriticalIllnessRequirement;
                    objtblLifeNeedAnalysis.CriticalIllnessFund = objProspect.objNeedAnalysis.CriticalIllnessAvailable;
                    objtblLifeNeedAnalysis.CriticalIllnessGap = objProspect.objNeedAnalysis.CriticalIllnessGap;
                    objtblLifeNeedAnalysis.HospitalizationReq = objProspect.objNeedAnalysis.HospitalRequirement;
                    objtblLifeNeedAnalysis.HospitalizationFund = objProspect.objNeedAnalysis.HospitalAvailable;
                    objtblLifeNeedAnalysis.HospitalizationGap = objProspect.objNeedAnalysis.HospitalGap;
                    objtblLifeNeedAnalysis.TotalReq = objProspect.objNeedAnalysis.TotalRequirement;
                    objtblLifeNeedAnalysis.TotalFund = objProspect.objNeedAnalysis.TotalAvailable;
                    objtblLifeNeedAnalysis.TotalGap = objProspect.objNeedAnalysis.TotalGap;
                    objtblLifeNeedAnalysis.AddExpReq = objProspect.objNeedAnalysis.AdditionalRequirement;
                    objtblLifeNeedAnalysis.AddExpFund = objProspect.objNeedAnalysis.AdditionalAvailable;
                    objtblLifeNeedAnalysis.AddExpGap = objProspect.objNeedAnalysis.AdditionalGap;
                    objtblLifeNeedAnalysis.WealthReq = objProspect.objNeedAnalysis.WealthRequirement;
                    objtblLifeNeedAnalysis.IncomeReq = objProspect.objNeedAnalysis.LivingExpense;
                    objtblLifeNeedAnalysis.DreamReq = objProspect.objNeedAnalysis.FinancialExpense;
                    objtblLifeNeedAnalysis.MaturityDreamReq = objProspect.objNeedAnalysis.FinancialExpense2017;
                    objtblLifeNeedAnalysis.TotalReq1 = objProspect.objNeedAnalysis.TotalExpense;
                    objtblLifeNeedAnalysis.MaturityTotalReq1 = objProspect.objNeedAnalysis.TotalExpense2017;
                    objtblLifeNeedAnalysis.EmergencyPolicy1 = objProspect.objNeedAnalysis.EmergencyFund1;
                    objtblLifeNeedAnalysis.EmergencyPolicy2 = objProspect.objNeedAnalysis.EmergencyFund2;
                    objtblLifeNeedAnalysis.EmergencyPolicy3 = objProspect.objNeedAnalysis.EmergencyFund3;
                    objtblLifeNeedAnalysis.EmergencyTotal2 = objProspect.objNeedAnalysis.TotalEmergencyFund;
                    objtblLifeNeedAnalysis.MaturityPolicy1 = objProspect.objNeedAnalysis.MaturityFund1;
                    objtblLifeNeedAnalysis.MaturityPolicy2 = objProspect.objNeedAnalysis.MaturityFund2;
                    objtblLifeNeedAnalysis.MaturityPolicy3 = objProspect.objNeedAnalysis.MaturityFund3;
                    objtblLifeNeedAnalysis.MaturityTotal2 = objProspect.objNeedAnalysis.TotalMaturityFund;
                    if (objProspect.objNeedAnalysis.EmergencyFundGap != null || objProspect.objNeedAnalysis.MaturityFundGap != null)
                    {
                        IsSuspectComplete = true;
                    }
                    objtblLifeNeedAnalysis.Gap1 = objProspect.objNeedAnalysis.EmergencyFundGap;
                    objtblLifeNeedAnalysis.Gap2 = objProspect.objNeedAnalysis.MaturityFundGap;
                    if (objProspect.objNeedAnalysis.SelectedProducts != null)
                    {
                        IsProductSelected = true;
                    }
                    objtblLifeNeedAnalysis.ProductsSelected = objProspect.objNeedAnalysis.SelectedProducts;
                    objtblLifeNeedAnalysis.UploadSignPath = objProspect.Upload;
                    objtblLifeNeedAnalysis.NoOfOtherPolicies = objProspect.objNeedAnalysis.DependantCount;
                    objOppurtunity.NeedAnalysisID = objtblLifeNeedAnalysis.NeedAnalysisID;
                    objContact.tblLifeNeedAnalysis.Add(objtblLifeNeedAnalysis);


                    foreach (var item in objProspect.objNeedAnalysis.objFinancialNeeds)
                    {
                        tblNeedFinancialNeed obj = new tblNeedFinancialNeed();
                        obj.CurrentReq = item.CurrReq;
                        obj.EstimatedAmount = item.EstAmount;
                        obj.FundBalance = item.FundBalance;
                        obj.Gap = item.Gap;
                        obj.Name = item.Name;
                        obj.NeedAnalysisID = objProspect.objNeedAnalysis.NeedAnalysisID;

                        objtblLifeNeedAnalysis.tblNeedFinancialNeeds.Add(obj);
                    }
                    objtblLifeNeedAnalysis.FinancialCurrReqTotal = objProspect.objNeedAnalysis.objFinancialNeed.RequirementTotal;
                    objtblLifeNeedAnalysis.FinancialEstAmount = objProspect.objNeedAnalysis.objFinancialNeed.EstimateTotal;
                    objtblLifeNeedAnalysis.FinancialFund = objProspect.objNeedAnalysis.objFinancialNeed.FundBalanceTotal;
                    objtblLifeNeedAnalysis.FinancialGap = objProspect.objNeedAnalysis.objFinancialNeed.GapTotal;
                    tblNeedRetirementCalculator objRetire = new tblNeedRetirementCalculator();
                    objRetire.FromYear = objProspect.objNeedAnalysis.CalculatorFromYear;
                    objRetire.ToYear = objProspect.objNeedAnalysis.CalculatorToYear;
                    objRetire.InflationRate = objProspect.objNeedAnalysis.CalculatorInflationRate;
                    objRetire.PlanNoYears = objProspect.objNeedAnalysis.CalculatorPlanNoYears;
                    objRetire.IntrestRate = objProspect.objNeedAnalysis.CalculatorIntrestRate;
                    objRetire.TotalMonthlyExp = objProspect.objNeedAnalysis.objCalculator.TotalMonthlyExpense;
                    objRetire.EstMonthlyExp = objProspect.objNeedAnalysis.objCalculator.EstimatedTotalMonthlyExpense;
                    objRetire.CurrentFoodExp = objProspect.objNeedAnalysis.objCalculator.FoodExpense;
                    objRetire.EstFoodExp = objProspect.objNeedAnalysis.objCalculator.EstimatedFoodExpense;
                    objRetire.CurrentWaterExp = objProspect.objNeedAnalysis.objCalculator.WaterExpense;
                    objRetire.EstWaterExp = objProspect.objNeedAnalysis.objCalculator.EstimatedWaterExpense;
                    objRetire.CurrentRentExp = objProspect.objNeedAnalysis.objCalculator.RentExpense;
                    objRetire.EstRentExp = objProspect.objNeedAnalysis.objCalculator.EstimatedRentExpense;
                    objRetire.CurrentLeaseExp = objProspect.objNeedAnalysis.objCalculator.LeaseExpense;
                    objRetire.EstLeaseExp = objProspect.objNeedAnalysis.objCalculator.EstimatedLeaseExpense;
                    objRetire.CurrentTransportExp = objProspect.objNeedAnalysis.objCalculator.TransportExpense;
                    objRetire.EstTransportExp = objProspect.objNeedAnalysis.objCalculator.EstimatedTransportExpense;
                    objRetire.CurrentMedExp = objProspect.objNeedAnalysis.objCalculator.MedicineExpense;
                    objRetire.EstMedExp = objProspect.objNeedAnalysis.objCalculator.EstimatedMedicineExpense;
                    objRetire.CurrentEduExp = objProspect.objNeedAnalysis.objCalculator.EducationExpense;
                    objRetire.EstEduExp = objProspect.objNeedAnalysis.objCalculator.EstimatedEducationExpense;
                    objRetire.CurrentClothesExp = objProspect.objNeedAnalysis.objCalculator.ClothesExpense;
                    objRetire.EstClothesExp = objProspect.objNeedAnalysis.objCalculator.EstimatedClothesExpense;
                    objRetire.CurrentEntertainmentExp = objProspect.objNeedAnalysis.objCalculator.EntertainmentExpense;
                    objRetire.EstEntertainmentExp = objProspect.objNeedAnalysis.objCalculator.EstimatedEntertainmentExpense;
                    objRetire.CurrentCharity = objProspect.objNeedAnalysis.objCalculator.CharityExpense;
                    objRetire.EstCharity = objProspect.objNeedAnalysis.objCalculator.EstimatedCharityExpense;
                    objRetire.CurrentOtherExp = objProspect.objNeedAnalysis.objCalculator.OtherExpense;
                    objRetire.EstOtherExp = objProspect.objNeedAnalysis.objCalculator.EstimatedOtherExpense;
                    objRetire.CurrentMonthlySalary = objProspect.objNeedAnalysis.objCalculator.Salary;
                    objRetire.CurrentEPFBalance = objProspect.objNeedAnalysis.objCalculator.CurrentEPFBalance;
                    objRetire.EstEPFBalance = objProspect.objNeedAnalysis.objCalculator.EstimatedEPFBalance;
                    objRetire.CurrentMonthly20Sal = objProspect.objNeedAnalysis.objCalculator.MonthlyAllocation20;
                    objRetire.CurrentETFBalance = objProspect.objNeedAnalysis.objCalculator.CurrentETFBalance;
                    objRetire.EstETFBalance = objProspect.objNeedAnalysis.objCalculator.EstimatedETFBalance;
                    objRetire.CurrentMonthly3Sal = objProspect.objNeedAnalysis.objCalculator.MonthlyAllocation3;
                    objRetire.CurrentGratuityFund = objProspect.objNeedAnalysis.objCalculator.CurrentGratuityFund;
                    objRetire.EstGratuityFund = objProspect.objNeedAnalysis.objCalculator.EstimatedGratuityFund;
                    objRetire.TotalEstMonthlyExpFund = objProspect.objNeedAnalysis.objCalculator.TotalRetirementFund;
                    objRetire.ChildEduFund = objProspect.objNeedAnalysis.Financial0;
                    objRetire.ChildWeddingFund = objProspect.objNeedAnalysis.Financial1;
                    objRetire.VehicleFund = objProspect.objNeedAnalysis.Financial2;
                    objRetire.LoanFund = objProspect.objNeedAnalysis.Financial3;
                    objRetire.OtherFund = objProspect.objNeedAnalysis.Financial4;
                    objRetire.FundBalance = objProspect.objNeedAnalysis.objCalculator.FundBalanceTotal;
                    objRetire.PerAnnIncomeIntrest = objProspect.objNeedAnalysis.objCalculator.PerAnnumIncome;
                    objRetire.EstAnnualLivExp = objProspect.objNeedAnalysis.objCalculator.EstimatedAnnuallivingExpenses;
                    objRetire.TotalAnnualExp = objProspect.objNeedAnalysis.objCalculator.AnnualIncomeSurplus;
                    objRetire.ExistingOthIncome = objProspect.objNeedAnalysis.objCalculator.Exsitingotherincome;
                    if (objProspect.objNeedAnalysis.objCalculator.MonthlyPensionGap != null)
                    {
                        IsSuspectComplete = true;
                    }
                    objRetire.PensionGap = objProspect.objNeedAnalysis.objCalculator.MonthlyPensionGap;
                    objtblLifeNeedAnalysis.tblNeedRetirementCalculators.Add(objRetire);

                    tblNeedHealthCalculator objHealth = new tblNeedHealthCalculator();
                    objHealth.CriticalillnessReq = objProspect.objNeedAnalysis.CriticalRequiremenent;
                    objHealth.CriticalIllenssFund = objProspect.objNeedAnalysis.CriticalFund;
                    objHealth.CriticalIllnessGap = objProspect.objNeedAnalysis.CriticalGap;
                    objHealth.HospReq = objProspect.objNeedAnalysis.HospitalizationRequiremenent;
                    objHealth.HospFund = objProspect.objNeedAnalysis.HospitalizationFund;
                    objHealth.HospGap = objProspect.objNeedAnalysis.HospitalizationGap;
                    objHealth.AddLossReq = objProspect.objNeedAnalysis.additionalexpenseRequiremenent;
                    objHealth.AddLossFund = objProspect.objNeedAnalysis.additionalexpenseFund;
                    objHealth.AddLossGap = objProspect.objNeedAnalysis.additionalexpenseGap;
                    objHealth.HospitalBills = objProspect.objNeedAnalysis.HospitalBills;
                    objHealth.HospRetireExp = objProspect.objNeedAnalysis.HospitalRtrExp;
                    objHealth.HealthAdversities = String.Join(",", objProspect.objNeedAnalysis.objadversities);
                    objHealth.AnnualAmountHealthExp = objProspect.objNeedAnalysis.objannualamount;
                    objHealth.CoverageHealthExp = objProspect.objNeedAnalysis.objcoverage;
                    objHealth.AdequacyHealthExp = objProspect.objNeedAnalysis.objadequacy;
                    if (objProspect.objNeedAnalysis.CriticalGap == null && objProspect.objNeedAnalysis.HospitalizationGap == null && objProspect.objNeedAnalysis.additionalexpenseGap == null)
                    {
                    }
                    else
                    {
                        IsSuspectComplete = true;

                    }
                    objtblLifeNeedAnalysis.tblNeedHealthCalculators.Add(objHealth);

                    tblNeedEducationCalculator objEdu = new tblNeedEducationCalculator();
                    objEdu.Inflationrate = objProspect.objNeedAnalysis.EduInflationRate;
                    objEdu.AnnualEduExp = objProspect.objNeedAnalysis.AnnualEduExpense;
                    objEdu.EduMaturityValue = objProspect.objNeedAnalysis.EduMaturity;
                    objEdu.LumpSum = objProspect.objNeedAnalysis.EduLumpSum;
                    if (objProspect.objNeedAnalysis.EduGapTotal != null)
                    {
                        IsSuspectComplete = true;
                    }
                    objEdu.EduGapTotal = objProspect.objNeedAnalysis.EduGapTotal;
                    objEdu.NeedAnalysisID = objProspect.objNeedAnalysis.NeedAnalysisID;
                    objtblLifeNeedAnalysis.tblNeedEducationCalculators.Add(objEdu);

                    List<tblNeedEduGCEAL> objGCEAL = new List<tblNeedEduGCEAL>();
                    foreach (var item in objProspect.objNeedAnalysis.objGCEAL)
                    {
                        tblNeedEduGCEAL obj = new tblNeedEduGCEAL();
                        obj.CurrentReq = item.CurrRequirement;
                        obj.MaturityAge = item.MaturityAge;
                        obj.Term = item.Term;
                        obj.EstAmount = item.EstAmount;
                        obj.AvailableFund = item.AvailableFund;
                        obj.Gap = item.Gap;
                        obj.Relationship = item.Relationship;
                        obj.Age = item.Age;
                        obj.EduCalcID = objEdu.Id;
                        objEdu.tblNeedEduGCEALs.Add(obj);
                    }

                    List<tblNeedEduForeign> objForeign = new List<tblNeedEduForeign>();
                    foreach (var item in objProspect.objNeedAnalysis.objHigherForeign)
                    {
                        tblNeedEduForeign obj = new tblNeedEduForeign();
                        obj.CurrentReq = item.CurrRequirement;
                        obj.Term = item.Term;
                        obj.MaturityAge = item.MaturityAge;
                        obj.EstAmount = item.EstAmount;
                        obj.AvailableFund = item.AvailableFund;
                        obj.Gap = item.Gap;
                        obj.Relationship = item.Relationship;
                        obj.Age = item.Age;
                        obj.EduCalcID = objEdu.Id;

                        objEdu.tblNeedEduForeigns.Add(obj);
                    }
                    List<tblNeedEduHigher> objHigher = new List<tblNeedEduHigher>();
                    foreach (var item in objProspect.objNeedAnalysis.objHigherEdu)
                    {
                        tblNeedEduHigher obj = new tblNeedEduHigher();
                        obj.CurrentReq = item.CurrRequirement;
                        obj.Term = item.Term;
                        obj.MaturityAge = item.MaturityAge;
                        obj.EstAmount = item.EstAmount;
                        obj.AvailableFund = item.AvailableFund;
                        obj.Gap = item.Gap;
                        obj.Relationship = item.Relationship;
                        obj.Age = item.Age;
                        obj.EduCalcID = objEdu.Id;
                        objEdu.tblNeedEduHighers.Add(obj);
                    }
                    List<tblNeedEduLocal> objLocal = new List<tblNeedEduLocal>();
                    foreach (var item in objProspect.objNeedAnalysis.objHigherEdu)
                    {
                        tblNeedEduLocal obj = new tblNeedEduLocal();
                        obj.CurrentReq = item.CurrRequirement;
                        obj.Term = item.Term;
                        obj.MaturityAge = item.MaturityAge;
                        obj.EstAmount = item.EstAmount;
                        obj.AvailableFund = item.AvailableFund;
                        obj.Gap = item.Gap;
                        obj.Relationship = item.Relationship;
                        obj.Age = item.Age;
                        obj.EduCalcID = objEdu.Id;
                        objEdu.tblNeedEduLocals.Add(obj);
                    }

                    tblNeedSavingCalculator objSave = new tblNeedSavingCalculator();
                    objSave.Inflationrate = objProspect.objNeedAnalysis.EduInflationRate;
                    objSave.AnnualSavingExp = objProspect.objNeedAnalysis.AnnualSaveExpense;
                    if (objProspect.objNeedAnalysis.SavingReqTotal != null && objProspect.objNeedAnalysis.SavingEstTotal != null && objProspect.objNeedAnalysis.SavingCurrentTotal != null && objProspect.objNeedAnalysis.SavingGapTotal != null)
                    {
                        IsSuspectComplete = true;
                    }
                    objSave.CurrReqTotal = objProspect.objNeedAnalysis.SavingReqTotal;
                    objSave.EstAmountTotal = objProspect.objNeedAnalysis.SavingEstTotal;
                    objSave.AvailableFund = objProspect.objNeedAnalysis.SavingCurrentTotal;
                    objSave.GapTotal = objProspect.objNeedAnalysis.SavingGapTotal;
                    objtblLifeNeedAnalysis.tblNeedSavingCalculators.Add(objSave);

                    List<tblNeedSaveCar> objCar = new List<tblNeedSaveCar>();
                    foreach (var item in objProspect.objNeedAnalysis.objCar)
                    {
                        tblNeedSaveCar obj = new tblNeedSaveCar();
                        obj.CurrentReq = item.CurrRequirement;
                        obj.Term = item.Term;
                        obj.EstAmount = item.EstAmount;
                        obj.AvailableFund = item.AvailableFund;
                        obj.Gap = item.Gap;
                        obj.Relationship = item.Relationship;
                        obj.Age = item.Age;
                        obj.MaturityAge = item.MaturityAge;
                        obj.SaveCalcID = objSave.Id;
                        objSave.tblNeedSaveCars.Add(obj);
                    }

                    List<tblNeedSaveHouse> objHouse = new List<tblNeedSaveHouse>();
                    foreach (var item in objProspect.objNeedAnalysis.objHouse)
                    {
                        tblNeedSaveHouse obj = new tblNeedSaveHouse();
                        obj.CurrentReq = item.CurrRequirement;
                        obj.Term = item.Term;
                        obj.EstAmount = item.EstAmount;
                        obj.AvailableFund = item.AvailableFund;
                        obj.Gap = item.Gap;
                        obj.Relationship = item.Relationship;
                        obj.Age = item.Age;
                        obj.MaturityAge = item.MaturityAge;
                        obj.SaveCalcID = objSave.Id;
                        objSave.tblNeedSaveHouses.Add(obj);
                    }
                    List<tblNeedSaveOther> objOther = new List<tblNeedSaveOther>();
                    foreach (var item in objProspect.objNeedAnalysis.objOthers)
                    {
                        tblNeedSaveOther obj = new tblNeedSaveOther();
                        obj.CurrentReq = item.CurrRequirement;
                        obj.Term = item.Term;
                        obj.EstAmount = item.EstAmount;
                        obj.AvailableFund = item.AvailableFund;
                        obj.Gap = item.Gap;
                        obj.Relationship = item.Relationship;
                        obj.Age = item.Age;
                        obj.MaturityAge = item.MaturityAge;
                        obj.SaveCalcID = objSave.Id;
                        objSave.tblNeedSaveOthers.Add(obj);
                    }
                    List<tblNeedSaveTour> objTour = new List<tblNeedSaveTour>();
                    foreach (var item in objProspect.objNeedAnalysis.objForeignTour)
                    {
                        tblNeedSaveTour obj = new tblNeedSaveTour();
                        obj.CurrentReq = item.CurrRequirement;
                        obj.Term = item.Term;
                        obj.EstAmount = item.EstAmount;
                        obj.AvailableFund = item.AvailableFund;
                        obj.Gap = item.Gap;
                        obj.Relationship = item.Relationship;
                        obj.Age = item.Age;
                        obj.MaturityAge = item.MaturityAge;
                        obj.SaveCalcID = objSave.Id;
                        objSave.tblNeedSaveTours.Add(obj);
                    }
                    List<tblNeedSaveWedding> objWedding = new List<tblNeedSaveWedding>();
                    foreach (var item in objProspect.objNeedAnalysis.objWedding)
                    {
                        tblNeedSaveWedding obj = new tblNeedSaveWedding();
                        obj.CurrentReq = item.CurrRequirement;
                        obj.Term = item.Term;
                        obj.EstAmount = item.EstAmount;
                        obj.AvailableFund = item.AvailableFund;
                        obj.Gap = item.Gap;
                        obj.Relationship = item.Relationship;
                        obj.Age = item.Age;
                        obj.MaturityAge = item.MaturityAge;
                        obj.SaveCalcID = objSave.Id;
                        objSave.tblNeedSaveWeddings.Add(obj);
                    }

                    tblNeedHumanValueCalculator objHumanValue = new tblNeedHumanValueCalculator();
                    objHumanValue.MonthlyEarning = objProspect.objNeedAnalysis.MonthlyEarning;
                    objHumanValue.NoOfYears = objProspect.objNeedAnalysis.YearsofEarning;
                    objHumanValue.IntrestRate = objProspect.objNeedAnalysis.ProIntrestRate;
                    objHumanValue.EstIncome = objProspect.objNeedAnalysis.EstimatedIncome;
                    objHumanValue.FutureAvailableFund = objProspect.objNeedAnalysis.FutureFund;
                    if (objProspect.objNeedAnalysis.EmergencyFund != null)
                    {
                        IsSuspectComplete = true;
                    }
                    objHumanValue.EmergencyFundReq = objProspect.objNeedAnalysis.EmergencyFund;
                    objHumanValue.AvailableFund = objProspect.objNeedAnalysis.AvailableFund;
                    objtblLifeNeedAnalysis.tblNeedHumanValueCalculators.Add(objHumanValue);

                }
                else
                {
                    objProspect.IsNeedAnalysisCompleted = false;

                }
                #region Setting Status
                if (IsProspect)
                {
                    objOppurtunity.StageID = 2; // Prospect
                    objProspect.ProspectStage = 2;
                }
                else
                {
                    objOppurtunity.StageID = 1; // Suspect
                }
                if (IsSuspectComplete == true && IsProductSelected == true)
                {
                    objOppurtunity.StageID = 4;//Need Analysis Completed
                    objProspect.ProspectStage = 4;
                }
                else
                {
                    objOppurtunity.StageID = 2; // Prospect
                    objProspect.ProspectStage = 2;

                }
                //if (objProspect.IsNeedAnalysisCompleted && objProspect.IsConfirmedProspect)
                //{
                //    objOppurtunity.StageID = 4; //Need Analysis
                //    objProspect.ProspectStage = 4;
                //}
                //else if (!objProspect.IsNeedAnalysisCompleted && objProspect.IsConfirmedProspect)
                //{
                //    objOppurtunity.StageID = 3; //Confirmed Prospect
                //    objProspect.ProspectStage = 3;
                //}
                //else
                //{
                //    objOppurtunity.StageID = 2; // Prospect
                //    objProspect.ProspectStage = 2;
                //}

                #endregion
                Context.SaveChanges();
                objProspect.Message = "Success";
                return objProspect;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void UpdateNeedStatus(int NeedAnalysisID)
        {

            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    foreach (var Need in Context.tblNeeds.Where(a => a.NeedAnalysisID == NeedAnalysisID).ToList())
                    {
                        tblNeed objNeed = new tblNeed();
                        tblNeed ExisitingNeed = new tblNeed();
                        ExisitingNeed = Context.tblNeeds.Where(a => a.NeedID == Need.NeedID).FirstOrDefault();
                        objNeed = Context.tblNeeds.Where(a => a.NeedID == Need.NeedID).FirstOrDefault();
                        objNeed.IsDeleted = true;
                        Context.Entry(ExisitingNeed).CurrentValues.SetValues(objNeed);
                    }
                    Context.SaveChanges();
                }
            }
            catch (Exception)
            {


            }
        }
        public AIA.Life.Models.Opportunity.LifeQuote LoadProspectMaster(AIA.Life.Models.Opportunity.LifeQuote objQuoteList)
        {
            Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();

            List<int?> lstCommontypeID = new List<int?>();
            lstCommontypeID.Add(269);
            lstCommontypeID.Add(270);
            objQuoteList.objProspect.lstSalutation = objCommonBusiness.GetSalutation();
            objQuoteList.objProspect.lstOccupation = objCommonBusiness.GetOccupation();
            objQuoteList.objProspect.lstGender = objCommonBusiness.GetGender();
            objQuoteList.objProspect.objNeedAnalysis.dllChildRelatioship = objCommonBusiness.GetChildRelationship();
            //objProspect.MaritalStatuslist = objCommonBusiness.GetMasCommonTypeMasterListItem(null, null, lstMaritalStatusCommontypeID);
            objQuoteList.objProspect.MaritalStatuslist = objCommonBusiness.GetMaritalStatus();
            objQuoteList.objProspect.objNeedAnalysis.objDependants = objCommonBusiness.GetDependant(objQuoteList);
            objQuoteList.objProspect.lstRelations = objCommonBusiness.GetMasCommonTypeMasterListItem("HealthRelationship");
            objQuoteList.objProspect.lstMotorVehicle = objCommonBusiness.GetMasCommonTypeMasterListItem("VehicleType");
            objQuoteList.objProspect.lstDependentRelationship = objCommonBusiness.GetMasCommonTypeMasterListItem(null, null, lstCommontypeID);
            objQuoteList.objProspect.lstAvgMonthlyIncome = objCommonBusiness.GetMasCommonTypeMasterListItem("MonthlyIncomeRange");
            objQuoteList.objProspect.lstCurrentStatus = objCommonBusiness.GetMasCommonTypeMasterListItem("PrevInsurenceCurrentStatus");
            objQuoteList.objProspect.lstNeedsPriority = objCommonBusiness.GetMasCommonTypeMasterListItem("NeedPriorityValue");
            objQuoteList.objProspect.lstPurposeOfMeeting = objCommonBusiness.GetMasCommonTypeMasterListItem("PurposeOfNextMeeting");
            objQuoteList.objProspect.ListPlan = objCommonBusiness.ListProducts();
            objQuoteList.objProspect.objNeedAnalysis.objFamilyIncome.RateOfInterest = Convert.ToInt32(ConfigurationManager.AppSettings["RateOfInterest"]);
            objQuoteList.LstPremiumTerm = objCommonBusiness.GetPensionPeriod();
            objQuoteList.lstGender = objCommonBusiness.GetGender();
            objQuoteList.lstOccupation = objCommonBusiness.GetOccupation();
            objQuoteList.lstLanguage = objCommonBusiness.GetMasCommonTypeMasterListItem("Language");
            objQuoteList.lstPrefMode = objCommonBusiness.GetPreferredModes(string.Empty);
            objQuoteList.lstSumInsured = objCommonBusiness.GetSumInsured();
            return objQuoteList;
        }
        public AIA.Life.Models.Opportunity.LifeQuote LoadQuoteMaster(AIA.Life.Models.Opportunity.LifeQuote objQuoteList)
        {
            Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();

            List<int?> lstCommontypeID = new List<int?>();
            lstCommontypeID.Add(269);
            lstCommontypeID.Add(270);
            objQuoteList.objProspect.LstType = objCommonBusiness.GetType();
            objQuoteList.objProspect.lstSalutation = objCommonBusiness.GetSalutation();
            objQuoteList.objProspect.lstOccupation = objCommonBusiness.GetOccupation();
            objQuoteList.objProspect.lstGender = objCommonBusiness.GetGender();

            //objProspect.MaritalStatuslist = objCommonBusiness.GetMasCommonTypeMasterListItem(null, null, lstMaritalStatusCommontypeID);
            objQuoteList.objProspect.MaritalStatuslist = objCommonBusiness.GetMaritalStatus();
            objQuoteList.objProspect.lstRelations = objCommonBusiness.GetMasCommonTypeMasterListItem("HealthRelationship");
            objQuoteList.objProspect.lstMotorVehicle = objCommonBusiness.GetMasCommonTypeMasterListItem("VehicleType");
            objQuoteList.objProspect.lstDependentRelationship = objCommonBusiness.GetMasCommonTypeMasterListItem(null, null, lstCommontypeID);
            objQuoteList.objProspect.lstAvgMonthlyIncome = objCommonBusiness.GetMasCommonTypeMasterListItem("MonthlyIncomeRange");
            objQuoteList.objProspect.lstCurrentStatus = objCommonBusiness.GetMasCommonTypeMasterListItem("PrevInsurenceCurrentStatus");
            objQuoteList.objProspect.lstNeedsPriority = objCommonBusiness.GetMasCommonTypeMasterListItem("NeedPriorityValue");
            objQuoteList.objProspect.lstPurposeOfMeeting = objCommonBusiness.GetMasCommonTypeMasterListItem("PurposeOfNextMeeting");
            objQuoteList.objProspect.ListPlan = objCommonBusiness.ListProducts();
            objQuoteList.objProspect.objNeedAnalysis.objFamilyIncome.RateOfInterest = Convert.ToInt32(ConfigurationManager.AppSettings["RateOfInterest"]);

            objQuoteList.lstGender = objCommonBusiness.GetGender();
            objQuoteList.lstOccupation = objCommonBusiness.GetOccupation();
            objQuoteList.lstLanguage = objCommonBusiness.GetMasCommonTypeMasterListItem("Language");
            objQuoteList.lstPrefMode = objCommonBusiness.GetPreferredModes(string.Empty);
            objQuoteList.lstSumInsured = objCommonBusiness.GetSumInsured();
            objQuoteList.objProspect.LstPensionPeriod = objCommonBusiness.GetPensionPeriod();
            objQuoteList.objProspect.LstRetirementAge = objCommonBusiness.GetRetirementAge();
            objQuoteList.objProspect.LstDrawDownPeriod = objCommonBusiness.GetDrawDownPeriod();
            return objQuoteList;
        }

        public AIA.Life.Models.Opportunity.Prospect LoadContactInformation(AIA.Life.Models.Opportunity.Prospect objProspect, AIA.Life.Models.Opportunity.LifeQuote ObjQuote=null)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                var tblcontact = Context.tblContacts.Where(a => a.ContactID == objProspect.ContactID).FirstOrDefault();
                if (tblcontact != null)
                {
                    objProspect.Type = tblcontact.ContactType;
                    objProspect.Name = tblcontact.FirstName;
                    objProspect.LastName = tblcontact.LastName;
                    objProspect.Email = tblcontact.EmailID;
                    objProspect.Mobile = tblcontact.MobileNo;
                    objProspect.Home = tblcontact.PhoneNo;
                    objProspect.Work = tblcontact.Work;
                    objProspect.AgeNextBdy = Convert.ToInt32(tblcontact.Age);
                    objProspect.CurrentAge = Convert.ToInt32(tblcontact.CurrentAge);
                    objProspect.ClientCode = tblcontact.ClientCode;
                    objProspect.DateofBirth = tblcontact.DateOfBirth;
                    Common.CommonBusiness objcommonbusines = new Common.CommonBusiness();
                    int age = 0;
                    if (ObjQuote != null)
                    {
                        age = objcommonbusines.GetCurrentAge(tblcontact.DateOfBirth, ObjQuote.RiskCommencementDate);

                    }
                    else
                    {
                        age = objcommonbusines.GetCurrentAge(tblcontact.DateOfBirth);
                    }


                    objProspect.AgeNextBdy = age;
                    objProspect.EmployerName = tblcontact.Employer;
                    // Prospect DOB for Need Analysis
                    objProspect.objNeedAnalysis.ProspectDOB = objProspect.DateofBirth;
                    objProspect.IntroducerCode = tblcontact.IntroducerCode;
                    //objProspect.Occupation = Context.tblMasLifeOccupations.Where(a => a.CompanyCode == tblcontact.OccupationID.ToString()).Select(a => a.OccupationCode).FirstOrDefault();
                    objProspect.Occupation = Context.tblMasLifeOccupations.Where(a => a.CompanyCode == tblcontact.OccupationID.ToString()).Select(a => a.OccupationCode + "|" + a.SinhalaDesc + "|" + a.TamilDesc).FirstOrDefault();
                    objProspect.NIC = tblcontact.NICNO;
                    int MaritalStatusID = Convert.ToInt32(tblcontact.MaritalStatusID);
                    objProspect.MaritalStatus = Context.tblMasCommonTypes.Where(a => a.CommonTypesID == tblcontact.MaritalStatusID && a.MasterType == "MaritalStatus").Select(a => a.Code).FirstOrDefault();

                    objProspect.objNeedAnalysis.objSpouseDetails.FullName = tblcontact.SpouseName;
                    objProspect.objNeedAnalysis.objSpouseDetails.AgeNextBirthday = Convert.ToInt32(tblcontact.SpouseAge);
                    objProspect.objNeedAnalysis.objSpouseDetails.CurrentAge = Convert.ToInt32(tblcontact.CurrentAge);
                    objProspect.objNeedAnalysis.objSpouseDetails.DateOfBirth = tblcontact.SpouseDOB;
                    objProspect.objNeedAnalysis.DependantCount = tblcontact.DependenceCount;
                    var objDependants = Context.tblDependants.Where(a => a.ContactID == tblcontact.ContactID).ToList();
                    foreach (var item in objDependants)
                    {
                        Dependants obj = new Dependants();
                        obj.Name = item.DependantName;
                        obj.AgeNextBirthday = Convert.ToInt32(item.DependantAge);
                        obj.DOB = item.DependantDOB;
                        obj.Relationship = item.DependantRelation;
                        objProspect.objNeedAnalysis.objDependants.Add(obj);

                    }
                    //objProspect.Gender = GetGenderCode(Convert.ToInt32(tblcontact.Gender));
                    objProspect.Gender = tblcontact.Gender;
                    objProspect.AvgMonthlyIncome = tblcontact.MonthlyIncome;
                    objProspect.Salutation = Context.tblMasCommonTypes.Where(a => a.Code == tblcontact.Title).Select(b => b.Description).FirstOrDefault();
                    objProspect.PassPort = tblcontact.PassportNo;
                    //objProspect.Gender = tblcontact.Gender;
                    //objProspect.AvgMonthlyIncome = tblcontact.MonthlyIncome;
                    objProspect.Place = tblcontact.Place;
                    objProspect.SamsLeadNumber = tblcontact.LeadNo;
                    if (objProspect.objAddress == null)
                    {
                        objProspect.objAddress = new AIA.Life.Models.Common.Address();
                    }
                    if (tblcontact.tblAddress != null)
                    {
                        if (objProspect.objAddress == null)
                        {
                            objProspect.objAddress = new AIA.Life.Models.Common.Address();
                        }
                        objProspect.objAddress.Address1 = tblcontact.tblAddress.Address1;
                        objProspect.objAddress.Address2 = tblcontact.tblAddress.Address2;
                        objProspect.objAddress.Address3 = tblcontact.tblAddress.Address3;
                        objProspect.objAddress.Pincode = tblcontact.tblAddress.Pincode + "|" + tblcontact.tblAddress.City;
                        objProspect.objAddress.City = tblcontact.tblAddress.City;
                        objProspect.objAddress.State = tblcontact.tblAddress.State;
                        objProspect.objAddress.District = tblcontact.tblAddress.District;
                        objProspect.objAddress.Country = tblcontact.tblAddress.Country;
                    }

                    //#region Fetch Dependents
                    //foreach (var DependentContact in Context.tblContacts.Where(a => a.ParentContactID == objProspect.ContactID))
                    //{
                    //    if (DependentContact.Relationship == "268")
                    //    {
                    //        objProspect.objNeedAnalysis.objSpouseDetails.FullName = DependentContact.FirstName;
                    //        objProspect.objNeedAnalysis.objSpouseDetails.DateOfBirth = DependentContact.DateOfBirth;
                    //        objProspect.objNeedAnalysis.objSpouseDetails.Age = Convert.ToInt32(DependentContact.Age);
                    //        var SpouseMaritalStatus = (from r in Context.tblMasCommonTypes
                    //                                   where r.CommonTypesID == MaritalStatusID
                    //                                   select r.ShortDesc).FirstOrDefault();
                    //        objProspect.objNeedAnalysis.objSpouseDetails.MaritialStatus = SpouseMaritalStatus;
                    //        objProspect.objNeedAnalysis.objSpouseDetails.OccuaptionID = Convert.ToString(DependentContact.OccupationID);
                    //        objProspect.objNeedAnalysis.objSpouseDetails.ContactID = DependentContact.ContactID;
                    //        objProspect.objNeedAnalysis.objSpouseDetails.Employer = DependentContact.Employer;
                    //    }
                    //    else
                    //    {
                    //        Dependents objDependent = new Dependents();
                    //        objDependent.DependentName = DependentContact.FirstName;
                    //        objDependent.Relationship = DependentContact.Relationship;
                    //        objDependent.Age = Convert.ToInt32(DependentContact.Age);
                    //        objDependent.DateOfBirth = DependentContact.DateOfBirth;
                    //        objDependent.ContactID = DependentContact.ContactID;
                    //        objProspect.objNeedAnalysis.objDependents.Add(objDependent);
                    //    }
                    //}
                    //#endregion

                    var tblopportunity = Context.tblOpportunities.Where(a => a.ContactID == objProspect.ContactID).FirstOrDefault();
                    if (tblopportunity != null)
                    {
                        objProspect.objNeedAnalysis.objNeeds = new List<Needs>();
                        objProspect.ProspectStage = tblopportunity.StageID;
                        var objtblLifeNeedAnalysis = Context.tblLifeNeedAnalysis.Where(a => a.NeedAnalysisID == tblopportunity.NeedAnalysisID).FirstOrDefault();
                        var objtblMasNeeds = (from e in Context.tblMasNeeds
                                              select e).ToList();

                        if (objtblLifeNeedAnalysis != null)
                        {
                            objProspect.objNeedAnalysis.NeedAnalysisID = objtblLifeNeedAnalysis.NeedAnalysisID;

                            #region Estimates
                            objProspect.objNeedAnalysis.objEstimateDetails = new EstimateDetails();
                            objProspect.objNeedAnalysis.objEstimateDetails.Food = objtblLifeNeedAnalysis.Food;
                            objProspect.objNeedAnalysis.objEstimateDetails.HouseElectricityWaterRent = objtblLifeNeedAnalysis.House_Elec_Water_Phone;
                            objProspect.objNeedAnalysis.objEstimateDetails.Clothes = objtblLifeNeedAnalysis.Clothes;
                            objProspect.objNeedAnalysis.objEstimateDetails.Transport = objtblLifeNeedAnalysis.Transport;
                            objProspect.objNeedAnalysis.objEstimateDetails.HealthCare = objtblLifeNeedAnalysis.Family_HealthCare;
                            objProspect.objNeedAnalysis.objEstimateDetails.FamilyEducation = objtblLifeNeedAnalysis.Edu_of_Child;
                            objProspect.objNeedAnalysis.objEstimateDetails.SpecialEvents = objtblLifeNeedAnalysis.SpecialEvents;
                            objProspect.objNeedAnalysis.objEstimateDetails.MaidAndOtherHelpers = objtblLifeNeedAnalysis.MaidAndOthers;
                            objProspect.objNeedAnalysis.objEstimateDetails.OtherMontly = objtblLifeNeedAnalysis.Other_Monthly;
                            objProspect.objNeedAnalysis.objEstimateDetails.TotalMonthlyExp = objtblLifeNeedAnalysis.Total_Monthly;
                            objProspect.objNeedAnalysis.objEstimateDetails.MonthlyInstallments = objtblLifeNeedAnalysis.Monthly_Installments;

                            #endregion

                            #region Assests And Liabilities
                            objProspect.objNeedAnalysis.objAssetsAndLiabilities = new AssetsAndLiabilities();
                            objProspect.objNeedAnalysis.objAssetsAndLiabilities.LandOrHouse = objtblLifeNeedAnalysis.Land_Assets;
                            objProspect.objNeedAnalysis.objAssetsAndLiabilities.MotorVehicle = objtblLifeNeedAnalysis.Motor_Assets;

                            //int MotorVehicleTypeID = Convert.ToInt32(objtblLifeNeedAnalysis.MotorVehicleType);
                            //var MotorVehicleType = (from r in Context.tblMasCommonTypes
                            //                        where r.CommonTypesID == MotorVehicleTypeID
                            //                        select r.Description).FirstOrDefault();

                            objProspect.objNeedAnalysis.objAssetsAndLiabilities.MotorVehicleType = objtblLifeNeedAnalysis.MotorVehicleType;
                            objProspect.objNeedAnalysis.objAssetsAndLiabilities.BankDeposits = objtblLifeNeedAnalysis.Bank_Assets;
                            if (!string.IsNullOrEmpty(objtblLifeNeedAnalysis.Assets_investments))
                            {
                                objProspect.objNeedAnalysis.objAssetsAndLiabilities.Investments = Convert.ToDecimal(objtblLifeNeedAnalysis.Assets_investments);
                            }
                            objProspect.objNeedAnalysis.objAssetsAndLiabilities.AssetsTotal = Convert.ToInt32(objtblLifeNeedAnalysis.Total_Assets);
                            objProspect.objNeedAnalysis.objAssetsAndLiabilities.Loans = objtblLifeNeedAnalysis.Loan_LB;
                            objProspect.objNeedAnalysis.objAssetsAndLiabilities.Mortgauges = objtblLifeNeedAnalysis.Mortgages_LB;
                            objProspect.objNeedAnalysis.objAssetsAndLiabilities.leases = objtblLifeNeedAnalysis.Leases_LB;
                            objProspect.objNeedAnalysis.objAssetsAndLiabilities.others = objtblLifeNeedAnalysis.Others_LB;
                            objProspect.objNeedAnalysis.objAssetsAndLiabilities.Liab_Total = objtblLifeNeedAnalysis.Total_LB;

                            #endregion

                            #region FamilyIncome

                            objProspect.objNeedAnalysis.objFamilyIncome = new FamilyIncome();
                            objProspect.objNeedAnalysis.objFamilyIncome.ProspectMonthlyIncome = objtblLifeNeedAnalysis.Prospect_Monthly;
                            objProspect.objNeedAnalysis.objFamilyIncome.SpouseMonthlyIncome = objtblLifeNeedAnalysis.Spouse_Monthly;
                            objProspect.objNeedAnalysis.objFamilyIncome.HouseHoldTotal = objtblLifeNeedAnalysis.HouseHold_Monthly;
                            objProspect.objNeedAnalysis.objFamilyIncome.CapitalReq = objtblLifeNeedAnalysis.Capital_Monthly_Exp_1;
                            objProspect.objNeedAnalysis.objFamilyIncome.PersonalLifeInsurance = objtblLifeNeedAnalysis.Life_Insurance;
                            objProspect.objNeedAnalysis.objFamilyIncome.SavingsAndInvestments = objtblLifeNeedAnalysis.OtherSavings;
                            objProspect.objNeedAnalysis.objFamilyIncome.TotalProtection = objtblLifeNeedAnalysis.Capital_Monthly_Exp_2;
                            objProspect.objNeedAnalysis.objFamilyIncome.GapIdentified = objtblLifeNeedAnalysis.Gap_Identified;
                            objProspect.objNeedAnalysis.objFamilyIncome.RateOfInterest = Convert.ToInt32(ConfigurationManager.AppSettings["RateOfInterest"]);

                            objProspect.objNeedAnalysis.objFamilyIncome.objLstPrevInsuranceDetails = Context.tblPreviousInsurenceInfoes.
                           Where(a => a.NeedAnalysisID == tblopportunity.NeedAnalysisID).Select(x => new PrevInsuranceDetails
                           {
                               Company = x.CompanyName,
                               PolicyOrProposalNo = x.Policy_ProposalNo,
                               Death = x.TotalSIAtDeath,
                               Accidental = x.AccidentalBenifit,
                               Critical = x.CriticalIllnessBenifit,
                               Hospitalization = x.HospitalizationPerDay,
                               CurrentStatus = x.CurrentStatus,

                           }).ToList();

                            if (objProspect.objNeedAnalysis.objFamilyIncome.objLstPrevInsuranceDetails.Count > 0)
                            {
                                for (int i = 0; i < objProspect.objNeedAnalysis.objFamilyIncome.objLstPrevInsuranceDetails.Count; i++)
                                {

                                    int CurrentStatusID = Convert.ToInt32(objProspect.objNeedAnalysis.objFamilyIncome.objLstPrevInsuranceDetails[i].CurrentStatus);
                                    var SelectedCurrentStatus = (from r in Context.tblMasCommonTypes
                                                                 where r.CommonTypesID == CurrentStatusID
                                                                 select r.Description).FirstOrDefault();
                                    objProspect.objNeedAnalysis.objFamilyIncome.objLstPrevInsuranceDetails[i].CurrentStatus = SelectedCurrentStatus;
                                }
                            }

                            objProspect.objNeedAnalysis.objFamilyIncome.IsOtherInsurance = objtblLifeNeedAnalysis.IsCoveredUnderOtherPolicy;
                            objProspect.objNeedAnalysis.objFamilyIncome.NoOfJanashaktiPolicy = objtblLifeNeedAnalysis.NoOfJanashaktiPolicies;
                            objProspect.objNeedAnalysis.objFamilyIncome.NoOfOtherPolicies = objtblLifeNeedAnalysis.NoOfOtherPolicies;


                            objProspect.objNeedAnalysis.objFamilyIncome.TotalDeath = objProspect.objNeedAnalysis.objFamilyIncome.objLstPrevInsuranceDetails.Sum(x => Convert.ToInt32(x.Death));
                            objProspect.objNeedAnalysis.objFamilyIncome.TotalAccidental = objProspect.objNeedAnalysis.objFamilyIncome.objLstPrevInsuranceDetails.Sum(x => Convert.ToInt32(x.Accidental));
                            objProspect.objNeedAnalysis.objFamilyIncome.TotalCritical = objProspect.objNeedAnalysis.objFamilyIncome.objLstPrevInsuranceDetails.Sum(x => Convert.ToInt32(x.Critical));
                            objProspect.objNeedAnalysis.objFamilyIncome.TotalHospitalization = objProspect.objNeedAnalysis.objFamilyIncome.objLstPrevInsuranceDetails.Sum(x => Convert.ToInt32(x.Hospitalization));

                            #endregion

                            #region Fetch Needs
                            GetNeedsData(objProspect);
                            for (int i = 0; i < objProspect.objNeedAnalysis.objNeeds.Count(); i++)
                            {
                                var DBneed = objtblLifeNeedAnalysis.tblNeeds.Where(a => a.IdentifiedNeed == objProspect.objNeedAnalysis.objNeeds[i].NeedID).FirstOrDefault();

                                if (DBneed != null)
                                {
                                    objProspect.objNeedAnalysis.objNeeds[i].Priority = DBneed.Priority;
                                    objProspect.objNeedAnalysis.objNeeds[i].PKNeedID = DBneed.NeedID;
                                    objProspect.objNeedAnalysis.objNeeds[i].IsNeedOpted = true;
                                    objProspect.objNeedAnalysis.objNeeds[i].Value = DBneed.Value;

                                }
                            }
                            //foreach (var item in objtblLifeNeedAnalysis.tblNeeds)
                            //{
                            //    Needs objNeeds = new Needs();
                            //    objNeeds.PKNeedID = item.NeedID;
                            //    objNeeds.NeedID = item.IdentifiedNeed;                        
                            //    objNeeds.Priority = item.Priority;             
                            //    objNeeds.NeedName = item.NeedName;                   
                            //    objNeeds.IsNeedOpted = true;                     
                            //    objNeeds.Value = item.Value;
                            //    objNeeds.PlanSuggested = item.PlanSuggested;
                            //    objProspect.objNeedAnalysis.objNeeds.Add(objNeeds);
                            //}

                            //if (objProspect.objNeedAnalysis.objNeeds.Count() < 1)
                            //{
                            //    GetNeedsData(objProspect);
                            //}
                            objProspect.objNeedAnalysis.Total = Convert.ToString(objtblLifeNeedAnalysis.TotalNeedValue);
                            objProspect.objNeedAnalysis.ProductSelected = objtblLifeNeedAnalysis.ProductSelected;
                            #endregion

                            #region Fetch AdditionalDetails

                            objProspect.objNeedAnalysis.DateOfNextMeeting = objtblLifeNeedAnalysis.Date_NextMeeting;
                            objProspect.objNeedAnalysis.TimeOfNextMeeting = Convert.ToString(objtblLifeNeedAnalysis.Time_NextMeeting);
                            objProspect.objNeedAnalysis.PurposeOfMeeting = objtblLifeNeedAnalysis.PurposeOfMeeting;
                            objProspect.objNeedAnalysis.UploadSignPath = objtblLifeNeedAnalysis.UploadSignPath;
                            objProspect.objNeedAnalysis.NotePadPath = objtblLifeNeedAnalysis.NotePadPath;
                            objProspect.objNeedAnalysis.ProspectSign = objtblLifeNeedAnalysis.ProspectSign;
                            objProspect.objNeedAnalysis.CreatedDate = objtblLifeNeedAnalysis.CreatedDate;
                            objProspect.objNeedAnalysis.SelectedProducts = objtblLifeNeedAnalysis.ProductsSelected;
                            #endregion

                            objProspect.objNeedAnalysis.chkEducation1 = objtblLifeNeedAnalysis.chkEdu1 ?? false;
                            objProspect.objNeedAnalysis.chkEducation2 = objtblLifeNeedAnalysis.chkEdu2 ?? false;
                            objProspect.objNeedAnalysis.chkEducation3 = objtblLifeNeedAnalysis.chkEdu3 ?? false;
                            objProspect.objNeedAnalysis.chkEducation4 = objtblLifeNeedAnalysis.chkEdu4 ?? false;
                            objProspect.objNeedAnalysis.chkEducation5 = objtblLifeNeedAnalysis.chkEdu5 ?? false;
                            objProspect.objNeedAnalysis.chkHealth1 = objtblLifeNeedAnalysis.chkHealth1 ?? false;
                            objProspect.objNeedAnalysis.chkHealth2 = objtblLifeNeedAnalysis.chkHealth2 ?? false;
                            objProspect.objNeedAnalysis.chkHealth3 = objtblLifeNeedAnalysis.chkHealth3 ?? false;
                            objProspect.objNeedAnalysis.chkHealth4 = objtblLifeNeedAnalysis.chkHealth4 ?? false;
                            objProspect.objNeedAnalysis.chkHealth5 = objtblLifeNeedAnalysis.chkHealth5 ?? false;
                            objProspect.objNeedAnalysis.chkProtection1 = objtblLifeNeedAnalysis.chkProtection1 ?? false;
                            objProspect.objNeedAnalysis.chkProtection2 = objtblLifeNeedAnalysis.chkProtection2 ?? false;
                            objProspect.objNeedAnalysis.chkProtection3 = objtblLifeNeedAnalysis.chkProtection3 ?? false;
                            objProspect.objNeedAnalysis.chkProtection4 = objtblLifeNeedAnalysis.chkProtection4 ?? false;
                            objProspect.objNeedAnalysis.chkProtection5 = objtblLifeNeedAnalysis.chkProtection5 ?? false;
                            objProspect.objNeedAnalysis.chkRetirement1 = objtblLifeNeedAnalysis.chkRetire1 ?? false;
                            objProspect.objNeedAnalysis.chkRetirement2 = objtblLifeNeedAnalysis.chkRetire2 ?? false;
                            objProspect.objNeedAnalysis.chkRetirement3 = objtblLifeNeedAnalysis.chkRetire3 ?? false;
                            objProspect.objNeedAnalysis.chkRetirement4 = objtblLifeNeedAnalysis.chkRetire4 ?? false;
                            objProspect.objNeedAnalysis.chkRetirement5 = objtblLifeNeedAnalysis.chkRetire5 ?? false;
                            objProspect.objNeedAnalysis.chkSaving1 = objtblLifeNeedAnalysis.chkSaving1 ?? false;
                            objProspect.objNeedAnalysis.chkSaving2 = objtblLifeNeedAnalysis.chkSaving2 ?? false;
                            objProspect.objNeedAnalysis.chkSaving3 = objtblLifeNeedAnalysis.chkSaving3 ?? false;
                            objProspect.objNeedAnalysis.chkSaving4 = objtblLifeNeedAnalysis.chkSaving4 ?? false;
                            objProspect.objNeedAnalysis.chkSaving5 = objtblLifeNeedAnalysis.chkSaving5 ?? false;
                            objProspect.objNeedAnalysis.chkconfirm = objtblLifeNeedAnalysis.chkconfirm ?? false;
                            objProspect.objNeedAnalysis.chkprodconfirm = objtblLifeNeedAnalysis.chkprodconfirm ?? false;
                            objProspect.objNeedAnalysis.FNAFromYear = Convert.ToInt32(objtblLifeNeedAnalysis.FromYear);
                            objProspect.objNeedAnalysis.FNAToYear = Convert.ToInt32(objtblLifeNeedAnalysis.ToYear);
                            objProspect.objNeedAnalysis.FNAPlanNoYear = Convert.ToInt32(objtblLifeNeedAnalysis.PlanNoOfYears);
                            objProspect.objNeedAnalysis.FNAIntrestRate = Convert.ToInt32(objtblLifeNeedAnalysis.RateOfInterest);
                            objProspect.objNeedAnalysis.FNAInflationRate = Convert.ToInt32(objtblLifeNeedAnalysis.InflationRate);
                            objProspect.objNeedAnalysis.Assets0 = Convert.ToInt64(objtblLifeNeedAnalysis.Land_Assets);
                            objProspect.objNeedAnalysis.Assets1 = Convert.ToInt64(objtblLifeNeedAnalysis.FixedDeposit);
                            objProspect.objNeedAnalysis.Assets2 = Convert.ToInt64(objtblLifeNeedAnalysis.Shares);
                            objProspect.objNeedAnalysis.Assets3 = Convert.ToInt64(objtblLifeNeedAnalysis.Vehicle);
                            objProspect.objNeedAnalysis.Assets4 = Convert.ToInt64(objtblLifeNeedAnalysis.Jewellery);
                            objProspect.objNeedAnalysis.Assets5 = Convert.ToInt64(objtblLifeNeedAnalysis.OtherAssets);
                            objProspect.objNeedAnalysis.objAssetsAndLiabilities.AssetsTotal = Convert.ToInt64(objtblLifeNeedAnalysis.TotalAssets);
                            objProspect.objNeedAnalysis.Liabilityone0 = Convert.ToInt64(objtblLifeNeedAnalysis.Loan);
                            objProspect.objNeedAnalysis.Liabilitytwo0 = Convert.ToInt64(objtblLifeNeedAnalysis.InsuredLoan);
                            objProspect.objNeedAnalysis.Liabilityone1 = Convert.ToInt64(objtblLifeNeedAnalysis.CreditCard);
                            objProspect.objNeedAnalysis.Liabilitytwo1 = Convert.ToInt64(objtblLifeNeedAnalysis.InsuredCreditCard);
                            objProspect.objNeedAnalysis.Liabilityone2 = Convert.ToInt64(objtblLifeNeedAnalysis.Lease);
                            objProspect.objNeedAnalysis.Liabilitytwo2 = Convert.ToInt64(objtblLifeNeedAnalysis.InsuredLease);
                            objProspect.objNeedAnalysis.Liabilityone3 = Convert.ToInt64(objtblLifeNeedAnalysis.OtherLiability);
                            objProspect.objNeedAnalysis.Liabilitytwo3 = Convert.ToInt64(objtblLifeNeedAnalysis.InsuredOtherLiability);
                            objProspect.objNeedAnalysis.objAssetsAndLiabilities.LiabilityTotal = Convert.ToInt64(objtblLifeNeedAnalysis.TotalLiability);
                            objProspect.objNeedAnalysis.objAssetsAndLiabilities.InsuredLiabilityTotal = Convert.ToInt64(objtblLifeNeedAnalysis.InsuredTotalLiability);
                            objProspect.objNeedAnalysis.objAssetsAndLiabilities.NetAssests = Convert.ToInt64(objtblLifeNeedAnalysis.NetAssets);
                            objProspect.objNeedAnalysis.objAssetsAndLiabilities.LumpsumRequirement = Convert.ToInt64(objtblLifeNeedAnalysis.LumpSumReq);
                            objProspect.objNeedAnalysis.Income0 = Convert.ToInt64(objtblLifeNeedAnalysis.Salary);
                            objProspect.objNeedAnalysis.Income1 = Convert.ToInt64(objtblLifeNeedAnalysis.Intrest);
                            objProspect.objNeedAnalysis.Income2 = Convert.ToInt64(objtblLifeNeedAnalysis.Rent);
                            objProspect.objNeedAnalysis.Income3 = Convert.ToInt64(objtblLifeNeedAnalysis.OtherIncome);
                            objProspect.objNeedAnalysis.objFamilyIncome.TotalIncome = Convert.ToInt64(objtblLifeNeedAnalysis.TotalIncome);
                            objProspect.objNeedAnalysis.Expense0 = Convert.ToInt64(objtblLifeNeedAnalysis.AnnualExp);
                            objProspect.objNeedAnalysis.Expense1 = Convert.ToInt64(objtblLifeNeedAnalysis.AnnualVacation);
                            objProspect.objNeedAnalysis.Expense2 = Convert.ToInt64(objtblLifeNeedAnalysis.Installment);
                            objProspect.objNeedAnalysis.Expense3 = Convert.ToInt64(objtblLifeNeedAnalysis.VehExp);
                            objProspect.objNeedAnalysis.Expense4 = Convert.ToInt64(objtblLifeNeedAnalysis.LoanExp);
                            objProspect.objNeedAnalysis.Expense5 = Convert.ToInt64(objtblLifeNeedAnalysis.OtherExp);
                            objProspect.objNeedAnalysis.objFamilyIncome.TotalExpense = Convert.ToInt64(objtblLifeNeedAnalysis.TotalExp);
                            objProspect.objNeedAnalysis.objFamilyIncome.IncomeLumpsumRequirement = Convert.ToInt64(objtblLifeNeedAnalysis.LumpSumReqExp);
                            objProspect.objNeedAnalysis.objAssetsAndLiabilities.SurPlusAssets = Convert.ToInt64(objtblLifeNeedAnalysis.SurplusExp);
                            objProspect.objNeedAnalysis.CriticalIllnessRequirement = Convert.ToInt64(objtblLifeNeedAnalysis.CriticalIllnessReq);
                            objProspect.objNeedAnalysis.CriticalIllnessAvailable = Convert.ToInt64(objtblLifeNeedAnalysis.CriticalIllnessFund);
                            objProspect.objNeedAnalysis.CriticalIllnessGap = Convert.ToInt64(objtblLifeNeedAnalysis.CriticalIllnessGap);
                            objProspect.objNeedAnalysis.HospitalRequirement = Convert.ToInt64(objtblLifeNeedAnalysis.HospitalizationReq);
                            objProspect.objNeedAnalysis.HospitalAvailable = Convert.ToInt64(objtblLifeNeedAnalysis.HospitalizationFund);
                            objProspect.objNeedAnalysis.HospitalGap = Convert.ToInt64(objtblLifeNeedAnalysis.HospitalizationGap);
                            objProspect.objNeedAnalysis.TotalRequirement = Convert.ToInt64(objtblLifeNeedAnalysis.TotalReq);
                            objProspect.objNeedAnalysis.TotalAvailable = Convert.ToInt64(objtblLifeNeedAnalysis.TotalFund);
                            objProspect.objNeedAnalysis.TotalGap = Convert.ToInt64(objtblLifeNeedAnalysis.TotalGap);
                            objProspect.objNeedAnalysis.AdditionalRequirement = Convert.ToInt64(objtblLifeNeedAnalysis.AddExpReq);
                            objProspect.objNeedAnalysis.AdditionalAvailable = Convert.ToInt64(objtblLifeNeedAnalysis.AddExpFund);
                            objProspect.objNeedAnalysis.AdditionalGap = Convert.ToInt64(objtblLifeNeedAnalysis.AddExpGap);
                            objProspect.objNeedAnalysis.WealthRequirement = Convert.ToInt64(objtblLifeNeedAnalysis.WealthReq);
                            objProspect.objNeedAnalysis.LivingExpense = Convert.ToInt64(objtblLifeNeedAnalysis.IncomeReq);
                            objProspect.objNeedAnalysis.FinancialExpense = Convert.ToInt64(objtblLifeNeedAnalysis.DreamReq);
                            objProspect.objNeedAnalysis.FinancialExpense2017 = Convert.ToInt64(objtblLifeNeedAnalysis.MaturityDreamReq);
                            objProspect.objNeedAnalysis.TotalExpense = Convert.ToInt64(objtblLifeNeedAnalysis.TotalReq1);
                            objProspect.objNeedAnalysis.TotalExpense2017 = Convert.ToInt64(objtblLifeNeedAnalysis.MaturityTotalReq1);
                            objProspect.objNeedAnalysis.EmergencyFund1 = Convert.ToInt64(objtblLifeNeedAnalysis.EmergencyPolicy1);
                            objProspect.objNeedAnalysis.EmergencyFund2 = Convert.ToInt64(objtblLifeNeedAnalysis.EmergencyPolicy2);
                            objProspect.objNeedAnalysis.EmergencyFund3 = Convert.ToInt64(objtblLifeNeedAnalysis.EmergencyPolicy3);
                            objProspect.objNeedAnalysis.TotalEmergencyFund = Convert.ToInt64(objtblLifeNeedAnalysis.EmergencyTotal2);
                            objProspect.objNeedAnalysis.MaturityFund1 = Convert.ToInt64(objtblLifeNeedAnalysis.MaturityPolicy1);
                            objProspect.objNeedAnalysis.MaturityFund2 = Convert.ToInt64(objtblLifeNeedAnalysis.MaturityPolicy2);
                            objProspect.objNeedAnalysis.MaturityFund3 = Convert.ToInt64(objtblLifeNeedAnalysis.MaturityPolicy3);
                            objProspect.objNeedAnalysis.TotalMaturityFund = Convert.ToInt64(objtblLifeNeedAnalysis.MaturityTotal2);
                            objProspect.objNeedAnalysis.EmergencyFundGap = Convert.ToInt64(objtblLifeNeedAnalysis.Gap1);
                            objProspect.objNeedAnalysis.MaturityFundGap = Convert.ToInt64(objtblLifeNeedAnalysis.Gap2);
                            // objProspect.objNeedAnalysis.DependantCount = objtblLifeNeedAnalysis.NoOfOtherPolicies;
                            objProspect.objNeedAnalysis.ProspectSign = objtblLifeNeedAnalysis.ProspectSign;
                            var objNeedFinancialNeed = Context.tblNeedFinancialNeeds.Where(a => a.NeedAnalysisID == tblopportunity.NeedAnalysisID).ToList();

                            if (objNeedFinancialNeed != null)
                            {
                                List<FinancialNeeds> objFinancialNeed = new List<FinancialNeeds>();
                                foreach (var item in objNeedFinancialNeed)
                                {
                                    FinancialNeeds obj = new FinancialNeeds();
                                    obj.CurrReq = Convert.ToInt64(item.CurrentReq);
                                    obj.EstAmount = Convert.ToInt64(item.EstimatedAmount);
                                    obj.FundBalance = Convert.ToInt64(item.FundBalance);
                                    obj.Gap = Convert.ToInt64(item.Gap);
                                    obj.Relationship = item.Relationship;
                                    obj.Name = item.Name;
                                    objProspect.objNeedAnalysis.objFinancialNeeds.Add(obj);

                                }

                            }
                            objProspect.objNeedAnalysis.objFinancialNeed.RequirementTotal = Convert.ToInt64(objtblLifeNeedAnalysis.FinancialCurrReqTotal);
                            objProspect.objNeedAnalysis.objFinancialNeed.EstimateTotal = Convert.ToInt64(objtblLifeNeedAnalysis.FinancialEstAmount);
                            objProspect.objNeedAnalysis.objFinancialNeed.FundBalanceTotal = Convert.ToInt64(objtblLifeNeedAnalysis.FinancialFund);
                            objProspect.objNeedAnalysis.objFinancialNeed.GapTotal = Convert.ToInt64(objtblLifeNeedAnalysis.FinancialGap);
                            var objPrevPolicy = Context.tblPrevPolicies.Where(a => a.NeedAnalysisID == tblopportunity.NeedAnalysisID).ToList();
                            foreach (tblPrevPolicy item in objPrevPolicy)
                            {
                                PrevPolicy obj = new PrevPolicy();
                                obj.PolicyNo = item.PolicyNumber;
                                obj.MaturityFund = Convert.ToInt64(item.MaturityFund);
                                objProspect.objNeedAnalysis.objPrevPolicy.Add(obj);
                            }
                            var objNeedRetireCal = Context.tblNeedRetirementCalculators.Where(a => a.NeedAnalysisID == tblopportunity.NeedAnalysisID).FirstOrDefault();
                            if (objNeedRetireCal != null)
                            {
                                objProspect.objNeedAnalysis.CalculatorFromYear = objNeedRetireCal.FromYear;
                                objProspect.objNeedAnalysis.CalculatorToYear = objNeedRetireCal.ToYear;
                                objProspect.objNeedAnalysis.CalculatorInflationRate = objNeedRetireCal.InflationRate;
                                objProspect.objNeedAnalysis.CalculatorIntrestRate = objNeedRetireCal.IntrestRate;
                                objProspect.objNeedAnalysis.CalculatorPlanNoYears = objNeedRetireCal.PlanNoYears;
                                objProspect.objNeedAnalysis.objCalculator.TotalMonthlyExpense = Convert.ToInt64(objNeedRetireCal.TotalMonthlyExp);
                                objProspect.objNeedAnalysis.objCalculator.EstimatedTotalMonthlyExpense = Convert.ToInt64(objNeedRetireCal.EstMonthlyExp);
                                objProspect.objNeedAnalysis.objCalculator.FoodExpense = Convert.ToInt64(objNeedRetireCal.CurrentFoodExp);
                                objProspect.objNeedAnalysis.objCalculator.EstimatedFoodExpense = Convert.ToInt64(objNeedRetireCal.EstFoodExp);
                                objProspect.objNeedAnalysis.objCalculator.WaterExpense = Convert.ToInt64(objNeedRetireCal.CurrentWaterExp);
                                objProspect.objNeedAnalysis.objCalculator.EstimatedWaterExpense = Convert.ToInt64(objNeedRetireCal.EstWaterExp);
                                objProspect.objNeedAnalysis.objCalculator.RentExpense = Convert.ToInt64(objNeedRetireCal.CurrentRentExp);
                                objProspect.objNeedAnalysis.objCalculator.EstimatedRentExpense = Convert.ToInt64(objNeedRetireCal.EstRentExp);
                                objProspect.objNeedAnalysis.objCalculator.LeaseExpense = Convert.ToInt64(objNeedRetireCal.CurrentLeaseExp);
                                objProspect.objNeedAnalysis.objCalculator.EstimatedLeaseExpense = Convert.ToInt64(objNeedRetireCal.EstLeaseExp);
                                objProspect.objNeedAnalysis.objCalculator.TransportExpense = Convert.ToInt64(objNeedRetireCal.CurrentTransportExp);
                                objProspect.objNeedAnalysis.objCalculator.EstimatedTransportExpense = Convert.ToInt64(objNeedRetireCal.EstTransportExp);
                                objProspect.objNeedAnalysis.objCalculator.MedicineExpense = Convert.ToInt64(objNeedRetireCal.CurrentMedExp);
                                objProspect.objNeedAnalysis.objCalculator.EstimatedMedicineExpense = Convert.ToInt64(objNeedRetireCal.EstMedExp);
                                objProspect.objNeedAnalysis.objCalculator.EducationExpense = Convert.ToInt64(objNeedRetireCal.CurrentEduExp);
                                objProspect.objNeedAnalysis.objCalculator.EstimatedEducationExpense = Convert.ToInt64(objNeedRetireCal.EstEduExp);
                                objProspect.objNeedAnalysis.objCalculator.ClothesExpense = Convert.ToInt64(objNeedRetireCal.CurrentClothesExp);
                                objProspect.objNeedAnalysis.objCalculator.EstimatedClothesExpense = Convert.ToInt64(objNeedRetireCal.EstClothesExp);
                                objProspect.objNeedAnalysis.objCalculator.EntertainmentExpense = Convert.ToInt64(objNeedRetireCal.CurrentEntertainmentExp);
                                objProspect.objNeedAnalysis.objCalculator.EstimatedEntertainmentExpense = Convert.ToInt64(objNeedRetireCal.EstEntertainmentExp);
                                objProspect.objNeedAnalysis.objCalculator.CharityExpense = Convert.ToInt64(objNeedRetireCal.CurrentCharity);
                                objProspect.objNeedAnalysis.objCalculator.EstimatedCharityExpense = Convert.ToInt64(objNeedRetireCal.EstCharity);
                                objProspect.objNeedAnalysis.objCalculator.OtherExpense = Convert.ToInt64(objNeedRetireCal.CurrentOtherExp);
                                objProspect.objNeedAnalysis.objCalculator.EstimatedOtherExpense = Convert.ToInt64(objNeedRetireCal.EstOtherExp);
                                objProspect.objNeedAnalysis.objCalculator.Salary = Convert.ToInt64(objNeedRetireCal.CurrentMonthlySalary);
                                objProspect.objNeedAnalysis.objCalculator.CurrentEPFBalance = Convert.ToInt64(objNeedRetireCal.CurrentEPFBalance);
                                objProspect.objNeedAnalysis.objCalculator.EstimatedEPFBalance = Convert.ToInt64(objNeedRetireCal.EstEPFBalance);
                                objProspect.objNeedAnalysis.objCalculator.MonthlyAllocation20 = Convert.ToInt64(objNeedRetireCal.CurrentMonthly20Sal);
                                objProspect.objNeedAnalysis.objCalculator.CurrentETFBalance = Convert.ToInt64(objNeedRetireCal.CurrentETFBalance);
                                objProspect.objNeedAnalysis.objCalculator.EstimatedETFBalance = Convert.ToInt64(objNeedRetireCal.EstETFBalance);
                                objProspect.objNeedAnalysis.objCalculator.MonthlyAllocation3 = Convert.ToInt64(objNeedRetireCal.CurrentMonthly3Sal);
                                objProspect.objNeedAnalysis.objCalculator.CurrentGratuityFund = Convert.ToInt64(objNeedRetireCal.CurrentGratuityFund);
                                objProspect.objNeedAnalysis.objCalculator.EstimatedGratuityFund = Convert.ToInt64(objNeedRetireCal.EstGratuityFund);
                                objProspect.objNeedAnalysis.objCalculator.TotalRetirementFund = Convert.ToInt64(objNeedRetireCal.TotalEstMonthlyExpFund);
                                objProspect.objNeedAnalysis.Financial0 = Convert.ToInt64(objNeedRetireCal.ChildEduFund);
                                objProspect.objNeedAnalysis.Financial1 = Convert.ToInt64(objNeedRetireCal.ChildWeddingFund);
                                objProspect.objNeedAnalysis.Financial2 = Convert.ToInt64(objNeedRetireCal.VehicleFund);
                                objProspect.objNeedAnalysis.Financial3 = Convert.ToInt64(objNeedRetireCal.LoanFund);
                                objProspect.objNeedAnalysis.Financial4 = Convert.ToInt64(objNeedRetireCal.OtherFund);
                                objProspect.objNeedAnalysis.objCalculator.FundBalanceTotal = Convert.ToInt64(objNeedRetireCal.FundBalance);
                                objProspect.objNeedAnalysis.objCalculator.PerAnnumIncome = Convert.ToInt64(objNeedRetireCal.PerAnnIncomeIntrest);
                                objProspect.objNeedAnalysis.objCalculator.EstimatedAnnuallivingExpenses = Convert.ToInt64(objNeedRetireCal.EstAnnualLivExp);
                                objProspect.objNeedAnalysis.objCalculator.AnnualIncomeSurplus = Convert.ToInt64(objNeedRetireCal.TotalAnnualExp);
                                objProspect.objNeedAnalysis.objCalculator.Exsitingotherincome = Convert.ToInt64(objNeedRetireCal.ExistingOthIncome);
                                objProspect.objNeedAnalysis.objCalculator.MonthlyPensionGap = Convert.ToInt64(objNeedRetireCal.PensionGap);

                            }
                            var objNeedHealthCal = Context.tblNeedHealthCalculators.Where(a => a.NeedAnalysisID == tblopportunity.NeedAnalysisID).FirstOrDefault();
                            if (objNeedHealthCal != null)
                            {
                                objProspect.objNeedAnalysis.CriticalRequiremenent = Convert.ToInt64(objNeedHealthCal.CriticalillnessReq);
                                objProspect.objNeedAnalysis.CriticalFund = Convert.ToInt64(objNeedHealthCal.CriticalIllenssFund);
                                objProspect.objNeedAnalysis.CriticalGap = Convert.ToInt64(objNeedHealthCal.CriticalIllnessGap);
                                objProspect.objNeedAnalysis.HospitalizationRequiremenent = Convert.ToInt64(objNeedHealthCal.HospReq);
                                objProspect.objNeedAnalysis.HospitalizationFund = Convert.ToInt64(objNeedHealthCal.HospFund);
                                objProspect.objNeedAnalysis.HospitalizationGap = Convert.ToInt64(objNeedHealthCal.HospGap);
                                objProspect.objNeedAnalysis.additionalexpenseRequiremenent = Convert.ToInt64(objNeedHealthCal.AddLossReq);
                                objProspect.objNeedAnalysis.additionalexpenseFund = Convert.ToInt64(objNeedHealthCal.AddLossFund);
                                objProspect.objNeedAnalysis.additionalexpenseGap = Convert.ToInt64(objNeedHealthCal.AddLossGap);
                                objProspect.objNeedAnalysis.objadversities = GetString(objNeedHealthCal.HealthAdversities);
                                objProspect.objNeedAnalysis.objannualamount = objNeedHealthCal.AnnualAmountHealthExp;
                                objProspect.objNeedAnalysis.objcoverage = objNeedHealthCal.CoverageHealthExp;
                                objProspect.objNeedAnalysis.objadequacy = objNeedHealthCal.AdequacyHealthExp;
                                objProspect.objNeedAnalysis.HospitalBills = objNeedHealthCal.HospitalBills;
                                objProspect.objNeedAnalysis.HospitalRtrExp = objNeedHealthCal.HospRetireExp;

                            }
                            var objNeedEduCal = Context.tblNeedEducationCalculators.Where(a => a.NeedAnalysisID == tblopportunity.NeedAnalysisID).FirstOrDefault();
                            if (objNeedEduCal != null)
                            {
                                objProspect.objNeedAnalysis.EduInflationRate = objNeedEduCal.Inflationrate;
                                objProspect.objNeedAnalysis.AnnualEduExpense = Convert.ToInt64(objNeedEduCal.AnnualEduExp);
                                objProspect.objNeedAnalysis.EduMaturity = Convert.ToInt64(objNeedEduCal.EduMaturityValue);
                                objProspect.objNeedAnalysis.EduLumpSum = Convert.ToInt64(objNeedEduCal.LumpSum);
                                objProspect.objNeedAnalysis.EduGapTotal = Convert.ToInt64(objNeedEduCal.EduGapTotal);
                                objProspect.objNeedAnalysis.MonthlyEduExpense = Convert.ToInt64(objNeedEduCal.MonthlyEduExp);


                                var objGCEAL = Context.tblNeedEduGCEALs.Where(a => a.EduCalcID == objNeedEduCal.Id).ToList();
                                if (objGCEAL != null)
                                {
                                    foreach (var item in objGCEAL)
                                    {
                                        GCEAL obj = new GCEAL();
                                        obj.CurrRequirement = Convert.ToInt64(item.CurrentReq);
                                        obj.AvailableFund = Convert.ToInt64(item.AvailableFund);
                                        obj.MaturityAge = Convert.ToInt32(item.MaturityAge);
                                        obj.Age = item.Age;
                                        obj.EstAmount = Convert.ToInt64(item.EstAmount);
                                        obj.Gap = Convert.ToInt64(item.Gap);
                                        obj.Relationship = item.Relationship;
                                        obj.Term = Convert.ToInt32(item.Term);
                                        objProspect.objNeedAnalysis.objGCEAL.Add(obj);

                                    }
                                }
                                var objLocalStudies = Context.tblNeedEduLocals.Where(a => a.EduCalcID == objNeedEduCal.Id).ToList();
                                if (objLocalStudies != null)
                                {
                                    foreach (var item in objLocalStudies)
                                    {
                                        LocalStudies obj = new LocalStudies();
                                        obj.CurrRequirement = Convert.ToInt64(item.CurrentReq);
                                        obj.AvailableFund = Convert.ToInt64(item.AvailableFund);
                                        obj.Age = item.Age;
                                        obj.MaturityAge = Convert.ToInt32(item.MaturityAge);
                                        obj.EstAmount = Convert.ToInt64(item.EstAmount);
                                        obj.Gap = Convert.ToInt64(item.Gap);
                                        obj.Relationship = item.Relationship;
                                        obj.Term = Convert.ToInt32(item.Term);
                                        objProspect.objNeedAnalysis.objLocal.Add(obj);
                                    }
                                }
                                var objHigherStudies = Context.tblNeedEduHighers.Where(a => a.EduCalcID == objNeedEduCal.Id).ToList();
                                if (objHigherStudies != null)
                                {
                                    foreach (var item in objHigherStudies)
                                    {
                                        HigherEduDegree obj = new HigherEduDegree();
                                        obj.CurrRequirement = Convert.ToInt64(item.CurrentReq);
                                        obj.AvailableFund = Convert.ToInt64(item.AvailableFund);
                                        obj.MaturityAge = Convert.ToInt32(item.MaturityAge);
                                        obj.Age = item.Age;
                                        obj.EstAmount = Convert.ToInt64(item.EstAmount);
                                        obj.Gap = Convert.ToInt64(item.Gap);
                                        obj.Relationship = item.Relationship;
                                        obj.Term = Convert.ToInt32(item.Term);
                                        objProspect.objNeedAnalysis.objHigherEdu.Add(obj);
                                    }
                                }
                                var objForeignHigher = Context.tblNeedEduForeigns.Where(a => a.EduCalcID == objNeedEduCal.Id).ToList();
                                if (objForeignHigher != null)
                                {
                                    foreach (var item in objForeignHigher)
                                    {
                                        HigherForeignDegree obj = new HigherForeignDegree();
                                        obj.CurrRequirement = Convert.ToInt64(item.CurrentReq);
                                        obj.AvailableFund = Convert.ToInt64(item.AvailableFund);
                                        obj.MaturityAge = Convert.ToInt32(item.MaturityAge);
                                        obj.Age = item.Age;
                                        obj.EstAmount = Convert.ToInt64(item.EstAmount);
                                        obj.Gap = Convert.ToInt64(item.Gap);
                                        obj.Relationship = item.Relationship;
                                        obj.Term = Convert.ToInt32(item.Term);
                                        objProspect.objNeedAnalysis.objHigherForeign.Add(obj);
                                    }
                                }
                            }
                            var objtblNeedSaveCal = Context.tblNeedSavingCalculators.Where(a => a.NeedAnalysisID == tblopportunity.NeedAnalysisID).FirstOrDefault();

                            if (objtblNeedSaveCal != null)
                            {
                                objProspect.objNeedAnalysis.AnnualSaveExpense = Convert.ToInt64(objtblNeedSaveCal.AnnualSavingExp);
                                objProspect.objNeedAnalysis.SavingTarget = Convert.ToInt64(objtblNeedSaveCal.AnnualSavingExp);
                                objProspect.objNeedAnalysis.SavingReqTotal = Convert.ToInt64(objtblNeedSaveCal.CurrReqTotal);
                                objProspect.objNeedAnalysis.SavingEstTotal = Convert.ToInt64(objtblNeedSaveCal.EstAmountTotal);
                                objProspect.objNeedAnalysis.SavingCurrentTotal = Convert.ToInt64(objtblNeedSaveCal.AvailableFund);
                                objProspect.objNeedAnalysis.SavingGapTotal = Convert.ToInt64(objtblNeedSaveCal.GapTotal);
                                objProspect.objNeedAnalysis.MonthlySaveExpense = Convert.ToInt64(objtblNeedSaveCal.MonthlySaveExp);
                                objProspect.objNeedAnalysis.SavInflationRate = Convert.ToInt32(objtblNeedSaveCal.Inflationrate);

                                var objNeedSavWedding = Context.tblNeedSaveWeddings.Where(a => a.SaveCalcID == objtblNeedSaveCal.Id).ToList();
                                if (objNeedSavWedding != null)
                                {
                                    foreach (var item in objNeedSavWedding)
                                    {
                                        Wedding obj = new Wedding();
                                        obj.CurrRequirement = Convert.ToInt64(item.CurrentReq);
                                        obj.Term = Convert.ToInt32(item.Term);
                                        obj.MaturityAge = Convert.ToInt32(item.MaturityAge);
                                        obj.EstAmount = Convert.ToInt64(item.EstAmount);
                                        obj.AvailableFund = Convert.ToInt64(item.AvailableFund);
                                        obj.Gap = Convert.ToInt64(item.Gap);
                                        obj.Relationship = item.Relationship;
                                        obj.Age = item.Age;
                                        objProspect.objNeedAnalysis.objWedding.Add(obj);
                                    }
                                }
                                var objNeedSavHouse = Context.tblNeedSaveHouses.Where(a => a.SaveCalcID == objtblNeedSaveCal.Id).ToList();
                                if (objNeedSavHouse != null)
                                {
                                    foreach (var item in objNeedSavHouse)
                                    {
                                        House obj = new House();
                                        obj.CurrRequirement = Convert.ToInt64(item.CurrentReq);
                                        obj.Term = Convert.ToInt32(item.Term);
                                        obj.MaturityAge = Convert.ToInt32(item.MaturityAge);
                                        obj.EstAmount = Convert.ToInt64(item.EstAmount);
                                        obj.AvailableFund = Convert.ToInt64(item.AvailableFund);
                                        obj.Gap = Convert.ToInt64(item.Gap);
                                        obj.Relationship = item.Relationship;
                                        obj.Age = item.Age;
                                        objProspect.objNeedAnalysis.objHouse.Add(obj);
                                    }
                                }
                                var objNeedSavCar = Context.tblNeedSaveCars.Where(a => a.SaveCalcID == objtblNeedSaveCal.Id).ToList();
                                if (objNeedSavCar != null)
                                {
                                    foreach (var item in objNeedSavCar)
                                    {
                                        Car obj = new Car();
                                        obj.CurrRequirement = Convert.ToInt64(item.CurrentReq);
                                        obj.Term = Convert.ToInt32(item.Term);
                                        obj.MaturityAge = Convert.ToInt32(item.MaturityAge);
                                        obj.EstAmount = Convert.ToInt64(item.EstAmount);
                                        obj.AvailableFund = Convert.ToInt64(item.AvailableFund);
                                        obj.Gap = Convert.ToInt64(item.Gap);
                                        obj.Relationship = item.Relationship;
                                        obj.Age = item.Age;
                                        objProspect.objNeedAnalysis.objCar.Add(obj);
                                    }
                                }
                                var objNeedSavTours = Context.tblNeedSaveTours.Where(a => a.SaveCalcID == objtblNeedSaveCal.Id).ToList();
                                if (objNeedSavTours != null)
                                {
                                    foreach (var item in objNeedSavTours)
                                    {
                                        ForeignTour obj = new ForeignTour();
                                        obj.CurrRequirement = Convert.ToInt64(item.CurrentReq);
                                        obj.Term = Convert.ToInt32(item.Term);
                                        obj.MaturityAge = Convert.ToInt32(item.MaturityAge);
                                        obj.EstAmount = Convert.ToInt64(item.EstAmount);
                                        obj.AvailableFund = Convert.ToInt64(item.AvailableFund);
                                        obj.Gap = Convert.ToInt64(item.Gap);
                                        obj.Relationship = item.Relationship;
                                        obj.Age = item.Age;
                                        objProspect.objNeedAnalysis.objForeignTour.Add(obj);
                                    }
                                }
                                var objNeedSavOthers = Context.tblNeedSaveOthers.Where(a => a.SaveCalcID == objtblNeedSaveCal.Id).ToList();
                                if (objNeedSavOthers != null)
                                {
                                    foreach (var item in objNeedSavOthers)
                                    {
                                        Others obj = new Others();
                                        obj.CurrRequirement = Convert.ToInt64(item.CurrentReq);
                                        obj.Term = Convert.ToInt32(item.Term);
                                        obj.MaturityAge = Convert.ToInt32(item.MaturityAge);
                                        obj.EstAmount = Convert.ToInt64(item.EstAmount);
                                        obj.AvailableFund = Convert.ToInt64(item.AvailableFund);
                                        obj.Gap = Convert.ToInt64(item.Gap);
                                        obj.Relationship = item.Relationship;
                                        obj.Age = item.Age;
                                        objProspect.objNeedAnalysis.objOthers.Add(obj);
                                    }
                                }
                            }
                            var objNeedHumanValueCal = Context.tblNeedHumanValueCalculators.Where(a => a.NeedAnalysisID == tblopportunity.NeedAnalysisID).FirstOrDefault();
                            if (objNeedHumanValueCal != null)
                            {
                                objProspect.objNeedAnalysis.MonthlyEarning = Convert.ToInt64(objNeedHumanValueCal.MonthlyEarning);
                                objProspect.objNeedAnalysis.YearsofEarning = Convert.ToInt32(objNeedHumanValueCal.NoOfYears);
                                objProspect.objNeedAnalysis.ProIntrestRate = Convert.ToInt32(objNeedHumanValueCal.IntrestRate);
                                objProspect.objNeedAnalysis.EstimatedIncome = Convert.ToInt64(objNeedHumanValueCal.EstIncome);
                                objProspect.objNeedAnalysis.FutureFund = Convert.ToInt64(objNeedHumanValueCal.FutureAvailableFund);
                                objProspect.objNeedAnalysis.AvailableFund = Convert.ToInt64(objNeedHumanValueCal.AvailableFund);
                                objProspect.objNeedAnalysis.EmergencyFund = Convert.ToInt64(objNeedHumanValueCal.EmergencyFundReq);

                            }

                        }
                        else
                        {
                            GetNeedsData(objProspect);
                        }
                        if (objProspect.objNeedAnalysis.CreatedDate == null)
                        {
                            objProspect.objNeedAnalysis.CreatedDate = DateTime.Now;
                        }

                    }
                }

                Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
                List<int> lstCommontypeID = new List<int>();
                lstCommontypeID.Add(269);
                lstCommontypeID.Add(270);

                objProspect.objPreviousInsuranceList = LoadPreviousInsuranceGrid(objProspect);
                //objProspect.objNeedAnalysis.EmergencyFund1 = Convert.ToInt32(objProspect.objPreviousInsuranceList[0].Deathbenifit);
                //objProspect.objNeedAnalysis.EmergencyFund2 = Convert.ToInt32(objProspect.objPreviousInsuranceList[1].Deathbenifit);
                //objProspect.objNeedAnalysis.EmergencyFund3 = Convert.ToInt32(objProspect.objPreviousInsuranceList[2].Deathbenifit);
                //objProspect.objNeedAnalysis.Policy1 = Convert.ToString(objProspect.objPreviousInsuranceList[0].PolicyNumber);
                //objProspect.objNeedAnalysis.Policy2 = Convert.ToString(objProspect.objPreviousInsuranceList[1].PolicyNumber);
                //objProspect.objNeedAnalysis.Policy3 = Convert.ToString(objProspect.objPreviousInsuranceList[2].PolicyNumber);
                objProspect.objNeedAnalysis.dllChildName = objCommonBusiness.GetChildName(objProspect);
                objProspect.objNeedAnalysis.dllChildName.Add(new MasterListItem { Text = objProspect.Name, Value = objProspect.Name });
                if (objProspect.objNeedAnalysis.objSpouseDetails.FullName != null)
                {
                    objProspect.objNeedAnalysis.dllChildName.Add(new MasterListItem { Text = objProspect.objNeedAnalysis.objSpouseDetails.FullName, Value = objProspect.objNeedAnalysis.objSpouseDetails.FullName });

                }
                objProspect.objNeedAnalysis.dllChildRelatioship = objCommonBusiness.GetChildRelationship();
                objProspect.objNeedAnalysis.dllannualamount = objCommonBusiness.GetAnnualAmountFNA();
                objProspect.objNeedAnalysis.dlladversities = objCommonBusiness.GetHealthAdversitiesFNA();
                objProspect.objNeedAnalysis.dlladequacy = objCommonBusiness.GetAdequacyFNA();
                objProspect.objNeedAnalysis.dllcoverage = objCommonBusiness.GetCoverageFNA();
                objProspect.lstSalutation = objCommonBusiness.GetSalutation();
                objProspect.lstOccupation = objCommonBusiness.GetOccupation();
                objProspect.lstGender = objCommonBusiness.GetGender();
                objProspect.MaritalStatuslist = objCommonBusiness.GetMaritalStatus();
                objProspect.lstRelations = objCommonBusiness.GetMasCommonTypeMasterListItem("HealthRelationship");
                objProspect.lstMotorVehicle = objCommonBusiness.GetMasCommonTypeMasterListItem("VehicleType");
                // objProspect.lstDependentRelationship = objCommonBusiness.GetMasCommonTypeMasterListItem(null, null, lstCommontypeID);
                objProspect.lstAvgMonthlyIncome = objCommonBusiness.GetMasCommonTypeMasterListItem("MonthlyIncomeRange");
                objProspect.lstCurrentStatus = objCommonBusiness.GetMasCommonTypeMasterListItem("PrevInsurenceCurrentStatus");
                objProspect.lstNeedsPriority = objCommonBusiness.GetMasCommonTypeMasterListItem("NeedPriorityValue");
                objProspect.lstPurposeOfMeeting = objCommonBusiness.GetMasCommonTypeMasterListItem("PurposeOfNextMeeting");
                objProspect.ListPlan = objCommonBusiness.ListProducts();
                string CreatedBy = objProspect.CreatedBy;
                string role = Context.GetRoleByUserName(CreatedBy).FirstOrDefault();
                if (!string.IsNullOrEmpty(role))
                {
                    if (role == "Banca Agent ")
                    {
                        objProspect.LstType = objCommonBusiness.GetBancaType();
                    }
                    if (role != "Banca Agent ")
                    {
                        objProspect.LstType = objCommonBusiness.GetType();
                    }
                }
                objProspect.LstPensionPeriod = objCommonBusiness.GetPensionPeriod();
                objProspect.LstRetirementAge = objCommonBusiness.GetRetirementAge();
                objProspect.LstDrawDownPeriod = objCommonBusiness.GetDrawDownPeriod();
                objProspect.LstMaturityBenefits = objCommonBusiness.GetMaturityBenefits();
                objProspect.ListPlan = objCommonBusiness.ListProducts();
                objProspect.objNeedAnalysis.objFamilyIncome.RateOfInterest = Convert.ToInt32(ConfigurationManager.AppSettings["RateOfInterest"]);


                return objProspect;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<PreviousInsuranceList> LoadPreviousInsuranceGrid(AIA.Life.Models.Opportunity.Prospect ObjProspect)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            var LstDetails = Context.usp_GetPreviousPolicyDetails(ObjProspect.NIC).ToList();
            List<PreviousInsuranceList> lstPreviousInsuranceList = new List<PreviousInsuranceList>();
            lstPreviousInsuranceList = LstDetails.Select(a => new PreviousInsuranceList()
            {
                AnnualPremium = Convert.ToString(a.POlPREM),
                PolicyNumber = a.POLICYNO,
                NameOfTheComp = "AIA",
                SumAssured = "0",
                Deathbenifit = Convert.ToString(a.ADB),
                IllNessBenifit = Convert.ToString(a.CI),
                PermanentDisability = Convert.ToString(a.WOB),
                HospitalizationPerDay = Convert.ToString(a.HDB),
                status = a.Longdesc
            }).ToList();
            ObjProspect.objPreviousInsuranceList.AddRange(lstPreviousInsuranceList);
            //System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            //con.Open();
            //System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            //cmd.Connection = con;
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "usp_GetPreviousPolicyDetails";
            //cmd.Parameters.Add("@NICNo", SqlDbType.VarChar);
            //cmd.Parameters[0].Value = ObjLifeQuote.objProspect.NIC;

            //DataSet ds = new DataSet();
            //System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
            //da.Fill(ds);
            //List<PreviousInsuranceList> lstPreviousInsuranceList = new List<PreviousInsuranceList>();
            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    PreviousInsuranceList previousInsuranceList = new PreviousInsuranceList();
            //    previousInsuranceList.NameOfTheComp = "AIA";
            //    previousInsuranceList.PolicyNumber = Convert.ToString(ds.Tables[0].Rows[i]["POLICYNO"]);
            //    previousInsuranceList.SumAssured = 0;
            //    previousInsuranceList.AnnualPremium = Convert.ToDecimal(ds.Tables[0].Rows[i]["POlPREM"]);
            //    previousInsuranceList.Deathbenifit = Convert.ToString(ds.Tables[0].Rows[i]["ADB"]);
            //    previousInsuranceList.IllNessBenifit = Convert.ToString(ds.Tables[0].Rows[i]["CI"]);
            //    previousInsuranceList.PermanentDisability = Convert.ToString(ds.Tables[0].Rows[i]["WOB"]);
            //    previousInsuranceList.status = Convert.ToString(ds.Tables[0].Rows[i]["Longdesc"]);
            //    lstPreviousInsuranceList.Add(previousInsuranceList);
            //}

            return ObjProspect.objPreviousInsuranceList;
        }
        public void DeleteExisitingInsurerInfo(int NeedAnalysisID)
        {
            using (AVOAIALifeEntities entity = new AVOAIALifeEntities())
            {

                foreach (tblPreviousInsurenceInfo obj in entity.tblPreviousInsurenceInfoes.Where(a => a.NeedAnalysisID == NeedAnalysisID).ToList())
                {
                    entity.tblPreviousInsurenceInfoes.Remove(obj);
                }
                entity.SaveChanges();

            }


        }
        public List<Needs> GetNeedsData(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            var objtblMasNeeds = (from e in Context.tblMasNeeds
                                  select e).ToList();

            foreach (var item in objtblMasNeeds)
            {
                Needs objMassNeeds = new Needs();

                objMassNeeds.NeedID = Convert.ToInt32(item.NeedID);
                objMassNeeds.NeedName = item.NeedName;
                objMassNeeds.Priority = "";
                objMassNeeds.Value = "";
                objMassNeeds.IsNeedOpted = false;
                objMassNeeds.PlanSuggested = item.SuggestedProductName;
                objMassNeeds.ImagePath = item.ImagePath;
                objProspect.objNeedAnalysis.objNeeds.Add(objMassNeeds);
            }
            return objProspect.objNeedAnalysis.objNeeds;
        }

        public AIA.Life.Models.Opportunity.Prospect DeleteOpportunity(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            try
            {

                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {

                    var Opportunity = Context.tblOpportunities.Where(a => a.ContactID == objProspect.ContactID).FirstOrDefault();
                    if (Opportunity != null)
                    {
                        Opportunity.IsDeleted = true;
                        Context.SaveChanges();
                        objProspect.Message = "Success";
                    }
                    else
                    {
                        objProspect.Message = "Error";
                    }
                }
            }
            catch (Exception)
            {
                objProspect.Message = "Error";


            }
            return objProspect;
        }
        public QuoteList GetVariant(string Plan)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            QuoteList objQuoteList = new QuoteList();
            try
            {
                int ProductId = Context.tblProducts.Where(a => a.ProductName == Plan).Select(b => b.ProductId).FirstOrDefault();

                //int planid =Context.tblMasProductPlans.Where(a => a.ProductId == ProductId).Select(b => b.PlanId).FirstOrDefault();
                if (Plan != null && Plan != "")
                {
                    objQuoteList.ObjLifeQuote.objProspect.ListVariant = Context.tblMasProductPlans.Where(a => a.ProductId == ProductId && a.Active == true).Select(a => new MasterListItem { ID = a.PlanId, Text = a.PlanDescriprion }).ToList();
                }
                return objQuoteList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public QuoteList GetReason(string Decision)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            QuoteList objQuoteList = new QuoteList();
            try
            {
                //Decline - 187 = reason field,not taken up - 1176 = reason field,postpone - 1449 = reason & Duration,withdrawn - 2299 = reason field
                if (Decision == "187")
                {
                    Decision = "Decline";
                }
                else if (Decision == "1176")
                {
                    Decision = "NOT TAKEN UP";
                }
                else if (Decision == "1449")
                {
                    Decision = "postpone";
                }
                else if (Decision == "2299")
                {
                    Decision = "withdrawn";
                }
                int ProductId;
                List<string> Reasons = Context.tblUWReasons.Where(a => a.Reason == Decision || a.Reason2 == Decision).Select(b => b.LongDescription).ToList();
                objQuoteList.objListReason = Reasons;
                //int planid =Context.tblMasProductPlans.Where(a => a.ProductId == ProductId).Select(b => b.PlanId).FirstOrDefault();
                if (Decision != null && Decision != "")
                {
                    // objQuoteList.ObjLifeQuote.objProspect.ListVariant = Context.tblUWReasons.Where(a => a.Reason =Decision||a.Reason2=Decision).Select(a => new MasterListItem { ID = a.PlanId, Text = a.PlanDescriprion }).ToList();
                }
                return objQuoteList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public QuoteList GetPlanCode(int plan)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            QuoteList objQuoteList = new QuoteList();
            try
            {
                objQuoteList.ObjLifeQuote.objProductDetials.PlanCode = Context.tblMasProductPlans.Where(a => a.PlanId == plan).Select(a => a.PlanCode).SingleOrDefault();

                objQuoteList.ObjLifeQuote.LstPolicyTerm = Context.tblMasPolicyTerms.Where(a => a.PlanID == plan).Select(b => new MasterListItem { Value = b.Term.ToString(), Text = b.Term.ToString() }).ToList();

                objQuoteList.ObjLifeQuote.LstPremiumTerm = Context.tblMasProductPlans.Where(a => a.PlanId == plan).Select(b => new MasterListItem { Value = b.PremiumTerm.ToString(), Text = b.PremiumTerm.ToString() }).ToList();

                Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
                List<MasterListItem> LstPreferedMode = objCommonBusiness.GetPreferredModes(objQuoteList.ObjLifeQuote.objProductDetials.PlanCode);
                foreach (var item in objQuoteList.ObjLifeQuote.LstPremiumTerm)
                {
                    if (item.Value == "4")
                    {
                        LstPreferedMode.RemoveAt(1);
                        LstPreferedMode.RemoveAt(1);
                        LstPreferedMode.RemoveAt(1);


                    }
                    objQuoteList.ObjLifeQuote.lstPrefMode = LstPreferedMode;
                }
                if (objQuoteList.ObjLifeQuote.objProductDetials.PlanCode == "HPA")
                {
                    LstPreferedMode.RemoveAt(3);
                }
                objQuoteList.ObjLifeQuote.lstPrefMode = LstPreferedMode;

                return objQuoteList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public LifeQuote GetSAM(int plan, int Age)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            LifeQuote ObjLifeQuote = new LifeQuote();
            try
            {
                List<tblMasSAM> lsttblMasSAM = Context.tblMasSAMs.Where(a => a.PlanId == plan).ToList();
                int MinSAM = lsttblMasSAM.Where(a => a.MinAge <= Age && a.MixAge >= Age).Select(a => a.MinSAM).FirstOrDefault();


                int MaxSAM = lsttblMasSAM.Where(a => a.MinAge <= Age && a.MixAge >= Age).Select(a => a.MixSAM).FirstOrDefault();
                for (int i = MinSAM; i <= MaxSAM; i++)
                {
                    ObjLifeQuote.lstSAM.Add(new MasterListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString()
                    });
                }

                return ObjLifeQuote;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public int GetGenderStatus(string Code)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            return Context.tblMasCommonTypes.Where(a => a.Code == Code).Select(a => a.CommonTypesID).FirstOrDefault();
        }
        public string GetGenderCode(int ID)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            return Context.tblMasCommonTypes.Where(a => a.CommonTypesID == ID).Select(a => a.Code).FirstOrDefault();
        }
        public string StringArray(string[] strArr)
        {
            string res = string.Empty;
            for (int i = 0; i < strArr.Count(); i++)
            {
                if (i == 0)
                {
                    res += strArr[i];
                }
                else
                {
                    res = res + "," + strArr[i];

                }
            }
            return res;
        }
        public string[] GetString(string str)
        {
            string[] res;
            if (str != null)
            {
                res = str.Split(',');
            }
            else
            {
                res = null;
            }
            return res;
        }
        public SuspectPool AllocateLead(SuspectPool objSuspectPool)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            //tblUserDetail objtblUserDetail =  Context.tblUserDetails.Where(a => a.LoginID == objSuspectPool.AssignedTo).FirstOrDefault();
            tblOpportunity objOppurtunity = Context.tblOpportunities.Where(a => a.ContactID == objSuspectPool.ContactId).FirstOrDefault();
            string userId = Common.CommonBusiness.GetUserId(objSuspectPool.AssignedTo);
            tblContact objtblContact = Context.tblContacts.Where(a => a.ContactID == objSuspectPool.ContactId).FirstOrDefault();
            try
            {
                objtblContact.CreatedBy = userId;
                objOppurtunity.tblContact = objtblContact;
                objOppurtunity.Createdby = userId;
                objOppurtunity.StageID = 1;
                Context.SaveChanges();

                AIA.Life.Models.Opportunity.Prospect prospect = new AIA.Life.Models.Opportunity.Prospect();
                prospect.ContactID = objtblContact.ContactID;
                prospect.AssignedTo = objSuspectPool.AssignedTo;                
                prospect = LoadContactInformation(prospect);
                prospect.UserName = objSuspectPool.UserName;
                objSuspectPool.Message = "Success";
                //SamsClient samsClient = new SamsClient();

                //samsClient.UpdateLead(Context, prospect);
                return objSuspectPool;
            }
            catch (Exception ex)
            {
                objSuspectPool.Message = "Error";
                return objSuspectPool;
                throw;
            }
        }
        //public Suspect GetSuspect( suspect)
        //{
        //    using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
        //    {
        //        var tblcontact = Context.tblContacts.Where(a => a.ContactID == objProspect.ContactID).FirstOrDefault();
        //        if (tblcontact != null)
        //        {
        //            objProspect.Type = tblcontact.ContactType;
        //            objProspect.Name = tblcontact.FirstName;
        //            objProspect.LastName = tblcontact.LastName;
        //            objProspect.Email = tblcontact.EmailID;
        //            objProspect.Mobile = tblcontact.MobileNo;
        //            objProspect.Home = tblcontact.PhoneNo;
        //            objProspect.Work = tblcontact.Work;
        //            objProspect.AgeNextBdy = Convert.ToInt32(tblcontact.Age);
        //            objProspect.CurrentAge = Convert.ToInt32(tblcontact.CurrentAge);
        //            objProspect.ClientCode = tblcontact.ClientCode;
        //            objProspect.DateofBirth = tblcontact.DateOfBirth;
        //            objProspect.EmployerName = tblcontact.Employer;
        //            // Prospect DOB for Need Analysis
        //            objProspect.objNeedAnalysis.ProspectDOB = objProspect.DateofBirth;
        //            objProspect.Occupation = Context.tblMasLifeOccupations.Where(a => a.CompanyCode == tblcontact.OccupationID.ToString()).Select(a => a.OccupationCode).FirstOrDefault();
        //            objProspect.NIC = tblcontact.NICNO;
        //        }
        //            return suspect;
        //    }
        //}
        public CampaignLeadType SaveCampaignLead(CampaignLeadType objCampaignLead)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            int commontypeid = Context.tblMasCommonTypes.Where(a => a.CommonTypesID != 0)
                .OrderByDescending(a => a.CommonTypesID).Take(1).Select(a => a.CommonTypesID).FirstOrDefault();
            tblMasCommonType objtblMasCommonType = new tblMasCommonType();
            objtblMasCommonType.CommonTypesID = commontypeid + 1;
            objtblMasCommonType.Code = objCampaignLead.Code;
            objtblMasCommonType.Description = objCampaignLead.Description;
            objtblMasCommonType.MasterType = "Type";
            objtblMasCommonType.isDeleted = 0;
            objtblMasCommonType.EffectiveDate = DateTime.Now;
            objtblMasCommonType.ShortDesc = "";
            objtblMasCommonType.IsValid = true;
            Context.tblMasCommonTypes.Add(objtblMasCommonType);
            Context.SaveChanges();



            return objCampaignLead;
        }
     
    }
}