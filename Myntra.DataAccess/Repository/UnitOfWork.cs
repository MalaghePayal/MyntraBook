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
        public IProductRepository productRepository { get; private set; }
        public ICompanyRepository companyRepository { get; private set; }
        public IShoppingCartRepository shoppingCartRepository { get; private set; }
        public IApplicationUserRepository applicationUserRepository { get; private set; }
        public IOrderHeaderRepository orderHeaderRepository { get; private set; }
        public IOrderDetailRepository orderDetailRepository { get; private set; }

        
            
        public UnitOfWork(ApplicationDbContext context)
        {
            _context =context;
            categoryRepository = new CategoryRepository(_context);
            productRepository = new ProductRepository(_context);
            companyRepository = new CompanyRepository(_context);
            shoppingCartRepository =new ShoppingCartRepository(_context);
            applicationUserRepository =new ApplicationUserRepository(_context);
            orderHeaderRepository = new OrderHeaderRepository(_context);
            orderDetailRepository = new OrderDetailRepository(_context);

        }
        

        public void Save()
        {
           _context.SaveChanges();  
        }
    }
}
