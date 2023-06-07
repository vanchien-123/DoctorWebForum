using DoctorWebForum.Data;
using DoctorWebForum.DTOs;
using DoctorWebForum.Interfaces;
using DoctorWebForum.Entities;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Identity;

namespace DoctorWebForum.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _context;

        public PostService(ApplicationDbContext context)
        {
            _context = context; ;
        }
        public async Task<IEnumerable<Post>> GetPosts(string sTerm = "")
        {
            var query = _context.Post.AsQueryable();

            if (!string.IsNullOrEmpty(sTerm))
                query = query.Where(x => x.Title.ToUpper().Contains(sTerm.ToUpper()));

            var result = await query.ToListAsync();
            //sTerm = sTerm.ToLower();
            //IEnumerable<Post> post = await (from Post in _context.Post
            //                                where string.IsNullOrWhiteSpace(sTerm) || (Post != null && Post.Description.ToLower().StartsWith(sTerm))
            //                                select new Post
            //                                {
            //                                    Id = Post.Id,
            //                                    Title = Post.Title,
            //                                    Description = Post.Description,
            //                                    HtmlContent = Post.HtmlContent,
            //                                    UserId = Post.UserId
            //                                }
            //             ).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<PostDisplayModel>> GetPostV1(string sTerm = "")
        {
            var query = _context.Post
                .Include(x => x.Feedbacks).ThenInclude(x => x.User)
                .Where(x => !x.IsDelete)
                .Include(y => y.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(sTerm))
                query = query.Where(x => x.Description.ToUpper().Contains(sTerm.ToUpper()));



            var results = await query.Select(x => new PostDisplayModel
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                HtmlContent = x.HtmlContent,
                UserId = x.UserId,
                User = x.User,
                Feedbacks = (ICollection<Feedback>)(x.Feedbacks != null ? x.Feedbacks.Where(p => !p.User.IsDelete) : null)
                //currentUser = _userManager.GetUserId(this.User);
                ,
            }).Where(x => !x.User.IsDelete).ToListAsync();

            return results;
        }

    }
}
