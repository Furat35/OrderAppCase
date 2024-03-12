using Microsoft.AspNetCore.Http;

namespace Ordering.Application.Helpers
{
    public interface IFileService
    {
        Task<string> UploadFile(string folderNameToUpload, IFormFile file);
        void RemoveFile(string folderName, string fileName);
        byte[] GetImage(string folderName, string fileName);
    }
}
