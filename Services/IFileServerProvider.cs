using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUploadDownload.Services
{
    public interface IFileServerProvider
    {
        Task UploadFromStreamAsync(string fileName, Stream stream);

        Task<Stream> GetDownloadStreamAsync(string fileName);
    }
}
