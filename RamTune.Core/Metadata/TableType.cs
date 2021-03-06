﻿using System.Xml.Serialization;

namespace RamTune.Core.Metadata
{
    public enum AxisType
    {
        YAxis = TableType.YAxis,

        XAxis = TableType.XAxis,

        StaticYAxis = TableType.StaticYAxis,

        StaticXAxis = TableType.StaticXAxis,
    }

    public enum TableType
    {
        Unknown,

        [XmlEnum(Name = "1D")]
        OneD,

        [XmlEnum(Name = "2D")]
        TwoD,

        [XmlEnum(Name = "3D")]
        ThreeD,

        [XmlEnum(Name = "1DRam")]
        OneDRam,

        [XmlEnum(Name = "2DRam")]
        TwoDRam,

        [XmlEnum(Name = "3DRam")]
        ThreeDRam,

        [XmlEnum(Name = "Y Axis")]
        YAxis,

        [XmlEnum(Name = "X Axis")]
        XAxis,

        [XmlEnum(Name = "Static Y Axis")]
        StaticYAxis,

        [XmlEnum(Name = "Static X Axis")]
        StaticXAxis,
    }
}