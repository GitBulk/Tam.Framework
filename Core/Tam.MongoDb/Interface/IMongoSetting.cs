using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.MongoDb.Interface
{
    public interface IMongoSetting
    {
        string ConnectionString { get; set; }

        string DatabaseName { get; set; }
    }
}
