using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Tracing
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;

            #region HttpClient diagnostics hack

            //var handlerType = typeof(HttpClient).Assembly.GetType("System.Net.Http.DiagnosticsHandler");
            //var listenerField = handlerType.GetField("s_diagnosticListener", BindingFlags.NonPublic | BindingFlags.Static);
            //var listener = listenerField.GetValue(null) as DiagnosticListener;
            //listener.Subscribe(new NullObserver(), name => false);

            #endregion

            services.AddHttpClient().AddLogging(builder => builder.SetMinimumLevel(LogLevel.Information).AddConsole(options => options.IncludeScopes = true));
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
                    var loggerFactory = context.RequestServices.GetService<ILoggerFactory>();
                    var logger = loggerFactory.CreateLogger("TracingParent");

                    var httpFactory = context.RequestServices.GetService<IHttpClientFactory>();
                    var client = httpFactory.CreateClient();
                    var response = await client.GetAsync("http://localhost:5001");
                    var text = await response.Content.ReadAsStringAsync();
                    await context.Response.WriteAsync($"Child says: {text}");
                });
            });
        }

        class NullObserver : IObserver<KeyValuePair<string, object>>
        {
            public void OnCompleted()
            {
            }

            public void OnError(Exception error)
            {
            }

            public void OnNext(KeyValuePair<string, object> value)
            {
            }
        }
    }
}
