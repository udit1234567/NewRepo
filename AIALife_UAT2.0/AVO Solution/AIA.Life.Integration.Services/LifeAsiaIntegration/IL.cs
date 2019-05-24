using AIA.Life.Repository.AIAEntity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Integration.Services.LifeAsiaIntegration
{
    public static class IL
    {
        static ILIntegrator integrator = new ILIntegrator();
        static bool _enableIL = Convert.ToBoolean(ConfigurationManager.AppSettings["ILIntegration"]);
        public static object ClientEnquiry(object obj)
        {
            if (_enableIL)
            {
                string responseText = integrator.GeneratePlainText(obj, "ClientEnquiryRequest", "ClientEnquiryResponse");
                obj = integrator.ReadResponseString(responseText, obj, "ClientEnquiryResponse");
            }
            return obj;
        }
        public static object ClientCreation(object obj)
        {
            if (_enableIL)
            {
                string responseText = integrator.GeneratePlainText(obj, "ClientCreationRequest", "ClientCreationResponse");
                obj = integrator.ReadResponseString(responseText, obj, "ClientCreationResponse");
            }
            return obj;
        }
        public static object ModifyClient(object obj)
        {
            if (_enableIL)
            {
                string responseText = integrator.GeneratePlainText(obj, "ClientModifyRequest", "ClientModifyResponse");
                obj = integrator.ReadResponseString(responseText, obj, "ClientModifyResponse");
            }
            return obj;
        }
        public static object ClientRelationshipEnquiry(object obj)
        {
            if (_enableIL)
            {
                string responseText = integrator.GeneratePlainText(obj, "ClientRelEnqRequest", "ClientRelEnqResponse");
                obj = integrator.ReadResponseString(responseText, obj, "ClientRelEnqResponse");
            }
            return obj;
        }
        public static object ClientRelationshipCreation(object obj)
        {
            if (_enableIL)
            {
                string responseText = integrator.GeneratePlainText(obj, "ClientRelCreationRequest", "ClientRelCreationResponse");
                obj = integrator.ReadResponseString(responseText, obj, "ClientRelCreationResponse");
            }
            return obj;
        }
        public static object QuickProposal(object obj)
        {
            if (_enableIL)
            {
                string responseText = integrator.GeneratePlainText(obj, "QuickProposalRequest", "QuickProposalResponse");
                obj = integrator.ReadResponseString(responseText, obj, "QuickProposalResponse");
            }
            return obj;
        }
        public static object BizDate(object obj)
        {
            if (_enableIL)
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                var accMonth = Context.tblILIntegrations.Where(a => a.ColumnName == "ACCTMN").FirstOrDefault();
                var accYear = Context.tblILIntegrations.Where(a => a.ColumnName == "ACCTYR").FirstOrDefault();
                accMonth.FieldValue = (DateTime.Now.Month).ToString().PadLeft(2, '0');
                accYear.FieldValue = DateTime.Now.Year.ToString();
                Context.SaveChanges();
                string responseText = integrator.GeneratePlainText(obj, "BIZDateRequest", "BIZDateResponse");
                if(responseText!=null && responseText.ToUpper().Contains("ACCT PERD OUTSIDE RANGE"))
                {
                    var accMonth2=Context.tblILIntegrations.Where(a => a.ColumnName == "ACCTMN").FirstOrDefault();
                    var accYear2 = Context.tblILIntegrations.Where(a => a.ColumnName == "ACCTYR").FirstOrDefault();
                    accMonth2.FieldValue = (DateTime.Now.Month-1).ToString().PadLeft(2, '0');
                    accYear2.FieldValue = DateTime.Now.Year.ToString();
                    Context.SaveChanges();
                    string responseText1 = integrator.GeneratePlainText(obj, "BIZDateRequest", "BIZDateResponse");
                    if (responseText != null && responseText1.ToUpper().Contains("ACCT PERD OUTSIDE RANGE"))
                    {
                        var accMonth1 = Context.tblILIntegrations.Where(a => a.ColumnName == "ACCTMN").FirstOrDefault();
                        var accYear1 = Context.tblILIntegrations.Where(a => a.ColumnName == "ACCTYR").FirstOrDefault();
                        accMonth1.FieldValue = (DateTime.Now.Month - 1).ToString().PadLeft(2, '0');
                        accYear1.FieldValue = (DateTime.Now.Year-1).ToString();
                        Context.SaveChanges();
                        string responseText2 = integrator.GeneratePlainText(obj, "BIZDateRequest", "BIZDateResponse");
                        obj = integrator.ReadResponseString(responseText2, obj, "BIZDateResponse");
                    }
                    else
                        obj = integrator.ReadResponseString(responseText1, obj, "BIZDateResponse");
                }
                else
                    obj = integrator.ReadResponseString(responseText, obj, "BIZDateResponse");
            }
            return obj;
        }
        public static object RecieptEnquiry(object obj)
        {
            if (_enableIL)
            {
                string responseText = integrator.GeneratePlainText(obj, "ReceiptEnqRequest", "ReceiptEnqResponse");
                obj = integrator.ReadResponseString(responseText, obj, "ReceiptEnqResponse");
            }
            return obj;
        }
        public static object ModifyProposalAddLife(object obj)
        {
            if (_enableIL)
            {
                string responseText = integrator.GeneratePlainText(obj, "AddLifeRequest", "AddLifeResponse");
                obj = integrator.ReadResponseString(responseText, obj, "AddLifeResponse");
            }
            return obj;
        }
        public static object ModifyProposalModifyLife(object obj)
        {
            if (_enableIL)
            {
                string responseText = integrator.GeneratePlainText(obj, "ModifyLifeRequest", "ModifyLifeResponse");
                obj = integrator.ReadResponseString(responseText, obj, "ModifyLifeResponse");
            }
            return obj;
        }
        public static object RefreshRiders(object obj)
        {
            if (_enableIL)
            {
                string responseText = integrator.ModifyLifeDetailsRiderRefresh(obj);
                obj = integrator.ReadResponseString(responseText, obj, "ModifyLifeResponse");
            }
            return obj;
        }
        public static object RefreshAll(object obj)
        {
            if (_enableIL)
            {
                string responseText = integrator.ModifyLifeRefreshAll(obj);
                obj = integrator.ReadResponseString(responseText, obj, "ModifyLifeResponse");
            }
            return obj;
        }
        public static object UpdatePolicyNotes(object obj)
        {
            if (_enableIL)
            {
                string responseText = integrator.ModifyLifeDetailsPolicyRemarks(obj);
                obj = integrator.ReadResponseString(responseText, obj, "ModifyLifeResponse");
            }
            return obj;
        }
        public static object DeleteLife(object obj)
        {
            if (_enableIL)
            {
                string responseText = integrator.GeneratePlainText(obj, "DeleteLifeRequest", "DeleteLifeResponse");
                obj = integrator.ReadResponseString(responseText, obj, "DeleteLifeResponse");
            }
            return obj;
        }
        public static object ProposalPreIssueValidation(object obj)
        {
            if (_enableIL)
            {
                string responseText = integrator.GeneratePlainText(obj, "PreIssueRulesRequest", "PreIssueRulesResponse");
                obj = integrator.ReadResponseString(responseText, obj, "PreIssueRulesResponse");
            }
            return obj;
        }
        public static object ProposalFollowupEnquiry(object obj)
        {
            if (_enableIL)
            {
                string responseText = integrator.GeneratePlainText(obj, "FollowUpEnqRequest", "FollowUpEnqResponse");
                obj = integrator.ReadResponseString(responseText, obj, "FollowUpEnqResponse");
            }
            return obj;
        }
        public static object ProposalFollowupModify(object obj)
        {
            if (_enableIL)
            {
                string responseText = integrator.GeneratePlainText(obj, "FollowUpModifyRequest", "FollowUpModifyResponse");
                obj = integrator.ReadResponseString(responseText, obj, "FollowUpModifyResponse");
            }
            return obj;
        }
        public static object ProposalUWApproval(object obj)
        {
            if (_enableIL)
            {
                string responseText = integrator.GeneratePlainText(obj, "UWApprovalRequest", "UWApprovalResponse");
                obj = integrator.ReadResponseString(responseText, obj, "UWApprovalResponse");
            }
            return obj;
        }
        public static object WithdrawProposal(object obj)
        {
            if (_enableIL)
            {
                string responseText = integrator.GeneratePlainText(obj, "WithdrawRequest", "WithdrawResponse");
                obj = integrator.ReadResponseString(responseText, obj, "WithdrawResponse");
            }
            return obj;
        }
        public static object QualityControl(object obj)
        {
            if (_enableIL)
            {
                string responseText = integrator.GeneratePlainText(obj, "QCRequest", "QCResponse");
                obj = integrator.ReadResponseString(responseText, obj, "QCResponse");
            }
            return obj;
        }
        public static object ProposalIssuance(object obj)
        {
            if (_enableIL)
            {
                string responseText = integrator.GeneratePlainText(obj, "PolicyIssuanceRequest", "PolicyIssuanceResponse");
                obj = integrator.ReadResponseString(responseText, obj, "PolicyIssuanceResponse");
            }
            return obj;
        }
        public static object WorkflowAck(object obj)
        {
            if (_enableIL)
            {
                string responseText = integrator.GeneratePlainText(obj, "WorkflowAckRequest", "WorkflowAckResponse");
                obj = integrator.ReadResponseString(responseText, obj, "WorkflowAckResponse");
            }
            return obj;
        }
    }
}
