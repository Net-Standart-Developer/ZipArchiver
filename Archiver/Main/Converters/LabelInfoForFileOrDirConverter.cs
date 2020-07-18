using System;
using System.Globalization;
using System.Windows.Data;
using FileExplorer.Elements;

namespace Archiver.Main.Converters
{
    class LabelInfoForFileOrDirConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "Полное имя " + (((FileSystemElement)value).IsFile ? "файла" : "каталога");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
