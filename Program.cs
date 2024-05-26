using Microsoft.EntityFrameworkCore;
using MyProApiDiplom.CommonAppData.User;
using MyProApiDiplom.Models;
using MyProApiDiplom.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR(); // Добавляем SignalR
builder.Services.AddDbContext<IlecContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// Register IMessageService and its implementation MessageService
builder.Services.AddScoped<IMessageService, MessageService>();

// Register UsermyClass
builder.Services.AddScoped<UsermyClass>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure CORS policy
app.UseCors(policy =>
{
    policy.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod();
});

// Configure the HTTP request pipeline.
app.UseRouting(); // Добавляем вызов UseRouting()

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chathub"); // Добавляем маршрут для хаба SignalR
    endpoints.MapControllers();
});

app.Run();
