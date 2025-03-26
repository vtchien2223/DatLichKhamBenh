using Microsoft.AspNetCore.Mvc;
using nhom4_quanlyadmin.Models;
using nhom4_quanlyadmin.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nhom4_quanlyadmin.Controllers
{
    public class PostApiController : Controller
    {
        private readonly ApiService _apiService;

        public PostApiController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // Display list of posts
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var posts = await _apiService.GetAsync<List<Post>>("PostApi");
            return View(posts);
        }

        // Show form for adding a new post
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Post());  // Pass a new Post object to the view
        }

        // Handle creation of a new post
        [HttpPost]
        public async Task<IActionResult> Create(Post post)
        {
            if (ModelState.IsValid)
            {
                // Automatically set CreatedAt to current date and time
                post.CreatedAt = DateTime.Now;

                var response = await _apiService.PostAsync("PostApi", post);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Post created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                TempData["ErrorMessage"] = "Error creating post.";
            }
            return View(post);  // Return the view with the invalid model
        }

        // Show form for editing a post
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _apiService.GetAsync<Post>($"PostApi/{id}");
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // Handle updating a post
        [HttpPost]
        public async Task<IActionResult> Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiService.PutAsync($"PostApi/{post.Id}", post);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Post updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                TempData["ErrorMessage"] = "Error updating post.";
            }
            return View(post);
        }

        // Handle deleting a post
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiService.DeleteAsync($"PostApi/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Post deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Error deleting post.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
