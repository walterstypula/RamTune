using System.Collections.Generic;
using System.Linq;
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

    public static class AxisExtensions
    {
        public static bool IsStaticAxis(this Axis axis)
        {
            var staticAxisTypes = new[] { (int)AxisType.StaticXAxis, (int)AxisType.StaticYAxis };

            return staticAxisTypes.Any(a => a == (int)axis.Type);
        }

        public static bool IsColumnAxis ( this Axis axis)
        {
            var axisTypes = new[] { (int)AxisType.StaticXAxis, (int)AxisType.XAxis };

            return axisTypes.Any(a => a == (int)axis.Type);
        }
    }
}