using RamTune.Core.Metadata;
using System.Collections.Generic;

namespace RamTune.Core
{
    public interface ITableReader
    {
        List<List<byte[]>> LoadAxisData(Axis axis);

        List<List<byte[]>> LoadTableData(TableBase table, int? columnElements, int? rowElements);

        void ApplyChanges(long address, byte[] bytes);

        Definition Rom { get; set; }

        void Save(string filePath);
    }
}