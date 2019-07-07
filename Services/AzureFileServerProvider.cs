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
        private readonly string _connectionString = "";

        private readonly CloudFileShare _fileShare;

        private readonly string _baseDir;

        private readonly string _fileShareName;

        private readonly IOptions<UploadConfig> _config;

        public AzureFileServerProvider(IOptions<UploadConfig> config)
        {
            _config = config;

            _baseDir = _config.Value.BaseDir ?? "uploaded";
            _fileShareName = _config.Value.FileShareName;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_connectionString);

            // Create a CloudFileClient object for credentialed access to Azure Files.
            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

            // Get a reference to the file share we created previously.
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

        public async Task UploadFromStreamAsync(string fileName, Stream stream)
        {
            await CheckFileShare();

            // Get a reference to the file we created previously.
            CloudFile file = (await GetBaseDir()).GetFileReference(fileName);

            await file.UploadFromStreamAsync(stream);
        }

        public async Task<Stream> GetDownloadStreamAsync(string fileName)
        {
            await CheckFileShare();

            CloudFile file = (await GetBaseDir()).GetFileReference(fileName);

            if (!(await file.ExistsAsync()))
            {
                throw new ArgumentException($"Arquivo {fileName} não encontrado");
            }

            return await file.OpenReadAsync();
        }
    }
}
