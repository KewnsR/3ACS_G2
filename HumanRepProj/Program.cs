using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.ML.OnnxRuntime;
using HumanRepProj.Data;
using HumanRepProj.HealthChecks;
using HumanRepProj.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentCors", policy =>
    {
        policy.WithOrigins("http://localhost:7036")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
        options.AccessDeniedPath = "/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            sqlOptions.CommandTimeout(60);
        });
});

// Health checks
builder.Services.AddHealthChecks()
    .AddCheck<DbContextHealthCheck<ApplicationDbContext>>("database_check", HealthStatus.Unhealthy, new[] { "ready" });

// ONNX Runtime - YOLOv8n-face
var env = builder.Environment;
var wwwrootPath = env.WebRootPath;
var modelPath = Path.Combine(wwwrootPath, "models", "yolov8n-face.onnx");

if (!File.Exists(modelPath))
    throw new FileNotFoundException($"Model file not found at {modelPath}");

builder.Services.AddSingleton<InferenceSession>(provider =>
    new InferenceSession(modelPath, new Microsoft.ML.OnnxRuntime.SessionOptions
    {
        ExecutionMode = Microsoft.ML.OnnxRuntime.ExecutionMode.ORT_SEQUENTIAL
    }));

// Face Recognition Service
builder.Services.AddScoped<FaceRecognitionService>();
builder.Services.AddSingleton<IOnnxFaceDetectionService, OnnxFaceDetectionService>();

var app = builder.Build();

// Middleware pipeline
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
app.UseStaticFiles();     // ✅ Serves static files (e.g., ONNX models in `wwwroot/`)
app.UseRouting();         // ✅ Routes HTTP requests to endpoints
app.UseCors("DevelopmentCors");
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseHealthChecks("/health");

app.MapControllers();
app.MapRazorPages();
app.MapGet("/", () => Results.Redirect("/Login"));

// Verify database connection
try
{
    await VerifyDatabaseConnection(app.Services, app.Logger);
}
catch (Exception ex)
{
    app.Logger.LogCritical(ex, "Database verification failed");
    throw;
}

await app.RunAsync();

async Task VerifyDatabaseConnection(IServiceProvider services, ILogger logger)
{
    using var scope = services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        logger.LogInformation("Verifying database connection...");
        if (await dbContext.Database.CanConnectAsync())
        {
            logger.LogInformation("Database connection successful");
        }
        else
        {
            logger.LogError("Database connection failed");
        }
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "Database verification failed");
        throw;
    }
}