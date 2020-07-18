using System.IO;
using System;

namespace Archiver.Model.ArchivingMethod
{
    class SelectorOfArchivingMethod
    {
        public Archiver ArchiverByDefault { get; } = new ZipArchiver();

        public Archiver SelectArchiverUsingPath(string path)
        {
            return SelectArchiver(Path.GetExtension(path));
        }
        public Archiver SelectArchiverUsingOption(string option)
        {
            return SelectArchiver(option);
        }

        private Archiver SelectArchiver(string extension)
        {
            switch (extension)
            {
                case ".zip":
                    return new ZipArchiver();
                default:
                    return null;
            }
        }
    }
}
