using Amazon;
using Amazon.S3;
using CSEONS.AuthApplication.Domain;
using CSEONS.AuthApplication.Domain.Entities;
using CSEONS.AuthApplication.Domain.Repositories.Abstract;
using CSEONS.AuthApplication.Domain.Repositories.AWSSDK;
using CSEONS.AuthApplication.Domain.Repositories.EntityFramework;
using CSEONS.AuthApplication.Hubs;
using CSEONS.AuthApplication.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

namespace CSEONS.AuthApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.Bind("Project", new Config());

            builder.Services.AddTransient<IMessagesRepository, EFMessagesRepository>();
            builder.Services.AddTransient<IGroupRepository, EFGroupRepository>();
            builder.Services.AddTransient<DataManager>();
            builder.Services.AddTransient<GroupManager>();

            var a = JsonConvert.SerializeObject(new Config());

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(config =>
            {
                config.UseSqlServer(Config.ConnectionString);
            });

            builder.Services.AddSingleton<IImageHandlerRepository>(x =>
            {
                return new AWSImageRepository(new AmazonS3Client(Config.AWSAccesToken, Config.AWSSecretAccesToken, new AmazonS3Config()
                {
                    ForcePathStyle = true,
                    RegionEndpoint = RegionEndpoint.EUWest1,
                    ServiceURL = Config.ServiceURL
                }), Config.BucketName);
            });
            
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.AllowedUserNameCharacters = "‡·‚„‰Â∏ÊÁËÈÍÎÏÌÓÔÒÚÛÙıˆ˜¯˘˙˚¸˝˛ˇ¿¡¬√ƒ≈®∆«»… ÀÃÕŒœ–—“”‘’÷◊ÿŸ⁄€‹›ﬁﬂabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ _-.";
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/Login";
                config.AccessDeniedPath = "/";
            });

            builder.Services.AddAuthorization();
            builder.Services.AddSignalR();

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

            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapHub<NotificationHub>("/chat");

            app.Run();
        }
    }
}