using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorWebForum.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Location { get; set; }
        [NotMapped ]
        public string? QualificationName { get; set; }
        public int? QualificationId { get; set; }
        public Qualification Qualification { get; set; }
        [NotMapped] 
        public string? ExperienceYear { get; set; }
        public int? ExperienceId { get; set; }
        public Experience Experience { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsDoctor { get; set; }
        public bool IsShow { get; set; }
        public bool IsDelete { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
