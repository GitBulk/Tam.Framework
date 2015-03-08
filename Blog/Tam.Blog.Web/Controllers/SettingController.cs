using System.Web.Mvc;
using Tam.Blog.Web.Models;

namespace Tam.Blog.Web.Controllers
{
    public class SettingController : Controller
    {
        //
        // GET: /Setting/
        public ActionResult Index()
        {
            //BlogSettingManager.AddOrUpdate();
            var blogSetting = new BlogSetting()
            {
                AllowContact = BlogSettingManager.AllowContact,
                AllowRegister = BlogSettingManager.AllowRegister,
                Description = BlogSettingManager.Description,
                Footer = BlogSettingManager.Footer,
                ThankMessage = BlogSettingManager.ThankMessage,
                Title = BlogSettingManager.Title
            };
            return View(blogSetting);
        }

        [HttpPost]
        public ActionResult Index(BlogSetting model)
        {
            BlogSettingManager.AddOrUpdate(model);
            return View();
        }
    }
}