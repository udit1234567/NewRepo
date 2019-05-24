using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Opportunity
{
    public class ChildDetails
    {
      
        public string SumAssured { get; set; }
        public string Assured { get; set; }
        public string Relationship { get; set; }
        public int AgeNextBirthday { get; set; }
        public int CurrentAge { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string Gender { get; set; }       
        public string Name { get; set; }


    }
}
