using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace RamTune.Core.Metadata
{
    [XmlRoot(ElementName = "rom")]
    public class Definition
    {
        [XmlElement(ElementName = "romid")]
        public RomId RomId { get; set; }

        [XmlElement(ElementName = "include")]
        public string Base { get; set; }

        [XmlElement(ElementName = "table")]
        public List<Table> Tables { get; set; }
    }
}