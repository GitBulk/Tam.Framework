using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Tam.MongoDb.Model;
using Tam.Repository.Interface;

namespace Tam.MongoDb.Interface
{
    public interface IMongoRepository<T> where T : MongoBaseEntity
    {
        /// <summary>
        /// Add items/documents
        /// </summary>
        /// <param name="items">The items to add</param>
        void Add(System.Collections.Generic.IEnumerable<T> items);

        /// <summary>
        /// Add item/document
        /// </summary>
        /// <param name="item">The entity to add</param>
        /// <returns>The added item including its new ObjectId.</returns>
        T Add(T item);

        /// <summary>
        /// Returns collectio name of repository
        /// </summary>
        string CollectionName { get; }

        /// <summary>
        /// Counts the total items/document in the repository.
        /// </summary>
        /// <returns></returns>
        long Count();

        /// <summary>
        /// Counts the items/document which are matching the condition in the repository.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        long Count(System.Linq.Expressions.Expression<Func<T, bool>> condition);

        /// <summary>
        /// Delete an item/document in collection
        /// </summary>
        /// <param name="id">The value representing the ObjectId of the entity to delete.</param>
        /// <returns>Returns true for success, false otherwise.</returns>
        bool Delete(MongoDB.Bson.ObjectId id);

        /// <summary>
        /// Delete an item/document in collection
        /// </summary>
        /// <param name="item">The item to delete</param>
        /// <returns>Returns true for success, false otherwise.</returns>
        bool Delete(T item);

        /// <summary>
        /// Delete all document/item in collection
        /// </summary>
        void DeleteAll();

        /// <summary>
        /// Checks if the entity exists for given condition.
        /// </summary>
        /// <param name="condition">The expresion</param>
        /// <returns>Returns true when an entity matching the condition exists, false otherwise.</returns>
        bool Exists(System.Linq.Expressions.Expression<Func<T, bool>> condition);

        /// <summary>
        /// Get many items
        /// </summary>
        /// <param name="condition">The expression</param>
        /// <param name="orderBy">Orderby items</param>
        /// <returns>The items which are mathing the condition</returns>
        System.Collections.Generic.List<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> condition = null, Func<System.Linq.IQueryable<T>, System.Linq.IOrderedQueryable<T>> orderBy = null);

        /// <summary>
        /// Get item by id
        /// </summary>
        /// <param name="id">The value representing the ObjectId of the entity to retrieve.</param>
        /// <returns>The item T</returns>
        T GetById(MongoDB.Bson.ObjectId id);

        /// <summary>
        /// Set deleted status for item.
        /// </summary>
        /// <param name="item">Item to set deleted status</param>
        /// <returns>The item T after set deleted status</returns>
        T SetDeletedStatus(T item);

        /// <summary>
        /// Set deleted status for item.
        /// </summary>
        /// <param name="id">The value representing the ObjectId of the entity to delete.</param>
        /// <returns>The item T after set deleted status</returns>
        T SetDeletedStatus(ObjectId id);

        /// <summary>
        /// Update item
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns>The updated entity.</returns>
        T Update(T item);

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="query">MongoQuery to search</param>
        /// <param name="skip">Bypasses a specified number of elements in a sequence and then returns the remaining elements.</param>
        /// <param name="take">Returns a specified number of contiguous elements from the start of a sequence.</param>
        /// <returns>The items which are matching MongoQuery</returns>
        SearchResult SearchFor(IMongoQuery query, int skip, int take);

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="condition">The expresion</param>
        /// <param name="skip">Bypasses a specified number of elements in a sequence and then returns the remaining elements.</param>
        /// <param name="take">Returns a specified number of contiguous elements from the start of a sequence.</param>
        /// <returns>The items which are matching condition</returns>
        SearchResult SearchFor(Expression<Func<T, bool>> condition, int skip, int take = 12);

        /// <summary>
        /// Get many items
        /// </summary>
        /// <param name="condition">The expresion/where clause</param>
        /// <param name="orderBy">Orderby items</param>
        /// <returns>A List of T type</returns>
        List<T> GetItems(Expression<Func<T, bool>> condition, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
    }

}

