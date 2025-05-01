using Microsoft.EntityFrameworkCore;
using Myntra.DataAccess.Data;
using Myntra.DataAccess.Repository.IRepository;
using Myntra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Myntra.DataAccess.Repository
{
    /*Repository<OrderDetail> is a generic base class that provides common data access methods (like Add, GetAll, Remove, etc.) applicable to any entity type. By inheriting from this class, OrderDetailRepository gains these implementations, promoting code reuse.​

IOrderDetailRepository is an interface that defines additional operations specific to the OrderDetail entity, such as Update and Save. By implementing this interface, OrderDetailRepository ensures it provides these specific functionalities.*/
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private ApplicationDbContext _context;
        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        public void Update(OrderDetail obj)
        {
            _context.OrderDetails.Update(obj);
        }
    }
}
