using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NateW.Ssm
{
    public class EcuImage
    {
        private List<EcuImageRange> ranges;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        protected List<EcuImageRange> Ranges { get { return this.ranges; } }
        byte[] binaryFileData;

        public EcuImage(string fileName)
        {
            this.ranges = new List<EcuImageRange>();
            this.binaryFileData = new byte[1024 * 1024];
            try
            {
                int bytesRead = 0;
               
                using (Stream file = File.OpenRead(fileName))
                {
                    while (bytesRead < this.binaryFileData.Length)
                    {
                        bytesRead += file.Read(
                            this.binaryFileData,
                            bytesRead,
                            this.binaryFileData.Length);
                    }
                }
            }
            catch(Exception)
            {
                // just let the binary file array stay as-is (initialized to zeros).
            }
        }

        public byte GetValue(int address)
        {
            foreach (EcuImageRange range in this.ranges)
            {
                byte result;
                if (range.TryGetValue(address, out result))
                {
                    return result;
                }
            }

            return this.binaryFileData[address];
        }
    }
}
