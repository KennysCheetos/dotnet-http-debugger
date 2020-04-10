using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Text;

namespace httpdebugger
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Run(async context =>
            {
                var requestTime = DateTime.Now;
                var fileName = $"{requestTime.ToFileTime()}.txt";

                var request = context.Request;

                StringBuilder str = new StringBuilder();
                str.AppendLine("Time: " + requestTime);
                str.AppendLine($"Protocol: {request.Protocol}");
                str.AppendLine($"Method: {request.Method}");
                str.AppendLine($"Host: {request.Host}");
                str.AppendLine($"Scheme: {request.Scheme}");
                str.AppendLine($"Path: {request.Path}");
                str.AppendLine($"QueryString: {request.QueryString}");
                str.AppendLine($"ContentType: {request.ContentType}");
                str.AppendLine($"ContentLength: {request.ContentLength}");

                str.AppendLine($"Headers: ");

                foreach (var header in request.Headers)
                {
                    str.AppendLine($"   {header.Key}: {header.Value}");
                }

                if (request.HasFormContentType)
                {
                    str.AppendLine($"Form: ");

                    foreach (var header in request.Form)
                    {
                        str.AppendLine($"   {header.Key}: {header.Value}");
                    }
                }
        
                str.AppendLine($"Cookies: ");

                foreach (var header in request.Cookies)
                {
                    str.AppendLine($"   {header.Key}: {header.Value}");
                }

                str.AppendLine("Body:");
                str.AppendLine();

                string body = string.Empty;

                using (var reader = new StreamReader(context.Request.Body))
                {
                    body = await reader.ReadToEndAsync();
                }

                str.AppendLine(body);

                System.IO.File.WriteAllText(fileName, str.ToString());

                await context.Response.WriteAsync("OK");
            });
        }
    }
}
