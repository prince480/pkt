using PKT.DataAccess;
using PKT.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<SessionHandler>();
builder.Services.AddSingleton<UserInfo>();
builder.Services.AddScoped<IExcelService, ExcelService>();
//Newly Added

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register the DBContext as a singleton service

builder.Services.AddSingleton<DBContext>();
builder.Services.AddSingleton<SqlHelper>();



builder.Services.AddScoped<NavBarActionFilter>();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.AddService<NavBarActionFilter>();
});


var app = builder.Build();
 
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseSession();


app.UseStaticFiles();


app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SignIn}/{action=Login}/{id?}");

app.Run();
