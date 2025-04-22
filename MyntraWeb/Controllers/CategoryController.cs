using Microsoft.AspNetCore.Mvc;

namespace MyntraWeb.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
