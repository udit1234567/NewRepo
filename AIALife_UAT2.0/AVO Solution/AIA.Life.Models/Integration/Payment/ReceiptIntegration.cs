using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Integration.Payment
{
   

    public class WsTemporaryProposalReceiptHdr
    {
       
        public WsTemporaryProposalReceiptHdr()
        {
            companyCode = "00003";
            vendercode = "INUBE";
        }
        public string companyCode { get; set; }
        public string branchCode { get; set; }
        public string agentCode { get; set; }
        public string proposalNo { get; set; }
        public string policyNo { get; set; }
        public string tempReceiptNo { get; set; }
        public string totalAmount { get; set; }
        public string createBy { get; set; }
        public string createDt { get; set; }
        public string insuredCode { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string surName { get; set; }
        public string productPlan { get; set; }
        public string proposalDate { get; set; }
        public string modeOfPayment { get; set; }
        public string receiptedDate { get; set; }
        public string receiptNo { get; set; }
        public string vendercode { get; set; }
        public string quotationNo { get; set; }
    }

    public class WsTemporaryProposalReceiptDet
    {
        public string proposalNo { get; set; }
        public string instrumentType { get; set; }
        public string currency { get; set; }
        public string amount { get; set; }
        public string instrumentDate { get; set; }
        public string instrumentNumber { get; set; }
        public string eopPgCurrencyno { get; set; }
        public string eopPgTransid { get; set; }
        public string eopPgStatusCode { get; set; }
        public string eopPgErrorCode { get; set; }
        public string eopPgErrorDetail { get; set; }
        public string eopPgErrorMsg { get; set; }
        public string bankCode { get; set; }
        public string bankBranchCode { get; set; }
    }

    public class ReceiptRequest
    {
        public ReceiptRequest()
        {
            wsTemporaryProposalReceiptHdr = new List<WsTemporaryProposalReceiptHdr>();
            wsTemporaryProposalReceiptDet = new List<WsTemporaryProposalReceiptDet>();
        }
        public string userId { get; set; }
        public List<WsTemporaryProposalReceiptHdr> wsTemporaryProposalReceiptHdr { get; set; }
        public List<WsTemporaryProposalReceiptDet> wsTemporaryProposalReceiptDet { get; set; }
    }


    public class ReceiptResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string  Code { get; set; }
    }


}
