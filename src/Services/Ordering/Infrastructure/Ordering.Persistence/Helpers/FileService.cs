using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Ordering.Application.Helpers;

namespace Ordering.Persistence.Helpers
{
    public class FileService : IFileService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public FileService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> UploadFile(string folderNameToUpload, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file!");

            var fileExtension = Path.GetExtension(file.FileName);
            if (!fileExtension.Equals(".png", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Invalid file format! Only PNG images are allowed.");

            var uploadsFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "uploads", folderNameToUpload);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return uniqueFileName;
        }

        public void RemoveFile(string folderName, string fileName)
        {
            if (string.IsNullOrEmpty(folderName) || string.IsNullOrEmpty(fileName))
                throw new ArgumentException("Invalid folder name!");

            var currentDirectory = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(currentDirectory, "wwwroot", "uploads", folderName);

            if (File.Exists(filePath))
                File.Delete(filePath);
            else
                throw new FileNotFoundException($"File not found: {filePath}");
        }

        public byte[] GetImage(string folderName, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("Invalid file name");

            var currentDirectory = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(currentDirectory, "wwwroot", "uploads", folderName, fileName);

            if (File.Exists(filePath))
                return File.ReadAllBytes(filePath);
            else
                throw new FileNotFoundException($"File not found: {filePath}");
        }
    }
}
