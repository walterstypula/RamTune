using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace RamTune.Core.Metadata
{
    [XmlRoot(ElementName = "table")]
    public class Table : TableBase
    {
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        [XmlAttribute(AttributeName = "category")]
        public string Category { get; set; }

        [XmlAttribute(AttributeName = "level")]
        public int Level { get; set; }

        [XmlElement(ElementName = "table")]
        public List<Axis> Axis { get; set; }

        public Axis GetAxis(params AxisType[] axisType)
        {
            return Axis?.FirstOrDefault(tt => axisType.Any(at => (int)at == (int)tt.Type));
        }
    }
}