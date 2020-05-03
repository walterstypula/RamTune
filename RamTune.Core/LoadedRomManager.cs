using RamTune.Core.Metadata;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RamTune.Core
{
    public class LoadedRomManager : ITableReader
    {
        private readonly Stream _romStream;
        private long offset = 192 * 1024;
        public Definition Rom { get; set; }

        public LoadedRomManager(Stream stream, DefinitionLoader loader)
        {
            _romStream = stream;

            if (_romStream.Length <= offset)
            {
                offset -= _romStream.Length;
            }
            else if (_romStream.Length > offset)
            {
                offset = 0;
            }

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

                var b = _romStream.SeekAndReadElement(romId.InternalIdAddress, "", romId.InternalIdString.Length);
                var romInternalId = Encoding.UTF8.GetString(b);

                return romInternalId == romId.InternalIdString;
            })?.RomId?.InternalIdString;

            return loader.GetDefinitionByInternalId(internalId);
        }

        public List<List<string>> LoadAxisData(Axis axis)
        {
            if (axis == null)
            {
                return null;
            }

            if (axis.IsStaticAxis())
            {
                var list = LoadStaticAxisData(axis).ToList();

                if (axis.IsColumnAxis())
                {
                    return new List<List<string>> { list };
                }
                else
                {
                    return list.Select(s => new List<string> { s }).ToList();
                }
            }

            if (axis.IsColumnAxis())
            {
                return LoadTableData(axis, axis.Elements, null);
            }
            else
            {
                return LoadTableData(axis, null, axis.Elements);
            }
        }

        private List<string> LoadStaticAxisData(Axis axis)
        {
            return axis.Data;
        }

        public List<List<string>> LoadTableData(TableBase table, int? columnElements, int? rowElements)
        {
            var columns = columnElements ?? 1;
            var rows = rowElements ?? 1;

            var endian = table.Scaling.Endian;
            var storageType = table.Scaling.StorageType;
            var expression = table.Scaling.ToExpr;
            var format = table.Scaling.Format;
            var byteArraySize = table.Scaling.ParseStorageSize();
            var address = table.Address.ConvertHexToInt();

            if (address > offset)
            {
                address -= (int)offset;
            }

            var tableData = _romStream.Read(address, columns, rows, endian, byteArraySize);

            return tableData.ParseDataValue(storageType, expression, format);
        }
    }
}