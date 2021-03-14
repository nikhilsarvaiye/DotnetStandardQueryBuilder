namespace DotnetStandardQueryBuilder
{
    using DotnetStandardQueryBuilder.AzureCosmosSql;
    using DotnetStandardQueryBuilder.Core;
    using DotnetStandardQueryBuilder.Sql;
    using System;
    using System.Collections.Generic;

    public class AzureCosmosSqlQueryBuilder : IQueryBuilder<AzureCosmosSqlQueryBuilder, SqlQuery>
    {
        public const string Alias = "x";

        public IRequest Request { get; }

        public AzureCosmosSqlQueryBuilder(IRequest request)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public SqlQuery Query()
        {
            if (Request == null)
            {
                return null;
            }

            var sqlExpression = new SqlExpression(Request).Where().Select().OrderBy();

            return new SqlQuery
            {
                Query = $@" {sqlExpression.SelectClause}
						{(!string.IsNullOrEmpty(sqlExpression.WhereClause) ? $" WHERE {sqlExpression.WhereClause}" : string.Empty)}
						{(!string.IsNullOrEmpty(sqlExpression.OrderByClause) ? $" ORDER BY {sqlExpression.OrderByClause}" : string.Empty)}",
                Values = sqlExpression.Values
            };
        }

        public SqlQuery QueryCount()
        {
            if (Request == null)
            {
                return null;
            }

            Request.Select = new List<string>() { "COUNT(1)" };

            var sqlExpression = new SqlExpression(Request).Where().Select().OrderBy();

            return new SqlQuery
            {
                Query = $@" {sqlExpression.SelectClause}
						{(!string.IsNullOrEmpty(sqlExpression.WhereClause) ? $" WHERE {sqlExpression.WhereClause}" : string.Empty)}
						{(!string.IsNullOrEmpty(sqlExpression.OrderByClause) ? $" ORDER BY {sqlExpression.OrderByClause}" : string.Empty)}
						{sqlExpression.PageQuery}",
                Values = sqlExpression.Values
            };
        }
    }
}