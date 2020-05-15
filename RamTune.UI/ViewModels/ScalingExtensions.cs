using RamTune.Core.Metadata;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace RamTune.UI.ViewModels
{
    public static class ScalingExtensions
    {
        public static string ChangeValue(this Scaling scaling, string value, byte[] byteValue, Direction direction)
        {
            var changeDirection = direction == Direction.Increment ? 1 : -1;

            if (scaling.StorageType == StorageType.bloblist)
            {
                return GetNextBloblistItem(scaling.Data, value, changeDirection);
            }

            var updatedValue = CalcNextValue(value, scaling.Inc, changeDirection, byteValue, scaling);

            return updatedValue;
        }

        public static string SetValue(this Scaling scaling, string value)
        {
            return value;
        }

        private static string CalcNextValue(string currentValue, string adjustment, int direction, byte[] byteValue, Scaling scaling)
        {
            var val = decimal.Parse(currentValue);
            var adjust = decimal.Parse(adjustment, NumberStyles.Any) * direction;

            val += adjust;

            var newValue = $"{val}";
            var output = scaling.GetBytesValue(newValue);

            if (byteValue.SequenceEqual(output))
            {
                newValue = CalcNextValue(newValue, adjustment, direction, byteValue, scaling);
            }

            return newValue;
        }

        private static string GetNextBloblistItem(List<ScalingData> dataList, string value, int direction)
        {
            var dataIndex = dataList.FindIndex(d => d.Name == value);
            var nextIndex = dataIndex + direction;

            if (nextIndex < 0 || nextIndex >= dataList.Count)
            {
                return value;
            }

            return dataList[nextIndex].Name;
        }
    }
}