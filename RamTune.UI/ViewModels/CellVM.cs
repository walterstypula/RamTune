using MVVM;
using RamTune.Core.Metadata;
using System.Linq;

namespace RamTune.UI.ViewModels
{
    public class CellVM : ViewModelBase
    {
        public bool IsStaticAxis
        {
            get { return Get<bool>(nameof(IsStaticAxis)); }
            set { Set(nameof(IsStaticAxis), value); }
        }

        public bool IsSelected
        {
            get { return Get<bool>(nameof(IsSelected)); }
            set { Set(nameof(IsSelected), value); }
        }

        public byte[] ByteValue
        {
            get { return Get<byte[]>(nameof(ByteValue)); }
            set { Set(nameof(ByteValue), value); }
        }

        public string DisplayValue
        {
            get
            {
                var scaling = Scaling;
                var storageType = scaling.StorageType;
                var expression = scaling.ToExpr;
                var format = scaling.Format;

                return ByteValue.ParseDataValue(storageType, expression, format);
            }
            set
            {
                var scaling = Scaling;
                var storageType = scaling.StorageType;
                var expression = scaling.FrExpr;

                var currentValue = Get<byte[]>(nameof(ByteValue));
                var output = value.ParseStringValue(storageType, expression);

                while (currentValue.SequenceEqual(output))
                {
                    value = Scaling.IncrementValue(value);
                    output = value.ParseStringValue(storageType, expression);
                }

                Set(nameof(ByteValue), output);
                OnPropertyChanged(nameof(DisplayValue));
            }
        }

        public Scaling Scaling { get; set; }

        public void IncrementValue()
        {
            DisplayValue = Scaling.IncrementValue(DisplayValue);
        }

        public void DecrementValue()
        {
            DisplayValue = Scaling.DecrementValue(DisplayValue);
        }

        public void SetValue(string value)
        {
            DisplayValue = Scaling.SetValue(value);
        }
    }

    public static class ScalingExtensions
    {
        public static string IncrementValue(this Scaling scaling, string value)
        {
            var val = decimal.Parse(value);
            var inc = decimal.Parse(scaling.Inc);

            val = val + inc;

            return $"{val}";
        }

        public static string DecrementValue(this Scaling scaling, string value)
        {
            var val = decimal.Parse(value);
            var inc = decimal.Parse(scaling.Inc);

            val = val - inc;

            return $"{val}";
        }

        public static byte[] SetValue(this Scaling scaling, byte[] value)
        {
            return new byte[] { };
        }

        public static string SetValue(this Scaling scaling, string value)
        {
            return value;
        }
    }
}