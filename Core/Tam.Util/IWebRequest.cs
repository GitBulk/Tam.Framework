using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Util
{
    public interface IWebRequest
    {
        string Get(string url);
        string Post(string url);
    }
}
