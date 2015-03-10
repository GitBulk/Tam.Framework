using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Configuration;
using Tam.Blog.Business.Interface;
using Tam.Blog.Cache.Interface;
using Tam.Blog.Repository.Interface;
using Tam.Database;
using Tam.Repository.Interface;

namespace Tam.Blog.Business.Implementation
{
    public abstract class BaseBusiness<T> : IBaseBusiness<T> where T : class
    {
        protected static int CacheTimeInMinute = 60; // 60 minutes

        /// <summary>
        /// CacheTimeOf{0}
        /// </summary>
        private static string CacheTimeFormat = "CacheTimeOfTable{0}";

        private IBaseRepository<T> baseRepository;
        private IBaseCache<T> baseCache;
        protected ISqlServerHelper sqlHelper;
        protected IUnitOfWork unitOfWork;

        static BaseBusiness()
        {
            SetCacheTime();
        }

        private static void SetCacheTime()
        {
            string appKey = string.Format(CacheTimeFormat, typeof(T).Name);
            string temp = Convert.ToString(ConfigurationManager.AppSettings[appKey]);
            if (string.IsNullOrWhiteSpace(temp) == false)
            {
                try
                {
                    if (Information.IsNumeric(temp))
                    {
                        CacheTimeInMinute = Convert.ToInt32(temp);
                    }
                }
                catch { CacheTimeInMinute = 60; }
            }
        }

        public BaseBusiness(IUnitOfWork unitOfWork, IBaseRepository<T> repository)
            : this(unitOfWork, repository, null, null)
        { }

        public BaseBusiness(IUnitOfWork unitOfWork, IBaseRepository<T> repository, ISqlServerHelper sqlHelper)
            : this(unitOfWork, repository, sqlHelper, null)
        {
        }

        public BaseBusiness(IUnitOfWork unitOfWork, IBaseRepository<T> repository, ISqlServerHelper sqlHelper,
            IBaseCache<T> baseCache)
        {
            this.unitOfWork = unitOfWork;
            this.baseRepository = repository;
            this.sqlHelper = sqlHelper;
            this.baseCache = baseCache;
        }

        public virtual int Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity");
            }
            this.baseRepository.Add(entity);
            return this.unitOfWork.SaveChanges();
        }

        public virtual int Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity");
            }
            this.baseRepository.Delete(entity);
            return this.unitOfWork.SaveChanges();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return this.baseRepository.GetAll();
        }

        public string GetAuthor()
        {
            return "ToanTN";
        }

        public virtual T GetItem(object id)
        {
            T item = this.baseRepository.GetById(id);
            return item;
        }

        public virtual int Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity");
            }
            this.baseRepository.Update(entity);
            return this.unitOfWork.SaveChanges();
        }
    }
}