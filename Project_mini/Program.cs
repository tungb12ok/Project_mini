using System.Text;
using Microsoft.EntityFrameworkCore;
using Project_mini.Models;
using Project_mini.Repositories;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Project_mini.Helpers;
using Project_mini.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure database connection
builder.Services.AddDbContext<MiniProjectContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Configure JWT Bearer Authentication
builder.Services.AddAuthentication(options =>
{
    // Setting default authentication and challenge schemes to JWT Bearer
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Configure JWT Bearer validation
    options.TokenValidationParameters = new TokenValidationParameters
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

// Swagger Configuration with JWT Bearer Support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Project Mini", Version = "v1" });

    // Add JWT Bearer token support in Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\n\nExample: 'Bearer 12345abcdef'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Register application services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDepartmentServices, DepartmentServices>();
builder.Services.AddScoped<IJwtHelper, JwtHelper>();


// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });

    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("https://localhost:7150") 
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();

// Configure distributed memory cache and session management
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");

app.UseSession();
app.UseHttpsRedirection();

app.UseAuthentication();  // Ensure authentication middleware is added before authorization
app.UseAuthorization();   // Ensure authorization middleware is added after authentication

app.MapControllers();

app.Run();
