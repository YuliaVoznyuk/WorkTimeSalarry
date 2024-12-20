using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WorkTimeSalary.Application.Factories;
using WorkTimeSalary.Application.Interfaces;
using WorkTimeSalary.Application.Services;
using WorkTimeSalary.Domain.Entities;
using WorkTimeSalary.Infrastructure.DbContext;
using WorkTimeSalary.UI.Middlewares;

namespace WorkTimeSalary
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (environment == "Development")
            {
                builder.Services.AddDbContext<WorkTimeSalaryDbContext>(options =>
                    options.UseSqlServer(connectionString)
                           .EnableSensitiveDataLogging());
            }
            else
            {
                builder.Services.AddDbContext<WorkTimeSalaryDbContext>(options =>
                    options.UseSqlServer(connectionString));
            }

            builder.Services.AddIdentity<Employee, IdentityRole<int>>(option =>
            {

            }).AddEntityFrameworkStores<WorkTimeSalaryDbContext>()
               .AddDefaultTokenProviders();



            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            builder.Services.AddAutoMapper(typeof(Application.Mapper.DepartmentProfile).Assembly);

            var serviceTypes = typeof(Application.Services.DepartmentService).Assembly.GetTypes()
                        .Where(type => type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IService<>)) && !type.IsAbstract)
                        .ToList();

            builder.Services.AddScoped<IServiceFactory, ServiceFactory>();
            
            foreach (var serviceType in serviceTypes)
            {
                var typeInterface = serviceType.GetInterfaces()[0];
                builder.Services.AddScoped(typeInterface, serviceType);
            }
            builder.Services.AddScoped<IWorkPositionService, WorkPositionService>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMiddleware<ExeptionMiddleware>();
            app.UseRouting();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<WorkTimeSalaryDbContext>();

                dbContext.Database.EnsureCreated();
            }

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
