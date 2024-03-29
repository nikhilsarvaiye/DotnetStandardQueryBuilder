﻿namespace DotnetStandardQueryBuilder
{
    using DotnetStandardQueryBuilder.Core;
    using DotnetStandardQueryBuilder.Sql;
    using DotnetStandardQueryBuilder.Sql.Extensions;
    using System;
    using System.Collections.Generic;

    public class SqlQueryBuilder : IQueryBuilder<SqlQueryBuilder, SqlQuery>
    {
        private string _tableName;

        public IRequest Request { get; }

        public SqlQueryBuilder(IRequest request, string tableName)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
            _tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        }

        public SqlQuery Query()
        {
            if (Request == null)
            {
                return null;
            }

            var sqlExpression = new SqlExpression(Request).Where().Select(_tableName).OrderBy().Paginate();

            return new SqlQuery
            {
                Query = $@" {sqlExpression.SelectClause}
						{(!string.IsNullOrEmpty(sqlExpression.WhereClause) ? $" WHERE {sqlExpression.WhereClause}" : string.Empty)}
						{(!string.IsNullOrEmpty(sqlExpression.OrderByClause) ? $" ORDER BY {sqlExpression.OrderByClause}" : string.Empty)}
						{sqlExpression.PageQuery}",
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

            var sqlExpression = new SqlExpression(Request).Where().Select(_tableName);

            return new SqlQuery
            {
                Query = $@" {sqlExpression.SelectClause}
						{(!string.IsNullOrEmpty(sqlExpression.WhereClause) ? $" WHERE {sqlExpression.WhereClause}" : string.Empty)}
						{(!string.IsNullOrEmpty(sqlExpression.OrderByClause) ? $" ORDER BY {sqlExpression.OrderByClause}" : string.Empty)}",
                Values = sqlExpression.Values
            };
        }

        public SqlQuery CreateQuery<T>(List<T> items, List<string> excludeColumns = null)
            where T : class
        {
            return new CreateBuilder(Request).Build(items, _tableName, excludeColumns);
        }

        public SqlQuery UpdateQuery<T>(List<T> items, List<string> excludeColumns = null)
            where T : class
        {
            return new UpdateBuilder(Request).Build(items, _tableName, excludeColumns);
        }

        public SqlQuery DeleteQuery()
        {
            return new DeleteBuilder(Request).Build(_tableName);
        }
    }
}