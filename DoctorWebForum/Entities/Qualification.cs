namespace DoctorWebForum.Entities
{
    public class Qualification
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
}
