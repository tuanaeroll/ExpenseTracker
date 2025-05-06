using ExpenseTrackerBank.WebApi.Interfaces;
using ExpenseTrackerBank.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI servis kaydı
builder.Services.AddScoped<IPaymentService, PaymentService>();

var app = builder.Build();

// Swagger middleware'leri
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
