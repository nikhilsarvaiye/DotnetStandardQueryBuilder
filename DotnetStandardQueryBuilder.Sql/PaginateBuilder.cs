namespace DotnetStandardQueryBuilder.Sql
{
    using System;

    internal class PaginateBuilder
    {
        private readonly SqlExpression _sqlQuery;

        public PaginateBuilder(SqlExpression sqlQuery)
        {
            _sqlQuery = sqlQuery ?? throw new ArgumentNullException(nameof(sqlQuery));
        }

        public SqlExpression Build()
        {
            _sqlQuery.PageQuery += $@"{(_sqlQuery.Request.Page > 1 ? $" OFFSET {_sqlQuery.Request.Page} ROWS FETCH NEXT {_sqlQuery.Request.PageSize} ROWS ONLY" : string.Empty)}";
            return _sqlQuery;
        }
    }
}
