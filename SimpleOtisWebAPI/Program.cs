using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SimpleOtisAPI.Domain.Interfaces;
using SimpleOtisAPI.Domain.Models;
using SimpleOtisAPI.Domain.Services;
using SimpleOtisWebAPI.Services;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

//Registering Swagger API Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("V1", new OpenApiInfo
    {
        Title = "SimpleOtisAPI",
        Version = "v1",
        Description = "Simple Otis API for Learning",
        TermsOfService = new Uri("https://dotnettutorials.net"),
        Contact = new OpenApiContact
        {
            Name = "Test",
            Email = "Supporttest@gmail.com",
            Url = new Uri("https://nothing.com")
        },
        License = new OpenApiLicense
        {
            Name = "Test License",
            Url = new Uri("https://license.com")
        }
    });
});

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
    options.FormatterMappings.SetMediaTypeMappingForFormat("txt", "text/plain");
}).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    }).AddXmlSerializerFormatters();

//Configure DbConext With SQL Server ConnectionString
builder.Services.AddDbContext<SimpleOtisAPIContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"));
});

builder.Services.AddScoped<SimpleOtisAPIContext, SimpleOtisAPIContext>();
builder.Services.AddScoped<IWidgetData, WidgetData>();
builder.Services.AddScoped<ILogicalPrograms, LogicalPrograms>();
builder.Services.AddScoped<IUserPreference, UserPreferenceData>();
builder.Services.AddScoped<IDynamicMenu, DynamicMenu>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
//For In-Memory Cache 
builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller}/{action}/{Id?}");

//Registering Swagger Middleware Components
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/V1/swagger.json", "My SimpleOtisAPI V1");
    });
}

app.UseMiddleware<CustomNotAcceptableMiddleware>();

app.Run();
