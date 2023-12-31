﻿using Data.Entities;

namespace Logic.Models.DTO.ProductDTO
{
    public class UpdateProductDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int? DiscountPercent { get; set; }
        public int Quantity { get; set; }
        public int SubCategoryId { get; set; }
        public int ProductionCompanyId { get; set; }
    }
}
