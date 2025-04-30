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
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
      

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
           
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.companyRepository.GetAll().ToList();
            
            return View(objCompanyList);
        }
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
           
            if (id==null ||id==0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                Company Companyobj = _unitOfWork.companyRepository.Get(u => u.Id==id);
                return View(Companyobj);
            }
            
        }
        [HttpPost]
        public IActionResult Upsert( Company CompanyObj)
        {
            if (ModelState.IsValid)
            {
                if (CompanyObj.Id ==0)
                {
                    _unitOfWork.companyRepository.Add(CompanyObj);
                    _unitOfWork.Save();
                    TempData["Success"] = "Company Created Successfully";
                    return RedirectToAction("Index");

                }
                else
                {
                    _unitOfWork.companyRepository.Update(CompanyObj);
                    _unitOfWork.Save();
                    TempData["Success"] = "Company Updated Successfully";
                    return RedirectToAction("Index");
                }
              
            }
            else
            {
                return View(CompanyObj);
            }

            
        }
        
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();

            }
            Company? CompanyFromDb = _unitOfWork.companyRepository.Get(u => u.Id == id);
           
            if (CompanyFromDb == null)
            {
                return NotFound();

            }

            return View(CompanyFromDb);

        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Company? obj = _unitOfWork.companyRepository.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();

            }
            _unitOfWork.companyRepository.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Company Deleted Successfully";
            return RedirectToAction("Index");


        }
    }
}
