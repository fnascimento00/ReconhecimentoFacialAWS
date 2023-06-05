using ReconhecimentoFacialAWS.Domains.Receivers;
using ReconhecimentoFacialAWS.Extensions;
using ReconhecimentoFacialAWS.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IUserRepository, UserRepository>(s => 
{
    return UserRepository.Create();
});

builder.Services.AddScoped<ILoginUserREC, LoginUserREC>();
builder.Services.AddScoped<IAddUserREC, AddUserREC>();
builder.Services.AddScoped<ICameraService, CameraService>();
builder.Services.Configure<AWSSettings>(builder.Configuration.GetSection("AWSSettings"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "CookieAuth";
    options.DefaultChallengeScheme = "CookieAuth";
})
.AddCookie("CookieAuth", x =>
{
    x.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    x.Cookie.SameSite = SameSiteMode.Lax;
    x.Cookie.HttpOnly = true;
    x.Cookie.IsEssential = true;
    x.Cookie.Name = "ReconhecimentoFacialAWS";
    x.LoginPath = "/Login";
    x.SlidingExpiration = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseExceptionHandler("/error");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
