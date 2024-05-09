using HalloDocWebEntity.Data;
using HalloDocWebRepo.Implementation;
using HalloDocWebRepo.Interface;
using HalloDocWebServices.Implementation;
using HalloDocWebServices.Interfaces;
using Rotativa.AspNetCore;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation(); 
builder.Services.AddScoped<IPatient_Service, Patient_Service>();
builder.Services.AddScoped<IAdmin_Service, Admin_Service>();
builder.Services.AddScoped<IPatient_Repository, Patient_Repository>();
builder.Services.AddScoped<IAdmin_Repository, Admin_Repository>();
builder.Services.AddScoped<IProvider_Repository, Provider_Repository>();
builder.Services.AddScoped<IProvider_Service, Provider_Service>();
builder.Services.AddScoped<IJWT_Service, JWT_Service>();
builder.Services.AddDbContext<ApplicationContext>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".AdventureWorks.Session";
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.IsEssential = true;
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
app.UseRotativa();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
