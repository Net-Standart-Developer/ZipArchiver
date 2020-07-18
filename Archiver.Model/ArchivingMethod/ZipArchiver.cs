using System;
using System.IO;
using System.IO.Compression;
using System.Windows;
using Archiver.Model.Extensions;

namespace Archiver.Model.ArchivingMethod
{
    class ZipArchiver : Archiver
    {
        public override void CompressFile(string path, IProgress<ProgressInfo> onProgressArchivationChanged)
        {
            if (File.Exists(Path.ChangeExtension(path,"zip")))
            {
                MessageBox.Show("Такой файл уже существует");
            }
            else
            {
                using (ZipArchive archive = ZipFile.Open(Path.ChangeExtension(path, ".zip"), ZipArchiveMode.Create))
                {
                    onProgressArchivationChanged.Report(new ProgressInfo(0, 1, true) { CurrentFile = Path.GetFileName(path)});
                    archive.CreateMyEntryFromFile(path, Path.GetFileName(path));
                }
            }
            onProgressArchivationChanged.Report(new ProgressInfo(0, true) { IsHaveCancel = true });
        }
        public override void CompressDirectory(string path, IProgress<ProgressInfo> onProgressArchivationChanged)
        {
            if (File.Exists(path + ".zip"))
            {
                MessageBox.Show("Такой файл уже существует");
            }
            else
            {
                ZipFileProgressExtensionForDir.CreateFromDirectory(path, path + ".zip", onProgressArchivationChanged);
                //ZipFile.CreateFromDirectory(path, path + ".zip");
            }
            onProgressArchivationChanged.Report(new ProgressInfo(0, true) { IsHaveCancel = true });
        }

        public override void Decompress(string path, IProgress<ProgressInfo> onProgressArchivationChanged) //не используется, не обработан случай, если распакованный файл уже есть
        {
            using (FileStream zip = new FileStream(path, FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zip, ZipArchiveMode.Read))
                {
                    string pathToExtract = Path.GetDirectoryName(path);
                    ProgressInfo info = new ProgressInfo(0, archive.Entries.Count, false);
                    foreach (var file in archive.Entries)
                    {
                        string test = "";
                        if (file.FullName.Contains("/"))
                        {
                            test = Path.Combine(pathToExtract, Path.GetDirectoryName(file.FullName));
                            if (!Directory.Exists(Path.Combine(pathToExtract, Path.GetDirectoryName(file.FullName))))
                            {
                                Directory.CreateDirectory(Path.Combine(pathToExtract, Path.GetDirectoryName(file.FullName)));
                            }
                        }
                        if(Path.HasExtension(file.FullName))
                        {
                            test = Path.Combine(pathToExtract, file.FullName); 
                            //
                            file.ExtractToFile(Path.Combine(pathToExtract, file.FullName));
                        }
                        
                        info.Value++;
                        onProgressArchivationChanged.Report(info);
                    }
                }
            }
        }
        public override void DecompressToDir(string path, IProgress<ProgressInfo> onProgressArchivationChanged)
        {
            string directory = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            else
            {
                MessageBox.Show("Папка для распаковки уже существует");
                onProgressArchivationChanged.Report(new ProgressInfo(0, false) { IsHaveCancel = true });
                return;
            }

            Stream zipReadingStream = new FileStream(path, FileMode.Open);
            ZipArchive zip = new ZipArchive(zipReadingStream);
            zip.ExtractToDirectory(directory, onProgressArchivationChanged);

            zipReadingStream.Close();
            //ZipFile.ExtractToDirectory(path, directory);


        }
    }
}
