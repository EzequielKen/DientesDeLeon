using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddControllersWithViews();

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

var app = builder.Build();

// Pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Orden correcto: Routing -> Authentication -> Authorization
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Static Web Assets (.NET 9)
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
