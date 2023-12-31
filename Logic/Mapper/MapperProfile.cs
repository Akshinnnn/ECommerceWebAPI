﻿using AutoMapper;
using Data.Entities;
using Logic.Models.DTO.CategoryDTO;
using Logic.Models.DTO.SubCategoryDTO;
using Logic.Models.DTO.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Models.DTO.ProductInfoDTO;
using Logic.Models.DTO.UserDTO;
using Microsoft.AspNetCore.Identity;
using Logic.Models.DTO.RoleDTO;
using Logic.Models.DTO.ProductionCompanyDTO;
using Logic.Models.DTO.BasketDTO;
using Logic.Models.DTO.OrderDTO;

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

            CreateMap<ProductInformation, AddProductInfoDTO>().ReverseMap();
            CreateMap<ProductInformation, UpdateProductInfoDTO>().ReverseMap();
            CreateMap<ProductInformation, GetProductInfoDTO>().ReverseMap();

            CreateMap<ProductionCompany, AddCompanyDTO>().ReverseMap();
            CreateMap<ProductionCompany, UpdateCompanyDTO>().ReverseMap();
            CreateMap<ProductionCompany, GetCompanyDTO>().ReverseMap();

            CreateMap<User, RegisterUserDTO>().ReverseMap();
            CreateMap<User, LoginUserDTO>().ReverseMap();
            CreateMap<User, UpdateUserDTO>().ReverseMap();
            CreateMap<User, GetUserDTO>().ReverseMap();

            CreateMap<AddRoleDTO, IdentityRole>().ReverseMap();
            CreateMap<GetRolesDTO, IdentityRole>().ReverseMap();

            CreateMap<AddBasketDTO, Basket>().ReverseMap();
            CreateMap<GetBasketDTO, Basket>().ReverseMap();

            CreateMap<GetOrderDTO, Order>().ReverseMap();
            CreateMap<GetOrderProductDTO, OrderProduct>().ReverseMap();
        }
    }
}
