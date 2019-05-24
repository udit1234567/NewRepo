using AIA.Life.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.UWRules
{
    public class UWRule
    {
        public UWRule()
        {
            LstRuleParameters = new List<RuleParameters>();
        }
        public string RuleName { get; set; }
        public decimal RuleID { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string ClassName { get; set; }
        public List<string> ProductName { get; set; }
        public string StrProducts { get; set; }
        public string Description { get; set; }
        public DateTime? EffectDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public decimal RuleSetID { get; set; }
        public string ExistingRuleSetName { get; set; }
        public bool IsEditMode { get; set; }

        public List<MasterListItem> LstClass { get; set; }
        public List<MasterListItem> LstProducts { get; set; }
        public List<MasterListItem> LstParameters { get; set; }
        public List<MasterListItem> LstParameterTypes { get; set; }

        public List<RuleParameters> LstRuleParameters { get; set; }
        public List<MasterListItem> LstRuleOutCome { get; set; }


        public List<MasterListItem> LstRuleName { get; set; }

        public List<RuleSetInfo> LstRuleSet { get; set; }

        public string RuleSetName { get; set; }
        public int RuleSetPriority { get; set; }
        public string RuleOutCome { get; set; }
        public string Message { get; set; }
        public string RuleSetType { get; set; }
    }



    public class RuleParameters
    {
        public int? ParameterID { get; set; }
        public string ParameterName { get; set; }
        public string Parametertype { get; set; }

        public string List { get; set; }
        public DateTime? FromDT { get; set; }
        public DateTime? ToDT { get; set; }
        public string StrFrom { get; set; }
        public string StrTo { get; set; }

        public int IntFrom { get; set; }
        public int IntTo { get; set; }



    }

    public class RuleSetInfo
    {
        public int RuleSetID { get; set; }
        public string RuleSetName { get; set; }
        public string Description { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string Action { get; set; }

    }



}
