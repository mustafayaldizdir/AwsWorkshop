using AwsWorkshop.Product.Api.Infrastructure.Persistence;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using AwsWorkshop.Product.Api.Core.Application.Abstracts.Repositories;
using AwsWorkshop.Product.Api.Infrastructure.Persistence.Concretes.Services.Repositories;
using AwsWorkshop.Product.Api.Core.Application.Abstracts.UnitOfWorks;
using AwsWorkshop.Product.Api.Infrastructure.Persistence.Concretes.Services.UnitOfWorks;
using AwsWorkshop.Product.Api.Core.LogConfigurations;
using AwsWorkshop.Product.Api.Core.Application.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AwsWorkshop.Product.Api.Infrastructure.Persistence.Concretes.Services;
using AwsWorkshop.Product.Api.Core.Application.Settings;
using AwsWorkshop.Product.Api.Core.Application.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
await builder.Services.AddPersistenceService();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
{
    PropertyNameCaseInsensitive = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    MaxDepth = 10,
    ReferenceHandler = ReferenceHandler.IgnoreCycles,
    WriteIndented = true,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
};

serializerOptions.Converters.Add(new JsonStringEnumConverter());
builder.Services.AddSingleton(s => serializerOptions);

builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient(typeof(IServiceGeneric<,>), typeof(ServiceGeneric<,>));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IProductService, ProductService>();

builder.Services.AddSerilogCustomizationService(builder);

builder.Services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
    {
        sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
{
    var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptionsSettings>();
    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience[0],
        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),
        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}


app.UseSwagger();
app.UseSwaggerUI();

app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());
app.UseSerilogRequestLogging();
//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
