using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tam.MongoDb.Model
{
    public class SearchResult<T> where T : class
    {
        public long TotalItem { get; set; }

        public List<T> TotalResult { get; set; }
    }

    public class SearchResult: SearchResult<MongoBaseEntity>
    {

    }
}
