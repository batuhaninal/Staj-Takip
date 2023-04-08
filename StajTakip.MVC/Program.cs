using Microsoft.EntityFrameworkCore;
using StajTakip.Business.Abstract;
using StajTakip.Business.Concrete;
using StajTakip.DataAccess.Abstract;
using StajTakip.DataAccess.Concrete.Contexts;
using StajTakip.DataAccess.Concrete.EntityFramework.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<StajTakipContext>();
builder.Services.AddScoped<ITempRepository, TempRepository>();
builder.Services.AddScoped<ITempRepositoryAsync, TempRepositoryAsync>();
builder.Services.AddScoped<ITempService, TempService>();


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

app.Run();
