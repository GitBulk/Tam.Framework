using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Control.BootstrapControls.Pagination
{
    public class OnlyPageControlsOptions : PagerOptions
    {
        public OnlyPageControlsOptions()
        {
            this.IsShowPages = false;
            this.Goto = false;
        }
    }
}
