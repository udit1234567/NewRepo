using AIA.Life.Integration.Services.Policy;
using AIA.Life.Models.Common;
using AIA.Life.Models.Integration.Payment;
using AIA.Life.Models.Integration.Payment.Inforce;
using AIA.Life.Models.Integration.Payment.InGrace;
using AIA.Life.Models.Integration.Payment.LapsedPolicies;
using AIA.Life.Models.Integration.Payment.RenewableAgentClients;
using AIA.Life.Models.Integration.Payment.RenewedAgentClients;
using AIA.Life.Models.Integration.Payment.RunningLapse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Integration.Services.Payment
{
    public class PaymentIntegration
    {
        public static string GetPostParametersToAPI(string controllerName, string MethodName, string Url, object obj)
        {
            try
            {
                string _url = Url + controllerName + "/" + MethodName;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
                request.Method = "POST";
                request.ContentType = "application/json";
                string requestData = JsonConvert.SerializeObject(obj);
                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] data = encoder.GetBytes(requestData);
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return result;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        #region Renewal Summary

        public PaymentSummaryAPIResponse FetchRenewalSummaryInfo(PaymentServiceModel objPaymentModel)
        {
            PaymentSummaryAPIRequest objObject = new PaymentSummaryAPIRequest();
            objObject = MappingToRenewalSummary(objPaymentModel);
            string URl = "http://secure.AIA.com:8080/Life_Finance_Sending/jersey/";
            string result = GetPostParametersToAPI("Agent", "getRenevalSummary", URl, objObject);
            PaymentSummaryAPIResponse objRenewalResponse = new PaymentSummaryAPIResponse();
            Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
            objRenewalResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<PaymentSummaryAPIResponse>(result, settings);
            return objRenewalResponse;
        }

        public PaymentSummaryAPIRequest MappingToRenewalSummary(PaymentServiceModel objPaymentModel)
        {
            PaymentSummaryAPIRequest objRenewalSummary = new PaymentSummaryAPIRequest();
            objRenewalSummary.agentCode = "AGE000000040001";
            objRenewalSummary.companyCode = "00003";
            objRenewalSummary.fromDate = "01-11-2017";
            objRenewalSummary.toDate = "31-12-2018";
            objRenewalSummary.userId = "NayanajithG";
            return objRenewalSummary;
        }

        #endregion

        #region Renewed Client Policies

        public PaymentProposalSummaryAPIResponse FetchRenewedClientPoliciesInfo(PaymentServiceModel objPaymentModel)
        {
            PaymentProposalSummaryAPIRequest objObject = new PaymentProposalSummaryAPIRequest();
            objObject = MappingToRenewalClientServiceObject(objPaymentModel);
            string URl = "http://secure.AIA.com:8080/Lif_Ind_Rnwl_Sending/";
            string result = GetPostParametersToAPI("jersey", "renewedClientsPolicies", URl, objObject);
            PaymentProposalSummaryAPIResponse objRenewedPoliciesResponse = new PaymentProposalSummaryAPIResponse();
            Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
            objRenewedPoliciesResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<PaymentProposalSummaryAPIResponse>(result, settings);
            return objRenewedPoliciesResponse;
        }

        public PaymentProposalSummaryAPIRequest MappingToRenewalClientServiceObject(PaymentServiceModel objPaymentModel)
        {
            PaymentProposalSummaryAPIRequest objObject = new PaymentProposalSummaryAPIRequest();
            objObject.agentCode = "AGE000000004121";
            objObject.companyCode = "00003";
            //objObject.fromDate = "01-01-2014";
            //objObject.toDate = "31-12-2015";
            //objObject.fromDate = Convert.ToString(DateTime.Now.AddMonths(-10).Date);
            //objObject.toDate =Convert.ToString(DateTime.Now.Date);
            objObject.renewalFlag = "RENEWED";
            return objObject;
        }

        #endregion

        #region Renewed AgentsClients Policies
        public RenewedAgentsClientsInfoRespone FetchRenewedAgentsClientsInfo(PaymentServiceModel objPaymentModel)
        {
            RenewedAgentsClientsInfoRequest objObject = new RenewedAgentsClientsInfoRequest();
            objObject = MappingToRenewedAgentsClients(objPaymentModel);
            string URl = "http://secure.AIA.com:8080/Lif_Ind_Rnwl_Sending/";
            string result = GetPostParametersToAPI("jersey", "renewedAgentsClients", URl, objObject);
            RenewedAgentsClientsInfoRespone objRenewedAgentClientsResponse = new RenewedAgentsClientsInfoRespone();
            Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
            objRenewedAgentClientsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<RenewedAgentsClientsInfoRespone>(result, settings);
            return objRenewedAgentClientsResponse;
        }

        public RenewedAgentsClientsInfoRequest MappingToRenewedAgentsClients(PaymentServiceModel objPaymentModel)
        {
            RenewedAgentsClientsInfoRequest objRenewableAgentClinets = new RenewedAgentsClientsInfoRequest();
            objRenewableAgentClinets.agentCode = "AGE000000004121";
            objRenewableAgentClinets.companyCode = "00003";
            //objRenewableAgentClinets.fromDate = "01-01-2014";
            //objRenewableAgentClinets.toDate = "31-12-2014";
            objRenewableAgentClinets.proposerCode = "";
            return objRenewableAgentClinets;
        }
        #endregion

        #region Renewable AgentsClients Policies
        public RenewableAgentsClientsInfoResponse FetchRenewableAgentsClientsInfo(PaymentServiceModel objPaymentModel)
        {
            RenewableAgentsClientsInfoRequest objObject = new RenewableAgentsClientsInfoRequest();
            objObject = MappingToRenewableAgentsClients(objPaymentModel);
            string URl = "http://secure.AIA.com:8080/Lif_Ind_Rnwl_Sending/";
            string result = GetPostParametersToAPI("jersey", "renewableAgentsClients", URl, objObject);
            RenewableAgentsClientsInfoResponse objRenewableAgentsClientsResponse = new RenewableAgentsClientsInfoResponse();
            Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
            objRenewableAgentsClientsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<RenewableAgentsClientsInfoResponse>(result, settings);
            return objRenewableAgentsClientsResponse;
        }

        public RenewableAgentsClientsInfoRequest MappingToRenewableAgentsClients(PaymentServiceModel objPaymentModel)
        {
            RenewableAgentsClientsInfoRequest objRenewableAgentClinets = new RenewableAgentsClientsInfoRequest();
            objRenewableAgentClinets.agentCode = "AGE000000004121";
            objRenewableAgentClinets.companyCode = "00003";
            //objRenewableAgentClinets.fromDate = "01-01-2014";
            //objRenewableAgentClinets.toDate = "31-12-2015";
            objRenewableAgentClinets.proposerCode = "IM0000002595503";
            return objRenewableAgentClinets;
        }
        #endregion

        #region Inforce Policies
        public RenewedInforcePoliciesDetailsAPIResponse FetchRenewedInforcePoliciesInfo(PaymentServiceModel objPaymentModel)
        {
            RenewedInforcePoliciesDetailsAPIRequest objObject = new RenewedInforcePoliciesDetailsAPIRequest();
            objObject = MappingToRenewedInforceServiceObject(objPaymentModel);
            string URl = "http://secure.AIA.com:8080/Lif_Ind_Rnwl_Sending/";
            string result = GetPostParametersToAPI("jersey", "renewableAgentsClientsInforce", URl, objObject);
            RenewedInforcePoliciesDetailsAPIResponse objRenewedInforcePoliciesResponse = new RenewedInforcePoliciesDetailsAPIResponse();
            Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
            objRenewedInforcePoliciesResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<RenewedInforcePoliciesDetailsAPIResponse>(result, settings);
            return objRenewedInforcePoliciesResponse;
        }

        public RenewedInforcePoliciesDetailsAPIRequest MappingToRenewedInforceServiceObject(PaymentServiceModel objPaymentModel)
        {
            RenewedInforcePoliciesDetailsAPIRequest objInforceObject = new RenewedInforcePoliciesDetailsAPIRequest();
            objInforceObject.agentCode = "AGE000000004121";
            objInforceObject.companyCode = "00003";
            //objInforceObject.fromDate = "01-01-2014";
            //objInforceObject.toDate = "31-12-2015";
            objInforceObject.proposerCode = "IM0000002595503";
            return objInforceObject;
        }
        #endregion

        #region Policies InGrace Period
        public RenewedInGracePoliciesDetailsAPIResponse FetchPoliciesinGracePeriodInfo(PaymentServiceModel objPaymentModel)
        {
            RenewedInGracePoliciesDetailsAPIRequest objObject = new RenewedInGracePoliciesDetailsAPIRequest();
            objObject = MappingToRenewedInGraceServiceObject(objPaymentModel);
            string URl = "http://secure.AIA.com:8080/Lif_Ind_Rnwl_Sending/";
            string result = GetPostParametersToAPI("jersey", "renewableAgentsClientsGrace", URl, objObject);
            RenewedInGracePoliciesDetailsAPIResponse objRenewedInGracePoliciesResponse = new RenewedInGracePoliciesDetailsAPIResponse();
            Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
            objRenewedInGracePoliciesResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<RenewedInGracePoliciesDetailsAPIResponse>(result, settings);
            return objRenewedInGracePoliciesResponse;
        }

        public RenewedInGracePoliciesDetailsAPIRequest MappingToRenewedInGraceServiceObject(PaymentServiceModel objPaymentModel)
        {
            RenewedInGracePoliciesDetailsAPIRequest objInGraceObject = new RenewedInGracePoliciesDetailsAPIRequest();
            objInGraceObject.agentCode = "AGE000000004121";
            objInGraceObject.companyCode = "00003";
            //objInGraceObject.fromDate = "01-01-2014";
            //objInGraceObject.toDate = "31-12-2015";
            objInGraceObject.proposerCode = "IM0000002595503";
            return objInGraceObject;
        }
        #endregion

        #region Running Lapse Policies
        public RenewedRunningLapsePoliciesDetailsAPIResponse FetchRunningLapsePoliciesInfo(PaymentServiceModel objPaymentModel)
        {
            RenewedRunningLapsePoliciesDetailsAPIRequest objObject = new RenewedRunningLapsePoliciesDetailsAPIRequest();
            objObject = MappingToRenewedRunningLapseServiceObject(objPaymentModel);
            string URl = " http://secure.AIA.com:8080/Lif_Ind_Rnwl_Sending/";
            string result = GetPostParametersToAPI("jersey", "renewableAgentsClientsRunningLapse", URl, objObject);
            RenewedRunningLapsePoliciesDetailsAPIResponse objRenewedRunningLapsePoliciesResponse = new RenewedRunningLapsePoliciesDetailsAPIResponse();
            Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
            objRenewedRunningLapsePoliciesResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<RenewedRunningLapsePoliciesDetailsAPIResponse>(result, settings);
            return objRenewedRunningLapsePoliciesResponse;
        }

        public RenewedRunningLapsePoliciesDetailsAPIRequest MappingToRenewedRunningLapseServiceObject(PaymentServiceModel objPaymentModel)
        {
            RenewedRunningLapsePoliciesDetailsAPIRequest objInGraceObject = new RenewedRunningLapsePoliciesDetailsAPIRequest();
            objInGraceObject.agentCode = "AGE000000051387";
            objInGraceObject.companyCode = "00003";
            //objInGraceObject.fromDate = "01-01-2015";
            //objInGraceObject.toDate = "31-12-2017";
            objInGraceObject.proposerCode = "ID0000002236869";
            return objInGraceObject;
        }
        #endregion

        #region Lapsed Policies Info
        public RenewedLapsedPoliciesDetailsAPIResponse FetchRenewedlapsedPoliciesInfo(PaymentServiceModel objPaymentModel)
        {
            RenewedLapsedPoliciesDetailsAPIRequest objObject = new RenewedLapsedPoliciesDetailsAPIRequest();
            objObject = MappingToRenewedlapsedServiceObject(objPaymentModel);
            string URl = "http://secure.AIA.com:8080/Lif_Ind_Rnwl_Sending/";
            string result = GetPostParametersToAPI("jersey", "renewableAgentsClientsLapsed", URl, objObject);
            RenewedLapsedPoliciesDetailsAPIResponse objRenewedlapsedPoliciesResponse = new RenewedLapsedPoliciesDetailsAPIResponse();
            Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
            objRenewedlapsedPoliciesResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<RenewedLapsedPoliciesDetailsAPIResponse>(result, settings);
            return objRenewedlapsedPoliciesResponse;
        }

        public RenewedLapsedPoliciesDetailsAPIRequest MappingToRenewedlapsedServiceObject(PaymentServiceModel objPaymentModel)
        {
            RenewedLapsedPoliciesDetailsAPIRequest objInGraceObject = new RenewedLapsedPoliciesDetailsAPIRequest();
            objInGraceObject.agentCode = "AGE000000004121";
            objInGraceObject.companyCode = "00003";
            //objInGraceObject.fromDate = "01-01-2014";
            //objInGraceObject.toDate = "31-12-2017";
            objInGraceObject.proposerCode = "IN0000001311079";
            return objInGraceObject;
        }
        #endregion

        public PaymentServiceModel FetchPaymentProposals(PaymentServiceModel objPaymentModel)
        {
            List<PaymentProposal> ProposalPayments = new List<PaymentProposal>();
            ProposalPayments.Add(new PaymentProposal { PaymentProposalNumber = "1", PaymentProposalFirstName = "Rajeev", PaymentProposalLastName = "Gandhi", PaymentProposalActualPremiumAmt = "10000", PaymentProposalAmountPayable = "10000", PaymentProposalDepositPaid = "100000", PaymentProposalDaysLeft = "3" });
            ProposalPayments.Add(new PaymentProposal { PaymentProposalNumber = "2", PaymentProposalFirstName = "Rahul", PaymentProposalLastName = "Gandhi", PaymentProposalActualPremiumAmt = "20000", PaymentProposalAmountPayable = "20000", PaymentProposalDepositPaid = "200000", PaymentProposalDaysLeft = "3" });
            ProposalPayments.Add(new PaymentProposal { PaymentProposalNumber = "3", PaymentProposalFirstName = "Shekar", PaymentProposalLastName = "Kaur", PaymentProposalActualPremiumAmt = "30000", PaymentProposalAmountPayable = "30000", PaymentProposalDepositPaid = "300000", PaymentProposalDaysLeft = "3" });
            ProposalPayments.Add(new PaymentProposal { PaymentProposalNumber = "4", PaymentProposalFirstName = "Shekar", PaymentProposalLastName = "Kaur", PaymentProposalActualPremiumAmt = "30000", PaymentProposalAmountPayable = "30000", PaymentProposalDepositPaid = "300000", PaymentProposalDaysLeft = "3" });
            ProposalPayments.Add(new PaymentProposal { PaymentProposalNumber = "5", PaymentProposalFirstName = "Shekar", PaymentProposalLastName = "Kaur", PaymentProposalActualPremiumAmt = "30000", PaymentProposalAmountPayable = "30000", PaymentProposalDepositPaid = "300000", PaymentProposalDaysLeft = "3" });
            ProposalPayments.Add(new PaymentProposal { PaymentProposalNumber = "6", PaymentProposalFirstName = "Shekar", PaymentProposalLastName = "Kaur", PaymentProposalActualPremiumAmt = "30000", PaymentProposalAmountPayable = "30000", PaymentProposalDepositPaid = "300000", PaymentProposalDaysLeft = "3" });
            ProposalPayments.Add(new PaymentProposal { PaymentProposalNumber = "7", PaymentProposalFirstName = "Shekar", PaymentProposalLastName = "Kaur", PaymentProposalActualPremiumAmt = "30000", PaymentProposalAmountPayable = "30000", PaymentProposalDepositPaid = "300000", PaymentProposalDaysLeft = "3" });
            ProposalPayments.Add(new PaymentProposal { PaymentProposalNumber = "8", PaymentProposalFirstName = "Shekar", PaymentProposalLastName = "Kaur", PaymentProposalActualPremiumAmt = "30000", PaymentProposalAmountPayable = "30000", PaymentProposalDepositPaid = "300000", PaymentProposalDaysLeft = "3" });
            objPaymentModel.ObjPaymentProposalPool = ProposalPayments;
            return objPaymentModel;
        }
        public PaymentServiceModel FetchRenewalProposals(PaymentServiceModel objPaymentModel)
        {
            //FetchRenewableAgentsClientsInfo(objPaymentModel);
            //FetchRenewedAgentsClientsInfo(objPaymentModel);
            List<PaymentRenewal> ObjlstPaymentRenewalPool = new List<PaymentRenewal>();
            //ObjlstPaymentRenewalPool.Add(new PaymentRenewal { PaymentRenewalPolicies = "Count", PaymentCorrespondingDeposit = "Count", PaymentRenewedPolicies = "Count" });
            PaymentSummaryAPIResponse objResponse = FetchRenewalSummaryInfo(objPaymentModel);
            if (objResponse.status == "Success")
            {
                PaymentRenewal objPaymentRenewal = new PaymentRenewal();
                objPaymentRenewal.PaymentRenewalCountPolicies = objResponse.agentRenevalSummary.renewableCount;
                objPaymentRenewal.PaymentRenewalPolicies = objResponse.agentRenevalSummary.renewableValue;
                objPaymentRenewal.PaymentRenewedCountPolicies = objResponse.agentRenevalSummary.renewedCount;
                objPaymentRenewal.PaymentRenewedPolicies = objResponse.agentRenevalSummary.renewedValue;
                objPaymentRenewal.PaymentCorrespondingDeposit = objResponse.agentRenevalSummary.depositBalance;
                ObjlstPaymentRenewalPool.Add(objPaymentRenewal);
            }
            objPaymentModel.ObjPaymentRenewalPool = ObjlstPaymentRenewalPool;
            return objPaymentModel;
        }
        public PaymentServiceModel FetchRenewedPolicies(PaymentServiceModel objPaymentModel)
        {
            List<RenewedPolicies> Renewedpolicies = new List<RenewedPolicies>();
            PaymentProposalSummaryAPIResponse objResponse = FetchRenewedClientPoliciesInfo(objPaymentModel);
            if (objResponse.Status == "Success")
            {
                foreach (var item in objResponse.output)
                {
                    RenewedPolicies objclientInfo = new RenewedPolicies();
                    objclientInfo.PaymentRenewedFirstName = item.firstName;
                    objclientInfo.PaymentRenewedLastName = item.lastName;
                    objclientInfo.PaymentRenewedMobile = item.mobileTelNo;
                    objclientInfo.PaymentRenewedHome = item.homeTelNo;
                    objclientInfo.PaymentRenewedWork = item.workTelNo;
                    objclientInfo.PaymentRenewedStarClassification = "4";
                    Renewedpolicies.Add(objclientInfo);
                }
            }
            objPaymentModel.ObjPaymentRenewedPoliciesPool = Renewedpolicies;
            return objPaymentModel;
        }
        public PaymentServiceModel FetchRenewedAllPolicies(PaymentServiceModel objPaymentModel)
        {
            List<RenewedAllPolicies> RenewedPolicies = new List<RenewedAllPolicies>();
            RenewedPolicies.Add(new RenewedAllPolicies { PolicyNumber = 1, FirstName = "Kiranmai", DepositAmount = "100000", LastPaymentDate = "20/11/2017", NoOfUnpaidPremiums = "3", PremiumAmount = "300000", Status = "Running", PremiumDueDate = "30/12/2018", TotalAmounttobePaid = "500000", TotalArrears = "3", DaysLeftForLapse = "3" });
            RenewedPolicies.Add(new RenewedAllPolicies { PolicyNumber = 2, FirstName = "Kranthi", DepositAmount = "100000", LastPaymentDate = "02/01/2017", NoOfUnpaidPremiums = "3", PremiumAmount = "370000", Status = "Lapsed", PremiumDueDate = "30/12/2018", TotalAmounttobePaid = "500000", TotalArrears = "3", DaysLeftForLapse = "3" });
            RenewedPolicies.Add(new RenewedAllPolicies { PolicyNumber = 3, FirstName = "Varma", DepositAmount = "100000", LastPaymentDate = "20/10/2017", NoOfUnpaidPremiums = "3", PremiumAmount = "350000", Status = "Inforced", PremiumDueDate = "30/12/2018", TotalAmounttobePaid = "500000", TotalArrears = "3", DaysLeftForLapse = "3" });
            RenewedPolicies.Add(new RenewedAllPolicies { PolicyNumber = 3, FirstName = "Varma", DepositAmount = "100000", LastPaymentDate = "20/10/2017", NoOfUnpaidPremiums = "3", PremiumAmount = "350000", Status = "Inforced", PremiumDueDate = "30/12/2018", TotalAmounttobePaid = "500000", TotalArrears = "3", DaysLeftForLapse = "3" });
            RenewedPolicies.Add(new RenewedAllPolicies { PolicyNumber = 3, FirstName = "Varma", DepositAmount = "100000", LastPaymentDate = "20/10/2017", NoOfUnpaidPremiums = "3", PremiumAmount = "350000", Status = "Inforced", PremiumDueDate = "30/12/2018", TotalAmounttobePaid = "500000", TotalArrears = "3", DaysLeftForLapse = "3" });
            RenewedPolicies.Add(new RenewedAllPolicies { PolicyNumber = 3, FirstName = "Varma", DepositAmount = "100000", LastPaymentDate = "20/10/2017", NoOfUnpaidPremiums = "3", PremiumAmount = "350000", Status = "Inforced", PremiumDueDate = "30/12/2018", TotalAmounttobePaid = "500000", TotalArrears = "3", DaysLeftForLapse = "3" });
            RenewedPolicies.Add(new RenewedAllPolicies { PolicyNumber = 3, FirstName = "Varma", DepositAmount = "100000", LastPaymentDate = "20/10/2017", NoOfUnpaidPremiums = "3", PremiumAmount = "350000", Status = "Inforced", PremiumDueDate = "30/12/2018", TotalAmounttobePaid = "500000", TotalArrears = "3", DaysLeftForLapse = "3" });
            RenewedPolicies.Add(new RenewedAllPolicies { PolicyNumber = 3, FirstName = "Varma", DepositAmount = "100000", LastPaymentDate = "20/10/2017", NoOfUnpaidPremiums = "3", PremiumAmount = "350000", Status = "Inforced", PremiumDueDate = "30/12/2018", TotalAmounttobePaid = "500000", TotalArrears = "3", DaysLeftForLapse = "3" });
            objPaymentModel.ObjPaymentRenewedAllPolicies = RenewedPolicies;

            #region Inforced Policies
            List<RenewedInforcePolicies> RenewedInforcedPolicies = new List<RenewedInforcePolicies>();
            RenewedInforcePoliciesDetailsAPIResponse objResponse = FetchRenewedInforcePoliciesInfo(objPaymentModel);
            if (objResponse.Status == "Success")
            {
                foreach (var item in objResponse.output)
                {
                    int i = 1;
                    RenewedInforcePolicies objclientInfo = new RenewedInforcePolicies();
                    objclientInfo.FirstName = item.firstName;
                    objclientInfo.PolicyNumber = item.policyNo;
                    objclientInfo.PolicyId = i;
                    objclientInfo.PremiumAmount = item.premiumAmount;
                    objclientInfo.PremiumDueDate = item.premiumDueDate;
                    objclientInfo.TotalAmounttobePaid = item.totalAmountToBePaid;
                    objclientInfo.DepositAmount = item.depositAmount;
                    RenewedInforcedPolicies.Add(objclientInfo);
                    i++;
                }
            }
            objPaymentModel.ObjPaymentRenewedInforcePolicies = RenewedInforcedPolicies;
            #endregion

            #region Policies InGracePeriod
            List<RenewedPoliciesinGracePeriod> PoliciesinGracePeriod = new List<RenewedPoliciesinGracePeriod>();
            RenewedInGracePoliciesDetailsAPIResponse objInGraceResponse = FetchPoliciesinGracePeriodInfo(objPaymentModel);
            if (objInGraceResponse.Status == "Success")
            {
                foreach (var item in objInGraceResponse.output)
                {
                    int i = 1;
                    RenewedPoliciesinGracePeriod objclientInfo = new RenewedPoliciesinGracePeriod();
                    objclientInfo.FirstName = item.firstName;
                    objclientInfo.PolicyNumber = item.policyNo;
                    objclientInfo.PolicyId = i;
                    objclientInfo.PremiumAmount = item.premiumAmount;
                    objclientInfo.PremiumDueDate = item.premiumDueDate;
                    objclientInfo.TotalAmounttobePaid = item.totalAmountToBePaid;
                    objclientInfo.DepositAmount = item.depositAmount;
                    objclientInfo.DaysLeftForRunningLapse = item.daysLeftforLaps;
                    objclientInfo.LastPaymentDate = item.lastPaymentDate;
                    PoliciesinGracePeriod.Add(objclientInfo);
                    i++;
                }
            }
            objPaymentModel.ObjPaymentRenewedPoliciesinGracePeriod = PoliciesinGracePeriod;
            #endregion

            #region Running Lapse Policies
            List<RenewedRunningLapsePolicies> RunningLapsePolicies = new List<RenewedRunningLapsePolicies>();
            RenewedRunningLapsePoliciesDetailsAPIResponse objRunningLapseResponse = FetchRunningLapsePoliciesInfo(objPaymentModel);
            if (objRunningLapseResponse.Status == "Success")
            {
                foreach (var item in objRunningLapseResponse.output)
                {
                    int i = 1;
                    RenewedRunningLapsePolicies objRunningLapseInfo = new RenewedRunningLapsePolicies();
                    objRunningLapseInfo.FirstName = item.firstName;
                    objRunningLapseInfo.PolicyNumber = item.policyNo;
                    objRunningLapseInfo.PolicyId = i;
                    objRunningLapseInfo.PremiumAmount = item.premiumAmount;
                    objRunningLapseInfo.PremiumDueDate = item.premiumDueDate;
                    objRunningLapseInfo.TotalAmounttobePaid = item.totalAmountToBePaid;
                    objRunningLapseInfo.DepositAmount = item.depositAmount;
                    objRunningLapseInfo.DaysLeftForLapse = item.daysLeftforLaps;
                    objRunningLapseInfo.LastPaymentDate = item.lastPaymentDate;
                    objRunningLapseInfo.NoOfUnpaidPremiums = item.noOfunpaidPremiums;
                    objRunningLapseInfo.TotalArrears = item.totalAreas;
                    objRunningLapseInfo.TotalLateFees = item.totalLateFees;
                    RunningLapsePolicies.Add(objRunningLapseInfo);
                    i++;
                }
            }

            objPaymentModel.ObjPaymentRenewedRunningLapsePolicies = RunningLapsePolicies;
            #endregion

            #region Lapsed Policies
            List<RenewedLapsedPolicies> lstRenewedlapsedPolicies = new List<RenewedLapsedPolicies>();
            RenewedLapsedPoliciesDetailsAPIResponse objlapsedResponse = FetchRenewedlapsedPoliciesInfo(objPaymentModel);
            if (objlapsedResponse.Status == "Success")
            {
                foreach (var item in objlapsedResponse.output)
                {
                    int i = 1;
                    RenewedLapsedPolicies objlapsedInfo = new RenewedLapsedPolicies();
                    objlapsedInfo.FirstName = item.firstName;
                    objlapsedInfo.PolicyNumber = item.policyNo;
                    objlapsedInfo.PolicyId = i;
                    objlapsedInfo.PremiumAmount = item.premiumAmount;
                    objlapsedInfo.PremiumDueDate = item.premiumDueDate;
                    objlapsedInfo.TotalAmounttobePaid = item.totalAmountToBePaid;
                    objlapsedInfo.DepositAmount = item.depositAmount;
                    objlapsedInfo.NoOfUnpaidPremiums = item.noOfunpaidPremiums;
                    objlapsedInfo.LastPaymentDate = item.lastPaymentDate;
                    objlapsedInfo.LapsedOn = item.lapsedOn;
                    objlapsedInfo.TotalArrears = item.totalAreas;
                    objlapsedInfo.TotalLateFees = item.totalLateFees;
                    lstRenewedlapsedPolicies.Add(objlapsedInfo);
                    i++;
                }
            }
            objPaymentModel.ObjPaymentRenewedLapsedPolicies = lstRenewedlapsedPolicies;
            #endregion

            if (objPaymentModel.ObjPaymentRenewedAllPolicies == null)
            {
                objPaymentModel.ObjPaymentRenewedAllPolicies = new List<RenewedAllPolicies>();
            }
            if (objPaymentModel.ObjPaymentRenewedInforcePolicies == null)
            {
                objPaymentModel.ObjPaymentRenewedInforcePolicies = new List<RenewedInforcePolicies>();
            }
            if (objPaymentModel.ObjPaymentRenewedPoliciesinGracePeriod == null)
            {
                objPaymentModel.ObjPaymentRenewedPoliciesinGracePeriod = new List<RenewedPoliciesinGracePeriod>();
            }
            if (objPaymentModel.ObjPaymentRenewedRunningLapsePolicies == null)
            {
                objPaymentModel.ObjPaymentRenewedRunningLapsePolicies = new List<RenewedRunningLapsePolicies>();
            }
            if (objPaymentModel.ObjPaymentRenewedLapsedPolicies == null)
            {
                objPaymentModel.ObjPaymentRenewedLapsedPolicies = new List<RenewedLapsedPolicies>();
            }

            return objPaymentModel;
        }
        public PaymentServiceModel FetchPolicyHolderDetails(PaymentServiceModel objPaymentModel)
        {
            //Common.CommonBusiness objCommonBusiness = new Common.CommonBusiness();
            List<PolicyHolderRenewedPolicies> PolicyHolderRenewedPolicies = new List<PolicyHolderRenewedPolicies>();
            objPaymentModel.FirstName = "Zeeshan";
            objPaymentModel.MiddleName = "Mizra";
            objPaymentModel.LastName = "Hyder";
            objPaymentModel.Email = "Zeeshan@inubesolutions.com";
            //objPaymentModel.lstSalutation = objCommonBusiness.GetSalutation();
            objPaymentModel.Mobile = "9876465456";
            objPaymentModel.Home = "5654674656";
            objPaymentModel.Work = "08954455678";
            PolicyHolderRenewedPolicies.Add(new PolicyHolderRenewedPolicies { PolicyHolderPolicyNo = "PN176", PolicyHolderDepositAmount = 200000, PolicyHolderLastPaymentDate = DateTime.Now, PolicyHolderNextPremiumDueDate = DateTime.Now, PolicyHolderPremiumAmount = 250000 });
            objPaymentModel.ObjPolicyHolderRenewedPolicies = PolicyHolderRenewedPolicies;

            List<RenewedAllPolicies> RenewedPolicies = new List<RenewedAllPolicies>();
            RenewedPolicies.Add(new RenewedAllPolicies { PolicyNumber = 1, FirstName = "Kiranmai", DepositAmount = "100000", LastPaymentDate = "20/11/2017", NoOfUnpaidPremiums = "3", PremiumAmount = "300000", Status = "Running", PremiumDueDate = "30/12/2018", TotalAmounttobePaid = "500000", TotalArrears = "3", DaysLeftForLapse = "3" });
            RenewedPolicies.Add(new RenewedAllPolicies { PolicyNumber = 2, FirstName = "Kranthi", DepositAmount = "100000", LastPaymentDate = "02/01/2017", NoOfUnpaidPremiums = "3", PremiumAmount = "370000", Status = "Lapsed", PremiumDueDate = "30/12/2018", TotalAmounttobePaid = "500000", TotalArrears = "3", DaysLeftForLapse = "3" });
            RenewedPolicies.Add(new RenewedAllPolicies { PolicyNumber = 3, FirstName = "Varma", DepositAmount = "100000", LastPaymentDate = "20/10/2017", NoOfUnpaidPremiums = "3", PremiumAmount = "350000", Status = "Inforced", PremiumDueDate = "30/12/2018", TotalAmounttobePaid = "500000", TotalArrears = "3", DaysLeftForLapse = "3" });
            RenewedPolicies.Add(new RenewedAllPolicies { PolicyNumber = 1, FirstName = "Kiranmai", DepositAmount = "100000", LastPaymentDate = "20/11/2017", NoOfUnpaidPremiums = "3", PremiumAmount = "300000", Status = "Running", PremiumDueDate = "30/12/2018", TotalAmounttobePaid = "500000", TotalArrears = "3", DaysLeftForLapse = "3" });
            RenewedPolicies.Add(new RenewedAllPolicies { PolicyNumber = 2, FirstName = "Kranthi", DepositAmount = "100000", LastPaymentDate = "02/01/2017", NoOfUnpaidPremiums = "3", PremiumAmount = "370000", Status = "Lapsed", PremiumDueDate = "30/12/2018", TotalAmounttobePaid = "500000", TotalArrears = "3", DaysLeftForLapse = "3" });
            RenewedPolicies.Add(new RenewedAllPolicies { PolicyNumber = 3, FirstName = "Varma", DepositAmount = "100000", LastPaymentDate = "20/10/2017", NoOfUnpaidPremiums = "3", PremiumAmount = "350000", Status = "Inforced", PremiumDueDate = "30/12/2018", TotalAmounttobePaid = "500000", TotalArrears = "3", DaysLeftForLapse = "3" });
            RenewedPolicies.Add(new RenewedAllPolicies { PolicyNumber = 1, FirstName = "Kiranmai", DepositAmount = "100000", LastPaymentDate = "20/11/2017", NoOfUnpaidPremiums = "3", PremiumAmount = "300000", Status = "Running", PremiumDueDate = "30/12/2018", TotalAmounttobePaid = "500000", TotalArrears = "3", DaysLeftForLapse = "3" });
            RenewedPolicies.Add(new RenewedAllPolicies { PolicyNumber = 2, FirstName = "Kranthi", DepositAmount = "100000", LastPaymentDate = "02/01/2017", NoOfUnpaidPremiums = "3", PremiumAmount = "370000", Status = "Lapsed", PremiumDueDate = "30/12/2018", TotalAmounttobePaid = "500000", TotalArrears = "3", DaysLeftForLapse = "3" });
            RenewedPolicies.Add(new RenewedAllPolicies { PolicyNumber = 3, FirstName = "Varma", DepositAmount = "100000", LastPaymentDate = "20/10/2017", NoOfUnpaidPremiums = "3", PremiumAmount = "350000", Status = "Inforced", PremiumDueDate = "30/12/2018", TotalAmounttobePaid = "500000", TotalArrears = "3", DaysLeftForLapse = "3" });
            objPaymentModel.ObjPaymentRenewedAllPolicies = RenewedPolicies;


            #region Inforced Policies
            List<RenewedInforcePolicies> RenewedInforcedPolicies = new List<RenewedInforcePolicies>();
            RenewedInforcePoliciesDetailsAPIResponse objResponse = FetchRenewedInforcePoliciesInfo(objPaymentModel);
            if (objResponse.Status == "Success")
            {
                foreach (var item in objResponse.output)
                {
                    int i = 1;
                    RenewedInforcePolicies objclientInfo = new RenewedInforcePolicies();
                    objclientInfo.FirstName = item.firstName;
                    objclientInfo.PolicyNumber = item.policyNo;
                    objclientInfo.PolicyId = i;
                    objclientInfo.PremiumAmount = item.premiumAmount;
                    objclientInfo.PremiumDueDate = item.premiumDueDate;
                    objclientInfo.TotalAmounttobePaid = item.totalAmountToBePaid;
                    objclientInfo.DepositAmount = item.depositAmount;
                    RenewedInforcedPolicies.Add(objclientInfo);
                    i++;
                }
            }
            objPaymentModel.ObjPaymentRenewedInforcePolicies = RenewedInforcedPolicies;
            #endregion

            #region Policies InGracePeriod
            List<RenewedPoliciesinGracePeriod> PoliciesinGracePeriod = new List<RenewedPoliciesinGracePeriod>();
            RenewedInGracePoliciesDetailsAPIResponse objInGraceResponse = FetchPoliciesinGracePeriodInfo(objPaymentModel);
            if (objInGraceResponse.Status == "Success")
            {
                foreach (var item in objInGraceResponse.output)
                {
                    int i = 1;
                    RenewedPoliciesinGracePeriod objclientInfo = new RenewedPoliciesinGracePeriod();
                    objclientInfo.FirstName = item.firstName;
                    objclientInfo.PolicyNumber = item.policyNo;
                    objclientInfo.PolicyId = i;
                    objclientInfo.PremiumAmount = item.premiumAmount;
                    objclientInfo.PremiumDueDate = item.premiumDueDate;
                    objclientInfo.TotalAmounttobePaid = item.totalAmountToBePaid;
                    objclientInfo.DepositAmount = item.depositAmount;
                    objclientInfo.DaysLeftForRunningLapse = item.daysLeftforLaps;
                    objclientInfo.LastPaymentDate = item.lastPaymentDate;
                    PoliciesinGracePeriod.Add(objclientInfo);
                    i++;
                }
            }
            objPaymentModel.ObjPaymentRenewedPoliciesinGracePeriod = PoliciesinGracePeriod;
            #endregion

            #region Running Lapse Policies
            List<RenewedRunningLapsePolicies> RunningLapsePolicies = new List<RenewedRunningLapsePolicies>();
            RenewedRunningLapsePoliciesDetailsAPIResponse objRunningLapseResponse = FetchRunningLapsePoliciesInfo(objPaymentModel);
            if (objRunningLapseResponse.Status == "Success")
            {
                foreach (var item in objRunningLapseResponse.output)
                {
                    int i = 1;
                    RenewedRunningLapsePolicies objRunningLapseInfo = new RenewedRunningLapsePolicies();
                    objRunningLapseInfo.FirstName = item.firstName;
                    objRunningLapseInfo.PolicyNumber = item.policyNo;
                    objRunningLapseInfo.PolicyId = i;
                    objRunningLapseInfo.PremiumAmount = item.premiumAmount;
                    objRunningLapseInfo.PremiumDueDate = item.premiumDueDate;
                    objRunningLapseInfo.TotalAmounttobePaid = item.totalAmountToBePaid;
                    objRunningLapseInfo.DepositAmount = item.depositAmount;
                    objRunningLapseInfo.DaysLeftForLapse = item.daysLeftforLaps;
                    objRunningLapseInfo.LastPaymentDate = item.lastPaymentDate;
                    objRunningLapseInfo.NoOfUnpaidPremiums = item.noOfunpaidPremiums;
                    objRunningLapseInfo.TotalArrears = item.totalAreas;
                    objRunningLapseInfo.TotalLateFees = item.totalLateFees;
                    RunningLapsePolicies.Add(objRunningLapseInfo);
                    i++;
                }
            }

            objPaymentModel.ObjPaymentRenewedRunningLapsePolicies = RunningLapsePolicies;
            #endregion

            #region Lapsed Policies
            List<RenewedLapsedPolicies> lstRenewedlapsedPolicies = new List<RenewedLapsedPolicies>();
            RenewedLapsedPoliciesDetailsAPIResponse objlapsedResponse = FetchRenewedlapsedPoliciesInfo(objPaymentModel);
            if (objlapsedResponse.Status == "Success")
            {
                foreach (var item in objlapsedResponse.output)
                {
                    int i = 1;
                    RenewedLapsedPolicies objlapsedInfo = new RenewedLapsedPolicies();
                    objlapsedInfo.FirstName = item.firstName;
                    objlapsedInfo.PolicyNumber = item.policyNo;
                    objlapsedInfo.PolicyId = i;
                    objlapsedInfo.PremiumAmount = item.premiumAmount;
                    objlapsedInfo.PremiumDueDate = item.premiumDueDate;
                    objlapsedInfo.TotalAmounttobePaid = item.totalAmountToBePaid;
                    objlapsedInfo.DepositAmount = item.depositAmount;
                    objlapsedInfo.NoOfUnpaidPremiums = item.noOfunpaidPremiums;
                    objlapsedInfo.LastPaymentDate = item.lastPaymentDate;
                    objlapsedInfo.LapsedOn = item.lapsedOn;
                    objlapsedInfo.TotalArrears = item.totalAreas;
                    objlapsedInfo.TotalLateFees = item.totalLateFees;
                    lstRenewedlapsedPolicies.Add(objlapsedInfo);
                    i++;
                }
            }
            objPaymentModel.ObjPaymentRenewedLapsedPolicies = lstRenewedlapsedPolicies;
            #endregion

            if (objPaymentModel.ObjPaymentRenewedAllPolicies == null)
            {
                objPaymentModel.ObjPaymentRenewedAllPolicies = new List<RenewedAllPolicies>();
            }
            if (objPaymentModel.ObjPaymentRenewedInforcePolicies == null)
            {
                objPaymentModel.ObjPaymentRenewedInforcePolicies = new List<RenewedInforcePolicies>();
            }
            if (objPaymentModel.ObjPaymentRenewedPoliciesinGracePeriod == null)
            {
                objPaymentModel.ObjPaymentRenewedPoliciesinGracePeriod = new List<RenewedPoliciesinGracePeriod>();
            }
            if (objPaymentModel.ObjPaymentRenewedRunningLapsePolicies == null)
            {
                objPaymentModel.ObjPaymentRenewedRunningLapsePolicies = new List<RenewedRunningLapsePolicies>();
            }
            if (objPaymentModel.ObjPaymentRenewedLapsedPolicies == null)
            {
                objPaymentModel.ObjPaymentRenewedLapsedPolicies = new List<RenewedLapsedPolicies>();
            }
            return objPaymentModel;
        }



        public string PushReceiptInfoToCore(AIA.Life.Models.Policy.Policy objpolicy)
        {
            try
            {
                ReceiptRequest objReceiptRequest = new ReceiptRequest();
                PolicyIntegration objpolicyIntegration = new PolicyIntegration();
              Agent objAgentInfo=  objpolicyIntegration.FetchAgentBranch(objpolicy.UserName);
                objReceiptRequest.userId = objpolicy.UserName;
                 // objReceiptRequest.userId = "TEST";  // test
                foreach (var Instrument in objpolicy.objPaymentInfo.objInstrumentDetails)
                {
                    WsTemporaryProposalReceiptHdr objhdr = new WsTemporaryProposalReceiptHdr();
                    if (objAgentInfo != null)
                    {
                        objhdr.branchCode = objAgentInfo.BranchCode;  // test
                        objhdr.agentCode = objAgentInfo.AgentCode; // test
                    }
                    else
                    {
                        objhdr.branchCode = "SIF";  // test
                        objhdr.agentCode = "AGE000000000158"; // test
                    }
                    
                    objhdr.policyNo = string.Empty;
                    objhdr.modeOfPayment = "Q";
                    objhdr.proposalNo = objpolicy.ProposalNo.ToUpper();
                    objhdr.tempReceiptNo = Convert.ToString(Instrument.InstumentID);
                    objhdr.totalAmount = Convert.ToString(Instrument.InstrumentAmount);

                    objhdr.createBy = objpolicy.UserName.ToUpper();
                   // objhdr.createBy = "TEST";  //test

                    objhdr.createDt =DateTime.Now.Date.ToShortDateString();
                    objhdr.insuredCode =Convert.ToString( objpolicy.ContactID);
                    objhdr.firstName = objpolicy.objProspectDetails.FirstName.ToUpper();
                    objhdr.middleName = objpolicy.objProspectDetails.MiddleName != null ? objpolicy.objProspectDetails.MiddleName.ToUpper() : "";
                    objhdr.productPlan = objpolicy.PlanCode.ToUpper();
                    objhdr.surName = objpolicy.objProspectDetails.LastName.ToUpper();
                    objhdr.proposalDate = DateTime.Now.Date.ToShortDateString();
                    objhdr.receiptNo = string.Empty;
                    objhdr.quotationNo = objpolicy.QuoteNo.ToUpper();

                    WsTemporaryProposalReceiptDet objReceiptDetail = new WsTemporaryProposalReceiptDet();
                    objReceiptDetail.proposalNo = objpolicy.ProposalNo.ToUpper();
                    #region Set Instrument Type
                    if (Instrument.PaymentMode == "Cheque")
                    {
                        objReceiptDetail.instrumentType = "CHEQUE";
                    }
                    else if (Instrument.PaymentMode == "Cash")
                    {
                        objReceiptDetail.instrumentType = "CASH";
                    }
                    else if (Instrument.PaymentMode == "DD")
                    {
                        objReceiptDetail.instrumentType = "DRAFT";
                    }
                    else
                    {
                        objReceiptDetail.instrumentType = string.Empty;
                    } 
                    #endregion


                    objReceiptDetail.currency = "LKR";
                    objReceiptDetail.currency = objReceiptDetail.currency.ToUpper();
                    objReceiptDetail.amount = Convert.ToString(Instrument.InstrumentAmount);
                    objReceiptDetail.instrumentNumber = Instrument.InstrumentNo;
                    objReceiptDetail.instrumentDate = Instrument.InstrumentDate.Value.Date.ToShortDateString();
                    // For PG 
                    objReceiptDetail.eopPgCurrencyno = string.Empty;
                    objReceiptDetail.eopPgTransid = string.Empty;
                    objReceiptDetail.eopPgStatusCode = string.Empty;
                    objReceiptDetail.eopPgErrorDetail = string.Empty;
                    objReceiptDetail.eopPgErrorMsg = string.Empty;
                    // Till here

                    // Test Purpose
                    objReceiptDetail.bankCode = "7278"; //test
                    objReceiptDetail.bankCode = objReceiptDetail.bankCode.ToUpper();
                    objReceiptDetail.bankBranchCode = "168"; //test
                    objReceiptDetail.bankBranchCode = objReceiptDetail.bankBranchCode.ToUpper();



                    objReceiptRequest.wsTemporaryProposalReceiptHdr.Add(objhdr);
                    objReceiptRequest.wsTemporaryProposalReceiptDet.Add(objReceiptDetail);
                }

                string URl = "http://secure.AIA.com:8080/Life_Agent_Saving/jersey/";
                string result = GetPostParametersToAPI("Receipting", "SaveProposalReceptDetails", URl, objReceiptRequest);
                ReceiptResponse objReceiptResponse = new ReceiptResponse();
                Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
                objReceiptResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ReceiptResponse>(result, settings);
                if (objReceiptResponse.Status == "Success")
                {
                    objpolicy.Message = "Success";
                }
                else
                {
                    objpolicy.Message = "Error";
                }
            }
            catch (Exception ex)
            {

                objpolicy.Message = "Error";
            }

            return objpolicy.Message;
        }
    }
}
