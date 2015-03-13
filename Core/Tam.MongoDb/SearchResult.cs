using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tam.MongoDb
{
    public class SearchResult
    {
        public long TotalItem { get; set; }

        public List<MongoBaseEntity> TotalResult { get; set; }
    }
}
