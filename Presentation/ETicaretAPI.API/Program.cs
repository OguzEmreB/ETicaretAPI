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
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true); // Dafault validator filter�n� iptal ediyoruz.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin",options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, // olu�turulacak token de�erini kimlerin/ hangi originlerin/ sitelerin kullanaca��n�
                                     // belirledi�imiz de�erdir. => www....com
            ValidateIssuer = true, // olu�turulacak token de�erini kimin da��tt���n� ifade edece�imiz aland�r. www.myapi.com
            ValidateLifetime = true,// olu�turulan token de�erinin s�resini kontrol edecek olan do�rulamad�r.
            ValidateIssuerSigningKey = true, // �retilecek token de�erinin uygulaamam�za ait bir de�er oldu�unu
                                             // ifade eden security key verisinin do�rulamad*s�d�r.

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
app.UseStaticFiles(); // wwwroot kullanabilmek i�in
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
