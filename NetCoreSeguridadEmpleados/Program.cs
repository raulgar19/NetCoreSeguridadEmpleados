using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NetCoreSeguridadEmpleados.Data;
using NetCoreSeguridadEmpleados.Policies;
using NetCoreSeguridadEmpleados.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
options.EnableEndpointRouting = false)
    .AddSessionStateTempDataProvider();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(
    CookieAuthenticationDefaults.AuthenticationScheme,
    config =>
    {
        config.AccessDeniedPath = "/Managed/ErrorAcceso";
    }
);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SOLOJEFES", policy =>

        policy.RequireRole("PRESIDENTE", "DIRECTOR", "ANALISTA"));

    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("Admin"));

    options.AddPolicy("SoloRicos", policy =>
        policy.Requirements.Add(new OverSalarioRequirement()));

    options.AddPolicy("DeleteEmpleados", policy =>
        policy.Requirements.Add(new DeleteEmpleadoRequirement()));
});

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