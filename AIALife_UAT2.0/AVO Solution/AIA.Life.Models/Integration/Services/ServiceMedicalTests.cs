using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Integration.Services
{
    public class MedicalTestRequest
    {
        public string productCode { get; set; }
        public string age { get; set; }
        public string sumInsured { get; set; }
      
    }

    public class Data
    {
        public List<string> MER { get; set; }
        public List<string> MUA { get; set; }
        public List<string> LIP { get; set; }
        public List<string> FBST { get; set; }
    }

    public class MedicalTestResponse
    {
        public string message { get; set; }
        public string status { get; set; }
        public Data data { get; set; }
    }

}
