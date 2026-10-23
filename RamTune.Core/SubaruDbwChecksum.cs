using System.IO;

namespace RamTune.Core
{
    /// <summary>
    /// Subaru 32-bit (DBW) ROM checksum, the "subarudbw" checksum module in EcuFlash definitions.
    /// A table of 17 entries {start, end, checksum} (big-endian uint32) sits 0x480 bytes before the end of the ROM.
    /// Each used entry stores 0x5AA5A55A minus the 32-bit sum of the words in [start, end]. Unused entries are {0, 0, 0x5AA5A55A}.
    /// </summary>
    public static class SubaruDbwChecksum
    {
        public const string ModuleName = "subarudbw";

        private const uint Magic = 0x5AA5A55A;
        private const int TableOffsetFromEnd = 0x480;
        private const int Entries = 17;

        /// <summary>
        /// Recalculates every checksum in the table. Returns the number of ranges corrected, or -1 when no valid table is found.
        /// </summary>
        public static int Fix(Stream rom)
        {
            var data = new byte[rom.Length];
            rom.Seek(0, SeekOrigin.Begin);
            rom.ReadExactly(data);

            var tableAddress = data.Length - TableOffsetFromEnd;

            if (!IsValidTable(data, tableAddress))
            {
                return -1;
            }

            var corrected = 0;

            for (var i = 0; i < Entries; i++)
            {
                var entry = tableAddress + i * 12;
                var start = ReadUInt32(data, entry);
                var end = ReadUInt32(data, entry + 4);

                if (start == 0 && end == 0)
                {
                    continue;
                }

                var checksum = Calculate(data, start, end);

                if (ReadUInt32(data, entry + 8) != checksum)
                {
                    corrected++;
                }

                rom.Seek(entry + 8, SeekOrigin.Begin);
                rom.Write(ToBigEndian(checksum), 0, 4);
            }

            return corrected;
        }

        private static bool IsValidTable(byte[] data, int tableAddress)
        {
            if (tableAddress <= 0)
            {
                return false;
            }

            var used = 0;

            for (var i = 0; i < Entries; i++)
            {
                var entry = tableAddress + i * 12;
                var start = ReadUInt32(data, entry);
                var end = ReadUInt32(data, entry + 4);

                if (start == 0 && end == 0)
                {
                    if (ReadUInt32(data, entry + 8) != Magic)
                    {
                        return false;
                    }

                    continue;
                }

                if (start >= end || end >= data.Length || start % 4 != 0)
                {
                    return false;
                }

                used++;
            }

            return used > 0;
        }

        private static uint Calculate(byte[] data, uint start, uint end)
        {
            uint sum = 0;

            for (var address = start; address < end; address += 4)
            {
                sum += ReadUInt32(data, (int)address);
            }

            return Magic - sum;
        }

        private static uint ReadUInt32(byte[] data, int address)
        {
            return (uint)(data[address] << 24 | data[address + 1] << 16 | data[address + 2] << 8 | data[address + 3]);
        }

        private static uint ReadUInt32(byte[] data, uint address)
        {
            return ReadUInt32(data, (int)address);
        }

        private static byte[] ToBigEndian(uint value)
        {
            return new[] { (byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)value };
        }
    }
}
