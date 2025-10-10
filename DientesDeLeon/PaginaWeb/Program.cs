using _02___sistemas._00___Perfil;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.SignalR;
using PaginaWeb.Models;
using PaginaWeb.Servicios._00___Perfil;

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<PerfilServicio>();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login";          // ruta de login
        options.LogoutPath = "/Home/Logout";        // ruta de logout
        options.AccessDeniedPath = "/Home/Denegado";// ruta de acceso denegado (opcional)
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        // options.Cookie.Name = "AuthCookie";       // opcional
    });

builder.Services.AddAuthorization();

// === SignalR ===
builder.Services.AddSignalR();

// CORS para SignalR (si el front está en otro dominio/puerto ajustá orígenes)
builder.Services.AddCors(options =>
{
    options.AddPolicy("SignalRCors", p =>
        p.AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials()
         .SetIsOriginAllowed(_ => true)); // En producción, restringí orígenes
});
builder.Services.AddSession();
var app = builder.Build();
app.UseSession();
// Pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
// Orden correcto: Routing -> (CORS) -> Authentication -> Authorization
app.UseRouting();

app.UseCors("SignalRCors");

app.UseAuthentication();
app.UseAuthorization();

// Static Web Assets (.NET 9)
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// === Endpoint del Hub de SignalR ===
app.MapHub<EsperasHub>("/hubs/esperas");

app.Run();

/// ================= HUB =================
public class EsperasHub : Hub
{
    // Útil si querés segmentar (p.ej. "medicos", "recepcion" o "consultorio-3")
    public Task JoinGrupo(string grupo) =>
        Groups.AddToGroupAsync(Context.ConnectionId, grupo);

    public Task LeaveGrupo(string grupo) =>
        Groups.RemoveFromGroupAsync(Context.ConnectionId, grupo);
}
