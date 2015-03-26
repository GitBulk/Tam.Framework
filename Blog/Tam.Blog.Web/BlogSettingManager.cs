using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tam.Blog.Web.Models;
using NLog;
using System.IO;
using Newtonsoft.Json;
namespace Tam.Blog.Web
{
    public static class BlogSettingManager
    {

        static Logger Logger = LogManager.GetCurrentClassLogger();
        //static string FilePath = Server.MapPath("~/App_Data/blogsettings.json");
        static object LockObject = new object();
        public static string Title { get; set; }
        public static string Description { get; set; }

        public static string ThankMessage { get; set; }

        public static bool AllowRegister { get; set; }

        public static bool AllowContact { get; set; }

        public static string Footer { get; set; }

        static BlogSettingManager()
        {
            Init();
        }

        private static void Init()
        {
            Logger.Trace("Trace Init BlogSettingManager.");
            string filePath = HttpContext.Current.Server.MapPath("~/App_Data/blogsettings.json");
            if (File.Exists(filePath))
            {
                StreamReader reader = new StreamReader(filePath);
                string content = reader.ReadToEnd();
                BlogSetting blogSetting = null;
                try
                {
                    blogSetting = JsonConvert.DeserializeObject<BlogSetting>(content);
                }
                catch (Exception ex)
                {
                    blogSetting = new BlogSetting
                    {
                        AllowContact = false,
                        AllowRegister = false,
                        Description = "description",
                        Footer = "footer",
                        ThankMessage = "thanks",
                        Title = "title"
                    };
                    Logger.Error(ex);
                }
                SetSettingValue(blogSetting);
            }
            //else
            //{
            //    var blogSetting = new BlogSetting
            //    {
            //        AllowContact = false,
            //        AllowRegister = false,
            //        Description = "description",
            //        Footer = "footer",
            //        ThankMessage = "thanks",
            //        Title = "title"
            //    };
            //    SetSettingValue(blogSetting);
            //    string json = JsonConvert.SerializeObject(blogSetting, Formatting.Indented);

            //    lock (LockObject)
            //    {
            //        StreamWriter writer = new StreamWriter(filePath, false); // false is overwriten
            //        writer.WriteLine(json);
            //        writer.Flush();
            //        writer.Close();
            //    }
            //}
        }

        private static void SetSettingValue(BlogSetting blogSetting)
        {
            Title = blogSetting.Title;
            Description = blogSetting.Description;
            ThankMessage = blogSetting.ThankMessage;
            AllowRegister = blogSetting.AllowRegister;
            AllowContact = blogSetting.AllowContact;
            Footer = blogSetting.Footer;
        }

        public static bool AddOrUpdate(BlogSetting blogSetting)
        {
            try
            {
                if (blogSetting == null)
                {
                    return false;
                }
                //var blogSetting = new BlogSetting
                //{
                //    AllowContact = false,
                //    AllowRegister = false,
                //    Description = "description",
                //    Footer = "footer",
                //    ThankMessage = "thanks",
                //    Title = "title"
                //};
                string json = JsonConvert.SerializeObject(blogSetting, Formatting.Indented);
                string filePath = HttpContext.Current.Server.MapPath("~/App_Data/blogsettings.json");
                lock (LockObject)
                {
                    StreamWriter writer = new StreamWriter(filePath, false); // false is overwriten
                    writer.WriteLine(json);
                    writer.Flush();
                    writer.Close();
                }
                SetSettingValue(blogSetting);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }


        public static bool AddOrGetExisting()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }
    }
}