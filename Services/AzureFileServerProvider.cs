using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.File;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApiUploadDownload.Models;

namespace WebApiUploadDownload.Services
{
    public class AzureFileServerProvider : IFileServerProvider
    {
        private readonly CloudFileShare _fileShare;
        private readonly string _baseDir;
        private readonly string _fileShareName;

        private readonly IOptions<UploadConfig> _config;

        public AzureFileServerProvider(IOptions<UploadConfig> config)
        {
            _config = config;

            _baseDir = _config.Value.BaseDir ?? "uploaded";
            _fileShareName = _config.Value.FileShareName;

            string connectionString = Environment.GetEnvironmentVariable("WAUD_File_Server_API_Key");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

            _fileShare = fileClient.GetShareReference(_fileShareName);
        }

        private async Task CheckFileShare()
        {
            if (!(await _fileShare.ExistsAsync()))
            {
                throw new ArgumentException($"FileShare {_fileShareName} não existe.");
            }

        }

        private async Task<CloudFileDirectory> GetBaseDir()
        {
            CloudFileDirectory baseDir = _fileShare.GetRootDirectoryReference().GetDirectoryReference(_baseDir);
            await baseDir.CreateIfNotExistsAsync();

            return baseDir;
        }

        public async Task UploadFromStreamAsync(string nomeArquivo, Stream stream)
        {
            await CheckFileShare();

            CloudFile file = (await GetBaseDir()).GetFileReference(nomeArquivo);

            await file.UploadFromStreamAsync(stream);
        }

        public async Task<Stream> GetDownloadStreamAsync(string nomeArquivo)
        {
            await CheckFileShare();

            CloudFile file = (await GetBaseDir()).GetFileReference(nomeArquivo);

            if (!(await file.ExistsAsync()))
            {
                throw new ArgumentException($"Arquivo {nomeArquivo} não encontrado");
            }

            return await file.OpenReadAsync();
        }
    }
}
