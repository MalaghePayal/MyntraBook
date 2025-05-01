using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Myntra.DataAccess.Repository.IRepository;
using Myntra.Models;
using Myntra.Models.ViewModels;
using System.Security.Claims;

namespace MyntraWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        
        
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM shoppingCartVM { get; set; }

        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        
        [Authorize]
        public IActionResult Index()
        {
            // Retrieve the current logged-in user's ID
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            shoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.shoppingCartRepository.GetAll(u => u.ApplicationUserId == UserId,
                includeProperties:"Product")
            };
            return View(shoppingCartVM);
        }
    }
}
