using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.MongoDb
{
    public interface IMongoSetting
    {
        string ConnectionString { get; set; }
    }
}
