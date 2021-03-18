namespace DotnetStandardQueryBuilder.Sql
{
    using DotnetStandardQueryBuilder.Core;
    using DotnetStandardQueryBuilder.Sql.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class UpdateBuilder
    {
        private readonly IRequest _request;

        public UpdateBuilder(IRequest request)
        {
            _request = request ?? throw new ArgumentNullException(nameof(request));
        }

        internal SqlQuery Build<T>(List<T> items, string tableName, List<string> excludeColumns = null)
        {
            if (items?.Count == 0)
            {
                return null;
            }

            if (excludeColumns == null)
            {
                excludeColumns = new List<string>();
            }

            var where = new SqlExpression(_request).Where();

            var valuesQuery = Build(items, excludeColumns);

            var values = new List<KeyValuePair<string, object>>();
            values.AddRange(valuesQuery.Values);
            values.AddRange(where.Values);

            var whereClause = !string.IsNullOrEmpty(where.WhereClause) ? $" WHERE {where.WhereClause}" : string.Empty;

            return new SqlQuery
            {
                Query = $"UPDATE {tableName} {valuesQuery.Expression} {whereClause}",
                Values = values.ToDictionary()
            };
        }

        private WhereClause Build<T>(List<T> items, List<string> excludeColumns)
        {
            int counter = 1;

            var valueQueries = new List<string>();
            var values = new List<KeyValuePair<string, object>>();

            foreach (var item in items)
            {
                var value = GetUpdateSqlColumnValues(item, counter, excludeColumns);

                valueQueries.Add($"SET {value.Key}");

                values.AddRange(value.Value);

                counter++;
            }

            var expression = string.Join(" ", valueQueries);

            return new WhereClause
            {
                Expression = expression,
                Values = values
            };
        }

        private KeyValuePair<string, List<KeyValuePair<string, object>>> GetUpdateSqlColumnValues<T>(T t, int counter, List<string> excludeColumns)
        {
            int valueCounter = 1;

            var valueNames = new List<string>();
            // var names = new List<string>();
            var dictionary = new Dictionary<string, object>();
            foreach (var property in t.GetType().GetProperties().Where(x => !excludeColumns.Any(y => y.ToLower() == x.Name.ToLowerInvariant())))
            {
                var name = $"{property.Name}{counter}{valueCounter}";

                dictionary[name] = property.GetValue(t)?.ToValue();

                valueNames.Add($"{property.Name}=@{name}");
                valueCounter++;
            }

            var columnValueNames = string.Join(",", valueNames);
            var values = dictionary.ToKeyValuePairList();

            return new KeyValuePair<string, List<KeyValuePair<string, object>>>(columnValueNames, values);
        }
    }
}