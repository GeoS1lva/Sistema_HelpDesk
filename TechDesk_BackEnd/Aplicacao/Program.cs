using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sistema_HelpDesk.Desk.Application.Autenticação.Interface;
using Sistema_HelpDesk.Desk.Application.Autenticação.Service;
using Sistema_HelpDesk.Desk.Application.Users.Interface;
using Sistema_HelpDesk.Desk.Application.Users.Service.RedefinirSenha;
using Sistema_HelpDesk.Desk.Domain.Emaill;
using Sistema_HelpDesk.Desk.Domain.Users;
using Sistema_HelpDesk.Desk.Domain.Users.Interface;
using Sistema_HelpDesk.Desk.Infra.Autenticação;
using Sistema_HelpDesk.Desk.Infra.Context;
using Sistema_HelpDesk.Desk.Infra.Email;
using Sistema_HelpDesk.Desk.Infra.Users;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<SqlServerIdentityDbContext>();
builder.Services.AddDbContext<SqlServerIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoSql")));

builder.Services
    .AddIdentityCore<UserLogin>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequiredUniqueChars = 1;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;

        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
        options.Lockout.MaxFailedAccessAttempts = 3;

        options.SignIn.RequireConfirmedAccount = true;
    })
    .AddRoles<UserRole>()
    .AddEntityFrameworkStores<SqlServerIdentityDbContext>()
    .AddSignInManager<SignInManager<UserLogin>>()
    .AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
    opt.TokenLifespan=TimeSpan.FromHours(2));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,

            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)),
            ClockSkew = TimeSpan.Zero,
            NameClaimType = System.Security.Claims.ClaimTypes.Name,
            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                if (ctx.Request.Cookies.TryGetValue("jwt", out var token))
                    ctx.Token = token;
                return Task.CompletedTask;
            }
        };
    });

builder.Services
    .AddAuthorization(options =>
    {
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    });

builder.Services.AddScoped<IJwtGerador, JwtGerador>();
builder.Services.AddScoped<IAutheService, AutheService>();
builder.Services.AddScoped<IRolesInfra, RolesInfra>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration.GetSection("AuthMessageSenderOptions"));
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IResetarSenhaService, ResetarSenhaService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TechDesk",
        Version = "v1",
        Description = "APIs para o Sistema de HelpDesk"
    });
});

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var seedService = scope.ServiceProvider.GetRequiredService<IRolesInfra>();
    await seedService.InicializarRolesAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
