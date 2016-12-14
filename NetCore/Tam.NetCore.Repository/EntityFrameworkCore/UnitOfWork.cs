using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tam.Repository.Contraction;

namespace Tam.NetCore.Repository.EntityFrameworkCore
{
    public class UnitOfWork : IUnitOfWork
    {
        protected DbContext context;
        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (context != null)
                    {
                        context.Dispose();
                    }
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancell)
        {
            return context.SaveChangesAsync(cancell);
        }
    }
}
