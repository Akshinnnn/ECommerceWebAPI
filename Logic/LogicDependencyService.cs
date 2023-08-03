using Data.DAL;
using Logic.Mapper;
using Logic.Repository;
using Logic.Repository.Implementations;
using Logic.Services;
using Logic.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public static class LogicDependencyService
    {
        public static IServiceCollection LogicServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapperProfile));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICategoryService, CategoryService>();

            return services;
        }
    }
}
