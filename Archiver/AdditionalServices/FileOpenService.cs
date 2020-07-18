using Microsoft.Win32;


namespace Archiver.AdditionalServices
{
    static class FileOpenService
    {
        public static string Path
        {
            get
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "All files|*.*";
                if (openFileDialog.ShowDialog() == true)
                    return openFileDialog.FileName;
                return null;
            }
        }
    }
}
