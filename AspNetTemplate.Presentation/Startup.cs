using AspNetTemplate.DataAccess.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AspNetTemplate.DataAccess.Repository.IRepository;
using AspNetTemplate.DataAccess.Repository.Repository;
using AspNetTemplate.ApplicationService.UserService;
using System.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using AspNetTemplate.CommonService;
using Microsoft.AspNetCore.Hosting.Internal;
using AspNetTemplate.ApplicationService.AccountService;
using JqueryDataTables.ServerSide.AspNetCoreWeb.DependencyInjection;

namespace AspNetTemplate
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMemoryCache();
            services.AddSingleton<IMemoryCache>(provider => new MemoryCache(new MemoryCacheOptions
            {
                SizeLimit = 20
            }));

           

            services.AddScoped<IDbTransaction>(provider =>
            {
                var UnitOfWork = (DapperUnitOfWork)provider.GetService(typeof(IUnitOfWork));
                return UnitOfWork.Transaction;
            });


            services.AddScoped<IUnitOfWork, DapperUnitOfWork>(provider => new DapperUnitOfWork(Configuration.GetConnectionString("DefaultConnection")));

            // Enable cookie authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.AddScoped<ILocalizationService, LocalizationService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICryptographyService, CryptographyService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILocalizationRepository, LocalizationRepository>();
            services.AddScoped<IExpenseInfoRepository, ExpenseInfoRepository>();

            services.AddSingleton<FileServiceData>(provider => new FileServiceData {
                ExpensePhotoPath = HostingEnvironment.WebRootPath + "\\expensefiles\\"
            });
            services.AddJqueryDataTables();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "/",
                    defaults: new { controller = "Home", action = "Index"});

            });
        }
    }
}
