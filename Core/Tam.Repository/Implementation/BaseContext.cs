using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tam.Repository.Interface;

namespace Tam.Repository.Implementation
{
    public class BaseContext
    {
        IBaseRepository baseRepo;
        public BaseContext(IBaseRepository repo)
        {
            this.baseRepo = repo;
        }

        public IBaseRepository TheRepository { get; set; }
    }
}
