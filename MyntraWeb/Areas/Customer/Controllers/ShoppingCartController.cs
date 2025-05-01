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
                includeProperties: "Product"),
                OrderHeader=new()
            };
            foreach (var item in shoppingCartVM.ShoppingCartList)
            {

                item.Price = GetPriceBasedOnQuantity(item);
                shoppingCartVM.OrderHeader.OrderTotal += (item.Price * item.Count);
            }

            return View(shoppingCartVM);
        }


        public IActionResult Summary()
        {
            // Step 1: Retrieve the current logged-in user's ID
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            // Step 2: Initialize ViewModel with shopping cart items and empty order header
            shoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.shoppingCartRepository.GetAll(u => u.ApplicationUserId == UserId,
                includeProperties: "Product"),
                OrderHeader =new()
            }; 
            // Step 3: Populate OrderHeader with current user's profile details
            shoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.applicationUserRepository.Get(u => u.Id == UserId);
            
            shoppingCartVM.OrderHeader.Name = shoppingCartVM.OrderHeader.ApplicationUser.Name;
            shoppingCartVM.OrderHeader.PhoneNumber = shoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;

            shoppingCartVM.OrderHeader.StreetAddress = shoppingCartVM.OrderHeader.ApplicationUser.StreetAdress;
            shoppingCartVM.OrderHeader.City = shoppingCartVM.OrderHeader.ApplicationUser.City;
            shoppingCartVM.OrderHeader.State = shoppingCartVM.OrderHeader.ApplicationUser.State;
            shoppingCartVM.OrderHeader.PostalCode = shoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

            // Step 4: Calculate OrderTotal by iterating over cart items and computing price
            foreach (var item in shoppingCartVM.ShoppingCartList)
            {

                item.Price = GetPriceBasedOnQuantity(item);
                shoppingCartVM.OrderHeader.OrderTotal += (item.Price * item.Count);
            }
            // Step 5: Return the populated ViewModel to the summary view
            return View(shoppingCartVM);
        }


        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {

            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price1to50;

            }
            else
            {
                if (shoppingCart.Count < 100)
                {
                    return shoppingCart.Product.Price50to100;

                }
                else
                {
                    return shoppingCart.Product.Price100;
                }
            }
        }

        public IActionResult plus(int cartId)
        {
            var cartFromDb = _unitOfWork.shoppingCartRepository.Get(u => u.Id == cartId);
            cartFromDb.Count += 1;
            _unitOfWork.shoppingCartRepository.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));


        }
        public IActionResult minus(int cartId)
        {
            var cartFromDb = _unitOfWork.shoppingCartRepository.Get(u => u.Id == cartId);
            if (cartFromDb.Count <= 1)
            {
                _unitOfWork.shoppingCartRepository.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.shoppingCartRepository.Update(cartFromDb);

            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult remove(int cartId)
        {
            var cartFromDb = _unitOfWork.shoppingCartRepository.Get(u => u.Id == cartId);

            _unitOfWork.shoppingCartRepository.Remove(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));

        }
    }
}
