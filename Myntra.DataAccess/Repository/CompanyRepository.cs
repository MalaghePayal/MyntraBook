using Microsoft.EntityFrameworkCore;
using Myntra.DataAccess.Data;
using Myntra.DataAccess.Repository.IRepository;
using Myntra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myntra.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private  ApplicationDbContext _context;
        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
            
                _context =context;
        }

        public void Update(Company obj)
        {
           _context.Companies.Update(obj);
        }
    }
}
