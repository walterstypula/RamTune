using RamTune.Core.Metadata;

namespace RamTune.Core
{
    public interface ITableReader
    {
        string[,] LoadTableData(TableBase table, int columnElements = 1, int rowElements = 1);
    }
}