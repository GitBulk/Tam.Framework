using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Blog.Cache.Interface
{
    public interface IBaseCache<T> where T : class
    {
        void Add(string valueOfId, T entity, int expiredTime = 60); // 60 minutes

        void Delete(string valueOfId);

        void Update(string valueOfId, T entity, int expiredTime = 60); // 60 minutes

        T GetItem(object id);

        bool Exists(object id);

        T AddOrGetExistingItem(object id, int expiredInMinute);
    }
}
