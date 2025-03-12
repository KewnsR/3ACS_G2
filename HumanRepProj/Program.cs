using Microsoft.EntityFrameworkCore;
using HumanRepProj.Data; // Ensure this namespace is correct
using Microsoft.Extensions.DependencyInjection; // Add this using directive
using Microsoft.EntityFrameworkCore.SqlServer; // Add this using directive

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession();

// Configure the database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    // This will redirect the root URL to the Login page
    endpoints.MapGet("/", context =>
    {
        context.Response.Redirect("/Login");
        return Task.CompletedTask;
    });
});

app.Run();
