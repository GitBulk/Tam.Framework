using System.Collections.Generic;

namespace Tam.Repository.Contraction
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
