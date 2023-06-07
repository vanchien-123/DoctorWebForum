using DoctorWebForum.Data;
using DoctorWebForum.DTOs;
using DoctorWebForum.Interfaces;
using DoctorWebForum.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Versioning;

namespace DoctorWebForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPostService _postService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IPostService postService, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _postService = postService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            //load post and feedback
            var post = await _postService.GetPostV1();
            int rowCount = _context.ApplicationUser.Where(x=>!x.IsDelete).Count();
            var currentUser = _userManager.GetUserId(this.User);

            var homeModel = new HomeDisplayModel
            {
                Posts = post.Select(x => new PostDisplayModel()
                {
                    currentUserId = currentUser,
                    Id = x.Id,
                    User=x.User,
                    Description = x.Description,  
                    Feedbacks = x.Feedbacks,
                    HtmlContent = x.HtmlContent,
                    Title = x.Title,
                    UserId = x.UserId
                }),

                intCount = rowCount,
            };

            return View(homeModel);
        }

        public IActionResult Blog() 
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost([Bind("Id,Title,Description,HtmlContent,UserId")] Post post)
        {
            post.UserId = _userManager.GetUserId(this.User);

            if(post.UserId == null)
            {
                return Redirect("~/Identity/Account/Login");
            }

            _context.Add(post);
            await _context.SaveChangesAsync();
            return Redirect("~/");
            //return View(post);
        }

        public async Task<IActionResult> EditPost(int? id)
        {
            var post = await _context.Post.Include(x => x.User).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (post == null) return NotFound();

            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Name", post.UserId);
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPostV2(int id, Post post)
        {
            post.UserId = _userManager.GetUserId(this.User);

            _context.Update(post);
            await _context.SaveChangesAsync();

            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Name", post.UserId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var post = await _context.Post.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (post != null)
            {
                post.IsDelete = true;
                _context.Update(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFeedback(FeedbackModel model)
        {
            var entity = new Feedback()
            {
                Content = model.Content,
                PostId = model.PostId
            };
            entity.UserId = _userManager.GetUserId(this.User);

            if (entity.UserId == null)
            {
                return Redirect("~/Identity/Account/Login");
            }

            //feedback.PostId = _post.Id;
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return Redirect("~/");

            //ViewData["PostId"] = new SelectList(_context.Post, "Id", "Id", feedback.PostId);
            //ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", feedback.UserId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFeedback(int? id)
        {
            var feedback = await _context.Feedback.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (feedback != null)
            {
                _context.Feedback.Remove(feedback);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}