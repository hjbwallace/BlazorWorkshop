using Microsoft.EntityFrameworkCore;
using StoreAdmin.Core;
using StoreAdmin.Core.Data;
using StoreAdmin.Core.Stores;
using StoreAdmin.Core.Users;
using StoreAdmin.WorkshopApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

// Register the StoreAdmin services
builder.Services.AddSingleton<IRepository<Store>, StoreRepository>();
builder.Services.AddSingleton<IRepository<User>, UserRepository>();
builder.Services.AddDbContextFactory<WorkshopDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("WorkshopDb")));

var app = builder.Build();

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Create the database if it doesn't already exist
await CreateDatabaseAsync();

await app.RunAsync();

async Task CreateDatabaseAsync()
{
    using var scope = app!.Services.CreateScope();
    var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<WorkshopDbContext>>();
    var context = await contextFactory.CreateDbContextAsync();

    if (await context.Database.EnsureCreatedAsync())
    {
        var storeRepository = scope.ServiceProvider.GetRequiredService<IRepository<Store>>();
        var userRepository = scope.ServiceProvider.GetRequiredService<IRepository<User>>();
        await ExampleDataSeeder.RunAsync(storeRepository, userRepository);
    }
}