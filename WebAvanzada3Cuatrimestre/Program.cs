using Microsoft.EntityFrameworkCore;
using WebAvanzada3Cuatrimestre.Application.Mapping;
using WebAvanzada3Cuatrimestre.Infrastructure.Data;
using WebAvanzada3Cuatrimestre.Infrastructure.Repository;
using WebAvanzada3Cuatrimestre.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorio generico: misma interfaz e implementacion vistas en clase.
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
// Persona 2/3: registrar los servicios de Categoria y Producto al implementarlos.
builder.Services.AddAutoMapper(cfg => { }, typeof(MapeoClases));

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseMiddleware<MiddlewareGlobalExceptionHandler>(); // Manejador visto en el repositorio de clase.
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}").WithStaticAssets();
app.Run();
