using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Mvc.Extension.BoostrapPager
{
    public class PagerOptions
    {
        public int CurentPage { get; set; }

        public bool DisplayFirstPage { get; set; }

        public bool DisplayLastPage { get; set; }

        public bool DisplayNextPage { get; set; }

        public bool DisplayPreviousPage { get; set; }

        public string TextLastPage { get; set; }

        public string TextFirstPage { get; set; }

        public string TextNextPage { get; set; }

        public string TextPreviousPage { get; set; }

        public int PageSize { get; set; }

        public int TotalResult { get; set; }

        public int NumberOfPage { get; set; }

        public bool IsShowPages { get; set; }

        public int NumberOfPagesLeftSide { get; set; }

        public int NumberOfPagesRightSide { get; set; }

        public Size Size { get; set; }

        public string Page { get; set; }

        public PagerOptions()
        {
            this.PageSize = 10;
            this.DisplayFirstPage = this.DisplayLastPage = this.DisplayNextPage = this.DisplayPreviousPage = true;
            this.NumberOfPage = 5;
            this.NumberOfPagesLeftSide = this.NumberOfPagesRightSide = 2;
            this.TextFirstPage = "&laquo;";
            this.TextLastPage = "&raquo;";
            this.TextPreviousPage = "&lsaquo;";
            this.TextNextPage = "&rsaquo;";
            this.IsShowPages = true;
            this.Page = "page";
            this.Size = Size.Normal;
        }
    }
}