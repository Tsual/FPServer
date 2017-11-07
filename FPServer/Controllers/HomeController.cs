﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FPServer.Models;
using FPServer.Core;

namespace FPServer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            using (var server = FrameCorex.getService())
            {
                if(server.UserRegist_CheckLIDNotExsist("test"))
                {
                    server.UserRegist("test", "test");
                }
                server.UserLogin("test", "123213");
                var user = FrameCorex.GetServiceInstanceInfo(server).User;
                user.Infos.Remark = "test remark";
                user.SaveInfos();
                int a = 0;
            }
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            using (var server=FrameCorex.getService())
            {
                server.UserRegist("test", "test");
            }

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
