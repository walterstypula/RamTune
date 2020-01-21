using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace RamTune.Core.Metadata
{
    [XmlRoot(ElementName = "rom")]
    public class DefinitionScalings
    {
        [XmlElement(ElementName = "scaling")]
        public List<Scaling> Scalings { get; set; }
    }
}