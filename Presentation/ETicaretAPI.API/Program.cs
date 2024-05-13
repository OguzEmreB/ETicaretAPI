using ETicaret.Application.Validators;
using ETicaretAPI.Infrastructure.Filters;
using ETicaretAPI.Infrastructure;
using ETicaretAPI.Persistence;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddCors(options => options.AddDefaultPolicy(
//policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin())); // herkese izin verilir. app.UseCors(); eklenir

policy => policy.WithOrigins("http://localhost:4200", "https://localhost:4200", "http://localhost:58202", "https://localhost:58202").AllowAnyHeader().AllowAnyMethod()));



builder.Services.AddControllers(options=>options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>()) //
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true); // Dafault validator filterýný iptal ediyoruz.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
