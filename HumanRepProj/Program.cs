using Microsoft.EntityFrameworkCore;
using HumanRepProj.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;
using System.Data;
using HumanRepProj.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
        options.AccessDeniedPath = "/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

// Enhanced database configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
            sqlOptions.CommandTimeout(60); // 60 seconds command timeout
        });
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
});

// Add health checks
builder.Services.AddHealthChecks()
    .AddCheck<DbContextHealthCheck<ApplicationDbContext>>(
        "database_check",
        HealthStatus.Unhealthy,
        new[] { "ready" });

var app = builder.Build();

// Database connection verification
await VerifyDatabaseConnection(app.Services, app.Logger);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Proper ordering is important here
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseHealthChecks("/health");

app.MapRazorPages();

// Redirect root to Login page
app.MapGet("/", () => Results.Redirect("/Login"));

app.Run();

async Task VerifyDatabaseConnection(IServiceProvider services, ILogger logger)
{
    using var scope = services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    try
    {
        logger.LogInformation("Verifying database connection...");

        // Try a simple query
        var canConnect = await dbContext.Database.CanConnectAsync();

        if (canConnect)
        {
            logger.LogInformation("Database connection successful");

            // Verify we can perform a simple read operation
            var testEmployee = await dbContext.Employees
    .AsNoTracking() // Avoid tracking issues
    .Select(e => new { e.EmployeeID, e.FirstName }) // Only request columns that exist
    .FirstOrDefaultAsync();

            if (testEmployee != null)
            {
                logger.LogInformation($"Test query successful. Found employee {testEmployee.EmployeeID}");
            }

            // Verify we can perform a simple write operation (in a transaction that we'll roll back)
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var testUpdate = await dbContext.Database.ExecuteSqlRawAsync(
                    "UPDATE Employees SET FirstName = FirstName WHERE EmployeeID = 1");
                logger.LogInformation("Test update operation successful");
                await transaction.RollbackAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Test update operation failed");
                await transaction.RollbackAsync();
                throw;
            }
        }
        else
        {
            logger.LogError("Database connection failed");
        }
    }
    catch (SqlException sqlEx)
    {
        logger.LogCritical(sqlEx, "SQL Server connection error. Check your connection string and network.");
        throw;
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "Database verification failed");
        throw;
    }
}