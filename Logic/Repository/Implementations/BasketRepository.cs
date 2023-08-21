using Data.DAL;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Repository.Implementations
{
    public class BasketRepository : IBasketRepository
    {
        private readonly AppDbContext _context;
        public BasketRepository(AppDbContext context)
        {
            _context = context;
        }

        public void ClearBasket(Expression<Func<Basket, bool>> expression)
        {
            var baskets = _context.Baskets.Where(expression);
            _context.Baskets.RemoveRange(baskets.ToArray());
        }
    }
}
