using Prism.Mvvm;
using FileExplorer.Elements;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace Archiver.Main.ViewModels
{
    public class FileSystemElementViewModel : BindableBase
    {
        public FileSystemElement Element { get; }

        public string Name => " " + Element.Name;
        public string Icon => Element.IsFile ? "pack://application:,,,/Resources/Icons/file.png" : "pack://application:,,,/Resources/Icons/directory.png";

        public string Size 
        {
            get
            {
                long size = 0;
                size = Element.Size;
                    
                if (size < 1024)
                    return size + " байт";
                if (size < Math.Pow(2, 20))
                    return Math.Round(size / 1024F, 1) + " Кбайт";
                if (size < Math.Pow(2, 30))
                    return Math.Round(size / 1024F / 1024F, 1) + " Мбайт";
                else return Math.Round(size / 1024F / 1024F / 1024F, 1) + " Гбайт";
            }
        }
        private async Task<long> GetSize()
        {
            return await Task.Run(() => Element.Size);
        }

        public ObservableCollection<FileSystemElementViewModel> SubFileSystemElements { get; private set; }
        

        public FileSystemElementViewModel(FileSystemElement fileSystemElement)
        {
            Element = fileSystemElement;
            if (fileSystemElement.Childrens != null)
                InitializeChildren();
        }
        public void InitializeChildren()
        {
            SubFileSystemElements = new ObservableCollection<FileSystemElementViewModel>
                    (Element.Childrens.Select(el => new FileSystemElementViewModel(el)));

            
        }
    }
}
