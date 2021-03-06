﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Joonasw.AspNetCore.SecurityHeaders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.DependencyInjection;

namespace iShop.Web.Server.Commons.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddCustomCsp(this IApplicationBuilder app)
        {
            app.UseCsp(csp =>
            {
                csp.ByDefaultAllow.FromSelf();
                csp.AllowScripts.AllowUnsafeEval().FromSelf();
                csp.AllowStyles.AllowUnsafeInline().FromSelf();
                csp.AllowImages.From("data:").FromSelf();
                csp.AllowStyles.From("https://unpkg.com/ngx-bootstrap/datepicker/bs-datepicker.css");
                csp.SetReportOnly();

            });
            return app;
        }

        public static IApplicationBuilder AddDevMiddleware(this IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetRequiredService<IHostingEnvironment>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
                app.UseSwagger();
                app.UseSwaggerUI(s => 
                {
                    s.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
                    s.RoutePrefix = "api/swagger";

                });
            }
            else
            {
                // Instead of redirect to Home/Error, we will responce a error message
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }
            return app;
        }
    }
}
