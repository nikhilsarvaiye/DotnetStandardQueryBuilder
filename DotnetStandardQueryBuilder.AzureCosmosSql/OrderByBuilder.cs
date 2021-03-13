namespace DotnetStandardQueryBuilder.AzureCosmosSql
{
    using DotnetStandardQueryBuilder.Core;
    using DotnetStandardQueryBuilder.Sql;
    using System;
    using System.Linq;

    public class OrderByBuilder
    {
        private readonly SqlExpression _sqlQuery;

        public OrderByBuilder(SqlExpression sqlQuery)
        {
            _sqlQuery = sqlQuery ?? throw new ArgumentNullException(nameof(sqlQuery));
        }

        public SqlExpression Build()
        {
            if ((_sqlQuery.Request.Sorts == null) || (_sqlQuery.Request.Sorts.Count > 1))
            {
                return _sqlQuery;
            }

            var orderByClause = string.Join(",", _sqlQuery.Request.Sorts.ToList().Select(x => $"{AzureCosmosSqlQueryBuilder.Alias}.{x.Property} {(x.Direction == SortDirection.Ascending ? "ASC" : "DSC")}"));
            _sqlQuery.OrderByClause = $@" {orderByClause}";
            return _sqlQuery;
        }
    }
}
