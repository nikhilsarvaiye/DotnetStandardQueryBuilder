namespace DotnetStandardQueryBuilder
{
    using MongoDB.Driver;
    using DotnetStandardQueryBuilder.Core;
    using DotnetStandardQueryBuilder.Mongo.Extensions;
    using System;

    public class MongoQueryBuilder<T> : IQueryBuilder<T, IFindFluent<T, T>>
        where T : class
    {
        private IMongoCollection<T> _collection;

        public IRequest Request { get; }

        public MongoQueryBuilder(IRequest request, IMongoCollection<T> collection)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        public IFindFluent<T, T> Query()
        {
            if (Request == null)
            {
                return _collection.Find(_ => true);
            }

            var filterDefinition = Request.Filter.ToFilterDefinition<T>();

            return _collection.Find(filterDefinition)
                .Sort(Request.Sorts)
                .Paginate(Request.Page, Request.PageSize)
                .Project(Request.Select);
        }

        public IFindFluent<T, T> QueryCount()
        {
            var filterDefinition = Request.Filter.ToFilterDefinition<T>();

            return _collection.Find(filterDefinition)
                .Sort(Request.Sorts)
                .Project(Request.Select);
        }
    }
}
