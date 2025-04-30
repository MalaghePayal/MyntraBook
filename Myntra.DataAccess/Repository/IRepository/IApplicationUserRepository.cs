using Myntra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myntra.DataAccess.Repository.IRepository
{
    public interface IApplicationUserRepository:IRepository<ApplicationUser>
    {
        void Update(ApplicationUser obj);
        
    }
}
