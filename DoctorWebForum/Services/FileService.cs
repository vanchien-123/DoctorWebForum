using DoctorWebForum.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using System;

namespace DoctorWebForum.Services
{
    public class FileService : IFileService
    {
        IWebHostEnvironment environment;

        public FileService(IWebHostEnvironment env)
        {
            environment = env;
        }


        public async Task<IActionResult> SaveImage(IFormFile imageFile)
        {

            if (imageFile != null && imageFile.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", imageFile.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                return RedirectToAction("Index", "Home"); 
            }

            return RedirectToAction("Index", "Home"); 
        }

        private IActionResult RedirectToAction(string v1, string v2)
        {
            throw new NotImplementedException();
        }

        public bool DeleteImage(string imageFileName)
    {
        try
        {
            var wwwPath = this.environment.WebRootPath;
            var path = Path.Combine(wwwPath, "Uploads\\", imageFileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
}
