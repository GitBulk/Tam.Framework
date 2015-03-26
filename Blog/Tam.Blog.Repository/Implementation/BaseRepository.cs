using Microsoft.VisualBasic;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Tam.Blog.Repository.Interface;
using Tam.Database;
using Tam.Repository.EntityFramework;

namespace Tam.Blog.Repository.Implementation
{
    //public class BaseRepository<T> : EFBaseRepository<T>, IEntityFrameworkRepository<T> where T : class
    //{
    //    public BaseRepository(DbContext context)
    //        : this(context, false, null, null)
    //    { }

    //    public BaseRepository(DbContext context, ISqlServerHelper sqlHelper)
    //        : this(context, false, sqlHelper, null)
    //    { }

    //    public BaseRepository(DbContext context, bool isSaveChage, ISqlServerHelper sqlHelper,
    //        IUnitOfWork unitOfWork)
    //        : base(context, isSaveChage, sqlHelper, unitOfWork)
    //    {

    //    }

    //    private static int GetMaxPageSize()
    //    {
    //        int maxPageSize = 50;
    //        try
    //        {
    //            string temp = Convert.ToString(ConfigurationManager.AppSettings["MaxPageSize"]);
    //            if (string.IsNullOrWhiteSpace(temp) == false)
    //            {
    //                if (Information.IsNumeric(temp))
    //                {
    //                    int tempPageSize = Convert.ToInt32(temp);
    //                    if (tempPageSize > 0)
    //                    {
    //                        maxPageSize = tempPageSize;
    //                    }
    //                }
    //            }
    //        }
    //        catch
    //        {
    //            maxPageSize = 50;
    //        }
    //        return maxPageSize;
    //    }

    //    protected override int MaxPageSize
    //    {
    //        get
    //        {
    //            int temp = GetMaxPageSize();
    //            return temp;
    //        }
    //    }

    //    public string GetEFConnectionString()
    //    {
    //        var temp = ConfigurationManager.ConnectionStrings["GreatBlogEntities"];
    //        if (temp != null)
    //        {
    //            return temp.ToString();
    //        }
    //        return "";
    //    }

    //    public string GetSqlConnectionString()
    //    {
    //        //return ConnectionUtil.GetSqlConnectionString();
    //        //return ConfigurationManager.ConnectionStrings["TestEntities"].ToString();
    //        //return @"Data Source=TOAN-PC\SQLEXPRESS;initial catalog=Test1.1;persist security info=True;user id=sa;password=123456;multipleactiveresultsets=True;";

    //        var temp = ConfigurationManager.ConnectionStrings["GreatBlogSql"];
    //        if (temp != null)
    //        {
    //            return temp.ToString();
    //        }
    //        return "";
    //    }
    //}

    //public class BaseRepository<T> : IBaseRepository<T> where T : class
    //{
    //    protected static int MaxPageSize = 50;
    //    protected ISqlServerHelper sqlHelper;

    //    protected IUnitOfWork unitOfWork;

    //    //protected GreatBlogEntities context;
    //    //private GreatBlogEntities context;
    //    private DbContext context;

    //    //protected DbSet<T> dbSet;
    //    private DbSet<T> dbSet;

    //    private bool disposed = false;

    //    static BaseRepository()
    //    {
    //        GetMaxPageSize();
    //    }

    //    private static void GetMaxPageSize()
    //    {
    //        try
    //        {
    //            string temp = Convert.ToString(ConfigurationManager.AppSettings["MaxPageSize"]);
    //            if (string.IsNullOrWhiteSpace(temp) == false)
    //            {
    //                if (Information.IsNumeric(temp))
    //                {
    //                    int tempPageSize = Convert.ToInt32(temp);
    //                    if (tempPageSize > 0)
    //                    {
    //                        MaxPageSize = tempPageSize;
    //                    }
    //                }
    //            }
    //        }
    //        catch
    //        {
    //            MaxPageSize = 50;
    //        }
    //    }

    //    public BaseRepository(DbContext context)
    //        : this(context, false, null, null)
    //    { }

    //    public BaseRepository(DbContext context, ISqlServerHelper sqlHelper)
    //        : this(context, false, sqlHelper, null)
    //    { }

    //    public BaseRepository(DbContext context, bool isSaveChage, ISqlServerHelper sqlHelper,
    //        IUnitOfWork unitOfWork)
    //    {
    //        if (context == null)
    //        {
    //            throw new ArgumentNullException("Context");
    //        }
    //        Log = LogManager.GetLogger(this.GetType().FullName);
    //        this.IsSaveChanges = IsSaveChanges;
    //        this.sqlHelper = sqlHelper;
    //        this.context = context;
    //        this.dbSet = context.Set<T>();
    //        this.unitOfWork = unitOfWork;
    //    }

    //    public bool IsSaveChanges { get; set; }

    //    protected Logger Log { get; private set; }

    //    public virtual void Add(T entityToInsert)
    //    {
    //        this.dbSet.Add(entityToInsert);
    //        SaveChanges();
    //    }

    //    public virtual async Task AddAsync(T entity)
    //    {
    //        if (entity == null)
    //        {
    //            return;
    //        }
    //        this.dbSet.Add(entity);
    //        await SaveChangesAsync();
    //    }

    //    public virtual async Task<int> CountAsync()
    //    {
    //        return await this.dbSet.CountAsync();
    //    }

    //    public virtual async Task<int> CountAsync(System.Linq.Expressions.Expression<Func<T, bool>> match)
    //    {
    //        return await this.dbSet.CountAsync(match);
    //    }

    //    public virtual void Delete(object id)
    //    {
    //        T entityToDelete = GetById(id);
    //        if (entityToDelete != null)
    //        {
    //            Delete(entityToDelete);
    //        }
    //    }

    //    public virtual void Delete(T entityToDelete)
    //    {
    //        if (context.Entry(entityToDelete).State == EntityState.Detached)
    //        {
    //            dbSet.Attach(entityToDelete);
    //        }
    //        dbSet.Remove(entityToDelete);
    //        SaveChanges();
    //    }

    //    public void Dispose()
    //    {
    //        Dispose(true);
    //        GC.SuppressFinalize(this);
    //    }

    //    public IEnumerable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
    //    {
    //        IQueryable<T> query = dbSet;
    //        if (filter != null)
    //        {
    //            query = query.Where(filter);
    //        }
    //        foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
    //        {
    //            query = query.Include(includeProperty);
    //        }
    //        if (orderBy != null)
    //        {
    //            return orderBy(query).ToList();
    //        }
    //        else
    //        {
    //            return query.ToList();
    //        }
    //    }

    //    public virtual IEnumerable<T> GetAll()
    //    {
    //        return dbSet.ToList();
    //    }

    //    public virtual async Task<List<T>> GetAllAsync()
    //    {
    //        return await this.dbSet.ToListAsync();
    //    }

    //    public virtual T GetById(object id)
    //    {
    //        return this.dbSet.Find(id);
    //    }

    //    public virtual T GetById(params object[] keyValues)
    //    {
    //        return this.dbSet.Find(keyValues);
    //    }

    //    /// <summary>
    //    /// Get an entity by id asynchronous
    //    /// </summary>
    //    /// <param name="id">id (primary key)</param>
    //    /// <returns>Entity</returns>
    //    public virtual async Task<T> GetByIdAsync(object id)
    //    {
    //        return await this.dbSet.FindAsync(id);
    //    }

    //    public string GetEFConnectionString()
    //    {
    //        var temp = ConfigurationManager.ConnectionStrings["GreatBlogEntities"];
    //        if (temp != null)
    //        {
    //            return temp.ToString();
    //        }
    //        return "";
    //    }

    //    public T GetItem(System.Linq.Expressions.Expression<Func<T, bool>> match)
    //    {
    //        var query = this.dbSet.AsQueryable();
    //        if (match != null)
    //        {
    //            query = query.Where(match);
    //        }
    //        return this.dbSet.FirstOrDefault();
    //    }

    //    public virtual async Task<T> GetItemAsync(System.Linq.Expressions.Expression<Func<T, bool>> match)
    //    {
    //        var query = this.dbSet.AsQueryable();
    //        if (match != null)
    //        {
    //            query = query.Where(match);
    //        }
    //        return await this.dbSet.FirstOrDefaultAsync();
    //    }

    //    public async Task<List<T>> GetItemsAsync(System.Linq.Expressions.Expression<Func<T, bool>> match)
    //    {
    //        var query = this.dbSet.AsQueryable();
    //        if (match != null)
    //        {
    //            query = query.Where(match);
    //        }
    //        return await query.ToListAsync();
    //    }

    //    public string GetSqlConnectionString()
    //    {
    //        //return ConnectionUtil.GetSqlConnectionString();
    //        //return ConfigurationManager.ConnectionStrings["TestEntities"].ToString();
    //        //return @"Data Source=TOAN-PC\SQLEXPRESS;initial catalog=Test1.1;persist security info=True;user id=sa;password=123456;multipleactiveresultsets=True;";

    //        var temp = ConfigurationManager.ConnectionStrings["GreatBlogSql"];
    //        if (temp != null)
    //        {
    //            return temp.ToString();
    //        }
    //        return "";
    //    }

    //    public virtual async Task RemoveAsync(T entity)
    //    {
    //        if (entity == null)
    //        {
    //            return;
    //        }
    //        if (this.context.Entry(entity).State == EntityState.Detached)
    //        {
    //            this.dbSet.Attach(entity);
    //        }
    //        this.dbSet.Remove(entity);
    //        await SaveChangesAsync();
    //    }

    //    public IQueryable<T> SearchFor(System.Linq.Expressions.Expression<Func<T, bool>> where, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
    //    {
    //        return SearchFor(where, 0, 0, orderBy);
    //    }

    //    public IQueryable<T> SearchFor(System.Linq.Expressions.Expression<Func<T, bool>> where, int skip, int take, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
    //    {
    //        var query = this.dbSet.AsQueryable();
    //        if (where != null)
    //        {
    //            query = query.Where(where);
    //        }
    //        if (orderBy != null)
    //        {
    //            query = orderBy(query);
    //        }
    //        if (skip > 0)
    //        {
    //            query = query.Skip(skip);
    //        }
    //        if (take > MaxPageSize)
    //        {
    //            take = MaxPageSize;
    //        }
    //        if (take > 0)
    //        {
    //            query = query.Take(take);
    //        }
    //        return query;
    //    }

    //    public virtual async Task<List<T>> SearchForAsync(System.Linq.Expressions.Expression<Func<T, bool>> where,
    //        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
    //    {
    //        return await SearchForAsync(where, 0, 0, orderBy);
    //    }

    //    public virtual async Task<List<T>> SearchForAsync(System.Linq.Expressions.Expression<Func<T, bool>> where,
    //        int skip, int take, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
    //    {
    //        var query = this.dbSet.AsQueryable();
    //        if (where != null)
    //        {
    //            query = query.Where(where);
    //        }
    //        if (orderBy != null)
    //        {
    //            query = orderBy(query);
    //        }
    //        if (skip > 0)
    //        {
    //            query = query.Skip(skip);
    //        }
    //        if (take > MaxPageSize)
    //        {
    //            take = MaxPageSize;
    //        }
    //        if (take > 0)
    //        {
    //            query = query.Take(take);
    //        }
    //        return await query.ToListAsync();
    //    }

    //    public virtual void Update(T entityToUpdate)
    //    {
    //        dbSet.Attach(entityToUpdate);
    //        context.Entry(entityToUpdate).State = EntityState.Modified;
    //        SaveChanges();
    //    }

    //    public virtual async Task UpdateAsync(T entity)
    //    {
    //        if (entity == null)
    //        {
    //            return;
    //        }
    //        this.dbSet.Attach(entity);
    //        this.context.Entry(entity).State = EntityState.Modified;
    //        await SaveChangesAsync();
    //    }

    //    protected virtual void Dispose(bool disposing)
    //    {
    //        if (!this.disposed)
    //        {
    //            if (disposing)
    //            {
    //                this.context.Dispose();
    //            }
    //        }
    //        this.disposed = true;
    //    }

    //    private int SaveChanges()
    //    {
    //        if (this.IsSaveChanges)
    //        {
    //            return this.context.SaveChanges();
    //        }
    //        return 0;
    //    }

    //    private async Task<int> SaveChangesAsync()
    //    {
    //        if (this.IsSaveChanges)
    //        {
    //            return await this.context.SaveChangesAsync();
    //        }
    //        return 0;
    //    }
    //}
}