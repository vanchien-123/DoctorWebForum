using DoctorWebForum.Entities;

namespace DoctorWebForum.DTOs
{
    public class PostDisplayModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? HtmlContent { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public string? currentUserId { get; set; }
    }
}
