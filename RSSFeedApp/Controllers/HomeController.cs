using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XMLManipulatorEngine;
namespace RSSFeedApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var xmlManipulator = new XMLManipulator();

            var rssFeedUrl = ConfigurationManager.AppSettings["RssFeedUrl"];
            var items = xmlManipulator.GetRssFeedsObjects(rssFeedUrl);
            return View(items);
        }

    }
}