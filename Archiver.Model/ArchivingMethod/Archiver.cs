using System;
using System.IO;
using System.IO.Compression;

namespace Archiver.Model.ArchivingMethod
{
    abstract class Archiver
    {
        public abstract void CompressFile(string path, IProgress<ProgressInfo> onProgressArchivationChanged);
        public abstract void CompressDirectory(string path, IProgress<ProgressInfo> onProgressArchivationChanged);

        public abstract void Decompress(string path, IProgress<ProgressInfo> onProgressArchivationChanged);
        public abstract void DecompressToDir(string path, IProgress<ProgressInfo> onProgressArchivationChanged);
    }
}
