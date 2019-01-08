using System.IO;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrainingTask.BLL;
using TrainingTask.Common.Interfaces;
using TrainingTask.Common.Logging;
using TrainingTask.DAL;

namespace TrainingTask.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var logsFolder = Path.Combine(appFolder, "Logs");
            Directory.CreateDirectory(logsFolder);
            var path = Path.Combine(logsFolder, "log.txt");

            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerDocument();

            services.AddSingleton<ILog, Log>(provider => new Log(path));
            services.RegisterRepositories(connectionString);
            services.RegisterServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILog log)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseSwagger().UseSwaggerUi3();

            app.UseExceptionHandler(
                options => {
                    options.Run(
                        async context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.ContentType = "text/html";
                            var ex = context.Features.Get<IExceptionHandlerFeature>();
                            if (ex != null)
                            {
                                var err = $"<h4>Error: {ex.Error.Message}</h4>";
                                log.Error($"Error: { ex.Error.Message} Trace: { ex.Error.StackTrace }");
                                await context.Response.WriteAsync(err).ConfigureAwait(false);
                            }
                        });
                }
            );

            app.UseMvc();
        }
    }
}
