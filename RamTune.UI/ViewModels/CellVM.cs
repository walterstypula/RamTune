using MVVM;
using RamTune.Core.Metadata;
using System.Linq;
using System.Text;

namespace RamTune.UI.ViewModels
{
    public class CellVM : ViewModelBase
    {
        public bool IsStatic
        {
            get { return Get<bool>(nameof(IsStatic)); }
            set { Set(nameof(IsStatic), value); }
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
                return IsStatic
                        ? Encoding.UTF8.GetString(ByteValue)
                        : ByteValue.ParseDataValue(Scaling.StorageType, Scaling.ToExpr, Scaling.Format);
            }
            private set
            {
                var output = value.ParseStringValue(Scaling.StorageType, Scaling.FrExpr);
                var currentValue = Get<byte[]>(nameof(ByteValue));

                var isIncrement = decimal.Parse(value) > decimal.Parse(DisplayValue);

                while (currentValue.SequenceEqual(output))
                {
                    value = isIncrement
                                ? Scaling.IncrementValue(value)
                                : Scaling.DecrementValue(value);

                    output = value.ParseStringValue(Scaling.StorageType, Scaling.FrExpr);
                }

                Set(nameof(ByteValue), output);
                OnPropertyChanged(nameof(DisplayValue));
            }
        }

        public Scaling Scaling { get; set; }

        public void IncrementValue()
        {
            if (IsStatic)
            {
                return;
            }

            DisplayValue = Scaling.IncrementValue(DisplayValue);
        }

        public void DecrementValue()
        {
            if (IsStatic)
            {
                return;
            }

            DisplayValue = Scaling.DecrementValue(DisplayValue);
        }

        public void SetValue(string value)
        {
            if (IsStatic)
            {
                return;
            }

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