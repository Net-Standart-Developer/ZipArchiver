using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver.Model.ArchivingMethod
{
    //class GZipArchiver : Archiver
    //{
    //    public void Compress(string path, string compressedFile)
    //    {
    //        using (FileStream sourceStream = new FileStream(path, FileMode.OpenOrCreate))
    //        {
    //            using (FileStream targetStream = File.Create(compressedFile))
    //            {
    //                using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
    //                {
    //                    sourceStream.CopyTo(compressionStream);
    //                }
    //            }
    //        }
    //    }
    //    public void Decompress(string path, string unpackedFile)
    //    {
    //        using (FileStream sourceStream = new FileStream(path, FileMode.OpenOrCreate))
    //        {
    //            using (FileStream targetStream = File.Create(unpackedFile))
    //            {
    //                using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
    //                {
    //                    decompressionStream.CopyTo(targetStream);
    //                }
    //            }
    //        }
    //    }
    //}
}
