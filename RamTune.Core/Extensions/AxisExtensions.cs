using System.Linq;

namespace RamTune.Core.Metadata
{
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