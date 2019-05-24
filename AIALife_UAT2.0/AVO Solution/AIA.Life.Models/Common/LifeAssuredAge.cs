using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Common
{
    public class LifeAssuredAge
    {
        public string QuoteNo { get; set; }
        public DateTime? Rcd { get; set; }
        public bool MainLifeAge { get; set; }
        public bool SpouseAge { get; set; }
        public bool Child1Age { get; set; }
        public bool Child2Age { get; set; }
        public bool Child3Age { get; set; }
        public bool Child4Age { get; set; }
        public bool Child5Age { get; set; }
    }
}
