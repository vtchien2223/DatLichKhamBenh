using Microsoft.EntityFrameworkCore;
using Nhom4_QLBA_API.Models;

namespace Nhom4_QLBA_API.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<Post> GetPostById(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task AddPost(Post post)
        {
            post.CreatedAt = DateTime.UtcNow;
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePost(Post post)
        {
            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }
    }
}
