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
    /*Repository<ApplicationUser> is a generic base class that provides common data access methods (like Add, GetAll, Remove, etc.) applicable to any entity type. By inheriting from this class, ApplicationUserRepository gains these implementations, promoting code reuse.​

IApplicationUserRepository is an interface that defines additional operations specific to the ApplicationUser entity, such as Update and Save. By implementing this interface, ApplicationUserRepository ensures it provides these specific functionalities.*/
    public class ApplicationUserRepository :Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _context;
        public ApplicationUserRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
        

        public void Update(ApplicationUser obj)
        {
            _context.Categories.Update(obj);
        }
    }
}
