using Data.DAL;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Repository.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddRange(OrderProduct[] orderProducts)
        {
            await _context.OrderProducts.AddRangeAsync(orderProducts);
            await _context.SaveChangesAsync();
        }
    }
}
