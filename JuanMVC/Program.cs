using JuanMVC;
using JuanMVC.DAL;
using JuanMVC.Email;
using JuanMVC.Models;
using JuanMVC.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<LayoutService>();

builder.Services.AddSignalR();


builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.Password.RequireNonAlphanumeric = false;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<JuanDbContext>();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToAccessDenied = options.Events.OnRedirectToLogin = context =>
    {
        var uri = new Uri(context.RedirectUri);

        if (context.HttpContext.Request.Path.Value.StartsWith("/manage"))
            context.Response.Redirect("/manage/account/login" + uri.Query);
        else
            context.Response.Redirect("/account/login" + uri.Query);

        return Task.CompletedTask;
    };
});


builder.Services.AddDbContext<JuanDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

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

app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapHub<JuanHub>("/hub");



app.Run();
