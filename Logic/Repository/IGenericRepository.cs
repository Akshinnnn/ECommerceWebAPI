using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task Add(T entity);
        IQueryable<T> GetAll();
        Task<T> GetById(int id);
        void Update(T entity);
        void Delete(int id);
        IQueryable<T> GetByExpression(Expression<Func<T, bool>> expression);
        Task Commit();
    }
}
