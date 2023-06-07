using DoctorWebForum.DTOs;
using DoctorWebForum.Entities;

namespace DoctorWebForum.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetPosts(string sTerm = "");
        Task<IEnumerable<PostDisplayModel>> GetPostV1(string sTerm = "");
    }
}
