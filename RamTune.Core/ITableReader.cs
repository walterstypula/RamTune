using RamTune.Core.Metadata;
using System.Collections.Generic;

namespace RamTune.Core
{
    public interface ITableReader
    {
        List<List<byte[]>> LoadAxisData(Axis axis);

        List<List<byte[]>> LoadTableData(TableBase table, int? columnElements, int? rowElements);


        public Definition Rom { get; set; }
    }
}