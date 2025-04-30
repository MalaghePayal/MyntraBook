using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Myntra.DataAccess.Data;
using Myntra.DataAccess.Repository;
using Myntra.DataAccess.Repository.IRepository;
using Myntra.Models;
using Myntra.Models.ViewModels;
using Myntra.Utility;
using System.Collections.Generic;
using System.IO;

namespace MyntraWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_User_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment    _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.productRepository.GetAll(includeProperties:"category").ToList();
            
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
                ProductVM.Product = _unitOfWork.productRepository.Get(u => u.Id==id, includeProperties: "category");
                return View(ProductVM);
            }
            
        }
        [HttpPost]
        public IActionResult Upsert( ProductViewModel ProductVM,IFormFile? file)
        {
            
            
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file!=null)
                {
                    string fileName = Guid.NewGuid().ToString()+ Path.GetExtension(file.FileName);    
                    string ProductPath = Path.Combine(wwwRootPath, @"images\product");
                    if (!string.IsNullOrEmpty(ProductVM.Product.ImageUrl))
                    {
                        // i.e. we are updating old image with new one. bcz we are checking if file name is not empty (file!=null)and imageurl is not empty
                        //so delete the old image first 
                        //old image is saved in product table with \\ append to it at start so trim this backslash
                        //\images\product\27e5a121-ae9b-4534-9bfd-d5b410b7fc09.jpg saved file in products table 
                        var oldImagePath = Path.Combine(wwwRootPath, ProductVM.Product.ImageUrl.TrimStart('\\'));
                        //now delete that old image
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(ProductPath, fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    ProductVM.Product.ImageUrl = @"\images\product\" + fileName;
                }
                

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
