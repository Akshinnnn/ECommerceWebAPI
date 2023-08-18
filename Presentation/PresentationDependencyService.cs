using Data.DAL;
using FluentValidation;
using FluentValidation.AspNetCore;
using Logic.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

namespace Presentation
{
    public static class PresentationDependencyService
    {
        public static IServiceCollection PresentationServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddValidatorsFromAssemblyContaining<AddCategoryValidator>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            
            return services;
        }
    }
}
