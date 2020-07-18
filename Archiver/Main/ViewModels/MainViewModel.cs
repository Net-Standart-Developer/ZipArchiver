using Prism.Commands;
using Prism.Mvvm;
using Archiver.Model;
using FileExplorer;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Collections.Specialized;
using FileExplorer.Elements;
using Archiver.AdditionalServices;
using System.Threading.Tasks;
using System.Diagnostics;
using Archiver.Main.AdditionalWindows;
using System.Windows;

namespace Archiver.Main.ViewModels
{
    class MainViewModel : BindableBase
    {
        private ArchiveManager archiveManager = new ArchiveManager();
        private FileManager fileManager;

        private FileSystemElementViewModel selectedItem;
        public FileSystemElementViewModel SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<FileSystemElementViewModel> FileSystemElements { get; private set; }

        public MainViewModel()
        {
            Messenger.Default.Register<string>(this, "Object_DropInMainArchiverField", (path) =>
            {
                if (System.IO.Path.GetExtension(path) == ".rar" || System.IO.Path.GetExtension(path) == ".7z")
                {
                    System.Windows.MessageBox.Show("Поддерживаемый формат разархивации - zip");
                    return;
                }
                ArchivationProgressWindow archivationProgressWindow = new ArchivationProgressWindow();
                archivationProgressWindow.Show();

                archiveManager.HandleObject(path, new Progress<ProgressInfo>(info => archivationProgressWindow.ChangeProgress(info)));
                
            });

            Messenger.Default.Register<string>(this, "Object_DropInFileExplorer", (path) =>
            {
                AddNewFilesOrDirectories(path);
            });

            Messenger.Default.Register<System.Windows.Controls.TreeViewItem>(this, "TreeViewItem_Expanded", (item) =>
            {
                FileSystemElementViewModel element = (FileSystemElementViewModel)item.Header;

                if (!element.Element.IsFile)
                {
                    foreach (var subElement in element.SubFileSystemElements)
                        if (fileManager.AddSubElements(subElement.Element))
                            subElement.InitializeChildren();
                }
                else
                    Process.Start(element.Element.Info.FullName);
            });

            Messenger.Default.Register<object>(this, "Theme_Changed", (obj) =>
            {
                ClearField.Execute(obj);
            });
        }
        private static void Watch<T, T2>(ReadOnlyObservableCollection<T> collToWatch, ObservableCollection<T2> collToUpdate,
            Func<T2, object> modelProperty)
        {
            ((INotifyCollectionChanged)collToWatch).CollectionChanged += (s, a) =>
            {
                if (a.NewItems?.Count == 1) collToUpdate.Add((T2)Activator.CreateInstance(typeof(T2), (T)a.NewItems[0], null));
                if (a.OldItems?.Count == 1) collToUpdate.Remove(collToUpdate.First(mv => modelProperty(mv) == a.OldItems[0]));
            };
        }

        private void AddNewFilesOrDirectories(string path) //здесь есть ошибка
        {
            try
            {
                if (path != null)
                {
                    ClearField.Execute(new Object());

                    fileManager = new FileManager(path);
                    FileSystemElements = new ObservableCollection<FileSystemElementViewModel>
                    (fileManager.Elements.Select(el => new FileSystemElementViewModel(el)));

                    Watch<FileSystemElement, FileSystemElementViewModel>(fileManager.Elements, FileSystemElements, el => el.Element);

                    RaisePropertyChanged(nameof(FileSystemElements));
                }
            }
            catch(Exception ex)
            {
                //MessageBox.Show("Неверный выбор елемента по пути " + path);
            }
        }


        private RelayCommand archivate;
        public RelayCommand Archivate
        {
            get
            {
                return archivate ??
                    (archivate = new RelayCommand((obj) =>
                    {
                        if (System.IO.Path.GetExtension(selectedItem.Element.Info.FullName) == ".rar" || System.IO.Path.GetExtension(selectedItem.Element.Info.FullName) == ".7z")
                        {
                            System.Windows.MessageBox.Show("Поддерживаемый формат разархивации - zip");
                            return;
                        }
                        ArchivationProgressWindow archivationProgressWindow = new ArchivationProgressWindow();
                        archivationProgressWindow.Show();

                        archiveManager.HandleObject(selectedItem.Element.Info.FullName, new Progress<ProgressInfo>(info => archivationProgressWindow.ChangeProgress(info)));

                    }, (obj) => selectedItem != null));
            }
        }

        private RelayCommand openFile;
        public RelayCommand OpenFile
        {
            get
            {
                return openFile ??
                    (openFile = new RelayCommand((obj) =>
                    {
                        AddNewFilesOrDirectories(FileOpenService.Path);
                    }));
            }
        }

        private RelayCommand openDirectory;
        public RelayCommand OpenDirectory
        {
            get
            {
                return openDirectory ??
                    (openDirectory = new RelayCommand((obj) =>
                    {
                        AddNewFilesOrDirectories(DirectoryOpenService.Path);
                    }));
            }
        }

        private RelayCommand clearField;
        public RelayCommand ClearField
        {
            get
            {
                return clearField ??
                    (clearField = new RelayCommand((obj) =>
                    {
                        FileSystemElements = null;
                        SelectedItem = null;
                        RaisePropertyChanged(nameof(FileSystemElements));
                        RaisePropertyChanged(nameof(SelectedItem));
                    }, (obj) => FileSystemElements != null));
            }
        }

        private RelayCommand getInfo;
        public RelayCommand GetInfo
        {
            get
            {
                return getInfo ??
                    (getInfo = new RelayCommand((obj) =>
                    {
                        FileInformationWindow fileInformationWindow = new FileInformationWindow(selectedItem);
                        fileInformationWindow.Show();
                    }, (obj) => selectedItem != null));
            }
        }
    }
}
