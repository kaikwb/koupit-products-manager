using DotNetEnv;
using koupit_products_manager.Models;
using koupit_products_manager.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

if (builder.Environment.IsDevelopment())
{
    Env.TraversePath().Load();
}

// PostgreSQL database
var dataSourceBuilder = new NpgsqlDataSourceBuilder(
    $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
    $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
    $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
    $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
    $"Password={Environment.GetEnvironmentVariable("DB_PASS")};"
);
dataSourceBuilder.MapEnum<AttributeDataType>();
var dataSource = dataSourceBuilder.Build();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<PostgresDbContext>(options => { options.UseNpgsql(dataSource); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "countries",
    pattern: "{controller=Countries}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "manufacturers",
    pattern: "{controller=Manufacturers}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "attributes",
    pattern: "{controller=Attributes}/{action=Index}/{id?}");

app.Run();