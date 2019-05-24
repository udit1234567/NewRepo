
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using AIA.Life.Repository.AIAEntity;
using System.Data.Entity.Core.Objects;

namespace AIA.Presentation.AVOLife
{
    public class GenerateCodes
    {
        /// <summary>
        /// It Getnerates Unique QuoteNo or proposalNo for all products
        /// </summary>
        /// <returns>Quote/Proposal No</returns>
        public String GenerateQuoteNo()
        {
            AVOAIALifeEntities objEntities = new AVOAIALifeEntities();
            System.Data.Objects.ObjectParameter param1 = new System.Data.Objects.ObjectParameter("NextNo", SqlDbType.Int);
            //var iSeqno = objEntities.usp_GetNextQuoteNumber("QuoteNo",param1);
            Int32 NextSeqNo = Convert.ToInt32(param1.Value);
            String QuoteNumber =  System.Configuration.ConfigurationManager.AppSettings["QuoteNumberFixedCode"] + NextSeqNo.ToString().PadLeft(7, '0');

            return QuoteNumber;

        }

        /// <summary>
        /// It Generates Unique UserId or Sub UserId based on IMD code
        /// </summary>
        /// <param name="AgentCode">Intermediary Code</param>
        /// <returns>UserId or SubUser Id</returns>
        public String GenerateUserID(string AgentCode)
        {
            //AgentCode = "User1234";
            AVOAIALifeEntities objEntities = new AVOAIALifeEntities();
            ObjectParameter param1 = new ObjectParameter("NextNo", SqlDbType.Int);
           // System.Data.Objects.ObjectParameter param1 = new System.Data.Objects.ObjectParameter("NextNo", SqlDbType.Int);
            var iSeqno = objEntities.usp_GetNextUserId(param1,AgentCode);
            Int32 NextSeqNo = Convert.ToInt32(param1.Value);
            String UserID = AgentCode +"_"+NextSeqNo.ToString().PadLeft(3, '0');

            return UserID;

        }

        /// <summary>
        /// It Generates Unique IMD Code for temp purpose
        /// </summary>
        /// <returns>IMD Code</returns>
        public String GenerateIMDCode()
        {
            AVOAIALifeEntities objEntities = new AVOAIALifeEntities();
            System.Data.Objects.ObjectParameter param1 = new System.Data.Objects.ObjectParameter("NextNo", SqlDbType.Int);
            //var iSeqno = objEntities.usp_GetNextIMDCode(param1);
            Int32 NextSeqNo = Convert.ToInt32(param1.Value);
            String IMDCode = "$IMD" + NextSeqNo.ToString().PadLeft(5, '0');

            return IMDCode;

        }

        /// <summary>
        /// Generate Marine Profile ID
        /// </summary>
        /// <returns></returns>
        public String GenerateMarineProfileNo()
        {
            AVOAIALifeEntities objEntities = new AVOAIALifeEntities();
            System.Data.Objects.ObjectParameter param1 = new System.Data.Objects.ObjectParameter("NextNo", SqlDbType.Int);
           // var iSeqno = objEntities.usp_GetNextMarineProfileId(param1);
            Int32 NextSeqNo = Convert.ToInt32(param1.Value);
            String ProfileNo = NextSeqNo.ToString().PadLeft(6, '0');

            return ProfileNo;

        }

        public String GenerateTransactionNO()
        {
            AVOAIALifeEntities objEntities = new AVOAIALifeEntities();

            string strProposalNo = string.Empty;
            strProposalNo = DateTime.Today.Day.ToString().PadLeft(2, '0') + DateTime.Today.Month.ToString().PadLeft(2, '0') + DateTime.Today.Year.ToString().Substring(2, 2);
            System.Data.Objects.ObjectParameter param1 = new System.Data.Objects.ObjectParameter("NextNo", SqlDbType.Int);
           // var iSeqno = objEntities.usp_getNextProposalNo(strProposalNo, param1);
            Int32 NextSeqNo = Convert.ToInt32(param1.Value);

            string ProposalNo = "C" + strProposalNo + NextSeqNo.ToString().PadLeft(5, '0'); //DateTime.Now.ToString("ddMMyyHHmmss");
            return ProposalNo;
        }

        public String GeneratePGTransactionNO()
        {
            AVOAIALifeEntities objEntities = new AVOAIALifeEntities();

            string strProposalNo = string.Empty;
            strProposalNo = DateTime.Today.Day.ToString().PadLeft(2, '0') + DateTime.Today.Month.ToString().PadLeft(2, '0') + DateTime.Today.Year.ToString().Substring(2, 2);
            System.Data.Objects.ObjectParameter param1 = new System.Data.Objects.ObjectParameter("NextNo", SqlDbType.Int);
            //var iSeqno = objEntities.usp_getNextProposalNo(strProposalNo, param1);
            Int32 NextSeqNo = Convert.ToInt32(param1.Value);

            string ProposalNo = "PGT" + strProposalNo + NextSeqNo.ToString().PadLeft(5, '0'); //DateTime.Now.ToString("ddMMyyHHmmss");
            return ProposalNo;
        }

        public String GenerateReplenishmentTransactionNO()
        {
            AVOAIALifeEntities objEntities = new AVOAIALifeEntities();

            string strProposalNo = string.Empty;
            strProposalNo = DateTime.Today.Day.ToString().PadLeft(2, '0') + DateTime.Today.Month.ToString().PadLeft(2, '0') + DateTime.Today.Year.ToString().Substring(2, 2);
            System.Data.Objects.ObjectParameter param1 = new System.Data.Objects.ObjectParameter("NextNo", SqlDbType.Int);
           // var iSeqno = objEntities.usp_getNextProposalNo(strProposalNo, param1);
            Int32 NextSeqNo = Convert.ToInt32(param1.Value);

            string ProposalNo = "D" + strProposalNo + NextSeqNo.ToString().PadLeft(5, '0'); //DateTime.Now.ToString("ddMMyyHHmmss");
            return ProposalNo;
        }
        public String GenerateIRNo(string branchCode, string Date, string AgencyCode = null)
        {
            AVOAIALifeEntities objEntities = new AVOAIALifeEntities();
            System.Data.Objects.ObjectParameter param1 = new System.Data.Objects.ObjectParameter("NextNo", SqlDbType.Int);
            //var iSeqno = objEntities.usp_GetNextIRNumber("IRNo", param1);
            Int32 NextSeqNo = Convert.ToInt32(param1.Value);
            String IRNumber = null;
            if (AgencyCode == null)
            {
                IRNumber = branchCode + "-" + "SGN" + "-" + Date + "-" + NextSeqNo.ToString().PadLeft(6, '0');
            }
            else
            {
                IRNumber = branchCode + "-" + AgencyCode + "-" + Date + "-" + NextSeqNo.ToString().PadLeft(6, '0');
            }

            return IRNumber;

        }
    }
}
