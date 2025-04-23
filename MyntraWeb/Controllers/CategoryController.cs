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
        public IActionResult Create([Bind("Name,DisplayOrder")] Category obj)
        {
            if (obj.Name== obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Displayorder can not exactly match the Name");

            }
            if (ModelState.IsValid)
            {
                _context.Categories.Add(obj);
                _context.SaveChanges();
                TempData["Success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }

            return View();
        }
        [HttpGet]
        public IActionResult Edit(int? id) 
        {
            if (id==null || id==0)
            {
                return NotFound();

            }
            Category? categoryFromDb = _context.Categories.Find(id);
            //Category? categoryFromDb1 = _context.Categories.FirstOrDefault(u => u.Id == id);
            //Category? categoryFromDb2 = _context.Categories.Where(u => u.Id == id).FirstOrDefault();
            if (categoryFromDb ==null)
            {
                return NotFound();

            }

            return View(categoryFromDb);
        
        }
        [HttpPost]
        public IActionResult Edit([Bind("Id,Name,DisplayOrder")] Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Displayorder can not exactly match the Name");

            }
            if (ModelState.IsValid)
            {
                _context.Categories.Update(obj);
                _context.SaveChanges();
                TempData["Success"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }

            return View(obj);

        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();

            }
            Category? categoryFromDb = _context.Categories.Find(id);
            //Category? categoryFromDb1 = _context.Categories.FirstOrDefault(u => u.Id == id);
            //Category? categoryFromDb2 = _context.Categories.Where(u => u.Id == id).FirstOrDefault();
            if (categoryFromDb == null)
            {
                return NotFound();

            }

            return View(categoryFromDb);

        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? obj = _context.Categories.Find(id);
            if (obj ==null)
            {
                return NotFound();

            }
                _context.Categories.Remove(obj);
                _context.SaveChanges();
            TempData["Success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");
          

        }
    }
}
