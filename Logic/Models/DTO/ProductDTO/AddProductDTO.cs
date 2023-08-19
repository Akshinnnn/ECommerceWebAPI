using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models.DTO.ProductDTO
{
    public class AddProductDTO
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int? DiscountPercent { get; set; }
        public int Quantity { get; set; }
        public int SubCategoryId { get; set; }
        public int ProductionCompanyId { get; set; }
    }
}
