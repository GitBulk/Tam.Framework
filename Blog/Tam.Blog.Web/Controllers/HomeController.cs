using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tam.Blog.Model;
using Tam.Blog.Cache.Interface;
using System.Threading.Tasks;
using System.IO;

namespace Tam.Blog.Web.Controllers
{
    public class HomeController : Controller
    {
        //IUserService userService;
        //IPostCache postCache;

        //public HomeController() { }

        //public HomeController(IPostCache postCache, IUserService userService)
        //{
        //    this.postCache = postCache;
        //    this.userService = userService;
        //}

        //public HomeController(IUserService userService)
        //{
        //    //this.postCache = postCache;
        //    this.userService = userService;
        //}

        public ActionResult Di()
        {
            //string author = this.userService.GetAuthor();
            //User user = this.userService.GetUserByEmail("toan");
            return View();
        }

        public ActionResult Article()
        {
            //var result = this.postCache.Get12NewestItems();
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

#if !DEBUG
        [OutputCache(Duration = 86400)] // 1 hour
#endif
        public async Task<ActionResult> About()
        {
            string path = Server.MapPath("~/App_Data/about.txt");
            var reader = new StreamReader(path);
            string content = await reader.ReadToEndAsync();
            ViewBag.Message = content;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}