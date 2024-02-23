using System;
using System.IO;
using System.Threading.Tasks;

namespace LLMS.Service
{
    internal class ImageService : IImageService
    {
        Task<int?> IImageService.GetImageIdByUrlAsync(string imageUrl)
        {
            throw new NotImplementedException();
        }

        Task<string> IImageService.GetImageUrlByIdAsync(int imageId)
        {
            throw new NotImplementedException();
        }

        Task<string> IImageService.UploadImageAsync(Stream imageStream, string imageName)
        {
            throw new NotImplementedException();
        }
    }
}
