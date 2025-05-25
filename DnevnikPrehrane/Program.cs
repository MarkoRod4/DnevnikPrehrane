using DnevnikPrehrane.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    await SeedRolesAndUsersAsync(roleManager, userManager);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();


async Task SeedRolesAndUsersAsync(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
{
    // Dodaj uloge ako ne postoje

    string[] roles = new[] { "ADMIN", "USER", "TRENER" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Seed ADMIN
    var adminEmail = "admin@example.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
        await userManager.CreateAsync(adminUser, "Admin123!");
        await userManager.AddToRoleAsync(adminUser, "ADMIN");
    }

    // Seed USER
    var userEmail = "user@example.com";
    var user = await userManager.FindByEmailAsync(userEmail);
    if (user == null)
    {
        user = new IdentityUser { UserName = userEmail, Email = userEmail, EmailConfirmed = true };
        await userManager.CreateAsync(user, "User123!");
        await userManager.AddToRoleAsync(user, "USER");
    }

    // Seed TRENER
    var trenerEmail = "trener@example.com";
    var trener = await userManager.FindByEmailAsync(trenerEmail);
    if (trener == null)
    {
        trener = new IdentityUser { UserName = trenerEmail, Email = trenerEmail, EmailConfirmed = true };
        await userManager.CreateAsync(trener, "Trener123!");
        await userManager.AddToRoleAsync(trener, "TRENER");
    }
}
