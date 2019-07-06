using Microsoft.Azure;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.File;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUploadDownload.Services
{
    public class AzureFileServerProvider
    {
        private readonly string _connectionString = "";

        private readonly CloudFileShare _fileShare;

        private readonly string _baseDir = "uploaded";

        private readonly string _fileShareName = "flshr201906301117";

        public AzureFileServerProvider()
        {
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

        public async Task<bool> UploadFromStreamAsync(String fileName, Stream stream)
        {
            await CheckFileShare();

            // Get a reference to the file we created previously.
            CloudFile file = (await GetBaseDir()).GetFileReference(fileName);

            await file.UploadFromStreamAsync(stream);

            return true;
        }

        public async Task<Stream> GetDownloadStreamAsync(String fileName)
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
