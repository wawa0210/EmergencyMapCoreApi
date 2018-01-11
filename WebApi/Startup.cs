using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using EmergencyAccount.Application;
using EmergencyCompany.Application;
using Exceptionless;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Filter;
using WebApi.FrameWork;

namespace WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables();
            Configuration = builder.Build();
            ConfigurationNew = configuration;
        }
        public IConfigurationRoot Configuration { get; private set; }

        public IConfiguration ConfigurationNew { get; set; }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            MapperInit.InitMapping();
            app.UseExceptionless("riuCGjWnRDEXcvLASaeRHVdYE9OxHyFtb9SBXPvU");
            app.UseMiddleware<ExceptionHandlerMiddleWare>();
            app.UseCors(builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            app.UseMvc();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddEntityFrameworkSqlServer().AddDbContext<EfDbContext>(options => options.UseMySQL(Configuration.GetConnectionString("SqlServer")));
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidationActionFilter));
                options.RespectBrowserAcceptHeader = true;
            });
            services.AddAuthentication().AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Configuration["Tokens:Issuer"],
                    ValidAudience = Configuration["Tokens:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                };
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule());

        }
    }

    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // The generic ILogger<TCategoryName> service was added to the ServiceCollection by ASP.NET Core.
            // It was then registered with Autofac using the Populate method in ConfigureServices.
            builder.Register(c => new AccountService()).As<IAccountService>().InstancePerLifetimeScope();
            builder.Register(c => new CompanyService()).As<ICompanyService>().InstancePerLifetimeScope();
            builder.Register(c => new DangerousProductService()).As<IDangerousProductService>().InstancePerLifetimeScope();
            //builder.Register(c => new MuseumService(c.Resolve<EfDbContext>())).As<IMuseumService>().InstancePerLifetimeScope();
            //builder.Register(c => new AntiquesClassService(c.Resolve<EfDbContext>())).As<IAntiquesClassService>().InstancePerLifetimeScope();
            //builder.Register(c => new AntiquesService(c.Resolve<EfDbContext>())).As<IAntiquesService>().InstancePerLifetimeScope();
        }
    }
}
