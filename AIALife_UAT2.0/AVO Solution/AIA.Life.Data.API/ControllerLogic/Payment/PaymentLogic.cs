using AIA.Life.Data.API.ControllerLogic.Policy;
using AIA.Life.Data.API.ControllerLogic.UWRules;
using AIA.Life.Repository.AIAEntity;
using AIA.Life.Models.Payment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using AIA.CrossCutting;
using log4net;
//using AIA.Life.Integration.Services.LifeAsiaIntegration;
using AIA.Life.Integration.Services.EmailandSMS;
using AIA.Life.Models.EmailSMSDetails;
//using AIA.Life.Integration.Services.OnlineReceipt;
using System.Threading;
using System.Configuration;
using System.Globalization;
//using AIA.Life.Integration.Services.SamsIntegration;

namespace AIA.Life.Data.API.ControllerLogic.Payment
{
    public class PaymentLogic
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        public PaymentModel FetchProposals(PaymentModel objPaymentModel)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            //tblProduct objtblProduct = new tblProduct();
            if (string.IsNullOrEmpty(objPaymentModel.QuoteNo))
            {
                objPaymentModel.lstPaymentItems = (from objpolicy in Context.tblPolicies
                                                   join objtblpolicyrelationship in Context.tblPolicyRelationships on objpolicy.PolicyID equals objtblpolicyrelationship.PolicyID
                                                   join objtblpolicyclients in Context.tblPolicyClients on objtblpolicyrelationship.PolicyClientID equals objtblpolicyclients.PolicyClientID
                                                   join objProposalPayments in Context.tblProposalPremiums on objpolicy.PolicyID equals objProposalPayments.PolicyID
                                                   join objProduct in Context.tblProducts on objpolicy.PlanID equals objProduct.ProductId
                                                   where objpolicy.PolicyStageStatusID == 1153 // Payment Pending
                                                   select new PaymentItems
                                                   {
                                                       //PaymentId = objpolicy.
                                                       ProposalNo = objpolicy.ProposalNo,
                                                       //InsuredName = objtblpolicyclients.FirstName,
                                                       InsuredName = (objtblpolicyclients.FullName != "CORP" ? objtblpolicyclients.FirstName : objtblpolicyclients.CorporateName),
                                                       PlanName = objProduct.ProductName,
                                                       PolicyId = objpolicy.PolicyID,
                                                       PolicyTerm = objpolicy.PolicyTerm,
                                                       IssueDate = objpolicy.CreatedDate,
                                                       Premium = objProposalPayments.AnnualPremium,
                                                       CustomerMobile = objtblpolicyclients.MobileNo,
                                                       PreferredLanguage = objpolicy.PreferredLanguage,
                                                       Salutation = objtblpolicyclients.Title,
                                                       PolicyStartDate = objpolicy.PolicyStartDate,
                                                       PolicyEndDate = objpolicy.PolicyEndDate,
                                                       ProductName = objProduct.ProductName,
                                                       ProductID = objProduct.ProductId,
                                                       Email = objtblpolicyclients.EmailID,
                                                       PrefferedMode = objpolicy.PaymentFrequency,
                                                       PlanId = objpolicy.PlanID,
                                                       Mobile = objtblpolicyclients.MobileNo,
                                                       IsAfc = objpolicy.IsAfc
                                                   }).ToList();
            }
            else
            {
                objPaymentModel.lstPaymentItems = (from objpolicy in Context.tblPolicies.Where(a => a.QuoteNo == objPaymentModel.QuoteNo && a.Createdby == Context.tblUserDetails.Where(ud => ud.LoginID == objPaymentModel.UserName).Select(ud => ud.UserID).FirstOrDefault().ToString())
                                                   join objtblpolicyrelationship in Context.tblPolicyRelationships on objpolicy.PolicyID equals objtblpolicyrelationship.PolicyID
                                                   join objtblpolicyclients in Context.tblPolicyClients on objtblpolicyrelationship.PolicyClientID equals objtblpolicyclients.PolicyClientID
                                                   join objProposalPayments in Context.tblProposalPremiums on objpolicy.PolicyID equals objProposalPayments.PolicyID
                                                   join objProduct in Context.tblProducts on objpolicy.ProductID equals objProduct.ProductId
                                                   //where objpolicy.PolicyStageStatusID == 1153 && objpolicy.QuoteNo == objPaymentModel.QuoteNo // Payment Pending
                                                   select new PaymentItems
                                                   {
                                                       ProposalNo = objpolicy.ProposalNo,
                                                       //InsuredName = objtblpolicyclients.FirstName,
                                                       InsuredName = (objtblpolicyclients.FullName != "CORP" ? objtblpolicyclients.FirstName : objtblpolicyclients.CorporateName),
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
                                                       Mobile = objtblpolicyclients.MobileNo,
                                                       QuoteNo = objpolicy.QuoteNo,
                                                       ProductCode = objProduct.ProductCode,
                                                       PlanCode = Context.tblMasProductPlans.Where(pp => pp.PlanId == objpolicy.PlanID).FirstOrDefault().PlanCode,
                                                       IsAfc = objpolicy.IsAfc
                                                   }).OrderByDescending(a => a.PolicyId).ToList();
            }

            Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
            objPaymentModel.LstInstrumentType = objCommonBusiness.GetInstrumentType();

            objPaymentModel.lstPayableCurrency = objCommonBusiness.GetCurrency();

            return objPaymentModel;
        }

        public PaymentModel SaveProposalPaymentInfo(PaymentModel objPaymentModel)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                tblPolicy objtblpolicy = new tblPolicy();
                objtblpolicy = Context.tblPolicies.Where(a => a.ProposalNo == objPaymentModel.ProposalNo).FirstOrDefault();

                if (objtblpolicy != null)
                {
                    var proposalPremium = objtblpolicy.tblProposalPremiums.FirstOrDefault();
                    decimal? actualPremium = proposalPremium.AnnualPremium + proposalPremium.AdditionalPremium;
                    decimal? payablePremium = Convert.ToDecimal(objPaymentModel.PayableAmount);
                    if (proposalPremium.AdditionalPremium != null && proposalPremium.AdditionalPremium != 0)
                    {
                        payablePremium = payablePremium + proposalPremium.AnnualPremium;
                    }
                    if (payablePremium < (actualPremium - 100) || (!string.IsNullOrEmpty(objPaymentModel.ChequeNo) && objPaymentModel.chequeAmount < (actualPremium - 100)))
                    {
                        objPaymentModel.Error.ErrorMessage = "Please check the Payable premium and entered amount.";
                    }
                    else
                    {
                        objPaymentModel.QuoteNo = objtblpolicy.QuoteNo;
                        tblPolicyPaymentMap policyPaymentMap = objtblpolicy.tblPolicyPaymentMaps.FirstOrDefault();
                        if (policyPaymentMap == null)
                        {
                            policyPaymentMap = new tblPolicyPaymentMap();
                            policyPaymentMap.CreatedDate = DateTime.Now;
                        }
                        tblPayment payment = policyPaymentMap.tblPayment;
                        if (payment == null)
                        {
                            payment = new tblPayment();
                            payment.CreatedDate = DateTime.Now;
                        }
                        tblPaymentInstrumentDetail tblPaymentInstrument = new tblPaymentInstrumentDetail();

                        payment.TxnNo = objPaymentModel.TransactionNo;
                        payment.PaidAmount = Convert.ToDecimal(objPaymentModel.PayableAmount);
                        payment.ChequeSubmission = false;
                        payment.ReceiptNo = "NOTACK";

                        if (payment.PaymentID == decimal.Zero)
                            Context.tblPayments.Add(payment);


                        tblPaymentInstrument.InstrumentAmount = Convert.ToDecimal(objPaymentModel.PayableAmount);
                        if (objPaymentModel.SelectedPayment == "othertypes")
                        {
                            tblPaymentInstrument.PaymentMode = objPaymentModel.PaymentChanel;
                            tblPaymentInstrument.InstrumentNo = objPaymentModel.ChequeNo;
                            tblPaymentInstrument.BankBranch = objPaymentModel.BranchName;

                        }
                        else
                            tblPaymentInstrument.PaymentMode = objPaymentModel.SelectedPayment;

                        tblPaymentInstrument.InstrumentDate = DateTime.Now;
                        tblPaymentInstrument.UpdatedDate = DateTime.Now;
                        tblPaymentInstrument.tblPayment = payment;
                        Context.tblPaymentInstrumentDetails.Add(tblPaymentInstrument);

                        policyPaymentMap.Premium = objtblpolicy.tblProposalPremiums.FirstOrDefault().AnnualPremium;
                        policyPaymentMap.PaidAmount = Convert.ToDecimal(objPaymentModel.PayableAmount);
                        policyPaymentMap.PendingAmount = policyPaymentMap.Premium - policyPaymentMap.PaidAmount;
                        policyPaymentMap.tblPayment = payment;
                        policyPaymentMap.tblPolicy = objtblpolicy;
                        if (policyPaymentMap.PolicyPaymentMapID == decimal.Zero)
                            Context.tblPolicyPaymentMaps.Add(policyPaymentMap);
                        Context.SaveChanges();
                        objPaymentModel.PayableAmount = Convert.ToString(actualPremium);
                        objPaymentModel = AckPaymentandProcess(Context, objPaymentModel, objtblpolicy);
                    }
                }

                return objPaymentModel;

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw ex;
            }
        }

        public PaymentModel SavePGTransaction(PaymentModel objPaymentModel)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                tblPolicy objtblpolicy = new tblPolicy();
                objtblpolicy = Context.tblPolicies.Where(a => a.ProposalNo == objPaymentModel.ProposalNo).FirstOrDefault();

                if (objtblpolicy != null)
                {

                    tblPolicyPaymentMap policyPaymentMap = new tblPolicyPaymentMap();
                    policyPaymentMap.CreatedDate = DateTime.Now;
                    tblPayment payment = new tblPayment();
                    payment.CreatedDate = DateTime.Now;


                    payment.TxnNo = objPaymentModel.TransactionNo;
                    payment.PaidAmount = Convert.ToDecimal(objPaymentModel.PayableAmount);
                    payment.ChequeSubmission = false;
                    payment.ReceiptNo = "NOTACK";
                    payment.PayerType = objPaymentModel.ReqId;
                    if (payment.PaymentID == decimal.Zero)
                        Context.tblPayments.Add(payment);


                    policyPaymentMap.Premium = objtblpolicy.tblProposalPremiums.FirstOrDefault().AnnualPremium;
                    policyPaymentMap.PaidAmount = 0;
                    policyPaymentMap.PendingAmount = policyPaymentMap.Premium - policyPaymentMap.PaidAmount;
                    policyPaymentMap.tblPayment = payment;
                    policyPaymentMap.tblPolicy = objtblpolicy;
                    if (policyPaymentMap.PolicyPaymentMapID == decimal.Zero)
                        Context.tblPolicyPaymentMaps.Add(policyPaymentMap);

                }
                Context.SaveChanges();
                return objPaymentModel;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PaymentModel UpdatePGTransaction(PaymentModel objPaymentModel)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                tblPayment payment = Context.tblPayments.Where(a => a.PayerType == objPaymentModel.ReqId).FirstOrDefault();
                if (payment != null)
                {
                    tblPolicyPaymentMap policyPaymentMap = payment.tblPolicyPaymentMaps.FirstOrDefault();
                    tblPaymentInstrumentDetail tblPaymentInstrument = new tblPaymentInstrumentDetail();
                    objPaymentModel.TransactionNo = payment.TxnNo;
                    payment.UpdatedDate = DateTime.Now;
                    tblPaymentInstrument.InstrumentAmount = Convert.ToDecimal(objPaymentModel.PayableAmount);
                    tblPaymentInstrument.PaymentMode = objPaymentModel.SelectedPayment;
                    tblPaymentInstrument.PGResponse = objPaymentModel.PGResponse;
                    tblPaymentInstrument.RGIPaymentStatus = objPaymentModel.Message;

                    tblPaymentInstrument.InstrumentDate = DateTime.Now;
                    tblPaymentInstrument.UpdatedDate = DateTime.Now;
                    tblPaymentInstrument.tblPayment = payment;
                    Context.tblPaymentInstrumentDetails.Add(tblPaymentInstrument);

                    policyPaymentMap.PaidAmount = Convert.ToDecimal(objPaymentModel.PayableAmount);
                    policyPaymentMap.PendingAmount = policyPaymentMap.Premium - policyPaymentMap.PaidAmount;
                    Context.SaveChanges();

                    if (ConfigurationManager.AppSettings["PublishEnvironment"].ToString() == "SIT" || ConfigurationManager.AppSettings["PublishEnvironment"].ToString() == "UAT")
                    {
                        objPaymentModel.PayableAmount = Convert.ToString(policyPaymentMap.Premium);
                    }

                    tblPolicy objtblpolicy = policyPaymentMap.tblPolicy;
                    string responseCode = string.Empty;
                    string responseText = string.Empty;
                    if (!string.IsNullOrEmpty(objPaymentModel.PGResponse))
                    {
                        var resp = objPaymentModel.PGResponse.Split('|');
                        if (resp.Length > 1)
                        {
                            responseCode = resp[0];
                            responseText = resp[1];
                        }
                    }
                    if (objtblpolicy != null && responseCode == "00")
                    {
                        objPaymentModel.QuoteNo = objtblpolicy.QuoteNo;
                        objPaymentModel.ProposalNo = objtblpolicy.ProposalNo;
                        //DialogReceipt dialogReceipt = new DialogReceipt();
                        //dialogReceipt.SendDialogOnlinePremium(objPaymentModel);
                        Thread.Sleep(60000);
                        objPaymentModel = AckPaymentandProcess(Context, objPaymentModel, objtblpolicy);
                    }
                    else
                        objPaymentModel.Error.ErrorMessage = "Your payment was unsuccessful, please check the accuracy of your details and try again " + responseText;
                }
                return objPaymentModel;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PaymentModel AckPaymentandProcess(AVOAIALifeEntities Context, PaymentModel objPaymentModel, tblPolicy objtblpolicy)
        {
            SMSIntegration objSMSIntegration = new SMSIntegration();
            SMSDetails objSMSDetails = new SMSDetails();
            Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
            #region UW deviations
            UWRuleLogic objLogic = new UWRuleLogic();
            PolicyLogic objPolicyLogic = new PolicyLogic();
            objPaymentModel.Message = string.Empty;
            

            //bool dataUpdated = false;
            //var ilData = Context.tblLogILUpdates.Where(a => a.ProposalNo == objPaymentModel.ProposalNo).FirstOrDefault();
            //if (ilData != null)
            //{
            //    if (ilData.ServiceStatus == "SUCC" && ilData.ServiceName == "ModifyProposal")
            //        dataUpdated = true;
            //}
            //if (dataUpdated)
            //{
            //    //S003
            //    var CustomerDetails=objtblpolicy.tblPolicyRelationships.FirstOrDefault().tblPolicyClient;
            //    //var Title = Context.tblMasCommonTypes.Where(a => a.Code == CustomerDetails.Title).Select(a => a.ShortDesc).FirstOrDefault();
            //    //objSMSDetails.Salutation = objCommonBusiness.ConverttoTitlecase(Title);
            //    string Sal = CustomerDetails.Title;
            //    var Salutation = Context.tblMasCommonTypes.Where(a => a.Code == Sal && a.MasterType == "Salutation").Select(a => a.ShortDesc).FirstOrDefault();
            //    var Salu = Context.tblMasCommonTypes.Where(a => a.Description == Sal && a.MasterType == "Salutation").Select(a => a.ShortDesc).FirstOrDefault();
            //    if (!String.IsNullOrEmpty(Salutation))
            //    {
            //        objSMSDetails.Salutation = Salutation;
            //    }
            //    else if (!String.IsNullOrEmpty(Salu))
            //    {
            //        objSMSDetails.Salutation = Salu;
            //    }
            //    else
            //    {
            //        objSMSDetails.Salutation = Sal;
            //    }
            //    var Name = "";
            //    if (CustomerDetails.FullName == "CORP")
            //    {
            //        Name = CustomerDetails.CorporateName;
            //    }
            //    else
            //    {
            //        Name = CustomerDetails.LastName;
            //    }
            //    objSMSDetails.Name = objCommonBusiness.ConverttoTitlecase(Name);
            //    objSMSDetails.SMSTemplate = "S003";
            //    objSMSDetails.ProposalNumber = objPaymentModel.ProposalNo;
            //    objSMSDetails.MobileNumber = CustomerDetails.MobileNo;
            //    objSMSDetails.SMSEnvironment = Convert.ToString(ConfigurationManager.AppSettings["SMSEnvironment"]);
            //    if (!String.IsNullOrEmpty(objSMSDetails.MobileNumber))
            //    {
            //        objSMSIntegration.SMSNotification(objSMSDetails);
            //    }
            //  //  objPaymentModel = (PaymentModel)IL.RecieptEnquiry(objPaymentModel);
            //}
            //if ((objPaymentModel.PayingAmount) >= (Convert.ToDecimal(objPaymentModel.PayableAmount) - 100) || System.Web.Configuration.WebConfigurationManager.AppSettings["PublishEnvironment"] == "SIT")
            //{
            AIA.Life.Models.Policy.Policy objPolicy = new AIA.Life.Models.Policy.Policy();
            objPolicy.ProposalFetch = true;
            objPolicy.ProposalNo = objPaymentModel.ProposalNo;
            objPolicy.QuoteNo = objtblpolicy.QuoteNo;
            objPolicy = objPolicyLogic.FetchProposalInfo(objPolicy);
            // //S012
            //if (objPaymentModel.SelectedPayment == "othertypes")
            //{
            //    var createdBy = Context.tblPolicies.Where(a => a.ProposalNo == objPolicy.ProposalNo).Select(a => a.Createdby).FirstOrDefault();
            //    objSMSDetails.MobileNumber = Context.tblUserDetails.Where(a => a.UserID.ToString() == createdBy).Select(a => a.ContactNo).FirstOrDefault();
            //    //objSMSDetails.MobileNumber = Context.tblMasIMOUsers.Where(a => a.UserID == objPolicy.AgentCode).Select(a => a.MobileNo).FirstOrDefault();
            //    objSMSDetails.SMSTemplate = "S012";
            //    objSMSDetails.PolicyNo= objPaymentModel.ProposalNo;
            //    objSMSDetails.Premium = String.Format(CultureInfo.GetCultureInfo(1033), "{0:n0}", objPolicy.AnnualPremium); //Convert.ToString(objPolicy.AnnualPremium);
            //    objSMSDetails.SMSEnvironment = Convert.ToString(ConfigurationManager.AppSettings["SMSEnvironment"]);
            //    objSMSIntegration.SMSNotification(objSMSDetails);
            //}
            string Message = string.Empty;
            Message = objLogic.ValidateDeviation(objPolicy);

            //tblPayment tblPayment = objtblpolicy.tblPolicyPaymentMaps.OrderByDescending(a => a.PolicyPaymentMapID).FirstOrDefault().tblPayment;
            //tblPayment.ChequeSubmission = true;
            //tblPayment.ReceiptNo = "ACK";
            //string leadNo = Context.tblLifeQQs.Where(a => a.QuoteNo == objtblpolicy.QuoteNo).Select(a => a.tblContact).FirstOrDefault().LeadNo;
            // if (dataUpdated)
            // objPaymentModel = (PaymentModel)IL.ProposalPreIssueValidation(objPaymentModel);

            if (!string.IsNullOrEmpty(Message.Trim()) || objtblpolicy.PolicyStageStatusID == 2376)// Or Counter offer Case
            {
                objPaymentModel.Message = "Success";
                if (objtblpolicy != null)
                {
                    Message = "Your proposal has been forwarded to the Underwriter for further processing.";
                    objtblpolicy.PolicyRemarks = Message;
                    objtblpolicy.PolicyStageStatusID = 193;// UW
                    objtblpolicy.IsAllocated = false; // Pending for Allocation
                    objtblpolicy.ProposalSubmitDate = DateTime.Now;
                    //if (!string.IsNullOrEmpty(leadNo))
                    //{
                    //   // SamsClient samsClient = new SamsClient();
                    //   // samsClient.UpdateLeadStatus(Context, Convert.ToInt32(leadNo), 9);
                    //}
                    //SMS S005
                    var createdBy = Context.tblPolicies.Where(a => a.ProposalNo == objPolicy.ProposalNo).Select(a => a.Createdby).FirstOrDefault();
                    objSMSDetails.MobileNumber = Context.tblUserDetails.Where(a => a.UserID.ToString() == createdBy).Select(a => a.ContactNo).FirstOrDefault();
                    //objSMSDetails.WPMobileNumber= Context.tblMasIMOUsers.Where(a => a.UserID == objPolicy.UserName).Select(a => a.MobileNo).FirstOrDefault();
                    objSMSDetails.SMSTemplate = "S005";
                    objSMSDetails.PolicyNo = objPaymentModel.ProposalNo;
                    objSMSDetails.SMSEnvironment = Convert.ToString(ConfigurationManager.AppSettings["SMSEnvironment"]);
                    objSMSIntegration.SMSNotification(objSMSDetails);

                }
                objPaymentModel.UWMessage = "" + Message;
                Context.SaveChanges();

            }
            else
            {
                objtblpolicy.PolicyNo = objPaymentModel.ProposalNo = objPolicy.ProposalNo;
                objtblpolicy.PolicyStageStatusID = 192;// Issued
                DateTime today = DateTime.Now;
                int[] exceptionDays = new int[3] { 29, 30, 31 };
                if (exceptionDays.Contains(today.Day))
                    today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 28);
                objtblpolicy.PolicyIssueDate = today;
                objtblpolicy.PolicyStartDate = today;
                objtblpolicy.PolicyEndDate = today.AddYears(Convert.ToInt32(objtblpolicy.PolicyTerm));
                Context.SaveChanges();
                objPaymentModel.Message = "Success";
            }
            #endregion



            //if (!string.IsNullOrEmpty(objPaymentModel.ProposalNo) && dataUpdated)
            //{
            //    //objPaymentModel = (PaymentModel)IL.ProposalPreIssueValidation(objPaymentModel);
            //    if (string.IsNullOrEmpty(objPaymentModel.Error.ErrorMessage) && (objPaymentModel.PreIssueValidations.Count <= 1))
            //    {

            //        if (string.IsNullOrEmpty(objPaymentModel.Error.ErrorMessage))
            //        {
            //            Thread.Sleep(5000);
            //           // objPaymentModel = (PaymentModel)IL.ProposalUWApproval(objPaymentModel);
            //            if (string.IsNullOrEmpty(objPaymentModel.Error.ErrorMessage))
            //            {
            //                //objPaymentModel = (PaymentModel)IL.QualityControl(objPaymentModel);
            //                if (string.IsNullOrEmpty(objPaymentModel.Error.ErrorMessage))
            //                {
            //                    //objPaymentModel = (PaymentModel)IL.ProposalIssuance(objPaymentModel);
            //                    if (string.IsNullOrEmpty(objPaymentModel.Error.ErrorMessage))
            //                    {
            //                        objtblpolicy.PolicyNo = objPaymentModel.ProposalNo = objPolicy.ProposalNo;
            //                        objtblpolicy.PolicyStageStatusID = 192;// Issued
            //                        DateTime today = DateTime.Now;
            //                        int[] exceptionDays = new int[3] { 29, 30, 31 };
            //                        if (exceptionDays.Contains(today.Day))
            //                            today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 28);
            //                        objtblpolicy.PolicyIssueDate = today;
            //                        objtblpolicy.PolicyStartDate = today;
            //                        objtblpolicy.PolicyEndDate = today.AddYears(Convert.ToInt32(objtblpolicy.PolicyTerm));
            //                        objPaymentModel.Message = "Success";
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (dataUpdated)
            //        {
            //            if (!string.IsNullOrEmpty(leadNo))
            //            {
            //               // SamsClient samsClient = new SamsClient();
            //                //samsClient.UpdateLeadStatus(Context, Convert.ToInt32(leadNo), 9);
            //            }
            //            objtblpolicy.PolicyStageStatusID = 193;// UW
            //            objtblpolicy.IsAllocated = false;
            //            objtblpolicy.ProposalSubmitDate = DateTime.Now;
            //            objPaymentModel.UWMessage = "Your proposal has been forwarded to the underwriter for further processing.";
            //        }
            //        else
            //        {
            //            objPaymentModel.UWMessage = "Payment is Successful. Your proposal is under processing, you will be notified soon.";
            //        }
            //    }
            //    Context.SaveChanges();
            //}
            //}
            //else
            //{
            //    if(objPaymentModel.PayingAmount==0)
            //        objPaymentModel.UWMessage = "Payment is Successful. Your proposal is under processing, you will be notified soon.";
            //    else
            //        objPaymentModel.UWMessage = "Your payment is successful. Payment reference number is " + objPaymentModel.TransactionNo;
            //}
            return objPaymentModel;
        }

        private string GetTransactionNumber(string startchar)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            ObjectParameter nextNo = new ObjectParameter("NextNo", SqlDbType.Int);
            int genteratedNo = Context.usp_GetNextTxnNumber("TransactionNo", nextNo);
            Int64 seqNo = Convert.ToInt64(nextNo.Value);
            string value = seqNo.ToString("D9");
            string transactionNo = startchar + value;
            return transactionNo;
        }

        public PaymentModel FetchPendingPayments(PaymentModel objPaymentModel)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                var pendingReceipts = Context.usp_FetchPendingPaymentProposal().ToList();

                foreach (var item in pendingReceipts)
                {
                    PaymentModel paymentModel = new PaymentModel();
                    paymentModel.QuoteNo = item;
                    paymentModel.lstPaymentItems = (from objpolicy in Context.tblPolicies.Where(a => a.QuoteNo == paymentModel.QuoteNo)
                                                    join objtblpolicyrelationship in Context.tblPolicyRelationships on objpolicy.PolicyID equals objtblpolicyrelationship.PolicyID
                                                    join objtblpolicyclients in Context.tblPolicyClients on objtblpolicyrelationship.PolicyClientID equals objtblpolicyclients.PolicyClientID
                                                    join objProposalPayments in Context.tblProposalPremiums on objpolicy.PolicyID equals objProposalPayments.PolicyID
                                                    join objProduct in Context.tblProducts on objpolicy.ProductID equals objProduct.ProductId
                                                    //where objpolicy.PolicyStageStatusID == 1153 && objpolicy.QuoteNo == objPaymentModel.QuoteNo // Payment Pending
                                                    select new PaymentItems
                                                    {
                                                        ProposalNo = objpolicy.ProposalNo,
                                                        InsuredName = objtblpolicyclients.FirstName,
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
                                                        Mobile = objtblpolicyclients.MobileNo,
                                                        QuoteNo = objpolicy.QuoteNo,
                                                        ProductCode = objProduct.ProductCode,
                                                        PlanCode = Context.tblMasProductPlans.Where(pp => pp.PlanId == objpolicy.PlanID).FirstOrDefault().PlanCode,
                                                        IsAfc = objpolicy.IsAfc,
                                                        UserName = Context.tblUserDetails.Where(a => a.UserID.ToString() == objpolicy.Createdby).Select(a => a.LoginID).FirstOrDefault()
                                                    }).OrderByDescending(a => a.PolicyId).ToList();
                    objPaymentModel.lstPaymentItems.AddRange(paymentModel.lstPaymentItems);
                }
                return objPaymentModel;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PaymentModel CheckPaymentStatusUpdate(PaymentModel objPaymentModel)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                tblPolicy policy = Context.tblPolicies.Where(a => a.QuoteNo == objPaymentModel.QuoteNo).FirstOrDefault();
                if (policy != null)
                {
                    objPaymentModel = AckPaymentandProcess(Context, objPaymentModel, policy);
                }
                return objPaymentModel;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}