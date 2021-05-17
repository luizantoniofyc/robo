using Becomex.Robo.Application.Helpers;
using Becomex.Robo.UserInterface.Models;
using Bexomex.Robo.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Becomex.Robo.UserInterface.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        private readonly string _baseApiUrl;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;

            _baseApiUrl = _config.GetValue<string>("AppConfig:AppURLs:UrlRobotApi");
        }

        public IActionResult Index()
        {
            ViewBag.RobotUrl = $"{_baseApiUrl}/robots";

            return View();
        }

        public IActionResult Control()
        {
            var model = new RobotModel();

            ViewBag.RobotUrl = $"{_baseApiUrl}/robots";

            var rotationStateList = Enum.GetValues(typeof(HeadRotationState)).Cast<HeadRotationState>()
                .Select(s => new SelectListItem
                { 
                    Text = s.GetDescription(),
                    Value = ((int)s).ToString()
                }).ToList();

            rotationStateList.Insert(0, new SelectListItem() { Value = "0", Text = "Selecione" });

            var incliniationStateList = Enum.GetValues(typeof(HeadInclinationState)).Cast<HeadInclinationState>()
                .Select(s => new SelectListItem
                {
                    Text = s.GetDescription(),
                    Value = ((int)s).ToString()
                }).ToList();

            incliniationStateList.Insert(0, new SelectListItem() { Value = "0", Text = "Selecione" });

            model.HeadRotationStateList = new SelectList(rotationStateList, "Value", "Text");
            model.HeadInclinationStateList = new SelectList(incliniationStateList, "Value", "Text");

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
