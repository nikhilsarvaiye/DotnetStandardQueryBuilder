namespace DotnetStandardQueryBuilder.Mongo
{
    using DotnetStandardQueryBuilder.Core;
    using DotnetStandardQueryBuilder.Mongo.Extensions;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class FilterDefinitionBuilder
    {
        private readonly IFilter _filter;

        public FilterDefinitionBuilder(IFilter filter)
        {
            _filter = filter;
        }

        internal FilterDefinition<T> Build<T>()
        {
            if (_filter != null)
            {
                return Build<T>(_filter);
            }
            return Builders<T>.Filter.Empty;
        }

        private FilterDefinition<T> Build<T>(IFilter filter)
        {
            if (filter == null)
            {
                return Builders<T>.Filter.Empty;
            }

            switch (filter)
            {
                case CompositeFilter _:

                    var compositeFilter = filter as CompositeFilter;

                    var compositeParameters = new Dictionary<string, object>();
                    var filterDefinitions = compositeFilter.Filters.Where(x => x != null).Select(x => Build<T>(x)).ToList();

                    return BuildCompositeFilterDefinition<T>(compositeFilter.LogicalOperator, filterDefinitions);

                case Filter _:

                    var _filter = filter as Filter;

                    return GetFilterExpression<T>(_filter);
            }

            return Builders<T>.Filter.Empty;
        }

        private FilterDefinition<T> BuildCompositeFilterDefinition<T>(LogicalOperator logicalOperator, IEnumerable<FilterDefinition<T>> filters)

        {
            switch (logicalOperator)
            {
                case LogicalOperator.And:
                    return Builders<T>.Filter.And(filters);

                case LogicalOperator.Or:
                    return Builders<T>.Filter.Or(filters);

                case LogicalOperator.Not:
                    return Builders<T>.Filter.Not(filters.FirstOrDefault());

                default:
                    throw new NotImplementedException(nameof(logicalOperator));
            }
        }

        private FilterDefinition<T> GetFilterExpression<T>(Filter filter)

        {
            var filterColumn = filter.Property.ToBsonProperty();
            var filterValue = filter.Value.ToBsonValue();

            switch (filter.Operator)
            {
                case FilterOperator.IsEqualTo:
                    return Builders<T>.Filter.Eq(filterColumn, filterValue);

                case FilterOperator.IsNotEqualTo:
                    return Builders<T>.Filter.Not(Builders<T>.Filter.Eq(filterColumn, filterValue));

                case FilterOperator.IsGreaterThan:
                    return Builders<T>.Filter.Gt(filterColumn, filterValue);

                case FilterOperator.IsGreaterThanOrEqualTo:
                    return Builders<T>.Filter.Gte(filterColumn, filterValue);

                case FilterOperator.IsLessThan:
                    return Builders<T>.Filter.Lt(filterColumn, filterValue);

                case FilterOperator.IsLessThanOrEqualTo:
                    return Builders<T>.Filter.Lte(filterColumn, filterValue);

                case FilterOperator.Contains:
                    return Builders<T>.Filter.Regex(filterColumn, new BsonRegularExpression($"{filterValue}", "i"));

                case FilterOperator.StartsWith:
                    return Builders<T>.Filter.Regex(filterColumn, new BsonRegularExpression($"^{filterValue}", "i"));

                case FilterOperator.EndsWith:
                    return Builders<T>.Filter.Regex(filterColumn, new BsonRegularExpression($"{filterValue}$", "i"));

                case FilterOperator.IsNull:
                    return Builders<T>.Filter.Exists(filterColumn, false);

                case FilterOperator.IsContainedIn:
                    return Builders<T>.Filter.In(filterColumn, filterValue.AsBsonArray);

                case FilterOperator.DoesNotContain:
                    return Builders<T>.Filter.Not(Builders<T>.Filter.In(filterColumn, filterValue.AsBsonArray));

                case FilterOperator.IsNotNull:
                    return Builders<T>.Filter.Exists(filterColumn);

                case FilterOperator.IsEmpty:
                    return Builders<T>.Filter.Eq(filterColumn, string.Empty);

                case FilterOperator.IsNotEmpty:
                    return Builders<T>.Filter.Not(Builders<T>.Filter.Eq(filterColumn, string.Empty));

                default:
                    throw new NotImplementedException(nameof(filter.Operator));
            }
        }
    }
}