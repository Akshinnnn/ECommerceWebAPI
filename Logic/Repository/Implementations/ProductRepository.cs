using Data.DAL;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Repository.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task UpdateRange(Product[] products)
        {
            _context.Products.UpdateRange(products);
            await _context.SaveChangesAsync();
        }
    }
}
