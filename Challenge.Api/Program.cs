using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Challenge.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local")
                .UseStartup<Startup>()
                .UseKestrel()
                .UseUrls("http://*:5500")
                .Build();

            host.Run();
        }

    }
}
