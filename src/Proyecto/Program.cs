using Microsoft.EntityFrameworkCore;
using Proyecto.Models.Domain;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// --- ACÁ VA LA CONFIGURACIÓN QUE ENCONTRASTE ---
builder.Services.AddDbContext<DBContext>(opt =>
{
    // Tu truco para ver el SQL en la consola:
    opt.LogTo(Console.WriteLine, new[] {
        DbLoggerCategory.Database.Command.Name
    },
    LogLevel.Information).EnableSensitiveDataLogging();

    // Conectamos a SQL Server leyendo el nombre del appsettings.json:
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser,IdentityRole>()
    .AddEntityFrameworkStores<DBContext>()
    .AddDefaultTokenProviders();


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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



using (var ambiente = app.Services.CreateScope())
{
    var services = ambiente.ServiceProvider;
    try{
    var context = services.GetRequiredService<DBContext>();
    var usuarioManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    
    await context.Database.MigrateAsync(); // Aplica las migraciones pendientes (si las hay)
    await LoadDataBase.CargarDatos(context, usuarioManager, roleManager);

    

    }
    catch(Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error cargando la base de datos.");
    }
}

app.Run();
