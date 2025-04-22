using Microsoft.AspNetCore.Mvc;
using MyntraWeb.Data;
using MyntraWeb.Models;

namespace MyntraWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _context.Categories.ToList();

            return View(objCategoryList);
        }
    }
}
