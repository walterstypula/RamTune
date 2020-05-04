using RamTune.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows.Media;

namespace RamTune.UI.Coverters
{
    public class CellColorConverter : IMultiValueConverter
    {
        private List<Color> _heatMapColors = new List<Color>
        {
            Color.FromRgb(143, 143, 255),//Blue
            Color.FromRgb(0, 255, 255),//Cyan
            Color.FromRgb(0, 255, 0),//Green
            Color.FromRgb(255, 255, 0),//Yellow
            Color.FromRgb(255, 0, 0),//Red
        };

        /// <summary>
        /// Courtesy of Davide Dolla https://stackoverflow.com/a/37911674
        /// </summary>
        /// <param name="tableValue"></param>
        /// <param name="scalingMinValue"></param>
        /// <param name="scalingMaxValue"></param>
        /// <returns></returns>
        private Color GetColorForValue(double tableValue, double scalingMinValue, double scalingMaxValue)
        {
            var colorCount = _heatMapColors.Count - 1;

            var valuePercentage = (tableValue - scalingMinValue) / (scalingMaxValue - scalingMinValue); // value%
            var colorPercentage = 1d / colorCount; // % of each block of color. the last is the "100% Color"
            var colorBlockIndex = (int)(valuePercentage / colorPercentage); // the integer part repersents how many block to skip
            var valuePercentageResidual = valuePercentage - (colorBlockIndex * colorPercentage); //remove the part represented of block
            var percentTargetColor = valuePercentageResidual / colorPercentage; // % of color of this block that will be filled

            var cTarget = _heatMapColors[colorBlockIndex];
            var cNext = tableValue >= scalingMaxValue
                               ? _heatMapColors[colorBlockIndex]
                               : _heatMapColors[colorBlockIndex + 1];

            var deltaR = cNext.R - cTarget.R;
            var deltaG = cNext.G - cTarget.G;
            var deltaB = cNext.B - cTarget.B;

            var R = cTarget.R + (deltaR * percentTargetColor);
            var G = cTarget.G + (deltaG * percentTargetColor);
            var B = cTarget.B + (deltaB * percentTargetColor);

            var c = Color.FromRgb((byte)R, (byte)G, (byte)B);

            return c;
        }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var value = values[0] as string;
            var scaling = values[1] as Scaling;

            if (scaling == null
                || !float.TryParse(value, out var current)
                || !float.TryParse(scaling.Min, out var min)
                || !float.TryParse(scaling.Max, out var max))
            {
                return new SolidColorBrush(Colors.White);
            }

            var color = GetColorForValue(current, min, max);
            return new SolidColorBrush(color);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("CellColorConverter is a OneWay converter.");
        }
    }
}