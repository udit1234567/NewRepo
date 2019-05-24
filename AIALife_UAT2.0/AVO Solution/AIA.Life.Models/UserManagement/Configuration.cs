using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.UserManagement
{
    public class Configuration
    {
        public List<ConfigurationGridData> ObjConfigurationGridData { get; set; }

        public class ConfigurationGridData
        {
            public int ConvertionID { get; set; }
            public string ConvertionFrom { get; set; }
            public string ConvertionTo { get; set; }
            public string SpecifiedTimeLine { get; set; }
        }
    }
}
