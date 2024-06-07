using Microsoft.EntityFrameworkCore;
using MyProApiDiplom.CommonAppData.User;
using MyProApiDiplom.Models;
using MyProApiDiplom.Services;
using Microsoft.AspNetCore.SignalR;

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

// Call to add other services
ConfigureServices(builder.Services);

var app = builder.Build();

// Configure CORS policy
app.UseCors(policy =>
{
    policy.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod();
});

// Call to configure the HTTP request pipeline
Configure(app, app.Environment);

app.Run();

void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddDbContext<IlecContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    services.AddScoped<UsermyClass>();

    // Другие конфигурации
}

void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        app.UseHttpsRedirection();
    }

    app.UseRouting();

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapHub<ChatHub>("/chathub"); // Добавляем маршрут для хаба SignalR
    });
}
