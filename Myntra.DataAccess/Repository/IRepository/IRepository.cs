using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Myntra.DataAccess.Repository.IRepository
{
    internal interface IRepository<T> where T:class
    {
        //T will be Category for any other generic model on which we want to perform CRUD
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T,bool>> filter);
        void Add(T entity);
 
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);


    }
}
