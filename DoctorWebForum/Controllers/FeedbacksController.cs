using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoctorWebForum.Data;
using DoctorWebForum.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace DoctorWebForum.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FeedbacksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Post _post;
        private readonly string UserId;

        public FeedbacksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, Post post)
        {
            _context = context;
            _userManager = userManager;
            _post = post;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Feedback.Include(x => x.Post).Include(x=>x.User).AsQueryable();
            return View( await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            var feedback = await _context.Feedback
                .Include(x => x.Post)
                .Include(x=>x.User)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            if (feedback == null) return NotFound();

            return View(feedback);
        }

        public IActionResult Create()
        {
            ViewData["PostId"] = new SelectList(_context.Post, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Feedback feedback)
        {
            ViewData["PostId"] = new SelectList(_context.Post, "Id", "Id", feedback.PostId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", feedback.UserId);

            _context.Add(feedback);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var feedback = await _context.Feedback.FindAsync(id);    
            if (feedback == null) return NotFound();

            ViewData["PostId"] = new SelectList(_context.Post, "Id", "Id", feedback.PostId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Name", feedback.UserId);
            return View(feedback);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Feedback feedback)
        {
            if (feedback == null) return NotFound();

            _context.Update(feedback);

            await _context.SaveChangesAsync();

            ViewData["PostId"] = new SelectList(_context.Post, "Id", "Id", feedback.PostId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", feedback.UserId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id) 
        {
            var feedback = await _context.Feedback
                .Include(x => x.User)
                .Include(x=>x.Post)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (feedback == null) return NotFound();

            ViewData["PostId"] = new SelectList(_context.Post, "Id", "Id", feedback.PostId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Name", feedback.UserId);

            return View(feedback);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var feedback = await _context.Feedback.FindAsync(id);
            if (feedback != null)
            {
                _context.Feedback.Remove(feedback);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
