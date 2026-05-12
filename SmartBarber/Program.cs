using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connStr = "Server=JLRR_NOTE;Database=SmartBarber;Trusted_Connection=True;TrustServerCertificate=True";

builder.Services.AddDbContext<DatabaseContext>(opt => opt.UseSqlServer(connStr));

builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var app = builder.Build();

app.UseSession();
app.MapControllerRoute("default", "{controller=Pessoa}/{action=Login}");

app.Run();
