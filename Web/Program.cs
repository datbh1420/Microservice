using Microsoft.AspNetCore.Authentication.Cookies;
using Web.Models;
using Web.Services;
using Web.Services.IServices;
using Web.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<ICouponService, CouponService>();
builder.Services.AddHttpClient<ICartService, CartService>();
builder.Services.AddHttpClient<IOrderService, OrderService>();

// Config DI
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IVnPayService, VnPayService>();
// Setting SD
SD.AuthAPIBase = builder.Configuration["AuthAPI"];
SD.ProductAPIBase = builder.Configuration["ProductAPI"];
SD.CouponAPIBase = builder.Configuration["CouponAPI"];
SD.CartAPIBase = builder.Configuration["CartAPI"];
SD.OrderAPIBase = builder.Configuration["OrderAPI"];


// Config Authorization

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });
//.AddGoogle(options =>
//{
//    options.ClientId = builder.Configuration.GetValue<string>("Authentication:Google:ClientId");
//    options.ClientSecret = builder.Configuration.GetValue<string>("Authentication:Google:ClientSecret");
//})
//.AddFacebook(options =>
//{
//    options.AppId = builder.Configuration.GetValue<string>("Authentication:Facebook:AppId");
//    options.AppSecret = builder.Configuration.GetValue<string>("Authentication:Facebook:AppSecret");
//    options.CallbackPath = "/signin-facebook";
//});

builder.Services.Configure<GoogleOption>(builder.Configuration.GetSection("Authentication:Google"));
builder.Services.Configure<FaceBookOption>(builder.Configuration.GetSection("Authentication:Facebook"));
builder.Services.Configure<VnpayOption>(builder.Configuration.GetSection("Payment:Vnpay"));


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
