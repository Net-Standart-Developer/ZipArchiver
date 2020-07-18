using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.Messaging;

namespace Archiver.Main.UserElements
{
    /// <summary>
    /// Логика взаимодействия для FileExplorer.xaml
    /// </summary>
    public partial class FileExplorer : UserControl
    {
        public FileExplorer()
        {
            InitializeComponent();
        }

        private void fileSystemElementsTreeView_Expanded(object sender, RoutedEventArgs e)
        {
             TreeViewItem item = (TreeViewItem)e.OriginalSource;
             Messenger.Default.Send<TreeViewItem>(item, "TreeViewItem_Expanded");
        }

        private void fileSystemElementsTreeView_Collapsed(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)e.OriginalSource;
            Messenger.Default.Send<TreeViewItem>(item, "TreeViewItem_Collapsed");
        }

        private void fileSystemElementsTreeView_Selected(object sender, RoutedEventArgs e)
        {
            //Messenger.Default.Send<string>(path[i], "Object_Drop");
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] path = (string[])e.Data.GetData(DataFormats.FileDrop);
                for (int i = 0; i < path.Length; i++)
                {
                    Messenger.Default.Send<string>(path[i], "Object_DropInFileExplorer");
                }
            }
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }
    }
}
