using AIA.Life.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Business.Common
{
    public class CommonBusiness
    {

        public AddressMaster GetAddresMasters(AddressMaster objAddressMaster)
        {
            #region Call API
            objAddressMaster = WebApiLogic.GetPostComplexTypeToAPI<AddressMaster>(objAddressMaster, "LoadAddressMaster", "Master");
            #endregion
            return objAddressMaster;
        }


        public ProductMasters LoadProductMasters(ProductMasters obj)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<ProductMasters>(obj, "LoadProductMasters", "Master");
            #endregion
            return obj;
        }

        public ProposalDetails GetPolicyDetails(ProposalDetails obj)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<ProposalDetails>(obj, "LoadProposalDetails", "Policy");
            #endregion
            return obj;

        }

        public AppVersion GetLatestVersion(AppVersion obj)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<AppVersion>(obj, "GetLatestVersion", "Policy");
            #endregion
            return obj;
        }

        public AIA.Life.Models.Policy.OCRResponse gooleVisionTextDecoderApi(AIA.Life.Models.Policy.OCRResponse obj)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.OCRResponse>(obj, "gooleVisionTextDecoderApi", "Policy");
            #endregion
            return obj;
        }

        public AppVersion UpdateLatestVersion(AppVersion Obj)
        {
            #region Call API
            Obj = WebApiLogic.GetPostComplexTypeToAPI<AppVersion>(Obj, "UpdateLatestVersion", "Policy");
            #endregion
            return Obj;
        }
        public void CreateServiceLog(TpServiceLog Obj)
        {
            WebApiLogic.FireForgetAPI(Obj, "CreateServiceLog", "Policy");
        }

        public SARFALDetails FetchSarAndFalDetails(SARFALDetails sARFALDetails)
        {
            #region Call API
            sARFALDetails = WebApiLogic.GetPostComplexTypeToAPI<SARFALDetails>(sARFALDetails, "FetchSarAndFalDetails", "Policy");
            #endregion
            return sARFALDetails;
        }
        public AIA.Life.Models.Common.AuthorizeUser CheckAuthorisation(AIA.Life.Models.Common.AuthorizeUser authorizeUser)
        {
            #region Call API
            authorizeUser = WebApiLogic.GetPostComplexTypeToAPI<AuthorizeUser>(authorizeUser, "CheckAuthorisation", "Policy");
            #endregion
            return authorizeUser;
        }
        public LifeAssuredAge CheckAgeChangeQuoteMembers(LifeAssuredAge lifeAssuredAge)
        {
            #region Call API
            lifeAssuredAge = WebApiLogic.GetPostComplexTypeToAPI<LifeAssuredAge>(lifeAssuredAge, "CheckAgeChangeQuoteMembers", "Policy");
            #endregion
            return lifeAssuredAge;
        }
        public AIA.Life.Models.UserManagement.ResouceManagent ContentListDetails(AIA.Life.Models.UserManagement.ResouceManagent objResourceCatagory)
        {
            #region Call API
            objResourceCatagory = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.UserManagement.ResouceManagent>(objResourceCatagory, "ContentList", "UserManagementData");
            #endregion
            return objResourceCatagory;
        }
    }
}
