using Entities;
using Entities.Dto;
using HumanResourceAPI.Extenstions;
using HumanResourceAPI.Infrastrcuture;
using HumanResourceAPI.Utility;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Repository.DataShaping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// NLog: Setup NLog for Dependency injection
IConfiguration configuration = builder.Configuration;
LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Custom extension methods for configuring services
builder.Services.ConfigureCustomServices();
builder.Services.IISIntegrationConfig();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureSqlContext(configuration);
builder.Services.ConfigureRepository();
builder.Services.AddAutoMapper();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureVersioning();

// Configure Identity
builder.Services.AddAuthentication();
builder.Services.CongfigureIdentity();
builder.Services.ConfigureJWT(configuration);
builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>();
builder.Services.AddScoped<IDataShaper<CompanyDto>, DataShaper<CompanyDto>>();
builder.Services.AddScoped<CompanyLinks>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true; // Add this line to respect Accept header
    config.ReturnHttpNotAcceptable = true; // Return 406 if not acceptable
})
.AddNewtonsoftJson()
.AddXmlDataContractSerializerFormatters();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "Human Resource API v1");
    s.SwaggerEndpoint("/swagger/v2/swagger.json", "Human Resource API v2");
});

app.UseHttpsRedirection();

app.UseStaticFiles(); // Added to serve static files from wwwroot
app.UseCors("CorsPolicy");

// To handle reverse proxy headers (like X-Forwarded-For)
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All
});
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
app.MigrateDatabase().Run();