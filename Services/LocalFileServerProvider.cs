using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApiUploadDownload.Models;

namespace WebApiUploadDownload.Services
{
    public class LocalFileServerProvider : IFileServerProvider
    {
        private readonly string _baseDir;
        private readonly IHostingEnvironment _env;
        private readonly IOptions<UploadConfig> _config;
        private string FullBaseDir => Path.Combine(_env.WebRootPath, _baseDir);

        public LocalFileServerProvider(IHostingEnvironment env, IOptions<UploadConfig> config)
        {
            _env = env;
            _config = config;
            _baseDir = _config.Value.BaseDir ?? "uploaded";
        }

        public Task<Stream> GetDownloadStreamAsync(string nomeArquivo)
        {
            var caminhoArquivo = Path.Combine(FullBaseDir, nomeArquivo);

            var fileInfo = new FileInfo(caminhoArquivo);

            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException($"Arquivo não encontrado: {nomeArquivo}");
            }

            return Task.FromResult((Stream) fileInfo.OpenRead());
        }

        public Task UploadFromStreamAsync(string nomeArquivo, Stream stream)
        {
            Directory.CreateDirectory(FullBaseDir);
            using (var fileStream = new FileStream(Path.Combine(FullBaseDir, nomeArquivo), FileMode.Create, FileAccess.Write))
            {
                return stream.CopyToAsync(fileStream);
            }
        }
    }
}
