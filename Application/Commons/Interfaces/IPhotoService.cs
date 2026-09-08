using Microsoft.AspNetCore.Http;

namespace Application.Commons.Interfaces
{
    public interface IPhotoService
    {
        Task<string> UploadPhotoAsync(IFormFile file, string folderName = "products");
        Task<bool> DeletePhotoAsync(string publicId);
    }
}
