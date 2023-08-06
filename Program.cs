using Microsoft.AspNetCore.Authentication.Cookies;
using ProyectoPrimerParcial.Servicios;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Usuarios/Login";
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddSingleton<IUsuarioService, UsuarioServicioWithDapper>();
builder.Services.AddSingleton<IClientesService, ClientesServicioWithDapper>();
builder.Services.AddSingleton<IArticulosService, ArticulosServiceWithDapper>();
builder.Services.AddSingleton<IFacturasService, FacturasServiceWithDapper>();

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
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuarios}/{action=Login}");

app.Run();
