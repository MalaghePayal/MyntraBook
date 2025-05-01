using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Myntra.DataAccess.Data;
using Myntra.DataAccess.Repository.IRepository;

namespace Myntra.DataAccess.Repository
{
    public class Repository<T>: IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            this.dbSet= _context.Set<T>();
            //_context.Categories ==deSet 
            _context.Products.Include(u => u.category).Include(u=>u.CategoryId);
        }
        public void Add(T entity)
        {

            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter,string? includeProperties,bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked)
            {
              query = dbSet;
            }
            else
            {
                 query = dbSet.AsNoTracking();
            }
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in
                    includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }

        //public IEnumerable<T> GetAll(string? includeProperties =null)
        //{
        //    IQueryable<T> query = dbSet;
        //    if (string.IsNullOrEmpty(includeProperties))
        //    {
        //        foreach(var includeProp in 
        //            includeProperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
        //        {
        //            query = query.Include(includeProp);
        //        }
        //    }
        //    return query.ToList();
        //}
        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            // Include the related properties correctly
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
        dbSet.RemoveRange(entity);
        }
    }
}
