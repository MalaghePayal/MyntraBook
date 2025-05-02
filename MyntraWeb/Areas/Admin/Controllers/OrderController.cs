using Microsoft.AspNetCore.Mvc;
using Myntra.DataAccess.Repository.IRepository;
using Myntra.Models;
using Myntra.Models.ViewModels;

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
            // Get all OrderHeaders
            var orderHeaders = _unitOfWork.orderHeaderRepository.GetAll(includeProperties: "ApplicationUser");

            // Create list of OrderVMs
            List<OrderVM> orderVMList = new();

            foreach (var header in orderHeaders)
            {
                var orderDetails = _unitOfWork.orderDetailRepository.GetAll(u => u.OrderHeaderId == header.Id,
                    includeProperties: "Product");

                orderVMList.Add(new OrderVM
                {
                    OrderHeader = header,
                    OrderDetail = orderDetails
                });
            }

            return View(orderVMList);
        }
    }
}
