namespace DotnetStandardQueryBuilder.AzureCosmosSql
{
    using DotnetStandardQueryBuilder.Core;
    using DotnetStandardQueryBuilder.Sql;
    using DotnetStandardQueryBuilder.Sql.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class WhereBuilder
    {
        private readonly SqlExpression _sqlQuery;

        private int valuesCounter = 1;

        public WhereBuilder(SqlExpression sqlQuery)
        {
            _sqlQuery = sqlQuery ?? throw new ArgumentNullException(nameof(sqlQuery));
        }

        internal SqlExpression Build()
        {
            if (_sqlQuery.Request.Filter == null)
            {
                return null;
            }

            valuesCounter = 1;
            var whereClause = Build(_sqlQuery.Request.Filter);
            _sqlQuery.WhereClause = whereClause.Expression;
            _sqlQuery.Values = whereClause.Values.ToDictionary();
            return _sqlQuery;
        }

        private WhereClause Build(IFilter filter)
        {
            if (filter == null)
            {
                return null;
            }

            var whereClause = new WhereClause();

            switch (filter)
            {
                case CompositeFilter _:

                    var compositeFilter = filter as CompositeFilter;

                    var compositeParameters = new Dictionary<string, object>();
                    var values = compositeFilter.Filters.Where(x => x != null).Select(x => Build(x)).ToList();

                    whereClause.Expression = $"({string.Join($" {compositeFilter.LogicalOperator} ", values.Select(x => x.Expression))})";
                    whereClause.Values = values.Select(x => x.Values).ToFlatList();

                    break;

                case Filter _:

                    var _filter = filter as Filter;

                    var whereFilter = GetWhereFilter(_filter);
                    whereClause.Expression = whereFilter.Key;
                    whereClause.Values = new List<KeyValuePair<string, object>>() { whereFilter.Value };
                    valuesCounter++;
                    break;
            }

            return whereClause;
        }

        private KeyValuePair<string, KeyValuePair<string, object>> GetWhereFilter(Filter filter)
        {
            string expression;
            var filterColumn = filter.Property.ToColumn();
            var value = filter.ToValue();
            var filterValue = new KeyValuePair<string, object>($"@{filterColumn}{valuesCounter}", value);

            switch (filter.Operator)
            {
                case FilterOperator.IsEqualTo:
                    expression = $"{filterColumn}={filterValue.Value}";
                    break;

                case FilterOperator.IsNotEqualTo:
                    expression = $"{filterColumn}!={filterValue.Value}";
                    break;

                case FilterOperator.IsGreaterThan:
                    expression = $"{filterColumn}>{filterValue.Value}";
                    break;

                case FilterOperator.IsGreaterThanOrEqualTo:
                    expression = $"{filterColumn}>={filterValue.Value}";
                    break;

                case FilterOperator.IsLessThan:
                    expression = $"{filterColumn}<{filterValue.Value}";
                    break;

                case FilterOperator.IsLessThanOrEqualTo:
                    expression = $"{filterColumn}<={filterValue.Value}";
                    break;

                case FilterOperator.Contains:
                    expression = $"CONTAINS(LOWER({filterColumn}),LOWER({filterValue.Value}))";
                    expression += " OR ";
                    expression += $"ARRAY_CONTAINS({filterColumn},{filterValue.Value})";
                    break;

                case FilterOperator.StartsWith:
                    expression = $"STARTSWITH(LOWER({filterColumn}),LOWER({filterValue.Value}))";
                    break;

                case FilterOperator.EndsWith:
                    expression = $"ENDSWITH(LOWER({filterColumn}),LOWER({filterValue.Value}))";
                    break;

                case FilterOperator.IsNull:
                    expression = $"IS_NULL({filterColumn})";
                    break;

                case FilterOperator.IsContainedIn:
                    expression = $"ARRAY_CONTAINS({filterColumn},{filterValue.Value})";
                    break;

                case FilterOperator.DoesNotContain:
                    throw new NotImplementedException(nameof(filter.Operator));
                case FilterOperator.IsNotNull:
                    throw new NotImplementedException(nameof(filter.Operator));
                case FilterOperator.IsEmpty:
                    throw new NotImplementedException(nameof(filter.Operator));
                case FilterOperator.IsNotEmpty:
                    throw new NotImplementedException(nameof(filter.Operator));
                default:
                    throw new NotImplementedException(nameof(filter.Operator));
            }

            return new KeyValuePair<string, KeyValuePair<string, object>>(expression, filterValue);
        }
    }
}