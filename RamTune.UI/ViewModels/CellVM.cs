using MVVM;
using RamTune.Core.Metadata;
using System;
using System.Linq;
using System.Text;

namespace RamTune.UI.ViewModels
{
    public class CellVM : ViewModelBase
    {
        private byte[] _unmodifiedValue = null;

        public byte[] ByteValue
        {
            get { return Get<byte[]>(nameof(ByteValue)); }
            set
            {
                if (_unmodifiedValue == null)
                {
                    _unmodifiedValue = Get<byte[]>(nameof(ByteValue));
                }

                Set(nameof(ByteValue), value);
                OnPropertyChanged(nameof(IsDirty));
            }
        }

        public bool IsDirty
        {
            get { return !_unmodifiedValue?.SequenceEqual(Get<byte[]>(nameof(ByteValue))) ?? false; }
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

        public void ChangeValue(Direction direction)
        {
            if (IsStatic)
            {
                return;
            }
            var currentValue = DisplayValue;
            var currentByteValue = ByteValue;
            DisplayValue = Scaling.ChangeValue(currentValue, currentByteValue, direction);
        }

        public void SetValue(string value)
        {
            if (IsStatic)
            {
                return;
            }

            DisplayValue = Scaling.SetValue(value);
        }

        public void ResetValue()
        {
            ByteValue = _unmodifiedValue;
            OnPropertyChanged(nameof(DisplayValue));
        }
    }
}