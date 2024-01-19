using Company.BusinessLogic;
using Company.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ICompanyService, CompanyService>();
builder.Services.AddTransient<ICompanyRepository, CompanyRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseExceptionHandler("/Home/Error");
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "EditCompany",
    pattern: "/put/{id:int}",
    defaults: new { controller = "Test", action = "EditCompany" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
