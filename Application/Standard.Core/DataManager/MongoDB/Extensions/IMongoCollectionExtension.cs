using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Standard.Core.DataManager.MongoDB.Extensions
{
    public static class IMongoCollectionExtension
    {
        public static async Task<List<TDocument>> FindAsynchronous<TDocument>(
            this IMongoCollection<TDocument> collection, Expression<Func<TDocument, bool>> filter)
        {
            return await collection.Find(filter).ToListAsync();
        }

        public static async Task<List<TProjection>> FindAndProject<TDocument, TProjection>(
            this IMongoCollection<TDocument> collection, Expression<Func<TDocument, bool>> filter,
            Expression<Func<TDocument, TProjection>> projection)
        {
            return await collection.Find(filter).Project(projection).ToListAsync();
        }
    }
}