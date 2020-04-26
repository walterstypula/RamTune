using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace RamTune.Core.Metadata
{
    public class TableBase
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "address")]
        public string Address { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public TableType Type { get; set; }

        [XmlAttribute(AttributeName = "scaling")]
        public string ScalingName { get; set; }

        [XmlIgnore]
        public Scaling Scaling { get; set; }

        [XmlIgnore]
        public List<string> ScalingSource { get; } = new List<string>();

        public override string ToString()
        {
            return $"{Name} - {Scaling?.Units}";
        }
    }
}