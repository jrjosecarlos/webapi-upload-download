using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUploadDownload.Services
{
    interface IFileServerProvider
    {
        Task<bool> UploadFromStreamAsync(String fileName, Stream stream);

        Task<Stream> GetDownloadStreamAsync(String fileName);
    }
}
