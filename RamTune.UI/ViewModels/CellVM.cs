using MVVM;

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
    }
}