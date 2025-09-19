using BukyBookWeb.Data;
using BukyBookWeb.IRepository;
using BukyBookWeb.IService;
using BukyBookWeb.Logging;
using BukyBookWeb.Models;
using BukyBookWeb.Repositories;
using BukyBookWeb.Repository;
using BukyBookWeb.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

SerilogConfig.ConfigureSerilog(builder);


builder.Services.AddControllersWithViews()
    .AddViewLocalization()           
    .AddDataAnnotationsLocalization(); 

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources"); 

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefultConnection")
    ));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; 
    options.AccessDeniedPath = "/Account/AccessDenied"; 
});



builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService,CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped<IAccountService,AccountService>();
builder.Services.AddScoped<IAccountRepository,AccountRepository>();
builder.Services.AddScoped<IAdminService,AdminService>();
builder.Services.AddScoped<IAdminRepository,AdminRepository>();
builder.Services.AddScoped<ICalculatorRepository, CalculatorReposity>();
builder.Services.AddScoped<ICalculatorService,CalculatorService>();

var app = builder.Build();



var supportedCultures = new[] { "en-US", "bn-BD" };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList()
};

app.UseRequestLocalization(localizationOptions);


app.UseMiddleware<ErrorHandlingMiddleware>();

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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context1 = services.GetRequiredService<ApplicationDbContext>();
  
    context1.Database.EnsureCreated();
    await SeedData.Initialize(services);
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
