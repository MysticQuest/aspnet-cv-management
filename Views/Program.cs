using Microsoft.EntityFrameworkCore;
using CvManagementApp.Models;
using CvManagementApp.Services;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services);
var app = builder.Build();
ConfigureApp(app);

app.Run();


void ConfigureServices(IServiceCollection services)
{
    services.AddRazorPages();

    var inMemoryDbName = 
        builder.Configuration.GetValue<string>("InMemoryDatabase:Name") ?? "CvManagementDb";
    builder.Services.AddDbContext<CvManagementDbContext>(options =>
    options.UseInMemoryDatabase(inMemoryDbName));

    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    services.AddScoped<ICandidateRepository, CandidateRepository>();

    services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
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
