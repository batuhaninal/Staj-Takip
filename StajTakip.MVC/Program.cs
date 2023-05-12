using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using StajTakip.Business.Abstract;
using StajTakip.Business.Concrete;
using StajTakip.Business.DependencyResolvers.Autofac;
using StajTakip.Business.Utilities.AutoMapper.Profiles;
using StajTakip.DataAccess.Abstract;
using StajTakip.DataAccess.Concrete.Contexts;
using StajTakip.DataAccess.Concrete.EntityFramework.Repositories;
using StajTakip.Entities.Concrete;
using StajTakip.MVC.ViewComponents.Student;


var builder = WebApplication.CreateBuilder(args);

// Razor Runtime
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

// Add services to the container.
builder.Services.AddControllersWithViews();


//builder.Services.AddScoped<ITempRepository, TempRepository>();
//builder.Services.AddScoped<ITempRepositoryAsync, TempRepositoryAsync>();
//builder.Services.AddScoped<ITempService, TempService>();

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

builder.Services.AddAutoMapper(typeof(InternshipsBookProfile));

builder.Services.AddNotyf(config =>
{
    config.Position = NotyfPosition.BottomRight;
    config.DurationInSeconds = 10;
    config.IsDismissable = true;
});

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterModule(new AutofacBusinessModule());
    });

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

app.UseNotyf();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
