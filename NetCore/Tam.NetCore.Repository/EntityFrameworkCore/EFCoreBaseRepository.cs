using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tam.Repository.Contraction;

namespace Tam.NetCore.Repository.EntityFrameworkCore
{
    public class EFCoreBaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected DbContext context;
        private bool disposed = false;
        private DbSet<T> dbSet;
        public bool IsSaveChanges { get; set; }

        /// <summary>
        /// The number of items in one page. Default value is 12.
        /// </summary>
        public int MaxPageSize { get; set; } = 12;
        public EFCoreBaseRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        private int SaveChanges()
        {
            if (this.IsSaveChanges)
            {
                return this.context.SaveChanges();
            }
            return 0;
        }

        private static bool IsNull(object item)
        {
            return (item == null);
        }

        protected static void ThrowIfNull(object item)
        {
            if (IsNull(item))
            {
                throw new ArgumentNullException(nameof(item));
            }
        }

        private Task<int> SaveChangesAsync()
        {
            if (this.IsSaveChanges)
            {
                return this.context.SaveChangesAsync();
            }
            return Task.FromResult(0);
        }

        public virtual int Add(T item)
        {
            ThrowIfNull(item);
            this.dbSet.Add(item);
            return SaveChanges();
        }


        public virtual Task AddAsync(T entity)
        {
            ThrowIfNull(entity);
            this.dbSet.AddAsync(entity);
            return SaveChangesAsync();
        }


        public int Count()
        {
            return this.dbSet.Count();
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                return Count();
            }
            return this.dbSet.Count(predicate);
        }

        public Task<int> CountAsync()
        {
            return this.dbSet.CountAsync();
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> match)
        {
            return this.dbSet.CountAsync(match);
        }

        public virtual int Delete(T item)
        {
            if (this.context.Entry(item).State == EntityState.Detached)
            {
                this.dbSet.Attach(item);
            }
            this.dbSet.Remove(item);
            return SaveChanges();
        }

        public virtual int Delete(object id)
        {
            T itemToDelete = GetById(id);
            if (itemToDelete != null)
            {
                return Delete(itemToDelete);
            }
            return 0;
        }

        public Task DeleteAsync(T entity)
        {
            ThrowIfNull(entity);
            if (this.context.Entry(entity).State == EntityState.Detached)
            {
                this.dbSet.Attach(entity);
            }
            this.dbSet.Remove(entity);
            return SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }
            }
            this.disposed = true;
        }

        public  virtual IEnumerable<T> GetAll()
        {
            return this.dbSet.ToList();
        }

        public virtual Task<List<T>> GetAllAsync()
        {
            return this.dbSet.ToListAsync();
        }

        public virtual T GetById(params object[] keyValues)
        {
            return this.dbSet.Find(keyValues);
        }

        public virtual T GetById(object id)
        {
            return this.dbSet.Find(id);
        }

        public virtual Task<T> GetByIdAsync(object id)
        {
            return this.dbSet.FindAsync(id);
        }

        public virtual T GetItem(Expression<Func<T, bool>> where)
        {
            var query = this.dbSet.AsQueryable();
            if (where != null)
            {
                query = query.Where(where);
            }
            return this.dbSet.FirstOrDefault();
        }

        public Task<T> GetItemAsync(Expression<Func<T, bool>> match)
        {
            var query = this.dbSet.AsQueryable();
            if (match != null)
            {
                query = query.Where(match);
            }
            return query.FirstOrDefaultAsync();
        }

        public IEnumerable<T> GetItems(Expression<Func<T, bool>> whereCondition = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            var query = this.dbSet.AsQueryable();
            if (whereCondition != null)
            {
                query = query.Where(whereCondition);
            }
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public Task<List<T>> GetItemsAsync(Expression<Func<T, bool>> match)
        {
            var query = this.dbSet.AsQueryable();
            if (match != null)
            {
                query = query.Where(match);
            }
            return query.ToListAsync();
        }

        public SearchResult<T> SearchFor(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            return SearchFor(filter, 0, 0, orderBy);
        }

        public SearchResult<T> SearchFor(Expression<Func<T, bool>> filter, int skip, int take, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            var query = this.dbSet.AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            int count = query.Count();
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (skip > 0)
            {
                query = query.Skip(skip);
            }
            if (take > MaxPageSize)
            {
                take = MaxPageSize;
            }
            if (take > 0)
            {
                query = query.Take(take);
            }
            return new SearchResult<T>
            {
                TotalItem = count,
                Data = query.ToList(),
                PageSize = take
            };
        }

        public Task<SearchResult<T>> SearchForAsync(Expression<Func<T, bool>> where, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            return SearchForAsync(where, 0, 0, orderBy);
        }

        public async Task<SearchResult<T>> SearchForAsync(Expression<Func<T, bool>> where, int skip, int take, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            var query = this.dbSet.AsQueryable();
            if (where != null)
            {
                query = query.Where(where);
            }
            int count = await query.CountAsync();
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (skip > 0)
            {
                query = query.Skip(skip);
            }
            if (take > MaxPageSize)
            {
                take = MaxPageSize;
            }
            if (take > 0)
            {
                query = query.Take(take);
            }
            return new SearchResult<T>
            {
                PageSize = take,
                Data = await query.ToListAsync(),
                TotalItem = count
            };
        }

        public virtual int Update(T item)
        {
            ThrowIfNull(item);
            this.dbSet.Attach(item);
            this.context.Entry(item).State = EntityState.Modified;
            return SaveChanges();
        }

        public Task UpdateAsync(T entity)
        {
            ThrowIfNull(entity);
            this.dbSet.Attach(entity);
            this.context.Entry(entity).State = EntityState.Modified;
            return SaveChangesAsync();
        }
    }
}
