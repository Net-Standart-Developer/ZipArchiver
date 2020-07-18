using System.IO;
using Archiver.Model.ArchivingMethod;
using System.Threading.Tasks;
using System;

namespace Archiver.Model
{
    public class ArchiveManager
    {
        private ArchivingMethod.Archiver archiver;
        private SelectorOfArchivingMethod selector = new SelectorOfArchivingMethod();
        
        public ArchiveManager() : this(null) { }
        public ArchiveManager(string wayOfArchivation)
        {
            if (wayOfArchivation != null)
                archiver = selector.SelectArchiverUsingOption(wayOfArchivation);
            else
                archiver = selector.ArchiverByDefault;
        }

        public async Task HandleObject(string path, IProgress<ProgressInfo> onProgressArchivationChanged)
        {
            await Task.Run(() =>
            {
                if (Path.HasExtension(path))
                {
                    var archiverForDecompression = selector.SelectArchiverUsingPath(path); //если файл для распаковки, архиватор не будет пустым
                    if (archiverForDecompression != null)
                        archiverForDecompression.DecompressToDir(path, onProgressArchivationChanged);
                    else
                        archiver.CompressFile(path, onProgressArchivationChanged);
                }
                else
                    archiver.CompressDirectory(path, onProgressArchivationChanged);
            });
        }
    }
}
