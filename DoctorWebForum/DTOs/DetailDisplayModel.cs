using DoctorWebForum.Entities;

namespace DoctorWebForum.DTOs
{
    public class DetailDisplayModel
    {
        public bool IsUser { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public int? QualificationId { get; set; }
        public Qualification Qualification { get; set; }
        public int? ExperienceId { get; set; }
        public Experience Experience { get; set; }
        public string ProfilePicture { get; set; }
        public ApplicationUser User { get; set; }
        public bool IsDoctor { get; set; }
        public bool IsShow { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<Post> Posts { get; set; } 
    }
}
