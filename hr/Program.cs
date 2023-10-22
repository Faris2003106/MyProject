using hr.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<HRDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("ctr"));
});


builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<HRDbContext>()
        .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(op =>
{
    op.SignIn.RequireConfirmedAccount = false;
    op.SignIn.RequireConfirmedEmail = false;
   
    
    //op.Password.RequiredLength = 6;
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
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
