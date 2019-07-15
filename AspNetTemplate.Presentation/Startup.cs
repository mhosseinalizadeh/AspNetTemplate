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
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AspNetTemplate.ClientEntity;
using AspNetTemplate.ApplicationService.Helpers;
using System;
using static AspNetTemplate.ClientEntity.Enums;
using Serilog;
using Serilog.Events;

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

            //Configure appSettings
            IConfigurationSection appSettings = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettings);

            //Configure MemorayCache as in momory cache system
            services.AddMemoryCache();
            services.AddSingleton<IMemoryCache>(provider => new MemoryCache(new MemoryCacheOptions
            {
                SizeLimit = 20
            }));

            //Register Transaction for Unit of Work
            services.AddScoped<IDbTransaction>(provider =>
            {
                var UnitOfWork = (DapperUnitOfWork)provider.GetService(typeof(IUnitOfWork));
                return UnitOfWork.Transaction;
            });

            //Register Unit of Work
            services.AddScoped<IUnitOfWork, DapperUnitOfWork>(provider => new DapperUnitOfWork(Configuration.GetConnectionString("DefaultConnection")));

            //Using simple cookie authentication system
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            //Register application services
            services.AddScoped<ILocalizationService, LocalizationService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICryptographyService, CryptographyService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IEmailService, EmailService>();

            //Register repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILocalizationRepository, LocalizationRepository>();
            services.AddScoped<IExpenseInfoRepository, ExpenseInfoRepository>();

            //Register email body creator classes
            services.AddScoped<AddExpenseNotifyBodyCreator>();
            services.AddScoped<DeclineExpenseNotifyBodyCreator>();
            services.AddScoped<ApproveExpenseNotifyBodyCreator>();

            //Register email body creator resolver function
            services.AddTransient<Func<NotifyType, INotifyBodyCreator>>(serviceProvider => key =>
            {
                switch (key)
                {
                    case NotifyType.AddExpense:
                        return serviceProvider.GetService<AddExpenseNotifyBodyCreator>();
                    case NotifyType.DeclineExpense:
                        return serviceProvider.GetService<DeclineExpenseNotifyBodyCreator>();
                    case NotifyType.ApproveExpense:
                        return serviceProvider.GetService<ApproveExpenseNotifyBodyCreator>();
                    default:
                        throw new NotImplementedException(); // or maybe return null, up to you
                }
            });

            //Register ActionContextAccessor
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();

            //Register File Service data as singleton
            services.AddSingleton(provider => new FileServiceData {
                ExpensePhotoPath = HostingEnvironment.WebRootPath + "\\expensefiles\\"
            });

            //Use Serilog as exception logging system
            //Create log file per day and max file count is 31
            services.AddSingleton<ILogger>(c =>
            {
                return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .WriteTo.Async(config => config.File("logs\\log.txt", fileSizeLimitBytes: null, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 31, shared: true))
                .CreateLogger();
            });

            //USe Datatable for grid view
            services.AddJqueryDataTables();

            //Register compatibility version
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
