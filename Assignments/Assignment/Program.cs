using Assignment_Service.Implementation;
using Assignment_Service.Interface;
using Assignment_Repository.Interface;
using Assignment_Repository.Implementation;
using Assignment_WebEntity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation(); ;
builder.Services.AddScoped<ILMS_Service, LMS_Service>();
builder.Services.AddScoped<ILMS_Repository, LMS_Repository>();

builder.Services.AddDbContext<ApplicationContext>();
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
