using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Control.BootstrapControls.Pagination
{
    public class PagerOption
    {
        public int CurentPage { get; set; }

        public virtual bool DisplayFirstLastPage { get; set; }

        public virtual bool DisplayPreviousNextPage { get; set; }

        public string TextLastPage { get; set; }

        public string TextFirstPage { get; set; }

        public string TextNextPage { get; set; }

        public string TextPreviousPage { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public int NumberOfPage { get; set; }

        public virtual bool IsShowPages { get; set; }

        public int NumberOfPagesLeftSide { get; set; }

        public int NumberOfPagesRightSide { get; set; }

        public PagerSize Size { get; set; }

        public string Page { get; set; }

        public virtual bool Goto { get; set; }

        public PagerAlignment Alignment { get; set; }

        public bool UseBoostrapPagerStyle { get; set; }

        public PagerOption()
        {
            this.PageSize = 10;
            this.DisplayFirstLastPage = this.DisplayPreviousNextPage = true;
            this.NumberOfPage = 5;
            this.NumberOfPagesLeftSide = this.NumberOfPagesRightSide = 2;
            this.TextFirstPage = "&laquo;";
            this.TextLastPage = "&raquo;";
            this.TextPreviousPage = "&lsaquo;";
            this.TextNextPage = "&rsaquo;";
            this.IsShowPages = true;
            this.Page = "page";
            this.Size = PagerSize.Normal;
            this.Alignment = PagerAlignment.Left;
        }
    }
}