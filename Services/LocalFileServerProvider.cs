using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUploadDownload.Services
{
    public class LocalFileServerProvider : IFileServerProvider
    {
        private readonly string _baseDir;

        public LocalFileServerProvider(string baseDir)
        {
            _baseDir = baseDir;
        }

        public Task<Stream> GetDownloadStreamAsync(string fileName)
        {
            var caminhoArquivo = Path.Combine(_baseDir, fileName);

            var fileInfo = new FileInfo(caminhoArquivo);

            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException($"Arquivo não encontrado: {fileName}");
            }

            return Task.FromResult((Stream) fileInfo.OpenRead());
        }

        public Task UploadFromStreamAsync(string fileName, Stream stream)
        {
            Directory.CreateDirectory(_baseDir);
            using (var fileStream = new FileStream(Path.Combine(_baseDir, fileName), FileMode.Create, FileAccess.Write))
            {
                return stream.CopyToAsync(fileStream);
            }
        }
    }
}
