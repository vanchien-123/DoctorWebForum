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
using DoctorWebForum.DTOs;
using DoctorWebForum.Interfaces;
using Microsoft.Extensions.Hosting;
using DoctorWebForum.Constant;
using Microsoft.AspNetCore.Authorization;

namespace DoctorWebForum.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string UserId;
        private readonly IPostService _postService;
        public PostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IPostService postService)
        {
            _context = context;
            _userManager = userManager;
            _postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Post
                .Include(p => p.User)
                .Where(x => !x.IsDelete && !x.User.IsDelete);
                //.Where(x => !x.User.IsDelete);

            //(ICollection<Feedback>)(x.Feedbacks != null ? x.Feedbacks.Where(p => !p.User.IsDelete) : null)

            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            var post = await _context.Post.Include(p => p.User).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (post == null) return NotFound();

            return View(post);
        }

        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post)
        {
            _context.Add(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", post.UserId);
            return View(post);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var post = await _context.Post.FindAsync(id);

            if (post == null) return NotFound();

            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Name", post.UserId);
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Post post)
        {
            _context.Update(post);
            await _context.SaveChangesAsync();

            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Name", post.UserId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var post = await _context.Post.Include(p => p.User).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (post == null) return NotFound();


            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Name", post.UserId);

            return View(post);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post.FindAsync(id);

            if (post != null)
            {
                post.IsDelete = true;
                _context.Update(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
