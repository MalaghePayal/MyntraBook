using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Myntra.DataAccess.Repository.IRepository;
using Myntra.Models.ViewModels;

namespace MyntraWeb.Areas.Customer.Controllers
{
    public class ShoppingCartController : Controller
    {
        [Area("Customer")]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
