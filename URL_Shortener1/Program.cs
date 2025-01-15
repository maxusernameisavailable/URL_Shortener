using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using URL_Shortener1.DBContext;
using URL_Shortener1.Models;
using URL_Shortener1.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DBConnect");

builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedAccount = false;
});

builder.Services.AddScoped<IURLService, URLService>();

var app = builder.Build();

/*var scope = app.Services.CreateScope();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

if (!await roleManager.RoleExistsAsync("Admin"))
{
    await roleManager.CreateAsync(new Role { Name = "Admin" });
}

if (!await roleManager.RoleExistsAsync("User"))
{
    await roleManager.CreateAsync(new Role { Name = "User" });
}

var adminName = "Admin";
var adminRole = "Admin";
var adminUser = await userManager.FindByNameAsync(adminName);

if (adminUser == null)
{
    adminUser = new User { UserName = adminName, SecurityStamp = Guid.NewGuid().ToString() };
    await userManager.CreateAsync(adminUser, "1234");

    //adminUser.SecurityStamp = Guid.NewGuid().ToString();
    //await userManager.UpdateAsync(adminUser);

    await userManager.AddToRoleAsync(adminUser, adminRole);
} else
{
    if (!await userManager.IsInRoleAsync(adminUser, adminRole))
    {
        await userManager.AddToRoleAsync(adminUser, adminRole);
    }
}*/

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ShortURLsTable}/{action=ShortURLsTableView}"
    );

app.Run();
