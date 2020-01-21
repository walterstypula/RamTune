using System.Xml.Serialization;

namespace RamTune.Core.Metadata
{
    public enum StorageType
    {
        Unknown,

        [XmlEnum(Name = "uint32")]
        uint32,

        [XmlEnum(Name = "uint16")]
        uint16,

        [XmlEnum(Name = "float")]
        @float,

        [XmlEnum(Name = "uint8")]
        uint8,

        [XmlEnum(Name = "bloblist")]
        bloblist,
    }
}