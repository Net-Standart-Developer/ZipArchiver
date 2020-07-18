using GalaSoft.MvvmLight.Messaging;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Archiver.Main.UserElements
{
    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void Theme_Changed(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<object>(null, "Theme_Changed");

            string theme = ((RadioButton)e.Source).Content.ToString();

            switch (theme)
            {
                case "тёмно-синяя тема":
                    theme = "DarkBlueTheme";
                    break;
                case "тёмно-оранжевая тема":
                    theme = "DarkOrangeTheme";
                    break;
                default:
                    break;
            }

            Uri uri = new Uri("/Resources/Styles/" + theme + "/Core.xaml", UriKind.Relative);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }
    }
}
