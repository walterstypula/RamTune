using System.Xml.Serialization;

namespace RamTune.Core.Metadata
{
    public enum StorageType
    {
        Unknown,

        [XmlEnum(Name = "uint32")]
        uint32,

        [XmlEnum(Name = "int32")]
        int32,

        [XmlEnum(Name = "uint16")]
        uint16,

        [XmlEnum(Name = "int16")]
        int16,

        [XmlEnum(Name = "float")]
        @float,

        [XmlEnum(Name = "uint8")]
        uint8,

        [XmlEnum(Name = "int8")]
        int8,

        [XmlEnum(Name = "bloblist")]
        bloblist,
    }
}