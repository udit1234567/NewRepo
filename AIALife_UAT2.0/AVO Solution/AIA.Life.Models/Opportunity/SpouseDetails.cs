using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Opportunity
{
   public class SpouseDetails
    {
       
        public int AgeNextBirthday { get; set; }
        public int CurrrentAge { get; set; }
        public DateTime? DOB { get; set; }
        public string SpouseName { get; set; }
        public string SpouseNIC { get; set; }
        public string Gender { get; set; }
        public string Occupation { get; set; }
        public string AssuredName { get; set; }
        
    }
}
