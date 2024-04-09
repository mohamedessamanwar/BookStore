using BookStore.BusnessLogic;
using BookStore.BusnessLogic.Helper;
using BookStore.BusnessLogic.Services.Email;
using BookStore.DataAccessLayer;
using BookStore.DataAccessLayer.Contexts;
using BookStore.DataAccessLayer.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using NToastNotify;
using Stripe;
namespace BookStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDataAcessLayerRegistration();
            builder.Services.AddServicesRegistration();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                  .AddEntityFrameworkStores<ApplicationDbContext>()
                  .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                                    .AddCookie(options =>
                                    {
                                        options.LoginPath = "/Account/Login"; // Specify your login page route here
                                    });
            builder.Services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            });
            builder.Services.Configure<StripeSitting>(builder.Configuration.GetSection(nameof(Stripe)));
            builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));
            builder.Services.AddTransient<IMailingService, MailingService>();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
            {
                ProgressBar = true,
                PositionClass = ToastPositions.TopRight,
                PreventDuplicates = true,
                CloseButton = true
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseSession();
            app.UseStaticFiles();
            app.UseRouting();
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>(); //API Key atamas? yap?lm?? oluyor.
            app.UseAuthentication();
            app.UseAuthorization();
            //	app.UseCors("AllowAll");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.Run();
        }
    }
}
