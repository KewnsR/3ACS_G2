var builder = WebApplication.CreateBuilder(args);

// Enable Razor Pages and Session
builder.Services.AddRazorPages();
builder.Services.AddSession(); // ✅ Enable session support

var app = builder.Build();

// Configure the request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession(); // ✅ Enable session middleware
app.UseAuthorization();

// ✅ Redirect to Login Page First
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapFallbackToPage("/Login");
});

app.Run();
