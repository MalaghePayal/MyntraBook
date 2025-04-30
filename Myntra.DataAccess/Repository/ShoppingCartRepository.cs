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
    /*Repository<ShoppingCart> is a generic base class that provides common data access methods (like Add, GetAll, Remove, etc.) applicable to any entity type. By inheriting from this class, ShoppingCartRepository gains these implementations, promoting code reuse.​

IShoppingCartRepository is an interface that defines additional operations specific to the ShoppingCart entity, such as Update and Save. By implementing this interface, ShoppingCartRepository ensures it provides these specific functionalities.*/
    public class ShoppingCartRepository :Repository<ShoppingCart>, IShoppingCartRepository
    {
        private ApplicationDbContext _context;
        public ShoppingCartRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
        

        public void Update(ShoppingCart obj)
        {
            _context.ShoppingCarts.Update(obj);
        }
    }
}
