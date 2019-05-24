using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.CrossCutting
{
    public class CrossCuttingConstants
    {
        

        #region Relationships
        public const string Relationship = "PolicyRelationShip";
        public const string ProposerCode = "Proposer";
        public const string NomineeCode = "Nominee";
        public const string InsuredCode = "Insured";
        #endregion

       

        public const string itemTypeProduct = "Product";
        public const string itemTypeMenu = "Menu";
        public const string itemTypePayment = "Payment";
        public const string PageNameUsers = "Users";
        public const string PageNameRoles = "Roles";
        public const string AppNameAgent = "IMD";
        public const string AppNameEmployee = "Employee";
        public const string AllowedChars = "abcdefghijkmnGHJKLMNOPQRSTUVWXYZ234567opqrstuoM4jBpAw39Qoo3aSGyLiYnFqi5wYSpL2vwxyzABCDEF89@!#$&*";

        // UW Decision
        public const string UWDecisionAccepted = "184";
        public const string UWDecisionAcceptwithloading = "185";
        public const string UWDecisionDecline = "187";
        public const string UWDecisionPostPone = "1449";
        public const string UWDecisionWithDrawn = "2299";
        public const string UWDecisionReferToUW = "2298";
        public const string UWDecisionOutStandingReq = "1177";
        public const string UWDecisionCounterOffer = "186";
        public const string UWDecisionNotTaken = "1176";

        // Stage Status ID
        public const int PolicyStageStatusIssued = 194;
        public const int PolicyStageStatusDecline = 196;
        public const int PolicyStageStatusReferToUW = 193;
        public const int PolicyStageStatusOutStandingReq = 2375;
        public const int PolicyStageStatusCounterOffer = 2376;
        public const int PolicyStageStatusnotTaken = 2374;
        public const int PolicyStageStatusPending = 191;
        public const int PolicyStageStatusWithDrawn = 2491;
        public const int PolicyStageStatusPostPone = 2490;
        public const int PolicyStageStatusNotTakenUp = 2374;
        




    }

    public static class Codes
    {
        public static string GetErrorCode()
        {
            return DateTime.Now.Day.ToString().PadLeft(2, '0') +
                DateTime.Now.Month.ToString().PadLeft(2, '0') +
                DateTime.Now.Year.ToString().Substring(2, 2) +
                DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                DateTime.Now.Minute.ToString().PadLeft(2, '0') +
                DateTime.Now.Second.ToString().PadLeft(2, '0') +
                DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
        }
        public static string GetTransationNo()
        {
            return "T" + DateTime.Now.Day.ToString().PadLeft(2, '0') +
                DateTime.Now.Month.ToString().PadLeft(2, '0') +
                DateTime.Now.Year.ToString().Substring(2, 2) +
                DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                DateTime.Now.Minute.ToString().PadLeft(2, '0') +
                DateTime.Now.Second.ToString().PadLeft(2, '0') +
                DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
        }
    }
    public enum MedicalFeePaidBy
    {
        PaidByAIA = 1,
        PaidByCustomer = 2
    }
}
