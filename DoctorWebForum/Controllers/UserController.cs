using DoctorWebForum.DTOs;
using DoctorWebForum.Interfaces;
using DoctorWebForum.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace DoctorWebForum.Controllers
{
    public class UserController : Controller
    {

        private readonly ILogger<UserController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Member(string sTerm = "", int qualificationId = 0, int experienceId = 0)
        {
            IEnumerable<ApplicationUser> users = await _userService.GetUsers(sTerm, qualificationId, experienceId);
            IEnumerable<Qualification> qualifications = await _userService.Qualifications();
            IEnumerable<Experience> experiences = await _userService.Experiences();
            UserDisplayModel userModel = new UserDisplayModel
            {
                Users = users,
                Qualifications = qualifications,
                STerm = sTerm,
                QualificationId = qualificationId,
                Experiences = experiences,
                ExperienceId = experienceId,
                
            };
            return View(userModel);
        }

        public async Task<IActionResult> Details(string id)
        {
            var currentUser = _userManager.GetUserId(this.User);

            var user = await _userService.GetUserById(id);

            user.currentId = currentUser;

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}
