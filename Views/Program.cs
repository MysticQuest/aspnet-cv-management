using Microsoft.EntityFrameworkCore;
using CvManagementApp.Models;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services);
var app = builder.Build();
ConfigureApp(app);

app.Run();


void ConfigureServices(IServiceCollection services)
{
    services.AddRazorPages();

    builder.Services.AddDbContext<CvManagementDbContext>(options =>
    options.UseInMemoryDatabase("CvManagementDb"));
}

void ConfigureApp(WebApplication app)
{
    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapRazorPages();
}
