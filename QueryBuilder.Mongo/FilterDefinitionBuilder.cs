namespace QueryBuilder.Mongo
{
    using MongoDB.Bson;
    using MongoDB.Driver;
    using QueryBuilder.Core;
    using QueryBuilder.Mongo.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class FilterDefinitionBuilder
    {
        private readonly IFilter _filter;

        public FilterDefinitionBuilder(IFilter filter)
        {
            _filter = filter ?? throw new ArgumentNullException(nameof(filter));
        }

        internal FilterDefinition<T> Build<T>()
        {
            if (_filter != null)
            {
                return Build<T>(null, new List<IFilter> { _filter });
            }
            return Builders<T>.Filter.Empty;
        }

        private FilterDefinition<T> Build<T>(FilterDefinition<T> filterDefinition, List<IFilter> filters, LogicalOperator logicalOperator = LogicalOperator.And)

        {
            if (filters == null)
            {
                return null;
            }

            var filterDefinitions = new List<FilterDefinition<T>>();
            foreach (var filterItem in filters)
            {
                switch (filterItem)
                {
                    case CompositeFilter _:

                        var compositeFilter = filterItem as CompositeFilter;

                        filterDefinition = Build(filterDefinition,
                                                                    compositeFilter?.Filters,
                                                                    compositeFilter.LogicalOperator);
                        filterDefinitions.Add(filterDefinition);
                        break;

                    case Filter _:

                        var filter = filterItem as Filter;
                        filterDefinitions.Add(GetFilterExpression<T>(filter));
                        break;
                }
            }

            if (filterDefinitions.Count == 1)
            {
                if (logicalOperator == LogicalOperator.Not)
                {
                    return GetFilterExpression<T>(logicalOperator, filterDefinitions);
                }
                return filterDefinitions.FirstOrDefault();
            }
            else if (filterDefinitions.Count == 2)
            {
                return GetFilterExpression<T>(logicalOperator, filterDefinitions);
            }

            return filterDefinition;
        }

        private List<FilterDefinition<T>> ClubFilters<T>(List<FilterDefinition<T>> filterDefinitions, LogicalOperator logicalOperator)

        {
            if (filterDefinitions.Count <= 1)
            {
                return filterDefinitions;
            }

            var clubFilters = new List<FilterDefinition<T>>();

            foreach (var range in Enumerable.Range(0, filterDefinitions.Count - 2).Select(x => x))
            {
                clubFilters.Add(filterDefinitions[range]);
            }

            clubFilters.Add(GetFilterExpression<T>(logicalOperator, filterDefinitions));
            return clubFilters;
        }

        private FilterDefinition<T> GetFilterExpression<T>(LogicalOperator logicalOperator, IEnumerable<FilterDefinition<T>> filters)

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
                    return Builders<T>.Filter.Regex(filterColumn, new BsonRegularExpression($"^((?!{filterValue}).)*$", "i"));

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
