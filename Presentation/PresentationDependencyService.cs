using Data.DAL;
using FluentValidation;
using FluentValidation.AspNetCore;
using Logic.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Presentation.Middlewares;
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

            services.AddTransient<GlobalExceptionHandler>();

            services.AddValidatorsFromAssemblyContaining<AddCategoryValidator>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddMvc()
                .AddNewtonsoftJson(options => 
                { 
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; 
                });

            services.AddLogging(lb =>
            {
                lb.ClearProviders();
                lb.AddSerilog();
            });

            return services;
        }
    }
}
