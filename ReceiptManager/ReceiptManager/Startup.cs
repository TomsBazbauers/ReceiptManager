using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ReceiptManager.Core.Services;
using ReceiptManager.Core.Validations;
using ReceiptManager.Data;
using ReceiptManager.RequestHandlers;
using ReceiptManager.Services;

namespace ReceiptManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ReceiptManager", Version = "v1" });
            });

            services.AddDbContext<ReceiptManagerDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("receipt-manager"));
            });

            services.AddScoped<IReceiptManagerDbContext, ReceiptManagerDbContext>();
            services.AddScoped<IDbService, DbService>();

            services.AddScoped<IReceiptService, ReceiptService>();

            services.AddScoped<IItemValidator, ItemNameValidator>();
            services.AddScoped<IItemValidator, ItemCountValidator>();
            services.AddScoped<IRequestValidator, RequestValidator>();

            services.AddSingleton<IMapper>(AutoMapperConfig.CreateMapper());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReceiptManager v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
