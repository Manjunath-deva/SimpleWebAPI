using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SimpleOtisAPI.Domain.Interfaces;
using SimpleOtisAPI.Domain.Models;
using SimpleOtisAPI.Domain.Services;
using SimpleOtisWebAPI.Services;
using System.Linq;
using System.Text;

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

//connect Key Vault thru Managed Identity
var keyVaultEndpoint = new Uri(builder.Configuration.GetConnectionString("KeyVaultUrl"));
var secretClient = new SecretClient(keyVaultEndpoint, new DefaultAzureCredential());
builder.Services.AddSingleton(secretClient);

//connect App configuration thru Managed Identity
var url = builder.Configuration.GetConnectionString("AzureAppConfigUrl");
var token = new DefaultAzureCredential(new DefaultAzureCredentialOptions() { 
            VisualStudioTenantId = "cc032e40-e595-4675-a668-0da63c26c269"
        });


//configure Azure App Configuration in Local
builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(new Uri(url), token);
    options.ConfigureRefresh(refresh =>
    {
        refresh.Register("RefreshValue", refreshAll: true);
    });
});

builder.Services.AddAzureAppConfiguration();

//JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddControllers(options =>
{
    options.OutputFormatters.RemoveType<StringOutputFormatter>();
    options.ReturnHttpNotAcceptable = true;
    options.FormatterMappings.SetMediaTypeMappingForFormat("txt", "text/plain");
}).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    }).AddXmlSerializerFormatters();

//Configure DbConext With SQL Server ConnectionString
builder.Services.AddDbContext<SimpleOtisAPIContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"));//GetValue<string>("ConnectionStrings:SQLServer"));
});

builder.Services.AddScoped<SimpleOtisAPIContext, SimpleOtisAPIContext>();
builder.Services.AddScoped<IWidgetData, WidgetData>();
builder.Services.AddScoped<ILogicalPrograms, LogicalPrograms>();
builder.Services.AddScoped<IUserPreference, UserPreferenceData>();
builder.Services.AddScoped<IDynamicMenu, DynamicMenu>();
builder.Services.AddScoped<IRegister_Login, Register_Login>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
//For In-Memory Cache 
builder.Services.AddMemoryCache();
//For Distributed Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["RedisCacheOptions:Configuration"];
    options.InstanceName = builder.Configuration["RedisCacheOptions:InstanceName"];
});


var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAzureAppConfiguration();
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
