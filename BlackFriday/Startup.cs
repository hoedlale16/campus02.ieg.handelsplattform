using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackFriday.ServiceClients;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Swashbuckle.AspNetCore.Swagger;

namespace BlackFriday
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
            services.AddMvc();

            //Add Polly-Extenions for Resilience Handling
            // Retry per Srevice: 3 times
            // CircuitBreaker: Handling for temporary errors
            //Logging: https://github.com/App-vNext/Polly/wiki/Polly-and-HttpClientFactory#configuring-policies-to-use-services-registered-with-di-such-as-iloggert

            //SimpleCreditCartServiceClient
            services.AddHttpClient<SimpleCreditCartServiceClient>()
                .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.CircuitBreakerAsync(
                        handledEventsAllowedBeforeBreaking: 5,
                        durationOfBreak: TimeSpan.FromMinutes(1)
                ))
                .AddPolicyHandler((service, request) => HttpPolicyExtensions.HandleTransientHttpError()
                    .WaitAndRetryAsync(new[]
                        {
                            TimeSpan.FromSeconds(1),
                            TimeSpan.FromSeconds(1),
                            TimeSpan.FromSeconds(1)
                        },
                        onRetry: (outcome, timespan, retryAttempt, context) =>
                        {
                            service.GetService<ILogger<SimpleCreditCartServiceClient>>()
                                .LogError("Delaying for {delay}ms, then making retry {retry}.", timespan.TotalMilliseconds, retryAttempt);

                            if(retryAttempt == 3)
                            {
                                service.GetService<ILogger<SimpleCreditCartServiceClient>>().LogError("Service seems to be down - try next one...");
                                //This is done in SimpleCreditCartServiceClient - Catch Block
                            }
                        }
                ));

            //SimpleProductFtpProductService
            services.AddHttpClient<SimpleProductServiceClient>()

               .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.CircuitBreakerAsync(
                       handledEventsAllowedBeforeBreaking: 5,
                       durationOfBreak: TimeSpan.FromMinutes(1)
               ))
               .AddPolicyHandler((service, request) => HttpPolicyExtensions.HandleTransientHttpError()
                   .WaitAndRetryAsync(new[]
                       {
                            TimeSpan.FromSeconds(1),
                            TimeSpan.FromSeconds(1),
                            TimeSpan.FromSeconds(1)
                       },
                       onRetry: (outcome, timespan, retryAttempt, context) =>
                       {
                           service.GetService<ILogger<SimpleProductServiceClient>>()
                               .LogError("Delaying for {delay}ms, then making retry {retry}.", timespan.TotalMilliseconds, retryAttempt);

                           if (retryAttempt == 3)
                           {
                               service.GetService<ILogger<SimpleProductServiceClient>>().LogError("Service seems to be down - try next one...");
                                //This is done in SimpleCreditCartServiceClient - Catch Block
                            }
                       }
               ));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "BlackFriday API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Black Friday API");
            });
            app.UseMvc();
        }
    }
}
