using DoctorWebForum.Constant;
using DoctorWebForum.Data;
using DoctorWebForum.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DoctorWebForum.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountsController : Controller
    {
        private readonly ILogger<UserController> _logger;

        private readonly ApplicationDbContext _context;
        public AccountsController(ApplicationDbContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ApplicationUser
                .Include(x => x.Qualification)
                .Include(x => x.Experience)
                .Where(x => !x.IsDelete)
                .AsQueryable();

            return View( await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(string? id)
        {
            var user = await _context.ApplicationUser
                .Include(x => x.Qualification)
                .Include(x => x.Experience)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (user == null) return NotFound();

            return View(user);
        }

        public IActionResult Create()
        {
            ViewData["QualificationId"] = new SelectList(_context.Qualification, "Id", "Name");
            ViewData["ExperienceId"] = new SelectList(_context.Experience, "Id", "Years");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUser applicationUser)
        {
            ViewData["QualificationId"] = new SelectList(_context.Qualification, "Id", "Id", applicationUser.QualificationId);
            ViewData["ExperienceId"] = new SelectList(_context.Experience, "Id", "Id", applicationUser.ExperienceId);

            //string password = applicationUser.PasswordHash;

            //string hashedPassword = HashPassword(password);

            _context.Add(applicationUser);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string? id)
        {
            
            var applicationUser = await _context.ApplicationUser.FindAsync(id);

            if (applicationUser == null) return NotFound();

            ViewData["QualificationId"] = new SelectList(_context.Qualification, "Id", "Name", applicationUser.QualificationId);
            ViewData["ExperienceId"] = new SelectList(_context.Experience, "Id", "Years", applicationUser.ExperienceId);
            return View(applicationUser);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser applicationUser)
        {

            if (applicationUser == null) return NotFound();

            //string password = applicationUser.PasswordHash;

            //string hashedPassword = HashPassword(password);

            _context.Update(applicationUser);

            await _context.SaveChangesAsync();

            ViewData["QualificationId"] = new SelectList(_context.Qualification, "Id", "Name", applicationUser.QualificationId);
            ViewData["ExperienceId"] = new SelectList(_context.Experience, "Id", "Years", applicationUser.ExperienceId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string? id)
        {
            var applicationUser = await _context.ApplicationUser
                .Include(x => x.Qualification)
                .Include(x => x.Experience)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (applicationUser == null) return NotFound();

            ViewData["QualificationId"] = new SelectList(_context.Qualification, "Id", "Name", applicationUser.QualificationId);
            ViewData["ExperienceId"] = new SelectList(_context.Experience, "Id", "Years", applicationUser.ExperienceId);

            return View(applicationUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string? id)
        {
            var applicationUser = await _context.ApplicationUser.FindAsync(id);

            if (applicationUser != null)
            {
                applicationUser.IsDelete = true;
                _context.Update(applicationUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //public static string HashPassword(string password)
        //{
        //    using (var sha256 = SHA256.Create())
        //    {
        //        // Convert the password to bytes
        //        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        //        // Compute the hash
        //        byte[] hashBytes = sha256.ComputeHash(passwordBytes);

        //        // Convert the hash to a string
        //        string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

        //        return hash;
        //    }
        //}



        //public string HashPassWord(string password)
        //{
        //    SHA256 hash = SHA256.Create();

        //    var passwordBytes = Encoding.Default.GetBytes(password);

        //    var hashedPassword = hash.ComputeHash(passwordBytes);

        //    return Convert.ToHexString(hashedPassword);
        //}

    }
}
