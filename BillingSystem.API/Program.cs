
using BillingSystem.Contracts;
using BillingSystem.Db;
using BillingSystem.Services.CouponService;
using BillingSystem.Services.GameItemService;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BillingSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<BillingSystemDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BillingSystemConnection")));

            builder.Services.AddControllers();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IGameItemService, GameItemService>();
            builder.Services.AddScoped<ICouponService, CouponService>();

            builder.Services.AddControllers()
                .AddFluentValidation(validation => validation.RegisterValidatorsFromAssemblyContaining<GameItemValidator>());
            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
