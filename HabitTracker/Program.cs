using System.Security.Claims;
using HabitTracker.Data;
using HabitTracker.Endpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "HabitTracker", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
            Array.Empty<string>()
        }
    });
});

// In Program.cs, before adding JWT bearer
var jwtKey = builder.Configuration["JwtKey"];
Console.WriteLine($"JWT Key configured: {!string.IsNullOrEmpty(jwtKey)}");
Console.WriteLine($"JWT Key length: {jwtKey?.Length ?? 0}");

builder.Services.AddDbContext<HabitContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Auth services
builder.Services.AddIdentityCore<IdentityUser>(options => 
    {
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<HabitContext>();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        // Some testing
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                Console.WriteLine($"Token received: {context.Token?[..50]}...");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validated successfully");
                var userId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine($"User ID from token: {userId}");
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                Console.WriteLine($"Exception type: {context.Exception.GetType().Name}");
                if (context.Exception.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {context.Exception.InnerException.Message}");
                }
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine($"Challenge triggered - Error: {context.Error}");
                Console.WriteLine($"Error description: {context.ErrorDescription}");
                return Task.CompletedTask;
            }
        };
        
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Token;
                Console.WriteLine($"Received token: {token}");
                Console.WriteLine($"Token length: {token?.Length}");
            
                // Check if token has proper JWT structure (header.payload.signature)
                var parts = token?.Split('.');
                Console.WriteLine($"Token parts count: {parts?.Length}");
                if (parts?.Length == 3)
                {
                    Console.WriteLine($"Header length: {parts[0].Length}");
                    Console.WriteLine($"Payload length: {parts[1].Length}");
                    Console.WriteLine($"Signature length: {parts[2].Length}");
                }
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                Console.WriteLine($"Full exception: {context.Exception}");
                return Task.CompletedTask;
            }
        };
        // Test over
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"]!)
            )
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.MapHabitsEndpoints();
app.MapUserEndpoints();

app.Run();