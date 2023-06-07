using DoctorWebForum.Data;
using DoctorWebForum.Interfaces;
using DoctorWebForum.Entities;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using DoctorWebForum.DTOs;

namespace DoctorWebForum.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context; ;
        }

        public async Task<IEnumerable<Experience>> Experiences()
        {
            return await _context.Experience.ToListAsync();
        }

        public async Task<IEnumerable<Qualification>> Qualifications()
        {
            return await _context.Qualification.ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers(string sTerm = "", int qualificationId = 0, int experienceId = 0)
        {
            sTerm = sTerm.ToLower();
            const bool V = false;
            IEnumerable<ApplicationUser> user = await (from ApplicationUser in _context.ApplicationUser
                                                       join qualification in _context.Qualification
                                             on ApplicationUser.QualificationId equals qualification.Id
                                                       join experiennce in _context.Experience
                                                       on ApplicationUser.ExperienceId equals experiennce.Id
                                                       where (string.IsNullOrWhiteSpace(sTerm) || (ApplicationUser != null && ApplicationUser.Name.ToLower().StartsWith(sTerm))) && ApplicationUser.IsDelete == false
                                                       select new ApplicationUser
                                                       {
                                                           Id = ApplicationUser.Id,
                                                           ProfilePicture = ApplicationUser.ProfilePicture,
                                                           Name = ApplicationUser.Name,
                                                           PhoneNumber = ApplicationUser.PhoneNumber,
                                                           Location = ApplicationUser.Location,
                                                           IsDoctor = ApplicationUser.IsDoctor,
                                                           QualificationName = qualification.Name,
                                                           QualificationId = ApplicationUser.QualificationId,
                                                           ExperienceYear = experiennce.Years,
                                                           ExperienceId = ApplicationUser.ExperienceId
                                                           //GenreName = qualification.Name
                                                       }
                         ).ToListAsync();
            if (qualificationId > 0)
            {

                user = user.Where(a => a.QualificationId == qualificationId).ToList();

            }

            if (experienceId > 0)
            {

                user = user.Where(a => a.ExperienceId == experienceId).ToList();

            }
            return user;
        }

        public async Task<UserDisplayModel> GetUserById(string Id)
        {
            var user = await _context.ApplicationUser
                .Include(x => x.Qualification)
                .Include(x => x.Experience)
                .Include(x => x.Posts).ThenInclude(x=>x.Feedbacks).ThenInclude(x=>x.User)
                .Include(x => x.Feedbacks)
                .Where(x => x.Id == Id).Select(x => new UserDisplayModel()
                {
                    Id = x.Id,
                    Email = x.Email,
                    Experience = x.Experience,
                    ExperienceId = x.ExperienceId,
                    ExperienceYear = x.ExperienceYear,
                    IsDoctor = x.IsDoctor,
                    IsShow = x.IsShow,
                    Location = x.Location,
                    Name = x.Name,
                    PhoneNumber = x.PhoneNumber,
                    Posts = x.Posts.Select(p => new PostDisplayModel
                    {
                        currentUserId = p.UserId,
                        Description = p.Description,
                        Feedbacks = p.Feedbacks,
                        HtmlContent = p.HtmlContent,
                        Id = p.Id,
                        Title = p.Title,
                        User = p.User,
                        UserId = p.UserId
                    }),
                    ProfilePicture = x.ProfilePicture,
                    Qualification = x.Qualification,
                    QualificationId = x.QualificationId,
                }).FirstOrDefaultAsync();

            return user;
        }
    }
}
