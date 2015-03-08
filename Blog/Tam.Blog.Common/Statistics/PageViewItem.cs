using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Blog.Common.Statistics
{
    public class PageViewItem
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public int ViewCount { get; set; }
    }
}
