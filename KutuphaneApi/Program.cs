using KutuphaneCore.Entities;
using KutuphaneDataAccess;
using KutuphaneDataAccess.Repository;
using KutuphaneServis.Interfaces;
using KutuphaneServis.MapProfile;
using KutuphaneServis.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;

//log yapýlandýrmasý
Log.Logger = new LoggerConfiguration()
.WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
.CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();



//RateLimiting yapýlandýrmasý
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", options =>
    {
        options.PermitLimit = 5; // 5 istek
        options.Window = TimeSpan.FromSeconds(10); // 10 saniyelik pencere
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2; // Kuyrukta en fazla 2 istek bekleyebilir
    });
});



//jwt yapýandýrmasý
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        RoleClaimType = ClaimTypes.Role,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Kutuphane API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    }); 
});

//builder.Services.AddDbContext<DatabaseConnection>();

builder.Services.AddDbContext<DatabaseConnection>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// AutoMapper config
builder.Services.AddAutoMapper(typeof(MapProfile));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// service implementations
builder.Services.AddScoped<IGenericRepository<Author>, Repository<Author>>();
builder.Services.AddScoped<IGenericRepository<Book>, Repository<Book>>();
builder.Services.AddScoped<IGenericRepository<Category>, Repository<Category>>();
builder.Services.AddScoped<IGenericRepository<User>, Repository<User>>();
builder.Services.AddScoped<IGenericRepository<UploadedFiles>, Repository<UploadedFiles>>();
builder.Services.AddScoped<IGenericRepository<BookLoan>, Repository<BookLoan>>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<IBookLoanService, BookLoanService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();
