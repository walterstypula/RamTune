using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace RamTune.Core.Metadata
{
    [XmlRoot(ElementName = "roms")]
    public class Roms
    {
        [XmlElement(ElementName = "rom")]
        public List<Definition> Rom { get; set; }
    }
}