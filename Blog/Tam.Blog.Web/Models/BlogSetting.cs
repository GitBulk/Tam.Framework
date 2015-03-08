using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tam.Blog.Web.Models
{
    public class BlogSetting
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public string ThankMessage { get; set; }

        public bool AllowRegister { get; set; }

        public bool AllowContact { get; set; }

        public string Footer { get; set; }


    }
}