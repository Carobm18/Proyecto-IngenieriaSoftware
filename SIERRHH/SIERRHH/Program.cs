using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services to the container.
builder.Services.AddControllersWithViews();
//Se configura el esquema de autenticacion por medio de cookies en la aplicacion web
builder.Services.AddAuthentication("CookieAuthentication").AddCookie("CookieAuthentication",
    config => { config.Cookie.Name = "UserloginCookie"; config.LoginPath = "/UsuarioEmpleado/Login"; });

//Se habilita el string de conexion
builder.Services.AddDbContext<SIERRHH.Models.AppBdContext>(
 options => options.UseSqlServer(builder.Configuration.GetConnectionString("Stringconexion")));

// Configure options for file uploads
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB, ajusta según tus necesidades
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//Aqui se autoriza
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
