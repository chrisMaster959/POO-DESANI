using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connStr = "Server=JLRR_NOTE;Database=SmartBarber;Trusted_Connection=True;TrustServerCertificate=True";

builder.Services.AddDbContext<DatabaseContext>(opt => opt.UseSqlServer(connStr));

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.MapControllerRoute("default", "{controller=Pessoa}/{action=Login}");

app.Run();
