using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NetCoreSeguridadEmpleados.Data;
using NetCoreSeguridadEmpleados.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options => options.EnableEndpointRouting = false);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();


string connectionString = builder.Configuration.GetConnectionString("SqlLocal");
builder.Services.AddDbContext<HospitalContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddTransient<RepositoryHospital>();

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

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "defalut",
        template: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();