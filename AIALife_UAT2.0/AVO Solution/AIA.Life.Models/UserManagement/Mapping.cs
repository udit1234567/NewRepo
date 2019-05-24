using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.UserManagement
{
    public class Mapping
    {
        public string mappingcode1 { get; set; }
        public string mappingcode2 { get; set; }
        public decimal index { get; set; }
        public IEnumerable<Mapping> ListMappingDetails { get; set; }
    }

    public class SMMapping
    {

    }
}
