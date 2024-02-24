using System;
using System.IO;
using System.Threading.Tasks;

namespace LLMS.Service
{
    internal class ImageService : IImageService
    {
        public Task<int?> GetImageIdByUrlAsync(string imageUrl)
        {
            // 返回模拟的 image ID 或 null
            return Task.FromResult<int?>(null); // 假设找不到对应的 image ID
        }

        public Task<string> GetImageUrlByIdAsync(int imageId)
        {
            // 返回模拟的 image URL 或 null
            return Task.FromResult<string>(null); // 假设找不到对应的 image URL
        }

        public Task<string> UploadImageAsync(Stream imageStream, string imageName)
        {
            // 返回模拟的 image URL
            return Task.FromResult("https://example.com/mock-image-url"); // 返回一个模拟的图片 URL
        }
    }
}
