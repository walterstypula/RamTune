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

        public TableData TableData
        {
            get
            {
                return this.tableData;
            }
        }

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
            using (Stream configurationStream = File.OpenRead("Configuration\\LiveTuning.json"))
            {
                JsonValue configuration = JsonValue.Load(configurationStream);

                // It would be nice to use the "internal ID" here (like "A2WC522N") but
                // that value is stored in memory that the logger cannot access.
                // So we use the less-common "ECU Identifier" instead ("2F12785206").
                string ecuIdentifier = (string)configuration["EcuIdentifier"];
                int tableAddress;

                if (!TryGetHexValue(configuration, "TableStructureAddress", out tableAddress))
                {
                    return false;
                }

                if (string.Compare(this.logger.EcuIdentifier, ecuIdentifier) != 0)
                {
                    Trace(string.Format("LiveTuning Initialization: Expected '{0}' but found '{1}'", ecuIdentifier, this.logger.EcuIdentifier));
                    return false;
                }
                
                int fourBytes = await this.logger.Query4Bytes(tableAddress);
                int xSize = (fourBytes & 0xFF00) >> 8;
                int ySize = fourBytes >> 24;
                
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
                byte tableType = (byte)(tableFlags & 0xFF);
                if (tableType != 0x04)
                {
                    Trace(string.Format("LiveTuning Initialization: Unsupported table type {0}.", tableType));
                    return false;
                }

                this.tableData = new TableData();
                int tableXAddress = EndianSwap(await this.logger.Query4Bytes(tableAddress + 4));
                this.tableData.XHeaders = await this.LoadFloatArray(tableXAddress, xSize);

                int tableYAddress = EndianSwap(await this.logger.Query4Bytes(tableAddress + 8));
                this.tableData.YHeaders = await this.LoadFloatArray(tableYAddress, ySize);

                int tableDataAddress = EndianSwap(await this.logger.Query4Bytes(tableAddress + 12));
                this.tableData.Cells = new byte[xSize][];

                for(int x = 0; x < xSize; x++)
                {
                    tableData.Cells[x] = await this.LoadByteArray(tableDataAddress + (x * ySize), ySize);
                }

                Trace("LiveTuning initialization successful.");
                return true;
            }
        }

        private int EndianSwap(int input)
        {
            int output = input & 0xFF;
            output <<= 8;
            input >>= 8;
            output |= input & 0xFF;
            output <<= 8;
            input >>= 8;
            output |= input & 0xFF;
            output <<= 8;
            input >>= 8;
            output |= input & 0xFF;
            return output;
        }

        private async Task<float[]> LoadFloatArray(int baseAddress, int length)
        {
            float[] result = new float[length];
            for (int index = 0; index < length; index++)
            {
                int cellAddress = baseAddress + (index * 4);
                int fourBytes = await this.logger.Query4Bytes(cellAddress);
                float value = (float)EndianSwap(fourBytes);
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
