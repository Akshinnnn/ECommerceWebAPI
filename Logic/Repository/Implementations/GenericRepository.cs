using Data.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Repository.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        public DbSet<T> Table => _context.Set<T>();

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Add(T entity)
        {
            await Table.AddAsync(entity);
        }

        public void Delete(int id)
        {
            var entity = Table.Find(id);
            Table.Remove(entity!);
        }

        public IQueryable<T> GetAll()
        {
            return Table;
        }

        public IQueryable<T> GetByExpression(Expression<Func<T, bool>> expression)
        {
            return Table.Where(expression);
        }

        public async Task<T> GetById(int id)
        {
            return await Table.FindAsync(id);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
