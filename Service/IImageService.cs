using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLMS.Service
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(Stream imageStream, string imageName);
        Task<int> GetImageIdByUrlAsync(string imageUrl);
        Task<string> GetImageUrlByIdAsync(int imageId);

        Task<int> CreateImageRecordAsync(string imageUrl);
    }

}
