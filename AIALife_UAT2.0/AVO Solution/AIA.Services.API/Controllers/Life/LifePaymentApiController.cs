using AIA.Life.Business.Payment;
using AIA.Life.Models.Integration.Payment;
using AIA.Life.Models.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AIA.Services.API.Controllers.Life
{
    public class LifePaymentApiController : ApiController
    {
        public PaymentModel FetchProposals(PaymentModel objPaymentModel)
        {
            PaymentBusiness objPaymentBusiness = new PaymentBusiness();
            return objPaymentBusiness.FetchProposals(objPaymentModel);
        }

        public PaymentServiceModel FetchPaymentProposals(PaymentServiceModel objPaymentModel)
        {
            PaymentBusiness objPaymentBusiness = new PaymentBusiness();
            return objPaymentBusiness.FetchPaymentProposals(objPaymentModel);
        }

        public PaymentServiceModel FetchRenewalProposals(PaymentServiceModel objPaymentModel)
        {
            PaymentBusiness objPaymentBusiness = new PaymentBusiness();
            return objPaymentBusiness.FetchRenewalProposals(objPaymentModel);
        }

        public PaymentServiceModel FetchRenewedPolicies(PaymentServiceModel objPaymentModel)
        {
            PaymentBusiness objPaymentBusiness = new PaymentBusiness();
            return objPaymentBusiness.FetchRenewedPolicies(objPaymentModel);
        }

        public PaymentServiceModel FetchRenewedAllPolicies(PaymentServiceModel objPaymentModel)
        {
            PaymentBusiness objPaymentBusiness = new PaymentBusiness();
            return objPaymentBusiness.FetchRenewedAllPolicies(objPaymentModel);
        }
        public PaymentServiceModel FetchPolicyHolderDetails(PaymentServiceModel objPaymentModel)
        {
            PaymentBusiness objPaymentBusiness = new PaymentBusiness();
            return objPaymentBusiness.FetchPolicyHolderDetails(objPaymentModel);
        }

        public PaymentModel SaveProposalPaymentInfo(PaymentModel objPaymentModel)
        {
            PaymentBusiness objPaymentBusiness = new PaymentBusiness();
            return objPaymentBusiness.SaveProposalPaymentInfo(objPaymentModel);
        }
        public PaymentModel MCashPayment(PaymentModel objPaymentModel)
        {
            PaymentBusiness objPaymentBusiness = new PaymentBusiness();
            return objPaymentBusiness.MCashPayment(objPaymentModel);
        }
        public PaymentModel SavePGTransaction(PaymentModel objPaymentModel)
        {
            PaymentBusiness objPaymentBusiness = new PaymentBusiness();
            return objPaymentBusiness.SavePGTransaction(objPaymentModel);
        }
        public PaymentModel UpdatePGTransaction(PaymentModel objPaymentModel)
        {
            PaymentBusiness objPaymentBusiness = new PaymentBusiness();
            return objPaymentBusiness.UpdatePGTransaction(objPaymentModel);
        }
        public PaymentModel CheckPaymentStatusUpdate(PaymentModel objPaymentModel)
        {
            PaymentBusiness objPaymentBusiness = new PaymentBusiness();
            return objPaymentBusiness.CheckPaymentStatusUpdate(objPaymentModel);
        }
        public PaymentModel FetchPendingPayments(PaymentModel objPaymentModel)
        {
            PaymentBusiness objPaymentBusiness = new PaymentBusiness();
            return objPaymentBusiness.FetchPendingPayments(objPaymentModel);
        }
    }
}
