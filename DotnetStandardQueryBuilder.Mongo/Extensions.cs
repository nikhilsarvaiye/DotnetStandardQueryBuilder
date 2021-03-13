namespace DotnetStandardQueryBuilder.Mongo.Extensions
{
    using MongoDB.Bson;
    using MongoDB.Driver;
    using DotnetStandardQueryBuilder.Core;
    using System.Collections.Generic;

    public static class Extensions
    {
        public static IFindFluent<T, T> Query<T>(this IMongoCollection<T> collection, IRequest request)
            where T : class
        {
            return new MongoQueryBuilder<T>(request, collection).Query();
        }

        public static IFindFluent<T, T> QueryCount<T>(this IMongoCollection<T> collection, IRequest request)
            where T : class
        {
            return new MongoQueryBuilder<T>(request, collection).QueryCount();
        }

        public static IFindFluent<T, T> Query<T>(this IRequest request, IMongoCollection<T> collection)
            where T : class
        {
            return new MongoQueryBuilder<T>(request, collection).Query();
        }

        public static IFindFluent<T, T> QueryCount<T>(this IRequest request, IMongoCollection<T> collection)
            where T : class
        {
            return new MongoQueryBuilder<T>(request, collection).QueryCount();
        }

        public static FilterDefinition<T> ToFilterDefinition<T>(this IFilter filter)
        {
            return new FilterDefinitionBuilder(filter).Build<T>();
        }
        
        public static IFindFluent<T, T> Project<T>(this IFindFluent<T, T> query, List<string> select)
        {
            return new ProjectBuilder(select).Build(query);
        }

        public static IFindFluent<T, T> Sort<T>(this IFindFluent<T, T> query, List<Sort> sorts)
        {
            return new SortBuilder(sorts).Build(query);
        }

        public static IFindFluent<T, T> Paginate<T>(this IFindFluent<T, T> query, int page, int? pageSize)
        {
            return new PaginateBuilder(page, pageSize).Build(query);
        }

        public static string ToBsonProperty(this string property)
        {
            return new BsonPropertyBuilder(property).Build();
        }
        
        public static BsonValue ToBsonValue(this object value)
        {
            return new BsonValueBuilder(value).Build();
        }
    }
}
