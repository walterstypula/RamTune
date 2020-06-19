using System.Xml.Serialization;

namespace RamTune.Core.Metadata
{
    public class ScalingData
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }
}