using System;
using System.IO;

namespace FileExplorer.Elements
{
    public class FileElement : FileSystemElement
    {
        public override bool IsFile => true;

        public override long Size => ((FileInfo)Info).Length;

        public FileElement(FileInfo fileInfo) : base(fileInfo) { }
    }
}
