using EmployeeManagementSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// Add Razor Pages support.
builder.Services.AddRazorPages();

// Register EmployeeDatabase as a singleton so the same database connection string
// is reused across all requests. The database file (employees.db) is created in
// the application working directory the first time the service is constructed.
var connString = builder.Configuration.GetConnectionString("EmployeeDb")
                 ?? "Data Source=employees.db";
builder.Services.AddSingleton(new EmployeeDatabase(connString));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();
