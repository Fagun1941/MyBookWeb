using BukyBookWeb.Data;
using BukyBookWeb.IRepository;
using BukyBookWeb.Models;
using BukyBookWeb.Repositories;
using BukyBookWeb.Repository;
using BukyBookWeb.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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


//builder.Services.AddTransient<CategoryRepository>();
//builder.Services.AddTransient<CategoryService>();
//builder.Services.AddTransient<ProductService>();
//builder.Services.AddTransient<ProductRepository>();
//builder.Services.AddTransient<AccountService>();
//builder.Services.AddTransient<AccountRepository>();
//builder.Services.AddTransient<AdminService>();
//builder.Services.AddTransient<AdminRepository>();

var app = builder.Build();

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
