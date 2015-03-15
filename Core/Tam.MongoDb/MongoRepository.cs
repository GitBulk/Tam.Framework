using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tam.MongoDb
{
    public class MongoRepository<T> : IMongoRepository<T> where T : MongoBaseEntity
    {
        MongoCollection<T> collection;
        MongoDatabase database;
        public MongoRepository(string connectionString)
        {
            this.database = GetDatabase(connectionString);
            this.collection = GetCollection(this.database);
        }

        public MongoRepository(MongoUrl url)
        {
            this.database = GetDatabase(url);
            this.collection = GetCollection(this.database);
        }

        private MongoDatabase GetDatabase(MongoUrl url)
        {
            var client = new MongoClient(url);
            MongoServer server = client.GetServer();
            return server.GetDatabase(url.DatabaseName);
        }

        private MongoDatabase GetDatabase(string connectionString)
        {
            var client = new MongoClient(connectionString);
            MongoServer server = client.GetServer();
            string databaseName = new MongoUrl(connectionString).DatabaseName;
            return server.GetDatabase(databaseName);
        }

        private MongoCollection<T> GetCollection(MongoDatabase database)
        {
            return database.GetCollection<T>(typeof(T).Name);
        }

        public string CollectionName
        {
            get
            {
                return this.collection.Name;
            }
        }

        public virtual T Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item is null");
            }
            item.Id = ObjectId.GenerateNewId();
            this.collection.Insert<T>(item);
            return item;
        }

        public virtual void Add(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("item is null");
            }
            if (items.Count() > 0)
            {
                this.collection.InsertBatch<T>(items);
            }
        }

        public virtual T Update(T item)
        {
            var query = Query<T>.EQ(i => i.Id, item.Id);
            var update = Update<T>.Replace(item);
            WriteConcernResult result = this.collection.Update(query, update);
            return item;
        }

        public virtual T SetDeletedStatus(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item is null");
            }
            item.Deleted = true;
            return Update(item);
        }

        public virtual bool Delete(ObjectId id)
        {
            WriteConcernResult result = this.collection.Remove(Query<T>.EQ<ObjectId>(q => q.Id, id));
            // or
            //this.collection.Remove(Query.EQ("_id", id));
            return result.DocumentsAffected == 1;
        }

        public virtual bool Delete(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item is null");
            }
            return Delete(item.Id);
        }

        public virtual void DeleteAll()
        {
            this.database.DropCollection(this.CollectionName); // DropCollection runs faster
            // or
            //this.collection.RemoveAll();
        }

        public virtual T GetById(ObjectId id)
        {
            T result = this.collection.FindOneByIdAs<T>(id);
            return result;
        }

        /// <summary>
        /// Counts all documents in this collection.
        /// </summary>
        /// <returns></returns>
        public virtual long Count()
        {
            return this.collection.Count();
        }

        public virtual long Count(Expression<Func<T, bool>> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException("condition is null");
            }
            var query = this.collection.AsQueryable<T>().Where(condition);
            return query.LongCount();
        }

        public virtual List<T> Get(Expression<Func<T, bool>> condition = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            var query = this.collection.AsQueryable<T>();
            if (condition != null)
            {
                query = query.Where(condition);
            }

            // example
            // Func<IQueryable<Product>, IOrderedQueryable<Product>> orderingFunc = query => query.OrderBy(p => p.Price);
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.ToList();
        }

        public virtual bool Exists(Expression<Func<T, bool>> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException("condition is null");
            }
            bool result = this.collection.AsQueryable<T>().Any(condition);
            return result;
        }

        public virtual SearchResult SearchFor(IMongoQuery query, int skip, int take)
        {
            long count = this.collection.Find(query).Count();
            var results = this.collection.Find(query).SetSkip(skip).SetLimit(take).ToList<MongoBaseEntity>();
            return new SearchResult
            {
                TotalItem = count,
                TotalResult = results
            };
        }

        public virtual SearchResult SearchFor(Expression<Func<T, bool>> condition, int skip, int take = 12)
        {
            return SearchFor(condition, skip, take, null);
        }

        public virtual SearchResult SearchFor(Expression<Func<T, bool>> condition, int skip, int take = 12, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            var searchResult = new SearchResult();
            if (take < 1)
            {
                return searchResult;
            }
            var query = this.collection.AsQueryable<T>();
            if (condition != null)
            {
                query = query.Where(condition);
            }
            long count = query.LongCount();
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (skip > 0)
            {
                query = query.Skip(skip);
            }
            var result = query.Take(take).ToList<MongoBaseEntity>();

            searchResult.TotalItem = count;
            searchResult.TotalResult = result;

            return searchResult;
        }

        public virtual List<T> GetItems(Expression<Func<T, bool>> condition, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            var query = this.collection.AsQueryable<T>();
            if (condition != null)
            {
                query = query.Where(condition);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.ToList<T>();
        }
    }
}