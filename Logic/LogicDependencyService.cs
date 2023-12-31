﻿using Data.DAL;
using FluentValidation;
using Logic.Mapper;
using Logic.Models.EmailConfigurationModel;
using Logic.Repository;
using Logic.Repository.Implementations;
using Logic.Services;
using Logic.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public static class LogicDependencyService
    {
        public static IServiceCollection LogicServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddAutoMapper(typeof(MapperProfile));

            //Taking values from appsettings 
            services.Configure<EmailConfigModel>(config.GetSection("EmailConfiguration"));
            
            
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISubCategoryService, SubCategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductInfoService, ProductInfoService>();
            services.AddScoped<IProductionCompanyService, ProductionCompanyService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IOrderService, OrderService>();

            //JWT configuration:
            //services.AddAuthentication(a =>
            //{
            //    a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(op =>
            //{
            //    op.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        SaveSigninToken = true,
            //        ClockSkew = TimeSpan.Zero,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"])),
            //        ValidAudience = config["JWT:Audience"],
            //        ValidIssuer = config["JWT:Issuer"]
            //    };
            //    op.Events = new JwtBearerEvents
            //    {
            //        /*
            //         * When validation fails this function will return completedTask
            //         * if token is expired then it will add information about expiration to the header of the response
            //         */
            //        OnAuthenticationFailed = context =>
            //        {
            //            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            //            {
            //                context.Response.Headers.Add("TOKEN-IS-EXPIRED", "true");
            //            }

            //            return Task.CompletedTask;
            //        }
            //    };
            //});

            return services;
        }
    }
}
