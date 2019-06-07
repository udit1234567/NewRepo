
using AIA.Life.Models.Opportunity;
using AIA.Life.Models.Reports;
using Microsoft.Reporting.WebForms;
using Rotativa.MVC;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using AIA.Life.Models.Integration.Services;
using AIA.Life.Models.Common;
using AIA.Life.Repository.AIAEntity;
using AIA.Life.Models.Integration;
using AIA.Life.Models.Policy;
using log4net;
using AIA.CrossCutting;
using AIA.Life.Models.EmailSMSDetails;
using iTextSharp.text.pdf;
using AIA.Presentation.AVOLife.ExceptionHandling;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.Serialization;
using Fonet;

namespace AIA.Presentation.AVOLife.Controllers
{
    [Authorize]
    [ErrorLogging]
    [SessionTimeout]
    public class ReportsController : Controller
    {
        AVOAIALifeEntities Context = new AVOAIALifeEntities();
        private string _username = string.Empty;
        AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
        public ReportsController()
        {
            _username = System.Web.HttpContext.Current.User.Identity.Name;
        }
        #region CLA Letter

        public byte[] ReportForCLADocument(string QuoteNo, string ProposalNo)
        {
            try
            {

                Microsoft.Reporting.WebForms.ReportViewer vr = new Microsoft.Reporting.WebForms.ReportViewer();
                DataSet dataset = CLAmemberDetails(QuoteNo);
                List<DataSet> dslst = new List<DataSet>();
                DataSet ds = new DataSet();
                dslst.Add(dataset);
                ReportParameter[] parameters;
                DataSet ds2 = new DataSet();
                ds2 = FuncForMainLife(ProposalNo);
                dslst.Add(ds2);
                DataSet ds3 = new DataSet();
                ds3 = FuncForSpouseLife(ProposalNo);
                dslst.Add(ds3);
                DataSet ds4 = new DataSet();
                ds4 = FuncForC1(ProposalNo);
                dslst.Add(ds4);
                DataSet ds5 = new DataSet();
                ds5 = FuncForC2(ProposalNo);
                dslst.Add(ds5);
                DataSet ds6 = new DataSet();
                ds6 = FuncForC3(ProposalNo);
                dslst.Add(ds6);
                DataSet ds7 = new DataSet();
                ds7 = FuncForC4(ProposalNo);
                dslst.Add(ds7);
                DataSet ds8 = new DataSet();
                ds8 = FuncForC5(ProposalNo);
                dslst.Add(ds8);
                DataSet ds9 = new DataSet();
                ds9 = FuncForMainLifeLoadingBasis(ProposalNo);
                dslst.Add(ds9);
                DataSet ds10 = new DataSet();
                ds10 = FuncForDeletedReiders(ProposalNo);
                dslst.Add(ds10);
                DataSet ds11 = new DataSet();
                ds11 = FuncForAllRidersforallLives(ProposalNo);
                dslst.Add(ds11);
                parameters = new ReportParameter[2];
                parameters[0] = new ReportParameter("QuoteNo", QuoteNo);
                parameters[1] = new ReportParameter("ProposalNo", ProposalNo);
                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/CLAdocument.rdlc");
                return bytes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public void CLADocumentTest(string QuoteNo, string ProposalNo)
        {
            try
            {

                Microsoft.Reporting.WebForms.ReportViewer vr = new Microsoft.Reporting.WebForms.ReportViewer();
                DataSet dataset = CLAmemberDetails(QuoteNo);
                List<DataSet> dslst = new List<DataSet>();
                DataSet ds = new DataSet();
                dslst.Add(dataset);
                ReportParameter[] parameters;
                DataSet ds2 = new DataSet();
                ds2 = FuncForMainLife(ProposalNo);
                dslst.Add(ds2);
                DataSet ds3 = new DataSet();
                ds3 = FuncForSpouseLife(ProposalNo);
                dslst.Add(ds3);
                DataSet ds4 = new DataSet();
                ds4 = FuncForC1(ProposalNo);
                dslst.Add(ds4);
                DataSet ds5 = new DataSet();
                ds5 = FuncForC2(ProposalNo);
                dslst.Add(ds5);
                DataSet ds6 = new DataSet();
                ds6 = FuncForC3(ProposalNo);
                dslst.Add(ds6);
                DataSet ds7 = new DataSet();
                ds7 = FuncForC4(ProposalNo);
                dslst.Add(ds7);
                DataSet ds8 = new DataSet();
                ds8 = FuncForC5(ProposalNo);
                dslst.Add(ds8);
                DataSet ds9 = new DataSet();
                ds9 = FuncForMainLifeLoadingBasis(ProposalNo);
                dslst.Add(ds9);
                DataSet ds10 = new DataSet();
                ds10 = FuncForDeletedReiders(ProposalNo);
                dslst.Add(ds10);
                DataSet ds11 = new DataSet();
                ds11 = FuncForAllRidersforallLives(ProposalNo);
                dslst.Add(ds11);

                parameters = new ReportParameter[2];
                parameters[0] = new ReportParameter("QuoteNo", QuoteNo);
                parameters[1] = new ReportParameter("ProposalNo", ProposalNo);
                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/CLAdocument.rdlc");
                RenderReports(bytes, QuoteNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForC5(string ProposalNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_CLAChild5BenifitDetails"; /*"SP_QuotePDFDetails";*/
                cmd.Parameters.AddWithValue("@ProposalNo", ProposalNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForMainLifeLoadingBasis(string ProposalNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_CLAMainLifeBenifitDetailsLoadingBasis"; /*"SP_QuotePDFDetails";*/
                cmd.Parameters.AddWithValue("@ProposalNo", ProposalNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
            public DataSet FuncForDeletedReiders(string ProposalNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_CLADeletedRiders"; /*"SP_QuotePDFDetails";*/
                cmd.Parameters.AddWithValue("@ProposalNo", ProposalNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
            public DataSet FuncForAllRidersforallLives(string ProposalNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetCLAExclusion"; /*"SP_QuotePDFDetails";*/
                cmd.Parameters.AddWithValue("@ProposalNo", ProposalNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForC4(string ProposalNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_CLAChild4BenifitDetails"; /*"SP_QuotePDFDetails";*/
                cmd.Parameters.AddWithValue("@ProposalNo", ProposalNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForC3(string ProposalNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_CLAChild3BenifitDetails"; /*"SP_QuotePDFDetails";*/
                cmd.Parameters.AddWithValue("@ProposalNo", ProposalNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForC2(string ProposalNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_CLAChild2BenifitDetails"; /*"SP_QuotePDFDetails";*/
                cmd.Parameters.AddWithValue("@ProposalNo", ProposalNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForC1(string ProposalNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_CLAChild1BenifitDetails"; /*"SP_QuotePDFDetails";*/
                cmd.Parameters.AddWithValue("@ProposalNo", ProposalNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForMainLife(string ProposalNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_CLAMainLifeBenifitDetails"; /*"SP_QuotePDFDetails";*/
                cmd.Parameters.AddWithValue("@ProposalNo", ProposalNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForSpouseLife(string ProposalNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_CLASpouseBenifitDetails"; /*"SP_QuotePDFDetails";*/
                cmd.Parameters.AddWithValue("@ProposalNo", ProposalNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet CLAmemberDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_CLAMemberDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForMedicalReports(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetDocumentsForPDF";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForGetLoyaltyRewardValue(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetLoyaltyRewardValue";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForGetDFIllustration(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetDFIllustration";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForPensionBoosterDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetQuotePensionbooster";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FuncForGetMemberBenefitDetailsByRiderId(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetMemberBenefitDetailsByRiderId";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Added To get Doc String  ---By Aditya
        public DataSet FuncForGetDocString(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetDocumentString";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FunForQuatationSignatures(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_QuotationPDFSinatureDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FunForReportLabel(string ProductId, string Language, string ReportName)
        {
            try
            {
                // ReportName = "";
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetReportLabel";
                cmd.Parameters.AddWithValue("@ProductId", ProductId);
                cmd.Parameters.AddWithValue("@Language", Language);
                cmd.Parameters.AddWithValue("@ReportName", ReportName);
                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet FuncforMonthlyPensionBoosterDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_MonthlyPensionBooster";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FuncForYearPensionsDivDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_YearPensionDividendDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        //public DataSet GetReportLabel(string ProductCode, string Language, string Type)
        //{
        //    try
        //    {
        //        string PrefferedLanguage = "";
        //        if (Language == "1139" || Language == "Sinhala" || Language == "S")
        //        {
        //            PrefferedLanguage = "Sinhala";
        //        }
        //        else if (Language == "1138" || Language == "Tamil" || Language == "T")
        //        {
        //            PrefferedLanguage = "Tamil";
        //        }
        //        else
        //        {
        //            PrefferedLanguage = "English";
        //        }
        //        System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //        con.Open();
        //        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
        //        cmd.Connection = con;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "usp_GetReportLabel";
        //        cmd.Parameters.AddWithValue("@ProductCode", ProductCode);
        //        cmd.Parameters.AddWithValue("@Language", PrefferedLanguage);
        //        cmd.Parameters.AddWithValue("@ReportName", Type);

        //        DataSet ds = new DataSet();
        //        System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
        //        da.Fill(ds);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}



        #region UW Decision Reports
        public void ReportForUWDesicionLetter(string ProposalNo, string PreferredLanguage, string ReportType)
        {
            try
            {
                var ByteArray = GenererateUWReports(ProposalNo, PreferredLanguage, ReportType);
                RenderReports(ByteArray, ProposalNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Byte[] GenererateUWReports(string ProposalNo, string PreferredLanguage, string ReportType)
        {
            try
            {
                Microsoft.Reporting.WebForms.ReportViewer vr = new Microsoft.Reporting.WebForms.ReportViewer();
                DataSet dataset = UWDetails(ProposalNo);
                List<DataSet> dslst = new List<DataSet>();
                DataSet ds = new DataSet();
                dslst.Add(dataset);
                ReportParameter[] parameters;
                parameters = new ReportParameter[1];
                parameters[0] = new ReportParameter("ProposalNo", ProposalNo);
                if (ReportType == CrossCutting.CrossCuttingConstants.UWDecisionDecline)
                {
                    byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/UWDeclineReport.rdlc");
                    return bytes;
                }
                else if (ReportType == CrossCutting.CrossCuttingConstants.UWDecisionPostPone)
                {
                    byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/UWPostponeReport.rdlc");
                    return bytes;
                }
                else if (ReportType == CrossCutting.CrossCuttingConstants.UWDecisionNotTaken)
                {
                    byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/UWNTUReport.rdlc");
                    return bytes;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet UWDetails(string ProposalNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_UWDetails";
                cmd.Parameters.AddWithValue("@ProposalNo", ProposalNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Added For Illustration 
        public byte[] ReportForIllustration(string QuoteNo, string ProductCode, string PreferredLanguage)
        {
            byte[] bytes = null;
            try
            {

                Microsoft.Reporting.WebForms.ReportViewer vr = new Microsoft.Reporting.WebForms.ReportViewer();
                List<DataSet> dslst = new List<DataSet>();
                ReportParameter[] parameters;
                DataSet ds1 = new DataSet();
                ds1 = FuncForIllustration(QuoteNo);
                dslst.Add(ds1);
                parameters = new ReportParameter[1];
                parameters[0] = new ReportParameter("QuoteNo", QuoteNo);
                switch (ProductCode)
                {
                    case "PPG":
                        {
                            bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/IllustrationofEasypension.rdlc");
                        }
                        break;
                    case "PPH":
                        {
                            bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/IllustrationofSmartpensions.rdlc");
                        }
                        break;
                    case "EPB":
                        {
                            bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/IllustrationofEasypension.rdlc");
                        }
                        break;
                    case "HPA":
                        {
                            bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/IllustrationofHealthprotector.rdlc");
                        }
                        break;
                    case "SBB":
                        {
                            bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/IllustrationofSmartbuilder.rdlc");
                        }
                        break;
                    case "SBE":
                        {
                            bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/IllustrationofSmartbuildergold.rdlc");
                        }
                        break;
                    case "SBG": //"PPV"
                        {
                            bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/IllustrationofPriorityvalue.rdlc");
                        }
                        break;
                    default:
                        break;

                }


                return bytes;
            }
            catch (Exception ex)
            {
                ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                return null;
            }
        }
        #endregion
        public void ReportForPolicyIllustration(string PolicyNo, string QuoteNo, string ProductCode, string PreferredLanguage)
        {
            byte[] bytes = null;
            try
            {

                Microsoft.Reporting.WebForms.ReportViewer vr = new Microsoft.Reporting.WebForms.ReportViewer();
                ReportParameter[] parameters;
                DataSet dataset = FuncForPolicyDetailsPDF(PolicyNo);
                List<DataSet> dslst = new List<DataSet>();
                dslst.Add(dataset);
                DataSet ds2 = new DataSet();
                ds2 = FuncForPolicyBenefitDetailsPDF(PolicyNo);
                dslst.Add(ds2);
                DataSet ds3 = new DataSet();
                ds3 = FuncForDividendRates(QuoteNo);
                dslst.Add(ds3);
                DataSet ds4 = new DataSet();
                ds4 = FuncForIllustration(QuoteNo);
                dslst.Add(ds4);
                parameters = new ReportParameter[2];
                parameters[0] = new ReportParameter("PolicyNo", PolicyNo);
                parameters[1] = new ReportParameter("QuoteNo", QuoteNo);
                switch (ProductCode)
                {
                    case "PPG":
                        {
                            bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/IllustrationofEasypension.rdlc");
                        }
                        break;
                    case "PPH":
                        {
                            bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/IllustrationofSmartpensions.rdlc");
                        }
                        break;
                    case "PEP":
                        {
                            bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/IllustrationofEducationplan.rdlc");
                            RenderReports(bytes, QuoteNo);
                        }
                        break;
                    case "HPA":
                        {
                            bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/IllustrationofHealthprotector.rdlc");
                        }
                        break;
                    case "SBB":
                        {
                            bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/IllustrationofSmartbuilder.rdlc");
                        }
                        break;
                    case "SBE":
                        {
                            bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/IllustrationofSmartbuildergold.rdlc");
                        }
                        break;
                    case "PPV":
                        {
                            bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/IllustrationofPriorityvalue.rdlc");
                        }
                        break;
                    default:
                        break;

                }
                //return bytes;
            }
            catch (Exception ex)
            {
                ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                // return null;
            }
        }
        // GET: Reports
        public void ReportForCashQuotation(string QuoteNo, string ProductCode, string PreferredLanguage)
        {
            QuoteNo = CrossCutting_EncryptDecrypt.Decrypt(QuoteNo);
            ProductCode = CrossCutting_EncryptDecrypt.Decrypt(ProductCode);
            PreferredLanguage = CrossCutting_EncryptDecrypt.Decrypt(PreferredLanguage);
            RenderReports(ReportQuotation(QuoteNo, ProductCode, PreferredLanguage), QuoteNo);

        }

        public void ReportForProposalQuotation(string QuoteNo, string ProductCode, string PreferredLanguage)
        {
            try
            {
                Microsoft.Reporting.WebForms.ReportViewer vr = new Microsoft.Reporting.WebForms.ReportViewer();
                DataSet dataset = CreateQuote(QuoteNo);
                List<DataSet> dslst = new List<DataSet>();
                DataSet ds = new DataSet();
                dslst.Add(dataset);
                ReportParameter[] parameters;
                DataSet ds2 = new DataSet();
                ds2 = FuncForIllustration(QuoteNo);
                dslst.Add(ds2);
                DataSet ds3 = new DataSet();
                ds3 = FuncForDividendRates(QuoteNo);
                dslst.Add(ds3);
                DataSet ds4 = new DataSet();
                ds4 = FuncForMonthlyDrawDown(QuoteNo);
                dslst.Add(ds4);
                DataSet ds5 = new DataSet();
                ds5 = FuncForMainLifeBenefits(QuoteNo);
                dslst.Add(ds5);
                DataSet ds6 = new DataSet();
                ds6 = FuncForSpouseBenefits(QuoteNo);
                dslst.Add(ds6);
                DataSet ds7 = new DataSet();
                ds7 = FuncForChildBenefits(QuoteNo);
                dslst.Add(ds7);
                DataSet ds8 = new DataSet();
                ds8 = FuncForSmartPensionsPlanChoices(QuoteNo);
                dslst.Add(ds8);
                DataSet ds9 = new DataSet();
                ds9 = FuncForEveryYearDrawDownDetails(QuoteNo);
                dslst.Add(ds9);
                DataSet ds10 = new DataSet();
                ds10 = FuncForMemberBenefitDetails(QuoteNo);
                dslst.Add(ds10);
                DataSet ds11 = new DataSet();
                ds11 = FuncForMATURITY_BENEFIT_PREMIUM_PAYING_MODE(QuoteNo);
                dslst.Add(ds11);
                DataSet ds12 = new DataSet();
                ds12 = FuncForEducationPlan(QuoteNo);
                dslst.Add(ds12);
                DataSet ds13 = new DataSet();
                ds13 = FuncForEducationPlan1(QuoteNo);
                dslst.Add(ds13);
                DataSet ds14 = new DataSet();
                ds14 = FuncForEasyPensionsDailyPremium(QuoteNo);
                dslst.Add(ds14);
                DataSet ds15 = new DataSet();
                ds15 = FuncForEasyPensionspremium(QuoteNo);
                dslst.Add(ds15);
                DataSet ds16 = new DataSet();
                ds16 = FuncForDeathFundIllustration(QuoteNo);
                dslst.Add(ds16);
                DataSet ds17 = new DataSet();
                ds17 = FuncForMedicalReports(QuoteNo);
                dslst.Add(ds17);
                DataSet ds18 = new DataSet();
                ds18 = FuncForGetLoyaltyRewardValue(QuoteNo);
                dslst.Add(ds18);
                DataSet ds19 = new DataSet();
                ds19 = FuncForPensionBoosterDetails(QuoteNo);
                dslst.Add(ds19);
                DataSet ds20 = new DataSet();
                ds20 = FuncForGetMemberBenefitDetailsByRiderId(QuoteNo);
                dslst.Add(ds20);
                DataSet ds21 = new DataSet();
                ds21 = FunForQuatationSignatures(QuoteNo);
                dslst.Add(ds21);
                //added by Prasad for Smart Pension 24-Aug
                DataSet ds22 = new DataSet();
                ds22 = FuncforMonthlyPensionBoosterDetails(QuoteNo);
                dslst.Add(ds22);

                DataSet ds23 = new DataSet();
                ds23 = FuncForYearPensionsDivDetails(QuoteNo);
                dslst.Add(ds23);

                parameters = new ReportParameter[1];
                parameters[0] = new ReportParameter("QuoteNo", QuoteNo);

                if (PreferredLanguage == "1137")
                {
                    switch (ProductCode)
                    {
                        case "PPG":
                            {

                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforEasypension.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;

                        case "LLP":
                            {

                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforEasypension.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "PPH":
                        case "PSP":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforSmartpensions.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "EPB":
                        case "PEP":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforEducationplan.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "HPA":
                        case "PHP":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforHealthprotector.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "SBA":
                        case "SBB":
                        case "SBC":
                        case "SBD":
                        case "PSB":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforSmartBuilder.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "SBE":
                        case "SBF":
                        case "SMG":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforSmartbuildergold.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "SBG":
                        case "SBH":
                        case "PPV":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforPriorityvalue.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        default:
                            break;

                    }
                }
                else if (PreferredLanguage == "1138")
                {
                    switch (ProductCode)
                    {
                        case "PPG":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/EasypensionquotationinTamil.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "PSP":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/SmartpensionquotationinTamil.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "PEP":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/EducationplanquotationinTamil.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "PHP":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/HealthprotectorquotationinTamil.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "PSB":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/SmartbuilderquotationinTamil.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "SMG":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/SmartbuildergoldquotationinTamil.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "PPV":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforPriorityvalue.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        default:
                            break;

                    }

                }
                else
                {
                    switch (ProductCode)
                    {
                        case "PPG":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/EasypensionquotationinSinhala.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "PSP":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/SmartpensionquotationinSinhala.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "PEP":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/EducationplanquotationinSinhala.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "PHP":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/HealthprotectorquotationinSinhala.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "PSB":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/SmartbuilderquotationinSinhala.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "SMG":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/SmartbuildergoldquotationinSinhala.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "PPV":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforPriorityvalue.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        default:
                            break;

                    }

                }
            }
            catch (Exception ex)
            {
                ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                throw ex;
            }
        }
        public DataSet CreatePolicy(string PolicyNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_PolicyPDFDetails";
                cmd.Parameters.AddWithValue("@PolicyNo", PolicyNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Used for sending email on click of Save option of quotation
        /// </summary>
        /// <param name="QuoteNo"></param>
        /// <param name="ProductCode"></param>
        /// <param name="PreferredLanguage"></param>
        /// <returns></returns>
        public byte[] ReportQuotation(string QuoteNo, string ProductCode, string PreferredLanguage)
        {
            byte[] bytes = null;
            try
            {
                    Microsoft.Reporting.WebForms.ReportViewer vr = new Microsoft.Reporting.WebForms.ReportViewer();

                    DataSet dataset = CreateQuote(QuoteNo);
                    List<DataSet> dslst = new List<DataSet>();
                    DataSet ds = new DataSet();
                    dslst.Add(dataset);
                    ReportParameter[] parameters;
                    DataSet ds2 = new DataSet();
                    ds2 = FuncForIllustration(QuoteNo);
                    dslst.Add(ds2);
                    DataSet ds3 = new DataSet();
                    ds3 = FuncForDividendRates(QuoteNo);
                    dslst.Add(ds3);
                    DataSet ds4 = new DataSet();
                    ds4 = FuncForMonthlyDrawDown(QuoteNo);
                    dslst.Add(ds4);
                    DataSet ds5 = new DataSet();
                    ds5 = FuncForMainLifeBenefits(QuoteNo);
                    dslst.Add(ds5);
                    DataSet ds6 = new DataSet();
                    ds6 = FuncForSpouseBenefits(QuoteNo);
                    dslst.Add(ds6);
                    DataSet ds7 = new DataSet();
                    ds7 = FuncForChildBenefits(QuoteNo);
                    dslst.Add(ds7);
                    DataSet ds8 = new DataSet();
                    ds8 = FuncForSmartPensionsPlanChoices(QuoteNo);
                    dslst.Add(ds8);
                    DataSet ds9 = new DataSet();
                    ds9 = FuncForEveryYearDrawDownDetails(QuoteNo);
                    dslst.Add(ds9);
                    DataSet ds10 = new DataSet();
                    ds10 = FuncForMemberBenefitDetails(QuoteNo);
                    dslst.Add(ds10);
                    DataSet ds11 = new DataSet();
                    ds11 = FuncForMATURITY_BENEFIT_PREMIUM_PAYING_MODE(QuoteNo);
                    dslst.Add(ds11);
                    DataSet ds12 = new DataSet();
                    ds12 = FuncForEducationPlan(QuoteNo);
                    dslst.Add(ds12);
                    DataSet ds13 = new DataSet();
                    ds13 = FuncForEducationPlan1(QuoteNo);
                    dslst.Add(ds13);
                    DataSet ds14 = new DataSet();
                    ds14 = FuncForEasyPensionsDailyPremium(QuoteNo);
                    dslst.Add(ds14);
                    DataSet ds15 = new DataSet();
                    ds15 = FuncForEasyPensionspremium(QuoteNo);
                    dslst.Add(ds15);
                    DataSet ds16 = new DataSet();
                    ds16 = FuncForDeathFundIllustration(QuoteNo);
                    dslst.Add(ds16);
                    DataSet ds17 = new DataSet();
                    ds17 = FuncForMedicalReports(QuoteNo);
                    dslst.Add(ds17);
                    DataSet ds18 = new DataSet();
                    ds18 = FuncForGetLoyaltyRewardValue(QuoteNo);
                    dslst.Add(ds18);
                    DataSet ds19 = new DataSet();
                    ds19 = FuncForPensionBoosterDetails(QuoteNo);
                    dslst.Add(ds19);
                    DataSet ds20 = new DataSet();
                    ds20 = FuncForGetMemberBenefitDetailsByRiderId(QuoteNo);
                    dslst.Add(ds20);
                    DataSet ds21 = new DataSet();
                    ds21 = FunForQuatationSignatures(QuoteNo);
                    dslst.Add(ds21);
                    //DataSet ds22 = new DataSet();
                    //ds22 = FunForReportLabel(ProductCode, PreferredLanguage,"Quotation");
                    //dslst.Add(ds22);
                    //added by Prasad for smart pension changes - 24-aug
                    DataSet ds22 = new DataSet();
                    ds22 = FuncforMonthlyPensionBoosterDetails(QuoteNo);
                    dslst.Add(ds22);

                    DataSet ds23 = new DataSet();
                    ds23 = FuncForYearPensionsDivDetails(QuoteNo);
                    dslst.Add(ds23);

                    DataSet ds24 = new DataSet();
                    ds24 = GetReportLabel(ProductCode, PreferredLanguage, "Quotation");
                    dslst.Add(ds24);
                    DataSet ds25 = new DataSet();
                    ds25 = FuncForGetDocString(QuoteNo);
                    dslst.Add(ds25);

                parameters = new ReportParameter[1];
                    parameters[0] = new ReportParameter("QuoteNo", QuoteNo);

                    if (PreferredLanguage == "1137" || PreferredLanguage == "English"|| PreferredLanguage == "E")
                    {
                        switch (ProductCode)
                        {
                            case "PPG":
                                {

                                    bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforEasypension.rdlc");
                                }
                                break;
                        case "LLP":
                            {

                                bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforLiveLife.rdlc");
                            }
                            break;
                        case "PSP":
                            case "PPH":
                                {
                                    bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforSmartpensions.rdlc");
                                }
                                break;
                            case "PEP":
                            case "EPB":
                            case "EPA":
                                {
                                    bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforEducationplan.rdlc");
                                }
                                break;
                            case "PHP":
                            case "HPA":
                                {
                                    bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforHealthprotector.rdlc");
                                }
                                break;
                            case "PSB":
                            case "SBA":
                            case "SBB":
                            case "SBC":
                            case "SBD":
                                {
                                    bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforSmartBuilder.rdlc");
                                }
                                break;
                            case "SMG":
                            case "SBE":
                            case "SBF":
                                {
                                    bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforSmartbuildergold.rdlc");
                                }
                                break;
                            case "PPV":
                            case "SBG":
                            case "SBH":
                                {
                                    bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforPriorityvalue.rdlc");
                                }
                                break;
                            default:
                                break;

                        }
                    }
                    else if (PreferredLanguage == "1138" || PreferredLanguage == "Tamil"|| PreferredLanguage == "T")
                    {
                        switch (ProductCode)
                        {
                            case "PPG":
                                {
                                    bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/EasypensionquotationinTamil.rdlc");
                                }
                                break;
                            case "PSP":
                            case "PPH":
                                {
                                    bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/SmartpensionquotationinTamil.rdlc");
                                }
                                break;
                            case "PEP":
                            case "EPB":
                                {
                                    bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/EducationplanquotationinTamil.rdlc");
                                }
                                break;
                            case "PHP":
                            case "HPA":
                                {
                                    bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/HealthprotectorquotationinTamil.rdlc");
                                }
                                break;
                            case "PSB":
                            case "SBA":
                            case "SBB":
                            case "SBC":
                            case "SBD":
                                {
                                    bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/SmartbuilderquotationinTamil.rdlc");
                                }
                                break;
                            case "SMG":
                            case "SBE":
                            case "SBF":
                                {
                                    bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/SmartbuildergoldquotationinTamil.rdlc");
                                }
                                break;
                            case "PPV":
                            case "SBG":
                            case "SBH":
                                {
                                    bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforPriorityvalue.rdlc");
                                }
                                break;
                            default:
                                break;

                        }

                    }
                    else if (PreferredLanguage == "1139" || PreferredLanguage == "Sinhala"|| PreferredLanguage == "S")
                    {

                        switch (ProductCode)
                        {
                            case "PPG":
                                {
                                    bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/EasypensionquotationinSinhala.rdlc");
                                }
                                break;
                            case "PSP":
                            case "PPH":
                                {
                                    bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/SmartpensionquotationinSinhala.rdlc");
                                }
                                break;
                            case "PEP":
                            case "EPB":
                                {
                                    bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/EducationplanquotationinSinhala.rdlc");
                                }
                                break;
                            case "PHP":
                            case "HPA":
                                {
                                    bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/HealthprotectorquotationinSinhala.rdlc");
                                }
                                break;
                            case "PSB":
                            case "SBA":
                            case "SBB":
                            case "SBC":
                            case "SBD":
                                {
                                    bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/SmartbuilderquotationinSinhala.rdlc");
                                }
                                break;
                            case "SMG":
                            case "SBE":
                            case "SBF":
                                {
                                    bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/SmartbuildergoldquotationinSinhala.rdlc");
                                }
                                break;
                            case "PPV":
                            case "SBG":
                            case "SBH":
                                {
                                    bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforPriorityvalue.rdlc");
                                }
                                break;
                            default:
                                break;

                        }

                    }

                    return bytes;

            }
            catch (Exception ex)
            {
                ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                throw ex;
            }
        }

        public Byte[] GenerateSmartPensionQuotation(string QuoteNo)
        {
            XmlDocument doc = new XmlDocument();
            //var xml = Context.usp_getQuoteXml(QuoteNo).FirstOrDefault();
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            con.Open();
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_getQuoteXml";//usp_GetReportLabel
            cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
            //var xmlStr = "";
            //cmd.Parameters[0].Value = xmlStr;
            DataSet ds = new DataSet();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
            da.Fill(ds);
            if(ds.Tables.Count != 0)
            {
                var xmlString = ds.Tables[0].Rows[0][0].ToString();
                doc.LoadXml(xmlString);
            }

            if (doc != null)
            {
                XmlNode quoteDetails = doc.SelectSingleNode("LifeQQ/QuotePDFDetails/QuotePDFDetails");
                #region FullName
                string title = GetNodeText(quoteDetails, "Title");
                string firstName = GetNodeText(quoteDetails, "FirstName");
                string lastName = GetNodeText(quoteDetails, "LastName");
                XmlElement elePropFullName = doc.CreateElement("ProposerFullName");
                elePropFullName.InnerText = title + " " + firstName + " " + lastName;
                quoteDetails.AppendChild(elePropFullName);
                #endregion

                #region LKR_LIFE_SA_and_LKR_LIFE_SIB
                int AnnualizePremium = GetNodeInt(quoteDetails, "AnnualizePremium");
                int ADDITIONAL_LIFE_BENEFIT_SumInsured = GetNodeInt(quoteDetails, "ADDITIONAL_LIFE_BENEFIT_SumInsured ");
                int MonthlySurvivorIncome = GetNodeInt(quoteDetails, "MonthlySurvivorIncome");

                int LKR_LIFE_SA = (AnnualizePremium + ADDITIONAL_LIFE_BENEFIT_SumInsured) / 100 * (100 - MonthlySurvivorIncome);

                XmlElement eleLKR_LIFE_SA = doc.CreateElement("LKR_LIFE_SA");
                eleLKR_LIFE_SA.InnerText = LKR_LIFE_SA + "";
                quoteDetails.AppendChild(eleLKR_LIFE_SA);

                int LKR_LIFE_SIB = 0;
                if (MonthlySurvivorIncome != 0)
                {
                    LKR_LIFE_SIB = ((AnnualizePremium + ADDITIONAL_LIFE_BENEFIT_SumInsured) / 100) * MonthlySurvivorIncome;
                }

                XmlElement eleLKR_LIFE_SIB = doc.CreateElement("LKR_LIFE_SIB");
                eleLKR_LIFE_SIB.InnerText = LKR_LIFE_SIB + "";
                quoteDetails.AppendChild(eleLKR_LIFE_SIB);

                #endregion

                #region Hospitalization_Benefit_SumInsured2X
                int Hospitalization_Benefit_SumInsured = GetNodeInt(quoteDetails, "Hospitalization_Benefit_SumInsured");

                int Hospitalization_Benefit_SumInsured2X = Hospitalization_Benefit_SumInsured * 2;

                XmlElement eleHospitalization_Benefit_SumInsured2X = doc.CreateElement("Hospitalization_Benefit_SumInsured2X");
                eleHospitalization_Benefit_SumInsured2X.InnerText = Hospitalization_Benefit_SumInsured2X + "";
                quoteDetails.AppendChild(eleHospitalization_Benefit_SumInsured2X);

                #endregion



                #region Child_Benefits

                int Child_Hospitalization_Benefit_SumInsured = GetNodeInt(quoteDetails, "Child_Hospitalization_Benefit_SumInsured");

                int Child_Hospitalization_Benefit_SumInsured2X = Child_Hospitalization_Benefit_SumInsured * 2;

                XmlElement eleChild_Hospitalization_Benefit_SumInsured2X = doc.CreateElement("Child_Hospitalization_Benefit_SumInsured2X");
                eleChild_Hospitalization_Benefit_SumInsured2X.InnerText = Child_Hospitalization_Benefit_SumInsured2X + "";
                quoteDetails.AppendChild(eleChild_Hospitalization_Benefit_SumInsured2X);

                int Child_Health_Care_Benefit_SumInsured = GetNodeInt(quoteDetails, "Child_Health_Care_Benefit_SumInsured");

                double Child_Health_Care_Benefit_SumInsuredPerDay = Child_Hospitalization_Benefit_SumInsured * 0.01;

                XmlElement eleChild_Health_Care_Benefit_SumInsuredPerDay = doc.CreateElement("Child_Health_Care_Benefit_SumInsuredPerDay");
                eleChild_Health_Care_Benefit_SumInsuredPerDay.InnerText = Child_Health_Care_Benefit_SumInsuredPerDay + "";
                quoteDetails.AppendChild(eleChild_Health_Care_Benefit_SumInsuredPerDay);

                #endregion

                #region MonthlySurvivorIncome2


                int MonthlySurvivorIncome2 = 100 - MonthlySurvivorIncome;

                XmlElement eleMonthlySurvivorIncome2 = doc.CreateElement("MonthlySurvivorIncome2");
                eleMonthlySurvivorIncome2.InnerText = MonthlySurvivorIncome2 + "";
                quoteDetails.AppendChild(eleMonthlySurvivorIncome2);
                #endregion

                #region FuneralExpenseBenefit

                int BASIC_LIFE_COVER_AnnualPremium = GetNodeInt(quoteDetails, "BASIC_LIFE_COVER_AnnualPremium");
                int FuneralExpenseBenefit = BASIC_LIFE_COVER_AnnualPremium * 5;
                if (FuneralExpenseBenefit > 500000)
                {
                    FuneralExpenseBenefit = 500000;
                }

                XmlElement eleFuneralExpenseBenefit = doc.CreateElement("FuneralExpenseBenefit");
                eleFuneralExpenseBenefit.InnerText = FuneralExpenseBenefit + "";
                quoteDetails.AppendChild(eleFuneralExpenseBenefit);

                #endregion

                #region Today
                XmlElement eleToday = doc.CreateElement("Today");
                eleToday.InnerText = DateTime.Now.ToString("dd/MM/yyyy");
                quoteDetails.AppendChild(eleToday);
                #endregion


                #region LifeBenefits


                int TotalLifeBenefit = AnnualizePremium + ADDITIONAL_LIFE_BENEFIT_SumInsured;

                XmlElement eleTotalLifeBenefit = doc.CreateElement("TotalLifeBenefit");
                eleTotalLifeBenefit.InnerText = TotalLifeBenefit + "";
                quoteDetails.AppendChild(eleTotalLifeBenefit);

                int AmountofLifeBenefitoptedasSurvivorIncomeBenefit = (AnnualizePremium + ADDITIONAL_LIFE_BENEFIT_SumInsured) / 100 * MonthlySurvivorIncome;
                XmlElement eleAmountofLifeBenefitoptedasSurvivorIncomeBenefit = doc.CreateElement("Today");
                eleAmountofLifeBenefitoptedasSurvivorIncomeBenefit.InnerText = AmountofLifeBenefitoptedasSurvivorIncomeBenefit + "";
                quoteDetails.AppendChild(eleAmountofLifeBenefitoptedasSurvivorIncomeBenefit);


                #endregion

                #region TotalPremiumAndLoadingAmount

                XmlNodeList premiums = doc.SelectNodes("LifeQQ/MemberBenefitDetails/MemberBenefitDetails/Premium");
                XmlNodeList annualPremiums = doc.SelectNodes("LifeQQ/MemberBenefitDetails/MemberBenefitDetails/AnnualPremium");
                XmlNodeList loadingAmounts = doc.SelectNodes("LifeQQ/MemberBenefitDetails/MemberBenefitDetails/LoadingAmount");
                int TotalPremium = 0;
                foreach (XmlNode n in premiums)
                {
                    TotalPremium += Convert.ToInt32(n.InnerText);
                }
                int TotalAnnualPremium = 0;
                foreach (XmlNode n in annualPremiums)
                {
                    TotalAnnualPremium += Convert.ToInt32(n.InnerText);
                }
                int TotalLoadingAmount = 0;
                foreach (XmlNode n in loadingAmounts)
                {
                    TotalLoadingAmount += Convert.ToInt32(n.InnerText);
                }

                XmlElement _ele = doc.CreateElement("TotalPremium");
                _ele.InnerText = TotalPremium + "";
                quoteDetails.AppendChild(_ele);

                XmlElement _ele1 = doc.CreateElement("TotalAnnualPremium");
                _ele1.InnerText = TotalAnnualPremium + "";
                quoteDetails.AppendChild(_ele1);

                XmlElement _ele2 = doc.CreateElement("TotalLoadingAmount");
                _ele2.InnerText = TotalLoadingAmount + "";
                quoteDetails.AppendChild(_ele2);

                #endregion

                string xslPath = System.Web.HttpContext.Current.Server.MapPath(@"~/Reports/XSLT/QuoteSmartPension_en.xsl");
                Byte[] ByteArray = GenerateLetter(doc.InnerXml, xslPath);
                return ByteArray;
            }
            return null;
        }
        public void ReportForProposal(string QuoteNo, string ProductCode, string PrefferedLanguage)
        {
            try
            {
                Microsoft.Reporting.WebForms.ReportViewer vr = new Microsoft.Reporting.WebForms.ReportViewer();
                List<DataSet> dslst = new List<DataSet>();
                QuoteNo = CrossCutting_EncryptDecrypt.Decrypt(QuoteNo);
                ProductCode = CrossCutting_EncryptDecrypt.Decrypt(ProductCode);
                if (string.IsNullOrEmpty(PrefferedLanguage))
                {
                    PrefferedLanguage = "E";
                }
                else
                {
                PrefferedLanguage = CrossCutting_EncryptDecrypt.Decrypt(PrefferedLanguage);
                }
                #region DataSetListing
              
               
                    if (ProductCode != "PPG") //ProductCode != "PSP"
                    {
                        DataSet ds = new DataSet();
                        ds = FuncForMainLifeDetails(QuoteNo);
                        dslst.Add(ds);
                        DataSet ds2 = new DataSet();
                        ds2 = FuncForSpouseDetails(QuoteNo);
                        dslst.Add(ds2);
                        DataSet ds3 = new DataSet();
                        ds3 = FuncForPolicyOwnerDetails(QuoteNo);
                        dslst.Add(ds3);
                        DataSet ds4 = new DataSet();
                        ds4 = FuncForChildDetails(QuoteNo);
                        dslst.Add(ds4);
                        DataSet ds5 = new DataSet();
                        ds5 = FuncForBeneficiaryDetails(QuoteNo);
                        dslst.Add(ds5);
                        DataSet ds6 = new DataSet();
                        ds6 = FuncForPolicyDetails(QuoteNo);
                        dslst.Add(ds6);
                        DataSet ds7 = new DataSet();
                        ds7 = FuncForPolicyBenefitDetails(QuoteNo);
                        dslst.Add(ds7);
                        DataSet ds8 = new DataSet();
                        ds8 = FuncForProposalPremiumDetails(QuoteNo);
                        dslst.Add(ds8);
                        DataSet ds9 = new DataSet();
                        ds9 = FuncForSpouseHabitLifeStyle(QuoteNo);
                        dslst.Add(ds9);
                        DataSet ds10 = new DataSet();
                        ds10 = FuncForSpouseOtherInformation(QuoteNo);
                        dslst.Add(ds10);
                        DataSet ds11 = new DataSet();
                        ds11 = FuncForFemaleLivesInformation(QuoteNo);
                        dslst.Add(ds11);
                        DataSet ds12 = new DataSet();
                        ds12 = FuncForHabitMoreDetails(QuoteNo);
                        dslst.Add(ds12);
                        DataSet ds13 = new DataSet();
                        ds13 = FuncForStateOfHealth(QuoteNo);
                        dslst.Add(ds13);
                        DataSet ds14 = new DataSet();
                        ds14 = FuncForSelfFamilyHistory(QuoteNo);
                        dslst.Add(ds14);
                        DataSet ds15 = new DataSet();
                        ds15 = FuncForSpouseFamilyHistory(QuoteNo);
                        dslst.Add(ds15);
                        DataSet ds16 = new DataSet();
                        ds16 = FuncForPreviousPolicyPremium(QuoteNo);
                        dslst.Add(ds16);
                        DataSet ds17 = new DataSet();
                        ds17 = FuncBeneficiaryDetails(QuoteNo);
                        dslst.Add(ds17);
                        DataSet ds18 = new DataSet();
                        ds18 = FuncPolicyDocumentsDetails(QuoteNo);
                        dslst.Add(ds18);
                        DataSet ds19 = new DataSet();
                        ds19 = FuncPolicyDocumentWPSinatureDetails(QuoteNo);
                        dslst.Add(ds19);
                        DataSet ds20 = new DataSet();
                        ds20 = FuncPolicyDocumentSpouseSinatureDetails(QuoteNo);
                        dslst.Add(ds20);
                        DataSet ds21 = new DataSet();
                        ds21 = FuncOccupationHazardousworkDetails(QuoteNo);
                        dslst.Add(ds21);
                        DataSet ds22 = new DataSet();
                        ds22 = GetReportLabel(ProductCode, PrefferedLanguage, "Proposal");
                        dslst.Add(ds22);
                        DataSet ds23 = new DataSet();
                        ds23 = GetPreviousAndCurrentLifeInsuranceQuestion(QuoteNo);
                        dslst.Add(ds23);
                        DataSet ds24 = new DataSet();
                        ds24 = GetFamilyBackGroundQuestion(QuoteNo);
                        dslst.Add(ds24);
                        DataSet ds25 = new DataSet();
                        ds25 = GetChildMedicalHistoryQuestion(QuoteNo);
                        dslst.Add(ds25);
                        DataSet ds26 = new DataSet();
                        ds26 = GetPolicyMemberClaimInfo(QuoteNo);
                        dslst.Add(ds26);
                    }
                    else if (ProductCode == "PPG") //ProductCode == "PSP" ||
                    {
                        DataSet ds1 = new DataSet();
                        ds1 = FuncForMainLifeDetails(QuoteNo);
                        dslst.Add(ds1);
                        DataSet ds2 = new DataSet();
                        ds2 = FuncForSpouseDetails(QuoteNo);
                        dslst.Add(ds2);
                        DataSet ds3 = new DataSet();
                        ds3 = FuncForPolicyOwnerDetails(QuoteNo);
                        dslst.Add(ds3);
                        DataSet ds4 = new DataSet();
                        ds4 = FuncForChildDetails(QuoteNo);
                        dslst.Add(ds4);
                        DataSet ds5 = new DataSet();
                        ds5 = FuncBeneficiaryDetails(QuoteNo);
                        dslst.Add(ds5);
                        DataSet ds6 = new DataSet();
                        ds6 = FuncForPolicyDetails(QuoteNo);
                        dslst.Add(ds6);
                        DataSet ds7 = new DataSet();
                        ds7 = FuncForPolicyBenefitDetails(QuoteNo);
                        dslst.Add(ds7);
                        DataSet ds8 = new DataSet();
                        ds8 = FuncForSelfFamilyHistory(QuoteNo);
                        dslst.Add(ds8);
                        DataSet ds9 = new DataSet();
                        ds9 = FuncForSpouseFamilyHistory(QuoteNo);
                        dslst.Add(ds9);
                        DataSet ds10 = new DataSet();
                        ds10 = FuncForFemaleLivesInformation(QuoteNo);
                        dslst.Add(ds10);
                        DataSet ds11 = new DataSet();
                        ds11 = FuncForHabitMoreDetails(QuoteNo);
                        dslst.Add(ds11);
                        DataSet ds12 = new DataSet();
                        ds12 = FuncForSpouseHabitLifeStyle(QuoteNo);
                        dslst.Add(ds12);
                        DataSet ds13 = new DataSet();
                        ds13 = FuncForStateOfHealth(QuoteNo);
                        dslst.Add(ds13);
                        DataSet ds14 = new DataSet();
                        ds14 = FuncForSpouseOtherInformation(QuoteNo);
                        dslst.Add(ds14);
                        DataSet ds15 = new DataSet();
                        ds15 = FuncForProposalPremiumDetails(QuoteNo);
                        dslst.Add(ds15);
                        DataSet ds16 = new DataSet();
                        ds16 = FuncPolicyDocumentsDetails(QuoteNo);
                        dslst.Add(ds16);
                        DataSet ds17 = new DataSet();
                        ds17 = FuncPolicyDocumentWPSinatureDetails(QuoteNo);
                        dslst.Add(ds17);
                        DataSet ds18 = new DataSet();
                        ds18 = FuncOccupationHazardousworkDetails(QuoteNo);
                        dslst.Add(ds18);
                        DataSet ds19 = new DataSet();
                        ds19 = FuncPolicyDocumentSpouseSinatureDetails(QuoteNo);
                        dslst.Add(ds19);
                        DataSet ds20 = new DataSet();
                        ds20 = GetReportLabel(ProductCode, PrefferedLanguage, "Proposal");
                        dslst.Add(ds20);
                        DataSet ds21 = new DataSet();
                        ds21 = FuncForPreviousPolicyPremium(QuoteNo);
                        dslst.Add(ds21);
                        DataSet ds22 = new DataSet();
                        ds22 = GetPreviousAndCurrentLifeInsuranceQuestion(QuoteNo);
                        dslst.Add(ds22);
                        DataSet ds23 = new DataSet();
                        ds23 = GetFamilyBackGroundQuestion(QuoteNo);
                        dslst.Add(ds23);
                        DataSet ds24 = new DataSet();
                        ds24 = GetPolicyMemberClaimInfo(QuoteNo);
                        dslst.Add(ds24);
                        DataSet ds25 = new DataSet();
                        ds25 = GetPolicyOwnerDetailsEP(QuoteNo);
                        dslst.Add(ds25);
                    }


                    #endregion

                    #region Old
                    //DataSet ds = new DataSet();
                    //ds = FuncForMainLifeDetails(QuoteNo);
                    //dslst.Add(ds);
                    //DataSet ds2 = new DataSet();
                    //ds2 = FuncForSpouseDetails(QuoteNo);
                    //dslst.Add(ds2);
                    //DataSet ds3 = new DataSet();
                    //ds3 = FuncForPolicyOwnerDetails(QuoteNo);
                    //dslst.Add(ds3);
                    //DataSet ds4 = new DataSet();
                    //ds4 = FuncForChildDetails(QuoteNo);
                    //dslst.Add(ds4);
                    //DataSet ds5 = new DataSet();
                    //ds5 = FuncForBeneficiaryDetails(QuoteNo);
                    //dslst.Add(ds5);
                    //DataSet ds6 = new DataSet();
                    //ds6 = FuncForPolicyDetails(QuoteNo);
                    //dslst.Add(ds6);
                    //DataSet ds7 = new DataSet();
                    //ds7 = FuncForPolicyBenefitDetails(QuoteNo);
                    //dslst.Add(ds7);
                    //DataSet ds8 = new DataSet();
                    //ds8 = FuncForProposalPremiumDetails(QuoteNo);
                    //dslst.Add(ds8);
                    //DataSet ds9 = new DataSet();
                    //ds9 = FuncForSpouseHabitLifeStyle(QuoteNo);
                    //dslst.Add(ds9);
                    //DataSet ds10 = new DataSet();
                    //ds10 = FuncForSpouseOtherInformation(QuoteNo);
                    //dslst.Add(ds10);
                    //DataSet ds11 = new DataSet();
                    //ds11 = FuncForFemaleLivesInformation(QuoteNo);
                    //dslst.Add(ds11);
                    //DataSet ds12 = new DataSet();
                    //ds12 = FuncForHabitMoreDetails(QuoteNo);
                    //dslst.Add(ds12);
                    //DataSet ds13 = new DataSet();
                    //ds13 = FuncForStateOfHealth(QuoteNo);
                    //dslst.Add(ds13);
                    //DataSet ds14 = new DataSet();
                    //ds14 = FuncForSelfFamilyHistory(QuoteNo);
                    //dslst.Add(ds14);
                    //DataSet ds15 = new DataSet();
                    //ds15 = FuncForSpouseFamilyHistory(QuoteNo);
                    //dslst.Add(ds15);
                    //DataSet ds16=new DataSet();
                    //ds16 = FuncForPreviousPolicyPremium(QuoteNo);
                    //dslst.Add(ds16);

                    #endregion
                    ReportParameter[] parameters = new ReportParameter[1];

                    parameters[0] = new ReportParameter("QuoteNo", QuoteNo);

                    //byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/ProposalforEasypension.rdlc");
                    //RenderReports(bytes, "ProposalPDF");
                    switch (ProductCode)
                    {
                        case "PPG":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/ProposalforEasypension.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "PSP":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/ProposalforSmartpensions.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "PEP":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/ProposalforEducationplan.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "PHP":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/ProposalforHealthprotector.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "PSB":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/ProposalforSmartbuilder.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "SMG":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/ProposalforSmartbuildergold.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "PPV":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/ProposalforPriorityvalue.rdlc");
                                RenderReports(bytes, QuoteNo);
                            }
                            break;
                        default:
                            break;

                    }
               
            }
            catch (Exception ex)
            {
                ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                throw ex;
            }
        }


        // Medical letter RDLC
        public byte[] PendingRequirementsProposalMedicalLetter(string QuoteNo, string ProductCode, string MedicalLabName)
        {
            try
            {
                Microsoft.Reporting.WebForms.ReportViewer vr = new Microsoft.Reporting.WebForms.ReportViewer();
                List<DataSet> dslst = new List<DataSet>();
                #region DataSetListing
                DataSet ds1 = new DataSet();
                ds1 = FuncForMainLifeDetails(QuoteNo);
                dslst.Add(ds1);
                DataSet ds2 = new DataSet();
                ds2 = FuncMedicalForPolicyOwnerDetails(QuoteNo);
                dslst.Add(ds2);
                DataSet ds3 = new DataSet();
                ds3 = FuncMedicalForMedicalLabsDetails(MedicalLabName);
                dslst.Add(ds3);
                DataSet ds4 = new DataSet();
                ds4 = FuncMedicalForMedicalLabDocumentTypesDetails(QuoteNo);
                dslst.Add(ds4);
                DataSet ds5 = new DataSet();
                ds5 = FuncMedicalForMedicalLabProductNameDetails(QuoteNo);
                dslst.Add(ds5);
                DataSet ds6 = new DataSet();
                ds6 = FuncMedicalForSelfDetails(QuoteNo);
                dslst.Add(ds6);

                #endregion

                ReportParameter[] parameters = new ReportParameter[2];

                parameters[0] = new ReportParameter("QuoteNo", QuoteNo);

                parameters[1] = new ReportParameter("MedicalLabName", MedicalLabName);

                byte[] bytes = GenerateMedicalRDLCReports(dslst, parameters, @"~/Reports/ProposalMedicalLetter.rdlc");
                //RenderReports(bytes, QuoteNo);
                return bytes;
            }
            catch (Exception ex)
            {
                ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                throw ex;
            }
        }
        //Email PDF 
        public byte[] ProposalReport(string QuoteNo, string ProductCode, string PrefferedLanguage)
        {
            byte[] bytes = null;

            try
            {
                
               
                    Microsoft.Reporting.WebForms.ReportViewer vr = new Microsoft.Reporting.WebForms.ReportViewer();
                    List<DataSet> dslst = new List<DataSet>();

                    if (ProductCode != "PPG") //ProductCode != "PSP"
                    {
                        DataSet ds = new DataSet();
                        ds = FuncForMainLifeDetails(QuoteNo);
                        dslst.Add(ds);
                        DataSet ds2 = new DataSet();
                        ds2 = FuncForSpouseDetails(QuoteNo);
                        dslst.Add(ds2);
                        DataSet ds3 = new DataSet();
                        ds3 = FuncForPolicyOwnerDetails(QuoteNo);
                        dslst.Add(ds3);
                        DataSet ds4 = new DataSet();
                        ds4 = FuncForChildDetails(QuoteNo);
                        dslst.Add(ds4);
                        DataSet ds5 = new DataSet();
                        ds5 = FuncForBeneficiaryDetails(QuoteNo);
                        dslst.Add(ds5);
                        DataSet ds6 = new DataSet();
                        ds6 = FuncForPolicyDetails(QuoteNo);
                        dslst.Add(ds6);
                        DataSet ds7 = new DataSet();
                        ds7 = FuncForPolicyBenefitDetails(QuoteNo);
                        dslst.Add(ds7);
                        DataSet ds8 = new DataSet();
                        ds8 = FuncForProposalPremiumDetails(QuoteNo);
                        dslst.Add(ds8);
                        DataSet ds9 = new DataSet();
                        ds9 = FuncForSpouseHabitLifeStyle(QuoteNo);
                        dslst.Add(ds9);
                        DataSet ds10 = new DataSet();
                        ds10 = FuncForSpouseOtherInformation(QuoteNo);
                        dslst.Add(ds10);
                        DataSet ds11 = new DataSet();
                        ds11 = FuncForFemaleLivesInformation(QuoteNo);
                        dslst.Add(ds11);
                        DataSet ds12 = new DataSet();
                        ds12 = FuncForHabitMoreDetails(QuoteNo);
                        dslst.Add(ds12);
                        DataSet ds13 = new DataSet();
                        ds13 = FuncForStateOfHealth(QuoteNo);
                        dslst.Add(ds13);
                        DataSet ds14 = new DataSet();
                        ds14 = FuncForSelfFamilyHistory(QuoteNo);
                        dslst.Add(ds14);
                        DataSet ds15 = new DataSet();
                        ds15 = FuncForSpouseFamilyHistory(QuoteNo);
                        dslst.Add(ds15);
                        DataSet ds16 = new DataSet();
                        ds16 = FuncForPreviousPolicyPremium(QuoteNo);
                        dslst.Add(ds16);
                        DataSet ds17 = new DataSet();
                        ds17 = FuncBeneficiaryDetails(QuoteNo);
                        dslst.Add(ds17);
                        DataSet ds18 = new DataSet();
                        ds18 = FuncPolicyDocumentsDetails(QuoteNo);
                        dslst.Add(ds18);
                        DataSet ds19 = new DataSet();
                        ds19 = FuncPolicyDocumentWPSinatureDetails(QuoteNo);
                        dslst.Add(ds19);
                        DataSet ds20 = new DataSet();
                        ds20 = FuncPolicyDocumentSpouseSinatureDetails(QuoteNo);
                        dslst.Add(ds20);
                        DataSet ds21 = new DataSet();
                        ds21 = FuncOccupationHazardousworkDetails(QuoteNo);
                        dslst.Add(ds21);
                        DataSet ds22 = new DataSet();
                        ds22 = GetReportLabel(ProductCode, PrefferedLanguage, "Proposal");
                        dslst.Add(ds22);
                        DataSet ds23 = new DataSet();
                        ds23 = GetPreviousAndCurrentLifeInsuranceQuestion(QuoteNo);
                        dslst.Add(ds23);
                        DataSet ds24 = new DataSet();
                        ds24 = GetFamilyBackGroundQuestion(QuoteNo);
                        dslst.Add(ds24);
                        DataSet ds25 = new DataSet();
                        ds25 = GetChildMedicalHistoryQuestion(QuoteNo);
                        dslst.Add(ds25);
                        DataSet ds26 = new DataSet();
                        ds26 = GetPolicyMemberClaimInfo(QuoteNo);
                        dslst.Add(ds26);

                    }
                    else if (ProductCode == "PPG") //ProductCode == "PSP" ||
                    {
                        DataSet ds1 = new DataSet();
                        ds1 = FuncForMainLifeDetails(QuoteNo);
                        dslst.Add(ds1);
                        DataSet ds2 = new DataSet();
                        ds2 = FuncForSpouseDetails(QuoteNo);
                        dslst.Add(ds2);
                        DataSet ds3 = new DataSet();
                        ds3 = FuncForPolicyOwnerDetails(QuoteNo);
                        dslst.Add(ds3);
                        DataSet ds4 = new DataSet();
                        ds4 = FuncForChildDetails(QuoteNo);
                        dslst.Add(ds4);
                        DataSet ds5 = new DataSet();
                        ds5 = FuncBeneficiaryDetails(QuoteNo);
                        dslst.Add(ds5);
                        DataSet ds6 = new DataSet();
                        ds6 = FuncForPolicyDetails(QuoteNo);
                        dslst.Add(ds6);
                        DataSet ds7 = new DataSet();
                        ds7 = FuncForPolicyBenefitDetails(QuoteNo);
                        dslst.Add(ds7);
                        DataSet ds8 = new DataSet();
                        ds8 = FuncForSelfFamilyHistory(QuoteNo);
                        dslst.Add(ds8);
                        DataSet ds9 = new DataSet();
                        ds9 = FuncForSpouseFamilyHistory(QuoteNo);
                        dslst.Add(ds9);
                        DataSet ds10 = new DataSet();
                        ds10 = FuncForFemaleLivesInformation(QuoteNo);
                        dslst.Add(ds10);
                        DataSet ds11 = new DataSet();
                        ds11 = FuncForHabitMoreDetails(QuoteNo);
                        dslst.Add(ds11);
                        DataSet ds12 = new DataSet();
                        ds12 = FuncForSpouseHabitLifeStyle(QuoteNo);
                        dslst.Add(ds12);
                        DataSet ds13 = new DataSet();
                        ds13 = FuncForStateOfHealth(QuoteNo);
                        dslst.Add(ds13);
                        DataSet ds14 = new DataSet();
                        ds14 = FuncForSpouseOtherInformation(QuoteNo);
                        dslst.Add(ds14);
                        DataSet ds15 = new DataSet();
                        ds15 = FuncForProposalPremiumDetails(QuoteNo);
                        dslst.Add(ds15);
                        DataSet ds16 = new DataSet();
                        ds16 = FuncPolicyDocumentsDetails(QuoteNo);
                        dslst.Add(ds16);
                        DataSet ds17 = new DataSet();
                        ds17 = FuncPolicyDocumentWPSinatureDetails(QuoteNo);
                        dslst.Add(ds17);
                        DataSet ds18 = new DataSet();
                        ds18 = FuncOccupationHazardousworkDetails(QuoteNo);
                        dslst.Add(ds18);
                        DataSet ds19 = new DataSet();
                        ds19 = FuncPolicyDocumentSpouseSinatureDetails(QuoteNo);
                        dslst.Add(ds19);
                        DataSet ds20 = new DataSet();
                        ds20 = GetReportLabel(ProductCode, PrefferedLanguage, "Proposal");
                        dslst.Add(ds20);
                        DataSet ds21 = new DataSet();
                        ds21 = FuncForPreviousPolicyPremium(QuoteNo);
                        dslst.Add(ds21);
                        DataSet ds22 = new DataSet();
                        ds22 = GetPreviousAndCurrentLifeInsuranceQuestion(QuoteNo);
                        dslst.Add(ds22);
                        DataSet ds23 = new DataSet();
                        ds23 = GetFamilyBackGroundQuestion(QuoteNo);
                        dslst.Add(ds23);
                        DataSet ds24 = new DataSet();
                        ds24 = GetPolicyMemberClaimInfo(QuoteNo);
                        dslst.Add(ds24);
                        DataSet ds25 = new DataSet();
                        ds25 = GetPolicyOwnerDetailsEP(QuoteNo);
                        dslst.Add(ds25);

                    }

                    //DataSet ds = new DataSet();
                    //ds = FuncForMainLifeDetails(QuoteNo);
                    //dslst.Add(ds);
                    //DataSet ds2 = new DataSet();
                    //ds2 = FuncForSpouseDetails(QuoteNo);
                    //dslst.Add(ds2);
                    //DataSet ds3 = new DataSet();
                    //ds3 = FuncForPolicyOwnerDetails(QuoteNo);
                    //dslst.Add(ds3);
                    //DataSet ds4 = new DataSet();
                    //ds4 = FuncForChildDetails(QuoteNo);
                    //dslst.Add(ds4);
                    //DataSet ds5 = new DataSet();
                    //ds5 = FuncForBeneficiaryDetails(QuoteNo);
                    //dslst.Add(ds5);
                    //DataSet ds6 = new DataSet();
                    //ds6 = FuncForPolicyDetails(QuoteNo);
                    //dslst.Add(ds6);
                    //DataSet ds7 = new DataSet();
                    //ds7 = FuncForPolicyBenefitDetails(QuoteNo);
                    //dslst.Add(ds7);
                    //DataSet ds8 = new DataSet();
                    //ds8 = FuncForProposalPremiumDetails(QuoteNo);
                    //dslst.Add(ds8);
                    //DataSet ds9 = new DataSet();
                    //ds9 = FuncForSpouseHabitLifeStyle(QuoteNo);
                    //dslst.Add(ds9);
                    //DataSet ds10 = new DataSet();
                    //ds10 = FuncForSpouseOtherInformation(QuoteNo);
                    //dslst.Add(ds10);
                    //DataSet ds11 = new DataSet();
                    //ds11 = FuncForFemaleLivesInformation(QuoteNo);
                    //dslst.Add(ds11);
                    //DataSet ds12 = new DataSet();
                    //ds12 = FuncForHabitMoreDetails(QuoteNo);
                    //dslst.Add(ds12);
                    //DataSet ds13 = new DataSet();
                    //ds13 = FuncForStateOfHealth(QuoteNo);
                    //dslst.Add(ds13);
                    //DataSet ds14 = new DataSet();
                    //ds14 = FuncForSelfFamilyHistory(QuoteNo);
                    //dslst.Add(ds14);
                    //DataSet ds15 = new DataSet();
                    //ds15 = FuncForSpouseFamilyHistory(QuoteNo);
                    //dslst.Add(ds15);
                    //DataSet ds16 = new DataSet();
                    //ds16 = FuncForPreviousPolicyPremium(QuoteNo);
                    //dslst.Add(ds16);
                    ReportParameter[] parameters = new ReportParameter[1];

                    parameters[0] = new ReportParameter("QuoteNo", QuoteNo);

                    //byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/ProposalforEasypension.rdlc");
                    //RenderReports(bytes, "ProposalPDF");
                    switch (ProductCode)
                    {
                        case "PPG":
                            {
                                bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/ProposalforEasypension.rdlc");
                                //RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "PPH":
                        case "PSP":
                            {
                                bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/ProposalforSmartpensions.rdlc");
                                //RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "EPB":
                        case "EPA":
                        case "PEP":
                            {
                                bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/ProposalforEducationplan.rdlc");
                                //RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "HPA":
                        case "PHP":
                            {
                                bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/ProposalforHealthprotector.rdlc");
                                //RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "SBA":
                        case "SBB":
                        case "SBC":
                        case "SBD":
                        case "PSB":
                            {
                                bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/ProposalforSmartbuilder.rdlc");
                                //RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "SBE":
                        case "SBF":
                        case "SMG":
                            {
                                bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/ProposalforSmartbuildergold.rdlc");
                                //RenderReports(bytes, QuoteNo);
                            }
                            break;
                        case "SBH":
                        case "SBG":
                        case "PPV":
                            {
                                bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/ProposalforPriorityvalue.rdlc");
                                //RenderReports(bytes, QuoteNo);
                            }
                            break;
                        default:
                            break;

                    }
                    return bytes;
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Proposal DataSets

        public DataSet FuncMedicalForMedicalLabProductNameDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_MedicalLabProductNameDetails ";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FuncMedicalForSelfDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_MemberLevelPaidBy";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FuncMedicalForMedicalLabDocumentTypesDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_MedicalLabDocumentTypesDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncMedicalForMedicalLabsDetails(string MedicalLabName)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_MedicalLabsDetails";
                cmd.Parameters.AddWithValue("@MedicalLabName", MedicalLabName);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncMedicalForPolicyOwnerDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_MedicalLetterPolicyOwnerDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForMainLifeDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_MainLifeDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForSpouseDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_spouseLifeDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForChildDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_ChildLifeDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForPolicyOwnerDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_PolicyOwnerDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForBeneficiaryDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_ReportPF_BeneficiaryDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForPolicyDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_PolicyDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForPolicyBenefitDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_ReportPF_PolicyBenefitDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForProposalPremiumDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_ProposalPremiumDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Spouse Habit LifeStyle & Spouse Other Info
        public DataSet FuncForSpouseHabitLifeStyle(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_ReportPF_HabitLifeStyle";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForSpouseOtherInformation(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_ReportPF_OtherInfo";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForFemaleLivesInformation(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_ReportPF_FemaleLives";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //till here
        public DataSet FuncForHabitMoreDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_ReportPF_HabitMoreDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForStateOfHealth(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_ReportPF_StateofHealth";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Family History
        public DataSet FuncForSelfFamilyHistory(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_ReportPF_FamilyHistory_Self";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForSpouseFamilyHistory(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_ReportPF_FamilyHistory_Spouse";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForPreviousPolicyPremium(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetOngoingProposalDetails_pdf";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        //quotation->emailPDF()->sendemailwithPdfAttachment()->16/05/2018
        public string SendEmailWithPdfAttachment(string QuoteNo, string ProductCode, string PreferredLanguage)//, int? MemberID
        {
            try
            {
                string directryPath = Server.MapPath("~/QuotePdfDoc");
                QuoteNo = CrossCutting_EncryptDecrypt.Decrypt(QuoteNo);
                ProductCode = CrossCutting_EncryptDecrypt.Decrypt(ProductCode);
                PreferredLanguage = CrossCutting_EncryptDecrypt.Decrypt(PreferredLanguage);
                //If folder for pdf doesnot exist then it will provide error as folder doesnot exist.
                if (!Directory.Exists(directryPath))
                {
                    Directory.CreateDirectory(directryPath);
                }
                string filePath = Path.Combine(directryPath, QuoteNo + DateTime.Now.Ticks + ".pdf");

                Microsoft.Reporting.WebForms.ReportViewer vr = new Microsoft.Reporting.WebForms.ReportViewer();
                DataSet dataset = CreateQuote(QuoteNo);
                List<DataSet> dslst = new List<DataSet>();
                DataSet ds = new DataSet();
                dslst.Add(dataset);
                ReportParameter[] parameters;
                DataSet ds2 = new DataSet();
                ds2 = FuncForIllustration(QuoteNo);
                dslst.Add(ds2);
                DataSet ds3 = new DataSet();
                ds3 = FuncForDividendRates(QuoteNo);
                dslst.Add(ds3);
                DataSet ds4 = new DataSet();
                ds4 = FuncForMonthlyDrawDown(QuoteNo);
                dslst.Add(ds4);
                DataSet ds5 = new DataSet();
                ds5 = FuncForMainLifeBenefits(QuoteNo);
                dslst.Add(ds5);
                DataSet ds6 = new DataSet();
                ds6 = FuncForSpouseBenefits(QuoteNo);
                dslst.Add(ds6);
                DataSet ds7 = new DataSet();
                ds7 = FuncForChildBenefits(QuoteNo);
                dslst.Add(ds7);
                DataSet ds8 = new DataSet();
                ds8 = FuncForSmartPensionsPlanChoices(QuoteNo);
                dslst.Add(ds8);
                DataSet ds9 = new DataSet();
                ds9 = FuncForEveryYearDrawDownDetails(QuoteNo);
                dslst.Add(ds9);
                DataSet ds10 = new DataSet();
                ds10 = FuncForMemberBenefitDetails(QuoteNo);
                dslst.Add(ds10);
                DataSet ds11 = new DataSet();
                ds11 = FuncForMATURITY_BENEFIT_PREMIUM_PAYING_MODE(QuoteNo);
                dslst.Add(ds11);
                DataSet ds12 = new DataSet();
                ds12 = FuncForEducationPlan(QuoteNo);
                dslst.Add(ds12);
                DataSet ds13 = new DataSet();
                ds13 = FuncForEducationPlan1(QuoteNo);
                dslst.Add(ds13);
                DataSet ds14 = new DataSet();
                ds14 = FuncForEasyPensionsDailyPremium(QuoteNo);
                dslst.Add(ds14);
                DataSet ds15 = new DataSet();
                ds15 = FuncForEasyPensionspremium(QuoteNo);
                dslst.Add(ds15);
                DataSet ds16 = new DataSet();
                ds16 = FuncForDeathFundIllustration(QuoteNo);
                dslst.Add(ds16);
                DataSet ds17 = new DataSet();
                ds17 = FuncForMedicalReports(QuoteNo);
                dslst.Add(ds17);
                DataSet ds18 = new DataSet();
                ds18 = FuncForGetLoyaltyRewardValue(QuoteNo);
                dslst.Add(ds18);
                DataSet ds19 = new DataSet();
                ds19 = FuncForPensionBoosterDetails(QuoteNo);
                dslst.Add(ds19);
                DataSet ds20 = new DataSet();
                ds20 = FuncForGetMemberBenefitDetailsByRiderId(QuoteNo);
                dslst.Add(ds20);
                DataSet ds21 = new DataSet();
                ds21 = FunForQuatationSignatures(QuoteNo);
                dslst.Add(ds21);
                DataSet ds23 = new DataSet();
                ds23 = FuncforMonthlyPensionBoosterDetails(QuoteNo);
                dslst.Add(ds23);

                parameters = new ReportParameter[1];
                parameters[0] = new ReportParameter("QuoteNo", QuoteNo);

                if (PreferredLanguage == "1137")
                {
                    switch (ProductCode)
                    {
                        case "PPG":
                            {

                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforEasypension.rdlc");

                                RenderReport(bytes, filePath);
                            }
                            break;
                        case "PSP":
                            {

                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforSmartpensions.rdlc");

                                RenderReport(bytes, filePath);
                            }
                            break;
                        case "PEP":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforEducationplan.rdlc");

                                RenderReport(bytes, filePath);
                            }
                            break;
                        case "PHP":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforHealthprotector.rdlc");

                                RenderReport(bytes, filePath);
                            }
                            break;
                        case "PSB":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforSmartBuilder.rdlc");

                                RenderReport(bytes, filePath);
                            }
                            break;
                        case "SMG":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforSmartbuildergold.rdlc");

                                RenderReport(bytes, filePath);
                            }
                            break;
                        case "PPV":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforPriorityvalue.rdlc");

                                RenderReport(bytes, filePath);
                            }
                            break;
                        default:
                            break;

                    }
                }
                else if (PreferredLanguage == "1138")
                {
                    switch (ProductCode)
                    {
                        case "PPG":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/EasypensionquotationinTamil.rdlc");

                                RenderReport(bytes, filePath);
                            }
                            break;
                        case "PSP":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/SmartpensionquotationinTamil.rdlc");

                                RenderReport(bytes, filePath);
                            }
                            break;
                        case "PEP":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/EducationplanquotationinTamil.rdlc");

                                RenderReport(bytes, filePath);
                            }
                            break;
                        case "PHP":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/HealthprotectorquotationinTamil.rdlc");

                                RenderReport(bytes, filePath);
                            }
                            break;
                        case "PSB":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/SmartbuilderquotationinTamil.rdlc");

                                RenderReport(bytes, filePath);
                            }
                            break;
                        case "SMG":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/SmartbuildergoldquotationinTamil.rdlc");

                                RenderReport(bytes, filePath);
                            }
                            break;
                        case "PPV":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforPriorityvalue.rdlc");

                                RenderReport(bytes, filePath);
                            }
                            break;
                        default:
                            break;

                    }

                }
                else
                {
                    switch (ProductCode)
                    {
                        case "PPG":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/EasypensionquotationinSinhala.rdlc");

                                RenderReport(bytes, filePath);
                            }
                            break;
                        case "PSP":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/SmartpensionquotationinSinhala.rdlc");

                                RenderReport(bytes, filePath);
                            }
                            break;
                        case "PEP":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/EducationplanquotationinSinhala.rdlc");

                                RenderReport(bytes, filePath);
                            }
                            break;
                        case "PHP":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/HealthprotectorquotationinSinhala.rdlc");

                                RenderReport(bytes, filePath);
                            }
                            break;
                        case "PSB":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/SmartbuilderquotationinSinhala.rdlc");

                                RenderReport(bytes, filePath);
                            }
                            break;
                        case "SMG":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/SmartbuildergoldquotationinSinhala.rdlc");

                                RenderReport(bytes, filePath);
                            }
                            break;
                        case "PPV":
                            {
                                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/QuotationforPriorityvalue.rdlc");

                                RenderReport(bytes, filePath);

                            }
                            break;
                        default:
                            break;

                    }

                }
                SendEmail(QuoteNo, filePath);
                return "Email pdf sent successfully";
            }
            catch (Exception ex)
            {
                ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                // log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                // Logger.Error(ex);
                tblLog objlogoutput = new tblLog();
                objlogoutput.Date = DateTime.Now;
                objlogoutput.Thread = "101";
                objlogoutput.Level = "Error";
                objlogoutput.Logger = Convert.ToString(Logger);
                objlogoutput.Message = "Email pdf sending failed reason: " + ex;
                objlogoutput.ErrorCode = Codes.GetErrorCode();
                Context.tblLogs.Add(objlogoutput);
                Context.SaveChanges();

                return "Email pdf sending failed reason: " + ex;

                //throw ex;
            }
        }

        public void RenderReport(byte[] bytes, string filePath)
        {
            try
            {
                if (bytes != null)
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write))
                    {
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet CreateProposal(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_ProposalPDF";
                cmd.Parameters.AddWithValue("@ProposalNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet CreateQuote(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_QuotePDFDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetDividendRates(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Dividendrates";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FuncForIllustration(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetQuoteIllustration";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForPolicyDetailsPDF(string PolicyNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_PolicyDetails_PDF";
                cmd.Parameters.AddWithValue("@PolicyNo", PolicyNo);
                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public DataSet FuncForPolicyBenefitDetailsPDF(string PolicyNo)
        //{
        //    try
        //    {
        //        System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //        con.Open();
        //        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
        //        cmd.Connection = con;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "usp_PolicyBefenitDetails_PDF";
        //        cmd.Parameters.AddWithValue("@PolicyNo", PolicyNo);
        //        DataSet ds = new DataSet();
        //        System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
        //        da.Fill(ds);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public DataSet FuncForEveryYearDrawDownDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_EveryYearDrawDownDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForMemberBenefitDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetMemberBenefitDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForMATURITY_BENEFIT_PREMIUM_PAYING_MODE(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MATURITY_BENEFIT_PREMIUM_PAYING_MODE";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForEducationPlan(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetMemberBenefitDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForEducationPlan1(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetBenefitDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForMainLifeBenefits(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_CLAMainLifeBenifitDetails1";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForSpouseBenefits(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_CLASpouseBenifitDetails1";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForSmartPensionsPlanChoices(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_SmartPensionsPlanChoices";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForEasyPensionspremium(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetMemberBenefitDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForDeathFundIllustration(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetDFIllustration";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForEasyPensionsDailyPremium(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_EveryYearDrawDownDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForChildBenefits(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_CLAChild1BenifitDetails1";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForDividendRates(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_Dividendrates";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForMonthlyDrawDown(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_MonthlyDrawDownDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForMonthlyPensionBooster(string PolicyNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_MonthlyPensionBooster";
                cmd.Parameters.AddWithValue("@QuoteNo", PolicyNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForQuotePensionBooster(string PolicyNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetQuotePensionbooster";
                cmd.Parameters.AddWithValue("@QuoteNo", PolicyNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForLoyalityReward(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetLoyaltyRewardValue";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public DataSet GetEmailAddress(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_GetEmailAddress";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public byte[] GenerateRDLCReportsMail(List<System.Data.DataSet> dsPayementStmt, ReportParameter[] parameters, string ReportPath)
        {
            try
            {
                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;
                ReportViewer viewer = new ReportViewer();

                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.ReportPath = System.Web.Hosting.HostingEnvironment.MapPath(ReportPath);

                //viewer.LocalReport.ReportPath = Server.MapPath(ReportPath);
                if (parameters != null)
                {
                    viewer.LocalReport.SetParameters(parameters);
                }
                viewer.LocalReport.DataSources.Clear();
                int count = 0;
                foreach (DataSet item in dsPayementStmt)
                {
                    count++;
                    viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet" + count, item.Tables[0]));
                }
                viewer.LocalReport.Refresh();
                byte[] bytes;
                try
                {
                    bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                    return bytes;
                }
                catch (Exception ex)
                {
                    ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                    log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                    Logger.Error(ex);
                    throw ex;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public byte[] GenerateRDLCReports(List<System.Data.DataSet> dsPayementStmt, ReportParameter[] parameters, string ReportPath)
        {
            try
            {
                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;
                ReportViewer viewer = new ReportViewer();

                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.ReportPath = System.Web.Hosting.HostingEnvironment.MapPath(ReportPath);
                //viewer.LocalReport.ReportPath = Server.MapPath(ReportPath);
                if (parameters != null)
                {
                    viewer.LocalReport.SetParameters(parameters);
                }
                viewer.LocalReport.DataSources.Clear();
                int count = 0;
                foreach (DataSet item in dsPayementStmt)
                {
                    count++;
                    viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet" + count, item.Tables[0]));
                }
                viewer.LocalReport.Refresh();
                byte[] bytes;
                try
                {
                    bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                    return bytes;
                }
                catch (Exception ex)
                {
                    ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                    log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                    Logger.Error(ex);
                    throw ex;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public byte[] GenerateMedicalRDLCReports(List<System.Data.DataSet> dsPayementStmt, ReportParameter[] parameters, string ReportPath)
        {
            try
            {
                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;
                ReportViewer viewer = new ReportViewer();

                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.ReportPath = System.Web.Hosting.HostingEnvironment.MapPath(ReportPath);
                //viewer.LocalReport.ReportPath = Server.MapPath(ReportPath);
                //if (parameters != null)
                //{
                //    viewer.LocalReport.SetParameters(parameters);
                //}
                viewer.LocalReport.DataSources.Clear();
                int count = 0;
                foreach (DataSet item in dsPayementStmt)
                {
                    count++;
                    viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet" + count, item.Tables[0]));
                }
                viewer.LocalReport.Refresh();
                byte[] bytes;
                try
                {
                    bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                    return bytes;
                }
                catch (Exception ex)
                {
                    ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                    log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                    Logger.Error(ex);
                    throw ex;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RenderReports(byte[] bytes, string fileName)
        {
            try
            {
                if (bytes != null)
                {

                    Response.Clear();
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".pdf");
                    Response.OutputStream.Write(bytes, 0, bytes.Length);
                    Response.Flush();
                    try
                    {
                        Response.End();
                    }

                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        public DataSet CashAdvanceQuotation(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_CashAdvanceQuote";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                return null;
            }
        }

        public DataSet CashQuotationBenefits(string QuoteNo)//int? MemberID
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_CashAdvanceQuoteBenefitDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                //cmd.Parameters.AddWithValue("@MemberID", MemberID);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);

                return ds;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public byte[] GenerateRDLCReports1(string ReportPath)
        {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            ReportViewer viewer = new ReportViewer();

            viewer.ProcessingMode = ProcessingMode.Local;

            viewer.LocalReport.ReportPath = Server.MapPath(ReportPath);

            viewer.LocalReport.Refresh();
            byte[] bytes;
            try
            {
                bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                return bytes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public void ReportForCashQuotation1()
        {
            try
            {
                Microsoft.Reporting.WebForms.ReportViewer vr = new Microsoft.Reporting.WebForms.ReportViewer();

                byte[] bytes = GenerateRDLCReports1(@"~/Reports/MedicalExaminerReportForm.rdlc");
                RenderReports(bytes, "Medical Examiner Report");

                // byte[] bytes = GenerateRDLCReports1(@"~/Reports/MedicalReqtCoveringLettertoLab.rdlc");
                // RenderReports(bytes, "Medical Reqt Covering Letter Lab Report");

                // byte[] bytes1 = GenerateRDLCReports1(@"~/Reports/MedicalReqtCoveringLettertoAssured.rdlc");
                // RenderReports(bytes1, "Medical Reqt Covering Letter Assured Report");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public void SendEmail(string QuoteNo, string filePath)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    DataSet dataset = GetEmailAddress(QuoteNo);

                    string Emailto = "";

                    foreach (DataTable table in dataset.Tables)
                    {
                        foreach (DataRow dr in table.Rows)
                        {
                            Emailto = dr["EmailID"].ToString();
                        }
                    }
                    //System.IO.File.Na
                    //string FileName = Path.GetFileName(filePath);
                    byte[] ByteArray = System.IO.File.ReadAllBytes(filePath);
                    EmailDetails objEmail = new EmailDetails();
                    objEmail.EmailID = Emailto;
                    objEmail.MailTemplate = "T002";
                    objEmail.Subject = "Quotation PDF Attachment";
                    objEmail.QuoteNumber = QuoteNo;
                    objEmail.ByteArray = ByteArray;
                    bool isQuotationPool = true;
                    //objEmail.QuotePdfFileName = filePath;
                    // SmtpClient objClient = new SmtpClient();
                    //MailMessage message = new MailMessage();
                    //string fromEmailAddress = string.Empty;
                    // string FilePath = string.Empty;
                    // message.To.Add(Emailto);

                    //message.Subject = "Quotation PDF Attachment";
                    //message.Body = "PFA";
                    //test code
                    //if (objClient.Port == 0 || objClient.Host == string.Empty)
                    //{
                    //    objClient.Port = 587;
                    //    objClient.Host = "smtp.office365.com";
                    //}
                    //Attachment att_Success = new Attachment(new MemoryStream(filePath);
                    Attachment attachment = new Attachment(filePath);
                    //    System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(filePath);
                    //message.Attachments.Add(attachment);
                    try
                    {

                        //EmailIntegration ObjEmailIntegration = new EmailIntegration();
                        //ObjEmailIntegration.EmailNotification(objEmail, isQuotationPool);
                        //objClient.Send(message);

                    }
                    catch (SmtpException ex)
                    {
                        ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                        // log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                        // Logger.Error(ex);
                        tblLog objlogoutput = new tblLog();
                        objlogoutput.Date = DateTime.Now;
                        objlogoutput.Thread = "101";
                        objlogoutput.Level = "Error";
                        objlogoutput.Logger = Convert.ToString(Logger);
                        objlogoutput.Message = "Email pdf sending failed reason: " + ex;
                        objlogoutput.ErrorCode = Codes.GetErrorCode();
                        Context.tblLogs.Add(objlogoutput);
                        Context.SaveChanges();

                    }

                }
            }

            catch (Exception ex)
            {
                ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                // log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                // Logger.Error(ex);
                tblLog objlogoutput = new tblLog();
                objlogoutput.Date = DateTime.Now;
                objlogoutput.Thread = "101";
                objlogoutput.Level = "Error";
                objlogoutput.Logger = Convert.ToString(Logger);
                objlogoutput.Message = "Email pdf sending failed reason: " + ex;
                objlogoutput.ErrorCode = Codes.GetErrorCode();
                Context.tblLogs.Add(objlogoutput);
                Context.SaveChanges();

                //return "Email pdf sending failed reason: " + ex;
            }
        }
        /// <summary>
        /// Method To Get UW Decision Report
        /// </summary>
        /// <param name="QuoteNo"></param>
        /// <returns></returns>
        public ActionResult UWDecisionReport(string ProposalNo)
        {
            try
            {
                //  Test To Purpose need to  Remmove Once Functionality is Comepleted
                UWDecisionReport objUWReport = new UWDecisionReport();
                objUWReport.Product = "Product Name";
                objUWReport.Commencement = DateTime.Now;
                objUWReport.PolicyNo = "P123467777";

                objUWReport.objMemberDetails = new List<AssuredMembers>();
                AssuredMembers objAssuredMember = new AssuredMembers();
                objAssuredMember.Age = 23;
                objAssuredMember.AssuredName = "MainLife";
                objAssuredMember.Occupation = "Teacher";
                objAssuredMember.Name = "Raza";
                objAssuredMember.MedicalSAR = "34";
                objAssuredMember.FinancialSAR = "22";
                objAssuredMember.ListMedicalRequirements = new List<string> { "Blood Test", "Urine Test", "ECG", "CT Scan" };
                objAssuredMember.ListFianacialRequirements = new List<string> { "Form 16", "PAN CARD" };

                objAssuredMember.objUWDeviations = new List<UWDeviationInfo>();

                UWDeviationInfo obj1 = new UWDeviationInfo();
                obj1.DeviationMessage = "Dummy";
                obj1.Decision = "Approve";
                obj1.Date = DateTime.Now;
                obj1.Remarks = "test";
                obj1.User = "User1";
                objAssuredMember.objUWDeviations.Add(obj1);
                UWDeviationInfo obj2 = new UWDeviationInfo();
                obj2.DeviationMessage = "Dummy";
                obj2.Decision = "Approve";
                obj2.Date = DateTime.Now;
                obj2.Remarks = "test";
                obj2.User = "User1";
                objAssuredMember.objUWDeviations.Add(obj2);
                objUWReport.objMemberDetails.Add(objAssuredMember);

                AssuredMembers objAssuredMember1 = new AssuredMembers();
                objAssuredMember1.Age = 23;
                objAssuredMember1.AssuredName = "Spouse";
                objAssuredMember1.Occupation = "Teacher";
                objAssuredMember1.Name = "Raza";
                objAssuredMember1.MedicalSAR = "34";
                objAssuredMember1.FinancialSAR = "22";
                objAssuredMember1.ListMedicalRequirements = new List<string> { "Blood Test", "Urine Test", "ECG", "CT Scan" };
                objAssuredMember1.ListFianacialRequirements = new List<string> { "Form 16", "PAN CARD" };

                objAssuredMember1.objUWDeviations = new List<UWDeviationInfo>();

                UWDeviationInfo obj3 = new UWDeviationInfo();
                obj3.DeviationMessage = "Dummy";
                obj3.Decision = "Approve";
                obj3.Date = DateTime.Now;
                obj3.Remarks = "test";
                obj3.User = "User1";
                objAssuredMember1.objUWDeviations.Add(obj3);
                UWDeviationInfo obj4 = new UWDeviationInfo();
                obj4.DeviationMessage = "Dummy";
                obj4.Decision = "Approve";
                obj4.Date = DateTime.Now;
                obj4.Remarks = "test";
                obj4.User = "User1";
                objAssuredMember1.objUWDeviations.Add(obj4);
                objUWReport.objMemberDetails.Add(objAssuredMember1);

                // Rider Info
                objUWReport.objLstRiderInfo = new List<RiderInfo>();
                RiderInfo objRider = new RiderInfo();
                objRider.RiderName = "Test1";
                objRider.objBenefitMemberRelationship = new List<RiderRelation>();
                RiderRelation objr1 = new RiderRelation();
                objr1.Assured_Name = "MainLife";
                objr1.RiderCurrentSI = "345555";
                objr1.RiderTotalSI = "985555";
                RiderRelation objr2 = new RiderRelation();
                objr2.Assured_Name = "Spouse";
                objr2.RiderCurrentSI = "345555";
                objr2.RiderTotalSI = "985555";
                objRider.objBenefitMemberRelationship.Add(objr1);
                objRider.objBenefitMemberRelationship.Add(objr2);

                RiderInfo objRider2 = new RiderInfo();
                objRider2.RiderName = "Test1";
                objRider2.objBenefitMemberRelationship = new List<RiderRelation>();
                RiderRelation objr3 = new RiderRelation();
                objr3.Assured_Name = "MainLife";
                objr3.RiderCurrentSI = "345555";
                objr3.RiderTotalSI = "985555";
                RiderRelation objr4 = new RiderRelation();
                objr4.Assured_Name = "Spouse";
                objr4.RiderCurrentSI = "345555";
                objr4.RiderTotalSI = "985555";
                objRider2.objBenefitMemberRelationship.Add(objr3);
                objRider2.objBenefitMemberRelationship.Add(objr4);

                objUWReport.objLstRiderInfo.Add(objRider);
                objUWReport.objLstRiderInfo.Add(objRider2);

                objUWReport.Decision = "Approved";
                objUWReport.DateTime = DateTime.Now;
                return View(objUWReport);
            }
            catch (Exception ex)
            {
                ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                return null;
            }
            // Till here


            //UWDecisionReport objUWReport = new UWDecisionReport();
            //objUWReport.ProposalNo = ProposalNo;
            //objUWReport = WebApiLogic.GetPostComplexTypeToAPI<Life.Models.Reports.UWDecisionReport>(objUWReport, "FetchDataForUWDecisionReport", "ReportsApi");            
        }

        public ActionResult UWDecisionReportPdf(string QuoteNo)
        {
            try
            {
                var report = new ActionAsPdf("UWDecisionReport", new { QuoteNo = QuoteNo });
                return report;
            }
            catch (Exception ex)
            {
                ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                return null;
            }
        }
        public Byte[] RetirementCalcPdf(int ContactID)
        {
            try
            {
                AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
                objProspect.ProspectStage = 4;// Set Stage as Need Analysis
                objProspect.ContactID = ContactID;
                objProspect = objProspectBusiness.LoadContactInformation(objProspect);

                var pdf = new ViewAsPdf("RetirementCalcReport", objProspect);
                Byte[] pdfData = pdf.BuildPdf(ControllerContext);
                return pdfData;
            }
            catch (Exception ex)
            {
                ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                return null;
            }
        }
        public ActionResult RetirementCalcReport(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            //AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            //objProspect.ProspectStage = 4;// Set Stage as Need Analysis
            //objProspect.ContactID = ContactID;
            //objProspect = objProspectBusiness.LoadContactInformation(objProspect);
            objProspect.CreatedBy = _username;
            objProspect.WPPhone = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.MobileNo).FirstOrDefault();
            objProspect.WPCode = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.AgentCode).FirstOrDefault();
            return new ViewAsPdf(objProspect) { FileName = "RetirementCalculator.pdf" };
        }
        public ActionResult HealthCalcPdf(int ContactID)
        {
            var report = new ActionAsPdf("HealthCalcReport", new { ContactID = ContactID });
            return report;
        }
        public ActionResult HealthCalcReport(int ContactID)
        {
            AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            objProspect.ProspectStage = 4;// Set Stage as Need Analysis
            objProspect.ContactID = ContactID;
            objProspect = objProspectBusiness.LoadContactInformation(objProspect);
            objProspect.CreatedBy = _username;
            objProspect.WPPhone = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.MobileNo).FirstOrDefault();
            objProspect.WPCode = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.AgentCode).FirstOrDefault();
            objProspect.objNeedAnalysis.adversities = String.Join(",", objProspect.objNeedAnalysis.objadversities);
            //objProspect.objNeedAnalysis.dlladversities.Add(new SelectListItem { Text = "Critical Illnesses" });
            //objProspect.objNeedAnalysis.dlladversities.Add(new SelectListItem { Text = "Excess Payments/Taxes" });
            //objProspect.objNeedAnalysis.dlladversities.Add(new SelectListItem { Text = "Loss of Income" });
            //objProspect.objNeedAnalysis.dlladversities.Add(new SelectListItem { Text = "Major Surgeries" });
            //objProspect.objNeedAnalysis.dlladversities.Add(new SelectListItem { Text = "Pre and Post Hospitalization Expenses" });
            return new ViewAsPdf(objProspect) { FileName = "HealthCalculator.pdf" };
        }
        public ActionResult EduCalcPdf(int ContactID)
        {
            var report = new ActionAsPdf("EduCalcReport", new { ContactID = ContactID });
            return report;
        }
        public ActionResult EduCalcReport(int ContactID)
        {
            AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            objProspect.ProspectStage = 4;// Set Stage as Need Analysis
            objProspect.ContactID = ContactID;
            objProspect = objProspectBusiness.LoadContactInformation(objProspect);
            objProspect.CreatedBy = _username;
            objProspect.WPPhone = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.MobileNo).FirstOrDefault();
            objProspect.WPCode = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.AgentCode).FirstOrDefault();
            for (int i = 0; i < objProspect.objNeedAnalysis.objGCEAL.Count; i++)
            {
                objProspect.objNeedAnalysis.objGCEAL[i].Relationship = FirstCharToUpper(objProspect.objNeedAnalysis.objGCEAL[i].Relationship);
            }
            for (int i = 0; i < objProspect.objNeedAnalysis.objLocal.Count; i++)
            {
                objProspect.objNeedAnalysis.objLocal[i].Relationship = FirstCharToUpper(objProspect.objNeedAnalysis.objLocal[i].Relationship);
            }
            for (int i = 0; i < objProspect.objNeedAnalysis.objHigherEdu.Count; i++)
            {
                objProspect.objNeedAnalysis.objHigherEdu[i].Relationship = FirstCharToUpper(objProspect.objNeedAnalysis.objHigherEdu[i].Relationship);
            }
            for (int i = 0; i < objProspect.objNeedAnalysis.objHigherForeign.Count; i++)
            {
                objProspect.objNeedAnalysis.objHigherForeign[i].Relationship = FirstCharToUpper(objProspect.objNeedAnalysis.objHigherForeign[i].Relationship);
            }
            return new ViewAsPdf(objProspect) { FileName = "EducationCalculator.pdf" };
        }
        public string FirstCharToUpper(string value)
        {
            value = value.ToLower();
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.  
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.  
            // ... Uppercase the lowercase letters following spaces.  
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }
        public ActionResult SavingCalcPdf(int ContactID)
        {
            var report = new ActionAsPdf("SavingCalcReport", new { ContactID = ContactID });
            return report;
        }
        public ActionResult SavingCalcReport(int ContactID)
        {
            try
            {
                AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
                objProspect.ProspectStage = 4;// Set Stage as Need Analysis
                objProspect.ContactID = ContactID;
                objProspect = objProspectBusiness.LoadContactInformation(objProspect);
                objProspect.CreatedBy = _username;
                objProspect.WPPhone = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.MobileNo).FirstOrDefault();
                objProspect.WPCode = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.AgentCode).FirstOrDefault();
                for (int i = 0; i < objProspect.objNeedAnalysis.objWedding.Count; i++)
                {
                    objProspect.objNeedAnalysis.objWedding[i].Relationship = FirstCharToUpper(objProspect.objNeedAnalysis.objWedding[i].Relationship);
                }
                for (int i = 0; i < objProspect.objNeedAnalysis.objHouse.Count; i++)
                {
                    objProspect.objNeedAnalysis.objHouse[i].Relationship = FirstCharToUpper(objProspect.objNeedAnalysis.objHouse[i].Relationship);
                }
                for (int i = 0; i < objProspect.objNeedAnalysis.objCar.Count; i++)
                {
                    objProspect.objNeedAnalysis.objCar[i].Relationship = FirstCharToUpper(objProspect.objNeedAnalysis.objCar[i].Relationship);
                }
                for (int i = 0; i < objProspect.objNeedAnalysis.objForeignTour.Count; i++)
                {
                    objProspect.objNeedAnalysis.objForeignTour[i].Relationship = FirstCharToUpper(objProspect.objNeedAnalysis.objForeignTour[i].Relationship);
                }
                for (int i = 0; i < objProspect.objNeedAnalysis.objOthers.Count; i++)
                {
                    objProspect.objNeedAnalysis.objOthers[i].Relationship = FirstCharToUpper(objProspect.objNeedAnalysis.objOthers[i].Relationship);
                }
                return new ViewAsPdf(objProspect) { FileName = "SavingCalculator.pdf" };
            }
            catch (Exception e)
            {
                return View();
            }
        }
        public ActionResult HumanValueCalcPdf(int ContactID)
        {
            var report = new ActionAsPdf("HumanValueCalcReport", new { ContactID = ContactID });
            return report;
        }
        public ActionResult HumanValueCalcReport(int ContactID)
        {
            AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            objProspect.ProspectStage = 4;// Set Stage as Need Analysis
            objProspect.ContactID = ContactID;
            objProspect = objProspectBusiness.LoadContactInformation(objProspect);
            objProspect.CreatedBy = _username;
            objProspect.WPPhone = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.MobileNo).FirstOrDefault();
            objProspect.WPCode = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.AgentCode).FirstOrDefault();
            return new ViewAsPdf(objProspect) { FileName = "HumanValueCalculator.pdf" };
        }
        public ActionResult FNAPdf(int ContactID)
        {
            var report = new ActionAsPdf("FNAReport", new { ContactID = ContactID });
            return report;
        }
        public ActionResult FNAReport(int ContactID)
        {
            AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            objProspect.ProspectStage = 4;// Set Stage as Need Analysis
            objProspect.ContactID = ContactID;
            objProspect = objProspectBusiness.LoadContactInformation(objProspect);
            objProspect.CreatedBy = _username;
            objProspect.WPPhone = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.MobileNo).FirstOrDefault();
            objProspect.WPCode = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.AgentCode).FirstOrDefault();
            return new ViewAsPdf(objProspect) { FileName = "FNA.pdf" };
        }
        //public ActionResult FNAEmail(int ContactID) 
        //{
        //    AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
        //    objProspect.ProspectStage = 4;// Set Stage as Need Analysis
        //    objProspect.ContactID = ContactID;
        //    objProspect = WebApiLogic.GetPostComplexTypeToAPI<Prospect>(objProspect, "LoadContactInformation", "SuspectApi");

        //    objProspect.ByteArray = FNAPdf(objProspect);
        //}
        public ActionResult NeedIdentifyPdf(int ContactID)
        {
            var report = new ActionAsPdf("NeedIdentifyReport", new { ContactID = ContactID });
            return report;
        }
        public ActionResult NeedIdentifyReport(int ContactID)
        {
            AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            objProspect.ProspectStage = 4;// Set Stage as Need Analysis
            objProspect.ContactID = ContactID;
            objProspect = objProspectBusiness.LoadContactInformation(objProspect);

            return View(objProspect);
        }

        public byte[] ReportForIllustrationPDF(string QuoteNo, string ProductCode, string PreferredLanguage, string PolicyNo)
        {
            byte[] bytes = null;
            try
            {
                Microsoft.Reporting.WebForms.ReportViewer vr = new Microsoft.Reporting.WebForms.ReportViewer();
                List<DataSet> dslst = new List<DataSet>();
                ReportParameter[] parameters;
                DataSet ds1 = new DataSet();
                ds1 = FuncForIllustration(PolicyNo);
                dslst.Add(ds1);
                DataSet ds2 = new DataSet();
                //ds2 = CreatePolicy(QuoteNo);
                ds2 = CreateQuote(QuoteNo);
                dslst.Add(ds2);
                DataSet ds3 = new DataSet();
                ds3 = FuncForDividendRates(PolicyNo);
                dslst.Add(ds3);
                DataSet ds4 = new DataSet();
                ds4 = FuncForMonthlyDrawDown(PolicyNo);
                dslst.Add(ds4);               
                DataSet ds5 = new DataSet();
                ds5 = FuncForPolicyDetailsPDF(PolicyNo);
                dslst.Add(ds5);
                DataSet ds6 = new DataSet();
                ds6 = FuncForGetLoyaltyRewardValue(QuoteNo);
                dslst.Add(ds6);
                DataSet ds7 = new DataSet();
                ds7 = FuncForGetDFIllustration(PolicyNo);
                dslst.Add(ds7);
                DataSet ds8 = new DataSet();
                ds8 = FuncForQuotePensionBooster(PolicyNo);
                dslst.Add(ds8);
                DataSet ds9 = new DataSet();
                ds9 = FuncForLoyalityReward(QuoteNo);
                dslst.Add(ds9);
                DataSet ds10 = new DataSet();
                ds10 = FuncForMonthlyPensionBooster(PolicyNo);
                dslst.Add(ds10);
                parameters = new ReportParameter[2];
                parameters[0] = new ReportParameter("QuoteNo", QuoteNo);
                parameters[1] = new ReportParameter("PolicyNo", PolicyNo);
                switch (ProductCode)
                {
                    case "PPG":
                        {
                            bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/IllustrationofEasypension.rdlc");
                        }
                        break;
                    case "PPH":
                    case "PSP":
                        {
                            bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/IllustrationofSmartpensions.rdlc");
                        }
                        break;
                    case "EPB":
                    case "EPA":
                    case "PEP":
                        {
                            bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/IllustrationofEducationplan.rdlc");
                        }
                        break;
                    case "HPA":
                    case "PHP":
                        {
                            bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/IllustrationofHealthprotector.rdlc");
                        }
                        break;
                    case "SBB":
                    case "SBA":
                    case "SBC":
                    case "SBD":
                    case "PSB":
                        {
                            bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/IllustrationofSmartbuilder.rdlc");
                        }
                        break;
                    case "SBE":
                    case "SBF":
                    case "SMG":
                        {
                            bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/IllustrationofSmartbuildergold.rdlc");
                        }
                        break;
                    case "SBG":
                    case "SBH":
                    case "PPV":
                        {
                            bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/IllustrationofPriorityvalue.rdlc");
                        }
                        break;
                    default:
                        break;

                }
                // return bytes;
            }
            catch (Exception ex)
            {
                ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
            }
            return bytes;
        }

        #region NewDataSets_11.06.2018

        public DataSet FuncBeneficiaryDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_BeneficiaryDetails";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet FuncPolicyDocumentsDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_PolicyDocumentsDetails";
                //cmd.CommandType = CommandType.Text;
                //cmd.CommandText = "SELECT B.DocumentUploadID,A.PolicyID,B.[File],B.FilePath,B.FileName,B.ItemType,B.DocumentType FROM tblPolicy  as A INNER JOIN tblPolicyDocuments as B ON A.PolicyID = B.PolicyID where A.QuoteNo='"+QuoteNo+"'";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FuncPolicyDocumentWPSinatureDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_PolicyDocumentsWPSinatureDetails";
                //cmd.CommandType = CommandType.Text;
                //cmd.CommandText = "SELECT B.DocumentUploadID,A.PolicyID,B.[File],B.FilePath,B.FileName,B.ItemType,B.DocumentType FROM tblPolicy  as A INNER JOIN tblPolicyDocuments as B ON A.PolicyID = B.PolicyID where A.QuoteNo='"+QuoteNo+"'";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FuncPolicyDocumentSpouseSinatureDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_PolicyDocumentsSpouseSinatureDetails";
                //cmd.CommandType = CommandType.Text;
                //cmd.CommandText = "SELECT B.DocumentUploadID,A.PolicyID,B.[File],B.FilePath,B.FileName,B.ItemType,B.DocumentType FROM tblPolicy  as A INNER JOIN tblPolicyDocuments as B ON A.PolicyID = B.PolicyID where A.QuoteNo='"+QuoteNo+"'";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FuncOccupationHazardousworkDetails(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_ReportPF_OccupationHazardousworkQuestion";
                //cmd.CommandType = CommandType.Text;
                //cmd.CommandText = "SELECT B.DocumentUploadID,A.PolicyID,B.[File],B.FilePath,B.FileName,B.ItemType,B.DocumentType FROM tblPolicy  as A INNER JOIN tblPolicyDocuments as B ON A.PolicyID = B.PolicyID where A.QuoteNo='"+QuoteNo+"'";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion



        //public DataSet FuncForPolicyDetailsPDF(string PolicyNo)
        //{
        //    try
        //    {
        //        System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //        con.Open();
        //        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
        //        cmd.Connection = con;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "usp_PolicyDetails_PDF";
        //        cmd.Parameters.AddWithValue("@PolicyNo", PolicyNo);
        //        DataSet ds = new DataSet();
        //        System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
        //        da.Fill(ds);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public DataSet FuncForPolicyBenefitDetailsPDF(string PolicyNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_PolicyBefenitDetails_PDF";
                cmd.Parameters.AddWithValue("@PolicyNo", PolicyNo);
                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private static byte[] concatAndAddContent(List<byte[]> pdf)
        //{
        //    byte[] all;

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        Document doc = new Document();

        //        PdfWriter writer = PdfWriter.GetInstance(doc, ms);
        //        doc.SetPageSize(PageSize.A4);
        //        doc.Open();
        //        PdfContentByte cb = writer.DirectContent;
        //        PdfImportedPage page;

        //        PdfReader reader;
        //        foreach (byte[] p in pdf)
        //        {
        //            reader = new PdfReader(p);
        //            int pages = reader.NumberOfPages;

        //            // loop over document pages
        //            for (int i = 1; i <= pages; i++)
        //            {
        //                doc.SetPageSize(PageSize.A4);
        //                doc.NewPage();
        //                page = writer.GetImportedPage(reader, i);
        //                cb.AddTemplate(page, 0, 0);
        //            }
        //        }

        //        doc.Close();
        //        all = ms.GetBuffer();
        //        ms.Flush();
        //        ms.Dispose();
        //    }
        //    return all;
        //}
        public void TestForPolicySchedulePDF(string PolicyNo, string ProductCode, string PreferredLanguage, HttpContextBase context = null)
        {
            byte[] bytes = null;
            try
            {
                Microsoft.Reporting.WebForms.ReportViewer vr = new Microsoft.Reporting.WebForms.ReportViewer();
                DataSet dataset = FuncForPolicyDetailsPDF(PolicyNo);
                List<DataSet> dslst = new List<DataSet>();
                dslst.Add(dataset);
                ReportParameter[] parameters;
                DataSet ds2 = new DataSet();
                ds2 = FuncForPolicyBenefitDetailsPDF(PolicyNo);
                dslst.Add(ds2);
                DataSet ds3 = new DataSet();
                ds3 = FuncForDividendRates(PolicyNo);
                dslst.Add(ds3);
                DataSet ds4 = new DataSet();
                ds4 = GetReportLabel(ProductCode, PreferredLanguage, "Policy");
                dslst.Add(ds4);
                DataSet ds5 = new DataSet();
                ds5 = FuncForPolicyBeneFiciaryetails(PolicyNo);
                dslst.Add(ds5);
                DataSet ds6 = new DataSet();
                ds6 = FuncForPolicyPremiumDetailsforEduPlan(PolicyNo);
                dslst.Add(ds6);

                parameters = new ReportParameter[1];
                parameters[0] = new ReportParameter("PolicyNo", PolicyNo);


                //if (PreferredLanguage == "E")
                //{
                switch (ProductCode)
                {
                    case "PPG":
                        {

                            bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/PolicyScheduleforEasyPensions.rdlc");
                            RenderReports(bytes, PolicyNo);
                        }
                        break;
                    case "PPH":
                    case "PSP":
                        {
                            bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/PolicyScheduleforSmartPension.rdlc");
                            RenderReports(bytes, PolicyNo);
                        }
                        break;
                    case "EPB":
                    case "PEP":
                        {
                            bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/PolicyscheduleforEducationplan.rdlc");
                            RenderReports(bytes, PolicyNo);
                        }
                        break;
                    case "HPA":
                    case "PHP":
                        {
                            bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/PolicyscheduleforHealthprotector.rdlc");
                            RenderReports(bytes, PolicyNo);
                        }
                        break;
                    case "SBA":
                    case "SBB":
                    case "SBC":
                    case "SBD":
                    case "PSB":
                        {
                            bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/PolicyScheduleforSmartbuilder.rdlc");
                            RenderReports(bytes, PolicyNo);
                        }
                        break;
                    case "SBE":
                    case "SBF":
                    case "SMG":
                        {
                            bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/PolicyScheduleforSmartbuildergold.rdlc");
                            RenderReports(bytes, PolicyNo);
                        }
                        break;
                    case "SBG":
                    case "SBH":
                    case "PPV":
                        {
                            bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/PolicyScheduleforPriorityvalue.rdlc");
                            RenderReports(bytes, PolicyNo);
                        }
                        break;
                    default:
                        break;

                }
                //}
                //else if (PreferredLanguage == "T")
                //{
                //    switch (ProductCode)
                //    {
                //        case "PPG":
                //            {
                //                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/EasypenionpolicyscheduleinTamil.rdlc", context);
                //                RenderReports(bytes, PolicyNo);
                //            }
                //            break;
                //        case "PPH":
                //        case "PSP":
                //            {
                //                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/SmartpensionpolicyscheduleinTamil.rdlc", context);
                //                RenderReports(bytes, PolicyNo);
                //            }
                //            break;
                //        case "EPB":
                //        case "PEP":
                //            {
                //                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/EducationplanpolicyscheduleinTamil.rdlc", context);
                //                RenderReports(bytes, PolicyNo);
                //            }
                //            break;
                //        case "HPA":
                //        case "PHP":
                //            {
                //                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/HealthprotectorpolicyscheduleinTamil.rdlc", context);
                //                RenderReports(bytes, PolicyNo);
                //            }
                //            break;
                //        case "SBA":
                //        case "SBB":
                //        case "SBC":
                //        case "SBD":
                //        case "PSB":
                //            {
                //                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/SmartbuilderpolicyscheduleinTamil.rdlc", context);
                //                RenderReports(bytes, PolicyNo);
                //            }
                //            break;
                //        case "SBE":
                //        case "SBF":
                //        case "SMG":
                //            {
                //                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/SmartbuildergoldpolicyscheduleinTamil.rdlc", context);
                //                RenderReports(bytes, PolicyNo);
                //            }
                //            break;
                //        case "SBG":
                //        case "SBH":
                //        case "PPV":
                //            {
                //                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/PriorityvaluepolicyscheduleinTamil.rdlc", context);
                //                RenderReports(bytes, PolicyNo);
                //            }
                //            break;
                //        default:
                //            break;

                //    }

                //}
                //else if (PreferredLanguage == "S")
                //    {
                //        switch (ProductCode)
                //        {
                //            case "PPG":
                //                {
                //                    bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/EasypensionpolicyscheduleinSinhala.rdlc", context);
                //                    RenderReports(bytes, PolicyNo);
                //                }
                //                break;
                //            case "PPH":
                //            case "PSP":
                //            {
                //                    bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/SmartpensionpolicyscheduleinSinhala.rdlc", context);
                //                    RenderReports(bytes, PolicyNo);
                //                }
                //                break;
                //           case "EPB":
                //           case "PEP":
                //            {
                //                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/EducationplanpolicyscheduleinSinhala.rdlc", context);
                //                    RenderReports(bytes, PolicyNo);
                //                }
                //                break;
                //        case "HPA":
                //        case "PHP":
                //            {
                //                    bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/HealthprotectorpolicyscheduleinSinhala.rdlc", context);
                //                    RenderReports(bytes, PolicyNo);
                //                }
                //                break;
                //        case "SBA":
                //        case "SBB":
                //        case "SBC":
                //        case "SBD":
                //        case "PSB":
                //            {
                //                    bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/SmartbuilderpolicyscheduleinSinhala.rdlc", context);
                //                    RenderReports(bytes, PolicyNo);
                //                }
                //                break;
                //        case "SBE":
                //        case "SBF":
                //        case "SMG":
                //            {
                //                    bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/SmartbuildergoldpolicyscheduleinSinhala.rdlc", context);
                //                    RenderReports(bytes, PolicyNo);
                //                }
                //                break;
                //        case "SBG":
                //        case "SBH":
                //        case "PPV":
                //            {
                //                    bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/PriorityvaluepolicyscheduleinSinhala.rdlc", context);
                //                    RenderReports(bytes, PolicyNo);
                //                }
                //                break;
                //            default:
                //                break;

                //        }

                //}


            }
            catch (Exception ex)
            {
                ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                throw ex;
            }
            // return bytes;
        }




        public byte[] ReportForPolicySchedule(string PolicyNo, string ProductCode, string PreferredLanguage)
        {

            byte[] bytes = null;
            try
            {
                
                Microsoft.Reporting.WebForms.ReportViewer vr = new Microsoft.Reporting.WebForms.ReportViewer();
                DataSet dataset = FuncForPolicyDetailsPDF(PolicyNo);
                List<DataSet> dslst = new List<DataSet>();
                dslst.Add(dataset);
                ReportParameter[] parameters;
                DataSet ds2 = new DataSet();
                ds2 = FuncForPolicyBenefitDetailsPDF(PolicyNo);
                dslst.Add(ds2);
                DataSet ds3 = new DataSet();
                ds3 = FuncForDividendRates(PolicyNo);
                dslst.Add(ds3);
                DataSet ds4 = new DataSet();
                ds4 = GetReportLabel(ProductCode, PreferredLanguage, "Policy");
                dslst.Add(ds4);
                DataSet ds5 = new DataSet();
                ds5 = FuncForPolicyBeneFiciaryetails(PolicyNo);
                dslst.Add(ds5);
                DataSet ds6 = new DataSet();
                ds6 = FuncForPolicyPremiumDetailsforEduPlan(PolicyNo);
                dslst.Add(ds6);

                parameters = new ReportParameter[1];
                parameters[0] = new ReportParameter("PolicyNo", PolicyNo);


                switch (ProductCode)
                {
                    case "PPG":
                        {

                            bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/PolicyScheduleforEasyPensions.rdlc");
                        }
                        break;
                    case "PPH":
                    case "PSP":
                        {
                            bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/PolicyScheduleforSmartPension.rdlc");
                        }
                        break;
                    case "EPB":
                    case "EPA":
                    case "PEP":
                        {
                            bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/PolicyscheduleforEducationplan.rdlc");
                        }
                        break;
                    case "HPA":
                    case "PHP":
                        {
                            bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/PolicyscheduleforHealthprotector.rdlc");
                        }
                        break;
                    case "SBA":
                    case "SBB":
                    case "SBC":
                    case "SBD":
                    case "PSB":
                        {
                            bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/PolicyScheduleforSmartbuilder.rdlc");
                        }
                        break;
                    case "SBE":
                    case "SBF":
                    case "SMG":
                        {
                            bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/PolicyScheduleforSmartbuildergold.rdlc");
                        }
                        break;
                    case "SBG":
                    case "SBH":
                    case "PPV":
                        {
                            bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/PolicyScheduleforPriorityvalue.rdlc");
                        }
                        break;
                    default:
                        break;

                }

                //else if (PreferredLanguage == "1138")
                //{
                //    switch (ProductCode)
                //    {
                //        case "PPG":
                //            {
                //                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/EasypenionpolicyscheduleinTamil.rdlc", context);

                //            }
                //            break;
                //        case "PPH":
                //            {
                //                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/SmartpensionpolicyscheduleinTamil.rdlc", context);

                //            }
                //            break;
                //        case "EPB":
                //            {
                //                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/EducationplanpolicyscheduleinTamil.rdlc", context);

                //            }
                //            break;
                //        case "HPA":
                //            {
                //                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/PolicyscheduleforHealthprotector.rdlc", context);

                //            }
                //            break;
                //        case "SBA":
                //        case "SBB":
                //        case "SBC":
                //        case "SBD":
                //            {
                //                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/PolicyScheduleforSmartbuilder.rdlc", context);

                //            }
                //            break;
                //        case "SBE":
                //        case "SBF":
                //            {
                //                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/SmartbuildergoldpolicyscheduleinTamil.rdlc", context);

                //            }
                //            break;
                //        case "SBG":
                //        case "SBH":
                //            {
                //                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/PriorityvaluepolicyscheduleinTamil.rdlc", context);

                //            }
                //            break;
                //        default:
                //            break;

                //    }

                //}
                //else
                //{
                //    switch (ProductCode)
                //    {
                //        case "PPG":
                //            {
                //                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/EasypensionpolicyscheduleinSinhala.rdlc", context);

                //            }
                //            break;
                //        case "PPH":
                //            {
                //                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/SmartpensionpolicyscheduleinSinhala.rdlc", context);

                //            }
                //            break;
                //        case "EPB":
                //            {
                //                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/EducationplanpolicyscheduleinSinhala.rdlc", context);

                //            }
                //            break;
                //        case "HPA":
                //            {
                //                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/PolicyscheduleforHealthprotector.rdlc", context);

                //            }
                //            break;
                //        case "SBB":
                //        case "SBA":
                //        case "SBC":
                //        case "SBD":
                //            {
                //                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/PolicyScheduleforSmartbuilder.rdlc", context);

                //            }
                //            break;
                //        case "SBE":
                //        case "SBF":
                //            {
                //                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/SmartbuildergoldpolicyscheduleinSinhala.rdlc", context);

                //            }
                //            break;
                //        case "SBG":
                //        case "SBH":
                //            {
                //                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/PriorityvaluepolicyscheduleinSinhala.rdlc", context);

                //            }
                //            break;
                //        default:
                //            break;

                //    }

                //}   


            }
            catch (Exception ex)
            {
                ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                throw ex;
            }
            return bytes;
        }


        public byte[] ReportForCoveringLetter(string PolicyNo, string ProductCode, string PreferredLanguage)
        {

            byte[] bytes = null;
            try
            {
                Microsoft.Reporting.WebForms.ReportViewer vr = new Microsoft.Reporting.WebForms.ReportViewer();
                DataSet dataset = FuncForCovLetterDetailPDF(PolicyNo);
                List<DataSet> dslst = new List<DataSet>();
                dslst.Add(dataset);
                ReportParameter[] parameters;
                parameters = new ReportParameter[1];
                parameters[0] = new ReportParameter("PolicyNo", PolicyNo);

                if (PreferredLanguage == "E")
                {
                    switch (ProductCode)
                    {
                        case "PPG":
                        case "SBE":
                        case "SBF":
                        case "SBG":
                        case "SBH":
                        case "SBA":
                        case "SBB":
                        case "SBC":
                        case "SBD":
                        case "EPA":
                        case "EPB":
                        case "SMG":
                        case "PPV":
                        case "PSB":
                        case "PEP":
                            {
                                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/OtherProductsCoveringLetter.rdlc");
                            }
                            break;
                        case "HPA":
                        case "PSP":
                        case "PPH":
                        case "PHP":
                            {
                                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/HealthProtectorAndSmartPensionsCoveringLetter.rdlc");

                            }
                            break;
                        default:
                            break;

                    }
                }
                else
                {
                    switch (ProductCode)
                    {
                        case "PPG":
                        case "SBE":
                        case "SBF":
                        case "SBG":
                        case "SBH":
                        case "SBA":
                        case "SBB":
                        case "SBC":
                        case "SBD":
                        case "EPB":
                        case "SMG":
                        case "PPV":
                        case "PSB":
                        case "EPA":
                        case "PEP":
                            {
                                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/OtherProductsCoveringLetter.rdlc");
                            }
                            break;
                        case "HPA":
                        case "PSP":
                        case "PPH":
                        case "PHP":
                            {
                                bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/HealthProtectorAndSmartPensionsCoveringLetter.rdlc");

                            }
                            break;
                        default:
                            break;

                    }
                }

            }
            catch (Exception ex)
            {
                ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                throw ex;
            }
            return bytes;
        }
        public byte[] ReportForAFCCoveringLetter(string PolicyNo, string ProductCode, string PreferredLanguage)
        {

            byte[] bytes = null;
            try
            {
                Microsoft.Reporting.WebForms.ReportViewer vr = new Microsoft.Reporting.WebForms.ReportViewer();
                DataSet dataset = FuncForCovLetterDetailPDF(PolicyNo);
                List<DataSet> dslst = new List<DataSet>();
                dslst.Add(dataset);
                ReportParameter[] parameters;
                parameters = new ReportParameter[1];
                parameters[0] = new ReportParameter("PolicyNo", PolicyNo);

                //if (PreferredLanguage == "E")
                //{

                    bytes = GenerateRDLCReportsMail(dslst, parameters, @"~/Reports/AFCCoverLetter.rdlc");
                //}

            }
            catch (Exception ex)
            {
                ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                throw ex;
            }
            return bytes;
        }


        public DataSet FuncForCovLetterDetailPDF(string PolicyNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_CovLetterDetails_PDF";
                cmd.Parameters.AddWithValue("@PolicyNo", PolicyNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetReportLabel(string ProductCode, string Language, string Type)
        {
            try
            {
                string PrefferedLanguage = "S";
                if (Language == "1139" || Language == "Sinhala" || Language == "S")
                {
                    PrefferedLanguage = "Sinhala";

                }
                else if (Language == "1138" || Language == "Tamil" || Language == "T")
                {
                    PrefferedLanguage = "Tamil";
                }
                else
                {
                    PrefferedLanguage = "English";
                }
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetReportLabel";
                cmd.Parameters.AddWithValue("@ProductCode", ProductCode);
                cmd.Parameters.AddWithValue("@Language", PrefferedLanguage);
                cmd.Parameters.AddWithValue("@ReportName", Type);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPreviousAndCurrentLifeInsuranceQuestion(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_ReportPF_PreviousAndCurrentLifeInsuranceQuestion";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetFamilyBackGroundQuestion(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_ReportPF_FamilyBackGroundQuestion";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetChildMedicalHistoryQuestion(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_ReportPF_ChildMedicalHistoryQuestion";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPolicyMemberClaimInfo(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_ReportPF_PolicyMemberClaimInfo";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPolicyOwnerDetailsEP(string QuoteNo)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_PolictOwnerDetailsEasyPension";
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);

                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FuncForPolicyBeneFiciaryetails(string policyno)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_PolicyBeneficiaryDetails_PDF";
                cmd.Parameters.AddWithValue("@PolicyNo", policyno);
                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //added by Prasad for Education Policy Premium
        public DataSet FuncForPolicyPremiumDetailsforEduPlan(string policyno)
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_PolicyPremiumDetails_PDF";
                cmd.Parameters.AddWithValue("@PolicyNo", policyno);
                DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void GenerateHealthProtectorLetter()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"D:\00code\AIAReports\AIAReports\RHIReports\Quotation.xml");

            XmlNode quoteDetails = doc.SelectSingleNode("LifeQQ/QuotePDFDetails/QuotePDFDetails");
            string age = quoteDetails.SelectSingleNode("Age").InnerText;
            string spouseAge = quoteDetails.SelectSingleNode("Spouseage").InnerText;
            string policyTerm = quoteDetails.SelectSingleNode("PolicyTermID").InnerText;

            if (!string.IsNullOrEmpty(age))
            {
                int CoverEndAge = Convert.ToInt32(age) + Convert.ToInt32(policyTerm);
                XmlElement eleCoverEndAge = doc.CreateElement("CoverEndAge");
                eleCoverEndAge.InnerText = CoverEndAge + "";
                quoteDetails.AppendChild(eleCoverEndAge);
            }

            if (!string.IsNullOrEmpty(spouseAge))
            {
                int SpouseCoverEndAge = Convert.ToInt32(spouseAge) + Convert.ToInt32(policyTerm);
                XmlElement eleCoverEndAge = doc.CreateElement("SpouseCoverEndAge");
                eleCoverEndAge.InnerText = SpouseCoverEndAge + "";
                quoteDetails.AppendChild(eleCoverEndAge);
            }

            string Hospital_Income_Benefit_SumInsured = quoteDetails.SelectSingleNode("Hospital_Income_Benefit_SumInsured").InnerText;
            if (!string.IsNullOrEmpty(Hospital_Income_Benefit_SumInsured))
            {
                Hospital_Income_Benefit_SumInsured = Hospital_Income_Benefit_SumInsured.Replace(",", "");
                int _Hospital_Income_Benefit_SumInsured = Convert.ToInt32(Hospital_Income_Benefit_SumInsured);
                XmlElement hospben2x = doc.CreateElement("Hospital_Income_Benefit_SumInsured2X");
                hospben2x.InnerText = (_Hospital_Income_Benefit_SumInsured * 2) + "";
                quoteDetails.AppendChild(hospben2x);
            }

            string Spouse_Hospital_Income_Benefit_SumInsured = quoteDetails.SelectSingleNode("Spouse_Hospital_Income_Benefit_SumInsured").InnerText;
            if (!string.IsNullOrEmpty(Hospital_Income_Benefit_SumInsured))
            {
                Spouse_Hospital_Income_Benefit_SumInsured = Spouse_Hospital_Income_Benefit_SumInsured.Replace(",", "");
                int _Spouse_Hospital_Income_Benefit_SumInsured = Convert.ToInt32(Spouse_Hospital_Income_Benefit_SumInsured);
                XmlElement hospben2x = doc.CreateElement("Spouse_Hospital_Income_Benefit_SumInsured2X");
                hospben2x.InnerText = (_Spouse_Hospital_Income_Benefit_SumInsured * 2) + "";
                quoteDetails.AppendChild(hospben2x);
            }

            string Child_Hospital_Income_Benefit_SumInsured = quoteDetails.SelectSingleNode("Child_Hospital_Income_Benefit_SumInsured").InnerText;
            if (!string.IsNullOrEmpty(Hospital_Income_Benefit_SumInsured))
            {
                Child_Hospital_Income_Benefit_SumInsured = Child_Hospital_Income_Benefit_SumInsured.Replace(",", "");
                int _Child_Hospital_Income_Benefit_SumInsured = Convert.ToInt32(Child_Hospital_Income_Benefit_SumInsured);
                XmlElement hospben2x = doc.CreateElement("Child_Hospital_Income_Benefit_SumInsured2X");
                hospben2x.InnerText = (_Child_Hospital_Income_Benefit_SumInsured * 2) + "";
                quoteDetails.AppendChild(hospben2x);
            }



            string selectedPremiumFrequency = quoteDetails.SelectSingleNode("PremiumPayingFrequency").InnerText;
            selectedPremiumFrequency = selectedPremiumFrequency.ToUpper();
            if (selectedPremiumFrequency.Equals("QUARTERLY") || selectedPremiumFrequency.Equals("HALFYEARLY") || selectedPremiumFrequency.Equals("HALF YEARLY"))
            {
                XmlElement eleSelFreq = doc.CreateElement("SelectedPremiumFrequencyNotAnnual");
                eleSelFreq.InnerText = "YES";
                quoteDetails.AppendChild(eleSelFreq);
            }




            bool ShowChildCovers = false;
            XmlNodeList nodeList = doc.SelectNodes("LifeQQ/MemberBenefitDetailsByRiderId/MemberBenefitDetailsByRiderId");
            foreach (XmlNode node in nodeList)
            {
                string RiderId = node.SelectSingleNode("RiderId").InnerText;
                XmlNode MainSA = node.SelectSingleNode("MainSA");
                XmlNode SpouseSA = node.SelectSingleNode("SpouseSA");
                XmlNode RiderUnit = node.SelectSingleNode("RiderUnit");
                if (RiderId.Equals("10"))
                {
                    MainSA.InnerText = "Yes";
                }
                else
                {
                    string mainSAValue = "";
                    if (!MainSA.InnerText.Equals("-"))
                    {
                        mainSAValue = "LKR " + FormatCurrency(MainSA.InnerText);
                        if (RiderUnit.Value != null)
                        {
                            mainSAValue += (" " + RiderUnit.Value);
                        }
                        MainSA.InnerText = mainSAValue;
                    }
                }

                if (RiderId.Equals("15"))
                {
                    MainSA.InnerText = "Yes";
                }
                else
                {
                    string spouseSAValue = "";
                    if (!SpouseSA.InnerText.Equals("-"))
                    {
                        spouseSAValue = "LKR " + FormatCurrency(SpouseSA.InnerText);
                        if (RiderUnit.Value != null)
                        {
                            spouseSAValue += (" " + RiderUnit.Value);
                        }
                        SpouseSA.InnerText = spouseSAValue;
                    }
                }
                string Child1SA = node.SelectSingleNode("Child1SA").InnerText;
                string Child2SA = node.SelectSingleNode("Child2SA").InnerText;
                string Child3SA = node.SelectSingleNode("Child3SA").InnerText;
                string Child4SA = node.SelectSingleNode("Child4SA").InnerText;
                string Child5SA = node.SelectSingleNode("Child5SA").InnerText;

                int TotalChildSA = Convert.ToInt32(Child1SA) + Convert.ToInt32(Child2SA) + Convert.ToInt32(Child3SA) +
                        Convert.ToInt32(Child4SA) + Convert.ToInt32(Child5SA);


                if (TotalChildSA > 0)
                {
                    ShowChildCovers = true;


                    string childSAValue = "";

                    if (RiderId.Equals("15"))
                    {
                        childSAValue = "YES";
                    }
                    else
                    {
                        childSAValue = "LKR " + FormatCurrency(TotalChildSA + "");
                    }

                    XmlElement TotalChildSA_elem = doc.CreateElement("TotalChildSA");
                    TotalChildSA_elem.InnerText = childSAValue;
                    node.AppendChild(TotalChildSA_elem);

                    XmlElement ShowChildCover_elem = doc.CreateElement("ShowCover");
                    ShowChildCover_elem.InnerText = "YES";
                    node.AppendChild(ShowChildCover_elem);

                }

                XmlNode node1 = node.SelectSingleNode("Spouse_Life_Benefit_SumInsured");
                XmlNode node2 = node.SelectSingleNode("Spouse_Accident_Benefit_SumInsured");
                if (node1 != null && node2 != null)
                {
                    int Spouse_Life_Benefit_SumInsured = Convert.ToInt32(node1.InnerText);
                    int Spouse_Accident_Benefit_SumInsured = Convert.ToInt32(node2.InnerText);

                    int Spouse_Life_PLUS_Accident_SumInsured = Spouse_Life_Benefit_SumInsured + Spouse_Accident_Benefit_SumInsured;

                    XmlElement Spouse_Life_PLUS_Accident_SumInsured_elem = doc.CreateElement("Spouse_Life_PLUS_Accident_SumInsured");
                    Spouse_Life_PLUS_Accident_SumInsured_elem.InnerText = Spouse_Life_PLUS_Accident_SumInsured + "";
                    node.AppendChild(Spouse_Life_PLUS_Accident_SumInsured_elem);
                }

            }
            if (ShowChildCovers)
            {
                XmlElement eleChildCovers = doc.CreateElement("ShowChildCovers");
                eleChildCovers.InnerText = "YES";
                quoteDetails.AppendChild(eleChildCovers);
            }

            string xslPath = System.Web.HttpContext.Current.Server.MapPath(@"~/Content/Templates/XSLTemplates/QuoteHealthProtector.xsl");
            GenerateLetter(doc.InnerXml, xslPath);
        }
        private string FormatCurrency(string input)
        {
            int _input = Convert.ToInt32(input);
            return string.Format("{0:#,###,###0}", _input);
        }

        public byte[] GenerateLetter(string inputXML, string XSLPath)
        {
            FonetDriver driver = FonetDriver.Make();

            MemoryStream data_stream = new MemoryStream();
            var writer = new StreamWriter(data_stream);
            writer.Write(inputXML);
            writer.Flush();
            var xslt = new XslCompiledTransform();

            xslt.Load(XSLPath);

            var tempDir = System.Web.HttpContext.Current.Server.MapPath(@"~/temp");
            var tempFile = tempDir + "\\" + Guid.NewGuid() + ".pdf";

            data_stream.Seek(0, SeekOrigin.Begin);
            var fo_stream = new MemoryStream();
            using (var reader1 = XmlReader.Create(data_stream))
            {
                try
                {
                    xslt.Transform(reader1, null, fo_stream);
                }
                catch (Exception tex)
                {
                    data_stream.Seek(0, SeekOrigin.Begin);
                    var sr = new StreamReader(data_stream);
                    var xmlstr = sr.ReadToEnd();
                    throw tex;
                }
            }
            data_stream.Seek(0, SeekOrigin.Begin);
            var sr1 = new StreamReader(data_stream);
            var xmlstr1 = sr1.ReadToEnd();
            data_stream.Close();
            fo_stream.Seek(0, SeekOrigin.Begin);


            driver.BaseDirectory = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath(@"~/Content/Templates/XSLTemplates"));
            driver.Options = new Fonet.Render.Pdf.PdfRendererOptions();
            driver.Options.FontType = Fonet.Render.Pdf.FontType.Embed;

            warnings = new List<string>();
            driver.OnWarning += HandleWarning;
            driver.OnError += HandleErrors;

            
            driver.Render(fo_stream, new FileStream(tempFile, FileMode.Create));

            fo_stream.Close();
            byte[] ByteArray = System.IO.File.ReadAllBytes(tempFile);
            return ByteArray;
        }

        private List<string> warnings;

        private void HandleWarning(object Driver, FonetEventArgs e)
        {
            warnings.Add("W:" + e.GetMessage());
        }
        private void HandleErrors(object Driver, FonetEventArgs e)
        {
            warnings.Add("E:" + e.GetMessage());
        }
        public Byte[] GenerateEduPlanQuotation(string QuoteNo)
        {
            XmlDocument doc = new XmlDocument();
            //var xml = Context.usp_getQuoteXml(QuoteNo).FirstOrDefault();
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            con.Open();
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_getQuoteXml";//usp_GetReportLabel
            cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
            //var xmlStr = "";
            //cmd.Parameters[0].Value = xmlStr;
            DataSet ds = new DataSet();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
            da.Fill(ds);
            if (ds.Tables.Count != 0)
            {
                var xmlString = ds.Tables[0].Rows[0][0].ToString();
                doc.LoadXml(xmlString);
            }

            if (doc != null)
            {
                XmlNode quoteDetails = doc.SelectSingleNode("LifeQQ/QuotePDFDetails/QuotePDFDetails");

                #region FullName
                string title = GetNodeText(quoteDetails, "Title");
                string firstName = GetNodeText(quoteDetails, "FirstName");
                string lastName = GetNodeText(quoteDetails, "LastName");
                XmlElement elePropFullName = doc.CreateElement("ProposerFullName");
                elePropFullName.InnerText = title + " " + firstName + " " + lastName;
                quoteDetails.AppendChild(elePropFullName);
                #endregion

                int age = GetNodeInt(quoteDetails, "Age");
                int policyTerm = GetNodeInt(quoteDetails, "PolicyTermID");

                int AgeAtMaturity = age + policyTerm;
                XmlElement eleAgeAtMaturity = doc.CreateElement("AgeAtMaturity");
                eleAgeAtMaturity.InnerText = AgeAtMaturity + "";
                quoteDetails.AppendChild(eleAgeAtMaturity);


                int TotalLifeBenefit = 0;
                int AnnualPremiumForSelMode1 = 0;
                int Premium1 = 0;
                int AnnualPremiumForSelMode2 = 0;
                int Premium2 = 0;
                int AnnualPremiumForSelMode3 = 0;
                int Premium3 = 0;
                int AnnualPremiumForSelMode4 = 0;
                int Premium4 = 0;
                int AnnualPremiumForSelMode5 = 0;
                int Premium5 = 0;
                int LoadingAmount2 = 0;
                int LoadingAmount5 = 0;
                string ShowLoading = "false";
                string ShowLivingBenefits = "false";
                string ShowProtectionBenefits = "false";
                string ShowSpouseBenefits = "false";
                string ShowChildBenefits = "false";
                XmlNodeList benefits = doc.SelectNodes("LifeQQ/MemberBenefitDetails/MemberBenefitDetails");
                int TotalAnnualPremiumForSelMode = 0;
                int TotalPremium = 0;
                int TotalAnnualPremiumForSelModeAPlusB = 0;
                int TotalPremiumAPlusB = 0;
                foreach (XmlNode b in benefits)
                {

                    XmlNode d = b.SelectSingleNode("DisplayName");
                    string DisplayName = d.InnerText;
                    if (DisplayName.Contains("Basic") || DisplayName.Contains("Additional Life Benefit"))
                    {
                        XmlNode si = b.SelectSingleNode("SumInsured");
                        TotalLifeBenefit += Convert.ToInt32(si.InnerText);
                    }
                    int AnnualPremiumForSelMode = GetNodeInt(b, "AnnualPremiumForSelMode");
                    int Premium = GetNodeInt(b, "Premium");
                    int LoadingAmount = GetNodeInt(b, "LoadingAmount");
                    int AnnualDiscountForSelMode = GetNodeInt(b, "AnnualDiscountForSelMode");
                    int RelationShip = GetNodeInt(b, "RelationShip");
                    int DiscountAmount = GetNodeInt(b, "DiscountAmount");
                    if (DisplayName.Contains("Basic"))
                    {
                        AnnualPremiumForSelMode1 = AnnualPremiumForSelMode;
                        Premium1 = Premium;
                    }
                    else if (DisplayName.Contains("Additional Life Benefit"))
                    {

                        AnnualPremiumForSelMode2 = AnnualPremiumForSelMode + AnnualDiscountForSelMode;
                        Premium2 = Premium + DiscountAmount;
                        if (LoadingAmount == 267)
                        {
                            LoadingAmount2 = LoadingAmount;
                        }

                        AnnualPremiumForSelMode3 = AnnualDiscountForSelMode;
                        Premium3 = DiscountAmount;

                        AnnualPremiumForSelMode4 = AnnualPremiumForSelMode;
                        Premium4 = Premium;
                    }
                    else if (DisplayName.Contains("Waiver of Premium"))
                    {

                        AnnualPremiumForSelMode5 = AnnualPremiumForSelMode;
                        Premium5 = Premium;
                        if (LoadingAmount == 267)
                        {
                            LoadingAmount5 = LoadingAmount;
                        }

                    }

                    if (!DisplayName.Contains("Basic"))
                    {
                        TotalAnnualPremiumForSelMode += AnnualPremiumForSelMode;
                        TotalPremium += Premium;
                    }
                    TotalAnnualPremiumForSelModeAPlusB += AnnualPremiumForSelMode;
                    TotalPremiumAPlusB += Premium;
                    #region XXXLivingBenefits

                    if (RelationShip == 267 &&
                            (DisplayName.Contains("Critical") ||
                                DisplayName.Contains("Accident") ||
                                DisplayName.Contains("Adult") ||
                                DisplayName.Contains("Hospitalization") ||
                                DisplayName.Contains("Hospital")))
                    {
                        AddNode(doc, b, "XXXDisplayInLivingBenefitsSection", "true");
                        ShowLivingBenefits = "true";
                    }

                    if (DisplayName.Contains("Accident"))
                    {
                        AddNode(doc, b, "XXXDisplayInProtectionBenefitsSection", "true");
                        ShowProtectionBenefits = "true";
                    }

                    if (RelationShip == 268)
                    {
                        AddNode(doc, b, "XXXDisplayInSpouseBenefitsSection", "true");
                        ShowSpouseBenefits = "true";
                    }

                    if (RelationShip == 269 || RelationShip == 270)
                    {
                        AddNode(doc, b, "XXXDisplayInChildBenefitsSection", "true");
                        ShowChildBenefits = "true";
                    }

                    #endregion

                    if (LoadingAmount != 0)
                    {
                        ShowLoading = "true";
                    }
                }
                int AnnualPremiumForSelMode6 = AnnualPremiumForSelMode1 + AnnualPremiumForSelMode4 + AnnualPremiumForSelMode5;
                int Premium6 = Premium1 + Premium4 + Premium5;


                XmlElement XXXBenefitsAndPremiums = doc.CreateElement("XXXBenefitsAndPremiums");
                AddNode(doc, XXXBenefitsAndPremiums, "AnnualPremiumForSelMode1", FormatCurrency(AnnualPremiumForSelMode1));
                AddNode(doc, XXXBenefitsAndPremiums, "Premium1", FormatCurrency(Premium1));
                AddNode(doc, XXXBenefitsAndPremiums, "AnnualPremiumForSelMode2", FormatCurrency(AnnualPremiumForSelMode2));
                AddNode(doc, XXXBenefitsAndPremiums, "Premium2", FormatCurrency(Premium2));
                AddNode(doc, XXXBenefitsAndPremiums, "AnnualPremiumForSelMode3", FormatCurrency(AnnualPremiumForSelMode3));
                AddNode(doc, XXXBenefitsAndPremiums, "Premium3", FormatCurrency(Premium3));
                AddNode(doc, XXXBenefitsAndPremiums, "AnnualPremiumForSelMode4", FormatCurrency(AnnualPremiumForSelMode4));
                AddNode(doc, XXXBenefitsAndPremiums, "Premium4", FormatCurrency(Premium4));
                AddNode(doc, XXXBenefitsAndPremiums, "AnnualPremiumForSelMode5", FormatCurrency(AnnualPremiumForSelMode5));
                AddNode(doc, XXXBenefitsAndPremiums, "Premium5", FormatCurrency(Premium5));
                AddNode(doc, XXXBenefitsAndPremiums, "AnnualPremiumForSelMode6", FormatCurrency(AnnualPremiumForSelMode6));
                AddNode(doc, XXXBenefitsAndPremiums, "Premium6", FormatCurrency(Premium6));
                AddNode(doc, XXXBenefitsAndPremiums, "ShowLoading", ShowLoading);

                AddNode(doc, XXXBenefitsAndPremiums, "XXXShowLivingBenefits", ShowLivingBenefits);
                AddNode(doc, XXXBenefitsAndPremiums, "XXXShowProtectionBenefits", ShowProtectionBenefits);
                AddNode(doc, XXXBenefitsAndPremiums, "XXXShowShowSpouseBenefits", ShowSpouseBenefits);
                AddNode(doc, XXXBenefitsAndPremiums, "XXXShowShowChildBenefits", ShowChildBenefits);

                doc.DocumentElement.AppendChild(XXXBenefitsAndPremiums);

                XmlElement eleTotalLifeBenefit = doc.CreateElement("TotalLifeBenefit");
                eleTotalLifeBenefit.InnerText = TotalLifeBenefit + "";
                quoteDetails.AppendChild(eleTotalLifeBenefit);

                TotalAnnualPremiumForSelMode = TotalAnnualPremiumForSelMode -
                    GetNodeIntByTag(doc, "LifeQQ/BenefitDetails/BenefitDetails/AnnualSelModeAdditionalPremium") -
                    GetNodeIntByTag(doc, "LifeQQ/BenefitDetails/BenefitDetails/AnnualSelModeProtectionPremium");

                TotalPremium = TotalPremium -
                    GetNodeIntByTag(doc, "LifeQQ/BenefitDetails/BenefitDetails/AdditionalPremium") -
                    GetNodeIntByTag(doc, "LifeQQ/BenefitDetails/BenefitDetails/ProtectionPremium");


                AddNode(doc, XXXBenefitsAndPremiums, "XXXTotalAnnualPremiumForSelModeB", FormatCurrency(TotalAnnualPremiumForSelMode));
                AddNode(doc, XXXBenefitsAndPremiums, "XXXTotalPremiumB", FormatCurrency(TotalPremium));
                AddNode(doc, XXXBenefitsAndPremiums, "XXXTotalAnnualPremiumForSelModeAPlusB", FormatCurrency(TotalAnnualPremiumForSelModeAPlusB));
                AddNode(doc, XXXBenefitsAndPremiums, "XXXTotalPremiumAPlusB", FormatCurrency(TotalPremiumAPlusB));

                string xslPath = System.Web.HttpContext.Current.Server.MapPath(@"~/Reports/XSLT/QutoteEduPlan_en.xsl");
                Byte[] ByteArray = GenerateLetter(doc.InnerXml, xslPath);
                return ByteArray;
            }
            return null;
        }
        public void AddNode(XmlDocument doc, XmlElement parent, string NodeName, string NodeValue)
        {
            XmlElement node = doc.CreateElement(NodeName);
            node.InnerText = NodeValue;
            parent.AppendChild(node);
        }
        public void AddNode(XmlDocument doc, XmlNode parent, string NodeName, string NodeValue)
        {
            XmlElement node = doc.CreateElement(NodeName);
            node.InnerText = NodeValue;
            parent.AppendChild(node);
        }
        //private string FormatCurrency(string input)
        //{
        //    int _input = Convert.ToInt32(input);
        //    return string.Format("{0:#,###,###0}", _input);
        //}
        private string FormatCurrency(int input)
        {
            return string.Format("{0:#,###,###0}", input);
        }
        public int GetNodeInt(XmlNode parent, string Tag)
        {
            XmlNode _n = parent.SelectSingleNode(Tag);
            if (_n == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(_n.InnerText.Replace(",", ""));
            }
        }

        public int GetNodeIntByTag(XmlDocument doc, string path)
        {
            XmlNode _n = doc.SelectSingleNode(path);
            if (_n == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(_n.InnerText.Replace(",", ""));
            }
        }
        public string GetNodeText(XmlNode parent, string Tag)
        {
            XmlNode _n = parent.SelectSingleNode(Tag);
            if (_n == null)
            {
                return "";
            }
            else
            {
                return _n.InnerText;
            }
        }
        public Byte[] GenerateEduPlanProposal(string QuoteNo)
        {
            XmlDocument doc = new XmlDocument();
            //var xml = Context.usp_getQuoteXml(QuoteNo).FirstOrDefault();
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            con.Open();
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_getProposalXml";//usp_GetReportLabel
            cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
            //var xmlStr = "";
            //cmd.Parameters[0].Value = xmlStr;
            DataSet ds = new DataSet();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
            da.Fill(ds);
            if (ds.Tables.Count != 0)
            {
                var xmlString = ds.Tables[0].Rows[0][0].ToString();
                doc.LoadXml(xmlString);
            }

            if (doc != null)
            {
                XmlNode mainLife = doc.SelectSingleNode("LifeQQ/MainLifeDetails/MainLifeDetails");

                string proposerSign = GetImage(doc, "LifeQQ/PolicyDocumentsDetails/PolicyDocumentsDetails", "File");
                string agentSign = GetImage(doc, "LifeQQ/PolicyDocumentsWPSinatureDetails/PolicyDocumentsWPSinatureDetails", "File");
                string spouseSign = GetImage(doc, "LifeQQ/PolicyDocumentsSpouseSinatureDetails/PolicyDocumentsSpouseSinatureDetails", "File");

                AddNode(doc, mainLife, "proposerSign", proposerSign);
                AddNode(doc, mainLife, "agentSign", agentSign);
                AddNode(doc, mainLife, "spouseSign", spouseSign);

                string xslPath = System.Web.HttpContext.Current.Server.MapPath(@"~/Reports/XSLT/ProposalEduPlan_en.xsl");
                Byte[] ByteArray = GenerateLetter(doc.InnerXml, xslPath);
                return ByteArray;
            }
            return null;
        }
        public Byte[] GenerateSmartPensionProposal(string QuoteNo)
        {
            XmlDocument doc = new XmlDocument();
            //var xml = Context.usp_getQuoteXml(QuoteNo).FirstOrDefault();
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            con.Open();
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_getProposalXml";//usp_GetReportLabel
            cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
            //var xmlStr = "";
            //cmd.Parameters[0].Value = xmlStr;
            DataSet ds = new DataSet();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
            da.Fill(ds);
            if (ds.Tables.Count != 0)
            {
                var xmlString = ds.Tables[0].Rows[0][0].ToString();
                doc.LoadXml(xmlString);
            }

            if (doc != null)
            {
                XmlNode mainLife = doc.SelectSingleNode("LifeQQ/MainLifeDetails/MainLifeDetails");

                string proposerSign = GetImage(doc, "LifeQQ/PolicyDocumentsDetails/PolicyDocumentsDetails", "File");
                string agentSign = GetImage(doc, "LifeQQ/PolicyDocumentsWPSinatureDetails/PolicyDocumentsWPSinatureDetails", "File");
                string spouseSign = GetImage(doc, "LifeQQ/PolicyDocumentsSpouseSinatureDetails/PolicyDocumentsSpouseSinatureDetails", "File");

                AddNode(doc, mainLife, "proposerSign", proposerSign);
                AddNode(doc, mainLife, "agentSign", agentSign);
                AddNode(doc, mainLife, "spouseSign", spouseSign);

                //XmlNode quoteDetails = doc.SelectSingleNode("LifeQQ/QuotePDFDetails/QuotePDFDetails");
                string xslPath = System.Web.HttpContext.Current.Server.MapPath(@"~/Reports/XSLT/ProposalSmartPension_en.xsl");
                Byte[] ByteArray = GenerateLetter(doc.InnerXml, xslPath);
                return ByteArray;
            }
            return null;
        }
        public string GetImage(XmlDocument doc, string path, string attribute)
        {
            XmlNode image = doc.SelectSingleNode(path);
            if (image == null)
            {
                return "";
            }
            XmlNode attr = image.Attributes[attribute];
            if (attr == null)
            {
                return "";
            }
            string base64 = attr.InnerText;
            byte[] data = Convert.FromBase64String(base64);
            string temppath = System.Web.HttpContext.Current.Server.MapPath("~/temp/" + Guid.NewGuid());
            System.IO.File.WriteAllBytes(temppath, data);
            return temppath;
        }
        public byte[] GenerateSmartPensionPolicy(string QuoteNo)
        {
            //XmlDocument doc = new XmlDocument();
            //doc.Load(@"D:\00code\AIAReports\AIAReports\RHIReports\CoverLetter.xml");
            XmlDocument doc = new XmlDocument();
            //var xml = Context.usp_getQuoteXml(QuoteNo).FirstOrDefault();
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            con.Open();
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_GetCovLetterDetailsXml";//usp_GetReportLabel
            cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
            //var xmlStr = "";
            //cmd.Parameters[0].Value = xmlStr;
            DataSet ds = new DataSet();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
            da.Fill(ds);
            if (ds.Tables.Count != 0)
            {
                var xmlString = ds.Tables[0].Rows[0][0].ToString();
                doc.LoadXml(xmlString);
            }

            //XmlDocument policy_doc = new XmlDocument();
            //policy_doc.Load(@"D:\00code\AIAReports\AIAReports\RHIReports\SmartPensionPolicy.xml");
            XmlDocument policy_doc = new XmlDocument();
            //var xml = Context.usp_getQuoteXml(QuoteNo).FirstOrDefault();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            conn.Open();
            System.Data.SqlClient.SqlCommand cmdd = new System.Data.SqlClient.SqlCommand();
            cmdd.Connection = conn;
            cmdd.CommandType = CommandType.StoredProcedure;
            cmdd.CommandText = "usp_GetPolicyScheduleXml";//usp_GetReportLabel
            cmdd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
            //var xmlStr = "";
            //cmd.Parameters[0].Value = xmlStr;
            DataSet dss = new DataSet();
            System.Data.SqlClient.SqlDataAdapter daa = new System.Data.SqlClient.SqlDataAdapter(cmdd);
            daa.Fill(dss);
            if (dss.Tables.Count != 0)
            {
                var xmlString = dss.Tables[0].Rows[0][0].ToString();
                policy_doc.LoadXml(xmlString);
            }
            XmlNode policyRoot = policy_doc.SelectSingleNode("LifeQQ");

            int Aggregate_Premium = GetNodeIntByTag(policy_doc, "LifeQQ/PolicyPremiumDetails_PDF/PolicyPremiumDetails_PDF/Aggregate_Premium");
            int Additional_Premium = GetNodeIntByTag(policy_doc, "LifeQQ/PolicyPremiumDetails_PDF/PolicyPremiumDetails_PDF/Additional_Premium");
            int TotalPremiumAPlusB = Aggregate_Premium + Additional_Premium;
            AddNode(policy_doc, policy_doc.SelectSingleNode("LifeQQ/PolicyPremiumDetails_PDF/PolicyPremiumDetails_PDF"), "TotalPremiumAPlusB", FormatCurrency(TotalPremiumAPlusB));


            foreach (XmlNode node in policyRoot.ChildNodes)
            {
                XmlNode imported = doc.ImportNode(node, true);
                doc.DocumentElement.AppendChild(imported);
            }

            //XmlDocument illustration_doc = new XmlDocument();
            //illustration_doc.Load(@"D:\00code\AIAReports\AIAReports\RHIReports\SmartPensionPolicyIllustration.xml");
            XmlDocument illustration_doc = new XmlDocument();
            //var xml = Context.usp_getQuoteXml(QuoteNo).FirstOrDefault();
            System.Data.SqlClient.SqlConnection connn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            connn.Open();
            System.Data.SqlClient.SqlCommand cmddd = new System.Data.SqlClient.SqlCommand();
            cmddd.Connection = connn;
            cmddd.CommandType = CommandType.StoredProcedure;
            cmddd.CommandText = "usp_getPolicyIllustrationXml";//usp_GetReportLabel
            cmddd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
            //var xmlStr = "";
            //cmd.Parameters[0].Value = xmlStr;
            DataSet dsss = new DataSet();
            System.Data.SqlClient.SqlDataAdapter daaa = new System.Data.SqlClient.SqlDataAdapter(cmddd);
            daa.Fill(dsss);
            if (dsss.Tables.Count != 0)
            {
                var xmlString = dsss.Tables[0].Rows[0][0].ToString();
                illustration_doc.LoadXml(xmlString);
            }
            XmlNode policyRoot2 = illustration_doc.SelectSingleNode("LifeQQ");

            foreach (XmlNode node in policyRoot2.ChildNodes)
            {
                XmlNode imported = doc.ImportNode(node, true);
                doc.DocumentElement.AppendChild(imported);
            }


            string xslPath = System.Web.HttpContext.Current.Server.MapPath(@"~/Reports/XSLT/PolicySmartPension_en.xsl");
            Byte[] bytes = GenerateLetter(doc.InnerXml, xslPath);
            return bytes;
        }


    }

}
public static class Extensions
{
    public static string ToXml(this DataSet ds)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (TextWriter streamWriter = new StreamWriter(memoryStream))
            {
                var xmlSerializer = new XmlSerializer(typeof(DataSet));
                xmlSerializer.Serialize(streamWriter, ds);
                return System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }
    }
}
