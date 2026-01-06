using Entities.Models;
using HumanResourceAPI.Infrastrcuture;
using HumanResourceAPI.Infrastrcuture.Repository;
using LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository;
using System.Reflection;

namespace HumanResourceAPI.Extenstions
{
    public static class ServiceExtensions
    {
        // Extension methods for IServiceCollection can be added here
        public static void ConfigureCustomServices(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

        public static void IISIntegrationConfig(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {
                // Configure IIS options here if needed
            });

        public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
        {
            //services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IOrderService, OrderService>();
            return services;
        }

        public static void ConfigureRepository(this IServiceCollection services) =>
            services.AddTransient(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>))
            .AddTransient<IRepositoryManager, RepositoryManager>();


        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddScoped<ILoggerManager, LoggerManager>();

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<Entities.AppDbContext>(opts =>
                opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"), b =>
                    b.MigrationsAssembly("HumanResourceAPI")));

        public static void AddAutoMapper(this IServiceCollection services) =>
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Human Resource API",
                    Version = "v1"
                });
                s.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Human Resource API",
                    Version = "v2"
                });
            });
        }

        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                opt.ApiVersionReader = new Microsoft.AspNetCore.Mvc.Versioning.HeaderApiVersionReader("api-version");
                //opt.Conventions.Controller<HumanResourceAPI.Controllers.CompaniesController>()
                //    .HasApiVersion(new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0));
                //opt.Conventions.Controller<HumanResourceAPI.Controllers.CompaniesV2Controller>()
                //    .HasApiVersion(new Microsoft.AspNetCore.Mvc.ApiVersion(2, 0));
            });
        }


        // Configure Identity password and user settings
        public static void CongfigureIdentity(this IServiceCollection services)
        {
            var buidler = services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 10;
                options.User.RequireUniqueEmail = true;
            });
            buidler = new IdentityBuilder(buidler.UserType,
                typeof(IdentityRole), buidler.Services);
            buidler.AddEntityFrameworkStores<Entities.AppDbContext>().AddDefaultTokenProviders();
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            // JWT configuration can be added here
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetSection("secretKey").Value;
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }
    }
}
