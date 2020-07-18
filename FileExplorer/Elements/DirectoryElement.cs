using System.IO;
using System.Threading.Tasks;

namespace FileExplorer.Elements
{
    public class DirectoryElement : FileSystemElement
    {
        public override bool IsFile => false;

        public override long Size
        {
            get
            {
                return ((DirectoryInfo)Info).GetSize();
            }
        }

        public DirectoryElement(DirectoryInfo directoryInfo) : base(directoryInfo)
        {
            Childrens = new System.Collections.ObjectModel.ObservableCollection<FileSystemElement>();
        }

        public override void AddElement(FileSystemElement fileSystemElement)
        {
            if(fileSystemElement != null)
                Childrens.Add(fileSystemElement);
        }
        public override void RemoveElement(FileSystemElement fileSystemElement)
        {
            if(fileSystemElement != null && Childrens.Contains(fileSystemElement))
                Childrens.Remove(fileSystemElement);
        }
    }
}
