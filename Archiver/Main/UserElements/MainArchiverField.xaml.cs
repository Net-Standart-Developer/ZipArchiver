using System.Windows;
using System.Windows.Controls;
using Archiver;
using GalaSoft.MvvmLight.Messaging;

namespace Archiver.Main.UserElements
{
    /// <summary>
    /// Логика взаимодействия для MainArchiverField.xaml
    /// </summary>
    public partial class MainArchiverField : UserControl
    {
        public MainArchiverField()
        {
            InitializeComponent();
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] path = (string[])e.Data.GetData(DataFormats.FileDrop);
                for(int i = 0; i < path.Length; i++)
                {
                    Messenger.Default.Send<string>(path[i], "Object_DropInMainArchiverField");
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
