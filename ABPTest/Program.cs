
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Interfaces;
using Repositories.Repositories;
using Services.Filters;
using Services.Interfaces;
using Services.Mapper;
using Services.Services;

namespace ABPTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(options =>
               options.Filters.Add(typeof(NotImplExceptionFilterAttribute))); // Підключив фільтр для помилок

            builder.Services.AddAutoMapper(typeof(AppMappingProfile).Assembly);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Injections

            builder.Services.AddScoped<ITokenExperementsRepository, TokenExperementsRepository>();
            builder.Services.AddScoped<ITokenRepository, TokenRepositiry>();

            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IExperementService, ExperementService>();

            #endregion

            var connection = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ApplicationContext>(options =>
               options.UseSqlServer(connection, b => b.MigrationsAssembly("Repositories"))); // Підключив базу даних і налаштував місце зберігання міграцій

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