﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DoctorWebForum.Interfaces;
using DoctorWebForum.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoctorWebForum.Data;

namespace DoctorWebForum.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IFileService _fileService;


        public IndexModel(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IFileService fileService)
        {
                _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _fileService = fileService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Name")]
            public string Name { get; set; }
            public string Location { get; set; }
            public int? QualificationId { get; set; }
            public Qualification Qualification { get; set; }
            public int? ExperienceId { get; set; }
            public Experience Experience { get; set; }
            public string ProfilePicture { get; set; }
            public IFormFile ImageFile { get; set; }

            [Display(Name = "Is Show?")]
            public bool IsShow { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            ViewData["QualificationId"] = new SelectList(_context.Qualification, "Id", "Name");
            ViewData["ExperienceId"] = new SelectList(_context.Experience, "Id", "Years");

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Name = user.Name,
                Location = user.Location,
                QualificationId = user.QualificationId,
                ExperienceId = user.ExperienceId,
                ProfilePicture = user.ProfilePicture,
                IsShow = user.IsShow,
        };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if (Input.Name != user.Name)
            {
                user.Name = Input.Name;
                await _userManager.UpdateAsync(user);
            }

            if (Input.Location != user.Location)
            {
                user.Location = Input.Location;
                await _userManager.UpdateAsync(user);
            }

            if (Input.QualificationId != user.QualificationId)
            {
                user.QualificationId = Input.QualificationId;
                await _userManager.UpdateAsync(user);
            }

            if (Input.ExperienceId != user.ExperienceId)
            {
                user.ExperienceId = Input.ExperienceId;
                await _userManager.UpdateAsync(user);
            }

            if (Input.ImageFile != null)
            {
                user.ProfilePicture = Input.ImageFile.FileName;

                if (Input.ProfilePicture != user.ProfilePicture)
                {
                    if (Input.ImageFile != null)
                    {
                        var result = _fileService.SaveImage(Input.ImageFile);
                        await _userManager.UpdateAsync(user);

                    }
                }
            }
            

            if (Input.IsShow != user.IsShow)
            {
                user.IsShow = Input.IsShow;
                await _userManager.UpdateAsync(user);
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
