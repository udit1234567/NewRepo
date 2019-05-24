using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Common
{
    public class Questions
    {
        public string ProductCode { get; set; }
        public int QId { get; set; }
        public string Qset { get; set; }
        public int? SeqNo { get; set; }
        public string QText { get; set; }
        public string QTypeSet { get; set; }
        public string AnswerType { get; set; }
        public string AnswerText { get; set; }
        public string TextType { get; set; }
        public string QsetLink { get; set; }
        public DateTime dateTime { get; set; }
        public int? Index { get; set; }
    }
}
