using System;
using System.Collections.Generic;
using System.IO;
using FPServer.Helper;
using System.Linq;
using System.Threading.Tasks;
using FPServer.Core;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FPServer
{
    public class Program
    {



        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        //"http://localhost:" + FrameCorex.Config[Enums.AppConfigEnum.AppPort]
        public static IWebHost BuildWebHost(string[] args)
        {
            var res = WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://localhost:" + FrameCorex.Config[Enums.AppConfigEnum.AppPort])
                .UseStartup<Startup>()
                .ConfigureLogging((ctx, log) =>
                {
                    log.AddConfiguration(ctx.Configuration.GetSection("Logging"));
                    log.AddDebug();
                    log.AddConsole();
                });

            //res = res.UseUrls("http://localhost:" + FrameCorex.Config[Enums.AppConfigEnum.AppPort]);


            try
            {
                res = res.UseUrls(FrameCorex.Config[Enums.AppConfigEnum.ExtAddres].Split('|').GenerateUrls());
            }
            catch (Exception)
            {
                res = res.UseUrls("http://localhost:" + FrameCorex.Config[Enums.AppConfigEnum.AppPort]);
            }

            return res.Build();
        }
    }
}
