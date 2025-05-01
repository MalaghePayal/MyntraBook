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
    /*Repository<OrderHeader> is a generic base class that provides common data access methods (like Add, GetAll, Remove, etc.) applicable to any entity type. By inheriting from this class, OrderHeaderRepository gains these implementations, promoting code reuse.​

IOrderHeaderRepository is an interface that defines additional operations specific to the OrderHeader entity, such as Update and Save. By implementing this interface, OrderHeaderRepository ensures it provides these specific functionalities.*/
    public class OrderHeaderRepository :Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _context;
        public OrderHeaderRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
        

        public void Update(OrderHeader obj)
        {
            _context.OrderHeaders.Update(obj);
        }
    }
}
