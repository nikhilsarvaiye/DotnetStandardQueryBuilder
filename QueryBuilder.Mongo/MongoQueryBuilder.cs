namespace QueryBuilder
{
    using MongoDB.Driver;
    using QueryBuilder.Core;
    using QueryBuilder.Mongo.Extensions;
    using System;

    public class MongoQueryBuilder<T>
        where T : class
    {
        private IRequest _request;

        public MongoQueryBuilder(IRequest request)
        {
            _request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public IFindFluent<T, T> Query(IMongoCollection<T> collection)
        {
            if (_request == null)
            {
                return collection.Find(_ => true);
            }

            var filterDefinition = _request.Filter.ToFilterDefinition<T>();

            return collection.Find(filterDefinition)
                .Paginate(_request.Page, _request.PageSize)
                .Sort(_request.Sorts)
                .Project(_request.Select);
        }

        public IFindFluent<T, T> QueryCount<T>(IMongoCollection<T> collection)
        {
            var filterDefinition = _request.Filter.ToFilterDefinition<T>();

            return collection.Find(filterDefinition)
                .Sort(_request.Sorts)
                .Project(_request.Select);
        }
    }
}
