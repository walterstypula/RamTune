using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace RamTune.Core.Metadata
{
    [XmlRoot(ElementName = "table")]
    public class Axis : TableBase
    {
        [XmlAttribute(AttributeName = "elements")]
        public int Elements { get; set; }

        [XmlElement(ElementName = "data")]
        public List<string> Data { get; set; }
    }
}