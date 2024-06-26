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
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true); // Dafault validator filterını iptal ediyoruz.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, // oluşturulacak token değerini kimlerin/ hangi originlerin/ sitelerin kullanacağını
                                     // belirlediğimiz değerdir. => www....com
            ValidateIssuer = true, // oluşturulacak token değerini kimin dağıttığını ifade edeceğimiz alandır. www.myapi.com
            ValidateLifetime = true,// oluşturulan token değerinin süresini kontrol edecek olan doğrulamadır.
            ValidateIssuerSigningKey = true, // üretilecek token değerinin uygulaamamıza ait bir değer olduğunu
                                             // ifade eden security key verisinin doğrulamad*sıdır.

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow :false
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
