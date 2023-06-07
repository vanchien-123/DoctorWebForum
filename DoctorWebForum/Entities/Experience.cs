namespace DoctorWebForum.Entities
{
    public class Experience
    {
        public int Id { get; set; }
        public string? Years {get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
}
