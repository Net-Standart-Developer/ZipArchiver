using System.Collections.ObjectModel;
using System.IO;
using Prism.Mvvm;

namespace FileExplorer.Elements
{
    public abstract class FileSystemElement : BindableBase
    {
        public FileSystemInfo Info { get; private set; }
        public virtual string Name => Info.Name;
        public abstract bool IsFile { get; }

        public abstract long Size { get; }

        public ObservableCollection<FileSystemElement> Childrens { get; protected set; }

        public FileSystemElement(FileSystemInfo info)
        {
            Info = info;
        }

        public virtual void AddElement(FileSystemElement fileSystemElement) { }
        public virtual void RemoveElement(FileSystemElement fileSystemElement) { }
    }
}
