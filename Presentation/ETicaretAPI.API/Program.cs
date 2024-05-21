using ETicaret.Application.Validators;
using ETicaret.Infrastructure.Filters;
using ETicaret.Infrastructure;
using ETicaret.Persistence;
using FluentValidation.AspNetCore;
using ETicaret.Infrastructure.Services.Storage.Local;
using ETicaret.Infrastructure.Services.Storage.Azure;
using MediatR;
using System.Reflection;
using ETicaret.Application;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationService();


builder.Services.AddStorage<LocalStorage>();
//builder.Services.AddStorage<AzureStorage>();


builder.Services.AddCors(options => options.AddDefaultPolicy(
//policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin())); // herkese izin verilir. app.UseCors(); eklenir

policy => policy.WithOrigins("http://localhost:4200", "https://localhost:4200", "http://localhost:58202", "https://localhost:58202", "https://localhost:4300", "http://localhost:4300").AllowAnyHeader().AllowAnyMethod()));



builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>()) //
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true); // Dafault validator filterýný iptal ediyoruz.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin",options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, // oluþturulacak token deðerini kimlerin/ hangi originlerin/ sitelerin kullanacaðýný
                                     // belirlediðimiz deðerdir. => www....com
            ValidateIssuer = true, // oluþturulacak token deðerini kimin daðýttýðýný ifade edeceðimiz alandýr. www.myapi.com
            ValidateLifetime = true,// oluþturulan token deðerinin süresini kontrol edecek olan doðrulamadýr.
            ValidateIssuerSigningKey = true, // üretilecek token deðerinin uygulaamamýza ait bir deðer olduðunu
                                             // ifade eden security key verisinin doðrulamad*sýdýr.

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]))
        };
    });

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles(); // wwwroot kullanabilmek için
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
