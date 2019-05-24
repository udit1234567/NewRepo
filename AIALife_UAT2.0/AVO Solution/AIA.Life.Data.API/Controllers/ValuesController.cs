//using AIA.Life.Integration.Services.LifeAsiaIntegration;
using AIA.Life.Models.Opportunity;
using AIA.Life.Models.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AIA.Life.Data.API.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
        [HttpPost]
        public void GetPlainText()
        {
            Prospect prospect = new Prospect();
            prospect.Name = "Preeti";
            prospect.LastName = "SumanMishra";
            prospect.DateofBirth = Convert.ToDateTime("01-01-1996");
            //prospect.SalutationCode = "MISS";
            prospect.Gender = "F";
            prospect.MaritalStatus = "S";
            prospect.objAddress.Address1 = "STestaddress";
            prospect.objAddress.Address2 = "testdoor";
            prospect.objAddress.Pincode = "00100";
            prospect.Mobile = "9876543210";
            prospect.NIC = "965011234V";
            //prospect.OccupationCode = "2110";
            prospect.Home = "77998767897";
            prospect.Work = "19898767897";
            //prospect = (Prospect)IL.ClientCreation(prospect);
            prospect.ClientCode = "30248452";
            Policy policy = new Policy(); ;
            policy.PlanCode = "PPG";
            policy.AgentCode = "60000991";
            policy.PaymentFrequency = "01";
            policy.AnnualPremium = 32517;
            policy.objProspectDetails = new AIA.Life.Models.Common.MemberDetails();
            policy.objProspectDetails.ClientCode = prospect.ClientCode;
            //ILIntegrator obj = new ILIntegrator();
            //policy = (Policy)IL.BizDate(policy);
            //policy = (Policy)IL.QuickProposal(policy);
            //policy = (Policy)IL.WorkflowAck(policy);
            policy.ProposalNo = "50126628";
            //policy.objMemberDetails = new List<AIA.Life.Models.Common.MemberDetails>();
            //AIA.Life.Models.Common.MemberDetails memberDetails = new AIA.Life.Models.Common.MemberDetails();
            //memberDetails.ClientCode = prospect.ClientCode;
            //memberDetails.Weight = "65";
            //memberDetails.Height = "167";
            //policy.objMemberDetails.Add(memberDetails);
            //policy.objMemberDetails[0].objBenifitDetails = new List<AIA.Life.Models.Common.BenifitDetails>();
            //AIA.Life.Models.Common.BenifitDetails benifitDetails = new AIA.Life.Models.Common.BenifitDetails();
            //benifitDetails.RiderCode = "TPPG";
            //benifitDetails.RiderPremium = "24000";
            //benifitDetails.RiderSuminsured = "480000";
            //policy.objMemberDetails[0].objBenifitDetails.Add(benifitDetails);
            //AIA.Life.Models.Common.BenifitDetails benifitDetails1 = new AIA.Life.Models.Common.BenifitDetails();
            //benifitDetails1.RiderCode = "TMLA";
            //benifitDetails1.RiderPremium = "5130";
            //benifitDetails1.RiderSuminsured = "456000";
            //policy.objMemberDetails[0].objBenifitDetails.Add(benifitDetails1);
            //AIA.Life.Models.Common.BenifitDetails benifitDetails2 = new AIA.Life.Models.Common.BenifitDetails();
            //benifitDetails2.RiderCode = "TMAC";
            //benifitDetails2.RiderPremium = "1260";
            //benifitDetails2.RiderSuminsured = "720000";
            //policy.objMemberDetails[0].objBenifitDetails.Add(benifitDetails2);
            //AIA.Life.Models.Common.BenifitDetails benifitDetails3 = new AIA.Life.Models.Common.BenifitDetails();
            //benifitDetails3.RiderCode = "TMWG";
            //benifitDetails3.RiderPremium = "2127";
            //benifitDetails3.RiderSuminsured = "3039";
            //policy.objMemberDetails[0].objBenifitDetails.Add(benifitDetails3);
            //policy = (Policy)IL.ModifyProposalAddLife(policy);
            //policy = (Policy)IL.ProposalPreIssueValidation(policy);
            //policy = (Policy)IL.ProposalFollowupEnquiry(policy);
            //policy = (Policy)IL.ProposalUWApproval(policy);
            //policy = (Policy)IL.QualityControl(policy);
            //policy = (Policy)IL.ProposalIssuance(policy);
            //obj.ReadResponseString("", new Policy(), "PreIssueRulesResponse");
        }
    }
}

