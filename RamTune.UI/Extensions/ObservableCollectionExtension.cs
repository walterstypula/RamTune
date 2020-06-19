using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RamTune.Core.Metadata;

namespace RamTune.UI.ViewModels
{
    public static class ObservableCollectionExtension
    {
        public static ObservableCollection<ObservableCollection<CellVm>> ToCellObservableCollection(this IEnumerable<IEnumerable<byte[]>> tableData, Scaling scaling, bool? isStaticAxis, int? address)
        {
            if (tableData == null)
            {
                return null;
            }

            var rows = tableData.Select(o =>
            {
                var columns = o.Select(s =>
                {
                    var cell = new CellVm
                    {
                        ByteValue = s,
                        Scaling = scaling,
                        IsStatic = isStaticAxis.GetValueOrDefault(),
                        Address = address.GetValueOrDefault()
                    };

                    if (address.HasValue)
                    {
                        address++;
                    }

                    return cell;
                });

                return new ObservableCollection<CellVm>(columns);
            });

            return new ObservableCollection<ObservableCollection<CellVm>>(rows);
        }
    }
}