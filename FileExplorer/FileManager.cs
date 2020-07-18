using System.Collections.ObjectModel;
using System.IO;
using FileExplorer.Elements;
using FileExplorer.Operations;

namespace FileExplorer
{
    public class FileManager
    {
        private FileSystemElementsSelector elementsSelector = new FileSystemElementsSelector();

        public ReadOnlyObservableCollection<FileSystemElement> Elements { get; }
        private readonly ObservableCollection<FileSystemElement> elements;
        

        public FileManager(string path)
        {
            elements = elementsSelector.GetFileSystemElements(path);
            Elements = new ReadOnlyObservableCollection<FileSystemElement>(elements);
        }

        public bool AddSubElements(FileSystemElement fileSystemElement)
        {
            if(!fileSystemElement.IsFile)
            {
                elementsSelector.InitializeSubElements(fileSystemElement);
                return true;
            }
            return false;
        }
    }
}
