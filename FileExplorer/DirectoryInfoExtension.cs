using System.IO;
using System.Threading.Tasks;

namespace FileExplorer
{
    static class DirectoryInfoExtension
    {
        public static long GetSize(this DirectoryInfo directory)
        {
            long size = 0;
            foreach (var file in directory.GetFiles())
            {
                size += file.Length;
            }
            foreach (var subDir in directory.GetDirectories())
            {
                size += subDir.GetSize();
            }
            return size;
        }
    }
}
