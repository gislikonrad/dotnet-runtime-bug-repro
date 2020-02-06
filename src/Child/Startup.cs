using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Child
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            services.AddLogging(builder => builder.SetMinimumLevel(LogLevel.Information).AddDebug().AddConsole(options => options.IncludeScopes = true));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var factory = context.RequestServices.GetRequiredService<ILoggerFactory>();
                    var logger = factory.CreateLogger("ChildStartup");
                    var traceparent = context.Request.Headers[HeaderNames.TraceParent];
                    if (!StringValues.IsNullOrEmpty(traceparent))
                        logger.LogInformation($"Received traceparent: {traceparent}");
                    await context.Response.WriteAsync("Hello parent!");
                });
            });
        }
    }
}
