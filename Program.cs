using Scalar.AspNetCore;
using AdminService.Clients;
using AdminService.Repositories;
using AdminService.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddOpenApi();

// MongoDB
builder.Services.AddSingleton<IMongoClient>(
    new MongoClient(builder.Configuration["MongoDB:ConnectionString"]));

// Repository
builder.Services.AddScoped<IAdminRepository, AdminRepository>();

// Service
builder.Services.AddScoped<AdminService.Services.AdminService>();

// Client
builder.Services.AddHttpClient<IClassServiceClient, ClassServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ClassService:BaseUrl"]!);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
