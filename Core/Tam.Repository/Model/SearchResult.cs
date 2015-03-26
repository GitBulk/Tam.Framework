using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Repository.Model
{
    public class SearchResult<T> where T : class
    {
        public int TotalItem { get; set; }

        public List<T> Data { get; set; }

        public int PageSize { get; set; }

        public SearchResult()
        {
            this.Data = new List<T>();
        }
    }

}
