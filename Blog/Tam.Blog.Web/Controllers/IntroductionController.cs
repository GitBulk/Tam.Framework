using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tam.Blog.Web.Models;

namespace Tam.Blog.Web.Controllers
{
    public class IntroductionController : Controller
    {   //
        // GET: /Introduction/
        public ActionResult Index(string selectedLetter)
        {
            ViewBag.Message = "";
            return View();
        }
	}
}