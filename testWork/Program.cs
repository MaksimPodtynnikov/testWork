using testWork.Data;
using Microsoft.EntityFrameworkCore;
using testWork.Services.Interfaces;
using testWork.Services.Implementations;
using testWork.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDBContent>(op => op.UseNpgsql("Host=localhost;Port=5432;Database=departmentsDB;Username=postgres;Password=112233"));
builder.Services.AddTransient<IDepartments, DepartmentsRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentsService>();
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
	pattern: "{controller=Departments}/{action=Index}/{id?}");

app.Run();
