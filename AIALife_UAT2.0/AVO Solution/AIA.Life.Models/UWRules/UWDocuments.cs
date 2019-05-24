using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AIA.Life.Models.UWRules.Documents
{
    [XmlRoot(ElementName = "ProdRiderData")]
    public class ProdRiderData
    {
        [XmlAttribute(AttributeName = "Gender")]
        public string Gender { get; set; }
        [XmlAttribute(AttributeName = "ProductId")]
        public string ProductId { get; set; }
        [XmlAttribute(AttributeName = "SumInsured")]
        public string SumInsured { get; set; }
        [XmlAttribute(AttributeName = "Age")]
        public string Age { get; set; }
        [XmlAttribute(AttributeName = "RiderId")]
        public string RiderId { get; set; }
        [XmlAttribute(AttributeName = "UserId")]
        public string UserId { get; set; }
    }

    [XmlRoot(ElementName = "ProdRiders")]
    public class ProdRiders
    {
        [XmlElement(ElementName = "ProdRiderData")]
        public List<ProdRiderData> ProdRiderData { get; set; }
    }

}
