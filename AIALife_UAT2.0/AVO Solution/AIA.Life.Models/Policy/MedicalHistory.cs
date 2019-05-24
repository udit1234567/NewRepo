using AIA.Life.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Policy
{
    public class MedicalHistoryQuestions
    {
        public int QuestionID { get; set; }
        public int MedicalHistoryID { get; set; }
        public string QuestionText { get; set; }
        public string Answer { get; set; }
        public string Details { get; set; }
        public string CotrolType { get; set; }
        public string SubType { get; set; }
        public string SubControlType { get; set; }
        public string SubQuestion { get; set; }
        public string SubAnswer { get; set; }
        public string Value { get; set; }
        public string Master { get; set; }
        //public int? ParentID { get; set; }
        public int? SequenceNo { get; set; }
        public string[] Diseases { get; set; }
        public string SelectedDiseases { get; set; }
        public List<MasterListItem> LstDropDownvalues { get; set; }

    }
}
