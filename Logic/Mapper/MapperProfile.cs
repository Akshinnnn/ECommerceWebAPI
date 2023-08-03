using AutoMapper;
using Data.Entities;
using Logic.Models.DTO.CategoryDTO;
using Logic.Models.DTO.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Category, AddCategoryDTO>().ReverseMap();
            CreateMap<Category, UpdateCategoryDTO>().ReverseMap();
            CreateMap<Category, GetCategoryDTO>().ReverseMap();

            CreateMap<SubCategory, AddSubCategoryDTO>().ReverseMap();
            CreateMap<SubCategory, UpdateSubCategoryDTO>().ReverseMap();
            CreateMap<SubCategory, GetSubCategoryDTO>().ReverseMap();

            CreateMap<Product, AddProductDTO>().ReverseMap();
            CreateMap<Product, UpdateProductDTO>().ReverseMap();
            CreateMap<Product, GetProductDTO>().ReverseMap();
        }
    }
}
