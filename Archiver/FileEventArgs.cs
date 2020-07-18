
namespace Archiver
{
    class FileEventArgs
    {
        internal string Path { get; }
        public FileEventArgs(string path) => Path = path;
    }
}
