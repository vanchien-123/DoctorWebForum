using Microsoft.AspNetCore.Mvc;

namespace DoctorWebForum.Interfaces
{
    public interface IFileService
    {
        Task<IActionResult> SaveImage(IFormFile imageFile);
        public bool DeleteImage(string imageFileName);
    }
}
