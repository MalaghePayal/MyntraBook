using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Myntra.DataAccess.Repository.IRepository;
using Myntra.Models;
using Myntra.Models.ViewModels;
using Myntra.Utility;
using System.Security.Claims;

namespace MyntraWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {


        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
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
        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {
            // Step 1: Retrieve the current logged-in user's ID
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

             // Step 2: Fetch all shopping cart items for the user, including related Product info
            shoppingCartVM.ShoppingCartList = _unitOfWork.shoppingCartRepository.GetAll(u => u.ApplicationUserId == UserId,
                includeProperties: "Product");

            // Step 3: Set order date and associate the order with the current user
            shoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            shoppingCartVM.OrderHeader.ApplicationUserId = UserId;

            // Step 4: Retrieve the ApplicationUser object to check if the user is linked to a company
            ApplicationUser applicationUser = _unitOfWork.applicationUserRepository.Get(u => u.Id == UserId);

            // Step 4: Calculate OrderTotal by iterating over cart items and computing price
            foreach (var item in shoppingCartVM.ShoppingCartList)
            {

                item.Price = GetPriceBasedOnQuantity(item);
                shoppingCartVM.OrderHeader.OrderTotal += (item.Price * item.Count);
            }
            // Step 6: Set payment and order status based on account type (regular vs company)
            if (applicationUser.CompanyId.GetValueOrDefault()==0)
            {
                //it is a regular customer account
                // Regular customer – payment required upfront
                shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                shoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            }
            else{
                //it is company account user  
                // Company account – approved for delayed payment

                shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                shoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            }
            // Step 7: Save the OrderHeader (main order summary) to the database
            _unitOfWork.orderHeaderRepository.Add(shoppingCartVM.OrderHeader);
            _unitOfWork.Save();
            // Step 8: Loop through each cart item to create corresponding OrderDetail records
            foreach (var cart in shoppingCartVM.ShoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = shoppingCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count=cart.Count
                };

                // Step 9: Save OrderDetail entry to the database
                _unitOfWork.orderDetailRepository.Add(orderDetail);
                _unitOfWork.Save();
            }
            // Step 10: (Optional - Redundant logic) Re-check if user is a regular account to capture payment
            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                //it is a regular customer account and we need to capture payment
                //// Stripe payment logic placeholder (not implemented yet)
                shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                shoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            }
            // Step 11: Redirect user to OrderConfirmation page with newly created order ID
            return RedirectToAction(nameof(OrderConfirmation), new {id=shoppingCartVM.OrderHeader.Id});
        }
        
        // Step 7: Accept the order ID and pass it to the view for displaying confirmation details
        public IActionResult OrderConfirmation(int id)
        { 
            // The view will use this ID to fetch and display order details
            return View(id);
           
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
