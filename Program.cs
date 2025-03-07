using Microsoft.EntityFrameworkCore;
using Unioteq.TrackNTrace.Models;
using Unioteq.TrackNTrace.Service.Repository; // Make sure to include this namespace

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Unioteq")));

// Register the MachineCheckPointRepository
builder.Services.AddScoped<MachineCheckPointRepository>(); // Register the repository

builder.Services.AddScoped<ShopFloorRepository>();

builder.Services.AddScoped<OperationRepository>();

builder.Services.AddScoped<PlantRepository>();

builder.Services.AddScoped<ModelPartsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Register}/{id?}");

app.Run();
