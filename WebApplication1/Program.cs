using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AuthDbContext>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();
builder.Services.ConfigureApplicationCookie(Config =>
{
	Config.LoginPath = "/Login";
});
builder.Services.AddDataProtection();

//implement audit log methods here
builder.Services.AddScoped<AuditLogService>();

//recaptcha
builder.Services.AddReCaptcha(builder.Configuration.GetSection("ReCaptcha"));

//session management
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(3); // Set your desired session timeout duration
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStatusCodePagesWithRedirects("/errors/custom404");
app.UseExceptionHandler("/errors/custom500");
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.Run();
