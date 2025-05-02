using Microsoft.AspNetCore.Mvc;
using Myntra.DataAccess.Repository.IRepository;

namespace MyntraWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork =unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
