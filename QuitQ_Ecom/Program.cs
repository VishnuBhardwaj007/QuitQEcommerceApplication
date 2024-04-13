using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using QuitQ_Ecom.Models;
using QuitQ_Ecom.Repository;
using System;
using System.Text;
using Serilog;
using Microsoft.OpenApi.Models;

namespace QuitQ_Ecom
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            builder.Services.AddDbContext<QuitQEcomContext>();
            builder.Services.AddTransient<ICategory, CategoryRepositoryImpl>();
            builder.Services.AddTransient<IBrand, BrandRepositoryImpl>();
            builder.Services.AddScoped<IProduct, ProductRepositoryImpl>();
            builder.Services.AddScoped<IUser,UserRepositoryImpl>();
            builder.Services.AddTransient<IOrder, OrderRepositoryImpl>();
            builder.Services.AddTransient<IPayment,PaymentRepositoryImpl>();
            builder.Services.AddTransient<IOrderItem, OrderItemRepositoryImpl>();
            builder.Services.AddTransient<IUserAddress, UserAddressRepositoryImpl>();
            builder.Services.AddTransient<IShipper,ShipperRepositoryImpl>();



            
            builder.Services.AddTransient<IWishlist, WishlistRepositoryImpl>();
            builder.Services.AddTransient<IState, StateRepositoryImpl>();
            builder.Services.AddTransient<ICity, CityRepositoryImpl>();
            builder.Services.AddScoped<ICart, CartRepositoryImpl>();





            builder.Services.AddTransient<IStore, StoreRepositoryImpl>();
            builder.Services.AddScoped<ISubCategory, SubCategoryRepositoryImpl>();
            builder.Services.AddScoped<IGender, GenderRepositoryImpl>();
            builder.Services.AddScoped<IImage, ImageRepository>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c=>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "jwtToken_Auth_API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Here Enter JWT Token with bearer format like bearer[space] token"
                });
                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference =  new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            

            Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Information()
             .WriteTo.Console()
             .WriteTo.File("logs/Loggr-.txt", rollingInterval: RollingInterval.Day)
             .CreateLogger();


            builder.Services.AddCors(
                option =>
                {
                    option.AddDefaultPolicy(builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
                }
                );

            builder.Host.UseSerilog();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthentication();

            
            app.UseAuthorization();
            

            app.MapControllers();

            app.Run();
        }
    }
}
