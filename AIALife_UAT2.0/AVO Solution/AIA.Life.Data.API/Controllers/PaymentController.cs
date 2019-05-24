using AIA.CrossCutting;
using AIA.Life.Data.API.ControllerLogic.Payment;
//using AIA.Life.Integration.Services.MCash;
//using AIA.Life.Integration.Services.Payment;
using AIA.Life.Models.Integration.Payment;
using AIA.Life.Models.Payment;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AIA.Life.Data.API.Controllers
{
    public class PaymentController : ApiController
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        public PaymentModel FetchProposals(PaymentModel objPaymentModel)
        {
            try
            {
                PaymentLogic objPaymentLogic = new PaymentLogic();
                return objPaymentLogic.FetchProposals(objPaymentModel);
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = objPaymentModel.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(ex);
                objPaymentModel.Error.ErrorMessage = "Something went wrong. Please contact the admin, your reference code : " + objPaymentModel.Error.ErrorCode;
                return objPaymentModel;
            }
        }

        //public PaymentServiceModel FetchPaymentProposals(PaymentServiceModel objPaymentModel)
        //{
        //    PaymentIntegration objPaymentLogic = new PaymentIntegration();
        //    return objPaymentLogic.FetchPaymentProposals(objPaymentModel);
        //}
        //public PaymentServiceModel FetchPolicyHolderDetails(PaymentServiceModel objPaymentModel)
        //{
        //    PaymentIntegration objPaymentLogic = new PaymentIntegration();
        //    return objPaymentLogic.FetchPolicyHolderDetails(objPaymentModel);
        //}
        public PaymentModel SaveProposalPaymentInfo(PaymentModel objPaymentModel)
        {
            try
            {
                PaymentLogic objPaymentLogic = new PaymentLogic();
                return objPaymentLogic.SaveProposalPaymentInfo(objPaymentModel);
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                if (objPaymentModel.Message == "Success")
                    objPaymentModel.Message = "Success";
                return objPaymentModel;
            }
        }
        public PaymentModel MCashPayment(PaymentModel objPaymentModel)
        {
            try
            {
                PaymentLogic objPaymentLogic = new PaymentLogic();
                //McashIntegration mcashIntegration = new McashIntegration();
                ////objPaymentModel = new PaymentModel();
                ////objPaymentModel.PayableAmount = "100";
                ////objPaymentModel.TransactionNo = "T2404201876575765";
                ////objPaymentModel.ProposalNo = "50125759";
                ////objPaymentModel.McashPin = "0000";
                ////objPaymentModel.McashMobile = "0718929748";
                ////objPaymentModel.Mobile = "0712911111";
                ////mcashIntegration.PayUtilitiesDirect(objPaymentModel);
                //if (mcashIntegration.PayUtilitiesDirect(objPaymentModel))
                    objPaymentModel = objPaymentLogic.SaveProposalPaymentInfo(objPaymentModel);
                return objPaymentModel;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                objPaymentModel.Message = "Error";
                objPaymentModel.Error.ErrorMessage = "Something went wrong. Please contact the admin, your reference code : " + objPaymentModel.Error.ErrorCode;
                return objPaymentModel;
            }
        }
        public PaymentModel SavePGTransaction(PaymentModel objPaymentModel)
        {
            try
            {
                PaymentLogic objPaymentLogic = new PaymentLogic();
                objPaymentModel = objPaymentLogic.SavePGTransaction(objPaymentModel);
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = objPaymentModel.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(ex);
                objPaymentModel.Error.ErrorMessage = "Something went wrong. Please contact the admin, your reference code : " + objPaymentModel.Error.ErrorCode;
            }
            return objPaymentModel;
        }
        public PaymentModel UpdatePGTransaction(PaymentModel objPaymentModel)
        {
            try
            {
                PaymentLogic objPaymentLogic = new PaymentLogic();
                objPaymentModel = objPaymentLogic.UpdatePGTransaction(objPaymentModel);
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = objPaymentModel.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(ex);
                objPaymentModel.Error.ErrorMessage = "Something went wrong. Please contact the admin, your reference code : " + objPaymentModel.Error.ErrorCode;
            }
            return objPaymentModel;
        }
        public PaymentModel CheckPaymentStatusUpdate(PaymentModel objPaymentModel)
        {
            try
            {
                PaymentLogic objPaymentLogic = new PaymentLogic();
                objPaymentModel = objPaymentLogic.CheckPaymentStatusUpdate(objPaymentModel);
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = objPaymentModel.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(ex);
                objPaymentModel.Error.ErrorMessage = "Something went wrong. Please contact the admin, your reference code : " + objPaymentModel.Error.ErrorCode;
            }
            return objPaymentModel;
        }
        public PaymentModel FetchPendingPayments(PaymentModel objPaymentModel)
        {
            try
            {
                PaymentLogic objPaymentLogic = new PaymentLogic();
                objPaymentModel = objPaymentLogic.FetchPendingPayments(objPaymentModel);
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = objPaymentModel.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(ex);
                objPaymentModel.Error.ErrorMessage = "Something went wrong. Please contact the admin, your reference code : " + objPaymentModel.Error.ErrorCode;
            }
            return objPaymentModel;
        }
    }
}
