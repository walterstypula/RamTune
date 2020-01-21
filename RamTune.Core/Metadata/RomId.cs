using System.Xml;
using System.Xml.Serialization;

namespace RamTune.Core.Metadata
{
    [XmlRoot(ElementName = "romid")]
    public class RomId
    {
        [XmlElement(ElementName = "xmlid")]
        public string XmlId { get; set; }

        [XmlElement(ElementName = "internalidaddress")]
        public string InternalIdAddress { get; set; }

        [XmlElement(ElementName = "internalidstring")]
        public string InternalIdString { get; set; }

        [XmlElement(ElementName = "ecuid")]
        public string EcuId { get; set; }

        [XmlElement(ElementName = "year")]
        public string Year { get; set; }

        [XmlElement(ElementName = "market")]
        public string Market { get; set; }

        [XmlElement(ElementName = "make")]
        public string Make { get; set; }

        [XmlElement(ElementName = "model")]
        public string Model { get; set; }

        [XmlElement(ElementName = "submodel")]
        public string SubModel { get; set; }

        [XmlElement(ElementName = "transmission")]
        public string Transmission { get; set; }

        [XmlElement(ElementName = "memmodel")]
        public string MemModel { get; set; }

        [XmlElement(ElementName = "flashmethod")]
        public string FlashMethod { get; set; }

        [XmlElement(ElementName = "checksummodule")]
        public string ChecksumModule { get; set; }

        [XmlElement(ElementName = "filesize")]
        public string FileSize { get; set; }
    }
}