using DoctorWebForum.Entities;

namespace DoctorWebForum.DTOs
{
    public class FeedbackModel
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public ApplicationUser User { get; set; }
    }
}
