using MVVM;
using RamTune.Core;
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

        /// <summary>
        /// Set by the ROM compare view when this cell's value differs from the matching cell in the other ROM.
        /// </summary>
        public bool IsDifferent
        {
            get { return Get<bool>(nameof(IsDifferent)); }
            set { Set(nameof(IsDifferent), value); }
        }

        public long? Address { get; set; }

        public Scaling Scaling { get; set; }

        public void ChangeValue(Direction direction, ChangeType changeType)
        {
            if (IsStatic)
            {
                return;
            }
            var currentValue = DisplayValue;
            var currentByteValue = ByteValue;
            DisplayValue = Scaling.ChangeValue(currentValue, currentByteValue, direction, changeType);
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

        /// <summary>
        /// Takes the value of <paramref name="source"/>: byte for byte when both scalings store values the same way,
        /// otherwise through the display value so it is converted to this cell's scaling.
        /// </summary>
        public void CopyValueFrom(CellVM source)
        {
            if (IsStatic || source.IsStatic)
            {
                return;
            }

            if (Scaling.IsCompatibleWith(source.Scaling))
            {
                ByteValue = source.ByteValue.ToArray();
                OnPropertyChanged(nameof(DisplayValue));
                return;
            }

            SetValue(source.DisplayValue);
        }

        public bool HasSameValueAs(CellVM other)
        {
            if (IsStatic || other.IsStatic || Scaling.IsCompatibleWith(other.Scaling))
            {
                return ByteValue.SequenceEqual(other.ByteValue);
            }

            return DisplayValue == other.DisplayValue;
        }
    }
}