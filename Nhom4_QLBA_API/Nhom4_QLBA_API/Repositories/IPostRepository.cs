using Nhom4_QLBA_API.Models;

namespace Nhom4_QLBA_API.Repositories
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllPosts();
        Task<Post> GetPostById(int id);
        Task AddPost(Post post);
        Task UpdatePost(Post post);
        Task DeletePost(int id);
    }
}
