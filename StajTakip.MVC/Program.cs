using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Autofac;
using Autofac.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using StajTakip.Business.Abstract;
using StajTakip.Business.BackgroundServices;
using StajTakip.Business.Concrete;

using StajTakip.Business.DependencyResolvers.Autofac;
using StajTakip.Business.Utilities.AutoMapper.Profiles;
using StajTakip.Entities.Concrete;


var builder = WebApplication.CreateBuilder(args);

// Razor Runtime
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new PathString("/Auth/Login");
        options.LogoutPath = new PathString("/Auth/Logout");
        options.Cookie = new CookieBuilder
        {
            Name = "StajTakip",
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            SecurePolicy = CookieSecurePolicy.SameAsRequest
        };
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(2);
        options.AccessDeniedPath = new PathString("/Auth/AccessDenied");
        options.Events.OnRedirectToLogin = async context =>
        {
            if (context.Request.Path.StartsWithSegments(new PathString("/Admin")))
            {
                context.Response.Redirect("/Admin/Auth/Login");
            }
            else
            {
                context.Response.Redirect(context.RedirectUri);
            }
        };
    });


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

builder.Services.AddSingleton(sp => new ConnectionFactory()
{
    HostName = builder.Configuration["RabbitMQOptions:Host"],
    Port = int.Parse(builder.Configuration["RabbitMQOptions:Port"]),
    UserName = builder.Configuration["RabbitMQOptions:UserName"],
    Password = builder.Configuration["RabbitMQOptions:Password"],
    DispatchConsumersAsync = true
});

builder.Services.AddSingleton<RabbitMQClientService>();
builder.Services.AddSingleton<RabbitMQPublisher>();
builder.Services.AddHostedService<MailSenderBackgroundService>();
builder.Services.AddHostedService<MessageSenderBackgroundService>();

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

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

//app.UseStatusCodePagesWithReExecute("/Auth/NotFound/", "?code={0}");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseNotyf();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "Admin",
      pattern: "{area:exists}/{controller=Message}/{action=Inbox}/{id?}"
    );

    endpoints.MapControllerRoute(
        name: "Student",
        pattern: "{controller=Message}/{action=Inbox}/{id?}");

    endpoints.MapControllerRoute(
        name: "pdfRoute",
        pattern: "InternshipDocument/ShowPdf/{id}",
        defaults: new { controller = "InternshipDocument", action = "ShowPdf" });


    app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

    endpoints.MapFallback(context =>
    {
        context.Response.Redirect("/Auth/NotFoundPage");
        return Task.CompletedTask;
    });

});

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
