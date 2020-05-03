using RamTune.Core.Metadata;
using System;
using System.Windows.Data;
using System.Windows.Media;

namespace RamTune.UI.Coverters
{
    public class CellColorConverter : IMultiValueConverter
    {
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

            var color = GetColor(min, max, current);

            return new SolidColorBrush(color);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("CellColorConverter is a OneWay converter.");
        }

        private static Color GetColor(float min, float max, float value)
        {
            double middle = min + (max - min) / 2;
            double brightness;
            double unbrightness;
            if (value > middle)
            {
                brightness = (value - middle) / (max - middle);
                unbrightness = 1 - brightness;

                byte red = 255;
                byte green = 255;
                byte blue = (byte)(255 * unbrightness);

                return Color.FromRgb(red, green, blue);
            }
            else
            {
                brightness = (middle - value) / (middle - min);
                unbrightness = ((1 - brightness) + 1) / 2;

                byte red = (byte)(255 * unbrightness);
                byte green = (byte)(255 * unbrightness);
                byte blue = 255;
                return Color.FromRgb(red, green, blue);
            }
        }
    }
}