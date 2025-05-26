using EduPro.Data;
using EduPro.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args); // create for Web application 

// database 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor(); 


// session 
builder.Services.AddDistributedMemoryCache(); // local storage
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 10 mins
    options.Cookie.HttpOnly = true; // 
    options.Cookie.IsEssential = true; 
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IErrorLogService, ErrorLogService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IUserCourseService, UserCourseService>();
builder.Services.AddScoped<IChatService, LLMRouterService>();

// Add logging
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

// authencation on cookie 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // remeber me use cookies
        options.SlidingExpiration = true;
    });

builder.Services.AddControllersWithViews(); // 

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // view request bootstrap w Css 

app.UseRouting(); // from controller to controller 

app.UseSession(); // use local stroage 

app.UseAuthentication();  // user or admin 
app.UseAuthorization(); // user activitites & admin activities tanyha 5als 

// Configure routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=EduPro}/{action=Index}/{id?}");

app.Run();