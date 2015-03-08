using System;
using System.Threading.Tasks;

namespace Tam.Blog.Repository.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}