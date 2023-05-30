using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using StajTakip.Business.DependencyResolvers.Autofac;
using StajTakip.Business.Utilities.AutoMapper.Profiles;
using StajTakip.Entities.Concrete;


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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "Admin",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=InternshipDocument}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "pdfRoute",
        pattern: "InternshipDocument/ShowPdf/{id}",
        defaults: new { controller = "InternshipDocument", action = "ShowPdf" });

});

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
