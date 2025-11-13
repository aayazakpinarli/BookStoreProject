using APP.Domain;
using APP.Models;
using APP.Services;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the IoC container.
var connectionString = builder.Configuration.GetConnectionString(nameof(Db));
builder.Services.AddDbContext<DbContext, Db>(options => options.UseSqlite(connectionString));

builder.Services.AddScoped<IService<BookRequest, BookResponse>, BookService>();
builder.Services.AddScoped<IService<AuthorRequest, AuthorResponse>, AuthorService>();
builder.Services.AddScoped<IService<GenreRequest, GenreResponse>, GenreService>();


builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();

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
    pattern: "{controller=Genre}/{action=Index}/{id?}");

app.Run();