using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tam.Repository.Contraction;

namespace Tam.Repository.MongoDb
{
    public class MongoRepository<T> : IMongoRepository<T> where T : MongoBaseEntity
    {
        // NOTE: Mongo driver didn't support Async API at version 1.0.
        private const string MongoConnectionStringKey = "MongoConnectionString";
        private MongoCollection<T> collection;
        public MongoCollection<T> Collection
        {
            get
            {
                if (collection == null)
                {
                    this.collection = GetCollection(this.database);
                }
                return this.collection;
            }
        }

        private MongoDatabase database;

        public MongoRepository()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[MongoConnectionStringKey].ToString();
            this.database = GetDatabase(connectionString, "blog");
        }

        public MongoRepository(string connectionString, string databaseName)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException("databaseName");
            }
            this.database = GetDatabase(connectionString, databaseName.ToLower());
        }

        public MongoRepository(IMongoSetting setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException("setting");
            }
            this.database = GetDatabase(setting.ConnectionString, setting.DatabaseName.ToLower());
        }

        public MongoRepository(MongoUrl url)
        {
            this.database = GetDatabase(url);
        }

        private MongoDatabase GetDatabase(MongoUrl url)
        {
            var client = new MongoClient(url);
            MongoServer server = client.GetServer();
            return server.GetDatabase(url.DatabaseName);
        }

        private MongoDatabase GetDatabase(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            MongoServer server = client.GetServer();
            return server.GetDatabase(databaseName.ToLower());
        }

        private MongoCollection<T> GetCollection(MongoDatabase database)
        {
            return database.GetCollection<T>(typeof(T).Name.ToLower());
        }

        public string CollectionName
        {
            get
            {
                return this.Collection.Name;
            }
        }

        public string GetBackgroundTechnology()
        {
            return "Mongo";
        }

        public Task AddAsync(T entity)
        {
            return Task.Run(() =>
                {
                    Add(entity);
                });
        }

        public Task<int> CountAsync()
        {
            return Task.Run<int>(() =>
                {
                    return Count();
                });
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> match)
        {
            // this is a fake async.
            // actually, It is c# async, not Mongo async.
            // Mongo driver didn't support Async API at version 1.0. So sad.
            return Task.Run<int>(() =>
                {
                    return Count(match);
                });
        }

        public Task<List<T>> GetAllAsync()
        {
            // this is a fake async.
            // actually, It is c# async, not Mongo async.
            // Mongo driver didn't support Async API at version 1.0. So sad.
            return Task.Run<List<T>>(() =>
                {
                    return GetAll().ToList();
                });
        }

        public Task<T> GetByIdAsync(object id)
        {
            return Task.Run<T>(() =>
                {
                    return GetById(id);
                });
        }

        public Task<T> GetItemAsync(Expression<Func<T, bool>> match)
        {
            return Task.Run<T>(() =>
                {
                    return GetItem(match);
                });
        }

        public Task DeleteAsync(T entity)
        {
            return Task.Run(() =>
                {
                    Delete(entity);
                });
        }

        public Task<SearchResult<T>> SearchForAsync(Expression<Func<T, bool>> where, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            return Task.Run<SearchResult<T>>(() =>
                {
                    return SearchFor(where, orderBy);
                });
        }

        public Task<SearchResult<T>> SearchForAsync(Expression<Func<T, bool>> where, int skip, int take, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            return Task.Run<SearchResult<T>>(() =>
            {
                return SearchFor(where, skip, take, orderBy);
            });
        }

        public Task UpdateAsync(T entity)
        {
            //WriteConcernResult result = this.Collection.Remove(Query<T>.EQ<ObjectId>(q => q.Id, entity.Id));
            // or
            //this.collection.Remove(Query.EQ("_id", id));
            //return result.DocumentsAffected == 1;

            return Task.Run(() =>
            {
                Update(entity);
            });
        }

        public virtual void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item is null");
            }
            item.Id = ObjectId.GenerateNewId();
            this.Collection.Insert<T>(item);
        }

        public int Count()
        {
            return Convert.ToInt32(this.Collection.Count());
        }

        public int Count(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter is null");
            }
            var query = this.collection.AsQueryable<T>().Where(filter);
            return Convert.ToInt32(query.Count());
        }

        protected ObjectId ConvertToMongoObjectId(object id)
        {
            if (id.GetType() == typeof(string))
            {
                var oid = new ObjectId(id as string);
                return oid;
            }
            return BsonValue.Create(id).AsObjectId;
        }

        public void Delete(object id)
        {
            if (id.GetType() == typeof(string))
            {
                var oid = new ObjectId(id as string);
                //WriteConcernResult result = this.Collection.Remove(Query<T>.EQ<ObjectId>(q => q.Id, oid));
                // or
                WriteConcernResult result = this.Collection.Remove(Query.EQ("_id", oid));
            }
            else
            {
                //WriteConcernResult result = this.Collection.Remove(Query<T>.EQ<ObjectId>(q => q.Id, BsonValue.Create(id).AsObjectId));
                WriteConcernResult result = this.Collection.Remove(Query.EQ("_id", BsonValue.Create(id)));
            }
        }

        public void Delete(T item)
        {
            WriteConcernResult result = this.Collection.Remove(Query<T>.EQ<ObjectId>(q => q.Id, item.Id));
            // or
            //this.collection.Remove(Query.EQ("_id", id));
        }

        public void Dispose()
        {
        }

        public virtual IEnumerable<T> GetItems(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            var query = this.Collection.AsQueryable<T>();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // example
            // Func<IQueryable<Product>, IOrderedQueryable<Product>> orderingFunc = query => query.OrderBy(p => p.Price);
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query.ToList();
        }

        public IEnumerable<T> GetAll()
        {
            return this.Collection.AsQueryable<T>().ToList();
        }

        public virtual T GetById(object id)
        {
            T result = default(T);
            BsonValue value = ConvertToMongoObjectId(id);
            result = this.Collection.FindOneByIdAs<T>(value);
            //if (id.GetType() == typeof(string))
            //{
            //    var oid = new ObjectId(id as string);
            //    result = this.Collection.FindOneByIdAs<T>(oid);
            //}
            //else
            //{
            //    result = this.Collection.FindOneByIdAs<T>(BsonValue.Create(id));
            //}
            return result;
        }

        [Obsolete("Mongo doesn't support many keys")]
        public T GetById(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public virtual T GetItem(Expression<Func<T, bool>> where)
        {
            var query = this.Collection.AsQueryable<T>();
            if (where != null)
            {
                query = query.Where(where);
            }
            return query.FirstOrDefault();
        }

        public SearchResult<T> SearchFor(Expression<Func<T, bool>> where, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            return SearchFor(where, orderBy);
        }

        public SearchResult<T> SearchFor(Expression<Func<T, bool>> where, int skip, int take, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            var searchResult = new SearchResult<T>();
            if (take < 1)
            {
                return searchResult;
            }
            var query = this.Collection.AsQueryable<T>();
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
            var result = query.Take(take).ToList<T>();
            searchResult.Data = result;
            searchResult.PageSize = take;
            searchResult.TotalItem = count;
            return searchResult;
        }

        public virtual void Update(T item)
        {
            var query = Query<T>.EQ(i => i.Id, item.Id);
            var update = Update<T>.Replace(item);
            WriteConcernResult result = this.Collection.Update(query, update);
        }

        protected virtual SearchResult<T> SearchFor(IMongoQuery query, int skip, int take)
        {
            int count = Convert.ToInt32(this.Collection.Find(query).Count());
            var result = this.Collection.Find(query).SetSkip(skip).SetLimit(take).ToList<T>();
            return new SearchResult<T>
            {
                Data = result,
                PageSize = take,
                TotalItem = count
            };
        }


        public Task<List<T>> GetItemsAsync(Expression<Func<T, bool>> match)
        {
            return Task.Run<List<T>>(() =>
                {
                    return GetItems(match).ToList();
                });
        }
    }
}