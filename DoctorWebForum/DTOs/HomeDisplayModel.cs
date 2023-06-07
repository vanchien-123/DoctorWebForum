using DoctorWebForum.Entities;    

namespace DoctorWebForum.DTOs
{
    public class HomeDisplayModel
    {
        public IEnumerable<PostDisplayModel> Posts { get; set; } = new List<PostDisplayModel>();

        public int intCount { get; set; }
        

    }
}
