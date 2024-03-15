
using System;
using System.IO;

namespace Kat_UNIT21
{
    public class CsvFileLocator : ICsvFileLocator
    {
        public string LocateCsvFile()
        {
            var binDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(binDirectory, "mps.csv");
        }
    }
}
