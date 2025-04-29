using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Myntra.DataAccess.Data;
using Myntra.DataAccess.Repository;
using Myntra.DataAccess.Repository.IRepository;
using Myntra.Models;

namespace MyntraWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.categoryRepository.GetAll().ToList();

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
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Displayorder can not exactly match the Name");

            }
            if (ModelState.IsValid)
            {
                _unitOfWork.categoryRepository.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }

            return View();
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();

            }
            Category? categoryFromDb = _unitOfWork.categoryRepository.Get(u => u.Id == id);
            //Category? categoryFromDb1 = _context.Categories.FirstOrDefault(u => u.Id == id);
            //Category? categoryFromDb2 = _context.Categories.Where(u => u.Id == id).FirstOrDefault();
            if (categoryFromDb == null)
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
                _unitOfWork.categoryRepository.Update(obj);
                _unitOfWork.Save();
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
            Category? categoryFromDb = _unitOfWork.categoryRepository.Get(u => u.Id == id);
            //Category? categoryFromDb1 = _context.Categories.FirstOrDefault(u => u.Id == id);
            //Category? categoryFromDb2 = _context.Categories.Where(u => u.Id == id).FirstOrDefault();
            if (categoryFromDb == null)
            {
                return NotFound();

            }

            return View(categoryFromDb);

        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? obj = _unitOfWork.categoryRepository.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();

            }
            _unitOfWork.categoryRepository.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");


        }
    }
}
