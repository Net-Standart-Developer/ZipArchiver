using System.Collections.ObjectModel;
using FileExplorer.Elements;
using System.IO;

namespace FileExplorer.Operations
{
    class FileSystemElementsSelector
    {
        public ObservableCollection<FileSystemElement> GetFileSystemElements(string path)
        {
            ObservableCollection<FileSystemElement> elements;
            if (Path.HasExtension(path))
            {
                elements = new ObservableCollection<FileSystemElement>() { new FileElement(new FileInfo(path)) };
            }
            else
            {
                DirectoryInfo directory = new DirectoryInfo(path);
                elements = new ObservableCollection<FileSystemElement>() { new DirectoryElement(directory) };
                addSubElements(elements[0]);
            }
            return elements;
        }
        public void InitializeSubElements(FileSystemElement fileSystemElement)
        {
            addSubElements(fileSystemElement);
        }
        private void addSubElements(FileSystemElement fileSystemElement)
        {
            DirectoryInfo directory = (DirectoryInfo)fileSystemElement.Info;

            foreach (var subDirectory in directory.GetDirectories())
            {
                fileSystemElement.AddElement(new DirectoryElement(subDirectory));
            }
            foreach (var file in directory.GetFiles())
            {
                fileSystemElement.AddElement(new FileElement(file));
            }
        }
    }
}
