using Myntra.DataAccess.Data;
using Myntra.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myntra.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICategoryRepository categoryRepository { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context =context;
            categoryRepository = new CategoryRepository(_context);
           
    }
        

        public void Save()
        {
           _context.SaveChanges();  
        }
    }
}
