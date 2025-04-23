using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create([Bind("Name,DisplayOrder")] Category category)
        {
            if (category.Name== category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Displayorder can not exactly match the Name");

            }
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
