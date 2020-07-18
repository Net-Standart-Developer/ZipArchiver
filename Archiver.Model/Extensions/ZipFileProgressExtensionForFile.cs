using System;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Archiver.Model.Extensions
{
    public static class ZipFileProgressExtensionsFroFile
    {

        public static ZipArchiveEntry CreateMyEntryFromFile(this ZipArchive destination, string sourceFileName, string entryName) =>
            DoCreateEntryFromFile(destination, sourceFileName, entryName, null);


        public static ZipArchiveEntry CreateMyEntryFromFile(this ZipArchive destination,
                                                          string sourceFileName, string entryName, CompressionLevel compressionLevel) =>
            DoCreateEntryFromFile(destination, sourceFileName, entryName, compressionLevel);

        internal static ZipArchiveEntry DoCreateEntryFromFile(this ZipArchive destination,
                                                              string sourceFileName, string entryName, CompressionLevel? compressionLevel)
        {
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));

            if (sourceFileName == null)
                throw new ArgumentNullException(nameof(sourceFileName));

            if (entryName == null)
                throw new ArgumentNullException(nameof(entryName));

            using (Stream fs = new FileStream(sourceFileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 0x1000, useAsync: false))
            {
                ZipArchiveEntry entry = compressionLevel.HasValue
                                    ? destination.CreateEntry(entryName, compressionLevel.Value)
                                    : destination.CreateEntry(entryName);

                DateTime lastWrite = File.GetLastWriteTime(sourceFileName);

                if (lastWrite.Year < 1980 || lastWrite.Year > 2107)
                    lastWrite = new DateTime(1980, 1, 1, 0, 0, 0);

                entry.LastWriteTime = lastWrite;

                using (Stream es = entry.Open())
                {
                    fs.CopyTo(es);
                }
                    

                return entry;
            }
            
        }
    }
}
