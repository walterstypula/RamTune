using MVVM;
using System.Collections.Generic;

namespace RamTune.UI.ViewModels
{
    public class GroupTableVm : ViewModelBase
    {
        public string Name
        {
            get { return Get<string>(nameof(Name)); }
            set { Set(nameof(Name), value); }
        }

        public IEnumerable<TableVm> Tables { get; set; }
    }
}