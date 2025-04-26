using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Myntra.DataAccess.Data;
using Myntra.DataAccess.Repository;
using Myntra.DataAccess.Repository.IRepository;
using Myntra.Models;

namespace MyntraWeb.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.productRepository.GetAll().ToList();

            return View(objProductList);
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create([Bind("Title,Description,ISBN,Author,ListPrice,Price1to50,Price50to100,Price100")] Product obj)
        {
        //    if (obj.Name == obj.DisplayOrder.ToString())
        //    {
        //        ModelState.AddModelError("name", "The Displayorder can not exactly match the Name");

        //    }
            if (ModelState.IsValid)
            {
                _unitOfWork.productRepository.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Product Created Successfully";
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
            Product? productFromDb = _unitOfWork.productRepository.Get(u => u.Id == id);
            //Category? categoryFromDb1 = _context.Categories.FirstOrDefault(u => u.Id == id);
            //Category? categoryFromDb2 = _context.Categories.Where(u => u.Id == id).FirstOrDefault();
            if (productFromDb == null)
            {
                return NotFound();

            }

            return View(productFromDb);

        }
        [HttpPost]
        public IActionResult Edit([Bind("Id,Title,Description,ISBN,Author,ListPrice,Price1to50,Price50to100,Price100")] Product obj)
        {
           
            if (ModelState.IsValid)
            {
                _unitOfWork.productRepository.Update(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Product Updated Successfully";
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
            Product? productFromDb = _unitOfWork.productRepository.Get(u => u.Id == id);
            //Category? categoryFromDb1 = _context.Categories.FirstOrDefault(u => u.Id == id);
            //Category? categoryFromDb2 = _context.Categories.Where(u => u.Id == id).FirstOrDefault();
            if (productFromDb == null)
            {
                return NotFound();

            }

            return View(productFromDb);

        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Product? obj = _unitOfWork.productRepository.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();

            }
            _unitOfWork.productRepository.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Product Deleted Successfully";
            return RedirectToAction("Index");


        }
    }
}
