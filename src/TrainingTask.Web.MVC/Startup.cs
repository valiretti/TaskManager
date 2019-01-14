using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrainingTask.BLL;
using TrainingTask.Common.Interfaces;
using TrainingTask.Common.Logging;
using TrainingTask.DAL;

namespace TrainingTask.Web.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var appFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var logsFolder = Path.Combine(appFolder, "Logs");
            Directory.CreateDirectory(logsFolder);
            var path = Path.Combine(logsFolder, "log.txt");

            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            var useOrm = Configuration.GetSection("ORM").GetValue<bool>("Nhibernate");
            var useNlog = Configuration.GetSection("Logs").GetValue<bool>("Nlog");

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var builder = new ContainerBuilder();

            if (useOrm)
            {
                builder.RegisterModule(new NHibernateModule(connectionString));
            }
            else
            {
                builder.RegisterModule(new AdoNetModule(connectionString));
            }

            if (useNlog)
            {
                builder.RegisterType<Nlog>().As<ILog>().SingleInstance();
            }
            else
            {
                builder.RegisterType<Log>().As<ILog>().WithParameter("pathToLogging", path).SingleInstance();
            }

            builder.RegisterModule<BusinessModule>();

            builder.RegisterModule<MapperModule>();

            builder.Populate(services);
            var container = builder.Build();

            return new AutofacServiceProvider(container);
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
