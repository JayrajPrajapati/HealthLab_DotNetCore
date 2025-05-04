using HealthLayby.Admin;
using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Models.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/";
                });

AppSetting.ClientEmail = builder.Configuration["EmailSettings:ClientEmail"] ?? string.Empty;
AppSetting.ClientEmailPassword = builder.Configuration["EmailSettings:ClientEmailPassword"] ?? string.Empty;
AppSetting.IsSSLEnabled = Convert.ToBoolean(builder.Configuration["EmailSettings:IsSSLEnabled"]);

builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext")));

// Add Log4Net for logging error
builder.Logging.AddLog4Net(new Log4NetProviderOptions("log4net.config", true));


builder.Services.RegisterAllServices();

builder.Services.AddStripeInfrastructure(builder.Configuration);

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();
app.ConfigRoute();

app.Run();


