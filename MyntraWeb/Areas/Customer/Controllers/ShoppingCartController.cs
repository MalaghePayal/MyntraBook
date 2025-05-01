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
            foreach (var item in shoppingCartVM.ShoppingCartList)
            {

                item.Price = GetPriceBasedOnQuantity(item);
                shoppingCartVM.OrderTotal+= (item.Price * item.Count);
            }

            return View(shoppingCartVM);
        }

        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {

            if (shoppingCart.Count<=50)
            {
                return shoppingCart.Product.Price1to50;

            }
            else
            {
                if (shoppingCart.Count<100)
                {
                    return shoppingCart.Product.Price50to100;

                }
                else
                {
                    return shoppingCart.Product.Price100;
                }
            }
        }
    }
}
