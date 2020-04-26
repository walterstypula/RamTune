using RamTune.Core.Metadata;
using System.Collections.Generic;

namespace RamTune.Core
{
    public interface ITableReader
    {
        List<string> LoadAxisData(Axis axis);

        List<List<string>> LoadTableData(TableBase table, int? columnElements, int? rowElements);


        public Definition Rom { get; set; }
    }
}