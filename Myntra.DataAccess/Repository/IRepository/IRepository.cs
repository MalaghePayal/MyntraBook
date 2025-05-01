using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Myntra.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T:class
    {
        //T will be Category for any other generic model on which we want to perform CRUD
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter, string? includeProperties = null);
        T Get(Expression<Func<T,bool>> filter, string? includeProperties = null,bool tracked =false);
        void Add(T entity);
 
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);


    }
}
