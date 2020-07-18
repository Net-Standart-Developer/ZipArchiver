using System.Windows.Forms;


namespace Archiver.AdditionalServices
{
    static class DirectoryOpenService
    {
        public static string Path
        {
            get
            {
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    folderBrowserDialog.Description = "Выберите папку";
                    folderBrowserDialog.ShowNewFolderButton = false;
                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        return folderBrowserDialog.SelectedPath;
                    }
                }
                return null;
            }
        }
    }
}
