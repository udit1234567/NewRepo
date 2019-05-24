using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AIA.Life.Repository.AIAEntity;
using System.Data.Entity.Core.Objects;
using System.Data;

namespace AIA.Data.Life.API.ControllerLogic.Common
{
    public class QuoteNo
    {

        public string GenerateQuoteNo(string BranchCode, string Version)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            ObjectParameter nextNo = new ObjectParameter("NextNo", SqlDbType.Int);
            Context.usp_GetNextQuoteNumber("QuoteNo", nextNo);
            Int64 seqNo = Convert.ToInt64(nextNo.Value);
            string value = seqNo.ToString("D7");
            DateTime CurrentDate = DateTime.Now;
            string GetYear = CurrentDate.Year.ToString();
            GetYear = GetYear.Substring(GetYear.Length - 2);
         //   string QuoteNo = "QL-" + BranchCode + "-" + GetYear + "-" + value+"-"+ Version;
            string QuoteNo = "Q" + BranchCode + GetYear  + value + "-" + Version;
            return QuoteNo;

        }
    }
}