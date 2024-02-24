using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LLMS.Service
{
    internal class ImageService : IImageService
    {
        private readonly CloudBlobContainer _container;

        public ImageService(string connectionString, string containerName)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(containerName);
            _container.CreateIfNotExistsAsync().Wait();
        }

        public Task<int?> GetImageIdByUrlAsync(string imageUrl)
        {
            // 实现逻辑，根据 imageUrl 获取 image ID
            // 这里返回 null 作为示例
            return Task.FromResult<int?>(null);
        }

        public Task<string> GetImageUrlByIdAsync(int imageId)
        {
            // 实现逻辑，根据 imageId 获取 imageUrl
            // 这里返回 null 作为示例
            return Task.FromResult<string>(null);
        }

        public async Task<string> UploadImageAsync(Stream imageStream, string imageName)
        {
            try
            {
                var blockBlob = _container.GetBlockBlobReference(imageName);
                await blockBlob.UploadFromStreamAsync(imageStream);
                return blockBlob.Uri.ToString();
            }
            catch (Exception ex)
            {
                // 记录异常、处理异常
                throw new ApplicationException($"An error occurred while uploading the image: {ex.Message}");
            }
        }
    }
}
