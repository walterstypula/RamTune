using MVVM;
using RamTune.Core.Metadata;
using System.Text;

namespace RamTune.UI.ViewModels
{
    public class CellVM : ViewModelBase
    {
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
                        : Scaling.GetDisplayValue(ByteValue);
            }
            private set
            {
                var byteValue = Scaling.GetBytesValue(value);
                ByteValue = byteValue;
                OnPropertyChanged(nameof(DisplayValue));
            }
        }

        public bool IsSelected
        {
            get { return Get<bool>(nameof(IsSelected)); }
            set { Set(nameof(IsSelected), value); }
        }

        public bool IsStatic
        {
            get { return Get<bool>(nameof(IsStatic)); }
            set { Set(nameof(IsStatic), value); }
        }

        public Scaling Scaling { get; set; }

        public void DecrementValue()
        {
            if (IsStatic)
            {
                return;
            }
            var currentValue = DisplayValue;
            var currentByteValue = ByteValue;
            DisplayValue = Scaling.DecrementValue(currentValue, currentByteValue);
        }

        public void IncrementValue()
        {
            if (IsStatic)
            {
                return;
            }

            var currentValue = DisplayValue;
            var currentByteValue = ByteValue;
            DisplayValue = Scaling.IncrementValue(currentValue, currentByteValue);
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
}