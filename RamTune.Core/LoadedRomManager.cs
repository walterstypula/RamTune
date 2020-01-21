using RamTune.Core.Metadata;
using System.IO;
using System.Linq;
using System.Text;

namespace RamTune.Core
{
    public class LoadedRomManager : ITableReader
    {
        private readonly Stream _romStream;
        public Definition Rom { get; set; }

        public LoadedRomManager(Stream stream, DefinitionLoader loader)
        {
            _romStream = stream;
            Rom = Load(loader);
        }

        private Definition Load(DefinitionLoader loader)
        {
            var internalId = loader.Definitions.First(d =>
            {
                var romId = d.RomId;

                if (romId.InternalIdString == null)
                {
                    return false;
                }

                var b = SeekAndReadElement(romId.InternalIdAddress, "", romId.InternalIdString.Length);
                var romInternalId = Encoding.UTF8.GetString(b);

                return romInternalId == romId.InternalIdString;
            })?.RomId?.InternalIdString;

            return loader.GetDefinitionByInternalId(internalId);
        }

        public string[,] LoadTableData(TableBase table, int columnElements = 1, int rowElements = 1)
        {
            var tableData = new string[rowElements, columnElements];

            if (table is Axis axis && (axis.Type == TableType.StaticXAxis || axis.Type == TableType.StaticYAxis))
            {
                for (int rowElement = 0; rowElement < rowElements; rowElement++)
                {
                    for (int columnElement = 0; columnElement < columnElements; columnElement++)
                    {
                        var index = axis.Type == TableType.StaticXAxis ? rowElement : columnElement;

                        tableData[rowElement, columnElement] = axis.Data[index];
                    }
                }

                return tableData;
            }

            var endian = table.Scaling.Endian;
            var storageType = table.Scaling.StorageType;
            var byteArraySize = storageType.ParseStorageSize();

            var a = table.Scaling.ToExpr;
            var format = table.Scaling.Format;

            _romStream.Seek(table.Address.ConvertHexToInt(), SeekOrigin.Begin);

            for (int row = 0; row < rowElements; row++)
            {
                for (int column = 0; column < columnElements; column++)
                {
                    var byteValue = ReadElement(endian, byteArraySize);
                    tableData[row, column] = byteValue.ParseDataValue(storageType, a, format);
                }
            }

            return tableData;
        }

        private byte[] ReadElement(string endian, int byteArraySize)
        {
            byte[] b = new byte[byteArraySize];
            _romStream.Read(b, 0, byteArraySize);
            b.ReverseBytes(endian);

            return b;
        }

        private byte[] SeekAndReadElement(string address, string endian, int byteArraySize)
        {
            _romStream.Seek(address.ConvertHexToInt(), SeekOrigin.Begin);
            return ReadElement(endian, byteArraySize);
        }
    }
}