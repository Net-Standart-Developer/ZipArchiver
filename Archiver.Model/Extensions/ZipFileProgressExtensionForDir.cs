using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver.Model.Extensions
{
    class ZipFileProgressExtensionForDir
    {
        
        public static ZipArchive OpenRead(string archiveFileName) => Open(archiveFileName, ZipArchiveMode.Read);

  
        public static ZipArchive Open(string archiveFileName, ZipArchiveMode mode) => Open(archiveFileName, mode, entryNameEncoding: null);


        public static ZipArchive Open(string archiveFileName, ZipArchiveMode mode, Encoding entryNameEncoding = null)
        {


            FileMode fileMode;
            FileAccess access;
            FileShare fileShare;

            switch (mode)
            {
                case ZipArchiveMode.Read:
                    fileMode = FileMode.Open;
                    access = FileAccess.Read;
                    fileShare = FileShare.Read;
                    break;

                case ZipArchiveMode.Create:
                    fileMode = FileMode.CreateNew;
                    access = FileAccess.Write;
                    fileShare = FileShare.None;
                    break;

                case ZipArchiveMode.Update:
                    fileMode = FileMode.OpenOrCreate;
                    access = FileAccess.ReadWrite;
                    fileShare = FileShare.None;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode));
            }


            FileStream fs = new FileStream(archiveFileName, fileMode, access, fileShare, bufferSize: 0x1000, useAsync: false);

            try
            {
                return new ZipArchive(fs, mode, leaveOpen: false, entryNameEncoding: entryNameEncoding);
            }
            catch
            {
                fs.Dispose();
                throw;
            }
        }

        public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, IProgress<ProgressInfo> archivationProgress) =>
            DoCreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, archivationProgress, compressionLevel: null, includeBaseDirectory: false, entryNameEncoding: null);

        public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, IProgress<ProgressInfo> archivationProgress, CompressionLevel compressionLevel, bool includeBaseDirectory) =>
            DoCreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, archivationProgress, compressionLevel, includeBaseDirectory, entryNameEncoding: null);


        public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, IProgress<ProgressInfo> archivationProgress,
                                               CompressionLevel compressionLevel, bool includeBaseDirectory, Encoding entryNameEncoding = null) =>
            DoCreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, archivationProgress, compressionLevel, includeBaseDirectory, entryNameEncoding);

        private static void DoCreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, IProgress<ProgressInfo> archivationProgress,
                                                  CompressionLevel? compressionLevel, bool includeBaseDirectory, Encoding entryNameEncoding = null)

        {

            sourceDirectoryName = Path.GetFullPath(sourceDirectoryName);
            destinationArchiveFileName = Path.GetFullPath(destinationArchiveFileName);

            using (ZipArchive archive = Open(destinationArchiveFileName, ZipArchiveMode.Create, entryNameEncoding))
            {
                bool directoryIsEmpty = true;

                //add files and directories
                DirectoryInfo di = new DirectoryInfo(sourceDirectoryName);

                string basePath = di.FullName;

                if (includeBaseDirectory && di.Parent != null)
                    basePath = di.Parent.FullName;

                // Windows' MaxPath (260) is used as an arbitrary default capacity, as it is likely
                // to be greater than the length of typical entry names from the file system, even
                // on non-Windows platforms. The capacity will be increased, if needed.
                const int DefaultCapacity = 260;
                char[] entryNameBuffer = ArrayPool<char>.Shared.Rent(DefaultCapacity);

                try
                {
                    ProgressInfo info = new ProgressInfo(0, di.EnumerateFileSystemInfos("*", SearchOption.AllDirectories).Count(), true);
                    foreach (FileSystemInfo file in di.EnumerateFileSystemInfos("*", SearchOption.AllDirectories))
                    {
                        directoryIsEmpty = false;

                        int entryNameLength = file.FullName.Length - basePath.Length;
                        Debug.Assert(entryNameLength > 0);

                        if (file is FileInfo)
                        {
                            // Create entry for file:
                            string entryName = ZipFileUtils.EntryFromPath(file.FullName, basePath.Length, entryNameLength, ref entryNameBuffer);
                            ZipFileProgressExtensionsFroFile.DoCreateEntryFromFile(archive, file.FullName, entryName, compressionLevel);
                        }
                        else
                        {
                            // Entry marking an empty dir:
                            if (file is DirectoryInfo possiblyEmpty && ZipFileUtils.IsDirEmpty(possiblyEmpty))
                            {
                                // FullName never returns a directory separator character on the end,
                                // but Zip archives require it to specify an explicit directory:
                                string entryName = ZipFileUtils.EntryFromPath(file.FullName, basePath.Length, entryNameLength, ref entryNameBuffer, appendPathSeparator: true);
                                archive.CreateEntry(entryName);
                            }
                        }
                        info.Value++;
                        info.CurrentFile = file.Name;
                        archivationProgress.Report(info);
                    }  // foreach

                    // If no entries create an empty root directory entry:
                    if (includeBaseDirectory && directoryIsEmpty)
                        archive.CreateEntry(ZipFileUtils.EntryFromPath(di.Name, 0, di.Name.Length, ref entryNameBuffer, appendPathSeparator: true));
                }
                finally
                {
                    ArrayPool<char>.Shared.Return(entryNameBuffer);
                }

            }
        }
    }
    internal static partial class ZipFileUtils
    {
        // Per the .ZIP File Format Specification 4.4.17.1 all slashes should be forward slashes
        private const char PathSeparatorChar = '/';
        private const string PathSeparatorString = "/";

        public static string EntryFromPath(string entry, int offset, int length, ref char[] buffer, bool appendPathSeparator = false)
        {
            Debug.Assert(length <= entry.Length - offset);
            Debug.Assert(buffer != null);

            // Remove any leading slashes from the entry name:
            while (length > 0)
            {
                if (entry[offset] != Path.DirectorySeparatorChar &&
                    entry[offset] != Path.AltDirectorySeparatorChar)
                    break;

                offset++;
                length--;
            }

            if (length == 0)
                return appendPathSeparator ? PathSeparatorString : string.Empty;

            int resultLength = appendPathSeparator ? length + 1 : length;
            EnsureCapacity(ref buffer, resultLength);
            entry.CopyTo(offset, buffer, 0, length);

            // '/' is a more broadly recognized directory separator on all platforms (eg: mac, linux)
            // We don't use Path.DirectorySeparatorChar or AltDirectorySeparatorChar because this is
            // explicitly trying to standardize to '/'
            for (int i = 0; i < length; i++)
            {
                char ch = buffer[i];
                if (ch == Path.DirectorySeparatorChar || ch == Path.AltDirectorySeparatorChar)
                    buffer[i] = PathSeparatorChar;
            }

            if (appendPathSeparator)
                buffer[length] = PathSeparatorChar;

            return new string(buffer, 0, resultLength);
        }

        public static void EnsureCapacity(ref char[] buffer, int min)
        {
            Debug.Assert(buffer != null);
            Debug.Assert(min > 0);

            if (buffer.Length < min)
            {
                int newCapacity = buffer.Length * 2;
                if (newCapacity < min)
                    newCapacity = min;

                char[] oldBuffer = buffer;
                buffer = ArrayPool<char>.Shared.Rent(newCapacity);
                ArrayPool<char>.Shared.Return(oldBuffer);
            }
        }

        public static bool IsDirEmpty(DirectoryInfo possiblyEmptyDir)
        {
            using (IEnumerator<string> enumerator = Directory.EnumerateFileSystemEntries(possiblyEmptyDir.FullName).GetEnumerator())
                return !enumerator.MoveNext();
        }
    }
}
