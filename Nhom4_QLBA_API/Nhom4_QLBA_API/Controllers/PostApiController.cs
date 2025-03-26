using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nhom4_QLBA_API.Models;
using Nhom4_QLBA_API.Repositories;

namespace Nhom4_QLBA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostApiController : ControllerBase
    {
        private readonly IPostRepository _repository;

        public PostApiController(IPostRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            return Ok(await _repository.GetAllPosts());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _repository.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CreatePost(Post post)
        {
            await _repository.AddPost(post);
            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePost(int id, Post post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }
            await _repository.UpdatePost(post);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost(int id)
        {
            var post = await _repository.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            await _repository.DeletePost(id);
            return NoContent();
        }
    }
}
