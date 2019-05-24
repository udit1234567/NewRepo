using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AIA.Life.Models.UWRules
{

    [XmlRoot(ElementName = "item")]
    public class Item
    {
        [XmlAttribute(AttributeName = "ParameterName")]
        public string ParameterName { get; set; }
        [XmlAttribute(AttributeName = "ParameterValue")]
        public string ParameterValue { get; set; }
    }

    [XmlRoot(ElementName = "Rider")]
    public class Rider
    {
        [XmlElement(ElementName = "item")]
        public List<Item> Item { get; set; }
    }

    [XmlRoot(ElementName = "HealthQuestions")]
    public class HealthQuestions
    {
        [XmlElement(ElementName = "item")]
        public List<Item> Item { get; set; }
    }
    [XmlRoot(ElementName = "AdditionalQuestions")]
    public class AdditionalQuestions
    {
        [XmlElement(ElementName = "item")]
        public List<Item> Item { get; set; }
    }
    [XmlRoot(ElementName = "FamilyQuestions")]
    public class FamilyQuestions
    {
        [XmlElement(ElementName = "item")]
        public List<Item> Item { get; set; }
    }

    [XmlRoot(ElementName = "Alcohol")]
    public class Alcohol
    {
        [XmlElement(ElementName = "item")]
        public List<Item> Item { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "LifeStyle")]
    public class LifeStyle
    {
        [XmlElement(ElementName = "item")]
        public List<Item> Item { get; set; }
        [XmlElement(ElementName = "Alcohol")]
        public List<Alcohol> Alcohol { get; set; }
    }

    [XmlRoot(ElementName = "PreviousPolicy")]
    public class PreviousPolicy
    {
        [XmlElement(ElementName = "item")]
        public List<Item> Item { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "PreviousRiders")]
        public List<PreviousRiders> PreviousRiders { get; set; }

    }

    [XmlRoot(ElementName = "PreviousRiders")]
    public class PreviousRiders
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "item")]
        public List<Item> Item { get; set; }
    }


    [XmlRoot(ElementName = "Member")]
    public class Member
    {
        [XmlElement(ElementName = "item")]
        public List<Item> Item { get; set; }
        [XmlElement(ElementName = "Rider")]
        public Rider Rider { get; set; }
        [XmlElement(ElementName = "HealthQuestions")]
        public HealthQuestions HealthQuestions { get; set; }
        [XmlElement(ElementName = "AdditionalQuestions")]
        public AdditionalQuestions AdditionalQuestions { get; set; }
        [XmlElement(ElementName = "FamilyQuestions")]
        public FamilyQuestions FamilyQuestions { get; set; }
        [XmlElement(ElementName = "LifeStyle")]
        public LifeStyle LifeStyle { get; set; }
        [XmlElement(ElementName = "PreviousPolicy")]
        public List<PreviousPolicy> PreviousPolicy { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "Policy")]
    public class Policy
    {
        [XmlElement(ElementName = "item")]
        public List<Item> Item { get; set; }
        [XmlElement(ElementName = "Member")]
        public List<Member> Member { get; set; }
    }

}
