using AIA.Life.Business.Policy;
//using AIA.Life.Integration.Services.Payment;
//using AIA.Life.Integration.Services.Policy;
//using AIA.Life.Models.Integration.Payment;
using AIA.Life.Models.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Business.Payment
{
    public class PaymentBusiness
    {
        public PaymentModel FetchProposals(PaymentModel objPaymentModel)
        {

            #region Call API
            objPaymentModel = WebApiLogic.GetPostComplexTypeToAPI<PaymentModel>(objPaymentModel, "FetchProposals", "Payment");
            #endregion
            return objPaymentModel;

        }

        //public PaymentServiceModel FetchPaymentProposals(PaymentServiceModel objPaymentModel)
        //{
        //    #region Call API
        //    objPaymentModel = WebApiLogic.GetPostComplexTypeToAPI<PaymentServiceModel>(objPaymentModel, "FetchPaymentProposals", "Payment");
        //    #endregion
        //    return objPaymentModel;

        //}

        //public PaymentServiceModel FetchRenewalProposals(PaymentServiceModel objPaymentModel)
        //{
        //    #region Call API
        //    objPaymentModel = WebApiLogic.GetPostComplexTypeToAPI<PaymentServiceModel>(objPaymentModel, "FetchRenewalProposals", "Payment");
        //    #endregion
        //    return objPaymentModel;

        //}

        //public PaymentServiceModel FetchRenewedPolicies(PaymentServiceModel objPaymentModel)
        //{
        //    #region Call API
        //    objPaymentModel = WebApiLogic.GetPostComplexTypeToAPI<PaymentServiceModel>(objPaymentModel, "FetchRenewedPolicies", "Payment");
        //    #endregion
        //    return objPaymentModel;

        //}

        //public PaymentServiceModel FetchRenewedAllPolicies(PaymentServiceModel objPaymentModel)
        //{
        //    #region Call API
        //    objPaymentModel = WebApiLogic.GetPostComplexTypeToAPI<PaymentServiceModel>(objPaymentModel, "FetchRenewedAllPolicies", "Payment");
        //    #endregion
        //    return objPaymentModel;

        //}

        //public PaymentServiceModel FetchPolicyHolderDetails(PaymentServiceModel objPaymentModel)
        //{
        //    #region Call API
        //    objPaymentModel = WebApiLogic.GetPostComplexTypeToAPI<PaymentServiceModel>(objPaymentModel, "FetchPolicyHolderDetails", "Payment");
        //    #endregion
        //    return objPaymentModel;

        //}

        public PaymentModel SaveProposalPaymentInfo(PaymentModel objPaymentModel)
        {


            #region Call API
            objPaymentModel = WebApiLogic.GetPostComplexTypeToAPI<PaymentModel>(objPaymentModel, "SaveProposalPaymentInfo", "Payment");       
            #endregion
            return objPaymentModel;

        }

        public PaymentModel MCashPayment(PaymentModel objPaymentModel)
        {
            #region Call API
            objPaymentModel = WebApiLogic.GetPostComplexTypeToAPI<PaymentModel>(objPaymentModel, "MCashPayment", "Payment");
            #endregion
            return objPaymentModel;

        }
        public PaymentModel SavePGTransaction(PaymentModel objPaymentModel)
        {
            #region Call API
            objPaymentModel = WebApiLogic.GetPostComplexTypeToAPI<PaymentModel>(objPaymentModel, "SavePGTransaction", "Payment");
            #endregion
            return objPaymentModel;

        }
        public PaymentModel UpdatePGTransaction(PaymentModel objPaymentModel)
        {
            #region Call API
            objPaymentModel = WebApiLogic.GetPostComplexTypeToAPI<PaymentModel>(objPaymentModel, "UpdatePGTransaction", "Payment");
            #endregion
            return objPaymentModel;

        }
        public PaymentModel CheckPaymentStatusUpdate(PaymentModel objPaymentModel)
        {
            #region Call API
            objPaymentModel = WebApiLogic.GetPostComplexTypeToAPI<PaymentModel>(objPaymentModel, "CheckPaymentStatusUpdate", "Payment");
            #endregion
            return objPaymentModel;

        }
        public PaymentModel FetchPendingPayments(PaymentModel objPaymentModel)
        {
            #region Call API
            objPaymentModel = WebApiLogic.GetPostComplexTypeToAPI<PaymentModel>(objPaymentModel, "FetchPendingPayments", "Payment");
            #endregion
            return objPaymentModel;

        }
    }
}
