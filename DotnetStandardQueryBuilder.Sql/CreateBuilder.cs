namespace DotnetStandardQueryBuilder.Sql
{
    using DotnetStandardQueryBuilder.Core;
    using DotnetStandardQueryBuilder.Sql.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class CreateBuilder
    {
        private readonly IRequest _request;

        public CreateBuilder(IRequest request)
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

            var columnsQuery = GetInsertSqlColumnNames(items.FirstOrDefault(), excludeColumns);

            var valuesQuery = Build(items, excludeColumns);

            return new SqlQuery
            {
                Query = $"INSERT INTO {tableName} {columnsQuery} {valuesQuery.Expression}",
                Values = valuesQuery.Values?.ToDictionary()
            };
        }

        private WhereClause Build<T>(List<T> items, List<string> excludeColumns)
        {
            int counter = 1;

            var valueQueries = new List<string>();
            var values = new List<KeyValuePair<string, object>>();

            foreach (var item in items)
            {
                var value = GetInsertSqlColumnValues(item, counter, excludeColumns);

                valueQueries.Add(value.Key);

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

        private KeyValuePair<string, List<KeyValuePair<string, object>>> GetInsertSqlColumnValues<T>(T t, int counter, List<string> excludeColumns)
        {
            int valueCounter = 1;

            var valueNames = new List<string>();
            var dictionary = new Dictionary<string, object>();

            foreach (var property in t.GetType().GetProperties().Where(x => !excludeColumns.Any(y => y.ToLower() == x.Name.ToLowerInvariant())))
            {
                var name = $"{property.Name}{counter}{valueCounter}";

                dictionary[name] = property.GetValue(t)?.ToValue();

                valueNames.Add($"@{name}");
                valueCounter++;
            }

            var columnValueNames = $"VALUES ({string.Join(",", valueNames)})";
            var values = dictionary.ToKeyValuePairList();

            return new KeyValuePair<string, List<KeyValuePair<string, object>>>(columnValueNames, values);
        }

        private string GetInsertSqlColumnNames<T>(T t, List<string> excludeColumns)
        {
            return $@"({string.Join(",", t.GetType().GetProperties()
                .Where(x => !excludeColumns.Any(y => y.ToLower() == x.Name.ToLowerInvariant()))
                .Select(x => x.Name))})";
        }
    }
}