using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Myntra.DataAccess.Data;
using Myntra.DataAccess.Repository;
using Myntra.DataAccess.Repository.IRepository;
using Myntra.Models;
using Myntra.Models.ViewModels;
using System.Collections.Generic;

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
        public IActionResult Upsert(int? id)
        {
            ProductViewModel ProductVM = new()
            {
                CategoryList =  _unitOfWork.categoryRepository.GetAll().ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()

                }),
                Product = new Product()
            };
            if (id==null ||id==0)
            {
                //create
                return View(ProductVM);
            }
            else
            {
                //update
                ProductVM.Product = _unitOfWork.productRepository.Get(u => u.Id==id);
                return View(ProductVM);
            }
            
        }
        [HttpPost]
        public IActionResult Upsert( ProductViewModel ProductVM,IFormFile? file)
        {
            
            
            if (ModelState.IsValid)
            {
                if (ProductVM.Product.Id ==0)
                {
                    _unitOfWork.productRepository.Add(ProductVM.Product);
                    _unitOfWork.Save();
                    TempData["Success"] = "Product Created Successfully";
                    return RedirectToAction("Index");

                }
                else
                {
                    _unitOfWork.productRepository.Update(ProductVM.Product);
                    _unitOfWork.Save();
                    TempData["Success"] = "Product Updated Successfully";
                    return RedirectToAction("Index");
                }
              
            }
            else
            {
                ProductVM.CategoryList = _unitOfWork.categoryRepository.GetAll().ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()

                });
                    
              
                return View(ProductVM);
            }

            
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
