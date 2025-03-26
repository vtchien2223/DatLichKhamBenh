using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace nhom4_quanlyadmin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
