using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Myntra.DataAccess.Repository.IRepository;
using Myntra.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace MyntraWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> ProductList = _unitOfWork.productRepository.GetAll(includeProperties:"category"); 
            return View(ProductList);
        }
        public IActionResult Details(int productId)
        {
            ShoppingCart Cart = new ()
            {
                Product = _unitOfWork.productRepository.Get(u => u.Id == productId, includeProperties: "category"),
                Count = 1,
                ProductId = productId
            };
          
            return View(Cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart )
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = UserId;

            ShoppingCart cartFromDb = _unitOfWork.shoppingCartRepository.Get(u=>u.ApplicationUserId==UserId && u.ProductId==shoppingCart.ProductId);

            if (cartFromDb !=null)
            {
                //Shopping cart exists. update count
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.shoppingCartRepository.Update(cartFromDb);

            }
            else
            {
                //add new cart record
                _unitOfWork.shoppingCartRepository.Add(shoppingCart);
            }
                _unitOfWork.shoppingCartRepository.Add(shoppingCart);

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
