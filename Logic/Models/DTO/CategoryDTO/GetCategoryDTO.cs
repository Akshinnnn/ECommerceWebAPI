﻿using Data.Entities;

namespace Logic.Models.DTO.CategoryDTO
{
    public class GetCategoryDTO
    {
        public string CategoryName { get; set; }
        public ICollection<GetSubCategoryDTO> SubCategories { get; set; }
    }
}
