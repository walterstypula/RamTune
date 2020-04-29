using MVVM;
using RamTune.Core.Metadata;

namespace RamTune.UI.ViewModels
{
    public class CellVM : ViewModelBase
    {
        public bool IsSelected
        {
            get { return Get<bool>(nameof(IsSelected)); }
            set { Set(nameof(IsSelected), value); }
        }

        public string Value
        {
            get { return Get<string>(nameof(Value)); }
            set { Set(nameof(Value), value); }
        }

        public Scaling Scaling { get; set; }

        public void IncrementValue()
        {
            Value = Scaling.IncrementValue(Value);
        }

        public void DecrementValue()
        {
            Value = Scaling.DecrementValue(Value);
        }

        public void SetValue(string value)
        {
            Value = Scaling.SetValue(value);
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

        public static string SetValue(this Scaling scaling, string value)
        {
            return value;
        }
    }
}