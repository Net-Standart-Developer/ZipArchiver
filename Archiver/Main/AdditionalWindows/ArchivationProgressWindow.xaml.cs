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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Archiver.Main.AdditionalWindows
{
    /// <summary>
    /// Логика взаимодействия для ArchivationProgressWindow.xaml
    /// </summary>
    public partial class ArchivationProgressWindow : Window
    {
        public ArchivationProgressWindow()
        {
            InitializeComponent();
        }
        public void ChangeProgress(ProgressInfo info)
        {
            if (info.MaxValue == info.Value || info.IsHaveCancel)
                Close();

            if (info.IsArchivation)
                textForDescription.Text = "Выполняется архивация";
            else
                textForDescription.Text = "Выполняется разархивация";

            if (info.CurrentFile != null)
                textForCurrentFile.Text = "Обрабатываемый файл/папка " + info.CurrentFile;

            if (info.MaxValue != -1)
                progressBar.Maximum = info.MaxValue;

            if (info.Value != -1)
                progressBar.Value = info.Value;

            
        }
    }
}
