using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace Tam.Control.BootstrapControls.Pagination
{
    public class OnlyPageNumberOption : PagerOption
    {
        public OnlyPageNumberOption()
        {
            // I break up inheritance oop
            this.IsShowPages = true;
            this.DisplayFirstLastPage = false;
            this.DisplayPreviousNextPage = false;
        }

        public override bool IsShowPages
        {
            get
            {
                return true;
            }
            set
            {
                base.IsShowPages = true;
            }
        }

        public override bool DisplayFirstLastPage
        {
            get
            {
                return false;
            }
            set
            {
                base.DisplayFirstLastPage = false;
            }
        }

        public override bool DisplayPreviousNextPage
        {
            get
            {
                return false;
            }
            set
            {
                base.DisplayPreviousNextPage = false;
            }
        }

    }
}
