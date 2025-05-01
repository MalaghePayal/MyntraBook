using Microsoft.AspNetCore.Mvc;

namespace MyntraWeb.Areas.Customer.Controllers
{
    public class ShoppingCartController : Controller
    {
        [Area("Customer")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
