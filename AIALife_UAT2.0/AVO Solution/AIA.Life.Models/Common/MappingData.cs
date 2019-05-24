using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Common
{
    public class MappingData
    {
        public string SourceObjectProperty { get; set; }
        public string FieldValue { get; set; }
        public bool IsHardCoded { get; set; }
        public string DataType { get; set; }
        public int? PropertyLength { get; set; }
        public string ColumnName { get; set; }
        public int Occurance { get; set; }
    }
}
