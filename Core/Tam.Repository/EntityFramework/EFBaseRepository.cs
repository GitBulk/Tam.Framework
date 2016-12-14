using Microsoft.VisualBasic;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tam.Database;
using Tam.Repository.Contraction;

namespace Tam.Repository.EntityFramework
{
    public abstract class EFBaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected ISqlServerHelper sqlHelper;

        protected IUnitOfWork unitOfWork;

        private DbContext context;

        private DbSet<T> dbSet;

        private bool disposed = false;

        private int maxPageSize;
        protected virtual int MaxPageSize
        {
            get
            {
                int maxPageSize = GetMaxPageSize();
                return maxPageSize;
            }
            set
            {
                maxPageSize = value;
            }
        }

        public EFBaseRepository(DbContext context)
            : this(context, false, null, null)
        { }

        public EFBaseRepository(DbContext context, ISqlServerHelper sqlHelper)
            : this(context, false, sqlHelper, null)
        { }

        public EFBaseRepository(DbContext context, bool isSaveChage, ISqlServerHelper sqlHelper,
            IUnitOfWork unitOfWork)
        {
            if (context == null)
            {
                throw new ArgumentNullException("Context");
            }
            Log = LogManager.GetLogger(this.GetType().FullName);
            this.IsSaveChanges = IsSaveChanges;
            this.sqlHelper = sqlHelper;
            this.context = context;
            this.dbSet = context.Set<T>();
            this.unitOfWork = unitOfWork;
        }

        public bool IsSaveChanges { get; set; }

        protected Logger Log { get; private set; }

        private static int GetMaxPageSize()
        {
            int maxPageSize = 50;
            try
            {
                string temp = Convert.ToString(ConfigurationManager.AppSettings["EFMaxPageSize"]);
                if (string.IsNullOrWhiteSpace(temp) == false)
                {
                    if (Information.IsNumeric(temp))
                    {
                        int tempPageSize = Convert.ToInt32(temp);
                        if (tempPageSize > 0)
                        {
                            maxPageSize = tempPageSize;
                        }
                    }
                }
            }
            catch
            {
                maxPageSize = 50;
            }
            return maxPageSize;
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

        public virtual int Add(T entityToInsert)
        {
            ThrowIfNull(entityToInsert);
            this.dbSet.Add(entityToInsert);
            return SaveChanges();
        }

        public virtual async Task AddAsync(T entity)
        {
            ThrowIfNull(entity);
            this.dbSet.Add(entity);
            await SaveChangesAsync();
        }

        public virtual async Task<int> CountAsync()
        {
            return await this.dbSet.CountAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> match)
        {
            return await this.dbSet.CountAsync(match);
        }

        public virtual int Delete(object id)
        {
            T entityToDelete = GetById(id);
            if (entityToDelete != null)
            {
                return Delete(entityToDelete);
            }
            return 0;
        }

        public virtual int Delete(T entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
            return SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<T> GetItems(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await this.dbSet.ToListAsync();
        }

        public virtual T GetById(object id)
        {
            return this.dbSet.Find(id);
        }

        public virtual T GetById(params object[] keyValues)
        {
            return this.dbSet.Find(keyValues);
        }

        /// <summary>
        /// Get an entity by id asynchronous
        /// </summary>
        /// <param name="id">id (primary key)</param>
        /// <returns>Entity</returns>
        public virtual async Task<T> GetByIdAsync(object id)
        {
            return await this.dbSet.FindAsync(id);
        }

        public T GetItem(Expression<Func<T, bool>> match)
        {
            var query = this.dbSet.AsQueryable();
            if (match != null)
            {
                query = query.Where(match);
            }
            return query.FirstOrDefault();
        }

        public virtual async Task<T> GetItemAsync(Expression<Func<T, bool>> match)
        {
            var query = this.dbSet.AsQueryable();
            if (match != null)
            {
                query = query.Where(match);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetItemsAsync(Expression<Func<T, bool>> match)
        {
            var query = this.dbSet.AsQueryable();
            if (match != null)
            {
                query = query.Where(match);
            }
            return await query.ToListAsync();
        }


        public virtual async Task DeleteAsync(T entity)
        {
            if (entity == null)
            {
                return;
            }
            if (this.context.Entry(entity).State == EntityState.Detached)
            {
                this.dbSet.Attach(entity);
            }
            this.dbSet.Remove(entity);
            await SaveChangesAsync();
        }

        //public virtual async Task<List<T>> SearchForAsync(System.Linq.Expressions.Expression<Func<T, bool>> where,
        //    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        //{
        //    return await SearchForAsync(where, 0, 0, orderBy);
        //}

        //public virtual async Task<List<T>> SearchForAsync(System.Linq.Expressions.Expression<Func<T, bool>> where,
        //    int skip, int take, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        //{
        //    var query = QueryData(where, skip, take, orderBy);
        //    return await query.ToListAsync();
        //}

        //private IQueryable<T> QueryData(System.Linq.Expressions.Expression<Func<T, bool>> where, int skip, int take, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        //{
        //    var query = this.dbSet.AsQueryable();
        //    if (where != null)
        //    {
        //        query = query.Where(where);
        //    }
        //    if (orderBy != null)
        //    {
        //        query = orderBy(query);
        //    }
        //    if (skip > 0)
        //    {
        //        query = query.Skip(skip);
        //    }
        //    if (take > MaxPageSize)
        //    {
        //        take = MaxPageSize;
        //    }
        //    if (take > 0)
        //    {
        //        query = query.Take(take);
        //    }
        //    return query;
        //}

        public virtual int Update(T entityToUpdate)
        {
            ThrowIfNull(entityToUpdate);
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
            return SaveChanges();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            ThrowIfNull(entity);
            this.dbSet.Attach(entity);
            this.context.Entry(entity).State = EntityState.Modified;
            await SaveChangesAsync();
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

        private int SaveChanges()
        {
            if (this.IsSaveChanges)
            {
                return this.context.SaveChanges();
            }
            return 0;
        }

        private async Task<int> SaveChangesAsync()
        {
            if (this.IsSaveChanges)
            {
                return await this.context.SaveChangesAsync();
            }
            return 0;
        }

        public string GetBackgroundTechnology()
        {
            return "Entity Framework";
        }

        public int Count(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
            {
                return Count();
            }
            return this.dbSet.Count(filter);
        }

        public int Count()
        {
            return this.dbSet.Count();
        }


        public SearchResult<T> SearchFor(Expression<Func<T, bool>> where, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            return SearchFor(where, 0, 0, orderBy);
        }

        public SearchResult<T> SearchFor(Expression<Func<T, bool>> where, int skip, int take, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            var query = this.dbSet.AsQueryable();
            if (where != null)
            {
                query = query.Where(where);
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
                TotalItem = 0
            };
        }

    }
}
