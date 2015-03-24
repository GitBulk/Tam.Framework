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
            // I break up inheritance oop
            this.IsShowPages = false;
            this.Goto = false;
        }

        public override bool IsShowPages
        {
            get
            {
                return false;
            }
            set
            {
                base.IsShowPages = false;
            }
        }

        public override bool Goto
        {
            get
            {
                return false;
            }
            set
            {
                base.Goto = false;
            }
        }
    }
}
