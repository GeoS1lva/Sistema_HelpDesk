using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using Sistema_HelpDesk.Desk.Application.Contracts.Repositories;
using Sistema_HelpDesk.Desk.Application.Contracts.Security;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Autenticação;
using Sistema_HelpDesk.Desk.Application.UseCases.Category;
using Sistema_HelpDesk.Desk.Application.UseCases.Category.Interface;
using Sistema_HelpDesk.Desk.Application.UseCases.Companies;
using Sistema_HelpDesk.Desk.Application.UseCases.Companies.Interface;
using Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks;
using Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.Interface;
using Sistema_HelpDesk.Desk.Application.UseCases.Tickets;
using Sistema_HelpDesk.Desk.Application.UseCases.Tickets.Interface;
using Sistema_HelpDesk.Desk.Application.UseCases.Users;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.Interface;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.ResetPasswords;
using Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies;
using Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.Interface;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;
using Sistema_HelpDesk.Desk.Infra.Autenticação;
using Sistema_HelpDesk.Desk.Infra.Context;
using Sistema_HelpDesk.Desk.Infra.Email;
using Sistema_HelpDesk.Desk.Infra.Identity;
using Sistema_HelpDesk.Desk.Infra.JSONSerializacao;
using Sistema_HelpDesk.Desk.Infra.Persistence.UnitOfWork;
using Sistema_HelpDesk.Desk.Infra.SingaLR;
using System.Text;
using System.Text.Json.Serialization;

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
builder.Services.Configure<EmailConfiguracao>(builder.Configuration.GetSection("EmailConfiguracao"));
builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddScoped<IResetarSenhaUseCase, ResetarSenhaUseCase>();
builder.Services.AddScoped<IAutheUseCase, AutheUseCase>();

builder.Services.AddScoped<ICriarUsuarioSistemaUseCase, CriarUsuarioSistemaUseCase>();
builder.Services.AddScoped<IRemoverUsuarioSistemaUseCase, RemoverUsuarioSistemaUseCase>();
builder.Services.AddScoped<IRetornarInformacoesUseCase, RetornarInformacoesUseCase>();
builder.Services.AddScoped<IAtualizarInformacoesUsuarioSistemaUseCase, AtualizarInformacoesUsuarioSistemaUseCase>();

builder.Services.AddScoped<ICrirEmpresaUseCase, CrirEmpresaUseCase>();
builder.Services.AddScoped<IAtualizarEmpresaUseCase, AtualizarEmpresaUseCase>();
builder.Services.AddScoped<IRetornarInformacoesEmpresaUseCase, RetornarInformacoesEmpresaUseCase>();
builder.Services.AddScoped<IRemoverAtivarEmpresaUseCase, RemoverAtivarEmpresaUseCase>();

builder.Services.AddScoped<ICriarUsuarioEmpresaUseCase, CriarUsuarioEmpresaUseCase>();
builder.Services.AddScoped<IRetornarInformacoesUsuarioEmpresaUseCase, RetornarInformacoesUsuarioEmpresaUseCase>();
builder.Services.AddScoped<IRemoverAtivarUsuarioEmpresaUseCase, RemoverAtivarUsuarioEmpresaUseCase>();
builder.Services.AddScoped<IAtualizarInformacoesUsuarioEmpresaUseCase, AtualizarInformacoesUsuarioEmpresaUseCase>();

builder.Services.AddScoped<ICriarMesaAtendimentoUseCase, CriarMesaAtendimentoUseCase>();
builder.Services.AddScoped<IRemoverMesaAtendimentoUseCase, RemoverMesaAtendimentoUseCase>();
builder.Services.AddScoped<IRetornarInformacoesMesasAtendimentoUseCase, RetornarInformacoesMesasAtendimentoUseCase>();

builder.Services.AddScoped<ICriarCategoriaUseCase, CriarCategoriaUseCase>();
builder.Services.AddScoped<IDeletarAtivarCategoriaUseCase, DeletarAtivarCategoriaUseCase>();
builder.Services.AddScoped<IRetornarInformacoesCategoriaUseCase, RetornarInformacoesCategoriaUseCase>();

builder.Services.AddScoped<ICriarChamadoUseCase, CriarChamadoUseCase>();
builder.Services.AddScoped<IAcoesChamadoUseCase, AcoesChamadoUseCase>();
builder.Services.AddScoped<IRetornarInformacoesChamadosUseCase, RetornarInformacoesChamadosUseCase>();
builder.Services.AddScoped<IAtualizarInformacoesChamadoUseCase, AtualizarInformacoesChamadosUseCase>();

builder.Services.AddScoped<IRolesRepositorys, RolesRepositorys>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey(nameof(AtualizarSlaJob));

    q.AddJob<AtualizarSlaJob>(options => options
        .WithIdentity(jobKey));

q.AddTrigger(options => options
    .ForJob(jobKey)
    .WithIdentity($"{nameof(AtualizarSlaJob)}--trigger")
    .StartNow()
    .WithSimpleSchedule(x => x
        .WithInterval(TimeSpan.FromMinutes(3))
        .RepeatForever())
    );
});

builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontEnd", policy =>
    {
        policy
            .WithOrigins(
            "https://lashaun-unbumped-squarely.ngrok-free.dev/",
            "http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddSignalR();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureHttpsDefaults(httpsOptions =>
    {
        httpsOptions.AllowAnyClientCertificate();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.Converters.Add(new JsonDateTimeOffsetConverter());
        options.JsonSerializerOptions.Converters.Add(new JsonTimeSpanConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TechDesk",
        Version = "v1",
        Description = "APIs para o Sistema de HelpDesk - TechDesk"
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seedService = scope.ServiceProvider.GetRequiredService<IRolesRepositorys>();
    await seedService.InicializarRolesAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PermitirFrontEnd");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<EnviarMensagemHub>("/chatHub");

app.MapControllers();

app.Run();
