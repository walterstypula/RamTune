///////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nate Waddoups
// Lumberjack.cs
// Core application logic for Win32 and WinCE versions of the SSM logger
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;
using NateW.Ssm.ApplicationLogic.Properties;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Forms;
using System.Json;
using System.Threading.Tasks;

namespace NateW.Ssm.ApplicationLogic
{    
    public class TableData
    {
        public float[] XHeaders;
        public float[] YHeaders;
        public byte[][] Cells;
    }

    public partial class Lumberjack
    {
        private TableData tableData;

        private static bool TryGetHexValue(JsonValue configuration, string key, out int result)
        {
            result = 0;
            string stringValue = (string)configuration[key];

            if (!int.TryParse(
                stringValue,
                System.Globalization.NumberStyles.HexNumber,
                System.Globalization.CultureInfo.InvariantCulture,
                out result))
            {
                Trace(string.Format("LiveTuning Initialization: {0} must be a hex number.", key));
                return false;
            }

            return true;
        }

        public async Task<bool> LiveTuningInitialize()
        {
            using (Stream configurationStream = File.OpenRead("LiveTuning.json"))
            {
                JsonValue configuration = JsonValue.Load(configurationStream);
                string internalId = (string)configuration["InternalId"];
                int internalIdAddress;
                int tableAddress;

                if (!TryGetHexValue(configuration, "InternalIdAddress", out internalIdAddress))
                {
                    return false;
                }

                if (!TryGetHexValue(configuration, "TableStructureAddress", out tableAddress))
                {
                    return false;
                }

                long internalId1 = await this.logger.Query4Bytes(internalIdAddress);
                long internalId2 = await this.logger.Query4Bytes(internalIdAddress + 4);

                List<byte> idBytes = new List<byte>();
                idBytes.AddRange(BitConverter.GetBytes(internalId1));
                idBytes.AddRange(BitConverter.GetBytes(internalId2));
                string actualId = System.Text.Encoding.ASCII.GetString(idBytes.ToArray());

                if (string.Compare(actualId, internalId) == 0)
                {
                    Trace(string.Format("LiveTuning Initialization: Expected '{0}' but found '{1}'", internalId, actualId));
                    return false;
                }

                int fourBytes = await this.logger.Query4Bytes(tableAddress);
                int xSize = fourBytes & 0xFFFF;
                int ySize = fourBytes >> 16;
                
                if ((xSize < 0) || (xSize > 25))
                {
                    Trace(string.Format("LiveTuning Initialization: {0} X size seems unlikely. Double check that table address.", xSize));
                    return false;
                }

                if ((ySize < 0) || (ySize > 25))
                {
                    Trace(string.Format("LiveTuning Initialization: {0} Y size seems unlikely. Double check that table address.", xSize));
                    return false;
                }

                int tableFlags = await this.logger.Query4Bytes(tableAddress + 16);
                byte tableType = (byte)(tableFlags >> 24);
                if (tableType != 0x04)
                {
                    Trace(string.Format("LiveTuning Initialization: Unsupported table type {0}.", tableType));
                    return false;
                }

                this.tableData = new TableData();
                int tableXAddress = await this.logger.Query4Bytes(tableAddress + 4);
                this.tableData.XHeaders = await this.LoadFloatArray(tableXAddress, xSize);
                
                int tableYAddress = await this.logger.Query4Bytes(tableAddress + 8);
                this.tableData.YHeaders = await this.LoadFloatArray(tableYAddress, ySize);

                int tableDataAddress = await this.logger.Query4Bytes(tableAddress + 12);
                this.tableData.Cells = new byte[xSize][];

                for(int x = 0; x < xSize; x++)
                {
                    tableData.Cells[x] = await this.LoadByteArray(tableDataAddress + (x * ySize), ySize);
                }

                Trace("LiveTuning initialization successful.");
                return true;
            }
        }

        private async Task<float[]> LoadFloatArray(int baseAddress, int length)
        {
            float[] result = new float[length];
            for (int index = 0; index < length; index++)
            {
                float value = (float)await this.logger.Query4Bytes(baseAddress + index * 4);
                result[index] = value;
            }

            return result;
        }

        /// <summary>
        /// TODO: support length that is not a multiple of 4
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private async Task<byte[]> LoadByteArray(int baseAddress, int length)
        {
            byte[] result = new byte[length];
            for (int index = 0; index < length; index += 4)
            {
                int value = await this.logger.Query4Bytes(baseAddress + index * 4);

                result[index + 0] = (byte) (value >> 24);
                result[index + 1] = (byte) (value >> 16);
                result[index + 2] = (byte) (value >> 8);
                result[index + 3] = (byte) value;
            }

            return result;
        }
    }
}
