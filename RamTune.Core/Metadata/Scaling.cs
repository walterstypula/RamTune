using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace RamTune.Core.Metadata
{
    [XmlRoot(ElementName = "scaling")]
    public class Scaling
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "units")]
        public string Units { get; set; }

        [XmlAttribute(AttributeName = "toexpr")]
        public string ToExpr { get; set; }

        [XmlAttribute(AttributeName = "frexpr")]
        public string FrExpr { get; set; }

        [XmlAttribute(AttributeName = "format")]
        public string Format { get; set; }

        [XmlAttribute(AttributeName = "min")]
        public string Min { get; set; }

        [XmlAttribute(AttributeName = "max")]
        public string Max { get; set; }

        [XmlAttribute(AttributeName = "inc")]
        public string Inc { get; set; }

        [XmlAttribute(AttributeName = "storagetype")]
        public StorageType StorageType { get; set; }

        [XmlAttribute(AttributeName = "endian")]
        public string Endian { get; set; }

        [XmlElement(ElementName = "data")]
        public List<ScalingData> Data { get; set; }
    }

    public class ScalingData
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }
}