using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
//using Microsoft.WindowsAzure.Storage;
//using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Helpers
{
    public class BlobHelper : IBlobHelper
    {
        private readonly BlobServiceClient _blobClient;

        public BlobHelper(IConfiguration configuration)
        {
            string keys = configuration["Blob:ConnectionString"];

         

            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(keys);
            //_blobClient = storageAccount.CreateCloudBlobClient();        
        }

        public async Task<Guid> UploadBlobAsync(byte[] file, string containerName, string keys)
        {
            MemoryStream stream = new MemoryStream(file);
            Guid name = Guid.NewGuid();

            var blobHttpHeader = new BlobHttpHeaders();
            //string extension = Path.GetExtension(file);
            //switch (extension.ToLower())
            //{
            //    case ".jpg":
            //    case ".jpeg":
            //        blobHttpHeader.ContentType = "image/jpeg";
            //        break;
            //    case ".png":
            //        blobHttpHeader.ContentType = "image/png";
            //        break;
            //    case ".gif":
            //        blobHttpHeader.ContentType = "image/gif";
            //        break;
            //    default:
            //        break;
            //}

            BlobServiceClient blobServiceClient = new BlobServiceClient(keys);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient($"{name}");
            await blobClient.UploadAsync(stream, blobHttpHeader);

            //BlobContainerClient container = _blobClient.GetBlobContainerClient(containerName);
            //BlockBlobClient blockBlob = container.GetBlockBlobClient($"{name}");
            //await blockBlob.UploadAsync(stream);

            return name;
        }

        public async Task<Guid> UploadBlobAsync(IFormFile file, string containerName, string keys)
        {
            Stream stream = file.OpenReadStream();
            Guid name = Guid.NewGuid();

            var blobHttpHeader = new BlobHttpHeaders();
            string extension = Path.GetExtension(file.FileName);
            switch (extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    blobHttpHeader.ContentType = "image/jpeg";
                    break;
                case ".png":
                    blobHttpHeader.ContentType = "image/png";
                    break;
                case ".gif":
                    blobHttpHeader.ContentType = "image/gif";
                    break;
                default:
                    break;
            }

            BlobServiceClient blobServiceClient = new BlobServiceClient(keys);
            BlobContainerClient containerClient =  blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient($"{name}");
            await blobClient.UploadAsync(stream, blobHttpHeader);

            return name;
        }

        public async Task<Guid> UploadBlobAsync(string image, string containerName)
        {
            Stream stream = File.OpenRead(image);
            Guid name = Guid.NewGuid();
            BlobContainerClient container = _blobClient.GetBlobContainerClient(containerName);
            BlockBlobClient blockBlob = container.GetBlockBlobClient($"{name}");
            await blockBlob.UploadAsync(stream);
            return name;
        }

        public async Task DeleteBlobAsync(Guid id, string containerName)
        {
            BlobContainerClient container = _blobClient.GetBlobContainerClient(containerName);
            BlockBlobClient blockBlob = container.GetBlockBlobClient($"{id}");
            await blockBlob.DeleteAsync();
        }
    }
}
