
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.ServiceContracts;
using ShoppingCart.Services;
using ShoppingCartApplication.Mappers;

namespace ShoppingCart
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            

            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ShoppingCartDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ShoppingCartConnectionString")));

            builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
            
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IOrderService,OrderService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            var app = builder.Build();

            
            
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
