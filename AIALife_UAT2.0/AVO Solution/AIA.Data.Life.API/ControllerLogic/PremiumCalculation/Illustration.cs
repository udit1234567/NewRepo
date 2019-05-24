using AIA.Life.Models.Common;
using AIA.Life.Models.Opportunity;
using AIA.Life.Repository.AIAEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AIA.Data.Life.API.ControllerLogic.PremiumCalculation
{
    public class Illustration: MapObject
    {
        public LifeQuote GetIllustration(LifeQuote objLifeQuote)
        {
            string xmlStr = MapQuotePremiumObject(new AIA.Life.Repository.AIAEntity.AVOAIALifeEntities(),objLifeQuote);
            #region  Log Input 
            AVOAIALifeEntities entities = new AVOAIALifeEntities();
            tbllogxml objlogxml = new tbllogxml();
            objlogxml.Description = "Illustration xml";
            objlogxml.PolicyID = Convert.ToString(objLifeQuote.objProspect.ContactID);
            objlogxml.UserID = objLifeQuote.UserName;
            objlogxml.XMlData = xmlStr;
            objlogxml.CreatedDate = DateTime.Now;
            entities.tbllogxmls.Add(objlogxml);
            entities.SaveChanges();
            #endregion
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            con.Open();
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_GetIllustration";
            cmd.Parameters.Add("@XmlStr", SqlDbType.VarChar);
            cmd.Parameters[0].Value = xmlStr;
            DataSet ds = new DataSet();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
            da.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Illustation ill = new Illustation();
                ill.PolicyYear = Convert.ToInt32(ds.Tables[0].Rows[i]["PolicyYear"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["PolicyYear"]);
                ill.BasicPremium = Convert.ToInt32(ds.Tables[0].Rows[i]["BasicPremium"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["BasicPremium"]);
                ill.MainBenefitsPremium = Convert.ToInt32(ds.Tables[0].Rows[i]["MainBenefitsPremium"]==DBNull.Value?0: ds.Tables[0].Rows[i]["MainBenefitsPremium"]);
                ill.AdditionalBenefitsPremiums = Convert.ToInt32(ds.Tables[0].Rows[i]["AdditionalBenefitsPremiums"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["AdditionalBenefitsPremiums"]);
                ill.TotalPremium = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalPremium"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["TotalPremium"]);
                ill.FundBalanceDiv4 = Convert.ToInt64(ds.Tables[0].Rows[i]["FundBalanceDiv4"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["FundBalanceDiv4"]);
                ill.SurrenderValueDiv4 = Convert.ToInt64(ds.Tables[0].Rows[i]["SurrenderValueDiv4"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["SurrenderValueDiv4"]);
                ill.DrawDownDiv4 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv4"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv4"]);
                ill.PensionBoosterDiv4 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv4_Pensionbooster"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv4_Pensionbooster"]);
                ill.FundBalanceDiv8 = Convert.ToInt64(ds.Tables[0].Rows[i]["FundBalanceDiv8"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["FundBalanceDiv8"]);
                ill.SurrenderValueDiv8 = Convert.ToInt64(ds.Tables[0].Rows[i]["SurrenderValueDiv8"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["SurrenderValueDiv8"]);
                ill.DrawDownDiv8 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv8"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv8"]);
                ill.PensionBoosterDiv8 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv8_Pensionbooster"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv8_Pensionbooster"]);
                ill.FundBalanceDiv12 = Convert.ToInt64(ds.Tables[0].Rows[i]["FundBalanceDiv12"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["FundBalanceDiv12"]);
                ill.SurrenderValueDiv12 = Convert.ToInt64(ds.Tables[0].Rows[i]["SurrenderValueDiv12"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["SurrenderValueDiv12"]);
                ill.DrawDownDiv12 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv12"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv12"]);
                ill.PensionBoosterDiv12 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv12_Pensionbooster"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv12_Pensionbooster"]);
                ill.FundBalanceDiv5 = Convert.ToInt64(ds.Tables[0].Rows[i]["FundBalanceDiv5"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["FundBalanceDiv5"]);
                ill.FundBalanceDiv6 = Convert.ToInt64(ds.Tables[0].Rows[i]["FundBalanceDiv6"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["FundBalanceDiv6"]);
                ill.FundBalanceDiv7 = Convert.ToInt64(ds.Tables[0].Rows[i]["FundBalanceDiv7"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["FundBalanceDiv7"]);
                ill.FundBalanceDiv9 = Convert.ToInt64(ds.Tables[0].Rows[i]["FundBalanceDiv9"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["FundBalanceDiv9"]);
                ill.FundBalanceDiv10 = Convert.ToInt64(ds.Tables[0].Rows[i]["FundBalanceDiv10"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["FundBalanceDiv10"]);
                ill.FundBalanceDiv11 = Convert.ToInt64(ds.Tables[0].Rows[i]["FundBalanceDiv11"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["FundBalanceDiv11"]);
                ill.DrawDownDiv5 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv5"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv5"]);
                ill.DrawDownDiv6 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv6"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv6"]);
                ill.DrawDownDiv7 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv7"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv7"]);
                ill.DrawDownDiv9 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv9"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv9"]);
                ill.DrawDownDiv10 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv10"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv10"]);
                ill.DrawDownDiv11 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv11"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv11"]);
                ill.UnAllocatedPremium = Convert.ToInt64(ds.Tables[0].Rows[i]["UnAllocatedPremium"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["UnAllocatedPremium"]);
                objLifeQuote.LstIllustation.Add(ill);
            }
            return objLifeQuote;
        }
        public void GetDrawDownDetails(LifeQuote objLifeQuote)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            string xmlStr = MapQuotePremiumObject(new AIA.Life.Repository.AIAEntity.AVOAIALifeEntities(), objLifeQuote);
            #region  Log Input 
            AVOAIALifeEntities entities = new AVOAIALifeEntities();
            tbllogxml objlogxml = new tbllogxml();
            objlogxml.Description = "DrawDown xml";
            objlogxml.PolicyID = Convert.ToString(objLifeQuote.objProspect.ContactID);
            objlogxml.UserID = objLifeQuote.UserName;
            objlogxml.XMlData = xmlStr;
            objlogxml.CreatedDate = DateTime.Now;
            entities.tbllogxmls.Add(objlogxml);
            entities.SaveChanges();
            #endregion
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            con.Open();
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "exec usp_GetDrawDownDetails '"+ xmlStr +"'";
            //cmd.Parameters.Add("@XmlStr", SqlDbType.VarChar);
            //cmd.Parameters[0].Value = xmlStr;
            DataSet ds = new DataSet();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
            da.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                //DrawDownDetails ddd = new DrawDownDetails();
                tblQuoteDrawDownDetail objtblQuoteDrawDownDetail = new tblQuoteDrawDownDetail();
                objtblQuoteDrawDownDetail.LifeQQID = objLifeQuote.LifeQQID;
                objtblQuoteDrawDownDetail.PaymentFequency = Convert.ToInt32(ds.Tables[0].Rows[i]["PaymentFequency"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["PaymentFequency"]);
                objtblQuoteDrawDownDetail.DrawDownDiv4 = Convert.ToDecimal(ds.Tables[0].Rows[i]["DrawDownDiv4"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv4"]);
                objtblQuoteDrawDownDetail.DrawDownDiv8 = Convert.ToDecimal(ds.Tables[0].Rows[i]["DrawDownDiv8"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv8"]);
                objtblQuoteDrawDownDetail.DrawDownDiv12 = Convert.ToDecimal(ds.Tables[0].Rows[i]["DrawDownDiv12"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv12"]);
                Context.tblQuoteDrawDownDetails.Add(objtblQuoteDrawDownDetail);
            }
            Context.SaveChanges();
        }

        public AIA.Life.Models.Policy.Policy GetProposalIllustration(AIA.Life.Models.Policy.Policy objpolicy)
        {
            //string xmlStr = MapQuotePremiumObjectFOrUW(new AIA.Life.Repository.AIAEntity.AVOAIALifeEntities(), objProposal);
            string xmlStr = string.Empty;
            #region  Log Input 
            AVOAIALifeEntities entities = new AVOAIALifeEntities();
            tbllogxml objlogxml = new tbllogxml();
            objlogxml.Description = "PolicyIllustration xml";
            objlogxml.PolicyID = Convert.ToString(objpolicy.PolicyID);
            objlogxml.UserID = objpolicy.UserName;
            objlogxml.XMlData = xmlStr;
            objlogxml.CreatedDate = DateTime.Now;
            entities.tbllogxmls.Add(objlogxml);
            entities.SaveChanges();
            #endregion
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            con.Open();
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_GetIllustration";
            cmd.Parameters.Add("@XmlStr",  SqlDbType.VarChar);
            cmd.Parameters.Add("@QuoteNo", SqlDbType.VarChar);
            cmd.Parameters[0].Value = xmlStr;
            cmd.Parameters[1].Value = objpolicy.ProposalNo;
            DataSet ds = new DataSet();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
            da.Fill(ds);
            objpolicy.LstIllustation = new List<Illustation>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Illustation ill = new Illustation();
                ill.PolicyYear = Convert.ToInt32(ds.Tables[0].Rows[i]["PolicyYear"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["PolicyYear"]);
                ill.BasicPremium = Convert.ToInt32(ds.Tables[0].Rows[i]["BasicPremium"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["BasicPremium"]);
                ill.MainBenefitsPremium = Convert.ToInt32(ds.Tables[0].Rows[i]["MainBenefitsPremium"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["MainBenefitsPremium"]);
                ill.AdditionalBenefitsPremiums = Convert.ToInt32(ds.Tables[0].Rows[i]["AdditionalBenefitsPremiums"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["AdditionalBenefitsPremiums"]);
                ill.TotalPremium = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalPremium"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["TotalPremium"]);
                ill.FundBalanceDiv4 = Convert.ToInt64(ds.Tables[0].Rows[i]["FundBalanceDiv4"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["FundBalanceDiv4"]);
                ill.SurrenderValueDiv4 = Convert.ToInt64(ds.Tables[0].Rows[i]["SurrenderValueDiv4"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["SurrenderValueDiv4"]);
                ill.DrawDownDiv4 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv4"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv4"]);
                ill.PensionBoosterDiv4 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv4_Pensionbooster"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv4_Pensionbooster"]);
                ill.FundBalanceDiv8 = Convert.ToInt64(ds.Tables[0].Rows[i]["FundBalanceDiv8"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["FundBalanceDiv8"]);
                ill.SurrenderValueDiv8 = Convert.ToInt64(ds.Tables[0].Rows[i]["SurrenderValueDiv8"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["SurrenderValueDiv8"]);
                ill.DrawDownDiv8 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv8"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv8"]);
                ill.PensionBoosterDiv8 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv8_Pensionbooster"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv8_Pensionbooster"]);
                ill.FundBalanceDiv12 = Convert.ToInt64(ds.Tables[0].Rows[i]["FundBalanceDiv12"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["FundBalanceDiv12"]);
                ill.SurrenderValueDiv12 = Convert.ToInt64(ds.Tables[0].Rows[i]["SurrenderValueDiv12"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["SurrenderValueDiv12"]);
                ill.DrawDownDiv12 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv12"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv12"]);
                ill.PensionBoosterDiv12 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv12_Pensionbooster"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv12_Pensionbooster"]);
                ill.PensionBoosterDiv12 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv12_Pensionbooster"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv12_Pensionbooster"]);
                ill.FundBalanceDiv5 = Convert.ToInt64(ds.Tables[0].Rows[i]["FundBalanceDiv5"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["FundBalanceDiv5"]);
                ill.FundBalanceDiv6 = Convert.ToInt64(ds.Tables[0].Rows[i]["FundBalanceDiv6"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["FundBalanceDiv6"]);
                ill.FundBalanceDiv7 = Convert.ToInt64(ds.Tables[0].Rows[i]["FundBalanceDiv7"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["FundBalanceDiv7"]);
                ill.FundBalanceDiv9 = Convert.ToInt64(ds.Tables[0].Rows[i]["FundBalanceDiv9"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["FundBalanceDiv9"]);
                ill.FundBalanceDiv10 = Convert.ToInt64(ds.Tables[0].Rows[i]["FundBalanceDiv10"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["FundBalanceDiv10"]);
                ill.FundBalanceDiv11 = Convert.ToInt64(ds.Tables[0].Rows[i]["FundBalanceDiv11"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["FundBalanceDiv11"]);
                ill.DrawDownDiv5 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv5"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv5"]);
                ill.DrawDownDiv6 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv6"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv6"]);
                ill.DrawDownDiv7 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv7"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv7"]);
                ill.DrawDownDiv9 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv9"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv9"]);
                ill.DrawDownDiv10 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv10"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv10"]);
                ill.DrawDownDiv11 = Convert.ToInt64(ds.Tables[0].Rows[i]["DrawDownDiv11"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["DrawDownDiv11"]);
                ill.UnAllocatedPremium = Convert.ToInt64(ds.Tables[0].Rows[i]["UnAllocatedPremium"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["UnAllocatedPremium"]);
                objpolicy.LstIllustation.Add(ill);
            }
            return objpolicy;
        }
    }
}