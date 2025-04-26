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
    /*Repository<Category> is a generic base class that provides common data access methods (like Add, GetAll, Remove, etc.) applicable to any entity type. By inheriting from this class, CategoryRepository gains these implementations, promoting code reuse.​

ICategoryRepository is an interface that defines additional operations specific to the Category entity, such as Update and Save. By implementing this interface, CategoryRepository ensures it provides these specific functionalities.*/
    public class CategoryRepository :Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
        

        public void Update(Category obj)
        {
            _context.Categories.Update(obj);
        }
    }
}
