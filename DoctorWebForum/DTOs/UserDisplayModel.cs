using DoctorWebForum.Entities;

namespace DoctorWebForum.DTOs
{   
    public class UserDisplayModel
    {   
        public IEnumerable<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();/*để tránh lỗi Referent*/
        public IEnumerable<Qualification> Qualifications{ get; set; } = new List<Qualification>(); /*để tránh lỗi Referent*/
        public IEnumerable<Experience> Experiences { get; set; } = new List<Experience>();/*để tránh lỗi Referent*/
        public string Id { get; set; }
        public string STerm { get; set; } = "";
        public bool IsDoctor { get; set; }
        public string  Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string QualificationName { get; set; }
        public string ProfilePicture { get; set; }
        public int? ExperienceId { get; set; }
        public Experience Experience { get; set; }
        public int? QualificationId { get; set; }
        public Qualification Qualification { get; set; }
        public string ExperienceYear { get; set; }
        public bool IsShow { get; set; }
        public string currentId { get; set; }
        public IEnumerable<PostDisplayModel> Posts { get; set; } = new List<PostDisplayModel>();

    }
}
